# Foundation 引导请求身份与 Rider 文件占用验收

[Task Hosting 契约](../../../docs/build/Inno.Build.TaskHosting.md) · [构建缓存架构](../../../docs/architecture/BUILD_CACHE_ARCHITECTURE.md)

## 诊断结论

用户在 Rider 启动 Editor 时遇到 `CS2012`：编译器无法写入 Task Foundation 的 `obj/Inno.Core.IO/debug/Inno.Core.IO.dll`。开始检查时，引擎 Git 工作区干净，基线为合并 PR #60 后的 `a08f87efe`；没有发现 tracked 源码被删除。

检查时该文件已经能够独占打开，Windows Restart Manager 没有报告持续占用者。这只说明检查时没有持续占用，不能据此推断报错时的具体进程。

使用全新的验收输出目录、四个独立入口及 `-m:8`，修复前复现了同一 Foundation 闭包内的 `CS2012` 和 `GenerateDepsFile` 文件占用错误。它不依赖旧缓存或用户项目，也不需要运行 BGCS 生成。合法删除构建产物会触发冷构建，这次暴露的是中立引导请求的并发缺陷。

## 原因与修复

`PrepareInnoTaskPublisher` 将临时的 `InnoTaskBootstrapOwner` 传入 Foundation 的全局构建属性。不同入口生成不同编号，MSBuild 因而将其视为不同配置请求；这些请求却使用同一个 Foundation `ArtifactsPath`，并发写入相同的 DLL 与 deps 文件。

修复只调整 `build/msbuild/Inno.Build.Tasks.targets`：

- Foundation 不再接收 publisher 的临时操作编号，使用现有工具属性移除规则。
- 配置、真实 SDK 输入及既有产品属性隔离保持原有规则。同一中立身份的编译由 SDK 复用。
- Publisher 仍保留操作编号、所有权声明与独立私有加载目录；完整 Task runtime 仍使用现有 Core IO lease、冻结、取消和原子发布协议。
- 独立并发构建继续使用各自的 `ArtifactsPath`；没有新增跨进程协议、重试写入、全局串行化或 Rider 专属路径。

没有修改 BGCS 源码、包、第三方源码、生成绑定、平台代码、运行时 API 或 `tests/` 源码。没有改变两个开发入口。

## 实际验收

原始证据位于 `artifacts/acceptance/rider-foundation-20261101/`；该 owner 在 11 月 1 日建立，验收于 11 月 2 日完成。

| 检查 | 结果与证据 |
| --- | --- |
| 修复前并行冷构建 | 失败并保留；`baseline.log/.binlog/.json`。四个不同操作编号写入相同 Foundation 输出；Catalogs 编译执行四次，出现两项写入错误。 |
| 显式 ArtifactsPath 并行冷构建 | 通过；`fixed.log/.binlog/.json`。四次入口调用不再向 Foundation 传入操作编号；Catalogs 和 Core.IO 各实际编译一次。 |
| 默认布局并行冷构建 | 通过；`default.log/.binlog/.json`。使用验收 Task artifacts 下的默认 `foundation` 布局，保留 `-m:8`。 |
| 三次热构建 | 全部通过；`hot-1` 至 `hot-3` 的日志、binlog 和 JSON。Foundation 闭包均为零次 Csc，无写入错误。 |
| 现有相关回归 | 8 passed / 0 failed / 0 skipped；`regressions/bootstrap-regressions.trx`。覆盖 Debug/Release 菱形图、产品 TFM/RID/AOT 隔离、预求值 IDE worker、私有加载与 Foundation 可写、编译器工具边界及 Solution owner。未修改测试源码或降低断言。 |
| 正式 Editor Debug | 通过；`editor-parallel.log/.binlog`，`-m:8 -nr:false`，SDK 10.0.402，0 warning / 0 error，18m38s。原生输入完整性校验耗时期间采集的栈显示正常哈希读取，未跳过校验。 |
| FlappyBird 副本启动 | 通过；`startup.log` 和启动日志。真实 Direct3D11，隐藏窗口，600 帧，退出码 0，Editor 状态保存及资源释放完成。未修改原 Samples 项目。 |

Binlog 检查使用 `BinaryLogReplayEventSource` 公开 API。报告区分入口请求次数和实际 Csc 执行次数，不能把缓存请求事件当成重复编译。所有验收通过标准 SDK 和公开入口完成，没有测试后门或私有反射。

## 边界自查与限制

- 操作所有权与编译身份现在各自承担真实职责；没有把随机编号、平台名称或 IDE 实现放入中立编译键。
- Core IO 和既有 Task 发布机制直接复用，没有新建锁、缓存管理器或加载服务。
- 真实普通/私有输出和 IDE 预求值调用方均有回归；源码、SDK 版本和公共 API 未扩大。
- 本轮只验证该构建修复及启动，不宣称完整 FlappyBird 玩法、所有 GPU、Browser 或 macOS 产品矩阵通过。
- 没有操作 Rider 界面；Rider 若仍持有修改前的工程求值，应重新加载 Solution 后再直接 Run，入口会正常执行 Build，无需手工预构建或重新 clone。
- Editor 正常构建已执行既有自动缓存维护。本次额外的临时验收目录递归清理在进程启动前被自动审批拒绝，仅返回 `blocked by policy`；目录及证据均保留，没有改用其他删除机制。
- Windows 无指定的 `Glass.aiff`，未播放提示音。
