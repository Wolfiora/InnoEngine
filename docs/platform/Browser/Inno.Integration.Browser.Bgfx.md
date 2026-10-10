# Inno.Integration.Browser.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeBuildPlans 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Integration.Browser.Bgfx;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Toolchains.Bgfx;

GameContentCompilerFactory compiler = BrowserBgfxIntegration.contentCompilerFactory;
IGameContentCompiler scoped = compiler(assets, serialization, types);
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。Browser nativeOptions 显式 Static；WebGL 2 defines、SDK 宏映射与 force include 位于本集成，真实配置 bytes 参与 Native 指纹。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[本轮验收](../../architecture/BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L12) | Connects platform publication facts to BGFX compilation without owning a product component list. |
| [`static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.contentCompilerFactory`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L32) | Gets the scoped factory for the currently supported Player graphics API closure. |
| [`static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.shaderProfile`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L27) | Gets the explicit shader configuration used by this integration. |
| [`static Inno.Build.Toolchains.NativeComponentBuildOptions Inno.Integration.Browser.Bgfx.BrowserBgfxIntegration.nativeOptions`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs#L17) | Gets WebGL 2 configuration and Emscripten header mappings for the shared static recipe. |

### `Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxShaderProfiles.cs#L11) | Owns explicit Browser shader dialects and offline product capabilities. |
| [`static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.Browser.Bgfx.BrowserBgfxShaderProfiles.target`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. |

## 项目依赖

- [Inno.Build.Browser](Inno.Build.Browser.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
