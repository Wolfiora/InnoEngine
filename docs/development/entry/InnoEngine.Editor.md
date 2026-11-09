# InnoEngine.Editor

[开发入口](../building.md) · [Wiki 首页](../../README.md) · [平台产品](../../platform/PRODUCT_STARTUP.md)

## 职责与依赖

普通 .NET 10 可执行项目。项目求值时选择 WindowsX64 或 MacOSArm64 Editor 的 ProjectReference，Program 在原始主线程直接调用对应组合的公开 Run。共同 Editor、服务和后端仍由所属类库维护。

Windows 入口可执行文件通过 `<ApplicationIcon>` 嵌入 `build/branding/logo.ico`。Explorer、任务管理器和 SDL 任务栏窗口读取同一图标资源，因此不需要在窗口创建后另行设置图标。macOS 的 `dotnet publish -r osx-arm64` 在 publish 目录旁生成 `InnoEngine.Editor.app`，把 `build/branding/logo.icns` 放入 `Contents/Resources` 并写入 `Info.plist` 的 `CFBundleIconFile`；`<ApplicationIcon>` 在 macOS 不参与。构建资源说明见 [build/branding](../../../build/branding/README.md)，设计源见[品牌资源](../../../assets/branding/README.md)。

入口与平台产品使用正常项目构建。修改 C# 源码后再次运行，SDK 编译受影响的依赖；程序不在启动过程中调用 MSBuild，也不加载另一份预构建 Editor。

入口通过 SDK 的本地构建属性和 `ProjectReference` metadata 明确拥有所选平台产品的构建闭包。即使外层 Solution 调度器关闭引用构建或排除产品作为独立构建根，正常入口 Build 仍由 SDK 构建其真实依赖。Design-time 保留外层的引用构建策略，不启动产品 Native 准备；没有 IDE 专用入口、任务或输出目录。

## 快速使用

从仓库根目录执行：

```sh
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
dotnet run --configuration Release --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj -- /absolute/MyGame
```

无目录参数时打开原生项目目录选择窗口；取消正常退出。原生对话框取消的限制见[产品启动](../../platform/PRODUCT_STARTUP.md)。首次构建需要当前平台工具链，之后复用匹配的支持缓存。

### macOS 应用包

在 macOS 上执行 Release publish 时，`CreateMacOSEditorApplicationBundle` 在 publish 目录旁生成 `InnoEngine.Editor.app`：

```sh
dotnet publish entry/InnoEngine.Editor/InnoEngine.Editor.csproj -c Release -r osx-arm64
```

默认输出位于 `entry/InnoEngine.Editor/bin/Release/net10.0/osx-arm64/InnoEngine.Editor.app`。包内 `Contents/MacOS` 是完整 publish 输出，`Contents/Resources/logo.icns` 来自 `build/branding`，`Contents/Info.plist` 声明 `CFBundleExecutable`、`CFBundleIconFile=logo.icns` 和稳定标识 `io.innoengine.editor`。目标只在 macOS 且 RID 为 `osx-*` 时运行；不执行代码签名或公证，正式分发需要另行处理。默认 framework-dependent；分发给未安装 .NET 10 的机器时增加 `--self-contained true`。日常 `dotnet run`/Rider 仍运行裸可执行文件，不读取该包。

## API、所有权与失败

本项目没有 public/protected API。运行时只调用已由工程选择的产品。Ctrl+C 取消令牌由入口拥有，订阅在退出时移除；产品 Host 负责会话、设备、任务和 callback 的生命周期。

入口持有当前目标与配置的产品读取 lease，显式 product clean 和 CLI editor 构建先取得相同 owner 的独占权限。锁位置由本次工程构建声明，不扫描源码 checkout，不依赖启动 receipt。

退出码为 0 正常结束、2 取消、1 未处理失败。不支持的 SDK 宿主在构建时明确拒绝。

## 输出与验证

平台产品准备的 Native 文件、部署索引和后端工具作为传递构建输出进入 entry 的输出目录。加载继续使用显式部署映射。

平台组合库通过共同的 `InnoExposeRuntimeClosure` SDK 构建规则传递自身 `.deps.json` 与实际 `ReferenceCopyLocalPaths`。动态发现的 Panel 和 Feature 随运行闭包部署；`PrivateAssets="compile"` 继续隔离实现侧编译引用，入口不维护扩展类型名单。

工程检查与编译结果见[当前验收](../../../.agents/records/acceptance/current.md)。Rider 窗口、输入及项目加载的实机验证由用户完成；macOS 工程求值不等于 macOS 实机通过。
