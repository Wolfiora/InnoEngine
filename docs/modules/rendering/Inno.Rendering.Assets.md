# Inno.Rendering.Assets

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

保存 Shader、Material、Texture、Geometry 与 Pipeline 的后端中立运行资产协议、开放 ID、序列化与目标产物。依赖 Rendering Core、Assets、References 与必要 Foundation；不引用 Runtime、Shaders、Assets Pipeline 或资产创作项目。

## 数据和所有权

`ShaderDefinition` 是可编辑声明；`ShaderAsset.SetDefinition` 捕获并验证完整嵌套状态，失败不改变原状态。getter 返回独立声明副本。Material 默认值和纹理 Asset 引用属于本层；编译后的纯绑定接口属于 Core。

Material 根据开放 `ShaderContractId`、`ShaderPassRoleId` 与设备能力选择 Technique；Resolver 不内建 sprite、PBR 或固定 Pass Tag。Pipeline 资产只保存扩展 Stable Type ID 与中立配置。GPU 创建、缓存和退休由 Runtime 独占管理。

`RenderTargetArtifactPath` 返回逻辑内容定位；产物通过 `ArtifactLease.OpenRead()` 或内容 store 读取。持久模型不公开物理路径，不保存设备 handle。

## 使用示例

```csharp
using Inno.Rendering;
using Inno.Rendering.Assets;

static MaterialPassResolution? SelectPass(
    MaterialAsset material,
    GraphicsCapabilities capabilities
) {
    return MaterialPassResolver.Resolve(material, new ShaderContractId("sample.surface"),
        new ShaderPassRoleId("sample.draw"), capabilities);
}
```

导入、图编辑与目标编译位于 [Authoring](Inno.Rendering.Assets.Authoring.md)。本项目可进入静态 Player 闭包；Authoring 不进入 Player。迁移后的持久类型保留原 Stable Type ID，逻辑脚本 namespace 仍为 `InnoEngine.Rendering`。

## 验证

`tests/rendering/Inno.Rendering.Assets.Tests` 覆盖资产 round-trip、Material contract 和 Geometry artifact；真实 GPU 发布由 Runtime 测试和 Player 验收负责。

## API 参考

[完整声明、行为与调用约束](Inno.Rendering.Assets/api.md)

<!-- api-include: Inno.Rendering.Assets/api.md -->

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
