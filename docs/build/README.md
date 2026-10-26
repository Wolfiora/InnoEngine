# 统一构建

[Wiki 首页](../README.md) · [平台包](../platform/README.md) · [共享后端](../backends/README.md)

构建机制、具体实现和发行注册分别维护。Editor、CLI 和 MSBuild 消费同一显式发行定义；共同 Build 不依赖具体发行或 Editor。

| 项目 | 职责 |
| --- | --- |
| [Inno.Build](Inno.Build.md) | Profile、内容/代码快照、Game/Plugin pipeline、诊断与原子提交。 |
| [Inno.Build.Managed](Inno.Build.Managed.md) | 可替换 managed deployment 契约与能力目录。 |
| [Inno.Build.Toolchains](Inno.Build.Toolchains.md) | 冻结目标/宿主/组件、进程执行、哈希、Native 发布与部署机制。 |
| [Inno.Build.Composition](Inno.Build.Composition.md) | 绑定完整平台贡献的中立不可变组合。 |
| [Inno.Build.Products.Standard](Inno.Build.Products.Standard.md) | 可复用产品的组件闭包、逻辑依赖与加载清单。 |
| [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md) | 内置平台、backend integration 与托管部署实现的显式注册。 |
| [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md) | 中立供给、验证、准备和原子发布。 |
| [Inno.Build.Tasks](Inno.Build.Tasks.md) | 普通 Build/Publish 的薄 Task 入口，产品与工具属性隔离。 |
| [Inno.Build.TaskHosting](Inno.Build.TaskHosting.md) | 小型引导发布库；按内容复用完整 Task runtime，并管理私有 reader 的退休。 |
| [Inno.Build.Cli](Inno.Build.Cli.md) | 唯一构建命令程序。 |

Windows、MacOS、Browser 的 target/source/validator/template 属于各平台 Build 包。DotNet compiler 属于 [DotNet backend](../backends/DotNet/README.md)。BGFX、SDL3 等组件 recipe 属于各自 backend。不存在 Host Toolchain 的固定组件列表或 Desktop 发布目标。

```text
显式 Profile / 产品目标
→ distribution 与能力预检
→ 冻结宿主、SDK、组件与绑定
→ 内容/代码/Native/managed 准备
→ 所属平台布局与完整性校验
→ 原子提交
```

游戏导出目标不改变正在运行的 Editor 目标或 ImGui Shader。CLI 的 `--tools-target` 与 `--target` 分别指定作者工具和游戏目标。失败与取消保留上次完整输出；不从旧目录读取兼容产物。

## 共享绑定生成

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：统一 BGCS 生成与发布实现；平台 integration 使用官方公开配置贡献 Profile。

## 验证入口

`Invoke-IsolatedTests.ps1` 同时执行普通套件、隔离用例发现和逐进程执行；`-Filter` 作用于三个阶段。真实 Native 测试使用明确的 `-Target` 与 `-ProductProperties`，不以中立 fixture 代替 ABI 验收。

`Invoke-ManagedChecks.ps1` 从实际引用图选择不依赖 Native 生成及产品准备的工程。标记 `BuildGraph=Complete` 的仓库集成测试要求完整 `SourceDebug` 输出，在产品 CI 的完整套件中执行；独立托管阶段将其前置条件写入 `deferred-test-context.json`，其余架构 fixture 测试继续执行。引擎 target 与 .NET `RuntimeIdentifier` 分别传递，Editor 入口的直接引用与宿主产品引用共用同一目标上下文。

需要验证全新托管输出或并行验收时，可对 `Invoke-ManagedChecks.ps1` 指定独立的 `-ArtifactsPath`；构建、普通测试、隔离发现和隔离执行共用该 SDK 输出 owner。

Reload fixture 使用普通 `ProjectReference` 声明构建依赖，通过 SDK 的 `OutputItemType` 接收实际产物，再部署到测试宿主的 Modules 目录；不拼接 `bin/<configuration>/<tfm>`，也不从另一输出 owner 借用 fixture。

Wiki 脚本 fixture 从测试工程实际求值的 framework `ReferencePath` 编译，并在所属输出目录保存构建清单；运行测试的 runtime 补丁与 SDK reference pack 可以不同，不按 runtime 补丁号推导 reference pack 路径。

Task fixture 通过官方 binlog replay 的 `Csc` 事件核对实际编译次数，并验证每次根构建的 owner、私有加载位置和完整请求身份复用。RID 命名 fixture 显式请求 untrimmed 图，工具包 scope 由独立用例验证 SDK 推导的能力，避免让是否安装 Wasm workload 改变测试输入。Browser 绑定 fixture 使用正式 `LibraryImport` Profile，并验证 `FunctionTable` 被 SDK 拒绝后 last-good 产物仍可校验和复用。

`Invoke-BuildCacheChecks.ps1` 准备一次 Editor 后连续执行十次热构建，要求 Native 进程数与 binding generation 数均为零，并核对 current 内容及时间戳稳定。

`Invoke-WikiChecks.ps1` 在已构建的 Build entry 和所选 Player 引用闭包上运行，先通过正式脚本工作流更新 IDE 投影，再从该投影读取准确的裁剪 reference。它检查 Wiki 链接、项目覆盖、源码签名及可编译示例，不按缓存目录时间猜测 reference。

```powershell
./tests/Invoke-WikiChecks.ps1 -ProjectDirectory /path/to/Project -Target windows-x64
```

`verify-native` 要求目标注册 Editor test host，并在生成前解析对应 SDK；没有该 runner 的目标明确拒绝。Browser 的正式生成和产品验收通过其 `bindings --product player`、Support Pack 和游戏发布流程执行。

## CI 依赖图

构建与依赖 CI 共用 `.github/actions/setup-dotnet`。它从提交的 `global.json` 读取 SDK 基线，交给固定官方 action 显式安装到 runner 的独立临时目录。安装不把 `latestPatch` 展开为下载通道；CLI 仍按原声明执行已安装 SDK 的补丁选择，运行日志记录实际 SDK 和编译器。

完整测试、Support Pack、Wiki 与 renderer 门禁按各自成功的构建前置条件执行，独立验证失败后仍收集其他适用门禁的结果。任一门禁失败都会使 job 失败；取消或构建前置条件失败时不执行依赖检查。

Renderer smoke 紧随 Debug 产品构建，通过 `Invoke-EditorSmoke.ps1` 验证进程退出、完整 Shell 释放、帧数、后端及 sRGB。默认进程期限为 180 秒，输出分别写入所属 Project 的 Logs；超时采集可用的 macOS 原生栈并终止自有进程树，输出管道也有独立收尾期限。日志中的产品资源释放不能替代完整适配器释放或进程退出。

独立绑定 Task 按组件 ID 和源码 owner 共同解析参与绑定的产品声明；共用源码的离线工具不参与运行库绑定参数选择。不同有效绑定声明仍明确拒绝，不按宿主或平台名称选择第一份配置。

- [NuGet 依赖还原与提交](DEPENDENCY_SUBMISSION.md)：标准 PackageReference、SDK Restore、包身份验证与 CI snapshot 提交。
