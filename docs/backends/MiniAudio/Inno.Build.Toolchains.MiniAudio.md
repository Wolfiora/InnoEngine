# Inno.Build.Toolchains.MiniAudio

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.MiniAudio.md)

## 职责、依赖和公开 API

负责该共享 backend 的独立动态库源码声明和构建，接受已经冻结的明确目标、SDK 与 Shared 组件选项。平台 SDK 由对应 provider 选择，本库通过共同 Toolchains 执行并发布完整产物。它不根据当前宿主重新选择输出目标，也没有生产 Program。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.MiniAudio;

static Task<NativeBuildProduct> PrepareComponentAsync(
    NativeBuildContext preparedContext,
    CancellationToken cancellationToken
) {
    return MiniAudioToolchain.BuildAsync(preparedContext, cancellationToken);
}
```

`CreateRecipe(context)` 在编译与生成绑定之前返回同一份源码、构建实现、SDK 和有序选项声明。它借用 context，不创建 staging、启动工具或读取生成输出；内容指纹计算仍会严格读取声明的真实输入。`BuildAsync` 消费这份声明，发布器在执行前加入已选 generation 的实际内容。

上述构建调用用于已有构建操作：`preparedContext` 必须包含冻结的工具链、该组件的构建选项及所需绑定 generation。调用方在整个编译期间保留这些输入的读取保护，并在部署或链接完成后 Dispose 返回的 `NativeBuildProduct`。正常产品构建由发行组合的 `ProductNativeBuildPlan` 统一准备这些依赖。


## 常见流程与生命周期

日常 Editor 构建使用 `InnoEngine.Build editor`；Support Pack 由独立 `support-pack --target <target>` 请求准备。两者选择各自的产品闭包。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 源码归属

当前唯一源码 owner：`backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio/Inno.Build.Toolchains.MiniAudio.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain`

<!-- api: T:Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain 3d05a0c07cc7ded385b994113af187652542844349846e99148eb9ed11e843dc -->
<!-- api: P:Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.componentDescriptor 49dcc467a03ccfff4c10df866d425a7e802c46aa133c5871f7071719d9a50a59 -->
<!-- api: M:Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,System.Threading.CancellationToken) 5e103d03b0555eeb0e2418f33e0e8ce01551362a2e87af6296538e740192c102 -->
<!-- api: M:Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext) 678e5bdc9ce2c3ca67222ccbbd0fff8b22299c724ad4825a28e16370183993d1 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-df211cc1b1abbeea"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio/MiniAudioToolchain.cs#L49) | Builds the component using the explicitly selected target SDK and validates the complete native product.<br>`context`：The operation-owned checkout, configuration, tools and target.<br>`cancellationToken`：Cancels and drains active tool processes before publication.<br>返回：The immutable validated product; a failed or canceled candidate is not published.<br>T:System.InvalidOperationException：The SDK is unassigned or native compilation or export validation fails.<br>T:System.OperationCanceledException：The operation was canceled before publication. |
| <a id="api-84e5ab74f8fa7f27"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio/MiniAudioToolchain.cs#L100) | Declares the standalone source, SDK and ordered options without compiling or generating bindings.<br>`context`：The operation with an explicit target toolchain and this component's shared-library options.<br>返回：A frozen declaration also consumed by BuildAsync; selected binding outputs are added by publication.<br>T:System.ArgumentNullException：The context is null.<br>T:System.InvalidOperationException：Target tools or component options have not been selected.<br>T:System.NotSupportedException：The request uses static rather than standalone shared linkage.<br>T:System.IO.FileNotFoundException：The component's declared build project is absent from the checkout. |
| <a id="api-46489018ec014f41"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain.componentDescriptor { get; }`](../../../backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio/MiniAudioToolchain.cs#L21) | Gets the component's sole source, binding and intermediate owner. |
| <a id="api-b7087cc3a2b9127b"></a>[`public static class Inno.Build.Toolchains.MiniAudio.MiniAudioToolchain`](../../../backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio/MiniAudioToolchain.cs#L13) | Owns this backend's native recipe while SDK selection remains with the platform provider. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
