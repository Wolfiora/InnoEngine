using System;
using System.Diagnostics;
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
    public void CompositionReplacesPreeditAndCommitsOnceWithoutSubmitting()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        backend.DrainEvents(context);
        Composition(backend, context, 1, TextCompositionPhase.Started, "中");
        Assert.Equal(UiControlValue.Text("中"), backend.GetValue(context, document, "edit"));
        Composition(backend, context, 1, TextCompositionPhase.Updated, "中文");
        Assert.Equal(UiControlValue.Text("中文"), backend.GetValue(context, document, "edit"));
        Assert.DoesNotContain(backend.DrainEvents(context), item => item.type is UiEventType.Change or UiEventType.Submit);
        backend.ProcessInput(context, new(default, KeyModifier.None, new([
            new KeyInputChange(KeyCode.Enter, true, false, KeyModifier.None),
            new TextCompositionChange(TextCompositionPhase.Committed, new(1, "完成")),
            new KeyInputChange(KeyCode.Enter, false, false, KeyModifier.None),
            new GamepadButtonChange(1, GamepadButton.South, true)])));
        Assert.Equal(UiControlValue.Text("完成"), backend.GetValue(context, document, "edit"));
        UiEvent[] events = backend.DrainEvents(context).ToArray();
        Assert.Single(events.Where(item => item.type == UiEventType.Change));
        Assert.DoesNotContain(events, item => item.type == UiEventType.Submit);
        backend.DestroyContext(context);
    }

    [Fact]
    public void CompositionCancellationRestoresSelectionAndRejectsRetiredOperations()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        backend.SetValue(context, document, "edit", UiControlValue.Text("Original"));
        backend.ProcessInput(context, new(default, KeyModifier.None, new([
            new KeyInputChange(KeyCode.A, true, false, KeyModifier.Control),
            new KeyInputChange(KeyCode.A, false, false, KeyModifier.Control)])));
        Composition(backend, context, 1, TextCompositionPhase.Started, "候选");
        Assert.Equal(UiControlValue.Text("候选"), backend.GetValue(context, document, "edit"));
        Composition(backend, context, 1, TextCompositionPhase.Cancelled, "");
        Assert.Equal(UiControlValue.Text("Original"), backend.GetValue(context, document, "edit"));
        Composition(backend, context, 2, TextCompositionPhase.Started, "新");
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new([new TextCompositionChange(TextCompositionPhase.Committed, new(1, "过期"))]),
            textComposition: new(2, "新")));
        Assert.Equal(UiControlValue.Text("新"), backend.GetValue(context, document, "edit"));
        backend.DrainEvents(context);
        backend.FocusElement(context, document, "other");
        Assert.Equal(UiControlValue.Text("Original"), backend.GetValue(context, document, "edit"));
        Assert.DoesNotContain(backend.DrainEvents(context), item => item.value == UiControlValue.Text("新"));
        Composition(backend, context, 2, TextCompositionPhase.Committed, "迟到");
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(context, document, "other"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void ProgrammaticWriteCannotBeOverwrittenByRetiredComposition()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        Composition(backend, context, 1, TextCompositionPhase.Started, "候选");
        backend.SetValue(context, document, "edit", UiControlValue.Text("Authoritative"));
        Composition(backend, context, 1, TextCompositionPhase.Cancelled, "");
        Assert.Equal(UiControlValue.Text("Authoritative"), backend.GetValue(context, document, "edit"));
        Assert.Equal(new UiFocusTarget(document, "edit"), backend.GetFocusedElement(context));
        Composition(backend, context, 2, TextCompositionPhase.Started, "新");
        backend.SetAttribute(context, document, "edit", "value", "Current");
        Composition(backend, context, 2, TextCompositionPhase.Committed, "迟到");
        Assert.Equal(UiControlValue.Text("Current"), backend.GetValue(context, document, "edit"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void LostInputRouteCancelsPreeditWithoutBlurringTheControl()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        Composition(backend, context, 1, TextCompositionPhase.Started, "候选");
        backend.ProcessInput(context, EmptyInput());
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(context, document, "edit"));
        Assert.Equal(new UiFocusTarget(document, "edit"), backend.GetFocusedElement(context));
        Composition(backend, context, 1, TextCompositionPhase.Committed, "迟到");
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(context, document, "edit"));
        Composition(backend, context, 2, TextCompositionPhase.Started, "候选");
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new([new TextInputChange("Current", KeyModifier.None)])));
        Assert.Equal(UiControlValue.Text("Current"), backend.GetValue(context, document, "edit"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void CompositionUsesScalarSelectionsAndSurvivesStyleChanges()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        backend.SetValue(context, document, "edit", UiControlValue.Text("A😀B"));
        backend.ProcessInput(context, new(default, KeyModifier.None, new([
            new KeyInputChange(KeyCode.Home, true, false, KeyModifier.None),
            new KeyInputChange(KeyCode.Home, false, false, KeyModifier.None),
            new KeyInputChange(KeyCode.RightArrow, true, false, KeyModifier.None),
            new KeyInputChange(KeyCode.RightArrow, false, false, KeyModifier.None),
            new KeyInputChange(KeyCode.RightArrow, true, false, KeyModifier.Shift),
            new KeyInputChange(KeyCode.RightArrow, false, false, KeyModifier.Shift)])));
        var state = new TextCompositionState(1, "中😀", 1, 1);
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new([new TextCompositionChange(TextCompositionPhase.Started, state)]), textComposition: state));
        Assert.Equal(UiControlValue.Text("A中😀B"), backend.GetValue(context, document, "edit"));
        backend.SetAttribute(context, document, "edit", "style", "width: 180px;");
        backend.RefreshLayout(context);
        Composition(backend, context, 1, TextCompositionPhase.Updated, "");
        Assert.Equal(UiControlValue.Text("AB"), backend.GetValue(context, document, "edit"));
        Composition(backend, context, 1, TextCompositionPhase.Cancelled, "");
        Assert.Equal(UiControlValue.Text("A😀B"), backend.GetValue(context, document, "edit"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void RetiredCompositionDoesNotValidateANewControlsResources()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiDocumentHandle document = OpenCompositionDocument(backend, context);
        Composition(backend, context, 1, TextCompositionPhase.Started, "Candidate");
        backend.FocusElement(context, document, "other");
        backend.SetAttribute(context, document, "other", "style", "font-family: UnavailableCompositionFont;");
        backend.RefreshLayout(context);
        Composition(backend, context, 1, TextCompositionPhase.Committed, "Retired");
        Assert.Equal(UiControlValue.Text(""), backend.GetValue(context, document, "other"));
        Assert.Throws<InvalidOperationException>(() =>
            Composition(backend, context, 2, TextCompositionPhase.Started, "Current"));
        backend.DestroyContext(context);
    }

    [Fact]
    public void UnchangedCompositionDoesNotAddPerFrameInteropWork()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        OpenCompositionDocument(backend, context);
        UiInputSnapshot input = EmptyInput();
        for (int frame = 0; frame < 100; frame++)
            backend.ProcessInput(context, input);
        for (int sample = 0; sample < 5; sample++)
        {
            long baseline = GC.GetAllocatedBytesForCurrentThread();
            for (int frame = 0; frame < 1_000; frame++)
                backend.GetTime(context);
            long interopBytes = GC.GetAllocatedBytesForCurrentThread() - baseline;
            long allocated = GC.GetAllocatedBytesForCurrentThread();
            long started = Stopwatch.GetTimestamp();
            for (int frame = 0; frame < 1_000; frame++)
                backend.ProcessInput(context, input);
            TimeSpan elapsed = Stopwatch.GetElapsedTime(started);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - allocated;
            m_output.WriteLine($"sample={sample} frames=1000 bytes={bytes} singleCallBytes={interopBytes} milliseconds={elapsed.TotalMilliseconds:F4}");
            Assert.True(bytes <= interopBytes, $"Unchanged input allocated {bytes} bytes; one protected native call allocated {interopBytes}.");
        }
        backend.DestroyContext(context);
    }

    [Fact]
    public void CompositionHonorsControlLimitAndRetiresAcrossDocumentCycles()
    {
        using var backend = new RmlUiBackend();
        backend.RegisterFont(new(File.ReadAllBytes(FontPath()), 0, "Composition", TextFontStyle.Normal, 400));
        UiContextHandle context = backend.CreateContext(new("composition", 640, 360));
        UiContextHandle independent = backend.CreateContext(new("independent", 640, 360));
        UiDocumentHandle other = OpenCompositionDocument(backend, independent);
        for (ulong operation = 1; operation <= 100; operation++)
        {
            UiDocumentHandle document = OpenCompositionDocument(backend, context);
            backend.SetAttribute(context, document, "edit", "maxlength", "2");
            Composition(backend, context, operation * 2, TextCompositionPhase.Started, "ABCDE");
            Composition(backend, context, operation * 2, TextCompositionPhase.Committed, "ABC");
            Assert.Equal(UiControlValue.Text("AB"), backend.GetValue(context, document, "edit"));
            Assert.Equal(UiControlValue.Text(""), backend.GetValue(independent, other, "edit"));
            Composition(backend, context, operation * 2 + 1, TextCompositionPhase.Started, "待");
            backend.CloseDocument(context, document);
        }
        backend.DestroyContext(context);
        backend.DestroyContext(independent);
    }

    private static UiDocumentHandle OpenCompositionDocument(
        RmlUiBackend backend,
        UiContextHandle context
    ) {
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>body { font-family: Composition; font-size: 18px; }
            input { width: 240px; height: 30px; display: block; }</style></head>
            <body><form><input id="edit" type="text"/><input id="other" type="text"/></form></body></rml>
            """));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        Assert.Equal(UiFocusResult.Focused, backend.FocusElement(context, document, "edit"));
        return document;
    }

    private static void Composition(
        RmlUiBackend backend,
        UiContextHandle context,
        ulong operation,
        TextCompositionPhase phase,
        string text
    ) {
        var state = new TextCompositionState(operation, text);
        backend.ProcessInput(context, new(default, KeyModifier.None,
            new([new TextCompositionChange(phase, state)]),
            textComposition: phase is TextCompositionPhase.Started or TextCompositionPhase.Updated ? state : null));
    }
}
