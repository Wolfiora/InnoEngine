using System;
using System.Threading;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Input;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void KeyboardAndProgrammaticFocusShareOneAuthorityAcrossDensityChanges()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("keyboard-focus", 1280, 720));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
            input { width: 80dp; height: 25dp; tab-index: auto; }
            #hidden { display: none; }
            </style></head><body>
            <input id="first"/><input id="disabled" disabled/>
            <input id="hidden"/><input id="last"/>
            </body></rml>
            """));
        backend.ShowDocument(context, document);
        foreach (float zoom in new[] { .75f, .85f, .9f, 1f, 1.1f, 1.25f, 1.5f })
        foreach (float dpi in new[] { 1f, 1.25f, 1.5f, 2f })
        {
            backend.SetViewport(context, 1280, 720, zoom * dpi);
            backend.RefreshLayout(context);
            Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "first"));
            backend.ProcessInput(context, FrameInput(default, default, KeyModifier.None, [KeyCode.Tab], [], [], [], []));
            Assert.Equal(new UiFocusTarget(document, "last"), backend.GetFocusedElement(context));
            backend.ProcessInput(context, FrameInput(default, default, KeyModifier.Shift, [KeyCode.Tab], [], [], [], []));
            Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
            backend.SetValue(context, document, "first", UiControlValue.Text("stable focus"));
            backend.RefreshLayout(context);
            Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
            backend.ClearFocus(context);
        }
        backend.CloseDocument(context, document);
        Assert.Null(backend.GetFocusedElement(context));
        backend.DestroyContext(context);
    }

    [Fact]
    public void ForeignThreadCannotUseOrRetireTheBackend()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("thread", 640, 360));
        Exception? queryFailure = null;
        Exception? disposeFailure = null;
        var thread = new Thread(() =>
        {
            queryFailure = Record.Exception(() => backend.GetFocusedElement(context));
            disposeFailure = Record.Exception(backend.Dispose);
        });
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(5)));
        Assert.IsType<InvalidOperationException>(queryFailure);
        Assert.IsType<InvalidOperationException>(disposeFailure);
        Assert.Null(backend.GetFocusedElement(context));
        backend.DestroyContext(context);
    }

    [Fact]
    public void ExplicitFocusUsesDocumentPolicyAndSurvivesOrdinaryChanges()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new UiContextOptions("focus", 640, 360));
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
              body { margin: 0; }
              input { width: 120px; height: 30px; }
              #hidden { display: none; }
              #blocked { focus: none; }
            </style></head><body>
              <input id="first"/><input id="second"/>
              <input id="disabled" disabled/><input id="hidden"/><div id="blocked"/>
            </body></rml>
            """));
        backend.ShowDocument(context, document);
        backend.Update(context, EmptyInput());
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "first"));
        Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
        Assert.Equal(UiFocusResult.Unchanged, backend.FocusElement(context, document, "first"));
        Assert.Equal(UiFocusResult.NotFound, backend.FocusElement(context, document, "absent"));
        Assert.Equal(UiFocusResult.NotFocusable, backend.FocusElement(context, document, "disabled"));
        Assert.Equal(UiFocusResult.NotFocusable, backend.FocusElement(context, document, "hidden"));
        Assert.Equal(UiFocusResult.NotFocusable, backend.FocusElement(context, document, "blocked"));
        Assert.True(backend.SetAttribute(context, document, "first", "value", "changed"));
        backend.Update(context, EmptyInput());
        Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
        backend.DrainEvents(context);
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "second"));
        var events = backend.DrainEvents(context);
        Assert.Contains(events, item => item.type == UiEventType.Blur && item.targetId == "first");
        Assert.Contains(events, item => item.type == UiEventType.Focus && item.targetId == "second");
        Assert.True(backend.ClearFocus(context));
        Assert.Null(backend.GetFocusedElement(context));
        Assert.True(backend.ClearFocus(context));
        backend.HideDocument(context, document);
        Assert.Equal(UiFocusResult.NotFocusable, backend.FocusElement(context, document, "first"));
        backend.CloseDocument(context, document);
        Assert.Throws<InvalidOperationException>(() => backend.FocusElement(context, document, "first"));
        backend.DestroyContext(context);
        Assert.Throws<InvalidOperationException>(() => backend.GetFocusedElement(context));
    }

    [Fact]
    public void FocusIsIsolatedAndRetiresAcrossRepeatedDocumentReplacement()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle first = backend.CreateContext(new UiContextOptions("first", 640, 360));
        UiContextHandle second = backend.CreateContext(new UiContextOptions("second", 640, 360));
        UiDocumentSource source = Rml("<rml><body><input id='name'/></body></rml>");
        UiDocumentHandle kept = backend.LoadDocument(second, source);
        backend.ShowDocument(second, kept);
        backend.Update(second, EmptyInput());
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(second, kept, "name"));
        for (int i = 0; i < 100; i++)
        {
            UiDocumentHandle document = backend.LoadDocument(first, source);
            backend.ShowDocument(first, document);
            backend.Update(first, EmptyInput());
            Assert.Equal(UiFocusResult.Focused, backend.FocusElement(first, document, "name"));
            Assert.Throws<InvalidOperationException>(() => backend.FocusElement(second, document, "name"));
            backend.CloseDocument(first, document);
            backend.Update(first, EmptyInput());
            Assert.Null(backend.GetFocusedElement(first));
            Assert.Equal(new UiFocusTarget(kept, "name"), backend.GetFocusedElement(second));
            backend.DrainEvents(first);
        }
        backend.DestroyContext(first);
        backend.DestroyContext(second);
    }
}
