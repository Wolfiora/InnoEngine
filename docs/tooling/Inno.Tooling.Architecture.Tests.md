# Inno.Tooling.Architecture.Tests

[Tooling 索引](README.md) · [架构工具](Inno.Tooling.Architecture.md) · [Wiki 首页](../README.md)

## 职责与边界

位于真实 `tests/tooling/Inno.Tooling.Architecture.Tests` 和相同 Solution Folder，验证公开架构命令对错误源码的拒绝行为。它不是 Runtime/Player 依赖，也不提供游戏 API。

全部规则 fixture 直接调用公开 `ArchitectureValidator.Execute`，不引用开发入口，也不为每个用例重新准备 Build 工具链。命令路由由 Build 测试中的 `ArchitectureVerificationCommandTests` 通过公开 `BuildApplication.Run` 单独验证。不引用工具 internal 类，不使用 IVT 或反射测试后门。

## 验证协议

`ArchitectureCommandTests.CliValidatesSourceContracts` 每次在临时目录建立隔离 solution，先验证其已知的三项“缺少产品 composition 项目”基线，再写入单项源码验证额外违规是否符合预期：

- global using。
- Player 泄露具体 Bgfx backend。
- 业务代码手工拼接 Asset resolver context。
- 非 native API 暴露 Ma 类型。
- 用 `OnCleanupFailed` 诊断回调吞掉资源退休失败。
- 直接 catch Pending / 全限定 Timeout，遗漏包装异常树。
- 合法 `Exception` + `RetirementPendingException.Find` filter 不新增违规。

该项目没有 protected 扩展契约。参数与输出经过实际公开命令入口；测试集合串行捕获诊断并在 finally 恢复 Console 与清理临时目录。SDK 图测试仍调用真实的 design-time MSBuild；坏例不进入产品源码，也不会禁用原架构规则。

## 编译符号矩阵

`ArchitectureSymbolTests` 使用与工具相同的 Roslyn package，在临时 fixture 中编译真正的 dependency/product DLL，
放入对应项目的 Debug 输出位置，再调用公开验证命令。符号断言检查编译后的公共表面，不能靠文本扫描碰巧通过。
没有引用工具 internal 类型、IVT、反射穿透或生产后门；不把这些故意缺少完整产品的 fixture 当作可运行引擎。

- 字段、属性、protected/protected-internal 方法、事件、delegate、基类和接口。
- 类型/方法 generic constraints、嵌套公开类型、数组、Task/集合嵌套、tuple。
- unsafe pointer、unmanaged function pointer 参数与返回值、封闭泛型外层中的 nested type。
- private、private-protected、internal 容器与 private nested type 的正向允许用例。
- BGFX、BGFX Tools、SDL3、MiniAudio 的真实 assembly identity，避免改类型名称或 alias 绕过规则。
- Shell/Player/Editor 的具体 Adapter 暴露与中立 contract 正向允许。
- Editor ProjectReference 的真实公开依赖、`PrivateAssets="compile"` 与有效可见性。
- SDL3 consumer 规则使用当前名称拼写；只允许对应 Platform/ImGui presentation、Native、toolchain 和 tests。

这组矩阵验证静态编译边界，不证明任意动态 callback、反射生成、原生函数入口或 GC 行为安全。
静态门禁和产品运行的当前结果统一记录于[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。历史审查编号不作为当前 API 或验收依据。

## 工作流

SDK 引用隔离测试建立两个无依赖关系的真实项目，验证所属项目 API 可用、另一项目即使已经编译也不能被示例调用。同时检查 `Compile Remove` 和条件编译符号实际决定 API 清单。使用 design-time `ResolveReferences`，不执行 Native 或产品准备。

`WikiCoverageTests` 通过公开验证入口构造临时 Wiki：检查项目唯一页面、索引、签名变更、缺失符号、相对链接、锚点和外部产品边界。所有页面的 C# 示例都参与编译，包括架构页和分类索引。逻辑脚本例使用独立生成的 Runtime/Editor 契约 fixture，验证缺引用、错程序集角色及宿主实现不可见的失败路径；这组测试不代替实际引擎裁剪 reference 的全量示例验收。

```sh
dotnet test tests/tooling/Inno.Tooling.Architecture.Tests/Inno.Tooling.Architecture.Tests.csproj
dotnet run --project "entry/InnoEngine.Build/InnoEngine.Build.csproj" -- verify
```

当前工具的符号检查要求 Debug solution 产物；负向源码 fixture 故意不伪造整个产品。测试不能替代完整解决方案归类检查、真实编译符号审计或运行时 GC/Recovery 验证。VSTest 需要本地进程通信权限。

## 实际仓库门禁

RepositoryArchitectureTests 通过公开验证入口检查当前 checkout 的源码、项目、Solution、引用和架构规则。小型正反例与当前仓库必须分别通过；Wiki 的实际 SDK 引用、API 和示例门禁由 Build 测试中的 RepositoryWikiTests 执行。

`RepositoryInputTests` 通过公开 assembly metadata 构造可回收的独立程序集，验证 [Inno.Testing.Source](../testing/Inno.Testing.Source.md) 的所属输入隔离，以及空值、缺失、相对路径、无效目录和重复声明的拒绝。测试只使用公开反射/Emit API，不读取或改写生产私有成员。
