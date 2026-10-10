# Inno.Input

[Input 索引](README.md) · [Runtime](Inno.Input.Runtime.md) · [Wiki 首页](../README.md)

`Inno.Input` 是后端中立的物理输入契约。它引用 `Inno.Core.Input` 的 `KeyCode`、`MouseButton` 值类型，但不引用 SDL3、窗口实现、Editor 或具体 Input Action 模型。

## 公开 API

| API | 稳定语义 |
| --- | --- |
| `IInputBackend.Capture(frameIndex)` | 在帧边界产生完整不可变快照，并消费瞬时 pressed/released/delta。 |
| `IInputService.snapshot` | 当前 Session 当前帧的唯一输入状态。 |
| `InputSnapshot` | 键鼠 down/pressed/released、位置、delta、wheel、按顺序提交的 Unicode 文本与 frame index。 |
| `InputExecutionContext` | `AsyncLocal` 隔离、严格 LIFO 的服务绑定。 |
| `Input` | 游戏脚本使用的无状态 façade。 |

```csharp
using InnoEngine.Input;

if (Input.WasKeyPressed(KeyCode.Space))
{
    // Trigger one gameplay command.
}
```

无活动 scope 时 façade 明确抛出异常。Snapshot 不受查询顺序影响；状态 owner 是 Runtime Subsystem，而不是静态类。SDL3 的 UTF-8 文本提交通过 `TextInputEvent` 进入 `textInput`，供 UI Context 消费；IME 预编辑/候选区、Gamepad 与 touch 尚未进入当前稳定契约。

[下一页：Inno.Input.Runtime](Inno.Input.Runtime.md)

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Input.IInputBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.IInputBackend`](../../src/services/input/Inno.Input/IInputBackend.cs#L8) | Captures complete input state from one platform backend at frame boundaries. |
| [`Inno.Input.InputSnapshot Inno.Input.IInputBackend.Capture(long frameIndex)`](../../src/services/input/Inno.Input/IInputBackend.cs#L19) | Captures a snapshot and consumes transient press, release, movement, and wheel state. |

### `Inno.Input.IInputService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.IInputService`](../../src/services/input/Inno.Input/IInputService.cs#L6) | Exposes the current immutable input snapshot to runtime systems and scripts. |
| [`Inno.Input.InputSnapshot Inno.Input.IInputService.snapshot`](../../src/services/input/Inno.Input/IInputService.cs#L11) | Gets the snapshot captured at the current frame boundary. |

### `Inno.Input.Input`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.Input`](../../src/services/input/Inno.Input/Input.cs#L56) | Provides script-friendly access to the current frame's input snapshot. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Input.Input.mouseDelta`](../../src/services/input/Inno.Input/Input.cs#L71) | Gets pointer movement accumulated during this frame. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Input.Input.mousePosition`](../../src/services/input/Inno.Input/Input.cs#L66) | Gets the current pointer position in window coordinates. |
| [`static Inno.Core.Mathematics.Vector2 Inno.Input.Input.scrollDelta`](../../src/services/input/Inno.Input/Input.cs#L76) | Gets wheel movement accumulated during this frame. |
| [`static Inno.Input.InputSnapshot Inno.Input.Input.snapshot`](../../src/services/input/Inno.Input/Input.cs#L61) | Gets the complete immutable current-frame snapshot. |
| [`static bool Inno.Input.Input.IsKeyDown(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L87) | Determines whether a key is currently held. |
| [`static bool Inno.Input.Input.IsMouseButtonDown(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L120) | Determines whether a pointer button is currently held. |
| [`static bool Inno.Input.Input.WasKeyPressed(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L98) | Determines whether a key was newly pressed this frame. |
| [`static bool Inno.Input.Input.WasKeyReleased(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L109) | Determines whether a key was newly released this frame. |
| [`static bool Inno.Input.Input.WasMouseButtonPressed(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L131) | Determines whether a pointer button was newly pressed this frame. |
| [`static bool Inno.Input.Input.WasMouseButtonReleased(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L142) | Determines whether a pointer button was newly released this frame. |

### `Inno.Input.InputExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.InputExecutionContext`](../../src/services/input/Inno.Input/Input.cs#L13) | Binds one input service to the current asynchronous execution context. |
| [`static Inno.Input.IInputService Inno.Input.InputExecutionContext.current`](../../src/services/input/Inno.Input/Input.cs#L23) | Gets the input service bound to the current execution context. |
| [`static System.IDisposable Inno.Input.InputExecutionContext.EnterScope(Inno.Input.IInputService input)`](../../src/services/input/Inno.Input/Input.cs#L45) | Binds an input service until the returned strict last-in-first-out scope is disposed. |
| [`static bool Inno.Input.InputExecutionContext.TryGet(out Inno.Input.IInputService? input)`](../../src/services/input/Inno.Input/Input.cs#L34) | Tries to read input when a host has installed an input subsystem for this frame. |

### `Inno.Input.InputSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier Inno.Input.InputSnapshot.modifiers`](../../src/services/input/Inno.Input/InputSnapshot.cs#L120) | Gets the active keyboard modifier mask at this frame boundary. |
| [`Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.mouseDelta`](../../src/services/input/Inno.Input/InputSnapshot.cs#L110) | Gets pointer movement accumulated since the previous snapshot. |
| [`Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.mousePosition`](../../src/services/input/Inno.Input/InputSnapshot.cs#L105) | Gets the current pointer position in window coordinates. |
| [`Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.scrollDelta`](../../src/services/input/Inno.Input/InputSnapshot.cs#L115) | Gets wheel movement accumulated since the previous snapshot. |
| [`Inno.Input.InputSnapshot`](../../src/services/input/Inno.Input/InputSnapshot.cs#L13) | Captures immutable keyboard and pointer state at one runtime frame boundary. |
| [`Inno.Input.InputSnapshot.InputSnapshot(long frameIndex, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysDown = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysPressed = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysReleased = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsDown = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsPressed = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsReleased = null, Inno.Core.Mathematics.Vector2 mousePosition = default(Inno.Core.Mathematics.Vector2), Inno.Core.Mathematics.Vector2 mouseDelta = default(Inno.Core.Mathematics.Vector2), Inno.Core.Mathematics.Vector2 scrollDelta = default(Inno.Core.Mathematics.Vector2), Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, System.Collections.Generic.IEnumerable<string>? textInput = null)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L62) | Creates a complete immutable input snapshot. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Input.InputSnapshot.keysPressed`](../../src/services/input/Inno.Input/InputSnapshot.cs#L130) | Gets the physical keys newly pressed during this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Input.InputSnapshot.keysReleased`](../../src/services/input/Inno.Input/InputSnapshot.cs#L135) | Gets the physical keys newly released during this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Input.InputSnapshot.mouseButtonsPressed`](../../src/services/input/Inno.Input/InputSnapshot.cs#L140) | Gets the pointer buttons newly pressed during this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Input.InputSnapshot.mouseButtonsReleased`](../../src/services/input/Inno.Input/InputSnapshot.cs#L145) | Gets the pointer buttons newly released during this frame. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Input.InputSnapshot.textInput`](../../src/services/input/Inno.Input/InputSnapshot.cs#L125) | Gets the ordered UTF-8 text commits captured during this frame. |
| [`bool Inno.Input.InputSnapshot.IsKeyDown(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L156) | Determines whether a key is currently held. |
| [`bool Inno.Input.InputSnapshot.IsMouseButtonDown(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L189) | Determines whether a pointer button is currently held. |
| [`bool Inno.Input.InputSnapshot.WasKeyPressed(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L167) | Determines whether a key was newly pressed during this frame. |
| [`bool Inno.Input.InputSnapshot.WasKeyReleased(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L178) | Determines whether a key was newly released during this frame. |
| [`bool Inno.Input.InputSnapshot.WasMouseButtonPressed(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L200) | Determines whether a pointer button was newly pressed during this frame. |
| [`bool Inno.Input.InputSnapshot.WasMouseButtonReleased(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L211) | Determines whether a pointer button was newly released during this frame. |
| [`long Inno.Input.InputSnapshot.frameIndex`](../../src/services/input/Inno.Input/InputSnapshot.cs#L100) | Gets the runtime frame that owns this snapshot. |
| [`static Inno.Input.InputSnapshot Inno.Input.InputSnapshot.empty`](../../src/services/input/Inno.Input/InputSnapshot.cs#L95) | Gets an empty initial snapshot. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
