# Inno.Build.Toolchains.MiniAudio

[Build 索引](README.md) · [Wiki 首页](../README.md) · [统一 CLI](Inno.Build.Cli.md) · [Native](../native/Inno.Native.MiniAudio.md)

## 职责、依赖和公开 API

组件的宿主原生构建库，没有 Program。复用 ToolchainEnvironment、现有平台 builder 和产物复制规则，将 pinned extern/Native 源构建为当前宿主目标。唯一公开构建入口是 `MiniAudioToolchain.BuildAsync(NativeBuildContext context, CancellationToken cancellationToken = default)`。context 明确给出源码 checkout 与 debug/release 配置；取消杀死活动进程树，构建失败明确抛出。没有供外部派生者使用的 protected 扩展点；平台 builder 是内部实现。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.MiniAudio;

await MiniAudioToolchain.BuildAsync(new NativeBuildContext(engineRoot, "release"), cancellationToken);
```

## 常见流程与生命周期

通常使用 `Inno.Build.Cli engine` 按依赖顺序构建全部组件，再构建 Editor/Support Pack。库不启动其他组件 Program。产物属于 .lib/<component>/<target>，Debug 和 Release 命名保持独立。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与 .lib 不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。
