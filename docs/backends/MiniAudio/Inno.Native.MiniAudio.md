# Inno.Native.MiniAudio

[分类索引](README.md) · [MiniAudio Toolchain](Inno.Build.Toolchains.MiniAudio.md) · [Wiki 首页](../../README.md)

该项目提供 miniaudio 0.11.25 的完整 generated C ABI binding。公开面包含 `MiniAudio` 函数表、`Ma*` enum、struct、delegate 与 pointer wrapper；完整声明从本页的分组 ABI 参考进入。

## 职责与边界

- 目标 profile 是构建输入，不是运行时可切换属性；动态 profile 从应用已部署的 `native` 树装载当前配置动态库。
- Debug 与 Release 分别绑定 `miniaudio-debug`、`miniaudio-release`。实际文件名及逻辑库名由产品部署索引提供；loader 不再推导平台前缀或扩展名。
- 该程序集是后端 ABI，不是稳定游戏音频 API。业务、Scene、Asset 与脚本不得直接依赖；由 `Inno.Adapter.Audio.MiniAudio` adapter 隔离。

## 生成来源与生命周期

Binding 由当前 BGCS 和本项目配置从 `extern/miniaudio/miniaudio.h` 生成，native library 与 managed binding 必须始终来自同一份头文件和 native source。当前 vendor 固定为 tag `0.11.25`；升级时直接重新生成当前 API，不保留旧 ABI 兼容层。

当前 generated function table 的精确符号集合以生成文件为准，并由独立 native tests 检查。首次调用任一 generated 函数会装载整个 function table；缺失任何 required export 都会立即失败。调用方必须遵守 miniaudio 原生的 init/uninit、线程与 callback 生命周期，不得跨插件代际持有 native pointer、delegate 或运行时对象。

当前发行目标包括 macOS ARM64、Windows x64 与 Browser Wasm；实机验收范围见 [当前验收](../../architecture/CURRENT_ACCEPTANCE.md)。Linux 不在当前发行范围内。

## 初始化、内存与失败边界

| 阶段 | 入口族 | owner 与结果 |
| --- | --- | --- |
| 冻结设置 | `EngineConfigInit` 或 `DeviceConfigInit` | 配置是值数据；pointer、callback 与 user data 所指对象仍由调用方保留。 |
| 初始化 | `EngineInit`、`DeviceInit` | 检查 `MaResult.Success` 后才记录初始化成功。配置和输出结构必须有匹配 ABI 及整个使用期有效的地址。 |
| 运行 | Engine/Device/Sound 的处理与控制入口 | 不因一个 pointer wrapper 仍存在就认为底层对象有效；设备、voice 与 clip 的 generation 由 Adapter 管理。 |
| 结束 voice | Sound 的停止与 Uninit | 先停止使用并排空相应工作，再释放该 voice 所引用的来源和缓冲区。 |
| 结束设备 | `EngineUninit`、`DeviceUninit` | 只对成功初始化的对象执行匹配 Uninit，随后释放调用方分配的结构内存。 |

当前 Adapter 为每个设备分配独立 MaEngine 存储，检查 EngineInit 的明确结果；初始化失败按是否已成功初始化决定 Uninit，再释放存储。无输出设备模式显式提供处理缓冲区，不伪装成已打开硬件设备。这些属于 Adapter 实现；Native API 本身不提供引擎 Asset、Identity 或 Scene 生命周期。

Raw 指针不复制底层对象、不延长来源寿命，也不替调用者执行 Uninit。跨调用保存的输入 bytes、解码来源与 callback 状态必须有明确 owner；借用内存不能在处理完成前释放。callback 可能由音频处理线程调用，不能直接操作 Editor 或 Scene owner thread，也不能将异常传回 Native。

业务使用中立 Audio API；后台解码、encoded content lease、失败 clip 与设备 generation 在 MiniAudio Adapter 收口。退出必须先完成 voice/source 与处理任务，再结束 engine 和设备存储。原生返回错误与设备不可用由 Adapter 转成中立状态和诊断。

## 基本验证

```csharp
using System.Runtime.InteropServices;
using Inno.Native.MiniAudio;

unsafe
{
    uint major = 0;
    uint minor = 0;
    uint revision = 0;
    MiniAudio.Raw.Version(&major, &minor, &revision);
    string? version = Marshal.PtrToStringUTF8((nint)MiniAudio.Raw.VersionString());
}
```

生产代码通过 [中立 Audio API](../../audio/Inno.Audio.md) 工作；上述调用仅用于 Native test 与底层 adapter 实现。

## 静态目标

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/miniaudio/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)为准。

## 源码归属

当前唯一源码 owner：`backends/MiniAudio/native/Inno.Native.MiniAudio/Inno.Native.MiniAudio.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: T:Inno.Native.MiniAudio.MiniAudio 56fe2dd550a169475ab674592354636c7813ab7d832b9e68f283d94aa0ead451 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-51696dd28a5dbd8b"></a>[`public static class Inno.Native.MiniAudio.MiniAudio`](../../../backends/MiniAudio/native/Inno.Native.MiniAudio/MiniAudio.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。

## 完整 ABI 参考

[按类型与函数分组的 ABI 参考](Inno.Native.MiniAudio.Api/README.md)保留全部生成声明及稳定锚点。项目页是职责与生命周期的唯一入口；完整参考按同一项目的主题分页，避免在一次阅读中展开数 MB 声明。

<!-- api-include: Inno.Native.MiniAudio.Api/ABI-01.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-02.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-03.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-04.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-05.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-06.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-07.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-08.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-09.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-10.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-11.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-12.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-13.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-14.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-15.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-16.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-17.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-18.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-19.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-20.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-21.md -->
<!-- api-include: Inno.Native.MiniAudio.Api/ABI-22.md -->
