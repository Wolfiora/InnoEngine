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

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/ui/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)为准。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj`。共同领域与平台产品通过明确契约组合。

## 当前源码公开 API 清单

当前签名由正式源码语义清单核对；生命周期与错误约束见前文及对应 XML。

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

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-dad13bc0b36feadb"></a>[`Inno.Native.UI.EventType.Blur = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L572) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b4fe1edab86a903"></a>[`Inno.Native.UI.EventType.Change = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L569) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b5d3f3423dded4d"></a>[`Inno.Native.UI.EventType.Click = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L568) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e0b02200c3751f41"></a>[`Inno.Native.UI.EventType.Focus = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L571) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d4f2e00a6be16f7"></a>[`Inno.Native.UI.EventType.MouseEnter = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L573) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7608aa4764881de"></a>[`Inno.Native.UI.EventType.MouseLeave = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L574) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42d86732878ba8d8"></a>[`Inno.Native.UI.EventType.Submit = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L570) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9bdfce379aea994f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.MeshId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L588) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a92f947792e6324f"></a>[`public byte Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorEnabled`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L592) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8e5c7631eb176e03"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L600) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-53d507dc407371a0"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L598) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2d690aec7e3d036c"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L594) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b2a09cc7176c6f1"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.ScissorY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L596) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-81435a810be6c061"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand.TextureId`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L590) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bfd2c446d2a1c7f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Document`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L627) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5cb9127c0821b604"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.TargetIdLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L629) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f60f3f8b2a545c30"></a>[`public Inno.Native.UI.EventType Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Type`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L625) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1369f22560d2a1ca"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.CommandCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L658) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-14db3561a9bf3246"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.MeshUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L654) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1261aa7cf6f233a"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedMeshCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L656) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c7724b251bb4799"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.ReleasedTextureCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-51d2294182448922"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo.TextureUpdateCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L660) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-58e195eae19b203d"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L687) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-417dda2d94944385"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.IndexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L693) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc74cc4eba0a65b0"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L689) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cdf5031047554a4c"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo.VertexCount`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L691) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6b9ed1da3e830635"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.ByteLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L737) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-15b3f09011c6407f"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Height`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L735) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b338dc38bfa36a0f"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Id`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L729) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-46c4c6823b076285"></a>[`public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L731) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc1e8a4d684e7f84"></a>[`public int Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.Width`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L733) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c074c69a8583920"></a>[`public uint Inno.Native.UI.InnoUIRmlUiAdapterVertex.Color`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L770) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22bad3b1637b0781"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.U`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L766) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12ff79ca32f3de91"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.V`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L768) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8aca8c4aea829dfe"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.X`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L762) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-148c365aaead8e25"></a>[`public float Inno.Native.UI.InnoUIRmlUiAdapterVertex.Y`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L764) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-19ff102aca8b09cd"></a>[`Inno.Native.UI.Result.BackendError = 5`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L821) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2d0397ea5a753974"></a>[`Inno.Native.UI.Result.BufferTooSmall = 6`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L822) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74b18616f0b88211"></a>[`Inno.Native.UI.Result.InvalidArgument = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L818) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef365a894eb0e71a"></a>[`Inno.Native.UI.Result.InvalidHandle = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L820) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-262c75b0184500df"></a>[`Inno.Native.UI.Result.NotFound = 7`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L823) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62bdc53d4fb303a4"></a>[`Inno.Native.UI.Result.OutOfMemory = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L819) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9daaf069fb747de5"></a>[`Inno.Native.UI.Result.Success = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L817) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b85460337e65c4c"></a>[`Inno.Native.UI.Result.UnknownError = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L816) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c61e1003fdf1d1fe"></a>[`public const string Inno.Native.UI.UiNative.NativeLibraryName = "inno-ui"`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L18) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5f122b63ffea322e"></a>[`public const string Inno.Native.UI.UiNativeContext.NativeLibraryName = "inno-ui"`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L48) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1115cb6aed213116"></a>[`public static void Inno.Native.UI.UiNative.Dispose()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L32) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b84366b008dee9b"></a>[`public static System.Threading.Tasks.ValueTask Inno.Native.UI.UiNative.DisposeAsync()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L33) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0619901bbc44d6d3"></a>[`public static void Inno.Native.UI.UiNative.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L30) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7a126b4562aed86"></a>[`public static void Inno.Native.UI.UiNative.Load(string libraryPath = "inno-ui")`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L31) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13b90c296dd199c8"></a>[`public void Inno.Native.UI.UiNativeContext.Dispose()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L225) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| <a id="api-d830a5aaeb37e379"></a>[`public System.Threading.Tasks.ValueTask Inno.Native.UI.UiNativeContext.DisposeAsync()`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L226) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.<br>返回：A task that represents the asynchronous dispose operation. |
| <a id="api-c630ec0176a9efa6"></a>[`public static Inno.Native.UI.UiNativeContext Inno.Native.UI.UiNativeContext.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L223) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec23f0b75bb81eee"></a>[`public static Inno.Native.UI.UiNativeContext Inno.Native.UI.UiNativeContext.Load(string libraryPath = "inno-ui")`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L224) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a362ffaada7c8689"></a>[`public sbyte* Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorMessage(Inno.Native.UI.BgcsInnoRmluiError* error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L541) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ec5d82ff4d26bc1"></a>[`public void Inno.Native.UI.UiNativeContext.RawApi.BgcsInnoRmluiErrorRelease(Inno.Native.UI.BgcsInnoRmluiError* error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L533) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83ea297bbd38636e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ClearEvents(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L253) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c58ec687dd1b9df"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CloseDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L261) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-57c9719d5baf6b5f"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyCommands(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative* commands, ulong command_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L269) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8d5fc9eb1adbeb6"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyEventTargetId(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, sbyte* target_id, ulong target_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L277) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42ae656fc7cb5c85"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyMeshIndices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong mesh, uint* indices, ulong index_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L285) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c48cff10b33035e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyMeshVertices(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong mesh, Inno.Native.UI.InnoUIRmlUiAdapterVertexNative* vertices, ulong vertex_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L293) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c4fad6ee3d1f47b"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedMeshes(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* meshes, ulong mesh_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L301) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3efa47dc92daa6a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyReleasedTextures(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* textures, ulong texture_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8831d85bb497ea40"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CopyTexturePixels(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong texture, byte* pixels, ulong pixel_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L317) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0c61c9a1682d5b5"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CreateContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, sbyte* name, int width, int height, float density, ulong* context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L325) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4a508bc8798f2ade"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.CreateRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime** bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L469) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f632cd3ddb870f1"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.DestroyContext(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L333) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9e3f25a24e22fe8"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.DestroyRuntime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L525) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5b8f529d32dd6b43"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.FinishFrame(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L341) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9510cbc8a546595d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetEventCount(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* count, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L349) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a33aa8aba489a7a0"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetEventInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative* event_info, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L357) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f077fc88cb87a2a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetMeshInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative* mesh, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L365) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8bd865b83c74be81"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.GetTextureInfo(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative* texture, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9081d9ea46717f2"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.HasElementAtPoint(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int x, int y, byte* hit, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L381) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ddc5d1fc7fb292c"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.HideDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L389) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fee3372bf58f69af"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.LoadDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* markup, sbyte* source_url, ulong* document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L397) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b09e1b982ca891d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.LoadFont(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, byte* data, ulong data_length, sbyte* family, int style, int weight, byte fallback, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L405) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e8bfa5c5e3cd2268"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessKey(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int key, byte down, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8870e2b14ed6a4d"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseButton(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int button, byte down, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L421) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e8e37ea651671637"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseMove(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int x, int y, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a61a7d61f4162d14"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessMouseWheel(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, float x, float y, int modifiers, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L437) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-54c1361e290ab73e"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ProcessText(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* text, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6cd63b9c9063eb00"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.RegisterTexture(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* source, int width, int height, byte* pixels, ulong pixel_capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L453) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1a345719b691682"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.Render(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative* frame, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L461) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e727ac8ae6fd495"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetAttribute(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* name, sbyte* value, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L477) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-753103148f25a7dc"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetClass(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* class_name, byte active, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L485) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-375d83bfdbc81e00"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetInnerMarkup(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, sbyte* markup, byte* changed, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L493) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8a3a33c67aa489a"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.SetViewport(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, int width, int height, float density, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L501) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9788b1e05fd14801"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.ShowDocument(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L509) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-abcd307dd41fea97"></a>[`public int Inno.Native.UI.UiNativeContext.RawApi.Update(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L517) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f353b2ebefa19b59"></a>[`public static bool Inno.Native.UI.UiNative.IsInitialized { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L21) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3909f9a7ddbc175f"></a>[`public static Inno.Native.UI.UiNativeContext.RawApi Inno.Native.UI.UiNative.Raw { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L22) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-37b5e71826627620"></a>[`public Inno.Native.UI.UiNativeContext.RawApi Inno.Native.UI.UiNativeContext.Raw { get; }`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L87) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d10fc4ae112558de"></a>[`public struct Inno.Native.UI.BgcsInnoRmluiError`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fee19cdc2f45b81a"></a>[`public enum Inno.Native.UI.EventType : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L566) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e921798e9e0377fe"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterDrawCommand`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-84f836f74b13c3ac"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterDrawCommandNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L610) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-59ef2c7f99d6d446"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterEventInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L622) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e4b9c9dc6247449"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterEventInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L639) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5162e7195f294974"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterFrameInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L651) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c0ad05fe2111a32"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterFrameInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L672) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b25e45369623984"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterMeshInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L684) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8b8e407410611e0a"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterMeshInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L703) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b33b1f1a8c5d72e"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterRuntime`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L714) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba257466669d0f42"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L726) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d708d9b0dc1300de"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterTextureInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L747) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c2fac75dfcc960a3"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterVertex`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L759) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08e714ad007ce58a"></a>[`public struct Inno.Native.UI.InnoUIRmlUiAdapterVertexNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L780) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef6c4c983512f4bb"></a>[`public struct Inno.Native.UI.NativeArray_78bd09dc87a774a8`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L791) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c845623293755c2"></a>[`public struct Inno.Native.UI.NativeArray_eefffd4ad3ec208d`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L803) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f9ba524814c9b04"></a>[`public enum Inno.Native.UI.Result : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L814) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-425b7a996d4e627e"></a>[`public static class Inno.Native.UI.UiNative`](../../../backends/RmlUi/native/Inno.Native.UI/UiNative.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API.<br>Owns one explicitly shared context. Replacement closes the previous context; existing resource leases retain its native cleanup capability. |
| <a id="api-ac337dc28ea72070"></a>[`public sealed class Inno.Native.UI.UiNativeContext : System.IDisposable, System.IAsyncDisposable`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L46) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a38141ba0e7b125a"></a>[`public sealed class Inno.Native.UI.UiNativeContext.RawApi`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L247) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。

### Context 值、焦点、布局与时间 ABI

以下成员由正式 BGCS 生成，供具体 RmlUi adapter 使用，未导出到游戏脚本。所有 Runtime 指针与缓冲区仅在所属线程的当前调用期间借用。Raw 方法同时返回 BGCS 桥接状态、领域 Result 与调用私有错误对象；按上文释放错误，不保存跨帧指针。

<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.AdvanceTime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Double,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 0b0fef97d944daba9d3d8148e211c59207e4b67e1e59d21a74b751a7c8cd6b20 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ClearFocus(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 41caf650edb10e674b69ff1205a579b0ff9edf901129876d4944634cfd86464a -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.CopyEventValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 47bb41adfc7224f3b213b8dba5c1686434d4523506a9ed38a71db5526900ae14 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.FocusElement(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,Inno.Native.UI.FocusOutcome*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 9e017c4db82b6ff0698c92f55ed8566af32c9b65d782612aefe9e5f4f50312c7 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetFocusedElement(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64*,System.SByte*,System.UInt64,System.UInt64*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 321bb85d04ca732e5710a874adeabd401f8533902d96f44f7c6a380d6bf29324 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetTime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Double*,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) c9a095807cf5aa75e6a17b752cca3aab35a88bf993a324d101a104ca2b65cdb3 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.GetValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfoNative*,System.SByte*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 5dbcb734c98d7ba8ea3994c88cbd030bf6fd8837033eb96033ed3e49072f180b -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.PrepareLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) eae6e1e08f7886a5a8f9231fd4b3ec76d3e292a447f3a9a27f2984fc9303a8d8 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.ReadLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,System.UInt64,Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfoNative*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) b92fdb695d06188562e1f41359e56a0d97335a18c0b361b52911b2c82000c859 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.RefreshLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 09b4f65fcb3a7f4746d1729bfa55aceeea00f441121d48eb7a7f4002cf7d560b -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetTimePaused(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.Byte,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) bf6a621e3bd4485b67f12198e13879dfd275f365cfc476daaf614fa0b39fd72c -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.SetValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.UInt64,System.SByte*,Inno.Native.UI.ControlValueKind,System.SByte*,System.Double,System.Byte,System.Byte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 8fb8ee7a195990da40a582baebef66ec08037e5ccb4c17a0628c83f8ba314cc0 -->
<!-- api: T:Inno.Native.UI.ControlValueKind 66e4726b6775270956f5d2331e3ee35f994d9f163da79142db941ee86555332c -->
<!-- api: F:Inno.Native.UI.ControlValueKind.None 72441750ae83aa3d4211f1013811bf4532a99e39e8748ad07636da1fedf1e94b -->
<!-- api: F:Inno.Native.UI.ControlValueKind.Text fbd7384c55b1a64f07d6b3f645a37b9f4ba652028fda09a139b228adea088ce2 -->
<!-- api: F:Inno.Native.UI.ControlValueKind.Boolean 24c387be2c9bdb2d46b38db7d9c7fc0c6960d6985b28c93a48bf5c97cc211b11 -->
<!-- api: F:Inno.Native.UI.ControlValueKind.Number 6c90f3f3602b1b5e4909e7e4c79a772e69c28a55edb2a0ca16dad8f8ede51833 -->
<!-- api: F:Inno.Native.UI.ControlValueKind.Selection b92a8d5c037f7d6117c042448d3c6e139921dddd80ced4ca4b17a48b1261b222 -->
<!-- api: T:Inno.Native.UI.EditPhase f094da4ab4242354fd72570e873a5058dc6c2e513073baece5560a4078ba3527 -->
<!-- api: F:Inno.Native.UI.EditPhase.None fc9f7b42ee0198279033cafa0420f9257e7a0a810b121a03dbed32551e038bdc -->
<!-- api: F:Inno.Native.UI.EditPhase.Editing 8e70d8f3b728dcad05566add25e0ec925a72c856f29bed29291ed9297362aaea -->
<!-- api: F:Inno.Native.UI.EditPhase.Committed 9a5116c3e5ce81e15530fdbda5ce6bf20f7685fc7fdcc427042699e633882a2a -->
<!-- api: T:Inno.Native.UI.EventOrigin f7fd739b527e57b3b293a3a0c672ea6984a62662be7555f7a6e56975b6c585e5 -->
<!-- api: F:Inno.Native.UI.EventOrigin.Backend f1712d7f57fc2fb23f9e5b3860a55c30582e4fce362f5c67076ab5d7464476ab -->
<!-- api: F:Inno.Native.UI.EventOrigin.UserInput 9c603e4b6adf630ef8b590cf43300a9ac60e831ca21462708c20c7bc11bfa8f8 -->
<!-- api: F:Inno.Native.UI.EventOrigin.Programmatic 3026a9768c0124f5bf4396897eca43059f8d13b6dad5e3575e5af78ba7c81be8 -->
<!-- api: T:Inno.Native.UI.FocusOutcome 9c2f0e9335633733f1c878fd1f872a0ab968937889756aaac7878574524081d6 -->
<!-- api: F:Inno.Native.UI.FocusOutcome.Focused 863f89fefa0219d18b2b480f3ccc3837aeb4907313ed8bbbc514b5285b6adfff -->
<!-- api: F:Inno.Native.UI.FocusOutcome.Unchanged a15462d9e6367149461360ccc546fdf3c6c8fe5b9c6775a321f1c344c5b85f00 -->
<!-- api: F:Inno.Native.UI.FocusOutcome.NotFound fa9bf54741cb6b3dd5bf91363970f9fc292edc35814cd30c0134c34ea68df00f -->
<!-- api: F:Inno.Native.UI.FocusOutcome.NotFocusable 444042dffb0e8f33f2c7b925da6a0405b40fd787300ac3e1e70081abe0235484 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo 63451a7827cbc27cc99ece581d04586462f75f92c3202b0063d6bcc57afc3c79 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Kind 1054f8bc9d1b308d914a0589151ed35e1214fea7b0d382dea91ad481465efb7e -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Number e61d7aadc6386a1484c3644909dd28b06ae08920c1abdfd605dd2c031c7856cd -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Checked d1907d22bd8154652cede0f30f5361c15d6e0d6e61460176322b5acb6bc2b769 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.TextLength af25293afa63267b1815a0a08f321c2cc50f3e389c3614f938d3a759e020272e -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfoNative 6a41696f911b1103a5e5b5eb9772988b6e7625fffecb0efdfe52c80461f0783c -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo 66939115617ecac05f867a79c66ba4ac838454f62f12fe505877e54ece49ef5a -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.Status 94f2cbe668d54b5477fc50170338feeb664b56708238dc8970b3c2f09a997acb -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.Revision dda60a1931b6ba451732667352c09c7620f143a5aa1e7106cbd6dab6d586ab62 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentX 2b3cc11ab8b6e6518a2e902b83d70bf1587c17e9668abfb2092fc013fa03f8d3 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentY 0dbffb2d6ca50d0f42e001c7abfbf0fe502b25a7fc0112515b30638c1106bb03 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentWidth fc2d4345e855eecb7e6bcdd26ef10239e181d1fb43aa5a1a074768f192ea9a7b -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentHeight 7cd8f73eaf8cbed50d7b0b96c295dcd3a7c785d3eda5ace6c8e6c698a6dc0ed4 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderX f1e20d58aa2ee9e529d8be1294aac8e3bb3d49333d9e493c78cca12e17444b5d -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderY 22a144426325b00e46d1f99b044c36c229ee6d624d44ebc528d5eb5dccc07793 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderWidth 88e8dd536bcb6ac485eb4229e2d92fa6373ec2910d8f0e0c4affd462e4f6999c -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderHeight 9ffe4a0ab16c9e0c4cdafc1e3cd1fc5bdca07930d74c86598f88535e96e4992b -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipX 612affc78e49cc56aef4bcb2b0e3568ba15d3e7640b1e7e25a32429255330e47 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipY c3ced3068e325159f15a94610efe3dc9e4a296d3d78ef02934e6b81c7e7f220d -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipWidth 62e37ad1fb4c9547f08bd56488976e23b67cb1783751a0adc034195628fec668 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipHeight 5e1c4f2341f738c85a25d361bd0cb5b9f13601c7ad5957c5631d57ec46ad5fd8 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M11 d686c0ff2582f4e082d02ee5568a0fc9a811538d0d85f291db569e9c9a335765 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M12 de465522c4a4bf12368684f06444a5c1918e2f2bd7679e29ccd3f565d49d5cc1 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M13 1fb9fd2b76793e8429ba32bd5e348aea34450d1cf4ec3f46e069047ea0ed25c4 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M14 67807868c8ec51422c33d039bb8f5c98dcb6bdf9f7d16d01742746050ba32b09 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M21 cbf3ab7e5199a52daf31b725c0515913bc6f29184605d4ff8dd6cb64e8934bed -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M22 5e8c410cf35f1b629c0ecfc4671dc4ba0f01458029a1ac4bce798f59348952f2 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M23 007843ae0c15aa8e396937b7fc867dfbf71267836214c38c9826812369f3e498 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M24 747c6de0932d504b0cf7c3112a8539230218265c5e5b501b18d12fb8839a81de -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M31 033d21e81be40604fc10d3c19fe0866f01e054fb0042a9374834933a51606210 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M32 25546912dbbeb46693bc7bb8dd5f54b054c4d5dc2961128a8aa11c46bd6920d0 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M33 dff2af094596dd034580a751bce5cc79b72ac08b4df6c4095acbbac4525d99c3 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M34 82e280b47e544393d92af43402dd0e419b2f456dd79ab9c2b6ebecae155dfcb8 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M41 e46e33417e1d325d7ebac1c5b64e946fcc82167d42afc640c3889e4e5b042e9c -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M42 e5bcddcbf36ab8f9b9fa875fd29d0eec7c6c25e575b78e526b5eabcc11543cfb -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M43 b58f4cd6648a5a239011f5388b8dcaa96cb6510eb8409ec9d924c019c8a5c847 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M44 6f351c363d9ee10b8bc7e2098b2f4a3f68e15af2659a801992dc283d64212aa8 -->
<!-- api: T:Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfoNative f90bcbe549d7ee09718ac07eb9ab30d7151c36703f237c2bef7ca9e91952ae98 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Value 91758cff0698bdd9279fa3ff2d4ced8c9b1fff771fd045838e933ebb3e92451f -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Phase 1fdb37f5ef9b81189c5e595088dde76cfe0f33a01a3595e146ff7d821d254966 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Origin 1a044c6137bf536fea5350b3eff61b944f2cb5d74424f8e30d375a5dddfdd7f7 -->
<!-- api: T:Inno.Native.UI.NativeArray_ddadfd40a13315f3 13982a444d79efe2bb467b95e6696e4dfe79934b0f334e79eed405cf3ba0641a -->

| 声明 | 当前语义 |
|---|---|
| [`public int Inno.Native.UI.UiNativeContext.RawApi.AdvanceTime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, double seconds, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L365) | 在所属 Context 上推进非负秒数；只影响该 Context 的时钟。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.ClearFocus(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, byte* cleared, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L381) | 清除该 Context 当前焦点，输出是否发生改变。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.CopyEventValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong index, sbyte* text, ulong capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L413) | 复制指定事件快照的 UTF-8 值；容量包含结尾零字节，长度从事件值信息取得。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.FocusElement(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, Inno.Native.UI.FocusOutcome* outcome, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L485) | 请求文档元素焦点，通过 FocusOutcome 区分成功、未变化、未找到和不可聚焦。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.GetFocusedElement(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* document, sbyte* element_id, ulong capacity, ulong* length, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L509) | 返回当前焦点文档与元素 ID；先查询所需 UTF-8 容量，再复制同一 owner-thread 状态。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.GetTime(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, double* seconds, byte* paused, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L533) | 读取 Context 累积秒数和暂停状态，不推进时间。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.GetValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfoNative* value, sbyte* text, ulong capacity, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L541) | 读取当前控件的类型化值和文本长度；传入缓冲区接收 UTF-8 文本。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.PrepareLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong* revision, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L581) | 刷新当前布局并输出 revision，作为后续批量读取的同一布局身份。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.ReadLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, ulong revision, Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfoNative* layout, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L629) | 读取指定 revision 的元素最终几何、裁剪与变换；错误或过期 revision 明确失败。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.RefreshLayout(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L637) | 只刷新布局，不注入输入或推进上下文时间。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.SetTimePaused(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, byte paused, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L693) | 设置所属 Context 的时间暂停状态，其他 Context 不受影响。 |
| [`public int Inno.Native.UI.UiNativeContext.RawApi.SetValue(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, ulong document, sbyte* element_id, Inno.Native.UI.ControlValueKind kind, sbyte* text, double number, byte @checked, byte* accepted, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L701) | 设置匹配控件类型的值；accepted 表达控件是否接受，不伪装成用户提交。 |
| [`public enum Inno.Native.UI.ControlValueKind : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L774) | 原生控件值的窄 ABI 类型标签。 |
| [`Inno.Native.UI.ControlValueKind.None = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L776) | 没有控件值或没有编辑阶段。 |
| [`Inno.Native.UI.ControlValueKind.Text = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L777) | 文本内容。 |
| [`Inno.Native.UI.ControlValueKind.Boolean = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L778) | 布尔选中状态。 |
| [`Inno.Native.UI.ControlValueKind.Number = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L779) | 有限数值。 |
| [`Inno.Native.UI.ControlValueKind.Selection = 4`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L780) | 选择项的稳定控件值。 |
| [`public enum Inno.Native.UI.EditPhase : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L789) | 当前事件对应的编辑或提交阶段。 |
| [`Inno.Native.UI.EditPhase.None = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L791) | 没有控件值或没有编辑阶段。 |
| [`Inno.Native.UI.EditPhase.Editing = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L792) | 尚在编辑的中间值。 |
| [`Inno.Native.UI.EditPhase.Committed = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L793) | 已完成的控件提交。 |
| [`public enum Inno.Native.UI.EventOrigin : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L802) | 当前事件由用户、程序还是后端流程产生。 |
| [`Inno.Native.UI.EventOrigin.Backend = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L804) | 后端生命周期或布局流程产生。 |
| [`Inno.Native.UI.EventOrigin.UserInput = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L805) | 当前输入操作产生。 |
| [`Inno.Native.UI.EventOrigin.Programmatic = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L806) | 显式程序操作产生。 |
| [`public enum Inno.Native.UI.FocusOutcome : uint`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L832) | 显式请求焦点的领域结果，与 ABI 调用失败独立。 |
| [`Inno.Native.UI.FocusOutcome.Focused = 0`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L834) | 焦点成功改变。 |
| [`Inno.Native.UI.FocusOutcome.Unchanged = 1`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L835) | 目标已经持有焦点。 |
| [`Inno.Native.UI.FocusOutcome.NotFound = 2`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L836) | 指定元素不存在。 |
| [`Inno.Native.UI.FocusOutcome.NotFocusable = 3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L837) | 目标不能取得焦点。 |
| [`public struct Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L848) | 类型化控件值及可单独复制的 UTF-8 文本长度。 |
| [`public Inno.Native.UI.ControlValueKind Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Kind`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L851) | 控件值标签，决定读取哪个值字段。 |
| [`public double Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Number`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L853) | 有限数值。 |
| [`public byte Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.Checked`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L855) | 布尔控件状态，零为 false。 |
| [`public ulong Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo.TextLength`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L857) | UTF-8 文本字节长度，不包含结尾零字节。 |
| [`public struct Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L867) | BGCS 生成的原生存储布局视图，与对应 managed DTO 配对；不作为领域数据保留。 |
| [`public struct Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L916) | 同一布局 revision 下的元素几何、裁剪和局部到 Context 变换。 |
| [`public uint Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.Status`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L919) | 元素查询状态：0 不存在，1 不可见，2 具有有效布局。 |
| [`public ulong Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.Revision`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L921) | 该布局快照的 Context revision。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L923) | 内容盒在 Context 像素坐标中的横向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L925) | 内容盒在 Context 像素坐标中的纵向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L927) | 内容盒在 Context 像素坐标中的宽度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ContentHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L929) | 内容盒在 Context 像素坐标中的高度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L931) | 边框盒在 Context 像素坐标中的横向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L933) | 边框盒在 Context 像素坐标中的纵向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L935) | 边框盒在 Context 像素坐标中的宽度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.BorderHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L937) | 边框盒在 Context 像素坐标中的高度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipX`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L939) | 有效裁剪范围在 Context 像素坐标中的横向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipY`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L941) | 有效裁剪范围在 Context 像素坐标中的纵向位置。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipWidth`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L943) | 有效裁剪范围在 Context 像素坐标中的宽度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.ClipHeight`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L945) | 有效裁剪范围在 Context 像素坐标中的高度。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M11`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L947) | 元素局部坐标到 Context 齐次变换矩阵的第 1 行、第 1 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M12`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L949) | 元素局部坐标到 Context 齐次变换矩阵的第 1 行、第 2 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M13`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L951) | 元素局部坐标到 Context 齐次变换矩阵的第 1 行、第 3 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M14`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L953) | 元素局部坐标到 Context 齐次变换矩阵的第 1 行、第 4 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M21`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L955) | 元素局部坐标到 Context 齐次变换矩阵的第 2 行、第 1 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M22`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L957) | 元素局部坐标到 Context 齐次变换矩阵的第 2 行、第 2 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M23`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L959) | 元素局部坐标到 Context 齐次变换矩阵的第 2 行、第 3 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M24`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L961) | 元素局部坐标到 Context 齐次变换矩阵的第 2 行、第 4 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M31`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L963) | 元素局部坐标到 Context 齐次变换矩阵的第 3 行、第 1 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M32`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L965) | 元素局部坐标到 Context 齐次变换矩阵的第 3 行、第 2 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M33`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L967) | 元素局部坐标到 Context 齐次变换矩阵的第 3 行、第 3 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M34`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L969) | 元素局部坐标到 Context 齐次变换矩阵的第 3 行、第 4 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M41`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L971) | 元素局部坐标到 Context 齐次变换矩阵的第 4 行、第 1 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M42`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L973) | 元素局部坐标到 Context 齐次变换矩阵的第 4 行、第 2 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M43`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L975) | 元素局部坐标到 Context 齐次变换矩阵的第 4 行、第 3 列。 |
| [`public float Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfo.M44`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L977) | 元素局部坐标到 Context 齐次变换矩阵的第 4 行、第 4 列。 |
| [`public struct Inno.Native.UI.InnoUIRmlUiAdapterElementLayoutInfoNative`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L987) | BGCS 生成的原生存储布局视图，与对应 managed DTO 配对；不作为领域数据保留。 |
| [`public Inno.Native.UI.InnoUIRmlUiAdapterControlValueInfo Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Value`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1008) | 事件发生时捕获的类型化控件值信息。 |
| [`public Inno.Native.UI.EditPhase Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Phase`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1010) | 事件对应的编辑或提交阶段。 |
| [`public Inno.Native.UI.EventOrigin Inno.Native.UI.InnoUIRmlUiAdapterEventInfo.Origin`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1012) | 该事件的实际来源。 |
| [`public struct Inno.Native.UI.NativeArray_ddadfd40a13315f3`](../../../backends/RmlUi/native/Inno.Native.UI/Generated/Bindings.cs#L1186) | BGCS 为当前原生存储布局生成的固定数组容器，仅用于 ABI 搬运。 |

## 导入图片 ABI

这些成员仅供后端适配层消费，不属于脚本 API。由 BGCS 从手写 facade 正式生成。

<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.RegisterImage(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.SByte*,System.UInt64,System.Int32,System.Int32,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) 67d452d572e19722e4b53b06e4b79667f981764c01cdc05f7407692de1dcb683 -->
<!-- api: M:Inno.Native.UI.UiNativeContext.RawApi.UnregisterImage(Inno.Native.UI.InnoUIRmlUiAdapterRuntime*,System.UInt64,System.SByte*,Inno.Native.UI.Result*,Inno.Native.UI.BgcsInnoRmluiError**) ea065c12215e4bdd2aebbfc21d3a27dc718d051a4002c0e6ef4e0aa7f7d60943 -->
<!-- api: F:Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.ImageId 83b1b9f86ddf4ac6385b50022f2ce8ea3d59cadb1e0d7bbac164695a9e29e442 -->

| 声明 | 语义 |
|---|---|
| `public int Inno.Native.UI.UiNativeContext.RawApi.RegisterImage(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* source, ulong image, int width, int height, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)` | 在指定 context 注册逻辑图片来源及布局尺寸；返回桥接状态，并通过独立 out 参数返回领域结果与错误对象。 |
| `public int Inno.Native.UI.UiNativeContext.RawApi.UnregisterImage(Inno.Native.UI.InnoUIRmlUiAdapterRuntime* bgcs_self, ulong context, sbyte* source, Inno.Native.UI.Result* bgcs_result, Inno.Native.UI.BgcsInnoRmluiError** bgcs_error)` | 使当前 context 的来源失效并退休相关纹理引用；失败保持可诊断的原生状态。 |
| `public ulong Inno.Native.UI.InnoUIRmlUiAdapterTextureInfo.ImageId` | 导入图片的不透明资源身份；零表示后端像素纹理。此值不是 GPU handle。 |
