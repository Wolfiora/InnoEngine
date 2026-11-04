using System;
using System.Collections.Generic;

using Inno.Assets;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Runtime.Contracts;
using Inno.Text;
using Inno.UI.Runtime;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiRuntimeTests
{
    [Fact]
    public void FrameScopeCapturesInputAndBindsTheScriptFacade()
    {
        var backend = new CapturingBackend();
        using var runtime = new UiRuntime(backend, new RejectingArtifacts());
        InputSnapshot snapshot = new(
            7,
            new InputSequence([
                new KeyInputChange(KeyCode.Enter, true, false, KeyModifier.Control),
                new PointerButtonInputChange(new Vector2(12f, 34f), MouseButton.Left, false, KeyModifier.Control),
                new PointerScrollInputChange(new Vector2(12f, 34f), new Vector2(0f, 2f), KeyModifier.Control),
                new TextInputChange("界", KeyModifier.Control)
            ]),
            mousePosition: new Vector2(12f, 34f),
            modifiers: KeyModifier.Control);
        using IDisposable input = InputExecutionContext.EnterScope(new FixedInput(snapshot));
        var frame = new RuntimeFrame(7, 1f, 1f, 0.016f, 0.016f, 1f, false);
        runtime.Attach();

        runtime.BeginFrame(frame);
        Inno.UI.UI.Update(new UiContextHandle(10));

        UiInputSnapshot captured = Assert.IsType<UiInputSnapshot>(backend.lastInput);
        Assert.Equal(new Vector2(12f, 34f), captured.mousePosition);
        Assert.Contains(KeyCode.Enter, captured.keysPressed);
        Assert.Contains(MouseButton.Left, captured.buttonsReleased);
        Assert.Equal("界", Assert.Single(captured.textInput));
        runtime.EndFrame(frame);
        Assert.Throws<InvalidOperationException>(() => _ = UiExecutionContext.current);
    }

    private sealed class FixedInput(InputSnapshot snapshot) : IInputService
    {
        public InputSnapshot snapshot { get; } = snapshot;
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
        internal Action? stopping;
        internal Func<UiContextHandle, UiImageSource, IUiImageRegistration>? registerImage;
        internal int destroyedContexts;
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
        public void DestroyContext(UiContextHandle context) => destroyedContexts++;
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
        ) => registerImage?.Invoke(context, image) ?? throw new NotSupportedException();
        public void RegisterTexture(UiContextHandle context, string source, UiTextureData texture) { }
        public bool HasElementAtPoint(UiContextHandle context, Inno.Core.Mathematics.Vector2 position) => false;
        public void Update(UiContextHandle context, UiInputSnapshot input) => lastInput = input;
        public UiRenderFrame Render(UiContextHandle context) => UiRenderFrame.empty;
        public IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context) => [];
        public void Dispose() => stopping?.Invoke();
    }
}
