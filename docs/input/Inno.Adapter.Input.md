# Inno.Adapter.Input

[Input 索引](README.md) · [中立 Input](Inno.Input.md) · [SDL3 implementation](Inno.Adapter.Input.Sdl3.md)

该项目定义平台事件到 Session Input backend 之间的 Adapter family，不包含 SDL3 引用。

## 公开 API

- `InputBackendId`：Composition 启动时使用的 input implementation 选择。
- `IInputBackendFactory.CreateEventSource`：创建 application-owned event source；`acceptAllWindows` 为 `false` 时只接收指定主窗口，为 `true` 时接收所有窗口，但调用方仍须按目标 Session 筛选事件。
- `IInputEventSource`：接收中立 `Event`，并为每个 RuntimeSession 创建隔离的 `IInputBackend`。

Shell 拥有主窗口 event source，并在平台事件泵中调用 `ProcessEvent`。Editor 另为 Play Session 创建全窗口 event source，只把经过 Game View 焦点与画面命中策略的中立事件交给它。每个 Session 只拥有自己由 `CreateBackend` 返回的 backend；销毁 Session 不得销毁 application event source。

```csharp
using IInputEventSource source = catalog.input.CreateEventSource(
    selection.input,
    window,
    acceptAllWindows: false);
using IInputBackend backend = source.CreateBackend();
```

Action Map、rebinding 和 UI navigation 不属于此 family。

## 平台事件交付

Sdl3InputSource 复用 Core Events 的 EventDispatcher/EventHub，将 backend 注册为 Event 订阅。全局已消费事件不再传入 Session；backend Dispose 释放自身订阅，其他 Session 保持有效。source 的列表只管理 backend 生命周期，不承担第二套事件分发。Game View 的焦点策略仍在 Editor presentation 边界筛选，同一平台事件入口适用于桌面和 Web。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `sdl3` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `InputBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `InputBackendProvider(InputBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `InputBackendProvider.CreateEventSource` | 实现者的创建扩展点；返回 caller-owned `IInputEventSource`，不允许 null。 |
| `InputBackendCatalog(IEnumerable<InputBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `InputBackendCatalog.supportedBackends / CreateEventSource` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IInputBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `InputBackendId`，无需新增枚举或修改中央分支。
