using System;
using System.Collections.Generic;
using Xunit;
using Inno.Assets;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Runtime.Contracts;
using Inno.Text;
using Inno.UI.Runtime;

namespace Inno.UI.Tests;

public sealed class UIRuntimeTests
{
    [Fact]
    public void FrameScopeCapturesInputAndBindsTheScriptFacade()
    {
        var backend = new CapturingBackend();
        using var runtime = new UIRuntime(backend, new RejectingArtifacts());
        var snapshot = new InputSnapshot(
            7,
            keysPressed: [KeyCode.Enter],
            mouseButtonsReleased: [MouseButton.Left],
            mousePosition: new Vector2(12f, 34f),
            scrollDelta: new Vector2(0f, 2f),
            modifiers: KeyModifier.Control,
            textInput: ["界"]
        );
        using IDisposable input = InputExecutionContext.EnterScope(new FixedInput(snapshot));
        var frame = new RuntimeFrame(7, 1f, 1f, 0.016f, 0.016f, 1f, false);
        runtime.Attach();

        runtime.BeginFrame(frame);
        UI.Update(new UIContextHandle(10));

        UIInputSnapshot captured = Assert.IsType<UIInputSnapshot>(backend.lastInput);
        Assert.Equal(new Vector2(12f, 34f), captured.mousePosition);
        Assert.Contains(KeyCode.Enter, captured.keysPressed);
        Assert.Contains(MouseButton.Left, captured.buttonsReleased);
        Assert.Equal("界", Assert.Single(captured.textInput));
        runtime.EndFrame(frame);
        Assert.Throws<InvalidOperationException>(() => _ = UIExecutionContext.current);
    }

    private sealed class FixedInput(InputSnapshot snapshot) : IInputService
    {
        public InputSnapshot snapshot { get; } = snapshot;
    }

    private sealed class RejectingArtifacts : IAssetArtifactLookup
    {
        public ArtifactLease AcquireArtifact(
            Guid persistentID,
            string outputName
        ) => throw new InvalidOperationException("No asset access was expected.");

        public bool TryGetArtifact(
            Guid persistentID,
            string outputName,
            out AssetArtifactInfo? artifact
        ) {
            artifact = null;
            return false;
        }
    }

    private sealed class CapturingBackend : IUIBackend
    {
        public string implementationID => "tests.ui";
        public UIBackendCapabilities capabilities { get; } = new([new UIDocumentLanguageID("tests.ui-language")], true);

        public UIContextHandle CreateContext(UIContextOptions options) => new(1);

        public void DestroyContext(UIContextHandle context) {}

        public void SetViewport(
            UIContextHandle context,
            int width,
            int height,
            float density
        ) {}

        public UIDocumentHandle LoadDocument(
            UIContextHandle context,
            UIDocumentSource source
        ) => new(1);

        public void ShowDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public void HideDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public void CloseDocument(
            UIContextHandle context,
            UIDocumentHandle document
        ) {}

        public bool SetText(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string text
        ) => true;

        public bool SetContent(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            UIDocumentFragment content
        ) => true;

        public bool SetAttribute(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string name,
            string value
        ) => true;

        public bool SetClass(
            UIContextHandle context,
            UIDocumentHandle document,
            string elementID,
            string className,
            bool active
        ) => true;

        public void RegisterFont(UIFontRegistration registration) {}

        public void RegisterTexture(
            UIContextHandle context,
            string source,
            UITextureData texture
        ) {}

        public bool HasElementAtPoint(
            UIContextHandle context,
            Vector2 position
        ) => false;

        public void Update(
            UIContextHandle context,
            UIInputSnapshot input
        ) => lastInput = input;

        public UIRenderFrame Render(UIContextHandle context) => UIRenderFrame.empty;

        public IReadOnlyList<UIEvent> DrainEvents(UIContextHandle context) => [];

        public void Dispose() {}

        internal UIInputSnapshot? lastInput { get; private set; }
    }
}
