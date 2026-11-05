# 标准 NuGet 还原验收

本轮于 2026-10-26 至 2026-10-27 在 Windows x64 执行，将源码工程恢复为标准 `.csproj` / `PackageReference` 和 SDK Restore。SDK 10.0.402、C# 14、BGCS 4.1.2 保持不变。

## 范围

- 删除引擎 726 份及 Rendering2D 测试工程 1 份依赖清单；修改前工作区均无已有变更，逐文件核对 SHA-256 后退出，原始字节在仓库外验收目录归档。
- 删除全局生成开关、命名 targets、Editor 清单宿主上下文、锁检查脚本、过时专项处置表及其正常流程消费者。
- CI 直接调用 SDK Restore；托管验证继续检查 tracked 和 untracked 源码变化。
- 输入冻结继续消费实际 assets、包文件、reference pack、项目和编译输入；依赖清单不再参与身份或复制。
- BGCS SDK、NuGet 包内容、第三方源码和生成绑定均不属于手工修改范围。
- Core IO lease、Task 还原与编译的求值隔离、取消、输入校验和原子发布保持。

## 已执行验证

| 范围 | 结果与证据 |
| --- | --- |
| 全 Solution SourceDebug 普通 Restore | 通过；`source-restore.log` / binlog |
| Editor Debug / Release 普通 Restore | 均通过；两个正式入口请求，不使用额外依赖清单 |
| Windows CoreCLR / NativeAOT Restore | 均通过；真实 `win-x64` 与 SDK AOT 属性 |
| Browser Mono / Wasm AOT Restore | 均通过；真实 `browser-wasm` 与裁剪 / AOT 属性 |
| Rendering2D 测试工程 Restore | 通过；独立 ArtifactsPath，没有新增依赖清单 |
| 独立干净源码 Restore | 通过；3,021 份当前源码文件，开始前不带 bin / obj / artifacts；没有复制 NuGet 锁清单 |
| 干净目录 Core.IO 直接 Build | 通过；正常 SDK 命令，零警告、零错误 |
| Build 开发入口与构建测试工程 | 通过；零警告、零错误 |
| 相关普通测试 | 30 项通过，零失败、零跳过；`final-tests/shared/*.trx` |
| 输入冻结独立进程测试 | 1 项通过，零失败、零跳过；`final-tests/isolated/*.trx` |
| 架构与真实 SDK 工程图 | 238/238 通过；`architecture-final.log` / `project-graph.json` |
| XML / CI YAML / 当前 Wiki 文件链接 | 277 份 XML、3 份 YAML 及当前文档链接通过；`source-validation.json` |
| Editor Debug 实际构建 | 通过；顶层零警告、零错误；`editor-debug.log` / binlog |
| 固定源码 Editor 热构建 | 通过；200.718 秒，Native processes=0、bindingGenerations=0、materializedBytes=0；`editor-hot.log` / binlog |
| FlappyBird Editor 有界启动 | D3D11、sRGB、300 帧、资源释放完成、exit 0；`flappy-editor.*` |
| 依赖清单残留 | 正常源码目录 0，生成规则和正常流程消费者 0；没有新增 ignore 规则 |
| BGCS 完整性 | 仓库无修改；核对的 130 个包文件 SHA-256 全部未变；没有修改 extern 或生成绑定 |

测试覆盖标准 RID/AOT/裁剪解析、宿主产品引用、编译器工具输出 owner、精确 BGCS 声明、SDK 包身份、输入失效，以及 owned Restore / compilation 的求值隔离。没有减少生命周期保护或放宽生产校验。

本地直接构建测试工程使用 SourceDebug 输出。普通套件排除 `ProcessIsolation=Required`，该项通过 SDK 求值取得最终程序集后在单独测试进程中执行；不能把只执行普通套件当作隔离用例已执行。

首次包身份回归错误地将完整签名 ZIP 的 SHA-512 与 NuGet 内容身份直接比较。已改为分别校验存档与 `.nupkg.sha512`、assets 与 `.nupkg.metadata`；两种哈希语义依据 [NuGet 官方说明](https://github.com/NuGet/Home/wiki/Nupkg-Metadata-File)。初始失败日志保留，最终源码的 31 项回归全部重新通过，没有修改包或屏蔽断言。

第一次实际 Editor 构建因输入契约变化进行一次 Native 冷准备，共 18 个原生进程、7 次绑定生成；耗时 39 分 58.93 秒。热构建复用完整产物，没有再次原生编译或生成。上述计时不是受控前后性能对照，不据此宣称提速。SDK 的既有安全事实诊断和第三方构建输出保留在日志中，没有全局忽略。

## 缓存维护

使用正式 `clean --scope cache --configuration Debug --cache-time-seconds 120` 完成维护，保留现有 64 GiB / 20 GiB 默认策略。按 owner 协议逻辑退休 4,636,545,432 字节，protected 174,384,757 字节，skipped 0；C 盘实际可用空间增加 3,232,452,608 字节。逻辑字节与实际卷空间分别记录于 `cache-clean-final.log` / JSON。

未清理 SDK / NuGet 全局缓存、未知目录或用户内容。运行协调锁继续位于原有输出 owner；它们不属于依赖清单。

## 深层架构自查

- 从工程求值、Restore、Task 引导、输入冻结到 Native 准备检查真实调用链：只退出额外的依赖锁策略，SDK 仍是唯一包解析者。
- 包与 reference pack 的真实内容继续进入不可变输入；PackageReference 变更仍使快照失效，不用缺少清单来跳过身份校验。
- 删除命名规则、入口附加上下文、专用脚本和失效处置表，没有集中搬家、兼容读取、别名或第二份手工注册表。
- 平台事实与宿主路由没有下放到共享层；Windows / macOS 路由通过真实 MSBuild 求值测试，实际架构图通过。
- 继续复用 Core IO 的路径与 lease 能力，没有新增解析器、锁管理器、事件系统或测试后门。
- 没有新增 public 产品 API、global using、implicit using 或 metadata using 注入；游戏 API、Stable ID 和序列化形状未改变。

本轮范围内达到源码与职责清楚的要求；不将其表述为整个引擎已无任何问题。

## 未执行与限制

- 本轮没有重新运行 macOS 实机、Browser 产品执行、Editor Release 完整构建或 Windows 全部发布矩阵；相应 Restore 与静态图不替代产品运行。
- FlappyBird 验证为隐藏窗口的 Editor 启动、帧推进和退出，不代表已经验收 Play 玩法、视觉、焦点、DPI、音频或长期运行。
- CI 已改为普通 SDK Restore 并通过本地语法检查；没有推送或触发新的远端 CI。
- 当前选择不再冻结未来所有传递依赖和包源状态；源码包声明、SDK 基线和实际还原输入继续可验证。
- Windows 无 `/System/Library/Sounds/Glass.aiff`，未播放指定提示音。

## 证据位置

`D:/InnoEngine-Verification/nuget-standard-2026-10-26` 保存修改前清单、SHA-256、原始归档、SDK 信息、构建日志、binlog、TRX 和最终完整性结果。归档不参与生产构建，不是依赖注册表或兼容读取路径。历史记录中已经清理或归档到仓库外的原始产物链接单独记录，不视为当前 Wiki 页面缺失。

变更提交至当前分支，author / committer 均为 `Aaron Liao <hsuankailiao@gmail.com>`，无 co-author；不推送。
