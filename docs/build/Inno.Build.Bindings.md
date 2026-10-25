# Inno.Build.Bindings

[Build 索引](README.md) · [Wiki 首页](../README.md) · [中立工具链](Inno.Build.Toolchains.md)

## 生成结果的读取生命周期

平台 Profile 可用官方 `BindingImportMode` 明确选择导入机制；不指定时保留组件配置。此选择与 runtime、SDK 和有效配置共同进入生成指纹。平台负责选择，组件公共配置不维护平台表。

实际生成的 descriptor 用 `usesRuntimeImports` 表达初始化责任：runtime import 调用官方生成的 `Create`，provider 模式根据实际链接选择 `FromStatic` 或 `Load`。选择文件同时冻结该事实和链接方式；托管编译不能自行从平台名或文件后缀推断。Native 部署继续严格验证与 managed 编译相同的选择。

`NativeBindingGenerator.GenerateAsync` 返回 `NativeBindingGenerationResult`。结果按 Native 项目路径索引完整生成闭包，并拥有每个结果的读取保护。描述符只是路径与身份，不会延长缓存的存活期；调用方必须在编译、链接或复制结束后释放结果。

目标生成缓存允许多个 reader 同时消费。修复损坏产物和回收目录必须先取得 `BuildArtifactWriteLease`，等待所有 reader 释放。取消一个等待者不会释放其他调用者持有的保护。

Host source 是可变的工作区输出，因此生成结果继续持有该组件的 generation owner，直到消费结束。目标产物使用独立读取 lease，不在整个 Native 编译期间阻塞其他有效读取者。

MSBuild 在读取保护期间把唯一的 `Bindings.cs` 复制到本次 SDK 构建的 `obj/.../bindings/Bindings.cs`。编译器随后只消费该副本；相同字节不重复写入，不更新时间。目标、指纹或完整文件集合不匹配时直接失败。

输入指纹对配置及官方分析证据中的 JSON 对象键使用 ordinal 稳定顺序，避免不可变字典的进程随机枚举顺序改变身份；数组仍保留原顺序，定义、参数、搜索优先级和全部实际输入继续参与验证。此规范化只用于引擎缓存身份，不重写 BGCS 配置、生成结果或 SDK 内部缓存。

引擎绑定实现使用构建时生成的内容身份，包含明确源码闭包、实际 Roslyn 编译器、目标框架、语言、条件符号及相关编译选项。独立 `ArtifactsPath` 引起的引擎 DLL/PDB 布局差异不构成生成输入变化；SDK、扩展、worker 资源和配置来源仍按实际内容核验。配置相同不代表可以跳过锁后及发布前的 fresh 输入验证。

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

共享绑定生成应用库是唯一 BGCS 消费边界。`NativeBindingGenerator` 实现中立 `INativeBindingGenerator`，直接调用 BGCS 公开库；不引用 MSBuild Task。Toolchains、平台模块、Support Pack 只借用中立接口。平台 integration 创建 Profile 和实现，标准发行只注册完整贡献，CLI、Editor 构建和薄 Task 传递同一服务。此程序集不能进入 Player 运行闭包。

## 配置与执行

每个组件的 `Bindings/bindings.props` 是唯一生成入口，声明 `InnoNativeComponentId` 和公共 `InnoBindingConfig`。目标 ABI 与 managed runtime 来自平台 integration 的 `NativeBindingProfile`。公共 JSON 只保存组件语义；SDK provider 冻结 compiler、sysroot、系统头、宏和参数，接入层映射到 BGCS 官方配置。条件、表达式、执行代码、配置路径逃逸和不匹配目标均拒绝。

生成身份使用本次选择的配置、目标、输出模式、扩展和实际输入。修改无关平台文件不会使当前生成失效；修改当前配置、facade、include、SDK、工具或生成实现仍会失效。每个验证阶段重新解析定义，不能借此跳过当前选择变化的检查。

1. 冻结组件、目标、HostSource/TargetArtifacts、CheckOnly 与预期指纹。
2. 使用 SDK 公开 HostServices 路径服务，通过 GenerationRequest.MaterializeAsync 冻结选中配置、继承链和插件输入，取得全部 generation lease（规范路径排序）。
3. 在 fresh binding-locks 阶段校验完整输入；批次内物理读取去重。
4. 通过 BindingHost 执行正式生成流程：C++ facade 经 SDK 的 Cpp2C 和 managed 生成链输出，纯 C 从 header 开始。消费经过 BindingArtifactManifest 验证的实际产物，完整目标缓存可复用。
5. fresh binding-generation 校验，取消检查，再发布全部完整结果；失败不返回部分闭包。

BGCS 路径使用正式配置中的明确输入；目标 SDK 的 compiler、sysroot 和系统 include 通过本次 `NativeToolchainSelection` 转换为 SDK 的 typed target settings，不提供自定义环境变量替换语法。直接库调用不修改进程环境。
CLI 和 MSBuild 的 TargetArtifacts 请求先通过同一发行解析明确目标的工具链；HostSource 不隐式选择发布目标。
独立绑定定义不必注册游戏发布目标；MSBuild 仅取得发行明确贡献的 SDK。
直接库调用可以提供完整的显式 Profile；MSBuild 使用发行注册的目标，缺少目标时失败。

BGCS 包族固定为正式 4.1.2。接入使用 `BindingGenerator.CreateBuilder()`，通过公开 frontend factory 替换点将 `WorkerAssetLocator` 的 applicationBase 固定到已加载 SDK 的部署目录。标准 SDK 定位器负责验证和准备 worker；私有 MSBuild Task 不借用 MSBuild 宿主的 application base，不自行解包或复制 SDK 内部实现。

Profile 使用开放的 `ManagedRuntimeId`。平台可通过 `configureGenerator` 注册官方扩展；Browser integration 显式注册 `BrowserWasmPlugin`，仅安装包不会激活能力。注册 action 只描述不可变组合，不捕获操作状态；其实现程序集及引用的 BGCS 扩展参与缓存身份。共享构建执行器和组件配置不识别 Browser 名称。

公共配置使用 BGCS 当前严格格式。SDK 格式标记只属于外部生成协议，不增加引擎持久化格式的兼容读取。生成结果由 SDK 的 verified artifact/native-source manifest 读取，逻辑输入根由引擎映射到冻结复制树；编译、链接、部署、缓存 owner 与 lease 仍由引擎负责。

## 输出与所有权

HostSource 使用组件的 `Generated/Bindings.cs` 及 `Native/Generated`。TargetArtifacts 使用 `artifacts/bindings/<component>/<target>/<fingerprint>/Generated`、`Native` 与完整性 manifest。候选由批次 owner 清理；已完整发布的独立缓存可留存。生成锁位于组件 `obj/bindings/generation.lock`，锁文件保留。SDK 自身缓存位于 `artifacts/build-tools/bindings/sdk-workspaces/<component>/<target>`，不进入创作源码目录。

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
    NativeToolchainSelection selectedTools,
    NativeBindingProfile profile,
    IReadOnlyList<NativeComponentDescriptor> components,
    CancellationToken cancellationToken
) {
    NativeBuildContext operation = new NativeBuildContext(engineRoot, "release")
        .WithToolchain(selectedTools)
        .WithBindingGenerator(new NativeBindingGenerator(profile));
    using NativeBindingGenerationResult generations =
        await NativeBindingPreparation.PrepareAsync(
            operation, components, cancellationToken);
    foreach (NativeBindingGenerationDescriptor generation in generations.Values)
        System.Console.WriteLine(generation.bindingsPath);
}
```

示例中的 engineRoot、selectedTools、components 由产品组合预检并冻结。内部 reader、identity、batch、publication 不作为稳定 API。

## 验证入口

`BindingDefinitionTests`、`BindingGenerationBatchTests`、`NativeBindingGenerationTests`、`NativeBindingCompilationTests` 覆盖定义、bridge、扩展/消费、冻结 SDK 路径、CheckOnly、失败、取消、篡改和输出保护。生成业务 fixture 直接测试公开生成库；MSBuild 入口另由实际项目构建验证。实际结果见当前验收报告。

## 生成工具宿主资产

生成工具的 RuntimeIdentifier 使用所选 SDK 的 NETCoreSdkRuntimeIdentifier，独立于游戏发布 target。NuGet Restore 选择实际宿主 Native 包资产；不以文件名列表删掉其他平台 DLL。BGCS worker manifests 与 worker assets 使用正式包的部署布局；私有 Task 宿主也必须携带完整执行闭包。Runtime 与生成器继续分开，Player 不携带 BGCS 生成器。

当前包消费、worker 私有宿主及可重定位原生输入的验收状态见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。SDK 插件由正式配置和公开宿主装载，不恢复独立的引擎扩展工程协议。

## 当前源码公开 API 清单

当前签名由正式源码语义清单核对；生命周期与错误约束见前文及对应 XML。

<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.#ctor(Inno.Build.Bindings.NativeBindingProfile) 78c941b2cdfe38aec973d075c405ed38d0a3a2c0ee03cef57d655facbb278de3 -->
<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.ComputeInputFingerprintAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBindingGenerationRequest,System.Threading.CancellationToken) 725170f81c1190e69bebe9aac0632962a242124c9621e27f115c3791dfce006d -->
<!-- api: M:Inno.Build.Bindings.NativeBindingGenerator.GenerateAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBindingGenerationRequest,System.Threading.CancellationToken) 82521de32f3f4f1a83373536efc6b9f158d28e80579bc7f9ca648e87d6d0d70a -->
<!-- api: M:Inno.Build.Bindings.NativeBindingProfile.#ctor(System.String,BGCS.Configuration.TargetConfiguration,System.Nullable{BGCS.Core.Targets.ManagedRuntimeId},System.Collections.Generic.IEnumerable{System.String},System.Action{BGCS.BindingHostBuilder},System.Nullable{BGCS.Core.BindingImportMode}) e580738ea557569e211163ff809f58544b6d141a3cebae412e7886e2859d3181 -->
<!-- api: P:Inno.Build.Bindings.NativeBindingProfile.inputPaths 6bece7ed193b007f003560c741c94a0992ecd8d47749a870ddd1a4a6ee1198c6 -->
<!-- api: P:Inno.Build.Bindings.NativeBindingProfile.runtime 8e1a0f3862da7a5708b37617860bb0f79deb5faa2230c3cc5cfe8f486e5ecaf7 -->
<!-- api: P:Inno.Build.Bindings.NativeBindingProfile.target 3a3a6f06a994dbe8ddd2613744f9d636af4d7b1c899d9b1a252cf2ccd333d5df -->
<!-- api: P:Inno.Build.Bindings.NativeBindingProfile.targetId 7284e5f7207883962134f0b64c72c0d36726cc738a7fc0fb5f0f02130a01db74 -->
<!-- api: T:Inno.Build.Bindings.NativeBindingGenerator ed761625983dcc7ca08f22a1852393bcfb68053a389526618ce30b3118fc7c10 -->
<!-- api: T:Inno.Build.Bindings.NativeBindingProfile 54fcaf54f2c720efad4f8e258567666026881bc8213fa9fbc111f3d4ef1800af -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-7d1ced719f2cb6f9"></a>[`public Inno.Build.Bindings.NativeBindingGenerator.NativeBindingGenerator(Inno.Build.Bindings.NativeBindingProfile profile)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L22) | Creates a generator that obtains target tools and input ownership from each build operation.<br>`profile`：The explicitly registered target profile. |
| <a id="api-b68756500cefb4ae"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.Bindings.NativeBindingGenerator.ComputeInputFingerprintAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBindingGenerationRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.Inputs.cs#L16) | Computes the selected generator source input identity without producing bindings or compiling a product.<br>`context`：The borrowed operation with frozen target tools and phase-local input reads.<br>`request`：The exact component, configuration target and output mode whose source closure is planned.<br>`cancellationToken`：Cancels configuration loading, dependency evaluation and content hashing.<br>返回：The complete source preparation identity, distinct from any generated artifact fingerprint. It remains computable when reconstructible generation outputs have been retired.<br>T:System.InvalidOperationException：The selected tools, configuration, generator or extension inputs cannot be resolved.<br>T:System.OperationCanceledException：Input planning was canceled.<br>T:System.NotSupportedException：Configuration inheritance uses a remote input without local build ownership. |
| <a id="api-bdc543b7cd9f7e86"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeBindingGenerationResult> Inno.Build.Bindings.NativeBindingGenerator.GenerateAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBindingGenerationRequest request, System.Threading.CancellationToken cancellationToken)`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L28) | Generates or verifies the requested closure under publication ownership.<br>`context`：The borrowed operation owning frozen inputs, tool selection and statistics. Configuration path variables must resolve from its selected toolchain without changing process state.<br>`request`：The immutable target, component closure and publication policy.<br>`cancellationToken`：Cancels preparation; active work must drain before returning.<br>返回：An owned complete immutable map indexed by Native owner project, never a partial closure. Dispose the result only after all generated-source and native-bridge consumers finish.<br>T:System.InvalidOperationException：Inputs change during preparation, a required frozen SDK value is absent, or required generator capabilities are unavailable.<br>T:System.IO.InvalidDataException：A definition or generated output fails its declared contract.<br>T:System.OperationCanceledException：The request was canceled and its active work has completed cleanup. |
| <a id="api-36cfa46071120b8b"></a>[`public Inno.Build.Bindings.NativeBindingProfile.NativeBindingProfile(string targetId, BGCS.Configuration.TargetConfiguration target, BGCS.Core.Targets.ManagedRuntimeId? runtime = null, System.Collections.Generic.IEnumerable<string>? inputPaths = null, System.Action<BGCS.BindingHostBuilder>? configureGenerator = null, BGCS.Core.BindingImportMode? importMode = null)`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L46) | Creates a target profile without discovering an SDK or executing a generator. <br>`targetId`：The engine target identity that may consume this profile.<br>`target`：The official BGCS native target selection.<br>`runtime`：The official BGCS managed runtime selection.<br>`inputPaths`：Checkout-relative implementation inputs owned by this profile.<br>`configureGenerator`：An immutable platform-owned registration action for official BGCS extensions, or null for the standard SDK. The action must not capture mutable state; its implementation and extension assemblies participate in input identity.<br>`importMode`：The platform's official import mechanism, or null to preserve the component choice. |
| <a id="api-f29bc469b5f12a92"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Bindings.NativeBindingProfile.inputPaths { get; }`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L89) | Gets the profile's exact checkout implementation inputs. |
| <a id="api-dc275bfe6affcbdb"></a>[`public BGCS.Core.Targets.ManagedRuntimeId? Inno.Build.Bindings.NativeBindingProfile.runtime { get; }`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L81) | Gets the managed projection runtime, or null for the component's portable default. |
| <a id="api-425e0e5b6913867f"></a>[`public BGCS.Configuration.TargetConfiguration Inno.Build.Bindings.NativeBindingProfile.target { get; }`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L77) | Gets the immutable official BGCS target configuration. |
| <a id="api-8a722321463b942a"></a>[`public string Inno.Build.Bindings.NativeBindingProfile.targetId { get; }`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L73) | Gets the engine target authorized to consume this profile. |
| <a id="api-f9f1859591826b8a"></a>[`public sealed class Inno.Build.Bindings.NativeBindingGenerator : Inno.Build.Toolchains.INativeBindingGenerator`](../../build/bindings/Inno.Build.Bindings/NativeBindingGenerator.cs#L12) | Executes the shared facade-to-C-to-managed binding workflow independently of MSBuild.<br>Provides preparation input planning for the shared binding generator without producing generated artifacts. |
| <a id="api-503652649b30b0db"></a>[`public sealed class Inno.Build.Bindings.NativeBindingProfile`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L17) | Captures one platform's official BGCS target and managed projection choices. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- `BGCS`：构建期 Profile 的官方配置类型依赖；中立 Toolchains 与游戏运行契约不暴露这些类型。

<!-- api: P:Inno.Build.Bindings.NativeBindingProfile.importMode fcb3aacdab5b905ec28bb2bff198d8cd94404059b7c65db9a900de11ca65db93 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-f0766fe50b74a3de"></a>[`public BGCS.Core.BindingImportMode? Inno.Build.Bindings.NativeBindingProfile.importMode { get; }`](../../build/bindings/Inno.Build.Bindings/Configuration/NativeBindingProfile.cs#L85) | Gets the explicit platform import mechanism, or null for the component's mechanism. |
