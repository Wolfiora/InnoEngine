# Input API

[Wiki 首页](../README.md) · [Runtime](../runtime/README.md) · [Platform](../platform/README.md)

Input 分成后端中立契约、Session Runtime 与 Core Events 适配器。平台后端负责采集系统输入；Action Map、rebinding、UI navigation 等解释策略不进入输入事实层。文本输入会话由当前 Host 路由至窗口，不要求脚本引用平台实现。

| 项目 | 职责 |
| --- | --- |
| [Inno.Input](Inno.Input.md) | `IInputService`、不可变 `InputSnapshot`、脚本 façade 与 backend contract |
| [Inno.Input.Runtime](Inno.Input.Runtime.md) | 每 Session 的 Begin/End frame scope 与快照生命周期 |
| [Inno.Adapter.Input](Inno.Adapter.Input.md) | Provider 选择、Core Events 输入累积、会话隔离与文本输入窗口路由 |
