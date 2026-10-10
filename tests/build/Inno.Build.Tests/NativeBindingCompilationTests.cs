using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

using Inno.Build.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBindingCompilationTests
{
    [Fact]
    public async Task TargetCompilationReplacesHostSourceAndRejectsAMissingSelection()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBindingCompilation", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "Generated"));
        try
        {
            File.WriteAllText(Path.Combine(root, "Generated", "Bindings.cs"), "#error Host bindings must not be compiled for this target.");
            File.WriteAllText(Path.Combine(root, "api.h"), "int sample(int value);");
            string config = Path.Combine(root, "bindgen.json");
            File.WriteAllText(config, JsonSerializer.Serialize(new
            {
                Preset = "c-library", Namespace = "Fixture.Native", ApiName = "Api", LibName = "fixture",
                EntryFiles = new[] { "api.h" }, AllowedHeaders = new[] { "api.h" },
                GenerateRuntimeSource = false, MergeGeneratedFilesToSingleFile = true,
                SingleFileOutputName = "Bindings.cs"
            }));
            string engine = FindEngine();
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            string definition = Path.Combine(root, "bindings.props");
            new XDocument(new XElement("Project",
                new XElement("PropertyGroup", new XElement("InnoBindingHostConfig", "bindgen.json")),
                new XElement("ItemGroup", new XElement("InnoBindingTarget", new XAttribute("Include", "fixture-target"),
                    new XElement("Config", "bindgen.json")))))
                .Save(definition);
            string sources = Path.Combine(root, "compile-sources.txt");
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("Import", new XAttribute("Project", "$(InnoNativeBindingSelection)"),
                    new XAttribute("Condition", "'$(InnoNativeBindingSelection)' != '' and Exists('$(InnoNativeBindingSelection)')")),
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "net9.0"),
                    new XElement("AllowUnsafeBlocks", "true"),
                    new XElement("ImplicitUsings", "disable"),
                    new XElement("EnableDefaultCompileItems", "false"),
                    new XElement("Nullable", "enable"),
                    new XElement("IsTestProject", "true"),
                    new XElement("BindGenGeneratedBindings", "true"),
                    new XElement("InnoBindingTargetId", "fixture-target"),
                    new XElement("InnoBindingEngineRoot", root),
                    new XElement("InnoBindingDefinition", definition),
                    new XElement("BindGenRoot", Path.Combine(engine, "..", "BindGen-CS")),
                    new XElement("InnoBindingOutputMode", "TargetArtifacts")),
                new XElement("Import", new XAttribute("Project", definition)),
                new XElement("Import", new XAttribute("Project", Path.Combine(engine, "Directory.Build.targets"))),
                new XElement("ItemGroup", new XElement("Reference", new XAttribute("Include", "BGCS.Runtime"),
                    new XElement("HintPath", typeof(BGCS.Runtime.FunctionTable).Assembly.Location))),
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
            Assert.StartsWith(Path.Combine(root, "obj", "fixture-target") + Path.DirectorySeparatorChar, selected);
            Assert.True(File.Exists(selected), output);
            Assert.True(File.Exists(Path.Combine(root, "bin", "Release", "net9.0", "Fixture.dll")), output);

            string selection = Path.Combine(root, "SelectedBindings.props");
            NativeBindingGenerationDescriptor.WriteSelection(selection,
                new Dictionary<string, NativeBindingGenerationDescriptor>
                {
                    [project] = new()
                    {
                        fingerprint = Directory.GetParent(Path.GetDirectoryName(selected)!)!.Name,
                        bindingsPath = selected,
                        bridgeDirectory = string.Empty
                    }
                }, new Dictionary<string, NativeLibraryKind> { [project] = NativeLibraryKind.Shared });
            (int reused, string reuseOutput) = await Build(project, ["-p:InnoNativeBindingSelection=" + selection]);
            Assert.True(reused == 0, reuseOutput);
            Assert.Equal(selected, Assert.Single(File.ReadAllLines(sources)
                .Where(static path => Path.GetFileName(path) == "Bindings.cs")));
            Assert.DoesNotContain("INNO-TASK-RUNTIME", reuseOutput);

            string staticSelection = Path.Combine(root, "StaticBindings.props");
            NativeBindingGenerationDescriptor.WriteSelection(staticSelection,
                new Dictionary<string, NativeBindingGenerationDescriptor>
                {
                    [project] = new()
                    {
                        fingerprint = Directory.GetParent(Path.GetDirectoryName(selected)!)!.Name,
                        bindingsPath = selected,
                        bridgeDirectory = string.Empty
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
                new XElement("PropertyGroup", new XElement("TargetFramework", "net9.0"),
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

    private static string FindEngine()
    {
        DirectoryInfo? current = new(AppContext.BaseDirectory);
        while (current is not null && !File.Exists(Path.Combine(current.FullName, "InnoEngine.sln")))
            current = current.Parent;
        return current?.FullName ?? throw new DirectoryNotFoundException("The engine checkout is unavailable.");
    }

    private static async Task<(int exitCode, string output)> Build(
        string project,
        string[] arguments
    ) {
        string host = Path.Combine(Path.GetFullPath("../../..", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(host)
        {
            WorkingDirectory = FindEngine(), UseShellExecute = false, CreateNoWindow = true,
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
