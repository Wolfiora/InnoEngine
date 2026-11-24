using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BGCS.Runtime;
using Xunit;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Editor.Interactions;
using Inno.Editor.Panel.Hierarchy;
using Inno.Native.ImGui;
using Inno.Scene;
using Inno.Testing.Editor.Native;

using NativeGui = Inno.Native.ImGui.ImGui;
using NativeVector = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.Panels.Native.Tests;

public sealed class LargePanelWorkflowTests
{
    [Theory]
    [InlineData("hierarchy")]
    [InlineData("list")]
    [InlineData("grid")]
    [InlineData("tree")]
    [Trait("Acceptance", "Workflows")]
    public unsafe void RangeRenameAndDragWorkAcrossClippedRows(string view)
    {
        using var host = new NativeProjectPanelHost(
            typeof(LargePanelWorkflowTests).Assembly,
            supportsVertexOffsets: true
        );
        bool hierarchy = view == "hierarchy";
        GameScene? scene = hierarchy ? LoadInteractionHierarchy(host) : null;
        if (!hierarchy)
        {
            for (int index = 0; index < 256; index++)
            {
                Directory.CreateDirectory(Path.Combine(host.projectRoot, "Assets", "Folder " + index.ToString("D5")));
            }
            host.assets.Rescan();
        }
        EditorPanelExtension panel = host.native.GetPanel(hierarchy ? "scene.hierarchy" : "asset.file-browser");
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        if (view == "grid")
        {
            SelectGrid(host, panel);
        }
        IdentityObject first = hierarchy ? scene!.GetObjects()[0] : Entry("Folder 00000");
        IdentityObject last = hierarchy ? scene!.GetObjects()[^1] : Entry("Folder 00255");
        string area = hierarchy ? "panel/scene.hierarchy" : "panel/asset.file-browser";
        string windowName = hierarchy ? "##HierarchyContent" : view == "tree" ? "##TreeScroll" : "##EntriesScroll";
        EditorInteractions interactions = host.runtime.interactions;
        if (view is "list" or "grid")
        {
            SetLargeQuery(host, panel, "Folder 00042");
            ImGuiWindow* filtered = FindInteractionWindow(windowName);
            NativeGui.Raw.AddMousePosEvent(
                NativeGui.Raw.GetIO(),
                filtered->Pos.X + filtered->Size.X - 60f,
                filtered->Pos.Y + filtered->Size.Y * 0.7f
            );
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 0);
            host.native.DrawPanel(panel);
            NativePanelInput.Key(host.native, panel, ImGuiKey.Home);
            Assert.Same(Entry("Folder 00042"), interactions.selection.selectedTarget);
            NativePanelInput.Key(host.native, panel, ImGuiKey.End);
            Assert.Same(Entry("Folder 00042"), interactions.selection.selectedTarget);
            SetLargeQuery(host, panel, string.Empty);
            NativeGui.Raw.AddMousePosEvent(
                NativeGui.Raw.GetIO(),
                filtered->Pos.X + filtered->Size.X * 0.4f,
                filtered->Pos.Y + filtered->Size.Y * 0.7f
            );
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 0);
            host.native.DrawPanel(panel);
        }
        interactions.SetSelection(first);
        NativePanelInput.Focus(host.native, panel, windowName);
        NativePanelInput.Key(host.native, panel, ImGuiKey.End, shift: true);
        Assert.Equal(256, interactions.selection.count);
        Assert.True(interactions.selection.Contains(first));
        Assert.True(interactions.selection.Contains(last));
        NativePanelInput.Key(host.native, panel, ImGuiKey.End);
        Assert.Same(last, interactions.selection.selectedTarget);
        Assert.Equal(1, interactions.selection.count);
        string original = hierarchy ? ((GameObject)last).name : ((AssetFileEntry)last).name;
        Guid? objectID = hierarchy ? ((GameObject)last).identity.persistentID : null;
        Assert.True(interactions.For(area, last).Execute(hierarchy ? "hierarchy/rename" : "file-browser/rename"));
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
        Assert.NotEqual(0u, context->ActiveId);
        Assert.Equal(context->ActiveId, context->InputTextState.ID);
        Assert.Equal(FindInteractionWindow(windowName)->ID, context->ActiveIdWindow->ID);
        NativePanelInput.Key(host.native, panel, ImGuiKey.A, primary: true);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Backspace);
        using (NativeStringLease text = Utf8Codec.EncodeNullTerminated("Renamed last", allowsNull: false))
        {
            NativeGui.Raw.AddInputCharactersUTF8(NativeGui.Raw.GetIO(), (sbyte*)text.pointer);
            host.native.DrawPanel(panel);
            host.native.DrawPanel(panel);
        }
        NativePanelInput.Key(host.native, panel, ImGuiKey.Enter);
        Assert.True(interactions.history.canUndo);
        if (hierarchy)
        {
            Assert.Equal("Renamed last", ((GameObject)last).name);
            Assert.Equal(objectID, ((GameObject)last).identity.persistentID);
        }
        else
        {
            last = Entry("Renamed last");
        }
        Assert.True(interactions.history.Undo().succeeded);
        if (hierarchy)
        {
            Assert.Equal(original, ((GameObject)last).name);
        }
        else
        {
            Assert.True(Directory.Exists(Path.Combine(host.projectRoot, "Assets", original)));
        }
        Assert.True(interactions.history.Redo().succeeded);
        if (!hierarchy)
        {
            last = Entry("Renamed last");
        }
        interactions.SetSelection(last);
        RuntimeIdentity drag = interactions.For(area, last).BeginDrag(new EditorDragData(last, "Clipped source"));
        Assert.Equal(hierarchy ? ((GameObject)last).identity.runtimeIdentity : ((AssetFileEntry)last).identity.runtimeIdentity, drag);
        ImGuiWindow* window = FindInteractionWindow(windowName);
        NativeGui.Raw.AddMousePosEvent(NativeGui.Raw.GetIO(), window->Pos.X + window->Size.X * 0.7f, window->Pos.Y + 140f);
        NativeGui.Raw.AddMouseWheelEvent(NativeGui.Raw.GetIO(), 0f, -100_000f);
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        Assert.True(window->Scroll.Y > window->Size.Y);
        NativeGui.Raw.AddMouseWheelEvent(NativeGui.Raw.GetIO(), 0f, 100_000f);
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        Assert.Equal(0f, window->Scroll.Y);
        Assert.True(interactions.For(area).TryGetActiveDragIdentity(out RuntimeIdentity retained));
        Assert.Equal(drag, retained);
        EditorInteraction drop = hierarchy
                                 ? interactions.For(area, new HierarchyObjectDropTarget((GameObject)first))
                                 : interactions.For(area, ((AssetFileEntry)first).assetPath.ToString());
        Assert.True(drop.QueryDrop(drag, EditorDropPlacement.Into).canDrop);
        Assert.True(drop.Drop(drag, EditorDropPlacement.Into).accepted);
        if (hierarchy)
        {
            Assert.Same(((GameObject)first).transform, ((GameObject)last).transform.parent);
            Assert.Equal(256, scene!.GetObjects().Count);
        }
        else
        {
            Assert.True(Directory.Exists(Path.Combine(host.projectRoot, "Assets", "Folder 00000", "Renamed last")));
        }
        Assert.True(interactions.history.Undo().succeeded);
        if (hierarchy)
        {
            Assert.Null(((GameObject)last).transform.parent);
        }
        else
        {
            Assert.True(Directory.Exists(Path.Combine(host.projectRoot, "Assets", "Renamed last")));
        }
        Assert.True(interactions.history.Redo().succeeded);
        host.native.DrawPanel(panel);

        AssetFileEntry Entry(string name)
        {
            Assert.True(host.assets.TryGetFileSystemEntry(AssetPath.Project(name), out AssetFileEntry entry));
            return entry;
        }
    }

    private static unsafe void SetLargeQuery(
        NativeProjectPanelHost host,
        EditorPanelExtension panel,
        string query
    ) {
        FocusAssetSearch(host, panel);
        NativePanelInput.Key(host.native, panel, ImGuiKey.A, primary: true);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Backspace);
        using NativeStringLease text = Utf8Codec.EncodeNullTerminated(query, allowsNull: false);
        NativeGui.Raw.AddInputCharactersUTF8(NativeGui.Raw.GetIO(), (sbyte*)text.pointer);
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
    }

    private static unsafe void SelectGrid(
        NativeProjectPanelHost host,
        EditorPanelExtension panel
    ) {
        ImGuiWindow* pane = FindInteractionWindow("##ContentPane");
        float start = pane->DC.CursorStartPos.Y;
        for (float offset = 24f; offset <= 64f; offset += 4f)
        {
            NativeGui.Raw.AddMousePosEvent(NativeGui.Raw.GetIO(), pane->Pos.X + 20f, start + offset);
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(NativeGui.Raw.GetIO(), 0, 0);
            host.native.DrawPanel(panel);
            host.runtime.SaveState();
            if (File.ReadAllText(Path.Combine(host.projectRoot, "editor.ini")).Contains("viewMode=\"Grid\"", StringComparison.Ordinal))
            {
                return;
            }
        }
        throw new InvalidOperationException("Ordinary pointer input did not select the actual Browser grid.");
    }

    private static GameScene LoadInteractionHierarchy(NativeProjectPanelHost host)
    {
        GameScene scene = host.CreateScene();
        for (int index = 0; index < 256; index++)
        {
            _ = scene.CreateObject("Object " + index.ToString("D5"));
        }
        _ = host.workspace.Save(scene, string.Empty);
        return scene;
    }

    private static unsafe void FocusAssetSearch(
        NativeProjectPanelHost host,
        EditorPanelExtension panel
    ) {
        ImGuiWindow* pane = FindInteractionWindow("##ContentPane");
        Vector2 start = Unsafe.BitCast<NativeVector, Vector2>(pane->DC.CursorStartPos);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        // Locate the existing text input using ordinary clicks; no private Panel field is read or changed.
        for (float offset = 24f; offset <= 64f; offset += 4f)
        {
            NativeGui.Raw.AddMousePosEvent(io, pane->Pos.X + pane->Size.X - 70f, start.Y + offset);
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 0);
            host.native.DrawPanel(panel);
            ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
            if (context->InputTextState.ID != 0 && context->ActiveId == context->InputTextState.ID)
            {
                return;
            }
        }
        throw new InvalidOperationException("Ordinary pointer input did not focus the existing asset search field.");
    }

    private static unsafe ImGuiWindow* FindInteractionWindow(string fragment)
    {
        ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
        for (int index = 0; index < context->Windows.Size; index++)
        {
            ImGuiWindow* window = context->Windows.Data[index];
            string? name = Marshal.PtrToStringUTF8((nint)window->Name);
            if (window->Active != 0 && name?.Contains(fragment, StringComparison.Ordinal) == true)
            {
                return window;
            }
        }
        throw new InvalidOperationException("The drawn native input owner is absent: " + fragment);
    }
}
