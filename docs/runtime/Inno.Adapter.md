# Inno.Adapter

[Runtime 索引](README.md) · [Shell](Inno.Shell.md) · [默认 Runtime catalog](Inno.Adapter.Default.md)

`Inno.Adapter` 是所有 runtime backend family 的统一组合契约，本身不引用任何具体 implementation 或 authoring pipeline。

## 公开 API

- `AdapterSelection`：一次 Host 启动所使用的 Platform、Input、Storage、Rendering 与 Audio 中立枚举集合；`defaultValue` 表示标准发行版组合。
- `IAdapterCatalog`：公开上述五个 family factory。

```csharp
AdapterSelection selection = AdapterSelection.defaultValue;
IAdapterCatalog catalog = new DefaultAdapterCatalog();
```

`AdapterSelection` 只保存选择，不保存 native handle、实例或可热重载对象。Presentation 和 compiler 不属于 runtime catalog；它们由 [authoring catalog](Inno.Adapter.Authoring.Default.md) 独立扩展。

## Composition 预检

`AdapterSelection.Validate(IAdapterCatalog)` 在创建任何领域服务前校验 platform/input/storage/rendering/audio/text/ui 的完整选择；无效或缺失注册抛出 `NotSupportedException`。所有选择为开放的领域 backend ID，默认值未赋值，内置静态 ID 只表示默认实现。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.AdapterSelection`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Adapter.AdapterSelection.Validate(Inno.Adapter.IAdapterCatalog catalog)`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L71) | Validates every selected registration before the composition creates any service. |
| [`Inno.Adapter.Audio.AudioBackendId Inno.Adapter.AdapterSelection.audio`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L47) | Gets the selected audio backend. |
| [`static Inno.Adapter.AdapterSelection Inno.Adapter.AdapterSelection.defaultValue`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L22) | Gets the built-in default adapter selection shipped by the engine distribution. |
| [`Inno.Adapter.Input.InputBackendId Inno.Adapter.AdapterSelection.input`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L32) | Gets the selected input backend. |
| [`Inno.Adapter.Platform.PlatformBackendId Inno.Adapter.AdapterSelection.platform`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L27) | Gets the selected platform backend. |
| [`Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.AdapterSelection.rendering`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L42) | Gets the selected rendering backend. |
| [`Inno.Adapter.Storage.StorageBackendId Inno.Adapter.AdapterSelection.storage`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L37) | Gets the selected storage backend. |
| [`Inno.Adapter.Text.TextBackendId Inno.Adapter.AdapterSelection.text`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L52) | Gets the selected Unicode text backend. |
| [`Inno.Adapter.UI.UiBackendId Inno.Adapter.AdapterSelection.ui`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L57) | Gets the selected retained-mode UI backend. |
| [`Inno.Adapter.AdapterSelection`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L17) | Selects one implementation for every replaceable host backend family. |

### `Inno.Adapter.IAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.IAdapterCatalog.audio`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L39) | Gets the audio backend factory. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.IAdapterCatalog.input`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L24) | Gets the input backend factory. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.IAdapterCatalog.platform`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L19) | Gets the platform backend factory. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.IAdapterCatalog.rendering`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L34) | Gets the rendering backend factory. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.IAdapterCatalog.storage`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L29) | Gets the application-storage backend factory. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.IAdapterCatalog.text`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L44) | Gets the Unicode text backend factory. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.IAdapterCatalog.ui`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L49) | Gets the retained-mode UI backend factory. |
| [`Inno.Adapter.IAdapterCatalog`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L14) | Exposes factories for every replaceable host backend family without exposing implementations. |

## 项目依赖

- [Inno.Adapter.Audio](../audio/Inno.Adapter.Audio.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Platform](../platform/Inno.Adapter.Platform.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Rendering](../rendering/Inno.Adapter.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Storage](../storage/Inno.Adapter.Storage.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
