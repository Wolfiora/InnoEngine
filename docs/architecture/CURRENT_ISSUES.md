# 当前问题与未完成的验收

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

当前事实以[平台职责与缓存验收](../../records/architecture/acceptance/PLATFORM_OWNERSHIP_CACHE_ACCEPTANCE_2026_10_22.md)为准。未执行不等于已确认缺陷，契约通过不等于所有设备已验收。

## 仍开放的问题

| ID | 范围 | 事实、影响与处理边界 |
| --- | --- | --- |
| G03 | BGCS / Browser | 正式 BGCS 3.0.1 以 `BGCS-CSHARP-ABI-PLAN` 拒绝 BGFX 按值结构体调用，缺少 aggregate calling profile，阻塞 Mono Wasm / Wasm AOT。由 SDK 实现目标 ABI 后，通过原正式链复验，不补丁、不伪造、不借用宿主 ABI。见[专项记录](../../records/architecture/acceptance/BGCS301_BROWSER_AGGREGATE_ABI.md)。 |
| O01 | macOS / Metal | 历史 revision 的两条 CI 在单线程 Metal surface 退休用例超时，根因未确认。本轮没有 Mac，不能关闭或直接归责 BGCS。 |
| O02 | 完整构建性能 | 本轮有当前热构建采样及同 .NET 10 的路径解析实现对照，但没有完整改造前的空闲、同输入五次 Editor 基线。历史高负载耗时不作有效对照，不宣称整体提速或已排除全部回退。 |
| O06 | XML / Wiki 语义 | 符号、签名和示例门禁不证明全部历史注释足够具体；部分历史参数说明仍需补充实际约束和失败语义。新增构建 API 的所有权、失败与生命周期单独说明。 |
| R01 | 外部插件事件 | 外部插件仍有公开动画帧 C# delegate 事件，尚未统一 Core Events hub/token 生命周期；没有第二套完整事件总线。修改公开事件超出本轮保持插件签名的范围，见[复用审计](../../records/architecture/acceptance/ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md)。 |
| R02 | 外部插件 Identity | 外部 UI 插件跨帧状态仍持有 live owner，应在使用边界经原 Identity scope 解析。已有 Collect/Dispose 不替代统一解析要求；未复现泄漏或崩溃，见同一审计。 |

## 尚未执行或覆盖不足

| 范围 | 验收边界 |
| --- | --- |
| macOS ARM64 / Unix | 本轮无实机；Metal、Editor Debug/Release、示例产品、CoreCLR、NativeAOT 和不同卷格式未执行。历史 CI 与 fixture 仅保留原范围。 |
| Browser 产品 | 已解析实际 workload；G03 阻塞正式生成和后续产品，不能从 Windows 推断成功。 |
| Rider 交互 | SDK 入口及 Task worker 回归不等于本轮操作过 Rider Run；没有单独的 Rider 生产分支。 |
| GUI / 设备 | Play、昼夜切换、返回 Edit、窗口变化和退出按报告列出；完整 DPI/焦点、所有输入、音频听感及长期运行不计作通过。 |
| 特殊文件系统 U07 | Native 输入比较策略仍需在 Windows 大小写敏感目录及 macOS 两种卷上实测。独立探针启用属性曾被拒绝，没有有效复现；物理路径 resolver 不代表全部卷语义已解决。 |
| 历史 SDK 异常 U08 | 历史 runner 曾出现内部 `MissingMethodException`，当前未再出现，根因未确认；不写成已根治或已复现业务错误。 |

## 本轮已处理的边界

- 组件平台 JSON、共享平台表、共享 Windows junction、CLI Browser 专用分支和 import library 后缀推导，由各 owner 的明确贡献替代。
- `verify-native` 在生成前校验已注册 Editor runner，Windows 正式验收与 Browser 静态产品能力分开；R03 的产品/runner 范围错误已修正。
- BGCS 包、源码和实际加载身份分别核对；生成物只经官方生成流程更新。
- 冻结源码消费者显式接收原绑定产物 owner，不再推测位置。辅助托管求值使用 SDK 独立 ArtifactsPath，复用 Core 协调保护覆盖还原、求值及哈希，避免覆盖普通还原图。
- 活动读取者、等待者和进程 owner 受协调锁保护；只回收证明归属的 payload 和元数据，不按后缀或年龄猜权限。未知历史内容、活动 worker 和审批拒绝的独立测试目录保留并报告。
- 根 Build/Publish 和 CLI 成功路径复用同一有界维护；失败与取消不发布有效缓存。普通 FileLease 释放不等于删除锁文件。
- MSBuild Task 的编译 workspace 在初始求值、等待和执行期间持有 owner 保护，并被既有 load 进程依赖引用；同一进程操作重复登记保留完整依赖闭包。真实 SDK 构建期间三次维护保留全部已选 assets，后续 Task 复用及完整产品准备通过，关闭缓存 Restore 结果与工作区退休之间的竞态。
- Support Pack 的 Native 准备直接写入外层独占候选，不再把嵌套事务的锁和工作 claim 纳入不可变内容。准备索引通过既有缓存登记关联产物，随旧包退休；相同 current 和索引内容保留时间戳。

## 架构结论的适用范围

检查深入真实执行、依赖、失败和资源生命周期，并以公共边界 fixture 及实际调用方验证。中立层不引用具体平台或 BGCS 类型；平台基础、integration、backend、产品和发行各有职责。所有权及 IO 复用 Core，没有增加事件、Identity 或序列化平行管理器。

此结论不关闭 R01/R02，也不把构建体系整理表述为整个引擎完美。持续自查要求见根 `AGENTS.md` 第 30 节。
