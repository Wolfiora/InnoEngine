# Inno.Core.Input

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [下一页：Diagnose](Inno.Core.Diagnostics.md)

Input 项目定义跨平台输入值、不可变的输入变化及其原始顺序，不维护窗口焦点或实时按键状态。采集由平台适配器完成，事件传递复用 [Inno.Core.Events](Inno.Core.Events.md)，每个运行 Session 的状态属于 [Inno.Input](../input/Inno.Input.md)。

`InputSequence` 保存按采集顺序排列的 `KeyInputChange`、`TextInputChange` 和定位指针变化。每条变化保留当时的物理修饰键；指针变化保留当时的位置，不能用帧末位置替换。同一帧的按下、移动、释放，以及文本、光标移动、文本，均按原顺序交付。pressed/released、文本和 delta 摘要只从此序列派生。

`MapPointer` 同步映射当前与前一指针位置，不缩放滚轮、不保存映射委托，也不改变键盘或文本顺序。输入为空时复用 `InputSequence.empty`。该值不是事件总线或输入所有权服务。

`TextCompositionChange` 区分 Started、Updated、Committed 和 Cancelled。不可变的 `TextCompositionState` 保存所属路由递增操作 ID、完整文本及 Unicode scalar 选区；它不保存输入法或控件对象。文本必须是合法 Unicode，选区越界明确拒绝。取消文本不进入提交摘要；只有 Committed 将文本加入 `InputSequence.textInput`，不会重复提交。

`hasTextCompositionActivity` 保留本帧开始、更新及终止事实。帧末已经没有活动组合，不代表本帧的 Enter 可以再次触发游戏动作。按键原始顺序和事实仍保留，由输入消费边界区分候选确认与业务动作。

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
using Inno.Core.Input;

static bool IsSaveShortcut(
    KeyCode key,
    KeyModifier modifiers
) {
    return key == KeyCode.S && (modifiers & KeyModifier.Control) != 0;
}
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

## 事件组合

本项目定义输入值，不拥有事件派发器。订阅、派发和订阅释放见 [Core Events 输入事件示例](Inno.Core.Events.md#输入事件组合)。

## 手柄输入事实

`GamepadInputChange` 的连接、按钮、轴变化属于同一 `InputSequence`，不生成键盘事件。`deviceId` 只标识当前物理连接，不能作为对象 persistent ID 或保存到项目数据。标准按钮按物理位置命名；摇杆范围为 [-1, 1]，右／下为正，扳机为 [0, 1]。

`GamepadState` 复制并冻结按钮与轴。`GetAxis(axis, deadZone)` 使用明确的轴向死区，并把死区以外的行程重新映射到完整范围；无样本返回零，非有限值及越界输入明确失败。该值不拥有设备或窗口，也不决定菜单导航、重复或事件消费。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

### `Inno.Core.Input.TextCompositionChange`

<!-- api: T:Inno.Core.Input.TextCompositionChange 6e601ec2752097083e12e856e7c393c7e9e9f88fa87c7fc3eef9e4d13d127b2f -->
<!-- api: P:Inno.Core.Input.TextCompositionChange.EqualityContract aaa4359082492d53e7ca626ec0165bf03394d88f70da4ac00fe2e5c2dee799f0 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.#ctor(Inno.Core.Input.TextCompositionPhase,Inno.Core.Input.TextCompositionState,Inno.Core.Input.KeyModifier) 08ecea2d94e5ee8772c7cc11dd435a7c4b55e4006419d095298c02bc9e08750d -->
<!-- api: P:Inno.Core.Input.TextCompositionChange.phase e945900f797873467910c2f1556cb0bb97b5ea5319157f315c091b9c343b14ea -->
<!-- api: P:Inno.Core.Input.TextCompositionChange.state 21e1398253785c080380310ba382db503e512add434fcdbecb4ca89d4069f643 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.ToString 97754d368832c2d429cc8dfd429531321e05b3e82b1e3e06d01091703eba1a64 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.PrintMembers(System.Text.StringBuilder) 72bdf03c45371f94a609bcd84afb6a602a568695250271a18c7a132e8a63e68d -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.op_Inequality(Inno.Core.Input.TextCompositionChange,Inno.Core.Input.TextCompositionChange) e6be0f47b1e7a6d2217fadafede9765ce43378c19b68b764d6db010172f5d11a -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.op_Equality(Inno.Core.Input.TextCompositionChange,Inno.Core.Input.TextCompositionChange) 063c0b77ff0eafeb7d7ffd8a317385d564f4f4a15f9e688dfa90ab3eb9b039c6 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.GetHashCode 80347a607177a290f7bef404ae296f66af8520524212e80003e383b74334934e -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.Equals(System.Object) 3f64f577d8c41fffdcf01cbfd9ddc6df176475a7c1adb65b27384ef344332e41 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.Equals(Inno.Core.Input.InputChange) 5a7e7414e07ec4ec577f357b18efe19bb4892b195709a09984401b8ee5313715 -->
<!-- api: M:Inno.Core.Input.TextCompositionChange.Equals(Inno.Core.Input.TextCompositionChange) 035cdc019731e8246394772278fbe73ef821eae4f79e18019ac67ed46b8a2591 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-f58dc10f534b2ba8"></a>[`public sealed record Inno.Core.Input.TextCompositionChange : Inno.Core.Input.InputChange, System.IEquatable<Inno.Core.Input.TextCompositionChange>`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Places an input-method composition transition among the ordered physical input facts. | — |
| <a id="api-0ada3c8d20bf8c42"></a>[`protected override System.Type Inno.Core.Input.TextCompositionChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-312db6fe95b60319"></a>[`public Inno.Core.Input.TextCompositionChange.TextCompositionChange(Inno.Core.Input.TextCompositionPhase phase, Inno.Core.Input.TextCompositionState state, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L28) | Creates an immutable composition transition.<br>`phase`：Whether composition starts, changes, commits or cancels.<br>`state`：Operation identity and complete text at this transition; cancellation text is never committed.<br>`modifiers`：Physical modifiers observed at the transition. | `System.ArgumentNullException`：The state is null.<br>`System.ArgumentOutOfRangeException`：The phase is not defined. |
| <a id="api-a9001dfe4abe419d"></a>[`public Inno.Core.Input.TextCompositionPhase Inno.Core.Input.TextCompositionChange.phase { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L43) | Gets the transition, including the distinction between commit and cancellation. | — |
| <a id="api-f31713a4192fb4e3"></a>[`public Inno.Core.Input.TextCompositionState Inno.Core.Input.TextCompositionChange.state { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L47) | Gets the immutable identity, text and scalar selection. | — |
| <a id="api-6cc843046c0b43d8"></a>[`public override string Inno.Core.Input.TextCompositionChange.ToString()`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-2dc63a13d7cfc35b"></a>[`protected override bool Inno.Core.Input.TextCompositionChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-7d3a9e7258fb3bb0"></a>[`public static bool Inno.Core.Input.TextCompositionChange.operator !=(Inno.Core.Input.TextCompositionChange? left, Inno.Core.Input.TextCompositionChange? right)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-5dfab6a4429785b5"></a>[`public static bool Inno.Core.Input.TextCompositionChange.operator ==(Inno.Core.Input.TextCompositionChange? left, Inno.Core.Input.TextCompositionChange? right)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-aab93e29b7e26597"></a>[`public override int Inno.Core.Input.TextCompositionChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-693f2eb28add701e"></a>[`public override bool Inno.Core.Input.TextCompositionChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-4b1525e2b9c953c1"></a>[`public override sealed bool Inno.Core.Input.TextCompositionChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-0245104fec322bbf"></a>[`public bool Inno.Core.Input.TextCompositionChange.Equals(Inno.Core.Input.TextCompositionChange? other)`](../../src/foundation/core/Inno.Core.Input/TextCompositionChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.TextCompositionState`

<!-- api: T:Inno.Core.Input.TextCompositionState 1f561582ec0c52c36e186970c832abbd0cf4221c35dcb501a45685dcbdc99ac8 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.#ctor(System.UInt64,System.String,System.Int32,System.Int32) c4cb3b6cf3fdbc279b59db1625101917e9745cde6036e340934f4113899c858e -->
<!-- api: P:Inno.Core.Input.TextCompositionState.compositionId c50c8438c59dfafda77c431a8ace9a135d2b79e0f4b2efdb6bbb7923b7ffb558 -->
<!-- api: P:Inno.Core.Input.TextCompositionState.text 43104feb288e4ebe314bd615c7cbddaef6be75b04baaafedcaadf8cb78ecdfa4 -->
<!-- api: P:Inno.Core.Input.TextCompositionState.selectionStart 9eb9daeb07a7a9fe4d150d69ddae8a35994a032357eb429536a0c584335de554 -->
<!-- api: P:Inno.Core.Input.TextCompositionState.selectionLength 694fad8fbccf132a218c72ae7f4b7af00dbbeeaf19bfb6fad96193fbdb51d802 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.ToString 1d0d34e61cb6b5a1ca1c8ef0cad61a811635f352e5ed60476bf1a45c11ae28b4 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.op_Inequality(Inno.Core.Input.TextCompositionState,Inno.Core.Input.TextCompositionState) 553980ee44263f4158dab41e6c5cb132fb9b60f4e4ca24ea6f7421c5169e930c -->
<!-- api: M:Inno.Core.Input.TextCompositionState.op_Equality(Inno.Core.Input.TextCompositionState,Inno.Core.Input.TextCompositionState) f7fccabfcc792957e1f0ba077c4e9fa1616fca167e4c1e6e66721e0cb407e5d9 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.GetHashCode f92a51f3bdad8782eb2ec8464047333c0e6f336e7b75bea34e07dae97354a733 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.Equals(System.Object) e570979dbbea57f80a0da36045859d0064d8a3c2c6cb10909d3fcda8917fe650 -->
<!-- api: M:Inno.Core.Input.TextCompositionState.Equals(Inno.Core.Input.TextCompositionState) 56274796d477e096899d6f3ea6ec45cf74fdf0379877c0402163d5d2abb81cd8 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-cbc9bf49217e9649"></a>[`public sealed record Inno.Core.Input.TextCompositionState : System.IEquatable<Inno.Core.Input.TextCompositionState>`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | Freezes provisional or committed text without retaining an operating-system input context. | — |
| <a id="api-7400cf2905a7b42b"></a>[`public Inno.Core.Input.TextCompositionState.TextCompositionState(ulong compositionId, string text, int selectionStart = -1, int selectionLength = 0)`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L36) | Captures one composition using Unicode scalar positions, independently of UTF-8 or UTF-16 storage.<br>`compositionId`：Nonzero operation identity, increasing for each composition in its owning input route.<br>`text`：Well-formed Unicode text; empty provisional text is allowed.<br>`selectionStart`：Selection start in Unicode scalar values, or -1 when the input method supplies no selection.<br>`selectionLength`：Selection length in Unicode scalar values; must be zero when the start is unspecified. | `System.ArgumentNullException`：The text is null.<br>`System.ArgumentException`：The text contains an unpaired UTF-16 surrogate.<br>`System.ArgumentOutOfRangeException`：The identity is zero or the selection is outside the text. |
| <a id="api-a56f29cf2604510c"></a>[`public ulong Inno.Core.Input.TextCompositionState.compositionId { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L66) | Gets the input-route operation identity, never an object or persistent asset identity. | — |
| <a id="api-52ab42645c4ffaa8"></a>[`public string Inno.Core.Input.TextCompositionState.text { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L70) | Gets the complete text without normalization or native storage ownership. | — |
| <a id="api-c813cfdd9f1ac4db"></a>[`public int Inno.Core.Input.TextCompositionState.selectionStart { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L74) | Gets the scalar selection start, or -1 when unspecified. | — |
| <a id="api-c0aad5038a84b6d5"></a>[`public int Inno.Core.Input.TextCompositionState.selectionLength { get; }`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L78) | Gets the scalar selection length. | — |
| <a id="api-af4981f83a89062d"></a>[`public override string Inno.Core.Input.TextCompositionState.ToString()`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-bfd128c75bd35fdd"></a>[`public static bool Inno.Core.Input.TextCompositionState.operator !=(Inno.Core.Input.TextCompositionState? left, Inno.Core.Input.TextCompositionState? right)`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | 编译器生成的值不等比较。 | — |
| <a id="api-f02c7e55a60a3b15"></a>[`public static bool Inno.Core.Input.TextCompositionState.operator ==(Inno.Core.Input.TextCompositionState? left, Inno.Core.Input.TextCompositionState? right)`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | 编译器生成的记录值相等比较。 | — |
| <a id="api-1445e3ad03937ba3"></a>[`public override int Inno.Core.Input.TextCompositionState.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-6927fa4f4493a17a"></a>[`public override bool Inno.Core.Input.TextCompositionState.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-3299b3742231234b"></a>[`public bool Inno.Core.Input.TextCompositionState.Equals(Inno.Core.Input.TextCompositionState? other)`](../../src/foundation/core/Inno.Core.Input/TextCompositionState.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.TextCompositionPhase`

<!-- api: T:Inno.Core.Input.TextCompositionPhase 65c83ef791457c07a0aec6c6012e5bc207879fdc10f3bc8207f890d56f0d45b5 -->
<!-- api: F:Inno.Core.Input.TextCompositionPhase.Started 75fb7be1563f2df41de1615b0f5c33866105f03f8eac655b654025fe584b8194 -->
<!-- api: F:Inno.Core.Input.TextCompositionPhase.Updated b40d0e4a8af4aeed60e85d9fe8da638ef9206892c5352b86ba20a32449b683d9 -->
<!-- api: F:Inno.Core.Input.TextCompositionPhase.Committed f0bd9db400ce36b144ffeb13ee9c313ea1baaa28569780ce4e176050d91b1fd4 -->
<!-- api: F:Inno.Core.Input.TextCompositionPhase.Cancelled c8a0f77878882418b7637ecb9fa61e46ebf56c552f71a012c5f1c0cca70bb6cc -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-4c1886268ab41966"></a>[`public enum Inno.Core.Input.TextCompositionPhase : int`](../../src/foundation/core/Inno.Core.Input/TextCompositionPhase.cs#L6) | Distinguishes the lifetime transitions of one input-method composition. | — |
| <a id="api-c0cc6c2bba914e62"></a>[`Inno.Core.Input.TextCompositionPhase.Started = 0`](../../src/foundation/core/Inno.Core.Input/TextCompositionPhase.cs#L11) | A new composition owns provisional text and a new operation identity. | — |
| <a id="api-ec4eeb1533f3ef39"></a>[`Inno.Core.Input.TextCompositionPhase.Updated = 1`](../../src/foundation/core/Inno.Core.Input/TextCompositionPhase.cs#L15) | Provisional text or its selection changed without committing content. | — |
| <a id="api-6e7ea05bbdf6b0c1"></a>[`Inno.Core.Input.TextCompositionPhase.Committed = 2`](../../src/foundation/core/Inno.Core.Input/TextCompositionPhase.cs#L19) | The composition ended with the supplied committed text. | — |
| <a id="api-58ef31a1520733cb"></a>[`Inno.Core.Input.TextCompositionPhase.Cancelled = 3`](../../src/foundation/core/Inno.Core.Input/TextCompositionPhase.cs#L23) | The composition ended without committing its provisional text. | — |

### `Inno.Core.Input.GamepadAxis`

<!-- api: T:Inno.Core.Input.GamepadAxis e293373e95c86e73302a7b18a58028d1f00301b4dd67db39ee7d46867f583c97 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.LeftX 58a362062573aeca03812192e866124feb7c76fec342d9b26477942e0c7245e4 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.LeftY b4ad53ad7c6efa3e02d3647bb7f286b95abce565a775a616c91a233fda75f3b1 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.RightX 42269031f553db7ab4a16683885e1dddab6c80cafeee9a320f48eeecc2fe0311 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.RightY 2ec160a608b772d18dc8d44af7068463707a7b47b556b574ad27e3d9ab6c6cb4 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.LeftTrigger 7c19cd785b77e42e674b57c998f98602de0d2af1557fdc375cba3f40cd483ea1 -->
<!-- api: F:Inno.Core.Input.GamepadAxis.RightTrigger 05b5aefc2fba5fe32f7cdf9ac1cd57b20be15ebea334e6b8b2fac07e60684325 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-a39ac24aeb669aa2"></a>[`public enum Inno.Core.Input.GamepadAxis : int`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L6) | Names normalized controller axes; stick down/right is positive and triggers range from zero to one. | — |
| <a id="api-d2c43b5f7563f033"></a>[`Inno.Core.Input.GamepadAxis.LeftX = 0`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L11) | Left stick horizontal displacement. | — |
| <a id="api-a44d3500740e8e3c"></a>[`Inno.Core.Input.GamepadAxis.LeftY = 1`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L15) | Left stick vertical displacement. | — |
| <a id="api-bc3ec9ee542fdf20"></a>[`Inno.Core.Input.GamepadAxis.RightX = 2`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L19) | Right stick horizontal displacement. | — |
| <a id="api-2d2feb2f1a41a355"></a>[`Inno.Core.Input.GamepadAxis.RightY = 3`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L23) | Right stick vertical displacement. | — |
| <a id="api-89f3995b47393ca0"></a>[`Inno.Core.Input.GamepadAxis.LeftTrigger = 4`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L27) | Left trigger travel. | — |
| <a id="api-f9d1cdd196371907"></a>[`Inno.Core.Input.GamepadAxis.RightTrigger = 5`](../../src/foundation/core/Inno.Core.Input/GamepadAxis.cs#L31) | Right trigger travel. | — |

### `Inno.Core.Input.GamepadAxisChange`

<!-- api: T:Inno.Core.Input.GamepadAxisChange d1f6b7ff1f4bed4f861e4e100a3fcaa11eaa54c7a21ac6dae7f2ceacb1b50065 -->
<!-- api: P:Inno.Core.Input.GamepadAxisChange.EqualityContract 9acbdcf6e0c79a6ac1589e615e32863e813124c9dce4c10d713073a25ad07a43 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.#ctor(System.UInt64,Inno.Core.Input.GamepadAxis,System.Single) 6d2b1d51812b963a247ff21c70980cb78c7a0a32e8ec066acd1bc5b64831ea57 -->
<!-- api: P:Inno.Core.Input.GamepadAxisChange.axis 79bfb0fda13f95b123cb6da1f79111126c71e66426531d90bbacf05e57678563 -->
<!-- api: P:Inno.Core.Input.GamepadAxisChange.value 20fc252c816dad81883d90554a9f5348dcfc1c865a4e4348b126cc5dfd5a1e62 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.ToString 51de84de2042065fe968bca0207661e11f07b21d5cf036196ea787ee1c414a40 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.PrintMembers(System.Text.StringBuilder) 65def3a24fcffc097a78e795b777b1e3243bc2912ad0cbc7311a4abc17adf6cd -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.op_Inequality(Inno.Core.Input.GamepadAxisChange,Inno.Core.Input.GamepadAxisChange) fab6501d85b58b5e524d24533a175f95f54714b9c3428753ec0dc2a94d1770e0 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.op_Equality(Inno.Core.Input.GamepadAxisChange,Inno.Core.Input.GamepadAxisChange) 3d87e5a5de99bb85e22f7e4162d0d641b20401cff2f86e27185c92c4601e0955 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.GetHashCode b94ae4d7b6a5971bd47f0d77cfda291c047f405d96228865dd595e1ad87244ac -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.Equals(System.Object) 41f23a737b3badf1a849491874fdf4ed021c81359600eaab99e763ce6a482b68 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.Equals(Inno.Core.Input.GamepadInputChange) c2894ed3b849af79183551020164e49736e1d71f61958ee3aaa4637058e45ad3 -->
<!-- api: M:Inno.Core.Input.GamepadAxisChange.Equals(Inno.Core.Input.GamepadAxisChange) 98474343a63629fd0316c469b5243c56f9fb3891225bbf6c3f356ce650b2518b -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-90095d2baf29042d"></a>[`public sealed record Inno.Core.Input.GamepadAxisChange : Inno.Core.Input.GamepadInputChange, System.IEquatable<Inno.Core.Input.GamepadAxisChange>`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Captures a normalized axis sample without applying a consumer-specific dead zone. | — |
| <a id="api-5870a1bfb1d0f7f9"></a>[`protected override System.Type Inno.Core.Input.GamepadAxisChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-31d65645a7b9d81c"></a>[`public Inno.Core.Input.GamepadAxisChange.GamepadAxisChange(ulong deviceId, Inno.Core.Input.GamepadAxis axis, float value)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L23) | Captures one finite controller axis value.<br>`deviceId`：Nonzero connection identity.<br>`axis`：Standard axis whose range determines validation.<br>`value`：Stick value in [-1, 1], or trigger value in [0, 1]. | `System.ArgumentOutOfRangeException`：The axis is undefined, non-finite, or outside its normalized range. |
| <a id="api-1d4ad36ae456a1dd"></a>[`public Inno.Core.Input.GamepadAxis Inno.Core.Input.GamepadAxisChange.axis { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L36) | Gets the sampled axis. | — |
| <a id="api-64306ea5083a70f7"></a>[`public float Inno.Core.Input.GamepadAxisChange.value { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L40) | Gets its finite normalized value before dead-zone processing. | — |
| <a id="api-5bf7313b28208ced"></a>[`public override string Inno.Core.Input.GamepadAxisChange.ToString()`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-d6fbb8402c1c95e0"></a>[`protected override bool Inno.Core.Input.GamepadAxisChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-1659bfbafe4f2ae7"></a>[`public static bool Inno.Core.Input.GamepadAxisChange.operator !=(Inno.Core.Input.GamepadAxisChange? left, Inno.Core.Input.GamepadAxisChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | 编译器生成的值不等比较。 | — |
| <a id="api-010a4b4e8fa72a51"></a>[`public static bool Inno.Core.Input.GamepadAxisChange.operator ==(Inno.Core.Input.GamepadAxisChange? left, Inno.Core.Input.GamepadAxisChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | 编译器生成的记录值相等比较。 | — |
| <a id="api-5f33492addc50a10"></a>[`public override int Inno.Core.Input.GamepadAxisChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-4e5ef263ab458701"></a>[`public override bool Inno.Core.Input.GamepadAxisChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-7cc30e82e792cd31"></a>[`public override sealed bool Inno.Core.Input.GamepadAxisChange.Equals(Inno.Core.Input.GamepadInputChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-b664f4cd78e3c348"></a>[`public bool Inno.Core.Input.GamepadAxisChange.Equals(Inno.Core.Input.GamepadAxisChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadAxisChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.GamepadButton`

<!-- api: T:Inno.Core.Input.GamepadButton ee961f0c135d8063262929d8f3c3361273bacdc6e9b26faea41597c3e2c1e455 -->
<!-- api: F:Inno.Core.Input.GamepadButton.South 1a3cd62580c28637f0d1556cec8947b584fdac05ad443ee18ed932ccd8ab8ebe -->
<!-- api: F:Inno.Core.Input.GamepadButton.East e6435e855d6d1e538888eec16cc41e120a3e12734cee96224660596122df1a24 -->
<!-- api: F:Inno.Core.Input.GamepadButton.West 218c89d4482be8f19afd674449443b1606f38e563e25f31f284212c9a1f99619 -->
<!-- api: F:Inno.Core.Input.GamepadButton.North ac36a32553d16ba8a3896dd918ef60ecab76ed4a8d9865205d88d8d35cf97d2e -->
<!-- api: F:Inno.Core.Input.GamepadButton.Back e40b398b00e6d33c997c9dd8699dcbfdfa6a0117438e7484db81d194c3d7fca5 -->
<!-- api: F:Inno.Core.Input.GamepadButton.Guide 590436057b34d11d1445e2b809b77eb09b0111b9866365551ceb016a98e73375 -->
<!-- api: F:Inno.Core.Input.GamepadButton.Start 753e4a3761238d2494cdecdcaa3aebf5c7c4eb8e6f25dbe6c048f938e5b9efbd -->
<!-- api: F:Inno.Core.Input.GamepadButton.LeftStick 9fcd7ec6327527b31a56454249c46b7de2356096031dd87680d04b09ef1b2167 -->
<!-- api: F:Inno.Core.Input.GamepadButton.RightStick 94b6a8d78e47f345dbc496591c6da10117007c13b68042490fd296851df2ef26 -->
<!-- api: F:Inno.Core.Input.GamepadButton.LeftShoulder 1bc31f21e390b7d8ae92dc8412f04021e566a1053843c847182962b7050f70fe -->
<!-- api: F:Inno.Core.Input.GamepadButton.RightShoulder 864ca7abbe7816e47b06223207968077e7291fd556c56f3d62618ad772f6bf3c -->
<!-- api: F:Inno.Core.Input.GamepadButton.DpadUp edabd5b53748f4c26dd7db2281a3574c29b1b2820916ec7b2d214bfd48645d2d -->
<!-- api: F:Inno.Core.Input.GamepadButton.DpadDown 018adde8823418e7c0f3050c972f413d62ca8a5159e26ae21b82563e0ba2b79c -->
<!-- api: F:Inno.Core.Input.GamepadButton.DpadLeft 506e77417badbbd9a498e5b40c8a2661e417b9ca8783d70fca690aa170b09db9 -->
<!-- api: F:Inno.Core.Input.GamepadButton.DpadRight 660d8caabf1348e70a72462f621873525a6aef75f8ee33e8fad27475972f479f -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-be32c0b736fcce92"></a>[`public enum Inno.Core.Input.GamepadButton : int`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L6) | Names standard controller positions independently of printed labels or platform button codes. | — |
| <a id="api-49c91c4fd96c9c8a"></a>[`Inno.Core.Input.GamepadButton.South = 0`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L11) | Bottom face button. | — |
| <a id="api-51fc389d98cc4597"></a>[`Inno.Core.Input.GamepadButton.East = 1`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L15) | Right face button. | — |
| <a id="api-98170f36e8c83683"></a>[`Inno.Core.Input.GamepadButton.West = 2`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L19) | Left face button. | — |
| <a id="api-a3212776af9f5c06"></a>[`Inno.Core.Input.GamepadButton.North = 3`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L23) | Top face button. | — |
| <a id="api-2f3327fe62d6aa1a"></a>[`Inno.Core.Input.GamepadButton.Back = 4`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L27) | Secondary menu button. | — |
| <a id="api-70acb3cad369e335"></a>[`Inno.Core.Input.GamepadButton.Guide = 5`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L31) | System guide button when delivered by the host. | — |
| <a id="api-de2682e9e4ec3ff4"></a>[`Inno.Core.Input.GamepadButton.Start = 6`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L35) | Primary menu button. | — |
| <a id="api-0ff6420e1179a06c"></a>[`Inno.Core.Input.GamepadButton.LeftStick = 7`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L39) | Left stick press. | — |
| <a id="api-ed0c8b931ad3cd8e"></a>[`Inno.Core.Input.GamepadButton.RightStick = 8`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L43) | Right stick press. | — |
| <a id="api-7a80ff1cbdf7dd2e"></a>[`Inno.Core.Input.GamepadButton.LeftShoulder = 9`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L47) | Left shoulder button. | — |
| <a id="api-e27f9f217ef6e62e"></a>[`Inno.Core.Input.GamepadButton.RightShoulder = 10`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L51) | Right shoulder button. | — |
| <a id="api-447c64b593c87f2f"></a>[`Inno.Core.Input.GamepadButton.DpadUp = 11`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L55) | Upward directional button. | — |
| <a id="api-4954f47e2f83609b"></a>[`Inno.Core.Input.GamepadButton.DpadDown = 12`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L59) | Downward directional button. | — |
| <a id="api-e5d6fb8c349a7f70"></a>[`Inno.Core.Input.GamepadButton.DpadLeft = 13`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L63) | Leftward directional button. | — |
| <a id="api-1d3b250311543545"></a>[`Inno.Core.Input.GamepadButton.DpadRight = 14`](../../src/foundation/core/Inno.Core.Input/GamepadButton.cs#L67) | Rightward directional button. | — |

### `Inno.Core.Input.GamepadButtonChange`

<!-- api: T:Inno.Core.Input.GamepadButtonChange 699e8396dd8a400ad25f1f82a56824be65623d443cb55e929367280112e4bd22 -->
<!-- api: P:Inno.Core.Input.GamepadButtonChange.EqualityContract f160d409d22302be28cbd1280b91b0ce61629ee4252e33073666940a34cc8bdb -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.#ctor(System.UInt64,Inno.Core.Input.GamepadButton,System.Boolean) f3a479bd300ac1830d83c4c8ad5691a3ec2827d626edd8385cc392146398a6b6 -->
<!-- api: P:Inno.Core.Input.GamepadButtonChange.button 626bae3ac1ec6a573ab2f08ce3a1afefd808917d67a296f5410a76a0cf1d9eef -->
<!-- api: P:Inno.Core.Input.GamepadButtonChange.pressed a80257642ada1097fe2c42cebbf1fe06fae526e4e7d5784fa0fae5f548e93e4c -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.ToString 88f643ed77d0af8c0cb76c8d963ea2a00f08507a412bb1225eac3b9c1954d35e -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.PrintMembers(System.Text.StringBuilder) 885efa05b35622e3bbd0e9d9c042430377312a18857c13902a982f1e3505b1e4 -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.op_Inequality(Inno.Core.Input.GamepadButtonChange,Inno.Core.Input.GamepadButtonChange) d5e4a33c7f7e4ee84cb4c3f1e5b5426310f665cf16f94165452c6c8920a8691f -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.op_Equality(Inno.Core.Input.GamepadButtonChange,Inno.Core.Input.GamepadButtonChange) 570dbe9d2155f8134436e6c48a3925d95ee9238204062db44dce3939868e9791 -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.GetHashCode e128acb78612242a65c7e0b9355b827f8ed4fd46b68375150e53098a0d075b68 -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.Equals(System.Object) 1abbbf55cffc94528d9f20b5dd25feceab8fc3adadd4cc93faec59786cdbd15d -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.Equals(Inno.Core.Input.GamepadInputChange) 87c324b5eaca9b613794c4c92a24319ae7c6cdf2158d9dd99d7f8e08be57a404 -->
<!-- api: M:Inno.Core.Input.GamepadButtonChange.Equals(Inno.Core.Input.GamepadButtonChange) 379b60aa80c6a41b165ad78f5603d1e07bd5775a95b44ab1d44eee8741fe43f9 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-30f1cfd91d747936"></a>[`public sealed record Inno.Core.Input.GamepadButtonChange : Inno.Core.Input.GamepadInputChange, System.IEquatable<Inno.Core.Input.GamepadButtonChange>`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Captures a physical controller button transition without manufacturing keyboard events or repeats. | — |
| <a id="api-d40aed87125f3e1d"></a>[`protected override System.Type Inno.Core.Input.GamepadButtonChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-92e71a623db6a6cb"></a>[`public Inno.Core.Input.GamepadButtonChange.GamepadButtonChange(ulong deviceId, Inno.Core.Input.GamepadButton button, bool pressed)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L25) | Captures one standard controller button edge.<br>`deviceId`：Nonzero connection identity.<br>`button`：Standard physical button position.<br>`pressed`：True for press, false for release. | `System.ArgumentOutOfRangeException`：The button is undefined. |
| <a id="api-d0746d4b889cf929"></a>[`public Inno.Core.Input.GamepadButton Inno.Core.Input.GamepadButtonChange.button { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L39) | Gets the physical button position. | — |
| <a id="api-ea21aa83dc67a0b5"></a>[`public bool Inno.Core.Input.GamepadButtonChange.pressed { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L43) | Gets whether the button became held. | — |
| <a id="api-54274a5db36b16c8"></a>[`public override string Inno.Core.Input.GamepadButtonChange.ToString()`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-c2cdb502638da30b"></a>[`protected override bool Inno.Core.Input.GamepadButtonChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-0112e664e965607f"></a>[`public static bool Inno.Core.Input.GamepadButtonChange.operator !=(Inno.Core.Input.GamepadButtonChange? left, Inno.Core.Input.GamepadButtonChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-992d8e711215010d"></a>[`public static bool Inno.Core.Input.GamepadButtonChange.operator ==(Inno.Core.Input.GamepadButtonChange? left, Inno.Core.Input.GamepadButtonChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-2db32180eb5781bd"></a>[`public override int Inno.Core.Input.GamepadButtonChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-348f020d830e5699"></a>[`public override bool Inno.Core.Input.GamepadButtonChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-f055ed5fc9835c66"></a>[`public override sealed bool Inno.Core.Input.GamepadButtonChange.Equals(Inno.Core.Input.GamepadInputChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-342dbd3dd6f7d904"></a>[`public bool Inno.Core.Input.GamepadButtonChange.Equals(Inno.Core.Input.GamepadButtonChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadButtonChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.GamepadConnectionChange`

<!-- api: T:Inno.Core.Input.GamepadConnectionChange 32b645235662d3c4b43dd89b298b82068a879da60c9189317fddbfac221b38ab -->
<!-- api: P:Inno.Core.Input.GamepadConnectionChange.EqualityContract 41f78f97d0c7422a5bc1a957cf184db9db2eb032fa22b5efcfbe581f96366e32 -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.#ctor(System.UInt64,System.Boolean) f4de4a744455c8f700e2624baa9eb7d120db5a9d745d426736b30929fcae6cc5 -->
<!-- api: P:Inno.Core.Input.GamepadConnectionChange.connected 1c007fed84e4f0ac0fff850fc9e8bbcf4726c0184f9e49e231ba98b7d56ed112 -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.ToString 368d815a5235446ccf76c88c3ae92651fcb84188bb29562789aaab83f4424cb5 -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.PrintMembers(System.Text.StringBuilder) b09462a9326333e9d209eda956fdc481449e729233241cf5d9eda44da841930d -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.op_Inequality(Inno.Core.Input.GamepadConnectionChange,Inno.Core.Input.GamepadConnectionChange) 3b593019b78764a310c28d024994262960e78d4bb3eb3c6ade6eff2801ee03ff -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.op_Equality(Inno.Core.Input.GamepadConnectionChange,Inno.Core.Input.GamepadConnectionChange) e8bf2d8dd7a92d2fef85dc33ff766db7be414afda4e364a7c01b1b2f3ad1c85b -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.GetHashCode 5570e16e9dc9bf6f26342c6b2334b92b2d1a433a0dc989d3cd000b54d78a8779 -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.Equals(System.Object) 58b4fb01f19ccb4374e92fce158b69eafc30ca44aab948fdbe685d7e4aa06ccf -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.Equals(Inno.Core.Input.GamepadInputChange) ed0c80c1b29b6e46a56abc38fc5f69ec13ab01ec2d4afa3b70146c5acf8dce6d -->
<!-- api: M:Inno.Core.Input.GamepadConnectionChange.Equals(Inno.Core.Input.GamepadConnectionChange) 2f833c4208123188ddb45f31708327d3188956b9cf4bcbce81a02c65165440b8 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-bfb7e809f1c06ecb"></a>[`public sealed record Inno.Core.Input.GamepadConnectionChange : Inno.Core.Input.GamepadInputChange, System.IEquatable<Inno.Core.Input.GamepadConnectionChange>`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Announces a controller connection or retires all held state for that connection. | — |
| <a id="api-4089363dd048b139"></a>[`protected override System.Type Inno.Core.Input.GamepadConnectionChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-db2e2a3bd25b6f7a"></a>[`public Inno.Core.Input.GamepadConnectionChange.GamepadConnectionChange(ulong deviceId, bool connected)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L17) | Captures an ordered device lifetime transition.<br>`deviceId`：Nonzero connection identity.<br>`connected`：True for attachment; false when the connection and all held controls retire. | — |
| <a id="api-16bd4b5d50415ade"></a>[`public bool Inno.Core.Input.GamepadConnectionChange.connected { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L25) | Gets whether this transition establishes a live connection. | — |
| <a id="api-1c9fe81efbe2a6f2"></a>[`public override string Inno.Core.Input.GamepadConnectionChange.ToString()`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-5f4175ad9bb88a5b"></a>[`protected override bool Inno.Core.Input.GamepadConnectionChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-36b8daaace26634d"></a>[`public static bool Inno.Core.Input.GamepadConnectionChange.operator !=(Inno.Core.Input.GamepadConnectionChange? left, Inno.Core.Input.GamepadConnectionChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | 编译器生成的值不等比较。 | — |
| <a id="api-031465787ebcaa2f"></a>[`public static bool Inno.Core.Input.GamepadConnectionChange.operator ==(Inno.Core.Input.GamepadConnectionChange? left, Inno.Core.Input.GamepadConnectionChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | 编译器生成的记录值相等比较。 | — |
| <a id="api-2e7b6975ccb8b47c"></a>[`public override int Inno.Core.Input.GamepadConnectionChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-26317126e00ab7b6"></a>[`public override bool Inno.Core.Input.GamepadConnectionChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-aaca47ae09d6ec93"></a>[`public override sealed bool Inno.Core.Input.GamepadConnectionChange.Equals(Inno.Core.Input.GamepadInputChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-4878aee9d9e04c7c"></a>[`public bool Inno.Core.Input.GamepadConnectionChange.Equals(Inno.Core.Input.GamepadConnectionChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadConnectionChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.GamepadInputChange`

<!-- api: T:Inno.Core.Input.GamepadInputChange 9074ac5c874a564b0b409e9d75929b5eec06256f844068faeef1cc1531641491 -->
<!-- api: P:Inno.Core.Input.GamepadInputChange.EqualityContract 96a1a33397212866441090d853fbb46a47760a270be717c9dfbc9e23678e0bea -->
<!-- api: P:Inno.Core.Input.GamepadInputChange.deviceId fd847112184c8aff72107773ce6a1c63d24bb92334c441b5ab05cf24e15399af -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.ToString 135ae22ecf64bc98c7d1244298a8c4d74000884431e4a0fe03c11d8b5b34c20d -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.PrintMembers(System.Text.StringBuilder) 37325ade9bd6f0d36f3e08cd46e5c3d2dcc78974b121ebb7a751b8c4a82d0089 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.op_Inequality(Inno.Core.Input.GamepadInputChange,Inno.Core.Input.GamepadInputChange) c9804592bda1409726895286c682ad30d660a0d0a494966c3d88b4bece5b636e -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.op_Equality(Inno.Core.Input.GamepadInputChange,Inno.Core.Input.GamepadInputChange) dbda83b609b04debf9d8f058cc30f6dc4e4984f2582b318a356106b823b8b602 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.GetHashCode 73d215494057d63ab496b0c29ca9a82cc6b32c82be81c35c9ac2d07d23828608 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.Equals(System.Object) 8e018390509652950b215344210d2227a19e2efc1845ac03d4f28a3e6562e610 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.Equals(Inno.Core.Input.InputChange) 5c6b8fa7fc1a69cc8272192f259a3c5a96256420dfc62a897f573420c274be95 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.Equals(Inno.Core.Input.GamepadInputChange) 6c5f8f0c3226dcd06f12aa0ecd835d040611c54aae8daf8fadd3e5e715213707 -->
<!-- api: M:Inno.Core.Input.GamepadInputChange.#ctor(Inno.Core.Input.GamepadInputChange) 01c160d4adf62bf595438a7563f3812a14db10f8ae65bae8429dd6e50e489d1f -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-69f7fdbaa23feb20"></a>[`public abstract record Inno.Core.Input.GamepadInputChange : Inno.Core.Input.InputChange, System.IEquatable<Inno.Core.Input.GamepadInputChange>`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Identifies an ordered controller fact by its connection-lifetime identity, never a native pointer. | — |
| <a id="api-620165f49430a5bd"></a>[`protected override System.Type Inno.Core.Input.GamepadInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-0fa19d4f806640d1"></a>[`public ulong Inno.Core.Input.GamepadInputChange.deviceId { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L25) | Gets the connection identity; it must not be persisted as a user device preference. | — |
| <a id="api-28f02323480a60bd"></a>[`public override string Inno.Core.Input.GamepadInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-f0c1e82de77e19b8"></a>[`protected override bool Inno.Core.Input.GamepadInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-bf76631ab57764bf"></a>[`public static bool Inno.Core.Input.GamepadInputChange.operator !=(Inno.Core.Input.GamepadInputChange? left, Inno.Core.Input.GamepadInputChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-ccafd847e921e760"></a>[`public static bool Inno.Core.Input.GamepadInputChange.operator ==(Inno.Core.Input.GamepadInputChange? left, Inno.Core.Input.GamepadInputChange? right)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-40d6612cca3944f1"></a>[`public override int Inno.Core.Input.GamepadInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-c4760b34345ac078"></a>[`public override bool Inno.Core.Input.GamepadInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-88f6b21df2a47e59"></a>[`public override sealed bool Inno.Core.Input.GamepadInputChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-5a5329956b27fdb2"></a>[`public virtual bool Inno.Core.Input.GamepadInputChange.Equals(Inno.Core.Input.GamepadInputChange? other)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-aae358b5f5416277"></a>[`protected Inno.Core.Input.GamepadInputChange.GamepadInputChange(Inno.Core.Input.GamepadInputChange original)`](../../src/foundation/core/Inno.Core.Input/GamepadInputChange.cs#L8) | 编译器生成的记录构造入口，参数对应左列声明的成员。 | — |

### `Inno.Core.Input.GamepadState`

<!-- api: T:Inno.Core.Input.GamepadState 458bb2299a5eff1b7a674daed19b5ea9cbc3d911b36eb50e360cf7da13fe07f9 -->
<!-- api: M:Inno.Core.Input.GamepadState.#ctor(System.UInt64,System.Collections.Generic.IEnumerable{Inno.Core.Input.GamepadButton},System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Input.GamepadAxis,System.Single}) 7de71e14897590be840e6d68868610dd522e884c709a098768faa79ce86e3446 -->
<!-- api: P:Inno.Core.Input.GamepadState.deviceId 4c4276dbc00a3f8828028fc84e5dda60cf1da4cd05ae45019edebe31a2b4711e -->
<!-- api: P:Inno.Core.Input.GamepadState.buttonsDown 1594e19b6a9a6b1e67f29522ddb148ab1203734842d82344fbe4beeeb9533d32 -->
<!-- api: P:Inno.Core.Input.GamepadState.axes bd76625896a72ec300f58d95f41cc084e15de9442775a8db285d392eb9678dd9 -->
<!-- api: M:Inno.Core.Input.GamepadState.GetAxis(Inno.Core.Input.GamepadAxis,System.Single) 654c8b34bee44dfdaa8213ef96080dcebd1f1f55ceb1a20c47ec1bbface4785d -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-0ed48d6cc1f1196f"></a>[`public sealed class Inno.Core.Input.GamepadState`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L10) | Freezes the held controls of one live controller connection at an input boundary. | — |
| <a id="api-097f9fb51e9d94a5"></a>[`public Inno.Core.Input.GamepadState.GamepadState(ulong deviceId, System.Collections.Generic.IEnumerable<Inno.Core.Input.GamepadButton>? buttonsDown = null, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Input.GamepadAxis, float>? axes = null)`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L27) | Copies held controls without retaining mutable collections supplied by a backend.<br>`deviceId`：Nonzero connection-lifetime identity.<br>`buttonsDown`：Held standard buttons, or null for none.<br>`axes`：Finite normalized samples, or null for centered axes. | `System.ArgumentOutOfRangeException`：The identity is zero or a control/value is invalid. |
| <a id="api-a9e63bbcbb96e2e9"></a>[`public ulong Inno.Core.Input.GamepadState.deviceId { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L46) | Gets the transient connection identity. | — |
| <a id="api-0acab34929099fb9"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Core.Input.GamepadButton> Inno.Core.Input.GamepadState.buttonsDown { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L50) | Gets immutable held buttons; edges remain in the ordered input sequence. | — |
| <a id="api-7ce0f7b9ea6b5ccd"></a>[`public System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Input.GamepadAxis, float> Inno.Core.Input.GamepadState.axes { get; }`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L54) | Gets immutable raw normalized samples; missing axes read as zero. | — |
| <a id="api-3a59058afae8007a"></a>[`public float Inno.Core.Input.GamepadState.GetAxis(Inno.Core.Input.GamepadAxis axis, float deadZone = 0)`](../../src/foundation/core/Inno.Core.Input/GamepadState.cs#L71) | Applies an axial dead zone and rescales the remaining travel continuously to full range.<br>`axis`：Standard axis to query.<br>`deadZone`：Finite dead-zone radius in [0, 1); zero returns the raw normalized sample.<br>返回：Zero within the dead zone or for an unsampled axis; otherwise normalized remaining travel. | `System.ArgumentOutOfRangeException`：The axis or dead zone is invalid. |

### `Inno.Core.Input.InputChange`

<!-- api: T:Inno.Core.Input.InputChange 84b6ae3206dc4b33212169adc7bddeef120c54f4eaa439e2ee31d934f2fcc22c -->
<!-- api: P:Inno.Core.Input.InputChange.EqualityContract 3911b310145ac9209f3ff77d51e7c2dc025954368c4fb386d8136f4deffd4d68 -->
<!-- api: P:Inno.Core.Input.InputChange.modifiers 81d16f52a395e953e411fe3a7b08f757fbb221675b77cbb6ea12ef0353e7b136 -->
<!-- api: M:Inno.Core.Input.InputChange.ToString 5a11fd4e5d30470b5b5f4d053b263c2cebb66ec229ae8c43bf66ba6773e3fbf8 -->
<!-- api: M:Inno.Core.Input.InputChange.PrintMembers(System.Text.StringBuilder) a97d474a1b7365ea451b6ae00b9354ab6781fccaac52b89045ab9b8f6d4cf975 -->
<!-- api: M:Inno.Core.Input.InputChange.op_Inequality(Inno.Core.Input.InputChange,Inno.Core.Input.InputChange) 11b667af5d85500e98128acb8c4a2dcaa2bf6af3679e4322043e20d5d632f8d3 -->
<!-- api: M:Inno.Core.Input.InputChange.op_Equality(Inno.Core.Input.InputChange,Inno.Core.Input.InputChange) 2f4e65684d8b01e95cc99ea4fc667dbffbfd8d7529bae1789c8cb547db932e4a -->
<!-- api: M:Inno.Core.Input.InputChange.GetHashCode eb018c0fe882755e3e3c8225686a0dfc50acc521f5b9384f386e49c05d0729e4 -->
<!-- api: M:Inno.Core.Input.InputChange.Equals(System.Object) 67101bebdac8862c6ecd1be66db8365d762eae88caf8394305bc650ba8b81a1a -->
<!-- api: M:Inno.Core.Input.InputChange.Equals(Inno.Core.Input.InputChange) 5032d8753db9fb92db52bdd31f2c5b804c600316b30f88f3afdefcabb9d32f67 -->
<!-- api: M:Inno.Core.Input.InputChange.#ctor(Inno.Core.Input.InputChange) 9d1da2fab9cbac4f284774ff10b27f600c9bb048a74c9e23f6d2f13b87f17ba5 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-e1d06ff465225e18"></a>[`public abstract record Inno.Core.Input.InputChange : System.IEquatable<Inno.Core.Input.InputChange>`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | Describes one immutable input fact in acquisition order, without mutable event-dispatch state. | — |
| <a id="api-8bceedbbab032771"></a>[`protected virtual System.Type Inno.Core.Input.InputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-b8d170c7a76e969a"></a>[`public Inno.Core.Input.KeyModifier Inno.Core.Input.InputChange.modifiers { get; }`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L19) | Gets the physical modifiers observed at this change, not at the end of the frame. | — |
| <a id="api-a7c105f5a95fb2d6"></a>[`public override string Inno.Core.Input.InputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-65d03f680ea08287"></a>[`protected virtual bool Inno.Core.Input.InputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-49ae2544e80eeff5"></a>[`public static bool Inno.Core.Input.InputChange.operator !=(Inno.Core.Input.InputChange? left, Inno.Core.Input.InputChange? right)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | 编译器生成的值不等比较。 | — |
| <a id="api-24f423598e37ef05"></a>[`public static bool Inno.Core.Input.InputChange.operator ==(Inno.Core.Input.InputChange? left, Inno.Core.Input.InputChange? right)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | 编译器生成的记录值相等比较。 | — |
| <a id="api-e23952dedf0e27c1"></a>[`public override int Inno.Core.Input.InputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-bb15961708bcb263"></a>[`public override bool Inno.Core.Input.InputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-f4a61a9ba314ee7a"></a>[`public virtual bool Inno.Core.Input.InputChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-434c8584ad2df16d"></a>[`protected Inno.Core.Input.InputChange.InputChange(Inno.Core.Input.InputChange original)`](../../src/foundation/core/Inno.Core.Input/InputChange.cs#L6) | 编译器生成的记录构造入口，参数对应左列声明的成员。 | — |

### `Inno.Core.Input.InputSequence`

<!-- api: T:Inno.Core.Input.InputSequence 64cf2231891427a483e738a132ff235c17afede89a15fe681763bbf7d6c774c0 -->
<!-- api: M:Inno.Core.Input.InputSequence.#ctor(System.Collections.Generic.IEnumerable{Inno.Core.Input.InputChange}) 9188e19d14eff597af3f7423326e59fcd93f66c2196459d542c1f62c183c78bc -->
<!-- api: P:Inno.Core.Input.InputSequence.empty c7f8f35d4971125cf44bd1ce0feb85d5bc5ace4523ab7c75db2c6f0f6d29862d -->
<!-- api: P:Inno.Core.Input.InputSequence.changes 7c512319dafeb930c9c23e03135566a1983e95445b7bfd866c38e680bf7ce83e -->
<!-- api: P:Inno.Core.Input.InputSequence.keysPressed 72b1b7784f36a14c2d0102e4f0a2caee8d97227c543da7793c5579e04afaa90a -->
<!-- api: P:Inno.Core.Input.InputSequence.keysReleased 3f195e2df82c86021e5d2c4269cbe4a23c863c7ee40800f6781fd87526ed024c -->
<!-- api: P:Inno.Core.Input.InputSequence.buttonsPressed 3988ce0332ceb1074f419e1b46567bff7a05da4a7c28910fd5d4e830d399264e -->
<!-- api: P:Inno.Core.Input.InputSequence.buttonsReleased 951dc5f14fba5dd3436cb5bbfbf8cea2bb0a73a38d3d7d93130893d7c28974dd -->
<!-- api: P:Inno.Core.Input.InputSequence.textInput 950d4ea76439615b5e9321872fa41b3674e4fca6b326f40407b9bfe5cd170cae -->
<!-- api: P:Inno.Core.Input.InputSequence.hasTextCompositionActivity 7122f4f192eac643f5e87d6dabf4eb2c257570c02af96c0250243bcb56efd806 -->
<!-- api: P:Inno.Core.Input.InputSequence.mouseDelta 299219821218db04dd80564f4c2ccc1289bba2fd18d28249a7b88636f85417d0 -->
<!-- api: P:Inno.Core.Input.InputSequence.scrollDelta 560df4b0884531b201154ce7e48adb336b918fe860bb217dc535b90687bfdfdc -->
<!-- api: M:Inno.Core.Input.InputSequence.MapPointer(System.Func{Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2}) 1245c521414041e5a9fba47369e0b3411085e3ab98ad28063a00cf451cc89e84 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-f30746df092436d8"></a>[`public sealed class Inno.Core.Input.InputSequence`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L16) | Owns immutable ordered input facts and derives convenient frame summaries from that same sequence. | — |
| <a id="api-ed8d2c7b983e05b1"></a>[`public Inno.Core.Input.InputSequence.InputSequence(System.Collections.Generic.IEnumerable<Inno.Core.Input.InputChange> changes)`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L27) | Creates a frozen sequence in acquisition order.<br>`changes`：Non-null immutable input facts in their original order. | `System.ArgumentNullException`：The sequence or a contained change is null. |
| <a id="api-05e1c5d71a95a459"></a>[`public static Inno.Core.Input.InputSequence Inno.Core.Input.InputSequence.empty { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L83) | Gets the reusable sequence containing no input changes. | — |
| <a id="api-38e05a0120f6ea2d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Input.InputChange> Inno.Core.Input.InputSequence.changes { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L87) | Gets all changes in their original order, including repeated keys. | — |
| <a id="api-f34486bd5bd6296b"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Core.Input.KeyCode> Inno.Core.Input.InputSequence.keysPressed { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L91) | Gets non-repeated physical presses present in this sequence. | — |
| <a id="api-c3677395d3ba65c7"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Core.Input.KeyCode> Inno.Core.Input.InputSequence.keysReleased { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L95) | Gets physical key releases present in this sequence. | — |
| <a id="api-60fd717127bee86a"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Core.Input.MouseButton> Inno.Core.Input.InputSequence.buttonsPressed { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L99) | Gets physical pointer presses present in this sequence. | — |
| <a id="api-17ff1f00ee33afa4"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Core.Input.MouseButton> Inno.Core.Input.InputSequence.buttonsReleased { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L103) | Gets physical pointer releases present in this sequence. | — |
| <a id="api-dd0551b27a90f5dc"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Core.Input.InputSequence.textInput { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L107) | Gets committed text in acquisition order. | — |
| <a id="api-ac8aac8b6b9671a6"></a>[`public bool Inno.Core.Input.InputSequence.hasTextCompositionActivity { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L114) | Gets whether this sample contains composition activity, including a final commit or cancellation. | — |
| <a id="api-6ed82ced64ee5494"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Core.Input.InputSequence.mouseDelta { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L118) | Gets the sum of pointer movements in the sequence's coordinate space. | — |
| <a id="api-121511993031b7bb"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Core.Input.InputSequence.scrollDelta { get; }`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L122) | Gets accumulated wheel movement, independent of coordinate density. | — |
| <a id="api-4410ba73e345c508"></a>[`public Inno.Core.Input.InputSequence Inno.Core.Input.InputSequence.MapPointer(System.Func<Inno.Core.Mathematics.Vector2, Inno.Core.Mathematics.Vector2> map)`](../../src/foundation/core/Inno.Core.Input/InputSequence.cs#L139) | Maps pointer positions without reordering keys, text or pointer transitions.<br>`map`：A synchronous mapping in the caller's owned viewport; it is not retained.<br>返回：A new coordinate-mapped sequence, or this sequence when it contains no pointer facts. | `System.ArgumentNullException`：The mapping is null.<br>`System.ArgumentOutOfRangeException`：The mapping returns a non-finite position. |

### `Inno.Core.Input.KeyCode`

<!-- api: T:Inno.Core.Input.KeyCode 251a3164af6ca6294fe67d77a2131c26da870657157f75b556efb057a961aca5 -->
<!-- api: F:Inno.Core.Input.KeyCode.Unknown b206cdb54340239bc795a451f2162d7994bd3babbd983b486367dabf2a7742bc -->
<!-- api: F:Inno.Core.Input.KeyCode.A 2bd02cebcb678125172ace470d531ff37642cb7945dae97bbd96906d76ba7dbf -->
<!-- api: F:Inno.Core.Input.KeyCode.B 8cb5b34a037730e33ecb693c88870d9258e3af9b779f814d056d162a913f5b09 -->
<!-- api: F:Inno.Core.Input.KeyCode.C 40525129e5bb1ba695aae0cebe0f146238dd7f4c68c53a56d6eb29d76f7ffb27 -->
<!-- api: F:Inno.Core.Input.KeyCode.D a23e2d507f0c92ecdd443ab4e9bfc9cb4676fdbbf94c9a2f8452e60fc3113752 -->
<!-- api: F:Inno.Core.Input.KeyCode.E 6767ad8de3a438a28c74f9193445f6d613319538d362c2f40a0b2668229a2e16 -->
<!-- api: F:Inno.Core.Input.KeyCode.F c91bbf7814ad980034cd73fd6d9333fbab5b8348450f8eefb9f6f2c2cf07a0c2 -->
<!-- api: F:Inno.Core.Input.KeyCode.G 3993b4d87b0a933194ecee91e727ce176071211d4359dd5eb12391dce77f318b -->
<!-- api: F:Inno.Core.Input.KeyCode.H bbc7511c8a882dfe219b5ea557965e6a8511ea31daa9799432acfe34574217be -->
<!-- api: F:Inno.Core.Input.KeyCode.I 6fd6d93904895f83e0240d00c2995f1c9fbaf668c781724f65f2165186364c99 -->
<!-- api: F:Inno.Core.Input.KeyCode.J 1e57aba2c32436ced81c640b1ed15cb6e4f204d885f3709949e5d88ead86de54 -->
<!-- api: F:Inno.Core.Input.KeyCode.K f17b2847c80f11c35734f883098102d474e6d708f479a8238ca6dadb1782dce9 -->
<!-- api: F:Inno.Core.Input.KeyCode.L 065fbcb35b13a34f68d07fdf61b1e6bb044664027d37b8d28aa565b7bedede1b -->
<!-- api: F:Inno.Core.Input.KeyCode.M 4343472fd518839e4350f76bebd4e1452d24a818787c0ca24e72a02de2a0dbbd -->
<!-- api: F:Inno.Core.Input.KeyCode.N 055a55e666a09e880120e200e27d6ece2309dd5d6dbbc925957c38029918d62d -->
<!-- api: F:Inno.Core.Input.KeyCode.O e094fa686715756033f38e18571ba2877a0ae513d52b2a66f99c973b7901cd7b -->
<!-- api: F:Inno.Core.Input.KeyCode.P a577771e6ed568824f894c1f8be05fab29285b51b016c559b5d79c4094af11f1 -->
<!-- api: F:Inno.Core.Input.KeyCode.Q de3661f9ed336b9fc595e0fcf376d1882a6010aff0aea031d7f942a504bff70d -->
<!-- api: F:Inno.Core.Input.KeyCode.R 3d04ca06be5753a93b941683a513f05b6cc379a60669be0a7815a8b90b5a8be9 -->
<!-- api: F:Inno.Core.Input.KeyCode.S 1d4bf0bceb89186197a85767bea4f224a800531a0c8783546194a5e169448048 -->
<!-- api: F:Inno.Core.Input.KeyCode.T 03f976a5807242df66b7a4f5b129a5fe25d79cf66a943c7292b5a658586d8825 -->
<!-- api: F:Inno.Core.Input.KeyCode.U a688a0203ecdf11afbe96d9e53e11d829d98cf602f432f83c58e11d0097cc0ea -->
<!-- api: F:Inno.Core.Input.KeyCode.V 45fbfd771fbec49cac64ebdf16c74fde1887a28992876e37c8b782e71d9da45f -->
<!-- api: F:Inno.Core.Input.KeyCode.W 09ef816690c87ce6c3b3fcb0cf22c3eda24ad825e24eb89311133b33d7006887 -->
<!-- api: F:Inno.Core.Input.KeyCode.X b43dedf091372fa4eb2b7d487ce5ee080ec80765cef8d9199cc94fc4c69c0c6d -->
<!-- api: F:Inno.Core.Input.KeyCode.Y d916be136596d5658d49108c6ef99fd879b2de6af780920b4109abcf15ad10bd -->
<!-- api: F:Inno.Core.Input.KeyCode.Z 0db2e8be49b5462768116e0168f42b3c6fe1989d2a3fafd1618deb1d070a1cd5 -->
<!-- api: F:Inno.Core.Input.KeyCode.D0 aab1ec5fd3eba744c1df13707e558a356cca4e597ae17d80b581f1b9d18ec6b5 -->
<!-- api: F:Inno.Core.Input.KeyCode.D1 de7c0375ab865a825b0a60d8ba4e06faa7ad2591924bc6338d4a172a8e1c8650 -->
<!-- api: F:Inno.Core.Input.KeyCode.D2 634da4cb16811b576b08319e8a5d1c0918ad0fa9492733b991389440ee4e3e8f -->
<!-- api: F:Inno.Core.Input.KeyCode.D3 58bd2233af8d221e879104b65607bc3804820529978f4c5b066a9c0091a42917 -->
<!-- api: F:Inno.Core.Input.KeyCode.D4 b8d073f99929e47c13a5c06f56d713b1f02c6956370a07ac70d074594f40cb4e -->
<!-- api: F:Inno.Core.Input.KeyCode.D5 0a8c2730f205c758842b38c4a57ca028a62c3eed65494b8e4a1c6d33bf7d64e8 -->
<!-- api: F:Inno.Core.Input.KeyCode.D6 7893ee471764a6f4ac99ecef3a1d91a13aaa80d90a8dc3691231c148899bd647 -->
<!-- api: F:Inno.Core.Input.KeyCode.D7 f6d27c2fc82a75628edee55548eecb910175e9bf512327244bcddb55ef8f1ef1 -->
<!-- api: F:Inno.Core.Input.KeyCode.D8 0badc2b0b2880b701f2a49b3e718a261ec0927008065b1a335b59d546af87dfb -->
<!-- api: F:Inno.Core.Input.KeyCode.D9 4e28553a39ca4915ac379ee7f5546241b84a0ba3c198fbb72cafde463d98d70b -->
<!-- api: F:Inno.Core.Input.KeyCode.Escape 3b29e9665f90378ad12bccb4956ae4fc8addcb98769a8c16938bc08bea283abe -->
<!-- api: F:Inno.Core.Input.KeyCode.Space 8f39ae52aadc92f1863912709750cb92f48562df180d18647c1d9972a20c5499 -->
<!-- api: F:Inno.Core.Input.KeyCode.Enter 92d809c5d7664bbe49f8180a1b5982bf1b730ccf13f79583bea3cb4bc3627255 -->
<!-- api: F:Inno.Core.Input.KeyCode.Tab 561106f99aba9c92212a00857bbbe095ca1f484fff733ebfa917f0d9828f0ce9 -->
<!-- api: F:Inno.Core.Input.KeyCode.Backspace 976636da12c62c0659ba3bfe0b4f2dd82f2a1ed853cdfc2390b407b5a817a8e2 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftArrow 5f8cda4659f790bfeb4f5f3b432cbf2f235943e1c9c9b1c9a0d5e21c6710320c -->
<!-- api: F:Inno.Core.Input.KeyCode.UpArrow ad0ff3f65854f59d06bd6870574716511dd86a80e4df8237b75f2e61e50cd700 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightArrow 5cf533c89627cb25a733935868bfc76726e92c79fe8a4529801434bd907a8a2e -->
<!-- api: F:Inno.Core.Input.KeyCode.DownArrow b1fa12a57702f20c31672ca62c3d187e3e8ec06acbf49659209ff1601175b00e -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftSuper 2a2510931ffe70390babb9d27c42cd8d0c15009785ff2cfd47abe1b934d72ee5 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightSuper 253aaec6dcbcb47c8207730ad4fd8fbb5016fce62028e7ab7fff5dbdded42a15 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftShift 21836ef75b76798c4072df63c999cd4a138e379c9aa44d30110b8642ac448329 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightShift c7546df26faceb1ffb1c9252e393d4f5ddfb32f7ee3624c7fa60febc0b7c10c6 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftCtrl 0e3f75bd6c5e6badf2e2a62bb68632270bbe42b6af2d9b03fca9e66e8b828946 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightCtrl 7ab6af4f2de5cd3bc53750ed493fbb31637a54340b41b249664a73d740f6e01e -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftAlt 2b2bf0a91816a295fbc258a9705d8cb968ead3546544cf010bc583c0b15cc35a -->
<!-- api: F:Inno.Core.Input.KeyCode.RightAlt 90cbafd9eaafa5665d1d88f127236aa478b4a428a28f0b899df867101c9bdde4 -->
<!-- api: F:Inno.Core.Input.KeyCode.CapsLock 884eb8c992a01da7edcf14d99fc22ee61eab3cdcf40680df504ed3d3e6f1d65f -->
<!-- api: F:Inno.Core.Input.KeyCode.Insert 7b8d81d574f724149c6e84e30c8c1af3fa62ba7b3c47982470a81af2a8cb6cf2 -->
<!-- api: F:Inno.Core.Input.KeyCode.Delete e4fcf91afd85c3313efff33ae432f99553b13e5d37072fbc7004a402cae7502e -->
<!-- api: F:Inno.Core.Input.KeyCode.Home d1b44c38dadef31b310e22e9138a4354bafdb7a7b3515b19991ced4d926ae3a5 -->
<!-- api: F:Inno.Core.Input.KeyCode.End 1ffe51ec08d0f2006962db5f739f5847accf37838e8cc040f346bdb20c41ee7e -->
<!-- api: F:Inno.Core.Input.KeyCode.PageUp 9751d866bb294ac4a8a375e1605c3828a73824e538099adf19ebc82488aeb537 -->
<!-- api: F:Inno.Core.Input.KeyCode.PageDown d26ca27403abb8fe2dae8cc2757c9fab9d0b811f935bc45ffe82d621a3abc092 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad0 cc28f573f845046e50aaf3a1c811d59f1c1fc776957c229d2244a6f2436fc57b -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad1 5e15554db1aa1896662142fc3d6677a324c4f56c8dd94a30030defc2d6f602e5 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad2 b3d0dd9fe4a7ecd8a10680e64a600d6083a4a6bfb74e0056fe2902524b770bea -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad3 79ecdc3a85833d6fba23d772359235a1d1c88cf2ae03c6cf33cfb4e6c334b480 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad4 265e3bc34b91d17247f49b92791edddadebd1bb72edfe83881fcc272fd87cb72 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad5 b3ce72fcd5f593720749b3b9907361b92ca4dbc5420c88e9a409a6374416ca1e -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad6 7b1ac62df91fab6c896bcd89207a6f5f2f6ffae11add6130667ee03a999c52e8 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad7 0a826073347b1a8eb19d2c31da0a11f82d0ec5384891b9f371b8eeee26a9457a -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad8 bc15c7962c217e8284910012a01d14dc3982c6f9a605b23e18a8ec10d84c4b30 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad9 4dc9672f11ab4442ee6ec77926c535a7a04c567da227b6f1a54307dd4207877e -->
<!-- api: F:Inno.Core.Input.KeyCode.NumLock 9d8618f86435925315fc2126913ac99b75523d8df0e9a09d655cb62193c2d5d7 -->
<!-- api: F:Inno.Core.Input.KeyCode.ScrollLock f9f1f93c214c4419a103cf693cb30960118f15cda8090f7f459f7e807998b636 -->
<!-- api: F:Inno.Core.Input.KeyCode.F1 998746c5c65601ad28ca7f987ade0041d2c6e5eb77d96f99c37c34f271b027e7 -->
<!-- api: F:Inno.Core.Input.KeyCode.F2 4855f670aa07d7de8fc48f5416ba4f1207dc0f122bf50e680d408a679b3d4989 -->
<!-- api: F:Inno.Core.Input.KeyCode.F3 ad142e08f4fe1267af925d5a447453d0f632b030f87c132f56b730a82a59b9e4 -->
<!-- api: F:Inno.Core.Input.KeyCode.F4 0e44bfdced9844d7a9103b570e56ca303fd9ddd4bc22a2e202cfbdea0a9b72e4 -->
<!-- api: F:Inno.Core.Input.KeyCode.F5 eadad76cf354c289ccd5ef12bed886b5497ab090ce425400220bd8dc52ec01e0 -->
<!-- api: F:Inno.Core.Input.KeyCode.F6 2a11b9add8f5495d44a559ad424e6da3e0097d57847054f0cb91e3a9804e8e5f -->
<!-- api: F:Inno.Core.Input.KeyCode.F7 c6ba3e8ca993c40d8831687bef02d6e1e631ae424e6ab968ad64d0ad1dc0a305 -->
<!-- api: F:Inno.Core.Input.KeyCode.F8 9e59ad53490005cd94809cf578c7d5f196e68b2ae59e16545b83a2ecd60c7d20 -->
<!-- api: F:Inno.Core.Input.KeyCode.F9 625fd7b68e7bc1e4acdde3fbd2faab6860e4a2ab2d947c2496e2d0f23fa25f75 -->
<!-- api: F:Inno.Core.Input.KeyCode.F10 9ee0cb8481da32ebfb22d71722df4d47c6881f0cbee8d7b87865ad5abd81500c -->
<!-- api: F:Inno.Core.Input.KeyCode.F11 74cba16a9c715bb7bce4023c62cac5ae6dccb0c16aa38ccb68fa7faa67351faa -->
<!-- api: F:Inno.Core.Input.KeyCode.F12 abb1d48823dd610647ed4c187ccb234f4794d88f8d12ea109981b191b98cf2fa -->
<!-- api: F:Inno.Core.Input.KeyCode.Plus 8dd1479a21649a2127260de6697785a827fb6b59111ac282f8a0d248fe511b27 -->
<!-- api: F:Inno.Core.Input.KeyCode.Comma e404284c1dbf330abf783fbfef4b0cac03244cce4d67b302c4837154cc8970e6 -->
<!-- api: F:Inno.Core.Input.KeyCode.Minus 896a0593823c849def481aa56e661e3e72f0a486e0006919edba855fecc75be0 -->
<!-- api: F:Inno.Core.Input.KeyCode.Period 89b1fb2b6ba803a5146670590496f7aa8b8d295657e547ecbfcbf2ec2f05e2af -->
<!-- api: F:Inno.Core.Input.KeyCode.Slash bebddba80b5fc2b6f84ea6702fb7660a9238fee3eebde22b2035cf89a218b4cb -->
<!-- api: F:Inno.Core.Input.KeyCode.Tilde 6c65e0fe23dfc40f3de2de136e223fc8f4ff0ec67a906a8432b75bde457a048a -->
<!-- api: F:Inno.Core.Input.KeyCode.Backslash 1485a2e0ff53a59a5218d821072aeab0beb97fad82b86f03c3efd95a0c420d86 -->
<!-- api: F:Inno.Core.Input.KeyCode.Semicolon 560bb8d7705ece187af125abc28c42b3a84eac4194e806bd2839be2e44c0ec87 -->
<!-- api: F:Inno.Core.Input.KeyCode.Quote 36837c3947233541e2c22b9df6898f069a91e7f5eb103879941059033c134730 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftBracket 2f448aec323c519ba4dd0b085b86052f1a9982ee313e34c5be39dc67fc80fece -->
<!-- api: F:Inno.Core.Input.KeyCode.RightBracket 2723105ce5be960b3ed92779e32be73dd23a852d6ce711a43f486b5c753870ba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0f91b108c5967cec"></a>[`Inno.Core.Input.KeyCode.A = 65`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L16) | The A letter key. | — | — |
| <a id="api-f44fce77f6a29122"></a>[`Inno.Core.Input.KeyCode.B = 66`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L20) | The B letter key. | — | — |
| <a id="api-63c45c05923f9b23"></a>[`Inno.Core.Input.KeyCode.Backslash = 220`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L385) | The backslash key. | — | — |
| <a id="api-fe37d9e3468d037e"></a>[`Inno.Core.Input.KeyCode.Backspace = 8`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L178) | The backspace key. | — | — |
| <a id="api-aadb04f938667619"></a>[`Inno.Core.Input.KeyCode.C = 67`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L24) | The C letter key. | — | — |
| <a id="api-446034ed1dcae190"></a>[`Inno.Core.Input.KeyCode.CapsLock = 20`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L233) | The caps lock key. | — | — |
| <a id="api-f244085f9e3e572f"></a>[`Inno.Core.Input.KeyCode.Comma = 188`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L365) | The comma key. | — | — |
| <a id="api-f23cc8a0493cd14e"></a>[`Inno.Core.Input.KeyCode.D = 68`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L28) | The D letter key. | — | — |
| <a id="api-2a8e17f07a77aae3"></a>[`Inno.Core.Input.KeyCode.D0 = 48`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L121) | The 0 key on the primary keyboard row. | — | — |
| <a id="api-7fd0e5057c0b736f"></a>[`Inno.Core.Input.KeyCode.D1 = 49`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L125) | The 1 key on the primary keyboard row. | — | — |
| <a id="api-a0b33b979d3bc951"></a>[`Inno.Core.Input.KeyCode.D2 = 50`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L129) | The 2 key on the primary keyboard row. | — | — |
| <a id="api-1fdd3c5ce1a171bc"></a>[`Inno.Core.Input.KeyCode.D3 = 51`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L133) | The 3 key on the primary keyboard row. | — | — |
| <a id="api-85815228791672d6"></a>[`Inno.Core.Input.KeyCode.D4 = 52`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L137) | The 4 key on the primary keyboard row. | — | — |
| <a id="api-eacc84ef5c15d733"></a>[`Inno.Core.Input.KeyCode.D5 = 53`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L141) | The 5 key on the primary keyboard row. | — | — |
| <a id="api-c84abb064bec2634"></a>[`Inno.Core.Input.KeyCode.D6 = 54`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L145) | The 6 key on the primary keyboard row. | — | — |
| <a id="api-c5ce4c12e615f257"></a>[`Inno.Core.Input.KeyCode.D7 = 55`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L149) | The 7 key on the primary keyboard row. | — | — |
| <a id="api-4a3e9dbde75ddb8f"></a>[`Inno.Core.Input.KeyCode.D8 = 56`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L153) | The 8 key on the primary keyboard row. | — | — |
| <a id="api-cb0feaee97d6e771"></a>[`Inno.Core.Input.KeyCode.D9 = 57`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L157) | The 9 key on the primary keyboard row. | — | — |
| <a id="api-3b545e1ef6ec16bd"></a>[`Inno.Core.Input.KeyCode.Delete = 46`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L241) | The delete key. | — | — |
| <a id="api-2ae54add65a510f4"></a>[`Inno.Core.Input.KeyCode.DownArrow = 40`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L195) | The down arrow key. | — | — |
| <a id="api-be5c463fad7dc64e"></a>[`Inno.Core.Input.KeyCode.E = 69`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L32) | The E letter key. | — | — |
| <a id="api-6098c653b32e3af2"></a>[`Inno.Core.Input.KeyCode.End = 35`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L249) | The end key. | — | — |
| <a id="api-e1c5b86514cba43a"></a>[`Inno.Core.Input.KeyCode.Enter = 13`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L170) | The enter key. | — | — |
| <a id="api-57cbf3ccd1e47f5e"></a>[`Inno.Core.Input.KeyCode.Escape = 27`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L162) | The escape key. | — | — |
| <a id="api-3221f022528217b7"></a>[`Inno.Core.Input.KeyCode.F = 70`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L36) | The F letter key. | — | — |
| <a id="api-7e1418c96fbeaed7"></a>[`Inno.Core.Input.KeyCode.F1 = 112`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L312) | The F1 function key. | — | — |
| <a id="api-c7358a7fd5162870"></a>[`Inno.Core.Input.KeyCode.F10 = 121`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L348) | The F10 function key. | — | — |
| <a id="api-38357c037c60a6ba"></a>[`Inno.Core.Input.KeyCode.F11 = 122`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L352) | The F11 function key. | — | — |
| <a id="api-aff8a123eb4bc3d3"></a>[`Inno.Core.Input.KeyCode.F12 = 123`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L356) | The F12 function key. | — | — |
| <a id="api-7b867e78505daa0c"></a>[`Inno.Core.Input.KeyCode.F2 = 113`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L316) | The F2 function key. | — | — |
| <a id="api-47154330e9e769fe"></a>[`Inno.Core.Input.KeyCode.F3 = 114`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L320) | The F3 function key. | — | — |
| <a id="api-7cd6dcc594c0a1af"></a>[`Inno.Core.Input.KeyCode.F4 = 115`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L324) | The F4 function key. | — | — |
| <a id="api-6818b9b3e5d7b9f9"></a>[`Inno.Core.Input.KeyCode.F5 = 116`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L328) | The F5 function key. | — | — |
| <a id="api-9546595ce027e142"></a>[`Inno.Core.Input.KeyCode.F6 = 117`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L332) | The F6 function key. | — | — |
| <a id="api-ca78835243556d12"></a>[`Inno.Core.Input.KeyCode.F7 = 118`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L336) | The F7 function key. | — | — |
| <a id="api-ed5a1365bf3feec5"></a>[`Inno.Core.Input.KeyCode.F8 = 119`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L340) | The F8 function key. | — | — |
| <a id="api-c84733e60004436c"></a>[`Inno.Core.Input.KeyCode.F9 = 120`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L344) | The F9 function key. | — | — |
| <a id="api-78f252e670675881"></a>[`Inno.Core.Input.KeyCode.G = 71`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L40) | The G letter key. | — | — |
| <a id="api-8c69e7b9da043206"></a>[`Inno.Core.Input.KeyCode.H = 72`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L44) | The H letter key. | — | — |
| <a id="api-3fd63433d8a17e12"></a>[`Inno.Core.Input.KeyCode.Home = 36`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L245) | The home key. | — | — |
| <a id="api-ac0dfe73c8335ebe"></a>[`Inno.Core.Input.KeyCode.I = 73`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L48) | The I letter key. | — | — |
| <a id="api-f784d3ca69ab34d4"></a>[`Inno.Core.Input.KeyCode.Insert = 45`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L237) | The insert key. | — | — |
| <a id="api-a7976a4d591eda5e"></a>[`Inno.Core.Input.KeyCode.J = 74`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L52) | The J letter key. | — | — |
| <a id="api-3430e23da93543fb"></a>[`Inno.Core.Input.KeyCode.K = 75`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L56) | The K letter key. | — | — |
| <a id="api-e0b1072645eaf168"></a>[`Inno.Core.Input.KeyCode.L = 76`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L60) | The L letter key. | — | — |
| <a id="api-8763a75fd5717ccc"></a>[`Inno.Core.Input.KeyCode.LeftAlt = 164`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L224) | The left alt key. | — | — |
| <a id="api-009d9895f585b422"></a>[`Inno.Core.Input.KeyCode.LeftArrow = 37`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L183) | The left arrow key. | — | — |
| <a id="api-e385b7c04522786a"></a>[`Inno.Core.Input.KeyCode.LeftBracket = 219`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L397) | The left bracket key. | — | — |
| <a id="api-daaa4b7b2b9bc047"></a>[`Inno.Core.Input.KeyCode.LeftCtrl = 162`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L216) | The left ctrl key. | — | — |
| <a id="api-fd6d0e111c789e35"></a>[`Inno.Core.Input.KeyCode.LeftShift = 160`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L208) | The left shift key. | — | — |
| <a id="api-e1b1f8cdd8f437a2"></a>[`Inno.Core.Input.KeyCode.LeftSuper = 91`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L200) | The left super key. | — | — |
| <a id="api-21330a1a6cb3ca74"></a>[`Inno.Core.Input.KeyCode.M = 77`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L64) | The M letter key. | — | — |
| <a id="api-be0fdb180e5ce201"></a>[`Inno.Core.Input.KeyCode.Minus = 189`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L369) | The minus key. | — | — |
| <a id="api-c0252f0e26d8a26e"></a>[`Inno.Core.Input.KeyCode.N = 78`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L68) | The N letter key. | — | — |
| <a id="api-e9746528312296c4"></a>[`Inno.Core.Input.KeyCode.NumLock = 144`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L303) | The num lock key. | — | — |
| <a id="api-bf1b2b2a13de96e7"></a>[`Inno.Core.Input.KeyCode.NumPad0 = 96`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L262) | The 0 key on the numeric keypad. | — | — |
| <a id="api-81563fe66421b12e"></a>[`Inno.Core.Input.KeyCode.NumPad1 = 97`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L266) | The 1 key on the numeric keypad. | — | — |
| <a id="api-a89c58d023bb1bd5"></a>[`Inno.Core.Input.KeyCode.NumPad2 = 98`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L270) | The 2 key on the numeric keypad. | — | — |
| <a id="api-398cec1f492de096"></a>[`Inno.Core.Input.KeyCode.NumPad3 = 99`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L274) | The 3 key on the numeric keypad. | — | — |
| <a id="api-ec7ee06f20d07479"></a>[`Inno.Core.Input.KeyCode.NumPad4 = 100`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L278) | The 4 key on the numeric keypad. | — | — |
| <a id="api-4f5112527a8c3704"></a>[`Inno.Core.Input.KeyCode.NumPad5 = 101`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L282) | The 5 key on the numeric keypad. | — | — |
| <a id="api-93c8cb91e1dad7fd"></a>[`Inno.Core.Input.KeyCode.NumPad6 = 102`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L286) | The 6 key on the numeric keypad. | — | — |
| <a id="api-32268a2abea9c31d"></a>[`Inno.Core.Input.KeyCode.NumPad7 = 103`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L290) | The 7 key on the numeric keypad. | — | — |
| <a id="api-48cee7f66bba0e35"></a>[`Inno.Core.Input.KeyCode.NumPad8 = 104`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L294) | The 8 key on the numeric keypad. | — | — |
| <a id="api-acf1d1b23e596fb0"></a>[`Inno.Core.Input.KeyCode.NumPad9 = 105`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L298) | The 9 key on the numeric keypad. | — | — |
| <a id="api-73fd055e3e2bfabc"></a>[`Inno.Core.Input.KeyCode.O = 79`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L72) | The O letter key. | — | — |
| <a id="api-52abd16e75290a6d"></a>[`Inno.Core.Input.KeyCode.P = 80`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L76) | The P letter key. | — | — |
| <a id="api-28ae504ccb2e1a38"></a>[`Inno.Core.Input.KeyCode.PageDown = 34`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L257) | The page down key. | — | — |
| <a id="api-9db44d714ffcbc79"></a>[`Inno.Core.Input.KeyCode.PageUp = 33`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L253) | The page up key. | — | — |
| <a id="api-c2f396b04becbd0a"></a>[`Inno.Core.Input.KeyCode.Period = 190`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L373) | The period key. | — | — |
| <a id="api-470b209259ab747a"></a>[`Inno.Core.Input.KeyCode.Plus = 187`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L361) | The plus key. | — | — |
| <a id="api-79f47b7d9be224d3"></a>[`Inno.Core.Input.KeyCode.Q = 81`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L80) | The Q letter key. | — | — |
| <a id="api-7c8438b3f93cc945"></a>[`Inno.Core.Input.KeyCode.Quote = 222`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L393) | The quote key. | — | — |
| <a id="api-c775d83b82e35b1f"></a>[`Inno.Core.Input.KeyCode.R = 82`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L84) | The R letter key. | — | — |
| <a id="api-7bc9fde7a2f74536"></a>[`Inno.Core.Input.KeyCode.RightAlt = 165`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L228) | The right alt key. | — | — |
| <a id="api-68e4537fe75d827c"></a>[`Inno.Core.Input.KeyCode.RightArrow = 39`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L191) | The right arrow key. | — | — |
| <a id="api-0a3d75a0f545a6c4"></a>[`Inno.Core.Input.KeyCode.RightBracket = 221`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L401) | The right bracket key. | — | — |
| <a id="api-ab259d3e2cbf0de5"></a>[`Inno.Core.Input.KeyCode.RightCtrl = 163`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L220) | The right ctrl key. | — | — |
| <a id="api-5f5646fa0eefe262"></a>[`Inno.Core.Input.KeyCode.RightShift = 161`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L212) | The right shift key. | — | — |
| <a id="api-16b2209864366b0b"></a>[`Inno.Core.Input.KeyCode.RightSuper = 92`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L204) | The right super key. | — | — |
| <a id="api-160901622332250c"></a>[`Inno.Core.Input.KeyCode.S = 83`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L88) | The S letter key. | — | — |
| <a id="api-8247e19dc1162a0a"></a>[`Inno.Core.Input.KeyCode.ScrollLock = 145`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L307) | The scroll lock key. | — | — |
| <a id="api-6a6aed0262595493"></a>[`Inno.Core.Input.KeyCode.Semicolon = 186`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L389) | The semicolon key. | — | — |
| <a id="api-1442b826d5b07589"></a>[`Inno.Core.Input.KeyCode.Slash = 191`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L377) | The slash key. | — | — |
| <a id="api-9ebe24b557c80c3c"></a>[`Inno.Core.Input.KeyCode.Space = 32`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L166) | The space key. | — | — |
| <a id="api-bb636558801586d7"></a>[`Inno.Core.Input.KeyCode.T = 84`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L92) | The T letter key. | — | — |
| <a id="api-0c24ccea3cc75455"></a>[`Inno.Core.Input.KeyCode.Tab = 9`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L174) | The tab key. | — | — |
| <a id="api-abb15547f5c1706a"></a>[`Inno.Core.Input.KeyCode.Tilde = 192`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L381) | The tilde key. | — | — |
| <a id="api-81cd7a5693ddc69a"></a>[`Inno.Core.Input.KeyCode.U = 85`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L96) | The U letter key. | — | — |
| <a id="api-467e9d3075a2fe60"></a>[`Inno.Core.Input.KeyCode.Unknown = 0`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L11) | No recognized physical key. | — | — |
| <a id="api-8b1dc7c5a6784adf"></a>[`Inno.Core.Input.KeyCode.UpArrow = 38`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L187) | The up arrow key. | — | — |
| <a id="api-69cec177f1c5b460"></a>[`Inno.Core.Input.KeyCode.V = 86`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L100) | The V letter key. | — | — |
| <a id="api-4e4424f2831e19ed"></a>[`Inno.Core.Input.KeyCode.W = 87`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L104) | The W letter key. | — | — |
| <a id="api-15ddd672e6446fb9"></a>[`Inno.Core.Input.KeyCode.X = 88`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L108) | The X letter key. | — | — |
| <a id="api-b48eeaaef450bc23"></a>[`Inno.Core.Input.KeyCode.Y = 89`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L112) | The Y letter key. | — | — |
| <a id="api-2c94164985c1afce"></a>[`Inno.Core.Input.KeyCode.Z = 90`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L116) | The Z letter key. | — | — |
| <a id="api-2ce06d9d7c30c576"></a>[`public enum Inno.Core.Input.KeyCode : int`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L6) | Represents a backend-neutral physical key used by runtime input and editor shortcut contracts. | — | — |

### `Inno.Core.Input.KeyInputChange`

<!-- api: T:Inno.Core.Input.KeyInputChange 24cbb2e1cfba3801ba12147e742cf3ee4207e00c8804884e75854f391bfd4ad3 -->
<!-- api: P:Inno.Core.Input.KeyInputChange.EqualityContract 6172e48eb1d0009bfa63f3c73cf18838905d96e15dd5b51f6f64f3f0ce41dfa6 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.#ctor(Inno.Core.Input.KeyCode,System.Boolean,System.Boolean,Inno.Core.Input.KeyModifier) a5bb3c6740f6ae6b9f93a26bfe9b069c054d9864a14984a22e0a6861e66b6608 -->
<!-- api: P:Inno.Core.Input.KeyInputChange.key 807f7d411f7af695917ae072fd66b6e79f8ddb1664642729fbc19eb3dff855ec -->
<!-- api: P:Inno.Core.Input.KeyInputChange.isPressed c4bbc4ae05b16f3ab497e5084f542e83368cde49088038b98852cbf6a23d2a26 -->
<!-- api: P:Inno.Core.Input.KeyInputChange.isRepeat fdad2d1eb70d06bd17644e0b12f50ced96a5a4b88120882bda2381199758fd92 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.ToString 5cff33f5e911e95fde161270519d8fc47f983894e5586f470c87c560dc67a279 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.PrintMembers(System.Text.StringBuilder) 121e7d5db0ad4cc2d1052f6f6d22622ac16cdbb7c1e832c0b9972f31d2473eab -->
<!-- api: M:Inno.Core.Input.KeyInputChange.op_Inequality(Inno.Core.Input.KeyInputChange,Inno.Core.Input.KeyInputChange) ab9623cba3ef498df27051a16c6ac921c8631c948012b101b56b6a168720545e -->
<!-- api: M:Inno.Core.Input.KeyInputChange.op_Equality(Inno.Core.Input.KeyInputChange,Inno.Core.Input.KeyInputChange) 0d9a030a51ed4a0028f15f6aafb2616f12831a72b2e56b7dda1d8a6e6a50199f -->
<!-- api: M:Inno.Core.Input.KeyInputChange.GetHashCode f68095462804e40f9020a2d20a280b3dc09b09895d6560e41d0cbc3bc4e1ea46 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.Equals(System.Object) ca5c13dcc2f9a98c6f1df5808709b71a022ebedf2c0a4b03087a0188694e5569 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.Equals(Inno.Core.Input.InputChange) 49a6fdfa599bab2f225ca2819e5c13fcb2643aa32ce8810274fadc5193329629 -->
<!-- api: M:Inno.Core.Input.KeyInputChange.Equals(Inno.Core.Input.KeyInputChange) 66b302c13f273988a94b6e16738fe2047530d74b3e0c52ea885b4112c5031f4c -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-4c55af999fa1230d"></a>[`public sealed record Inno.Core.Input.KeyInputChange : Inno.Core.Input.InputChange, System.IEquatable<Inno.Core.Input.KeyInputChange>`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Captures a physical key transition or repeat at its original input position. | — |
| <a id="api-a4ad77408c59f99b"></a>[`protected override System.Type Inno.Core.Input.KeyInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-2fd0463ad0798faa"></a>[`public Inno.Core.Input.KeyInputChange.KeyInputChange(Inno.Core.Input.KeyCode key, bool isPressed, bool isRepeat, Inno.Core.Input.KeyModifier modifiers)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L26) | Creates a key change independent of any platform event object.<br>`key`：The physical key.<br>`isPressed`：True for a press or repeat; false for a release.<br>`isRepeat`：True only for an operating-system key repeat.<br>`modifiers`：Physical modifiers at this change. | `System.ArgumentException`：A release is marked as a repeated press. |
| <a id="api-cc11f52667704800"></a>[`public Inno.Core.Input.KeyCode Inno.Core.Input.KeyInputChange.key { get; }`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L42) | Gets the physical key. | — |
| <a id="api-216ba5afe5e66966"></a>[`public bool Inno.Core.Input.KeyInputChange.isPressed { get; }`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L46) | Gets whether this change presses or repeats the key. | — |
| <a id="api-d5b4e29b1034917d"></a>[`public bool Inno.Core.Input.KeyInputChange.isRepeat { get; }`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L50) | Gets whether this is a repeated press rather than a new transition. | — |
| <a id="api-0bae46ffa2bebec6"></a>[`public override string Inno.Core.Input.KeyInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-b84f604cbba9ff16"></a>[`protected override bool Inno.Core.Input.KeyInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-5620c31b55a654cf"></a>[`public static bool Inno.Core.Input.KeyInputChange.operator !=(Inno.Core.Input.KeyInputChange? left, Inno.Core.Input.KeyInputChange? right)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | 编译器生成的值不等比较。 | — |
| <a id="api-f5d02669c1f49b41"></a>[`public static bool Inno.Core.Input.KeyInputChange.operator ==(Inno.Core.Input.KeyInputChange? left, Inno.Core.Input.KeyInputChange? right)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | 编译器生成的记录值相等比较。 | — |
| <a id="api-1edda3cd6d03d492"></a>[`public override int Inno.Core.Input.KeyInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-59ce7cafd8479a81"></a>[`public override bool Inno.Core.Input.KeyInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-1f73f729104ac8b1"></a>[`public override sealed bool Inno.Core.Input.KeyInputChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-d8934d9c11ab2970"></a>[`public bool Inno.Core.Input.KeyInputChange.Equals(Inno.Core.Input.KeyInputChange? other)`](../../src/foundation/core/Inno.Core.Input/KeyInputChange.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.KeyModifier`

<!-- api: T:Inno.Core.Input.KeyModifier f2f9bcd5b6a7ec816cc359a65134696ab78ec2a086ced5dfe18b46029c710bc6 -->
<!-- api: F:Inno.Core.Input.KeyModifier.None 602e73689a1774b0ef64b1d4b13f4716a732ce34157c56e1379aac25dd93aca3 -->
<!-- api: F:Inno.Core.Input.KeyModifier.Alt 4e61123c99c49be63c65ec99da8ff183dc53af472cbd1fb5a9f837616968a5ae -->
<!-- api: F:Inno.Core.Input.KeyModifier.Control 126564b667973c20b93951f51a580adc9549572c4d217ff4460174806d5eb03f -->
<!-- api: F:Inno.Core.Input.KeyModifier.Shift 7e858c8712655b7f1fa79fb277bfd7be967e27993b9d3abf79a314dfd1991ef4 -->
<!-- api: F:Inno.Core.Input.KeyModifier.Super 145315bc89c6d4460464dd5007ff42e046d7a9ad1dfe74d3ce5bbd4682689bc1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-23000daebdc38f64"></a>[`Inno.Core.Input.KeyModifier.Alt = 1`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L18) | The alt key. | — | — |
| <a id="api-06761bbd7e661b62"></a>[`Inno.Core.Input.KeyModifier.Control = 2`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L22) | The control key. | — | — |
| <a id="api-1b88d9abe40f9761"></a>[`Inno.Core.Input.KeyModifier.None = 0`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L14) | The none key. | — | — |
| <a id="api-d638bbb4d2f75288"></a>[`Inno.Core.Input.KeyModifier.Shift = 4`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L26) | The shift key. | — | — |
| <a id="api-0727c3cb697045d7"></a>[`Inno.Core.Input.KeyModifier.Super = 8`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L30) | The super key. | — | — |
| <a id="api-f0de48a27308a705"></a>[`public enum Inno.Core.Input.KeyModifier : int`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L9) | Identifies the supported key modifier values for this contract. | — | — |

### `Inno.Core.Input.MouseButton`

<!-- api: T:Inno.Core.Input.MouseButton f56eea1c919709e285645c021b7906e33efb417ec40aec927d639c9468d7d963 -->
<!-- api: F:Inno.Core.Input.MouseButton.Left aab5b19af45df4328f78986ec1e82eb70b15cf67029edb8835f463921a8b2fb1 -->
<!-- api: F:Inno.Core.Input.MouseButton.Right b32f3940bfa573835b25b3eff46b3688bfbd5df1a4a10009613a9cdba6304c76 -->
<!-- api: F:Inno.Core.Input.MouseButton.Middle 9231deff09d3964534a4289a75c4a777746e87a910167736ea5846a9c7443693 -->
<!-- api: F:Inno.Core.Input.MouseButton.XButton1 19f3bcab1ba4c02ec4456a67f078305a3e49e09b38da451000c540f9b4479421 -->
<!-- api: F:Inno.Core.Input.MouseButton.XButton2 aa2fb598ec37e694bdd580eb8696a08f5fff3e3b72fb6e2b8a86f460f3fa5d62 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-845906274d27b175"></a>[`Inno.Core.Input.MouseButton.Left = 0`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L11) | The left key. | — | — |
| <a id="api-64e82c041f04039e"></a>[`Inno.Core.Input.MouseButton.Middle = 2`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L19) | The middle key. | — | — |
| <a id="api-23f3072f56781ad4"></a>[`Inno.Core.Input.MouseButton.Right = 1`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L15) | The right key. | — | — |
| <a id="api-95181e1092f8f209"></a>[`Inno.Core.Input.MouseButton.XButton1 = 3`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L23) | The xbutton1 key. | — | — |
| <a id="api-568fdba226a42cda"></a>[`Inno.Core.Input.MouseButton.XButton2 = 4`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L27) | The xbutton2 key. | — | — |
| <a id="api-01b091eb5ecbacb5"></a>[`public enum Inno.Core.Input.MouseButton : int`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L6) | Identifies the supported mouse button values for this contract. | — | — |

### `Inno.Core.Input.MouseCursor`

<!-- api: T:Inno.Core.Input.MouseCursor 7ef61aef518f37c54894212bb399f83993ff09b35e18b0a711ae6e1d90ccded3 -->
<!-- api: F:Inno.Core.Input.MouseCursor.None 4ff9ce909399a710f17399c11dfc0f2f46754713488d9fb867a08e7bc8073b79 -->
<!-- api: F:Inno.Core.Input.MouseCursor.Arrow 8c7c34e19b5fcfaf7cac4ed38b9964abc6e0dd265ddc970912fde3759d4fd709 -->
<!-- api: F:Inno.Core.Input.MouseCursor.TextInput a358e751f78c75a6248ca730065ae0fffcf734ec879e75c827ef7a07182f2188 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeAll 664d9031e5c7744fa3716c98e72b533e11b8730634477ccc373d702f18b9c78b -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNS 6d52f4a2a225ee8820cb7a31e4b0d9351c222cf7b184c29a3fb646cc0d5e0296 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeEW 706a8983f7eb34b1e14d4ac0543754989e3f0a33851d31c8a71631d97fc8f327 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNESW aa70462e29c40c5e057f03d07e5caac3c92101278b52fea096f768b9a9db4b93 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNWSE 419f95b699ae4d885eb7c5b77da6e0adff61a63181bd6fe413d55a30c3efe3bf -->
<!-- api: F:Inno.Core.Input.MouseCursor.Hand 85d804cad009fe8fc52e7f373fa1cf0f32b7241acd7b3fb4c6cfdedcdcf926ab -->
<!-- api: F:Inno.Core.Input.MouseCursor.NotAllowed cfeddc1c64eb28d70c6d17ae648b4b8495144c57478e52fc768b5f3d470d2002 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9e78ec82726a8961"></a>[`Inno.Core.Input.MouseCursor.Arrow = 0`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L16) | The arrow key. | — | — |
| <a id="api-d6df7b594df7a93a"></a>[`Inno.Core.Input.MouseCursor.Hand = 7`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L44) | The hand key. | — | — |
| <a id="api-df346b19603d4537"></a>[`Inno.Core.Input.MouseCursor.None = -1`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L11) | The none key. | — | — |
| <a id="api-3fc82460676573c7"></a>[`Inno.Core.Input.MouseCursor.NotAllowed = 8`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L48) | The not allowed key. | — | — |
| <a id="api-64d6a9feba2a5634"></a>[`Inno.Core.Input.MouseCursor.ResizeAll = 2`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L24) | The resize all key. | — | — |
| <a id="api-8aa7035795a11187"></a>[`Inno.Core.Input.MouseCursor.ResizeEW = 4`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L32) | The resize ew key. | — | — |
| <a id="api-61d39ddf9c7a19f9"></a>[`Inno.Core.Input.MouseCursor.ResizeNESW = 5`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L36) | The resize nesw key. | — | — |
| <a id="api-1b18cb3d6f92d300"></a>[`Inno.Core.Input.MouseCursor.ResizeNS = 3`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L28) | The resize ns key. | — | — |
| <a id="api-e1623b9d534c0657"></a>[`Inno.Core.Input.MouseCursor.ResizeNWSE = 6`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L40) | The resize nwse key. | — | — |
| <a id="api-f1a56f564ae8caff"></a>[`Inno.Core.Input.MouseCursor.TextInput = 1`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L20) | The text input key. | — | — |
| <a id="api-026f0b41f8bb1887"></a>[`public enum Inno.Core.Input.MouseCursor : int`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L6) | Identifies the supported mouse cursor values for this contract. | — | — |

### `Inno.Core.Input.PointerButtonInputChange`

<!-- api: T:Inno.Core.Input.PointerButtonInputChange 2b871b63f47ac23d433882a0c8e1c7f526c3026355653d0186404c5c3ff32efc -->
<!-- api: P:Inno.Core.Input.PointerButtonInputChange.EqualityContract 431d1d3da85d782290da30881e71e441abf5446bd0bfe3af060456b3f52eb476 -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.#ctor(Inno.Core.Mathematics.Vector2,Inno.Core.Input.MouseButton,System.Boolean,Inno.Core.Input.KeyModifier) 4b24c59f8686f508c41a93c18291a0432e05b6b666a31d380b9fb574877a97ea -->
<!-- api: P:Inno.Core.Input.PointerButtonInputChange.button 8ebd5c43215d6e498fbb28a377f8b5808240274520b25c2cc2855bebe3ce7776 -->
<!-- api: P:Inno.Core.Input.PointerButtonInputChange.isPressed 3fa667e3a5efe0a3ece00c17d1e195782a35de7ebce44150692cf874b184c47d -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.ToString 2f06f891ce4a20c4c348773091333f3b8c8fd8f9ec1b4954cc6add5c90bee07a -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.PrintMembers(System.Text.StringBuilder) a97b9b6b295e41e7c6a2ee80a81f9110730891d4a77a28f854f5988f3834824b -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.op_Inequality(Inno.Core.Input.PointerButtonInputChange,Inno.Core.Input.PointerButtonInputChange) 348e7fbc43546eb310b964e0b3986c06a0a413c018819e198baf9044d71c4150 -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.op_Equality(Inno.Core.Input.PointerButtonInputChange,Inno.Core.Input.PointerButtonInputChange) f66d103eaea06152f02431f3871664b7bed260f9044b5ca6c6b7edc210a108b9 -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.GetHashCode 12672d30c32860049c6a0ad95a465bb1eee895c40c06f34f352bfdd0e49347b2 -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.Equals(System.Object) 005e0b3dccda513ec3b324404c91c9cd9765dc67bc47fa6325424d2517f9f37f -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.Equals(Inno.Core.Input.PointerInputChange) 3a001dc4e8e0911d1fb40401e14775aa92c71dc7e77e80e78de0f5133de79954 -->
<!-- api: M:Inno.Core.Input.PointerButtonInputChange.Equals(Inno.Core.Input.PointerButtonInputChange) 010a104a146beba549178f6c78436d2253630f10f42dc1419dc2ba452771f5e5 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-3cd1b50eff003271"></a>[`public sealed record Inno.Core.Input.PointerButtonInputChange : Inno.Core.Input.PointerInputChange, System.IEquatable<Inno.Core.Input.PointerButtonInputChange>`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Captures a pointer-button transition together with its hit-test position. | — |
| <a id="api-784c83b999186775"></a>[`protected override System.Type Inno.Core.Input.PointerButtonInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-82d46bb32a86de66"></a>[`public Inno.Core.Input.PointerButtonInputChange.PointerButtonInputChange(Inno.Core.Mathematics.Vector2 position, Inno.Core.Input.MouseButton button, bool isPressed, Inno.Core.Input.KeyModifier modifiers)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L28) | Creates a positioned pointer-button transition.<br>`position`：Pointer coordinates at the transition.<br>`button`：The physical pointer button.<br>`isPressed`：True for a press; false for a release.<br>`modifiers`：Physical modifiers at the transition. | `System.ArgumentOutOfRangeException`：A position component is not finite. |
| <a id="api-3404bdf4fe8fe7f7"></a>[`public Inno.Core.Input.MouseButton Inno.Core.Input.PointerButtonInputChange.button { get; }`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L41) | Gets the physical pointer button. | — |
| <a id="api-496d301d4b7898b1"></a>[`public bool Inno.Core.Input.PointerButtonInputChange.isPressed { get; }`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L45) | Gets whether the button is pressed by this transition. | — |
| <a id="api-bc6e9cf4a83223b1"></a>[`public override string Inno.Core.Input.PointerButtonInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-ac552b77bd5a7f0c"></a>[`protected override bool Inno.Core.Input.PointerButtonInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-0adb029b604cd1a1"></a>[`public static bool Inno.Core.Input.PointerButtonInputChange.operator !=(Inno.Core.Input.PointerButtonInputChange? left, Inno.Core.Input.PointerButtonInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-967b173474e1f086"></a>[`public static bool Inno.Core.Input.PointerButtonInputChange.operator ==(Inno.Core.Input.PointerButtonInputChange? left, Inno.Core.Input.PointerButtonInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-b6446b5c4af646a1"></a>[`public override int Inno.Core.Input.PointerButtonInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-b71ac759fe96eed9"></a>[`public override bool Inno.Core.Input.PointerButtonInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-e921e012f046650e"></a>[`public override sealed bool Inno.Core.Input.PointerButtonInputChange.Equals(Inno.Core.Input.PointerInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-3410106123b314df"></a>[`public bool Inno.Core.Input.PointerButtonInputChange.Equals(Inno.Core.Input.PointerButtonInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerButtonInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.PointerInputChange`

<!-- api: T:Inno.Core.Input.PointerInputChange fb7bfd017caec454a39f5ee41126a87a3455516e9cabf7cd2c8c262e723a17b6 -->
<!-- api: P:Inno.Core.Input.PointerInputChange.EqualityContract 1cdde54d30827f6ad34d0cb79c2aa22eab81b2505d60b79706934cee9bc7d058 -->
<!-- api: P:Inno.Core.Input.PointerInputChange.position 63198c11ed07ea36d7f2ff69f19b6bb45d87fe9c70b19b25925eeb829fd54c6c -->
<!-- api: M:Inno.Core.Input.PointerInputChange.ToString f2736cc013b4a2a71c5278783dcb9ca702b95d0aa6687b7992d7301638c4b617 -->
<!-- api: M:Inno.Core.Input.PointerInputChange.PrintMembers(System.Text.StringBuilder) b9195442dd7682be1a404adc8bda0ec331479601da632166a8d774246c01c9a2 -->
<!-- api: M:Inno.Core.Input.PointerInputChange.op_Inequality(Inno.Core.Input.PointerInputChange,Inno.Core.Input.PointerInputChange) 56ff90cb8fe6a346b60e4aa27cdde3f9e5efc7674a4723e75161c34936db5050 -->
<!-- api: M:Inno.Core.Input.PointerInputChange.op_Equality(Inno.Core.Input.PointerInputChange,Inno.Core.Input.PointerInputChange) e3627f0d26c46d9b4cbd1956afe7512c6a6ef86dfb90a4d783c24239ab8c51ce -->
<!-- api: M:Inno.Core.Input.PointerInputChange.GetHashCode 7ffbd343c0a495264954c789e20fa02827ee3b87e2668d92c6dc3757e9c1b02f -->
<!-- api: M:Inno.Core.Input.PointerInputChange.Equals(System.Object) 8d055b041d1a486e23665725b68239264e1e6ec4896fd2ea3d00fb73c891678f -->
<!-- api: M:Inno.Core.Input.PointerInputChange.Equals(Inno.Core.Input.InputChange) 8bbeeec0c5994603cc31c41a66bc54a58452b408b21d1c09f44675a66838cf48 -->
<!-- api: M:Inno.Core.Input.PointerInputChange.Equals(Inno.Core.Input.PointerInputChange) 0c23cc71cd16faae57912aaf2118f6dfaa555e4ff1b01f6201654aec905fd2f4 -->
<!-- api: M:Inno.Core.Input.PointerInputChange.#ctor(Inno.Core.Input.PointerInputChange) 61c6a42a1823cd033b2f150f2945275cb09657cf299f8ed8dfe96bbadb76b47e -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-ec10f05bf38504d2"></a>[`public abstract record Inno.Core.Input.PointerInputChange : Inno.Core.Input.InputChange, System.IEquatable<Inno.Core.Input.PointerInputChange>`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Captures a pointer fact in the coordinate space declared by its containing snapshot. | — |
| <a id="api-e6216c25b2774db0"></a>[`protected override System.Type Inno.Core.Input.PointerInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-bc90626d2e28964b"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Core.Input.PointerInputChange.position { get; }`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L35) | Gets the pointer position at this change, not the final frame position. | — |
| <a id="api-a4c1a843e227905e"></a>[`public override string Inno.Core.Input.PointerInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-dbf683da14af2260"></a>[`protected override bool Inno.Core.Input.PointerInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-95140918d2addd8f"></a>[`public static bool Inno.Core.Input.PointerInputChange.operator !=(Inno.Core.Input.PointerInputChange? left, Inno.Core.Input.PointerInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | 编译器生成的值不等比较。 | — |
| <a id="api-fe8c2517b9fda12d"></a>[`public static bool Inno.Core.Input.PointerInputChange.operator ==(Inno.Core.Input.PointerInputChange? left, Inno.Core.Input.PointerInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | 编译器生成的记录值相等比较。 | — |
| <a id="api-c2e2857ddfbbe7af"></a>[`public override int Inno.Core.Input.PointerInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-f0fadf03b83a258f"></a>[`public override bool Inno.Core.Input.PointerInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-ac67d2e268ca7e77"></a>[`public override sealed bool Inno.Core.Input.PointerInputChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-1c8f7cd4582dc6c7"></a>[`public virtual bool Inno.Core.Input.PointerInputChange.Equals(Inno.Core.Input.PointerInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-45eb41b0950aebf0"></a>[`protected Inno.Core.Input.PointerInputChange.PointerInputChange(Inno.Core.Input.PointerInputChange original)`](../../src/foundation/core/Inno.Core.Input/PointerInputChange.cs#L9) | 编译器生成的记录构造入口，参数对应左列声明的成员。 | — |

### `Inno.Core.Input.PointerMoveInputChange`

<!-- api: T:Inno.Core.Input.PointerMoveInputChange 32fd7dada8393b1e145a7719f42a7e365747791ad4ae721ead1e36d05bfef4b4 -->
<!-- api: P:Inno.Core.Input.PointerMoveInputChange.EqualityContract 0c95bfb8839b57dc911dd3ef294b2f8e3c0bde7be73dc6a42f9552c1bcc44e32 -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.#ctor(Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2,Inno.Core.Input.KeyModifier) 0b66698152ff262cb3010f73705ed71fb724487ef844d4536fb0eeffd532277c -->
<!-- api: P:Inno.Core.Input.PointerMoveInputChange.previousPosition a04857ed7e3a647748bee90e70749cded8f38a05d6ef4344a59efe99691c4aa7 -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.ToString 8fff3e1effe4175cbe6c3f14da7d6476ef617713afd53a7f10be09e842feae3a -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.PrintMembers(System.Text.StringBuilder) b522404b631ec08d01107cd97b21e5bd620599a8150819418769a1bb6a628d4d -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.op_Inequality(Inno.Core.Input.PointerMoveInputChange,Inno.Core.Input.PointerMoveInputChange) f150de09d202511ecf2acce40f9ec819c007cfef9a0f5f731fe77dc4c9a6b128 -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.op_Equality(Inno.Core.Input.PointerMoveInputChange,Inno.Core.Input.PointerMoveInputChange) 2e80213e5633ade37e9cacc5f8999e6954b8d68adfbed7b175dd0aa025e2d389 -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.GetHashCode 526d955bcf293559e49ba37976f13254704fcf93443015d0df34f2a0c5b58b5f -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.Equals(System.Object) 4ac9b1d3007956c05d9f2709577ff240a25b4485502b12d5b9c72e085d35707e -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.Equals(Inno.Core.Input.PointerInputChange) da32087d65a40b9e2ed7fbaaa4bf27b9cce72ca18efd796d5e393b24d47438df -->
<!-- api: M:Inno.Core.Input.PointerMoveInputChange.Equals(Inno.Core.Input.PointerMoveInputChange) c130e29fdea2b9a4584d55a0d82c6468e27b294620ae078e012b1a51c37f39d1 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-4a21b5946d015a95"></a>[`public sealed record Inno.Core.Input.PointerMoveInputChange : Inno.Core.Input.PointerInputChange, System.IEquatable<Inno.Core.Input.PointerMoveInputChange>`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Captures pointer movement and its prior position in one declared coordinate space. | — |
| <a id="api-626c26f58cc5f078"></a>[`protected override System.Type Inno.Core.Input.PointerMoveInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-b3bc3a274dae36f6"></a>[`public Inno.Core.Input.PointerMoveInputChange.PointerMoveInputChange(Inno.Core.Mathematics.Vector2 position, Inno.Core.Mathematics.Vector2 previousPosition, Inno.Core.Input.KeyModifier modifiers)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L26) | Creates a pointer movement without accumulating away its ordering.<br>`position`：The new pointer position.<br>`previousPosition`：The preceding pointer position.<br>`modifiers`：Physical modifiers at the movement. | `System.ArgumentOutOfRangeException`：A position is not finite. |
| <a id="api-66e8d99dfa091d8d"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Core.Input.PointerMoveInputChange.previousPosition { get; }`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L39) | Gets the pointer position immediately preceding this movement. | — |
| <a id="api-c20da8907c63e1a8"></a>[`public override string Inno.Core.Input.PointerMoveInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-8c332744ea70786c"></a>[`protected override bool Inno.Core.Input.PointerMoveInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-61d67e8f8e4347e5"></a>[`public static bool Inno.Core.Input.PointerMoveInputChange.operator !=(Inno.Core.Input.PointerMoveInputChange? left, Inno.Core.Input.PointerMoveInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | 编译器生成的值不等比较。 | — |
| <a id="api-9e5500015d0bf2b1"></a>[`public static bool Inno.Core.Input.PointerMoveInputChange.operator ==(Inno.Core.Input.PointerMoveInputChange? left, Inno.Core.Input.PointerMoveInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | 编译器生成的记录值相等比较。 | — |
| <a id="api-46f655af39c4cf26"></a>[`public override int Inno.Core.Input.PointerMoveInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-79e6d3f227e1d056"></a>[`public override bool Inno.Core.Input.PointerMoveInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-ea2f723c56857386"></a>[`public override sealed bool Inno.Core.Input.PointerMoveInputChange.Equals(Inno.Core.Input.PointerInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-520740da53fa0fb3"></a>[`public bool Inno.Core.Input.PointerMoveInputChange.Equals(Inno.Core.Input.PointerMoveInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerMoveInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.PointerScrollInputChange`

<!-- api: T:Inno.Core.Input.PointerScrollInputChange 04fdb9503addf47a1fcc3e8ca296f7df3b3061aa568fdf4b842ab3ac53c0b818 -->
<!-- api: P:Inno.Core.Input.PointerScrollInputChange.EqualityContract 7d38fe8dbf8d9eb9a27d0afc0bdf6ccb44653405ca4f2d6081e3d0fde68fff34 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.#ctor(Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2,Inno.Core.Input.KeyModifier) 8de8574829022f6e77b6b7057c208d1a07674e39d5b3c086efc8ee17b9be7004 -->
<!-- api: P:Inno.Core.Input.PointerScrollInputChange.delta 2097caad385342c8e5546c4c4c602cb34417ecff36f581a733f0717701e56adb -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.ToString b0b73a891fac4b2f853c7cc4271c0357fc017a31e1b9ae041c28fd32483bf7b7 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.PrintMembers(System.Text.StringBuilder) 711971605838a7743cf7c61be9e762e1dd09d130841b16f17ad772b6caacf1c0 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.op_Inequality(Inno.Core.Input.PointerScrollInputChange,Inno.Core.Input.PointerScrollInputChange) f6e14d5822bed7675f7d2f8f391b79d604864f6018ced6995b0d7212e8e7ea63 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.op_Equality(Inno.Core.Input.PointerScrollInputChange,Inno.Core.Input.PointerScrollInputChange) a7885ca5bfe7f00827b7b08c313071b29a84f14d158419175f2cbf7f19cf04b1 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.GetHashCode 9adc2e7e7726e61471e8be995b0bed9182f174e3ab531bc20d6ff18e403d5866 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.Equals(System.Object) 704d0cb08e69dca706f69ee6b334eec60bcdede1fa7b550657d9e76c1e145072 -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.Equals(Inno.Core.Input.PointerInputChange) b0f3ac382b55271481bf520394b04ca41c199bb86885ef60e3aa72e71070edaf -->
<!-- api: M:Inno.Core.Input.PointerScrollInputChange.Equals(Inno.Core.Input.PointerScrollInputChange) 92791a6231cd9418a4b773f4c3eb75871b092da1c5b8b0974e3dc724614e668a -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-941f0376aa2640c3"></a>[`public sealed record Inno.Core.Input.PointerScrollInputChange : Inno.Core.Input.PointerInputChange, System.IEquatable<Inno.Core.Input.PointerScrollInputChange>`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Captures wheel movement at the pointer position that receives it. | — |
| <a id="api-b12e58f7aa28b6d6"></a>[`protected override System.Type Inno.Core.Input.PointerScrollInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-9dad4b05cff31279"></a>[`public Inno.Core.Input.PointerScrollInputChange.PointerScrollInputChange(Inno.Core.Mathematics.Vector2 position, Inno.Core.Mathematics.Vector2 delta, Inno.Core.Input.KeyModifier modifiers)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L26) | Creates a positioned wheel change.<br>`position`：The pointer position at the wheel event.<br>`delta`：Wheel movement, positive right horizontally and up vertically.<br>`modifiers`：Physical modifiers at the wheel event. | `System.ArgumentOutOfRangeException`：A position or wheel component is not finite. |
| <a id="api-a7bc1635f0bae958"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Core.Input.PointerScrollInputChange.delta { get; }`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L39) | Gets wheel movement independently of pointer-coordinate scaling. | — |
| <a id="api-2ee3fb36bbc1432f"></a>[`public override string Inno.Core.Input.PointerScrollInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-e0ece3f2d356f6b9"></a>[`protected override bool Inno.Core.Input.PointerScrollInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-e953be392e9b39e1"></a>[`public static bool Inno.Core.Input.PointerScrollInputChange.operator !=(Inno.Core.Input.PointerScrollInputChange? left, Inno.Core.Input.PointerScrollInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | 编译器生成的值不等比较。 | — |
| <a id="api-1ae94fd33cff1913"></a>[`public static bool Inno.Core.Input.PointerScrollInputChange.operator ==(Inno.Core.Input.PointerScrollInputChange? left, Inno.Core.Input.PointerScrollInputChange? right)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | 编译器生成的记录值相等比较。 | — |
| <a id="api-69521f07b869be3f"></a>[`public override int Inno.Core.Input.PointerScrollInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-be668b3d02daf615"></a>[`public override bool Inno.Core.Input.PointerScrollInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-c0da6ca54f4fc90c"></a>[`public override sealed bool Inno.Core.Input.PointerScrollInputChange.Equals(Inno.Core.Input.PointerInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-1af911f842b42e1c"></a>[`public bool Inno.Core.Input.PointerScrollInputChange.Equals(Inno.Core.Input.PointerScrollInputChange? other)`](../../src/foundation/core/Inno.Core.Input/PointerScrollInputChange.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.Core.Input.TextInputChange`

<!-- api: T:Inno.Core.Input.TextInputChange df1e081b310ae4c27bd809fa2c799843ccbc59e09c26b59b222306fe974485cb -->
<!-- api: P:Inno.Core.Input.TextInputChange.EqualityContract f2a498d9b139b612f95bf35d670fbe38de68ede423fcf6b0b5bdadbf6fcf9342 -->
<!-- api: M:Inno.Core.Input.TextInputChange.#ctor(System.String,Inno.Core.Input.KeyModifier) 7119f332181b114685be7da575c612c1d458009607bbf5a4695dcccf57d282c7 -->
<!-- api: P:Inno.Core.Input.TextInputChange.text d1a94520c95e6774e49c9edc1b0787e07495def3191f9e0c80cdf87353c430d9 -->
<!-- api: M:Inno.Core.Input.TextInputChange.ToString 5c588398fb405a11aee71f52cf7e1515421a4da31deb3fc917e3baa47307b0c0 -->
<!-- api: M:Inno.Core.Input.TextInputChange.PrintMembers(System.Text.StringBuilder) 37c784f9ba3900b66f0987d9396db2607b279b55b4c09bc272502d91ec6937ea -->
<!-- api: M:Inno.Core.Input.TextInputChange.op_Inequality(Inno.Core.Input.TextInputChange,Inno.Core.Input.TextInputChange) f7055069a6f2c6ffcb819119d9c99a2570a7e99fd5a5852d357378d015e39ac1 -->
<!-- api: M:Inno.Core.Input.TextInputChange.op_Equality(Inno.Core.Input.TextInputChange,Inno.Core.Input.TextInputChange) 36a416832eb25efda50ff98f159563ab85010dbf8133323d85d4812b601f292f -->
<!-- api: M:Inno.Core.Input.TextInputChange.GetHashCode 9b1dde37b6c228054a868545a31ee8cc7e207d7194ed628d5ee953d670fbd140 -->
<!-- api: M:Inno.Core.Input.TextInputChange.Equals(System.Object) 42041abde11fcf789bafd8d1f2a3d7acbc3dbfa0fdc43f70a6b6ba84f761d16c -->
<!-- api: M:Inno.Core.Input.TextInputChange.Equals(Inno.Core.Input.InputChange) 49b998cc3ede0707c0ec866381f8ff9a8919d202a5b1bb13848b41f19da5f070 -->
<!-- api: M:Inno.Core.Input.TextInputChange.Equals(Inno.Core.Input.TextInputChange) a0179682c7deb3fe1afec7d035a857d8af47b9c229baccd4e632ea8c64e7084c -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-6b0e5c9595dc951d"></a>[`public sealed record Inno.Core.Input.TextInputChange : Inno.Core.Input.InputChange, System.IEquatable<Inno.Core.Input.TextInputChange>`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Captures committed Unicode text at its original position among other input changes. | — |
| <a id="api-64f9eb8d22e3b7c5"></a>[`protected override System.Type Inno.Core.Input.TextInputChange.EqualityContract { get; }`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | 编译器生成的记录运行时类型相等契约。 | — |
| <a id="api-5498bd9ee0e42f1a"></a>[`public Inno.Core.Input.TextInputChange.TextInputChange(string text, Inno.Core.Input.KeyModifier modifiers)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L22) | Creates a completed text-input fact.<br>`text`：Nonempty committed Unicode text.<br>`modifiers`：Physical modifiers at the commit. | `System.ArgumentException`：The text is empty. |
| <a id="api-27a99cf319473632"></a>[`public string Inno.Core.Input.TextInputChange.text { get; }`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L33) | Gets committed Unicode text without changing its normalization. | — |
| <a id="api-7d178f4a65daba3b"></a>[`public override string Inno.Core.Input.TextInputChange.ToString()`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — |
| <a id="api-40dd4538c1e4326d"></a>[`protected override bool Inno.Core.Input.TextInputChange.PrintMembers(System.Text.StringBuilder builder)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | 编译器生成的记录成员文本格式化。 | — |
| <a id="api-91df6629a51656c4"></a>[`public static bool Inno.Core.Input.TextInputChange.operator !=(Inno.Core.Input.TextInputChange? left, Inno.Core.Input.TextInputChange? right)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-de3f766011310ed0"></a>[`public static bool Inno.Core.Input.TextInputChange.operator ==(Inno.Core.Input.TextInputChange? left, Inno.Core.Input.TextInputChange? right)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-308bb0aa38f08c1b"></a>[`public override int Inno.Core.Input.TextInputChange.GetHashCode()`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — |
| <a id="api-82880ebe1a83d0f7"></a>[`public override bool Inno.Core.Input.TextInputChange.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Determines whether the specified object is equal to the current object.<br>`obj`：The object to compare with the current object.<br>返回：`true` if the specified object is equal to the current object; otherwise, `false`. | — |
| <a id="api-8d9c48d5a80742e2"></a>[`public override sealed bool Inno.Core.Input.TextInputChange.Equals(Inno.Core.Input.InputChange? other)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-26fa70d3cd18d196"></a>[`public bool Inno.Core.Input.TextInputChange.Equals(Inno.Core.Input.TextInputChange? other)`](../../src/foundation/core/Inno.Core.Input/TextInputChange.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
