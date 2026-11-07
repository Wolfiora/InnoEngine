# 文档、Agent 规则与本地查询：阶段验收

本阶段功能和批准范围内的局部验收完成。实现提交为 a9b6c08ba080afcda697ba1bb18238f4f80d87d8，分支 ai-pipeline，基线 6e2cd51e4a5f7c3df1119a840e100b5cea90a57b。后续交付补齐 rulebook coverage 的 Git ignore 例外路径和本报告附件；未 push、未触发远程矩阵、未部署网站，未关闭 #75。

## 交付与归属

- docs 保存产品设计、原理、用法、API 与使用边界；rulebook 保存修改要求；四个标准 SKILL.md 保存工作流；records 保存计划、决策和证据。
- [598 项迁移清单](../plans/docs-agent-migration.json) 覆盖全部基线文件；72 份原始附件哈希不变；[434 页审查登记](../plans/docs-agent-page-review.json) 覆盖全量分类、链接、包含和迁移发现。
- 246 个实际项目均有唯一同名入口，测试与 fixture 页统一位于 development/testing；59 个长入口按完整 API 章节拆分，Native .Api 分片仍由原入口纳入覆盖。
- [规则登记](../../rulebook/coverage.json) 保留 241 条原规则和 26 个补充来源章节，形成 58 个完整加载单元。根 AGENTS 为 3268 字节；每阶段包含根指令并受 16 KiB 预算约束，不截断规则。
- 规则、skill 与记录不进入默认 docs 查询；记录修改不进入 Native 配方或全部测试的政策输入。未知可执行 Agent 输入仍保守扩大验证范围。
- 引擎运行逻辑、公开产品 API、第三方、生成绑定和相邻仓库未修改。

## 移除的旧机制

删除旧 docs/architecture/rules、根 records 和原 16 个领域根分类，删除旧 build/entry/tooling 文档入口；没有转发页。C# 与维护规范归 rulebook，产品原理保留在 docs，历史状态归 records。32 个迁移后空目录已安全移除。

移除 Wiki 对旧规则目录的特例、按分片文件名猜 owner 的选择方式，以及把整个 .agents 当作政策输入的行为。标题、链接、围栏与 inclusion 共用 Markdig 语法树；现有示例编译改为消费相同语法树。规则路由与行为标签统一由 coverage.json 维护。

## 查询与正确性

既有 Build 入口提供 docs index/search/read/symbol；无需新可执行项目。Markdig 1.5.1 限于工具层。查询只读实际文档与项目清单，索引复用 Core IO 和 Build 的 owner、lease、原子写入；不执行 SDK、Native、引擎构建、文档代码或网络请求。

精确 XML ID 和 API 锚点返回声明与所属使用约束；非精确符号返回候选；零命中明确为 Empty；无效目标失败。长页返回章节导航或带 offset 的连续原文；Navigation/Partial 不能当作完整契约。缓存覆盖内容、路径、包含和工具身份，损坏派生数据会重建，失败包含图和取消不发布错误索引。

代表性入口已核对：ConsoleStyle 属于 Logging 内部呈现；Panel 创建由 Registry/Host 驱动、退休不保留帧借用；Identity 区分 persistent 与 runtime/generation；Native 消费者持有独立读取保护；backend 页面解释线程、句柄、回调和第三方限制。Asset 删除的 durable tombstone 目标与实际 Library 边界已明确分开，原整改要求保留在 [open-items](../open-items.md#持久删除身份的既有缺口)。

## 局部验证

[检查结果](docs-agent/checks.json) 记录 51 项文档/Wiki 回归、49 项验证工具回归、规则加载与负向用例、全仓廉价源码规则、全部文档链接/覆盖，以及五个选定 owner 的 513 项真实 SDK API 声明和 5 个示例编译。重复运行不累加为新测试数量。

首次失败保留：中文标题锚点、旧 API 重复锚点、越界异常契约、历史缺失链接、分页与查询排序均按原语义修正；端到端夹具改为包含当前工作区输入，保留时间戳的 Git 暂存显式重新读取内容。未弱化原断言或添加产品测试入口。两项 Completion 复验 19 分 29 秒，两项真实宿主/联合测试复验 25 分 44 秒，属于验证工具自身的端到端检查，不是日常文档查询成本。

## 查询采样与读取量

同一 Windows 主机与 .NET 10.0.402，首次空索引使用同一 docs 和真实项目清单的独立临时副本；完成后释放全部 lease 并清理副本。首次索引 10.427 秒，共 434 页。各 warm 查询 5 次，原始样本、字节、章节数、状态、来源和快照均在 [性能附件](docs-agent/query-performance.json)。最终采样时 SDK/测试进程已完成，Git 暂存也在该批期间发生；不宣称专用空闲主机基准。

| 查询 | 中位数 | JSON 字节 | 返回章节 | 状态 |
| --- | --- | --- | --- | --- |
| console | 3.842 s | 3659 | 5 | Found |
| identity | 4.434 s | 7975 | 11 | Found |
| toolchains | 3.804 s | 5594 | 1 | Navigation |

| 原整页读取 | 基线字节 | 当前入口字节 |
| --- | --- | --- |
| console | 7466 | 8487 |
| identity | 27193 | 28034 |
| toolchains | 362130 | 64307 |

此表比较读取量，不把不存在的旧 docs CLI 耗时虚构为改造前速度。Console 是导航摘要，Toolchains 的续节仍需按任务读取；缩小结果没有把约束算作已读。另保留与端到端测试并发时的较慢样本，不以重测隐藏资源竞争。

## 保留边界

Web Docs、部署、远程 MCP、向量数据库与额外 LLM 服务未实现，属于明确排除范围。#75 的 Windows/macOS 全矩阵、设备、长期运行、正式性能目标和草稿发布演练仍待原计划验收。

本轮全量归类与迁移审查不等于重新认证所有历史 API 的运行语义。原 O06 XML/Wiki 语义债务保留；没有将旧签名标记、历史通过或最新文档索引冒充当前完整源码认证。查询仍明确显示 Unverified，所选源码检查另存证据。

当前索引保留约 48 MB 的 434 份页面载荷；过期已发布载荷由同一 owner 在独占保护下回收。测试和冷查询的独占临时副本均随生命周期清理；原始失败诊断保留。本机 Glass.aiff 不存在，未播放完成提示音。
