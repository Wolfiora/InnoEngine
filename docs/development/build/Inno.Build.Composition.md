# Inno.Build.Composition

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../adding-platform.md)

## 职责与边界

中立的构建组合契约。只依赖共同 Build、Managed、Support Pack、Toolchains 及必要创作输入，不注册平台、DotNet compiler 或 backend。

## 组合、生命周期与扩展

`BuildPlatformContribution` 将目标描述、打包工厂、Support Pack source、SDK provider 和可选 Editor 项目绑定成一个平台贡献。`GameBuildContribution` 再绑定内容编译工厂；`BuildDistribution` 接收完整游戏贡献，拒绝重复 ID、错配 source、错误 Editor 能力和重复 Native-only ID。没有目录扫描或自动发现。

`BuildDistribution.TryResolveNativeToolchain(targetId, out provider)` 只查询已注册的同目标 SDK 贡献。独立绑定定义可以不注册游戏发布目标；查询失败不会选择其他目标，也不屏蔽已注册 provider 的解析失败。

`BuildPipelineFactory.Create` 接收借用的作者端服务与冻结 distribution。运行目标和 `BuildCompositionContext.host` 分离；`toolsTarget` 描述作者工具的执行目标，不替代游戏目标。factory、catalog 与 provider 为借用；Pipeline 拥有每次构建事务，source 拥有准备任务。

具体注册属于 [标准发行组合](Inno.Build.Distribution.Standard.md)。共享 Editor Hosting 接收此项目的 distribution，不引用标准发行组合。新增发行或平台先组成完整贡献，然后在自己的发行入口注册。

```csharp
using System.Collections.Generic;
using Inno.Build.Composition;
using Inno.Build.Managed;

static BuildDistribution Compose(
    IReadOnlyList<GameBuildContribution> contributions,
    IReadOnlyList<IManagedDeploymentCompiler> deployments
) {
    return new BuildDistribution(contributions, deployments);
}
```

没有额外 protected 扩展点。SDK 不支持宿主/目标组合时明确失败；发布前的失败和取消保留旧完整输出。契约测试位于 `tests/build/Inno.Build.Tests/BuildCompositionTests.cs`。

平台贡献还声明目标自身的不可变 `nativeProducts`。`BuildDistribution.ResolveNativeProduct(targetId, productId)` 是普通 MSBuild Native 准备的唯一选择入口；Task 不维护 Editor/Player 的 backend 列表或产品 switch。不同平台可以给同一 product ID 选择不同闭包；未注册的产品在 SDK 解析和 staging 前明确失败。Browser 的聚合/最终链接由自身发布链处理，不注册普通 Editor Native 构建。

## 生成服务组合

BuildCompositionContext 必须提供 INativeBindingGenerator。平台目标和 Support Pack 只借用此服务；BuildPipelineFactory 将其传递给完整执行链，不引用具体 BGCS 实现。

## 一次组合的检查顺序

1. 创建 `BuildDistribution` 时冻结完整贡献，拒绝重复目标和部署实现。此时不启动 SDK、生成器或编译器。
2. `CreateBindings` 调用平台 packager 工厂，核对 ID、RID 和默认部署能力，再用当前 Assets、SerializationRegistry、TypeCatalog 创建内容 compiler。compiler 与 packager 必须具有相同目标 ID。
3. `BuildPipelineFactory.Create` 将这些绑定与当前创作服务交给 Pipeline。它不会接管 EngineHost、Assets、PluginEnvironment、Settings 或 ScriptCompiler 的释放权。
4. 请求发布时才解析所选部署与 Support Pack，并进入 Pipeline 的构建事务。注册一个目标不表示当前机器已经安装其 SDK。

`BuildDistribution` 的工厂和平台配置可以是长生命周期的不可变配置；`CreateBindings` 的结果借用了当前创作服务，不能把它当作跨 Session 的静态内容 compiler 缓存。创作 Host 必须等待使用这些服务的构建任务结束后再退休其资源。

扩展时分别提供平台贡献和内容编译工厂，再组成 `GameBuildContribution`。替换图形内容 compiler 不要求修改平台 packager；替换 SDK provider 也不要求修改共同 Pipeline。只有产品确实支持的 Native 闭包才加入 `nativeProducts`，不能凭目标名称自动推导。

## API 参考

[完整声明、行为与调用约束](Inno.Build.Composition/api.md)

<!-- api-include: Inno.Build.Composition/api.md -->

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../../modules/assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../../modules/core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../../modules/extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../../modules/runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../../modules/plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Compiler](../../modules/scripting/Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../../modules/core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
