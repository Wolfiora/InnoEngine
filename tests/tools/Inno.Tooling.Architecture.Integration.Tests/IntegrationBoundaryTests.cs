using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Inno.Tooling.Architecture.Integration.Tests;

[Collection("Architecture diagnostics")]
public sealed class IntegrationBoundaryTests
{
    [Theory]
    [InlineData("<PropertyGroup><IsTestSupportProject>true</IsTestSupportProject></PropertyGroup>")]
    [InlineData("<ItemGroup><PackageReference Include=\"xunit\" Version=\"2.6.6\" /></ItemGroup>")]
    public void ImportedTestDeclarationsCannotEscapeRootOwnership(string declaration)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoImportedTestOwnership", Guid.NewGuid().ToString("N"));
        try
        {
            string owner = Path.Combine(root, "backends", "Fixture", "Probe");
            Directory.CreateDirectory(owner);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            File.WriteAllText(Path.Combine(root, "TestSupport.props"), "<Project>" + declaration + "</Project>");
            File.WriteAllText(
                Path.Combine(owner, "Probe.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><Import Project=\"../../../TestSupport.props\" />"
                + "<PropertyGroup><TargetFramework>net10.0</TargetFramework><ImplicitUsings>disable</ImplicitUsings></PropertyGroup></Project>"
            );
            string host = Path.GetFullPath(
                Path.Combine(
                    RuntimeEnvironment.GetRuntimeDirectory(),
                    "..",
                    "..",
                    "..",
                    OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"
                )
            );
            (_, string report) = ArchitectureCommandFixture.Run(
                root,
                "--source-only",
                "--dotnet",
                host,
                "--artifacts-path",
                Path.Combine(root, "artifacts")
            );
            Assert.Contains("effective test projects must belong to the root tests directory", report, StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Theory]
    [InlineData("Probe.Tests", "domain", false, "")]
    [InlineData("Probe.Integration.Tests", "integration", false, "")]
    [InlineData("Probe.Native.Tests", "native", true, "")]
    [InlineData("Inno.Native.Probe.Tests", "native", true, "")]
    [InlineData("Probe.Tests", "integration", false, "test project name and execution layer")]
    [InlineData("Probe.Tests", "contract", true, "a test that deploys a native product")]
    [InlineData("Orphan.Tests", "domain", false, "test project must be named after the project it tests")]
    [InlineData("Probe.Checks", "domain", false, "test project name must end with .Tests")]
    public void TestProjectNamesFollowTheirSubjectAndExecutionLayer(
        string name,
        string layer,
        bool nativeProduct,
        string rejection
    ) => AssertTestProjectRules(
        rejection,
        root => WriteProject(
            root,
            "tests/core",
            name,
            "<IsTestProject>true</IsTestProject><InnoValidationLayer>" + layer + "</InnoValidationLayer>"
            + (nativeProduct ? "<InnoTestNativeProduct>editor</InnoTestNativeProduct>" : string.Empty)
        )
    );

    [Theory]
    [InlineData("Probe.Tests", "tests/core", "")]
    [InlineData("Probe.Tests", "tests/services", "test project must live in tests/core")]
    [InlineData("Inno.Build.Products.Tests", "tests/build/pipeline", "")]
    [InlineData("Inno.Build.Products.Tests", "tests/core", "test project must live in tests/build/pipeline")]
    public void TestProjectsLiveInTheTestDirectoryOfTheirSubject(
        string name,
        string directory,
        string rejection
    ) => AssertTestProjectRules(
        rejection,
        root => WriteProject(
            root,
            directory,
            name,
            "<IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer>"
        )
    );

    [Theory]
    [InlineData("tests/support", "Inno.Testing.Probe", "tests/services", "Consumer.Tests", "")]
    [InlineData("tests/core/fixtures", "Inno.Testing.Probe", "tests/core", "Probe.Tests", "")]
    [InlineData("tests/support", "Probe.Helpers", "tests/core", "Probe.Tests", "shared test support in tests/support must be named")]
    [InlineData("tests/core", "Inno.Testing.Probe", "tests/core", "Probe.Tests", "test support belongs to tests/support")]
    [InlineData(
        "tests/core/fixtures",
        "Inno.Testing.Probe",
        "tests/services",
        "Consumer.Tests",
        "is shared by several test directories"
    )]
    public void TestSupportLivesWithItsOwnerOrInSharedSupport(
        string directory,
        string name,
        string consumerDirectory,
        string consumer,
        string rejection
    ) => AssertTestProjectRules(
        rejection,
        root =>
        {
            WriteProject(root, directory, name, "<IsTestSupportProject>true</IsTestSupportProject>");
            WriteProject(
                root,
                consumerDirectory,
                consumer,
                "<IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer>",
                "<ItemGroup><ProjectReference Include=\"" + Path.Combine(root, directory, name, name + ".csproj") + "\" /></ItemGroup>"
            );
        }
    );

    [Theory]
    [InlineData("backends/ImGui/tests/Probe", "<IsTestProject>true</IsTestProject>", "", true)]
    [InlineData("custom/Probe", "<IsTestSupportProject>true</IsTestSupportProject>", "", true)]
    [InlineData("dependencies/Probe", "<IsTestProject>true</IsTestProject>", "", true)]
    [InlineData("backends/ImGui/Probe", "", "<PackageReference Include=\"xunit\" Version=\"2.6.6\" />", true)]
    [InlineData("tests/backends/ImGui/Probe", "<IsTestProject>true</IsTestProject>", "", false)]
    [InlineData("extern/Upstream/tests/Probe", "<IsTestProject>true</IsTestProject>", "", false)]
    public void RepositoryTestOwnershipCoversDeclarationsPackagesAndUnknownRoots(
        string owner,
        string properties,
        string items,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoTestOwnershipTests", Guid.NewGuid().ToString("N"));
        try
        {
            string directory = Path.Combine(root, owner);
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            File.WriteAllText(
                Path.Combine(directory, "Probe.csproj"),
                "<Project><PropertyGroup>" + properties + "</PropertyGroup><ItemGroup>" + items + "</ItemGroup></Project>"
            );
            (_, string report) = ArchitectureCommandFixture.Run(root);
            Assert.Equal(rejected, report.Contains("engine-owned tests and test support must belong", StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Theory]
    [InlineData("src/services/Probe", "tests/core/Probe.Tests", "production code cannot reference test support", true)]
    [InlineData("tests/build/Probe.Tests", "tests/build/fixtures/Probe.Support", "production code cannot reference test support", false)]
    [InlineData("platforms/WindowsX64/build/Probe", "backends/Bgfx/build/ProbeBackend", "platform base modules cannot select", true)]
    [InlineData(
        "platforms/WindowsX64/integrations/Probe",
        "backends/Bgfx/build/ProbeBackend",
        "platform base modules cannot select",
        false
    )]
    [InlineData("backends/Bgfx/build/ProbeBackend", "platforms/WindowsX64/integrations/Probe", "reusable backends cannot depend", true)]
    [InlineData(
        "platforms/BrowserWasm32/integrations/Probe",
        "platforms/WindowsX64/integrations/ProbeWindows",
        "one concrete platform cannot depend",
        true
    )]
    [InlineData(
        "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime",
        "backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx",
        "runtime surface integrations cannot depend",
        false
    )]
    [InlineData(
        "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime",
        "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx",
        "runtime surface integrations cannot depend",
        true
    )]
    [InlineData(
        "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime",
        "backends/Bgfx/native/Inno.Native.Bgfx",
        "runtime surface integrations cannot depend",
        true
    )]
    [InlineData("build/toolchains/Probe", "build/bindings/Inno.Build.Bindings", "neutral toolchains cannot depend", true)]
    [InlineData("build/bindings/Inno.Build.Bindings", "build/tasks/Inno.Build.Tasks", "binding generation cannot depend", true)]
    [InlineData("platforms/WindowsX64/build/Probe", "build/bindings/Inno.Build.Bindings", "platform modules must borrow", true)]
    [InlineData(
        "src/composition/editor/features/Inno.Editor.TestFeature",
        "src/composition/editor/presentation/Inno.Editor.ImGui",
        "Editor feature editing must be presentation-independent",
        true
    )]
    [InlineData(
        "src/composition/editor/features/Inno.Editor.TestFeature",
        "src/composition/editor/framework/Inno.Editor.Interactions",
        "Editor feature editing must be presentation-independent",
        false
    )]
    [InlineData(
        "src/composition/editor/panels/Inno.Editor.Panel.A",
        "src/composition/editor/panels/Inno.Editor.Panel.B",
        "Panels cannot depend on other Panels",
        true
    )]
    [InlineData(
        "src/composition/editor/panels/Inno.Editor.Panel.A",
        "src/composition/editor/presentation/Inno.Editor.Assets.ImGui",
        "Panels cannot depend on other Panels",
        false
    )]
    [InlineData(
        "src/composition/editor/panels/Inno.Editor.Panel.A",
        "backends/ImGui/native/Inno.Native.ImGui",
        "native UI access belongs",
        true
    )]
    [InlineData(
        "src/composition/editor/presentation/Inno.Editor.Graph.ImGui",
        "src/composition/editor/features/Inno.Editor.Shaders",
        "reusable Graph mechanisms cannot depend",
        true
    )]
    public void CliChecksActualPlatformIntegrationProjectReferences(
        string owner,
        string dependency,
        string expected,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoIntegrationBoundaryTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string destination = CreateProject(dependency, string.Empty);
            CreateProject(
                owner,
                "<ItemGroup><ProjectReference Include=\""
                + Path.GetRelativePath(Path.Combine(root, owner), destination).Replace('\\', '/')
                + "\" /></ItemGroup>"
            );
            (_, string report) = ArchitectureCommandFixture.Run(root);
            Assert.Equal(rejected, report.Contains(expected, StringComparison.Ordinal));
            Assert.DoesNotContain("explicit project dependency is missing", report);
            string CreateProject(
                string relative,
                string references
            ) {
                string directory = Path.Combine(root, relative);
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, Path.GetFileName(directory) + ".csproj");
                File.WriteAllText(
                    path,
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>" + references + "</Project>"
                );
                return path;
            }
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ScriptSdkReferencesPanelContractsOnlyWhenThePanelActuallyExportsThem(bool exportsContract)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoSdkOwnerTests", Guid.NewGuid().ToString("N"));
        try
        {
            string host = Path.Combine(root, "build/cli/Inno.Build.Cli");
            string panel = Path.Combine(root, "src/composition/editor/panels/Inno.Editor.Panel.Probe");
            Directory.CreateDirectory(host);
            Directory.CreateDirectory(Path.Combine(panel, "Properties"));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string panelProject = Path.Combine(panel, "Inno.Editor.Panel.Probe.csproj");
            File.WriteAllText(
                panelProject,
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>"
            );
            string include = Path.GetRelativePath(host, panelProject).Replace('\\', '/');
            File.WriteAllText(
                Path.Combine(host, "Inno.Build.Cli.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup><ItemGroup><ProjectReference Include=\""
                + include + "\" /></ItemGroup></Project>"
            );
            const string C_EXPORT_DECLARATION = "[assembly: Inno.Scripting.Api.ScriptingApiExport(typeof(System.String), Inno.Scripting.Api.ScriptingApiScope.Editor)]";
            File.WriteAllText(
                Path.Combine(panel, "Properties/ScriptingApi.cs"),
                exportsContract ? C_EXPORT_DECLARATION : "// " + C_EXPORT_DECLARATION
            );
            (_, string report) = ArchitectureCommandFixture.Run(root);
            Assert.Equal(!exportsContract, report.Contains("script SDK host must reference API owners", StringComparison.Ordinal));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private static void AssertTestProjectRules(
        string rejection,
        Action<string> arrange
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoTestIdentity", Guid.NewGuid().ToString("N"));
        try
        {
            File.WriteAllText(
                Path.Combine(Directory.CreateDirectory(root).FullName, "InnoEngine.sln"),
                "Microsoft Visual Studio Solution File, Format Version 12.00"
            );
            WriteProject(root, "src/core", "Probe", string.Empty);
            WriteProject(root, "src/core", "Inno.Native.Probe", string.Empty);
            WriteProject(root, "src/services", "Consumer", string.Empty);
            arrange(root);
            string host = Path.GetFullPath(
                Path.Combine(
                    RuntimeEnvironment.GetRuntimeDirectory(),
                    "..",
                    "..",
                    "..",
                    OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"
                )
            );
            (_, string report) = ArchitectureCommandFixture.Run(
                root,
                "--source-only",
                "--dotnet",
                host,
                "--artifacts-path",
                Path.Combine(root, "artifacts")
            );
            string[] rules =
            [
                "test project name must end with .Tests",
                "test project name and execution layer",
                "a test that deploys a native product",
                "test project must be named after the project it tests",
                "test project must live in",
                "shared test support in tests/support must be named",
                "test support belongs to tests/support",
                "is shared by several test directories"
            ];
            Assert.Equal(
                rules.Where(rule => rejection.StartsWith(rule, StringComparison.Ordinal)),
                rules.Where(rule => report.Contains(rule, StringComparison.Ordinal))
            );
            Assert.Contains(rejection, report, StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private static void WriteProject(
        string root,
        string directory,
        string name,
        string properties,
        string items = ""
    ) {
        string owner = Directory.CreateDirectory(Path.Combine(root, directory, name)).FullName;
        File.WriteAllText(
            Path.Combine(owner, name + ".csproj"),
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
            + "<ImplicitUsings>disable</ImplicitUsings>" + properties + "</PropertyGroup>" + items + "</Project>"
        );
    }
}
