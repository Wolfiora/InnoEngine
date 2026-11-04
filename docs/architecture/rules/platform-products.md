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
