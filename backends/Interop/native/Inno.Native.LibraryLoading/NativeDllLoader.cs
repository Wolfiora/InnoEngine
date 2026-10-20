using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Inno.Native.LibraryLoading;

/// <summary>
/// Resolves explicitly deployed native binaries and files without consulting repository caches.
/// </summary>
public static class NativeDllLoader
{
    private static readonly string S_OUTPUT_ROOT = ResolveOutputRoot();

    /// <summary>
    /// Resolves one logical module from the product's explicit deployment index without loading it.
    /// </summary>
    /// <param name="libraryName">
    /// The exact logical library identity declared by the selected product.
    /// </param>
    /// <returns>
    /// The validated absolute library path; ownership is acquired by the consumer's module provider.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The identity is empty or contains a path or wildcard.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The product index, library declaration or payload is missing.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The deployment index is malformed.
    /// </exception>
    public static string FindNativeLibrary(string libraryName)
    {
        ValidateFileName(libraryName, nameof(libraryName));
        string nativeRoot = Path.Combine(S_OUTPUT_ROOT, NativeDllConstants.NATIVE_DIR_NAME);
        return ReadIndex(nativeRoot).ResolveLibrary(nativeRoot, libraryName);
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
        ValidateFileName(fileName, nameof(fileName));
        string nativeRoot = Path.Combine(S_OUTPUT_ROOT, NativeDllConstants.NATIVE_DIR_NAME);
        return ReadIndex(nativeRoot).ResolveFile(nativeRoot, fileName);
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

    private static void ValidateFileName(
        string fileName,
        string parameterName
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName, parameterName);
        if (fileName is "." or ".." || fileName != Path.GetFileName(fileName)
            || fileName.IndexOfAny(['*', '?', '/', '\\']) >= 0)
            throw new ArgumentException("Native discovery requires an exact file name.", parameterName);
    }

}
