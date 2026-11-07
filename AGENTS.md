# InnoEngine agent instructions

架构、职责归属、依赖方向、低耦合和资源所有权优先于功能便利与速度。复用 Core IO、Events、Identity、Serialization；禁止平行管理器、测试后门、旧版兼容层和隐式 fallback。源码注释与公开 XML 使用英文，产品文档正文使用中文。

## 开始任务

1. 确认分支、工作区差异、任务 owner、行为变化与有限验收范围。保护其他会话改动；相邻仓库只读，除非用户明确授权。
2. 从根目录或子目录启动都先运行 `tests/ci/Test-AgentRules.ps1 -Paths <仓库相对路径> -ShowInstructions`。路径始终相对仓库根，也可使用绝对路径。按行为用 `-Rules <单元>` 或 `-Behaviors <标签>` 补选；单元、关联关系和路由唯一登记于 [规则登记](.agents/rulebook/coverage.json)。不能仅依赖客户端发现嵌套指令。
3. 加载器给出完整章节、来源、选中理由及字节数。超过 16 KiB 时按输出的 `-Stage 1`、`-Stage 2` 等分阶段读完全部适用单元；禁止截断或跳过。根指令硬上限 8 KiB。新边界使用 `-Behaviors new-boundary`，交付使用 `-Behaviors delivery`；完整组合由登记维护。

## 工作入口

- [inno-change](.agents/skills/inno-change/SKILL.md)：owner、行为、规则和有限范围。
- [inno-verify](.agents/skills/inno-verify/SKILL.md)：统一 validate 计划、失败处理与证据身份。
- [inno-api-docs](.agents/skills/inno-api-docs/SKILL.md)：实际符号、使用契约、项目文档与示例。
- [inno-delivery](.agents/skills/inno-delivery/SKILL.md)：语义审查、真实验收和交付。

技能只定义工作流程，不能替代适用规则。默认一个 owner 一个实现者；并行必须拥有独立文件责任、工作区和输出 owner，禁止重复全仓扫描、构建和验收。

## 权威内容与完成边界

- [docs](docs/README.md)：人和 AI 共用的引擎设计、原理、用法及 API；优先通过 Build 的 `docs search/read/symbol` 读取相关章节。
- [rulebook](.agents/rulebook/README.md)：修改仓库的要求；人类贡献者从 [CONTRIBUTING](CONTRIBUTING.md) 进入同一规则。
- [records](.agents/records/README.md)：必要计划、长期决策和验收。接续只记录范围、决策、当前输入身份、有效检查、阻塞与下一步，不默认全文加载历史。
- “彻底完成”指本任务全部适用行为、调用方、项目引用、公开 XML、文档和验证均完成。无关改进另记 issue，不无限扩大任务。允许修改任务必需的回归、契约及验证工具测试；禁止无关重写、弱化断言、反射穿透和扩大生产可见性。
- 验证证据必须匹配当前输入；缺失、跳过、不可用环境不等于通过。提交、push、报告修改或接续本身不是重复验收的理由。最终核对获批计划，明确实际证据、未完成验收和外部阻塞；可继续的工作自动继续。
- 构建输出按工作区、配置和目标稳定复用，日志独立归档。本机使用 C 盘明确 owner；临时产物按现有 lease、登记与退休协议清理，不猜测归属，不清理其他会话。远程交付遵守用户授权与 delivery 规则。
