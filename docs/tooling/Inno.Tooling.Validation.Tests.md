# Inno.Tooling.Validation.Tests

`ValidationSolutionTests` 使用真实 SDK 重现 IDE 配置未勾选构建时 `.slnf` 成功但不产出程序集的情况，再编译统一入口保留的 `.slnx`，验证所有选中 owner 参与构建且 SourceDebug 不被 IDE 重映射为 Debug。测试同时确认政策失败不会提前执行该构建。

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
