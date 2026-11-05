# 当前架构与提交条件审查

日期：2026-10-12。分支：`kernel`。基准 HEAD：`8070302d0daa7ea19b2211b224c2bd6cb4dca1b9`。

## 用户问题与结论

本轮问题是：当前架构是否清晰、是否仍有妥协耦合、扩展是否方便，以及现在能否提交代码。

结论：共同领域、backend、平台、integration 和产品组合的主边界已经清楚，无须重新推翻总架构。但是当前整批整改尚未完成，存在实际构建一致性与发布失败语义问题，不能把它作为“完整收口并已验收”的版本合入 main。

可以在 kernel 保存明确标注未完成状态的阶段提交。阶段提交应包含各自所需的契约、调用方、项目和文档，不能只提交半边 API。此结论不等于批准当前整批改动用于发布。本轮没有自动提交、重置或修改生产实现；新增审查记录和可重现探针，重新执行了必要验证。

## 审查范围与依据

- 232 个项目的当前有效 MSBuild 求值图，包括共同注入的引用、Compile 项、程序集与产品身份。
- Solution 项目、嵌套分组、有效路径与声明引用。
- Rendering 四层、Runtime/Player/Editor Hosting、开放 provider/catalog、平台集成与标准发行组合的依赖方向。
- SDL 窗口操作、BGFX surface 初始化和退休、ImGui 附加窗口关闭、Shell 退出屏障。
- Native recipe、可复用 workspace、绑定结果读取保护、Support Pack 输入规划与发布、Task 构建作用域复用、开发入口。
- 缓存 owner/依赖/退休实现以及实际调用链是否接入。
- 当前 Wiki 内容边界、页面规模、既有严格门禁结果和本轮计划状态。

有效图检查覆盖全部项目，不能由此推导全部产品已实际编译、全部源码无缺陷或所有平台已经实测。关键执行链采用源码审查和公开边界探针；本轮没有逐行人工审查全部 1,776 个手写生产源码文件。

原始证据：`artifacts/acceptance/build-entry-cache-wiki/20261012-architecture-review/`。

## 需要处理的发现

### F01 — P1：托管输入身份没有成为编译的固定输入

位置：

- [FilePlayerSupportPackPreparation.cs](../../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L188)：实际 `dotnet build` 接收工作区中的原项目。
- 同文件第 210 行：Player 源码仍从工作区项目提取。
- [ManagedBuildInputSnapshot.cs](../../../build/toolchains/Inno.Build.Toolchains/Managed/ManagedBuildInputSnapshot.cs#L115)：校验重新读取当前文件，未交付编译可消费的固定副本。

输入先为 A，编译读到 B，发布前又恢复 A 时，初始及最终内容哈希一致，实际产物仍可能来自 B。仅在编译前后验证内容不能关闭这个执行窗口。

本轮通过真实 .NET SDK 9.0.318 与公开 `ManagedBuildInputSnapshot` 复现：计划源码返回 `first`，编译源码返回 `other`，恢复 `first` 后 `VerifyAsync()` 成功。该探针验证的是实际 SDK 编译与输入快照机制，不是完整 Windows Player 导出矩阵。

证据：`managed-execution-probe.json`、`managed-execution-probe.log` 和 `probe/ArchitectureReviewProbe.cs`。

处理：操作持有完整托管输入快照，构建和 PlayerLink 提取都消费该快照；包含 imports、源码、资源、生成器和实际引用。继续在必要边界验证原输入变化，不能只添加一次哈希，也不能用硬链接代替固定输入。

### F02 — P2：提交成功后的错误仍被报告为发布失败

位置：

- [PlayerSupportPackCatalog.cs](../../../build/pipeline/Inno.Build/Player/PlayerSupportPackCatalog.cs#L240)：先写入新 `current`，再取得并校验返回 lease。
- [PlayerSupportPackPublisher.cs](../../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L133)：catalog 已提交后才写 preparation index，异常直接传播。
- 同文件第 143–146 行：`finally` 中的 staging 删除异常也可覆盖原结果。

本轮公开 API 探针让 preparation index 目录出现确定性的 IO 失败。调用抛 `IOException`，但 `current` 已变更，且新 Pack 可正常读取。它没有暴露半成品；问题是返回状态与提交事实不一致，违反“失败保留上次输出”的一般描述。

证据：`failure-boundary-probes.json`。`packCurrentChangedDespiteFailure=true`、`newlySelectedPackReadable=true`。

处理：明确唯一提交点；提交前取得并验证返回的读取保护。输入索引作为可重建的查询索引，与真实 Pack 权威区分。提交后的索引/清理错误作为提交后诊断，或者纳入同一明确事务，不能向调用者伪装成提交前失败。补充索引权限失败、提交后取消和 staging 清理失败的公开边界回归。

### F03 — P2：缓存保护清单不完整时未安全失败

位置：[BuildCacheInventory.cs](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheInventory.cs#L90)。

`Protect()` 静默跳过未登记的 root。owner 返回一个当前产物路径，但该路径未包含在 artifact inventory 中时，其保护关系没有被验证或保留，其他依赖产物仍可进入退休候选。

本轮公开 API 探针返回真实存在但遗漏登记的 current root；计划接受了该清单，并把 fixture 依赖加入 candidates。探针只创建计划，没有执行缓存删除。该机制目前尚未接入标准发行的自动清理，因此这里是启用前必须修正的协议缺口，不是已经发生生产文件误删的证据。

处理：登记后的完整保护根必须可解析。缺失、模糊或不完整关系明确拒绝退休；如果确有需要忽略失效 receipt，应由拥有完整依赖信息的 publisher 明确完成对账，清理器不自行猜测。

### F04 — P2：预算和退休仍只有机制，没有完整产品调用链

位置：[CleanCommand.cs](../../../build/cli/Inno.Build.Cli/Commands/CleanCommand.cs#L15)。

`clean` 只接受 `product`；生产检索没有发现标准发行、Native/Binding/Pack publisher 使用 `BuildCacheOwnerDescriptor`/`BuildCacheCleanupPlan` 注册和执行回收。读取 leases 与退休基础契约已有实现，64 GiB/20 GiB/5 秒策略尚未形成实际自动管理能力。

处理：同一发行/build operation 注册真实 owner、当前产品及 Pack 依赖根，协调现有 publication/reader/workspace leases，然后接入显式 cache clean 和构建后的有界评估。不得绕过 F03 或 publisher 的读写保护间隙先开启删除。

### F05 — P2：Build 命令本身仍有重复准备成本

当时位置：`entry/Inno.Entry.Bootstrap/DevelopmentEntry.cs:147`。这是该次审查的历史源码位置，当前已删除；现行入口见 [InnoEngine.Build](../../../entry/InnoEngine.Build/Program.cs)。

每次 Build 命令都执行依赖准备、CLI 托管 Build 和 TargetPath 求值。Editor 的有效产品直接启动路径已经分离；Build 应用自身的准备尚未达到同等复用程度。

之前同环境的有效 Pack 热命中为约 69.5 秒，其中 CLI 托管构建约 39.8 秒。该历史日志证明 Pack 内部跳过了 Player Build/Native compiler/绑定生成，不能证明 Build 入口没有重复工作，也不是本轮操作系统进程计数。

处理：轻量入口使用明确的 Build 工具输入身份与已验证 publication，在源码或 SDK 改变时准备新工具。调用实际构建仍保留正确的阶段校验；不能以任意 mtime 或“已完成”标记跳过身份检查。

### F06 — P2：Wiki 完整行为与产品验收仍未闭合

当前 Wiki 有 284 页，232 项目页面有对应位置，已有生成 ABI 索引。但 ImGui、MiniAudio 的 Native 页面分别约 5.82 MB、5.24 MB；大量条目只有声明和 ABI 形状，不能替代函数行为、借用内存、callback、线程与失败语义。

最后一次严格清单有 45,459 个声明、189 个示例，示例编译无错误；该严格门禁失败，且之后还增加了 Native 索引和新 Cache API。本轮未重新执行完整 Wiki 严格门禁，不能沿用此前示例结果宣称最终 Wiki 已通过。

本轮当前 docs 检索未发现 `Inno.Canvas`、`Inno.Rendering2D`、对应外部仓库名和消费者目录表述；浏览器 Canvas 和引擎 GraphCanvasState 是合法概念。内容边界的这一结果不等于整份 Wiki 逐页审查完成。

处理：保留一个权威项目入口，以工作流和 owner 解释真实契约，把完整 ABI 参考按同一项目的主题导航组织；保持每个符号唯一映射。完成精确引用与严格门禁，然后核对全部本机发布、外部 API 消费者、reload、性能及视觉证据。

### F07 — P2 扩展热点：动态 Native 加载仍有固定宿主映射

位置：[NativeDllLoader.cs](../../../backends/Interop/native/Inno.Native.LibraryLoading/NativeDllLoader.cs#L201)。

动态 loader 通过 Windows/macOS/Linux 和 ProcessArchitecture 拼出部署目录 target，并自行决定 dll/dylib/so 名称。它没有引用平台程序集，也没有把系统判断放入共同 Scene/Rendering 领域；当前桌面目标可以正常消费这种部署。但它仍是共享 Interop backend 内的命名系统映射，不能宣称未来任何新系统都只增加平台目录而绝不修改此实现。

静态链接的 Browser 或未来主机可以完全不使用动态 loader，这个发现不代表它们当前运行错误。如果新系统确实需要动态 Native 加载，应由产品部署明确提供目标/文件布局和必要宿主加载接入，共同 loader 负责校验与调用，不继续扩大封闭 OS 表。宿主构建工具的自动探测与运行产品部署目标保持独立。

## 已经清楚的边界

| 部分 | 当前判断 | 依据 |
| --- | --- | --- |
| Rendering Core | 只提供中立图形机制 | 项目依赖与当前有效图，不依赖资产/平台/backend |
| Rendering Assets / Authoring / Runtime | 职责分开，Player 运行闭包排除创作与构建工具 | 项目图与架构门禁 |
| SDL | 共同窗口操作与平台 SDK 差异分开 | Sdl3WindowOperations、三个 host integrations |
| BGFX | surface SPI 由平台 runtime integration 注入 | provider 构造函数要求 IBgfxSurfaceIntegration，Device 不维护命名平台名单 |
| 窗口退休 | framebuffer 销毁确认先于原生窗口释放 | managed 提交序号、退休 Task、owner 安全点与 RetirementBarrier |
| Runtime/Player/Editor Hosting | 借用中立 factory/config/context | 当前有效依赖图，没有具体平台引用 |
| Native 结果 | Native 与 Binding leases 由结果集合持有 | ProductNativeBuildResult、NativeBindingGenerationResult 的释放链 |
| 平台打包与内容编译 | 同一目标绑定两个独立职责 | GameBuildTargetBinding、GameBuildContribution |
| 运行 backend 注册 | 开放 ID、catalog 和明确 selection | GraphicsApi/RenderingBackendId 是开放值；无隐式 rendering 默认选择 |
| Graph 帧路径 | validation 与最终 compile 分开 | revision 缓存、mutation rollback、Contributors snapshot/scratch、帧内最终 Compile 调用 |
| Task 复用 | Build 生命周期保存 BCL 数据，不保存 SDK/ALC owner | IBuildEngine4 注册与 fresh 请求身份检查 |

标准发行引用具体 backend 和 platform、产品入口选择 SDL/BGFX、组合 ImGui+SDL+BGFX 的 presentation 项目引用两者，都是明确的组合职责。它们不应移入共同领域，也不需要靠消灭所有具体引用实现“零耦合”。

Bootstrap 通过公开应用入口加载平台产品/CLI，是保持轻量依赖的启动适配。这里没有使用私有反射或测试穿透。

## 扩展路径

### 新 backend

```text
backends/NewBackend/
├─ runtime/                 实现中立 provider / device / service
├─ native/                  真正需要 Native 时建立
├─ build/                   组件 recipe、内容 compiler 与产物声明
└─ tests/

platforms/<实际平台>/integrations/
├─ runtime integration      有真实 surface / SDK 差异时建立
└─ build integration        有真实工具 / 编译差异时建立

产品组合                    注入 provider / selection
标准发行组合                绑定 compiler / Native plan / integration
```

平台 packager、Scene、Input 和共同 Player 流程不需要认识新 renderer 的实现名。没有实际差异时无需新建 integration。

### 新平台

新增真实系统服务、目标/ABI/SDK、必要 integrations、薄产品入口、布局/签名/Support Pack，在发行组合中注册。日常开发入口支持该平台 Editor 时，再增加宿主路由。

新增 Windows CPU 可以复用现有 Windows 产品源码。Linux 当前只有 Native tooling；NS/iOS 等实际可用性还取决于 SDK、托管运行时、第三方库和真实设备验收。

### 新能力

已有中立契约能表达时只增加实现。新增 XR、多输出、mesh/ray tracing 等真实能力若超出现有契约，应在所属领域增加中立能力/生命周期契约；这属于能力演进，不应通过命名平台 switch 或 Native 指针泄漏实现。

## 本轮验证结果

| 检查 | 结果 |
| --- | --- |
| 当前源码架构测试重新编译 | 0 警告、0 错误；owner 和测试载入 Architecture DLL SHA-256 一致 |
| 选定架构/集成/Wiki 边界 fixture | 11 passed，0 failed；约 22 秒 |
| 真实 SDK MSBuild 求值图 | 232/232，通过；包含共同注入引用与 Player 禁止闭包检查 |
| Solution | 232 项目对应 232 实际项目；无游离、失效、重复或空分组 |
| 日常生产可执行 | 165 非测试项目中只有 InnoEngine.Editor / InnoEngine.Build |
| CacheProbe | 默认 Library；只在所属测试传入明确属性时构建隔离 Exe |
| 同长度临时源码修改并恢复 | 实际编译到 other，但 snapshot fresh 验证接受 first；确认 F01 |
| Pack 提交后索引 IO 失败 | 实际异常且 current 已改变；确认 F02 |
| 未登记保护根 | 计划接受并选中 fixture 依赖；确认 F03，未执行删除 |
| git diff --check | 通过 |
| 磁盘 | 审查开始可用约 116.9 GiB；未清理用户/SDK/NuGet 缓存 |

实际命令与代码保留在 artifacts 审查目录。探针是被忽略的临时验收项目，未加入生产 Solution，未引入新的生产 Program。

## 完成当前计划前的其他核对

- Binding TargetArtifacts 仍在 Native 项目 `obj/<target>/<fingerprint>`，不是计划中的 `artifacts/bindings`。这是布局未迁完，现有目标隔离并不因此失效。Task 和标准发行对同一组件使用不同 descriptor.id，迁移前须统一真正的 owner 身份，不能只替换路径字符串。
- 缓存 inventory 存在两处成对所有权校验，执行前还会探测全部未保护条目；5 秒限制并未覆盖整个规划成本。接入真实 owner 后要测量并收口，不能承诺大型缓存回收总耗时已经受限。
- Cache 路径比较在 macOS 一律忽略大小写；大小写敏感文件系统的行为需要统一路径语义并验证。本轮没有 macOS 实机证据。
- 完整文件映射仍有 2,602 条 Pending；不能把这个过程记录当作最终逐文件闭包已核查。
- 本轮未运行最终 Windows Debug/Release GUI、CoreCLR/NativeAOT Player、Browser 解释执行/AOT、完整 reload GC barrier 和外部消费者矩阵。此前有限 GUI/启动测量不能作为当前最终源码的整套验收。

## 建议提交方式

可以保存阶段成果，例如：

```text
refactor(build): establish explicit development and artifact ownership boundaries
docs(architecture): record current review findings and acceptance gaps
```

不建议把当前全部内容以“complete cleanup”或“fully validated”语义直接合入 main。先修复 F01–F03，完成真实缓存调用链与 Build 工具准备收口，再通过当前 Wiki 和必要产品门禁。

尚未实测的平台继续明确标注。`/System/Library/Sounds/Glass.aiff` 在当前 Windows 环境不可访问，提示音未播放。
