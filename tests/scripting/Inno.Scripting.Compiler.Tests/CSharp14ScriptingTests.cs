using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Xml.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains;
using Inno.Plugins;
using Inno.Scripting.Compiler;
using Xunit;

namespace Inno.Scripting.Compiler.Tests;

/// <summary>
/// Verifies the language contract shared by authoring compilation and SDK IDE projects.
/// </summary>
public sealed class CSharp14ScriptingTests
{
    [Theory]
    [InlineData("LangVersion", "preview")]
    [InlineData("ImplicitUsings", "enable")]
    [InlineData("DefineConstants", "UNEXPECTED")]
    [InlineData("DisableImplicitConfigurationDefines", "false")]
    public async Task IdeRejectsOverridesOfTheFrozenCompilerContract(
        string property,
        string value
    ) {
        using var fixture = new CompilerFixture();
        fixture.Write("Probe.cs", "public sealed class Probe { }");
        fixture.Rescan();
        fixture.compiler.GenerateProjectFiles();
        string project = Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj");
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(host, project);
        using var output = new StringWriter();
        using var errors = new StringWriter();
        await Assert.ThrowsAsync<InvalidOperationException>(() => ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "build", project, "--disable-build-servers", "-m:1", "-nr:false", "-p:" + property + "=" + value],
            fixture.projectRoot, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath), output, errors));
        Assert.Contains("IDE compiler input '" + property + "' differs", output.ToString() + errors, StringComparison.Ordinal);
    }

    /// <summary>
    /// Projects installed extension properties and field-backed plugin properties into the IDE contract.
    /// </summary>
    /// <returns>
    /// Completion after both the engine and SDK compile the consumer against the installed source package.
    /// </returns>
    [Fact]
    public async Task InstalledPluginExtensionMembersAreAvailableInIdeReferences()
    {
        using var fixture = new CompilerFixture((
            root,
            serialization
        ) => {
            using FileStream stream = File.Create(Path.Combine(root, "Plugins", "language.iplugin"));
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            using (Stream manifest = archive.CreateEntry("Plugin.inno").Open())
                manifest.Write(serialization.Serialize(new PluginManifest
                {
                    pluginId = "tests.csharp14",
                    displayName = "CSharp14 source plugin"
                }));
            using (Stream source = archive.CreateEntry("Assets/PluginLanguage.cs").Open())
                source.Write(Encoding.UTF8.GetBytes("""
                    namespace LanguagePlugin;
                    public sealed class Counter
                    {
                        public int count { get; set => field = value; } = 3;
                    }
                    public static class CounterExtensions
                    {
                        extension(Counter counter)
                        {
                            public bool hasCount => counter.count > 0;
                        }
                    }
                    """));
            using Stream metadata = archive.CreateEntry("Assets/PluginLanguage.cs.imeta").Open();
            metadata.Write(serialization.Serialize(new ScriptingAssetSourceMeta
            {
                persistentId = Guid.NewGuid(),
                sourceKind = (int)AssetSourceKind.File,
                importerId = "inno.editor.csharp-script"
            }));
        });
        fixture.Write("Consumer.cs", """
            using LanguagePlugin;
            public static class Consumer
            {
                public static bool Evaluate() => new Counter().hasCount;
            }
            """);
        ScriptCompilationResult result = fixture.Compile();
        Assert.True(result.success, string.Join(Environment.NewLine, result.diagnostics));
        fixture.compiler.GenerateProjectFiles();
        string projectPath = Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(host, projectPath);
        _ = await BuildIdeAsync(sdk, projectPath, fixture.projectRoot);
    }

    /// <summary>
    /// Compiles property storage, conditional assignment, spans, lambdas and extension properties through both entry points.
    /// </summary>
    /// <returns>
    /// Completion after the SDK has compiled the generated IDE project without analyzer load failures.
    /// </returns>
    [Fact]
    public async Task AuthoringAndIdeAcceptTheSameCSharp14Source()
    {
        using var fixture = new CompilerFixture();
        fixture.Write("LanguageProbe.cs", """
            using System;
            using System.Collections.Generic;
            using InnoEngine.Reflection;
            using InnoEngine.Scene;
            using InnoEngine.Serialization;

            [StableTypeId("bf92a310-7446-4a19-81f3-0a02bd32f087")]
            public sealed class LanguageProbe : GameBehavior
            {
                [SerializableProperty]
                public int count
                {
                    get;
                    set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
                } = 3;

                public string genericName => nameof(List<>);
                public void Assign(LanguageProbe? target) => target?.count = count;
                public bool Contains(ReadOnlySpan<int> values) => values.Contains(3);
                public int ApplyRef()
                {
                    RefAction increment = (ref value) => value++;
                    int current = count;
                    increment(ref current);
                    return current;
                }
                private delegate void RefAction(ref int value);
            }

            public static class LanguageProbeExtensions
            {
                extension(LanguageProbe probe)
                {
                    public bool hasCount => probe.count > 0;
                }
            }

            public static class LanguageProbeConsumer
            {
                public static bool Evaluate() => new LanguageProbe().hasCount;
            }
            """);
        fixture.Rescan();
        ScriptCompilationResult first = fixture.Compile();
        Assert.True(first.success, string.Join(Environment.NewLine, first.diagnostics));
        ScriptCompilationResult cached = fixture.Compile();
        Assert.True(cached.success);
        Assert.Equal(first.outputDirectory, cached.outputDirectory);
        Assert.Empty(cached.compiledAssemblyNames);
        fixture.Write("LanguageSemantics.cs", """
            using System;
            public static class LanguageSemantics
            {
                private sealed class Value
                {
                    public int count
                    {
                        get;
                        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
                    } = 3;
                }
                public static bool Evaluate()
                {
                    int receiverCalls = 0;
                    int valueCalls = 0;
                    Value? receiver = null;
                    Value? Receive()
                    {
                        receiverCalls++;
                        return receiver;
                    }
                    int Produce()
                    {
                        valueCalls++;
                        return 7;
                    }
                    Receive()?.count = Produce();
                    if (receiverCalls != 1 || valueCalls != 0) return false;
                    receiver = new Value();
                    if (receiver.count != 3) return false;
                    Receive()?.count = Produce();
                    if (receiverCalls != 2 || valueCalls != 1 || receiver.count != 7) return false;
                    try
                    {
                        receiver.count = -1;
                        return false;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return receiver.count == 7;
                    }
                }
            }
            """);
        ScriptCompilationResult semantics = fixture.Compile();
        Assert.True(semantics.success, string.Join(Environment.NewLine, semantics.diagnostics));
        string assemblyPath = Path.Combine(semantics.outputDirectory!, "Inno.GameScripts.dll");
        WeakReference retired = InvokePublicSemantics(assemblyPath);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(retired.IsAlive);
        fixture.compiler.GenerateProjectFiles();
        string projectPath = Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj");
        XDocument project = XDocument.Load(projectPath);
        Assert.Equal("net10.0", Assert.Single(project.Descendants("TargetFramework")).Value);
        Assert.Equal("14.0", Assert.Single(project.Descendants("LangVersion")).Value);
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(host, projectPath);
        string output = await BuildIdeAsync(sdk, projectPath, fixture.projectRoot);
        Assert.DoesNotContain("CS8032", output, StringComparison.Ordinal);
    }

    private static async Task<string> BuildIdeAsync(
        DotNetSdkDescriptor sdk,
        string project,
        string directory
    ) {
        using var output = new StringWriter();
        using var errors = new StringWriter();
        try
        {
            await ToolchainEnvironment.RunAsync(sdk.hostPath,
                [sdk.cliPath, "build", project, "--disable-build-servers", "-m:1", "-nr:false", "-v:quiet"],
                directory, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath), output, errors);
            return output.ToString();
        }
        catch (InvalidOperationException failure)
        {
            throw new InvalidOperationException(output + Environment.NewLine + errors, failure);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference InvokePublicSemantics(string assemblyPath)
    {
        var lifetime = new AssemblyLoadContext("csharp14-semantics", isCollectible: true);
        var retired = new WeakReference(lifetime);
        try
        {
            using var stream = new MemoryStream(File.ReadAllBytes(assemblyPath));
            Assembly assembly = lifetime.LoadFromStream(stream);
            Func<bool> evaluate = assembly.GetType("LanguageSemantics", throwOnError: true)!
                .GetMethod("Evaluate", BindingFlags.Public | BindingFlags.Static)!.CreateDelegate<Func<bool>>();
            Assert.True(evaluate());
        }
        finally
        {
            lifetime.Unload();
        }
        return retired;
    }

    /// <summary>
    /// Invalidates reference and script artifacts when framework bytes change without changing the public contract.
    /// </summary>
    [Fact]
    public void FrameworkContentChangesSelectNewArtifacts()
    {
        var environment = new MutableCompilationEnvironment();
        using var fixture = new CompilerFixture(compilationEnvironment: environment);
        fixture.Write("Counter.cs", "public sealed class Counter { public int count { get; set; } }");
        fixture.Rescan();
        string host = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        ScriptCompilationInputs selected = new Inno.Build.SdkScriptCompilationEnvironment(host, fixture.projectRoot).Capture();
        string destination = Path.Combine(fixture.projectRoot, "Framework");
        Directory.CreateDirectory(destination);
        foreach (string path in selected.frameworkReferences)
            File.Copy(path, Path.Combine(destination, Path.GetFileName(path)));
        environment.inputs = new ScriptCompilationInputs(selected.sdkIdentity,
            Directory.EnumerateFiles(destination, "*.dll"), selected.predefinedSymbols);
        ScriptCompilationResult first = fixture.Compile();
        Assert.True(first.success, string.Join(Environment.NewLine, first.diagnostics));
        string referenceRoot = Path.Combine(fixture.projectRoot, "Library", "ScriptApi", "Runtime");
        string[] references = Directory.GetDirectories(referenceRoot);
        using (FileStream changed = new(Path.Combine(destination, "System.Runtime.dll"), FileMode.Append))
            changed.WriteByte(0);
        ScriptCompilationResult second = fixture.Compile();
        Assert.True(second.success, string.Join(Environment.NewLine, second.diagnostics));
        Assert.NotEqual(first.outputDirectory, second.outputDirectory);
        Assert.NotEmpty(second.compiledAssemblyNames);
        Assert.NotEmpty(Directory.GetDirectories(referenceRoot).Except(references));
    }

    private sealed class MutableCompilationEnvironment : IScriptCompilationEnvironmentProvider
    {
        internal ScriptCompilationInputs inputs { get; set; } = null!;

        public ScriptCompilationInputs Capture(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return inputs;
        }
    }
}
