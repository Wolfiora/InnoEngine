# Inno.Editor.Audio

[Editor 索引](README.md) · [Play Mode](Inno.Editor.PlayMode.md) · [Audio](../audio/README.md) · [Application](Inno.Editor.Hosting.md)

`Inno.Editor.Audio` 管理 Editor 的 Edit/Play 音频设备 generation、预览与诊断，不实现 Mixer 窗口、波形编辑器或 Scene 音频组件。

## 公开 API

| 类型 | 成员 | 语义 |
| --- | --- | --- |
| `IEditorAudioHost` | `CreateRuntimeSubsystemFactory`、`EnterExecutionScope`、`PlayPreview`、`StopPreview` | Play Mode 与组合根依赖的最小可替换边界。 |
| `EditorAudioHost` | 构造函数与 `IEditorAudioHost` 全部成员 | 默认创建 MiniAudio，失败时建立明确 muted generation 并写入 Editor Log。 |

Edit Session 启动时拥有常驻 generation。进入 Play 时创建独立 generation，并暂停 Edit master Bus；退出 Play 时先释放 Play audio，再释放 Play Scene/Session，最后恢复 Edit Bus。这样 preview Voice、游戏 Voice、句柄和 completion event 不会跨 Session 混用。

```csharp
var options = new RuntimeSessionOptions
{
    subsystemFactories = [audio.CreateRuntimeSubsystemFactory()]
};

using RuntimeSession session = engine.CreateSession(options);
session.Tick(deltaTime);
```

Audio 的 scope、update 与逆序释放全部由 Session Subsystem Pipeline 管理；`EnterExecutionScope` 只用于 Editor 帧中不属于 Session tick 的预览表现代码。`deviceFactory` 是平台组合与 headless 测试的 public 注入边界，不是测试后门。Audio extension reload 通过 Runtime 的 TypeCatalog generation 在 frame-safe update 中原子刷新；候选失败保留 last-good。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Audio.EditorAudioHost`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioVoiceHandle Inno.Editor.Audio.EditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions? options = null)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L164) | Starts an Editor-owned preview voice through an active Edit Mode session. |
| [`Inno.Editor.Audio.EditorAudioHost`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L19) | Owns Editor audio devices, previews, diagnostics, and isolated Play Mode audio generations. |
| [`Inno.Editor.Audio.EditorAudioHost.EditorAudioHost(Inno.Extensibility.Types.TypeCatalog types, Inno.Assets.IAssetArtifactLookup artifacts, Inno.Core.Diagnostics.DiagnosticHub diagnostics, System.Func<Inno.Audio.IAudioDevice> deviceFactory, System.Func<Inno.Audio.AudioProjectSettings>? settingsProvider = null)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L47) | Creates the Editor audio host over the active authoring artifact lookup and settings source. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystemFactory Inno.Editor.Audio.EditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L70) | Creates the reusable feature factory used by Edit and Play runtime sessions. |
| [`System.IDisposable Inno.Editor.Audio.EditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L147) | Binds the script-facing audio façade to one session's runtime. |
| [`bool Inno.Editor.Audio.EditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioVoiceHandle voice)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L188) | Stops one preview voice owned by an active Edit Mode session. |
| [`void Inno.Editor.Audio.EditorAudioHost.Dispose()`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L201) | Releases every active audio generation before their owning Editor sessions are torn down. |

### `Inno.Editor.Audio.IEditorAudioHost`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.AudioVoiceHandle Inno.Editor.Audio.IEditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions? options = null)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L50) | Starts an Editor-owned preview voice through an active Edit session. |
| [`Inno.Editor.Audio.IEditorAudioHost`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L11) | Coordinates isolated audio runtime generations for Edit and Play Mode sessions. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystemFactory Inno.Editor.Audio.IEditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L22) | Creates the reusable factory that contributes isolated audio runtimes to Editor sessions. |
| [`System.IDisposable Inno.Editor.Audio.IEditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L33) | Binds the script-facing façade to one session's audio runtime. |
| [`bool Inno.Editor.Audio.IEditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioVoiceHandle voice)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L68) | Stops one preview voice owned by an active Edit session. |

## 项目依赖

- [Inno.Audio.Runtime](../audio/Inno.Audio.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio](../audio/Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
