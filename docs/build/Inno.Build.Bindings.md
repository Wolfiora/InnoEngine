# Inno.Build.Bindings

[Build 索引](README.md) · [Wiki 首页](../README.md) · [中立工具链](Inno.Build.Toolchains.md) · [当前验收](../architecture/BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_ACCEPTANCE.md)

## 职责与边界

共享绑定生成应用库是唯一 BGCS 消费边界。`NativeBindingGenerator` 实现中立 `INativeBindingGenerator`，直接调用 BGCS 公开库；不引用 MSBuild Task。Toolchains、平台模块、Support Pack 只借用中立接口。标准发行创建实现，CLI、Editor 构建和薄 Task 传递同一服务。此程序集不能进入 Player 运行闭包。

## 配置与执行

每个组件的 `Bindings/bindings.props` 是唯一生成入口声明。它记录相对于文件的 host config、可选 bridge/extension 和明确 target 映射；ABI/marshalling 仍属于 BGCS JSON。声明拒绝条件、表达式、执行代码、路径逃逸和目标缺失，不尝试其他 ABI。

1. 冻结组件、目标、HostSource/TargetArtifacts、CheckOnly 与预期指纹。
2. 解析定义、准备实际 extension，取得全部 generation lease（规范路径排序）。
3. 在 fresh binding-locks 阶段校验完整输入；批次内物理读取去重。
4. C++ facade → BGCS Cpp2C C bridge → C header → BGCS managed 单文件；纯 C 从 header 开始。完整目标缓存可复用。
5. fresh binding-generation 校验，取消检查，再发布全部完整结果；失败不返回部分闭包。

路径配置中的 `%NAME%` 只从本次 `NativeToolchainSelection.environment` 展开；managed 与 C++ bridge 使用相同规则。
直接库调用不会继承为子进程准备的环境，也不修改进程环境。缺少或嵌套未展开的变量在候选创建前明确失败。
CLI 和 MSBuild 的 TargetArtifacts 请求先通过同一发行解析明确目标的工具链；HostSource 不隐式选择发布目标。
独立绑定定义不必注册游戏发布目标；MSBuild 仅取得发行明确贡献的 SDK。
未贡献 SDK 的独立定义仍可解析自身完整的路径和 ABI，但不能展开缺失的 SDK 变量，也不借用另一目标。

## 输出与所有权

HostSource 使用组件已有 `Generated/Bindings.cs` 及 `Native/Generated`。TargetArtifacts 使用组件 `obj/<target>/<fingerprint>/Generated`、`Native` 与完整性 manifest。候选由批次 owner 清理；已完整发布的独立缓存可留存。生成锁位于组件 `obj/bindings/generation.lock`，锁文件保留。扩展产物位于 `artifacts/build-tools/bindings/extensions`，拥有独立完整性、源码/SDK 身份与 lease。

CheckOnly 生成候选并比较完整输出，不能改写当前产物。对外请求不暴露 BGCS/MSBuild 类型。`WithBindingGenerator` 不接管 provider 生命周期。GenerateAsync 返回成功的只读完整映射；参数、缺 SDK、错误定义、损坏或变化输入和取消均明确失败。没有 provider 的实际生成请求在 staging 前失败。

## 使用示例

```csharp
using System.Threading;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;

NativeBuildContext operation = new NativeBuildContext(engineRoot, "release")
    .WithToolchain(selectedTools)
    .WithBindingGenerator(new NativeBindingGenerator(selectedDotnetHost));
var generations = await NativeBindingPreparation.PrepareAsync(
    operation,
    components,
    CancellationToken.None);
```

示例中的 engineRoot、selectedTools、selectedDotnetHost、components 由产品组合预检并冻结。内部 reader、identity、batch、publication 不作为稳定 API。

## 验证入口

`BindingDefinitionTests`、`BindingGenerationBatchTests`、`NativeBindingGenerationTests`、`NativeBindingCompilationTests` 覆盖定义、bridge、扩展/消费、冻结 SDK 路径、CheckOnly、失败、取消、篡改和输出保护。生成业务 fixture 直接测试公开生成库；MSBuild 入口另由实际项目构建验证。实际结果见当前验收报告。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.Bindings.NativeBindingGenerator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Bindings.NativeBindingGenerator`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L12) | Executes the shared facade-to-C-to-managed binding workflow independently of MSBuild. |
| [`Inno.Build.Bindings.NativeBindingGenerator.NativeBindingGenerator(string dotnetHost)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L25) | Captures the managed tool executable used only when a declared generator extension must build. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBindingGenerationDescriptor>> Inno.Build.Bindings.NativeBindingGenerator.GenerateAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBindingGenerationRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L32) | See the implemented contract. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- `BGCS`：实现依赖，PrivateAssets="compile"。
- `BGCS.Cpp2C`：实现依赖，PrivateAssets="compile"。
