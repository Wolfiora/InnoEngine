# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 当前范围

2026-10-14，kernel 基线 `b488d5d8588ec56e6ca1ab50bb1f30c680e81e8f` 后的构建成本、工具宿主与缓存生命周期整改。没有自动提交。本轮不执行 Computer Use、桌面焦点、视觉或声学验收；后台与 headless 结果仅证明对应的非 GUI 行为。

详细命令、首次失败、修复及回收过程见[本轮验收记录](../../records/architecture/acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md)。历史结果不冒充当前通过证据。

后续审查发现的五项局部边界现已实现修复，本机新证据与具体限制见[局部整改报告](../../records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md)。下面产品与性能表属于上一轮已完成结果；本轮提交判断以新报告的具体范围为准。

## 已验证能力

| 范围 | 当前结果 |
| --- | --- |
| 源码配置 | 最新 SourceDebug / SourceRelease 均 0 errors、0 warnings。 |
| 构建契约 | 完整套件、受影响复验、SDK 准入与实际 CLI 引用测试按名称合并后 325 个唯一用例、剩余失败 0。重叠测试不累加。 |
| 工具进程 | 默认 JIT；30 轮 fixture、10 次真实身份检查、非法 nonce、崩溃、终止及真实十分钟截止通过。 |
| 热 Native | 三次热 Build 的 compiler、binding generation、materialization、完整 Task 编译均为 0；哈希、exports 和原子发布门禁保留。 |
| 失效与并发 | 同长度/mtime、损坏、取消、并发契约通过；两个实际 Editor Build 正常完成；小 Native 修改复用兼容 workspace。 |
| Support Pack | Windows 与 Browser 第二次相同请求复用同一内容身份，无 Native、绑定或 Player 编译。显式 SDK 同时用于工具与实际命令。 |
| Windows Player | CoreCLR、NativeAOT 重新发布；各两次 360 帧后台运行正常退出、stderr 为空、存储递增、DSP 时钟推进。 |
| Browser Player | 解释执行/AOT 重新发布；各两次隔离 headless 运行，输入、持久化、帧推进、冻结/恢复通过，JS errors=0。 |
| Editor | Debug 三次、Release 一次 120 帧后台运行正常退出；有效部署直接启动，无 Native、绑定或 Pack 准备。 |
| 公开消费 | 当前公开契约的源码编译与 Plugin 导出通过；Wiki 不以外部产品解释引擎职责。 |
| Wiki | 严格 CLI 校验使用实际生成的裁剪 references：45,592 个符号、190 段示例，错误 0；项目图 230/230，架构测试 122/122。 |
| owner | PID/启动 ticks、首次写入前 claim、读取保护、角色区分及安全退休通过；活动、损坏或未知 owner 保留。 |

NativeAOT 发现 owner 记录的反射式 JSON 读取违反裁剪要求，已改为显式读写，实际重新发布与运行通过。

## 测量

| 项目 | 实际结果 |
| --- | --- |
| Debug 热 Build | 119.047 / 146.297 / 138.343 秒，中位数 138.343 秒。 |
| Native 串行 / 批次 | 38.225 / 11.976 秒；读取 14.730 / 5.228 GB；两者 compiler=0。 |
| 小 Native 修改 | 179.015 秒、2 个工具进程、9,821 字节复制；原字节与 mtime 恢复后热命中。 |
| Pack 第二次请求 | Windows 32.188 秒；Browser SDK 修正后 32.578 秒。 |
| Debug 首帧 | 最后清理后独立启动 7.268 / 3.665 / 3.626 秒；每次 120 帧正常退出，无 Native/Task/Pack 准备。 |
| Release 首帧 | 最后清理后独立启动 7.072 秒；120 帧正常退出。 |

首帧包含部署校验、Host 与项目初始化，不是宿主查询时间。不同冷准备、SDK 切换及并行负载结果不混入热构建统计。

## 限制与提交判断

已执行的非 GUI 源码、契约和产品路径通过。完整哈希、项目求值仍有分钟级热 Build 成本，不能称引擎或构建绝无问题。提交说明应保留实际环境与未实测范围。

最后正式缓存清理实际增加 14,342,897,664 字节，清理后可用 65,568,768,000 字节；逻辑退休量不等同于卷增加量。恢复 **200,000,000,000 字节**可用空间尚未达成：37 个历史目录删除被自动审批拒绝，未绕过、未补写 owner，未清理用户、SDK 或 NuGet 缓存。

MacOSArm64 Native / Editor / Player 实机 **未实测**。GUI、完整焦点/DPI/视觉与声学按用户要求不执行；远端 CI 以实际远端记录为准。

平台 not built 表示未参加当前 Solution 配置的批量构建。两个入口与产品启动链见[启动说明](../platform/PRODUCT_STARTUP.md)。

Web headless 启动保留第三方 WebGL 能力查询和 ScriptProcessorNode 弃用警告，JS errors=0；不据此宣称没有警告或视觉/听感通过。实际 CLI 创作闭包已补齐 Animation 引用，重新生成和全例编译通过。

### 最新 CI 范围

最新已完成的 Windows/macOS CI 在 Native、托管和基础架构阶段通过，完整测试阶段仍有跨平台失败，整改及新结果见[当前局部验收报告](../../records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md)。目标限定 Wiki 验证只证明共同项目与所选产品编译；外国产品的未执行记录不计为通过，完整目标覆盖需要各自环境。
