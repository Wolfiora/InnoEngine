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
using Inno.Adapter.Audio.MiniAudio;
using Inno.Audio;

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider`

<!-- api: T:Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider 630061d4d826d1fd83169959c0ce317bc6aa6aa945f97590412cb2e5eb2c2afe -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.#ctor e630c173c1be4a078c3191d9a740aa9bcff7726ff82ce533cc40e40238444f42 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions) 327469ff642b5c5de8ceec03a42d4bc4797beab2fd7cb366e161355deea7fcc2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-80b9a277e5ebbaf4"></a>[`public Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.MiniAudioBackendProvider()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. | — | — |
| <a id="api-6d961682925d4432"></a>[`public override Inno.Audio.IAudioDevice Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions options)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L17) | Creates a caller-owned audio service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`options`：Backend-neutral device creation options. | — | — |
| <a id="api-3e1aef0c5da1dbbc"></a>[`public sealed class Inno.Adapter.Audio.MiniAudio.MiniAudioBackendProvider : Inno.Adapter.Audio.AudioBackendProvider`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioBackendProvider.cs#L9) | Supplies the MiniAudio implementation through the neutral audio creation boundary. | — | — |

### `Inno.Adapter.Audio.MiniAudio.MiniAudioDevice`

<!-- api: T:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice dd1beabc48b2867fc76a71f32715e8b5f8d0095aa626776a6a89117d4dca0540 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.#ctor 36afb38d543656eec144ade87bd432696cf2e73a66f70454c6b26b0bdae169a0 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.#ctor(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions) 607a2bb9e2cb19aa3e12dc93ce27fc6d536890cd23782ba5b199a98fb7a8ac83 -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.capabilities a2432d67e847d9b3330edee976239414ad038e74e781585ecc837c7c2dd0171c -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.generation 20869b637217658c1b2705cca851e24b2b93a33eb72beb2ee54b31e654cb5da5 -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.state 3264a2f35261bc6040daaa85ceb40781a9988809a5a2d80b718889abc5dae984 -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.dspTime e1c862bcefe0936d7937d80fda5b0ad5ccdd8e51c637cabed85b0f2593276c32 -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.statistics d18497c0cc181bf5014c911b94b455f79a4ca737c698acde2a47b800c58218b6 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.Dispose ca00cbf8b0599d17a77c7147a5ef9757f898c66cbd68919d0938a69b37059293 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4be80341694152c6"></a>[`public Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.MiniAudioDevice()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L43) | Creates a MiniAudio device using the default operating-system output device. | — | — |
| <a id="api-301a06bc56226cee"></a>[`public Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.MiniAudioDevice(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions options)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L57) | Creates one MiniAudio backend generation with explicit output and headless settings.<br>`options`：Device graph and operating-system output settings. | — | `System.InvalidOperationException`：Thrown when the native engine cannot be initialized with the requested settings. |
| <a id="api-6b0a9d3548d57194"></a>[`public void Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.Dispose()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L576) | Releases all sounds, buses, processor nodes, listeners, and the native engine in dependency order. | — | `Inno.Core.Execution.RetirementPendingException`：Canceled encoded preparation still owns a source reader. The owner must retain this device and retry disposal at its retirement safe point until preparation drains. |
| <a id="api-5205f0346c2a6d04"></a>[`public Inno.Audio.AudioCapabilities Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.capabilities { get; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L101) | Gets MiniAudio capabilities for this immutable device generation. | — | — |
| <a id="api-3099ed198c8fca24"></a>[`public double Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.dspTime { get; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L116) | Gets the monotonic MiniAudio engine clock in seconds. | — | — |
| <a id="api-0d44d7b006e89977"></a>[`public uint Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.generation { get; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L106) | Gets the non-zero generation encoded into every handle created by this device. | — | — |
| <a id="api-8ee746a3b5f2a2e9"></a>[`public Inno.Audio.AudioDeviceState Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.state { get; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L111) | Gets current native output availability without exposing a MiniAudio device type. | — | — |
| <a id="api-147a07d819d04afd"></a>[`public Inno.Audio.AudioStatistics Inno.Adapter.Audio.MiniAudio.MiniAudioDevice.statistics { get; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L124) | Gets current native resource counts and approximate decoded storage. | — | — |
| <a id="api-511eb4602e961aa4"></a>[`public sealed class Inno.Adapter.Audio.MiniAudio.MiniAudioDevice : Inno.Audio.AudioDevice, Inno.Audio.IAudioDevice`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L16) | Implements the backend-neutral audio device contract with a private MiniAudio engine and node graph. | — | — |

### `Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions`

<!-- api: T:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions 5e18b8595d06597b37a9a6d9abbcb22e9f2c51cc04e4b7790ac83357c66a0ac5 -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.noDevice d9f1ecc895c10867c6903cb64b2b4462dbdda0d86e500cb8902b4ddaa3b493db -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.channels 7b088cbfe023d54274d3b2cdee91bda50ba0b1fcd718aded397c01dbdf6f37fd -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.sampleRate c02426ae551a6c0b45f051ba14a785603cd8e474f59828295be3aed7e22894fb -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.listenerCount 84c73b72bbd80296a39c2486002ed0c89c4568683a4926dee53c2909bdfb35fa -->
<!-- api: P:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.limits 78960162164e0fff221cc31dc63e967234daf6b04e73703445093fd42b6d2085 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.ToString e8a3546d59d490653537a43f96a0ea22254cfe747c6dc244a673b6dbd8171750 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.op_Inequality(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions,Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions) 7a6c9786bd5d3df27053db2fbeeb32963ca59c0c2a60e365c3ab9db9d1ce0230 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.op_Equality(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions,Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions) d23b6652d40998eada39216a9b2af6ac4bd0bec61c3babe219b717e66a274602 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.GetHashCode 07e9b961238a4978a5692bb7cdc96ddd3325239e51500a5c594f9288d6a1b9fc -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.Equals(System.Object) e116384341406f4cbe556677b69d208043c5168f67342c0809a625293a3fc5fd -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.Equals(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions) ab5d69031670f94c17dac7c8000e37cd03ddf55f42dc290f29f22d3a23b1b402 -->
<!-- api: M:Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.#ctor 322c44801164fd20623165f29a00a940611047c12554a731afd8d7e194c367a1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-392a35d30df49c7f"></a>[`public Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.MiniAudioDeviceOptions()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-61df1fe492e64757"></a>[`public bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.Equals(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions? other)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-4d77ab4b206268f0"></a>[`public override bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.Equals(object? obj)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-7ce17a1bc4f9132f"></a>[`public override int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.GetHashCode()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-f7d5ca2c9cb61251"></a>[`public static bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.operator ==(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions? left, Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions? right)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-7a01e74136dd09cd"></a>[`public static bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.operator !=(Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions? left, Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions? right)`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a1d7b0b31eab4976"></a>[`public override string Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.ToString()`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-ffda082718624524"></a>[`public int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.channels { get; init; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L19) | Gets or initializes the output channel count used by the engine graph. | — | — |
| <a id="api-d9d8bdf4d421e47f"></a>[`public Inno.Audio.AudioDeviceLimits Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.limits { get; init; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L34) | Gets finite device resource capacities, including pending completion admission. | — | — |
| <a id="api-d0f79c8f33ac5c69"></a>[`public int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.listenerCount { get; init; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L29) | Gets or initializes the maximum listener count exposed by this backend generation. | — | — |
| <a id="api-6e937add8021fa52"></a>[`public bool Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.noDevice { get; init; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L14) | Gets or initializes whether the engine advances without opening an operating-system output device. | — | — |
| <a id="api-ed8a7524f524527b"></a>[`public int Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions.sampleRate { get; init; }`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L24) | Gets or initializes the output sample rate in frames per second. | — | — |
| <a id="api-5be0106127f0ba10"></a>[`public sealed record Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions : System.IEquatable<Inno.Adapter.Audio.MiniAudio.MiniAudioDeviceOptions>`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDeviceOptions.cs#L9) | Configures creation of one MiniAudio backend generation. | — | — |

## 项目依赖

- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.MiniAudio](Inno.Native.MiniAudio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Audio](../../audio/Inno.Adapter.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Audio](../../audio/Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
