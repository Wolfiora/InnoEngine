# Inno.Adapter.Audio

[Audio 索引](README.md) · [中立 Audio](Inno.Audio.md) · [MiniAudio implementation](../backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.md)

该项目定义 Audio Adapter family，不包含 MiniAudio binding 或 `Ma*` 类型。

## 公开 API

- `AudioBackendId`：Composition 启动时使用的设备 implementation 选择。
- `AudioBackendOptions`：只描述后端中立的设备启动策略；`noDevice` 用于无硬件运行和测试。
- `IAudioBackendFactory.CreateDevice`：返回 caller-owned `IAudioDevice`。

```csharp
IAudioDevice device = catalog.audio.CreateDevice(
    selection.audio,
    new AudioBackendOptions { noDevice = true });
```

设备丢失、muted state、voice/mixer 语义仍由 `Inno.Audio` / `Inno.Audio.Runtime` 处理。Factory 不包装 gameplay AudioSource、BGM 或 Dialogue 模型。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `miniAudio` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `AudioBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `AudioBackendProvider(AudioBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `AudioBackendProvider.CreateDevice` | 实现者的创建扩展点；返回 caller-owned `IAudioDevice`，不允许 null。 |
| `AudioBackendCatalog(IEnumerable<AudioBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `AudioBackendCatalog.supportedBackends / CreateDevice` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IAudioBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `AudioBackendId`，无需新增枚举或修改中央分支。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Audio.AudioBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.AudioBackendCatalog`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L11) | Resolves audio providers from one immutable, composition-owned registration snapshot. |
| [`Inno.Adapter.Audio.AudioBackendCatalog.AudioBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Audio.AudioBackendProvider> providers)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service. |
| [`Inno.Audio.IAudioDevice Inno.Adapter.Audio.AudioBackendCatalog.CreateDevice(Inno.Adapter.Audio.AudioBackendId backend, Inno.Adapter.Audio.AudioBackendOptions options)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L45) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Audio.AudioBackendId> Inno.Adapter.Audio.AudioBackendCatalog.supportedBackends`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L39) | See the implemented contract. |

### `Inno.Adapter.Audio.AudioBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.AudioBackendId`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | Identifies a audio implementation without closing the set of supported backends. |
| [`Inno.Adapter.Audio.AudioBackendId.AudioBackendId(string value)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier. |
| [`bool Inno.Adapter.Audio.AudioBackendId.isValid`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L41) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.Audio.AudioBackendId.ToString()`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L49) | Returns the identifier without resolving a provider. |
| [`static Inno.Adapter.Audio.AudioBackendId Inno.Adapter.Audio.AudioBackendId.miniAudio`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L31) | Gets the identifier of the bundled miniAudio implementation. |
| [`string Inno.Adapter.Audio.AudioBackendId.value`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. |

### `Inno.Adapter.Audio.AudioBackendOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.AudioBackendOptions`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendOptions.cs#L6) | Configures backend-neutral audio device creation. |
| [`bool Inno.Adapter.Audio.AudioBackendOptions.noDevice`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendOptions.cs#L11) | Gets whether the device must run without opening an operating-system playback endpoint. |

### `Inno.Adapter.Audio.AudioBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.AudioBackendId Inno.Adapter.Audio.AudioBackendProvider.id`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. |
| [`Inno.Adapter.Audio.AudioBackendProvider`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L13) | Describes one explicitly composed audio implementation and its creation boundary. |
| [`Inno.Adapter.Audio.AudioBackendProvider.AudioBackendProvider(Inno.Adapter.Audio.AudioBackendId id)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L24) | Captures the identity assigned by the composition owner. |
| [`abstract Inno.Audio.IAudioDevice Inno.Adapter.Audio.AudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions options)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L45) | Creates a caller-owned audio service using this implementation. |

### `Inno.Adapter.Audio.IAudioBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.IAudioBackendFactory`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L9) | Creates backend-neutral audio devices from explicit backend selections. |
| [`Inno.Audio.IAudioDevice Inno.Adapter.Audio.IAudioBackendFactory.CreateDevice(Inno.Adapter.Audio.AudioBackendId backend, Inno.Adapter.Audio.AudioBackendOptions options = default(Inno.Adapter.Audio.AudioBackendOptions))`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L31) | Creates a new audio device for one runtime audio generation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Audio.AudioBackendId> Inno.Adapter.Audio.IAudioBackendFactory.supportedBackends`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. |

## 项目依赖

- [Inno.Audio](Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
