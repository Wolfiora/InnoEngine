using System;
using System.Numerics;
using BGCS.Runtime;
using Inno.Native.ImGui;
using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Exposes a script-safe, pointer-free subset of Dear ImGui for custom Editor tools.
/// Every call is valid only while an Editor drawing callback is active.
/// </summary>
public static unsafe partial class ImGui
{
    /// <summary>
    /// Draws unformatted text.
    /// </summary>
    /// <param name="text">
    /// Literal text to draw; percent characters are displayed without format-string interpretation.
    /// </param>
    public static void Text(string text)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TextUnformatted(text);
    }

    /// <summary>
    /// Draws a standard button.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional ImGui identity suffix.
    /// </param>
    /// <param name="size">
    /// The requested size, or zero components for automatic sizing.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the button is activated.
    /// </returns>
    public static bool Button(
        string label,
        Vector2 size = default
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.Button(label, size);
    }

    /// <summary>
    /// Draws a compact button.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the button is activated.
    /// </returns>
    /// <param name="label">
    /// The visible label and optional ImGui identity suffix.
    /// </param>
    public static bool SmallButton(string label)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.SmallButton(label);
    }

    /// <summary>
    /// Draws and edits a Boolean value.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    /// <param name="value">
    /// The checked state, updated when the user toggles the control.
    /// </param>
    public static bool Checkbox(
        string label,
        ref bool value
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.Checkbox(label, ref value);
    }

    /// <summary>
    /// Draws and edits a bounded UTF-8 text value.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="value">
    /// The managed string to display and update.
    /// </param>
    /// <param name="capacity">
    /// The positive maximum UTF-8 buffer capacity, including the terminator.
    /// </param>
    /// <param name="flags">
    /// Text editing behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the control reports an edit or submit event.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="capacity"/> is not positive.
    /// </exception>
    public static bool InputText(
        string label,
        ref string value,
        int capacity = 1024,
        ImGuiInputTextFlags flags = ImGuiInputTextFlags.None
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        return ImGuiUtf8Buffer.InputText(label, null, ref value, (nuint)capacity, flags);
    }

    /// <summary>
    /// Draws and edits a bounded UTF-8 text value with placeholder text.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="hint">
    /// The placeholder shown while the value is empty.
    /// </param>
    /// <param name="value">
    /// The managed string to display and update.
    /// </param>
    /// <param name="capacity">
    /// The positive maximum UTF-8 buffer capacity, including the terminator.
    /// </param>
    /// <param name="flags">
    /// Text editing behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the control reports an edit or submit event.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="capacity"/> is not positive.
    /// </exception>
    public static bool InputTextWithHint(
        string label,
        string hint,
        ref string value,
        int capacity = 1024,
        ImGuiInputTextFlags flags = ImGuiInputTextFlags.None
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        return ImGuiUtf8Buffer.InputText(label, hint, ref value, (nuint)capacity, flags);
    }

    /// <summary>
    /// Draws and edits an integer value.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    /// <param name="value">
    /// The integer displayed by the control and replaced after an accepted edit.
    /// </param>
    /// <param name="step">
    /// Increment used by the ordinary step button.
    /// </param>
    /// <param name="fastStep">
    /// Increment used by the accelerated step button.
    /// </param>
    /// <param name="flags">
    /// Text editing options for the numeric field.
    /// </param>
    public static bool InputInt(
        string label,
        ref int value,
        int step = 1,
        int fastStep = 100,
        ImGuiInputTextFlags flags = ImGuiInputTextFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.InputInt(label, ref value, step, fastStep, (int)flags);
    }

    /// <summary>
    /// Draws and edits a floating-point value.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    /// <param name="value">
    /// The number displayed with three fractional digits and replaced after an accepted edit.
    /// </param>
    public static bool InputFloat(
        string label,
        ref float value
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.InputFloat(label, ref value, 0f, 0f, "%.3f", 0);
    }

    /// <summary>
    /// Draws a floating-point drag control.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="value">
    /// The value to display and update.
    /// </param>
    /// <param name="speed">
    /// The drag speed.
    /// </param>
    /// <param name="minimum">
    /// The inclusive minimum, or zero with a zero maximum for no clamping.
    /// </param>
    /// <param name="maximum">
    /// The inclusive maximum, or zero with a zero minimum for no clamping.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    public static bool DragFloat(
        string label,
        ref float value,
        float speed = 1f,
        float minimum = 0f,
        float maximum = 0f
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.DragFloat(label, ref value, speed, minimum, maximum);
    }

    /// <summary>
    /// Draws a bounded floating-point slider.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="value">
    /// The value to display and update.
    /// </param>
    /// <param name="minimum">
    /// The inclusive minimum.
    /// </param>
    /// <param name="maximum">
    /// The inclusive maximum.
    /// </param>
    /// <param name="flags">
    /// Slider behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    public static bool SliderFloat(
        string label,
        ref float value,
        float minimum,
        float maximum,
        ImGuiSliderFlags flags = ImGuiSliderFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.SliderFloat(label, ref value, minimum, maximum, (int)flags);
    }

    /// <summary>
    /// Draws and edits a four-component color.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="value">
    /// The RGBA value to display and update.
    /// </param>
    /// <param name="flags">
    /// Color editing behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changes.
    /// </returns>
    public static unsafe bool ColorEdit4(
        string label,
        ref Vector4 value,
        ImGuiColorEditFlags flags = ImGuiColorEditFlags.None
    ) {
        fixed (Vector4* nativeValue = &value)
        {
            using NativeStringLease nativeLabel = Utf8Codec.EncodeNullTerminated(label, allowsNull: false);
            return RawImGui.Raw.ColorEdit4((sbyte*)nativeLabel.pointer, (float*)nativeValue, (int)flags) != 0;
        }
    }

    /// <summary>
    /// Displays a linear RGBA color through a display-sRGB picker and returns edits in linear space.
    /// </summary>
    /// <param name="label">
    /// The stable identity and visible label of the color control.
    /// </param>
    /// <param name="value">
    /// Linear RGB and alpha; only changed RGB channels are decoded, while alpha is never transfer-encoded.
    /// </param>
    /// <param name="flags">
    /// Dear ImGui color-edit flags; display modes interpret sRGB values and input storage must be RGB.
    /// </param>
    /// <returns>
    /// True if the display color changed and <paramref name="value"/> contains decoded linear RGB.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// HSV input storage conflicts with the linear-RGB value contract.
    /// </exception>
    public static bool ColorEditLinear4(
        string label,
        ref Vector4 value,
        ImGuiColorEditFlags flags = ImGuiColorEditFlags.None
    ) {
        if ((flags & ImGuiColorEditFlags.InputHsv) != 0)
            throw new ArgumentException("Linear RGBA editing requires RGB input storage.", nameof(flags));
        Vector4 display = new(LinearToSrgb(value.X), LinearToSrgb(value.Y), LinearToSrgb(value.Z), value.W);
        Vector4 original = display;
        if (!ColorEdit4(label, ref display, flags | ImGuiColorEditFlags.InputRgb))
            return false;
        value = new Vector4(
            display.X == original.X ? value.X : SrgbToLinear(display.X),
            display.Y == original.Y ? value.Y : SrgbToLinear(display.Y),
            display.Z == original.Z ? value.Z : SrgbToLinear(display.Z),
            display.W
        );
        return true;
    }

    /// <summary>
    /// Begins a child region.
    /// </summary>
    /// <param name="id">
    /// The child identity.
    /// </param>
    /// <param name="size">
    /// The requested child size.
    /// </param>
    /// <param name="flags">
    /// Child-region behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when child contents should be submitted.
    /// </returns>
    /// <param name="windowFlags">
    /// Scroll and window policies for the child.
    /// </param>
    public static bool BeginChild(
        string id,
        Vector2 size = default,
        ImGuiChildFlags flags = ImGuiChildFlags.None,
        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.BeginChild(id, size, (int)flags, (int)windowFlags);
    }

    /// <summary>
    /// Ends the current child region.
    /// </summary>
    public static void EndChild()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndChild();
    }

    /// <summary>
    /// Begins a table.
    /// </summary>
    /// <param name="id">
    /// The table identity.
    /// </param>
    /// <param name="columns">
    /// The positive column count.
    /// </param>
    /// <param name="flags">
    /// Table behavior.
    /// </param>
    /// <param name="size">
    /// The requested outer size.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when table contents should be submitted.
    /// </returns>
    /// <param name="innerWidth">
    /// Optional inner scrolling width; zero uses the outer size.
    /// </param>
    public static bool BeginTable(
        string id,
        int columns,
        ImGuiTableFlags flags = ImGuiTableFlags.None,
        Vector2 size = default,
        float innerWidth = 0f
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.BeginTable(id, columns, (int)flags, size, innerWidth);
    }

    /// <summary>
    /// Ends the current table.
    /// </summary>
    public static void EndTable()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndTable();
    }

    /// <summary>
    /// Declares one table column.
    /// </summary>
    /// <param name="label">
    /// The column label.
    /// </param>
    /// <param name="flags">
    /// Column behavior.
    /// </param>
    /// <param name="widthOrWeight">
    /// The initial fixed width or stretch weight.
    /// </param>
    public static void TableSetupColumn(
        string label,
        ImGuiTableColumnFlags flags = ImGuiTableColumnFlags.None,
        float widthOrWeight = 0f
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TableSetupColumn(label, (int)(flags), widthOrWeight);
    }

    /// <summary>
    /// Draws the table header row from configured column labels.
    /// </summary>
    public static void TableHeadersRow()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TableHeadersRow();
    }

    /// <summary>
    /// Advances to the next table row.
    /// </summary>
    /// <param name="flags">
    /// Row presentation flags.
    /// </param>
    /// <param name="minimumHeight">
    /// Minimum row height in UI pixels.
    /// </param>
    public static void TableNextRow(
        ImGuiTableRowFlags flags = ImGuiTableRowFlags.None,
        float minimumHeight = 0f
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TableNextRow((int)flags, minimumHeight);
    }

    /// <summary>
    /// Advances to the next table column.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the column is visible.
    /// </returns>
    public static bool TableNextColumn()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.TableNextColumn();
    }

    /// <summary>
    /// Begins a combo popup.
    /// </summary>
    /// <param name="label">
    /// The control label.
    /// </param>
    /// <param name="preview">
    /// The preview text.
    /// </param>
    /// <param name="flags">
    /// Combo behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when combo contents should be submitted.
    /// </returns>
    public static bool BeginCombo(
        string label,
        string preview,
        ImGuiComboFlags flags = ImGuiComboFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.BeginCombo(label, preview, (int)(flags));
    }

    /// <summary>
    /// Ends the current combo popup.
    /// </summary>
    public static void EndCombo()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndCombo();
    }

    /// <summary>
    /// Draws one selectable item.
    /// </summary>
    /// <param name="label">
    /// The visible label and identity.
    /// </param>
    /// <param name="selected">
    /// Whether the item is currently selected.
    /// </param>
    /// <param name="flags">
    /// Selectable behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the item is activated.
    /// </returns>
    /// <param name="size">
    /// The interaction bounds, or zero for the ordinary content size.
    /// </param>
    public static bool Selectable(
        string label,
        bool selected = false,
        ImGuiSelectableFlags flags = ImGuiSelectableFlags.None,
        Vector2 size = default
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.Selectable(label, selected, (int)flags, size);
    }

    /// <summary>
    /// Draws a collapsible section header.
    /// </summary>
    /// <param name="label">
    /// The visible label and identity.
    /// </param>
    /// <param name="flags">
    /// Tree-node behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the section is open.
    /// </returns>
    public static bool CollapsingHeader(
        string label,
        ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.CollapsingHeader(label, (int)(flags));
    }

    /// <summary>
    /// Draws a tree node and pushes its tree scope when it is open.
    /// </summary>
    /// <param name="label">
    /// The visible label and identity.
    /// </param>
    /// <param name="flags">
    /// Tree-node behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when child contents should be submitted.
    /// </returns>
    public static bool TreeNode(
        string label,
        ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.TreeNodeEx(label, (int)(flags));
    }

    /// <summary>
    /// Ends the current open tree-node scope.
    /// </summary>
    public static void TreePop()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TreePop();
    }

    /// <summary>
    /// Begins a tab bar.
    /// </summary>
    /// <param name="id">
    /// The tab bar identity.
    /// </param>
    /// <param name="flags">
    /// Tab-bar behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when tab contents should be submitted.
    /// </returns>
    public static bool BeginTabBar(
        string id,
        ImGuiTabBarFlags flags = ImGuiTabBarFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.BeginTabBar(id, (int)(flags));
    }

    /// <summary>
    /// Ends the current tab bar.
    /// </summary>
    public static void EndTabBar()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndTabBar();
    }

    /// <summary>
    /// Begins one tab item.
    /// </summary>
    /// <param name="label">
    /// The tab label and identity.
    /// </param>
    /// <param name="flags">
    /// Tab-item behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when tab contents should be submitted.
    /// </returns>
    public static bool BeginTabItem(
        string label,
        ImGuiTabItemFlags flags = ImGuiTabItemFlags.None
    ) {
        using NativeStringLease nativeLabel = Utf8Codec.EncodeNullTerminated(label, allowsNull: false);
        return RawImGui.Raw.BeginTabItem((sbyte*)nativeLabel.pointer, null, (int)flags) != 0;
    }

    /// <summary>
    /// Begins one closeable tab item.
    /// </summary>
    /// <param name="label">
    /// The tab label and identity.
    /// </param>
    /// <param name="isOpen">
    /// Receives whether the tab remains open.
    /// </param>
    /// <param name="flags">
    /// Tab-item behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when tab contents should be submitted.
    /// </returns>
    public static unsafe bool BeginTabItem(
        string label,
        ref bool isOpen,
        ImGuiTabItemFlags flags = ImGuiTabItemFlags.None
    ) {
        return RawImGui.BeginTabItem(label, ref isOpen, (int)flags);
    }

    /// <summary>
    /// Ends the current tab item.
    /// </summary>
    public static void EndTabItem()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndTabItem();
    }

    /// <summary>
    /// Opens a named popup.
    /// </summary>
    /// <param name="id">
    /// The popup identity.
    /// </param>
    /// <param name="flags">
    /// Popup opening behavior.
    /// </param>
    public static void OpenPopup(
        string id,
        ImGuiPopupFlags flags = ImGuiPopupFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.OpenPopup(id, (int)(flags));
    }

    /// <summary>
    /// Begins a named popup.
    /// </summary>
    /// <param name="id">
    /// The popup identity.
    /// </param>
    /// <param name="flags">
    /// Popup window behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when popup contents should be submitted.
    /// </returns>
    public static bool BeginPopup(
        string id,
        ImGuiWindowFlags flags = ImGuiWindowFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.BeginPopup(id, (int)(flags));
    }

    /// <summary>
    /// Ends the current popup.
    /// </summary>
    public static void EndPopup()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndPopup();
    }

    /// <summary>
    /// Closes the current popup.
    /// </summary>
    public static void CloseCurrentPopup()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.CloseCurrentPopup();
    }

    /// <summary>
    /// Begins a disabled block.
    /// </summary>
    /// <param name="disabled">
    /// Whether controls in the block are disabled.
    /// </param>
    public static void BeginDisabled(bool disabled = true)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.BeginDisabled(disabled);
    }

    /// <summary>
    /// Ends the current disabled block.
    /// </summary>
    public static void EndDisabled()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndDisabled();
    }

    /// <summary>
    /// Begins an item group.
    /// </summary>
    public static void BeginGroup()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.BeginGroup();
    }

    /// <summary>
    /// Ends the current item group.
    /// </summary>
    public static void EndGroup()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.EndGroup();
    }

    /// <summary>
    /// Pushes a string identity onto the ImGui ID stack.
    /// </summary>
    /// <param name="id">
    /// The stable identity used to locate the requested value.
    /// </param>
    public static void PushId(string id)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushID(id);
    }

    /// <summary>
    /// Pushes an integer identity onto the ImGui ID stack.
    /// </summary>
    /// <param name="id">
    /// The stable identity used to locate the requested value.
    /// </param>
    public static void PushId(int id)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushID(id);
    }

    /// <summary>
    /// Pops one identity from the ImGui ID stack.
    /// </summary>
    public static void PopId()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PopID();
    }

    /// <summary>
    /// Pushes one color override.
    /// </summary>
    /// <param name="color">
    /// The style color slot.
    /// </param>
    /// <param name="value">
    /// The linear RGBA value.
    /// </param>
    public static void PushStyleColor(
        ImGuiCol color,
        Vector4 value
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushStyleColor((int)(color), value);
    }

    /// <summary>
    /// Pops color overrides.
    /// </summary>
    /// <param name="count">
    /// The positive number of overrides to pop.
    /// </param>
    public static void PopStyleColor(int count = 1)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PopStyleColor(count);
    }

    /// <summary>
    /// Pushes one scalar style override.
    /// </summary>
    /// <param name="style">
    /// The style variable.
    /// </param>
    /// <param name="value">
    /// The scalar value.
    /// </param>
    public static void PushStyleVar(
        ImGuiStyleVar style,
        float value
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushStyleVar((int)(style), value);
    }

    /// <summary>
    /// Pushes one two-component style override.
    /// </summary>
    /// <param name="style">
    /// The style variable.
    /// </param>
    /// <param name="value">
    /// The vector value.
    /// </param>
    public static void PushStyleVar(
        ImGuiStyleVar style,
        Vector2 value
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushStyleVar((int)(style), value);
    }

    /// <summary>
    /// Pops style-variable overrides.
    /// </summary>
    /// <param name="count">
    /// The positive number of overrides to pop.
    /// </param>
    public static void PopStyleVar(int count = 1)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PopStyleVar(count);
    }

    /// <summary>
    /// Places the next item on the same line.
    /// </summary>
    /// <param name="offset">
    /// The horizontal offset from the line start, or zero for automatic placement.
    /// </param>
    /// <param name="spacing">
    /// The explicit spacing, or a negative value for the current style spacing.
    /// </param>
    public static void SameLine(
        float offset = 0f,
        float spacing = -1f
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SameLine(offset, spacing);
    }

    /// <summary>
    /// Draws a horizontal separator.
    /// </summary>
    public static void Separator()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.Separator();
    }

    /// <summary>
    /// Draws a labeled horizontal separator.
    /// </summary>
    /// <param name="label">
    /// The label text validated by the separator text operation.
    /// </param>
    public static void SeparatorText(string label)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SeparatorText(label);
    }

    /// <summary>
    /// Adds one standard vertical spacing unit.
    /// </summary>
    public static void Spacing()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.Spacing();
    }

    /// <summary>
    /// Advances layout by an invisible size.
    /// </summary>
    /// <param name="size">
    /// The size consumed by dummy; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void Dummy(Vector2 size)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.Dummy(size);
    }

    /// <summary>
    /// Sets the width of the next item.
    /// </summary>
    /// <param name="width">
    /// The width in logical units or pixels required by this operation.
    /// </param>
    public static void SetNextItemWidth(float width)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetNextItemWidth(width);
    }

    /// <summary>
    /// Gets the remaining content size in the current region.
    /// </summary>
    /// <returns>
    /// The remaining local width and height.
    /// </returns>
    public static Vector2 GetContentRegionAvailable()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetContentRegionAvail();
    }

    /// <summary>
    /// Gets the current local cursor position.
    /// </summary>
    /// <returns>
    /// The local cursor position.
    /// </returns>
    public static Vector2 GetCursorPosition()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetCursorPos();
    }

    /// <summary>
    /// Sets the current local cursor position.
    /// </summary>
    /// <param name="position">
    /// The position consumed by set cursor position; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void SetCursorPosition(Vector2 position)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetCursorPos(position);
    }

    /// <summary>
    /// Gets the current screen-space cursor position.
    /// </summary>
    /// <returns>
    /// The screen-space cursor position.
    /// </returns>
    public static Vector2 GetCursorScreenPosition()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetCursorScreenPos();
    }

    /// <summary>
    /// Gets the minimum screen-space corner of the previous item.
    /// </summary>
    /// <returns>
    /// The item minimum corner.
    /// </returns>
    public static Vector2 GetItemMinimum()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetItemRectMin();
    }

    /// <summary>
    /// Gets the maximum screen-space corner of the previous item.
    /// </summary>
    /// <returns>
    /// The item maximum corner.
    /// </returns>
    public static Vector2 GetItemMaximum()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetItemRectMax();
    }

    /// <summary>
    /// Measures text using the current font.
    /// </summary>
    /// <returns>
    /// The measured width and height.
    /// </returns>
    /// <param name="text">
    /// The text text validated by the measure text operation.
    /// </param>
    public static Vector2 MeasureText(string text)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.CalcTextSize(text);
    }

    /// <summary>
    /// Gets whether the previous item is hovered.
    /// </summary>
    /// <param name="flags">
    /// Hovered query behavior.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the query succeeds.
    /// </returns>
    public static bool IsItemHovered(ImGuiHoveredFlags flags = ImGuiHoveredFlags.None)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsItemHovered((int)(flags));
    }

    /// <summary>
    /// Gets whether the previous item is active.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> while the item owns an active interaction.
    /// </returns>
    public static bool IsItemActive()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsItemActive();
    }

    /// <summary>
    /// Gets whether the previous item was clicked with a mouse button.
    /// </summary>
    /// <param name="button">
    /// The mouse button to query.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the item was clicked.
    /// </returns>
    public static bool IsItemClicked(ImGuiMouseButton button = ImGuiMouseButton.Left)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsItemClicked((int)(button));
    }

    /// <summary>
    /// Gets whether a mouse button is held in the current Editor UI frame, including input consumed by UI controls.
    /// </summary>
    /// <param name="button">
    /// The UI mouse button to query.
    /// </param>
    /// <returns>
    /// True while the button is held; callers must also check their item's active or hovered state before editing.
    /// </returns>
    public static bool IsMouseButtonDown(ImGuiMouseButton button = ImGuiMouseButton.Left)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsMouseDown((int)button);
    }

    /// <summary>
    /// Gets accumulated pointer displacement from the current UI drag's starting position for timeline and canvas controls.
    /// </summary>
    /// <param name="button">
    /// The button that owns the drag.
    /// </param>
    /// <param name="threshold">
    /// Movement required to start dragging, or a negative value to use the Editor's configured threshold.
    /// </param>
    /// <returns>
    /// Displacement in UI coordinates, or zero before the drag threshold is crossed or when no drag is available.
    /// The value is cumulative; it must be applied to a captured starting value, not added again each frame.
    /// </returns>
    public static Vector2 GetMouseDragDelta(
        ImGuiMouseButton button = ImGuiMouseButton.Left,
        float threshold = -1f
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetMouseDragDelta((int)button, threshold);
    }

    /// <summary>
    /// Converts a floating-point RGBA color to Dear ImGui packed color order.
    /// </summary>
    /// <returns>
    /// The packed color value.
    /// </returns>
    /// <param name="color">
    /// The color consumed by to packed color; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static uint ToPackedColor(Vector4 color)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.ColorConvertFloat4ToU32(color);
    }

    /// <summary>
    /// Adds a line to the current window draw list.
    /// </summary>
    /// <param name="start">
    /// The screen-space start point.
    /// </param>
    /// <param name="end">
    /// The screen-space end point.
    /// </param>
    /// <param name="color">
    /// The packed ImGui color.
    /// </param>
    /// <param name="thickness">
    /// The line thickness.
    /// </param>
    public static void DrawLine(
        Vector2 start,
        Vector2 end,
        uint color,
        float thickness = 1f
    ) {
        EditorUiSession.current.EnsureDrawing();
        ImGui.DrawNativeLine(RawImGui.Raw.GetWindowDrawList(), start, end, color, thickness);
    }

    /// <summary>
    /// Adds a rectangle outline to the current window draw list.
    /// </summary>
    /// <param name="minimum">
    /// The screen-space minimum corner.
    /// </param>
    /// <param name="maximum">
    /// The screen-space maximum corner.
    /// </param>
    /// <param name="color">
    /// The packed ImGui color.
    /// </param>
    /// <param name="rounding">
    /// The corner rounding radius.
    /// </param>
    /// <param name="thickness">
    /// The outline thickness.
    /// </param>
    public static void DrawRectangle(
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding = 0f,
        float thickness = 1f
    ) {
        EditorUiSession.current.EnsureDrawing();
        ImGui.DrawNativeRectangle(RawImGui.Raw.GetWindowDrawList(), minimum, maximum, color, rounding, 0, thickness);
    }

    /// <summary>
    /// Adds a filled rectangle to the current window draw list.
    /// </summary>
    /// <param name="minimum">
    /// The screen-space minimum corner.
    /// </param>
    /// <param name="maximum">
    /// The screen-space maximum corner.
    /// </param>
    /// <param name="color">
    /// The packed ImGui color.
    /// </param>
    /// <param name="rounding">
    /// The corner rounding radius.
    /// </param>
    public static void DrawFilledRectangle(
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding = 0f
    ) {
        EditorUiSession.current.EnsureDrawing();
        ImGui.DrawNativeFilledRectangle(RawImGui.Raw.GetWindowDrawList(), minimum, maximum, color, rounding, 0);
    }

    /// <summary>
    /// Draws a cubic curve without exposing a native draw list or borrowed pointer.
    /// </summary>
    /// <param name="start">
    /// The first screen-space endpoint.
    /// </param>
    /// <param name="firstControl">
    /// The control point beside the first endpoint.
    /// </param>
    /// <param name="secondControl">
    /// The control point beside the final endpoint.
    /// </param>
    /// <param name="end">
    /// The final screen-space endpoint.
    /// </param>
    /// <param name="color">
    /// The packed presentation color.
    /// </param>
    /// <param name="thickness">
    /// The positive stroke width.
    /// </param>
    public static void DrawCubicCurve(
        Vector2 start,
        Vector2 firstControl,
        Vector2 secondControl,
        Vector2 end,
        uint color,
        float thickness = 1f
    ) {
        EditorUiSession.current.EnsureDrawing();
        ImVec2C a = ToNative(start), b = ToNative(firstControl), c = ToNative(secondControl), d = ToNative(end);
        RawImGui.Raw.AddBezierCubic(RawImGui.Raw.GetWindowDrawList(), &a, &b, &c, &d, color, thickness, 0);
    }

    /// <summary>
    /// Adds text to the current window draw list.
    /// </summary>
    /// <param name="position">
    /// The screen-space text origin.
    /// </param>
    /// <param name="color">
    /// The packed ImGui color.
    /// </param>
    /// <param name="text">
    /// The text to draw.
    /// </param>
    public static void DrawText(
        Vector2 position,
        uint color,
        string text
    ) {
        EditorUiSession.current.EnsureDrawing();
        ImGui.DrawNativeText(RawImGui.Raw.GetWindowDrawList(), position, color, text);
    }

    internal static ImVec2C ToNative(Vector2 value) => new()
    {
        X = value.X,
        Y = value.Y
    };
    internal static ImVec4C ToNative(Vector4 value) => new()
    {
        X = value.X,
        Y = value.Y,
        Z = value.Z,
        W = value.W
    };
    internal static Vector2 FromNative(ImVec2C value) => new(value.X, value.Y);
    internal static Vector4 FromNative(ImVec4C value) => new(value.X, value.Y, value.Z, value.W);
    internal static void DrawNativeLine(
        ImDrawList* drawList,
        Vector2 start,
        Vector2 end,
        uint color,
        float thickness
    ) {
        ImVec2C nativeStart = ToNative(start);
        ImVec2C nativeEnd = ToNative(end);
        RawImGui.Raw.AddLine(drawList, &nativeStart, &nativeEnd, color, thickness);
    }

    internal static void DrawNativeRectangle(
        ImDrawList* drawList,
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding,
        int flags,
        float thickness
    ) {
        ImVec2C nativeMinimum = ToNative(minimum);
        ImVec2C nativeMaximum = ToNative(maximum);
        RawImGui.Raw.AddRect(drawList, &nativeMinimum, &nativeMaximum, color, rounding, flags, thickness);
    }

    internal static void DrawNativeFilledRectangle(
        ImDrawList* drawList,
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding,
        int flags
    ) {
        ImVec2C nativeMinimum = ToNative(minimum);
        ImVec2C nativeMaximum = ToNative(maximum);
        RawImGui.Raw.AddRectFilled(drawList, &nativeMinimum, &nativeMaximum, color, rounding, flags);
    }

    internal static void ConstrainNativeWindowSize(
        Vector2 minimum,
        Vector2 maximum,
        delegate* unmanaged[Cdecl]<ImGuiSizeCallbackData*, void> callback,
        void* userData
    ) {
        ImVec2C nativeMinimum = ToNative(minimum);
        ImVec2C nativeMaximum = ToNative(maximum);
        RawImGui.Raw.SetNextWindowSizeConstraints(&nativeMinimum, &nativeMaximum, callback, userData);
    }

    internal static void PushNativeClipRectangle(
        ImDrawList* drawList,
        Vector2 minimum,
        Vector2 maximum,
        byte intersect
    ) {
        ImVec2C nativeMinimum = ToNative(minimum);
        ImVec2C nativeMaximum = ToNative(maximum);
        RawImGui.Raw.PushClipRect(drawList, &nativeMinimum, &nativeMaximum, intersect);
    }

    internal static void DrawNativePathLine(
        ImDrawList* drawList,
        Vector2 position
    ) {
        ImVec2C nativePosition = ToNative(position);
        RawImGui.Raw.PathLineTo(drawList, &nativePosition);
    }

    internal static void DrawNativePathQuadratic(
        ImDrawList* drawList,
        Vector2 control,
        Vector2 end,
        int segments
    ) {
        ImVec2C nativeControl = ToNative(control);
        ImVec2C nativeEnd = ToNative(end);
        RawImGui.Raw.PathBezierQuadraticCurveTo(drawList, &nativeControl, &nativeEnd, segments);
    }

    internal static void DrawNativeArrow(
        ImDrawList* drawList,
        Vector2 position,
        uint color,
        ImGuiDir direction,
        float scale
    ) {
        ImVec2C nativePosition = ToNative(position);
        RawImGui.Raw.RenderArrow(drawList, &nativePosition, color, direction, scale);
    }

    internal static void DrawNativeText(
        ImDrawList* drawList,
        Vector2 position,
        uint color,
        string text
    ) {
        using NativeStringLease nativeText = Utf8Codec.EncodeNullTerminated(text, allowsNull: false);
        ImVec2C nativePosition = ToNative(position);
        RawImGui.Raw.AddText(drawList, &nativePosition, color, (sbyte*)nativeText.pointer, null);
    }

    internal static void DrawNativeText(
        ImDrawList* drawList,
        ImFont* font,
        float fontSize,
        Vector2 position,
        uint color,
        string text,
        float wrapWidth = 0f
    ) {
        using NativeStringLease nativeText = Utf8Codec.EncodeNullTerminated(text, allowsNull: false);
        ImVec2C nativePosition = ToNative(position);
        RawImGui.Raw.AddText(drawList, font, fontSize, &nativePosition, color, (sbyte*)nativeText.pointer, null, wrapWidth, null);
    }

    internal static bool BeginNativeWindow(
        string label,
        ImGuiWindowFlags flags,
        bool modal
    ) {
        using NativeStringLease nativeLabel = Utf8Codec.EncodeNullTerminated(label, allowsNull: false);
        return (modal ? RawImGui.Raw.BeginPopupModal((sbyte*)nativeLabel.pointer, null, (int)flags) : RawImGui.Raw.Begin((sbyte*)nativeLabel.pointer, null, (int)flags)) != 0;
    }

    internal static uint HashNativeId(
        string text,
        uint seed = 0
    ) {
        using NativeStringLease nativeText = Utf8Codec.EncodeNullTerminated(text, allowsNull: false);
        return RawImGui.Raw.ImHashStr((sbyte*)nativeText.pointer, 0, seed);
    }

    private static float LinearToSrgb(float value) => value <= 0.0031308f ? value * 12.92f : 1.055f * MathF.Pow(value, 1f / 2.4f) - 0.055f;
    private static float SrgbToLinear(float value) => value <= 0.04045f ? value / 12.92f : MathF.Pow((value + 0.055f) / 1.055f, 2.4f);
}
