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

`Invoke-BuildCacheChecks.ps1` 准备一次 Editor 后连续执行十次热构建，要求 Native 进程数与 binding generation 数均为零，并核对 current 内容及时间戳稳定。

`Invoke-WikiChecks.ps1` 在已构建的 Build entry 和所选 Player 引用闭包上运行，先通过正式脚本工作流更新 IDE 投影，再从该投影读取准确的裁剪 reference。它检查 Wiki 链接、项目覆盖、源码签名及可编译示例，不按缓存目录时间猜测 reference。

```powershell
./tests/Invoke-WikiChecks.ps1 -ProjectDirectory /path/to/Project -Target windows-x64
```

`verify-native` 要求目标注册 Editor test host，并在生成前解析对应 SDK；没有该 runner 的目标明确拒绝。Browser 的正式生成和产品验收通过其 `bindings --product player`、Support Pack 和游戏发布流程执行。

## CI 依赖图

- [NuGet 依赖图提交](DEPENDENCY_SUBMISSION.md)：固定包版本、锁文件还原与 CI snapshot 提交。
