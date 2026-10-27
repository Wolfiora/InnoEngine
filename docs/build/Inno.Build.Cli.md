# Inno.Build.Cli

[Build 索引](README.md) · [Wiki 首页](../README.md) · [开发 Build 入口](../entry/InnoEngine.Build.md)

## 职责与边界

统一构建应用库，接收明确命令并组合现有 Build、发行、工具链和创作服务。它由 `InnoEngine.Build` 调用，不是第三个 IDE 日常启动项目。

通用 Build 和运行时不得反向引用 CLI。平台、内容编译器、Native 组件与托管部署的注册属于标准发行组合；命令不维护第二套注册表。

创作闭包显式包含 Animation 资产导入及其公开脚本契约。CLI 生成的 Runtime reference 与产品可用能力保持一致；不能只用含额外测试引用的程序集验证示例。生成入口继续读取项目自己的 scripting 清单，不增加中央类型白名单。

## 快速使用

从仓库根目录运行：

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --configuration Release
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- verify .
```

开发入口补齐宿主与默认配置；SDK 和 checkout 由命令的正常上下文解析。直接嵌入 `BuildApplication` 时，调用者需要提供相应命令所要求的明确上下文；空参数只显示帮助。

项目命令支持 `--dotnet <absolute-sdk-host>`，内容工具、Pack 准备及最终发布共用该冻结 SDK 选择。未声明的参数与缺值立即失败；不能靠改变产品目标来选择工具宿主。SDK 与 workload 的安装是开发环境准备，产物缓存清理不删除 SDK。

`scripts --ide` 在成功编译项目及安装插件后，通过同一 `ScriptCompiler` 正式生成 .NET 10 / C# 14 IDE 工程和裁剪引用。普通 `scripts` 仍只导出脚本程序集。IDE 工程与 `Library/` 是派生内容，不手工修改。

SDK 生成的 `Inno.Build.Cli.deps.json` 随开发入口复制。该清单属于 CLI 自己的显式创作闭包，使库宿主和开发入口发现同一组脚本契约；不在编译器中维护领域类型名单。

Assets、Shaders 与通用 Graph 创作 API 直接引用其 feature 或可复用 presentation 项目，不引用具体 Panel 来间接携带脚本契约。命令行编译无需实例化这些 UI；实际绘制仍由 Editor 的 session 和 Panel 负责。

CLI 与平台 Editor 组合库显式启用共同的 `InnoExposeRuntimeClosure` SDK 构建规则。清单与实际运行依赖随引用复制，不改变项目的 `PrivateAssets="compile"` 边界，也不扫描输出目录来猜测扩展。

| 命令 | 行为 |
| --- | --- |
| `support` | 准备所选目标的 Editor 或 Player 支持产物，不构建整个 Editor 托管产品。 |
| `editor` | 增量构建 Editor entry 及其依赖；可用布尔 `--clean` 先清理所选产品。 |
| `clean --scope product` | 预检 SDK 和目标，清理产品拥有的工作状态；`--dry-run` 预览同一计划。 |
| `bindings` | 从发行声明的产品组件集合准备统一绑定。 |
| `support-pack` | 独立准备并校验目标 Pack，不包含游戏内容。 |
| `game` | 准备 Pack，再在创作 owner thread 捕获生成状态并发布游戏。 |
| `plugin`、`scripts`、`import-sample` | 使用既有创作服务执行项目操作。 |
| `shader` | 调用选中目标的 Shader 编译工具。 |
| `verify`、`verify-native` | 执行架构检查或 Native 生成与实际消费验证。 |

`clean --scope cache` 只退休明确登记的无引用 generation；`--dry-run` 输出同一份候选计划。默认成功构建按 64 GiB、20 GiB 空闲阈值和 5 秒完整操作预算评估回收。未知、重复、缺值以及不属于该命令的参数会被拒绝。

Editor 配置独立于游戏发布配置。Support Pack 保持 Release 准备语义；Browser 的最终链接与 workload 由明确的 Browser 发布目标选择。

项目命令也接受 `--engine-root <checkout>`，用于通过 SDK 的独立输出目录运行工具时明确指定 Native 工具的源码输入。该路径在启动工具或创建 Project 工作状态前校验；普通 checkout 内的开发入口可以自动定位。游戏使用独立输出目录时先通过 `support-pack --engine-root ...` 准备所需 Pack，再传入 `--support-packs`。

`verify-native` 要求 PowerShell 7 的 `pwsh`，并在开始生成前检查统一测试入口。第一阶段用 `HostSource` 的独立候选比对仓库中的完整 managed/native 生成源码；目标产物由后续正式 Native 产品构建准备，不要求首次运行前已经存在内容寻址缓存。原生测试完成部署后复用 `tests/Invoke-IsolatedTests.ps1`，普通与 `ProcessIsolation=Required` 用例分别执行并汇总失败；不排除 BGCS。TRX、发现结果及执行清单位于 `artifacts/acceptance/native-bindings/<target>/<operation>/`，成功报告记录实际结果目录。

## 公开 API

| 声明 | 行为与结果 | 所有权/线程 | 失败或限制 |
| --- | --- | --- | --- |
| `BuildApplication` | 提供一次构建命令的应用边界。 | 不创建应用窗口，不是 Runtime 服务。 | 不向脚本导出。 |
| `Run(string[] arguments, CancellationToken cancellationToken) : Task<int>` | 执行命令并汇总退出结果。0 为成功，1 为命令或执行失败，2 为取消。 | 借用令牌；所属进程、workspace 和读取 leases 在结束前释放。Ctrl+C 订阅由外部入口负责。 | null 数组抛 `ArgumentNullException`。取消和失败不提交半成品。 |

CLI 由 entry 正常项目引用并直接调用。其支持准备复用标准 Native plan，工具生命周期由 SDK 工程与具体命令管理；没有额外的 CLI worker 或启动 publication。

## 初始化与所有权

命令先获得发行定义和明确工具目标，再创建对应工作流程。项目命令准备离线工具后打开创作 workspace，不创建 SDL 窗口或 ImGui 呈现。

`game` 先等待 `EnsurePlayerSupportPackAsync`，持有其读取 lease，再在当前 owner thread 启动 `BuildGameAsync`。游戏构建另行取得 Pack 读取保护并持有至编译、链接和打包完成。这样准备阶段可以异步运行，创作快照仍按线程协议捕获。

Native 构建结果由 `ProductNativeBuildResult` 唯一拥有；部署完成后释放所有产物 leases。Shader 命令也必须在工具进程结束后才释放其工具产物。

项目命令借用所选离线工具的实际 executable/include 路径，保留 Native 结果至整个内容编译结束。按命令取得的工具不会形成第二份 CLI 启动 publication；内容 compiler 工厂接收同一 `ToolRunner`。运行于哪个 Debug/Release 配置不会重新决定工具文件名。

## 失败、取消与缓存

产品清理只能处理明确产品、目标与配置；不能清空整个仓库的 bin/obj 或删除其他有效 Pack。活动产品通过 lease 阻止破坏性清理。

SDK 子进程环境与父 IDE 隔离。取消时终止所属进程树并等待退出，再释放 staging 和读取保护。产物身份使用实际内容哈希；最终发布与可复用 Native 工作目录有不同身份。

Support Pack 准备前缓存、缓存预算回收和完整性能矩阵尚未验收，状态以[当前问题](../architecture/CURRENT_ISSUES.md)为准。

## 扩展与验证

新增目标、backend 或部署 compiler 在[标准发行组合](Inno.Build.Distribution.Standard.md)接入。新增命令只组合既有公开边界，不在解析器实现编译或目录退休。

命令契约由 `DevelopmentEntryCommandTests` 验证；Pack、Native、Task 和发布回归位于 Build 测试项目。Windows、Browser 产品运行与其他平台实机证据分别报告，不能以 CLI 编译成功替代。

相邻页面：[Build Pipeline](Inno.Build.md) · [Toolchains](Inno.Build.Toolchains.md) · [Support Pack](Inno.Build.SupportPacks.Core.md) · [Task Hosting](Inno.Build.TaskHosting.md)

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Cli.BuildApplication`

<!-- api: T:Inno.Build.Cli.BuildApplication 5feb127b395302e89588037726f079518b465572b5636555e0e8d5f923b0f9f8 -->
<!-- api: M:Inno.Build.Cli.BuildApplication.Run(System.String[],System.Threading.CancellationToken) 400f0d36b86664da5273e2b1f0f60687598f4e14cfe48080468bb8994a762531 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-93c6a267497d2f98"></a>[`public static System.Threading.Tasks.Task<int> Inno.Build.Cli.BuildApplication.Run(string[] arguments, System.Threading.CancellationToken cancellationToken)`](../../build/cli/Inno.Build.Cli/BuildApplication.cs#L28) | Runs one build command without taking ownership of the caller's cancellation source.<br>`arguments`：The command and its documented options; help prints the supported command names.<br>`cancellationToken`：Cancels owned work and preserves completed publications according to their pipeline contracts.<br>返回：Zero on success, two on cancellation, or one on a command or execution failure.<br>T:System.ArgumentNullException：The argument array is absent. |
| <a id="api-df8e541fac90a97d"></a>[`public static class Inno.Build.Cli.BuildApplication`](../../build/cli/Inno.Build.Cli/BuildApplication.cs#L11) | Executes explicit engine and project build commands for a caller-owned development lifetime. |
