# Inno.Tooling.Validation

[Tooling 索引](README.md) · [Wiki 首页](../README.md) · [架构验证](Inno.Tooling.Architecture.md) · [回归](Inno.Tooling.Validation.Tests.md)

## 职责与边界

工具层库由既有 Build 入口的 `validate` 命令调用，不进入 Runtime、Player 或脚本表面，也不增加日常 executable。依赖架构验证库与中立 Build Toolchains。

托管与原生构建属性分别记录，原生目标从现有 development product 声明取得。每个属性上下文构建所需项目的并集，测试消费该上下文的 SDK 输出快照；不同目标不共享可写输出。领域测试的实际传递依赖由计划器与架构检查共同禁止进入 native 准备。

先通过选定 SDK 批量求值真实工程，再由 Git merge base、staged/unstaged/untracked 输入确定 owner 与消费者。源码、资源、imports、ProjectReference 和显式 `InnoValidationInput` 来自 SDK。未知关系扩大范围并记录原因；没有行为测试或领域测试传递引用 native 时拒绝计划。

实际引用边上的全局属性变体也进入图，条件源码和条件 native 依赖不能因根项目默认属性而丢失。托管构建并行度为处理器数与 4 的较小值；原生准备仍保持单 owner，跨 MSBuild 工作进程的准备复用尚未获得并发验收，不能提前启用。

## 公开 API

| 类型/成员 | 语义 |
|---|---|
| `ValidationCommand` | 唯一公开命令入口，没有实例或 protected 扩展点。 |
| `ValidationCommand.ExecuteAsync(string[] arguments, CancellationToken cancellationToken = default)` | 执行 `plan`、`run`、`full` 或 `evidence`。返回 0 表示请求成功；参数无效、输入过期、证据缺失或失败抛出异常，由 Build CLI 报告非零退出码。取消排空子进程。 |

计划、SDK snapshot 与结果 JSON 是内部诊断模型，不是引擎持久化协议或脚本 API。只适用于当前工具实现，不提供旧格式读取。

## 工作流与示例

```powershell
dotnet run --project entry/InnoEngine.Build --no-build -- validate plan --engine-root . --dotnet /absolute/path/to/dotnet --base origin/master --output artifacts/validation/review
dotnet run --project entry/InnoEngine.Build --no-build -- validate run --engine-root . --plan artifacts/validation/review/plan.json --output artifacts/validation/run
dotnet run --project entry/InnoEngine.Build --no-build -- validate evidence --engine-root . --plan artifacts/validation/run/plan.json --evidence artifacts/validation/run/results.json
```

SDK 路径按本机显式选择，Windows 使用 `dotnet.exe`。构建目录稳定复用，日志按运行分开。正在执行的 CLI 与待替换的构建输出必须有不同写入 owner。

`plan --full true` 生成完整功能计划而不执行；`full` 直接执行完整计划。`--extended true` 额外要求从未准备的 native 缓存开始，记录冷构建、十轮热构建，并将真实 Editor 面板生命周期扩大为 1,000 轮。已有 native 产物不能被标记为冷构建；普通修改涉及构建、SDK 或 reload 时也提前选择对应热缓存检查。

计划分别记录静态、领域、公共契约、集成、Native、产品和性能证据；编译是这些证据共享的前提。完整功能计划还准备 Release 上下文，运行真实 Release 产品旅程、验证 source-independent Player Support Pack，并在所有必要结果成功后归档同一份产物。不同配置有独立 SDK 输出；发布不会再次构建。

GitHub 的 Plan 阶段只准备工具和一个计划，Run 阶段消费该计划。`--cache-input` / `--cache-output` 指定不可变 native/bindings 的运输归档。发布器验证请求身份、完整文件集合及内容哈希，并重新建立本地登记和读取保护；只导出已实际采用的完整生成物，未采用候选不随每轮缓存累积。跨 runner 不传输活动 lease、PID、cache owner 记录或可写 `obj`。外层运输缓存以完整归档内容为键，归档包含 recipe/工具/SDK/目标/配置身份的 generation manifests；旧候选不能充当测试结果。

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

当前迁移和未完成验收见 [实施记录](../../records/architecture/plans/AI_PIPELINE_IMPLEMENTATION.md)。产品与设备矩阵仍在接入；工具回归通过不代表完整引擎认证。

<!-- api: T:Inno.Tooling.Validation.ValidationCommand 0253332414abf0d30d3c1e9deaf9e45ac081661bcecb4b2bf6a99d4d3ec3a864 -->
<!-- api: M:Inno.Tooling.Validation.ValidationCommand.ExecuteAsync(System.String[],System.Threading.CancellationToken) c89fe80bda2a1d8c91cd0790bb909429696aeddd0d61054d87c277467842a71c -->
