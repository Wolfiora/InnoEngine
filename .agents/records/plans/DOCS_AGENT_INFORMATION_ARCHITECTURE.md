# 文档与 Agent 信息架构实施

- 基线：`ai-pipeline`，`6e2cd51e4a5f7c3df1119a840e100b5cea90a57b`。
- 范围：产品文档、rulebook、skills、records、本地查询、相关规则及影响分析工具；不改引擎行为、第三方或生成绑定。
- 决策：一份事实一个权威位置；人和 AI 共用 docs；四个技能保持标准目录；查询不启动 SDK、Native 或网络。
- 清单：[逐文件迁移与原始身份](docs-agent-migration.json)。原始证据附件保持字节，历史通过不能提升为当前通过。
- 验收边界：局部工具编译、相关回归、规则/链接/文档覆盖及查询采样；不启动完整 Native、跨平台 CI/CD 或长期矩阵。
- 当前状态：本阶段功能与必要局部验收完成；实现提交 `a9b6c08ba080afcda697ba1bb18238f4f80d87d8`。#75 的未执行验收保持原状态。
- 交付证据：[阶段报告](../acceptance/DOCS_AGENT_INFORMATION_ARCHITECTURE_ACCEPTANCE.md)。后续网站与大型矩阵按原有边界独立开展。
