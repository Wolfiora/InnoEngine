using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
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
        string dotnetHost = Path.Combine(
            Path.GetFullPath("../../..", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var compiler = new ScriptCompiler(
            new ScriptCompilerOptions
            {
                projectRootDirectory = project,
                compilationEnvironment = new Inno.Build.SdkScriptCompilationEnvironment(dotnetHost, project)
            }, assets, plugins);
        compiler.GenerateProjectFiles();
    }

}
