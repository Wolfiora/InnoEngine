using System;
using System.IO;
using System.Numerics;
using System.Threading;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Xunit;
using Xunit.Abstractions;
using System.Text.Json;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class DynamicModalTests
{
    private readonly ITestOutputHelper m_output;
    public DynamicModalTests(ITestOutputHelper output) => m_output = output;
    [Fact]
    [Trait("Acceptance", "Visual")]
    public void ChangedContentFitsOnTheTransitionFrameThroughTheActualRenderer()
    {
        using var host = new ModalHost();
        host.Frame();
        host.Frame();
        float shortHeight = host.state.windowSize.Y;
        host.state.message = "First line\nSecond line\nThird line\nFourth line\nFifth line";
        host.Frame();
        Assert.True(host.state.windowSize.Y > shortHeight + 20f);
        Assert.True(host.state.actionMax.Y <= host.state.windowMax.Y, "The current action was clipped by the previous message height.");
        host.state.message = "Short";
        host.Frame();
        Assert.Equal(shortHeight, host.state.windowSize.Y);
        Assert.Equal(host.state.draws, host.state.preparations);
    }

    [Theory]
    [InlineData(1280, 720)]
    [InlineData(1920, 1080)]
    [InlineData(640, 480)]
    [Trait("Acceptance", "Visual")]
    public void ScaleMatrixBoundsCurrentTextAndActions(
        int width,
        int height
    ) {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.9f, 1f, 1.1f, 1.25f, 1.5f })
        foreach (float dpi in new[] { 1f, 1.25f, 1.5f, 2f })
        {
            using var host = new ModalHost(width, height);
            host.state.zoom = zoom;
            NativeGui.Raw.GetStyle()->FontScaleDpi = dpi;
            host.Frame();
            host.Frame();
            string[] messages = ["Short", "First\nSecond", new string('W', 600), "中文状态 / café / Ελληνικά\n" + new string('x', 4000), "", "Short"];
            foreach (string text in messages)
            {
                host.state.message = text;
                host.Frame();
                m_output.WriteLine(JsonSerializer.Serialize(new
                {
                    width, height, zoom, dpi, messageLength = text.Length,
                    wrapWidth = host.state.wrapWidth, bodyHeight = host.state.bodyHeight,
                    bodyContentHeight = host.state.bodyContentHeight,
                    windowWidth = host.state.windowSize.X, windowHeight = host.state.windowSize.Y,
                    actionMinimumX = host.state.actionMin.X, actionMinimumY = host.state.actionMin.Y,
                    actionMaximumX = host.state.actionMax.X, actionMaximumY = host.state.actionMax.Y,
                    scrollMaxX = host.state.scrollMaxX
                }));
                Assert.True(host.state.windowSize.X <= width && host.state.windowSize.Y <= height);
                Assert.True(host.state.actionMax.Y <= host.state.windowMax.Y + 1f,
                    $"Action exceeded the window: zoom={zoom}, dpi={dpi}, message length={text.Length}.");
                Assert.InRange(host.state.scrollMaxX, 0f, 0.01f);
                Vector2 first = host.state.windowSize;
                host.Frame();
                Assert.Equal(first, host.state.windowSize);
            }
        }
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void ReopeningAndFailuresPreserveTheOwnedNativeScopes()
    {
        using var host = new ModalHost();
        for (int cycle = 0; cycle < 100; cycle++)
        {
            host.state.visible = true;
            host.Frame();
            host.state.visible = false;
            host.Frame();
            Thread.Sleep(510);
            host.Frame();
            Assert.False(host.state.popupOpen);
        }
        host.state.visible = true;
        host.state.rejectPreparation = true;
        int draws = host.state.draws;
        host.Frame();
        Assert.Equal(draws, host.state.draws);
        host.state.rejectPreparation = false;
        host.Refresh();
        host.Frame();
        Assert.True(host.state.draws > draws);
        host.state.rejectDraw = true;
        host.Frame();
        host.state.rejectDraw = false;
        host.Refresh();
        host.Frame();
    }

    [Fact]
    [Trait("Acceptance", "Visual")]
    public void CurrentActionRectangleCancelsOnceAndBecomesDisabled()
    {
        using var host = new ModalHost();
        host.Frame();
        host.Frame();
        host.state.message = "Line one\nLine two\nLine three";
        host.Frame();
        Vector2 hit = (host.state.actionMin + host.state.actionMax) * 0.5f;
        var io = NativeGui.Raw.GetIO();
        NativeGui.Raw.AddMousePosEvent(io, hit.X, hit.Y);
        NativeGui.Raw.AddMouseButtonEvent(io, 0, 1);
        host.Frame();
        NativeGui.Raw.AddMouseButtonEvent(io, 0, 0);
        host.Frame();
        Assert.Equal(1, host.state.actions);
        NativeGui.Raw.AddMouseButtonEvent(io, 0, 1);
        host.Frame();
        NativeGui.Raw.AddMouseButtonEvent(io, 0, 0);
        host.Frame();
        Assert.Equal(1, host.state.actions);
    }

    [Fact]
    [Trait("Acceptance", "Visual")]
    public void DetailsActivityAndActionChangesUseTheCurrentFrameLayout()
    {
        using var host = new ModalHost();
        host.Frame();
        host.Frame();
        host.state.details = new string('D', 8000);
        host.state.indeterminate = true;
        host.Frame();
        Assert.True(host.state.bodyContentHeight > host.state.bodyHeight);
        Assert.True(host.state.actionMax.Y <= host.state.windowMax.Y);
        host.state.details = null;
        host.state.showAction = false;
        host.state.showProgress = false;
        host.state.indeterminate = false;
        host.state.message = string.Empty;
        host.Frame();
        Assert.Equal(0f, host.state.layout.size.Y);
        host.state.showAction = true;
        host.state.message = "Cancel using the keyboard";
        host.Frame();
        var io = NativeGui.Raw.GetIO();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Escape, 1);
        host.Frame();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Escape, 0);
        host.Frame();
        Assert.Equal(1, host.state.actions);
    }

    [Fact]
    [Trait("Acceptance", "Visual")]
    public void TabAndEnterActivateTheCurrentEnabledAction()
    {
        using var host = new ModalHost();
        var io = NativeGui.Raw.GetIO();
        io->ConfigFlags |= (int)Inno.Native.ImGui.ImGuiConfigFlags.NavEnableKeyboard;
        host.Frame();
        host.Frame();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Tab, 1);
        host.Frame();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Tab, 0);
        host.Frame();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Enter, 1);
        host.Frame();
        NativeGui.Raw.AddKeyEvent(io, Inno.Native.ImGui.ImGuiKey.Enter, 0);
        host.Frame();
        Assert.Equal(1, host.state.actions);
    }

    [Fact]
    [Trait("Acceptance", "Visual")]
    public void ExplicitWidthAndResizableWindowPolicyArePreserved()
    {
        using var host = new ModalHost();
        host.state.initialSize = new(680, 0);
        host.Frame();
        host.Frame();
        Assert.Equal(680, host.state.windowSize.X);
        host.state.initialSize = new(600, 300);
        host.state.resizable = true;
        host.state.visible = false;
        host.Frame();
        Thread.Sleep(510);
        host.Frame();
        host.state.visible = true;
        host.Frame();
        host.Frame();
        Vector2 fixedSize = host.state.windowSize;
        host.state.message = new string('W', 1000);
        host.Frame();
        Assert.Equal(fixedSize, host.state.windowSize);
        Assert.True(host.state.actionMax.Y <= host.state.windowMax.Y);
    }

    [Fact]
    public void FontAndZoomChangesInvalidatePreparedMeasurementsInTheSameSession()
    {
        using var host = new ModalHost();
        host.state.message = new string('W', 60);
        host.Frame();
        host.Frame();
        float originalActionWidth = host.state.actionMax.X - host.state.actionMin.X;
        float originalHeight = host.state.layout.size.Y;
        var io = NativeGui.Raw.GetIO();
        var configuration = NativeGui.Raw.ImFontConfig();
        try
        {
            configuration->SizePixels = 16f;
            configuration->GlyphExtraAdvanceX = 8f;
            io->FontDefault = NativeGui.Raw.AddFontDefault(io->Fonts, configuration);
            Assert.True(io->FontDefault != null);
        }
        finally
        {
            NativeGui.Raw.Destroy(configuration);
        }
        host.Frame();
        host.Frame();
        Assert.True(host.state.actionMax.X - host.state.actionMin.X > originalActionWidth);
        Assert.True(host.state.layout.size.Y > originalHeight);
        host.state.zoom = 1.5f;
        host.Frame();
        host.Frame();
        Assert.Equal(690f, host.state.windowSize.X);
        Assert.True(host.state.actionMax.Y <= host.state.windowMax.Y);
    }

    [Fact]
    public void InvalidWindowDimensionsAreQuarantinedBeforeNativeLayout()
    {
        using var host = new ModalHost();
        host.state.initialSize = new(float.NaN, 0f);
        host.Frame();
        Assert.Equal(0, host.state.draws);
        Assert.Equal(0, host.state.preparations);
        host.state.initialSize = Vector2.Zero;
        host.Refresh();
        host.Frame();
        Assert.True(host.state.draws > 0);
    }

    [Fact]
    public void InvalidLiteralTextFailsWithoutCorruptingTheNextNativeFrame()
    {
        using var host = new ModalHost();
        host.Frame();
        foreach (string text in new[] { "zero\0text", "invalid\ud800" })
        {
            int draws = host.state.draws;
            host.state.message = text;
            host.Frame();
            Assert.Equal(draws, host.state.draws);
            host.state.message = "Valid text";
            host.Refresh();
            host.Frame();
            Assert.True(host.state.draws > draws);
        }
    }

    [Fact]
    public void IndependentSessionsDoNotSharePreparedDialogContent()
    {
        using var first = new ModalHost();
        using var second = new ModalHost();
        first.state.message = "First session";
        second.state.message = "Second session\nWith another line";
        first.Frame();
        second.Frame();
        first.Frame();
        Assert.NotEqual(first.state.windowSize.Y, second.state.windowSize.Y);
        Assert.Equal("First session", first.state.content.message);
        Assert.Equal("Second session\nWith another line", second.state.content.message);
    }

    private sealed class ModalHost : IDisposable
    {
        private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoModalTests", Guid.NewGuid().ToString("N"));
        private readonly ImGuiNativeContext* m_native;
        private readonly ModuleHost m_modules;
        private readonly TypeCatalog m_types;
        private readonly LogRouter m_logs = new();
        private readonly ImGuiEditorRuntime m_runtime;
        internal readonly DynamicModalState state = new();

        internal ModalHost(
            int width = 1280,
            int height = 720
        ) {
            Directory.CreateDirectory(m_root);
            m_native = NativeGui.Raw.CreateContext(null);
            NativeGui.Raw.SetCurrentContext(m_native);
            var io = NativeGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = width, Y = height };
            io->DeltaTime = 0.2f;
            io->IniFilename = null;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            m_modules = new ModuleHost(new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(DynamicModalTests).Assembly) });
            m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
            m_runtime = new ImGuiEditorRuntime(new EditorContext(m_root, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl")),
                m_types, m_logs, [state]);
            m_runtime.Start();
        }

        internal void Refresh() => m_types.Rebuild();

        internal void Frame()
        {
            NativeGui.Raw.SetCurrentContext(m_native);
            m_runtime.PrepareFrame();
            NativeGui.NewFrame();
            m_runtime.Draw();
            state.popupOpen = NativeGui.IsPopupOpen("Dynamic Modal##tests.dynamic-modal");
            NativeGui.Render();
        }

        public void Dispose()
        {
            NativeGui.Raw.SetCurrentContext(m_native);
            m_runtime.Dispose();
            m_types.Dispose();
            m_modules.Dispose();
            m_logs.Dispose();
            NativeGui.Raw.DestroyContext(m_native);
            Directory.Delete(m_root, true);
        }
    }
}

public sealed class DynamicModalState
{
    public string message = "Short";
    public float zoom = 1f;
    public bool showAction = true;
    public bool showProgress = true;
    public bool indeterminate;
    public string? details;
    public Vector2 initialSize;
    public bool resizable;
    public bool visible = true;
    public bool rejectPreparation;
    public bool rejectDraw;
    public bool canceled;
    public int actions;
    public bool popupOpen;
    public Vector2 actionMin;
    public float scrollMaxX;
    public float wrapWidth;
    public float bodyHeight;
    public float bodyContentHeight;
    public int preparations;
    public int draws;
    public DialogPresentation content;
    public DialogLayout layout;
    public Vector2 windowSize;
    public Vector2 windowMax;
    public Vector2 actionMax;
}

[EditorModal("tests.dynamic-modal", "Dynamic Modal")]
public sealed unsafe class DynamicTestModal(DynamicModalState state) : EditorModal
{
    public override bool isVisible => state.visible;
    public override bool allowScrolling => false;
    public override Vector2 initialSize => state.initialSize;
    public override bool canResize => state.resizable;
    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        state.preparations++;
        if (state.rejectPreparation) throw new InvalidOperationException("Rejected modal measurement.");
        EditorUiSession.current.widgets.style.SetZoom(state.zoom);
        state.content = new(state.message, state.showProgress ? 0.5f : null, state.indeterminate,
            actionLabel: state.showAction ? "Cancel" : null, actionEnabled: !state.canceled, details: state.details, cancelOnEscape: true);
        state.layout = EditorUiSession.current.widgets.MeasureDialog(state.content, availableContentSize);
        return state.layout.size;
    }

    protected override void OnDraw(EditorContext context)
    {
        state.draws++;
        state.windowSize = NativeGui.GetWindowSize();
        state.windowMax = NativeGui.GetWindowPos() + state.windowSize;
        if (EditorUiSession.current.widgets.DrawDialog(state.content, state.layout))
        {
            state.actions++;
            state.canceled = true;
        }
        state.actionMin = NativeGui.GetItemRectMin();
        state.scrollMaxX = NativeGui.GetScrollMaxX();
        var window = NativeGui.Raw.GetCurrentWindow();
        state.wrapWidth = NativeGui.GetContentRegionAvail().X;
        state.bodyHeight = state.message.Length == 0 ? 0f : NativeGui.CalcTextSize(state.message, false, state.wrapWidth).Y;
        state.bodyContentHeight = state.bodyHeight;
        if (window->DC.ChildWindows.Size > 0)
        {
            var child = window->DC.ChildWindows.Data[0];
            state.wrapWidth = child->WorkRect.Max.X - child->WorkRect.Min.X;
            state.bodyHeight = child->Size.Y;
            state.bodyContentHeight = child->ContentSize.Y;
        }
        if (state.rejectDraw) throw new InvalidOperationException("Rejected modal drawing.");
        state.actionMax = NativeGui.GetItemRectMax();
    }
}
