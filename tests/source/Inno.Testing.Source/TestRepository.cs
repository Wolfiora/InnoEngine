using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Inno.Testing.Source;

/// <summary>
/// Resolves source and SDK output inputs explicitly declared by a test assembly's project.
/// </summary>
public static class TestRepository
{
    private const string C_ROOT_METADATA = "Inno.Testing.RepositoryRoot";
    private const string C_ARTIFACTS_METADATA = "Inno.Testing.ArtifactsPath";

    /// <summary>
    /// Reads the SDK output owner. Assemblies inside the executor's declared output owner use its process binding
    /// when retained outputs move to another runner; independent fixture assemblies retain their own declarations.
    /// </summary>
    /// <param name="testAssembly">
    /// The test assembly carrying the optional evaluated artifacts path.
    /// </param>
    /// <returns>
    /// The declared absolute artifacts directory, or null when normal SDK output paths were selected.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The test assembly is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The declaration is duplicated, empty or relative.
    /// </exception>
    public static string? GetArtifactsPath(Assembly testAssembly)
    {
        ArgumentNullException.ThrowIfNull(testAssembly);
        AssemblyMetadataAttribute? declaration = testAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .SingleOrDefault(static attribute => attribute.Key == C_ARTIFACTS_METADATA);
        if (declaration is null)
        {
            return null;
        }
        string? directory = declaration.Value;
        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory))
        {
            throw new InvalidOperationException("Tests require the absolute SDK artifacts owner recorded by their project.");
        }
        directory = GetProcessBinding(testAssembly, "INNO_TESTING_ARTIFACTS_PATH") ?? directory;
        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory))
        {
            throw new InvalidOperationException("Tests require the absolute SDK artifacts owner recorded by their project.");
        }
        return Path.GetFullPath(directory);
    }

    /// <summary>
    /// Reads and validates the checkout input independently of the test assembly's output directory.
    /// The isolated executor rebinds only assemblies inside its declared output owner when authenticated outputs move
    /// between runners. Independent fixture assemblies continue to resolve their own declared checkouts.
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
        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory))
        {
            throw new InvalidOperationException("Tests require the source checkout declared by their SDK project.");
        }
        directory = GetProcessBinding(testAssembly, "INNO_TESTING_REPOSITORY_ROOT") ?? directory;
        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory)
            || !File.Exists(Path.Combine(directory, "InnoEngine.sln"))
        ) {
            throw new InvalidOperationException("Tests require the source checkout declared by their SDK project.");
        }

        return Path.GetFullPath(directory);
    }

    private static string? GetProcessBinding(
        Assembly testAssembly,
        string variable
    ) {
        string? owner = Environment.GetEnvironmentVariable("INNO_TESTING_ARTIFACTS_PATH");
        if (owner is null || testAssembly.IsDynamic || string.IsNullOrWhiteSpace(testAssembly.Location))
        {
            return null;
        }
        if (string.IsNullOrWhiteSpace(owner) || !Path.IsPathFullyQualified(owner))
        {
            throw new InvalidOperationException("The test process must declare an absolute output owner before rebinding inputs.");
        }
        string prefix = Path.TrimEndingDirectorySeparator(Path.GetFullPath(owner)) + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(testAssembly.Location).StartsWith(prefix, StringComparison.Ordinal))
        {
            return null;
        }
        return Environment.GetEnvironmentVariable(variable);
    }
}
