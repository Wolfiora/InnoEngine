# 从产品文档迁出的状态片段

以下原文来自本次迁移前的页面。日期、通过状态和范围只属于原记录，不作为当前提交的证明；持续生效的机制与使用契约留在产品文档。

## 1. docs/architecture/identity-and-reload.md

> 文中“必须”和“禁止”是架构约束；“当前实现”与“当前缺口”来自本次源码审计。

## 2. docs/architecture/identity-and-reload.md

2026-09-08 生产追加：`ReferenceRecoveryTransaction` 已实现共享 `IGenerationChange` 五阶段契约。

## 3. docs/architecture/identity-and-reload.md

2026-09-08 领域追加：Audio Provider 的部分构造由 TypeRegistry 的候选 ownership 登记，Provider/Clip/Bus

## 4. docs/architecture/identity-and-reload.md

2026-09-08 下一批已接通 Rendering Provider/Pipeline/Feature、reload Complete/Rollback、Runtime 和 GPU

## 5. docs/architecture/identity-and-reload.md

这些是当前已接入并验证的 Scene + Asset-reference + History 链路，不代表 Assets 全量、Graph、Settings、
Plugin availability 已完成共同批次迁移。下列全域目标仍保持其原有验收强度。

## 6. docs/architecture/identity-and-reload.md

这不代表正常帧内所有 GPU 替换/淘汰路径、Rendering 全部资源职责拆分或全域 Recovery 已完成；继续以累积收口报告为准。

## 7. docs/development/build/Inno.Build.SupportPacks.Core.md

已通过的当前证据包括 fixture 并发只准备一次、热命中跳过 producer、损坏/额外文件修复和输入变更拒绝。真实 Windows 请求与性能正在复验，不能据此宣称完整发布矩阵或全部缓存预算已经完成。

## 8. docs/architecture/build-and-cache.md

## 当前能力与未完成部分

## 9. docs/architecture/build-and-cache.md

真实 Windows 第二次请求已确认跳过实际准备。

## 10. docs/development/testing/Inno.Rendering.Shaders.Tests.md

上述 public 方法是 xUnit 发现入口，不是产品 API。2026-09-11 最新检查点 107 项 case 全部通过；其中 15 项调用本机 Metal 原生编译器。
