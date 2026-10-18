# 两个开发入口验收（2026-10-10）

[设计与边界](../../../docs/architecture/DEVELOPMENT_ENTRY_ARCHITECTURE.md) · [架构索引](../../../docs/architecture/README.md) · [启动指南](../../../docs/platform/PRODUCT_STARTUP.md)

## 用户问题与源码结果

- 日常启动只保留 InnoEngine Editor、InnoEngine Rebuild 两个默认 Exe；其余平台 Editor、Player 宿主和 Build.Cli 为库。
- 宿主判断仅属于 entry，Windows/macOS 显式调用各自组合；Linux 只提供已有 Native 工具重建，不伪造 Editor。
- 无项目参数时打开 SDL 原生文件夹选择窗口；取消正常退出，不硬编码 Samples 位置。
- Rebuild 先校验目标工具链，再清理可重建目录并构建本机产品；入口生命周期复用 Core.IO lease。
- BGCS 作为 dependencies 下固定 revision 子模块，独立源码和 targets 边界保留。

## 本轮证据

基线与结果：`artifacts/acceptance/development-entry-2026-10-10/`。
开始 revision：8070302d0daa7ea19b2211b224c2bd6cb4dca1b9，保留原有未提交修改。
BGCS pin：d4bfe3939b6ab2c404080f9da58e4b5f64c861be。
Windows .NET SDK 9.0.318；本机 SDK 不含 wasm-tools。
只读 `git ls-remote` 已确认 BGCS pin 为远端 main 的当前 HEAD；证据为 `results/bgcs-remote-pin.txt`。没有把这一检查当作全新网络 clone 的完整执行证据。

| Gate | 当前状态 |
| --- | --- |
| 两个入口独立编译、help/status | 最终默认 Solution Build 通过，0 警告/错误，8.22 秒；status/help 通过；无 BGCS/extern 独立副本冷构建及默认 Rebuild 通过 |
| Solution 默认启动闭包、完整源码配置 | SourceDebug 完整 Build 通过，0 警告/错误，18:55.80；默认配置只选 7 个 bootstrap 依赖项目 |
| Windows Editor 构建与重复 smoke 启动 | 平台库冷构建、统一入口首次及 Rebuild 后重启的 8 帧 smoke 均通过；最终 exit 0，构建 0 警告/错误，初始化、首帧、退出和释放完整 |
| Rebuild 真实本机执行与预检失败 | 无参数统一入口通过，exit 0，2736.073 秒；Windows Support Pack 已原子发布；不支持目标预检失败且 sentinel 保留 |
| Solution inventory / 架构 / 文档 | 232/232 项目，144 个非空 Folder，0 游离项目；最终真实 MSBuild 图和架构检查通过；2053 个链接有效；93 个变更路径已分类 |
| 内容测试进程消费者 | ContentCacheProcessTests 1/1 通过 |
| 还原与 viewport 回归 | OwnedRestore/StaticGraphRestore 7/7、ViewportMetrics 6/6 通过 |
| 原生目录选择窗口人工选择/取消 | 未执行，本轮不使用 Computer Use |
| Rider/VS 真实菜单、窗口交互 | 未执行，以项目类型/配置证据独立报告 |
| 完全从零的网络 clone 与首次全部依赖初始化 | 未完整执行；无依赖 bootstrap 冷编译和 BGCS 远端 pin 已分别验证 |
| macOS / Linux 实机 | 未实测 |
| Web / NativeAOT 游戏完整发布运行 | 本轮启动整改尚未重跑，不能沿用历史结果当作本轮通过 |

## 公开契约变化

- `DevelopmentEntry.RunEditor(string[])` 与 `RunRebuildAsync(string[])`：两个开发入口共用准备、宿主路由和取消。
- 平台 `WindowsEditorComposition.Run(string[], CancellationToken)` / `MacOSEditorComposition.Run(...)`：公开的主线程组合调用，System 签名不泄漏平台类型。
- `BuildApplication.Run(string[], CancellationToken)`：工具应用库；取消和 Console 订阅由外层入口持有。
- `Sdl3FolderDialog.SelectFolder(Sdl3PlatformApplication)`：backend 原生 UI 能力，借用 application，callback 完成后释放其 owner。

## 清洁度与所有权

新建三个 entry 项目；移除平台 Editor Program 和内部 CLI Program。Player Program 保留为发布输入，没有 Exe 源工程 startup。
CacheProbe 的真实进程测试仅由所属测试项目生成隔离 Exe，正常 Solution 求值为 Library。
当前源码没有旧入口兼容 wrapper；Player 发布模板仍为真实 Exe。
MSBuild、Docs 和消费者同步；两个开发入口不把具体平台依赖反向传入共享业务。

## 命令与证据对应

命令在仓库根目录执行；长构建使用 `--disable-build-servers -m:1 -nodeReuse:false`，独立构建作用域不继承 Rider 的 Solution 属性。

| 验证入口 | 证据 |
| --- | --- |
| 默认 `dotnet build InnoEngine.sln --configuration Debug` | `logs/entry-ready-build.log` |
| `dotnet clean InnoEngine.sln --configuration SourceDebug`，随后独立 `dotnet build InnoEngine.sln --configuration SourceDebug` | `logs/source-debug-clean.log`、`logs/source-debug-build-phased.log` |
| 不含 BGCS/extern 的独立 checkout 副本：默认 Build 和 SDK Rebuild | `logs/bootstrap-without-submodules.log`、`logs/bootstrap-rebuild-final.log` |
| 两个生成器的产品目标冷还原与编译 | `logs/analyzer-clean-restore.log` |
| 真实无参数 `InnoEngine.Rebuild.dll` | `logs/rebuild-final.log`、`results/rebuild-final.json` |
| 统一 Editor 入口，FlappyBird，`--graphics-api d3d11 --smoke-frames 8` | `logs/editor-smoke-final.log`、`logs/editor-smoke-restart.log`、`results/editor-smoke-restart.json` |
| 公开 BuildApplication 的 `verify . --project-graph <输出>` | `logs/architecture-final.log`、`results/project-graph.json` |
| 真实多进程 ContentCache、OwnedRestore/StaticGraphRestore、ViewportMetrics 定向测试 | `logs/cache-process-tests.log`、`logs/restore-tests.log`、`logs/viewport-tests.log` 及对应 TRX |
| 项目/Folder/Exe、Markdown 链接、逐文件变更闭包 | `results/layout.json`、`results/document-links.json`、`file-map.tsv` |

架构验证的库式命令由本轮隔离 public runner 调用真实公开入口；没有新增生产 Program、私有反射或测试后门。
本轮只报告上述实际命令的结果；完整源码 Build 不表示全部测试已执行，8 帧 smoke 不表示玩法、视觉、音频或真实焦点已验收。

## 耗时、原生警告与磁盘边界

最终无参数 Rebuild 从 19:02:46 至 19:48:22 完成，包含工具应用准备、SDK Clean、Debug Editor、Release Native/Shader 工具及 Windows Player Support Pack。
这是本轮清理后的准备测量，不能当作日常纯增量构建耗时，也没有前后同条件的性能改善结论。
其最后的 Player 工程构建为 18:37.93，MSBuild 汇总为 0 警告/错误；这些阶段存在包含关系，不能把各日志时长相加当作总耗时。

原生着色器工具的一次实际编译为 8:10.50，0 错误、2942 条警告。日志保留 MSB8029、第三方 C4819/C4702 及 BGCS safety diagnostics；原生流程没有被写成零警告通过，没有修改 extern 或生成绑定来消除诊断。
Windows Support Pack 当前指针为 `9ea9b25f09a4552ac91cbfaaf1513c89c551b02bffc68582ff353ddd48eecd7d`。

Rebuild 后的统一 Editor 再次启动共 847.000 秒，其中 SDK 产品构建为 13:51.04，随后 Host 初始化约 12 秒并完成 8 帧、状态保存和最终释放。
以上为修复前测量；后续已准备产品直接启动整改、Support Pack 分离及当前证据见[直接启动验收](EDITOR_PREPARED_STARTUP_ACCEPTANCE_2026_10_10.md)。
这次启动在 Release Support Pack 准备后重新编译 Debug 依赖，不能标记为纯缓存热启动。当前入口每次都会进行真实产品 Build；本轮保证启动职责简单和配置明确，没有即时启动或整体提速结论。

本轮清理由当前 checkout 的 Rebuild 和 SDK Clean 管理；运行中的入口/CLI、Native 有效缓存、游戏数据、验收日志和全局 NuGet/SDK 缓存保留。
冷副本和 public runner 属于隔离验收产物，没有进入生产 Solution。磁盘最终观察另写入 `results/disk-final.json`，不能由一次空间观察推断其他历史缓存的来源。

## 构建问题与修复依据

1. 产品还原曾将生成器 assets 写入目标目录，但编译使用中立目录。生成器现在在 SDK props 导入前声明工具身份；独立冷副本验证两个生成器仅使用中立 assets。
2. Source Solution restore 不混入平台产品根；普通还原选择明确的中立源码集合，产品按目标另行还原。静态图的完整全局属性序列化继续保留，超长属性测试通过。
3. 逐项目 SDK Rebuild 曾交错清理生成器，导致 CS0006。开发 Rebuild 先完成 SDK Clean，再由新进程 Build；Analyzer 保留 Solution 上下文，私有 Task 引导使用自己的图。
4. 库式调用的架构审计需显式提供其 Roslyn 契约元数据；无法解析的公开类型仍报告失败，没有放宽检查。
5. 进程测试 fixture 使用独立 bin/obj；显式排除所有 bin/obj 源文件，防止普通和进程构建产生重复 assembly attribute。

失败尝试的日志保留在同一 logs 目录；不能把修复前的失败记为通过。完整 Source 构建、定向测试与最终产品准备分别记录。

## 本轮结论与仍需实际环境的检查

两个开发入口、共享启动契约、唯一本机路由、固定源码依赖、普通还原/构建、完整共享源码和定向回归已经完成本机验证。
Windows SDK 不含 wasm-tools，当前 Editor 真实两次启动均成功，没有将 Web workload 混入本机启动闭包。
Shared Host 不依赖 entry，Player 发布模板仍生成 Exe，源码库不会作为额外日常启动项目。

原生目录选择器的选择/取消、实际 Rider/VS 下拉菜单、窗口拖动/焦点/视觉、macOS/Linux 实机及本轮 Web/NativeAOT 发布运行尚未完成。
这些项独立保留；本轮没有 Computer Use，也不声称所有历史计划和全部设备验收无保留完成。

没有自动提交。Glass.aiff 在 Windows 不可访问，提示音未播放。
