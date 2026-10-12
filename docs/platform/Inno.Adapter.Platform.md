# Inno.Adapter.Platform

[Platform 索引](README.md) · [中立 Platform](Inno.Platform.md) · [SDL3 implementation](../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)

该项目定义平台 Adapter family 的稳定边界，不包含 SDL3 引用。

## 公开 API

- `PlatformBackendId`：Composition 启动时使用的后端选择；默认注册 ID 为 `sdl3`。
- `IPlatformBackendFactory.CreateApplication`：把中立选择解析为 caller-owned `IPlatformApplication`。

Host 只保存稳定 ID、factory 和 `IPlatformApplication` / `IPlatformWindow`。未知或未安装的 backend 必须明确失败；不得返回伪实现。原生 surface 只在相互配合的具体 Adapter 之间传递，不进入服务、Shell、Player 或脚本契约。

```csharp
using System;
using Inno.Adapter.Platform;
using Inno.Platform;

static void RunWithWindow(
    IPlatformBackendFactory factory,
    PlatformBackendId selection,
    PlatformWindowOptions options,
    Action<IPlatformApplication, IPlatformWindow> run
) {
    using IPlatformApplication application = factory.CreateApplication(selection);
    using IPlatformWindow window = application.CreateWindow(options);
    run(application, window);
}
```

相邻职责：窗口/事件语义见 [Inno.Platform](Inno.Platform.md)，SDL3 映射见 [Inno.Adapter.Platform.Sdl3](../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `sdl3` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `PlatformBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `PlatformBackendProvider(PlatformBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `PlatformBackendProvider.CreateApplication` | 实现者的创建扩展点；返回 caller-owned `IPlatformApplication`，不允许 null。 |
| `PlatformBackendCatalog(IEnumerable<PlatformBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `PlatformBackendCatalog.supportedBackends / CreateApplication` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IPlatformBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `PlatformBackendId`，无需新增枚举或修改中央分支。

## 原生呈现 SPI

| API | 当前语义 |
| --- | --- |
| `INativeWindowSurface.nativeHandles` | 窗口可选实现的 Adapter SPI，返回窗口 owner 持有的借用原生 surface。 |
| `PlatformNativeHandles(windowHandle, displayHandle, handleKind)` | 只读描述；默认 display handle 为零、ABI ID 未赋值。`windowHandle` / `displayHandle` 不转移所有权，窗口释放后失效。 |
| `PlatformNativeHandleId(string)`；`value`、`isValid`、`ToString()` | 开放、区分大小写的稳定 surface ABI ID；空白构造失败，默认值未赋值。 |
| `PlatformNativeHandleId.win32 / cocoa / browserCanvas` | 内置 HWND、Cocoa 窗口及 UTF-8 canvas selector ABI。第三方可以声明自己的 ID，不修改本项目。 |

具体图形 Adapter 验证它理解的 ID 与句柄；不支持必须明确失败。窗口释放后再次借用句柄抛出 `ObjectDisposedException`。BGFX 在接管原生进程资源之前拒绝未实现此 SPI 的窗口。新增平台 surface 的映射留在具体窗口和图形 Adapter，不能扩展为服务层原生访问 API。这里的 SPI 不进入 Scripting API；不参与对象 Identity、History 或序列化。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Platform.INativeWindowSurface`

<!-- api: T:Inno.Adapter.Platform.INativeWindowSurface 50f19c2ee8deff9fe051a180aa07c7b6e82287b44587c3728dc7dae9a0243da4 -->
<!-- api: P:Inno.Adapter.Platform.INativeWindowSurface.nativeHandles 83585ddb8c57bb7443b2f838c775a0a379a952e0c5c28c430d5319d6225e25de -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2ca88f33ca867c43"></a>[`Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.INativeWindowSurface.nativeHandles { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/INativeWindowSurface.cs#L20) | Gets handles owned by the live window; consumers must neither free nor retain them beyond its lifetime. | — | `System.ObjectDisposedException`：The owning window has been released. |
| <a id="api-2c6ba4b4de7adc1f"></a>[`public interface Inno.Adapter.Platform.INativeWindowSurface`](../../src/adapters/platform/Inno.Adapter.Platform/INativeWindowSurface.cs#L12) | Supplies borrowed native presentation handles to cooperating adapters. | Window implementations opt into this SPI when they support native rendering integration. Shared platform, Shell, and Player contracts do not expose these ABI details. | — |

### `Inno.Adapter.Platform.IPlatformBackendFactory`

<!-- api: T:Inno.Adapter.Platform.IPlatformBackendFactory eb81e904ae372277a0f0806e7bfd0a32e3a2a7ba8c8635f8bcd2338ddafe86df -->
<!-- api: P:Inno.Adapter.Platform.IPlatformBackendFactory.supportedBackends 662bdca89ddf9cdbda4cf9c4827b9b0aaf3603b8465c2fc09e46741216ce2f5b -->
<!-- api: M:Inno.Adapter.Platform.IPlatformBackendFactory.CreateApplication(Inno.Adapter.Platform.PlatformBackendId) 036adda1d672d74107b04e2fa2e2c0524f5a678ff1be70865eae9971752a3f7e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2d8f99f9d80e9957"></a>[`Inno.Platform.IPlatformApplication Inno.Adapter.Platform.IPlatformBackendFactory.CreateApplication(Inno.Adapter.Platform.PlatformBackendId backend)`](../../src/adapters/platform/Inno.Adapter.Platform/IPlatformBackendFactory.cs#L28) | Creates a new platform application for the selected backend.<br>返回：A caller-owned backend-neutral platform application.<br>`backend`：platform implementation selected by the composition root. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend. |
| <a id="api-b24c962e1b2054c3"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Platform.PlatformBackendId> Inno.Adapter.Platform.IPlatformBackendFactory.supportedBackends { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/IPlatformBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-457717396dd7b603"></a>[`public interface Inno.Adapter.Platform.IPlatformBackendFactory`](../../src/adapters/platform/Inno.Adapter.Platform/IPlatformBackendFactory.cs#L9) | Creates backend-neutral platform applications from explicit backend selections. | — | — |

### `Inno.Adapter.Platform.PlatformBackendCatalog`

<!-- api: T:Inno.Adapter.Platform.PlatformBackendCatalog 8fb2ea7aaa1356e7c9eaaf13040097110273e3082b9c8a7273b547c66435c1b5 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Platform.PlatformBackendProvider}) a34137081bfb39622c94aa2734ac31b8362d77ee64b0f9a1d39846a74912032b -->
<!-- api: P:Inno.Adapter.Platform.PlatformBackendCatalog.supportedBackends f062eb45f91c949af8ad040e54939efac518c3cc6f865a88faa7843975aab955 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendCatalog.CreateApplication(Inno.Adapter.Platform.PlatformBackendId) 6473a514085e88a03865517b2ad26fc8d2e944539517d5e491d82f974bf17a51 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-55a0c4d18ca3fd41"></a>[`public Inno.Adapter.Platform.PlatformBackendCatalog.PlatformBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Platform.PlatformBackendProvider> providers)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service.<br>`providers`：Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`：The provider sequence is null.<br>`System.ArgumentException`：A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-d68580ee647b3d08"></a>[`public Inno.Platform.IPlatformApplication Inno.Adapter.Platform.PlatformBackendCatalog.CreateApplication(Inno.Adapter.Platform.PlatformBackendId backend)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendCatalog.cs#L45) | Creates a new platform application for the selected backend.<br>返回：A caller-owned backend-neutral platform application.<br>`backend`：platform implementation selected by the composition root. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend.<br>`System.InvalidOperationException`：The selected provider returned no service. |
| <a id="api-77993bc0299f285e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Platform.PlatformBackendId> Inno.Adapter.Platform.PlatformBackendCatalog.supportedBackends { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendCatalog.cs#L39) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-53938d921e23c66d"></a>[`public sealed class Inno.Adapter.Platform.PlatformBackendCatalog : Inno.Adapter.Platform.IPlatformBackendFactory`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendCatalog.cs#L11) | Resolves platform providers from one immutable, composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Platform.PlatformBackendId`

<!-- api: T:Inno.Adapter.Platform.PlatformBackendId 7b694790be21cc31d88ce119ad6e8f12dc0f346e4df5fd407869e4c8f1e0148c -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.#ctor(System.String) 148face1a4321ee82f56fa07cd69ff3f95efd62982e49801eab19a7c97aafaf5 -->
<!-- api: P:Inno.Adapter.Platform.PlatformBackendId.sdl3 1d0890bf2f8a6fb2de2c6fb84fe145e82007cfc8370a61dea740ef4235be8007 -->
<!-- api: P:Inno.Adapter.Platform.PlatformBackendId.value a3cc43a014e4beaa875b24d1a9875e8fb0804b0a482e11f89e90f64b9999a4fa -->
<!-- api: P:Inno.Adapter.Platform.PlatformBackendId.isValid 1ecac1a19caffb0f30514f34dd3aacfa3c92848411372a9052621f100131a925 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.ToString 0498c29eacec105836f417df9780398fb30e7bf9c0af4d4c2df8ed3ae44cb580 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.op_Inequality(Inno.Adapter.Platform.PlatformBackendId,Inno.Adapter.Platform.PlatformBackendId) 26159d62c489054dbb3b70833be6d2767a7fd96cd3a95bcd48f98ad2095cda7d -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.op_Equality(Inno.Adapter.Platform.PlatformBackendId,Inno.Adapter.Platform.PlatformBackendId) 5cbf751f35ec507b5838f2bf9087a1c2cebdc3c915dcd142e4a7a5a50fc2e679 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.GetHashCode fc4a352086f08db77a34c8da92beba958f02cebaa3389d830debfcc6cf99878e -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.Equals(System.Object) e8fcaff794810b023a996b85bbad3d569bf5ea5280b28b3a06c30f37a997d4c9 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendId.Equals(Inno.Adapter.Platform.PlatformBackendId) 54841c36b2ac7be92e67cd242ddf2e49454900b00205e784777a4332063ccba5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4182d1ec05c08810"></a>[`public Inno.Adapter.Platform.PlatformBackendId.PlatformBackendId(string value)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`：A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-4b0d27b992c5bc71"></a>[`public bool Inno.Adapter.Platform.PlatformBackendId.Equals(Inno.Adapter.Platform.PlatformBackendId other)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2ce9f3c97d881f1a"></a>[`public override bool Inno.Adapter.Platform.PlatformBackendId.Equals(object obj)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-581ca508521d12d1"></a>[`public override int Inno.Adapter.Platform.PlatformBackendId.GetHashCode()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-eedc45070bcbde2d"></a>[`public static bool Inno.Adapter.Platform.PlatformBackendId.operator ==(Inno.Adapter.Platform.PlatformBackendId left, Inno.Adapter.Platform.PlatformBackendId right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d69434242dac8a38"></a>[`public static bool Inno.Adapter.Platform.PlatformBackendId.operator !=(Inno.Adapter.Platform.PlatformBackendId left, Inno.Adapter.Platform.PlatformBackendId right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-33e49a67651288ca"></a>[`public override string Inno.Adapter.Platform.PlatformBackendId.ToString()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L49) | Returns the identifier without resolving a provider.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-002af4dd6ac3bb88"></a>[`public bool Inno.Adapter.Platform.PlatformBackendId.isValid { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-43fd1cc0b860d2f1"></a>[`public static Inno.Adapter.Platform.PlatformBackendId Inno.Adapter.Platform.PlatformBackendId.sdl3 { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L31) | Gets the identifier of the bundled sdl3 implementation. | — | — |
| <a id="api-4aabba1c53ecdf7b"></a>[`public string Inno.Adapter.Platform.PlatformBackendId.value { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-625b4f17bcdd768e"></a>[`public readonly record struct Inno.Adapter.Platform.PlatformBackendId : System.IEquatable<Inno.Adapter.Platform.PlatformBackendId>`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendId.cs#L8) | Identifies a platform implementation without closing the set of supported backends. | — | — |

### `Inno.Adapter.Platform.PlatformBackendProvider`

<!-- api: T:Inno.Adapter.Platform.PlatformBackendProvider dbf655deb4c038dc8104e6a2355fbee7334a378c91d6520bcc729e8b370b31bf -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendProvider.#ctor(Inno.Adapter.Platform.PlatformBackendId) e18aa6a825576b68f7cdc20f9fb6e4ec70f3f67416abb065c7be4b7d5a5ebb17 -->
<!-- api: P:Inno.Adapter.Platform.PlatformBackendProvider.id 41158748da3588c699b920fd6a18ccbf8950beacc3d3b99002cd047fbebbc873 -->
<!-- api: M:Inno.Adapter.Platform.PlatformBackendProvider.CreateApplication fc588b8c1dec6f3794abf5cb090c476c1b37ae984aa3e0f426b35fd7f4824764 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d6267c4f2054d9c7"></a>[`protected Inno.Adapter.Platform.PlatformBackendProvider.PlatformBackendProvider(Inno.Adapter.Platform.PlatformBackendId id)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendProvider.cs#L24) | Captures the identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-910f3f52937b8b94"></a>[`public abstract Inno.Platform.IPlatformApplication Inno.Adapter.Platform.PlatformBackendProvider.CreateApplication()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendProvider.cs#L42) | Creates a caller-owned platform service using this implementation.<br>返回：A new service owned by the caller; providers must not return null. | — | — |
| <a id="api-feaeb3bea20d9883"></a>[`public Inno.Adapter.Platform.PlatformBackendId Inno.Adapter.Platform.PlatformBackendProvider.id { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-0c0d04f9c6bd14a7"></a>[`public abstract class Inno.Adapter.Platform.PlatformBackendProvider`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformBackendProvider.cs#L13) | Describes one explicitly composed platform implementation and its creation boundary. | The composition owns this provider and its immutable registration identity. Products are owned by their callers. This registration does not perform type discovery. | — |

### `Inno.Adapter.Platform.PlatformNativeHandleId`

<!-- api: T:Inno.Adapter.Platform.PlatformNativeHandleId f07458603bdb3c853da7209f30b7ecd1f6226731a9bde30969c1182a208f5644 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.#ctor(System.String) 0893812d59f8ae89334f98d1b2acb62bdf54770e911c02f5d4d6a52600963c89 -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandleId.win32 be4e160ded718c1ed6254d7fcd2c3b6b2d7140fa1dde30246370367a7e65b34e -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandleId.cocoa 8f0fc848f1e9a917659f44171687c16df9c4b5093c50e5843b315de4edc4054b -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandleId.browserCanvas 4da80e9384b66f4b73ff78971acaf514f063f2fa035b72d45bc5ed93a910ee8f -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandleId.value 003003ac24ce332d71886073beb2072fdf0cf2e857dcc81c2d48cc4f8dd4983c -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandleId.isValid 2f3aeeaa4f772aaad01cfef6e903f27e7558a725f84f2dc5ad7a7144442ebf9f -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.ToString 96f29e9e0fa76fd26deb5ef7297698aae72a3531280d1f4d9863724b325f3fd2 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.op_Inequality(Inno.Adapter.Platform.PlatformNativeHandleId,Inno.Adapter.Platform.PlatformNativeHandleId) ba9c190b84f9e961e41c21e964bee32c75b5d14d2e278fcf442d29d37670fe10 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.op_Equality(Inno.Adapter.Platform.PlatformNativeHandleId,Inno.Adapter.Platform.PlatformNativeHandleId) de5f70c2163731f4ee7144101fea545e0d851e281e8acb364d88804c8ec97541 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.GetHashCode 52b0d307a61d66684dc8c61019de7f225ebb9a9ba2cd31ab5c2b511241c35d33 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.Equals(System.Object) 9a03ffcdb68282bda75eedf4ce60142295b33c35775cd2a65aac7c2931953a42 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandleId.Equals(Inno.Adapter.Platform.PlatformNativeHandleId) 81562b05bd5d1e2aabdfbc1674de02a601723d1c0d082c89275498b69e034da6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0b6989e629aa48d2"></a>[`public Inno.Adapter.Platform.PlatformNativeHandleId.PlatformNativeHandleId(string value)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L19) | Creates an ordinal, case-sensitive ABI identifier.<br>`value`：Stable nonempty identifier without whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-efc4bc36bee5ad43"></a>[`public bool Inno.Adapter.Platform.PlatformNativeHandleId.Equals(Inno.Adapter.Platform.PlatformNativeHandleId other)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-0f842ac943631868"></a>[`public override bool Inno.Adapter.Platform.PlatformNativeHandleId.Equals(object obj)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c3dd63813d5b00f5"></a>[`public override int Inno.Adapter.Platform.PlatformNativeHandleId.GetHashCode()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d2f27e8d26ba0584"></a>[`public static bool Inno.Adapter.Platform.PlatformNativeHandleId.operator ==(Inno.Adapter.Platform.PlatformNativeHandleId left, Inno.Adapter.Platform.PlatformNativeHandleId right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e5333548c20740b5"></a>[`public static bool Inno.Adapter.Platform.PlatformNativeHandleId.operator !=(Inno.Adapter.Platform.PlatformNativeHandleId left, Inno.Adapter.Platform.PlatformNativeHandleId right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-7140e5622486663d"></a>[`public override string Inno.Adapter.Platform.PlatformNativeHandleId.ToString()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L59) | Returns the identifier without resolving an implementation.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-348c780dba4a5d0e"></a>[`public static Inno.Adapter.Platform.PlatformNativeHandleId Inno.Adapter.Platform.PlatformNativeHandleId.browserCanvas { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L41) | Gets the UTF-8 canvas selector surface ABI. | — | — |
| <a id="api-c5009deac9d2a586"></a>[`public static Inno.Adapter.Platform.PlatformNativeHandleId Inno.Adapter.Platform.PlatformNativeHandleId.cocoa { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L36) | Gets the Cocoa window surface ABI. | — | — |
| <a id="api-c32494e7e866b6bf"></a>[`public bool Inno.Adapter.Platform.PlatformNativeHandleId.isValid { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L51) | Gets whether a surface ABI was assigned. | — | — |
| <a id="api-3f1f01a29bccbc6d"></a>[`public string Inno.Adapter.Platform.PlatformNativeHandleId.value { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L46) | Gets the stable identifier; the default value is unassigned. | — | — |
| <a id="api-0f70828ef9a577c3"></a>[`public static Inno.Adapter.Platform.PlatformNativeHandleId Inno.Adapter.Platform.PlatformNativeHandleId.win32 { get; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L31) | Gets the Win32 HWND surface ABI. | — | — |
| <a id="api-970b5b95c9bb2ab4"></a>[`public readonly record struct Inno.Adapter.Platform.PlatformNativeHandleId : System.IEquatable<Inno.Adapter.Platform.PlatformNativeHandleId>`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandleId.cs#L8) | Identifies an open native surface ABI understood by cooperating window and graphics adapters. | — | — |

### `Inno.Adapter.Platform.PlatformNativeHandles`

<!-- api: T:Inno.Adapter.Platform.PlatformNativeHandles 54fd50bbc0fd22869a898e128942c2038676dcda3e856a84aa41e6c6a2c6fc47 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.#ctor(System.IntPtr,System.IntPtr,Inno.Adapter.Platform.PlatformNativeHandleId) 704de19327a80ea7089d1dc33dc73f0ace8dfe5decbb875d565c1b4321685320 -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandles.windowHandle 83f9ab9943aaf15601d254e003edd7c940cc2cc8d8a54a14cebc19eb291fb88a -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandles.displayHandle d0087184d06c289ac9aede14c107b256d6e7276aace610f375a79a48bc8eaead -->
<!-- api: P:Inno.Adapter.Platform.PlatformNativeHandles.handleKind bee843946308f0203feab1568014493edfa760d056dfd04f8d37e17640dc723d -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.ToString e87148d1c5ae795c749e0332485094070a17323749fedac9b4482b0781848f83 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.op_Inequality(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Platform.PlatformNativeHandles) ee7e488e940e66ae480d2d0995f71e672573ff095b830d4af1781d2915937e0d -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.op_Equality(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Platform.PlatformNativeHandles) da1d47ad5b3b639d93d94b06ae7060bd7ec88b6ad72ae2ca3f1ceb2f17e44eb8 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.GetHashCode 6e9cc16776ca8dbc3e97d1389fab3fbfc282cbe70306dc6a857708cf278573ca -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.Equals(System.Object) 49dee11261b6bb27c502ec723ed39f6cfa0814c97510f041ca4e825cdd4af7c4 -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.Equals(Inno.Adapter.Platform.PlatformNativeHandles) 0a3a5dbb7024612b8496bfb0431f1e2cf30d5dc8bd54f6dd6397f6cff5a14a1c -->
<!-- api: M:Inno.Adapter.Platform.PlatformNativeHandles.Deconstruct(System.IntPtr@,System.IntPtr@,Inno.Adapter.Platform.PlatformNativeHandleId@) 8e3e2d32943bb546064a828f623a051e651ded364939afee5bb69aa7f96c45e7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9c541c5a35bae281"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles.PlatformNativeHandles(nint windowHandle, nint displayHandle = 0, Inno.Adapter.Platform.PlatformNativeHandleId handleKind = default(Inno.Adapter.Platform.PlatformNativeHandleId))`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Borrows surface handles from a live window for cooperating native adapters.<br>返回：A borrowed adapter-level surface description.<br>`windowHandle`：Native window handle, or a stable UTF-8 canvas selector for browser surfaces.<br>`displayHandle`：Native display handle when required by the platform.<br>`handleKind`：Platform handle kind. | Handles remain valid only while their window is alive. Consumers must not release them. The handle identifier selects the ABI; this value is not a runtime or scripting contract. | — |
| <a id="api-99e7a42f7533f8b4"></a>[`public void Inno.Adapter.Platform.PlatformNativeHandles.Deconstruct(out nint windowHandle, out nint displayHandle, out Inno.Adapter.Platform.PlatformNativeHandleId handleKind)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-e695bdc7e57ba68d"></a>[`public bool Inno.Adapter.Platform.PlatformNativeHandles.Equals(Inno.Adapter.Platform.PlatformNativeHandles other)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-16176c090243478d"></a>[`public override bool Inno.Adapter.Platform.PlatformNativeHandles.Equals(object obj)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-211ced5d98e3382f"></a>[`public override int Inno.Adapter.Platform.PlatformNativeHandles.GetHashCode()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-3f20c985eacf836f"></a>[`public static bool Inno.Adapter.Platform.PlatformNativeHandles.operator ==(Inno.Adapter.Platform.PlatformNativeHandles left, Inno.Adapter.Platform.PlatformNativeHandles right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cd67ee38fc583538"></a>[`public static bool Inno.Adapter.Platform.PlatformNativeHandles.operator !=(Inno.Adapter.Platform.PlatformNativeHandles left, Inno.Adapter.Platform.PlatformNativeHandles right)`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-203ca6e730b2590f"></a>[`public override string Inno.Adapter.Platform.PlatformNativeHandles.ToString()`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6490945a915be201"></a>[`public nint Inno.Adapter.Platform.PlatformNativeHandles.displayHandle { get; init; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L26) | Native display handle when required by the platform. | — | — |
| <a id="api-6e826b4400251e2a"></a>[`public Inno.Adapter.Platform.PlatformNativeHandleId Inno.Adapter.Platform.PlatformNativeHandles.handleKind { get; init; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L27) | Platform handle kind. | — | — |
| <a id="api-d4c462e8b85dc3af"></a>[`public nint Inno.Adapter.Platform.PlatformNativeHandles.windowHandle { get; init; }`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L25) | Native window handle, or a stable UTF-8 canvas selector for browser surfaces. | — | — |
| <a id="api-519d2d49ec00f982"></a>[`public readonly record struct Inno.Adapter.Platform.PlatformNativeHandles : System.IEquatable<Inno.Adapter.Platform.PlatformNativeHandles>`](../../src/adapters/platform/Inno.Adapter.Platform/PlatformNativeHandles.cs#L24) | Borrows surface handles from a live window for cooperating native adapters.<br>返回：A borrowed adapter-level surface description.<br>`windowHandle`：Native window handle, or a stable UTF-8 canvas selector for browser surfaces.<br>`displayHandle`：Native display handle when required by the platform.<br>`handleKind`：Platform handle kind. | Handles remain valid only while their window is alive. Consumers must not release them. The handle identifier selects the ABI; this value is not a runtime or scripting contract. | — |

## 项目依赖

- [Inno.Platform](Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
