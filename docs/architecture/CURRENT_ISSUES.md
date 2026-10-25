# 当前问题与未完成的验收

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

当前复验见[BGCS 4.1.2 消费验收](../../records/architecture/acceptance/BGCS412_CONSUMER_ACCEPTANCE_2026_10_25.md)；前序平台和缓存结果按其原记录范围保留。未执行不等于已确认缺陷，契约通过不等于所有设备已验收。

最新全仓深审见[四仓库核心验收报告](../../records/architecture/acceptance/FULL_ENGINE_AUDIT_2026_10_24.md)。正式架构门禁通过不表示资源清理、错误原子性和热路径成本都已闭合。

## 本轮已修复并验证

F02/F08 已在 Core Coroutine owner 内统一修复：嵌套枚举器逆序释放，完成/停止/失败共用退休流程；重入停止失效 handle 后在回调返回安全点清理，不再改坏活动遍历。Debug/Release 各 40 项回归通过，Runtime 62 项通过。异常明确汇总，未增加第二套调度器或事件系统。

F01 的引擎路径也已闭合：Browser 平台选择官方 LibraryImport，解释执行与 AOT 产品均运行 3,000 帧。BGCS 4.1.2 已补齐 Browser import admission 和 target provider fingerprint，两个 SDK issue 已关闭；本地公开 API 探针、正式生成及实际产品复验通过。Runtime 回归不证明 session admission 已接入，见 R02。

本次消费升级另外修复 Task Restore 与 compilation 的求值隔离；不再把新 DLL 与旧 package content items 混入同一 publication。修复位于中立 MSBuild 边界，真实公共 Task 回归和七个正式生成 owner 已通过；没有修改 SDK 或添加包名特例。

完整证据与全部英文 issues 见[本轮专项报告](../../records/architecture/acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)。以下未关闭项目仍需按 issue 验收。

## 仍开放的问题

| ID | 范围 | 事实、影响与处理边界 |
| --- | --- | --- |
| R02 | Runtime coroutine 接入 | 私有 scheduler 只有创建、Tick、stop 和 Dispose，当前没有 admission 路径；需落地最小真实契约或删除未使用状态，不能宣称已集成。 |
| T01/T02 | Browser 验收覆盖 | 非致命 console 404 尚未归因；最高分跨启动写入没有在有界游戏运行中触发，已各自提交 issue。 |
| B01/B02 | 工具与仓库卫生 | Windows native 源码编码告警、额外 target/RID 锁 scope 的归属与收敛需要继续处理。 |
| F03 | History 拒绝操作的原子性 | 非 LIFO Commit 在校验 ID 前弹栈，public handler 反例确认内层修改无法随后回滚/Undo。不能用正常顺序用例通过关闭此项。 |
| F04/F05 | 插件 UI / 2D 热路径 | UI 插件在纹理 GPU 命中前按 draw 完整复制像素；2D 模型的非空 viewContent 使完整帧缓存持续绕过。见全仓报告的分配实测和调用链，尚未修复。 |
| F06/F07 | 热构建与缓存维护成本 | 暖 Native 准备反复读取约 4.5 GB；当前 5 秒 inventory 超时，30 秒 dry-run 可完成。内容/owner 正确性协议已存在，但没有以此证明日常维护必然收敛或性能足够。 |
| F09 | 测试产物作用域 | 包身份用例写死默认 obj，隔离 ArtifactsPath 下误读另一还原的旧包身份。实际隔离 assets 与锁文件均为当前版本；不能通过删除缓存或额外还原掩盖测试输入错误。 |
| O01 | macOS / Metal | 历史 revision 的两条 CI 在单线程 Metal surface 退休用例超时，根因未确认。本轮没有 Mac，不能关闭或直接归责 BGCS。 |
| O02 | 完整构建性能 | 本轮有当前热构建采样及同 .NET 10 的路径解析实现对照，但没有完整改造前的空闲、同输入五次 Editor 基线。历史高负载耗时不作有效对照，不宣称整体提速或已排除全部回退。 |
| O06 | XML / Wiki 语义 | 符号、签名和示例门禁不证明全部历史注释足够具体；部分历史参数说明仍需补充实际约束和失败语义。新增构建 API 的所有权、失败与生命周期单独说明。 |

## 尚未执行或覆盖不足

| 范围 | 验收边界 |
| --- | --- |
| macOS ARM64 / Unix | 本轮无实机；Metal、Editor Debug/Release、示例产品、CoreCLR、NativeAOT 和不同卷格式未执行。历史 CI 与 fixture 仅保留原范围。 |
| Browser 产品 | 4.1.2 Native 与两种发布通过，解释执行和 AOT 各运行 3,000 帧。完整交互、游戏得分跨启动保存及 Browser 矩阵仍未闭合。 |
| Rider 交互 | SDK 入口及 Task worker 回归不等于本轮操作过 Rider Run；没有单独的 Rider 生产分支。 |
| GUI / 设备 | Play、昼夜切换、返回 Edit、窗口变化和退出按报告列出；完整 DPI/焦点、所有输入、音频听感及长期运行不计作通过。 |
| 特殊文件系统 U07 | Native 输入比较策略仍需在 Windows 大小写敏感目录及 macOS 两种卷上实测。独立探针启用属性曾被拒绝，没有有效复现；物理路径 resolver 不代表全部卷语义已解决。 |
| 历史 SDK 异常 U08 | 历史 runner 曾出现内部 `MissingMethodException`，当前未再出现，根因未确认；不写成已根治或已复现业务错误。 |

## 本轮已处理的边界

- 2026-10-22 插件审计中的 R01/R02 已通过插件源码、Core 事件异常回归、弱 Identity 生命周期回归及正式安装态验证关闭；[该次复验](../../records/architecture/acceptance/PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md)明确范围。它们与本页 Runtime coroutine 接入的 R02 是不同报告内的编号，不代表 Runtime admission 已完成。

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

此前插件事件 / Identity 的具体缺口已在后续插件复验中关闭；本页仍开放问题继续按各 issue 验收。构建和插件验收不等于整个引擎完美。持续自查要求见根 `AGENTS.md` 第 30 节。
