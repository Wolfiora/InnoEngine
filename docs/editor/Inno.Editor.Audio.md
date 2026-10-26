# Inno.Editor.Audio

[Editor 索引](README.md) · [Play Mode](Inno.Editor.PlayMode.md) · [Audio](../audio/README.md) · [Application](Inno.Editor.Hosting.md)

`Inno.Editor.Audio` 管理 Editor 的 Edit/Play 音频设备 generation、预览与诊断，不实现 Mixer 窗口、波形编辑器或 Scene 音频组件。

## 公开 API

| 类型 | 成员 | 语义 |
| --- | --- | --- |
| `IEditorAudioHost` | `CreateRuntimeSubsystemFactory`、`EnterExecutionScope`、`PlayPreview`、`StopPreview` | Play Mode 与组合根依赖的最小可替换边界。 |
| `EditorAudioHost` | 构造函数与 `IEditorAudioHost` 全部成员 | 默认创建 MiniAudio，失败时建立明确 muted generation 并写入 Editor Log。 |

Edit Session 启动时拥有常驻 generation。进入 Play 时创建独立 generation，并暂停 Edit master Bus；退出 Play 时先释放 Play audio，再释放 Play Scene/Session，最后恢复 Edit Bus。这样 preview Voice、游戏 Voice、句柄和 completion event 不会跨 Session 混用。

示例借用已完成音频子系统组合的 Edit Session；预览结束由同一 owner 调用 `StopPreview(session, voice)`。

```csharp
using Inno.Audio;
using Inno.Editor.Audio;
using Inno.Runtime;

static AudioVoiceHandle Preview(
    IEditorAudioHost audio,
    RuntimeSession activeEditSession,
    AudioClipAsset clip
) {
    return audio.PlayPreview(activeEditSession, clip);
}
```

Audio 的 scope、update 与逆序释放全部由 Session Subsystem Pipeline 管理；`EnterExecutionScope` 只用于 Editor 帧中不属于 Session tick 的预览表现代码。`deviceFactory` 是平台组合与 headless 测试的 public 注入边界，不是测试后门。Audio extension reload 通过 Runtime 的 TypeCatalog generation 在 frame-safe update 中原子刷新；候选失败保留 last-good。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Audio.EditorAudioHost 7a9a74a4b43fbf3ccb70c33035c333f1b586b7a88c6de78416bf6dc938695e75 -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.#ctor(Inno.Extensibility.Types.TypeCatalog,Inno.Assets.IAssetArtifactLookup,Inno.Core.Diagnostics.DiagnosticHub,System.Func{Inno.Audio.IAudioDevice},System.Func{Inno.Audio.AudioProjectSettings}) 804da94bde7b435429321a452811a390cd83c94adcb72cf1ffa6ec7069a78ff4 -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession) 0f7317889cb8314be72ed0dd893f2977dee6ac8b95e55ca0cd2202f67750456f -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession) 5576d2c664a6a128f599150bfddba5b436b30155ae7d0d33500f8985a6a16621 -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession,Inno.Audio.AudioClipAsset,System.Nullable{Inno.Audio.AudioPlayOptions}) 85eddd13cff434f7089eeac37827fcffb2fb8c67891812d350c06d19551c0b6d -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession,Inno.Audio.AudioVoiceHandle) 5c5f7e59ef963c8a04c0e40225324d65723ab3d76d43970e57927228f8bc8c2c -->
<!-- api: M:Inno.Editor.Audio.EditorAudioHost.Dispose 6767c2e4710787844c236412623c0547040248d4b2e0197bffc4fbda23563d14 -->
<!-- api: T:Inno.Editor.Audio.IEditorAudioHost 6a31fddf3fa43696ff2b4ca6a6b42075502ab074548d43cda28ae2a4dedd6ca2 -->
<!-- api: M:Inno.Editor.Audio.IEditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession) 29639e8bdaa22dba8a2d6fd1307e3684e8d51e699954daf4761481f7a79ff8ee -->
<!-- api: M:Inno.Editor.Audio.IEditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession) 959f0fc2f4052d10ac59673bac0791a09ed56f115e9bb7c3fa9ebea37c5b1879 -->
<!-- api: M:Inno.Editor.Audio.IEditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession,Inno.Audio.AudioClipAsset,System.Nullable{Inno.Audio.AudioPlayOptions}) 8ce29262b4eb538d5b76c73f9bd50ee0a7cc0f512b12d2223229c5551ceda1fc -->
<!-- api: M:Inno.Editor.Audio.IEditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession,Inno.Audio.AudioVoiceHandle) 55ad496800f8e7e4ad5ab5d8f7919a015e89cda611c9c46e5ac42c3b5caadef8 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-d185679bf694c7a0"></a>[`public sealed class Inno.Editor.Audio.EditorAudioHost : Inno.Editor.Audio.IEditorAudioHost`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L19) | Owns Editor audio devices, previews, diagnostics, and isolated Play Mode audio generations. | — | — |
| <a id="api-c6a4498c4317c19e"></a>[`public Inno.Editor.Audio.EditorAudioHost.EditorAudioHost(Inno.Extensibility.Types.TypeCatalog types, Inno.Assets.IAssetArtifactLookup artifacts, Inno.Core.Diagnostics.DiagnosticHub diagnostics, System.Func<Inno.Audio.IAudioDevice> deviceFactory, System.Func<Inno.Audio.AudioProjectSettings>? settingsProvider = null)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L47) | Creates the Editor audio host over the active authoring artifact lookup and settings source.<br>`types`: Active type catalog used for provider and mixer extension generations.<br>`artifacts`: Authoring artifact lookup shared by Edit and isolated Play sessions.<br>`diagnostics`: Shared Editor diagnostic hub that owns current audio issues.<br>`deviceFactory`: Required host-owned backend-neutral device factory.<br>`settingsProvider`: Optional host-owned callback that returns the latest isolated audio project settings snapshot. | — | — |
| <a id="api-cae213197754b210"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystemFactory Inno.Editor.Audio.EditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L70) | Creates the reusable feature factory used by Edit and Play runtime sessions.<br>`session`: The explicit session owner; it is never exposed through subsystem contracts. | A factory that creates and releases one isolated audio generation per session. | — |
| <a id="api-1224137a2838fc31"></a>[`public System.IDisposable Inno.Editor.Audio.EditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L147) | Binds the script-facing audio façade to one session's runtime.<br>`session`: Session whose feature pipeline owns an active Editor audio generation. | A strict last-in-first-out execution scope. | — |
| <a id="api-f4059cc63a7e5c31"></a>[`public Inno.Audio.AudioVoiceHandle Inno.Editor.Audio.EditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions? options = null)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L164) | Starts an Editor-owned preview voice through an active Edit Mode session.<br>`session`: Active Edit Mode session.<br>`clip`: Imported audio clip to preview.<br>`options`: Optional playback parameters; omitted values use engine defaults. | The preview voice handle, initially in the preparing state. | — |
| <a id="api-51e973f47202fc31"></a>[`public bool Inno.Editor.Audio.EditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioVoiceHandle voice)`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L188) | Stops one preview voice owned by an active Edit Mode session.<br>`session`: Active Edit Mode session.<br>`voice`: Preview voice to stop. | when a live preview was stopped. | — |
| <a id="api-b437279da15fada8"></a>[`public void Inno.Editor.Audio.EditorAudioHost.Dispose()`](../../src/composition/editor/features/Inno.Editor.Audio/EditorAudioHost.cs#L201) | Releases every active audio generation before their owning Editor sessions are torn down. | — | — |
| <a id="api-c772d406f562b0a4"></a>[`public interface Inno.Editor.Audio.IEditorAudioHost : System.IDisposable`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L11) | Coordinates isolated audio runtime generations for Edit and Play Mode sessions. | — | — |
| <a id="api-9b7795befc631748"></a>[`Inno.Runtime.Contracts.IRuntimeSubsystemFactory Inno.Editor.Audio.IEditorAudioHost.CreateRuntimeSubsystemFactory(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L22) | Creates the reusable factory that contributes isolated audio runtimes to Editor sessions.<br>`session`: The explicit session being composed. | A factory whose features are owned and released by their runtime sessions. | — |
| <a id="api-72faa45a1016dd21"></a>[`System.IDisposable Inno.Editor.Audio.IEditorAudioHost.EnterExecutionScope(Inno.Runtime.RuntimeSession session)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L33) | Binds the script-facing façade to one session's audio runtime.<br>`session`: Session whose feature pipeline owns an active Editor audio generation. | A strict last-in-first-out execution scope. | — |
| <a id="api-4127f1783c8ce9bf"></a>[`Inno.Audio.AudioVoiceHandle Inno.Editor.Audio.IEditorAudioHost.PlayPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioClipAsset clip, Inno.Audio.AudioPlayOptions? options = null)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L50) | Starts an Editor-owned preview voice through an active Edit session.<br>`session`: Active Edit Mode session.<br>`clip`: Imported audio clip to preview.<br>`options`: Optional playback parameters; omitted values use engine defaults. | The preview voice handle, initially in the preparing state. | — |
| <a id="api-917917e5ba22e1c6"></a>[`bool Inno.Editor.Audio.IEditorAudioHost.StopPreview(Inno.Runtime.RuntimeSession session, Inno.Audio.AudioVoiceHandle voice)`](../../src/composition/editor/features/Inno.Editor.Audio/IEditorAudioHost.cs#L68) | Stops one preview voice owned by an active Edit session.<br>`session`: Active Edit Mode session.<br>`voice`: Preview voice to stop. | when a live preview was stopped. | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Audio.Runtime` | 实现依赖；不传递 compile surface |
| `Inno.Core.Logging` | 实现依赖；不传递 compile surface |
| `Inno.Audio` | 公开签名依赖 |
| `Inno.Assets` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Runtime` | 公开签名依赖 |
| `Inno.Core.Diagnostics` | 公开签名依赖 |
| `Inno.Runtime.Contracts` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
