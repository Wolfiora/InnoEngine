# Inno.Build.Cli

[Build 索引](README.md) · [Wiki 首页](../README.md) · [开发 Build 入口](../entry/InnoEngine.Build.md)

## 职责与边界

统一构建应用库，接收明确命令并组合现有 Build、发行、工具链和创作服务。它由 `InnoEngine.Build` 调用，不是第三个 IDE 日常启动项目。

通用 Build 和运行时不得反向引用 CLI。平台、内容编译器、Native 组件与托管部署的注册属于标准发行组合；命令不维护第二套注册表。

## 快速使用

从仓库根目录运行：

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --configuration Release
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- verify .
```

开发入口补齐宿主、SDK 和 checkout 参数。直接嵌入 `BuildApplication` 时，调用者需要提供相应命令所要求的明确上下文；空参数只显示帮助。

| 命令 | 行为 |
| --- | --- |
| `editor` | 增量构建选定 Editor；可用布尔 `--clean` 先清理所选产品。 |
| `clean --scope product` | 预检 SDK 和目标，清理产品拥有的工作状态；`--dry-run` 预览同一计划。 |
| `bindings` | 从发行声明的产品组件集合准备统一绑定。 |
| `support-pack` | 独立准备并校验目标 Pack，不包含游戏内容。 |
| `game` | 准备 Pack，再在创作 owner thread 捕获生成状态并发布游戏。 |
| `plugin`、`scripts`、`import-sample` | 使用既有创作服务执行项目操作。 |
| `shader` | 调用选中目标的 Shader 编译工具。 |
| `verify`、`verify-native` | 执行架构检查或 Native 生成与实际消费验证。 |

`clean --scope cache` 只退休明确登记的无引用 generation；`--dry-run` 输出同一份候选计划。默认成功构建按 64 GiB、20 GiB 空闲阈值和 5 秒完整操作预算评估回收。未知、重复、缺值以及不属于该命令的参数会被拒绝。

Editor 配置独立于游戏发布配置。Support Pack 保持 Release 准备语义；Browser 的最终链接与 workload 由明确的 Browser 发布目标选择。

## 公开 API

| 声明 | 行为与结果 | 所有权/线程 | 失败或限制 |
| --- | --- | --- | --- |
| `BuildApplication` | 提供一次构建命令的应用边界。 | 不创建应用窗口，不是 Runtime 服务。 | 不向脚本导出。 |
| `Run(string[] arguments, CancellationToken cancellationToken) : Task<int>` | 执行命令并汇总退出结果。0 为成功，1 为命令或执行失败，2 为取消。 | 借用令牌；所属进程、workspace 和读取 leases 在结束前释放。Ctrl+C 订阅由外部入口负责。 | null 数组抛 `ArgumentNullException`。取消和失败不提交半成品。 |

`ReadInputIdentityAsync(engineRoot, configuration, dotnetHost, cancellationToken)` 供轻量启动器验证当前工具源码图与 SDK 身份。它不编译 CLI，也不准备 Native；返回字符串不转移任何 SDK 或加载上下文 owner。

## 初始化与所有权

命令先获得发行定义和明确工具目标，再创建对应工作流程。项目命令准备离线工具后打开创作 workspace，不创建 SDL 窗口或 ImGui 呈现。

`game` 先等待 `EnsurePlayerSupportPackAsync`，持有其读取 lease，再在当前 owner thread 启动 `BuildGameAsync`。游戏构建另行取得 Pack 读取保护并持有至编译、链接和打包完成。这样准备阶段可以异步运行，创作快照仍按线程协议捕获。

Native 构建结果由 `ProductNativeBuildResult` 唯一拥有；部署完成后释放所有产物 leases。Shader 命令也必须在工具进程结束后才释放其工具产物。

## 失败、取消与缓存

产品清理只能处理明确产品、目标与配置；不能清空整个仓库的 bin/obj 或删除其他有效 Pack。活动产品通过 lease 阻止破坏性清理。

SDK 子进程环境与父 IDE 隔离。取消时终止所属进程树并等待退出，再释放 staging 和读取保护。产物身份使用实际内容哈希；最终发布与可复用 Native 工作目录有不同身份。

Support Pack 准备前缓存、缓存预算回收和完整性能矩阵尚未验收，状态以[当前问题](../architecture/CURRENT_ISSUES.md)为准。

## 扩展与验证

新增目标、backend 或部署 compiler 在[标准发行组合](Inno.Build.Distribution.Standard.md)接入。新增命令只组合既有公开边界，不在解析器实现编译或目录退休。

命令契约由 `DevelopmentEntryCommandTests` 验证；Pack、Native、Task 和发布回归位于 Build 测试项目。Windows、Browser 产品运行与其他平台实机证据分别报告，不能以 CLI 编译成功替代。

相邻页面：[Build Pipeline](Inno.Build.md) · [Toolchains](Inno.Build.Toolchains.md) · [Support Pack](Inno.Build.SupportPacks.Core.md) · [Task Hosting](Inno.Build.TaskHosting.md)

## 当前源码公开 API 清单

按功能类型核对全部公开与可扩展 protected 声明。类型的组合与所有权从前文工作流进入。

### `Inno.Build.Cli.BuildApplication`

<!-- api: T:Inno.Build.Cli.BuildApplication 5feb127b395302e89588037726f079518b465572b5636555e0e8d5f923b0f9f8 -->
<!-- api: M:Inno.Build.Cli.BuildApplication.ReadInputIdentityAsync(System.String,System.String,System.String,System.Threading.CancellationToken) cb8bf5db30a3f23c4fc7ad15f8fab0ab5599b4c6555002c3a82e17c0ae9a7bac -->
<!-- api: M:Inno.Build.Cli.BuildApplication.Run(System.String[],System.Threading.CancellationToken) 400f0d36b86664da5273e2b1f0f60687598f4e14cfe48080468bb8994a762531 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-27377f124c175bc4"></a>[`public static System.Threading.Tasks.Task<string> Inno.Build.Cli.BuildApplication.ReadInputIdentityAsync(string engineRoot, string configuration, string dotnetHost, System.Threading.CancellationToken cancellationToken)`](../../build/cli/Inno.Build.Cli/BuildApplication.cs#L27) | Inspects the selected SDK's evaluated Build-tool input closure without compiling or preparing Native products.<br>返回：The content identity of the actual source graph, restored dependencies, compiler and SDK.<br>`engineRoot`：The absolute source checkout.<br>`configuration`：The explicit Debug or Release compiler configuration.<br>`dotnetHost`：The selected managed SDK executable.<br>`cancellationToken`：Cancels evaluation and input hashing. | — | T:System.IO.IOException: A required source, SDK or restored dependency cannot be read.<br>T:System.InvalidOperationException: The SDK cannot evaluate the current graph.<br>T:System.OperationCanceledException: Inspection was canceled. |
| <a id="api-93c6a267497d2f98"></a>[`public static System.Threading.Tasks.Task<int> Inno.Build.Cli.BuildApplication.Run(string[] arguments, System.Threading.CancellationToken cancellationToken)`](../../build/cli/Inno.Build.Cli/BuildApplication.cs#L56) | Runs one build command without taking ownership of the caller's cancellation source.<br>返回：Zero on success, two on cancellation, or one on a command or execution failure.<br>`arguments`：The command and its documented options; help prints the supported command names.<br>`cancellationToken`：Cancels owned work and preserves completed publications according to their pipeline contracts. | — | T:System.ArgumentNullException: The argument array is absent. |
| <a id="api-df8e541fac90a97d"></a>[`public static class Inno.Build.Cli.BuildApplication`](../../build/cli/Inno.Build.Cli/BuildApplication.cs#L14) | Executes explicit engine and project build commands for a caller-owned development lifetime. | — | — |
