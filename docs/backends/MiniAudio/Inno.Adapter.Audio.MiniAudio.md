# Inno.Adapter.Audio.MiniAudio

Clip 不再只是文件描述。`CreateClip` 创建并保留 native resource-manager data source，使用异步 Decode/Stream；`GetClipState` 无阻塞查询 Preparing/Ready/Failed。每个 Voice 仍有独立播放游标，decoded buffer 由 native resource manager 共享。播放准备错误以 DecodeFailed 完成。DestroyClip/Dispose 在 EngineUninit 前释放所有 data source 和原生分配；Artifact 路径的寿命由上层 cache 租约保证。

[分类索引](README.md) · [Runtime](../../audio/Inno.Audio.Runtime.md) · [Native binding](Inno.Native.MiniAudio.md) · [Build toolchain](Inno.Build.Toolchains.MiniAudio.md) · [Wiki 首页](../../README.md)

`Inno.Adapter.Audio.MiniAudio` 是 `IAudioDevice` 的官方后端适配器，也是运行时中唯一允许引用 `Inno.Native.MiniAudio` 的项目。程序集 public/protected API 只暴露后端中立类型。

## 公开 API

| 类型 | 成员 | 语义 |
| --- | --- | --- |
| `MiniAudioDeviceOptions` | `noDevice`、`channels`、`sampleRate`、`listenerCount`、`limits` | 创建一个不可变 backend generation；listener 数为 1–4。 |
| `MiniAudioDevice` | 默认构造、options 构造、`IAudioDevice`、`Dispose` | 包装 `ma_engine`、resource manager、sound/group 与 node graph。 |

设备支持 WAV/FLAC/MP3、Decode/Stream、异步准备、scheduled playback、2D pan、基础 3D distance/cone/doppler、多个 listener、Bus graph 与八种标准 processor。滤波器映射到 native biquad coefficient/node，delay 映射到 native delay node。

`Play` 在分配原生 Voice 前拒绝未初始化的 options 与非有限/负 scheduled time，返回 invalid handle。
`SetVoiceParameters` 对默认零 pitch 返回 false；`SetBusVolume` 对 NaN/Infinity 返回 false，不污染已播放的声音。
其余数值与空间向量由中立配置构造函数验证；静音后端遵循相同接纳规则。no-device 测试实际创建 native Clip，
验证拒绝不会增加 active voice 数且后续合法播放仍能推进。

```csharp
using IAudioDevice device = new MiniAudioDevice(new MiniAudioDeviceOptions
{
    noDevice = true,
    sampleRate = 48000,
    channels = 2
});
```

`noDevice` 不是静默跳过：adapter 主动拉取 PCM frame，使 native graph、DSP clock、scheduled voice 与自然结束在 CI/headless 环境真实推进。普通模式使用 macOS CoreAudio 或 Windows WASAPI 默认输出。

初始化任一步失败都会逆序回滚 native allocation；运行中会在控制线程检查 native output device state，并把无法重新启动的 generation 标记为 Lost，交由 Runtime 候选恢复。Dispose 先停止 Voice，再释放 Clip、processor、Bus、Listener 和 engine。native library 与 binding 必须来自同一固定 miniaudio commit，不混用动态 ABI。

`MiniAudioDeviceOptions.limits` 接收 `AudioDeviceLimits`，创建时验证非 null。Clip、Bus 与 Voice 分配在 native 入口前检查预算；未排空 completion 仍占 Voice 容量，TryDequeueCompletion 后才可重新接纳。容量不足返回 invalid handle，不发起 native operation。

## Composition provider

`MiniAudioBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateDevice(AudioBackendOptions)` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。






## 本轮边界与所有权

MiniAudio 的文件型 native 入口所需 materialization 仅属于此 Adapter。编码缓存按内容身份复用，不按 voice 复制；Stream 保留 native 流式解码，不变为整段 PCM 常驻。准备任务在后台执行，native audio callback 保留在原生侧；销毁顺序为完成/取消准备、释放 native clip、释放 source lease。

## 源码归属

当前唯一源码 owner：`backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/Inno.Adapter.Audio.MiniAudio.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L9) | Supplies the MiniAudio implementation through the neutral audio creation boundary. |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.MiniAudioBackendProvider()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Audio.IAudioDevice Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions options)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L17) | See the implemented contract. |

### `Inno.Adapter.Audio.MiniAudio.MiniAudioDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioDevice`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L16) | Implements the backend-neutral audio device contract with a private MiniAudio engine and node graph. |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.MiniAudioDevice()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L43) | Creates a MiniAudio device using the default operating-system output device. |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.MiniAudioDevice(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions options)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L57) | Creates one MiniAudio backend generation with explicit output and headless settings. |
| [`Inno.Audio.AudioCapabilities Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.capabilities`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L101) | Gets MiniAudio capabilities for this immutable device generation. |
| [`Inno.Audio.AudioDeviceState Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.state`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L111) | Gets current native output availability without exposing a MiniAudio device type. |
| [`Inno.Audio.AudioStatistics Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.statistics`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L124) | Gets current native resource counts and approximate decoded storage. |
| [`double Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.dspTime`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L116) | Gets the monotonic MiniAudio engine clock in seconds. |
| [`uint Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.generation`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L106) | Gets the non-zero generation encoded into every handle created by this device. |
| [`void Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.Dispose()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L576) | Releases all sounds, buses, processor nodes, listeners, and the native engine in dependency order. |

### `Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Configures creation of one MiniAudio backend generation. |
| [`Inno.Audio.AudioDeviceLimits Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.limits`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L34) | Gets finite device resource capacities, including pending completion admission. |
| [`bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.noDevice`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L14) | Gets or initializes whether the engine advances without opening an operating-system output device. |
| [`int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.channels`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L19) | Gets or initializes the output channel count used by the engine graph. |
| [`int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.listenerCount`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L29) | Gets or initializes the maximum listener count exposed by this backend generation. |
| [`int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.sampleRate`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L24) | Gets or initializes the output sample rate in frames per second. |

## 项目依赖

- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.MiniAudio](Inno.Native.MiniAudio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Audio](../../audio/Inno.Adapter.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Audio](../../audio/Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
