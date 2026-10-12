using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
    /// The selected native library cannot be loaded by the current process.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The product's explicit native deployment index is malformed.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The product index, exact logical library entry or its payload is absent.
    /// </exception>
    public static IntPtr LoadNativeDll(
        string libraryName,
        Assembly bindingAssembly
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(libraryName);
        ArgumentNullException.ThrowIfNull(bindingAssembly);
        ValidateFileName(libraryName);
        RegisterResolverOnce(bindingAssembly);
        string nativeRoot = Path.Combine(S_OUTPUT_ROOT, NativeDllConstants.NATIVE_DIR_NAME);
        NativeDeploymentIndex index = ReadIndex(nativeRoot);
        return NativeLibrary.Load(index.ResolveLibrary(nativeRoot, libraryName));
    }

    /// <summary>
    /// Resolves an exact tool or resource alias from the product's explicit native deployment index.
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
    /// The index, alias or selected payload is absent.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The product's explicit deployment index is malformed.
    /// </exception>
    public static string FindNativeFile(string fileName)
    {
        ValidateFileName(fileName);
        string nativeRoot = Path.Combine(S_OUTPUT_ROOT, NativeDllConstants.NATIVE_DIR_NAME);
        return ReadIndex(nativeRoot).ResolveFile(nativeRoot, fileName);
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
        string nativeRoot = Path.Combine(S_OUTPUT_ROOT, NativeDllConstants.NATIVE_DIR_NAME);
        NativeDeploymentIndex index = ReadIndex(nativeRoot);
        return index.libraries.ContainsKey(libraryName)
            ? NativeLibrary.Load(index.ResolveLibrary(nativeRoot, libraryName)) : IntPtr.Zero;
    }

    private static NativeDeploymentIndex ReadIndex(string nativeRoot)
    {
        string path = Path.Combine(nativeRoot, NativeDeploymentIndex.C_FILE_NAME);
        if (!File.Exists(path))
            throw new FileNotFoundException("The product has no explicit native deployment index. Prepare the product before starting it.", path);
        return NativeDeploymentIndex.Decode(File.ReadAllBytes(path));
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000",
        Justification = "Assembly-local outputs serve isolated hosts; an empty bundled assembly location explicitly selects the application directory.")]
    private static string ResolveOutputRoot()
    {
        string location = typeof(NativeDllLoader).Assembly.Location;
        return string.IsNullOrEmpty(location) ? AppContext.BaseDirectory : Path.GetDirectoryName(location)!;
    }

    private static void ValidateFileName(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        if (fileName is "." or ".." || fileName != Path.GetFileName(fileName)
            || fileName.IndexOfAny(['*', '?', '/', '\\']) >= 0)
            throw new ArgumentException("Native discovery requires an exact file name.", nameof(fileName));
    }

}
