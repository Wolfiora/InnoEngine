# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 工具链与边界

SDK 为 **10.0.402**，`latestPatch`，禁用预览 SDK；运行目标为 `net10.0` / `net10.0-browser`，语言为 **C# 14**。两个 Roslyn 生成器保留 `netstandard2.0` 编译器工具产物。源码及脚本逐文件声明普通 using，禁止 global using、implicit using、MSBuild `Using` item 和 metadata 隐式导入。

BGCS 包族精确固定 **4.1.0**，通过正式 NuGet 包及官方公开 API 使用。平台拥有 SDK、ABI、Profile、链接和产品布局；组件拥有公共绑定语义与 recipe。开发入口只有 `InnoEngine.Editor` 和 `InnoEngine.Build`；Editor 启动期间不构建引擎源码。

当前代码及命令、日志、binlog、TRX、包哈希和文件树见 [BGCS 消费验收](../../records/architecture/acceptance/BGCS410_CONSUMER_ACCEPTANCE_2026_10_24.md)。本地证据位于 `artifacts/acceptance/bgcs4-2026-10-24/`，不进入 Git，也不是产品依赖。

## 当前取得的结果

| 检查 | 当前状态 |
| --- | --- |
| 源码全集 SourceDebug | 最终源码通过，0 警告、0 错误，包含配置测试的物化契约修正。 |
| 正式 Build 入口 | 通过，内容身份修改后的构建为 0 警告、0 错误。 |
| BGCS 使用边界 | 130 个包文件哈希不变；BGCS 源码工作区干净，递归检查无 Agent 类 Markdown 文件。 |
| 绑定相关回归 | 内容身份修改后的 27 项通过，0 失败、0 跳过；独立真实 MSVC 宏隔离回归亦通过，结果分别留存，不重复累计。 |
| 架构 / XML / Wiki | 正式门禁通过，238/238 工程完成 SDK 求值；具体 API 与示例数量见当前报告。 |
| 架构验证器回归 | 144 项通过，0 失败、0 跳过。 |
| Windows 产品 | Editor Debug/Release、CoreCLR Player、NativeAOT Player 的 D3D11/D3D12/Vulkan 共 12 项通过，各推进 3,000 帧并有序退出。具体证据及设备覆盖边界见报告。 |
| 普通 / 隔离完整套件 | 对应工程完整复验后，有效去重结果为 2,141 通过、0 失败、18 项平台跳过；59 项隔离发现、28 次隔离执行全部通过。首轮失败及一次部署不完整的验证命令记录保留，具体替换范围见报告。 |
| 脚本与 IDE | 三个调用方的正式脚本编译、IDE 投影和 Release IDE 编译全部通过。 |
| 十次热构建 | 10/10 通过，原生编译进程、绑定生成和源码复制均为 0，selection 保持稳定。并行负载下总耗时中位数约 191 秒，输入校验成本仍需改进。 |
| 检出字节完整性 | 28 个原生生成文件的 84 次 Git 过滤检查及 21 项生成清单哈希通过；Git 保留 SDK 原始字节。 |
| Browser | 五组件正式生成通过；实际 native 编译受下述 SDK 问题阻塞。 |
| macOS ARM64 | 无设备，未执行实机验收。 |

## 外部限制与证据边界

- **Browser：暂缓。** SDK 将系统头匿名 union 的诊断名称写成非法 C 类型；不引用引擎的精简 SDK 工程已复现。见[复现、职责与继续条件](../../records/architecture/acceptance/BGCS410_ANONYMOUS_SYSTEM_RECORD_REPRO.md)。未修改 SDK、第三方头或生成源，未关闭断言。Mono Wasm / Wasm AOT 产品不计作通过。
- **macOS ARM64：未执行。** Metal、Editor、示例产品、CoreCLR、NativeAOT 和 Unix 文件系统结果不能从 Windows 或 fixture 推断。
- **Rider UI：未执行。** SDK/MSBuild 和 Task 回归与实际点击 Rider Run 分开报告；没有专属 Rider 生产路径。
- 隐藏窗口帧推进不能证明全部视觉、DPI、焦点、输入、音频听感或多小时稳定性。实际设备和已执行操作按产品报告列出。

测试保留生产校验、输入冻结、lease、取消和原子发布，不排除整类 BGCS 测试，不使用 IVT、反射穿透或测试后门。普通执行、隔离发现和隔离执行共用筛选条件。

性能只报告当前条件下的重复采样及明确归因。没有完整改造前同 SDK、同输入、空闲环境的基线时，不宣称整体提速或已排除全部性能回退。

## 相邻证据与持续自查

平台职责及缓存协议的既有证据见[平台与缓存验收](../../records/architecture/acceptance/PLATFORM_OWNERSHIP_CACHE_ACCEPTANCE_2026_10_22.md)；插件 Events / Identity 的专项证据见[插件复验](../../records/architecture/acceptance/PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md)。它们只证明各自记录的代码状态和范围，不能替代当前包与产品的复验。

每次交付都沿实际请求、依赖、输入、生成、编译、链接、并发、失败和资源释放检查职责边界，并复核 Core IO、Events、Identity、Serialization 的复用。依据为根 `AGENTS.md` 第 30 节；未完成项明确保留，不以“完美”代替证据。
