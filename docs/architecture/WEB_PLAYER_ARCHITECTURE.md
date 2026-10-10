# Browser Player：共享运行流与平台边界

[架构索引](README.md) · [Wiki 首页](../README.md) · [Browser 平台包](../platform/Browser/README.md) · [扩展指南](PLATFORM_EXTENSION_GUIDE.md)

## 当前组合

Scene、玩法、内容、Input、Rendering、Audio、Text、UI、Storage 使用共同领域契约。Browser 包负责页面/JS 生命周期、HTTP 下载、调度、浏览器存储、Emscripten SDK、最终静态链接与站点布局。共享 Shell/Player Runtime 不根据 OS 或托管运行时选择行为。

```text
Editor / CLI / MSBuild
→ StandardBuildDistribution
→ BrowserBuildModule + 所选 managed compiler
→ 共享内容/代码 pipeline
→ backend-owned static recipe + 同一目标绑定闭包
→ Browser 包装/校验/原子提交

页面 main.js / BrowserBridge
→ BrowserPlayerComposition
→ PlayerApplication / Shell / EngineHost / RuntimeSession
→ 同一领域运行服务与 Core Events
```

## 必要差异及归属

| 差异 | 实现 owner | 共用部分 |
| --- | --- | --- |
| requestAnimationFrame、暂停/恢复、页面回调 | Browser Player/Bridge | ScheduledShellFrameDriver 和 Shell 生命周期。 |
| HTTP metadata 与 owned pack stream | HttpPlayerContentSource | ContentPackReader、Content store、Asset lease、只读 Settings。 |
| 浏览器存储 | Browser Storage Adapter | StorageScope、Runtime API。 |
| 单线程、调用线程渲染、inline 日志 | Browser composition 显式配置 | Job、Rendering、LogRouter 契约。 |
| wasm32 ABI、静态原生链接、WebGL 2 | Browser SDK/构建包与共享 backend recipe | 同一窄 facade、BGCS 生成链和中立渲染协议。 |
| 解释执行或 AOT | DotNet managed deployment compiler | 同一冻结代码闭包、静态注册与 Browser 宿主。 |

Browser 目标稳定 ID 仍是 `browser-wasm`，描述明确为 wasm32；这个 ID 不表示构建宿主必须是 Windows。SDK 从所选 .NET 工程/workload 解析，Node、Python、CMake、Emscripten 只在操作边界冻结，不在领域查 PATH 或选择最高版本。

## 原生与绑定

SDL、BGFX、MiniAudio、Text、RmlUi 在各自 backend 维护一份源码。组件的 StaticLibraries.cmake 声明自己的源、依赖、语义选项和 archive；Browser 聚合只组合选定组件、SDK约束、异常/longjmp及最终链接要求。

目标 C 桥与 managed 单文件位于 Native owner 的 `obj/browser-wasm/<generationFingerprint>`。Browser managed 发布消费同一次准备的 BindingSelection，发现身份不一致时失败。CMake 中间态属于 Browser 构建 owner 的 `obj/native`；完整 archive 闭包按目标/指纹位于 artifacts，不覆盖宿主生成物。

`wasm_sjlj_shim.c` 属于当前 SDK 的 ABI/link 边界，不能进入玩法或服务层。更换 runtime/SDK 必须重新验收 interop、异常、回调和最终链接，不能凭成功编译声称可运行。

## 构建入口

生产入口共六个，构建程序只保留 Inno.Build.Cli。示例中的 SDK 必须具有工程需要的 Wasm workload，路径按本机安装填写：

```powershell
dotnet build build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -c Release -m:1
dotnet build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll game `
  --tools-target windows-x64 --target browser-wasm --deployment mono-wasm `
  --project <project-directory> --support-packs <support-pack-root> --output <output-directory>
```

`mono-wasm-aot` 选择 AOT；作者工具目标独立于游戏目标，导出 Browser 不改变当前 Editor 的 Native 或 ImGui Shader。内容 metadata 从同一 catalog 读取 Pack 身份，不重复维护 content-pack.txt，不先写入另一份 /Content 再解压。

## 生命周期和未来接入

共同退出协议停止新工作、取消并完成任务、注销回调、提交存储，再退休资源。输入由 Platform Event → Core Events → EventInput → Session 快照；没有 Browser 专用玩法输入入口。

将来接入 CoreCLR WebAssembly 需要新的部署 compiler 和匹配 SDK/link resolver，重新验证启动、回调、异常、interop与实际游戏。Browser宿主及共同领域可以复用，不在Scene、Rendering或Input增加runtime switch。当前不宣称这个未来部署已实现，也不提供 Browser Editor。

本轮解释执行、AOT、音频、持久化、昼夜颜色、光照和星光的实际结果见平台归属验收；启动成功不能代替视觉与玩法验证。
