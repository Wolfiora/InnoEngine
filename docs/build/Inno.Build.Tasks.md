# Inno.Build.Tasks

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Shader 库](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md) · [Support Pack](Inno.Build.SupportPacks.Core.md)

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

## 本轮边界与所有权

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

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Tasks.CompileShaderTask`

<!-- api: T:Inno.Build.Tasks.CompileShaderTask c39e8f59c784fa4571af87fdf3d4eebf9b3edc15c26d77418b1b18f6140f3f94 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.EngineRoot 2d7a1e65a459c9c0d03810d863d005ee7f74fa6f9356ec6b7a98edb154b8c3b6 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Configuration 1ab66675d82182d5d3a85457a80cef012a4e58866258f324a785639bd03b1fa1 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.AssetRoot 88c9d2fc78392d450dee66e56c9bd6558c1576278586867bfc67afa551fca468 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.ShaderPath d4c5b1d62a8fede93cf80bb17bf77b015deb4da77031518bda5cce26cc27b337 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Target b79bea8a0b61e05c1d0a8d5c44bb022cfc5bc92788175ace1ee9019a55299f78 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.ToolTarget cdbc752af84fb3a27546a5c0750369230f677fa3ae2acf24ddecc12b0720f4e4 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Renderer ad2c8c4658abb86ca7a1c1f33a3339c7ffa03063f69eb7513102bc72dc9b05d2 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.OutputFile e3e2374357adc2c0ba1e98da55f8cf04feff6a33a69718b6fdffc11ffb9e1663 -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.Execute 12cca3ab6d3604126c62a0e36743ef22f1df95ed0e56b484c3fc6bc529fd141b -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.Cancel 5507f05d49118d4161d95555b2dc2af93316fdf5ec8f7ce6018c90a0c15277f7 -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.#ctor e768f0db28186b6552c40b2e640cbe1bba4b70dec0e5b0e22080ccaf60376ada -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-2e282983370f3b9a"></a>[`public Inno.Build.Tasks.CompileShaderTask.CompileShaderTask()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L22) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-3e843d7bc51c1d99"></a>[`public void Inno.Build.Tasks.CompileShaderTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L118) | Cancels offline tool preparation and prevents publication of newly prepared tool products. |
| <a id="api-ca9d1529950eaad3"></a>[`public override bool Inno.Build.Tasks.CompileShaderTask.Execute()`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L83) | Compiles the requested graph artifact and reports compilation failures through MSBuild.<br>返回：True after writing the artifact; false when validation or compilation fails. |
| <a id="api-4cc2a6806019a2cd"></a>[`public string Inno.Build.Tasks.CompileShaderTask.AssetRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L43) | Gets or sets the authoring source directory. |
| <a id="api-21957baa84608a49"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Configuration { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L37) | Gets or sets the managed and native tool configuration. |
| <a id="api-e944b3105e31d7b3"></a>[`public string Inno.Build.Tasks.CompileShaderTask.EngineRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L32) | Gets or sets the checkout whose native offline tools must match this build. |
| <a id="api-cd302b54af465f25"></a>[`public string Inno.Build.Tasks.CompileShaderTask.OutputFile { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L75) | Gets or sets the compiled file inside an owned Outputs subdirectory. Its parent artifact directory is validated and atomically published with a content manifest. |
| <a id="api-c2071a8cc41cac44"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Renderer { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L68) | Gets or sets the graphics API identifier. |
| <a id="api-58d4eb4f091bffbf"></a>[`public string Inno.Build.Tasks.CompileShaderTask.ShaderPath { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L49) | Gets or sets the source-local shader graph path. |
| <a id="api-afd1629572f60a51"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Target { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L55) | Gets or sets the graphics compiler target platform. |
| <a id="api-88d5c566bc444191"></a>[`public string Inno.Build.Tasks.CompileShaderTask.ToolTarget { get; set; }`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L62) | Gets or sets the explicit native target of the offline compiler executed by this task. This selection is independent of the shader artifact target. |
| <a id="api-b386a65d470033c7"></a>[`public sealed class Inno.Build.Tasks.CompileShaderTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L22) | Connects MSBuild to the same graph compiler used by the unified build workflow. |

### `Inno.Build.Tasks.GenerateBindingsTask`

<!-- api: T:Inno.Build.Tasks.GenerateBindingsTask 9888e2e81a05c9847cec0574040e74cd70158c9cf7787ef62a205f347c4b1388 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.EngineRoot bdeffe4ce336627a0cc3a55759f0e1f4fbaba31d5dc31ce8a3e250be349a9a37 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ComponentId f6a30989c1fd387e18b33971339e59d18e51d64b9043d011675c01bd3792c222 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ComponentProject 173f9c3c116a09937603bd65ad2d20951169464e5c8a448fbe386f0272855469 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.BindingDefinition 798cdc61b581c31c89094193b0ec7415eb4e01eb6710655fb16fa8adf97e0350 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.TargetId 50bcfc7b63c3066b66427d3727b69f99e8c9b44e64ae082be73e483f30a537cb -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.OutputMode 2e38e8e75860e01000860f0a181aa0d20f74bfb8ad910f4eeae0c60b5f943f68 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.DotnetHost 674d29f561647b7040fc4d987d221c7cb7cc67f1b9659695b1657e610bfa6578 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.CheckOnly e46aac5cb6e527147d053972ea34d8b7a9e6499be4cd851dd7041275c215e1ba -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath ebe67c4d24f29cb63b2e2386c041d930038193d15e2f6f97e8eb707f16519cf9 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint c45fbc96ef738c6ccbbaa09a63442aa9248320c261ba3ab67f85f5434c68b3db -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings 51e1e88206b45e6308882aafea8531d0b9209b2949698b0fc9469efea32c59a1 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint 3573f4c3015cf9fb7871a0a919fa6d9df75195a5c1f24bf74d892244a99b1924 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.NativeDirectory ef4b387dcc0ac2adb2b0e6f76b18522308e866de74284d032632a6248c261b2f -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ArtifactDirectory c17ec197a5817bc20134d2e2c30c3c063223732aa5408c75913ebb89263d1ac1 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ArtifactOwnerDirectory 315a73a705fbf1aef3cb82068a3c2623897dee4434b83d3932c6c6046ab53b42 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.Cancel 6450d836be0b8cae8c50e013978bec40372ba8b55d248611dba53f9864007117 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.Execute 3d0f76b35164ef0bff7c13131341d9139b45174dcd8fe52e61dd9c83833312a2 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.#ctor 81c4f9dce488cc2cb313f6dd2e6a3f30e04dde68fb7f26f30024b80c4a6e6ccc -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-ff3d6b09600f6748"></a>[`public Inno.Build.Tasks.GenerateBindingsTask.GenerateBindingsTask()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L18) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-0e2f3cc8eb4f48dd"></a>[`public void Inno.Build.Tasks.GenerateBindingsTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L107) | Cancels owned work before retiring the task cancellation source. |
| <a id="api-5781726cd5c70adf"></a>[`public override bool Inno.Build.Tasks.GenerateBindingsTask.Execute()`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L120) | Executes one explicit component request and publishes task outputs only after success.<br>返回：True after generation or comparison; false after a logged failure and complete cleanup. |
| <a id="api-1a480d62e4caeaba"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ArtifactDirectory { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L96) | Gets the protected immutable artifact root, or an empty value for host-source generation. |
| <a id="api-66ed8053f6f55f55"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ArtifactOwnerDirectory { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L102) | Gets the publisher root coordinating the selected immutable artifact, or empty for host sources. |
| <a id="api-575a1911eea02136"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.BindingDefinition { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L43) | Gets or sets the sole literal definition consumed by both the project and direct generation. |
| <a id="api-a6f2d71733c80c5a"></a>[`public bool Inno.Build.Tasks.GenerateBindingsTask.CheckOnly { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L62) | Gets or sets comparison without publication. |
| <a id="api-c71184f5383ef80c"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ComponentId { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L33) | Gets or sets the stable component identity declared by the sole binding definition. |
| <a id="api-982ad2c776d8e749"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ComponentProject { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L38) | Gets or sets the absolute Native owner project, which must belong to this checkout. |
| <a id="api-d0abb47b973cb34d"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L66) | Gets or sets an optional request-owned descriptor destination written only after success. |
| <a id="api-67b85ee3fb7aaee3"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.DotnetHost { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L58) | Gets or sets the explicitly selected managed SDK host used to compose the target toolchain. |
| <a id="api-c0c3fd50a5e4cfea"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.EngineRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L28) | Gets or sets the absolute checkout owning the component definition. |
| <a id="api-9264f16119860a61"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L70) | Gets or sets an optional expected identity; stale requests fail before generation. |
| <a id="api-47ecc7e873a694fd"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L75) | Gets the single source selected after complete publication or successful comparison. |
| <a id="api-aade4e3b794412b5"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L80) | Gets the complete validated input identity. |
| <a id="api-470866008b43e628"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.NativeDirectory { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L90) | Gets the complete native artifacts and compilation requirements selected by this operation. |
| <a id="api-af714cac5c42448a"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.OutputMode { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L53) | Gets or sets HostSource or TargetArtifacts without deriving publication from a path. |
| <a id="api-2e58c35aa575bcc7"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.TargetId { get; set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L48) | Gets or sets the explicit configuration identity selected from the definition. |
| <a id="api-56d9cd602fa62564"></a>[`public sealed class Inno.Build.Tasks.GenerateBindingsTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L18) | Maps an explicit MSBuild request to the shared binding application without owning generation policy. |

### `Inno.Build.Tasks.MaintainBuildCachesTask`

<!-- api: T:Inno.Build.Tasks.MaintainBuildCachesTask 6a56c37da05d5e03cb999d2fda5e5ad93a6a2fee7890cc42354f599bb8942b8a -->
<!-- api: P:Inno.Build.Tasks.MaintainBuildCachesTask.EngineRoot dd402a9e231b189d206f3f86d6330a90bdb230a9d137563750c9bfbf65c8f96d -->
<!-- api: M:Inno.Build.Tasks.MaintainBuildCachesTask.Execute 1afc95e4d782b4058fbfa1af9962a07bc750ae92db202feb12ab1457aa9f54be -->
<!-- api: M:Inno.Build.Tasks.MaintainBuildCachesTask.#ctor dfec429dd6a59b67a3d9cf466b193a88f0e8979850718ff23c5cf5608ec8d8fb -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-5463b4ef34c0f8d3"></a>[`public Inno.Build.Tasks.MaintainBuildCachesTask.MaintainBuildCachesTask()`](../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L12) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-47d1c42c2532768d"></a>[`public override bool Inno.Build.Tasks.MaintainBuildCachesTask.Execute()`](../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L26) | Attempts maintenance without changing the successful product build outcome.<br>返回：True after completed or explicitly deferred automatic maintenance. |
| <a id="api-537a4fc1c2e564e4"></a>[`public string Inno.Build.Tasks.MaintainBuildCachesTask.EngineRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L18) | Gets or sets the absolute checkout owning the registered cache catalog. |
| <a id="api-461c154fdc94dce2"></a>[`public sealed class Inno.Build.Tasks.MaintainBuildCachesTask : Microsoft.Build.Utilities.Task`](../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L12) | Runs bounded cache maintenance after a successful root Editor operation. |

### `Inno.Build.Tasks.PrepareProductNativeTask`

<!-- api: T:Inno.Build.Tasks.PrepareProductNativeTask 55efeac7cb8b6103a78d1e9e245696398b156d9a054bc9030919d2665a3b9b5f -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot a80af1a12910d8c1215a898ba93d22363a51f92a9cb0421732771cdc80334149 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory 17f7f369c896945157f2f1a6e02dd6271473aa38a555c4763da1df0e2b0f92b4 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.ProductId 29fd2b41ad62d4fdb80b86e4b0c9e847c2110c6c6a59b858d46f3d10f0aaa64d -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.TargetId ff6a327ebf667b6491656edbebd2c0a5f8876b2fb3d4ea6476163a6d76f57bbc -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.Configuration 975731c6c2c6c2f7137caed7cd982ce11cc0c863872a25beb4e531eb1a2bc2e4 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionOutputPath d4dd4420898a681b95ec745832ae05c588982a65dedc21d74c5a29826f94ddc9 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionInputPath acda8cb7514bcba45495d508f6c76afa86593abcc33e6cf865206933a8a21686 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.Execute 9c0769b983bdde9b656a1f61ee0ca06dcfc3c42785e00d2721438996c27b2e97 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.Cancel 1836685d1d740271593d145b676374ac6faa86c1ba5296b35ad94fecaa82f820 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.#ctor 3f6cb457d27201c9feacf6a352fdce8fd594d2357f4798bab751017497fe204a -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-7bed92c629f73bb2"></a>[`public Inno.Build.Tasks.PrepareProductNativeTask.PrepareProductNativeTask()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L18) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-0b337b5b1b0c5708"></a>[`public void Inno.Build.Tasks.PrepareProductNativeTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L140) | Requests process-tree retirement before or during task execution without racing token disposal. |
| <a id="api-b8ca781d2842b2bf"></a>[`public override bool Inno.Build.Tasks.PrepareProductNativeTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L70) | Prepares all host-native products and installs their exact closure after successful validation.<br>返回：True after deployment, or false after reporting a build failure or cancellation to MSBuild. |
| <a id="api-be26d4b8d714e351"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionInputPath { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L62) | Gets or sets the managed build's frozen selection that publication must preserve before deployment. An empty value denotes initial preparation without a preceding managed compilation. |
| <a id="api-3c90fc6208778207"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionOutputPath { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L56) | Gets or sets an optional operation-owned selection consumed by the subsequent managed project closure. |
| <a id="api-b480f403e77a86d1"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.Configuration { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L51) | Gets or sets the debug or release configuration used by the managed application. |
| <a id="api-e7cf1daf35a50c42"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L28) | Gets or sets the source checkout containing the unified native toolchains. |
| <a id="api-3d71b74537f79659"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L34) | Gets or sets the managed application directory receiving the complete native deployment. |
| <a id="api-1d019d5a3b1dffca"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.ProductId { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L40) | Gets or sets the exact product identity whose component closure must be prepared. |
| <a id="api-a11fccffcf2147ca"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.TargetId { get; set; }`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L46) | Gets or sets the explicitly declared product target, independent of the task execution machine. |
| <a id="api-90feb81b27588db9"></a>[`public sealed class Inno.Build.Tasks.PrepareProductNativeTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L18) | Connects explicit product builds and publication to their target-native closure and exact deployment. |

### `Inno.Build.Tasks.PublishSupportPackTask`

<!-- api: T:Inno.Build.Tasks.PublishSupportPackTask afb7c65f60b0a8f3964a13d725462ebeb09360afa879863eff93e0fb01e0a4ac -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.EngineRoot 62b4837658cdd6d0c46675dac36f90a40df50adb812e435929ad029ea2c1e453 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.OutputRoot 055794987c03c7d294d5c8fa755b269197002815f8a087194cf8a31ecb3f226f -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.Target 13c85b1db754fe83bf44a772e4848640a8ea59fd8d4dfea9d38303ba01a5fe66 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.DotnetHost 6d3632deaede563157464c9b62227f7a6376a40cc24be084209760a499194614 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.ToolTarget 773c3baf90f3a29102b99ce14aa6e31c32e5c4f8711d6e0f21dd2f6c2a874219 -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.Execute d560b8eb68d4d2c71db2a345c8a6453c1c2fc159d3fa0ac59247c2602d07c02a -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.Cancel 4ed313b958d440c7b332188b188958a8d2c3802dc6cf47e183009d370c5e063b -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.#ctor 4e1d97a6c9b454101630af2574fbb21bf337ba058d75e6dcf10be889c53aa83c -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-eb6353f8d53da889"></a>[`public Inno.Build.Tasks.PublishSupportPackTask.PublishSupportPackTask()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L17) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-c2cc3ab11edf1284"></a>[`public void Inno.Build.Tasks.PublishSupportPackTask.Cancel()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L95) | Requests cancellation before or during execution without racing disposal. |
| <a id="api-340855085dd8722b"></a>[`public override bool Inno.Build.Tasks.PublishSupportPackTask.Execute()`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L58) | Publishes and validates the target pack, reporting failures through MSBuild.<br>返回：True after installation; false when publication fails or is canceled. |
| <a id="api-cd3d0b94e175c13c"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.DotnetHost { get; set; }`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L44) | Gets or sets the SDK host executable. |
| <a id="api-40bae46b53808cd2"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.EngineRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L27) | Gets or sets the engine checkout directory. |
| <a id="api-0574e0cdd7b940fd"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.OutputRoot { get; set; }`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L33) | Gets or sets the directory owning installed target packs. |
| <a id="api-7c02113ae29f3bb8"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.Target { get; set; }`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L39) | Gets or sets the requested build target identifier. |
| <a id="api-8ee71db1117460e4"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.ToolTarget { get; set; }`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L50) | Gets or sets the independently selected native target for executable compilation tools. |
| <a id="api-8c2e8f3c7f71bbe4"></a>[`public sealed class Inno.Build.Tasks.PublishSupportPackTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L17) | Connects MSBuild publication to the common atomic Support Pack publisher. |

## 项目依赖

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](Inno.Build.Composition.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Shaders](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Build.Distribution.Standard](Inno.Build.Distribution.Standard.md)：实现依赖，PrivateAssets="compile"。

Native 产品与离线 Shader 工具完成 SDK 预检后，按已选择计划登记 publisher roots，复用标准发行的唯一缓存 owner 定义。登记不认领未记录的历史目录，也不创建第二份后端闭包。

<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.UsesRuntimeImports f11504af6db8afaf30461a0086da0387a73167f0a8580b46eb0cf6b6344638bb -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-3021459b17db2638"></a>[`public bool Inno.Build.Tasks.GenerateBindingsTask.UsesRuntimeImports { get; private set; }`](../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L85) | Gets whether the selected generated context requires runtime-import initialization. |
