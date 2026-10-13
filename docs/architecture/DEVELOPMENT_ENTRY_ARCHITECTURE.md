# 开发入口与产品准备

[架构索引](README.md) · [Wiki 首页](../README.md) · [入口项目](../entry/README.md)

## 两个日常入口

| 入口 | 默认行为 |
| --- | --- |
| `InnoEngine.Editor` | 验证已准备的当前宿主 Editor 并启动；缺失或损坏时准备产品。 |
| `InnoEngine.Build` | 增量构建当前宿主 Editor；`--clean` 才先清理所选产品工作状态。 |

Windows x64 与 macOS ARM64 路由到已实现的 Editor 组合。未实现宿主明确报告不支持，不选择其他平台产品。独立发布命令使用明确目标，不用宿主覆盖游戏目标。

## 文件与依赖

```text
entry/
├─ Directory.Build.props
├─ Directory.Build.targets
├─ InnoEngine.Editor/
│  ├─ InnoEngine.Editor.csproj
│  └─ Program.cs
├─ InnoEngine.Build/
│  ├─ InnoEngine.Build.csproj
│  └─ Program.cs
└─ Inno.Entry.Bootstrap/
   ├─ DevelopmentEntry.cs
   ├─ DevelopmentBuildRequest.cs
   ├─ DevelopmentHost.cs
   ├─ DevelopmentDependencies.cs
   ├─ DevelopmentBuildTool.cs
   ├─ DevelopmentProduct.cs
   ├─ DevelopmentApplication.cs
   └─ DevelopmentProcess.cs
```

两个入口只引用 Bootstrap。Bootstrap 使用 BCL 与 Core.IO，不静态引用完整 Editor、Build 或 Native 生成工具图。其他生产项目不引用 entry。

平台 Editor 和 CLI 保持库，提供公开应用入口。Bootstrap 加载选定的已构建应用；平台产品负责实际后端与 integration 组合。Player 源项目是发布输入，最终 Exe 由平台发布模板生成。

## Editor 启动

```text
解析宿主和配置
→ 取得 checkout 读取保护
→ 检查产品 receipt 与实际部署清单
→ 有效：取得产品读取保护并再次验证
→ 无效：转为独占准备，完成后记录部署清单
→ 在原主线程启动平台 Editor
→ 应用退出后释放产品和 checkout 保护
```

产品记录位于 `artifacts/products/editor/<target>/<configuration>/ready.json`。它保存实际部署文件及哈希；只有标记文件而没有匹配部署不能启动。验证不扫描整个第三方源码闭包，也不运行 Native compiler 或生成器。

产品程序集仍位于平台项目的 SDK 输出目录。ready.json 是启动入口的部署记录，不是 Native 缓存有效性的权威依据。

修改引擎源码后运行 Build。启动入口使用已准备部署，不自动重新构建全部源码。未传游戏 Project 路径时，由平台 Editor 打开目录选择窗口；选择取消可以正常退出。

## Build 与清理

默认增量构建。Editor 配置跟随开发入口的 Debug/Release，允许显式覆盖；Support Pack 保持自己的 Release 准备语义，不随 Editor 默认构建附带发布。

产品清理先验证目标与 SDK，再按实际 SDK 输出位置建立清理计划，取得产品独占保护后执行。它使对应 receipt 失效，不清除其他产品、Native、Binding 或 Support Pack 缓存。活动 Editor 持有产品读取保护，清理不能直接移除其文件。

完整命令和当前限制见 [InnoEngine.Build](../entry/InnoEngine.Build.md)。Native、绑定、Pack 和 workspace 发布者记录明确的 ownership；缓存清理只管理登记产物，完整产品与性能验收结果单独列出。

## Build 工具准备复用

Build 工具拥有独立 `artifacts/entry/build/<configuration>/ready.json`，包含源码身份与真实部署清单。有效部署先验证实际哈希，再通过可退休检查上下文调用公开输入检查入口；相同 SDK、源码、资源、imports 和引用身份命中时直接执行工具，不再次编译 CLI。源码或 SDK 改变时重新准备。检查上下文成功卸载后才能加载实际应用，失败进入明确停止状态。

## IDE 配置

默认 Debug/Release Solution 配置构建两个入口及轻量依赖。`SourceDebug`/`SourceRelease` 用于共享源码和测试闭包。平台产品通过明确产品构建执行，Browser 发布使用独立目标和 workload。

IDE 的 `not built` 表示该项目未被本次 Solution 配置选中，不表示它失效或缺少用途。源码中的平台产品库、测试探针和工具库不应作为日常启动项；两个共享 .run 配置对应两个入口。

## 失败、退出与扩展

缺 SDK、未实现宿主或无效参数在产品清理前报告。取消等待和子进程执行时，当前 operation 排空自己创建的任务；锁文件持久保留，不能通过删除锁文件解锁。

首次 clone 需要 Git、仓库声明的 .NET SDK 与对应 Native SDK；缺失的固定 revision 子模块由准备流程初始化，入口不安装系统 SDK。新宿主只有在产品实现可用后才加入 `DevelopmentHost`。新增 backend 或游戏发布目标在发行与平台组合注册，不修改共同入口业务。

本次实际验证范围见[当前验收](CURRENT_ACCEPTANCE.md)。
