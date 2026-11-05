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

现有 Build 入口的 `validate plan/run/full/evidence` 是统一验收入口，见 [验证计划](../tooling/Inno.Tooling.Validation.md)。计划从批量 SDK 图选择实际 owner 和传递消费者，将托管领域与原生产品前提分开；每个配置先构建并集，之后执行各套件。全仓源码检查、受影响编译元数据和 Wiki 检查具有独立证据，不由某个测试项目隐式重复全仓构建。引擎 target 与 .NET `RuntimeIdentifier` 分别传递，Editor 入口的直接引用与宿主产品引用共用同一目标上下文。

构建输出使用稳定的 `validate --artifacts-path <owned-directory>`；运行日志使用独立的 `--output`，不因更换日志目录制造冷构建。构建、普通测试、隔离发现和隔离执行消费同一 SDK 图和输出 owner，并通过现有登记与 lease 协议保护。

Debug 与 SourceDebug 的 PDB 嵌入实际编译源码，使用稳定的源码路径映射；开发程序集不把当前提交 SHA 写入版本属性或 Source Link。只提交验证报告不会因此重编译全部程序集，离线调试仍能读取对应源码。Release 保留 SDK 的提交 SHA 和 Source Link，验证计划与交付清单也继续记录确切提交、源码和产物身份。该配置在 SDK 确定默认 Configuration 后生效，普通 `dotnet build` 与显式 Debug 使用相同规则。

共同 props 在 SDK 默认输出路径建立前，为具有产品输入的工程选择独立 `ArtifactsProjectName`：身份覆盖 Native target、工具 target、产品 props 和 Native build fingerprint。中立工程与 Roslyn 工具保持中立，SDK 自身的 configuration、TFM 和 RID pivots 不变；还原和编译消费同一 owner。调用者显式设置的 SDK 输出属性仍由调用者负责，构建工具必须读取求值后的实际路径，不能拼接 `ArtifactsPath/bin/<project>`。这避免两个正式开发入口在同一个 SDK 构建图中争写共享工程的 DLL；不靠串行构建或清空真实 RID 规避。

Editor 开发入口只声明需要还原其宿主路由选中的产品工程。共享 `RestoreProductTargetGraph` 使用已有 `RestoreOwnedProjectTask`、Core IO lease 和 Restore/compilation 隔离，先完成该闭包，再开始入口的直接及传递引用构建。平台产品独立构建仍按同一协议准备自身；入口不保存平台名单或另建依赖解析器。

Reload fixture 使用普通 `ProjectReference` 声明构建依赖，通过 SDK 的 `OutputItemType` 接收实际产物，再部署到测试宿主的 Modules 目录；不拼接 `bin/<configuration>/<tfm>`，也不从另一输出 owner 借用 fixture。

Wiki 脚本 fixture 从测试工程实际求值的 framework `ReferencePath` 编译，并在所属输出目录保存构建清单；运行测试的 runtime 补丁与 SDK reference pack 可以不同，不按 runtime 补丁号推导 reference pack 路径。

Task fixture 通过官方 binlog replay 的 `Csc` 事件核对实际编译次数，并验证每次根构建的 owner、私有加载位置和完整请求身份复用。RID 命名 fixture 显式请求 untrimmed 图，工具包 scope 由独立用例验证 SDK 推导的能力，避免让是否安装 Wasm workload 改变测试输入。Browser 绑定 fixture 使用正式 `LibraryImport` Profile，并验证 `FunctionTable` 被 SDK 拒绝后 last-good 产物仍可校验和复用。

`Invoke-BuildCacheChecks.ps1` 消费统一计划已经准备的 Editor，连续执行十次热构建，要求 Native 进程数与 binding generation 数均为零，并核对 current 内容及时间戳稳定。

统一计划中的 Wiki 检查消费已构建的实际引用闭包；脚本示例先通过正式脚本工作流更新 IDE 投影，再从该投影读取准确的裁剪 reference。它检查 Wiki 链接、项目覆盖、源码签名及可编译示例，不按缓存目录时间猜测 reference。

```powershell
dotnet run --project entry/InnoEngine.Build -- validate run --base origin/master
```

`verify-native` 要求目标注册 Editor test host，并在生成前解析对应 SDK；没有该 runner 的目标明确拒绝。Browser 的正式生成和产品验收通过其 `bindings --product player`、Support Pack 和游戏发布流程执行。

## CI 依赖图

构建与依赖 CI 共用 `.github/actions/setup-dotnet`。它从提交的 `global.json` 读取 SDK 基线，交给固定官方 action 显式安装到 runner 的独立临时目录。安装不把 `latestPatch` 展开为下载通道；CLI 仍按原声明执行已安装 SDK 的补丁选择，运行日志记录实际 SDK 和编译器。

完整测试、Support Pack、Wiki 与 renderer 门禁按计划中已排序的前置条件执行。独立政策诊断继续收集；必需检查失败或取消后，后续昂贵上下文明确阻塞。任一必需门禁失败都会使 job 失败。

Renderer smoke 在 Debug 产品阶段的所需契约与测试通过后，通过 `Invoke-EditorSmoke.ps1` 验证进程退出、完整 Shell 释放、帧数、后端及 sRGB。默认进程期限为 180 秒，输出分别写入所属 Project 的 Logs；超时采集可用的 macOS 原生栈并终止自有进程树，输出管道也有独立收尾期限。日志中的产品资源释放不能替代完整适配器释放或进程退出。

独立绑定 Task 按组件 ID 和源码 owner 共同解析参与绑定的产品声明；共用源码的离线工具不参与运行库绑定参数选择。不同有效绑定声明仍明确拒绝，不按宿主或平台名称选择第一份配置。

- [NuGet 依赖还原与提交](DEPENDENCY_SUBMISSION.md)：标准 PackageReference、SDK Restore、包身份验证与 CI snapshot 提交。
