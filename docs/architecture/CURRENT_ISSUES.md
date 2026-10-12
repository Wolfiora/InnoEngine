# 当前问题与验证缺口

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

当前正在实施开发入口、构建缓存与 Wiki 整改。以下项目未完成，不以历史测试结果替代本次验证。

| 问题 | 当前处理 | 完成所需证据 |
| --- | --- | --- |
| 重复构建及无关输入失效 | 入口默认增量、显式组件闭包与兼容 workspace 已修改；三次热构建 Native compiler/绑定生成均为零 | 小源码修改测量及整体成本收口 |
| Support Pack 准备成本 | 输入身份及准备前索引已接入；真实 Windows 准备成功，第二次请求返回相同 Pack，未再次执行 Player Build、Native compiler 或绑定生成 | 完整 managed 冻结执行、最终发布矩阵；入口自身托管准备仍有开销 |
| 托管编译输入一致性 | 固定执行 API 与实际源码提取已接入；真实 SDK A→B→A 回归通过 | 真实 Windows/Browser Pack 与最终发布消费验证 |
| Support Pack 提交后失败语义 | 唯一提交点和读取交接已修复，索引/清理错误变为提交后诊断 | 索引及清理错误回归通过；完整产品发布继续验证 |
| 构建缓存占用与活动消费 | 读取保护已接入；owner/预算/依赖退休的基础契约及专项测试通过 | 实际 publisher 与产品保护根登记、自动/手动回收及磁盘记录 |
| 缓存保护根完整性 | 初始/fresh 清理拒绝未登记根，标准 publisher 已登记 | 生产范围的依赖、reader 保护与实构建清理证据 |
| 新系统的动态 Native 加载 | 产品提供显式 Deployment.index；loader 删除命名平台与 CPU 映射 | 未知系统/自定义后缀回归通过；Windows 产品索引与 AOT 消费待实测 |
| Wiki 内容完整性 | 最新严格清单包含 45,459 个公开声明，189 段示例编译无错误；四个 Native 项目已补可折叠 ABI 索引 | 当前新增契约复验、原生行为说明与逐页审查；部分平台精确引用闭包仍有环境缺口 |
| Wiki 内容核对 | 类型修饰符及 ref-return 的签名检测已修复，28 项契约测试通过；已有映射按当前符号更新 | 缺失生成 API 与逐页行为审查仍需补齐，更新映射不等于全部内容已验收 |
| Task bootstrap 成本 | 已修复嵌套请求丢失 owner；真实并行 SDK 图通过。修复后三次完整热 Editor Build 为 172/151/146 秒，Native compiler 和绑定生成均为零 | 跨节点准备与有效输入校验成本继续核对；完整测量矩阵尚未完成 |
| 创作注解模块加载 | 长运行发现纯注解程序集被标为 Editor-only；已修正加载范围，专项加载/Player 擦除测试通过 | 实际 Editor 长运行及重载复验 |
| 本次产品验证 | Windows Editor 准备、短启动与有限真实 GUI 验收通过：Play、按钮输入、停止、附加窗口关闭、正常退出及再次启动 | 定量拖动、resize、多屏 DPI、完整 reload、Player/Web、音频与视觉矩阵 |
| 远端 CI | 本地 workflow 修改不等于远端通过 | GitHub 实际运行结果 |
| 历史 staging 访问拒绝 | 尚无本次证据证明外部来源已排除 | 故障复现或足够覆盖的并发/发布记录 |

macOS 实机、Linux 完整产品、多屏 DPI 和声学听音没有本轮证据。Linux 当前只声明已实现的 Native 工具能力。
详细执行记录与历史事实保留在仓库 records 区；当前 API 从所属项目页读取。

当前依赖图、可重现发现及提交条件见[架构审查记录](../../records/architecture/acceptance/CURRENT_ARCHITECTURE_REVIEW_2026_10_12.md)。结构门禁通过不代表上述行为问题已经修复。
