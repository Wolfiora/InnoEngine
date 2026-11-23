using System;
using System.Numerics;
using Inno.Core.Logging;
using Inno.Editor.ImGui;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;

internal sealed class ConsolePanelContent
{
    private const string C_ICON_BUG = ImGuiIcon.C_BUG;
    private const string C_ICON_INFO = ImGuiIcon.C_CIRCLE_INFO;
    private const string C_ICON_WARN = ImGuiIcon.C_TRIANGLE_EXCLAMATION;
    private const string C_ICON_ERROR = ImGuiIcon.C_CIRCLE_XMARK;
    private const string C_ICON_FATAL = ImGuiIcon.C_SKULL_CROSSBONES;
    private const string C_ICON_DEFAULT = ImGuiIcon.C_FILE_LINES;

    internal void DrawDisabledHint(string text)
    {
        Gui.BeginDisabled(true);
        Gui.Text(text);
        Gui.EndDisabled();
    }

    internal (Vector4 color, string icon) GetLevelVisual(LogLevel level)
        => level switch
    {
        LogLevel.Debug => (ConsoleStyle.debug, C_ICON_BUG),
        LogLevel.Info => (ConsoleStyle.info, C_ICON_INFO),
        LogLevel.Warn => (ConsoleStyle.warning, C_ICON_WARN),
        LogLevel.Error => (ConsoleStyle.error, C_ICON_ERROR),
        LogLevel.Fatal => (ConsoleStyle.fatal, C_ICON_FATAL),
        _ => (EditorPalette.text, C_ICON_DEFAULT)
    };

    internal Vector4 GetCollapsedBgColor() => ConsoleStyle.collapsedCard;

    internal Vector4 GetCollapsedBorderColor() => ConsoleStyle.collapsedBorder;

    internal Vector4 GetExpandedBgColor(Vector4 levelColor) => ConsoleStyle.GetExpandedCard(levelColor);

    internal string GetFirstLine(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] is '\n' or '\r')
            {
                return text[..i];
            }
        }

        return text;
    }

    internal string FitTextWithEllipsis(
        string text,
        float maxWidth
    ) {
        const string C_ELLIPSIS = "...";
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        if (maxWidth <= 1f)
        {
            return C_ELLIPSIS;
        }
        if (Gui.MeasureText(text).X <= maxWidth)
        {
            return text;
        }
        float ellipsisWidth = Gui.MeasureText(C_ELLIPSIS).X;
        if (ellipsisWidth >= maxWidth)
        {
            return C_ELLIPSIS;
        }
        float maxTextWidth = maxWidth - ellipsisWidth;
        int low = 0;
        int high = text.Length;
        while (low < high)
        {
            int middle = (low + high + 1) / 2;
            if (Gui.MeasureText(text[..middle]).X <= maxTextWidth)
            {
                low = middle;
            }
            else
            {
                high = middle - 1;
            }
        }

        return low <= 0 ? C_ELLIPSIS : text[..low] + C_ELLIPSIS;
    }
}
