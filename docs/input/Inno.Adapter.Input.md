# Inno.Adapter.Input

[Input 索引](README.md) · [中立 Input](Inno.Input.md) · [SDL3 implementation](Inno.Adapter.Input.Sdl3.md)

该项目定义平台事件到 Session Input backend 之间的 Adapter family，不包含 SDL3 引用。

## 公开 API

- `InputBackend`：Composition 启动时使用的 input implementation 选择。
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
