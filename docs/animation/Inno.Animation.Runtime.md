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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Animation.Runtime.AnimationBindingRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.Runtime.AnimationBindingRuntime`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L12) | Resolves declared binding protocols through transactional, generation-owned extension snapshots. |
| [`Inno.Animation.Runtime.AnimationBindingRuntime.AnimationBindingRuntime(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics)`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L28) | Creates a binding owner connected to the host's transactional type catalog. |
| [`void Inno.Animation.Runtime.AnimationBindingRuntime.Apply(System.ReadOnlySpan<Inno.Animation.AnimationSample> samples)`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L58) | Applies a frame batch using the currently committed binding generation. |
| [`void Inno.Animation.Runtime.AnimationBindingRuntime.Dispose()`](../../src/content/animation/Inno.Animation.Runtime/AnimationBindingRuntime.cs#L95) | Releases all provider instances and unregisters their generation participant. |

### `Inno.Animation.Runtime.AnimationRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L87) | Starts one clip with the runtime's default playback policy. |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.Runtime.AnimationRuntime.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L120) | Starts one clip with an explicit clock, looping, speed, layer, and blend policy. |
| [`Inno.Animation.Runtime.AnimationRuntime`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L14) | Owns animation playback, sampling, blending, and marker dispatch for one runtime session. |
| [`Inno.Animation.Runtime.AnimationRuntime.AnimationRuntime(Inno.Core.Events.EventDispatcher events, Inno.Animation.IAnimationBindingSink bindings, int maxPlaybacks = 16384)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L54) | Creates one isolated animation runtime generation. |
| [`bool Inno.Animation.Runtime.AnimationRuntime.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L193) | Pauses one live playback without changing its clip-local time. |
| [`bool Inno.Animation.Runtime.AnimationRuntime.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L210) | Resumes one paused playback from its current clip-local time. |
| [`bool Inno.Animation.Runtime.AnimationRuntime.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L233) | Seeks one live playback to a clamped clip-local time. |
| [`bool Inno.Animation.Runtime.AnimationRuntime.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L175) | Stops and retires one live playback slot. |
| [`bool Inno.Animation.Runtime.AnimationRuntime.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L260) | Tries to read the state and clip-local time of one live playback. |
| [`int Inno.Animation.Runtime.AnimationRuntime.playbackCount`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L159) | Gets active animation playback slots. |
| [`long Inno.Animation.Runtime.AnimationRuntime.rejectedPlaybacks`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L164) | Gets playback admissions rejected by finite capacity. |
| [`override void Inno.Animation.Runtime.AnimationRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L33) | Captures snapshots and binds service façades. |
| [`override void Inno.Animation.Runtime.AnimationRuntime.OnStop()`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L334) | Releases every live playback in this runtime generation. |
| [`override void Inno.Animation.Runtime.AnimationRuntime.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L40) | Advances domain state on the variable clock. |
| [`void Inno.Animation.Runtime.AnimationRuntime.Update(float scaledDeltaTime, float unscaledDeltaTime)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntime.cs#L285) | Advances every playback and applies one deterministic blended sample batch. |

### `Inno.Animation.Runtime.AnimationRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.Runtime.AnimationRuntimeFactory`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L11) | Creates one animation runtime feature for each isolated session. |
| [`Inno.Animation.Runtime.AnimationRuntimeFactory.AnimationRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Animation.Runtime.AnimationRuntime> runtimeFactory)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L21) | Creates a reusable feature factory around a composition-owned runtime callback. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Animation.Runtime.AnimationRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L43) | Creates a feature over a newly allocated animation runtime. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Animation.Runtime.AnimationRuntimeFactory.descriptor`](../../src/content/animation/Inno.Animation.Runtime/AnimationRuntimeFactory.cs#L29) | Gets ordering metadata that evaluates animation after scene simulation. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Animation](Inno.Animation.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
