# Inno.Audio.Runtime

## 宿主暂停

Attach 后，Runtime 通过 Core Events 的 `EventDispatcher.dispatched` 完成观察入口监听 `ApplicationSuspensionChangedEvent`，即使事件被全局消费也会收到系统生命周期通知。
暂停作用于 active 和仍被 voice 使用的 retired mixer master bus；用户设置的 paused 值独立保留，
恢复不会清除它。设备替换与 mixer 替换继承当前宿主暂停状态。
Backend 拒绝暂停时回滚已经改变的 master bus 并抛出；回滚失败使 generation Faulted。
停止时先退订完成观察回调，再退休输出资源；没有额外平台事件总线或平台判断。

## Provider 所有权与恢复诊断

Provider 实例在 `AudioExtensionRegistry` 的 TypeCatalog 候选快照构造阶段创建，由该快照独占释放；Runtime 不再维护第二套延迟创建/释放的 Provider generation。所有 Provider 都尝试退休，清理失败经 TypeRegistry 将共享 generation gate 置为 Faulted。它不是可回滚的普通候选失败。

Mixer 恢复成功会移除先前该配置的 Missing/候选失败诊断。旧 Bus 仅等待实际路由到它的 Voice，而不是等待整个引擎静音；安装和 Update 安全点均可回收已无 Voice 的代际。

`AudioRuntimeOptions.maxPendingPreloads` 默认 256，限制尚未完成的 preload 等待者。超限明确拒绝，停止后不能通过 Bus 控制入口访问已释放设备。

`maxContentSnapshots` 默认 1024，限制每次 Update 中所有 Provider 成功贡献的 emitter + listener 总量，
不等同于播放 Voice 预算。Runtime 按 priority、stable provider ID 顺序逐个分配剩余容量；Provider 单独收集，
成功返回且全部 identity 验证通过后才整批接纳。失败、超限、内部吞掉提交异常或跨 Provider 重复，都丢弃该 Provider
的全部贡献，发布 `AUDIO_CONTENT_PROVIDER_FAILED`，不占用下一 Provider 的容量。下次成功后诊断自动解除。
该 Provider 在本帧没有贡献的旧 emitter 按常规 absent-content 规则停止，而非继续播放部分失败数据。

每个 provider context 在调用结束后撤销并清除 Clip 引用，整个 host content scope 在 Update 返回时关闭。
`AudioRuntime.contentStatistics` 返回最近一次 collection 的 `AudioContentStatistics`：`capacity`、
`acceptedSnapshots`、`rejectedProviders`，只包含中立计数，无 Provider/Scene/Asset 引用；它是 Host 诊断入口，
不扩展设备 ABI 或游戏 `Audio.statistics`。设备 generation 替换后计数重新开始。
`RetirementPendingException` 穿过内容隔离边界，不伪装成一般 Provider 失败。

内部资源由明确的 owner 独占，不属于 public API，也不是 partial 文件拆分：

| Owner | 独占状态与责任 |
| --- | --- |
| `AudioClipCache` | 原生 Clip、Artifact lease、preload waiter、decoded budget。 |
| `AudioVoiceOwner` | Runtime/backend handle 映射、调度、抢占、终止状态、Clip 引用与完成事件。 |
| `AudioMixerOwner` | Bus、控制状态、候选 graph、被 Voice 保留的旧 graph。 |
| `AudioContentOwner` | Provider 的单次内容消费、emitter 增量记录、listener 选择与诊断。 |
| `AudioDeviceCandidate` | 未发布设备及其 Mixer 的准备、所有权转交与失败退休。 |
| `AudioRuntime` | `IAudioService` 门面、owner 组合、帧顺序、设备恢复与生命周期。 |

普通退休失败会继续尝试其余资源并聚合；`RetirementPendingException` 不属于普通失败：保留当前 owner、Clip Artifact lease、未处理 Bus 和下层 device，重试从当前阶段继续，禁止继续播放。Provider 也必须重试真实 Dispose hook，不能提前置 disposed。设备替换使用 Core retirement deadline；超时由 TypeRegistry 封锁共享 generation gate，不能包装成可恢复的 `false`。

完成事件入队遇到背压时保留终止中的 Voice，下一次安全点重试；已释放的 Clip 引用不会重复递减，事件只成功发布一次。终止操作仍可能报告明确背压错误，Host 需要排空当前 EventDispatcher 后继续。

新请求先校验显式 options 并取得 Artifact 请求，再执行预算抢占；未初始化 options 或 Artifact acquisition 的直接
Pending 不改变旧 Voice。普通 Missing/metadata/解码失败仍遵循 Preparing → DecodeFailed 完成事件，不改成同步
异常，也不承诺所有异步失败都不会消耗 Voice 预算。抢占遇到事件背压时，新请求的 Artifact 交给共享 generation
retirement barrier 释放；重试旧 Voice 的完成步骤，不重复 Stop 或增加 stolen 统计。

每个缓存项内部拥有 Core `LifetimeScope`：先释放 native Clip，再释放 Artifact，Pending 时保持当前步骤。
已开始退休的条目不再被新 Voice 复用；重试不会再次销毁已成功退出的 Clip。ReleasePreload 的计数只扣一次，
待退出条目由后续 Update 安全点继续处理。Update 中取消/失败的异步 preload 在资源退出前保留 waiter，不提前报告完成或
把 Pending 变成解码错误。命中已有缓存时，新请求多取得的 Artifact 也经共享 retirement barrier 清理。

首次 `GetClipState` 成功且未返回 Failed 后才增加 preload 引用。查询异常或准备失败会退休本次新建且无人引用的
cache entry；已有 Voice/preload 的缓存项保持原引用。回滚中 Pending 使用相同 barrier 排空，普通回滚失败与
原查询异常一起保留并 Fault 共享 generation，不能以“准备失败”掩盖资源清理失败。

`ReplaceDevice` 消费已验证的 replacement：候选图构建失败会释放候选，保留旧设备；开始退休旧设备后发生的失败不可伪回滚，会尝试全部资源清理、释放候选并 Fault 共享 generation。`TryRecoverDevice` 不会把这一类失败转换为 `false`。成功恢复会撤销 no-device、device-lost 和 recovery-failed 状态诊断。

## 本轮统一收口

`AudioRuntime : RuntimeSubsystem, IAudioService` 组合 IAudioDevice，不继承设备。公共 Voice 使用 `AudioVoiceHandle` 与 runtime owner generation；backend Voice 使用独立 `AudioDeviceVoiceHandle` 与 device generation，不能互换。

`PreloadAsync` 现在启动真实 native 准备，后端通过 `GetClipState` 返回 Preparing/Ready/Failed；等待者只在准备成功或失败时完成。Preparing 期间通过 Update 安全点处理取消与完成，关闭/设备替换会取消未完成等待。不得在停止 tick 的 owner thread 上同步等待 preload。

Play 立即返回 Preparing 句柄。请求捕获 metadata、内容 revision 和 `audio-data` 的 ArtifactLease，不跨帧持有 AudioClipAsset。Clip cache 按 persistent ID、content revision、artifact key 和 load mode 区分；租约在最后一个 voice/preload 释放后退休。Emitter record 同样只保存 ID 与 revision，避免比较同一个被原位更新的 Asset 引用而漏掉变化。

所有问题进入注入的 `IDiagnosticReporter`，内容提供者读取统一 `ContentReadScope`。Session 的 reporter/registration 由 pipeline construction lifetime 持有。Runtime options 在构造时复制，修改调用者的 options 不会偷改活动预算。

[Audio 索引](README.md) · [Core contract](Inno.Audio.md) · [MiniAudio](../backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.md)

`Inno.Audio.Runtime` 实现控制线程上的播放编排，不认识 Scene、Editor 或具体后端。`AudioRuntime` 同时实现 `IAudioService`，并拥有一个 `IAudioDevice` generation。

## 公开 API

| 类型 | 作用 |
| --- | --- |
| `AudioRuntime` | 播放/预加载、Mixer 安装、设备替换/恢复、Provider 同步、每帧安全点更新与释放。 |
| `AudioRuntimeOptions` | `maxVoices`、`maxPendingPreloads`、`maxContentSnapshots`、decoded cache budget、automatic stream threshold 与 device recovery interval。 |
| `AudioContentStatistics` | 最近一次内容收集的容量、接纳 snapshot 数与拒绝 Provider 数；通过 `AudioRuntime.contentStatistics` 读取。 |
| `AudioProjectSettings`（Inno.Audio） | 可部署的 default mixer、master volume 与 Runtime budgets。 |
| `MutedAudioDevice` | 无声卡或测试环境下明确的静音设备；时钟、状态和完成仍推进。 |
| `ContentReadScope`（Inno.References） | Host 提供的一帧 Identity-backed 内容边界，使用后必须释放。 |
| `AudioEmitterSnapshot`、`AudioListenerSnapshot` | 不含 Scene 类型的 emitter/listener 状态。 |
| `AudioContentProviderExtensionAttribute`、`AudioContentProvider`、`AudioContentProviderContext` | TypeCatalog 自动发现的控制线程扩展协议。 |

## 更新与 generation

TypeCatalog 在统一候选事务中准备和退休 Provider；`Update(deltaTime)` 在控制线程消费已发布 generation、同步 emitter/listener、推进 backend、收集完成并派发事件。候选构造失败且清理成功时保留 last-good；清理失败必须 Faulted。不得在音频 callback 中刷新 Catalog。

Voice stealing 在 `maxVoices` 达到上限时按 priority、创建序列和稳定 handle 确定性选择。Clip cache 按 Asset persistent ID、content generation 和 load mode 分键；预加载引用与 live voice 引用独立，旧不可变 Artifact 会保留到使用它的 Voice 结束。Automatic 会在 encoded threshold 或估算 decoded footprint 超过预算时选择 Stream；显式 Decode 超过预算则明确失败。

```csharp
using Inno.Audio.Runtime;

static void UpdateOwnedStandaloneAudio(
    AudioRuntime audio,
    float deltaTime
) {
    using (audio.EnterExecutionScope())
        audio.Update(deltaTime);
}
```

MiniAudio 原生线程不执行 provider、反射、Asset IO、托管日志、锁或托管 DSP callback。扩展只在控制线程提交中立 snapshot/graph，设备只消费已构建状态。

## 失败语义

- 设备初始化失败由组合根显式建立 `MutedAudioDevice` 并发布诊断，不伪装为正常输出。
- `ApplyMixer` 的候选无效时返回 `false` 并保留当前 graph。
- `ReplaceDevice` 使旧 generation 句柄失效，并在候选设备上先重建 last-good Mixer graph；设备丢失的 Voice 以明确 completion reason 收敛。
- Host 提供 recovery factory 时，Runtime 按 `deviceRecoveryIntervalSeconds` 在 `Update` 安全点重试；候选初始化或 graph 构建失败不会替换当前 generation，成功后保留 Bus volume/mute/pause 控制状态。
- Dispose 停止 Voice、释放 Clip/Bus/Listener、provider generation 和设备，重复释放安全。
- `Update` 在执行 Provider 前拒绝负数、NaN 与无限 deltaTime，不将无效时间静默改为零。

`MutedAudioDevice(AudioDeviceLimits? limits = null)` 使用与官方 native backend 一致的 Clip/Bus/Voice + completion 接纳限制。静音不会意味着无限积累；调用者仍必须通过 Update 和 TryDequeueCompletion 推进生命周期。






## 本轮边界与所有权

Runtime 从 ArtifactLease 创建编码来源并交给设备；AudioClipCache 不取得物理路径。准备任务受取消与 owner-thread 发布控制；退出先取消并完成准备，再退休 native clip 并释放 source lease。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Audio.Runtime.AudioContentStatistics`

<!-- api: T:Inno.Audio.Runtime.AudioContentStatistics b826dcfe0a36c856198d3a18c67642b68663a68f84f435d42649f7be92bc18cf -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.#ctor(System.Int32,System.Int32,System.Int32) 47e451a5011169bc786109b31c54d18e567a930a54d6378b5ccf7b1a99b944a1 -->
<!-- api: P:Inno.Audio.Runtime.AudioContentStatistics.capacity 86f1657419124e16a7dd5625bc7c63df4eb41098980241e54abb214b12cddac1 -->
<!-- api: P:Inno.Audio.Runtime.AudioContentStatistics.acceptedSnapshots 53253e232c653c0751053ea3486e0e5729a8ec465937bdf9769b97e156a1d430 -->
<!-- api: P:Inno.Audio.Runtime.AudioContentStatistics.rejectedProviders ac3586c30caef6af96efb7a21656bb3d024dd57e64072499308666a181538369 -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.ToString a1968cbb3648abee4a768ce61bfbc091310d14c8bc67ae9736e5be52275258cf -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.op_Inequality(Inno.Audio.Runtime.AudioContentStatistics,Inno.Audio.Runtime.AudioContentStatistics) fd850ddb044917bd8ad92391a1fa19fb9dc4765e0556bff6feb9a631ef3b8c04 -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.op_Equality(Inno.Audio.Runtime.AudioContentStatistics,Inno.Audio.Runtime.AudioContentStatistics) f05f3d21778a95db4cf05c5479c4dd78132306070ac540b54849417fe17054b4 -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.GetHashCode de807fcc7ffaa97623063784d758766a165245d76dc8b34e5442c5b97730ce04 -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.Equals(System.Object) 3163462d011f83aa2c9b49dfe1c146262928b8fdd09bac4484d9d4e8ce8b6bce -->
<!-- api: M:Inno.Audio.Runtime.AudioContentStatistics.Equals(Inno.Audio.Runtime.AudioContentStatistics) eaa4f862449b3ab1d7ec8607d5a6b314213f671aa6d13bd392b14f68a4a1afc3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ab87a63427cb06e1"></a>[`public Inno.Audio.Runtime.AudioContentStatistics.AudioContentStatistics(int capacity, int acceptedSnapshots, int rejectedProviders)`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L25) | Creates a content admission snapshot without retaining providers or host content.<br>`capacity`：Combined emitter and listener budget available to the update.<br>`acceptedSnapshots`：Snapshots accepted from fully validated provider contributions.<br>`rejectedProviders`：Provider contributions discarded because submission, validation, or capacity admission failed. | — | `System.ArgumentOutOfRangeException`：A count is negative or accepted snapshots exceed capacity. |
| <a id="api-dcea8e9bd6b51298"></a>[`public bool Inno.Audio.Runtime.AudioContentStatistics.Equals(Inno.Audio.Runtime.AudioContentStatistics other)`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-22056d7dbfa3576c"></a>[`public override bool Inno.Audio.Runtime.AudioContentStatistics.Equals(object obj)`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1a15b40d1163ed1e"></a>[`public override int Inno.Audio.Runtime.AudioContentStatistics.GetHashCode()`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-784a15d3f1b0782a"></a>[`public static bool Inno.Audio.Runtime.AudioContentStatistics.operator ==(Inno.Audio.Runtime.AudioContentStatistics left, Inno.Audio.Runtime.AudioContentStatistics right)`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-fd1aa9780e42dddb"></a>[`public static bool Inno.Audio.Runtime.AudioContentStatistics.operator !=(Inno.Audio.Runtime.AudioContentStatistics left, Inno.Audio.Runtime.AudioContentStatistics right)`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-470ad72048bbb7a4"></a>[`public override string Inno.Audio.Runtime.AudioContentStatistics.ToString()`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-14917d171719f53b"></a>[`public int Inno.Audio.Runtime.AudioContentStatistics.acceptedSnapshots { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L47) | Gets accepted snapshots, bounded by capacity and excluding every rejected partial contribution. | — | — |
| <a id="api-c3649d021501c35f"></a>[`public int Inno.Audio.Runtime.AudioContentStatistics.capacity { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L42) | Gets the immutable combined snapshot budget assigned to the runtime. | — | — |
| <a id="api-28478e617ffcf517"></a>[`public int Inno.Audio.Runtime.AudioContentStatistics.rejectedProviders { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L52) | Gets rejected contributions in this collection, including overflow and duplicate identities. | — | — |
| <a id="api-969c5c3bee093916"></a>[`public readonly record struct Inno.Audio.Runtime.AudioContentStatistics : System.IEquatable<Inno.Audio.Runtime.AudioContentStatistics>`](../../src/services/audio/Inno.Audio.Runtime/AudioContentStatistics.cs#L8) | Reports immutable content admission pressure from the most recent control-thread collection. | — | — |

### `Inno.Audio.Runtime.AudioRuntime`

<!-- api: T:Inno.Audio.Runtime.AudioRuntime 3d0ed881446d02f01f7635ad44cfe6154287ce83ddd9355cba36b35b0e929d52 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.#ctor(Inno.Extensibility.Types.TypeCatalog,Inno.Audio.IAudioDevice,Inno.Assets.IAssetArtifactLookup,Inno.Core.Events.EventDispatcher,Inno.Core.Diagnostics.IDiagnosticReporter,Inno.Audio.Runtime.AudioRuntimeOptions,System.Func{Inno.References.ContentReadScope},System.Func{Inno.Audio.IAudioDevice}) 6bf56cf8031d8ed7538b21499b6757835dfee275b0cded264f4b047fab871ba0 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntime.capabilities c77d6558b0bf615afd30a111e157c8d32d7c778f3656dbb5fca63495caad143a -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntime.deviceState d70ab9d3bcdbcf1516729b8af1f7a44ee0552e7127585d9d41f67db27bb9d48e -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntime.dspTime c248d5089c6a686a6c2436d24a46c35f951c4072834e43d46e5d581a39d24318 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.OnStart d355e59dd11daeb7847ac4d9b85110398978bb74c59118c318ff03d3cb572dd9 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) b4d3b076449d8ba7a0edc7bb58bfd4a547f9933b4708831a2766e84a60a2b3a9 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame) 75b5a6ee73ffd47301bed38e7dcb0e01b82b6d164c391fb7d0a7d1db4664a897 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntime.statistics bff3858a68e163f92451c79ff4c0f161a5db15e0c7f3cfcfab8538d41b95d3a2 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntime.contentStatistics 96857cdf262c75e7fab798f8dcf3ff1e1c953c26e20172114eab7b3d29976d5b -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.EnterExecutionScope d28d6c4bc252b38a92aeb7c5dcfdc2c459707cff27eeda97ee1762c4d651ce84 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Play(Inno.Audio.AudioClipAsset) b6a4ac4e6ba6bde5acede193dddd8dab930fd667a55322d4130f248eb3d74870 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Play(Inno.Audio.AudioClipAsset,Inno.Audio.AudioPlayOptions) 14c497ed6306a71b1e00512349b120db0ff45f1ca6009bb47a3b812fa40b836b -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.PlayScheduled(Inno.Audio.AudioClipAsset,System.Double,Inno.Audio.AudioPlayOptions) a52cdecce286cf9f01fd5358906870ca4d3f221bed9387abd35d36754134f9f4 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Stop(Inno.Audio.AudioVoiceHandle) abd50c4d86f7db4a5fb4eaea64125685103bd8e954041dfd5284f38846b15aac -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Pause(Inno.Audio.AudioVoiceHandle) b3fa58e413efcdbb9ec048413ca936faaf188e6586962e18f1b8a1e99694d868 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Resume(Inno.Audio.AudioVoiceHandle) d8f121b2e1ef706584c999010953e28c07c2df015cdd2ddcc1e4054504e592cd -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Seek(Inno.Audio.AudioVoiceHandle,System.TimeSpan) 360e8d599bd85db56dd8ba682fba5aeb4bde33e268c614e4536fc300820ae6fd -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.SetVoiceParameters(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioVoiceParameters) 6ceddf38160ede907569bedc5e3bbfec5e84d78c90ffffbb5ea75781b8bacdb9 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.TryGetVoiceState(Inno.Audio.AudioVoiceHandle,Inno.Audio.AudioPlaybackState@) e400d146bbe59c7a79ac68cbd82ad5b582fbb3b7687584aab5fcfb8404a58e7e -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.SetBusVolume(Inno.Audio.AudioBusId,System.Single) 9387b1bc889cb7910859cc5ad44217014c88b2c409eabb59f13bce59c42fcb0e -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.SetBusMuted(Inno.Audio.AudioBusId,System.Boolean) 00652d16f3409e5a3a3fd2dd559880267659c7434931efaef1ffb029898012a4 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.SetBusPaused(Inno.Audio.AudioBusId,System.Boolean) e95d82a6c312d9cc6ab90e3af3a17ad3e7c16142580a9692af615e5bd3078ef8 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.PreloadAsync(Inno.Audio.AudioClipAsset,Inno.Audio.AudioClipLoadMode,System.Threading.CancellationToken) a356d0f90c4d221c1b5b44d4b70a83e033ea41d97f8615da6c28af981a52b2ca -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.ReleasePreload(Inno.Audio.AudioClipAsset) 21cd682fcc1265fa52e8cf7b32c0996224837358f39fcbdd3b4b3d3fb53562fc -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.ApplyMixer(Inno.Audio.AudioMixerAsset) 81438857f740f87f9837fb2f6d7b8d2bdb2dc7283e514e5959d7eff4b6d46bfb -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.ReplaceDevice(Inno.Audio.IAudioDevice) b2555fe7bdb91fd62d696485d9c7355d216aa8e4bd72acd83ee1586d8f86b05b -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.TryRecoverDevice(System.Func{Inno.Audio.IAudioDevice}) 86cf68ae4f59e55426cad3dab10fc05510d7517477007dae994723db32079e0f -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.Update(System.Single) 893005f4087e2ec4c2fd4f306634707cea5f5a6ab5dc389850579f0710cc36a0 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntime.OnStop 96faddfd336798992155302acc38566b6463a163413d2634522f8e6b008ecdd6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a62df8b8f0045cc5"></a>[`public Inno.Audio.Runtime.AudioRuntime.AudioRuntime(Inno.Extensibility.Types.TypeCatalog types, Inno.Audio.IAudioDevice device, Inno.Assets.IAssetArtifactLookup artifacts, Inno.Core.Events.EventDispatcher events, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, Inno.Audio.Runtime.AudioRuntimeOptions? options = null, System.Func<Inno.References.ContentReadScope>? contentScopeProvider = null, System.Func<Inno.Audio.IAudioDevice>? deviceRecoveryFactory = null)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L78) | Creates an audio runtime over one backend device generation.<br>`types`：Type catalog used for mixer and content-provider discovery.<br>`device`：Backend device whose ownership transfers to this runtime.<br>`artifacts`：Lookup for verified immutable audio-data artifacts.<br>`events`：Main-thread event dispatcher that receives voice completion events.<br>`diagnostics`：Required owner-bound diagnostic producer.<br>`options`：Optional bounded runtime resource policies.<br>`contentScopeProvider`：Optional callback that supplies update-scoped host content without coupling Runtime to Scene.<br>`deviceRecoveryFactory`：Optional host factory used to retry a real output device at main-thread update safety points. | — | `System.ArgumentNullException`：A required device, catalog, artifact lookup, event dispatcher, or diagnostic reporter is null.<br>`System.ArgumentOutOfRangeException`：A voice, preload, or content limit is not positive, a byte budget is negative, or the recovery delay is invalid. |
| <a id="api-abf5b4d5e73c37fa"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.ApplyMixer(Inno.Audio.AudioMixerAsset asset)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L458) | Builds and atomically installs a mixer graph from reloadable extensions.<br>返回：true when every extension resolved and the candidate became active.<br>`asset`：Mixer asset containing stable extension identifiers and neutral state. | — | — |
| <a id="api-322e89e01c8fffa2"></a>[`public System.IDisposable Inno.Audio.Runtime.AudioRuntime.EnterExecutionScope()`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L182) | Binds this runtime to script-facing audio APIs for the current asynchronous flow.<br>返回：A strict last-in-first-out execution scope owned by the caller. | — | `System.ObjectDisposedException`：Thrown when this runtime has been disposed. |
| <a id="api-0367b85500513492"></a>[`protected override void Inno.Audio.Runtime.AudioRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L149) | Captures snapshots and binds service façades.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-fc62888ecfc2fff6"></a>[`protected override void Inno.Audio.Runtime.AudioRuntime.OnStart()`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L139) | Acquires resources after dependencies have started. | — | — |
| <a id="api-80ecf30ccf0c1fab"></a>[`protected override void Inno.Audio.Runtime.AudioRuntime.OnStop()`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L703) | Stops voices and releases providers, cache entries, mixer generations, and the owned backend device. | — | — |
| <a id="api-5805be4a9933b10e"></a>[`protected override void Inno.Audio.Runtime.AudioRuntime.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L156) | Advances domain state on the variable clock.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-b5094e6c333bb0d7"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.Pause(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L272) | Pauses a live or preparing voice.<br>返回：true when the voice entered the paused state.<br>`voice`：Runtime voice handle to pause. | — | — |
| <a id="api-f310d8e3960c795a"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Audio.Runtime.AudioRuntime.Play(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L214) | Starts one clip using explicit playback parameters.<br>返回：A preparing voice handle owned by the current device generation.<br>`clip`：Imported clip to prepare and play.<br>`options`：Immutable playback parameters. | — | `System.ArgumentException`：Playback options are uninitialized. The request is rejected before asset acquisition or voice stealing. |
| <a id="api-5c6a28a7d440b072"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Audio.Runtime.AudioRuntime.Play(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L197) | Starts one clip using default playback parameters.<br>返回：A preparing voice handle owned by the current device generation.<br>`clip`：Imported clip to prepare and play. | — | — |
| <a id="api-932866468aba7368"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Audio.Runtime.AudioRuntime.PlayScheduled(Inno.Audio.AudioClipAsset clip, double scheduledDspTime, Inno.Audio.AudioPlayOptions options)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L237) | Schedules one clip against the monotonic audio clock.<br>返回：A preparing voice handle owned by the current device generation.<br>`clip`：Imported clip to prepare and play.<br>`scheduledDspTime`：Absolute audio-clock start time in seconds.<br>`options`：Immutable playback parameters. | — | — |
| <a id="api-b908eae9c021c0e8"></a>[`public System.Threading.Tasks.ValueTask Inno.Audio.Runtime.AudioRuntime.PreloadAsync(Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioClipLoadMode loadMode = Inno.Audio.AudioClipLoadMode.Automatic, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L428) | Prepares and explicitly retains a clip cache entry.<br>返回：An operation completed at an update safety point after native preparation succeeds or fails.<br>`clip`：Imported clip to retain.<br>`loadMode`：Storage strategy override.<br>`cancellationToken`：Cancellation token observed before the cache commit. | — | — |
| <a id="api-56ec85e7ace701c1"></a>[`public void Inno.Audio.Runtime.AudioRuntime.ReleasePreload(Inno.Audio.AudioClipAsset clip)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L443) | Releases one explicit preload retention without interrupting voices.<br>`clip`：Imported clip whose preload retention should be released. | — | — |
| <a id="api-e98c8c8cb9dacc02"></a>[`public void Inno.Audio.Runtime.AudioRuntime.ReplaceDevice(Inno.Audio.IAudioDevice replacement)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L506) | Replaces a lost or muted backend at a main-thread safety point.<br>`replacement`：Initialized replacement device, consumed on success and disposed if preparation fails. | — | `System.AggregateException`：Retirement failed after publication preparation; the generation gate is Faulted and the host must restart.<br>`Inno.Core.Execution.RetirementTimeoutException`：Old or candidate work did not drain before its deadline. Both owners remain retained and the host must restart. |
| <a id="api-12f4e76ea6892478"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.Resume(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L287) | Resumes a paused voice.<br>返回：true when the voice resumed or returned to preparation.<br>`voice`：Runtime voice handle to resume. | — | — |
| <a id="api-452dddf0d7efae14"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.Seek(Inno.Audio.AudioVoiceHandle voice, System.TimeSpan position)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L305) | Moves a live or preparing voice cursor to a clip-relative position.<br>返回：true when the cursor request was accepted.<br>`voice`：Runtime voice handle to seek.<br>`position`：Non-negative clip-relative position. | — | — |
| <a id="api-1dbbbfd553de2433"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.SetBusMuted(Inno.Audio.AudioBusId bus, bool muted)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L385) | Updates mute state for a semantic mixer bus.<br>返回：true when the active mixer contains the bus.<br>`bus`：Stable bus identifier.<br>`muted`：Whether output from the bus is silenced. | — | — |
| <a id="api-c42e3b3304818efa"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.SetBusPaused(Inno.Audio.AudioBusId bus, bool paused)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L405) | Updates pause state for a semantic mixer bus.<br>返回：true when the active mixer contains the bus.<br>`bus`：Stable bus identifier.<br>`paused`：Whether routed voices should pause. | — | — |
| <a id="api-9fd38b5f5abb3219"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.SetBusVolume(Inno.Audio.AudioBusId bus, float volume)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L365) | Updates linear gain for a semantic mixer bus.<br>返回：true when the active mixer contains the bus.<br>`bus`：Stable bus identifier.<br>`volume`：Non-negative linear gain. | — | — |
| <a id="api-e90f04f17c0e4229"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.SetVoiceParameters(Inno.Audio.AudioVoiceHandle voice, Inno.Audio.AudioVoiceParameters parameters)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L325) | Replaces mutable parameters for a live or preparing voice.<br>返回：true when the parameter request was accepted.<br>`voice`：Runtime voice handle to update.<br>`parameters`：Current voice parameter snapshot. | — | — |
| <a id="api-2c24964424cb669c"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.Stop(Inno.Audio.AudioVoiceHandle voice)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L257) | Stops a live or preparing voice.<br>返回：true when a live voice was stopped.<br>`voice`：Runtime voice handle to stop. | — | — |
| <a id="api-4be96d5f81ae35ec"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.TryGetVoiceState(Inno.Audio.AudioVoiceHandle voice, out Inno.Audio.AudioPlaybackState playbackState)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L345) | Queries the current state of a runtime voice.<br>返回：true for active and terminal handles from this runtime generation.<br>`voice`：Runtime voice handle to query.<br>`playbackState`：Receives the current state when the handle is known. | — | — |
| <a id="api-6cc2b881a9f1698d"></a>[`public bool Inno.Audio.Runtime.AudioRuntime.TryRecoverDevice(System.Func<Inno.Audio.IAudioDevice> deviceFactory)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L588) | Attempts to replace a muted or lost output generation while preserving the active mixer graph.<br>返回：true when output is already ready or a ready candidate was installed; otherwise false and the current muted or lost generation remains active.<br>`deviceFactory`：Host-owned factory that creates a fresh initialized device candidate. | — | — |
| <a id="api-bff47068c54035e9"></a>[`public void Inno.Audio.Runtime.AudioRuntime.Update(float deltaTime)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L670) | Advances providers, pending preparation, backend maintenance, and completion dispatch at a main-thread safety point.<br>`deltaTime`：Non-negative elapsed frame time in seconds. | — | `System.ArgumentOutOfRangeException`：The elapsed time is negative or non-finite. |
| <a id="api-5158187c329a9da8"></a>[`public Inno.Audio.AudioCapabilities Inno.Audio.Runtime.AudioRuntime.capabilities { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L126) | Gets immutable capabilities for the current backend generation. | — | — |
| <a id="api-f9aadf9289af075b"></a>[`public Inno.Audio.Runtime.AudioContentStatistics Inno.Audio.Runtime.AudioRuntime.contentStatistics { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L171) | Gets bounded provider admission counts from the most recent content collection on this device generation. | — | — |
| <a id="api-34e784df21599043"></a>[`public Inno.Audio.AudioDeviceState Inno.Audio.Runtime.AudioRuntime.deviceState { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L131) | Gets the current output availability state. | — | — |
| <a id="api-7c29dd59f4eb23f6"></a>[`public double Inno.Audio.Runtime.AudioRuntime.dspTime { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L136) | Gets the monotonic backend audio clock in seconds. | — | — |
| <a id="api-7da56903c3092e1e"></a>[`public Inno.Audio.AudioStatistics Inno.Audio.Runtime.AudioRuntime.statistics { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L161) | Gets current runtime resource statistics. | — | — |
| <a id="api-fc17f0a21c22f20a"></a>[`public sealed class Inno.Audio.Runtime.AudioRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Audio.IAudioService`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntime.cs#L19) | Owns playback scheduling, clip retention, mixer generations, and content synchronization. | — | — |

### `Inno.Audio.Runtime.AudioRuntimeFactory`

<!-- api: T:Inno.Audio.Runtime.AudioRuntimeFactory a6c850781c1061e64c4e601ea3a1e993b611a6b4baf5f4ec2d65b6f4295d299b -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Audio.Runtime.AudioRuntime}) 5fd6b1bb3f972bb0cdd7bec3f93c0dfc8b5b412cb75491ba1608d3f6e57b40a7 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeFactory.descriptor a14b51ce2cc68e8ae76e9f1ab17b1eed4cdae090befd817f87dfe85635791401 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) f59c4fed6abc839d1c328570593572cbfda265e8feb6883b9b4c9f4f42d5a4b9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-66f7451ea1e1ad11"></a>[`public Inno.Audio.Runtime.AudioRuntimeFactory.AudioRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Audio.Runtime.AudioRuntime> runtimeFactory)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeFactory.cs#L21) | Creates a reusable factory around a composition-owned audio runtime callback.<br>`runtimeFactory`：The callback that creates a distinct audio runtime for the supplied session. | — | — |
| <a id="api-964afb1451de92aa"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Audio.Runtime.AudioRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeFactory.cs#L43) | Creates an audio feature over a newly allocated runtime layer.<br>返回：A new feature that exclusively owns its audio runtime layer.<br>`context`：The isolated session context passed to the audio runtime factory. | — | — |
| <a id="api-1b28ece01f84da0a"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Audio.Runtime.AudioRuntimeFactory.descriptor { get; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeFactory.cs#L29) | Gets stable ordering metadata that updates audio after scene simulation. | — | — |
| <a id="api-0aa615762cf6542a"></a>[`public sealed class Inno.Audio.Runtime.AudioRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeFactory.cs#L11) | Creates one audio lifecycle feature for every isolated runtime session. | — | — |

### `Inno.Audio.Runtime.AudioRuntimeOptions`

<!-- api: T:Inno.Audio.Runtime.AudioRuntimeOptions b33e6638b00333a9e4d211d7b59ee0cd8b1880ad0081a3f0a0af8d9f7e8fd761 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.maxVoices db2699e818846b7f33a37b388fbae5a9ac807c0136a462f96e1ba24bcdda4076 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.maxPendingPreloads 4bd4763d778db1f42fe7e337f84f6270e3399ca0286e9127e3264897df30dc9c -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.maxContentSnapshots cb9a65d414d68e32ccd19e7a0c16827cbecc8e95bf81ff9af510256e5d186c4d -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.decodedCacheBudgetBytes 3a6833813b9aeec05d753c80bc59f7e99b24ae1f69eed87a9cb384838c624dac -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.automaticStreamingThresholdBytes 5977d7c5f981c25f39c539342769efa360a6a9370e10220af0aa21f397a76bd8 -->
<!-- api: P:Inno.Audio.Runtime.AudioRuntimeOptions.deviceRecoveryIntervalSeconds 82904c1b4be8299657ca022380b7f339110637ac4390d2e89ec00235f02f7e51 -->
<!-- api: M:Inno.Audio.Runtime.AudioRuntimeOptions.#ctor 113175c0d4797f978c7591de63e5bb35641f0dc19e417fd6dd3d5d40d4ee9b25 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dbb08affdc31a7b4"></a>[`public Inno.Audio.Runtime.AudioRuntimeOptions.AudioRuntimeOptions()`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8e71fcfd65399814"></a>[`public long Inno.Audio.Runtime.AudioRuntimeOptions.automaticStreamingThresholdBytes { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L33) | Gets or sets the encoded byte threshold used by automatic load-mode selection. | — | — |
| <a id="api-f45c5096194be750"></a>[`public long Inno.Audio.Runtime.AudioRuntimeOptions.decodedCacheBudgetBytes { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L28) | Gets or sets the decoded clip cache budget in bytes. | — | — |
| <a id="api-9991551b35db7674"></a>[`public float Inno.Audio.Runtime.AudioRuntimeOptions.deviceRecoveryIntervalSeconds { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L38) | Gets or sets the positive delay between output-device recovery attempts in seconds. | — | — |
| <a id="api-b553bb20405ad919"></a>[`public int Inno.Audio.Runtime.AudioRuntimeOptions.maxContentSnapshots { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L23) | Gets or sets the maximum combined emitter and listener snapshots accepted across all providers per update. | — | — |
| <a id="api-e54342ddfe30bfea"></a>[`public int Inno.Audio.Runtime.AudioRuntimeOptions.maxPendingPreloads { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L18) | Gets or sets the maximum number of unfinished preload waiters accepted by this owner. | — | — |
| <a id="api-0d93f693b1815422"></a>[`public int Inno.Audio.Runtime.AudioRuntimeOptions.maxVoices { get; set; }`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L13) | Gets or sets the maximum number of preparing, scheduled, paused, and playing voices. | — | — |
| <a id="api-40ad330200c59bfc"></a>[`public sealed class Inno.Audio.Runtime.AudioRuntimeOptions`](../../src/services/audio/Inno.Audio.Runtime/AudioRuntimeOptions.cs#L8) | Configures bounded audio runtime resource policies. | — | — |

### `Inno.Audio.Runtime.MutedAudioDevice`

<!-- api: T:Inno.Audio.Runtime.MutedAudioDevice d4de906339df4ad567d497d9cec5d0490e8e3001f798b3731d2b9c666f32e154 -->
<!-- api: M:Inno.Audio.Runtime.MutedAudioDevice.#ctor(System.Int32,Inno.Audio.AudioDeviceLimits) 9436211956436d0d3aea2e7e10013aea974c8714e6d3583948bdead493e9faf6 -->
<!-- api: P:Inno.Audio.Runtime.MutedAudioDevice.capabilities f558bfa4bb88639def36c0919b55c806aed05eaf066d53e58604664a4d576663 -->
<!-- api: P:Inno.Audio.Runtime.MutedAudioDevice.generation 3d413f134ad76c111badd700a3d3a28987a81b70c4d3f87e864456803ba44589 -->
<!-- api: P:Inno.Audio.Runtime.MutedAudioDevice.state a81b1da46406e5c5df0d9e8c97928f74e62697758dbd3b52f1c75a7c4417fd88 -->
<!-- api: P:Inno.Audio.Runtime.MutedAudioDevice.dspTime 94180a106b1624a67e2ac9b3b61f8493dd938daccb68ae08d32f543c18e332b0 -->
<!-- api: P:Inno.Audio.Runtime.MutedAudioDevice.statistics 7ae2bcf1c2185280c2e90d651e0a0e1fe069b70991612d95c7ab615007db5545 -->
<!-- api: M:Inno.Audio.Runtime.MutedAudioDevice.Dispose 69f2d90ed497b01bac220a325203ab4572d66010c13e618ffcc966ce2489a5da -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d4c7a91ab74556b7"></a>[`public Inno.Audio.Runtime.MutedAudioDevice.MutedAudioDevice(int sampleRate = 48000, Inno.Audio.AudioDeviceLimits? limits = null)`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L35) | Creates a muted device with deterministic clock and lifecycle behavior.<br>`sampleRate`：Positive logical output sample rate used for capability reporting.<br>`limits`：Optional finite device resource capacities. | — | — |
| <a id="api-305d87584eb0f187"></a>[`public void Inno.Audio.Runtime.MutedAudioDevice.Dispose()`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L327) | Releases all logical clips, voices, buses, and listeners. | — | — |
| <a id="api-04c4b00038c17499"></a>[`public Inno.Audio.AudioCapabilities Inno.Audio.Runtime.MutedAudioDevice.capabilities { get; }`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L50) | Gets logical capabilities available while output is muted. | — | — |
| <a id="api-6836db18b8c8033a"></a>[`public double Inno.Audio.Runtime.MutedAudioDevice.dspTime { get; }`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L65) | Gets the deterministic logical audio clock in seconds. | — | — |
| <a id="api-448c7fc06499fbf9"></a>[`public uint Inno.Audio.Runtime.MutedAudioDevice.generation { get; }`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L55) | Gets the non-zero logical device generation. | — | — |
| <a id="api-a5ae22d8516a0473"></a>[`public Inno.Audio.AudioDeviceState Inno.Audio.Runtime.MutedAudioDevice.state { get; }`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L60) | Gets Inno.Audio.AudioDeviceState.Muted until this device is disposed. | — | — |
| <a id="api-36e45379dcaa4d8e"></a>[`public Inno.Audio.AudioStatistics Inno.Audio.Runtime.MutedAudioDevice.statistics { get; }`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L70) | Gets current logical resource statistics. | — | — |
| <a id="api-a63dcfeede6e3455"></a>[`public sealed class Inno.Audio.Runtime.MutedAudioDevice : Inno.Audio.AudioDevice, Inno.Audio.IAudioDevice`](../../src/services/audio/Inno.Audio.Runtime/MutedAudioDevice.cs#L12) | Advances deterministic audio state without opening an operating-system output device. | This device is used for headless tests and explicit no-device recovery. It never claims audible output. | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Audio](Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
