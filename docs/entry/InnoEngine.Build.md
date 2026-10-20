# InnoEngine.Build

[开发入口](README.md) · [Wiki 首页](../README.md) · [构建应用](../build/Inno.Build.Cli.md)

## 职责与依赖

普通 .NET 10 可执行项目，通过 ProjectReference 直接调用 BuildApplication.Run。SDK 编译工具源码；运行入口不再准备、发布或反射加载另一份 CLI。

无参数时准备当前 SDK 宿主的 Editor 支持产物。宿主只在 entry 选择，显式游戏发布目标继续由调用参数决定。

## 快速使用

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support --product player --target windows-x64
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --configuration Release
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --clean
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
```

| 命令 | 行为 |
| --- | --- |
| 无参数 / support | 准备所选产品的 Native、绑定和后端工具；有效缓存复用，不编译整个 Editor C# 产品。 |
| editor | 使用 SDK 构建 Editor entry 及依赖。 |
| editor --clean | 先清理所选产品工作目录，再构建。 |
| clean --scope product | 清理 entry 与对应平台产品的所选配置；dry-run 仅预览。 |
| support-pack | 独立准备游戏发布所需的完整引擎支持闭包。 |
| game / plugin / scripts / import-sample | 保留各自的项目内容与代码工作流。 |
| verify / verify-native / bindings / shader | 保留明确的验证、生成与离线编译入口。 |

support、editor 和 product clean 默认跟随入口编译配置；Support Pack 默认 Release。--configuration 可显式覆盖。--clean 只允许用于 editor，参数无效时明确失败。

## API、所有权与取消

本项目没有 public/protected API。具体命令由 CLI 类库实现，复用标准发行组合与现有 Native plan。取消令牌由 Program 拥有，Ctrl+C 订阅在退出时释放。

支持产物的读取 leases 保持到本次消费者结束。产品清理先验证目标和 SDK，再获取独占产品 lease；活动 Editor 阻止相同产品被清理。Native/Binding 缓存、其他配置、Support Pack、游戏输出和用户数据不属于产品 clean。

共享缓存清理仍仅处理已登记 owner；不传 target 检查全部，传 target 只选择对应目标可退休产物。程序不自动认领旧入口遗留的未登记目录。

## 验证与限制

Build 工具的编译是普通 SDK 工程构建。支持准备需要对应 SDK；可移植目标描述能在其他宿主编译，不保证该宿主能执行目标 Native 编译。

本轮不提供独立引擎发行包。实际检查状态见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
