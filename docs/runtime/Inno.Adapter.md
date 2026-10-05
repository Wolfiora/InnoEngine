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
