using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inno.Native.LibraryLoading;

/// <summary>
/// Loads explicitly deployed native binaries without changing the application or consulting repository caches.
/// </summary>
public static class NativeDllLoader
{
    private static readonly string S_OUTPUT_ROOT = ResolveOutputRoot();
    private static readonly Lock RESOLVER_LOCK = new();
    private static readonly ConditionalWeakTable<Assembly, object> REGISTERED_RESOLVERS = new();

    /// <summary>
    /// Loads a native library from the output native folder and registers its binding owner's import resolver.
    /// </summary>
    /// <param name="libraryName">
    /// Library name without platform extension.
    /// </param>
    /// <param name="bindingAssembly">
    /// The explicit assembly containing imports for this library; stack inspection is never required.
    /// </param>
    /// <returns>
    /// A caller-owned handle. Transfer it to one native context or release it with NativeLibrary.Free.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The binding assembly is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The library name is empty, contains a path or contains a wildcard.
    /// </exception>
    /// <exception cref="DllNotFoundException">
    /// No library matching the current process target is deployed.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Multiple deployed files match the requested library and process target.
    /// </exception>
    public static IntPtr LoadNativeDll(
        string libraryName,
        Assembly bindingAssembly
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(libraryName);
        ArgumentNullException.ThrowIfNull(bindingAssembly);
        ValidateFileName(libraryName);
        RegisterResolverOnce(bindingAssembly);
        var candidateNames = GetLibraryFileNames(libraryName);
        foreach (var root in GetSearchRoots())
        {
            foreach (var name in candidateNames)
            {
                var match = FindPreferredTargetFile(root, name);
                if (match != null)
                {
                    return NativeLibrary.Load(match);
                }
            }
        }

        throw new DllNotFoundException($"Native library not found under native output: {libraryName}");
    }

    /// <summary>
    /// Finds a file under the output native folder.
    /// </summary>
    /// <param name="fileName">
    /// Exact file name to locate.
    /// </param>
    /// <returns>
    /// Full path to the file.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The file name is empty, contains a path or contains a wildcard.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// No file with this exact name is deployed for the current process target.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The deployment contains multiple matching files for the current target.
    /// </exception>
    public static string FindNativeFile(string fileName)
    {
        ValidateFileName(fileName);
        foreach (var root in GetSearchRoots())
        {
            var match = FindPreferredTargetFile(root, fileName);
            if (match != null)
            {
                return match;
            }
        }

        throw new FileNotFoundException($"Native file not found under search roots: {fileName}");
    }

    private static void RegisterResolverOnce(Assembly targetAssembly)
    {
        lock (RESOLVER_LOCK)
        {
            if (REGISTERED_RESOLVERS.TryGetValue(targetAssembly, out _))
            {
                return;
            }

            NativeLibrary.SetDllImportResolver(targetAssembly, ResolveNativeLibrary);
            REGISTERED_RESOLVERS.Add(targetAssembly, new object());
        }
    }

    private static IntPtr ResolveNativeLibrary(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath
    ) {
        var candidateNames = GetLibraryFileNames(libraryName);
        foreach (var root in GetSearchRoots())
        {
            foreach (var name in candidateNames)
            {
                var match = FindPreferredTargetFile(root, name);
                if (match != null)
                {
                    return NativeLibrary.Load(match);
                }
            }
        }

        return IntPtr.Zero;
    }

    private static IReadOnlyList<string> GetLibraryFileNames(string libraryName)
    {
        var names = new List<string>();
        if (libraryName.Contains('.'))
        {
            names.Add(libraryName);
        }

        if (OperatingSystem.IsWindows())
        {
            names.Add($"{libraryName}.dll");
            if (!libraryName.StartsWith("lib", StringComparison.OrdinalIgnoreCase))
            {
                names.Add($"lib{libraryName}.dll");
            }
            return names;
        }

        if (OperatingSystem.IsMacOS())
        {
            names.Add($"lib{libraryName}.dylib");
            names.Add($"{libraryName}.dylib");
            return names;
        }

        names.Add($"lib{libraryName}.so");
        names.Add($"{libraryName}.so");
        return names;
    }

    private static IEnumerable<string> GetSearchRoots()
    {
        var baseDir = S_OUTPUT_ROOT;
        var nativeRoot = Path.Combine(baseDir, NativeDllConstants.NATIVE_DIR_NAME);
        if (Directory.Exists(nativeRoot))
        {
            yield return nativeRoot;
        }
    }

    private static string? FindPreferredTargetFile(
        string root,
        string fileName
    ) {
        var candidates = Directory
            .EnumerateFiles(root, fileName, SearchOption.AllDirectories)
            .ToArray();
        if (candidates.Length == 0)
        {
            return null;
        }

        string targetSegment = $"/{GetTargetPlatformIdentifier()}/";
        string[] matching = candidates.Where(path =>
            NormalizePath(path).Contains(targetSegment, StringComparison.OrdinalIgnoreCase)).ToArray();
        return matching.Length switch
        {
            0 => null,
            1 => matching[0],
            _ => throw new InvalidDataException($"Native deployment contains multiple '{fileName}' files for '{GetTargetPlatformIdentifier()}'.")
        };
    }

    private static string GetTargetPlatformIdentifier()
    {
        string operatingSystem = OperatingSystem.IsMacOS()
            ? "macos"
            : OperatingSystem.IsWindows()
                ? "windows"
                : OperatingSystem.IsLinux()
                    ? "linux"
                    : throw new PlatformNotSupportedException("Native library loading supports Windows, macOS, and Linux.");
        string architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            Architecture.X86 => "x86",
            Architecture.Arm => "arm",
            _ => throw new PlatformNotSupportedException(
                $"Unsupported native architecture: {RuntimeInformation.ProcessArchitecture}.")
        };
        return $"{operatingSystem}-{architecture}";
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000",
        Justification = "Assembly-local outputs serve isolated hosts; an empty bundled assembly location explicitly selects the application directory.")]
    private static string ResolveOutputRoot()
    {
        string location = typeof(NativeDllLoader).Assembly.Location;
        return string.IsNullOrEmpty(location) ? AppContext.BaseDirectory : Path.GetDirectoryName(location)!;
    }

    private static string NormalizePath(string path) => path.Replace('\\', '/');

    private static void ValidateFileName(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        if (fileName is "." or ".." || fileName != Path.GetFileName(fileName)
            || fileName.IndexOfAny(['*', '?', '/', '\\']) >= 0)
            throw new ArgumentException("Native discovery requires an exact file name.", nameof(fileName));
    }

}
