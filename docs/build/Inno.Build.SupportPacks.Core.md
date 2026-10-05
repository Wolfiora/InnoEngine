# Inno.Build.SupportPacks.Core

[Build 索引](README.md) · [Wiki 首页](../README.md) · [内置平台组合](Inno.Build.SupportPacks.md) · [Build 管线](Inno.Build.md)

## 职责与依赖

只拥有 Support Pack 事务和源码供给协调。依赖中立 Inno.Build 契约，目录索引提交复用 `Inno.Core.IO.AtomicFile`，准备和发布所有权复用 `FileLease`，不引用具体平台、Web、Native 工具链或 Editor。Host 显式注册平台 source；核心不根据 target 名称选择实现。

发布在 Prepare、Validate 与指纹计算后检查取消。完整产物进入 `<target>/<fingerprint>`，最后原子替换 `current` 索引；旧目录保留，已开始的读者不会被新发布覆盖。索引提交前取消保留原选择，可能留下完整但未选中的可重建产物。

## 所有公开 API

| API | 稳定语义 |
| --- | --- |
| `IPlayerSupportPackSource.target` | source 负责的稳定平台 ID。 |
| `PrepareAsync(context, cancellationToken)` | 将完整运行时和编译输入写入隔离 staging；不能替换已安装目录。 |
| 继承的 `IPlayerSupportPackValidator.Validate(directory)` | 由平台验证所需文件和 ABI 输入。 |
| `PlayerSupportPackBuildContext(engineRoot, stagingDirectory, dotnetHost)` 及同名只读属性 | 显式提供源码、事务目录和 SDK；不暴露已安装目录。 |
| `PlayerSupportPackPublisher(sources)` | 冻结独立注册表，重复目标立即失败。 |
| `PublishAsync(engineRoot, outputRoot, target, dotnetHost, cancellationToken)` | prepare → 校验与内容指纹 → 不可变目录 → 原子 current 索引；成功返回绝对目录。 |
| `SourcePlayerSupportPackProvisioner(engineRoot, publisher)` | 使用 Host 注册的发布器检查源码并准备当前 Pack。 |
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
Host 先调用 `BuildPipeline.EnsurePlayerSupportPackAsync`，通过 provisioner 检查当前源码、SDK 与原生产物并准备；
已安装但无效的 Pack 保持明确失败。之后在 authoring owner thread 调用 `BuildGameAsync`。

## 错误与生命周期

source 抛出、取消或校验失败时删除 staging，保留原索引。每个 target 的准备使用可取消的进程间 lease；
提交阶段使用独立索引 lease。相同内容复用相同目录；已发布目录被外部修改时明确失败，不覆盖读者正在使用的输入。
不同内容发布为新目录，旧目录只能在没有读者的显式 clean 生命周期中删除。

SDK 由 DOTNET_HOST_PATH、DOTNET_ROOT 或 Host 安装位置确定，目标 source 检查自己的工具能力。整个过程没有 collectible 模块激活，游戏导出仍受 generation admission barrier 管理。
