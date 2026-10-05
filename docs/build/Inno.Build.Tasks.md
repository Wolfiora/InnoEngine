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
发布 Editor 时在最终 PublishDir 中安装确定的产品集合，框架 reference pack 也使用 MSBuild 实际解析的 targeting pack。
