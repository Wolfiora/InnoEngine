# Inno.Build API

[项目入口](../Inno.Build.md)

## 公开 API

| API | 语义 |
| --- | --- |
| `BuildSettings`, `BuildSettingsStore` | 项目拥有的 Game/Plugin 导出默认值；使用当前 Inno Serialization 原子保存为 `Settings.Build.inno` |
| `BuildProfile`, `BuildProfileStore`, `BuildTargetId` | 一次 Game 构建所需的可验证 profile 与目标身份；`managedDeployment` 独立选择托管运行时，`Copy()` 创建隔离请求输入 |
| `GameBuildRequest`, `PluginBuildRequest` | 一次不可变构建请求 |
| `BuildProgress`, `BuildDiagnostic`, `BuildDiagnosticSeverity`, `BuildResult` | 进度、结构化诊断与最终结果 |
| `BuildPipeline` | Game/Plugin 的最小异步入口，并公开当前注册目标、显示名称查询与 `EnsurePlayerSupportPackAsync` 预备入口 |
| `IGameBuildTarget` | 可替换的平台打包目标，声明稳定 ID、显示名称、runtimeIdentifier 和默认 managed deployment；内容编译器独立绑定 |
| `GameBuildContentContext`, `GameBuildPackageContext` | 隔离 staging context；Package context 提供已经校验的 `ManagedDeploymentResult`，packager 只组织平台布局 |
| `BuildPipeline.GetManagedDeployments(target)` | 返回所选平台可用的托管 provider ID，不通过封闭平台 switch 选择运行时 |
| `PlayerSupportPackCatalog` | 验证并解析不可变部署 closure；PublishAsync 发布完整候选并原子切换 current 索引 |
| `IPlayerSupportPackProvisioner` | Host 注入的当前源码与 SDK 异步准备边界；独立发行可不安装 SDK |

Content writer、`.iplugin` archive writer、snapshot fingerprint、script stage、staging transaction 与 player composer 全部 internal。
