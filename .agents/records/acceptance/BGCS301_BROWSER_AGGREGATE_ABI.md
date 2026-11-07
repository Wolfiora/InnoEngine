# BGCS 3.0.1 Browser Wasm 按值结构体调用问题

## 实际失败

2026-10-20，Windows x64 宿主通过 NuGet BGCS 3.0.1、SDK 10.0.402 所选 Emscripten 工具链执行正式入口：

```powershell
dotnet entry/InnoEngine.Build/bin/Release/net10.0/InnoEngine.Build.dll bindings `
  --engine-root C:/Dev/GameEngineDev/InnoEngine `
  --target browser-wasm --tools-target windows-x64 --product player `
  --configuration Release --dotnet C:/Users/23842/.dotnet/dotnet.exe
```

CMake 与 Ninja 使用已安装工具，在本次调用进程的 PATH 中明确提供。SDK、clang/clang++、sysroot、Node、Python 由 Browser provider 从实际 .NET workload 取得。没有使用 Windows 头文件或替换 Browser runtime。

当前 BGFX 配置使用正式目标 `emscripten-wasm32-emscripten` 和 `api.runtime = browserWasm`。正式生成在 C# ABI 规划阶段失败：

```text
BGCS-CSHARP-ABI-PLAN:
By-value use of 'struct bgfx_uniform_handle_s' requires independently verified
managed calling-ABI evidence or a pointer bridge.
The managed runtime has no implemented aggregate calling profile.
```

完整日志：`artifacts/acceptance/bgcs301-2026-10-20/browser-bindings-03.log`。此前的常量和 ownership 省略诊断不是这个失败的根因。失败没有发布不完整候选。

## 代码依据与范围

BGCS 的 `src/BGCS.Platforms/ManagedRuntimeResolver.cs` 在 BrowserWasm 下返回空 aggregate ABI；`src/BGCS.CSharp/Planning/AggregateAbiPlanner.cs` 遇到按值记录要求明确 profile，否则拒绝。`docs/abi-safety.md` 也明确记录 Browser 与其他 CPU 的 aggregate profile 尚未实现。相同大小或字段布局不等于调用 ABI 已证明。

这项问题阻塞当前 Browser Player 绑定闭包，因而阻塞其 Mono Wasm / Wasm AOT 最终构建与运行验收。Windows x64 已完成七组件正式生成及完整八组件原生支持准备，不能把 Browser 的失败推广成所有目标失败。

当前 SDK 源码的 aggregate profile 还限制在 x64。macOS ARM64 的 C 按值记录需要同样核对，但本轮没有实际 Mac 设备结果，不能把源码推断写成该设备已复现。

2026-10-21 又从实际还原的 3.0.1 `BGCS.Platforms.dll` 调用公开 `ManagedRuntimeResolver.Resolve`：BrowserWasm 与 macOS ARM64 CoreCLR 的 `aggregateAbi` 均为空，Windows x64 CoreCLR 非空。证据为 `bgcs-published-runtime-capability-final-2026-10-21.json`。这是正式二进制能力查询，不涉及修改 SDK，也不替代真实 Mac 生成、编译或运行；此前仅加载 Host assembly 的探针未找到该类型，已明确记为未评估，不使用其空值作结论。

## 理想解决边界

在 BGCS 的正式管线内实现以下一种完整能力，并由公开配置或目标能力选择：

1. 为真实 .NET 10 Browser Mono/Wasm AOT 实现并验证正确的结构体调用降级；或者
2. 由 SDK 为 C 按值参数及返回值生成明确的 pointer bridge，进入正式 native manifest、静态 adapter、缓存与联合发布协议。不能只处理 C++ facade 而遗漏 C API。

不能把 Browser 标成 CoreCLR、注入 Windows x64 aggregate profile、按 sizeof 猜测、关闭 ABI 检查或手改生成绑定。引擎共同层不增加平台 switch、库专用桥接表或平行 import。若 SDK 选择生成桥接，最终 Native 编译继续消费其经过验证的 manifest。

## SDK 验收要求

- 覆盖 C 单字段小记录、混合字段、嵌套记录及按值参数/返回值；包含 BGFX handle 的实际调用。
- 同一发布 revision 通过 Browser Mono Wasm 和 Wasm AOT 的真实编译、链接及运行，验证静态导出表和函数指针 transport。
- 检查目标宽度、对齐、调用及回调语义，错误或未知 ABI 仍明确失败。
- 冷/热生成、CheckOnly、配置或工具变化失效、失败/取消不发布半成品。
- 生成原生和 managed 输出均进入正式 manifest，移动到冻结复制树后仍可编译，不在引擎后处理生成文本。
- Windows x64 和可用的 macOS ARM64 同 revision 回归分别报告，不由 Browser 结果推断。

## 引擎重新验收条件

消费包含上述能力的正式 BGCS 包后，重新执行 Browser 五组件生成、冻结原生聚合、静态闭包验证、Mono Wasm / Wasm AOT 发布和实际启动、输入、存储、帧推进及退出检查。当前此项记为失败且由 SDK 能力阻塞，不能记为通过。

## 2026-10-22 再复验

实际 workload 与已配置工具下再次出现相同 BGFX aggregate ABI 拒绝，宏常量诊断一并保留；[本次记录](PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md)包含命令证据和扩展协议边界。未修改 SDK 或其包内容。
