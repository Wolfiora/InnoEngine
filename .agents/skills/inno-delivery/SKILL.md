---
name: inno-delivery
description: Review InnoEngine semantic risks and matching evidence, then prepare accurate issues or pull requests and diagnose CI failures.
---

通过规则加载器补选 delivery、review、notification，入口见 [交付规则](../../rulebook/delivery.md)。审查实际变更的依赖方向、输入来源、失败回滚、并发和资源退休，不按目录整齐或测试数量判断完成。

核对获批计划和每项证据身份，区分实现缺失、未验收及外部阻塞；未执行不能算通过。交付输出最终行为、实际检查、未完成项和行动。PR/issue 标题使用 type(scope): description，标题正文英文；不要将历史结果描述为当前通过。

首次 CI 失败先诊断根因，不循环重跑直到绿色。提交身份、push、发布及对外通信遵守 [交付规则](../../rulebook/delivery.md) 和用户授权；本地阶段性验收不自动触发大型远程矩阵。
