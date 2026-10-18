# Inno.UI.Runtime

[UI 索引](README.md) · [Runtime](../runtime/README.md)

`UiRuntimeFactory` 以 `inno.runtime.ui` 注册 Session Subsystem（order `-700`），依赖 Input 与 Text。`UiRuntime` 管理导入文档的字体 artifact、后端与 Context，并允许每个 Context 接收独立 `UiInputSnapshot`。调用方先把实际输出区域命中的输入转换为 Context 局部坐标，再显式更新对应 Context。每个 Context 的输入范围独立。文档关闭或 Context 销毁时，最后一个使用者释放对应字体 lease；Session 释放时清理剩余资源。

公开构造入口 `UiRuntime(IUiBackend, IAssetArtifactLookup)` 转交后端所有权。Runtime 只根据 backend 的 implementation ID、document language 与 capability 做中立校验，不出现具体实现名。它在加载导入文档前注册该文档声明的字体，以文档内容版本隔离字体族；同一字体 artifact 被多个 Context 使用时只保留一份 lease。UI 只依赖 Input 帧序，不再伪依赖独立 Text runtime。

## 生命周期与扩展点

Factory 的公开 `descriptor` 声明 Input/Text 依赖，`Create(RuntimeSubsystemContext)` 为每个 Session 构造 Runtime。`UiRuntime` 的公开方法与 [IUiService](Inno.UI.md) 一一对应：先 `CreateContext`，然后 `LoadDocument` / `ShowDocument`，每帧 `Update`、`DrainEvents`、`Render`，最终 `CloseDocument` / `DestroyContext`。`OnBeginFrame` 捕获 Input 快照，`OnStop` 释放 backend 及字体 artifact lease；它们只是 RuntimeSubsystem 覆写，不属于脚本 API。

```csharp
using System;
using Inno.UI;
using Inno.UI.Runtime;

static void WithDocument(
    UiRuntime runtime,
    UiDocumentSource source,
    Action<UiContextHandle, UiDocumentHandle> consumeWhileActive
) {
    using IDisposable scope = runtime.EnterExecutionScope();
    UiContextHandle context = UI.CreateContext(new UiContextOptions("HUD", 1280, 720));
    try
    {
        UiDocumentHandle document = UI.LoadDocument(context, source);
        UI.ShowDocument(context, document);
        consumeWhileActive(context, document);
    }
    finally
    {
        UI.DestroyContext(context);
    }
}
```

此段供 Host 控制 Session 的场景使用；普通脚本在活动帧内调用 `UI` 门面。跨 Session 的 Context/Document handle 不能复用；持久配置只保存资产身份与业务数据。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.UI.Runtime.UiRuntime`

<!-- api: T:Inno.UI.Runtime.UiRuntime 80b56c6fe1c3f1e7dff932dcfa0977cc7a04a016640a29610ae975d65725612c -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.#ctor(Inno.UI.IUiBackend,Inno.Assets.IAssetArtifactLookup) 8349efde0718568b31d90b6a196b79b8a6cbb0ac197f634d4918a8f67993671e -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 27b4988a382abbed830f83b3c7be6145374b79cafc06b712e702cd129329d5f0 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.EnterExecutionScope 18228ecde9cacfaf57618ccec9aab96a1d0c2bf1118849a5b91ff345f7c4fca2 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.CreateContext(Inno.UI.UiContextOptions) 7a36709c8f0a0e7ee75d54b290b59f3ec54f1483a4b9a0a6222cdd5b67e637de -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.DestroyContext(Inno.UI.UiContextHandle) b6cd78f0b10a0f8f7871699ee08858a7a36cd138ac5c337194b2c48d5b9b9e9a -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.SetViewport(Inno.UI.UiContextHandle,System.Int32,System.Int32,System.Single) 518e6fd393cbd8e0d3b39aaa72d327bdba114ec4571d567f5721d5c08b26cdd0 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentSource) 236c71373039a6b059d9cce772eb4d6d45a8da52cab178d5b2d94273fc788fbc -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentAsset) 03e2c25ecd5f609116e76752391ee8a99ae94029c0eeba93d600cd6d1f7de932 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.ShowDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) cb35a5434ad25cae9b4ce892d831c5146c015cd6c979634f28f4d6592e9784b0 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.HideDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) f67f765aeeccf5851cd3854510b40f72bbf4bce88fedc5f5bea11db7f7c43507 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.CloseDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) cdcfdb677fd3cd6723dfc85a39c8105d0d54170ecb088e2ddcf4913297f99b01 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.SetText(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String) 1c61300576d4ba93ff8bfb1ff9ab75f40d92af9e32e8ae989461d4262c31d9f1 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.SetContent(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiDocumentFragment) b3a7361656ad28783c72c14f79fc08e3990af33ac0884572000613de07df7b1a -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.SetAttribute(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.String) 7ac81b855ca09db517838083926fc08bd40180a178ed7db5f43d4ec1ca209213 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.SetClass(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.Boolean) bd697e69bc302c77429fd39e31cce3154ca6b5de3348f0978db5b0732e2ccb7f -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.RegisterTexture(Inno.UI.UiContextHandle,System.String,Inno.UI.UiTextureData) 31c188f1bb2f841a6dbd6b75aa8daddd282cdc443187c7f8f6ceea42ca17f871 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.HasElementAtPoint(Inno.UI.UiContextHandle,Inno.Core.Mathematics.Vector2) 1ba17df3afc24eca603701fc3c28ed3419c58a6ff611ffffbb440c39349ee51e -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle) d41e622cb05dd8633a477d4219153dd2fe90a2c4352f77200a3981ce1ddec369 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) d06e0b5d54c5f8dd04c9fa79ae06f58bb6779b7480e32fe996840a464b65c76f -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.Render(Inno.UI.UiContextHandle) a1d60e83147610c40f8e98351724d1a24c3c54c790d485fe9a8391d8795de393 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.DrainEvents(Inno.UI.UiContextHandle) 8f9c102f5d5689955d1db3eb64ac3ac078f247cfe41e6d01e194970ddc307ce7 -->
<!-- api: M:Inno.UI.Runtime.UiRuntime.OnStop b3a3ec4cf07ca6bf0c2e5861e8f7aa22aea6e506e9e873835e6c1c9480c36f3a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b831e283edce0c50"></a>[`public Inno.UI.Runtime.UiRuntime.UiRuntime(Inno.UI.IUiBackend backend, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L37) | Creates a UI runtime and assumes ownership of its backend.<br>`backend`：The retained-mode UI backend.<br>`artifacts`：The immutable asset artifact lookup. | — | — |
| <a id="api-f04ab27bb5fac9c8"></a>[`public void Inno.UI.Runtime.UiRuntime.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L254) | Closes the selected document and releases its retained state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-cec8b469ab7d0fc9"></a>[`public Inno.UI.UiContextHandle Inno.UI.Runtime.UiRuntime.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L78) | Creates a context using this implementation's validated inputs.<br>返回：The validated ui context handle that represents the completed operation.<br>`options`：The validated configuration that controls this operation. | — | — |
| <a id="api-8ff0b9edfcb1f80f"></a>[`public void Inno.UI.Runtime.UiRuntime.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L90) | Destroys the context after its in-flight references retire.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-b91a22edc173566b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.Runtime.UiRuntime.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L488) | Returns and clears events emitted by this UI context.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-d8b6ac13f94c9561"></a>[`public System.IDisposable Inno.UI.Runtime.UiRuntime.EnterExecutionScope()`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L63) | Binds this runtime to the current asynchronous execution context.<br>返回：The caller-owned binding scope. | — | — |
| <a id="api-4e0befbb4a8df8ae"></a>[`public bool Inno.UI.Runtime.UiRuntime.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L419) | Tests whether an interactive document element occupies the supplied point.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`position`：The position consumed by has element at point; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-9bf29f6ab2c95d84"></a>[`public void Inno.UI.Runtime.UiRuntime.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L237) | Hides the selected document while retaining its state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-5b97c1ff0b323e1d"></a>[`public Inno.UI.UiDocumentHandle Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L161) | Loads a document into the specified independent UI context.<br>返回：The validated ui document handle that represents the completed operation.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by load document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-7383570aea6c1a56"></a>[`public Inno.UI.UiDocumentHandle Inno.UI.Runtime.UiRuntime.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L139) | Loads a document into the specified independent UI context.<br>返回：The validated ui document handle that represents the completed operation.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation. | — | — |
| <a id="api-921fb1be94023a67"></a>[`protected override void Inno.UI.Runtime.UiRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L51) | Captures the input service snapshot and binds script-facing UI for the complete runtime frame.<br>`frame`：The current runtime frame. | — | — |
| <a id="api-2947eac38c077bd2"></a>[`protected override void Inno.UI.Runtime.UiRuntime.OnStop()`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L497) | Releases native UI state before releasing retained font artifacts. | — | — |
| <a id="api-a0079f9c6b070081"></a>[`public void Inno.UI.Runtime.UiRuntime.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L398) | Registers a named RGBA texture source for UI document drawing.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>`texture`：The texture consumed by register texture; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-aa7fc284b05d4113"></a>[`public Inno.UI.UiRenderFrame Inno.UI.Runtime.UiRuntime.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L473) | Records value rendering for the current frame.<br>返回：The validated ui render frame that represents the completed operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-91ef9237f8f2d6e5"></a>[`public bool Inno.UI.Runtime.UiRuntime.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L343) | Updates the attribute state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set attribute; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set attribute operation.<br>`name`：The human-readable name used for presentation and diagnostics.<br>`value`：The concrete value read or transformed by this operation. | — | — |
| <a id="api-85a74a0bbe4f96b9"></a>[`public bool Inno.UI.Runtime.UiRuntime.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L375) | Updates the class state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set class; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set class operation.<br>`className`：The class name text validated by the set class operation.<br>`active`：Whether active behavior is enabled while set class executes. | — | — |
| <a id="api-a31b5b82adb56f07"></a>[`public bool Inno.UI.Runtime.UiRuntime.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment content)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L311) | Updates the content state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set content operation.<br>`content`：The content consumed by set content; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-9f5161281c420b7a"></a>[`public bool Inno.UI.Runtime.UiRuntime.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L283) | Updates the text state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set text; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set text operation.<br>`text`：The text text validated by the set text operation. | — | — |
| <a id="api-860ec0f4dcc725fb"></a>[`public void Inno.UI.Runtime.UiRuntime.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L117) | Updates the viewport state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation.<br>`density`：The density consumed by set viewport; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-e5314467e8d711a6"></a>[`public void Inno.UI.Runtime.UiRuntime.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L220) | Makes the selected document visible in its owning UI context.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-06f897ac3fab910e"></a>[`public void Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L456) | Advances one UI context with input routed from its rendered view.<br>`context`：The context that owns the active document.<br>`input`：Input mapped into the context's local pixel coordinates. | — | — |
| <a id="api-739afc3c6fc0508f"></a>[`public void Inno.UI.Runtime.UiRuntime.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L433) | Recomputes owned state from the current validated inputs.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-7f1ed2e5b166efaa"></a>[`public sealed class Inno.UI.Runtime.UiRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.UI.IUiService`](../../src/services/ui/Inno.UI.Runtime/UiRuntime.cs#L19) | Owns retained UI contexts, imported font leases, and current-frame input for one runtime session. | — | — |

### `Inno.UI.Runtime.UiRuntimeFactory`

<!-- api: T:Inno.UI.Runtime.UiRuntimeFactory 927c6c35e6298bd4d07e3ede5e4a78cad3ca5a50a284a229057082cae2fa071c -->
<!-- api: M:Inno.UI.Runtime.UiRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.UI.Runtime.UiRuntime}) ae69bcdabfc7c37371b1a7730614ecd27a667f9acc7e0926432f1c436634fcc8 -->
<!-- api: P:Inno.UI.Runtime.UiRuntimeFactory.descriptor 17a0d810c8bea92a39c4d4154a58bdb793a1e6f6048efec04bc9cfe8fd4b41a7 -->
<!-- api: M:Inno.UI.Runtime.UiRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 0a5bb0a28136a071257e5458e0bfc6082d5cb981849e11cce97b98083d739260 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-00b596699091cd76"></a>[`public Inno.UI.Runtime.UiRuntimeFactory.UiRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.UI.Runtime.UiRuntime> runtimeFactory)`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L21) | Creates a reusable UI runtime factory.<br>`runtimeFactory`：The callback that creates one runtime per session. | — | — |
| <a id="api-a48a93bd9bf0a092"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.UI.Runtime.UiRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L46) | Creates one session-owned UI runtime.<br>返回：The unattached runtime.<br>`context`：The isolated session construction context. | — | — |
| <a id="api-ce2f86909f4e5d80"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.UI.Runtime.UiRuntimeFactory.descriptor { get; }`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L29) | Gets stable ordering metadata that makes UI available before scene simulation. | — | — |
| <a id="api-bd5d2a8d10518d7e"></a>[`public sealed class Inno.UI.Runtime.UiRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/ui/Inno.UI.Runtime/UiRuntimeFactory.cs#L11) | Creates one UI service for every isolated runtime session. | — | — |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Text](../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
