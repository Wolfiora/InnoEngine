# Inno.Tooling.Validation.Tests

Wiki 计划回归要求文档结构检查先于签名与示例，并将真实项目及源码链接目标纳入证据。仅源码修改触发的项目页检查也必须具备这一前置责任。

`ValidationSolutionTests` 使用真实 SDK 重现 IDE 配置未勾选构建时 `.slnf` 成功但不产出程序集的情况，再编译统一入口保留的 `.slnx`，验证所有选中 owner 参与构建且 SourceDebug 不被 IDE 重映射为 Debug。测试同时确认政策失败不会提前执行该构建。

环境回归使用 SDK 实际选择的外部引用 pack、imports 和链接源码，修改相同长度内容并恢复时间戳，验证旧计划仍会失效。引用 pack 的名称和版本来自 SDK 声明，不维护另一份版本表。

范围选择回归覆盖已跟踪源码等长修改并恢复时间戳、Git 索引缓存未刷新、暂存区修改但工作区恢复 HEAD，以及 CRLF checkout 转换与 Unicode 文件名。内容比较必须使用 Git 的实际 clean filters，不能依靠 `git diff` 的时间戳缓存，也不能把正常换行转换误认为源码变化。

子模块回归覆盖索引隐藏的实际源码修改、指向 owner 外部的链接文本校验、被引用的子模块编译输入归属，以及被忽略的实际编译源码拒绝。换行用例分别覆盖规范化 blob 与直接提交 CRLF 的 blob；Unix 使用真实符号链接，Windows 使用 Git 的实际 checkout 表示。

[Tooling 索引](README.md) · [验证入口](Inno.Tooling.Validation.md) · [Wiki 首页](../README.md)

通过公开 `ValidationCommand.ExecuteAsync` 驱动真实临时 Git 仓库和选定 SDK，不使用 IVT、私有反射或生产后门。fixture 拥有临时源码、输出和 Git 元数据，排空子进程后清理明确的临时目录；测试程序集不进入产品。

`ValidationPlanningTests` 覆盖新增/移动/删除、imports、动态发现输入、纯文档范围、未知 owner、领域测试传递 native 依赖、空行为测试、缺失与旧输入证据、基础分支推进后的实际 merge base，以及从 development product 声明选择原生上下文而保留独立领域 owner。

另外覆盖未初始化 submodule 的明确身份、script-only Wiki 的真实产品准备、输出竞争和取消、拒绝接管未知目录，以及深层计划中的冷输入、十轮热构建和 Release 产品前提。CI PowerShell 契约分别检查结果汇总、日程、发布来源、压缩包边界和产物字节；这些负例不能替代 Windows/macOS 上的真实产品执行。

引用边的条件属性必须保留其条件源码和 native 需求；只改测试实现仍选择行为测试，但不重复验证未变化的生产 API。外部 import 即使长度和时间戳不变，内容变化也必须使旧证据失效。实际源码编译与 TRX 集成实验记录在实施记录中，不以合成结果替代真实编译失败。

全仓源码政策的输入覆盖另外包含测试 C#、文档中的 C# 模板和根级 MSBuild 属性文件，防止这些位置的 global/implicit using 变化借用旧架构结果。

CI 契约由统一入口发现 `tests/ci/*.Tests.ps1`，没有独立脚本名单；空发现必须失败。保留协议用例模拟 CLI 导出与认证的不同结果，检查失败或取消后只运输新完成的不可变归档，旧候选不能冒充新导出。模拟协议不替代真实 native 发布器与托管 CI 验收。

需要 Git 与 .NET 10 SDK。环境不可用应失败。测试类的 public 方法仅是 xUnit 发现入口，没有产品 public/protected 契约。

```powershell
dotnet test tests/tooling/Inno.Tooling.Validation.Tests/Inno.Tooling.Validation.Tests.csproj --configuration Debug
```
