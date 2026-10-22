# Inno.Integration.WindowsX64.Bgfx

Native GENie/MSBuild 执行选择只保留实际使用的编译器、生成器与 SDK 输入，不把未执行的 .NET host 放入 Shader 工具身份。Windows SDK 以独立系统 PATH 初始化，外层 Browser 的 CMake/Ninja PATH 不改变它的编译身份；实际 SDK、环境与有序参数仍冻结并校验。

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeComponents 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Integration.WindowsX64.Bgfx;

static IGameContentCompiler CreateCompiler(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
) {
    return WindowsX64BgfxIntegration.CreateContentCompilerFactory(new ToolRunner())(assets, serialization, types);
}
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration`

<!-- api: T:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration 0c6d5cf56b8d9f66c12e8ddabca8f9ca18d0130a767ab69ce54ea6b5cd8211b0 -->
<!-- api: P:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.nativeProfile a8a714e7102f091a3f7288be96639e479b7327142118082f17c7d1bb7cf41a17 -->
<!-- api: P:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.shaderProfile 6bda475928e60bd085f197795f596511d99e6486c3dcd58ff811f822f92140c1 -->
<!-- api: M:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) b2b4aaec4fc8b0db44821344400817cd0ee1b7857ab3502484f349d490b467bd -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-a501c4524913f10d"></a>[`public static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner tools)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxIntegration.cs#L36) | Creates the scoped content factory using the caller's protected offline tool deployment.<br>`tools`：The borrowed executables and shader includes, retained through every compiler created by this factory.<br>返回：A factory borrowing current authoring services without owning them or the selected tools.<br>T:System.ArgumentNullException：The selected tools are null. |
| <a id="api-91f68cd1d422ca22"></a>[`public static Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.nativeProfile { get; }`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxIntegration.cs#L17) | Gets the immutable SDK invocation for this platform and backend pairing. |
| <a id="api-2715254c1343aa91"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.shaderProfile { get; }`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxIntegration.cs#L22) | Gets the explicit shader configuration used by this integration. |
| <a id="api-dc04e103f2194d15"></a>[`public static class Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxIntegration.cs#L12) | Connects platform publication facts to BGFX compilation without owning a product component list. |

### `Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxShaderProfiles`

<!-- api: T:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxShaderProfiles f024d0a7613105c41f8cc65da5c83b906e582fa7727df01bed5b809326f597b8 -->
<!-- api: P:Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxShaderProfiles.target c7fa46944c0fa6f5b49810d7f8bd66e26266b7443bfc2570e663958b6e0cd15c -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-7bfe14724a9e4969"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxShaderProfiles.target { get; }`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. |
| <a id="api-b0b0973a36137cfd"></a>[`public static class Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxShaderProfiles`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx/WindowsX64BgfxShaderProfiles.cs#L11) | Owns explicit Windows shader dialects and offline product capabilities. |

## 项目依赖

- [Inno.Build.WindowsX64](Inno.Build.WindowsX64.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
