# Inno.Plugins.Authoring API

[项目入口](../Inno.Plugins.Authoring.md)

## 公开 API

内部依赖 Core Execution 的统一退休异常分类；不会向游戏脚本暴露底层退休调度器或原生 callback。

- `PluginEnvironment`：active/discovery generation、pending code activation、owner-thread Update/Refresh。
- `PluginSourceService`：一次隔离 Scan 与 `.iplugin` validation。
- `PluginSourceKind`, `PluginSourceLimits`：安装源种类和资源上限。
- `PluginCandidate`, `PluginDiagnostic`, `PluginScanResult`：不可变候选与诊断模型。
