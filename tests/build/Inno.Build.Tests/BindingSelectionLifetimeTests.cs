using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BindingSelectionLifetimeTests
{
    [Fact]
    public async Task PublicationWithoutItsCompiledSelectionFailsBeforePreparingTheTaskRuntime()
    {
        string engine = ToolchainEnvironment.FindRepoRoot();
        string root = Path.Combine(Path.GetTempPath(), "InnoSelectionPreflight", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.Copy(Path.Combine(engine, "global.json"), Path.Combine(root, "global.json"));
            string project = Path.Combine(root, "Fixture.proj");
            new XDocument(new XElement("Project",
                new XElement("Import", new XAttribute("Project", Path.Combine(engine, "Directory.Build.targets"))),
                new XElement("PropertyGroup", new XElement("InnoProductId", "player")),
                new XElement("Target", new XAttribute("Name", "PrepareInnoBuildTaskHost"),
                    new XElement("Error", new XAttribute("Text", "An invalid publication prepared the task runtime.")))))
                .Save(project);
            string sdk = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
            using CancellationTokenSource timeout = new(TimeSpan.FromMinutes(3));
            using StringWriter output = new();
            using StringWriter error = new();
            Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(sdk,
                ["msbuild", project, "-t:PublishProductNativeDeployment", "-nodeReuse:false", "-v:minimal"],
                root, timeout.Token, null, output, error));
            Assert.NotNull(failure);
            string messages = output.ToString() + error;
            Assert.Contains("Native publication requires the exact binding selection", messages);
            Assert.DoesNotContain("An invalid publication prepared the task runtime.", messages);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task SelectionRemainsOwnedThroughBuildAndIsReleasedAfterPublish()
    {
        string engine = ToolchainEnvironment.FindRepoRoot();
        string root = Path.Combine(Path.GetTempPath(), "InnoSelectionLifetime", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.Copy(Path.Combine(engine, "global.json"), Path.Combine(root, "global.json"));
            File.WriteAllText(Path.Combine(root, "Directory.Build.props"), "<Project />");
            string selection = Path.Combine(root, "NativeBindingSelection.props");
            new XDocument(new XElement("Project",
                new XElement("Import", new XAttribute("Project", Path.Combine(engine, "Directory.Build.targets"))),
                new XElement("PropertyGroup",
                    new XElement("_InnoOwnedProductBindingSelection", selection),
                    new XElement("InnoNativeBindingSelection", selection)),
                new XElement("Target", new XAttribute("Name", "CreateFixtureSelection"),
                    new XAttribute("BeforeTargets", "PrepareForBuild"),
                    new XElement("WriteLinesToFile", new XAttribute("File", selection),
                        new XAttribute("Lines", "<Project />"), new XAttribute("Overwrite", "true"))),
                new XElement("Target", new XAttribute("Name", "RequireCompiledSelectionForPublication"),
                    new XAttribute("BeforeTargets", "Publish"),
                    new XElement("Error", new XAttribute("Condition", $"!Exists('{selection}')"),
                        new XAttribute("Text", "Build retired the selection before its publication consumer.")))))
                .Save(Path.Combine(root, "Directory.Build.targets"));
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net9.0"),
                    new XElement("ImplicitUsings", "disable"), new XElement("Nullable", "enable"))))
                .Save(project);
            File.WriteAllText(Path.Combine(root, "Api.cs"), "public static class Api { public static int value => 42; }");

            await Execute("build");
            Assert.True(File.Exists(selection));
            await Execute("publish");
            Assert.False(File.Exists(selection));

            async Task Execute(string command)
            {
                string sdk = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
                    OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
                using CancellationTokenSource timeout = new(TimeSpan.FromMinutes(3));
                using StringWriter output = new();
                using StringWriter error = new();
                Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(sdk,
                    [command, project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "-v:minimal"],
                    root, timeout.Token, null, output, error));
                Assert.True(failure is null, output.ToString() + error + failure);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
