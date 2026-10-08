# 平台、后端与集成边界整改验收与交接

[架构索引](README.md) · [批准计划](BACKEND_PLATFORM_INTEGRATION_PLAN.md)

## 当前结论

**源码整改完成，完整产品验收待续跑。** 用户在额度仅剩 4% 时明确要求停止 Computer Use，并允许未跑完的验收标注后留待额度重置。本报告不表示四条发布/运行路径、物理 UI 或性能矩阵已全部通过；当前不作“完全验收、可直接提交”的结论。

保留实施前未提交工作区，未自动提交。BGCS 源码、extern 及生成绑定内容没有为本次集成整改手工修改。

## 用户提出的问题与解决结果

| 问题 | 实施结果 |
| --- | --- |
| 平台 BGFX profile 使平台基础模块依赖共享后端 | 四个真实 BGFX integration 程序集拥有 Native/Shader profile 与内容 compiler 工厂；Windows、macOS、Browser、Linux 基础构建项目移除 BGFX 引用及旧配置入口。Standard Distribution 唯一组合。 |
| SDL、ImGui 是否也存在宿主耦合和重复窗口接入 | 三个 SDL integration 注入初始化、窗口、surface、焦点及能力。SDL 应用唯一拥有创建/接管目录；ImGui 复用该接入，删除重复 Win32/Cocoa 解析及 WindowsX64 FramebufferScale 返回 ABI。 |
| Native 组件是否仍隐式适配平台、如何扩展 | 每个 ProductNativeBuildStep 显式声明 Static/Shared、有序 CMake 参数、配置输入及组件 owner；配置全部参与内容指纹，unsupported recipe 在 staging 前失败。没有为 MiniAudio/Text/RmlUi 创建空逐平台项目。 |

## 已完成的架构和公开契约

- `IGameContentCompiler` 与平台 `IGameBuildTarget` 分离；`GameBuildTargetBinding` 构造时拒绝 target 不一致。
- `BuildTargetFactory` 无创作服务参数；`GameContentCompilerFactory` 借用当前 generation；`GameBuildContribution` 是完整发行输入；`CreateBindings` 替代 `CreateTargets`，没有兼容入口。
- `NativeComponentBuildOptions` 冻结链接方式、有序参数和配置文件；拒绝 SDK/组件 ownership 参数覆盖，recipe 参数冲突在工具启动前失败。
- `ISdl3HostIntegration`、`Sdl3HostCapabilities` 隔离 SDL 宿主策略；创建成功后窗口转交应用，外部窗口只登记不接管原生销毁权。
- `ImGuiInteractionOptions` 明确 Command 键与水平滚轮规则；framebuffer ratio 与逻辑 DPI 区分，Monitor 更新在帧安全点进行，保留上一帧 DPI 供 ImGui 自己执行缩放转换。
- 平台 Player 仅引用运行期 SDL integration；BGFX 构建集成不进入运行闭包。Services、Player Runtime、Editor Hosting 不引用具体 integration。

```text
Standard Distribution / 平台产品
├─ 平台基础：target、SDK、布局、packager
├─ backend：领域实现、Native facade、component recipe
└─ integration：真实平台与 backend 配置/调用
   ├─ 对应平台契约
   └─ 对应 backend 契约

共同 Runtime / Build / Editor Hosting → 中立契约
```

新增四个 BGFX 和三个 SDL 集成库，及 SDL Adapter 原生契约测试项目。没有新增生产 Program。完整文件树、移除路径和 API 规则见批准计划及各项目唯一 Wiki；逐文件映射位于 `artifacts/acceptance/backend-platform-integration/file-map.tsv`。

## 环境、命令与已通过结果

- Windows `10.0.26200`，.NET SDK `9.0.318`；Web SDK 位于 `C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe`，含现有 wasm-tools。
- HEAD：`818163b5222632a6bc2cbe92804b0c6b62b8921d`，实际验收对象包含未提交源码。保存 baseline diff/revisions 与 handoff source hashes。
- `dotnet build InnoEngine.sln -m:1 -nodeReuse:false`：Debug 79.484 秒，Release 124.532 秒，均 0 warning / 0 error。Solution 默认批量构建不包含五个平台产品入口，不能替代产品构建。
- Debug 有效 MSBuild 架构验证通过 225/225 项目；最终结构扫描通过，225 项目、142 非空 Solution Folder，无游离源码、空物理目录或重复生产入口。只有六个生产 Program。
- Markdown 本地链接：InnoEngine 252 页、BGCS 54 页、Canvas 2 页、Rendering2D 1 页，共 309 页通过。

所有测试通过真实公开边界，没有测试后门、反射穿透或 InternalsVisibleTo。命令与耗时见 `results/contract-executions.json`、`results/remaining-gates.json`；以下 TRX 为本次最终已完成门禁。

| 测试报告 | 通过 | 失败 | 未执行 |
| --- | ---: | ---: | ---: |
| `build-final` | 147 | 0 | 0 |
| `Inno.Tooling.Architecture.Tests` | 86 | 0 | 0 |
| `Inno.Editor.Hosting.Tests` | 6 | 0 | 0 |
| `Inno.Rendering.Assets.Authoring.Tests` | 33 | 0 | 0 |
| `Inno.Rendering.Shaders.Tests` | 112 | 0 | 16 |
| `Inno.Input.Tests` | 18 | 0 | 0 |
| `Inno.Editor.PlayMode.Tests` | 54 | 0 | 0 |
| `Inno.Editor.Interactions.Tests` | 99 | 0 | 0 |
| `Inno.Extensibility.Reload.Tests` | 22 | 0 | 0 |
| `Inno.Runtime.Tests` | 61 | 0 | 0 |
| `Inno.Rendering.Runtime.Tests` | 86 | 0 | 0 |
| `Inno.Rendering.Tests` | 49 | 0 | 0 |
| `sdl-host` | 7 | 0 | 0 |

Shader 的 16 个案例由当前 fixture 的条件发现跳过，不记作通过。之前 ImGui 34/34 与 SDL 7/7 原生窗口回归通过；**最后的逻辑 DPI 转换补充后，35 项 ImGui 最终重验尚未完成**，不得将旧 34 项结果冒充最终结果。

早期 `build-all.trx` 的 1 项失败来自测试 fixture 未显式注入 Shared 请求；已同步 fixture，最终 `build-final.trx` 147/147。早期发布因冻结后修改输入被稳定性检查正确拒绝；这些失败/中断日志保留，不计作成功产品验收。

## 明确待验收项目

| 门禁 | 状态 |
| --- | --- |
| 最终 ImGui 35 项、多视口/DPI 重验 | 原生准备期间按用户要求中断，待续跑 |
| Release 有效 MSBuild 架构图 | 检查进程中断；Release 编译已通过，架构图仍需独立完成 |
| Windows CoreCLR FlappyBird 发布与实际运行 | 新冻结源码冷构建未完成，待续跑 |
| Windows NativeAOT FlappyBird | 待发布、Native 调用和实际运行 |
| Web 解释执行 / Web AOT | 待发布、链接、输入、光照/星光/音频/存储实际运行 |
| Windows Editor Debug / Release 产品 Build 与 UI | Solution 编译不能替代；产品重验、Play、浮动 GameView、Modal/重叠、DPI/小地图待完成 |
| Canvas / Rendering2D 消费和 GPU | 待续跑 |
| BGFX Adapter 原生设备与 Shader 测试 | 待续跑 |
| 准备构建及三次热构建 | 待测工具进程数、扫描字节、耗时、部署次数；不宣称性能提升 |
| macOS / Linux SDK 与实机 | 本机只能检查源码/组合；macOS Native/Editor/Player 未实测，Linux 不宣称完整 Player |

Computer Use 已按用户要求停止，本轮只完成了技能初始化/窗口枚举，未执行 Editor UI 输入；不能把它计为 UI 实机验收。

## 所有权与失败边界

compiler 借用创作服务；Pipeline 固定 generation，失败时取消并完成关联工作，不接管 catalog/serialization。产品 Native 计划拥有 frozen request 与工具选择，staging 完整验证后提交；强制中断不能把候选标为有效发布，后续重复完整性校验并重建。

SDL 应用借用不可变 host、拥有自己创建的窗口；外部窗口先失效 wrapper/解除登记，再由原 owner 销毁。ImGui context 在 SDL 应用停止前注销 callback、释放 viewport/renderer；不另建事件总线。持久身份、Core Events、Play 隔离与 Reload GC barrier 保持现有协议。

## 扩展方式

新图形 backend 实现 `IGameContentCompiler` 与自己的 component recipe，必要时增加真实 integration；发行替换 compiler/Native plan，平台 packager 保持不变。WindowsX86 增加明确 target、ABI/SDK 支持及必要 integration 配置，复用 Windows 入口，实测后注册。NS/iOS 只有取得真实 SDK 接入时建平台和 integration，不创建空项目。Browser 新托管运行时替换 managed compiler/link 接入，不改共同玩法、输入或领域机制。

## 续跑与交付

交接清单与可直接执行命令：[续跑清单](BACKEND_PLATFORM_INTEGRATION_HANDOFF.md)。验收日志、停止的确切进程和磁盘记录保存在本次 artifacts 目录。正在运行的本任务长验收均已停止，未启动新的后台验收或自动化；额度重置后由用户发起继续。

仅移除本轮临时 junction 的清理动作被自动审批审核以通用 blocked-by-policy 原因拒绝，未执行删除；没有清理用户或 SDK 全局缓存。当前 C 盘约 84.8 GB 可用，保留 Native 中间缓存用于后续重验。

建议 commit title：`refactor(integration): separate platform modules from shared backends`。当前保留未提交改动；建议完成上述必需验收后提交。Windows 不存在指定 `Glass.aiff`，提示音未播放。
