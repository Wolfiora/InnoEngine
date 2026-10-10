# Backend 运行接入与构建成本整改计划

[架构索引](README.md) · [当前问题](CURRENT_ISSUES.md) · [平台扩展指南](PLATFORM_EXTENSION_GUIDE.md) · [既有集成验收](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)

## 1. 状态、基线与完成定义

**状态：源码整改、消费者迁移与本机可自动执行的产品、性能、架构门禁已完成。** 当前证据以[本轮验收](BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_ACCEPTANCE.md)为准；最终 GUI reload、多屏 DPI、声学与其他机器实测仍明确未完成，不计作通过。

2026-10-09 核对的 InnoEngine HEAD：`1518aa77e87cd208122292780c97c7c280aeb700`。实施前保存了已有三个文档修改；基线 revision、工作区、SDK、工具和磁盘容量位于 artifacts/acceptance/backend-runtime-boundary/baseline。

本计划保留现有总体分层，处理三个已经确认的局部问题：

| 编号 | 当前证据 | 必须达到的结果 |
| --- | --- | --- |
| R01 | Windows/macOS SDL integration 重复窗口创建和初始焦点读取 | 共同操作由 SDL backend 唯一实现，integration 只选择配置并执行实际系统接入 |
| R02 | BGFX 主 surface 白名单为 Win32/Cocoa/Canvas，附加 surface 为 Win32/Cocoa；主输出 sRGB 还有 Canvas 判断 | 新 surface 接入通过 BGFX 所属 SPI 注入，共享 BGFX device 不维护平台 ABI 白名单 |
| R02-L | 附加 viewport 关闭时原生窗口可能早于 framebuffer 销毁命令完成而释放 | renderer 确认退休后，原 owner 在安全点销毁窗口；故障保留依赖 owner |
| R03 | 三次热构建 215.250–242.281 秒；Native 阶段 76.350–80.703 秒；每次约 13.95 GB 哈希读取，新增 17 个私有 Task 载入目录 | 消除可以证明重复的准备、生成请求和读取；保留完整内容校验、明确验证阶段与私有载入所有权，并交付同环境测量 |

当前 `NativeBuildContext` 已共享初始 `NativeBuildInputState`。每次 `CaptureVerification` 创建独立读取缓存。不能把已经存在的初始共享描述为本次新增优化，也不能直接把验证缓存改成全操作常驻缓存。

完成要求：生产源码、全部消费者、项目引用、Solution、公开 XML、Wiki、架构门禁、成功/失败/取消/边界测试、本机发布路径和性能证据共同闭合。macOS 实机及需要真实焦点的 GUI gate 单独记录；未执行不得计作通过。

本次不升级目标框架，不修改 BGCS 源码、extern 或生成绑定，不增加生产 Program，不自动提交。未来 iOS、NS、Linux 产品不建立占位项目。最初按用户要求不使用 Computer Use；用户在续跑时已允许，追加 Windows 真实 GUI 证据。其他设备或尚未完成的交互门禁仍单列，不以后台测试替代。

## 2. 理想边界

```text
平台产品组合
├─ Sdl3PlatformBackendProvider(所选 SDL host integration)
├─ BgfxRenderingBackendProvider(所选 BGFX surface integration)
└─ 共享 Player / Editor Hosting / Shell

平台运行集成 → 对应 backend SPI + 中立 Adapter 契约
共享 backend → 中立领域/Adapter 契约 + 自己的 Native
平台构建集成 → 平台 Build 契约 + backend Build 契约
Standard Distribution → 平台 Build + backend Build + 所选构建集成
中立 Toolchains → 注入的绑定生成契约
Inno.Build.Bindings → BGCS 公开库 + 中立 Toolchains 契约
MSBuild Tasks → 共享构建应用流程
```

运行时和构建期集成具有不同程序集闭包。既有 `Inno.Integration.<Platform>.Bgfx` 保留构建职责；新增 `.Bgfx.Runtime` 只承载真实运行接入，不引用既有构建集成。

平台差异由 integration 显式选择；BGFX 的第三方 API 调用、renderer 行为与资源退休仍归共享 backend。中立 Rendering 不接收 BGFX SPI、native pointers 或具体集成类型。

## 3. R01：SDL 共同窗口操作唯一化

### 3.1 文件结构

```text
backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/
├─ Api/
│  ├─ ISdl3HostIntegration.cs                     保留，完善行为与所有权说明
│  ├─ Sdl3HostCapabilities.cs                     保留
│  └─ Sdl3WindowOperations.cs                     新增，两个窄的公共 SDK 操作
├─ Sdl3PlatformApplication.Windows.cs             修改，保留唯一注册和失败清理
└─ Sdl3PlatformWindow.Internal.cs                 复核，不另建窗口目录

platforms/<Windows|MacOS|Browser>/integrations/
└─ Inno.Integration.<Platform>.Sdl3/
   └─ <Platform>Sdl3HostIntegration.cs            修改，共同操作调用 SDL backend

backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/
├─ Sdl3WindowOperationsTests.cs                   新增
├─ HostIntegrationTests.cs                        修改/回归
├─ WindowOwnershipTests.cs                        回归
└─ WindowSurfaceTests.cs                          回归
```

### 3.2 最小 API

`Sdl3WindowOperations` 是无状态、SDL 所属的窄操作入口，不管理应用、窗口目录、事件或会话。仅公开：

- `CreateWindow(PlatformWindowOptions options, bool requireOpenGl)`：通过一个统一的 SDL properties 路径创建窗口，返回非零、不透明 SDL handle；失败释放临时 properties。成功返回后调用方接管窗口销毁权。
- `ReadFocus(nint windowHandle)`：读取现有 SDL window flags；不修改焦点，不注册窗口，不接管资源。

Windows/macOS 用 `requireOpenGl: false`；Browser 用 `true`。Windows/macOS 初始焦点调用共同读取；Browser 保留等候真实焦点事件的策略。平台 surface 解析及 macOS 初始化 hint 保留在各自 integration。

共同创建代码逐项对应现有 title、width、height、visible、resizable、highPixelDensity。不得因改为统一 properties 路径改变实际窗口行为或遗漏校验。

`Sdl3PlatformApplication` 仍唯一负责 owner thread、创建/接管登记、包装失败清理和退出。integration 成功创建窗口后转移所有权；外部窗口不转移原生销毁权。Core Events 与 ImGui 的窗口接入不另建机制。

### 3.3 R01 门禁

- 同一组 options 在 Windows 实际 SDL 上得到相同尺寸、可见性、可调整大小和 DPI flags。
- 创建失败不遗留 properties 或 window；包装失败只销毁新建窗口。
- 多应用目录隔离；外部窗口登记失败、解除登记和原 owner 销毁不重复释放。
- Browser 保持 OpenGL 属性、单窗口能力与初始失焦策略。
- 多视口、live-resize、焦点和失焦释放回归；macOS 本机行为另行实测。

## 4. R02：BGFX surface 运行接入开放化

### 4.1 文件与程序集

```text
backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/
├─ Surfaces/
│  ├─ IBgfxSurfaceIntegration.cs                  新增；含紧密相关的 surface role
│  ├─ BgfxSurfaceDescriptor.cs                    新增；只表达本次所需的初始化事实
│  └─ BgfxSurfaceBinding.cs                       新增，internal；验证后转换至 Native
├─ BgfxRenderingBackendProvider.cs                修改，显式接收运行集成
├─ BgfxDeviceOptions.cs                           修改，传递借用的运行集成
├─ BgfxDevice.cs                                  修改，主 surface 预检与初始化
├─ BgfxDevice.Surfaces.cs                         修改，附加 surface 与缓存描述
└─ Inno.Adapter.Rendering.Bgfx.csproj             复核边界

platforms/Windows/integrations/Inno.Integration.Windows.Bgfx.Runtime/
├─ Inno.Integration.Windows.Bgfx.Runtime.csproj    新增，库
└─ WindowsBgfxSurfaceIntegration.cs               新增，Win32 接入
platforms/MacOS/integrations/Inno.Integration.MacOS.Bgfx.Runtime/
├─ Inno.Integration.MacOS.Bgfx.Runtime.csproj      新增，库
└─ MacOSBgfxSurfaceIntegration.cs                 新增，Cocoa 接入
platforms/Browser/integrations/Inno.Integration.Browser.Bgfx.Runtime/
├─ Inno.Integration.Browser.Bgfx.Runtime.csproj    新增，库
└─ BrowserBgfxSurfaceIntegration.cs               新增，Canvas 接入

src/composition/adapters/
├─ Inno.Adapter.Default/
│  ├─ DefaultAdapterCatalogOptions.cs             修改，必填 rendering factory
│  ├─ DefaultAdapterCatalog.cs                    修改，删除隐式 BGFX 创建
│  └─ Inno.Adapter.Default.csproj                 删除不再使用的直接 BGFX 引用
└─ Inno.Adapter.Authoring.Default/
   ├─ DefaultAuthoringAdapterCatalog.cs           修改，消费统一 options
   └─ Inno.Adapter.Authoring.Default.csproj       复核公开/实现引用

backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/
├─ BgfxSurfaceIntegrationTests.cs                 新增
└─ BgfxDeviceTests.cs                             修改/回归；既有 Fixture/Collection 同步
tests/tooling/Inno.Tooling.Architecture.Tests/
└─ IntegrationBoundaryTests.cs                    通过公开架构 CLI 覆盖运行集成边界
```

不新增 Linux/iOS/NS 运行集成：目前没有相应产品实现与验收。已有四个 BGFX 构建集成继续保留。

### 4.2 SPI 与所有权

当前 `IBgfxSurfaceIntegration`：

- `supportsAdditionalSurfaces`：平台接入允许附加输出的明确能力；实际使用还必须满足 BGFX device 的 `GraphicsCapability.SwapChain`。
- `Resolve(PlatformNativeHandles handles, BgfxSurfaceRole role)`：把 Primary/Additional 角色对应的 adapter surface 转换为经过验证的 `BgfxSurfaceDescriptor`。未知 ABI、无效 handle 或不支持角色明确拒绝。

`BgfxSurfaceDescriptor` 只保存现有实现实际需要的 borrowed window/display handles 与主输出 sRGB reset 许可。当前无需求的 graphics context、外部 backbuffer、SDK 对象和 callbacks 不预留。将来有真实接入需求时在所属 SPI 精确扩展。

这份描述是 Adapter 内的转换结果，不是第二个窗口目录或 Native owner。原窗口 owner 始终负责销毁；device 只创建、缓存和退休 GPU surface。集成对象为不可变配置，由 provider/device 借用，不创建全局注册表。

`BgfxSurfaceBinding` 把描述转换成私有 BGFX PlatformData。BGFX structs/enums 只存在于本 backend/Native 内，三个 runtime integration 均不引用 `Inno.Native.Bgfx`。

### 4.3 主 surface、附加 surface 与颜色

1. 在取得 BGFX process lease 和调用 Native 前，校验必需集成、handle 与角色。
2. 主 surface 由注入集成解析，删除 `ApplyPlatformData` 内 Win32/Cocoa/Canvas 白名单。
3. sRGB reset 根据请求与集成许可组合；删除 device 中 `browserCanvas` 条件。Browser 接入保留当前有效的颜色行为。
4. 附加 surface 创建使用同一集成和能力检查，删除 `ValidateWindowHandles` 的固定 ABI 列表。
5. 解析后的描述在 surface 创建时固定；resize 复用描述，不每帧重新解析或扫描窗口。
6. 共享 backend 保留与 BGFX renderer 行为相关的颜色逻辑，例如当前 D3D 附加 framebuffer 的输出 transfer。不得把 renderer 规则搬到所有平台各复制一份。
7. 最小化/无可用输出仍使用 nullable presentation extent；内部最小资源尺寸不得冒充真实可呈现尺寸。
8. GPU surface 在帧安全点退休，原生窗口保持存活直到引用它的 GPU 工作安全退休；检查现有 ImGui 销毁顺序，若只依赖延迟 destroy 后立即释放窗口，必须一并修正。

关闭顺序专项覆盖 `BgfxImGuiRenderer.RetireViewport`、`PlatformImGuiViewportBackend.DestroyViewportWindow`、`BgfxDevice` 退休队列与 SDL application 的退出。SDK 对 destroy/提交/完成的实际保证已核对；不能把任意帧数计数当作 GPU 完成。延后原生窗口释放复用现有 presentation 生命周期，在 owner thread 完成两阶段关闭（停止交互与提交 surface 退休 → 确认 backend 安全点 → 原 owner 销毁窗口），不另建窗口权威表。completion 契约属于 backend/presentation 边界，并同步公开 API 与真实 Native 测试；不能把阻塞等待或可重入渲染塞入 ImGui Native callback。

本次不虚构通用 device-lost 自动恢复。真实 surface 销毁重建或外部 GPU context 将来接入时，必须再核对 generation、线程、重建与恢复协议；尺寸更新不能冒充完整设备恢复。

### 4.4 产品组合及默认 Catalog

`DefaultAdapterCatalogOptions` 新增必填 `IRenderingBackendFactory rendering`。删除 `DefaultAdapterCatalog`/`DefaultAuthoringAdapterCatalog` 的可选 `renderingProviders` 参数与隐式 `new BgfxRenderingBackendProvider()`；同步所有当前调用方，不保留旧 overload。

各平台产品显式创建 `RenderingBackendCatalog`，注册 `new BgfxRenderingBackendProvider(new <Platform>BgfxSurfaceIntegration())`。普通产品不再依赖构建期集成。StandardAdapterSelection 仍表达明确 backend ID，实际 provider 与 selection 在启动前一致性校验。

直接创建 windowless `BgfxDevice` 不要求平台 surface 集成；其后来请求 window surface 时若未配置，必须在 Native 调用前明确失败。Provider 注册要求非空集成；windowless 测试通过真实公开 device 入口验证，不添加测试专用实现。

同步以下消费者及项目引用：

- Windows/macOS EditorComposition、PlayerComposition；BrowserPlayerComposition。
- `BgfxImGuiRenderer` 的 surface 创建、resize、颜色查询及退出。
- Shell、Input、Editor Hosting、PlayMode、Rendering Runtime 中构造 Catalog 的测试。
- Canvas/Rendering2D 当前工具和测试中直接创建 Catalog/device 的消费者。
- 三个平台 Player Support Pack 模板、生成静态注册和实际发布闭包。

### 4.5 R02 门禁

- fixture 使用新的 surface ID，由注入集成映射真实测试窗口，验证无需修改 backend 白名单；不把伪造指针传给 Native。
- 无集成、错误 ABI、无效 handle、拒绝 Additional 都在 Native/process owner 创建前失败。
- integration 自己决定接受的 ABI；共享 backend 不出现平台 ID 判断或具体 runtime integration 类型。
- Browser 拒绝附加窗口；SDL 多窗口能力、集成许可及 BGFX SwapChain 三者缺一即拒绝。
- windowless rendering、最小化/恢复、尺寸/DPI、附加窗口 resize 与关闭、异常清理、颜色回归。
- fake 只实现公开 SPI；Native 调用使用真实 SDK 窗口。不得使用反射穿透、InternalsVisibleTo 或测试后门。
- Windows/macOS Player closure 不包含 Build、BGCS、构建期 integration 或 Authoring。


## 4.6 R02-L：渲染 surface 与附加窗口退休

当前实现正式以 Task 退休替换立即销毁：`BgfxDevice.RetireWindowSurface`、
`IPlatformImGuiRenderer.RetireViewport`；最终退出分别调用 owner-thread drain。
Task 只承诺 renderer 已停止使用借用窗口，不是 GPU fence。

```text
backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/
├─ Surfaces/BgfxSurfaceRetirement.cs               内部，每个 surface 的退休组
├─ BgfxDevice.Surfaces.cs                          resize 旧 framebuffer 纳入同一组
├─ BgfxDevice.Frames.cs                            单调提交序号与命令完成确认
└─ BgfxDevice.Retirement.cs                        RetirementBarrier，故障保留 owner
backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/
├─ Api/IPlatformImGuiRenderer.Api.cs               RetireViewport / DrainViewportRetirements
├─ Api/PlatformImGuiContext.Api.cs                 context 接管 renderer 的公开所有权
├─ Api/Sdl3PlatformApplicationImGuiExtensions.cs   创建/释放事务
├─ Context/
│  ├─ PlatformImGuiContext.cs                      从 Internal 纯迁移再调整生命周期
│  └─ PlatformImGuiContext.Clipboard.cs            纯迁移
├─ Viewports/
│  ├─ PlatformImGuiViewportBackend.cs              创建与输入路由
│  └─ PlatformImGuiViewportBackend.Retirement.cs   detach、延期销毁、最终 drain
├─ Rendering/
│  ├─ PlatformImGuiSdlRenderer.cs                  完成 Task 的 SDL renderer
│  └─ ImGuiPackedColor.cs                         纯迁移
└─ Interop/
   ├─ ImGuiPlatformIoNative.cs                     纯迁移
   └─ Sdl3PlatformWindowAccess.cs                  纯迁移
backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/
├─ BgfxImGuiRenderer.cs                           surface 退休与一次 Dispose
└─ ImGuiPresentationContext.cs                    删除重复 renderer Dispose
backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/
└─ BgfxSurfaceRetirementTests.cs                  实际 SDL/D3D11，线程模式分别运行
tests/editor/Inno.Editor.ImGui.Tests/
└─ ViewportRetirementTests.cs                     公开 renderer 替换，窗口存活与清理失败
```

原 Internal/*.Internal.cs 删除，namespace/程序集保持不变；纯移动内容摘要保存在
results/imgui-pure-moves.json。字段和现有目录仍各有一个 owner。

关闭顺序：先停止交互/注销登记/清除 callback userdata，再移除绘制和提交；
排入全部 framebuffer destroy 命令，下一次成功 frame 确认之前提交帧已被 renderer 处理，
最后由 SDL 原窗口 owner 在 managed 安全点销毁。使用 managed ulong 单调序号，
不比较 uint 帧号，也不把 deferredDestroyFrames 当作完成信号。
[BGFX 官方 frame 同步语义](https://bkaradzic.github.io/bgfx/bgfx.html#frame)与当前仓库命令队列共同核对。

Native callback 不等待 Task、不推进帧、不向原生侧抛异常。最终 drain 只在闭帧、
无 encoder/graph 的 owner-thread 安全点推进不呈现帧，并使用 RetirementBarrier 有界协调。
超时、Faulted 或 native frame 失败保留窗口、设备及 callback owner，不能继续释放依赖。
成功创建 context 才接管 renderer；重复创建拒绝；失败由创建方清理尚未转移的 renderer。

## 5. R03：构建应用流程复用、验证阶段与 Task 成本

### 5.1 先测量，再确定收益

现有统计仅能证明成本，不能把 17 个载入目录当作 17 次完整编译，也不能把 Native 工具进程 0 当作没有托管/生成进程。

新增分阶段统计：初始输入读取、锁等待后验证、构建后验证、输出完整性、绑定输入发现/解析/生成、SDK 探测、Task bootstrap/build/publication/private loading。分别记录耗时、读取次数/bytes、唯一文件、重复读取、进程、复制/hardlink bytes、载入目录和清理结果。

同环境记录一次明确准备构建、三次串行热构建和两个真实并发构建。冷热定义、OS 文件缓存和源码闭包必须记录；不得清理用户/SDK 全局缓存来制造基准。

### 5.2 绑定生成成为共享库

```text
build/bindings/Inno.Build.Bindings/
├─ Inno.Build.Bindings.csproj                     新增，库；引用 BGCS 公开库
├─ NativeBindingGenerator.cs                      新增，唯一公开应用入口
├─ Configuration/BindingGenerationConfiguration.cs
├─ Identity/NativeBindingGenerationIdentity.cs    从 Tasks 迁入
├─ Generation/BindingGenerationOperation.cs       单次 batch 生命周期 owner
├─ Generation/HostBindingGeneration.cs            迁入现有 host 流程
├─ Generation/TargetBindingGeneration.cs          迁入现有 target 流程
├─ Publication/BindingGenerationPublication.cs    staging、校验和原子发布
└─ Output/BindingGenerationDiagnostics.cs         结构化结果与诊断

build/toolchains/Inno.Build.Toolchains/
├─ NativeBuildContext.cs                          修改，显式借用生成契约
└─ Native/
   ├─ INativeBindingGenerator.cs                  新增，中立 provider 边界
   ├─ NativeBindingGenerationRequest.cs           新增，不含 BGCS 类型
   ├─ NativeBindingPreparation.cs                 修改，调用注入库入口
   ├─ NativeBindingGenerationDescriptor.cs        修改/复核完整输出校验
   ├─ NativeBuildInputState.cs                    修改，明确验证阶段
   ├─ NativeInputSnapshot.cs                      修改，拆出读取 cache
   ├─ NativeInputReadCache.cs                     新增，由原文件拆出
   ├─ NativeInputVerificationScope.cs             新增，internal，阶段 owner
   ├─ NativeBuildPhaseStatistics.cs               新增，纯诊断值
   ├─ NativeBuildStatistics.cs                    修改，完整阶段计数
   ├─ NativeArtifactPublisher.cs                  修改，使用明确阶段并记录输出成本
   ├─ NativeBuildFingerprint.cs                   修改/复核完整身份
   ├─ NativeBuildRecipe.cs                        复核声明闭包
   ├─ NativeInputMaterializer.cs                  复核固定输入与复制校验
   ├─ NativeCMakeSource.cs                        冷 producer 的独立复制树及严格输入路径解析
   ├─ NativeCMakeExecutor.cs                      桌面／Browser 共用 source 准备与执行
   └─ ProductNativeBuildPlan.cs                   修改，整个闭包一次绑定生成请求

build/tasks/Inno.Build.Tasks/
├─ GenerateBindingsTask.cs                        修改，只保留 MSBuild 参数/输出/取消适配
├─ NativeBindingGenerationIdentity.cs             删除旧所属文件，迁入 Bindings
├─ PrepareProductNativeTask.cs                    修改，注入生成实现与分阶段报告
├─ CompileShaderTask.cs                           修改，共用生成/准备流程
├─ PublishSupportPackTask.cs                      修改消费者
└─ Inno.Build.Tasks.csproj                        修改引用与编译实现身份
```

中立 Toolchains 不引用 BGCS、MSBuild 或具体 Bindings 实现。Standard Distribution/Task composition 负责注入库 provider；共同模块不能从目录或环境变量自动发现它。

`NativeBindingGenerationRequest` 冻结配置、目标、组件、CheckOnly/输出模式及所需执行上下文。`GenerateAsync` 返回完整不可变 descriptor 集合；返回前所有生成工作已经完成，失败/取消没有部分成功结果。

移除 `NativeBindingPreparation` 为执行引擎自己的生成流程而创建 `GenerateBindings.proj`、再启动子 `dotnet msbuild` 的路径。SDK 编译与平台原生工具仍由对应 executor 管理；BGCS 本身通过其公开库调用，保持独立。

保留当前 host/target 输出、C++ bridge → C header → managed binding、CheckOnly、lowering/plugin 指纹、目标隔离、required export 校验和完整诊断。生成逻辑迁出时同步 `Inno.BindingImplementation` 源码身份所属，不能让新公共库缺少生成实现身份，也不以无关程序集 MVID 使全部组件宽泛失效。

### 5.3 输入与验证阶段规则

1. 同一 batch 初始输入集合按真实物理文件去重，保留全部逻辑身份及输入声明；include 顺序不能排序抹平。
2. 同一个有明确定义的验证阶段，每个物理文件最多读取一次；下一次独立锁等待、冷生成/编译或提交前验证建立新阶段。
3. **不能跨锁等待或 Native 工作复用旧验证结论。** 初始 snapshot 相同不意味着现在文件仍相同。
4. Batch 生成只有在统一取得按规范顺序排列的 generation leases、建立 fresh union verification 后，才允许共享该批验证结果；单组件调用也走同一机制。不得在不同等待区间拼接缓存。
5. Cold generation 必须使用 operation 固定的源码输入；沿用 NativeInputMaterializer 的内容核对，不能用链接回可修改源文件的 hardlink 冒充不可变输入。SDK/工具输入继续按实际读取与稳定性约束验证，额外物化成本必须计入基准。

实施复核追加：冷 CMake 同样通过 publisher scope 固定工作区输入，Browser 的组件 include、bridge 与定义使用同一复制树。原始输入的阶段哈希仍保留，输出与外部 SDK 不重定向；热命中不复制。真实 CMake 测试临时修改并恢复原 header，调用产物确认编译器只读取固定内容。
6. 批次完成后重新检查完整声明集合与哈希；缓存完整性单独验证。取消/变化发生在提交前时不返回部署描述，保留上次完整输出。
7. 单个完整、按自身 fingerprint 发布的生成缓存可保留；产品发布和请求结果必须是完整闭包。不得靠回滚有效共享缓存破坏其他消费者。
8. 保留每个 Native artifact 的锁等待后 fresh verification 和冷编译后 fresh verification；本次不把不同组件的这些独立阶段强行合并。
9. 源码、facade、配置、生成器/lowering、compiler/SDK/sysroot/linker、ABI、include 顺序、参数、绑定与输出声明继续参与身份；不基于 length/mtime 命中有效缓存。
10. 只缩小被证明与 recipe 无关的输入；生成器与 compiler 的真实依赖必须留下。现有 executionRoot 等影响实际产物的参数不能仅为跨 checkout 复用而删除。

### 5.4 Task 生命周期复用

```text
build/tasks/Inno.Build.TaskHosting/
├─ BuildTaskRuntimeTask.cs                        修改，接入 Build 作用域请求复用
├─ TaskRuntimeRequestKey.cs                       新增，internal，完整准备身份
├─ TaskRuntimeBuildScope.cs                       新增，internal，作用域 owner
├─ TaskRuntimePreparation.cs                      新增，internal，单次完整准备
├─ PublishTaskHostTask.cs                         修改，共用不可变 publication
└─ PublishTaskHostTask.Retirement.cs              修改/回归，活跃载入保护

build/msbuild/Inno.Build.Tasks.targets            修改，共用清理后的工具属性与准备请求
Directory.Build.props / targets                  修改薄集成
```

- 使用 MSBuild 的 Build 作用域（`IBuildEngine4` task object lifetime），相同请求在同一个执行节点内复用完整准备结果；不同进程通过 Core.IO FileLease 协调 publication。
- 本机当前 Microsoft.Build.Framework 17.12.50 的公开契约确认 Build lifetime 在构建结束时释放注册对象。作用域仅保留有界请求/结果 metadata，不持有完整文件 bytes；有生命周期必要性的 owner 设置禁止提前回收，并在 Dispose 中完成取消与释放。释放线程可能任意，不能借它调用有 owner-thread 要求的 SDK。
- 请求身份包含源码/工具闭包、SDK、配置、清理后的全局属性、BGCS/扩展实现与输出 owner。目标 RID/Wasm/AOT/产品属性不污染工具身份。
- 跨私有载入上下文的共享数据仅使用 BCL/MSBuild 中立 metadata，不能依赖私有类在两个载入上下文中具有相同类型身份。
- 保留最小 bootstrap 隔离、节点 Yield/Reacquire、immutable host、私有载入 reader 和 owner retirement。不得重新引入共享 obj 并发 `CS2012`。
- 首先消除重复 preparation/hash/publication；载入目录按 MSBuild 的实际隔离边界管理。只有真实证明相同 Build scope、节点及 runtime identity 能安全共用，才复用同一个私有载入目录。
- 不承诺全进程只有一个目录；不同 Task runtime 内容或独立构建使用独立 owner。节点复用时不能继续使用上一轮源码对应的载入结果。
- 等待者取消不销毁其他消费者使用的准备结果；作用域结束释放请求状态。已被 CLR/MSBuild 载入的程序集目录在其进程仍活跃时不强行删除。
- 未知 owner 状态、权限拒绝、异常退出遵循现有保护规则；不能通过删除锁文件或强行删除活跃目录清理空间。

### 5.5 R03 性能及正确性门禁

- 正常 Product Native 绑定准备不启动引擎自己的嵌套 generator MSBuild 子进程。
- 同一节点、同一 Build scope、同一完整请求的 runtime preparation 为一次；新源码/SDK/配置与新 Build scope 重新判定。
- 相同验证阶段重复物理文件哈希为零；独立安全边界的必要再读单独计数。
- 热构建不启动 Native compiler，不重生成匹配的绑定，不替换相同部署文件或其 mtime。
- 同长度同 mtime 输入篡改、文件增删、参数/SDK/facade/lowering 变化均正确失效；输出损坏必须修复或明确失败。
- 锁等待期间输入变化、生成/编译中输入变化、预取消、等待取消、权限拒绝、并发与 Task 源码变化覆盖成功/失败路径。
- 至少两个真实并发 MSBuild，无死锁、CS2012、错误 runtime 内容或互删目录。
- 同环境三次热构建比较分阶段数据。总耗时若没有明显降低，必须解释成本去向并继续处理测量确认的重复工作；不得以目录数下降宣称性能提升。
- 完整哈希的剩余不可消除成本明确列出；不得承诺零 IO、零目录或任意机器固定秒数。

## 6. 测试、消费者与架构门禁

新增/修改现有公开边界测试，不通过扩大 internal 或反射穿透验证内部计数：

```text
tests/build/Inno.Build.Tests/
├─ BindingGenerationBatchTests.cs                 新增
├─ NativeVerificationPhaseTests.cs                新增
├─ TaskRuntimeBuildTests.cs                       通过公开 Task 覆盖 Build 生命周期复用
├─ TaskRuntimeBuildTests.cs                       回归 CS2012/Yield/Reacquire
├─ TaskHostPublicationTests.cs                    回归 ownership/retirement
├─ NativeArtifactIdentityTests.cs                 修改/回归
├─ NativeArtifactPublicationTests.cs              修改/回归
├─ NativeBindingGenerationTests.cs                修改/回归
├─ NativeBindingCompilationTests.cs               实际消费编译
└─ PlatformBackendIntegrationTests.cs             修改/回归
tests/tooling/Inno.Tooling.Architecture.Tests/
├─ IntegrationBoundaryTests.cs                    通过公开架构 CLI 覆盖运行集成边界
└─ IntegrationBoundaryTests.cs                    修改/回归
```

计数通过实际公开构建结果/诊断与进程日志取得；不会为测试增加生产回调或后门。

工具与规则同步：`ArchitectureRules`、`ArchitectureValidator`、`PublicApiBoundaryValidator`、`GenerationCleanupValidator` 和项目/文档覆盖。要求：

- 共同 Services/Runtime/Player/Hosting 不引用 runtime integration 或 BGFX SPI。
- runtime integration 不引用 Build、BGCS、Native.Bgfx、其他平台或构建 integration。
- 共享 backend 不引用任何 integration 或平台实现；允许对应 SPI 的 Adapter-level borrowed handles。
- TaskHosting 保持只依赖必要 MSBuild/Core.IO；绑定应用库不引用 Tasks 或具体平台。
- Player 闭包不含 Bindings/构建期集成；新项目全部入 Solution，无空 Folder、重复程序集、冗余 Program。
- 全部旧白名单、隐式 provider 创建、旧 optional overload、旧生成 subprocess 和迁移文件没有生产残留。
- 持久身份、脚本 namespace、Core Events、Identity、Missing/last-good、History/Workspace 和 GC unload barrier 不改变。

## 7. 文档及执行证据

执行时建立：

```text
artifacts/acceptance/backend-runtime-boundary/
├─ baseline/
│  ├─ revisions.json
│  ├─ environment.json
│  ├─ working-tree.diff
│  ├─ project-dependencies.json
│  ├─ public-api.txt
│  └─ performance.json
├─ file-map.tsv
├─ results/
├─ logs/
└─ captures/
```

file-map 覆盖每个旧/新源文件、项目、模板、生成配置、测试、工具和 Wiki；记录责任、操作、API 变化、消费者、对应 gate 和完成状态。以引用闭包和编译补全，不把示例清单当作所有消费者。

实施过程中同步：

- `BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_ACCEPTANCE.md`：新增实际验收报告。
- Architecture README、CURRENT_ISSUES、ENGINE_ARCHITECTURE_OVERVIEW、PLATFORM_RUNTIME_ARCHITECTURE、PLATFORM_EXTENSION_GUIDE。
- SDL、BGFX、Default/Authoring Catalog、Toolchains、Tasks、TaskHosting 项目页。
- `docs/platform/<Platform>/Inno.Integration.<Platform>.Bgfx.Runtime.md`：三个新运行集成项目页及平台索引。
- `docs/build/Inno.Build.Bindings.md`：新共享绑定应用库页及 Build/Wiki 覆盖。
- AGENTS 中现有边界合并更新，不重复堆叠原则；新增公开 API 完整英文 XML、多参数排版、显式 using。

文档明确标注方案、当前实现与本轮证据。历史通过结果不替代本轮 gate。

## 8. 实施顺序

| 阶段 | 任务 | 通过标准 |
| --- | --- | --- |
| 0 | 新 revision/工作区/依赖/运行基线及成本分阶段记录 | 不覆盖未提交修改，问题与计数可复现 |
| 1 | SDL 窗口共同操作与三个 integration 消费者 | Native flags、失败清理、焦点/owner 回归 |
| 2 | BGFX SPI、三个 runtime integration、显式 Rendering factory | 启动预检、颜色、附加 surface、windowless 与闭包通过 |
| 3 | 绑定业务迁入 Bindings 库；Task/CLI/产品注入；删除 nested MSBuild | 生成身份、CheckOnly、host/target 与实际绑定消费通过 |
| 4 | 验证 scope、binding batch 与统计 | 同长度篡改/增删/等待变化/取消/并发全部通过 |
| 5 | Task Build scope 请求复用与安全退休 | 单次准备、源码更新、实际并发、Design-time/IDE Build 通过 |
| 6 | 完整产品/消费者矩阵、三次热构建与 Wiki/Solution | 无旧实现、所有可执行 gate 通过，性能证据闭合 |

每阶段同步全部消费者、编译相关闭包、通过必要测试，再进入下一阶段。纯迁移与行为修改分开核对 token、初始化及释放顺序。

## 9. 实际发布、运行与扩展验收

使用 `InnoEngine.Samples/FlappyBird`，重新验证 Windows Debug/Release 普通 Editor Build、Windows CoreCLR/NativeAOT、Web 解释执行/AOT 的发布与实际运行；Canvas/Rendering2D 编译与 Plugin/脚本消费。

颜色回归覆盖昼夜亮度、光照方向和移动、星光；主/附加 surface 的 sRGB 行为分别留证。后台验证可以执行输入、像素、存储与音频样本检查，不能替代真实焦点、多屏 DPI、live-resize 或声学听音。

用户允许后执行浮动 GameView、前景/Popup/Modal 消费、附加窗口关闭/resize、最小化/恢复、DPI/显示器迁移。macOS 本机 Native/Editor/Player 单独记录；未具备设备的 iOS/NS 不声明支持。

扩展示例必须证明：

1. 新 SDL 宿主复用共同窗口操作，只实现真实配置和 surface 接入。
2. 新 BGFX surface ABI 由新的 runtime integration 接入，不修改共享 device 白名单。
3. 已有 surface 类型的新 CPU 目标复用运行集成，只调整明确目标/ABI/构建配置。
4. 新图形 backend 不需要 BGFX SPI，使用领域中立 rendering factory 注入。
5. 新 SDK/生成器实现替换中立生成 provider，不改变共同领域或让 Player 引入构建库。

## 10. 最终交付与验收口径

报告与对话共同总结 R01–R03 的问题、原因、最终归属、API 必要性、迁移/删除清单、命令/revision/SDK、测试、实际运行、性能前后数据和未实测项。

性能结论必须区分被消除的重复工作、仍需执行的完整校验和实测总耗时。发布与源码整改完成不能冒充全部设备/GUI 验收完成。

阶段结束检查 C 盘；只清理本轮精确归属、无活跃 owner 的缓存。保留当前产物、用户/SDK 缓存及验收日志。支持按 `type(scope): subject` 给出 commit title，不自动提交。

建议最终拆分：`refactor(sdl): centralize shared window operations`、`refactor(bgfx): inject runtime surface integration`、`refactor(build): share binding generation pipeline`、`perf(build): reuse scoped preparation and verification`、`docs(architecture): record runtime boundary acceptance`。

指定 `/System/Library/Sounds/Glass.aiff` 在当前 Windows 环境不可访问；本次设计提示音未播放。

## 11. 本次实际追加的生成定义、恢复与消费闭包

```text
backends/{Sdl3,Bgfx,ImGui,MiniAudio,Text,RmlUi}/native/Inno.Native.*/Bindings/
└─ bindings.props                                共 7 份；另含 ImGuizmo，唯一配置位置
build/bindings/Inno.Build.Bindings/
├─ Configuration/BindingDefinitionReader.cs       严格相对 literal 数据读取
└─ Generation/BindingExtensionPreparation.cs      真实扩展源码/SDK/生成器身份与 immutable 发布
build/tasks/Inno.Build.TaskHosting/
└─ RestoreOwnedProjectTask.cs                     共享 restore 图写 lease + Yield/Reacquire
build/toolchains/Inno.Build.Toolchains/
├─ NativeBuildProduct.cs                          返回已选 bindingGeneration
└─ Native/NativeBindingGenerationDescriptor.cs    WriteSelection 保持原生/托管同一身份
build/support/Inno.Build.SupportPacks.Core/
└─ FilePlayerSupportPackPreparation.cs            managed build 使用实际 Native selection
tests/build/Inno.Build.Tests/
├─ OwnedRestoreTests.cs                           并发/取消/错误/显式属性与转义
└─ NativeBindingGenerationTests.cs                选择顺序、冲突拒绝、无效名称与旧输出保留
```

bindings.props 只声明 host config/bridge、实际 target 映射与 extension project；禁止条件脚本、
环境推导、任意 MSBuild 表达式和目录逃逸。目标 bridge 缺失直接失败，没有 ABI fallback。
Native .csproj 导入、component descriptor 指向、库读取与 Task 使用同一个定义。
Component descriptor 原 bindingConfig 删除。

批次 canonical 排序取得全部 generation lease；fresh union 校验分别位于锁后和生成后。
每个 native artifact 的独立锁后、冷编译后和实际输出保留独立完整校验。
Native 产品返回它已选 bindingGeneration；Support Pack 把该选择传给后续 managed build，
不能重新推导另一份绑定或再请求同一产品闭包。只读选择文件属于操作 staging。

基线并发发现普通内部 NuGet restore 争用同一 interop obj：在既有 TaskHosting 使用
RestoreOwnedProjectTask 协调实际写入，复用 Core.IO，不修改 BGCS、SDK 或全局缓存。
等待节点 Yield，获得 lease 后 Reacquire；活动 restore 清理后释放 lease，取消不删除锁文件。
Shared registry 只保存 BCL rows 和文件 metadata，不保存 private ALC 类型、SDK owner 或 lease。

实际实现文件、受影响的保留文件与验收状态最终完整归入 file-map.tsv；没有新增其他生产库或 Program。

## 实施中发现的托管并发写入边界

实际两个普通 SDK 进程共用 bin/obj 会产生 GenerateDepsFile 占用和 CS2012。整改尊重标准 SDK ArtifactsPath，为独立请求建立独立托管输出 owner，并使 interop 编译/还原跟随该 owner；不扩大 Native 锁覆盖整个项目图。新增 tests/build/Inno.Build.Tests/BuildOutputOwnershipTests.cs 通过实际并发 SDK 编译验证此边界。失败日志保留，最终并发验收必须使用声明明确的独立输出根。

## 实施中发现的直接生成 SDK 环境边界

Emscripten 路径原由生成子进程环境提供；改为直接库调用后，配置中的 SDK 路径必须显式取自冻结 toolchain。
新增 Configuration/BindingConfigurationPaths.cs，managed 与 bridge 统一展开，不修改进程环境、不按平台名称分支。
CLI 与 Task 传入发行贡献的同目标 SDK；独立绑定定义不必注册产品 publisher，但缺少实际 SDK 配置时严格失败。
BindingGenerationBatchTests 增加 SDK 路径隔离与失败保护；BindingToolchainTaskTests 验证缺失定义和预取消不产生候选。
NativeBindingGenerationTests 通过公开生成库验证业务流程，NativeBindingCompilationTests 继续验证真实 MSBuild 消费。
