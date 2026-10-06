using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

using Inno.Build.Tasks;
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
            string sources = Path.Combine(root, "compile-sources.txt");
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "net9.0"),
                    new XElement("AllowUnsafeBlocks", "true"),
                    new XElement("ImplicitUsings", "disable"),
                    new XElement("Nullable", "enable"),
                    new XElement("IsTestProject", "true"),
                    new XElement("BindGenGeneratedBindings", "true"),
                    new XElement("BindGenProfile", "fixture-target"),
                    new XElement("BindGenConfig", config),
                    new XElement("BindGenRoot", Path.Combine(engine, "..", "BindGen-CS")),
                    new XElement("BindGenTargetOutputRoot", Path.Combine(root, "obj", "fixture-target"))),
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
            Assert.Equal(1, output.Split("INNO-TASK-HOST prepared", StringSplitOptions.None).Length - 1);
            string selected = Assert.Single(File.ReadAllLines(sources)
                .Where(static path => Path.GetFileName(path) == "Bindings.cs"));
            Assert.StartsWith(Path.Combine(root, "obj", "fixture-target") + Path.DirectorySeparatorChar, selected);
            Assert.True(File.Exists(selected), output);
            Assert.True(File.Exists(Path.Combine(root, "bin", "Release", "net9.0", "Fixture.dll")), output);

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
