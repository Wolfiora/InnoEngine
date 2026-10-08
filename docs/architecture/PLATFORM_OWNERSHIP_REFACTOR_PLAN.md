# 平台归属、共享后端与产品组合重构执行计划

[架构索引](README.md) · [Wiki 首页](../README.md)

## 1. 目标与执行状态

本文件记录用户于 2026-10-07 批准的完整实施约定。验收结果单独记录在
`PLATFORM_OWNERSHIP_REFACTOR_ACCEPTANCE.md`；计划中的结构不能作为已经实现的证据。

**按职责维护共享代码，按平台集中维护系统接入，按明确目标构建产物，按产品组合 Editor 或 Player。**

完成条件：共享领域不依赖具体平台；共享 backend 具有唯一源码 owner；平台产品显式组合；
Editor、CLI、MSBuild 使用同一发行定义；现有产品和生成、原生、发布路径全部替换；
消费者、Solution、验证器、文档和本机必要验收闭合。删除被替代的实现，不自动提交。

## 2. 范围与四个维度

| 维度 | 示例 | 责任 |
|---|---|---|
| 维护平台 | Windows、MacOS、Browser、NS | 系统接入、平台包 |
| 明确运行目标 | WindowsX64、MacOSArm64、BrowserWasm32 | CPU、ABI、系统约束 |
| 产品 | Editor、Player、Build CLI | 功能闭包、启动 |
| 托管部署 | CoreCLR、Mono Wasm 解释执行/AOT、NativeAOT | 托管编译、执行、最终链接 |

稳定目标 ID 保留 `windows-x64`、`macos-arm64`、`browser-wasm`；Browser 明确声明 wasm32。
RID、triple、ABI 和组件工具参数通过目标描述映射，不拼接 ID，不由宿主推导目标。
Desktop 不作为平台、发布目标或组合入口。

本次迁移 WindowsX64 Editor 与 CoreCLR/NativeAOT Player、MacOSArm64 Editor/Player、
Browser Wasm32 解释执行/AOT Player。保留既有 Linux Native 工具链，不注册 Linux 游戏目标。
WindowsX86/Arm64、MacOSX64、iOS、NS 和其他主机只记录具体接入设计，不建空项目。
Browser Editor 不在范围。保持目标框架；不修改 extern；不手工修改生成绑定。
BGCS 独立维护，只同步引擎消费配置、扩展和验证。

## 3. 根目录与依赖

```text
InnoEngine/
├─ src/
│  ├─ foundation/{core,extensibility,scripting}/
│  ├─ content/{deployment,assets,references,scene,animation}/
│  ├─ services/{platform,input,storage,rendering,audio,text,ui}/
│  ├─ runtime/{contracts,engine,generators,plugins,scripting}/
│  ├─ adapters/{common,platform,input,storage,rendering,audio,text,ui,presentation}/
│  └─ composition/
│     ├─ default/Inno.Engine.Default/
│     ├─ adapters/{Inno.Adapter.Default,Inno.Adapter.Authoring.Default}/
│     ├─ shell/Inno.Shell/
│     ├─ player/Inno.Player.Runtime/
│     └─ editor/{contracts,framework,hosting,presentation,features,panels}/
├─ backends/{Interop,Sdl3,Bgfx,MiniAudio,ImGui,Text,RmlUi,FileSystem,DotNet}/
├─ platforms/{Windows,MacOS,Browser,Linux}/
├─ build/{pipeline,managed,toolchains,support,composition,distributions,tasks,msbuild,cli}/
├─ tools/
├─ tests/
├─ docs/
├─ extern/
└─ artifacts/
```

依赖由外到内：平台产品 → 共享 Editor/Player/Shell + backend + 平台系统集成；
领域 Runtime → 中立契约/Content/Foundation；backend → 中立 SPI + 所属 Native；
平台 Build → 中立 Build + backend recipe + SDK；发行组合 → 平台 Build + 托管部署实现。

禁止共享领域、Shell、Player Runtime 引用平台；禁止共享 Editor Hosting 引用产品入口、具体发行或选择 backend；
禁止 backend 引用平台、平台互相引用、运行服务引用 Build/签名/SDK、按平台复制 Native。
工具宿主只决定工具是否可执行。工具探测留在明确 provider，不能替代请求目标。

## 4. 平台包完整结构

```text
platforms/Windows/
├─ Directory.Build.props
├─ editor/Inno.Editor.Windows/
│  ├─ Inno.Editor.Windows.csproj
│  ├─ Program.cs
│  └─ WindowsEditorComposition.cs
├─ player/Inno.Player.Windows/
│  ├─ Inno.Player.Windows.csproj
│  ├─ Program.cs
│  ├─ WindowsPlayerComposition.cs
│  └─ WindowsPlayerContentSource.cs
├─ runtime/Inno.Platform.Windows/
│  ├─ Inno.Platform.Windows.csproj
│  └─ WindowsApplicationLocations.cs
└─ build/Inno.Build.Windows/
   ├─ Inno.Build.Windows.csproj
   ├─ WindowsBuildModule.cs
   ├─ Targets/WindowsX64GameBuildTarget.cs
   ├─ Toolchain/WindowsNativeToolchainProvider.cs
   ├─ Packaging/                       仅实际实现
   ├─ SupportPacks/{WindowsPlayerSupportPackSource,WindowsSupportPackValidator}.cs
   └─ Templates/WindowsPlayer.project.xml

platforms/MacOS/
├─ editor/Inno.Editor.MacOS/{Inno.Editor.MacOS.csproj,Program.cs,MacOSEditorComposition.cs}
├─ player/Inno.Player.MacOS/
│  ├─ Inno.Player.MacOS.csproj
│  ├─ Program.cs
│  ├─ MacOSPlayerComposition.cs
│  └─ MacOSPlayerContentSource.cs
├─ runtime/Inno.Platform.MacOS/{Inno.Platform.MacOS.csproj,MacOSApplicationLocations.cs}
└─ build/Inno.Build.MacOS/
   ├─ Inno.Build.MacOS.csproj
   ├─ MacOSBuildModule.cs
   ├─ Targets/MacOSArm64GameBuildTarget.cs
   ├─ Toolchain/MacOSNativeToolchainProvider.cs
   ├─ Packaging/                       仅实际实现
   ├─ SupportPacks/                    平台 source 与 validator
   └─ Templates/MacOSPlayer.project.xml

platforms/Browser/
├─ player/Inno.Player.Browser/
│  ├─ Inno.Player.Browser.csproj
│  ├─ Program.cs
│  ├─ BrowserPlayerComposition.cs
│  ├─ HttpPlayerContentSource.cs
│  ├─ BrowserBridge.cs
│  └─ wwwroot/{index.html,main.js}
├─ runtime/Inno.Adapter.Storage.Browser/
└─ build/Inno.Build.Browser/
   ├─ Inno.Build.Browser.csproj
   ├─ BrowserBuildModule.cs
   ├─ Targets/BrowserWasm32GameBuildTarget.cs
   ├─ Toolchain/EmscriptenNativeToolchainProvider.cs
   ├─ Native/{CMakeLists.txt,wasm_sjlj_shim.c}
   ├─ SupportPacks/
   └─ Templates/BrowserPlayer.project.xml

platforms/Linux/build/Inno.Build.Linux/
├─ Inno.Build.Linux.csproj
├─ LinuxBuildModule.cs
└─ Toolchain/LinuxNativeToolchainProvider.cs
```

ApplicationLocations 只实现 OS 位置规则，不重复存储、内容缓存、输入或窗口机制。
同一 OS 新 CPU 复用产品源码，仅增加真实目标和工具链参数；入口/框架确有区别才增项目。
Browser JS 生命周期、下载与 Browser Storage 留在 Browser 包。

## 5. 共享 backend 完整结构和迁移表

```text
backends/<component>/
├─ runtime/<具体 Adapter 项目>/
├─ native/Inno.Native.<component>/
│  ├─ Inno.Native.<component>.csproj
│  ├─ managed 初始化入口
│  ├─ Native/{include,src,Generated}/
│  │  └─ CMakeLists.txt
│  ├─ Bindings/{common.json,bindgen.json,实际目标配置,C++ bridge 定义}/
│  │  └─ Extension/                   有实际扩展才建立
│  ├─ Generated/Bindings.cs
│  └─ obj/<targetId>/<generationFingerprint>/{Native,Generated}/
├─ build/<组件 Toolchain 项目>/
│  ├─ 组件 recipe 与构建实现
│  ├─ Native/                        实际组件构建片段
│  └─ obj/native/<targetId>/<fingerprint>/
├─ tests/                           组件测试唯一 owner
└─ docs/                            不建立空目录；权威 Wiki 在 docs/backends
```

| Backend | 迁移项目 |
|---|---|
| Interop | Inno.Native.LibraryLoading |
| Sdl3 | Platform.Sdl3 Adapter、Native.Sdl3、Toolchains.Sdl3 |
| Bgfx | Rendering.Bgfx Adapter、Native.Bgfx、Bgfx/Bgfx.Tools/Bgfx.Shaders Toolchains |
| MiniAudio | Audio.MiniAudio Adapter、Native.MiniAudio、对应 Toolchain |
| ImGui | 两个 Presentation Adapter、Native.ImGui、BindingExtension、Native.ImGuizmo、两个 Toolchain |
| Text | FreeTypeHarfBuzz Adapter、Native.Text、对应 Toolchain |
| RmlUi | Runtime/Authoring Adapter、Native.UI、对应 Toolchain |
| FileSystem | Content.FileSystem、Storage.FileSystem |
| DotNet | Modules.DotNet、Serialization.DotNet、Build.Managed.DotNet |

物理移动保留程序集/CLR namespace，明确拆分与重命名除外。源代码只有一份；纯 C 或无扩展组件不建空结构。
Default 两项目迁入 composition/adapters，只组合 provider。BGFX Authoring provider 归 BGFX build，
ImGui Presentation provider/context 归 ImGui backend。

## 6. 契约、组合和启动

| 契约 | 所属 | 必要语义 |
|---|---|---|
| PlatformTargetDescriptor | 中立 Build | 目标 ID、平台归属、架构、ABI、产品要求 |
| BuildHostDescriptor | 中立 Toolchains | 实际执行工具的系统和架构 |
| NativeToolchainSelection | 中立 Toolchains | 冻结工具、SDK、环境、参数和身份 |
| NativeComponentDescriptor | 中立 Toolchains | 组件 ID 与 Native/Binding/Toolchain owner 明确位置 |
| BuildPlatformContribution | 中立 Composition | 绑定描述、target factory、Support Pack 与 toolchain |
| ProductNativeBuildPlan | 中立 Toolchains | 产品组件闭包和选定工具链 |

复用 BuildTargetId、ManagedDeploymentId 和 Native recipe；操作开始冻结，预检失败在 staging 前报告。
组件路径显式传入，不根据程序集名猜测目录。SDK 和进程环境不能进入领域或游戏持久状态。

中立 `Inno.Build.Composition` 保留 Distribution、Context、PipelineFactory、TargetFactory、Contribution，
不引用具体平台、compiler 或 backend。具体 `Inno.Build.Distribution.Standard/StandardBuildDistribution`
是唯一内置注册入口。Windows/MacOS/Browser 模块提供完整绑定贡献，DotNet compiler 只注册一次；
Linux 仅提供工具链能力。删除 BuiltInBuildDistribution。预检唯一 ID、工厂、source、能力和工具执行支持。
Editor、CLI、Task 消费同一 distribution，禁止扫描目录或隐式安装发现。

Editor.Application 替换为共享库 Editor.Hosting + Windows/MacOS 薄入口。
公开 `EditorApplication.RunAsync(EditorLaunchOptions, CancellationToken)`，内部 EditorHost 保持 internal。
Options 注入创作位置、Adapter catalogs/明确 selection、Presentation/window/frame driver、distribution/context、
既有 module/serialization 边界和日志/宿主服务。不引入全能 IEditorPlatform 或服务定位器。
借用不可变配置；Host 创建的会话、设备、UI、任务、callback 由 Host 释放，失败/取消也清理。

中立 AdapterSelection 删除 defaultValue 和具体初始值，缺失明确失败。平台组合显式选择；
自定义平台不必依赖 Default。Shell/Player/测试消费者同步，不留默认兼容层。

Player.Runtime 保留玩法、内容校验和退出。DesktopPlayerComposition 删除；Windows/MacOS 明确内容布局，
共用文件读取在 FileSystem backend，不猜测 Resources/Content fallback。Browser 原启动模型迁入平台。
生产入口只保留 Editor.Windows、Editor.MacOS、Player.Windows、Player.MacOS、Player.Browser、Build.Cli；
其余生产项目为库，不按 CPU 复制 Program。

## 7. Native、Shader、Support Pack 与 MSBuild

HostNativeToolchain 的宿主探测/目标选择拆开：共同进程、取消、环境、SDK 身份、快照和发布机制
在 Toolchains，Windows/MacOS/Linux/Emscripten SDK 选择在各平台。删除 CreateForCurrentPlatform。
Context 持有明确目标/宿主/工具链/组件描述，GetNativeBuildRoot 和 recipe 不假定 build/toolchains 布局。
组件 recipe 留在 backend；通用 CMake/MSBuild/Make executor 不选择平台产品。
删除 Toolchains.Host，通用闭包/部署收回 Toolchains，组件由 ProductNativeBuildPlan 声明。
保留完整内容哈希、稳定性检查、FileLease、原子发布、export 与 binding 身份校验、不重复替换加载中 DLL。

Browser 巨型 CMake 拆成 backend 组件片段；Browser 只聚合选定组件与 Emscripten/最终链接约束。
桥路径、组件和源码根显式输入。保留 WebGL2、异常/longjmp、callback、静态链接、必要 shim 并记录原因。
托管/原生使用同一次冻结的绑定闭包。原 Browser Toolchain 合入 Build.Browser，旧项目删除。

ImGui SDL 事件与 BGFX 绘制分离；共享 Widgets/Panel 不变。Shader 按 Editor 产品目标构建，
不根据当前 OS 选择；导出游戏不改变 Editor Shader。输入/目标/GPU API/tool/resource name 入指纹。
Player 闭包不得包含 ImGui、ImGuizmo、Editor 字体或 Shader compiler。

Support Pack source、validator、模板归平台，Desktop source 删除；复制/校验/模板机制归 Core。
原具体 SupportPacks 项目删除。DotNet compiler 移 backend，Editor 与 Player 编译配置独立。
source/pack 按目标、部署、组件/绑定/工具身份区分；失败/取消保留上次完整输出。

PrepareEditorNativeTask 正式替换为 PrepareProductNativeTask。产品项目显式声明产品/目标/Native 闭包；
IDE Build/Publish 自动准备，design-time 不启动工作。Task bootstrap 清除产品 RID/AOT/profile/target 属性，
工具按宿主执行，bin/obj 独立；保留 Task Host retirement 和加载 DLL 部署行为，验证无循环。
外国宿主产品/Browser 发布入口在 Solution 可见但不参与本机默认批量构建。

```text
artifacts/
├─ native/<component>/<targetId>/<fingerprint>/
├─ bindings/<component>/<targetId>/<fingerprint>/
├─ managed/<deploymentId>/<targetId>/<fingerprint>/
├─ support-packs/<targetId>/<deploymentId>/<fingerprint>/
├─ products/<productId>/<targetId>/<fingerprint>/
├─ build-tools/
└─ acceptance/<runId>/
```

旧缓存直接失效，不作为 fallback。组件中间态在所属 obj/native；同目标产品可复用相符 Native，
托管产品闭包/部署/属性不同必须隔离。

## 8. 所有权与迁移

组合预检 → 冻结 → 创建应用/设备/Session → Run/Suspend/Resume → 停止接受工作 → 取消并完成任务
→ 注销 callback → 保存 Editor 状态/提交存储 → 退休领域资源 → 释放设备/窗口/原生进程 owner。
Core Events、Identity、Missing、last-good、候选原子切换、History/Workspace、Edit/Play 隔离保持唯一 owner。
Full GC → finalizers → Full GC + 弱 monitor、Pending/Faulted gate 不得弱化。

记录 revision、SDK、工作区、有效依赖、公开 API 和持久身份。file-map.tsv 逐文件记录原路径/目标路径/
职责/API/消费者/验证/状态。移动阶段核对 token、初始化和释放顺序；行为改动另验。
修改所有 ProjectReference、import、CMake、模板、资源、BGCS 定义并重新生成。持久身份变化前记录，
必要时通过既有显式 Stable ID 固定；History/state/Asset/Plugin ID、脚本逻辑 namespace 不变。
Canvas、Rendering2D、Samples 同步，重新构建 Plugin/脚本 references，Player 排除 Samples/Authoring。

| 阶段 | 工作 | 门禁 |
|---|---|---|
| 0 | 基线与文件闭包 | 可追溯 |
| 1 | 目标/宿主/组合中立契约 | 编译与预检 |
| 2 | 组件 owner、Native 路径 | 当前构建正确、无 fallback |
| 3 | backend 迁移 | Native/生成/managed 闭包 |
| 4 | 平台 SDK、Browser 片段、发行注册 | 宿主不决定目标 |
| 5 | Editor Hosting 与平台入口 | Build/启动/Play/退出 |
| 6 | Player/Support Pack/模板 | 四条本机发布路径 |
| 7 | Solution/验证器/消费者/Wiki | 全仓覆盖 |
| 8 | UI/reload/性能/发布矩阵 | 证据闭合、删除旧实现 |

每阶段同步全部调用方、编译、成功/失败/取消/边界测试、记录映射、删除旧实现；不保留双目录/转发 API。

## 9. 验证器、规范与文档

验证器覆盖 backends/platforms/distributions 和有效 MSBuild 引用。禁止共享→平台、Runtime→Build/Editor/Authoring、
backend→平台、平台互相引用、中立 Composition→发行；检查目标/ABI/产物对应、边界外宿主判断、隐式默认、
组件 owner/程序集/scripting 唯一性、普通 Compile Link、空 Folder/孤立项目/无效配置/循环。
生成绑定的选定输出允许作为唯一 Compile source。生产 Exe 白名单更新，不能利用新根绕过规则。

AGENTS 的 XML、显式 using、m_、多参数排版、Editor 项目级 namespace/引用可见性、最小 API 强制执行。
禁止 InternalsVisibleTo/反射穿透/测试后门/legacy reader/schema version/旧目录 fallback。
更新目录、Native owner、平台注册规则，合并重复条款。

文档更新：本计划/本次验收、ENGINE_ARCHITECTURE_OVERVIEW、PLATFORM_RUNTIME_ARCHITECTURE、
PLATFORM_EXTENSION_GUIDE、WEB_PLAYER_ARCHITECTURE、CSHARP_DEVELOPMENT_STANDARD、CURRENT_ISSUES、架构索引。
新增 docs/backends 与 docs/platform/Windows/MacOS/Browser/Linux 索引和唯一项目页；修复所有入站链接，
public/protected API 以源码为准，内部实现、当前能力、未来设计和实机证据严格区分。

## 10. 验收矩阵

契约：目标/宿主/产品/部署独立，Editor 与导出目标不同，重复 ID/缺组件/错误 ABI/SDK/不支持部署失败；
自定义平台贡献无需改领域、未注册不发现、自定义 Adapter 不要求 Default、三宿主发行一致、Player 闭包纯净。
通过真实公开契约，不扩大可见性或引入测试后门。组件测试迁所属 backend，其余领域/集成留根 tests。

Native：干净 IDE Build/Publish、design-time、引导隔离/无循环、binding/export 实际调用、recipe 正确失效、
同长度 mtime 修改、损坏输出、热构建、并发/取消/等待/加载 DLL/权限、旧输出保留、checkout/目标隔离。
冷构建+三次热构建记录耗时、工具进程、扫描字节、部署次数；不以推断代替测量。

Editor FlappyBird：Debug/Release/Play/停止/退出、场景删除/缺 Asset 隔离、reload 成败/旧强引用/GC/Faulted，
浮动 GameView 与失焦释放、Popup/Modal/前景吸收、滚动 owner、向下受限选择器、2:3 label/换行/DPI、
Export 自动关闭、小地图、History/Workspace/Missing/退出保存。真实焦点验收在允许空闲时段执行。

发布：Windows CoreCLR、NativeAOT、Browser 解释执行、Browser AOT 的真实发布/启动/输入/内容/声音/存储/退出；
昼夜亮度/颜色、光照方向/移动、夜晚星光逐项验收，不以启动成功替代效果。macOS 源码/组合检查与实机验收分开，
本机不可执行标记未实测。Canvas/Rendering2D Plugin/脚本/渲染消费验证。BGCS 独立 managed/NativeAOT/Wasm 调用单独报告。

## 11. 未来平台接入

```text
platforms/NS/
├─ player/Inno.Player.NS/
├─ runtime/                          实际 SDK Adapter
├─ native/                           必要 facade/生成绑定
├─ build/Inno.Build.NS/{Targets,Toolchain,Managed,Packaging,SupportPacks}/
├─ editor/Inno.Editor.NS.Tools/       有专属工具才提供，运行在既有 Editor
├─ tests/
└─ docs/

platforms/IOS/
├─ player/Inno.Player.IOS/{Program.cs,AppDelegate.cs,IosPlayerComposition.cs}
├─ runtime/
├─ build/Inno.Build.IOS/{Targets,Toolchain,Packaging,Signing}/
├─ tests/
└─ docs/
```

不在本轮建占位。接入步骤：真实实现 → 明确设备/模拟器目标/ABI/产品/部署 → SDK provider/可执行宿主
→ 选择并验证 backend → 薄产品组合 → 布局/打包/签名/Support Pack → 唯一发行注册 → 实机完整验收。
NS 复用 SDL/BGFX/MiniAudio 需合法可用实现并验证，不将架构可组合宣称为 SDK/运行时支持。
Browser 替换托管运行时新增 compiler/链接边界，重验互操作/callback/异常/启动，复用领域代码和 Browser 宿主。

## 12. 最终交付约束

文档与对话逐项总结用户问题、结果、目录/删除/API/依赖/owner、实际命令/环境/revision/证据、
本机发布运行、未实测平台、性能测量、扩展示例和清洁度。提供 `type(scope): subject` 建议，不提交。
保持三个完整发布目标；新归属机制不构成未实现平台支持。指定 Glass.aiff 在 Windows 不可访问时明确记录未播放。
