# Inno.Native.UI

[分类索引](README.md) · [Wiki 首页](../../README.md) · [UI Toolchain](Inno.Build.Toolchains.UI.md)

手写的 `Native/include/RmlUiRuntime.hpp` 是唯一语义边界：它用 PImpl 与中立 DTO 隐藏 RmlUi。本项目 `Bindings/rmlui.bridge.json` 驱动 BindGen-CS Cpp2C 生成 `Native/Generated/` C ABI，随后 `Bindings/bindgen.json` 从 C header 生成本项目的 `Generated/Bindings.cs`。两个生成器使用独立目录，避免原子替换输出时互相删除。桥接库固定使用 RmlUi 6.0 源码与对应 Text/字体依赖，产物为配置专属 `inno-ui` 动态库。原生 handle 只由 UI adapter 持有；游戏脚本及渲染 Plugin 消费 `Inno.UI` 中立数据，不直接调用 ABI。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `UiNative` 覆盖 runtime/context/document 生命周期、DOM 修改、输入、字体、命名纹理、帧几何、裁剪、像素及事件复制；`InnoUi*` 数据结构只为 ABI 使用。异常文本由 Cpp2C 的 thread-local last-error 通道传递，稳定 `Result` 不泄漏 RmlUi 名称。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/ui/<platform>`，缺库和 ABI 错误直接失败，不查询源码仓库。

RmlUi 字体引擎是进程级资源，内部 `RmlUiProcessHost` 显式管理引用计数、owner thread、Context 和最后释放。Context 销毁时释放文档与像素，但最后一个 backend 关闭仍可能收到字体纹理释放回调，因此原生层保留所需渲染接口壳直至最终 shutdown。文档仅在根布局某轴仍为 `auto` 时设置该轴的 Context `100%`，显式 CSS 不被覆盖；viewport 改变继续由所属实现重新布局。Context/Document ID 在进程内单调分配，拒绝跨 backend 旧句柄。这些为内部机制，不是上层公开协议。

参见 [UI adapter](Inno.Adapter.UI.RmlUi.md) 和 [绑定验收工具](../../build/Inno.Build.Cli.md)。实际执行环境与结果见[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)；macOS 和其他设备的运行证据需分别提供。

## 调用顺序与资源增量

1. 在 UI 进程宿主线程创建 Runtime，再创建 Context。所有后续调用由该线程串行协调；`Create` 检查宿主线程，不代表每个后续函数都会再次阻止错误线程访问。
2. 注册所需字体和命名纹理，加载并显示文档。Context/document ID 仅属于本 Runtime；它们不是跨 reload 的对象身份。
3. 输入传给当前 Context，调用 `Update`，再调用 `Render` 取得本帧计数。`Render` 收集数据，不直接执行 GPU 绘制。
4. 按更新索引读取 `GetMeshInfo` / `GetTextureInfo`，再按返回的资源 ID 复制变化数据；复制命令和释放 ID。未变化的资源可继续复用消费方缓存。
5. 全部增量处理完才调用 `FinishFrame`。它清除更新/释放列表，事件队列由 `ClearEvents` 独立清理。不要在下一次 Render 后继续使用上一帧的更新索引。
6. 关闭时先停止输入和绘制，销毁 Context，最后销毁 Runtime。Runtime 也会清理仍存活的 Context；其内部 renderer 的最终退休受进程宿主协调。

DOM 修改的 `changed` 表示是否找到了元素并执行设置，不是比较旧值后的差异判定。目标元素不存在时允许 `Success` 且 `changed=0`。命名纹理注册接收未预乘的 RGBA 字节，内部复制并预乘 alpha，不能预先重复预乘。

## 最小事件读取示例

下面的后端代码借用已创建的 Runtime/Context，在 owner thread 读取目标元素 ID。事件长度不含结尾零字节，复制容量必须多留一个字节。完成全部复制后清空队列；若复制失败，示例保留队列供 owner 处理失败。它不替代领域层的事件路由或对象身份解析。

```csharp
using System;
using System.Runtime.InteropServices;
using System.Text;
using Inno.Native.UI;

public static class NativeEventReader
{
    public static string[] ReadTargetIds(
        InnoUiRuntime runtime,
        ulong context
    ) {
        ulong count = 0;
        RequireSuccess(UiNative.GetEventCount(runtime, context, ref count));
        string[] targets = new string[checked((int)count)];
        for (int index = 0; index < targets.Length; index++)
        {
            InnoUiEventInfo info = default;
            RequireSuccess(UiNative.GetEventInfo(runtime, context, (ulong)index, ref info));
            int length = checked((int)info.TargetIdLength);
            byte[] bytes = new byte[checked(length + 1)];
            Span<InnoUiUtf8CodeUnit> units = MemoryMarshal.Cast<byte, InnoUiUtf8CodeUnit>(bytes.AsSpan());
            RequireSuccess(UiNative.CopyEventTargetId(runtime, context, (ulong)index, units));
            targets[index] = Encoding.UTF8.GetString(bytes.AsSpan(0, length));
        }
        RequireSuccess(UiNative.ClearEvents(runtime, context));
        return targets;
    }

    private static void RequireSuccess(InnoUiResult result)
    {
        if (result != InnoUiResult.Success)
            throw new InvalidOperationException(result.ToString());
    }
}
```

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。生成输出分别属于宿主 Generated/Bindings.cs 和目标 `artifacts/bindings/ui/browser-wasm/<generationFingerprint>/Generated/Bindings.cs`，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

覆盖当前选择的全部 314 个公开声明；语义核对 facade、Runtime、renderer 和生成调用体。下列指针包装只供 Native adapter 使用，不承担领域 Identity 或资源管理。

### `Inno.Native.UI.InnoUiByte`

<!-- api: T:Inno.Native.UI.InnoUiByte 53cee026084d37fedb5d11226532e39b2e0311903b88e23cd086622c57abc546 -->
<!-- api: F:Inno.Native.UI.InnoUiByte.Value 631b22188e7ed9a81894dbca28afabe9e69d1fa7ed7a85d103f7ef06b4dc4360 -->
<!-- api: M:Inno.Native.UI.InnoUiByte.#ctor(System.Byte) d5cd45d7b5eb21cee3d0c49de7bf15131519f12ceeb5a09314f5fe03a0ef0077 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7339999065eb5d79"></a>[`public struct Inno.Native.UI.InnoUiByte`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L75) | 一个字节的 ABI 包装。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-2b219f7f51e24a64"></a>[`public byte Inno.Native.UI.InnoUiByte.Value`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L77) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-06d80aa3584300e7"></a>[`public Inno.Native.UI.InnoUiByte.InnoUiByte(byte value = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L78) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiBytePtr`

<!-- api: T:Inno.Native.UI.InnoUiBytePtr 37440fb6eec4d849b1122ef9571efb23bc6a5ee3520cd1c7ef28911f38c93668 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.#ctor(Inno.Native.UI.InnoUiByte*) 5a95751394d8920e3af1989d3606940bb154719139be02ddc44eb5e26d7f55ae -->
<!-- api: F:Inno.Native.UI.InnoUiBytePtr.Handle 2f1e4f2552324b14b08016c5b6288caa89289381c92f806f206ff938d01e3361 -->
<!-- api: P:Inno.Native.UI.InnoUiBytePtr.IsNull 49beba239da4e4cb46c7c7cdeba133807109d87d24d486e8347ba2e02be9a649 -->
<!-- api: P:Inno.Native.UI.InnoUiBytePtr.Null bb8ab55fa3d0495abd987030bcdf808400f466ca8d707427e04d8b58161dd134 -->
<!-- api: P:Inno.Native.UI.InnoUiBytePtr.Item(System.Int32) 607f46d08d4f5e7a2d793a930cfc32be79fc2797044ab2e422d5e94d017755a5 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.op_Implicit(Inno.Native.UI.InnoUiByte*)~Inno.Native.UI.InnoUiBytePtr 59d412e9d03d065de45a9220e3f12dc095f8c3fcbdd46c85ed972d3b1632ac59 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.op_Implicit(Inno.Native.UI.InnoUiBytePtr)~Inno.Native.UI.InnoUiByte* 9aba3a5b3a3174de5c36dffbfc9cdec517a7e0d0845eb21bc0405037b562f699 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.op_Equality(Inno.Native.UI.InnoUiBytePtr,Inno.Native.UI.InnoUiBytePtr) 93856efffd1cdf76acc62df649fb8c682a3b5bdc2a58654107564651b8ad960c -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.op_Inequality(Inno.Native.UI.InnoUiBytePtr,Inno.Native.UI.InnoUiBytePtr) f78bd8bf1acc462293275ff9783533e6eaf47a8f7219444561f5194dc0c8a278 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.Equals(Inno.Native.UI.InnoUiBytePtr) 0b55c34e6ed7ff187898a9130fa3cf8b3f401ec6d1d24fc32dc4bdb54a3b695e -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.Equals(System.Object) d452987edd21d890475acec9186b7a756ce99a5b8116e1af4022611f18407f00 -->
<!-- api: M:Inno.Native.UI.InnoUiBytePtr.GetHashCode 8e9ad65cd7baab0debc064c65b613be7024928efa3c72cb0fa59ff3f836ea4dd -->
<!-- api: P:Inno.Native.UI.InnoUiBytePtr.Value 3e7f8a6d14d10ff35341c04cb7a97f1f344ac1fb3423581982e05df093faf66e -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3ffd4c8a1815793e"></a>[`public struct Inno.Native.UI.InnoUiBytePtr : System.IEquatable<Inno.Native.UI.InnoUiBytePtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L248) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-2ea905904c4263d3"></a>[`public Inno.Native.UI.InnoUiBytePtr.InnoUiBytePtr(Inno.Native.UI.InnoUiByte* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L250) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-ff0eb23a6691b58f"></a>[`public Inno.Native.UI.InnoUiByte* Inno.Native.UI.InnoUiBytePtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L255) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-c44ef19c05a1901f"></a>[`public bool Inno.Native.UI.InnoUiBytePtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L256) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-7dabbc37b3d7606f"></a>[`public static Inno.Native.UI.InnoUiBytePtr Inno.Native.UI.InnoUiBytePtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L257) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-4940074a9a1a3bc3"></a>[`public Inno.Native.UI.InnoUiByte Inno.Native.UI.InnoUiBytePtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L259) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-e2a90f2112b1b804"></a>[`public static Inno.Native.UI.InnoUiBytePtr.implicit operator Inno.Native.UI.InnoUiBytePtr(Inno.Native.UI.InnoUiByte* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L261) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-2ac7d0d3630c70ad"></a>[`public static Inno.Native.UI.InnoUiBytePtr.implicit operator Inno.Native.UI.InnoUiByte*(Inno.Native.UI.InnoUiBytePtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L262) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-64a3594cec3f7f3c"></a>[`public static bool Inno.Native.UI.InnoUiBytePtr.operator ==(Inno.Native.UI.InnoUiBytePtr left, Inno.Native.UI.InnoUiBytePtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L263) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-5c32ca62e69113ca"></a>[`public static bool Inno.Native.UI.InnoUiBytePtr.operator !=(Inno.Native.UI.InnoUiBytePtr left, Inno.Native.UI.InnoUiBytePtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L264) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-c08cf2847161b89f"></a>[`public bool Inno.Native.UI.InnoUiBytePtr.Equals(Inno.Native.UI.InnoUiBytePtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L265) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-dc808f2fecb552f6"></a>[`public override bool Inno.Native.UI.InnoUiBytePtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L266) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-15c41d6d689dfae0"></a>[`public override int Inno.Native.UI.InnoUiBytePtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L267) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-2c539d8c7c4311b7"></a>[`public ref byte Inno.Native.UI.InnoUiBytePtr.Value { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L269) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiDrawCommand`

<!-- api: T:Inno.Native.UI.InnoUiDrawCommand 7eb65fab3c3825c0a07599c8faa521c5233c21cf437e404f9cd8b9763c873edf -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.MeshId 30cc41e7dd6bb1c47df87c6acf6eef9d3bbf67acd8a74524ef521f461fae176b -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.TextureId 1cd9f251d2d27a67900328cc2eca443611f022053aaaf4215a603851c52f568b -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.ScissorEnabled 7fbf7c380209d0022a4d94e9fdd25f745d9f06529ed708cbad5e5701f4aea760 -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.ScissorX 5d72d78e6f1dedfac8b94da75e37a4fcdb1d75a3700a59c0c9fe0dd754448892 -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.ScissorY 2d0391ebbc836345d04d355c64ba65772d21216fd13637a5502004d8b0daf539 -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.ScissorWidth 397120a7086a22a4dc0ccf0ce3a0bdab2bd8a4fbb5f6332ee1b460e3448c04aa -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommand.ScissorHeight 7a9a4972d02f7276411f70447091405021fe028fb7689279723bfedbe519a63f -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommand.#ctor(System.UInt64,System.UInt64,System.Byte,System.Int32,System.Int32,System.Int32,System.Int32) 7b69d5e473606918f774abbacbbef124dc13716ea1dde99572ab81f8e1ac2299 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7a86e84197a8d41c"></a>[`public struct Inno.Native.UI.InnoUiDrawCommand`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L148) | 引用 retained mesh/texture 的绘制命令和裁剪状态。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-0f9292e7968b39d2"></a>[`public ulong Inno.Native.UI.InnoUiDrawCommand.MeshId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L150) | 本 Context 的 retained mesh ID。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-e328a6a19832512b"></a>[`public ulong Inno.Native.UI.InnoUiDrawCommand.TextureId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L151) | 本 Context 的纹理 ID。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-b2a299777851c642"></a>[`public byte Inno.Native.UI.InnoUiDrawCommand.ScissorEnabled`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L152) | 非零表示此命令启用裁剪。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-bb00217e2badb9d2"></a>[`public int Inno.Native.UI.InnoUiDrawCommand.ScissorX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L153) | 裁剪区域左边界。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-56d6ccc9d5360ffd"></a>[`public int Inno.Native.UI.InnoUiDrawCommand.ScissorY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L154) | 裁剪区域上边界。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-12b423f246d5e50a"></a>[`public int Inno.Native.UI.InnoUiDrawCommand.ScissorWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L155) | 裁剪区域宽度。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-b3996592cdf9ff6c"></a>[`public int Inno.Native.UI.InnoUiDrawCommand.ScissorHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L156) | 裁剪区域高度。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-83538bff98f0f793"></a>[`public Inno.Native.UI.InnoUiDrawCommand.InnoUiDrawCommand(ulong mesh_id = 0, ulong texture_id = 0, byte scissor_enabled = 0, int scissor_x = 0, int scissor_y = 0, int scissor_width = 0, int scissor_height = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L157) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiDrawCommandPtr`

<!-- api: T:Inno.Native.UI.InnoUiDrawCommandPtr 23cb2af67670e6700a42e737bdda7a4651a4195b72be20af94a1adef12582871 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.#ctor(Inno.Native.UI.InnoUiDrawCommand*) 4de6764a341ff104fede32e6a6c0c641674c53f2a23055bbbcc3d4c19fef1520 -->
<!-- api: F:Inno.Native.UI.InnoUiDrawCommandPtr.Handle e20348a07d79efbbfec28f02215cebaee63cddc972c3cbc5724484ff9aecaffc -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.IsNull 3619c34aa965dabe77db1945624188570f9b880b7b046630c66d3896f3f400f0 -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.Null f619feb0aa6643b8696f2dffc6978a78812cd25a226f449fac658c94e69fd5a1 -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.Item(System.Int32) ef474df4656064adcdfe5336619bf47f99763458a9a961d40050f351a0567a31 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.op_Implicit(Inno.Native.UI.InnoUiDrawCommand*)~Inno.Native.UI.InnoUiDrawCommandPtr e32d8f033a33eda01f044392d0597d6842396be5fa483c4e8c651f4e1a037946 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.op_Implicit(Inno.Native.UI.InnoUiDrawCommandPtr)~Inno.Native.UI.InnoUiDrawCommand* 5ea77577ac1f5f4c2b6cf2bb73a8d40148ac0eee78e55c49d3c3f18689b5996c -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.op_Equality(Inno.Native.UI.InnoUiDrawCommandPtr,Inno.Native.UI.InnoUiDrawCommandPtr) 358c2e83e75c0b1400c06f207216fb2e1d91904d1186b52b24380e8249249222 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.op_Inequality(Inno.Native.UI.InnoUiDrawCommandPtr,Inno.Native.UI.InnoUiDrawCommandPtr) e5cfcc9601432b4618cedef7dd09fbc885925fe32a893ffec6218d0e48ee1ba8 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.Equals(Inno.Native.UI.InnoUiDrawCommandPtr) 918bb43a62be399ef9c1c26d341a49957eb6244bda90a2a3ee2d52810c752db1 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.Equals(System.Object) d007418052a0cd7ef54247ffdbf9c24203857bf576c3df961efac193256bebc7 -->
<!-- api: M:Inno.Native.UI.InnoUiDrawCommandPtr.GetHashCode dc53624648a8895b82594a441321dd885368f055205088567eee1df9fd89c307 -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.MeshId f76c6b0929e14e3d4c8ba6c15f13057895d54e19d82caad3813762d9b295502a -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.TextureId f92d23fc9157017046c9b72590cbe680e3f21dbfbc4569349f3803385bea8b68 -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.ScissorEnabled 7ba26c440b6187643f63395967f6f969f2e069673fc1e481ee471ace1f73bcca -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.ScissorX 4dd4a21d8fe0b6f68d58209bf3092fb03e2929e0ff67e6e5eaa6e3f883ccfdba -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.ScissorY 21051a542f3f35f15300533251d19e6f42b9ba2ea5e8874ecca7288f4f58b725 -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.ScissorWidth ab5533bcd6fad9d9330c35f3e42ae55c4c51df914844198787e2b7074318de5f -->
<!-- api: P:Inno.Native.UI.InnoUiDrawCommandPtr.ScissorHeight f552ce09b4461eac30b036e2266bbcccd35f34f56d6c68a3992942be19ba6e7e -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e5da7a45b545ad48"></a>[`public struct Inno.Native.UI.InnoUiDrawCommandPtr : System.IEquatable<Inno.Native.UI.InnoUiDrawCommandPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L273) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-1bb5df039ce3e361"></a>[`public Inno.Native.UI.InnoUiDrawCommandPtr.InnoUiDrawCommandPtr(Inno.Native.UI.InnoUiDrawCommand* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L275) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-1232044d93ad3a21"></a>[`public Inno.Native.UI.InnoUiDrawCommand* Inno.Native.UI.InnoUiDrawCommandPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L280) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-fc914c33bb19238b"></a>[`public bool Inno.Native.UI.InnoUiDrawCommandPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L281) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-b86ecac729a76d33"></a>[`public static Inno.Native.UI.InnoUiDrawCommandPtr Inno.Native.UI.InnoUiDrawCommandPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L282) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-6084760adc76fdd9"></a>[`public Inno.Native.UI.InnoUiDrawCommand Inno.Native.UI.InnoUiDrawCommandPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L284) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-dfec210466f9b5db"></a>[`public static Inno.Native.UI.InnoUiDrawCommandPtr.implicit operator Inno.Native.UI.InnoUiDrawCommandPtr(Inno.Native.UI.InnoUiDrawCommand* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L286) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-f4efcf68a636d1fa"></a>[`public static Inno.Native.UI.InnoUiDrawCommandPtr.implicit operator Inno.Native.UI.InnoUiDrawCommand*(Inno.Native.UI.InnoUiDrawCommandPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L287) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-ac1508c81fcdb0fa"></a>[`public static bool Inno.Native.UI.InnoUiDrawCommandPtr.operator ==(Inno.Native.UI.InnoUiDrawCommandPtr left, Inno.Native.UI.InnoUiDrawCommandPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L288) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-8eab01486f1ec0a9"></a>[`public static bool Inno.Native.UI.InnoUiDrawCommandPtr.operator !=(Inno.Native.UI.InnoUiDrawCommandPtr left, Inno.Native.UI.InnoUiDrawCommandPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L289) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-a7227430a24fe862"></a>[`public bool Inno.Native.UI.InnoUiDrawCommandPtr.Equals(Inno.Native.UI.InnoUiDrawCommandPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L290) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-98ba7c2f5da79769"></a>[`public override bool Inno.Native.UI.InnoUiDrawCommandPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L291) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-501213627d74887f"></a>[`public override int Inno.Native.UI.InnoUiDrawCommandPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L292) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-81d0a4752663c477"></a>[`public ref ulong Inno.Native.UI.InnoUiDrawCommandPtr.MeshId { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L294) | 本 Context 的 retained mesh ID。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-bdf96649c31e43e5"></a>[`public ref ulong Inno.Native.UI.InnoUiDrawCommandPtr.TextureId { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L295) | 本 Context 的纹理 ID。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-32a0e096458712e0"></a>[`public ref byte Inno.Native.UI.InnoUiDrawCommandPtr.ScissorEnabled { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L296) | 非零表示此命令启用裁剪。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-ca866e4c2809d469"></a>[`public ref int Inno.Native.UI.InnoUiDrawCommandPtr.ScissorX { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L297) | 裁剪区域左边界。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-37e81c1b8fa6f248"></a>[`public ref int Inno.Native.UI.InnoUiDrawCommandPtr.ScissorY { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L298) | 裁剪区域上边界。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-54c778b0e279bd7a"></a>[`public ref int Inno.Native.UI.InnoUiDrawCommandPtr.ScissorWidth { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L299) | 裁剪区域宽度。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-b1234da350a3a446"></a>[`public ref int Inno.Native.UI.InnoUiDrawCommandPtr.ScissorHeight { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L300) | 裁剪区域高度。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiEventInfo`

<!-- api: T:Inno.Native.UI.InnoUiEventInfo 10b3e7f186cbef293ed6924e075d79d36c8bb5d35334e258485433936a1046c4 -->
<!-- api: F:Inno.Native.UI.InnoUiEventInfo.Type 314bccbfaada2bd18071a6c15015a1e876d5092085801eff55b696d112c55e55 -->
<!-- api: F:Inno.Native.UI.InnoUiEventInfo.Document 2afbedd3ef8012f3896f54f2084181f655cc6d18bf968edabd40a592bded34fe -->
<!-- api: F:Inno.Native.UI.InnoUiEventInfo.TargetIdLength 60f5c460b84daaed3b9d23687330e0dedac17182ed7de2169142b44b8ff025bc -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfo.#ctor(Inno.Native.UI.InnoUiEventType,System.UInt64,System.UInt64) 431b5787403c67b89608201007db72bf61b50ef52ac48b445dc73ef416df9766 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aa626eb5b12c2950"></a>[`public struct Inno.Native.UI.InnoUiEventInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L234) | 排队文档事件的种类、文档和目标文本长度。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-a4b2d6ed18d59bcd"></a>[`public Inno.Native.UI.InnoUiEventType Inno.Native.UI.InnoUiEventInfo.Type`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L236) | 事件种类。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-276e71a91fba2a10"></a>[`public ulong Inno.Native.UI.InnoUiEventInfo.Document`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L237) | 事件来源 document ID；消费时仍需由领域 owner 验证。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-e9c89085ca3ea361"></a>[`public ulong Inno.Native.UI.InnoUiEventInfo.TargetIdLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L238) | 目标元素 ID 的 UTF-8 字节数，不含结尾零字节。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-2eb5326bf328f80f"></a>[`public Inno.Native.UI.InnoUiEventInfo.InnoUiEventInfo(Inno.Native.UI.InnoUiEventType type = Inno.Native.UI.InnoUiEventType.Click, ulong document = 0, ulong target_id_length = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L239) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiEventInfoPtr`

<!-- api: T:Inno.Native.UI.InnoUiEventInfoPtr d6889f8bd22912cef587d99c6032a7d8a397642a06a34eecfe5e840a235ee6c7 -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.#ctor(Inno.Native.UI.InnoUiEventInfo*) 90638bbf29fd739791797a21e6b672885cb2880572dcb62984ca7b0b0518efa8 -->
<!-- api: F:Inno.Native.UI.InnoUiEventInfoPtr.Handle 17a20106da48d521d882457da163bc1cc8028a841f3e28d0d7270500c17afd34 -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.IsNull d37f819914be4ddcb406f1d72f2b81396cf4ae8b2466b2b97451c06f775c1b10 -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.Null 23cc4b70f28e2bf304e8c3d0a65d38d687101d4ae738219c6b0b9eb390c578f8 -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.Item(System.Int32) d6e7ae3399e03eb85457d28b3ec454d7ace60239594f94e1460ec6a359b22089 -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.op_Implicit(Inno.Native.UI.InnoUiEventInfo*)~Inno.Native.UI.InnoUiEventInfoPtr 81bd0f64095f59b2359968f0d709bb57bc30b72c075b28e07ebb1e3632ca2ace -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.op_Implicit(Inno.Native.UI.InnoUiEventInfoPtr)~Inno.Native.UI.InnoUiEventInfo* 1d68e9f2a87368d06f4919cd8b3acd88d2496571e260feb0e58ae59c1c06de5e -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.op_Equality(Inno.Native.UI.InnoUiEventInfoPtr,Inno.Native.UI.InnoUiEventInfoPtr) c8bad7ef82f14e3bf8956b63577c708df9e4da28d0e843b1c50b48cac1a97681 -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.op_Inequality(Inno.Native.UI.InnoUiEventInfoPtr,Inno.Native.UI.InnoUiEventInfoPtr) a4685578d9d8939541ea165dba97ca78cc1581310259fef047f181c4c753dba6 -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.Equals(Inno.Native.UI.InnoUiEventInfoPtr) a074eed3bde51384e0803146b6387fd7f6918e8d68ec988143ee981a167f8202 -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.Equals(System.Object) f9f1a112b217ddd98d4a9be99dbd643fe00c0bb752a9ace74c3a341474b167fe -->
<!-- api: M:Inno.Native.UI.InnoUiEventInfoPtr.GetHashCode 26adc42b278454f8a28b9027caa1601bcee8f3b0b0aa54f66c0a24e5647b7bc1 -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.Type 37623bb1631aa83dd2816eea006e8c57dc6b78ae09895c8ac1af5a61d60fe446 -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.Document ffdff5d886929431a90eccae9f8e70b8b1e262fbd8e6d9a76233159026d26e6a -->
<!-- api: P:Inno.Native.UI.InnoUiEventInfoPtr.TargetIdLength e13879e3f309885030e033cd59f2682b2d11c3902215b426c1800175dfaa04ca -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8300deb1cc8dfe9d"></a>[`public struct Inno.Native.UI.InnoUiEventInfoPtr : System.IEquatable<Inno.Native.UI.InnoUiEventInfoPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L304) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-17710e8f775a7f36"></a>[`public Inno.Native.UI.InnoUiEventInfoPtr.InnoUiEventInfoPtr(Inno.Native.UI.InnoUiEventInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L306) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-ffe7499be7411e42"></a>[`public Inno.Native.UI.InnoUiEventInfo* Inno.Native.UI.InnoUiEventInfoPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L311) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-91336ab4d6e23995"></a>[`public bool Inno.Native.UI.InnoUiEventInfoPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L312) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-cef56372a07f8bd0"></a>[`public static Inno.Native.UI.InnoUiEventInfoPtr Inno.Native.UI.InnoUiEventInfoPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L313) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-36b054a75020355b"></a>[`public Inno.Native.UI.InnoUiEventInfo Inno.Native.UI.InnoUiEventInfoPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L315) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-31f22931c8284202"></a>[`public static Inno.Native.UI.InnoUiEventInfoPtr.implicit operator Inno.Native.UI.InnoUiEventInfoPtr(Inno.Native.UI.InnoUiEventInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L317) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-f83a00d90b437f26"></a>[`public static Inno.Native.UI.InnoUiEventInfoPtr.implicit operator Inno.Native.UI.InnoUiEventInfo*(Inno.Native.UI.InnoUiEventInfoPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L318) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-eba2a66631a17e3c"></a>[`public static bool Inno.Native.UI.InnoUiEventInfoPtr.operator ==(Inno.Native.UI.InnoUiEventInfoPtr left, Inno.Native.UI.InnoUiEventInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L319) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-ce82d2c7e8323f3d"></a>[`public static bool Inno.Native.UI.InnoUiEventInfoPtr.operator !=(Inno.Native.UI.InnoUiEventInfoPtr left, Inno.Native.UI.InnoUiEventInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L320) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-6ae3bc887d516e75"></a>[`public bool Inno.Native.UI.InnoUiEventInfoPtr.Equals(Inno.Native.UI.InnoUiEventInfoPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L321) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-972a01e59100904b"></a>[`public override bool Inno.Native.UI.InnoUiEventInfoPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L322) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-6a702434ceac070e"></a>[`public override int Inno.Native.UI.InnoUiEventInfoPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L323) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-8b885c63ba9a793c"></a>[`public ref Inno.Native.UI.InnoUiEventType Inno.Native.UI.InnoUiEventInfoPtr.Type { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L325) | 事件种类。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-464ce8d157524379"></a>[`public ref ulong Inno.Native.UI.InnoUiEventInfoPtr.Document { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L326) | 事件来源 document ID；消费时仍需由领域 owner 验证。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-507fad0ba1da6c14"></a>[`public ref ulong Inno.Native.UI.InnoUiEventInfoPtr.TargetIdLength { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L327) | 目标元素 ID 的 UTF-8 字节数，不含结尾零字节。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiEventType`

<!-- api: T:Inno.Native.UI.InnoUiEventType e3244aba27dc966a239e3a6e10b49b4c818e23fd26c4c03fa4f43e1ace0d92bf -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.Click d6b877484925fc601388301a56474dfccfc75d8ea3406fbfa2005c07174de7e2 -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.Change 42ddd64b1600ebafc8c0ab4defcd58eab639d401cef27abf6a8a2b9e2fb0e6c8 -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.Submit 9930427c3b4315ff39403c10d8678ebd5ac9b49cd87f22f929e3442d86d2cd08 -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.Focus 31c0dc1c4997aaac976e76bbf586c1eb1f8dd9ce6111a9134cac547d2812e88f -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.Blur c1b2e2d0471e76b7bbd2f22a9a1de30a5d2afaf4ea30e03512e7d830d574f66b -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.MouseEnter 88868619f317118886b2b973bf0c7ae6f4115239621ae8ab205c3e3dbe5ac328 -->
<!-- api: F:Inno.Native.UI.InnoUiEventType.MouseLeave fda705b3228a2dc5b39cdeeabbc4b7f0a2e738d61dd210b71dca29570dba056f -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-07214f98c35212e2"></a>[`public enum Inno.Native.UI.InnoUiEventType : int`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L36) | 文档事件类型，不携带底层事件对象。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-4d9fde216497e932"></a>[`Inno.Native.UI.InnoUiEventType.Click = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L38) | click 事件。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-94cb175b70095616"></a>[`Inno.Native.UI.InnoUiEventType.Change = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L39) | change 事件。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-e0c7d8b3a5fe1d2f"></a>[`Inno.Native.UI.InnoUiEventType.Submit = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L40) | submit 事件。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-72e263b1f5e22947"></a>[`Inno.Native.UI.InnoUiEventType.Focus = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L41) | focus 事件。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-cbef7655a03ced0c"></a>[`Inno.Native.UI.InnoUiEventType.Blur = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L42) | blur 事件。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-b97dde2ac6747954"></a>[`Inno.Native.UI.InnoUiEventType.MouseEnter = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L43) | 当前实现由 mouseover 映射。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-74c6d04f17e84003"></a>[`Inno.Native.UI.InnoUiEventType.MouseLeave = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L44) | 当前实现由 mouseout 映射。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |

### `Inno.Native.UI.InnoUiFrameInfo`

<!-- api: T:Inno.Native.UI.InnoUiFrameInfo cf4b0b5fca017dca1ee273212b858edf64525b22d8db7698386bff5330516e5d -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfo.MeshUpdateCount 95b4ca41a040b32a766e15b5fcd65cad97acd2c9665a42e25579f8cab4f609a9 -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfo.ReleasedMeshCount c5646455f4771bab4c0dc884bb2330380735920c2f2a220152dee5a23e88420f -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfo.CommandCount d868217fbfcf6f7850a6d19c12e02f5b3fbfc2abf00161858536c49a45e43a0e -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfo.TextureUpdateCount bb115196fde29333506aab77c762581a60f6039e6ae41f88b23fa754570ca49b -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfo.ReleasedTextureCount 19f137c2c73a1b5724556cf7c6666f2771346c4d23bca9a3cac5eb89a06c60f5 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfo.#ctor(System.UInt64,System.UInt64,System.UInt64,System.UInt64,System.UInt64) c4ad7968c9f49a6d7d9f08787c176ee81b789372f0d626a5074b3faf12f49989 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0e2557346808bd53"></a>[`public struct Inno.Native.UI.InnoUiFrameInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L173) | 当前帧资源增量与命令计数。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-b028cd291e470745"></a>[`public ulong Inno.Native.UI.InnoUiFrameInfo.MeshUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L175) | 本帧 mesh 更新条目数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-d7d14f85b633023c"></a>[`public ulong Inno.Native.UI.InnoUiFrameInfo.ReleasedMeshCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L176) | 本帧释放 mesh 的条目数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-20ce6b708bc65790"></a>[`public ulong Inno.Native.UI.InnoUiFrameInfo.CommandCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L177) | 本帧 DrawCommand 数量。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-59671d4c27ce7b91"></a>[`public ulong Inno.Native.UI.InnoUiFrameInfo.TextureUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L178) | 本帧 texture 更新条目数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-8f1a8e1a07ea9e66"></a>[`public ulong Inno.Native.UI.InnoUiFrameInfo.ReleasedTextureCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L179) | 本帧释放 texture 的条目数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-be8af808a483399a"></a>[`public Inno.Native.UI.InnoUiFrameInfo.InnoUiFrameInfo(ulong mesh_update_count = 0, ulong released_mesh_count = 0, ulong command_count = 0, ulong texture_update_count = 0, ulong released_texture_count = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L180) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiFrameInfoPtr`

<!-- api: T:Inno.Native.UI.InnoUiFrameInfoPtr 6bf28a043130668744d931dcb331917fa67a0787c740fbad900529279133a60b -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.#ctor(Inno.Native.UI.InnoUiFrameInfo*) e92f1fc38eb8e1fdc41b6582f2aabc1b2c58f16f6f22af6bf27b8c9f066d20ea -->
<!-- api: F:Inno.Native.UI.InnoUiFrameInfoPtr.Handle 6f00573ffafb0d3e63cd857ef2da3b81dc36f6e390ed84ce99ae110432dfce86 -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.IsNull c976b89db91df46a13d4ba368552f736fe6791ecd05726dffc9351d74f3f1cde -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.Null 2cbfad0e030afbe6f41616971097786da8a14752477c3fed2aec74f0dc48cfb6 -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.Item(System.Int32) ce46daacb4c21bbd9fb0fc0063496a3adeac169518129bb48a332443c8dc9dc0 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.op_Implicit(Inno.Native.UI.InnoUiFrameInfo*)~Inno.Native.UI.InnoUiFrameInfoPtr f0209f597ea210580674d8bbb0927b42503b53c92084cd2243c92362edf6b5cc -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.op_Implicit(Inno.Native.UI.InnoUiFrameInfoPtr)~Inno.Native.UI.InnoUiFrameInfo* d7a0c926739de9f0c91a535ed97dfd651a01eb5beea93f125b7deef46fff0333 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.op_Equality(Inno.Native.UI.InnoUiFrameInfoPtr,Inno.Native.UI.InnoUiFrameInfoPtr) 4db747a1a3d9a320ce09b8876b70c6dd833e00ba1bd146dc38b8de09ecc6ba14 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.op_Inequality(Inno.Native.UI.InnoUiFrameInfoPtr,Inno.Native.UI.InnoUiFrameInfoPtr) ee18998e50cdcc34f8527785e9f6720042292766d762c97a06230f7e5ed30df3 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.Equals(Inno.Native.UI.InnoUiFrameInfoPtr) 9fcc7d50aaf80a20d66a5c7abb7ea36c932e9a82b0e5e070a54cbfadd1415a81 -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.Equals(System.Object) 927d2494bfeb1eb25a82a854fa079bb18aa2e6b8a8d689772b717905c7c9a98f -->
<!-- api: M:Inno.Native.UI.InnoUiFrameInfoPtr.GetHashCode de97df169904e8b6a18b8077072bef5049fb3f9cc12505bcda864d18304d019b -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.MeshUpdateCount 9a032e275f56c7eeab6f48cfdaf26251ae7c8eaefe16f4ebc158ffa243de46c7 -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.ReleasedMeshCount c15116aa29d5aef39797acec456e09fdd37f562a91e31f7e8397075e9483259f -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.CommandCount 3746f2701169facdeaa64b47090bc87de56a99c23e43788c16811787f2ef3d99 -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.TextureUpdateCount 4d80e1d7e1aab4a6279579947fbaeea01f68e9367ebf37686b7c6d21d7b32be3 -->
<!-- api: P:Inno.Native.UI.InnoUiFrameInfoPtr.ReleasedTextureCount ff39b297c80dccdc772adfed5bdc1f84496d05a9c35362f650bd3a457821b7cc -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-860b1a035b3da9f8"></a>[`public struct Inno.Native.UI.InnoUiFrameInfoPtr : System.IEquatable<Inno.Native.UI.InnoUiFrameInfoPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L331) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-67ea9d7f6d48dcbc"></a>[`public Inno.Native.UI.InnoUiFrameInfoPtr.InnoUiFrameInfoPtr(Inno.Native.UI.InnoUiFrameInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L333) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-c0c870e69354a859"></a>[`public Inno.Native.UI.InnoUiFrameInfo* Inno.Native.UI.InnoUiFrameInfoPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L338) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-432e4ae39662937e"></a>[`public bool Inno.Native.UI.InnoUiFrameInfoPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L339) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-a4e2fbf9f573e97e"></a>[`public static Inno.Native.UI.InnoUiFrameInfoPtr Inno.Native.UI.InnoUiFrameInfoPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L340) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-047ee2d829038d45"></a>[`public Inno.Native.UI.InnoUiFrameInfo Inno.Native.UI.InnoUiFrameInfoPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L342) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-bed1ade4d98b175b"></a>[`public static Inno.Native.UI.InnoUiFrameInfoPtr.implicit operator Inno.Native.UI.InnoUiFrameInfoPtr(Inno.Native.UI.InnoUiFrameInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L344) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-6ad0060aab3f7129"></a>[`public static Inno.Native.UI.InnoUiFrameInfoPtr.implicit operator Inno.Native.UI.InnoUiFrameInfo*(Inno.Native.UI.InnoUiFrameInfoPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L345) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-4c1080911117c75b"></a>[`public static bool Inno.Native.UI.InnoUiFrameInfoPtr.operator ==(Inno.Native.UI.InnoUiFrameInfoPtr left, Inno.Native.UI.InnoUiFrameInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L346) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-0394ad55f7574d32"></a>[`public static bool Inno.Native.UI.InnoUiFrameInfoPtr.operator !=(Inno.Native.UI.InnoUiFrameInfoPtr left, Inno.Native.UI.InnoUiFrameInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L347) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-f351b6a4d8c14309"></a>[`public bool Inno.Native.UI.InnoUiFrameInfoPtr.Equals(Inno.Native.UI.InnoUiFrameInfoPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L348) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-dc5c8ae04a942935"></a>[`public override bool Inno.Native.UI.InnoUiFrameInfoPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L349) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-9389e77c2a7d3e3a"></a>[`public override int Inno.Native.UI.InnoUiFrameInfoPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L350) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-ef7cb92e6df0192a"></a>[`public ref ulong Inno.Native.UI.InnoUiFrameInfoPtr.MeshUpdateCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L352) | 本帧 mesh 更新条目数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-34259e036b66db80"></a>[`public ref ulong Inno.Native.UI.InnoUiFrameInfoPtr.ReleasedMeshCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L353) | 本帧释放 mesh 的条目数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-dec6c8e70ff11b5a"></a>[`public ref ulong Inno.Native.UI.InnoUiFrameInfoPtr.CommandCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L354) | 本帧 DrawCommand 数量。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-8d6bb546faa1e3dd"></a>[`public ref ulong Inno.Native.UI.InnoUiFrameInfoPtr.TextureUpdateCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L355) | 本帧 texture 更新条目数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-239f7a258fe9bda1"></a>[`public ref ulong Inno.Native.UI.InnoUiFrameInfoPtr.ReleasedTextureCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L356) | 本帧释放 texture 的条目数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiHandle`

<!-- api: T:Inno.Native.UI.InnoUiHandle ea969fc89e28dd391333378b32f55390dbf2da160bc2055da4c7d958112ee36d -->
<!-- api: F:Inno.Native.UI.InnoUiHandle.Value 87ed5e1a2ea6c7274f5fa63c5df3c4647f6ed57299e8f75b97d80b4b7b393e0c -->
<!-- api: M:Inno.Native.UI.InnoUiHandle.#ctor(System.UInt64) a2bde2c727e7ef3443a67ad9d73a38eeb6da3443f744becff28b4003a67be7b5 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4ec402b51243f8fd"></a>[`public struct Inno.Native.UI.InnoUiHandle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L114) | 一个资源 ID 的 ABI 包装，不是 Runtime 指针。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-cee9ff3ac593a64b"></a>[`public ulong Inno.Native.UI.InnoUiHandle.Value`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L116) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-105380bff70a3776"></a>[`public Inno.Native.UI.InnoUiHandle.InnoUiHandle(ulong value = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L117) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiHandlePtr`

<!-- api: T:Inno.Native.UI.InnoUiHandlePtr 1eb224bc7e16fa8a82b0edde1f2dc4847975172db3e90ca997b7c66c2a80a818 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.#ctor(Inno.Native.UI.InnoUiHandle*) 0eb432b485bca3cb7c2184471a1b272144e176ca71247e87b97cddd592ad2901 -->
<!-- api: F:Inno.Native.UI.InnoUiHandlePtr.Handle e564281e87fb8128b70df1547d88c8f257450c8f735377c890a6b2f50d5e8993 -->
<!-- api: P:Inno.Native.UI.InnoUiHandlePtr.IsNull 6b30603256b10a5f025ec4a723ca0c036f33abf2a733a7fee7dc1cac8efc9e3e -->
<!-- api: P:Inno.Native.UI.InnoUiHandlePtr.Null b98dfa086446a1f1b7e0f5f2cdec9a6a1c4be457513fefa132fd93396a02161f -->
<!-- api: P:Inno.Native.UI.InnoUiHandlePtr.Item(System.Int32) 56c4c77005899f8ef39813707d62d72bdc0cb0c5ca391c10f08edb7b9138fc7a -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.op_Implicit(Inno.Native.UI.InnoUiHandle*)~Inno.Native.UI.InnoUiHandlePtr bab04a3686e16aff8d118d824e0ad54ecd46179a909d71098fa163c7a8a8df98 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.op_Implicit(Inno.Native.UI.InnoUiHandlePtr)~Inno.Native.UI.InnoUiHandle* 2f7b9d09fab31b5efaa8ccd159271d80525af7269625d15010b7c6702818cadb -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.op_Equality(Inno.Native.UI.InnoUiHandlePtr,Inno.Native.UI.InnoUiHandlePtr) 11e90680c7934a5739407b0eff3639b2178a3dd69035e02b3db978079540fa39 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.op_Inequality(Inno.Native.UI.InnoUiHandlePtr,Inno.Native.UI.InnoUiHandlePtr) 1aa125dc497e88ab419fd11513ea706ef45c764b03d8e57f1e465a95d0932d44 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.Equals(Inno.Native.UI.InnoUiHandlePtr) 91d95ab03e48325d28fb2d38fcf1304045a99a48adbcd087c28692feca933117 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.Equals(System.Object) 14a50b5b692a05a4b33f9eb4d28988f27e5fd416e5c925da1a545a0c7b2e0679 -->
<!-- api: M:Inno.Native.UI.InnoUiHandlePtr.GetHashCode 18ce13b5e715ed8b89344217212b4d880890f303d0e10595706bdc3bcb347225 -->
<!-- api: P:Inno.Native.UI.InnoUiHandlePtr.Value 36ad49da28068540eacb453a16ac3ceb6759de93371dd8c1e254b9a3d99458ad -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f57cff0a033e60f"></a>[`public struct Inno.Native.UI.InnoUiHandlePtr : System.IEquatable<Inno.Native.UI.InnoUiHandlePtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L360) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-2f675548639c25ba"></a>[`public Inno.Native.UI.InnoUiHandlePtr.InnoUiHandlePtr(Inno.Native.UI.InnoUiHandle* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L362) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-9d627cd14134d060"></a>[`public Inno.Native.UI.InnoUiHandle* Inno.Native.UI.InnoUiHandlePtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L367) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-d73c233c08159efb"></a>[`public bool Inno.Native.UI.InnoUiHandlePtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L368) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-6a81cad15507c7a5"></a>[`public static Inno.Native.UI.InnoUiHandlePtr Inno.Native.UI.InnoUiHandlePtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L369) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-9b6db4b616cb41e3"></a>[`public Inno.Native.UI.InnoUiHandle Inno.Native.UI.InnoUiHandlePtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L371) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-1193e19c98bfb7ad"></a>[`public static Inno.Native.UI.InnoUiHandlePtr.implicit operator Inno.Native.UI.InnoUiHandlePtr(Inno.Native.UI.InnoUiHandle* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L373) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-5b71de5b2218d722"></a>[`public static Inno.Native.UI.InnoUiHandlePtr.implicit operator Inno.Native.UI.InnoUiHandle*(Inno.Native.UI.InnoUiHandlePtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L374) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-eb1a71781d9523da"></a>[`public static bool Inno.Native.UI.InnoUiHandlePtr.operator ==(Inno.Native.UI.InnoUiHandlePtr left, Inno.Native.UI.InnoUiHandlePtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L375) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-bec00a0c35ffcfcb"></a>[`public static bool Inno.Native.UI.InnoUiHandlePtr.operator !=(Inno.Native.UI.InnoUiHandlePtr left, Inno.Native.UI.InnoUiHandlePtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L376) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-37b1e1e7124037e3"></a>[`public bool Inno.Native.UI.InnoUiHandlePtr.Equals(Inno.Native.UI.InnoUiHandlePtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L377) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-853a81a706d15795"></a>[`public override bool Inno.Native.UI.InnoUiHandlePtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L378) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-020cbe75c307ea59"></a>[`public override int Inno.Native.UI.InnoUiHandlePtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L379) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-da720a143b87790a"></a>[`public ref ulong Inno.Native.UI.InnoUiHandlePtr.Value { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L381) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiIndex`

<!-- api: T:Inno.Native.UI.InnoUiIndex 8daabfa5b0530f6f509ce6445c9bb24769b591dfa50f31ca3f496427722c14b3 -->
<!-- api: F:Inno.Native.UI.InnoUiIndex.Value 2b7a59e3bf84d8ae9b7407b23dd57bc1614e8c1f8afdc14b204a03d2ef0feda1 -->
<!-- api: M:Inno.Native.UI.InnoUiIndex.#ctor(System.UInt32) 9a283aa81484718acecee45fc3179022fe2f331c1d33f1327edfffdef35b89fe -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9ec5bd8c549210e9"></a>[`public struct Inno.Native.UI.InnoUiIndex`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L101) | 一个 32-bit mesh 索引的 ABI 包装。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-f96b57dc456d1816"></a>[`public uint Inno.Native.UI.InnoUiIndex.Value`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L103) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-f2c870003f069700"></a>[`public Inno.Native.UI.InnoUiIndex.InnoUiIndex(uint value = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L104) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiIndexPtr`

<!-- api: T:Inno.Native.UI.InnoUiIndexPtr 9bd1cf22231d261f92204844764a7325af190d8fdba1beac9cc792259e8e6a1d -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.#ctor(Inno.Native.UI.InnoUiIndex*) 06e65a6d6ce0685c641afad6e244e0da488cb87e0fb3b1d366342a5551ca5970 -->
<!-- api: F:Inno.Native.UI.InnoUiIndexPtr.Handle 06cfd7e682c8a78c09694e8b1834f245b17180ecdcce9b70505a70379c8003a4 -->
<!-- api: P:Inno.Native.UI.InnoUiIndexPtr.IsNull ac836d9685252a616bb1c664f34dc9f22429acd9cd67858675cdfbf35da8d6dc -->
<!-- api: P:Inno.Native.UI.InnoUiIndexPtr.Null a13197a67153a68b4943cd5fa9536e584b3c32d66a95f821ce1dc70a61ae1a46 -->
<!-- api: P:Inno.Native.UI.InnoUiIndexPtr.Item(System.Int32) d32331ded6c4d88162bb9116bb7b371f955fd991bb557d86016c4676bff04372 -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.op_Implicit(Inno.Native.UI.InnoUiIndex*)~Inno.Native.UI.InnoUiIndexPtr 4c48315a868414ad871f97a19696b0e71a52e1195af3741b300aede962cf344f -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.op_Implicit(Inno.Native.UI.InnoUiIndexPtr)~Inno.Native.UI.InnoUiIndex* 1a3c2bbf23d2880343b5378a4f49b8fc2fb5c6a6de41049c29cd6e7ad0cd5488 -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.op_Equality(Inno.Native.UI.InnoUiIndexPtr,Inno.Native.UI.InnoUiIndexPtr) 35201c866269e2260eae786188af80dcbc0f6d616f982b4e8946cdcb632b41b6 -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.op_Inequality(Inno.Native.UI.InnoUiIndexPtr,Inno.Native.UI.InnoUiIndexPtr) 3f63c7aa964ee1da8fc12f6a2807b6544b22b82e1f4b81ee8bbe7babe7576f46 -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.Equals(Inno.Native.UI.InnoUiIndexPtr) c62dfccfedc395285cce190bd9c0fec0b0e0b491833e1f8735b716eccb5ee292 -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.Equals(System.Object) 678cb1383a1b1cc2c76ae6818b6eb146527821e59b60ad7588ca6c3ccba7fb9a -->
<!-- api: M:Inno.Native.UI.InnoUiIndexPtr.GetHashCode c693b5ab04164b54ddc06af6732fe583ea1ddf7ea29122afc242a7e932a780bd -->
<!-- api: P:Inno.Native.UI.InnoUiIndexPtr.Value c7776dd61e62220ddf0199f83f690b52fa74bb1881c560aa6e4260911210ec52 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3d962a3c9dee5430"></a>[`public struct Inno.Native.UI.InnoUiIndexPtr : System.IEquatable<Inno.Native.UI.InnoUiIndexPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L385) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-654cb7a6a1ab4a30"></a>[`public Inno.Native.UI.InnoUiIndexPtr.InnoUiIndexPtr(Inno.Native.UI.InnoUiIndex* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L387) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-6ca847c6f62e7620"></a>[`public Inno.Native.UI.InnoUiIndex* Inno.Native.UI.InnoUiIndexPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L392) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-4ab0f04e81e2992a"></a>[`public bool Inno.Native.UI.InnoUiIndexPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L393) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-8aabc51fc225287c"></a>[`public static Inno.Native.UI.InnoUiIndexPtr Inno.Native.UI.InnoUiIndexPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L394) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-bc644a7c058f94c3"></a>[`public Inno.Native.UI.InnoUiIndex Inno.Native.UI.InnoUiIndexPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L396) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-07b74b41471c8f83"></a>[`public static Inno.Native.UI.InnoUiIndexPtr.implicit operator Inno.Native.UI.InnoUiIndexPtr(Inno.Native.UI.InnoUiIndex* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L398) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-41173d7fff481320"></a>[`public static Inno.Native.UI.InnoUiIndexPtr.implicit operator Inno.Native.UI.InnoUiIndex*(Inno.Native.UI.InnoUiIndexPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L399) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-f3ed87166a80b715"></a>[`public static bool Inno.Native.UI.InnoUiIndexPtr.operator ==(Inno.Native.UI.InnoUiIndexPtr left, Inno.Native.UI.InnoUiIndexPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L400) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-5c032475cf15a974"></a>[`public static bool Inno.Native.UI.InnoUiIndexPtr.operator !=(Inno.Native.UI.InnoUiIndexPtr left, Inno.Native.UI.InnoUiIndexPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L401) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-03fc3e0972297497"></a>[`public bool Inno.Native.UI.InnoUiIndexPtr.Equals(Inno.Native.UI.InnoUiIndexPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L402) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-cd83b9aecbee0f12"></a>[`public override bool Inno.Native.UI.InnoUiIndexPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L403) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-4137bf18d1074ab7"></a>[`public override int Inno.Native.UI.InnoUiIndexPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L404) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-9060a9a133a5eed1"></a>[`public ref uint Inno.Native.UI.InnoUiIndexPtr.Value { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L406) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiMeshInfo`

<!-- api: T:Inno.Native.UI.InnoUiMeshInfo a43d8ce23c747f34bce5108ee2a301699e186ce00d35965d011f0476db3a6b6e -->
<!-- api: F:Inno.Native.UI.InnoUiMeshInfo.Id 9e5972d55209d4344662000cc25ad98bbda7d15e9f36ae497e8424c7d0ee6aec -->
<!-- api: F:Inno.Native.UI.InnoUiMeshInfo.Revision 6fb1768f23d473a6b4ba53de6394d63a149e1e2e5a806ac62c3b7182373d2969 -->
<!-- api: F:Inno.Native.UI.InnoUiMeshInfo.VertexCount 60149794e17363fba4ad4b853b3b573d9cb4c00d1c3ad2d90ac35c94a20f8f4c -->
<!-- api: F:Inno.Native.UI.InnoUiMeshInfo.IndexCount 588fffb85dbbf324b60bafb04d9dbcbe0024b4bd910cd1ad367ca04c05b236ae -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfo.#ctor(System.UInt64,System.UInt64,System.UInt64,System.UInt64) 9386ca3eba291f286de2af16a53b6abfbd1ef88f9394912eb68a444f66314313 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f07f8e8ce9a1be2d"></a>[`public struct Inno.Native.UI.InnoUiMeshInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L194) | 一份 mesh 更新的 ID、revision 和长度。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-36aaf2c200b6bf29"></a>[`public ulong Inno.Native.UI.InnoUiMeshInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L196) | 所属 Context 内的资源 ID。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-f107dcea3decc019"></a>[`public ulong Inno.Native.UI.InnoUiMeshInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L197) | 该资源的内容 revision，用于判断是否更新消费缓存。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-3d7b8a2f6a10dee2"></a>[`public ulong Inno.Native.UI.InnoUiMeshInfo.VertexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L198) | 所需 Vertex 元素数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-c9ed3b212f3d61d9"></a>[`public ulong Inno.Native.UI.InnoUiMeshInfo.IndexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L199) | 所需 Index 元素数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-a7fd15f65220027b"></a>[`public Inno.Native.UI.InnoUiMeshInfo.InnoUiMeshInfo(ulong id = 0, ulong revision = 0, ulong vertex_count = 0, ulong index_count = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L200) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiMeshInfoPtr`

<!-- api: T:Inno.Native.UI.InnoUiMeshInfoPtr 48537b4e64831a58ea66c839c77bc0ab225ca5cc1dfb45216dfc8304c811eea6 -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.#ctor(Inno.Native.UI.InnoUiMeshInfo*) 452cc24bd25a2098309cd179f12fecf2279f9a2ef9e94bcfb1341bd1429cff9a -->
<!-- api: F:Inno.Native.UI.InnoUiMeshInfoPtr.Handle 0b3052546f50de8da067e636bbf6603842ff351838cefb00b8accbdda4eb27e7 -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.IsNull 41bac32b0f9329a23747dd21a91b970d15c1a19bf090357b32fbccaa64c4e60d -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.Null ca252a81986e9d77dfe28093716824b04ed5f0fc00b37e1cea7d140763e56838 -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.Item(System.Int32) a710d0ff55da424a37a4012522e277545c55ea30ddc975a440895a436bad91ff -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.op_Implicit(Inno.Native.UI.InnoUiMeshInfo*)~Inno.Native.UI.InnoUiMeshInfoPtr 4bff9497205526fd363d923588a221dedd6658e0e81ff8dc460b8f02ec642f2a -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.op_Implicit(Inno.Native.UI.InnoUiMeshInfoPtr)~Inno.Native.UI.InnoUiMeshInfo* 829b5e01fbc26b89b3c5a33fb9c45e763dd594990851dbb741384207ddbb5f85 -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.op_Equality(Inno.Native.UI.InnoUiMeshInfoPtr,Inno.Native.UI.InnoUiMeshInfoPtr) ae07540c863e1605ebae48f8922086ee6d8caf21e48834683b66c89bd9296e0d -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.op_Inequality(Inno.Native.UI.InnoUiMeshInfoPtr,Inno.Native.UI.InnoUiMeshInfoPtr) 98629400b9a4ef7cc3610ed287c7a9d0ece002e36ef88d3b904a45a7400f4633 -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.Equals(Inno.Native.UI.InnoUiMeshInfoPtr) f20cac3117d0c200880d1e051a750031f2f0abee41fbc7fad81b900b619f5ccc -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.Equals(System.Object) a1ccd7a07c327f56c593a17a52df5c945741bd89346f002a4a184015105269a9 -->
<!-- api: M:Inno.Native.UI.InnoUiMeshInfoPtr.GetHashCode bd0e156d55de566eb60ab7b443e93e64e3fc0095b3bdc73bb27c15a396d99889 -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.Id 3983f52a4a54687ea1bc80143bf0de7e920302efeb3b69e90ed7ac3471dc58d5 -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.Revision 54332749f7b92e37d6ae24cd5deb457b81b399480271f529a5a7ece77f5a365e -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.VertexCount 409f5dc057085b68fce6ef74adef61de284d3278d75c4b498eec5047719353a8 -->
<!-- api: P:Inno.Native.UI.InnoUiMeshInfoPtr.IndexCount 740ee1d7a441b02b93ef7a74573e456a40fa05d4bf4ba4b720b7119b2d2a4c45 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7fec350a66a52394"></a>[`public struct Inno.Native.UI.InnoUiMeshInfoPtr : System.IEquatable<Inno.Native.UI.InnoUiMeshInfoPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L410) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-138f0be4a65cca11"></a>[`public Inno.Native.UI.InnoUiMeshInfoPtr.InnoUiMeshInfoPtr(Inno.Native.UI.InnoUiMeshInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L412) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-b8ede7e0d520f88a"></a>[`public Inno.Native.UI.InnoUiMeshInfo* Inno.Native.UI.InnoUiMeshInfoPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L417) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-f918adf4a2a23671"></a>[`public bool Inno.Native.UI.InnoUiMeshInfoPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L418) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-2fc1d2a7773eb398"></a>[`public static Inno.Native.UI.InnoUiMeshInfoPtr Inno.Native.UI.InnoUiMeshInfoPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L419) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-d0a039c9dc471680"></a>[`public Inno.Native.UI.InnoUiMeshInfo Inno.Native.UI.InnoUiMeshInfoPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L421) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-c29fe8f8c069074c"></a>[`public static Inno.Native.UI.InnoUiMeshInfoPtr.implicit operator Inno.Native.UI.InnoUiMeshInfoPtr(Inno.Native.UI.InnoUiMeshInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L423) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-a66b3439c89594ac"></a>[`public static Inno.Native.UI.InnoUiMeshInfoPtr.implicit operator Inno.Native.UI.InnoUiMeshInfo*(Inno.Native.UI.InnoUiMeshInfoPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L424) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-8219d719ea751235"></a>[`public static bool Inno.Native.UI.InnoUiMeshInfoPtr.operator ==(Inno.Native.UI.InnoUiMeshInfoPtr left, Inno.Native.UI.InnoUiMeshInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L425) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-05e8aef534f6caa1"></a>[`public static bool Inno.Native.UI.InnoUiMeshInfoPtr.operator !=(Inno.Native.UI.InnoUiMeshInfoPtr left, Inno.Native.UI.InnoUiMeshInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L426) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-c23973fec6c444e4"></a>[`public bool Inno.Native.UI.InnoUiMeshInfoPtr.Equals(Inno.Native.UI.InnoUiMeshInfoPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L427) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-17cea5991983a00c"></a>[`public override bool Inno.Native.UI.InnoUiMeshInfoPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L428) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-7d167699df20ed90"></a>[`public override int Inno.Native.UI.InnoUiMeshInfoPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L429) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-b86b9a32bc7c18b1"></a>[`public ref ulong Inno.Native.UI.InnoUiMeshInfoPtr.Id { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L431) | 所属 Context 内的资源 ID。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-a72c5bfc442b6d06"></a>[`public ref ulong Inno.Native.UI.InnoUiMeshInfoPtr.Revision { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L432) | 该资源的内容 revision，用于判断是否更新消费缓存。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-d849c49440783a2f"></a>[`public ref ulong Inno.Native.UI.InnoUiMeshInfoPtr.VertexCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L433) | 所需 Vertex 元素数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-8b35c08476b8131a"></a>[`public ref ulong Inno.Native.UI.InnoUiMeshInfoPtr.IndexCount { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L434) | 所需 Index 元素数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiResult`

<!-- api: T:Inno.Native.UI.InnoUiResult d082a4810579ee0d8a7fffbd0a9a869ef2d2b808537e84995ca1b4a605d0e121 -->
<!-- api: F:Inno.Native.UI.InnoUiResult.UnknownError 5af5cd18a0ae32553cd2c1c438d187348990fea4c5cb27d86971a39713233817 -->
<!-- api: F:Inno.Native.UI.InnoUiResult.Success 4d946421a0853143f3de0312d8d65d5f4d7791c7d87b07cfce62e1caeaf79792 -->
<!-- api: F:Inno.Native.UI.InnoUiResult.InvalidArgument d448fa0b9b7c34d9cbc6ff6af5564f18b73232f44efd310c18f05c8e577642bb -->
<!-- api: F:Inno.Native.UI.InnoUiResult.OutOfMemory a904610a2a28039716d7b453dfeaa87e2c3fa64b0f74adb27a54840e7eaea04c -->
<!-- api: F:Inno.Native.UI.InnoUiResult.InvalidHandle ede17ef97a28ff5dfdd7a1e217b900e44950faf5d37211818ca88e2000c18de3 -->
<!-- api: F:Inno.Native.UI.InnoUiResult.BackendError edd4e81f81c53c9afc39a352d5fca27b3d58fa52f09b52adb7734d77d46bcf4d -->
<!-- api: F:Inno.Native.UI.InnoUiResult.BufferTooSmall c586d90933855e075eabe3159bc3a78e3f4b3abd758d5df3f017caf714a9e156 -->
<!-- api: F:Inno.Native.UI.InnoUiResult.NotFound e2dbe1d1bd574f8d79474b47c7c0d558eb2157e8c9546beb398ad96efbd457de -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-57521ff1cb5e7866"></a>[`public enum Inno.Native.UI.InnoUiResult : int`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L21) | 领域结果；零值 UnknownError 表示桥接异常。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-283b82b9f920e0aa"></a>[`Inno.Native.UI.InnoUiResult.UnknownError = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L23) | C 桥出现异常；读取 GetLastError。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-89ab39a65e340aee"></a>[`Inno.Native.UI.InnoUiResult.Success = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L24) | 操作完成；changed/hit 等输出仍须分别检查。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-8df0db9a17f69aff"></a>[`Inno.Native.UI.InnoUiResult.InvalidArgument = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L25) | 参数、尺寸或缓冲区无效。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-907c275423db1e91"></a>[`Inno.Native.UI.InnoUiResult.OutOfMemory = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L26) | 操作分配失败。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-2a29d730a03112ac"></a>[`Inno.Native.UI.InnoUiResult.InvalidHandle = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L27) | 当前 owner 不包含指定 handle。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-6332bf9551b81c98"></a>[`Inno.Native.UI.InnoUiResult.BackendError = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L28) | 底层操作无法完成。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-c9e2a2ce8d3f42a3"></a>[`Inno.Native.UI.InnoUiResult.BufferTooSmall = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L29) | 输出容量不足。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |
| <a id="api-6b3fd6f2f430fc18"></a>[`Inno.Native.UI.InnoUiResult.NotFound = 7`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L30) | 请求的资源、更新索引或事件索引不存在。 | 纯值，无资源所有权。 | 枚举值见当前声明。 |

### `Inno.Native.UI.InnoUiRuntime`

<!-- api: T:Inno.Native.UI.InnoUiRuntime 79f47a0d7ecec89df090705f95f4c0a1829b861a8fe608e824b2c987cbe12b8f -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.#ctor(System.IntPtr) 607e4c4795d67cf759d0d83f7b4517b5f698f84dd161b11965eaaa24a430b8dd -->
<!-- api: P:Inno.Native.UI.InnoUiRuntime.Handle 0ae3838142b6b3959a6beded0767750cefe07c29a5284c019b8ec0901973a198 -->
<!-- api: P:Inno.Native.UI.InnoUiRuntime.IsNull 3205968a9244329dd39ea24b44f1b9a5835ab6488a3ab9fb5f8dfc7930cd050d -->
<!-- api: P:Inno.Native.UI.InnoUiRuntime.Null 08450d4672e9b1fb5c4977bd71997a5a8cef8ed0cd41bc6c8d3841d9b45381f9 -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.op_Implicit(System.IntPtr)~Inno.Native.UI.InnoUiRuntime d0467a1c1a618ae80419cacd110138fab44b66464abb97bcd4d088da9fe1fb95 -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.op_Equality(Inno.Native.UI.InnoUiRuntime,Inno.Native.UI.InnoUiRuntime) 9f1cad5d68a0723e43933df1ef5ab2505ba13ce8a7745677a8e699db540cde15 -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.op_Inequality(Inno.Native.UI.InnoUiRuntime,Inno.Native.UI.InnoUiRuntime) e9f63f3157dbf61112dee86c12489f57d29043c0ad6f0f9577fae584a667a060 -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.Equals(Inno.Native.UI.InnoUiRuntime) 297e27003f1b3d8cc1e46dfc5ff6e2adc4c16ac1fac20e7a80e53ab0242cdb3b -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.Equals(System.Object) bf224763aef6768607d5d31efe8ba957085d0f64a3a351c3a78ec37673e35407 -->
<!-- api: M:Inno.Native.UI.InnoUiRuntime.GetHashCode 711e8713dea9e2b5805583539bec26c735d7bec411b8683bdff0e3b514fc9f4e -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a982a4019d74530d"></a>[`public readonly struct Inno.Native.UI.InnoUiRuntime : System.IEquatable<Inno.Native.UI.InnoUiRuntime>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L51) | 不透明 Runtime 地址的值包装；复制不转移释放权。 | 原 Runtime owner 保留唯一销毁权。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-ff3e55117c3d0bfe"></a>[`public Inno.Native.UI.InnoUiRuntime.InnoUiRuntime(nint handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L53) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-4b73f36d63fa2273"></a>[`public nint Inno.Native.UI.InnoUiRuntime.Handle { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L58) | 读取包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-ee1db17ec3130368"></a>[`public bool Inno.Native.UI.InnoUiRuntime.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L59) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-b90a0014536c1b9c"></a>[`public static Inno.Native.UI.InnoUiRuntime Inno.Native.UI.InnoUiRuntime.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L60) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-0ec64e746d835371"></a>[`public static Inno.Native.UI.InnoUiRuntime.implicit operator Inno.Native.UI.InnoUiRuntime(nint handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L62) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-27a25bcd4cacd203"></a>[`public static bool Inno.Native.UI.InnoUiRuntime.operator ==(Inno.Native.UI.InnoUiRuntime left, Inno.Native.UI.InnoUiRuntime right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L63) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-d4bdfe59cd5d3598"></a>[`public static bool Inno.Native.UI.InnoUiRuntime.operator !=(Inno.Native.UI.InnoUiRuntime left, Inno.Native.UI.InnoUiRuntime right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L64) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-948462be145b6650"></a>[`public bool Inno.Native.UI.InnoUiRuntime.Equals(Inno.Native.UI.InnoUiRuntime other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L65) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-3696446892394092"></a>[`public override bool Inno.Native.UI.InnoUiRuntime.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L66) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-2f6cbfcf921f779a"></a>[`public override int Inno.Native.UI.InnoUiRuntime.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L67) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |

### `Inno.Native.UI.InnoUiTextureInfo`

<!-- api: T:Inno.Native.UI.InnoUiTextureInfo 4095104ff868a605a003cdae1bd3a5cffe7b512328ba04003b48db9e1f8fa212 -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfo.Id 83a53f77cb9947923be0a4b48aa61dfbf50931d29fe1277643b94c33f192b522 -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfo.Revision 5432f9d99de645c7b9c8e94db9859f2b71010dfb0cf676d5678b1e3285302823 -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfo.Width 646b5f64379a2d0efe0b99ca072bbf0aa460ebe246de0c0b2a3e96c12406465b -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfo.Height 02e266983d9c303fa3508611b57c46e889f12bb523ddd1b314553ffeea68f1f8 -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfo.ByteLength 768d14fa6b1050f0138d2856b58f04a39a44a7b2b03436d6719cecb59dfe3af7 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfo.#ctor(System.UInt64,System.UInt64,System.Int32,System.Int32,System.UInt64) 70a554bac5fb66200f2d455ad77b7df0386cf39aa7bed4c69bb3c979669884bf -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-93d905977b2b40e1"></a>[`public struct Inno.Native.UI.InnoUiTextureInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L213) | 一份 texture 更新的 ID、revision、尺寸和长度。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-aaa4efba45b6e96b"></a>[`public ulong Inno.Native.UI.InnoUiTextureInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L215) | 所属 Context 内的资源 ID。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-c1bacb41f8f4a42e"></a>[`public ulong Inno.Native.UI.InnoUiTextureInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L216) | 该资源的内容 revision，用于判断是否更新消费缓存。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-5ea912fdbbe1ce3a"></a>[`public int Inno.Native.UI.InnoUiTextureInfo.Width`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L217) | 纹理像素宽度。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-daf624a70f09ec61"></a>[`public int Inno.Native.UI.InnoUiTextureInfo.Height`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L218) | 纹理像素高度。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-7d13114827529da2"></a>[`public ulong Inno.Native.UI.InnoUiTextureInfo.ByteLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L219) | 纹理像素缓冲区所需字节数。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-a6af3006456cf147"></a>[`public Inno.Native.UI.InnoUiTextureInfo.InnoUiTextureInfo(ulong id = 0, ulong revision = 0, int width = 0, int height = 0, ulong byte_length = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L220) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiTextureInfoPtr`

<!-- api: T:Inno.Native.UI.InnoUiTextureInfoPtr cb32cece7f48ac033656e5c4073768d140d69dfcaf2055fa62a69a18d7c812f5 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.#ctor(Inno.Native.UI.InnoUiTextureInfo*) b8bf701b8359e11b76074432a2df1a9b013b745d4ab0ebe895631f90c51d1829 -->
<!-- api: F:Inno.Native.UI.InnoUiTextureInfoPtr.Handle c0952529f22ae85e9f7203cb7b173516f843684ce9a611780af2f973996b157a -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.IsNull d86f17acebddc71673b3621f722aaf6ea60deec3f2aeb90d6c94f83905c80a3c -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Null 1bae5e889f5e66a663bea45bd3878e5bfd53a4c32eea69d9909acd33df3ee197 -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Item(System.Int32) 4aeabd480480d716b43bd253b8a7c439123e67f0ca7e21f72366c6a4f135b5a1 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.op_Implicit(Inno.Native.UI.InnoUiTextureInfo*)~Inno.Native.UI.InnoUiTextureInfoPtr 31e9f11acf270eea72af81a8517a51171eb06a1d75e3566572821aec5b908167 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.op_Implicit(Inno.Native.UI.InnoUiTextureInfoPtr)~Inno.Native.UI.InnoUiTextureInfo* a3df4b35f19334af9d7e498f7570a2cdf52d33f116ee6d10aaaf0ee71332bff7 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.op_Equality(Inno.Native.UI.InnoUiTextureInfoPtr,Inno.Native.UI.InnoUiTextureInfoPtr) d4655ebd1fb616ea3cb891b8db88df59a4e5175350cc25b1c8290844f91f23c2 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.op_Inequality(Inno.Native.UI.InnoUiTextureInfoPtr,Inno.Native.UI.InnoUiTextureInfoPtr) e3674bc2ad8b88c50af572f22000fbf384361dda4d6573817d58acb40c5f7ef4 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.Equals(Inno.Native.UI.InnoUiTextureInfoPtr) afd76cc1db105223cf1b9cd3cc8b02ba5eb07aa5efbbab84a2bf4286fce3d0bc -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.Equals(System.Object) fa772754ac4b762ba9a1006af368f0996fa919b1bf8903d67833cc5cdd52a254 -->
<!-- api: M:Inno.Native.UI.InnoUiTextureInfoPtr.GetHashCode 6a3ba91dc32b9ebeeb3d02050ed033427f583553329615fa7029e694c6a36256 -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Id 9793c501d629cbb4cd331c3a4db3b1a9e1660cbfb12df1468ec2c859e6ed71fa -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Revision d60709feef21761c5771c59670979b10773e9cdc0460fa295f872aa190667652 -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Width 8392a54b9b7b45394b4a17afbfd5a31a48a449fff12e86d995cbd86f6efd7a90 -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.Height e4db2b0a8ded04854b199c1ddeb5a07ec54fa37bcfef1738cb5da27505fea051 -->
<!-- api: P:Inno.Native.UI.InnoUiTextureInfoPtr.ByteLength 159154270757b164b845ec9d326f5a9abcf03528bfb4af960f2573bac3e151ab -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f12b25cdc4a6863c"></a>[`public struct Inno.Native.UI.InnoUiTextureInfoPtr : System.IEquatable<Inno.Native.UI.InnoUiTextureInfoPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L438) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-727c1c51a97a6e41"></a>[`public Inno.Native.UI.InnoUiTextureInfoPtr.InnoUiTextureInfoPtr(Inno.Native.UI.InnoUiTextureInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L440) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-fd73a502fa42075e"></a>[`public Inno.Native.UI.InnoUiTextureInfo* Inno.Native.UI.InnoUiTextureInfoPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L445) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-5a6c9f3789e26ff2"></a>[`public bool Inno.Native.UI.InnoUiTextureInfoPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L446) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-0f1b47bb0bca6e44"></a>[`public static Inno.Native.UI.InnoUiTextureInfoPtr Inno.Native.UI.InnoUiTextureInfoPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L447) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-3d4162e78a379bdd"></a>[`public Inno.Native.UI.InnoUiTextureInfo Inno.Native.UI.InnoUiTextureInfoPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L449) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-2b66ecaff2f5543d"></a>[`public static Inno.Native.UI.InnoUiTextureInfoPtr.implicit operator Inno.Native.UI.InnoUiTextureInfoPtr(Inno.Native.UI.InnoUiTextureInfo* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L451) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-e00d9315d4abe626"></a>[`public static Inno.Native.UI.InnoUiTextureInfoPtr.implicit operator Inno.Native.UI.InnoUiTextureInfo*(Inno.Native.UI.InnoUiTextureInfoPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L452) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-93b5346f63fe40ec"></a>[`public static bool Inno.Native.UI.InnoUiTextureInfoPtr.operator ==(Inno.Native.UI.InnoUiTextureInfoPtr left, Inno.Native.UI.InnoUiTextureInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L453) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-5a488c6e7919532e"></a>[`public static bool Inno.Native.UI.InnoUiTextureInfoPtr.operator !=(Inno.Native.UI.InnoUiTextureInfoPtr left, Inno.Native.UI.InnoUiTextureInfoPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L454) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-e565199956e2e7fb"></a>[`public bool Inno.Native.UI.InnoUiTextureInfoPtr.Equals(Inno.Native.UI.InnoUiTextureInfoPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L455) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-b9d6ed824b6d0d0f"></a>[`public override bool Inno.Native.UI.InnoUiTextureInfoPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L456) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-1c8b6dc2b20f28b7"></a>[`public override int Inno.Native.UI.InnoUiTextureInfoPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L457) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-c484e3689b520fc7"></a>[`public ref ulong Inno.Native.UI.InnoUiTextureInfoPtr.Id { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L459) | 所属 Context 内的资源 ID。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-e7dfae687b1dc568"></a>[`public ref ulong Inno.Native.UI.InnoUiTextureInfoPtr.Revision { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L460) | 该资源的内容 revision，用于判断是否更新消费缓存。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-bbe9be68ffb1dd60"></a>[`public ref int Inno.Native.UI.InnoUiTextureInfoPtr.Width { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L461) | 纹理像素宽度。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-1bcd31fed133634c"></a>[`public ref int Inno.Native.UI.InnoUiTextureInfoPtr.Height { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L462) | 纹理像素高度。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-4db51e07c1cdd98e"></a>[`public ref ulong Inno.Native.UI.InnoUiTextureInfoPtr.ByteLength { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L463) | 纹理像素缓冲区所需字节数。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiUtf8CodeUnit`

<!-- api: T:Inno.Native.UI.InnoUiUtf8CodeUnit 5654b5a9fe3a9c5a02a22b171c01f456a5d77b7715034f8482021f7f0d6cd2ad -->
<!-- api: F:Inno.Native.UI.InnoUiUtf8CodeUnit.Value 69828b9c35fd0c99b5e8ac8dfa1a1b463cb3c6553bf7559f394cccc965c08a24 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnit.#ctor(System.Byte) 73a3f336f9a670211bd504766bd1ebe3984de5200d71050374c7cc0e3389611d -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3c06aa6fed24f60b"></a>[`public struct Inno.Native.UI.InnoUiUtf8CodeUnit`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L88) | 一个 UTF-8 代码单元的 ABI 包装，不是 UTF-16 字符。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-ab43d98dff9f315f"></a>[`public byte Inno.Native.UI.InnoUiUtf8CodeUnit.Value`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L90) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-229fe9fb2fda5931"></a>[`public Inno.Native.UI.InnoUiUtf8CodeUnit.InnoUiUtf8CodeUnit(byte value = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L91) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiUtf8CodeUnitPtr`

<!-- api: T:Inno.Native.UI.InnoUiUtf8CodeUnitPtr acd39032bd1bb537dee18fddb00cd3259231e7b208bbf5c22aed283f2f77c436 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.#ctor(Inno.Native.UI.InnoUiUtf8CodeUnit*) 2b841de4605c1355f57ec247931dfd30f770ff700e2297899fd4e8142f483a9a -->
<!-- api: F:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Handle 8a7005b8c7410438329b02e7beaecb6f90760ca8f44f601948e0895797e4db0c -->
<!-- api: P:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.IsNull 89d109c6791b2954fc04f0d6d40ce1feac5a428b70a5b489edf0cfe39c8d1763 -->
<!-- api: P:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Null 47c92951c98ef69971f62d4957ecedb740f77888823240c58b7fa4b5ee1c0b94 -->
<!-- api: P:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Item(System.Int32) 9f0e8cf9786c70d6d34aaa6cc1089ed65532509747177d5a758cad6910c2b9dd -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.op_Implicit(Inno.Native.UI.InnoUiUtf8CodeUnit*)~Inno.Native.UI.InnoUiUtf8CodeUnitPtr 22639296738677d2e7234e22ef9428a08783b75f9979fa0af6387ea1a2307778 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.op_Implicit(Inno.Native.UI.InnoUiUtf8CodeUnitPtr)~Inno.Native.UI.InnoUiUtf8CodeUnit* 247ae1995dfa788299c2eae9ebb074009a6cad56d33b64742b56a2e3c9259da8 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.op_Equality(Inno.Native.UI.InnoUiUtf8CodeUnitPtr,Inno.Native.UI.InnoUiUtf8CodeUnitPtr) 678d0fa8a292b0d4e97ce401f0548ed97ba2dcd08ba56aaa37329f373aa158db -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.op_Inequality(Inno.Native.UI.InnoUiUtf8CodeUnitPtr,Inno.Native.UI.InnoUiUtf8CodeUnitPtr) 3fc44f3f714ff0d32272f64c74d6122af05961f511504e9c10c84802268b3d0d -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Equals(Inno.Native.UI.InnoUiUtf8CodeUnitPtr) 9eec72d983f0b31a0d7af681e9b402c7680b45d6765e7f3f1464bcd3855ec566 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Equals(System.Object) f7bd35f2b82e3c7c1e30821d845544b4f71b2e1061a2fcce05806503f00f5503 -->
<!-- api: M:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.GetHashCode 9576b16b76d668e4945548afaedcdf043131c56a15689c9776036f2ff2ed17f9 -->
<!-- api: P:Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Value 5db1e439d2bd196bd77784eccc4f20180c00a3a1592412571967114793ceabfe -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a033c094a550b5f5"></a>[`public struct Inno.Native.UI.InnoUiUtf8CodeUnitPtr : System.IEquatable<Inno.Native.UI.InnoUiUtf8CodeUnitPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L467) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-a781461f0e045347"></a>[`public Inno.Native.UI.InnoUiUtf8CodeUnitPtr.InnoUiUtf8CodeUnitPtr(Inno.Native.UI.InnoUiUtf8CodeUnit* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L469) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-ec66c03f1e925a4d"></a>[`public Inno.Native.UI.InnoUiUtf8CodeUnit* Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L474) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-f93987507dac7353"></a>[`public bool Inno.Native.UI.InnoUiUtf8CodeUnitPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L475) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-9130e3da0e8ad5cf"></a>[`public static Inno.Native.UI.InnoUiUtf8CodeUnitPtr Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L476) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-d89ad1af5d183ae4"></a>[`public Inno.Native.UI.InnoUiUtf8CodeUnit Inno.Native.UI.InnoUiUtf8CodeUnitPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L478) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-55b745233d5d71e3"></a>[`public static Inno.Native.UI.InnoUiUtf8CodeUnitPtr.implicit operator Inno.Native.UI.InnoUiUtf8CodeUnitPtr(Inno.Native.UI.InnoUiUtf8CodeUnit* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L480) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-453e26b6a3def6fe"></a>[`public static Inno.Native.UI.InnoUiUtf8CodeUnitPtr.implicit operator Inno.Native.UI.InnoUiUtf8CodeUnit*(Inno.Native.UI.InnoUiUtf8CodeUnitPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L481) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-e012b38b2f3b1c1d"></a>[`public static bool Inno.Native.UI.InnoUiUtf8CodeUnitPtr.operator ==(Inno.Native.UI.InnoUiUtf8CodeUnitPtr left, Inno.Native.UI.InnoUiUtf8CodeUnitPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L482) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-2e5c872ecd5caa26"></a>[`public static bool Inno.Native.UI.InnoUiUtf8CodeUnitPtr.operator !=(Inno.Native.UI.InnoUiUtf8CodeUnitPtr left, Inno.Native.UI.InnoUiUtf8CodeUnitPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L483) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-6ec30a40d05c23c3"></a>[`public bool Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Equals(Inno.Native.UI.InnoUiUtf8CodeUnitPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L484) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-98c65826f002bfc4"></a>[`public override bool Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L485) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-17000b1f1705ffe8"></a>[`public override int Inno.Native.UI.InnoUiUtf8CodeUnitPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L486) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-642ac3569d6d9304"></a>[`public ref byte Inno.Native.UI.InnoUiUtf8CodeUnitPtr.Value { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L488) | 当前 ABI 标量；具体含义由所属 Byte、Utf8CodeUnit、Index 或资源 Handle 类型确定。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.InnoUiVertex`

<!-- api: T:Inno.Native.UI.InnoUiVertex d53aff3a12c5bb98b7149f52501b02cab4994f8ca4264a93905d47a19178fe58 -->
<!-- api: F:Inno.Native.UI.InnoUiVertex.X 8cf2391b6a4a5f88d5472d2f0817d10bd38c5118d31963570836424f2a0439c6 -->
<!-- api: F:Inno.Native.UI.InnoUiVertex.Y 9027752c17d2cb4b69a084d15e843fc6fc8e5beb4199ac97af0907d6c4ec5bee -->
<!-- api: F:Inno.Native.UI.InnoUiVertex.U 280a1aefea7234c0db0c618c5f1bcaa86203e6bb6946e7aaea66847f7a22bfb4 -->
<!-- api: F:Inno.Native.UI.InnoUiVertex.V ce6b7899b14479a447a617d006ab30c628e6a68244ec02420e702bb70a683b08 -->
<!-- api: F:Inno.Native.UI.InnoUiVertex.Color 991ce5c8c6a6195d91464884f916fae6380ebfdbcfa6832c22f32349a89ff410 -->
<!-- api: M:Inno.Native.UI.InnoUiVertex.#ctor(System.Single,System.Single,System.Single,System.Single,System.UInt32) 743eac580aeee99d660f8276b0fbe09c9c65141bcd6c7606ed5e515281757ba2 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d65fa515335dc4db"></a>[`public struct Inno.Native.UI.InnoUiVertex`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L127) | 预乘 UI 顶点值。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-40b41d3fe239c85a"></a>[`public float Inno.Native.UI.InnoUiVertex.X`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L129) | 顶点横坐标。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-42a546f46c5fc93a"></a>[`public float Inno.Native.UI.InnoUiVertex.Y`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L130) | 顶点纵坐标。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-e59836f7d01251ea"></a>[`public float Inno.Native.UI.InnoUiVertex.U`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L131) | 纹理横向坐标。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-1301bd567b36c8db"></a>[`public float Inno.Native.UI.InnoUiVertex.V`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L132) | 纹理纵向坐标。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-558ba59239b5954c"></a>[`public uint Inno.Native.UI.InnoUiVertex.Color`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L133) | 预乘 alpha 的打包顶点颜色。 | 字段属于当前值副本。 | 只解释成功操作产出的数据。 |
| <a id="api-3a3f288785994bcf"></a>[`public Inno.Native.UI.InnoUiVertex.InnoUiVertex(float x = 0, float y = 0, float u = 0, float v = 0, uint color = 0)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L134) | 按声明参数初始化 ABI 值字段。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |

### `Inno.Native.UI.InnoUiVertexPtr`

<!-- api: T:Inno.Native.UI.InnoUiVertexPtr d71684843092783443850a73a846bc63fbff791caa8dcca7c62ed5aa7069a5be -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.#ctor(Inno.Native.UI.InnoUiVertex*) c197e17f6942854482215eec995c8fae6042ee51b197e61d969e74130df7bff3 -->
<!-- api: F:Inno.Native.UI.InnoUiVertexPtr.Handle 635210c2117b3bb8b656aafbd10cd0bf35b6dc488dd5f2e40add893e98f1babd -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.IsNull 0793ddc99f72cae7a0f9a6efefeae26ccb14d0238dd0772934f0213df53a55ea -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.Null fea2ea9d402a5a8da01efc6a9f5b694644085e54739b15fb5934ed7c34377eea -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.Item(System.Int32) 56c7b9a84fbe4c30dc401625548cfbf547da75f753ca55e13b8bb6a8eb57a511 -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.op_Implicit(Inno.Native.UI.InnoUiVertex*)~Inno.Native.UI.InnoUiVertexPtr 786908712712b1e5c1f842020c0a05eb161925d80e28a22d7b837513000bbdbe -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.op_Implicit(Inno.Native.UI.InnoUiVertexPtr)~Inno.Native.UI.InnoUiVertex* f218d7d57c26f9b407972f11a5f98456b48e937c218243f09b794166a753dc77 -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.op_Equality(Inno.Native.UI.InnoUiVertexPtr,Inno.Native.UI.InnoUiVertexPtr) f10c6a7a3548f0a2d8ef183b2420699adbba4a3b1d5abbd81264ea48928f138a -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.op_Inequality(Inno.Native.UI.InnoUiVertexPtr,Inno.Native.UI.InnoUiVertexPtr) 9ebe92907a9f68a21c9770922b7714b3f013ce3a72407ea2ecfa26ed1ffc464d -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.Equals(Inno.Native.UI.InnoUiVertexPtr) bce91d658973d65c00e47142615ec137bcf81fb7fd26ec076abd82faf8093342 -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.Equals(System.Object) 914ff12aa9e99ae1c7c44bfeafaeae189dd54b989940368a420f6cd4a921b7ca -->
<!-- api: M:Inno.Native.UI.InnoUiVertexPtr.GetHashCode 5e29afec796b9b07df8a185655d93f719b66f7681ecd0f3bc5883d363694bd38 -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.X ea9b39ee400748cf4050cf2958fbf640a5701853f56ab6221517be18c0cd2e96 -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.Y a5f2c0a962f09135dcb9cb7090a870e052d55c21423ce901e810025a0204c963 -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.U e611c68a702c9ed144501d51633039e85f084ccb88adc3e5fee7f54db389ba17 -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.V 4747b71ba92af4df52624f972eba93dad2a9aaf9170021f6aa46ee4dee5a87ce -->
<!-- api: P:Inno.Native.UI.InnoUiVertexPtr.Color 24fe93b51ae4c5018ee7d8ae3932e3a74090f871d77b2d1ecb277644599d63b3 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1561dcb5cc704d15"></a>[`public struct Inno.Native.UI.InnoUiVertexPtr : System.IEquatable<Inno.Native.UI.InnoUiVertexPtr>`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L492) | 对应 ABI 数据的借用指针包装。 | 不分配、固定或释放指向的内存。 | ref 属性和索引不检查容量与生命周期。 |
| <a id="api-189e35b0772d10b5"></a>[`public Inno.Native.UI.InnoUiVertexPtr.InnoUiVertexPtr(Inno.Native.UI.InnoUiVertex* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L494) | 只包装原生地址值，不创建目标。 | 复制不增加原生释放权。 | 默认值不证明某次操作成功。 |
| <a id="api-85d582a132db4eeb"></a>[`public Inno.Native.UI.InnoUiVertex* Inno.Native.UI.InnoUiVertexPtr.Handle`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L499) | 读取或替换包装的原生地址。 | 不延长原内存寿命，不释放旧地址。 | 不检查地址是否悬空。 |
| <a id="api-48b39e29fe26de82"></a>[`public bool Inno.Native.UI.InnoUiVertexPtr.IsNull { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L500) | 仅比较地址是否为零。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-d884df4d426a20d8"></a>[`public static Inno.Native.UI.InnoUiVertexPtr Inno.Native.UI.InnoUiVertexPtr.Null { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L501) | 返回零地址包装。 | 不访问或分配目标。 | 非零不等于对象仍存活。 |
| <a id="api-c125abea12b7ff81"></a>[`public Inno.Native.UI.InnoUiVertex Inno.Native.UI.InnoUiVertexPtr.this[int index] { get; set; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L503) | 按数组 index 读写完整 ABI 值。 | 调用方保持连续数组和索引有效。 | 无边界检查或自动固定。 |
| <a id="api-f4ed645106da375a"></a>[`public static Inno.Native.UI.InnoUiVertexPtr.implicit operator Inno.Native.UI.InnoUiVertexPtr(Inno.Native.UI.InnoUiVertex* handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L505) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-d702f6e4db9f99e7"></a>[`public static Inno.Native.UI.InnoUiVertexPtr.implicit operator Inno.Native.UI.InnoUiVertex*(Inno.Native.UI.InnoUiVertexPtr handle)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L506) | 转换原生指针与其 typed wrapper。 | 仅复制地址，不转移 owner。 | 不验证目标类型、容量或存活状态。 |
| <a id="api-95a89a6f84aad970"></a>[`public static bool Inno.Native.UI.InnoUiVertexPtr.operator ==(Inno.Native.UI.InnoUiVertexPtr left, Inno.Native.UI.InnoUiVertexPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L507) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-10875d91e3318696"></a>[`public static bool Inno.Native.UI.InnoUiVertexPtr.operator !=(Inno.Native.UI.InnoUiVertexPtr left, Inno.Native.UI.InnoUiVertexPtr right)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L508) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-1fe5f12440c7f5cc"></a>[`public bool Inno.Native.UI.InnoUiVertexPtr.Equals(Inno.Native.UI.InnoUiVertexPtr other)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L509) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-193b74c6ad58b17e"></a>[`public override bool Inno.Native.UI.InnoUiVertexPtr.Equals(object? obj)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L510) | 按地址值判断相等或不等。 | 不解引用原生目标。 | 相等不保证当前对象仍存活。 |
| <a id="api-6b5a88f43658fc16"></a>[`public override int Inno.Native.UI.InnoUiVertexPtr.GetHashCode()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L511) | 计算地址值的哈希。 | 不解引用原生目标。 | 不是持久 ID 或跨 generation 定位协议。 |
| <a id="api-669868fec848a6c9"></a>[`public ref float Inno.Native.UI.InnoUiVertexPtr.X { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L513) | 顶点横坐标。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-d9ad305d200ceeed"></a>[`public ref float Inno.Native.UI.InnoUiVertexPtr.Y { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L514) | 顶点纵坐标。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-f5e209621b1907cb"></a>[`public ref float Inno.Native.UI.InnoUiVertexPtr.U { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L515) | 纹理横向坐标。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-ed23056050fbc152"></a>[`public ref float Inno.Native.UI.InnoUiVertexPtr.V { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L516) | 纹理纵向坐标。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |
| <a id="api-e6ab807af35c2737"></a>[`public ref uint Inno.Native.UI.InnoUiVertexPtr.Color { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L517) | 预乘 alpha 的打包顶点颜色。 | 可写 ref 指向借用的原生字段。 | 不能在内存释放或移动后保留该 ref。 |

### `Inno.Native.UI.UiNative`

<!-- api: T:Inno.Native.UI.UiNative 059837a6d7a0fd2d5b2ed85dbbb07f414a4130d6dffc3f4fabc1b15b4a6fe815 -->
<!-- api: M:Inno.Native.UI.UiNative.GetLastError 838c16d891adc20343dfab7a99b8abfcbf082aec8bc3667aca6a4c742f11e130 -->
<!-- api: M:Inno.Native.UI.UiNative.ClearLastError 9cf6c062a1928a0cb90ce609ba830f350892192b26028e5b1c1a8a993b02fade -->
<!-- api: M:Inno.Native.UI.UiNative.Create fee9aad85963d977d11d78f1a501cca6bc5d3a76d014ef6a1b2ba63621ba1f20 -->
<!-- api: M:Inno.Native.UI.UiNative.Destroy(Inno.Native.UI.InnoUiRuntime) 1bb1cc8f9f246ef233477f647fefecb7ca8d011d0ead3a45d5bee76a39e9a640 -->
<!-- api: M:Inno.Native.UI.UiNative.CreateContext(Inno.Native.UI.InnoUiRuntime,System.Byte*,System.Int32,System.Int32,System.Single,System.UInt64*) 4984fff4b9de46a23dbbe8e0fa826d80178a15fca88197e9b7dc393c9aeee8cf -->
<!-- api: M:Inno.Native.UI.UiNative.CreateContext(Inno.Native.UI.InnoUiRuntime,System.String,System.Int32,System.Int32,System.Single,System.UInt64@) 6781ae38fe7f89c67533f1ec4c848981eb1565b74831917170914457892314ba -->
<!-- api: M:Inno.Native.UI.UiNative.DestroyContext(Inno.Native.UI.InnoUiRuntime,System.UInt64) 99f15671c289f5079305696802bf5b9951eee2edd4791a19a708f1e672cf9d46 -->
<!-- api: M:Inno.Native.UI.UiNative.SetViewport(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Int32,System.Single) 6c8867c4789ed5f07aded664bed2b288edcabef63813df974856d88224276b08 -->
<!-- api: M:Inno.Native.UI.UiNative.LoadDocument(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Byte*,System.Byte*,System.UInt64*) 89d115d1653ebeb09424393b1ad75b13732d3ec649eb3f0987bc7f73bc690799 -->
<!-- api: M:Inno.Native.UI.UiNative.LoadDocument(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.String,System.String,System.UInt64@) 3e55f899f1073c8de47fc0d80a4782034077ed76f5fafbadcc62808726072763 -->
<!-- api: M:Inno.Native.UI.UiNative.ShowDocument(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64) 418e4d376330a6fc5b1b4afc860f9dc36897d639886e172cd90c0d078a06db00 -->
<!-- api: M:Inno.Native.UI.UiNative.HideDocument(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64) e9af6f02b475cd52a7c8c7684c17b0ed52aac51dd7e9756283dc3af9663402cf -->
<!-- api: M:Inno.Native.UI.UiNative.CloseDocument(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64) e4ab301a82e2ca19797e093a5f7e6d5c5b3fd0f58ae2a8980dabc6ebe0eaf209 -->
<!-- api: M:Inno.Native.UI.UiNative.SetInnerMarkup(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Byte*,System.Byte*,System.Byte*) e35b3503b004da9e4f4d20188de5781560177779f834b967b3baeaac5b6c8318 -->
<!-- api: M:Inno.Native.UI.UiNative.SetInnerMarkup(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.String,System.String,System.Byte@) 5799c7ed78bf0eae0201f947c978e291c66b3ec70d88a1f3bff7567abf98321a -->
<!-- api: M:Inno.Native.UI.UiNative.SetAttribute(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Byte*,System.Byte*,System.Byte*,System.Byte*) 7ec73e6aebcd3e8a42f2eee49d59372ad352cbb1471f560dae58ed1024aa8400 -->
<!-- api: M:Inno.Native.UI.UiNative.SetAttribute(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.String,System.String,System.String,System.Byte@) cd779a82cd95eb94bd300b04a143f6fa3f67e79e163d7a7db8473e8727ca6c1c -->
<!-- api: M:Inno.Native.UI.UiNative.SetClass(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Byte*,System.Byte*,System.Byte,System.Byte*) a0aa1c43260d16fac3a71dd53659d4cd7b0f59d92ef6568307977cc9ce3cf140 -->
<!-- api: M:Inno.Native.UI.UiNative.SetClass(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.String,System.String,System.Byte,System.Byte@) 9ea4ea734ae905525242cc7da8dbf50f3ee84c82a10970be880a20c808bcc7d5 -->
<!-- api: M:Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime,Inno.Native.UI.InnoUiBytePtr,System.UInt64,System.Byte*,System.Int32,System.Int32,System.Byte) c2f23f2332ca33a163766eb7f42715a004cde30b94e090514cf706a7ef26d927 -->
<!-- api: M:Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime,Inno.Native.UI.InnoUiByte@,System.UInt64,System.Byte*,System.Int32,System.Int32,System.Byte) d523fbbbc9c5fcc3dd1dca278cf1f91ad7de75fde7a62b484f286ad9ca86e2dc -->
<!-- api: M:Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime,System.Span{Inno.Native.UI.InnoUiByte},System.String,System.Int32,System.Int32,System.Byte) 7a38c17b9b8964e3d069cf4ff54d2b2f4b2f1ebcb64d8ab9685291bd6986551d -->
<!-- api: M:Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Byte*,System.Int32,System.Int32,Inno.Native.UI.InnoUiBytePtr,System.UInt64) 42a17cb51c1da3cd535af8a818fef4c0281f466ac59945a54768102447481edf -->
<!-- api: M:Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Byte*,System.Int32,System.Int32,Inno.Native.UI.InnoUiByte@,System.UInt64) edd7e6669afd0a7270e7de14cd3238f750cb19237e24088d4b667ef7d79546e7 -->
<!-- api: M:Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.String,System.Int32,System.Int32,System.Span{Inno.Native.UI.InnoUiByte}) ff39272adb78b92e3dfa25a872ff59ebbcee5e7fe5d63f42f93a912f76d73e8c -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessMouseMove(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Int32,System.Int32) 88d7fd6227a7dc86425c00e4ff7ca0c0d4303ede687fe33687ec520390f5f30b -->
<!-- api: M:Inno.Native.UI.UiNative.HasElementAtPoint(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Int32,System.Byte*) 80941a30fe4db3792c98c48cbccc70c312df5806af3e0f890c9f6a2a7a4c999f -->
<!-- api: M:Inno.Native.UI.UiNative.HasElementAtPoint(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Int32,System.Byte@) 5baf7115dce985cf8b02530d42b6367d351ef0ec03ec777944223cd161ceb28e -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessMouseButton(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Byte,System.Int32) ae98d7fa423614ac7a9bbdeb48134d1813cdf5819580ceeeb9c16f5e411bc5f4 -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessMouseWheel(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Single,System.Single,System.Int32) cf0cc1fc834da362340a240307372da5a4c51283fe50124c989f0336bacb8b9b -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessKey(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Int32,System.Byte,System.Int32) 44b338ce87ff5fd891d3f17f3d42a5f8bda57b7efdac0b3a43216775074e9087 -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessText(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Byte*) 0ea9765494180182c4349d867485f395c404551ee599309bccb20fdf4f2b485a -->
<!-- api: M:Inno.Native.UI.UiNative.ProcessText(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.String) 95b9dedee08a0db44aff7a93cc93e723ac54a868290495ba173bf91a68397812 -->
<!-- api: M:Inno.Native.UI.UiNative.Update(Inno.Native.UI.InnoUiRuntime,System.UInt64) 87d7282a2eec96e793b480358d790cd37f7ed0a2a4b3d7eb5b45452315e89c28 -->
<!-- api: M:Inno.Native.UI.UiNative.Render(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiFrameInfoPtr) e8065cc571aff6f4a6a7a43fa0b95959803a8bc44ea649b84bbc20a5f14e0145 -->
<!-- api: M:Inno.Native.UI.UiNative.Render(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiFrameInfo@) 12cd4e1149fd9b4cb1b2d949c5229a81c0b0b49053674e6a1e3c94273e93064b -->
<!-- api: M:Inno.Native.UI.UiNative.GetMeshInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiMeshInfoPtr) d26743c6024d7981eb68fc509dc4164768857a70737c7c86e39c023db3a12480 -->
<!-- api: M:Inno.Native.UI.UiNative.GetMeshInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiMeshInfo@) 175a9056229541f2ec269c2e6c199c58b6cceb30b4a6e98b6175c3ae3ddc9e03 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiVertexPtr,System.UInt64) 5374af2289e9151db12055b38764c31999ddaf4852a9f3ea13dc46300df57c26 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiVertex@,System.UInt64) a520e3ccf447f6d02f7b8965bde6272d831a9cbc5c3c160bb00422e5f3784325 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Span{Inno.Native.UI.InnoUiVertex}) 6d5dccff1bdd8343743161f71dba2859c7058bd2bf11ecbdb2177a0d1e6a244a -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiIndexPtr,System.UInt64) 1d089be18972ea2ca30abe1868c5b50ca0df7602f67541961951b8ad6ea0b353 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiIndex@,System.UInt64) cd5ef0c3d65a3dec7f2ae5245dacfbb49640c3bd6c72747e8b093f0f76a82e88 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Span{Inno.Native.UI.InnoUiIndex}) d02dbe3e5cfa67713d5418236ef561358eaa452dd632307f6a3e35590af4b979 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiDrawCommandPtr,System.UInt64) e5912113e21b5628a86c35b8a7848a3c9f3e1a06328dc3b38f286b1b2f82220c -->
<!-- api: M:Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiDrawCommand@,System.UInt64) 61b13b99bf33229d93eb71dcd654fefd3f8fef7162a1ca17651e4a9339006da1 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Span{Inno.Native.UI.InnoUiDrawCommand}) b001ff1068015d043d559683405a29faf3a1f34f2ee3f5b25febe4a10c5f0b09 -->
<!-- api: M:Inno.Native.UI.UiNative.GetTextureInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiTextureInfoPtr) e14f1c09325afb0c7eabfe0dd3174b6b650bd97aca9dcdeec92511f07ac1aa44 -->
<!-- api: M:Inno.Native.UI.UiNative.GetTextureInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiTextureInfo@) 4967378c3056cec35af49c29c4e745e87d0a81a67f6125cd7485e7b7217e3bba -->
<!-- api: M:Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiBytePtr,System.UInt64) 8be67f96882bd7e0df7e1bf33a412a9fc13392a2b7234930a4e84721e1c7e1cd -->
<!-- api: M:Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiByte@,System.UInt64) 819678747c23a25a251e44815233bde26f8b7acff3425aa3594646e34d97f107 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Span{Inno.Native.UI.InnoUiByte}) 79ad3a5e6a7ce4f1ae289a17f95cabaaa75e41b51a8534867d43d33be430a969 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiHandlePtr,System.UInt64) 28cac6b08ac3333a513243dc00d5477b8c5bad8909796b54fe7058b6bcba099f -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiHandle@,System.UInt64) e4b3e4acce0bd571dc155b3af8c3d4bb773a78104aead379451db5e1f83dcd1e -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Span{Inno.Native.UI.InnoUiHandle}) c370309619b47d36af0d978a055e6e64b35a52560b24921b0bbffbef3becbeff -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiHandlePtr,System.UInt64) a0b32cdffe62dbfeb98889a2ddcd46a754ffc1a3a85ac41a9c84410cce4b4935 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime,System.UInt64,Inno.Native.UI.InnoUiHandle@,System.UInt64) 26e5671ab0f938d12488b380aa24b44a85f79141130bd71557c52df9a97c96fb -->
<!-- api: M:Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.Span{Inno.Native.UI.InnoUiHandle}) 45210c12ca2b5311baefa6ba2cb19589550e5c9cd364da486e381977afdaa719 -->
<!-- api: M:Inno.Native.UI.UiNative.FinishFrame(Inno.Native.UI.InnoUiRuntime,System.UInt64) bd2871dbb30d1a6d731c7547b6dd79e62a4f1b9b2795554a40caee9ba0fd56b2 -->
<!-- api: M:Inno.Native.UI.UiNative.GetEventCount(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64*) 1ec962bc59057ef33b709eebf6a3ae602c520423990590ab49e12733e0eefe80 -->
<!-- api: M:Inno.Native.UI.UiNative.GetEventCount(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64@) a333a2d7f94401978f368ada3d6b14a6585c92ef25ce962db2227b2ba401611a -->
<!-- api: M:Inno.Native.UI.UiNative.GetEventInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiEventInfoPtr) 358852221eeca5cd65fb5349d4e215627ceb85dda9991ae209855769a82e07b7 -->
<!-- api: M:Inno.Native.UI.UiNative.GetEventInfo(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiEventInfo@) 3f964e1781f1eeca384da58d961e3cab0dc963d9d97a34500caa1b6c42291085 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiUtf8CodeUnitPtr,System.UInt64) 46a299f4b5d76896ff22007063347409a4b2f37e973a3ae36ac25b0bea778253 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,Inno.Native.UI.InnoUiUtf8CodeUnit@,System.UInt64) 14eb32abc4f6c76cc3163c717a776c2ae087dee653b8036fe340d2508f436006 -->
<!-- api: M:Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime,System.UInt64,System.UInt64,System.Span{Inno.Native.UI.InnoUiUtf8CodeUnit}) ee9abae05560325c6a306cb08bbce5987fe93d3d04e54d91bbcf68045014f29e -->
<!-- api: M:Inno.Native.UI.UiNative.ClearEvents(Inno.Native.UI.InnoUiRuntime,System.UInt64) 02af325778818010689cd5c163a92c8e49972e843fa584c8ef077c38b9087fa7 -->
<!-- api: M:Inno.Native.UI.UiNative.InitApi(BGCS.Runtime.INativeContext) 480cab83e57ff9850b122a27c98cf6e9d74390b289637d69ca4cc0712f48173f -->
<!-- api: M:Inno.Native.UI.UiNative.FreeApi 8b66cbb29d2352c8ce884699d78fe8fecea7fb079ab11cf351fbd10be0c46631 -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-425b7a996d4e627e"></a>[`public static class Inno.Native.UI.UiNative`](../../../backends/RmlUi/native/Inno.Native.UI/UiNative.cs#L10) | UI facade 的静态 managed 入口。 | 值结构/枚举不接管资源；静态入口遵守下列 owner 契约。 | Native SPI，不向游戏脚本导出。 |
| <a id="api-0c2973862014176e"></a>[`public static string? Inno.Native.UI.UiNative.GetLastError()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L997) | 复制当前线程的桥接错误为 managed string。 | 返回字符串由 GC 管理，不转移原生文本释放权。 | 没有错误时为空文本；原生空指针封送为 null。 |
| <a id="api-e5f53ddcc2d6b951"></a>[`public static void Inno.Native.UI.UiNative.ClearLastError()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1004) | 清除当前线程桥接错误。 | 不释放 Runtime、Context 或文档。 | 不改变此前领域操作的结果。 |
| <a id="api-3874b204a59e63b4"></a>[`public static Inno.Native.UI.InnoUiRuntime Inno.Native.UI.UiNative.Create()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1010) | 创建一个 Runtime 并取得进程级 UI 宿主引用。 | 调用方成为唯一 Runtime owner；在创建线程销毁。 | 其他线程已拥有进程宿主或初始化失败时返回空 handle，读取桥接错误。 |
| <a id="api-10ce7ac44424dc69"></a>[`public static void Inno.Native.UI.UiNative.Destroy(Inno.Native.UI.InnoUiRuntime self)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1017) | 销毁 Runtime 的全部 Context，最后释放进程宿主引用。 | 停止使用其文档、帧和事件后调用一次。 | 不再使用任何 handle 副本；失效非零指针或重复销毁没有安全保证。 |
| <a id="api-85b88f2f2da0c21c"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CreateContext(Inno.Native.UI.InnoUiRuntime self, byte* name, int width, int height, float density, ulong* context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1023) | 创建指定名称、尺寸与 density 的 Context；成功写入非零 ID。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 名称须非空，尺寸须正数，density 须有限且大于零；参数无效或底层创建失败显式返回结果。 |
| <a id="api-a5f702198f194805"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CreateContext(Inno.Native.UI.InnoUiRuntime self, string name, int width, int height, float density, ref ulong context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1030) | 创建指定名称、尺寸与 density 的 Context；成功写入非零 ID。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字符串临时转为 UTF-8，调用结束释放临时内存。 | 名称须非空，尺寸须正数，density 须有限且大于零；参数无效或底层创建失败显式返回结果。 |
| <a id="api-6e9f644190388a13"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.DestroyContext(Inno.Native.UI.InnoUiRuntime self, ulong context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1049) | 注销事件并销毁 Context、文档和资源；保留内部 renderer 壳直至进程宿主关闭。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 零 ID 返回 InvalidArgument，不属于该 Runtime 的 ID 返回 InvalidHandle。 |
| <a id="api-fec790c114e71f65"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetViewport(Inno.Native.UI.InnoUiRuntime self, ulong context, int width, int height, float density)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1056) | 更新 Context 尺寸和独立像素比例。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle；尺寸非正或 density 无效返回 InvalidArgument。 |
| <a id="api-20b74b461ecb08ae"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.LoadDocument(Inno.Native.UI.InnoUiRuntime self, ulong context, byte* markup, byte* sourceUrl, ulong* document)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1063) | 从 markup 与 source URL 创建文档；仅根布局仍为 auto 的轴补为 100%。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 文档由 Context 拥有；成功写入 document ID。 | 失效 Context、null 文本及底层加载失败分别报告；不覆盖显式 CSS 宽高。 |
| <a id="api-f358fa5d0419eef8"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.LoadDocument(Inno.Native.UI.InnoUiRuntime self, ulong context, string markup, string sourceUrl, ref ulong document)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1070) | 从 markup 与 source URL 创建文档；仅根布局仍为 auto 的轴补为 100%。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 文档由 Context 拥有；成功写入 document ID。 字符串临时转为 UTF-8，调用结束释放临时内存。 | 失效 Context、null 文本及底层加载失败分别报告；不覆盖显式 CSS 宽高。 |
| <a id="api-70bfb760e3e7040c"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ShowDocument(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1093) | 显示所属 Context 的文档。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | Context/document 不匹配返回 InvalidHandle。 |
| <a id="api-e9042380a48d8be9"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.HideDocument(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1100) | 隐藏文档但保留文档及其 ID。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | Context/document 不匹配返回 InvalidHandle。 |
| <a id="api-d9b6365d3cad4854"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CloseDocument(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1107) | 移除文档 ID 映射并关闭文档。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 关闭后不得继续使用该 document ID。 | 失效文档返回 InvalidHandle；不是可重复成功的释放入口。 |
| <a id="api-099deb6a0d61f058"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetInnerMarkup(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, byte* elementId, byte* markup, byte* changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1114) | 找到 element ID 时设置其内部 markup 并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | markup 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。changed 不表示新旧值比较。 |
| <a id="api-601143a85d94fd69"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetInnerMarkup(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, string elementId, string markup, ref byte changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1121) | 找到 element ID 时设置其内部 markup 并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字符串临时转为 UTF-8，调用结束释放临时内存。 | markup 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。changed 不表示新旧值比较。 |
| <a id="api-00a32a8acc5166ae"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetAttribute(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, byte* elementId, byte* name, byte* value, byte* changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1144) | 找到 element ID 时设置属性并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | name/value 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。 |
| <a id="api-0cc974ce1883d695"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetAttribute(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, string elementId, string name, string value, ref byte changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1151) | 找到 element ID 时设置属性并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字符串临时转为 UTF-8，调用结束释放临时内存。 | name/value 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。 |
| <a id="api-27838d96f535a527"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetClass(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, byte* elementId, byte* className, byte active, byte* changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1178) | 找到 element ID 时按 active 是否非零切换 class 并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | className 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。 |
| <a id="api-6e8cf2036e1eb5ee"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.SetClass(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong document, string elementId, string className, byte active, ref byte changed)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1185) | 找到 element ID 时按 active 是否非零切换 class 并写 changed=1。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字符串临时转为 UTF-8，调用结束释放临时内存。 | className 为 null 返回 InvalidArgument；没有匹配元素则 Success 且 changed=0。 |
| <a id="api-e25b1330e615f123"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime self, Inno.Native.UI.InnoUiBytePtr data, ulong dataCount, byte* family, int style, int weight, byte fallback)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1208) | 复制字体输入并向进程级字体引擎注册 family、style、weight 和 fallback。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字体内存由进程宿主保留至最终关闭。 | 数据和 family 非空，style 为 0..2，weight 为 1..1000；注册失败返回结果，不返回独立 font handle。 |
| <a id="api-017981ed9430856b"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime self, ref Inno.Native.UI.InnoUiByte data, ulong dataCount, byte* family, int style, int weight, byte fallback)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1215) | 复制字体输入并向进程级字体引擎注册 family、style、weight 和 fallback。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字体内存由进程宿主保留至最终关闭。 | 数据和 family 非空，style 为 0..2，weight 为 1..1000；注册失败返回结果，不返回独立 font handle。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-276a4607b6dadbc8"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.LoadFont(Inno.Native.UI.InnoUiRuntime self, System.Span<Inno.Native.UI.InnoUiByte> data, string family, int style, int weight, byte fallback)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1225) | 复制字体输入并向进程级字体引擎注册 family、style、weight 和 fallback。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字体内存由进程宿主保留至最终关闭。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 字符串临时转为 UTF-8，调用结束释放临时内存。 | 数据和 family 非空，style 为 0..2，weight 为 1..1000；注册失败返回结果，不返回独立 font handle。 |
| <a id="api-80c9d1212a067f8a"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime self, ulong context, byte* source, int width, int height, Inno.Native.UI.InnoUiBytePtr pixels, ulong pixelsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1244) | 复制指定 source 的直通 RGBA 字节并转为预乘 alpha，供 Context 解析命名纹理。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 输入仅在调用期间借用；无需保留原像素数组。 | source 非空，尺寸正数，输入长度必须恰为 width×height×4；失效 Context 或无效输入失败。 |
| <a id="api-e2e0eb977c1d3e1c"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime self, ulong context, byte* source, int width, int height, ref Inno.Native.UI.InnoUiByte pixels, ulong pixelsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1251) | 复制指定 source 的直通 RGBA 字节并转为预乘 alpha，供 Context 解析命名纹理。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 输入仅在调用期间借用；无需保留原像素数组。 | source 非空，尺寸正数，输入长度必须恰为 width×height×4；失效 Context 或无效输入失败。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-c9353ef2f390ea5b"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.RegisterTexture(Inno.Native.UI.InnoUiRuntime self, ulong context, string source, int width, int height, System.Span<Inno.Native.UI.InnoUiByte> pixels)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1261) | 复制指定 source 的直通 RGBA 字节并转为预乘 alpha，供 Context 解析命名纹理。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 输入仅在调用期间借用；无需保留原像素数组。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 字符串临时转为 UTF-8，调用结束释放临时内存。 | source 非空，尺寸正数，输入长度必须恰为 width×height×4；失效 Context 或无效输入失败。 |
| <a id="api-e54e8a680509d77f"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessMouseMove(Inno.Native.UI.InnoUiRuntime self, ulong context, int x, int y, int modifiers)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1280) | 将 Context 坐标和 modifier 状态传入 UI 鼠标移动处理。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle；Success 不等于输入被消费。 |
| <a id="api-c1cb0dae26f9f88d"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.HasElementAtPoint(Inno.Native.UI.InnoUiRuntime self, ulong context, int x, int y, byte* hit)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1287) | 命中实际交互元素时写 hit=1；根、body、rml 和文档本体不算命中。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 仅 Success 时读取 hit；失效 Context 返回 InvalidHandle。 |
| <a id="api-948d3794100bb300"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.HasElementAtPoint(Inno.Native.UI.InnoUiRuntime self, ulong context, int x, int y, ref byte hit)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1294) | 命中实际交互元素时写 hit=1；根、body、rml 和文档本体不算命中。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 仅 Success 时读取 hit；失效 Context 返回 InvalidHandle。 |
| <a id="api-0adb0a00bb407609"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessMouseButton(Inno.Native.UI.InnoUiRuntime self, ulong context, int button, byte down, int modifiers)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1303) | 根据 down 是否非零传递鼠标按钮按下或释放。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | button 必须为 0..4；返回成功不等于消费该事件。 |
| <a id="api-70cb8802c548f206"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessMouseWheel(Inno.Native.UI.InnoUiRuntime self, ulong context, float x, float y, int modifiers)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1310) | 传递二维滚轮量和 modifier 状态。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle；返回成功不报告消费状态。 |
| <a id="api-3b24382f59b4a312"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessKey(Inno.Native.UI.InnoUiRuntime self, ulong context, int key, byte down, int modifiers)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1317) | 由当前 adapter 的键码映射传递按下或释放。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 未识别键映射为底层 unknown；不是操作系统原生键码透传。 |
| <a id="api-34a93fdf8e23e530"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessText(Inno.Native.UI.InnoUiRuntime self, ulong context, byte* text)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1324) | 传递 UTF-8 文本输入。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 空指针返回 InvalidArgument；按键与文本输入分别传入。 |
| <a id="api-61f7f5963c7c1d74"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ProcessText(Inno.Native.UI.InnoUiRuntime self, ulong context, string text)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1331) | 传递 UTF-8 文本输入。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 字符串临时转为 UTF-8，调用结束释放临时内存。 | 空指针返回 InvalidArgument；按键与文本输入分别传入。 |
| <a id="api-5b25643e6a9ed77b"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.Update(Inno.Native.UI.InnoUiRuntime self, ulong context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1347) | 更新当前 Context 的状态和布局。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle，底层更新失败返回 BackendError。 |
| <a id="api-542cba5125965651"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.Render(Inno.Native.UI.InnoUiRuntime self, ulong context, Inno.Native.UI.InnoUiFrameInfoPtr frame)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1354) | 收集本帧绘制与资源增量，成功写入 FrameInfo。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 在下一次 Render/FinishFrame 前完成需要的复制。 | 失效 Context 或底层渲染失败返回结果；此调用不直接提交 GPU 命令。 |
| <a id="api-1f265a3783afc923"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.Render(Inno.Native.UI.InnoUiRuntime self, ulong context, ref Inno.Native.UI.InnoUiFrameInfo frame)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1361) | 收集本帧绘制与资源增量，成功写入 FrameInfo。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 在下一次 Render/FinishFrame 前完成需要的复制。 | 失效 Context 或底层渲染失败返回结果；此调用不直接提交 GPU 命令。 |
| <a id="api-d70c6567975fff14"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetMeshInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, Inno.Native.UI.InnoUiMeshInfoPtr mesh)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1371) | 按本帧 mesh update 索引查询资源 ID、revision 和数组长度。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引不在更新列表返回 NotFound；index 不是 mesh ID。 |
| <a id="api-b6a0abcfa92df621"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetMeshInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, ref Inno.Native.UI.InnoUiMeshInfo mesh)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1378) | 按本帧 mesh update 索引查询资源 ID、revision 和数组长度。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引不在更新列表返回 NotFound；index 不是 mesh ID。 |
| <a id="api-d29274273ad95c30"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, Inno.Native.UI.InnoUiVertexPtr vertices, ulong verticesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1388) | 复制指定 mesh ID 的顶点值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 目标缓冲区由调用方提供。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall，非空结果要求有效输出指针。 |
| <a id="api-1c7d941c59f93f19"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, ref Inno.Native.UI.InnoUiVertex vertices, ulong verticesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1395) | 复制指定 mesh ID 的顶点值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 目标缓冲区由调用方提供。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall，非空结果要求有效输出指针。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-5831b92744cf38c8"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshVertices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, System.Span<Inno.Native.UI.InnoUiVertex> vertices)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1405) | 复制指定 mesh ID 的顶点值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 目标缓冲区由调用方提供。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall，非空结果要求有效输出指针。 |
| <a id="api-664b489722bcddfe"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, Inno.Native.UI.InnoUiIndexPtr indices, ulong indicesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1414) | 复制指定 mesh ID 的 32-bit 索引值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按 Index 元素数计。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall。 |
| <a id="api-db495957479c02d4"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, ref Inno.Native.UI.InnoUiIndex indices, ulong indicesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1421) | 复制指定 mesh ID 的 32-bit 索引值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按 Index 元素数计。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-571f5b81d5ebf79d"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyMeshIndices(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong mesh, System.Span<Inno.Native.UI.InnoUiIndex> indices)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1431) | 复制指定 mesh ID 的 32-bit 索引值。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按 Index 元素数计。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 不存在的 mesh 返回 NotFound；不足容量返回 BufferTooSmall。 |
| <a id="api-ea9abc969b14b080"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime self, ulong context, Inno.Native.UI.InnoUiDrawCommandPtr commands, ulong commandsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1440) | 复制当前帧按顺序收集的 DrawCommand。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按命令数计；复制后由调用方管理。 | 不足容量返回 BufferTooSmall；不是容量查询接口，先读 FrameInfo。 |
| <a id="api-00aaf5b27eb9d276"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime self, ulong context, ref Inno.Native.UI.InnoUiDrawCommand commands, ulong commandsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1447) | 复制当前帧按顺序收集的 DrawCommand。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按命令数计；复制后由调用方管理。 | 不足容量返回 BufferTooSmall；不是容量查询接口，先读 FrameInfo。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-5f765ce48708f060"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyCommands(Inno.Native.UI.InnoUiRuntime self, ulong context, System.Span<Inno.Native.UI.InnoUiDrawCommand> commands)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1457) | 复制当前帧按顺序收集的 DrawCommand。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按命令数计；复制后由调用方管理。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 不足容量返回 BufferTooSmall；不是容量查询接口，先读 FrameInfo。 |
| <a id="api-6769da70f77a6176"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetTextureInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, Inno.Native.UI.InnoUiTextureInfoPtr texture)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1466) | 按 texture update 索引查询 ID、revision、尺寸和所需字节数。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引不在更新列表返回 NotFound；index 不是 texture ID。 |
| <a id="api-f86627f0f3d230cc"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetTextureInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, ref Inno.Native.UI.InnoUiTextureInfo texture)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1473) | 按 texture update 索引查询 ID、revision、尺寸和所需字节数。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引不在更新列表返回 NotFound；index 不是 texture ID。 |
| <a id="api-b23d28608c7a5193"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong texture, Inno.Native.UI.InnoUiBytePtr pixels, ulong pixelsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1483) | 复制指定 texture ID 的 RGBA 像素。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按字节计。 | 不存在的 texture 返回 NotFound；不足容量返回 BufferTooSmall。 |
| <a id="api-dbfaa6a2303ddbab"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong texture, ref Inno.Native.UI.InnoUiByte pixels, ulong pixelsCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1490) | 复制指定 texture ID 的 RGBA 像素。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按字节计。 | 不存在的 texture 返回 NotFound；不足容量返回 BufferTooSmall。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-1eadce1416d8494d"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyTexturePixels(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong texture, System.Span<Inno.Native.UI.InnoUiByte> pixels)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1500) | 复制指定 texture ID 的 RGBA 像素。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 容量按字节计。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 不存在的 texture 返回 NotFound；不足容量返回 BufferTooSmall。 |
| <a id="api-892bbb98336631f5"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime self, ulong context, Inno.Native.UI.InnoUiHandlePtr textures, ulong texturesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1509) | 复制本帧已释放的 texture ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 |
| <a id="api-b3b82750c6c51abe"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime self, ulong context, ref Inno.Native.UI.InnoUiHandle textures, ulong texturesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1516) | 复制本帧已释放的 texture ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-3e79e6e84508f381"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedTextures(Inno.Native.UI.InnoUiRuntime self, ulong context, System.Span<Inno.Native.UI.InnoUiHandle> textures)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1526) | 复制本帧已释放的 texture ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 |
| <a id="api-afd1579aa7c16a67"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime self, ulong context, Inno.Native.UI.InnoUiHandlePtr meshes, ulong meshesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1535) | 复制本帧已释放的 mesh ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 |
| <a id="api-625305ff2f618bcd"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime self, ulong context, ref Inno.Native.UI.InnoUiHandle meshes, ulong meshesCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1542) | 复制本帧已释放的 mesh ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-5c6a51a7756b141c"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyReleasedMeshes(Inno.Native.UI.InnoUiRuntime self, ulong context, System.Span<Inno.Native.UI.InnoUiHandle> meshes)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1552) | 复制本帧已释放的 mesh ID 列表。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 消费方据此释放自己的对应渲染缓存。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 容量不足返回 BufferTooSmall；在 FinishFrame 清除增量前读取。 |
| <a id="api-fa70ed577ccd961e"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.FinishFrame(Inno.Native.UI.InnoUiRuntime self, ulong context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1561) | 清除本帧 mesh/texture 更新及释放列表；保留仍存活的资源。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 仅在全部增量消费完成后调用。 | 失效 Context 返回 InvalidHandle；不替代 ClearEvents，也不销毁 Context。 |
| <a id="api-4eb0f10d4cecbbae"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetEventCount(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong* count)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1568) | 读取当前排队事件数。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle，并把 count 清零。 |
| <a id="api-8ad47da8faf7c734"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetEventCount(Inno.Native.UI.InnoUiRuntime self, ulong context, ref ulong count)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1575) | 读取当前排队事件数。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 失效 Context 返回 InvalidHandle，并把 count 清零。 |
| <a id="api-6e525aa678f13923"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetEventInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, Inno.Native.UI.InnoUiEventInfoPtr eventInfo)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1584) | 按队列索引读取事件种类、document ID 与 target ID UTF-8 长度。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引越界返回 NotFound；长度不包含结尾零字节。 |
| <a id="api-59fdcd963d143b70"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.GetEventInfo(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, ref Inno.Native.UI.InnoUiEventInfo eventInfo)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1591) | 按队列索引读取事件种类、document ID 与 target ID UTF-8 长度。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 | 索引越界返回 NotFound；长度不包含结尾零字节。 |
| <a id="api-4e4a85bc9f659f84"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, Inno.Native.UI.InnoUiUtf8CodeUnitPtr targetId, ulong targetIdCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1601) | 复制事件目标 ID 的 UTF-8 文本并追加一个零字节。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 至少提供 TargetIdLength+1 个 Utf8CodeUnit。 | 索引越界返回 NotFound；少于含终止符所需容量返回 BufferTooSmall。 |
| <a id="api-8ff579604209f7fe"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, ref Inno.Native.UI.InnoUiUtf8CodeUnit targetId, ulong targetIdCount)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1608) | 复制事件目标 ID 的 UTF-8 文本并追加一个零字节。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 至少提供 TargetIdLength+1 个 Utf8CodeUnit。 | 索引越界返回 NotFound；少于含终止符所需容量返回 BufferTooSmall。 ref 只固定首元素，调用方保证后续声明容量真实存在。 |
| <a id="api-8def3044e028c4ad"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.CopyEventTargetId(Inno.Native.UI.InnoUiRuntime self, ulong context, ulong index, System.Span<Inno.Native.UI.InnoUiUtf8CodeUnit> targetId)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1618) | 复制事件目标 ID 的 UTF-8 文本并追加一个零字节。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 至少提供 TargetIdLength+1 个 Utf8CodeUnit。 Span overload 固定缓冲区并从 Span.Length 传递元素数。 | 索引越界返回 NotFound；少于含终止符所需容量返回 BufferTooSmall。 |
| <a id="api-8e4794d1f907c313"></a>[`public static Inno.Native.UI.InnoUiResult Inno.Native.UI.UiNative.ClearEvents(Inno.Native.UI.InnoUiRuntime self, ulong context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1627) | 清空当前 Context 的事件队列。 | 在创建 Runtime 的进程宿主线程串行调用；调用方保持 Runtime 和所需 Context 存活。 清理后此前的事件索引不再有效。 | 失效 Context 返回 InvalidHandle；不清除帧资源增量。 |
| <a id="api-f9e26b84bc534078"></a>[`public static void Inno.Native.UI.UiNative.InitApi(BGCS.Runtime.INativeContext context)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1637) | 完整解析 exports 后切换函数表，失败释放候选并保留旧表。 | FunctionTable 构造成功后接管 context；解析失败也释放候选 context。不得与调用并发。 | 缺 export 时传播加载错误；不创建 UI Runtime。 |
| <a id="api-ecda23e20197fbd2"></a>[`public static void Inno.Native.UI.UiNative.FreeApi()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1691) | 释放生成函数表及其持有的 Native context。 | 所有调用停止且 Runtime 已销毁后由加载 owner 执行。 | 不自动逐个销毁 UI Runtime；释放后禁止再调用函数表。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
