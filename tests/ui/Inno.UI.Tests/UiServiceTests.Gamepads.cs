using System.Collections.Generic;
using System.Linq;

using Inno.Adapter.UI.RmlUi;
using Inno.Core.Input;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiServiceTests
{
    [Fact]
    public void GamepadNavigationUsesNativeFocusAndActivatesOncePerPress()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new("gamepad", 640, 360));
        UiDocumentHandle document = OpenNavigationDocument(backend, context);
        backend.FocusElement(context, document, "first");
        var down = new GamepadState(1, [GamepadButton.DpadDown]);
        backend.ProcessInput(context, GamepadFrame(down, new GamepadButtonChange(1, GamepadButton.DpadDown, true)));
        Assert.Equal(new UiFocusTarget(document, "second"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, GamepadFrame(new(1), new GamepadButtonChange(1, GamepadButton.DpadDown, false)));
        backend.DrainEvents(context);
        var activate = new GamepadState(1, [GamepadButton.South]);
        backend.ProcessInput(context, GamepadFrame(activate, new GamepadButtonChange(1, GamepadButton.South, true)));
        Assert.Single(backend.DrainEvents(context).Where(item => item.type == UiEventType.Click && item.targetId == "second"));
        backend.AdvanceTime(context, 10);
        backend.ProcessInput(context, GamepadFrame(activate));
        Assert.DoesNotContain(backend.DrainEvents(context), item => item.type == UiEventType.Click);
        backend.ProcessInput(context, GamepadFrame(new(1), new GamepadButtonChange(1, GamepadButton.South, false)));
        backend.ProcessInput(context, GamepadFrame(new(1),
            new GamepadButtonChange(1, GamepadButton.DpadUp, true),
            new GamepadButtonChange(1, GamepadButton.DpadUp, false)));
        Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
        backend.DestroyContext(context);
    }

    [Fact]
    public void GamepadRepeatUsesContextTimeAndStopsOnFocusOrDeviceRetirement()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle first = backend.CreateContext(new("first", 640, 360));
        UiContextHandle second = backend.CreateContext(new("second", 640, 360));
        UiDocumentHandle firstDocument = OpenNavigationDocument(backend, first);
        UiDocumentHandle secondDocument = OpenNavigationDocument(backend, second);
        backend.SetTimePaused(first, true);
        backend.SetTimePaused(second, true);
        backend.FocusElement(first, firstDocument, "first");
        backend.FocusElement(second, secondDocument, "first");
        var down = new GamepadState(1, [GamepadButton.DpadDown]);
        UiInputSnapshot press = GamepadFrame(down, new GamepadButtonChange(1, GamepadButton.DpadDown, true));
        UiInputSnapshot held = GamepadFrame(down);
        backend.ProcessInput(first, press);
        backend.ProcessInput(second, press);
        for (int frame = 0; frame < 10; frame++)
            backend.Update(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "second"), backend.GetFocusedElement(first));
        backend.AdvanceTime(first, .39);
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "second"), backend.GetFocusedElement(first));
        backend.AdvanceTime(first, .02);
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "third"), backend.GetFocusedElement(first));
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "third"), backend.GetFocusedElement(first));
        backend.AdvanceTime(first, 10);
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "fourth"), backend.GetFocusedElement(first));
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "fourth"), backend.GetFocusedElement(first));
        backend.ProcessInput(second, held);
        Assert.Equal(new UiFocusTarget(secondDocument, "second"), backend.GetFocusedElement(second));
        backend.FocusElement(first, firstDocument, "first");
        backend.AdvanceTime(first, 1);
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "first"), backend.GetFocusedElement(first));
        backend.ProcessInput(first, press);
        backend.ProcessInput(first, new(default, KeyModifier.None, new([new GamepadConnectionChange(1, false)])));
        backend.AdvanceTime(first, 1);
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(firstDocument, "second"), backend.GetFocusedElement(first));
        backend.CloseDocument(first, firstDocument);
        UiDocumentHandle replacement = OpenNavigationDocument(backend, first);
        backend.FocusElement(first, replacement, "first");
        backend.ProcessInput(first, held);
        Assert.Equal(new UiFocusTarget(replacement, "first"), backend.GetFocusedElement(first));
        backend.DestroyContext(first);
        backend.DestroyContext(second);
    }

    [Fact]
    public void StickNavigationUsesHysteresisAndDoesNotReplayLostInputOwnership()
    {
        using var backend = new RmlUiBackend();
        UiContextHandle context = backend.CreateContext(new("stick", 640, 360));
        UiDocumentHandle document = OpenNavigationDocument(backend, context);
        backend.FocusElement(context, document, "first");
        backend.SetTimePaused(context, true);
        backend.ProcessInput(context, Axis(.3f));
        Assert.Equal(new UiFocusTarget(document, "first"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, Axis(.6f));
        Assert.Equal(new UiFocusTarget(document, "second"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, Axis(.5f));
        backend.ProcessInput(context, Axis(.6f));
        Assert.Equal(new UiFocusTarget(document, "second"), backend.GetFocusedElement(context));
        backend.AdvanceTime(context, .41);
        backend.ProcessInput(context, Axis(.5f));
        Assert.Equal(new UiFocusTarget(document, "third"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, Axis(.3f));
        backend.AdvanceTime(context, 1);
        backend.ProcessInput(context, Axis(.5f));
        Assert.Equal(new UiFocusTarget(document, "third"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, Axis(-.7f));
        Assert.Equal(new UiFocusTarget(document, "second"), backend.GetFocusedElement(context));
        backend.ProcessInput(context, EmptyInput());
        backend.AdvanceTime(context, 1);
        backend.ProcessInput(context, GamepadFrame(new(1, axes: new Dictionary<GamepadAxis, float> { [GamepadAxis.LeftY] = -.7f })));
        Assert.Equal(new UiFocusTarget(document, "second"), backend.GetFocusedElement(context));
        backend.DestroyContext(context);

        static UiInputSnapshot Axis(float value) => GamepadFrame(
            new(1, axes: new Dictionary<GamepadAxis, float> { [GamepadAxis.LeftY] = value }),
            new GamepadAxisChange(1, GamepadAxis.LeftY, value));
    }

    private static UiDocumentHandle OpenNavigationDocument(
        RmlUiBackend backend,
        UiContextHandle context
    ) {
        UiDocumentHandle document = backend.LoadDocument(context, Rml("""
            <rml><head><style>
              body { margin: 0; }
              div { width: 120px; height: 40px; display: block; tab-index: auto; nav: auto; }
            </style></head><body>
              <div id="first"/><div id="second"/><div id="third"/><div id="fourth"/>
            </body></rml>
            """));
        backend.ShowDocument(context, document);
        backend.RefreshLayout(context);
        return document;
    }

    private static UiInputSnapshot GamepadFrame(
        GamepadState state,
        params InputChange[] changes
    ) => new(default, KeyModifier.None, new(changes), new Dictionary<ulong, GamepadState> { [state.deviceId] = state });
}
