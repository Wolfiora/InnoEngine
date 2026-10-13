# Inno.Integration.WindowsX64.Bgfx.Runtime

[平台分类](README.md) · [平台索引](../README.md) · [Wiki 首页](../../README.md) · [BGFX backend](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

## 职责与边界

`WindowsX64BgfxSurfaceIntegration` 只负责 Win32 系统 surface 与 BGFX runtime SPI 的连接。依赖 BGFX Adapter 与中立 Platform SPI；不引用 Native.Bgfx、BGFX build integration、BGCS、Task 或其他平台。对象无资源所有权，产品与 Device 借用不可变配置。

## 行为与生命周期

Resolve 验证本平台 ABI、角色和非零句柄，返回不可变借用描述；成功不接管原窗口/显示句柄。supportsAdditionalSurfaces 为 `true`，主输出 sRGB reset 许可为 `true`。BGFX Device 在 Native 初始化前验证，并在 surface 创建时解析一次。附加窗口还必须满足实际 BGFX SwapChain 能力。

窗口由平台应用或明确外部 owner 管理，Device 只借用。关闭先 RetireWindowSurface，等待渲染命令处理完成，再由窗口 owner 销毁；Task 完成不是通用 GPU fence。无效 ABI/角色/句柄明确失败，不创建新进程 owner。

## 产品组合

```csharp
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.WindowsX64.Bgfx.Runtime;

var rendering = new BgfxRenderingBackendProvider(new WindowsX64BgfxSurfaceIntegration());
```

将 rendering 赋给 DefaultAdapterCatalogOptions.rendering。Editor/Player 共用实现，构建 integration 独立选择。macOS 实机结果与源码检查分别记录；Browser 不宣称附加窗口能力。

## 验证入口

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration`

<!-- api: T:Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration bf65d57cfdb8408594f979cdb52b028aaf35cd5378abfde964a551703a2bc84d -->
<!-- api: P:Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.supportsAdditionalSurfaces 07aca03858ac38be8c717c7249b68863ae72454428e0e7d6ffb80a588f982b97 -->
<!-- api: M:Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) 777cc8f0b551945187b48f802bfa1c66fa3ad7da75b34e8c6636e3d2e8787a0e -->
<!-- api: M:Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.#ctor 9dbca1c0d944514b8a77bf96e86a4135929be0f0c6e662d1869753852b8e1f21 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b8f624356a57ca83"></a>[`public Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.WindowsX64BgfxSurfaceIntegration()`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime/WindowsX64BgfxSurfaceIntegration.cs#L10) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-153e5710f81bbb69"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime/WindowsX64BgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired.<br>返回：The validated handle binding and reset capability, without transferring window ownership.<br>`handles`：The platform window owner's immutable borrowed handles.<br>`role`：The requested primary or additional presentation role. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The surface ABI or requested presentation role is not supported by this integration.<br>`System.ArgumentException`：The requested role or native window handle is invalid. |
| <a id="api-9ee6f654ee11b4c1"></a>[`public bool Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime/WindowsX64BgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-52ef7479dc522194"></a>[`public sealed class Inno.Integration.WindowsX64.Bgfx.Runtime.WindowsX64BgfxSurfaceIntegration : Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Bgfx.Runtime/WindowsX64BgfxSurfaceIntegration.cs#L10) | Connects the Windows surface ABI to BGFX without exposing native BGFX types. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
