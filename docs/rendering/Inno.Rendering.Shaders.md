# Inno.Rendering.Shaders

[Rendering 索引](README.md) · [Wiki 首页](../README.md) · [BGFX 工具链](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)

## 职责与当前状态

内置 Shader 创作层。目前已经实现语言中立的源码函数接口、多实现分析快照、函数端口推导、图节点降低、typed region 与 stage。
依赖通用 Graph、Serialization、Diagnostics、Execution、TypeRegistry 和 Rendering 的 GPU 阶段契约；不直接引用
BGFX、Rendering.Assets、Scene、Editor 或 2D 插件。
它不创建 GPU 对象，Rendering 运行时也不反向引用它。

`.ishader` 图 importer、函数源码模块和 typed program 通过资产创作与目标编译链形成运行产物。
[Shader Editor](../editor/Inno.Editor.Panel.ShaderEditor.md)、Target/Template 扩展、安全帧原子发布和当前资产替换均已接入。实际视觉与硬件验证范围见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。

## Shader 作者的三个入口

用户创作 Shader 只有三个主要入口。它们不是三套彼此竞争的 Shader 系统，而是在同一条
`Graph → typed Shader IR → Adapter source generation → native compilation` 链中承担不同层级的职责。

| 入口 | 适合谁与适用内容 | 产生什么 | 明确不负责什么 |
| --- | --- | --- | --- |
| `.ishadersource` | Shader/TA 程序员编写 Noise、采样、BRDF、滤镜等局部算法 | 一个或多个具有显式强类型输入/输出的 Source Function 节点 | 不声明完整 Shader、`main()`、Pass、Stage、Render State 或隐式 GPU 输入 |
| 编辑 `.ishader` | 艺术家与 TA 用 Shader Editor 组合完整 Shader 或可复用 Sub Graph Node | 持久化的 `GraphDocument`，包含节点、连接、参数、Stage、Pass、Technique 或可复用函数边界 | 不保存生成的后端源码，不从任意源码反向恢复画布 |
| `ShaderTarget` | 渲染程序员或高级 TA 为 PBR、Terrain、Particle 等领域定义稳定管线规则 | 编译前把领域级作者图展开为完整、普通的 Stage/Pass Graph | 不直接生成 Shader IR、BGFX/HLSL/GLSL 字符串或 GPU binary，也不调度场景 Render Graph |

### `.ishadersource`：局部算法函数

`.ishadersource` 是源码函数库，不是完整源码 Shader。Import Settings 必须显式列出公开函数；未列出的函数只作为
private helper。前端从公开函数签名建立节点端口并验证 include、类型和多实现接口一致性，函数体在公共 IR 中作为
类型化 `SourceCall` 保留，由对应 Adapter 在生成阶段嵌入。公共图编译器不会把函数体反编译成可视化节点，也不会
允许函数通过隐藏的 native global 绕过 Stage 接口。

选择规则：一个算法如果用现有可视化节点表达会过度膨胀、依赖语言特有实现，或天然适合作为稳定函数调用，就使用
`.ishadersource`。它仍必须把值、资源和副作用依赖显式暴露在函数接口中。

### `.ishader`：主要可视化创作资产

`.ishader` 是 Shader 创作的唯一权威图资产。它既可以是具有显式 Vertex/Fragment/Compute Stage、Pass 和 Technique
的完整可编译 Shader，也可以通过 Function Inputs / Function Outputs 定义可复用 Graph Node。普通 Function 子图在
导入候选中递归内联；Domain Output 子图保留为 Target 可识别的领域边界。保存写回的是 Graph 本身，Check 才把当前
图经统一 typed IR 和 Adapter 工具链编译；生成的后端源码不是可反向编辑的第二份源。

`ShaderGraphTemplate` 不是第四种 Shader 表示。Template 只在创建 Asset 时运行一次，为新的 `.ishader` 提供初始
节点、连接、Pass 和设置；生成后资产独立存在，后续编辑和编译不再调用该 Template。希望作者看见并能修改完整
Stage/Pass 结构时，应优先使用 Template 创建显式 `.ishader`，由模板声明匹配的 Contract 与阶段结构。

### `ShaderTarget`：领域级程序结构展开

`ShaderTarget` 是 generation-scoped 的 Editor/Plugin 扩展。资产只持久化与 `[ShaderTarget(id)]` 匹配的稳定 ID；每次
Import 或 Check 捕获编译候选时，Target 接收作者图的独立副本，把高层 Surface/Domain Output 契约展开成具有明确
Stage Input/Output、资源、Technique、Pass 和 Render State 的普通 `GraphDocument`，随后仍进入公共节点降低、typed
IR 和 Adapter 编译链。Target 返回 Graph，不拥有编译器后端，也不能直接注入 native Shader 字符串。

选择规则：只有一个领域需要从同一份高层 Surface 自动派生多个固定 Pass，或必须统一约束管线接口时才使用 Target，
例如从 PBR Surface 派生 Forward、Depth、Shadow 和 GBuffer Pass。简单 Sprite、Post-process、Compute 或希望所有
Stage 结构对作者可见的 Shader，可以完全不选择 Target，并直接编辑显式 Stage Graph。Target 与 `.ishadersource` 不重复：
前者决定完整程序骨架，后者只实现骨架中被调用的局部算法。

```text
.ishadersource function ───────────────┐
                                      │ SourceCall node
authored .ishader ── reusable graphs ─┼─→ expanded stage graph ─→ typed Shader IR ─→ Adapter ─→ GPU binary
                                      │
optional ShaderTarget ─ domain expand ┘
```

实际项目通常组合三者：艺术家在 `.ishader` 中连接节点，高级 TA 用可复用 `.ishader` 和 `.ishadersource` 封装复杂算法，
渲染程序员只在需要隐藏并强制执行领域管线结构时提供 `ShaderTarget`。

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

## 图定义的可复用节点

普通 Shader 作者不再需要为了组合节点编写 `IShaderNodeCompiler` 和专用 Inspector。用 File Browser 的
**Create / Shader / Reusable Node** 建立普通 `.ishader`。节点身份与目录位于图级 `ShaderGraphNodeSettings`；
**Function Inputs** 和 **Function Outputs** 各自可选、每个方向最多一个，但两者至少存在一个且总端口数不能为零。
因此常量生成器可以只有 Outputs，存储写入或 discard 包装可以声明为 `SideEffect` 并只有 Inputs，普通计算节点可同时拥有两者。
端口 ID 是持久连接身份，改名不会按位置误接旧边。
保存后，该资产自动出现在其他 Shader 的 **Create / Graph Nodes** 菜单，目录和顺序由节点图自身的
`createPath` / `createOrder` 决定。

`Function` 节点在导入候选中递归内联，调用方默认值、子图属性声明、源码函数依赖和调用阶段一起进入同一
authoring artifact。运行时只消费已编译程序，不加载、遍历或解释节点图。循环引用、缺少必填输入、无值输出、
冲突资源声明或调用方残留的旧端口会使当前导入/Check 明确失败，原始图仍保留用于修复。

作者可以直接把 Vertex 节点输出连到 Fragment 节点输入。降低阶段按完整类型推导桥接值，生成 Vertex varying
输出与对应 Fragment varying 输入，并为同一来源的多路消费复用一条桥；作者图、Undo 和语义 hash 不保存这些
实现 plumbing。资源句柄不能跨阶段传递，Compute 与 Raster 之间以及 Fragment 反向到 Vertex 的连接明确拒绝。
Editor 的 **Organize / Collapse to Subgraph** 会从同一 Stage 的选择推导外部边界，创建新的 `.ishader` 节点资产，
并在原图中以一个调用节点替换选择；输入、输出、属性声明和多路连接均按稳定端口身份保留。

`DomainOutput` 用来定义领域终点，例如某插件的 Surface Output。它声明输入和稳定 `role`，没有通用引擎领域分支；
插件 Target 只按 Role 识别该边界并把它展开成公共 Stage/IR。这样普通组合逻辑留在 `.ishader`，只有真正安排
阶段接口、资源语义和领域 Contract 的部分需要 Target 代码。`IShaderNodeCompiler` 仍保留给无法由既有图与公共 IR
表达的新原语，而不是每个便利节点的默认扩展方式。

## 脚本扩展边界

本项目唯一的 `Properties/ScriptingApi.cs` 显式导出节点/前端扩展契约、描述对象和 typed IR，逻辑命名空间为
`InnoEditor.Rendering.Shaders`，全部是 Editor scope。Registry、后台编译 owner 和 Adapter 实现不暴露给普通脚本。
运行时脚本不能引用这些创作类型；Player 仍只需要运行时接口和预编译产物。
图读写工具 `ShaderGraphDocument`、`ShaderGraphBindings`、`ShaderGraphTemplates` 已导出到 Editor 脚本，
配合宿主提供的 SerializationRegistry 与完整引用上下文使用。脚本不能构造 Registry 或捕获 converter generation。
自定义节点绘制通过独立 `Inno.Editor.Shaders` 的 `ShaderNodeDrawContext.Read/Write` 使用中立数据，与编译扩展独立。

## Target 与模板

| API | 契约 |
| --- | --- |
| `[ShaderTarget(id)]` 与 `ShaderTarget.Expand(context, cancellationToken)` | Attribute 提供构造前可读取的唯一稳定 ID；基类只承载行为。插件将领域输出展开为已有阶段图，声明 Contract/Role 和绑定，不生成原生 Shader 字符串 |
| `ShaderTargetContext.document/serialization/references` | 独立原图副本、借用的 owner converter 和完整引用上下文，只在调用期间有效 |
| `ShaderTargetRegistry(types).ids/Expand/Dispose` | 宿主的 generation-scoped 发现与调用；Missing Target 明确失败且不改原图 |
| `ShaderGraphDocument.targetKey/ReadTarget/SetTarget` | 持久化稳定 Target ID；未指定领域 Target 的图显式创作通用阶段 |
| `[ShaderGraphTemplate(id, displayName)]` 与 `ShaderGraphTemplate.Create` | Attribute 提供不可变创建元数据；基类只承载创建行为，并向 File Browser 贡献 Shader 创建模板 |
| `ShaderGraphTemplateInfo(id, displayName)` | 不含 provider 的菜单快照 |
| `ShaderGraphTemplateRegistry(types).templates/Create/Dispose` | 按稳定 ID 创建独立图，重复 ID 或缺失模板明确失败 |

Target 在导入时先展开，随后捕获其引入的源码与资产依赖，进入同一公共 IR 和 Adapter 编译链。
Authoring Artifact 同时保存原图与展开图；Export/Editor 读取原图，编译读取展开图。目标生成的节点位置不影响语义指纹。
Target 与 Template 的 provider 只存活于 TypeRegistry snapshot，公共菜单快照只含字符串；运行时不解释 Target 或图。

Shader Target、Shader Template 与 Render Pipeline 使用同一条身份规则：继承表达行为，参数化 Attribute 是不可变 Stable ID 的唯一来源。Registry 先读取元数据、校验重复 ID，再构造扩展；扩展实例不再暴露返回常量的 `id` 覆盖成员。

```csharp
using System.Collections.Generic;
using InnoEngine.Graphs;
using InnoEditor.Rendering.Shaders;

public sealed class HalfNodeCompiler : IShaderNodeCompiler
{
    public string definitionId => "project.shader.half";
    public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context)
        => [new("value", ShaderSourceType.Atomic("float"), GraphPortDirection.Output)];
    public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
        => new Dictionary<string, ShaderIrValue> { ["value"] = context.builder.Constant(0.5f) };
}
```

以上代码放入 `.editor.cs`。已验证真实脚本编译与生成 IDE 项目的裁剪引用均只向 Editor 暴露该接口；
这是 API 可扩展性验证，不代替 Shader Editor 中自定义节点的完整绘制、创建、热重载和卸载验收。

## 注册调用

Catalog 与 Registry 另有 `AnalyzeModule(implementations)`。Registry 在整个多实现分析期间持有同一 operation scope，
不能在两个语言实现之间切换 generation。缺失语言返回可恢复诊断；空候选集或重复 implementation key 属于调用错误。

## 组合示例

```csharp
using Inno.Rendering.Shaders;

static ShaderSourceAnalysis Inspect(
    IShaderSourceFrontend frontend,
    string languageId,
    ShaderSourceRequest request
) {
    var frontends = new ShaderSourceFrontendCatalog([frontend]);
    return frontends.Analyze(languageId, request);
}
```

具体前端在所属工具链组合，使用不同稳定语言 ID 注册，不需要向公共编译器添加语言分支。
该项目借用中立请求和受控 resolver，不解析某种第三方语法。

## 多实现分析与依赖快照

- 调用者列出需要一致校验的 Adapter 实现及条件编译配置；公共层不翻译 native 代码，也不擅自枚举未知宏组合。
- 返回类型、参数名、方向、次序和聚合布局必须一致；函数的私有实现名允许不同。
- `AnalyzeModule` 用受控 resolver 捕获全部成功读取的源；同一路径在一次分析中返回不同内容会失败。
- 前端成功结果中声明但未通过 resolver 读取的依赖会失败，避免稍后编译偷偷读另一个文件版本。
- `contentHash` 覆盖语言、入口、源路径、全部源文本、include 解析边及按 key 排序的 defines，忽略实现集合的枚举顺序。
  它只是输入指纹，**不是**完整编译缓存 key；图语义、Target、工具链身份和资源布局仍须纳入最终编译 key。
- `ShaderIrStage.contentHash` 覆盖降低后的类型、值/副作用顺序、所有嵌套区域、绑定和源码输入；移动节点不改变它。
  它同样不是完整缓存 key，调用者还必须加入工具链身份、能力、优化和变体配置。
- 源码错误不能吞掉共同的 retirement barrier；缺失语言恢复后可重新分析原始中立输入，无须丢弃函数引用。

## Typed IR 的当前边界

当前 typed region、嵌套分支/计数循环与 stage 已由 ShaderGraphProgramCompiler 组合，并接到 `.ishader` importer、Target 展开、资源/控制流节点 UI 与原子发布。底层 IR 能力仍不等于某个领域的高层创作节点；这些由对应渲染插件贡献。
常量保存 IEEE/整数位模式而不是 native 字符串；输入使用中立名称；聚合保留完整成员类型；参数按语义名称检查后，
调用操作按函数声明顺序传参，返回值在前、out/inout 在后。`Build` 冻结快照，之后继续编辑 builder 不改变旧结果。

```csharp
using System.Collections.Generic;
using Inno.Rendering.Shaders;

static ShaderIrBlock BuildBrightness()
{
    var builder = new ShaderIrBuilder();
    ShaderIrValue brightness = builder.Input("brightness", ShaderSourceType.Atomic("float"));
    ShaderIrValue result = builder.Binary(ShaderIrOperation.Multiply, brightness, builder.Constant(2f));
    return builder.Build(new Dictionary<string, ShaderIrValue> { ["brightness"] = result });
}
```

- 运算不做隐式数值转换；`Multiply` 是逐分量乘法，不暗示矩阵线性代数乘法。
- 矩阵构造按列排列标量；矩阵 `Extract` 取列向量。结构与数组保留精确布局、次序和长度。
- `Select` 只选择已经求出的值，不是分支，也不能借它抑制另一侧的源码副作用。
- `Branch` 才是真正条件执行：两个回调输出名和类型必须完全一致，只有选中分支的指令执行，合并值才对外可见。
- `Loop` 的 uint 次数在进入循环前求值；每次迭代提供索引和具名 carried state，输出同时替换状态，零次返回初值。
  允许空输出/空状态的纯副作用区域；禁止将资源句柄当普通合并值。嵌套 builder 在回调返回后关闭，不能编辑父层或使用兄弟层的未合并值。
- `Sample` 使用 fragment 隐式导数；其他阶段必须用 `SampleLevel`。`Discard` 仅用于 Fragment，阶段检查递归进入所有嵌套区域。
- Storage buffer 使用 uint 下标；2D image 使用 int2，array/3D 使用 int3；图 IR 检查元素、格式及读写权限。
  `StorageLoad` 也保留内存顺序，不能把写入前后的两次 load 合并；原子加返回原值，只允许 read-write int/uint buffer。
- 所有源码调用保守视作有副作用；即使返回值无人使用，也保留原始调用顺序。本批次不做 DCE 或跨调用重排。
- 图降低按稳定拓扑顺序执行，独立节点以文档顺序排序；不删除未使用的源码调用。输入索引线性建立，不按每个节点全表搜索连线。
- Stage 支持显式 raster/compute 接口、uniform/采样纹理、storage、MRT 与工作组；图 artifact 冻结源码依赖并调用 typed 编译链。Target、图级资源/控制节点接线和定义/产物使用同一发布代际。
- 创作流程统一经过图和 typed IR；内部 Shader、目标编译与产物缓存的实际回归范围见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。

### 图输入默认值

`ShaderGraphLiteral.Zero(type)` 为支持的标量、向量、浮点矩阵、结构体和固定数组创建精确位模式默认值。
`type` 保存完整类型，`scalarBits` 以声明/矩阵列顺序保存 IEEE float、int、uint、bool 的原始位；`GetScalarTypes()` 校验形状，`Emit(builder, expectedType)` 发出同一公共 IR。
函数节点的 required 输入必须由图内连线提供；常量同样是图节点，不能由 Inspector 在节点外覆写。optional 输入未连接时由编译器自动生成该端口类型的零值，Editor 只显示这一事实，不提供第二套默认值编辑入口。历史文档中的 `input-default.<portId>` 数据不会参与求值。
不能给 storage、opaque texture 或效果顺序令牌生成零值；这类端口不能声明为 optional。类型变更要显式修复，不做隐式转换。`ShaderGraphLiteral` 仅导出给 Editor 创作脚本。

## 端口与生命周期

- 输入为 `input.<name>`，输出为 `output.<name>`，非 void 返回值为 `return`。`inout` 生成两个端口。
- `void` 只用于无返回值；参数、结构体成员和数组元素均不能使用它，也不能用 alias/嵌套数组绕过值类型检查。
- 结构体保留聚合端口，同时提供 `.member` 端口；展开/折叠不改变边的身份。数组保留固定元素类型和长度。
- 聚合端口类型标识包含完整布局的确定性 hash，不能把不同长度数组当作同一种图类型。
- lowering 已验证聚合与成员冲突、按名称发现失效连接和缺失编译器恢复；原图不被修改。悬挂端口在 Editor 中保留稳定身份并提供显式修复/重绑定，不按位置误接。
- Catalog 是 owner 持有的不可变注册快照，不是全局单例。独立组合时调用者负责 provider 生命周期；
  `ShaderSourceFrontendRegistry` 则使用共享 TypeRegistry 发现、候选冲突回滚和 provider 退休，解析期间
  持有共同 operation scope。现有测试覆盖该注册闭环，不等于真实 collectible 插件、Editor 文档和 GPU 发布的完整热卸载验收。
- 持久化只能保存稳定 ID 与中立值，不能保存 frontend、resolver、图定义对象或解析器。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Rendering.Shaders.IShaderNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.IShaderNodeCompiler 940441be9c82516ef28a86b0f7341e5675f500ef76526f444a440a92b863445c -->
<!-- api: P:Inno.Rendering.Shaders.IShaderNodeCompiler.definitionId a2dbdd40d9ce70ae225322a37cf43c4b15e31560f6649484f944b81c569e751f -->
<!-- api: M:Inno.Rendering.Shaders.IShaderNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 28b67450c6cc4f79237dae702c1801878ebaf3c59de4f816cea370c4ddeb6dcf -->
<!-- api: M:Inno.Rendering.Shaders.IShaderNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) 9ec41eca75a8707529a24f7a4d6dd541095a5c583bc16ceca4082428ec338248 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-588651202a6768bb"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.IShaderNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L50) | Describes ports for the current neutral properties and resolved source/target inputs.<br>返回：Complete typed ports; duplicate identities are invalid.<br>`context`：Invocation-scoped node properties and frozen source/target data. | — | — |
| <a id="api-808a8a24dbcec64c"></a>[`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.IShaderNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L60) | Lowers the node into the supplied typed builder without generating source strings.<br>返回：Exactly the declared output ports by stable identity.<br>`context`：Connected typed inputs and the shared region builder. | — | — |
| <a id="api-5dd65f3e87d018f9"></a>[`string Inno.Rendering.Shaders.IShaderNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L40) | Gets the exact graph node definition identity implemented by this compiler. | — | — |
| <a id="api-ba9fb136c54b929a"></a>[`public interface Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L35) | Defines one shader node's typed ports and lowering; drawing belongs to a separate editor extension. | — | — |

### `Inno.Rendering.Shaders.IShaderSourceFrontend`

<!-- api: T:Inno.Rendering.Shaders.IShaderSourceFrontend 7c5413d52ecf2fdf9a23f34e4f59070eeb49993857c940e8e6a0498c063b585f -->
<!-- api: P:Inno.Rendering.Shaders.IShaderSourceFrontend.languageId 750947e51224b2a559a4e262dd9b52ac4c2ca8f92e04ab836a4488c15402effd -->
<!-- api: M:Inno.Rendering.Shaders.IShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest) 38a80b6b18e658d2cfb90803f79e6a721f45ac04f5078ff00bbe4ceaa5bc5cbb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a82047ad7900bf53"></a>[`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.IShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L202) | Analyzes an immutable source candidate with explicit preprocessing inputs.<br>返回：A successful interface or a recoverable set of original-source diagnostics.<br>`request`：Source, selected function, dependencies, and target inputs. | — | — |
| <a id="api-f80725ebc5409f72"></a>[`string Inno.Rendering.Shaders.IShaderSourceFrontend.languageId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L192) | Gets the open source language identity, distinct from a rendering backend or GPU API. | — | — |
| <a id="api-18214149aef080e5"></a>[`public interface Inno.Rendering.Shaders.IShaderSourceFrontend`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L187) | Parses one source language into canonical function interfaces; it does not create GPU objects. | — | — |

### `Inno.Rendering.Shaders.IShaderSourceResolver`

<!-- api: T:Inno.Rendering.Shaders.IShaderSourceResolver 3abc2f5eac8dba26e193c4506788c9d2312be102ad9257e3d47551c341e2ee7a -->
<!-- api: M:Inno.Rendering.Shaders.IShaderSourceResolver.ReadInclude(System.String,System.String) 46077455d82c70ccfe32a5e92ec55a7fc1e5b0e83348b839cf68120663f66d87 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dee207d9ff3966bf"></a>[`Inno.Rendering.Shaders.ShaderSourceFile Inno.Rendering.Shaders.IShaderSourceResolver.ReadInclude(string includingFile, string include)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L79) | Resolves and reads an include without bypassing asset mount permissions.<br>返回：A resolved immutable source snapshot.<br>`includingFile`：Resolved path of the including source.<br>`include`：Source-local include spelling. | — | — |
| <a id="api-6187b4034cd4ddd5"></a>[`public interface Inno.Rendering.Shaders.IShaderSourceResolver`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L65) | Reads dependencies only from the current source candidate and records their import dependencies. | — | — |

### `Inno.Rendering.Shaders.ShaderBinaryNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderBinaryNodeCompiler 6069672db308fb8bf1429de515c829d6dc8a9c8a30d2323760bf9868967512c1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.definitionId 048d290cb1c7cfe5e6538590732860d7b1d858bbc92eb7c7825d3cb42124ae41 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 4875f2548f0286c6d1316bb3bc600eeacca2a5cecf298ce4ad2b6ff45957252e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) c07808daaa9e923afe15975c553dfee8409dcebc6e83388ac00a8f04923664a6 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.#ctor 4ba4d2dae2689af46f003c0a6d459bb9b3041af70b54008a7181620d1ed0eeb4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e1a3caf95b65e151"></a>[`public Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.ShaderBinaryNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L55) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e6f80540a994ed2c"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L70) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-bece33cc265d4e59"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L86) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-0fa9a5660c9fa3b1"></a>[`public string Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L60) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-381205c6f4f03bfe"></a>[`public sealed class Inno.Rendering.Shaders.ShaderBinaryNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L55) | Lowers an explicit arithmetic/comparison operation; operation IDs are node configuration, not backend code. | — | — |

### `Inno.Rendering.Shaders.ShaderConstantNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderConstantNodeCompiler 8239cd4f050dd7049a07045ef69514a7b5831386e21ec82911352bd7d422b2b2 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderConstantNodeCompiler.definitionId ccdf2d1bd9b66417970961d7545b57dc1cf0f864a8c3f26d82b8c58d2da899ae -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstantNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 824f4fae9c5e2bd40c33d3fb45afa65d1b0fcc9cf89632709346dc376dcadcfb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstantNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) 6e371fc0f6be4d34694506846c4bf733effc9803c8dc5537477392e880ed0fe9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstantNodeCompiler.#ctor 4f7fe4f85c6f9d60cc646642cfe36d07cfe12dcdccfdcc744fccab4a084d860f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c21b842a9528d3d2"></a>[`public Inno.Rendering.Shaders.ShaderConstantNodeCompiler.ShaderConstantNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L11) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ba11b0aed48f7f14"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderConstantNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L26) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-0ae520225320c934"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderConstantNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L37) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-94e8b43ffa4198c3"></a>[`public string Inno.Rendering.Shaders.ShaderConstantNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L16) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-876bb82bc7aa2b55"></a>[`public sealed class Inno.Rendering.Shaders.ShaderConstantNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L11) | Lowers an exact scalar constant; its type/value are native graph properties. | — | — |

### `Inno.Rendering.Shaders.ShaderConstructNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderConstructNodeCompiler 7ae58e125bc35e59ebd15d1d8aa6415e1fb52c66edde3d5807c6f10dca4aad7f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderConstructNodeCompiler.definitionId b7c99f2409b7d918617e22c2a138e5338377a005cdb4a546fce1564f261053a0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstructNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) c35dece3da7e8f9594dd0f99a9e38ec8fa2854b5cd671f61fc9ed58d8aa6d12f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstructNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) 4378c8263b1dee98591660171fd203969ac0ba9562a2ec9a61c82b86e6de46d0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderConstructNodeCompiler.#ctor 6bfd92352f2c042ab85e99969fbd28ac9b094618ca985a2942ab6f4981efd4bf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b496db049b3f57a9"></a>[`public Inno.Rendering.Shaders.ShaderConstructNodeCompiler.ShaderConstructNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L103) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9693672523a3ffcd"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderConstructNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L118) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-046460084cf8e836"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderConstructNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L133) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-c568809eb4dd70d0"></a>[`public string Inno.Rendering.Shaders.ShaderConstructNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L108) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-bbffeaf8f5421317"></a>[`public sealed class Inno.Rendering.Shaders.ShaderConstructNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L103) | Constructs a vector or column-major matrix from individually connected scalar components. | — | — |

### `Inno.Rendering.Shaders.ShaderExtractNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderExtractNodeCompiler d0c45373ab683333690481a2701605c5ea304a327ab71d928d63f96011729ded -->
<!-- api: P:Inno.Rendering.Shaders.ShaderExtractNodeCompiler.definitionId ffb31ae031dd1e6152ab5e9bec11e78cc299d3d403bd214ce2ba53bf16d69a66 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderExtractNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) f4e142122b769c09f7ad7b33cbe9bba4adce2c9e9c785d342337cb98ce650440 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderExtractNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) a3f484d9947b512888912b8f1354aa5921b5f41751aab5165363d0f0042473aa -->
<!-- api: M:Inno.Rendering.Shaders.ShaderExtractNodeCompiler.#ctor f75ffb494c74fb653e74e3c26a3682e01f60a91981381f3c791dcf7c815cdac6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-676fb7bb251e574a"></a>[`public Inno.Rendering.Shaders.ShaderExtractNodeCompiler.ShaderExtractNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7777f6b3cd9b9b47"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderExtractNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L25) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-59d767e54fa01957"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderExtractNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L41) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-df7f2f055f1c7413"></a>[`public string Inno.Rendering.Shaders.ShaderExtractNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L15) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-804326ad984bf5d1"></a>[`public sealed class Inno.Rendering.Shaders.ShaderExtractNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L10) | Extracts one statically selected component from a vector or matrix. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphBindings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphBindings 901f047b518977a8ac3cb472194b7a833d403ffd80678fa86a343ca1987bad5d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphBindings.RemoveNodes(Inno.Core.Graphs.GraphDocument,System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) dcc38400a957efdb1afe176a57c7dc5e840ac359cccaed876ae75a37a50ca95a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphBindings.ChangeInput(Inno.Core.Graphs.GraphDocument,Inno.Core.Graphs.GraphNodeId,Inno.Rendering.Shaders.ShaderGraphInputSettings,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) b798b5f2e27b91e649bf41fbbfe5bdd44528d79e21ac8213009e7845be7f2690 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d53802c0cb1058f7"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphBindings.ChangeInput(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Graphs.GraphNodeId nodeId, Inno.Rendering.Shaders.ShaderGraphInputSettings settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L135) | Creates a detached candidate with the input and its material/resource declaration updated together.<br>返回：A serializable candidate. Invalid input remains visible for normal graph validation.<br>`graph`：Current shader graph, never mutated by this operation.<br>`nodeId`：Stage input node identity.<br>`settings`：New input settings, including incomplete values while typing.<br>`serialization`：Owner native converter registry.<br>`context`：Complete owner reference context for texture defaults. | — | `System.ArgumentException`：The identity does not name a stage input node. |
| <a id="api-c312d7bf83962f7e"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphBindings.RemoveNodes(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L36) | Removes selected nodes, stage-owned nodes and declarations which lose their last graph owner.<br>返回：A detached candidate suitable for one atomic history entry; unrelated incomplete content is retained.<br>`graph`：Current shader graph, never mutated by this operation.<br>`nodeIds`：Explicitly selected node identities. Deleting an output also deletes its stage contents.<br>`serialization`：Owner native converter registry.<br>`context`：Complete owner reference context for material defaults. | — | `System.ArgumentException`：A selected identity is not in the document. |
| <a id="api-442f820ba6ab6558"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphBindings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L13) | Keeps graph input edits and the material-visible parameter contract in one neutral document change. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler 24f4f84a329259f1ffb112cdf7ea509b8ee53a2cada7b9bdb886aa0d9c15e3ce -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.definitionId bec9c03543077982f612073ac50c7d3c7c5e7b76a4ae6e34d1acf4f9e6475a14 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 02826426ca8445fa2cb74dec9557c3a1b4d90f079c9afd3cf811757576f9c3eb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) cc7ea230a09386e8de8df2bc8ba8b5e29db0a42684cdae52eef0439883f2cc4c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.#ctor 78cb3f1a7d07475ab2767e19fdd0d259c32ba200b8e0a78cb4527c2f48a3a090 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f341108f9e5d4f8b"></a>[`public Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.ShaderGraphCallNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L601) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5ffaa7f393698501"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L616) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-14deccebb422e952"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L627) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-ec64400b403fe79c"></a>[`public string Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L606) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-6ca5717da7dd6f7f"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L601) | Describes graph-authored node references before they are expanded or consumed by a Target. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphClipboard`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphClipboard 8a470ebe36ffb8b73efd118c77c0c22dda48094b18c083f48014f48274eb883b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphClipboard.Copy(Inno.Core.Graphs.GraphDocument,System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) e2de3b765a2153b93ac31319bfc5e5c4032465bb82da3cfa833d2348c90ac50f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphClipboard.Paste(Inno.Core.Graphs.GraphDocument,Inno.Core.Graphs.GraphDocument,System.Boolean,System.Nullable{Inno.Core.Graphs.GraphNodeId},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 42718c3209f70ec808a6107c8592f3a626ce70a2318eab0b68e4866fbb40affd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-334f7365fd366a21"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphClipboard.Copy(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L33) | Captures selected nodes and the owned contents of selected stage outputs.<br>返回：Detached fragment retaining source declarations for a later paste.<br>`graph`：Source document, never changed.<br>`nodes`：Selected stable identities.<br>`serialization`：Current native converter registry.<br>`context`：Owner reference context. | — | — |
| <a id="api-56864a7e46110bda"></a>[`public static Inno.Rendering.Shaders.ShaderGraphPasteResult Inno.Rendering.Shaders.ShaderGraphClipboard.Paste(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Graphs.GraphDocument fragment, bool preserveExternalStageReferences, Inno.Core.Graphs.GraphNodeId? activeStage, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L73) | Creates a detached paste candidate with new node identities and remapped program, Pass and Technique references.<br>返回：A complete candidate suitable for one History transaction.<br>`graph`：Destination document, never changed.<br>`fragment`：Detached copied fragment, never changed.<br>`preserveExternalStageReferences`：Whether references to existing destination stages belong to the same document.<br>`activeStage`：Destination stage for copied non-stage nodes.<br>`serialization`：Current native converter registry.<br>`context`：Owner reference context. | — | — |
| <a id="api-145c099c6f9035a1"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphClipboard`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L13) | Copies and pastes shader structures as detached atomic candidates, including their parameter and Pass contracts. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphDiagnostic`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphDiagnostic 7837acff347f1d06016a28e8118a944504e538b75296accbbc98783cfd90d493 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.#ctor(System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,System.Nullable{Inno.Core.Graphs.GraphNodeId},System.String) f130ddb6824c98543e3149c1bf4445e4a5959665ad62e1c12aaaa8835c9eb168 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphDiagnostic.code dbc75aba75aef2be1ffe9266efbc96610a1a67134bb1f795a6a6b1202a758e79 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphDiagnostic.severity f3a22db2d0ec5b72c7e9f8ae72b026d68ee8c09d3e1254d619d6b7fdabfb8e47 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphDiagnostic.message bcccda7ffdf00b5e4bc6fccec6d82efc54b2d987c4e324fdc6cd8f0671d581df -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphDiagnostic.nodeId 4470250267995a9410c455e7113407b220a013fe6fb29de6c6e5dda86720e38e -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphDiagnostic.portId 10194821cc32ec7e3fe39a81d30b540712dc29f31e7075ed4988660b2c10b76e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.ToString 65fbd9ee04e78eeee85905755fb21ca9dd2d1f7ed8511b31d1109ab00745d1db -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.op_Inequality(Inno.Rendering.Shaders.ShaderGraphDiagnostic,Inno.Rendering.Shaders.ShaderGraphDiagnostic) 53efef2f792fd1d7e558579e5fc4f192a6299d18ad2110aaef15a4d4f7ff9d4f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.op_Equality(Inno.Rendering.Shaders.ShaderGraphDiagnostic,Inno.Rendering.Shaders.ShaderGraphDiagnostic) 508a0ca9cf14d1278cfd99a02dcf8a11bb94eb9332bb512b0c10a799ad900a31 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.GetHashCode efefb79a27589330911363ba59d2860256bc7cd50d2dee94233b756a81872c23 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.Equals(System.Object) 4e99b77cc2472809e3cc8ea9b3e051cb32890d59ff06062be715f58e3b66ed8f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.Equals(Inno.Rendering.Shaders.ShaderGraphDiagnostic) d699f43d3c46a0f9013bb0abfb40dbdf090f29eb6c24898dd02d89a06aeb635a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDiagnostic.Deconstruct(System.String@,Inno.Core.Diagnostics.DiagnosticSeverity@,System.String@,System.Nullable{Inno.Core.Graphs.GraphNodeId}@,System.String@) 936aa4e6f03a6096928dd0c39cd714d880b28d103de24598a02e3dd9c79bc4d6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d4f1f223d0bcfa03"></a>[`public Inno.Rendering.Shaders.ShaderGraphDiagnostic.ShaderGraphDiagnostic(string code, Inno.Core.Diagnostics.DiagnosticSeverity severity, string message, Inno.Core.Graphs.GraphNodeId? nodeId = null, string? portId = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Identifies a graph compilation problem without retaining a node or compiler instance.<br>`code`：Stable diagnostic identity.<br>`severity`：Diagnostic severity.<br>`message`：Human-readable explanation.<br>`nodeId`：Affected node identity when known.<br>`portId`：Affected semantic port when known. | — | — |
| <a id="api-dc0f1c5e135f9857"></a>[`public void Inno.Rendering.Shaders.ShaderGraphDiagnostic.Deconstruct(out string code, out Inno.Core.Diagnostics.DiagnosticSeverity severity, out string message, out Inno.Core.Graphs.GraphNodeId? nodeId, out string? portId)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-a2795128924fe995"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphDiagnostic.Equals(Inno.Rendering.Shaders.ShaderGraphDiagnostic? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a34580dc9413c05f"></a>[`public override bool Inno.Rendering.Shaders.ShaderGraphDiagnostic.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-069d92db806b489f"></a>[`public override int Inno.Rendering.Shaders.ShaderGraphDiagnostic.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-fd46c7cadecd802d"></a>[`public static bool Inno.Rendering.Shaders.ShaderGraphDiagnostic.operator ==(Inno.Rendering.Shaders.ShaderGraphDiagnostic? left, Inno.Rendering.Shaders.ShaderGraphDiagnostic? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6ab20b0da452db5e"></a>[`public static bool Inno.Rendering.Shaders.ShaderGraphDiagnostic.operator !=(Inno.Rendering.Shaders.ShaderGraphDiagnostic? left, Inno.Rendering.Shaders.ShaderGraphDiagnostic? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-bf51c4a125bbd07d"></a>[`public override string Inno.Rendering.Shaders.ShaderGraphDiagnostic.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-a15ba4f096d2045b"></a>[`public string Inno.Rendering.Shaders.ShaderGraphDiagnostic.code { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L91) | Stable diagnostic identity. | — | — |
| <a id="api-24373cb2a43d04c2"></a>[`public string Inno.Rendering.Shaders.ShaderGraphDiagnostic.message { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L93) | Human-readable explanation. | — | — |
| <a id="api-995996b209a81e13"></a>[`public Inno.Core.Graphs.GraphNodeId? Inno.Rendering.Shaders.ShaderGraphDiagnostic.nodeId { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L94) | Affected node identity when known. | — | — |
| <a id="api-cb1d0e4a1901505a"></a>[`public string? Inno.Rendering.Shaders.ShaderGraphDiagnostic.portId { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L95) | Affected semantic port when known. | — | — |
| <a id="api-e1ed83c2442a5008"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.Shaders.ShaderGraphDiagnostic.severity { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L92) | Diagnostic severity. | — | — |
| <a id="api-8ab368e5851ba32f"></a>[`public sealed record Inno.Rendering.Shaders.ShaderGraphDiagnostic : System.IEquatable<Inno.Rendering.Shaders.ShaderGraphDiagnostic>`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Identifies a graph compilation problem without retaining a node or compiler instance.<br>`code`：Stable diagnostic identity.<br>`severity`：Diagnostic severity.<br>`message`：Human-readable explanation.<br>`nodeId`：Affected node identity when known.<br>`portId`：Affected semantic port when known. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphDocument`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphDocument 8fa91b9df6b70dd4376624131bee76a972379b4748e847eb698804203613d542 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.definitionKey b0df78460d8c4260605c43fecbab1a372c691b7bd92ac63004556411296b131f -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.targetKey 9374fa3944e206a88baabfb371b9fec1d032934b11ca2ee0890f316535269d4e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.ReadTarget(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) d6d506b113effab6f106e09de5a6e7e950d395e4a254cfc4db1145294c938e16 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.SetTarget(Inno.Core.Graphs.GraphDocument,System.String,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 18c7569beaea4312902075f073ea43816b7a58a71e9099e6407682af93a21100 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.outputDefinitionId 77cfa1432364ca6f01083e32b37c1a462bdb56d0d1b514404885fbd5fbf31937 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.stageKey bfdc587063263594055b621641182858ae937377778e08063e50bed860f2c340 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.settingsKey d442c90dc0e8f0602336f06092e3b315aefcf4b628bc152cadc379ca46ef1895 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphDocument.inputDefaultPrefix 3bc7cdc3bad9c7239407df50bf4c51ec4b345c6a9a349844bc019d944e0d9615 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.Create(Inno.Rendering.Assets.ShaderDefinition,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) a6e2a76bf7f91baa7a2c32bd3cb6a3b6539a1ced4e8b155724159781c12b686a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.ReadDefinition(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) f300a0964c11fe3f1d25c85effbb0b05f9a4b079a0ebe44192d4d06875a6a3d1 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.Encode``1(``0,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) f923bf8ddc9c601be3441da5869a15b9039e33ed543c79ba2d2e60028929bdd8 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.Decode``1(Inno.Core.Graphs.GraphSerializedValue,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 4b3243ea60acf95a5758d88304ad58d2d035c9196fdc5048970f598936611e99 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphDocument.Read``1(Inno.Core.Graphs.GraphNodeRecord,System.String,``0,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) b6357c0969acb569f00a72da30610973e1761a5d11d7d0d831cf53e0365f1305 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b8636f71ba4978dc"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.definitionKey = "inno.shader.definition"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L16) | Identifies the graph metadata containing the source-free material and pass contract. | — | — |
| <a id="api-c38f2deb4e07325b"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.inputDefaultPrefix = "input-default."`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L86) | Prefixes stable port IDs storing explicit typed defaults for unconnected inputs. | — | — |
| <a id="api-56820e2c99c66a02"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.outputDefinitionId = "inno.shader.stage-output"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L74) | Identifies a stage output node; its incoming edges name the stage's GPU outputs. | — | — |
| <a id="api-d1c1bfae6c0fa0fe"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.settingsKey = "settings"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L82) | Identifies the structured settings stored on a stage output node. | — | — |
| <a id="api-2f9cc43987792423"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.stageKey = "stage"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L78) | Identifies a node property containing its owning stage output node identity. | — | — |
| <a id="api-905f72a9e58b98aa"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphDocument.targetKey = "inno.shader.target"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L20) | Identifies the optional domain target; absent means explicitly authored generic stages. | — | — |
| <a id="api-3def7599919e771d"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphDocument.Create(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L103) | Creates an empty graph with a source-free shader contract; incomplete graphs remain serializable.<br>返回：A detached graph ready for authoring.<br>`definition`：Material parameters, techniques and pass state.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context. | — | — |
| <a id="api-c7e65f69dfc89df1"></a>[`public static T Inno.Rendering.Shaders.ShaderGraphDocument.Decode<T>(Inno.Core.Graphs.GraphSerializedValue value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L187) | Decodes a graph value against the current owner generation.<br>返回：A detached current-generation value.<br>`value`：Neutral graph value.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context. | — | — |
| <a id="api-0c72d9390e853513"></a>[`public static Inno.Core.Graphs.GraphSerializedValue Inno.Rendering.Shaders.ShaderGraphDocument.Encode<T>(T value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L159) | Encodes a node or document value through the common native serialization channel.<br>返回：Neutral bytes suitable for graph persistence and history.<br>`value`：Detached authoring value.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context. | — | — |
| <a id="api-9f775a3499adf074"></a>[`public static T Inno.Rendering.Shaders.ShaderGraphDocument.Read<T>(Inno.Core.Graphs.GraphNodeRecord node, string key, T defaultValue, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L222) | Reads a named node value; absent values use the declared default, corrupt values never do.<br>返回：The decoded value or explicit absent-property default.<br>`node`：Neutral node record.<br>`key`：Stable property key.<br>`defaultValue`：Value used only when the property is absent.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context. | — | — |
| <a id="api-1ada87466990fa46"></a>[`public static Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Shaders.ShaderGraphDocument.ReadDefinition(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L130) | Reads the material and pass contract without evaluating or altering the graph.<br>返回：A detached contract; missing or corrupt metadata is an explicit error.<br>`graph`：Native shader graph.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context. | — | — |
| <a id="api-bc30fcbad0a2b214"></a>[`public static string Inno.Rendering.Shaders.ShaderGraphDocument.ReadTarget(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L37) | Reads the stable domain target assignment without resolving extension instances.<br>返回：The assigned stable ID, or an empty string for generic explicit stage authoring.<br>`graph`：Authored graph.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-7c12fd008235c2f1"></a>[`public static void Inno.Rendering.Shaders.ShaderGraphDocument.SetTarget(Inno.Core.Graphs.GraphDocument graph, string targetId, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L59) | Assigns a domain target by stable identity without retaining its current provider.<br>`graph`：Document being authored.<br>`targetId`：Stable target identity; empty selects explicit generic stage authoring.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-95e642b9eb575b9f"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphDocument`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L11) | Defines the native graph document protocol shared by shader import, templates and the editor. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphInputSettings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphInputSettings 1ab855558f628eb3445aa815da6816bdc629900e9212f7e9bda9b25f1b1520c1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphInputSettings.id 663ab3a0598f53bf8d60a9284bd3726f0095fb55c258795dbcd8511ca86fcd20 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphInputSettings.type c71674fda5d4898c247fb967d5e86d397013e3c4f5445ec4e9d9fe63ce8d2626 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphInputSettings.kind c932ff8bfae35025cf835c0a66b4b1384aa072fdfc4885f6cb6b29c04d7bd894 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphInputSettings.semantic cb32990cafa0365aea57764983618317998bca20d5b0029fe216321bc3659133 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphInputSettings.location b625a427510dc7cd23218e8930ecda57fc6c4a471517bc2a80d31fa38d3346b5 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphInputSettings.CreateBinding 7c326d0b9f28c10f0bd7a5d4a4c3362587055b2c716b3976eee01804071ace4f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphInputSettings.#ctor 6f6a5df4de50b8d9492fa8d7575bbae96205fb1238817d99406f818095460fa2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e6f8d6203949985b"></a>[`public Inno.Rendering.Shaders.ShaderGraphInputSettings.ShaderGraphInputSettings()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L285) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ae04e5459407d00c"></a>[`public Inno.Rendering.Shaders.ShaderIrStageInput Inno.Rendering.Shaders.ShaderGraphInputSettings.CreateBinding()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L314) | Validates the persisted descriptor and freezes its stage binding.<br>返回：An immutable backend-neutral input binding. | — | — |
| <a id="api-16127808bcb96137"></a>[`public string Inno.Rendering.Shaders.ShaderGraphInputSettings.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L290) | Gets or sets the stable logical binding identity. | — | — |
| <a id="api-ae3990185d8ab25d"></a>[`public Inno.Rendering.Shaders.ShaderIrInputKind Inno.Rendering.Shaders.ShaderGraphInputSettings.kind { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L298) | Gets or sets the stage input category. | — | — |
| <a id="api-804e0f214c064dc2"></a>[`public int Inno.Rendering.Shaders.ShaderGraphInputSettings.location { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L306) | Gets or sets the interface index. | — | — |
| <a id="api-2bdb36b6f511affe"></a>[`public string Inno.Rendering.Shaders.ShaderGraphInputSettings.semantic { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L302) | Gets or sets the target semantic, not a native expression. | — | — |
| <a id="api-818fe8cdd37424d9"></a>[`public Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphInputSettings.type { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L294) | Gets or sets the complete neutral type descriptor. | — | — |
| <a id="api-0d7ad70ea7477e75"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphInputSettings : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L285) | Stores a stage input node's logical interface without native names or GPU handles. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphLiteral`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphLiteral 0067123cc496c0b526a4053aa990004efc0ea49b756ccf5fcfcd54d1ad4c32ef -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLiteral.type 148434a5e4477e4797eb250b65f618c2236938c82cacd4a29a65bd26377631f6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLiteral.scalarBits 6bedcdbbf18700bdfeb00aff53c9f1c24aa3a89f499db45f268b83780d0337ab -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphLiteral.Zero(Inno.Rendering.Shaders.ShaderSourceType) 8d24a2f5f03939c7d34b5c1c1b43d797247802e38683e624a43dd31d3cc21415 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphLiteral.GetScalarTypes 949a655ba6f1f9ee66eee2332e479fb5517af28f6904aae0d0925a99e581d211 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphLiteral.Emit(Inno.Rendering.Shaders.ShaderIrBuilder,Inno.Rendering.Shaders.ShaderSourceType) 4f409e16929f5e7daa3d8f0aaf1a2adf2841cc78a697dcf64982eae4ef1f4bad -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphLiteral.#ctor cc9955ed595405dbe9f32f01eacb29930dc215992281821f48fb1c2754e0bee9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c5f2833626f138b9"></a>[`public Inno.Rendering.Shaders.ShaderGraphLiteral.ShaderGraphLiteral()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7a4c323f51989203"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderGraphLiteral.Emit(Inno.Rendering.Shaders.ShaderIrBuilder builder, Inno.Rendering.Shaders.ShaderSourceType expectedType)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L76) | Emits ordinary typed constants and aggregate construction into the common IR.<br>返回：A value owned by the supplied builder.<br>`builder`：Current region's instruction owner.<br>`expectedType`：Current port type; stale defaults never undergo implicit conversion. | — | `System.InvalidOperationException`：The stored type, component count or boolean bits are invalid. |
| <a id="api-8918f9ee99691b59"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderGraphLiteral.GetScalarTypes()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L47) | Gets the exact scalar type of each stored component for shared Inspector value controls.<br>返回：Detached scalar identities in the same order as the bit patterns. | — | — |
| <a id="api-a0defaf74ee1efc1"></a>[`public static Inno.Rendering.Shaders.ShaderGraphLiteral Inno.Rendering.Shaders.ShaderGraphLiteral.Zero(Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L33) | Creates an explicit zero value for a supported scalar, vector, matrix, structure or array.<br>返回：A detached zero literal with an exact type and component count.<br>`type`：Complete numeric or boolean value type; resources and effect tokens are not values. | — | `System.NotSupportedException`：The type is opaque or exceeds the bounded literal size. |
| <a id="api-85a917b117803461"></a>[`public uint[] Inno.Rendering.Shaders.ShaderGraphLiteral.scalarBits { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L19) | Gets or sets scalar bit patterns in declaration order; matrices use column-major order. | — | — |
| <a id="api-45ae1433903b5e11"></a>[`public Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphLiteral.type { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L15) | Gets or sets the complete value type, including named aggregates and fixed arrays. | — | — |
| <a id="api-41e63c2e17cf5fad"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphLiteral : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L10) | Stores an exact typed value for an unconnected node input, independently of source-language syntax. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphLoweringRequest`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphLoweringRequest e38369327d7ba4a58240cd7cc7d4057a7ce2f97f4635266b3f3fea76f9cd31ba -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphLoweringRequest.#ctor(Inno.Core.Graphs.GraphDocument,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Core.Graphs.GraphEndpoint},System.String,System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Graphs.GraphNodeId,Inno.Rendering.Shaders.ShaderSourceModuleAnalysis},System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Graphs.GraphNodeId,Inno.Rendering.Shaders.ShaderIrStageInput}) 9f48945fd1daf5cfe0856b475ca9c5087a39c32950a625ec4c7eb8f400261ab3 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringRequest.outputs 21f854ebcc29db3278a0d1b275eafb38f762b0942ca8f13aaec7a314d31c15b1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringRequest.implementationId 69ad3a9d43d7200db131d50d04433e4fb9266c2a03649c553a9c6a44cc3f0974 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringRequest.sourceModules 422b170d38edcf4713525d8abe458e4687ec452aba7292846d9fb496158e7126 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringRequest.stageInputs c44e283c0a80cb35a4e181381f1ca7d4bda6114ad3dc3e2bfba92a7ff1282b55 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fbd14f9724195a37"></a>[`public Inno.Rendering.Shaders.ShaderGraphLoweringRequest.ShaderGraphLoweringRequest(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphEndpoint> outputs, string implementationId, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis>? sourceModules = null, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderIrStageInput>? stageInputs = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L35) | Captures a region without retaining an asset object, source resolver, node provider or UI selection.<br>`graph`：Nodes and edges in this stage region; canvas positions are not compilation inputs.<br>`outputs`：Named region results mapped to stable output endpoints.<br>`implementationId`：Exact source implementation key selected by the target.<br>`sourceModules`：Already analyzed source modules keyed by their source node identity.<br>`stageInputs`：Target-assigned stage inputs keyed by their input node identity. | — | — |
| <a id="api-3a3a939418ad678d"></a>[`public string Inno.Rendering.Shaders.ShaderGraphLoweringRequest.implementationId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L61) | Gets the exact implementation key selected by the target. | — | — |
| <a id="api-abadf32a20e79b90"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphEndpoint> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.outputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L57) | Gets named region outputs by stable graph endpoint. | — | — |
| <a id="api-2da22bc65b6e8901"></a>[`public System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.sourceModules { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L65) | Gets frozen source modules, including unavailable/failed modules for accurate diagnostics. | — | — |
| <a id="api-3cfa077c87ccb9ea"></a>[`public System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderIrStageInput> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.stageInputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L69) | Gets target-assigned input descriptors; they contain no native expressions. | — | — |
| <a id="api-8fb67e72c3a83faa"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphLoweringRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L13) | Freezes one target-selected graph region and its externally resolved, neutral compilation inputs. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphLoweringResult`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphLoweringResult 58ad5ed32cfaa6230343a12add168555f4931fccf41b5e533b93ef10dd95a974 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringResult.block 9616fe369189979e4131d25bc78e99d124c5352f43f9b979182372b98412546a -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringResult.diagnostics 2fb654f1a021f0f79e0f7ceca3b856b08917c42e064cb60856ad0af4af21087e -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphLoweringResult.succeeded 803914676509342047989ea733624556e111baa7bd1fc0ec59d7f5f370817477 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6f6376570c1712f4"></a>[`public Inno.Rendering.Shaders.ShaderIrBlock? Inno.Rendering.Shaders.ShaderGraphLoweringResult.block { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L113) | Gets the immutable typed region, or null on failure. | — | — |
| <a id="api-58a668e8977a1bcd"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphDiagnostic> Inno.Rendering.Shaders.ShaderGraphLoweringResult.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L117) | Gets located graph diagnostics, including missing definitions and stale source ports. | — | — |
| <a id="api-0a201b0a270ac542"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphLoweringResult.succeeded { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L121) | Gets whether lowering produced a complete region without errors; native compilation is a separate gate. | — | — |
| <a id="api-93592dfd755c17a9"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphLoweringResult`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L101) | Contains a detached typed region only after all graph, port and node lowering validation succeeds. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeEffect`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeEffect a0c25ff2c8724ce662076f68a010fadb79b93801aa7ea30bd448106e4654cfda -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodeEffect.Pure da511ab431dcb2b22d9eb76a53c09cf17072ee92a50196437eab50ac29082985 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodeEffect.SideEffect b120bf63ec2a8ff05fd9385a3d7771e7a4fe0c5f8f3b74013f5493053509d250 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-16c380978eed3ed6"></a>[`Inno.Rendering.Shaders.ShaderGraphNodeEffect.Pure = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L33) | The function only computes returned values and therefore requires at least one output. | — | — |
| <a id="api-14c8928521fd5ee0"></a>[`Inno.Rendering.Shaders.ShaderGraphNodeEffect.SideEffect = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L37) | The function may contain ordered GPU effects and can intentionally expose no returned values. | — | — |
| <a id="api-13e7a6e108726352"></a>[`public enum Inno.Rendering.Shaders.ShaderGraphNodeEffect : int`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L28) | Declares whether a graph-authored function is pure or intentionally emits ordered effects. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler 3f1e5541cb3857e217f40ca92bb0ed1adc7d1c59f8c800ba93b1fbff4ca6aef5 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.definitionId c172e5de99affe89341e1498900e98f1251c4dad69cc54984a6ce8f084ba248d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 31b05006e7e3b216962335ddb5e38496d585f2ac7b9acf157c8b5908e60695ff -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) aed509ff69426974ad51c7ae350025f9fb0a6825d5719086d294bbbd5b564a5a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.#ctor 0f6e4375154b8e261a10c76f58d0371d155b53b20d6ab6f25da87cc7656d27cc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6fdc7931d70b904b"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.ShaderGraphNodeInputsCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L634) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-75da4e1f4172f1c9"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L649) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-753cc43fb9ada43b"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L661) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-1d53adaab112ec3e"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L639) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-2b361927e7898ee8"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L634) | Describes the multi-port external inputs while editing a graph-authored node asset. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeInputSettings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeInputSettings 8f70cf9c7cc0c5c121911897c16f86387603c95025f81e22a90b8d8ebab5033f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInputSettings.ports 2ad2e9a87182cf631857f4f28fe5f1469465720f72d9168baad6a47fa3e8937d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInputSettings.#ctor ab81a116999bb01d8a48271ad1d3bc8904fe5314c905c783726a06d88e39624b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5b2d886f58def2da"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeInputSettings.ShaderGraphNodeInputSettings()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L96) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-13989e8e27d56283"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInputSettings.ports { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L101) | Gets or sets values supplied by callers and exposed as outputs inside the node graph. | — | — |
| <a id="api-59a48c73fb3668bd"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeInputSettings : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L96) | Stores values supplied by callers to a graph-authored node. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeInterface`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeInterface a3efd89a9575ec6175c40d7764350646d0a990821237dc04093e8381f750d846 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.displayName f1f60731278423e2cbb8930939b3fecf06e6640644f468dc7358cfe8177a3d75 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.createPath b6e7a0cc02b0aad09709cf822f2748b45c9fc3c186c6554244f2ab1b274f7352 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.createOrder 9e88b0a1d9224ca1e9cd7964cdfdd7d01c626421a824429f1fdf39fd0085b22d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.kind 4d49f31d0d590d0e6afb42146d2c364b191cf1f9fd80e529a0ec6d3c1efc1b4d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.effect 6a30708e46fd217476439c0c69944661242aa8706aa603a3097a59990fdaac9a -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.role 530345ef0339a8db1300e16aa6352df9aead5967586fe117fbef2ab4821452cc -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.inputs 7dc1dd00a8fc0c23158682ecf18904984ddefb1b41ef62d326820fb72d35f04e -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeInterface.outputs fcd75e30ff4747c84be8874e0d64f28e7377c2c6607ae35eb1ca91a4c1ffccd3 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInterface.GetPorts 1ef7c92ebaf71b0543ad6e6bd1307457e155b397e808a4b43f809fc034a80f9b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeInterface.#ctor 6c6c30fc75505a13b436191796adfdc8a4441c3504cf4a255c7c660ee8a7d1c6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0128a6f008888728"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeInterface.ShaderGraphNodeInterface()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L118) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5dd202981f0dd8a8"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeInterface.GetPorts()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L159) | Gets detached typed ports in deterministic input-then-output order.<br>返回：A provider-free port snapshot suitable for Editor presentation and validation. | — | — |
| <a id="api-abd0eb1861dfd094"></a>[`public int Inno.Rendering.Shaders.ShaderGraphNodeInterface.createOrder { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L131) | Gets or sets the deterministic order within the creation catalog. | — | — |
| <a id="api-19ec6e4751d44252"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeInterface.createPath { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L127) | Gets or sets the slash-separated creation catalog beneath Graph Nodes. | — | — |
| <a id="api-1bedff046d36b722"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeInterface.displayName { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L123) | Gets or sets the node title displayed to authors. | — | — |
| <a id="api-d16341e1ef0cf45d"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeEffect Inno.Rendering.Shaders.ShaderGraphNodeInterface.effect { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L139) | Gets or sets whether an inline function is pure or intentionally emits ordered effects. | — | — |
| <a id="api-fd51560574950fa1"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInterface.inputs { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L147) | Gets or sets externally supplied inputs. | — | — |
| <a id="api-682d02278237a109"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeKind Inno.Rendering.Shaders.ShaderGraphNodeInterface.kind { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L135) | Gets or sets whether the graph is inline computation or a Target-owned output boundary. | — | — |
| <a id="api-e304ea71f214a1f5"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInterface.outputs { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L151) | Gets or sets externally visible results. | — | — |
| <a id="api-868d6dddad414d7e"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeInterface.role { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L143) | Gets or sets the Target-owned role for a domain output. | — | — |
| <a id="api-86a838e4a1ce6925"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeInterface : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L118) | Freezes the complete public interface resolved from one graph-authored node asset. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeKind`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeKind 7796620825246e58e80f7f79c23ff7030e023ce7be2730d1defdf2eca6665c37 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodeKind.Function e5072c3d5f70da4141db43c9dc6b6a7444f16344c0115d29ed90decf9acf4542 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodeKind.DomainOutput 94aac29b608df06fbf30f00c85547d9fa5fa01840f9c754fa7daecc02d659a54 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3c106dad421e914c"></a>[`Inno.Rendering.Shaders.ShaderGraphNodeKind.DomainOutput = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L22) | Leaves the node as a typed domain boundary for the selected Shader Target. | — | — |
| <a id="api-dc684144f9cf554d"></a>[`Inno.Rendering.Shaders.ShaderGraphNodeKind.Function = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L18) | Inlines the node graph into the caller before Target expansion and typed lowering. | — | — |
| <a id="api-ea1a71b89eb72279"></a>[`public enum Inno.Rendering.Shaders.ShaderGraphNodeKind : int`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L13) | Chooses whether a graph-authored node is inlined or consumed by a domain Target. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler 2c2abbc78956fea7cb841ad1f577f7e3a484c4326877678e3aed18bd62a8be36 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.definitionId 7aa92bfec54199d6d06aa8deb7f2ef12f1ff69a8b5f8e59761e2af0c30467444 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 71e62c4ae994589b07c0e118075be8a96ee427826fbdcc15f2cf4dd70a8604fd -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) c1c80549e552018b10f0ab58e80401e7885016c3a172027cba704272720645b1 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.#ctor 687c8de217852baacfea344b85e58f2fb53c31c3fd8614483c8ebc13678dad88 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f11a0bdac75e815"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.ShaderGraphNodeOutputsCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L668) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-b4c188b3ad4867b1"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L683) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-cb8de97078c29568"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L695) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-99677e361e0970ff"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L673) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-c6ebc293b3245308"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L668) | Describes the multi-port returned values while editing a graph-authored function node asset. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings 217c60a7de2293120a4fd4a22b58327f7d2d55153ba86162e0709c8971175de6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings.ports 94ad3e548145559008e97eaec39267b348dfd426a9aecd0af3dd6cc7b598daf0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings.#ctor 8ccd09d0972f8f624c676ff7f79e36b04a41aaff939e4c9309cdba718d2da498 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b673e20a30c4021c"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings.ShaderGraphNodeOutputSettings()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L107) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7666d6ea4ca6d88b"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings.ports { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L112) | Gets or sets values collected inside the node graph and exposed as outputs to callers. | — | — |
| <a id="api-25c88d63b7a55dbe"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L107) | Stores values returned by a graph-authored function node. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodePortDefinition`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodePortDefinition b8a10e345dac11db5f6ccce43013b9d7562524efed76dd65b48960b38eb598b7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.id 5e3fe9154c49f87ba7470bffdfff3a155afd72a5de22a7593c292eb9ca0b2c54 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.type 0257e3637744e5e418255f3d28a24fd8a0f302f7a1262662e85685b0597fbef5 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.required ad8da5c5c181d32c6019ad0be944326f815fb08f49d5bf8e8bd2ecfbc07755fe -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.#ctor de8d4041e1194a5542faaf9babfb61c89d564e00ff63edb76cc17d3639210711 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e620d339d0b6674f"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.ShaderGraphNodePortDefinition()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L74) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ffcd70686807958f"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L79) | Gets or sets the stable node-local port identity. | — | — |
| <a id="api-c6869fc56b54e21b"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.required { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L87) | Gets or sets whether callers must connect the input instead of using an explicit/default zero. | — | — |
| <a id="api-0826d89f2c26d000"></a>[`public Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.type { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L83) | Gets or sets the complete backend-neutral value type. | — | — |
| <a id="api-64587fed60c95114"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodePortDefinition : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L74) | Declares one stable, typed port on a graph-authored node interface. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodes`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodes 1a2953c80d46da632d5606bfba6e6feba67249f50e9130f8daaa14faf74b22da -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodes.inputDefinitionId df30aab0f3869860b551b433b88e53d98bb26ed3f3eb3eaabf8429ccc3f79aa5 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodes.outputDefinitionId d29bc362a85edfbc21aa112c8b73d6658749cb5696feb4f540fead448b3d2eba -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodes.callDefinitionId 38d2d3dc76aa736a3a084955b2a98072be5a9a670585b8caec93d734d55769f0 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodes.interfaceKey 6a7000a540dec40e61cb833f25423d6bcb338860b14a37476c30cf6fe8803884 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphNodes.settingsKey 365e03272201a40d155978b7f4af48efa721db2e517cafb1349463952cd2ff1b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.IsNodeGraph(Inno.Core.Graphs.GraphDocument) 3aac94e8d749711e0659490cbd177fbf1f7dc99e643b2218477a59c2c5c8d5b1 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.WriteSettings(Inno.Core.Graphs.GraphDocument,Inno.Rendering.Shaders.ShaderGraphNodeSettings,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 88c963313a3e3fe9b386b1fcadee6430f358209bf6de5d5e3b9cc7f9a6011f3f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.ReadSettings(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 6c29d150af0ca163fa4d803e3b793cdf4e10a20dcd03835d524bcc289fbf621b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.ReadInterface(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 93f45bf9499a40f7537d3f4236ed4dd9026b48614b68357ed66bb22b3ac47229 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.ReadCallInterface(Inno.Core.Graphs.GraphNodeRecord,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) e3c2b61e236ab624ee45723cf67ee674fc3d040b818bcfaa825bab873ea9dda9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodes.Expand(Inno.Core.Graphs.GraphDocument,System.Func{System.Guid,System.String,Inno.Core.Graphs.GraphDocument},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) f23a56f59c8563e13fd5bcc968493bae9f2b249683ea6f244627f189bebfd06d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ae0a1dea8fe3039e"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphNodes.callDefinitionId = "inno.shader.graph-node"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L181) | Identifies a reference to another Shader graph used as a node. | — | — |
| <a id="api-91619e395d833984"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphNodes.inputDefinitionId = "inno.shader.node-inputs"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L173) | Identifies the single multi-port node-input interface record. | — | — |
| <a id="api-0664d6fb107e2b3e"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphNodes.interfaceKey = "interface"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L185) | Identifies the serialized interface snapshot retained by a graph-node reference. | — | — |
| <a id="api-d787cb856ef84e92"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphNodes.outputDefinitionId = "inno.shader.node-outputs"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L177) | Identifies the optional multi-port node-output interface record. | — | — |
| <a id="api-66ab37bd5c6c96fb"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphNodes.settingsKey = "inno.shader.node-settings"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L189) | Identifies graph-level reusable-node metadata, independent of either interface direction. | — | — |
| <a id="api-b6f691bc67e52fcd"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphNodes.Expand(Inno.Core.Graphs.GraphDocument graph, System.Func<System.Guid, string, Inno.Core.Graphs.GraphDocument> resolve, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L359) | Expands every inline graph-node reference and refreshes domain-output interfaces.<br>返回：A detached graph containing no inline graph-node references.<br>`graph`：Authored parent graph.<br>`resolve`：Resolves a referenced Shader graph by persistent identity and diagnostic path.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-0328fa0f127c5642"></a>[`public static bool Inno.Rendering.Shaders.ShaderGraphNodes.IsNodeGraph(Inno.Core.Graphs.GraphDocument graph)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L200) | Determines whether a Shader graph declares a reusable node interface.<br>返回：true when graph-level node settings are present.<br>`graph`：Graph to inspect. | — | — |
| <a id="api-f8a044e955465981"></a>[`public static Inno.Rendering.Shaders.ShaderGraphNodeInterface Inno.Rendering.Shaders.ShaderGraphNodes.ReadCallInterface(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L330) | Reads the current interface snapshot stored on a graph-node reference.<br>返回：The detached stored interface.<br>`node`：Graph-node reference.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-fe237d304c2db917"></a>[`public static Inno.Rendering.Shaders.ShaderGraphNodeInterface Inno.Rendering.Shaders.ShaderGraphNodes.ReadInterface(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L273) | Reads and validates the public node interface declared by a Shader graph.<br>返回：The detached graph-node interface.<br>`graph`：Detached graph asset source.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-03788cb8cdfacb87"></a>[`public static Inno.Rendering.Shaders.ShaderGraphNodeSettings Inno.Rendering.Shaders.ShaderGraphNodes.ReadSettings(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L247) | Reads required graph-level reusable-node metadata.<br>返回：Detached node metadata.<br>`graph`：Graph declaring the reusable node.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-fbfba8888b88115c"></a>[`public static void Inno.Rendering.Shaders.ShaderGraphNodes.WriteSettings(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.Shaders.ShaderGraphNodeSettings settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L221) | Writes graph-level reusable-node metadata without coupling it to an input or output record.<br>`graph`：Graph whose node identity is being assigned.<br>`settings`：Detached node metadata.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-d8a06463463f6945"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphNodes`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L168) | Reads and expands reusable node graphs without retaining assets or provider instances. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphNodeSettings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphNodeSettings 38410172c9825ce3c04fbeb4dc49a51fed15c43813aebfcf998de572012e5071 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.displayName 062b9d87913ae5e21b31a33a9d35ff078de703c3d5a11f7453c800d436e41870 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.createPath 1cbd0acb06d4a833e238596f7f402585a3e4a8cca7f08757cf2eeb086184bac0 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.createOrder f6c238e03d71bd06487d7bf5256ad66d77c56771182966a7bacc976f9e7132ca -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.kind 1ad8b50ee36e5dc6f015cbaa17ea22ed1f8697e0caa13c5b6afe0f2703334df6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.effect af180cab175281838abbdfdeeaa3250ce07837404fca7c9a7e8185f76940bb89 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphNodeSettings.role b8e5c6f1d118a4273839555c858f055daaec3c5c5a5813914c156ad11533716b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphNodeSettings.#ctor dd8dea6dd7ec397ef7953bd1f812d78d42af372e70b56102ba2649cb9ff3a2df -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dd9484b826e8e3e3"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeSettings.ShaderGraphNodeSettings()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L43) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ab8b902b9114db26"></a>[`public int Inno.Rendering.Shaders.ShaderGraphNodeSettings.createOrder { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L56) | Gets or sets the deterministic order within the creation catalog. | — | — |
| <a id="api-7c11474d9b58e411"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeSettings.createPath { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L52) | Gets or sets the slash-separated creation catalog beneath Graph Nodes. | — | — |
| <a id="api-f784398825e6aa23"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeSettings.displayName { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L48) | Gets or sets the node title displayed to authors. | — | — |
| <a id="api-b17bba023c8a9f78"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeEffect Inno.Rendering.Shaders.ShaderGraphNodeSettings.effect { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L64) | Gets or sets the observable computation behavior of an inline function. | — | — |
| <a id="api-2d9010b0fd52d067"></a>[`public Inno.Rendering.Shaders.ShaderGraphNodeKind Inno.Rendering.Shaders.ShaderGraphNodeSettings.kind { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L60) | Gets or sets how a reference to this graph participates in compilation. | — | — |
| <a id="api-40f3081f4f1049d1"></a>[`public string Inno.Rendering.Shaders.ShaderGraphNodeSettings.role { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L68) | Gets or sets the domain role consumed by a Target; empty for ordinary inline functions. | — | — |
| <a id="api-44dbca6a6d1fe606"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphNodeSettings : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L43) | Stores graph-level identity and catalog metadata for a reusable Shader node. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphOutput`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphOutput ba96366a0fa8eed7a1b264c1fa50758a81ae5bf504650834eac418e04e8cc01b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphOutput.id 8ed846e03b2947d7cab9a436d6214bf89c72928d85f5e4891634e1f587b3022d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphOutput.kind d229f865c4bc677424a991fb57b7568f79a9ccb54aa65cf965a42d9353e53292 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphOutput.semantic c76ad81afc343643c10e2b2058031becbddaa5abb7b7bbdecc59a3d68b4b6853 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphOutput.location 4c04602ae2cfedc2da4cc44cecbb569f7945aed7082d5dfacbfab08cebdeadd9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ff6da8764fe808b9"></a>[`public string Inno.Rendering.Shaders.ShaderGraphOutput.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L267) | Gets or sets the stable input port identity on the stage output node. | — | — |
| <a id="api-57f1deee557994c0"></a>[`public Inno.Rendering.Shaders.ShaderIrOutputKind Inno.Rendering.Shaders.ShaderGraphOutput.kind { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L271) | Gets or sets the GPU destination category. | — | — |
| <a id="api-dc7be1f66bfcc97d"></a>[`public int Inno.Rendering.Shaders.ShaderGraphOutput.location { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L279) | Gets or sets the varying or attachment index. | — | — |
| <a id="api-31df57a5d2410652"></a>[`public string Inno.Rendering.Shaders.ShaderGraphOutput.semantic { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L275) | Gets or sets the varying semantic; empty for fixed position, color or depth outputs. | — | — |
| <a id="api-d3fd4c96ff589270"></a>[`public struct Inno.Rendering.Shaders.ShaderGraphOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L262) | Stores one stable output port's destination, independent of adapter source syntax. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphPass`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphPass bd0c7e062e3d41af71f62520c62c5b2aa349e7460f097d9e262156da4ce1cef4 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphPass.#ctor(System.String,System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderIrStage}) b6081ef14717773b0ac42917631ceab877e5995dbec4be8eec5b7e21b6376dd7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPass.name 058c6fcafe00109b7dc1d9ec92db041b57ac02642a74203b06b789f82470258c -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPass.stages 2038710c3341544244827f0c972923a0946a7de953f7b0eea91a25798b355f3c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-427a4d7a608e728d"></a>[`public Inno.Rendering.Shaders.ShaderGraphPass.ShaderGraphPass(string name, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStage> stages)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L22) | Captures a pass's ordered typed stages.<br>`name`：Exact pass identity in the program contract.<br>`stages`：Complete raster or compute stage set. | — | — |
| <a id="api-5b89b538336683d3"></a>[`public string Inno.Rendering.Shaders.ShaderGraphPass.name { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L34) | Gets the stable pass identity. | — | — |
| <a id="api-b8e928e26a71b908"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStage> Inno.Rendering.Shaders.ShaderGraphPass.stages { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L38) | Gets immutable graph-lowered stages. | — | — |
| <a id="api-31b93a06cb4d678d"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphPass`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L11) | Freezes the graph-lowered stages of one material-selectable GPU pass. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphPassProgram`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphPassProgram 242d723b5c0110770eb637fc73c90150db0ecc5e286f063e603ded9c432436d1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPassProgram.pass 49901d75d0e33f5e3d013dc804bb2cba4d69d140829cbf8ee98ec2db389db0e7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPassProgram.stages 9296ea106883724f4e719af243b6679a4fe7d75a4462656f7cdd52dd133d3aba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-176a5ec9da115e55"></a>[`public string Inno.Rendering.Shaders.ShaderGraphPassProgram.pass { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L18) | Gets or sets the exact pass identity in the runtime shader definition. | — | — |
| <a id="api-f067147171804e68"></a>[`public string[] Inno.Rendering.Shaders.ShaderGraphPassProgram.stages { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L22) | Gets or sets stable stage output node identities; computation remains owned by those nodes. | — | — |
| <a id="api-8b4c6e868a9099e7"></a>[`public struct Inno.Rendering.Shaders.ShaderGraphPassProgram`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L13) | References shared stage output identities from a material-selectable pass. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphPasteResult`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphPasteResult a225ffab7ebdfc35df29f36f1dd04f9a90d0400594f0e30a8d74d1338fdc11e7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPasteResult.document 0f39b7bbedbebd7ef12773c2e2fe4f747ba08ebe3829c1c945cece3ddc27e0e7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphPasteResult.insertedNodes 7b9f1373bcb5e24d0c4550d6850c7cf6e00e7a17d8e4cfb940337c419d62e111 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-54f3517587663073"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPasteResult.document { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L182) | Gets the complete detached candidate; the caller owns its subsequent edits. | — | — |
| <a id="api-c42d2afc25c7fb98"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Rendering.Shaders.ShaderGraphPasteResult.insertedNodes { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L186) | Gets new identities suitable for selecting the pasted nodes. | — | — |
| <a id="api-72e015917e4ad436"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphPasteResult`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L170) | Contains a detached paste candidate and its newly allocated node identities. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphProgramCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphProgramCompiler e22d5489ed65d52112ba0d20a00a6415487269479503c617cd5087f13b7eb447 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphProgramCompiler.#ctor(Inno.Rendering.Shaders.ShaderNodeCompilerRegistry) 06169420b86e24ecdb1f6d3ae67747207ba24d01d49fab74e3fead555760de55 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphProgramCompiler.Lower(Inno.Core.Graphs.GraphDocument,System.String,System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Graphs.GraphNodeId,Inno.Rendering.Shaders.ShaderSourceModuleAnalysis},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,System.Threading.CancellationToken) 4404217b13ece9336618702bcab8ec2b64ac1801d99137689be3c3ad4c1bb0b5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f0b0b545c06462e7"></a>[`public Inno.Rendering.Shaders.ShaderGraphProgramCompiler.ShaderGraphProgramCompiler(Inno.Rendering.Shaders.ShaderNodeCompilerRegistry nodes)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L28) | Uses the shared node compiler generation rather than retaining individual providers.<br>`nodes`：The owner-scoped node registry. | — | — |
| <a id="api-80635f956093f083"></a>[`public Inno.Rendering.Shaders.ShaderGraphProgramResult Inno.Rendering.Shaders.ShaderGraphProgramCompiler.Lower(Inno.Core.Graphs.GraphDocument document, string implementationId, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis> sources, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L55) | Validates the complete pass/stage graph without deleting invalid or unavailable records.<br>返回：All typed passes, or diagnostics without a partial publishable program.<br>`document`：Authored graph, copied before validation.<br>`implementationId`：Exact adapter implementation identity selected by the caller.<br>`sources`：Frozen function modules keyed by source node identity.<br>`serialization`：The owner converter registry.<br>`context`：The complete owner reference context.<br>`cancellationToken`：Cancellation between stages and node invocations. | — | — |
| <a id="api-710a030fce5cf3ed"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphProgramCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L18) | Partitions explicit GPU-stage regions and lowers them through registered node compilers. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphProgramResult`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphProgramResult dd908c39a15b36641fa0f364640a2867671d70ee61a3de1a585adccb611d8a2d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphProgramResult.passes f7a0aac0ea364e9d79f2cc1bf39fe9f01aa5f9d0ad5e00641000a1ceb9d655e3 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphProgramResult.diagnostics c92897cac4241d37c2baf8fd023d24fb32a0d0b5a00dbbbc9f058299fb059948 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphProgramResult.succeeded 1abdb46e1b81f2f14f03d7131bd6d458d84bd60cdb3772518adcbd729177997f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3cf753381a4cb954"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphDiagnostic> Inno.Rendering.Shaders.ShaderGraphProgramResult.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L60) | Gets stable graph/node diagnostics without provider references. | — | — |
| <a id="api-a538c46738c941d5"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphPass> Inno.Rendering.Shaders.ShaderGraphProgramResult.passes { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L56) | Gets the complete stage set; empty if any stage failed. | — | — |
| <a id="api-e82a7e806efb6a99"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphProgramResult.succeeded { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L64) | Gets whether every declared pass was lowered without errors. | — | — |
| <a id="api-8c5026d097d9ab52"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphProgramResult`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L44) | Contains the result of lowering an entire shader graph, before target-native compilation. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphPrograms`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphPrograms 1a716be71cb497c6a35cc4dd5b5a0958ba15e0e4828b7fc9edd5a0eeed85f4e9 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderGraphPrograms.bindingsKey dfba735f3638c569ba570b73abe4b8bf569313ab77fb51328f84c24968c4f572 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphPrograms.Read(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 84b432ad38b368d1d4b3b0a522054804ff23f101e753fffa5f8bc67c9525a330 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphPrograms.Bind(Inno.Core.Graphs.GraphDocument,System.String,System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId},Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 6ff6ebece154f96328053a0868b86ab41ac1f1e43b2ee48940397e0a07f76668 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphPrograms.RemovePass(Inno.Core.Graphs.GraphDocument,System.String,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) a82990b981a69ff4a5b2703bd09d8f4c19f9fe8c1188b7f4713294c931cd2f2a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c5eab437b9af8a2a"></a>[`public const string Inno.Rendering.Shaders.ShaderGraphPrograms.bindingsKey = "inno.shader.programs"`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L33) | Identifies native metadata holding pass references to shared stage programs. | — | — |
| <a id="api-f7e1df7ced47f15c"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPrograms.Bind(Inno.Core.Graphs.GraphDocument graph, string pass, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> stages, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L80) | Assigns existing shared stages to one pass in an atomic detached candidate.<br>返回：A candidate suitable for one shared History transaction.<br>`graph`：Source document, never modified.<br>`pass`：Existing pass identity.<br>`stages`：Existing stage output node identities; an empty set preserves an incomplete pass.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-10a14f18bd7e2112"></a>[`public static Inno.Rendering.Shaders.ShaderGraphPassProgram[] Inno.Rendering.Shaders.ShaderGraphPrograms.Read(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L50) | Reads detached pass-to-stage references, including unresolved authored identities.<br>返回：Detached native references; corrupt or missing program metadata is an error.<br>`graph`：Shader authoring document.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-03f2610ad2637cb0"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPrograms.RemovePass(Inno.Core.Graphs.GraphDocument graph, string pass, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L127) | Removes a pass and only computations whose final referencing pass was removed.<br>返回：A candidate preserving programs shared by other passes and their parameter declarations.<br>`graph`：Source document, never modified.<br>`pass`：Existing pass identity.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-ff0b8d46385927e3"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphPrograms`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L28) | Edits pass-to-program references without copying computation or changing runtime render states. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphStageSettings`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphStageSettings 8507d3987e20d26c8c2855a599f88e54814c360bb29bb5209f02dd2d23ec7950 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphStageSettings.stage 88b1de2d2972cd8c095c0da360c47741bc3685580b85d8755b711d6a288851e1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphStageSettings.outputs 301bf4bfa5dca8a85d2707f8b3450f98c60fe37bc93edeca8294addba1e8ff65 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsX 4605358706c62fd45099951404c48178e8634994a9dffe87f999858f5f2b7206 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsY 92f7eb0da3a7cdcecaeaa0c0f0f0d31ca0469313c022b62b2e799165b3875061 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsZ 5e5be620f87c08045090a6a61691884b70f4a627345cbb891940b8a5cac001af -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphStageSettings.#ctor 2ff7adc51b65bffc3cd28746fcd4221e5311ff97dc123ca14db507713cb1f932 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b8c0d9e263bd02d9"></a>[`public Inno.Rendering.Shaders.ShaderGraphStageSettings.ShaderGraphStageSettings()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L235) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-96d199bdb4f787e1"></a>[`public Inno.Rendering.Shaders.ShaderGraphOutput[] Inno.Rendering.Shaders.ShaderGraphStageSettings.outputs { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L244) | Gets or sets output ports and their GPU destinations. | — | — |
| <a id="api-9d338cb7fdcffb92"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.Shaders.ShaderGraphStageSettings.stage { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L240) | Gets or sets the programmable stage. | — | — |
| <a id="api-f8b70c33d67039e2"></a>[`public int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsX { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L248) | Gets or sets compute workgroup width. | — | — |
| <a id="api-bd9002e61694703a"></a>[`public int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsY { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L252) | Gets or sets compute workgroup height. | — | — |
| <a id="api-f7385e3098dc3606"></a>[`public int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsZ { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L256) | Gets or sets compute workgroup depth. | — | — |
| <a id="api-9c19dcc85f72b7c2"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphStageSettings : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L235) | Stores one stage's explicit output interface and compute dimensions inside its output node. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphTemplate`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphTemplate 01c388ea7d62dd7b43ec0828a3a0a50008f1ff5aad2503aa3f8f79991e0039fb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplate.Create(Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 923da8cd6c009aea0f8c9662c31179a701bc9c5c897b1c4e39862fae33f01398 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplate.#ctor 7a65db4de87f918a1dedbae630e28de17024e771f35bebe7f1c1878474a03b79 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-332de975e4c7dd00"></a>[`protected Inno.Rendering.Shaders.ShaderGraphTemplate.ShaderGraphTemplate()`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L48) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-aab4f0cff7bdc126"></a>[`public abstract Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplate.Create(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L62) | Creates a fresh detached graph with its target and parameter declarations.<br>返回：A graph ready for native serialization and ordinary shader import.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | — |
| <a id="api-283ac79540187948"></a>[`public abstract class Inno.Rendering.Shaders.ShaderGraphTemplate`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L48) | Contributes an ordinary shader graph to the shared asset creation workflow. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphTemplateAttribute`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphTemplateAttribute dadc0146941316c8ea124f6f6fceab019bdc45d2d172f9820e23cd2656098338 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.#ctor(System.String,System.String) f945aba1da8c9f582a2672264545912c3b667632f8717f8fb69320db9bf4b05c -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.id aa7ab151441d65f50c142a9794b16c07885a0643e19e03b0f00b58974d52caed -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.displayName 671323fb7374bd24d8def1e9aa86c202068becf8ecd142544d1b0b7564902b18 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9e256b39d7ac4365"></a>[`public Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.ShaderGraphTemplateAttribute(string id, string displayName)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L22) | Creates Shader graph template discovery metadata.<br>`id`：Stable template identity used by creation commands.<br>`displayName`：User-facing creation menu label. | — | — |
| <a id="api-bc8e432dfe5bfc32"></a>[`public string Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.displayName { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L42) | Gets the user-facing creation menu label. | — | — |
| <a id="api-01b8b63c6c553245"></a>[`public string Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L37) | Gets the stable template identity used by creation commands. | — | — |
| <a id="api-3e627b19f9998376"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphTemplateAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L11) | Declares immutable creation metadata for a Shader graph template. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphTemplateInfo`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphTemplateInfo 2ad8c359aabbb84d411b8cacc534b1299f0ddd5848f3066890d5f56af397f82c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.#ctor(System.String,System.String) 44fb344ca63c27bd233763fac4153f72916aa4b888cc2cebcdd3f857244612d6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.id 06272fe17264962f10da05d7275c45ff7def54b91536d09163368fa6347e1981 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.displayName 9f19744ee7af8911cc228388ceb4f57f4a2bebb0b686f73bfdaab7c10bcc65bd -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.ToString 1e8e591812b72796142de7521cd904dcf3be9a923bea47eb5031bc4f84729556 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.op_Inequality(Inno.Rendering.Shaders.ShaderGraphTemplateInfo,Inno.Rendering.Shaders.ShaderGraphTemplateInfo) ba12e2eea333bf864fc9d35016782d9a170f157285b9eae0ca70b9832b4b4afc -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.op_Equality(Inno.Rendering.Shaders.ShaderGraphTemplateInfo,Inno.Rendering.Shaders.ShaderGraphTemplateInfo) c51cf25cf5c8f343118e386606056ea72651557bf0ccf7efc108bcde85758620 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.GetHashCode fa35af836c7dcdbc3c44e6eb0a6cb740bb331c95d0b6513b6bc0846e48b962a4 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Equals(System.Object) 89b18468e0dd6e63b957d66db99e200c2d41f68cc2ce18c07617f0c95524d91a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Equals(Inno.Rendering.Shaders.ShaderGraphTemplateInfo) bd432a0ab71bc24424c61c5ff70b4da37771622843e00f68445ddb646ef8cbea -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Deconstruct(System.String@,System.String@) 6c124882d403827fd748be028595d5c805a613515f2a32a81e8f3b03f51e61de -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e9fc80b7da58c1b1"></a>[`public Inno.Rendering.Shaders.ShaderGraphTemplateInfo.ShaderGraphTemplateInfo(string id, string displayName)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Describes a creation menu item without retaining a plugin instance.<br>`id`：Stable template identity.<br>`displayName`：User-facing label. | — | — |
| <a id="api-cd18068a3e1d1ef2"></a>[`public void Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Deconstruct(out string id, out string displayName)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-00b478a94970fcc4"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Equals(Inno.Rendering.Shaders.ShaderGraphTemplateInfo? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-55d2867dc8b73e08"></a>[`public override bool Inno.Rendering.Shaders.ShaderGraphTemplateInfo.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-5c72be94a8fea39f"></a>[`public override int Inno.Rendering.Shaders.ShaderGraphTemplateInfo.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-f1bdc6aea9cbde9d"></a>[`public static bool Inno.Rendering.Shaders.ShaderGraphTemplateInfo.operator ==(Inno.Rendering.Shaders.ShaderGraphTemplateInfo? left, Inno.Rendering.Shaders.ShaderGraphTemplateInfo? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4bfe24237a5dbda7"></a>[`public static bool Inno.Rendering.Shaders.ShaderGraphTemplateInfo.operator !=(Inno.Rendering.Shaders.ShaderGraphTemplateInfo? left, Inno.Rendering.Shaders.ShaderGraphTemplateInfo? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3f687d4c379df4c7"></a>[`public override string Inno.Rendering.Shaders.ShaderGraphTemplateInfo.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-331e07fae6525a88"></a>[`public string Inno.Rendering.Shaders.ShaderGraphTemplateInfo.displayName { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L22) | User-facing label. | — | — |
| <a id="api-88fda148f300bd28"></a>[`public string Inno.Rendering.Shaders.ShaderGraphTemplateInfo.id { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L21) | Stable template identity. | — | — |
| <a id="api-9d53d85c3d361b24"></a>[`public sealed record Inno.Rendering.Shaders.ShaderGraphTemplateInfo : System.IEquatable<Inno.Rendering.Shaders.ShaderGraphTemplateInfo>`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Describes a creation menu item without retaining a plugin instance.<br>`id`：Stable template identity.<br>`displayName`：User-facing label. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphTemplateRegistry`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphTemplateRegistry 32602361998e69d9bcda5952c8befd207c03e8ef2bd1f7f9d03c62967d7bf12f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) 17cba7722a3c77eedd6803b4e2acad6b65245b3c0fd09e9193f9a9e7c97113f1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.templates baea24f0b7f6cdf7daff200b2e85fc4c8c268663b365d6b01f4818dddb4b6024 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Create(System.String,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 488c9ed26ba408f6ec482c58d17bfa5f72e9a251ed36d0458869e9bc51389d3a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Dispose c7040dc8097efebc1db881082578e8d8c679aab69f5f25c1571d5fdbb3ac6c37 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f3f739ce03e2db9e"></a>[`public Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.ShaderGraphTemplateRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L39) | Registers a template owner with the shared type-generation catalog.<br>`types`：Catalog which must outlive this owner. | — | — |
| <a id="api-eb5202eab2648c4d"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Create(string id, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L76) | Invokes the selected template under one generation lease.<br>返回：A detached graph; no provider instance crosses the invocation boundary.<br>`id`：Stable template identity from the creation menu.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context. | — | `System.InvalidOperationException`：The selected template is unavailable. |
| <a id="api-5f6d34e514af1abb"></a>[`public void Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Dispose()`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L91) | Retires providers through the shared generation lifecycle. | — | — |
| <a id="api-e75769383e21f94c"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphTemplateInfo> Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.templates { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L48) | Gets detached menu descriptions for the current generation. | — | — |
| <a id="api-b67715d5a563745c"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphTemplateRegistry : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L28) | Owns generation-safe template discovery and invocation for editor asset creation. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphTemplates`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphTemplates d23a3b9de8fba18a66dc5aba126eaaf619f360a62f0cc95dd813190f0421ee91 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphTemplates.CreateRaster(Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) d21653e8a4acebc2a504272d5eac6379caef98191547d85be85565bb960a0440 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b9e90f846ab5c161"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplates.CreateRaster(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphTemplates.cs#L26) | Creates a raster graph accepting clip-space XY positions and an exposed RGBA color.<br>返回：A valid two-stage document; pipelines may replace its vertex transform or provide their own templates.<br>`serialization`：Owner native converter registry.<br>`context`：Complete owner asset/reference context for material defaults. | — | — |
| <a id="api-2fc1f27a986e3029"></a>[`public static class Inno.Rendering.Shaders.ShaderGraphTemplates`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphTemplates.cs#L12) | Provides small backend-neutral starting documents composed exclusively of ordinary graph nodes. | — | — |

### `Inno.Rendering.Shaders.ShaderGraphType`

<!-- api: T:Inno.Rendering.Shaders.ShaderGraphType 10114ba5bbc1bff28ce6b4ffab52d7dff5dd046ed8803d64560453f1663316f5 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.id 79e3704c712b7de9f8e19a596393485b5172de167fb3a46d4da75003dd80a0e2 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.element 8058c4b5beeea9919f27ee72d5e2a6bcfc07e2da96b42ecb8accb3fb64bd91e6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.length 95fcc82b8536b9e2c9275a4b5dd3771412b93b09c92754bdb25518632b5c681e -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.fieldNames 262a99668f364897e4649bd89b0ddbb72c4b9b9f072c71f2d9dfbc76b3810aa9 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.fieldTypes e17c1410d3a8b7c4b0101684c31dba14f4e95fc39a8480c2dc992856f54f9aab -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.isStorage 5845703b5ccf7d9e69a49357780c3f739dd40aa61ac56145e61e60e8a8207504 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.isImage 34654cd346e588bf1e913527d5e8b4d11fe172021c480d9d6826ffd595557262 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.storageElement 7f12557b972b7aaa8e01203c2893b4bda1a7a867c5603414c7a6ed10f9b374d1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.access 3adc9df8b8689fbf00d6a2e2b1ccaed55482e107da884ef0068423a0a96ef56f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.format bb762ae20fa5efa677ea405f8cb6b52c3681e79fcce136a4c22a6da5f6deb7be -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.dimension 1af64b71427eb42ffba0c5e9e62c1d16370b12468af58a3b0168e2d3048269bc -->
<!-- api: P:Inno.Rendering.Shaders.ShaderGraphType.isArray e9a80aba2828c36393c768d0dd025883a30d962eea0d23aae7b038d91b340126 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphType.Capture(Inno.Rendering.Shaders.ShaderSourceType) 8b0dd8c5e8b2f0700c97b937136eb52f16da14b92fdd0e1cd74869acdadaa078 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphType.CreateType 2b9d50eece783f1b15e45ea88fce5f3892fb78565e3fa58f28b5effffe416598 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderGraphType.#ctor 5cd3fd6bceeee9cd7c18cca07b5f07ac7e702564d3e0c48e7aaf657e1586116b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bb4ace7de5ce5a38"></a>[`public Inno.Rendering.Shaders.ShaderGraphType.ShaderGraphType()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-1aa2a37b9b5f5938"></a>[`public static Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphType.Capture(Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L70) | Captures a full immutable source type as reload-safe authoring data.<br>返回：A detached descriptor whose reconstructed type has the same complete interface.<br>`type`：Atomic, aggregate or storage type to capture. | — | — |
| <a id="api-6ba3336cbaeb56dc"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderGraphType.CreateType()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L97) | Validates and freezes the complete descriptor.<br>返回：An immutable type; contradictory or incomplete descriptors fail explicitly. | — | — |
| <a id="api-30fef87da91a9029"></a>[`public Inno.Rendering.RenderStorageAccess Inno.Rendering.Shaders.ShaderGraphType.access { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L47) | Gets or sets storage access. | — | — |
| <a id="api-01d685e53c42ce91"></a>[`public Inno.Rendering.RenderTextureDimension Inno.Rendering.Shaders.ShaderGraphType.dimension { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L55) | Gets or sets the image spatial dimension. | — | — |
| <a id="api-94e346663ccaf39a"></a>[`public Inno.Rendering.Shaders.ShaderGraphType? Inno.Rendering.Shaders.ShaderGraphType.element { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L19) | Gets or sets the fixed array element type, or null for a non-array. | — | — |
| <a id="api-e8f02e174ffbc76f"></a>[`public string[] Inno.Rendering.Shaders.ShaderGraphType.fieldNames { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L27) | Gets or sets ordered structure member names. | — | — |
| <a id="api-3641a1dbfc8bcccc"></a>[`public Inno.Rendering.Shaders.ShaderGraphType[] Inno.Rendering.Shaders.ShaderGraphType.fieldTypes { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L31) | Gets or sets ordered structure member types. | — | — |
| <a id="api-1fb0a06440f6388e"></a>[`public Inno.Rendering.RenderTextureFormat Inno.Rendering.Shaders.ShaderGraphType.format { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L51) | Gets or sets the image texel format. | — | — |
| <a id="api-fc4dc8be05e107d3"></a>[`public string Inno.Rendering.Shaders.ShaderGraphType.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L15) | Gets or sets the atomic or nominal structure type identity. | — | — |
| <a id="api-e1e2d5da2cf7e3a2"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphType.isArray { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L59) | Gets or sets whether an image has array layers. | — | — |
| <a id="api-0bf32106ff5b393b"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphType.isImage { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L39) | Gets or sets whether storage is an image instead of a buffer. | — | — |
| <a id="api-c0b3ea8ab25c6a36"></a>[`public bool Inno.Rendering.Shaders.ShaderGraphType.isStorage { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L35) | Gets or sets whether the descriptor represents a storage binding. | — | — |
| <a id="api-f4ba44f2f2ade19e"></a>[`public int Inno.Rendering.Shaders.ShaderGraphType.length { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L23) | Gets or sets the fixed array length. | — | — |
| <a id="api-3c081627030544b0"></a>[`public Inno.Rendering.Shaders.ShaderGraphType? Inno.Rendering.Shaders.ShaderGraphType.storageElement { get; set; }`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L43) | Gets or sets the complete buffer element type. | — | — |
| <a id="api-cabe82d4f7d39cf3"></a>[`public sealed class Inno.Rendering.Shaders.ShaderGraphType : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L10) | Persists a complete shader type as neutral native-serializable data, without extension instances. | — | — |

### `Inno.Rendering.Shaders.ShaderIrBlock`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrBlock b833582742c220081999d644ba06f3075d3f589679f5cd5b9acae50cf5ddcfea -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrBlock.instructions e7fce129a192060f4b7003918facbec04f599f2c076eca434e57defb5391f390 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrBlock.outputs 45681c667af8678f47bad53adc226de5c3bb3e3cf567b6dbb1e7af8829dd5bc9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c2ed19acbaed5763"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrInstruction> Inno.Rendering.Shaders.ShaderIrBlock.instructions { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L214) | Gets instructions in evaluation order, including source calls whose outputs are unused. | — | — |
| <a id="api-db13ef35f9762af6"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBlock.outputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L218) | Gets named values returned to the enclosing stage or region. | — | — |
| <a id="api-120bf605f991d0a9"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrBlock`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L201) | Contains immutable ordered instructions and nested structured regions without retaining builder callbacks. | — | — |

### `Inno.Rendering.Shaders.ShaderIrBuilder`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrBuilder 54f32aba84ca0548851621cee4349b839685308034a2a59e3dded6b6335a2a1f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Branch(Inno.Rendering.Shaders.ShaderIrValue,System.Func{Inno.Rendering.Shaders.ShaderIrBuilder,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue}},System.Func{Inno.Rendering.Shaders.ShaderIrBuilder,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue}}) 3a096808eab560fbfbb13c1d5234b23f6eaa43de965df7e0b9da51094fa45357 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Loop(Inno.Rendering.Shaders.ShaderIrValue,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue},System.Func{Inno.Rendering.Shaders.ShaderIrBuilder,Inno.Rendering.Shaders.ShaderIrValue,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue},System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue}}) bfbed6b7f195be14f654aa01a7b2f202713dfa94596b9dcb5784d76b88b905bf -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.#ctor c87ff1608534e9055a4673fdd7f88de13c4aa6c26ec8693a74ae87d38dd24dba -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Input(System.String,Inno.Rendering.Shaders.ShaderSourceType) a092f7ccdb57be90a36c7cafdcf263cac4f9754116fd05054db3d0edf22e9004 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Constant(System.Single) 621212347a9a05c5b393e9a3319365659fed84a457f67f5a96ed31d10b6fbb2d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Constant(System.Int32) dd769709564278ca8dd1cbc3a47ddb4a92c4a345eda86a437429ad54d2cb14aa -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Constant(System.UInt32) 52cb976e2c57ce689bbed5974bb5b960d12b32303e00d5f2048372b2622d5b8c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Constant(System.Boolean) 60eecd0a11b7ef202ce8ef93814b700aa59200168abda89c8c75939e33cb5afe -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Binary(Inno.Rendering.Shaders.ShaderIrOperation,Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) f0fc27475f3d9835e234512c69a680ad62c246b7f9174b92d6b6a50935381407 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Select(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) 8dd299f0595a86e09e89e5cda5646dc5911c30fb7a1380b8b45b4d76afd3867f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Construct(Inno.Rendering.Shaders.ShaderSourceType,Inno.Rendering.Shaders.ShaderIrValue[]) 57315bcbf6c8ca0befa27a2c5523dcb47a72ebff57f9f025dbddf31630ae404e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Extract(Inno.Rendering.Shaders.ShaderIrValue,System.Int32) 91949e027012c7f9e15e65f42498218be6f4d84192a393187ae511a1f37d1ab7 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Call(Inno.Rendering.Shaders.ShaderSourceModuleAnalysis,System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue}) 3ad74d11485f67845befc3f09b109331408f9a40f82d01ec5d1bf93d95b713c8 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Build(System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Rendering.Shaders.ShaderIrValue}) bc7a77cdd9dd78f92ead0ed9676127147affa38ac387e1c593d99ad4908d0759 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Sample(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) b113f3ac79262eccf707ef22184d4d16775f54a66fc682874dcec5103e4b2a0f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.SampleLevel(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) f2f9d7aa9e7260df2d8a15967d265078574324f8ff1287391efa7dec12820ab8 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.LoadStorage(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) 30f27945707a3a4d88c78b06905575760b758f597fc5082021ff70602a9312d7 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.StoreStorage(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) 658b6a9e7d9e30912eee85a5c37479d2403f579f7e732b02f40ca0235914bd29 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.AtomicAddStorage(Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue,Inno.Rendering.Shaders.ShaderIrValue) d02951c210068d4d0f736f92cb98134e3a0b434257534aef962e7faeccbb573b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrBuilder.Discard(Inno.Rendering.Shaders.ShaderIrValue) 95c1e152888cb608a0ca246cf39c62b70506103b2cbb556156a0e5179a1e2ac0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-347e4e9b4919a659"></a>[`public Inno.Rendering.Shaders.ShaderIrBuilder.ShaderIrBuilder()`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L26) | Creates an independent typed region builder and value identity scope. | — | — |
| <a id="api-677da49d6f83433c"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.AtomicAddStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue index, Inno.Rendering.Shaders.ShaderIrValue value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L140) | Atomically adds to a scalar integer buffer element and returns its previous value.<br>返回：The original scalar value observed by the atomic operation.<br>`resource`：A read-write int or uint storage buffer.<br>`index`：Unsigned element index.<br>`value`：Addition operand matching the buffer element type. | — | `System.ArgumentException`：The resource is not an appropriate read-write integer buffer. |
| <a id="api-8bb6840e447f091b"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Binary(Inno.Rendering.Shaders.ShaderIrOperation operation, Inno.Rendering.Shaders.ShaderIrValue left, Inno.Rendering.Shaders.ShaderIrValue right)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L140) | Applies an explicitly typed binary arithmetic or scalar comparison operation.<br>返回：The result; comparisons produce a scalar Boolean.<br>`operation`：Add, Subtract, Multiply, Divide, Minimum, Maximum, Equal or LessThan.<br>`left`：Left operand from this builder.<br>`right`：Right operand of the same complete type. | — | `System.ArgumentException`：Operands belong to another builder, differ in type, or the operation is invalid for them. |
| <a id="api-0ff6f3aa24663f29"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Branch(Inno.Rendering.Shaders.ShaderIrValue condition, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> whenTrue, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> whenFalse)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L34) | Builds a real conditional: only the selected region executes, including its memory and source-call effects.<br>返回：Values merged into the enclosing scope; empty output maps represent effect-only branches.<br>`condition`：Scalar Boolean visible in this region.<br>`whenTrue`：Transient callback building the true region and its named outputs.<br>`whenFalse`：Transient callback building the false region with exactly matching output names/types. | — | `System.ArgumentException`：The condition or branch output contracts are incompatible.<br>`System.InvalidOperationException`：A callback mutates the enclosing scope or declares a nested stage input. |
| <a id="api-2361a178eb9bbc60"></a>[`public Inno.Rendering.Shaders.ShaderIrBlock Inno.Rendering.Shaders.ShaderIrBuilder.Build(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> outputs)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L364) | Freezes the current region without pruning unused calls or retaining this mutable builder.<br>返回：An immutable block unaffected by later builder changes.<br>`outputs`：Named values returned by the enclosing region. | — | `System.ArgumentException`：An output name is empty or a value belongs to another builder. |
| <a id="api-2450fd7507fb85bd"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Call(Inno.Rendering.Shaders.ShaderSourceModuleAnalysis module, string implementationId, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> inputs)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L315) | Calls a validated source implementation using semantic parameter names, preserving all call side effects.<br>返回：Named results: return, followed by output.<name> for out/inout parameters.<br>`module`：A module whose selected implementations and variants share the same interface.<br>`implementationId`：The exact implementation/configuration selected by the target.<br>`inputs`：Exactly the input/inout parameters by public name; output-only parameters are not supplied. | — | `System.ArgumentException`：The module failed, implementation is absent, or input names/types do not match. |
| <a id="api-0ad8c5324a09f61d"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(bool value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L120) | Produces a scalar Boolean constant.<br>返回：A bool-typed intermediate value.<br>`value`：Scalar Boolean value. | — | — |
| <a id="api-7c1d70d412bb2899"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(int value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L98) | Produces an exact signed 32-bit integer constant.<br>返回：An int-typed intermediate value.<br>`value`：Scalar integer value. | — | — |
| <a id="api-c4121fc74baacc9e"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(float value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L82) | Produces a finite, exact 32-bit floating-point constant.<br>返回：A float-typed intermediate value.<br>`value`：Finite scalar value, including signed zero. | — | `System.ArgumentOutOfRangeException`：The value is NaN or infinite. |
| <a id="api-431a4b97c5b4777c"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(uint value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L109) | Produces an exact unsigned 32-bit integer constant.<br>返回：A uint-typed intermediate value.<br>`value`：Scalar unsigned integer value. | — | — |
| <a id="api-124b406c9689a936"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Construct(Inno.Rendering.Shaders.ShaderSourceType type, params Inno.Rendering.Shaders.ShaderIrValue[] members)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L217) | Constructs a complete vector, column-major matrix, structure or fixed array from typed members.<br>返回：A value of the requested type.<br>`type`：The complete aggregate type.<br>`members`：Scalar vector/matrix components, declaration-ordered structure fields, or array elements. | — | `System.ArgumentException`：The type is not an aggregate or members have incorrect count or types. |
| <a id="api-9b5980d374196bbb"></a>[`public void Inno.Rendering.Shaders.ShaderIrBuilder.Discard(Inno.Rendering.Shaders.ShaderIrValue condition)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L165) | Discards a fragment when the Boolean condition is true; other stages reject this instruction.<br>`condition`：Scalar Boolean from this block. | — | `System.ArgumentException`：The condition is not a Boolean or belongs to another builder. |
| <a id="api-9967e84aa69324e6"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Extract(Inno.Rendering.Shaders.ShaderIrValue value, int memberIndex)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L262) | Extracts one static component, structure member or array element without source-language member syntax.<br>返回：The precisely typed selected member.<br>`value`：Aggregate value from this builder.<br>`memberIndex`：Zero-based member index; a matrix index selects a column vector. | — | `System.ArgumentException`：The input is not an aggregate.<br>`System.ArgumentOutOfRangeException`：The member index is outside the aggregate layout. |
| <a id="api-479382e7741e0ba3"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Input(string name, Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L49) | Reads a named input from the enclosing typed function or stage interface.<br>返回：The shared input value for this name and type.<br>`name`：Stable interface identity; it is not emitted as native source verbatim.<br>`type`：Complete non-void input type. | — | `System.ArgumentException`：The name is empty, type is void, or this input was already declared with another type. |
| <a id="api-46a1d6229ce98d44"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.LoadStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue coordinate)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L83) | Loads a storage value at this exact point in the block's memory-effect sequence.<br>返回：The declared buffer element or float4 image value.<br>`resource`：A readable typed storage input.<br>`coordinate`：uint buffer index; int2 image coordinate, or int3 for an image array/volume. | — | `System.ArgumentException`：The resource is write-only or the coordinate is incompatible. |
| <a id="api-ea80416ce2c31f40"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Loop(Inno.Rendering.Shaders.ShaderIrValue iterations, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> initialState, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, Inno.Rendering.Shaders.ShaderIrValue, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> body)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L82) | Builds a counted loop; each iteration receives the preceding iteration's complete carried state.<br>返回：Final carried values in the enclosing scope; an empty state map permits effect-only loops.<br>`iterations`：Unsigned iteration count, evaluated once before entering the loop.<br>`initialState`：Named values used before the first iteration and returned unchanged for zero iterations.<br>`body`：Transient callback receiving its builder, uint iteration index and typed carried values. | — | `System.ArgumentException`：The count is not uint or the body's output state changes names or types.<br>`System.InvalidOperationException`：The body mutates the enclosing builder. |
| <a id="api-3d172abc38efb1ac"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Sample(Inno.Rendering.Shaders.ShaderIrValue texture, Inno.Rendering.Shaders.ShaderIrValue coordinate)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L26) | Samples a texture using implicit derivatives; the enclosing stage must be Fragment.<br>返回：The sampled float4 value.<br>`texture`：A sampled 2D, 2D-array, 3D or cube texture.<br>`coordinate`：float2 for 2D, or float3 for an array, volume or cube. | — | `System.ArgumentException`：Operands have incompatible types or owners. |
| <a id="api-a43e56619e638e5f"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.SampleLevel(Inno.Rendering.Shaders.ShaderIrValue texture, Inno.Rendering.Shaders.ShaderIrValue coordinate, Inno.Rendering.Shaders.ShaderIrValue level)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L54) | Samples a texture at an explicit floating-point mip level without implicit derivatives.<br>返回：The sampled float4 value.<br>`texture`：A sampled 2D, 2D-array, 3D or cube texture.<br>`coordinate`：float2 for 2D, or float3 for an array, volume or cube.<br>`level`：Scalar floating-point mip level. | — | `System.ArgumentException`：Operands have incompatible types or owners. |
| <a id="api-f754d69d904712f4"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Select(Inno.Rendering.Shaders.ShaderIrValue condition, Inno.Rendering.Shaders.ShaderIrValue whenTrue, Inno.Rendering.Shaders.ShaderIrValue whenFalse)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L185) | Selects between equal typed values without conditionally executing their producers.<br>返回：A value with the selected operands' complete type.<br>`condition`：Scalar Boolean condition from this builder.<br>`whenTrue`：Value selected when true.<br>`whenFalse`：Value selected when false. | — | `System.ArgumentException`：The condition is not bool or selected value types differ. |
| <a id="api-a4cd8f6099165794"></a>[`public void Inno.Rendering.Shaders.ShaderIrBuilder.StoreStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue coordinate, Inno.Rendering.Shaders.ShaderIrValue value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L110) | Stores a storage value without pruning unused writes or reordering surrounding memory operations.<br>`resource`：A writable typed storage input.<br>`coordinate`：uint buffer index; int2 image coordinate, or int3 for an image array/volume.<br>`value`：Exactly the declared element or image value type. | — | `System.ArgumentException`：The resource is read-only or operands have incompatible types. |
| <a id="api-6dd41edfcf5f18fb"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrBuilder`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L11) | Builds typed shader IR instructions for graph compilation. | — | — |

### `Inno.Rendering.Shaders.ShaderIrInputKind`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrInputKind a6cdf6c2e767e526497e661c20f0504b8140393f10fdf4e4cf27364a5e76db29 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.VertexAttribute 471162ee238d2b491c8c501e1eaf34418dd924f8abde70894d0653ce15c98c14 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.Varying d3e649947818ae252e04130a518a348006543a247165d1443b2d431cdb07d193 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.Uniform 262f0b0c814a693ea5b9ed1bb0b20d20a168fb9a96ce353cc1f03eaff3c058fa -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.SampledTexture 10b110ae225dfd8f40c999097b1553371644acef51b0ee383faa87583878abfb -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.Storage 4fefa24fcf3b96228cd0c2011942335c185a8df93e10541b3920ed0579274099 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrInputKind.Builtin afcfd789f2abbb21b1f059c01520625e7df7d5ffb810f17bf4f0bf5a5a426ce6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-06e6918fa341e55c"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.Builtin = 5`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L36) | A standard GPU stage input identified by a target semantic, not native source text. | — | — |
| <a id="api-47d4286ef517aa98"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.SampledTexture = 3`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L28) | A sampled texture binding whose slot is assigned by target resource layout. | — | — |
| <a id="api-c2176d77db0bc22e"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.Storage = 4`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L32) | A typed storage buffer or image whose access, shape and layout are explicit. | — | — |
| <a id="api-f87f3a96d158beb1"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.Uniform = 2`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L24) | A material, frame or draw uniform identified by a stable binding identity. | — | — |
| <a id="api-b3af3ca8e5dd57df"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.Varying = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L20) | An interpolated value written by the preceding raster stage. | — | — |
| <a id="api-83df41d556b0e550"></a>[`Inno.Rendering.Shaders.ShaderIrInputKind.VertexAttribute = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L16) | A vertex stream attribute, including instance-rate streams. | — | — |
| <a id="api-abd58932ffc0b8a0"></a>[`public enum Inno.Rendering.Shaders.ShaderIrInputKind : int`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L11) | Identifies the GPU interface through which a stage receives a value. | — | — |

### `Inno.Rendering.Shaders.ShaderIrInstruction`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrInstruction 884df9a495c3189667301c2edf296cef071084f54ebac9e915dc60687bee443d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.operation caed2228daced8c9ca70caa4fece8571f1027914e4a04ea4c5dbefe128e9d39d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.inputs 31ee07ae981048d8635f72f6d24917d71a882f9c689934cf73485ae8c7e27eb2 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.outputs 74a7da0a5ee07896984b63d29121da9d4b4a182518f3d2c12ba302a9577ab544 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.inputName 6b135841451da79f934f636de4b13259c08c5382dd40649aa72018370a834e12 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.constantBits 93b6c081e556777b6eb94f510ab86e3d7a82020f1b7436026779993e9d7181c3 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.memberIndex 72ab69439f5a525f4e7c9c183beda1e27f9ae2e94e4fdf51b3cd4002a16e8703 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.source c5b49d928b2c1303a1e79b7e093dc974baeaddc624dbc79c10e8c8500e63ed4c -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.regions c17bb20ceee0ca44329a629b74ca201b1d8f1dafb2a5e5e0a33b89e812f3d007 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrInstruction.hasSideEffects c9710ee3c05e8177b7781fb16a96f458bdd051bacce34c1824fc787ea8bda38f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0fc628961e5c020b"></a>[`public ulong Inno.Rendering.Shaders.ShaderIrInstruction.constantBits { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L177) | Gets exact scalar bits for a Constant instruction; the result type determines interpretation. | — | — |
| <a id="api-703ea85fe35396b0"></a>[`public bool Inno.Rendering.Shaders.ShaderIrInstruction.hasSideEffects { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L193) | Gets whether removing or reordering this instruction may change observable behavior. | — | — |
| <a id="api-1a1f0baea0c372ce"></a>[`public string? Inno.Rendering.Shaders.ShaderIrInstruction.inputName { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L173) | Gets the stable interface or region-parameter identity, only for Input or RegionInput. | — | — |
| <a id="api-58056de069f40533"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrInstruction.inputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L165) | Gets immutable operands in operation or function-declaration order. | — | — |
| <a id="api-52a9b83596c3c974"></a>[`public int Inno.Rendering.Shaders.ShaderIrInstruction.memberIndex { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L181) | Gets the zero-based component/member/element ordinal for an Extract instruction. | — | — |
| <a id="api-b21783f57cce0a8f"></a>[`public Inno.Rendering.Shaders.ShaderIrOperation Inno.Rendering.Shaders.ShaderIrInstruction.operation { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L161) | Gets the semantic operation independent of any source language. | — | — |
| <a id="api-84b60344a6b4a571"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrInstruction.outputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L169) | Gets produced values; a call orders its return value first, then out/inout parameters. | — | — |
| <a id="api-73b75fb72987d8ce"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrBlock> Inno.Rendering.Shaders.ShaderIrInstruction.regions { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L189) | Gets immutable nested regions: true/false for Branch, or one body for Loop. | — | — |
| <a id="api-36a9260c2257589a"></a>[`public Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis? Inno.Rendering.Shaders.ShaderIrInstruction.source { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L185) | Gets a frozen, analyzed function implementation only for a SourceCall instruction. | — | — |
| <a id="api-13e34aa59738d05e"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrInstruction`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L136) | Stores one immutable typed instruction without generated stage source or backend expressions. | — | — |

### `Inno.Rendering.Shaders.ShaderIrOperation`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrOperation 2602d80d32631c8fa7d0c831ebd45270c4232321db38bf1d81e775baafa3626c -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Input 660ab7b150298cf5de5603de9863952cda5020f57a19aeb41338cae44a87cdea -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Constant 1c442c17508cc96487c7b445f6dbc859608f1a7105baa325f994795116702c3a -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Construct 8cdece5d335df978e5c65be6a7bc1dfb6aec9567ed1154622f4ce6d25f9b8f31 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Extract da6529d070cafbb9114969700089d48c440248cf78f76f5bcdd351396da9b5b3 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Add 334a1b79acc20d85f406c6e8bd996814af1a12f38d5327f7503424cfffd7c273 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Subtract 2e66a201ad3891cd997f95553d67c917c9ae7ca8dab80b7bc31ad848c096a839 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Multiply aba5c7429eff8e8aa162edde67ece6fcacf8d5b1d167138ab8e6778db4318554 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Divide f6579c998d58254d3c6437f1b033fc7cf7e3fa6e27f9b390fd8bb05205fe6c89 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Minimum 712ced7b1f39a80d1f097b5a77dafb31d4f1073c4194e25468b3095b2e255e7e -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Maximum 846ec0c4c7c6f96c4b0c00debbe8baca8014be0b3321dc3749bbde607b0e892b -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Equal da15f1fa25e89e6c593533447dbbf81ea99a07784ec2b2b4d866f6aa69f7dd10 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.LessThan 34f02149ddd906432c8a04b78a578f83666d0f22de3e15a38bdc7e4dc192ac40 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Select 7e118a4cb7f901022aa0d5f3ec10b1eabd99b504f897853cf3a31ff0e0e84595 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.SourceCall e1706a171eb8cbe532587360f6e55a0a8547fdfd2cd933af08646bbc2ed7230a -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.TextureSample 1923933df40b03c8b2d64de37617fd814f78d195261db41ce33f08d4b0bf3ef3 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.TextureSampleLevel d1a7e0caa11c602a03259977b499da4b6217019fb645a41a236dbc06330b9f39 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.StorageLoad 51c01418b6c746b87b20da8daa0510ad42611fd6da22c1aa6b4aa7d01e0ab92d -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.StorageStore 14a099e50b1d8f38ffb4f2f5cca5c8956c7af88b4e3e5055b4f055f05d9b8ebf -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.StorageAtomicAdd f3a918687ff66d249964a42e9a6b64c9451c85c2dce5d81d6b7826da3412e260 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Discard 9c07f1dbbb423c372f274d5dce34312153ae60d0132ba1b4dc6254f2f80394b3 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.RegionInput a70cdf240cb60d45ab95c9a359c3c7b1a92a426bc79e6f8b5f8449f8ef1e91c9 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Branch cd3fbfe386cc715f76e517565cf0f6ca21299a90f15a75499a8fb928ea25f46c -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOperation.Loop d70443ad22dde93bf2808a846973ab2c59f7ef4bd9f5e805a3338c605ba73e8c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-693ed5963efadb82"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Add = 4`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L58) | Adds two equal numeric types component by component. | — | — |
| <a id="api-68f944c4cb05730a"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Branch = 21`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L126) | Executes exactly one nested region and merges its named output values. | — | — |
| <a id="api-9f042b8a7a171de3"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Constant = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L46) | Produces an exact scalar constant stored as bits, not source-language text. | — | — |
| <a id="api-3189bf20434b1f0b"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Construct = 2`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L50) | Constructs an aggregate from typed scalar or member values. | — | — |
| <a id="api-991d04954aed8d37"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Discard = 19`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L118) | Discards the current fragment when the scalar condition is true. | — | — |
| <a id="api-194f46b14e9e339a"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Divide = 7`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L70) | Divides two equal numeric types component by component. | — | — |
| <a id="api-d6f81016f0a843c6"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Equal = 10`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L82) | Tests equality of two scalar values and produces a scalar Boolean. | — | — |
| <a id="api-ef6f412c79d2a7e8"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Extract = 3`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L54) | Extracts one statically selected vector component, array element or structure field. | — | — |
| <a id="api-72987cc351d41d43"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Input = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L42) | Reads a named input supplied by the enclosing function or stage. | — | — |
| <a id="api-f4985e7c09d33aaf"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.LessThan = 11`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L86) | Compares two equal numeric scalar types and produces a scalar Boolean. | — | — |
| <a id="api-c95799f6b0200f34"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Loop = 22`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L130) | Executes a counted loop with explicit typed carried values and ordered body effects. | — | — |
| <a id="api-5f3256d3880df8c9"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Maximum = 9`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L78) | Selects the component-wise maximum of two equal numeric types. | — | — |
| <a id="api-9dcc913c39b12cf7"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Minimum = 8`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L74) | Selects the component-wise minimum of two equal numeric types. | — | — |
| <a id="api-48dd518f66720392"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Multiply = 6`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L66) | Multiplies two equal numeric types component by component; this is not matrix multiplication. | — | — |
| <a id="api-722840ba25397f7c"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.RegionInput = 20`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L122) | Provides a typed iteration index or loop-carried value inside a nested region. | — | — |
| <a id="api-7cff16d2988da385"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Select = 12`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L90) | Selects between equal types using a scalar Boolean; it does not introduce control flow. | — | — |
| <a id="api-e91424bfafc19b0d"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.SourceCall = 13`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L94) | Calls a parsed source function with named inputs and ordered multiple outputs. | — | — |
| <a id="api-930e54b3f8a485df"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.StorageAtomicAdd = 18`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L114) | Atomically adds to an integer buffer element and returns the original value. | — | — |
| <a id="api-e068efc6c9a52389"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.StorageLoad = 16`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L106) | Loads one typed buffer element or formatted image texel in memory-effect order. | — | — |
| <a id="api-a38aa25c8d56404d"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.StorageStore = 17`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L110) | Stores one typed buffer element or formatted image texel in memory-effect order. | — | — |
| <a id="api-27c8ad8256f5b37e"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.Subtract = 5`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L62) | Subtracts two equal numeric types component by component. | — | — |
| <a id="api-786e0bfcb282bd21"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.TextureSample = 14`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L98) | Samples a floating-point texture using implicit fragment derivatives. | — | — |
| <a id="api-6d132011dd494c58"></a>[`Inno.Rendering.Shaders.ShaderIrOperation.TextureSampleLevel = 15`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L102) | Samples a floating-point texture at an explicit mip level. | — | — |
| <a id="api-62cf819c139f48f7"></a>[`public enum Inno.Rendering.Shaders.ShaderIrOperation : int`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L37) | Defines backend-neutral value operations; graph node IDs are not instruction opcodes. | — | — |

### `Inno.Rendering.Shaders.ShaderIrOutputKind`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrOutputKind 81e0aa3efe4663fc02ba8047a61015e9469c3b7f73a7c839c025239ac3b3e4f0 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOutputKind.ClipPosition 0c462de867811fa0a2a621474f5cf8cdede15bb2f3401a60f47e5564441f4a3a -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOutputKind.Varying 0855b909e0860c3c532dc70ff6aab34ea4b890b850bf817300a360f7ece51859 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOutputKind.Color a9fc5ddbfa610aa8f0dd3f2d3a6f378030b4b52351d2ea3b7d9aed3adb3d5fb0 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderIrOutputKind.Depth 5b89db8c292e4be9991be7a9e97aab7dbd1e403aa29c935c3413cd7cda61e1ce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-90079215c27f34c6"></a>[`Inno.Rendering.Shaders.ShaderIrOutputKind.ClipPosition = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L47) | The vertex position in homogeneous clip space. | — | — |
| <a id="api-006c9356c4fef898"></a>[`Inno.Rendering.Shaders.ShaderIrOutputKind.Color = 2`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L55) | A fragment color attachment at its explicit location. | — | — |
| <a id="api-2d2c5ec8dc42755d"></a>[`Inno.Rendering.Shaders.ShaderIrOutputKind.Depth = 3`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L59) | The fragment depth value. | — | — |
| <a id="api-6214a0b5482f1960"></a>[`Inno.Rendering.Shaders.ShaderIrOutputKind.Varying = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L51) | A value interpolated for the next raster stage. | — | — |
| <a id="api-a4dcb578881bb96f"></a>[`public enum Inno.Rendering.Shaders.ShaderIrOutputKind : int`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L42) | Identifies where a typed stage output is written. | — | — |

### `Inno.Rendering.Shaders.ShaderIrStage`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrStage 88e4285730c89e3abf71801ada66ad2612da532cb32736956508b7151da2925d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrStage.#ctor(Inno.Rendering.ShaderStage,Inno.Rendering.Shaders.ShaderIrBlock,System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderIrStageInput},System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderIrStageOutput},System.Int32,System.Int32,System.Int32) 9c7694f851cc81460b3f140723d8da2944580529751cea758285bfc6cdc191ae -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.stage ce90adaca54d20a06f3634afaf35c709cb03ab9fd65685b9063332b663185703 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.body 02c5579555eac1d6d14738fbb7a0084e3d618b3e574b7f33918324186ddc3313 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.inputs 01117716b9b77d87626ab77cb7b531a7aa8e72410bda174b72535a53c900d724 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.outputs 18b054237f28c506cb3159ee489fd7edc0124d0729d0c0f4287ee0cc9d26b1ed -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.threadsX 5fb475274d94a9cfefe648d4a287e9722322af0a3ac0fe66ce4797e777a01f68 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.threadsY 6da33451a1567586212b522147cb7811e7bba5e60162914a1fc35de800048a8b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.threadsZ 8854c9a54000143f44fb459ecbf05677072366cdae28d67acdc0d6e718b24fe9 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStage.contentHash 85ae0564ae03a4491c6ee69bce43363c61c940f998dada78442f36e95bd52f5e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1a133fe2d98a20a7"></a>[`public Inno.Rendering.Shaders.ShaderIrStage.ShaderIrStage(Inno.Rendering.ShaderStage stage, Inno.Rendering.Shaders.ShaderIrBlock body, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStageInput> inputs, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStageOutput> outputs, int threadsX = 1, int threadsY = 1, int threadsZ = 1)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L216) | Validates and freezes a single stage's interface and ordered body.<br>`stage`：Exactly Vertex, Fragment or Compute.<br>`body`：Immutable typed instructions and named outputs.<br>`inputs`：Exactly the input bindings read by the body.<br>`outputs`：Exactly the GPU destinations written by the body.<br>`threadsX`：Compute workgroup X size; one for raster stages.<br>`threadsY`：Compute workgroup Y size; one for raster stages.<br>`threadsZ`：Compute workgroup Z size; one for raster stages. | — | — |
| <a id="api-80db68cf1e129bde"></a>[`public Inno.Rendering.Shaders.ShaderIrBlock Inno.Rendering.Shaders.ShaderIrStage.body { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L250) | Gets its immutable ordered instructions. | — | — |
| <a id="api-ffbcf55af3cf059b"></a>[`public string Inno.Rendering.Shaders.ShaderIrStage.contentHash { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L275) | Gets a deterministic hash of stage semantics, ordered effects, nested regions, complete types, resource layout and frozen sources. A compiled artifact cache must additionally include toolchain identity, target capabilities, optimization and variant configuration. | — | — |
| <a id="api-c2ba77b8405f3e8d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStageInput> Inno.Rendering.Shaders.ShaderIrStage.inputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L254) | Gets all input bindings. | — | — |
| <a id="api-9510bea69efa583d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStageOutput> Inno.Rendering.Shaders.ShaderIrStage.outputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L258) | Gets all output destinations. | — | — |
| <a id="api-8c35c23a9658328e"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.Shaders.ShaderIrStage.stage { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L246) | Gets the programmable stage. | — | — |
| <a id="api-e9e795275a8da82a"></a>[`public int Inno.Rendering.Shaders.ShaderIrStage.threadsX { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L262) | Gets the compute workgroup X size. | — | — |
| <a id="api-a3eabf2d83984493"></a>[`public int Inno.Rendering.Shaders.ShaderIrStage.threadsY { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L266) | Gets the compute workgroup Y size. | — | — |
| <a id="api-031579c273060026"></a>[`public int Inno.Rendering.Shaders.ShaderIrStage.threadsZ { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L270) | Gets the compute workgroup Z size. | — | — |
| <a id="api-743271efa4cbf897"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrStage`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L190) | Freezes a typed GPU stage and its explicit interface. It contains no main function, varying declarations or source expressions. Targets own stage/resource layout; adapters translate these semantics into their native language. | — | — |

### `Inno.Rendering.Shaders.ShaderIrStageInput`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrStageInput 3e54ec1cefd47e1fda28b9da6dbe23f6f097a0d3dc5c6fd13ca5ce04c7337605 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrStageInput.#ctor(System.String,Inno.Rendering.Shaders.ShaderSourceType,Inno.Rendering.Shaders.ShaderIrInputKind,System.String,System.Int32) 174cb8446f4d91578f001c1f2b485fd34e48bd85adcacde5e3dde118dd1ab0bd -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageInput.id 25af6b689c9029e3a4af50dabf3b2fe6b1594f2af323ec4cdee0f5cd869cb88d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageInput.type dc84e472b6fbe4741e697ae7b983debe3d7f175a00b3d5934e122044b112d633 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageInput.kind d4a90fd014e35259eb18b7ae2f9602528eda0d06f2d3928bb76ca3a751a72370 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageInput.semantic af6904ee29280b2f3659186d4a060296b03bbcea787111f79c03f0bab02c371d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageInput.location 632dd2be59eda21c683a0cc6a0bb792d6847ae0291bfb1993d7667d849bff760 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1dd1a5808d785ab"></a>[`public Inno.Rendering.Shaders.ShaderIrStageInput.ShaderIrStageInput(string id, Inno.Rendering.Shaders.ShaderSourceType type, Inno.Rendering.Shaders.ShaderIrInputKind kind, string semantic = "", int location = 0)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L85) | Creates an immutable stage input binding.<br>`id`：Stable binding identity, also used by the block's named Input instruction.<br>`type`：Complete non-void type.<br>`kind`：Source of the GPU value.<br>`semantic`：Target semantic, such as position, texcoord, color, instance-data or fragment-coordinate.<br>`location`：Attribute/semantic index or assigned texture slot, never a native handle. | — | — |
| <a id="api-afb3500d458fc4a6"></a>[`public string Inno.Rendering.Shaders.ShaderIrStageInput.id { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L111) | Gets the logical binding identity. | — | — |
| <a id="api-0649b19dfeb2169c"></a>[`public Inno.Rendering.Shaders.ShaderIrInputKind Inno.Rendering.Shaders.ShaderIrStageInput.kind { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L119) | Gets the GPU input category. | — | — |
| <a id="api-2ae529771335f1e8"></a>[`public int Inno.Rendering.Shaders.ShaderIrStageInput.location { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L127) | Gets the target-assigned interface location or sampled-texture slot. | — | — |
| <a id="api-df4bee17640bef47"></a>[`public string Inno.Rendering.Shaders.ShaderIrStageInput.semantic { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L123) | Gets the target semantic; an adapter must reject unsupported semantics. | — | — |
| <a id="api-2db5b8d87b8e7d94"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderIrStageInput.type { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L115) | Gets the complete typed value received by the stage. | — | — |
| <a id="api-e72b1aca01702f6d"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrStageInput`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L65) | Describes a stage input without embedding native declarations or expressions. | — | — |

### `Inno.Rendering.Shaders.ShaderIrStageOutput`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrStageOutput 52185aa42909ede3542f3654d75f18947b0b4389b2e90aa9e69733c6fc74717e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderIrStageOutput.#ctor(System.String,Inno.Rendering.Shaders.ShaderIrOutputKind,System.String,System.Int32) 330b9ee5bb35c3555e935ed8eb8a8e79f25b234f36669f1d37e61a6a2554d225 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageOutput.id d409dc2e580e5577aa26fd0b93fcb08fcdc107ad24d504c8f41d6d3c4e806138 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageOutput.kind 7fc9ef1b5c819bdc058c9c63cbafbf1cc24d5effbfc92c66db83b86af8fe1696 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageOutput.semantic 29a80ff5e06fcc2ff85fb07fbed7612169261d5939859fcbcd844bc96d15eb94 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrStageOutput.location 78f266bb6f8775faa16e52472edbf1fa59331c92fce2bd8d15e814bd5eb6e9a7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6c756a48d89f91b1"></a>[`public Inno.Rendering.Shaders.ShaderIrStageOutput.ShaderIrStageOutput(string id, Inno.Rendering.Shaders.ShaderIrOutputKind kind, string semantic = "", int location = 0)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L150) | Creates an immutable stage output binding.<br>`id`：The exact name in the block's output map.<br>`kind`：GPU output category.<br>`semantic`：Varying semantic matched by the next stage; otherwise empty.<br>`location`：Varying index or color attachment index. | — | — |
| <a id="api-708b0f4dc536c6f0"></a>[`public string Inno.Rendering.Shaders.ShaderIrStageOutput.id { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L171) | Gets the exact block output identity. | — | — |
| <a id="api-fc1d975d25e9b8c2"></a>[`public Inno.Rendering.Shaders.ShaderIrOutputKind Inno.Rendering.Shaders.ShaderIrStageOutput.kind { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L175) | Gets the GPU output category. | — | — |
| <a id="api-a8760ba2bafdf4c3"></a>[`public int Inno.Rendering.Shaders.ShaderIrStageOutput.location { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L183) | Gets the target-assigned varying or attachment index. | — | — |
| <a id="api-7d245ccc06038732"></a>[`public string Inno.Rendering.Shaders.ShaderIrStageOutput.semantic { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L179) | Gets the varying semantic, not native assignment syntax. | — | — |
| <a id="api-1551b6a1eac1b00e"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrStageOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L133) | Describes a named block output's GPU destination. | — | — |

### `Inno.Rendering.Shaders.ShaderIrValue`

<!-- api: T:Inno.Rendering.Shaders.ShaderIrValue 380a970a2f12c1878ae8b000e41ca05e1da0d4ca48fda4be90a6f8e3b8dcbd19 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrValue.index 250457d9f153e0b53f85bb9be5031118d14bdad6345f5fb0cdf91185b02d3cd7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderIrValue.type 6a3c94bc1a02f0f7eda5aa841f8ac090bbcf37fddd832eba63bcbefcebbcf9b7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d5205356ac843fe4"></a>[`public int Inno.Rendering.Shaders.ShaderIrValue.index { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L26) | Gets the deterministic block-local value index, not a GPU register or resource slot. | — | — |
| <a id="api-9bdc632e4e5e347a"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderIrValue.type { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L30) | Gets the complete language-independent value type. | — | — |
| <a id="api-b5e283593e7c2ede"></a>[`public sealed class Inno.Rendering.Shaders.ShaderIrValue`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L11) | Identifies a typed single-assignment value within one intermediate block. | — | — |

### `Inno.Rendering.Shaders.ShaderNodeCompilerCatalog`

<!-- api: T:Inno.Rendering.Shaders.ShaderNodeCompilerCatalog 63ce907b1b76c55775a5a534512fa966253b2f18a1901fc83bd5260a12d368c3 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.IShaderNodeCompiler}) fcb84e604a82e45afd740bd569c94f8b23bf336f586de981f01790340cd64e95 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.definitionIds 9721b75c2c0b7b7c296bc540f4112a5daca3f79b7aaa69ac13c98232ac14192e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.DescribePorts(Inno.Core.Graphs.GraphNodeRecord,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,Inno.Rendering.Shaders.ShaderSourceModuleAnalysis,System.String,Inno.Rendering.Shaders.ShaderIrStageInput) d590f8ebff4343848c99630180f5867691507ff266d99aeec0d8defd2b89d246 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,System.Threading.CancellationToken) ce9de4216bacae627da675db21808713e63af178b9ba4a1ad24eb18bb9ef50fa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f3237e47574f6fd"></a>[`public Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.ShaderNodeCompilerCatalog(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.IShaderNodeCompiler> compilers)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L27) | Validates a complete generation of node compiler registrations.<br>`compilers`：Providers owned by the calling generation or composition scope. | — | — |
| <a id="api-378b526166fd17ac"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.DescribePorts(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? source = null, string implementationId = "", Inno.Rendering.Shaders.ShaderIrStageInput? input = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L70) | Describes current typed ports for editor presentation without exposing the compiler provider.<br>返回：A detached immutable typed port snapshot.<br>`node`：Neutral node properties.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context.<br>`source`：Resolved frozen function module, or null.<br>`implementationId`：Selected source implementation.<br>`input`：Resolved stage input interface, or null. | — | — |
| <a id="api-2c2452cd53c490a2"></a>[`public Inno.Rendering.Shaders.ShaderGraphLoweringResult Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest request, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L103) | Lowers an entire target-selected region, preserving stable topological/document order and all source calls. Missing definitions, stale ports and cycles fail without mutating graph records or dropping edges.<br>返回：A detached typed region or precise graph diagnostics.<br>`request`：Frozen region, outputs and resolved source/target data.<br>`serialization`：The current owner's native converter registry.<br>`context`：The complete owner reference/asset serialization context; never synthesized by this compiler.<br>`cancellationToken`：Cancellation between node invocations. | — | — |
| <a id="api-559abb8eb6e515b8"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.definitionIds { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L44) | Gets stable registered definition identities, without exposing providers. | — | — |
| <a id="api-623495418c472314"></a>[`public sealed class Inno.Rendering.Shaders.ShaderNodeCompilerCatalog`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L16) | Owns an immutable compiler map; the common lowering algorithm never switches on concrete node identities. | — | — |

### `Inno.Rendering.Shaders.ShaderNodeCompilerRegistry`

<!-- api: T:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry 7cca4c3261cb7f2fc740e412bc712dff4747c5e6e1f7e4f57333189c70deb26d -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) 3e1d4d91305135062598ad102489e4599b0fe7e9f8d5c8ef06797dec2723f7c0 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.definitionIds bc18f5f2138ca278fe74acb2bb278c29b9e1b885392103ae85efcb0e2c709c7a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,System.Threading.CancellationToken) a1a443d945e5d543fd4689e07f4a527143c056af9a5aa7e5048d95262515a0c4 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DescribePorts(Inno.Core.Graphs.GraphNodeRecord,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,Inno.Rendering.Shaders.ShaderSourceModuleAnalysis,System.String,Inno.Rendering.Shaders.ShaderIrStageInput) c0a4e9c7e2ba5166659a0521d2173d1fb8db057fbd7c7c85a0546852ea33dc6f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot) 66864dd983120278532105f635cfc01eb5cd9de0476e4b1023fd33d5d94ba372 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderNodeCompilerCatalog) b80f9d5ecc0d98ca0ea3c95785412aac387de4eea66250fb02886541ea535055 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8a0f4fd02bb5f224"></a>[`public Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.ShaderNodeCompilerRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L24) | Registers the compiler generation owner with the shared type catalog.<br>`types`：Owner catalog, which must outlive this registry. | — | — |
| <a id="api-c7f6cd61ce12cab0"></a>[`protected override Inno.Rendering.Shaders.ShaderNodeCompilerCatalog Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L111) | Builds a validated result from the current immutable input snapshot.<br>返回：The validated shader node compiler catalog that represents the completed operation.<br>`types`：The active type catalog generation used for extension resolution. | — | — |
| <a id="api-14ecea20bcf55cfc"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DescribePorts(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? source = null, string implementationId = "", Inno.Rendering.Shaders.ShaderIrStageInput? input = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L90) | Captures a node's current typed ports under the shared generation lease.<br>返回：An immutable provider-free port snapshot.<br>`node`：Neutral node properties.<br>`serialization`：Owner converter registry.<br>`context`：Complete owner reference context.<br>`source`：Resolved frozen source function, or null.<br>`implementationId`：Selected source implementation identity.<br>`input`：Resolved stage input interface, or null. | — | — |
| <a id="api-be2112b69b1c7b44"></a>[`protected override void Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderNodeCompilerCatalog snapshot)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L126) | Releases the generation lease retained by an immutable registry snapshot.<br>`snapshot`：The immutable state snapshot consumed by this operation. | — | — |
| <a id="api-043e3fae57efe102"></a>[`public Inno.Rendering.Shaders.ShaderGraphLoweringResult Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest request, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L56) | Lowers a whole region under one shared generation operation.<br>返回：A detached region or neutral diagnostics, never compiler instances.<br>`request`：Frozen graph region and resolved immutable inputs.<br>`serialization`：The current owner converter registry.<br>`context`：The complete owner reference/asset serialization context.<br>`cancellationToken`：Cancellation between node invocations. | — | — |
| <a id="api-7301a233c7d42900"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.definitionIds { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L29) | Gets stable compiler identities without exposing provider instances. | — | — |
| <a id="api-2df0f3d3fb54125b"></a>[`public sealed class Inno.Rendering.Shaders.ShaderNodeCompilerRegistry : Inno.Extensibility.Types.TypeRegistry<Inno.Rendering.Shaders.ShaderNodeCompilerCatalog>`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L14) | Owns reloadable shader node compilers through the shared candidate, rollback and retirement protocol. | — | — |

### `Inno.Rendering.Shaders.ShaderNodeDescriptionContext`

<!-- api: T:Inno.Rendering.Shaders.ShaderNodeDescriptionContext 731bf28fe0f321432938fe377ede6fc453089cf0d6b51f6d3591c2f4817303e5 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.nodeId 55e6158ebd6107f67c53ee8d5ff9e4b87da13819302aba5d305bebe32967dbc0 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.definitionId ce6b25426f8efe62c8ab7df1a87c8f5742e26d3f3f2faa9f11b40c575f7d3560 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.sourceModule 9b4de40a28bd13e052a9219c08319015a06073a46302e4c1874458f197a67c7c -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.implementationId 4bfb1000a102f3d8bc83416a35c8bf6e9be39551f61b4a9dc05dadf476c596f6 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.stageInput 74bd9e09e933cc4d8af6b5cbbe2ae9b05c2dd49bd34fcc3cce1f0716c9656114 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeDescriptionContext.Read``1(System.String,``0) 59898d30f316429a96951c4eab866d7b909f0377285f8118752dd221f6e318bc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-62eeb15bf65bdf47"></a>[`public T Inno.Rendering.Shaders.ShaderNodeDescriptionContext.Read<T>(string id, T defaultValue)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L124) | Reads a property using the owner's converter generation and complete reference resolver context.<br>返回：The deserialized value or the supplied absent-property default.<br>`id`：Stable property identity.<br>`defaultValue`：Value used only when the property is absent, never after corrupt-data failure. | — | — |
| <a id="api-d00f9cc9c9e52992"></a>[`public string Inno.Rendering.Shaders.ShaderNodeDescriptionContext.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L95) | Gets the node definition's stable identity. | — | — |
| <a id="api-317352845e9bfb65"></a>[`public string Inno.Rendering.Shaders.ShaderNodeDescriptionContext.implementationId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L103) | Gets the exact implementation key chosen by the target. | — | — |
| <a id="api-ed682059231a99af"></a>[`public Inno.Core.Graphs.GraphNodeId Inno.Rendering.Shaders.ShaderNodeDescriptionContext.nodeId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L91) | Gets the node's stable identity, never its editor runtime handle. | — | — |
| <a id="api-eb928cf7c7c1d38f"></a>[`public Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? Inno.Rendering.Shaders.ShaderNodeDescriptionContext.sourceModule { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L99) | Gets a frozen module resolved by the asset owner; null means unassigned or unavailable. | — | — |
| <a id="api-10ae05816c2d647f"></a>[`public Inno.Rendering.Shaders.ShaderIrStageInput? Inno.Rendering.Shaders.ShaderNodeDescriptionContext.stageInput { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L107) | Gets the target-assigned stage input for this node, when applicable. | — | — |
| <a id="api-86933343dad23cf8"></a>[`public sealed class Inno.Rendering.Shaders.ShaderNodeDescriptionContext`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L66) | Provides invocation-scoped property decoding through the complete owner serialization context. | — | — |

### `Inno.Rendering.Shaders.ShaderNodeLoweringContext`

<!-- api: T:Inno.Rendering.Shaders.ShaderNodeLoweringContext e87eaf1f3af015b5382ed7a4a601a3b3a89729d8975737355115948e48d795ca -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeLoweringContext.description 308be04e99680eba51407e490786abce22e9a8a39e254b95688ff186c1edfbea -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeLoweringContext.builder b4f0c41272b04f74731e2841ac9b2cccaa1ae32a91e88bc805331690356899ad -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodeLoweringContext.inputs 5e64f017707c2ccf777a885c7e87b003031459f8526830e7557dd98e349822a9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodeLoweringContext.Input(System.String) 9aba5de3fd94fda8966d2bcc3ba4d47e4bcbd8f6a11267629eae13fd03dad3c0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fb7599e821506ce7"></a>[`public Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderNodeLoweringContext.Input(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L171) | Requires one connected input without implicit defaults or positional rebinding.<br>返回：The connected typed value.<br>`id`：Exact semantic input port identity. | — | — |
| <a id="api-00b5176d479d96c6"></a>[`public Inno.Rendering.Shaders.ShaderIrBuilder Inno.Rendering.Shaders.ShaderNodeLoweringContext.builder { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L157) | Gets the builder shared by nodes in this typed region. | — | — |
| <a id="api-1aff326b66e21a0c"></a>[`public Inno.Rendering.Shaders.ShaderNodeDescriptionContext Inno.Rendering.Shaders.ShaderNodeLoweringContext.description { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L153) | Gets the invocation-scoped node description. | — | — |
| <a id="api-fc8d8eb785c69429"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderNodeLoweringContext.inputs { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L161) | Gets connected values keyed by semantic input port identity, never connection order. | — | — |
| <a id="api-1615a4fb8a4db4ed"></a>[`public sealed class Inno.Rendering.Shaders.ShaderNodeLoweringContext`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L138) | Supplies one node's connected values and the region builder during lowering only. | — | — |

### `Inno.Rendering.Shaders.ShaderNodePort`

<!-- api: T:Inno.Rendering.Shaders.ShaderNodePort 46ceb16ff25aaa4fa68d679f2bccc90552c397018633ce33bc8a586da40d533e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.#ctor(System.String,Inno.Rendering.Shaders.ShaderSourceType,Inno.Core.Graphs.GraphPortDirection,System.Boolean) 06461a8b639c03701572f884c714a989ea0fbebc3c03b9e4ec325fa75237aa1c -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodePort.id 0b0d08ce97b25540033750fff9e1187b6aaa6c421d0b1ff11df467825d037148 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodePort.type 189d675f50e301a14e70e3bd8033e31bc4c70b24c7651946c3a00130666a4d8f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodePort.direction 3ad90bf84b7923d73ddeb78afc46036c5d006464d551bb784dcefc3a05c466ca -->
<!-- api: P:Inno.Rendering.Shaders.ShaderNodePort.required f237913ed90b314c158e5af71329a745893c52a324c5e8bd995123586bf2470e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.ToString dad751c3db7f57a727417be6a177a0609c6047793a4f744e04f866fddd8ab583 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.op_Inequality(Inno.Rendering.Shaders.ShaderNodePort,Inno.Rendering.Shaders.ShaderNodePort) 13a22a606f0c33f9dd6c46bf785348ab25a4783b65ceb65db23d64be639f4ed3 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.op_Equality(Inno.Rendering.Shaders.ShaderNodePort,Inno.Rendering.Shaders.ShaderNodePort) ed8f6f564d704cb41aa0f05f8244ed92095470e383fad730d2d142cc30c900b6 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.GetHashCode 5b13c47abf792499edc1595b4dda76e8861c60e3a5e4fdd07f4282e0b0de9a0a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.Equals(System.Object) 0e1953375dd794ce95943a832e04f9afef98ec0b6627fedfcdf9c76e18272270 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.Equals(Inno.Rendering.Shaders.ShaderNodePort) 06830f4843ca2121b4e24bf0049ec0306f30c379632c07593ce4fd0f66d8481f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderNodePort.Deconstruct(System.String@,Inno.Rendering.Shaders.ShaderSourceType@,Inno.Core.Graphs.GraphPortDirection@,System.Boolean@) 309c572b59f6449a2135f7353cd7bec4eb9f9f6979c7919f0211b8fe40c88281 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-764e05e150182397"></a>[`public Inno.Rendering.Shaders.ShaderNodePort.ShaderNodePort(string id, Inno.Rendering.Shaders.ShaderSourceType type, Inno.Core.Graphs.GraphPortDirection direction, bool required = true)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Declares a typed shader port independently of editor presentation and backend source syntax.<br>`id`：Stable node-local port identity.<br>`type`：Complete value type, including all aggregate fields.<br>`direction`：Input or output value flow.<br>`required`：Whether an input must be connected; alternative aggregate/member inputs validate in their compiler. | — | — |
| <a id="api-061902830e309261"></a>[`public void Inno.Rendering.Shaders.ShaderNodePort.Deconstruct(out string id, out Inno.Rendering.Shaders.ShaderSourceType type, out Inno.Core.Graphs.GraphPortDirection direction, out bool required)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-6289e83387f5063f"></a>[`public bool Inno.Rendering.Shaders.ShaderNodePort.Equals(Inno.Rendering.Shaders.ShaderNodePort? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-39364d624b235191"></a>[`public override bool Inno.Rendering.Shaders.ShaderNodePort.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-01665b321d2e21ac"></a>[`public override int Inno.Rendering.Shaders.ShaderNodePort.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-0ffa210250958dd0"></a>[`public static bool Inno.Rendering.Shaders.ShaderNodePort.operator ==(Inno.Rendering.Shaders.ShaderNodePort? left, Inno.Rendering.Shaders.ShaderNodePort? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-196d13a215cb0695"></a>[`public static bool Inno.Rendering.Shaders.ShaderNodePort.operator !=(Inno.Rendering.Shaders.ShaderNodePort? left, Inno.Rendering.Shaders.ShaderNodePort? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9a7757df3e38835e"></a>[`public override string Inno.Rendering.Shaders.ShaderNodePort.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-066c352ef87a3c23"></a>[`public Inno.Core.Graphs.GraphPortDirection Inno.Rendering.Shaders.ShaderNodePort.direction { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L28) | Input or output value flow. | — | — |
| <a id="api-62bbd36632e4a619"></a>[`public string Inno.Rendering.Shaders.ShaderNodePort.id { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L26) | Stable node-local port identity. | — | — |
| <a id="api-3cf9879e9b86397f"></a>[`public bool Inno.Rendering.Shaders.ShaderNodePort.required { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L29) | Whether an input must be connected; alternative aggregate/member inputs validate in their compiler. | — | — |
| <a id="api-683d1dab0a0131d3"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderNodePort.type { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L27) | Complete value type, including all aggregate fields. | — | — |
| <a id="api-7f970da683c5cadc"></a>[`public sealed record Inno.Rendering.Shaders.ShaderNodePort : System.IEquatable<Inno.Rendering.Shaders.ShaderNodePort>`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Declares a typed shader port independently of editor presentation and backend source syntax.<br>`id`：Stable node-local port identity.<br>`type`：Complete value type, including all aggregate fields.<br>`direction`：Input or output value flow.<br>`required`：Whether an input must be connected; alternative aggregate/member inputs validate in their compiler. | — | — |

### `Inno.Rendering.Shaders.ShaderRerouteNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderRerouteNodeCompiler 0fe7dbdb1d8c2599c2132711a0d4c23bbd3cda9c946cb20bc7b8910192348826 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.definitionId e1b6a6dd0c83c21aa0a6dfb3e0461b1498eed21a8b1f444b2ae324c0f90d2442 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 3eb82923e15a3c80b0e5b3736e053c2f65fe8ac272c49f339c3ebdd39718e11e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) cf3c78a0dadf6eac823cf7dc79b37bf101ab604166e810126f9374238a010f62 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.#ctor 85ecbbf3d00a21880518dae92fd2763f936791c14086d14859fd5e79a72b8b11 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1908bb1d969a72d"></a>[`public Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.ShaderRerouteNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9fea490a8a930b5e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L24) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-887cc2f99e2cd811"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L38) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-827bb2db5d78f72c"></a>[`public string Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L14) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-45710c8819a4e5f6"></a>[`public sealed class Inno.Rendering.Shaders.ShaderRerouteNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L9) | Forwards a typed connection without adding instructions or changing resource-effect order. | — | — |

### `Inno.Rendering.Shaders.ShaderSampleNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderSampleNodeCompiler d43ed5435d0de2af6ddb0c25e56305241ccbd2eef52e5e9b8a9e87b89b804068 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSampleNodeCompiler.definitionId c219176dcdd7abd34c0ab846195336e17c2aa0d8a4f0cb903b5e8171d25f7498 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSampleNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) b6320b0677e65c11f2e8a69be9da1b2e0f7ba6a13ed6dfb1d6c234f3d8ff3bc6 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSampleNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) a8698c7b17387e5446eac0dc385448644deffb13d18b8693d999f08af6fcfe2a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSampleNodeCompiler.#ctor 40c9e28925bb2316a7673b6c418d36a775ab77f9ddbfaeae3b1c481406bbe244 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d1b83abc563f3347"></a>[`public Inno.Rendering.Shaders.ShaderSampleNodeCompiler.ShaderSampleNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L48) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-961f069ecde8eb43"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSampleNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L63) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-8df08175fc9bb51e"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSampleNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L82) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-80ed71331c0cbf17"></a>[`public string Inno.Rendering.Shaders.ShaderSampleNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L53) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-aa229d0b1f57e89d"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSampleNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L48) | Samples a graph-connected texture with implicit derivatives or an explicit level of detail. | — | — |

### `Inno.Rendering.Shaders.ShaderSelectNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderSelectNodeCompiler 43e34bc5529b5a701970a61bb144652d4905b913bbd0f949f656c2d6ddf288be -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSelectNodeCompiler.definitionId fd9a727466125169bbb2bfd045581ab7096f80b313e58690b3f52578d453870a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSelectNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) da8e65ad6bb5c47bb522b1c910274d622e076a0efde47f3e9a09abfc6e96d6a9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSelectNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) 47ac989983f0c09e7e151e8e66575bd3883c2a11ee391aad7f83682472556d98 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSelectNodeCompiler.#ctor a5a47bbcb07be7f6b3d797d5b9c133df2d45c0f796d4658fb93bee7a2de43750 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e864cbe4c6ff3aa2"></a>[`public Inno.Rendering.Shaders.ShaderSelectNodeCompiler.ShaderSelectNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L195) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e3663dc79aba7546"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSelectNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L210) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-4b6bf72b36e1c715"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSelectNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L225) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-d899609117eb04bc"></a>[`public string Inno.Rendering.Shaders.ShaderSelectNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L200) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-2470847c2b2da51b"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSelectNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L195) | Selects between equal typed values; all producer effects remain evaluated before selection. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceAnalysis`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceAnalysis 30ee7af30d74a21cff2fb7fa2f65dab5dccd174b8b7f870a1f181fb70f247f19 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceAnalysis.#ctor(Inno.Rendering.Shaders.ShaderSourceFunction,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderSourceDiagnostic}) 773370949f7015c731e30cd20b46aed2608242e57b47859210756e0f27624083 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceAnalysis.function 7b1cb1f2cc9d60acfd545295f62f9c03af1c036b5016f55fd1ca16bbc9d8a8dc -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceAnalysis.dependencies ee79bd2f941d3b50e56468173723cd409349bbc7efd1f9032442b5aec870f203 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceAnalysis.diagnostics c8f935f4a58de6c3ed7d18899fe8d3eb54406fb017b281fac4bc65fa8b4d3e3a -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceAnalysis.succeeded d0cfe2594404ad0c081c784812828b930606cd9370a750e0f71b93a2267999ad -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-53f0e115d11650d9"></a>[`public Inno.Rendering.Shaders.ShaderSourceAnalysis.ShaderSourceAnalysis(Inno.Rendering.Shaders.ShaderSourceFunction? function, System.Collections.Generic.IEnumerable<string> dependencies, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceDiagnostic> diagnostics)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L155) | Captures a completed analysis without retaining compiler objects.<br>`function`：Parsed interface, or null when parsing failed.<br>`dependencies`：Resolved included sources, excluding the root.<br>`diagnostics`：All problems discovered by the frontend. | — | — |
| <a id="api-d7e1ef91684bbecc"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceAnalysis.dependencies { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L173) | Gets original source dependencies for invalidation. | — | — |
| <a id="api-2dca64ddfc287e6c"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceDiagnostic> Inno.Rendering.Shaders.ShaderSourceAnalysis.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L177) | Gets immutable, provider-neutral diagnostics. | — | — |
| <a id="api-70bec7042baafc5a"></a>[`public Inno.Rendering.Shaders.ShaderSourceFunction? Inno.Rendering.Shaders.ShaderSourceAnalysis.function { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L169) | Gets the parsed interface, or null after an analysis failure. | — | — |
| <a id="api-9619b702b323996c"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceAnalysis.succeeded { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L181) | Gets whether a usable interface was found without errors. | — | — |
| <a id="api-c5b76d71f4b7c058"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L141) | Returns a recoverable source analysis result and complete original-source dependencies. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceDiagnostic`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceDiagnostic e5a629f349fe0e1d5726a6b8607aef0fe50402d3f3eba76c582b41f3103ae8ce -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.#ctor(System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,Inno.Rendering.Shaders.ShaderSourcePosition) c56bf11c5ce245099d6f4e90d48ba7c3846022d02ec67443c840544f869e850b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceDiagnostic.code cb4408656d1fa98d23358f829160c5ae98807c05664b5e632db6f9a9a0285dea -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceDiagnostic.severity 86c686252a266b6650b52c4bcda5663fa91ba9d27ead48d32e1754e9479aa6ec -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceDiagnostic.message 676ab51e584c6e3e1c8847918ea8f5ad5aefd57ed1c05b25a8dd0408f086f947 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceDiagnostic.location a084c9aab3a1db887bd2bde048218c19ec4f8d6ff7279153240b30f1013bdaa9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.ToString 91ab5190ca6c69fdd3c8096334b6fe6bc8f4f2e7a583af4259369b5a978e4116 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.op_Inequality(Inno.Rendering.Shaders.ShaderSourceDiagnostic,Inno.Rendering.Shaders.ShaderSourceDiagnostic) 59496499d4f35e7734066f7e01b6980a1e4fa310cd95f680a368a37d17a3211a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.op_Equality(Inno.Rendering.Shaders.ShaderSourceDiagnostic,Inno.Rendering.Shaders.ShaderSourceDiagnostic) e36fe768cdf29c0344ebda021ae12979c1de155cf5f0bf06b3df8cd95ed908f4 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.GetHashCode 64c2adeeab755432a77d9c0a9f3d4d656af9f4577bcb8b503bd8dfee9373755c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.Equals(System.Object) 42232f4671508021027276ec4d8ed35fb3eab3ef2714407f4ce5cc86c9e92360 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.Equals(Inno.Rendering.Shaders.ShaderSourceDiagnostic) 301a2f8f055c55009fbf29459ef08408b859a8aa7cbe69a6431c49f6502c2508 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceDiagnostic.Deconstruct(System.String@,Inno.Core.Diagnostics.DiagnosticSeverity@,System.String@,Inno.Rendering.Shaders.ShaderSourcePosition@) e43f6bc69e41b7ab41fc79038eda9ba7502d4de915b161b00697189fbe3df480 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-28ce6afaa201915c"></a>[`public Inno.Rendering.Shaders.ShaderSourceDiagnostic.ShaderSourceDiagnostic(string code, Inno.Core.Diagnostics.DiagnosticSeverity severity, string message, Inno.Rendering.Shaders.ShaderSourcePosition location)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Reports a frontend problem without retaining a language-specific exception or parser.<br>`code`：Stable diagnostic code.<br>`severity`：Diagnostic severity.<br>`message`：Actionable diagnostic description.<br>`location`：Original source position. | — | — |
| <a id="api-0f34ea2538b162fd"></a>[`public void Inno.Rendering.Shaders.ShaderSourceDiagnostic.Deconstruct(out string code, out Inno.Core.Diagnostics.DiagnosticSeverity severity, out string message, out Inno.Rendering.Shaders.ShaderSourcePosition location)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-e770877a9641332d"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceDiagnostic.Equals(Inno.Rendering.Shaders.ShaderSourceDiagnostic? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-7cdadb9f11f2ac8a"></a>[`public override bool Inno.Rendering.Shaders.ShaderSourceDiagnostic.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-c0c9bcfdf51d1dba"></a>[`public override int Inno.Rendering.Shaders.ShaderSourceDiagnostic.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-0849402f505b8436"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceDiagnostic.operator ==(Inno.Rendering.Shaders.ShaderSourceDiagnostic? left, Inno.Rendering.Shaders.ShaderSourceDiagnostic? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5baf9419f3d4cdc0"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceDiagnostic.operator !=(Inno.Rendering.Shaders.ShaderSourceDiagnostic? left, Inno.Rendering.Shaders.ShaderSourceDiagnostic? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c89f3b045697a086"></a>[`public override string Inno.Rendering.Shaders.ShaderSourceDiagnostic.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-46634a03654b6ea8"></a>[`public string Inno.Rendering.Shaders.ShaderSourceDiagnostic.code { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L42) | Stable diagnostic code. | — | — |
| <a id="api-00b704150cb515be"></a>[`public Inno.Rendering.Shaders.ShaderSourcePosition Inno.Rendering.Shaders.ShaderSourceDiagnostic.location { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L45) | Original source position. | — | — |
| <a id="api-ba6b9afb54f00a5b"></a>[`public string Inno.Rendering.Shaders.ShaderSourceDiagnostic.message { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L44) | Actionable diagnostic description. | — | — |
| <a id="api-f37d64bfb062d8a9"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.Shaders.ShaderSourceDiagnostic.severity { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L43) | Diagnostic severity. | — | — |
| <a id="api-6586c6dca809d6f2"></a>[`public sealed record Inno.Rendering.Shaders.ShaderSourceDiagnostic : System.IEquatable<Inno.Rendering.Shaders.ShaderSourceDiagnostic>`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Reports a frontend problem without retaining a language-specific exception or parser.<br>`code`：Stable diagnostic code.<br>`severity`：Diagnostic severity.<br>`message`：Actionable diagnostic description.<br>`location`：Original source position. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceField`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceField d75bb1c1659c5aa898457d5f78c55f1f34876ca4bc19f2293f4c53a1e8582247 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceField.#ctor(System.String,Inno.Rendering.Shaders.ShaderSourceType) 036dd28c931342afb0d1cf6244a5da7b14e21d4318ca19ec576bfccbd5ed06f8 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceField.name 5081cd329ef09e56b1d3c45859608d7090a722d4f3838d6529423bf6852207e2 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceField.type dbcb9a5b974081ac1155823f9d9bf6fea2aa1005c4ac49690944e6803b50b8ba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c571356a4a5cfc2f"></a>[`public Inno.Rendering.Shaders.ShaderSourceField.ShaderSourceField(string name, Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L169) | Creates a structure field.<br>`name`：Stable field name.<br>`type`：Complete field type. | — | `System.ArgumentException`：The field has void type or is an opaque storage binding. |
| <a id="api-5d2e50c69d393c2a"></a>[`public string Inno.Rendering.Shaders.ShaderSourceField.name { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L182) | Gets the public field name. | — | — |
| <a id="api-593e9c9420b4cf22"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceField.type { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L186) | Gets the complete field type. | — | — |
| <a id="api-06072236b7d53a52"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceField`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L155) | Describes one immutable named structure field. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceFile`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceFile 46809e9acf49029502bc7feb47438049535597ba2931e6e3fd6a93fe74a4050e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.#ctor(System.String,System.String) 0ba56cff07b448b796b2b1d01db9c2705d12bb6a6b1d7645611b3fca62f258d8 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFile.assetPath 5a1b788cea9ed69f685de9b7c7bbd76bf347e38d87fbcfc2e5f605261f21b804 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFile.text c4bce54a48a18ca4f4426e53ba57f0633fd619f102cd42ecce8a601213c9bcf4 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.ToString 04fad2c1f5d2afe7e143f936825657ade889cc0384fe52b2ab0b38a6362ff4cb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.op_Inequality(Inno.Rendering.Shaders.ShaderSourceFile,Inno.Rendering.Shaders.ShaderSourceFile) 6aab2a6166efbd2ac917169a10c330d34c3dbfd3e46e331b18784ec57a9327f9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.op_Equality(Inno.Rendering.Shaders.ShaderSourceFile,Inno.Rendering.Shaders.ShaderSourceFile) f0591e3fe53124533e50de625cf5e0bb41785b12e0e93afe36a9c10a7e06030e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.GetHashCode 1fda97bc2acab432cd2ed28c6b568953a1a1722c5272ec0247217603b361f3b6 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.Equals(System.Object) 67e85889d77f0e3afee8c36fb172ad9b80027f9a03c8bde0d21db78d37efb8da -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.Equals(Inno.Rendering.Shaders.ShaderSourceFile) 3c73f7a0a3d7bb8a3b87fc8f6639d3f45b202a4d21818906520e666b514b32f7 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFile.Deconstruct(System.String@,System.String@) dea2c9f31c9b076d94a1c8e84e0ecdea46fd89b352288d119380b3dc73b32e17 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-55645f7086115fe6"></a>[`public Inno.Rendering.Shaders.ShaderSourceFile.ShaderSourceFile(string assetPath, string text)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Contains immutable source text supplied by the asset candidate's controlled source resolver.<br>`assetPath`：Resolved asset mount path.<br>`text`：UTF-8 decoded immutable source text. | — | — |
| <a id="api-1475a17907cd0899"></a>[`public void Inno.Rendering.Shaders.ShaderSourceFile.Deconstruct(out string assetPath, out string text)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-687e4a6f0beba0cf"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceFile.Equals(Inno.Rendering.Shaders.ShaderSourceFile? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-9f28d089523b108a"></a>[`public override bool Inno.Rendering.Shaders.ShaderSourceFile.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-47dd599abe66c665"></a>[`public override int Inno.Rendering.Shaders.ShaderSourceFile.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-8146aad7da055ec2"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceFile.operator ==(Inno.Rendering.Shaders.ShaderSourceFile? left, Inno.Rendering.Shaders.ShaderSourceFile? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-67c6052d96542cb7"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceFile.operator !=(Inno.Rendering.Shaders.ShaderSourceFile? left, Inno.Rendering.Shaders.ShaderSourceFile? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2145c5a5d1b98a74"></a>[`public override string Inno.Rendering.Shaders.ShaderSourceFile.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-6ecbb8a50f1f2bfd"></a>[`public string Inno.Rendering.Shaders.ShaderSourceFile.assetPath { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L58) | Resolved asset mount path. | — | — |
| <a id="api-0e7776db860cd242"></a>[`public string Inno.Rendering.Shaders.ShaderSourceFile.text { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L59) | UTF-8 decoded immutable source text. | — | — |
| <a id="api-128abf55f08d9050"></a>[`public sealed record Inno.Rendering.Shaders.ShaderSourceFile : System.IEquatable<Inno.Rendering.Shaders.ShaderSourceFile>`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Contains immutable source text supplied by the asset candidate's controlled source resolver.<br>`assetPath`：Resolved asset mount path.<br>`text`：UTF-8 decoded immutable source text. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceFrontendCatalog`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceFrontendCatalog e332ddc4b00c825f52f87ce0882644bfd119f7e7441773cf6bfc628cc65066ce -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.IShaderSourceFrontend}) c4ec0d39cd8f145eed1f82d94d68cd89ecb3129534825bf7e14246fab2801328 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.languageIds 062121de0262925eb1529208376bc64071a779ae87547fc5d989f697cb6a55eb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.Analyze(System.String,Inno.Rendering.Shaders.ShaderSourceRequest) 51a3761385fa105903a8687ecfccdf1d1bc2b2c5c88747a38999ba43d6f8f1a0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.AnalyzeModule(System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderSourceImplementationRequest}) 1488a62ab6e215f9d146375b7df92de859f3521aca3f77c38d9cca9420fbc01a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4299b866205f6089"></a>[`public Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.ShaderSourceFrontendCatalog(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.IShaderSourceFrontend> frontends)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L25) | Captures a validated language provider set without a process-global registry.<br>`frontends`：Providers owned and retired by the calling generation. | — | `System.ArgumentException`：Language IDs are missing or duplicated. |
| <a id="api-4ea0698074d24b77"></a>[`public Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.Analyze(string languageId, Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L56) | Analyzes source using an explicitly selected language.<br>返回：The selected frontend's analysis result.<br>`languageId`：Registered language identity, never inferred from the active graphics API.<br>`request`：Immutable source analysis request. | — | `System.NotSupportedException`：The requested language provider is unavailable. |
| <a id="api-97004348a3bf6807"></a>[`public Inno.Rendering.Shaders.ShaderSourceModuleAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.AnalyzeModule(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceImplementationRequest> implementations)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L79) | Analyzes all selected implementations and variants against one immutable provider generation.<br>返回：Frozen source inputs and a common interface, or recoverable missing/interface diagnostics.<br>`implementations`：Explicit implementation/configuration candidates; no native-code translation is attempted. | — | `System.ArgumentException`：The candidate list is empty or contains null or duplicate implementation IDs. |
| <a id="api-1efbb578c4afbac8"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.languageIds { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L37) | Gets registered language identities for source import settings. | — | — |
| <a id="api-c2acd9afe665cb35"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceFrontendCatalog`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L12) | Owns an immutable language registration snapshot for one authoring generation. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceFrontendRegistry`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry 312b3196983aa0c2fec274dbbf90c006585b1d3e1c706424bc0cdd2555410a72 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) 0fe7beb33c2dd1b8c4796c5a539fad74b4c6e2a2c3b71c96d2f139b9ed35ab4b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.languageIds b7801055c0bc7f3c7b37d85ea9305fb664b2a8c0dc8ed8046e4815469e9b4432 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Analyze(System.String,Inno.Rendering.Shaders.ShaderSourceRequest) 58943e250414e0d0e21cc9d1292bef548ca89fb7f1552125ecbf34e283c2607f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.AnalyzeModule(System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderSourceImplementationRequest}) 90ddb9439ff95b44e23a2abe65f4a459de859a9c320c26b51cceaed80dcd0dcf -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot) 140a1be9a6b0b58ad3bd8af90803bad7795fe14b3edfa80dbf6ea5d3b640f93e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderSourceFrontendCatalog) 4630d5df1f6777cb1d9d5df71a5d3b9b3b2e3ac2c66a93f0c316c53077046185 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7a749ca4d6beaedf"></a>[`public Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.ShaderSourceFrontendRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L22) | Registers this authoring owner with the shared candidate and rollback coordinator.<br>`types`：The owner type catalog; it must outlive this registry. | — | — |
| <a id="api-2a3099734367a852"></a>[`public Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Analyze(string languageId, Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L51) | Analyzes source while preventing provider retirement for the complete synchronous operation.<br>返回：A neutral interface/diagnostic result without a provider reference.<br>`languageId`：The exact registered language identity.<br>`request`：Candidate source inputs and include resolver. | — | `System.NotSupportedException`：The language is not available in the current generation. |
| <a id="api-59295634fa367260"></a>[`public Inno.Rendering.Shaders.ShaderSourceModuleAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.AnalyzeModule(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceImplementationRequest> implementations)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L71) | Freezes and validates a complete module without mixing frontend generations between implementations.<br>返回：A neutral module result containing no frontend or source resolver references.<br>`implementations`：Explicit implementation and variant candidates from the current asset transaction. | — | `System.ArgumentException`：The candidate list is empty or has duplicate implementation keys. |
| <a id="api-95a5a31b3f0f022a"></a>[`protected override Inno.Rendering.Shaders.ShaderSourceFrontendCatalog Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L86) | Builds a validated result from the current immutable input snapshot.<br>返回：The validated shader source frontend catalog that represents the completed operation.<br>`types`：The active type catalog generation used for extension resolution. | — | — |
| <a id="api-a5d09c3ad0ec21e6"></a>[`protected override void Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderSourceFrontendCatalog snapshot)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L102) | Releases the generation lease retained by an immutable registry snapshot.<br>`snapshot`：The immutable state snapshot consumed by this operation. | — | — |
| <a id="api-45753d63ae6fab10"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.languageIds { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L27) | Gets immutable language identities available in the current generation. | — | — |
| <a id="api-6772b6e2f6f25be7"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceFrontendRegistry : Inno.Extensibility.Types.TypeRegistry<Inno.Rendering.Shaders.ShaderSourceFrontendCatalog>`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L12) | Discovers source language providers through the shared type-generation transaction and owns their retirement. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceFunction`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceFunction 97d386cc29540e8fe2b825a4b583ca32f9f327c798b6502ed0681c4b22de71b8 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFunction.#ctor(System.String,Inno.Rendering.Shaders.ShaderSourceType,System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderSourceParameter},Inno.Rendering.Shaders.ShaderSourcePosition) a6a345c77e15d4caacf46229d604aa56f5fa8ab78fef549db8f671a552453c46 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFunction.name 665a84e80ac23d0fbd763ddfec6ef1d1c29ea6d4d61e18866bdfb62a6a1b4820 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFunction.returnType 42320f94866fd84ac9f0235ed8d685e42fbb6fb6dc8d40b8408fe5c2ca295401 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFunction.parameters a60bbf99365da0d7587e6c69067d1e73022bb7f02dfd15e21657fb318866c969 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceFunction.location 90d620aeee5b92d239a85c11f97cd44a0d561598a695f40eebe26ecce1d3c689 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceFunction.HasSameInterface(Inno.Rendering.Shaders.ShaderSourceFunction) 5ba9e6dc81206cf2f4856819a3abfa521bffbcd1c4ddb13981a7712c56886d75 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5128492ca62e9848"></a>[`public Inno.Rendering.Shaders.ShaderSourceFunction.ShaderSourceFunction(string name, Inno.Rendering.Shaders.ShaderSourceType returnType, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceParameter> parameters, Inno.Rendering.Shaders.ShaderSourcePosition location)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L97) | Captures a source function declaration without retaining parser or provider objects.<br>`name`：Implementation function name selected by import settings.<br>`returnType`：Canonical result type; void has no result port.<br>`parameters`：Unique named parameters in call order.<br>`location`：Original declaration location. | — | `System.ArgumentException`：The name is reserved or parameters have duplicate names. |
| <a id="api-53dc6f723fe3a81e"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceFunction.HasSameInterface(Inno.Rendering.Shaders.ShaderSourceFunction? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L142) | Validates alternative implementations by semantic names, types, direction, and call order.<br>返回：True when both implementations expose the same graph ports and calling interface.<br>`other`：Candidate interface, whose private implementation function name may differ. | — | — |
| <a id="api-0e4d3c0eacfd38ce"></a>[`public Inno.Rendering.Shaders.ShaderSourcePosition Inno.Rendering.Shaders.ShaderSourceFunction.location { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L131) | Gets the original declaration position. | — | — |
| <a id="api-70d2ede40d414584"></a>[`public string Inno.Rendering.Shaders.ShaderSourceFunction.name { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L119) | Gets the implementation function name. | — | — |
| <a id="api-4385b234e5a82b3a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceParameter> Inno.Rendering.Shaders.ShaderSourceFunction.parameters { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L127) | Gets the immutable function parameters. | — | — |
| <a id="api-e0dee88ee97864e2"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceFunction.returnType { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L123) | Gets the canonical result type. | — | — |
| <a id="api-f06ff23b3edf2df1"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceFunction`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L77) | Contains the single callable interface exported by a source asset. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis 5b9446194f830cc9146ea7b2fbc97cf49af9a60fa5d35a558baf6d093a1bff11 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.implementationId bedd302eea4ab9782611b5ab135aa11b2efffe440c41f1347cdc66abf31c4004 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.languageId 6e07e0b986e698b341dfb7bd3d75fc56e9d60399739f84aacc2fc9442dfa6ce0 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sourcePath e3ce9dd8d2a946a3dd0b210c37a3db9ccbf8101ae8710c359f3d54b093b8dcce -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.entryPoint 77138d89c8800f3c7512552080de1944d22416c02e804daf0e0d0d8ad15b414b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.defines 1708198cee6635660b35b1971fc8d071f5616a80a39494bca85474ff03201f66 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sources bea22fde0c57c06aa4aa207d979ac2b96134b77c9de0be20af5da1277e9bd54a -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.includes 285226fff98bb2510acbf0c81558ab041753325a26413d7b44e89b6c7c9ff48b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.analysis 40e43edc7f92013f26921572cc87e2adcb912c893b2670570c1a123c07908bb2 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.contentHash 788f55ccc1680228b0587f42cc128e0108b1bac9f295bc86efe352835cb7fe3c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.CreateSourceRequest 50a41030b918c1ab4f7d8067af02571987c274280dbf7a4e395d02a86c03b1a0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4a835db3867dbccc"></a>[`public Inno.Rendering.Shaders.ShaderSourceRequest Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.CreateSourceRequest()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L139) | Recreates a compiler input backed only by immutable captured sources and resolution edges.<br>返回：A request with no filesystem access or reference to the original frontend, resolver or generation. | — | — |
| <a id="api-895fa9bf2670d086"></a>[`public Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.analysis { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L126) | Gets the parsed interface and implementation-local diagnostics. | — | — |
| <a id="api-7ab9ebbf8b135065"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.contentHash { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L131) | Gets a deterministic source-input hash. A compiler cache must additionally include its toolchain, target, graph semantics and binding layout; this hash alone is not a compiled artifact key. | — | — |
| <a id="api-d93bf140cb6bccc6"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.defines { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L114) | Gets the immutable preprocessing inputs used for this implementation. | — | — |
| <a id="api-5a27b9ac1e7de143"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.entryPoint { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L110) | Gets the implementation's selected callable function name. | — | — |
| <a id="api-e70e7c4b6dfe028d"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.implementationId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L98) | Gets the stable implementation/configuration key. | — | — |
| <a id="api-3b64d60879998c02"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceInclude> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.includes { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L122) | Gets the original resolver's frozen include edges, including aliases and mounted source paths. | — | — |
| <a id="api-a4879cd2255a0f58"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.languageId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L102) | Gets the selected source language. | — | — |
| <a id="api-56d2ad596b3cfa8c"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sourcePath { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L106) | Gets the root source path in the frozen source set. | — | — |
| <a id="api-d2353174f373a5a7"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceFile> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sources { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L118) | Gets every successfully read source, including the root, ordered by path. | — | — |
| <a id="api-085de6c17e1efa8e"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L75) | Freezes a parsed implementation and its complete source inputs without retaining a frontend or resolver. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceImplementationRequest`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceImplementationRequest d084472d9cf310ebd30fa4085db5e0f9f3c221fa7b5e69e10305b163d6fbcf8c -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceImplementationRequest.#ctor(System.String,System.String,Inno.Rendering.Shaders.ShaderSourceRequest) 58259a9f1770d374035da57d45b51b96ef75944ce318e5b27a3b13e3dcb6e8cd -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationRequest.implementationId c50e5813c4778ef50df4a02fba1f130b54e42aa3022379ff8870c0741cb64a06 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationRequest.languageId 3295038c57d6de297799626c82437520a2034050495ca3385729252fddd64eb9 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceImplementationRequest.source 713d0db5012dd20997229fe3cd40587b563dbff6840426e38a7e866695d50084 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c9e66f6214466c33"></a>[`public Inno.Rendering.Shaders.ShaderSourceImplementationRequest.ShaderSourceImplementationRequest(string implementationId, string languageId, Inno.Rendering.Shaders.ShaderSourceRequest source)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L46) | Creates a candidate without inferring a language or adapter from its filename.<br>`implementationId`：Unique owner-defined key, including the target or variant when applicable.<br>`languageId`：Registered source language identity.<br>`source`：Source function and candidate-scoped dependency resolver. | — | — |
| <a id="api-960e8c12d2085510"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationRequest.implementationId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L61) | Gets the stable owner-defined implementation/configuration key. | — | — |
| <a id="api-a1ae521a67042226"></a>[`public string Inno.Rendering.Shaders.ShaderSourceImplementationRequest.languageId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L65) | Gets the explicitly selected language identity. | — | — |
| <a id="api-826a78e8042b5f5c"></a>[`public Inno.Rendering.Shaders.ShaderSourceRequest Inno.Rendering.Shaders.ShaderSourceImplementationRequest.source { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L69) | Gets the transient candidate request, which must not be persisted or retained after analysis. | — | — |
| <a id="api-29e30ce5414d18ae"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceImplementationRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L32) | Identifies one explicitly selected implementation and preprocessing configuration of a source module. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceInclude`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceInclude 096e3ed3c904ce66a6702ff74fbd994033c66dbc188a27406304f561786813a1 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.#ctor(System.String,System.String,System.String) 7e03cebf45736210c47c8879f575dde3b82efee27c8fed5c05b24bc633dbd23d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceInclude.includingFile 5d6d6ed4c6d8ff48e2647283e4584506797a80993820d26044bf4ee8f69c3989 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceInclude.include af5a3c4681b202b384a8e8a41f2bdd2498f167f326431a3efced6f68bf4f7724 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceInclude.resolvedPath 1b60f460c78d3aa32afdad56525965bc8a28461e707d594e67f4a449237ef51a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.ToString 917bf3d1e9c16b54700ff6b5d256a7020b3bc5545e7050883837039a6ba77a17 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.op_Inequality(Inno.Rendering.Shaders.ShaderSourceInclude,Inno.Rendering.Shaders.ShaderSourceInclude) 3bca0f44732562bc0516872f63eadcfe37c7d3c176553705628bbec12f9a00fc -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.op_Equality(Inno.Rendering.Shaders.ShaderSourceInclude,Inno.Rendering.Shaders.ShaderSourceInclude) b972b2af53d33f8b3e57bc109403a949d3b2b4bd5dab4e9e3d0b87920c7b58e5 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.GetHashCode 92400e87fb7075563d08acfc0e27e20cd559af7aac30ef85cf2c09af2b10362e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.Equals(System.Object) d0a80ba3174d541c34f75c49756b2cf5b54a01f697017fdea7de499da146be3f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.Equals(Inno.Rendering.Shaders.ShaderSourceInclude) 2d2f9405e2675b4fd7e58e9368e54939a278ee01e17c65e3aa10d5dc5b385d71 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceInclude.Deconstruct(System.String@,System.String@,System.String@) f4189cb1a7a1a999c37f2f5325298e10e275b1bcd8b16a029128b32f4def0c87 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a8371db80d8addd9"></a>[`public Inno.Rendering.Shaders.ShaderSourceInclude.ShaderSourceInclude(string includingFile, string include, string resolvedPath)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Records an include resolution edge so compilation never reinterprets authoring paths against live files.<br>`includingFile`：Source that requested the include.<br>`include`：Original include spelling passed to the source resolver.<br>`resolvedPath`：Exact source identity within the frozen file set. | — | — |
| <a id="api-a1dd79eb86873caf"></a>[`public void Inno.Rendering.Shaders.ShaderSourceInclude.Deconstruct(out string includingFile, out string include, out string resolvedPath)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-c43bdfbd40325290"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceInclude.Equals(Inno.Rendering.Shaders.ShaderSourceInclude? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-16461c7a99a4cc03"></a>[`public override bool Inno.Rendering.Shaders.ShaderSourceInclude.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-02e19ead386c5451"></a>[`public override int Inno.Rendering.Shaders.ShaderSourceInclude.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-a6830ef313179912"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceInclude.operator ==(Inno.Rendering.Shaders.ShaderSourceInclude? left, Inno.Rendering.Shaders.ShaderSourceInclude? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4af3d8ad367fb558"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourceInclude.operator !=(Inno.Rendering.Shaders.ShaderSourceInclude? left, Inno.Rendering.Shaders.ShaderSourceInclude? right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-255aa17e1a36c123"></a>[`public override string Inno.Rendering.Shaders.ShaderSourceInclude.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-6083820b43001bf1"></a>[`public string Inno.Rendering.Shaders.ShaderSourceInclude.include { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L25) | Original include spelling passed to the source resolver. | — | — |
| <a id="api-64fb8642c2276512"></a>[`public string Inno.Rendering.Shaders.ShaderSourceInclude.includingFile { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L24) | Source that requested the include. | — | — |
| <a id="api-63c0955194993dc3"></a>[`public string Inno.Rendering.Shaders.ShaderSourceInclude.resolvedPath { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L26) | Exact source identity within the frozen file set. | — | — |
| <a id="api-645b08a55926df12"></a>[`public sealed record Inno.Rendering.Shaders.ShaderSourceInclude : System.IEquatable<Inno.Rendering.Shaders.ShaderSourceInclude>`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Records an include resolution edge so compilation never reinterprets authoring paths against live files.<br>`includingFile`：Source that requested the include.<br>`include`：Original include spelling passed to the source resolver.<br>`resolvedPath`：Exact source identity within the frozen file set. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceModuleAnalysis`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceModuleAnalysis 82ff4e5380f9b798a9b0c7e63d064b21faf80d609d696934a8c47060d3f720f8 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.implementations f3e96ffa34c5793d55b5236b5769d814dbd92aaff4bfe3485ee93257c0056617 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.diagnostics 51c8066234ee11bd7bf96a4c8027ab3032c5e294e22919b433a8fe1e021d59af -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.function 6992e87dc2b93116d6a50d7abf2a104abfbd85a7112473fbd6e2e8b3ebcf551f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.succeeded f6869c52261ed50e3e0d477976e3a11273cb745a87ffe60b6dcf54fa00833d57 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9bcf213e17bc87e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceDiagnostic> Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L223) | Gets implementation and cross-implementation diagnostics with original source positions. | — | — |
| <a id="api-0a034b9793fe6864"></a>[`public Inno.Rendering.Shaders.ShaderSourceFunction? Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.function { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L227) | Gets the common interface only when all supplied implementations agree and are valid. | — | — |
| <a id="api-d1e77ec7be0e348f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis> Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.implementations { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L219) | Gets all implementation snapshots, including failed or unavailable implementations. | — | — |
| <a id="api-b252d38e242eb90f"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.succeeded { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L231) | Gets whether interface analysis succeeded; native compilation is a separate gate. | — | — |
| <a id="api-2fb52372a10297e6"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceModuleAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L203) | Reports whether every supplied implementation and variant has the same callable graph interface. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceNodeCompiler cada1b504f1a04580ab52dde752f99c538e3f282b86fbe8ca01070ae38e894f9 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceNodeCompiler.definitionId b22b41af10d7488e751244271a929b3b5f23c5f33e7bb41043e22ea3e4c33726 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 58c0d1b3ead3361366fc6c3e1beec48129965af8a4320034f8991bc689e745b9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) fc71a3ae726e2ec2f730461df92c6420f18fb240106c389804633e5e957f3203 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceNodeCompiler.#ctor 07b73ce4ef37b07b4e6617bb0a3e736b763c237756e5dfc891d57047030f414a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b817a4f6a3bfbddd"></a>[`public Inno.Rendering.Shaders.ShaderSourceNodeCompiler.ShaderSourceNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L232) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8d770279f36d70ec"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSourceNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L247) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-d0389d2df719781c"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSourceNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L280) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-37db8b75088ddef4"></a>[`public string Inno.Rendering.Shaders.ShaderSourceNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L237) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-3aa06cd8781854b9"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L232) | Lowers a parsed function module with name-based ports and explicit aggregate/member connection alternatives. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceNodeDefinition`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceNodeDefinition e868456dcc57c7f489593b371d561fb6c9af5c7ca89d6eff21121188cbe7eb1a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceNodeDefinition.#ctor(Inno.Rendering.Shaders.ShaderSourceFunction) 8babbc924e275f23524e9d7167697b91dc5a36c360bd02a8e808643fbfbffd54 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceNodeDefinition.function 4c9468efe7ccf2f849c700d84002ef4cc8d06922eed5be6665d1f892badcbfa2 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord) 2e58bb68a94ac4eb02a0b19afefab7082eee0f9c4ef4b8361f30cdd18232d583 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-29141535010b7788"></a>[`public Inno.Rendering.Shaders.ShaderSourceNodeDefinition.ShaderSourceNodeDefinition(Inno.Rendering.Shaders.ShaderSourceFunction function)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L22) | Creates a source node definition for the currently resolved source generation.<br>`function`：The single function derived by the source frontend. | — | — |
| <a id="api-7490896a0a80a93d"></a>[`public override System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphPortDefinition> Inno.Rendering.Shaders.ShaderSourceNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L66) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`node`：The node consumed by get ports; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-0331f6b5d4f01f37"></a>[`public Inno.Rendering.Shaders.ShaderSourceFunction Inno.Rendering.Shaders.ShaderSourceNodeDefinition.function { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L55) | Gets the immutable parsed source interface belonging to this definition. | — | — |
| <a id="api-2864034b65df4948"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceNodeDefinition : Inno.Core.Graphs.GraphNodeDefinition`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L12) | Projects one resolved source interface into immutable graph ports without duplicating authored port declarations. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceParameter`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceParameter bd935a5352eb55f57de3bd275c21da4244c5c61818f6e42f08d04c87621244ea -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceParameter.#ctor(System.String,Inno.Rendering.Shaders.ShaderSourceType,Inno.Rendering.Shaders.ShaderSourceParameterDirection) 47200cf8002ca19544010cfd9943d6eca9621b0eec042f3003a28ec56ef59407 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceParameter.name 137d4464a70559110a3f2efc69b430e99ef2e1ef4f3aed02f1780b6f31dcda5b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceParameter.type 0b9413a687203117581b2307e9ceb43af495cff1a0b0c1fab21292973fbb2828 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceParameter.direction 1f3b7a84e388e7e59558ac040c30040393199e2d742a2aa3f06128996a9c371d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1917859015a83edf"></a>[`public Inno.Rendering.Shaders.ShaderSourceParameter.ShaderSourceParameter(string name, Inno.Rendering.Shaders.ShaderSourceType type, Inno.Rendering.Shaders.ShaderSourceParameterDirection direction)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L46) | Creates a validated function parameter.<br>`name`：Public parameter name, used for semantic port matching rather than ordinal matching.<br>`type`：Canonical value type.<br>`direction`：Declared value flow. | — | `System.ArgumentException`：The parameter has void type. |
| <a id="api-8c26345344625ee2"></a>[`public Inno.Rendering.Shaders.ShaderSourceParameterDirection Inno.Rendering.Shaders.ShaderSourceParameter.direction { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L71) | Gets the declared value flow. | — | — |
| <a id="api-0bb1b0ea329fb349"></a>[`public string Inno.Rendering.Shaders.ShaderSourceParameter.name { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L63) | Gets the public parameter name. | — | — |
| <a id="api-86f17a4bd0074889"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceParameter.type { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L67) | Gets the complete canonical value type. | — | — |
| <a id="api-0d3c5d42f9960b82"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceParameter`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L29) | Describes one parameter parsed from a public source declaration. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceParameterDirection`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceParameterDirection bd835aa560ab6b268a5459f2c82a19016b93dc703c35780e5ec4cb8d4f4983a0 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderSourceParameterDirection.Input 6a62c067410a5ea036821907167553848de421d4637110d5fabc9367de6c73a9 -->
<!-- api: F:Inno.Rendering.Shaders.ShaderSourceParameterDirection.Output 5079160f8135a8adb6b6ad12139059d0aa15800f592abd0bc0800974a5778e9b -->
<!-- api: F:Inno.Rendering.Shaders.ShaderSourceParameterDirection.InputOutput 15d08a641b8bf049ec8f7b919521ba5f2040c2852f4f83b6edae2756684d5d29 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cea8c9947a0bc4de"></a>[`Inno.Rendering.Shaders.ShaderSourceParameterDirection.Input = 0`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L15) | The caller supplies a value. | — | — |
| <a id="api-75eda891823de433"></a>[`Inno.Rendering.Shaders.ShaderSourceParameterDirection.InputOutput = 2`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L23) | The caller supplies a value and the function produces its replacement. | — | — |
| <a id="api-16e9dcb5a8644bda"></a>[`Inno.Rendering.Shaders.ShaderSourceParameterDirection.Output = 1`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L19) | The function produces a value. | — | — |
| <a id="api-06226497a02f4463"></a>[`public enum Inno.Rendering.Shaders.ShaderSourceParameterDirection : int`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L10) | Declares value flow across a source function parameter. | — | — |

### `Inno.Rendering.Shaders.ShaderSourcePosition`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourcePosition 2f9795487dc79002049192791fbab8e187766740e633203f0eba4b8758b75606 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.#ctor(System.String,System.Int32,System.Int32) db7a8e246ab909fcd83aff66997917ad7d85364977bb5a2a55fc16088260869f -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourcePosition.assetPath 025f2c16ab0ac00e884a4325c3d4449c18854c93cbcffd4068bc4f20a7463c28 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourcePosition.line 84c3832bbb23b74597077a54a156af89ce12a26ba87f5387aaae176a4765f09e -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourcePosition.column 2ce9f6cfeeec4b5867786364c93ff97fadf950ce8a90c23fc974f35c9e69ddb8 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.ToString 5cee25c321d333d9045f89133df70059b299fa232c1524b92d750e1c9b4e817f -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.op_Inequality(Inno.Rendering.Shaders.ShaderSourcePosition,Inno.Rendering.Shaders.ShaderSourcePosition) 4b7111a255988c64d2b53a23ff494d0b8269948b6d2222ec371927c8234a2593 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.op_Equality(Inno.Rendering.Shaders.ShaderSourcePosition,Inno.Rendering.Shaders.ShaderSourcePosition) 71a2180d2b93c2f4948dc93952c9412023f653f99481a0fc363bb04ea7ccd830 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.GetHashCode 7640032bbf81b9b813e90325374405fc4e245de61748b36e5d5981e5b42d1a31 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.Equals(System.Object) e5b221d494d18e05da8925a0bac12de4e4455226bf1b0e45593982dab47b25d0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.Equals(Inno.Rendering.Shaders.ShaderSourcePosition) c14548b8069f47fc397d09a8971c80cb8b2a5bc27bc7c416079ab9d9ff010d2a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourcePosition.Deconstruct(System.String@,System.Int32@,System.Int32@) 92b43c73f4e9f2b841334b95fcfebd0d3b26ee3d0beaee62e2f7661d667656fc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b4edbc9f0a46edef"></a>[`public Inno.Rendering.Shaders.ShaderSourcePosition.ShaderSourcePosition(string assetPath, int line, int column)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Identifies an original source position, not a generated temporary compiler filename.<br>`assetPath`：Source path in the current asset mount.<br>`line`：One-based source line.<br>`column`：One-based source column. | — | — |
| <a id="api-bd1a1462a643e7f9"></a>[`public void Inno.Rendering.Shaders.ShaderSourcePosition.Deconstruct(out string assetPath, out int line, out int column)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-7582e84449ec1a1f"></a>[`public bool Inno.Rendering.Shaders.ShaderSourcePosition.Equals(Inno.Rendering.Shaders.ShaderSourcePosition other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-7d418c303285eec1"></a>[`public override bool Inno.Rendering.Shaders.ShaderSourcePosition.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7dbd46465a1b8254"></a>[`public override int Inno.Rendering.Shaders.ShaderSourcePosition.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8f1587baf71b6bf3"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourcePosition.operator ==(Inno.Rendering.Shaders.ShaderSourcePosition left, Inno.Rendering.Shaders.ShaderSourcePosition right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-49070013b14fae21"></a>[`public static bool Inno.Rendering.Shaders.ShaderSourcePosition.operator !=(Inno.Rendering.Shaders.ShaderSourcePosition left, Inno.Rendering.Shaders.ShaderSourcePosition right)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e86d1647614c08cb"></a>[`public override string Inno.Rendering.Shaders.ShaderSourcePosition.ToString()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6a0fa649ba86e5cf"></a>[`public string Inno.Rendering.Shaders.ShaderSourcePosition.assetPath { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L21) | Source path in the current asset mount. | — | — |
| <a id="api-7f30623a61d6e5a6"></a>[`public int Inno.Rendering.Shaders.ShaderSourcePosition.column { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L23) | One-based source column. | — | — |
| <a id="api-c688553b67e5f3c2"></a>[`public int Inno.Rendering.Shaders.ShaderSourcePosition.line { get; init; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L22) | One-based source line. | — | — |
| <a id="api-a89fe68fccd0b248"></a>[`public readonly record struct Inno.Rendering.Shaders.ShaderSourcePosition : System.IEquatable<Inno.Rendering.Shaders.ShaderSourcePosition>`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Identifies an original source position, not a generated temporary compiler filename.<br>`assetPath`：Source path in the current asset mount.<br>`line`：One-based source line.<br>`column`：One-based source column. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceRequest`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceRequest d310826fca3b8f685613cc3f3c89296a78b596967383ccb6d829dedb21343240 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceRequest.#ctor(Inno.Rendering.Shaders.ShaderSourceFile,System.String,Inno.Rendering.Shaders.IShaderSourceResolver,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) 9130d83d990c2e8af9b2caf7db778aec6b41e44b3b1a024b4508ec2979a3f437 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceRequest.source e29008a4edccb14d0b2c75c9f6889a4c819a7c1636f8f353c7cdeb435a57d41a -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceRequest.entryPoint 96b9101d21b1e88241487e1e9df8343d5034c78ca5745a10358ac791af3b6562 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceRequest.resolver 3c70c1a48283df5b4f239ddf00d1cafd351382166ea1a39e90ab62a130fb8a18 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceRequest.defines 6d0f56ec390f8db87c872269f9f4ab6fee3c5e48c84e6a207f1db007cd5a13c2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b43942c5b3f05941"></a>[`public Inno.Rendering.Shaders.ShaderSourceRequest.ShaderSourceRequest(Inno.Rendering.Shaders.ShaderSourceFile source, string entryPoint, Inno.Rendering.Shaders.IShaderSourceResolver resolver, System.Collections.Generic.IReadOnlyDictionary<string, string>? defines = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L105) | Creates an analysis request for an explicitly selected function.<br>`source`：Root source snapshot.<br>`entryPoint`：The one public function selected by source import settings.<br>`resolver`：Candidate-scoped include resolver.<br>`defines`：Target and variant preprocessing inputs, or null for no defines. | — | — |
| <a id="api-b3fa2b342e0ae701"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Shaders.ShaderSourceRequest.defines { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L135) | Gets immutable preprocessing inputs included in analysis cache identity. | — | — |
| <a id="api-f219fdcf47a93ee3"></a>[`public string Inno.Rendering.Shaders.ShaderSourceRequest.entryPoint { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L127) | Gets the explicitly selected export function name. | — | — |
| <a id="api-53d263170b50dbf5"></a>[`public Inno.Rendering.Shaders.IShaderSourceResolver Inno.Rendering.Shaders.ShaderSourceRequest.resolver { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L131) | Gets the candidate-scoped dependency resolver. | — | — |
| <a id="api-dd12395cc6c98423"></a>[`public Inno.Rendering.Shaders.ShaderSourceFile Inno.Rendering.Shaders.ShaderSourceRequest.source { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L123) | Gets the root source snapshot. | — | — |
| <a id="api-644f315c35719ee4"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L88) | Carries the immutable inputs required to execute one shader source request. | — | — |

### `Inno.Rendering.Shaders.ShaderSourceType`

<!-- api: T:Inno.Rendering.Shaders.ShaderSourceType 855e40200208bc28b4401dc7dce4c8dabb889e657da894c00ac7c1dd46fe0ecd -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceType.id c5f1f8a60cbb080d722c4602c5a68120fdddcda332a8d68b7cf1656c7af0e716 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceType.elementType 051a0c929d07d3ff6852ce4f293e328e60ae48b82d1f51e5109b6da7bf0157a3 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceType.elementCount c6530613d3769617ae5be1e7481c46f06cea8a7c48cb58c619b8a98e8aa15a3b -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceType.fields 9a9a0233cf0851a099ead9cfa74a7e3d1f761ef03ea69bfd5fd74e33377632b1 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderSourceType.storage b0c43be78d725223720a06eddbb5ef733d88ffa0aff4eaea94cb797b2d82cbb9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceType.Storage(Inno.Rendering.Shaders.ShaderStorageType) efc5092deb693e63692de2c956079fca12566e512a5472bfb828de8f836aee6e -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceType.Atomic(System.String) 074934dd856cf3c498b151a7f79f6d114209687560bc1c7ba59a261323752bdb -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceType.ArrayOf(Inno.Rendering.Shaders.ShaderSourceType,System.Int32) f1ec2455f99d7853c4100b5061249bb5bd0f699062e98d5647f005aa03287d42 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceType.Structure(System.String,System.Collections.Generic.IEnumerable{Inno.Rendering.Shaders.ShaderSourceField}) a9126f3b03afb249037cdf4422cdf6316d77a487e7ec21c559be428aa2abc852 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderSourceType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderSourceType) 253d6d4eb340b6f622d8f31f0fa296dfe2b92d0cc0ed1604a1c8265393204a34 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-56a1a29bc0493957"></a>[`public static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.ArrayOf(Inno.Rendering.Shaders.ShaderSourceType element, int count)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L87) | Creates a fixed-length array without collapsing its element type.<br>返回：An immutable array type.<br>`element`：Element type.<br>`count`：Positive fixed element count. | — | `System.ArgumentException`：The element is void or an opaque storage binding. |
| <a id="api-c7191e026a2e4b85"></a>[`public static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Atomic(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L70) | Creates an atomic type supplied by a language or graph target.<br>返回：An immutable atomic type.<br>`id`：Canonical type identity, such as float3, texture2d, or a provider-qualified opaque type. | — | — |
| <a id="api-ac79467dd2f3fb91"></a>[`public bool Inno.Rendering.Shaders.ShaderSourceType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderSourceType? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L136) | Checks both nominal identity and complete aggregate layout.<br>返回：True only when both descriptors express the same function interface.<br>`other`：Candidate type from another implementation. | — | — |
| <a id="api-52fba5c48cea5e88"></a>[`public static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Storage(Inno.Rendering.Shaders.ShaderStorageType storage)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L58) | Creates an opaque storage binding value with a complete element, format and access contract.<br>返回：An immutable storage handle type; it cannot be constructed or copied as an aggregate.<br>`storage`：Resource description, independent of any actual GPU object. | — | — |
| <a id="api-f701715fa3e0e54b"></a>[`public static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Structure(string id, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceField> fields)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L113) | Creates a nominal structure with a validated immutable field layout.<br>返回：An immutable structure type.<br>`id`：Public structure identity shared by alternative implementations.<br>`fields`：Unique named fields in declaration order. | — | `System.ArgumentException`：The identity is reserved, or fields are empty, null, or duplicated. |
| <a id="api-a798433012cd5115"></a>[`public int Inno.Rendering.Shaders.ShaderSourceType.elementCount { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L38) | Gets the fixed array length, or zero for a non-array value. | — | — |
| <a id="api-fe9c340a96f90602"></a>[`public Inno.Rendering.Shaders.ShaderSourceType? Inno.Rendering.Shaders.ShaderSourceType.elementType { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L34) | Gets the array element type, or null for a non-array value. | — | — |
| <a id="api-185b04c7c95475b6"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceField> Inno.Rendering.Shaders.ShaderSourceType.fields { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L42) | Gets immutable structure fields in declaration order. | — | — |
| <a id="api-c4b790df5a952319"></a>[`public string Inno.Rendering.Shaders.ShaderSourceType.id { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L30) | Gets the open semantic type identifier, independent of source-language spelling. | — | — |
| <a id="api-5378fc9692271b6a"></a>[`public Inno.Rendering.Shaders.ShaderStorageType? Inno.Rendering.Shaders.ShaderSourceType.storage { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L47) | Gets a typed storage binding contract, or null for ordinary values and sampled textures. | — | — |
| <a id="api-8dca3fa895b48b68"></a>[`public sealed class Inno.Rendering.Shaders.ShaderSourceType`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L10) | Describes a language-independent function value, including aggregate members and fixed arrays. | — | — |

### `Inno.Rendering.Shaders.ShaderStageInputNodeCompiler`

<!-- api: T:Inno.Rendering.Shaders.ShaderStageInputNodeCompiler 50f48e6b10b48e0510178bddd24f0c38073eb72229048801a7fa0e02aa51f640 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.definitionId 68c3e292377c79893716a4b4aa5407e103f0de172e8005a4dca576f54eb41de1 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext) 952a341714ba4e6b84b06b5578015d47751a0cfe7c2a94f2c620635eab8be1e6 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext) 7d9d075f9451659bd151eeae3a085b21059507db566589392228c96866816bc9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.#ctor 8d596de513b11b9fa6d6e2f580941b4c129a6ad8c398482381d4e963b9ddd9e5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bfa0080615a901c5"></a>[`public Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.ShaderStageInputNodeCompiler()`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L157) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-a215f2a14d568bc5"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L172) | Gets a ports required by the implemented contract.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-08f90902d3871f89"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L183) | Lowers this graph node to typed shader IR after validating its inputs.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-4d3c3d2377fe6a70"></a>[`public string Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.definitionId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L162) | Gets the definition id text used by the current instance. | — | — |
| <a id="api-3742b74fe5f3d0f9"></a>[`public sealed class Inno.Rendering.Shaders.ShaderStageInputNodeCompiler : Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L157) | Reads one target-assigned stage/resource input without choosing a native variable name. | — | — |

### `Inno.Rendering.Shaders.ShaderStorageType`

<!-- api: T:Inno.Rendering.Shaders.ShaderStorageType 2590b619403790a315e1758a735eaaf8911c7f0a4a5b98dc810279be1000956d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.valueType 560a03c9a0b219184b5c84b67eb8565bbcf54143ec5f7285410d1843e4091ff7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.access f0643a8580229fde67c33794c3287f1308ded7c4a251105f946b6c5b0fcf5fa7 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.format db3a8bf5308f830ccf271ba174addd1421a2d1d980834df1ad50129d8dc6b124 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.dimension e7c678c73fde4c91ef6b703c2e7ee3e1252c593d1c33ccba243a9c008d93b873 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.array d2f385596a332fe969f53d1beb8a4b5aa9f6c380544c7e998d91ba79bbfce506 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderStorageType.isImage 07712428fa49a82eee93f2696bdb7496166643ecd598a8c6958055fc793c9685 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStorageType.Buffer(Inno.Rendering.Shaders.ShaderSourceType,Inno.Rendering.RenderStorageAccess) 76dcf55d8859d55b506fc0c9165bf91cfbb5a47d6b18bd5cd61a6347e52a122b -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStorageType.Image(Inno.Rendering.RenderTextureFormat,Inno.Rendering.RenderStorageAccess,Inno.Rendering.RenderTextureDimension,System.Boolean) 5856c31a0971d16b467b9ae96f2148119c01e00ca1ec578040171a34ea1c8efe -->
<!-- api: M:Inno.Rendering.Shaders.ShaderStorageType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderStorageType) b31f683a19b97c5aa465d886885991aeb119a5f0809d009dc523d265197fcf47 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cb47966e53277588"></a>[`public static Inno.Rendering.Shaders.ShaderStorageType Inno.Rendering.Shaders.ShaderStorageType.Buffer(Inno.Rendering.Shaders.ShaderSourceType element, Inno.Rendering.RenderStorageAccess access)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L67) | Creates a structured storage buffer with an explicit element layout.<br>返回：An immutable storage buffer description.<br>`element`：Non-void value layout; resources cannot be embedded in buffer elements.<br>`access`：Permitted memory operations. | — | `System.ArgumentException`：The element contains void or resource handles. |
| <a id="api-07b1f24a94d6700e"></a>[`public static Inno.Rendering.Shaders.ShaderStorageType Inno.Rendering.Shaders.ShaderStorageType.Image(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderStorageAccess access, Inno.Rendering.RenderTextureDimension dimension = Inno.Rendering.RenderTextureDimension.Texture2D, bool array = false)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L97) | Creates a formatted storage image; target capabilities must separately support its access and format.<br>返回：An immutable image description with float4 load/store values.<br>`format`：Linear, non-depth image format supported by the rendering contract.<br>`access`：Permitted memory operations.<br>`dimension`：Two-dimensional or three-dimensional image shape.<br>`array`：Whether a two-dimensional image is an array. | — | `System.ArgumentException`：The format or shape cannot represent storage image operations. |
| <a id="api-6c6141b0de4a5ad0"></a>[`public bool Inno.Rendering.Shaders.ShaderStorageType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderStorageType? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L119) | Compares complete access, format, shape and element layout contracts.<br>返回：True only when both bindings permit exactly the same operations and values.<br>`other`：Candidate resource description. | — | — |
| <a id="api-d8e3cbec152d42e1"></a>[`public Inno.Rendering.RenderStorageAccess Inno.Rendering.Shaders.ShaderStorageType.access { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L34) | Gets the permitted memory access, independent of Render Graph scheduling. | — | — |
| <a id="api-f3ce6febda13908b"></a>[`public bool Inno.Rendering.Shaders.ShaderStorageType.array { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L46) | Gets whether a two-dimensional storage image has array layers. | — | — |
| <a id="api-1ee1874908d9c06f"></a>[`public Inno.Rendering.RenderTextureDimension Inno.Rendering.Shaders.ShaderStorageType.dimension { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L42) | Gets the image dimension; ignored for buffers. | — | — |
| <a id="api-f4b4bf62ff50c952"></a>[`public Inno.Rendering.RenderTextureFormat? Inno.Rendering.Shaders.ShaderStorageType.format { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L38) | Gets the exact storage image format, or null for a structured buffer. | — | — |
| <a id="api-7d36024aba2d756d"></a>[`public bool Inno.Rendering.Shaders.ShaderStorageType.isImage { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L50) | Gets whether this descriptor denotes an image rather than a structured buffer. | — | — |
| <a id="api-fa232ea1bcafd43c"></a>[`public Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderStorageType.valueType { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L30) | Gets the buffer element or image load/store value type; image operations use float4. | — | — |
| <a id="api-9ea6b97956b3a422"></a>[`public sealed class Inno.Rendering.Shaders.ShaderStorageType`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L9) | Describes typed storage without embedding a backend register, declaration or resource handle. | — | — |

### `Inno.Rendering.Shaders.ShaderTarget`

<!-- api: T:Inno.Rendering.Shaders.ShaderTarget d3f38b980bebcc9842f879ba958c4eb4669d0c8ab439fee36cfdd275bd6d7b99 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTarget.Expand(Inno.Rendering.Shaders.ShaderTargetContext,System.Threading.CancellationToken) a43cf39e3b0f0e39ed3e6725760c9dd966c1e2689a27d6adb9b02ff9b3ed7df9 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTarget.#ctor 38c59b77bc5e34638c792d38aec70030a1d5ff49374db9616f6ad2956d957cac -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-830d39c18be06b89"></a>[`protected Inno.Rendering.Shaders.ShaderTarget.ShaderTarget()`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L34) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-0911ebb221bcaf22"></a>[`public abstract Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTarget.Expand(Inno.Rendering.Shaders.ShaderTargetContext context, System.Threading.CancellationToken cancellationToken)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L48) | Builds explicit stage interfaces, resource declarations, techniques and pass states.<br>返回：A detached graph for the common typed compiler; all required capabilities belong in its pass contracts.<br>`context`：Invocation-scoped detached document and complete owner serialization services.<br>`cancellationToken`：Cancellation between expansion operations. | — | — |
| <a id="api-c83ace802694dada"></a>[`public abstract class Inno.Rendering.Shaders.ShaderTarget`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L34) | Expands a domain's surface contract into ordinary graph stages before source dependency capture. | — | — |

### `Inno.Rendering.Shaders.ShaderTargetAttribute`

<!-- api: T:Inno.Rendering.Shaders.ShaderTargetAttribute 673f53b0e8947bb11302e6088f1e5c9dc6de28b5cb921be75301d498b381ca82 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTargetAttribute.#ctor(System.String) 15af4849e59d95369459d8298edd6a458fec3efd12b2feec8f4aa62b4b607cef -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetAttribute.id b77c424e763747617842e5509f01d65a18a50da700682d6183c9402146dd910b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8b5d43fc19b2c1cf"></a>[`public Inno.Rendering.Shaders.ShaderTargetAttribute.ShaderTargetAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L20) | Creates target discovery metadata.<br>`id`：Stable target identity persisted by Shader assets. | — | — |
| <a id="api-1f26752b9d6c71a0"></a>[`public string Inno.Rendering.Shaders.ShaderTargetAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L28) | Gets the stable target identity persisted by Shader assets. | — | — |
| <a id="api-858fa5689a204637"></a>[`public sealed class Inno.Rendering.Shaders.ShaderTargetAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L12) | Declares the immutable identity used to select a Shader Target from an authored graph. | — | — |

### `Inno.Rendering.Shaders.ShaderTargetContext`

<!-- api: T:Inno.Rendering.Shaders.ShaderTargetContext 738d97052f1c3d1978bbb6497f76610bea58228c5947172eafb7bd679f745907 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetContext.document ef46b512c722680a5cf1dd8b9332820dbcd30a48f1d2a6d8728874aabadbe63d -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetContext.serialization bdc2847f8e0c8878aeab135ccf07d9d401e1968f27ae8c0f8333e10f88c6ee85 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetContext.references 891006c8256cac109dfec52730f115619405b31b985b68d9ce0bdebcdf23430d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3a0ef6831521ae09"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTargetContext.document { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L72) | Gets a private document copy; expansion cannot modify the authored source. | — | — |
| <a id="api-992df8f098e57ba5"></a>[`public Inno.Core.Serialization.SerializationContext Inno.Rendering.Shaders.ShaderTargetContext.references { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L80) | Gets the complete owner reference context used for generated resource defaults. | — | — |
| <a id="api-7aa686411ce5983a"></a>[`public Inno.Core.Serialization.SerializationRegistry Inno.Rendering.Shaders.ShaderTargetContext.serialization { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L76) | Gets the current owner converter registry, valid only during expansion. | — | — |
| <a id="api-aeb8f4dd6186810d"></a>[`public sealed class Inno.Rendering.Shaders.ShaderTargetContext`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L57) | Supplies neutral authoring data to a domain target without exposing an adapter or live asset. | — | — |

### `Inno.Rendering.Shaders.ShaderTargetRegistry`

<!-- api: T:Inno.Rendering.Shaders.ShaderTargetRegistry 7ab758a2f4e97d944e86563180785b5932283b1ec4892ea6ae19552776382b1a -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTargetRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) 858a61192d2b46d9c82440da36523fea485da172c33620ecd58aaea086ef2954 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetRegistry.ids 0f39925b559ff67ef9d3fd5a9aeb619365c751210e053b04cd4fe88220133431 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTargetRegistry.Expand(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,System.Threading.CancellationToken) fb780106de38dfcc97dab6fd50c475fbdb4c4ca79308b506708147a183610cd2 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTargetRegistry.Dispose 292485bc50bb8df95bf7ff1d4446ca8d3e4d4941d0e6c22171e5afb99ae20788 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a0d71c26d4648cef"></a>[`public Inno.Rendering.Shaders.ShaderTargetRegistry.ShaderTargetRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L26) | Creates a target owner participating in shared candidate publication and retirement.<br>`types`：The authoring catalog, which must outlive this registry. | — | — |
| <a id="api-dcd023eddce14ea8"></a>[`public void Inno.Rendering.Shaders.ShaderTargetRegistry.Dispose()`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L93) | Retires the target snapshot through the shared registry lifecycle. | — | — |
| <a id="api-37453efeae37812e"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTargetRegistry.Expand(Inno.Core.Graphs.GraphDocument document, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L68) | Expands an assigned target, or copies an explicitly authored low-level graph with no domain target.<br>返回：Detached explicit stage graph consumed by the existing compiler.<br>`document`：Authored source, never modified by this operation.<br>`serialization`：Current owner converters.<br>`context`：Complete owner reference context.<br>`cancellationToken`：Cancellation before and during extension invocation. | — | `Inno.Rendering.Shaders.ShaderTargetUnavailableException`：The assigned target is absent from this generation.<br>`System.InvalidOperationException`：The target returns an invalid result. |
| <a id="api-c8dad68d0229b4be"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderTargetRegistry.ids { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L35) | Gets detached stable target identities available in the current generation. | — | — |
| <a id="api-e02f4de7b4225f6f"></a>[`public sealed class Inno.Rendering.Shaders.ShaderTargetRegistry : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L15) | Resolves domain targets under the same generation lease as their graph expansion. | — | — |

### `Inno.Rendering.Shaders.ShaderTargetUnavailableException`

<!-- api: T:Inno.Rendering.Shaders.ShaderTargetUnavailableException d02509f4194f5b59bd0c752f9ffa8c84c280a7f69d8ab6b8d3d64f6d496fe5d0 -->
<!-- api: M:Inno.Rendering.Shaders.ShaderTargetUnavailableException.#ctor(System.String) 54dae01bb93df6cb28baeaafaa6a9f53d06c82df65b88e23ebfb6f79f116f952 -->
<!-- api: P:Inno.Rendering.Shaders.ShaderTargetUnavailableException.targetId 90a0ba39cbfa2e787dfb48bb2f55000a9de88cd509108c25ed61fd89020a1371 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c3611e667a80eeb3"></a>[`public Inno.Rendering.Shaders.ShaderTargetUnavailableException.ShaderTargetUnavailableException(string targetId)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L16) | Creates a missing-target diagnostic without retaining extension objects.<br>`targetId`：The stable authored target identity. | — | — |
| <a id="api-5c29448561623d12"></a>[`public string Inno.Rendering.Shaders.ShaderTargetUnavailableException.targetId { get; }`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L26) | Gets the stable target identity required by the authored graph. | — | — |
| <a id="api-189c293bca127c72"></a>[`public sealed class Inno.Rendering.Shaders.ShaderTargetUnavailableException : System.InvalidOperationException`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L8) | Identifies an authored target that is absent from the current extension generation. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Core.Graphs](../core/Inno.Core.Graphs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
