# Inno.Rendering.Assets.Authoring

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

承载 Texture、Shader、Material、Geometry 和 Pipeline 的导入、源码冻结、图编辑、目标编译与 last-good 保存。依赖运行资产、Shaders 与 Assets Pipeline；不进入发行 Player。语言解析与原生编译继续由具体 provider 和工具链实现。

## 工作流

Importer 通过既有 Asset Pipeline 取得完整序列化与引用上下文，冻结源码、依赖及图定义。目标 compiler 接收不可变请求，返回结构化诊断及完整候选。候选验证成功后发布；失败保留相同逻辑资产的 last-good。

`ShaderSourceBundle`、`ShaderGraphArtifact` 将依赖内容冻结为 bytes，运行消费者不重新读取创作目录。SDK 必须使用文件时，由具体工具链创建短生命周期输入文件；临时路径不进入运行资产协议。

`ShaderGraphSourceStore` 管理创作源保存。`ShaderLastGoodStore` 管理可重建创作缓存；两者均不创建 GPU owner。`TextureTargetCompiler` 与 Shader compiler 是可替换边界。

## 使用示例

```csharp
using System.Collections.Generic;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering.Assets.Authoring;

static byte[] FreezeGraph(
    GraphDocument graph,
    IReadOnlyDictionary<GraphNodeId, byte[]> functions,
    SerializationRegistry serialization
) {
    return ShaderGraphArtifact.Encode(graph, functions, serialization);
}
```

注册归属本项目的 importer/provider 由 [默认创作 Adapter](../runtime/Inno.Adapter.Authoring.Default.md) 完成。项目各自维护唯一 `Properties/ScriptingApi.cs`；本层仅导出 Editor API。

## 验证与失败

`tests/rendering/Inno.Rendering.Assets.Authoring.Tests` 验证导入、图冻结、编译候选与 last-good。扩展 generation、Missing 与发布事务沿现有 Asset/TypeRegistry 协调器处理；不能为了目标编译绕过该事务。

## API 参考

[完整声明、行为与调用约束](Inno.Rendering.Assets.Authoring/api.md)

<!-- api-include: Inno.Rendering.Assets.Authoring/api.md -->

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Shaders](Inno.Rendering.Shaders.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
