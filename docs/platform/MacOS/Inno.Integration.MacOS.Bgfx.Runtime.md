# Inno.Integration.MacOS.Bgfx.Runtime

[平台分类](README.md) · [平台索引](../README.md) · [Wiki 首页](../../README.md) · [BGFX backend](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

## 职责与边界

`MacOSBgfxSurfaceIntegration` 只负责 Cocoa 系统 surface 与 BGFX runtime SPI 的连接。依赖 BGFX Adapter 与中立 Platform SPI；不引用 Native.Bgfx、BGFX build integration、BGCS、Task 或其他平台。对象无资源所有权，产品与 Device 借用不可变配置。

## 行为与生命周期

Resolve 验证本平台 ABI、角色和非零句柄，返回不可变借用描述；成功不接管原窗口/显示句柄。supportsAdditionalSurfaces 为 `true`，主输出 sRGB reset 许可为 `true`。BGFX Device 在 Native 初始化前验证，并在 surface 创建时解析一次。附加窗口还必须满足实际 BGFX SwapChain 能力。

窗口由平台应用或明确外部 owner 管理，Device 只借用。关闭先 RetireWindowSurface，等待渲染命令处理完成，再由窗口 owner 销毁；Task 完成不是通用 GPU fence。无效 ABI/角色/句柄明确失败，不创建新进程 owner。

## 产品组合

```csharp
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.MacOS.Bgfx.Runtime;

var rendering = new BgfxRenderingBackendProvider(new MacOSBgfxSurfaceIntegration());
```

将 rendering 赋给 DefaultAdapterCatalogOptions.rendering。Editor/Player 共用实现，构建 integration 独立选择。macOS 实机结果与源码检查分别记录；Browser 不宣称附加窗口能力。

## 验证入口

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration`

<!-- api: T:Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration af6290fe213f1a44c09ae067a53f09f46c8f2524277270684102dde16a72aee9 -->
<!-- api: P:Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.supportsAdditionalSurfaces c250be34d6e75e7b38d655c93a9235eb31f00def3ecd667591f5c45f56a4acf9 -->
<!-- api: M:Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) 570df0d77a6f601fbc05374a02e771ebe8eebc5434d71b4a23b0a2304645e860 -->
<!-- api: M:Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.#ctor 76d5ec773be93603d045879cd4e55c4aa99903af48e2881b77d70f6a2cd9e918 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e11836705da2d32d"></a>[`public Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.MacOSBgfxSurfaceIntegration()`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Bgfx.Runtime/MacOSBgfxSurfaceIntegration.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8ac89968be236e31"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Bgfx.Runtime/MacOSBgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired.<br>返回：The validated handle binding and reset capability, without transferring window ownership.<br>`handles`：The platform window owner's immutable borrowed handles.<br>`role`：The requested primary or additional presentation role. | — | `System.NotSupportedException`：The surface ABI or requested presentation role is not supported by this integration.<br>`System.ArgumentException`：The requested role or native window handle is invalid. |
| <a id="api-6ccf650267a89524"></a>[`public bool Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Bgfx.Runtime/MacOSBgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. | — | — |
| <a id="api-7b520a93e85e0b82"></a>[`public sealed class Inno.Integration.MacOS.Bgfx.Runtime.MacOSBgfxSurfaceIntegration : Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../platforms/MacOS/integrations/Inno.Integration.MacOS.Bgfx.Runtime/MacOSBgfxSurfaceIntegration.cs#L10) | Connects the MacOS surface ABI to BGFX without exposing native BGFX types. | — | — |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
