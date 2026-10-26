# Inno.Integration.BrowserWasm32.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeComponents 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Integration.BrowserWasm32.Bgfx;

static IGameContentCompiler CreateCompiler(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
) {
    return BrowserWasm32BgfxIntegration.CreateContentCompilerFactory(new ToolRunner())(assets, serialization, types);
}
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。Browser nativeOptions 显式 Static；WebGL 2 defines、SDK 宏映射与 force include 位于本集成，真实配置 bytes 参与 Native 指纹。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration`

<!-- api: T:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration daf32ea98dd5442307fae3654735e7de7e6f51bdbe37b817b090033289c7d78e -->
<!-- api: P:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.nativeOptions bb7d47cb835a34bb93eda8b82228c50d47a04a8274aa6b8c6adc8bcdb3cae2dc -->
<!-- api: P:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.shaderProfile eda3d14262f37da64038a8744f494713be8b0475c5df08c0b37c5ce91d0c1c53 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) 4e297eaf169e62d2879298bd5a6e6d082e4d74fd8efed35fa49ff8621b2ab2ca -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4440a0d9cb1e2784"></a>[`public static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner tools)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxIntegration.cs#L42) | Creates the scoped content factory using the caller's protected offline tool deployment.<br>返回：A factory borrowing current authoring services without owning them or the selected tools.<br>`tools`：The borrowed executables and shader includes, retained through every compiler created by this factory. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：The selected tools are null. |
| <a id="api-2be3237ba41fff96"></a>[`public static Inno.Build.Toolchains.NativeComponentBuildOptions Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.nativeOptions { get; }`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxIntegration.cs#L18) | Gets WebGL 2 configuration and Emscripten header mappings for the shared static recipe. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-24a59c08cc1bff65"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration.shaderProfile { get; }`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxIntegration.cs#L28) | Gets the explicit shader configuration used by this integration. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6430c3d1526ed658"></a>[`public static class Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxIntegration`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxIntegration.cs#L13) | Connects platform publication facts to BGFX compilation without owning a product component list. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxShaderProfiles`

<!-- api: T:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxShaderProfiles 54aff2f70b5dd53dd185178e7d21e8bdd2712f27916e505d9a5a5a3dd91640ef -->
<!-- api: P:Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxShaderProfiles.target d61ab5f359e5001d1a193ea89e7165bf6762e1ffe6018f5dfb43418224f6e452 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b7fc1a873075a25f"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxShaderProfiles.target { get; }`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-0b5ad18bfc194db8"></a>[`public static class Inno.Integration.BrowserWasm32.Bgfx.BrowserWasm32BgfxShaderProfiles`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxShaderProfiles.cs#L11) | Owns explicit Browser shader dialects and offline product capabilities. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Build.BrowserWasm32](Inno.Build.BrowserWasm32.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
