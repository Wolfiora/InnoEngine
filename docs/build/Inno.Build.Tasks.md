# Inno.Build.Tasks

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Shader 库](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md) · [Support Pack](Inno.Build.SupportPacks.Core.md)

## 职责与依赖

ImGui Shader 的平台/API 集合由平台 Editor 产品显式注入，不由后端列出平台或根据宿主推导。`CompileShaderTask` 按实际离线工具身份、源码完整内容、compiler runtime、平台/API/配置/资源名生成指纹；每个 API 的 `Outputs` 与 manifest 由写 lease 和原子目录事务发布。热构建完整验证后复用，输入同长度同 mtime 修改或输出损坏也会重建；编译过程中输入变化时拒绝候选。共同 Native 哈希、完整性和 Core.IO 机制被直接复用。

MSBuild 的薄适配层，调用与 CLI 相同的库。引用固定版本的 Microsoft.Build 编译契约包，执行时由宿主 SDK 提供实现程序集；不引入另一套命令解析或发布流程。

## 全部公开 API

| 类型/成员 | 语义 |
| --- | --- |
| `CompileShaderTask` | sealed MSBuild Task；EngineRoot、AssetRoot、ShaderPath、Target、ToolTarget、Renderer、OutputFile 为必填输入。 |
| `CompileShaderTask.Execute()` | 编译图产物，成功 true，异常作为 MSBuild 错误并返回 false。 |
| `PublishSupportPackTask` | sealed、可取消发布 Task；EngineRoot、OutputRoot、Target、ToolTarget 必填，DotnetHost 选择 SDK。 |
| `Execute()` / `Cancel()` | 调用核心 publisher；失败返回 false；取消可在执行前或执行中请求，锁保护取消与 disposal。 |
| `GenerateBindingsTask` | sealed、可取消的共享生成库适配；EngineRoot、ComponentProject、BindingDefinition、TargetId、OutputMode、DotnetHost 明确输入，所有配置来自组件唯一 bindings.props。 |
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
                   Target="windows-x64" Renderer="Direct3D11" OutputFile="/artifacts/ImGui/Outputs/ImGui.bin" />
```

调用前构建 Task 库。平台输出属性从宿主 Task 构建中移除，避免将执行于桌面的编译器构建为 browser-wasm。Task 加载失败即构建失败，不能静默使用陈旧产物。
Task 引导、不可变运行闭包及私有加载目录写入 `artifacts/build-tools/tasks`；扩展工具有独立宿主输出，均与产品产物隔离。
组件引用的 BGCS.Runtime 使用标准 SDK `ArtifactsPath` 选择自己的目标输出；独立 BGCS 仓库不承担引擎路径规则。

## 产品原生部署 Task

`PrepareProductNativeTask` 接收明确的 `ProductId`、`TargetId`、`EngineRoot`、`OutputDirectory` 与 `Configuration`；执行 SDK 来自当前 MSBuild 宿主环境。离线 Shader Task 的 `ToolTarget` 与游戏 Shader 目标分别声明。
`Execute()` 通过同一 Standard Distribution 获取产品闭包和目标工具链，调用 `ProductNativeBuildPlan` 与 `ProductNativeDeployment`；成功返回 true，错误记入 MSBuild 并返回 false。
`Cancel()` 在执行前或执行中取消拥有的进程树和 staging；不扫描 `.lib` 或固定配置 glob。
普通 Editor Build 在 `BeforeTargets="AssignProjectConfiguration"` 准备并安装到 MSBuild 的 `TargetDir`；Publish 在最终 `PublishDir` 安装同一契约的产品集合，框架 reference pack 使用 MSBuild 实际解析的 targeting pack。设计期构建不执行原生准备。
Task 不成为产品的 managed runtime 引用；产品 RID、AOT、Native profile 和 IDE 全局属性从工具引导中移除。CLI `engine` 使用普通产品 Build 入口部署，不维护另一处固定 bin 路径。
Task 的取消、编译、产物校验和部署失败都使 MSBuild 失败；内容一致的原生树保留原文件。

## 本轮边界与所有权

唯一 MSBuild 引导位于 `build/msbuild/Inno.Build.Tasks.targets`。[TaskHosting](Inno.Build.TaskHosting.md) 用精确文件集合和 SHA-256 发布一份内容寻址运行闭包；各 evaluation 使用私有加载目录，优先硬链接复用完整依赖。普通 Build 与 Publish 准备对应产品 Native 闭包；Design-time 不执行 Native。`PrepareProductNativeTask` 输出输入扫描、进程与耗时指标。

内部加载目录清理只在全部 owner 进程已确认退出时删除旧缓存。系统拒绝查询进程状态时保留目录，
不能因此使有效构建失败；编译、完整性校验和部署失败仍按正常 Task 错误处理。

同一次 MSBuild 生命周期的 Shader Task 只在首次准备时解析离线 SDK；后续 Task 借用同一已验证的 immutable 工具产物，不重新读取 PATH 或选择 SDK。中立文件路径记录通过 BuildEngine 的 build-lifetime ownership 跨独立 Task load context 复用；每次使用仍验证完整工具输出，构建结束释放该记录。

## 生成 Task 边界

GenerateBindingsTask 只接收 EngineRoot、ComponentProject、BindingDefinition、TargetId、OutputMode、DotnetHost、CheckOnly 和预期指纹/描述输出，映射到共享生成库。旧 ConfigPath/BridgeConfigPath/隐藏扩展入口已删除；配置来自组件唯一 bindings.props。CompileShaderTask、Native Task 与 Support Pack 传递同一中立生成服务。

TargetArtifacts 先通过同一 Standard Distribution 解析请求目标的冻结工具链，再调用生成库。
SDK 路径变量由库从该选择展开，不读取另一套临时进程环境；HostSource 不要求虚构发布工具链。
独立绑定配置可以不注册产品平台：若发行没有同目标 SDK 贡献，配置必须自身完整，缺少 SDK 变量仍明确失败。

## 产品绑定选择的准备顺序

普通产品 Build 在 AssignProjectConfiguration 前准备完整 Native 闭包，在私有 Task reader 目录生成 operation-owned NativeBindingSelection.props。所有非 Analyzer 项目引用消费同一份预期指纹及实际 Bindings.cs 目录，不再独立重复生成。BGFX 离线工具和运行库共享绑定 owner，汇总时要求它们的完整 generation 一致。产物提交与托管编译使用同一选择。

Build 可能是 Publish 的嵌套依赖，因此不能在 AfterTargets="Build" 删除选择文件，也不以 IsPublishing 猜测调用方式。Publish 完成最后的 Native 校验后删除选择；普通 Build 的选择由已有私有 reader/process ownership 退休流程回收。失败或取消时保留仍可能被 consumer 借用的文件，下一次 reader 退休确认后清理。

## Publish 的绑定一致性

PrepareProductNativeTask.BindingSelectionInputPath 指向托管编译实际消费的 operation selection。Publish 再次准备 Native 后，在安装前通过 NativeBindingGenerationDescriptor.ValidateSelection 严格检查项目闭包、指纹和源码位置；变化则失败，不能部署另一代 Native。选择文档在 Native Publish 成功后才清理。

产品 Publish 在 PrepareForPublish 前检查 selection 存在；缺失时在 Task runtime 引导前明确失败。
直接使用 --no-build 发布产品必须显式提供其托管编译使用的 selection，不能隐式换成当前 Native generation。
这是当前输入契约，不提供旧输出推导或 fallback。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

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
| [`Inno.Build.Tasks.GenerateBindingsTask`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L17) | Maps an explicit MSBuild request to the shared binding application without owning generation policy. |
| [`bool Inno.Build.Tasks.GenerateBindingsTask.CheckOnly`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L56) | Gets or sets comparison without publication. |
| [`override bool Inno.Build.Tasks.GenerateBindingsTask.Execute()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L97) | Executes one explicit component request and publishes task outputs only after success. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.BindingDefinition`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L36) | Gets or sets the sole literal definition consumed by both the project and direct generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ComponentProject`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L31) | Gets or sets the absolute Native owner project, which must belong to this checkout. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L60) | Gets or sets an optional request-owned descriptor destination written only after success. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.DotnetHost`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L51) | Gets or sets the explicitly selected SDK host for declared generator extensions. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L26) | Gets or sets the absolute checkout owning the component definition. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L64) | Gets or sets an optional expected identity; stale requests fail before generation. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L68) | Gets the single source selected after complete publication or successful comparison. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L73) | Gets the complete validated input identity. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.NativeBridgeDirectory`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L78) | Gets the complete C bridge directory, or an empty value for a direct C API. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.OutputMode`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L46) | Gets or sets HostSource or TargetArtifacts without deriving publication from a path. |
| [`string Inno.Build.Tasks.GenerateBindingsTask.TargetId`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L41) | Gets or sets the explicit configuration identity selected from the definition. |
| [`void Inno.Build.Tasks.GenerateBindingsTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L84) | Cancels owned work before retiring the task cancellation source. |

### `Inno.Build.Tasks.PrepareProductNativeTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.PrepareProductNativeTask`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L17) | Connects explicit product builds and publication to their target-native closure and exact deployment. |
| [`override bool Inno.Build.Tasks.PrepareProductNativeTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L69) | Prepares all host-native products and installs their exact closure after successful validation. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionInputPath`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L61) | Gets or sets the managed build's frozen selection that publication must preserve before deployment. An empty value denotes initial preparation without a preceding managed compilation. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionOutputPath`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L55) | Gets or sets an optional operation-owned selection consumed by the subsequent managed project closure. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.Configuration`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L50) | Gets or sets the debug or release configuration used by the managed application. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L26) | Gets or sets the source checkout containing the unified native toolchains. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L32) | Gets or sets the managed application directory receiving the complete native deployment. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.ProductId`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L38) | Gets or sets the exact product identity whose component closure must be prepared. |
| [`string Inno.Build.Tasks.PrepareProductNativeTask.TargetId`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L44) | Gets or sets the explicitly declared product target, independent of the task execution machine. |
| [`void Inno.Build.Tasks.PrepareProductNativeTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L137) | Requests process-tree retirement before or during task execution without racing token disposal. |

### `Inno.Build.Tasks.PublishSupportPackTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Tasks.PublishSupportPackTask`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L16) | Connects MSBuild publication to the common atomic Support Pack publisher. |
| [`override bool Inno.Build.Tasks.PublishSupportPackTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L57) | Publishes and validates the target pack, reporting failures through MSBuild. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.DotnetHost`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L43) | Gets or sets the SDK host executable. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.EngineRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L25) | Gets or sets the engine checkout directory. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.OutputRoot`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L31) | Gets or sets the directory owning installed target packs. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.Target`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L37) | Gets or sets the requested build target identifier. |
| [`string Inno.Build.Tasks.PublishSupportPackTask.ToolTarget`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L48) | Gets or sets the independently selected native target for executable compilation tools. |
| [`void Inno.Build.Tasks.PublishSupportPackTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L93) | Requests cancellation before or during execution without racing disposal. |

## 项目依赖

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](Inno.Build.Composition.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Shaders](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。
