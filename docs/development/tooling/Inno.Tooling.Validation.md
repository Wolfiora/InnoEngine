# Inno.Tooling.Validation

[Tooling 索引](README.md) · [Wiki 首页](../../README.md) · [架构验证](Inno.Tooling.Architecture.md) · [回归](../testing/Inno.Tooling.Validation.Tests.md)

## 职责与边界

工具层库由既有 Build 入口的 `validate` 命令调用，不进入 Runtime、Player 或脚本表面，也不增加日常 executable。依赖架构验证库与中立 Build Toolchains。

测试项目的运行说明、导航和 PowerShell 命令变化只要求文档检查，不因测试 owner 消费 Native 就编译整个套件。实际 C# 示例仍准备对应 owner 的编译前提，并覆盖其 inclusion 分片。只有 SDK 明确声明 `IsTestProject` 的 runner 使用这一边界；`tests/` 下的公开支持库仍执行 API、XML 和示例契约检查。

托管与原生构建属性分别记录，原生目标从现有 development product 声明取得。每个属性上下文构建所需项目的并集，测试消费该上下文的 SDK 输出快照；不同目标不共享可写输出。领域测试的实际传递依赖由计划器与架构检查共同禁止进入 native 准备。

执行目录中的临时 `.slnx` 直接表达计划选中的项目和配置，不继承 IDE `.sln` 的 `Build.0` 排除或配置重映射。原有解决方案仍负责开发环境导航；验收不得因其中某个 Player 或测试项目未勾选构建而静默跳过。构建并集在执行政策检查前写入诊断目录，政策失败仍禁止启动构建。

同一进程中的并集调用向所有根项目传递一个 Task bootstrap owner，避免每个解决方案根生成不同的全局属性实例并竞争共享输出。Clean 与 Build 是两个进程，必须分别生成身份，不能复用已退出进程的声明。内容身份要求重新编译时，在工作区写入 lease 内先完成单独串行的 SDK Clean，再启动有界并行 Build；不用会交错执行多个 Clean/Build 的 `--no-incremental`。完整编译与输出核验结束后才记录输入身份，清理不构成通过证据。

Clean 通过既有 `InnoPrepareProductNative=false` 明确禁止产品准备，避免清理项目引用时触发原生构建。该参数只作用于清理进程；后续 Build 保留产品选择、原生准备、消费者保护与部署核验。

只有依赖内容证明成立时，Clean 才设置 `InnoCleanProjectReferences=false`；它在 SDK 的引用清理边界生效，覆盖 development entry 为正常 Build 强制开启的引用构建设置。它不改变后续 Build。图求值和实际构建统一显式声明 `BuildProjectReferences=true`、`BuildingInsideVisualStudio=false`，避免入口与独立根仅因等价默认属性而创建重复项目实例。

同一上下文中只有选中根项目各自独占的 SDK `Compile` 输入变化时，可以仅清理这些根项目，保留已核验的未变化依赖输出。即使两个选中根项目互相依赖，也都显式清理和重新编译，不把其源码误判为必须清理全部下游依赖的变化。这个判断同时要求其余依赖源码、共享或链接输入、imports、资源、生成器、工具、配置、还原包及全部原有输出内容匹配；缺少证明、其余依赖变化或输出损坏仍执行完整依赖清理。随后仍构建实际并集并执行全部选中断言，不用时间戳证明依赖有效。原生上下文同样可以保留未变化的托管依赖输出，但原生输入身份、准备、消费者读取保护、绑定选择和部署核验全部保留。

上述内容证明成立时，未选中的普通依赖引用只调用标准 `GetTargetPath` 取得已经验证的输出，不重复遍历其 `Build`。选中根的名单直接读取本次生成的 SDK solution；根之间的引用继续按实际属性实例执行 `Build`，包括产品准备后才确定的绑定选择，不能仅依赖 solution 的静态排序。自定义引用 targets 保留原行为。这个策略只作用于已验证并集，私有 Task 引导和 runtime 准备不继承它；产品根的原生准备、消费者 lease 和部署核验仍执行。构建日志包含 SDK 的实际项目、target 和 task 耗时，便于区分内容读取、编译与重复遍历成本。

计划内的检查顺序就是执行顺序：全仓政策先行，随后完成托管上下文的构建、API、Wiki 与行为检查，再进入原生和 Release 上下文，最终归档。每个必需前提必须排在消费者之前，否则计划生成失败。先前必需检查失败时，后续昂贵上下文明确阻塞，保留独立政策诊断，避免等待完整原生/Release 构建后才看到托管失败。

测试方法或类用标准 `SupportedOSPlatform` 声明真实操作系统契约。执行器只读取当前 SDK 产物的元数据，不加载测试对象，也不维护测试名称白名单。当前宿主不适用的具体用例在 `host-contracts.json` 和 owner 结果中明确记录为 NotApplicable；对应宿主上的 Skip、普通 Skip、未知环境声明及没有任何必需用例实际执行仍然失败。旧测试的提前返回也不能冒充其他平台上的验证。设备或工具缺失不能借用 OS 声明变成通过。

先通过选定 SDK 批量求值真实工程，再由 Git merge base、staged/unstaged/untracked 输入确定 owner 与消费者。源码、资源、imports、ProjectReference 和显式 `InnoValidationInput` 来自 SDK。未知关系扩大范围并记录原因；没有行为测试或领域测试传递引用 native 时拒绝计划。

实际引用边上的全局属性变体也进入图，条件源码和条件 native 依赖不能因根项目默认属性而丢失。托管构建并行度为处理器数与 4 的较小值；原生准备仍保持单 owner，跨 MSBuild 工作进程的准备复用尚未获得并发验收，不能提前启用。

选中项目存在不同有效属性实例时，其完整输入仍属于依赖清理的保护范围。默认根项目的 Clean 不能证明另一个消费者的不同输出实例已清理；这些输入变化保守触发完整依赖清理，避免等长且保留时间戳的源码修改执行旧变体。

捕获事实完全一致的属性变体共用输入正文，同时保留全部原始属性集合；条件源码、imports 或属性值不同的变体仍单独保存。计划中的 `planningSeconds` 记录源码盘点、SDK 图、范围选择与执行环境身份的实际耗时，不能用计划耗时代替构建和测试耗时。

执行环境身份包含选定 SDK、宿主、运行时、引用 packs、PowerShell 和图中的外部输入。包含关系先归并为互不重叠的读取根，再以最多四个独立内容快照并行校验；开始与结束阶段仍分别完整读取内容及目录成员，不使用时间戳缓存。SDK 图日志区分实际求值与快照整理耗时，便于定位剩余开销。

计划阶段的工具内容读取与 SDK 图求值独立并发，两者都完成后再补入图声明的外部输入及实际引用包；结束校验重新读取完整环境。引用包通过 SDK 的已知框架声明和 NuGet 框架解析取得，浏览器等平台限定框架仍按其基础框架选择引用包。

所有检查结束后，源码与执行环境的内容快照独立并行读取，等待两者全部完成再核对结果；任一内容变化都记录失败。这里不复用计划阶段读取的 bytes，也不省略输出、依赖与活动 owner 的收尾核验。

结果文件在收尾核验完成前始终记录 `completed=false`。只有完成最终源码、环境、产物核验及 owner 登记后才写入 `completed=true`；它表示执行已完成，仍须逐项检查失败结果。中断、读取异常或缺少完成字段的中间文件不能用于 `validate evidence`、局部结果复用或 CI 汇总，即使其中每个已有检查都显示通过。

已完成编译的输入记录也必须等待收尾核验。编译成功但源码、环境或对应输出在运行中改变时，不发布该次可复用编译记录；恢复源码后必须重新证明编译输入，不能因错误产物与提前写下的记录一致而跳过重编译。单纯的断言失败可以保留内容核验完整的编译记录，但不能变成测试通过。

真实产品旅程的 Samples 输入由固定仓库 URL 与提交选择。Git 依赖缓存放在有写入 lease 的稳定验证工作区，运行目录只保留输入 archive 和来源／完整性日志。每次使用检查 bare owner、远端身份、全部 Git 对象完整性和确切 commit，再从对象导出固定项目；不读取相邻工作区的未提交文件。有同来源、同提交的相邻仓库时只读导入对象，否则从声明的 HTTPS 来源获取。损坏对象缓存隔离并重新获取；网络或实际构建失败仍失败，不把失败重试改写为通过。

编译证据沿 SDK 实际项目实例的引用边取得还原图和输出，区分相同项目被不同消费者以不同属性引用的情况。范围分析可以保守扩大源码影响，但不能把未选中消费者的私有构建目录当作当前必需产物。SDK 自带图求值使用至多四个线程，不在整个求值外层追加串行锁。

Wiki 的脚本投影使用该检查明确声明的 Build 前置项及其 SDK 输出；完整计划中同一个项目可以同时具有 Debug 和 Release 构建项，不能只按项目名查找执行入口，也不能借用尚未执行的 Release 产物。

脚本示例通过 Markdown 语法树识别实际 C# 围栏。项目任一选中分片使用脚本投影时，该项目入口、全部选中分片及其编译前置项使用同一投影上下文，避免验证器展开完整项目文档后在另一个上下文重复执行缺少投影引用的示例。

已跟踪工作区文件额外通过 `git hash-object --stdin-paths` 的实际内容与 HEAD blob 比较，保留 Git 的 clean filters 和换行转换；等长修改并恢复时间戳不能绕过范围选择。暂存区差异独立加入选择，即使工作区已经恢复 HEAD；移动仍同时覆盖旧、新路径。此过程不写 Git 对象或刷新索引。

内容可以精确匹配已提交 blob，或经 Git clean 转换后匹配；历史中直接提交的 CRLF 不应被当前换行配置误报。子模块递归核验实际内容，符号链接仅校验链接文本、禁止遍历目标或链接父目录。子模块身份进入原生检查输入及实际引用其中源码的编译 owner。实际 SDK 编译源码若被忽略且未纳入输入身份，计划必须失败，不能静默复用证据。

## 公开 API

文档结构与链接由一个前置检查全仓验证；Wiki 契约检查只执行签名和示例，显式依赖该前置结果。结构证据同时绑定源码、项目声明和链接目标输入，不能在删除目标或新增项目后借用旧通过结果。

| 类型/成员 | 语义 |
|---|---|
| `ValidationCommand` | 唯一公开命令入口，没有实例或 protected 扩展点。 |
| `ValidationCommand.ExecuteAsync(string[] arguments, CancellationToken cancellationToken = default)` | 执行 `plan`、`run`、`full` 或 `evidence`。返回 0 表示请求成功；参数无效、输入过期、证据缺失或失败抛出异常，由 Build CLI 报告非零退出码。取消排空子进程。 |

计划、SDK snapshot 与结果 JSON 是内部诊断模型，不是引擎持久化协议或脚本 API。只适用于当前工具实现，不提供旧格式读取。

一次测试并集中的实际结果按项目归属：完整通过的 owner 保留 Passed，失败 owner 保留实际失败，尚未执行的依赖仍为 Blocked。非零执行退出码另外保留失败记录，不能因部分 owner 通过而认证整批成功。每个 owner 都必须完成隔离发现及全部已发现用例，零执行、漏报和未声明跳过仍失败；本地复用继续要求输入、依赖与产物完全匹配。

共享测试执行脚本是每个测试检查的直接输入。修改它会选择全部适用测试 owner 并使旧执行证据失效；该脚本不被加入普通编译或 native recipe 的输入，避免无关重编译。首次还原新增实际 NuGet imports 时环境身份会变化，只有还原后的环境稳定，才能验证后续的精确输入复用。

## 工作流与示例

```powershell
dotnet run --project entry/InnoEngine.Build --no-build -- validate plan --engine-root . --dotnet /absolute/path/to/dotnet --base origin/master --output artifacts/validation/review
dotnet run --project entry/InnoEngine.Build --no-build -- validate run --engine-root . --plan artifacts/validation/review/plan.json --output artifacts/validation/run
dotnet run --project entry/InnoEngine.Build --no-build -- validate evidence --engine-root . --plan artifacts/validation/run/plan.json --evidence artifacts/validation/run/results.json
```

SDK 路径按本机显式选择，Windows 使用 `dotnet.exe`。构建目录稳定复用，日志按运行分开。正在执行的 CLI 与待替换的构建输出必须有不同写入 owner。

`plan --full true` 生成完整功能计划而不执行；`full` 直接执行完整计划。`--extended true` 额外要求从未准备的 native 缓存开始，记录冷构建、十轮热构建，并将真实 Editor 面板生命周期扩大为 1,000 轮。已有 native 产物不能被标记为冷构建；普通修改涉及构建、SDK 或 reload 时也提前选择对应热缓存检查。

计划分别记录静态、领域、公共契约、集成、Native、产品和性能证据；编译是这些证据共享的前提。完整功能计划还准备 Release 上下文，运行真实 Release 产品旅程、验证 source-independent Player Support Pack，并在所有必要结果成功后归档同一份产物。不同配置有独立 SDK 输出；发布不会再次构建。

测试项目的 `InnoValidationEnvironment` 可以声明分号分隔的标准 OS 名称（windows、macos、linux）；未知名称失败，其他宿主在准备产物前明确排除。performance 层只在 extended、构建/原生/reload 或性能测试输入变化时采样，结果不可复用；普通完整功能矩阵保留明确排除理由，不把未采样记录为通过。

GitHub 的 Plan 阶段只准备工具和一个计划，Run 阶段消费该计划。`--cache-input` / `--cache-output` 指定不可变 native/bindings 的运输归档。发布器验证请求身份、完整文件集合及内容哈希，并重新建立本地登记和读取保护；只导出已实际采用的完整生成物，未采用候选不随每轮缓存累积。跨 runner 不传输活动 lease、PID、cache owner 记录或可写 `obj`。外层运输缓存以完整归档内容为键，归档包含 recipe/工具/SDK/目标/配置身份的 generation manifests；旧候选不能充当测试结果。

主仓库保留完整 Git 历史以计算正确 merge base；递归子模块仅取得锁定提交所需的浅历史，不因主仓库 `fetch-depth: 0` 同时下载全部第三方历史。计划仍核对真实检出身份和干净状态，子模块获取失败直接失败。

每次输入盘点对独立文件和子模块使用最多四路并行，读取实际内容并保留链接边界检查；取消或失败会等待所有工作者退出。计划阶段的工具内容校验与 SDK 图求值并行完成后才生成计划。执行前后仍重新盘点，不把时间戳、Git 索引缓存或前一次读取当作新鲜度证据。

Retain 阶段在失败或取消后以独立归档保留已完成的不可变生成物。`evidence --cache-output` 先导出完整生成物，再独立核验证据；导出成功不改变失败、缺失或取消的验证结论。不能把本次开始时还原的旧缓存当作本次完成的产物。

PR 将新导出的不可变归档写入 GitHub 自带的 PR merge-ref 缓存范围，后续同一 PR 的提交可以复用准备产物；默认分支不能读取 PR 范围的缓存。默认分支只在完整执行成功后保存自己的缓存。两种来源在消费时都重新导入校验，不复用旧测试绿色状态。失败后的新导出归档保留独立文件，再按完整摘要核对运输副本。

每天台北时间 03:17 在默认分支有新提交时执行完整功能；周日由 04:17 的深层验收独占，不重复每日任务。`Engine validation` 对齐两个平台的计划与逐项结果。完整认证保留 30 天，可交付归档保留 90 天。手动草稿 Release 要求成功运行 ID、确切提交和标签，同时验证服务器下载摘要与产物 manifest；`verify_only` 可演练所有读取和校验步骤。远程实际执行状态仍以实施记录为准。

默认可写 owner 为 `artifacts/validation/workspaces/<configuration>`，通过现有缓存登记与 Core IO 协调 lease 持有整个构建和测试过程。显式目录必须是新目录或此前已登记的同一 owner；工具拒绝接管未知目录。相同 owner 的同时运行会等待，取消时释放读取与写入责任。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Validation;

public static class ValidationExample
{
    public static Task<int> CreatePlan(string[] options)
        => ValidationCommand.ExecuteAsync(["plan", ..options], CancellationToken.None);
}
```

## 失败与生命周期

缺失结果、空测试、输入中途变化均不能认证。第一次失败日志与 TRX 保留；重试写入新目录并记录原因。本地确定性结果必须匹配相关源码、测试、验证工具、配置与环境。CI 跨运行结果目前不会自动复用。

编译、API、Wiki 与测试证据同时记录所需输出文件的内容摘要和完整成员集合；被覆盖、缺失或增加的输出会使复用失效。前置检查失败或取消后，依赖项明确记录为 Blocked。构建日志、TRX 与结果分别按上下文归档。

环境身份包含选定 SDK、实际运行时、PowerShell 与外部 imports 的内容。编译类证据另外记录实际还原图和依赖包内容；同长度、同时间戳的内容变化仍使证据失效。工作区写入 lease 内保存已完成编译的输入与输出身份，身份变化时强制重新编译，避免 SDK 时间戳增量检查误用旧程序集；这个记录不代表测试通过。

只修改测试实现时仍构建并运行受影响行为测试，未变化生产 API 的 XML/Wiki 检查明确列为不适用。生产源码、公开契约或直接文档输入变化时按实际 owner 恢复相应检查。测试报告必须包含实际执行记录，跳过、空发现、计数不一致或非 Passed 用例不能认证为成功。

全仓源码政策的证据输入包含生产目录，以及所有受检查的 C# 文件、C# 模板和 MSBuild 项目/属性/目标文件。测试目录和文档中的源码模板也受 using 政策约束，修改这些输入不能复用旧的架构通过结果。

当前迁移和未完成验收见 [实施记录](../../../.agents/records/plans/AI_PIPELINE_IMPLEMENTATION.md)。产品与设备矩阵仍在接入；工具回归通过不代表完整引擎认证。

<!-- api: T:Inno.Tooling.Validation.ValidationCommand 0253332414abf0d30d3c1e9deaf9e45ac081661bcecb4b2bf6a99d4d3ec3a864 -->
<!-- api: M:Inno.Tooling.Validation.ValidationCommand.ExecuteAsync(System.String[],System.Threading.CancellationToken) c89fe80bda2a1d8c91cd0790bb909429696aeddd0d61054d87c277467842a71c -->

## 文档与 Agent 输入分界

项目文档与 api-include / 同名目录分片通过共同文档归属映射选择 API 和示例 owner。规则输入只包含根指令、rulebook 与加载器；skills 和 records 由独立的结构与链接检查覆盖。记录改动不进入产品编译或 Native 选择；未知可执行文件仍按真实输入处理，不能因位于 .agents 而豁免。

缺少项目页由必要的 documentation 门禁失败；映射存在不代表源码 API 已通过。
