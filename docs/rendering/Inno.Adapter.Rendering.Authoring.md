# Inno.Adapter.Rendering.Authoring

[Rendering 索引](README.md) · [Runtime adapter contract](Inno.Adapter.Rendering.md) · [默认 Authoring catalog](../runtime/Inno.Adapter.Authoring.Default.md)

该项目把 rendering authoring compiler contract 与 runtime device factory 分离，只进入 Editor/authoring 闭包。

## 公开 API

- `IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain`：创建与所选 runtime backend 匹配的 shader target compiler。
- `IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler`：创建匹配的 texture target compiler。

接口使用开放的 `RenderingBackendId` 选择实现，并返回 `Inno.Rendering.Assets.Authoring` 中的中立 compiler contract。具体 shaderc/texturec 工具、路径与进程实现属于 backend 构建实现。

`RenderingAuthoringBackendProvider` 声明 `id`、`CreateShaderCompilerToolchain()` 和 `CreateTextureTargetCompiler()`。
`RenderingAuthoringBackendCatalog(runtime, providers)` 捕获不可变注册集合，并在创建 native device 前校验 runtime 与 authoring ID 集合完全一致；空 ID、重复 ID 或未配对集合抛出 `ArgumentException`。
`supportedBackends` 提供当前注册集合；未注册 ID 的创建操作明确失败。
`DefaultAuthoringAdapterCatalog(options, authoringProviders, presentationProviders)` 接收完整显式组合；options.rendering 与 authoringProviders 的 Backend ID 必须配对。不存在 null 默认 BGFX 分支。

该程序集允许依赖 `Inno.Rendering.Assets.Authoring`，但 `Inno.Adapter.Rendering`、`Inno.Adapter`、`Inno.Adapter.Default`、`Inno.Shell` 与 Player 均不得反向引用它。

## 注册身份

`RenderingAuthoringBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`

<!-- api: T:Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory 7bcc77b8b3ee5c07ea86a09b569d8a2c36658fd938a3d913e425baf1cbc2ed36 -->
<!-- api: P:Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.supportedBackends 110582cf9ae8cbd4aa9bf175306ea135b843ec78502d28bdb49b3be333f0bed1 -->
<!-- api: M:Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId) f7c99e6350d0cb241ad7fe2000b7d82059c74ec1e15b08a5858d9d7c2fe28e1f -->
<!-- api: M:Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId) 6e906c6e352724e04474f88449861a464d1a9e8802d23b2c81c745cb2b7b7a7f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ecca267be2a326ed"></a>[`Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L26) | Creates the shader compiler toolchain paired with the selected rendering backend.<br>返回：A target compiler compatible with devices created for the same backend.<br>`backend`：Stable rendering backend identity selected by the authoring composition root. | — | — |
| <a id="api-ea2b65610b830d8c"></a>[`Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L37) | Creates the texture compiler paired with the selected rendering backend.<br>返回：A texture target compiler compatible with devices created for the same backend.<br>`backend`：Stable rendering backend identity selected by the authoring composition root. | — | — |
| <a id="api-c18ffc0ed0e3767e"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.supportedBackends { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L15) | Gets the runtime backend identities supported by this authoring composition. | — | — |
| <a id="api-c6890c348cd49894"></a>[`public interface Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L10) | Creates offline rendering compilers paired with a selected runtime rendering backend. | — | — |

### `Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog`

<!-- api: T:Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog cc73e0b4ec9e40886c76cb474d28fb088a400676f9196d7e10de0258aab3edb8 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.#ctor(Inno.Adapter.Rendering.IRenderingBackendFactory,System.Collections.Generic.IEnumerable{Inno.Adapter.Rendering.RenderingAuthoringBackendProvider}) bfafe0cef1e111d66aab7a9f3e2eedd438842fc1e79e8d7cb4ae5dd46e885244 -->
<!-- api: P:Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.supportedBackends c6892c597ac46ec3ae4de5a24ddbcf3dd298b973445528d31fea3a7a24bceef6 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId) e977833d2b9aa6fe410f46be362e1be52201c1bedf8ef0741d78d946a5da64fd -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId) 8839d6a2b8654dd0e78ac79502d7156f6341e0856300cbedf10593770a0ec7ed -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eefd1be1d6940185"></a>[`public Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.RenderingAuthoringBackendCatalog(Inno.Adapter.Rendering.IRenderingBackendFactory runtime, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider> providers)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L28) | Validates that every runtime backend has exactly one matching authoring provider.<br>`runtime`：Runtime provider snapshot paired with these authoring tools.<br>`providers`：Complete authoring provider set owned by this composition generation. | — | `System.ArgumentException`：Provider IDs are missing, duplicated, or do not match the runtime set. |
| <a id="api-a606989f55803dc7"></a>[`public Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L57) | Creates a shader compiler toolchain using this implementation's validated inputs.<br>返回：The validated ishader compiler toolchain that represents the completed operation.<br>`backend`：The backend consumed by create shader compiler toolchain; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-e0cd373517079662"></a>[`public Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L70) | Creates a texture target compiler using this implementation's validated inputs.<br>返回：The validated itexture target compiler that represents the completed operation.<br>`backend`：The backend consumed by create texture target compiler; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-8dd7e79c67107bf3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.supportedBackends { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L46) | Gets backend registrations available in this type generation. | — | — |
| <a id="api-5bf82fc0930a1f05"></a>[`public sealed class Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog : Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L12) | Pairs one immutable authoring provider set with a runtime rendering catalog. | — | — |

### `Inno.Adapter.Rendering.RenderingAuthoringBackendProvider`

<!-- api: T:Inno.Adapter.Rendering.RenderingAuthoringBackendProvider 5fb6e16a70476229f80acf680c91274b0d8020b6543c174e53110799f5410631 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.#ctor(Inno.Adapter.Rendering.RenderingBackendId) 0ab3eb3c56a367f8788a2d234859ae92992b14e9d1937912d41b70d4f3a95a70 -->
<!-- api: P:Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.id 19da0a1ea9d99312a095cea2ecb486b6a1ba9b87742ce8ec2fbbc61556801c67 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateShaderCompilerToolchain f8f24da1fbc43aa6f6e1a8add8320c38e702b4b428f158d00685eabe75a22a27 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateTextureTargetCompiler 79f2fe8a3bdad442d8dd2c432c09b14bdb52118e729075ff9ee5e9c46f43ade5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-35f7709e89d6158e"></a>[`protected Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.RenderingAuthoringBackendProvider(Inno.Adapter.Rendering.RenderingBackendId id)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L21) | Captures the registration identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-b49059cff5efa6b0"></a>[`public abstract Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateShaderCompilerToolchain()`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L39) | Creates the shader toolchain for this implementation.<br>返回：A compiler compatible with devices registered under the same identity. | — | — |
| <a id="api-14cb9030236a87fd"></a>[`public abstract Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateTextureTargetCompiler()`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L47) | Creates the texture compiler for this implementation.<br>返回：A compiler compatible with devices registered under the same identity. | — | — |
| <a id="api-34917d64fbc6dc44"></a>[`public Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.id { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L31) | Gets the stable runtime implementation identity served by these tools. | — | — |
| <a id="api-0505aa995479bc9b"></a>[`public abstract class Inno.Adapter.Rendering.RenderingAuthoringBackendProvider`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L10) | Supplies authoring tools paired with one runtime rendering implementation. | — | — |

## 项目依赖

- [Inno.Adapter.Rendering](Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets.Authoring](Inno.Rendering.Assets.Authoring.md)：公开引用边界由实际签名核对。
