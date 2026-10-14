# Inno.Entry.Bootstrap

Build 入口借用命令中明确的 `--dotnet`，将同一 SDK 传给工具 inspection、工具 publication 与实际构建。未指定时使用当前宿主 SDK。重复、缺值或不存在的 SDK 在 checkout 写 lease 前失败；这不是另一个产品或后端选择入口。

[入口索引](README.md) · [Wiki 首页](../README.md) · [Core.IO](../core/Inno.Core.IO.md)

## 职责与边界

为源码 checkout 提供宿主路由、依赖准备和开发应用启动。只依赖 BCL 与 Core.IO；平台产品和构建应用通过其公开入口加载，Bootstrap 不静态拉入完整产品图。

## 快速使用

通常直接使用两个 IDE 入口。嵌入开发宿主时，在原主线程调用 `DevelopmentEntry.RunEditor(args)`；构建入口调用并等待 `DevelopmentEntry.RunBuildAsync(args)`。

## 公开 API

| 声明 | 行为与结果 | 所有权/线程 | 失败或限制 |
| --- | --- | --- | --- |
| `DevelopmentEntry` | 提供开发宿主的两个应用入口。 | 无需创建实例；不属于游戏脚本 API。 | 仅源码 checkout 开发场景。 |
| `RunEditor(string[] arguments) : int` | 验证当前宿主的准备记录，必要时准备，再启动 Editor。无 Project 参数打开目录选择窗口。 | 必须在原应用主线程调用；方法拥有加载宿主和读取 leases，退出时释放。 | null 参数抛 `ArgumentNullException`；成功或对话框取消为 0，取消准备为 2，其他失败为 1。 |
| `RunBuildAsync(string[] arguments) : Task<int>` | 无参数执行当前宿主 Editor 增量构建；显式参数传给构建应用。 | 等待所属工作完成；方法拥有取消订阅和 checkout 独占 lease。 | null 参数抛 `ArgumentNullException`；成功 0、取消 2、失败 1。 |

## 准备、借用与退出

有效 Editor 部署先取得 checkout 与产品两份读取保护，再验证一次完整文件清单与哈希，不扫描 Native 源码闭包，也不预先定位 SDK。缺少有效部署时释放读取保护，按 checkout → product 的顺序取得准备独占保护；准备完成后重新取得读取保护并再次验证，防止锁切换期间部署改变。引擎源码修改通过 Build 应用；游戏资源变化继续走 Editor 导入和 reload。

Editor 持有 checkout 与产品共享读取保护；Build 与产品清理分别取得对应独占保护。准备失败不发布 ready.json，已退出 owner 的锁文件仍保留。不要通过删除锁文件解锁。

已准备 Build 工具先取得 publication 读取保护，再校验清单和当前源码输入身份；保护持续覆盖载入、命令执行与卸载。首次引导的独占 SDK 输出仅用于创建固定输入的不可变 publication，载入检查退休后由入口清理；长期编译 workspace 与 publication 分别登记 owner，不能混作临时输出。

配置和产物布局见[开发入口架构](../architecture/DEVELOPMENT_ENTRY_ARCHITECTURE.md)。Bootstrap 不是 collectible 游戏模块管理器，不能替代 Runtime 的 generation 和退休协议。

## 内部实现（非公开）

- `DevelopmentHost`：只映射实际支持的宿主。
- `DevelopmentBuildRequest`：补齐当前宿主与入口配置，不覆盖显式游戏目标。
- `DevelopmentDependencies`：初始化缺失的固定 revision 子模块，保留已初始化工作区。
- `DevelopmentProcess`：隔离 SDK 进程环境，取消后等待退出。
- `DevelopmentProduct`：校验完整部署、原子记录准备结果。
- `DevelopmentApplication`：按依赖解析器加载应用，调用公开应用入口。
- `DevelopmentBuildTool`：按明确工具定义校验部署与源码身份。输入检查、发布各在同一 Build 可执行程序的独立 worker 中完成；采用普通进程生命周期，退出后释放工具程序集。cache clean 选择轻量 Toolchains 应用，其余命令选择 CLI，二者各自拥有 receipt、workspace 与 publication。

## 扩展与验证

新增宿主应先实现平台产品与工具链，再加入宿主映射。新增 backend 或游戏发布目标在发行组合注册，不加入 Bootstrap。

构建两个 entry 项目可验证轻量依赖边界；实际启动、首次准备和重复启动应分别验收。当前已执行项与限制见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Entry.Bootstrap.DevelopmentEntry`

<!-- api: T:Inno.Entry.Bootstrap.DevelopmentEntry c152608544e274ce11fb40f1347814be3ff8cb9abc996eafcf26352fe96fd684 -->
<!-- api: M:Inno.Entry.Bootstrap.DevelopmentEntry.RunEditor(System.String[]) 92d7af88221ce2ad84d1c1fafc0a9f42bc8a67b79eeca2d7a34b39ef8b9634f9 -->
<!-- api: M:Inno.Entry.Bootstrap.DevelopmentEntry.RunBuildAsync(System.String[]) 537c6918e7153d7a7be5ed77cc7c53c6b4ccf4feb9381259935689797f049982 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8586686fb91a9f13"></a>[`public static System.Threading.Tasks.Task<int> Inno.Entry.Bootstrap.DevelopmentEntry.RunBuildAsync(string[] arguments)`](../../entry/Inno.Entry.Bootstrap/DevelopmentEntry.cs#L131) | Prepares and invokes the shared build application for the current development host.<br>返回：Zero on success, two on cancellation, or one on preparation or build failure.<br>`arguments`：No arguments selects an incremental Editor build. Use --clean to clean the selected product first. | — | `System.ArgumentNullException`：The argument array is absent. |
| <a id="api-593fe4fcbbee9a19"></a>[`public static int Inno.Entry.Bootstrap.DevelopmentEntry.RunEditor(string[] arguments)`](../../entry/Inno.Entry.Bootstrap/DevelopmentEntry.cs#L31) | Runs the validated prepared Editor on the original main thread, building it when preparation is absent.<br>返回：Zero on normal exit or dialog cancellation, two on cancellation, or one on failure.<br>`arguments`：An optional project directory followed by Editor options; no directory opens a native folder dialog. Use --help or --status to inspect the entry without building a product. Engine source changes are applied by the Build entry; opening a prepared Editor does not invoke build tools. | — | `System.ArgumentNullException`：The argument array is absent. |
| <a id="api-bca9893d3018c001"></a>[`public static class Inno.Entry.Bootstrap.DevelopmentEntry`](../../entry/Inno.Entry.Bootstrap/DevelopmentEntry.cs#L13) | Provides the two checkout development commands without selecting platforms in shared engine code. | — | — |

