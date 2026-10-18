using System.Collections.Generic;

namespace Inno.Build.Toolchains;

/// <summary>
/// Creates explicitly selected deployment metadata from a frozen Native closure.
/// </summary>
/// <param name="products">
/// Borrowed complete products whose leases remain held through metadata preparation.
/// </param>
/// <param name="files">
/// Exact relative deployment paths mapped to verified source files.
/// </param>
/// <returns>
/// Relative metadata paths mapped to immutable bytes; collisions with payloads are rejected.
/// </returns>
public delegate IReadOnlyDictionary<string, byte[]> NativeDeploymentMetadataFactory(
    IReadOnlyList<NativeBuildProduct> products,
    IReadOnlyDictionary<string, string> files
);
