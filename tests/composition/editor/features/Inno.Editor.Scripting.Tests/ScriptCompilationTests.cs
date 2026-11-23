using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Xunit;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Annotations;
using Inno.Editor.Interactions;
using Inno.Editor.PlayMode;
using Inno.Editor.Scene;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Plugins;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;
using Inno.Testing.Scripting;

namespace Inno.Editor.Scripting.Tests;

public sealed class ScriptCompilationTests : ScriptingPipelineFixture
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceLocalAssetPathsUseCompileTimeOwnershipAcrossDeployments(bool runtimeDeployment)
    {
        m_fixture.Write("Nested,=Folder/SourceLocalPathProbe.cs", """
            using InnoEngine.Assets;
            public static class SourceLocalPathProbe
            {
                public static AssetPath Resolve() => Assets.LocalPath("Materials/Default.imaterial");
            }
            """);
        ScriptCompilationResult result = runtimeDeployment ? m_fixture.CompileRuntimeDeployment() : m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
        AssetPath path = InvokePublicPathProbe(Path.Combine(result.outputDirectory!, "Inno.GameScripts.dll"));
        Assert.Equal(AssetSourceID.project, path.source);
        Assert.Equal("Materials/Default.imaterial", path.localPath);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceLocalAssetPathsFollowInstalledPluginOwnership(bool runtimeDeployment)
    {
        using var fixture = new ScriptingFixture((
            root,
            serialization
        ) => WritePluginPackage(
            root,
            "local-path.iplugin",
            serialization,
            new PluginManifest
            {
                pluginId = "tests.local-path",
                displayName = "Source Ownership Fixture"
            },
            new Dictionary<string, byte[]>
            {
                ["Assets/Nested.imeta"] = serialization.Serialize(
                    new ScriptingAssetSourceMeta { persistentId = Guid.NewGuid(), sourceKind = (int)AssetSourceKind.Directory }
                ),
                ["Assets/Nested/SourceLocalPathProbe.cs"] = Encoding.UTF8.GetBytes("""
                    using InnoEngine.Assets;
                    public static class SourceLocalPathProbe
                    {
                        public static AssetPath Resolve() => Assets.LocalPath("Materials/Default.imaterial");
                    }
                    """),
                ["Assets/Nested/SourceLocalPathProbe.cs.imeta"] = CreateScriptSourceMeta(serialization, Guid.NewGuid())
            }
        ));
        ScriptCompilationResult result = runtimeDeployment ? fixture.CompileRuntimeDeployment() : fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
        string assemblyPath = Assert.Single(
            result.runtimeAssemblyPaths.Where(static path => Path.GetFileName(path).StartsWith("Inno.Plugin.", StringComparison.Ordinal))
        );
        AssetPath path = InvokePublicPathProbe(assemblyPath);
        Assert.Equal(new AssetSourceID("tests.local-path"), path.source);
        Assert.Equal("Materials/Default.imaterial", path.localPath);
    }

    [Fact]
    public void SourceLocalAssetPathsPreserveCallerSemanticsInIdeReferencesAndProjectMapping()
    {
        m_fixture.Write("Nested,=Folder/SourceLocalPathProbe.cs", """
            using InnoEngine.Assets;
            public static class SourceLocalPathProbe
            {
                public static AssetPath Resolve() => Assets.LocalPath("Materials/Default.imaterial");
            }
            """);
        m_fixture.Rescan();
        m_fixture.compiler.GenerateProjectFiles();
        XDocument project = XDocument.Load(Path.Combine(m_fixture.projectRoot, "Inno.GameScripts.csproj"));
        string map = Assert.Single(project.Descendants("PathMap")).Value;
        Assert.Contains("Nested,,==Folder", map, StringComparison.Ordinal);
        Assert.Contains("project::./Nested,,==Folder", map, StringComparison.Ordinal);
        Assert.Contains(
            project.Descendants("HintPath"),
            reference => ContainsCustomAttribute(Path.GetFullPath(reference.Value, m_fixture.projectRoot), "CallerFilePathAttribute")
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GeneratedRegistrationUsesTargetFrameworkReferenceContracts(bool runtimeDeployment)
    {
        m_fixture.Write("FrameworkReferenceProbe.cs", """
            using InnoEngine.Mathematics;
            using InnoEngine.Scene;
            public sealed class FrameworkReferenceProbe : GameBehavior
            {
                public Vector2 point { get; set; }
            }
            """);
        ScriptCompilationResult result = runtimeDeployment ? m_fixture.CompileRuntimeDeployment() : m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
        using FileStream stream = File.OpenRead(Path.Combine(result.outputDirectory!, "Inno.GameScripts.dll"));
        using var executable = new PEReader(stream);
        MetadataReader metadata = executable.GetMetadataReader();
        Assert.DoesNotContain(
            metadata.AssemblyReferences,
            handle => metadata.GetString(metadata.GetAssemblyReference(handle).Name) == "System.Private.CoreLib"
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RegistrationImplementationReferencesDoNotExposeIgnoredApi(bool runtimeDeployment)
    {
        m_fixture.Write("IgnoredApiProbe.cs", """
            using InnoEngine.Scene;
            public static class IgnoredApiProbe
            {
                public static void Capture(SceneAsset value) => value.CaptureFrom(null!, null!, null!);
            }
            """);
        ScriptCompilationResult result = runtimeDeployment ? m_fixture.CompileRuntimeDeployment() : m_fixture.Compile();
        Assert.False(result.success);
        Assert.Contains(
            result.diagnostics,
            static diagnostic => diagnostic.id == "CS1061" && diagnostic.message.Contains("CaptureFrom", StringComparison.Ordinal)
        );
    }

    [Fact]
    public void DispatchObservationIsAvailableInRuntimeCompilationAndIdeReferences()
    {
        m_fixture.Write("DispatchObservation.cs", """
            using System;
            using InnoEngine.Events;
            public static class DispatchObservationProbe
            {
                public static void Attach(
                    EventDispatcher dispatcher,
                    Action<Event> observer
                ) {
                    dispatcher.dispatched += observer;
                }
                public static void Detach(
                    EventDispatcher dispatcher,
                    Action<Event> observer
                ) {
                    dispatcher.dispatched -= observer;
                }
            }
            """);
        ScriptCompilationResult compilation = m_fixture.CompileRuntimeDeployment();
        Assert.True(compilation.success, FormatDiagnostics(compilation));
        m_fixture.compiler.GenerateProjectFiles();
        foreach (string project in new[]
        {
            "Inno.GameScripts.csproj",
            "Inno.EditorScripts.csproj"
        }

        ) {
            XDocument document = XDocument.Load(Path.Combine(m_fixture.projectRoot, project));
            bool found = false;
            foreach (XElement reference in document.Descendants("HintPath"))
            {
                using FileStream stream = File.OpenRead(Path.GetFullPath(reference.Value, m_fixture.projectRoot));
                using var executable = new PEReader(stream);
                MetadataReader metadata = executable.GetMetadataReader();
                foreach (TypeDefinitionHandle handle in metadata.TypeDefinitions)
                {
                    TypeDefinition type = metadata.GetTypeDefinition(handle);
                    if (metadata.GetString(type.Namespace) != "InnoEngine.Events" || metadata.GetString(type.Name) != "EventDispatcher")
                    {
                        continue;
                    }
                    found = type
                        .GetEvents()
                        .Any(eventHandle => metadata.GetString(metadata.GetEventDefinition(eventHandle).Name) == "dispatched");
                }
            }

            Assert.True(found, $"The IDE reference set in {project} must contain the dispatch observation event.");
        }
    }

    [Fact]
    public void ShaderPreviewOptionalInputsRemainNullableInTheLogicalAndImplementationApis()
    {
        m_fixture.Write("NullablePreview.editor.cs", """
            #nullable enable
            #pragma warning error CS8625, CS8602, CS8604
            using InnoEngine.Rendering;
            using InnoEditor.Shaders;
            public sealed class NullablePreviewProbe
            {
                public bool Resolve(ShaderPreviewContext preview, IRenderResourceService resources)
                    => resources.TryResolveMaterialArtifact(preview.resourceID, preview.artifact, preview.material,
                        new ShaderContractID("test.surface"), new ShaderPassRoleID("test.color"),
                        ShaderProgramKind.Raster, null, null, preview.diagnostics, out _);
                public void TypedCallback(RenderGraphBuilder graph)
                    => graph.AddRasterPass("typed", new RenderPhaseID("test"), "payload",
                        static (value, context) => Consume(value));
                private static void Consume(string value) { _ = value.Length; }
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
    }

    [Fact]
    public void ShaderNodeExtensionsCompileThroughTheEditorOnlyLogicalApi()
    {
        m_fixture.Write("ShaderNodeProbe.editor.cs", """
            using System.Collections.Generic;
            using System.Threading;
            using InnoEngine.Graphs;
            using InnoEngine.Serialization;
            using InnoEngine.Assets;
            using InnoEngine.Rendering;
            using InnoEditor.Assets;
            using InnoEditor.Rendering.Shaders;
            using InnoEditor.Rendering.Assets;
            using InnoEditor.Shaders;
            using InnoEditor.Rendering;

            public static class PipelineSettingsProbe
            {
                public static SerializedRenderExtensionState Capture<T>(T settings) where T : class, ISerializable
                    => new(EditorAssets.CaptureProperties(settings));
                public static void Edit<T>(InnoEditor.Rendering.PipelineDocuments documents, AssetPath path, T settings) where T : class, ISerializable
                    => documents.ReplaceSettings(documents.Open(path), settings);
            }

            [ShaderPreviewProvider("tests.preview")]
            public sealed class PreviewProbe : ShaderPreviewProvider
            {
                public override EditorViewportLayer CreateLayer(ShaderPreviewContext context)
                {
                    var frame = new RenderFrameData();
                    frame.Set(new("tests.preview"), context);
                    return new("tests.preview", null, frame, 0);
                }
                public static bool Resolve(IRenderResourceService resources, ShaderPreviewContext context)
                    => resources.TryResolveMaterialArtifact(context.resourceID, context.artifact, context.material,
                        new("tests.preview"), new("draw"), ShaderProgramKind.Raster, null, null, context.diagnostics, out _);
            }

            public sealed class ShaderNodeProbe : IShaderNodeCompiler
            {
                public string definitionID => "tests.shader-node-probe";
                public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context)
                    => [new("value", ShaderSourceType.Atomic("float"), GraphPortDirection.Output)];
                public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
                    => new Dictionary<string, ShaderIrValue> { ["value"] = context.builder.Constant(0.5f) };
            }

            [ShaderTarget("tests.script-target")]
            public sealed class ShaderTargetProbe : ShaderTarget
            {
                public override GraphDocument Expand(ShaderTargetContext context, CancellationToken cancellationToken)
                    => ShaderGraphTemplates.CreateRaster(context.serialization, context.references);
            }

            [ShaderGraphTemplate("tests.script-template", "Script Surface")]
            public sealed class ShaderTemplateProbe : ShaderGraphTemplate
            {
                public override GraphDocument Create(SerializationRegistry serialization, SerializationContext context)
                {
                    GraphDocument graph = ShaderGraphTemplates.CreateRaster(serialization, context);
                    ShaderParameterPresentation.Write(graph, new("gain"), new()
                    { group = "Surface", description = "Preview gain", hasRange = true, minimum = 0, maximum = 2 }, serialization, context);
                    ShaderGraphDocument.SetTarget(graph, "tests.script-target", serialization, context);
                    return ShaderGraphBindings.RemoveNodes(graph, [], serialization, context);
                }
            }

            [ShaderNodeDrawer("tests.shader-node-probe")]
            public sealed class ShaderDrawerProbe : ShaderNodeDrawer
            {
                public override void Draw(ShaderNodeDrawContext context)
                {
                    _ = context.Read("gain", 1f);
                    _ = context.previews.deviceGeneration;
                    context.DrawProperty<ShaderFunctionAsset?>("function", "Function", null);
                }
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.Contains(result.moduleDeployments, static request => request.scope == AssemblyScope.Editor);
        m_fixture.compiler.GenerateProjectFiles();
        Assert.True(ContainsShaderApi("Inno.EditorScripts.csproj"));
        Assert.False(ContainsShaderApi("Inno.GameScripts.csproj"));
        bool ContainsShaderApi(string project)
        {
            XDocument document = XDocument.Load(Path.Combine(m_fixture.projectRoot, project));
            foreach (XElement element in document.Descendants("HintPath"))
            {
                string path = Path.GetFullPath(element.Value, m_fixture.projectRoot);
                using FileStream stream = File.OpenRead(path);
                using var executable = new PEReader(stream);
                MetadataReader metadata = executable.GetMetadataReader();
                if (metadata.TypeDefinitions.Any(handle =>
                {
                    TypeDefinition type = metadata.GetTypeDefinition(handle);
                    return metadata.GetString(type.Namespace) == "InnoEditor.Rendering.Shaders"
                           && metadata.GetString(type.Name) == "IShaderNodeCompiler";
                })) {
                    return true;
                }
            }

            return false;
        }
    }

    [Fact]
    public void UserStaticImportsKeepTheirBindingWhenNativeApiNamespacesAreIntroduced()
    {
        m_fixture.Write("StaticNames.editor.cs", """
            using System;
            using InnoEditor.Rendering.Shaders;
            using static Inno.StaticNames.Helpers;
            namespace Inno.StaticNames;
            public static class Helpers
            {
                public static ShaderGraphInputSettings Input() => new();
                public static T Settings<T>(T value) => value;
            }
            public sealed class Consumer
            {
                public ShaderGraphInputSettings Build() => Input();
                public Func<ShaderGraphInputSettings> Factory() => Input;
                public int Value() => Settings<int>(7);
                public int Local()
                {
                    Func<int> Input = () => 9;
                    return Input();
                }
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
    }

    [Fact]
    public void RuntimeScriptsCannotReferenceShaderAuthoringIr()
    {
        m_fixture.Write("ShaderRuntimeLeak.cs", """
            using InnoEditor.Rendering.Shaders;
            public sealed class ShaderRuntimeLeak { public ShaderIrBuilder? builder; }
            """);
        Assert.False(m_fixture.Compile().success);
    }

    [Fact]
    public void RuntimeAndEditorSourcesProduceSeparateDeterministicArtifacts()
    {
        m_fixture.Write("ProjectBehavior.cs", """
            using InnoEngine.Scene;

            public sealed class ProjectBehavior : GameBehavior
            {
            }
            """);
        m_fixture.Write("ProjectTools.editor.cs", """
            using InnoEditor.Core;

            public sealed class ProjectTools
            {
                public EditorContext? context { get; set; }
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.NotNull(result.outputDirectory);
        Assert.True(File.Exists(Path.Combine(result.outputDirectory!, "Inno.GameScripts.dll")));
        Assert.True(File.Exists(Path.Combine(result.outputDirectory!, "Inno.EditorScripts.dll")));
        Assert.NotEmpty(result.runtimeAssemblyPaths);
        Assert.Contains(result.moduleDeployments, static request => request.scope == AssemblyScope.Runtime);
        Assert.Contains(result.moduleDeployments, static request => request.scope == AssemblyScope.Editor);
    }

    [Fact]
    public void ScriptApiTypeMatchingItsImplementationNamespaceRemainsCallable()
    {
        m_fixture.Write("AssetLookupProbe.cs", """
            using InnoEngine.Assets;
            using InnoEngine.Rendering;

            namespace Inno.Rendering2D;

            internal static class AssetLookupProbe
            {
                internal static bool TryResolve(AssetPath path, out MaterialAsset? asset)
                    => Assets.TryLoad(path, out asset);
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
    }

    [Fact]
    public void EditorPluginSettingsExtensionsCompileThroughLogicalNamespaces()
    {
        m_fixture.Write("SettingsProbe.editor.cs", """
            using InnoEditor.Settings;
            using InnoEngine.Serialization;
            using InnoEngine.Settings;

            [ProjectSettingPath("Project/Tests/Probe")]
            public sealed class SettingsProbeEditor : ProjectSettingEditor<SettingsProbe>
            {
                protected override void OnDraw(SettingsProbe setting)
                {
                }
            }

            public sealed class SettingsProbe : ISerializable
            {
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
    }

    [Fact]
    public void RuntimeDeploymentCompilationDoesNotCompileOrValidateEditorSources()
    {
        m_fixture.Write("RuntimeOnly.cs", "public sealed class RuntimeOnly { }");
        m_fixture.Write("BrokenTool.editor.cs", "this is deliberately invalid editor C#");
        ScriptCompilationResult result = m_fixture.CompileRuntimeDeployment();
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.NotNull(result.outputDirectory);
        Assert.True(File.Exists(Path.Combine(result.outputDirectory!, "Inno.GameScripts.dll")));
        Assert.False(File.Exists(Path.Combine(result.outputDirectory!, "Inno.EditorScripts.dll")));
        Assert.DoesNotContain(result.moduleDeployments, static request => request.scope == AssemblyScope.Editor);
        Assert.DoesNotContain(
            result.compiledAssemblyNames.Concat(result.reusedAssemblyNames),
            static assemblyName => assemblyName.Contains("Editor", StringComparison.Ordinal)
        );
    }

    [Fact]
    public void InspectorPresentationMetadataIsAuthoringOnlyWhileSerializationRemainsInPlayer()
    {
        m_fixture.Write("AttributedBehavior.cs", """
            using InnoEngine.Scene;
            using InnoEngine.Serialization;
            using InnoEditor.Annotations;

            public sealed class CustomPresentationAttribute : InspectorPresentationAttribute
            {
            }

            public sealed class AttributedBehavior : GameBehavior
            {
                [SerializableProperty]
                [Header("Visual")]
                [Text("Persistent authoring text.")]
                [Tooltip("Authoring help only.")]
                [Range(0, 10)]
                [CustomPresentation]
                public int value { get; set; }
            }
            """);
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult authoring = m_fixture.CompilePending(reload);
        Assert.True(authoring.success, FormatDiagnostics(authoring));
        Assert.True(reload.ApplyPendingReload());
        string authoringAssembly = Path.Combine(authoring.outputDirectory!, "Inno.GameScripts.dll");
        Assert.True(ContainsCustomAttribute(authoringAssembly, nameof(HeaderAttribute)));
        Assert.True(ContainsCustomAttribute(authoringAssembly, nameof(TextAttribute)));
        Assert.True(ContainsCustomAttribute(authoringAssembly, nameof(TooltipAttribute)));
        Assert.True(ContainsCustomAttribute(authoringAssembly, nameof(RangeAttribute)));
        Assert.True(ContainsCustomAttribute(authoringAssembly, "CustomPresentationAttribute"));
        Assert.True(ContainsCustomAttribute(authoringAssembly, nameof(SerializablePropertyAttribute)));
        ScriptCompilationResult player = m_fixture.CompileRuntimeDeployment();
        Assert.True(player.success, FormatDiagnostics(player));
        string playerAssembly = Path.Combine(player.outputDirectory!, "Inno.GameScripts.dll");
        Assert.False(ContainsCustomAttribute(playerAssembly, nameof(HeaderAttribute)));
        Assert.False(ContainsCustomAttribute(playerAssembly, nameof(TextAttribute)));
        Assert.False(ContainsCustomAttribute(playerAssembly, nameof(TooltipAttribute)));
        Assert.False(ContainsCustomAttribute(playerAssembly, nameof(RangeAttribute)));
        Assert.False(ContainsCustomAttribute(playerAssembly, "CustomPresentationAttribute"));
        Assert.True(ContainsCustomAttribute(playerAssembly, nameof(SerializablePropertyAttribute)));
        using FileStream stream = File.OpenRead(playerAssembly);
        using var executable = new PEReader(stream);
        MetadataReader metadata = executable.GetMetadataReader();
        Assert.DoesNotContain(
            metadata.AssemblyReferences,
            handle => metadata.GetString(metadata.GetAssemblyReference(handle).Name).Contains("Editor", StringComparison.Ordinal)
        );
        Assert.DoesNotContain(
            metadata.TypeDefinitions,
            handle => metadata.GetString(metadata.GetTypeDefinition(handle).Name) == "CustomPresentationAttribute"
        );
    }

    [Fact]
    public void RuntimeDeploymentBindsToTargetPlayerAssembliesAndInvalidatesItsCache()
    {
        m_fixture.Write("TargetBoundBehavior.cs", """
            using InnoEngine.Scene;

            public sealed class TargetBoundBehavior : GameBehavior
            {
            }
            """);
        string targetRuntime = m_fixture.CreateDeploymentRuntime();
        ScriptCompilationResult first = m_fixture.CompileRuntimeDeployment(targetRuntime);
        Assert.True(first.success, FormatDiagnostics(first));
        File.Delete(Path.Combine(targetRuntime, "Inno.Scene.dll"));
        ScriptCompilationResult second = m_fixture.CompileRuntimeDeployment(targetRuntime);
        Assert.False(second.success);
        Assert.Contains(second.diagnostics, static diagnostic => diagnostic.severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void CompilationWideUsingDirectiveIsRejected()
    {
        m_fixture.Write("ForbiddenUsing.cs", """
            global using InnoEngine.Scene;

            public sealed class ForbiddenUsingBehavior : GameBehavior
            {
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.False(result.success);
        Assert.Contains(result.diagnostics, static diagnostic => diagnostic.id == "INNO2003");
    }

    [Fact]
    public void RuntimeApiExposesOnlyGameBehaviorAsTheEnabledLifecycleBase()
    {
        m_fixture.Write("RemovedBehaviorBase.cs", """
            using InnoEngine.Scene;

            public sealed class RemovedBehaviorBase : Behavior
            {
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.False(result.success);
        Assert.Contains(result.diagnostics, static diagnostic => diagnostic.id == "CS0246");
    }

    [Fact]
    public void RuntimeSourceCannotReferenceEditorApiButEditorSourceCan()
    {
        const string C_SOURCE = """
            using InnoEditor.Core;

            public sealed class EditorApiProbe
            {
                public EditorContext? context { get; set; }
            }
            """;
        m_fixture.Write("EditorApiProbe.cs", C_SOURCE);
        ScriptCompilationResult runtimeResult = m_fixture.Compile();
        Assert.False(runtimeResult.success);
        m_fixture.Move("EditorApiProbe.cs", "EditorApiProbe.editor.cs");
        ScriptCompilationResult editorResult = m_fixture.Compile();
        Assert.True(editorResult.success, FormatDiagnostics(editorResult));
    }

    [Fact]
    public void AdditionalAttachableTypeWithoutCanonicalSourceFailsCompilation()
    {
        m_fixture.Write("PrimaryProbe.cs", """
            using InnoEngine.Scene;

            public sealed class PrimaryProbe : GameBehavior
            {
            }

            public sealed class SecondaryProbe : GameBehavior
            {
            }
            """);
        ScriptCompilationResult result = m_fixture.Compile();
        Assert.False(result.success);
        Assert.Contains(
            result.diagnostics,
            static diagnostic => (
                diagnostic.id == "INNO2001"
                && diagnostic.severity == DiagnosticSeverity.Error
                && diagnostic.message.Contains("SecondaryProbe", StringComparison.Ordinal)
            )
        );
    }

    [Fact]
    public void CachedCompilationReplaysWarningsAndReusesTheGeneration()
    {
        m_fixture.Write("CachedWarningProbe.cs", "public sealed class CachedWarningProbe { private int unused; }");
        ScriptCompilationResult first = m_fixture.Compile();
        ScriptCompilationResult second = m_fixture.Compile();
        Assert.True(first.success, FormatDiagnostics(first));
        Assert.True(second.success, FormatDiagnostics(second));
        ScriptDiagnostic firstWarning = Assert.Single(
            first.diagnostics,
            static diagnostic => diagnostic.id == "CS0169" && diagnostic.severity == DiagnosticSeverity.Warning
        );
        Assert.Contains(second.diagnostics, diagnostic => diagnostic == firstWarning);
        Assert.Equal(first.outputDirectory, second.outputDirectory);
        Assert.NotEmpty(second.reusedAssemblyNames);
    }

    [Fact]
    public void CompilerProgressIsMonotonicAndIncludesStageTimings()
    {
        m_fixture.Write("ProgressProbe.cs", "public sealed class ProgressProbe { }");
        var progress = new ProgressRecorder();
        ScriptCompilationResult result = m_fixture.Compile(progress);
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.NotEmpty(progress.values);
        Assert.All(progress.values, static value => Assert.InRange(value.fraction, 0f, 1f));
        Assert.True(
            progress.values.Zip(progress.values.Skip(1), static (
                left,
                right
            ) => right.fraction >= left.fraction).All(static value => value)
        );
        Assert.NotEmpty(result.stageTimings);
        Assert.All(result.stageTimings, static timing => Assert.True(timing.elapsed >= TimeSpan.Zero));
    }

    [Fact]
    public void PreCanceledCompilationDoesNotPublishAnArtifactGeneration()
    {
        m_fixture.Write("CanceledProbe.cs", "public sealed class CanceledProbe { }");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => m_fixture.Compile(cancellationToken: cancellation.Token));
        string artifactRoot = Path.Combine(m_fixture.projectRoot, "Library", "Artifacts", "ScriptAssemblies");
        Assert.True(!Directory.Exists(artifactRoot) || !Directory.EnumerateDirectories(artifactRoot).Any());
    }

    [Fact]
    public void LogicalDocumentationProjectsNamespacesWithoutLosingMemberDescriptions()
    {
        m_fixture.compiler.GenerateProjectFiles();
        XDocument project = XDocument.Load(Path.Combine(m_fixture.projectRoot, "Inno.GameScripts.csproj"));
        XElement[] members = project
            .Descendants("HintPath")
            .Select(reference => Path.ChangeExtension(Path.GetFullPath(reference.Value, m_fixture.projectRoot), ".xml"))
            .Where(File.Exists)
            .SelectMany(path => XDocument.Load(path).Descendants("member"))
            .ToArray();
        XElement manager = Assert.Single(members.Where(member => member.Attribute("name")?.Value == "T:InnoEngine.Scene.SceneManager"));
        Assert.Contains("scene operations", manager.Element("summary")!.Value, StringComparison.Ordinal);
        Assert.Contains(manager.Descendants("see"), reference => reference.Attribute("cref")?.Value == "T:InnoEngine.Scene.SceneWorld");
        Assert.Contains(
            members,
            member => (
                member.Attribute("name")?.Value == "P:InnoEngine.Scene.SceneManager.activeScene"
                && member.Element("summary")?.Value.Contains("active scene", StringComparison.Ordinal) == true
            )
        );
        Assert.DoesNotContain(
            members,
            member => member.Attribute("name")?.Value.StartsWith("T:Inno.Scene.", StringComparison.Ordinal) == true
        );
    }

    [Fact]
    public void GenerateProjectFilesUsesTheSameRuntimeAndEditorClassification()
    {
        m_fixture.Write("Runtime.cs", "using InnoEngine.Scene; public sealed class RuntimeScript : GameBehavior { }");
        m_fixture.Write("Tools.editor.cs", "public sealed class EditorScript { }");
        m_fixture.Rescan();
        m_fixture.compiler.GenerateProjectFiles();
        string gameProject = File.ReadAllText(Path.Combine(m_fixture.projectRoot, "Inno.GameScripts.csproj"));
        string editorProject = File.ReadAllText(Path.Combine(m_fixture.projectRoot, "Inno.EditorScripts.csproj"));
        Assert.Contains("Compile Include=\"Assets/Runtime.cs\"", gameProject);
        Assert.DoesNotContain("Tools.editor.cs", gameProject);
        Assert.Contains("Compile Include=\"Assets/Tools.editor.cs\"", editorProject);
        Assert.Contains("Inno.GameScripts.csproj", editorProject);
        Assert.DoesNotContain("<Compile Include=\"Library", gameProject);
        Assert.True(File.Exists(Path.Combine(m_fixture.projectRoot, "InnoProject.sln")));
    }

    [Fact]
    public void ProjectTildeDirectoriesRemainBrowsableAndParticipateInAuthoringCompilation()
    {
        m_fixture.Write("Runtime.cs", "using InnoEngine.Scene; public sealed class RuntimeScript : GameBehavior { }");
        m_fixture.Write("~Examples/Example.cs", "using InnoEngine.Scene; public sealed class ExampleScript : GameBehavior { }");
        m_fixture.Rescan();
        Assert.True(m_fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Examples"), out AssetFileEntry tildeRoot));
        Assert.False(tildeRoot.isSample);
        Assert.False(tildeRoot.isSampleContent);
        Assert.True(m_fixture.assets.TryGetFileSystemEntry(AssetPath.Project("~Examples/Example.cs"), out AssetFileEntry tildeSource));
        Assert.False(tildeSource.isSampleContent);
        Assert.True(m_fixture.assets.TryGetInfo(AssetPath.Project("~Examples/Example.cs"), out _));
        ScriptCompilationResult compilation = m_fixture.Compile();
        Assert.True(compilation.success, FormatDiagnostics(compilation));
        m_fixture.compiler.GenerateProjectFiles();
        string gameProject = File.ReadAllText(Path.Combine(m_fixture.projectRoot, "Inno.GameScripts.csproj"));
        Assert.Contains("Compile Include=\"Assets/Runtime.cs\"", gameProject);
        Assert.Contains("Compile Include=\"Assets/~Examples/Example.cs\"", gameProject);
    }

    [Fact]
    public void IdeProjectionUsesPluginMetadataReferencesWithoutExtraProjects()
    {
        using var fixture = new ScriptingFixture(WriteProjectionPlugin);
        fixture.Write("UsesProjectionPlugin.cs", """
            using ProjectionPlugin;

            public sealed class UsesProjectionPlugin
            {
                public ProjectionRuntime? runtime { get; set; }
            }
            """);
        string staleProject = Path.Combine(fixture.projectRoot, "Inno.Plugin.Stale.csproj");
        File.WriteAllText(staleProject, "stale");
        ScriptCompilationResult result = fixture.Compile();
        fixture.compiler.GenerateProjectFiles();
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.False(File.Exists(staleProject));
        Assert.False(File.Exists(Path.Combine(fixture.projectRoot, "Inno.Plugin.TestsProjection.csproj")));
        string solution = File.ReadAllText(Path.Combine(fixture.projectRoot, "InnoProject.sln"));
        Assert.DoesNotContain("Inno.Plugin.TestsProjection", solution, StringComparison.Ordinal);
        string gameProject = File.ReadAllText(Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj"));
        Assert.Contains("Inno.Plugin.TestsProjection.dll", gameProject, StringComparison.Ordinal);
        Assert.DoesNotContain("ProjectReference Include=\"Inno.Plugin.TestsProjection.csproj\"", gameProject);
    }

    [Fact]
    public void SourceArtifactsAreCatalogedButRuntimeExportContainsNoSourceFiles()
    {
        m_fixture.Write("Scripts/Tracked.cs", "using InnoEngine.Scene; public sealed class Tracked : GameBehavior { }");
        ScriptCompilationResult result = m_fixture.Compile();
        string contentRoot = Path.Combine(m_fixture.projectRoot, "RuntimeContent");
        AssetRuntimeContentInfo exported = m_fixture.assets.ExportRuntimeArtifacts(contentRoot);
        Assert.True(result.success, FormatDiagnostics(result));
        Assert.True(m_fixture.assets.TryGetInfo(AssetPath.Project("Scripts/Tracked.cs"), out AssetInfo? info));
        Assert.NotNull(info);
        Assert.Equal("inno.editor.csharp-script", info.importerID);
        Assert.True(m_fixture.assets.TryGetArtifact(info.persistentID, "source", out AssetArtifactInfo? source));
        Assert.NotNull(source);
        using (ArtifactLease encoded = m_fixture.assets.AcquireArtifact(info.persistentID, "source"))
        {
            Assert.NotEmpty(encoded.ReadAllBytes());
        }
        Assert.Equal(0, exported.assetCount);
        Assert.DoesNotContain(
            Directory.EnumerateFiles(contentRoot, "*", SearchOption.AllDirectories),
            static path => path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
        );
    }

    [Fact]
    public void FailedResultHasNoActivationOrRuntimeArtifacts()
    {
        ScriptCompilationResult result = ScriptCompilationResult.Failure(
            new ScriptDiagnostic("INNO-TEST", DiagnosticSeverity.Error, "Injected compilation failure.", filePath: null, line: 0, column: 0)
        );
        Assert.False(result.success);
        Assert.Null(result.outputDirectory);
        Assert.Empty(result.moduleDeployments);
        Assert.Empty(result.runtimeAssemblyPaths);
        Assert.Empty(result.compiledAssemblyNames);
        Assert.Empty(result.reusedAssemblyNames);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static AssetPath InvokePublicPathProbe(string assemblyPath)
    {
        var lifetime = new AssemblyLoadContext("source-local-path-test", isCollectible: true);
        try
        {
            using var stream = new MemoryStream(File.ReadAllBytes(assemblyPath));
            Assembly assembly = lifetime.LoadFromStream(stream);
            Func<AssetPath> resolve = assembly.GetType("SourceLocalPathProbe", throwOnError: true)!.GetMethod(
                "Resolve",
                BindingFlags.Public | BindingFlags.Static
            )!.CreateDelegate<Func<AssetPath>>();
            return resolve();
        }
        finally
        {
            lifetime.Unload();
        }
    }

    private static void WriteProjectionPlugin(
        string projectRoot,
        SerializationRegistry serialization
    ) {
        const string C_SOURCE = """
            using InnoEngine.Scene;

            namespace ProjectionPlugin;

            public sealed class ProjectionRuntime : GameBehavior
            {
            }
            """;
        WritePluginPackage(
            projectRoot,
            "projection.iplugin",
            serialization,
            new PluginManifest { pluginId = "tests.projection", displayName = "Projection Plugin" },
            new Dictionary<string, byte[]>
            {
                ["Assets/ProjectionRuntime.cs"] = Encoding.UTF8.GetBytes(C_SOURCE),
                ["Assets/ProjectionRuntime.cs.imeta"] = CreateScriptSourceMeta(serialization, Guid.NewGuid())
            }
        );
    }

    private static bool ContainsCustomAttribute(
        string assemblyPath,
        string attributeTypeName
    ) {
        using FileStream stream = File.OpenRead(assemblyPath);
        using var portableExecutable = new PEReader(stream);
        MetadataReader metadata = portableExecutable.GetMetadataReader();
        foreach (CustomAttributeHandle handle in metadata.CustomAttributes)
        {
            CustomAttribute attribute = metadata.GetCustomAttribute(handle);
            string? typeName = attribute.Constructor.Kind switch
            {
                HandleKind.MemberReference => GetMemberReferenceDeclaringTypeName(metadata, (MemberReferenceHandle)attribute.Constructor),
                HandleKind.MethodDefinition => metadata.GetString(
                    metadata.GetTypeDefinition(
                        metadata.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor).GetDeclaringType()
                    ).Name
                ),
                _ => null
            };
            if (string.Equals(typeName, attributeTypeName, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static string? GetMemberReferenceDeclaringTypeName(
        MetadataReader metadata,
        MemberReferenceHandle constructorHandle
    ) {
        MemberReference constructor = metadata.GetMemberReference(constructorHandle);
        return constructor.Parent.Kind switch
        {
            HandleKind.TypeReference => metadata.GetString(metadata.GetTypeReference((TypeReferenceHandle)constructor.Parent).Name),
            HandleKind.TypeDefinition => metadata.GetString(metadata.GetTypeDefinition((TypeDefinitionHandle)constructor.Parent).Name),
            _ => null
        };
    }

    private sealed class ProgressRecorder : IProgress<ScriptCompilationProgress>
    {
        public void Report(ScriptCompilationProgress value) => values.Add(value);

        internal List<ScriptCompilationProgress> values { get; } = [];
    }
}
