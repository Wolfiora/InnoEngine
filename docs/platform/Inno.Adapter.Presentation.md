# Inno.Adapter.Presentation

[Platform 索引](README.md) · [Rendering](../rendering/README.md) · [Runtime adapters](../runtime/Inno.Adapter.md)

该项目定义 authoring product 的图形 Presentation family。它不暴露 SDL3 window、BGFX handle 或具体 ImGui context。

## 公开 API

- `PresentationBackendId`：开放、区分大小写的稳定实现 ID，默认值表示未分配；内置值为 `imGui`。
- `PresentationBackendProvider`、`PresentationBackendCatalog`：组合入口提供完整 provider 集合，catalog 验证 ID、拒绝重复和 null，并发布只读 `supportedBackends`。注册不创建原生上下文。
- `PresentationFeatures`：声明中立 capability。
- `PresentationBackendOptions`：组合中立 platform/window/render device、shader compiler 与 asset source。
- `PresentationTextureHandle`：presentation generation 内使用的 opaque texture token。
- `IPresentationContext`：layout、frame draw、texture registration、render-graph contribution，以及 `TryGetWindowId(viewportId, out windowId)` 查询活动 viewport 的平台窗口身份。
- `IPresentationBackendFactory`：创建 presentation context。
- `IAuthoringAdapterCatalog`：在 runtime `IAdapterCatalog` 上增加 rendering authoring 与 presentation factory。

EditorHost 只保存 `IPresentationContext`。SDL3 event bridge 与 BGFX ImGui renderer 在 `Inno.Adapter.Authoring.Default` 内组合，不能出现在 Host 的字段、参数、返回值或 using 中。

```csharp
using IPresentationContext presentation = authoringCatalog.presentation.CreateContext(
    PresentationBackendId.imGui,
    options);
```

Presentation 是 Application 生命周期资源，必须先于 shared render device 释放。

新增实现继承 `PresentationBackendProvider`，由组合入口赋予稳定 ID 并注册到 catalog。
`CreateContext` 的调用方拥有返回的 context；provider 不接管 options 中的平台与渲染资源。
未知 ID 抛出 `NotSupportedException`，provider 返回 null 则抛出 `InvalidOperationException`。
`DefaultAuthoringAdapterCatalog` 的 `presentationProviders` 为 null 时注册内置 ImGui，空序列明确注册零个实现。
