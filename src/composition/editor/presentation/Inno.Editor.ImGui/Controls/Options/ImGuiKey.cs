using System;

using NativeKey = Inno.Native.ImGui.ImGuiKey;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported key choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiKey : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeKey.None,
    /// <summary>
    /// Identifies the NamedKeyBegin presentation key or modifier.
    /// </summary>
    NamedKeyBegin = (int)NativeKey.NamedKeyBegin,
    /// <summary>
    /// == ImGuiKey_NamedKey_BEGIN
    /// </summary>
    Tab = (int)NativeKey.Tab,
    /// <summary>
    /// Identifies the LeftArrow presentation key or modifier.
    /// </summary>
    LeftArrow = (int)NativeKey.LeftArrow,
    /// <summary>
    /// Identifies the RightArrow presentation key or modifier.
    /// </summary>
    RightArrow = (int)NativeKey.RightArrow,
    /// <summary>
    /// Identifies the UpArrow presentation key or modifier.
    /// </summary>
    UpArrow = (int)NativeKey.UpArrow,
    /// <summary>
    /// Identifies the DownArrow presentation key or modifier.
    /// </summary>
    DownArrow = (int)NativeKey.DownArrow,
    /// <summary>
    /// Identifies the PageUp presentation key or modifier.
    /// </summary>
    PageUp = (int)NativeKey.PageUp,
    /// <summary>
    /// Identifies the PageDown presentation key or modifier.
    /// </summary>
    PageDown = (int)NativeKey.PageDown,
    /// <summary>
    /// Identifies the Home presentation key or modifier.
    /// </summary>
    Home = (int)NativeKey.Home,
    /// <summary>
    /// Identifies the End presentation key or modifier.
    /// </summary>
    End = (int)NativeKey.End,
    /// <summary>
    /// Identifies the Insert presentation key or modifier.
    /// </summary>
    Insert = (int)NativeKey.Insert,
    /// <summary>
    /// Identifies the Delete presentation key or modifier.
    /// </summary>
    Delete = (int)NativeKey.Delete,
    /// <summary>
    /// Identifies the Backspace presentation key or modifier.
    /// </summary>
    Backspace = (int)NativeKey.Backspace,
    /// <summary>
    /// Identifies the Space presentation key or modifier.
    /// </summary>
    Space = (int)NativeKey.Space,
    /// <summary>
    /// Identifies the Enter presentation key or modifier.
    /// </summary>
    Enter = (int)NativeKey.Enter,
    /// <summary>
    /// Identifies the Escape presentation key or modifier.
    /// </summary>
    Escape = (int)NativeKey.Escape,
    /// <summary>
    /// Identifies the LeftCtrl presentation key or modifier.
    /// </summary>
    LeftCtrl = (int)NativeKey.LeftCtrl,
    /// <summary>
    /// Identifies the LeftShift presentation key or modifier.
    /// </summary>
    LeftShift = (int)NativeKey.LeftShift,
    /// <summary>
    /// Identifies the LeftAlt presentation key or modifier.
    /// </summary>
    LeftAlt = (int)NativeKey.LeftAlt,
    /// <summary>
    /// Identifies the LeftSuper presentation key or modifier.
    /// </summary>
    LeftSuper = (int)NativeKey.LeftSuper,
    /// <summary>
    /// Identifies the RightCtrl presentation key or modifier.
    /// </summary>
    RightCtrl = (int)NativeKey.RightCtrl,
    /// <summary>
    /// Identifies the RightShift presentation key or modifier.
    /// </summary>
    RightShift = (int)NativeKey.RightShift,
    /// <summary>
    /// Identifies the RightAlt presentation key or modifier.
    /// </summary>
    RightAlt = (int)NativeKey.RightAlt,
    /// <summary>
    /// Identifies the RightSuper presentation key or modifier.
    /// </summary>
    RightSuper = (int)NativeKey.RightSuper,
    /// <summary>
    /// Identifies the Menu presentation key or modifier.
    /// </summary>
    Menu = (int)NativeKey.Menu,
    /// <summary>
    /// Identifies the _0 presentation key or modifier.
    /// </summary>
    _0 = (int)NativeKey._0,
    /// <summary>
    /// Identifies the _1 presentation key or modifier.
    /// </summary>
    _1 = (int)NativeKey._1,
    /// <summary>
    /// Identifies the _2 presentation key or modifier.
    /// </summary>
    _2 = (int)NativeKey._2,
    /// <summary>
    /// Identifies the _3 presentation key or modifier.
    /// </summary>
    _3 = (int)NativeKey._3,
    /// <summary>
    /// Identifies the _4 presentation key or modifier.
    /// </summary>
    _4 = (int)NativeKey._4,
    /// <summary>
    /// Identifies the _5 presentation key or modifier.
    /// </summary>
    _5 = (int)NativeKey._5,
    /// <summary>
    /// Identifies the _6 presentation key or modifier.
    /// </summary>
    _6 = (int)NativeKey._6,
    /// <summary>
    /// Identifies the _7 presentation key or modifier.
    /// </summary>
    _7 = (int)NativeKey._7,
    /// <summary>
    /// Identifies the _8 presentation key or modifier.
    /// </summary>
    _8 = (int)NativeKey._8,
    /// <summary>
    /// Identifies the _9 presentation key or modifier.
    /// </summary>
    _9 = (int)NativeKey._9,
    /// <summary>
    /// Identifies the A presentation key or modifier.
    /// </summary>
    A = (int)NativeKey.A,
    /// <summary>
    /// Identifies the B presentation key or modifier.
    /// </summary>
    B = (int)NativeKey.B,
    /// <summary>
    /// Identifies the C presentation key or modifier.
    /// </summary>
    C = (int)NativeKey.C,
    /// <summary>
    /// Identifies the D presentation key or modifier.
    /// </summary>
    D = (int)NativeKey.D,
    /// <summary>
    /// Identifies the E presentation key or modifier.
    /// </summary>
    E = (int)NativeKey.E,
    /// <summary>
    /// Identifies the F presentation key or modifier.
    /// </summary>
    F = (int)NativeKey.F,
    /// <summary>
    /// Identifies the G presentation key or modifier.
    /// </summary>
    G = (int)NativeKey.G,
    /// <summary>
    /// Identifies the H presentation key or modifier.
    /// </summary>
    H = (int)NativeKey.H,
    /// <summary>
    /// Identifies the I presentation key or modifier.
    /// </summary>
    I = (int)NativeKey.I,
    /// <summary>
    /// Identifies the J presentation key or modifier.
    /// </summary>
    J = (int)NativeKey.J,
    /// <summary>
    /// Identifies the K presentation key or modifier.
    /// </summary>
    K = (int)NativeKey.K,
    /// <summary>
    /// Identifies the L presentation key or modifier.
    /// </summary>
    L = (int)NativeKey.L,
    /// <summary>
    /// Identifies the M presentation key or modifier.
    /// </summary>
    M = (int)NativeKey.M,
    /// <summary>
    /// Identifies the N presentation key or modifier.
    /// </summary>
    N = (int)NativeKey.N,
    /// <summary>
    /// Identifies the O presentation key or modifier.
    /// </summary>
    O = (int)NativeKey.O,
    /// <summary>
    /// Identifies the P presentation key or modifier.
    /// </summary>
    P = (int)NativeKey.P,
    /// <summary>
    /// Identifies the Q presentation key or modifier.
    /// </summary>
    Q = (int)NativeKey.Q,
    /// <summary>
    /// Identifies the R presentation key or modifier.
    /// </summary>
    R = (int)NativeKey.R,
    /// <summary>
    /// Identifies the S presentation key or modifier.
    /// </summary>
    S = (int)NativeKey.S,
    /// <summary>
    /// Identifies the T presentation key or modifier.
    /// </summary>
    T = (int)NativeKey.T,
    /// <summary>
    /// Identifies the U presentation key or modifier.
    /// </summary>
    U = (int)NativeKey.U,
    /// <summary>
    /// Identifies the V presentation key or modifier.
    /// </summary>
    V = (int)NativeKey.V,
    /// <summary>
    /// Identifies the W presentation key or modifier.
    /// </summary>
    W = (int)NativeKey.W,
    /// <summary>
    /// Identifies the X presentation key or modifier.
    /// </summary>
    X = (int)NativeKey.X,
    /// <summary>
    /// Identifies the Y presentation key or modifier.
    /// </summary>
    Y = (int)NativeKey.Y,
    /// <summary>
    /// Identifies the Z presentation key or modifier.
    /// </summary>
    Z = (int)NativeKey.Z,
    /// <summary>
    /// Identifies the F1 presentation key or modifier.
    /// </summary>
    F1 = (int)NativeKey.F1,
    /// <summary>
    /// Identifies the F2 presentation key or modifier.
    /// </summary>
    F2 = (int)NativeKey.F2,
    /// <summary>
    /// Identifies the F3 presentation key or modifier.
    /// </summary>
    F3 = (int)NativeKey.F3,
    /// <summary>
    /// Identifies the F4 presentation key or modifier.
    /// </summary>
    F4 = (int)NativeKey.F4,
    /// <summary>
    /// Identifies the F5 presentation key or modifier.
    /// </summary>
    F5 = (int)NativeKey.F5,
    /// <summary>
    /// Identifies the F6 presentation key or modifier.
    /// </summary>
    F6 = (int)NativeKey.F6,
    /// <summary>
    /// Identifies the F7 presentation key or modifier.
    /// </summary>
    F7 = (int)NativeKey.F7,
    /// <summary>
    /// Identifies the F8 presentation key or modifier.
    /// </summary>
    F8 = (int)NativeKey.F8,
    /// <summary>
    /// Identifies the F9 presentation key or modifier.
    /// </summary>
    F9 = (int)NativeKey.F9,
    /// <summary>
    /// Identifies the F10 presentation key or modifier.
    /// </summary>
    F10 = (int)NativeKey.F10,
    /// <summary>
    /// Identifies the F11 presentation key or modifier.
    /// </summary>
    F11 = (int)NativeKey.F11,
    /// <summary>
    /// Identifies the F12 presentation key or modifier.
    /// </summary>
    F12 = (int)NativeKey.F12,
    /// <summary>
    /// Identifies the F13 presentation key or modifier.
    /// </summary>
    F13 = (int)NativeKey.F13,
    /// <summary>
    /// Identifies the F14 presentation key or modifier.
    /// </summary>
    F14 = (int)NativeKey.F14,
    /// <summary>
    /// Identifies the F15 presentation key or modifier.
    /// </summary>
    F15 = (int)NativeKey.F15,
    /// <summary>
    /// Identifies the F16 presentation key or modifier.
    /// </summary>
    F16 = (int)NativeKey.F16,
    /// <summary>
    /// Identifies the F17 presentation key or modifier.
    /// </summary>
    F17 = (int)NativeKey.F17,
    /// <summary>
    /// Identifies the F18 presentation key or modifier.
    /// </summary>
    F18 = (int)NativeKey.F18,
    /// <summary>
    /// Identifies the F19 presentation key or modifier.
    /// </summary>
    F19 = (int)NativeKey.F19,
    /// <summary>
    /// Identifies the F20 presentation key or modifier.
    /// </summary>
    F20 = (int)NativeKey.F20,
    /// <summary>
    /// Identifies the F21 presentation key or modifier.
    /// </summary>
    F21 = (int)NativeKey.F21,
    /// <summary>
    /// Identifies the F22 presentation key or modifier.
    /// </summary>
    F22 = (int)NativeKey.F22,
    /// <summary>
    /// Identifies the F23 presentation key or modifier.
    /// </summary>
    F23 = (int)NativeKey.F23,
    /// <summary>
    /// Identifies the F24 presentation key or modifier.
    /// </summary>
    F24 = (int)NativeKey.F24,
    /// <summary>
    /// Identifies the Apostrophe presentation key or modifier.
    /// </summary>
    Apostrophe = (int)NativeKey.Apostrophe,
    /// <summary>
    /// Identifies the Comma presentation key or modifier.
    /// </summary>
    Comma = (int)NativeKey.Comma,
    /// <summary>
    /// Identifies the Minus presentation key or modifier.
    /// </summary>
    Minus = (int)NativeKey.Minus,
    /// <summary>
    /// Identifies the Period presentation key or modifier.
    /// </summary>
    Period = (int)NativeKey.Period,
    /// <summary>
    /// Identifies the Slash presentation key or modifier.
    /// </summary>
    Slash = (int)NativeKey.Slash,
    /// <summary>
    /// Identifies the Semicolon presentation key or modifier.
    /// </summary>
    Semicolon = (int)NativeKey.Semicolon,
    /// <summary>
    /// Identifies the Equal presentation key or modifier.
    /// </summary>
    Equal = (int)NativeKey.Equal,
    /// <summary>
    /// Identifies the LeftBracket presentation key or modifier.
    /// </summary>
    LeftBracket = (int)NativeKey.LeftBracket,
    /// <summary>
    /// Identifies the Backslash presentation key or modifier.
    /// </summary>
    Backslash = (int)NativeKey.Backslash,
    /// <summary>
    /// Identifies the RightBracket presentation key or modifier.
    /// </summary>
    RightBracket = (int)NativeKey.RightBracket,
    /// <summary>
    /// Identifies the GraveAccent presentation key or modifier.
    /// </summary>
    GraveAccent = (int)NativeKey.GraveAccent,
    /// <summary>
    /// Identifies the CapsLock presentation key or modifier.
    /// </summary>
    CapsLock = (int)NativeKey.CapsLock,
    /// <summary>
    /// Identifies the ScrollLock presentation key or modifier.
    /// </summary>
    ScrollLock = (int)NativeKey.ScrollLock,
    /// <summary>
    /// Identifies the NumLock presentation key or modifier.
    /// </summary>
    NumLock = (int)NativeKey.NumLock,
    /// <summary>
    /// Identifies the PrintScreen presentation key or modifier.
    /// </summary>
    PrintScreen = (int)NativeKey.PrintScreen,
    /// <summary>
    /// Identifies the Pause presentation key or modifier.
    /// </summary>
    Pause = (int)NativeKey.Pause,
    /// <summary>
    /// Identifies the Keypad0 presentation key or modifier.
    /// </summary>
    Keypad0 = (int)NativeKey.Keypad0,
    /// <summary>
    /// Identifies the Keypad1 presentation key or modifier.
    /// </summary>
    Keypad1 = (int)NativeKey.Keypad1,
    /// <summary>
    /// Identifies the Keypad2 presentation key or modifier.
    /// </summary>
    Keypad2 = (int)NativeKey.Keypad2,
    /// <summary>
    /// Identifies the Keypad3 presentation key or modifier.
    /// </summary>
    Keypad3 = (int)NativeKey.Keypad3,
    /// <summary>
    /// Identifies the Keypad4 presentation key or modifier.
    /// </summary>
    Keypad4 = (int)NativeKey.Keypad4,
    /// <summary>
    /// Identifies the Keypad5 presentation key or modifier.
    /// </summary>
    Keypad5 = (int)NativeKey.Keypad5,
    /// <summary>
    /// Identifies the Keypad6 presentation key or modifier.
    /// </summary>
    Keypad6 = (int)NativeKey.Keypad6,
    /// <summary>
    /// Identifies the Keypad7 presentation key or modifier.
    /// </summary>
    Keypad7 = (int)NativeKey.Keypad7,
    /// <summary>
    /// Identifies the Keypad8 presentation key or modifier.
    /// </summary>
    Keypad8 = (int)NativeKey.Keypad8,
    /// <summary>
    /// Identifies the Keypad9 presentation key or modifier.
    /// </summary>
    Keypad9 = (int)NativeKey.Keypad9,
    /// <summary>
    /// Identifies the KeypadDecimal presentation key or modifier.
    /// </summary>
    KeypadDecimal = (int)NativeKey.KeypadDecimal,
    /// <summary>
    /// Identifies the KeypadDivide presentation key or modifier.
    /// </summary>
    KeypadDivide = (int)NativeKey.KeypadDivide,
    /// <summary>
    /// Identifies the KeypadMultiply presentation key or modifier.
    /// </summary>
    KeypadMultiply = (int)NativeKey.KeypadMultiply,
    /// <summary>
    /// Identifies the KeypadSubtract presentation key or modifier.
    /// </summary>
    KeypadSubtract = (int)NativeKey.KeypadSubtract,
    /// <summary>
    /// Identifies the KeypadAdd presentation key or modifier.
    /// </summary>
    KeypadAdd = (int)NativeKey.KeypadAdd,
    /// <summary>
    /// Identifies the KeypadEnter presentation key or modifier.
    /// </summary>
    KeypadEnter = (int)NativeKey.KeypadEnter,
    /// <summary>
    /// Identifies the KeypadEqual presentation key or modifier.
    /// </summary>
    KeypadEqual = (int)NativeKey.KeypadEqual,
    /// <summary>
    /// Identifies the AppBack presentation key or modifier.
    /// </summary>
    AppBack = (int)NativeKey.AppBack,
    /// <summary>
    /// Identifies the AppForward presentation key or modifier.
    /// </summary>
    AppForward = (int)NativeKey.AppForward,
    /// <summary>
    /// Identifies the Oem102 presentation key or modifier.
    /// </summary>
    Oem102 = (int)NativeKey.Oem102,
    /// <summary>
    /// Identifies the GamepadStart presentation key or modifier.
    /// </summary>
    GamepadStart = (int)NativeKey.GamepadStart,
    /// <summary>
    /// Identifies the GamepadBack presentation key or modifier.
    /// </summary>
    GamepadBack = (int)NativeKey.GamepadBack,
    /// <summary>
    /// Identifies the GamepadFaceLeft presentation key or modifier.
    /// </summary>
    GamepadFaceLeft = (int)NativeKey.GamepadFaceLeft,
    /// <summary>
    /// Identifies the GamepadFaceRight presentation key or modifier.
    /// </summary>
    GamepadFaceRight = (int)NativeKey.GamepadFaceRight,
    /// <summary>
    /// Identifies the GamepadFaceUp presentation key or modifier.
    /// </summary>
    GamepadFaceUp = (int)NativeKey.GamepadFaceUp,
    /// <summary>
    /// Identifies the GamepadFaceDown presentation key or modifier.
    /// </summary>
    GamepadFaceDown = (int)NativeKey.GamepadFaceDown,
    /// <summary>
    /// Identifies the GamepadDpadLeft presentation key or modifier.
    /// </summary>
    GamepadDpadLeft = (int)NativeKey.GamepadDpadLeft,
    /// <summary>
    /// Identifies the GamepadDpadRight presentation key or modifier.
    /// </summary>
    GamepadDpadRight = (int)NativeKey.GamepadDpadRight,
    /// <summary>
    /// Identifies the GamepadDpadUp presentation key or modifier.
    /// </summary>
    GamepadDpadUp = (int)NativeKey.GamepadDpadUp,
    /// <summary>
    /// Identifies the GamepadDpadDown presentation key or modifier.
    /// </summary>
    GamepadDpadDown = (int)NativeKey.GamepadDpadDown,
    /// <summary>
    /// Identifies the GamepadL1 presentation key or modifier.
    /// </summary>
    GamepadL1 = (int)NativeKey.GamepadL1,
    /// <summary>
    /// Identifies the GamepadR1 presentation key or modifier.
    /// </summary>
    GamepadR1 = (int)NativeKey.GamepadR1,
    /// <summary>
    /// Identifies the GamepadL2 presentation key or modifier.
    /// </summary>
    GamepadL2 = (int)NativeKey.GamepadL2,
    /// <summary>
    /// Identifies the GamepadR2 presentation key or modifier.
    /// </summary>
    GamepadR2 = (int)NativeKey.GamepadR2,
    /// <summary>
    /// Identifies the GamepadL3 presentation key or modifier.
    /// </summary>
    GamepadL3 = (int)NativeKey.GamepadL3,
    /// <summary>
    /// Identifies the GamepadR3 presentation key or modifier.
    /// </summary>
    GamepadR3 = (int)NativeKey.GamepadR3,
    /// <summary>
    /// Identifies the GamepadLStickLeft presentation key or modifier.
    /// </summary>
    GamepadLStickLeft = (int)NativeKey.GamepadLStickLeft,
    /// <summary>
    /// Identifies the GamepadLStickRight presentation key or modifier.
    /// </summary>
    GamepadLStickRight = (int)NativeKey.GamepadLStickRight,
    /// <summary>
    /// Identifies the GamepadLStickUp presentation key or modifier.
    /// </summary>
    GamepadLStickUp = (int)NativeKey.GamepadLStickUp,
    /// <summary>
    /// Identifies the GamepadLStickDown presentation key or modifier.
    /// </summary>
    GamepadLStickDown = (int)NativeKey.GamepadLStickDown,
    /// <summary>
    /// Identifies the GamepadRStickLeft presentation key or modifier.
    /// </summary>
    GamepadRStickLeft = (int)NativeKey.GamepadRStickLeft,
    /// <summary>
    /// Identifies the GamepadRStickRight presentation key or modifier.
    /// </summary>
    GamepadRStickRight = (int)NativeKey.GamepadRStickRight,
    /// <summary>
    /// Identifies the GamepadRStickUp presentation key or modifier.
    /// </summary>
    GamepadRStickUp = (int)NativeKey.GamepadRStickUp,
    /// <summary>
    /// Identifies the GamepadRStickDown presentation key or modifier.
    /// </summary>
    GamepadRStickDown = (int)NativeKey.GamepadRStickDown,
    /// <summary>
    /// Identifies the MouseLeft presentation key or modifier.
    /// </summary>
    MouseLeft = (int)NativeKey.MouseLeft,
    /// <summary>
    /// Identifies the MouseRight presentation key or modifier.
    /// </summary>
    MouseRight = (int)NativeKey.MouseRight,
    /// <summary>
    /// Identifies the MouseMiddle presentation key or modifier.
    /// </summary>
    MouseMiddle = (int)NativeKey.MouseMiddle,
    /// <summary>
    /// Identifies the MouseX1 presentation key or modifier.
    /// </summary>
    MouseX1 = (int)NativeKey.MouseX1,
    /// <summary>
    /// Identifies the MouseX2 presentation key or modifier.
    /// </summary>
    MouseX2 = (int)NativeKey.MouseX2,
    /// <summary>
    /// Identifies the MouseWheelX presentation key or modifier.
    /// </summary>
    MouseWheelX = (int)NativeKey.MouseWheelX,
    /// <summary>
    /// Identifies the MouseWheelY presentation key or modifier.
    /// </summary>
    MouseWheelY = (int)NativeKey.MouseWheelY,
    /// <summary>
    /// Identifies the ReservedForModCtrl presentation key or modifier.
    /// </summary>
    ReservedForModCtrl = (int)NativeKey.ReservedForModCtrl,
    /// <summary>
    /// Identifies the ReservedForModShift presentation key or modifier.
    /// </summary>
    ReservedForModShift = (int)NativeKey.ReservedForModShift,
    /// <summary>
    /// Identifies the ReservedForModAlt presentation key or modifier.
    /// </summary>
    ReservedForModAlt = (int)NativeKey.ReservedForModAlt,
    /// <summary>
    /// Identifies the ReservedForModSuper presentation key or modifier.
    /// </summary>
    ReservedForModSuper = (int)NativeKey.ReservedForModSuper,
    /// <summary>
    /// Identifies the NamedKeyEnd presentation key or modifier.
    /// </summary>
    NamedKeyEnd = (int)NativeKey.NamedKeyEnd,
    /// <summary>
    /// Identifies the NamedKeyCount presentation key or modifier.
    /// </summary>
    NamedKeyCount = (int)NativeKey.NamedKeyCount,
    /// <summary>
    /// Identifies the ImGuiModNone presentation key or modifier.
    /// </summary>
    ImGuiModNone = (int)NativeKey.ImGuiModNone,
    /// <summary>
    /// Identifies the ImGuiModCtrl presentation key or modifier.
    /// </summary>
    ImGuiModCtrl = (int)NativeKey.ImGuiModCtrl,
    /// <summary>
    /// Identifies the ImGuiModShift presentation key or modifier.
    /// </summary>
    ImGuiModShift = (int)NativeKey.ImGuiModShift,
    /// <summary>
    /// Identifies the ImGuiModAlt presentation key or modifier.
    /// </summary>
    ImGuiModAlt = (int)NativeKey.ImGuiModAlt,
    /// <summary>
    /// Identifies the ImGuiModSuper presentation key or modifier.
    /// </summary>
    ImGuiModSuper = (int)NativeKey.ImGuiModSuper,
    /// <summary>
    /// Identifies the ImGuiModMask presentation key or modifier.
    /// </summary>
    ImGuiModMask = (int)NativeKey.ImGuiModMask
}
