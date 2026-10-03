# Inno.Adapter.Input.Sdl3

[Input 索引](README.md) · [Runtime](Inno.Input.Runtime.md) · [Platform SDL3](../platform/Inno.Adapter.Platform.Sdl3.md)

`Inno.Adapter.Input.Sdl3` 是具体 adapter。`Sdl3InputSource` 接收 `Inno.Adapter.Platform.Sdl3` 已翻译的中立 Event，并为 Edit/Play/Player 创建彼此隔离的 `Sdl3InputBackend`。

## 公开 API

| API | 语义 |
| --- | --- |
| `Sdl3InputSource(windowId)` | 限定一个 SDL window；零表示接收全部 window。 |
| `CreateBackend()` | 创建 caller-owned、可独立释放的 Session backend。 |
| `ProcessEvent(Event)` | 经 Core Events 同步分发中立事件，遵守其消费语义。 |
| `Sdl3InputBackend` | 累积键鼠状态；focus loss 确定性释放 held state。 |

Source Dispose 会断开全部 backend；backend Dispose 会从 Source 注销。Adapter 不把 SDL handle 或原生 enum 暴露给 `Inno.Input`。

Source 拥有 `EventDispatcher` / `EventHub`，backend 持有可释放订阅；不建立另一套事件队列或监听器注册表。
已消费的事件不更新 Input 状态。桌面和 Web 都使用 SDL3 翻译后的同一入口。
