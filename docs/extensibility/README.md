# Extensibility API

[Wiki 首页](../README.md) · [Scripting](../scripting/README.md)

| 项目 | 职责 |
| --- | --- |
| [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md) | 无反向依赖的显式类型事实和静态注册契约 |
| [Inno.Extensibility.Modules](Inno.Extensibility.Modules.md) | 中立来源、依赖闭包、目录 candidate transaction 与 generation owner |
| [Inno.Extensibility.Types](Inno.Extensibility.Types.md) | Stable Type ID、不可变 TypeCache snapshot 与通用 TypeRegistry |
| [Inno.Extensibility.Reload](Inno.Extensibility.Reload.md) | collectible generation 的强制 GC unload barrier 与终止性 retention failure |

所有 Attribute 扫描只发生在 candidate build。候选完整验证后在安全点原子 Activate；持久状态只保存 Stable ID 和中立数据，不跨 generation 长期保存 `Type`、实例或 delegate。
