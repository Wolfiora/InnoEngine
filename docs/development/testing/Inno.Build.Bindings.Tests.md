# Inno.Build.Bindings.Tests

[测试索引](README.md) · [绑定工具](../build/Inno.Build.Bindings.md)

## 职责与执行前提

验证绑定 SDK 的配置、输入身份、原生到托管代码生成、不可变产物与读取者生命周期。这些回归需要还原的 BGCS SDK 及其 worker，可以在受支持的 Windows/macOS 主机运行；不准备 Editor、窗口、GPU 或引擎 Native 产品。

## 设计与实现

每个用例拥有独立临时根，通过公开 `NativeBindingGenerator`、profile 和产物协议操作真实文件。输入与生成使用实际 SDK，失败、取消和损坏不能替换已发布的完整 generation。跨进程字典顺序用例继续启动独立测试进程；父进程等待子进程完成后清理它拥有的文件。

本 owner 从 Build 产品集成套件迁入五份测试，断言和失败场景保持。原生编译器部署、产品导出、绑定消费的 MSBuild 集成仍归属 [Inno.Build.Tests](Inno.Build.Tests.md)，不以 SDK 用例替代产品验收。

## 使用与覆盖

常规任务通过统一 `validate plan/run` 按实际项目引用选择本套件。定位 SDK 回归时也可执行有界项目测试：

```shell
dotnet test tests/build/Inno.Build.Bindings.Tests/Inno.Build.Bindings.Tests.csproj -c Debug
```

- 配置边界：拒绝可执行、非字面量、越界声明和未声明目标。
- 生成与选择：输入变化、同长度变更、损坏修复、只检查模式、输出闭包及稳定项目选择。
- 批次与所有权：共享读取、等待锁后的新鲜度、活动消费者、取消和失败不发布部分结果。
- profile：显式 Browser 扩展、SDK 路径和 ABI 选择，保留既有环境隔离。
- 独立进程：对象键顺序不改变相同配置的内容身份。

托管测试承载不代表不使用 SDK 的 native worker；它表示没有引擎 Native 产品准备依赖。缺失 SDK 仍然失败，不能跳过后声称通过。

## 源码场景

| 测试文件 | 场景入口示例 |
| --- | --- |
| [BindingDefinitionTests.cs](../../../tests/build/Inno.Build.Bindings.Tests/BindingDefinitionTests.cs) | `ExecutableOrNonLiteralDefinitionsFailBeforeOutputCreation`、`UndeclaredTargetNeverFallsBackToTheHostAbi` |
| [BindingGenerationBatchTests.cs](../../../tests/build/Inno.Build.Bindings.Tests/BindingGenerationBatchTests.cs) | `OneBatchReusesSharedReadsOnlyWithinEachFreshPhase`、`ActiveConsumersBlockReplacementAndReleaseAllowsRepair`、`AFailedBatchPublishesNoPartialClosureAndPreservesEarlierOutputs`、`FrozenSdkPathsSelectIndependentHeadersWithoutChangingTheProcessEnvironment` |
| [NativeBindingGenerationTests.cs](../../../tests/build/Inno.Build.Bindings.Tests/NativeBindingGenerationTests.cs) | `ManagedSelectionPreservesExactNativeIdentitiesInStableProjectOrder`、`PublicationRejectsChangedOrIncompleteManagedSelection`、`CollidingManagedSelectionDoesNotReplaceAnExistingCompleteSelection`、`UnsafeManagedProjectNameIsRejectedBeforeCreatingSelectionOutput` |

生成回归检查未选目标映射不失效、选中配置变化仍产生独立 generation。用例保持顺序调度；锁竞争和取消由所属场景显式建立。
