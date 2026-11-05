# Inno.Tooling.Validation.Tests

[测试与 fixture 索引](README.md)

Wiki 计划回归要求文档结构检查先于签名与示例，并将真实项目及源码链接目标纳入证据。仅源码修改触发的项目页检查也必须具备这一前置责任。

`ValidationSolutionTests` 使用真实 SDK 重现 IDE 配置未勾选构建时 `.slnf` 成功但不产出程序集的情况，再编译统一入口保留的 `.slnx`，验证所有选中 owner 参与构建且 SourceDebug 不被 IDE 重映射为 Debug。测试同时确认政策失败不会提前执行该构建。

环境回归使用 SDK 实际选择的外部引用 pack、imports 和链接源码，修改相同长度内容并恢复时间戳，验证旧计划仍会失效。引用 pack 的名称和版本来自 SDK 声明，不维护另一份版本表。

收尾回归让真实检查脚本在执行中同时修改仓库源码输入和外部 import，保持文件长度与时间戳；执行结果必须分别保留 `input-stability` 与 `environment-stability` 失败。独立并行读取不能掩盖任一变化。

`ValidationCompletionTests` 在真实检查进入后锁住外部 import，使最后的内容读取失败；全部检查已通过的中间文件仍不能再次认证。解除读取阻塞后，完整运行及证据核验必须通过。CI 汇总另外拒绝缺少、未完成或类型错误的完成字段。

编译记录回归在规划后修改真实测试源码，使编译后的断言失败，再恢复原字节与时间戳；修复后的运行必须重新编译并通过。接着仅修改叶子源码，验证未变化依赖的实际程序集字节与写入时间保持不变；最后修改依赖源码并恢复其时间戳，在新的 Git 基线上验证旧编译结果仍失效，实际断言必须看到新的依赖值。时间戳仅用于观察是否发生了不必要编译，复用判断仍依赖完整内容证明。

范围选择回归覆盖已跟踪源码等长修改并恢复时间戳、Git 索引缓存未刷新、暂存区修改但工作区恢复 HEAD，以及 CRLF checkout 转换与 Unicode 文件名。内容比较必须使用 Git 的实际 clean filters，不能依靠 `git diff` 的时间戳缓存，也不能把正常换行转换误认为源码变化。

子模块回归覆盖索引隐藏的实际源码修改、指向 owner 外部的链接文本校验、被引用的子模块编译输入归属，以及被忽略的实际编译源码拒绝。换行用例分别覆盖规范化 blob 与直接提交 CRLF 的 blob；Unix 使用真实符号链接，Windows 使用 Git 的实际 checkout 表示。

[Tooling 索引](../tooling/README.md) · [验证入口](../tooling/Inno.Tooling.Validation.md) · [Wiki 首页](../../README.md)

通过公开 `ValidationCommand.ExecuteAsync` 驱动真实临时 Git 仓库和选定 SDK，不使用 IVT、私有反射或生产后门。fixture 拥有临时源码、输出和 Git 元数据，排空子进程后清理明确的临时目录；测试程序集不进入产品。

`ValidationHostContractsTests` 在独立的真实仓库副本中，通过统一入口编译并运行 Input 消费者，核对真实 TRX：类级 OS 声明、方法级覆盖及历史平台 guard 的不适用记录；对应宿主上的跳过和没有声明的跳过必须失败。fixture 的 SDK 项目与执行协议声明为动态输入；普通 Input 源码或断言变化不重复运行验证器自身的完整协议回归。

`ValidationTestOwnershipTests` 通过真实 Input/UI 编译及共享、隔离测试，检查一个 owner 的失败不会把另一个已完成 owner 误记为阻塞。失败进程的独立记录始终阻止整批认证；修复失败 owner 后，输入、依赖和输出仍匹配的本地成功 owner 可以复用。隔离发现声明的每个用例必须有唯一完成记录，漏报不能由同程序集其他成功用例掩盖。fixture 使用与当前验证程序集配套的执行脚本，并在自己的基线中记录该输入。

`ValidationPlanningTests` 覆盖新增/移动/删除、imports、动态发现输入、纯文档范围、未知 owner、领域测试传递 native 依赖、空行为测试、缺失与旧输入证据、基础分支推进后的实际 merge base，以及从 development product 声明选择原生上下文而保留独立领域 owner。

共享执行脚本回归检查全部测试 owner 的选择及直接证据输入，同时确认执行协议不会被错误地登记为普通源码编译输入。局部复用集成先完成真实还原，避免把首次补全的 NuGet import 图误当成稳定环境。

另外覆盖未初始化 submodule 的明确身份、script-only Wiki 的真实产品准备、输出竞争和取消、拒绝接管未知目录，以及深层计划中的冷输入、十轮热构建和 Release 产品前提。CI PowerShell 契约分别检查结果汇总、日程、发布来源、压缩包边界和产物字节；这些负例不能替代 Windows/macOS 上的真实产品执行。

引用边的条件属性必须保留其条件源码和 native 需求；只改测试实现仍选择行为测试，但不重复验证未变化的生产 API。外部 import 即使长度和时间戳不变，内容变化也必须使旧证据失效。实际源码编译与 TRX 集成实验记录在实施记录中，不以合成结果替代真实编译失败。

全仓源码政策的输入覆盖另外包含测试 C#、文档中的 C# 模板和根级 MSBuild 属性文件，防止这些位置的 global/implicit using 变化借用旧架构结果。

CI 契约由统一入口发现 `tests/ci/*.Tests.ps1`，没有独立脚本名单；空发现必须失败。保留协议用例模拟 CLI 导出与认证的不同结果，检查失败或取消后只运输新完成的不可变归档，旧候选不能冒充新导出。模拟协议不替代真实 native 发布器与托管 CI 验收。

需要 Git 与 .NET 10 SDK。环境不可用应失败。测试类的 public 方法仅是 xUnit 发现入口，没有产品 public/protected 契约。

```powershell
dotnet test tests/tooling/Inno.Tooling.Validation.Tests/Inno.Tooling.Validation.Tests.csproj --configuration Debug
```

## 文档与 Agent 输入边界

文档影响回归区分 rule-only、skill-only、record-only 和 API inclusion。记录修改不选择产品构建或 Native 配方；任意名称的 API 分片沿包含关系选择实际 owner，缺失必要分片拒绝生成可用计划。未知可执行 `.agents/` 输入继续保守扩大范围。
