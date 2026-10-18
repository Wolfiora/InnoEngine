# Inno.Text.Runtime

[Text 索引](README.md) · [Runtime](../runtime/README.md)

`TextRuntimeFactory` 以 `inno.runtime.text` 注册 Session Subsystem（order `-800`）。`TextRuntime` 持有 `ITextBackend`、字体 artifact lease 与 execution scope；Session 结束时释放后端和 lease。它不保存 Scene 对象，也不负责 GPU 上传。UI 的 Session Subsystem 明确依赖它。

公开构造入口 `TextRuntime(ITextBackend, IAssetArtifactLookup)` 明确转交 backend 所有权；`TextRuntimeFactory(Func<RuntimeSubsystemContext, TextRuntime>)` 是 Composition 用的 factory。`EnterExecutionScope` 可为受控工具/测试显式绑定 `Text` 门面；正常运行由 `OnBeginFrame` 自动绑定。`Shape`/`Rasterize` 只在 FontAsset 可解析且 artifact 完整时工作，缺失或已销毁的 backend 明确失败。扩展后端应实现 `ITextBackend` 并通过 `Inno.Adapter.Text` 工厂进入 Session，不继承此 Runtime。

## 生命周期与扩展点

Factory 的公开 `descriptor` 声明启动顺序，`Create(RuntimeSubsystemContext)` 为每个 Session 创建独立 Runtime。`TextRuntime.Shape` 先解析字体 artifact 并缓存 face，`Rasterize` 使用同一 face；`OnStop` 逆序释放原生 face、backend 和 artifact lease。`OnBeginFrame` / `OnStop` 仅为 RuntimeSubsystem 生命周期覆写，不是脚本入口。

```csharp
using System;
using Inno.Text;
using Inno.Text.Runtime;

static TextLayout ShapeTitle(
    TextRuntime runtime,
    FontAsset font
) {
    using IDisposable scope = runtime.EnterExecutionScope();
    return Text.Shape(font, "界面", new TextStyle(24f));
}
```

此段供拥有 `TextRuntime` 的 Host 使用；普通脚本在活动帧内直接使用 `Text`。错误的字体引用、已停止的 Runtime、无活动 scope 或后端错误都会明确失败；热重载后不可保存旧 face handle。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Text.Runtime.TextRuntime`

<!-- api: T:Inno.Text.Runtime.TextRuntime 7ca11833379bf54547276794e7b614a404a9f1a214f3e286232426078579c6dc -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.#ctor(Inno.Text.ITextBackend,Inno.Assets.IAssetArtifactLookup) e352a3074abd28611379c7205c4598689274ad388a23454bcc9c95c312babb6a -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 234c88b4819c2f5c21e1ee1b93aec28e786ae33b9b61b95c1ea5e72b661344f9 -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.EnterExecutionScope 35d6962ba7fe225cc253040bc91dcf052a4b14ece8ef8dbcfac464224d01da5e -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.Shape(Inno.Text.FontAsset,System.String,Inno.Text.TextStyle,Inno.Text.TextShapingOptions) bafcc4038782a9516f6d1bdc6f7c25b6567a9e31b9b7161d66abdaa3bf1a17b2 -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.Rasterize(Inno.Text.FontAsset,System.Int32,System.UInt32,System.Single) 8d24a3f7734153097399d6cbdb99d273701bb1c9208b2f3bb9e66b430b0e3ee2 -->
<!-- api: M:Inno.Text.Runtime.TextRuntime.OnStop c6374443b97785a52787768b84c6c6ea7b8ca2b824750ce9db52a7e5253c23e8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67b7b5a8096d1e62"></a>[`public Inno.Text.Runtime.TextRuntime.TextRuntime(Inno.Text.ITextBackend backend, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L30) | Creates a text runtime and assumes ownership of its backend.<br>`backend`：The native shaping backend.<br>`artifacts`：The immutable asset artifact lookup. | — | — |
| <a id="api-c43a8bad0766707a"></a>[`public System.IDisposable Inno.Text.Runtime.TextRuntime.EnterExecutionScope()`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L52) | Binds this runtime to the current asynchronous execution context.<br>返回：The caller-owned binding scope. | — | — |
| <a id="api-e48c4d33df96a84b"></a>[`protected override void Inno.Text.Runtime.TextRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L44) | Binds the script-facing text facade for the complete runtime frame.<br>`frame`：The current runtime frame. | — | — |
| <a id="api-0659c12945c75932"></a>[`protected override void Inno.Text.Runtime.TextRuntime.OnStop()`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L123) | Releases every native face and retained immutable artifact before the backend. | — | — |
| <a id="api-2eb50ce3869f089a"></a>[`public Inno.Text.GlyphBitmap Inno.Text.Runtime.TextRuntime.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L107) | Rasterizes one glyph from an imported font.<br>返回：The immutable glyph bitmap.<br>`font`：The imported font source.<br>`faceIndex`：The zero-based collection face index.<br>`glyphId`：The font-specific glyph identifier.<br>`fontSize`：The positive logical pixel size. | — | — |
| <a id="api-a01403bd1171a22c"></a>[`public Inno.Text.TextLayout Inno.Text.Runtime.TextRuntime.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L76) | Shapes one Unicode string with an imported font.<br>返回：The immutable shaped layout.<br>`font`：The imported font source.<br>`text`：The Unicode source text.<br>`style`：Font selection and sizing.<br>`options`：Language, script, and direction hints. | — | — |
| <a id="api-665a1b10f0fa3c62"></a>[`public sealed class Inno.Text.Runtime.TextRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Text.ITextService`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L14) | Owns imported font leases and a replaceable Unicode shaping backend for one runtime session. | — | — |

### `Inno.Text.Runtime.TextRuntimeFactory`

<!-- api: T:Inno.Text.Runtime.TextRuntimeFactory d565ebd5f183d1b507b6e23053bb3925e2cbc203974780c6d51db03053f214f9 -->
<!-- api: M:Inno.Text.Runtime.TextRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Text.Runtime.TextRuntime}) b6ae54845518e7a89637630fc1f073e66b62c5a9865ca2074bd028924804be85 -->
<!-- api: P:Inno.Text.Runtime.TextRuntimeFactory.descriptor a343ebeba889ceb09193617015ba9372b0937f26ba6fbc20adf629a437e744a4 -->
<!-- api: M:Inno.Text.Runtime.TextRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 93d88a5d0f7e48c0664fbc4c4367444d4fd6d2371a3b181b3e6a1ed05ce6629d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e612fa257175ebc4"></a>[`public Inno.Text.Runtime.TextRuntimeFactory.TextRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Text.Runtime.TextRuntime> runtimeFactory)`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L21) | Creates a reusable text runtime factory.<br>`runtimeFactory`：The callback that creates one runtime per session. | — | — |
| <a id="api-11b0686ea06ba0a0"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Text.Runtime.TextRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L42) | Creates one session-owned text runtime.<br>返回：The unattached runtime.<br>`context`：The isolated session construction context. | — | — |
| <a id="api-eb35c7a9b79d72c4"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Text.Runtime.TextRuntimeFactory.descriptor { get; }`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L29) | Gets stable ordering metadata that makes text available before scene simulation. | — | — |
| <a id="api-c281d2b76bba5817"></a>[`public sealed class Inno.Text.Runtime.TextRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L11) | Creates one text service for every isolated runtime session. | — | — |

## 项目依赖

- [Inno.Text](Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
