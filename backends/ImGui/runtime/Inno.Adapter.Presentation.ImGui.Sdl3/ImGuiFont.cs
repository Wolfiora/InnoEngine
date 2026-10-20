using System;
using System.Collections.Generic;

using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Provides scoped access to font faces registered for the current ImGui context.
/// </summary>
public static unsafe class ImGuiFont
{
    private static readonly object s_sync = new();
    private static readonly Dictionary<nuint, Dictionary<ImGuiFontStyle, nint>> s_fontsByContext = [];

    /// <summary>
    /// Gets whether an exact font style is available in the current ImGui context.
    /// </summary>
    /// <param name="style">
    /// The composable style to query.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the exact face is registered.
    /// </returns>
    public static bool IsAvailable(ImGuiFontStyle style)
    {
        ValidateStyle(style);
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        if (context == null)
            return false;

        lock (s_sync)
        {
            return s_fontsByContext.TryGetValue(GetContextKey(context), out Dictionary<ImGuiFontStyle, nint>? fonts) &&
                   fonts.ContainsKey(style);
        }
    }

    /// <summary>
    /// Pushes a font style for the current ImGui context and returns a scope that restores it.
    /// </summary>
    /// <param name="style">
    /// The composable style to use.
    /// </param>
    /// <returns>
    /// A disposable scope. Missing faces gracefully fall back to the nearest registered face.
    /// </returns>
    public static ImGuiFontScope PushStyle(ImGuiFontStyle style)
    {
        ValidateStyle(style);
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        if (context == null)
            return default;

        nuint contextKey = GetContextKey(context);
        nint font;
        lock (s_sync)
        {
            if (!s_fontsByContext.TryGetValue(contextKey, out Dictionary<ImGuiFontStyle, nint>? fonts) ||
                !TryResolve(fonts, style, out font))
            {
                return default;
            }
        }

        NativeImGui.Raw.PushFont((ImFont*)font, 0f);
        return new ImGuiFontScope(contextKey, true);
    }

    internal static void RegisterContext(
        ImGuiNativeContext* context,
        IReadOnlyDictionary<ImGuiFontStyle, nint> fonts
    ) {
        if (context == null)
            throw new ArgumentException("An ImGui context is required.", nameof(context));
        ArgumentNullException.ThrowIfNull(fonts);

        var registered = new Dictionary<ImGuiFontStyle, nint>();
        foreach ((ImGuiFontStyle style, nint font) in fonts)
        {
            ValidateStyle(style);
            if (font != 0)
                registered[style] = font;
        }

        lock (s_sync)
            s_fontsByContext[GetContextKey(context)] = registered;
    }

    internal static void RegisterStyle(
        ImGuiNativeContext* context,
        ImGuiFontStyle style,
        nint font
    ) {
        if (context == null)
            throw new ArgumentException("An ImGui context is required.", nameof(context));
        ValidateStyle(style);
        if (font == 0)
            throw new ArgumentException("A loaded font is required.", nameof(font));

        lock (s_sync)
        {
            nuint contextKey = GetContextKey(context);
            if (!s_fontsByContext.TryGetValue(contextKey, out Dictionary<ImGuiFontStyle, nint>? fonts))
            {
                fonts = [];
                s_fontsByContext.Add(contextKey, fonts);
            }
            fonts[style] = font;
        }
    }

    internal static void UnregisterContext(ImGuiNativeContext* context)
    {
        if (context == null)
            return;
        lock (s_sync)
            s_fontsByContext.Remove(GetContextKey(context));
    }

    internal static void ValidateStyle(ImGuiFontStyle style)
    {
        const ImGuiFontStyle supported = ImGuiFontStyle.Bold | ImGuiFontStyle.Italic;
        if ((style & ~supported) != 0)
            throw new ArgumentOutOfRangeException(nameof(style), style, "The font style contains unsupported flags.");
    }

    internal static void PopStyle(nuint contextKey)
    {
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        if (context == null || GetContextKey(context) != contextKey)
            throw new InvalidOperationException("The font style scope must be disposed in the context where it was created.");
        NativeImGui.PopFont();
    }

    private static unsafe nuint GetContextKey(ImGuiNativeContext* context) => (nuint)context;

    private static bool TryResolve(
        IReadOnlyDictionary<ImGuiFontStyle, nint> fonts,
        ImGuiFontStyle style,
        out nint font
    ) {
        if (fonts.TryGetValue(style, out font))
            return true;
        if ((style & ImGuiFontStyle.Bold) != 0 && fonts.TryGetValue(ImGuiFontStyle.Bold, out font))
            return true;
        if ((style & ImGuiFontStyle.Italic) != 0 && fonts.TryGetValue(ImGuiFontStyle.Italic, out font))
            return true;
        return fonts.TryGetValue(ImGuiFontStyle.Regular, out font);
    }
}

/// <summary>
/// Restores the previous ImGui font when a scoped style ends.
/// </summary>
public ref struct ImGuiFontScope
{
    private readonly nuint m_contextKey;
    private bool m_isPushed;

    internal ImGuiFontScope(
        nuint contextKey,
        bool isPushed
    ) {
        m_contextKey = contextKey;
        m_isPushed = isPushed;
    }

    /// <summary>
    /// Restores the font that was active before this scope.
    /// </summary>
    public void Dispose()
    {
        if (!m_isPushed)
            return;
        ImGuiFont.PopStyle(m_contextKey);
        m_isPushed = false;
    }
}
