# Native 构建的平台职责与所有权

[架构索引](README.md) · [Wiki 首页](../README.md) · [缓存协议](BUILD_CACHE_ARCHITECTURE.md)

## 当前职责

共享 backend 唯一拥有组件源码、facade、公共 BGCS 语义及可移植 recipe。平台基础 build 拥有目标身份、ABI、SDK 与通用链接设置。平台 Build integration 将 SDK、BGCS Profile、组件选项、执行器和部署布局组合为完整贡献。标准发行只注册完整贡献与托管部署实现。

| 内容 | 唯一 owner |
| --- | --- |
| Windows junction 与执行路径 | `platforms/Windows/build/Inno.Build.Windows.Host` |
| macOS 宿主能力 | `platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Host` |
| 编译器、SDK、系统头和链接格式 | 对应平台 `Toolchain` 与 `Native` |
| BGCS ABI 与 managed runtime | 对应平台 Build integration 的 `Bindings` |
| Native 库导入名、组件特定链接参数 | 对应平台 Build integration 的 `Products` |
| 产品部署路径 | 对应平台 Build integration 的 `Deployment` |
| 组件闭包与逻辑依赖 | `build/products/Inno.Build.Products.Standard` |
| 请求、执行、冻结、lease、发布与维护 | 中立 Toolchains / Composition / Core IO |

平台基础模块不引用 backend 或 BGCS；integration 可以依赖本平台基础模块、公共产品库及 backend。平台不能互相引用。Browser 借用 `BuildHostServices.workspaces` 执行工具，不依赖 Windows 项目。

平台贡献同时声明支持的 managed deployment ID。发行按该声明从平台 descriptor 取得 RID，并配置共享 .NET 发布器；发布器不内置平台 RID 名单。组件专属导出宏也通过 integration 的构建参数传入，共享 CMake 不自行选择 Windows 导出策略。

## 绑定协议

使用精确固定的 BGCS **3.0.1** 正式 NuGet 包。每个组件只有 `Bindings/bindings.props` 和公共 `bindgen.json`；前者只声明 `InnoNativeComponentId`、`InnoBindingConfig`。公共配置保存头文件、命名、选择、封送与输出格式，不保存平台 target 或 SDK 搜索表。

平台通过官方 `TargetConfiguration` 构造 `NativeBindingProfile`，再贡献 `INativeBindingGenerator`。生成器仅使用官方 `GenerationRequest.FromFile`、`WithConfiguration`、分析与生成 API。`NativeCompilerInputs` 已包含绝对编译器、sysroot、有序系统头、宏和参数；BGCS 接入不解释 INCLUDE、SDKROOT 或 EM_CACHE。

`HostSource` 只选择组件源码输出位置，ABI 仍必须明确。`TargetArtifacts` 发布独立目标产物。两者均禁止回退到宿主 ABI。BGCS 的源码、包内容、二进制与生成结果均不手工修改。

## 执行与产物

`NativeProductContribution` 显式绑定 `ProductNativeBuildPlan` 与 `INativeProductExecutor`。普通组件执行器和 Browser 聚合执行器返回相同的 `ProductNativeBuildResult`。链接方式由组件选项声明，不从执行器或空 delegate 推断。

`NativeArtifact` 明确声明 SharedLibrary、ImportLibrary、StaticArchive 或 SupportFile。publisher 验证声明与全部实际文件一致，并将角色记录纳入完整性清单。ImGuizmo 消费 ImGui 声明的链接角色，不推导文件后缀。消费者必须持有结果，直到所有编译、链接和复制结束。

一次操作按顺序完成：预检能力 → 冻结 host/SDK/Profile/组件 → 计算身份 → 等待 lease 后 fresh 校验 → 复用或正式生成 bindings → 已选执行器 → 验证完整产物 → 原子发布和布局 → 托管消费同一选择 → 成功根操作维护一次缓存。

CMake 的源码参数、构建输出及进程工作目录均使用宿主提供的执行目录。进程不以原始 checkout 作为工作目录；长路径与跨卷映射只由宿主 workspace provider 处理，共享执行器不判断盘符或操作系统。

操作开始时将 checkout 解析为固定物理 owner，后续派生 context 不重新选择 owner。源码输入可以经过只读别名；声明的别名及其物理位置均映射到同一冻结复制树，并保留原读取路径以验证变化。外部 SDK 路径保持自己的位置，不通过全仓库字符串替换重定位。

## 文件树

```text
build/
  bindings/Inno.Build.Bindings/
    Configuration/NativeBindingProfile.cs
    Configuration/BindingDefinitionReader.cs
    Generation/BindingGenerationOperation.cs
    Identity/NativeBindingGenerationIdentity.cs
  products/Inno.Build.Products.Standard/
    StandardNativeComponents.cs
    StandardNativeDeployment.cs
    StandardSupportPackRequirements.cs
  composition/Inno.Build.Composition/
    BuildPlatformContribution.cs
    NativeProductContribution.cs
    BuildDistribution.cs
  toolchains/Inno.Build.Toolchains/
    Host/{BuildHostServices,IBuildHostProvider,IToolchainWorkspaceProvider}.cs
    Native/{NativeCompilerInputs,NativeLinkPlan,NativeArtifact,INativeProductExecutor}.cs
    Native/{ComponentNativeProductExecutor,NativeArtifactPublisher}.cs
    Cache/{BuildCachePublisher,BuildCacheRetirement}.cs
    Maintenance/BuildMaintenanceApplication.cs
  tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs
  distributions/Inno.Build.Distribution.Standard/
    StandardBuildDistribution.cs
    StandardBuildHosts.cs
    StandardDevelopmentProducts.props
platforms/
  Windows/build/Inno.Build.Windows.Host/Workspace/
    WindowsToolchainWorkspaceProvider.cs
    WindowsToolchainDirectory.cs
  <WindowsX64|MacOSArm64|BrowserWasm32>/
    build/Inno.Build.<Platform>/
      Toolchain/<Platform SDK provider>.cs
      Native/<Platform>LinkerSettings.cs
      ProductBuild.props
    integrations/Inno.Integration.<Platform>.Build/
      <Platform>BuildIntegration.cs
      Bindings/<Platform>BindingProfile.cs
      Products/<Platform>NativeProducts.cs
      Deployment/<Platform>NativeLayout.cs
  BrowserWasm32/build/Inno.Build.BrowserWasm32/Native/
    BrowserWasm32NativeProductExecutor.cs
backends/<Component>/
  native/Inno.Native.<Component>/
    Bindings/{bindings.props,bindgen.json,<actual extensions>}
    Native/{<facade>,Generated/}
    Generated/Bindings.cs
  build/Inno.Build.Toolchains.<Component>/
    <portable recipe>
    obj/native/<target>/<workspace identity>/
src/foundation/core/Inno.Core.IO/
  FileLease.cs
  CoordinatedFileLease.cs
  ProcessOwnedDirectory.cs
entry/
  DevelopmentHost.props
  InnoEngine.Editor/{InnoEngine.Editor.csproj,Program.cs}
  InnoEngine.Build/{InnoEngine.Build.csproj,Program.cs}
```

不存在未实现的 Linux 产品配置或支持声明。目录结构及 fixture 不能代替平台实机验收。实际结果与未执行范围见[当前验收](CURRENT_ACCEPTANCE.md)。
