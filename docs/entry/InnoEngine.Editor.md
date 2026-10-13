# InnoEngine.Editor

[开发入口索引](README.md) · [Wiki 首页](../README.md) · [Bootstrap](Inno.Entry.Bootstrap.md)

## 职责与依赖

唯一 Editor 开发可执行项目。`Program` 内部调用 `DevelopmentEntry.RunEditor`，只依赖 Bootstrap。
启动时验证当前目标/配置的已准备产品，通过公开 `Run` 调用平台组合。有效产品启动不执行 Git、MSBuild、绑定生成或 Native 编译。
首次缺少有效准备记录，或部署文件缺失/损坏时，才初始化缺失子模块并构建当前宿主的 Editor。
不把整个平台/backend 闭包放进入口的静态 ProjectReference，首次编译入口只需要 .NET 9。

## 使用

```sh
dotnet run --project "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"
dotnet run --project "entry/InnoEngine.Editor/InnoEngine.Editor.csproj" -- /absolute/MyGame --smoke-frames 60
```

无目录参数时通过 SDL 原生文件夹选择窗口选择已有项目；取消正常退出，不创建默认游戏。
原生对话框没有程序化取消入口；运行取消在用户选择或关闭对话框后生效，callback owner 保留至原生完成。
修改引擎/backend 源码后运行 InnoEngine.Build，完成后再启动 Editor。普通启动使用已经构建的产品，不扫描 SDK 或自动编译引擎源码。游戏脚本/资产仍由 Editor 既有导入与 reload 流程处理。
Debug 与 Release 的准备记录隔离；Release Build 不代表已准备 Debug Editor。
Windows x64 与 macOS ARM64 有真实 Editor 组合。未实现的宿主明确拒绝启动。

## API、所有权与失败

本可执行项目没有 public/protected API。主线程同步等待准备，再在原始线程执行 UI；不是线程池运行 Editor。
运行期间持有 checkout 和当前产品的共享读取 lease，Build 等待其退出。Bootstrap 释放解析订阅和 lease；平台 Host 释放会话、设备、任务及 callback。
Git/SDK/构建失败返回非零；缺失平台不切换到另一平台。`--help`、`--status` 不启动构建。
准备记录位于 `artifacts/products/editor/<target>/<configuration>/ready.json`，包含实际程序集位置与部署文件 SHA-256。
启动检查完整文件集合与内容哈希，不以单一完成标记、长度或 mtime 判断可运行。记录描述已构建产品，不表示当前源码自动同步。

IDE 启动配置和产品状态见[启动指南](../platform/PRODUCT_STARTUP.md)。
