# Inno.Build.Toolchains.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [中立工具链](../build/Inno.Build.Toolchains.md)

## 职责与边界

验证中立构建契约，直接引用 Inno.Build.Toolchains。测试不依赖 BGCS、Editor、具体原生后端或私有实现。

`NativeBindingCompilationTests` 经公开的 Load、ResolveRoot、VerifyInputs 契约验证文件身份与映射。fixture 只创建当前描述格式，每例使用独立临时目录，并在 Dispose 中释放文件。

`ManagedProjectGraphCaptureTests` 检查真实 SDK 的批量属性、引用与编译符号；`ProductNativePreparationTests` 检查冻结请求、独立读取保护、内容失效及等待取消。`BuildCacheInventoryProgressTests` 覆盖只读 dry-run、单个 writer 使其分片失效，以及保护关系不完整时不得退休任何分片。`NativeArtifactImportTests` 将纯 payload/manifest 导入另一个 fixture checkout，验证发布器重新登记 owner，并拒绝同长度篡改、漏文件、额外成员和错误身份；真实 producer 的首次失败不会被缓存隐藏。

## 公开测试入口

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

公开行为覆盖不代表原生编译、导出、ABI 或产品运行已经通过。这些结果分别记录在[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。测试不提供受保护扩展点，也不持有 collectible generation。
