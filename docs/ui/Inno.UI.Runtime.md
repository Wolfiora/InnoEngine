# Inno.UI.Runtime

[UI 索引](README.md) · [Runtime](../runtime/README.md)

`UiRuntimeFactory` 以 `inno.runtime.ui` 注册 Session Subsystem（order `-700`），依赖 Input 与 Text。`UiRuntime` 管理导入文档的字体 artifact、后端与 Context，并允许每个 Context 接收独立 `UiInputSnapshot`。Canvas 先由实际 RenderView 路由输入，再显式更新对应 Context；不会向所有世界画布广播窗口鼠标。文档关闭或 Context 销毁时，最后一个使用者释放对应字体 lease；Session 释放时清理剩余资源。

公开构造入口 `UiRuntime(IUiBackend, IAssetArtifactLookup)` 转交后端所有权。Runtime 只根据 backend 的 implementation ID、document language 与 capability 做中立校验，不出现具体实现名。它在加载导入文档前注册该文档声明的字体，以文档内容版本隔离字体族；同一字体 artifact 被多个 Context 使用时只保留一份 lease。UI 只依赖 Input 帧序，不再伪依赖独立 Text runtime。

## 生命周期与扩展点

Factory 的公开 `descriptor` 声明 Input/Text 依赖，`Create(RuntimeSubsystemContext)` 为每个 Session 构造 Runtime。`UiRuntime` 的公开方法与 [IUiService](Inno.UI.md) 一一对应：先 `CreateContext`，然后 `LoadDocument` / `ShowDocument`，每帧 `Update`、`DrainEvents`、`Render`，最终 `CloseDocument` / `DestroyContext`。`OnBeginFrame` 捕获 Input 快照，`OnStop` 释放 backend 及字体 artifact lease；它们只是 RuntimeSubsystem 覆写，不属于脚本 API。

```csharp
using Inno.UI;

using IDisposable scope = runtime.EnterExecutionScope();
UiContextHandle context = UI.CreateContext(new UiContextOptions("HUD", 1280, 720));
UiDocumentHandle document = UI.LoadDocument(context, source);
UI.ShowDocument(context, document);
```

此段供 Host 控制 Session 的场景使用；普通脚本在活动帧内调用 `UI` 门面。跨 Session 的 Context/Document handle 不能复用；持久配置只保存资产身份与业务数据。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.UI.Runtime.UiRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Runtime.UiRuntime`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L19) | Owns retained UI contexts, imported font leases, and current-frame input for one runtime session. |
| [`Inno.UI.Runtime.UiRuntime.UiRuntime(Inno.UI.IUiBackend backend, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L37) | Creates a UI runtime and assumes ownership of its backend. |
| [`Inno.UI.UiContextHandle Inno.UI.Runtime.UiRuntime.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L78) | Creates a context using this implementation's validated inputs. |
| [`Inno.UI.UiDocumentHandle Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L161) | Loads a document into the specified independent UI context. |
| [`Inno.UI.UiDocumentHandle Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L139) | Loads a document into the specified independent UI context. |
| [`Inno.UI.UiRenderFrame Inno.UI.Runtime.UiRuntime.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L473) | Records value rendering for the current frame. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.Runtime.UiRuntime.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L488) | Returns and clears events emitted by this UI context. |
| [`System.IDisposable Inno.UI.Runtime.UiRuntime.EnterExecutionScope()`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L63) | Binds this runtime to the current asynchronous execution context. |
| [`bool Inno.UI.Runtime.UiRuntime.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L419) | Tests whether an interactive document element occupies the supplied point. |
| [`bool Inno.UI.Runtime.UiRuntime.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L343) | Updates the attribute state and applies the resulting invariants. |
| [`bool Inno.UI.Runtime.UiRuntime.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L375) | Updates the class state and applies the resulting invariants. |
| [`bool Inno.UI.Runtime.UiRuntime.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment content)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L311) | Updates the content state and applies the resulting invariants. |
| [`bool Inno.UI.Runtime.UiRuntime.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L283) | Updates the text state and applies the resulting invariants. |
| [`override void Inno.UI.Runtime.UiRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L51) | Captures the input service snapshot and binds script-facing UI for the complete runtime frame. |
| [`override void Inno.UI.Runtime.UiRuntime.OnStop()`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L497) | Releases native UI state before releasing retained font artifacts. |
| [`void Inno.UI.Runtime.UiRuntime.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L254) | Closes the selected document and releases its retained state. |
| [`void Inno.UI.Runtime.UiRuntime.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L90) | Destroys the context after its in-flight references retire. |
| [`void Inno.UI.Runtime.UiRuntime.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L237) | Hides the selected document while retaining its state. |
| [`void Inno.UI.Runtime.UiRuntime.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L398) | Registers a named RGBA texture source for UI document drawing. |
| [`void Inno.UI.Runtime.UiRuntime.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L117) | Updates the viewport state and applies the resulting invariants. |
| [`void Inno.UI.Runtime.UiRuntime.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L220) | Makes the selected document visible in its owning UI context. |
| [`void Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L433) | Recomputes owned state from the current validated inputs. |
| [`void Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L456) | Advances one UI context with input routed from its rendered view. |

### `Inno.UI.Runtime.UiRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.UI.Runtime.UiRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L46) | Creates one session-owned UI runtime. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.UI.Runtime.UiRuntimeFactory.descriptor`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L29) | Gets stable ordering metadata that makes UI available before scene simulation. |
| [`Inno.UI.Runtime.UiRuntimeFactory`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L11) | Creates one UI service for every isolated runtime session. |
| [`Inno.UI.Runtime.UiRuntimeFactory.UiRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.UI.Runtime.UiRuntime> runtimeFactory)`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L21) | Creates a reusable UI runtime factory. |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Text](../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
