# Inno.Rendering

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

只提供设备、能力、资源描述、命令、绑定反射和 RenderGraph 机制。仅依赖必要 Foundation 契约；不引用 Assets、References、Scene、Editor、Adapter 或 Native。不声明 Camera、Light、PBR、2D/3D 模型，也不解析资产或 Shader 源码。

## 组合与生命周期

设备由具体 Adapter 创建，宿主持有唯一设备 owner。图中的资源、pass 与 handle 只属于该图及其 generation；已提交 pass 声明冻结，不能在后续 mutation 中修改。GPU 资源由资源 owner 明确退休，资产持久化不保存 GPU handle。

`IRenderDevice.primaryPresentationSize` 必须由实现提供：有效值是真实物理像素尺寸，`null` 表示当前没有可用主输出。`SetPrimaryPresentationSize` 在帧安全点发布变化；零尺寸不是有效值。原生 SDK 内部的最小资源尺寸不代表可呈现输出。离屏目标使用自己的有效尺寸。

`Validate()` 返回 `RenderGraphValidationResult`，只执行关系、能力、初始化、依赖、cycle 和 view 数量检查。`Compile()` 复用当前 revision 的验证分析，然后分配资源并构造最终图。任何 mutation 都使过期分析失效。完整编译每个正常 Runtime 帧只执行一次。

`BeginMutationScope()` 提供 LIFO 事务：失败回滚资源、pass、output、name scope 和此前冻结的验证结果；成功 `Commit()` 验证并冻结新声明。回滚不重用已发放的资源身份，旧 handle 不能误指向新资源。

`AllowParallelRecording()` 只并行执行声明允许录制的 callback；实际命令仍按编译后的 pass 顺序回放。录制会立即复制 uniform 与 transform 的借用数据，普通 handle 和 sampler 按值保存，栅格与 stencil 状态保留当前对象语义。内部录制缓冲仅由本次 `Execute()` 拥有；正常完成、录制失败、回放失败和 backend 退出异常都释放缓冲及记录的引用。没有并行 pass 时直接执行串行 callback，不建立空录制目录。该机制不改变设备线程归属、资源 generation 或 GPU 退休协议。

## 使用示例

```csharp
using Inno.Rendering;

static RenderGraphValidationResult ValidateGraph(RenderGraphBuilder graph)
{
    return graph.Validate();
}
```

实际 Pipeline、Material 与请求组合见 [Runtime](Inno.Rendering.Runtime.md)；运行资产见 [Assets](Inno.Rendering.Assets.md)；Shader 图与 IR 见 [Shaders](Inno.Rendering.Shaders.md)。`ShaderInterface` 仅保存编译绑定事实，Material 默认值与 Asset 引用属于 Assets。

`RenderDeviceAllocationCounters` 是设备 generation 的累计分配快照，未提供测量的实现返回 null。`primaryPresentationEncodesSrgb` 报告真实输出编码能力；输出传递与图层合成程序由宿主注入。

## 验证

`tests/rendering/Inno.Rendering.Tests` 验证完整图语义、mutation rollback、revision 失效与严格二进制协议；架构验证器禁止本项目引用资产或实现层。

## 图内增量验证

已接受的 pass 冻结后，内部 frame-local workspace 保存资源初始化状态、读写依赖和 phase 关系。下一贡献只分析新增声明；完整候选仍执行输出有效性、live pass、拓扑排序、cycle 和 view limit 检查。未来 phase 可以向先前 pass 增加依赖，不能因为前缀曾通过就跳过新关系。

修改尚未冻结的已分析 pass，或事务回滚时，内部分析重置并在需要时重建。对外 Validate 结果仍是当前 revision 的不可变快照；Compile 只分配一次最终图。scratch 不进入静态缓存，也不保存 extension 对象或回调。

## API 参考

[完整声明、行为与调用约束](Inno.Rendering/api.md)

<!-- api-include: Inno.Rendering/api.md -->

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
