# Inno.Build.Toolchains.Bgfx

[分类索引](README.md) · [BGFX Native](Inno.Native.Bgfx.md) · [Wiki 首页](../../README.md)

这是 BGFX native library 与工具的构建库，按当前 host/target 与 debug/release 配置生成 `artifacts/native/<component>/<target>/<fingerprint>` 产物，供 Editor 开发运行和 Support Pack 生产使用。原生库与 shaderc/texturec 等工具只收集对应配置的编译产物；Windows Release Player Support Pack 的构建期 shader 编译需要生成 Release 工具。

它允许引用 BGFX Native/Tools 与公共 Toolchains，但不得被 Runtime、Editor feature 或 Player 引用。

## 统一构建入口

本项目现在是库，没有 Program。全部公开构建入口为 `BgfxNativeBuild.BuildAsync(NativeBuildContext, BgfxNativeBuildProfile, CancellationToken = default)` 和 `BgfxToolsBuild.BuildAsync(NativeBuildContext, BgfxNativeBuildProfile, CancellationToken = default)`；前者构建图形 Native，后者独立构建宿主 shaderc/texturec 等离线工具。两者返回 `NativeBuildProduct`。两者验证 debug/release 配置并传播构建失败。正常工作流使用 Inno.Build.Cli engine，不启动单独组件 CLI。

两者均使用显式 checkout/configuration；所有 GENie、make 和 MSBuild 子进程接受取消并等待进程树退出。两者在各自拥有的 `obj/native/<target>/<fingerprint>/Sources` 中复制当前 bgfx、bx、bimg 源码并运行上游 GENie；`.build` 只出现在这个私有快照中。源码 checkout 不接收编译产物。工具路径在运行前解析并计入指纹。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

// Resolve explicit target tools and borrow the platform-contributed profile first.
NativeBuildContext context = selectedContext;
BgfxNativeBuildProfile platformProfile = selectedProfile;
await BgfxNativeBuild.BuildAsync(context, platformProfile, cancellationToken);
await BgfxToolsBuild.BuildAsync(context, platformProfile, cancellationToken);
```

## 平台配置与所有权

`BgfxNativeBuildProfile` 仅贡献目标 ID、GENie 参数、产物路径 token 和 `CreateBuildInvocation`。
`BgfxBuildInvocation` 冻结工具名称及有序参数，不执行进程。Windows/MSBuild、macOS/Make、Linux/Make
的具体配置由所属平台维护。共享 recipe 在 staging 前取得一次 invocation，并把真实参数、工具和布局纳入指纹。
NativeBuildContext target 与 profile 不一致明确失败，不猜测当前 OS 的产品目标。

共享 owner 负责源快照、组件选项、取消、输出验证和原子发布。profile 不持有设备、回调、进程或临时资源。
Browser 静态构建复用组件 CMake recipe，由其平台 aggregate 执行，不使用 GENie profile。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/Inno.Build.Toolchains.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L10) | Freezes an SDK tool name and ordered arguments for execution from the component source root. |
| [`Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.BgfxBuildInvocation(string tool, System.Collections.Generic.IEnumerable<string> arguments)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L27) | Snapshots the invocation so later caller mutations cannot change its fingerprint or execution. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.arguments`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L48) | Gets immutable, ordered compiler arguments included in the recipe identity. |
| [`string Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.tool`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L43) | Gets the declared tool name resolved by the operation's frozen toolchain. |

### `Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L10) | Publishes pinned graphics runtime libraries from an isolated source snapshot. |
| [`static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.componentDescriptor`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L15) | Gets the unique native and recipe owners used for sources and target-scoped intermediates. |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L50) | Builds and installs graphics artifacts using explicit platform-owned SDK configuration. |

### `Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L8) | Supplies platform-owned project generation and SDK invocation data to the shared BGFX recipe. |
| [`abstract Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.CreateBuildInvocation(Inno.Build.Toolchains.NativeBuildContext context, bool includeTools)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L37) | Freezes the selected SDK invocation without executing tools or creating staging. |
| [`abstract System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.generatorArguments`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L18) | Gets ordered vendor project-generation arguments, excluding shared component options. |
| [`abstract string Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.artifactPathToken`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L23) | Gets the vendor output directory token used to select this target's artifacts. |
| [`abstract string Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.targetId`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L13) | Gets the exact native target accepted by this configuration. |

### `Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L10) | Publishes pinned graphics tools from an isolated source snapshot. |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L36) | Builds and installs graphics artifacts using explicit platform-owned SDK configuration. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
