using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported input text flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiInputTextFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiInputTextFlags.None,
    /// <summary>
    /// Allow 0123456789.+-*/
    /// </summary>
    CharsDecimal = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CharsDecimal,
    /// <summary>
    /// Allow 0123456789ABCDEFabcdef
    /// </summary>
    CharsHexadecimal = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CharsHexadecimal,
    /// <summary>
    /// Allow 0123456789.+-*/eE (Scientific notation input)
    /// </summary>
    CharsScientific = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CharsScientific,
    /// <summary>
    /// Turn a..z into A..Z
    /// </summary>
    CharsUppercase = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CharsUppercase,
    /// <summary>
    /// Filter out spaces, tabs
    /// </summary>
    CharsNoBlank = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CharsNoBlank,
    /// <summary>
    /// Pressing TAB input a '\t' character into the text field
    /// </summary>
    AllowTabInput = (int)Inno.Native.ImGui.ImGuiInputTextFlags.AllowTabInput,
    /// <summary>
    /// Return 'true' when Enter is pressed (as opposed to every time the value was modified). Consider using IsItemDeactivatedAfterEdit() instead!
    /// </summary>
    EnterReturnsTrue = (int)Inno.Native.ImGui.ImGuiInputTextFlags.EnterReturnsTrue,
    /// <summary>
    /// Escape key clears content if not empty, and deactivate otherwise (contrast to default behavior of Escape to revert)
    /// </summary>
    EscapeClearsAll = (int)Inno.Native.ImGui.ImGuiInputTextFlags.EscapeClearsAll,
    /// <summary>
    /// In multi-line mode, validate with Enter, add new line with Ctrl+Enter (default is opposite: validate with Ctrl+Enter, add line with Enter).
    /// </summary>
    CtrlEnterForNewLine = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CtrlEnterForNewLine,
    /// <summary>
    /// Read-only mode
    /// </summary>
    ReadOnly = (int)Inno.Native.ImGui.ImGuiInputTextFlags.ReadOnly,
    /// <summary>
    /// Password mode, display all characters as '*', disable copy
    /// </summary>
    Password = (int)Inno.Native.ImGui.ImGuiInputTextFlags.Password,
    /// <summary>
    /// Overwrite mode
    /// </summary>
    AlwaysOverwrite = (int)Inno.Native.ImGui.ImGuiInputTextFlags.AlwaysOverwrite,
    /// <summary>
    /// Select entire text when first taking mouse focus
    /// </summary>
    AutoSelectAll = (int)Inno.Native.ImGui.ImGuiInputTextFlags.AutoSelectAll,
    /// <summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: parse empty string as zero value.
    /// </summary>
    ParseEmptyRefVal = (int)Inno.Native.ImGui.ImGuiInputTextFlags.ParseEmptyRefVal,
    /// <summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: when value is zero, do not display it. Generally used with ImGuiInputTextFlags_ParseEmptyRefVal.
    /// </summary>
    DisplayEmptyRefVal = (int)Inno.Native.ImGui.ImGuiInputTextFlags.DisplayEmptyRefVal,
    /// <summary>
    /// Disable following the cursor horizontally
    /// </summary>
    NoHorizontalScroll = (int)Inno.Native.ImGui.ImGuiInputTextFlags.NoHorizontalScroll,
    /// <summary>
    /// Disable undo/redo. Note that input text owns the text data while active, if you want to provide your own undo/redo stack you need e.g. to call ClearActiveID().
    /// </summary>
    NoUndoRedo = (int)Inno.Native.ImGui.ImGuiInputTextFlags.NoUndoRedo,
    /// <summary>
    /// When text doesn't fit, elide left side to ensure right side stays visible. Useful for path/filenames. Single-line only!
    /// </summary>
    ElideLeft = (int)Inno.Native.ImGui.ImGuiInputTextFlags.ElideLeft,
    /// <summary>
    /// Callback on pressing TAB (for completion handling)
    /// </summary>
    CallbackCompletion = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCompletion,
    /// <summary>
    /// Callback on pressing Up/Down arrows (for history handling)
    /// </summary>
    CallbackHistory = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackHistory,
    /// <summary>
    /// Callback on each iteration. User code may query cursor position, modify text buffer.
    /// </summary>
    CallbackAlways = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackAlways,
    /// <summary>
    /// Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
    /// </summary>
    CallbackCharFilter = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCharFilter,
    /// <summary>
    /// Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow. Notify when the string wants to be resized (for string types which hold a cache of their Size). You will be provided a new BufSize in the callback and NEED to honor it. (see misc/cpp/imgui_stdlib.h for an example of using this)
    /// </summary>
    CallbackResize = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackResize,
    /// <summary>
    /// Callback on any edit. Note that InputText() already returns true on edit + you can always use IsItemEdited(). The callback is useful to manipulate the underlying buffer while focus is active.
    /// </summary>
    CallbackEdit = (int)Inno.Native.ImGui.ImGuiInputTextFlags.CallbackEdit,
    /// <summary>
    /// InputTextMultiline(): word-wrap lines that are too long.
    /// </summary>
    WordWrap = (int)Inno.Native.ImGui.ImGuiInputTextFlags.WordWrap,
}
