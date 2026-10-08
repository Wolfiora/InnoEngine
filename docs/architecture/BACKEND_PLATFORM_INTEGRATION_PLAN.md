# 平台、后端与集成边界完整整改计划

[架构索引](README.md) · [Wiki 首页](../README.md) · [本轮验收](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)

## 批准范围与完成定义

按平台维护 SDK、目标与打包，按 backend 维护第三方接入，用真实 integration 程序集连接两者。落实用户批准的全部七个执行阶段；保持 windows-x64、macos-arm64、browser-wasm 稳定 ID，Linux 仅承接已有 Native 工具能力。BGCS 独立，本轮不修改其源码；不修改 extern、生成绑定，不新增生产 Program，不自动提交。保留执行前已有未提交修改。

平台基础 build/runtime → 中立契约；backend → 中立契约与自己的 Native；integration → 同平台契约与 backend；产品与 Standard Distribution 显式选择实现。不同平台互不依赖，backend 不依赖 integration。共享 Runtime、Player、Editor Hosting 不引用集成。

## 完整执行阶段与门禁

| 阶段 | 文件闭包与实际职责 | 完成门禁 |
| --- | --- | --- |
| 0 | baseline/revisions.json、working-tree.diff、environment.json、project-dependencies.json；file-map.tsv | 保存旧工作区、SDK、磁盘和依赖 |
| 1 | IGameContentCompiler、GameBuildTargetBinding；IGameBuildTarget、BuildPipeline、GameBuildPipeline；BuildTargetFactory、GameContentCompilerFactory、GameBuildContribution、BuildDistribution、BuildPipelineFactory | compiler/packager 分开；无参数平台工厂；目标不一致立即失败；取消关联任务 |
| 2 | 四个 BGFX integration；三个平台 Targets、BuildModule、csproj；StandardBuildDistribution、StandardNativeBuildPlans；Shader Task、CLI、Hosting、Support Pack 消费者 | 平台基础无 backend 引用，保持 Windows 四 API、Mac Metal、WebGL 2 内容闭包 |
| 3 | NativeComponentBuildOptions、NativeBuildContext、ProductNativeBuildStep/Plan、NativeStaticBuildDefinition、NativeCMakeExecutor、NativeBuildRecipe/Fingerprint；各组件 toolchain 与 CMake；BrowserToolchain 聚合 | Shared/Static、有序定义、真实配置输入与 ABI 纳入指纹；缺能力 staging 前失败；缓存、锁、原子发布不弱化 |
| 4 | ISdl3HostIntegration、Sdl3HostCapabilities、Application/Window/Provider；三个 SDL integration；Default catalogs；五个产品 composition | 只登记自建/明确接管窗口；包装失败销毁自建窗口；借用先失效后由 owner 销毁；线程与 Core Events 保持 |
| 5 | ImGuiInteractionOptions、Context、ViewportBackend、PlatformIoNative、SDL extensions、BGFX Presentation Provider/Context | 统一 surface；无 framebuffer 返回 ABI 回调；NewFrame 前尺度/DPI 同步；无可呈现输出跳过；viewport 失败清理 |
| 6 | Solution、ArchitectureRules/Validator/PlatformOwnershipValidator；七个 API 页、分类索引、扩展指南、AGENTS | 有效 MSBuild 引用、公开 XML、风格、Player 闭包、无空 Solution Folder、无旧生产入口 |
| 7 | Build/SDL/ImGui/Architecture 新测试与受影响测试；Windows Debug/Release、CoreCLR、NativeAOT、Web 解释/AOT、Canvas/Rendering2D | 真实发布和运行；视觉/焦点证据独立报告；macOS/Linux 未实测状态明确；准备+三次热构建测量 |

每阶段同步消费者、编译、测试与文件映射；失败修复后继续，不保留转发接口或双实现。运行矩阵使用 InnoEngine.Samples/FlappyBird。仅清理本轮明确 owner、已无活跃进程的中间态，保留当前产物与日志。

## 新增与迁移后的文件树

```text
build/pipeline/Inno.Build/Game/
├─ IGameBuildTarget.cs                 平台验证与打包
├─ IGameContentCompiler.cs             后端中立内容入口
├─ GameBuildTargetBinding.cs           完整目标绑定
└─ GameBuildPipeline.cs                操作 owner、取消与原子提交
build/composition/Inno.Build.Composition/
├─ BuildTargetFactory.cs               无参数平台工厂
├─ GameContentCompilerFactory.cs       借用创作 generation
├─ GameBuildContribution.cs            平台+compiler 一次注册
├─ BuildPlatformContribution.cs        中立平台贡献
├─ BuildDistribution.cs               冻结闭包、预检与 CreateBindings
└─ BuildPipelineFactory.cs
build/toolchains/Inno.Build.Toolchains/Native/
├─ NativeComponentBuildOptions.cs      链接与不可变参数
├─ ProductNativeBuildStep.cs
├─ ProductNativeBuildPlan.cs
├─ NativeStaticBuildDefinition.cs      实际支持的 ABI/归档
├─ NativeCMakeExecutor.cs
├─ NativeBuildRecipe.cs
└─ NativeBuildFingerprint.cs
platforms/Windows/integrations/
├─ Inno.Integration.Windows.Bgfx/
│  ├─ Inno.Integration.Windows.Bgfx.csproj
│  ├─ WindowsBgfxIntegration.cs
│  ├─ WindowsBgfxBuildProfile.cs
│  └─ WindowsBgfxShaderProfiles.cs
└─ Inno.Integration.Windows.Sdl3/
   ├─ Inno.Integration.Windows.Sdl3.csproj
   └─ WindowsSdl3HostIntegration.cs
platforms/MacOS/integrations/
├─ Inno.Integration.MacOS.Bgfx/
│  ├─ Inno.Integration.MacOS.Bgfx.csproj
│  ├─ MacOSBgfxIntegration.cs
│  ├─ MacOSBgfxBuildProfile.cs
│  └─ MacOSBgfxShaderProfiles.cs
└─ Inno.Integration.MacOS.Sdl3/
   ├─ Inno.Integration.MacOS.Sdl3.csproj
   └─ MacOSSdl3HostIntegration.cs
platforms/Browser/integrations/
├─ Inno.Integration.Browser.Bgfx/
│  ├─ Inno.Integration.Browser.Bgfx.csproj
│  ├─ BrowserBgfxIntegration.cs
│  └─ BrowserBgfxShaderProfiles.cs
└─ Inno.Integration.Browser.Sdl3/
   ├─ Inno.Integration.Browser.Sdl3.csproj
   └─ BrowserSdl3HostIntegration.cs
platforms/Linux/integrations/Inno.Integration.Linux.Bgfx/
├─ Inno.Integration.Linux.Bgfx.csproj
├─ LinuxBgfxIntegration.cs
└─ LinuxBgfxBuildProfile.cs
backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/
├─ Api/ISdl3HostIntegration.cs
├─ Api/Sdl3HostCapabilities.cs
├─ Api/Sdl3PlatformApplication.cs
├─ Api/Sdl3PlatformWindow.cs
├─ Sdl3PlatformBackendProvider.cs
├─ Sdl3PlatformApplication.Internal.cs
├─ Sdl3PlatformApplication.Windows.cs    窗口职责，不是 Windows OS 实现
└─ Sdl3PlatformWindow.Internal.cs
backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/
├─ Api/ImGuiInteractionOptions.cs
├─ Api/Sdl3PlatformApplicationImGuiExtensions.cs
├─ Internal/PlatformImGuiContext.Internal.cs
├─ Internal/PlatformImGuiViewportBackend.Internal.cs
└─ Internal/ImGuiPlatformIoNative.Internal.cs
backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/
├─ Inno.Adapter.Platform.Sdl3.Tests.csproj
├─ HostIntegrationTests.cs
├─ WindowOwnershipTests.cs
└─ WindowSurfaceTests.cs
tests/build/Inno.Build.Tests/
├─ GameContentCompilerBindingTests.cs
├─ PlatformBackendIntegrationTests.cs
└─ NativeComponentBuildOptionsTests.cs
tests/editor/Inno.Editor.ImGui.Tests/
├─ ViewportMetricsTests.cs
└─ ImGuiInteractionOptionsTests.cs
tests/tooling/Inno.Tooling.Architecture.Tests/
└─ IntegrationBoundaryTests.cs
tests/NativeProductDeployment.targets  正式产品 Native 计划的测试部署
```

## 生命周期与失败边界

compiler 借用 AssetPipeline、SerializationRegistry、TypeCatalog，不进入 static cache。packager 不取得创作服务。GameBuildPipeline 拥有 staging 与异步工作；失败/取消 drain 后保留旧完整输出。

SDL integration 是不可变借用配置；application 接管成功创建的原生窗口。AdoptWindow 不接管原生销毁权；ReleaseWindow 先使 managed wrapper 不可用。ImGui viewport 使用同一应用登记，释放 renderer、解除窗口登记，再销毁原生窗口与 GCHandle。

Native operation 冻结 SDK、target、generation、组件 options 和真实输入。桌面步骤 Shared；Browser 聚合 Static。当前静态 .a recipe 仅明确支持 browser-wasm，不宣称 Windows .lib。所有输出保持内容哈希、lease、稳定性和 export 校验。

## 扩展步骤与验收边界

WindowsX86 复用 Windows 产品入口，先实现目标/SDK/ABI，再在相应 integration 提供实际参数并验收后注册。新图形 backend 只替换集成 compiler 与 Native 步骤，packager 不变。NS/iOS 只有真实 SDK 接入时建立 integration，不建立空项目。Browser 新托管运行时替换 deployment/link 接入，复用内容与玩法。

验收报告记录每条命令、结果、未实测项和性能数据；源码边界通过不能替代 macOS、Linux 或真实高 DPI 设备验收。最终交付同时总结用户的三个疑问：BGFX 平台配置耦合、其他 backend 是否有同类问题、如何让今后新增平台/后端有固定接入位置。

## 额度交接状态

源码与契约整改完成，Solution Debug/Release 和已完成契约门禁通过。用户要求将未完成产品/实机/性能验收留待额度重置；详见[当前报告](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)及[续跑清单](BACKEND_PLATFORM_INTEGRATION_HANDOFF.md)。不得把待验收写成已完成。
