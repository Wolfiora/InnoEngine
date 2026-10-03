# Inno.Build.Toolchains.Browser

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Native 绑定](../native/BindingGeneration.md) · [Support Pack](Inno.Build.SupportPacks.md)

## 职责与边界

WebAssembly 构建目标库，没有 Program，不包含游戏、Scene、渲染算法或另一套 Native 实现。使用 .NET wasm-tools 的 Emscripten 编译稳定 Native 项目和相同第三方源码。所有 CMake 中间产物归本项目 obj/native/browser-wasm，各组件发布 archive 位于 .lib/component/browser-wasm。

## 编译并行度

Web 运行时的单线程能力不限制编译并行度。工具链不覆盖 `EMCC_CORES`、`BINARYEN_CORES`，CMake build
也不固定单个 worker；宿主可通过已有工具环境变量（包括 `CMAKE_BUILD_PARALLEL_LEVEL`）限制资源，未设置时采用工具默认策略。
验证时为避免影响前台游戏而设置的并行预算只属于验证进程，不写入产品默认值。

## 所有 public API

| API | 语义 |
| --- | --- |
| `BrowserToolchain.ResolveEnvironmentAsync(dotnetHost, projectPath, cancellationToken)` | 查询目标项目的 MSBuild workload 选择，返回该项目实际使用的 SDK、Cache、Node 和 Python 路径；不猜测已安装包的最高版本，不修改父进程环境。 |
| `BrowserToolchain.BuildAsync(engineRoot, dotnetHost, cancellationToken)` | 生成五套目标 binding、顺序编译并安装静态 archive，失败及取消传播。 |

没有 protected 扩展点。

```csharp
using Inno.Build.Toolchains.Browser;

await BrowserToolchain.BuildAsync(engineRoot, dotnetHost, cancellationToken);
```

需要 .NET 9 wasm-tools、CMake 和 Ninja。macOS/Linux 使用本机 python3；Windows 使用 workload 携带的 Python。工具链不保存开发机器的绝对路径。

返回环境还显式设置 `BGCS_CC` 与 `BGCS_CPP2C_CXX` 为选中 SDK 的 clang/clang++，
生成、fingerprint、Cpp2C native 验证与最终链接使用同一目标编译器。
这两个变量仅进入子进程，不覆盖开发者的系统 LLVM 或父进程环境。
BGCS 解析器自己的 Clang builtin resource headers 由解析器包提供，SDK 负责目标 sysroot 与标准库。

## 约束与失败

SDK、sysroot 或工具缺失立即失败。BGFX 使用 SDK 的版本头获取编译宏，明确构建 GLES 3.0 对应 WebGL 2；不修改 extern。wasm_sjlj_shim.c 是已记录的 LLVM/Emscripten lowering shim，属于目标工具链；没有第二套业务 ABI。编译一次只启动一个 Native 编译进程，取消杀死子进程树。

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
