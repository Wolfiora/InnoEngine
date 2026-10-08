# Inno.Tooling.Architecture

[Tooling 索引](README.md) · [Issues](../issues/README.md) · [整改规格](../issues/2026-08-31-architecture-remediation-master-plan.md)

退休 catch 的 AST 规则拒绝直接捕获 `RetirementPendingException` / `RetirementTimeoutException`（包括全限定名）。
应捕获 Exception，并用 Core `RetirementPendingException.Find` 检查整棵异常树后原样传播。
规则包含两个反例和一个合法 filter 正例；它是源码约束，不声称静态证明任意动态异常、类型别名或 native callback 行为。

Registry 派生实现不得直接调用 `OnCleanupFailed` 代替退休失败传播。源码 AST 检查会拒绝该绕过；统一清理应使用 `DisposeExtensions` 并让 TypeRegistry 封锁 generation。负向行为见[CLI 测试项目](Inno.Tooling.Architecture.Tests.md)。

本项目是架构验证库，由统一 Build CLI 调用。`ArchitectureValidator.Execute(arguments, cancellationToken)` 从包含
`InnoEngine.sln` 的根目录加载源码与 `.csproj` 图，返回 0 表示通过，1 表示违反项；源码根缺失时抛出
`DirectoryNotFoundException`。可指定源码根与 `--configuration Debug|Release`，默认 Debug。
未知选项、重复配置和多个根抛出 `ArgumentException`。
默认检查手写源码的多参数声明：逐参数换行、右括号对齐，以及紧随参数列表的 `) {`。
检查基于 C# 语法节点，覆盖方法、构造函数、委托、主构造函数、局部函数和 Lambda，不改写源码。

## 检查范围

源码检查之外，工具通过 Roslyn 读取明确选定配置的程序集元数据，递归检查 public/protected
基类、接口、返回值、参数、泛型约束与外层封闭泛型、数组、tuple、指针及 unmanaged function pointer 中的依赖。
它能识别类型别名和多行签名，不依赖名称正则。有效可见性同时考虑外层容器，internal/private 容器中的 public
成员不被误判为对外 API；private-protected 成员不形成程序集外扩展契约。对应正反例由真实编译 DLL 驱动的 CLI 测试覆盖。
Editor 公开签名中实际需要的项目不得标为 PrivateAssets=compile；只用于实现的项目必须声明 PrivateAssets=compile。
ProjectReference 收口为前置实现组和后置公开 API 组，禁止混合分组、重复类别分组或颠倒顺序。
这两项判断基于实际程序集的公开签名；只看运行时会不会加载某个程序集不能决定其传递性。
Host/Shell 不得公开具体 Adapter。
Native 符号检查目前覆盖 BGFX、SDL3、MiniAudio；ImGui presentation 的边界仍由专项规则约束，
不宣称已经通过统一规则证明所有 ImGui 类型均不可见。

必须先构建 Solution，再运行 CLI 审计；工具不替代编译，也不证明任意插件的动态线程行为。

- friend assembly、Obsolete、type forwarder、兼容字段和禁用实现名；
- global/implicit using、循环 ProjectReference 和 removed project；
- Core/Build/Runtime/Rendering/Audio/Native 依赖方向；
- Player dependency closure；
- Native 类型泄漏和 Engine 内部脚本 Log facade；
- tests 的 non-public reflection；
- `src`、`backends`、`platforms`、`build`、`tools` 的全部创作源码项目必须进入 Solution，按真实职责和组件/平台 owner 归类；验证工具共用 `tools`；
- 全部 test project 必须进入 Solution；组件测试归所属 backend，通用领域/集成测试位于根 `tests`；TestModule/TestAssembly/TestDependency 保留 fixture 职责；
- `Inno.Audio` 不得引用 Runtime、Scene、Editor、Platform、Native 或具体 backend；只有 MiniAudio adapter/toolchain/native/tests 可直接引用 native binding；
- Audio scripting 清单不得导出设备、native binding 或 backend，MiniAudio 实时适配源码不得持有托管 extension generation/reflection 对象；
- public/protected 多行英文 XML 的 summary/param/typeparam/returns/exception contract。

```text
dotnet build InnoEngine.sln -m:1 -p:UseSharedCompilation=false
dotnet run --no-build --project build/cli/Inno.Build.Cli -- verify .
```

指定 `--dotnet <绝对 SDK executable>` 后还会通过 SDK 的只读 MSBuild item/property 查询检查实际导入后的
ProjectReference、Compile、程序集身份、产品与 Native target。不会执行 Restore、Build、Native 或生成 target。
`--project-graph <output.json>` 同时保存此次有效图，必须与 `--dotnet` 一起使用。每个查询有一分钟预算；取消会
停止并完成已启动的子进程。检查包含 imported 平台依赖、普通源码的第二 owner、重复程序集、循环与 Player 创作/构建闭包。
这些检查与源码/公开 API 检查共同使用，不能用只读 evaluation 代替真实编译、发布或实机运行。

`--expand-xml` 只机械展开已有 XML 标签，不改变说明内容。缺失说明由作者补齐，
工具不根据方法名称猜测语义，也不把继承契约替换为占位文本。正常 CI 不使用写入参数。

## 库入口与 Host 隔离检查

项目现在为库，公开 ArchitectureValidator.Execute(arguments) 返回 0/1，默认从当前目录寻找源码根，首个参数可指定根。使用统一 CLI verify 调用。新检查拒绝重复 Native Browser 项目、额外工具 Exe、共享 Foundation/Shell/Player Runtime 的 IsBrowser 判断及 Browser Player 的源码链接。核心 Build 的 Editor 禁止依赖规则继续生效，仅 Build CLI composition root 可组合作者端参考。

BGCS/Cpp2C target profile 必须显式声明 Native owner 内的输出目录，与宿主输出不能相同或互相包含。
此规则覆盖 managed binding 和 C++ bridge，避免任一生成器的原子目录替换删除另一目标的产物。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Tooling.Architecture.ArchitectureValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Tooling.Architecture.ArchitectureValidator`](../../tools/Inno.Tooling.Architecture/ArchitectureValidator.cs#L16) | Checks repository dependency, API documentation and source ownership invariants. |
| [`static int Inno.Tooling.Architecture.ArchitectureValidator.Execute(string[] arguments, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../tools/Inno.Tooling.Architecture/ArchitectureValidator.cs#L55) | Executes repository validation or an explicitly requested documentation maintenance operation. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../build/Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
