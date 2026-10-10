# 统一构建

[Wiki 首页](../README.md) · [平台包](../platform/README.md) · [共享后端](../backends/README.md)

构建机制、具体实现和发行注册分别维护。Editor、CLI 和 MSBuild 消费同一显式发行定义；共同 Build 不依赖具体发行或 Editor。

| 项目 | 职责 |
| --- | --- |
| [Inno.Build](Inno.Build.md) | Profile、内容/代码快照、Game/Plugin pipeline、诊断与原子提交。 |
| [Inno.Build.Managed](Inno.Build.Managed.md) | 可替换 managed deployment 契约与能力目录。 |
| [Inno.Build.Toolchains](Inno.Build.Toolchains.md) | 冻结目标/宿主/组件、进程执行、哈希、Native 发布与部署机制。 |
| [Inno.Build.Composition](Inno.Build.Composition.md) | 绑定完整平台贡献的中立不可变组合。 |
| [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md) | 唯一内置注册与标准产品组件闭包。 |
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

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：唯一 BGCS 消费实现，供中立生成契约组合。
