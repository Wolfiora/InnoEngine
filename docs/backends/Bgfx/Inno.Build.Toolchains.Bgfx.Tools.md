# Inno.Build.Toolchains.Bgfx.Tools

[分类索引](README.md) · [Rendering Assets](../../rendering/Inno.Rendering.Assets.md) · [Wiki 首页](../../README.md)

## 公开 API 与平台边界

- `BgfxShaderTargetProfile(id, renderers)`：开放目标身份及不可变 renderer catalog，重复 renderer 或空集合明确失败。
- `BgfxShaderCompilerProfile(capabilities, shadercPlatform, vertexProfile, fragmentProfile, computeProfile, defines)`：冻结编译方言、能力和 defines；`key` 包含全部输入，调用方修改原集合不会改变它。
- `BgfxShadercToolchain(profile, tools)`：只执行配置指定的编译，未知引擎目标无需增加 backend switch。
- `BgfxAuthoringProvider(profile, tools)`：向创作组合注册同一 compiler。
- `BgfxGameContentCompiler(assets, serialization, types, profile, backends)`：构造时验证完整 renderer 闭包，`CompileAsync` 输出 source-free Shader 与 portable KTX。
- `BgfxTargetCapabilities.Create(backend)`：提供可复用的完整离线能力集合。平台可以直接提供自己的更严格 `GraphicsCapabilities`。
- `BgfxTextureTargetCompiler`：离线纹理编译。
- `BgfxShaderSourceFrontend`：显式 BGFX SC 前端；具体语言语义属于 backend。

Windows、macOS、Browser 的实际 Shader profile 分别由所属平台的 `*BgfxShaderProfiles.target` 提供。
Browser 使用 `asm.js` / `300_es` 和 WebGL 2 能力集合，拒绝 Compute；macOS Vulkan 所需
`BGFX_SHADER_LANGUAGE_SPIRV=1` 由其配置声明。共享 compiler 不识别命名平台或维护平台表。
配置改变会进入内容编译和内置 Shader 指纹，不复用旧 dialect、能力或 defines 的产物。

这些类型属于 authoring/build。`ToolRunner` 只执行冻结 host tools，Shader target 与 tool execution target 独立。
Player 从内容 store 读取编译产物，不引用本项目；没有旧平台枚举或三个封闭内容工厂。

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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L11) | Registers the BGFX authoring compiler and texture target compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.BgfxAuthoringProvider(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile target, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L25) | Registers the bundled rendering authoring tools without initializing a compiler. |
| [`override Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateShaderCompilerToolchain()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L35) | See the implemented contract. |
| [`override Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Build.Toolchains.Bgfx.Tools.BgfxAuthoringProvider.CreateTextureTargetCompiler()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxAuthoringProvider.cs#L38) | See the implemented contract. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.target`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L75) | See the implemented contract. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L20) | Produces source-free BGFX shader and texture artifacts for one Player target. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.BgfxGameContentCompiler(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, System.Collections.Generic.IEnumerable<Inno.Rendering.GraphicsApi> backends)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L49) | Creates a compiler using explicit target profiles and the selected runtime API closure. |
| [`System.Threading.Tasks.ValueTask Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L95) | Captures the active Asset generation and compiles every required runtime variant. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L14) | Freezes one renderer's vendor dialect, capability facts and semantic compiler defines. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.BgfxShaderCompilerProfile(Inno.Rendering.GraphicsCapabilities capabilities, string shadercPlatform, string vertexProfile, string fragmentProfile, string computeProfile, System.Collections.Generic.IEnumerable<string>? defines = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L43) | Validates and snapshots platform-supplied compiler inputs. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.capabilities`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L72) | Gets the immutable offline validation capabilities. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.defines`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L97) | Gets immutable ordered compiler defines. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.computeProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L92) | Gets the compute dialect, or an empty string when compute is unavailable. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.fragmentProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L87) | Gets the fragment stage dialect. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.key`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L102) | Gets a deterministic identity covering dialects, defines and offline capabilities. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.shadercPlatform`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L77) | Gets the vendor shader compiler platform argument. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile.vertexProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderCompilerProfile.cs#L82) | Gets the vertex stage dialect. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L13) | Parses BGFX SC function modules without placing BGFX grammar in the common shader model. |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L29) | Analyzes source text and returns validated output with diagnostics. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.languageId`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L18) | Gets the language id text used by the current instance. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.Resolve(Inno.Rendering.GraphicsApi backend)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L60) | Resolves renderer configuration without choosing a platform or executing tools. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L11) | Binds an open target identity to immutable renderer profiles without a backend-owned platform list. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.BgfxShaderTargetProfile(string id, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderCompilerProfile> renderers)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L27) | Freezes the renderer configuration contributed by the platform. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile.id`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShaderTargetProfile.cs#L46) | Gets the explicit stable target identity. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L16) | Compiles common Shader IR stages with the BGFX shaderc toolchain. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.BgfxShadercToolchain(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile targetPlatform, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L30) | Creates a compiler for one explicit offline target platform. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CreateTarget(Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L54) | Creates a target using this implementation's validated inputs. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.supportedSourceLanguages`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L32) | Gets shader source languages accepted by this toolchain. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderStageToolResult> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CompileAsync(Inno.Rendering.Assets.Authoring.ShaderStageToolRequest request, System.Threading.CancellationToken cancellationToken)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L51) | Compiles the supplied source into a validated runtime artifact. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.implementationId`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L37) | Gets the implementation id text used by the current instance. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L10) | Supplies one consistent offline capability profile to all BGFX target compilers. |
| [`static Inno.Rendering.GraphicsCapabilities Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities.Create(Inno.Rendering.GraphicsApi backend)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L21) | Creates the limits and format set used to validate one target's shader output. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L15) | Converts artist texture sources into validated KTX containers with BGFX texturec. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.BgfxTextureTargetCompiler(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L25) | Creates a texture compiler using frozen host tools or the application's explicit native deployment. |
| [`System.Threading.Tasks.ValueTask<byte[]> Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.CompileKtxAsync(System.IO.Stream source, Inno.Rendering.Assets.TextureColorSpace colorSpace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L42) | Compiles the supplied source into a validated runtime artifact. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTool`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L6) | Known bgfx tool executables. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryc`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L15) | Geometry compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryv`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L19) | Geometry validator. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Shaderc`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L11) | Shader compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturec`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L23) | Texture compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturev`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L27) | Texture validator. |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L6) | Contains the immutable result of one bgfx tool invocation. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.ToolRunResult(int exitCode, string standardOutput, string standardError)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L20) | Creates a tool invocation result. |
| [`bool Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.succeeded`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L48) | Gets whether the tool exited successfully. |
| [`int Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.exitCode`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L33) | Gets the native process exit code. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardError`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L43) | Gets captured standard error. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardOutput`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L38) | Gets captured standard output. |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.Run(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L67) | Executes the configured workflow and returns its process outcome. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L15) | Runs bgfx tool executables from the native output with argument-safe process invocation. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L23) | Creates a runner for tools explicitly deployed with the current application. Resolution is deferred until execution; constructing a compiler does not start or discover a process. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner(System.Collections.Generic.IReadOnlyDictionary<Inno.Build.Toolchains.Bgfx.Tools.BgfxTool, string> executables)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L37) | Freezes executables selected and validated by the host's toolchain operation. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult> Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.RunAsync(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L101) | Runs the external tool asynchronously and captures its complete process outcome. |

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
