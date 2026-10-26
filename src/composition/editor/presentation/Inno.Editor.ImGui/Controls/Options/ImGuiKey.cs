using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported key choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiKey : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiKey.None,
    /// <summary>
    /// Identifies the NamedKeyBegin presentation key or modifier.
    /// </summary>
    NamedKeyBegin = (int)Inno.Native.ImGui.ImGuiKey.NamedKeyBegin,
    /// <summary>
    /// == ImGuiKey_NamedKey_BEGIN
    /// </summary>
    Tab = (int)Inno.Native.ImGui.ImGuiKey.Tab,
    /// <summary>
    /// Identifies the LeftArrow presentation key or modifier.
    /// </summary>
    LeftArrow = (int)Inno.Native.ImGui.ImGuiKey.LeftArrow,
    /// <summary>
    /// Identifies the RightArrow presentation key or modifier.
    /// </summary>
    RightArrow = (int)Inno.Native.ImGui.ImGuiKey.RightArrow,
    /// <summary>
    /// Identifies the UpArrow presentation key or modifier.
    /// </summary>
    UpArrow = (int)Inno.Native.ImGui.ImGuiKey.UpArrow,
    /// <summary>
    /// Identifies the DownArrow presentation key or modifier.
    /// </summary>
    DownArrow = (int)Inno.Native.ImGui.ImGuiKey.DownArrow,
    /// <summary>
    /// Identifies the PageUp presentation key or modifier.
    /// </summary>
    PageUp = (int)Inno.Native.ImGui.ImGuiKey.PageUp,
    /// <summary>
    /// Identifies the PageDown presentation key or modifier.
    /// </summary>
    PageDown = (int)Inno.Native.ImGui.ImGuiKey.PageDown,
    /// <summary>
    /// Identifies the Home presentation key or modifier.
    /// </summary>
    Home = (int)Inno.Native.ImGui.ImGuiKey.Home,
    /// <summary>
    /// Identifies the End presentation key or modifier.
    /// </summary>
    End = (int)Inno.Native.ImGui.ImGuiKey.End,
    /// <summary>
    /// Identifies the Insert presentation key or modifier.
    /// </summary>
    Insert = (int)Inno.Native.ImGui.ImGuiKey.Insert,
    /// <summary>
    /// Identifies the Delete presentation key or modifier.
    /// </summary>
    Delete = (int)Inno.Native.ImGui.ImGuiKey.Delete,
    /// <summary>
    /// Identifies the Backspace presentation key or modifier.
    /// </summary>
    Backspace = (int)Inno.Native.ImGui.ImGuiKey.Backspace,
    /// <summary>
    /// Identifies the Space presentation key or modifier.
    /// </summary>
    Space = (int)Inno.Native.ImGui.ImGuiKey.Space,
    /// <summary>
    /// Identifies the Enter presentation key or modifier.
    /// </summary>
    Enter = (int)Inno.Native.ImGui.ImGuiKey.Enter,
    /// <summary>
    /// Identifies the Escape presentation key or modifier.
    /// </summary>
    Escape = (int)Inno.Native.ImGui.ImGuiKey.Escape,
    /// <summary>
    /// Identifies the LeftCtrl presentation key or modifier.
    /// </summary>
    LeftCtrl = (int)Inno.Native.ImGui.ImGuiKey.LeftCtrl,
    /// <summary>
    /// Identifies the LeftShift presentation key or modifier.
    /// </summary>
    LeftShift = (int)Inno.Native.ImGui.ImGuiKey.LeftShift,
    /// <summary>
    /// Identifies the LeftAlt presentation key or modifier.
    /// </summary>
    LeftAlt = (int)Inno.Native.ImGui.ImGuiKey.LeftAlt,
    /// <summary>
    /// Identifies the LeftSuper presentation key or modifier.
    /// </summary>
    LeftSuper = (int)Inno.Native.ImGui.ImGuiKey.LeftSuper,
    /// <summary>
    /// Identifies the RightCtrl presentation key or modifier.
    /// </summary>
    RightCtrl = (int)Inno.Native.ImGui.ImGuiKey.RightCtrl,
    /// <summary>
    /// Identifies the RightShift presentation key or modifier.
    /// </summary>
    RightShift = (int)Inno.Native.ImGui.ImGuiKey.RightShift,
    /// <summary>
    /// Identifies the RightAlt presentation key or modifier.
    /// </summary>
    RightAlt = (int)Inno.Native.ImGui.ImGuiKey.RightAlt,
    /// <summary>
    /// Identifies the RightSuper presentation key or modifier.
    /// </summary>
    RightSuper = (int)Inno.Native.ImGui.ImGuiKey.RightSuper,
    /// <summary>
    /// Identifies the Menu presentation key or modifier.
    /// </summary>
    Menu = (int)Inno.Native.ImGui.ImGuiKey.Menu,
    /// <summary>
    /// Identifies the _0 presentation key or modifier.
    /// </summary>
    _0 = (int)Inno.Native.ImGui.ImGuiKey._0,
    /// <summary>
    /// Identifies the _1 presentation key or modifier.
    /// </summary>
    _1 = (int)Inno.Native.ImGui.ImGuiKey._1,
    /// <summary>
    /// Identifies the _2 presentation key or modifier.
    /// </summary>
    _2 = (int)Inno.Native.ImGui.ImGuiKey._2,
    /// <summary>
    /// Identifies the _3 presentation key or modifier.
    /// </summary>
    _3 = (int)Inno.Native.ImGui.ImGuiKey._3,
    /// <summary>
    /// Identifies the _4 presentation key or modifier.
    /// </summary>
    _4 = (int)Inno.Native.ImGui.ImGuiKey._4,
    /// <summary>
    /// Identifies the _5 presentation key or modifier.
    /// </summary>
    _5 = (int)Inno.Native.ImGui.ImGuiKey._5,
    /// <summary>
    /// Identifies the _6 presentation key or modifier.
    /// </summary>
    _6 = (int)Inno.Native.ImGui.ImGuiKey._6,
    /// <summary>
    /// Identifies the _7 presentation key or modifier.
    /// </summary>
    _7 = (int)Inno.Native.ImGui.ImGuiKey._7,
    /// <summary>
    /// Identifies the _8 presentation key or modifier.
    /// </summary>
    _8 = (int)Inno.Native.ImGui.ImGuiKey._8,
    /// <summary>
    /// Identifies the _9 presentation key or modifier.
    /// </summary>
    _9 = (int)Inno.Native.ImGui.ImGuiKey._9,
    /// <summary>
    /// Identifies the A presentation key or modifier.
    /// </summary>
    A = (int)Inno.Native.ImGui.ImGuiKey.A,
    /// <summary>
    /// Identifies the B presentation key or modifier.
    /// </summary>
    B = (int)Inno.Native.ImGui.ImGuiKey.B,
    /// <summary>
    /// Identifies the C presentation key or modifier.
    /// </summary>
    C = (int)Inno.Native.ImGui.ImGuiKey.C,
    /// <summary>
    /// Identifies the D presentation key or modifier.
    /// </summary>
    D = (int)Inno.Native.ImGui.ImGuiKey.D,
    /// <summary>
    /// Identifies the E presentation key or modifier.
    /// </summary>
    E = (int)Inno.Native.ImGui.ImGuiKey.E,
    /// <summary>
    /// Identifies the F presentation key or modifier.
    /// </summary>
    F = (int)Inno.Native.ImGui.ImGuiKey.F,
    /// <summary>
    /// Identifies the G presentation key or modifier.
    /// </summary>
    G = (int)Inno.Native.ImGui.ImGuiKey.G,
    /// <summary>
    /// Identifies the H presentation key or modifier.
    /// </summary>
    H = (int)Inno.Native.ImGui.ImGuiKey.H,
    /// <summary>
    /// Identifies the I presentation key or modifier.
    /// </summary>
    I = (int)Inno.Native.ImGui.ImGuiKey.I,
    /// <summary>
    /// Identifies the J presentation key or modifier.
    /// </summary>
    J = (int)Inno.Native.ImGui.ImGuiKey.J,
    /// <summary>
    /// Identifies the K presentation key or modifier.
    /// </summary>
    K = (int)Inno.Native.ImGui.ImGuiKey.K,
    /// <summary>
    /// Identifies the L presentation key or modifier.
    /// </summary>
    L = (int)Inno.Native.ImGui.ImGuiKey.L,
    /// <summary>
    /// Identifies the M presentation key or modifier.
    /// </summary>
    M = (int)Inno.Native.ImGui.ImGuiKey.M,
    /// <summary>
    /// Identifies the N presentation key or modifier.
    /// </summary>
    N = (int)Inno.Native.ImGui.ImGuiKey.N,
    /// <summary>
    /// Identifies the O presentation key or modifier.
    /// </summary>
    O = (int)Inno.Native.ImGui.ImGuiKey.O,
    /// <summary>
    /// Identifies the P presentation key or modifier.
    /// </summary>
    P = (int)Inno.Native.ImGui.ImGuiKey.P,
    /// <summary>
    /// Identifies the Q presentation key or modifier.
    /// </summary>
    Q = (int)Inno.Native.ImGui.ImGuiKey.Q,
    /// <summary>
    /// Identifies the R presentation key or modifier.
    /// </summary>
    R = (int)Inno.Native.ImGui.ImGuiKey.R,
    /// <summary>
    /// Identifies the S presentation key or modifier.
    /// </summary>
    S = (int)Inno.Native.ImGui.ImGuiKey.S,
    /// <summary>
    /// Identifies the T presentation key or modifier.
    /// </summary>
    T = (int)Inno.Native.ImGui.ImGuiKey.T,
    /// <summary>
    /// Identifies the U presentation key or modifier.
    /// </summary>
    U = (int)Inno.Native.ImGui.ImGuiKey.U,
    /// <summary>
    /// Identifies the V presentation key or modifier.
    /// </summary>
    V = (int)Inno.Native.ImGui.ImGuiKey.V,
    /// <summary>
    /// Identifies the W presentation key or modifier.
    /// </summary>
    W = (int)Inno.Native.ImGui.ImGuiKey.W,
    /// <summary>
    /// Identifies the X presentation key or modifier.
    /// </summary>
    X = (int)Inno.Native.ImGui.ImGuiKey.X,
    /// <summary>
    /// Identifies the Y presentation key or modifier.
    /// </summary>
    Y = (int)Inno.Native.ImGui.ImGuiKey.Y,
    /// <summary>
    /// Identifies the Z presentation key or modifier.
    /// </summary>
    Z = (int)Inno.Native.ImGui.ImGuiKey.Z,
    /// <summary>
    /// Identifies the F1 presentation key or modifier.
    /// </summary>
    F1 = (int)Inno.Native.ImGui.ImGuiKey.F1,
    /// <summary>
    /// Identifies the F2 presentation key or modifier.
    /// </summary>
    F2 = (int)Inno.Native.ImGui.ImGuiKey.F2,
    /// <summary>
    /// Identifies the F3 presentation key or modifier.
    /// </summary>
    F3 = (int)Inno.Native.ImGui.ImGuiKey.F3,
    /// <summary>
    /// Identifies the F4 presentation key or modifier.
    /// </summary>
    F4 = (int)Inno.Native.ImGui.ImGuiKey.F4,
    /// <summary>
    /// Identifies the F5 presentation key or modifier.
    /// </summary>
    F5 = (int)Inno.Native.ImGui.ImGuiKey.F5,
    /// <summary>
    /// Identifies the F6 presentation key or modifier.
    /// </summary>
    F6 = (int)Inno.Native.ImGui.ImGuiKey.F6,
    /// <summary>
    /// Identifies the F7 presentation key or modifier.
    /// </summary>
    F7 = (int)Inno.Native.ImGui.ImGuiKey.F7,
    /// <summary>
    /// Identifies the F8 presentation key or modifier.
    /// </summary>
    F8 = (int)Inno.Native.ImGui.ImGuiKey.F8,
    /// <summary>
    /// Identifies the F9 presentation key or modifier.
    /// </summary>
    F9 = (int)Inno.Native.ImGui.ImGuiKey.F9,
    /// <summary>
    /// Identifies the F10 presentation key or modifier.
    /// </summary>
    F10 = (int)Inno.Native.ImGui.ImGuiKey.F10,
    /// <summary>
    /// Identifies the F11 presentation key or modifier.
    /// </summary>
    F11 = (int)Inno.Native.ImGui.ImGuiKey.F11,
    /// <summary>
    /// Identifies the F12 presentation key or modifier.
    /// </summary>
    F12 = (int)Inno.Native.ImGui.ImGuiKey.F12,
    /// <summary>
    /// Identifies the F13 presentation key or modifier.
    /// </summary>
    F13 = (int)Inno.Native.ImGui.ImGuiKey.F13,
    /// <summary>
    /// Identifies the F14 presentation key or modifier.
    /// </summary>
    F14 = (int)Inno.Native.ImGui.ImGuiKey.F14,
    /// <summary>
    /// Identifies the F15 presentation key or modifier.
    /// </summary>
    F15 = (int)Inno.Native.ImGui.ImGuiKey.F15,
    /// <summary>
    /// Identifies the F16 presentation key or modifier.
    /// </summary>
    F16 = (int)Inno.Native.ImGui.ImGuiKey.F16,
    /// <summary>
    /// Identifies the F17 presentation key or modifier.
    /// </summary>
    F17 = (int)Inno.Native.ImGui.ImGuiKey.F17,
    /// <summary>
    /// Identifies the F18 presentation key or modifier.
    /// </summary>
    F18 = (int)Inno.Native.ImGui.ImGuiKey.F18,
    /// <summary>
    /// Identifies the F19 presentation key or modifier.
    /// </summary>
    F19 = (int)Inno.Native.ImGui.ImGuiKey.F19,
    /// <summary>
    /// Identifies the F20 presentation key or modifier.
    /// </summary>
    F20 = (int)Inno.Native.ImGui.ImGuiKey.F20,
    /// <summary>
    /// Identifies the F21 presentation key or modifier.
    /// </summary>
    F21 = (int)Inno.Native.ImGui.ImGuiKey.F21,
    /// <summary>
    /// Identifies the F22 presentation key or modifier.
    /// </summary>
    F22 = (int)Inno.Native.ImGui.ImGuiKey.F22,
    /// <summary>
    /// Identifies the F23 presentation key or modifier.
    /// </summary>
    F23 = (int)Inno.Native.ImGui.ImGuiKey.F23,
    /// <summary>
    /// Identifies the F24 presentation key or modifier.
    /// </summary>
    F24 = (int)Inno.Native.ImGui.ImGuiKey.F24,
    /// <summary>
    /// Identifies the Apostrophe presentation key or modifier.
    /// </summary>
    Apostrophe = (int)Inno.Native.ImGui.ImGuiKey.Apostrophe,
    /// <summary>
    /// Identifies the Comma presentation key or modifier.
    /// </summary>
    Comma = (int)Inno.Native.ImGui.ImGuiKey.Comma,
    /// <summary>
    /// Identifies the Minus presentation key or modifier.
    /// </summary>
    Minus = (int)Inno.Native.ImGui.ImGuiKey.Minus,
    /// <summary>
    /// Identifies the Period presentation key or modifier.
    /// </summary>
    Period = (int)Inno.Native.ImGui.ImGuiKey.Period,
    /// <summary>
    /// Identifies the Slash presentation key or modifier.
    /// </summary>
    Slash = (int)Inno.Native.ImGui.ImGuiKey.Slash,
    /// <summary>
    /// Identifies the Semicolon presentation key or modifier.
    /// </summary>
    Semicolon = (int)Inno.Native.ImGui.ImGuiKey.Semicolon,
    /// <summary>
    /// Identifies the Equal presentation key or modifier.
    /// </summary>
    Equal = (int)Inno.Native.ImGui.ImGuiKey.Equal,
    /// <summary>
    /// Identifies the LeftBracket presentation key or modifier.
    /// </summary>
    LeftBracket = (int)Inno.Native.ImGui.ImGuiKey.LeftBracket,
    /// <summary>
    /// Identifies the Backslash presentation key or modifier.
    /// </summary>
    Backslash = (int)Inno.Native.ImGui.ImGuiKey.Backslash,
    /// <summary>
    /// Identifies the RightBracket presentation key or modifier.
    /// </summary>
    RightBracket = (int)Inno.Native.ImGui.ImGuiKey.RightBracket,
    /// <summary>
    /// Identifies the GraveAccent presentation key or modifier.
    /// </summary>
    GraveAccent = (int)Inno.Native.ImGui.ImGuiKey.GraveAccent,
    /// <summary>
    /// Identifies the CapsLock presentation key or modifier.
    /// </summary>
    CapsLock = (int)Inno.Native.ImGui.ImGuiKey.CapsLock,
    /// <summary>
    /// Identifies the ScrollLock presentation key or modifier.
    /// </summary>
    ScrollLock = (int)Inno.Native.ImGui.ImGuiKey.ScrollLock,
    /// <summary>
    /// Identifies the NumLock presentation key or modifier.
    /// </summary>
    NumLock = (int)Inno.Native.ImGui.ImGuiKey.NumLock,
    /// <summary>
    /// Identifies the PrintScreen presentation key or modifier.
    /// </summary>
    PrintScreen = (int)Inno.Native.ImGui.ImGuiKey.PrintScreen,
    /// <summary>
    /// Identifies the Pause presentation key or modifier.
    /// </summary>
    Pause = (int)Inno.Native.ImGui.ImGuiKey.Pause,
    /// <summary>
    /// Identifies the Keypad0 presentation key or modifier.
    /// </summary>
    Keypad0 = (int)Inno.Native.ImGui.ImGuiKey.Keypad0,
    /// <summary>
    /// Identifies the Keypad1 presentation key or modifier.
    /// </summary>
    Keypad1 = (int)Inno.Native.ImGui.ImGuiKey.Keypad1,
    /// <summary>
    /// Identifies the Keypad2 presentation key or modifier.
    /// </summary>
    Keypad2 = (int)Inno.Native.ImGui.ImGuiKey.Keypad2,
    /// <summary>
    /// Identifies the Keypad3 presentation key or modifier.
    /// </summary>
    Keypad3 = (int)Inno.Native.ImGui.ImGuiKey.Keypad3,
    /// <summary>
    /// Identifies the Keypad4 presentation key or modifier.
    /// </summary>
    Keypad4 = (int)Inno.Native.ImGui.ImGuiKey.Keypad4,
    /// <summary>
    /// Identifies the Keypad5 presentation key or modifier.
    /// </summary>
    Keypad5 = (int)Inno.Native.ImGui.ImGuiKey.Keypad5,
    /// <summary>
    /// Identifies the Keypad6 presentation key or modifier.
    /// </summary>
    Keypad6 = (int)Inno.Native.ImGui.ImGuiKey.Keypad6,
    /// <summary>
    /// Identifies the Keypad7 presentation key or modifier.
    /// </summary>
    Keypad7 = (int)Inno.Native.ImGui.ImGuiKey.Keypad7,
    /// <summary>
    /// Identifies the Keypad8 presentation key or modifier.
    /// </summary>
    Keypad8 = (int)Inno.Native.ImGui.ImGuiKey.Keypad8,
    /// <summary>
    /// Identifies the Keypad9 presentation key or modifier.
    /// </summary>
    Keypad9 = (int)Inno.Native.ImGui.ImGuiKey.Keypad9,
    /// <summary>
    /// Identifies the KeypadDecimal presentation key or modifier.
    /// </summary>
    KeypadDecimal = (int)Inno.Native.ImGui.ImGuiKey.KeypadDecimal,
    /// <summary>
    /// Identifies the KeypadDivide presentation key or modifier.
    /// </summary>
    KeypadDivide = (int)Inno.Native.ImGui.ImGuiKey.KeypadDivide,
    /// <summary>
    /// Identifies the KeypadMultiply presentation key or modifier.
    /// </summary>
    KeypadMultiply = (int)Inno.Native.ImGui.ImGuiKey.KeypadMultiply,
    /// <summary>
    /// Identifies the KeypadSubtract presentation key or modifier.
    /// </summary>
    KeypadSubtract = (int)Inno.Native.ImGui.ImGuiKey.KeypadSubtract,
    /// <summary>
    /// Identifies the KeypadAdd presentation key or modifier.
    /// </summary>
    KeypadAdd = (int)Inno.Native.ImGui.ImGuiKey.KeypadAdd,
    /// <summary>
    /// Identifies the KeypadEnter presentation key or modifier.
    /// </summary>
    KeypadEnter = (int)Inno.Native.ImGui.ImGuiKey.KeypadEnter,
    /// <summary>
    /// Identifies the KeypadEqual presentation key or modifier.
    /// </summary>
    KeypadEqual = (int)Inno.Native.ImGui.ImGuiKey.KeypadEqual,
    /// <summary>
    /// Identifies the AppBack presentation key or modifier.
    /// </summary>
    AppBack = (int)Inno.Native.ImGui.ImGuiKey.AppBack,
    /// <summary>
    /// Identifies the AppForward presentation key or modifier.
    /// </summary>
    AppForward = (int)Inno.Native.ImGui.ImGuiKey.AppForward,
    /// <summary>
    /// Identifies the Oem102 presentation key or modifier.
    /// </summary>
    Oem102 = (int)Inno.Native.ImGui.ImGuiKey.Oem102,
    /// <summary>
    /// Identifies the GamepadStart presentation key or modifier.
    /// </summary>
    GamepadStart = (int)Inno.Native.ImGui.ImGuiKey.GamepadStart,
    /// <summary>
    /// Identifies the GamepadBack presentation key or modifier.
    /// </summary>
    GamepadBack = (int)Inno.Native.ImGui.ImGuiKey.GamepadBack,
    /// <summary>
    /// Identifies the GamepadFaceLeft presentation key or modifier.
    /// </summary>
    GamepadFaceLeft = (int)Inno.Native.ImGui.ImGuiKey.GamepadFaceLeft,
    /// <summary>
    /// Identifies the GamepadFaceRight presentation key or modifier.
    /// </summary>
    GamepadFaceRight = (int)Inno.Native.ImGui.ImGuiKey.GamepadFaceRight,
    /// <summary>
    /// Identifies the GamepadFaceUp presentation key or modifier.
    /// </summary>
    GamepadFaceUp = (int)Inno.Native.ImGui.ImGuiKey.GamepadFaceUp,
    /// <summary>
    /// Identifies the GamepadFaceDown presentation key or modifier.
    /// </summary>
    GamepadFaceDown = (int)Inno.Native.ImGui.ImGuiKey.GamepadFaceDown,
    /// <summary>
    /// Identifies the GamepadDpadLeft presentation key or modifier.
    /// </summary>
    GamepadDpadLeft = (int)Inno.Native.ImGui.ImGuiKey.GamepadDpadLeft,
    /// <summary>
    /// Identifies the GamepadDpadRight presentation key or modifier.
    /// </summary>
    GamepadDpadRight = (int)Inno.Native.ImGui.ImGuiKey.GamepadDpadRight,
    /// <summary>
    /// Identifies the GamepadDpadUp presentation key or modifier.
    /// </summary>
    GamepadDpadUp = (int)Inno.Native.ImGui.ImGuiKey.GamepadDpadUp,
    /// <summary>
    /// Identifies the GamepadDpadDown presentation key or modifier.
    /// </summary>
    GamepadDpadDown = (int)Inno.Native.ImGui.ImGuiKey.GamepadDpadDown,
    /// <summary>
    /// Identifies the GamepadL1 presentation key or modifier.
    /// </summary>
    GamepadL1 = (int)Inno.Native.ImGui.ImGuiKey.GamepadL1,
    /// <summary>
    /// Identifies the GamepadR1 presentation key or modifier.
    /// </summary>
    GamepadR1 = (int)Inno.Native.ImGui.ImGuiKey.GamepadR1,
    /// <summary>
    /// Identifies the GamepadL2 presentation key or modifier.
    /// </summary>
    GamepadL2 = (int)Inno.Native.ImGui.ImGuiKey.GamepadL2,
    /// <summary>
    /// Identifies the GamepadR2 presentation key or modifier.
    /// </summary>
    GamepadR2 = (int)Inno.Native.ImGui.ImGuiKey.GamepadR2,
    /// <summary>
    /// Identifies the GamepadL3 presentation key or modifier.
    /// </summary>
    GamepadL3 = (int)Inno.Native.ImGui.ImGuiKey.GamepadL3,
    /// <summary>
    /// Identifies the GamepadR3 presentation key or modifier.
    /// </summary>
    GamepadR3 = (int)Inno.Native.ImGui.ImGuiKey.GamepadR3,
    /// <summary>
    /// Identifies the GamepadLStickLeft presentation key or modifier.
    /// </summary>
    GamepadLStickLeft = (int)Inno.Native.ImGui.ImGuiKey.GamepadLStickLeft,
    /// <summary>
    /// Identifies the GamepadLStickRight presentation key or modifier.
    /// </summary>
    GamepadLStickRight = (int)Inno.Native.ImGui.ImGuiKey.GamepadLStickRight,
    /// <summary>
    /// Identifies the GamepadLStickUp presentation key or modifier.
    /// </summary>
    GamepadLStickUp = (int)Inno.Native.ImGui.ImGuiKey.GamepadLStickUp,
    /// <summary>
    /// Identifies the GamepadLStickDown presentation key or modifier.
    /// </summary>
    GamepadLStickDown = (int)Inno.Native.ImGui.ImGuiKey.GamepadLStickDown,
    /// <summary>
    /// Identifies the GamepadRStickLeft presentation key or modifier.
    /// </summary>
    GamepadRStickLeft = (int)Inno.Native.ImGui.ImGuiKey.GamepadRStickLeft,
    /// <summary>
    /// Identifies the GamepadRStickRight presentation key or modifier.
    /// </summary>
    GamepadRStickRight = (int)Inno.Native.ImGui.ImGuiKey.GamepadRStickRight,
    /// <summary>
    /// Identifies the GamepadRStickUp presentation key or modifier.
    /// </summary>
    GamepadRStickUp = (int)Inno.Native.ImGui.ImGuiKey.GamepadRStickUp,
    /// <summary>
    /// Identifies the GamepadRStickDown presentation key or modifier.
    /// </summary>
    GamepadRStickDown = (int)Inno.Native.ImGui.ImGuiKey.GamepadRStickDown,
    /// <summary>
    /// Identifies the MouseLeft presentation key or modifier.
    /// </summary>
    MouseLeft = (int)Inno.Native.ImGui.ImGuiKey.MouseLeft,
    /// <summary>
    /// Identifies the MouseRight presentation key or modifier.
    /// </summary>
    MouseRight = (int)Inno.Native.ImGui.ImGuiKey.MouseRight,
    /// <summary>
    /// Identifies the MouseMiddle presentation key or modifier.
    /// </summary>
    MouseMiddle = (int)Inno.Native.ImGui.ImGuiKey.MouseMiddle,
    /// <summary>
    /// Identifies the MouseX1 presentation key or modifier.
    /// </summary>
    MouseX1 = (int)Inno.Native.ImGui.ImGuiKey.MouseX1,
    /// <summary>
    /// Identifies the MouseX2 presentation key or modifier.
    /// </summary>
    MouseX2 = (int)Inno.Native.ImGui.ImGuiKey.MouseX2,
    /// <summary>
    /// Identifies the MouseWheelX presentation key or modifier.
    /// </summary>
    MouseWheelX = (int)Inno.Native.ImGui.ImGuiKey.MouseWheelX,
    /// <summary>
    /// Identifies the MouseWheelY presentation key or modifier.
    /// </summary>
    MouseWheelY = (int)Inno.Native.ImGui.ImGuiKey.MouseWheelY,
    /// <summary>
    /// Identifies the ReservedForModCtrl presentation key or modifier.
    /// </summary>
    ReservedForModCtrl = (int)Inno.Native.ImGui.ImGuiKey.ReservedForModCtrl,
    /// <summary>
    /// Identifies the ReservedForModShift presentation key or modifier.
    /// </summary>
    ReservedForModShift = (int)Inno.Native.ImGui.ImGuiKey.ReservedForModShift,
    /// <summary>
    /// Identifies the ReservedForModAlt presentation key or modifier.
    /// </summary>
    ReservedForModAlt = (int)Inno.Native.ImGui.ImGuiKey.ReservedForModAlt,
    /// <summary>
    /// Identifies the ReservedForModSuper presentation key or modifier.
    /// </summary>
    ReservedForModSuper = (int)Inno.Native.ImGui.ImGuiKey.ReservedForModSuper,
    /// <summary>
    /// Identifies the NamedKeyEnd presentation key or modifier.
    /// </summary>
    NamedKeyEnd = (int)Inno.Native.ImGui.ImGuiKey.NamedKeyEnd,
    /// <summary>
    /// Identifies the NamedKeyCount presentation key or modifier.
    /// </summary>
    NamedKeyCount = (int)Inno.Native.ImGui.ImGuiKey.NamedKeyCount,
    /// <summary>
    /// Identifies the ImGuiModNone presentation key or modifier.
    /// </summary>
    ImGuiModNone = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModNone,
    /// <summary>
    /// Identifies the ImGuiModCtrl presentation key or modifier.
    /// </summary>
    ImGuiModCtrl = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModCtrl,
    /// <summary>
    /// Identifies the ImGuiModShift presentation key or modifier.
    /// </summary>
    ImGuiModShift = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModShift,
    /// <summary>
    /// Identifies the ImGuiModAlt presentation key or modifier.
    /// </summary>
    ImGuiModAlt = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModAlt,
    /// <summary>
    /// Identifies the ImGuiModSuper presentation key or modifier.
    /// </summary>
    ImGuiModSuper = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModSuper,
    /// <summary>
    /// Identifies the ImGuiModMask presentation key or modifier.
    /// </summary>
    ImGuiModMask = (int)Inno.Native.ImGui.ImGuiKey.ImGuiModMask,
}
