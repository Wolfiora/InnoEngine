using Xunit;
using System;
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
        Style* borrowed = ImGuizmo.Raw.GetStyle();
        Assert.True(borrowed != null);
        Style* owned = ImGuizmo.Raw.StyleStyle();
        Assert.True(owned != null);
        Assert.True(owned != borrowed);
        try
        {
            ReadOnlySpan<ImVec4C> borrowedColors = borrowed->Colors;
            ReadOnlySpan<ImVec4C> ownedColors = owned->Colors;
            Assert.Equal(borrowedColors.Length, ownedColors.Length);
            Assert.True(owned->TranslationLineThickness > 0);
        }
        finally
        {
            ImGuizmo.Raw.StyleDestroy(owned);
        }
        Assert.True(ImGuizmo.Raw.GetStyle() == borrowed);
        Assert.True(borrowed->TranslationLineThickness > 0);
    }

    [Fact]
    public unsafe void IntegerScopeAndSingleStringIdRestoreTheirParentState()
    {
        ReadOnlySpan<byte> text = "gizmo-probe\0"u8;
        fixed (byte* bytes = text)
        {
            uint original = ImGuizmo.Raw.GetID((sbyte*)bytes);
            ImGuizmo.PushID(9137);
            try
            {
                uint scoped = ImGuizmo.Raw.GetID((sbyte*)bytes);
                Assert.NotEqual(original, scoped);
                Assert.Equal(scoped, ImGuizmo.Raw.GetID((sbyte*)bytes));
            }
            finally
            {
                ImGuizmo.PopId();
            }
            Assert.Equal(original, ImGuizmo.Raw.GetID((sbyte*)bytes));
        }
    }
}
