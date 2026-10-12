# Inno.Integration.Browser.Bgfx.Runtime

[平台分类](README.md) · [平台索引](../README.md) · [Wiki 首页](../../README.md) · [BGFX backend](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

## 职责与边界

`BrowserBgfxSurfaceIntegration` 只负责 Canvas 系统 surface 与 BGFX runtime SPI 的连接。依赖 BGFX Adapter 与中立 Platform SPI；不引用 Native.Bgfx、BGFX build integration、BGCS、Task 或其他平台。对象无资源所有权，产品与 Device 借用不可变配置。

## 行为与生命周期

Resolve 验证本平台 ABI、角色和非零句柄，返回不可变借用描述；成功不接管原窗口/显示句柄。supportsAdditionalSurfaces 为 `false`，主输出 sRGB reset 许可为 `false`。BGFX Device 在 Native 初始化前验证，并在 surface 创建时解析一次。附加窗口还必须满足实际 BGFX SwapChain 能力。

窗口由平台应用或明确外部 owner 管理，Device 只借用。关闭先 RetireWindowSurface，等待渲染命令处理完成，再由窗口 owner 销毁；Task 完成不是通用 GPU fence。无效 ABI/角色/句柄明确失败，不创建新进程 owner。

## 产品组合

```csharp
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.Browser.Bgfx.Runtime;

var rendering = new BgfxRenderingBackendProvider(new BrowserBgfxSurfaceIntegration());
```

将 rendering 赋给 DefaultAdapterCatalogOptions.rendering。Editor/Player 共用实现，构建 integration 独立选择。macOS 实机结果与源码检查分别记录；Browser 不宣称附加窗口能力。

## 验证入口

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration`

<!-- api: T:Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration cbbd955b19b3f880ea7510cb3de93afe5cdfe6a1325e496a8eade71385503bbe -->
<!-- api: P:Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.supportsAdditionalSurfaces 2b17c3bc09c094cd53067594e5ed7755650985d749c086e5a7235be03d934b44 -->
<!-- api: M:Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) 69a14be3e457319732453169509ce2e678344eb77a226f49946336f6975dddea -->
<!-- api: M:Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.#ctor 7da88a2257fef2fa79879bfb59afbdec8a0f31b66f04322d1a6daec6129c718a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b370fc9b3aa2e8c7"></a>[`public Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.BrowserBgfxSurfaceIntegration()`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7ad4d13770d6a191"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired.<br>返回：The validated handle binding and reset capability, without transferring window ownership.<br>`handles`：The platform window owner's immutable borrowed handles.<br>`role`：The requested primary or additional presentation role. | — | `System.NotSupportedException`：The surface ABI or requested presentation role is not supported by this integration.<br>`System.ArgumentException`：The requested role or native window handle is invalid. |
| <a id="api-50d2e36029f8a377"></a>[`public bool Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. | — | — |
| <a id="api-2aba3054d9219bf5"></a>[`public sealed class Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration : Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L10) | Connects the Browser surface ABI to BGFX without exposing native BGFX types. | — | — |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
