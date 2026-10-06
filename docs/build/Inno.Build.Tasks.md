# Inno.Build.Tasks

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Shader 库](Inno.Build.Toolchains.Bgfx.Shaders.md) · [Support Pack](Inno.Build.SupportPacks.md)

## 职责与依赖

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
<CompileShaderTask AssetRoot="/assets" ShaderPath="ImGui.ishader"
                   Platform="WindowsX64" Renderer="Direct3D11" OutputFile="/artifacts/ImGui.bin" />
```

调用前构建 Task 库。平台输出属性从宿主 Task 构建中移除，避免将执行于桌面的编译器构建为 browser-wasm。Task 加载失败即构建失败，不能静默使用陈旧产物。
Task 与扩展的宿主依赖图写入 `artifacts/build-tools/bindings`，独立于各目标的发布与符号文件。
组件引用的 BGCS.Runtime 使用标准 SDK `ArtifactsPath` 选择自己的目标输出；独立 BGCS 仓库不承担引擎路径规则。

## Editor 原生部署 Task

`PrepareEditorNativeTask` 的 `EngineRoot`、`OutputDirectory` 为必填输入，`Configuration` 指定 debug/release。
`Execute()` 调用同一 HostNativeBuild 与 HostNativeDeployment，成功返回 true；错误记入 MSBuild 并返回 false。
`Cancel()` 在执行前或执行中取消拥有的进程树和 staging；不扫描 `.lib` 或固定配置 glob。
普通 Editor Build 在 `AfterTargets="Build"` 安装到 MSBuild 的 `TargetDir`；Publish 在最终 `PublishDir` 安装同一契约的产品集合，框架 reference pack 使用 MSBuild 实际解析的 targeting pack。设计期构建不执行原生准备。
Editor 的 Task 与依赖使用 `artifacts/build-tools/editor` 的宿主输出，不成为 Editor 的 managed runtime 引用；目标发布属性从 Task 构建移除。CLI `engine` 通过这个普通 Build 入口部署，不再维护另一处固定的 Editor bin 路径。
Task 的取消、编译、产物校验和部署失败都使 MSBuild 失败；内容一致的原生树保留原文件。






## 本轮边界与所有权

唯一 MSBuild 引导位于 build/msbuild/Inno.Build.Tasks.targets。host 工具隔离目标 RID、AOT、Wasm 与 IDE 全局属性，显式构建依赖。一次闭包中同一配置/工具身份只准备一次 immutable host，再为各 evaluation 提供私有加载目录。普通 Build 与 Publish 都准备完整 Editor native 闭包；Design-time 不执行 native 工作。PrepareEditorNativeTask 输出输入扫描、进程与耗时指标。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Tasks.CompileShaderTask`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.Tasks.CompileShaderTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L107) | Cancels offline tool preparation and prevents publication of newly prepared tool products. |
| [`override bool Inno.Build.Tasks.CompileShaderTask.Execute()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L73) | Compiles the requested graph artifact and reports compilation failures through MSBuild. |
| [`string Inno.Build.Tasks.CompileShaderTask.AssetRoot`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L40) | Gets or sets the authoring source directory. |
| [`string Inno.Build.Tasks.CompileShaderTask.Configuration`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L35) | Gets or sets the managed and native tool configuration. |
| [`string Inno.Build.Tasks.CompileShaderTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L29) | Gets or sets the checkout whose native offline tools must match this build. |
| [`string Inno.Build.Tasks.CompileShaderTask.OutputFile`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L64) | Gets or sets the compiled artifact destination. |
| [`string Inno.Build.Tasks.CompileShaderTask.Platform`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L52) | Gets or sets the graphics compiler target platform. |
| [`string Inno.Build.Tasks.CompileShaderTask.Renderer`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L58) | Gets or sets the graphics API identifier. |
| [`string Inno.Build.Tasks.CompileShaderTask.ShaderPath`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L46) | Gets or sets the source-local shader graph path. |
| [`Inno.Build.Tasks.CompileShaderTask`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L20) | Connects MSBuild to the same graph compiler used by the unified build workflow. |

### `Inno.Build.Tasks.GenerateBindingsTask`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.Tasks.GenerateBindingsTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L92) | Requests cancellation and prevents publication after the active generator has drained. |
| [`override bool Inno.Build.Tasks.GenerateBindingsTask.Execute()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L105) | Executes facade lowering and managed emission, or reports their structured diagnostics. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.BridgeConfigPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L48) | Gets or sets an optional C++ facade bridge definition evaluated before managed generation. |
| [`bool Inno.Build.Tasks.GenerateBindingsTask.CheckOnly`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L53) | Gets or sets whether generation is compared against current sources without replacing them. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ConfigPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L36) | Gets or sets the composed managed binding definition. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L64) | Gets or sets an optional request-owned descriptor destination for command-line toolchain consumers. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L30) | Gets or sets the checkout root used to give source inputs stable logical identities. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L69) | Gets or sets an input identity frozen by the native build; a different current identity fails before generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L74) | Gets the generated managed source after a successful generation or comparison. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L80) | Gets the immutable input identity of the validated generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.NativeBridgeDirectory`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L86) | Gets the complete C bridge root when the component has a facade bridge, otherwise an empty string. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.OutputDirectory`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L42) | Gets or sets the managed output root containing the single binding source. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.TargetOutputRoot`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L59) | Gets or sets the optional target output root whose children are immutable generation fingerprints. An empty value selects the checked-in host output layout. |
| [`Inno.Build.Tasks.GenerateBindingsTask`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L22) | Runs the BGCS library pipeline inside MSBuild without invoking a second generator process. |

### `Inno.Build.Tasks.PrepareEditorNativeTask`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.Tasks.PrepareEditorNativeTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L84) | Requests process-tree retirement before or during task execution without racing token disposal. |
| [`override bool Inno.Build.Tasks.PrepareEditorNativeTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L43) | Prepares all host-native products and installs their exact closure after successful validation. |
| [`string Inno.Build.Tasks.PrepareEditorNativeTask.Configuration`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L35) | Gets or sets the debug or release configuration used by the managed application. |
| [`string Inno.Build.Tasks.PrepareEditorNativeTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L23) | Gets or sets the source checkout containing the unified native toolchains. |
| [`string Inno.Build.Tasks.PrepareEditorNativeTask.OutputDirectory`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L29) | Gets or sets the managed application directory receiving the complete native deployment. |
| [`Inno.Build.Tasks.PrepareEditorNativeTask`](../../build/tasks/Inno.Build.Tasks/PrepareEditorNativeTask.cs#L14) | Connects Editor builds and publication to host-native preparation and exact product deployment. |

### `Inno.Build.Tasks.PublishSupportPackTask`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.Tasks.PublishSupportPackTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L82) | Requests cancellation before or during execution without racing disposal. |
| [`override bool Inno.Build.Tasks.PublishSupportPackTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L48) | Publishes and validates the target pack, reporting failures through MSBuild. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.DotnetHost`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L40) | Gets or sets the SDK host executable. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L22) | Gets or sets the engine checkout directory. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.OutputRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L28) | Gets or sets the directory owning installed target packs. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.Target`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L34) | Gets or sets the requested build target identifier. |
| [`Inno.Build.Tasks.PublishSupportPackTask`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L13) | Connects MSBuild publication to the common atomic Support Pack publisher. |

## 项目依赖

- [Inno.Build.Toolchains.Bgfx](Inno.Build.Toolchains.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Composition](Inno.Build.Composition.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Host](Inno.Build.Toolchains.Host.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖（`PrivateAssets="compile"`）。
- `BGCS`：实现依赖（`PrivateAssets="compile"`）。
- `BGCS.Cpp2C`：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Bgfx.Shaders](Inno.Build.Toolchains.Bgfx.Shaders.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
