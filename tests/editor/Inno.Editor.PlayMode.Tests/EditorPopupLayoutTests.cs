using System;
using System.Numerics;

using Inno.Native.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class EditorPopupLayoutTests
{
    [Fact]
    public void ContextMenuSearchUsesCompletePopupContentWidth()
    {
        ImGuiContextPtr context = NativeImGui.CreateContext();
        try
        {
            ConfigureContext();
            EditorWidget.SetupStyle();
            string query = string.Empty;
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowSize(new Vector2(400f, 280f), ImGuiCond.Always);
                _ = NativeImGui.Begin("Context menu bounds");
                _ = NativeImGui.Button("Target");
                NativeImGui.OpenPopup("commands");

                Assert.True(EditorWidget.BeginContextMenu("commands"));
                _ = EditorWidget.SearchInput("commands", "Search commands...", ref query);
                if (frame == 2)
                {
                    float inputWidth = NativeImGui.GetItemRectSize().X;
                    float contentWidth = NativeImGui.GetContentRegionAvail().X;
                    Assert.True(inputWidth >= EditorWidget.style.searchPopupWidth - 1f);
                    Assert.InRange(MathF.Abs(inputWidth - contentWidth), 0f, 2f);
                    Assert.InRange(NativeImGui.GetScrollMaxX(), 0f, 1f);
                }
                NativeImGui.Selectable("Rename");
                EditorWidget.EndContextMenu();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.DestroyContext(context);
        }
    }

    [Fact]
    public void TwoItemSelectorOpensBelowControlWithoutOverflow()
    {
        ImGuiContextPtr context = NativeImGui.CreateContext();
        try
        {
            ConfigureContext();
            EditorWidget.SetupStyle();

            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), ImGuiCond.Always);
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), ImGuiCond.Always);
                _ = NativeImGui.Begin("Selector bounds", ImGuiWindowFlags.NoTitleBar);
                NativeImGui.SetNextItemWidth(140f);
                float triggerBottom = NativeImGui.GetCursorScreenPos().Y + NativeImGui.GetFrameHeight();
                NativeImGui.OpenPopup("##menu_selector_popup_tag");

                Assert.True(EditorWidget.BeginBoundedCombo("tag", "Untagged"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                _ = NativeImGui.Selectable("Untagged");
                _ = NativeImGui.Selectable("Test");
                if (frame == 2)
                {
                    Assert.InRange(popupPosition.Y, triggerBottom - 1f, triggerBottom + 1f);
                    Assert.True(popupSize.Y <= 250f * 0.45f + 1f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                    Assert.InRange(NativeImGui.GetScrollMaxY(), 0f, 1f);
                }
                EditorWidget.EndBoundedCombo();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.DestroyContext(context);
        }
    }

    [Fact]
    public void LongSelectorNearWindowBottomScrollsWithoutOpeningUpward()
    {
        ImGuiContextPtr context = NativeImGui.CreateContext();
        try
        {
            ConfigureContext();
            EditorWidget.SetupStyle();
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), ImGuiCond.Always);
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), ImGuiCond.Always);
                _ = NativeImGui.Begin("Selector near bottom", ImGuiWindowFlags.NoTitleBar);
                NativeImGui.SetCursorScreenPos(new Vector2(35f, 225f));
                float triggerBottom = NativeImGui.GetCursorScreenPos().Y + NativeImGui.GetFrameHeight();
                NativeImGui.SetNextItemWidth(140f);
                NativeImGui.OpenPopup("##menu_selector_popup_long");

                Assert.True(EditorWidget.BeginBoundedCombo("long", "Choose"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                for (int index = 0; index < 40; index++)
                    _ = NativeImGui.Selectable($"Item {index}");
                if (frame == 2)
                {
                    Assert.InRange(popupPosition.Y, triggerBottom - 1f, triggerBottom + 1f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                    Assert.True(NativeImGui.GetScrollMaxY() > 0f);
                }
                EditorWidget.EndBoundedCombo();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.DestroyContext(context);
        }
    }

    [Fact]
    public void SelectorInsideLayoutChildUsesContainingPanelBounds()
    {
        ImGuiContextPtr context = NativeImGui.CreateContext();
        try
        {
            ConfigureContext();
            EditorWidget.SetupStyle();
            for (int frame = 0; frame < 3; frame++)
            {
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f, 20f), ImGuiCond.Always);
                NativeImGui.SetNextWindowSize(new Vector2(300f, 250f), ImGuiCond.Always);
                _ = NativeImGui.Begin("Panel with layout child", ImGuiWindowFlags.NoTitleBar);
                _ = NativeImGui.BeginChild("Fields", new Vector2(200f, 55f));
                NativeImGui.SetNextItemWidth(140f);
                NativeImGui.OpenPopup("##menu_selector_popup_nested");

                Assert.True(EditorWidget.BeginBoundedCombo("nested", "Choose"));
                Vector2 popupPosition = NativeImGui.GetWindowPos();
                Vector2 popupSize = NativeImGui.GetWindowSize();
                for (int index = 0; index < 40; index++)
                    _ = NativeImGui.Selectable($"Item {index}");
                if (frame == 2)
                {
                    Assert.True(popupSize.Y > 55f);
                    Assert.True(popupPosition.Y + popupSize.Y <= 270f + 1f);
                }
                EditorWidget.EndBoundedCombo();
                NativeImGui.EndChild();
                NativeImGui.End();
                NativeImGui.Render();
            }
        }
        finally
        {
            NativeImGui.DestroyContext(context);
        }
    }

    private static void ConfigureContext()
    {
        ImGuiIOPtr io = NativeImGui.GetIO();
        io.DisplaySize = new Vector2(640f, 480f);
        io.DeltaTime = 1f / 60f;
        io.BackendFlags |= ImGuiBackendFlags.RendererHasTextures;
        io.Fonts.RendererHasTextures = true;
    }
}
