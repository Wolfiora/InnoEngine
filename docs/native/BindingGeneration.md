# Native binding generation

[Native 索引](README.md) · [构建验收](../build/Inno.Build.Cli.md) · [Wiki 首页](../README.md)

每个 `Inno.Native.XXX` 项目独立拥有 BGCS 配置与两个 MSBuild 入口。宿主生成结果为同一项目的 `Generated/Bindings.cs`，明确目标 profile 使用 `obj/<target>/Generated/Bindings.cs`。普通 `dotnet build` 编译所选目标的文件；只有该文件缺失时自动生成。文件头的 `ABI reference target` 记录解析目标，不代表其他目标已通过 ABI 验收。

以 SDL3 为例，在仓库根目录运行：

```shell
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:GenerateBindings -c Release
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:CheckBindings -c Release
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -c Release
```

其他项目只需替换 `.csproj` 路径。`GenerateBindings` 运行该项目的 BGCS 配置；`CheckBindings` 在临时目录重新生成并比较当前文件，不改写 `Generated/`。默认查找同级的 `BindGen-CS` 仓库；若位置不同，加上 `-p:BindGenRoot=/absolute/path/to/BindGen-CS`。SDK host 不在 `PATH` 时，可设置 `-p:BindGenDotNetHost=/absolute/path/to/dotnet`。

| 项目 | 项目内配置 | 项目内特殊扩展 |
| --- | --- | --- |
| `Inno.Native.Bgfx` | `Bindings/bindgen.json` | `Bindings/bgfx-api-shape.json`、`bgfx-macro-enums.json` |
| `Inno.Native.ImGui` | `Bindings/bindgen.json` | `Bindings/cimgui-api-shape.json`、`Bindings/Extension/` |
| `Inno.Native.ImGuizmo` | `Bindings/bindgen.json` | 无 |
| `Inno.Native.MiniAudio` | `Bindings/bindgen.json` | 无 |
| `Inno.Native.Sdl3` | `Bindings/bindgen.json` | 无 |
| `Inno.Native.Text` | `Bindings/bindgen.json` | 无 |
| `Inno.Native.UI` | `Bindings/bindgen.json` | `Bindings/rmlui.bridge.json` |

ImGui 的 `Bindings/Extension` 是仅在生成/检查时构建的 BGCS 插件项目；`ImVector<T>` 与 `ImTextureID` 的 ABI 实现作为模板合入同一份生成文件，不进入正常 Native 项目的源码列表。Text 的原生实现归属 `Inno.Native.Text/Native/`。UI 项目的 `GenerateBindings` 先用本项目的 bridge 配置从 `Inno.Native.UI/Native/include/RmlUiRuntime.hpp` 生成 C ABI，再生成本项目的 C# 文件；原生桥产物归属 `Inno.Native.UI/Native/Generated/`，不属于 managed binding。

`Native/` 只放原生源代码与必要的生成桥，不放 CMake 构建目录。Text/UI 的原生中间产物分别写入对应 `Inno.Build.Toolchains.XXX/obj/native/`；最终动态库写入 `.lib/<domain>/<platform>/`，由 `NativeDllLoader` 按现有搜索规则部署和加载。

所有 API 命名、枚举、marshalling 与 ownership 约束应写在所属项目的配置或扩展中。不要修改 `Generated/Bindings.cs`，也不要在项目里另写 native import。完整原生依赖、solution、ABI 和运行时测试使用 [统一 CLI 的 verify-native](../build/Inno.Build.Cli.md)；一个主机的通过结果不能替代其他平台的实机报告。

Cpp2C 的主机编译器与 BGCS 的 libclang 必须使用兼容的标准库和 builtin headers。需要指定编译器时使用既有 `BGCS_CPP2C_CXX`，不把开发者的 LLVM 或 SDK 绝对路径写入引擎配置。Web profile 从自己的 Emscripten workload 获取 sysroot。

## 目标 profile 与输出隔离

Bgfx、SDL3、MiniAudio、Text、UI 各只有一个 Native 项目。Bindings/common.json 保存共同声明与映射，bindgen.json 为宿主 profile，bindgen.browser-wasm.json 为 wasm32/static profile；后者的输出为各项目 obj/browser-wasm/Generated/Bindings.cs，不覆盖宿主 Generated/Bindings.cs。所有文件来自 BGCS，不手写 DllImport 或修改生成代码。

InnoNativeTarget 通过 ProjectReference 传播，bin/obj 按目标隔离。UI Cpp2C 的 rmlui.bridge.browser-wasm.json 继承相同 facade 定义并使用实际 Emscripten sysroot；不会在 Web profile 中混用主机 C++ 头文件。Cpp2C 的 include/sysroot/compiler 配置支持环境变量展开，与生成器配置的路径语义一致。

UI 的目标 C++ 桥输出为所属 Native 项目的 `obj/browser-wasm/Native`，宿主桥仍为 `Native/Generated`。
Browser CMake 通过 `INNO_UI_BRIDGE_ROOT` 选择目标桥；目标 BGCS profile 只解析同一目标的 header。
原生桥与 managed binding 的目标输出均与宿主输出分离，生成任何一方不会覆盖或删除另一方。
