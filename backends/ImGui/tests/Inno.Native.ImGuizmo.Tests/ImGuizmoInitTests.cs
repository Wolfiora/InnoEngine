using Xunit;
using Xunit.Abstractions;

namespace Inno.Native.ImGuizmo.Tests;

public sealed class ImGuizmoInitTests
{
    private readonly ITestOutputHelper output;

    public ImGuizmoInitTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Init_ShouldLoad()
    {
        bool isUsing = ImGuizmo.IsUsing();
        output.WriteLine($"ImGuizmo.IsUsing: {isUsing}");
        Assert.False(isUsing);
    }

    [Fact]
    public unsafe void OwnedStyleCanRetireWithoutReleasingTheGlobalBorrow()
    {
        Style* borrowed = ImGuizmo.GetStyle();
        Assert.True(borrowed != null);
        Style* owned = ImGuizmo.StyleStyle();
        Assert.True(owned != null);
        Assert.True(owned != borrowed);
        try
        {
            Assert.Equal(borrowed->Colors.Length, owned->Colors.Length);
            Assert.True(owned->TranslationLineThickness > 0);
        }
        finally
        {
            ImGuizmo.StyleDestroy(new StylePtr(owned));
        }
        Assert.True(ImGuizmo.GetStyle() == borrowed);
        Assert.True(borrowed->TranslationLineThickness > 0);
    }

    [Fact]
    public void IntegerScopeAndSingleStringIdRestoreTheirParentState()
    {
        uint original = ImGuizmo.GetID("gizmo-probe");
        ImGuizmo.PushID(9137);
        try
        {
            uint scoped = ImGuizmo.GetID("gizmo-probe");
            Assert.NotEqual(original, scoped);
            Assert.Equal(scoped, ImGuizmo.GetID("gizmo-probe"));
        }
        finally
        {
            ImGuizmo.PopID();
        }
        Assert.Equal(original, ImGuizmo.GetID("gizmo-probe"));
    }
}
