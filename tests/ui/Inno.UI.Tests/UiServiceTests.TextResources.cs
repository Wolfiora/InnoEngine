using System;
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
