# Inno.Native.UI

[分类索引](README.md) · [Wiki 首页](../../README.md) · [UI Toolchain](Inno.Build.Toolchains.UI.md)

手写的 `Native/include/RmlUiRuntime.hpp` 是唯一语义边界：它用 PImpl 与中立 DTO 隐藏 RmlUi。本项目 `Bindings/rmlui.bridge.json` 驱动 BindGen-CS Cpp2C 生成 `Native/Generated/` C ABI，随后 `Bindings/bindgen.json` 从 C header 生成本项目的 `Generated/Bindings.cs`。两个生成器使用独立目录，避免原子替换输出时互相删除。桥接库固定使用 RmlUi 6.0 源码与对应 Text/字体依赖，产物为配置专属 `inno-ui` 动态库。原生 handle 只由 UI adapter 持有；游戏脚本及渲染 Plugin 消费 `Inno.UI` 中立数据，不直接调用 ABI。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `UiNative` 覆盖 runtime/context/document 生命周期、DOM 修改、输入、字体、命名纹理、帧几何、裁剪、像素及事件复制；`InnoUi*` 数据结构只为 ABI 使用。异常文本由 Cpp2C 的 thread-local last-error 通道传递，稳定 `Result` 不泄漏 RmlUi 名称。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/ui/<platform>`，缺库和 ABI 错误直接失败，不查询源码仓库。

RmlUi 字体引擎是进程级资源，内部 `RmlUiProcessHost` 显式管理引用计数、owner thread、Context 和最后释放。Context 销毁时释放文档与像素，但最后一个 backend 关闭仍可能收到字体纹理释放回调，因此原生层保留所需渲染接口壳直至最终 shutdown。文档仅在根布局某轴仍为 `auto` 时设置该轴的 Context `100%`，显式 CSS 不被覆盖；viewport 改变继续由所属实现重新布局。Context/Document ID 在进程内单调分配，拒绝跨 backend 旧句柄。这些为内部机制，不是上层公开协议。

参见 [UI adapter](Inno.Adapter.UI.RmlUi.md) 和 [绑定验收工具](../../build/Inno.Build.Cli.md)。本次 Windows UI 契约测试通过；完整环境与游戏运行证据见[本次验收](../../architecture/PLATFORM_RUNTIME_ACCEPTANCE.md)，macOS 与其他设备实机结果分别记录。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。生成输出分别属于宿主 Generated/Bindings.cs 和目标 obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Native.UI.UiNative`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Native.UI.UiNative`](../../../backends/RmlUi/native/Inno.Native.UI/UiNative.cs#L10) | Initializes generated imports against the component's dynamic native library. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
