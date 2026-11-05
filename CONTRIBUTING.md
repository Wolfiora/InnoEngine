# 参与 InnoEngine 开发

从 [产品文档](docs/README.md) 理解当前能力，从 [构建指南](docs/development/building.md) 准备环境。

开发要求只有一份权威来源：[rulebook](.agents/rulebook/README.md)。它同时适用于人类贡献者和 agent，包含架构、代码、文档、测试、交付及领域要求。根 [AGENTS.md](AGENTS.md) 说明如何按改动路径和行为取得完整规则。

开始修改前明确 owner、公开行为和验收范围；用 Build 的 `validate plan` 查看选择理由，再执行所需检查。查询引擎知识使用 `docs search/read/symbol`，维护方法见 [文档指南](docs/development/documentation.md)。

实施计划和验收写入 [records](.agents/records/README.md)，不混入产品用法。交付准确说明已执行与未执行项；历史报告不代表当前提交通过。
