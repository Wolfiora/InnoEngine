using Inno.Testing.Source;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.Cli;
using Inno.Build.Toolchains;
using Inno.Core.Identity;
using Inno.Core.IO;
using Inno.Core.Settings;
using Inno.Editor.Hosting;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scripting.Compiler;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class RepositoryWikiTests
{
    [Fact]
    public void CliLogicalRuntimeReferencesIncludeAnimation()
    {
        string project = Path.Combine(Path.GetTempPath(), "InnoCliWikiContracts", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(project);
        try
        {
            GenerateReferences(project, typeof(BuildApplication).Assembly);
            string assembly = Directory.EnumerateFiles(Path.Combine(project, "Library", "ScriptApi"),
                "Inno.ScriptApi.Runtime.dll", SearchOption.AllDirectories).Single();
            using FileStream stream = File.OpenRead(assembly);
            using var image = new PEReader(stream);
            MetadataReader metadata = image.GetMetadataReader();
            string[] types = metadata.TypeDefinitions.Select(handle =>
            {
                TypeDefinition type = metadata.GetTypeDefinition(handle);
                return metadata.GetString(type.Namespace) + "." + metadata.GetString(type.Name);
            }).ToArray();
            Assert.Contains("InnoEngine.Animation.Animation", types);
            Assert.Contains("InnoEngine.Animation.AnimationClipAsset", types);
            Assert.Contains("InnoEngine.UI.UiVertex", types);
            Assert.Contains("InnoEngine.Rendering.RenderPersistentResourceId", types);
        }
        finally
        {
            Directory.Delete(project, recursive: true);
        }
    }

    [Fact]
    public void LogicalEditorReferencesIncludeTheSharedProductFeatures()
    {
        string project = Path.Combine(Path.GetTempPath(), "InnoWikiContracts", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(project);
        try
        {
            GenerateReferences(project, typeof(RepositoryWikiTests).Assembly,
                typeof(EditorApplication).Assembly, typeof(BuildApplication).Assembly);
            string assembly = Directory.EnumerateFiles(Path.Combine(project, "Library", "ScriptApi"),
                "Inno.ScriptApi.Editor.dll", SearchOption.AllDirectories).Single();
            using FileStream stream = File.OpenRead(assembly);
            using var image = new PEReader(stream);
            MetadataReader metadata = image.GetMetadataReader();
            string[] types = metadata.TypeDefinitions.Select(handle =>
            {
                TypeDefinition type = metadata.GetTypeDefinition(handle);
                return metadata.GetString(type.Namespace) + "." + metadata.GetString(type.Name);
            }).ToArray();
            Assert.Contains("InnoEditor.Settings.EditorSetting", types);
            Assert.Contains("InnoEditor.Settings.ProjectSettingEditor`1", types);
            Assert.Contains("InnoEditor.Shaders.ShaderNodeDrawer", types);
            Assert.DoesNotContain(types, static type => type.StartsWith("Inno.Editor.", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(project, recursive: true);
        }
    }

    [Fact]
    [Trait("ExternalDependency", "NativeProduct")]
    public async Task CurrentWikiCompilesWithTheCurrentLogicalScriptContracts()
    {
        string repository = TestRepository.GetRoot(typeof(RepositoryWikiTests).Assembly);
        string owner = Path.Combine(repository, "artifacts", "acceptance", "wiki-gate", Guid.NewGuid().ToString("N"));
        string project = Path.Combine(owner, "Project");
        Directory.CreateDirectory(project);
        try
        {
            GenerateReferences(project, typeof(RepositoryWikiTests).Assembly,
                typeof(EditorApplication).Assembly, typeof(BuildApplication).Assembly);
            string references = Path.Combine(project, "Library", "ScriptApi");
            string runtime = Directory.EnumerateFiles(references, "Inno.ScriptApi.Runtime.dll", SearchOption.AllDirectories).Single();
            string editor = Directory.EnumerateFiles(references, "Inno.ScriptApi.Editor.dll", SearchOption.AllDirectories).Single();
            string target = OperatingSystem.IsWindows() ? "windows-x64" : "macos-arm64";
            string configuration = typeof(RepositoryWikiTests).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
            TextWriter previousError = Console.Error;
            using var errors = new StringWriter();
            int result;
            try
            {
                Console.SetError(errors);
                result = await BuildApplication.Run(
                    ["verify", repository, "--configuration", configuration, "--wiki", "--wiki-target", target,
                        "--dotnet", ToolchainEnvironment.ResolveExecutable("dotnet"),
                        "--wiki-api", Path.Combine(owner, "api.json"),
                        "--wiki-examples", Path.Combine(owner, "examples.json"),
                        "--wiki-runtime-reference", runtime, "--wiki-editor-reference", editor],
                    CancellationToken.None);
            }
            finally
            {
                Console.SetError(previousError);
                File.WriteAllText(Path.Combine(owner, "validation-errors.txt"), errors.ToString());
            }
            Assert.True(result == 0, $"Current Wiki validation failed; inventories are retained at '{owner}'.\n{errors}");
        }
        finally
        {
            // Only this test's empty project and generated references are disposable.
            // Keep the inventories for CI and local diagnosis.
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    private static void GenerateReferences(
        string project,
        params System.Reflection.Assembly[] roots
    ) {
        string assetsRoot = Path.Combine(project, "Assets");
        string pluginsRoot = Path.Combine(project, "Plugins");
        string libraryRoot = Path.Combine(project, "Library");
        Directory.CreateDirectory(assetsRoot);
        Directory.CreateDirectory(pluginsRoot);
        Directory.CreateDirectory(libraryRoot);
        var identities = new IdentityAllocator();
        using IDisposable identityScope = identities.EnterScope();
        using EngineHost host = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(roots),
            new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource()).Build();
        using IDisposable diagnostics = host.diagnostics.EnterScope();
        var sources = new PluginSourceService(host.serialization, pluginsRoot, libraryRoot);
        PluginScanResult scan = sources.Scan();
        using var assets = new AssetPipeline(host.modules, host.types, host.serialization, identities,
            host.diagnostics, host.logs, AssetPipelineOptions.Create(assetsRoot, libraryRoot)
                with { enableFileSystemWatcher = false });
        using var settings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.Combine(project, "Settings.Project.inno")),
            host.types, host.serialization, new ProjectId("wiki.contract.validation"),
            AssetSerializationContext.Create(assets));
        using var plugins = new PluginEnvironment(assets, settings, host.serialization, pluginsRoot,
            libraryRoot, scan, host.modules.generations);
        var compiler = new ScriptCompiler(new ScriptCompilerOptions { projectRootDirectory = project }, assets, plugins);
        compiler.GenerateProjectFiles();
    }

}
