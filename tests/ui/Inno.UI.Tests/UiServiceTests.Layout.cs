using System;
using System.Linq;
using System.IO;
using Inno.Text;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Mathematics;
using Inno.Core.Input;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void LayoutBatchCapturesCurrentTransformsWithoutAdvancingTimeOrConsumingInput()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("layout-batch", 320, 180));
        backend.SetTimePaused(context, true);
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
            body { margin: 0; }
            #box { display: block; position: absolute; left: 20px; top: 30px; width: 40px; height: 20px;
                   background-color: red; transform: translate(10px, 15px); }
            #hidden { display: none; }
            </style></head><body><div id="box"></div><div id="hidden"><div id="hidden-child"></div></div></body></rml>
            """));
        backend.ShowDocument(context, document);
        UiLayoutSnapshot first = backend.CaptureLayout(context, document, ["box", "hidden", "absent", "hidden-child"]);
        Assert.Equal(context, first.context);
        Assert.Equal(document, first.document);
        Assert.Equal(UiElementLayoutStatus.Available, first.elements[0].status);
        Assert.Equal(UiElementLayoutStatus.Hidden, first.elements[1].status);
        Assert.Equal(UiElementLayoutStatus.Missing, first.elements[2].status);
        Assert.Equal(UiElementLayoutStatus.Hidden, first.elements[3].status);
        UiElementLayout box = first.elements[0];
        AssertClose(30, box.borderBounds.x);
        AssertClose(45, box.borderBounds.y);
        AssertClose(40, box.borderBounds.width);
        AssertClose(20, box.borderBounds.height);
        Assert.True(box.TryLocalToContext(new Vector2(5, 7), out Vector2 point));
        AssertClose(35, point.x);
        AssertClose(52, point.y);
        Assert.True(box.TryContextToLocal(point, out Vector2 local));
        AssertClose(5, local.x);
        AssertClose(7, local.y);
        Assert.Equal(0, backend.GetTime(context).seconds);
        Assert.NotEmpty(backend.Render(context).meshUpdates);
        Assert.True(backend.SetAttribute(context, document, "box", "style", "width: 90px; transform: translate(25px, 5px);"));
        UiLayoutSnapshot next = backend.CaptureLayout(context, document, ["box"]);
        Assert.True(next.revision > first.revision);
        AssertClose(90, next.elements[0].borderBounds.width);
        AssertClose(45, next.elements[0].borderBounds.x);
        AssertClose(40, first.elements[0].borderBounds.width);
        AssertClose(30, first.elements[0].borderBounds.x);
        backend.CloseDocument(context, document);
        Assert.Throws<InvalidOperationException>(() => backend.CaptureLayout(context, document, ["box"]));
        backend.DestroyContext(context);
    }

    [Fact]
    public void RepeatedLayoutPreparationKeepsOnlyPublishableResourceUpdates()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("layout-publication", 320, 180));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>body { margin: 0; } #box { display: block; width: 10px; height: 20px; background-color: red; }</style></head>
            <body><div id="box"></div></body></rml>
            """));
        backend.ShowDocument(context, document);
        for (int iteration = 0; iteration < 100; iteration++)
        {
            Assert.True(backend.SetAttribute(context, document, "box", "style", $"width: {20 + iteration}px;"));
            UiLayoutSnapshot layout = backend.CaptureLayout(context, document, ["box"]);
            AssertClose(20 + iteration, layout.elements[0].borderBounds.width);
        }
        UiRenderFrame frame = backend.Render(context);
        Assert.NotEmpty(frame.meshUpdates);
        Assert.All(frame.meshUpdates, mesh => Assert.NotEmpty(mesh.vertices));
        Assert.Empty(backend.Render(context).meshUpdates);
        backend.DestroyContext(context);
    }

    [Fact]
    public void LayoutMappingPreservesPerspectiveAndRejectsSingularOrUnavailableGeometry()
    {
        Matrix transform = Matrix.identity;
        transform.m11 = 2;
        transform.m22 = 3;
        transform.m14 = 15;
        transform.m24 = 7;
        transform.m41 = 0.01f;
        var layout = new UiElementLayout("projected", UiElementLayoutStatus.Available, default, default, default, transform);
        Assert.True(layout.TryLocalToContext(new Vector2(8, 12), out Vector2 context));
        Assert.True(layout.TryContextToLocal(context, out Vector2 local));
        AssertClose(8, local.x);
        AssertClose(12, local.y);
        var singular = new UiElementLayout("singular", UiElementLayoutStatus.Available, default, default, default, default);
        Assert.False(singular.TryContextToLocal(context, out _));
        Assert.False(singular.TryLocalToContext(local, out _));
        var hidden = new UiElementLayout("hidden", UiElementLayoutStatus.Hidden, default, default, default, Matrix.identity);
        Assert.False(hidden.TryLocalToContext(local, out _));
        Assert.False(layout.TryLocalToContext(new Vector2(float.NaN, 0), out _));
        var mapping = new UiViewportMapping(new Vector2(320, 180), new Rect(40, 20, 640, 360));
        Vector2 viewport = mapping.ToViewport(context);
        Vector2 roundTrip = mapping.ToContext(viewport);
        AssertClose(context.x, roundTrip.x);
        AssertClose(context.y, roundTrip.y);
        Assert.Throws<InvalidOperationException>(() => default(UiViewportMapping).ToContext(default));
    }

    [Fact]
    public void UiInputSnapshotFreezesCallerOwnedCollections()
    {
        KeyCode[] keys = [KeyCode.A];
        MouseButton[] buttons = [MouseButton.Left];
        string[] text = ["before"];
        var sample = FrameInput(default, default, KeyModifier.None,
            keys, [], buttons, [], text);
        keys[0] = KeyCode.B;
        buttons[0] = MouseButton.Right;
        text[0] = "after";
        Assert.Equal(KeyCode.A, Assert.Single(sample.keysPressed));
        Assert.Equal(MouseButton.Left, Assert.Single(sample.buttonsPressed));
        Assert.Equal("before", Assert.Single(sample.textInput));
        Assert.Throws<NotSupportedException>(() => ((System.Collections.Generic.IList<string>)sample.textInput)[0] = "mutated");
    }

    [Fact]
    public void LayoutUsesFinalFlexGeometryAndDensityExactlyOnce()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("layout-density", 1280, 720));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
            body { margin: 0; }
            #row { display: flex; width: 200dp; height: 40dp; }
            #first { width: 40dp; height: 20dp; flex-shrink: 0; }
            #second { flex-grow: 1; height: 20dp; background-color: red; }
            </style></head><body><div id="row"><div id="first"></div><div id="second"></div></div></body></rml>
            """));
        backend.ShowDocument(context, document);
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.9f, 1f, 1.1f, 1.25f, 1.5f })
        foreach (float dpi in new[] { 1f, 1.25f, 1.5f, 2f })
        {
            float density = zoom * dpi;
            backend.SetViewport(context, 1280, 720, density);
            UiLayoutSnapshot batch = backend.CaptureLayout(context, document, ["row", "first", "second"]);
            // Layout edges are snapped to backend pixels; adjacency must still be exact.
            Assert.InRange(batch.elements[0].borderBounds.width, 200 * density - 0.501f, 200 * density + 0.501f);
            Assert.InRange(batch.elements[1].borderBounds.width, 40 * density - 0.501f, 40 * density + 0.501f);
            AssertClose(batch.elements[1].borderBounds.right, batch.elements[2].borderBounds.x);
            AssertClose(MathF.Round(batch.elements[0].borderBounds.right, MidpointRounding.AwayFromZero),
                batch.elements[2].borderBounds.right);
            var viewport = new UiViewportMapping(new Vector2(1280, 720), new Rect(20, 30, 640, 360));
            Vector2 point = batch.elements[2].borderBounds.min;
            Vector2 mapped = viewport.ToViewport(point);
            AssertClose(20 + point.x * 0.5f, mapped.x);
            AssertClose(point.x, viewport.ToContext(mapped).x);
            _ = backend.Render(context);
        }
        backend.DestroyContext(context);
    }

    [Fact]
    public void LayoutCapturePreservesQueuedInputFocusAndIndependentContexts()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "LayoutInput", TextFontStyle.Normal, 400));
        UiContextHandle first = backend.CreateContext(new UiContextOptions("layout-input-a", 320, 180));
        UiContextHandle second = backend.CreateContext(new UiContextOptions("layout-input-b", 320, 180));
        backend.SetTimePaused(first, true);
        backend.SetTimePaused(second, true);
        UiDocumentHandle a = backend.LoadDocument(first, Rml("<rml><head><style>body { font-family: LayoutInput; }</style></head><body><input id='text'/></body></rml>"));
        UiDocumentHandle b = backend.LoadDocument(second, Rml("<rml><head><style>body { font-family: LayoutInput; }</style></head><body><input id='text'/></body></rml>"));
        backend.ShowDocument(first, a);
        backend.ShowDocument(second, b);
        backend.RefreshLayout(first);
        backend.RefreshLayout(second);
        backend.FocusElement(first, a, "text");
        backend.FocusElement(second, b, "text");
        backend.DrainEvents(first);
        backend.DrainEvents(second);
        backend.ProcessInput(first, FrameInput(default, default, KeyModifier.None, [], [], [], [], ["A"]));
        for (int index = 0; index < 100; index++)
        {
            _ = backend.CaptureLayout(first, a, ["text"]);
            _ = backend.CaptureLayout(second, b, ["text"]);
            Assert.Equal(new UiFocusTarget(a, "text"), backend.GetFocusedElement(first));
            Assert.Equal(new UiFocusTarget(b, "text"), backend.GetFocusedElement(second));
            Assert.Equal(0, backend.GetTime(first).seconds);
            Assert.Equal(0, backend.GetTime(second).seconds);
        }
        UiEvent change = Assert.Single(backend.DrainEvents(first).Where(item => item.type == UiEventType.Change));
        Assert.Equal(UiControlValue.Text("A"), change.value);
        Assert.Empty(backend.DrainEvents(second));
        Assert.Throws<InvalidOperationException>(() => backend.CaptureLayout(second, a, ["text"]));
        backend.DestroyContext(first);
        Assert.Equal(new UiFocusTarget(b, "text"), backend.GetFocusedElement(second));
        backend.DestroyContext(second);
    }

    [Fact]
    public void LayoutCapturesAncestorClippingAndScrollOffsets()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("layout-scroll", 320, 180));
        backend.SetTimePaused(context, true);
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
            body { margin: 0; }
            #view { position: absolute; left: 20px; top: 30px; width: 80px; height: 40px; overflow: hidden auto; }
            #content { display: block; width: 60px; height: 400px; background-color: red; }
            </style></head><body><div id="view"><div id="content"></div></div></body></rml>
            """));
        backend.ShowDocument(context, document);
        UiElementLayout before = backend.CaptureLayout(context, document, ["content"]).elements[0];
        AssertClose(20, before.clipBounds.x);
        AssertClose(30, before.clipBounds.y);
        AssertClose(40, before.clipBounds.height);
        Assert.InRange(before.clipBounds.width, 1, 80);
        backend.ProcessInput(context, FrameInput(new Vector2(30, 40), new Vector2(0, -3), KeyModifier.None, [], [], [], [], []));
        for (int index = 0; index < 10; index++)
        {
            backend.AdvanceTime(context, 0.05);
            backend.Update(context, FrameInput(new Vector2(30, 40), default, KeyModifier.None, [], [], [], [], []));
        }
        UiElementLayout after = backend.CaptureLayout(context, document, ["content"]).elements[0];
        Assert.True(after.borderBounds.y < before.borderBounds.y);
        Assert.Equal(before.clipBounds, after.clipBounds);
        Assert.True(after.TryLocalToContext(Vector2.ZERO, out Vector2 origin));
        AssertClose(after.borderBounds.y, origin.y);
        backend.DestroyContext(context);
    }

    private static void AssertClose(
        float expected,
        float actual
    ) => Assert.InRange(actual, expected - 0.01f, expected + 0.01f);
}
