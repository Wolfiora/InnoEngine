using System;
using System.Collections.Generic;

namespace Inno.Editor.ImGui.ImGuiWidget;

internal sealed class TreeWidgetWindowState
{
    internal readonly HashSet<uint> navigationItemIDs = [];
    internal readonly List<TreeWidgetNodeState> treeNodeStack = [];
    internal readonly List<string> lastNodeIDsByDepth = [];
    internal readonly List<TreeWidgetLineSegment> lineSegments = [];
    internal readonly Dictionary<string, bool> hasNextSiblingByID = new(StringComparer.Ordinal);
    internal readonly Dictionary<string, bool> openStatesByID = new(StringComparer.Ordinal);
    internal ulong expansionRevision;
    internal uint activeContentID;
    internal string? activeContentOwner;
    internal int lastFrame = -1;
}
