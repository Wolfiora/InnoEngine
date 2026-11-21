using System;

using NativeInputTextFlags = Inno.Native.ImGui.ImGuiInputTextFlags;

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
    None = (int)NativeInputTextFlags.None,
    /// <summary>
    /// Allow 0123456789.+-*/
    /// </summary>
    CharsDecimal = (int)NativeInputTextFlags.CharsDecimal,
    /// <summary>
    /// Allow 0123456789ABCDEFabcdef
    /// </summary>
    CharsHexadecimal = (int)NativeInputTextFlags.CharsHexadecimal,
    /// <summary>
    /// Allow 0123456789.+-*/eE (Scientific notation input)
    /// </summary>
    CharsScientific = (int)NativeInputTextFlags.CharsScientific,
    /// <summary>
    /// Turn a..z into A..Z
    /// </summary>
    CharsUppercase = (int)NativeInputTextFlags.CharsUppercase,
    /// <summary>
    /// Filter out spaces, tabs
    /// </summary>
    CharsNoBlank = (int)NativeInputTextFlags.CharsNoBlank,
    /// <summary>
    /// Pressing TAB input a '\t' character into the text field
    /// </summary>
    AllowTabInput = (int)NativeInputTextFlags.AllowTabInput,
    /// <summary>
    /// Return 'true' when Enter is pressed (as opposed to every time the value was modified). Consider using IsItemDeactivatedAfterEdit() instead!
    /// </summary>
    EnterReturnsTrue = (int)NativeInputTextFlags.EnterReturnsTrue,
    /// <summary>
    /// Escape key clears content if not empty, and deactivate otherwise (contrast to default behavior of Escape to revert)
    /// </summary>
    EscapeClearsAll = (int)NativeInputTextFlags.EscapeClearsAll,
    /// <summary>
    /// In multi-line mode, validate with Enter, add new line with Ctrl+Enter (default is opposite: validate with Ctrl+Enter, add line with Enter).
    /// </summary>
    CtrlEnterForNewLine = (int)NativeInputTextFlags.CtrlEnterForNewLine,
    /// <summary>
    /// Read-only mode
    /// </summary>
    ReadOnly = (int)NativeInputTextFlags.ReadOnly,
    /// <summary>
    /// Password mode, display all characters as '*', disable copy
    /// </summary>
    Password = (int)NativeInputTextFlags.Password,
    /// <summary>
    /// Overwrite mode
    /// </summary>
    AlwaysOverwrite = (int)NativeInputTextFlags.AlwaysOverwrite,
    /// <summary>
    /// Select entire text when first taking mouse focus
    /// </summary>
    AutoSelectAll = (int)NativeInputTextFlags.AutoSelectAll,
    /// <summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: parse empty string as zero value.
    /// </summary>
    ParseEmptyRefVal = (int)NativeInputTextFlags.ParseEmptyRefVal,
    /// <summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: when value is zero, do not display it. Generally used with ImGuiInputTextFlags_ParseEmptyRefVal.
    /// </summary>
    DisplayEmptyRefVal = (int)NativeInputTextFlags.DisplayEmptyRefVal,
    /// <summary>
    /// Disable following the cursor horizontally
    /// </summary>
    NoHorizontalScroll = (int)NativeInputTextFlags.NoHorizontalScroll,
    /// <summary>
    /// Disable undo/redo. Note that input text owns the text data while active, if you want to provide your own undo/redo stack you need e.g. to call ClearActiveID().
    /// </summary>
    NoUndoRedo = (int)NativeInputTextFlags.NoUndoRedo,
    /// <summary>
    /// When text doesn't fit, elide left side to ensure right side stays visible. Useful for path/filenames. Single-line only!
    /// </summary>
    ElideLeft = (int)NativeInputTextFlags.ElideLeft,
    /// <summary>
    /// Callback on pressing TAB (for completion handling)
    /// </summary>
    CallbackCompletion = (int)NativeInputTextFlags.CallbackCompletion,
    /// <summary>
    /// Callback on pressing Up/Down arrows (for history handling)
    /// </summary>
    CallbackHistory = (int)NativeInputTextFlags.CallbackHistory,
    /// <summary>
    /// Callback on each iteration. User code may query cursor position, modify text buffer.
    /// </summary>
    CallbackAlways = (int)NativeInputTextFlags.CallbackAlways,
    /// <summary>
    /// Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
    /// </summary>
    CallbackCharFilter = (int)NativeInputTextFlags.CallbackCharFilter,
    /// <summary>
    /// Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow. Notify when the string wants to be resized (for string types which hold a cache of their Size). You will be provided a new BufSize in the callback and NEED to honor it. (see misc/cpp/imgui_stdlib.h for an example of using this)
    /// </summary>
    CallbackResize = (int)NativeInputTextFlags.CallbackResize,
    /// <summary>
    /// Callback on any edit. Note that InputText() already returns true on edit + you can always use IsItemEdited(). The callback is useful to manipulate the underlying buffer while focus is active.
    /// </summary>
    CallbackEdit = (int)NativeInputTextFlags.CallbackEdit,
    /// <summary>
    /// InputTextMultiline(): word-wrap lines that are too long.
    /// </summary>
    WordWrap = (int)NativeInputTextFlags.WordWrap
}
