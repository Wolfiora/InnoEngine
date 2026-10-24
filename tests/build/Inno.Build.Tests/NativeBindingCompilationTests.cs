using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Testing.Source;

using Inno.Build.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

[Trait("ExternalDependency", "BGCS")]
public sealed class NativeBindingCompilationTests
{
    [Fact]
    public async Task TargetCompilationReplacesHostSourceAndRejectsAMissingSelection()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBindingCompilation", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "Generated"));
        Directory.CreateDirectory(Path.Combine(root, "Source"));
        try
        {
            File.WriteAllText(Path.Combine(root, "Generated", "Bindings.cs"), "#error Host bindings must not be compiled for this target.");
            File.WriteAllText(Path.Combine(root, "Source", "api.h"), "int sample(int value);");
            string config = Path.Combine(root, "bindgen.json");
            File.WriteAllText(config, JsonSerializer.Serialize(new
            {
                schemaVersion = 2,
                library = new { id = "fixture", nativeName = "fixture" },
                source = new { language = "c", headers = new[] { "Source/api.h" } },
                api = new { @namespace = "Fixture.Native", name = "Api" },
                output = new { root = "Generated", layout = "singleFile" }
            }));
            string engine = TestRepository.GetRoot(typeof(NativeBindingCompilationTests).Assembly);
            string profile = "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Build/Bindings/WindowsX64BindingProfile.cs";
            string profileInput = Path.Combine(root, profile);
            Directory.CreateDirectory(Path.GetDirectoryName(profileInput)!);
            File.Copy(Path.Combine(engine, profile), profileInput);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            string definition = Path.Combine(root, "bindings.props");
            new XDocument(new XElement("Project",
                new XElement("PropertyGroup", new XElement("InnoNativeComponentId", "fixture"), new XElement("InnoBindingConfig", "bindgen.json"))))
                .Save(definition);
            string sources = Path.Combine(root, "compile-sources.txt");
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "net10.0"),
                    new XElement("LangVersion", "14.0"),
                    new XElement("AllowUnsafeBlocks", "true"),
                    new XElement("ImplicitUsings", "disable"),
                    new XElement("EnableDefaultCompileItems", "false"),
                    new XElement("Nullable", "enable"),
                    new XElement("IsTestProject", "true"),
                    new XElement("BindGenGeneratedBindings", "true"),
                    new XElement("InnoBindingTargetId", "windows-x64"),
                    new XElement("InnoBindingEngineRoot", root),
                    new XElement("InnoBindingDefinition", definition),
                    new XElement("InnoBindingOutputMode", "TargetArtifacts")),
                new XElement("Import", new XAttribute("Project", definition)),
                new XElement("Import", new XAttribute("Project", Path.Combine(engine, "Directory.Build.targets"))),
                new XElement("ItemGroup", new XElement("Reference", new XAttribute("Include", "BGCS.Runtime"),
                    new XElement("HintPath", typeof(BGCS.Runtime.NativeModuleSession).Assembly.Location))),
                new XElement("Target", new XAttribute("Name", "CaptureCompileSources"),
                    new XAttribute("AfterTargets", "ValidateBindGenBindings"),
                    new XElement("WriteLinesToFile", new XAttribute("File", sources),
                        new XAttribute("Lines", "@(Compile->'%(FullPath)')"), new XAttribute("Overwrite", "true")))))
                .Save(project);

            (int success, string output) = await Build(project, []);
            Assert.True(success == 0, output);
            Assert.Equal(1, output.Split("INNO-TASK-HOST verified", StringSplitOptions.None).Length - 1);
            string selected = Assert.Single(File.ReadAllLines(sources)
                .Where(static path => Path.GetFileName(path) == "Bindings.cs"));
            Assert.Equal(Path.Combine(root, "obj", "Release", "net10.0", "bindings", "Bindings.cs"), selected);
            Assert.True(File.Exists(selected), output);
            string artifact = Assert.Single(Directory.GetDirectories(Path.Combine(root, "artifacts", "bindings", "fixture", "windows-x64")));
            string publishedSource = Path.Combine(artifact, "Generated", "Bindings.cs");
            Assert.Equal(File.ReadAllBytes(publishedSource), File.ReadAllBytes(selected));
            Assert.True(File.Exists(Path.Combine(root, "bin", "Release", "net10.0", "Fixture.dll")), output);

            string selection = Path.Combine(root, "SelectedBindings.props");
            NativeBindingGenerationDescriptor.WriteSelection(selection,
                new Dictionary<string, NativeBindingGenerationDescriptor>
                {
                    [project] = new()
                    {
                        fingerprint = Path.GetFileName(artifact),
                        artifactDirectory = artifact,
                        artifactOwnerDirectory = Path.Combine(root, "artifacts", "bindings"),
                        bindingsPath = publishedSource,
                        nativeDirectory = string.Empty
                    }
                }, new Dictionary<string, NativeLibraryKind> { [project] = NativeLibraryKind.Shared });
            (int reused, string reuseOutput) = await Build(project, ["-p:InnoNativeBindingSelection=" + selection]);
            Assert.True(reused == 0, reuseOutput);
            Assert.Equal(selected, Assert.Single(File.ReadAllLines(sources)
                .Where(static path => Path.GetFileName(path) == "Bindings.cs")));
            Assert.DoesNotContain("INNO-BINDINGS-PREPARE", reuseOutput);
            Assert.DoesNotContain("INNO-TASK-RUNTIME", reuseOutput);

            string frozen = Path.Combine(root, "FrozenSources");
            Directory.CreateDirectory(frozen);
            string frozenProject = Path.Combine(frozen, "Fixture.csproj");
            XDocument frozenDefinition = XDocument.Load(project);
            frozenDefinition.Root!.Element("PropertyGroup")!.Element("InnoBindingEngineRoot")!.Value = frozen;
            frozenDefinition.Save(frozenProject);
            (int frozenExit, string frozenOutput) = await Build(frozenProject, ["-p:InnoNativeBindingSelection=" + selection]);
            Assert.True(frozenExit == 0, frozenOutput);
            Assert.DoesNotContain("INNO-BINDINGS-PREPARE", frozenOutput);
            Assert.True(File.Exists(Path.Combine(frozen, "obj", "Release", "net10.0", "bindings", "Bindings.cs")));

            string staticSelection = Path.Combine(root, "StaticBindings.props");
            NativeBindingGenerationDescriptor.WriteSelection(staticSelection,
                new Dictionary<string, NativeBindingGenerationDescriptor>
                {
                    [project] = new()
                    {
                        fingerprint = Path.GetFileName(artifact),
                        artifactDirectory = artifact,
                        artifactOwnerDirectory = Path.Combine(root, "artifacts", "bindings"),
                        bindingsPath = publishedSource,
                        nativeDirectory = string.Empty
                    }
                }, new Dictionary<string, NativeLibraryKind> { [project] = NativeLibraryKind.Static });
            string bootstrap = Path.Combine(root, "Bootstrap.cs");
            File.WriteAllText(bootstrap, "#if !INNO_STATIC_NATIVE\n#error The selected static component must omit dynamic initialization.\n#endif");
            XDocument staticProject = XDocument.Load(project);
            staticProject.Root!.Add(new XElement("ItemGroup", new XElement("Compile", new XAttribute("Include", bootstrap))));
            staticProject.Save(project);
            (int staticExit, string staticOutput) = await Build(project, ["-p:InnoNativeBindingSelection=" + staticSelection]);
            Assert.True(staticExit == 0, staticOutput);
            staticProject.Root!.Elements("ItemGroup").Last().Remove();
            staticProject.Save(project);

            await VerifyTransitiveSelection(root, engine, project, selection, sources, selected);

            (int rejected, string failure) = await Build(project,
                ["-p:InnoNativeBindingSelection=" + Path.Combine(root, "absent-selection.props")]);
            Assert.NotEqual(0, rejected);
            Assert.Contains("The selected native binding manifest is missing", failure);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    private static async Task VerifyTransitiveSelection(
        string root,
        string engine,
        string nativeProject,
        string selection,
        string sources,
        string selected
    ) {
        string middle = Path.Combine(root, "Consumers", "Middle", "Middle.csproj");
        string host = Path.Combine(root, "Consumers", "Host", "Host.csproj");
        foreach (var entry in new[] { (path: middle, reference: nativeProject), (path: host, reference: middle) })
        {
            Directory.CreateDirectory(Path.GetDirectoryName(entry.path)!);
            var document = new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"),
                    new XElement("IsTestProject", "true"), new XElement("EnableDefaultCompileItems", "false"),
                    new XElement("InnoProductReferenceProperties", "FixtureDeclared=true")),
                new XElement("Import", new XAttribute("Project", Path.Combine(engine, "Directory.Build.targets"))),
                new XElement("ItemGroup", new XElement("ProjectReference", new XAttribute("Include", entry.reference))));
            if (entry.path == host)
                document.Add(new XElement("Target", new XAttribute("Name", "SelectOperationBindings"),
                    new XAttribute("BeforeTargets", "AssignProjectConfiguration"),
                    new XElement("PropertyGroup", new XElement("InnoNativeBindingSelection", selection)),
                    new XElement("ItemGroup", new XElement("ProjectReference",
                        new XElement("AdditionalProperties", "InnoNativeBindingSelection=$(InnoNativeBindingSelection)")))));
            new XDocument(document).Save(entry.path);
        }
        (int exit, string output) = await Build(host, []);
        Assert.True(exit == 0, output);
        Assert.DoesNotContain("INNO-BINDINGS-PREPARE", output);
        Assert.DoesNotContain("INNO-TASK-RUNTIME", output);
        Assert.Equal(selected, Assert.Single(File.ReadAllLines(sources)
            .Where(static path => Path.GetFileName(path) == "Bindings.cs")));
    }

    private static async Task<(int exitCode, string output)> Build(
        string project,
        string[] arguments
    ) {
        string host = Path.Combine(Path.GetFullPath("../../..", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(host)
        {
            WorkingDirectory = TestRepository.GetRoot(typeof(NativeBindingCompilationTests).Assembly), UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        foreach (string argument in new[] { "build", project, "-c", "Release", "--disable-build-servers", "-m:1", "-nodeReuse:false", "-v:normal" }
            .Concat(arguments))
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return (process.ExitCode, await output + await error);
    }
}
