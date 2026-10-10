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

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_ACCEPTANCE.md)。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired. |
| [`Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L10) | Connects the Browser surface ABI to BGFX without exposing native BGFX types. |
| [`bool Inno.Integration.Browser.Bgfx.Runtime.BrowserBgfxSurfaceIntegration.supportsAdditionalSurfaces`](../../../platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/BrowserBgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
