# Inno.Build.Toolchains

托管工具使用 .NET 10 SDK。输入快照先读取求值后的工程图，再仅执行 SDK 的编译符号准备 target，捕获包括 `NET10_0` 在内的最终符号集合；该步骤不执行编译、原生准备或产品发布。SDK target 输入与求值器一起参与内容指纹，缺少符号准备能力时明确失败。

托管输入快照包含当前 SDK Restore 实际解析的 assets、包内容、reference pack、项目声明和编译选项。输入路径通过 MSBuild 求值取得，完整 target、SDK 发布属性与输出隔离保持明确；源码目录不保存额外依赖清单。固定源码编译消费同一不可变输入，详见[依赖还原与 CI](DEPENDENCY_SUBMISSION.md)。

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../adding-platform.md)

## 设计思路

工具链将输入冻结、执行、发布和消费者读取保护分开。相同内容可以复用不可变产物，但等待锁或执行工具后仍重新确认输入；可变源码和当前 reader 不会因为“缓存命中”而被忽略。具体平台贡献 SDK 与 recipe，共同工具链只处理明确请求与中立结果。

## 使用教程

需要检查项目关系时调用 `ManagedProjectGraphCapture.CaptureAsync`，传入绝对仓库、CLI 和项目路径，显式选择配置并贯穿取消令牌。它返回 SDK 求值快照，不代表项目已经构建。开启引用解析前先正常还原相关项目；不能拿另一个配置的 DLL 补齐引用。

需要消费 Native 产品时保持 `ProductNativeBuildResult` 存活，直到复制、编译、链接和部署结束后再释放。不要从结果中拿到路径就立即 Dispose，也不要用文件存在或相同长度代替读取 lease 的完整性校验。具体调用参数、异常和返回语义见下方对应章节及 API 参考。

## 仓库批量求值

根项目快照直接取自同一 `ProjectGraph` 的入口实例，不再逐个 `LoadProject` 重复求值。根属性、输出路径和 imports 因而对应同一次求值；条件引用产生的不同全局属性实例仍完整保留。回归覆盖非确定性 MSBuild 属性，要求根输出与图实例一致，避免把两次求值的结果拼成一个快照。

`ManagedProjectGraphCapture.CaptureAsync(engineRoot, dotnetHost, projects, properties, cancellationToken, resolveReferences)` 在明确选定的 SDK 中一次求值多个绝对项目路径，返回属性、ProjectReference、源码/资源/附加输入与 imports 的内部 JSON snapshot。`resolveReferences` 默认为 false；显式启用后，在同一 SDK 进程解析已还原的编译引用与隐式编译符号，供 Wiki 使用。它复用既有受 owner/lease 保护的求值器，不执行工程 Build、Restore 或 native target；求值器本身缺失时会先准备不可变工具产物。

`ProductNativePreparation` 负责将已完成准备转成中立数据供同一 MSBuild 根构建复用：`FreezeAsync` 冻结 recipe、生成器、源码及 SDK 的内容身份；`Capture` 仅从成功且仍持有读取保护的完整产品闭包收集 BCL 数据；`TryAcquireAsync` 为下一消费者独立取得读取 lease，并在等待后重新冻结当前完整请求，再检查已记录的目录成员与内容哈希。这些等待后的检查共用一个全新读取阶段，不要求消费者在等待前重复冻结一遍；旧阶段的内容缓存不进入这个阶段。未命中返回 null，调用方必须正常准备；它不表示验收通过。聚合静态产物或无 owner 的绑定产物不支持此复用。失败、取消、部署或释放失败均不得登记成功状态。

这是验证工具组合所需的最小公开入口，不进入脚本导出，没有 protected 扩展点。输入为空、路径非绝对、项目缺失、SDK 失败或取消均明确失败，不能返回部分图冒充完整覆盖。相同全局属性共享 ProjectCollection；不同配置或目标必须分别捕获。

## 原生生成输入的中立契约

`NativeArtifactTransfer` 负责 runner 之间的不可变产物传输。`ExportAsync(engineRoot, archivePath, cancellationToken)` 仅导出当前 checkout 已登记且通过完整清单校验的 native/bindings 生成物；导出期间保留各产物读取 lease，完成后原子替换归档，返回生成物数量。`ImportAsync` 先在登记的临时 owner 中检查完整归档、路径、文件集合和内容哈希，再安装候选，返回新增候选数量。已有目标由原 publisher 继续负责，活动读取者不受影响；候选只有在真实消费者重新验证请求输入后才能登记采用。

归档不包含 owner、PID、lease、可写 obj 或工作目录。损坏、越界、链接和不完整 payload 抛出 `IOException`，取消抛出 `OperationCanceledException`；失败归档保留供诊断，真实构建仍需通过。这些入口不证明测试通过，也不提供脚本或 protected 扩展接口。

<!-- api: T:Inno.Build.Toolchains.NativeArtifactTransfer 0e63aa46f32d1412cb9d5fc7cf3ff7074d619a05b1b60ba6270dfe0e7f2125a3 -->
<!-- api: M:Inno.Build.Toolchains.NativeArtifactTransfer.ExportAsync(System.String,System.String,System.Threading.CancellationToken) 577932ed79b8d8541f52ce6daa4284f08be35ee3b3101f777b48bef7cc0976a6 -->
<!-- api: M:Inno.Build.Toolchains.NativeArtifactTransfer.ImportAsync(System.String,System.String,System.Threading.CancellationToken) 030a4376b9a5f172fa2a144aa8cfa004f3985cdc8012e7b772076ed6ab037767 -->

`NativeBindingCompilation` 描述已选定生成结果的编译要求，不包含 BGCS 类型或平台选择逻辑。`NativeBindingSourceRoot` 标识组件内的根；`NativeBindingSourceFile` 保存相对路径、长度、SHA-256 与是否编译；`NativeBindingIncludeDirectory` 保留 include 顺序、system 与 framework 搜索语义。生成器适配层负责转换，具体平台 provider 负责选择真实 SDK。

`Load` 严格拒绝缺少必需字段、未知字段、无效根身份和越界路径。`GetInputs` 提供冻结输入集合，`VerifyInputs` 在消费前核对实际字节。外部用户头文件与生成源码必须复制进本次操作的冻结树；选定 SDK 的系统头仍由 SDK 提供。`nativeDirectory` 是完整原生产物与编译要求目录。

`defines` 只作用于清单标记的翻译单元；`compilerDefines` 记录解析目标事实，不覆盖消费编译器的内建宏。上游 CMake target 保持自身配置。`requiredExports` 和静态表要求记录实际链接契约，DLL 的符号存在性通过加载实际产物另行验证。

这些 public 类型是生成器和中立构建器之间的可替换边界，不进入游戏脚本导出。生成结果的 lease 继续覆盖冻结复制、编译和链接的完整消费期。

## 构建产物的借用与释放

`NativeBindingGenerationDescriptor.usesRuntimeImports` 是生成器提供的中立初始化事实，与 `NativeLibraryKind` 相互独立。selection 写入 `InnoNativeRuntimeImports`，绑定 owner 据此编译初始化入口；相同指纹但初始化方式不同的 selection 仍会被拒绝。此事实不携带 BGCS 类型或具体平台分支。

`ProductNativeBuildResult` 拥有 Native 产物读取 lease 和对应的 `NativeBindingGenerationResult`。需要复制 Native 文件、生成 managed selection、编译或链接的调用方必须在这些操作结束后才释放结果。Context 借用生成描述，不接管结果生命周期。

结果释放按反向顺序尝试全部 Native owner，随后释放绑定结果；某一项失败不会阻止其他 owner 清理，最终以 `AggregateException` 汇总。构建本身失败时，清理异常与原始构建异常一并保留。

`BuildArtifactReadLease` 在保护期间验证完整文件集合和内容哈希；`BuildArtifactWriteLease` 用同一 Core.IO 协议等待 reader 后进行修复或退休。两者不会通过删除锁文件解锁。读取保护只约束参与此协议的构建操作，不能阻止外部程序直接修改文件；每次重新获取产物仍会验证实际内容。

Native、Bindings、Support Pack、Task 和 Build 工具 publication 通过实际 publisher 登记产物、目标、当前根及依赖。`clean --scope cache` 使用这些登记，显式目标只选择该目标的可退休产物；共享宿主产物与未选目标继续保护其依赖。未登记目录不授予自动删除权限。

Native 工作目录中的 `recipe.json` 是内部诊断资料，记录实际有序声明、逻辑输入哈希、工作目录身份与最终产物身份。它不代替 fresh 输入验证或输出清单，也不参与产物读取 API。

## 缓存 owner 接入

每个 `BuildCachePublisher` 显式选择 `BuildCacheArtifactRole`：不可变产物的当前根保护实际部署；可重用 workspace 的偏好记录不永久钉住空闲目录；进程工作目录由 PID 与启动时间、生产 lease 和依赖保护。活动编译仍由独占 workspace lease 保护。临时 claim 必须在首次 payload 写入前建立，不把未经登记的历史目录纳入清理。

可重用 workspace 允许编译器生成的文件符号链接，例如 Unix dylib 的名称别名。完整链接链必须留在同一 workspace 并终止于普通文件；目录链接、越界、循环及悬空链接拒绝登记和退休。别名不重复计算目标文件字节。不可变产物和进程工作目录仍拒绝全部链接；Native candidate 发布前先验证 workspace。

完整 Native 热命中采用内部批次路径：预读候选及依赖选择，释放预读 lease 后按规范路径取得全部 workspace 锁，再取得完整产物 reader，并使用一个新的 `native-product-lock` 校验阶段。相同物理输入只完整哈希一次，各组件的逻辑输入、参数顺序和指纹仍分别验证。任一缺失或损坏都释放完整候选集合并进入原有冷路径；冷路径的锁后和编译后 fresh 校验不复用批次缓存。

`ProductNativeBuildStep.bindDependencies` 由 backend 提供需要依赖产物的最终 recipe 变换，与真实编译共用同一实现。共享 executor 不推断组件名、文件名或库类型。返回的产品闭包仍唯一拥有全部 Native 和 Binding leases。

构建组件通过 `BuildCacheOwnerDescriptor` 提供明确的物理根、协调 lease、只读产物清单和当前保护根。Callback 由操作借用，不进入静态注册或游戏脚本。登记者必须在同一协调 lease 下完成发布和保护根更新，并持有实际产物读取保护。

`BuildCachePublisher.RecordProcessUse` 累积同一进程操作的实际依赖，后续登记不会移除此前仍被该操作使用的 host 或 workspace。依赖仅是现有缓存图中的明确目录，不授予新路径所有权；进程退出后的统一退休释放整条保护关系。MSBuild Task 通过这一契约保护同一构建中已完成 Restore、但后续项目仍会复用的编译工作区。

`BuildCacheCleanupPlan.Create` 返回固定候选，预览与执行共用此提案。`ExecuteAsync` 在删除前重新检查清单、根和活动 reader；不会强行等待或删除活动 generation，也不删除持久协调文件。循环依赖、未登记依赖或保护根、重叠目录或路径越界在删除前失败。预算及返回结果详见[缓存架构](../../architecture/build-and-cache.md)。

`BuildMaintenanceApplication` 在既有登记目录中按产物保存已经完成的容量盘点。盘点身份包含生产者完整登记内容；下一次 writer 的 `BeginWorkingDirectory` 会使旧盘点失效。活动 writer 的变化中容量不保存为完成状态；中断后可以复用其他已完成分片。进度不是产物完整性证明，也不保存退休授权或旧保护关系；每次仍重新读取全部 owner、当前根和依赖，删除前在锁内检查文件边界。dry-run 不创建或更新盘点进度。

跨 runner 仅运输不可变 generation 的 payload 与 manifest。`NativeArtifactPublisher` 和产品批次读取路径仍按当前 recipe 验证完整内容并重新登记本地 owner；缺失文件、额外文件、同长度篡改或不匹配身份均不能绕过真实 producer。修复失败保留失败，不能因为存在一个缓存目录而报告成功。

## 固定托管编译输入

`ManagedBuildInputSnapshot` 记录实际 SDK 求值的输入身份。`CreateExecutionAsync` 将操作已声明的仓库输入复制到具有独占 lease 的工作目录；`ManagedBuildExecution.project` 和 `engineRoot` 用于实际编译与源码提取。复制逐文件校验计划哈希，内容相同的文件保留 mtime，移除旧清单中不再需要的 owned 输入。编译后验证源码副本，原输入仍在发布边界 fresh 校验。工作目录复用不代表发布产物有效性；外部 SDK 和包继续使用明确选定的位置。

带平台限定的 TFM（例如 `net10.0-browser`）保留完整编译目标身份；FrameworkReference 根据 SDK 的基础 framework moniker 选择对应 reference pack。解析使用所选 SDK 自带的 NuGet.Frameworks，不能用目录后缀推导或扫描另一套 SDK 补齐引用。

```csharp
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

static ValueTask<BuildCacheCleanupResult> RetireUnused(
    string checkout,
    IReadOnlyList<BuildCacheOwnerDescriptor> owners,
    CancellationToken cancellation
) {
    BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(
        checkout, owners, new BuildCachePolicy(), cancellationToken: cancellation);
    return plan.ExecuteAsync(cancellationToken: cancellation);
}
```

此例借用已登记的 owner，不发现或删除任意目录。实际回收时保留当前产物及其依赖；`retiredBytes` 仅表示逻辑文件大小，实际磁盘回收另行测量。


## 职责与边界

平台中立的 SDK 选择契约、工具执行、组件描述、内容指纹、Native 发布与产品部署机制。不探测当前 OS 来选择产品目标。

## Managed 输入快照

ManagedBuildInputSnapshot.CaptureAsync 使用已选 SDK 的实际 MSBuild 进程求值完整 ProjectGraph，并记录源码、资源、Analyzer、引用、编译属性、imports、package inputs 和 SDK reference packs。缺少必要还原图或真实输入会失败；不会用已有 Player DLL 代替源码输入。生成绑定另由选定 generator 的输入契约覆盖。VerifyAsync 重新求值并完整读取，能够发现新增源码、同长度同 mtime 修改。内部求值器是无 Program 的 SDK Task，按源码/SDK 身份发布且持有读取 lease；只有缺失或损坏时才准备该工具，不属于 Player 闭包。

## 组合、生命周期与扩展

`BuildHostDescriptor` 是实际执行工具的宿主；`NativeToolchainSelection` 是已解析的明确目标、绝对工具、SDK 输入、环境及参数。平台 provider 负责 SDK 选择；共同机制只执行冻结选择。

`NativeComponentDescriptor` 显式指定 Native、Toolchain、binding 配置和可选静态构建定义的唯一 owner。不同 checkout 的逻辑位置与物理读取分开。`NativeBuildContext.GetNativeBuildRoot` 不按程序集名称猜目录。

`NativeProductContribution.BuildAsync` 在所有步骤开始前批量准备同一目标的 bindings，再按明确依赖执行组件 recipe。`NativeBindingPreparation` 直接调用借用的 `INativeBindingGenerator`；具体 BGCS 应用流程在 Inno.Build.Bindings，结果按目标/指纹隔离。缺少 profile、工具或 SDK 在候选发布前明确失败。

`NativeArtifactPublisher` 使用完整内容哈希、operation-owned snapshot、写 lease、再次稳定性验证和原子提交。工具、SDK、参数、生成身份、真实 source/executor closure 共同参与指纹。热命中仍校验产物与输入内容，不依赖 mtime。

`NativeBuildRecipe.CreateForComponent` 可以在生成输出尚未存在时声明组件源码、执行代码和 SDK 输入，
不会调用生成器或读取生成目录。`NativeArtifactPublisher` 在计算最终指纹和执行 producer 前，
从当前 operation 取得实际 generation，把 managed binding、Native bridge 的字节和 generation identity
一并冻结到执行 recipe；缺 generation 会在 staging 或 producer 开始前失败。
产品通过 `ProductNativeBuildStep.createRecipe` 与实际 producer 共同注册声明，`ProductNativeBuildPlan.CreateRecipes` 校验 owner、目标和组件闭包。Support Pack 已接入准备前索引；真实产品验收仍在进行。

冷 CMake 生产流程由 `NativeCMakeExecutor.PrepareSourceAsync` 将 recipe 已冻结的工作区输入复制到所属 intermediate 的 `Sources`。`NativeCMakeSource` 只提供该复制树及声明输入的路径解析；原始源码即使在编译期间被修改后恢复，也不会改变编译器读取的内容。复制逐文件核对初始内容哈希，不使用 hardlink。兼容的 workspace 保留未变化文件的时间戳，只原子替换内容变化的文件，并移除已不属于本次输入闭包的 owned 文件。workspace 写入由独占 lease 串行化；最终产物按完整输入指纹单独发布。

`NativeCMakeExecutor.BuildAsync` 必须在 `NativeArtifactPublisher` 的 producer scope 内调用。它自动共用该 scope 的一次准备，解析 `-S`、`INNO_ROOT` 和已声明的 CMake 输入参数；输出安装位置及外部 SDK 保留实际 owner。Browser 在同一 scope 准备一次 source，并用该 descriptor 生成组件 include 与 bridge 路径，随后执行相同 executor。产物缓存命中不进入冷 producer，因而不复制源码；锁后及编译后的 fresh 原始输入校验仍保留。

选中的生成绑定输入在复制树中使用 Native 项目 owner 对应的稳定位置。绑定 generation 变化会更新对应输入内容，最终产物仍使用不同指纹；兼容 workspace 的 bridge 路径不会仅因生成缓存目录名变化而改变。workspace 身份只对实际选中的生成根及其 CMake 路径参数做这种规范化，其他 SDK、ABI、工具和配置参数继续区分 workspace。该映射属于内部实现，不是新的公开缓存入口。

Windows 编译工具的短路径遵守宿主选择的临时目录；实际执行根参与 workspace 身份，CMake 不会把另一个执行根生成的绝对路径缓存当作可复用中间态。不同 TEMP 的进程可以复用验证通过的不可变最终产物，但分别维护路径兼容的编译工作区。真实文件仍由组件 `obj/native` 持有，alias 仅在操作期间存在，执行锁与 alias 一起保留在临时根中，不能写入不可变产物。其他宿主直接使用真实目录。该策略由共同工具链处理，IDE 和 CLI 不需要使用相同临时目录。

`NativeBuildRecipe.CreateForComponent` 要求组件通过 `implementationPaths` 声明实际执行源码；共同执行代码由共享机制加入，未选择的平台配置不自动进入闭包。Browser 聚合只包含聚合实现与产物描述，Support Pack、打包和 Target 代码不参与原生归档指纹。共用 executor、组件 recipe、SDK、生成桥与参数继续参与失效；缩小无关范围不削弱发布前稳定性校验。

`ProductNativeDeployment` 从 plan 的明确布局生成 exact deployment；相同内容不替换加载中的 DLL。失败/取消保留旧完整部署，提交后清理失败独立报告。工具工作目录 alias 和文件权限属于内部执行机制，不能决定输出目标。

`StageAsync` 用于调用方已经独占的未发布候选：要求候选目录已存在且没有 `native` 子树，复制与校验复用同一部署实现，不在候选中创建锁、claim 或第二次发布事务。失败或取消时调用方必须废弃整个候选。Support Pack 使用此入口，让外层发布者独占事务；普通应用输出继续使用 `InstallAsync` 原子替换。

托管子进程通过 `DotNetSdkEnvironment.Create` 取得只作用于子进程的环境。它保留所选 Native SDK 环境，移除父 IDE/MSBuild 注入的 SDK resolver、MSBuild 程序集与扩展根，使所选 dotnet 根据被调用工程的 `global.json` 解析 SDK。共同进程执行器以 `null` 表示移除继承变量，以空字符串表示显式空值；不修改父进程，也不要求所有消费者锁定同一个 SDK。

`DotNetSdkResolver` 先经现有 `ResolveExecutable` 将调用者选择的 host 冻结为绝对路径，再在原工程目录解析一次 SDK，记录实际 Base Path 与 managed CLI 入口 `DotNetSdkDescriptor.cliPath`。发布执行使用相同的绝对 host 和冻结入口，不重新查询 PATH，也不从临时 Windows alias 的父目录选择 SDK。SDK 信息查询只在子进程中固定英文输出，以便严格读取所选身份和位置，不能从宿主安装列表猜最高版本。当前所选 host 或 SDK 入口缺失时明确失败，不复制或改写工程及其父目录的 `global.json`。

BGCS 包族由 `Inno.Packages.props` 固定版本，通过真实 SDK Restore 图取得资产。运行组件引用 BGCS.Runtime；生成器和 Cpp2C 仅属于绑定构建库与生成扩展，不进入 Player 的运行闭包。包内容哈希、实际生成器程序集和解析所需 Native 资产参与生成身份，不以版本字符串代替完整性校验。

共同 MSBuild 规则向实际产品引用传递明确目标和 Shader 配置；Analyzer 移除产品及发布属性，始终作为宿主工具构建。NuGet Runtime 资产按所选 RID/目标的实际 Restore 图部署，不建立源码 Runtime 的独立编译入口。

普通 IDE 产品输出保留产品项目的 `bin/<target>/<configuration>` 与 `obj/<target>`，方便启动和调试；它们是可替换的 IDE 输出，不作为不可变缓存。Native、bindings、Support Pack 与按游戏闭包发布的托管中间态采用明确目标和指纹隔离，部署前校验匹配闭包。最终游戏目录由 BuildProfile 指定。

```csharp
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Inno.Build.Toolchains;

static Task<ProductNativeBuildResult> Prepare(
    ProductNativeBuildPlan plan,
    NativeBuildContext selected,
    CancellationToken cancellation
) {
    return new ComponentNativeProductExecutor().BuildAsync(selected, plan, cancellation);
}
```

扩展 SDK 实现 `INativeToolchainProvider`，组件提供自己的 descriptor、recipe 与 CMake 定义；具体发行负责选择。没有固定内置组件列表或额外生产 Program。验证包括内容篡改、并发、取消、冷 CMake、绑定/export 一致性及产品热构建测量。

## 显式组件配置

NativeBuildContext.WithComponentOptions 固定对应 NativeComponentDescriptor 与 NativeComponentBuildOptions；ProductNativeBuildStep 必须提供它。libraryKind、有序 CMake definitions、输入文件 bytes 与目标、SDK、binding generation 共同参与 recipe identity。NativeStaticBuildDefinition 声明逻辑归档名称与完整源码输入；平台 NativeLinkPlan 提供实际归档格式。执行器与 Static/Shared 分别选择，未实现的组合在生成和 staging 前拒绝。

静态 recipe 的 `nativeSourceDirectory` 只在实际消费手写 Native CMake 片段时声明，并且必须包含已声明输入。纯 C 组件可以不提供该目录；聚合器按此契约传入选定副本，不根据组件名推导或创建空源码目录。

## 中立绑定与阶段校验

NativeBuildContext.WithBindingGenerator 借用 INativeBindingGenerator；派生 context 共用 provider、operation 输入状态和统计。NativeBindingPreparation 为完整 Product 请求一次批次，不生成临时 MSBuild 工程。BeginInputVerification 明确开启 fresh phase，阶段内部去重物理读；各独立锁等待和编译后必须重新验证。NativeBuildProduct.bindingGeneration 记录实际 Native 编译的绑定，WriteSelection 同时冻结每个实际组件的 Static/Shared 请求，供托管初始化和 Native 发布共同校验；静态组件只在 Native 编译边界定义 INNO_STATIC_NATIVE，不从宿主或目标名称推导。统计区分 source/verification/output、binding batch/generation、Native/managed 进程及 materialization bytes。完整性成本仍存在，不能称为零 IO。

## 并发托管输出归属

独立构建请求通过 SDK `--artifacts-path <request>/<target>` 各自持有托管 bin/obj；共同属性尊重该明确选择。独立 interop 编译和还原也采用同一请求根。普通 IDE bin/obj 只属于一个活动构建，两个独立进程同时写同一路径不受支持。Native 内容寻址缓存仍共享，写入继续由组件 lease 和阶段验证协调。

## 轻量维护与工具发布

BuildCacheOwnerCatalog 由实际发行登记 publisher 根、ID 和生命周期，不扫描目录推测 owner，也不把未登记 generation 纳入删除权限。BuildMaintenanceApplication 读取这些声明，并复用 BuildCacheCleanupPlan 的完整依赖保护和退休协议；首次 clone 没有登记时可安全得到空计划。活动 reader、进程和未知状态继续保护产物。


缓存清理的 entry 路由仅准备 Toolchains 及 Foundation 闭包，不加载完整 CLI、BGCS 生成器、Editor 或第三方后端。产品清理需要产品预检，仍属于完整 Build 命令。



## 缓存管理公开契约


| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-220a964fd3674ec8"></a>[`public static Inno.Build.Toolchains.BuildCacheCleanupPlan Inno.Build.Toolchains.BuildCacheCleanupPlan.Create(string workspace, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.BuildCacheOwnerDescriptor> owners, Inno.Build.Toolchains.BuildCachePolicy policy, bool retireAll = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheCleanupPlan.cs#L84) | 明确注册 owner 的物理边界、协调路径和借用的清单/保护根 callbacks。 | Callbacks 在本次清理期间有效；发布及根更新使用同一协调 lease。 | 不得通过目录名猜 owner；拒绝重复、越界或重叠注册。 |

### 进程加载保护


| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |


## 进程与构建所有权 API

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dc5639d1f9e77581"></a>[`public Inno.Build.Toolchains.BuildCacheArtifactDescriptor.BuildCacheArtifactDescriptor(string directory, string leasePath, string? targetId, long bytes, System.DateTime lastUseUtc, System.Collections.Generic.IReadOnlyList<string> dependencies, Inno.Build.Toolchains.BuildCacheArtifactRole role)`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactDescriptor.cs#L48) | 冻结目录、目标、角色、协调路径、字节、UTC 使用时间与依赖；大小只服务预算。 | 描述不接管 producer 或 reader lease。 | 绝对路径、非负大小、UTC 时间和有效角色必须成立。 |
| <a id="api-e8ab234735aee5cd"></a>[`public Inno.Build.Toolchains.BuildCacheArtifactRole Inno.Build.Toolchains.BuildCacheArtifactDescriptor.role { get; }`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactDescriptor.cs#L104) | Gets the declared lifetime used for retirement and preferred-workspace accounting. | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-2de77262d10ca4de"></a>[`public enum Inno.Build.Toolchains.BuildCacheArtifactRole : int`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactRole.cs#L6) | 明确区分不可变产物、可复用 workspace 与进程临时目录。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-50d01ce49dccb60c"></a>[`Inno.Build.Toolchains.BuildCacheArtifactRole.ImmutableArtifact = 0`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactRole.cs#L11) | 完整产物由当前产品/Pack/活动读取保护。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-f1ce6b608ef92971"></a>[`Inno.Build.Toolchains.BuildCacheArtifactRole.ReusableWorkspace = 1`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactRole.cs#L16) | 编译活动时保护；无活动时可按预算退休，即使最近使用。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-24a0b626c080cf61"></a>[`Inno.Build.Toolchains.BuildCacheArtifactRole.ProcessWorkingDirectory = 2`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCacheArtifactRole.cs#L21) | 进程活动或 Unknown 时保护，退出后可退休。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-bc1b00bfb648b5d9"></a>[`public Inno.Build.Toolchains.BuildCachePublisher.BuildCachePublisher(string directory, Inno.Build.Toolchains.BuildCacheArtifactRole role)`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCachePublisher.cs#L34) | 为明确 cache 根选择生命周期，之后通过 Describe 提供登记与保护关系。 | 构造不取得 lease；发布/读取交接由调用方持有 activity 与产物保护。 | 根必须绝对且不越界；不认领未登记目录。 |
| <a id="api-4a7656fc5be23ae0"></a>[`public void Inno.Build.Toolchains.BuildCachePublisher.BeginWorkingDirectory(string directory, string leasePath, string? targetId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../build/toolchains/Inno.Build.Toolchains/Cache/BuildCachePublisher.cs#L168) | 调用者取得 activity 与 producer 独占 lease 后，先登记 reusable workspace，再允许 payload 写入。 | lease 仍由调用方持有；不永久固定闲置 workspace。 | 已有无登记目录拒绝认领；目标归属按显式参数记录。 |
| <a id="api-c92ebb85860823eb"></a>[`public Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.NativeBuildRecipe.WithDependency(Inno.Build.Toolchains.NativeBuildInput input, string fingerprint)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L190) | 把实际依赖产物和指纹冻结进后端 recipe，保留选定工具；依赖产品仅在 recipe 构造期间借用。 | 不转移 NativeBuildProduct 的读取 leases。 | 没有声明转换的依赖步骤走正常冷路径，不能猜测链接参数。 |
| <a id="api-5d14520833bd34b0"></a>[`public Inno.Build.Toolchains.ProductNativeBuildStep.ProductNativeBuildStep(string id, Inno.Build.Toolchains.NativeComponentDescriptor component, Inno.Build.Toolchains.NativeComponentBuildOptions options, System.Collections.Generic.IReadOnlyList<string> dependencies, System.Func<Inno.Build.Toolchains.NativeBuildContext, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBuildProduct>, System.Threading.CancellationToken, System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct>>? build, System.Func<Inno.Build.Toolchains.NativeBuildContext, Inno.Build.Toolchains.NativeBuildRecipe>? createRecipe, System.Func<string, string, string?> deploymentPath, string? configuration = null, System.Func<Inno.Build.Toolchains.NativeBuildContext, Inno.Build.Toolchains.NativeBuildRecipe, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBuildProduct>, Inno.Build.Toolchains.NativeBuildRecipe>? bindDependencies = null)`](../../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L52) | Freezes a component step without embedding any backend list in the shared executor. | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-773f90ed800edf86"></a>[`public System.Func<Inno.Build.Toolchains.NativeBuildContext, Inno.Build.Toolchains.NativeBuildRecipe, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBuildProduct>, Inno.Build.Toolchains.NativeBuildRecipe>? Inno.Build.Toolchains.ProductNativeBuildStep.bindDependencies { get; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L134) | 把实际依赖产物和指纹冻结进后端 recipe，保留选定工具；依赖产品仅在 recipe 构造期间借用。 | 不转移 NativeBuildProduct 的读取 leases。 | 没有声明转换的依赖步骤走正常冷路径，不能猜测链接参数。 |

<!-- api: M:Inno.Build.Toolchains.BuildCacheArtifactDescriptor.#ctor(System.String,System.String,System.String,System.Int64,System.DateTime,System.Collections.Generic.IReadOnlyList{System.String},Inno.Build.Toolchains.BuildCacheArtifactRole) f6fa379b2d3a8d95bcfaca291db9ab151a8a523c085364c8c2c1646d08cf7540 -->
<!-- api: P:Inno.Build.Toolchains.BuildCacheArtifactDescriptor.role cb049d26c15256513dbaf3bb11e16505bc736370f2edcd516d9ac894c8dc15a0 -->
<!-- api: T:Inno.Build.Toolchains.BuildCacheArtifactRole 0f4d809a33201aac2e28281b3d990ca619777ec60963580147dda374b73f8dd9 -->
<!-- api: F:Inno.Build.Toolchains.BuildCacheArtifactRole.ImmutableArtifact fff47891a0b6f754becbc993baa9c78a386220e3c8a621819a770c4def35825b -->
<!-- api: F:Inno.Build.Toolchains.BuildCacheArtifactRole.ReusableWorkspace 760b2a8556bf470eb5dfa726596dfdb559859dfaedf7978ff9e54353764c0d57 -->
<!-- api: F:Inno.Build.Toolchains.BuildCacheArtifactRole.ProcessWorkingDirectory d1f4c3d78264b2ff619a7f6cf9fcd53707151d7ae868a45659ce5a749582f5dc -->
<!-- api: M:Inno.Build.Toolchains.BuildCachePublisher.#ctor(System.String,Inno.Build.Toolchains.BuildCacheArtifactRole) 2f20e13b755bd1c1e202c1246eacd36a0861fe73eef7f01f4bc278947e51b58d -->
<!-- api: M:Inno.Build.Toolchains.BuildCachePublisher.BeginWorkingDirectory(System.String,System.String,System.String,System.Threading.CancellationToken) ce4ae7ad4fd81a951e9d352ba38aeb2b078b48b3c5472ecdfeb75e951816f6df -->
<!-- api: M:Inno.Build.Toolchains.NativeBuildRecipe.WithDependency(Inno.Build.Toolchains.NativeBuildInput,System.String) 7773db98dcb83fa9fcb29fd27face79f19c631c75f49d8e2c8ba0b164ea2bd6e -->
<!-- api: M:Inno.Build.Toolchains.ProductNativeBuildStep.#ctor(System.String,Inno.Build.Toolchains.NativeComponentDescriptor,Inno.Build.Toolchains.NativeComponentBuildOptions,System.Collections.Generic.IReadOnlyList{System.String},System.Func{Inno.Build.Toolchains.NativeBuildContext,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Build.Toolchains.NativeBuildProduct},System.Threading.CancellationToken,System.Threading.Tasks.Task{Inno.Build.Toolchains.NativeBuildProduct}},System.Func{Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildRecipe},System.Func{System.String,System.String,System.String},System.String,System.Func{Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildRecipe,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Build.Toolchains.NativeBuildProduct},Inno.Build.Toolchains.NativeBuildRecipe}) 94819c6bc8d87e011092bc38691d4fc953eb94722ec9a51f6c5a84357377f5da -->
<!-- api: P:Inno.Build.Toolchains.ProductNativeBuildStep.bindDependencies e484ffc25dffb84e5d9d8d1534268c24c2b91eacf75ce15191d56e8367e33017 -->

有效 MSBuild 图的节点按项目路径与完整 global properties 排序，避免同路径多配置节点的无序枚举造成错误失效。参数、include 与编译项顺序继续参与身份；验证失败会报告文件或声明变化。内部 capture 只服务诊断，不构成新的公开缓存权威。

工具 publication 在固定输入前，使用同一 SDK 还原入口项目及 InnoBuildInputProject 声明的辅助输入项目。辅助项目来自有效图，清理的 global properties 保持一致；Capture/Verify 本身仍只读取，不通过缺 assets 的 fallback 掩盖错误。

### 输入路径别名与创作归属

同一校验阶段按最终物理路径去重哈希，但保留每次读取的原路径和 checkout 相对逻辑身份。macOS 的 `/var` 别名、目录链接与 Windows junction 不会把所属源码误记成外部 SDK 输入；固定编译副本仍按所属源码布局准备。独立锁等待、编译及提交后重新校验，不跨阶段复用内容结论。

## 原生生成编译契约 API

<!-- api: T:Inno.Build.Toolchains.NativeBindingCompilation 41474d8fb3c410d67959afaaa0a0e03bce1dfedccef6fc0ccbb5ec66085133a8 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.roots 32502591ef968e64945c8e2db9960f3a3f36fc4b1cf833d059f10e90a827aa7b -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.files 4d8890d75c5fbd99d816750d30f7fb9f7e02fe8f1fb4547ba3b9120cb213a040 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.includeDirectories 0b8503efa23b6d1cf903350d8ee609b3d77dc325814311ed6bf062c69155894e -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.defines 5a92d8c7c3824238be147aca94102f33299b5cad80794a009f76892e37c4a6b1 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.compilerDefines 6fce30e385b94818f5dec6f7ae4e34ec0e85b3447755826db74540634613f579 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.cxxStandard 1017028e0c7e12a790160633ea0fd2d7e9c65a90cd12e880bd23dfca361feecc -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.cStandard 5a77a5b63cae20df6782ba3c5c9dfca3b3d3a83457f3e1f4f3eca8f450490ac5 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.requiredExports 1a9480d6e47e4c485265b13ad5c37b0af219bcbf85b8a696026f8d20e6d6cbc9 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.staticExportSymbols b98e6d03c90c18f504d021118b76a5e04c5e01010a0d28cfe7512bff93e497c9 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.staticExportAbi 1ff77b1da8e927283ce7b4665ff5b794f7fefbdff6f2b8633720ad8fcd4d82d3 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingCompilation.staticExportNames 2b6c8363d2b4723086be2c017a21ddde6b20826bc5a6f6dc95924cc1c436a053 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingCompilation.Load(System.String) 1c7c7291da6b88fc6b7f310578a59432074b656da8af7124859b1fea7ef865ad -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingCompilation.ResolveRoot(System.String,System.String,System.String) 675817642ee13c1052f9a1addc5cffea835e2266c8578142e5d7b9714d96ca93 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingCompilation.VerifyInputs(System.Func{System.String,System.String},System.Threading.CancellationToken) 11c3c2bda073c2d197112e84a8717dca408249de321eada607cf298b34a72e79 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingCompilation.GetInputs(System.String,System.String) f447249eac74c2bdb4afa72cfedefcb3a9442222f44a3a185aa56c529b89d343 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingCompilation.#ctor a5ed398916d6074923d7171fbd32c6ab252752d63a1c5754c10bb0554c2c3a0b -->
<!-- api: T:Inno.Build.Toolchains.NativeBindingIncludeDirectory 1a72d09557dce75624f018f269dfd5da88d8fffb3ce77cbff8f5f2be3c0fef52 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.#ctor(System.String,System.String,System.Boolean,System.Boolean) d08afa54a5e67676e2338d30cac64c903cd45b3dbd8f0dae3ded5802bdf42750 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingIncludeDirectory.rootId 073be2a8896e71c7cd550dda9153f12cb22496be82eca953969e872dee49da68 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingIncludeDirectory.relativePath dd31a936343e9e473a4d869474a89d09ca1b6d631521e709850ad02f689b8d80 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingIncludeDirectory.system 9703b908c76a5f61ade9ca96c912c5b45a669bea24b8c0c118fb3ff727f7a73c -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingIncludeDirectory.framework 6d8fb8f23fd093a59c9a717cef12977502c28ce483e08baccd57c3d1bf535361 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.ToString f77a056371bf95d1ac95232203d33f019f172d1f332964185c0df7a48f735db6 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.op_Inequality(Inno.Build.Toolchains.NativeBindingIncludeDirectory,Inno.Build.Toolchains.NativeBindingIncludeDirectory) 1fa86ff74cc6f3a529b763a6647f89572ccd506926776a67d5560ac1ccd9cbf7 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.op_Equality(Inno.Build.Toolchains.NativeBindingIncludeDirectory,Inno.Build.Toolchains.NativeBindingIncludeDirectory) 5453401503d418e5261af446112186f960b738d704384b9e70fc1418a121164e -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.GetHashCode d85359342b378c8d6e0bbe18bed61f0e653ecace7d38d18a6761f2c150bc703f -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.Equals(System.Object) d46933f10dcb62d0e757c7d50bf5d2280c118389a32c41839334d954b81bab7a -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.Equals(Inno.Build.Toolchains.NativeBindingIncludeDirectory) 24c8a36d28bacd354aeb43ea5971142b74f37ce69fa4913f4c6e14b19fb426ad -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingIncludeDirectory.Deconstruct(System.String@,System.String@,System.Boolean@,System.Boolean@) 1f1ab7b41b2450c297a1a31952793eed3e27a68fcec0a9d834e38745dae2b5b1 -->
<!-- api: T:Inno.Build.Toolchains.NativeBindingSourceFile 914a67960cbcfd94e8b7bcbe8566538f457e428e88482df246bbb213a557b143 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.#ctor(System.String,System.String,System.Int64,System.String,System.Boolean) fe749dffd12ffd0d25edf076727c2799e7e068468e0624ef5c84042babadb197 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceFile.rootId f6111adda7f8f904f7077c4f2c0d0a78de280c39704fb5c34c85069a9d02f522 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceFile.relativePath 886e51cf36fb6f623be0c435d4a75917138c6d5c4bd0e2ee74ccf97f652a0b69 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceFile.length 2d3e246fe21fde4e588aaeca9c7efb8d74a32606c692ee5345bb8f0232962608 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceFile.sha256 0e76db777a004dcfe0c1fe19749974348ec60ad1d8227110cec889eef8a4aaf4 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceFile.compile b6de83707955d12c54a2ac183257a1b133b4d9f682b8476e4a364adfebc8fa14 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.ToString 9a778427df2fcb0113dd24c918d33a025118dd735d7af7b61806e195115ffd28 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.op_Inequality(Inno.Build.Toolchains.NativeBindingSourceFile,Inno.Build.Toolchains.NativeBindingSourceFile) e5c6fb21f74c6eaa8a4375ad1e40e8427dad058436516473e4c413f1ccfeecce -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.op_Equality(Inno.Build.Toolchains.NativeBindingSourceFile,Inno.Build.Toolchains.NativeBindingSourceFile) a19c677a793d722ae623ab255a07892ee53c0afc0433a53f1cc2211fd4c2780a -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.GetHashCode c0bfcd4facddb82b98735ef447ca2aba3a9966ab6120a6ab507647280c5e6ee7 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.Equals(System.Object) 4d6c45ef068efc66dea56b683e8d3418fe3f84bb30dc9a1460e8dd5edb8bf10d -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.Equals(Inno.Build.Toolchains.NativeBindingSourceFile) 627e7d26c377b7568615f78489b34b9793cc3ce7c99887e20a99107600d9c770 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceFile.Deconstruct(System.String@,System.String@,System.Int64@,System.String@,System.Boolean@) 747b86baf4c0273f5903f7be4fcc901fcb1e6b87487877e51b2e09cc0e97e836 -->
<!-- api: T:Inno.Build.Toolchains.NativeBindingSourceRoot 590f9ff139b0f1caf13a58d048baf0393619357b3f8461320298efe6fe35a452 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.#ctor(System.String,System.String,System.Boolean,System.Boolean) a3bcc94e777b65c74e9b75b9525b210b48c9c1cc688fac99bb7ca05f3a34a7dc -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceRoot.id 122ceab8e1594d0f25e5c104ef3d7052c96d9ea7d436a9e8231133f45ad7a04d -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceRoot.directory 07ce781cefac8248b20d7cd62cf33e8972c7374deeab1eb3f106602bd259b4c4 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceRoot.generated 4e8f925ddb7ab4efda2499a079a64701c44fdb8705866ccbd45a7efc3b7b91d5 -->
<!-- api: P:Inno.Build.Toolchains.NativeBindingSourceRoot.frozen 761cce9a5561bd89461d007051df286be66b9f89d96e0dfbf9606661a3969170 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.ToString 3a8029db56614b08d2219355808972d131873e27fb0b6882013bfbdd9fed3c7a -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.op_Inequality(Inno.Build.Toolchains.NativeBindingSourceRoot,Inno.Build.Toolchains.NativeBindingSourceRoot) a87295fc32ab02d6e291cd183a1c523f37b74ebe1c69a4a676e7ae25fa9f2416 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.op_Equality(Inno.Build.Toolchains.NativeBindingSourceRoot,Inno.Build.Toolchains.NativeBindingSourceRoot) ee2de049643a632db50aad1520ce8678667012adc46f10d73f677695ca2e62d9 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.GetHashCode 9a1188b0d254d2d9d81fcd0f80187f9ef0498bcab3604c4d4afcfb008a0663f9 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.Equals(System.Object) 03f57cec59cac68524284d1f6977c9bacfe48c34fda19ce12585f37fe1761005 -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.Equals(Inno.Build.Toolchains.NativeBindingSourceRoot) 48aae4a62296cb666f6ac405b6b49ca0b0a08e76e762dcd0e4fa8c741f1f11cd -->
<!-- api: M:Inno.Build.Toolchains.NativeBindingSourceRoot.Deconstruct(System.String@,System.String@,System.Boolean@,System.Boolean@) da81058ec205771bffc659eb1c8a417ab2940a671088e83bd6ecdd8bac368c14 -->

| 当前签名 | 语义 |
| --- | --- |
| [`public sealed class Inno.Build.Toolchains.NativeBindingCompilation`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L16) | Describes a selected component's native requirements independently of its source generator. |
| [`public required Inno.Build.Toolchains.NativeBindingSourceRoot[] Inno.Build.Toolchains.NativeBindingCompilation.roots { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L27) | Gets root bindings whose identities are local to this component. |
| [`public required Inno.Build.Toolchains.NativeBindingSourceFile[] Inno.Build.Toolchains.NativeBindingCompilation.files { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L32) | Gets all exact input files; only files marked compile are translation units. |
| [`public required Inno.Build.Toolchains.NativeBindingIncludeDirectory[] Inno.Build.Toolchains.NativeBindingCompilation.includeDirectories { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L37) | Gets include requirements in their original search order. |
| [`public required string[] Inno.Build.Toolchains.NativeBindingCompilation.defines { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L42) | Gets ordered definitions for manifest translation units; upstream target configuration remains component-owned. |
| [`public required string[] Inno.Build.Toolchains.NativeBindingCompilation.compilerDefines { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L47) | Gets parser target macro facts; these must not override the consuming compiler's builtins. |
| [`public required int Inno.Build.Toolchains.NativeBindingCompilation.cxxStandard { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L52) | Gets the minimum C++ standard, or zero when no C++ translation unit is required. |
| [`public required int Inno.Build.Toolchains.NativeBindingCompilation.cStandard { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L57) | Gets the minimum C standard, or zero when no C translation unit is required. |
| [`public required string[] Inno.Build.Toolchains.NativeBindingCompilation.requiredExports { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L62) | Gets the exact native symbols that the component must provide. |
| [`public required string[] Inno.Build.Toolchains.NativeBindingCompilation.staticExportSymbols { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L67) | Gets static table entry points, or an empty array for dynamic-only bindings. |
| [`public required string Inno.Build.Toolchains.NativeBindingCompilation.staticExportAbi { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L72) | Gets the static table ABI digest, or an empty value when no static table exists. |
| [`public required string[] Inno.Build.Toolchains.NativeBindingCompilation.staticExportNames { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L77) | Gets static table entries in address-resolution order. |
| [`public static Inno.Build.Toolchains.NativeBindingCompilation Inno.Build.Toolchains.NativeBindingCompilation.Load(string nativeDirectory)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L97) | Reads the current engine-owned compilation description from a selected native output. |
| [`public string Inno.Build.Toolchains.NativeBindingCompilation.ResolveRoot(string engineRoot, string nativeDirectory, string rootId)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L124) | Resolves a component-local root using the current checkout and selected output. |
| [`public void Inno.Build.Toolchains.NativeBindingCompilation.VerifyInputs(System.Func<string, string> resolveRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L152) | Verifies the exact bytes at original or remapped roots before their use. |
| [`public System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.NativeBuildInput> Inno.Build.Toolchains.NativeBindingCompilation.GetInputs(string engineRoot, string nativeDirectory)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L179) | Enumerates exact file declarations for standalone or aggregate recipe fingerprinting. |
| [`public Inno.Build.Toolchains.NativeBindingCompilation.NativeBindingCompilation()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingCompilation.cs#L16) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public sealed record Inno.Build.Toolchains.NativeBindingIncludeDirectory : System.IEquatable<Inno.Build.Toolchains.NativeBindingIncludeDirectory>`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Preserves the order and search semantics of one native include requirement. |
| [`public Inno.Build.Toolchains.NativeBindingIncludeDirectory.NativeBindingIncludeDirectory(string rootId, string relativePath, bool system, bool framework)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Preserves the order and search semantics of one native include requirement. |
| [`public string Inno.Build.Toolchains.NativeBindingIncludeDirectory.rootId { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L19) | The component-local input root. |
| [`public string Inno.Build.Toolchains.NativeBindingIncludeDirectory.relativePath { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L20) | The root-relative directory, or empty for the root itself. |
| [`public bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.system { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L21) | Whether this directory contains system headers. |
| [`public bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.framework { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L22) | Whether the compiler must use framework search semantics. |
| [`public override string Inno.Build.Toolchains.NativeBindingIncludeDirectory.ToString()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Returns a string that represents the current object. |
| [`public static bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.operator !=(Inno.Build.Toolchains.NativeBindingIncludeDirectory? left, Inno.Build.Toolchains.NativeBindingIncludeDirectory? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public static bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.operator ==(Inno.Build.Toolchains.NativeBindingIncludeDirectory? left, Inno.Build.Toolchains.NativeBindingIncludeDirectory? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public override int Inno.Build.Toolchains.NativeBindingIncludeDirectory.GetHashCode()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Serves as the default hash function. |
| [`public override bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.Equals(object? obj)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Determines whether the specified object is equal to the current object. |
| [`public bool Inno.Build.Toolchains.NativeBindingIncludeDirectory.Equals(Inno.Build.Toolchains.NativeBindingIncludeDirectory? other)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | Indicates whether the current object is equal to another object of the same type. |
| [`public void Inno.Build.Toolchains.NativeBindingIncludeDirectory.Deconstruct(out string rootId, out string relativePath, out bool system, out bool framework)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingIncludeDirectory.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public sealed record Inno.Build.Toolchains.NativeBindingSourceFile : System.IEquatable<Inno.Build.Toolchains.NativeBindingSourceFile>`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Identifies exact generated or external bytes needed by a native compilation. |
| [`public Inno.Build.Toolchains.NativeBindingSourceFile.NativeBindingSourceFile(string rootId, string relativePath, long length, string sha256, bool compile)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Identifies exact generated or external bytes needed by a native compilation. |
| [`public string Inno.Build.Toolchains.NativeBindingSourceFile.rootId { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L22) | The component-local input root. |
| [`public string Inno.Build.Toolchains.NativeBindingSourceFile.relativePath { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L23) | The portable root-relative filename. |
| [`public long Inno.Build.Toolchains.NativeBindingSourceFile.length { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L24) | The expected byte length. |
| [`public string Inno.Build.Toolchains.NativeBindingSourceFile.sha256 { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L25) | The lowercase SHA-256 content identity. |
| [`public bool Inno.Build.Toolchains.NativeBindingSourceFile.compile { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L26) | Whether this file is a translation unit rather than an include dependency. |
| [`public override string Inno.Build.Toolchains.NativeBindingSourceFile.ToString()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Returns a string that represents the current object. |
| [`public static bool Inno.Build.Toolchains.NativeBindingSourceFile.operator !=(Inno.Build.Toolchains.NativeBindingSourceFile? left, Inno.Build.Toolchains.NativeBindingSourceFile? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public static bool Inno.Build.Toolchains.NativeBindingSourceFile.operator ==(Inno.Build.Toolchains.NativeBindingSourceFile? left, Inno.Build.Toolchains.NativeBindingSourceFile? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public override int Inno.Build.Toolchains.NativeBindingSourceFile.GetHashCode()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Serves as the default hash function. |
| [`public override bool Inno.Build.Toolchains.NativeBindingSourceFile.Equals(object? obj)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Determines whether the specified object is equal to the current object. |
| [`public bool Inno.Build.Toolchains.NativeBindingSourceFile.Equals(Inno.Build.Toolchains.NativeBindingSourceFile? other)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | Indicates whether the current object is equal to another object of the same type. |
| [`public void Inno.Build.Toolchains.NativeBindingSourceFile.Deconstruct(out string rootId, out string relativePath, out long length, out string sha256, out bool compile)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceFile.cs#L21) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public sealed record Inno.Build.Toolchains.NativeBindingSourceRoot : System.IEquatable<Inno.Build.Toolchains.NativeBindingSourceRoot>`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Identifies one component-local root without exposing a generator-specific contract. |
| [`public Inno.Build.Toolchains.NativeBindingSourceRoot.NativeBindingSourceRoot(string id, string directory, bool generated, bool frozen)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Identifies one component-local root without exposing a generator-specific contract. |
| [`public string Inno.Build.Toolchains.NativeBindingSourceRoot.id { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L19) | An ordinal identity scoped to this component's selected generation. |
| [`public string Inno.Build.Toolchains.NativeBindingSourceRoot.directory { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L20) | The checkout-relative or absolute external location; empty for generated output. |
| [`public bool Inno.Build.Toolchains.NativeBindingSourceRoot.generated { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L21) | Whether this root denotes the selected native output directory. |
| [`public bool Inno.Build.Toolchains.NativeBindingSourceRoot.frozen { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L22) | Whether source files must be copied before compilation. |
| [`public override string Inno.Build.Toolchains.NativeBindingSourceRoot.ToString()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Returns a string that represents the current object. |
| [`public static bool Inno.Build.Toolchains.NativeBindingSourceRoot.operator !=(Inno.Build.Toolchains.NativeBindingSourceRoot? left, Inno.Build.Toolchains.NativeBindingSourceRoot? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public static bool Inno.Build.Toolchains.NativeBindingSourceRoot.operator ==(Inno.Build.Toolchains.NativeBindingSourceRoot? left, Inno.Build.Toolchains.NativeBindingSourceRoot? right)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |
| [`public override int Inno.Build.Toolchains.NativeBindingSourceRoot.GetHashCode()`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Serves as the default hash function. |
| [`public override bool Inno.Build.Toolchains.NativeBindingSourceRoot.Equals(object? obj)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Determines whether the specified object is equal to the current object. |
| [`public bool Inno.Build.Toolchains.NativeBindingSourceRoot.Equals(Inno.Build.Toolchains.NativeBindingSourceRoot? other)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | Indicates whether the current object is equal to another object of the same type. |
| [`public void Inno.Build.Toolchains.NativeBindingSourceRoot.Deconstruct(out string id, out string directory, out bool generated, out bool frozen)`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingSourceRoot.cs#L18) | 该 record 的值比较、复制或格式化成员；不持有生成结果的 lease。 |

<!-- api: P:Inno.Build.Toolchains.NativeBindingGenerationDescriptor.usesRuntimeImports 35804cffee9de4ea5be942b40efba7e3ddf2a596e39d70ca29052649190d951b -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-7b0de461c3eabf59"></a>[`public required bool Inno.Build.Toolchains.NativeBindingGenerationDescriptor.usesRuntimeImports { get; init; }`](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L99) | Gets whether the generated context uses runtime-resolved imports instead of a module provider. This initialization choice is independent of static or shared native linkage. |

## API 参考

[完整声明、行为与调用约束](Inno.Build.Toolchains/api.md)

<!-- api-include: Inno.Build.Toolchains/api.md -->

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Core.IO` | 公开签名依赖 |

<!-- api: T:Inno.Build.Toolchains.ManagedProjectGraphCapture a22e24db053bedc612d43cb42774802f045c86aeac1cad89283794470f4d169b -->
<!-- api: M:Inno.Build.Toolchains.ManagedProjectGraphCapture.CaptureAsync(System.String,System.String,System.Collections.Generic.IReadOnlyList{System.String},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String},System.Threading.CancellationToken,System.Boolean) 207a40418a4761b0d96675232421a3d98e0a9191f2a11d4172a067b712c8225a -->
<!-- api: T:Inno.Build.Toolchains.ProductNativePreparation 70d9105cbdf791c2b87ceb8bebbe0e9c90882b7a23174ededd45087aca29a0a9 -->
<!-- api: M:Inno.Build.Toolchains.ProductNativePreparation.FreezeAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,System.Threading.CancellationToken) 1f9c78f6949ad63810b4fdb2be8c1ae66c96d576029b12b73a6ba89487e19ba7 -->
<!-- api: M:Inno.Build.Toolchains.ProductNativePreparation.Capture(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildResult,System.String) 16f8422ba8f05ed588a00cf1de13d03cac8a3e7e01cb810af951af338d4eedb2 -->
<!-- api: M:Inno.Build.Toolchains.ProductNativePreparation.TryAcquireAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,System.String[][],System.String,System.Threading.CancellationToken) ff10e3a2521861ecffabeb17794fa7ff22bb05c0882d302517d4c2774c3de270 -->
