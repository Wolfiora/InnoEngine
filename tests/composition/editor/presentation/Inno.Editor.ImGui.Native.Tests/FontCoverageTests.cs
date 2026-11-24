using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Native.ImGui;
using Inno.Platform;
using Inno.Testing.Sdl3;

using BackendFont = Inno.Adapter.Presentation.ImGui.ImGuiFont;
using BackendFontScope = Inno.Adapter.Presentation.ImGui.ImGuiFontScope;
using BackendFontStyle = Inno.Adapter.Presentation.ImGui.ImGuiFontStyle;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed unsafe class FontCoverageTests
{
    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    [InlineData(2f)]
    [Trait("Acceptance", "Visual")]
    public void EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback(float zoom)
    {
        string contract = nameof(FontCoverageTests)
                          + "."
                          + nameof(EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback)
                          + "."
                          + zoom.ToString(CultureInfo.InvariantCulture);
        if (NativeWindowTestHost.RunIfRequired(contract))
        {
            return;
        }
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = application.CreateWindow(
            new PlatformWindowOptions { title = "Glyph coverage", width = 640, height = 480, visible = false }
        );
        using PlatformImGuiContext context = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        context.SetIniFile(null);
        context.RenderFrame(() =>
        {
            NativeImGui.Raw.GetStyle()->FontScaleMain = zoom;
            bool open = true;
            _ = NativeImGui.Begin("Glyph coverage", ref open, 0);
            try
            {
                foreach (BackendFontStyle style in new[]
                {
                    BackendFontStyle.Regular,
                    BackendFontStyle.Bold,
                    BackendFontStyle.Italic,
                    BackendFontStyle.Bold | BackendFontStyle.Italic
                }) {
                    Assert.True(BackendFont.IsAvailable(style));
                    using BackendFontScope scope = BackendFont.PushStyle(style);
                    ImFont* font = NativeImGui.Raw.GetFont();
                    ImFontBaked* baked = NativeImGui.Raw.GetFontBaked();
                    foreach (char value in "Az09!?")
                    {
                        ImFontGlyph* glyph = NativeImGui.Raw.FindGlyphNoFallback(baked, value);
                        Assert.True(glyph is not null, $"Face {style} has no U+{(int)value:X4} glyph.");
                        Assert.Equal((uint)value, glyph->Codepoint);
                        Assert.Equal(0u, glyph->SourceIdx);
                    }
                    ImFontGlyph* icon = NativeImGui.Raw.FindGlyphNoFallback(baked, 0xF0E2);
                    Assert.True(icon is not null);
                    Assert.InRange(icon->SourceIdx, 1u, 2u);
                    ImFontGlyph* logo = NativeImGui.Raw.FindGlyphNoFallback(baked, ImGuiIcon.C_INNO[0]);
                    Assert.True(logo is not null);
                    Assert.Equal((uint)ImGuiIcon.C_INNO[0], logo->Codepoint);
                    Assert.Equal(3u, logo->SourceIdx);
                    Assert.True(logo->AdvanceX > 0 && logo->X1 > logo->X0 && logo->Y1 > logo->Y0);
                    Assert.Equal(4, font->Sources.Size);
                    Assert.True(NativeImGui.Raw.FindGlyphNoFallback(baked, 0x6F22) is null);
                    NativeImGui.TextUnformatted(ImGuiIcon.C_INNO + " English / undo");
                }
            }
            finally
            {
                NativeImGui.End();
            }
        });
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(FontCoverageTests) + "." + nameof(SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow)
        )) {
            return;
        }
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = application.CreateWindow(
            new PlatformWindowOptions { title = "Replacement context", width = 320, height = 240, visible = false }
        );
        PlatformImGuiContext original = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        original.SetIniFile(null);
        _ = CaptureSource(original);
        original.Dispose();
        using PlatformImGuiContext replacement = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        replacement.SetIniFile(null);
        _ = CaptureSource(replacement);
        Assert.Throws<ObjectDisposedException>(() => original.RenderFrame(static () => {}));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void ApplicationShutdownRetiresBothRegisteredContexts()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(FontCoverageTests) + "." + nameof(ApplicationShutdownRetiresBothRegisteredContexts)))
        {
            return;
        }
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow firstWindow = application.CreateWindow(
            new PlatformWindowOptions { title = "First context", width = 320, height = 240, visible = false }
        );
        using Sdl3PlatformWindow secondWindow = application.CreateWindow(
            new PlatformWindowOptions { title = "Second context", width = 320, height = 240, visible = false }
        );
        PlatformImGuiContext first = application.CreateImGuiContext(
            firstWindow,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        first.SetIniFile(null);
        PlatformImGuiContext second = application.CreateImGuiContext(
            secondWindow,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        second.SetIniFile(null);
        _ = CaptureSource(first);
        _ = CaptureSource(second);
        application.Dispose();
        Assert.Throws<ObjectDisposedException>(() => first.RenderFrame(static () => {}));
        Assert.Throws<ObjectDisposedException>(() => second.RenderFrame(static () => {}));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void RetiringOneContextReleasesItsSourceAndPreservesTheOtherContext()
    {
        if (NativeWindowTestHost.RunIfRequired(
            nameof(FontCoverageTests) + "." + nameof(RetiringOneContextReleasesItsSourceAndPreservesTheOtherContext)
        )) {
            return;
        }
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = application.CreateWindow(
            new PlatformWindowOptions { title = "Retained font context", width = 320, height = 240, visible = false }
        );
        using PlatformImGuiContext retained = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        retained.SetIniFile(null);
        nint source = CaptureSource(retained);
        WeakReference<PlatformImGuiContext> retired = CreateAndRetire(application, source);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(retired.TryGetTarget(out _));
        Assert.Equal(source, CaptureSource(retained));
        Assert.Single(application.GetWindows());
    }

    [Fact]
    public void CustomFontStyleUsesOnlyItsOwnTextSourceAndIcons()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(FontCoverageTests) + "." + nameof(CustomFontStyleUsesOnlyItsOwnTextSourceAndIcons)))
        {
            return;
        }
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using Sdl3PlatformWindow window = application.CreateWindow(
            new PlatformWindowOptions { title = "Custom font", width = 320, height = 240, visible = false }
        );
        using PlatformImGuiContext context = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        context.SetIniFile(null);
        string fontPath = Path.Combine(AppContext.BaseDirectory, "BuiltIn", "Fonts", "JetBrainsMono-Bold.ttf");
        Assert.Throws<FileNotFoundException>(() => context.RegisterFontStyle(BackendFontStyle.Regular, fontPath + ".missing"));
        context.RegisterFontStyle(BackendFontStyle.Regular, fontPath, 24f);
        context.RenderFrame(() =>
        {
            using BackendFontScope scope = BackendFont.PushStyle(BackendFontStyle.Regular);
            ImFont* font = NativeImGui.Raw.GetFont();
            ImFontBaked* baked = NativeImGui.Raw.GetFontBaked();
            Assert.Equal(4, font->Sources.Size);
            Assert.Equal(new FileInfo(fontPath).Length, font->Sources.Data[0]->FontDataSize);
            Assert.Equal(24f, font->Sources.Data[0]->SizePixels);
            Assert.True(NativeImGui.Raw.FindGlyphNoFallback(baked, 'A') is not null);
            Assert.True(NativeImGui.Raw.FindGlyphNoFallback(baked, ImGuiIcon.C_INNO[0]) is not null);
            Assert.True(NativeImGui.Raw.FindGlyphNoFallback(baked, 0x6F22) is null);
        });
        Assert.Throws<InvalidOperationException>(() => context.RegisterFontStyle(BackendFontStyle.Regular, fontPath));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference<PlatformImGuiContext> CreateAndRetire(
        Sdl3PlatformApplication application,
        nint retainedSource
    ) {
        using Sdl3PlatformWindow window = application.CreateWindow(
            new PlatformWindowOptions { title = "Retired font context", width = 320, height = 240, visible = false }
        );
        PlatformImGuiContext context = application.CreateImGuiContext(
            window,
            ImGuiContextFlags.None,
            new ImGuiInteractionOptions(false, 1),
            new NoOutputRenderer()
        );
        context.SetIniFile(null);
        Assert.NotEqual(retainedSource, CaptureSource(context));
        context.Dispose();
        return new(context);
    }

    private static nint CaptureSource(PlatformImGuiContext context)
    {
        nint source = 0;
        context.RenderFrame(() =>
        {
            using BackendFontScope scope = BackendFont.PushStyle(BackendFontStyle.Regular);
            Assert.True(NativeImGui.Raw.FindGlyphNoFallback(NativeImGui.Raw.GetFontBaked(), 'A') is not null);
            source = (nint)NativeImGui.Raw.GetFont()->Sources.Data[0]->FontData;
        });
        Assert.NotEqual((nint)0, source);
        return source;
    }

    private sealed class NoOutputRenderer : IPlatformImGuiRenderer
    {
        public bool supportsViewports => false;
        public bool supportsVertexOffsets => false;

        public void RenderMain(nint drawData) {}

        public void SynchronizeMainOutput(
            int pixelWidth,
            int pixelHeight
        ) {}

        public void CreateViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();

        public void ResizeViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();

        public void RenderViewport(
            PlatformImGuiViewportTarget target,
            nint drawData
        ) => throw new NotSupportedException();

        public void PresentViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();

        public Task RetireViewportAsync(PlatformImGuiViewportTarget target) => Task.CompletedTask;

        public void DrainViewportRetirements() {}

        public void Dispose() {}
    }
}
