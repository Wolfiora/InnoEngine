using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Inno.Build;
using Xunit;

namespace Inno.Scripting.Compiler.Tests;

public sealed class SdkCompilationInputsTests
{
    [Fact]
    public void SelectedSdkSuppliesFrameworkSymbolsAndIdeReferences()
    {
        using var fixture = new CompilerFixture();
        string host = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        ScriptCompilationInputs inputs = new SdkScriptCompilationEnvironment(host, fixture.projectRoot).Capture();
        Assert.Contains("NET10_0", inputs.predefinedSymbols);
        Assert.Contains("NET10_0_OR_GREATER", inputs.predefinedSymbols);
        Assert.DoesNotContain("DEBUG", inputs.predefinedSymbols);
        Assert.DoesNotContain("TRACE", inputs.predefinedSymbols);
        Assert.Contains(inputs.frameworkReferences, static path => Path.GetFileName(path) == "System.Runtime.dll");
        Assert.All(inputs.frameworkReferences, static path => Assert.True(File.Exists(path), path));
        Assert.DoesNotContain(inputs.frameworkReferences, static path => Path.GetFileName(path) == "System.Private.CoreLib.dll");
        fixture.Write("FrameworkSymbols.cs", """
            #if !NET10_0 || !NET10_0_OR_GREATER
            #error SDK framework symbols must match runtime and IDE compilation.
            #endif
            public sealed class FrameworkSymbols { }
            """);
        fixture.Rescan();
        Assert.True(fixture.Compile().success);
        fixture.compiler.GenerateProjectFiles();
        string project = File.ReadAllText(Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj"));
        Assert.Contains("ValidateInnoScriptSdk", project, StringComparison.Ordinal);
        Assert.Contains("<DisableImplicitFrameworkReferences>true</DisableImplicitFrameworkReferences>", project, StringComparison.Ordinal);
        foreach (string path in inputs.frameworkReferences)
            Assert.Contains(path.Replace("&", "&amp;", StringComparison.Ordinal), project, StringComparison.Ordinal);
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(fixture.projectRoot, "Library", "Scripting"), "*.csproj", SearchOption.AllDirectories));
    }

    [Fact]
    public void MissingSelectedSdkDoesNotPublishScriptArtifactsOrRetainEvaluationProjects()
    {
        using var fixture = new CompilerFixture();
        File.WriteAllText(Path.Combine(fixture.projectRoot, "global.json"), """
            { "sdk": { "version": "10.0.999", "rollForward": "disable", "allowPrerelease": false } }
            """);
        fixture.Write("Probe.cs", "public sealed class Probe { }");
        fixture.Rescan();
        Assert.Throws<InvalidOperationException>(() => fixture.Compile());
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(fixture.projectRoot, "Library"), "*.dll", SearchOption.AllDirectories));
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(fixture.projectRoot, "Library"), "*.csproj", SearchOption.AllDirectories));
    }

    [Fact]
    public void CancelledSdkEvaluationCreatesNoPublishedInputs()
    {
        using var fixture = new CompilerFixture();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        string host = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var provider = new SdkScriptCompilationEnvironment(host, fixture.projectRoot);
        Assert.ThrowsAny<OperationCanceledException>(() => provider.Capture(cancellation.Token));
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(fixture.projectRoot, "Library"), "*.csproj", SearchOption.AllDirectories));
    }

    [Fact]
    public void InputContractCopiesCollectionsAndRejectsIncompleteReferences()
    {
        string[] references = [typeof(object).Assembly.Location];
        string[] symbols = ["NET10_0"];
        var inputs = new ScriptCompilationInputs("10.0.402", references, symbols);
        references[0] = "changed";
        symbols[0] = "CHANGED";
        Assert.True(Path.IsPathFullyQualified(Assert.Single(inputs.frameworkReferences)));
        Assert.Equal("NET10_0", Assert.Single(inputs.predefinedSymbols));
        Assert.Throws<ArgumentException>(() => new ScriptCompilationInputs("10.0.402", [], []));
        Assert.Throws<ArgumentException>(() => new ScriptCompilationInputs("10.0.402", ["relative.dll"], []));
    }
}
