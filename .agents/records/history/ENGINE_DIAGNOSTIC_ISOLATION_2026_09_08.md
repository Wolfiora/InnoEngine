# Edit / Play 诊断隔离历史验证

从 docs/modules/runtime/Inno.Engine.Default.md 迁出的历史原文。以下结果只描述原记录时点，不证明当前提交通过。

### Edit / Play 诊断隔离验证（2026-09-08）

原先 Animation 使用固定 source `inno.animation.bindings`，创建 Play 会话会撤销仍在使用的 Edit Reporter；随后更新或退出触发 `DiagnosticReporter` 过期异常，并使退休 gate 进入 Faulted。修复位于默认工厂的 source 构造，不改变 Core Reporter 的代际保护或 GC 屏障。

通过公开 `DefaultEngine.CreateSessionSubsystems` 和 `EngineHost.CreateSession` 的仓库外验证程序，修复前复现相同退休异常；修复后通过 32 次 Edit/Play 循环、三个并存会话、不同退出顺序、Missing 诊断互不覆盖与 Host 正常释放。现有 Diagnostics、Animation、Runtime、Editor PlayMode、Editor Audio、Editor Scripting 六个测试项目共 128 项通过；Solution 零警告/错误，Architecture 通过。本次未修改 tests，不把这些程序化验证称为真实 GUI 点击验收或全量发行验收。
