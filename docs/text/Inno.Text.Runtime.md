# Inno.Text.Runtime

[Text 索引](README.md) · [Runtime](../runtime/README.md)

`TextRuntimeFactory` 以 `inno.runtime.text` 注册 Session Subsystem（order `-800`）。`TextRuntime` 持有 `ITextBackend`、字体 artifact lease 与 execution scope；Session 结束时释放后端和 lease。它不保存 Scene 对象，也不负责 GPU 上传。UI 的 Session Subsystem 明确依赖它。

公开构造入口 `TextRuntime(ITextBackend, IAssetArtifactLookup)` 明确转交 backend 所有权；`TextRuntimeFactory(Func<RuntimeSubsystemContext, TextRuntime>)` 是 Composition 用的 factory。`EnterExecutionScope` 可为受控工具/测试显式绑定 `Text` 门面；正常运行由 `OnBeginFrame` 自动绑定。`Shape`/`Rasterize` 只在 FontAsset 可解析且 artifact 完整时工作，缺失或已销毁的 backend 明确失败。扩展后端应实现 `ITextBackend` 并通过 `Inno.Adapter.Text` 工厂进入 Session，不继承此 Runtime。

## 生命周期与扩展点

Factory 的公开 `descriptor` 声明启动顺序，`Create(RuntimeSubsystemContext)` 为每个 Session 创建独立 Runtime。`TextRuntime.Shape` 先解析字体 artifact 并缓存 face，`Rasterize` 使用同一 face；`OnStop` 逆序释放原生 face、backend 和 artifact lease。`OnBeginFrame` / `OnStop` 仅为 RuntimeSubsystem 生命周期覆写，不是脚本入口。

```csharp
using Inno.Text;

using IDisposable scope = runtime.EnterExecutionScope();
TextLayout title = Text.Shape(font, "界面", new TextStyle(24f));
```

此段供拥有 `TextRuntime` 的 Host 使用；普通脚本在活动帧内直接使用 `Text`。错误的字体引用、已停止的 Runtime、无活动 scope 或后端错误都会明确失败；热重载后不可保存旧 face handle。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Text.Runtime.TextRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.GlyphBitmap Inno.Text.Runtime.TextRuntime.Rasterize(Inno.Text.FontAsset font, int faceIndex, uint glyphId, float fontSize)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L107) | Rasterizes one glyph from an imported font. |
| [`Inno.Text.Runtime.TextRuntime`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L14) | Owns imported font leases and a replaceable Unicode shaping backend for one runtime session. |
| [`Inno.Text.Runtime.TextRuntime.TextRuntime(Inno.Text.ITextBackend backend, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L30) | Creates a text runtime and assumes ownership of its backend. |
| [`Inno.Text.TextLayout Inno.Text.Runtime.TextRuntime.Shape(Inno.Text.FontAsset font, string text, Inno.Text.TextStyle style, Inno.Text.TextShapingOptions options)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L76) | Shapes one Unicode string with an imported font. |
| [`System.IDisposable Inno.Text.Runtime.TextRuntime.EnterExecutionScope()`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L52) | Binds this runtime to the current asynchronous execution context. |
| [`override void Inno.Text.Runtime.TextRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L44) | Binds the script-facing text facade for the complete runtime frame. |
| [`override void Inno.Text.Runtime.TextRuntime.OnStop()`](../../src/services/text/Inno.Text.Runtime/TextRuntime.cs#L123) | Releases every native face and retained immutable artifact before the backend. |

### `Inno.Text.Runtime.TextRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Text.Runtime.TextRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L42) | Creates one session-owned text runtime. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Text.Runtime.TextRuntimeFactory.descriptor`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L29) | Gets stable ordering metadata that makes text available before scene simulation. |
| [`Inno.Text.Runtime.TextRuntimeFactory`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L11) | Creates one text service for every isolated runtime session. |
| [`Inno.Text.Runtime.TextRuntimeFactory.TextRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Text.Runtime.TextRuntime> runtimeFactory)`](../../src/services/text/Inno.Text.Runtime/TextRuntimeFactory.cs#L21) | Creates a reusable text runtime factory. |

## 项目依赖

- [Inno.Text](Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
