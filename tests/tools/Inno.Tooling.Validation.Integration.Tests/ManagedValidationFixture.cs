using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Testing.Source;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

internal static class ManagedValidationFixture
{
    internal const string C_TEST = "tests/services/input/Inno.Input.Tests/Inno.Input.Tests.csproj";
    private const string C_LOGGER = "tests/support/Inno.Testing.Discovery.TestLogger/Inno.Testing.Discovery.TestLogger.csproj";

    internal static async Task<ValidationRepository> CreateAsync()
    {
        var repository = new ValidationRepository(initialize: Initialize);
        try
        {
            string[] projects = Directory.GetFiles(repository.PathOf("."), "*.csproj", SearchOption.AllDirectories);
            await repository.SdkAsync(["sln", "InnoEngine.sln", "add", .. projects.Order(StringComparer.Ordinal)]);
            repository.Git("add", "InnoEngine.sln");
            repository.Git("commit", "-m", "Register the SDK test owners");
            return repository;
        }
        catch
        {
            repository.Dispose();
            throw;
        }
    }

    private static void Initialize(ValidationRepository repository)
    {
        string source = TestRepository.GetRoot(typeof(ManagedValidationFixture).Assembly);
        foreach (string path in new[]
        {
            "tools/Inno.Tooling.Validation/ValidationTestExecution.cs",
            C_LOGGER,
            "tests/support/Inno.Testing.Discovery.TestLogger/DiscoveryLogger.cs"
        }) {
            repository.Write(path, File.ReadAllText(Path.Combine(source, path)));
        }
        repository.Write("AGENTS.md", "# Fixture policy passed\n");
        repository.Write("README.md", "# SDK fixture\n");
        XElement rules = XElement.Load(Path.Combine(source, "Directory.Build.targets"));
        string[] compilationTargets = ["ApplyInnoCleanReferencePolicy", "UseInnoVerifiedProjectOutputs"];
        repository.Write(
            "Directory.Build.targets",
            new XElement(
                "Project",
                compilationTargets.Select(name =>
                    rules.Elements("Target").Single(target => (string?)target.Attribute("Name") == name))
            ).ToString()
        );
        repository.Write("src/foundation/core/Inno.Core.IO/Inno.Core.IO.csproj", ValidationRepository.Project());
        repository.Write(
            "src/services/input/Inno.Input/Inno.Input.csproj",
            ValidationRepository.Project("""
            <ItemGroup><ProjectReference Include="../../../foundation/core/Inno.Core.IO/Inno.Core.IO.csproj" /></ItemGroup>
            """)
        );
        repository.Write(
            "src/foundation/extensibility/Inno.Extensibility.Catalogs/Inno.Extensibility.Catalogs.csproj",
            ValidationRepository.Project()
        );
        // The repository architecture check requires the two product composition boundaries.
        const string C_SHELL = "src/composition/shell/Inno.Shell/Inno.Shell.csproj";
        repository.Write(C_SHELL, ValidationRepository.Project());
        foreach (string host in new[] {
            "src/composition/editor/hosting/Inno.Editor.Hosting/Inno.Editor.Hosting.csproj",
            "src/composition/player/Inno.Player.Runtime/Inno.Player.Runtime.csproj"
        }) {
            string shell = Path.GetRelativePath(Path.GetDirectoryName(host)!, C_SHELL).Replace('\\', '/');
            repository.Write(host, ValidationRepository.Project($"<ItemGroup><ProjectReference Include=\"{shell}\" /></ItemGroup>"));
        }
        repository.Write(
            C_TEST,
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup>
              <ProjectReference Include="../../../../src/services/input/Inno.Input/Inno.Input.csproj" />
              <ProjectReference Include="../../../support/Inno.Testing.Discovery.TestLogger/Inno.Testing.Discovery.TestLogger.csproj" ReferenceOutputAssembly="false" />
              <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
              <PackageReference Include="xunit" Version="2.6.6" />
              <PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
            </ItemGroup>
            """)
        );
    }
}
