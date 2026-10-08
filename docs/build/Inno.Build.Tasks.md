# Inno.Build.Tasks

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Shader 库](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md) · [Support Pack](Inno.Build.SupportPacks.Core.md)

## 职责与依赖

ImGui Shader 的平台/API 集合由平台 Editor 产品显式注入，不由后端列出平台或根据宿主推导。`CompileShaderTask` 按实际离线工具身份、源码完整内容、compiler runtime、平台/API/配置/资源名生成指纹；每个 API 的 `Outputs` 与 manifest 由写 lease 和原子目录事务发布。热构建完整验证后复用，输入同长度同 mtime 修改或输出损坏也会重建；编译过程中输入变化时拒绝候选。共同 Native 哈希、完整性和 Core.IO 机制被直接复用。

MSBuild 的薄适配层，调用与 CLI 相同的库。引用固定版本的 Microsoft.Build 编译契约包，执行时由宿主 SDK 提供实现程序集；不引入另一套命令解析或发布流程。

## 全部公开 API

| 类型/成员 | 语义 |
| --- | --- |
| `CompileShaderTask` | sealed MSBuild Task；AssetRoot、ShaderPath、Platform、Renderer、OutputFile 为必填输入。 |
| `CompileShaderTask.Execute()` | 编译图产物，成功 true，异常作为 MSBuild 错误并返回 false。 |
| `PublishSupportPackTask` | sealed、可取消发布 Task；EngineRoot、OutputRoot、Target 必填，DotnetHost 选择 SDK。 |
| `Execute()` / `Cancel()` | 调用核心 publisher；失败返回 false；取消可在执行前或执行中请求，锁保护取消与 disposal。 |
| `GenerateBindingsTask` | sealed、可取消 BGCS 库适配；ConfigPath、OutputDirectory 必填，BridgeConfigPath 指定共同 facade，TargetOutputRoot 选择按生成指纹隔离的目标输出。 |
| `GenerateBindingsTask.CheckOnly` | 在独立 staging 重新生成并比较，不提交变更。 |
| `GenerateBindingsTask.DescriptorOutputPath` | 可选的请求独占描述文件，仅在成功后写入。 |
| `GenerateBindingsTask.ExpectedFingerprint` | 可选的冻结身份；当前输入不匹配时在生成前失败，避免托管绑定与已编译原生库来自不同请求。 |
| `GeneratedBindings` / `GenerationFingerprint` / `NativeBridgeDirectory` | MSBuild 输出参数；表达本次校验成功的源、身份和可选桥，不能由固定 current 目录推断。 |
| `GenerateBindingsTask.Execute()` / `Cancel()` | 失败返回 false 并记录结构化错误；取消等待生成流程退出，在目标 staging 提交前检查取消。 |

没有供派生实现者使用的 protected 扩展点。SDK Task 的基础属性属于 MSBuild。

## 常见工作流

```xml
<UsingTask TaskName="Inno.Build.Tasks.CompileShaderTask" AssemblyFile="/build/Inno.Build.Tasks.dll" />
<CompileShaderTask EngineRoot="/engine" ToolTarget="windows-x64"
                   AssetRoot="/assets" ShaderPath="ImGui.ishader"
                   Platform="WindowsX64" Renderer="Direct3D11" OutputFile="/artifacts/ImGui/Outputs/ImGui.bin" />
```

调用前构建 Task 库。平台输出属性从宿主 Task 构建中移除，避免将执行于桌面的编译器构建为 browser-wasm。Task 加载失败即构建失败，不能静默使用陈旧产物。
Task 引导、不可变运行闭包及私有加载目录写入 `artifacts/build-tools/tasks`；扩展工具有独立宿主输出，均与产品产物隔离。
组件引用的 BGCS.Runtime 使用标准 SDK `ArtifactsPath` 选择自己的目标输出；独立 BGCS 仓库不承担引擎路径规则。

## 产品原生部署 Task

`PrepareProductNativeTask` 接收明确的 `ProductId`、`TargetId`、`EngineRoot`、`OutputDirectory` 与 `Configuration`；执行 SDK 来自当前 MSBuild 宿主环境。离线 Shader Task 的 `ToolTarget` 与游戏 Shader 目标分别声明。
`Execute()` 通过同一 Standard Distribution 获取产品闭包和目标工具链，调用 `ProductNativeBuildPlan` 与 `ProductNativeDeployment`；成功返回 true，错误记入 MSBuild 并返回 false。
`Cancel()` 在执行前或执行中取消拥有的进程树和 staging；不扫描 `.lib` 或固定配置 glob。
普通 Editor Build 在 `AfterTargets="Build"` 安装到 MSBuild 的 `TargetDir`；Publish 在最终 `PublishDir` 安装同一契约的产品集合，框架 reference pack 使用 MSBuild 实际解析的 targeting pack。设计期构建不执行原生准备。
Task 不成为产品的 managed runtime 引用；产品 RID、AOT、Native profile 和 IDE 全局属性从工具引导中移除。CLI `engine` 使用普通产品 Build 入口部署，不维护另一处固定 bin 路径。
Task 的取消、编译、产物校验和部署失败都使 MSBuild 失败；内容一致的原生树保留原文件。






## 本轮边界与所有权

唯一 MSBuild 引导位于 `build/msbuild/Inno.Build.Tasks.targets`。[TaskHosting](Inno.Build.TaskHosting.md) 用精确文件集合和 SHA-256 发布一份内容寻址运行闭包；各 evaluation 使用私有加载目录，优先硬链接复用完整依赖。普通 Build 与 Publish 准备对应产品 Native 闭包；Design-time 不执行 Native。`PrepareProductNativeTask` 输出输入扫描、进程与耗时指标。

内部加载目录清理只在全部 owner 进程已确认退出时删除旧缓存。系统拒绝查询进程状态时保留目录，
不能因此使有效构建失败；编译、完整性校验和部署失败仍按正常 Task 错误处理。

同一次 MSBuild 生命周期的 Shader Task 只在首次准备时解析离线 SDK；后续 Task 借用同一已验证的 immutable 工具产物，不重新读取 PATH 或选择 SDK。中立文件路径记录通过 BuildEngine 的 build-lifetime ownership 跨独立 Task load context 复用；每次使用仍验证完整工具输出，构建结束释放该记录。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Tasks.CompileShaderTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.CompileShaderTask`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L21) | Connects MSBuild to the same graph compiler used by the unified build workflow. |
| [`override bool Inno.Build.Tasks.CompileShaderTask.Execute()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L82) | Compiles the requested graph artifact and reports compilation failures through MSBuild. |
| [`string Inno.Build.Tasks.CompileShaderTask.AssetRoot`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L41) | Gets or sets the authoring source directory. |
| [`string Inno.Build.Tasks.CompileShaderTask.Configuration`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L36) | Gets or sets the managed and native tool configuration. |
| [`string Inno.Build.Tasks.CompileShaderTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L30) | Gets or sets the checkout whose native offline tools must match this build. |
| [`string Inno.Build.Tasks.CompileShaderTask.OutputFile`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L73) | Gets or sets the compiled file inside an owned Outputs subdirectory. Its parent artifact directory is validated and atomically published with a content manifest. |
| [`string Inno.Build.Tasks.CompileShaderTask.Renderer`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L66) | Gets or sets the graphics API identifier. |
| [`string Inno.Build.Tasks.CompileShaderTask.ShaderPath`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L47) | Gets or sets the source-local shader graph path. |
| [`string Inno.Build.Tasks.CompileShaderTask.Target`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L53) | Gets or sets the graphics compiler target platform. |
| [`string Inno.Build.Tasks.CompileShaderTask.ToolTarget`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L60) | Gets or sets the explicit native target of the offline compiler executed by this task. This selection is independent of the shader artifact target. |
| [`void Inno.Build.Tasks.CompileShaderTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L116) | Cancels offline tool preparation and prevents publication of newly prepared tool products. |

### `Inno.Build.Tasks.GenerateBindingsTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.GenerateBindingsTask`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L22) | Runs the BGCS library pipeline inside MSBuild without invoking a second generator process. |
| [`bool Inno.Build.Tasks.GenerateBindingsTask.CheckOnly`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L53) | Gets or sets whether generation is compared against current sources without replacing them. |
| [`override bool Inno.Build.Tasks.GenerateBindingsTask.Execute()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L105) | Executes facade lowering and managed emission, or reports their structured diagnostics. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.BridgeConfigPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L48) | Gets or sets an optional C++ facade bridge definition evaluated before managed generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ConfigPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L36) | Gets or sets the composed managed binding definition. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L64) | Gets or sets an optional request-owned descriptor destination for command-line toolchain consumers. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L30) | Gets or sets the checkout root used to give source inputs stable logical identities. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L69) | Gets or sets an input identity frozen by the native build; a different current identity fails before generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L74) | Gets the generated managed source after a successful generation or comparison. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L80) | Gets the immutable input identity of the validated generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.NativeBridgeDirectory`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L86) | Gets the complete C bridge root when the component has a facade bridge, otherwise an empty string. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.OutputDirectory`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L42) | Gets or sets the managed output root containing the single binding source. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.TargetOutputRoot`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L59) | Gets or sets the optional target output root whose children are immutable generation fingerprints. An empty value selects the checked-in host output layout. |
| [`void Inno.Build.Tasks.GenerateBindingsTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L92) | Requests cancellation and prevents publication after the active generator has drained. |

### `Inno.Build.Tasks.PrepareProductNativeTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.PrepareProductNativeTask`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L15) | Connects explicit product builds and publication to their target-native closure and exact deployment. |
| [`override bool Inno.Build.Tasks.PrepareProductNativeTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L56) | Prepares all host-native products and installs their exact closure after successful validation. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.Configuration`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L48) | Gets or sets the debug or release configuration used by the managed application. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L24) | Gets or sets the source checkout containing the unified native toolchains. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L30) | Gets or sets the managed application directory receiving the complete native deployment. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.ProductId`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L36) | Gets or sets the exact product identity whose component closure must be prepared. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.TargetId`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L42) | Gets or sets the explicitly declared product target, independent of the task execution machine. |
| [`void Inno.Build.Tasks.PrepareProductNativeTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L100) | Requests process-tree retirement before or during task execution without racing token disposal. |

### `Inno.Build.Tasks.PublishSupportPackTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.PublishSupportPackTask`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L15) | Connects MSBuild publication to the common atomic Support Pack publisher. |
| [`override bool Inno.Build.Tasks.PublishSupportPackTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L56) | Publishes and validates the target pack, reporting failures through MSBuild. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.DotnetHost`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L42) | Gets or sets the SDK host executable. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L24) | Gets or sets the engine checkout directory. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.OutputRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L30) | Gets or sets the directory owning installed target packs. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.Target`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L36) | Gets or sets the requested build target identifier. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.ToolTarget`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L47) | Gets or sets the independently selected native target for executable compilation tools. |
| [`void Inno.Build.Tasks.PublishSupportPackTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L91) | Requests cancellation before or during execution without racing disposal. |

## 项目依赖

- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](Inno.Build.Composition.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- `BGCS`：实现依赖，PrivateAssets="compile"。
- `BGCS.Cpp2C`：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Shaders](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。
