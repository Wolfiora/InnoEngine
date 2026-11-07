# Inno.Rendering.Shaders API

[项目入口](../Inno.Rendering.Shaders.md)

## 公开 API

| 类型 | 入口及语义 |
| --- | --- |
| `ShaderSourceType` | `Atomic`、`ArrayOf`、`Structure`、`Storage` 创建不可变类型；`id`、`elementType`、`elementCount`、`fields`、`storage` 描述布局；`IsEquivalentTo` 同时比较名称、布局及资源访问契约 |
| `ShaderStorageType` | `Buffer(element, access)` / `Image(format, access, dimension, array)`；只读 `valueType/access/format/dimension/array/isImage`；复用 Rendering 的 `RenderStorageAccess`，不复制一套访问枚举 |
| `ShaderSourceField` | `(name, type)` 构造一个不可变结构成员；公开同名只读属性 |
| `ShaderSourceParameterDirection` | `Input`、`Output`、`InputOutput`，区别函数值流向 |
| `ShaderSourceParameter` | `(name, type, direction)`；参数以名称匹配，不能按位置偷偷接回改名端口 |
| `ShaderSourceFunction` | `(name, returnType, parameters, location)`；`HasSameInterface` 校验替代实现的返回值、参数名、方向、顺序和类型，允许实现函数名不同 |
| `ShaderSourcePosition` | `assetPath`、从 1 开始的 `line`/`column`，指向原始文件 |
| `ShaderSourceDiagnostic` | `code`、`severity`、`message`、`location`，不保留异常或解析器实例 |
| `ShaderSourceFile` | `assetPath` 与不可变 `text` 输入快照 |
| `IShaderSourceResolver` | `ReadInclude(includingFile, include)`；调用者必须接到当前资产候选、挂载权限及依赖记录 |
| `ShaderSourceRequest` | 冻结 `source`、`entryPoint`、`resolver` 和 `defines`；语言不从当前 GPU API 猜测 |
| `ShaderSourceAnalysis` | `function`、`dependencies`、`diagnostics`、`succeeded`；成功仅表示接口分析无错误，不表示 GPU 编译通过 |
| `IShaderSourceFrontend` | `languageId`、`Analyze(request)`；一种语言的词法/声明分析扩展，不能创建 GPU 资源 |
| `ShaderSourceFrontendCatalog` | 构造时捕获唯一语言 ID 的集合；`languageIds`、`Analyze(languageId, request)`；缺失前端明确失败 |
| `ShaderSourceFrontendRegistry` | `(TypeCatalog)` 绑定 owner；按 `IShaderSourceFrontend` 接口直接发现实现，`languageIds`、`Analyze` 在共同 operation scope 下工作；`Build` 与 `DisposeSnapshot` 接入候选、回滚和退休 |
| `ShaderSourceNodeDefinition` | 从函数接口创建 `inno.shader.source` 的 `GraphNodeDefinition`；`function`、`GetPorts(node)` 返回缓存的端口快照 |
| `ShaderSourceImplementationRequest` | `(implementationId, languageId, source)`；键由 owner 明确区分 Adapter/Target/variant，不从文件名推断 |
| `ShaderSourceImplementationAnalysis` | `implementationId`、`languageId`、`sourcePath`、`entryPoint`、`defines`、`sources`、`includes`、`analysis`、`contentHash`；`CreateSourceRequest()` 只通过冻结文件和 include 边重建输入，不触碰活动 resolver |
| `ShaderSourceInclude` | `includingFile`、`include`、`resolvedPath`，保留 include 别名和挂载解析结果，而不是编译时重新猜路径 |
| `ShaderSourceModuleAnalysis` | `implementations` 保留成功和失败配置；`function` 仅在全部接口一致时提供；`diagnostics` 和 `succeeded` 不表示 native 编译成功 |
| `ShaderIrValue` | builder 分配的 `index` 和完整 `type`；不是 GPU register，不能跨 builder 混用 |
| `ShaderIrOperation` | 值运算：Input、Constant、Construct、Extract、Add/Subtract/Multiply/Divide/Minimum/Maximum、Equal/LessThan、Select、SourceCall；资源：TextureSample/TextureSampleLevel、StorageLoad/StorageStore/StorageAtomicAdd、Discard；控制流：RegionInput、Branch、Loop |
| `ShaderIrInstruction` | 不可变 `operation/inputs/outputs/regions`；操作数据为 `inputName/constantBits/memberIndex/source`；`hasSideEffects` 包括保守的源码调用、内存访问和控制流，不能任意删除或移动 |
| `ShaderIrBlock` | 按求值顺序保存 `instructions` 和具名 `outputs`，不持有 builder、前端或 resolver |
| `ShaderIrBuilder` | `Input`；float/int/uint/bool `Constant`；`Binary/Select/Construct/Extract/Call/Build`；`Sample/SampleLevel/LoadStorage/StoreStorage/AtomicAddStorage/Discard`；`Branch/Loop`。全部验证类型与词法作用域，冻结后不保留构造 callback |
| `ShaderIrStageInput` / `ShaderIrInputKind` | `id`、`type`、`kind`、`semantic`、`location`；描述 VertexAttribute、Varying、Uniform、SampledTexture、Storage、Builtin |
| `ShaderIrStageOutput` / `ShaderIrOutputKind` | `id`、`kind`、`semantic`、`location`；描述 ClipPosition、Varying、Color、Depth |
| `ShaderIrStage` | `(stage, body, inputs, outputs, threadsX, threadsY, threadsZ)`；复制接口、验证输入/输出类型及唯一绑定，禁止跨阶段错误接口；`contentHash` 覆盖阶段语义、嵌套区域、源码与布局，不包含画布位置 |
| `ShaderNodePort` | `id`、完整 `type`、`direction`、`required`，与 Editor 绘制类型分离 |
| `IShaderNodeCompiler` | `definitionId`、`GetPorts(context)`、`Lower(context)`；公共算法不集中判断具体节点类型 |
| `ShaderNodeDescriptionContext` | `nodeId`、`definitionId`、`sourceModule`、`implementationId`、`stageInput`；`Read<T>(id, defaultValue)` 使用完整 owner SerializationContext；禁止跨调用保留 |
| `ShaderNodeLoweringContext` | `description`、`builder`、`inputs`、`Input(id)`；只在当前降低调用中使用 |
| `ShaderGraphLoweringRequest` | 捕获图的独立中立副本、具名 `outputs`、`implementationId`、冻结 `sourceModules` 和 `stageInputs` |
| `ShaderGraphDiagnostic` | `code`、`severity`、`message`、可空 `nodeId` / `portId`；不保存旧 provider 或异常对象 |
| `ShaderGraphLoweringResult` | `block`、`diagnostics`、`succeeded`；失败没有半成品 region |
| `ShaderNodeCompilerCatalog` | `(compilers)` 验证唯一 definition ID；`definitionIds`、`Lower(request, serialization, context, cancellationToken)` |
| `ShaderNodeCompilerRegistry` | `(TypeCatalog)`、`definitionIds`、`Lower`；整个 region 使用同一代际 operation，候选回滚/退休复用 TypeRegistry |
| `ShaderConstantNodeCompiler` | `inno.shader.constant`；原生属性 `type` 为 float/int/uint/bool，`value` 必须使用对应原生数值类型 |
| `ShaderBinaryNodeCompiler` | `inno.shader.binary`；`type` 与 `operation`，端口 left/right/value；支持 add/subtract/multiply/divide/minimum/maximum/equal/less-than |
| `ShaderConstructNodeCompiler` | `inno.shader.construct`；向量/矩阵 `type`，按序 `component.N` 输入和 value 输出 |
| `ShaderStageInputNodeCompiler` | `inno.shader.stage-input`；从 Target 捕获的输入绑定产生 value，不接受 native 变量名 |
| `ShaderSelectNodeCompiler` | `inno.shader.select`；`type`、condition/true/false/value 端口，不能用它隐藏上游副作用 |
| `ShaderSourceNodeCompiler` | `inno.shader.source`；使用已冻结模块生成 return/out/inout 与结构成员，显式拒绝聚合和成员同时连接 |
| `ShaderRerouteNodeCompiler` | `inno.shader.reroute`；强类型 value 输入/输出透传，不添加指令或改变副作用顺序 |
| `ShaderGraphDocument` | `Create/ReadDefinition` 保存和读取图的材质/Pass 契约；`Encode/Decode/Read<T>` 使用 owner SerializationRegistry/Context。`definitionKey/outputDefinitionId/stageKey/settingsKey` 为原生图协议键；不能传入临时拼接的引用上下文 |
| `ShaderGraphStageSettings` | `stage/outputs/threadsX/threadsY/threadsZ`；在 output node 中持久化完整阶段和 Compute 工作组配置，Pass 通过独立绑定引用可共享 Stage |
| `ShaderGraphOutput` | `id/kind/semantic/location`；输出端口名、GPU 目标类别与显式位置，不保存 native 表达式 |
| `ShaderGraphInputSettings` | `id/type/kind/semantic/location` 与 `CreateBinding()`；未完成设置可保存，创建不可变 binding 时严格校验 |
| `ShaderGraphType` | `id/element/length/fieldNames/fieldTypes`；storage 的 `isStorage/isImage/storageElement/access/format/dimension/isArray`；`CreateType()` 拒绝矛盾描述并生成不可变类型 |
| `ShaderGraphNodeKind` | `Function` 在 Target 与 typed lowering 前内联；`DomainOutput` 保留为领域 Target 消费的稳定边界 |
| `ShaderGraphNodeEffect` | `Pure` 要求 Function 暴露至少一个输出；`SideEffect` 允许只有输入而没有返回值，用于存储写入、discard 或插件定义的有序效果 |
| `ShaderGraphNodeSettings` | 图级节点身份：`displayName/createPath/createOrder/kind/effect/role`。元数据不属于任一方向的边界节点，因此 input-only 与 output-only 节点都可完整发现 |
| `ShaderGraphNodePortDefinition` | 图节点公开端口的稳定 `id`、完整 `type` 与输入 `required`；端口次序只负责呈现，不作为连接身份 |
| `ShaderGraphNodeInputSettings` / `ShaderGraphNodeOutputSettings` | `.ishader` 内可独立存在的 Function Inputs / Function Outputs 设置；每个方向最多一个记录，每个记录可声明多个强类型端口 |
| `ShaderGraphNodeInterface` | 从节点图冻结的标题、创建目录、顺序、类型、效果、领域 Role 和端口快照；调用图只保存中立快照与 Shader 资产引用 |
| `ShaderGraphNodes.IsNodeGraph/ReadSettings/WriteSettings` | 识别并读写图级节点身份；不再靠是否存在 Function Inputs 猜测节点资产 |
| `ShaderGraphNodes.ReadInterface/Expand` | 校验节点图并递归展开引用；支持 input-only、output-only 与双向接口，检测引用环、陈旧端口、必填输入、输出缺值、属性冲突和重复端口，不保留 Asset/provider 实例 |
| `ShaderGraphTemplates` | `CreateRaster(serialization, context)` 创建有效默认图，不新增第二种 Shader 资产格式 |
| `ShaderGraphBindings` | `ChangeInput(graph, nodeId, settings, serialization, context)` 返回独立候选，原子调整输入节点与暴露参数契约，不修改传入图；未完成输入保留为可序列化内容，调用者将整个候选记录为一次 History 修改 |
| `ShaderGraphBindings.RemoveNodes(graph, nodeIds, serialization, context)` | 返回删除候选，不修改原图；输出节点连同阶段内容删除，清理失去最后所有者的 Pass/绑定及已删除 Pass 的 Technique 映射；共享参数保留默认值并更新阶段可见性，其他未完成内容不做全图清洗 |
| `ShaderGraphProgramCompiler` | `(ShaderNodeCompilerRegistry)` 与 `Lower(...)`；验证图级定义、阶段归属与接口，按 Pass 降低；Vertex 值直接连接到 Fragment 节点时自动生成确定性的 transient varying 接口，不把生成 plumbing 写回作者图 |
| `ShaderGraphPass` | `(name, stages)`；保存有序阶段快照，公开只读 `name/stages` |
| `ShaderGraphProgramResult` | 只读 `passes/diagnostics/succeeded`；成功降低不表示 Adapter 编译或 GPU 发布成功 |

Registry 的 protected override 复用 `TypeRegistry` 契约；Registry 本身封闭，语言和节点编译器直接通过各自接口发现，不要求重复的空 marker Attribute；端口呈现仍属于 Editor。
