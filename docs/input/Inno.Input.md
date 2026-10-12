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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Input.IInputBackend`

<!-- api: T:Inno.Input.IInputBackend b1e1051e943fc51d8b0a28ec6a7b38bee2adb47cc0ce08c6d9b8393edc11d875 -->
<!-- api: M:Inno.Input.IInputBackend.Capture(System.Int64) 0bc590c9b4f7bd8f5ecf4a2affa018890f421e810be056481fa38312b7b2d2c7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e4f02ea3a5cc325d"></a>[`Inno.Input.InputSnapshot Inno.Input.IInputBackend.Capture(long frameIndex)`](../../src/services/input/Inno.Input/IInputBackend.cs#L19) | Captures a snapshot and consumes transient press, release, movement, and wheel state.<br>返回：The immutable state for that exact frame.<br>`frameIndex`：The runtime frame receiving the snapshot. | — | — |
| <a id="api-990721dedc077995"></a>[`public interface Inno.Input.IInputBackend : System.IDisposable`](../../src/services/input/Inno.Input/IInputBackend.cs#L8) | Captures complete input state from one platform backend at frame boundaries. | — | — |

### `Inno.Input.IInputService`

<!-- api: T:Inno.Input.IInputService fc9aa1da981f6aa8d1ac766e9667abe835c1512191847db064a7f891bda022ea -->
<!-- api: P:Inno.Input.IInputService.snapshot 9b753cd957ff9ac343dc8d54c5999717f99350de6a5038d8857acdf1bd5f69b3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e76435d5ccb6ac16"></a>[`Inno.Input.InputSnapshot Inno.Input.IInputService.snapshot { get; }`](../../src/services/input/Inno.Input/IInputService.cs#L11) | Gets the snapshot captured at the current frame boundary. | — | — |
| <a id="api-bf368dad86821e50"></a>[`public interface Inno.Input.IInputService`](../../src/services/input/Inno.Input/IInputService.cs#L6) | Exposes the current immutable input snapshot to runtime systems and scripts. | — | — |

### `Inno.Input.Input`

<!-- api: T:Inno.Input.Input 6c75e5d80480dc656466ece0cfe0078fefe53f4fd26adc96ec646912eb069cf0 -->
<!-- api: P:Inno.Input.Input.snapshot a9973b0ec12cc76e9f6b4c08aa9ae15bef5971348f95b6619e3497d70e012a03 -->
<!-- api: P:Inno.Input.Input.mousePosition 66ec0c6f4a5421465d46ebebed0a475293fd442bc753967a6bd64dea82532576 -->
<!-- api: P:Inno.Input.Input.mouseDelta ff41dafc514f387c1ec2f16b9c26df7aafa5b3ae1557a1807ec9238411f45e8e -->
<!-- api: P:Inno.Input.Input.scrollDelta b5f2676fb0a22653a975ba2377d75557def0c8f09373bad4dc71f3b9b6de2e1b -->
<!-- api: M:Inno.Input.Input.IsKeyDown(Inno.Core.Input.KeyCode) 5a42a8d3040b324a10719a3e3e7fea1230d5c3aca7aa05a8a3efe61213907bce -->
<!-- api: M:Inno.Input.Input.WasKeyPressed(Inno.Core.Input.KeyCode) 722ad5cf2f4a00c742732f9b21beb333e07ff4aaa94ab528d5666f59ad13e4c7 -->
<!-- api: M:Inno.Input.Input.WasKeyReleased(Inno.Core.Input.KeyCode) 546d71daa74d65efdfc5ce9163f51cb7ab4f230dc5d33077f6b7fe6642df1dc8 -->
<!-- api: M:Inno.Input.Input.IsMouseButtonDown(Inno.Core.Input.MouseButton) 2f87e615d67b4a9470740ff8a93acd0765d7d6ad46ed72b43901b4a84b277ef7 -->
<!-- api: M:Inno.Input.Input.WasMouseButtonPressed(Inno.Core.Input.MouseButton) ba84bd97937ac97f27950de2489821f8aa5641fa5e3f5aac41d7bec16c13c1ea -->
<!-- api: M:Inno.Input.Input.WasMouseButtonReleased(Inno.Core.Input.MouseButton) 0d18e9a388c7a492b0a5cc27845173ae4eb537309047aca13cde7f856a0702f3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fd1fe60a0434f87e"></a>[`public static bool Inno.Input.Input.IsKeyDown(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L87) | Determines whether a key is currently held.<br>返回：true when the key is held.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-bb77460a31eeef1f"></a>[`public static bool Inno.Input.Input.IsMouseButtonDown(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L120) | Determines whether a pointer button is currently held.<br>返回：true when the button is held.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-e9c246048d20b088"></a>[`public static bool Inno.Input.Input.WasKeyPressed(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L98) | Determines whether a key was newly pressed this frame.<br>返回：true when the key transitioned to down.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-662c780c462e18f2"></a>[`public static bool Inno.Input.Input.WasKeyReleased(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/Input.cs#L109) | Determines whether a key was newly released this frame.<br>返回：true when the key transitioned to up.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-a4e14809466d53d5"></a>[`public static bool Inno.Input.Input.WasMouseButtonPressed(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L131) | Determines whether a pointer button was newly pressed this frame.<br>返回：true when the button transitioned to down.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-d43433d07bf4997f"></a>[`public static bool Inno.Input.Input.WasMouseButtonReleased(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/Input.cs#L142) | Determines whether a pointer button was newly released this frame.<br>返回：true when the button transitioned to up.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-ea69a7c4948fa711"></a>[`public static Inno.Core.Mathematics.Vector2 Inno.Input.Input.mouseDelta { get; }`](../../src/services/input/Inno.Input/Input.cs#L71) | Gets pointer movement accumulated during this frame. | — | — |
| <a id="api-7763b67b3a6bbb70"></a>[`public static Inno.Core.Mathematics.Vector2 Inno.Input.Input.mousePosition { get; }`](../../src/services/input/Inno.Input/Input.cs#L66) | Gets the current pointer position in window coordinates. | — | — |
| <a id="api-67b896291365de1b"></a>[`public static Inno.Core.Mathematics.Vector2 Inno.Input.Input.scrollDelta { get; }`](../../src/services/input/Inno.Input/Input.cs#L76) | Gets wheel movement accumulated during this frame. | — | — |
| <a id="api-e39d5bda6d54431f"></a>[`public static Inno.Input.InputSnapshot Inno.Input.Input.snapshot { get; }`](../../src/services/input/Inno.Input/Input.cs#L61) | Gets the complete immutable current-frame snapshot. | — | — |
| <a id="api-297f3ef80d095b3e"></a>[`public static class Inno.Input.Input`](../../src/services/input/Inno.Input/Input.cs#L56) | Provides script-friendly access to the current frame's input snapshot. | — | — |

### `Inno.Input.InputExecutionContext`

<!-- api: T:Inno.Input.InputExecutionContext 76f0d1a7520d2b9c4aa9d31e9c38180aaa27651cd964d16226b483ae3a84836a -->
<!-- api: P:Inno.Input.InputExecutionContext.current 418ed6b958d95472009d390e8f163cd45d66d11160ea38ba68683355464c867e -->
<!-- api: M:Inno.Input.InputExecutionContext.TryGet(Inno.Input.IInputService@) 1f76ef4b4f9de4a53b0a50d02d9ef79a8987735a5249c91c43148fa4d60d2d38 -->
<!-- api: M:Inno.Input.InputExecutionContext.EnterScope(Inno.Input.IInputService) 2365f5b098710f3e56639c267b82f3370261355334396de02c2edbfc972489fb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1951673d9de5925d"></a>[`public static System.IDisposable Inno.Input.InputExecutionContext.EnterScope(Inno.Input.IInputService input)`](../../src/services/input/Inno.Input/Input.cs#L45) | Binds an input service until the returned strict last-in-first-out scope is disposed.<br>返回：A strict last-in-first-out execution scope.<br>`input`：The host-owned input service. | — | — |
| <a id="api-ca5afd1c01ee0194"></a>[`public static bool Inno.Input.InputExecutionContext.TryGet(out Inno.Input.IInputService? input)`](../../src/services/input/Inno.Input/Input.cs#L34) | Tries to read input when a host has installed an input subsystem for this frame.<br>返回：Whether a live input service is bound.<br>`input`：Bound service, or null when unavailable. | — | — |
| <a id="api-31b6aa69f06eca5c"></a>[`public static Inno.Input.IInputService Inno.Input.InputExecutionContext.current { get; }`](../../src/services/input/Inno.Input/Input.cs#L23) | Gets the input service bound to the current execution context. | — | `System.InvalidOperationException`：Thrown when no input scope is active. |
| <a id="api-626f3678b873994f"></a>[`public static class Inno.Input.InputExecutionContext`](../../src/services/input/Inno.Input/Input.cs#L13) | Binds one input service to the current asynchronous execution context. | — | — |

### `Inno.Input.InputSnapshot`

<!-- api: T:Inno.Input.InputSnapshot 5eff2acf31902d9cc2724ee51fb276e355310ad654f72276a00fda3f0b02758c -->
<!-- api: M:Inno.Input.InputSnapshot.#ctor(System.Int64,System.Collections.Generic.IEnumerable{Inno.Core.Input.KeyCode},System.Collections.Generic.IEnumerable{Inno.Core.Input.KeyCode},System.Collections.Generic.IEnumerable{Inno.Core.Input.KeyCode},System.Collections.Generic.IEnumerable{Inno.Core.Input.MouseButton},System.Collections.Generic.IEnumerable{Inno.Core.Input.MouseButton},System.Collections.Generic.IEnumerable{Inno.Core.Input.MouseButton},Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2,Inno.Core.Input.KeyModifier,System.Collections.Generic.IEnumerable{System.String}) a8b64f6a598dfeea18deb0ff3a98b8260b63c2e8957aa6acd8ec863d7d3b5d00 -->
<!-- api: P:Inno.Input.InputSnapshot.empty f2ccd24494786f7be411203b7736615a5b0afd7d01b86d57104918c96b5e84cb -->
<!-- api: P:Inno.Input.InputSnapshot.frameIndex 3ac9f077995161e0d790c7934b952d72b72b25ac939298109440b5147800271a -->
<!-- api: P:Inno.Input.InputSnapshot.mousePosition 773319159770ce92fb9c0d9d19c1099f5f80b1e3eed3b77f6f584c6188954455 -->
<!-- api: P:Inno.Input.InputSnapshot.mouseDelta a432926264b730edeb6459cecf666c4f815c6cf1e774d3f3a229c2c39df90f15 -->
<!-- api: P:Inno.Input.InputSnapshot.scrollDelta 506b2a3b203da41e3a30d08d60617156c4c37c107f9309b0302cc651f055b676 -->
<!-- api: P:Inno.Input.InputSnapshot.modifiers a3b5a049056806401c1cdc27192c93ce856cc16583fcd0651b8320ab59e5eb7e -->
<!-- api: P:Inno.Input.InputSnapshot.textInput 2e2ea8ad08a4f1ac983e1bddd671f64a54b4c1545a9614a8004824040db7c83f -->
<!-- api: P:Inno.Input.InputSnapshot.keysPressed 18d02ca6fe4ce4a5d01af5c8520bdfac7746e10e66945ad07fc315aaabff7318 -->
<!-- api: P:Inno.Input.InputSnapshot.keysReleased 2022c97bc84ed987c634003503050297e36d8ac7d4b3bbee5caa876421e56b23 -->
<!-- api: P:Inno.Input.InputSnapshot.mouseButtonsPressed 55c7d1a601a06fdd203ebdbc0350a300091771dcc338ca3cd1e32c0b0c2ab54d -->
<!-- api: P:Inno.Input.InputSnapshot.mouseButtonsReleased 4bb328052d1f5f78175cf43972873c30fe68bd2303d78223d54aee82e72e4961 -->
<!-- api: M:Inno.Input.InputSnapshot.IsKeyDown(Inno.Core.Input.KeyCode) b3f3d5d5422c93ff8fb0ddf0a8066f9a21d349abe4d3a417adc786c13943b6cc -->
<!-- api: M:Inno.Input.InputSnapshot.WasKeyPressed(Inno.Core.Input.KeyCode) 657210378d9196d770b3bfd817f662b10a4677ad6b1d4ba74acecd2cc12639b4 -->
<!-- api: M:Inno.Input.InputSnapshot.WasKeyReleased(Inno.Core.Input.KeyCode) 33239b20ed159dbb6edfc8d00f982e9d41c0e14c299aef10f533af96dd1d77cf -->
<!-- api: M:Inno.Input.InputSnapshot.IsMouseButtonDown(Inno.Core.Input.MouseButton) 5b3cd51b82ccd338f7c48127d3bf22bef6ad7faa176c4c5bdaba667bdc2c6960 -->
<!-- api: M:Inno.Input.InputSnapshot.WasMouseButtonPressed(Inno.Core.Input.MouseButton) b54779d383e7079ea1247806850e1d9a95de506f2fa15994690bf1a646334c9e -->
<!-- api: M:Inno.Input.InputSnapshot.WasMouseButtonReleased(Inno.Core.Input.MouseButton) 152280991afded45ac7136f549d328b22132d6d24d926a8c320f8db26cd8bcbb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c95e45587772040f"></a>[`public Inno.Input.InputSnapshot.InputSnapshot(long frameIndex, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysDown = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysPressed = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.KeyCode>? keysReleased = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsDown = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsPressed = null, System.Collections.Generic.IEnumerable<Inno.Core.Input.MouseButton>? mouseButtonsReleased = null, Inno.Core.Mathematics.Vector2 mousePosition = default(Inno.Core.Mathematics.Vector2), Inno.Core.Mathematics.Vector2 mouseDelta = default(Inno.Core.Mathematics.Vector2), Inno.Core.Mathematics.Vector2 scrollDelta = default(Inno.Core.Mathematics.Vector2), Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, System.Collections.Generic.IEnumerable<string>? textInput = null)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L62) | Creates a complete immutable input snapshot.<br>`frameIndex`：The runtime frame that owns the snapshot.<br>`keysDown`：Keys held at the frame boundary.<br>`keysPressed`：Keys newly pressed since the previous snapshot.<br>`keysReleased`：Keys newly released since the previous snapshot.<br>`mouseButtonsDown`：Pointer buttons held at the frame boundary.<br>`mouseButtonsPressed`：Pointer buttons newly pressed since the previous snapshot.<br>`mouseButtonsReleased`：Pointer buttons newly released since the previous snapshot.<br>`mousePosition`：Current pointer position in window coordinates.<br>`mouseDelta`：Pointer movement accumulated since the previous snapshot.<br>`scrollDelta`：Wheel movement accumulated since the previous snapshot.<br>`modifiers`：Active keyboard modifiers at the frame boundary.<br>`textInput`：Ordered Unicode text commits captured since the previous snapshot. | — | — |
| <a id="api-d5bb346610d28635"></a>[`public bool Inno.Input.InputSnapshot.IsKeyDown(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L156) | Determines whether a key is currently held.<br>返回：true when the key is held.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-2c48955d7588600b"></a>[`public bool Inno.Input.InputSnapshot.IsMouseButtonDown(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L189) | Determines whether a pointer button is currently held.<br>返回：true when the button is held.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-c39fcc823212dc84"></a>[`public bool Inno.Input.InputSnapshot.WasKeyPressed(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L167) | Determines whether a key was newly pressed during this frame.<br>返回：true when the key transitioned to down.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-1eb55aa315473f2d"></a>[`public bool Inno.Input.InputSnapshot.WasKeyReleased(Inno.Core.Input.KeyCode key)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L178) | Determines whether a key was newly released during this frame.<br>返回：true when the key transitioned to up.<br>`key`：The backend-neutral physical key. | — | — |
| <a id="api-eb9ccd48efdfb5a2"></a>[`public bool Inno.Input.InputSnapshot.WasMouseButtonPressed(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L200) | Determines whether a pointer button was newly pressed during this frame.<br>返回：true when the button transitioned to down.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-3559726883b45532"></a>[`public bool Inno.Input.InputSnapshot.WasMouseButtonReleased(Inno.Core.Input.MouseButton button)`](../../src/services/input/Inno.Input/InputSnapshot.cs#L211) | Determines whether a pointer button was newly released during this frame.<br>返回：true when the button transitioned to up.<br>`button`：The backend-neutral pointer button. | — | — |
| <a id="api-f73fd16e95a3c4f1"></a>[`public static Inno.Input.InputSnapshot Inno.Input.InputSnapshot.empty { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L95) | Gets an empty initial snapshot. | — | — |
| <a id="api-b20af045199099aa"></a>[`public long Inno.Input.InputSnapshot.frameIndex { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L100) | Gets the runtime frame that owns this snapshot. | — | — |
| <a id="api-7886892514333c2d"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Input.InputSnapshot.keysPressed { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L130) | Gets the physical keys newly pressed during this frame. | — | — |
| <a id="api-25f5787f0e8e5e76"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Input.InputSnapshot.keysReleased { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L135) | Gets the physical keys newly released during this frame. | — | — |
| <a id="api-fa0446ab0a6bbd16"></a>[`public Inno.Core.Input.KeyModifier Inno.Input.InputSnapshot.modifiers { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L120) | Gets the active keyboard modifier mask at this frame boundary. | — | — |
| <a id="api-4f3226b19f7909d3"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Input.InputSnapshot.mouseButtonsPressed { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L140) | Gets the pointer buttons newly pressed during this frame. | — | — |
| <a id="api-4a2b431c63ec43b2"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Input.InputSnapshot.mouseButtonsReleased { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L145) | Gets the pointer buttons newly released during this frame. | — | — |
| <a id="api-9e6bda69f4fc365a"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.mouseDelta { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L110) | Gets pointer movement accumulated since the previous snapshot. | — | — |
| <a id="api-3e952018362314df"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.mousePosition { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L105) | Gets the current pointer position in window coordinates. | — | — |
| <a id="api-907bc25cf89324e8"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Input.InputSnapshot.scrollDelta { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L115) | Gets wheel movement accumulated since the previous snapshot. | — | — |
| <a id="api-19780866cf37c9bd"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Input.InputSnapshot.textInput { get; }`](../../src/services/input/Inno.Input/InputSnapshot.cs#L125) | Gets the ordered UTF-8 text commits captured during this frame. | — | — |
| <a id="api-61d5c3d11ee69c9d"></a>[`public sealed class Inno.Input.InputSnapshot`](../../src/services/input/Inno.Input/InputSnapshot.cs#L13) | Captures immutable keyboard and pointer state at one runtime frame boundary. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
