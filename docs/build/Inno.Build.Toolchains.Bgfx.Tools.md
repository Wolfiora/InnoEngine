# Inno.Build.Toolchains.Bgfx.Tools

[Build 索引](README.md) · [Rendering Assets](../rendering/Inno.Rendering.Assets.md)

## 公开 API

- `BgfxShadercToolchain`、`BgfxShaderTargetPlatform`：把共享 Shader IR 编译为目标 backend artifact。
- `BgfxTextureTargetCompiler`：把创作纹理离线编译为 portable KTX。
- `BgfxGameContentCompiler`：遍历目标构建 snapshot 并写入 `TargetArtifacts`。
- `BgfxTargetCapabilities.Create(platform, backend)`：为游戏内容与内置 Shader 编译器提供同一套目标能力约束，避免浏览器构建误用桌面 Compute/Storage 能力。
- `BgfxShaderSourceFrontend`：实现 `IShaderSourceFrontend`；`languageId` 为
  `inno.shader-language.bgfx-sc`，`Analyze(ShaderSourceRequest)` 返回函数接口、原始 include 依赖和定位诊断。

这些类型只在 authoring/build 路径使用。工具进程执行器 `BgfxTool`、`ToolRunner` 和 `ToolRunResult` 归属本项目 `Execution/`，与 Shader/Texture 离线编译策略一起留在构建层。Player 通过 `ContentRenderTargetArtifactProvider` 和只读内容 store 读取结果，不引用本项目或 BGFX tools。

`BgfxGameContentCompiler.CreateMacOSArm64` / `CreateWindowsX64` / `CreateBrowserWasm` 接收
`AssetPipeline`、`SerializationRegistry` 和 `TypeCatalog`；`CompileAsync(GameBuildContentContext, cancellationToken)`
写入当前构建事务的目标 staging。上述 public 参数对应的项目引用明确作为公开依赖；Native 与
具体 Asset 实现保持私有实现依赖。

Windows 游戏闭包同时编译 Direct3D11、Direct3D12、Vulkan 和 OpenGL，与该平台内置 Shader
产物及运行时可选 backend 保持一致。缺少任一目标的编译结果即导出失败，运行时不从源码补编。

`BgfxShaderTargetPlatform.BrowserWasm` 使用 shaderc 的 `asm.js` 平台与 `300_es` profile，生成 WebGL 2 的 OpenGL ES 顶点/片段着色器；该 profile 明确拒绝 Compute。`BgfxGameContentCompiler.CreateBrowserWasm` 使用同一目标编译 Shader 与 portable KTX，不向浏览器目标声明 Compute、Storage 或 Indirect 能力。本项目只负责离线内容；浏览器 Player、Native 链接和持久化的实现见 [Web 架构](../architecture/WEB_PLAYER_ARCHITECTURE.md)。

## 源码函数前端

定义和原型都禁止 `main()`；resolver 的共同 retirement barrier 原样传播，不能降格成普通 include 诊断。
跨实现/变体接口一致性与完整输入快照由 [`ShaderSourceFrontendCatalog.AnalyzeModule`](../rendering/Inno.Rendering.Shaders.md)
负责，BGFX 前端只分析其明确给定的一个配置。

前端包含词法、条件预处理与声明分析，不使用正则匹配函数签名。当前已覆盖注释、宏替换和别名重扫描、
token 拼接、include guard/pragma once、常量表达式、结构体、typedef、固定数组、函数声明与
`in/out/inout`。重载歧义、未知类型、非法数组和不平衡声明返回原始文件位置，不猜测接口。

函数体作边界与隐式阶段访问检查，完整合法性由 shaderc 检查；前端已接到新的 typed stage 编译，但资产替换尚未完成。
完整 BGFX 原生头文件、所有 storage/image 类型和语言扩展的覆盖仍待后续集成验证，不能把接口单元测试
当作真实 GPU 编译结果。扩展接口见 [Inno.Rendering.Shaders](../rendering/Inno.Rendering.Shaders.md)。

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
- 数组 typedef、全部原生语法/高级资源及完整反射覆盖仍待补齐；明确错误不是兼容旁路，也不代表完整计划已完成。

## 工具进程入口

`ToolRunner.Run(tool, arguments, workingDirectory)` 和
`RunAsync(tool, arguments, workingDirectory, cancellationToken)` 使用参数数组启动明确部署的工具。
取消先停止子进程树，再等待退出并观察两条输出读取任务。
`ToolRunResult` 保存 `exitCode`、`standardOutput`、`standardError` 与 `succeeded`。
非零退出码交给调用方转换为构建诊断；工具缺失抛出 `FileNotFoundException`。
原生加载器不搜索仓库或隐式部署工具。

```csharp
using Inno.Build.Toolchains.Bgfx.Tools;

ToolRunResult result = ToolRunner.Run(BgfxTool.Shaderc, ["--help"]);
```

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CompileAsync(Inno.Build.GameBuildContentContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L157) | Captures the active Asset generation and compiles every required runtime variant. |
| [`static Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CreateBrowserWasm(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L128) | Creates the WebGL 2 content compiler for a browser WebAssembly Player. |
| [`static Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CreateMacOSArm64(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L68) | Creates the canonical Metal compiler used by Apple Silicon macOS Players. |
| [`static Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler.CreateWindowsX64(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L98) | Creates the canonical compiler used by 64-bit Windows Players. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxGameContentCompiler.cs#L20) | Produces source-free BGFX shader and texture artifacts for one Player target. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Shaders.ShaderSourceAnalysis Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.Analyze(Inno.Rendering.Shaders.ShaderSourceRequest request)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L29) | Analyzes source text and returns validated output with diagnostics. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend.languageId`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L18) | Gets the language id text used by the current instance. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderSourceFrontend`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Sources/BgfxShaderSourceFrontend.cs#L13) | Parses BGFX SC function modules without placing BGFX grammar in the common shader model. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform.BrowserWasm`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L29) | Browser WebAssembly player using WebGL 2. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform.MacOSArm64`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L25) | Apple Silicon macOS player or editor. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform.WindowsX64`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L21) | 64-bit Windows player or editor. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L16) | Identifies a host or offline target supported by the bundled BGFX tools. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.BgfxShadercToolchain()`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L46) | Creates a compiler targeting the current supported host platform. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.BgfxShadercToolchain(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform targetPlatform, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L60) | Creates a compiler for one explicit offline target platform. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderStageToolResult> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CompileAsync(Inno.Rendering.Assets.Authoring.ShaderStageToolRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L51) | Compiles the supplied source into a validated runtime artifact. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.CreateTarget(Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L83) | Creates a target using this implementation's validated inputs. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.implementationId`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L37) | Gets the implementation id text used by the current instance. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain.supportedSourceLanguages`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Intermediate/BgfxShadercToolchain.Typed.cs#L32) | Gets shader source languages accepted by this toolchain. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShadercToolchain`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs#L35) | Compiles common Shader IR stages with the BGFX shaderc toolchain. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Rendering.GraphicsCapabilities Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities.Create(Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform platform, Inno.Rendering.GraphicsApi backend)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L24) | Creates the limits and format set used to validate one target's shader output. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTargetCapabilities`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxTargetCapabilities.cs#L10) | Supplies one consistent offline capability profile to all BGFX target compilers. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.BgfxTextureTargetCompiler(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L25) | Creates a texture compiler using frozen host tools or the application's explicit native deployment. |
| [`System.Threading.Tasks.ValueTask<byte[]> Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler.CompileKtxAsync(System.IO.Stream source, Inno.Rendering.Assets.TextureColorSpace colorSpace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L42) | Compiles the supplied source into a validated runtime artifact. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTextureTargetCompiler`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/BgfxTextureTargetCompiler.cs#L15) | Converts artist texture sources into validated KTX containers with BGFX texturec. |

### `Inno.Build.Toolchains.Bgfx.Tools.BgfxTool`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryc`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L15) | Geometry compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Geometryv`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L19) | Geometry validator. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Shaderc`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L11) | Shader compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturec`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L23) | Texture compiler. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool.Texturev`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L27) | Texture validator. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxTool`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/BgfxTool.cs#L6) | Known bgfx tool executables. |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.ToolRunResult(int exitCode, string standardOutput, string standardError)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L20) | Creates a tool invocation result. |
| [`int Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.exitCode`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L33) | Gets the native process exit code. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardError`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L43) | Gets captured standard error. |
| [`string Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.standardOutput`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L38) | Gets captured standard output. |
| [`bool Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult.succeeded`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L48) | Gets whether the tool exited successfully. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunResult.cs#L6) | Contains the immutable result of one bgfx tool invocation. |

### `Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner()`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L23) | Creates a runner for tools explicitly deployed with the current application. Resolution is deferred until execution; constructing a compiler does not start or discover a process. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.ToolRunner(System.Collections.Generic.IReadOnlyDictionary<Inno.Build.Toolchains.Bgfx.Tools.BgfxTool, string> executables)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L37) | Freezes executables selected and validated by the host's toolchain operation. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.Run(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null)`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L67) | Executes the configured workflow and returns its process outcome. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.Bgfx.Tools.ToolRunResult> Inno.Build.Toolchains.Bgfx.Tools.ToolRunner.RunAsync(Inno.Build.Toolchains.Bgfx.Tools.BgfxTool tool, System.Collections.Generic.IReadOnlyList<string> arguments, string? workingDirectory = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L101) | Runs the external tool asynchronously and captures its complete process outcome. |
| [`Inno.Build.Toolchains.Bgfx.Tools.ToolRunner`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Tools/Execution/ToolRunner.cs#L15) | Runs bgfx tool executables from the native output with argument-safe process invocation. |

## 项目依赖

- [Inno.Native.LibraryLoading](../native/Inno.Native.LibraryLoading.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Native.Bgfx](../native/Inno.Native.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build](Inno.Build.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Shaders](../rendering/Inno.Rendering.Shaders.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
