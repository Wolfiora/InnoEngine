using System;
using System.IO;
using System.Linq;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Input;
using Inno.Text;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void PausedContextsStepAndRefreshIndependentlyWithoutConsumingInput()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "TimeTest", TextFontStyle.Normal, 400));
        UiContextHandle first = backend.CreateContext(new UiContextOptions("first", 640, 360));
        UiContextHandle second = backend.CreateContext(new UiContextOptions("second", 640, 360));
        backend.SetTimePaused(first, true);
        backend.SetTimePaused(second, true);
        UiDocumentSource source = Rml("""
            <rml><head><style>
            body { margin: 0; font-family: TimeTest; }
            #box { display: block; width: 30px; height: 30px; background-color: red; }
            .wide #box { display: block; width: 150px; }
            </style></head><body id="body"><input id="text"/><div id="box"></div></body></rml>
            """);
        UiDocumentHandle firstDocument = backend.LoadDocument(first, source);
        UiDocumentHandle secondDocument = backend.LoadDocument(second, source);
        backend.ShowDocument(first, firstDocument);
        backend.ShowDocument(second, secondDocument);
        backend.RefreshLayout(first);
        backend.RefreshLayout(second);
        backend.FocusElement(first, firstDocument, "text");
        backend.DrainEvents(first);
        UiContextTime original = backend.GetTime(first);
        backend.AdvanceTime(first, 0.5);
        backend.RefreshLayout(first);
        backend.Update(first, EmptyInput());
        Assert.Equal(original.seconds + 0.5, backend.GetTime(first).seconds);
        Assert.Equal(0, backend.GetTime(second).seconds);
        Assert.Equal(new UiFocusTarget(firstDocument, "text"), backend.GetFocusedElement(first));
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(first, firstDocument, "text"));
        Assert.DoesNotContain(backend.DrainEvents(first), item => item.type == UiEventType.Change);
        backend.ProcessInput(first, FrameInput(default, default, KeyModifier.None, [], [], [], [], ["A"]));
        Assert.Equal(UiControlValue.Text("A"), backend.GetValue(first, firstDocument, "text"));
        Assert.Equal(original.seconds + 0.5, backend.GetTime(first).seconds);
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(second, secondDocument, "text"));
        Assert.NotEmpty(backend.Render(first).meshUpdates);
        Assert.True(backend.SetClass(first, firstDocument, "body", "wide", true));
        backend.RefreshLayout(first);
        Assert.NotEmpty(backend.Render(first).meshUpdates);
        Assert.Equal(original.seconds + 0.5, backend.GetTime(first).seconds);
        Assert.False(backend.capabilities.Supports(UiBackendCapability.TimeSeek));
        Assert.False(backend.TrySeekTime(first, 0));
        Assert.Equal(original.seconds + 0.5, backend.GetTime(first).seconds);
        Assert.Equal(new UiFocusTarget(firstDocument, "text"), backend.GetFocusedElement(first));
        Assert.Throws<ArgumentOutOfRangeException>(() => backend.AdvanceTime(first, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => backend.AdvanceTime(first, double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => backend.TrySeekTime(first, double.PositiveInfinity));
        backend.DestroyContext(first);
        backend.DestroyContext(second);
    }

    [Fact]
    public void AnimationUsesItsOwningContextClockAcrossInterleavedFrames()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle first = backend.CreateContext(new UiContextOptions("animated-first", 320, 180));
        UiContextHandle second = backend.CreateContext(new UiContextOptions("animated-second", 320, 180));
        backend.SetTimePaused(first, true);
        backend.SetTimePaused(second, true);
        UiDocumentSource source = Rml("""
            <rml><head><style>
            body { margin: 0; }
            @keyframes grow { from { width: 20px; } to { width: 120px; } }
            #box { display: block; width: 20px; height: 20px; background-color: red; animation: 1s linear grow; }
            </style></head><body><div id="box"></div></body></rml>
            """);
        UiDocumentHandle firstDocument = backend.LoadDocument(first, source);
        UiDocumentHandle secondDocument = backend.LoadDocument(second, source);
        backend.ShowDocument(first, firstDocument);
        backend.ShowDocument(second, secondDocument);
        backend.RefreshLayout(first);
        backend.RefreshLayout(second);
        UiRenderFrame initialFirst = backend.Render(first);
        UiRenderFrame initialSecond = backend.Render(second);
        float initialWidth = Width(initialFirst);
        Assert.Equal(initialWidth, Width(initialSecond));

        for (int step = 0; step < 10; step++)
        {
            backend.AdvanceTime(first, 0.05);
            backend.RefreshLayout(first);
        }
        float advancedWidth = Width(backend.Render(first));
        Assert.InRange(advancedWidth - initialWidth, 49f, 51f);
        backend.RefreshLayout(second);
        Assert.Empty(backend.Render(second).meshUpdates);
        Assert.Equal(0, backend.GetTime(second).seconds);

        for (int step = 0; step < 5; step++)
        {
            backend.AdvanceTime(second, 0.05);
            backend.RefreshLayout(second);
        }
        Assert.InRange(Width(backend.Render(second)) - initialWidth, 24f, 26f);
        backend.RefreshLayout(first);
        Assert.Empty(backend.Render(first).meshUpdates);
        Assert.Equal(0.5, backend.GetTime(first).seconds, 8);

        static float Width(UiRenderFrame frame)
        {
            UiVertex[] vertices = frame.meshUpdates.SelectMany(item => item.vertices).ToArray();
            Assert.NotEmpty(vertices);
            return vertices.Max(item => item.x) - vertices.Min(item => item.x);
        }
    }

    [Fact]
    public void OptionalCapabilitiesRemainOpenAndImmutable()
    {
        var extension = new UiBackendCapability("tests.custom-behavior");
        UiBackendCapability[] input = [extension];
        var capabilities = new UiBackendCapabilities([new UiDocumentLanguageId("tests.document")], input);
        input[0] = UiBackendCapability.TimeSeek;
        Assert.True(capabilities.Supports(extension));
        Assert.False(capabilities.Supports(UiBackendCapability.TimeSeek));
        Assert.Throws<ArgumentException>(() => new UiBackendCapability("invalid identity"));
        Assert.Throws<ArgumentException>(() => new UiBackendCapabilities([new UiDocumentLanguageId("tests.document")], [default]));
    }
}
