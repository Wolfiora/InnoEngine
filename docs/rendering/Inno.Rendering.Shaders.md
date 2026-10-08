# Inno.Rendering.Shaders

[Rendering 索引](README.md) · [Wiki 首页](../README.md) · [BGFX 工具链](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)

## 职责与当前状态

内置 Shader 创作层。目前已经实现语言中立的源码函数接口、多实现分析快照、函数端口推导、图节点降低、typed region 与 stage。
依赖通用 Graph、Serialization、Diagnostics、Execution、TypeRegistry 和 Rendering 的 GPU 阶段契约；不直接引用
BGFX、Rendering.Assets、Scene、Editor 或 2D 插件。
它不创建 GPU 对象，Rendering 运行时也不反向引用它。

`.ishader` 图 importer、函数源码模块和 typed program 已进入实际资产编译链，Rendering2D 与 ImGui 图产物已在 Metal 启动中使用。
[Shader Editor](../editor/Inno.Editor.Panel.ShaderEditor.md)、Target/Template 扩展、安全帧原子发布和当前资产替换均已接入。历史阶段与硬件验收边界以[阶段验收](../issues/2026-09-13-shader-authoring-validation.md)为准；本轮函数库与创作体验收口见[实施记录](../issues/2026-09-14-shader-editor-authoring-experience.md)。

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
Stage/Pass 结构时，应优先使用 Template 创建显式 `.ishader`，例如 Rendering2D 的 Sprite Shader。

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
using Inno.Build.Toolchains.Bgfx.Tools;

static ShaderSourceAnalysis Inspect(IShaderSourceResolver candidateSources)
{
    var frontends = new ShaderSourceFrontendCatalog([new BgfxShaderSourceFrontend()]);
    var source = new ShaderSourceFile("project::Functions/Tint.ishadersource",
        "vec4 Tint(vec4 color, float strength) { return color * strength; }");
    return frontends.Analyze("inno.shader-language.bgfx-sc",
        new ShaderSourceRequest(source, "Tint", candidateSources));
}
```

BGFX 类只出现在工具链组合处。其他前端实现 `IShaderSourceFrontend` 并用不同稳定语言 ID 注册，
不需要向公共编译器添加语言分支。

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
- 旧完整 SC 字符串创作入口已被统一图链取代；本轮还需完成最终内部 Shader 解耦和产物清理验收，详见[实施记录](../issues/2026-09-12-shader-authoring-execution.md)。

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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Rendering.Shaders.IShaderNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.IShaderNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L35) | Defines one shader node's typed ports and lowering; drawing belongs to a separate editor extension. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.IShaderNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L60) | Lowers the node into the supplied typed builder without generating source strings. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.IShaderNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L50) | Describes ports for the current neutral properties and resolved source/target inputs. |
| [`string Inno.Rendering.Shaders.IShaderNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L40) | Gets the exact graph node definition identity implemented by this compiler. |

### `Inno.Rendering.Shaders.IShaderSourceFrontend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.IShaderSourceFrontend`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L187) | Parses one source language into canonical function interfaces; it does not create GPU objects. |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.IShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L202) | Analyzes an immutable source candidate with explicit preprocessing inputs. |
| [`string Inno.Rendering.Shaders.IShaderSourceFrontend.languageId`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L192) | Gets the open source language identity, distinct from a rendering backend or GPU API. |

### `Inno.Rendering.Shaders.IShaderSourceResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.IShaderSourceResolver`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L65) | Reads dependencies only from the current source candidate and records their import dependencies. |
| [`Inno.Rendering.Shaders.ShaderSourceFile Inno.Rendering.Shaders.IShaderSourceResolver.ReadInclude(string includingFile, string include)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L79) | Resolves and reads an include without bypassing asset mount permissions. |

### `Inno.Rendering.Shaders.ShaderBinaryNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderBinaryNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L55) | Lowers an explicit arithmetic/comparison operation; operation IDs are node configuration, not backend code. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L86) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L70) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderBinaryNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L60) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderConstantNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderConstantNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L11) | Lowers an exact scalar constant; its type/value are native graph properties. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderConstantNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L37) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderConstantNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L26) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderConstantNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L16) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderConstructNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderConstructNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L103) | Constructs a vector or column-major matrix from individually connected scalar components. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderConstructNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L133) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderConstructNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L118) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderConstructNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L108) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderExtractNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderExtractNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L10) | Extracts one statically selected component from a vector or matrix. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderExtractNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L41) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderExtractNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L25) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderExtractNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L15) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderGraphBindings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphBindings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L13) | Keeps graph input edits and the material-visible parameter contract in one neutral document change. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphBindings.ChangeInput(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Graphs.GraphNodeId nodeId, Inno.Rendering.Shaders.ShaderGraphInputSettings settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L135) | Creates a detached candidate with the input and its material/resource declaration updated together. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphBindings.RemoveNodes(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphBindings.cs#L36) | Removes selected nodes, stage-owned nodes and declarations which lose their last graph owner. |

### `Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L601) | Describes graph-authored node references before they are expanded or consumed by a Target. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L627) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L616) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderGraphCallNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L606) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderGraphClipboard`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphClipboard`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L13) | Copies and pastes shader structures as detached atomic candidates, including their parameter and Pass contracts. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphClipboard.Copy(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L33) | Captures selected nodes and the owned contents of selected stage outputs. |
| [`static Inno.Rendering.Shaders.ShaderGraphPasteResult Inno.Rendering.Shaders.ShaderGraphClipboard.Paste(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Graphs.GraphDocument fragment, bool preserveExternalStageReferences, Inno.Core.Graphs.GraphNodeId? activeStage, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L73) | Creates a detached paste candidate with new node identities and remapped program, Pass and Technique references. |

### `Inno.Rendering.Shaders.ShaderGraphDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphDiagnostic`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L90) | Identifies a graph compilation problem without retaining a node or compiler instance. |

### `Inno.Rendering.Shaders.ShaderGraphDocument`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphDocument`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L11) | Defines the native graph document protocol shared by shader import, templates and the editor. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.definitionKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L16) | Identifies the graph metadata containing the source-free material and pass contract. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.inputDefaultPrefix`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L86) | Prefixes stable port IDs storing explicit typed defaults for unconnected inputs. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.outputDefinitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L74) | Identifies a stage output node; its incoming edges name the stage's GPU outputs. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.settingsKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L82) | Identifies the structured settings stored on a stage output node. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.stageKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L78) | Identifies a node property containing its owning stage output node identity. |
| [`const string Inno.Rendering.Shaders.ShaderGraphDocument.targetKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L20) | Identifies the optional domain target; absent means explicitly authored generic stages. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphDocument.Create(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L103) | Creates an empty graph with a source-free shader contract; incomplete graphs remain serializable. |
| [`static Inno.Core.Graphs.GraphSerializedValue Inno.Rendering.Shaders.ShaderGraphDocument.Encode<T>(T value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L159) | Encodes a node or document value through the common native serialization channel. |
| [`static Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Shaders.ShaderGraphDocument.ReadDefinition(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L130) | Reads the material and pass contract without evaluating or altering the graph. |
| [`static T Inno.Rendering.Shaders.ShaderGraphDocument.Decode<T>(Inno.Core.Graphs.GraphSerializedValue value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L187) | Decodes a graph value against the current owner generation. |
| [`static T Inno.Rendering.Shaders.ShaderGraphDocument.Read<T>(Inno.Core.Graphs.GraphNodeRecord node, string key, T defaultValue, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L222) | Reads a named node value; absent values use the declared default, corrupt values never do. |
| [`static string Inno.Rendering.Shaders.ShaderGraphDocument.ReadTarget(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L37) | Reads the stable domain target assignment without resolving extension instances. |
| [`static void Inno.Rendering.Shaders.ShaderGraphDocument.SetTarget(Inno.Core.Graphs.GraphDocument graph, string targetId, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L59) | Assigns a domain target by stable identity without retaining its current provider. |

### `Inno.Rendering.Shaders.ShaderGraphInputSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphInputSettings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L285) | Stores a stage input node's logical interface without native names or GPU handles. |
| [`Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphInputSettings.type`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L294) | Gets or sets the complete neutral type descriptor. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind Inno.Rendering.Shaders.ShaderGraphInputSettings.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L298) | Gets or sets the stage input category. |
| [`Inno.Rendering.Shaders.ShaderIrStageInput Inno.Rendering.Shaders.ShaderGraphInputSettings.CreateBinding()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L314) | Validates the persisted descriptor and freezes its stage binding. |
| [`int Inno.Rendering.Shaders.ShaderGraphInputSettings.location`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L306) | Gets or sets the interface index. |
| [`string Inno.Rendering.Shaders.ShaderGraphInputSettings.id`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L290) | Gets or sets the stable logical binding identity. |
| [`string Inno.Rendering.Shaders.ShaderGraphInputSettings.semantic`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L302) | Gets or sets the target semantic, not a native expression. |

### `Inno.Rendering.Shaders.ShaderGraphLiteral`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphLiteral`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L10) | Stores an exact typed value for an unconnected node input, independently of source-language syntax. |
| [`Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphLiteral.type`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L15) | Gets or sets the complete value type, including named aggregates and fixed arrays. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderGraphLiteral.Emit(Inno.Rendering.Shaders.ShaderIrBuilder builder, Inno.Rendering.Shaders.ShaderSourceType expectedType)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L76) | Emits ordinary typed constants and aggregate construction into the common IR. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderGraphLiteral.GetScalarTypes()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L47) | Gets the exact scalar type of each stored component for shared Inspector value controls. |
| [`static Inno.Rendering.Shaders.ShaderGraphLiteral Inno.Rendering.Shaders.ShaderGraphLiteral.Zero(Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L33) | Creates an explicit zero value for a supported scalar, vector, matrix, structure or array. |
| [`uint[] Inno.Rendering.Shaders.ShaderGraphLiteral.scalarBits`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphLiteral.cs#L19) | Gets or sets scalar bit patterns in declaration order; matrices use column-major order. |

### `Inno.Rendering.Shaders.ShaderGraphLoweringRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphLoweringRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L13) | Freezes one target-selected graph region and its externally resolved, neutral compilation inputs. |
| [`Inno.Rendering.Shaders.ShaderGraphLoweringRequest.ShaderGraphLoweringRequest(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphEndpoint> outputs, string implementationId, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis>? sourceModules = null, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderIrStageInput>? stageInputs = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L35) | Captures a region without retaining an asset object, source resolver, node provider or UI selection. |
| [`System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderIrStageInput> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.stageInputs`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L69) | Gets target-assigned input descriptors; they contain no native expressions. |
| [`System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.sourceModules`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L65) | Gets frozen source modules, including unavailable/failed modules for accurate diagnostics. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphEndpoint> Inno.Rendering.Shaders.ShaderGraphLoweringRequest.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L57) | Gets named region outputs by stable graph endpoint. |
| [`string Inno.Rendering.Shaders.ShaderGraphLoweringRequest.implementationId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L61) | Gets the exact implementation key selected by the target. |

### `Inno.Rendering.Shaders.ShaderGraphLoweringResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphLoweringResult`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L101) | Contains a detached typed region only after all graph, port and node lowering validation succeeds. |
| [`Inno.Rendering.Shaders.ShaderIrBlock? Inno.Rendering.Shaders.ShaderGraphLoweringResult.block`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L113) | Gets the immutable typed region, or null on failure. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphDiagnostic> Inno.Rendering.Shaders.ShaderGraphLoweringResult.diagnostics`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L117) | Gets located graph diagnostics, including missing definitions and stale source ports. |
| [`bool Inno.Rendering.Shaders.ShaderGraphLoweringResult.succeeded`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderGraphLoweringRequest.cs#L121) | Gets whether lowering produced a complete region without errors; native compilation is a separate gate. |

### `Inno.Rendering.Shaders.ShaderGraphNodeEffect`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeEffect`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L28) | Declares whether a graph-authored function is pure or intentionally emits ordered effects. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeEffect.Pure`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L33) | The function only computes returned values and therefore requires at least one output. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeEffect.SideEffect`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L37) | The function may contain ordered GPU effects and can intentionally expose no returned values. |

### `Inno.Rendering.Shaders.ShaderGraphNodeInputSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeInputSettings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L96) | Stores values supplied by callers to a graph-authored node. |
| [`Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInputSettings.ports`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L101) | Gets or sets values supplied by callers and exposed as outputs inside the node graph. |

### `Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L634) | Describes the multi-port external inputs while editing a graph-authored node asset. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L661) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L649) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeInputsCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L639) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderGraphNodeInterface`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeEffect Inno.Rendering.Shaders.ShaderGraphNodeInterface.effect`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L139) | Gets or sets whether an inline function is pure or intentionally emits ordered effects. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeInterface`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L118) | Freezes the complete public interface resolved from one graph-authored node asset. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeKind Inno.Rendering.Shaders.ShaderGraphNodeInterface.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L135) | Gets or sets whether the graph is inline computation or a Target-owned output boundary. |
| [`Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInterface.inputs`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L147) | Gets or sets externally supplied inputs. |
| [`Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeInterface.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L151) | Gets or sets externally visible results. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeInterface.GetPorts()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L159) | Gets detached typed ports in deterministic input-then-output order. |
| [`int Inno.Rendering.Shaders.ShaderGraphNodeInterface.createOrder`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L131) | Gets or sets the deterministic order within the creation catalog. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeInterface.createPath`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L127) | Gets or sets the slash-separated creation catalog beneath Graph Nodes. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeInterface.displayName`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L123) | Gets or sets the node title displayed to authors. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeInterface.role`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L143) | Gets or sets the Target-owned role for a domain output. |

### `Inno.Rendering.Shaders.ShaderGraphNodeKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeKind`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L13) | Chooses whether a graph-authored node is inlined or consumed by a domain Target. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeKind.DomainOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L22) | Leaves the node as a typed domain boundary for the selected Shader Target. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeKind.Function`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L18) | Inlines the node graph into the caller before Target expansion and typed lowering. |

### `Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L107) | Stores values returned by a graph-authored function node. |
| [`Inno.Rendering.Shaders.ShaderGraphNodePortDefinition[] Inno.Rendering.Shaders.ShaderGraphNodeOutputSettings.ports`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L112) | Gets or sets values collected inside the node graph and exposed as outputs to callers. |

### `Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L668) | Describes the multi-port returned values while editing a graph-authored function node asset. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L695) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L683) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeOutputsCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L673) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderGraphNodePortDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodePortDefinition`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L74) | Declares one stable, typed port on a graph-authored node interface. |
| [`Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.type`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L83) | Gets or sets the complete backend-neutral value type. |
| [`bool Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.required`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L87) | Gets or sets whether callers must connect the input instead of using an explicit/default zero. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodePortDefinition.id`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L79) | Gets or sets the stable node-local port identity. |

### `Inno.Rendering.Shaders.ShaderGraphNodeSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodeEffect Inno.Rendering.Shaders.ShaderGraphNodeSettings.effect`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L64) | Gets or sets the observable computation behavior of an inline function. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeKind Inno.Rendering.Shaders.ShaderGraphNodeSettings.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L60) | Gets or sets how a reference to this graph participates in compilation. |
| [`Inno.Rendering.Shaders.ShaderGraphNodeSettings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L43) | Stores graph-level identity and catalog metadata for a reusable Shader node. |
| [`int Inno.Rendering.Shaders.ShaderGraphNodeSettings.createOrder`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L56) | Gets or sets the deterministic order within the creation catalog. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeSettings.createPath`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L52) | Gets or sets the slash-separated creation catalog beneath Graph Nodes. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeSettings.displayName`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L48) | Gets or sets the node title displayed to authors. |
| [`string Inno.Rendering.Shaders.ShaderGraphNodeSettings.role`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L68) | Gets or sets the domain role consumed by a Target; empty for ordinary inline functions. |

### `Inno.Rendering.Shaders.ShaderGraphNodes`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphNodes`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L168) | Reads and expands reusable node graphs without retaining assets or provider instances. |
| [`const string Inno.Rendering.Shaders.ShaderGraphNodes.callDefinitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L181) | Identifies a reference to another Shader graph used as a node. |
| [`const string Inno.Rendering.Shaders.ShaderGraphNodes.inputDefinitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L173) | Identifies the single multi-port node-input interface record. |
| [`const string Inno.Rendering.Shaders.ShaderGraphNodes.interfaceKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L185) | Identifies the serialized interface snapshot retained by a graph-node reference. |
| [`const string Inno.Rendering.Shaders.ShaderGraphNodes.outputDefinitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L177) | Identifies the optional multi-port node-output interface record. |
| [`const string Inno.Rendering.Shaders.ShaderGraphNodes.settingsKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L189) | Identifies graph-level reusable-node metadata, independent of either interface direction. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphNodes.Expand(Inno.Core.Graphs.GraphDocument graph, System.Func<System.Guid, string, Inno.Core.Graphs.GraphDocument> resolve, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L359) | Expands every inline graph-node reference and refreshes domain-output interfaces. |
| [`static Inno.Rendering.Shaders.ShaderGraphNodeInterface Inno.Rendering.Shaders.ShaderGraphNodes.ReadCallInterface(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L330) | Reads the current interface snapshot stored on a graph-node reference. |
| [`static Inno.Rendering.Shaders.ShaderGraphNodeInterface Inno.Rendering.Shaders.ShaderGraphNodes.ReadInterface(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L273) | Reads and validates the public node interface declared by a Shader graph. |
| [`static Inno.Rendering.Shaders.ShaderGraphNodeSettings Inno.Rendering.Shaders.ShaderGraphNodes.ReadSettings(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L247) | Reads required graph-level reusable-node metadata. |
| [`static bool Inno.Rendering.Shaders.ShaderGraphNodes.IsNodeGraph(Inno.Core.Graphs.GraphDocument graph)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L200) | Determines whether a Shader graph declares a reusable node interface. |
| [`static void Inno.Rendering.Shaders.ShaderGraphNodes.WriteSettings(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.Shaders.ShaderGraphNodeSettings settings, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphNodes.cs#L221) | Writes graph-level reusable-node metadata without coupling it to an input or output record. |

### `Inno.Rendering.Shaders.ShaderGraphOutput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L262) | Stores one stable output port's destination, independent of adapter source syntax. |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind Inno.Rendering.Shaders.ShaderGraphOutput.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L271) | Gets or sets the GPU destination category. |
| [`int Inno.Rendering.Shaders.ShaderGraphOutput.location`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L279) | Gets or sets the varying or attachment index. |
| [`string Inno.Rendering.Shaders.ShaderGraphOutput.id`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L267) | Gets or sets the stable input port identity on the stage output node. |
| [`string Inno.Rendering.Shaders.ShaderGraphOutput.semantic`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L275) | Gets or sets the varying semantic; empty for fixed position, color or depth outputs. |

### `Inno.Rendering.Shaders.ShaderGraphPass`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphPass`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L11) | Freezes the graph-lowered stages of one material-selectable GPU pass. |
| [`Inno.Rendering.Shaders.ShaderGraphPass.ShaderGraphPass(string name, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStage> stages)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L22) | Captures a pass's ordered typed stages. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStage> Inno.Rendering.Shaders.ShaderGraphPass.stages`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L38) | Gets immutable graph-lowered stages. |
| [`string Inno.Rendering.Shaders.ShaderGraphPass.name`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L34) | Gets the stable pass identity. |

### `Inno.Rendering.Shaders.ShaderGraphPassProgram`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphPassProgram`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L13) | References shared stage output identities from a material-selectable pass. |
| [`string Inno.Rendering.Shaders.ShaderGraphPassProgram.pass`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L18) | Gets or sets the exact pass identity in the runtime shader definition. |
| [`string[] Inno.Rendering.Shaders.ShaderGraphPassProgram.stages`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L22) | Gets or sets stable stage output node identities; computation remains owned by those nodes. |

### `Inno.Rendering.Shaders.ShaderGraphPasteResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPasteResult.document`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L182) | Gets the complete detached candidate; the caller owns its subsequent edits. |
| [`Inno.Rendering.Shaders.ShaderGraphPasteResult`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L170) | Contains a detached paste candidate and its newly allocated node identities. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Rendering.Shaders.ShaderGraphPasteResult.insertedNodes`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphClipboard.cs#L186) | Gets new identities suitable for selecting the pasted nodes. |

### `Inno.Rendering.Shaders.ShaderGraphProgramCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphProgramCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L18) | Partitions explicit GPU-stage regions and lowers them through registered node compilers. |
| [`Inno.Rendering.Shaders.ShaderGraphProgramCompiler.ShaderGraphProgramCompiler(Inno.Rendering.Shaders.ShaderNodeCompilerRegistry nodes)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L28) | Uses the shared node compiler generation rather than retaining individual providers. |
| [`Inno.Rendering.Shaders.ShaderGraphProgramResult Inno.Rendering.Shaders.ShaderGraphProgramCompiler.Lower(Inno.Core.Graphs.GraphDocument document, string implementationId, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis> sources, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgramCompiler.cs#L55) | Validates the complete pass/stage graph without deleting invalid or unavailable records. |

### `Inno.Rendering.Shaders.ShaderGraphProgramResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphProgramResult`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L44) | Contains the result of lowering an entire shader graph, before target-native compilation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphDiagnostic> Inno.Rendering.Shaders.ShaderGraphProgramResult.diagnostics`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L60) | Gets stable graph/node diagnostics without provider references. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphPass> Inno.Rendering.Shaders.ShaderGraphProgramResult.passes`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L56) | Gets the complete stage set; empty if any stage failed. |
| [`bool Inno.Rendering.Shaders.ShaderGraphProgramResult.succeeded`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphProgram.cs#L64) | Gets whether every declared pass was lowered without errors. |

### `Inno.Rendering.Shaders.ShaderGraphPrograms`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphPrograms`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L28) | Edits pass-to-program references without copying computation or changing runtime render states. |
| [`const string Inno.Rendering.Shaders.ShaderGraphPrograms.bindingsKey`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L33) | Identifies native metadata holding pass references to shared stage programs. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPrograms.Bind(Inno.Core.Graphs.GraphDocument graph, string pass, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> stages, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L80) | Assigns existing shared stages to one pass in an atomic detached candidate. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphPrograms.RemovePass(Inno.Core.Graphs.GraphDocument graph, string pass, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L127) | Removes a pass and only computations whose final referencing pass was removed. |
| [`static Inno.Rendering.Shaders.ShaderGraphPassProgram[] Inno.Rendering.Shaders.ShaderGraphPrograms.Read(Inno.Core.Graphs.GraphDocument graph, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphPrograms.cs#L50) | Reads detached pass-to-stage references, including unresolved authored identities. |

### `Inno.Rendering.Shaders.ShaderGraphStageSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Shaders.ShaderGraphStageSettings.stage`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L240) | Gets or sets the programmable stage. |
| [`Inno.Rendering.Shaders.ShaderGraphOutput[] Inno.Rendering.Shaders.ShaderGraphStageSettings.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L244) | Gets or sets output ports and their GPU destinations. |
| [`Inno.Rendering.Shaders.ShaderGraphStageSettings`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L235) | Stores one stage's explicit output interface and compute dimensions inside its output node. |
| [`int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsX`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L248) | Gets or sets compute workgroup width. |
| [`int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsY`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L252) | Gets or sets compute workgroup height. |
| [`int Inno.Rendering.Shaders.ShaderGraphStageSettings.threadsZ`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphDocument.cs#L256) | Gets or sets compute workgroup depth. |

### `Inno.Rendering.Shaders.ShaderGraphTemplate`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphTemplate`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L48) | Contributes an ordinary shader graph to the shared asset creation workflow. |
| [`abstract Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplate.Create(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L62) | Creates a fresh detached graph with its target and parameter declarations. |

### `Inno.Rendering.Shaders.ShaderGraphTemplateAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphTemplateAttribute`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L10) | Declares immutable creation metadata for a Shader graph template. |
| [`Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.ShaderGraphTemplateAttribute(string id, string displayName)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L22) | Creates Shader graph template discovery metadata. |
| [`string Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.displayName`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L42) | Gets the user-facing creation menu label. |
| [`string Inno.Rendering.Shaders.ShaderGraphTemplateAttribute.id`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplate.cs#L37) | Gets the stable template identity used by creation commands. |

### `Inno.Rendering.Shaders.ShaderGraphTemplateInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphTemplateInfo`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L20) | Describes a creation menu item without retaining a plugin instance. |

### `Inno.Rendering.Shaders.ShaderGraphTemplateRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Create(string id, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L76) | Invokes the selected template under one generation lease. |
| [`Inno.Rendering.Shaders.ShaderGraphTemplateRegistry`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L28) | Owns generation-safe template discovery and invocation for editor asset creation. |
| [`Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.ShaderGraphTemplateRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L39) | Registers a template owner with the shared type-generation catalog. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderGraphTemplateInfo> Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.templates`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L48) | Gets detached menu descriptions for the current generation. |
| [`void Inno.Rendering.Shaders.ShaderGraphTemplateRegistry.Dispose()`](../../src/services/rendering/Inno.Rendering.Shaders/Templates/ShaderGraphTemplateRegistry.cs#L91) | Retires providers through the shared generation lifecycle. |

### `Inno.Rendering.Shaders.ShaderGraphTemplates`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphTemplates`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphTemplates.cs#L12) | Provides small backend-neutral starting documents composed exclusively of ordinary graph nodes. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderGraphTemplates.CreateRaster(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphTemplates.cs#L26) | Creates a raster graph accepting clip-space XY positions and an exposed RGBA color. |

### `Inno.Rendering.Shaders.ShaderGraphType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStorageAccess Inno.Rendering.Shaders.ShaderGraphType.access`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L47) | Gets or sets storage access. |
| [`Inno.Rendering.RenderTextureDimension Inno.Rendering.Shaders.ShaderGraphType.dimension`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L55) | Gets or sets the image spatial dimension. |
| [`Inno.Rendering.RenderTextureFormat Inno.Rendering.Shaders.ShaderGraphType.format`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L51) | Gets or sets the image texel format. |
| [`Inno.Rendering.Shaders.ShaderGraphType`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L10) | Persists a complete shader type as neutral native-serializable data, without extension instances. |
| [`Inno.Rendering.Shaders.ShaderGraphType? Inno.Rendering.Shaders.ShaderGraphType.element`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L19) | Gets or sets the fixed array element type, or null for a non-array. |
| [`Inno.Rendering.Shaders.ShaderGraphType? Inno.Rendering.Shaders.ShaderGraphType.storageElement`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L43) | Gets or sets the complete buffer element type. |
| [`Inno.Rendering.Shaders.ShaderGraphType[] Inno.Rendering.Shaders.ShaderGraphType.fieldTypes`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L31) | Gets or sets ordered structure member types. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderGraphType.CreateType()`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L97) | Validates and freezes the complete descriptor. |
| [`bool Inno.Rendering.Shaders.ShaderGraphType.isArray`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L59) | Gets or sets whether an image has array layers. |
| [`bool Inno.Rendering.Shaders.ShaderGraphType.isImage`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L39) | Gets or sets whether storage is an image instead of a buffer. |
| [`bool Inno.Rendering.Shaders.ShaderGraphType.isStorage`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L35) | Gets or sets whether the descriptor represents a storage binding. |
| [`int Inno.Rendering.Shaders.ShaderGraphType.length`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L23) | Gets or sets the fixed array length. |
| [`static Inno.Rendering.Shaders.ShaderGraphType Inno.Rendering.Shaders.ShaderGraphType.Capture(Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L70) | Captures a full immutable source type as reload-safe authoring data. |
| [`string Inno.Rendering.Shaders.ShaderGraphType.id`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L15) | Gets or sets the atomic or nominal structure type identity. |
| [`string[] Inno.Rendering.Shaders.ShaderGraphType.fieldNames`](../../src/services/rendering/Inno.Rendering.Shaders/Documents/ShaderGraphType.cs#L27) | Gets or sets ordered structure member names. |

### `Inno.Rendering.Shaders.ShaderIrBlock`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrBlock`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L201) | Contains immutable ordered instructions and nested structured regions without retaining builder callbacks. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBlock.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L218) | Gets named values returned to the enclosing stage or region. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrInstruction> Inno.Rendering.Shaders.ShaderIrBlock.instructions`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L214) | Gets instructions in evaluation order, including source calls whose outputs are unused. |

### `Inno.Rendering.Shaders.ShaderIrBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrBlock Inno.Rendering.Shaders.ShaderIrBuilder.Build(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> outputs)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L364) | Freezes the current region without pruning unused calls or retaining this mutable builder. |
| [`Inno.Rendering.Shaders.ShaderIrBuilder`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L11) | Builds typed shader IR instructions for graph compilation. |
| [`Inno.Rendering.Shaders.ShaderIrBuilder.ShaderIrBuilder()`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L26) | Creates an independent typed region builder and value identity scope. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.AtomicAddStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue index, Inno.Rendering.Shaders.ShaderIrValue value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L140) | Atomically adds to a scalar integer buffer element and returns its previous value. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Binary(Inno.Rendering.Shaders.ShaderIrOperation operation, Inno.Rendering.Shaders.ShaderIrValue left, Inno.Rendering.Shaders.ShaderIrValue right)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L140) | Applies an explicitly typed binary arithmetic or scalar comparison operation. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(bool value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L120) | Produces a scalar Boolean constant. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(float value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L82) | Produces a finite, exact 32-bit floating-point constant. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(int value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L98) | Produces an exact signed 32-bit integer constant. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Constant(uint value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L109) | Produces an exact unsigned 32-bit integer constant. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Construct(Inno.Rendering.Shaders.ShaderSourceType type, params Inno.Rendering.Shaders.ShaderIrValue[] members)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L217) | Constructs a complete vector, column-major matrix, structure or fixed array from typed members. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Extract(Inno.Rendering.Shaders.ShaderIrValue value, int memberIndex)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L262) | Extracts one static component, structure member or array element without source-language member syntax. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Input(string name, Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L49) | Reads a named input from the enclosing typed function or stage interface. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.LoadStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue coordinate)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L83) | Loads a storage value at this exact point in the block's memory-effect sequence. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Sample(Inno.Rendering.Shaders.ShaderIrValue texture, Inno.Rendering.Shaders.ShaderIrValue coordinate)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L26) | Samples a texture using implicit derivatives; the enclosing stage must be Fragment. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.SampleLevel(Inno.Rendering.Shaders.ShaderIrValue texture, Inno.Rendering.Shaders.ShaderIrValue coordinate, Inno.Rendering.Shaders.ShaderIrValue level)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L54) | Samples a texture at an explicit floating-point mip level without implicit derivatives. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderIrBuilder.Select(Inno.Rendering.Shaders.ShaderIrValue condition, Inno.Rendering.Shaders.ShaderIrValue whenTrue, Inno.Rendering.Shaders.ShaderIrValue whenFalse)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L185) | Selects between equal typed values without conditionally executing their producers. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Branch(Inno.Rendering.Shaders.ShaderIrValue condition, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> whenTrue, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> whenFalse)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L34) | Builds a real conditional: only the selected region executes, including its memory and source-call effects. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Call(Inno.Rendering.Shaders.ShaderSourceModuleAnalysis module, string implementationId, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> inputs)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.cs#L315) | Calls a validated source implementation using semantic parameter names, preserving all call side effects. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrBuilder.Loop(Inno.Rendering.Shaders.ShaderIrValue iterations, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> initialState, System.Func<Inno.Rendering.Shaders.ShaderIrBuilder, Inno.Rendering.Shaders.ShaderIrValue, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue>> body)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.ControlFlow.cs#L82) | Builds a counted loop; each iteration receives the preceding iteration's complete carried state. |
| [`void Inno.Rendering.Shaders.ShaderIrBuilder.Discard(Inno.Rendering.Shaders.ShaderIrValue condition)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L165) | Discards a fragment when the Boolean condition is true; other stages reject this instruction. |
| [`void Inno.Rendering.Shaders.ShaderIrBuilder.StoreStorage(Inno.Rendering.Shaders.ShaderIrValue resource, Inno.Rendering.Shaders.ShaderIrValue coordinate, Inno.Rendering.Shaders.ShaderIrValue value)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBuilder.Resources.cs#L110) | Stores a storage value without pruning unused writes or reordering surrounding memory operations. |

### `Inno.Rendering.Shaders.ShaderIrInputKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrInputKind`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L11) | Identifies the GPU interface through which a stage receives a value. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.Builtin`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L36) | A standard GPU stage input identified by a target semantic, not native source text. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.SampledTexture`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L28) | A sampled texture binding whose slot is assigned by target resource layout. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.Storage`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L32) | A typed storage buffer or image whose access, shape and layout are explicit. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.Uniform`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L24) | A material, frame or draw uniform identified by a stable binding identity. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.Varying`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L20) | An interpolated value written by the preceding raster stage. |
| [`Inno.Rendering.Shaders.ShaderIrInputKind.VertexAttribute`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L16) | A vertex stream attribute, including instance-rate streams. |

### `Inno.Rendering.Shaders.ShaderIrInstruction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrInstruction`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L136) | Stores one immutable typed instruction without generated stage source or backend expressions. |
| [`Inno.Rendering.Shaders.ShaderIrOperation Inno.Rendering.Shaders.ShaderIrInstruction.operation`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L161) | Gets the semantic operation independent of any source language. |
| [`Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis? Inno.Rendering.Shaders.ShaderIrInstruction.source`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L185) | Gets a frozen, analyzed function implementation only for a SourceCall instruction. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrBlock> Inno.Rendering.Shaders.ShaderIrInstruction.regions`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L189) | Gets immutable nested regions: true/false for Branch, or one body for Loop. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrInstruction.inputs`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L165) | Gets immutable operands in operation or function-declaration order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderIrInstruction.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L169) | Gets produced values; a call orders its return value first, then out/inout parameters. |
| [`bool Inno.Rendering.Shaders.ShaderIrInstruction.hasSideEffects`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L193) | Gets whether removing or reordering this instruction may change observable behavior. |
| [`int Inno.Rendering.Shaders.ShaderIrInstruction.memberIndex`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L181) | Gets the zero-based component/member/element ordinal for an Extract instruction. |
| [`string? Inno.Rendering.Shaders.ShaderIrInstruction.inputName`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L173) | Gets the stable interface or region-parameter identity, only for Input or RegionInput. |
| [`ulong Inno.Rendering.Shaders.ShaderIrInstruction.constantBits`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L177) | Gets exact scalar bits for a Constant instruction; the result type determines interpretation. |

### `Inno.Rendering.Shaders.ShaderIrOperation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrOperation`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L37) | Defines backend-neutral value operations; graph node IDs are not instruction opcodes. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Add`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L58) | Adds two equal numeric types component by component. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Branch`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L126) | Executes exactly one nested region and merges its named output values. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Constant`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L46) | Produces an exact scalar constant stored as bits, not source-language text. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Construct`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L50) | Constructs an aggregate from typed scalar or member values. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Discard`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L118) | Discards the current fragment when the scalar condition is true. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Divide`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L70) | Divides two equal numeric types component by component. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Equal`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L82) | Tests equality of two scalar values and produces a scalar Boolean. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Extract`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L54) | Extracts one statically selected vector component, array element or structure field. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Input`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L42) | Reads a named input supplied by the enclosing function or stage. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.LessThan`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L86) | Compares two equal numeric scalar types and produces a scalar Boolean. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Loop`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L130) | Executes a counted loop with explicit typed carried values and ordered body effects. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Maximum`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L78) | Selects the component-wise maximum of two equal numeric types. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Minimum`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L74) | Selects the component-wise minimum of two equal numeric types. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Multiply`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L66) | Multiplies two equal numeric types component by component; this is not matrix multiplication. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.RegionInput`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L122) | Provides a typed iteration index or loop-carried value inside a nested region. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Select`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L90) | Selects between equal types using a scalar Boolean; it does not introduce control flow. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.SourceCall`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L94) | Calls a parsed source function with named inputs and ordered multiple outputs. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.StorageAtomicAdd`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L114) | Atomically adds to an integer buffer element and returns the original value. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.StorageLoad`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L106) | Loads one typed buffer element or formatted image texel in memory-effect order. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.StorageStore`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L110) | Stores one typed buffer element or formatted image texel in memory-effect order. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.Subtract`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L62) | Subtracts two equal numeric types component by component. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.TextureSample`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L98) | Samples a floating-point texture using implicit fragment derivatives. |
| [`Inno.Rendering.Shaders.ShaderIrOperation.TextureSampleLevel`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L102) | Samples a floating-point texture at an explicit mip level. |

### `Inno.Rendering.Shaders.ShaderIrOutputKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L42) | Identifies where a typed stage output is written. |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind.ClipPosition`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L47) | The vertex position in homogeneous clip space. |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind.Color`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L55) | A fragment color attachment at its explicit location. |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind.Depth`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L59) | The fragment depth value. |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind.Varying`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L51) | A value interpolated for the next raster stage. |

### `Inno.Rendering.Shaders.ShaderIrStage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Shaders.ShaderIrStage.stage`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L246) | Gets the programmable stage. |
| [`Inno.Rendering.Shaders.ShaderIrBlock Inno.Rendering.Shaders.ShaderIrStage.body`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L250) | Gets its immutable ordered instructions. |
| [`Inno.Rendering.Shaders.ShaderIrStage`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L190) | Freezes a typed GPU stage and its explicit interface. It contains no main function, varying declarations or source expressions. Targets own stage/resource layout; adapters translate these semantics into their native language. |
| [`Inno.Rendering.Shaders.ShaderIrStage.ShaderIrStage(Inno.Rendering.ShaderStage stage, Inno.Rendering.Shaders.ShaderIrBlock body, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStageInput> inputs, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderIrStageOutput> outputs, int threadsX = 1, int threadsY = 1, int threadsZ = 1)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L216) | Validates and freezes a single stage's interface and ordered body. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStageInput> Inno.Rendering.Shaders.ShaderIrStage.inputs`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L254) | Gets all input bindings. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderIrStageOutput> Inno.Rendering.Shaders.ShaderIrStage.outputs`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L258) | Gets all output destinations. |
| [`int Inno.Rendering.Shaders.ShaderIrStage.threadsX`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L262) | Gets the compute workgroup X size. |
| [`int Inno.Rendering.Shaders.ShaderIrStage.threadsY`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L266) | Gets the compute workgroup Y size. |
| [`int Inno.Rendering.Shaders.ShaderIrStage.threadsZ`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L270) | Gets the compute workgroup Z size. |
| [`string Inno.Rendering.Shaders.ShaderIrStage.contentHash`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L275) | Gets a deterministic hash of stage semantics, ordered effects, nested regions, complete types, resource layout and frozen sources. A compiled artifact cache must additionally include toolchain identity, target capabilities, optimization and variant configuration. |

### `Inno.Rendering.Shaders.ShaderIrStageInput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrInputKind Inno.Rendering.Shaders.ShaderIrStageInput.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L119) | Gets the GPU input category. |
| [`Inno.Rendering.Shaders.ShaderIrStageInput`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L65) | Describes a stage input without embedding native declarations or expressions. |
| [`Inno.Rendering.Shaders.ShaderIrStageInput.ShaderIrStageInput(string id, Inno.Rendering.Shaders.ShaderSourceType type, Inno.Rendering.Shaders.ShaderIrInputKind kind, string semantic = "", int location = 0)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L85) | Creates an immutable stage input binding. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderIrStageInput.type`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L115) | Gets the complete typed value received by the stage. |
| [`int Inno.Rendering.Shaders.ShaderIrStageInput.location`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L127) | Gets the target-assigned interface location or sampled-texture slot. |
| [`string Inno.Rendering.Shaders.ShaderIrStageInput.id`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L111) | Gets the logical binding identity. |
| [`string Inno.Rendering.Shaders.ShaderIrStageInput.semantic`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L123) | Gets the target semantic; an adapter must reject unsupported semantics. |

### `Inno.Rendering.Shaders.ShaderIrStageOutput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrOutputKind Inno.Rendering.Shaders.ShaderIrStageOutput.kind`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L175) | Gets the GPU output category. |
| [`Inno.Rendering.Shaders.ShaderIrStageOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L133) | Describes a named block output's GPU destination. |
| [`Inno.Rendering.Shaders.ShaderIrStageOutput.ShaderIrStageOutput(string id, Inno.Rendering.Shaders.ShaderIrOutputKind kind, string semantic = "", int location = 0)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L150) | Creates an immutable stage output binding. |
| [`int Inno.Rendering.Shaders.ShaderIrStageOutput.location`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L183) | Gets the target-assigned varying or attachment index. |
| [`string Inno.Rendering.Shaders.ShaderIrStageOutput.id`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L171) | Gets the exact block output identity. |
| [`string Inno.Rendering.Shaders.ShaderIrStageOutput.semantic`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrStage.cs#L179) | Gets the varying semantic, not native assignment syntax. |

### `Inno.Rendering.Shaders.ShaderIrValue`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrValue`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L11) | Identifies a typed single-assignment value within one intermediate block. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderIrValue.type`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L30) | Gets the complete language-independent value type. |
| [`int Inno.Rendering.Shaders.ShaderIrValue.index`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderIrBlock.cs#L26) | Gets the deterministic block-local value index, not a GPU register or resource slot. |

### `Inno.Rendering.Shaders.ShaderNodeCompilerCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphLoweringResult Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest request, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L103) | Lowers an entire target-selected region, preserving stable topological/document order and all source calls. Missing definitions, stale ports and cycles fail without mutating graph records or dropping edges. |
| [`Inno.Rendering.Shaders.ShaderNodeCompilerCatalog`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L16) | Owns an immutable compiler map; the common lowering algorithm never switches on concrete node identities. |
| [`Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.ShaderNodeCompilerCatalog(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.IShaderNodeCompiler> compilers)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L27) | Validates a complete generation of node compiler registrations. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.DescribePorts(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? source = null, string implementationId = "", Inno.Rendering.Shaders.ShaderIrStageInput? input = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L70) | Describes current typed ports for editor presentation without exposing the compiler provider. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderNodeCompilerCatalog.definitionIds`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs#L44) | Gets stable registered definition identities, without exposing providers. |

### `Inno.Rendering.Shaders.ShaderNodeCompilerRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderGraphLoweringResult Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Lower(Inno.Rendering.Shaders.ShaderGraphLoweringRequest request, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L56) | Lowers a whole region under one shared generation operation. |
| [`Inno.Rendering.Shaders.ShaderNodeCompilerRegistry`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L14) | Owns reloadable shader node compilers through the shared candidate, rollback and retirement protocol. |
| [`Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.ShaderNodeCompilerRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L24) | Registers the compiler generation owner with the shared type catalog. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DescribePorts(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? source = null, string implementationId = "", Inno.Rendering.Shaders.ShaderIrStageInput? input = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L90) | Captures a node's current typed ports under the shared generation lease. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.definitionIds`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L29) | Gets stable compiler identities without exposing provider instances. |
| [`override Inno.Rendering.Shaders.ShaderNodeCompilerCatalog Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L111) | Builds a validated result from the current immutable input snapshot. |
| [`override void Inno.Rendering.Shaders.ShaderNodeCompilerRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderNodeCompilerCatalog snapshot)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs#L126) | Releases the generation lease retained by an immutable registry snapshot. |

### `Inno.Rendering.Shaders.ShaderNodeDescriptionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphNodeId Inno.Rendering.Shaders.ShaderNodeDescriptionContext.nodeId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L91) | Gets the node's stable identity, never its editor runtime handle. |
| [`Inno.Rendering.Shaders.ShaderIrStageInput? Inno.Rendering.Shaders.ShaderNodeDescriptionContext.stageInput`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L107) | Gets the target-assigned stage input for this node, when applicable. |
| [`Inno.Rendering.Shaders.ShaderNodeDescriptionContext`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L66) | Provides invocation-scoped property decoding through the complete owner serialization context. |
| [`Inno.Rendering.Shaders.ShaderSourceModuleAnalysis? Inno.Rendering.Shaders.ShaderNodeDescriptionContext.sourceModule`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L99) | Gets a frozen module resolved by the asset owner; null means unassigned or unavailable. |
| [`T Inno.Rendering.Shaders.ShaderNodeDescriptionContext.Read<T>(string id, T defaultValue)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L124) | Reads a property using the owner's converter generation and complete reference resolver context. |
| [`string Inno.Rendering.Shaders.ShaderNodeDescriptionContext.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L95) | Gets the node definition's stable identity. |
| [`string Inno.Rendering.Shaders.ShaderNodeDescriptionContext.implementationId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L103) | Gets the exact implementation key chosen by the target. |

### `Inno.Rendering.Shaders.ShaderNodeLoweringContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderIrBuilder Inno.Rendering.Shaders.ShaderNodeLoweringContext.builder`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L157) | Gets the builder shared by nodes in this typed region. |
| [`Inno.Rendering.Shaders.ShaderIrValue Inno.Rendering.Shaders.ShaderNodeLoweringContext.Input(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L171) | Requires one connected input without implicit defaults or positional rebinding. |
| [`Inno.Rendering.Shaders.ShaderNodeDescriptionContext Inno.Rendering.Shaders.ShaderNodeLoweringContext.description`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L153) | Gets the invocation-scoped node description. |
| [`Inno.Rendering.Shaders.ShaderNodeLoweringContext`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L138) | Supplies one node's connected values and the region builder during lowering only. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderNodeLoweringContext.inputs`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L161) | Gets connected values keyed by semantic input port identity, never connection order. |

### `Inno.Rendering.Shaders.ShaderNodePort`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderNodePort`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompiler.cs#L25) | Declares a typed shader port independently of editor presentation and backend source syntax. |

### `Inno.Rendering.Shaders.ShaderRerouteNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderRerouteNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L9) | Forwards a typed connection without adding instructions or changing resource-effect order. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L38) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L24) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderRerouteNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderRerouteNodeCompiler.cs#L14) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderSampleNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSampleNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L48) | Samples a graph-connected texture with implicit derivatives or an explicit level of detail. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSampleNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L82) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSampleNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L63) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderSampleNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderValueNodeCompilers.cs#L53) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderSelectNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSelectNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L195) | Selects between equal typed values; all producer effects remain evaluated before selection. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSelectNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L225) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSelectNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L210) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderSelectNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L200) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderSourceAnalysis`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L141) | Returns a recoverable source analysis result and complete original-source dependencies. |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis.ShaderSourceAnalysis(Inno.Rendering.Shaders.ShaderSourceFunction? function, System.Collections.Generic.IEnumerable<string> dependencies, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceDiagnostic> diagnostics)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L155) | Captures a completed analysis without retaining compiler objects. |
| [`Inno.Rendering.Shaders.ShaderSourceFunction? Inno.Rendering.Shaders.ShaderSourceAnalysis.function`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L169) | Gets the parsed interface, or null after an analysis failure. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceDiagnostic> Inno.Rendering.Shaders.ShaderSourceAnalysis.diagnostics`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L177) | Gets immutable, provider-neutral diagnostics. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceAnalysis.dependencies`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L173) | Gets original source dependencies for invalidation. |
| [`bool Inno.Rendering.Shaders.ShaderSourceAnalysis.succeeded`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L181) | Gets whether a usable interface was found without errors. |

### `Inno.Rendering.Shaders.ShaderSourceDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceDiagnostic`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L41) | Reports a frontend problem without retaining a language-specific exception or parser. |

### `Inno.Rendering.Shaders.ShaderSourceField`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceField`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L155) | Describes one immutable named structure field. |
| [`Inno.Rendering.Shaders.ShaderSourceField.ShaderSourceField(string name, Inno.Rendering.Shaders.ShaderSourceType type)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L169) | Creates a structure field. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceField.type`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L186) | Gets the complete field type. |
| [`string Inno.Rendering.Shaders.ShaderSourceField.name`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L182) | Gets the public field name. |

### `Inno.Rendering.Shaders.ShaderSourceFile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceFile`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L57) | Contains immutable source text supplied by the asset candidate's controlled source resolver. |

### `Inno.Rendering.Shaders.ShaderSourceFrontendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.Analyze(string languageId, Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L56) | Analyzes source using an explicitly selected language. |
| [`Inno.Rendering.Shaders.ShaderSourceFrontendCatalog`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L12) | Owns an immutable language registration snapshot for one authoring generation. |
| [`Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.ShaderSourceFrontendCatalog(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.IShaderSourceFrontend> frontends)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L25) | Captures a validated language provider set without a process-global registry. |
| [`Inno.Rendering.Shaders.ShaderSourceModuleAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.AnalyzeModule(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceImplementationRequest> implementations)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L79) | Analyzes all selected implementations and variants against one immutable provider generation. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceFrontendCatalog.languageIds`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendCatalog.cs#L37) | Gets registered language identities for source import settings. |

### `Inno.Rendering.Shaders.ShaderSourceFrontendRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Analyze(string languageId, Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L51) | Analyzes source while preventing provider retirement for the complete synchronous operation. |
| [`Inno.Rendering.Shaders.ShaderSourceFrontendRegistry`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L12) | Discovers source language providers through the shared type-generation transaction and owns their retirement. |
| [`Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.ShaderSourceFrontendRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L22) | Registers this authoring owner with the shared candidate and rollback coordinator. |
| [`Inno.Rendering.Shaders.ShaderSourceModuleAnalysis Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.AnalyzeModule(System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceImplementationRequest> implementations)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L71) | Freezes and validates a complete module without mixing frontend generations between implementations. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.languageIds`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L27) | Gets immutable language identities available in the current generation. |
| [`override Inno.Rendering.Shaders.ShaderSourceFrontendCatalog Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L86) | Builds a validated result from the current immutable input snapshot. |
| [`override void Inno.Rendering.Shaders.ShaderSourceFrontendRegistry.DisposeSnapshot(Inno.Rendering.Shaders.ShaderSourceFrontendCatalog snapshot)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFrontendRegistry.cs#L102) | Releases the generation lease retained by an immutable registry snapshot. |

### `Inno.Rendering.Shaders.ShaderSourceFunction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceFunction`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L77) | Contains the single callable interface exported by a source asset. |
| [`Inno.Rendering.Shaders.ShaderSourceFunction.ShaderSourceFunction(string name, Inno.Rendering.Shaders.ShaderSourceType returnType, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceParameter> parameters, Inno.Rendering.Shaders.ShaderSourcePosition location)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L97) | Captures a source function declaration without retaining parser or provider objects. |
| [`Inno.Rendering.Shaders.ShaderSourcePosition Inno.Rendering.Shaders.ShaderSourceFunction.location`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L131) | Gets the original declaration position. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceFunction.returnType`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L123) | Gets the canonical result type. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceParameter> Inno.Rendering.Shaders.ShaderSourceFunction.parameters`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L127) | Gets the immutable function parameters. |
| [`bool Inno.Rendering.Shaders.ShaderSourceFunction.HasSameInterface(Inno.Rendering.Shaders.ShaderSourceFunction? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L142) | Validates alternative implementations by semantic names, types, direction, and call order. |
| [`string Inno.Rendering.Shaders.ShaderSourceFunction.name`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L119) | Gets the implementation function name. |

### `Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.analysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L126) | Gets the parsed interface and implementation-local diagnostics. |
| [`Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L75) | Freezes a parsed implementation and its complete source inputs without retaining a frontend or resolver. |
| [`Inno.Rendering.Shaders.ShaderSourceRequest Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.CreateSourceRequest()`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L139) | Recreates a compiler input backed only by immutable captured sources and resolution edges. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.defines`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L114) | Gets the immutable preprocessing inputs used for this implementation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceFile> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sources`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L118) | Gets every successfully read source, including the root, ordered by path. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceInclude> Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.includes`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L122) | Gets the original resolver's frozen include edges, including aliases and mounted source paths. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.contentHash`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L131) | Gets a deterministic source-input hash. A compiler cache must additionally include its toolchain, target, graph semantics and binding layout; this hash alone is not a compiled artifact key. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.entryPoint`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L110) | Gets the implementation's selected callable function name. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.implementationId`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L98) | Gets the stable implementation/configuration key. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.languageId`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L102) | Gets the selected source language. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis.sourcePath`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L106) | Gets the root source path in the frozen source set. |

### `Inno.Rendering.Shaders.ShaderSourceImplementationRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceImplementationRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L32) | Identifies one explicitly selected implementation and preprocessing configuration of a source module. |
| [`Inno.Rendering.Shaders.ShaderSourceImplementationRequest.ShaderSourceImplementationRequest(string implementationId, string languageId, Inno.Rendering.Shaders.ShaderSourceRequest source)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L46) | Creates a candidate without inferring a language or adapter from its filename. |
| [`Inno.Rendering.Shaders.ShaderSourceRequest Inno.Rendering.Shaders.ShaderSourceImplementationRequest.source`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L69) | Gets the transient candidate request, which must not be persisted or retained after analysis. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationRequest.implementationId`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L61) | Gets the stable owner-defined implementation/configuration key. |
| [`string Inno.Rendering.Shaders.ShaderSourceImplementationRequest.languageId`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L65) | Gets the explicitly selected language identity. |

### `Inno.Rendering.Shaders.ShaderSourceInclude`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceInclude`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L23) | Records an include resolution edge so compilation never reinterprets authoring paths against live files. |

### `Inno.Rendering.Shaders.ShaderSourceModuleAnalysis`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceFunction? Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.function`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L227) | Gets the common interface only when all supplied implementations agree and are valid. |
| [`Inno.Rendering.Shaders.ShaderSourceModuleAnalysis`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L203) | Reports whether every supplied implementation and variant has the same callable graph interface. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceDiagnostic> Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.diagnostics`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L223) | Gets implementation and cross-implementation diagnostics with original source positions. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceImplementationAnalysis> Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.implementations`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L219) | Gets all implementation snapshots, including failed or unavailable implementations. |
| [`bool Inno.Rendering.Shaders.ShaderSourceModuleAnalysis.succeeded`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceModuleAnalysis.cs#L231) | Gets whether interface analysis succeeded; native compilation is a separate gate. |

### `Inno.Rendering.Shaders.ShaderSourceNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L232) | Lowers a parsed function module with name-based ports and explicit aggregate/member connection alternatives. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderSourceNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L280) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderSourceNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L247) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderSourceNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L237) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderSourceNodeDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceFunction Inno.Rendering.Shaders.ShaderSourceNodeDefinition.function`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L55) | Gets the immutable parsed source interface belonging to this definition. |
| [`Inno.Rendering.Shaders.ShaderSourceNodeDefinition`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L12) | Projects one resolved source interface into immutable graph ports without duplicating authored port declarations. |
| [`Inno.Rendering.Shaders.ShaderSourceNodeDefinition.ShaderSourceNodeDefinition(Inno.Rendering.Shaders.ShaderSourceFunction function)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L22) | Creates a source node definition for the currently resolved source generation. |
| [`override System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphPortDefinition> Inno.Rendering.Shaders.ShaderSourceNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceNodeDefinition.cs#L66) | Gets a ports required by the implemented contract. |

### `Inno.Rendering.Shaders.ShaderSourceParameter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceParameter`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L29) | Describes one parameter parsed from a public source declaration. |
| [`Inno.Rendering.Shaders.ShaderSourceParameter.ShaderSourceParameter(string name, Inno.Rendering.Shaders.ShaderSourceType type, Inno.Rendering.Shaders.ShaderSourceParameterDirection direction)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L46) | Creates a validated function parameter. |
| [`Inno.Rendering.Shaders.ShaderSourceParameterDirection Inno.Rendering.Shaders.ShaderSourceParameter.direction`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L71) | Gets the declared value flow. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceParameter.type`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L67) | Gets the complete canonical value type. |
| [`string Inno.Rendering.Shaders.ShaderSourceParameter.name`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L63) | Gets the public parameter name. |

### `Inno.Rendering.Shaders.ShaderSourceParameterDirection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceParameterDirection`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L10) | Declares value flow across a source function parameter. |
| [`Inno.Rendering.Shaders.ShaderSourceParameterDirection.Input`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L15) | The caller supplies a value. |
| [`Inno.Rendering.Shaders.ShaderSourceParameterDirection.InputOutput`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L23) | The caller supplies a value and the function produces its replacement. |
| [`Inno.Rendering.Shaders.ShaderSourceParameterDirection.Output`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceFunction.cs#L19) | The function produces a value. |

### `Inno.Rendering.Shaders.ShaderSourcePosition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourcePosition`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L20) | Identifies an original source position, not a generated temporary compiler filename. |

### `Inno.Rendering.Shaders.ShaderSourceRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.IShaderSourceResolver Inno.Rendering.Shaders.ShaderSourceRequest.resolver`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L131) | Gets the candidate-scoped dependency resolver. |
| [`Inno.Rendering.Shaders.ShaderSourceFile Inno.Rendering.Shaders.ShaderSourceRequest.source`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L123) | Gets the root source snapshot. |
| [`Inno.Rendering.Shaders.ShaderSourceRequest`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L88) | Carries the immutable inputs required to execute one shader source request. |
| [`Inno.Rendering.Shaders.ShaderSourceRequest.ShaderSourceRequest(Inno.Rendering.Shaders.ShaderSourceFile source, string entryPoint, Inno.Rendering.Shaders.IShaderSourceResolver resolver, System.Collections.Generic.IReadOnlyDictionary<string, string>? defines = null)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L105) | Creates an analysis request for an explicitly selected function. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Shaders.ShaderSourceRequest.defines`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L135) | Gets immutable preprocessing inputs included in analysis cache identity. |
| [`string Inno.Rendering.Shaders.ShaderSourceRequest.entryPoint`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/IShaderSourceFrontend.cs#L127) | Gets the explicitly selected export function name. |

### `Inno.Rendering.Shaders.ShaderSourceType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceType`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L10) | Describes a language-independent function value, including aggregate members and fixed arrays. |
| [`Inno.Rendering.Shaders.ShaderSourceType? Inno.Rendering.Shaders.ShaderSourceType.elementType`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L34) | Gets the array element type, or null for a non-array value. |
| [`Inno.Rendering.Shaders.ShaderStorageType? Inno.Rendering.Shaders.ShaderSourceType.storage`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L47) | Gets a typed storage binding contract, or null for ordinary values and sampled textures. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceField> Inno.Rendering.Shaders.ShaderSourceType.fields`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L42) | Gets immutable structure fields in declaration order. |
| [`bool Inno.Rendering.Shaders.ShaderSourceType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderSourceType? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L136) | Checks both nominal identity and complete aggregate layout. |
| [`int Inno.Rendering.Shaders.ShaderSourceType.elementCount`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L38) | Gets the fixed array length, or zero for a non-array value. |
| [`static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.ArrayOf(Inno.Rendering.Shaders.ShaderSourceType element, int count)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L87) | Creates a fixed-length array without collapsing its element type. |
| [`static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Atomic(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L70) | Creates an atomic type supplied by a language or graph target. |
| [`static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Storage(Inno.Rendering.Shaders.ShaderStorageType storage)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L58) | Creates an opaque storage binding value with a complete element, format and access contract. |
| [`static Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderSourceType.Structure(string id, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceField> fields)`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L113) | Creates a nominal structure with a validated immutable field layout. |
| [`string Inno.Rendering.Shaders.ShaderSourceType.id`](../../src/services/rendering/Inno.Rendering.Shaders/Sources/ShaderSourceType.cs#L30) | Gets the open semantic type identifier, independent of source-language spelling. |

### `Inno.Rendering.Shaders.ShaderStageInputNodeCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderStageInputNodeCompiler`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L157) | Reads one target-assigned stage/resource input without choosing a native variable name. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderIrValue> Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.Lower(Inno.Rendering.Shaders.ShaderNodeLoweringContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L183) | Lowers this graph node to typed shader IR after validating its inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderNodePort> Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.GetPorts(Inno.Rendering.Shaders.ShaderNodeDescriptionContext context)`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L172) | Gets a ports required by the implemented contract. |
| [`string Inno.Rendering.Shaders.ShaderStageInputNodeCompiler.definitionId`](../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderBuiltinNodeCompilers.cs#L162) | Gets the definition id text used by the current instance. |

### `Inno.Rendering.Shaders.ShaderStorageType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStorageAccess Inno.Rendering.Shaders.ShaderStorageType.access`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L34) | Gets the permitted memory access, independent of Render Graph scheduling. |
| [`Inno.Rendering.RenderTextureDimension Inno.Rendering.Shaders.ShaderStorageType.dimension`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L42) | Gets the image dimension; ignored for buffers. |
| [`Inno.Rendering.RenderTextureFormat? Inno.Rendering.Shaders.ShaderStorageType.format`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L38) | Gets the exact storage image format, or null for a structured buffer. |
| [`Inno.Rendering.Shaders.ShaderSourceType Inno.Rendering.Shaders.ShaderStorageType.valueType`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L30) | Gets the buffer element or image load/store value type; image operations use float4. |
| [`Inno.Rendering.Shaders.ShaderStorageType`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L9) | Describes typed storage without embedding a backend register, declaration or resource handle. |
| [`bool Inno.Rendering.Shaders.ShaderStorageType.IsEquivalentTo(Inno.Rendering.Shaders.ShaderStorageType? other)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L119) | Compares complete access, format, shape and element layout contracts. |
| [`bool Inno.Rendering.Shaders.ShaderStorageType.array`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L46) | Gets whether a two-dimensional storage image has array layers. |
| [`bool Inno.Rendering.Shaders.ShaderStorageType.isImage`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L50) | Gets whether this descriptor denotes an image rather than a structured buffer. |
| [`static Inno.Rendering.Shaders.ShaderStorageType Inno.Rendering.Shaders.ShaderStorageType.Buffer(Inno.Rendering.Shaders.ShaderSourceType element, Inno.Rendering.RenderStorageAccess access)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L67) | Creates a structured storage buffer with an explicit element layout. |
| [`static Inno.Rendering.Shaders.ShaderStorageType Inno.Rendering.Shaders.ShaderStorageType.Image(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderStorageAccess access, Inno.Rendering.RenderTextureDimension dimension = Inno.Rendering.RenderTextureDimension.Texture2D, bool array = false)`](../../src/services/rendering/Inno.Rendering.Shaders/Intermediate/ShaderStorageType.cs#L97) | Creates a formatted storage image; target capabilities must separately support its access and format. |

### `Inno.Rendering.Shaders.ShaderTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderTarget`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L34) | Expands a domain's surface contract into ordinary graph stages before source dependency capture. |
| [`abstract Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTarget.Expand(Inno.Rendering.Shaders.ShaderTargetContext context, System.Threading.CancellationToken cancellationToken)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L48) | Builds explicit stage interfaces, resource declarations, techniques and pass states. |

### `Inno.Rendering.Shaders.ShaderTargetAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderTargetAttribute`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L11) | Declares the immutable identity used to select a Shader Target from an authored graph. |
| [`Inno.Rendering.Shaders.ShaderTargetAttribute.ShaderTargetAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L20) | Creates target discovery metadata. |
| [`string Inno.Rendering.Shaders.ShaderTargetAttribute.id`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L28) | Gets the stable target identity persisted by Shader assets. |

### `Inno.Rendering.Shaders.ShaderTargetContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTargetContext.document`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L72) | Gets a private document copy; expansion cannot modify the authored source. |
| [`Inno.Core.Serialization.SerializationContext Inno.Rendering.Shaders.ShaderTargetContext.references`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L80) | Gets the complete owner reference context used for generated resource defaults. |
| [`Inno.Core.Serialization.SerializationRegistry Inno.Rendering.Shaders.ShaderTargetContext.serialization`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L76) | Gets the current owner converter registry, valid only during expansion. |
| [`Inno.Rendering.Shaders.ShaderTargetContext`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTarget.cs#L57) | Supplies neutral authoring data to a domain target without exposing an adapter or live asset. |

### `Inno.Rendering.Shaders.ShaderTargetRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument Inno.Rendering.Shaders.ShaderTargetRegistry.Expand(Inno.Core.Graphs.GraphDocument document, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L68) | Expands an assigned target, or copies an explicitly authored low-level graph with no domain target. |
| [`Inno.Rendering.Shaders.ShaderTargetRegistry`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L15) | Resolves domain targets under the same generation lease as their graph expansion. |
| [`Inno.Rendering.Shaders.ShaderTargetRegistry.ShaderTargetRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L26) | Creates a target owner participating in shared candidate publication and retirement. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Shaders.ShaderTargetRegistry.ids`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L35) | Gets detached stable target identities available in the current generation. |
| [`void Inno.Rendering.Shaders.ShaderTargetRegistry.Dispose()`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetRegistry.cs#L93) | Retires the target snapshot through the shared registry lifecycle. |

### `Inno.Rendering.Shaders.ShaderTargetUnavailableException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderTargetUnavailableException`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L8) | Identifies an authored target that is absent from the current extension generation. |
| [`Inno.Rendering.Shaders.ShaderTargetUnavailableException.ShaderTargetUnavailableException(string targetId)`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L16) | Creates a missing-target diagnostic without retaining extension objects. |
| [`string Inno.Rendering.Shaders.ShaderTargetUnavailableException.targetId`](../../src/services/rendering/Inno.Rendering.Shaders/Targets/ShaderTargetUnavailableException.cs#L26) | Gets the stable target identity required by the authored graph. |

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
