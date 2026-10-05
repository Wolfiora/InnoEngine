# Inno.Build.Managed

[Build 索引](README.md) · [Wiki 首页](../README.md) · [DotNet 发布器](Inno.Build.Managed.DotNet.md) · [Build Pipeline](Inno.Build.md)

## 职责与依赖

本库定义托管部署的中立契约，不引用平台 packager、Editor、Runtime、原生 SDK 或具体 .NET 发布实现。平台提供所需 target，Profile 选择 publisher，组合入口注入 catalog。领域 runtime 不知道使用哪种托管部署。

## 全部公开契约

| API | 当前行为 |
| --- | --- |
| `ManagedDeploymentId(value)` / `value` / `ToString()` | 开放、稳定的 provider identity；内建 coreClr、monoWasm、monoWasmAot、nativeAot 是已提供的 ID，不是封闭枚举。 |
| `ManagedDeploymentCapabilities(runtimeIdentifiers, dynamicCode, aheadOfTime, nativeStaticLinking)` | 防御性复制 target 列表，描述实际执行与原生链接能力。 |
| `ManagedDeploymentRequest(projectPath, runtimeIdentifier, codeInputDirectory, outputDirectory, logDirectory)` | 冻结项目、代码输入和隔离输出；构造时规范绝对路径，不启动工具或创建输出。 |
| `ManagedDeploymentResult(deployment, outputDirectory, sdkIdentity, files)` | 成功产物描述；复制 relative files，拒绝空产物、重复或越界路径。 |
| `IManagedDeploymentCompiler.id / capabilities / CompileAsync(request, cancellationToken)` | 唯一可替换 publisher 边界；错误/取消不得返回成功结果。 |
| `ManagedDeploymentCatalog(compilers)` | 冻结 provider 集合，拒绝空项与重复 identity。 |
| `availableDeployments / GetSupportedDeployments(runtimeIdentifier)` | 以稳定顺序返回不可变的已注册、支持目标的选择。 |
| `Resolve(deployment, runtimeIdentifier)` | 在 staging 前检查 provider 与 target；缺失能力明确失败。 |

没有 protected 扩展点；第三方 publisher 实现接口即可。

## 使用与生命周期

```csharp
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;

var publishers = new ManagedDeploymentCatalog([
    new CoreClrDeploymentCompiler(dotnetHost),
    new MonoWasmDeploymentCompiler(dotnetHost, aheadOfTime: false),
    new MonoWasmDeploymentCompiler(dotnetHost, aheadOfTime: true),
    new NativeAotDeploymentCompiler(dotnetHost)
]);
IManagedDeploymentCompiler compiler = publishers.Resolve(ManagedDeploymentId.monoWasm, "browser-wasm");
ManagedDeploymentResult output = await compiler.CompileAsync(request, cancellationToken);
```

Provider 不安装最终输出。Build Pipeline 独立验证 result identity、staging owner 和完整文件清单，再交平台布局；失败和取消保留上次完整产品。来源代码、generator 与 link template 留在 staging/Support Pack，不进入用户游戏内容。

新增 CoreCLR Wasm 或其他 runtime 时增加一个 provider 并由 composition 注册，复用目录、内容、事件、Scene 和 Player。当前尚未实现的 runtime 不注册、不提供占位 publisher。

## 测试

`ManagedDeploymentTests` 使用开放自定义 provider 验证 catalog 与 immutable boundary；`BuildPipelineTests` 验证真实 pipeline 的发布、失败、取消与原子提交。真实 .NET、Web AOT、NativeAOT 的独立验收结果由本轮验收报告记录。
