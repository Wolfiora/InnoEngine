# Inno.Audio.Runtime API

[项目入口](../Inno.Audio.Runtime.md)

## 公开 API

| 类型 | 作用 |
| --- | --- |
| `AudioRuntime` | 播放/预加载、Mixer 安装、设备替换/恢复、Provider 同步、每帧安全点更新与释放。 |
| `AudioRuntimeOptions` | `maxVoices`、`maxPendingPreloads`、`maxContentSnapshots`、decoded cache budget、automatic stream threshold 与 device recovery interval。 |
| `AudioContentStatistics` | 最近一次内容收集的容量、接纳 snapshot 数与拒绝 Provider 数；通过 `AudioRuntime.contentStatistics` 读取。 |
| `AudioProjectSettings`（Inno.Audio） | 可部署的 default mixer、master volume 与 Runtime budgets。 |
| `MutedAudioDevice` | 无声卡或测试环境下明确的静音设备；时钟、状态和完成仍推进。 |
| `ContentReadScope`（Inno.References） | Host 提供的一帧 Identity-backed 内容边界，使用后必须释放。 |
| `AudioEmitterSnapshot`、`AudioListenerSnapshot` | 不含 Scene 类型的 emitter/listener 状态。 |
| `AudioContentProviderExtensionAttribute`、`AudioContentProvider`、`AudioContentProviderContext` | TypeCatalog 自动发现的控制线程扩展协议。 |
