# InnoEngine.Editor

[开发入口](README.md) · [Wiki 首页](../README.md) · [平台产品](../platform/PRODUCT_STARTUP.md)

## 职责与依赖

普通 .NET 9 可执行项目。项目求值时选择 WindowsX64 或 MacOSArm64 Editor 的 ProjectReference，Program 在原始主线程直接调用对应组合的公开 Run。共同 Editor、服务和后端仍由所属类库维护。

入口与平台产品使用正常项目构建。修改 C# 源码后再次运行，SDK 编译受影响的依赖；程序不在启动过程中调用 MSBuild，也不加载另一份预构建 Editor。

## 快速使用

从仓库根目录执行：

```sh
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
dotnet run --configuration Release --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj -- /absolute/MyGame
```

无目录参数时打开原生项目目录选择窗口；取消正常退出。原生对话框取消的限制见[产品启动](../platform/PRODUCT_STARTUP.md)。首次构建需要当前平台工具链，之后复用匹配的支持缓存。

## API、所有权与失败

本项目没有 public/protected API。运行时只调用已由工程选择的产品。Ctrl+C 取消令牌由入口拥有，订阅在退出时移除；产品 Host 负责会话、设备、任务和 callback 的生命周期。

入口持有当前目标与配置的产品读取 lease，显式 product clean 和 CLI editor 构建先取得相同 owner 的独占权限。锁位置由本次工程构建声明，不扫描源码 checkout，不依赖启动 receipt。

退出码为 0 正常结束、2 取消、1 未处理失败。不支持的 SDK 宿主在构建时明确拒绝。

## 输出与验证

平台产品准备的 Native 文件、部署索引和后端工具作为传递构建输出进入 entry 的输出目录。加载继续使用显式部署映射。

工程检查与编译结果见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。Rider 窗口、输入及项目加载的实机验证由用户完成；macOS 工程求值不等于 macOS 实机通过。
