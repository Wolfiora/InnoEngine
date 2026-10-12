# Inno.Native.ImGuizmo

[分类索引](README.md) · [Editor Scene](../../editor/Inno.Editor.Scene.md) · [Wiki 首页](../../README.md)

## 职责与公开入口

承载 BGCS 生成的 gizmo 互操作表，以及宿主原生库初始化。`ImGuizmo` 的公开操作与 ABI 类型由
`Bindings/bindings.props` 选择的 `Bindings/bindgen.json` 及实际目标配置 生成到 `Generated/Bindings.cs`；不手工修改生成物。
这些 API 仅属于原生/presentation 边界，不属于后端中立 Scene 或脚本 API。

## 初始化及所有权

静态初始化以当前程序集作为明确 owner，首先加载 cimgui 依赖，再加载 gizmo API。
内部 `ImGuizmoNativeContext` 同时拥有两次加载取得的句柄；生成的 FunctionTable 拥有此 context。
释放顺序是 API 后依赖，重复释放安全。初始化失败释放本次已取得的两个句柄，不遗留忽略的 dependency handle。
该内部类型不是公开扩展契约。

Native library 必须由 `Inno.Build.Toolchains.ImGuizmo` 的产物提供；初始化失败传播异常，不能通过禁用 gizmo 的静默 fallback 掩盖。
模块正常使用期间原生表属于进程寿命；释放表后不能再调用其函数。

## 帧内使用与关闭顺序

宿主先创建 ImGui context 并连接 `SetImGuiContext`。每帧在 ImGui `NewFrame` 后调用
`BeginFrame`，在目标窗口中选择 draw list、设置屏幕矩形和投影方式，再执行 `Manipulate`。
同帧多个对象用 `PushID(int)` / `PopID` 隔离当前 UI 状态；场景修改和 History 仍由上层事务负责。

矩阵参数指向连续 16 个 `float`；平移、旋转和缩放向量各需 3 个。
生成的 `ref float` overload 只固定首元素，**不会验证后续数组容量**。
原生 ImGui 占位结构不是可独立构造的有效窗口或 context。

`GetStyle` 返回借用的全局样式，禁止调用 `StyleDestroy`。
只有 `StyleStyle` 创建的原始堆地址可以由唯一 owner 销毁。
所有调用、样式修改和 context 切换在 presentation owner thread 串行执行；
先停止绘制与输入，再结束相关 ImGui context 的使用，最后释放函数表和动态库。

当前消费配置仅保留单字符串、整数和指针值 ID 入口。每个字符串在一次调用中完整编码为 UTF-8；
不公开需要同一原生缓冲区 begin/end 的自动字符串区间投影。
样式创建的 Native allocator 与 `StyleDestroy` 清理操作、全局样式的借用语义都在生成配置中明确声明。

## 最小调用示例

下面的辅助函数仅用于已经初始化的 presentation owner thread：宿主已完成当前帧的
`BeginFrame`、窗口选择、`SetDrawlist`、`SetRect` 与投影设置。
它验证连续矩阵容量并固定调用期间的内存；返回后由调用方把修改后的矩阵提交给自己的编辑事务。
示例不创建 ImGui context，也不持有 Scene 对象。项目需要启用 `AllowUnsafeBlocks`。

```csharp
using System;
using Inno.Native.ImGuizmo;

internal static class GizmoTranslation
{
    internal static unsafe bool Draw(
        int scopeId,
        ReadOnlySpan<float> view,
        ReadOnlySpan<float> projection,
        Span<float> transform
    ) {
        if (view.Length != 16 || projection.Length != 16 || transform.Length != 16)
            throw new ArgumentException("Each matrix must contain exactly sixteen floats.");

        ImGuizmo.PushID(scopeId);
        try
        {
            fixed (float* viewPointer = view)
            fixed (float* projectionPointer = projection)
            fixed (float* transformPointer = transform)
                return ImGuizmo.Manipulate(viewPointer, projectionPointer,
                    ImGuizmoOperation.Translate, ImGuizmoMode.World,
                    transformPointer, null, null, null, null);
        }
        finally
        {
            ImGuizmo.PopID();
        }
    }
}
```

## 验证入口

[ImGuizmo 原生初始化测试](../../../backends/ImGui/tests/Inno.Native.ImGuizmo.Tests/ImGuizmoInitTests.cs) 通过公开 API 调用真实原生库，
验证依赖与符号初始化。此项不代替 Editor Scene 中手势和渲染的专项测试。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGuizmo/Inno.Native.ImGuizmo.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

本清单覆盖当前选定 ABI 的 148 个公开声明。逐项说明根据生成调用体与当前原生实现核对；没有把原生不安全指针包装描述为受检集合。

### `Inno.Native.ImGuizmo.ImGuizmo`

<!-- api: T:Inno.Native.ImGuizmo.ImGuizmo ed65d8c2c0b06497666de94eb48d12abbb1875a851043ff95de573abf339f5c3 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetDrawlist(Inno.Native.ImGuizmo.ImDrawList*) 3b8f58aa2fb366b592c74e19c0e6d9342d6d0108c818edab456510abf0454428 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetDrawlist(Inno.Native.ImGuizmo.ImDrawList@) 6b3cc023457c0551323cf5fa96b294471cc5d8efbd7425e3be57864b520a6f4d -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.BeginFrame e1f3fb8a92d4bf3a435194aec231c42df3818e68e4791181866f760853271177 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetImGuiContext(Inno.Native.ImGuizmo.ImGuiContext*) 40239cb63455da160f7684eb1572f8652044a5fbc83bc3c266b849a78769594f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetImGuiContext(Inno.Native.ImGuizmo.ImGuiContext@) c1dfe1aff967842ecfdef80c6996e669f7ae0982d5a6ca1c0efa9513d2524fa0 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver 02aa69d6729d1f58e2d74c231ae6ba5cbf9000587ea90ba3aa0d09ac4c7a52bc -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsing 83fb2a0c449ec0ceca14127ee91b00654f273d921fd4591c8ae8249adbefbc80 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsingViewManipulate ea38cd855ce3da20e0d95e665d3294789a212d0615d2221b7b7b721ae2723fe3 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsViewManipulateHovered ad29658464a10b311f22c6d6ba819045e7c75a23502531085feab08afa7c06d9 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsingAny d0cc7edcde1b47fe59c3188a7d6a979d10778fe38d5656e25ad9c14fc8495210 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Enable(BGCS.Runtime.Bool8) 9c70ca23e7b50e3a4331b46f9897a62818ee9335f4c24bf45b7e214a9f5e529a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Enable(System.Boolean) cdb6430407ac3ee87a5ef208f071af940dc3c3642aef736024a4b569f7923b31 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.DecomposeMatrixToComponents(System.Single*,System.Single*,System.Single*,System.Single*) 4f44e70f8e7ca10be7fe5b1eddf09a572df3d4ee3851423ca81f307d051fb38a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.DecomposeMatrixToComponents(System.Single*,System.Single@,System.Single@,System.Single@) 716451d05c56b593d7a2671a08f2d07a33536606be9cef9b158abd29317d01ea -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.RecomposeMatrixFromComponents(System.Single*,System.Single*,System.Single*,System.Single*) 0043f7c773e378204fca662d3fdb28267a12c84e0da84d41b0141a4906ea13d2 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.RecomposeMatrixFromComponents(System.Single*,System.Single*,System.Single*,System.Single@) aed0a1e41bba5df497ffc7f62201ab210cf692899297a00f14b597706cb72244 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetRect(System.Single,System.Single,System.Single,System.Single) f26606fcd30d73a581ed72f2ee0a0188b960ff2a2ac264d91b86a6164d3fcb12 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(BGCS.Runtime.Bool8) cbee568f9bcd46101b262ec94bf09c63fb61df05d81804a33ca5b4fce57cbfa8 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(System.Boolean) 49c781f0353d24d4b7228d0931d65aaa10b6fc277ade80e844b822a9d62ea34e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.DrawCubes(System.Single*,System.Single*,System.Single*,System.Int32) 3f186029332be7f639da96eac4e24ebbe1f602515d0a218a7dc59bdb3ebef9c8 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.DrawGrid(System.Single*,System.Single*,System.Single*,System.Single) b42edfb93368f2b7102d0731fd09a0a1746118fcb52d39ca4c9ca0555a9744d4 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single*,System.Single*,System.Single*,System.Single*,System.Single*) 8be2d82b63eed1b9f0c19238fd3acf205418b6825648250f0b13bebc29322856 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single@,System.Single@,System.Single*,System.Single*,System.Single*) fbffecce4db9997373ed76ba018837a7980b08fb1bc05c5b1abfbb3849deebaf -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single@,System.Single@) 408500c6209d7ffbb1d0d152809d32506eace9581c2e8b5ad4fc8fa8e1a8591e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single@,System.Single@,System.Single*) 8c1b7cb7143dd7aefbe7abf547e70a805bd07d101e95935d2ce91d689d4c98c3 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single@,System.Single@,System.Single*,System.Single*) 762f76e877e4fc3d29a3546345d9052d007ddd46283bd47ed0af91a6b765139b -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(System.Single*,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 19414e10d4eff71d3d5ee21083fd3f61d0485353e70b02b074d048b6ac78c039 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(System.Single@,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 51381724ba90de0cf7728999c9233e8ebdac5ac8de69e6ff83c5b73e12501d43 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single*,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) c9175845935e26ec9c6a962a99e42dc4dca67c46e15cc087679d1c6c8d2dcf61 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(System.Single@,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single@,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 18679f389c982a68f161be4dfd63eecac871eae3ac28dada0d63a61bc768263d -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAlternativeWindow(Inno.Native.ImGuizmo.ImGuiWindow*) 4fe710db0a33e84bff2781aaa97145dfd54f6004afe513efe721ad63144060c9 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAlternativeWindow(Inno.Native.ImGuizmo.ImGuiWindow@) 47e71be388ee9f2e9146a7ba23ea2dbdbff7e996056c8fe6308af92f7af19273 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PushID(System.Byte*) fb033bc39fecc615ac703bf663f730ecbbbb6ca54c4bf327fe470bad5dd23a6c -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PushID(System.String) d190085ff2db1e69cbe469abeda47b6132e102a560839e6814900e42686a04c0 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PushID(System.Void*) 91be1059d3f1295074d8dd908cf48c487f4ff71c20371d4c7cc2dc477eb99de5 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PushID(System.Int32) 63a106ccaebd75427616b37347c665bb4a219641465f184c82e15d18ebfa18ff -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PopID 1024605ec40f7d281ec2e4e4899eddbacbbdbb321e1dd1bfd95f264cd623d63b -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.GetID(System.Byte*) af792d618cbd7b8f0dcd44e3e990474835cc03a2998c99bfa44baed1067fe892 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.GetID(System.String) c7ec05f8c56e21f97069ae3715249b99c837d337a05909fc44d997e1283c1a14 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.GetID(System.Void*) e20fdc2de0ae3db622328d4c5fd107fcc00795469cb6b133b054bdf47293c136 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation) d83f7aacc5e51fd6554dc770d9f624d7ea8914d28066676104dc78147adc4a65 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetGizmoSizeClipSpace(System.Single) bb25e589a83bb87b1b264b8c47ef5d9d1b94cdcd90a9201d98417cb6cce1d009 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(BGCS.Runtime.Bool8) 6b06a8169ca39226c94bb407888477355a6d62255894e13b94eec72605f366ca -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(System.Boolean) b1e2ca41f94579b2eb908f680c606e995148682cfc56da22e8b581f4dd045239 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAxisLimit(System.Single) 5da062ac1593edf605124085d3a167e457c4341701237644fc72a42480910871 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(BGCS.Runtime.Bool8,BGCS.Runtime.Bool8,BGCS.Runtime.Bool8) 43edaa6bc7f179681547f02e783be8e63a8ebbf332629e9477b4e117b4f0417c -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(System.Boolean,System.Boolean,System.Boolean) 09fb6bfd4a376eaeb620b90c004e1444ade08d5517a7925759ea808602d34f89 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetPlaneLimit(System.Single) 6cdb4648a928c797ad7befb7da1a34886adcd264621a2fc2536af4ce5a15901e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver(System.Single*,System.Single) 71a739ff1c5b9c2b0cdb2b73baeb1debf792127e6b67ca8d3c6be533fa85b26a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver(System.Single@,System.Single) df838e5b416ff328bf62a27e633799a5208e2b2c98527345d1a135f7fa78cd55 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.StyleStyle bd88f5502e9c937b2785cfd126d139299664a08a74b55d1a02e5bc9a37dc25df -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.StyleDestroy(Inno.Native.ImGuizmo.StylePtr) 5e985d45d044d017b95416b9cefc7e99a903e9e8ee48784499ae573966b1917c -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.StyleDestroy(Inno.Native.ImGuizmo.Style@) c99f74a070f398563c8b477e0993189ca1e6d024ba8cae89d03986bee7155699 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.GetStyle 5e4533cb0b86e4550d42f9bf68588b926fe811883c0cc2fb443beb1c57281d0d -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.InitApi(BGCS.Runtime.INativeContext) 7c7c8ad4b6d9f55cda74db84ba418d3808bf5c20d17deaf6595ca89cca71fada -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.FreeApi 5acfc0ec7b0a9a620d81c062df90eb173353b2f3f07fd21d94816d4c99cb8234 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0dad34c884f5db58"></a>[`public static class Inno.Native.ImGuizmo.ImGuizmo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/ImGuizmo.cs#L9) | 生成的 gizmo 静态调用入口。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-d003874d0b816748"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetDrawlist(Inno.Native.ImGuizmo.ImDrawList* drawlist)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L510) | 选择本帧 gizmo 使用的 ImGui draw list；空指针选择当前窗口。 | 借用 ImGui 所属 draw list，不转移释放权。 | 在目标 ImGui 窗口与帧有效时调用。 |
| <a id="api-ce436fc5e111e634"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetDrawlist(ref Inno.Native.ImGuizmo.ImDrawList drawlist)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L516) | 选择本帧 gizmo 使用的 ImGui draw list；空指针选择当前窗口。 | 借用 ImGui 所属 draw list，不转移释放权。 | 在目标 ImGui 窗口与帧有效时调用。 |
| <a id="api-a13a7f4655de91ce"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.BeginFrame()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L525) | 在 ImGui NewFrame 后初始化本帧 gizmo 绘制状态。 | 在 presentation owner thread 调用一次。 | 不能脱离有效 ImGui context 或与另一线程并行调用。 |
| <a id="api-56e34023e3085d9d"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetImGuiContext(Inno.Native.ImGuizmo.ImGuiContext* ctx)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L531) | 把独立原生模块连接到指定 ImGui context。 | 借用 context；切换前停止原 context 的调用。 | 必须传入真实 ImGui 创建的对象；不使用本绑定的占位结构自行构造 context。 |
| <a id="api-54fa76b77c44ff84"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetImGuiContext(ref Inno.Native.ImGuizmo.ImGuiContext ctx)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L537) | 把独立原生模块连接到指定 ImGui context。 | 借用 context；切换前停止原 context 的调用。 | 必须传入真实 ImGui 创建的对象；不使用本绑定的占位结构自行构造 context。 |
| <a id="api-d1991e29ff85db09"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsOver()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L546) | 查询鼠标是否覆盖 gizmo；operation overload 限定操作，position overload 检查投影点与像素半径。 | 读取当前 gizmo 的帧状态，不创建或占有对象。 | position 指向 3 个连续 float，并使用此前设置的变换状态。 |
| <a id="api-0388ed704c16022f"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsing()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L553) | 查询当前 gizmo 是否正在拖动。 | 读取当前原生交互状态。 | 不能代替引擎的焦点、输入消费或 History 事务。 |
| <a id="api-eb2de5ca893f4dfd"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsingViewManipulate()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L560) | 查询视图操纵器是否正在拖动。 | 读取当前原生交互状态。 | 仅适用于已建立的当前帧。 |
| <a id="api-a7b3ef9363eb70ef"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsViewManipulateHovered()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L567) | 查询鼠标是否位于视图操纵器上。 | 只读取，不捕获输入。 | 悬停与正在拖动是不同状态。 |
| <a id="api-282d0906cea647e2"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsingAny()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L574) | 查询任意物体 gizmo 或边界控件是否正在拖动，不限定当前 ID。 | 只读取当前原生交互状态。 | 当前实现不包含视图操纵器；该状态单独由 IsUsingViewManipulate 查询。 |
| <a id="api-824db8758d381c50"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.Enable(BGCS.Runtime.Bool8 enable)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L581) | 持续启用或禁用 gizmo；禁用时使用不可交互样式。 | 修改当前原生 gizmo 配置。 | 保留到下一次 Enable；不是对象生命周期开关。 |
| <a id="api-2ddcddb3db0ec723"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.Enable(bool enable)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L587) | 持续启用或禁用 gizmo；禁用时使用不可交互样式。 | 修改当前原生 gizmo 配置。 | 保留到下一次 Enable；不是对象生命周期开关。 |
| <a id="api-dc2f935633feccc2"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.DecomposeMatrixToComponents(float* matrix, float* translation, float* rotation, float* scale)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L593) | 把矩阵分解为平移、角度和缩放；角度单位为度。 | 输入 16 个 float，三个输出各 3 个连续 float。 | 数值分解存在稳定性限制；不自动保留任意 shear 或奇异矩阵信息。 |
| <a id="api-ed4e2ae5331c5b49"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.DecomposeMatrixToComponents(float* matrix, ref float translation, ref float rotation, ref float scale)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L599) | 把矩阵分解为平移、角度和缩放；角度单位为度。 | 输入 16 个 float，三个输出各 3 个连续 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | 数值分解存在稳定性限制；不自动保留任意 shear 或奇异矩阵信息。 |
| <a id="api-7377bdcc5fa67f30"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.RecomposeMatrixFromComponents(float* translation, float* rotation, float* scale, float* matrix)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L614) | 从平移、以度为单位的旋转和缩放生成矩阵。 | 三个输入各 3 个 float，输出 16 个连续 float。 | 不校验所传缓冲区容量。 |
| <a id="api-3aaa16ae43bcfdf5"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.RecomposeMatrixFromComponents(float* translation, float* rotation, float* scale, ref float matrix)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L620) | 从平移、以度为单位的旋转和缩放生成矩阵。 | 三个输入各 3 个 float，输出 16 个连续 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | 不校验所传缓冲区容量。 |
| <a id="api-28c25175fd2fb563"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetRect(float x, float y, float width, float height)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L629) | 指定 gizmo 绘制和交互使用的屏幕矩形。 | 复制四个标量，不持有窗口。 | 调用方同步实际目标区域；不负责 viewport 或 DPI 路由。 |
| <a id="api-2f48991a6e5de205"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(BGCS.Runtime.Bool8 isOrthographic)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L635) | 设置当前投影是否为正交模式。 | 配置由调用方保持与 projection 矩阵一致。 | 错误模式会导致错误交互，不会自动解析矩阵。 |
| <a id="api-9407557979bcd4bc"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(bool isOrthographic)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L641) | 设置当前投影是否为正交模式。 | 配置由调用方保持与 projection 矩阵一致。 | 错误模式会导致错误交互，不会自动解析矩阵。 |
| <a id="api-4b626c3084628a11"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.DrawCubes(float* view, float* projection, float* matrices, int matrixCount)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L647) | 按 matrices 中的变换绘制调试立方体。 | view/projection 各 16 个 float；matrices 至少 matrixCount × 16 个 float。 | 不创建引擎几何或持久 GPU 资源。 |
| <a id="api-e4e69a09a23ed174"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.DrawGrid(float* view, float* projection, float* matrix, float gridSize)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L653) | 在指定矩阵平面绘制调试网格。 | 借用 view、projection、matrix 各 16 个 float。 | gridSize 由调用方选择；绘制进入当前 draw list。 |
| <a id="api-7e87d5246cb1671f"></a>[`public static BGCS.Runtime.Bool8 Inno.Native.ImGuizmo.ImGuizmo.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, float* matrix, float* deltaMatrix, float* snap, float* localBounds, float* boundsSnap)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L659) | 绘制并处理 gizmo，写回 matrix；返回本次是否发生变换。 | view/projection/matrix 各 16 个 float；非空 deltaMatrix 也必须有 16 个 float。 | snap、localBounds 和 boundsSnap 按操作约定提供足够容量；不自动提交 Scene 修改或 History。 |
| <a id="api-2b4d283278582641"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, ref float matrix, ref float deltaMatrix, float* snap, float* localBounds, float* boundsSnap)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L666) | 绘制并处理 gizmo，写回 matrix；返回本次是否发生变换。 | view/projection/matrix 各 16 个 float；非空 deltaMatrix 也必须有 16 个 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | snap、localBounds 和 boundsSnap 按操作约定提供足够容量；不自动提交 Scene 修改或 History。 |
| <a id="api-43da403f9e44bd1e"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, ref float matrix, ref float deltaMatrix)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L681) | 绘制并处理 gizmo，写回 matrix；返回本次是否发生变换。 | view/projection/matrix 各 16 个 float；非空 deltaMatrix 也必须有 16 个 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | snap、localBounds 和 boundsSnap 按操作约定提供足够容量；不自动提交 Scene 修改或 History。 |
| <a id="api-9e84031fd00e4eb6"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, ref float matrix, ref float deltaMatrix, float* snap)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L689) | 绘制并处理 gizmo，写回 matrix；返回本次是否发生变换。 | view/projection/matrix 各 16 个 float；非空 deltaMatrix 也必须有 16 个 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | snap、localBounds 和 boundsSnap 按操作约定提供足够容量；不自动提交 Scene 修改或 History。 |
| <a id="api-60bde415f52a194d"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, ref float matrix, ref float deltaMatrix, float* snap, float* localBounds)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L697) | 绘制并处理 gizmo，写回 matrix；返回本次是否发生变换。 | view/projection/matrix 各 16 个 float；非空 deltaMatrix 也必须有 16 个 float。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | snap、localBounds 和 boundsSnap 按操作约定提供足够容量；不自动提交 Scene 修改或 History。 |
| <a id="api-e079500b7e8ac382"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(float* view, float length, System.Numerics.Vector2 position, System.Numerics.Vector2 size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L703) | 绘制视图方向控件并写回 view；完整 overload 同时设置投影与物体操作上下文。 | view、projection、matrix 指针各需 16 个 float；按 overload 提供。 | 简化 overload 依赖此前 Manipulate 配置；未调用时使用完整 overload。 |
| <a id="api-b221d8475176b011"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(ref float view, float length, System.Numerics.Vector2 position, System.Numerics.Vector2 size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L709) | 绘制视图方向控件并写回 view；完整 overload 同时设置投影与物体操作上下文。 | view、projection、matrix 指针各需 16 个 float；按 overload 提供。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | 简化 overload 依赖此前 Manipulate 配置；未调用时使用完整 overload。 |
| <a id="api-7f843b8813ff7dcc"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, float* matrix, float length, System.Numerics.Vector2 position, System.Numerics.Vector2 size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L718) | 绘制视图方向控件并写回 view；完整 overload 同时设置投影与物体操作上下文。 | view、projection、matrix 指针各需 16 个 float；按 overload 提供。 | 简化 overload 依赖此前 Manipulate 配置；未调用时使用完整 overload。 |
| <a id="api-cee3f841b96b3c97"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.ViewManipulate(ref float view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, ref float matrix, float length, System.Numerics.Vector2 position, System.Numerics.Vector2 size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L724) | 绘制视图方向控件并写回 view；完整 overload 同时设置投影与物体操作上下文。 | view、projection、matrix 指针各需 16 个 float；按 overload 提供。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | 简化 overload 依赖此前 Manipulate 配置；未调用时使用完整 overload。 |
| <a id="api-155939519aeb9e1c"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAlternativeWindow(Inno.Native.ImGuizmo.ImGuiWindow* window)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L736) | 选择额外参与 gizmo 悬停判定的 ImGui 窗口。 | 借用真实 ImGuiWindow，不接管窗口销毁。 | 窗口释放前停止使用该借用；不能传入自行创建的占位值。 |
| <a id="api-a33c2c8963d22980"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAlternativeWindow(ref Inno.Native.ImGuizmo.ImGuiWindow window)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L742) | 选择额外参与 gizmo 悬停判定的 ImGui 窗口。 | 借用真实 ImGuiWindow，不接管窗口销毁。 | 窗口释放前停止使用该借用；不能传入自行创建的占位值。 |
| <a id="api-ae223ba7f81b3ccf"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PushID(byte* strId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L751) | 把文本、指针值或整数的哈希加入 gizmo ID 栈。 | 每次成功调用必须在同一线程用 PopID 配对。 | ID 只隔离当前 UI 状态，不是持久 Identity。 |
| <a id="api-dc664708f3c7fd9e"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PushID(string strId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L757) | 把文本、指针值或整数的哈希加入 gizmo ID 栈。 | 每次成功调用必须在同一线程用 PopID 配对。 本次调用期间编码 UTF-8，返回后释放临时缓冲区。 | ID 只隔离当前 UI 状态，不是持久 Identity。 |
| <a id="api-0e2731484d8a3c7c"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PushID(void* ptrId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L773) | 把文本、指针值或整数的哈希加入 gizmo ID 栈。 | 每次成功调用必须在同一线程用 PopID 配对。 | ID 只隔离当前 UI 状态，不是持久 Identity。 |
| <a id="api-decc610a92d24710"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PushID(int intId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L779) | 把文本、指针值或整数的哈希加入 gizmo ID 栈。 | 每次成功调用必须在同一线程用 PopID 配对。 | ID 只隔离当前 UI 状态，不是持久 Identity。 |
| <a id="api-0770f149794d6214"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PopID()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L785) | 弹出一层 gizmo ID scope。 | 只结束当前调用方自己建立的 scope。 | 不得在空栈上调用；推荐 finally 保持配对。 |
| <a id="api-a7a38c0cf17073c8"></a>[`public static uint Inno.Native.ImGuizmo.ImGuizmo.GetID(byte* strId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L791) | 将参数与当前 ID 栈组合，返回 UI ID。 | 不改变 ID 栈，不取得指针目标所有权。 | 不能用作资产、Scene 或跨 reload 的持久身份。 |
| <a id="api-b0391bffa6ed7561"></a>[`public static uint Inno.Native.ImGuizmo.ImGuizmo.GetID(string strId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L798) | 将参数与当前 ID 栈组合，返回 UI ID。 | 不改变 ID 栈，不取得指针目标所有权。 本次调用期间编码 UTF-8，返回后释放临时缓冲区。 | 不能用作资产、Scene 或跨 reload 的持久身份。 |
| <a id="api-ade6355fb0962dd5"></a>[`public static uint Inno.Native.ImGuizmo.ImGuizmo.GetID(void* ptrId)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L814) | 将参数与当前 ID 栈组合，返回 UI ID。 | 不改变 ID 栈，不取得指针目标所有权。 | 不能用作资产、Scene 或跨 reload 的持久身份。 |
| <a id="api-f56d8889187d02a5"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation op)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L821) | 查询鼠标是否覆盖 gizmo；operation overload 限定操作，position overload 检查投影点与像素半径。 | 读取当前 gizmo 的帧状态，不创建或占有对象。 | position 指向 3 个连续 float，并使用此前设置的变换状态。 |
| <a id="api-b63dab49d291a1f4"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetGizmoSizeClipSpace(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L828) | 调整 gizmo 在裁剪空间中的尺寸。 | 复制标量到原生配置。 | 不是像素大小或引擎对象缩放。 |
| <a id="api-bbbe31b8484ac8b7"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(BGCS.Runtime.Bool8 value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L834) | 允许或禁止为可见性翻转 gizmo 轴。 | 持续配置，直到下次修改。 | 禁止时保持正轴方向；不修改物体矩阵。 |
| <a id="api-fc812372612e5a1e"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(bool value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L840) | 允许或禁止为可见性翻转 gizmo 轴。 | 持续配置，直到下次修改。 | 禁止时保持正轴方向；不修改物体矩阵。 |
| <a id="api-25eb00fa4564ad4f"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAxisLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L846) | 调整轴接近观察方向时的隐藏阈值。 | 复制标量到原生配置。 | 依赖当前观察矩阵，不表达平台能力。 |
| <a id="api-78a515395e67b87b"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(BGCS.Runtime.Bool8 x, BGCS.Runtime.Bool8 y, BGCS.Runtime.Bool8 z)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L852) | 分别强制隐藏 X、Y、Z 轴；true 表示隐藏。 | 复制三个布尔值。 | 与 Enable 的整体交互开关不同。 |
| <a id="api-b8fa3341b26a6b54"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(bool x, bool y, bool z)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L858) | 分别强制隐藏 X、Y、Z 轴；true 表示隐藏。 | 复制三个布尔值。 | 与 Enable 的整体交互开关不同。 |
| <a id="api-d757ddaa7c72cf32"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetPlaneLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L864) | 调整平面接近侧视时的隐藏阈值。 | 复制标量到原生配置。 | 不改变当前物体或投影。 |
| <a id="api-22f1c87bf5da1e6a"></a>[`public static BGCS.Runtime.Bool8 Inno.Native.ImGuizmo.ImGuizmo.IsOver(float* position, float pixelRadius)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L870) | 查询鼠标是否覆盖 gizmo；operation overload 限定操作，position overload 检查投影点与像素半径。 | 读取当前 gizmo 的帧状态，不创建或占有对象。 | position 指向 3 个连续 float，并使用此前设置的变换状态。 |
| <a id="api-be4c5880fbdb2f37"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsOver(ref float position, float pixelRadius)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L877) | 查询鼠标是否覆盖 gizmo；operation overload 限定操作，position overload 检查投影点与像素半径。 | 读取当前 gizmo 的帧状态，不创建或占有对象。 ref float 仅固定首元素，调用方仍须提供所需的连续数组。 | position 指向 3 个连续 float，并使用此前设置的变换状态。 |
| <a id="api-6b6481db055f475f"></a>[`public static Inno.Native.ImGuizmo.StylePtr Inno.Native.ImGuizmo.ImGuizmo.StyleStyle()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L887) | 在原生堆上构造带默认样式的 Style。 | 返回值由调用方拥有，用 StyleDestroy 恰好释放一次。 | 不能把返回指针的值副本当作另一个可销毁对象。 |
| <a id="api-b1e43a295d4c7003"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.StyleDestroy(Inno.Native.ImGuizmo.StylePtr self)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L894) | 销毁由 StyleStyle 分配的样式对象。 | 只允许原始分配地址的唯一 owner 调用。 | 禁止销毁 GetStyle 的借用、栈上 Style 或已释放对象；ref overload 不提供这种保护。 |
| <a id="api-81ff95949c481518"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.StyleDestroy(ref Inno.Native.ImGuizmo.Style self)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L900) | 销毁由 StyleStyle 分配的样式对象。 | 只允许原始分配地址的唯一 owner 调用。 | 禁止销毁 GetStyle 的借用、栈上 Style 或已释放对象；ref overload 不提供这种保护。 |
| <a id="api-fa46adf15d2ecdc7"></a>[`public static Inno.Native.ImGuizmo.StylePtr Inno.Native.ImGuizmo.ImGuizmo.GetStyle()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L909) | 返回当前 gizmo 全局样式的借用指针。 | 在 owner thread 修改；不调用 StyleDestroy。 | 不能跨原生库退休保存指针或由其取得的 ref/Span。 |
| <a id="api-d2d97233702007de"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.InitApi(BGCS.Runtime.INativeContext context)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L919) | 解析完整符号表后替换当前生成调用表。 | 加载 owner 调用；FunctionTable 接管传入解析 context。 | 解析失败释放候选；不得与正在执行的 Native 调用并发。 |
| <a id="api-d689cbf6893df1c4"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.FreeApi()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L971) | 释放函数表及其所持 Native context。 | 所有调用停止后由加载 owner 最终执行。 | 不会自动结束 ImGui 帧或销毁外部 context；之后不得调用原表。 |

### `Inno.Native.ImGuizmo.ImGuizmoOperation`

<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoOperation 6bf7f9e6a258ec6ca246a9a537a5caa71e1ef4f1955c06e594d01e59e2322d3c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateX e3dd626a5241d835592c3b78573af191043a1345d3b2d400a7a7025303276d02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateY f15d8b4af077b7b3a44b18fc4a94f262b6f376b6f5abbcc5645eec8c155e3444 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateZ 234fb2c6251e3e03895a80cd959814236746e508d583ab949046d3070d8d6e3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateX 5b7ead8f7a885699c57c78f87f00d1cd492cafe2a8e6fce4d1bf4409824807c3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateY 9af9b3f01a2ed53f773d7acda06a2ac62cb5e7d11f9faf44b446137feb2192fc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateZ 24ee78358d546151d8d7ee70986cb9be6f0da09b3c173e76d779d8f4a84915c4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateScreen b591eea376526c45e43a9d91d08b6a7a7a3bd26f5c9967bc34dbf6e1e035f9b4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleX 043fffc74bff282f7b3ccff15229a07367d82b92e067a474a060216d16d189cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleY dbf122acf45943b5721aa116423e2d196ca63e14d7589dfcb69235ea8b127679 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZ 23eb68c7874059aa2cc4423af67ef495bcd3fa51076e0f7b52317fff44ef74f6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Bounds 50186e8d0d84362042c68d00f07f8a6d43d81c69517c31f364904058a858c659 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleXu 25683bf27265544ec21e165c486c822fda1c658c117b81d705655808ff968d99 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleYu 92839c18ece00d2fec2ba98f56504058bc90691ad2aa1df98f563facf3b1fff9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZu 539f592fa319d21645af44b72dd6244c344dd3db224c5bc3f2ee125007f44d42 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Translate 65f4dc54e414e64b33595cd67ee51782e48b54118f89bf3ab7d42e83cc05839d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Rotate 22dcca70138acb085947442bc9db4d6a091c3e3932ea2db98bc34fb3c8c79de3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Scale 6bb306c35cd7dcd4049f69f1686a1006dd559c82e51da6928a0dac814e464c52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Scaleu dd623bbdfab04a4c218378124e40d451cc9e0a7831568a2f12657c4e8cf90fe0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Universal 65c245af8ff9c48d39567a6ca5cb02648edcfac9620ea22f31c384479971bcf7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e947f2b321c138b1"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoOperation : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L26) | 可按位组合的变换操作标志。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-43900ba55866a636"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateX = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L28) | X 轴平移。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-4d3e9ef0b284d7c4"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateY = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L29) | Y 轴平移。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-34863e4810a9b516"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateZ = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L30) | Z 轴平移。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-ce26d54c18fd8693"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateX = 8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L31) | 绕 X 轴旋转。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-486ad0400c55fcf3"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateY = 16`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L32) | 绕 Y 轴旋转。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-f1b4ee15048cc595"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateZ = 32`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L33) | 绕 Z 轴旋转。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-0a0145e3a5fdfceb"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateScreen = 64`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L34) | 绕屏幕法线旋转。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-5dc424efea31ff2a"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleX = 128`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L35) | X 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-46a076509af2c622"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleY = 256`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L36) | Y 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-58cbb40ba85b1ff0"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZ = 512`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L37) | Z 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-9dcbeb936afc06a5"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Bounds = 1024`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L38) | 边界框调整。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-b07fac0f79872f56"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleXu = 2048`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L39) | 通用操纵器的 X 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-a358dbf06a215778"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleYu = 4096`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L40) | 通用操纵器的 Y 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-af2a21975af86da2"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZu = 8192`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L41) | 通用操纵器的 Z 轴缩放。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-98d814199c4c7d29"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Translate = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L42) | 三个平移轴组合。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-6fea563686cc5345"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Rotate = 120`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L43) | 三个旋转轴与屏幕旋转组合。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-441e053e384d387f"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Scale = 896`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L44) | 三个缩放轴组合。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-d3dbc8381a811efc"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Scaleu = 14336`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L45) | 三个通用缩放轴组合。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |
| <a id="api-4e736d1ad5e76713"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Universal = 14463`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L46) | 平移、旋转与通用缩放组合。 | 纯值，可按位组合。 | 不是引擎 Feature 或持久语义 ID。 |

### `Inno.Native.ImGuizmo.ImGuizmoMode`

<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoMode 65909b6a9c6e6ee448e29e02b8aec317eeabb952893a8a1d4247454b9884b94f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoMode.Local f09125d741b5a910d873be53eb382e0ce9e2d484d3314753cb0655cd49823a78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoMode.World 4987275973c43c985f156758508c3578fab4400b8b69b9b61ddf1bfbad15c4a9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2d56bfb7db457de4"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoMode : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L52) | 变换采用的坐标空间。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-d58148eb0cd5eef8"></a>[`Inno.Native.ImGuizmo.ImGuizmoMode.Local = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L54) | 按物体局部坐标轴操作。 | 纯值配置。 | 必须与调用方选择的编辑模式一致。 |
| <a id="api-5d860e611346635e"></a>[`Inno.Native.ImGuizmo.ImGuizmoMode.World = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L55) | 按世界坐标轴操作。 | 纯值配置。 | 必须与调用方选择的编辑模式一致。 |

### `Inno.Native.ImGuizmo.ImGuizmoColor`

<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoColor cf4f9bb7a70a975769702c81dabe5c54d8b4c6974592bb9673f8b7db6fbf2516 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionX e64cdb2e0003bc0425af8cb8c118baf28808e98a214f16a2c30ee159f3d23541 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionY 5f158eba9239a420d753d57123298ffa1444a35b0fabadb10ecdd2dd97b4b82b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionZ c7b44ea709130c0002c0af133113189befeb6e942384a84e09de04c0db9475d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneX 835f215122c8045ee628bd7faafb10c7036a90a378423484d1a228ec6cdb2b19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneY e13c5f8d8dd34334d9c5367f8d0835c022f52c4adf46a961cd37ed5a8e337f76 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneZ 9ae6277bc086c1bb207fabbb45da6020900811c331340ff3c4a3fa6c8a2d33c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Selection ec31b7c6a820ffb51e1456e3f1a6e584d5a75c6ce00824a08c9ae3b036a4bf28 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Inactive 46c4a991b4981ed8b4b2850f60ad33ab44a3c13c1f5e1665201d0bb0e4b0412a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.TranslationLine bd7be90bfb8d2ee5432ad681480336168923e4c89c69aa1c89e51cf37c332cf6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.ScaleLine 36e41fe706453c626a1858f4d9277fc99b2b98b46205945b158d929d31508903 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingBorder 8fc4433588ddb4a38ff6efb8a4a0acc7e58f209b3b09d09f2774a3955c92e2cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingFill b75bacaa2bee9fbdef627c4cea77ed4ce1a5e62be769d7fc2311b90c5ab4f9a0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.HatchedAxisLines 97692ca2d3dae36dddab5310cd9166b7d8aede45a7e4e70cc3ca91b280dce14d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Text 4247aa9a505e0e5fa780f742dc31aafa772d9a7a275645d5268913244e8160f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.TextShadow 6ad1ca4ec140a3341efba90aa19118a763bc819423179814da9b7a27ae391a5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Count 9abb494aa7940947b7e9ff327ec6e3f253670df14780e7145d21bc72f3261a0c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b5ddd4e5526053e0"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoColor : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L61) | Style 颜色数组的槽位。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-11930f021248778c"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionX = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L63) | X 轴的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-90658d9db52f6b52"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionY = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L64) | Y 轴的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-24555adb8be87afa"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionZ = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L65) | Z 轴的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-454141c8efa68f07"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneX = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L66) | X 平面的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-5437fa9441132719"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneY = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L67) | Y 平面的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-6b9c5b0049fab31a"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneZ = 5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L68) | Z 平面的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-360c19368c31ac85"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Selection = 6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L69) | 选中项的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-58ee63d7f72871e6"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Inactive = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L70) | 禁用项的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-adc7f7941421fa85"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.TranslationLine = 8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L71) | 平移辅助线的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-b53e5b2c885fbfb1"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.ScaleLine = 9`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L72) | 缩放辅助线的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-85e4b0654a06bc44"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingBorder = 10`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L73) | 旋转活动边缘的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-656b3593fc58b9f4"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingFill = 11`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L74) | 旋转活动填充的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-169d30a361a0408f"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.HatchedAxisLines = 12`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L75) | 背面轴虚线的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-0d27bde1532afd2c"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Text = 13`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L76) | 文字的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-c4abc66f41f9c275"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.TextShadow = 14`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L77) | 文字阴影的 RGBA 颜色槽位。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |
| <a id="api-4bdbb4e97292f174"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Count = 15`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L78) | 颜色槽位总数，不是可访问颜色。 | 用于索引当前 Style 的 15 项颜色。 | Count 不能用作数组下标。 |

### `Inno.Native.ImGuizmo.Style`

<!-- api: T:Inno.Native.ImGuizmo.Style 9f07e65b24c1ad8b3405ee5207f8642f01a94de562c9302422541ffc6c4bc86f -->
<!-- api: F:Inno.Native.ImGuizmo.Style.TranslationLineThickness ef7a9c34d0425b63d25d1a205b2ec8fe351a74346d258a1a2bcf748753212518 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.TranslationLineArrowSize de7ff1f57c8f860a8b6fe53fd7a29523e03be91a6004e259770d74986f80c028 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.RotationLineThickness 394fe0f464f61f77737ac3e2aebfc6f6d276d4b8ad23677b0f555ba343017b6c -->
<!-- api: F:Inno.Native.ImGuizmo.Style.RotationOuterLineThickness 16f9a2fc37702615d4c06e377afa7b32c95becd20c1ded7a84c2fd599174b43b -->
<!-- api: F:Inno.Native.ImGuizmo.Style.ScaleLineThickness bbca3b9b6c4ba17e884aa43aaa187a1017e93d3255e7d79c9ecccd1c7502f7c4 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.ScaleLineCircleSize 79a00c46bc0264dff7914b6ed7365a03627dc384671e0a2b024bd949d75cb5ad -->
<!-- api: F:Inno.Native.ImGuizmo.Style.HatchedAxisLineThickness d2d005dacfe0dc1118ee44de8f13fc13305deb6da50c56c4cf5f9fa70284ece5 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.CenterCircleSize 22f136fba2057d620d43fa8e0deef09fbf2acc66bcf427d995e2874c17fbd845 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_0 e72740fa7d88b7292728fa88ed333a2443230571966ae5d09a8808d932577a53 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_1 e3b3b7ffaa33222b46308cbd1732bdc03687ecba9e2e3e05a059cbf9412f4ffc -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_2 f046108143da31ee18579539d7655adeee07c6ac26f947789c790033564810ea -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_3 e4176a11d1ae63a988a77ef88e9b486cfc955bddc39891419563bb70385bae36 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_4 e59b1297a447bc2e9b5ba1f63ea75e74c39bc9bf3dd74b097d59be3341c36636 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_5 01ceaf5df82e7598a0de27702ea1d074d4d0174d7ec43ebb899b8483d7603310 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_6 513320aa33d42948aa7ef42b53e97bf3beae64760e2c56532978ab1e5f0106a8 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_7 017210751c6cfdb5effca1a9b2dcbfdb78e8c2df9c375aee2b3162329db47a26 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_8 635911a393b1ef73b2b95626a884e6eabfb5964b4c513aa0d5f119d8f629ea4e -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_9 6d5f7d736dfdf2c503b642e2932a8d93ba49bc2879daecf3c922630b7f77de2a -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_10 eac724cfc717d0d0670e2fb3532da544fdca3da24729b0b011822cd3d8844ea4 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_11 bfe3c2a8c18840b7615b354f2994bc10e4e681478759c25f5658b8bb1bcf8096 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_12 7284a30df418d3e91006a1c753e8199ee06773ce9131f4110c209b5e89103821 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_13 d38e94238e19a34adec515f1e42b02f179462f24ff9e656c8db6b3467522443a -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors_14 de421a75e2e5042edc852ac071769ed77638fd9b18dda6c2d585f63793316ec3 -->
<!-- api: P:Inno.Native.ImGuizmo.Style.Colors 418f11dee7e8cb3d4c2dee84de4d0fd5af4f99eea618cc2e89bacf23f6ff1197 -->
<!-- api: M:Inno.Native.ImGuizmo.Style.#ctor(System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Numerics.Vector4*) f8739bdcfbc8cf2e5c3bbb9980fa5fdc04e6acc0761267aa5cd0c3eb866d7cd2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3a087d023d7380fb"></a>[`public struct Inno.Native.ImGuizmo.Style`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L85) | 当前 ABI 的样式值布局。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-16f8b317178a925b"></a>[`public float Inno.Native.ImGuizmo.Style.TranslationLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L87) | 平移轴线宽。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-d5bc69672c400c0a"></a>[`public float Inno.Native.ImGuizmo.Style.TranslationLineArrowSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L88) | 平移轴箭头尺寸。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-bbf30041f24587bc"></a>[`public float Inno.Native.ImGuizmo.Style.RotationLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L89) | 旋转弧线宽。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-b3893ac9ca0ae8cb"></a>[`public float Inno.Native.ImGuizmo.Style.RotationOuterLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L90) | 旋转外圈线宽。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-87e6a364720841a0"></a>[`public float Inno.Native.ImGuizmo.Style.ScaleLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L91) | 缩放轴线宽。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-bba8fc1f070672fe"></a>[`public float Inno.Native.ImGuizmo.Style.ScaleLineCircleSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L92) | 缩放轴端圆尺寸。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-99745d956414e228"></a>[`public float Inno.Native.ImGuizmo.Style.HatchedAxisLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L93) | 背面轴虚线宽。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-ef8aad1d4d42a69f"></a>[`public float Inno.Native.ImGuizmo.Style.CenterCircleSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L94) | 平移/缩放中心圆尺寸。 | 当前结构内的可写值字段。 | 复制值不会自动更新原生全局样式。 |
| <a id="api-eacd01def7c8b1f9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L95) | 第 0 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-f5d90f244f163d0a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L96) | 第 1 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-7acb651b299adb8f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L97) | 第 2 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-e0b1e5dd416ca725"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L98) | 第 3 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-6e9c28c76577eebd"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L99) | 第 4 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-8dee2dd24945cd8e"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L100) | 第 5 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-c8fc7a7b153b305b"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L101) | 第 6 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-ad3121e0310139b6"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L102) | 第 7 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-5595da41105d090e"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L103) | 第 8 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-0756dcec8459c762"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_9`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L104) | 第 9 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-3babbdaaa7519057"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_10`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L105) | 第 10 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-f1d8bb82dab881f2"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_11`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L106) | 第 11 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-5f76a90c344f68ad"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_12`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L107) | 第 12 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-c3965e22990d1408"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_13`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L108) | 第 13 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-1a51ae8a7ba23026"></a>[`public System.Numerics.Vector4 Inno.Native.ImGuizmo.Style.Colors_14`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L109) | 第 14 项 RGBA 值，对应 ImGuizmoColor 同值槽位。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-6efda608858515d6"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGuizmo.Style.Colors { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L110) | 借用当前结构中连续 15 项 RGBA 颜色。 | 属于当前值；Span 不复制底层字段。 | 禁止跨内存释放、移动或原生库退休保存借用。 |
| <a id="api-3b111fe4a3882018"></a>[`public Inno.Native.ImGuizmo.Style.Style(float translationLineThickness = 0, float translationLineArrowSize = 0, float rotationLineThickness = 0, float rotationOuterLineThickness = 0, float scaleLineThickness = 0, float scaleLineCircleSize = 0, float hatchedAxisLineThickness = 0, float centerCircleSize = 0, System.Numerics.Vector4* colors = null)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L112) | 按参数写入样式值；非空 colors 复制 15 个 Vector4。 | 值由当前调用方拥有；不调用 Native 构造函数。 | 省略参数为零值，不能代替原生默认样式构造。 |

### `Inno.Native.ImGuizmo.ImDrawList`

<!-- api: T:Inno.Native.ImGuizmo.ImDrawList d4e78f18699da1994ace65f1d022b55ea48f98bfd2285331a86e3730d2c758f5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ed59b4381c73dd06"></a>[`public struct Inno.Native.ImGuizmo.ImDrawList`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L147) | 仅表示当前 ABI 布局的不透明 draw list 占位。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |

### `Inno.Native.ImGuizmo.ImGuiContext`

<!-- api: T:Inno.Native.ImGuizmo.ImGuiContext 3766ebcc6d4ed67d02ea2551aae7c62a7edee8616c6cc2de13758dc7a7271c7e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-467a80e5c02db33e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L158) | 仅表示当前 ABI 布局的不透明 context 占位。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |

### `Inno.Native.ImGuizmo.ImGuiWindow`

<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindow 191384eb1b84f42b9324c78a4eb3c14b33b8b9f6d6d697a5fda50e88c5472921 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9d580248b5ef2e19"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L169) | 仅表示当前 ABI 布局的不透明 window 占位。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |

### `Inno.Native.ImGuizmo.StylePtr`

<!-- api: T:Inno.Native.ImGuizmo.StylePtr 96b0ea802e74eba6d456fec00991a5113e7589ff219409cbeeba1e685486cfc4 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.#ctor(Inno.Native.ImGuizmo.Style*) 3f429ce60b5ba11a9f5250d13638164d62fdaab9568995396faca16216a56f6b -->
<!-- api: F:Inno.Native.ImGuizmo.StylePtr.Handle 56948c9ddcc11d3e26686355ffa8ff0ec690e1067774eb3d66a0e75395fa15e3 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.IsNull 04e2b13d488f8b5e878eeef68010abfefd5612335919f9d2b585a89403aa896b -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.Null f12133e852f24de4c93656ce8264ad0499b5442f38cf62d687ebfcae22969776 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.Item(System.Int32) c007ae146aadd3a6dd7007a4320dc64d7b0d0bfc36449676682b2337a246422f -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.op_Implicit(Inno.Native.ImGuizmo.Style*)~Inno.Native.ImGuizmo.StylePtr a0e6d04216624052aca7cce67b1e04a94d2d61c8d096be7613257a28e14bfd51 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.op_Implicit(Inno.Native.ImGuizmo.StylePtr)~Inno.Native.ImGuizmo.Style* 066b3a992f08a708f548238a0cfa06dee30cc6e7c9a62ef7630029623e53276a -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.op_Equality(Inno.Native.ImGuizmo.StylePtr,Inno.Native.ImGuizmo.StylePtr) e1b1b1fa70b4a76aeabc22bb4da55f1223c2efef95887d44f4f0414e04c76845 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.op_Inequality(Inno.Native.ImGuizmo.StylePtr,Inno.Native.ImGuizmo.StylePtr) d82caf85ef9067fbd4954fe917de48fcbb9c110377a96ccd2eac159609d507c4 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.Equals(Inno.Native.ImGuizmo.StylePtr) d5085c11de6720a4ea2ef367146379682d4db3b12dfe4cf47a6527e7fe847310 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.Equals(System.Object) b6b7b6915acfa3e1a2a4ec3a80c77d8809cdbb6ccf7dcd2f3eef94bb69124943 -->
<!-- api: M:Inno.Native.ImGuizmo.StylePtr.GetHashCode 52b091a8874bcdbf616ded5a7dd136ab1697bfe02e451e1b68331dec7b5c6d26 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.TranslationLineThickness f75628a55ee2d24725324855e17149b8a6c55415a063ac21086efa83a7cbc76b -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.TranslationLineArrowSize 982667fdf9f87b6576cd61387ba28d273e4709da2ae9737b1e12e946f5c7e2bc -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.RotationLineThickness d68740a894dd5706cb620c655347a3e564e2f05cc90b6713d6b11d36201a34a4 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.RotationOuterLineThickness 2fb9446d90921ecf2608331283bda9181e3d4ee6ac035f94546a968aff6ea2e8 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.ScaleLineThickness 9f03225b2a508d5bbc3bdd0cf084f6b5d7c18de245ac0b8f74c0da3e8faf3395 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.ScaleLineCircleSize e349daa64ee0b5e8718e9adcb1765927a41c43f7862411f0b2a5045fb6a98087 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.HatchedAxisLineThickness b58fd4b2de7e7a779b3116c70c53ae78c594ec60e4c6025d03e81d8e01905ad5 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.CenterCircleSize 317cda2418798c2f3989bbbf027ad88616c32daba233d655cfdc7f437bf493b7 -->
<!-- api: P:Inno.Native.ImGuizmo.StylePtr.Colors d96d13dd3df37ddbc9a904d20437ca56b1b3910f7e13899efe38db5e34dafd04 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-890bfb78d9b65955"></a>[`public struct Inno.Native.ImGuizmo.StylePtr : System.IEquatable<Inno.Native.ImGuizmo.StylePtr>`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L177) | 不拥有内存的 Style 指针包装。 | ABI 类型和调用仅用于所属 presentation/native 边界。 | 不属于 Scene 或脚本公开协议；布局随明确目标生成。 |
| <a id="api-d7d1fbad0925f0fc"></a>[`public Inno.Native.ImGuizmo.StylePtr.StylePtr(Inno.Native.ImGuizmo.Style* handle)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L179) | 包装传入地址，不创建样式对象。 | 借用内存，不接管释放权。 | 不验证地址或生命周期。 |
| <a id="api-59cd7b2333fd67ae"></a>[`public Inno.Native.ImGuizmo.Style* Inno.Native.ImGuizmo.StylePtr.Handle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L184) | 读取或替换包装的 Style 地址。 | 不释放旧值，不延长新值生命周期。 | 更换地址不验证其来源。 |
| <a id="api-578ea58cc5773cc1"></a>[`public bool Inno.Native.ImGuizmo.StylePtr.IsNull { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L185) | 只判断地址是否为零。 | 无分配或释放。 | 非零不表示仍然存活。 |
| <a id="api-ecc0ea6c9f3ef406"></a>[`public static Inno.Native.ImGuizmo.StylePtr Inno.Native.ImGuizmo.StylePtr.Null { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L186) | 返回零地址包装。 | 无分配或释放。 | 非零不表示仍然存活。 |
| <a id="api-e533e9a0b5280f55"></a>[`public Inno.Native.ImGuizmo.Style Inno.Native.ImGuizmo.StylePtr.this[int index] { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L188) | 读写 Handle[index] 的完整样式值。 | 借用连续 Style 数组。 | 不检查真实容量、空指针或已释放地址。 |
| <a id="api-fae41b43e5340a21"></a>[`public static Inno.Native.ImGuizmo.StylePtr.implicit operator Inno.Native.ImGuizmo.StylePtr(Inno.Native.ImGuizmo.Style* handle)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L190) | 在 Style* 与 StylePtr 之间转换。 | 仅复制地址，不转移 owner。 | 不检查实际内存。 |
| <a id="api-ab755a3b20386ce5"></a>[`public static Inno.Native.ImGuizmo.StylePtr.implicit operator Inno.Native.ImGuizmo.Style*(Inno.Native.ImGuizmo.StylePtr handle)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L191) | 在 Style* 与 StylePtr 之间转换。 | 仅复制地址，不转移 owner。 | 不检查实际内存。 |
| <a id="api-484d83e4a1731c87"></a>[`public static bool Inno.Native.ImGuizmo.StylePtr.operator ==(Inno.Native.ImGuizmo.StylePtr left, Inno.Native.ImGuizmo.StylePtr right)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L192) | 比较包装的原生地址值。 | 不读取目标对象。 | 地址相等不能证明对象仍然有效。 |
| <a id="api-a42a33d276326270"></a>[`public static bool Inno.Native.ImGuizmo.StylePtr.operator !=(Inno.Native.ImGuizmo.StylePtr left, Inno.Native.ImGuizmo.StylePtr right)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L193) | 比较包装的原生地址值。 | 不读取目标对象。 | 地址相等不能证明对象仍然有效。 |
| <a id="api-542cf09ee3a562a4"></a>[`public bool Inno.Native.ImGuizmo.StylePtr.Equals(Inno.Native.ImGuizmo.StylePtr other)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L194) | 比较包装的原生地址值。 | 不读取目标对象。 | 地址相等不能证明对象仍然有效。 |
| <a id="api-1835b16530604ecd"></a>[`public override bool Inno.Native.ImGuizmo.StylePtr.Equals(object? obj)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L195) | 比较包装的原生地址值。 | 不读取目标对象。 | 地址相等不能证明对象仍然有效。 |
| <a id="api-8cd18cfe6737bc25"></a>[`public override int Inno.Native.ImGuizmo.StylePtr.GetHashCode()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L196) | 计算地址值的哈希。 | 不访问内存。 | 不能持久化为对象身份。 |
| <a id="api-1a9f18bbefaf7507"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.TranslationLineThickness { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L198) | 平移轴线宽。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-605f9c0e9dd8118d"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.TranslationLineArrowSize { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L199) | 平移轴箭头尺寸。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-e79635d40c79503f"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.RotationLineThickness { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L200) | 旋转弧线宽。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-a4a652b5e3e547a8"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.RotationOuterLineThickness { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L201) | 旋转外圈线宽。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-b468b3897a0397fb"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.ScaleLineThickness { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L202) | 缩放轴线宽。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-fa2aabcc1464e783"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.ScaleLineCircleSize { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L203) | 缩放轴端圆尺寸。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-dee32258b130a398"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.HatchedAxisLineThickness { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L204) | 背面轴虚线宽。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-9bb07ba5f502a191"></a>[`public ref float Inno.Native.ImGuizmo.StylePtr.CenterCircleSize { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L205) | 平移/缩放中心圆尺寸。 | 返回可写 ref，直接修改原生样式。 | 不检查指针；ref 的寿命不得超过原样式。 |
| <a id="api-0226b8e448bc820f"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGuizmo.StylePtr.Colors { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L206) | 借用当前结构中连续 15 项 RGBA 颜色。 | 指向原生样式内存，随原 owner 失效。 | 禁止跨内存释放、移动或原生库退休保存借用。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
