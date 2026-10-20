# BGCS 接入、架构与基础能力复用审计

日期：2026-10-21。范围为当前四仓库工作区；本页不将架构门禁视为全源码行为证明。

[阶段验收](NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md) · [当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [Identity 强制标准](../../../docs/architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

## 对三个问题的回答

| 问题 | 当前结论 |
| --- | --- |
| BGCS 是否干净、未修改其源码、接入解耦？ | **是，当前证据支持。** 使用公共 NuGet 3.0.1；没有 BindGen-CS 工程引用，其工作区无修改。生成器接入归属于 Build.Bindings，中立构建只消费接口和冻结编译描述。必要的 BGCS.Runtime 互操作依赖仍存在于 Native/ImGui 实现边界。 |
| 整体架构是否干净？ | **主分层和依赖方向清楚，已有门禁通过；不能宣称没有任何缺口。** 233 工程门禁未发现规则覆盖范围内的违规。本次语义审计发现插件边界 R01/R02，以及验收入口范围不一致 R03。 |
| 是否存在严重耦合或重复基础系统？ | 在检查的生成器、平台、事件、身份及持久化路径中，未发现第二套完整 EventBus、插件资产数据库或序列化框架。Canvas 已复用 Core Events。Rendering2D 仍有未统一的局部事件通知；Canvas 跨帧对象引用也有收口空间。不能由检索结果推导四仓库所有业务逻辑都没有重复。 |

## BGCS 的可核验证据

- `Inno.Packages.props` 精确指定 3.0.1；当前 446 个 BGCS 包族锁文件解析项全部为该版本。
- 本次只读扫描了四仓库 240 个当前工程文件，没有指向 BindGen-CS 的 ProjectReference。该数量是 XML 扫描范围，不替代引擎 233 工程的有效 MSBuild 门禁。
- BGCS checkout 当前 HEAD 为 `e619418`，`git status --porcelain` 为空；本轮没有编辑该仓库。
- BGCS、BGCS.Contracts、BGCS.Runtime 的 `.nupkg.metadata` 来源均为 `https://api.nuget.org/v3/index.json`。使用所选 SDK 的公开 `NuGet.Packaging.PackageArchiveReader.GetContentHash` 核对标准内容哈希，与 NuGet metadata 一致。
- 原始 nupkg 的 SHA512 与各自 `.nupkg.sha512` 一致；实际安装内容逐文件与包内比对，分别 92、5、5 个文件一致，没有缺失或修改的运行文件。三个未解压的 ZIP 容器 metadata 项单列，不当成 payload 篡改。签名容器的原始哈希与 NuGet 标准内容哈希采用不同算法语义，不能直接比较二者后判定损坏。
- 已通过正式生成后比对七组件 HostSource，以及七组件 TargetArtifacts 的 GenerateBindings / CheckBindings。UI 输入清单由 SDK 正式重建；没有通过修改生成文本消除差异。

原始证据位于 `artifacts/acceptance/bgcs301-2026-10-20/`：

| 文件 | 证明范围 |
| --- | --- |
| `architecture-boundary-audit-2026-10-21.json` | checkout 状态、工程引用、包声明与审计范围。 |
| `bgcs-installed-package-integrity-final-2026-10-21.json` | 安装内容与包载荷比对。 |
| `bgcs-nuget-canonical-hash-2026-10-21.json` | 通过 SDK 公共 NuGet API 取得标准内容哈希。 |
| `final-bgcs-package-lock-audit-02.json` | 446 项包族锁定。 |
| `check-bindings-reconciled-01.json` | 正式生成与比对结果；失败记录和复验保留。 |
| `bgcs-published-runtime-capability-final-2026-10-21.json` | 正式二进制公开能力查询；Browser 与 ARM64 CoreCLR 未实现 aggregate profile，不能替代设备执行。 |

## 接入职责与文件树

```text
build/
├── msbuild/Inno.Packages.props                     精确包版本
├── bindings/Inno.Build.Bindings/                   唯一生成器应用边界
│   ├── Configuration/BindingConfigurationPaths.cs 固定实际 SDK 的编译器/路径输入
│   ├── Generation/BindingHostFactory.cs            SDK 公开 Host/Worker 创建
│   ├── Generation/BindingHostExecution.cs          GenerateAsync 与正式 manifest 消费
│   ├── Generation/BindingNativeCompilation.cs      SDK manifest → 中立编译描述
│   ├── Identity/                                  工具/配置/输入身份
│   └── Output/                                    完整性、诊断与候选发布
├── toolchains/Inno.Build.Toolchains/
│   ├── Native/INativeBindingGenerator.cs           中立可替换接口
│   ├── Native/NativeBindingCompilation.cs          冻结文件、include、define 与 exports
│   ├── Native/NativeCMakeExecutor.cs               消费已选工具与复制树
│   └── NativeBuildContext.cs                       操作、owner、目标和输入作用域
└── distributions/Inno.Build.Distribution.Standard/ 平台与部署组合

platforms/<platform>/build/                         各自实际系统 SDK/toolchain
backends/<component>/
├── native/Inno.Native.*/Native/                    手写窄 facade / SDK 生成 Native
├── native/Inno.Native.*/Bindings/                  SDK 配置与扩展
├── native/Inno.Native.*/Generated/Bindings.cs       SDK 生成 managed 单文件
├── runtime/                                       具体互操作与后端实现
└── build/                                         组件 recipe 与 owned intermediates
```

Build.Bindings 依赖 SDK，Toolchains 不引用 BGCS；它通过 `INativeBindingGenerator`、`NativeBindingCompilation` 等中立契约消费结果。平台提供实际 SDK 和不可变 invocation，Binding 应用把 SDK manifest 转为引擎冻结输入；这属于两套工具之间必要的适配，不是另写解析器或 ABI 推断器。

BGCS.Runtime 的地址、模块会话和 UTF-8 lease 被 Native/ImGui 实现使用；Editor 的 ImGui presentation 本来就依赖该具体技术。脚本和后端中立 Rendering API 不暴露 BGCS/BGFX handle，也没有把生成器引入 Runtime 服务。更换整个 ImGui presentation 仍有实际工程成本，不能声称所有实现都可零成本互换。

## Core 复用结果

| 路径 | 当前实现与判断 |
| --- | --- |
| Runtime Session / 输入 | Session 使用 Core EventDispatcher；EventInputSource 使用同一 Core 实现隔离输入 scope。没有额外通用总线。 |
| Audio / Animation | 运行服务注入 Core EventDispatcher，分别发布领域事件。领域事件类型和播放 handle 是必要协议，不是重复的事件系统。 |
| Canvas 交互 | `Canvas` 自己创建 Core dispatcher/hub；Listen、Enqueue、Flush、DiscardPending 和 hub.Dispose 都复用 Core。每个文档拥有独立 scope 是隔离需求。 |
| Rendering2D 帧事件 | `SpriteAnimator2D.frameEvent` 仍为直接 C# delegate 通知，未走 Core；见 R01。 |
| 领域索引与资源缓存 | Sprite region ID、GPU handle、chunk revision、几何/纹理增量缓存有各自语义，不应机械替换为 object Identity 或删除。 |
| Identity | 没有发现插件另建全局对象身份管理器；Canvas 跨帧状态仍保存 live owner，见 R02。 |
| 结构化持久化 | 检查的插件 Assets 使用引擎序列化属性与稳定类型协议，未发现插件自己的 JSON 持久化旁路。Build 的缓存 manifest 是工具私有派生产物，不是 Project/Scene 的第二格式。 |

单个服务的生命周期通知、清理回调列表、Native 到 managed 的事件 transport 和通用发布订阅系统职责不同。不能仅因出现 `event`、`Action`、Dictionary 或 Queue 就断言重复造轮子；应核对订阅、分发、线程、释放和 generation 语义。

## 当前需要明确处理的边界点

### R01：插件动画帧事件尚未统一 Core

位置：[SpriteAnimator2D.cs](../../../../InnoEngine.Rendering2D/Assets/Runtime/Components/SpriteAnimator2D.cs)，公开 `frameEvent` 和 `ApplyFrame`。

当前用 `event Action<string>` 直接订阅与调用。它没有第二套事件队列或管理器，因此不应夸大为完整 Event System 重复实现；但它未复用 Core 的订阅 token、hub scope 与销毁释放协议，和第 22 节要求的插件事件统一方向有差距。检索当前四仓库业务源码没有发现外部订阅该成员的调用方；这不证明不存在其他消费者。

理想收口：领域只定义中立帧事件和最小订阅入口，实际分发及 token 生命周期交给 Core；销毁、重入、异常和 generation 退休通过公开边界验证。不得新增平行 listener 字典，也不得同时保留两条分发路径。

本轮原计划要求保持公开签名；删除或替换该公开事件会改变此边界。本次先如实报告，没有悄悄重定义 API，也没有用兼容包装把两种协议并存。

### R02：Canvas 跨帧状态仍直接持有 owner

位置：[CanvasViewContentSource.cs](../../../../InnoEngine.Canvas/Assets/Runtime/Rendering/CanvasViewContentSource.cs)，`m_states`、`CanvasState.owner` 与 `CompleteFrame`。

字典已使用 Core RuntimeIdentity，但 value 仍保存 `Canvas` 强引用；跨帧操作通过该对象调用 `PublishEvents`，没有在实际使用边界由原 Identity scope 重新解析。`Collect` 中会检查 destroyed/runtime identity，provider.Dispose 也会清空状态，这些保护存在；它们不等于每个后续回调都经过 Identity。按 Identity 标准，这里仍应收口。

理想收口：保留明确的 UI/GPU 资源 owner，live Canvas 的寻址保存原 scope 的弱 Identity 快照，在后续使用时解析和校验；对象失效后退休相应状态，不建立第二个 resolver。验证销毁发生在收集/回调之间、组件替换、禁用、场景卸载及插件退休，并确认 UI/GPU 资源只释放一次。

这是源码规则审计发现，不是已经复现的泄漏、崩溃或 ALC 卸载失败。当前短运行通过不能替代这些边界用例。后续修正必须同步插件源、正式导出安装包、Samples、测试和设备证据，不能只改一份缓存或包内文件。

### R03：Native 全套验收入口的产品范围不一致

位置：[NativeBindingsVerification.cs](../../../build/cli/Inno.Build.Cli/NativeBindingsVerification.cs)，`DiscoverBindingProjects` 与 `BuildNativeDependenciesAsync`。

绑定项目发现会在目标没有 Editor 时选择 Player，但后续原生产品准备固定请求 `editor`，同时测试执行是宿主 VSTest/动态 Native 路径。因此不能把当前 `verify-native --target browser-wasm` 描述成已实现的 Browser 全套验收；本轮也没有执行并宣称该命令通过。正式 `bindings`、Support Pack 和 Browser 发布是独立的实际产品流程，Browser 当前实际失败来自前述 SDK ABI 能力。

理想边界：明确该入口的宿主 Native/Editor 测试能力，在任何生成前通过发行声明预检并拒绝不适用产品；Browser 的静态链接和网页运行使用真实 Player 验收入口。或者完整提供中立验证计划与可替换目标 runner；不能只把字符串改成 player 后仍用桌面 testhost 执行 Wasm。不要把命名平台 switch 或第二套组件名单放进共同工具链。

这是验收应用的目标/执行器范围问题，不是核心 Runtime 反向依赖平台。当前 Windows 七组件生成、完整构建和测试分别已有证据，不能把这些分项证据拼成整个 `verify-native` 已通过。

## 判定

BGCS 接入没有发现改源码、源码依赖入口或生成器反向侵入领域层的行为。整体架构有清楚的 owner、分层和扩展边界；已有实际构建、生命周期测试和架构门禁提供支持。

R01/R02/R03、现有 XML 语义质量、完整热构建成本和设备验收仍需按范围处理。它们不应被“架构已无敌优秀”覆盖，也不能无证据归因为 .NET 10 或 BGCS 升级引入的问题。
