using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation.ImGui;
using Inno.Native.ImGui;
using Inno.Platform;
using Inno.Testing.Processes;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using BackendFont = Inno.Adapter.Presentation.ImGui.ImGuiFont;
using BackendFontScope = Inno.Adapter.Presentation.ImGui.ImGuiFontScope;
using BackendFontStyle = Inno.Adapter.Presentation.ImGui.ImGuiFontStyle;

namespace Inno.Editor.ImGui.Tests;

public sealed unsafe class FontCoverageTests
{
    [Theory]
    [InlineData(0.75f)]
    [InlineData(1f)]
    [InlineData(1.5f)]
    [InlineData(2f)]
    [Trait("Acceptance", "Visual")]
    public void EveryTextFaceHasRealCjkGlyphsAndSharesOneOwnedSource(float zoom)
    {
        string contract = nameof(FontCoverageTests) + "." + nameof(EveryTextFaceHasRealCjkGlyphsAndSharesOneOwnedSource) + "." + zoom.ToString(CultureInfo.InvariantCulture);
        if (NativeWindowTestHost.RunIfRequired(contract))
            return;
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions { title = "Glyph coverage", width = 640, height = 480, visible = false });
        using var context = application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        nint sharedData = 0;
        context.RenderFrame(() =>
        {
            NativeImGui.Raw.GetStyle()->FontScaleMain = zoom;
            bool open = true;
            _ = NativeImGui.Begin("Glyph coverage", ref open, 0);
            try
            {
                foreach (BackendFontStyle style in new[] { BackendFontStyle.Regular, BackendFontStyle.Bold, BackendFontStyle.Italic, BackendFontStyle.Bold | BackendFontStyle.Italic })
                {
                    Assert.True(BackendFont.IsAvailable(style));
                    using BackendFontScope scope = BackendFont.PushStyle(style);
                    ImFont* font = NativeImGui.Raw.GetFont();
                    ImFontBaked* baked = NativeImGui.Raw.GetFontBaked();
                    foreach (char value in "Aé资源漢字")
                    {
                        ImFontGlyph* glyph = NativeImGui.Raw.FindGlyphNoFallback(baked, value);
                        Assert.True(glyph is not null, $"Face {style} has no U+{(int)value:X4} glyph.");
                        Assert.Equal((uint)value, glyph->Codepoint);
                        Assert.Equal(value is 'A' or 'é' ? 0u : 3u, glyph->SourceIdx);
                    }
                    ImFontGlyph* icon = NativeImGui.Raw.FindGlyphNoFallback(baked, 0xf0e2);
                    Assert.True(icon is not null);
                    Assert.InRange(icon->SourceIdx, 1u, 2u);
                    Assert.Equal(4, font->Sources.Size);
                    ImFontConfig* supplemental = font->Sources.Data[3];
                    Assert.Equal(16_437_364, supplemental->FontDataSize);
                    Assert.Equal(0, supplemental->FontDataOwnedByAtlas);
                    if (sharedData == 0)
                        sharedData = (nint)supplemental->FontData;
                    Assert.Equal(sharedData, (nint)supplemental->FontData);
                    NativeImGui.TextUnformatted("Aé资源漢字 / undo");
                }
            }
            finally
            {
                NativeImGui.End();
            }
        });
        Assert.NotEqual((nint)0, sharedData);
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(FontCoverageTests) + "." + nameof(SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow)))
            return;
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions { title = "Replacement context", width = 320, height = 240, visible = false });
        var original = application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        _ = CaptureSource(original);
        original.Dispose();
        using var replacement = application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        _ = CaptureSource(replacement);
        Assert.Throws<ObjectDisposedException>(() => original.RenderFrame(static () => { }));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void ApplicationShutdownRetiresBothRegisteredContexts()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(FontCoverageTests) + "." + nameof(ApplicationShutdownRetiresBothRegisteredContexts)))
            return;
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var firstWindow = application.CreateWindow(new PlatformWindowOptions { title = "First context", width = 320, height = 240, visible = false });
        using var secondWindow = application.CreateWindow(new PlatformWindowOptions { title = "Second context", width = 320, height = 240, visible = false });
        var first = application.CreateImGuiContext(firstWindow, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        var second = application.CreateImGuiContext(secondWindow, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        _ = CaptureSource(first);
        _ = CaptureSource(second);
        application.Dispose();
        Assert.Throws<ObjectDisposedException>(() => first.RenderFrame(static () => { }));
        Assert.Throws<ObjectDisposedException>(() => second.RenderFrame(static () => { }));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void RetiringTenContextsReleasesTheirSourcesAndPreservesAnotherContext()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(FontCoverageTests) + "." + nameof(RetiringTenContextsReleasesTheirSourcesAndPreservesAnotherContext)))
            return;
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions { title = "Retained font context", width = 320, height = 240, visible = false });
        using var retained = application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
        nint source = CaptureSource(retained);
        long baseline = GC.GetTotalMemory(forceFullCollection: true);
        var retired = new List<WeakReference<PlatformImGuiContext>>();
        for (int cycle = 0; cycle < 10; cycle++)
        {
            retired.Add(CreateAndRetire(application, source));
            Assert.Equal(source, CaptureSource(retained));
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.All(retired, reference => Assert.False(reference.TryGetTarget(out _)));
        Assert.InRange(GC.GetTotalMemory(forceFullCollection: true) - baseline, long.MinValue, 2L * 1024 * 1024);
        Assert.Single(application.GetWindows());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RequiredBundledContentIsRejectedBeforeNativeUse(bool corrupt)
    {
        string owner = Path.Combine(Path.GetTempPath(), "InnoFontContent", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(owner);
        try
        {
            foreach (string file in Directory.EnumerateFiles(AppContext.BaseDirectory, "*", SearchOption.AllDirectories))
            {
                string target = Path.Combine(owner, Path.GetRelativePath(AppContext.BaseDirectory, file));
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(file, target);
            }
            string font = Path.Combine(owner, "BuiltIn", "Fonts", "Supplemental", "NotoSansCJKsc-Regular.otf");
            Assert.True(File.Exists(font));
            if (corrupt)
            {
                using FileStream file = File.OpenWrite(font);
                file.WriteByte(0);
            }
            else
                File.Delete(font);
            NativeProcessTestHost.Run(Path.Combine(owner, Path.GetFileName(typeof(FontCoverageTests).Assembly.Location)),
                nameof(FontCoverageTests) + ".RejectOwnedContent." + corrupt);
        }
        finally
        {
            Directory.Delete(owner, recursive: true);
        }
    }

    internal static void RejectOwnedContent(bool corrupt)
    {
        using var application = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = application.CreateWindow(new PlatformWindowOptions { title = "Font content failure", width = 320, height = 240, visible = false });
        void Create() => application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer()).Dispose();
        if (corrupt)
            Assert.Throws<InvalidDataException>(Create);
        else
            Assert.Throws<FileNotFoundException>(Create);
        Assert.Single(application.GetWindows());
        Assert.False(BackendFont.IsAvailable(BackendFontStyle.Regular));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference<PlatformImGuiContext> CreateAndRetire(
        Sdl3PlatformApplication application,
        nint retainedSource
    ) {
        using var window = application.CreateWindow(new PlatformWindowOptions { title = "Retired font context", width = 320, height = 240, visible = false });
        var context = application.CreateImGuiContext(window, ImGuiContextFlags.None, new ImGuiInteractionOptions(false, 1), new NoOutputRenderer());
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
            Assert.True(NativeImGui.Raw.FindGlyphNoFallback(NativeImGui.Raw.GetFontBaked(), 0x6f22) is not null);
            source = (nint)NativeImGui.Raw.GetFont()->Sources.Data[3]->FontData;
        });
        Assert.NotEqual((nint)0, source);
        return source;
    }

    private sealed class NoOutputRenderer : IPlatformImGuiRenderer
    {
        public bool supportsViewports => false;
        public bool supportsVertexOffsets => false;
        public void RenderMain(IntPtr drawData) { }
        public void SynchronizeMainOutput(
            int pixelWidth,
            int pixelHeight
        ) { }
        public void CreateViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public void ResizeViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public void RenderViewport(
            PlatformImGuiViewportTarget target,
            IntPtr drawData
        ) => throw new NotSupportedException();
        public void PresentViewport(PlatformImGuiViewportTarget target) => throw new NotSupportedException();
        public Task RetireViewport(PlatformImGuiViewportTarget target) => Task.CompletedTask;
        public void DrainViewportRetirements() { }
        public void Dispose() { }
    }
}
