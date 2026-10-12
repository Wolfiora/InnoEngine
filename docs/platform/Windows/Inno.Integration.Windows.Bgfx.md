# Inno.Integration.Windows.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeBuildPlans 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Integration.Windows.Bgfx;

static IGameContentCompiler CreateCompiler(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
) {
    return WindowsBgfxIntegration.contentCompilerFactory(assets, serialization, types);
}
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration`

<!-- api: T:Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration 53fea4b092b19d26ac6772a763f74c886d7bf6d03cbd0f63cd05a027bc6bae30 -->
<!-- api: P:Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.nativeProfile b010e39831b308f7e7423a7a2cf9ffc5a5e3fc7f95d56497e4522591de67580c -->
<!-- api: P:Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.shaderProfile 8e74aac6109c9a7842acca875c434f459b4f8d64de249431efecb0eb4cfce72e -->
<!-- api: P:Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.contentCompilerFactory f9cca7de66ce57c1f4b29d905d5ec4b6577f23ccb3039224522cfcaa8e73894c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e9282416e50200e6"></a>[`public static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.contentCompilerFactory { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxIntegration.cs#L26) | Gets the scoped factory for the currently supported Player graphics API closure. | — | — |
| <a id="api-703272924e0818e4"></a>[`public static Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.nativeProfile { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxIntegration.cs#L16) | Gets the immutable SDK invocation for this platform and backend pairing. | — | — |
| <a id="api-92b08f67b52ae4cf"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration.shaderProfile { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxIntegration.cs#L21) | Gets the explicit shader configuration used by this integration. | — | — |
| <a id="api-e555ac9f6a48387d"></a>[`public static class Inno.Integration.Windows.Bgfx.WindowsBgfxIntegration`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxIntegration.cs#L11) | Connects platform publication facts to BGFX compilation without owning a product component list. | — | — |

### `Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles`

<!-- api: T:Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles 62d97ae9be93402b2595728c74c225fe9ab2788bd0dd7152884a93805ee9a351 -->
<!-- api: P:Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles.target b3ad4aff1521fce0708726f43411fc99f9c73e9b518799f0aba03874eb19d48e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-701b7690dfb6e6cb"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles.target { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. | — | — |
| <a id="api-5d8cba75ab4af90e"></a>[`public static class Inno.Integration.Windows.Bgfx.WindowsBgfxShaderProfiles`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx/WindowsBgfxShaderProfiles.cs#L11) | Owns explicit Windows shader dialects and offline product capabilities. | — | — |

## 项目依赖

- [Inno.Build.Windows](Inno.Build.Windows.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
