# 开发入口、构建缓存与全量 Wiki 整改执行规格

[架构记录](../README.md) · [当前执行结果](../acceptance/BUILD_ENTRY_CACHE_WIKI_REFINEMENT_ACCEPTANCE.md)

本记录落实用户批准的 B01–B08、W01–W07 计划。这里定义完成条件，不能作为已实现或已验收的证明。各文件的实际状态以本轮 file-map、源码与验收记录为准。

## 边界

- 保留 Native、Adapter、Toolchains 的职责命名，不统一改成 Backends 程序集名。
- 两个轻量开发入口为 InnoEngine.Editor / InnoEngine.Build；不增加其他生产 Program。
- Foundation、Content、Services、Runtime 保持中立；平台基础、共享 backend、integration 与产品组合各有唯一职责。
- 不改 extern、独立 BGCS 源码、生成绑定、目标框架或持久身份。
- 不保留旧入口、旧命令、旧缓存读取、兼容包装或 schema version。
- 保留原工作区修改，不自动提交。

## 源码职责树

```text
entry/
├─ InnoEngine.Editor/                  当前宿主路由与准备产物启动
├─ InnoEngine.Build/                   默认增量构建及显式 clean
└─ Inno.Entry.Bootstrap/               轻量依赖准备、产品锁、readiness 验证
build/
├─ cli/Inno.Build.Cli/
│  ├─ BuildApplication.cs             入口、命令结果
│  ├─ BuildCommandLine.cs             严格参数解析
│  ├─ Commands/                       editor、clean、support-pack、bindings、shader
│  └─ Cleanup/                        产品清理计划与执行
├─ toolchains/Inno.Build.Toolchains/
│  ├─ Native/                         精确输入、workspace、发布、结果与读取生命周期
│  ├─ Managed/                        实际托管准备输入
│  └─ Cache/                          owner、保护根、lease、预算与退休
├─ support/Inno.Build.SupportPacks.Core/ 准备身份索引、内容验证与原子发布
├─ pipeline/Inno.Build/                Pack lease 与游戏发布消费者
├─ bindings/Inno.Build.Bindings/       唯一定义、生成身份与阶段验证
├─ tasks/
│  ├─ Inno.Build.TaskHosting/          阶段图快照、Build 生命周期复用、私有加载 owner
│  └─ Inno.Build.Tasks/                MSBuild 参数、取消与应用流程适配
├─ msbuild/Inno.Build.Tasks.targets    普通 IDE Build/Publish 接入
├─ composition/Inno.Build.Composition/ 中立组合
└─ distributions/Inno.Build.Distribution.Standard/ 唯一标准发行组合
backends/<component>/
├─ runtime/                           共享领域实现
├─ native/                            facade、唯一定义和生成入口
├─ build/                             组件 recipe、工具要求与 obj/native workspace
└─ tests/                             组件边界回归
platforms/<platform>/
├─ build/                             SDK、目标、布局、Support Pack 与模板
├─ integrations/                      平台与 backend 的真实接入差异
├─ runtime/                           系统服务
├─ editor/                            已实现 Editor 产品组合
└─ player/                            已实现 Player 产品组合
```

树中的 Managed/Cache 等职责必须通过真实实现建立；缺少实现时不能创建空占位项目或把该树写成当前实现清单。

## B01–B08 行为规格

| 编号 | 实施要求 | 通过条件 |
| --- | --- | --- |
| B01 | 入口点号命名；默认 editor；删除 engine/rebuild；无 Project 打开选择目录窗口 | 只有两个日常启动入口，默认无 Clean；参数重复、缺值及非法组合明确失败 |
| B02 | 按实际目标、配置包含链和执行代码声明 recipe | 无关目标不失效；facade、共用执行、工具和参数变化正确失效；同长度同 mtime 修改被哈希发现 |
| B03 | preparationIdentity → contentFingerprint 索引；准备前取得读取保护并校验 | 第二次相同请求不运行 Native compiler、BGCS 或 Player managed Build；损坏、额外文件、取消及并发安全 |
| B04 | 产品/目标/配置限定的清理计划；预检后取得 owner | dry-run 与实际候选一致；活动产品受保护；不删除其他产品、游戏输出或有效缓存 |
| B05 | workspaceIdentity 与 artifactFingerprint 独立 | 小源码修改复用兼容工作目录；相同 bytes 不改 mtime；SDK/ABI/不兼容参数隔离；编译期间变化拒绝发布 |
| B06 | 明确注册 owner、读取 leases、当前产品和 Pack 的依赖保护根 | 活动 reader、依赖 generation 不被删除；独占退休后重新核查；权限、跳过和失败分别报告 |
| B07 | 阶段内一次 ProjectGraph 和输入读取；Build 生命周期成功准备复用 | 锁后、生成后、编译后 fresh 校验；失败不缓存；等待者取消不伤 owner；私有加载保持隔离 |
| B08 | BGFX 专用工具由集成显式贡献并冻结 | backend 无宿主路径猜测；缺工具在 staging 前失败；Browser 保留其静态 CMake 路线 |

### 入口命令

```text
InnoEngine.Build
InnoEngine.Build editor --configuration Release
InnoEngine.Build --clean
InnoEngine.Build clean --scope product --dry-run
InnoEngine.Build clean --scope product
InnoEngine.Build support-pack --target windows-x64
InnoEngine.Build clean --scope cache --dry-run
InnoEngine.Build clean --scope cache
InnoEngine.Build --help
```

以上为批准的最终命令集合；尚未实现的 cache scope 必须明确拒绝，不静默降级。保留 game、plugin、scripts、import-sample、verify、verify-native、bindings 和 shader 能力。

### 产物与所有权

```text
extern/                                         第三方创作源码
backends/<component>/build/<project>/obj/native/
└─ <target>/<workspaceIdentity>/                 独占、可复用的冻结输入工作目录
artifacts/
├─ native/<component>/<target>/<fingerprint>/     已验证不可变产物
├─ bindings/<component>/<target>/<fingerprint>/  已验证绑定
├─ managed/<deployment>/<target>/<fingerprint>/  托管产物
├─ support-packs/<target>/<contentFingerprint>/  发布输入闭包
├─ products/editor/<target>/<configuration>/    产品 readiness 与协调
├─ build-tools/tasks/                            工具准备、publication、私有 load owner
└─ acceptance/build-entry-cache-wiki/<runId>/     本轮证据
```

Editor receipt 必须校验实际部署文件集合与 bytes；有效启动不重新扫描第三方源码。源码变化由显式 Build 应用。首次准备与损坏修复仍完整校验。Temp 仅用于受控短路径别名或 operation 临时输入，不能充当长期成品缓存；不恢复混合职责的 .libs。

缓存预算目标为 64 GiB；空闲低于 20 GiB 触发压力回收；自动回收预算 5 秒。当前产物和活动 reader 优先保护，超预算也不能强制删除。自动清理失败不能把成功构建改成失败；显式清理需报告非零结果和具体路径。不清理用户、NuGet、SDK、浏览器全局缓存，不删除锁文件解锁。

## W01–W07 Wiki 规格

| 编号 | 要求 |
| --- | --- |
| W01 | 每个有效生产和测试项目有唯一权威页；全部页面逐页复核职责、owner、使用与验证入口 |
| W02 | 当前 Wiki 只说明当前契约；历史计划、审查、阶段验收迁入 records，保留原始事实 |
| W03 | 引擎职责不以外部插件产品、仓库路径或特有类型解释；保留引擎自身 Plugin 机制 |
| W04 | public 和外部可扩展 protected 的完整签名、nullable、默认值、约束、行为、失败和生命周期 |
| W05 | 分类索引、相对链接、源码链接、锚点和单一权威页一致 |
| W06 | 示例基于真实 API；宿主和逻辑脚本命名空间分别使用匹配的引用环境，独立编译 |
| W07 | 复用现有语义分析验证项目、API、内容边界、链接、示例；自动检查后仍需人工审阅 |

```text
docs/
├─ architecture/        当前分层、入口、缓存、规范、问题和验收
├─ entry/               两个入口与 Bootstrap
├─ core/ extensibility/ scripting/
├─ assets/ references/ scene/ animation/
├─ input/ storage/ rendering/ audio/ text/ ui/
├─ runtime/ plugins/ editor/
├─ build/               共同机制与发行组合
├─ backends/<component>/ README 与每个 runtime/native/build 项目页
├─ platform/<platform>/ README 与系统、产品、构建、integration 项目页
├─ native/              导航，不复制 backend 的完整 API 页
├─ tooling/             实际验证工具库
└─ testing/             测试项目的能力、命令与环境
records/architecture/
├─ plans/               批准的执行规格
├─ history/             历史事实与旧阶段快照
└─ acceptance/          本轮详细命令、结果、限制与证据位置
```

项目页先说明职责、最小工作流和依赖/初始化，再说明所有权、API、扩展、失败/取消、generation/reload、输入输出和测试。普通中文短段落与小而完整的示例优先，不能只粘贴 XML 或仅说“以源码为准”。不把内部缓存/工作目录类型写成公开稳定契约。

## 阶段门禁

1. 保存工作区、revision、API、项目图、磁盘和性能基线，缺失的原始测量明确标记。
2. 完成入口、参数和清理/readiness owner。
3. 完成精确 recipe、显式工具和 workspace。
4. 完成 Support Pack 准备前缓存与全部消费者读取 leases。
5. 完成统一 owner/依赖保护后启用预算回收，再验证 Task 阶段复用。
6. 完成全量 Wiki、历史分离、示例和架构门禁。
7. 完成 Solution、产品运行、性能和磁盘清理；缺少环境独立标记。

每阶段同步调用方、项目引用、公开 XML、Wiki 与测试；编译影响闭包，执行成功/失败/取消/边界测试，更新 maps 并删除被替换入口。失败不能用兼容层绕过。

## 实测与最终交付

- Windows Editor Debug/Release：普通 Build、准备后启动、目录选择、Play、停止、退出和重开。
- Windows CoreCLR/NativeAOT Player 与 Web 解释/AOT：实际发布、输入、音频、内容、存储、渲染及生命周期。
- 公开 API 消费者：编译、插件导出与脚本消费。
- 视觉/焦点：viewport 拖动与关闭、GameView、Popup/Modal、重叠窗口、颜色/光照/星光、ShaderEditor、reload、Missing、History、Workspace。
- macOS 实机另需对应设备；Linux 仅验证已有 Native 工具，不宣称完整产品支持。
- 同环境记录准备构建、三次热构建、Build 后三次启动、小 Native 改动、两次 Pack 请求、两个并发构建、清理预览与回收。
- 统计时间、实际工具进程、生成、ProjectGraph、哈希读取、复制、Task preparation/publication、部署、磁盘与活动保护。
- 最终报告在文档和对话逐项回答 B/W 状态、位置与扩展方式、owner/API 变化、Solution/Wiki 覆盖、实际结果和限制。
- 建议提交标题遵循 type(scope): subject；不自动提交。
