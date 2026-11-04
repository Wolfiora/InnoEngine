---
name: inno-verify
description: Plan and execute InnoEngine validation, inspect failures and verify whether evidence still matches its inputs.
---

通过 Build 入口的 validate plan/run/full/evidence 选择与执行验证。读取计划中的原因、环境和排除项；未知 owner、空发现或缺失环境不能当作通过。修改后先核验证据身份，不因 commit/push/接续重复相同检查。首次失败保留；重试记录原因。设备、性能、压力和生命周期按计划执行，禁止用过滤绕过构建依赖。统一入口尚未可用时明确报告未迁移，不能伪造结果。

