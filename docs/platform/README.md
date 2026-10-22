# 平台与系统接入

[Wiki 首页](../README.md) · [共享后端](../backends/README.md) · [扩展指南](../architecture/PLATFORM_EXTENSION_GUIDE.md)

中立窗口和平台事件契约属于 `Inno.Platform`；系统、SDK、启动与打包实现集中在所属平台包。运行目标必须包含 CPU/ABI，维护目录不等同于发布目标。

[产品启动与 “not built”](PRODUCT_STARTUP.md)列出当前 Editor、发布 Player、Browser 的实际启动方式与 Solution 配置。

| 平台包 | 当前范围 |
| --- | --- |
| [WindowsX64](WindowsX64/README.md) | Editor 与 Player，CoreCLR / NativeAOT。 |
| [MacOSArm64](MacOSArm64/README.md) | Editor 与 Player；本轮 Windows 环境未实测 macOS。 |
| [BrowserWasm32](BrowserWasm32/README.md) | Player，解释执行 / AOT；不提供 Browser Editor。 |

## 中立契约

- [Inno.Platform](Inno.Platform.md)
- [Inno.Adapter.Platform](Inno.Adapter.Platform.md)
- [Inno.Adapter.Presentation](Inno.Adapter.Presentation.md)

SDL3 和 ImGui 的共享实现见 backend 索引。NS、iOS 与新增 CPU 的接入结构是未来规划，未建立空项目或 SDK 占位。

- [Windows 构建宿主](Windows/README.md)：OS 工具执行能力，与 WindowsX64 发布目标分别拥有。
