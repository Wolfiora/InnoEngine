# Inno.Scripting.Compiler.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [脚本编译器](../scripting/Inno.Scripting.Compiler.md)

## 职责与边界

通过公开的 `ScriptCompiler`、资产和插件边界验证 .NET 10 / C# 14。工程不引用 Editor 呈现或原生绑定，能够在无 GPU、无绑定生成的环境运行。它不提供生产 API。

## 运行

先构建 Solution 的 SourceDebug 配置，再执行 `tests/Invoke-IsolatedTests.ps1`；独立托管 CI 使用 `tests/Invoke-ManagedChecks.ps1`。普通用例与隔离用例均保留 xUnit/VSTest。

| 场景 | 验证内容 |
| --- | --- |
| `AuthoringAndIdeAcceptTheSameCSharp14Source` | 引擎及生成 IDE 工程接受 `field`、空条件赋值、Span、泛型 `nameof`、Lambda 参数修饰符及扩展属性；执行公开探针检查默认值、异常后的状态和求值次数；collectible 探针成功卸载。 |
| `InstalledPluginExtensionMembersAreAvailableInIdeReferences` | 实际 `.iplugin` 中的扩展成员能够跨程序集被引擎和 IDE 同时消费。 |
| `FrameworkContentChangesSelectNewArtifacts` | reference pack 内容变化后重新生成 API 与脚本产物；该用例修改进程环境，使用独立 testhost。 |
| `GeneratedProjectsIgnoreAmbientBuildConfiguration` | 父目录存在拒绝构建的 `Directory.Build.props/targets` 时，生成的 Runtime 与 Editor 工程仍使用自己的完整输入完成构建。 |
| `NestedSolutionRestoresOnlyItsOwnProjects` | 仓库子目录的 Solution 正常还原自己的工程；引擎的 Solution 还原规则只作用于所属 `InnoEngine.sln`。 |
| `ScriptCachePublicationTests` | 从公开编译结果与进度观察拥有的产物：临时读取释放后发布，持续阻塞和取消不破坏前一 generation，失效缓存替换失败保留整棵原树，释放后显式编译完成修复。依赖 Windows 目录 sharing 语义的用例在其他宿主明确标为未执行。 |

Fixture 使用临时项目、公开资产挂载和序列化服务；测试完成后释放宿主及临时文件。没有 `InternalsVisibleTo`、私有反射或测试后门。
