# Inno.Build.Bindings

[Build 索引](README.md) · [Wiki 首页](../README.md) · [中立工具链](Inno.Build.Toolchains.md)

## 生成结果的读取生命周期

`NativeBindingGenerator.GenerateAsync` 返回 `NativeBindingGenerationResult`。结果按 Native 项目路径索引完整生成闭包，并拥有每个结果的读取保护。描述符只是路径与身份，不会延长缓存的存活期；调用方必须在编译、链接或复制结束后释放结果。

目标生成缓存允许多个 reader 同时消费。修复损坏产物和回收目录必须先取得 `BuildArtifactWriteLease`，等待所有 reader 释放。取消一个等待者不会释放其他调用者持有的保护。

Host source 是可变的工作区输出，因此生成结果继续持有该组件的 generation owner，直到消费结束。目标产物使用独立读取 lease，不在整个 Native 编译期间阻塞其他有效读取者。

MSBuild 在读取保护期间把唯一的 `Bindings.cs` 复制到本次 SDK 构建的 `obj/.../bindings/Bindings.cs`。编译器随后只消费该副本；相同字节不重复写入，不更新时间。目标、指纹或完整文件集合不匹配时直接失败。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

static async ValueTask PrepareAsync(
    INativeBindingGenerator generator,
    NativeBuildContext context,
    NativeBindingGenerationRequest request,
    CancellationToken cancellation
) {
    using NativeBindingGenerationResult result =
        await generator.GenerateAsync(context, request, cancellation);
    foreach (NativeBindingGenerationDescriptor generation in result.Values)
    {
        // Compile, link or copy the selected outputs before disposing result.
        System.Console.WriteLine(generation.fingerprint);
    }
}
```


## 职责与边界

共享绑定生成应用库是唯一 BGCS 消费边界。`NativeBindingGenerator` 实现中立 `INativeBindingGenerator`，直接调用 BGCS 公开库；不引用 MSBuild Task。Toolchains、平台模块、Support Pack 只借用中立接口。标准发行创建实现，CLI、Editor 构建和薄 Task 传递同一服务。此程序集不能进入 Player 运行闭包。

## 配置与执行

每个组件的 `Bindings/bindings.props` 是唯一生成入口声明。它记录相对于文件的 host config、可选 bridge/extension 和明确 target 映射；ABI/marshalling 仍属于 BGCS JSON。声明拒绝条件、表达式、执行代码、路径逃逸和目标缺失，不尝试其他 ABI。

生成身份使用本次选择的配置、目标、输出模式、扩展和实际输入。新增未选择目标的映射不会使当前生成失效；修改当前配置、facade、include、SDK、工具或生成实现仍会失效。每个验证阶段重新解析定义，不能借此跳过当前选择变化的检查。

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
using System.Collections.Generic;
using System.Threading.Tasks;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;

static async ValueTask InspectBindingsAsync(
    string engineRoot,
    string selectedDotnetHost,
    NativeToolchainSelection selectedTools,
    IReadOnlyList<NativeComponentDescriptor> components,
    CancellationToken cancellationToken
) {
    NativeBuildContext operation = new NativeBuildContext(engineRoot, "release")
        .WithToolchain(selectedTools)
        .WithBindingGenerator(new NativeBindingGenerator(selectedDotnetHost));
    using NativeBindingGenerationResult generations =
        await NativeBindingPreparation.PrepareAsync(
            operation, components, cancellationToken);
    foreach (NativeBindingGenerationDescriptor generation in generations.Values)
        System.Console.WriteLine(generation.bindingsPath);
}
```

示例中的 engineRoot、selectedTools、selectedDotnetHost、components 由产品组合预检并冻结。内部 reader、identity、batch、publication 不作为稳定 API。

## 验证入口

`BindingDefinitionTests`、`BindingGenerationBatchTests`、`NativeBindingGenerationTests`、`NativeBindingCompilationTests` 覆盖定义、bridge、扩展/消费、冻结 SDK 路径、CheckOnly、失败、取消、篡改和输出保护。生成业务 fixture 直接测试公开生成库；MSBuild 入口另由实际项目构建验证。实际结果见当前验收报告。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Bindings.NativeBindingGenerator`

<!-- api: T:Inno.Build.Bindings.NativeBindingGenerator ed761625983dcc7ca08f22a1852393bcfb68053a389526618ce30b3118fc7c10 -->
<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.#ctor(System.String) 065140b7b23dc421801fad061a92ba8193d54e85537cde0a1a1f89321a74ad6e -->
<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.GenerateAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBindingGenerationRequest,System.Threading.CancellationToken) 82521de32f3f4f1a83373536efc6b9f158d28e80579bc7f9ca648e87d6d0d70a -->

<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.ComputeInputFingerprintAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBindingGenerationRequest,System.Threading.CancellationToken) 725170f81c1190e69bebe9aac0632962a242124c9621e27f115c3791dfce006d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bdcbd846b59dd852"></a>[`public Inno.Build.Bindings.NativeBindingGenerator.NativeBindingGenerator(string dotnetHost)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L25) | Captures the managed tool executable used only when a declared generator extension must build.<br>`dotnetHost`：The explicit host executable or command selected by composition. | — | `System.ArgumentException`：The executable is empty. |
| <a id="api-bdc543b7cd9f7e86"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeBindingGenerationResult> Inno.Build.Bindings.NativeBindingGenerator.GenerateAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBindingGenerationRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L32) | Generates or verifies the requested closure under publication ownership.<br>返回：An owned complete immutable map indexed by Native owner project, never a partial closure. Dispose the result only after all generated-source and native-bridge consumers finish.<br>`context`：The borrowed operation owning frozen inputs, tool selection and statistics. Configuration path variables must resolve from its selected toolchain without changing process state.<br>`request`：The immutable target, component closure and publication policy.<br>`cancellationToken`：Cancels preparation; active work must drain before returning. | — | `System.InvalidOperationException`：Inputs change during preparation, a required frozen SDK value is absent, or required generator capabilities are unavailable.<br>`System.IO.InvalidDataException`：A definition or generated output fails its declared contract.<br>`System.OperationCanceledException`：The request was canceled and its active work has completed cleanup. |
| <a id="api-f9f1859591826b8a"></a>[`public sealed class Inno.Build.Bindings.NativeBindingGenerator : Inno.Build.Toolchains.INativeBindingGenerator`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L12) | Executes the shared facade-to-C-to-managed binding workflow independently of MSBuild. | — | — |
| <a id="api-b68756500cefb4ae"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.Bindings.NativeBindingGenerator.ComputeInputFingerprintAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBindingGenerationRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.Inputs.cs#L16) | 按同一 binding definition 选择当前配置，用内容哈希覆盖继承链、header、真实生成实现和 extension 源码/资源/依赖。 | 返回 source preparation identity；不接管 context，不调用实际生成。 | 要求本地 owned 配置；解析、依赖或 SDK 不可用时失败；观察 cancellation。 |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- `BGCS`：实现依赖，PrivateAssets="compile"。
- `BGCS.Cpp2C`：实现依赖，PrivateAssets="compile"。
