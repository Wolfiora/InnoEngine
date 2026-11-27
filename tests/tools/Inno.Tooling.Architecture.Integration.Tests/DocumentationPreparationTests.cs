using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Architecture.Integration.Tests;

public sealed class DocumentationPreparationTests : IDisposable
{
    private readonly string m_root = Directory.CreateTempSubdirectory("InnoDocumentationPreparation-").FullName;

    [Theory]
    [InlineData("Release")]
    [InlineData("SourceDebug")]
    public async Task ProjectAndTransitiveReferenceShareTheConfigurationAndInvocation(string configuration)
    {
        string sdk = ToolchainEnvironment.ResolveExecutable(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet");
        string solution = Path.Combine(m_root, "preparation.slnx");
        File.WriteAllText(Path.Combine(m_root, "Directory.Build.targets"), """
            <Project>
              <Target Name="RecordOwner" BeforeTargets="CoreCompile">
                <Error Condition="'$(InnoTaskBootstrapOwner)' == ''" Text="Missing invocation owner."/>
                <WriteLinesToFile File="$(MSBuildProjectDirectory)/owner.txt" Lines="$(Configuration)|$(InnoTaskBootstrapOwner)" Overwrite="true"/>
              </Target>
            </Project>
            """);
        foreach (string name in new[] { "Entry", "Dependency" })
        {
            string directory = Directory.CreateDirectory(Path.Combine(m_root, name)).FullName;
            var project = new XElement(
                "Project",
                new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"))
            );
            if (name == "Entry")
            {
                project.Add(
                    new XElement(
                        "ItemGroup",
                        new XElement("ProjectReference", new XAttribute("Include", "../Dependency/Dependency.csproj"))
                    )
                );
            }
            project.Save(Path.Combine(directory, name + ".csproj"));
            File.WriteAllText(Path.Combine(directory, name + ".cs"), "internal sealed class " + name + " {}");
        }
        Task<int> PrepareAsync(
            string project,
            string log
        ) => DocumentationReferences.ExecuteAsync([
            "prepare-references",
            m_root,
            sdk,
            configuration,
            Path.Combine(m_root, "out"),
            solution,
            Path.Combine(m_root, log),
            project
        ], CancellationToken.None);
        Assert.Equal(0, await PrepareAsync("Entry/Entry.csproj", "build.log"));
        string owner = File.ReadAllText(Path.Combine(m_root, "Entry/owner.txt")).Trim();
        Assert.Matches("^" + configuration + @"\|[a-f0-9]{32}$", owner);
        Assert.Equal(owner, File.ReadAllText(Path.Combine(m_root, "Dependency/owner.txt")).Trim());
        byte[] previous = File.ReadAllBytes(solution);
        Exception? rejected = await Record.ExceptionAsync(() => PrepareAsync("../outside.csproj", "invalid.log"));
        Assert.NotNull(rejected);
        Assert.Equal(previous, File.ReadAllBytes(solution));
    }

    [Theory]
    [InlineData("Inno.ScriptApi.Editor", "editor.dll", true)]
    [InlineData("Unrelated", "editor.dll", false)]
    [InlineData("Inno.ScriptApi.Runtime", "editor.dll", false)]
    [InlineData("Inno.ScriptApi.Editor", "missing.dll", false)]
    [InlineData("Inno.ScriptApi.Editor", null, false)]
    public async Task LogicalReferencesAreSelectedFromActualIdeItems(
        string editor,
        string? hint,
        bool valid
    ) {
        string project = Path.Combine(m_root, "Inno.EditorScripts.csproj");
        foreach (string file in new[] { "runtime.dll", "editor.dll" })
        {
            File.WriteAllText(Path.Combine(m_root, file), "selection fixture");
        }
        var xml = new XElement(
            "Project",
            new XElement("ItemGroup", new XElement("Compile", new XAttribute("Include", "Script.cs"))),
            new XElement(
                "ItemGroup",
                new XElement(
                    "Reference",
                    new XAttribute("Include", "Inno.ScriptApi.Runtime"),
                    new XElement("HintPath", "runtime.dll")
                ),
                new XElement("Reference", new XAttribute("Include", editor), hint == null ? null : new XElement("HintPath", hint))
            ),
            new XElement("ItemGroup", new XElement("None", new XAttribute("Include", "project.json")))
        );
        xml.Save(project);
        Task<int> selection = DocumentationReferences.ExecuteAsync(["script-references", project], CancellationToken.None);
        if (valid)
        {
            Assert.Equal(0, await selection);
        }
        else
        {
            Assert.NotNull(await Record.ExceptionAsync(() => selection));
        }
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
