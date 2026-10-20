using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Inno.Testing.Source;

/// <summary>
/// Resolves a source checkout explicitly declared by a test assembly's SDK project.
/// </summary>
public static class TestRepository
{
    private const string C_ROOT_METADATA = "Inno.Testing.RepositoryRoot";

    /// <summary>
    /// Reads and validates the checkout input independently of the test assembly's output directory.
    /// </summary>
    /// <param name="testAssembly">
    /// The test assembly carrying exactly one absolute repository-root metadata declaration.
    /// </param>
    /// <returns>
    /// The declared absolute checkout directory containing the engine solution marker.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The test assembly is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The declaration is missing, duplicated, relative, or does not identify an available checkout.
    /// </exception>
    public static string GetRoot(Assembly testAssembly)
    {
        ArgumentNullException.ThrowIfNull(testAssembly);
        string? directory = testAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .SingleOrDefault(static attribute => attribute.Key == C_ROOT_METADATA)?.Value;
        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory)
            || !File.Exists(Path.Combine(directory, "InnoEngine.sln")))
        {
            throw new InvalidOperationException("Tests require the source checkout declared by their SDK project.");
        }

        return Path.GetFullPath(directory);
    }
}
