# Inno.Native.Text

[Native 索引](README.md) · [Wiki 首页](../README.md) · [Text Toolchain](../build/Inno.Build.Toolchains.Text.md)

手写 `Native/include/TextRuntime.hpp` 是窄 C++ 语义 facade，`Native/src/TextRuntime.cpp` 通过 PImpl 隐藏 FreeType 与 HarfBuzz。`Bindings/text.bridge.json` 由 BGCS Cpp2C 生成宿主 `Native/Generated` C 桥，`Bindings/bindgen.json` 再从生成的 C header 生成 `Generated/Bindings.cs`。本项目没有第二套手写 C export。ABI 只供所属 Text adapter、toolchain 和 native tests 使用，脚本及其他业务不引用。升级 facade、映射或第三方输入后，通过统一构建入口重新生成并编译同一目标闭包。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `TextNative` 暴露 context 创建/销毁、face 加载/释放、metrics、UTF-8 shaping、glyph rasterization 及结果消息；`InnoText*` 数据结构只为 ABI 使用，不是引擎脚本协议。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/text/<platform>`，缺库即启动失败，不查询仓库构建缓存或提供伪排版结果。

`InnoTextRuntime` 与 face ID 由 [Text adapter](../text/Inno.Adapter.Text.FreeTypeHarfBuzz.md) 独占。Runtime 复制字体输入并拥有全部 face，在销毁底层字体库之前释放 face；调用方不能跨 session 使用句柄。Shape 和 Rasterize 接收 caller-owned 缓冲区及容量，容量不足返回明确结果；UTF-8 结果说明为 borrowed process-lifetime 文本。绑定映射与 ownership 位于本项目 `Bindings/common.json`。

使用[绑定验收工具](../build/Inno.Build.Cli.md)核对生成文件、实际库和完整部署输入。本次 Windows 的 Text 契约测试通过；具体环境、数量及其他平台的实机状态见[本次验收](../architecture/PLATFORM_RUNTIME_ACCEPTANCE.md)，不从单一宿主推断其他平台已通过。

## 静态目标

同一项目通过 `text.bridge.browser-wasm.json` 和 `bindgen.browser-wasm.json` 选择 wasm32 ABI 与静态符号解析，复用共同 facade 和映射。目标编译使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。目标 C 桥和 managed 源属于同一 `obj/browser-wasm/<generationFingerprint>`，分别位于 `Native` 和 `Generated/Bindings.cs`，不会覆盖宿主生成物。CMake 中间文件属于 Text 或 Browser toolchain 的目标与指纹目录。
