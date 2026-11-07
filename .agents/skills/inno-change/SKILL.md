---
name: inno-change
description: Scope an InnoEngine implementation by actual owners, behavior changes, applicable rules and finite acceptance.
---

先核对分支、差异和 owner，保护其他会话。用仓库根的 tests/ci/Test-AgentRules.ps1 按路径发现完整规则，并按行为补选；规则入口见 [rulebook](../../rulebook/README.md)。

用 Build 的 docs search/read/symbol 查现行设计和使用契约，再核对真实源码与项目关系。明确行为变化、生命周期、失败回滚和有限验收；已有 Core 能力优先。输出只保留 owner、规则单元、改动边界、必要调用方与验证理由。普通局部任务不重新审计整个引擎。

测试选择交给统一 validate plan，不保存测试名单。接续只更新 [records](../../records/README.md) 中已有计划的范围、决策、输入身份、有效证据、阻塞和下一步；无关改进另记 issue。
