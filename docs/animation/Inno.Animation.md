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

AnimationPlaybackHandle playback = Animation.Play(
    clip,
    new AnimationTarget(targetObject.identity),
    new AnimationPlayOptions
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Animation.Animation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.Animation`](../../src/content/animation/Inno.Animation/Animation.cs#L39) | Provides script-friendly animation control through the current runtime context. |
| [`static Inno.Animation.AnimationPlaybackHandle Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation/Animation.cs#L53) | Starts one clip with default playback options. |
| [`static Inno.Animation.AnimationPlaybackHandle Inno.Animation.Animation.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation/Animation.cs#L74) | Starts one clip with explicit playback options. |
| [`static bool Inno.Animation.Animation.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L101) | Pauses one live playback. |
| [`static bool Inno.Animation.Animation.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L112) | Resumes one paused playback. |
| [`static bool Inno.Animation.Animation.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation/Animation.cs#L126) | Seeks one live playback to a clip-local time. |
| [`static bool Inno.Animation.Animation.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/Animation.cs#L90) | Stops one live playback. |
| [`static bool Inno.Animation.Animation.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation/Animation.cs#L146) | Tries to read one live playback's state and position. |

### `Inno.Animation.AnimationBindingId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationBindingId`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L10) | Identifies one backend-neutral value destination inside an animation binding scope. |
| [`Inno.Animation.AnimationBindingId.AnimationBindingId(string value)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L18) | Creates a stable animation binding identifier. |
| [`bool Inno.Animation.AnimationBindingId.isValid`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L32) | Gets whether this identifier contains a usable value. |
| [`override string Inno.Animation.AnimationBindingId.ToString()`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L40) | Formats this identifier for diagnostics and persistence. |
| [`string Inno.Animation.AnimationBindingId.value`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L27) | Gets the stable binding protocol value. |

### `Inno.Animation.AnimationBindingProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationBindingProvider`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L8) | Applies one declared binding protocol on the owner thread; implementations never run in native callbacks. |
| [`abstract bool Inno.Animation.AnimationBindingProvider.TryApply(Inno.Animation.AnimationTarget target, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L22) | Attempts to apply a value to a destination resolved through Identity. |
| [`virtual void Inno.Animation.AnimationBindingProvider.Dispose()`](../../src/content/animation/Inno.Animation/AnimationBindingProvider.cs#L30) | Releases subscriptions and resources owned by this extension generation. |

### `Inno.Animation.AnimationBindingProviderAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationBindingId Inno.Animation.AnimationBindingProviderAttribute.bindingId`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L48) | Gets the supported track binding protocol. |
| [`Inno.Animation.AnimationBindingProviderAttribute`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L8) | Declares a stable, automatically discovered animation destination protocol. |
| [`Inno.Animation.AnimationBindingProviderAttribute.AnimationBindingProviderAttribute(string id, string bindingId, Inno.Animation.AnimationValueKind kind)`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L26) | Declares one provider and the value protocol it implements. |
| [`Inno.Animation.AnimationValueKind Inno.Animation.AnimationBindingProviderAttribute.kind`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L53) | Gets the accepted value representation. |
| [`string Inno.Animation.AnimationBindingProviderAttribute.id`](../../src/content/animation/Inno.Animation/AnimationBindingProviderAttribute.cs#L43) | Gets the stable provider identifier. |

### `Inno.Animation.AnimationClipAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationClipAsset`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L149) | Represents one imported backend-neutral collection of tracks and event markers. |
| [`Inno.Animation.AnimationEventMarker[] Inno.Animation.AnimationClipAsset.events`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L167) | Gets or sets ordered stable event markers. |
| [`Inno.Animation.AnimationTrack[] Inno.Animation.AnimationClipAsset.tracks`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L161) | Gets or sets the ordered backend-neutral value tracks. |
| [`float Inno.Animation.AnimationClipAsset.duration`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L155) | Gets or sets the positive clip duration in seconds. |
| [`void Inno.Animation.AnimationClipAsset.Validate()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L176) | Validates the complete clip contract before import or playback. |

### `Inno.Animation.AnimationClock`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationClock`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L222) | Selects the session clock used to advance an animation playback. |
| [`Inno.Animation.AnimationClock.Scaled`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L227) | Uses scaled simulation time and stops while the session is paused. |
| [`Inno.Animation.AnimationClock.Unscaled`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L232) | Uses unscaled session time and continues while simulation is paused. |

### `Inno.Animation.AnimationEventMarker`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationEventMarker`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L125) | Stores one stable event marker embedded in an animation clip. |
| [`byte[] Inno.Animation.AnimationEventMarker.payload`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L142) | Gets or sets opaque reload-safe event payload bytes. |
| [`float Inno.Animation.AnimationEventMarker.time`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L130) | Gets or sets clip-local marker time in seconds. |
| [`string Inno.Animation.AnimationEventMarker.eventId`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L136) | Gets or sets the stable event protocol identifier. |

### `Inno.Animation.AnimationExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationExecutionContext`](../../src/content/animation/Inno.Animation/Animation.cs#L10) | Binds one animation service to the current asynchronous execution context. |
| [`static Inno.Animation.IAnimationService Inno.Animation.AnimationExecutionContext.current`](../../src/content/animation/Inno.Animation/Animation.cs#L17) | Gets the animation service bound to the current execution context. |
| [`static System.IDisposable Inno.Animation.AnimationExecutionContext.EnterScope(Inno.Animation.IAnimationService animation)`](../../src/content/animation/Inno.Animation/Animation.cs#L28) | Binds one animation service until the returned strict scope is disposed. |

### `Inno.Animation.AnimationInterpolation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationInterpolation`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L206) | Selects how values are sampled between adjacent keyframes. |
| [`Inno.Animation.AnimationInterpolation.Linear`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L216) | Blends linearly between adjacent keyframes. |
| [`Inno.Animation.AnimationInterpolation.Step`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L211) | Holds the previous keyframe value until the next keyframe. |

### `Inno.Animation.AnimationKeyframe`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationKeyframe`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L14) | Stores one timestamped value inside an animation track. |
| [`Inno.Animation.AnimationKeyframe.AnimationKeyframe(float time, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L25) | Creates one animation keyframe. |
| [`Inno.Animation.AnimationValue Inno.Animation.AnimationKeyframe.value`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L44) | Gets or sets the sampled value. |
| [`float Inno.Animation.AnimationKeyframe.time`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L38) | Gets or sets clip-local time in seconds. |

### `Inno.Animation.AnimationMarkerEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationMarkerEvent`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L125) | Publishes a stable marker reached by one animation playback on the main runtime thread. |
| [`Inno.Animation.AnimationMarkerEvent.AnimationMarkerEvent(Inno.Animation.AnimationPlaybackHandle playback, string eventId, System.ReadOnlyMemory<byte> payload)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L139) | Creates one immutable animation marker event. |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.AnimationMarkerEvent.playback`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L155) | Gets the playback that crossed the marker. |
| [`System.ReadOnlyMemory<byte> Inno.Animation.AnimationMarkerEvent.payload`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L165) | Gets an immutable copy of the marker payload. |
| [`string Inno.Animation.AnimationMarkerEvent.eventId`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L160) | Gets the stable marker protocol identifier. |

### `Inno.Animation.AnimationPlayOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationClock Inno.Animation.AnimationPlayOptions.clock`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L290) | Gets or sets the session clock used by the playback. |
| [`Inno.Animation.AnimationPlayOptions`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L254) | Stores immutable options used to begin one animation playback. |
| [`bool Inno.Animation.AnimationPlayOptions.loop`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L285) | Gets or sets whether the clip repeats after its duration. |
| [`float Inno.Animation.AnimationPlayOptions.speed`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L270) | Gets or sets the non-negative playback speed multiplier. |
| [`float Inno.Animation.AnimationPlayOptions.weight`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L275) | Gets or sets the finite blend weight in the inclusive zero-to-one range. |
| [`int Inno.Animation.AnimationPlayOptions.layer`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L280) | Gets or sets the deterministic blend layer. |
| [`static Inno.Animation.AnimationPlayOptions Inno.Animation.AnimationPlayOptions.defaultValue`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L259) | Gets the default playback options. |

### `Inno.Animation.AnimationPlaybackAllocator`

| 当前声明 | 行为 |
| --- | --- |
| [`(int slot, uint generation) Inno.Animation.AnimationPlaybackAllocator.Decode(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L66) | Decodes only handles issued by this owner; slot liveness remains the runtime's responsibility. |
| [`Inno.Animation.AnimationPlaybackAllocator`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L9) | Owns opaque playback identity encoding without requiring a runtime to inherit a fake service base. |
| [`Inno.Animation.AnimationPlaybackAllocator.AnimationPlaybackAllocator()`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L19) | Creates a process-unique playback owner. |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.AnimationPlaybackAllocator.Create(int slot, uint slotGeneration)`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L47) | Encodes one owner-managed slot revision into an opaque playback handle. |
| [`uint Inno.Animation.AnimationPlaybackAllocator.generation`](../../src/content/animation/Inno.Animation/AnimationPlaybackAllocator.cs#L30) | Gets the generation shared by handles allocated by this owner. |

### `Inno.Animation.AnimationPlaybackHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationPlaybackHandle`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L296) | Identifies one generation-checked animation playback owned by a runtime service. |
| [`bool Inno.Animation.AnimationPlaybackHandle.isValid`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L309) | Gets whether the handle was initialized by an animation service. |
| [`uint Inno.Animation.AnimationPlaybackHandle.runtimeGeneration`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L316) | Gets the isolated runtime generation that created this handle. |

### `Inno.Animation.AnimationPlaybackState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationPlaybackState`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L238) | Describes the observable lifecycle of one animation playback. |
| [`Inno.Animation.AnimationPlaybackState.Paused`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L248) | The playback retains its position without advancing. |
| [`Inno.Animation.AnimationPlaybackState.Playing`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L243) | The playback is advancing and producing samples. |

### `Inno.Animation.AnimationSample`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationBindingId Inno.Animation.AnimationSample.binding`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L353) | Gets the stable target binding. |
| [`Inno.Animation.AnimationSample`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L322) | Stores one final blended value emitted for a stable binding identifier. |
| [`Inno.Animation.AnimationSample.AnimationSample(Inno.Animation.AnimationTarget target, Inno.Animation.AnimationBindingId binding, Inno.Animation.AnimationValue value)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L336) | Creates one final animation sample. |
| [`Inno.Animation.AnimationTarget Inno.Animation.AnimationSample.target`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L358) | Gets the generation-qualified destination for this sample. |
| [`Inno.Animation.AnimationValue Inno.Animation.AnimationSample.value`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L363) | Gets the final blended value. |

### `Inno.Animation.AnimationSampling`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationSampling`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L6) | Provides backend-neutral interpolation and weighted composition used by animation runtimes. |
| [`static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.AddWeighted(Inno.Animation.AnimationValue sum, Inno.Animation.AnimationValue value, float weight)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L45) | Adds one weighted value to an accumulation of the same kind. |
| [`static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.CompleteWeighted(Inno.Animation.AnimationValue sum, float inverseWeight)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L64) | Normalizes a weighted sum, including quaternion normalization. |
| [`static Inno.Animation.AnimationValue Inno.Animation.AnimationSampling.Interpolate(Inno.Animation.AnimationValue left, Inno.Animation.AnimationValue right, float amount)`](../../src/content/animation/Inno.Animation/AnimationSampling.cs#L23) | Interpolates compatible values, normalizing quaternion output. |

### `Inno.Animation.AnimationTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationTarget`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L9) | Identifies a transient animation destination without retaining the destination object. |
| [`Inno.Animation.AnimationTarget.AnimationTarget(Inno.Core.Identity.Identity identity)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L24) | Captures one currently registered destination in its original identity domain. |
| [`System.Guid Inno.Animation.AnimationTarget.persistentId`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L57) | Gets the persistent destination identifier, or empty for sampling-only playback. |
| [`TObject? Inno.Animation.AnimationTarget.Resolve<TObject>()`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L68) | Resolves the original runtime slot without binding to a replacement generation. |
| [`bool Inno.Animation.AnimationTarget.Equals(Inno.Animation.AnimationTarget other)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L79) | Compares runtime destinations, including their identity domain. |
| [`bool Inno.Animation.AnimationTarget.isSamplingOnly`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L47) | Gets whether this target deliberately omits object binding. |
| [`bool Inno.Animation.AnimationTarget.isValid`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L52) | Gets whether this value was initialized with a destination or sampling policy. |
| [`override bool Inno.Animation.AnimationTarget.Equals(object? obj)`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L91) | Compares a boxed destination with this value. |
| [`override int Inno.Animation.AnimationTarget.GetHashCode()`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L99) | Hashes the domain-qualified runtime destination. |
| [`static Inno.Animation.AnimationTarget Inno.Animation.AnimationTarget.samplingOnly`](../../src/content/animation/Inno.Animation/AnimationTarget.cs#L42) | Gets an explicit destination for sampling and markers without object binding. |

### `Inno.Animation.AnimationTrack`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationInterpolation Inno.Animation.AnimationTrack.interpolation`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L94) | Gets or sets the sampling rule between adjacent keyframes. |
| [`Inno.Animation.AnimationKeyframe[] Inno.Animation.AnimationTrack.keyframes`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L100) | Gets or sets ordered keyframes. |
| [`Inno.Animation.AnimationTrack`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L51) | Stores one ordered value track targeting a stable binding. |
| [`Inno.Animation.AnimationTrack.AnimationTrack()`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L56) | Creates an empty track for structured deserialization. |
| [`Inno.Animation.AnimationTrack.AnimationTrack(string bindingId, Inno.Animation.AnimationInterpolation interpolation, System.Collections.Generic.IEnumerable<Inno.Animation.AnimationKeyframe> keyframes)`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L72) | Creates one validated animation track. |
| [`string Inno.Animation.AnimationTrack.bindingId`](../../src/content/animation/Inno.Animation/AnimationClipAsset.cs#L88) | Gets or sets the stable target binding identifier. |

### `Inno.Animation.AnimationValue`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationValue`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L77) | Stores one backend-neutral scalar, vector, or quaternion animation value. |
| [`Inno.Animation.AnimationValue.AnimationValue(Inno.Animation.AnimationValueKind kind, float x, float y = 0, float z = 0, float w = 0)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L97) | Creates a four-component animation value. |
| [`Inno.Animation.AnimationValueKind Inno.Animation.AnimationValue.kind`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L114) | Gets or sets the component interpretation. |
| [`float Inno.Animation.AnimationValue.w`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L138) | Gets or sets the fourth component. |
| [`float Inno.Animation.AnimationValue.x`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L120) | Gets or sets the first component. |
| [`float Inno.Animation.AnimationValue.y`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L126) | Gets or sets the second component. |
| [`float Inno.Animation.AnimationValue.z`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L132) | Gets or sets the third component. |

### `Inno.Animation.AnimationValueKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationValueKind`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L46) | Describes the number and interpretation of components in an animation value. |
| [`Inno.Animation.AnimationValueKind.Quaternion`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L71) | Four normalized rotation components. |
| [`Inno.Animation.AnimationValueKind.Scalar`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L51) | A single scalar component. |
| [`Inno.Animation.AnimationValueKind.Vector2`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L56) | Two independent components. |
| [`Inno.Animation.AnimationValueKind.Vector3`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L61) | Three independent components. |
| [`Inno.Animation.AnimationValueKind.Vector4`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L66) | Four independent components. |

### `Inno.Animation.IAnimationBindingSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.IAnimationBindingSink`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L369) | Receives final backend-neutral samples after runtime blending has completed. |
| [`void Inno.Animation.IAnimationBindingSink.Apply(System.ReadOnlySpan<Inno.Animation.AnimationSample> samples)`](../../src/content/animation/Inno.Animation/AnimationTypes.cs#L377) | Applies one immutable frame of final animation samples. |

### `Inno.Animation.IAnimationService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L24) | Starts one clip with default playback options. |
| [`Inno.Animation.AnimationPlaybackHandle Inno.Animation.IAnimationService.Play(Inno.Animation.AnimationClipAsset clip, Inno.Animation.AnimationTarget target, Inno.Animation.AnimationPlayOptions options)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L44) | Starts one clip with explicit playback options. |
| [`Inno.Animation.IAnimationService`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L10) | Defines backend-neutral animation playback and control for one isolated runtime session. |
| [`bool Inno.Animation.IAnimationService.Pause(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L70) | Pauses one live playback at its current position. |
| [`bool Inno.Animation.IAnimationService.Resume(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L81) | Resumes one paused playback. |
| [`bool Inno.Animation.IAnimationService.Seek(Inno.Animation.AnimationPlaybackHandle handle, float time)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L95) | Seeks one live playback to a clip-local time. |
| [`bool Inno.Animation.IAnimationService.Stop(Inno.Animation.AnimationPlaybackHandle handle)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L59) | Stops one live playback. |
| [`bool Inno.Animation.IAnimationService.TryGetState(Inno.Animation.AnimationPlaybackHandle handle, out Inno.Animation.AnimationPlaybackState state, out float time)`](../../src/content/animation/Inno.Animation/IAnimationService.cs#L115) | Tries to read one live playback's state and position. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
