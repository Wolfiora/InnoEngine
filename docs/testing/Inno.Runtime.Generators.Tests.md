# Inno.Runtime.Generators.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Runtime](../runtime/Inno.Runtime.md)
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)
- [Inno.Adapter.Modules.DotNet](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Runtime.Generators](../runtime/Inno.Runtime.Generators.md)
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)

## 运行

从仓库根目录运行：

```powershell
dotnet test tests/runtime/Inno.Runtime.Generators.Tests/Inno.Runtime.Generators.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

使用项目声明的 SDK、包引用与 fixture。需要启动外部工具或子进程的场景，应按照对应源码列出的前置条件准备环境。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景；完整项目验证仍使用上面的命令。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [RuntimeModuleCatalogGeneratorTests.cs](../../tests/runtime/Inno.Runtime.Generators.Tests/RuntimeModuleCatalogGeneratorTests.cs) | `AConverterWithoutAConstructionEntryFailsDuringCompilation`、`GeneratedCatalogCompilesAndPublishesExactMetadataAndFactories` |
| [RuntimeSubsystemGeneratorTests.cs](../../tests/runtime/Inno.Runtime.Generators.Tests/RuntimeSubsystemGeneratorTests.cs) | `AddingLocalDeclarationUpdatesStronglyTypedCatalogWithoutEditingHost`、`DuplicateStableIdsFailDuringCompilation`、`InvalidFactoryBoundaryFailsInsteadOfBeingSilentlyDiscovered`、`CatalogPreservesCallerParameterNames` |
| [SerializationMetadataGeneratorTests.cs](../../tests/runtime/Inno.Runtime.Generators.Tests/SerializationMetadataGeneratorTests.cs) | `InvalidDeclarationsFailDuringGeneration`、`ExternalBaseMembersAndHooksUseTheirOwningAssemblyAccessors`、`GeneratedAccessorsRoundTripPrivateMembersHooksStructsAndClosedCollections`、`PrivateDeclarationsRemainInsidePartialOwners` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../tests/runtime/Inno.Runtime.Generators.Tests/Inno.Runtime.Generators.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。
