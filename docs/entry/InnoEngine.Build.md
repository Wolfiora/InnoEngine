# InnoEngine.Build

[开发入口](README.md) · [Wiki 首页](../README.md) · [构建应用](../build/Inno.Build.Cli.md)

## 职责与边界

日常构建可执行入口。无参数时增量构建当前宿主的 Editor；清理由明确参数控制。
入口只静态引用 Bootstrap，将明确目标交给共享构建应用，不在 backend 中推导平台。

## 快速使用

从仓库根目录执行：

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --configuration Release
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- --clean
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
```

默认 Editor 配置与入口的编译配置一致。显式 `--configuration` 选择产品配置；Support Pack 默认 Release。
`--clean` 是无值布尔参数，只用于 Editor 构建。重复、未知或缺值参数均失败。

## 初始化、所有权与退出

入口取得 checkout 独占 lease，初始化缺失源依赖，准备并调用 `BuildApplication`。
Editor 构建先校验目标和 SDK，再取得产品/目标/配置对应的独占 lease。
`clean --scope product` 与 `--clean` 使用同一清理计划：失效准备记录，清理所选产品的托管输出和中间目录。
其他配置、Native/Bindings 缓存、Support Pack 和游戏发布输出不属于该清理计划。

入口启动的 Editor 运行期间持有读取 lease；Build 等待其退出。等待和子进程可取消。
预检失败和预取消不执行清理。清理后的编译失败不会恢复已清理的可重建输出，也不会登记可运行产品。
成功构建当前宿主 Editor 后，Bootstrap 对实际部署文件建立完整哈希清单。随后同目标、同配置的 Editor 验证并启动产品。

## API 与扩展

本项目没有 public/protected API。命令实现属于 [Bootstrap](Inno.Entry.Bootstrap.md) 和 [BuildApplication](../build/Inno.Build.Cli.md)。
新增平台先提供平台产品和发行贡献，再更新开发宿主路由。游戏脚本、资产导入与 reload 仍属于 Editor 会话。

## 当前限制与验证

Windows x64、macOS ARM64 有 Editor 组合；Linux 没有完整 Editor，默认构建明确拒绝。
`clean --scope cache --dry-run` 预览登记 owner 的无引用候选，去掉 `--dry-run` 执行有界退休。`--cache-budget-gib`、`--cache-min-free-gib` 与 `--cache-time-seconds` 覆盖预算；保护集合超过预算时不会强制删除。产品清理与缓存退休分别执行。
本次产品、性能与运行验收状态见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
