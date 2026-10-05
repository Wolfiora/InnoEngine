# Inno.Adapter.Text

[Text 索引](README.md) · [Runtime adapter catalog](../runtime/Inno.Adapter.md)

`ITextBackendFactory` 与 `TextBackendId` 是 Composition 使用的中立选择协议。Host 依 `AdapterSelection` 选实现并为 Session 创建后端；游戏脚本只见 `Inno.Text` 契约。新增排版后端不改变 Text API。

当前 `TextBackendId.freeTypeHarfBuzz` 由默认 catalog 提供。工厂的 `CreateBackend` 每次返回调用方独占的 `ITextBackend`；`LoadFont` 返回 generation-local `TextFontHandle`，`ReleaseFont`/`Dispose` 逆序清理。`Shape` 和 `Rasterize` 不能接受上一后端代际的 handle。实现者不得把原生指针放入持久资产或脚本导出。

## 公开边界

| API | 用途 |
| --- | --- |
| `TextBackendId` | 选择内置 FreeType/HarfBuzz 实现；不是脚本导出。 |
| `ITextBackendFactory.CreateBackend` | Composition 在创建 Session 时取得独占 backend。 |

扩展后端仍须实现 [Text Service](Inno.Text.md) 的 `ITextBackend`，返回不可持久化的 face handle。Host 应在 Session 停止时调用 `Dispose`；原生依赖不向上层传播。无效 face 和后端故障必须在调用点报告。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `freeTypeHarfBuzz` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `TextBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `TextBackendProvider(TextBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `TextBackendProvider.CreateBackend` | 实现者的创建扩展点；返回 caller-owned `ITextBackend`，不允许 null。 |
| `TextBackendCatalog(IEnumerable<TextBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `TextBackendCatalog.supportedBackends / CreateBackend` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `ITextBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `TextBackendId`，无需新增枚举或修改中央分支。
