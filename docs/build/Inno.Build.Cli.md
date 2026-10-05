# Inno.Build.Cli

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Support Pack 核心](Inno.Build.SupportPacks.Core.md) · [平台组合](Inno.Build.SupportPacks.md)

## 职责与边界

引擎工具的唯一进程入口。组件 Native 工具链、Shader 编译、Support Pack 发布与架构验证都是库，由此组合。CLI 是作者端 composition root，可以加载 Editor 编译参考闭包；通用 Build 库禁止引用 Editor，Player 不包含本项目。

## 命令

```sh
dotnet run --project build/cli/Inno.Build.Cli -- engine --target windows-x64 --configuration Release
dotnet run --project build/cli/Inno.Build.Cli -- support-pack --target browser-wasm --dotnet /absolute/sdk/dotnet
dotnet run --project build/cli/Inno.Build.Cli -- game --project /project/FlappyBird --support-packs /packs --target browser-wasm --output /output
dotnet run --project build/cli/Inno.Build.Cli -- verify .
```

| 命令 | 功能 |
| --- | --- |
| `engine` | 先生成当前宿主绑定，再顺序构建 Native/工具、Editor 与请求目标 Support Pack。 |
| `clean` | 清理工作区各项目的 bin/obj 和 artifacts 下的 native/managed/support-packs/builds，保留正在执行 CLI 的输出。 |
| `bindings` | 通过各组件 BGCS 配置生成绑定。 |
| `support-pack` | 通过与 Editor 相同的发布器准备、校验并以带回滚的事务安装目标 Pack。 |
| `game` | 先准备目标 Support Pack，再冻结脚本与内容并通过注册平台输出游戏。 |
| `plugin` / `scripts` / `import-sample` | 项目作者端流程，共用现有引擎领域服务。 |
| `shader` | 调用统一 Shader 图编译库。 |
| `verify` / `verify-native` | 架构检查及 Native 完整性验收。 |

`verify-native` 将本次完整 solution 构建的 CLI 输出写入独立的 `artifacts/build-tools/verification/<runId>`，保持正在运行的验证入口可用。它不覆盖自身 DLL，也不跳过 CLI 编译；这避免 Windows 文件锁导致验证器不能构建完整 solution。

solution 的默认 Debug/Release 构建覆盖宿主源码与测试；Browser 项目保留在 solution 中用于编辑，由 `support-pack` / `game` 的目标请求构建和实际发布。直接编译 Browser 而未提供 native 绑定选择及指纹会明确失败。Web 解释执行和 AOT 的导出验收独立执行，不把宿主 solution 成功当作 Web 成功。

`--engine-root`、`--dotnet`、`--configuration`、`--output` 用于引擎流程；`verify-native` 也接受显式 `--engine-root`。
全部组件通过 `NativeBuildContext` 使用选中 checkout 的源码、overlay、按指纹隔离的中间态与原生产物。
显式根目录不会再触发对 CLI 程序集所在仓库的隐式解析；未指定时才发现默认 checkout。
项目命令的详细参数由 `help` 输出和源码解析器定义。Ctrl+C 取消当前 Native/managed 子进程树，
等待退出并排空输出，再以退出码 2 结束；不进入后续阶段或发布未完成的 staging。
Debug `engine` 构建 Editor Debug，但桌面 Support Pack 独立准备 Release runtime closure。

`game` 的两阶段协调属于 CLI composition root：先调用
`BuildPipeline.EnsurePlayerSupportPackAsync`，再在当前 authoring owner thread 调用 `BuildGameAsync`。
CLI 没有 UI scheduler，因此在入口等待准备任务完成；通用 Build 服务不会在捕获 snapshot 时隐式阻塞异步 provisioner。
Editor 使用同一准备 API，并在后续 owner-thread update 启动 build，准备期间仍可显示进度和接收取消。

## API 和生命周期

本项目不提供 public/protected API。对程序调用者稳定的入口属于各构建库，不通过反射调用 CLI 内部类型。进程退出码 0 表示成功，1 表示失败，2 表示取消。

Web 源码构建需要同一 .NET 9 SDK 的 wasm-tools，以及 PATH 上可用的 CMake/Ninja。不携带用户机器上的工具路径。原生产物可重建，不是提交到源码树的发布输入。

`import-sample` 驱动同一个 AssetSampleImportTransaction：owner 捕获与发布、后台准备与 preflight，完整保留 `~` 目录名。CLI 入口轮询已完成 phase；成功或异常退出前使用 Core RetirementBarrier 等待 Rollback/Dispose，包括取消后的私有目录清理。不保留旧的同步 AssetPipeline.ImportSample 接口。
CLI 的 Ctrl+C token 在准备、preflight 等待和最终发布前检查；取消后先排空事务，再返回取消退出码。


项目命令先由 `BuildComposition` 准备 CLI 自己的宿主 Native 和离线工具，再创建 authoring workspace。
CLI 的原生配置与其实际编译配置一致；游戏目标与 managed deployment 独立选择。
`engine` 将返回的确切 product 部署到 Editor 输出。默认 Support Pack 根为 `artifacts/support-packs`。
运行时加载器只读取部署树，项目命令不依赖隐式的 Native 复制或仓库 fallback。
