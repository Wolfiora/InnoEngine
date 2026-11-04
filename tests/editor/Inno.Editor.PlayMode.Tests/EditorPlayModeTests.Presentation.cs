using System;
using System.Collections.Generic;
using Inno.Adapter.Input;
using Inno.Assets;
using Inno.Audio;
using Inno.Core.Events;
using Inno.Core.Execution;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Audio;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scripting;
using Inno.Input;
using Inno.Input.Runtime;
using Inno.Runtime;
using Inno.Runtime.Contracts;
using Inno.UI;
using Inno.UI.Runtime;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed partial class EditorPlayModeTests
{
    [Fact]
    public void PresentationScopeUsesThePlaySnapshotAndRestoresTheSurroundingInputOwner()
    {
        using var source = new EventInputSource(7);
        using var audio = new PresentationAudio();
        var loop = new EditorPlayModeLoop();
        var options = new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Play,
            applicationId = "inno.tests.presentation",
            createLogSink = _ => new FileLogSink(System.IO.Path.Combine(m_projectRoot, "PresentationLogs")),
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createSubsystems = _ => [
                new InputRuntimeFactory(_ => source.CreateBackend()),
                new UiRuntimeFactory(_ => new UiRuntime(new CapturingBackend(), new RejectingArtifacts()))
            ]
        };
        m_engineHost.types.Rebuild();
        using var runtime = new EditorInteractionRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super")),
            m_engineHost.types,
            m_engineHost.logs,
            [loop, m_engineHost, options, new FakeScenePlayMode(), new FakeScriptCompilation(EditorScriptCompilationState.Ready), new EditorReloadCoordinator(), audio]);
        runtime.Start();
        Assert.True(runtime.interactions.TryGetModule<PresentationConsumer>(out var consumer));
        Assert.NotNull(consumer);
        IEditorPlayMode playMode = consumer.playMode;
        var outer = new PresentationInput();
        using IDisposable inputScope = InputExecutionContext.EnterScope(outer);
        using (loop.EnterPresentationScope())
            Assert.Same(outer, InputExecutionContext.current);
        Assert.True(playMode.EnterPlayMode());
        for (int frame = 0; frame < 8 && !playMode.isPlaying; frame++)
            runtime.Update(new EditorFrame(.016f, frame * .016f, true));
        Assert.True(playMode.isPlaying, playMode.lastFailure);
        source.ProcessEvent(new TextInputEvent(7, "presentation"));
        loop.Tick(.016f);
        Assert.Same(outer, InputExecutionContext.current);
        using (loop.EnterPresentationScope())
        {
            IInputService playing = InputExecutionContext.current;
            Assert.NotSame(outer, playing);
            Assert.Equal("presentation", Assert.Single(playing.snapshot.textInput));
            using (loop.EnterPresentationScope())
                Assert.Same(playing, InputExecutionContext.current);
            Assert.Same(playing, InputExecutionContext.current);
        }
        Assert.Same(outer, InputExecutionContext.current);
        IDisposable first = loop.EnterPresentationScope();
        IDisposable second = loop.EnterPresentationScope();
        Assert.Throws<InvalidOperationException>(first.Dispose);
        Assert.Equal("presentation", Assert.Single(InputExecutionContext.current.snapshot.textInput));
        second.Dispose();
        first.Dispose();
        Assert.Same(outer, InputExecutionContext.current);
        Assert.True(playMode.ExitPlayMode());
        loop.Quiesce();
        using (loop.EnterPresentationScope())
            Assert.Same(outer, InputExecutionContext.current);
    }

    [EditorModule("tests.play-presentation-consumer")]
    private sealed class PresentationConsumer(IEditorPlayMode playMode) : EditorModule
    {
        internal IEditorPlayMode playMode { get; } = playMode;
    }

    private sealed class PresentationInput : IInputService
    {
        public InputSnapshot snapshot => InputSnapshot.empty;
    }

    private sealed class PresentationAudio : IEditorAudioHost
    {
        public IRuntimeSubsystemFactory CreateRuntimeSubsystemFactory(RuntimeSession session) => throw new NotSupportedException();
        public IDisposable EnterExecutionScope(RuntimeSession session) => new LifetimeScope();
        public AudioVoiceHandle PlayPreview(
            RuntimeSession session,
            AudioClipAsset clip,
            AudioPlayOptions? options = null
        ) => throw new NotSupportedException();
        public bool StopPreview(
            RuntimeSession session,
            AudioVoiceHandle voice
        ) => throw new NotSupportedException();
        public void Dispose() { }
    }

    private sealed class RejectingArtifacts : IAssetArtifactLookup
    {
        public ArtifactLease AcquireArtifact(Guid persistentId, string outputName)
            => throw new InvalidOperationException("No asset access was expected.");

        public bool TryGetArtifact(Guid persistentId, string outputName, out AssetArtifactInfo? artifact)
        {
            artifact = null;
            return false;
        }
    }

    private sealed class CapturingBackend : IUiBackend
    {
        public UiLayoutSnapshot CaptureLayout(
            UiContextHandle context,
            UiDocumentHandle document,
            IReadOnlyList<string> elementIds
        ) => throw new NotSupportedException();

        internal UiInputSnapshot? lastInput { get; private set; }

        public string implementationId => "tests.ui";
        public UiBackendCapabilities capabilities { get; } = new([new("tests.ui-language")], [UiBackendCapability.FontCollectionFaceSelection]);

        /// <inheritdoc cref="IUiService.GetTime" />
        public UiContextTime GetTime(UiContextHandle context) => throw new NotSupportedException();

        /// <inheritdoc cref="IUiService.SetTimePaused" />
        public void SetTimePaused(
            UiContextHandle context,
            bool paused
        ) => throw new NotSupportedException();

        /// <inheritdoc cref="IUiService.AdvanceTime" />
        public void AdvanceTime(
            UiContextHandle context,
            double seconds
        ) => throw new NotSupportedException();

        /// <inheritdoc cref="IUiService.TrySeekTime" />
        public bool TrySeekTime(
            UiContextHandle context,
            double seconds
        ) => throw new NotSupportedException();

        /// <inheritdoc cref="IUiService.RefreshLayout" />
        public void RefreshLayout(UiContextHandle context) => throw new NotSupportedException();

        /// <inheritdoc cref="IUiService.ProcessInput" />
        public void ProcessInput(
            UiContextHandle context,
            UiInputSnapshot input
        ) => throw new NotSupportedException();

        public UiControlValue GetValue(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId
        ) => throw new NotSupportedException();

        public bool SetValue(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId,
            UiControlValue value
        ) => throw new NotSupportedException();

        public UiFocusResult FocusElement(
            UiContextHandle context,
            UiDocumentHandle document,
            string elementId
        ) => throw new NotSupportedException();
        public UiFocusTarget? GetFocusedElement(UiContextHandle context) => throw new NotSupportedException();
        public bool ClearFocus(UiContextHandle context) => throw new NotSupportedException();
        public UiContextHandle CreateContext(UiContextOptions options) => new(1);
        public void DestroyContext(UiContextHandle context) { }
        public void SetViewport(UiContextHandle context, int width, int height, float density) { }
        public UiDocumentHandle LoadDocument(UiContextHandle context, UiDocumentSource source) => new(1);
        public void ShowDocument(UiContextHandle context, UiDocumentHandle document) { }
        public void HideDocument(UiContextHandle context, UiDocumentHandle document) { }
        public void CloseDocument(UiContextHandle context, UiDocumentHandle document) { }
        public bool SetText(UiContextHandle context, UiDocumentHandle document, string elementId, string text) => true;
        public bool SetContent(UiContextHandle context, UiDocumentHandle document, string elementId, UiDocumentFragment content) => true;
        public bool SetAttribute(UiContextHandle context, UiDocumentHandle document, string elementId, string name, string value) => true;
        public bool SetClass(UiContextHandle context, UiDocumentHandle document, string elementId, string className, bool active) => true;
        public void RegisterFont(UiFontRegistration registration) { }
        public IUiImageRegistration RegisterImage(
            UiContextHandle context,
            UiImageSource image
        ) => throw new NotSupportedException();
        public void RegisterTexture(UiContextHandle context, string source, UiTextureData texture) { }
        public bool HasElementAtPoint(UiContextHandle context, Inno.Core.Mathematics.Vector2 position) => false;
        public void Update(UiContextHandle context, UiInputSnapshot input) => lastInput = input;
        public UiRenderFrame Render(UiContextHandle context) => UiRenderFrame.empty;
        public IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context) => [];
        public void Dispose() { }
    }
}
