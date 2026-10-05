# Native binding generation

[Native 索引](README.md) · [构建验收](../build/Inno.Build.Cli.md) · [Wiki 首页](../README.md)

每个 `Inno.Native.XXX` 项目独立拥有 BGCS 配置与两个 MSBuild 入口。宿主生成结果为同一项目的 `Generated/Bindings.cs`，明确目标 profile 使用 `obj/<target>/<generationFingerprint>/Generated/Bindings.cs`。
目标构建先解析实际输入身份，再检查对应生成物的精确文件集合及 SHA-256；文件损坏或输入变化时重新生成。
普通宿主 `dotnet build` 只在 checked-in 文件缺失时生成。文件头的 `ABI reference target` 记录解析目标，不代表其他目标已通过 ABI 验收。

所有声明 `BindGenGeneratedBindings=true` 的组件由共同 MSBuild 规则启用 `DisableRuntimeMarshalling`。
BGCS 已生成明确宽度、所有权和转换的 ABI carrier；运行时不得再次按默认封送规则解释枚举、句柄或 bool。
此规则对宿主与静态目标一致，不在组件源码中重复 assembly attribute。实际调用验收必须覆盖返回值及参数，`sizeof` 正确并不能证明 P/Invoke 的调用签名正确。

以 SDL3 为例，在仓库根目录运行：

```shell
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:GenerateBindings -c Release
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:CheckBindings -c Release
dotnet build native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -c Release
```

其他项目只需替换 `.csproj` 路径。`GenerateBindings` 运行该项目的 BGCS 配置；`CheckBindings` 在临时目录重新生成并比较当前文件，不改写 `Generated/`。默认查找同级的 `BindGen-CS` 仓库；若位置不同，加上 `-p:BindGenRoot=/absolute/path/to/BindGen-CS`。直接使用所选 SDK 的 dotnet 可执行文件，不通过另一套生成器 Program 或固定 SDK 路径。

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

`Native/` 只放原生源代码与必要的生成桥，不放 CMake 构建目录。Text/UI 的原生中间产物分别写入对应 `Inno.Build.Toolchains.XXX/obj/native/<target>/<fingerprint>/`；最终动态库由工具链返回确切产物路径，再由 HostNativeDeployment 或平台包部署。`NativeDllLoader` 只读取当前应用部署树，不查询源码仓库或构建缓存。

所有 API 命名、枚举、marshalling 与 ownership 约束应写在所属项目的配置或扩展中。不要修改 `Generated/Bindings.cs`，也不要在项目里另写 native import。完整原生依赖、solution、ABI 和运行时测试使用 [统一 CLI 的 verify-native](../build/Inno.Build.Cli.md)；一个主机的通过结果不能替代其他平台的实机报告。

Cpp2C 的主机编译器与 BGCS 的 libclang 必须使用兼容的标准库和 builtin headers。需要指定编译器时使用既有 `BGCS_CPP2C_CXX`，不把开发者的 LLVM 或 SDK 绝对路径写入引擎配置。Web profile 从自己的 Emscripten workload 获取 sysroot。

## 目标 profile 与输出隔离

Bgfx、Sdl3、MiniAudio、Text、UI 各只有一个 Native 项目。Bindings/common.json 保存共同声明与映射，bindgen.json 为宿主 profile，bindgen.browser-wasm.json 为 wasm32/static profile；后者的实际输出由 Task 的输入指纹确定，为各项目 `obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs`，不覆盖宿主 `Generated/Bindings.cs`。所有文件来自 BGCS，不手写 DllImport 或修改生成代码。

InnoNativeTarget 通过 ProjectReference 传播，bin/obj 按目标隔离。UI Cpp2C 的 rmlui.bridge.browser-wasm.json 继承相同 facade 定义并使用实际 Emscripten sysroot；不会在 Web profile 中混用主机 C++ 头文件。Cpp2C 的 include/sysroot/compiler 配置支持环境变量展开，与生成器配置的路径语义一致。

UI 的目标 C++ 桥输出为所属 Native 项目的 `obj/browser-wasm/<generationFingerprint>/Native`，宿主桥仍为 `Native/Generated`。
目标桥和 managed binding 在共同 staging 内完成后一起提交；失败和取消不改变已完成的 generation。
MSBuild 使用 `GeneratedBindings` 输出参数选择 Compile item，避免沿用调用前求值的固定路径。
目标生成完成后按 Compile item 身份移除原绑定，再加入选中的源；编译前要求恰好一份 `Bindings.cs`。
生成器 Task 与组件扩展使用独立的 `artifacts/build-tools/bindings` 宿主输出，移除 Player 的
目标、发布、调试与自定义输出参数。完整托管发布请求通过 SDK 的 `ArtifactsPath` 隔离，
不对单个 BGCS.Runtime 引用注入 restore 无法复现的输出属性；BGCS.Runtime 本身是目标中立 IL。
BGCS 不需要引用或识别 InnoEngine 属性。
工具链传入请求独占的 `BindGenDescriptorOutput` 文件，读取成功后的身份与路径，再以 `INNO_UI_BRIDGE_ROOT` 参数交给 CMake。
指纹包括 BGCS 各生成程序集、配置、plugin/lowering、编译器、实际 sysroot 和输入 header；扩展必须声明可靠的缓存身份。
目标缓存命中仍验证 bytes，`CheckBindings` 在独立 staging 检查 C 桥和 managed source。
