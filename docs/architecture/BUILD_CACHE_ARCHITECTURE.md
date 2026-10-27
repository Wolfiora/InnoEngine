# 构建输入、工作目录与缓存

[架构索引](README.md) · [Build](../build/README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

## 职责

构建工具冻结实际输入和工具选择，组件维护自己的 recipe，发布器验证并安装完整产物。缓存身份用于判断当前请求是否可以消费这些产物；工作目录用于保留兼容的编译中间态，两者有不同的生命周期。

最初的 Foundation 引导是独立的普通 SDK 编译工作区：显式 `ArtifactsPath` owner 下的 `.inno-task-foundation`，或默认 Task artifacts 下的固定 `foundation`。其配置目录复用，不按 Build 次数增加；它不属于不可变 publication，也不参与根据后缀猜测删除。中立引导和调用方源码不能以不同 SDK 属性共享同一个编译输出。实际 publisher 与 runtime publication 仍使用既有 Core.IO owner、lease 和维护协议。

绑定 generation 同时携带不可变产物目录与实际发布 owner；selection 将两者一起传入后续 MSBuild。冻结源码树只改变组件源码位置，不改变产物的协调域。生成 Task 也输出同一 owner，materialization 不从源码工作区推测它，并继续校验产物必须属于声明 owner。

托管输入规划使用 `artifacts/managed/evaluations/<request identity>` 下独立的 SDK `ArtifactsPath`，不改写普通 IDE 的 `obj`。还原、声明的输入工程还原、求值与内容读取都在同一 owner/叶锁范围内；不同全局属性的辅助工程拥有独立子布局，避免 RID 图相互覆盖。派生图可以重建，源输入变化仍在 fresh 校验中拒绝。

## 当前目录归属

缓存登记区分不可变产物、可重用 workspace 与进程工作目录。只有不可变产物的当前选择形成长期保护根；workspace 的偏好不会阻止空闲目录退休。活动编译的独占 lease、活动进程的 PID/启动时间和完整依赖仍优先保护。

临时 request、生成 staging 和 Task 私有加载目录 在首次 payload 写入前取得进程 claim；稳定 workspace 先由 `BeginWorkingDirectory` 建立生产者登记，再写入源码或 SDK 输出。已有但未登记的目录明确拒绝自动认领。工作目录的实际大小在清理清点时测量，半成品的旧尺寸记录不代表预算或完整性。

进程 claim 使用显式 JSON 字段读写，不依赖反射式序列化或运行时代码生成，因此可被 NativeAOT 产品消费。损坏的子进程记录产生 `Unknown`，保留目录；不能把无法读取的 owner 当成已经退出。

权限错误、损坏 owner 和无法验证的进程身份均保留。旧 PID-only 记录不作为当前身份解释；未经证明的历史目录不会因一次目录扫描获得删除权限。

| 位置 | 内容与 owner |
| --- | --- |
| `extern/` | 第三方源代码；构建只读 |
| `backends/<组件>/native/<项目>/Bindings/` | 当前组件的生成定义和配置 |
| `backends/<组件>/native/<项目>/Generated/` | HostSource 的 managed 生成输出 |
| `artifacts/bindings/<component>/<target>/<fingerprint>/` | 当前 TargetArtifacts 生成缓存；含 Native、Generated 和完整性清单 |
| `backends/<组件>/build/<项目>/obj/native/<target>/<workspaceIdentity>/` | 当前组件的固定输入副本与可复用 Native 编译工作目录 |
| `artifacts/native/<component>/<target>/<fingerprint>/` | 验证后的 Native 成品 |
| Support Pack root 下的 `<target>/<fingerprint>/` | 完整发布输入；`current` 原子选择当前内容身份 |
| `artifacts/products/editor/<target>/<configuration>/product.lock` | 正在运行的 Editor 与显式产品 clean 之间的读取/独占协调；不保存部署 receipt |
| `entry/InnoEngine.Editor/bin/<Configuration>/net10.0/` | SDK 构建的可运行入口、所选产品程序集和传递的 native 部署 |
| `artifacts/build-tools/tasks/` | Task 引导工作目录、不可变运行闭包和私有载入目录 |

目标绑定统一由显式组件 ID 选择。MSBuild 与直接生成读取同一份 `Bindings/bindings.props`，不存在旧目录读取路径。

Windows 工具执行可能使用用户 Temp 下的短路径 junction。它指向组件实际工作目录，由 `ToolchainWorkingDirectory` 取得独占使用权；进程停止后释放别名，保留实际输入和编译结果。别名与真实目录不能重复计入磁盘占用。

## 输入身份与工作目录身份

输入目录扫描排除受构建机制定义的派生目录，其中 `__pycache__` 是 Python 字节码缓存。Python 源文件和实际解释器/编译器仍以内容哈希校验；使用 Python 的 CMake 工具将缓存放入 SDK 身份隔离的 workspace，避免把 SDK 中的自动缓存当成新的源码。

`NativeBuildRecipe` 声明组件源码、所选生成身份、执行代码、SDK、工具及有序参数。完整输入内容决定最终 fingerprint；长度或 mtime 不足以证明内容一致。共同执行代码和实际组件参数改变仍应失效，无关平台配置不应进入当前目标的 recipe。

工作目录身份固定目标、ABI、工具、配置与布局。兼容的源码更新同步到固定副本并继续使用编译中间态。相同字节不重复复制；原始输入在必要验证阶段变化时拒绝发布。编译读取固定副本，不能用指向可变源码的 hardlink 代替输入快照。

## 消费者所有权

| 返回值 | 消费结束时释放的内容 |
| --- | --- |
| `NativeBindingGenerationResult` | 完整生成集合的读取保护；HostSource 保持生成写 owner，目标缓存保持共享读取 lease |
| `NativeBuildProduct` | 一个 Native 产物的读取保护 |
| `ProductNativeBuildResult` | 按依赖顺序取得的全部 Native 产物，以及其绑定生成保护 |
| `PlayerSupportPackLease` | 编译、链接或复制期间固定的 Pack 目录 |

描述符和路径是借用信息，保留字符串不会延长 lease。消费者应在所有工具进程和异步读取完成后释放结果。修复或退休取得同一 artifact 的独占保护，不能替换活动 reader 的目录。

MSBuild 的 `MaterializeBindingsTask` 在保护期间把选定绑定复制到 SDK 自己的中间目录。C# 编译只消费这个副本；任务返回后不长期持有缓存锁。

产品的 `NativeBindingSelection.props` 保存在该产品 SDK 中间目录中。只有声明 `BindGenGeneratedBindings` 的项目导入它，共同领域项目只传递选择位置。内容未变时保留文件和时间戳，避免每次构建因新的临时 import 路径而重编译整个产品。Publish 后仍保留该文件，产品 clean 回收；发布前继续验证它与本次 Native 闭包一致，不能凭存在性跳过校验。

绑定定义的缓存身份覆盖选中的配置、目标、输出模式、扩展和实际输入。添加或修改未选目标的映射不改变当前生成身份；所选配置、facade、include、工具或生成实现发生变化仍需重新生成。

## 当前能力与未完成部分

- Native 和绑定输出通过完整文件集合与内容哈希验证；支持共享读取与独占修复协调。
- Support Pack 在准备前计算实际输入身份，查询准备索引，并验证完整 Pack 内容后取得读取保护；真实 Windows 第二次请求已确认跳过实际准备。
- Task runtime 已有 Build 生命周期成功结果复用；同一根构建的轻量 publisher 共用编译 owner，各消费边界仍使用独立载入目录。普通 entry 的 C# 构建、产品 Native 准备和私有 Task 构建的成本分别记录；entry 不再维护 CLI 启动 publication。
- Native、目标绑定、Support Pack、managed workspace、图求值工具、生成扩展及 Task 缓存发布者通过 `BuildCachePublisher` 登记实际产物、读取锁和当前根；标准发行提供这些 owner，CLI 支持 `clean --scope cache --dry-run` 及实际清理。成功 Editor、绑定、Shader 或 Pack 命令执行有界压力回收。

## 退休契约

`BuildCacheOwnerDescriptor` 明确登记一个 publisher 的物理边界、持久协调 lease、产物清单及当前保护根。清理机制不通过目录名、后缀或时间戳猜测 owner。登记者必须提供真实的所有权证据，在同一协调 lease 下更新根，并在更新期间保留所引用产物的读取保护。

`BuildCacheArtifactDescriptor` 是只读的目录、真实 producer/reader lease、发布者在写权限下记录的文件大小、最后使用时间及依赖描述。预算读取登记大小，完整性仍独立验证实际文件集合与内容哈希；大小不是缓存有效性的证据。Native 成品可以提供自己的读取保护路径，可复用 workspace 提供既有编译独占 lease；清理不会另猜一个锁。时间仅决定回收顺序，不参与缓存完整性判断。当前根及其全部依赖受保护；无法确认的依赖、重叠所有权、位于待删目录中的锁或循环依赖会在删除前失败。

`BuildCacheCleanupPlan` 冻结预览候选。执行时重新读取 owner 清单和保护根，以零等待取得独占 artifact lease；活动 reader 或正在更新的 owner 会使对应工作跳过。活动父产物即使不在预算候选中，其依赖也会被保护。失败的父产物同样保留依赖。owner 协调文件始终保留。叶锁在参与者全部释放、owner 独占且 payload 与登记已退休后才可删除；普通 FileLease 的释放语义不变。

`BuildCachePolicy` 默认使用 64 GiB 的登记文件预算、20 GiB 的实际磁盘空闲阈值及 5 秒的完整清理操作预算（包含清单读取）。达到时间预算后不开始下一项操作，已经执行的文件系统操作仍须完成。当前保护集合超过预算时，不强制删除。

`BuildCacheCleanupResult.retiredBytes` 是已删除产物声明的逻辑文件大小，不是实际释放的磁盘块数。Hardlink、压缩及其他进程活动会影响真实回收量；磁盘报告必须另行记录卷的实际可用空间。

登记记录保存于 publisher 根目录，不进入产物内容哈希。发布者在实际变更后记录大小，清理不为预算重复遍历整个源码工作区。最终删除前仍逐项检查路径与 reparse point。

Unix 编译器在可复用 workspace 中生成的库名称文件别名由内部缓存清单验证：链条始终位于同一 owned workspace，最终指向普通文件；目录链接、逃逸、循环和悬空链接被拒绝。预算只累计实际文件，退休删除别名本身，不沿链接遍历。不可变发布内容和进程工作目录不允许此例外。Native workspace 的验证在最终 candidate 提交前执行。

Task 的 host、load、publisher 和 shared workspace 分别拥有独立登记根。`RecordProcessUse` 在共享协调权限内记录 PID 与 UTC 启动 ticks，并累积同一进程操作实际使用的依赖闭包；后续登记不能抹掉仍被使用的依赖。活动或不可确认的进程保护对应目录，load 同时保护它所用的 host 和 MSBuild 编译 workspace。PID 相同而启动时间不同表示原生命周期已经结束；权限或身份无法确认时延期退休。编译 workspace 使用外部持久 lease，不能把删除协调文件当作清理。

MSBuild 会在同一构建中缓存 Restore 目标结果，Task 方法返回不代表这些中间文件已经不再使用。Task 在首次输入求值、等待叶锁和执行期间按路径顺序持有相关 owner 活动保护，并在既有 load 进程登记中引用 workspace。即使不同项目随后复用 SDK 的 Restore 结果，维护也不会删除该 workspace；宿主进程退出后按同一退休协议回收，不在 Build registry 保存 lease 或私有 SDK 对象。

预算使用完成 writer 登记的 workspace 大小；未完成的零大小登记与进程工作目录逐项读取，并在读取中检查时间预算。这些大小只服务预算，不参与 Native/Pack 完整性命中。真正退休时取得独占权、重新核对依赖与路径，再统计实际文件字节；卷空间变化另行测量，不能把 hardlink 的逻辑字节视为实际回收量。

共享 owner lease 覆盖发布与读取交接，退休取得同一路径的独占保护。无法解析的 current root 或依赖会拒绝整次删除。未由当前 publisher 登记的目录不归自动清理；不会把历史目录年龄当作所有权证据。

不能依据文件年龄或目录名任意删除构建缓存，也不能删除锁文件来解锁。手动排查只处理已经明确失去 owner 的本轮中间输出；NuGet、SDK、用户和浏览器全局缓存不属于本系统管理范围。

## 继续阅读

- [Native Toolchains](../build/Inno.Build.Toolchains.md)：recipe、结果和读取保护。
- [Bindings](../build/Inno.Build.Bindings.md)：生成批次及完整性。
- [Support Packs](../build/Inno.Build.SupportPacks.Core.md)：准备、发布和消费。
- [Task Hosting](../build/Inno.Build.TaskHosting.md)：引导、共享运行闭包和私有载入。

## 元数据与成功根操作

`CoordinatedFileLease` 在打开或等待叶锁前取得 owner 共享权，并持有到叶锁释放。所有不可变 reader 明确接收 publisher 根，不推导 owner。ProcessOwnedDirectory 使用同一 root/owner.lock 协议。维护按规范路径取得所有 owner 独占权，fresh 验证 current、依赖、进程及路径，关闭叶锁句柄后退休已证明归属的登记和叶锁。未知文件和无法确认的记录保留并报告。

元数据退休独立于字节压力。已退出且不受依赖保护的进程工作目录也参与每次维护，不等待达到字节预算；不可变产物和可复用 workspace 仍按压力策略选择。内容相同的 `.current` 保持时间戳；过期工作 claim 和已登记 index 随依赖失效收敛。dry-run 不删除 payload、记录或指针。BGCS 缓存按公开配置指定的整个 SDK workspace 登记与退休，不读取或修改其内部缓存协议。

Editor 根 Build/Publish 与 CLI 共用 `BuildMaintenanceApplication`。IDE 普通构建成功后执行一次；CLI editor 的子 MSBuild 被明确抑制，由外层命令执行一次。game、plugin、scripts 和 import-sample 的成功根命令先释放 Workspace 与 Native 读取保护，再维护一次；失败或取消不进入成功维护。Build 入口自身不自动维护，保留 `clean --dry-run` 的只读含义。默认自动预算 5 秒、64 GiB、20 GiB 空闲阈值，显式维护默认 600 秒。自动失败单独报告；显式清理失败返回失败状态。
