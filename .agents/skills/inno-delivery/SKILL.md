---
name: inno-delivery
description: Review InnoEngine semantic risks and validation evidence, then prepare accurate English issues or pull requests and diagnose CI failures.
---

读取 docs/architecture/rules/delivery.md 与 review.md。围绕实际变更检查依赖方向、输入来源、失败回滚、并发和资源退休。核对计划中每项证据及输入身份，区分缺少实现与缺少证据。PR 描述只包含最终行为、适用验证和限制；标题为 type(scope): description，正文英文。诊断首次 CI 失败并修复根因，不循环重跑直到绿色。发布、合并与对外通信遵循当前用户授权范围。

