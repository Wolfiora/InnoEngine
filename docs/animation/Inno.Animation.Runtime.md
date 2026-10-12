# Inno.Animation.Runtime

[Animation 索引](README.md) · [Contract](Inno.Animation.md) · [Assets](Inno.Animation.Assets.md)

`AnimationRuntime : RuntimeSubsystem, IAnimationService` 为一个 Session 拥有全部 playback。每帧按选定 scaled/unscaled clock 前进，按 `(AnimationTarget, AnimationBindingId)` 分组，在最高有效 layer 内按 weight 混合。不同目标不会因为 binding 相同而串值；Quaternion 输出会归一化。

`AnimationRuntimeFactory` 声明对 Scene subsystem 的依赖，并在 Session update 阶段推进 runtime；Begin/End frame 管理 `AnimationExecutionContext`。Marker 通过 Session `EventDispatcher` 发布，原生实时音频路径不执行这些 Provider；Animation binding Provider 本身在控制线程执行。

构造必须注入 `IAnimationBindingSink`。默认发行使用 `AnimationBindingRuntime(types, diagnostics)`，按 `AnimationBindingProviderAttribute(id, bindingId, kind)` 发现 Provider，构建 TypeRegistry 候选，拒绝重复 ID/协议，退休时释放旧 Provider。`Apply(samples)` 在 owner thread 解析目标并应用值；缺失/不兼容/异常通过 Core reporter 发布，下一批成功时清除。Dispose 注销 registry，不拥有传入 reporter。

`Animation.Play(clip, target)` / `Play(clip, target, options)` 要求明确目标。`new AnimationTarget(object.identity)` 不持有对象；对象退休后不会自动控制新 runtime slot。纯采样/marker 使用 `AnimationTarget.samplingOnly`，不再以缺少 sink 隐式丢弃输出。播放捕获独立 track/keyframe/marker 副本；后续修改创作资产不影响已播放快照。

Provider 只实现中立绑定机制。Transform、Sprite、骨骼、Timeline 等具体绑定不内建；可通过 Provider 扩展，而不修改 Runtime 中央 switch。

[下一页：Inno.Animation.Assets](Inno.Animation.Assets.md)

## 播放预算

`AnimationRuntime(events, bindings, maxPlaybacks = 16384)` 要求正容量；Play 超限在捕获/分配前明确拒绝。创作数据冻结失败不消耗 slot。Stop/自然完成释放 slot，重用时增加 generation，旧句柄不能控制新播放。`playbackCount` 与 `rejectedPlaybacks` 是只读 Host 统计。没有增加具体 Animation binding 世界观或 gameplay Plugin。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Animation.Runtime.AnimationBindingRuntime`

<!-- api: T:Inno.Animation.Runtime.AnimationBindingRuntime 9174a291805cced090e4ea688f02f99394a49c31dad8b801d2302f84f6ef0a56 -->
<!-- api: M:Inno.Animation.Runtime.AnimationBindingRuntime.#ctor(Inno.Extensibility.Types.TypeCatalog,Inno.Core.Diagnostics.IDiagnosticReporter) 14921d5f2088504d851a4eca218f4be7a17d27678898bbc7674d4990fe311e01 -->
<!-- api: M:Inno.Animation.Runtime.AnimationBindingRuntime.Apply(System.ReadOnlySpan{Inno.Animation.AnimationSample}) 828e6fe0855c0a9db38721b23ededf85d5db78971e26353169a80453be300ffb -->
<!-- api: M:Inno.Animation.Runtime.AnimationBindingRuntime.Dispose 2208b4a68f318e8e852714b1610ad5ee8ddd12564809e3f219bd2c86b3bc1690 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6732bb87ae850dea"></a>[`public Inno.Animation.Runtime.AnimationBindingRuntime.AnimationBindingRuntime(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics)`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L28) | Creates a binding owner connected to the host's transactional type catalog.<br>`types`：The shared type catalog; candidate failure preserves its last good snapshot.<br>`diagnostics`：A borrowed reporter scoped to this session's binding diagnostics. | — | — |
| <a id="api-534777b1797eaf00"></a>[`public void Inno.Animation.Runtime.AnimationBindingRuntime.Apply(System.ReadOnlySpan<Inno.Animation.AnimationSample> samples)`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L58) | Applies a frame batch using the currently committed binding generation.<br>`samples`：Final samples, independently blended by destination and binding. | — | `System.ObjectDisposedException`：This binding owner has retired.<br>`System.InvalidOperationException`：Called outside the owner thread. |
| <a id="api-5a0eaabfa99c6b24"></a>[`public void Inno.Animation.Runtime.AnimationBindingRuntime.Dispose()`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L95) | Releases all provider instances and unregisters their generation participant. | — | — |
| <a id="api-a104264a6ecd53f3"></a>[`public sealed class Inno.Animation.Runtime.AnimationBindingRuntime : Inno.Animation.IAnimationBindingSink, System.IDisposable`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L12) | Resolves declared binding protocols through transactional, generation-owned extension snapshots. | — | — |

### `Inno.Animation.Runtime.AnimationRuntime`

<!-- api: T:Inno.Animation.Runtime.AnimationRuntime 4c049375eb40227dc58278b71c68332880c0d7a050c87818f7f9903ff88d7c26 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 29018befaf59a9acc133848414b56c07358a8a476123ae026a694255f8901a91 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame) b79c32a6162acc19a5ca54c7e7bf74605ea75c0569e30d3958336da68301f684 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.#ctor(Inno.Core.Events.EventDispatcher,Inno.Animation.IAnimationBindingSink,System.Int32) 102e1505c7eaf118c06bb9acda81ed97770d43e0afef0ae65391b0e539c2b379 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget) 55e3f9dd72bf6f59ce12cc42ad7c9cfbb6db76f397933469cdd64204687c914a -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset,Inno.Animation.AnimationTarget,Inno.Animation.AnimationPlayOptions) ded9b8d9636e5f19b46ba0b8ddd96beed296834f3f49d6d174726acafb50e02b -->
<!-- api: P:Inno.Animation.Runtime.AnimationRuntime.playbackCount 54f7b80763c820825bb60bbfa50e388af88aa8663233010d69e72ee439c8fae7 -->
<!-- api: P:Inno.Animation.Runtime.AnimationRuntime.rejectedPlaybacks 547bcdd877de2b09a870a22e4875bf045d3e6ed94de2402761aa89457bf99d34 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Stop(Inno.Animation.AnimationPlaybackHandle) dd197c5cb7a15e946231e7d6617c95c6e869e6fe908765839a7e994f49b3652b -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Pause(Inno.Animation.AnimationPlaybackHandle) 62f8149f1295581fc72e705c4f00bf59d0be3c04a2b39ccb9fa4626a780aeda0 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Resume(Inno.Animation.AnimationPlaybackHandle) 7723502804a0dd3b815111528194d2714fad947be18ff4d7496d0de445c3d956 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Seek(Inno.Animation.AnimationPlaybackHandle,System.Single) 7fa8d634bad0d5f09f668508ed774d7dd3f077ba43fb3a9c2429dfc1f6e2ee35 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.TryGetState(Inno.Animation.AnimationPlaybackHandle,Inno.Animation.AnimationPlaybackState@,System.Single@) 6e9922e86645c85b3a78d852fdf17ae80e40cb8d8939f57a8f1f5b41def9d616 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.Update(System.Single,System.Single) a7c6627fdaadfc4ea702cb3e264a07c88088913231b434f483cfd06931d88cf7 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntime.OnStop b16847755f24969a5f3e47a18389a5126eab5248434fffd63bf93801dd96049d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c1abd74e3789f3f0"></a>[`public Inno.Animation.Runtime.AnimationRuntime.AnimationRuntime(Inno.Core.Events.EventDispatcher events, Inno.Animation.IAnimationBindingSink bindings, int maxPlaybacks = 16384)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L54) | Creates one isolated animation runtime generation.<br>`events`：Session event dispatcher that receives marker events.<br>`bindings`：Required output boundary; sampling-only playback must use an explicit sampling target.<br>`maxPlaybacks`：Positive active playback capacity; rejection occurs before allocating a handle. | — | — |
| <a id="api-1f2db781a5213e67"></a>[`protected override void Inno.Animation.Runtime.AnimationRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L33) | Captures snapshots and binds service façades.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-d76aefb233556585"></a>[`protected override void Inno.Animation.Runtime.AnimationRuntime.OnStop()`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L334) | Releases every live playback in this runtime generation. | — | — |
| <a id="api-50b547607bac6e16"></a>[`protected override void Inno.Animation.Runtime.AnimationRuntime.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L40) | Advances domain state on the variable clock.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-3c03999d1b84de42"></a>[`public bool Inno.Animation.Runtime.AnimationRuntime.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L193) | Pauses one live playback without changing its clip-local time.<br>返回：true when a playing playback became paused; otherwise, false.<br>`handle`：Playback handle produced by this runtime generation. | — | — |
| <a id="api-cc0b395c8dabc05d"></a>[`public Inno.Animation.AnimationPlaybackHandle Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L120) | Starts one clip with an explicit clock, looping, speed, layer, and blend policy.<br>返回：A generation-checked handle owned by this runtime.<br>`clip`：Valid clip to sample until completion or explicit stop.<br>`options`：Playback options validated before a runtime slot is allocated.<br>`target`：The weak destination or explicit sampling-only policy. | — | `System.ObjectDisposedException`：Thrown when this runtime has been disposed.<br>`System.ArgumentNullException`：Thrown when clip is null.<br>`System.ArgumentException`：Thrown when options contains a non-finite or unsupported value.<br>`System.InvalidOperationException`：Thrown when the clip is structurally invalid. |
| <a id="api-f520bc9ddbac7e13"></a>[`public Inno.Animation.AnimationPlaybackHandle Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L87) | Starts one clip with the runtime's default playback policy.<br>返回：A generation-checked handle owned by this runtime.<br>`clip`：Valid clip to sample until completion or explicit stop.<br>`target`：The weak destination or explicit sampling-only policy. | — | `System.ObjectDisposedException`：Thrown when this runtime has been disposed.<br>`System.ArgumentNullException`：Thrown when clip is null.<br>`System.InvalidOperationException`：Thrown when the clip is structurally invalid. |
| <a id="api-da5e7b9adda2be5a"></a>[`public bool Inno.Animation.Runtime.AnimationRuntime.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L210) | Resumes one paused playback from its current clip-local time.<br>返回：true when a paused playback resumed; otherwise, false.<br>`handle`：Playback handle produced by this runtime generation. | — | — |
| <a id="api-ed5dcb613bc10795"></a>[`public bool Inno.Animation.Runtime.AnimationRuntime.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L233) | Seeks one live playback to a clamped clip-local time.<br>返回：true when the handle was live and its time was updated; otherwise, false.<br>`handle`：Playback handle produced by this runtime generation.<br>`time`：Finite clip-local time in seconds. | — | `System.ArgumentOutOfRangeException`：Thrown when time is not finite. |
| <a id="api-bcc3fa767505e5fd"></a>[`public bool Inno.Animation.Runtime.AnimationRuntime.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L175) | Stops and retires one live playback slot.<br>返回：true when a live playback was retired; otherwise, false.<br>`handle`：Playback handle produced by this runtime generation. | — | — |
| <a id="api-df27ce2477a1315e"></a>[`public bool Inno.Animation.Runtime.AnimationRuntime.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L260) | Tries to read the state and clip-local time of one live playback.<br>返回：true when the handle identifies a live playback; otherwise, false.<br>`handle`：Playback handle produced by this runtime generation.<br>`state`：Receives the live playback state, or the default value when the handle is stale.<br>`time`：Receives clip-local time in seconds, or zero when the handle is stale. | — | — |
| <a id="api-3a480106556a46b2"></a>[`public void Inno.Animation.Runtime.AnimationRuntime.Update(float scaledDeltaTime, float unscaledDeltaTime)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L285) | Advances every playback and applies one deterministic blended sample batch.<br>`scaledDeltaTime`：Non-negative scaled frame interval.<br>`unscaledDeltaTime`：Non-negative unscaled frame interval. | — | — |
| <a id="api-50486b5694c26a0c"></a>[`public int Inno.Animation.Runtime.AnimationRuntime.playbackCount { get; }`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L159) | Gets active animation playback slots. | — | — |
| <a id="api-fd23e244e0d02f17"></a>[`public long Inno.Animation.Runtime.AnimationRuntime.rejectedPlaybacks { get; }`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L164) | Gets playback admissions rejected by finite capacity. | — | — |
| <a id="api-028be9400d36b72c"></a>[`public sealed class Inno.Animation.Runtime.AnimationRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Animation.IAnimationService`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L14) | Owns animation playback, sampling, blending, and marker dispatch for one runtime session. | — | — |

### `Inno.Animation.Runtime.AnimationRuntimeFactory`

<!-- api: T:Inno.Animation.Runtime.AnimationRuntimeFactory 0bf1606185ddc84f9e15fb2dbc65cef6b4fae9b3afc91891c96aae970e085113 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Animation.Runtime.AnimationRuntime}) c2a222d830252ce8657b0ead451409961575c6588fd4bfee5eeb19b5ad85f5f1 -->
<!-- api: P:Inno.Animation.Runtime.AnimationRuntimeFactory.descriptor 348a3c23ad6b9fafa33ecd8a37deb3fdb915c80d5e6725d13f3f7a70e6aaaa60 -->
<!-- api: M:Inno.Animation.Runtime.AnimationRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 882f09518bb5a26806238fff7c8b2f162ef6faa857a7ab6d1fc7c8ec71a552cf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-26b60a3509541eef"></a>[`public Inno.Animation.Runtime.AnimationRuntimeFactory.AnimationRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Animation.Runtime.AnimationRuntime> runtimeFactory)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L21) | Creates a reusable feature factory around a composition-owned runtime callback.<br>`runtimeFactory`：Callback that creates a distinct animation runtime for each session. | — | — |
| <a id="api-7f07d0229554e965"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Animation.Runtime.AnimationRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L43) | Creates a feature over a newly allocated animation runtime.<br>返回：A new session-owned animation feature.<br>`context`：Isolated session context supplied to the runtime factory. | — | — |
| <a id="api-d52df9d0e737e17c"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Animation.Runtime.AnimationRuntimeFactory.descriptor { get; }`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L29) | Gets ordering metadata that evaluates animation after scene simulation. | — | — |
| <a id="api-c6d847260d87d80e"></a>[`public sealed class Inno.Animation.Runtime.AnimationRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L11) | Creates one animation runtime feature for each isolated session. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Animation](Inno.Animation.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
