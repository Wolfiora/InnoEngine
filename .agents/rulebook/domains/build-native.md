# domains/build-native

按完整规则单元加载；路由与来源见 [规则登记](../coverage.json)。

<!-- rule-unit: build-host -->
<a id="build-host"></a>
## 25. 宿主、平台目标与统一构建

- 共享 Foundation、Shell、Player Runtime 不通过 OperatingSystem.IsBrowser 判断能力。平台入口注入帧调度、模块激活、存储、日志和线程策略；Input 使用同一 Core Events 入口。
- Native 每组件只有一个项目，共同 BGCS 声明与 target profile 分开。Host 单文件位于 Generated/Bindings.cs，目标 managed 单文件位于artifacts/bindings/<component>/<target>/<generationFingerprint>/Generated/Bindings.cs；CMake 中间产物仍归对应 toolchain 的 obj/native。不同目标与生成身份不能覆盖彼此输出。
- BGCS 生成的 ABI carrier 已明确封送；所有绑定组件通过共同 MSBuild 规则启用 DisableRuntimeMarshalling，禁止让运行时重复解释声明或在各组件重复 assembly attribute。验收检查实际参数、返回与回调，不能仅检查 sizeof。
- Cpp2C 目标桥使用同一 artifacts/bindings/<component>/<target>/<generationFingerprint>/Native，与 managed 单文件经过共同 staging 验证和原子提交，不能覆盖宿主 Native/Generated。目标工具链读取当前请求的生成描述，通过 CMake 参数选择桥；Emscripten SDK/Cache/Node/Python 必须以目标项目的 MSBuild workload 选择为准，不从已安装包中猜测最高版本。
- 开发可执行项目只属于 entry 的 InnoEngine.Editor 与 InnoEngine.Build。Inno.Build.Cli 是共享应用库；组件工具链、Shader 编译、Support Pack 事务和架构验证继续作为库组合，MSBuild 使用薄 Task 适配。
- Editor 的普通 IDE/MSBuild 构建与 Publish 必须通过同一 Native 工具链和部署契约准备当前配置的完整原生闭包；构建失败不得报告可运行。运行时 Loader 只加载明确部署的文件，不承担构建、仓库缓存探测或旧符号兼容。内容完全一致的部署应保持原文件，避免重复复制或替换正在使用的 DLL。
- Support Pack 核心通过 IPlayerSupportPackSource 与 IPlayerSupportPackValidator 注册目标，不维护具体平台分支。Inno.Build.Composition 只接收中立绑定贡献；内置 target、managed deployment、Support Pack 和产品组件闭包只在 Inno.Build.Distribution.Standard 注册；Editor、CLI 与 MSBuild 注入宿主上下文后消费同一不可变 distribution。通用 Build 库禁止反向引用该组合库或 Editor。请求的目标与托管部署能力必须在 Support Pack 供给及外部工具启动前统一校验，不能先执行昂贵准备再拒绝不支持的组合。
<!-- /rule-unit: build-host -->

<!-- rule-unit: platform-products -->
<a id="platform-products"></a>
## 28. 平台、共享后端与产品组合

- 按职责维护共享代码，按平台集中系统/SDK接入，按包含架构和 ABI 的明确目标构建，按产品组合 Editor 或 Player。Desktop 不作为目标；工具宿主、运行目标、产品和托管部署分别声明。
- Foundation、Content、Services、Runtime、Shell、Player Runtime 与 Editor Hosting 不引用命名平台；backend 不引用平台，平台基础 build/runtime 不引用共享 backend 或 integrations，平台产品入口与同平台 integration 允许显式选择后端；平台不引用另一平台；Runtime 不引用构建、签名或 SDK 发布工具。
- AdapterSelection 不提供具体 backend 默认值。平台产品显式注入 catalog、selection、frame driver、keyboard policy、日志/内容/存储来源及 distribution；共用 Host 不调用 OS 判断选择这些实现。
- StandardBuildDistribution 是唯一内置发行注册点；平台模块贡献绑定完整的 target、Support Pack 和 SDK，DotNet deployment 独立注册。不能以目录扫描、环境变量、程序集命名或宿主 OS 隐式安装/选择平台。
- 普通 Native 产品闭包由平台贡献的 `nativeProducts` 声明，通过 distribution 解析；MSBuild Task 不得维护另一份产品 switch 或 backend 列表。未注册产品必须在 SDK/staging 前失败。
- NativeBuildContext 冻结实际 host、target、toolchain、组件位置及绑定。recipe 通过描述读取所属源码；CMake 参数仅由 NativeCMakeExecutor 组合，底层进程工具不能隐式增添目标参数。普通 Build/Publish 通过 PrepareProductNativeTask 准备完整匹配闭包，design-time 不执行 Native。
- 托管工具进程统一使用 DotNetSdkEnvironment 隔离父 IDE/MSBuild 的 SDK resolver、加载程序集及扩展根变量，并保留已选 Native SDK 环境。进程 override 的 null 明确移除继承变量，空字符串仍表示显式空值；不得通过污染父进程或锁定所有消费者 SDK 来规避隔离问题。SDK 由所属工程声明解析，产品 Native 准备策略由产品项目声明，共同 targets 不判断具体平台 ID。
- 工程选定的 SDK 必须冻结实际 CLI 入口，并由该入口执行发布；短路径、junction 或临时工作目录不能再次解析另一个 SDK。记录 SDK identity 与 CLI 路径，不能复制 global.json 或改写消费者 SDK 来掩盖执行差异。
- 多职责平台模块必须显式声明实际参与 Native recipe 的实现闭包，不能因修改 Support Pack 或包装代码使无关原生归档失效。校验器消费已冻结的组件产物声明，不重复维护另一份 backend 文件名单。
- 日常开发只有 `entry/InnoEngine.Editor` 的 InnoEngine.Editor 与 `entry/InnoEngine.Build` 的 InnoEngine.Build 两个可执行项目。`entry` 在工程求值阶段选择当前 SDK 宿主的产品引用，Program 直接调用公开应用入口；正常 C# 源码由 SDK 项目图构建，运行期间不组织源码构建；不得将这种便利路由下放到共享领域、backend 或构建目标。平台 Editor 和 Build.Cli 是库；Player 宿主源码是发布输入，只有完整发布模板生成可执行游戏。测试进程 fixture 仅由所属测试消费的隔离构建产生，不成为 IDE 日常启动项目。CPU 扩展复用所属平台产品，不复制 Program/backend；只有真实框架或启动差异才新建项目。
- Editor 产品的 Shader 平台/API 配置归所属平台，由显式产品构建属性传递；共享 ImGui 后端不维护命名平台或 CPU 选择表。Shader 增量必须校验内容、工具与产物完整性，并复用现有哈希、lease 和原子发布；不能仅根据时间戳跳过验证。
- 开发 Editor 通过 SDK 项目图构建当前宿主产品，在主线程直接调用公开 Run；引擎/backend 源码修改后正常再次构建或运行。启动期间不扫描源码、不运行子 MSBuild，也不读取预构建 receipt。游戏脚本/资产仍走既有导入与 reload。Editor Build 不附带发布另一配置的 Player Support Pack，游戏发布单独准备发行闭包。
- 平台 `ProductBuild.props` 声明共享 backend Shader 配置，Editor 在此基础上声明 UI Shader；Player 与 Editor 显式传递同一产品属性机制。Composition、OutputTransfer 与 ImGui 的 Shader 产物都使用独立 `Outputs` owner，不在共享 backend 项目中恢复目标选择表或时间戳跳过。
- 共享 Editor 的可发现功能和 Panel 由 `EditorProduct.props` 统一声明产品依赖，平台入口共同导入。Host 的实现依赖不能代替产品部署闭包；不得通过无效类型引用、运行时目录扫描或重复平台列表强行保留扩展。
- 未来平台只记录真实接入边界，不创建空项目、SDK占位或未经验收的发布目标。复用 SDL/BGFX/MiniAudio 必须验证实际目标实现、SDK、ABI、surface、线程、回调和生命周期。
- 新项目、目录迁移和公开契约必须同步 Solution、有效 MSBuild 引用、架构验证、所有消费者、唯一 Wiki 页面和当前验收；禁止通过移出旧扫描根绕过规则。

- BGFX 等共享 backend 不维护命名平台的工厂、枚举或目标表。平台提供不可变 SDK invocation、Shader 方言、defines 和能力；发行在一个绑定入口组合。实际有序参数、布局、能力和工具必须进入指纹。
- Support Pack 必须先完成只读预检并返回冻结计划，再取得发布 lease 和创建 staging；缺 SDK、预取消或错误计划不能创建输出。执行必须使用已选 managed CLI 与 Native toolchain，不能再次从 PATH 或临时目录解析 SDK。
- 工程的 NuGet 静态图还原必须使用 RestoreSerializeGlobalProperties 标准输入传输，保留 IDE 的完整全局属性；不得通过删除属性或修改 SDK 来回避 Windows 命令行长度限制。Solution 按配置使用支持 RestoreGraphProjectInput 的普通还原：Debug/Release 正常还原共同源码、工具、测试与 entry 的宿主依赖；SourceDebug/SourceRelease 保留源码验证闭包并排除 Editor entry。平台产品只通过明确入口引用或显式目标构建。不得把同一共享项目的不同产品目标写入一次 path-based Solution 还原，造成中立与目标 assets 相互遗漏。
- 内部工具引导与 owned project restore 必须隔离父 IDE 的 Solution entry point，只还原明确的项目闭包；外层 Solution restore 保留自身上下文。不得让 Windows Editor 为无关 Browser 产品要求 workload，也不得靠安装无关 workload 掩盖闭包错误。
- 代码生成器在 SDK props 导入前声明 IsRoslynComponent 工具身份，共同产物规则不按产品 Native 目标隔离其 bin/obj，确保普通 Restore 与编译使用同一中立目录。Analyzer 引用只移除产品与输出属性，保留所属 Solution 的构建上下文；私有 Task 引导才隔离 Solution。不得在编译阶段补写已求值项目的 NuGet imports，也不能在 CLI 中重复维护生成器项目名单或复制另一目标的还原产物。开发 Build 默认增量；显式产品 clean 根据有效输出路径建立 owner 清理计划，取得产品独占 lease 后执行。不能清理其他产品、目标或有效缓存。

- 平台 packager 仅验证和打包；内容 compiler 由后端实现，发行通过 GameBuildContribution 绑定准确目标。平台基础不持有 compiler，也不接收创作服务。
- Native 每个产品步骤显式声明 Static/Shared、有序组件参数与配置输入；真实配置参与指纹，recipe 在 staging 前拒绝未实现的链接方式。SDK 不隐式决定产品链接。
- SDL 应用的窗口目录只包含自身创建或明确接管的窗口；外部 owner 保留原生销毁权，解除登记先失效 wrapper。ImGui 使用同一 host surface，不重复解析 SDK 句柄；交互规则由产品注入，尺度在 NewFrame 前同步，不维护 CPU 专属返回 ABI。
- ImGui viewport 的位置与逻辑尺寸通过平台事件及 ImGui getter/setter 协议协调；drawable metrics 同步不得回写请求中的位置/尺寸或制造 Move/Resize 请求，避免双向状态同步干扰拖动。

- 两个 entry 使用普通 ProjectReference 和直接公开调用；不得恢复预构建 Editor receipt、CLI 启动 worker、运行时子 MSBuild 或共享 .run。仅 entry 可解除自身条件产品引用的 Solution 构建屏蔽；其他宿主产品不因此进入默认构建。
<!-- /rule-unit: platform-products -->

<!-- rule-unit: backend-build -->
<a id="backend-build"></a>
## 29. Backend 运行接入与生成成本

- SDL 共同窗口创建与焦点读取只有一个 backend owner；平台 integration 仅处理实际 SDK 差异。共享 BGFX Device 通过自身 IBgfxSurfaceIntegration 接收借用 surface，不维护命名平台/ABI 白名单；Default catalog 必须接收明确 rendering factory。
- 附加窗口关闭先解除交互/登记，再退休 renderer surface；渲染端确认已处理 framebuffer 销毁命令后才释放原生窗口。Native callback 不等待或推进帧；最终 drain 仅在 owner thread、闭帧、无 graph/encoder 的安全点执行，失败保留依赖 owner。完成 Task 不是通用 GPU fence。
- Native 生成定义唯一属于组件 Bindings/bindings.props，MSBuild 与库调用共用；Toolchains 只依赖 INativeBindingGenerator，具体 BGCS 应用流程属于 Inno.Build.Bindings，Task 仅映射参数/取消/日志。产品闭包只请求一次完整 binding batch，托管消费保留 Native 已选指纹及逐组件 Static/Shared 请求；动态初始化开关只在 Native 编译边界消费实际链接选择，不能由平台名称或宿主推导。
- 绑定配置路径变量只从 operation 冻结的 Native SDK 环境展开；直接库与 MSBuild 使用同一目标解析，不以修改进程环境或临时 PATH 让生成器获得 SDK。缺失配置在候选创建前失败。
- 产品 binding selection 属于该产品的 SDK 中间目录，Build/Publish 后保留供内容相同的下次构建复用；只由 Native 绑定项目导入，不能让瞬态选择文件成为共同领域项目的编译输入。内容相同不重写时间戳，变化时原子替换；产品清理收回该文件。Native Publish 仍严格核对本次 managed 编译使用的闭包、指纹和源码；保留选择文件不等于持有生成缓存的读取 lease，实际消费者继续独立取得读取保护。
- 初始、批次锁后/生成后及各 Native 锁后/编译后分别 fresh 校验；只在同一明确阶段去重完整内容读取，不能跨等待/执行复用旧结果。可变源码必须复制固定，不以 hardlink 伪装不可变输入。
- 冷 CMake producer 通过共同 executor 固定 recipe 声明的工作区输入；Browser 聚合的组件 include、bridge 与参数必须解析到同一复制树。热缓存命中不复制，外部 SDK 与输出保持自己的位置和 owner，不用字符串替换整个仓库根来误重定向输出。
- Task runtime 只在 IBuildEngine4 Build 生命周期、同一完整请求身份内复用成功不可变 publication；registry 只存 BCL 数据，不持有 SDK owner、lease 或私有 ALC 类型。每个实际载入边界仍有独立 owner；失败/取消不缓存。共享 restore 写入由 Core.IO lease 协调，等待时 Yield/Reacquire，保留锁文件。
- 独立并发托管构建必须通过 SDK 的明确 ArtifactsPath 使用各自 bin/obj owner；共同规则不得覆盖该输出选择。NuGet Runtime 资产按产品真实 Restore 图选择，Native 内容寻址缓存仍通过自身 lease 共享。普通 IDE bin/obj 仅属于一个活动构建，不能把两个独立进程同时写同一路径视为受支持的并发模式。
- 同一 SDK 图中的中立与产品请求也必须隔离实际输出；使用 ArtifactsPath 时，在 SDK 建立默认路径前选择包含产品输入的 ArtifactsProjectName，保留 SDK configuration/TFM/RID pivots 和显式调用方设置。开发 Editor 入口通过现有 owned restore 准备所选产品的完整引用闭包后再构建，不能依赖平台子项目稍后还原来满足入口的直接或 SDK 添加的传递引用。
- Owned NuGet Restore 必须使用标准 `ExcludeRestorePackageImports=true`，编译使用包含已恢复 imports 的独立求值配置。不得让同一缓存 ProjectInstance 把旧 package content items 带入新程序集输出，也不得通过包名特例、补写 NuGet imports 或删除缓存掩盖混装。
- NuGet 依赖使用正常 `.csproj` 的 `PackageReference` 与 SDK Restore，不启用、生成或提交依赖锁文件，不维护逐工程、宿主、RID、AOT 或裁剪的锁清单与命名规则。包版本由正式声明维护，BGCS 包族继续使用统一精确版本；消费者读取 SDK 实际求值的 assets 和包内容。CI 使用普通还原并验证构建、包身份和源码卫生；不得以自制依赖解析器、清空真实 SDK 属性或 ignore 规则代替正确流程。用于并发读写、进程所有权和原子发布的 Core IO lease 不属于依赖锁定，必须保留。

- 引擎管理的构建 SDK 工作缓存与协调锁必须位于对应的 `artifacts` / `obj` 或显式输出 owner 内，不为源码目录中的已退出缓存位置保留全局 ignore 或验证豁免。清理旧工程目录前，核对当前工程/还原图、Git 归属、符号链接、活动进程和完整文件清单；普通运行锁仍遵守现有 owner 退休协议。远端旧分支的历史配置不是当前分支的游离文件，不得跨分支只删清单或改写历史来制造已清理的结果。
<!-- /rule-unit: backend-build -->
