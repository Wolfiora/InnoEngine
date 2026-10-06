# Inno.Build.Toolchains.Browser

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Native 绑定](../native/BindingGeneration.md) · [Support Pack](Inno.Build.SupportPacks.md)

## 职责与边界

WebAssembly 构建目标库，没有 Program，不包含游戏、Scene、渲染算法或另一套 Native 实现。使用 .NET wasm-tools 的 Emscripten 编译稳定 Native 项目和相同第三方源码。
CMake 中间产物归本项目 `obj/native/browser-wasm/<fingerprint>`，完成的原生闭包位于
`artifacts/native/browser/browser-wasm/<fingerprint>/<component>/browser-wasm`。
指纹包含所选 SDK 的可执行工具与脚本、CMake/Ninja、原生源、工具链实现与五个组件的生成身份；完成后记录精确文件集合和 SHA-256。
读取缓存前验证内容，不从 `.lib` 或其他请求的固定桥目录选择产物。

## 编译并行度

Web 运行时的单线程能力不限制编译并行度。工具链不覆盖 `EMCC_CORES`、`BINARYEN_CORES`，CMake build
也不固定单个 worker；宿主可通过已有工具环境变量（包括 `CMAKE_BUILD_PARALLEL_LEVEL`）限制资源，未设置时采用工具默认策略。
验证时为避免影响前台游戏而设置的并行预算只属于验证进程，不写入产品默认值。

## 所有 public API

| API | 语义 |
| --- | --- |
| `EmscriptenToolchainResolver.ResolveAsync(dotnetHost, projectPath, cancellationToken)` | 查询目标项目的 MSBuild workload 选择，返回该项目实际使用的 SDK、Cache、Node 和 Python 路径；不猜测已安装包的最高版本，不修改父进程环境。 |
| `BrowserToolchain.BuildAsync(engineRoot, dotnetHost, cancellationToken)` | 返回 `Task<BrowserNativeArtifacts>`，生成五套目标 binding，编译并验证静态 archive，失败及取消传播。 |
| `BrowserNativeArtifacts.fingerprint` / `directory` | 本次成功请求的不可变输入身份和完整原生闭包绝对路径；由工具链创建，无公开构造函数。 |
| `BrowserNativeArtifacts.bindingSelectionPath` | 不可变 MSBuild 选择文件，按组件声明已冻结的绑定指纹；托管构建校验当前输入匹配，否则中止 Support Pack。 |

没有 protected 扩展点。

```csharp
using Inno.Build.Toolchains.Browser;

BrowserNativeArtifacts artifacts = await BrowserToolchain.BuildAsync(engineRoot, dotnetHost, cancellationToken);
```

需要 .NET 9 wasm-tools、CMake 和 Ninja。macOS/Linux 使用本机 python3；Windows 使用 workload 携带的 Python。工具链不保存开发机器的绝对路径。

返回环境还显式设置 `BGCS_CC` 与 `BGCS_CPP2C_CXX` 为选中 SDK 的 clang/clang++，
生成、fingerprint、Cpp2C native 验证与最终链接使用同一目标编译器。
这两个变量仅进入子进程，不覆盖开发者的系统 LLVM 或父进程环境。
BGCS 解析器自己的 Clang builtin resource headers 由解析器包提供，SDK 负责目标 sysroot 与标准库。

## 约束与失败

SDK、sysroot 或工具缺失立即失败。BGFX 使用 SDK 的版本头获取编译宏，明确构建 GLES 3.0 对应 WebGL 2；不修改 extern。wasm_sjlj_shim.c 是已记录的 LLVM/Emscripten lowering shim，属于目标工具链；没有第二套业务 ABI。
同一指纹由 `Inno.Core.IO.FileLease` 串行拥有中间树与发布；取消杀死子进程树、删除本次 staging，保留已完成产物。
提交前必须存在全部 11 个非空 archive，并重新验证输入指纹；编译期间输入变化会使候选失败，不能发布为原来的身份。
Support Pack 以返回的路径复制原生库，托管引用的 bin/obj 也包含该指纹，不混用其他 SDK 的程序集。

## SDK 的 SjLj 链接补足

当前 MSBuild 选择的 .NET workload 为 `9.0.20`，Emscripten pack 为 `3.1.56`；该 pack 的实际
clang 报告 `19.1.0`。这些事实与 BGCS 包内 Clang 20 解析器及其 resource headers 是不同层的工具。

以当前 SDK 编译 `setjmp` / `longjmp` 和 `-sSUPPORT_LONGJMP=wasm -fwasm-exceptions` 时，
编译器产生 `__wasm_setjmp` 与 `__wasm_setjmp_test` 引用，随 pack 提供的运行库未定义这两个符号。
`Native/wasm_sjlj_shim.c` 在 Browser 最终静态链接时补充 jump buffer 的 invocation/label 写入和匹配；
`longjmp` 的异常抛出与返回值仍由 SDK 实现。此文件不经 BGCS，也不进入桌面链接或共享 Player/Shell。

2026-10-02 已用实际 SDK 验证：无 shim 的同一 C 程序因上述两个符号缺失失败；加入 shim 后，
嵌套环境、跨中间调用帧跳转、`longjmp(env, 0)` 返回 1、20 次重复 invocation 均通过。
这是显式的 SDK lowering 补足，不能称为完全没有平台专用实现。升级 SDK 时必须重新执行同一链接及行为验证，
若 SDK 已提供这两个符号，应删除此补足并同步 Support Pack source、validator 与链接模板。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.Browser.BrowserNativeArtifacts`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Build.Toolchains.Browser.BrowserNativeArtifacts.bindingSelectionPath`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserNativeArtifacts.cs#L31) | Gets the immutable MSBuild selection file used to reject mismatched managed binding inputs. |
| [`string Inno.Build.Toolchains.Browser.BrowserNativeArtifacts.directory`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserNativeArtifacts.cs#L26) | Gets the absolute immutable install root containing component archive directories. |
| [`string Inno.Build.Toolchains.Browser.BrowserNativeArtifacts.fingerprint`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserNativeArtifacts.cs#L21) | Gets the identity of the SDK, source, configuration and selected binding generations. |
| [`Inno.Build.Toolchains.Browser.BrowserNativeArtifacts`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserNativeArtifacts.cs#L8) | Identifies the immutable native closure produced by one browser toolchain request. |

### `Inno.Build.Toolchains.Browser.BrowserToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.Browser.BrowserNativeArtifacts> Inno.Build.Toolchains.Browser.BrowserToolchain.BuildAsync(string engineRoot, string dotnetHost, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserToolchain.cs#L39) | Builds the native closure from source and publishes complete outputs by immutable input identity. |
| [`Inno.Build.Toolchains.Browser.BrowserToolchain`](../../build/toolchains/Inno.Build.Toolchains.Browser/BrowserToolchain.cs#L15) | Builds static native artifacts with the Emscripten SDK owned by the selected .NET workload. |

### `Inno.Build.Toolchains.Browser.EmscriptenToolchainResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyDictionary<string, string>> Inno.Build.Toolchains.Browser.EmscriptenToolchainResolver.ResolveAsync(string dotnetHost, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Browser/EmscriptenToolchainResolver.cs#L49) | Resolves the native toolchain actually selected by MSBuild for one browser project. |
| [`Inno.Build.Toolchains.Browser.EmscriptenToolchainResolver`](../../build/toolchains/Inno.Build.Toolchains.Browser/EmscriptenToolchainResolver.cs#L13) | Resolves Emscripten from the workload selected by one application project. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
