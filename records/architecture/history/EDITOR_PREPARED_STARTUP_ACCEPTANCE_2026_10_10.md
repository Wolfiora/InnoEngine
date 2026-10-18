# 已准备 Editor 直接启动整改与验收

[架构索引](../../../docs/architecture/README.md) · [开发入口设计](../../../docs/architecture/DEVELOPMENT_ENTRY_ARCHITECTURE.md) · [启动指南](../../../docs/platform/PRODUCT_STARTUP.md)

## 用户提出的问题

1. Rebuild 成功后仍需要约 14 分钟才能启动，违背日常启动预期。
2. 编译日志显示用户临时目录中的 InnoTools，无法确认生成物 owner。
3. 发布游戏是否也要求用户进行本地编译。

## 当前源码整改

- `DevelopmentEntry.RunEditor` 先取得共享 checkout lease，检查同目标/配置的已准备产品。校验通过直接在原始主线程调用公开 Editor Run，不执行 Git、MSBuild、绑定生成或 Native recipe。
- 新内部 `DevelopmentProduct` 记录实际程序集位置、完整部署文件集合和 SHA-256，通过 AtomicFile 发布。锁文件不属于应用 payload。不扫描 SDK 或将当前源码误认为已编译。
- 准备记录缺失、部署文件缺失/损坏/增加时进入独占准备；等锁后再次检查。成功构建且部署稳定才能记录，随后在共享 lease 下重新验证。失败/取消不登记可运行产品。
- 修改引擎/backend 源码后明确 Rebuild。游戏脚本与资产继续由 Editor 原流程导入/reload。
- 缺省 Rebuild 成功后记录所选配置的 Editor。Debug/Release 分别隔离。`engine`/`rebuild` 构建产品后返回，删除自动发布 Release Player Support Pack 的后续阶段。
- Entry 仍只依赖 Core.IO/BCL；共同领域、平台、backend 不反向依赖 entry。没有新增生产 Program、测试后门或旧路径 fallback。
- API 元数据检查发现平台 Editor 的生成目录公开使用 `SerializationTypeMetadata` 与 `ITypeCatalogRegistrar`。Windows/macOS 产品增加对应 Foundation 的公开引用组，其余组合引用仍为实现依赖。没有手工修改生成代码或扩大业务类型可见性。
- 外国产品的手写源码审计没有完整生成 API，因此只有取得编译元数据时才判断公开引用“未使用”；源码模式继续检查真实手写签名及非法依赖。完整生成表面需在所属平台编译后检查。

## 工作区与发布边界

```text
backends/<component>/build/<Toolchain>/obj/native/<target>/<fingerprint>/
  原生源码固定副本、生成工程与中间态的实际 owner

%TEMP%/InnoTools/<identity>/
  Windows junction，指向上述目录；避免 SDK 工具的路径长度限制

artifacts/native/<component>/<target>/<fingerprint>/Outputs/
  校验完成、可复用的 Native 产物

platforms/<platform>/editor/<Editor>/bin/<target>/<configuration>/<framework>/
  可直接运行的 Editor managed/native 部署

artifacts/entry/<target>/<configuration>/editor.json
  已准备产品位置和内容哈希，内部构建状态
```

检查实际 junction 的 LinkTarget，确认它们指向当前 checkout 的 backend obj/native。没有将 Native 产物另存一份到用户全局库缓存。正常结束只移除 alias；中断可能遗留 alias，锁文件保留。当前协议使用 obj/artifacts，`.libs` 不属于现有协议。

发布模板生成完整 Player Exe，消费 References、Analyzers、游戏部署定义与 Native 文件。Player 的 Program 进入 PlayerApplication，不调用 DevelopmentEntry、BuildApplication、MSBuild 或 BGCS。作者发布时仍有托管/内容编译，冷 Native 或 AOT 也需要构建时间；最终用户运行不启动这些编译器。框架依赖 CoreCLR 的运行时要求与构建 SDK 分别说明。

## 验证状态

证据目录：`artifacts/acceptance/editor-prepared-startup/`。

环境为 Windows x64、.NET SDK 9.0.318；HEAD 为 `8070302d0daa7ea19b2211b224c2bd6cb4dca1b9`，包含未提交工作区改动。现有改动保留，本轮不修改 tests、BGCS 或 extern，不自动提交。

- Entry Debug 编译：通过，0 警告/错误，5.52 秒。
- 当前 Windows 产品准备与 8 帧 smoke：通过，exit 0，951.503 秒；SDK 构建 15:35.16，Native 阶段无 C++ 进程。此数字属于准备成本，不是日常启动。
- 同一完整部署的两次直接启动：通过，5.928 / 5.937 秒，均完成 8 帧、退出保存与 Dispose，exit 0。日志没有 Preparing/Task/Native/Shader 构建消息。
- 缺省完整 Rebuild：通过，exit 0，1429.603 秒（23 分 49.60 秒）；最终 Editor SDK Build 为 12:34.43，0 警告/错误，并记录 `Editor prepared for direct startup (windows-x64/Debug)`。没有自动执行 Release Player Support Pack 发布。
- Rebuild 后立即直接启动：通过，10.100 秒；再次启动 5.852 秒。两次均完成 8 帧、退出保存与 Dispose，exit 0，构建消息计数为 0。首次 Host 初始化约 7.55 秒；这些数字包含初始化、帧运行与退出，不能描述成窗口瞬时出现。
- 公开引用修正后的 Windows 产品准备与 8 帧 smoke：通过，796.770 秒，SDK Build 为 13:03.27，0 警告/错误。仅失效本轮准备记录，由正式入口重新构建、登记部署并运行；此项是准备验证，不能算作普通启动耗时。
- 最终普通 `dotnet run`：通过，11.144 秒，包含入口增量构建、8 帧与正常退出，exit 0，产品/Task/Native/Shader 构建消息计数 0。当前完整产品准备记录保留，下一次普通启动直接复用。
- 文档链接：通过，检查 2,065 个相对链接，缺失 0；`git diff --check` 通过。
- SourceDebug：通过，725.368 秒，0 警告/错误，包含共享源码和测试项目编译；本轮没有修改或全量执行测试。
- 当前架构门禁：复验通过，exit 0；有效 MSBuild 图覆盖 232/232。首次检查未通过的记录保留：Rebuild 清理后的中立源码审计产物缺失，另发现生成注册 API 的公开依赖未直接声明。补齐 SourceDebug 并修正引用/源码审计边界后通过，不以缺失产物跳过检查。
- macOS/Linux、手动目录选择、真实焦点/UI 和游戏发布矩阵：本轮未执行；不以启动测试替代。

历史约 847 秒的数据描述修复前路径，见[原验收报告](DEVELOPMENT_ENTRY_ACCEPTANCE_2026_10_10.md)，不能用于声称修复后的启动性能。

### 实际命令与证据

以下命令从仓库根目录运行；使用已编译的入口，避免把入口自身的 IDE 增量编译混入产品启动测量。

```powershell
dotnet build 'entry/Editor/InnoEngine Editor.csproj' --configuration Debug --no-restore --disable-build-servers -m:1 -nodeReuse:false -v:quiet
dotnet build 'entry/Rebuild/InnoEngine Rebuild.csproj' --configuration Debug --no-restore --disable-build-servers -m:1 -nodeReuse:false -v:quiet
dotnet entry/Rebuild/bin/Debug/net9.0/InnoEngine.Rebuild.dll
dotnet entry/Editor/bin/Debug/net9.0/InnoEngine.Editor.dll C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird --graphics-api d3d11 --smoke-frames 8
dotnet build InnoEngine.sln --configuration SourceDebug --disable-build-servers -m:1 -nodeReuse:false -v:minimal
dotnet entry/Rebuild/bin/Debug/net9.0/InnoEngine.Rebuild.dll verify . --dotnet 'C:/Program Files/dotnet/dotnet.exe' --project-graph artifacts/acceptance/editor-prepared-startup/results/project-graph-final.json
dotnet run --project 'entry/Editor/InnoEngine Editor.csproj' --configuration Debug -- C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird --graphics-api d3d11 --smoke-frames 8
```

`results/rebuild.json` 与 `logs/rebuild.log` 记录完整 Rebuild；`results/after-rebuild-startup.json`、`results/after-rebuild-repeat.json` 和对应 smoke 日志记录紧随其后的启动。`results/direct-startup.json` 记录此前两次热启动。`results/native-workspace-aliases.json` 记录实际 junction 目标，`results/environment.json` 记录环境。`file-map.tsv` 记录本轮改动职责。
`results/source-debug.json` 与 `results/architecture-final.json` 记录源码编译及最终架构门禁，`results/project-graph-final.json` 保存有效项目图。首次未通过的 `results/architecture.json` 与对应日志保留。
`results/final-product-prepare.json` 与 `results/ordinary-run.json` 记录最终产品编译和普通运行，`results/final-checks.json`、`results/document-links.json`、`results/final-state.json` 记录差异、链接、产物及磁盘检查。SDK 清理仅处理所属可重建产物；保留当前 Native 缓存、产品、中立审计程序集与验收日志，没有清理用户、NuGet 或 SDK 全局缓存。

### 成本与未实测边界

本轮消除的是普通启动中的产品 Build 和跨配置 Support Pack 准备；完整清理重建仍有明显成本，不能声称整个构建体系已经达到秒级。Native 缓存命中不启动 C++ 编译器，但 Rebuild 仍执行必要输入/输出验证、工具准备和托管编译。

本轮 smoke 不替代真实 Play、视觉、焦点、多 DPI 或发布游戏实测。发布程序不调用编译器的结论来自当前 Player 入口与发布模板源码检查，本轮没有重新执行全部发布矩阵。Windows 无法访问 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。

建议提交标题：`fix(entry): launch prepared Editor without rebuilding`。不自动提交。
