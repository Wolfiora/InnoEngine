# Inno.Audio

## Provider 退休约束

`AudioContentProvider.Dispose()` 的派生 hook 如果返回 `RetirementPendingException`，实例保持未退休，
所属 generation 必须保留它并在 owner thread 重试。普通异常表示失败而不是 Pending；由 Runtime 的 TypeRegistry
汇总并关闭共享 generation gate。禁止在 hook 未完成时提前标记 disposed，也不允许运行托管实时 DSP callback。

[Audio 索引](README.md) · [Runtime](Inno.Audio.Runtime.md) · [Assets](Inno.Audio.Assets.md) · [Wiki 首页](../README.md)

`Inno.Audio` 是后端中立且脚本稳定的音频契约。它引用 Assets、Events、Mathematics、Serialization、Type metadata 与 Scripting API，但不引用 Runtime、Scene、Editor、Platform、MiniAudio 或任何 Native 项目。

## 公开契约

| 分组 | 类型 | 稳定语义 |
| --- | --- | --- |
| 游戏入口 | `Audio`、`AudioExecutionContext`、`IAudioService` | façade 只解析当前严格 LIFO scope；真实状态由显式服务拥有。 |
| 设备边界 | `IAudioDevice`、`AudioDevice`、`AudioClipDescriptor`、`AudioDeviceCompletion`、`AudioClipState`、`AudioDeviceLimits` | Host/backend 使用的低层契约，GetClipState 明确准备进度；不进入脚本 API。 |
| 句柄 | `AudioClipHandle`、`AudioDeviceVoiceHandle`、`AudioBusHandle`、`AudioListenerHandle` | 编码设备 generation；旧设备和已退休对象的句柄安全失败。 |
| 游戏 Voice | `AudioVoiceHandle` / `AudioVoiceAllocator` | 服务级请求身份与设备 Voice 分离；准备中即可取得 handle，Runtime 不再继承 AudioDevice。 |
| 状态 | `AudioCapabilities`、`AudioDeviceState`、`AudioPlaybackState`、`AudioCompletionReason`、`AudioStatistics` | 查询能力、输出状态、播放状态和资源计数。 |
| 播放 | `AudioPlayOptions`、`AudioVoiceParameters`、`AudioSpatialOptions`、`AudioListenerState`、`AudioClipLoadMode`、`AudioDistanceModel` | 2D/基础 3D、scheduled start、loop、priority、route 与 load mode。 |
| ID | `AudioBusId`、`AudioProcessorId`、`AudioParameterId`、`AudioCodecId` | 开放字符串协议；`AudioBusId.master` 永远存在。 |
| Clip | `AudioClipAsset`、`AudioClipMetadata`、`AudioClipMetadataCodec` | Asset 对象与严格 metadata payload 编解码。 |
| Mixer | `AudioMixerAsset`、`AudioMixer`、`AudioMixerBuilder`、`AudioBusDefinition`、`AudioProcessorConfiguration`、`AudioProcessorParameter` | 构建并验证有向无环 Bus graph。 |
| 扩展 | `AudioMixerExtensionAttribute`、`AudioMixerFeatureExtensionAttribute`、`AudioMixerExtension`、`AudioMixerFeature`、`SerializedAudioExtensionState`、`AudioMixerFeatureConfiguration` | Stable ID + 中立 bytes；不持久化 Plugin `Type`、实例或 delegate。 |
| 内容输入 | `AudioContentProviderExtensionAttribute`、`AudioContentProvider`、`AudioContentProviderContext`、`AudioEmitterSnapshot`、`AudioListenerSnapshot` | Provider 单次控制线程贡献；总容量限制、整批接纳、返回后撤销，不依赖 Scene 模型。 |
| 事件/诊断 | `AudioVoiceCompletedEvent`；Core `DiagnosticSeverity` / `IDiagnosticReporter` | 完成事件进入主线程 dispatcher；诊断只有 Core 一套 producer/hub/sink。 |

`Audio` 提供 `Play`、`PlayScheduled`、`Stop`、`Pause`、`Resume`、`Seek`、`SetVoiceParameters`、`TryGetVoiceState`、Bus 控制、`PreloadAsync`、`ReleasePreload`，以及 `dspTime`、`capabilities`、`deviceState`、`statistics`。

## 参数与接纳边界

播放配置、Voice 参数与空间配置的构造函数拒绝 NaN/Infinity；空间向量的每个轴都检查有限值，
`AudioClipLoadMode` 与 `AudioDistanceModel` 必须是已定义值。原有范围规则仍生效，有限零向量没有新增方向归一化要求。
显式播放应使用 `AudioPlayOptions.defaultValue` 或 `new AudioPlayOptions(...)`，不能使用绕过构造函数的
`default(AudioPlayOptions)`；后者没有有效 Bus，服务在抢占 Voice 或取得 Artifact 前拒绝。
`default(AudioVoiceParameters)` 的 pitch 为零，`SetVoiceParameters` 返回 false，不覆盖当前 Voice 参数。
这些验证属于中立契约，不依赖 MiniAudio；直接设备调用同样拒绝无效默认配置、非有限/负 schedule 与非有限 Bus volume。

## Content Provider 工作流

Host 持有 `ContentReadScope`，Runtime 为每个 Provider 创建独立 `AudioContentProviderContext`。
Provider 只在 `Submit(context)` 内解析 Identity-backed 内容并调用 `context.Submit(snapshot)`；
不得保存 context、解析到的对象或在后台任务中继续提交，也不得释放借用的 content scope。

- `content`、`deltaTime` 提供本次内容与时间；`capacity` 是本次剩余的 emitter + listener 共同预算。
- `Submit` 拒绝 default snapshot、未初始化的 emitter options、重复 identity 与超限。一次提交失败会使整批无效；吞掉异常也不能让此前部分提交生效。
- `emitters` / `listeners` 返回冻结副本；它们是当前 invocation 的快照，不是跨 generation 的持久状态。
- `Dispose` 清掉暂存引用并撤销提交/读取；Runtime 在成功或失败返回后都执行它，不释放上层拥有的 content scope。
- 所有收集操作限定于创建 context 的线程。直接构造 context 的工具/测试必须自行 Dispose。
- 跨 Provider 重复由 Runtime 在整批接纳前验证。较早成功的 Provider 保留；失败 Provider 不占用后续 Provider 的 identity 或预算。

这些扩展仍通过本项目单一 `Properties/ScriptingApi.cs` 导出，不增加 Runtime 中央类型名单或托管音频 callback。

## Mixer 工作流

```csharp
using InnoEngine.Audio;

var builder = new AudioMixerBuilder();
var music = new AudioBusId("game.audio.bus.music");
builder.AddBus(music, AudioBusId.master, volume: 0.8f);
builder.AddProcessor(
    music,
    new AudioProcessorConfiguration(
        AudioProcessorId.lowPass,
        [new AudioProcessorParameter(AudioParameterId.frequency, 12000f)]));
AudioMixer mixer = builder.Build();
```

Builder 拒绝重复 Bus、缺失 parent、环和无效 processor 参数。标准 processor 包含 LPF、HPF、BPF、notch、peak、low/high shelf 与 delay；协议仍使用开放 ID，Plugin 可组合而不修改中央 enum。

## 生命周期与错误

- `Play` 可立即返回 `Preparing` Voice；准备、解码和 Artifact 定位由 Runtime 完成。
- 自然结束、显式停止、抢占、解码失败和设备丢失都通过 `AudioVoiceCompletedEvent` 区分。
- 无活动 `AudioExecutionContext` 时 façade 抛出 `InvalidOperationException`。
- `AudioDevice` 的 opaque handle 编解码只向后端派生类型开放；Native 类型不得进入 public/protected 签名。

`AudioDeviceLimits(clips = 16384, voices = 65536, buses = 4096)` 是 Host 的中立正容量配置，不导出到游戏脚本。voices 包含活动 Voice 和尚未交付的 completion；这层容量不是 AudioRuntime 的 priority/stealing 策略。设备接纳失败返回 invalid handle，调用者必须检查；不分配部分 native 对象，也不丢掉完成通知来接纳新播放。






## 本轮边界与所有权

AudioClipDescriptor 仅描述格式、长度、编码及模式；IAudioClipSource 提供可独立读取的编码 lease，不携带 artifactPath。具体设备负责自己的准备与解码能力。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Audio.Audio`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.Audio`](../../src/services/audio/Inno.Audio/Audio.cs#L46) | Provides script-friendly access to the audio service in the current execution context. |
| [`static Inno.Audio.AudioCapabilities Inno.Audio.Audio.capabilities`](../../src/services/audio/Inno.Audio/Audio.cs#L51) | Gets immutable capabilities for the current audio device generation. |
| [`static Inno.Audio.AudioDeviceState Inno.Audio.Audio.deviceState`](../../src/services/audio/Inno.Audio/Audio.cs#L56) | Gets the current output availability state. |
| [`static Inno.Audio.AudioStatistics Inno.Audio.Audio.statistics`](../../src/services/audio/Inno.Audio/Audio.cs#L66) | Gets current audio resource statistics. |
| [`static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/Audio.cs#L77) | Starts one clip using default playback parameters. |
| [`static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/Audio.cs#L91) | Starts one clip using explicit playback parameters. |
| [`static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.PlayScheduled(Inno.Audio.AudioClipAsset clip, double scheduledDspTime, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/Audio.cs#L111) | Schedules one clip against the monotonic audio clock. |
| [`static System.Threading.Tasks.ValueTask Inno.Audio.Audio.PreloadAsync(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/audio/Inno.Audio/Audio.cs#L270) | Prepares a clip and retains it in the runtime cache. |
| [`static bool Inno.Audio.Audio.Pause(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L138) | Pauses a live voice. |
| [`static bool Inno.Audio.Audio.Resume(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L149) | Resumes a paused voice. |
| [`static bool Inno.Audio.Audio.Seek(Inno.Audio.AudioVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/Audio.cs#L163) | Moves a live voice cursor to a clip-relative position. |
| [`static bool Inno.Audio.Audio.SetBusMuted(Inno.Audio.AudioBusId bus, bool muted)`](../../src/services/audio/Inno.Audio/Audio.cs#L233) | Updates mute state for a semantic mixer bus. |
| [`static bool Inno.Audio.Audio.SetBusPaused(Inno.Audio.AudioBusId bus, bool paused)`](../../src/services/audio/Inno.Audio/Audio.cs#L250) | Updates pause state for a semantic mixer bus. |
| [`static bool Inno.Audio.Audio.SetBusVolume(Inno.Audio.AudioBusId bus, float volume)`](../../src/services/audio/Inno.Audio/Audio.cs#L216) | Updates linear gain for a semantic mixer bus. |
| [`static bool Inno.Audio.Audio.SetVoiceParameters(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/Audio.cs#L180) | Replaces mutable parameters for a live voice. |
| [`static bool Inno.Audio.Audio.Stop(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L127) | Stops a live voice. |
| [`static bool Inno.Audio.Audio.TryGetVoiceState(Inno.Audio.AudioVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/Audio.cs#L198) | Queries the current state of a voice. |
| [`static double Inno.Audio.Audio.dspTime`](../../src/services/audio/Inno.Audio/Audio.cs#L61) | Gets the monotonic audio clock in seconds. |
| [`static void Inno.Audio.Audio.ReleasePreload(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/Audio.cs#L283) | Releases one explicit preload retention without interrupting active voices. |

### `Inno.Audio.AudioBusDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioBusDefinition`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L87) | Describes one bus and its ordered processor chain in a compiled mixer graph. |
| [`Inno.Audio.AudioBusDefinition.AudioBusDefinition(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusId? parent, float volume, bool muted, System.Collections.Generic.IEnumerable<Inno.Audio.AudioProcessorConfiguration>? processors = null)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L109) | Creates an immutable mixer bus definition. |
| [`Inno.Audio.AudioBusId Inno.Audio.AudioBusDefinition.id`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L134) | Gets the stable bus identifier. |
| [`Inno.Audio.AudioBusId? Inno.Audio.AudioBusDefinition.parent`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L139) | Gets the parent bus, or for the master bus. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioProcessorConfiguration> Inno.Audio.AudioBusDefinition.processors`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L154) | Gets the ordered immutable processor chain. |
| [`bool Inno.Audio.AudioBusDefinition.muted`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L149) | Gets whether output from the bus is initially silenced. |
| [`float Inno.Audio.AudioBusDefinition.volume`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L144) | Gets the initial linear bus gain. |

### `Inno.Audio.AudioBusHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioBusHandle`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Identifies one backend mixer bus within a device generation. |
| [`bool Inno.Audio.AudioBusHandle.isValid`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L78) | Gets whether the handle contains a non-zero backend identity and generation. |
| [`uint Inno.Audio.AudioBusHandle.deviceGeneration`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L73) | Gets the generation of the device that created this handle. |

### `Inno.Audio.AudioBusId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioBusId`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | Identifies a mixer bus without imposing a closed bus catalog. |
| [`Inno.Audio.AudioBusId.AudioBusId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L16) | Creates an open mixer bus identifier. |
| [`bool Inno.Audio.AudioBusId.isValid`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L35) | Gets whether this identifier contains a usable protocol value. |
| [`override string Inno.Audio.AudioBusId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L43) | Formats the identifier for diagnostics and persistence. |
| [`static Inno.Audio.AudioBusId Inno.Audio.AudioBusId.master`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L25) | Gets the mandatory root bus identifier. |
| [`string Inno.Audio.AudioBusId.value`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L30) | Gets the globally stable protocol value. |

### `Inno.Audio.AudioCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCapabilities`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Describes backend-neutral features and limits for one device generation. |
| [`Inno.Audio.AudioCapabilities.AudioCapabilities(bool supportsStreaming, bool supportsScheduledPlayback, bool supportsSpatialAudio, int maxListeners, int sampleRate)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L174) | Creates an immutable audio capability snapshot. |
| [`bool Inno.Audio.AudioCapabilities.supportsScheduledPlayback`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L198) | Gets whether voices can start against the audio clock. |
| [`bool Inno.Audio.AudioCapabilities.supportsSpatialAudio`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L203) | Gets whether listener-relative spatial playback is available. |
| [`bool Inno.Audio.AudioCapabilities.supportsStreaming`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L193) | Gets whether clips can be decoded incrementally. |
| [`int Inno.Audio.AudioCapabilities.maxListeners`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L208) | Gets the maximum simultaneously active listener count. |
| [`int Inno.Audio.AudioCapabilities.sampleRate`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L213) | Gets the output sample rate in frames per second. |

### `Inno.Audio.AudioClipAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipAsset`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L154) | Represents one imported audio source and its compact runtime metadata. |
| [`Inno.Audio.AudioClipMetadata? Inno.Audio.AudioClipAsset.metadata`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L162) | Gets imported metadata, or before runtime content is loaded. |
| [`System.TimeSpan Inno.Audio.AudioClipAsset.duration`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L167) | Gets the imported clip duration, or zero before runtime content is loaded. |
| [`override void Inno.Audio.AudioClipAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L178) | Refreshes imported metadata after an artifact commit. |

### `Inno.Audio.AudioClipDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipDescriptor`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Describes one immutable encoded audio artifact presented to a backend device. |
| [`Inno.Audio.AudioClipDescriptor.AudioClipDescriptor(Inno.Audio.AudioCodecId codec, Inno.Audio.AudioClipLoadMode loadMode, int channels, int sampleRate, long frameCount, long encodedByteLength)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L34) | Creates a backend clip description. |
| [`Inno.Audio.AudioClipLoadMode Inno.Audio.AudioClipDescriptor.loadMode`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L66) | Gets the required storage strategy. |
| [`Inno.Audio.AudioCodecId Inno.Audio.AudioClipDescriptor.codec`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L61) | Gets the encoded codec protocol. |
| [`int Inno.Audio.AudioClipDescriptor.channels`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L71) | Gets the encoded channel count. |
| [`int Inno.Audio.AudioClipDescriptor.sampleRate`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L76) | Gets the encoded sample rate in frames per second. |
| [`long Inno.Audio.AudioClipDescriptor.encodedByteLength`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L86) | Gets the encoded artifact length in bytes. |
| [`long Inno.Audio.AudioClipDescriptor.frameCount`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L81) | Gets the total decoded frame count, or zero when unknown. |

### `Inno.Audio.AudioClipHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipHandle`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Identifies one backend clip allocation within a device generation. |
| [`bool Inno.Audio.AudioClipHandle.isValid`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L26) | Gets whether the handle contains a non-zero backend identity and generation. |
| [`uint Inno.Audio.AudioClipHandle.deviceGeneration`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L21) | Gets the generation of the device that created this handle. |

### `Inno.Audio.AudioClipLoadMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipLoadMode`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L9) | Selects how encoded clip data is prepared for playback. |
| [`Inno.Audio.AudioClipLoadMode.Automatic`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L14) | Lets the runtime choose between decoded and streamed storage. |
| [`Inno.Audio.AudioClipLoadMode.Decode`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L19) | Decodes the complete clip before playback. |
| [`Inno.Audio.AudioClipLoadMode.Stream`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L24) | Decodes encoded data incrementally while playback advances. |

### `Inno.Audio.AudioClipMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipMetadata`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Describes imported encoded audio independently from its runtime storage path. |
| [`Inno.Audio.AudioClipMetadata.AudioClipMetadata(Inno.Audio.AudioCodecId codec, int channels, int sampleRate, long frameCount, long encodedByteLength)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L32) | Creates imported audio metadata. |
| [`Inno.Audio.AudioCodecId Inno.Audio.AudioClipMetadata.codec`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L55) | Gets the codec protocol of the encoded artifact. |
| [`System.TimeSpan Inno.Audio.AudioClipMetadata.duration`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L80) | Gets the clip duration derived from frame count and sample rate. |
| [`int Inno.Audio.AudioClipMetadata.channels`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L60) | Gets the encoded channel count. |
| [`int Inno.Audio.AudioClipMetadata.sampleRate`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L65) | Gets the sample rate in frames per second. |
| [`long Inno.Audio.AudioClipMetadata.encodedByteLength`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L75) | Gets the encoded artifact length in bytes. |
| [`long Inno.Audio.AudioClipMetadata.frameCount`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L70) | Gets the total decoded frame count. |

### `Inno.Audio.AudioClipMetadataCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipMetadataCodec`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L86) | Encodes the compact runtime header shared by audio importers and runtime asset loading. |
| [`static Inno.Audio.AudioClipMetadata Inno.Audio.AudioClipMetadataCodec.Decode(System.ReadOnlySpan<byte> payload)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L128) | Decodes and validates one compact audio runtime header. |
| [`static byte[] Inno.Audio.AudioClipMetadataCodec.Encode(Inno.Audio.AudioClipMetadata metadata)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L100) | Encodes audio metadata into a compact deterministic runtime header. |

### `Inno.Audio.AudioClipState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipState`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L6) | Describes asynchronous preparation of a backend-owned clip generation. |
| [`Inno.Audio.AudioClipState.Failed`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L19) | Preparation failed; the clip must be destroyed and callers notified. |
| [`Inno.Audio.AudioClipState.Preparing`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L11) | Native preparation is still in progress and the clip must not start yet. |
| [`Inno.Audio.AudioClipState.Ready`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L15) | The retained native data source is ready for playback. |

### `Inno.Audio.AudioCodecId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCodecId`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | Identifies encoded audio data without constraining codec providers to a closed enum. |
| [`Inno.Audio.AudioCodecId.AudioCodecId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L199) | Creates an open codec identifier. |
| [`bool Inno.Audio.AudioCodecId.isValid`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L228) | Gets whether this identifier contains a usable protocol value. |
| [`override string Inno.Audio.AudioCodecId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L236) | Formats the identifier for diagnostics and persistence. |
| [`static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.flac`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L213) | Gets the standard Free Lossless Audio Codec identifier. |
| [`static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.mp3`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L218) | Gets the standard MPEG Layer III codec identifier. |
| [`static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.wav`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L208) | Gets the standard Waveform Audio codec identifier. |
| [`string Inno.Audio.AudioCodecId.value`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L223) | Gets the globally stable protocol value. |

### `Inno.Audio.AudioCompletionReason`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCompletionReason`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L97) | Explains why a voice stopped accepting playback commands. |
| [`Inno.Audio.AudioCompletionReason.DecodeFailed`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L117) | Encoded data could not be prepared or decoded. |
| [`Inno.Audio.AudioCompletionReason.DeviceLost`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L122) | Playback ended because the owning device generation was lost. |
| [`Inno.Audio.AudioCompletionReason.NaturalEnd`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L102) | The clip reached its natural end. |
| [`Inno.Audio.AudioCompletionReason.Stolen`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L112) | The runtime reclaimed the voice to satisfy its voice budget. |
| [`Inno.Audio.AudioCompletionReason.Stopped`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L107) | A caller explicitly stopped the voice. |

### `Inno.Audio.AudioContentProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioContentProvider`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L252) | Converts arbitrary host-owned content into backend-neutral emitter and listener snapshots. |
| [`abstract void Inno.Audio.AudioContentProvider.Submit(Inno.Audio.AudioContentProviderContext context)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L262) | Submits current immutable audio snapshots on the host control thread. |
| [`virtual void Inno.Audio.AudioContentProvider.Dispose(bool disposing)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L297) | Releases managed generation-scoped state. |
| [`void Inno.Audio.AudioContentProvider.Dispose()`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L270) | Releases generation-scoped provider state. |

### `Inno.Audio.AudioContentProviderContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioContentProviderContext`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L50) | Collects one bounded, atomic provider contribution on the owning control thread. |
| [`Inno.Audio.AudioContentProviderContext.AudioContentProviderContext(Inno.References.ContentReadScope content, float deltaTime, int capacity = 1024)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L77) | Opens a control-thread snapshot collector over one content read scope. |
| [`Inno.References.ContentReadScope Inno.Audio.AudioContentProviderContext.content`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L100) | Gets explicit host content visible during this update. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioEmitterSnapshot> Inno.Audio.AudioContentProviderContext.emitters`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L177) | Gets a frozen copy of submitted source state in provider order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioListenerSnapshot> Inno.Audio.AudioContentProviderContext.listeners`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L195) | Gets a frozen copy of submitted listener state in provider order. |
| [`float Inno.Audio.AudioContentProviderContext.deltaTime`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L112) | Gets non-negative elapsed frame time in seconds. |
| [`int Inno.Audio.AudioContentProviderContext.capacity`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L117) | Gets the immutable combined snapshot budget assigned to this invocation. |
| [`void Inno.Audio.AudioContentProviderContext.Dispose()`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L210) | Revokes submission and releases collected references without disposing the borrowed content scope. |
| [`void Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioEmitterSnapshot emitter)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L134) | Submits one immutable emitter snapshot for runtime synchronization. |
| [`void Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioListenerSnapshot listener)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L161) | Submits one immutable listener snapshot for runtime selection. |

### `Inno.Audio.AudioContentProviderExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioContentProviderExtensionAttribute`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L11) | Marks a reloadable provider that converts host content into immutable audio snapshots. |
| [`Inno.Audio.AudioContentProviderExtensionAttribute.AudioContentProviderExtensionAttribute(string id, int priority = 0)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L23) | Creates an audio content provider declaration. |
| [`int Inno.Audio.AudioContentProviderExtensionAttribute.priority`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L40) | Gets the provider invocation priority. |
| [`string Inno.Audio.AudioContentProviderExtensionAttribute.id`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L35) | Gets the globally stable provider identifier. |

### `Inno.Audio.AudioDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDevice`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L6) | Provides protected opaque-handle encoding and validation helpers for audio backends. |
| [`static Inno.Audio.AudioBusHandle Inno.Audio.AudioDevice.CreateBusHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L68) | Encodes a bus identity into a backend-neutral handle. |
| [`static Inno.Audio.AudioClipHandle Inno.Audio.AudioDevice.CreateClipHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L34) | Encodes a clip identity into a backend-neutral handle. |
| [`static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioBusHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L121) | Decodes a bus handle for backend lookup and generation validation. |
| [`static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioClipHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L99) | Decodes a clip handle for backend lookup and generation validation. |
| [`static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioDeviceVoiceHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L110) | Decodes a voice handle for backend lookup and generation validation. |
| [`static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioListenerHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L132) | Decodes a listener handle for backend lookup and generation validation. |
| [`static Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.AudioDevice.CreateVoiceHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L51) | Encodes a voice identity into a backend-neutral handle. |
| [`static Inno.Audio.AudioListenerHandle Inno.Audio.AudioDevice.CreateListenerHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L85) | Encodes a listener identity into a backend-neutral handle. |

### `Inno.Audio.AudioDevice.DeviceHandleIdentity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDevice.DeviceHandleIdentity`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Stores one decoded device-owned identity. |

### `Inno.Audio.AudioDeviceCompletion`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCompletionReason Inno.Audio.AudioDeviceCompletion.reason`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L37) | Gets the terminal playback reason. |
| [`Inno.Audio.AudioDeviceCompletion`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Reports one backend-detected terminal voice transition. |
| [`Inno.Audio.AudioDeviceCompletion.AudioDeviceCompletion(Inno.Audio.AudioDeviceVoiceHandle voice, Inno.Audio.AudioCompletionReason reason)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L19) | Creates a backend voice completion record. |
| [`Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.AudioDeviceCompletion.voice`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L32) | Gets the completed voice handle. |

### `Inno.Audio.AudioDeviceLimits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDeviceLimits`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L8) | Bounds device-side resource admission independently of the Runtime's smaller gameplay voice budget. |
| [`Inno.Audio.AudioDeviceLimits.AudioDeviceLimits(int clips = 16384, int voices = 65536, int buses = 4096)`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L25) | Creates immutable positive capacities for one backend generation. |
| [`int Inno.Audio.AudioDeviceLimits.buses`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L49) | Gets the maximum retained bus count. |
| [`int Inno.Audio.AudioDeviceLimits.clips`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L41) | Gets the maximum retained clip count. |
| [`int Inno.Audio.AudioDeviceLimits.voices`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L45) | Gets the combined voice and undelivered completion capacity. |

### `Inno.Audio.AudioDeviceState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDeviceState`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L30) | Describes the availability of the active audio output generation. |
| [`Inno.Audio.AudioDeviceState.Disposed`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L55) | The device has been released and cannot accept work. |
| [`Inno.Audio.AudioDeviceState.Initializing`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L35) | The device has not completed initialization. |
| [`Inno.Audio.AudioDeviceState.Lost`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L50) | The active output device was lost and recovery is pending. |
| [`Inno.Audio.AudioDeviceState.Muted`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L45) | Playback is advancing without an output device. |
| [`Inno.Audio.AudioDeviceState.Ready`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L40) | The device is available for audible playback. |

### `Inno.Audio.AudioDeviceVoiceHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDeviceVoiceHandle`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Identifies a backend voice allocation, distinct from a service's preparing playback handle. |
| [`bool Inno.Audio.AudioDeviceVoiceHandle.isValid`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L24) | Gets whether this value identifies a backend allocation. |
| [`uint Inno.Audio.AudioDeviceVoiceHandle.deviceGeneration`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L20) | Gets the backend device generation that owns this voice. |

### `Inno.Audio.AudioDistanceModel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDistanceModel`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L128) | Selects the distance attenuation model for one spatial voice. |
| [`Inno.Audio.AudioDistanceModel.Exponential`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L148) | Uses exponential-distance attenuation. |
| [`Inno.Audio.AudioDistanceModel.Inverse`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L138) | Uses inverse-distance attenuation. |
| [`Inno.Audio.AudioDistanceModel.Linear`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L143) | Uses linear-distance attenuation. |
| [`Inno.Audio.AudioDistanceModel.None`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L133) | Disables distance attenuation while retaining spatial direction. |

### `Inno.Audio.AudioEmitterSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioClipAsset Inno.Audio.AudioEmitterSnapshot.clip`](../../src/services/audio/Inno.Audio/AudioContent.cs#L52) | Gets the imported clip requested by the emitter. |
| [`Inno.Audio.AudioEmitterSnapshot`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Describes one immutable source playback request produced by a content provider. |
| [`Inno.Audio.AudioEmitterSnapshot.AudioEmitterSnapshot(System.Guid id, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options, bool shouldPlay, ulong playbackRevision = 0)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L28) | Creates an immutable emitter snapshot. |
| [`Inno.Audio.AudioPlayOptions Inno.Audio.AudioEmitterSnapshot.options`](../../src/services/audio/Inno.Audio/AudioContent.cs#L57) | Gets current playback and spatial parameters. |
| [`System.Guid Inno.Audio.AudioEmitterSnapshot.id`](../../src/services/audio/Inno.Audio/AudioContent.cs#L47) | Gets the stable emitter identity. |
| [`bool Inno.Audio.AudioEmitterSnapshot.shouldPlay`](../../src/services/audio/Inno.Audio/AudioContent.cs#L62) | Gets whether the runtime should retain active playback for this emitter. |
| [`ulong Inno.Audio.AudioEmitterSnapshot.playbackRevision`](../../src/services/audio/Inno.Audio/AudioContent.cs#L67) | Gets the source-owned revision that distinguishes explicit replay requests. |

### `Inno.Audio.AudioExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioExecutionContext`](../../src/services/audio/Inno.Audio/Audio.cs#L11) | Binds one host-owned audio service to the current asynchronous execution context. |
| [`static Inno.Audio.IAudioService Inno.Audio.AudioExecutionContext.current`](../../src/services/audio/Inno.Audio/Audio.cs#L21) | Gets the audio service bound to the current asynchronous execution context. |
| [`static System.IDisposable Inno.Audio.AudioExecutionContext.EnterScope(Inno.Audio.IAudioService audio)`](../../src/services/audio/Inno.Audio/Audio.cs#L35) | Binds an audio service until the returned strict last-in-first-out scope is disposed. |

### `Inno.Audio.AudioListenerHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioListenerHandle`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Identifies one spatial listener within a device generation. |
| [`bool Inno.Audio.AudioListenerHandle.isValid`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L104) | Gets whether the handle contains a non-zero backend identity and generation. |
| [`uint Inno.Audio.AudioListenerHandle.deviceGeneration`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L99) | Gets the generation of the device that created this handle. |

### `Inno.Audio.AudioListenerSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioListenerSnapshot`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Describes one immutable listener candidate produced by a content provider. |
| [`Inno.Audio.AudioListenerSnapshot.AudioListenerSnapshot(System.Guid id, int priority, Inno.Audio.AudioListenerState state, bool active)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L90) | Creates an immutable listener snapshot. |
| [`Inno.Audio.AudioListenerState Inno.Audio.AudioListenerSnapshot.state`](../../src/services/audio/Inno.Audio/AudioContent.cs#L117) | Gets the current listener transform. |
| [`System.Guid Inno.Audio.AudioListenerSnapshot.id`](../../src/services/audio/Inno.Audio/AudioContent.cs#L107) | Gets the stable listener identity. |
| [`bool Inno.Audio.AudioListenerSnapshot.active`](../../src/services/audio/Inno.Audio/AudioContent.cs#L122) | Gets whether the listener is eligible for selection. |
| [`int Inno.Audio.AudioListenerSnapshot.priority`](../../src/services/audio/Inno.Audio/AudioContent.cs#L112) | Gets the selection priority. |

### `Inno.Audio.AudioListenerState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioListenerState`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Contains one listener transform used by backend spatialization. |
| [`Inno.Audio.AudioListenerState.AudioListenerState(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 direction, Inno.Core.Mathematics.Vector3 up, Inno.Core.Mathematics.Vector3 velocity)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L623) | Creates a listener transform snapshot. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.direction`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L647) | Gets the world-space listener forward direction. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.position`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L642) | Gets the world-space listener position. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.up`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L652) | Gets the world-space listener up direction. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.velocity`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L657) | Gets the world-space listener velocity. |

### `Inno.Audio.AudioMixer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixer`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L160) | Contains one validated backend-neutral mixer graph. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioBusDefinition> Inno.Audio.AudioMixer.buses`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L172) | Gets buses in parent-before-child creation order. |

### `Inno.Audio.AudioMixerAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerAsset`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L383) | Selects a mixer extension and ordered features without defining a game-specific mixing model. |
| [`Inno.Audio.AudioMixerFeatureConfiguration[] Inno.Audio.AudioMixerAsset.features`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L403) | Gets or sets ordered feature configurations. |
| [`Inno.Audio.SerializedAudioExtensionState Inno.Audio.AudioMixerAsset.mixerState`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L397) | Gets or sets reload-safe mixer settings. |
| [`string Inno.Audio.AudioMixerAsset.mixerTypeId`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L391) | Gets or sets the globally stable mixer extension identifier. |

### `Inno.Audio.AudioMixerBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixer Inno.Audio.AudioMixerBuilder.Build()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L252) | Validates the graph and returns an immutable parent-before-child snapshot. |
| [`Inno.Audio.AudioMixerBuilder`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L178) | Builds and validates an open backend-neutral mixer graph. |
| [`void Inno.Audio.AudioMixerBuilder.AddBus(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusId parent, float volume = 1, bool muted = false)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L203) | Adds one semantic bus to the graph. |
| [`void Inno.Audio.AudioMixerBuilder.AddProcessor(Inno.Audio.AudioBusId bus, Inno.Audio.AudioProcessorConfiguration processor)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L228) | Appends one processor to an existing bus chain. |

### `Inno.Audio.AudioMixerExtension`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerExtension`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L67) | Builds a base mixer graph on the host control thread. |
| [`abstract void Inno.Audio.AudioMixerExtension.Build(Inno.Audio.AudioMixerBuilder builder, Inno.Audio.SerializedAudioExtensionState state)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L78) | Adds base buses and processors to a candidate mixer graph. |

### `Inno.Audio.AudioMixerExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerExtensionAttribute`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L8) | Marks a reloadable extension that creates the base audio mixer graph. |
| [`Inno.Audio.AudioMixerExtensionAttribute.AudioMixerExtensionAttribute(string id)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L17) | Creates a mixer extension declaration. |
| [`string Inno.Audio.AudioMixerExtensionAttribute.id`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L26) | Gets the globally stable mixer extension identifier. |

### `Inno.Audio.AudioMixerFeature`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerFeature`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L87) | Contributes optional buses and processors to a candidate mixer graph. |
| [`abstract void Inno.Audio.AudioMixerFeature.Build(Inno.Audio.AudioMixerBuilder builder, Inno.Audio.SerializedAudioExtensionState state)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L98) | Adds feature-owned graph declarations on the host control thread. |

### `Inno.Audio.AudioMixerFeatureConfiguration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerFeatureConfiguration`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L329) | Stores one ordered mixer feature selection using only stable data. |
| [`Inno.Audio.AudioMixerFeatureConfiguration.AudioMixerFeatureConfiguration()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L334) | Creates an empty feature configuration for deserialization. |
| [`Inno.Audio.AudioMixerFeatureConfiguration.AudioMixerFeatureConfiguration(string featureTypeId, Inno.Audio.SerializedAudioExtensionState? state = null, bool enabled = true)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L350) | Creates a mixer feature configuration. |
| [`Inno.Audio.SerializedAudioExtensionState Inno.Audio.AudioMixerFeatureConfiguration.state`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L370) | Gets or sets reload-safe feature settings. |
| [`bool Inno.Audio.AudioMixerFeatureConfiguration.enabled`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L376) | Gets or sets whether the feature participates in graph construction. |
| [`string Inno.Audio.AudioMixerFeatureConfiguration.featureTypeId`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L364) | Gets or sets the stable feature extension identifier. |

### `Inno.Audio.AudioMixerFeatureExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerFeatureExtensionAttribute`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L32) | Marks a reloadable extension that contributes to an audio mixer graph. |
| [`Inno.Audio.AudioMixerFeatureExtensionAttribute.AudioMixerFeatureExtensionAttribute(string id, int priority = 0)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L44) | Creates a mixer feature declaration. |
| [`int Inno.Audio.AudioMixerFeatureExtensionAttribute.priority`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L61) | Gets the feature invocation priority. |
| [`string Inno.Audio.AudioMixerFeatureExtensionAttribute.id`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L56) | Gets the globally stable feature extension identifier. |

### `Inno.Audio.AudioParameterId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioParameterId`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | Identifies one processor parameter without imposing a processor-specific enum. |
| [`Inno.Audio.AudioParameterId.AudioParameterId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L133) | Creates an open processor parameter identifier. |
| [`bool Inno.Audio.AudioParameterId.isValid`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L177) | Gets whether this identifier contains a usable protocol value. |
| [`override string Inno.Audio.AudioParameterId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L185) | Formats the identifier for diagnostics and persistence. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.decay`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L167) | Gets the standard delay feedback decay factor. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.delayMilliseconds`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L162) | Gets the standard delay duration parameter in milliseconds. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.frequency`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L142) | Gets the standard cutoff or center-frequency parameter in hertz. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.gainDecibels`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L152) | Gets the standard equalizer gain parameter in decibels. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.quality`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L147) | Gets the standard filter quality-factor parameter. |
| [`static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.shelfSlope`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L157) | Gets the standard equalizer shelf-slope parameter. |
| [`string Inno.Audio.AudioParameterId.value`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L172) | Gets the stable parameter protocol value. |

### `Inno.Audio.AudioPlayOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioBusId Inno.Audio.AudioPlayOptions.bus`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L525) | Gets the destination mixer bus. |
| [`Inno.Audio.AudioClipLoadMode Inno.Audio.AudioPlayOptions.loadMode`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L530) | Gets the clip preparation override. |
| [`Inno.Audio.AudioPlayOptions`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Contains immutable parameters used to create one playback voice. |
| [`Inno.Audio.AudioPlayOptions.AudioPlayOptions()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L423) | Creates default playback parameters. |
| [`Inno.Audio.AudioPlayOptions.AudioPlayOptions(float volume = 1, float pitch = 1, float pan = 0, bool loop = false, int priority = 0, Inno.Audio.AudioBusId? bus = null, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, Inno.Audio.AudioSpatialOptions? spatial = null)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L461) | Creates playback parameters. |
| [`Inno.Audio.AudioSpatialOptions? Inno.Audio.AudioPlayOptions.spatial`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L535) | Gets optional spatial playback parameters. |
| [`bool Inno.Audio.AudioPlayOptions.loop`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L515) | Gets whether playback loops at the clip end. |
| [`float Inno.Audio.AudioPlayOptions.pan`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L510) | Gets the stereo pan value. |
| [`float Inno.Audio.AudioPlayOptions.pitch`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L505) | Gets the playback-rate multiplier. |
| [`float Inno.Audio.AudioPlayOptions.volume`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L500) | Gets the linear voice gain. |
| [`int Inno.Audio.AudioPlayOptions.priority`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L520) | Gets the voice retention priority. |
| [`static Inno.Audio.AudioPlayOptions Inno.Audio.AudioPlayOptions.defaultValue`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L495) | Gets the default playback parameters. |

### `Inno.Audio.AudioPlaybackState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioPlaybackState`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L61) | Describes the observable lifecycle of one playback voice. |
| [`Inno.Audio.AudioPlaybackState.Completed`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L86) | The voice has completed and no longer accepts control commands. |
| [`Inno.Audio.AudioPlaybackState.Invalid`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L91) | The handle is invalid, stale, or unknown to the active runtime. |
| [`Inno.Audio.AudioPlaybackState.Paused`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L81) | The voice is paused at its current cursor. |
| [`Inno.Audio.AudioPlaybackState.Playing`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L76) | The voice is actively advancing. |
| [`Inno.Audio.AudioPlaybackState.Preparing`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L66) | The voice is waiting for clip preparation. |
| [`Inno.Audio.AudioPlaybackState.Scheduled`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L71) | The voice is scheduled against the audio clock. |

### `Inno.Audio.AudioProcessorConfiguration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioProcessorConfiguration`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L48) | Describes one ordered processor instance attached to a mixer bus. |
| [`Inno.Audio.AudioProcessorConfiguration.AudioProcessorConfiguration(Inno.Audio.AudioProcessorId id, System.Collections.Generic.IEnumerable<Inno.Audio.AudioProcessorParameter>? parameters = null)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L61) | Creates an immutable processor configuration. |
| [`Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorConfiguration.id`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L76) | Gets the open processor protocol identifier. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioProcessorParameter> Inno.Audio.AudioProcessorConfiguration.parameters`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L81) | Gets the immutable processor parameter set. |

### `Inno.Audio.AudioProcessorId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioProcessorId`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | Identifies an audio processor implementation without imposing a closed effect catalog. |
| [`Inno.Audio.AudioProcessorId.AudioProcessorId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L57) | Creates an open processor identifier. |
| [`bool Inno.Audio.AudioProcessorId.isValid`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L111) | Gets whether this identifier contains a usable protocol value. |
| [`override string Inno.Audio.AudioProcessorId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L119) | Formats the identifier for diagnostics and persistence. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.bandPass`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L76) | Gets the standard band-pass filter processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.delay`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L101) | Gets the standard delay processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.highPass`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L71) | Gets the standard high-pass filter processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.highShelf`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L96) | Gets the standard high-shelf equalizer processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.lowPass`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L66) | Gets the standard low-pass filter processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.lowShelf`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L91) | Gets the standard low-shelf equalizer processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.notch`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L81) | Gets the standard notch filter processor identifier. |
| [`static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.peak`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L86) | Gets the standard peak equalizer processor identifier. |
| [`string Inno.Audio.AudioProcessorId.value`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L106) | Gets the globally stable protocol value. |

### `Inno.Audio.AudioProcessorParameter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioParameterId Inno.Audio.AudioProcessorParameter.id`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L37) | Gets the open parameter identifier. |
| [`Inno.Audio.AudioProcessorParameter`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Stores one neutral numeric parameter assigned to an audio processor. |
| [`Inno.Audio.AudioProcessorParameter.AudioProcessorParameter(Inno.Audio.AudioParameterId id, float value)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L24) | Creates a processor parameter value. |
| [`float Inno.Audio.AudioProcessorParameter.value`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L42) | Gets the neutral numeric value. |

### `Inno.Audio.AudioProjectSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioMixerAsset? Inno.Audio.AudioProjectSettings.defaultMixer`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L27) | Gets or sets the default mixer asset, or for the master-only graph. |
| [`Inno.Audio.AudioProjectSettings`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L10) | Stores portable project-wide defaults for audio playback and caching. |
| [`const string Inno.Audio.AudioProjectSettings.settingProtocolId`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L17) | Gets the immutable project-setting protocol value used by discovery metadata. |
| [`float Inno.Audio.AudioProjectSettings.masterVolume`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L33) | Gets or sets the initial non-negative master bus gain. |
| [`int Inno.Audio.AudioProjectSettings.maxVoices`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L39) | Gets or sets the positive maximum active voice count. |
| [`long Inno.Audio.AudioProjectSettings.automaticStreamingThresholdBytes`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L51) | Gets or sets the encoded byte threshold used by automatic streaming selection. |
| [`long Inno.Audio.AudioProjectSettings.decodedCacheBudgetBytes`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L45) | Gets or sets the decoded clip cache budget in bytes. |
| [`static Inno.Core.Settings.ProjectSettingId Inno.Audio.AudioProjectSettings.settingId`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L22) | Gets the stable project-setting identity for audio runtime defaults. |

### `Inno.Audio.AudioSpatialOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioDistanceModel Inno.Audio.AudioSpatialOptions.distanceModel`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L377) | Gets the distance attenuation model. |
| [`Inno.Audio.AudioSpatialOptions`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Contains listener-relative positioning and attenuation parameters for one voice. |
| [`Inno.Audio.AudioSpatialOptions.AudioSpatialOptions(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 direction, Inno.Core.Mathematics.Vector3 velocity, Inno.Audio.AudioDistanceModel distanceModel = Inno.Audio.AudioDistanceModel.Inverse, float minDistance = 1, float maxDistance = 100, float rolloff = 1, float coneInnerAngle = 360, float coneOuterAngle = 360, float coneOuterGain = 1, float dopplerFactor = 1)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L317) | Creates spatial playback parameters. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.direction`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L367) | Gets the world-space source forward direction. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.position`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L362) | Gets the world-space source position. |
| [`Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.velocity`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L372) | Gets the world-space source velocity. |
| [`float Inno.Audio.AudioSpatialOptions.coneInnerAngle`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L397) | Gets the full-volume cone angle in degrees. |
| [`float Inno.Audio.AudioSpatialOptions.coneOuterAngle`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L402) | Gets the outer cone angle in degrees. |
| [`float Inno.Audio.AudioSpatialOptions.coneOuterGain`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L407) | Gets the gain outside the outer cone. |
| [`float Inno.Audio.AudioSpatialOptions.dopplerFactor`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L412) | Gets the Doppler effect multiplier. |
| [`float Inno.Audio.AudioSpatialOptions.maxDistance`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L387) | Gets the distance beyond which attenuation is clamped. |
| [`float Inno.Audio.AudioSpatialOptions.minDistance`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L382) | Gets the distance at which attenuation begins. |
| [`float Inno.Audio.AudioSpatialOptions.rolloff`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L392) | Gets the distance attenuation strength. |

### `Inno.Audio.AudioStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioStatistics`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Reports an immutable snapshot of runtime and backend resource usage. |
| [`Inno.Audio.AudioStatistics.AudioStatistics(int activeVoices, int loadedClips, long decodedBytes, long stolenVoiceCount)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L236) | Creates an audio statistics snapshot. |
| [`int Inno.Audio.AudioStatistics.activeVoices`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L255) | Gets voices that currently consume runtime budget. |
| [`int Inno.Audio.AudioStatistics.loadedClips`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L260) | Gets clip allocations held by the backend. |
| [`long Inno.Audio.AudioStatistics.decodedBytes`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L265) | Gets approximate decoded clip bytes held in memory. |
| [`long Inno.Audio.AudioStatistics.stolenVoiceCount`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L270) | Gets voices reclaimed since runtime creation. |

### `Inno.Audio.AudioVoiceAllocator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioVoiceAllocator`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L9) | Allocates service-owned playback handles without impersonating a backend audio device. |
| [`Inno.Audio.AudioVoiceAllocator.AudioVoiceAllocator()`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L21) | Creates an isolated playback identity owner. |
| [`Inno.Audio.AudioVoiceHandle Inno.Audio.AudioVoiceAllocator.Allocate()`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L43) | Allocates a unique service handle that cannot alias a different service's playback. |
| [`uint Inno.Audio.AudioVoiceAllocator.generation`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L32) | Gets the process-unique owner generation encoded in every allocated playback handle. |

### `Inno.Audio.AudioVoiceCompletedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCompletionReason Inno.Audio.AudioVoiceCompletedEvent.reason`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L38) | Gets the terminal playback reason. |
| [`Inno.Audio.AudioVoiceCompletedEvent`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L9) | Reports one voice terminal transition on the owning runtime's main-thread event dispatcher. |
| [`Inno.Audio.AudioVoiceCompletedEvent.AudioVoiceCompletedEvent(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioCompletionReason reason)`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L20) | Creates a voice completion event. |
| [`Inno.Audio.AudioVoiceHandle Inno.Audio.AudioVoiceCompletedEvent.voice`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L33) | Gets the completed voice handle. |

### `Inno.Audio.AudioVoiceHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioVoiceHandle`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Identifies one playback voice within a device generation. |
| [`bool Inno.Audio.AudioVoiceHandle.isValid`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L52) | Gets whether the handle contains a non-zero backend identity and generation. |
| [`uint Inno.Audio.AudioVoiceHandle.ownerGeneration`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L47) | Gets the generation of the device that created this handle. |

### `Inno.Audio.AudioVoiceParameters`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioSpatialOptions? Inno.Audio.AudioVoiceParameters.spatial`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L597) | Gets optional spatial playback parameters. |
| [`Inno.Audio.AudioVoiceParameters`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Contains mutable voice parameters that can be changed after playback begins. |
| [`Inno.Audio.AudioVoiceParameters.AudioVoiceParameters(float volume, float pitch, float pan, Inno.Audio.AudioSpatialOptions? spatial = null)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L561) | Creates a voice parameter snapshot. |
| [`float Inno.Audio.AudioVoiceParameters.pan`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L592) | Gets the stereo pan value. |
| [`float Inno.Audio.AudioVoiceParameters.pitch`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L587) | Gets the playback-rate multiplier. |
| [`float Inno.Audio.AudioVoiceParameters.volume`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L582) | Gets the linear voice gain. |

### `Inno.Audio.IAudioClipSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.IAudioClipSource`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L8) | Supplies immutable encoded audio independently of any backend's input layout. |
| [`System.IO.Stream Inno.Audio.IAudioClipSource.OpenRead()`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L26) | Opens a reader with an independent content pin and cursor. |
| [`long Inno.Audio.IAudioClipSource.length`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L18) | Gets the exact encoded byte length used to bound preparation. |
| [`string Inno.Audio.IAudioClipSource.contentHash`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L13) | Gets the SHA-256 identity of the complete encoded bytes. |

### `Inno.Audio.IAudioDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioBusHandle Inno.Audio.IAudioDevice.CreateBus(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusHandle parent = default(Inno.Audio.AudioBusHandle))`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L231) | Creates one bus routed to a parent bus. |
| [`Inno.Audio.AudioCapabilities Inno.Audio.IAudioDevice.capabilities`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L59) | Gets immutable capabilities for the active device generation. |
| [`Inno.Audio.AudioClipHandle Inno.Audio.IAudioDevice.CreateClip(Inno.Audio.AudioClipDescriptor descriptor, Inno.Audio.IAudioClipSource source)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L94) | Starts preparation of one decoded or streamed clip from immutable encoded content. |
| [`Inno.Audio.AudioClipState Inno.Audio.IAudioDevice.GetClipState(Inno.Audio.AudioClipHandle clip)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L54) | Queries asynchronous clip preparation without blocking the owner thread. |
| [`Inno.Audio.AudioDeviceState Inno.Audio.IAudioDevice.state`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L69) | Gets the current output availability state. |
| [`Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.IAudioDevice.Play(Inno.Audio.AudioClipHandle clip, Inno.Audio.AudioBusHandle bus, Inno.Audio.AudioPlayOptions options, double? scheduledDspTime = null)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L128) | Creates and starts or schedules one playback voice. |
| [`Inno.Audio.AudioListenerHandle Inno.Audio.IAudioDevice.CreateListener(Inno.Audio.AudioListenerState state)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L324) | Creates one backend spatial listener. |
| [`Inno.Audio.AudioStatistics Inno.Audio.IAudioDevice.statistics`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L79) | Gets current backend resource statistics. |
| [`Inno.Audio.IAudioDevice`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L43) | Owns one replaceable audio backend generation and all device-side audio objects. |
| [`bool Inno.Audio.IAudioDevice.AddBusProcessor(Inno.Audio.AudioBusHandle bus, Inno.Audio.AudioProcessorConfiguration processor)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L310) | Appends one backend-neutral processor configuration to a bus chain. |
| [`bool Inno.Audio.IAudioDevice.DestroyBus(Inno.Audio.AudioBusHandle bus)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L245) | Releases one graph-generation bus after dependent objects have been removed. |
| [`bool Inno.Audio.IAudioDevice.DestroyClip(Inno.Audio.AudioClipHandle clip)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L108) | Releases one clip after all voices using it have ended. |
| [`bool Inno.Audio.IAudioDevice.DestroyListener(Inno.Audio.AudioListenerHandle listener)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L352) | Releases one backend spatial listener. |
| [`bool Inno.Audio.IAudioDevice.Pause(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L155) | Pauses a live voice. |
| [`bool Inno.Audio.IAudioDevice.Resume(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L166) | Resumes a paused voice. |
| [`bool Inno.Audio.IAudioDevice.Seek(Inno.Audio.AudioDeviceVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L180) | Moves a live voice cursor to a clip-relative position. |
| [`bool Inno.Audio.IAudioDevice.SetBusMuted(Inno.Audio.AudioBusHandle bus, bool muted)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L276) | Updates mute state for one bus. |
| [`bool Inno.Audio.IAudioDevice.SetBusPaused(Inno.Audio.AudioBusHandle bus, bool paused)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L293) | Updates pause state for one bus and its routed voices. |
| [`bool Inno.Audio.IAudioDevice.SetBusVolume(Inno.Audio.AudioBusHandle bus, float volume)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L259) | Updates linear gain for one bus. |
| [`bool Inno.Audio.IAudioDevice.SetListener(Inno.Audio.AudioListenerHandle listener, Inno.Audio.AudioListenerState state)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L338) | Updates one backend spatial listener. |
| [`bool Inno.Audio.IAudioDevice.SetVoiceParameters(Inno.Audio.AudioDeviceVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L197) | Replaces mutable parameters for a live voice. |
| [`bool Inno.Audio.IAudioDevice.Stop(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L144) | Stops a voice and makes its handle terminal. |
| [`bool Inno.Audio.IAudioDevice.TryDequeueCompletion(out Inno.Audio.AudioDeviceCompletion completion)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L371) | Tries to consume one terminal voice transition recorded by the backend. |
| [`bool Inno.Audio.IAudioDevice.TryGetVoiceState(Inno.Audio.AudioDeviceVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L214) | Queries the current playback state for a voice. |
| [`double Inno.Audio.IAudioDevice.dspTime`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L74) | Gets the monotonic backend audio clock in seconds. |
| [`uint Inno.Audio.IAudioDevice.generation`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L64) | Gets the non-zero generation used to reject stale handles. |
| [`void Inno.Audio.IAudioDevice.Update(float deltaTime)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L360) | Advances backend maintenance at a main-thread safety point. |

### `Inno.Audio.IAudioService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioCapabilities Inno.Audio.IAudioService.capabilities`](../../src/services/audio/Inno.Audio/IAudioService.cs#L15) | Gets immutable capabilities for the current device generation. |
| [`Inno.Audio.AudioDeviceState Inno.Audio.IAudioService.deviceState`](../../src/services/audio/Inno.Audio/IAudioService.cs#L20) | Gets the current audio output state. |
| [`Inno.Audio.AudioStatistics Inno.Audio.IAudioService.statistics`](../../src/services/audio/Inno.Audio/IAudioService.cs#L30) | Gets current runtime and backend resource statistics. |
| [`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L41) | Starts one clip using default playback parameters. |
| [`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L58) | Starts one clip using explicit playback parameters. |
| [`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.PlayScheduled(Inno.Audio.AudioClipAsset clip, double scheduledDspTime, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L78) | Schedules one clip against the monotonic audio clock. |
| [`Inno.Audio.IAudioService`](../../src/services/audio/Inno.Audio/IAudioService.cs#L10) | Provides backend-neutral audio playback, caching, mixer, and device-state services. |
| [`System.Threading.Tasks.ValueTask Inno.Audio.IAudioService.PreloadAsync(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/audio/Inno.Audio/IAudioService.cs#L234) | Prepares a clip and retains it in the decoded or streamed cache. |
| [`bool Inno.Audio.IAudioService.Pause(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L104) | Pauses a live voice. |
| [`bool Inno.Audio.IAudioService.Resume(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L115) | Resumes a paused voice. |
| [`bool Inno.Audio.IAudioService.Seek(Inno.Audio.AudioVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L129) | Moves a live voice cursor to a clip-relative position. |
| [`bool Inno.Audio.IAudioService.SetBusMuted(Inno.Audio.AudioBusId bus, bool muted)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L197) | Updates mute state for a semantic mixer bus. |
| [`bool Inno.Audio.IAudioService.SetBusPaused(Inno.Audio.AudioBusId bus, bool paused)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L214) | Updates pause state for a semantic mixer bus. |
| [`bool Inno.Audio.IAudioService.SetBusVolume(Inno.Audio.AudioBusId bus, float volume)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L180) | Updates linear gain for a semantic mixer bus. |
| [`bool Inno.Audio.IAudioService.SetVoiceParameters(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L146) | Replaces mutable parameters for a live voice. |
| [`bool Inno.Audio.IAudioService.Stop(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L93) | Stops a live voice. |
| [`bool Inno.Audio.IAudioService.TryGetVoiceState(Inno.Audio.AudioVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L163) | Queries the current state of a voice. |
| [`double Inno.Audio.IAudioService.dspTime`](../../src/services/audio/Inno.Audio/IAudioService.cs#L25) | Gets the monotonic audio clock in seconds. |
| [`void Inno.Audio.IAudioService.ReleasePreload(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L246) | Releases one explicit preload retention without interrupting active voices. |

### `Inno.Audio.SerializedAudioExtensionState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.SerializedAudioExtensionState`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L286) | Stores reload-safe configuration for one mixer or feature extension generation. |
| [`Inno.Audio.SerializedAudioExtensionState.SerializedAudioExtensionState()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L291) | Creates empty extension state for deserialization. |
| [`Inno.Audio.SerializedAudioExtensionState.SerializedAudioExtensionState(System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L305) | Creates neutral extension state. |
| [`System.Guid Inno.Audio.SerializedAudioExtensionState.stableTypeId`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L316) | Gets or sets the stable settings type identity. |
| [`byte[] Inno.Audio.SerializedAudioExtensionState.propertyData`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L322) | Gets or sets neutral serialized property bytes. |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
