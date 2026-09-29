# Inno.Build.SupportPacks.Core

[Build 索引](README.md) · [Wiki 首页](../README.md) · [构建入口](Inno.Build.md) · [手动命令](Inno.Build.SupportPacks.md)

## 职责与边界

本项目承载 Player Support Pack 的发布实现与源码工作区供给器，依赖 `Inno.Build` 的目标 ID 和 Pack 校验契约。`Inno.Build` 只依赖 `IPlayerSupportPackProvisioner`，不引用 SDK、Player 项目或本项目。Editor 与 Build CLI 在 composition root 选择是否提供源码工作区供给能力。

发布器使用 `dotnet publish` 生成目标 Player，复制可部署程序集和对应目标的 release 原生库，在隔离目录验证后原子安装。缺少 SDK、目标原生库或校验失败会明确报错；失败不交付不完整的目标目录。

## 公开 API

| API | 语义 |
| --- | --- |
| `PlayerSupportPackPublisher.PublishAsync` | 从引擎源码与目标 ID 构建、验证并安装一个 Support Pack，返回已安装目录。 |
| `SourcePlayerSupportPackProvisioner` | 实现 `IPlayerSupportPackProvisioner`，在目标 Pack 缺失时调用发布器。 |
| `SourcePlayerSupportPackProvisioner.TryCreateForHost` | 从宿主二进制目录向上查找引擎源码根；可用 `INNO_ENGINE_ROOT` 指定根目录；独立发行环境返回 `null`。 |

## 初始化与常见工作流

Host 在创建 `BuildPipeline` 时传入；Editor 导出窗口先调用 `EnsurePlayerSupportPackAsync`，完成后从下一主线程帧启动构建：

```csharp
IPlayerSupportPackProvisioner? provisioner =
    SourcePlayerSupportPackProvisioner.TryCreateForHost(AppContext.BaseDirectory);

var pipeline = new BuildPipeline(
    assets, plugins, settings, serialization, generations, compiler,
    supportPackRoot, gameTargets, provisioner);
```

已有有效 Pack 不会重新发布。已有目录但内容无效时保留目录并报告校验错误，避免掩盖损坏状态。目标目录不存在时，每个目标在同一进程内串行供给；取消会停止发布进程并清理 staging。独立发行的 Editor 应预装经过验证的 Pack，无需源码或 SDK。

本项目没有 `protected` 扩展点。若要提供不同的 Pack 来源，由 Host 实现 `IPlayerSupportPackProvisioner`；Pack 的部署内容仍由 `PlayerSupportPackCatalog` 验证。

## 生命周期与错误

发布所需的 release 原生库必须已经存在于 `.lib/<component>/<target>`，SDK host 可通过 `DOTNET_HOST_PATH` 指定。发布期间仅创建重建型 staging；验证成功后才替换目标目录。失败不会改变已安装的有效 Pack。Pack 构建与验证属于 Export 前置阶段，游戏内容仍由后续的 Build Pipeline 构建和原子提交。

## 相邻页面

- [Inno.Build](Inno.Build.md)：Build 管线和可替换供给接口。
- [Inno.Build.SupportPacks](Inno.Build.SupportPacks.md)：手动执行同一发布器的 CLI。
