# Inno.Rendering.Runtime

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

本层把输出请求、运行资产和图形设备组合成可执行的帧。它拥有模型、Pipeline/Feature generation、资产到 GPU 的资源缓存及退休队列。

直接依赖运行资产和中立契约，不引用 Authoring、Assets Pipeline 或 Shader 编译工具。`ContentRenderTargetArtifactProvider` 通过逻辑内容定位读取已编译产物，运行时不编译创作源码。

## 初始化、扩展与资源退休

宿主提供 TypeCatalog、IRenderDevice、诊断和所需的 artifact provider。设备由宿主持有；Runtime 管理自己创建的模型、资源服务、输出 target 和 generation。构造函数只完成组合，帧驱动由其所属 Session 的 Subsystem 生命周期执行。

| 扩展需求 | 入口与责任 |
| --- | --- |
| 解释一种内容并生成输出 | 实现 `IRenderModel`，使用稳定模型扩展身份进入注册 |
| 定义绘制流程和可选功能 | 实现 Pipeline/Feature 扩展，声明所需设备能力 |
| 为最终帧追加中立图操作 | 注册 `IRenderFrameGraphContributor`，退出前解除注册 |
| 提供部署中的 Shader/Texture | 注入 `IRenderTargetArtifactProvider`，保持内容 owner 存活 |

Contributor 集合变化时才创建不可变 snapshot。帧开始固定一个 snapshot；帧内注册或注销影响下一帧，调用者不能在当前帧仍可能使用实例时立即释放它。注册不转移 Contributor 的生命周期所有权。

每个 Contributor 在自己的 graph mutation scope 中构建。普通扩展失败会报告诊断并回滚该 scope，已经接受的图仍保留；退休 Pending 会继续向 owner 传播。Validate 只做分析，最终 Compile 构造完整图；`RenderFrameStatistics.graphCompileCount` 用于核查实际次数。

下面只展示宿主创建 Runtime 的最小组合，不能单独替代 Session 启动和关闭流程：
```csharp
using Inno.Core.Diagnostics;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Runtime;

static RenderRuntime CreateRuntime(
    TypeCatalog types,
    IRenderDevice device,
    IDiagnosticReporter diagnostics
) {
    return new RenderRuntime(types, device, diagnostics);
}
```

主输出不可用时跳过其建图与输入换算，独立离屏输出继续运行。缺失产物、能力不足和内容损坏分别报告，候选失败保留 last-good。

退出先撤销候选并退休 generation，再清空请求和 Contributor 引用，释放输出 target、合成器、上传与资源服务，最后关闭设备帧。设备最终销毁仍属于宿主；退休 Pending 或失败不能当作正常完成。私有 scratch 保留容量，但在帧结束、异常和退出时清空 extension 引用，不是对外可持有的快照。

资源描述与 GPU 图机制见 [Core](Inno.Rendering.md)，持久资产见 [Assets](Inno.Rendering.Assets.md)，创作编译见 [Authoring](Inno.Rendering.Assets.Authoring.md)。该程序集不引用创作或编译工具，不内建 2D/3D 世界模型。

## API 参考

[完整声明、行为与调用约束](Inno.Rendering.Runtime/api.md)

<!-- api-include: Inno.Rendering.Runtime/api.md -->

## 项目依赖

- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Content](../assets/Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
