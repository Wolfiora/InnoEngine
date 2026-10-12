# Inno.Integration.Windows.Bgfx.Runtime

[平台分类](README.md) · [平台索引](../README.md) · [Wiki 首页](../../README.md) · [BGFX backend](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

## 职责与边界

`WindowsBgfxSurfaceIntegration` 只负责 Win32 系统 surface 与 BGFX runtime SPI 的连接。依赖 BGFX Adapter 与中立 Platform SPI；不引用 Native.Bgfx、BGFX build integration、BGCS、Task 或其他平台。对象无资源所有权，产品与 Device 借用不可变配置。

## 行为与生命周期

Resolve 验证本平台 ABI、角色和非零句柄，返回不可变借用描述；成功不接管原窗口/显示句柄。supportsAdditionalSurfaces 为 `true`，主输出 sRGB reset 许可为 `true`。BGFX Device 在 Native 初始化前验证，并在 surface 创建时解析一次。附加窗口还必须满足实际 BGFX SwapChain 能力。

窗口由平台应用或明确外部 owner 管理，Device 只借用。关闭先 RetireWindowSurface，等待渲染命令处理完成，再由窗口 owner 销毁；Task 完成不是通用 GPU fence。无效 ABI/角色/句柄明确失败，不创建新进程 owner。

## 产品组合

```csharp
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.Windows.Bgfx.Runtime;

var rendering = new BgfxRenderingBackendProvider(new WindowsBgfxSurfaceIntegration());
```

将 rendering 赋给 DefaultAdapterCatalogOptions.rendering。Editor/Player 共用实现，构建 integration 独立选择。macOS 实机结果与源码检查分别记录；Browser 不宣称附加窗口能力。

## 验证入口

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration`

<!-- api: T:Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration b5b9577af5f5c1a1d4b88bf320a66be6cfb6ec8e0ed10ec2671374e03caac046 -->
<!-- api: P:Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.supportsAdditionalSurfaces 06340877ee813dcb0bf38ff667a9e022b7d3e28c05d21ab96c762ea4eb27e4de -->
<!-- api: M:Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) f21bbc35c238d8061d5d175147f234f356524f0a7bd2239d277f27c270d9861e -->
<!-- api: M:Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.#ctor 0bd82d03aae57194fe2144b11ad13e444ea24825de2d977c1f36a0d02b59104f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7ff5a2cd2e4e7198"></a>[`public Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.WindowsBgfxSurfaceIntegration()`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx.Runtime/WindowsBgfxSurfaceIntegration.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-92aee54d029bcfcd"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx.Runtime/WindowsBgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired.<br>返回：The validated handle binding and reset capability, without transferring window ownership.<br>`handles`：The platform window owner's immutable borrowed handles.<br>`role`：The requested primary or additional presentation role. | — | `System.NotSupportedException`：The surface ABI or requested presentation role is not supported by this integration.<br>`System.ArgumentException`：The requested role or native window handle is invalid. |
| <a id="api-677f12f6ad60c112"></a>[`public bool Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx.Runtime/WindowsBgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. | — | — |
| <a id="api-9d6c26f7a6299a88"></a>[`public sealed class Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration : Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../platforms/Windows/integrations/Inno.Integration.Windows.Bgfx.Runtime/WindowsBgfxSurfaceIntegration.cs#L10) | Connects the Windows surface ABI to BGFX without exposing native BGFX types. | — | — |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
