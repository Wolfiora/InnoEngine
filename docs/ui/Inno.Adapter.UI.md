# Inno.Adapter.UI

[UI 索引](README.md) · [Runtime adapter catalog](../runtime/Inno.Adapter.md)

`IUiBackendFactory`、`UiBackendId`、`UiBackendProvider` 与 `UiBackendCatalog` 是 Composition 层的开放式后端选择协议。默认 catalog 注册 RmlUi provider；第三方可以注册自己的稳定 ID/provider，而不修改引擎枚举，也不要求游戏脚本引用原生库。

`UiBackendId.rmlUi` 只是内置 provider 的 ID，不是白名单。工厂的 `CreateBackend` 每次返回 Session 独占 `IUiBackend`；backend 必须声明 `implementationId` 与 `UiBackendCapabilities`，并在 `DestroyContext`/`Dispose` 完成所属资源退休。

## 公开边界

| API | 用途 |
| --- | --- |
| `UiBackendId`、`UiBackendProvider`、`UiBackendCatalog` | 开放式实现注册与不可变 composition snapshot；不是脚本导出。 |
| `IUiBackendFactory.CreateBackend` | Composition 在创建 Session 时取得独占 backend。 |

扩展实现只需符合 [IUiService](Inno.UI.md) 下方的后端中立帧与事件协议。它不可将 native context、RmlUi 元素或 GPU handle 写入资产或 Scene。Host 在 Session 停止时关闭 backend；后端创建失败应阻止 Session 启动，而非回退到空 UI。

## 注册身份

`UiBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.UI.IUiBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.IUiBackendFactory`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L9) | Creates isolated UI backends without exposing implementation assemblies to composition code. |
| [`Inno.UI.IUiBackend Inno.Adapter.UI.IUiBackendFactory.CreateBackend(Inno.Adapter.UI.UiBackendId backend)`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L25) | Creates one caller-owned UI backend. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.UI.UiBackendId> Inno.Adapter.UI.IUiBackendFactory.supportedBackends`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L14) | Gets exact backend identities available in this composition generation. |

### `Inno.Adapter.UI.UiBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.UiBackendCatalog`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L12) | Resolves UI providers from one immutable composition-owned registration snapshot. |
| [`Inno.Adapter.UI.UiBackendCatalog.UiBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.UI.UiBackendProvider> providers)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L22) | Validates and captures a complete provider set. |
| [`Inno.UI.IUiBackend Inno.Adapter.UI.UiBackendCatalog.CreateBackend(Inno.Adapter.UI.UiBackendId backend)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L45) | Creates a backend using this implementation's validated inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.UI.UiBackendId> Inno.Adapter.UI.UiBackendCatalog.supportedBackends`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L34) | Gets backend registrations available in this type generation. |

### `Inno.Adapter.UI.UiBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.UiBackendId`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | Identifies a UI implementation independently from its document language. |
| [`Inno.Adapter.UI.UiBackendId.UiBackendId(string value)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L16) | Creates an ordinal, case-sensitive backend identifier. |
| [`bool Inno.Adapter.UI.UiBackendId.isValid`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L38) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.UI.UiBackendId.ToString()`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L46) | Returns the stable identifier. |
| [`static Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.UiBackendId.rmlUi`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L28) | Gets the identifier of the bundled RmlUi implementation, not a closed backend list. |
| [`string Inno.Adapter.UI.UiBackendId.value`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L33) | Gets the stable identifier; the default value is unassigned. |

### `Inno.Adapter.UI.UiBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.UiBackendProvider.id`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L30) | Gets this provider's stable implementation identity. |
| [`Inno.Adapter.UI.UiBackendProvider`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L9) | Creates one concrete UI backend registered by a composition root. |
| [`Inno.Adapter.UI.UiBackendProvider.UiBackendProvider(Inno.Adapter.UI.UiBackendId id)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L20) | Captures the registration identity assigned by the composition owner. |
| [`abstract Inno.UI.IUiBackend Inno.Adapter.UI.UiBackendProvider.CreateBackend()`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L38) | Creates one caller-owned backend generation. |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
