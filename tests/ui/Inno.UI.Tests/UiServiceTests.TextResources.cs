using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Text;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void DocumentResourceBindingsApplyToStylesAndFragmentsWithoutChangingTextValues()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Registered", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("bound-font", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, new UiDocumentSource(
            RmlUiIdentifiers.documentLanguage,
            "<rml><head><style>body { font-family: Logical; }</style></head><body><div id='container'><input id='edit' value='Logical'/></div></body></rml>",
            resourceBindings: new Dictionary<string, string> { ["Logical"] = "Registered" }));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        Assert.Equal(UiControlValue.Text("Logical"), backend.GetValue(context, document, "edit"));
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "edit"));
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new InputSequence([new TextInputChange("A", KeyModifier.None)])));
        Assert.Contains("A", backend.GetValue(context, document, "edit").text);
        Assert.True(backend.SetContent(context, document, "container", new(RmlUiIdentifiers.documentLanguage,
            "<input id='next' style='font-family: Logical;' value='Logical'/>")));
        Assert.True(backend.SetAttribute(context, document, "next", "style", "font-family: Logical;"));
        backend.RefreshLayout(context);
        Assert.Equal(UiControlValue.Text("Logical"), backend.GetValue(context, document, "next"));
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "next"));
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new InputSequence([new TextInputChange("B", KeyModifier.None)])));
        Assert.Contains("B", backend.GetValue(context, document, "next").text);
        backend.CloseDocument(context, document);
        backend.DestroyContext(context);
    }

    [Theory]
    [InlineData("input")]
    [InlineData("textarea")]
    public void UnresolvedTextFontRejectsEditingAndRecoversThroughDeclaredResources(string tag)
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("unresolved-font", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, Rml(
            $"<rml><head><style>body {{ margin: 0; font-family: MissingFont; }} {tag} {{ width: 160px; height: 40px; }}</style></head><body><{tag} id='edit'></{tag}></body></rml>"));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "edit"));
        backend.DrainEvents(context);
        var text = new UiInputSnapshot(default, KeyModifier.None,
            new InputSequence([new TextInputChange("AB", KeyModifier.None)]));
        var left = new UiInputSnapshot(default, KeyModifier.None,
            new InputSequence([new KeyInputChange(KeyCode.LeftArrow, true, false, KeyModifier.None)]));
        var press = new UiInputSnapshot(new Vector2(10, 10), KeyModifier.None,
            new InputSequence([new PointerButtonInputChange(new Vector2(10, 10), MouseButton.Left, true, KeyModifier.None)]));
        foreach (UiInputSnapshot input in new[] { text, left, press })
        {
            InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() => backend.ProcessInput(context, input));
            Assert.Contains("edit", failure.Message);
            Assert.Contains("no resolved font face", failure.Message);
            Assert.Equal(UiControlValue.Text(""), backend.GetValue(context, document, "edit"));
            Assert.Equal(new UiFocusTarget(document, "edit"), backend.GetFocusedElement(context));
            Assert.DoesNotContain(backend.DrainEvents(context), item => item.type is UiEventType.Change or UiEventType.Submit);
        }
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "ReadyFont", TextFontStyle.Normal, 400));
        Assert.True(backend.SetAttribute(context, document, "edit", "style", "font-family: ReadyFont;"));
        backend.RefreshLayout(context);
        backend.ProcessInput(context, text);
        backend.ProcessInput(context, left);
        backend.ProcessInput(context, new UiInputSnapshot(default, KeyModifier.None,
            new InputSequence([new TextInputChange("C", KeyModifier.None)])));
        Assert.Equal(UiControlValue.Text("ACB"), backend.GetValue(context, document, "edit"));
        Assert.Equal(2, backend.DrainEvents(context).Count(item => item.type == UiEventType.Change));
        backend.CloseDocument(context, document);
        backend.DestroyContext(context);
    }
}
