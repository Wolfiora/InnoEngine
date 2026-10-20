# Inno.Native.UI

[分类索引](README.md) · [Wiki 首页](../../README.md) · [UI Toolchain](Inno.Build.Toolchains.UI.md)

手写的 `Native/include/RmlUiRuntime.hpp` 是唯一语义边界：它用 PImpl 与中立 DTO 隐藏 RmlUi。`Bindings/bindings.props` 选择正式 SDK 配置，同一次生成将 facade 降为 C ABI，再输出独立的 `Native/Generated/` 与 `Generated/Bindings.cs`。桥接库使用固定的 RmlUi 6.0 源码与对应 Text/字体依赖，产物为配置专属 `inno-ui` 动态库。原生 handle 只由 UI adapter 持有；游戏脚本及渲染 Plugin 消费 `Inno.UI` 中立数据，不直接调用 ABI。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `UiNative` 覆盖 runtime/context/document 生命周期、DOM 修改、输入、字体、命名纹理、帧几何、裁剪、像素及事件复制；`InnoUi*` 数据结构只为 ABI 使用。桥接 status、领域 Result 和每次调用独立的错误对象分别返回；错误消息读取后通过对应 Release 释放，稳定 `Result` 不泄漏 RmlUi 名称。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/ui/<platform>`，缺库和 ABI 错误直接失败，不查询源码仓库。

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

internal static unsafe class NativeEventReader
{
    internal static string[] ReadTargetIds(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        ulong count = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        int status = UiNative.Raw.GetEventCount(runtime, context, &count, &result, &error);
        Check(status, result, error);
        string[] targets = new string[checked((int)count)];
        for (int index = 0; index < targets.Length; index++)
        {
            InnoUIRmlUiAdapterEventInfo info = default;
            status = UiNative.Raw.GetEventInfo(runtime, context, (ulong)index,
                (InnoUIRmlUiAdapterEventInfoNative*)&info, &result, &error);
            Check(status, result, error);
            int length = checked((int)info.TargetIdLength);
            byte[] bytes = new byte[checked(length + 1)];
            fixed (byte* destination = bytes)
            {
                status = UiNative.Raw.CopyEventTargetId(runtime, context, (ulong)index,
                    (sbyte*)destination, (ulong)bytes.Length, &result, &error);
                Check(status, result, error);
            }
            targets[index] = Encoding.UTF8.GetString(bytes.AsSpan(0, length));
        }
        status = UiNative.Raw.ClearEvents(runtime, context, &result, &error);
        Check(status, result, error);
        return targets;
    }

    private static void Check(
        int status,
        Result result,
        BgcsInnoRmluiError* error
    ) {
        try
        {
            if (status != 0 || error != null)
                throw new InvalidOperationException(error == null ? $"Bridge status: {status}"
                    : Marshal.PtrToStringUTF8((nint)UiNative.Raw.BgcsInnoRmluiErrorMessage(error)));
            if (result != Result.Success)
                throw new InvalidOperationException($"UI call failed: {result}");
        }
        finally
        {
            if (error != null)
                UiNative.Raw.BgcsInnoRmluiErrorRelease(error);
        }
    }
}
```

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 通过类型初始化装载对应配置的库。生成输出分别属于宿主 Generated/Bindings.cs 和目标 `artifacts/bindings/ui/browser-wasm/<generationFingerprint>/Generated/Bindings.cs`，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj`。共同领域与平台产品通过明确契约组合。

## 当前源码公开 API 清单

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: F:Inno.Native.UI.EventType.Blur e3aa28dc6e163c958229e9676087165148c39adb8e34aa907f9da9e476965694 -->
<!-- api: F:Inno.Native.UI.EventType.Change 351e6f4d36f709c14a0ea8bf1db6f037d91ab8c4d8f5c6a982d3188e09ee0068 -->
<!-- api: F:Inno.Native.UI.EventType.Click e9b288f547b84d0a7d29543461860cb3dedd479d1ce927b911fc1fe20b4cafac -->
<!-- api: F:Inno.Native.UI.EventType.Focus be58317450fab54048d4055f5c910aa16fffa7cf9650ffac43189e4fe8a163dc -->
<!-- api: F:Inno.Native.UI.EventType.MouseEnter 275b8510f955cb5063e4e9468ad71ee087b1d6777c9859be9a0ce0dc79cc08fe -->
<!-- api: F:Inno.Native.UI.EventType.MouseLeave 9d6d1366e7464d48e8cedbadddcdab2ef7930698e64ebc1f3f5bf6cdcdccd906 -->
<!-- api: F:Inno.Native.UI.EventType.Submit a1c0f94a4e26ab17db6e24025fc2977ac2451c6c5da47f4a0a14b73080cbaeb1 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.MeshId 56c51ed1a453a345e322e155efced799412a6c531e5154dc33b4cdd917cc04a7 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorEnabled a597f4bc118da3d27e86ee8e33dd2b7451cda85c7e7ffc9555f614401b4ad609 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorHeight 487b65dff5ea21d50d4b5e0d9bfc872b2c915b950ae9fb80442fe35d3048ea5c -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorWidth a0fe12e89d3c1c6b826fe1f48169366a395f5050999cd8257d46575867b99060 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorX f032320970091a699f75c3ded13a2dd8ee6803c3e06dc151919563a687d3f273 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorY 817ff2b8e357870790bb1a401be06008c68dfab7c8f6dcf463c9fa03eaaaa292 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.TextureId 56d5bb1aaf0ed0b6805535f21638148e42385de6dcacc779753d72e024d60f23 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Document 31eac0bf23b78873ae35a98368304aafa4736b0f07bafbc1d3c948948813f2c2 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.TargetIdLength b69cf13682c064cc4d97c48cb34c3c3bef4a59cc0eba56ed58e2df6e5a4a5ad2 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Type 2981c7fe32330da4a8b7a99f770fec5ebabde6109a16697a81080d9cd672bff3 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.CommandCount 534715250732eae94a26e95db0af331badb458ed0888a5f00445419276e866e9 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.MeshUpdateCount d7011a7b50cf665f26dbd9114fe680207f989b77c2748ce2a1c7f9b133c76975 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedMeshCount de0cacb16300b1c6c0dac8121938b493a297f043057b5559863c5dd803679d0e -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedTextureCount ed7c314d720ff43c73443239a92bada1af8654a16227eb76849d025073998164 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.TextureUpdateCount c4f6c811cca31faa2629dd0ad1afcac6d6fc6e9ea16c982c213082b1c25ae9bc -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Id ec0fd4d9f13a78853614466ea846fc05f8b05d6cf79533fa1d498cad9aeca300 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.IndexCount 367f1ae2927994048a7f38dc2233799b31d87a7d2c47f175863c48822a10bdfe -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Revision 78b7e1c43b398b6c32eaeca213fbdc0f78776e42bc4e4432de0b8ddbb8b0d8ec -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.VertexCount 8d4e5b7457971e53f552b08dc70d64dae9563f261704e343664a19e77ca622b1 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.ByteLength 64d2878b9cb222b77abe2a60ce2f993903a9a615f1e3802140e90d71f5b27b02 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Height de901da5dbaaff57724d89628168dcc83f34844a98316e25d2864e32a0d7cdef -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Id 22c75ff88e52f7041db89f9bdaddc8d4d46cb7d4628d856c47fe44806be35c48 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Revision 7978cff41bbca72a353a0de36d462e6f4e067b1b9e2b3ab8d58e3515ea779ab6 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Width b4480bbe74f4741f77d0d0d5e019ddfc4678e6ebaeb7a53420c91e90bbbdc664 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterVertex.Color 1d604507b0155f69a3ee7bd7be270ea162dea4cfecf6d4e674a2598c0704aa33 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterVertex.U bc802a07e11a4089ebeda707ceabf088fdf8e7555d31856e337c98a965e59310 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterVertex.V 97dbe4e1ab0752242ba741d8bfe3df683962b544dd1db2b36b40b13341e61970 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterVertex.X 5eb66eacc72f8a34a812df808d4d3ef14fb84d1bad44e52d7fe2d16ebf77d868 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterVertex.Y ac43a8d9b6e8eaa32f8e1f4ffcb54d4a2babf1b57e719db96edca6a76647441e -->
<!-- api: F:Inno.Native.UI.Result.BackendError 1559fd2c6838053a2b33bcc9f808d4f620196a7d1941b4ac6af039c72014f36b -->
<!-- api: F:Inno.Native.UI.Result.BufferTooSmall 14958e82a98b446a41372a126483e550efe39af27129db4f22d6ad22af64e5e0 -->
<!-- api: F:Inno.Native.UI.Result.InvalidArgument ff1078b4fd7d177569db37c40bd65afbc717e2923118d014494d5e6622854e15 -->
<!-- api: F:Inno.Native.UI.Result.InvalidHandle 544b578344a69170083194cf5ddfaddbe7402418481aeb16d934d008eb1640f1 -->
<!-- api: F:Inno.Native.UI.Result.NotFound f0b39b380ca168385c18d83a89f3bfa37b81e89715115e56603dbc20bd879eed -->
<!-- api: F:Inno.Native.UI.Result.OutOfMemory 35bd52db13510a8ecf1345c5a836c0b2ea1fbad91417fa4b5cb9c72861094b87 -->
<!-- api: F:Inno.Native.UI.Result.Success 657b650eb2082b649d24828b10619b2f9d82ae3a1932f1b5f75018ff348a57f9 -->
<!-- api: F:Inno.Native.UI.Result.UnknownError f1999b3df2587ffec0ec09b245b2be6bd04f6e32e1c4cf4e3cb15d9added455f -->
<!-- api: F:Inno.Native.UI.UiNative.NativeLibraryName ca36deaf0770599a057999700e22d7ee16760aca805980820fbb5b211b920a7f -->
<!-- api: F:Inno.Native.UI.UiNativeContext.NativeLibraryName ba88844eaca64f9f2fdfe33d65a93c28ba7ec93b9d121670d14a7460f64f6261 -->
<!-- api: M:Inno.Native.UI.UiNative.Dispose 1de823861d2e8776b52f0e655848e7406a3f293de780c5a08fef4f90bdd18edc -->
<!-- api: M:Inno.Native.UI.UiNative.DisposeAsync 44577bfa81e9e30f7dc50b2736c8cab9dad3e29b73abd7640e664fd62cd2bdb6 -->
<!-- api: M:Inno.Native.UI.UiNative.FromProvider(BGCS.Runtime.INativeModuleProvider) c62c896c70834929dbdd0ff4d3856479ee285d65daf0dfa7459962c89eb4a6d0 -->
<!-- api: M:Inno.Native.UI.UiNative.Load(System.String) 17aee27fd2b2d941ed3420aa03f9e1d1da03e19ff3cc3a430cdc293cdc9a380d -->
<!-- api: M:Inno.Native.UI.UiNativeContext.Dispose eb1922c9d7274e227da66e08383d02d2d64c6a8517045423ba60a98479f2d048 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.DisposeAsync c3327ad9b6c656e338499dad745b342f38b1309c5a8925d7d851b87388273b5b -->
<!-- api: M:Inno.Native.UI.UiNativeContext.FromProvider(BGCS.Runtime.INativeModuleProvider) a3079de76e7351bb0b37ed46f8bf906681d10ba715573b926448182453f64817 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.Load(System.String) 170f231b977ed906033479647f5e5b0dcb90eee74c4126caa770813a1817b341 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorMessage(Inno.Native.UI.BgcsInnoRmluiError*) c965eb131b036a6dcd7312bbb0ed43996c93da6900072486c75b48d51c72a738 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorRelease(Inno.Native.UI.BgcsInnoRmluiError*) 6a2e905d8b6b0a885cdf5141865963e7bb3ad70c65aca044b5eeb49ac7ffb596 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ClearEvents(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 52acde26c07b6d31618987314f7e834af660926c06899e575b20d8411d726bea -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CloseDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 09f7882c39bda1934ae52add32da9834e404366016fad35325a1297f575bbb3f -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyCommands(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) baac29adcb39bd08c686e6bfb3e4dccce97a9da9d858a673f0d14305e187a712 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyEventTargetId(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 48edd25a3b7d70146c5db462a03861813e6c459252f4c71418c09a522f5114f3 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyMeshIndices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.UInt32*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) fe08e08dae1cb7303eb3ae099f260152f46afeb72851b2f871128367f77291da -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyMeshVertices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterVertexNative*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) c2fa8fa334762a5afb2f73e91c38f31853362320d8e16c7161985f74b156a1c9 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedMeshes(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) d25f382ce46691b3b49ec37e1d861662b7f8cdb5f4bcfac204f4ebbbc84b746f -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedTextures(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) da819c5ab179441dee3237da427ce6eee4d2420baafd5d6d69e709e90c473150 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyTexturePixels(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.Byte*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 804048f4507cbe7b67ac033271e458b84315735e34c3ef89eca1b7592f39525a -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CreateContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.SByte*,System.Int32,System.Int32,System.Single,System.UInt64*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) dd5479a921fc0536a85ce6e3a4c31a79386d6cbf0a147bb0c6db72a70f002d25 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CreateRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime**,Inno.Native.UI.BgcsInnoRmluiError**) 95ef96521ae047a656c71f23a180218cbda1213e4812d76b6cf01afeb7715cac -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.DestroyContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 022ce3e3913a752fd416f1ad78a0d1a4d0c1913a8feff34c0c22f64109b776b7 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.DestroyRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,Inno.Native.UI.BgcsInnoRmluiError**) 35d48a3047265ae8d94e6aa94563b8b77cca9d30fc7f7cff2dea2bb71cee4c21 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.FinishFrame(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 4ed2f7d391b40e997217e4ba74b4789fd06c43c78ee85ccc8c9f00367685eaa1 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetEventCount(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 4ebf0c951e7d0aefa04b774b9b199430f178f1d022e5783fdf20788738487bf1 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetEventInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 8305bfd091b4880933c5dee4da52eabc29a4a396ce4b57ca2f43175add0b6539 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetMeshInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) b04f04e2dfd8022f1f206ec9524634dc885cb90a32e54b4dd8452fe487d4cc92 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetTextureInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) ffbc8e5be9fe20f7dc89b4a694c681a0dd4eaf2be0b52b169c2451f1505f5b98 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.HasElementAtPoint(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Int32,System.Int32,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) abc329f23a217dcc85456151634325e7414eaffb9bfeae4f7306bb477aec5a0b -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.HideDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 2d0b8a5dbe4845296858699cc01f54519d070b7878902fe5ad0f4d1de20a073c -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.LoadDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.SByte*,System.SByte*,System.UInt64*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) b0ffa19757beccd14675a5a9fdb0cfaed99cd3a7087a6a738a02a9844cda4735 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.LoadFont(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.Byte*,System.UInt64,System.SByte*,System.Int32,System.Int32,System.Byte,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) ace6c7249556ead7cac14c3780ff4cbc2d2d3e3efb4d8d7c86534a9e1c668a32 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ProcessKey(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Int32,System.Byte,System.Int32,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) e3a5ce0fdb8867c3fa35e4a0b7884cc7f0b29c2626799432007d362535007509 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseButton(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Int32,System.Byte,System.Int32,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) a4049369db5b18a6d029d4b54f72b8ef24926bbff2232689550c23fff13e729a -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseMove(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Int32,System.Int32,System.Int32,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) e35b8e4fd8e194312f932071c47ccc76f374bf50a5edb7ea5bd0f1efb487b4f6 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseWheel(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Single,System.Single,System.Int32,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) b1a417774f138a944407a834453c1ad7c304036ab88d43722a95551bb7bd9f56 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ProcessText(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.SByte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) d531a0cb4a01de4ff33f52726564f198b3ffb6d3b2dc0ded8bc8c646de9c25b7 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.RegisterTexture(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.SByte*,System.Int32,System.Int32,System.Byte*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 505a275513e2abddf16415c9c11cd371a389f4d87d311bc443710a3f5e64c6d0 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.Render(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 90106ff41aa6437f444f03e4b9b9084f86c2411499fc8131e66576e5bbf5b35b -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetAttribute(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.SByte*,System.SByte*,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) eeab440cf01d1f49f40a9ba265251697e9c130d1e0d45bbf44653285e694546e -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetClass(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.SByte*,System.Byte,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 3ba0ed82b2e0140755fe2fed94b0b2ce930a1c0bd21db3cf2077b6f2dcd6f678 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetInnerMarkup(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.SByte*,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) e4e457cb3a792d64f5275b74db3b1b13d24b1df963e6058b90ce5ed7a8c72b75 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetViewport(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Int32,System.Int32,System.Single,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 6215c918172d7203ff0a4b65b44e08b85c5ca7c56acc669d82a98d019c5b2b1d -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ShowDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 23f9ad83a60a2811b76d312816f1481714d336913701ff74ab1cf3ed9395adb5 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.Update(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 442781c339430d7bfdc7609f8062c1e8bb9d22ae733c86cb4c0c71b93e71bbe7 -->
<!-- api: P:Inno.Native.UI.UiNative.IsInitialized f3a8f44f3b008db9dc99592cd803174ed6b9860a7cc361bc5f8a8d17f33453e2 -->
<!-- api: P:Inno.Native.UI.UiNative.Raw 753e15d9358c88d5f7b821e55df35e25bb05ef35e4c3d813d41c0cf5e27d05f7 -->
<!-- api: P:Inno.Native.UI.UiNativeContext.Raw 31262f51102093de8c3bf58612667e8d334b478d0f40c61c7f145c103b0b6560 -->
<!-- api: T:Inno.Native.UI.BgcsInnoRmluiError 2cb416c018e512a62f18f806f0c5b5e57c5ff143a87c4963238d3b819d1b9d56 -->
<!-- api: T:Inno.Native.UI.EventType 2f80f073b856a84094a89fa5d93f06843f00fa9273f9d4c08b8268c5e82c176c -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand e6f307873cd3fdac87740e76c6560c194e79a4b1f15950e9ff6e920d3c382192 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative 471d9a024af4c809b0113b6fc4c5361859f2fdf0cbd3179b6937d2e06ca5d1bd -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo 2f868076ee07ad97519a01666311d3a6937c75002a1c7649b7a213d1204b3b03 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative 8da60e3f63263e542ba9ae4360be2c3d3e76690c882285d4e08771c618bfae06 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo fc0e7b560445c9b47b07d9d195b64635bea755183ce741f150ff11d0f3a7c963 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative 1bc6ad704905df21dd71c7a7179e22b06880128f4ca3d8305d0628b5b7aeee0a -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo 2879fcbf9e6e295c74276cda7e12d9f171b5b3a618978f651955331ff2f72465 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative dbbea13f8c0ac423db44fa8bd0ffd6840105534b3c5504e188290aa3e31071cd -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterRuntime ac79b2caadd822ffff1c5571e64c5aab8db32d1a1ce860c9d0118a4eebf9d899 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo 79642fc0fb7495d16c91141e6752080cb88c3091450665e6880bbf208b418bec -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative 65e7d4c09000f43d1961589e5da5e4f8ad03e133dd916897f28510d4025b799c -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterVertex bb0cdc7057563026b4d2421faacab619c5f1df166bc3b7bddae94e2450ab6786 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterVertexNative 04492cbdf33ee552854260d66776c7fa00fca0a0154e0f55dcfd63e60ee2cc70 -->
<!-- api: T:Inno.Native.UI.NativeArray_78bd09dc87a774a8 7ef1d999dc4cb0860f300ee5972a30642497b788f8a24cdd0b2cd9baa3067a08 -->
<!-- api: T:Inno.Native.UI.NativeArray_eefffd4ad3ec208d 69efea17661a89f6a6942f30857639c0e10cbef291f9f4998cc11f15888b8f5c -->
<!-- api: T:Inno.Native.UI.Result ca8cb21780d4dad4374614d0ef430a99f52d548fbae496a17ebd34b6c2d84cba -->
<!-- api: T:Inno.Native.UI.UiNative 059837a6d7a0fd2d5b2ed85dbbb07f414a4130d6dffc3f4fabc1b15b4a6fe815 -->
<!-- api: T:Inno.Native.UI.UiNativeContext d0ed8f663548a97a88d2e66aad2b0b43671ba7810bf802d46f3e97d19d49f975 -->
<!-- api: T:Inno.Native.UI.UiNativeContext.RawApi e0f413b31854f28056c28e7528ea2f6c5d1c7ce93dee6d1223951f7fb4b61120 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-dad13bc0b36feadb"></a>[`Inno.Native.UI.EventType.Blur = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-1b4fe1edab86a903"></a>[`Inno.Native.UI.EventType.Change = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-2b5d3f3423dded4d"></a>[`Inno.Native.UI.EventType.Click = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-e0b02200c3751f41"></a>[`Inno.Native.UI.EventType.Focus = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-6d4f2e00a6be16f7"></a>[`Inno.Native.UI.EventType.MouseEnter = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-c7608aa4764881de"></a>[`Inno.Native.UI.EventType.MouseLeave = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-42d86732878ba8d8"></a>[`Inno.Native.UI.EventType.Submit = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-9bdfce379aea994f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.MeshId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L588) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a92f947792e6324f"></a>[`public byte Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorEnabled`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e5c7631eb176e03"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53d507dc407371a0"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d690aec7e3d036c"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L594) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b2a09cc7176c6f1"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L596) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81435a810be6c061"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.TextureId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0bfd2c446d2a1c7f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Document`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L627) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5cb9127c0821b604"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.TargetIdLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L629) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f60f3f8b2a545c30"></a>[`public Inno.Native.UI.EventType Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Type`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L625) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1369f22560d2a1ca"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.CommandCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L658) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14db3561a9bf3246"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.MeshUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e1261aa7cf6f233a"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedMeshCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c7724b251bb4799"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedTextureCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L662) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51d2294182448922"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.TextureUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L660) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58e195eae19b203d"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L687) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-417dda2d94944385"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.IndexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc74cc4eba0a65b0"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L689) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdf5031047554a4c"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.VertexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b9ed1da3e830635"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.ByteLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15b3f09011c6407f"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Height`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b338dc38bfa36a0f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L729) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46c4c6823b076285"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L731) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc1e8a4d684e7f84"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Width`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L733) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c074c69a8583920"></a>[`public uint Inno.Native.UI.InnoUIRmlUiAdapterVertex.Color`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22bad3b1637b0781"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.U`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-12ff79ca32f3de91"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.V`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8aca8c4aea829dfe"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.X`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-148c365aaead8e25"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.Y`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-19ff102aca8b09cd"></a>[`Inno.Native.UI.Result.BackendError = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-2d0397ea5a753974"></a>[`Inno.Native.UI.Result.BufferTooSmall = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L822) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-74b18616f0b88211"></a>[`Inno.Native.UI.Result.InvalidArgument = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ef365a894eb0e71a"></a>[`Inno.Native.UI.Result.InvalidHandle = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L820) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-262c75b0184500df"></a>[`Inno.Native.UI.Result.NotFound = 7`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L823) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-62bdc53d4fb303a4"></a>[`Inno.Native.UI.Result.OutOfMemory = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L819) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-9daaf069fb747de5"></a>[`Inno.Native.UI.Result.Success = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L817) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-9b85460337e65c4c"></a>[`Inno.Native.UI.Result.UnknownError = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-c61e1003fdf1d1fe"></a>[`public const string Inno.Native.UI.UiNative.NativeLibraryName = "inno-ui"`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L18) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`inno-ui` |
| <a id="api-5f122b63ffea322e"></a>[`public const string Inno.Native.UI.UiNativeContext.NativeLibraryName = "inno-ui"`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L48) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`inno-ui` |
| <a id="api-1115cb6aed213116"></a>[`public static void Inno.Native.UI.UiNative.Dispose()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L32) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b84366b008dee9b"></a>[`public static System.Threading.Tasks.ValueTask Inno.Native.UI.UiNative.DisposeAsync()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L33) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0619901bbc44d6d3"></a>[`public static void Inno.Native.UI.UiNative.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L30) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7a126b4562aed86"></a>[`public static void Inno.Native.UI.UiNative.Load(string libraryPath = "inno-ui")`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L31) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13b90c296dd199c8"></a>[`public void Inno.Native.UI.UiNativeContext.Dispose()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L225) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| <a id="api-d830a5aaeb37e379"></a>[`public System.Threading.Tasks.ValueTask Inno.Native.UI.UiNativeContext.DisposeAsync()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L226) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.<br>返回：A task that represents the asynchronous dispose operation. |
| <a id="api-c630ec0176a9efa6"></a>[`public static Inno.Native.UI.UiNativeContext Inno.Native.UI.UiNativeContext.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec23f0b75bb81eee"></a>[`public static Inno.Native.UI.UiNativeContext Inno.Native.UI.UiNativeContext.Load(string libraryPath = "inno-ui")`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a362ffaada7c8689"></a>[`public sbyte* Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorMessage(Inno.Native.UI.BgcsInnoRmluiError* error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L541) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ec5d82ff4d26bc1"></a>[`public void Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorRelease(Inno.Native.UI.BgcsInnoRmluiError* error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83ea297bbd38636e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ClearEvents(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c58ec687dd1b9df"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CloseDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L261) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-57c9719d5baf6b5f"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyCommands(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative* commands, ulong command_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L269) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8d5fc9eb1adbeb6"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyEventTargetId(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, sbyte* target_id, ulong target_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L277) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42ae656fc7cb5c85"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyMeshIndices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong mesh, uint* indices, ulong index_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L285) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c48cff10b33035e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyMeshVertices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong mesh, Inno.Native.UI.InnoUIRmlUiAdapterVertexNative* vertices, ulong vertex_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L293) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c4fad6ee3d1f47b"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedMeshes(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* meshes, ulong mesh_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L301) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3efa47dc92daa6a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedTextures(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* textures, ulong texture_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8831d85bb497ea40"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyTexturePixels(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong texture, byte* pixels, ulong pixel_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L317) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0c61c9a1682d5b5"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CreateContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, sbyte* name, int width, int height, float density, ulong* context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a508bc8798f2ade"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CreateRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime** bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f632cd3ddb870f1"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.DestroyContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L333) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9e3f25a24e22fe8"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.DestroyRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b8f529d32dd6b43"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.FinishFrame(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9510cbc8a546595d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetEventCount(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* count, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a33aa8aba489a7a0"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetEventInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative* event_info, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L357) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f077fc88cb87a2a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetMeshInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative* mesh, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bd865b83c74be81"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetTextureInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative* texture, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9081d9ea46717f2"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.HasElementAtPoint(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int x, int y, byte* hit, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ddc5d1fc7fb292c"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.HideDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fee3372bf58f69af"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.LoadDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* markup, sbyte* source_url, ulong* document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b09e1b982ca891d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.LoadFont(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, byte* data, ulong data_length, sbyte* family, int style, int weight, byte fallback, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L405) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8bfa5c5e3cd2268"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessKey(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int key, byte down, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L413) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8870e2b14ed6a4d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseButton(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int button, byte down, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8e37ea651671637"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseMove(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int x, int y, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a61a7d61f4162d14"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseWheel(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, float x, float y, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54c1361e290ab73e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessText(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* text, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6cd63b9c9063eb00"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.RegisterTexture(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* source, int width, int height, byte* pixels, ulong pixel_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1a345719b691682"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.Render(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative* frame, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L461) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e727ac8ae6fd495"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetAttribute(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* name, sbyte* value, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L477) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-753103148f25a7dc"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetClass(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* class_name, byte active, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L485) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-375d83bfdbc81e00"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetInnerMarkup(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* markup, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8a3a33c67aa489a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetViewport(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int width, int height, float density, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9788b1e05fd14801"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ShowDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abcd307dd41fea97"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.Update(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f353b2ebefa19b59"></a>[`public static bool Inno.Native.UI.UiNative.IsInitialized { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L21) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3909f9a7ddbc175f"></a>[`public static Inno.Native.UI.UiNativeContext.RawApi Inno.Native.UI.UiNative.Raw { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L22) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37b5e71826627620"></a>[`public Inno.Native.UI.UiNativeContext.RawApi Inno.Native.UI.UiNativeContext.Raw { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L87) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d10fc4ae112558de"></a>[`public struct Inno.Native.UI.BgcsInnoRmluiError`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fee19cdc2f45b81a"></a>[`public enum Inno.Native.UI.EventType : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e921798e9e0377fe"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L585) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84f836f74b13c3ac"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59ef2c7f99d6d446"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterEventInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e4b9c9dc6247449"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5162e7195f294974"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L651) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c0ad05fe2111a32"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L672) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b25e45369623984"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L684) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b8e407410611e0a"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L703) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b33b1f1a8c5d72e"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterRuntime`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L714) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba257466669d0f42"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d708d9b0dc1300de"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L747) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2fac75dfcc960a3"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterVertex`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08e714ad007ce58a"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterVertexNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef6c4c983512f4bb"></a>[`public struct Inno.Native.UI.NativeArray_78bd09dc87a774a8`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L791) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c845623293755c2"></a>[`public struct Inno.Native.UI.NativeArray_eefffd4ad3ec208d`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f9ba524814c9b04"></a>[`public enum Inno.Native.UI.Result : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L814) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-425b7a996d4e627e"></a>[`public static class Inno.Native.UI.UiNative`](../../../backends/RmlUi/native/Inno.Native.UI/UiNative.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API. |
| <a id="api-ac337dc28ea72070"></a>[`public sealed class Inno.Native.UI.UiNativeContext : System.IDisposable, System.IAsyncDisposable`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L46) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a38141ba0e7b125a"></a>[`public sealed class Inno.Native.UI.UiNativeContext.RawApi`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L247) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
