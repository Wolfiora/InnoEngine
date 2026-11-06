# Inno.Build.Toolchains.Tests

[测试与 fixture 索引](README.md)

SDK 图回归覆盖普通和 browser 限定框架的实际引用 pack 选择，以及显式 `TargetingPackVersion` 覆盖；不根据宿主平台猜测框架版本。

`ToolchainDiagnosticTests` 启动实际 SDK 的失败目标，确认捕获输出的调用仍保留原始诊断、退出失败与内层异常，避免验收报告只剩没有原因的退出码。

盘点接续回归通过真实文件系统事件在 64 个分片的首批 checkpoint 落盘后取消维护，确认完整保护图建立前没有退休任何产物。后续维护保留已完成分片的内容与时间戳，完成全部盘点后才允许退休；没有取消专用生产入口。

[测试索引](README.md) · [Wiki 首页](../../README.md) · [中立工具链](../build/Inno.Build.Toolchains.md)

## 职责与边界

验证中立构建契约，直接引用 Inno.Build.Toolchains。测试不依赖 BGCS、Editor、具体原生后端或私有实现。

`NativeArtifactIdentityTests` 和 `NativeVerificationPhaseTests` 从 Build 集成套件迁入本 owner，保留原有内容身份、目录别名、同长度／同时间戳修改及阶段去重断言；这些输入协议测试无需准备 Native 产品。`NativeInputBatchTests` 通过公开指纹入口验证批量别名去重、读取失败后的完整重试、所有 reader 排空和输入盘点完成后的取消，失败或取消不得留下可当作成功使用的部分快照。

`NativeBindingCompilationTests` 经公开的 Load、ResolveRoot、VerifyInputs 契约验证文件身份与映射。fixture 只创建当前描述格式，每例使用独立临时目录，并在 Dispose 中释放文件。

`ManagedProjectGraphCaptureTests` 检查真实 SDK 的批量属性、引用与编译符号；`ProductNativePreparationTests` 检查冻结请求、独立读取保护、内容失效及等待取消。`BuildCacheInventoryProgressTests` 覆盖只读 dry-run、单个 writer 使其分片失效，以及保护关系不完整时不得退休任何分片。`NativeArtifactImportTests` 将纯 payload/manifest 导入另一个 fixture checkout，验证发布器重新登记 owner，并拒绝同长度篡改、漏文件、额外成员和错误身份；真实 producer 的首次失败不会被缓存隐藏。

## 公开测试入口

`DevelopmentBuildProvenanceTests` 在独立 Git fixture 中使用仓库实际 props/targets 和 SDK。默认配置、显式 Debug、SourceDebug 以及 CI 模式的报告提交须保持 DLL/PDB 字节与 DLL 写入时间；直接解码 portable PDB 核对嵌入源码。Release 在报告提交后仍更新精确提交 SHA 和真实 PDB Source Link。此回归从仓库内执行，输出目录可以独立指定。

| 方法 | 场景 |
| --- | --- |
| RemappedInputsPreserveRootRelationshipsAndVerifyExactContent | 保留重叠根、有序宏和翻译单元标记；同长度内容变化仍失败 |
| UnsafeInputPathsAreRejectedWhenReadingTheDescription | 拒绝越界、绝对、驱动器和反斜杠路径 |
| LengthMismatchAndCancellationFailBeforeAcceptingAnInput | 拒绝长度变化并保留取消语义 |
| RootIdentitiesBelongToEachDescription | 同名逻辑根只属于各自清单，未知根明确失败 |
| Dispose | 释放本用例拥有的临时文件 |

## 执行

```shell
dotnet test tests/build/Inno.Build.Toolchains.Tests/Inno.Build.Toolchains.Tests.csproj -c Release
```

公开行为覆盖不代表原生编译、导出、ABI 或产品运行已经通过。这些结果分别记录在[当前验收](../../../.agents/records/acceptance/current.md)。测试不提供受保护扩展点，也不持有 collectible generation。
