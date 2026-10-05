# Inno.Native.MiniAudio

[Native 索引](README.md) · [MiniAudio Toolchain](../build/Inno.Build.Toolchains.MiniAudio.md) · [Wiki 首页](../README.md)

该项目提供 miniaudio 0.11.25 的完整 generated C ABI binding。公开面包含 `MiniAudio` 函数表、`Ma*` enum、struct、delegate 与 pointer wrapper；精确成员以同项目生成源码和 XML 文档为准。

## 职责与边界

- 目标 profile 是构建输入，不是运行时可切换属性；动态 profile 从应用已部署的 `native` 树装载当前配置动态库。
- Debug 与 Release 分别绑定 `miniaudio-debug`、`miniaudio-release`。macOS 的 `lib` 前缀及扩展名由 `Inno.Native.LibraryLoading` 解析。
- 该程序集是后端 ABI，不是稳定游戏音频 API。业务、Scene、Asset 与脚本不得直接依赖；由 `Inno.Adapter.Audio.MiniAudio` adapter 隔离。

## 生成来源与生命周期

Binding 由当前 BGCS 和本项目配置从 `extern/miniaudio/miniaudio.h` 生成，native library 与 managed binding 必须始终来自同一份头文件和 native source。当前 vendor 固定为 tag `0.11.25`；升级时直接重新生成当前 API，不保留旧 ABI 兼容层。

当前 generated function table 的精确符号集合以生成文件为准，并由独立 native tests 检查。首次调用任一 generated 函数会装载整个 function table；缺失任何 required export 都会立即失败。调用方必须遵守 miniaudio 原生的 init/uninit、线程与 callback 生命周期，不得跨插件代际持有 native pointer、delegate 或运行时对象。

当前发行目标包括 macOS ARM64、Windows x64 与 Browser Wasm；实机验收范围见 [本次报告](../architecture/PLATFORM_RUNTIME_ACCEPTANCE.md)。Linux 不在当前发行范围内。

## 基本验证

```csharp
using Inno.Native.MiniAudio;

uint major = 0;
uint minor = 0;
uint revision = 0;
MiniAudio.Version(ref major, ref minor, ref revision);
string? version = MiniAudio.VersionString();
```

生产代码通过 [中立 Audio API](../audio/Inno.Audio.md) 工作；上述调用仅用于 Native test 与底层 adapter 实现。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。生成输出分别属于宿主 Generated/Bindings.cs 和目标 obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs，没有 Native Browser 副本。
