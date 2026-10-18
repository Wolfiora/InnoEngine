using System;
using Inno.Adapter.Presentation.ImGui;
using Xunit;

namespace Inno.Editor.ImGui.Tests;

public sealed class ImGuiInteractionOptionsTests
{
    [Theory]
    [InlineData(false, 1)]
    [InlineData(true, -1)]
    public void ProductInteractionConventionsAreExplicitAndImmutable(
        bool commandKeys,
        int wheelDirection
    ) {
        var options = new ImGuiInteractionOptions(commandKeys, wheelDirection);
        Assert.Equal(commandKeys, options.commandKeyBehavior);
        Assert.Equal(wheelDirection, options.horizontalWheelDirection);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(-2)]
    public void WheelDirectionCannotSilentlyDisableOrAmplifyInput(int direction)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ImGuiInteractionOptions(false, direction));
    }
}
