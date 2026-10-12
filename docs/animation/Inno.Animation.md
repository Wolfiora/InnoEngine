# Inno.Animation

[Animation 索引](README.md) · [Runtime](Inno.Animation.Runtime.md) · [Wiki 首页](../README.md)

`Inno.Animation` 不引用 Scene、Rendering、Audio、Editor 或具体 backend。它定义可由 2D、3D、UI、Camera 或音频参数共同消费的值采样协议。

## 公开 API

| API | 稳定语义 |
| --- | --- |
| `AnimationClipAsset` | 带 Stable Type ID 的 duration、tracks 与 marker 集合。 |
| `AnimationBindingId` / `IAnimationBindingSink` | 以开放稳定 ID 接收最终 blended sample，不反射写属性。 |
| `AnimationTarget` | 使用 Identity 的原 runtime slot 定位目标；不同目标独立混合；纯采样显式选择 samplingOnly。 |
| `AnimationBindingProviderAttribute` / `AnimationBindingProvider` | 按稳定 binding ID/value kind 发现控制线程适配，不内建 Transform 或 Sprite。 |
| `AnimationValue` / `AnimationValueKind` | Scalar、Vector2/3/4 与 normalized Quaternion。 |
| `AnimationTrack` / `AnimationKeyframe` | Step/Linear 时间采样。 |
| `AnimationPlayOptions` | scaled/unscaled clock、speed、loop、layer、weight。 |
| `AnimationPlaybackHandle` | Runtime generation 校验的 opaque handle。 |
| `IAnimationService` / `Animation` | 显式基础设施边界与脚本 façade。 |
| `AnimationMarkerEvent` | 主线程 EventDispatcher 上的 stable event ID 与中立 payload。 |

```csharp
using InnoEngine.Animation;

static AnimationPlaybackHandle StartLoop(
    AnimationClipAsset clip,
    AnimationTarget target
) => Animation.Play(clip, target, new AnimationPlayOptions
    {
        speed = 1f,
        weight = 1f,
        layer = 0,
        loop = true,
        clock = AnimationClock.Scaled
    });
```

Clip 在 import/play 前严格验证。自然完成或显式 Stop 后句柄立即 stale，不能控制后来复用同一 slot 的播放。

[下一页：Inno.Animation.Runtime](Inno.Animation.Runtime.md)

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Animation.Animation`

<!-- api: T:Inno.Animation.Animation d6b807f61686173b91b4af76f07dbab0cccff7f3d7a23ed4ff317b174fdb5a1d -->
<!-- api: M:Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget) 8badee5bc29268c0e3b224ef836a36a98583c4e1abf51de9c28178e1432af357 -->
<!-- api: M:Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget,Inno.Animation.AnimationPlayOptions) 2022bf28cd3b7a227c1ea95b93035d613f88655513791b129c14b025d91bcb87 -->
<!-- api: M:Inno.Animation.Animation.Stop(Inno.Animation.AnimationPlaybackHandle) f6c109ef29eee1fd7a4d5b74d00569808f213dc3608a3756fc71c98e65da79a3 -->
<!-- api: M:Inno.Animation.Animation.Pause(Inno.Animation.AnimationPlaybackHandle) 9790f3506faaccf1966d6d6f25b498bf50912fca9538dd3b7c5c598ca4708aca -->
<!-- api: M:Inno.Animation.Animation.Resume(Inno.Animation.AnimationPlaybackHandle) d39670da36d3e1fc8816028cf96d299576550358aca8306e1429e55da64ed298 -->
<!-- api: M:Inno.Animation.Animation.Seek(Inno.Animation.AnimationPlaybackHandle,System.Single) 9be9b4921b589d66df8c4ef2732fd3c6b6bc25df5486ef0282bb44d6be21b5bc -->
<!-- api: M:Inno.Animation.Animation.TryGetState(Inno.Animation.AnimationPlaybackHandle,Inno.Animation.AnimationPlaybackState@,System.Single@) dc509a7d28777738faf0fcd2025d587166c681620f5e5f9f8dd61879e22d4655 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d6898af03783ed01"></a>[`public static bool Inno.Animation.Animation.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L101) | Pauses one live playback.<br>返回：true when the state changed.<br>`handle`：Playback handle to pause. | — | — |
| <a id="api-b8b4c8ff4639ca46"></a>[`public static Inno.Animation.AnimationPlaybackHandle Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation/Animation.cs#L74) | Starts one clip with explicit playback options.<br>返回：A generation-checked playback handle.<br>`clip`：Valid imported animation clip.<br>`options`：Playback and blend options.<br>`target`：The weak destination or explicit sampling-only policy. | — | — |
| <a id="api-d09b1f146067f302"></a>[`public static Inno.Animation.AnimationPlaybackHandle Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation/Animation.cs#L53) | Starts one clip with default playback options.<br>返回：A generation-checked playback handle.<br>`clip`：Valid imported animation clip.<br>`target`：The weak destination or explicit sampling-only policy. | — | — |
| <a id="api-df14354916f4e7e7"></a>[`public static bool Inno.Animation.Animation.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L112) | Resumes one paused playback.<br>返回：true when the state changed.<br>`handle`：Playback handle to resume. | — | — |
| <a id="api-67496c29376285f1"></a>[`public static bool Inno.Animation.Animation.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation/Animation.cs#L126) | Seeks one live playback to a clip-local time.<br>返回：true when the handle was live.<br>`handle`：Playback handle to seek.<br>`time`：Clip-local time in seconds. | — | — |
| <a id="api-7f0590922735ab7f"></a>[`public static bool Inno.Animation.Animation.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L90) | Stops one live playback.<br>返回：true when the playback was live.<br>`handle`：Playback handle to stop. | — | — |
| <a id="api-90ca906e0bf6f9f4"></a>[`public static bool Inno.Animation.Animation.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation/Animation.cs#L146) | Tries to read one live playback's state and position.<br>返回：true when the handle was live.<br>`handle`：Playback handle to query.<br>`state`：Receives current playback state.<br>`time`：Receives clip-local time in seconds. | — | — |
| <a id="api-b9e99d263b9f29a1"></a>[`public static class Inno.Animation.Animation`](../../src/content/animation/Inno.Animation/Animation.cs#L39) | Provides script-friendly animation control through the current runtime context. | — | — |

### `Inno.Animation.AnimationBindingId`

<!-- api: T:Inno.Animation.AnimationBindingId cb8f18552343e24429d040ebd8dd2c2f345cdb1a8d4fadde0ea9d2c9557bc4e2 -->
<!-- api: M:Inno.Animation.AnimationBindingId.#ctor(System.String) 8bd7feabe901a39e92f84e9cf9e4f97ba02c4da5f9e42369f89d76daf156332b -->
<!-- api: P:Inno.Animation.AnimationBindingId.value 5efff6a9d21fc5887867bd0bd5c14d0aca823a60587a6ad1668bc8a6c87f55f6 -->
<!-- api: P:Inno.Animation.AnimationBindingId.isValid 25394b6d72126056239de78853faea543c37e8de2899e767e2975f06f726cc30 -->
<!-- api: M:Inno.Animation.AnimationBindingId.ToString f53dae12cf5e230bda4f73891c0b8514a56b969f83f0ad911e8da01fe61e1306 -->
<!-- api: M:Inno.Animation.AnimationBindingId.op_Inequality(Inno.Animation.AnimationBindingId,Inno.Animation.AnimationBindingId) e2f2cd57651eb15b6a9a07936a84401f4a8dc1f9d12f068e32b4393028c92393 -->
<!-- api: M:Inno.Animation.AnimationBindingId.op_Equality(Inno.Animation.AnimationBindingId,Inno.Animation.AnimationBindingId) cc2243a16a4d7af3e891d8ec76209034047afb97b361bc4322c6689cede66997 -->
<!-- api: M:Inno.Animation.AnimationBindingId.GetHashCode 0ab2126c9809097555622fbbcc89d6c3572652ad1a9824b86ddd4c72af3e573e -->
<!-- api: M:Inno.Animation.AnimationBindingId.Equals(System.Object) 2958190f3ab9826310e835e7806d0e6b2ccc18dece02c77dff854fadda29c58a -->
<!-- api: M:Inno.Animation.AnimationBindingId.Equals(Inno.Animation.AnimationBindingId) 7363d85eacfff8f671a0e79d66b6293f82b6e93bcc0bdb79ebf23dcdfa6c1cd0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-57e7e88506476aa7"></a>[`public Inno.Animation.AnimationBindingId.AnimationBindingId(string value)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L18) | Creates a stable animation binding identifier.<br>`value`：The non-empty binding protocol value. | — | — |
| <a id="api-29559735baf00590"></a>[`public bool Inno.Animation.AnimationBindingId.Equals(Inno.Animation.AnimationBindingId other)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-04d0e1b0b50d59a8"></a>[`public override bool Inno.Animation.AnimationBindingId.Equals(object obj)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-903b2aebfd854f60"></a>[`public override int Inno.Animation.AnimationBindingId.GetHashCode()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-a81432329ffd5d77"></a>[`public static bool Inno.Animation.AnimationBindingId.operator ==(Inno.Animation.AnimationBindingId left, Inno.Animation.AnimationBindingId right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-413eb5f1c2bbcdae"></a>[`public static bool Inno.Animation.AnimationBindingId.operator !=(Inno.Animation.AnimationBindingId left, Inno.Animation.AnimationBindingId right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-712a64403e03dfce"></a>[`public override string Inno.Animation.AnimationBindingId.ToString()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L40) | Formats this identifier for diagnostics and persistence.<br>返回：The protocol value, or an empty string for an uninitialized identifier. | — | — |
| <a id="api-53b2f6ab5d1a144a"></a>[`public bool Inno.Animation.AnimationBindingId.isValid { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L32) | Gets whether this identifier contains a usable value. | — | — |
| <a id="api-823e30be1469213c"></a>[`public string Inno.Animation.AnimationBindingId.value { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L27) | Gets the stable binding protocol value. | — | — |
| <a id="api-79eecd8d528b9573"></a>[`public readonly record struct Inno.Animation.AnimationBindingId : System.IEquatable<Inno.Animation.AnimationBindingId>`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | Identifies one backend-neutral value destination inside an animation binding scope. | — | — |

### `Inno.Animation.AnimationBindingProvider`

<!-- api: T:Inno.Animation.AnimationBindingProvider 7e9ec26856b08ecdbbd46bca7c8e953c51b7f480a7c6c01cb4ae766ab64da95e -->
<!-- api: M:Inno.Animation.AnimationBindingProvider.TryApply(Inno.Animation.AnimationTarget,Inno.Animation.AnimationValue) a98572e06e5dc46f1af434e1fe2baa9f5d31790ed124ffe23478e4ce603a9eb6 -->
<!-- api: M:Inno.Animation.AnimationBindingProvider.Dispose 670e09aaba7f366e0643dc6e6d12cba15fdfb1022015c5673d02b3584b951d53 -->
<!-- api: M:Inno.Animation.AnimationBindingProvider.#ctor 6cd36db771ec53de244995963225f180833335e63acdf9d8617622ccd42aaa12 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2766b301b380bf05"></a>[`protected Inno.Animation.AnimationBindingProvider.AnimationBindingProvider()`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5058185ff1268aad"></a>[`public virtual void Inno.Animation.AnimationBindingProvider.Dispose()`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L30) | Releases subscriptions and resources owned by this extension generation. | — | — |
| <a id="api-a7b4911ad1e23f45"></a>[`public abstract bool Inno.Animation.AnimationBindingProvider.TryApply(Inno.Animation.AnimationTarget target, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L22) | Attempts to apply a value to a destination resolved through Identity.<br>返回：True when applied; false when the destination is currently missing or incompatible.<br>`target`：The weak, generation-qualified destination.<br>`value`：The final blended value for this provider's declared protocol. | — | — |
| <a id="api-6c13501789caaf93"></a>[`public abstract class Inno.Animation.AnimationBindingProvider : System.IDisposable`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L8) | Applies one declared binding protocol on the owner thread; implementations never run in native callbacks. | — | — |

### `Inno.Animation.AnimationBindingProviderAttribute`

<!-- api: T:Inno.Animation.AnimationBindingProviderAttribute 1a777d8f4e9dcbb199a3e20c0a4407a9c28f74f5d49bc59d80dda1c70cac5550 -->
<!-- api: M:Inno.Animation.AnimationBindingProviderAttribute.#ctor(System.String,System.String,Inno.Animation.AnimationValueKind) 7d33e9b2c80b397fb18148997d182541484b4db002bab24d44b45615a3b3ab7b -->
<!-- api: P:Inno.Animation.AnimationBindingProviderAttribute.id ac299753e49995ae790e9da4b4b74d173e0493b6be15f9c7ec345d213b79fa00 -->
<!-- api: P:Inno.Animation.AnimationBindingProviderAttribute.bindingId 43b896a6f6f80c8e5e5da05ef3a8fdea864a9288f9a6f4a19c7bcf8024837b8a -->
<!-- api: P:Inno.Animation.AnimationBindingProviderAttribute.kind 3dacb4ba54210dcb46f3c9a081c176ffbf03b12950dfd48092b31ab69fb8046c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-39d2158c585c1815"></a>[`public Inno.Animation.AnimationBindingProviderAttribute.AnimationBindingProviderAttribute(string id, string bindingId, Inno.Animation.AnimationValueKind kind)`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L26) | Declares one provider and the value protocol it implements.<br>`id`：Globally unique stable extension identifier.<br>`bindingId`：Stable track binding protocol, independent of CLR type names.<br>`kind`：The accepted value representation. | — | `System.ArgumentException`：An identifier is blank or the value representation is undefined. |
| <a id="api-2678b7318d94966c"></a>[`public Inno.Animation.AnimationBindingId Inno.Animation.AnimationBindingProviderAttribute.bindingId { get; }`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L48) | Gets the supported track binding protocol. | — | — |
| <a id="api-5f19dec9a06bc513"></a>[`public string Inno.Animation.AnimationBindingProviderAttribute.id { get; }`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L43) | Gets the stable provider identifier. | — | — |
| <a id="api-8674742a19bc0822"></a>[`public Inno.Animation.AnimationValueKind Inno.Animation.AnimationBindingProviderAttribute.kind { get; }`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L53) | Gets the accepted value representation. | — | — |
| <a id="api-0b3d72a408bebddc"></a>[`public sealed class Inno.Animation.AnimationBindingProviderAttribute : System.Attribute`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L9) | Declares a stable, automatically discovered animation destination protocol. | — | — |

### `Inno.Animation.AnimationClipAsset`

<!-- api: T:Inno.Animation.AnimationClipAsset acfcbf0569320d969a4d64a84f036d5fdb01d1de32da1e064511a337d470d395 -->
<!-- api: P:Inno.Animation.AnimationClipAsset.duration d15475ed7ffdb22770ed5892af308b345508c2b173317d27a12122fb17745cbd -->
<!-- api: P:Inno.Animation.AnimationClipAsset.tracks 597129f18e36e34a89aa4f80bcd3949a69a841876b2bb25c141f1e48c6449715 -->
<!-- api: P:Inno.Animation.AnimationClipAsset.events dbaecf669e3f3146f96845f2c2444a21600c1fbce1677c3e1cbafe8396e89492 -->
<!-- api: M:Inno.Animation.AnimationClipAsset.Validate facae62e36fd20788399ed031338e2d6837e4f74f6987c9b421adab86e3ad987 -->
<!-- api: M:Inno.Animation.AnimationClipAsset.#ctor 3c19ca3bd1bd1fbb16724829e93a28d52351fa545a2a2fafbe3534626f2e1357 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2b10a43817bc3bb7"></a>[`public Inno.Animation.AnimationClipAsset.AnimationClipAsset()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L150) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-2b12ea72cf4b0820"></a>[`public void Inno.Animation.AnimationClipAsset.Validate()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L176) | Validates the complete clip contract before import or playback. | — | `System.InvalidOperationException`：Thrown when duration, tracks, bindings, values, or event markers are invalid. |
| <a id="api-134d75fe45b17de4"></a>[`public float Inno.Animation.AnimationClipAsset.duration { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L156) | Gets or sets the positive clip duration in seconds. | — | — |
| <a id="api-0d3941c6141b89e6"></a>[`public Inno.Animation.AnimationEventMarker[] Inno.Animation.AnimationClipAsset.events { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L168) | Gets or sets ordered stable event markers. | — | — |
| <a id="api-2937abcf1cebbc6b"></a>[`public Inno.Animation.AnimationTrack[] Inno.Animation.AnimationClipAsset.tracks { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L162) | Gets or sets the ordered backend-neutral value tracks. | — | — |
| <a id="api-06e3aa1cec85c5c7"></a>[`public sealed class Inno.Animation.AnimationClipAsset : Inno.Assets.AssetObject`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L150) | Represents one imported backend-neutral collection of tracks and event markers. | — | — |

### `Inno.Animation.AnimationClock`

<!-- api: T:Inno.Animation.AnimationClock 1da1eaa8506c4f730e672dd4516236bcda43a0569506927047a09e242820fac0 -->
<!-- api: F:Inno.Animation.AnimationClock.Scaled 254db32eeaaaf1e7dab874d08911a5ec8b6694229b47f4179302a2c9b43fecb3 -->
<!-- api: F:Inno.Animation.AnimationClock.Unscaled f1532fc6cd96a880de1073a842d9f9eddc2c43e8b09cbc4a2408f20843d22592 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9b0de08d402dc914"></a>[`Inno.Animation.AnimationClock.Scaled = 0`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L227) | Uses scaled simulation time and stops while the session is paused. | — | — |
| <a id="api-27c0765bfa23121b"></a>[`Inno.Animation.AnimationClock.Unscaled = 1`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L232) | Uses unscaled session time and continues while simulation is paused. | — | — |
| <a id="api-f680ea5bd3dc5443"></a>[`public enum Inno.Animation.AnimationClock : int`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L222) | Selects the session clock used to advance an animation playback. | — | — |

### `Inno.Animation.AnimationEventMarker`

<!-- api: T:Inno.Animation.AnimationEventMarker 02f3e366a1f807a858f5eed83651170a7d5a3aa4ac1327526d223e261e6dff56 -->
<!-- api: P:Inno.Animation.AnimationEventMarker.time 3b3771f6d9cf834b55b9dd187fb470c91e85b9c5fd59e5799fc744875ef0cf60 -->
<!-- api: P:Inno.Animation.AnimationEventMarker.eventId b1b195f3ea9cbdf8b4083424132d489b2abe5ab0bb7a4e4cf8602ceb529db7e0 -->
<!-- api: P:Inno.Animation.AnimationEventMarker.payload d90e0a31fff48126a086fcd67d6bde1be7c5fd21678f904f37cd598b23fed9a9 -->
<!-- api: M:Inno.Animation.AnimationEventMarker.#ctor ad376cd6638b596e7d3220bd2be5326039ea8fe16e1ca567f70247a5a19124d6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0154b8d3be968a61"></a>[`public Inno.Animation.AnimationEventMarker.AnimationEventMarker()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L125) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-a79c156255a35d50"></a>[`public string Inno.Animation.AnimationEventMarker.eventId { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L137) | Gets or sets the stable event protocol identifier. | — | — |
| <a id="api-cfe37f8d3481a023"></a>[`public byte[] Inno.Animation.AnimationEventMarker.payload { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L143) | Gets or sets opaque reload-safe event payload bytes. | — | — |
| <a id="api-9731919301eb361e"></a>[`public float Inno.Animation.AnimationEventMarker.time { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L131) | Gets or sets clip-local marker time in seconds. | — | — |
| <a id="api-aed8a7394db5f07e"></a>[`public sealed class Inno.Animation.AnimationEventMarker : Inno.Core.Serialization.ISerializable`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L125) | Stores one stable event marker embedded in an animation clip. | — | — |

### `Inno.Animation.AnimationExecutionContext`

<!-- api: T:Inno.Animation.AnimationExecutionContext 4a8c6fc414b4f081ddfb8ba246c0a22fab1e4ca03a6f91448356fa378a46ac2d -->
<!-- api: P:Inno.Animation.AnimationExecutionContext.current f9dace5436eec4a5955963066389510655b5b7bb0020c7d188bb1f4c284e05aa -->
<!-- api: M:Inno.Animation.AnimationExecutionContext.EnterScope(Inno.Animation.IAnimationService) 468872396ee43412132c03587f7d1b6352372baf1ad87ba9583cc4bf808d7b33 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8fc9c040c9135be2"></a>[`public static System.IDisposable Inno.Animation.AnimationExecutionContext.EnterScope(Inno.Animation.IAnimationService animation)`](../../src/content/animation/Inno.Animation/Animation.cs#L28) | Binds one animation service until the returned strict scope is disposed.<br>返回：A strict last-in-first-out execution scope.<br>`animation`：Session-owned animation service. | — | — |
| <a id="api-4542bd2f4a79f871"></a>[`public static Inno.Animation.IAnimationService Inno.Animation.AnimationExecutionContext.current { get; }`](../../src/content/animation/Inno.Animation/Animation.cs#L17) | Gets the animation service bound to the current execution context. | — | — |
| <a id="api-ef9667a69906674f"></a>[`public static class Inno.Animation.AnimationExecutionContext`](../../src/content/animation/Inno.Animation/Animation.cs#L10) | Binds one animation service to the current asynchronous execution context. | — | — |

### `Inno.Animation.AnimationInterpolation`

<!-- api: T:Inno.Animation.AnimationInterpolation e9a732aa5f2b6b3beb5291b23c1f256dcdb45ede376e8f17a081dd5f1aa9cfe1 -->
<!-- api: F:Inno.Animation.AnimationInterpolation.Step 754303bb5ac29910837b27d8b05aa7dc0ceaa3d2498e42c156cab1c0c3c60a58 -->
<!-- api: F:Inno.Animation.AnimationInterpolation.Linear c997fd80b9932a14959e97e71038bdd61ba8ad06b6c440a3bfc00844701ac1ec -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d45a2b59a8357bc4"></a>[`Inno.Animation.AnimationInterpolation.Linear = 1`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L216) | Blends linearly between adjacent keyframes. | — | — |
| <a id="api-b9843b8dcebf6dc5"></a>[`Inno.Animation.AnimationInterpolation.Step = 0`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L211) | Holds the previous keyframe value until the next keyframe. | — | — |
| <a id="api-c489157b888d7fcb"></a>[`public enum Inno.Animation.AnimationInterpolation : int`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L206) | Selects how values are sampled between adjacent keyframes. | — | — |

### `Inno.Animation.AnimationKeyframe`

<!-- api: T:Inno.Animation.AnimationKeyframe 6aa02287d5c6ecdd50585dd1b4e0f91c95fe7966557da3b52412423a766885e6 -->
<!-- api: M:Inno.Animation.AnimationKeyframe.#ctor(System.Single,Inno.Animation.AnimationValue) f2e0c925404d277392d991e9712d2e67c0a8cb30443dea7945074e191745e43d -->
<!-- api: P:Inno.Animation.AnimationKeyframe.time c59022ff007d8b37f90b339824761306ef7b69bbceac7125f8acfef971f0612e -->
<!-- api: P:Inno.Animation.AnimationKeyframe.value 7c76956ad07628c39a3e4c9cab84b5139484a13c2ffc920f6297a9d52729f7a0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-224770cf60bef32c"></a>[`public Inno.Animation.AnimationKeyframe.AnimationKeyframe(float time, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L25) | Creates one animation keyframe.<br>`time`：Non-negative clip-local time in seconds.<br>`value`：Value sampled at the keyframe time. | — | — |
| <a id="api-5bd30a951ab23e03"></a>[`public float Inno.Animation.AnimationKeyframe.time { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L39) | Gets or sets clip-local time in seconds. | — | — |
| <a id="api-4640049ffce8eed4"></a>[`public Inno.Animation.AnimationValue Inno.Animation.AnimationKeyframe.value { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L45) | Gets or sets the sampled value. | — | — |
| <a id="api-51d05c27d04423d4"></a>[`public struct Inno.Animation.AnimationKeyframe`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L14) | Stores one timestamped value inside an animation track. | — | — |

### `Inno.Animation.AnimationMarkerEvent`

<!-- api: T:Inno.Animation.AnimationMarkerEvent c465ec72e9db5488dd01500ef69695622c636092d5b7ec7e424270a3a38d34ed -->
<!-- api: M:Inno.Animation.AnimationMarkerEvent.#ctor(Inno.Animation.AnimationPlaybackHandle,System.String,System.ReadOnlyMemory{System.Byte}) 79372b2891ded6b0fbf1d2cdf9665056bb3f981b2bb2f6c91bb0ebc2bce22238 -->
<!-- api: P:Inno.Animation.AnimationMarkerEvent.playback fbe6447ff3dc586cf6a37f6423a10b7ef5b6bfa82e30592e6e53e325a800e082 -->
<!-- api: P:Inno.Animation.AnimationMarkerEvent.eventId 1e95aaddfbd7d517e22cdb1cc656bec0732502c49fabd2d79187184ad704ff69 -->
<!-- api: P:Inno.Animation.AnimationMarkerEvent.payload ec5d23cd0abf9e9d6f3ad84fc4280078106f9468d504f43c8999c5edccf02b94 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-960333d128f18bf7"></a>[`public Inno.Animation.AnimationMarkerEvent.AnimationMarkerEvent(Inno.Animation.AnimationPlaybackHandle playback, string eventId, System.ReadOnlyMemory<byte> payload)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L139) | Creates one immutable animation marker event.<br>`playback`：Playback that crossed the marker.<br>`eventId`：Stable marker protocol identifier.<br>`payload`：Opaque reload-safe marker payload bytes. | — | — |
| <a id="api-05bd9f523f25aaba"></a>[`public string Inno.Animation.AnimationMarkerEvent.eventId { get; }`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L160) | Gets the stable marker protocol identifier. | — | — |
| <a id="api-2e8f0ec8c76f1832"></a>[`public System.ReadOnlyMemory<byte> Inno.Animation.AnimationMarkerEvent.payload { get; }`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L165) | Gets an immutable copy of the marker payload. | — | — |
| <a id="api-8be183e06f7a3748"></a>[`public Inno.Animation.AnimationPlaybackHandle Inno.Animation.AnimationMarkerEvent.playback { get; }`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L155) | Gets the playback that crossed the marker. | — | — |
| <a id="api-b8e550a4d52aab3e"></a>[`public sealed class Inno.Animation.AnimationMarkerEvent : Inno.Core.Events.Event`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L125) | Publishes a stable marker reached by one animation playback on the main runtime thread. | — | — |

### `Inno.Animation.AnimationPlaybackAllocator`

<!-- api: T:Inno.Animation.AnimationPlaybackAllocator 626e5980e5a537531dde4d941ab8a0da0208d288355397adcf3ec79404e48b69 -->
<!-- api: M:Inno.Animation.AnimationPlaybackAllocator.#ctor f2d4ac0f705f3b67d211c5acb4060b1e3158d447a8212de23683ce3cfc03bc2b -->
<!-- api: P:Inno.Animation.AnimationPlaybackAllocator.generation 9f9407ebf450a33f54f6874eaf3b339bd4e38127e575762b1eea63af8d7a9b20 -->
<!-- api: M:Inno.Animation.AnimationPlaybackAllocator.Create(System.Int32,System.UInt32) 0d4977eaf44bcecccdc7b2b71d473abf0def58801cfbc75a6000d95b68e2cd6d -->
<!-- api: M:Inno.Animation.AnimationPlaybackAllocator.Decode(Inno.Animation.AnimationPlaybackHandle) 6b09aa14f155dacc2cecb66aeff81f6294271f8f43d0ead40cf42e360ec7d992 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-72354bf408fc1e73"></a>[`public Inno.Animation.AnimationPlaybackAllocator.AnimationPlaybackAllocator()`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L19) | Creates a process-unique playback owner. | — | `System.InvalidOperationException`：The process exhausted its playback owner identities. |
| <a id="api-e8ea1a45e8b5c49d"></a>[`public Inno.Animation.AnimationPlaybackHandle Inno.Animation.AnimationPlaybackAllocator.Create(int slot, uint slotGeneration)`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L47) | Encodes one owner-managed slot revision into an opaque playback handle.<br>返回：A playback handle that cannot alias a different allocator.<br>`slot`：The non-negative slot owned by the runtime.<br>`slotGeneration`：The non-zero revision used to reject reused slots. | — | `System.ArgumentOutOfRangeException`：The slot or its generation is invalid. |
| <a id="api-3d0cd97ea56e45ea"></a>[`public (int slot, uint generation) Inno.Animation.AnimationPlaybackAllocator.Decode(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L66) | Decodes only handles issued by this owner; slot liveness remains the runtime's responsibility.<br>返回：The slot and revision, or an invalid slot when the owner does not match.<br>`handle`：The opaque playback handle. | — | — |
| <a id="api-4d07d9e38f95390d"></a>[`public uint Inno.Animation.AnimationPlaybackAllocator.generation { get; }`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L30) | Gets the generation shared by handles allocated by this owner. | — | — |
| <a id="api-86fd47b3c29ea82f"></a>[`public sealed class Inno.Animation.AnimationPlaybackAllocator`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L9) | Owns opaque playback identity encoding without requiring a runtime to inherit a fake service base. | — | — |

### `Inno.Animation.AnimationPlaybackHandle`

<!-- api: T:Inno.Animation.AnimationPlaybackHandle bcf0b6656296b6b34c96711dc538b67a522ca2c64f6e041a1a639585c68a01be -->
<!-- api: P:Inno.Animation.AnimationPlaybackHandle.isValid acb0c150b34bca3bc56d0ccdf2d8ac9e67e6c0549894f92fc65f12e8446a9a0a -->
<!-- api: P:Inno.Animation.AnimationPlaybackHandle.runtimeGeneration b4514f547afc75942bbde26c7d31543800146ea1533ed41f9834ab668769b2f4 -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.ToString c4c0a63ab3f81035cac02a8b9239b78e13786b8834e65ecd1f25c7126b150340 -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.op_Inequality(Inno.Animation.AnimationPlaybackHandle,Inno.Animation.AnimationPlaybackHandle) 354cfaceee2a6c0525f3aa2db429901fc623f191bc8a3d049bce58846d8e8428 -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.op_Equality(Inno.Animation.AnimationPlaybackHandle,Inno.Animation.AnimationPlaybackHandle) af246099f1eb4fe1814738d8b956dfd76af70854ebde693c9db75c37fc41c28d -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.GetHashCode 42e1f530f5106dc21f132bcbc9f5f457dfce41be4e4cc13d6846da6ff4664c5e -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.Equals(System.Object) 425941d858442ca7a10153e8b5bae10171364d44a093765a1fa9bd493cad4637 -->
<!-- api: M:Inno.Animation.AnimationPlaybackHandle.Equals(Inno.Animation.AnimationPlaybackHandle) b16d29352f54ad6401313e1a71e1313fe86fb6082c253be9f95ae7fb9e8d5f95 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4e8eef981f06cdea"></a>[`public bool Inno.Animation.AnimationPlaybackHandle.Equals(Inno.Animation.AnimationPlaybackHandle other)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-07ff6740504d2253"></a>[`public override bool Inno.Animation.AnimationPlaybackHandle.Equals(object obj)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-77d805ad4f608fb2"></a>[`public override int Inno.Animation.AnimationPlaybackHandle.GetHashCode()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d7ffbeb2ba6926c3"></a>[`public static bool Inno.Animation.AnimationPlaybackHandle.operator ==(Inno.Animation.AnimationPlaybackHandle left, Inno.Animation.AnimationPlaybackHandle right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-85565b9711f73beb"></a>[`public static bool Inno.Animation.AnimationPlaybackHandle.operator !=(Inno.Animation.AnimationPlaybackHandle left, Inno.Animation.AnimationPlaybackHandle right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9e5d4791a5717501"></a>[`public override string Inno.Animation.AnimationPlaybackHandle.ToString()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-5917ba1ed0380114"></a>[`public bool Inno.Animation.AnimationPlaybackHandle.isValid { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L309) | Gets whether the handle was initialized by an animation service. | — | — |
| <a id="api-bd927cb66b3e9573"></a>[`public uint Inno.Animation.AnimationPlaybackHandle.runtimeGeneration { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L316) | Gets the isolated runtime generation that created this handle. | — | — |
| <a id="api-f0a9c5333cd5a46e"></a>[`public readonly record struct Inno.Animation.AnimationPlaybackHandle : System.IEquatable<Inno.Animation.AnimationPlaybackHandle>`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Identifies one generation-checked animation playback owned by a runtime service. | — | — |

### `Inno.Animation.AnimationPlaybackState`

<!-- api: T:Inno.Animation.AnimationPlaybackState 71a65c88fe1dc2d6f086dd9c229fa9762f5e26b9ee6bd066d76b5c62a89eb7e6 -->
<!-- api: F:Inno.Animation.AnimationPlaybackState.Playing 3986bc9afab9576f9380606bc3cf4951d348ca720490803571f2b0615b038cfa -->
<!-- api: F:Inno.Animation.AnimationPlaybackState.Paused 7043ad35b4165d2e1cde55f5c307cd06c671438d47deef17443b33faa6d91aec -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-99f1c126c55d9bd1"></a>[`Inno.Animation.AnimationPlaybackState.Paused = 1`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L248) | The playback retains its position without advancing. | — | — |
| <a id="api-5040e1831eddf702"></a>[`Inno.Animation.AnimationPlaybackState.Playing = 0`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L243) | The playback is advancing and producing samples. | — | — |
| <a id="api-c2f9142596ef24b5"></a>[`public enum Inno.Animation.AnimationPlaybackState : int`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L238) | Describes the observable lifecycle of one animation playback. | — | — |

### `Inno.Animation.AnimationPlayOptions`

<!-- api: T:Inno.Animation.AnimationPlayOptions c00dfea0c0fa5ddcb0dfbdf9e771b51f44a34baa98064f1f86a7e21fd90be3ab -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.defaultValue 722b42b6457adecdeed6e2a347a78dc49558a898e58c2b7db46b8b051703fc61 -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.speed 054680427c658c412fd09ec82a9394c2d89af3a30685480b9a8dddf926b7428a -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.weight eeab5fee958481aaed9fc152cb25ebfdb95ab022323ce6d92cf3b953e36851ca -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.layer 6816de9185c8d38eb2c32500c644a194a3783daf526b2ab91cc7f593de4fbce4 -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.loop 44f87455875541c52ced7a2fc87b972af6f4f1be2f511d2c8d41e7205c68dbfa -->
<!-- api: P:Inno.Animation.AnimationPlayOptions.clock 5f0ba1370490f3ca2e9b818ca114bd9d3ad0ea98166d6d855f5a6c44921512a3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b98fabdb3a66d045"></a>[`public Inno.Animation.AnimationClock Inno.Animation.AnimationPlayOptions.clock { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L290) | Gets or sets the session clock used by the playback. | — | — |
| <a id="api-98a264f135d5f23a"></a>[`public static Inno.Animation.AnimationPlayOptions Inno.Animation.AnimationPlayOptions.defaultValue { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L259) | Gets the default playback options. | — | — |
| <a id="api-d84e8fec8b1c9b16"></a>[`public int Inno.Animation.AnimationPlayOptions.layer { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L280) | Gets or sets the deterministic blend layer. | — | — |
| <a id="api-cd571a2b4de48c97"></a>[`public bool Inno.Animation.AnimationPlayOptions.loop { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L285) | Gets or sets whether the clip repeats after its duration. | — | — |
| <a id="api-848f509881512cd1"></a>[`public float Inno.Animation.AnimationPlayOptions.speed { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L270) | Gets or sets the non-negative playback speed multiplier. | — | — |
| <a id="api-d08146d7139c1a12"></a>[`public float Inno.Animation.AnimationPlayOptions.weight { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L275) | Gets or sets the finite blend weight in the inclusive zero-to-one range. | — | — |
| <a id="api-9852c242252f5c30"></a>[`public struct Inno.Animation.AnimationPlayOptions`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L254) | Stores immutable options used to begin one animation playback. | — | — |

### `Inno.Animation.AnimationSample`

<!-- api: T:Inno.Animation.AnimationSample 9f85037ebf01978f18e7adc52b7e46ca75c2d395dd6f639f8f27f8ad2b57245d -->
<!-- api: M:Inno.Animation.AnimationSample.#ctor(Inno.Animation.AnimationTarget,Inno.Animation.AnimationBindingId,Inno.Animation.AnimationValue) 7817a79219bd89d6e1f60c818db10d3208866a212d94bfcd2ab3c8ad495da057 -->
<!-- api: P:Inno.Animation.AnimationSample.binding a8c4ea69c8c8f8b154aceb9b4224014051a8f75be8cd5fc42c2bfa8a4d605853 -->
<!-- api: P:Inno.Animation.AnimationSample.target f8682f3eeef815c860213fa15aac6d18ceff0854eb048399863857a770c9f144 -->
<!-- api: P:Inno.Animation.AnimationSample.value 03b0b65324336de9bfa6a9ca48453ec4c11a29b90ce51cce46054fef2fe5dea1 -->
<!-- api: M:Inno.Animation.AnimationSample.ToString 4370c55b7e5431b8e6740a75e49e6c062689d241a3282fb5b205a361acc39a4d -->
<!-- api: M:Inno.Animation.AnimationSample.op_Inequality(Inno.Animation.AnimationSample,Inno.Animation.AnimationSample) 91c6e835ab3200a9a1221c875274249bdbe4a21d52b1d6e51c2a1f26afd4bf22 -->
<!-- api: M:Inno.Animation.AnimationSample.op_Equality(Inno.Animation.AnimationSample,Inno.Animation.AnimationSample) d569d3478ac02ed8978048c1b9fee762e014af748f9c7cee8a1997f81e707a3d -->
<!-- api: M:Inno.Animation.AnimationSample.GetHashCode 6d3d4f864f1d99781729a294d33d884d488a61c021cecba05347c468eb2bd75b -->
<!-- api: M:Inno.Animation.AnimationSample.Equals(System.Object) dba04153f7e980330bbaee8e4afe19733b9f5fb0db8249460069a42a6646e36e -->
<!-- api: M:Inno.Animation.AnimationSample.Equals(Inno.Animation.AnimationSample) 8a12f2d95085585aa5483197ce5c48501cb2598612b694572a09d77e0463e10b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-acb964a979c180e2"></a>[`public Inno.Animation.AnimationSample.AnimationSample(Inno.Animation.AnimationTarget target, Inno.Animation.AnimationBindingId binding, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L336) | Creates one final animation sample.<br>`binding`：Stable target binding.<br>`value`：Final blended value.<br>`target`：The weak destination or explicit sampling-only policy. | — | — |
| <a id="api-3d251d13b35d271f"></a>[`public bool Inno.Animation.AnimationSample.Equals(Inno.Animation.AnimationSample other)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-28bcbb536de1ae04"></a>[`public override bool Inno.Animation.AnimationSample.Equals(object obj)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5e3d6ef9e51d2716"></a>[`public override int Inno.Animation.AnimationSample.GetHashCode()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-98e0cee6b592457f"></a>[`public static bool Inno.Animation.AnimationSample.operator ==(Inno.Animation.AnimationSample left, Inno.Animation.AnimationSample right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-288ec39e2edd372a"></a>[`public static bool Inno.Animation.AnimationSample.operator !=(Inno.Animation.AnimationSample left, Inno.Animation.AnimationSample right)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8e0bd892aa620e7a"></a>[`public override string Inno.Animation.AnimationSample.ToString()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ffc7038f0bc3fe93"></a>[`public Inno.Animation.AnimationBindingId Inno.Animation.AnimationSample.binding { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L353) | Gets the stable target binding. | — | — |
| <a id="api-b37d3de97a1637aa"></a>[`public Inno.Animation.AnimationTarget Inno.Animation.AnimationSample.target { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L358) | Gets the generation-qualified destination for this sample. | — | — |
| <a id="api-f16f1184bd8e8ebb"></a>[`public Inno.Animation.AnimationValue Inno.Animation.AnimationSample.value { get; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L363) | Gets the final blended value. | — | — |
| <a id="api-540a49c345165522"></a>[`public readonly record struct Inno.Animation.AnimationSample : System.IEquatable<Inno.Animation.AnimationSample>`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Stores one final blended value emitted for a stable binding identifier. | — | — |

### `Inno.Animation.AnimationSampling`

<!-- api: T:Inno.Animation.AnimationSampling 66be67103dc29dd87938bc60a81485b017e60c38322ad67c884d4de3057560ee -->
<!-- api: M:Inno.Animation.AnimationSampling.Interpolate(Inno.Animation.AnimationValue,Inno.Animation.AnimationValue,System.Single) 5f72385cc2d920c0538820c7a1e30bf6e26e77623f5141c7d5cf692320410864 -->
<!-- api: M:Inno.Animation.AnimationSampling.AddWeighted(Inno.Animation.AnimationValue,Inno.Animation.AnimationValue,System.Single) 248c5d7ae5d805644641af3cdaea18e5f578b0c8291cc35848122055730a554d -->
<!-- api: M:Inno.Animation.AnimationSampling.CompleteWeighted(Inno.Animation.AnimationValue,System.Single) 64f6c420b26f8240e7581cfe2b120b4f3208bb64e9df65503a3e1def2ac4c56e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3463951b43277bf0"></a>[`public static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.AddWeighted(Inno.Animation.AnimationValue sum, Inno.Animation.AnimationValue value, float weight)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L45) | Adds one weighted value to an accumulation of the same kind.<br>返回：The updated sum.<br>`sum`：The current weighted sum.<br>`value`：The value to accumulate.<br>`weight`：The blend weight. | — | — |
| <a id="api-e2dc2ad9fce5b867"></a>[`public static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.CompleteWeighted(Inno.Animation.AnimationValue sum, float inverseWeight)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L64) | Normalizes a weighted sum, including quaternion normalization.<br>返回：The final blended value.<br>`sum`：The accumulated sum.<br>`inverseWeight`：The reciprocal total weight. | — | — |
| <a id="api-88725d1ff3cc1f17"></a>[`public static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.Interpolate(Inno.Animation.AnimationValue left, Inno.Animation.AnimationValue right, float amount)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L23) | Interpolates compatible values, normalizing quaternion output.<br>返回：The interpolated value.<br>`left`：The initial value.<br>`right`：The final value of the same kind.<br>`amount`：The interpolation fraction, clamped to zero through one. | — | — |
| <a id="api-5e8dca405fed10d2"></a>[`public static class Inno.Animation.AnimationSampling`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L6) | Provides backend-neutral interpolation and weighted composition used by animation runtimes. | — | — |

### `Inno.Animation.AnimationTarget`

<!-- api: T:Inno.Animation.AnimationTarget 0aa562c5a876ef31e7287b6a9dc0f7e2fc59a789a3afe669d17b551d74b54c65 -->
<!-- api: M:Inno.Animation.AnimationTarget.#ctor(Inno.Core.Identity.Identity) 22e35c45c104048c8da289127b91fd5ea25d7d8ca081e55a2f773cedac3ea4dc -->
<!-- api: P:Inno.Animation.AnimationTarget.samplingOnly 48cfd4778c83d5cadd36472d7cec5135544862d74f0c507034946940c856bad4 -->
<!-- api: P:Inno.Animation.AnimationTarget.isSamplingOnly 7815152c665a26b58fed1a6c75ea1c65841ee2c8dd6b9003d7b6410dcd87b031 -->
<!-- api: P:Inno.Animation.AnimationTarget.isValid 65941781a1db4f831a78960c94b1c9dbf6d08ea93f575337047962af4e9bf86b -->
<!-- api: P:Inno.Animation.AnimationTarget.persistentId e51e12f6d2906a9c5ecf2667897380bbae80424cd6bfb48f0df478cce4880844 -->
<!-- api: M:Inno.Animation.AnimationTarget.Resolve``1 1000e5b6a5a7bc780fca02dbf28c3453b734e70d871b30c47fa073a17cd66de2 -->
<!-- api: M:Inno.Animation.AnimationTarget.Equals(Inno.Animation.AnimationTarget) 03b43a6edf89f0c365665eec1ae2e58adfedda604d9d43e91e6e7d5154cc0da6 -->
<!-- api: M:Inno.Animation.AnimationTarget.Equals(System.Object) 58a33a0dec5f799024e9330330849cfdfc36d0256fb58dd3df4943f1a2f5940d -->
<!-- api: M:Inno.Animation.AnimationTarget.GetHashCode 0a3ab0556a7b59f741940fcf9db91e6c8e9fd73fd8cf8bfb0eb157b026610bf9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1172d0b2df62ebc3"></a>[`public Inno.Animation.AnimationTarget.AnimationTarget(Inno.Core.Identity.Identity identity)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L24) | Captures one currently registered destination in its original identity domain.<br>`identity`：The destination's live identity, not a detached persistent identifier. | — | `System.ArgumentException`：The identity is not registered. |
| <a id="api-7cbbb82f049547ea"></a>[`public bool Inno.Animation.AnimationTarget.Equals(Inno.Animation.AnimationTarget other)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L79) | Compares runtime destinations, including their identity domain.<br>返回：True when both values denote the same runtime destination or sampling policy.<br>`other`：The destination to compare. | — | — |
| <a id="api-b63be64d5a9dc693"></a>[`public override bool Inno.Animation.AnimationTarget.Equals(object? obj)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L91) | Compares a boxed destination with this value.<br>返回：True for an equal animation destination.<br>`obj`：The value to compare. | — | — |
| <a id="api-3aca322d862ae96f"></a>[`public override int Inno.Animation.AnimationTarget.GetHashCode()`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L99) | Hashes the domain-qualified runtime destination.<br>返回：A hash consistent with destination equality. | — | — |
| <a id="api-2fdb97c44a47b7ab"></a>[`public TObject? Inno.Animation.AnimationTarget.Resolve<TObject>() where TObject : Inno.Core.Identity.IdentityObject`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L68) | Resolves the original runtime slot without binding to a replacement generation.<br>返回：The live destination, or null for a missing, retired, or incompatible target. | — | — |
| <a id="api-ed564242b434638a"></a>[`public bool Inno.Animation.AnimationTarget.isSamplingOnly { get; }`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L47) | Gets whether this target deliberately omits object binding. | — | — |
| <a id="api-330b8cab7ea95130"></a>[`public bool Inno.Animation.AnimationTarget.isValid { get; }`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L52) | Gets whether this value was initialized with a destination or sampling policy. | — | — |
| <a id="api-ef0ef2f917cee200"></a>[`public System.Guid Inno.Animation.AnimationTarget.persistentId { get; }`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L57) | Gets the persistent destination identifier, or empty for sampling-only playback. | — | — |
| <a id="api-0ba1bb8e80f91797"></a>[`public static Inno.Animation.AnimationTarget Inno.Animation.AnimationTarget.samplingOnly { get; }`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L42) | Gets an explicit destination for sampling and markers without object binding. | — | — |
| <a id="api-6cc1defcf81c22bb"></a>[`public readonly struct Inno.Animation.AnimationTarget : System.IEquatable<Inno.Animation.AnimationTarget>`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L9) | Identifies a transient animation destination without retaining the destination object. | — | — |

### `Inno.Animation.AnimationTrack`

<!-- api: T:Inno.Animation.AnimationTrack 51af2255350f68af6599d45b6b4b179c76c5d8c068585712501f93189f080008 -->
<!-- api: M:Inno.Animation.AnimationTrack.#ctor 4eac9a9da59c42cd9012f335b409c25d677e8f26d95624bd8d96070aba96347c -->
<!-- api: M:Inno.Animation.AnimationTrack.#ctor(System.String,Inno.Animation.AnimationInterpolation,System.Collections.Generic.IEnumerable{Inno.Animation.AnimationKeyframe}) 3a01f121bac7ec7b9c10b4cdcbe3ca3da750b90767ae6916a097e4245f419cbe -->
<!-- api: P:Inno.Animation.AnimationTrack.bindingId 8797900857d7e4755a32f79859c695c74e794c3151410026f39dd4c7c3783d28 -->
<!-- api: P:Inno.Animation.AnimationTrack.interpolation 5bc371ab29606e0d60905b7aa8df041d7376750a103025056fdabf07fe1b7f03 -->
<!-- api: P:Inno.Animation.AnimationTrack.keyframes 42a6c6a9e2151105be915374ff53e9abf79d70dd60787ca5b4caa8c3366f2fce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7547c35664d5ad07"></a>[`public Inno.Animation.AnimationTrack.AnimationTrack()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L56) | Creates an empty track for structured deserialization. | — | — |
| <a id="api-cdc5e3a93bc8694b"></a>[`public Inno.Animation.AnimationTrack.AnimationTrack(string bindingId, Inno.Animation.AnimationInterpolation interpolation, System.Collections.Generic.IEnumerable<Inno.Animation.AnimationKeyframe> keyframes)`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L72) | Creates one validated animation track.<br>`bindingId`：Stable target binding identifier.<br>`interpolation`：Sampling rule between adjacent keyframes.<br>`keyframes`：Keyframes ordered by non-decreasing clip-local time. | — | — |
| <a id="api-2a0be1bc12daf720"></a>[`public string Inno.Animation.AnimationTrack.bindingId { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L89) | Gets or sets the stable target binding identifier. | — | — |
| <a id="api-4f6a7708dfd27688"></a>[`public Inno.Animation.AnimationInterpolation Inno.Animation.AnimationTrack.interpolation { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L95) | Gets or sets the sampling rule between adjacent keyframes. | — | — |
| <a id="api-ac4a2fd9cfc81c71"></a>[`public Inno.Animation.AnimationKeyframe[] Inno.Animation.AnimationTrack.keyframes { get; set; }`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L101) | Gets or sets ordered keyframes. | — | — |
| <a id="api-7c490762dfa1d2c4"></a>[`public sealed class Inno.Animation.AnimationTrack : Inno.Core.Serialization.ISerializable`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L51) | Stores one ordered value track targeting a stable binding. | — | — |

### `Inno.Animation.AnimationValue`

<!-- api: T:Inno.Animation.AnimationValue 6663ccc3ba85b0839b8c4c3f775658c972ce3c04f64d918cc41d69fceeb7d1ec -->
<!-- api: M:Inno.Animation.AnimationValue.#ctor(Inno.Animation.AnimationValueKind,System.Single,System.Single,System.Single,System.Single) 67c3b8323970c49032bd1a6ffbc2d16bc46ed6e99d59a7d20b793a2b60475cd0 -->
<!-- api: P:Inno.Animation.AnimationValue.kind 3552ad8c4a5b24a7733467ff6575b11ce9afd85fba81989373de6f4e2d98a0bb -->
<!-- api: P:Inno.Animation.AnimationValue.x 0bf5b405d62d67d2269d32e43a95b12c6d490ab548335a4abff216b055494bdc -->
<!-- api: P:Inno.Animation.AnimationValue.y acf886c0b354430eb7ddc20b30e16341df8c5f783d8d30a0bfc2e6123cab48f9 -->
<!-- api: P:Inno.Animation.AnimationValue.z 5085d60869401db36e402cde22b21c7ed28382d43c55639b5e11ed7456380a87 -->
<!-- api: P:Inno.Animation.AnimationValue.w 80e50ddf7229306012f57449e299464bcb5e723324032dcaeae226d6c94eac3a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5eb4c13d6844128a"></a>[`public Inno.Animation.AnimationValue.AnimationValue(Inno.Animation.AnimationValueKind kind, float x, float y = 0, float z = 0, float w = 0)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L97) | Creates a four-component animation value.<br>`kind`：Interpretation of the populated components.<br>`x`：First component.<br>`y`：Second component.<br>`z`：Third component.<br>`w`：Fourth component. | — | — |
| <a id="api-82efd3d322450659"></a>[`public Inno.Animation.AnimationValueKind Inno.Animation.AnimationValue.kind { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L115) | Gets or sets the component interpretation. | — | — |
| <a id="api-ac97529c5ee31d7b"></a>[`public float Inno.Animation.AnimationValue.w { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L139) | Gets or sets the fourth component. | — | — |
| <a id="api-9de7be61c3b323f1"></a>[`public float Inno.Animation.AnimationValue.x { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L121) | Gets or sets the first component. | — | — |
| <a id="api-7e208569d63b59ad"></a>[`public float Inno.Animation.AnimationValue.y { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L127) | Gets or sets the second component. | — | — |
| <a id="api-b482a34792b289be"></a>[`public float Inno.Animation.AnimationValue.z { get; set; }`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L133) | Gets or sets the third component. | — | — |
| <a id="api-3e528ecadda0bdf0"></a>[`public struct Inno.Animation.AnimationValue`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L77) | Stores one backend-neutral scalar, vector, or quaternion animation value. | — | — |

### `Inno.Animation.AnimationValueKind`

<!-- api: T:Inno.Animation.AnimationValueKind 50ae905569ff9c8ecd85fec0312f521378c98a3d5203987cdbc79a17f19c194d -->
<!-- api: F:Inno.Animation.AnimationValueKind.Scalar 9146c7fdbe4da04141cd46a47f813f60bfcbb502a86dd71d23e153291fc921d6 -->
<!-- api: F:Inno.Animation.AnimationValueKind.Vector2 bdc03becff6514f3da1498e85ee2bc4ca8d103ec842ab995ab4eddf665dfcd6b -->
<!-- api: F:Inno.Animation.AnimationValueKind.Vector3 185c0f9b00899688fe0a2e319a4b48bf401897736d06093026d342c79675a0e1 -->
<!-- api: F:Inno.Animation.AnimationValueKind.Vector4 03105d5740c7bc5ee8f2fef5732c6527d5e9e2b04f4c954c7c846cd454b4bb05 -->
<!-- api: F:Inno.Animation.AnimationValueKind.Quaternion 0a176f7c9f7d1bb61f9d5c902dec1a55178a9e99e3e76c43172ffae847c2a3ad -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e41035cac4d5d471"></a>[`Inno.Animation.AnimationValueKind.Quaternion = 4`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L71) | Four normalized rotation components. | — | — |
| <a id="api-fc06e97f6eef0ddd"></a>[`Inno.Animation.AnimationValueKind.Scalar = 0`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L51) | A single scalar component. | — | — |
| <a id="api-1d1707114a0e7583"></a>[`Inno.Animation.AnimationValueKind.Vector2 = 1`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L56) | Two independent components. | — | — |
| <a id="api-0159fb4b5d412da5"></a>[`Inno.Animation.AnimationValueKind.Vector3 = 2`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L61) | Three independent components. | — | — |
| <a id="api-c9a843b617dce0b3"></a>[`Inno.Animation.AnimationValueKind.Vector4 = 3`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L66) | Four independent components. | — | — |
| <a id="api-4432af756bdc8d39"></a>[`public enum Inno.Animation.AnimationValueKind : int`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L46) | Describes the number and interpretation of components in an animation value. | — | — |

### `Inno.Animation.IAnimationBindingSink`

<!-- api: T:Inno.Animation.IAnimationBindingSink 891b0e4243d5c10c6bfe5e7e7eb8cc167dd1f12e4d78dc33f09344b6ad001ba5 -->
<!-- api: M:Inno.Animation.IAnimationBindingSink.Apply(System.ReadOnlySpan{Inno.Animation.AnimationSample}) 015de898f215c319cfc9bc41da5c7c2287a2ccb546605c3582a529890a78537e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c4423dca96dfa637"></a>[`void Inno.Animation.IAnimationBindingSink.Apply(System.ReadOnlySpan<Inno.Animation.AnimationSample> samples)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L377) | Applies one immutable frame of final animation samples.<br>`samples`：Samples sorted by stable binding identifier. | — | — |
| <a id="api-b71d0e72b3a92424"></a>[`public interface Inno.Animation.IAnimationBindingSink`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L369) | Receives final backend-neutral samples after runtime blending has completed. | — | — |

### `Inno.Animation.IAnimationService`

<!-- api: T:Inno.Animation.IAnimationService 2f1719d6b13c058c14e2c229931960b0e7d4ebe8b469f74ac55476b5d60deaf2 -->
<!-- api: M:Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget) 201bcd5adcec25db1baf1ed0806e92b44885ca6625ebac6644cfc7f7317ca395 -->
<!-- api: M:Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget,Inno.Animation.AnimationPlayOptions) 43151d586a1e09b474768b6fe94df1a61f18e850dbe44b5c469c3d83bd171ac5 -->
<!-- api: M:Inno.Animation.IAnimationService.Stop(Inno.Animation.AnimationPlaybackHandle) 8a91c7e19cebcaae6a21dcaf0af8c7bcaf1dd7cf8d2ddd8d652ca3b2bd79b474 -->
<!-- api: M:Inno.Animation.IAnimationService.Pause(Inno.Animation.AnimationPlaybackHandle) 8279b796afad8ac0ce6a273eb6abb656307062626531f4e6b37132e0ef2eb6f6 -->
<!-- api: M:Inno.Animation.IAnimationService.Resume(Inno.Animation.AnimationPlaybackHandle) 61deb8ad5785be0dfb66783af9c1290411626b3124e4b3f50cfc7183d06ded87 -->
<!-- api: M:Inno.Animation.IAnimationService.Seek(Inno.Animation.AnimationPlaybackHandle,System.Single) 80e68f1553dfb9476555abddc9a984a08e1b18f0d6c826b1cda7fea659af8579 -->
<!-- api: M:Inno.Animation.IAnimationService.TryGetState(Inno.Animation.AnimationPlaybackHandle,Inno.Animation.AnimationPlaybackState@,System.Single@) d3a25d7d2f5a6fd09fe5e68f1b50434ddc1bcf78d7b461c9296c713715381d4c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a5ccf2b40eae7627"></a>[`bool Inno.Animation.IAnimationService.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L70) | Pauses one live playback at its current position.<br>返回：true when the playback state changed.<br>`handle`：Playback handle from this runtime generation. | — | — |
| <a id="api-479c69b68039895d"></a>[`Inno.Animation.AnimationPlaybackHandle Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L44) | Starts one clip with explicit playback options.<br>返回：A generation-checked playback handle.<br>`clip`：Valid imported animation clip.<br>`options`：Playback clock, speed, blend, layer, and looping policy.<br>`target`：The weak destination or explicit sampling-only policy. | — | — |
| <a id="api-0d00d295f70e211c"></a>[`Inno.Animation.AnimationPlaybackHandle Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L24) | Starts one clip with default playback options.<br>返回：A generation-checked playback handle.<br>`clip`：Valid imported animation clip.<br>`target`：The weak destination or explicit sampling-only policy. | — | — |
| <a id="api-11a92db2741fb287"></a>[`bool Inno.Animation.IAnimationService.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L81) | Resumes one paused playback.<br>返回：true when the playback state changed.<br>`handle`：Playback handle from this runtime generation. | — | — |
| <a id="api-c8083f39b4ba1cf5"></a>[`bool Inno.Animation.IAnimationService.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L95) | Seeks one live playback to a clip-local time.<br>返回：true when the handle was live and the position was updated.<br>`handle`：Playback handle from this runtime generation.<br>`time`：Finite clip-local time in seconds, clamped to the clip duration. | — | — |
| <a id="api-595259a7b7d97f5c"></a>[`bool Inno.Animation.IAnimationService.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L59) | Stops one live playback.<br>返回：true when a live playback was stopped.<br>`handle`：Playback handle from this runtime generation. | — | — |
| <a id="api-4b9063aa09178baf"></a>[`bool Inno.Animation.IAnimationService.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L115) | Tries to read one live playback's state and position.<br>返回：true when the handle still identifies a live playback.<br>`handle`：Playback handle from this runtime generation.<br>`state`：Receives the current playback state when successful.<br>`time`：Receives the current clip-local time in seconds when successful. | — | — |
| <a id="api-4df2550beb612b3e"></a>[`public interface Inno.Animation.IAnimationService`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L10) | Defines backend-neutral animation playback and control for one isolated runtime session. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
