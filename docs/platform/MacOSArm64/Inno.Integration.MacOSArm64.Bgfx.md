# Inno.Integration.MacOSArm64.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

MacOS 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Native GENie/Make 执行选择只包含实际使用的 clang、clang++、Make、GENie 与 SDK 输入；未执行的 .NET host 和 CMake 不参与离线工具身份。绑定生成仍使用 operation 原先冻结的完整工具选择，两种职责分别校验。此处在 Windows 已完成源码与契约检查，macOS Native 执行需实机验证。

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeComponents 声明，integration 不重复注册发行。Player 产品不引用本构建期程序集；内容在发布时编译后通过中立 store 读取。

```csharp
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Integration.MacOSArm64.Bgfx;

static IGameContentCompiler CreateCompiler(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
) {
    return MacOSArm64BgfxIntegration.CreateContentCompilerFactory(new ToolRunner())(assets, serialization, types);
}
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration`

<!-- api: T:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration e475fee56fb65375d2bbb492d8f994ca0f36e8e41947c3a5712631bd17aecfb7 -->
<!-- api: P:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.nativeProfile 4f0135a7b0b7535a906ddad2872e8c633c39317a9030cf01b5026231ea737279 -->
<!-- api: P:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.shaderProfile 854026f5e16d4dbfb5d9493c4b0c7d2c9d7e69e4083628b86ed32ba23a869399 -->
<!-- api: M:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) 82e75c5778f15ce256287e6b2789d8de98794b25c000bb83f050b0c054167616 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-37ffa4109e010cba"></a>[`public static Inno.Build.Composition.GameContentCompilerFactory Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.CreateContentCompilerFactory(Inno.Build.Toolchains.Bgfx.Tools.ToolRunner tools)`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxIntegration.cs#L36) | Creates the scoped content factory using the caller's protected offline tool deployment.<br>`tools`：The borrowed executables and shader includes, retained through every compiler created by this factory.<br>返回：A factory borrowing current authoring services without owning them or the selected tools.<br>T:System.ArgumentNullException：The selected tools are null. |
| <a id="api-a22043b5d6d16904"></a>[`public static Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.nativeProfile { get; }`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxIntegration.cs#L17) | Gets the immutable SDK invocation for this platform and backend pairing. |
| <a id="api-0bae63c267655a92"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.shaderProfile { get; }`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxIntegration.cs#L22) | Gets the explicit shader configuration used by this integration. |
| <a id="api-a0e70a0c1bf3f22d"></a>[`public static class Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxIntegration.cs#L12) | Connects platform publication facts to BGFX compilation without owning a product component list. |

### `Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxShaderProfiles`

<!-- api: T:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxShaderProfiles a0eaf42d2aea94c3408fbca8f8bb5a92472c37cbb044166835d67e786ae7f8c3 -->
<!-- api: P:Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxShaderProfiles.target db68ac750e87b172915e1c13f3b820bee25d8c1e9da8c18a7f3c5c145f94bbce -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-49600e8ccb19db32"></a>[`public static Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxShaderProfiles.target { get; }`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxShaderProfiles.cs#L16) | Gets the immutable configuration shared by this platform's products and publication module. |
| <a id="api-11b5d520d66b1a8d"></a>[`public static class Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxShaderProfiles`](../../../platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Bgfx/MacOSArm64BgfxShaderProfiles.cs#L11) | Owns explicit MacOS shader dialects and offline product capabilities. |

## 项目依赖

- [Inno.Build.MacOSArm64](Inno.Build.MacOSArm64.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
