---
name: inno-verify
description: Plan and execute bounded InnoEngine validation; diagnose failures and reconcile evidence with current inputs.
---

从 [验证规则](../../rulebook/validation.md) 通过规则加载器取得本次适用单元。调用 Build 的 validate plan/run/full/evidence，读取 owner、选择理由、环境、前置产物与排除项；不另维护测试名单。

先核验证据身份，再补跑无效或缺失项。记录修改、提交、push 和恢复会话不是重复测试的理由。保留首次失败及重试原因；未知 owner、空发现、跳过或缺少环境不能算通过。设备、性能、压力和生命周期按实际计划执行。

输出检查状态、输入身份、失败原因、修复与尚缺证据，写入已有 [验收记录](../../records/acceptance/README.md)。用户限定本地局部验收时明确保留大型矩阵，不自动扩大执行范围。
