# Inno.Adapter.Audio

[Audio 索引](README.md) · [中立 Audio](Inno.Audio.md) · [MiniAudio implementation](Inno.Adapter.Audio.MiniAudio.md)

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
