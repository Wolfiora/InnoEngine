# Inno.Build.SupportPacks.Core

[Build 索引](README.md) · [Wiki 首页](../README.md) · [内置平台组合](Inno.Build.SupportPacks.md) · [Build 管线](Inno.Build.md)

## 职责与依赖

只拥有 Support Pack 事务和源码供给协调。依赖中立 Inno.Build 契约，目录提交复用 `Inno.Core.IO.AtomicDirectory`，不引用具体平台、Web、Native 工具链或 Editor。Host 显式注册平台 source；核心不根据 target 名称选择实现。

发布在 Prepare 后与完整 Validate 后分别检查取消；开始不可取消的目录安装前仍被取消时，保留原 installed pack 并清理 staging。

## 所有公开 API

| API | 稳定语义 |
| --- | --- |
| `IPlayerSupportPackSource.target` | source 负责的稳定平台 ID。 |
| `PrepareAsync(context, cancellationToken)` | 将完整运行时和编译输入写入隔离 staging；不能替换已安装目录。 |
| 继承的 `IPlayerSupportPackValidator.Validate(directory)` | 由平台验证所需文件和 ABI 输入。 |
| `PlayerSupportPackBuildContext(engineRoot, stagingDirectory, dotnetHost)` 及同名只读属性 | 显式提供源码、事务目录和 SDK；不暴露已安装目录。 |
| `PlayerSupportPackPublisher(sources)` | 冻结独立注册表，重复目标立即失败。 |
| `PublishAsync(engineRoot, outputRoot, target, dotnetHost, cancellationToken)` | prepare → 校验 → 带回滚的目录安装，成功返回绝对目录。 |
| `SourcePlayerSupportPackProvisioner(engineRoot, publisher)` | 使用 Host 注册的发布器供给缺失 Pack。 |
| `TryCreateForHost(startDirectory, publisher)` | 查找源码根或 INNO_ENGINE_ROOT；独立发行 Host 返回 null。 |
| `ProvisionAsync(target, supportPackRoot, cancellationToken)` | 实现 Inno.Build 的自动供给入口。 |

没有 protected 扩展点。实现新的 source 不修改事务核心。

## 常见工作流

```csharp
using System;
using Inno.Build.SupportPacks;

var publisher = BuiltInPlayerSupportPacks.CreatePublisher();
var provisioner = SourcePlayerSupportPackProvisioner.TryCreateForHost(
    AppContext.BaseDirectory,
    publisher);
```

此示例同时引用内置平台组合项目。第三方 Host 可以直接 `new PlayerSupportPackPublisher(sources)`。
Host 先调用 `BuildPipeline.EnsurePlayerSupportPackAsync`，在 Pack 缺失时通过 provisioner 准备；
已安装但无效的 Pack 保持明确失败。之后在 authoring owner thread 调用 `BuildGameAsync`。

## 错误与生命周期

source 抛出、取消、校验失败时删除 staging，保留原安装。只有完整候选校验成功后移动目录；候选安装失败恢复备份。
提交后的旧备份清理失败时，明确报告已安装的新 Pack 和残留备份位置，同时保留完整新 Pack。两次目录移动需要调用方协调同一目录的读写；它们不提供文件替换式的无间隙可见性。
SDK 由 DOTNET_HOST_PATH、DOTNET_ROOT 或 Host 安装位置确定，目标 source 检查自己的工具能力。整个过程没有 collectible 模块激活，游戏导出仍受 generation admission barrier 管理。
