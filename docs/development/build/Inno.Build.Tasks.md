# Inno.Build.Tasks

[Build 索引](README.md) · [Wiki 首页](../../README.md) · [Shader 库](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md) · [Support Pack](Inno.Build.SupportPacks.Core.md)

## 生成绑定的 MSBuild 消费

`GenerateBindingsTask` 负责生成请求、取消和输出映射；[TaskHosting](Inno.Build.TaskHosting.md) 的 `MaterializeBindingsTask` 负责把已选生成源交给当前 SDK 构建。复制流程位于轻量引导程序集，避免 Native 项目为读取自身绑定而递归准备完整 Task runtime。

| 输入/输出 | 语义 |
| --- | --- |
| `Source` | 明确选中的 `Bindings.cs` 绝对路径 |
| `IntermediateDirectory` | 此次 SDK 构建独占的中间目录 |
| `ComponentProject` | Host source 的 Native owner 项目 |
| `ArtifactDirectory` / `Fingerprint` | 目标生成缓存的位置和精确身份；Host source 的缓存位置为空 |
| `MaterializedSource` | 完整复制后的唯一 Compile source |

缓存读取与复制期间持有保护，Task 返回前释放。修复和回收不能替换正在复制的产物。取消、源文件缺失或完整性失败会使 Task 失败；失败路径不会发布半份生成源。Design-time build 不运行这些准备任务。


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
| `GenerateBindingsTask` | sealed、可取消的共享生成库适配；EngineRoot、ComponentId、ComponentProject、BindingDefinition、TargetId、OutputMode、DotnetHost 明确输入，所有配置来自组件唯一 bindings.props。 |
| `GenerateBindingsTask.CheckOnly` | 在独立 staging 重新生成并比较，不提交变更。 |
| `GenerateBindingsTask.DescriptorOutputPath` | 可选的请求独占描述文件，仅在成功后写入。 |
| `GenerateBindingsTask.ExpectedFingerprint` | 可选的冻结身份；当前输入不匹配时在生成前失败，避免托管绑定与已编译原生库来自不同请求。 |
| `GeneratedBindings` / `GenerationFingerprint` / `NativeDirectory` | MSBuild 输出参数；表达本次校验成功的源、身份以及完整原生产物和编译要求，不能由固定 current 目录推断。 |
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

## 所有权与生命周期

唯一 MSBuild 引导位于 `build/msbuild/Inno.Build.Tasks.targets`。[TaskHosting](Inno.Build.TaskHosting.md) 用精确文件集合和 SHA-256 发布一份内容寻址运行闭包；各 evaluation 使用私有加载目录，优先硬链接复用完整依赖。普通 Build 与 Publish 准备对应产品 Native 闭包；Design-time 不执行 Native。`PrepareProductNativeTask` 输出输入扫描、进程与耗时指标。

内部加载目录清理只在全部 owner 进程已确认退出时删除旧缓存。系统拒绝查询进程状态时保留目录，
不能因此使有效构建失败；编译、完整性校验和部署失败仍按正常 Task 错误处理。

同一次 MSBuild 生命周期的 Shader Task 只在首次准备时解析离线 SDK；后续 Task 借用同一已验证的 immutable 工具产物，不重新读取 PATH 或选择 SDK。中立文件路径记录通过 BuildEngine 的 build-lifetime ownership 跨独立 Task load context 复用；每次使用仍验证完整工具输出，构建结束释放该记录。

## 生成 Task 边界

GenerateBindingsTask 只接收 EngineRoot、ComponentId、ComponentProject、BindingDefinition、TargetId、OutputMode、DotnetHost、CheckOnly 和预期指纹/描述输出，映射到共享生成库。旧 ConfigPath/BridgeConfigPath/隐藏扩展入口已删除；配置来自组件唯一 bindings.props。CompileShaderTask、Native Task 与 Support Pack 传递同一中立生成服务。

TargetArtifacts 先通过同一 Standard Distribution 解析请求目标的冻结工具链，再调用生成库。
SDK 路径变量由库从该选择展开，不读取另一套临时进程环境；HostSource 不要求虚构发布工具链。
独立绑定配置可以不注册产品平台：若发行没有同目标 SDK 贡献，配置必须自身完整，缺少 SDK 变量仍明确失败。

## 产品绑定选择的准备顺序

普通产品 Build 在 AssignProjectConfiguration 前准备完整 Native 闭包，在产品 SDK 中间目录生成 NativeBindingSelection.props。非 Analyzer 引用传递同一份选择位置；只有 Native 绑定项目导入并消费预期指纹及实际 Bindings.cs 目录。共同领域项目不把该文件当作自己的编译输入。BGFX 离线工具和运行库共享绑定 owner，汇总时要求完整 generation 一致。

选择文件在 Build/Publish 后保留，由产品 clean 回收。内容未变时保持文件时间戳，变化时原子替换；不能在嵌套 Build 结束时提前删除，也不以 IsPublishing 猜测调用方式。它只记录身份，不持有生成缓存的读取保护；实际编译和部署仍取得各自的 lease。

## Publish 的绑定一致性

PrepareProductNativeTask.BindingSelectionInputPath 指向托管编译实际消费的 selection。Publish 再次准备 Native 后，在安装前通过 NativeBindingGenerationDescriptor.ValidateSelection 严格检查项目闭包、指纹和源码位置；变化则失败，不能部署另一代 Native。保留文件不能代替这一步验证。

产品 Publish 在 PrepareForPublish 前检查 selection 存在；缺失时在 Task runtime 引导前明确失败。
直接使用 --no-build 发布产品必须显式提供其托管编译使用的 selection，不能隐式换成当前 Native generation。
这是当前输入契约，不提供旧输出推导或 fallback。

## Task 宿主 RID

Task 运行图按所选 SDK 的 NETCoreSdkRuntimeIdentifier 恢复依赖。产品的 Browser/AOT/RID 属性在进入 Task 引导前移除；请求身份额外记录工具 RuntimeIdentifier 与 SDK RID。私有 load owner、publisher 读取保护和 Build 生命周期复用保持独立。

## API 参考

[完整声明、行为与调用约束](Inno.Build.Tasks/api.md)

<!-- api-include: Inno.Build.Tasks/api.md -->

## 项目依赖

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](Inno.Build.Composition.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Shaders](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../modules/extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。

Native 产品与离线 Shader 工具完成 SDK 预检后，按已选择计划登记 publisher roots，复用标准发行的唯一缓存 owner 定义。登记不认领未记录的历史目录，也不创建第二份后端闭包。

<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.UsesRuntimeImports f11504af6db8afaf30461a0086da0387a73167f0a8580b46eb0cf6b6344638bb -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-3021459b17db2638"></a>[`public bool Inno.Build.Tasks.GenerateBindingsTask.UsesRuntimeImports { get; private set; }`](../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L85) | Gets whether the selected generated context requires runtime-import initialization. |
