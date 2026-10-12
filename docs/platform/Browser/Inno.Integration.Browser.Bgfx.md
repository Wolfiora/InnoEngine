# Inno.Integration.Browser.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeBuildPlans 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Integration.Browser.Bgfx;

static IGameContentCompiler CreateCompiler(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
) {
    return BrowserBgfxIntegration.contentCompilerFactory(assets, serialization, types);
}
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。Browser nativeOptions 显式 Static；WebGL 2 defines、SDK 宏映射与 force include 位于本集成，真实配置 bytes 参与 Native 指纹。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration`

<!-- api: T:Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration 7834d46296cc42c6f3e5db40ac04b7e691fbb73519d40d5abe2540ecb5025065 -->
<!-- api: P:Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.nativeOptions 0c8f0282eaa9f425010799b2370cc775b7dddc1d3188e72b06ea4670629c77de -->
<!-- api: P:Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.shaderProfile 9c3fdaad711bfe31f372b0433e84186b2796bbc37bd7f45b3aca78bf45fe4be8 -->
<!-- api: P:Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.contentCompilerFactory 711662732701e1b3066f385cb23c719a165f88fe33b437007e50c94979c85270 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1904281b4957bc71"></a>[`public static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.contentCompilerFactory { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L32) | Gets the scoped factory for the currently supported Player graphics API closure. | — | — |
| <a id="api-18c8c476a718f756"></a>[`public static Inno.Build.Toolchains.NativeComponentBuildOptions Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.nativeOptions { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L17) | Gets WebGL 2 configuration and Emscripten header mappings for the shared static recipe. | — | — |
| <a id="api-b7f734cb656511fe"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.shaderProfile { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L27) | Gets the explicit shader configuration used by this integration. | — | — |
| <a id="api-2e329f836d034609"></a>[`public static class Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L12) | Connects platform publication facts to BGFX compilation without owning a product component list. | — | — |

### `Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles`

<!-- api: T:Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles ecea079c82ff7965eae6423b70706fe31f88c7f6777cb055ee9a078ea2e5a6f1 -->
<!-- api: P:Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles.target 1ab31f0e288a0197f0b67931fbbd3aa1f6cfe282c2b357c50ae643893acf6a5d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-be4cabb86302c1b1"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles.target { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. | — | — |
| <a id="api-b4288f1b70e75793"></a>[`public static class Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxShaderProfiles.cs#L11) | Owns explicit Browser shader dialects and offline product capabilities. | — | — |

## 项目依赖

- [Inno.Build.Browser](Inno.Build.Browser.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
