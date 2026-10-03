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

没有供派生实现者使用的 protected 扩展点。SDK Task 的基础属性属于 MSBuild。

## 常见工作流

```xml
<UsingTask TaskName="Inno.Build.Tasks.CompileShaderTask" AssemblyFile="/build/Inno.Build.Tasks.dll" />
<CompileShaderTask AssetRoot="/assets" ShaderPath="ImGui.ishader"
                   Platform="WindowsX64" Renderer="Direct3D11" OutputFile="/artifacts/ImGui.bin" />
```

调用前构建 Task 库。平台输出属性从宿主 Task 构建中移除，避免将执行于桌面的编译器构建为 browser-wasm。Task 加载失败即构建失败，不能静默使用陈旧产物。
