# Inno.Build.Toolchains.Bgfx.Tools

[分类索引](README.md) · [Rendering Assets](../../rendering/Inno.Rendering.Assets.md) · [Wiki 首页](../../README.md)

## 公开 API 与平台边界

- `BgfxShaderTargetProfile(id, renderers)`：开放目标身份及不可变 renderer catalog，重复 renderer 或空集合明确失败。
- `BgfxShaderCompilerProfile(capabilities, shadercPlatform, vertexProfile, fragmentProfile, computeProfile, defines)`：冻结编译方言、能力和 defines；`key` 包含全部输入，调用方修改原集合不会改变它。
- `BgfxShadercToolchain(profile, tools)`：只执行配置指定的编译，未知引擎目标无需增加 backend switch。
- `BgfxAuthoringProvider(profile, tools)`：向创作组合注册同一 compiler。
- `BgfxGameContentCompiler(assets, serialization, types, profile, backends, tools)`：借用明确工具，构造时验证完整 renderer 闭包，`CompileAsync` 输出 source-free Shader 与 portable KTX。
- `BgfxTargetCapabilities.Create(backend)`：提供可复用的完整离线能力集合。平台可以直接提供自己的更严格 `GraphicsCapabilities`。
- `BgfxTextureTargetCompiler`：离线纹理编译。
- `BgfxShaderSourceFrontend`：显式 BGFX SC 前端；具体语言语义属于 backend。

Windows、macOS、Browser 的实际 Shader profile 分别由所属平台的 `*BgfxShaderProfiles.target` 提供。
Browser 使用 `asm.js` / `300_es` 和 WebGL 2 能力集合，拒绝 Compute；macOS Vulkan 所需
`BGFX_SHADER_LANGUAGE_SPIRV=1` 由其配置声明。共享 compiler 不识别命名平台或维护平台表。
配置改变会进入内容编译和内置 Shader 指纹，不复用旧 dialect、能力或 defines 的产物。

这些类型属于 authoring/build。`ToolRunner` 只执行冻结 host tools，Shader target 与 tool execution target 独立。
Player 从内容 store 读取编译产物，不引用本项目；没有旧平台枚举或三个封闭内容工厂。

`ToolRunner()` 只消费应用部署索引中的逻辑工具名，不根据当前程序集的 DEBUG 符号选择文件。显式 `ToolRunner(executables, includesDirectory)` 借用同一个受保护工具产物的执行文件与 Shader include；调用者持有其读取 owner 至全部编译结束。`BgfxAuthoringProvider` 将同一 runner 交给 Shader 和纹理编译器。缺工具或 include 明确失败，不向仓库的 extern 目录回退。

## 源码函数前端

定义和原型都禁止 `main()`；resolver 的共同 retirement barrier 原样传播，不能降格成普通 include 诊断。
跨实现/变体接口一致性与完整输入快照由 [`ShaderSourceFrontendCatalog.AnalyzeModule`](../../rendering/Inno.Rendering.Shaders.md)
负责，BGFX 前端只分析其明确给定的一个配置。

前端包含词法、条件预处理与声明分析，不使用正则匹配函数签名。当前已覆盖注释、宏替换和别名重扫描、
token 拼接、include guard/pragma once、常量表达式、结构体、typedef、固定数组、函数声明与
`in/out/inout`。重载歧义、未知类型、非法数组和不平衡声明返回原始文件位置，不猜测接口。

函数体作边界与隐式阶段访问检查，完整合法性由 shaderc 检查；前端经 typed stage 编译进入当前统一 Shader 资产链。
完整 BGFX 原生头文件、所有 storage/image 类型和语言扩展的覆盖仍待后续集成验证，不能把接口单元测试
当作真实 GPU 编译结果。扩展接口见 [Inno.Rendering.Shaders](../../rendering/Inno.Rendering.Shaders.md)。

源码模块禁止非 const 全局绑定；阶段输入、varying 和实际资源槽由图目标/后端拥有。
源码不得隐式读取 `gl_*`、默认阶段 attribute/varying、预定义矩阵或生成绑定名，必须通过公开函数参数传入。
结构成员不被误判为阶段全局变量；目标语言的 native 保留名字不作为模块的局部变量名字使用。
这种职责划分也符合 BGFX 对阶段入口的要求。Windows D3D 编译需在 Windows 执行，不能由 macOS
结果替代：[BGFX shaderc 文档](https://bkaradzic.github.io/bgfx/tools.html#building-shaders)。

## 前端组合示例

创建 backend 前端并传入候选源 resolver；语言的解析规则由这个具体实现负责。

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

## Typed stage 编译

`BgfxShadercToolchain.supportedSourceLanguages` 声明支持的语言；
`CompileAsync(ShaderStageToolRequest, CancellationToken)` 消费 `ShaderIrStage`，生成入口/接口/绑定与源码函数调用，
复用现有 shaderc 进程执行器，返回不可变 bytes、逻辑到 native 绑定名及结构化诊断。核心不保存 SC 表达式。

- 私有函数/常量按冻结模块内容隔离；结构体按完整布局共享 native 类型，普通运算与函数调用共用值流。
- 只读取已冻结 include 解析边，不在后台重新使用活动 resolver 或猜测 include 别名。
- 无 varying 的阶段写入明确的无声明文件，避免 shaderc 对空文件误报警；不是插入伪造 input。
- shaderc 预处理会去掉 `#line`。此 Adapter 通过保留源码标记及原生错误代码摘录，恢复原始文件/行；无法精确映射的诊断保留 generated 位置，不伪造源行。
- 不把 exit code 0 当成唯一成功条件；原生明确 ERROR 或缺失 bytes 一样拒绝候选。
- 绑定反射属于具体工具链。OpenGL/OpenGLES 从 shaderc 的已编译 GLSL payload 读取实际 uniform 声明及引用，复用现有词法器处理 layout/precision；原生 scanner 遗漏的绑定按生成 IR 补入 BGFX 二进制 uniform 表，使引擎 manifest 与 BGFX 自身反射一致。保留存活记录的原生 metadata，删除只声明而未使用的记录；排序确定，采样器、矩阵及固定数组遵循当前 BGFX ABI。其他 backend 继续读取原生反射表。损坏的长度、终止符、UTF-8 或无法表达的存储返回 `BGFX_SHADER_REFLECTION`，不回退到未裁剪接口，也不放宽 Runtime 绑定校验。
- 原生 stdout/stderr 的格式解析只在 BGFX 工具链；公共 `ShaderCompiler` 仅消费结构化 `ShaderDiagnostic`，不再按 shaderc 行格式做正则判断。
- 支持 typed 纹理采样、显式 LOD、discard、buffer load/store/atomic-add 以及 2D/array/3D image load/store。
  BGFX 当前运行时只在 compute 暴露 storage 绑定，因此该限制由 Adapter 明确诊断；能力位、格式访问权限和 slot 上限均在调用 shaderc 前检查。
- Buffer 当前接受明确 stride 的 32-bit 标量、2/4 分量向量；任意结构体 packing、资源函数参数解析与屏障仍需完整 Target/源码前端接线，不能按主机结构体布局猜测。
- Branch/Loop 生成真实控制流，区域内的内存操作不提升到外部；循环先捕获全部 carried state 再同时替换，保证交换值等操作的含义。
- IR 矩阵统一按列构造/提取，Adapter 根据 `BGFX_SHADER_MATRIX_COLUMN_MAJOR` 映射下标和非方阵形状，不能直接把 SC/HLSL 行索引当作列。
- 当前 BGFX uniform storage 为 vec4/mat3/mat4 及固定数组，标量参数须由 Target 做显式 packing/component lowering。
- 现代桌面 GLSL 多颜色输出由 IR 生成器声明显式 attachment location，包含稀疏槽位；避免 shaderc 原样保留已移除的 `gl_FragData` 而触发 GPU 编译失败。ESSL、HLSL、Metal 和 SPIR-V 继续使用 shaderc 的对应输出 lowering，公共 Shader IR 不依赖这些语言差异。
- 数组 typedef、全部原生语法/高级资源及完整反射覆盖仍有边界；明确错误不是兼容旁路，不能据此宣称全部第三方语法已支持。

## 工具进程入口

`ToolRunner.Run(tool, arguments, workingDirectory)` 和
`RunAsync(tool, arguments, workingDirectory, cancellationToken)` 使用参数数组启动明确部署的工具。
取消先停止子进程树，再等待退出并观察两条输出读取任务。
`ToolRunResult` 保存 `exitCode`、`standardOutput`、`standardError` 与 `succeeded`。
非零退出码交给调用方转换为构建诊断；工具缺失抛出 `FileNotFoundException`。
原生加载器不搜索仓库或隐式部署工具。

```csharp
using Inno.Build.Toolchains.Bgfx.Tools;

var runner = new ToolRunner();
ToolRunResult result = runner.Run(BgfxTool.Shaderc, ["--help"]);
```

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Inno.Build.Toolchains.Bgfx.Tools.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider e55a300a420e61d8f7a758e454bf0b0f7ba202139dad8fcd75ce847a7b8a27ad -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.#ctor(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) 03bd06c69aed21fddd87aa964349830b104761dbb5fd86ac2389049fe29b83cd -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateShaderCompilerToolchain ec6ea79d05dc49f1183023059a88aa8ea88e8cce45f3a750c384c6232c17b22e -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateTextureTargetCompiler 96348f921ef504eea98b9ffe1110f3eb64b8612312d987abdd9bbb648bf79ea8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9e8ba34a304ce5c8"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.BgfxAuthoringProvider(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile target, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L25) | Registers the bundled rendering authoring tools without initializing a compiler.<br>`target`：The product's explicit preview shader target.<br>`tools`：Frozen executable tools, or null for the product's own deployed tools. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-47453aa4cd1c73ed"></a>[`public override Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateShaderCompilerToolchain()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L35) | Creates the shader toolchain for this implementation.<br>返回：A compiler compatible with devices registered under the same identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-4540f4c38ab65e64"></a>[`public override Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateTextureTargetCompiler()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L38) | Creates the texture compiler for this implementation.<br>返回：A compiler compatible with devices registered under the same identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-77c1421c57babc29"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider : Inno.Adapter.Rendering.RenderingAuthoringBackendProvider`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L11) | Registers the BGFX authoring compiler and texture target compiler. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler dcce377a81c4d60d5ccdbc259a32a5fd10d3800462719f7c6139e41376e8eaaf -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog,Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,System.Collections.Generic.IEnumerable{Inno.Rendering.GraphicsApi},Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) c553ad199b547b78ccd76ca6a0dacbe91ae584f530f8c1839b7b56672ad92847 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.target e21b7dd85c5d15c6883614e1b35ccf64fb970e91d4d6c641e5f0be5539cf82a5 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext,System.Threading.CancellationToken) c61c844d6f3d861d0f022e012f5310c2deca1916be0c75a8a5c45a601176e391 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-472f447d588f4442"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.BgfxGameContentCompiler(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, System.Collections.Generic.IEnumerable<Inno.Rendering.GraphicsApi> backends, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner tools)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L56) | Creates a compiler using explicit target profiles and the selected runtime API closure.<br>`assets`：The active authoring asset owner.<br>`serialization`：The matching serialization registry.<br>`types`：The matching extension catalog.<br>`platform`：The immutable platform-contributed shader configuration.<br>`backends`：The nonempty renderer closure to publish.<br>`tools`：Borrowed frozen executables and shader includes; retain their product owner through compilation. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：A renderer is missing, duplicated or not deployable.<br>`System.ArgumentNullException`：A required authoring service, profile, renderer collection, or borrowed tool runner is null. |
| <a id="api-6e36eb00191f3fbf"></a>[`public System.Threading.Tasks.ValueTask Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L105) | Captures the active Asset generation and compiles every required runtime variant.<br>返回：An operation that completes when the source-free target closure is staged.<br>`context`：The target staging path and immutable build generation services.<br>`cancellationToken`：Cancellation observed before every compiler invocation and artifact write. | 约束由所属类型与本页生命周期定义。 | `System.InvalidOperationException`：Thrown when an asset has no stable identity, source mount, or valid target compilation result.<br>`System.ArgumentNullException`：The build context is null. |
| <a id="api-aaea08f13ca11d89"></a>[`public Inno.Build.BuildTargetId Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.target { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L85) | Gets the exact publication target whose content this compiler produces. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-e16bdb3f9f5bdc1d"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler : Inno.Build.IGameContentCompiler`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L20) | Produces source-free BGFX shader and texture artifacts for one Player target. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile 1f8a1309a21e105e72e10c386d79a3f3eea491d25269751a96fc9581bd29461e -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.#ctor(Inno.Rendering.GraphicsCapabilities,System.String,System.String,System.String,System.String,System.Collections.Generic.IEnumerable{System.String}) 4612cd4b7a574b0d7d27a44d826c80a93bd081f940960319186fbe9019886cbe -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.capabilities 246826a47d2ff4494f7a0e419e8a791dbb86b82dd68a1cbdc7a4a097fd366d5f -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.shadercPlatform 179b26ad319a0d0785bf2f28a8244bdc9f0a9b70e58d540a2782b07ba06e3129 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.vertexProfile 3b695f264402c933cc47324de25e726b94af660e9944e579fcc4315ba957d958 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.fragmentProfile 5be66e599ce2b82c12060879ff647578af99cb4aa772d260b5240dd77069aa21 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.computeProfile 164d78d2461cf51b9915728e67a3f3b78115795dc3e374b51966c4c72e1d5ca9 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.defines 767306fa45970b85f1049b115294835b871c4e95c0aea009e8d5ea3479fe5759 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.key c41c65a3b3ec143c62ddc32872ddd88aba4e3869d9ab3ec494055a512126259e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-999a6bef93189438"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.BgfxShaderCompilerProfile(Inno.Rendering.GraphicsCapabilities capabilities, string shadercPlatform, string vertexProfile, string fragmentProfile, string computeProfile, System.Collections.Generic.IEnumerable<string>? defines = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L43) | Validates and snapshots platform-supplied compiler inputs.<br>`capabilities`：The immutable target capabilities.<br>`shadercPlatform`：The explicit vendor platform argument.<br>`vertexProfile`：The vertex shader dialect.<br>`fragmentProfile`：The fragment shader dialect.<br>`computeProfile`：The compute dialect, or empty when compute is unsupported.<br>`defines`：Optional ordered defines; copied on construction. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：A required string is blank, a define is null, or the compute declaration disagrees with capabilities.<br>`System.ArgumentNullException`：Capabilities are null. |
| <a id="api-df7d188f5f86c509"></a>[`public Inno.Rendering.GraphicsCapabilities Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.capabilities { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L72) | Gets the immutable offline validation capabilities. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7b3e08863dbfe92f"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.computeProfile { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L92) | Gets the compute dialect, or an empty string when compute is unavailable. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f2a4ebcf92e083ef"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.defines { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L97) | Gets immutable ordered compiler defines. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-33bd191d6a2446b8"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.fragmentProfile { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L87) | Gets the fragment stage dialect. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-0d5b91811b46929b"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.key { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L102) | Gets a deterministic identity covering dialects, defines and offline capabilities. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ad479c39ae115073"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.shadercPlatform { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L77) | Gets the vendor shader compiler platform argument. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a25fdb7b13b80ceb"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.vertexProfile { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L82) | Gets the vertex stage dialect. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a56475f1d023b63d"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L14) | Freezes one renderer's vendor dialect, capability facts and semantic compiler defines. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend cc12d214da115d95b7c042fd6e603743a9c308d38dca8addbe94e8da5b876a32 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.languageId 1ee84a7004d36e61a30fb22c24383c826ec52974ebd0a67bdb3a46f31955bdea -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest) 429b82a8203c90fe73dc572bad18fca0a0f28fc1ac56bd65653addf5c49f4455 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.#ctor 63c8e7a89f8d94f56175b9ba34a85bde3d4ffee61476f68577ad458af5a91256 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c6895ed9b4e43fad"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.BgfxShaderSourceFrontend()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L13) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-99e5ba5b8ca6a3db"></a>[`public Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L29) | Analyzes source text and returns validated output with diagnostics.<br>返回：The validated shader source analysis that represents the completed operation.<br>`request`：The validated immutable request that defines this operation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-481240d036783368"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.languageId { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L18) | Gets the language id text used by the current instance. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-9ea9da92ba911a42"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend : Inno.Rendering.Shaders.IShaderSourceFrontend`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L13) | Parses BGFX SC function modules without placing BGFX grammar in the common shader model. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile 102c5c2c9e018a6e7c92f40d1acccdc2f65b5a4a08e44787bba9c9b2b20d6735 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.#ctor(System.String,System.Collections.Generic.IEnumerable{Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile}) df2965b89b44729172c6eafb3863feee53bfb51fb5efd4b9a90280feae6af624 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.id 69f41037efe4b177b120d6bb5238066f049422b6aa9f4475d328df596e4c8366 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.Resolve(Inno.Rendering.GraphicsApi) da87f42b09131b7039ae15650ccc15fb5b2ed64a859ea2e8f840e7aba600f507 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c09c7980570c53b0"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.BgfxShaderTargetProfile(string id, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile> renderers)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L27) | Freezes the renderer configuration contributed by the platform.<br>`id`：The open stable target identity.<br>`renderers`：One immutable profile per supported graphics API. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The identity, renderer collection or a renderer entry is empty or duplicated. |
| <a id="api-c720bdf8d276b5c2"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.Resolve(Inno.Rendering.GraphicsApi backend)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L60) | Resolves renderer configuration without choosing a platform or executing tools.<br>返回：The borrowed immutable profile; unsupported APIs fail explicitly.<br>`backend`：The explicit graphics API. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The target has no configuration for this API. |
| <a id="api-58ef23b04eb1380c"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.id { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L46) | Gets the explicit stable target identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7e8fa095fbed9d67"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L11) | Binds an open target identity to immutable renderer profiles without a backend-owned platform list. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain 15358ba466c46a14b8c64a34aaf7985a61d08c8694322cc21576f57edebf62a0 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.#ctor(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) d20bb3a622f75ccbf546bebc05a3e84f2b7e78905823254ca1e287cb053475ef -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CreateTarget(Inno.Rendering.GraphicsCapabilities,System.Boolean,System.Boolean) 5d07d74fbb179f82962825066d514fd759185eb59fb609ec571296898ef8c71c -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.supportedSourceLanguages 064a3b865dd1f1881cf1f7b3748bd72c6affbdea8da5a860742fe9a8e75c60bf -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.implementationId acca7fb3aa275076be79c3c77888aeb503ab8d1aeef05ad6f4f6df719f100077 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CompileAsync(Inno.Rendering.Assets.Authoring.ShaderStageToolRequest,System.Threading.CancellationToken) f6839afaded398c6c6799197c4941b94907d9f9c8820eeb1d27dc9fa26752d50 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c28d7aa30a4ba2f3"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.BgfxShadercToolchain(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile targetPlatform, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L30) | Creates a compiler for one explicit offline target platform.<br>`targetPlatform`：Target platform whose shaderc profiles are required.<br>`tools`：Frozen host-selected executables, or null to use the application's explicit native deployment. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-bc0c98354605bad9"></a>[`public System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderStageToolResult> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CompileAsync(Inno.Rendering.Assets.Authoring.ShaderStageToolRequest request, System.Threading.CancellationToken cancellationToken)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L51) | Compiles the supplied source into a validated runtime artifact.<br>返回：An asynchronous operation that completes after all requested work has finished.<br>`request`：The validated immutable request that defines this operation.<br>`cancellationToken`：The token that cancels the operation before it commits. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-e5e1c3fbfdac267d"></a>[`public Inno.Rendering.Assets.Authoring.ShaderCompileTarget Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CreateTarget(Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L54) | Creates a target using this implementation's validated inputs.<br>返回：The validated shader compile target that represents the completed operation.<br>`capabilities`：The immutable graphics capabilities used to validate the operation.<br>`optimize`：Whether the compiler should optimize the generated runtime artifact.<br>`debugInformation`：Whether the compiler should retain target debug information. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-488602f3370aef89"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.implementationId { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L37) | Gets the implementation id text used by the current instance. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-380644fcc83943c3"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.supportedSourceLanguages { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L32) | Gets shader source languages accepted by this toolchain. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-08ef2d6d5b9c44f2"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain : Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L16) | Compiles common Shader IR stages with the BGFX shaderc toolchain. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities 9bb6198c63f9533b7a5b38c60fbb96e9ab4b4cbcf7bc154752dc019cb3932ede -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities.Create(Inno.Rendering.GraphicsApi) ca6f81d35c3fbca80bb341263f7861d7b5e0b7237d8946c62abc51b4bc67243f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-927405ea67d88a2d"></a>[`public static Inno.Rendering.GraphicsCapabilities Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities.Create(Inno.Rendering.GraphicsApi backend)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L21) | Creates the limits and format set used to validate one target's shader output.<br>返回：The immutable offline validation profile for the selected target.<br>`backend`：The renderer whose shader dialect is being compiled. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-afc2a00b87a341e8"></a>[`public static class Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L10) | Supplies one consistent offline capability profile to all BGFX target compilers. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler 5ba6e77e71892ea29769333983da1bbd85a8cb74110d022482d658564776a75e -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.#ctor(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) 4dc22d20017f0360e841b3fabe32b9af70736d448c2db88863d33b2c0956d936 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.CompileKtxAsync(System.IO.Stream,Inno.Rendering.Assets.TextureColorSpace,System.Threading.CancellationToken) 788910690ba52482aef632f5d5eb4077b131e8f0e1a7cf7a5d06d60477626cc7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7e95c7b5d11f9957"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.BgfxTextureTargetCompiler(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L25) | Creates a texture compiler using frozen host tools or the application's explicit native deployment.<br>`tools`：The selected tool runner, or null for application-deployed tools. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-977a1697b3de170c"></a>[`public System.Threading.Tasks.ValueTask<byte[]> Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.CompileKtxAsync(System.IO.Stream source, Inno.Rendering.Assets.TextureColorSpace colorSpace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L42) | Compiles the supplied source into a validated runtime artifact.<br>返回：An asynchronous operation that completes after all requested work has finished.<br>`source`：The borrowed encoded image stream; only this toolchain materializes a short-lived compiler input.<br>`colorSpace`：The color transfer convention preserved in the compiled texture artifact.<br>`cancellationToken`：The token that cancels the operation before it commits. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-eb45c025d66ab90a"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler : Inno.Rendering.Assets.Authoring.ITextureTargetCompiler`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L15) | Converts artist texture sources into validated KTX containers with BGFX texturec. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTool`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool 972ebfac61384cc9872a948470c008eb893bbb2386dc80a1927a6a3d7be32ca6 -->
<!-- api: F:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Shaderc 4d0f8b5a806cd683ca640e27cec6b15f6be6952df0d3ca998a8112e515c53502 -->
<!-- api: F:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryc a11dc0d171494b49394360cc1a6a3b502d675821b4fb651fb28b41b8fa695ded -->
<!-- api: F:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryv 07b0fa8f1d6134564392135ccb975f0bdbdd462c8ee320c6beaf0f7893c481a8 -->
<!-- api: F:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturec 5aeb4f19681c478750f4f25f9db570d7b12b7e3cca55a0edf2cd87c0e70bbee9 -->
<!-- api: F:Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturev 749032bc95d9d600c5f05d56871641dcc085cd1fa8c13a34b46a74b533f64076 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-609aee0d8dbeda4c"></a>[`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryc = 1`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L15) | Geometry compiler. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-3155dab9a986ac7a"></a>[`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryv = 2`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L19) | Geometry validator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-e5776f1f813b51b3"></a>[`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Shaderc = 0`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L11) | Shader compiler. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-57fed15c3dbd1ddc"></a>[`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturec = 3`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L23) | Texture compiler. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-2d8813184007e242"></a>[`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturev = 4`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L27) | Texture validator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7915d2f0694eace1"></a>[`public enum Inno.Build.Toolchains.Bgfx.Tools.BgfxTool : int`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L6) | Known bgfx tool executables. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult 0ba8f09f6aa78bc1f218c249ca9e2533846b7b1d6c3a522002fe3acc20163ade -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.#ctor(System.Int32,System.String,System.String) 9cabb2ad50916e2e99c68acdfe4127871abe3836a8b163a29d32bfb99ad0be15 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.exitCode 0b50e8d4391911bed3d299d3c9a9c1168397a6684a5dc4479ca4847b8bb1ff67 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardOutput 750011102f7c3bfcb9281df1d97a85452194e3322bb2fe4053c7f5f94067560e -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardError 407416fab6e6232da1c97fb9538837a02fdec246a4ade4cf20e6499afd8fb757 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.succeeded 75fdcc6dda56ed343621d194eb06b7dcb5f8c311e4906bbe7cd32316e42d9655 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7e994d0b146b431d"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.ToolRunResult(int exitCode, string standardOutput, string standardError)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L20) | Creates a tool invocation result.<br>`exitCode`：Native process exit code.<br>`standardOutput`：Captured standard output.<br>`standardError`：Captured standard error. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-872f664ce9ddbea4"></a>[`public int Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.exitCode { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L33) | Gets the native process exit code. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-65406d4e05c85e31"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardError { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L43) | Gets captured standard error. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-377eb5e56a748d74"></a>[`public string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardOutput { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L38) | Gets captured standard output. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-72f03983ef338d50"></a>[`public bool Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.succeeded { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L48) | Gets whether the tool exited successfully. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-72b18b2236d3c5a8"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L6) | Contains the immutable result of one bgfx tool invocation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Tools.ToolRunner 1a4e2889bf9c9b1ae749d340eff82640ac2e2ee8f9ab1799d92bbf0604a57e6e -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.#ctor 180138d1fd11c2fddb7a2fed112c9345d8a1e4fba3f691c6f76229a85e338d2d -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.#ctor(System.Collections.Generic.IReadOnlyDictionary{Inno.Build.Toolchains.Bgfx.Tools.BgfxTool,System.String},System.String) dbe11a932f9d28093142fa7fb47b796f338828b681980d894d1304afd7d1855f -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.Run(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool,System.Collections.Generic.IReadOnlyList{System.String},System.String) 62085ea243d4fc22f5d54cfcc04d4a26d84bf0fcfe17dcecc5629b55f13fc994 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.RunAsync(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool,System.Collections.Generic.IReadOnlyList{System.String},System.String,System.Threading.CancellationToken) 9599da4c871a263f617abe84b32df793eeacb222187816d786edd5a4d68781f5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6a0ced579415edd7"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L25) | Creates a runner for tools explicitly deployed with the current application. Resolution is deferred until execution; constructing a compiler does not start or discover a process. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c2f83a2109df6a1e"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner(System.Collections.Generic.IReadOnlyDictionary<Inno.Build.Toolchains.Bgfx.Tools.BgfxTool, string> executables, string includesDirectory)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L42) | Freezes executables selected and validated by the host's toolchain operation.<br>`executables`：Tool identities mapped to existing absolute executable paths; the collection is copied.<br>`includesDirectory`：The absolute shader include directory belonging to the same protected tool product. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：An identity is unsupported or an executable path is relative.<br>`System.IO.FileNotFoundException`：A declared executable is unavailable. |
| <a id="api-d527e22134ad37b1"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.Run(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L78) | Executes the configured workflow and returns its process outcome.<br>返回：The exit code and captured output.<br>`tool`：Tool to execute.<br>`arguments`：Individual command-line arguments without shell quoting.<br>`workingDirectory`：Optional working directory; defaults to P:System.AppContext.BaseDirectory. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c5a9cf30dc2459b8"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult> Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.RunAsync(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L112) | Runs the external tool asynchronously and captures its complete process outcome.<br>返回：The exit code and captured output.<br>`tool`：Tool to execute.<br>`arguments`：Individual command-line arguments without shell quoting.<br>`workingDirectory`：Optional working directory; defaults to P:System.AppContext.BaseDirectory.<br>`cancellationToken`：Cancellation that terminates the child process tree. | 约束由所属类型与本页生命周期定义。 | `System.IO.FileNotFoundException`：Thrown when the requested bgfx tool cannot be resolved.<br>`System.InvalidOperationException`：Thrown when the native process cannot be started.<br>`System.OperationCanceledException`：Cancellation stopped the process tree and all redirected output has been drained. |
| <a id="api-aaeacc262300ec5a"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L16) | Runs bgfx tool executables from the native output with argument-safe process invocation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.Bgfx](Inno.Native.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Shaders](../../rendering/Inno.Rendering.Shaders.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](../../rendering/Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets.Authoring](../../rendering/Inno.Rendering.Assets.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering.Authoring](../../rendering/Inno.Adapter.Rendering.Authoring.md)：公开引用边界由实际签名核对。
