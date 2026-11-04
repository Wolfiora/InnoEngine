using System;
using System.Linq;
using System.IO;
using Inno.Text;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Input;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void TextAndKeyChangesReplayInAcquisitionOrderWithinOneFrame()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "OrderedInput", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new UiContextOptions("ordered-input", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("<rml><head><style>body { font-family: OrderedInput; font-size: 18px; }</style></head><body><input id='text'/></body></rml>"));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        backend.FocusElement(context, document, "text");
        backend.DrainEvents(context);
        var sequence = new InputSequence([
            new TextInputChange("A", KeyModifier.None),
            new KeyInputChange(KeyCode.LeftArrow, true, false, KeyModifier.None),
            new KeyInputChange(KeyCode.LeftArrow, false, false, KeyModifier.None),
            new TextInputChange("B", KeyModifier.None)
        ]);
        backend.ProcessInput(context, new UiInputSnapshot(default, KeyModifier.None, sequence));
        Assert.Equal(UiControlValue.Text("BA"), backend.GetValue(context, document, "text"));
        UiEvent[] changes = backend.DrainEvents(context).Where(item => item.type == UiEventType.Change).ToArray();
        Assert.Equal(new[] { UiControlValue.Text("A"), UiControlValue.Text("BA") }, changes.Select(item => item.value));
        backend.CloseDocument(context, document);
        backend.DestroyContext(context);
    }

    [Fact]
    public void ControlValuesAreTypedAndProgrammaticWritesDoNotEcho()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("values", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><body>
              <input id="text" value="initial"/>
              <input id="check" type="checkbox"/>
              <input id="range" type="range" min="0" max="100" value="25"/>
              <select id="choice"><option value="a" selected>A</option><option value="b">B</option></select>
              <div id="label">Text</div>
            </body></rml>
            """));
        backend.ShowDocument(context, document);
        backend.Update(context, EmptyInput());
        backend.DrainEvents(context);
        Assert.Equal(UiControlValue.Text("initial"), backend.GetValue(context, document, "text"));
        Assert.Equal(UiControlValue.Boolean(false), backend.GetValue(context, document, "check"));
        Assert.Equal(UiControlValue.Number(25), backend.GetValue(context, document, "range"));
        Assert.Equal(UiControlValue.Selection("a"), backend.GetValue(context, document, "choice"));
        Assert.Equal(default, backend.GetValue(context, document, "label"));
        Assert.Equal(default, backend.GetValue(context, document, "missing"));
        Assert.True(backend.SetValue(context, document, "text", UiControlValue.Text("中文 café")));
        Assert.True(backend.SetValue(context, document, "check", UiControlValue.Boolean(true)));
        Assert.True(backend.SetValue(context, document, "range", UiControlValue.Number(65)));
        Assert.True(backend.SetValue(context, document, "choice", UiControlValue.Selection("b")));
        Assert.False(backend.SetValue(context, document, "choice", UiControlValue.Selection("absent")));
        Assert.False(backend.SetValue(context, document, "absent", UiControlValue.Text("value")));
        Assert.Throws<ArgumentException>(() => backend.SetValue(context, document, "check", UiControlValue.Text("true")));
        Assert.Throws<ArgumentOutOfRangeException>(() => UiControlValue.Number(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => UiControlValue.Number(double.PositiveInfinity));
        backend.Update(context, EmptyInput());
        Assert.Equal(UiControlValue.Text("中文 café"), backend.GetValue(context, document, "text"));
        Assert.Equal(UiControlValue.Boolean(true), backend.GetValue(context, document, "check"));
        Assert.Equal(UiControlValue.Number(65), backend.GetValue(context, document, "range"));
        Assert.Equal(UiControlValue.Selection("b"), backend.GetValue(context, document, "choice"));
        Assert.DoesNotContain(backend.DrainEvents(context), item => item.type is UiEventType.Change or UiEventType.Submit);
        backend.CloseDocument(context, document);
        Assert.Throws<InvalidOperationException>(() => backend.GetValue(context, document, "text"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void EventsCaptureValuesAtTheirOrderedInputBoundaryAndRetireWithTheirDocument()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "EventInput", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new UiContextOptions("events", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("<rml><head><style>body { font-family: EventInput; }</style></head><body><input id='text'/></body></rml>"));
        backend.ShowDocument(context, document);
        backend.Update(context, EmptyInput());
        backend.FocusElement(context, document, "text");
        backend.DrainEvents(context);
        backend.Update(context, FrameInput(default, default, KeyModifier.None, [], [], [], [], ["A", "B"]));
        UiEvent[] changes = backend.DrainEvents(context).Where(item => item.type == UiEventType.Change).ToArray();
        Assert.Equal(2, changes.Length);
        Assert.Equal(UiControlValue.Text("A"), changes[0].value);
        Assert.Equal(UiControlValue.Text("AB"), changes[1].value);
        Assert.All(changes, item =>
        {
            Assert.Equal(document, item.document);
            Assert.Equal(UiEventPhase.Editing, item.phase);
            Assert.Equal(UiEventOrigin.UserInput, item.origin);
        });
        backend.ClearFocus(context);
        UiEvent blur = Assert.Single(backend.DrainEvents(context).Where(item => item.type == UiEventType.Blur && item.targetId == "text"));
        Assert.Equal(UiControlValue.Text("AB"), blur.value);
        Assert.Equal(UiEventPhase.Committed, blur.phase);
        Assert.Equal(UiEventOrigin.Programmatic, blur.origin);
        backend.FocusElement(context, document, "text");
        backend.Update(context, FrameInput(default, default, KeyModifier.None, [], [], [], [], ["C"]));
        backend.CloseDocument(context, document);
        UiDocumentHandle replacement = backend.LoadDocument(context, Rml("<rml><body><input id='text'/></body></rml>"));
        Assert.NotEqual(document, replacement);
        Assert.DoesNotContain(backend.DrainEvents(context), item => item.document == document);
        backend.DestroyContext(context);
    }
}
