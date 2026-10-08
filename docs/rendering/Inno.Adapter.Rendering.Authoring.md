# Inno.Adapter.Rendering.Authoring

[Rendering 索引](README.md) · [Runtime adapter contract](Inno.Adapter.Rendering.md) · [默认 Authoring catalog](../runtime/Inno.Adapter.Authoring.Default.md)

该项目把 rendering authoring compiler contract 与 runtime device factory 分离，只进入 Editor/authoring 闭包。

## 公开 API

- `IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain`：创建与所选 runtime backend 匹配的 shader target compiler。
- `IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler`：创建匹配的 texture target compiler。

接口使用开放的 `RenderingBackendId` 选择实现，并返回 `Inno.Rendering.Assets` 中的中立 compiler contract。具体 shaderc/texturec 工具、路径与进程实现只存在于默认 Adapter 及对应 Toolchain。

`RenderingAuthoringBackendProvider` 声明 `id`、`CreateShaderCompilerToolchain()` 和 `CreateTextureTargetCompiler()`。
`RenderingAuthoringBackendCatalog(runtime, providers)` 捕获不可变注册集合，并在创建 native device 前校验 runtime 与 authoring ID 集合完全一致；空 ID、重复 ID 或未配对集合抛出 `ArgumentException`。
`supportedBackends` 提供当前注册集合；未注册 ID 的创建操作明确失败。
`DefaultAuthoringAdapterCatalog(renderingProviders, authoringProviders)` 支持完整自定义配对集合；两者均 null 使用内置 BGFX。

该程序集允许依赖 `Inno.Rendering.Assets`，但 `Inno.Adapter.Rendering`、`Inno.Adapter`、`Inno.Adapter.Default`、`Inno.Shell` 与 Player 均不得反向引用它。

## 注册身份

`RenderingAuthoringBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L10) | Creates offline rendering compilers paired with a selected runtime rendering backend. |
| [`Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L26) | Creates the shader compiler toolchain paired with the selected rendering backend. |
| [`Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L37) | Creates the texture compiler paired with the selected rendering backend. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory.supportedBackends`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/IRenderingAuthoringBackendFactory.cs#L15) | Gets the runtime backend identities supported by this authoring composition. |

### `Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L12) | Pairs one immutable authoring provider set with a runtime rendering catalog. |
| [`Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.RenderingAuthoringBackendCatalog(Inno.Adapter.Rendering.IRenderingBackendFactory runtime, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider> providers)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L28) | Validates that every runtime backend has exactly one matching authoring provider. |
| [`Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateShaderCompilerToolchain(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L57) | Creates a shader compiler toolchain using this implementation's validated inputs. |
| [`Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.CreateTextureTargetCompiler(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L70) | Creates a texture target compiler using this implementation's validated inputs. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.RenderingAuthoringBackendCatalog.supportedBackends`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendCatalog.cs#L46) | Gets backend registrations available in this type generation. |

### `Inno.Adapter.Rendering.RenderingAuthoringBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingAuthoringBackendProvider`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L10) | Supplies authoring tools paired with one runtime rendering implementation. |
| [`Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.RenderingAuthoringBackendProvider(Inno.Adapter.Rendering.RenderingBackendId id)`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L21) | Captures the registration identity assigned by the composition owner. |
| [`Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.id`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L31) | Gets the stable runtime implementation identity served by these tools. |
| [`abstract Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateShaderCompilerToolchain()`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L39) | Creates the shader toolchain for this implementation. |
| [`abstract Inno.Rendering.Assets.Authoring.ITextureTargetCompiler Inno.Adapter.Rendering.RenderingAuthoringBackendProvider.CreateTextureTargetCompiler()`](../../src/adapters/rendering/Inno.Adapter.Rendering.Authoring/RenderingAuthoringBackendProvider.cs#L47) | Creates the texture compiler for this implementation. |

## 项目依赖

- [Inno.Adapter.Rendering](Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets.Authoring](Inno.Rendering.Assets.Authoring.md)：公开引用边界由实际签名核对。
