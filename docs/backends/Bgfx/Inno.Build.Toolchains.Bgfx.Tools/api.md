# Inno.Build.Toolchains.Bgfx.Tools API

[项目入口](../Inno.Build.Toolchains.Bgfx.Tools.md)

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
