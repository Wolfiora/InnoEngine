# Inno.Integration.Linux.Bgfx

[平台索引](README.md) · [Wiki 首页](../../README.md) · [扩展指南](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Linux 平台与 BGFX 的构建接入 owner。依赖本平台的中立构建模块和 BGFX 构建契约；不引用 Inno.Native.Bgfx、其他平台或 Standard Distribution。Native/Shader 参数和 compiler 在这里组合；平台 packager 与共享 BGFX 均不依赖本程序集。

## 初始化、工作流与所有权

Standard Distribution 读取不可变 profile，绑定平台贡献和 compiler 工厂。compiler 借用当前创作 generation，不接管 Assets、Serialization 或 Types，不能跨 generation 缓存。产品组件列表由 StandardNativeBuildPlans 声明，integration 不重复注册发行。Linux 仅提供现有 Native 工具 profile，不注册 Player 或内容发布目标。

```csharp
using Inno.Integration.Linux.Bgfx;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Toolchains.Bgfx;

BgfxNativeBuildProfile profile = LinuxBgfxIntegration.CreateNativeProfile("linux-x64");
```

示例中的 assets/serialization/types 由创作 owner 提供。未知或不支持的 SDK、目标与图形能力明确失败；不自动选择宿主 OS。没有资源性 protected 扩展点；Native invocation 的行为实现仍属于 profile。

## 测试与平台状态

Build 的 PlatformBackendIntegrationTests、BuildCompositionTests、NativeComponentBuildOptionsTests 与真实发布验证此契约。当前实机与未实测状态见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Integration.Linux.Bgfx.LinuxBgfxIntegration`

<!-- api: T:Inno.Integration.Linux.Bgfx.LinuxBgfxIntegration a4fd94cf0e22098324a9e37ce56d7dbf1f45e481cd04c387b279bf247c91ac19 -->
<!-- api: M:Inno.Integration.Linux.Bgfx.LinuxBgfxIntegration.CreateNativeProfile(System.String) b8de05f4d2c8c5b3531ed7428afb484945a0ae8d90050df0412f9c19e3c20830 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4df4e94876f1a4f6"></a>[`public static Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile Inno.Integration.Linux.Bgfx.LinuxBgfxIntegration.CreateNativeProfile(string targetId)`](../../../platforms/Linux/integrations/Inno.Integration.Linux.Bgfx/LinuxBgfxIntegration.cs#L23) | Resolves SDK invocation data for an implemented Linux native target.<br>返回：Immutable backend configuration; unsupported targets fail without tool execution.<br>`targetId`：The explicitly requested native target. | — | `System.NotSupportedException`：The native target is not implemented by this module. |
| <a id="api-611747c573a75b06"></a>[`public static class Inno.Integration.Linux.Bgfx.LinuxBgfxIntegration`](../../../platforms/Linux/integrations/Inno.Integration.Linux.Bgfx/LinuxBgfxIntegration.cs#L9) | Connects the implemented Linux SDK targets to BGFX tool builds without registering a Player. | — | — |

## 项目依赖

- [Inno.Build.Linux](Inno.Build.Linux.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
