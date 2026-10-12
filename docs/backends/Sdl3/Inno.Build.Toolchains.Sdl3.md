# Inno.Build.Toolchains.Sdl3

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.Sdl3.md)

## 职责、依赖和公开 API

负责该共享 backend 的独立动态库源码声明和构建，接受已经冻结的明确目标、SDK 与 Shared 组件选项。平台 SDK 由对应 provider 选择，本库通过共同 Toolchains 执行并发布完整产物。它不根据当前宿主重新选择输出目标，也没有生产 Program。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Sdl3;

static Task<NativeBuildProduct> PrepareComponentAsync(
    NativeBuildContext preparedContext,
    CancellationToken cancellationToken
) {
    return Sdl3Toolchain.BuildAsync(preparedContext, cancellationToken);
}
```

`CreateRecipe(context)` 在编译与生成绑定之前返回同一份源码、构建实现、SDK 和有序选项声明。它借用 context，不创建 staging、启动工具或读取生成输出；内容指纹计算仍会严格读取声明的真实输入。`BuildAsync` 消费这份声明，发布器在执行前加入已选 generation 的实际内容。

上述构建调用用于已有构建操作：`preparedContext` 必须包含冻结的工具链、该组件的构建选项及所需绑定 generation。调用方在整个编译期间保留这些输入的读取保护，并在部署或链接完成后 Dispose 返回的 `NativeBuildProduct`。正常产品构建由发行组合的 `ProductNativeBuildPlan` 统一准备这些依赖。


## 常见流程与生命周期

日常 Editor 构建使用 `InnoEngine.Build editor`；Support Pack 由独立 `support-pack --target <target>` 请求准备。两者选择各自的产品闭包。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 源码归属

当前唯一源码 owner：`backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Inno.Build.Toolchains.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Toolchains.Sdl3.Sdl3Toolchain`

<!-- api: T:Inno.Build.Toolchains.Sdl3.Sdl3Toolchain dc5c7844b44fa8e24ca685f0a9ae4525dd3f9e59e6d271de65d75f58a3ecf388 -->
<!-- api: P:Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.componentDescriptor e1d4d1ab6d4b930a3e6f1ad43718fcadb671a6bc3bdf3be061c5afa216489a5d -->
<!-- api: M:Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,System.Threading.CancellationToken) 01aa72d1bc1830b7b7b9cd1b6efa6430dc39f0dfa484ab9913e76187ea2f209f -->

<!-- api: M:Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext) 54a08608f2fb8eb0d9784fcb91ef74bb6d19f3bd2b92ce880e40b35e11c70f19 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1ee46b8d3da362fd"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Sdl3Toolchain.cs#L48) | Builds the component using the explicitly selected target SDK and validates the complete native product.<br>返回：The immutable validated product; a failed or canceled candidate is not published.<br>`context`：The operation-owned checkout, configuration, tools and target.<br>`cancellationToken`：Cancels and drains active tool processes before publication. | — | `System.InvalidOperationException`：The SDK is unassigned or native compilation or export validation fails.<br>`System.OperationCanceledException`：The operation was canceled before publication. |
| <a id="api-9d10bf0884513234"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.componentDescriptor { get; }`](../../../backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Sdl3Toolchain.cs#L19) | Gets the component's sole source, binding and intermediate owner. | — | — |
| <a id="api-2f07ce9bbbdd0d4e"></a>[`public static class Inno.Build.Toolchains.Sdl3.Sdl3Toolchain`](../../../backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Sdl3Toolchain.cs#L12) | Owns this backend's native recipe while SDK selection remains with the platform provider. | — | — |
| <a id="api-f2adda145d725a4d"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.Sdl3.Sdl3Toolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Sdl3Toolchain.cs#L92) | 冻结独立 Shared recipe 的源码、实现、SDK 和有序参数；不要求绑定输出已存在。 | 借用 context；返回不可变声明，不取得源文件、SDK 或 generation 的 owner。 | context/SDK/组件选项缺失、Static 请求或 build owner 项目不存在时失败；声明中的其余文件在实际内容读取时验证。 |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
