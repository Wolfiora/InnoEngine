# Inno.Build.Toolchains.Bgfx

[Build 索引](README.md) · [BGFX Native](../native/Inno.Native.Bgfx.md)

这是 BGFX native library 与工具的构建库，按当前 host/target 与 debug/release 配置生成 `artifacts/native/<component>/<target>/<fingerprint>` 产物，供 Editor 开发运行和 Support Pack 生产使用。原生库与 shaderc/texturec 等工具只收集对应配置的编译产物；Windows Release Player Support Pack 的构建期 shader 编译需要生成 Release 工具。

它允许引用 BGFX Native/Tools 与公共 Toolchains，但不得被 Runtime、Editor feature 或 Player 引用。

## 统一构建入口

本项目现在是库，没有 Program。全部公开构建入口为 `BgfxNativeBuild.BuildAsync(NativeBuildContext, CancellationToken = default)` 和 `BgfxToolsBuild.BuildAsync(NativeBuildContext, CancellationToken = default)`；前者构建图形 Native，后者独立构建宿主 shaderc/texturec 等离线工具。两者返回 `NativeBuildProduct`。两者验证 debug/release 配置并传播构建失败。正常工作流使用 Inno.Build.Cli engine，不启动单独组件 CLI。

两者均使用显式 checkout/configuration；所有 GENie、make 和 MSBuild 子进程接受取消并等待进程树退出。两者在各自拥有的 `obj/native/<target>/<fingerprint>/Sources` 中复制当前 bgfx、bx、bimg 源码并运行上游 GENie；`.build` 只出现在这个私有快照中。源码 checkout 不接收编译产物。工具路径在运行前解析并计入指纹。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

var context = new NativeBuildContext(engineRoot, "release");
await BgfxNativeBuild.BuildAsync(context, cancellationToken);
await BgfxToolsBuild.BuildAsync(context, cancellationToken);
```

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L33) | Builds and installs the native graphics artifacts for the current host. |
| [`Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`](../../build/toolchains/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L10) | Publishes pinned graphics runtime libraries from an isolated source snapshot. |

### `Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L33) | Builds and installs the native graphics artifacts for the current host. |
| [`Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`](../../build/toolchains/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L10) | Publishes pinned graphics tools from an isolated source snapshot. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
