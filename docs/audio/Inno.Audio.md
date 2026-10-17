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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Audio.Audio`

<!-- api: T:Inno.Audio.Audio 36740e557e2870204313a1aca872b5689a0583ce473c6cb0f42a1ed2f0544d39 -->
<!-- api: P:Inno.Audio.Audio.capabilities c1f19adb08159ca24e92e870ab7b40d6c1259e7c6eac432c8a0c6620934e9751 -->
<!-- api: P:Inno.Audio.Audio.deviceState 0339e8547f0fbc8838c2faa11141aa9f0e73547f9ad5ade329404ca3c541433a -->
<!-- api: P:Inno.Audio.Audio.dspTime bf3624cde29eb04f9e4df42060972e5b9b9c7f5f489c17b98004ce2b4f586596 -->
<!-- api: P:Inno.Audio.Audio.statistics 2a2ea4cf7dd225adc0fe081bcf8885a2fa6961093e46c0de9d89cb56c228a00e -->
<!-- api: M:Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset) 17096736c9cbdea66658c1e9b7be532851d1cf0d7680d747b2fd137d6fadb106 -->
<!-- api: M:Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset,Inno.Audio.AudioPlayOptions) b30a1d32dcaad376bded303eb46ac9ecffad04d4b4a738e840ac137072c578af -->
<!-- api: M:Inno.Audio.Audio.PlayScheduled(Inno.Audio.AudioClipAsset,System.Double,Inno.Audio.AudioPlayOptions) 50b2883fc0a364cb86b91adf083fef1d45fb743c49e3e0c335a94ed4a6f5950e -->
<!-- api: M:Inno.Audio.Audio.Stop(Inno.Audio.AudioVoiceHandle) bba6be5b585b336a62cc98dc19eb3a7a79ead7d2b31d03418f126194e84928da -->
<!-- api: M:Inno.Audio.Audio.Pause(Inno.Audio.AudioVoiceHandle) 7a376f5ef69cded8c426c8be15532ac9ef390ff3079dd59bce1b540c47473669 -->
<!-- api: M:Inno.Audio.Audio.Resume(Inno.Audio.AudioVoiceHandle) b0972ea38c7ede3f4f61fb164226065849af8ee8af8f1faad66a076b73b661a0 -->
<!-- api: M:Inno.Audio.Audio.Seek(Inno.Audio.AudioVoiceHandle,System.TimeSpan) cb51abe67ebd31cf65207b15899fb1e704923420a2413f957137663196857026 -->
<!-- api: M:Inno.Audio.Audio.SetVoiceParameters(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioVoiceParameters) f9494607520fbf0be70a686e0daf0aac6dbaef8a0c330e0718a84c5403049eea -->
<!-- api: M:Inno.Audio.Audio.TryGetVoiceState(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioPlaybackState@) 0cc79ef809c38846fee8fd082b4202fc25248497282176bdccd25fe62aa94f8a -->
<!-- api: M:Inno.Audio.Audio.SetBusVolume(Inno.Audio.AudioBusId,System.Single) 09b7affaf51081851f54c74c314077e0e8244f1dd54184401870d5406cbcb19e -->
<!-- api: M:Inno.Audio.Audio.SetBusMuted(Inno.Audio.AudioBusId,System.Boolean) ba5253ce5b28591750215760c596a7bb9bd390f6c0b52e36362178e3cc0ae814 -->
<!-- api: M:Inno.Audio.Audio.SetBusPaused(Inno.Audio.AudioBusId,System.Boolean) c09a7bf75d74098f0b5626c5b350de0ad0a53bb91f40d460415433751cb174ca -->
<!-- api: M:Inno.Audio.Audio.PreloadAsync(Inno.Audio.AudioClipAsset,Inno.Audio.AudioClipLoadMode,System.Threading.CancellationToken) c886126816657cf6d9195dca7353e86c79e9bdfbd34eeac6b9f86454e608255f -->
<!-- api: M:Inno.Audio.Audio.ReleasePreload(Inno.Audio.AudioClipAsset) 74d052829351f42f1426c25dcf72ed57385018a979203ce9e4e2fa0b5d91740b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a1953e05a726da46"></a>[`public static bool Inno.Audio.Audio.Pause(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L138) | Pauses a live voice.<br>返回：true when the voice entered the paused state.<br>`voice`：Voice handle to pause. | — | — |
| <a id="api-be7ea9dc799afc17"></a>[`public static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/Audio.cs#L91) | Starts one clip using explicit playback parameters.<br>返回：A voice handle that may initially be preparing.<br>`clip`：Imported clip to prepare and play.<br>`options`：Immutable playback parameters. | — | — |
| <a id="api-c30e305dbfb82a5d"></a>[`public static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.Play(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/Audio.cs#L77) | Starts one clip using default playback parameters.<br>返回：A voice handle that may initially be preparing.<br>`clip`：Imported clip to prepare and play. | — | — |
| <a id="api-cf91ced0cadcf072"></a>[`public static Inno.Audio.AudioVoiceHandle Inno.Audio.Audio.PlayScheduled(Inno.Audio.AudioClipAsset clip, double scheduledDspTime, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/Audio.cs#L111) | Schedules one clip against the monotonic audio clock.<br>返回：A voice handle in preparing or scheduled state.<br>`clip`：Imported clip to prepare and play.<br>`scheduledDspTime`：Absolute audio-clock start time in seconds.<br>`options`：Immutable playback parameters. | — | — |
| <a id="api-0ae0e4245e8944c2"></a>[`public static System.Threading.Tasks.ValueTask Inno.Audio.Audio.PreloadAsync(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/audio/Inno.Audio/Audio.cs#L270) | Prepares a clip and retains it in the runtime cache.<br>返回：An asynchronous operation that completes after the cache entry is ready.<br>`clip`：Imported clip to prepare.<br>`loadMode`：Storage strategy override.<br>`cancellationToken`：Cancellation token observed before the cache commit. | — | — |
| <a id="api-7ed816073ecb34f0"></a>[`public static void Inno.Audio.Audio.ReleasePreload(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/Audio.cs#L283) | Releases one explicit preload retention without interrupting active voices.<br>`clip`：Imported clip whose preload retention should be released. | — | — |
| <a id="api-eac1cd12f01aeaee"></a>[`public static bool Inno.Audio.Audio.Resume(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L149) | Resumes a paused voice.<br>返回：true when the voice resumed.<br>`voice`：Voice handle to resume. | — | — |
| <a id="api-9c184cf205745db9"></a>[`public static bool Inno.Audio.Audio.Seek(Inno.Audio.AudioVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/Audio.cs#L163) | Moves a live voice cursor to a clip-relative position.<br>返回：true when the cursor was updated.<br>`voice`：Voice handle to seek.<br>`position`：Non-negative clip-relative position. | — | — |
| <a id="api-f2fc286045b8c5c5"></a>[`public static bool Inno.Audio.Audio.SetBusMuted(Inno.Audio.AudioBusId bus, bool muted)`](../../src/services/audio/Inno.Audio/Audio.cs#L233) | Updates mute state for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`muted`：Whether output from the bus is silenced. | — | — |
| <a id="api-6ead29d906369138"></a>[`public static bool Inno.Audio.Audio.SetBusPaused(Inno.Audio.AudioBusId bus, bool paused)`](../../src/services/audio/Inno.Audio/Audio.cs#L250) | Updates pause state for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`paused`：Whether routed voices should pause. | — | — |
| <a id="api-07e2f2c382250ef2"></a>[`public static bool Inno.Audio.Audio.SetBusVolume(Inno.Audio.AudioBusId bus, float volume)`](../../src/services/audio/Inno.Audio/Audio.cs#L216) | Updates linear gain for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`volume`：Non-negative linear gain. | — | — |
| <a id="api-259c049a7e81bbeb"></a>[`public static bool Inno.Audio.Audio.SetVoiceParameters(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/Audio.cs#L180) | Replaces mutable parameters for a live voice.<br>返回：true when the voice was updated.<br>`voice`：Voice handle to update.<br>`parameters`：Current voice parameter snapshot. | — | — |
| <a id="api-d3bde52a72e872b0"></a>[`public static bool Inno.Audio.Audio.Stop(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/Audio.cs#L127) | Stops a live voice.<br>返回：true when a live voice was stopped.<br>`voice`：Voice handle to stop. | — | — |
| <a id="api-5bc4e94efef646d8"></a>[`public static bool Inno.Audio.Audio.TryGetVoiceState(Inno.Audio.AudioVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/Audio.cs#L198) | Queries the current state of a voice.<br>返回：true when the handle belongs to a known voice.<br>`voice`：Voice handle to query.<br>`playbackState`：Receives the current state when the handle is known. | — | — |
| <a id="api-b8118eacb3112df4"></a>[`public static Inno.Audio.AudioCapabilities Inno.Audio.Audio.capabilities { get; }`](../../src/services/audio/Inno.Audio/Audio.cs#L51) | Gets immutable capabilities for the current audio device generation. | — | — |
| <a id="api-1124133414b050b5"></a>[`public static Inno.Audio.AudioDeviceState Inno.Audio.Audio.deviceState { get; }`](../../src/services/audio/Inno.Audio/Audio.cs#L56) | Gets the current output availability state. | — | — |
| <a id="api-acb54bdedf7f4376"></a>[`public static double Inno.Audio.Audio.dspTime { get; }`](../../src/services/audio/Inno.Audio/Audio.cs#L61) | Gets the monotonic audio clock in seconds. | — | — |
| <a id="api-32d39eab6df57929"></a>[`public static Inno.Audio.AudioStatistics Inno.Audio.Audio.statistics { get; }`](../../src/services/audio/Inno.Audio/Audio.cs#L66) | Gets current audio resource statistics. | — | — |
| <a id="api-227246b73d70109e"></a>[`public static class Inno.Audio.Audio`](../../src/services/audio/Inno.Audio/Audio.cs#L46) | Provides script-friendly access to the audio service in the current execution context. | — | — |

### `Inno.Audio.AudioBusDefinition`

<!-- api: T:Inno.Audio.AudioBusDefinition c708c6c6d6576a21fb6971fd141163bec3bf10450f00e9f2882dd671dac818b5 -->
<!-- api: M:Inno.Audio.AudioBusDefinition.#ctor(Inno.Audio.AudioBusId,System.Nullable{Inno.Audio.AudioBusId},System.Single,System.Boolean,System.Collections.Generic.IEnumerable{Inno.Audio.AudioProcessorConfiguration}) fe03e83dc59c25fa9f1a1301668dad5554a257e2fcc8fbb21202734c356d6139 -->
<!-- api: P:Inno.Audio.AudioBusDefinition.id c59bc55bb13732d857ff9c0bf9ec14765f1507d6ae0496bd78a93b399e7f73f5 -->
<!-- api: P:Inno.Audio.AudioBusDefinition.parent d4ffcfb6e36312dfbfd9063729b1b02583c85b26581312e2e11f36080e08d264 -->
<!-- api: P:Inno.Audio.AudioBusDefinition.volume 213f0a71c33b63c406d3578c4cbbf905e03d4a7945cfad49b351d083143d2b29 -->
<!-- api: P:Inno.Audio.AudioBusDefinition.muted 4bf9df4b243495a75f11b7cb791e7da2866611f72368d8f2c236f3065e54e024 -->
<!-- api: P:Inno.Audio.AudioBusDefinition.processors 0d163b0dbfd349cfd97edcdbc8a1a786edcdd45608f4e91452f69ec1154ac3fa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-273847d1ecdb5eb9"></a>[`public Inno.Audio.AudioBusDefinition.AudioBusDefinition(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusId? parent, float volume, bool muted, System.Collections.Generic.IEnumerable<Inno.Audio.AudioProcessorConfiguration>? processors = null)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L109) | Creates an immutable mixer bus definition.<br>`id`：Stable bus identifier.<br>`parent`：Parent bus identifier, or null only for the master bus.<br>`volume`：Non-negative linear bus gain.<br>`muted`：Whether output from the bus is initially silenced.<br>`processors`：Ordered processor chain. | — | — |
| <a id="api-d5ac6056c68cd98f"></a>[`public Inno.Audio.AudioBusId Inno.Audio.AudioBusDefinition.id { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L134) | Gets the stable bus identifier. | — | — |
| <a id="api-fbffa3b1dd5b7b73"></a>[`public bool Inno.Audio.AudioBusDefinition.muted { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L149) | Gets whether output from the bus is initially silenced. | — | — |
| <a id="api-7c9ffd5c05538655"></a>[`public Inno.Audio.AudioBusId? Inno.Audio.AudioBusDefinition.parent { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L139) | Gets the parent bus, or null for the master bus. | — | — |
| <a id="api-903e43d503eaf5a5"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioProcessorConfiguration> Inno.Audio.AudioBusDefinition.processors { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L154) | Gets the ordered immutable processor chain. | — | — |
| <a id="api-0fb2aa59fc9ce9d2"></a>[`public float Inno.Audio.AudioBusDefinition.volume { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L144) | Gets the initial linear bus gain. | — | — |
| <a id="api-3d9f904122f8fadf"></a>[`public sealed class Inno.Audio.AudioBusDefinition`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L87) | Describes one bus and its ordered processor chain in a compiled mixer graph. | — | — |

### `Inno.Audio.AudioBusHandle`

<!-- api: T:Inno.Audio.AudioBusHandle 9cd1e82ebe6b3f482a24a6a1521ac258f619188521320a2da4c5524a43cefb2c -->
<!-- api: P:Inno.Audio.AudioBusHandle.deviceGeneration f19edef89a765d343e87f05d65672073f85b60e36ef7224772550b737540e5ec -->
<!-- api: P:Inno.Audio.AudioBusHandle.isValid beb3f376e7674c1860f6d71446d911b54a4e004637cc24665b951eaf9c2e3db6 -->
<!-- api: M:Inno.Audio.AudioBusHandle.ToString 6994b16f92bd817a4ebe88bd936c62f158062bad73ff6d6eb11bf43c867c98e5 -->
<!-- api: M:Inno.Audio.AudioBusHandle.op_Inequality(Inno.Audio.AudioBusHandle,Inno.Audio.AudioBusHandle) c5a99ce3548791b2dbbae311d6dbc2f17cb50255877be933451528009446c492 -->
<!-- api: M:Inno.Audio.AudioBusHandle.op_Equality(Inno.Audio.AudioBusHandle,Inno.Audio.AudioBusHandle) 75cf7bbef243b980befd77c9f438f2f4c9317012933cb2a4aa553e68e67fad46 -->
<!-- api: M:Inno.Audio.AudioBusHandle.GetHashCode 210090536822fb2b6d8579ac858c20ad0ff044d570562a8579dd2c95e82346ff -->
<!-- api: M:Inno.Audio.AudioBusHandle.Equals(System.Object) 7cd8e973123b4faa7434443e4d128f68931a04fc951adc4df8202840dcd4d46b -->
<!-- api: M:Inno.Audio.AudioBusHandle.Equals(Inno.Audio.AudioBusHandle) c367a695519ebc95cf4245fbefe1830192ae348e3599c4d1d22c9774ec8fd962 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cf08bda70909f4e3"></a>[`public bool Inno.Audio.AudioBusHandle.Equals(Inno.Audio.AudioBusHandle other)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-3f425a717b8e41e1"></a>[`public override bool Inno.Audio.AudioBusHandle.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5e62b56516b4e568"></a>[`public override int Inno.Audio.AudioBusHandle.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-1adb916fd22d3289"></a>[`public static bool Inno.Audio.AudioBusHandle.operator ==(Inno.Audio.AudioBusHandle left, Inno.Audio.AudioBusHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ca1663ddea613518"></a>[`public static bool Inno.Audio.AudioBusHandle.operator !=(Inno.Audio.AudioBusHandle left, Inno.Audio.AudioBusHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-10153868147ff744"></a>[`public override string Inno.Audio.AudioBusHandle.ToString()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-f53ed6623f954a4b"></a>[`public uint Inno.Audio.AudioBusHandle.deviceGeneration { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L73) | Gets the generation of the device that created this handle. | — | — |
| <a id="api-887b7652e1e17249"></a>[`public bool Inno.Audio.AudioBusHandle.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L78) | Gets whether the handle contains a non-zero backend identity and generation. | — | — |
| <a id="api-54cad4dcf6d2c4d2"></a>[`public readonly record struct Inno.Audio.AudioBusHandle : System.IEquatable<Inno.Audio.AudioBusHandle>`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L58) | Identifies one backend mixer bus within a device generation. | — | — |

### `Inno.Audio.AudioBusId`

<!-- api: T:Inno.Audio.AudioBusId c2fb41547326f510ed79d715397e1aa0ca02b2d8505887401b6d0afda04793f3 -->
<!-- api: M:Inno.Audio.AudioBusId.#ctor(System.String) 604b66af02f7314fd52f109212663d29165dc289dae40cc96098c8a3750ec0ce -->
<!-- api: P:Inno.Audio.AudioBusId.master 4c0eebcf656e7aa27602e61f68162a427d82c8b5ba458a2072c23cf8f43a6e52 -->
<!-- api: P:Inno.Audio.AudioBusId.value 01a10b9c899491670ca8bd0fce6fd1b23c1e4185e3e7669051286c4b8c842d36 -->
<!-- api: P:Inno.Audio.AudioBusId.isValid 82911fc72926e57b38b5ad34e99d516644bcc320db09b83cf415b9689329f8bc -->
<!-- api: M:Inno.Audio.AudioBusId.ToString 7f585809f580f64393dfeb6c16b213dfacab2eb6f460ccd9960c69ba809c97f4 -->
<!-- api: M:Inno.Audio.AudioBusId.op_Inequality(Inno.Audio.AudioBusId,Inno.Audio.AudioBusId) a578e58b9370a9c2d3889c620fa48e696b696cab486685fe40348ed17bb2dfdf -->
<!-- api: M:Inno.Audio.AudioBusId.op_Equality(Inno.Audio.AudioBusId,Inno.Audio.AudioBusId) 7f5717b0ed76e99d90ca55385d365d5432923f7c162f3e0afb56ac9b5422b8be -->
<!-- api: M:Inno.Audio.AudioBusId.GetHashCode ef6cba69212975b367c3e61dd9a6f700fe6966dae65fa7af16f9509d9cb9926b -->
<!-- api: M:Inno.Audio.AudioBusId.Equals(System.Object) 1ff98a81ea9f44d6c7220817aabcb8a78a3490feded8d583bf11dbcaa8c91091 -->
<!-- api: M:Inno.Audio.AudioBusId.Equals(Inno.Audio.AudioBusId) 328f65ae11caa3b35ac8a0add47ef8c307db2aa08ded1384422fce7c42c23159 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-53a1f5457fd51ed5"></a>[`public Inno.Audio.AudioBusId.AudioBusId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L16) | Creates an open mixer bus identifier.<br>`value`：Globally stable bus protocol value. | — | — |
| <a id="api-c6e9f21fe28fd816"></a>[`public bool Inno.Audio.AudioBusId.Equals(Inno.Audio.AudioBusId other)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-88459ddc9e842617"></a>[`public override bool Inno.Audio.AudioBusId.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-bc750493ce94c0e8"></a>[`public override int Inno.Audio.AudioBusId.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-756257ffffa0bd68"></a>[`public static bool Inno.Audio.AudioBusId.operator ==(Inno.Audio.AudioBusId left, Inno.Audio.AudioBusId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e5a1308b2643e5b7"></a>[`public static bool Inno.Audio.AudioBusId.operator !=(Inno.Audio.AudioBusId left, Inno.Audio.AudioBusId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-210101edac30b7f9"></a>[`public override string Inno.Audio.AudioBusId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L43) | Formats the identifier for diagnostics and persistence.<br>返回：The stable protocol value. | — | — |
| <a id="api-6dea5bd9bccb0140"></a>[`public bool Inno.Audio.AudioBusId.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L35) | Gets whether this identifier contains a usable protocol value. | — | — |
| <a id="api-bc65ba8699778a6d"></a>[`public static Inno.Audio.AudioBusId Inno.Audio.AudioBusId.master { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L25) | Gets the mandatory root bus identifier. | — | — |
| <a id="api-a075da3f6d918526"></a>[`public string Inno.Audio.AudioBusId.value { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L30) | Gets the globally stable protocol value. | — | — |
| <a id="api-def161644a357183"></a>[`public readonly record struct Inno.Audio.AudioBusId : System.IEquatable<Inno.Audio.AudioBusId>`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L8) | Identifies a mixer bus without imposing a closed bus catalog. | — | — |

### `Inno.Audio.AudioCapabilities`

<!-- api: T:Inno.Audio.AudioCapabilities c1bc0476669fe5f1969d7279355d6302e43071accd746e8950d92fed27f4d5be -->
<!-- api: M:Inno.Audio.AudioCapabilities.#ctor(System.Boolean,System.Boolean,System.Boolean,System.Int32,System.Int32) 7d699ae5a5fbb419204b86a0958e3983392fa3299cbe94bb8b4af3f3c3344541 -->
<!-- api: P:Inno.Audio.AudioCapabilities.supportsStreaming b460dbd8e429cabdd227b968097be0c1e2a9c1b3b6eba33bdcb5da6e19b9dd50 -->
<!-- api: P:Inno.Audio.AudioCapabilities.supportsScheduledPlayback 903068cc2504758564ced0d85d2cbd53a3ad175dd7028f9dda842595bcbe863f -->
<!-- api: P:Inno.Audio.AudioCapabilities.supportsSpatialAudio 71068a8d2b90d38513eac4db6e29bb1b33912c89d80e397989f6c48fedb5a2f7 -->
<!-- api: P:Inno.Audio.AudioCapabilities.maxListeners a276547ee7e2509b04336b5c5fbd580a31d90d6efe32c22d0add3138efa9fd85 -->
<!-- api: P:Inno.Audio.AudioCapabilities.sampleRate 78855b506ed11baef51cde56e000a77b24eeea74d86403a069fd573eb1e72f56 -->
<!-- api: M:Inno.Audio.AudioCapabilities.ToString 8f072c9c839f9a640a23d8044266e0d7eac09cdb1d6610448ca684ca93bdbcc3 -->
<!-- api: M:Inno.Audio.AudioCapabilities.op_Inequality(Inno.Audio.AudioCapabilities,Inno.Audio.AudioCapabilities) a7cca98afe1fb9284320a5c6254db4d97dcfc0fca6ad903b8eaae032d2635144 -->
<!-- api: M:Inno.Audio.AudioCapabilities.op_Equality(Inno.Audio.AudioCapabilities,Inno.Audio.AudioCapabilities) 3f4ae874fa24f8f1e616f2e12d7c3f4bd0b4605fba680fa2530bf79779a93d5f -->
<!-- api: M:Inno.Audio.AudioCapabilities.GetHashCode e53fb88e55233ff009f99e930ce4397b41da6124975c6e85cf062f217e21ead8 -->
<!-- api: M:Inno.Audio.AudioCapabilities.Equals(System.Object) eee8fadca9a0256a23f6d3ec259903329a455b3ea8352bdbe5166b9cf50841e0 -->
<!-- api: M:Inno.Audio.AudioCapabilities.Equals(Inno.Audio.AudioCapabilities) 2360a26bc663294cca5ceed600a6fc6d4ba34761cf722b7bd98beb831c325a63 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-027e52720f29e16a"></a>[`public Inno.Audio.AudioCapabilities.AudioCapabilities(bool supportsStreaming, bool supportsScheduledPlayback, bool supportsSpatialAudio, int maxListeners, int sampleRate)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L174) | Creates an immutable audio capability snapshot.<br>`supportsStreaming`：Whether clips can be decoded incrementally.<br>`supportsScheduledPlayback`：Whether voices can start against the audio clock.<br>`supportsSpatialAudio`：Whether listener-relative spatial playback is available.<br>`maxListeners`：Maximum simultaneously active listeners.<br>`sampleRate`：Device output sample rate in frames per second. | — | — |
| <a id="api-119ab8fb9e6df3d8"></a>[`public bool Inno.Audio.AudioCapabilities.Equals(Inno.Audio.AudioCapabilities? other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-6c865316b6d8d084"></a>[`public override bool Inno.Audio.AudioCapabilities.Equals(object? obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-484673beed8f8f93"></a>[`public override int Inno.Audio.AudioCapabilities.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-f16ea01765cd7b66"></a>[`public static bool Inno.Audio.AudioCapabilities.operator ==(Inno.Audio.AudioCapabilities? left, Inno.Audio.AudioCapabilities? right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-20104eb1a92f3c80"></a>[`public static bool Inno.Audio.AudioCapabilities.operator !=(Inno.Audio.AudioCapabilities? left, Inno.Audio.AudioCapabilities? right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-1c53d9b3b6215469"></a>[`public override string Inno.Audio.AudioCapabilities.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-7d5f6f224bb9d604"></a>[`public int Inno.Audio.AudioCapabilities.maxListeners { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L208) | Gets the maximum simultaneously active listener count. | — | — |
| <a id="api-c20783589f6bb507"></a>[`public int Inno.Audio.AudioCapabilities.sampleRate { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L213) | Gets the output sample rate in frames per second. | — | — |
| <a id="api-452cd1ec9f2bfdbe"></a>[`public bool Inno.Audio.AudioCapabilities.supportsScheduledPlayback { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L198) | Gets whether voices can start against the audio clock. | — | — |
| <a id="api-8ef30c38f0e29737"></a>[`public bool Inno.Audio.AudioCapabilities.supportsSpatialAudio { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L203) | Gets whether listener-relative spatial playback is available. | — | — |
| <a id="api-7865a114f7a326c2"></a>[`public bool Inno.Audio.AudioCapabilities.supportsStreaming { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L193) | Gets whether clips can be decoded incrementally. | — | — |
| <a id="api-cd4ea04a8fa9a65f"></a>[`public sealed record Inno.Audio.AudioCapabilities : System.IEquatable<Inno.Audio.AudioCapabilities>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L154) | Describes backend-neutral features and limits for one device generation. | — | — |

### `Inno.Audio.AudioClipAsset`

<!-- api: T:Inno.Audio.AudioClipAsset 48e7a777c3f426b3e1f13140f3849e2e1964e81db0762cbf1c362aa99afaacff -->
<!-- api: P:Inno.Audio.AudioClipAsset.metadata 89b612c9023044f29ed591d7269869ecc415ae0cc0ec1193622b161df3968eb1 -->
<!-- api: P:Inno.Audio.AudioClipAsset.duration dafef6869740e645cf2c9c2a5639d5e2627cfa23e41de4f05a6a94a4448e3f2e -->
<!-- api: M:Inno.Audio.AudioClipAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory{System.Byte},System.ReadOnlyMemory{System.Byte}) b960a6b2fb8b77ed8774bba8c209043a39ddb45eb570d4ed382c532ec4abc0ec -->
<!-- api: M:Inno.Audio.AudioClipAsset.#ctor b7865e6f724f11cb00fc47264299ddcc70ca3599c5ce3a658c2c70cd827c8339 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-335798540af38214"></a>[`public Inno.Audio.AudioClipAsset.AudioClipAsset()`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L155) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-b9848174877636a9"></a>[`protected override void Inno.Audio.AudioClipAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L178) | Refreshes imported metadata after an artifact commit.<br>`previousPayload`：Previously committed compact header.<br>`currentPayload`：Newly committed compact header. | — | — |
| <a id="api-c225746ef90bc5b4"></a>[`public System.TimeSpan Inno.Audio.AudioClipAsset.duration { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L167) | Gets the imported clip duration, or zero before runtime content is loaded. | — | — |
| <a id="api-c2f82965bfc40888"></a>[`public Inno.Audio.AudioClipMetadata? Inno.Audio.AudioClipAsset.metadata { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L162) | Gets imported metadata, or null before runtime content is loaded. | — | — |
| <a id="api-56bf18d4fb2991fb"></a>[`public sealed class Inno.Audio.AudioClipAsset : Inno.Assets.AssetObject`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L155) | Represents one imported audio source and its compact runtime metadata. | — | — |

### `Inno.Audio.AudioClipDescriptor`

<!-- api: T:Inno.Audio.AudioClipDescriptor 8438aa638e71862a738f26fe04f85a653f0a8c39e2466ca069ce5b2752d29bdd -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.#ctor(Inno.Audio.AudioCodecId,Inno.Audio.AudioClipLoadMode,System.Int32,System.Int32,System.Int64,System.Int64) 746614f68c3fa338c331ddb4cf556c08faae7b6470253fc388099d3e9661cf1b -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.codec d1aa3d55e717f8eab45a142dedd9d320e903588672286ec2dd6cdbf6bdf76521 -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.loadMode 55679a5d7dcd7d1f3f5952bf0410b54835979d5d6519012ad8e5a468b78caaee -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.channels 0ef3934de41ce71afc4fe3361f68a1731924bcab8e0d247890bffdd62542669f -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.sampleRate 3a7597cb901a42355c77eaa09ad9174164da6877cc7d493f0c36eda585d2f315 -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.frameCount c08e849f51092e0d118e4bda8996caa6b4c330c0224ac704227ef6c02ef15211 -->
<!-- api: P:Inno.Audio.AudioClipDescriptor.encodedByteLength 72a284cb80e4149812a8a18f3eb43fd30f48bde865e2eb31fa1b12690be7be22 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.ToString b48f2323571c4cab280da091907d4cad66eeeaf2635adbc9ad1c2226ba00e2f3 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.op_Inequality(Inno.Audio.AudioClipDescriptor,Inno.Audio.AudioClipDescriptor) a77dc513dc28ddd2f2e8280ff766f535e82bed38f923bb01e9adedcc96e16487 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.op_Equality(Inno.Audio.AudioClipDescriptor,Inno.Audio.AudioClipDescriptor) 86f8e6874593d4c5551da4c2a55c0c1e2af2ff9f1cc627fce1e83e292298cd94 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.GetHashCode 93d7a548f1c467e8bb019ea9c402804876fc05e40d6175bae6ffdd32dacef531 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.Equals(System.Object) 75f2ec9b29268ac83bfc95961da126504baa97ef449257abae5ef0328f3044c5 -->
<!-- api: M:Inno.Audio.AudioClipDescriptor.Equals(Inno.Audio.AudioClipDescriptor) f7b4e1e2d4dce86545a8b2501da7390198a7e85686310d5f76e21cfae053de56 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a23158ba2b0fd167"></a>[`public Inno.Audio.AudioClipDescriptor.AudioClipDescriptor(Inno.Audio.AudioCodecId codec, Inno.Audio.AudioClipLoadMode loadMode, int channels, int sampleRate, long frameCount, long encodedByteLength)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L34) | Creates a backend clip description.<br>`codec`：Codec protocol used by the artifact.<br>`loadMode`：Required decoded or streamed storage strategy.<br>`channels`：Encoded channel count.<br>`sampleRate`：Encoded sample rate in frames per second.<br>`frameCount`：Total decoded frame count when known.<br>`encodedByteLength`：Encoded artifact length in bytes. | — | `System.ArgumentOutOfRangeException`：The load mode is undefined or a channel, rate, frame, or byte count is outside its valid range. |
| <a id="api-8a080d16ee428196"></a>[`public bool Inno.Audio.AudioClipDescriptor.Equals(Inno.Audio.AudioClipDescriptor other)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-bad73566d044fa9a"></a>[`public override bool Inno.Audio.AudioClipDescriptor.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-114b09c8c72203d7"></a>[`public override int Inno.Audio.AudioClipDescriptor.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-e9ddf677e76d7ebf"></a>[`public static bool Inno.Audio.AudioClipDescriptor.operator ==(Inno.Audio.AudioClipDescriptor left, Inno.Audio.AudioClipDescriptor right)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5316c864a0a28b6c"></a>[`public static bool Inno.Audio.AudioClipDescriptor.operator !=(Inno.Audio.AudioClipDescriptor left, Inno.Audio.AudioClipDescriptor right)`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-903b0d64588fea75"></a>[`public override string Inno.Audio.AudioClipDescriptor.ToString()`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-bd0872e4768718c4"></a>[`public int Inno.Audio.AudioClipDescriptor.channels { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L71) | Gets the encoded channel count. | — | — |
| <a id="api-01a4e28a556af036"></a>[`public Inno.Audio.AudioCodecId Inno.Audio.AudioClipDescriptor.codec { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L61) | Gets the encoded codec protocol. | — | — |
| <a id="api-2afb09da321e1bc5"></a>[`public long Inno.Audio.AudioClipDescriptor.encodedByteLength { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L86) | Gets the encoded artifact length in bytes. | — | — |
| <a id="api-f27c4448362103e9"></a>[`public long Inno.Audio.AudioClipDescriptor.frameCount { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L81) | Gets the total decoded frame count, or zero when unknown. | — | — |
| <a id="api-c4b2b179a8bb5d96"></a>[`public Inno.Audio.AudioClipLoadMode Inno.Audio.AudioClipDescriptor.loadMode { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L66) | Gets the required storage strategy. | — | — |
| <a id="api-f41347a9cbddec5d"></a>[`public int Inno.Audio.AudioClipDescriptor.sampleRate { get; }`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L76) | Gets the encoded sample rate in frames per second. | — | — |
| <a id="api-5715b5a625483ca3"></a>[`public readonly record struct Inno.Audio.AudioClipDescriptor : System.IEquatable<Inno.Audio.AudioClipDescriptor>`](../../src/services/audio/Inno.Audio/AudioClipDescriptor.cs#L8) | Describes one immutable encoded audio artifact presented to a backend device. | — | — |

### `Inno.Audio.AudioClipHandle`

<!-- api: T:Inno.Audio.AudioClipHandle 1ae7cd1bcac5f5200de214d595acc221f49d5b9961a6823ec98ad10fca74955c -->
<!-- api: P:Inno.Audio.AudioClipHandle.deviceGeneration b64dac163b4a60c129f1701625ed154f726af6694709399f9a63394510351490 -->
<!-- api: P:Inno.Audio.AudioClipHandle.isValid a3d8e87ea140c2fd99d98b94a690a8816a03f084be58fc20e9c78519eb393915 -->
<!-- api: M:Inno.Audio.AudioClipHandle.ToString 64b07121ab8275000227e400cfee099724caa891929874f2cee5a7c182a046af -->
<!-- api: M:Inno.Audio.AudioClipHandle.op_Inequality(Inno.Audio.AudioClipHandle,Inno.Audio.AudioClipHandle) 53029d576b3b3078154578e3665eb124b7369ae29c6cfceab43e04bde94f6161 -->
<!-- api: M:Inno.Audio.AudioClipHandle.op_Equality(Inno.Audio.AudioClipHandle,Inno.Audio.AudioClipHandle) cfb92dc373711c0992331f8644131fb52885e8086a5fd660a14b244ff63e9b17 -->
<!-- api: M:Inno.Audio.AudioClipHandle.GetHashCode 78b536b0e64902551a462a227e66de7a2210318d6a60aaceb32dc61a75b5dee2 -->
<!-- api: M:Inno.Audio.AudioClipHandle.Equals(System.Object) e27a7668c62d6fa78a66066de8f18034164131194765bd28a0e795c21187adae -->
<!-- api: M:Inno.Audio.AudioClipHandle.Equals(Inno.Audio.AudioClipHandle) b23cae5c78a5b1dc2a0b89793ff38675e3700b13aa4209c2773a874e4f07d5a9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9aa0ae8dc3045a24"></a>[`public bool Inno.Audio.AudioClipHandle.Equals(Inno.Audio.AudioClipHandle other)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-93a434058ab9b2be"></a>[`public override bool Inno.Audio.AudioClipHandle.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5d688e497ac507b6"></a>[`public override int Inno.Audio.AudioClipHandle.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8fbad5090f7cbfe1"></a>[`public static bool Inno.Audio.AudioClipHandle.operator ==(Inno.Audio.AudioClipHandle left, Inno.Audio.AudioClipHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-352894f52b380a88"></a>[`public static bool Inno.Audio.AudioClipHandle.operator !=(Inno.Audio.AudioClipHandle left, Inno.Audio.AudioClipHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-14fdf1eff032360c"></a>[`public override string Inno.Audio.AudioClipHandle.ToString()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6f6fb7cce1e9e7a5"></a>[`public uint Inno.Audio.AudioClipHandle.deviceGeneration { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L21) | Gets the generation of the device that created this handle. | — | — |
| <a id="api-db15ad90c58fd613"></a>[`public bool Inno.Audio.AudioClipHandle.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L26) | Gets whether the handle contains a non-zero backend identity and generation. | — | — |
| <a id="api-61b7c4f552127ce4"></a>[`public readonly record struct Inno.Audio.AudioClipHandle : System.IEquatable<Inno.Audio.AudioClipHandle>`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L6) | Identifies one backend clip allocation within a device generation. | — | — |

### `Inno.Audio.AudioClipLoadMode`

<!-- api: T:Inno.Audio.AudioClipLoadMode 2410024c187f29fad103945645c26b8f4e12c4111d10408e8ec033f3d7d0b32e -->
<!-- api: F:Inno.Audio.AudioClipLoadMode.Automatic 0aa9970c87cf4361d490b19357fca0253c03d8da1f3e7c530178b6652ea01223 -->
<!-- api: F:Inno.Audio.AudioClipLoadMode.Decode 15a195ee13b67e92275e44060c773f9974d20b64463f1a613e3981d2da75cd90 -->
<!-- api: F:Inno.Audio.AudioClipLoadMode.Stream dcf6912616484c5d9c35c7bfe00948af8e941aecf6e39c690d06115bbd31082a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f85262dccc1317ed"></a>[`Inno.Audio.AudioClipLoadMode.Automatic = 0`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L14) | Lets the runtime choose between decoded and streamed storage. | — | — |
| <a id="api-292da860eff8ccd4"></a>[`Inno.Audio.AudioClipLoadMode.Decode = 1`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L19) | Decodes the complete clip before playback. | — | — |
| <a id="api-0f5d7f179a52a8fe"></a>[`Inno.Audio.AudioClipLoadMode.Stream = 2`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L24) | Decodes encoded data incrementally while playback advances. | — | — |
| <a id="api-e540725f779f1b86"></a>[`public enum Inno.Audio.AudioClipLoadMode : int`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L9) | Selects how encoded clip data is prepared for playback. | — | — |

### `Inno.Audio.AudioClipMetadata`

<!-- api: T:Inno.Audio.AudioClipMetadata 0a224b04d0038738d95e24727eba9ae22bd80f4c693c9c2bf8b9c090886a73a5 -->
<!-- api: M:Inno.Audio.AudioClipMetadata.#ctor(Inno.Audio.AudioCodecId,System.Int32,System.Int32,System.Int64,System.Int64) b8508b0bef3fc319a04aff8b89feceb1045f1c5b0083cfefba6d24b487c7156d -->
<!-- api: P:Inno.Audio.AudioClipMetadata.codec 131046cdfb4bf638e34e26a9e56bc302d620e6ecd4b79836aa03be0f5959229f -->
<!-- api: P:Inno.Audio.AudioClipMetadata.channels e4c48c3c611c41a9baf61b8baac24e10debacba25f13a64289d2d986381a1b35 -->
<!-- api: P:Inno.Audio.AudioClipMetadata.sampleRate 394143b057f0feba8ef89fad6ca9cbc02037ddf05f84ee2b1776add063a9477d -->
<!-- api: P:Inno.Audio.AudioClipMetadata.frameCount 7ebb6c5d4d91faf36c3dddd6c0048a8379041b2e3c972e8d70a1b3aa00aea7fc -->
<!-- api: P:Inno.Audio.AudioClipMetadata.encodedByteLength 28b00fec4b97c13caca64bd3204a1f201a0027641613f79f7d7b7eca91fa2dc3 -->
<!-- api: P:Inno.Audio.AudioClipMetadata.duration 8415c260104d3c1ca4a40f2dfdf0c54352b1cbb0aaad3f2ad0ea997549bea7b0 -->
<!-- api: M:Inno.Audio.AudioClipMetadata.ToString 0961102d469641200a0b95b49c9f122bc7e5ec7b32fa47846282749a8dafc61e -->
<!-- api: M:Inno.Audio.AudioClipMetadata.op_Inequality(Inno.Audio.AudioClipMetadata,Inno.Audio.AudioClipMetadata) c23e078ee0f8fc90e4c3d6322b242ba35c0375c8d0285aa793495abb3e459210 -->
<!-- api: M:Inno.Audio.AudioClipMetadata.op_Equality(Inno.Audio.AudioClipMetadata,Inno.Audio.AudioClipMetadata) 4093b02982bd156ea45b6eaa4dbfe58baf5955a6495ce8e212682e61a07cd47e -->
<!-- api: M:Inno.Audio.AudioClipMetadata.GetHashCode f75dc68dc3d74d795f051ece001073a56364d92260c669b3a01c3421f84e2e84 -->
<!-- api: M:Inno.Audio.AudioClipMetadata.Equals(System.Object) 49629849bd16ba79aebfd1c17ccbde6083b6fe3affb4002d00b565bf06a301ba -->
<!-- api: M:Inno.Audio.AudioClipMetadata.Equals(Inno.Audio.AudioClipMetadata) b6dd02773fe65a2de66089355e15c705eb52ddb7b36233b5aee5e4be0fe4b77e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-27b5172129463fa0"></a>[`public Inno.Audio.AudioClipMetadata.AudioClipMetadata(Inno.Audio.AudioCodecId codec, int channels, int sampleRate, long frameCount, long encodedByteLength)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L32) | Creates imported audio metadata.<br>`codec`：Codec protocol of the encoded artifact.<br>`channels`：Encoded channel count.<br>`sampleRate`：Sample rate in frames per second.<br>`frameCount`：Total decoded frame count.<br>`encodedByteLength`：Encoded artifact length in bytes. | — | — |
| <a id="api-26a412ac4cf2a84a"></a>[`public bool Inno.Audio.AudioClipMetadata.Equals(Inno.Audio.AudioClipMetadata other)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-44760ca53ca2a4fc"></a>[`public override bool Inno.Audio.AudioClipMetadata.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1e2625876ee5cd1f"></a>[`public override int Inno.Audio.AudioClipMetadata.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-3fcd129ff27585de"></a>[`public static bool Inno.Audio.AudioClipMetadata.operator ==(Inno.Audio.AudioClipMetadata left, Inno.Audio.AudioClipMetadata right)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d5197bf2c82bed46"></a>[`public static bool Inno.Audio.AudioClipMetadata.operator !=(Inno.Audio.AudioClipMetadata left, Inno.Audio.AudioClipMetadata right)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2312ee2437f9a85a"></a>[`public override string Inno.Audio.AudioClipMetadata.ToString()`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-761cbb34e3215b66"></a>[`public int Inno.Audio.AudioClipMetadata.channels { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L60) | Gets the encoded channel count. | — | — |
| <a id="api-66f445103de195f5"></a>[`public Inno.Audio.AudioCodecId Inno.Audio.AudioClipMetadata.codec { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L55) | Gets the codec protocol of the encoded artifact. | — | — |
| <a id="api-30a06dcd81f4c9cd"></a>[`public System.TimeSpan Inno.Audio.AudioClipMetadata.duration { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L80) | Gets the clip duration derived from frame count and sample rate. | — | — |
| <a id="api-622a266ee5c0614b"></a>[`public long Inno.Audio.AudioClipMetadata.encodedByteLength { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L75) | Gets the encoded artifact length in bytes. | — | — |
| <a id="api-f7a6959faf2abed2"></a>[`public long Inno.Audio.AudioClipMetadata.frameCount { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L70) | Gets the total decoded frame count. | — | — |
| <a id="api-ce68de3ec9d7b50b"></a>[`public int Inno.Audio.AudioClipMetadata.sampleRate { get; }`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L65) | Gets the sample rate in frames per second. | — | — |
| <a id="api-0c722fd1ce9fb99a"></a>[`public readonly record struct Inno.Audio.AudioClipMetadata : System.IEquatable<Inno.Audio.AudioClipMetadata>`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L12) | Describes imported encoded audio independently from its runtime storage path. | — | — |

### `Inno.Audio.AudioClipMetadataCodec`

<!-- api: T:Inno.Audio.AudioClipMetadataCodec 3c0fd97b8e271bf6c5ea4b8dea30fbac3e22e9ef2a39af2541956c196fbe8a82 -->
<!-- api: M:Inno.Audio.AudioClipMetadataCodec.Encode(Inno.Audio.AudioClipMetadata) 406e6cf79e83c23990a7be6ee7b8682df8a8156e28fd8af5836a3e0bed75d986 -->
<!-- api: M:Inno.Audio.AudioClipMetadataCodec.Decode(System.ReadOnlySpan{System.Byte}) 1727542b88e80cccccf920392b0c051d9b1a40f11e4d5117429f5eb726c14b17 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1611435d349672e"></a>[`public static Inno.Audio.AudioClipMetadata Inno.Audio.AudioClipMetadataCodec.Decode(System.ReadOnlySpan<byte> payload)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L128) | Decodes and validates one compact audio runtime header.<br>返回：Valid imported audio metadata.<br>`payload`：Complete runtime header bytes. | — | `System.InvalidOperationException`：Thrown when the header is truncated, corrupt, or semantically invalid. |
| <a id="api-a15c45181293e55f"></a>[`public static byte[] Inno.Audio.AudioClipMetadataCodec.Encode(Inno.Audio.AudioClipMetadata metadata)`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L100) | Encodes audio metadata into a compact deterministic runtime header.<br>返回：Newly allocated runtime header bytes.<br>`metadata`：Valid metadata to encode. | — | — |
| <a id="api-dfb969705f5de5c0"></a>[`public static class Inno.Audio.AudioClipMetadataCodec`](../../src/services/audio/Inno.Audio/AudioClipAsset.cs#L86) | Encodes the compact runtime header shared by audio importers and runtime asset loading. | — | — |

### `Inno.Audio.AudioClipState`

<!-- api: T:Inno.Audio.AudioClipState 63de7cf0377fa700e58ddcc89c349ce16debb420fb2815bd88f22adaa20b090b -->
<!-- api: F:Inno.Audio.AudioClipState.Preparing ed89646d144827e51e1a9f219ea2f6acc1e76200b36a6408df40a73c7babb7fd -->
<!-- api: F:Inno.Audio.AudioClipState.Ready 06921b5d603de636b0287970087dc9e4d570ccd7abab87dfca2d34df10160947 -->
<!-- api: F:Inno.Audio.AudioClipState.Failed 4c9c629fdabf211d8f368521c0adf3abfeceb3902830e3c9dd0a5289cafeb4f8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8b53329f9cd6bea9"></a>[`Inno.Audio.AudioClipState.Failed = 2`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L19) | Preparation failed; the clip must be destroyed and callers notified. | — | — |
| <a id="api-cbca2904d18dc403"></a>[`Inno.Audio.AudioClipState.Preparing = 0`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L11) | Native preparation is still in progress and the clip must not start yet. | — | — |
| <a id="api-a41c373692e8e527"></a>[`Inno.Audio.AudioClipState.Ready = 1`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L15) | The retained native data source is ready for playback. | — | — |
| <a id="api-2fb88b4ba6c0ce35"></a>[`public enum Inno.Audio.AudioClipState : int`](../../src/services/audio/Inno.Audio/AudioClipState.cs#L6) | Describes asynchronous preparation of a backend-owned clip generation. | — | — |

### `Inno.Audio.AudioCodecId`

<!-- api: T:Inno.Audio.AudioCodecId 2860462f0a8162c9e4c92fa72c59291b63a974f147f04f334daf61cf01583676 -->
<!-- api: M:Inno.Audio.AudioCodecId.#ctor(System.String) 34a874ae35447de0b8c26c9d769146fbd1ae75d228bbf4373d641700975d61cd -->
<!-- api: P:Inno.Audio.AudioCodecId.wav f4994b26ecac0871e4a3423d7f1773b3dbf6de7604dde8663e13f52af6dd52f9 -->
<!-- api: P:Inno.Audio.AudioCodecId.flac de83c110416f285634cef5423cdb2b3c5fa7e78e452928adb7a60c72a642e87e -->
<!-- api: P:Inno.Audio.AudioCodecId.mp3 0b3c5b3257356b980c42c3971502ab187967a688655368a49022d3f4cf1aa723 -->
<!-- api: P:Inno.Audio.AudioCodecId.value b00304ed9c8c04fa2c3b410a28e65932f344967ae3bd535cc7ef8f7f6a89c1ba -->
<!-- api: P:Inno.Audio.AudioCodecId.isValid af00c83ae7e3ee5ac7e25d7fbe2dc2705b228209d7a6c14fb510ae6d3ee18461 -->
<!-- api: M:Inno.Audio.AudioCodecId.ToString 01bb63ba9c0b212356d55e3d6a2cb48e49b1cc9116859800eed724b6143751ce -->
<!-- api: M:Inno.Audio.AudioCodecId.op_Inequality(Inno.Audio.AudioCodecId,Inno.Audio.AudioCodecId) 60986e21e5e2eab4db42246323f0491fa3ddeb3ea14ae3f81db434da141a6344 -->
<!-- api: M:Inno.Audio.AudioCodecId.op_Equality(Inno.Audio.AudioCodecId,Inno.Audio.AudioCodecId) 6a300829a9db31c62b64e23d5ee301d6baef88bfa3267572b1e8104e3b1a4e80 -->
<!-- api: M:Inno.Audio.AudioCodecId.GetHashCode 8b3d318fddc3cde43e2b6a1a2f626f4a042bc3895239070a1eb3314064eec1fc -->
<!-- api: M:Inno.Audio.AudioCodecId.Equals(System.Object) a7b7b79205cdbf0a261d1d8fb47d07dc3436b0bd46460b240d81f965ea274105 -->
<!-- api: M:Inno.Audio.AudioCodecId.Equals(Inno.Audio.AudioCodecId) 12c37c66d5087cd1fba215d7b00924b72a71078177afd9070200b5237c8dda29 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1af5bde7e66b25f1"></a>[`public Inno.Audio.AudioCodecId.AudioCodecId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L199) | Creates an open codec identifier.<br>`value`：Globally stable codec protocol value. | — | — |
| <a id="api-8a87c9752741b2ef"></a>[`public bool Inno.Audio.AudioCodecId.Equals(Inno.Audio.AudioCodecId other)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-09d02a61b685e1ff"></a>[`public override bool Inno.Audio.AudioCodecId.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-6e48b2df68aa571c"></a>[`public override int Inno.Audio.AudioCodecId.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-9f3e015887fca32f"></a>[`public static bool Inno.Audio.AudioCodecId.operator ==(Inno.Audio.AudioCodecId left, Inno.Audio.AudioCodecId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-54cb4b8d0d13ffe4"></a>[`public static bool Inno.Audio.AudioCodecId.operator !=(Inno.Audio.AudioCodecId left, Inno.Audio.AudioCodecId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ac867add019aa5ff"></a>[`public override string Inno.Audio.AudioCodecId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L236) | Formats the identifier for diagnostics and persistence.<br>返回：The stable protocol value. | — | — |
| <a id="api-e0b34c3b7404edba"></a>[`public static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.flac { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L213) | Gets the standard Free Lossless Audio Codec identifier. | — | — |
| <a id="api-625d6c559070c87a"></a>[`public bool Inno.Audio.AudioCodecId.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L228) | Gets whether this identifier contains a usable protocol value. | — | — |
| <a id="api-fceed0cdd26557e8"></a>[`public static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.mp3 { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L218) | Gets the standard MPEG Layer III codec identifier. | — | — |
| <a id="api-2ddaf59606fcb070"></a>[`public string Inno.Audio.AudioCodecId.value { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L223) | Gets the globally stable protocol value. | — | — |
| <a id="api-bcc4e39fa39b822e"></a>[`public static Inno.Audio.AudioCodecId Inno.Audio.AudioCodecId.wav { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L208) | Gets the standard Waveform Audio codec identifier. | — | — |
| <a id="api-2ad16f6c5b70077f"></a>[`public readonly record struct Inno.Audio.AudioCodecId : System.IEquatable<Inno.Audio.AudioCodecId>`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L191) | Identifies encoded audio data without constraining codec providers to a closed enum. | — | — |

### `Inno.Audio.AudioCompletionReason`

<!-- api: T:Inno.Audio.AudioCompletionReason d1c886e7e7fb14ad9a2e266ba14c492dad8b51d7a3b2c8bfa8198d852f991342 -->
<!-- api: F:Inno.Audio.AudioCompletionReason.NaturalEnd af9852fe7944576271f7ae0f8b994c8302afedf2e20e9b310dd3b2105f99b54d -->
<!-- api: F:Inno.Audio.AudioCompletionReason.Stopped c0c7b0f7c4d725d0d675ebad67b639e6805ae298b9ed2d87594eefa4d7040e8d -->
<!-- api: F:Inno.Audio.AudioCompletionReason.Stolen 666332a052a7143cd9dfbb8ec977da291701b96bfdc7296517e3121f417f8bb1 -->
<!-- api: F:Inno.Audio.AudioCompletionReason.DecodeFailed aa6dc23af68916e36079031e769a5746908d971a40f0a6600fc3a6695d02b1b7 -->
<!-- api: F:Inno.Audio.AudioCompletionReason.DeviceLost 21b2481800797e7fac6a5cb812a2df79fdf67ab129e767a6199e2d5c920300fb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-25188d365abf4675"></a>[`Inno.Audio.AudioCompletionReason.DecodeFailed = 3`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L117) | Encoded data could not be prepared or decoded. | — | — |
| <a id="api-41eb98a566bc1009"></a>[`Inno.Audio.AudioCompletionReason.DeviceLost = 4`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L122) | Playback ended because the owning device generation was lost. | — | — |
| <a id="api-cc15da90d67cb2d5"></a>[`Inno.Audio.AudioCompletionReason.NaturalEnd = 0`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L102) | The clip reached its natural end. | — | — |
| <a id="api-9ad5e073bbe74ec4"></a>[`Inno.Audio.AudioCompletionReason.Stolen = 2`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L112) | The runtime reclaimed the voice to satisfy its voice budget. | — | — |
| <a id="api-17319fd8cc5f6031"></a>[`Inno.Audio.AudioCompletionReason.Stopped = 1`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L107) | A caller explicitly stopped the voice. | — | — |
| <a id="api-9931c8423d07ca64"></a>[`public enum Inno.Audio.AudioCompletionReason : int`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L97) | Explains why a voice stopped accepting playback commands. | — | — |

### `Inno.Audio.AudioContentProvider`

<!-- api: T:Inno.Audio.AudioContentProvider 5c3279fbf73dae080504d46f7e2aef7296dc21718943bf375953763be9a1374e -->
<!-- api: M:Inno.Audio.AudioContentProvider.Submit(Inno.Audio.AudioContentProviderContext) 4cfdec2a5aaad0dae53d5dc6d1fa36e0d3883e48eaa298adde98e0b68069ac8f -->
<!-- api: M:Inno.Audio.AudioContentProvider.Dispose 18ad1eb6d9e2f85af84767b2847e8e9402cf90b711ace30f9bd28cea52bf9263 -->
<!-- api: M:Inno.Audio.AudioContentProvider.Dispose(System.Boolean) b114591d3f1ed4334ed6d1a3a41e5ad7fd339d4d6b1f1e10ad5b2f85eb4808a7 -->
<!-- api: M:Inno.Audio.AudioContentProvider.#ctor 1536382d7b136218e9f2a91d67063d376af0257bb00581be6c8dad10debd926e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a8e685d27870860a"></a>[`protected Inno.Audio.AudioContentProvider.AudioContentProvider()`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L252) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-78c3f9fdd6fcf2a9"></a>[`public void Inno.Audio.AudioContentProvider.Dispose()`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L270) | Releases generation-scoped provider state. | — | `Inno.Core.Execution.RetirementPendingException`：Owned work is still draining. The generation owner must retain this provider and retry disposal. |
| <a id="api-07764cfe9d95e7f9"></a>[`protected virtual void Inno.Audio.AudioContentProvider.Dispose(bool disposing)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L297) | Releases managed generation-scoped state.<br>`disposing`：Always true for explicit disposal. | — | — |
| <a id="api-23f726ba067161d9"></a>[`public abstract void Inno.Audio.AudioContentProvider.Submit(Inno.Audio.AudioContentProviderContext context)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L262) | Submits current immutable audio snapshots on the host control thread.<br>`context`：Update-scoped content and snapshot collector. | — | — |
| <a id="api-b2fdd25fcac700a8"></a>[`public abstract class Inno.Audio.AudioContentProvider : System.IDisposable`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L252) | Converts arbitrary host-owned content into backend-neutral emitter and listener snapshots. | — | — |

### `Inno.Audio.AudioContentProviderContext`

<!-- api: T:Inno.Audio.AudioContentProviderContext 967a147a11e8e5b912041123a70d0f9bb1311cbf3b811871a6e9621e3b9414d3 -->
<!-- api: M:Inno.Audio.AudioContentProviderContext.#ctor(Inno.References.ContentReadScope,System.Single,System.Int32) df0d37eb83eff0236d707e75488cc3ba6123789d4a5f008eeaa5b274a9bc5d37 -->
<!-- api: P:Inno.Audio.AudioContentProviderContext.content 6c4b16430660b8da3b1ceb3647a433233ad13f87027d4c48710843aa5bb975b8 -->
<!-- api: P:Inno.Audio.AudioContentProviderContext.deltaTime ba00030bc4b43c5218bb24836acbaa2768753dbe6845f95904e7b31bb5318300 -->
<!-- api: P:Inno.Audio.AudioContentProviderContext.capacity e4900c08fe175db977de10213b4181caac800530cb2640b47d982511e6a9fd43 -->
<!-- api: M:Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioEmitterSnapshot) b620a1444d1efeb61229ea7eadc73f52b700fe6c6687d7e98649888398605af5 -->
<!-- api: M:Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioListenerSnapshot) e619b12461caa2e7201e97f7c83087cd8f13df11293d787ee03d7f2e248bc5b6 -->
<!-- api: P:Inno.Audio.AudioContentProviderContext.emitters db1860b19617b7d00b25c3ca80eb06505ea81811bd173f61b254daca2700a382 -->
<!-- api: P:Inno.Audio.AudioContentProviderContext.listeners b139134ef3a2bc159ee2d30529e50cedda92f6a825427a4972d5e8e7bbfa96df -->
<!-- api: M:Inno.Audio.AudioContentProviderContext.Dispose 7e5225719a3810b33e18e59c77fafe925770e6dcbf8a8cde27eacc01fcf1c4f6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a0ab6a7239b347dd"></a>[`public Inno.Audio.AudioContentProviderContext.AudioContentProviderContext(Inno.References.ContentReadScope content, float deltaTime, int capacity = 1024)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L77) | Opens a control-thread snapshot collector over one content read scope.<br>`content`：The caller-owned identity read scope.<br>`deltaTime`：Finite non-negative time elapsed since the previous update.<br>`capacity`：Maximum combined emitter and listener count accepted by this contribution. Zero permits an empty contribution only. | — | `System.ArgumentOutOfRangeException`：The elapsed time is invalid or capacity is negative.<br>`System.ArgumentNullException`：The borrowed content scope is null. |
| <a id="api-ff7f0fad07aa3d41"></a>[`public void Inno.Audio.AudioContentProviderContext.Dispose()`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L210) | Revokes submission and releases collected references without disposing the borrowed content scope. | — | `System.InvalidOperationException`：Disposal occurs off the owning control thread. |
| <a id="api-9209cc0f5deb45a1"></a>[`public void Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioEmitterSnapshot emitter)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L134) | Submits one immutable emitter snapshot for runtime synchronization.<br>`emitter`：Current state for one stable emitter identity. | — | `System.ArgumentException`：The emitter is uninitialized or its identity duplicates a submission in this contribution.<br>`System.InvalidOperationException`：The budget is exhausted, the contribution was rejected, or access occurs off the owner thread.<br>`System.ObjectDisposedException`：This provider invocation has ended. |
| <a id="api-76832aed389fe4db"></a>[`public void Inno.Audio.AudioContentProviderContext.Submit(Inno.Audio.AudioListenerSnapshot listener)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L161) | Submits one immutable listener snapshot for runtime selection.<br>`listener`：Current state for one stable listener identity. | — | `System.ArgumentException`：The listener is uninitialized or its identity duplicates a submission in this contribution.<br>`System.InvalidOperationException`：The budget is exhausted, the contribution was rejected, or access occurs off the owner thread.<br>`System.ObjectDisposedException`：This provider invocation has ended. |
| <a id="api-f899246a48ea2458"></a>[`public int Inno.Audio.AudioContentProviderContext.capacity { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L117) | Gets the immutable combined snapshot budget assigned to this invocation. | — | — |
| <a id="api-7beb77d500134e67"></a>[`public Inno.References.ContentReadScope Inno.Audio.AudioContentProviderContext.content { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L100) | Gets explicit host content visible during this update. | — | `System.ObjectDisposedException`：This provider invocation has ended.<br>`System.InvalidOperationException`：Access occurs off the owning control thread or a submission was rejected. |
| <a id="api-46065ceb82614af6"></a>[`public float Inno.Audio.AudioContentProviderContext.deltaTime { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L112) | Gets non-negative elapsed frame time in seconds. | — | — |
| <a id="api-38cf95412a2bb24a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioEmitterSnapshot> Inno.Audio.AudioContentProviderContext.emitters { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L177) | Gets a frozen copy of submitted source state in provider order. | — | `System.ObjectDisposedException`：This provider invocation has ended.<br>`System.InvalidOperationException`：The contribution was rejected or access occurs off the owning control thread. |
| <a id="api-bb5dd9c3bc3f0654"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioListenerSnapshot> Inno.Audio.AudioContentProviderContext.listeners { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L195) | Gets a frozen copy of submitted listener state in provider order. | — | `System.ObjectDisposedException`：This provider invocation has ended.<br>`System.InvalidOperationException`：The contribution was rejected or access occurs off the owning control thread. |
| <a id="api-9ba5ad2168e42c86"></a>[`public sealed class Inno.Audio.AudioContentProviderContext : System.IDisposable`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L50) | Collects one bounded, atomic provider contribution on the owning control thread. | The host disposes this borrowed context when the provider returns. A rejected submission invalidates the entire contribution, even if the provider catches the exception. The context does not own the content scope. | — |

### `Inno.Audio.AudioContentProviderExtensionAttribute`

<!-- api: T:Inno.Audio.AudioContentProviderExtensionAttribute cfa0424567e956b7a161526d3345b435d78d58971e2fdf0d1530743790e61bac -->
<!-- api: M:Inno.Audio.AudioContentProviderExtensionAttribute.#ctor(System.String,System.Int32) 577d067b5afe242461e376a2698732e68333b78fd447ad0e795732843cf98909 -->
<!-- api: P:Inno.Audio.AudioContentProviderExtensionAttribute.id e931593aa17622297c85fa2e0000da02305e7d15d0ad23abfb4286f16fd78674 -->
<!-- api: P:Inno.Audio.AudioContentProviderExtensionAttribute.priority 3851fc101a581c79f338e0d369f885025874044152f83ca4cfc1a330f28d7435 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-45fa8dcacf12a684"></a>[`public Inno.Audio.AudioContentProviderExtensionAttribute.AudioContentProviderExtensionAttribute(string id, int priority = 0)`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L23) | Creates an audio content provider declaration.<br>`id`：Globally stable provider identifier.<br>`priority`：Provider invocation priority; lower values run first. | — | — |
| <a id="api-b5f221c369430df5"></a>[`public string Inno.Audio.AudioContentProviderExtensionAttribute.id { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L35) | Gets the globally stable provider identifier. | — | — |
| <a id="api-87215a1c8c1d1380"></a>[`public int Inno.Audio.AudioContentProviderExtensionAttribute.priority { get; }`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L40) | Gets the provider invocation priority. | — | — |
| <a id="api-3f94751ab11a07f0"></a>[`public sealed class Inno.Audio.AudioContentProviderExtensionAttribute : System.Attribute`](../../src/services/audio/Inno.Audio/AudioContentProvider.cs#L12) | Marks a reloadable provider that converts host content into immutable audio snapshots. | — | — |

### `Inno.Audio.AudioDevice`

<!-- api: T:Inno.Audio.AudioDevice fe64959175beb0046223fb38cb0588118a44e1ec0462cd8a64899216904f951f -->
<!-- api: M:Inno.Audio.AudioDevice.CreateClipHandle(System.UInt64,System.UInt32) 0f975e15ef5083e37b12163603c80e20320b89090040eb1b842e4d86d0499785 -->
<!-- api: M:Inno.Audio.AudioDevice.CreateVoiceHandle(System.UInt64,System.UInt32) 5abc85936018753e87ab6982df73981336c51979833d7a97ca86d02e8c673d45 -->
<!-- api: M:Inno.Audio.AudioDevice.CreateBusHandle(System.UInt64,System.UInt32) fcb595c138d3001d415564a1977af62afe75637c4c12db5f22d9f2c66cb9610e -->
<!-- api: M:Inno.Audio.AudioDevice.CreateListenerHandle(System.UInt64,System.UInt32) 9061aff4b7564b12fb181177577258f74e20472a1cab775a1c821b5a125ff574 -->
<!-- api: M:Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioClipHandle) fa0279961d6027b624efab385fef2de92e82718b944fd3387d0e9b8f259f15ee -->
<!-- api: M:Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioDeviceVoiceHandle) 064ab809361e6592d90da6960a7cae11ca63b845137a82da4b8223ec0e4065df -->
<!-- api: M:Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioBusHandle) d5c7717da561498e0d3a6ba5885e7b80d17ef5b75d46132e6825d309c1456419 -->
<!-- api: M:Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioListenerHandle) 3394ac72b2f25e2017a40ecc91b974d10c1ecd10aeea7133e69511e96236fc57 -->
<!-- api: M:Inno.Audio.AudioDevice.#ctor 931256302d2801e405a505ee633bbc6375eabc196e6a1e730f1a83dccecfcad8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-46b7974822198edf"></a>[`protected Inno.Audio.AudioDevice.AudioDevice()`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L6) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-49ceb10e7cd6c61c"></a>[`protected static Inno.Audio.AudioBusHandle Inno.Audio.AudioDevice.CreateBusHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L68) | Encodes a bus identity into a backend-neutral handle.<br>返回：An opaque bus handle.<br>`value`：Non-zero backend bus identity.<br>`generation`：Owning device generation. | — | — |
| <a id="api-689cb5e8bda99693"></a>[`protected static Inno.Audio.AudioClipHandle Inno.Audio.AudioDevice.CreateClipHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L34) | Encodes a clip identity into a backend-neutral handle.<br>返回：An opaque clip handle.<br>`value`：Non-zero backend clip identity.<br>`generation`：Owning device generation. | — | — |
| <a id="api-f1e389e1c772adf7"></a>[`protected static Inno.Audio.AudioListenerHandle Inno.Audio.AudioDevice.CreateListenerHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L85) | Encodes a listener identity into a backend-neutral handle.<br>返回：An opaque listener handle.<br>`value`：Non-zero backend listener identity.<br>`generation`：Owning device generation. | — | — |
| <a id="api-d63be15f8662c87d"></a>[`protected static Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.AudioDevice.CreateVoiceHandle(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L51) | Encodes a voice identity into a backend-neutral handle.<br>返回：An opaque voice handle.<br>`value`：Non-zero backend voice identity.<br>`generation`：Owning device generation. | — | — |
| <a id="api-6b328fd9a696252e"></a>[`protected static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioBusHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L121) | Decodes a bus handle for backend lookup and generation validation.<br>返回：Backend identity and owning generation.<br>`handle`：Backend-neutral bus handle. | — | — |
| <a id="api-20777736c1c6df0e"></a>[`protected static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioClipHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L99) | Decodes a clip handle for backend lookup and generation validation.<br>返回：Backend identity and owning generation.<br>`handle`：Backend-neutral clip handle. | — | — |
| <a id="api-0347afdb5dce0fd0"></a>[`protected static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioDeviceVoiceHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L110) | Decodes a voice handle for backend lookup and generation validation.<br>返回：Backend identity and owning generation.<br>`handle`：Backend-neutral voice handle. | — | — |
| <a id="api-90025701ae7e220b"></a>[`protected static Inno.Audio.AudioDevice.DeviceHandleIdentity Inno.Audio.AudioDevice.GetHandleIdentity(Inno.Audio.AudioListenerHandle handle)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L132) | Decodes a listener handle for backend lookup and generation validation.<br>返回：Backend identity and owning generation.<br>`handle`：Backend-neutral listener handle. | — | — |
| <a id="api-679c55a8ac6b4d6a"></a>[`public abstract class Inno.Audio.AudioDevice`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L6) | Provides protected opaque-handle encoding and validation helpers for audio backends. | — | — |

### `Inno.Audio.AudioDevice.DeviceHandleIdentity`

<!-- api: T:Inno.Audio.AudioDevice.DeviceHandleIdentity 8a8c2d2d7acb0618a8206dc577acc17ffd156f555a54311d84f69bdfbec41797 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.#ctor(System.UInt64,System.UInt32) d3e0507916bd6c4078cec18efe705092979c29b471c2d511f780b76706601e5c -->
<!-- api: P:Inno.Audio.AudioDevice.DeviceHandleIdentity.value 009d1416e9ff663dc961a3f4e0d676db3c9eb4608839a9d0b387e0e3ab3591dd -->
<!-- api: P:Inno.Audio.AudioDevice.DeviceHandleIdentity.generation 5c1ff52895e99886c2321f523bf23a80fd92388b3888260da779c54bc40f3887 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.ToString 9752743db83272dbe0277f287768ce9e07b21323f013c4aab0285ad3c772b6b6 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.op_Inequality(Inno.Audio.AudioDevice.DeviceHandleIdentity,Inno.Audio.AudioDevice.DeviceHandleIdentity) 9744923f6746ea6fc0e33d203debb82d345c8f6c3e8bd3708c391e1a0c2ce1d9 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.op_Equality(Inno.Audio.AudioDevice.DeviceHandleIdentity,Inno.Audio.AudioDevice.DeviceHandleIdentity) ab17b44144380f6ac38c524f6210a71d9ab29af2e8c36badbe9b69ae51335adc -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.GetHashCode 777a3ce9aabcc83eab9827be5d29741fd28d326145ef9824d63c3f4ce63b2d28 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.Equals(System.Object) c3485a08afa12d9538ea18ed76a920bdd38f2d65010628424b7989f7a3234533 -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.Equals(Inno.Audio.AudioDevice.DeviceHandleIdentity) b42380311ee954ecac7657da1f3c7b362bd3303587eeffc3c6820a8abeb715ce -->
<!-- api: M:Inno.Audio.AudioDevice.DeviceHandleIdentity.Deconstruct(System.UInt64@,System.UInt32@) 1778fdabf87687b70cac6f0eb106c4b4e94b9044d12a61dd98e71ab5922c5b3e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9d405fa8f35cf00c"></a>[`public Inno.Audio.AudioDevice.DeviceHandleIdentity.DeviceHandleIdentity(ulong value, uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Stores one decoded device-owned identity.<br>`value`：Non-zero backend resource identity.<br>`generation`：Device generation that owns the resource. | — | — |
| <a id="api-a563ebfbf0e8ce63"></a>[`public void Inno.Audio.AudioDevice.DeviceHandleIdentity.Deconstruct(out ulong value, out uint generation)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-97dcf809a2ce23c8"></a>[`public bool Inno.Audio.AudioDevice.DeviceHandleIdentity.Equals(Inno.Audio.AudioDevice.DeviceHandleIdentity other)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-474f34b4f6667c65"></a>[`public override bool Inno.Audio.AudioDevice.DeviceHandleIdentity.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-eb8d08c5a4e36cf4"></a>[`public override int Inno.Audio.AudioDevice.DeviceHandleIdentity.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-632db581442a38de"></a>[`public static bool Inno.Audio.AudioDevice.DeviceHandleIdentity.operator ==(Inno.Audio.AudioDevice.DeviceHandleIdentity left, Inno.Audio.AudioDevice.DeviceHandleIdentity right)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f620bfc657184378"></a>[`public static bool Inno.Audio.AudioDevice.DeviceHandleIdentity.operator !=(Inno.Audio.AudioDevice.DeviceHandleIdentity left, Inno.Audio.AudioDevice.DeviceHandleIdentity right)`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-624f0b1dfd07aaa5"></a>[`public override string Inno.Audio.AudioDevice.DeviceHandleIdentity.ToString()`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-912ffafb7dfd3c10"></a>[`public uint Inno.Audio.AudioDevice.DeviceHandleIdentity.generation { get; init; }`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L19) | Device generation that owns the resource. | — | — |
| <a id="api-85ff85d110eabcb3"></a>[`public ulong Inno.Audio.AudioDevice.DeviceHandleIdentity.value { get; init; }`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L18) | Non-zero backend resource identity. | — | — |
| <a id="api-f90ed3527b4ebc22"></a>[`protected readonly record struct Inno.Audio.AudioDevice.DeviceHandleIdentity : System.IEquatable<Inno.Audio.AudioDevice.DeviceHandleIdentity>`](../../src/services/audio/Inno.Audio/AudioDevice.cs#L17) | Stores one decoded device-owned identity.<br>`value`：Non-zero backend resource identity.<br>`generation`：Device generation that owns the resource. | — | — |

### `Inno.Audio.AudioDeviceCompletion`

<!-- api: T:Inno.Audio.AudioDeviceCompletion fbe4b7cee7e6461a0b6029d566d25e6e9de11a1a0dc9e133fa9da10888a5222b -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.#ctor(Inno.Audio.AudioDeviceVoiceHandle,Inno.Audio.AudioCompletionReason) 8cf94891448b06e3e113b94ab030ed2fa3d1ea18fe5feb75a42eec452b73ed28 -->
<!-- api: P:Inno.Audio.AudioDeviceCompletion.voice 8bef00f90309f7294e66ff51bcfeeb39e4924719bbc412608b779a321faa7678 -->
<!-- api: P:Inno.Audio.AudioDeviceCompletion.reason 6574091a17632dfb9ec1488ebc3fc038e3e2dbe8a0dcc3c4c5325c1b6f573bc1 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.ToString 4ef3a2597f0eebecee4a986e6688ec55f11fe274ecc2e592e295e75598150d32 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.op_Inequality(Inno.Audio.AudioDeviceCompletion,Inno.Audio.AudioDeviceCompletion) 8f892b2fabb18db6d7bec4056822911b8bffa71f369bbce99f83919ffb0cf8c4 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.op_Equality(Inno.Audio.AudioDeviceCompletion,Inno.Audio.AudioDeviceCompletion) ea97fef1e4a78420a3e6de515e0706d8423871c2450c8db4668b169fc90827f7 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.GetHashCode 4279811e1927baba62d30ad92c8fbedbf83ffa598e5117cd6d036983aa9a86c2 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.Equals(System.Object) 735041d9c293f7732ec8e827d516a0c14dbe916d2efccb8e5d49fcd3cea8ab85 -->
<!-- api: M:Inno.Audio.AudioDeviceCompletion.Equals(Inno.Audio.AudioDeviceCompletion) 45cf45a09143163db9fa2701f2532702dbfcb1dc6d5dffc226868bcbdb940862 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a2818301dfcb6a21"></a>[`public Inno.Audio.AudioDeviceCompletion.AudioDeviceCompletion(Inno.Audio.AudioDeviceVoiceHandle voice, Inno.Audio.AudioCompletionReason reason)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L19) | Creates a backend voice completion record.<br>`voice`：Voice that reached a terminal state.<br>`reason`：Reason playback ended. | — | — |
| <a id="api-99cfb321e4ee3448"></a>[`public bool Inno.Audio.AudioDeviceCompletion.Equals(Inno.Audio.AudioDeviceCompletion other)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a8d07afc9885fe9c"></a>[`public override bool Inno.Audio.AudioDeviceCompletion.Equals(object obj)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-77823feb6bf228fd"></a>[`public override int Inno.Audio.AudioDeviceCompletion.GetHashCode()`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-9defcf1ea0427c83"></a>[`public static bool Inno.Audio.AudioDeviceCompletion.operator ==(Inno.Audio.AudioDeviceCompletion left, Inno.Audio.AudioDeviceCompletion right)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a0dd63a7383d0d7b"></a>[`public static bool Inno.Audio.AudioDeviceCompletion.operator !=(Inno.Audio.AudioDeviceCompletion left, Inno.Audio.AudioDeviceCompletion right)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-721c937832019f03"></a>[`public override string Inno.Audio.AudioDeviceCompletion.ToString()`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-456cc284614d5f24"></a>[`public Inno.Audio.AudioCompletionReason Inno.Audio.AudioDeviceCompletion.reason { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L37) | Gets the terminal playback reason. | — | — |
| <a id="api-c3aaaba4f93f7a17"></a>[`public Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.AudioDeviceCompletion.voice { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L32) | Gets the completed voice handle. | — | — |
| <a id="api-56f47e5f61cc32d1"></a>[`public readonly record struct Inno.Audio.AudioDeviceCompletion : System.IEquatable<Inno.Audio.AudioDeviceCompletion>`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L8) | Reports one backend-detected terminal voice transition. | — | — |

### `Inno.Audio.AudioDeviceLimits`

<!-- api: T:Inno.Audio.AudioDeviceLimits 488470c25474583f1808279d95612de0b234fe1467558a0c95190bc0607c5b1d -->
<!-- api: M:Inno.Audio.AudioDeviceLimits.#ctor(System.Int32,System.Int32,System.Int32) 064eb0121ce0cd4bfc37ee8e000a9e46ac7fc21827f8234786ba8dd40ba0eef2 -->
<!-- api: P:Inno.Audio.AudioDeviceLimits.clips 6875a4912fa81c8289d5cb6fd08ef3858ce60cf7e02891c23272ada625fe737d -->
<!-- api: P:Inno.Audio.AudioDeviceLimits.voices f1c06668db6350ffb86b20eda354664fef24ad2bbe055ed34069aa4f3847fb77 -->
<!-- api: P:Inno.Audio.AudioDeviceLimits.buses e7c4170f529269529a61514eebee1cb61bb1472995c7e3629bb5035a90335b1e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f6ec0dfe4d2b66d4"></a>[`public Inno.Audio.AudioDeviceLimits.AudioDeviceLimits(int clips = 16384, int voices = 65536, int buses = 4096)`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L25) | Creates immutable positive capacities for one backend generation.<br>`clips`：Maximum simultaneously retained clip descriptors and native sources.<br>`voices`：Maximum active voices plus undelivered completions.<br>`buses`：Maximum simultaneously retained mixer buses. | — | `System.ArgumentOutOfRangeException`：A capacity is not positive. |
| <a id="api-718837def761de4e"></a>[`public int Inno.Audio.AudioDeviceLimits.buses { get; }`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L49) | Gets the maximum retained bus count. | — | — |
| <a id="api-4afc64f4d89fdebb"></a>[`public int Inno.Audio.AudioDeviceLimits.clips { get; }`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L41) | Gets the maximum retained clip count. | — | — |
| <a id="api-ad64238ded3766bc"></a>[`public int Inno.Audio.AudioDeviceLimits.voices { get; }`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L45) | Gets the combined voice and undelivered completion capacity. | — | — |
| <a id="api-9e4fc986f1ff8167"></a>[`public sealed class Inno.Audio.AudioDeviceLimits`](../../src/services/audio/Inno.Audio/AudioDeviceLimits.cs#L8) | Bounds device-side resource admission independently of the Runtime's smaller gameplay voice budget. | — | — |

### `Inno.Audio.AudioDeviceState`

<!-- api: T:Inno.Audio.AudioDeviceState ffe5570dfcafed55b8a7b5232d8bb082b560a3a0c92c67204b30d44b6aecd87b -->
<!-- api: F:Inno.Audio.AudioDeviceState.Initializing f0970fe18065404b12ae5c435aa40715f118b5ffba01c37c169a668c39abbb3b -->
<!-- api: F:Inno.Audio.AudioDeviceState.Ready cd25e042631224974a369ed5f1ef4552f8885d3514920cceea1c20acf347d7a4 -->
<!-- api: F:Inno.Audio.AudioDeviceState.Muted f3ba0f23e804c6e734a4050facce78ee4cfba93481fec69cb826e2cc212da068 -->
<!-- api: F:Inno.Audio.AudioDeviceState.Lost 8e73d9f351e9992b9cc882b5e45591d3f09b96e999a99583a3e9891c6aa339e0 -->
<!-- api: F:Inno.Audio.AudioDeviceState.Disposed bb85f8abab195df2331bb45bce3910b3552eace2cf2b527a31d29f4a40cd6bc9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a2b470f488e84e42"></a>[`Inno.Audio.AudioDeviceState.Disposed = 4`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L55) | The device has been released and cannot accept work. | — | — |
| <a id="api-cd1c7b7c5c73f108"></a>[`Inno.Audio.AudioDeviceState.Initializing = 0`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L35) | The device has not completed initialization. | — | — |
| <a id="api-826bfc61301eba77"></a>[`Inno.Audio.AudioDeviceState.Lost = 3`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L50) | The active output device was lost and recovery is pending. | — | — |
| <a id="api-734f41e394746051"></a>[`Inno.Audio.AudioDeviceState.Muted = 2`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L45) | Playback is advancing without an output device. | — | — |
| <a id="api-52c1b1d7f570c05b"></a>[`Inno.Audio.AudioDeviceState.Ready = 1`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L40) | The device is available for audible playback. | — | — |
| <a id="api-2800c2fa99609ce2"></a>[`public enum Inno.Audio.AudioDeviceState : int`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L30) | Describes the availability of the active audio output generation. | — | — |

### `Inno.Audio.AudioDeviceVoiceHandle`

<!-- api: T:Inno.Audio.AudioDeviceVoiceHandle 9542ac5c1264ee343ecba5e77e441a24e327076659a5db30373a99c10419ddec -->
<!-- api: P:Inno.Audio.AudioDeviceVoiceHandle.deviceGeneration bb99abbec886780b4d82422ec80ce11a2bd90b05c5755de2eceb50005272f2f0 -->
<!-- api: P:Inno.Audio.AudioDeviceVoiceHandle.isValid 5a6666b318d76223139872565de8dea73c3198fd354e3d1893061c1d3ae88829 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.ToString e55092f3d8cb7daaad291485d5a5e10bcf31d0d9070891a7298e2de1e4fff477 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.op_Inequality(Inno.Audio.AudioDeviceVoiceHandle,Inno.Audio.AudioDeviceVoiceHandle) 717bafe5a4e4725b37f85e967aacf50993a7e97521b77fef3dde21806a8e7226 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.op_Equality(Inno.Audio.AudioDeviceVoiceHandle,Inno.Audio.AudioDeviceVoiceHandle) bce251483caa68722e03ca1e27b33bcf8c13a9a02b86224acb59542aa7bf21a4 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.GetHashCode 05b0babac6397cd7220285548ccad8d9e0c06a9f97973ad7c28fd5d4a981aac9 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.Equals(System.Object) 1104e8bb1b29c353f3a70043e3e606a83d4fc8fed40a0e567fb2e37c9f8d7291 -->
<!-- api: M:Inno.Audio.AudioDeviceVoiceHandle.Equals(Inno.Audio.AudioDeviceVoiceHandle) f1417fe94bca8556952920a547995fc51258395520bfa8b3f1b2abc7cbc096a4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-875cc88285cdc26f"></a>[`public bool Inno.Audio.AudioDeviceVoiceHandle.Equals(Inno.Audio.AudioDeviceVoiceHandle other)`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-fdc89bc4d2b0f060"></a>[`public override bool Inno.Audio.AudioDeviceVoiceHandle.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f4df1214f5f2de8a"></a>[`public override int Inno.Audio.AudioDeviceVoiceHandle.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-565767e3ea3cd6aa"></a>[`public static bool Inno.Audio.AudioDeviceVoiceHandle.operator ==(Inno.Audio.AudioDeviceVoiceHandle left, Inno.Audio.AudioDeviceVoiceHandle right)`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-68898855f411f7ba"></a>[`public static bool Inno.Audio.AudioDeviceVoiceHandle.operator !=(Inno.Audio.AudioDeviceVoiceHandle left, Inno.Audio.AudioDeviceVoiceHandle right)`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-646e5e2f6e85cd2d"></a>[`public override string Inno.Audio.AudioDeviceVoiceHandle.ToString()`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-d9e14bdb1d1fddd4"></a>[`public uint Inno.Audio.AudioDeviceVoiceHandle.deviceGeneration { get; }`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L20) | Gets the backend device generation that owns this voice. | — | — |
| <a id="api-120bdb5f5668e63c"></a>[`public bool Inno.Audio.AudioDeviceVoiceHandle.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L24) | Gets whether this value identifies a backend allocation. | — | — |
| <a id="api-eda1cdfae86c4f34"></a>[`public readonly record struct Inno.Audio.AudioDeviceVoiceHandle : System.IEquatable<Inno.Audio.AudioDeviceVoiceHandle>`](../../src/services/audio/Inno.Audio/AudioDeviceVoiceHandle.cs#L6) | Identifies a backend voice allocation, distinct from a service's preparing playback handle. | — | — |

### `Inno.Audio.AudioDistanceModel`

<!-- api: T:Inno.Audio.AudioDistanceModel 25f67b45512d7cd3cff07f6b70d6e8925bdf5ad37557051d60ef09ef300dc6fe -->
<!-- api: F:Inno.Audio.AudioDistanceModel.None ccb5c97190dbc8d4ac629c72e42b6272cadd435fd07bd1f2401979c19171d90d -->
<!-- api: F:Inno.Audio.AudioDistanceModel.Inverse e903b2d194aa3279483ffaf7113326d7844dde6019805b4cb8b0bf8c28c0945f -->
<!-- api: F:Inno.Audio.AudioDistanceModel.Linear ebbadbb9ed208f80f02dc3dd9c1c87b1a9806f39a752cbdec531f24b7dca1c54 -->
<!-- api: F:Inno.Audio.AudioDistanceModel.Exponential a06ab3ce1702cafc7433724a36014c81cc687ee86a84beb74f03483737a8c99b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7ac2cc77fe444d0c"></a>[`Inno.Audio.AudioDistanceModel.Exponential = 3`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L148) | Uses exponential-distance attenuation. | — | — |
| <a id="api-cc000fe26e21afbe"></a>[`Inno.Audio.AudioDistanceModel.Inverse = 1`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L138) | Uses inverse-distance attenuation. | — | — |
| <a id="api-c9003ff4f8c169f6"></a>[`Inno.Audio.AudioDistanceModel.Linear = 2`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L143) | Uses linear-distance attenuation. | — | — |
| <a id="api-2b8f9cc507dc94f2"></a>[`Inno.Audio.AudioDistanceModel.None = 0`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L133) | Disables distance attenuation while retaining spatial direction. | — | — |
| <a id="api-fc72d5615a3debf5"></a>[`public enum Inno.Audio.AudioDistanceModel : int`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L128) | Selects the distance attenuation model for one spatial voice. | — | — |

### `Inno.Audio.AudioEmitterSnapshot`

<!-- api: T:Inno.Audio.AudioEmitterSnapshot ba7c75c14dc19920c0cced8a4bace95a6846d6eaf3eb235e8b62fdb16794d57f -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.#ctor(System.Guid,Inno.Audio.AudioClipAsset,Inno.Audio.AudioPlayOptions,System.Boolean,System.UInt64) 7e276c04ca0a631330991fb74b7c924039a17c9453b1558f46284482af86a515 -->
<!-- api: P:Inno.Audio.AudioEmitterSnapshot.id c4c6565e610c7a7bac99f9320c2f6ec177d260cd3f653820e61bf50a74731574 -->
<!-- api: P:Inno.Audio.AudioEmitterSnapshot.clip 98d13904bc66a4bd1c7871d8e5669ce564255272650a23e5074c9216182a3a14 -->
<!-- api: P:Inno.Audio.AudioEmitterSnapshot.options 918f1dafcde5981a6b57efffc1912633f26a3f467ea216c84b9faf2aff4a5a27 -->
<!-- api: P:Inno.Audio.AudioEmitterSnapshot.shouldPlay 531a4323be1e4a8485e2bb16bdb094bb5b31f09376b3d832624d6a486bde3ed6 -->
<!-- api: P:Inno.Audio.AudioEmitterSnapshot.playbackRevision 8714dda52b8580804acde7c0449c02809e0567c6331e7fa50284af0b5252e66b -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.ToString 48c20f4a0360f13f01884d431752a8c331e372ebef9fb91f3f2cbf7d4c4ede93 -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.op_Inequality(Inno.Audio.AudioEmitterSnapshot,Inno.Audio.AudioEmitterSnapshot) bb6822f08100e2d3866ec756ffd1168b563b097f766dde13186cd41982a26b42 -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.op_Equality(Inno.Audio.AudioEmitterSnapshot,Inno.Audio.AudioEmitterSnapshot) d7c8cd37aaf9a2fd5dcf42d42f097fcbd026fd27303aa61003ae244d9eb0118a -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.GetHashCode bc24aeb517aae5bb13cb70337d1094f00f718b3864d9fdeee57e9281eda0dae7 -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.Equals(System.Object) 12fd0434607dad94765de20759801dcde110c1e1a98797d1a400004c2c0d7fff -->
<!-- api: M:Inno.Audio.AudioEmitterSnapshot.Equals(Inno.Audio.AudioEmitterSnapshot) d8492fabc1777f2e241e3b729f8cae0769a09cc029b125a4f9b54b849226e3bd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0526ee28b075436d"></a>[`public Inno.Audio.AudioEmitterSnapshot.AudioEmitterSnapshot(System.Guid id, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options, bool shouldPlay, ulong playbackRevision = 0)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L28) | Creates an immutable emitter snapshot.<br>`id`：Stable emitter identity across updates.<br>`clip`：Imported clip requested by the emitter.<br>`options`：Current playback and spatial parameters.<br>`shouldPlay`：Whether the runtime should retain active playback for this emitter.<br>`playbackRevision`：Monotonic source-owned revision used to request a fresh voice for the same emitter. | — | — |
| <a id="api-a6e3f5e9e63c910c"></a>[`public bool Inno.Audio.AudioEmitterSnapshot.Equals(Inno.Audio.AudioEmitterSnapshot other)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-be88efb2ff52cc04"></a>[`public override bool Inno.Audio.AudioEmitterSnapshot.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c8cbe1c1ba3983d7"></a>[`public override int Inno.Audio.AudioEmitterSnapshot.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-736067c5c30a11a1"></a>[`public static bool Inno.Audio.AudioEmitterSnapshot.operator ==(Inno.Audio.AudioEmitterSnapshot left, Inno.Audio.AudioEmitterSnapshot right)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2b44d8b2c79e0ea8"></a>[`public static bool Inno.Audio.AudioEmitterSnapshot.operator !=(Inno.Audio.AudioEmitterSnapshot left, Inno.Audio.AudioEmitterSnapshot right)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3c8e3557fbc82882"></a>[`public override string Inno.Audio.AudioEmitterSnapshot.ToString()`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-204af293cc91cebb"></a>[`public Inno.Audio.AudioClipAsset Inno.Audio.AudioEmitterSnapshot.clip { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L52) | Gets the imported clip requested by the emitter. | — | — |
| <a id="api-2ffed87d8e37344c"></a>[`public System.Guid Inno.Audio.AudioEmitterSnapshot.id { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L47) | Gets the stable emitter identity. | — | — |
| <a id="api-2d8bdef20b33aa19"></a>[`public Inno.Audio.AudioPlayOptions Inno.Audio.AudioEmitterSnapshot.options { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L57) | Gets current playback and spatial parameters. | — | — |
| <a id="api-459c183fcd840134"></a>[`public ulong Inno.Audio.AudioEmitterSnapshot.playbackRevision { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L67) | Gets the source-owned revision that distinguishes explicit replay requests. | — | — |
| <a id="api-c74fd04eebbc331a"></a>[`public bool Inno.Audio.AudioEmitterSnapshot.shouldPlay { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L62) | Gets whether the runtime should retain active playback for this emitter. | — | — |
| <a id="api-16062f4b2f6f3410"></a>[`public readonly record struct Inno.Audio.AudioEmitterSnapshot : System.IEquatable<Inno.Audio.AudioEmitterSnapshot>`](../../src/services/audio/Inno.Audio/AudioContent.cs#L8) | Describes one immutable source playback request produced by a content provider. | — | — |

### `Inno.Audio.AudioExecutionContext`

<!-- api: T:Inno.Audio.AudioExecutionContext 5985ce9c08e5dfb81d8e671ee1d3c25f69827ec34e98740a67e4812fb283eac5 -->
<!-- api: P:Inno.Audio.AudioExecutionContext.current 48394161decdada4a7b14f5b0a00acd51052fdc5800477fd1916e5206f292ef4 -->
<!-- api: M:Inno.Audio.AudioExecutionContext.EnterScope(Inno.Audio.IAudioService) a4e4f55cf724ee74e93ce5c64f1cd889e010dee5157c3c486bd8f7864afc37b8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bed98f3ed33e5a57"></a>[`public static System.IDisposable Inno.Audio.AudioExecutionContext.EnterScope(Inno.Audio.IAudioService audio)`](../../src/services/audio/Inno.Audio/Audio.cs#L35) | Binds an audio service until the returned strict last-in-first-out scope is disposed.<br>返回：A strict last-in-first-out scope owned by the caller.<br>`audio`：Host-owned audio service exposed to script-facing operations. | — | `System.ArgumentNullException`：Thrown when audio is null. |
| <a id="api-4b352fd34c537861"></a>[`public static Inno.Audio.IAudioService Inno.Audio.AudioExecutionContext.current { get; }`](../../src/services/audio/Inno.Audio/Audio.cs#L21) | Gets the audio service bound to the current asynchronous execution context. | — | `System.InvalidOperationException`：Thrown when no audio service is active for the caller. |
| <a id="api-1101111a1d7eda35"></a>[`public static class Inno.Audio.AudioExecutionContext`](../../src/services/audio/Inno.Audio/Audio.cs#L11) | Binds one host-owned audio service to the current asynchronous execution context. | — | — |

### `Inno.Audio.AudioListenerHandle`

<!-- api: T:Inno.Audio.AudioListenerHandle d1f9b6bdbe62e15ab3fe6fd04506cf5007622e6d4359ad738bbfa5e90cb4225f -->
<!-- api: P:Inno.Audio.AudioListenerHandle.deviceGeneration 612b986cab75e691b65ff6b756ea84fcb447b9476f0b5924e09c557edae6fc78 -->
<!-- api: P:Inno.Audio.AudioListenerHandle.isValid 98c06f62c8f1cb2d9ecdbb71a78d899871bbed2b06b953e1481f6af0e6174825 -->
<!-- api: M:Inno.Audio.AudioListenerHandle.ToString 5744fd9488bc47394cc097f13543f26aeaf2619d18cef17356c929d9ca64c1ad -->
<!-- api: M:Inno.Audio.AudioListenerHandle.op_Inequality(Inno.Audio.AudioListenerHandle,Inno.Audio.AudioListenerHandle) d57cd5d217fac070e81574e064be28efceacd79c356071f5e32a315c8b77870a -->
<!-- api: M:Inno.Audio.AudioListenerHandle.op_Equality(Inno.Audio.AudioListenerHandle,Inno.Audio.AudioListenerHandle) 14246ba1ed926f60934c4240394f8618ae3adea0fc1ad3def5e8417f30092539 -->
<!-- api: M:Inno.Audio.AudioListenerHandle.GetHashCode 2ebc91d42f3a10368afe43563e58994661c9d4d90c1dec8b92cfb57740f04ca8 -->
<!-- api: M:Inno.Audio.AudioListenerHandle.Equals(System.Object) 5600a950dcf4b760f190492f97f61d17f1d290e432dc0e4fa70dd0cb993f8622 -->
<!-- api: M:Inno.Audio.AudioListenerHandle.Equals(Inno.Audio.AudioListenerHandle) fd938bee81da91fd97c67850ca370d73aa09146474b8f6ddb095ca59240ba303 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-26b0914356ad007b"></a>[`public bool Inno.Audio.AudioListenerHandle.Equals(Inno.Audio.AudioListenerHandle other)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2602b9652badb41d"></a>[`public override bool Inno.Audio.AudioListenerHandle.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-92a7ee34df7b8754"></a>[`public override int Inno.Audio.AudioListenerHandle.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2278eb34ed539aa2"></a>[`public static bool Inno.Audio.AudioListenerHandle.operator ==(Inno.Audio.AudioListenerHandle left, Inno.Audio.AudioListenerHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-003bfc2676df3b2f"></a>[`public static bool Inno.Audio.AudioListenerHandle.operator !=(Inno.Audio.AudioListenerHandle left, Inno.Audio.AudioListenerHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f43b4329a408dd32"></a>[`public override string Inno.Audio.AudioListenerHandle.ToString()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-18066be58ae4018b"></a>[`public uint Inno.Audio.AudioListenerHandle.deviceGeneration { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L99) | Gets the generation of the device that created this handle. | — | — |
| <a id="api-a895651dfb910acc"></a>[`public bool Inno.Audio.AudioListenerHandle.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L104) | Gets whether the handle contains a non-zero backend identity and generation. | — | — |
| <a id="api-bd08ac5ffce05542"></a>[`public readonly record struct Inno.Audio.AudioListenerHandle : System.IEquatable<Inno.Audio.AudioListenerHandle>`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L84) | Identifies one spatial listener within a device generation. | — | — |

### `Inno.Audio.AudioListenerSnapshot`

<!-- api: T:Inno.Audio.AudioListenerSnapshot 04fbf71d0c38f9b5d164e163fd80c88139e57b4bc15cef611a1424ebd28edfa8 -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.#ctor(System.Guid,System.Int32,Inno.Audio.AudioListenerState,System.Boolean) e40389e05e72da7915f6be030700b869cf02f98e443f0962502bb4fb7342c45b -->
<!-- api: P:Inno.Audio.AudioListenerSnapshot.id 3f3490d0a2a6cde371aafed8a7b3e86a895cd92591d18ba8b11096d08f87a49f -->
<!-- api: P:Inno.Audio.AudioListenerSnapshot.priority 24e75a5dc881ec1a486d505dcbc3e4211a5afd4c036c29fd68d59366a2c86e2c -->
<!-- api: P:Inno.Audio.AudioListenerSnapshot.state a931dcd075d6322dc01948c2130f4412615cf642a21362d10f71fc7bd6c0ef2e -->
<!-- api: P:Inno.Audio.AudioListenerSnapshot.active 79e39b51f4b8c99686fb2e4f42edffcd3521871a22486d67291d0bcad34451a3 -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.ToString 4c193e55fb8ae47b4f1c2a49afe76a77dfaf2a112dd9f6eaab87b549cf2c7c42 -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.op_Inequality(Inno.Audio.AudioListenerSnapshot,Inno.Audio.AudioListenerSnapshot) c8adb9b3b03f2dc799ce274d82ceb19d0c9285dd51863eb4401420469e6fc77c -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.op_Equality(Inno.Audio.AudioListenerSnapshot,Inno.Audio.AudioListenerSnapshot) e8856a9e54cc3983e4349409b214069fec647934511aea9812fc9d68972e8c69 -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.GetHashCode 282dd868c5b617164e40f7e19afc6e94ab987127815e97e53b3be7feeb39761f -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.Equals(System.Object) fd8cc8b82d25d8f10b8cbe55b00ba37eef3678718bc3cfa3e406ffc9c64eddf1 -->
<!-- api: M:Inno.Audio.AudioListenerSnapshot.Equals(Inno.Audio.AudioListenerSnapshot) 586bf8e9728a646ae32f106eb99c828e7630e86263ba09aa568adf1c4b6393af -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2b868136c753b37c"></a>[`public Inno.Audio.AudioListenerSnapshot.AudioListenerSnapshot(System.Guid id, int priority, Inno.Audio.AudioListenerState state, bool active)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L90) | Creates an immutable listener snapshot.<br>`id`：Stable listener identity across updates.<br>`priority`：Selection priority; larger values win.<br>`state`：Current listener transform.<br>`active`：Whether the listener is eligible for selection. | — | — |
| <a id="api-3410f54bbaf715a1"></a>[`public bool Inno.Audio.AudioListenerSnapshot.Equals(Inno.Audio.AudioListenerSnapshot other)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-509f2a959082bc4e"></a>[`public override bool Inno.Audio.AudioListenerSnapshot.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-54fba429d53c1401"></a>[`public override int Inno.Audio.AudioListenerSnapshot.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-9aec063f3f5de4bc"></a>[`public static bool Inno.Audio.AudioListenerSnapshot.operator ==(Inno.Audio.AudioListenerSnapshot left, Inno.Audio.AudioListenerSnapshot right)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f453bfb5102661aa"></a>[`public static bool Inno.Audio.AudioListenerSnapshot.operator !=(Inno.Audio.AudioListenerSnapshot left, Inno.Audio.AudioListenerSnapshot right)`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-26c159e0545bba2c"></a>[`public override string Inno.Audio.AudioListenerSnapshot.ToString()`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6fcaff1e267e0683"></a>[`public bool Inno.Audio.AudioListenerSnapshot.active { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L122) | Gets whether the listener is eligible for selection. | — | — |
| <a id="api-ab3774704a986e06"></a>[`public System.Guid Inno.Audio.AudioListenerSnapshot.id { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L107) | Gets the stable listener identity. | — | — |
| <a id="api-04df0824fc1b3c1b"></a>[`public int Inno.Audio.AudioListenerSnapshot.priority { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L112) | Gets the selection priority. | — | — |
| <a id="api-dcb580f7ef5c3655"></a>[`public Inno.Audio.AudioListenerState Inno.Audio.AudioListenerSnapshot.state { get; }`](../../src/services/audio/Inno.Audio/AudioContent.cs#L117) | Gets the current listener transform. | — | — |
| <a id="api-c43c3cbcdabcce89"></a>[`public readonly record struct Inno.Audio.AudioListenerSnapshot : System.IEquatable<Inno.Audio.AudioListenerSnapshot>`](../../src/services/audio/Inno.Audio/AudioContent.cs#L73) | Describes one immutable listener candidate produced by a content provider. | — | — |

### `Inno.Audio.AudioListenerState`

<!-- api: T:Inno.Audio.AudioListenerState 1c42b3ef134343abb1ecf5ad49547366f3a0c8bc736a3a8168a852d5422cac39 -->
<!-- api: M:Inno.Audio.AudioListenerState.#ctor(Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3) 45ac363e69b2a98bb44aa9117b64ab8e0ecf887eb3a52ef6bfd8269a7dcc8550 -->
<!-- api: P:Inno.Audio.AudioListenerState.position c095a49ce410342a6d43259ce97e14a30260dd84b46d5a21e6ba1fb19861c267 -->
<!-- api: P:Inno.Audio.AudioListenerState.direction 23c2f31b5f1a53666c87eafc24d80f7003f579c7eba1b5e03ba6ab14ec2371c8 -->
<!-- api: P:Inno.Audio.AudioListenerState.up 4625de391e1766ee8dd698d2eb2583deef8280c16e28aa09f4c81756c14300b7 -->
<!-- api: P:Inno.Audio.AudioListenerState.velocity e8ea8fdebbc696748a8b3324949210bd488f633fbaabca1b9432308ed6e1ab95 -->
<!-- api: M:Inno.Audio.AudioListenerState.ToString a311957a8e892703483d7305343239635e05bf4b785df86a45410673465377f4 -->
<!-- api: M:Inno.Audio.AudioListenerState.op_Inequality(Inno.Audio.AudioListenerState,Inno.Audio.AudioListenerState) 03e847e9576a7e27871e3697f845c57834a215c963bdee0fb1b93865bd17fd4a -->
<!-- api: M:Inno.Audio.AudioListenerState.op_Equality(Inno.Audio.AudioListenerState,Inno.Audio.AudioListenerState) 376ca9452ffc05a72eefa922c17c5beb681c45b9cf7bfbdeb75ba280ab6be12f -->
<!-- api: M:Inno.Audio.AudioListenerState.GetHashCode c1c2ba65b080b6b1aba02f1c9b996be50c63e21f9b7adc8ba37e8446c99b5356 -->
<!-- api: M:Inno.Audio.AudioListenerState.Equals(System.Object) 20c2704b9eec3c445223300e49c04d60d5da86efd43b2f1b29a46f93fad15d8a -->
<!-- api: M:Inno.Audio.AudioListenerState.Equals(Inno.Audio.AudioListenerState) 561b4e2fe3224c2c0dfb7bb354bbea497f420b452853c23bc25c0efc8128fa8b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-726590198d91bb79"></a>[`public Inno.Audio.AudioListenerState.AudioListenerState(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 direction, Inno.Core.Mathematics.Vector3 up, Inno.Core.Mathematics.Vector3 velocity)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L623) | Creates a listener transform snapshot.<br>`position`：World-space listener position.<br>`direction`：World-space listener forward direction.<br>`up`：World-space listener up direction.<br>`velocity`：World-space listener velocity. | — | `System.ArgumentOutOfRangeException`：Any position, direction, up, or velocity component is non-finite. |
| <a id="api-c530d32a22a4af61"></a>[`public bool Inno.Audio.AudioListenerState.Equals(Inno.Audio.AudioListenerState other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-aab83a893e1b8f9c"></a>[`public override bool Inno.Audio.AudioListenerState.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-d4c8e47dde41066b"></a>[`public override int Inno.Audio.AudioListenerState.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-7a1ee8c101f6acbf"></a>[`public static bool Inno.Audio.AudioListenerState.operator ==(Inno.Audio.AudioListenerState left, Inno.Audio.AudioListenerState right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2f92c22b3ef1fa69"></a>[`public static bool Inno.Audio.AudioListenerState.operator !=(Inno.Audio.AudioListenerState left, Inno.Audio.AudioListenerState right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6140dfb863a2d8f4"></a>[`public override string Inno.Audio.AudioListenerState.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ccc19d0774a16d56"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.direction { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L647) | Gets the world-space listener forward direction. | — | — |
| <a id="api-ef04eab63a401cec"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.position { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L642) | Gets the world-space listener position. | — | — |
| <a id="api-726a9bc97a8354ba"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.up { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L652) | Gets the world-space listener up direction. | — | — |
| <a id="api-3a856ee30cb526f8"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioListenerState.velocity { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L657) | Gets the world-space listener velocity. | — | — |
| <a id="api-37b25b2581b875d3"></a>[`public readonly record struct Inno.Audio.AudioListenerState : System.IEquatable<Inno.Audio.AudioListenerState>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L603) | Contains one listener transform used by backend spatialization. | — | — |

### `Inno.Audio.AudioMixer`

<!-- api: T:Inno.Audio.AudioMixer 92855b5b64997e9e1fed179e2edc4a50d47d62387426c7fe649cb10eaada05e0 -->
<!-- api: P:Inno.Audio.AudioMixer.buses 39056326cfc3ef1977136903168d94945741de96924aeeb58e89e097c70fb500 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-35124c322457cf72"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioBusDefinition> Inno.Audio.AudioMixer.buses { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L172) | Gets buses in parent-before-child creation order. | — | — |
| <a id="api-29bf05e26f89624c"></a>[`public sealed class Inno.Audio.AudioMixer`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L160) | Contains one validated backend-neutral mixer graph. | — | — |

### `Inno.Audio.AudioMixerAsset`

<!-- api: T:Inno.Audio.AudioMixerAsset e66e68265a006c1272a159b19832040cc10ac05962f152e72b04a3828cb7c147 -->
<!-- api: P:Inno.Audio.AudioMixerAsset.mixerTypeId 653f7c57668dbb42493a759679200ecfc79ac3d66f40dda976cb29fd22a35bc8 -->
<!-- api: P:Inno.Audio.AudioMixerAsset.mixerState 6cfe1a441d17d7fa435b99073816a44b269aa20305520341a3132ade458d0ca9 -->
<!-- api: P:Inno.Audio.AudioMixerAsset.features bc70d9c92b493fe77c6918cebacef73f873d52b37642962b8272a186b1c4658e -->
<!-- api: M:Inno.Audio.AudioMixerAsset.#ctor 8d59a751fe89db88ae29b3f5c2092ce80c940d205bc2e731dc167b3eb7614a72 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0b66cb1e6cdf8c7f"></a>[`public Inno.Audio.AudioMixerAsset.AudioMixerAsset()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L384) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9833d4c950eac34e"></a>[`public Inno.Audio.AudioMixerFeatureConfiguration[] Inno.Audio.AudioMixerAsset.features { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L404) | Gets or sets ordered feature configurations. | — | — |
| <a id="api-e601be75dfd0d033"></a>[`public Inno.Audio.SerializedAudioExtensionState Inno.Audio.AudioMixerAsset.mixerState { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L398) | Gets or sets reload-safe mixer settings. | — | — |
| <a id="api-1dbc1b1e54627948"></a>[`public string Inno.Audio.AudioMixerAsset.mixerTypeId { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L392) | Gets or sets the globally stable mixer extension identifier. | — | — |
| <a id="api-845bd21c33abb364"></a>[`public sealed class Inno.Audio.AudioMixerAsset : Inno.Assets.AssetObject`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L384) | Selects a mixer extension and ordered features without defining a game-specific mixing model. | — | — |

### `Inno.Audio.AudioMixerBuilder`

<!-- api: T:Inno.Audio.AudioMixerBuilder 6959cce87860311c0f31ad720d668362bbf146d2d553f2ae228120527e134e46 -->
<!-- api: M:Inno.Audio.AudioMixerBuilder.AddBus(Inno.Audio.AudioBusId,Inno.Audio.AudioBusId,System.Single,System.Boolean) 511c9dce66cec902fa59aad7cd41df31a367d7979ea4e2b3446a15761c1d7a64 -->
<!-- api: M:Inno.Audio.AudioMixerBuilder.AddProcessor(Inno.Audio.AudioBusId,Inno.Audio.AudioProcessorConfiguration) 2f26c209856db99e893bb088ac1a98b89c9e975312e519ddc494854c85772847 -->
<!-- api: M:Inno.Audio.AudioMixerBuilder.Build 21794cf41399ae44bce3ad99642766216623608ec4cdb67eeb87ead159b570ae -->
<!-- api: M:Inno.Audio.AudioMixerBuilder.#ctor c61450781ca23eab1eefe32ce57a941c8afd227baaa49dfd8d69e19a038573cb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cb1a6fb5cc692579"></a>[`public Inno.Audio.AudioMixerBuilder.AudioMixerBuilder()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L178) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-0cbf3bb5f394d284"></a>[`public void Inno.Audio.AudioMixerBuilder.AddBus(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusId parent, float volume = 1, bool muted = false)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L203) | Adds one semantic bus to the graph.<br>`id`：Stable bus identifier.<br>`parent`：Parent bus identifier.<br>`volume`：Initial non-negative linear gain.<br>`muted`：Whether output is initially silenced. | — | `System.ArgumentException`：Thrown when the bus identifier is invalid or duplicated. |
| <a id="api-821a4ad7990ab117"></a>[`public void Inno.Audio.AudioMixerBuilder.AddProcessor(Inno.Audio.AudioBusId bus, Inno.Audio.AudioProcessorConfiguration processor)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L228) | Appends one processor to an existing bus chain.<br>`bus`：Existing bus identifier.<br>`processor`：Immutable processor configuration to append. | — | `System.Collections.Generic.KeyNotFoundException`：Thrown when the bus is not defined. |
| <a id="api-d30c0ab35bcb7dc9"></a>[`public Inno.Audio.AudioMixer Inno.Audio.AudioMixerBuilder.Build()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L252) | Validates the graph and returns an immutable parent-before-child snapshot.<br>返回：A validated mixer graph containing the mandatory master bus. | — | `System.InvalidOperationException`：Thrown when a parent is missing or bus routing contains a cycle. |
| <a id="api-781cf548a5529ceb"></a>[`public sealed class Inno.Audio.AudioMixerBuilder`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L178) | Builds and validates an open backend-neutral mixer graph. | — | — |

### `Inno.Audio.AudioMixerExtension`

<!-- api: T:Inno.Audio.AudioMixerExtension f5f48962af1ea53f5a6b5ddeac5b29438478b9f657dff463f8c666568d78d299 -->
<!-- api: M:Inno.Audio.AudioMixerExtension.Build(Inno.Audio.AudioMixerBuilder,Inno.Audio.SerializedAudioExtensionState) ca060f1b7de16f4ceb564a904ac1b20ec5e74040e8690cde9291fe5384eed495 -->
<!-- api: M:Inno.Audio.AudioMixerExtension.#ctor b023e67d66fd8e22ae4577f11cb0924ae0d954e9594df3e788be1522419764b9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e02227236760165d"></a>[`protected Inno.Audio.AudioMixerExtension.AudioMixerExtension()`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L67) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ad1ac2cabe87d645"></a>[`public abstract void Inno.Audio.AudioMixerExtension.Build(Inno.Audio.AudioMixerBuilder builder, Inno.Audio.SerializedAudioExtensionState state)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L78) | Adds base buses and processors to a candidate mixer graph.<br>`builder`：Candidate graph builder valid only for this invocation.<br>`state`：Reload-safe neutral extension state. | — | — |
| <a id="api-1040644af5a3212e"></a>[`public abstract class Inno.Audio.AudioMixerExtension`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L67) | Builds a base mixer graph on the host control thread. | — | — |

### `Inno.Audio.AudioMixerExtensionAttribute`

<!-- api: T:Inno.Audio.AudioMixerExtensionAttribute ce84b6ce7091577b41bc12471c8cb32b64d3fcbde4314d9cc354378f628a989f -->
<!-- api: M:Inno.Audio.AudioMixerExtensionAttribute.#ctor(System.String) e9ad6242dca154dc5f2deb20ab8cf5b2f53e6b762d653a352bff523144aedf5b -->
<!-- api: P:Inno.Audio.AudioMixerExtensionAttribute.id b77e7cd9ff77728ff7fb6e57b6652d640b28224d5d467d14fcee8f7753fdbd98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f2c1b6d1fd9e452"></a>[`public Inno.Audio.AudioMixerExtensionAttribute.AudioMixerExtensionAttribute(string id)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L17) | Creates a mixer extension declaration.<br>`id`：Globally stable mixer extension identifier. | — | — |
| <a id="api-4dbd3f861b2efe7c"></a>[`public string Inno.Audio.AudioMixerExtensionAttribute.id { get; }`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L26) | Gets the globally stable mixer extension identifier. | — | — |
| <a id="api-f2526b0918929a7a"></a>[`public sealed class Inno.Audio.AudioMixerExtensionAttribute : System.Attribute`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L9) | Marks a reloadable extension that creates the base audio mixer graph. | — | — |

### `Inno.Audio.AudioMixerFeature`

<!-- api: T:Inno.Audio.AudioMixerFeature d4c72c92845f2259e91aa93d39395d412da82709cb68474855fd75923fef01a2 -->
<!-- api: M:Inno.Audio.AudioMixerFeature.Build(Inno.Audio.AudioMixerBuilder,Inno.Audio.SerializedAudioExtensionState) 5bf2827a4071d3b4d566775d0b9db92ac70fb9c64ce139b5202fd52db4cad2bd -->
<!-- api: M:Inno.Audio.AudioMixerFeature.#ctor 586c09b2dc78336b5e9140734adbbabc6be3aff7acfbf327ec487a53c977627f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-15b3bcc65ea42241"></a>[`protected Inno.Audio.AudioMixerFeature.AudioMixerFeature()`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L87) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-453d668eb17bc417"></a>[`public abstract void Inno.Audio.AudioMixerFeature.Build(Inno.Audio.AudioMixerBuilder builder, Inno.Audio.SerializedAudioExtensionState state)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L98) | Adds feature-owned graph declarations on the host control thread.<br>`builder`：Candidate graph builder valid only for this invocation.<br>`state`：Reload-safe neutral feature state. | — | — |
| <a id="api-9203a3a141b34c50"></a>[`public abstract class Inno.Audio.AudioMixerFeature`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L87) | Contributes optional buses and processors to a candidate mixer graph. | — | — |

### `Inno.Audio.AudioMixerFeatureConfiguration`

<!-- api: T:Inno.Audio.AudioMixerFeatureConfiguration 116a5120452da82f418484bc2064b7f2335d2be7eeb342acf52e551566c74d59 -->
<!-- api: M:Inno.Audio.AudioMixerFeatureConfiguration.#ctor 05d7fdab9af8087c93d7460a0eea3a1bf4ef29d9a40ad26b5dfe2118839c7246 -->
<!-- api: M:Inno.Audio.AudioMixerFeatureConfiguration.#ctor(System.String,System.Nullable{Inno.Audio.SerializedAudioExtensionState},System.Boolean) 3600a20494b462bea7f1c9854ddefb25ed5bef484929f7c23b2958210ec86a31 -->
<!-- api: P:Inno.Audio.AudioMixerFeatureConfiguration.featureTypeId c19d6ad91b34a3145fd451033e5fd610c715b21bd50b2b3df528a6a7d4c8d6c7 -->
<!-- api: P:Inno.Audio.AudioMixerFeatureConfiguration.state 15c1aba1f185f6cd1612e9b31963bf0ae1d22857deac63aa108c73aa68a3847c -->
<!-- api: P:Inno.Audio.AudioMixerFeatureConfiguration.enabled f8f0966c80ab8e45fa1d39bc1828ebc1268bdd6bc870d1e98f53f7f501ab060e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-142ced871098d981"></a>[`public Inno.Audio.AudioMixerFeatureConfiguration.AudioMixerFeatureConfiguration()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L334) | Creates an empty feature configuration for deserialization. | — | — |
| <a id="api-29ab191b5651ece4"></a>[`public Inno.Audio.AudioMixerFeatureConfiguration.AudioMixerFeatureConfiguration(string featureTypeId, Inno.Audio.SerializedAudioExtensionState? state = null, bool enabled = true)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L350) | Creates a mixer feature configuration.<br>`featureTypeId`：Globally stable feature extension identifier.<br>`state`：Optional reload-safe feature state.<br>`enabled`：Whether the feature participates in graph construction. | — | — |
| <a id="api-d048bc1090ce7531"></a>[`public bool Inno.Audio.AudioMixerFeatureConfiguration.enabled { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L377) | Gets or sets whether the feature participates in graph construction. | — | — |
| <a id="api-6e1cca82620e8c5a"></a>[`public string Inno.Audio.AudioMixerFeatureConfiguration.featureTypeId { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L365) | Gets or sets the stable feature extension identifier. | — | — |
| <a id="api-588d94085bec6dfa"></a>[`public Inno.Audio.SerializedAudioExtensionState Inno.Audio.AudioMixerFeatureConfiguration.state { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L371) | Gets or sets reload-safe feature settings. | — | — |
| <a id="api-2e791651a70287c6"></a>[`public struct Inno.Audio.AudioMixerFeatureConfiguration`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L329) | Stores one ordered mixer feature selection using only stable data. | — | — |

### `Inno.Audio.AudioMixerFeatureExtensionAttribute`

<!-- api: T:Inno.Audio.AudioMixerFeatureExtensionAttribute 373552fd13520e7d5f77d12e5a7d6ac0ef1c8881605a272426e26b6c17cf6119 -->
<!-- api: M:Inno.Audio.AudioMixerFeatureExtensionAttribute.#ctor(System.String,System.Int32) d0bbe2833e9661f6b466b69545892fd069f6a8df0852686533dd332595f76cdb -->
<!-- api: P:Inno.Audio.AudioMixerFeatureExtensionAttribute.id fd7ae0a5fadc6b73acf2a645828650bd96250d6224d688cda3a44d1b473b194b -->
<!-- api: P:Inno.Audio.AudioMixerFeatureExtensionAttribute.priority af1d0901fc02380a2dd4cf099dbf998749ee824e9fc6e66052bbf68e8e99e9b4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f74fbc52aef7ef77"></a>[`public Inno.Audio.AudioMixerFeatureExtensionAttribute.AudioMixerFeatureExtensionAttribute(string id, int priority = 0)`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L44) | Creates a mixer feature declaration.<br>`id`：Globally stable feature extension identifier.<br>`priority`：Feature invocation priority; lower values build first. | — | — |
| <a id="api-61d5a70f535e66f1"></a>[`public string Inno.Audio.AudioMixerFeatureExtensionAttribute.id { get; }`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L56) | Gets the globally stable feature extension identifier. | — | — |
| <a id="api-c5ce71eb34a617af"></a>[`public int Inno.Audio.AudioMixerFeatureExtensionAttribute.priority { get; }`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L61) | Gets the feature invocation priority. | — | — |
| <a id="api-a78bac902fbea065"></a>[`public sealed class Inno.Audio.AudioMixerFeatureExtensionAttribute : System.Attribute`](../../src/services/audio/Inno.Audio/AudioMixerExtensions.cs#L33) | Marks a reloadable extension that contributes to an audio mixer graph. | — | — |

### `Inno.Audio.AudioParameterId`

<!-- api: T:Inno.Audio.AudioParameterId 63dd5fdb84bfc3708372647acf0ff380da9714dcd2c7c7ded714ea4df103f388 -->
<!-- api: M:Inno.Audio.AudioParameterId.#ctor(System.String) 01bc9182eb096cbfcb7b5e613717c1247aa0ea70e580b79e049801a49492ed84 -->
<!-- api: P:Inno.Audio.AudioParameterId.frequency a43358c5d8127b265d3b82072982739a9dc6543fab698a95cf05a7afc1150fcd -->
<!-- api: P:Inno.Audio.AudioParameterId.quality bba2690d3cd30e04ed1b66af41d9f0ab3b1b5a2368cea0f24420f4878a07fb5d -->
<!-- api: P:Inno.Audio.AudioParameterId.gainDecibels cb29035c9b7e8b205e38b2d7c0d51875fc44e10a3ff34858e3a325ddc4471dbd -->
<!-- api: P:Inno.Audio.AudioParameterId.shelfSlope cd6d8d645b2380e9829d5ea5bd8b81c92b788ec8409cdae17cdccef69d962d5d -->
<!-- api: P:Inno.Audio.AudioParameterId.delayMilliseconds cdd830b2660f185ffd2ebafc4ae838169f4a163c62673076a92f9d11ebd0a8a9 -->
<!-- api: P:Inno.Audio.AudioParameterId.decay 04fe22e5999d50feadc08fbb2f6914263c1f5e1020d8fd6fefbb1db056c699d5 -->
<!-- api: P:Inno.Audio.AudioParameterId.value 3636611e03a57f97594455bc20ec472a3bdbbf00d75cf84ff31f6d8ce69d54b7 -->
<!-- api: P:Inno.Audio.AudioParameterId.isValid 551caca74844e7c5e6da604ae7ba63a9ed440ec6cf72ae86c4c0a184279d1c7c -->
<!-- api: M:Inno.Audio.AudioParameterId.ToString e2439645abe7dc8a6923486fbc1d5856cd8681783d65176a49455eb505710d61 -->
<!-- api: M:Inno.Audio.AudioParameterId.op_Inequality(Inno.Audio.AudioParameterId,Inno.Audio.AudioParameterId) aec293749e326bdbc5cf98f3b7d3adc8e4a0bae65625a6e2744e4873f135b304 -->
<!-- api: M:Inno.Audio.AudioParameterId.op_Equality(Inno.Audio.AudioParameterId,Inno.Audio.AudioParameterId) e39fd09f2489e9f6ba5b9c6280dff83f1ded2241f12030fcbf79f69fe7dfe96c -->
<!-- api: M:Inno.Audio.AudioParameterId.GetHashCode 68b847adf3dfc3c7147a912e2bc14f10ecc9055c3d4df7983d27198bf30267f4 -->
<!-- api: M:Inno.Audio.AudioParameterId.Equals(System.Object) f5e7caf13cd16f661052715ed973614e5a8833163225a8b7d3f4be2c9cb398d5 -->
<!-- api: M:Inno.Audio.AudioParameterId.Equals(Inno.Audio.AudioParameterId) 863590205f7b3e50a8f35d3bb0099302a17e58cb789d87b3ed4c5d323bc6f90e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a2478b7eefa17e06"></a>[`public Inno.Audio.AudioParameterId.AudioParameterId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L133) | Creates an open processor parameter identifier.<br>`value`：Stable parameter name within its processor protocol. | — | — |
| <a id="api-32afb6008606fc5c"></a>[`public bool Inno.Audio.AudioParameterId.Equals(Inno.Audio.AudioParameterId other)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-0a76bfcd7f998679"></a>[`public override bool Inno.Audio.AudioParameterId.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-9168abe7c362095e"></a>[`public override int Inno.Audio.AudioParameterId.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-e0f1bff3c49b68e7"></a>[`public static bool Inno.Audio.AudioParameterId.operator ==(Inno.Audio.AudioParameterId left, Inno.Audio.AudioParameterId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2d6ce6dd3884a04d"></a>[`public static bool Inno.Audio.AudioParameterId.operator !=(Inno.Audio.AudioParameterId left, Inno.Audio.AudioParameterId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-77d36da41a70f706"></a>[`public override string Inno.Audio.AudioParameterId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L185) | Formats the identifier for diagnostics and persistence.<br>返回：The stable protocol value. | — | — |
| <a id="api-6ddecb8d7f1684b3"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.decay { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L167) | Gets the standard delay feedback decay factor. | — | — |
| <a id="api-c2260b95a74a3671"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.delayMilliseconds { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L162) | Gets the standard delay duration parameter in milliseconds. | — | — |
| <a id="api-9988c10129de6cab"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.frequency { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L142) | Gets the standard cutoff or center-frequency parameter in hertz. | — | — |
| <a id="api-d6398f3275f3c1c5"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.gainDecibels { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L152) | Gets the standard equalizer gain parameter in decibels. | — | — |
| <a id="api-8badecf58f440ef1"></a>[`public bool Inno.Audio.AudioParameterId.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L177) | Gets whether this identifier contains a usable protocol value. | — | — |
| <a id="api-b4370bb4eb660c77"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.quality { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L147) | Gets the standard filter quality-factor parameter. | — | — |
| <a id="api-233e0c69b868be5f"></a>[`public static Inno.Audio.AudioParameterId Inno.Audio.AudioParameterId.shelfSlope { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L157) | Gets the standard equalizer shelf-slope parameter. | — | — |
| <a id="api-dd76aa8694669af7"></a>[`public string Inno.Audio.AudioParameterId.value { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L172) | Gets the stable parameter protocol value. | — | — |
| <a id="api-21b5a5f1deff98d6"></a>[`public readonly record struct Inno.Audio.AudioParameterId : System.IEquatable<Inno.Audio.AudioParameterId>`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L125) | Identifies one processor parameter without imposing a processor-specific enum. | — | — |

### `Inno.Audio.AudioPlaybackState`

<!-- api: T:Inno.Audio.AudioPlaybackState 0e3354d9e4f78c0f7ec570e215cc93cf0f2cd7cac14222c66bb9f3229588e51d -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Preparing e18a97c39560e2760d64fadf090cc112e7971acb091d6f5b825bf4540b1657d5 -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Scheduled 3d58a3ddef69ac566310c53bec448d449233235c22278e5f214406f6909e1ce4 -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Playing 865aadeeeb04d190a25c660518ed887697ebb65221fb10b9837361802f113884 -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Paused 87d5f1594d5b35a29eabc9452ba28d75898d902f75637b52248b97aedea5b54d -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Completed 7f5cd141a58c85cff24a65bd6c3a8bfda5e5f88e686283798bb6e2c4fbc92c17 -->
<!-- api: F:Inno.Audio.AudioPlaybackState.Invalid ca6970f3ca94176ba90e10271f1dbed4a8dcf6580de465b3e3f165fd8e840791 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6bb021e331ac5055"></a>[`Inno.Audio.AudioPlaybackState.Completed = 4`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L86) | The voice has completed and no longer accepts control commands. | — | — |
| <a id="api-43f35b931603f3e7"></a>[`Inno.Audio.AudioPlaybackState.Invalid = 5`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L91) | The handle is invalid, stale, or unknown to the active runtime. | — | — |
| <a id="api-0c4e40f7294c4fe6"></a>[`Inno.Audio.AudioPlaybackState.Paused = 3`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L81) | The voice is paused at its current cursor. | — | — |
| <a id="api-0de28cc08ee1de4a"></a>[`Inno.Audio.AudioPlaybackState.Playing = 2`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L76) | The voice is actively advancing. | — | — |
| <a id="api-e42d42feb91cc45b"></a>[`Inno.Audio.AudioPlaybackState.Preparing = 0`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L66) | The voice is waiting for clip preparation. | — | — |
| <a id="api-188d57eef1900cbc"></a>[`Inno.Audio.AudioPlaybackState.Scheduled = 1`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L71) | The voice is scheduled against the audio clock. | — | — |
| <a id="api-c88e00e350d900ec"></a>[`public enum Inno.Audio.AudioPlaybackState : int`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L61) | Describes the observable lifecycle of one playback voice. | — | — |

### `Inno.Audio.AudioPlayOptions`

<!-- api: T:Inno.Audio.AudioPlayOptions 21273873d8afbaac1545203df24b2812ffb2b4564098255b4b8f6ae2849286a0 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.#ctor e003f76e36c18458e313f4b4f2081513d90bcf034bcfc33cd59c70ea38027be5 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.#ctor(System.Single,System.Single,System.Single,System.Boolean,System.Int32,System.Nullable{Inno.Audio.AudioBusId},Inno.Audio.AudioClipLoadMode,System.Nullable{Inno.Audio.AudioSpatialOptions}) 8f40856015e13455848e005e4636500537228f61cb506d3cb77a65cf19c4bf54 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.defaultValue fcd8af45518c44e84e6f65ce297c9ce1d329dcd5a757708f4f48e1b642020ff1 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.volume 2ed132cc305b9461190140ce988aa68e3fb2c147ee5587a75577b0b4e1282aeb -->
<!-- api: P:Inno.Audio.AudioPlayOptions.pitch 0981a8c6b2c0a24a1bb6fe1ba3dd1623dee0204aaf6b0d4f60605eca196afb58 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.pan e09f0f4e3c24bbe7527bbcdb13badeb1f98464f408a88079f4f9e4234a66578e -->
<!-- api: P:Inno.Audio.AudioPlayOptions.loop be64476c963f5cec8d10d74ddb3d6d36b237a7c962b3a77bb19f43326cdef368 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.priority 656cdc98b9f049933ebe571ea328d7735777a63c060a5112549c62124954abfb -->
<!-- api: P:Inno.Audio.AudioPlayOptions.bus 0fea0900f88ec9a73fa9d867a958c5b9157e1dfaf92bfc5f1ae84caf3dacd7b8 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.loadMode fb2746a0ae273a61e3dc82fff43fb67073b6be4fa2bf0a3a4c93093a82ca7c96 -->
<!-- api: P:Inno.Audio.AudioPlayOptions.spatial 6089d66994127e2dc06b267809bac05ebfa76747774f100dfd02df53bc48a292 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.ToString 955d959b2ee16b9d0802b36cb7c4a7080f306362403c8500720fe1bb214352e7 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.op_Inequality(Inno.Audio.AudioPlayOptions,Inno.Audio.AudioPlayOptions) 8ee0ee7d88a40efbdd88ee806ac17383813d68804ea4685cb1533b2787b4e786 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.op_Equality(Inno.Audio.AudioPlayOptions,Inno.Audio.AudioPlayOptions) 7bd23fe07bd687882b45f102c64bc17281d8e8a49b1a9cd0382020c2b80d4d76 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.GetHashCode 803d68d511d0416a4d3b8183937fa70917bacfac71700a25b4ef122c6af9051d -->
<!-- api: M:Inno.Audio.AudioPlayOptions.Equals(System.Object) 239fb827599ebaafb23311b7ef6c0b1b8e86c3961c1891390506e1dc150c5da9 -->
<!-- api: M:Inno.Audio.AudioPlayOptions.Equals(Inno.Audio.AudioPlayOptions) 59882bcee4def73a95d1837a279dde5eddc556e9648f5a80bc2a4eb94ce5a216 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-990b419e16a1779e"></a>[`public Inno.Audio.AudioPlayOptions.AudioPlayOptions()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L423) | Creates default playback parameters. | — | — |
| <a id="api-9e5fbbaebac1c0df"></a>[`public Inno.Audio.AudioPlayOptions.AudioPlayOptions(float volume = 1, float pitch = 1, float pan = 0, bool loop = false, int priority = 0, Inno.Audio.AudioBusId? bus = null, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, Inno.Audio.AudioSpatialOptions? spatial = null)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L461) | Creates playback parameters.<br>`volume`：Linear gain applied to the voice.<br>`pitch`：Playback-rate multiplier.<br>`pan`：Stereo pan from negative one to positive one.<br>`loop`：Whether playback restarts after reaching the clip end.<br>`priority`：Voice retention priority; larger values are retained first.<br>`bus`：Destination mixer bus.<br>`loadMode`：Clip preparation override.<br>`spatial`：Optional spatial playback parameters. | — | `System.ArgumentOutOfRangeException`：Gain, pitch, or pan is non-finite or outside its valid range, or the load mode is undefined.<br>`System.ArgumentException`：The supplied destination bus is uninitialized. |
| <a id="api-7f52c930492ba29b"></a>[`public bool Inno.Audio.AudioPlayOptions.Equals(Inno.Audio.AudioPlayOptions other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2d4885ec4e004d3b"></a>[`public override bool Inno.Audio.AudioPlayOptions.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-db604d7fcf716518"></a>[`public override int Inno.Audio.AudioPlayOptions.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-ad42a2659be05075"></a>[`public static bool Inno.Audio.AudioPlayOptions.operator ==(Inno.Audio.AudioPlayOptions left, Inno.Audio.AudioPlayOptions right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-7bbc531c71b67b42"></a>[`public static bool Inno.Audio.AudioPlayOptions.operator !=(Inno.Audio.AudioPlayOptions left, Inno.Audio.AudioPlayOptions right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cd3a1e40c09f8cf8"></a>[`public override string Inno.Audio.AudioPlayOptions.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-fc913b8db293e9d2"></a>[`public Inno.Audio.AudioBusId Inno.Audio.AudioPlayOptions.bus { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L525) | Gets the destination mixer bus. | — | — |
| <a id="api-a4942011fccfa101"></a>[`public static Inno.Audio.AudioPlayOptions Inno.Audio.AudioPlayOptions.defaultValue { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L495) | Gets the default playback parameters. | — | — |
| <a id="api-0a140f401dca230d"></a>[`public Inno.Audio.AudioClipLoadMode Inno.Audio.AudioPlayOptions.loadMode { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L530) | Gets the clip preparation override. | — | — |
| <a id="api-5539b182a8619ffc"></a>[`public bool Inno.Audio.AudioPlayOptions.loop { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L515) | Gets whether playback loops at the clip end. | — | — |
| <a id="api-977b0c3357d83d3c"></a>[`public float Inno.Audio.AudioPlayOptions.pan { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L510) | Gets the stereo pan value. | — | — |
| <a id="api-a2aed0ba9adffda6"></a>[`public float Inno.Audio.AudioPlayOptions.pitch { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L505) | Gets the playback-rate multiplier. | — | — |
| <a id="api-18352ed7de952598"></a>[`public int Inno.Audio.AudioPlayOptions.priority { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L520) | Gets the voice retention priority. | — | — |
| <a id="api-236295edc7b2043a"></a>[`public Inno.Audio.AudioSpatialOptions? Inno.Audio.AudioPlayOptions.spatial { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L535) | Gets optional spatial playback parameters. | — | — |
| <a id="api-b17f197046c7ca6d"></a>[`public float Inno.Audio.AudioPlayOptions.volume { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L500) | Gets the linear voice gain. | — | — |
| <a id="api-5a14c327d4aca249"></a>[`public readonly record struct Inno.Audio.AudioPlayOptions : System.IEquatable<Inno.Audio.AudioPlayOptions>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L418) | Contains immutable parameters used to create one playback voice. | — | — |

### `Inno.Audio.AudioProcessorConfiguration`

<!-- api: T:Inno.Audio.AudioProcessorConfiguration a9ecaf8c2b580c44c1468f20543aa01da878154cb0f7ec894377719139c50376 -->
<!-- api: M:Inno.Audio.AudioProcessorConfiguration.#ctor(Inno.Audio.AudioProcessorId,System.Collections.Generic.IEnumerable{Inno.Audio.AudioProcessorParameter}) 08f02b4a5fa4eb4a1aee66a4cc71d71a846d19bf9296ea422391b8e23fe07ed3 -->
<!-- api: P:Inno.Audio.AudioProcessorConfiguration.id 71c5cccb3cdb59e6915ca5a6fcaa89612188c38947b090d5e2f37b578fb16e54 -->
<!-- api: P:Inno.Audio.AudioProcessorConfiguration.parameters 80edf72054fac187b4fc9ddd6a9beda379f12e350fb5c3ba13667605b6911a52 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-233be0b61828d033"></a>[`public Inno.Audio.AudioProcessorConfiguration.AudioProcessorConfiguration(Inno.Audio.AudioProcessorId id, System.Collections.Generic.IEnumerable<Inno.Audio.AudioProcessorParameter>? parameters = null)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L61) | Creates an immutable processor configuration.<br>`id`：Open processor protocol identifier.<br>`parameters`：Complete processor parameter set. | — | — |
| <a id="api-28dfc0e680750a18"></a>[`public Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorConfiguration.id { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L76) | Gets the open processor protocol identifier. | — | — |
| <a id="api-622e7b2365608d5f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Audio.AudioProcessorParameter> Inno.Audio.AudioProcessorConfiguration.parameters { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L81) | Gets the immutable processor parameter set. | — | — |
| <a id="api-42a67d884ffa0cde"></a>[`public sealed class Inno.Audio.AudioProcessorConfiguration`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L48) | Describes one ordered processor instance attached to a mixer bus. | — | — |

### `Inno.Audio.AudioProcessorId`

<!-- api: T:Inno.Audio.AudioProcessorId 267b998113b659ed65ff2228067d844b348f5e3f23239842797b979d06addf6d -->
<!-- api: M:Inno.Audio.AudioProcessorId.#ctor(System.String) e6db3ab062456c52f3ace56b2593d038123d9668a6599323ef6d9d960a122f97 -->
<!-- api: P:Inno.Audio.AudioProcessorId.lowPass 61e19f94826667d33f39d29ce49dfb67dd3ee1a6fa48c33e669d7ffc1d986fe0 -->
<!-- api: P:Inno.Audio.AudioProcessorId.highPass a8ddfead58033393fa42c06f83cf2a53491ccad3fef651d38ab09ef46c1f93b4 -->
<!-- api: P:Inno.Audio.AudioProcessorId.bandPass ea9ddf184c5c5581ca3b4984424fae3f4415d636bce4dd7d5490ac98cb79585d -->
<!-- api: P:Inno.Audio.AudioProcessorId.notch 6885ce7c19fa1f355f494edcfd35c3ebf12e3b667a78c62f8807be152d6803bb -->
<!-- api: P:Inno.Audio.AudioProcessorId.peak 7c6d6c826799a59562374025fe5083716180fdbe3d9e24783a22f11c564a67dd -->
<!-- api: P:Inno.Audio.AudioProcessorId.lowShelf 91cbb5b96ee96d11de4b5b70d9bc2ba7b0158ce45a890d4a55b79824036e6662 -->
<!-- api: P:Inno.Audio.AudioProcessorId.highShelf c18b9eac07f4cd42ee9fbe837928242bd6534f860a21def19c73d55c28bbd2c8 -->
<!-- api: P:Inno.Audio.AudioProcessorId.delay 9aaad6c727a74e18bcadc6df96c14abdc1806e32a0ae4b2ac2ded076280663b0 -->
<!-- api: P:Inno.Audio.AudioProcessorId.value 7c5ebdb3b11624825d8ac36b9d2b188f2978b38b2ea49c6cba4ce7a0d765d0e9 -->
<!-- api: P:Inno.Audio.AudioProcessorId.isValid 8f0c3a4c7058ef079382620444c25099f6092a15c086260c78ef059858aed488 -->
<!-- api: M:Inno.Audio.AudioProcessorId.ToString ac221510d57642143c74b2179c829f25c3a8c15a46bff6013e5c5d06c9f5ba4a -->
<!-- api: M:Inno.Audio.AudioProcessorId.op_Inequality(Inno.Audio.AudioProcessorId,Inno.Audio.AudioProcessorId) 16914b2f48c7aebdc33b8abc9264d3a8d10188c4cae80cf9f0cf1e73ee4bd4af -->
<!-- api: M:Inno.Audio.AudioProcessorId.op_Equality(Inno.Audio.AudioProcessorId,Inno.Audio.AudioProcessorId) c2e6616887326221ea3bea9517ad2549f79b224bd54bc199e981f67a6f827f43 -->
<!-- api: M:Inno.Audio.AudioProcessorId.GetHashCode f0ce343f2e29bb9440008192ea33e49d25365899cc229c3bf119a8f57ae6e329 -->
<!-- api: M:Inno.Audio.AudioProcessorId.Equals(System.Object) c4fd8b9ca243a7a6059abbd049a1c0693539de91e9fa24a49812d0a4ce5393f6 -->
<!-- api: M:Inno.Audio.AudioProcessorId.Equals(Inno.Audio.AudioProcessorId) a7d8d61fd00e28b46564d5a705b0c655677162350e148025306e86b60ef9c254 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a1e4b069bbc8124a"></a>[`public Inno.Audio.AudioProcessorId.AudioProcessorId(string value)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L57) | Creates an open processor identifier.<br>`value`：Globally stable processor protocol value. | — | — |
| <a id="api-1956fefa9214f082"></a>[`public bool Inno.Audio.AudioProcessorId.Equals(Inno.Audio.AudioProcessorId other)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d0fdf479ad938c9c"></a>[`public override bool Inno.Audio.AudioProcessorId.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c3ab2d92241a00a0"></a>[`public override int Inno.Audio.AudioProcessorId.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-be3015b3fdfdafc2"></a>[`public static bool Inno.Audio.AudioProcessorId.operator ==(Inno.Audio.AudioProcessorId left, Inno.Audio.AudioProcessorId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2e2b1caaab86a6cc"></a>[`public static bool Inno.Audio.AudioProcessorId.operator !=(Inno.Audio.AudioProcessorId left, Inno.Audio.AudioProcessorId right)`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-63f4ba4afa4b7c09"></a>[`public override string Inno.Audio.AudioProcessorId.ToString()`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L119) | Formats the identifier for diagnostics and persistence.<br>返回：The stable protocol value. | — | — |
| <a id="api-57c9144d658e2d0b"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.bandPass { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L76) | Gets the standard band-pass filter processor identifier. | — | — |
| <a id="api-4b01e874933c01d7"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.delay { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L101) | Gets the standard delay processor identifier. | — | — |
| <a id="api-f8e38a8dbc222d3b"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.highPass { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L71) | Gets the standard high-pass filter processor identifier. | — | — |
| <a id="api-42f7e9a5e4a048d8"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.highShelf { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L96) | Gets the standard high-shelf equalizer processor identifier. | — | — |
| <a id="api-e535593f1bbfac96"></a>[`public bool Inno.Audio.AudioProcessorId.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L111) | Gets whether this identifier contains a usable protocol value. | — | — |
| <a id="api-81467be1fb0c46b3"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.lowPass { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L66) | Gets the standard low-pass filter processor identifier. | — | — |
| <a id="api-f361099b7eb23006"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.lowShelf { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L91) | Gets the standard low-shelf equalizer processor identifier. | — | — |
| <a id="api-79c46a20881b435f"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.notch { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L81) | Gets the standard notch filter processor identifier. | — | — |
| <a id="api-75f3cac926e0a968"></a>[`public static Inno.Audio.AudioProcessorId Inno.Audio.AudioProcessorId.peak { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L86) | Gets the standard peak equalizer processor identifier. | — | — |
| <a id="api-52309914c84b3189"></a>[`public string Inno.Audio.AudioProcessorId.value { get; }`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L106) | Gets the globally stable protocol value. | — | — |
| <a id="api-dd09820649e2d75d"></a>[`public readonly record struct Inno.Audio.AudioProcessorId : System.IEquatable<Inno.Audio.AudioProcessorId>`](../../src/services/audio/Inno.Audio/AudioProtocolIds.cs#L49) | Identifies an audio processor implementation without imposing a closed effect catalog. | — | — |

### `Inno.Audio.AudioProcessorParameter`

<!-- api: T:Inno.Audio.AudioProcessorParameter 9f1081c9b9c9339a8ea21230c204466b996586b0a345cafd0c4026912436f774 -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.#ctor(Inno.Audio.AudioParameterId,System.Single) c8a30dfcd76425776b276ad7d6d9770b2017e5a2fd14c30007889f514c970954 -->
<!-- api: P:Inno.Audio.AudioProcessorParameter.id c8a66ddc8b6dce1797686df23ee90724f782991ced8280fabb76bff3bc53c7ac -->
<!-- api: P:Inno.Audio.AudioProcessorParameter.value 81adbeea7ebc7458686df755eb981f305bef1165cff353a638ef9e296b73a3fd -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.ToString 94fc91e9bae9e75c6ff50d460629bab4aebfe1bf8ce83bb48c34e8dcca89fe20 -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.op_Inequality(Inno.Audio.AudioProcessorParameter,Inno.Audio.AudioProcessorParameter) 46c4b838ff2da02591fbf8c7663bc386ce3d75f5ef11a6912a3a2e72dad5010d -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.op_Equality(Inno.Audio.AudioProcessorParameter,Inno.Audio.AudioProcessorParameter) 904c38ca6ebedec96a2d3deaebcf056d57a93971d14df04391571f6058c21d02 -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.GetHashCode aff0b203dc7b705c61cac6f87459f9bf5251d01b45e3552af22ca47136e2af11 -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.Equals(System.Object) a690e44d09dbd61c6556256c8ae4d27c33d0f32d28f4af6bd14e824bdc87aa8e -->
<!-- api: M:Inno.Audio.AudioProcessorParameter.Equals(Inno.Audio.AudioProcessorParameter) c4b0894ebb005d47746a98896b5ae5d75ccf084acfd081b95c961f2da166d066 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fe8624356951213d"></a>[`public Inno.Audio.AudioProcessorParameter.AudioProcessorParameter(Inno.Audio.AudioParameterId id, float value)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L24) | Creates a processor parameter value.<br>`id`：Open parameter identifier understood by the processor protocol.<br>`value`：Neutral numeric value interpreted by the processor. | — | — |
| <a id="api-31b587d77fdf77c4"></a>[`public bool Inno.Audio.AudioProcessorParameter.Equals(Inno.Audio.AudioProcessorParameter other)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a9f8da4cee7ef730"></a>[`public override bool Inno.Audio.AudioProcessorParameter.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-a3a73c87c6312f48"></a>[`public override int Inno.Audio.AudioProcessorParameter.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-774b56c1fa47ffaf"></a>[`public static bool Inno.Audio.AudioProcessorParameter.operator ==(Inno.Audio.AudioProcessorParameter left, Inno.Audio.AudioProcessorParameter right)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a7cf1b9b5368bd6c"></a>[`public static bool Inno.Audio.AudioProcessorParameter.operator !=(Inno.Audio.AudioProcessorParameter left, Inno.Audio.AudioProcessorParameter right)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-61a38b9e7e4b0836"></a>[`public override string Inno.Audio.AudioProcessorParameter.ToString()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-31b206bc9d9c0c46"></a>[`public Inno.Audio.AudioParameterId Inno.Audio.AudioProcessorParameter.id { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L37) | Gets the open parameter identifier. | — | — |
| <a id="api-7110fc26c98eee63"></a>[`public float Inno.Audio.AudioProcessorParameter.value { get; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L42) | Gets the neutral numeric value. | — | — |
| <a id="api-006a1f1df60de021"></a>[`public readonly record struct Inno.Audio.AudioProcessorParameter : System.IEquatable<Inno.Audio.AudioProcessorParameter>`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L13) | Stores one neutral numeric parameter assigned to an audio processor. | — | — |

### `Inno.Audio.AudioProjectSettings`

<!-- api: T:Inno.Audio.AudioProjectSettings be9d0367734cb027edd4d219b81fc3f1c39a07862ffb4333074a9ae4423baa0f -->
<!-- api: F:Inno.Audio.AudioProjectSettings.settingProtocolId f1cdad1416a3fb1d847233432c53bd4385c9b05b842ee302b03d2b823a2ce540 -->
<!-- api: P:Inno.Audio.AudioProjectSettings.settingId 45504e77e90d17f7e40bcae0b2ec775a7143d97d48cbe8e66e2a52c77f2f5d9d -->
<!-- api: P:Inno.Audio.AudioProjectSettings.defaultMixer 38a806a7645b1ef2dbd16dd7184d069e8663873dfc395c3234850b9f3cf9f70c -->
<!-- api: P:Inno.Audio.AudioProjectSettings.masterVolume 61ca97406aded5e07e6b6cfd5b988b22b76a5a21842b52838cce6425effd4713 -->
<!-- api: P:Inno.Audio.AudioProjectSettings.maxVoices f54363ab488b554f45a64a5983c691d611e9bbc4494c552a8b0ca4567de03b77 -->
<!-- api: P:Inno.Audio.AudioProjectSettings.decodedCacheBudgetBytes b5539f605f6a8a5c86ca35ae7bf442d129c954e986af2ffc40aa972453af22e7 -->
<!-- api: P:Inno.Audio.AudioProjectSettings.automaticStreamingThresholdBytes d891e85dfff097fa83956989f9f1fd394af66a3e379bf67c979ed449228360e8 -->
<!-- api: M:Inno.Audio.AudioProjectSettings.#ctor ccbc1f1d67219b289786eb01176c0ea76fc1b492220598f70df1b50a81324c22 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-28a7889d434dbdfd"></a>[`public const string Inno.Audio.AudioProjectSettings.settingProtocolId = "inno.audio.runtime"`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L17) | Gets the immutable project-setting protocol value used by discovery metadata. | — | — |
| <a id="api-629974c0df19ba2a"></a>[`public Inno.Audio.AudioProjectSettings.AudioProjectSettings()`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-68e46ee69c2b7268"></a>[`public long Inno.Audio.AudioProjectSettings.automaticStreamingThresholdBytes { get; set; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L52) | Gets or sets the encoded byte threshold used by automatic streaming selection. | — | — |
| <a id="api-698b1f828c8e3dae"></a>[`public long Inno.Audio.AudioProjectSettings.decodedCacheBudgetBytes { get; set; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L46) | Gets or sets the decoded clip cache budget in bytes. | — | — |
| <a id="api-d12ebcfdda7e27e1"></a>[`public Inno.Audio.AudioMixerAsset? Inno.Audio.AudioProjectSettings.defaultMixer { get; set; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L28) | Gets or sets the default mixer asset, or null for the master-only graph. | — | — |
| <a id="api-b0d1c5149f4b3cb0"></a>[`public float Inno.Audio.AudioProjectSettings.masterVolume { get; set; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L34) | Gets or sets the initial non-negative master bus gain. | — | — |
| <a id="api-f5820ce84820e907"></a>[`public int Inno.Audio.AudioProjectSettings.maxVoices { get; set; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L40) | Gets or sets the positive maximum active voice count. | — | — |
| <a id="api-3992246f5dd5b647"></a>[`public static Inno.Core.Settings.ProjectSettingId Inno.Audio.AudioProjectSettings.settingId { get; }`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L22) | Gets the stable project-setting identity for audio runtime defaults. | — | — |
| <a id="api-336fb995d7479d30"></a>[`public sealed class Inno.Audio.AudioProjectSettings : Inno.Core.Serialization.ISerializable`](../../src/services/audio/Inno.Audio/AudioProjectSettings.cs#L12) | Stores portable project-wide defaults for audio playback and caching. | — | — |

### `Inno.Audio.AudioSpatialOptions`

<!-- api: T:Inno.Audio.AudioSpatialOptions 6229e93d6da3b3881e9290653825c248a36ba9c5452dda0c30310f3d62205732 -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.#ctor(Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Audio.AudioDistanceModel,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single) 78a66e21d4ee5ad5d0aa0aa3740f88cfb8c2366b0459d826e364a6c20eec417a -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.position 41946215ce190c3f6a855797a43dd6924711dd06226865d5ca979c5e8fee2e01 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.direction 510c1d5c5de2424147fe6cf3da27905ca2d405f5f259e1af5123de728f755829 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.velocity 2b8ba196a29f2e8d9fe095b14c64b5dd213490e26e48c22f2055f5a2e499016c -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.distanceModel daaa56a74321ee903139519f535d5c4af8772dc262db1a899ebb3fe84c1332b5 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.minDistance 7d210b69551f21f95ed7c9e1c5e4404ee90b05ac19a2f094f2d34334d7599ffb -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.maxDistance 2ad11cce9a2be3204408c365eda479a8f740bc5bce834f0bcfe95b54fb070819 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.rolloff 1b70ae835ef4b6f80eb0c62f568b5afc8516c4715cc83db29d231a7c2c434a85 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.coneInnerAngle f4af71a57ffe9a1c41140aecfa8a67273411a59e8e3e88b7fdcd292c91618734 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.coneOuterAngle 1277d051abf2aa24d340013d0370990e615187269c3a7a5d77e6ce979dad599e -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.coneOuterGain 7888e9291d099f1901c2aacd0c2eb3e31e525f13e9edabc62f5a1e4dfbf64fe7 -->
<!-- api: P:Inno.Audio.AudioSpatialOptions.dopplerFactor e832d09fab71ea42d77379a318c83410015ebc44d4c6437ec1cebf9f198da33b -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.ToString df808bf5d8aa7f0d8b2090a44a4936abccf3afa95b38333ca3792132d85c4963 -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.op_Inequality(Inno.Audio.AudioSpatialOptions,Inno.Audio.AudioSpatialOptions) 9b2ca0764eddef9b110fdf2f8c5eb22124615b5eb7d0429ae9be63742c93e7a9 -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.op_Equality(Inno.Audio.AudioSpatialOptions,Inno.Audio.AudioSpatialOptions) 71dff44f8e0da255a38e468a9576bf2c4591a790b7daa7dde76e8ed47a400568 -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.GetHashCode c072414867306875829201f7fd34790dec99625488ccace237eb09536ac2db26 -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.Equals(System.Object) 8a24449c6f5cfb888200743c1e8695f657280e4076a613cce52f552f1703a09c -->
<!-- api: M:Inno.Audio.AudioSpatialOptions.Equals(Inno.Audio.AudioSpatialOptions) 9b84f1973a247d6ca9afc622a06a1b60104b549fa89850e5d8d6cc645d6eecf8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-760e7f65fb4e8fd8"></a>[`public Inno.Audio.AudioSpatialOptions.AudioSpatialOptions(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 direction, Inno.Core.Mathematics.Vector3 velocity, Inno.Audio.AudioDistanceModel distanceModel = Inno.Audio.AudioDistanceModel.Inverse, float minDistance = 1, float maxDistance = 100, float rolloff = 1, float coneInnerAngle = 360, float coneOuterAngle = 360, float coneOuterGain = 1, float dopplerFactor = 1)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L317) | Creates spatial playback parameters.<br>`position`：World-space source position.<br>`direction`：World-space source forward direction.<br>`velocity`：World-space source velocity used for Doppler calculation.<br>`distanceModel`：Distance attenuation model.<br>`minDistance`：Distance at which attenuation begins.<br>`maxDistance`：Distance beyond which attenuation is clamped.<br>`rolloff`：Attenuation strength for the selected distance model.<br>`coneInnerAngle`：Full-volume source cone angle in degrees.<br>`coneOuterAngle`：Outer source cone angle in degrees.<br>`coneOuterGain`：Gain outside the outer cone.<br>`dopplerFactor`：Doppler effect multiplier. | — | `System.ArgumentOutOfRangeException`：A vector or scalar is non-finite, a spatial bound is invalid, or the distance model is undefined. |
| <a id="api-40f5f3470465ddd5"></a>[`public bool Inno.Audio.AudioSpatialOptions.Equals(Inno.Audio.AudioSpatialOptions other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-4ea635385e1fd57d"></a>[`public override bool Inno.Audio.AudioSpatialOptions.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-26d1b52ca608b3e5"></a>[`public override int Inno.Audio.AudioSpatialOptions.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-e9bc53c4160bc64d"></a>[`public static bool Inno.Audio.AudioSpatialOptions.operator ==(Inno.Audio.AudioSpatialOptions left, Inno.Audio.AudioSpatialOptions right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-34bf0d7c7db4fd6f"></a>[`public static bool Inno.Audio.AudioSpatialOptions.operator !=(Inno.Audio.AudioSpatialOptions left, Inno.Audio.AudioSpatialOptions right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c9deb5d6f8886615"></a>[`public override string Inno.Audio.AudioSpatialOptions.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-9b5a6cd9fcf706c4"></a>[`public float Inno.Audio.AudioSpatialOptions.coneInnerAngle { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L397) | Gets the full-volume cone angle in degrees. | — | — |
| <a id="api-3d7e5a6abd58386d"></a>[`public float Inno.Audio.AudioSpatialOptions.coneOuterAngle { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L402) | Gets the outer cone angle in degrees. | — | — |
| <a id="api-3d4c785001c0ae65"></a>[`public float Inno.Audio.AudioSpatialOptions.coneOuterGain { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L407) | Gets the gain outside the outer cone. | — | — |
| <a id="api-49197f370aa31e26"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.direction { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L367) | Gets the world-space source forward direction. | — | — |
| <a id="api-8b279b0cb288e079"></a>[`public Inno.Audio.AudioDistanceModel Inno.Audio.AudioSpatialOptions.distanceModel { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L377) | Gets the distance attenuation model. | — | — |
| <a id="api-72b49cf5c3e1473d"></a>[`public float Inno.Audio.AudioSpatialOptions.dopplerFactor { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L412) | Gets the Doppler effect multiplier. | — | — |
| <a id="api-7654e855422fbe9a"></a>[`public float Inno.Audio.AudioSpatialOptions.maxDistance { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L387) | Gets the distance beyond which attenuation is clamped. | — | — |
| <a id="api-de265104dbebb9b6"></a>[`public float Inno.Audio.AudioSpatialOptions.minDistance { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L382) | Gets the distance at which attenuation begins. | — | — |
| <a id="api-513e2f03578c8de9"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.position { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L362) | Gets the world-space source position. | — | — |
| <a id="api-c003503d7212d636"></a>[`public float Inno.Audio.AudioSpatialOptions.rolloff { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L392) | Gets the distance attenuation strength. | — | — |
| <a id="api-9957d43f947e9168"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Audio.AudioSpatialOptions.velocity { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L372) | Gets the world-space source velocity. | — | — |
| <a id="api-e9452eb9207bb012"></a>[`public readonly record struct Inno.Audio.AudioSpatialOptions : System.IEquatable<Inno.Audio.AudioSpatialOptions>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L276) | Contains listener-relative positioning and attenuation parameters for one voice. | — | — |

### `Inno.Audio.AudioStatistics`

<!-- api: T:Inno.Audio.AudioStatistics 92c6dcdff4972a0ee3082d564c597830c69bc75a48018a65745d7afd627a4016 -->
<!-- api: M:Inno.Audio.AudioStatistics.#ctor(System.Int32,System.Int32,System.Int64,System.Int64) 728187be66c18dc772f251d8d924c0bf17187f92d0bfe179a4eaf9697926def0 -->
<!-- api: P:Inno.Audio.AudioStatistics.activeVoices 50c99dcc255c4701ddddf382e1d82b64b79ddbf1ee534569f75f1ea41f783427 -->
<!-- api: P:Inno.Audio.AudioStatistics.loadedClips 785b7521792d766fd8dd4b3e32f86e9781d3fe89b8d8e0a39dd3ddc6b6d1ee87 -->
<!-- api: P:Inno.Audio.AudioStatistics.decodedBytes 83de8d23e647320aff0aa173039b1076418268919481a6c9d78b5f22f52a2afe -->
<!-- api: P:Inno.Audio.AudioStatistics.stolenVoiceCount bdd6694a78f11311f76d045528f417b188100b477b8a0a82a66673c325bff61e -->
<!-- api: M:Inno.Audio.AudioStatistics.ToString cb1bebe82d0722efe4806249a4c4021d857fa4a9671f6fb9ed880ee7e26ba5b5 -->
<!-- api: M:Inno.Audio.AudioStatistics.op_Inequality(Inno.Audio.AudioStatistics,Inno.Audio.AudioStatistics) e1a78787be019e0ee037b5387b0fba3c2d919a949d12c11a8a049788f47ffd8c -->
<!-- api: M:Inno.Audio.AudioStatistics.op_Equality(Inno.Audio.AudioStatistics,Inno.Audio.AudioStatistics) 84779ced1d35f46fbc1803dcb91a95fc8427d28540427db6f65df1f6bcc16ccc -->
<!-- api: M:Inno.Audio.AudioStatistics.GetHashCode 58589dd8c4ae7b049e63c672a953c3244a06a6ba00bc59f74c5801d925a6353e -->
<!-- api: M:Inno.Audio.AudioStatistics.Equals(System.Object) b8c2942af324c144582d1dc4a618b2e2f69bfdc2cf7e01d71afa09f9e3de77c6 -->
<!-- api: M:Inno.Audio.AudioStatistics.Equals(Inno.Audio.AudioStatistics) e0fa462f9501b5143edea9431f9f761251812f6ab1ef7de4f2f3abe14267721d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a5cb38fa0f77321e"></a>[`public Inno.Audio.AudioStatistics.AudioStatistics(int activeVoices, int loadedClips, long decodedBytes, long stolenVoiceCount)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L236) | Creates an audio statistics snapshot.<br>`activeVoices`：Voices that currently consume runtime budget.<br>`loadedClips`：Clip allocations held by the backend.<br>`decodedBytes`：Approximate decoded clip bytes held in memory.<br>`stolenVoiceCount`：Voices reclaimed since runtime creation. | — | — |
| <a id="api-e725edc1bb8edf8d"></a>[`public bool Inno.Audio.AudioStatistics.Equals(Inno.Audio.AudioStatistics other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-311d526b6d531b7e"></a>[`public override bool Inno.Audio.AudioStatistics.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-a9ef5a766d2a6524"></a>[`public override int Inno.Audio.AudioStatistics.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-29c57c82dacff90b"></a>[`public static bool Inno.Audio.AudioStatistics.operator ==(Inno.Audio.AudioStatistics left, Inno.Audio.AudioStatistics right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d8904cbcfba5ecf8"></a>[`public static bool Inno.Audio.AudioStatistics.operator !=(Inno.Audio.AudioStatistics left, Inno.Audio.AudioStatistics right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d23bd0f07d039730"></a>[`public override string Inno.Audio.AudioStatistics.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-cf7bba4acb830dfd"></a>[`public int Inno.Audio.AudioStatistics.activeVoices { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L255) | Gets voices that currently consume runtime budget. | — | — |
| <a id="api-81243924528dc72e"></a>[`public long Inno.Audio.AudioStatistics.decodedBytes { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L265) | Gets approximate decoded clip bytes held in memory. | — | — |
| <a id="api-b8403ac0e2c8fb37"></a>[`public int Inno.Audio.AudioStatistics.loadedClips { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L260) | Gets clip allocations held by the backend. | — | — |
| <a id="api-b8bbe6cf1cda5896"></a>[`public long Inno.Audio.AudioStatistics.stolenVoiceCount { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L270) | Gets voices reclaimed since runtime creation. | — | — |
| <a id="api-dc47ff150a9db349"></a>[`public readonly record struct Inno.Audio.AudioStatistics : System.IEquatable<Inno.Audio.AudioStatistics>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L219) | Reports an immutable snapshot of runtime and backend resource usage. | — | — |

### `Inno.Audio.AudioVoiceAllocator`

<!-- api: T:Inno.Audio.AudioVoiceAllocator 7ea5a0c52babbb04e44c7b8bdb284c9b23467822629ef3dc20947bce6dd5f2c3 -->
<!-- api: M:Inno.Audio.AudioVoiceAllocator.#ctor 1682e7ec36771abfb97b23688ab37ffd2912312b22c89686874f916f19ab99ab -->
<!-- api: P:Inno.Audio.AudioVoiceAllocator.generation 180ef6986400594c7bf985c8f8c64ce4058376022d05dd20c1ce1f578c5b9539 -->
<!-- api: M:Inno.Audio.AudioVoiceAllocator.Allocate 7fc8cf54b45bdaefdddd10f64c9070b843949a5ef305ccdda3e993d83be139ac -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-30f2a6c2e5785ddd"></a>[`public Inno.Audio.AudioVoiceAllocator.AudioVoiceAllocator()`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L21) | Creates an isolated playback identity owner. | — | `System.InvalidOperationException`：The process exhausted its owner generations. |
| <a id="api-fc64a8295a7dae21"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Audio.AudioVoiceAllocator.Allocate()`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L43) | Allocates a unique service handle that cannot alias a different service's playback.<br>返回：A new opaque playback handle; identities are never reused within this owner. | — | `System.OverflowException`：The owner exhausted its playback identities. |
| <a id="api-c836aa9b9686f622"></a>[`public uint Inno.Audio.AudioVoiceAllocator.generation { get; }`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L32) | Gets the process-unique owner generation encoded in every allocated playback handle. | — | — |
| <a id="api-6f7b86f4a7b0b6f6"></a>[`public sealed class Inno.Audio.AudioVoiceAllocator`](../../src/services/audio/Inno.Audio/AudioVoiceAllocator.cs#L9) | Allocates service-owned playback handles without impersonating a backend audio device. | — | — |

### `Inno.Audio.AudioVoiceCompletedEvent`

<!-- api: T:Inno.Audio.AudioVoiceCompletedEvent cf0b8426921f0127dc2215b14a4ae54f13ed98586b129194eb181ae4cf747b1f -->
<!-- api: M:Inno.Audio.AudioVoiceCompletedEvent.#ctor(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioCompletionReason) 84ada2c5265f5956ab330780666779e51394fbe36d88c3b5f7caa6773856cd83 -->
<!-- api: P:Inno.Audio.AudioVoiceCompletedEvent.voice 05aa91f7ad5ea6f0d9eb8d5a77afb132cef8db440720a3cfda0363d4f29c5181 -->
<!-- api: P:Inno.Audio.AudioVoiceCompletedEvent.reason 8b222bac01b06eb254556582b2e1cf7f622546014929f3861638448209428f98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-30e849588c7477ae"></a>[`public Inno.Audio.AudioVoiceCompletedEvent.AudioVoiceCompletedEvent(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioCompletionReason reason)`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L20) | Creates a voice completion event.<br>`voice`：Voice that reached a terminal state.<br>`reason`：Reason playback ended. | — | — |
| <a id="api-918955e06bbb9f8f"></a>[`public Inno.Audio.AudioCompletionReason Inno.Audio.AudioVoiceCompletedEvent.reason { get; }`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L38) | Gets the terminal playback reason. | — | — |
| <a id="api-0a5b4f31ebdef231"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Audio.AudioVoiceCompletedEvent.voice { get; }`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L33) | Gets the completed voice handle. | — | — |
| <a id="api-583a199059ba976b"></a>[`public sealed class Inno.Audio.AudioVoiceCompletedEvent : Inno.Core.Events.Event`](../../src/services/audio/Inno.Audio/AudioVoiceCompletedEvent.cs#L9) | Reports one voice terminal transition on the owning runtime's main-thread event dispatcher. | — | — |

### `Inno.Audio.AudioVoiceHandle`

<!-- api: T:Inno.Audio.AudioVoiceHandle b1fcb9774f3f822b4f9eb0fee9c22f00a6710d93da5e606ce2cf87b909aec71c -->
<!-- api: P:Inno.Audio.AudioVoiceHandle.ownerGeneration 91f2514d3d6a18f8a2265ae5ac9fa0c4f1c3adbb9f6822ed692e6be90077ed04 -->
<!-- api: P:Inno.Audio.AudioVoiceHandle.isValid eda2dde4fc60a6e5216e961240f87eb581eb0f7916b7f3312aab270f40faf618 -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.ToString 099130e8882158691339109584c205a2524d64ce29e415c08c3565e5de656774 -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.op_Inequality(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioVoiceHandle) aa865304ddc2f16c5f77b45bfd3c0e27b33666287f9dc46a9715ccc8c3c3b234 -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.op_Equality(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioVoiceHandle) f6a091b7da26bf58ebfb263e841ba1d32ccaa7bc60444025cf10adaac8498105 -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.GetHashCode 3b26eda75a7fce73f37079d4c6c2338e78d94991e6735d243a38cd87446f73ef -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.Equals(System.Object) 1fcb03dbd46fca407d645569b7b6feebecb3d040662b0512c20bbeabfc482bac -->
<!-- api: M:Inno.Audio.AudioVoiceHandle.Equals(Inno.Audio.AudioVoiceHandle) e3f2dc29ffc23ad976d5bba73221a0fdd1f4e889b184bd6d1f93235852e65145 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0e7d86f7b00f80bd"></a>[`public bool Inno.Audio.AudioVoiceHandle.Equals(Inno.Audio.AudioVoiceHandle other)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-f3e248dcfc0bb2ef"></a>[`public override bool Inno.Audio.AudioVoiceHandle.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-81e2905ef468cd43"></a>[`public override int Inno.Audio.AudioVoiceHandle.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-ed26e6c9cdfa2020"></a>[`public static bool Inno.Audio.AudioVoiceHandle.operator ==(Inno.Audio.AudioVoiceHandle left, Inno.Audio.AudioVoiceHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c506ad7162eebc4f"></a>[`public static bool Inno.Audio.AudioVoiceHandle.operator !=(Inno.Audio.AudioVoiceHandle left, Inno.Audio.AudioVoiceHandle right)`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c3f12a28dbdc9685"></a>[`public override string Inno.Audio.AudioVoiceHandle.ToString()`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-c7c40422228cd32d"></a>[`public bool Inno.Audio.AudioVoiceHandle.isValid { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L52) | Gets whether the handle contains a non-zero backend identity and generation. | — | — |
| <a id="api-2e9045e6e3f309da"></a>[`public uint Inno.Audio.AudioVoiceHandle.ownerGeneration { get; }`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L47) | Gets the generation of the device that created this handle. | — | — |
| <a id="api-e40d48d2f53e3fd5"></a>[`public readonly record struct Inno.Audio.AudioVoiceHandle : System.IEquatable<Inno.Audio.AudioVoiceHandle>`](../../src/services/audio/Inno.Audio/AudioHandles.cs#L32) | Identifies one playback voice within a device generation. | — | — |

### `Inno.Audio.AudioVoiceParameters`

<!-- api: T:Inno.Audio.AudioVoiceParameters 6442089860f6b0202ef5c5708292000bea54b46ef5b038866cf421e706f50137 -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.#ctor(System.Single,System.Single,System.Single,System.Nullable{Inno.Audio.AudioSpatialOptions}) a76ac677bb94aa4412fdfd167fdd232b69021681ebd5fb2a129f11424d2a4278 -->
<!-- api: P:Inno.Audio.AudioVoiceParameters.volume fe49521119d29c4217b2d914ddff97d353d5e03e7dd7b03ff00c13c6f167a433 -->
<!-- api: P:Inno.Audio.AudioVoiceParameters.pitch dd8beaf7c1cdac46fc169720fb80bca4ca162c4ee6a2ef69c699a071136b44dd -->
<!-- api: P:Inno.Audio.AudioVoiceParameters.pan 00390603330d907ad360408835c8e0b30caab6b5a3dd7f4b01ed602a593795d0 -->
<!-- api: P:Inno.Audio.AudioVoiceParameters.spatial d6f09e0fde0a327a03739f25aec5a06477087d54376c3193c1d2637e426e9a7f -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.ToString eab054e285247fee18d3606ef86e5c55f0601ae6a5d121382af2601000b0b2d8 -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.op_Inequality(Inno.Audio.AudioVoiceParameters,Inno.Audio.AudioVoiceParameters) 57fcafe7972fd6214c4d6e05eaca9bb38343f010eeca0d3ee00f6bd81cccc502 -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.op_Equality(Inno.Audio.AudioVoiceParameters,Inno.Audio.AudioVoiceParameters) 94e060368819fa0ab82110a034bf60c6a855332a7c4847884e2bb702b7c8bbf4 -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.GetHashCode 15a711b72da0f169f34b855a90e689d4c0ace39d9c362e50a334465af2fd1bba -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.Equals(System.Object) ec30eb6fafecb57217d0a3edd27115ad9feb3fa62f821a2a8ee26bc9878fa729 -->
<!-- api: M:Inno.Audio.AudioVoiceParameters.Equals(Inno.Audio.AudioVoiceParameters) 8f0c85a143829c136d950a3a7421b70fde93fc01a50250cb2073d63c49bdc1cb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e040aae9c94e7712"></a>[`public Inno.Audio.AudioVoiceParameters.AudioVoiceParameters(float volume, float pitch, float pan, Inno.Audio.AudioSpatialOptions? spatial = null)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L561) | Creates a voice parameter snapshot.<br>`volume`：Linear voice gain.<br>`pitch`：Playback-rate multiplier.<br>`pan`：Stereo pan from negative one to positive one.<br>`spatial`：Optional spatial playback parameters. | — | `System.ArgumentOutOfRangeException`：Gain, pitch, or pan is non-finite or outside its valid range. |
| <a id="api-132017806a5194a6"></a>[`public bool Inno.Audio.AudioVoiceParameters.Equals(Inno.Audio.AudioVoiceParameters other)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-af1c07c745aa8099"></a>[`public override bool Inno.Audio.AudioVoiceParameters.Equals(object obj)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-58f839d1e74d7ece"></a>[`public override int Inno.Audio.AudioVoiceParameters.GetHashCode()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8b4da3d9e954bfbb"></a>[`public static bool Inno.Audio.AudioVoiceParameters.operator ==(Inno.Audio.AudioVoiceParameters left, Inno.Audio.AudioVoiceParameters right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-191f87e17dcc473a"></a>[`public static bool Inno.Audio.AudioVoiceParameters.operator !=(Inno.Audio.AudioVoiceParameters left, Inno.Audio.AudioVoiceParameters right)`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6a03ac086200348e"></a>[`public override string Inno.Audio.AudioVoiceParameters.ToString()`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-06788e3c82408ec6"></a>[`public float Inno.Audio.AudioVoiceParameters.pan { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L592) | Gets the stereo pan value. | — | — |
| <a id="api-914d6de8df50f618"></a>[`public float Inno.Audio.AudioVoiceParameters.pitch { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L587) | Gets the playback-rate multiplier. | — | — |
| <a id="api-70252c1c48079934"></a>[`public Inno.Audio.AudioSpatialOptions? Inno.Audio.AudioVoiceParameters.spatial { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L597) | Gets optional spatial playback parameters. | — | — |
| <a id="api-dc81a3ccf8734d5c"></a>[`public float Inno.Audio.AudioVoiceParameters.volume { get; }`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L582) | Gets the linear voice gain. | — | — |
| <a id="api-3a5bdcea295d1b52"></a>[`public readonly record struct Inno.Audio.AudioVoiceParameters : System.IEquatable<Inno.Audio.AudioVoiceParameters>`](../../src/services/audio/Inno.Audio/AudioTypes.cs#L541) | Contains mutable voice parameters that can be changed after playback begins. | — | — |

### `Inno.Audio.IAudioClipSource`

<!-- api: T:Inno.Audio.IAudioClipSource 84a348faa02d6784142d57cca809da0ebf26cd040eb72a4b23a858fce219cd8c -->
<!-- api: P:Inno.Audio.IAudioClipSource.contentHash 32a3f51cf0e47a512d4e95d640af08201293d7153e803a11f06ec8c1cc7b7cee -->
<!-- api: P:Inno.Audio.IAudioClipSource.length 11b8be7140a909141a0d44ab326f6ba952cbd9e443ec13dacc1ded6319e38806 -->
<!-- api: M:Inno.Audio.IAudioClipSource.OpenRead 6082b1e8620bc3f97ae89c826b07493429d8ddfc5ff10547d34bd7712b5469c6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f914b8781d11136f"></a>[`System.IO.Stream Inno.Audio.IAudioClipSource.OpenRead()`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L26) | Opens a reader with an independent content pin and cursor.<br>返回：A caller-owned readable stream; missing or retired content fails the operation. | — | — |
| <a id="api-3c15972d84a08200"></a>[`string Inno.Audio.IAudioClipSource.contentHash { get; }`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L13) | Gets the SHA-256 identity of the complete encoded bytes. | — | — |
| <a id="api-8f7cf0a2981e93ef"></a>[`long Inno.Audio.IAudioClipSource.length { get; }`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L18) | Gets the exact encoded byte length used to bound preparation. | — | — |
| <a id="api-c1eadbcadbb6827f"></a>[`public interface Inno.Audio.IAudioClipSource`](../../src/services/audio/Inno.Audio/IAudioClipSource.cs#L8) | Supplies immutable encoded audio independently of any backend's input layout. | — | — |

### `Inno.Audio.IAudioDevice`

<!-- api: T:Inno.Audio.IAudioDevice 2b5b130a5c72713eb0e3d1b44561cee35455314ae38883dbf48ddc76872b315a -->
<!-- api: M:Inno.Audio.IAudioDevice.GetClipState(Inno.Audio.AudioClipHandle) 90d0f2656eb70b625bba5942043366357b702dd17620b4bb3d81d37c9cd9722b -->
<!-- api: P:Inno.Audio.IAudioDevice.capabilities 3ca74bda59303c47e74868a8aa90f01f81a07cc3777b6f0297132329862e0821 -->
<!-- api: P:Inno.Audio.IAudioDevice.generation a451f29876ab86c07c565aeeadcde62eb25c5ed314adceef4a503a5c26ab6556 -->
<!-- api: P:Inno.Audio.IAudioDevice.state bd8c0ac0f7c8ac63ed7a8567d0108398435132f135049700ecfe870221b58e51 -->
<!-- api: P:Inno.Audio.IAudioDevice.dspTime 0272247e57f03a0ce9563ef0259dbe188ea706714209f5baf937cd3352c22249 -->
<!-- api: P:Inno.Audio.IAudioDevice.statistics a783841fba78a32e6b5fe622fccbe83c885d685751c752a63063ab0baa40dd89 -->
<!-- api: M:Inno.Audio.IAudioDevice.CreateClip(Inno.Audio.AudioClipDescriptor,Inno.Audio.IAudioClipSource) 513efefa5c27f9abd17c4958d756d4083e2dc345c6083851282d23c899dff434 -->
<!-- api: M:Inno.Audio.IAudioDevice.DestroyClip(Inno.Audio.AudioClipHandle) df965404958e504320057e64dc932205cc1dd75ad6eadf789f0a23e05d699fc0 -->
<!-- api: M:Inno.Audio.IAudioDevice.Play(Inno.Audio.AudioClipHandle,Inno.Audio.AudioBusHandle,Inno.Audio.AudioPlayOptions,System.Nullable{System.Double}) 1cae89d50cc5011a77512c5a2c8ffeaaeda2a9c7779bd36bab922947cc90a40d -->
<!-- api: M:Inno.Audio.IAudioDevice.Stop(Inno.Audio.AudioDeviceVoiceHandle) 50a2e293f196b19f3e3d4796139457e3d8fdce321f3ee1bb1d9841941dd097b4 -->
<!-- api: M:Inno.Audio.IAudioDevice.Pause(Inno.Audio.AudioDeviceVoiceHandle) 56bf94b429a9864b453d55a950bdbc17545c061213cac9828f73b863ec392334 -->
<!-- api: M:Inno.Audio.IAudioDevice.Resume(Inno.Audio.AudioDeviceVoiceHandle) ddb9c453f686a25aa53000d76784a6ec862bd794abcd2b2933064001f22c101a -->
<!-- api: M:Inno.Audio.IAudioDevice.Seek(Inno.Audio.AudioDeviceVoiceHandle,System.TimeSpan) e4790027bfd351bc99f1b464b05eb385a236dfc10a5692252abc15e782b8d019 -->
<!-- api: M:Inno.Audio.IAudioDevice.SetVoiceParameters(Inno.Audio.AudioDeviceVoiceHandle,Inno.Audio.AudioVoiceParameters) b1052ce4a0c45bacedf36a55d7c07e7bc86e3946909c2bdf378a1fe7c54b691b -->
<!-- api: M:Inno.Audio.IAudioDevice.TryGetVoiceState(Inno.Audio.AudioDeviceVoiceHandle,Inno.Audio.AudioPlaybackState@) 1afdd303ceac6942b3119caa997bd7f0fb23d66c9dc59c8f73c7725be0c3b4b1 -->
<!-- api: M:Inno.Audio.IAudioDevice.CreateBus(Inno.Audio.AudioBusId,Inno.Audio.AudioBusHandle) 1d8887484694af7dc08793fad6eead87021a3ff6391f18d1de39545a7e47cff6 -->
<!-- api: M:Inno.Audio.IAudioDevice.DestroyBus(Inno.Audio.AudioBusHandle) 60dcece58a8d32b18a04d1347abc018d8fd51c33539f8ccb6e02ee731666f9ef -->
<!-- api: M:Inno.Audio.IAudioDevice.SetBusVolume(Inno.Audio.AudioBusHandle,System.Single) cde5fd723f1ab9b1d812b16bc53226154ae3f8182c07e15d93572dc0609458c8 -->
<!-- api: M:Inno.Audio.IAudioDevice.SetBusMuted(Inno.Audio.AudioBusHandle,System.Boolean) 2d6b8665e1abb5597ffec3337a1a8ce0eb6cb5843961d329fa9fd424041629f4 -->
<!-- api: M:Inno.Audio.IAudioDevice.SetBusPaused(Inno.Audio.AudioBusHandle,System.Boolean) 6dc286ed674bd92b84e12c77569e2d3c112472843389bfe7ab2027c1dfa079bd -->
<!-- api: M:Inno.Audio.IAudioDevice.AddBusProcessor(Inno.Audio.AudioBusHandle,Inno.Audio.AudioProcessorConfiguration) b6d487b59c72702141a3df523e5017363f4b41859daa2411d75fd5719ede77ba -->
<!-- api: M:Inno.Audio.IAudioDevice.CreateListener(Inno.Audio.AudioListenerState) e2f6c0fd28a17831f8f852d94e9f66481886b25beb1c83a8f4f78356813d8365 -->
<!-- api: M:Inno.Audio.IAudioDevice.SetListener(Inno.Audio.AudioListenerHandle,Inno.Audio.AudioListenerState) f86b7a6dd70c8c0dd9d014bd378a60eeadab5a79f20a8b2a0905666ed7347852 -->
<!-- api: M:Inno.Audio.IAudioDevice.DestroyListener(Inno.Audio.AudioListenerHandle) 3428d7d5ae535f57c8df72ba9ded9af497db85f95f78c1f90529435f1d756e75 -->
<!-- api: M:Inno.Audio.IAudioDevice.Update(System.Single) bceec086f5e46e31eb4c657f794e0d0dd6ad5b118c170df986d44d87279d77d2 -->
<!-- api: M:Inno.Audio.IAudioDevice.TryDequeueCompletion(Inno.Audio.AudioDeviceCompletion@) 125a3a853a0fed1aa6ab9b0a376a40a2232ca0ba0935c58affb6b94a2ede4e4b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-de207b6f6aaa73a5"></a>[`bool Inno.Audio.IAudioDevice.AddBusProcessor(Inno.Audio.AudioBusHandle bus, Inno.Audio.AudioProcessorConfiguration processor)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L310) | Appends one backend-neutral processor configuration to a bus chain.<br>返回：true when the backend recognized and attached the processor.<br>`bus`：Bus owned by the current device generation.<br>`processor`：Open processor protocol and neutral parameter values. | — | — |
| <a id="api-db8104b8f6cca90e"></a>[`Inno.Audio.AudioBusHandle Inno.Audio.IAudioDevice.CreateBus(Inno.Audio.AudioBusId id, Inno.Audio.AudioBusHandle parent = default(Inno.Audio.AudioBusHandle))`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L231) | Creates one bus routed to a parent bus.<br>返回：A bus handle owned by the current device generation.<br>`id`：Stable semantic bus identifier.<br>`parent`：Parent bus, or an invalid handle when creating the master bus. | — | — |
| <a id="api-264763428a857b97"></a>[`Inno.Audio.AudioClipHandle Inno.Audio.IAudioDevice.CreateClip(Inno.Audio.AudioClipDescriptor descriptor, Inno.Audio.IAudioClipSource source)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L94) | Starts preparation of one decoded or streamed clip from immutable encoded content.<br>返回：A handle owned by the current device generation.<br>`descriptor`：Encoded artifact and preparation requirements.<br>`source`：The immutable encoded source, borrowed until successful clip retirement. Opening a stream must independently pin its content; the caller retains source ownership. | — | — |
| <a id="api-4993c8336acb6085"></a>[`Inno.Audio.AudioListenerHandle Inno.Audio.IAudioDevice.CreateListener(Inno.Audio.AudioListenerState state)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L324) | Creates one backend spatial listener.<br>返回：A listener handle owned by the current device generation.<br>`state`：Initial listener transform. | — | — |
| <a id="api-3e90348ff0709e63"></a>[`bool Inno.Audio.IAudioDevice.DestroyBus(Inno.Audio.AudioBusHandle bus)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L245) | Releases one graph-generation bus after dependent objects have been removed.<br>返回：true when a live bus was released.<br>`bus`：Bus owned by the current device generation. | — | — |
| <a id="api-c855e51cc7dd1c52"></a>[`bool Inno.Audio.IAudioDevice.DestroyClip(Inno.Audio.AudioClipHandle clip)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L108) | Releases one clip after all voices using it have ended.<br>返回：true when a live clip was released.<br>`clip`：Clip owned by the current device generation. | — | — |
| <a id="api-c15cac8ae6971ab9"></a>[`bool Inno.Audio.IAudioDevice.DestroyListener(Inno.Audio.AudioListenerHandle listener)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L352) | Releases one backend spatial listener.<br>返回：true when a live listener was released.<br>`listener`：Listener owned by the current device generation. | — | — |
| <a id="api-0fd2b32f4bdac529"></a>[`Inno.Audio.AudioClipState Inno.Audio.IAudioDevice.GetClipState(Inno.Audio.AudioClipHandle clip)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L54) | Queries asynchronous clip preparation without blocking the owner thread.<br>返回：The preparation state, or Failed for a retired or invalid handle.<br>`clip`：A handle created by this device generation. | — | — |
| <a id="api-87d3387a4b2a01ba"></a>[`bool Inno.Audio.IAudioDevice.Pause(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L155) | Pauses a live voice.<br>返回：true when the voice entered the paused state.<br>`voice`：Voice owned by the current device generation. | — | — |
| <a id="api-e8cb461a32a82a11"></a>[`Inno.Audio.AudioDeviceVoiceHandle Inno.Audio.IAudioDevice.Play(Inno.Audio.AudioClipHandle clip, Inno.Audio.AudioBusHandle bus, Inno.Audio.AudioPlayOptions options, double? scheduledDspTime = null)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L128) | Creates and starts or schedules one playback voice.<br>返回：A new voice handle, or an invalid handle when options are uninitialized, scheduling is invalid, or backend preparation fails.<br>`clip`：Prepared clip owned by this device generation.<br>`bus`：Destination bus owned by this device generation.<br>`options`：Immutable playback parameters.<br>`scheduledDspTime`：Absolute audio-clock start time, or null for immediate playback. | — | — |
| <a id="api-a2d493f1ac8f159a"></a>[`bool Inno.Audio.IAudioDevice.Resume(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L166) | Resumes a paused voice.<br>返回：true when the voice resumed.<br>`voice`：Voice owned by the current device generation. | — | — |
| <a id="api-fec65760f357bf44"></a>[`bool Inno.Audio.IAudioDevice.Seek(Inno.Audio.AudioDeviceVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L180) | Moves a live voice cursor to a clip-relative position.<br>返回：true when the cursor was updated.<br>`voice`：Voice owned by the current device generation.<br>`position`：Non-negative clip-relative position. | — | — |
| <a id="api-9589095c90fa8106"></a>[`bool Inno.Audio.IAudioDevice.SetBusMuted(Inno.Audio.AudioBusHandle bus, bool muted)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L276) | Updates mute state for one bus.<br>返回：true when the bus was updated.<br>`bus`：Bus owned by the current device generation.<br>`muted`：Whether output from the bus is silenced. | — | — |
| <a id="api-0f6beb45b82d9bd1"></a>[`bool Inno.Audio.IAudioDevice.SetBusPaused(Inno.Audio.AudioBusHandle bus, bool paused)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L293) | Updates pause state for one bus and its routed voices.<br>返回：true when the bus was updated.<br>`bus`：Bus owned by the current device generation.<br>`paused`：Whether routed voices should pause. | — | — |
| <a id="api-a9472afeeb7a6f00"></a>[`bool Inno.Audio.IAudioDevice.SetBusVolume(Inno.Audio.AudioBusHandle bus, float volume)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L259) | Updates linear gain for one bus.<br>返回：true when the bus was updated.<br>`bus`：Bus owned by the current device generation.<br>`volume`：Non-negative linear gain. | — | — |
| <a id="api-087c67dbe3c42998"></a>[`bool Inno.Audio.IAudioDevice.SetListener(Inno.Audio.AudioListenerHandle listener, Inno.Audio.AudioListenerState state)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L338) | Updates one backend spatial listener.<br>返回：true when the listener was updated.<br>`listener`：Listener owned by the current device generation.<br>`state`：Current listener transform. | — | — |
| <a id="api-ca9dd2dff86551c7"></a>[`bool Inno.Audio.IAudioDevice.SetVoiceParameters(Inno.Audio.AudioDeviceVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L197) | Replaces mutable parameters for a live voice.<br>返回：true when a live voice was updated; false for stale handles or uninitialized parameters.<br>`voice`：Voice owned by the current device generation.<br>`parameters`：Current voice parameter snapshot. | — | — |
| <a id="api-37ed69d7dbf72224"></a>[`bool Inno.Audio.IAudioDevice.Stop(Inno.Audio.AudioDeviceVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L144) | Stops a voice and makes its handle terminal.<br>返回：true when a live voice was stopped.<br>`voice`：Voice owned by the current device generation. | — | — |
| <a id="api-6f23bbc5a2cf6a2a"></a>[`bool Inno.Audio.IAudioDevice.TryDequeueCompletion(out Inno.Audio.AudioDeviceCompletion completion)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L371) | Tries to consume one terminal voice transition recorded by the backend.<br>返回：true when a completion was dequeued.<br>`completion`：Receives one terminal transition when available. | — | — |
| <a id="api-7da80970a6fde54d"></a>[`bool Inno.Audio.IAudioDevice.TryGetVoiceState(Inno.Audio.AudioDeviceVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L214) | Queries the current playback state for a voice.<br>返回：true when the handle belongs to a known voice.<br>`voice`：Voice handle to query.<br>`playbackState`：Receives the current state when the handle is known. | — | — |
| <a id="api-35c3df76f77d1a86"></a>[`void Inno.Audio.IAudioDevice.Update(float deltaTime)`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L360) | Advances backend maintenance at a main-thread safety point.<br>`deltaTime`：Non-negative elapsed frame time in seconds. | — | — |
| <a id="api-1eba2aae80746e76"></a>[`Inno.Audio.AudioCapabilities Inno.Audio.IAudioDevice.capabilities { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L59) | Gets immutable capabilities for the active device generation. | — | — |
| <a id="api-97d620cab1b7e4c2"></a>[`double Inno.Audio.IAudioDevice.dspTime { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L74) | Gets the monotonic backend audio clock in seconds. | — | — |
| <a id="api-6de8b73ac0ddc674"></a>[`uint Inno.Audio.IAudioDevice.generation { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L64) | Gets the non-zero generation used to reject stale handles. | — | — |
| <a id="api-1891987944833e98"></a>[`Inno.Audio.AudioDeviceState Inno.Audio.IAudioDevice.state { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L69) | Gets the current output availability state. | — | — |
| <a id="api-071736b8bb68ec8f"></a>[`Inno.Audio.AudioStatistics Inno.Audio.IAudioDevice.statistics { get; }`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L79) | Gets current backend resource statistics. | — | — |
| <a id="api-d66f1600c83594c4"></a>[`public interface Inno.Audio.IAudioDevice : System.IDisposable`](../../src/services/audio/Inno.Audio/IAudioDevice.cs#L43) | Owns one replaceable audio backend generation and all device-side audio objects. | — | — |

### `Inno.Audio.IAudioService`

<!-- api: T:Inno.Audio.IAudioService 84838042c871d3fb99f4a8a89a09d2b1f97d0fb15d74e1cb7284f575339def0e -->
<!-- api: P:Inno.Audio.IAudioService.capabilities 14e840bcae2be36d444bfeb0ffa0fc0c52f2d456690c0e72b8cb20d2540e6978 -->
<!-- api: P:Inno.Audio.IAudioService.deviceState 495bb02390c6d266e7e4ca077d8f5a1ab5318a411f1901c9a52465aaae476ad9 -->
<!-- api: P:Inno.Audio.IAudioService.dspTime 90cdb9c6efe2623ae6ff9b69b958e3c88fed500143d9ff6897e972378bc9d4a0 -->
<!-- api: P:Inno.Audio.IAudioService.statistics caba9861449e46a5ddfea6cf1f5bcfc43eeef64c6abc480d62c998d9a8fa58be -->
<!-- api: M:Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset) 1ce8ccf6a2d52ae6fb2eb398913f53be7374478121b009c9fc30bfc678732692 -->
<!-- api: M:Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset,Inno.Audio.AudioPlayOptions) a1690b9936af71534d47b49bbd6097da6ed3601f02b4a4c99be82832838212ff -->
<!-- api: M:Inno.Audio.IAudioService.PlayScheduled(Inno.Audio.AudioClipAsset,System.Double,Inno.Audio.AudioPlayOptions) fe496f9f9ef304e271fe00cf4c68e5b56cefa1ec71dd2a5a36659800076025f2 -->
<!-- api: M:Inno.Audio.IAudioService.Stop(Inno.Audio.AudioVoiceHandle) 6dcd5ae0ddc6bd2f921c6e80a41d86bbe0f9190ffbaed5821985d128730982c5 -->
<!-- api: M:Inno.Audio.IAudioService.Pause(Inno.Audio.AudioVoiceHandle) b65fb2bd63e4bd9a41c093011281c9d43acfdd64ba0035d64667490efdb50809 -->
<!-- api: M:Inno.Audio.IAudioService.Resume(Inno.Audio.AudioVoiceHandle) ecc3f427bc0bd0baa03e6590f5340a1f18195df2189b9ca24a81b9e2cdaafa90 -->
<!-- api: M:Inno.Audio.IAudioService.Seek(Inno.Audio.AudioVoiceHandle,System.TimeSpan) b4025258f7edc98c180d5622ebd5279570ab77e1c339989448e5c36b00420c20 -->
<!-- api: M:Inno.Audio.IAudioService.SetVoiceParameters(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioVoiceParameters) e02704888153032fe8ddd1f245116a11b5cfdaab696f7d0c5be637134ff13817 -->
<!-- api: M:Inno.Audio.IAudioService.TryGetVoiceState(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioPlaybackState@) cab08ec84112b008d0db671afa893aa9ad41c1aa17182fa4041b8c6a93de05a1 -->
<!-- api: M:Inno.Audio.IAudioService.SetBusVolume(Inno.Audio.AudioBusId,System.Single) 9fec083b972703b837ff5becc020bbe9412b7598d3aa16aece133c36b92e2b3e -->
<!-- api: M:Inno.Audio.IAudioService.SetBusMuted(Inno.Audio.AudioBusId,System.Boolean) 83777d4ee495b6d0476c0649766810e7c8a3a877be4e8c7d248a4a854e0fd403 -->
<!-- api: M:Inno.Audio.IAudioService.SetBusPaused(Inno.Audio.AudioBusId,System.Boolean) 91dc2b22c2edd0638fa1417599591d50a333f43e25fa21de80e9aaa05a4942d3 -->
<!-- api: M:Inno.Audio.IAudioService.PreloadAsync(Inno.Audio.AudioClipAsset,Inno.Audio.AudioClipLoadMode,System.Threading.CancellationToken) cac01da8986195a3e3ea4378814e9b7bac392e0c08121d96eea7176df28855ee -->
<!-- api: M:Inno.Audio.IAudioService.ReleasePreload(Inno.Audio.AudioClipAsset) 5a91a566d8d239cf612557f1107ef851ba6a3153df64179a3d880673c7a207d5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e323347ceb7cbfec"></a>[`bool Inno.Audio.IAudioService.Pause(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L104) | Pauses a live voice.<br>返回：true when the voice entered the paused state.<br>`voice`：Voice handle to pause. | — | — |
| <a id="api-fe7be6809f7f363b"></a>[`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L58) | Starts one clip using explicit playback parameters.<br>返回：A voice handle that may initially be in the preparing state.<br>`clip`：Imported clip to prepare and play.<br>`options`：Immutable playback parameters. | — | `System.ArgumentException`：Playback options are uninitialized; construct options instead of passing a default struct. |
| <a id="api-8e05e8c639651ad9"></a>[`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.Play(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L41) | Starts one clip using default playback parameters.<br>返回：A voice handle that may initially be in the preparing state.<br>`clip`：Imported clip to prepare and play. | — | — |
| <a id="api-fc9f6ae536180ebb"></a>[`Inno.Audio.AudioVoiceHandle Inno.Audio.IAudioService.PlayScheduled(Inno.Audio.AudioClipAsset clip, double scheduledDspTime, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L78) | Schedules one clip against the monotonic audio clock.<br>返回：A voice handle in preparing or scheduled state.<br>`clip`：Imported clip to prepare and play.<br>`scheduledDspTime`：Absolute audio-clock start time in seconds.<br>`options`：Immutable playback parameters. | — | — |
| <a id="api-356cd1a46009f6eb"></a>[`System.Threading.Tasks.ValueTask Inno.Audio.IAudioService.PreloadAsync(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/audio/Inno.Audio/IAudioService.cs#L234) | Prepares a clip and retains it in the decoded or streamed cache.<br>返回：An asynchronous operation that completes after the cache entry is ready.<br>`clip`：Imported clip to prepare.<br>`loadMode`：Storage strategy override.<br>`cancellationToken`：Cancellation token observed before the cache commit. | — | — |
| <a id="api-f2d0369641916dd9"></a>[`void Inno.Audio.IAudioService.ReleasePreload(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L246) | Releases one explicit preload retention without interrupting active voices.<br>`clip`：Imported clip whose preload retention should be released. | — | — |
| <a id="api-db6324c53b1fad72"></a>[`bool Inno.Audio.IAudioService.Resume(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L115) | Resumes a paused voice.<br>返回：true when the voice resumed.<br>`voice`：Voice handle to resume. | — | — |
| <a id="api-a8612f57c497150c"></a>[`bool Inno.Audio.IAudioService.Seek(Inno.Audio.AudioVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L129) | Moves a live voice cursor to a clip-relative position.<br>返回：true when the cursor was updated.<br>`voice`：Voice handle to seek.<br>`position`：Non-negative clip-relative position. | — | — |
| <a id="api-49463fc7f7f7801a"></a>[`bool Inno.Audio.IAudioService.SetBusMuted(Inno.Audio.AudioBusId bus, bool muted)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L197) | Updates mute state for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`muted`：Whether output from the bus is silenced. | — | — |
| <a id="api-f50e357d3f534faf"></a>[`bool Inno.Audio.IAudioService.SetBusPaused(Inno.Audio.AudioBusId bus, bool paused)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L214) | Updates pause state for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`paused`：Whether routed voices should pause. | — | — |
| <a id="api-5811571adaa7841b"></a>[`bool Inno.Audio.IAudioService.SetBusVolume(Inno.Audio.AudioBusId bus, float volume)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L180) | Updates linear gain for a semantic mixer bus.<br>返回：true when the bus exists and was updated.<br>`bus`：Stable bus identifier.<br>`volume`：Non-negative linear gain. | — | — |
| <a id="api-2a546ee2e71d417e"></a>[`bool Inno.Audio.IAudioService.SetVoiceParameters(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L146) | Replaces mutable parameters for a live voice.<br>返回：true when a live voice was updated; false for stale handles or uninitialized parameters.<br>`voice`：Voice handle to update.<br>`parameters`：Current voice parameter snapshot. | — | — |
| <a id="api-280c2bdef3e6ace0"></a>[`bool Inno.Audio.IAudioService.Stop(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L93) | Stops a live voice.<br>返回：true when a live voice was stopped.<br>`voice`：Voice handle to stop. | — | — |
| <a id="api-92073b2bb880f0b2"></a>[`bool Inno.Audio.IAudioService.TryGetVoiceState(Inno.Audio.AudioVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio/IAudioService.cs#L163) | Queries the current state of a voice.<br>返回：true when the handle belongs to a known voice.<br>`voice`：Voice handle to query.<br>`playbackState`：Receives the current state when the handle is known. | — | — |
| <a id="api-0bac2f2d8924f93c"></a>[`Inno.Audio.AudioCapabilities Inno.Audio.IAudioService.capabilities { get; }`](../../src/services/audio/Inno.Audio/IAudioService.cs#L15) | Gets immutable capabilities for the current device generation. | — | — |
| <a id="api-f744db1f569a3bee"></a>[`Inno.Audio.AudioDeviceState Inno.Audio.IAudioService.deviceState { get; }`](../../src/services/audio/Inno.Audio/IAudioService.cs#L20) | Gets the current audio output state. | — | — |
| <a id="api-475a0b558f446b49"></a>[`double Inno.Audio.IAudioService.dspTime { get; }`](../../src/services/audio/Inno.Audio/IAudioService.cs#L25) | Gets the monotonic audio clock in seconds. | — | — |
| <a id="api-b2930276a37036bc"></a>[`Inno.Audio.AudioStatistics Inno.Audio.IAudioService.statistics { get; }`](../../src/services/audio/Inno.Audio/IAudioService.cs#L30) | Gets current runtime and backend resource statistics. | — | — |
| <a id="api-ed709029e1422031"></a>[`public interface Inno.Audio.IAudioService`](../../src/services/audio/Inno.Audio/IAudioService.cs#L10) | Provides backend-neutral audio playback, caching, mixer, and device-state services. | — | — |

### `Inno.Audio.SerializedAudioExtensionState`

<!-- api: T:Inno.Audio.SerializedAudioExtensionState f8c96af50e555199ea241bc1e7feb30ac3e191124cd9d597e7e4a970290b1232 -->
<!-- api: M:Inno.Audio.SerializedAudioExtensionState.#ctor 39b1a1dcd16ed09e9d60de3d9154f136cec4d9808434137624609008340bf444 -->
<!-- api: M:Inno.Audio.SerializedAudioExtensionState.#ctor(System.Guid,System.ReadOnlySpan{System.Byte}) 06dd4a3ce80b7cd061a1ea5ae9195d12b01edaf0999a28b3902c40027a773739 -->
<!-- api: P:Inno.Audio.SerializedAudioExtensionState.stableTypeId 5420968ab24e11c103ec39da1d7787dfe15c50180932e19cb63b323731c4c4c4 -->
<!-- api: P:Inno.Audio.SerializedAudioExtensionState.propertyData dfd9286c4b5d204035fa0d69acee3e6da9d3bd08d05d4d70a458166b610dfaf0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5786e498e12a2f7f"></a>[`public Inno.Audio.SerializedAudioExtensionState.SerializedAudioExtensionState()`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L291) | Creates empty extension state for deserialization. | — | — |
| <a id="api-ad835e254c2e3bb7"></a>[`public Inno.Audio.SerializedAudioExtensionState.SerializedAudioExtensionState(System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L305) | Creates neutral extension state.<br>`stableTypeId`：Stable settings type identity, or empty when no typed settings exist.<br>`propertyData`：Neutral serialized property bytes. | — | — |
| <a id="api-a7a82fc2f1d46a9a"></a>[`public byte[] Inno.Audio.SerializedAudioExtensionState.propertyData { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L323) | Gets or sets neutral serialized property bytes. | — | — |
| <a id="api-e6fa468bfab4895a"></a>[`public System.Guid Inno.Audio.SerializedAudioExtensionState.stableTypeId { get; set; }`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L317) | Gets or sets the stable settings type identity. | — | — |
| <a id="api-634da0b0565b298b"></a>[`public struct Inno.Audio.SerializedAudioExtensionState`](../../src/services/audio/Inno.Audio/AudioMixer.cs#L286) | Stores reload-safe configuration for one mixer or feature extension generation. | — | — |

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
