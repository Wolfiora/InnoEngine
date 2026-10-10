# Inno.Core.Input

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [下一页：Diagnose](Inno.Core.Diagnostics.md)

Input 项目只定义跨平台输入语义枚举，不维护按键状态。状态采集由 Platform 层完成，事件传递参见 [Inno.Core.Events](Inno.Core.Events.md)。

## KeyCode

| 分组 | 值 |
| --- | --- |
| 未知 | `Unknown` |
| 字母 | `A`–`Z` |
| 主键盘数字 | `D0`–`D9` |
| 常用控制 | `Escape`, `Space`, `Enter`, `Tab`, `Backspace` |
| 方向 | `LeftArrow`, `UpArrow`, `RightArrow`, `DownArrow` |
| 修饰键实体 | `LeftSuper`, `RightSuper`, `LeftShift`, `RightShift`, `LeftCtrl`, `RightCtrl`, `LeftAlt`, `RightAlt` |
| 编辑/导航 | `CapsLock`, `Insert`, `Delete`, `Home`, `End`, `PageUp`, `PageDown` |
| 数字键盘 | `NumPad0`–`NumPad9`, `NumLock` |
| 锁定 | `ScrollLock` |
| 功能键 | `F1`–`F12` |
| 标点 | `Plus`, `Comma`, `Minus`, `Period`, `Slash`, `Tilde`, `Backslash`, `Semicolon`, `Quote`, `LeftBracket`, `RightBracket` |

枚举数值接近常见虚拟键码，但业务代码应比较枚举名，不要假定所有平台都直接提供相同整数。

## KeyModifier

`[Flags]` enum：`None`、`Alt`、`Control`、`Shift`、`Super`。

```csharp
bool saveShortcut = e.key == KeyCode.S &&
    (e.modifiers & KeyModifier.Control) != 0;
```

macOS Command / Windows key 等平台主修饰键可映射到 `Super`。`KeyModifier` 表示事件发生时的组合状态，与 `KeyCode.LeftCtrl` 这种具体实体键不同。

## MouseButton

`Left`、`Right`、`Middle`、`XButton1`、`XButton2`。

## MouseCursor

| 值 | 用途 |
| --- | --- |
| `None` | 隐藏/不设置光标。 |
| `Arrow` | 默认箭头。 |
| `TextInput` | 文本输入 I-beam。 |
| `ResizeAll` | 全方向移动/缩放。 |
| `ResizeNS`, `ResizeEW` | 垂直、水平 resize。 |
| `ResizeNESW`, `ResizeNWSE` | 两种对角 resize。 |
| `Hand` | 可点击链接/抓手。 |
| `NotAllowed` | 禁止操作。 |

## 事件示例

```csharp
hub.Listen<MouseButtonPressedEvent>(e =>
{
    if (e.button == MouseButton.Left)
        BeginSelection();
});

hub.Listen<KeyPressedEvent>(e =>
{
    if (e.key == KeyCode.F5 && !e.repeat)
        StartGame();
});
```

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Input.KeyCode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyCode`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L6) | Represents a backend-neutral physical key used by runtime input and editor shortcut contracts. |
| [`Inno.Core.Input.KeyCode.A`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L16) | The A letter key. |
| [`Inno.Core.Input.KeyCode.B`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L20) | The B letter key. |
| [`Inno.Core.Input.KeyCode.Backslash`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L385) | The backslash key. |
| [`Inno.Core.Input.KeyCode.Backspace`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L178) | The backspace key. |
| [`Inno.Core.Input.KeyCode.C`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L24) | The C letter key. |
| [`Inno.Core.Input.KeyCode.CapsLock`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L233) | The caps lock key. |
| [`Inno.Core.Input.KeyCode.Comma`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L365) | The comma key. |
| [`Inno.Core.Input.KeyCode.D`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L28) | The D letter key. |
| [`Inno.Core.Input.KeyCode.D0`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L121) | The 0 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D1`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L125) | The 1 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D2`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L129) | The 2 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D3`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L133) | The 3 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D4`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L137) | The 4 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D5`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L141) | The 5 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D6`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L145) | The 6 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D7`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L149) | The 7 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D8`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L153) | The 8 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.D9`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L157) | The 9 key on the primary keyboard row. |
| [`Inno.Core.Input.KeyCode.Delete`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L241) | The delete key. |
| [`Inno.Core.Input.KeyCode.DownArrow`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L195) | The down arrow key. |
| [`Inno.Core.Input.KeyCode.E`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L32) | The E letter key. |
| [`Inno.Core.Input.KeyCode.End`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L249) | The end key. |
| [`Inno.Core.Input.KeyCode.Enter`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L170) | The enter key. |
| [`Inno.Core.Input.KeyCode.Escape`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L162) | The escape key. |
| [`Inno.Core.Input.KeyCode.F`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L36) | The F letter key. |
| [`Inno.Core.Input.KeyCode.F1`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L312) | The F1 function key. |
| [`Inno.Core.Input.KeyCode.F10`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L348) | The F10 function key. |
| [`Inno.Core.Input.KeyCode.F11`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L352) | The F11 function key. |
| [`Inno.Core.Input.KeyCode.F12`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L356) | The F12 function key. |
| [`Inno.Core.Input.KeyCode.F2`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L316) | The F2 function key. |
| [`Inno.Core.Input.KeyCode.F3`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L320) | The F3 function key. |
| [`Inno.Core.Input.KeyCode.F4`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L324) | The F4 function key. |
| [`Inno.Core.Input.KeyCode.F5`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L328) | The F5 function key. |
| [`Inno.Core.Input.KeyCode.F6`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L332) | The F6 function key. |
| [`Inno.Core.Input.KeyCode.F7`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L336) | The F7 function key. |
| [`Inno.Core.Input.KeyCode.F8`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L340) | The F8 function key. |
| [`Inno.Core.Input.KeyCode.F9`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L344) | The F9 function key. |
| [`Inno.Core.Input.KeyCode.G`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L40) | The G letter key. |
| [`Inno.Core.Input.KeyCode.H`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L44) | The H letter key. |
| [`Inno.Core.Input.KeyCode.Home`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L245) | The home key. |
| [`Inno.Core.Input.KeyCode.I`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L48) | The I letter key. |
| [`Inno.Core.Input.KeyCode.Insert`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L237) | The insert key. |
| [`Inno.Core.Input.KeyCode.J`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L52) | The J letter key. |
| [`Inno.Core.Input.KeyCode.K`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L56) | The K letter key. |
| [`Inno.Core.Input.KeyCode.L`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L60) | The L letter key. |
| [`Inno.Core.Input.KeyCode.LeftAlt`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L224) | The left alt key. |
| [`Inno.Core.Input.KeyCode.LeftArrow`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L183) | The left arrow key. |
| [`Inno.Core.Input.KeyCode.LeftBracket`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L397) | The left bracket key. |
| [`Inno.Core.Input.KeyCode.LeftCtrl`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L216) | The left ctrl key. |
| [`Inno.Core.Input.KeyCode.LeftShift`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L208) | The left shift key. |
| [`Inno.Core.Input.KeyCode.LeftSuper`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L200) | The left super key. |
| [`Inno.Core.Input.KeyCode.M`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L64) | The M letter key. |
| [`Inno.Core.Input.KeyCode.Minus`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L369) | The minus key. |
| [`Inno.Core.Input.KeyCode.N`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L68) | The N letter key. |
| [`Inno.Core.Input.KeyCode.NumLock`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L303) | The num lock key. |
| [`Inno.Core.Input.KeyCode.NumPad0`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L262) | The 0 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad1`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L266) | The 1 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad2`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L270) | The 2 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad3`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L274) | The 3 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad4`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L278) | The 4 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad5`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L282) | The 5 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad6`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L286) | The 6 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad7`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L290) | The 7 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad8`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L294) | The 8 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.NumPad9`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L298) | The 9 key on the numeric keypad. |
| [`Inno.Core.Input.KeyCode.O`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L72) | The O letter key. |
| [`Inno.Core.Input.KeyCode.P`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L76) | The P letter key. |
| [`Inno.Core.Input.KeyCode.PageDown`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L257) | The page down key. |
| [`Inno.Core.Input.KeyCode.PageUp`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L253) | The page up key. |
| [`Inno.Core.Input.KeyCode.Period`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L373) | The period key. |
| [`Inno.Core.Input.KeyCode.Plus`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L361) | The plus key. |
| [`Inno.Core.Input.KeyCode.Q`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L80) | The Q letter key. |
| [`Inno.Core.Input.KeyCode.Quote`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L393) | The quote key. |
| [`Inno.Core.Input.KeyCode.R`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L84) | The R letter key. |
| [`Inno.Core.Input.KeyCode.RightAlt`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L228) | The right alt key. |
| [`Inno.Core.Input.KeyCode.RightArrow`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L191) | The right arrow key. |
| [`Inno.Core.Input.KeyCode.RightBracket`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L401) | The right bracket key. |
| [`Inno.Core.Input.KeyCode.RightCtrl`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L220) | The right ctrl key. |
| [`Inno.Core.Input.KeyCode.RightShift`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L212) | The right shift key. |
| [`Inno.Core.Input.KeyCode.RightSuper`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L204) | The right super key. |
| [`Inno.Core.Input.KeyCode.S`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L88) | The S letter key. |
| [`Inno.Core.Input.KeyCode.ScrollLock`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L307) | The scroll lock key. |
| [`Inno.Core.Input.KeyCode.Semicolon`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L389) | The semicolon key. |
| [`Inno.Core.Input.KeyCode.Slash`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L377) | The slash key. |
| [`Inno.Core.Input.KeyCode.Space`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L166) | The space key. |
| [`Inno.Core.Input.KeyCode.T`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L92) | The T letter key. |
| [`Inno.Core.Input.KeyCode.Tab`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L174) | The tab key. |
| [`Inno.Core.Input.KeyCode.Tilde`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L381) | The tilde key. |
| [`Inno.Core.Input.KeyCode.U`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L96) | The U letter key. |
| [`Inno.Core.Input.KeyCode.Unknown`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L11) | No recognized physical key. |
| [`Inno.Core.Input.KeyCode.UpArrow`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L187) | The up arrow key. |
| [`Inno.Core.Input.KeyCode.V`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L100) | The V letter key. |
| [`Inno.Core.Input.KeyCode.W`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L104) | The W letter key. |
| [`Inno.Core.Input.KeyCode.X`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L108) | The X letter key. |
| [`Inno.Core.Input.KeyCode.Y`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L112) | The Y letter key. |
| [`Inno.Core.Input.KeyCode.Z`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L116) | The Z letter key. |

### `Inno.Core.Input.KeyModifier`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L8) | Identifies the supported key modifier values for this contract. |
| [`Inno.Core.Input.KeyModifier.Alt`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L18) | The alt key. |
| [`Inno.Core.Input.KeyModifier.Control`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L22) | The control key. |
| [`Inno.Core.Input.KeyModifier.None`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L14) | The none key. |
| [`Inno.Core.Input.KeyModifier.Shift`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L26) | The shift key. |
| [`Inno.Core.Input.KeyModifier.Super`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L30) | The super key. |

### `Inno.Core.Input.MouseButton`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.MouseButton`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L6) | Identifies the supported mouse button values for this contract. |
| [`Inno.Core.Input.MouseButton.Left`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L11) | The left key. |
| [`Inno.Core.Input.MouseButton.Middle`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L19) | The middle key. |
| [`Inno.Core.Input.MouseButton.Right`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L15) | The right key. |
| [`Inno.Core.Input.MouseButton.XButton1`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L23) | The xbutton1 key. |
| [`Inno.Core.Input.MouseButton.XButton2`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L27) | The xbutton2 key. |

### `Inno.Core.Input.MouseCursor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.MouseCursor`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L6) | Identifies the supported mouse cursor values for this contract. |
| [`Inno.Core.Input.MouseCursor.Arrow`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L16) | The arrow key. |
| [`Inno.Core.Input.MouseCursor.Hand`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L44) | The hand key. |
| [`Inno.Core.Input.MouseCursor.None`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L11) | The none key. |
| [`Inno.Core.Input.MouseCursor.NotAllowed`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L48) | The not allowed key. |
| [`Inno.Core.Input.MouseCursor.ResizeAll`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L24) | The resize all key. |
| [`Inno.Core.Input.MouseCursor.ResizeEW`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L32) | The resize ew key. |
| [`Inno.Core.Input.MouseCursor.ResizeNESW`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L36) | The resize nesw key. |
| [`Inno.Core.Input.MouseCursor.ResizeNS`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L28) | The resize ns key. |
| [`Inno.Core.Input.MouseCursor.ResizeNWSE`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L40) | The resize nwse key. |
| [`Inno.Core.Input.MouseCursor.TextInput`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L20) | The text input key. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
