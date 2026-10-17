# Inno.Integration.BrowserWasm32.Bgfx.Runtime

[平台分类](README.md) · [平台索引](../README.md) · [Wiki 首页](../../README.md) · [BGFX backend](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

## 职责与边界

`BrowserWasm32BgfxSurfaceIntegration` 只负责 Canvas 系统 surface 与 BGFX runtime SPI 的连接。依赖 BGFX Adapter 与中立 Platform SPI；不引用 Native.Bgfx、BGFX build integration、BGCS、Task 或其他平台。对象无资源所有权，产品与 Device 借用不可变配置。

## 行为与生命周期

Resolve 验证本平台 ABI、角色和非零句柄，返回不可变借用描述；成功不接管原窗口/显示句柄。supportsAdditionalSurfaces 为 `false`，主输出 sRGB reset 许可为 `false`。BGFX Device 在 Native 初始化前验证，并在 surface 创建时解析一次。附加窗口还必须满足实际 BGFX SwapChain 能力。

窗口由平台应用或明确外部 owner 管理，Device 只借用。关闭先 RetireWindowSurface，等待渲染命令处理完成，再由窗口 owner 销毁；Task 完成不是通用 GPU fence。无效 ABI/角色/句柄明确失败，不创建新进程 owner。

## 产品组合

```csharp
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.BrowserWasm32.Bgfx.Runtime;

var rendering = new BgfxRenderingBackendProvider(new BrowserWasm32BgfxSurfaceIntegration());
```

将 rendering 赋给 DefaultAdapterCatalogOptions.rendering。Editor/Player 共用实现，构建 integration 独立选择。macOS 实机结果与源码检查分别记录；Browser 不宣称附加窗口能力。

## 验证入口

BgfxSurfaceIntegrationTests 通过公开 SPI 验证未知 surface、错误角色和组合；BgfxSurfaceRetirementTests 验证真实窗口/命令退休。本轮结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration`

<!-- api: T:Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration 300a1077561de1918a6472e34cf38bd0cbfc4142d3bf2c4d8307f3ca8748fa99 -->
<!-- api: P:Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.supportsAdditionalSurfaces 01e851fd1c010d8ea089000fb2c04ae254b49199d864c1cafecb1153c12d9ff1 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) 039492e575e46847f6d0bd1e5f9cadcbfa5edfded11414283b54734587bd5106 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.#ctor 1322b89ab21c18988493ff043fc750b4a1431c05692e9acd291eb5058381e50b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f2feb73a3d5d549c"></a>[`public Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.BrowserWasm32BgfxSurfaceIntegration()`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx.Runtime/BrowserWasm32BgfxSurfaceIntegration.cs#L10) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-267b57b91b7e0dae"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx.Runtime/BrowserWasm32BgfxSurfaceIntegration.cs#L35) | Validates this system's borrowed surface ABI before native renderer ownership is acquired.<br>返回：The validated handle binding and reset capability, without transferring window ownership.<br>`handles`：The platform window owner's immutable borrowed handles.<br>`role`：The requested primary or additional presentation role. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The surface ABI or requested presentation role is not supported by this integration.<br>`System.ArgumentException`：The requested role or native window handle is invalid. |
| <a id="api-44c3ee4179da8b4a"></a>[`public bool Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx.Runtime/BrowserWasm32BgfxSurfaceIntegration.cs#L15) | Gets whether this system integration permits additional window surfaces. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-522d601b62b517c5"></a>[`public sealed class Inno.Integration.BrowserWasm32.Bgfx.Runtime.BrowserWasm32BgfxSurfaceIntegration : Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx.Runtime/BrowserWasm32BgfxSurfaceIntegration.cs#L10) | Connects the Browser surface ABI to BGFX without exposing native BGFX types. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Rendering.Bgfx](../../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
