# Kernel 提交后的问题整改与验收

基准：`e17a4a419721a283b3c9a8e3a856ffefa7b7acac`，分支 `kernel`。本记录持续更新，当前不是最终完成声明。

## 用户问题

用户已将上一阶段提交到 kernel，要求继续彻底解决审查发现与未完成计划，执行验收并检查是否还有问题。没有授权自动提交，本轮不提交。

## 当前整改

| 发现 | 当前实现 | 已取得证据 / 剩余门禁 |
| --- | --- | --- |
| F01 托管编译读到可变源码 | 新 ManagedBuildExecution 固定仓库输入；实际编译与 PlayerLink 提取消费该副本 | 真实 SDK 的 A→B→A 反例回归通过；实际 Windows/Browser Pack 仍待执行 |
| F02 提交后错误伪装成失败 | 返回 publication：唯一读取 owner 加独立提交后诊断；current 前取得读取 lease | preparation index IO 失败与 staging 删除失败回归通过 |
| F03 保护根遗漏 | 初始与 fresh 清理均拒绝未登记保护根 | 缺 root、跨 owner 依赖、活动 reader、回滚测试通过 |
| F04 缓存未接入 | 发布者登记 Native、绑定、Pack 和 workspace；标准发行提供 owner；CLI 手动与有界自动回收 | producer/reader 交接与跨 owner 依赖回归通过；实际清理、任务缓存完整预算仍需闭合 |
| F05 Build 自身重复编译 | 部署清单与实际输入身份校验；可退休检查上下文；有效时复用 CLI | 生产入口的两次运行及性能证据待取得 |
| F06 Wiki / 产品验收不完整 | 同步新契约、纠正旧绑定目录和生成命令，严格当前 SDK 门禁进行中 | 全量签名、示例、链接、Native 阅读结构和产品矩阵仍需闭合 |
| F07 Native loader 命名平台表 | 产品提供 Deployment.index，loader 只消费显式逻辑映射 | 未知系统/自定义库后缀、越界、缺失、损坏回归通过；实际动态产品待验 |

目标绑定统一迁至 `artifacts/bindings/<component>/<target>/<fingerprint>`。组件 ID 由唯一 bindings.props 显式声明，MSBuild 与直接生成不再分别从项目名推导。

## 本轮通过的测试批次

- `remediation-regressions.trx`：77 通过，0 失败，包括真实 SDK 编译及实际 MSBuild 目标绑定消费。
- `cache-producer-integration.trx`：38 通过，0 失败，包括标准 owner、跨 owner 绑定保护、活动 publisher/reader、预取消及 workspace 消费。
- 前期专项：固定输入/发布边界 36 通过，部署索引与 Native publication 17 通过。

批次有重叠，不能相加作为不同测试数量。此前两次失败尝试保留日志：预检时提前创建目录、清理预算未覆盖 planning 的测试已修复；一次原生绑定消费因测试期间修改构建输入而正确拒绝 Task publication，源文件稳定后重跑通过。

## 尚未完成的验收

完整 Windows Editor Debug/Release、CoreCLR/NativeAOT Player、Web 解释执行/AOT、公开 API 消费者、reload/GC barrier、GUI/音频/视觉和性能矩阵需要本轮执行。macOS 实机需要其他环境，Linux 仅已有工具能力。没有实测的项目不写成通过。

原始命令、日志、TRX、文件映射位于 `artifacts/acceptance/build-entry-cache-wiki/20261012-remediation/`。当前磁盘仍有约 115 GiB 可用；仅回收本轮明确 owner 且失去引用的中间缓存，保留有效产物和证据，不清理 SDK/NuGet/用户缓存。

## 追加阶段记录

- Build 工具实际第二次运行 8.86 秒，使用完整源码身份与部署校验，未编译 CLI。
- 首次检查上下文退休失败，定位到反射式 JSON 清单类型缓存。清单改为显式读写后实际退休通过；没有忽略弱监测。
- 严格 Wiki 当前 SDK 检查覆盖 232/232 项目，报告 81 项问题；已修正新增契约映射、英文 XML 排版、Interop 对基础 IO 的明确依赖及 foreign Player 实现注释，待最终复验。
- 当前 45,525 个声明的映射核对缺失为零；这不替代全量行为审查。
- ImGui/MiniAudio/SDL/BGFX 的大型 ABI 页改为同一权威项目下的主题参考，入口由约 0.9–5.8 MB 降至约 3–5 KB，全部声明保留。
- 实构建拒绝了未纳入清单的 BGCS 持久协调文件；已记录发布者拥有的完整根文件集合，未删除协调文件。
- 固定托管源码的实构建发现 vendor `tools/bin` 被机械排除；已改为识别实际项目的生成目录。对应 SDK 编译回归增加 vendor/bin 输入。
- 最新 Windows Pack 仍在实际准备：固定源码复制已完成、Task runtime 通过并开始构建所需 Shader tools。此前失败保留旧完整 Pack，不标记本次发布通过。

## 实构建与后续校验

- 固定源码 Windows Pack 成功发布：1394.11 秒；其中实际 Player 编译约 13 分 10 秒。路径与部署索引保留在日志，Native 映射覆盖五个所选运行库。
- 两次热请求返回同一 Pack，336.61 / 359.67 秒，没有再次运行 Player Build；与严格 Wiki 同时进行，不能作为独占环境性能结论。准备前的完整验证仍明显耗时。
- 实构建暴露的独立 BGCS Runtime Restore 已移到 Task 身份冻结之前。编译阶段不重复 Restore；锁后及编译后验证保留。
- 最新专项批次 67 项中 66 项通过；唯一失败是系统工具无帮助文本，长路径启动与退出均成功。移除无关输出断言后，Task/长路径 19 项全部通过。
- Wiki/架构契约 40 项通过，包括 Windows CRLF API 分页、包含循环/越界、内容边界与当前结构规则。
- 全 SDK Wiki 门禁覆盖 232/232 项目；分页 CRLF 漏读导致 32,854 个声明未进入读取结果，已修复读取器。另三项外国目标引用未准备及一个 XML 缺口，未把门禁写为通过。
- 已把 Pack 热命中的锁后验证合并到同一锁阶段的最终选择 gate；未命中仍在编译前验证。固定源码 workspace 使用稳定 BindingSelection.props，避免临时导入位置改变。该修改待实构建复验。
- 自动回收先前在 5 秒内未完成目录计数；当前改为 publisher 持有写权限时记录大小，清理按记录做预算且删除前重新检查路径。大小不作为内容有效性证据。
- 缓存登记补齐 evaluator、生成扩展、Task host/load/publisher/workspace；活动进程与 load→host 依赖保护，扩展结果持有读取 lease。本轮 17 条可重建登记数据同步到当前字段，没有添加旧格式读取或删除产物。
- 新增 owner 范围、实时清理、后续 Pack、Editor/Player/Web、视觉与性能矩阵仍在执行，不能宣称整份计划完成。

## 当前 owner 整合结果

- `all-cache-owner-regressions-fixed.trx`：70 通过，0 失败，0 跳过。新增覆盖进程 load 对 host 的保护、损坏大小仍不能通过内容校验、Task runtime/workspace 所有权。初次 16 项失败均因公开 fixture 不产生 SDK workspace，已在 Task 取得写 lease 后显式创建自己拥有的目录，未增加测试后门。
- 当前完整 CLI 闭包编译 0 warnings / 0 errors。
- `cache-current-dry-run.json`：11.01 秒，登记 6,568,284,880 字节，保护 6,544,419,985 字节，32 个候选。
- `cache-current-execute.json`：成功回收 23,864,895 逻辑字节，0 skips / 0 failures；卷 free 从 112,820,641,792 增至 112,846,307,328 字节。不是全部 C 盘占用的回收证明。
- 最新 Debug Editor 正在构建，尚未取得本次 GUI、再次启动和完整发布证据。Runtime 测试也在执行。
- 剩余实现核对：显式按目标缩小 cache clean 范围尚未接入；Windows/macOS Pack validator 仍含标准后端文件名名单，需从平台职责移到所选组件闭包。
- 本轮全量 Wiki 的目标引用缺口仍需按当前源码和真正目标准备；不能使用其他配置 DLL 充当通过证据。

- `runtime-remediation.trx`：61 通过，0 失败。`rendering-runtime-remediation-full.trx`：86 通过，0 失败；此前 7 项筛选只覆盖空内核边界，不与完整测试数相加。ImGui 首次整批运行在开始测试前主动取消，因为 Faulted retirement 必须独立进程；将分开执行。

## 2026-10-13 GUI 中止点

最新 Debug Editor 普通构建通过，托管阶段 0 warnings / 0 errors；准备耗时 38:06.81。真实启动观察到无参数目录选择及 FlappyBird，Play/按钮输入/停止/第一次正常退出通过；再次启动使用准备产物，Host 创建到首帧约 3.5 秒。三次自动拖动未成功复现浮动视口，不能标记拖动或附加窗口关闭通过。第二次关闭操作遇到 physical Escape，Computer Use 按用户停止信号中止；第二次正常退出未确认。没有自动提交。

剩余：cache clean 目标筛选、平台 Pack validator 的后端文件名名单、重复 BGFX tools/workspace 身份核对、最新 Pack 修复后的实构建、严格 SDK Wiki 与 foreign 目标引用准备、Release/AOT/Web/消费者/GUI/视觉/完整性能矩阵。

