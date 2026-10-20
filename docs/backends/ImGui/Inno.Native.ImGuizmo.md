# Inno.Native.ImGuizmo

[分类索引](README.md) · [Editor Scene](../../editor/Inno.Editor.Scene.md) · [Wiki 首页](../../README.md)

## 职责与公开入口

承载 BGCS 生成的 gizmo 互操作表，以及宿主原生库初始化。`ImGuizmo` 的公开操作与 ABI 类型由
`Bindings/bindings.props` 选择的 `Bindings/bindgen.json` 及实际目标配置 生成到 `Generated/Bindings.cs`；不手工修改生成物。
这些 API 仅属于原生/presentation 边界，不属于后端中立 Scene 或脚本 API。

## 初始化及所有权

静态初始化以当前程序集作为明确 owner，首先加载 cimgui 依赖，再加载 gizmo API。
内部 `ImGuizmoModuleProvider` 组合两个 `DynamicModuleProvider`，生成的 module context 拥有该 provider。
释放顺序是 API 后依赖，重复释放安全。API 卸载失败时保留其依赖，禁止继续使用该 provider。初始化失败按 SDK 的 provider 所有权协议释放本次取得的资源。
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
                return ImGuizmo.Raw.Manipulate(viewPointer, projectionPointer,
                    ImGuizmoOperation.Translate, ImGuizmoMode.World,
                    transformPointer, null, null, null, null) != 0;
        }
        finally
        {
            ImGuizmo.PopId();
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

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: F:Inno.Native.ImGuizmo.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN.Data 5b60a4b0608fe1158ad5e39e97c2459b45dd5abf53a5c32bdf798ae53e11c772 -->
<!-- api: F:Inno.Native.ImGuizmo.ImChunkStreamImGuiTableSettings.Buf a7e778cbd6fe83c92c2e2cf845250f427baeb2597a2d6c42e1cab080e7de40c9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImChunkStreamImGuiWindowSettings.Buf 3825877070be0cee6533a4760179a314af896ce1bae29af0479cc0b109db5806 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawChannel.CmdBuffer 5d2ee0bd99f194553d6df4b3a6ff1cd2b7976af1c0a2a6a32168b84a63c22055 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawChannel.IdxBuffer 6f3176846c7ad43419ef452901ba41f3fe083748c659909314e310ebed73875d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.ClipRect a49771bcb587227824b6c0821ca0f544afeccfdb330606ac14f5c8e39e6f016e -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.ElemCount 4aa27cd520c5d996642482f26edb0381bdfae693c74dcba3385d6f08e2570c65 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.IdxOffset b0d6bfcea179254a1dae559de3c074f3c8f0a06b3ea6d2ab109e160264118fce -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.TexRef f1ff80bd10445e166c50f980dc5357e8dd69cdca7f00d094e812b788bdb12408 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.UserCallback 720004c0944b7028959dfefc3fb24f150beac96b668e1b9ca0a234fbb10d3ba2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackData 3814fddd49f6be7f83f001c1296e78997e842ce65c87e23a73674b8b15f54edd -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackDataOffset 8f8a6b19034f417b88275e4b97e40e8231fde193a8d291f855b60d310c4dd766 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackDataSize 075ae231348e15d2246e13e9c728d81cc5f2ce66b1cfbcb9e0576b02d89c1860 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmd.VtxOffset 65206c6800162784154e8b7a7114239a1382157f52a05fca163ad7136ac838c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmdHeader.ClipRect 9ce226e9835c0f168abc3bf89d2d9b758fa254d709e57d2e2d0ff2571ecf4ac9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmdHeader.TexRef 469d3c0dedce77111bfcb9d3ad3c48753ac7f3a7eedbfcd34e66754126abb631 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawCmdHeader.VtxOffset a41ab44348b42cb3772ca5526cfe89b7a40743d2065f62eaf991085d4b9d7d53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.CmdLists cd0eab09fc693a0e53b347841cb53cb35054a1c677c22d06cbf5e761a6077081 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.CmdListsCount c5baa1ca521e87d64fd24edd6a992f2dd0bc8b5a9965be6d9526506dc371636b -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.DisplayPos 4aa31ab536cc7027b006da5181e82a3e8826a31a689e081cdc2eab32bccc97ff -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.DisplaySize 28b9ac3343a5f898ae6ee058aaa56a4a110bc885ef91634763474ef807961c14 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.FramebufferScale d886470a963ba5e8c4034c14234301876f539348abb1fe8356ef37a1fd718dbd -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.OwnerViewport b4e64c98b3fecb9a8ab6b0d25055caba8969a41e99b49b1d37295e8b04da08bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.Textures cf72b3ae6c434f4b9c4e26e901965d9e14b5a33bc75717b4fd240f5e40104112 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.TotalIdxCount 979592f3d0d8b705342c600623bb911e8e071ef07003dcc913f294b7257699c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.TotalVtxCount 21d382fc17159e73c02b2924c1502bae19950321428cbbfbdd16969e3a8d3b72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawData.Valid 144fde2b6e7fc2b376f952dd69943f80fc2da4effd8fe66b386a3cda75977c53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawDataBuilder.LayerData1 66a4a3b407e4c566060dec8793b643511521df0a496d25ce16624605b9d72e1c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawDataBuilder.Layers ce23c9a1fe80b5ec85742d35f949e9de6ed4828bda9ee1fabcc59c51e7a0ce6c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.CallbacksDataBuf f4716852eb5e5846096e82bdb36210b7550799ac50e0c7ac25cd1b1c6424037e -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.ClipRectStack 89574ee540cc2e4212646df987461996bc005d4964db494ffd64810e87408066 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.CmdBuffer 5689e9602ee16bb8ed1c285c05490097e7f15460ff5133215de42b27c96bc3d1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.CmdHeader cf87d795df02de4e758e8e51a113f49179ec3691d46e5cdefa239e3a2c2e30de -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.Data 77183e41fd89afe6b15ea62076264fdcd9b08802c4fffa606953aa5c46302c0a -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.Flags 25bf9c7e388b7d1b3c390f280a484d6bc4372d5f00955b3160b05dcd5d24f40c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.FringeScale 51acd77d2a239303600ea898c7a1ab5e486d6c844e7fc6f796f7a4d73a9f0956 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.IdxBuffer f56859374cf75039f83e9f994302874b99820feba121f8838962d4adb70f3f57 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.IdxWritePtr 2a575314320ff9e9202259372f19d074f675bc004e6cc5822b475b33f13c802f -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.OwnerName f3a12b17c38d662216fc2e1a10556478029470b1407679e29858bb1d51a3d22d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.Path fed2ce88f463dd36974ea1f6505ab9ce4524ff3e11201699f5eadade91d2dfda -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.Splitter 3eccc97cf695552da4581d88f175ce20c3abab35d309f0b378ff003f1b5256dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.TextureStack 9773c6d4b6fbbeeb368e5197b7997bc284f2e5e74c98dbe75f813a471f5e6862 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.VtxBuffer c322baf7b7b62c3a57ea0df24bc6d2410413a7c9a0f96864c8225b4977426420 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.VtxCurrentIdx dc9f0c8574557181bbc31b8386c116ed883a9c66e692ddd9f52e4a4d3198c17d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawList.VtxWritePtr fe4cd3a84ec83eedfedd3cae7369cbbf59146731e996db3fa82f2181fab1ea1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.ArcFastRadiusCutoff 05587179504732d88c45d81de4ca9b9a19ffef473fae43076ead9175f43fdf01 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.ArcFastVtx b283caf7a8e4355d647086758a69a7257c9185e98e8f777bb3420441d1d441c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.CircleSegmentCounts 90fed4485a165f6f1da9f0f7f9f3b9f3f93b386028f5c95e08a4cd34d3718b04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.CircleSegmentMaxError 3a36e2643b72224b5fef6e9689ac9d62efd5521913c108e0e821b74e1b5a6e8c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.ClipRectFullscreen 5805d47ec188472fe437133506bef4decf11b8ba3118aa4e4b3676bc3621d9c4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.Context ac6824f8747d890da61fc8a59a4267ae441d9b69aa6fe5c7ca4c571c24fa19b6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.CurveTessellationTol 4e8fddc84ec868bfd1e9a6897011a49c5d9cef8d801e15f75577df32be8714bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.DrawLists 081cea6565febc6b7ee2846623742b272d088c8f65c575c23530c28f66512e1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.Font d01ea6f4af642fb8b458eb2f308aa7ae6444b7c7c056ffff3cd25507f41262fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.FontAtlas 61198743c8f8c758cd4802628673d037a84367e2921b794904b02ede46c063e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.FontScale 3f91d0884867946c7442946af0fcbd7158dfe63a073d88bfc7c18f0a95da3f6d -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.FontSize fe9a549508c16d641e808c13582dccc0029f3fd8943386dca821e86426d0560e -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.InitialFlags 42fe40fb16212e9b3ce310e922a86df7ba2c866f1bce943dc146067085dc32dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.InitialFringeScale 1f00772e06da80c8e7fd4d4bfb10358537d5f9c355400a69c4f0a5a9d9f04a97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.TempBuffer e6a1e02bf3fbdc87443038e1e756261a93a2602f9dd9ab5a05d3401e218372d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.TexUvLines 951846ad92a486f4acda1de6bb1fdad9f6a51e249bc8388b4d4640eec6012249 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSharedData.TexUvWhitePixel 2adbd18514a47741ed10978c8cd04bf7e0dfa8da18a39df8000aba2b7c956b19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSplitter.Channels 54a8d8358846a1b73b8abe3fdd7d2f633013e5b05f74014e10b30d69bd21efbd -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSplitter.Count e5b0a17d17cca2cbaf6fe88b2135a365d41e341a111ac14265031b56739a8197 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawListSplitter.Current 330be69166302170e1b41f240bd180f83de227d9f1fd4f7cafcf34a72df0cad6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawVert.Col 8d721478322951ee400bb2c5ed957663c3df748d6f0ad21091d35faf1663898c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawVert.Pos 2c98731d4c329ebcd925ca7fe41b93887fa0317d1a017193beb6bbce7c6e304c -->
<!-- api: F:Inno.Native.ImGuizmo.ImDrawVert.Uv 547ca1c38fa8709030d563415dc6ed068b3792f4334d5d5d95e02eef12d09db4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.CurrentRasterizerDensity a33e370733757460d2ca509c4aeb5f57f5f83e81b29f1f03889836f37add6224 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.EllipsisAutoBake 0efce43e4773a636a859fcd956549f4cd4afa8ea7b4ae731fcc2e89c0f1647d2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.EllipsisChar f5dff61a6bfed283d1b492ba916cbb2fd7dcb6d834f02685b7e5781c44ccb976 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.FallbackChar 7227a522e53c97e13279e3e56ba7b03459223b263a72f00a5b5a134f072d525a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.Flags a54834d86a9a416a1e297266f08815fd1ae8062424a25cdb641eb0dc7b049c04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.FontId 875664d211b24cc71386ce60742acc739a027fadbf895fb524a9e609c23e378f -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.LastBaked d1f47c082789bc78ea5ceecab04ed747bd043a0feca28c1938a82772598ca27a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.LegacySize 3088ddcecb29e2529b9c36c53d2d923676f2af82a2cb2b17144cc90779c46add -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.OwnerAtlas 95db6f270cd0ad9ee6c1ca5ffb5a493a6882bc1400d75363f373bc7d139d6ea9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.RemapPairs 865063e83d2099fe98ec4154470b82b99bd3670fede34933d172f14118ef4799 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.Sources ead7c88fbad7bfa4b71f441b3d27fd1a2b3cec549fb26a42b4ce1d1a41a6a896 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFont.Used8kPagesMap b8ed87156273e445131cf012e234c13872f3bb30d46ed6fe145345cb5d39bd52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.Builder 49db2750609fb391bba9d5e061c82faf6d5334c7bafa2c329f19000f383a2858 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.DrawListSharedDatas 551570ef958ed53383c395daa8681c3114da3b0811e7b5d0870143736558b892 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.Flags b381619e8d49ef01f2b2d5f1fc745faab35cd3907e09a419c2791702e487f512 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.FontLoader 94060c15017c8e8e180b3967dec258acf822c383e570115882d8cac52eafd676 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderData e389d860918b940082ee30c205c5ea1d1bd4016ca233639d7ec32d8f9d46ff0c -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderFlags 3cf008424047308bcd0b6810273ef4ec33a89c958bee22e8fcc663bdbec74311 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderName f6bd458e2bdacc81c9597d4874a5343057881193cb4ea9889bab1c050c451fc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.FontNextUniqueId eb6ab11f1391a54ccee10fad726883c316635e93eb7bd6013e6a290398895c07 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.Fonts 013e80579d73fa2c02bd491d1a5ef35e34fde2c1ddb48fbd2897b202f99811c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.Locked 1a1f4d69e3834d895f8a491c16c86a0e651b301db124de4afb8ae5f387538951 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.OwnerContext 975bf45a76311822b1af52ef2fa7ed450019452bd9104ed60d37fafac4f7acd3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.RefCount e30f473bebebe3a774dd372b9760f7c917f165f5c68fda3a72e5a5a903e82bf3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.RendererHasTextures 60f7c5880ae24e425a57f024c63648a1db37782bc3396a4fb5b598dbc792d53a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.Sources 641b11dfd094e4b06b37e3daec7ddd2eab647dcad35f8bb8ce7e8af559c09564 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexData 52701385fa1a75f9b22b1ad513b2cc053c9581347c6255be1f9d0f2ccc58ff65 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexDesiredFormat 158d179b27a62cab8424899ba7918dd16b3d8dd08d712f6ad15aa256cdfbd4d0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexGlyphPadding c832dc7cc3d3166d098f60bfe317947d75ad394fc7c63a02e6ef334b7b8fd7ff -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexIsBuilt 25d11a64cdb01eb906a03b3ed6946ee04d27efb268dafe1897f0a8118fc59a2a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexList 8dc4cc0ea687b446e03de5e19c7bcd7c9f351ff5791614dea5e2d963fc55f5ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexMaxHeight c08b102215f9d29ee80a0084b10fa46d2b1d6f961b98cc4bf82b1a421f730369 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexMaxWidth 408be90fcebfcd8ef89b4b423cdea52744818daf74aa31a07a671c17d28cb411 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexMinHeight c9b5b65808ad63c3a6568aca18db11a91a056e087d703ef6e92fb548640f8ade -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexMinWidth c64b21749ea507c56b555827f6d2b0213d212300755fcc4abc6fb3f91e73c932 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexNextUniqueId 08e4ad009994adf71b51cd47cf155f220561cacd3e4159f2b0a3786b6bcb4e27 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexPixelsUseColors 49d2f8a925c434e3de950a8a55e78e33528c2c9bbaf5d96a2f245fe2fbb5aea4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexRef 295f8f8e8bc935e1f3d682e23265c232caeda1833e02aad6c564cb7ce0a85d20 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexUvLines 95a299ed821a2015f675e7346d0fcfe9599e0b381ab0df8371badbf14d4f1896 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexUvScale a61277fd003c32c54f170c58a5067d340d896eb3fc82813726b8ee2328a7b0ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.TexUvWhitePixel 993f29df01ba80917cb0ac27eb42b70513d2a3c97602a99f2c3368b95c8f01da -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlas.UserData e233d0fc91cdd562a01375c3312f8d90388f11d6c488900a5b838ef919b6ac75 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedDiscardedCount 867c465695ee342b14dd4c64609361ea004df686d8f120c8bcd05f6c74c92521 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedMap 34a814bcb49247879f73fd4c3fb64b8e3cf980ddad180226bd96a8aea18e0c13 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedPool b6afe25fb9836c75e911abf195975627b408b41e90690c8aa3404359bc8106ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.FrameCount 75bd9fd04dcfea67abbffd56c1068b1d3733c26cb1cab5f9e76234e4b1f64386 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.LockDisableResize 602523720ca2f5d685ffdc0b6c26a9a5e2bdbe02dfa120b8552e6728c594c574 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.MaxRectBounds b8d59e1c8e2bb90a09a70c34ab28c87a164cf46c0e0f0f1fef04b15210e1fcdd -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.MaxRectSize 4031bce9057e7e93fb8e5ba3a445cf05349e6842dce2a1df6be9be20775f3f15 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackContext b7630cf7e55c277d506fa50d3fd887a69bc2c35c694b9ba484574fc7802094d1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackIdLinesTexData 933518cdafe7bdd90d1de0b91c3ef35243c88a696703bfd8fba17c471261962d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackIdMouseCursors 8154b7d68daef0f724b4e1fbeeeab7c73fa6cd6ba0b358d9d938c8c6a8ec3c72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackNodes 38a36b1ba3841dcc02434b094e2a2e4634c17c0f3314f5174f85337a717cc110 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.PreloadedAllGlyphsRanges 4884b1c71148fe4c4564977b92c64e1d4a7f8155e12c6b36ae3be7d0108fe48a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.Rects 488bf768b69a7e66a3351dd9273447378dfe4c5d056abaf12d0288885b3696af -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsDiscardedCount d2f1e2f18bf5023987f595130e0ad7536b4bdca0648e86eb97e79101b31093fa -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsDiscardedSurface 6e3cd590b9a158684e14193adc1517d46829bcd84e5d64b4daf1e9e92f66992e -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsIndex 23ae82c14365d10d17919d829e355e4545d6d294027265aba30214df3aa47f65 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsIndexFreeListStart b02cc5b7ce7023ae9f6525e703dbf147b1e2888bd1f75c19aeeb45c08389f4fd -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsPackedCount b657e7ca58c8623031edd2b2af9b0b09858ccffb1f22fef0392007d37a425465 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsPackedSurface f30bb3876f656362952a0ca73a049f19bbc9cd3cab8da7ca100814df45fe0087 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontAtlasBuilder.TempBuffer d3ebb38cf8cb198e4d4aa78cd4e3e0f62c48d2d0a8b25347821afbe37d174685 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.Ascent 0a1513fd6e1f18211890806eca7bb00eea03cada2528c31c9a124004e5c05cd5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.BakedId a9a19156984da897abaaaf621badcb8f547b3421724ca54c2d6630394d53b1df -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.Descent 2d7ad63eb5bbd158ff10463837f45333dbe108ae0d7c4ce4715e5e0c410723de -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.FallbackAdvanceX d38c394e39e4b849582cafa14f410a0fff4ec5dd6360cb445f93ee6a1aa6ae0e -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.FallbackGlyphIndex 93df10cfe17b190d4d1ed6562d45f5f31ead0a77f90a18a7e009984252927d7a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.FontLoaderDatas 53992a66855315e9ac11638a474afd79aaad1f29f7980fcd34eef7081a436f6d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.Glyphs d690ba3707e425d019b0beb0d4845af8aa5006dfa8d5a19b70956af9d3823a48 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.IndexAdvanceX 4957cc96f59bc1f6073d7afa5a65a0893e6fc64c3df8cee59cfdd2e149ab119b -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.IndexLookup ad8feac2422bed0cb7aea0646ddfa4024e3086d3843c6490f53f604cdc71e620 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.LastUsedFrame c2a636ad23478e9808381d4cc80a5a2639babe7fd2613f61402611b04426dac7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.OwnerFont 3886b04e24387b911553bafa7dd516554e177d1a314b481b19c862bf20321ef7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.RasterizerDensity 2b545e63ee0e5dc3d29af3cefe7ce72a08682ab304ca382ab229486d5b7fae97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontBaked.Size d70b37055d7ff867e63dbd8c357127836693b74895cbea10d1b599d469e2c06a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.DstFont b8caf784ae92c7e4aff94e81654a177ca926c75dbc79f9d0e065431d429bc4e8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.EllipsisChar 1679cd36fe6bc89f197fdb2bd8125a08fbba379d6ebbe235fce4336ccf3a82e5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.ExtraSizeScale 6c1ff9603db66ef72bc4e441718375ed135656fa35f99e0fb2b8e02369306bf8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.Flags bd12a75037b725b0c1d835d7fc6aff9ab6e8c8249ab85da2d0285937307d73f0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontData 198f4a08dfb84404e0b3589c73dc2e3aca3f56c7319ccc9d89987e112bf20ef5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontDataOwnedByAtlas 087ee89b0868cfd6ae21c3d9861b89ba356a16d775fcd856d0156a6f34d83e7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontDataSize 04897623731756b44bb74f863eda25370d7f0c410380edfbf36a9be56cadbfe7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontLoader d77faf37db8994a07256cb9f830c25595418f3545d5f0dadd5771d76829fc3a9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontLoaderData f1a4ff3e3655997f4944e01ee39ec35235653f47e9de7dac7c0e7b63d9d58c06 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontLoaderFlags 3539e4df9cc1c9fa595c49c0d6df6627f2970d0f93df869cefa1dc7567b18207 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.FontNo 8c6300533060071bb7a6f30a9b9ba0f50ad014ab616016de31cd86874e6b4e9b -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphExcludeRanges b73c0f85e61ce6b09d4b20694978f47b4735eb5659e38d7f0fc46768f965e5d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphExtraAdvanceX b0f2d989ff105ace20f98e3d42d3cc3ec41020a5fa9a0e0c43475f19cc81af34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphMaxAdvanceX bdb1b78de219cf657c4901d7481bbde259e67198af50e9ea78f0f90d81b76a35 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphMinAdvanceX cea2cd9f62fd87fbc0356bf3b79deea76c9487af9cad4982a2c86bd80495d4da -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphOffset f2a9b403e488eea68649758f2ef8ce94da604232aff337819f896808d96d24f2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.GlyphRanges faab27fc8f76d469ecc6b6fe88f8663a9a162357bb27b1e901baf897cb13c1a3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.MergeMode 1a5c47224ae98fad1ab09f487e4bc06e5b1dcb41539c7c0e9f1f6637355a1b18 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.Name fb72e5905c1ee396ac9eeb0a124b4a0d152356552fe6e9e670987e239cd9e693 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.OversampleH 4f836f9d4ec2205a2067315dbe478ac941a4ceb7cf3842b8952a87285eeec242 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.OversampleV f02fac33bb115d36319ab3e861925059df42456cfa742d21364d04102f4fa5e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.PixelSnapH 1d60e0f3812255bc06c2d70402641a13490cb3e4c3bf8f97294e7f46b4539b3f -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.RasterizerDensity 12d85ea0cc4487e09ef58e9133f99be1a0a3e1fc829e9ca632d68c1b5fa7a9bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.RasterizerMultiply 1791c3fc243314d27251a07ac7e1e209f36a06dae614d21c54c934125e44a46c -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontConfig.SizePixels 7a6072fd61957779d358e6704f5f490b5f495eb67c9ac61694a6a079ffed573d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.AdvanceX 27452e56870b23da9345463a30fd0b3f812e58b7fd352f0e66a21afd13d7abd0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.PackId 56d8a71b3d4b71bedfdbf3341fe3a18698b7ca15a98638bba9de734566be6729 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.U0 dcde73a1fd69b2d1f1d313af37d9df8e51ffdd3a24775c7130367e3186a29bce -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.U1 bfe4d23341dbbbb7d023f81ee3b9b16df55833db3beff61a109ea590c337496d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.V0 5b2aa829351b488122aba7d7f4b56153a6b5742a496d0819e1e0e1384d2b342f -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.V1 c5340ccfe86725db4fba14cc4fcb49c6c11e95535ca88fe0c5ca26a01219bdae -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.X0 7204a4021078e5136fab8cb7fb4bfddfe76f935b9d5edc0d120fe4a0db5336a2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.X1 480f82c6d1141c6adc0c03aa9ec79436682e1ca158827a1915fb04aef57b5b34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.Y0 e7774c08b15402cd4e8129d61127fad0f6cb8fab43e895eff8635ecd497385f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontGlyph.Y1 9ad5dc34b422effb545e1f9e3677aa98ca29dab97b8f3e0f41551b3a5f0cd869 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontBakedDestroy fe592f8822684dca0ccb9a20011c5e257f231732320f4effe625fc956d02238f -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontBakedInit aa2f9e1d5bd88a75bb0fd6e7cd0349ebb2f188b0d30cdf6606a9607cecdadabc -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontBakedLoadGlyph 64f7084ebebacea273e87e98f53fc79aef7387744e28c7906587420275bb0ef3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontBakedSrcLoaderDataSize 63453e41178adb7e554948e3a887b612c8e67ab3bdf0924470f9b4b8cf9c620a -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontSrcContainsGlyph 477bb776ac928ce7c27356a4a68301ebaf65a4e7477e8c4bf9dc681b75fa7e6d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontSrcDestroy 49750abd7fbc1c19b57ccece90b9ff48c404207b2b438babb318a56c29ceec2d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.FontSrcInit 4d07dccf440650d87555f5ebbd8334631bf10bb2c1ca345e7cfd1843bfd64fe3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.LoaderInit f0a2cd8368ddaaace04f922c23481d9196b3b33e9a6575cabd0a322ec154bbf6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.LoaderShutdown e4089185581a04079ee4dce0a39e5d97c2eeff8fbfbbc432219ba23aeefebeb0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontLoader.Name d389a9cd8a26fa31746a32a487714a3c08acfee1a16486f477c47a1fd17e9f6d -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontStackData.Font b28ae19d3eeafcd102c2498ee87ca8cd659e2bc5178d7667558283d916e1df16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontStackData.FontSizeAfterScaling 89ff10600fde83ce3cd90f3a5d5612a7932a142c984c11903d43d9887ab8dd04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImFontStackData.FontSizeBeforeScaling 250fa3df33d0aaaffdc6694c818ad526035691c3f40e408785453739788002e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisNone 9699e14244663333bbf8698bce482ce1bfa848f617e58fe7cb7467ec515c3cee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisX bc52c1a48a271a5f9165282ce802ea6af2560ca25c42d407219640519afff67c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisY 1d0e78b542fe1b1bbd1495142b2a4a61c504d453f250f3bac820eb4d652c3dbb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.BoxSelectRectCurr ac63bc5cff47e6f382d1bc194ee498cad7f65cecc1a6ba8b1b4b07e5b3d44ffe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.BoxSelectRectPrev c45b9419adc397050732fd842b26b21023c97fbd0ebc363a778bf03f428380bd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.EndPosRel 4a41e15e9bd1bef6e89014eb4f8c9636069b9fca2c32e8488e52efd2f46fb488 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.Id c2f509672efb1ace5acb1cc5e37b2ba6d2decaea851184d37cda7e3ccbabf295 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsActive d8f05f135b0a3c772988e8e41022208407a50546370ab3d0051fa0b0599f9c82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStartedFromVoid 97e411da79450a361ab93d3940eba8c5589d6c9ad2bf9baac4aa39279ae6e75d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStartedSetNavIdOnce 45eea838332fa96bec44edc927a63295a30618d396891716ef8a59f9b7b7a174 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStarting 49c6cc3e630f0b8d0deb47b9759f1f1cc7105381731b4d924c8d3eeb34080dde -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.RequestClear 45bbc883ffdde2e94704f9b506493c359db031ddc4a88aac7ca92cb7dbf2ba71 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.ScrollAccum aaf5cd3d7c020782bd3488b3c4e381d4c894f5d40c88013ed46c59e6bf6eaa2a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.StartPosRel bd6c74a33fe22801b0d1c7b8a022705c68629c9fc4d29afdc99e31f2470ea863 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.UnclipMode a475360d61570a7cc71be2d283f26217fed641a70039875c85da97ac4bc6b6eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.UnclipRect 7a028016d6e6674ea46d39db0b0eb991febcb7f709b1a60c76b4b33d6f0d3858 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiBoxSelectState.Window a890c2550b0593364e42201ece08d7a87204bb023300ccc9ab514a3a23bbe0cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiColorMod.BackupValue 8fb90521ce9537bcd547860d6183a6fb48086181dc8cdbfb3bb41f19f4beea61 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiColorMod.Col 5baabce176ff225c2442b0f98ae8a7d4fdcfdcfaa84a9554025b6777ae7a93d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorMaxPos 0d7881930367185730cb2b63fc3ccc9caa75fb4de482f3cd217e240bd76dca39 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorPos 5e18448ba89bdd5980d4a673566856f1ed75ff4cd45d1a17d995667a4cb8d653 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorPosPrevLine 48bf058f7a3cbc606b79b201dba3797a26eeeba86e6e0c9f13892f8596c0bfc1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupLayout eea0e2e90286356b8a1fdc23e994c6246f51ae502c050be4a8b5ff0cb82934dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupPrevLineTextBaseOffset a19fc971e9234af733571568e89752ad87668f4859d0d7bb7be81594dbf3cbe4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiComboPreviewData.PreviewRect 8524334b27ca80d512c048cb2106c3620e6e2904d812e8ae7dac91416fd8ff98 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveId b14cd941bd55bf0cb8a87af2ee24430649bfefd337686a28265ba2a66000ef26 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdAllowOverlap 73e8ef1ac3d02398c3f53b830bc72b380f9a378f4191ecc01b9ea7590e34430f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdClickOffset 6593af4cce1c553532354bdb6b26b77035b16a7f37ebbe8ea2da9a46cde00d6b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdDisabledId d0c397da9f06149efe4aa6b376b35d3f366469b1dec58bdaa634bc2c65bf4f8f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdFromShortcut 009cb7ccdf309ac3b3422946d84463d6eebbc51e32794736c3316889d02c9945 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenEditedBefore 0bafb78ec9821bc6a46815bb0107128dfd062a29b5ad55017cc6377126e4ab92 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenEditedThisFrame 32a46d270009a354fa3d9b83818afee1564bc0fc68875af620f2115c152f2152 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenPressedBefore ff8d8d3a5ceb1775e436330f335ebf4ac4417be77e684b402245180db14955ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdIsAlive 7b1a48346ab665acb1cfde595c6516a6bb8d96eb5ee4bfd0ce524358a1d6db99 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdIsJustActivated 30c10b503b5404f54c87b1d90087ed06e7f3b48da5c72b8e4ac8151466c61cfc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdMouseButton e706097bcf949c693c2aa6516aee36f07da29e767120269d935671c994a5a2fa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdNoClearOnFocusLoss e70cb1829c9045964a8e1d1449337b7fb92c24a6547dc7588ec4f6683d4eecf5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdPreviousFrame 286775666e66392c2c208d052ff3647f97cbe91220ed64c22407a3803873de7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdSource c1732473f05bd58a8bf81f72fb867026aede9a8fc0ce08ec91248a2a65ac44d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdTimer af63addc91744f112cd3182b4b215ece1f7dc70b1fe08e4a50dc603e1c503858 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdUsingAllKeyboardKeys 73c64e1c14c91fab9a45b8dca2c387dbbf5466108f4ce487073ebda652b216f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdUsingNavDirMask 847ea6eea81ce6ac80e74553ad130748a8c959e0b223399febc1b08d1acc54d3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdValueOnActivation 93ae86d30444bdaa963893d21eb2cd1bb6b4f1e6ed67a5a686dbd9376bb647f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ActiveIdWindow 446ad44e975084d92042995172749db83d10c4fda0ca9655ca2f2ffa1d437f77 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.BeginComboDepth 1e816987bd76979cfbbe6ea49a82b573003ca8b2210e198afe98f539688db58b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.BeginMenuDepth 8a65f6f51288319e54ec7a0fe5a758f95ddd9d5d87c18eea011059d0756e33ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.BeginPopupStack c8462e690702fa4719119efa8be3552bf2f052be8c6433053891c6d29dee6955 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.BoxSelectState 28a9e97d5d0c32d99c52535a583cb6f80a73bd94d8e13ea513278ca4d36795c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ClipboardHandlerData f5aa39d03d9a086f0a61c3597a39b7dbc6df407f4b7033d0cd126b99d15103d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ClipperTempData d61926b2355a87df24a768ec9e189b10a5723b1987186b7f60a15ed0319f22a9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ClipperTempDataStacked ea8d7172eedfea611d5158f85ebbf481e378bb9766a52cf74e026755f9f4bf4e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditCurrentId 293106098026cb4a1f0ef7b7598774d75dbd1cb971bdaddaf32534ff0032f8fc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditOptions 07a3bb1a727e462030b86df9b5124c1bd15d7b19ce7c8255a473c05ebcd7cef3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedColor 378834904f285d691d1e60dbce727d6da6346caf3d8f2b0f488e58800176feb1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedHue a6b8e3a40f392979dfdc62ca2bd98926bc6c7c5f73737de113574ecb0ffa1421 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedId e79c4f4da1d0846bd8e7993f4fca3bddf3c68c4fefdfeef4fe2b0dda5cc215b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedSat a62cbefd32f429ab6c4af35752463cdebf96b62f595805375504dbe757fda1f1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorPickerRef ed9f4c235153f0c115f38701c8b96c2fa10ae4ca4d2b93c834e8259455f23964 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ColorStack ff561f597dd3a75218a581149111cfa285d2c9087c6eb02790cb9f4313b3a649 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ComboPreviewData 81b10c4171fc8b74a806c104c3375613915551376a8573e0a3185cccca63ad15 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigFlagsCurrFrame 3cf82a677d96a7350fed91668ad4fc9f386357a5b955cbcbbbc21af250decb27 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigFlagsLastFrame 9f1121e2b35ca217c21c38e5264d5a27a30062444e955914866780bf262ecb26 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigNavEnableTabbing a5dbccb2afc523c886bd4511d70ad08109f6f3ffaf7867365409348859af6892 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingKeyNext fe39d6b23694c55910a6d4e32b7f2a54a2f5b964b0db1ce92e897b2dc4f44da0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingKeyPrev b968332c20f105fe9fb73c5a7ccaa03a6ab0ab68c7956d94944c6d1b90ca8bcb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingWithGamepad fa1346c3f0061e240e067b856a021e30b7b1c2d48ae2ba97907e28d683467cde -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ContextName 6f168b15ea04c6b084fceb0a956266234b8731738a47648b322125296181e65a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentDpiScale e4dfaa2611417e1847233a622649b898b5c8f7b4f3b03a1c3e932733e1a14ea0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentFocusScopeId aacba9668781aefae1a09ec83b7e90e06b62154a9728f60b279feff7e0b992e5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentItemFlags e6e27ca5b7c20a703b02da1a7c08054f32b768ca76c5ee624db72f90cc59fd5f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentMultiSelect cbf9175cbea8ad5ff8d192c4563f034231b112c50b03a78369a66c776649f9c8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentTabBar 996058709de0637612aa62b9b210130721296fa3e6a206c36d87f81e0a40b71a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentTabBarStack 93e681a15ee437a7d047b9bfaaa15da25e6a25fef46c938067032baa2bf7a9e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentTable 762ec1f71562e8dc98eb0272c034a2449b948c09f80d4f377fa91f420a551b7f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentViewport 0e3b2a321085bd66dceb8a0f6f9f1b8058d9f238b7fd39ced3756852e40d4054 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentWindow c1b4c930ec40bc86081820c5b44e5e278ba2465611b34dd587c86d3430544014 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.CurrentWindowStack 3b9e8fd0d217c3214e8b448249b87b3e0ff74619a897de65c373d20c6ff5d8ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DataTypeZeroValue 5573707b70410ca80b0400f28700c25914981f25fef9dc3eded23e207980a5e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DeactivatedItemData c29ce395810e7c7e2a39aeb67d793eaa838dfbb9821b6d15604dd1add60091f8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugAllocInfo af57a499c53abac45bac316e8e304cb5bc5c172d3475e83ca255ff04a2082ed3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBeginReturnValueCullDepth 435e08ac2a0dd9cd9800960876b8574130aa9d76f538ad1bd9cfd07b33055268 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInLocateId 5e80b9426c7acd59ccc9a18b3bb5ff11bcd354f839ea6cf965b70d32d4fb8aee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInShortcutRouting 135ffb4c15870bed92632addc80fce73532841aa7c5dc1a181f9981430d1076b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInTable 10e6ae18c6c02f463f475d122279abeb71475718827370ff5b58c8e05c6a2f19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInWindow 81ffa6ed7c2acf9f518e5a2a1e81665bd1ce32eec0a2c8506bca8b6628e28e95 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugBreakKeyChord 191f503bb53d3290d31c9abcf12df777523fae1fedc8a97a562f43c68bdc81cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugDrawIdConflictsCount 8d6b6cb7f4ce1996dfeeea0efaabed4308574aaf9cd8b43a18aa7edf2b8366ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugDrawIdConflictsId 4c4e258f170edad4d5e1f17e244969cb26b31c4a20a2d620e92a25bf7dac925f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorBackup e8779fadac75702adb75389853ea1c6423ff4ac217ae67d11132cc3b05ad5b7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorIdx d63558cfc4d49c03ff32e07b7ae0b2835fe69677aa4f04d2e689b38e51c8ab00 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorTime 9f2479ee04d6cb1377ee136ce4f01c5ca4b7cde101793e5133278bea80c6d82f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugHookIdInfoId 56fcbdebc42882d0242cc40c205267974717ab37dc117ee6c574296ac635c856 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugHoveredDockNode 2c7c876d80890190ca11de63d429ddce8c797ff1dfe897075608e8ab86a4cbed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugIdStackTool 3ba133529bdd7949d7ff19b906d1f7c72ab20d7cb77fa0882ad62f1fdddca7a2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugItemPathQuery 0211d062e4d75a2be5978db80da2bc2c9988076ff3e6a8c91b053eb0cf368fb5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerActive d1bab8756193cc886af5e28583080ebe8b12dcecf6714fc7418aae1e4686619f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerBreakId 7be65befa613dd5e561ce92d5092d69f87d5124c0edf9a0af3ae60e9caac5a6f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerMouseButton d0cb7b5e41cee1331adddf4f2b15563ec872e6f3e74658701a8ef2054395fd29 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLocateFrames 9cc1fbae374b8a10b8805bfda58d67236f8ae08643cd20eec098eb50d9aae733 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLocateId 7ec613b19c03b89da75ac4b3bae275b75fd0a74ad7ba7a8f0dbc05a1ff07e24f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogAutoDisableFlags 4c6055590b0ef6a0107223c5ab13b3fe1b6adedaf4741c4f0c06ba57d550a4f6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogAutoDisableFrames ec2d68e038da6c9fb70c097417e6d12d3eaf8eb28a46399c3e64b9a9ffad33e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogBuf e54e8ad040d62ca6add99cb5f4a569558835e674775114c31a1a2fa59fe6ba3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogFlags 000c20bd4031f10ac6cedbc7c3c3a0847062b5f00e28f634c751bb250ffb6b44 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogIndex 8a7e7e4fa4303275662cb38b6b3a1c6d81a6b81019553e69a7f90878edad972f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugLogSkippedErrors 3374a52f5acbfbb3fe62508ea30b39ba410338eb4101f4125d8c5fda3734ca79 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugMetricsConfig 5c19961f99bc77c49f30f13c1f76ab6fcc757d651935f83dcf333585513f97c7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DebugShowGroupRects 7950608d8ac8375c1692c72890c99d02aac2cce42ba8c56f0ee10310c774fb83 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DimBgRatio 6e14960193acdfd60a68cb0dd3e3c8330017c7d611a6e8ab30b9001e729e0067 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DisabledAlphaBackup 56cce3d61dc8b2481d8f97f978253e4961ef73d7280ce637ea2b99ea1550bb08 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DisabledStackSize fdfaeda10b0486641a35f9b74d1eb04561ae1b25eaf7c22630b60ed00e727ecb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DockContext c74ac6ed91db4762114aca69eb9f4509bb2c49314cce9c6a7fb7d4682eeceef9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DockNodeWindowMenuHandler 6e377b35b255b99e59b7b9bc2b583bb98ca518c4c712c129d155b0423633bd20 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragCurrentAccum 1b28023d440d3a089eb969fe2bf46d212361316a4e7ce63649f138792df0c8f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragCurrentAccumDirty e76a9eacd6ef77dcef7dc8d02c84bb1e21596b300c3f7933759aaa14b3bc2c2d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFlagsCurr 4da4ee104ebeb9bfc9d8ab906676062fd551fc9f5da850484bd53ba4b22fb5bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFlagsPrev d087d5702805233c6606d35a53c2316b82576498265ed224c5c2d110a6588401 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFrameCount b5e3b44f9d93c8ef20e009c5f533d72864a4be9edf1f1a1186cd78b8da4108ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdCurr b3fa8d5336b9e102e20191c69e58b9ca19f92ab90d17c07d6072ceae30eafc50 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdCurrRectSurface 8ca40d01ecd4df4bdd43273e535b65ce4075e180175baf67d8e394095a303b88 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdPrev 130f67faa9f88e1063eea92be04ff3714dab3b55f75927b25816f4bd143d1874 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropActive 6cce3c60cec0635f9939f17f2d182b6438c573b77722935debab183edf10e657 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropHoldJustPressedId 07cd19cff2322cfb53b8cc9e3eeb2a468df4e29aa085a584b9aae0b017759862 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropMouseButton 4860fb8570f7f5fcd5d517d4cd1a9a9adc8eafacebefe1e8266b542970ab822f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropPayload 376ccccee9ed12d9b88010caa8c375f9693c943f63f828a0b97ddcdde93af0cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropPayloadBufHeap b0370ecefed90b70962c072e975d346615afcbd20959ad10a81432bae42c0ae0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropPayloadBufLocal 58b0a62e3dd435c17c3c3ea253f876c3b784255403ce3fa70fce6380a3eaa79b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropSourceFlags 14ac490543985185c0c69f854a00e860c9de826284aad9fdc5b2c7c32cacaf01 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropSourceFrameCount c7ffa2cc64f6fc3e0c9e554b828ecd291d3e731c1071a70ceb7d4ced3f9662a8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetClipRect 50a99dd5de19f40892f4cf73d0f542a38eeab603069d0c243b6e0564beedd6cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetFullViewport 918c5595737719f49050705e513ffd505819f7faf2df28c5d27946aed8fa3541 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetId c9a12c6983e7cdeece9fd0741301283b5710364a8c7663925313ba7ba4783b73 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetRect 089e80d3d94bbe65fcee3783213d72b8a8fc8abefe3be6e416e91895083bee1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropWithinSource 79e8314012d2f452a0c181f7bdc68287396127b6722ff58547ffa05059966838 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragDropWithinTarget e5772d5f4a00505bfb1deac52ae9511467246fc2b9eb3a863a6c4a6661ae31d0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DragSpeedDefaultRatio b611aa5adcf546de40f472a176057637074365edc6fa94098dcd022f09e0c10e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DrawChannelsTempMergeBuffer d1192832c4c83366922ce8a976e77f503bcbc54d3360a9828b02208e33b3627f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.DrawListSharedData 440cf1a71063a55095d10939af0f6a3f015904b01b1209f6b33619f17e6f6bd1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ErrorCallback e70e8dbc7ff9dd8d443c6e3eecf93396d32f7bd1958b63f56f5663650295629b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ErrorCallbackUserData e6a6f68e2b704d0d3a2769fc1bcbd545c75d3910bf3cea51f9f4145bbabcc709 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ErrorCountCurrentFrame f8b57de7fa6ec8c188d0ed06c92e97304e04a364119c1be4b1fd44f9ca0ef44f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ErrorFirst b9f541d1afd5c3dff54981028702b133876920028409660357e71522195f8769 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ErrorTooltipLockedPos f9d3e2a482a76216abf53d7bd3f1e68f6c0c27ca590d7cd1a52b6adf3117fdf1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FallbackMonitor ac6daedc749c3a7454cd5e5c389c435f1cec5dbc36605137475bad0a464252ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FocusScopeStack e08defec1f260a19a15ddcd889f32b816c17f8094b49d227533407be57fb6232 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Font 2fcbbe67cf233e57873830d71899b2b57004026b28ca409f42bae7893fbd455b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontAtlases 698ba1ec304370570e770d086be62c8493e279c866d2f01de6c21d689fbf1008 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontBaked 0552c2e428aa6889768214c1b4119f2e09f78a2a234fb2c9d127059e866017e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontBakedScale c33efe525caa335a064b58aad6814d1a5022a48c921d06b69019566b145adf92 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontRasterizerDensity 2ee75dd631d88b1563a048bd671a1c73667fdf8caae51964c68fe6d033cd4d90 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontSize 4fecbf76bbeb1b69c0194e99c7fda3958268ce70df18f8e643020080a17613ff -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontSizeBase efaab3061d1f8ccb2fb338a50173beaa9c7f401b677ed01911ece2e86732a1d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FontStack 745c838ff982198792e9dd32e34f6d29a80edec3006f2cfb352fc0690de2cefa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FrameCount 521dbf1a0a4d26a4d2f62504df6a8917ab2c1d9288d4e0d41f825e261e733384 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FrameCountEnded 8ebfd4ab94a32ae9c23e7435b30bafd1b4b7742219bbe22998c655b23bcfdb4f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FrameCountPlatformEnded c846163188e79c871e375bb56a13892e2385bc147dc1bdaf60ad20d4da6d2446 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FrameCountRendered c655365d08e7ec09fcf71c455f98dd11bfb101ca81becf7431030eefd7383420 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrame e6d404d509eb72ea72ac6f30512c5dba679441cf47d0d4e05af2fb5b1f7b0bcc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameAccum 41f5ec1f120d0f099fc8e0856aee3f54a19ac674ca89189ad1d7245e7b32533c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameCount f5b341caae362026653d3ab02467a04f3adf4ef5ad1d5b99a8cea29c8b4bd84d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameIdx 53a6cd883504dc19eef59f2cbba114f5d882a4ab9d513a51670e2805e9e75a2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.GcCompactAll 40654411cef8e93bdcad5010f79163d6ac6640ddc2f28d22436c47a7c81b889e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.GroupStack 021680a90567166b95cdc5d9f4962db0a0d540abc46ef0dfcb1d607626f0a3ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HookIdNext 04800191e324b1eb23e2896eb9281199ee8fac8649d53df3d68c58156575d7d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Hooks 6f173eed5541aed11a9b285d6648ba5a377ec2abd165b2713a6f92b701b1d738 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayClearTimer 72a55d61f32f74f69dfe097a2033ad0dad16610bcea1dd5511ab12efbdf706e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayId 85a69924f975c49a02d0d182f919cfd42dd7642952f99555ef88bcc60474996e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayIdPreviousFrame 471cc53aaeed5778004ec9493dcd0497162bdf516d4445a16245f9fdc426e239 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayTimer 0a5868945b4d8d85ada0d2fa007caa78397076b7d6f90bd6e8a1c4ca7a4a736c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverItemUnlockedStationaryId 69acf23f32b9816c4985cfc280aa703b7fccd8808a12bba9dfcae3d8a1e3be08 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoverWindowUnlockedStationaryId 66faf76d84578a71d5eb7277e3e3ed2bed08f0933857eb666c8529f541bf93fa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredId b50db2a7b6789ca084fbf9afaedcb4218dacbe35579b17cd1f60403762754ca8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdAllowOverlap 2567542bc555f1d8167e2194493ce84210f79d6f731328ef2c908f412e61335b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdIsDisabled e750c6adc95b003c1b8e42ed8f4288a37e3acd99d955ab99d0935b862954081f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdNotActiveTimer 69a7e5a5d5cbe1c66740a1d44bbaefb39e788c21a17bce7bc03e110e53050c87 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdPreviousFrame 6b4d8857d2794e06ae856f0a0b750efd3f5755273d695bc4aa552935c49c7e23 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdPreviousFrameItemCount 2d454ef72b56428147d2e2f310a8f6fc0bb5140816ea6a628d8640a1f7333531 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredIdTimer bc1d5fc4e6b517cb05375b346fd48ff379465b6ae7c942a9f2d7315547ea9628 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredWindow e024f5cab1af6d2ae55b20cff56d4a4b282f05f6305212914151d31abf7b2c1b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredWindowBeforeClear ef7f7571ac057050a9f0dca7c31b7cf958ee54b32070211a03fe8b8c29e875b6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.HoveredWindowUnderMovingWindow 8ec59dc91dcd6394e3d2dc4ce0371dbed06f966027b14af68de19a4af724967c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Initialized 6cde647fcfa68a0e155f85fb66e8b0fa80f3a1428efdb55f8d827553a7c4a407 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputEventsNextEventId d2f44a39de2aae416a3a3d1ae1981d66a9c3427048311a743b744872fc752a16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputEventsNextMouseSource bc00c65f99fdce2d7c80778ec359d10a3b46b1c883753089fce136216fb8f0e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputEventsQueue 2cf6845979f9a0f84fc2ed5ec3a26224639f565b72762b1d53ed091529ff7018 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputEventsTrail 545076d63656f63f0aae5314b3f47fa1700b672e9e07efaf2a610db48479b497 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputTextDeactivatedState a6c9f9045127952b9da4b78b029eb30394df4433c7f756c025fb6e0dd3cd2068 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputTextLineIndex 05b9387b8c297183348481975567d8511fd37fc8931f590c3601c3df47b05c72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputTextPasswordFontBackupBaked 9697bd0cc6da893df26ca947660c760f67d13c71945d4eb3ecd8e051d8909a5c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputTextPasswordFontBackupFlags fb22be789e828c4d69584fb29d5b93475d667ea40cdea565e78e1e3e2aba59d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.InputTextState 6f63209a0fa117abc306dc036d7a71c9c0ed227004f8d91aeb67f11a83567cca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Io 75b52cdefd5c34486e91fdaa18c5032f530d12d0008591a083370a6efd836c45 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ItemFlagsStack 0935b235054ea9b364c18650cba6989004fffe9bb1e4c82479924d9ed18965a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ItemUnclipByLog 999c726b281ee70be29a36ea20f08ba4409157047f144291dfa027620b6d2487 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.KeysMayBeCharInput 34cc7a46f4b551b33bc305f599ed9e5dd5333e9c1086cf9194f04bd2215cd97d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.KeysOwnerData 6120652960b1344c5472c1c8eafd810a32a544891fa5e0c3f325b8dbb61684eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.KeysRoutingTable 85c1ea241aa2b865777eaee60795e15561534ef3bdd868fce6f7bab5de90eea4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastActiveId 56ec196ab9cac88538de87fec9f4ec2091c176b175dd889881923a4bdd2474a6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastActiveIdTimer 8efda72bc09a74b81e57d87203471fc2e8f7116070744c3220ca29e13c1305e5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastItemData b5f490135126a4bbe9db91979395c103a5555f363e18706db2890549ebdf57df -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastKeyModsChangeFromNoneTime 1e42303f0e02c48d62aa16384d7e9ef270676d8da99a3c431fce37835893ade5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastKeyModsChangeTime c4d7c8d7eba21d166c31cf51982990e1aad8ca8f986b96df0b2d4d34a63368bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LastKeyboardKeyPressTime 01d1897fccc2709d849d4a093273f42ec86862e127c9efd3a0e756f03c7da64e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LocalizationTable 4bc61e9c6d0cdea483dc483195ed884facf5e1dbb5d3767dc1623f1485a686ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogBuffer ffb0fef420b217cd521c9ea1124f0457a58de73d9a003d80bafba2f4a78f948b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogDepthRef 583709daee0250fbf7f468121b7532d3fe64a01af569aa0f41442f4e0f745a14 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogDepthToExpand d21e637c39168447dcc096d9e451ea78a5ef7948d1a57796cbc1e79d7933981a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogDepthToExpandDefault 3d34d8ec16e6601282b61ac35994a9b8ebb95dec2fcc8cb35dd210b9c71e1126 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogEnabled 3913f42de4f1a77b1cbcc3e075a1edade3f396acd3dc186f2dbe9deed74b436f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogFile d618bd4e4ce4d881ae14db863f71196b344e1ebba0687652723d80fe1f4c6fcc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogFlags 77697b6f8b430d25c98a37d88b87d1768028bac17383d62293088218b54d9378 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogLineFirstItem ac5b7e41ba284e38b5a1e149abbbb35ecedcc24e1a482bc13ca13b52cf8019d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogLinePosY 7d10ec5eb10d5e28fe514f1bbbf7495bc858b2027308c43adc9ea4e895086a60 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogNextPrefix 3c7557a18723f5bb6c471ad854c909ba1d19275f85e5d09878639138c536cfe7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogNextSuffix abe49d2da9ec0c454c0a4c076ee5311c98ae4868a3bfd039f08a35a595b85e05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.LogWindow 357abe61d850881b736c0cea62ba32055e2a5ba0884b4c900109194ef35920db -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MenusIdSubmittedThisFrame 217f8536baf91f44628758214259ff829c9d03df10bb21f9ec29c523f9121dc7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MouseCursor bbea11c2a6a8a24dbdcb9f10d9b948d1b9c4e6154f63bc4141cbf2462f1be27d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MouseLastHoveredViewport b13bd7fdda2d7148bb781ee7932546cb6eac7c71746341e3fa9dec5c61f0d566 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MouseLastValidPos c744ed2027bc19e95efcc0c0af4233e0e57b531698f2edb7170644d39d296ea6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MouseStationaryTimer 9b20986ae294c96389f305c62f840c42a1035c2802a4f0a13156320225361d95 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MouseViewport 2977020f25b93bba039c6bd3bc81dbc5043c176c5f489c22c79cbd0c59809313 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MovingWindow bf78b9574d36572d651c77cd62564fa1606c37dae4fb188ec4ef4ebcb2c02a68 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MultiSelectStorage a6f2ba03ca65861254ebd1c272d41b9be5e44eac1b7c974fa14e204915c934e1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MultiSelectTempData 8a33aa9254f2af38203873a3435cc7047c2a73eff0c8b495bb2e6957da7cde31 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.MultiSelectTempDataStacked 87ceee86389d7e5653bfba5925fbe272e8bb30ae1f82b11ad63054c1d6fc3ed1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavActivateDownId 18870664355d637d9c87cc6fc1959dbc7432f6cf93349741c7af2fec1f10fcfe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavActivateFlags a5a87b6ae31591831a3be34fcaff3d8eb5b11b4e66e8011ec48fef5121dd17ef -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavActivateId 1ae7027e87915e8062df57a306630660847ee32f976d2d3757063304ac065201 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavActivatePressedId 3c963dfaa53373cad0121f6a679ef59268ce8471d0cd44355691dce9bb01f624 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavAnyRequest 1b65fff1dd0d14dd55ca3f810bbdf9a80b5eab45aed71fcf139e8b32f221b326 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavCursorHideFrames 7afa42ffe198ac6d36224a348ff75f526a3ba934e6939ac4adec73e68b34e2e8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavCursorVisible 906bfc45f059a13400f7d1c29afb41aa83c7e88e61d722da67cc81699040e853 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavFocusRoute 59c31d76a3693879dcf10888d0b484e7ccd98854d4d3fda168a9bbf53ab39d97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavFocusScopeId 69e8f755ee131e845a008693d260f2be6ab6e3fec4ee7de57518bb790de9e508 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavHighlightActivatedId 7893bd30f71ce27e95263fbd97a5af60a04e55f36596d7cc63280e118818622c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavHighlightActivatedTimer 6bfe7d74ab1fb128a9b692e295882d02aff5068808157b01297e7169425a5cf5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavHighlightItemUnderNav 8c285e7fb45fb42520bf87900d5dee08696b73873525893bc41c66db23fd1d05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavId 52d17bd3a8085ad197edb30f3a89e4963ec6a891296b2d52b5d9b3ae2b34a2cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavIdIsAlive 0b46769853f0346f901abf8d2f4d13a134b840f3dac074a0463773fec88e98e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavInitRequest 6dddfeb9c7d6303081ca74389e96262612617616cb8ffce12bc56e4802750799 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavInitRequestFromMove 6616285fe464cbca40e63df055149a2d4001d6b57b2d3f58c4789e87debef9bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavInitResult 62082e9d70e3733c22714847b10c78cfd83057e24e5efcd554237b1cfd13be3f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavInputSource 5d558d6b0f6fde1bf533cda31340ef29983bb4b1a62093d52b9688930a3af15c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedFromFocusScopeId 974105b76fa1fa416cb1f2a8a7a167a44605a457e71347aebc3b44012df9e02b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToFocusScopeId be354e8130574c96fa7748e5d750223009858e77cbb12821aca4898c6dc9a2b8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToHasSelectionData 191d2221aef1335a68aaaf2bbd79bbde24981583e0c8df02c4c5e0978d28454e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToId 593e1991e7bd11ca3eb93688e35a57a56a2545512e6a81d077d2891c9a0cb6de -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToIsTabbing 54fa5d45e830bd76dfe31875133b9d82d0bfc3197a95766eae01cf9ee4008e83 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToKeyMods 78b9988929eda8d9fe4e267f3fb8c7419ebe46968b0aceb734f845529fe981f8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavLastValidSelectionUserData e71488f7de6ab9ccc9477ab1daccfc95034db6b53b8cdf4f20e595c9f2629f36 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavLayer ba2a99fd486e5cab5702e5130c7844505090d4787ba7387eafa40baefad099e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMousePosDirty 342f11331a3392e93a6b23d0851023fc38e98a47620e18a7babf0eb177623eba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveClipDir b3852f7bbb9df60deb0b0ef764a31578bb5486b030c1093f4a7c63ed384ca15b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveDir 88e8166f6cfe808a4c9bf9add406ee2732736ebb2727a312fe5b30e0421274f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveDirForDebug 718ebb9627b42aa28c2036444d8fd98f33f8fae4443572ae5ad983da602aab02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveFlags a648c0a65f22f6532fa7a2482429cc474a366c60b59742c8032bd90eed8ae064 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveForwardToNextFrame 382a0c62abdc47eed098ee331958ef0a533c1ee416a8697914e3610068bf6068 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveKeyMods 13169154430aa6bcd0d4a50eeb33ad5771a1b4277652029739f4c0eb50734593 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultLocal e74124c6b5c093b453accf62ce4585c311eb1c4d9837b9a1cbc5c25dd431c9fd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultLocalVisible 07409e095527b7c7f4fa32d5b99b515f770933f03fda7b63b824e14ba124f7cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultOther 1b267b833f97e2776c5ad513ce1ccd0304f18e3daaac6a2cec7cdffb79ed37b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveScoringItems 07f120c0768fd3680a629d141c63437c481181778dc889796e62d6347a06c388 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveScrollFlags 03a84ac403620625548389b6960d44a7dcd74a14b1bb79f0031bd7e290bf7ab8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavMoveSubmitted 0590ff0b2c5dd19c9246880b702a176893a59b938f72a3f50b65cc8fbafd2f9b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavNextActivateFlags cd09a05e907a27d171cd955e6813a29bc2842da1d3db24007848ff3c81bba23e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavNextActivateId 18ec7ad3f328cc54c6e13107ad130c618b568d44aa95d7419b21dfe8b3383539 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavScoringDebugCount 8cf60f6c811b31f1a673cbe591e60251fa2d0f8dbb06fe02934f6b4c3f5cd6b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavScoringNoClipRect 44fa6f5dc40288f1f4928d664b9380aa005de47addd0fad5a6f737fd9538c362 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavScoringRect fdd0909a71597fe9dcb173cbe96273ccbf6f5dcc1da009bbfde7a983261fe8dd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavTabbingCounter 374a25904c9945247d250266947644d0e9a8199693a4f17079681d3dc1965c7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavTabbingDir 87e581dfa45c17fc37e09ba038e60da8465ee9b1ebfae89440814742eeca3fac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavTabbingResultFirst 736b1d73930c5a1f7e8cb69d96f8628c2ed1e43123ef7eafed549dd2fe77eb91 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindow 350fff254b7b35a50c968941ec3d088ddf6379c0f3b4c2c49faf5666c11dfc2f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingAccumDeltaPos c0a901e782422f4b79c41c41e6d6676fe8e23996b3e99d8af73e0a50ebef84da -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingAccumDeltaSize 0d303a9585981dabce54bc27b09b7e9e099a6c8c518ccae1e84d7132a977cabf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingHighlightAlpha 6a337e13b17937f774a8747ab02c2515f614efbc0a4bf5083e47f6ebd69b4a85 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingInputSource b9fcfb8177512e9d33b4b17f6da1731a60c9addf88912904d343be009a7e41ce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingListWindow 5b4415acbebe28e08c1a179851d90d180b43073ecee751cad2ccf43900024961 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTarget 1b613488c59e206e8abf868e5b67009bd1c29f8a5d929b609ee6e8f730e43a8b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTargetAnim 4f4757e1b83aa14cfea812192b4450302fb145786803ab1c59dbe877d8a10d68 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTimer 38cb2d93d9356428b30e0ca5af2ac4ae20a674f9cce4b8724aa22be42d8c04ce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingToggleKey 46cdd23f2df472a9f44541315dbe8a4ba4ee503ab9c03e7656f59a25277d8625 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NavWindowingToggleLayer bad075cdca36276a80690f3dd849ca394de2dfc866a0d1d6cdfe09c766bba154 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NextItemData 16779832f61f1ec999dac7c83c88a154176e2f6996265010ad2529582d48e70c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.NextWindowData 9a606ae1ee47ebf5a332901be95df6d47da76f455d9c08c9ed197ac892e921dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.OpenPopupStack 0607ad294324f765d387699a035961f26c7d5c9729e7034b3d6b217ab982748b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformImeData c8d247aba3f3b2255ff82cba5cfb5d5249dfe44a4feb98001157a5195cf81246 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformImeDataPrev 7e4af27693c82877978de2144a9eb6fffd4a132923270261e97fd690af1e79d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformIo 140ff55447e916adec9c34495b12e91fbd87dec1e3be324d2d71e0cf3268a31c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformLastFocusedViewportId f94b4e5bf8d4da1891f016d857ae61b88cfd6ef8cb96959042de7dc25a7a6704 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformMonitorsFullWorkRect 39572c31431d3a15fba5287dc4e03a3da0b62859d4869b88693372457eb2444f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.PlatformWindowsCreatedCount 2cae78271d0c14567f903950eb9f2a7dd6bc5275ec113b117cfbb160ed9a6434 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ScrollbarClickDeltaToGrabCenter f35d05aa56331966d50914639b0062316768b7af56c6b26b9acc9e7b387f670b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ScrollbarSeekMode 1dd58fc281d2214b5941fde2032fd0185741ff2a9582d6a83b69e08d14dcfb7f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsDirtyTimer d8d58b861b59e01ab760a301590048635698a128eaf8bc07961696e3b7bcd2df -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsHandlers fb903968597a43d59d11b73b8d335aeb4dbe7edd0fa119c9e000f9d933041f3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsIniData bd0bbab6b16a31c56c3070b8b8b59d2e83ebe3dfc36a7e939152995ba690b0e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsLoaded 644917ff89319ab14371103b30600b33ed978347c94d940d6bae19d8013c15bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsTables dc19f59d0e2b0903526e4c363f518e2257ed660899061f54c8c26b3aa9841983 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SettingsWindows 2cd4375981f23f1dab6456cfcd71b3c2ddf9e230edaa85cbe5008547795ef7a4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ShrinkWidthBuffer 4a03b50c5700aedd57a80d8eb360de97b284c39c643107d61091b8ea35263d23 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SliderCurrentAccum a87cd5138edc8c847e3b9db49577449fe06c627324a1371e707eff8fff01904e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SliderCurrentAccumDirty 7051099c9a1b283342836dec979a952ca1900d1a063ade01b59717ff4933cb9f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.SliderGrabClickOffset 95e11c63c9aec7000f9ea7b40166b49d9a5d6be29187584673c55d19f8d1ab49 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.StackSizesInBeginForCurrentWindow 6aef40e3b27f680e5348b9f51b1f11981e4fade3087b48a4f540ec03acaf1e25 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.StackSizesInNewFrame fec326be12803b1c45b4accb94f6bdaa9b868c5d17bf9e7457ca53742b4ce436 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Style 3f829971b109b1a2112b2512c3c5383d86e4078c842d17736a06d1e67ebc04bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.StyleVarStack 90644e42fa9d88abd189d4b4ec0d64d782053d20c609497bf260daa6889cf5c8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TabBars 35474fb8bf8420188b0b12c830273ac966141246e217f77e5e4ac22ee5122fbb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Tables 359532849a76510920f308d4897f919803b93b3fa83241358910832f73169bed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TablesLastTimeActive d7060ef08708dfeadcc49198cab396a40b0114bb5aba05eef400ea7305a2f39e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TablesTempData 734185e93caf760e3935c8a1ee3a0eb3e4d229f7f279ae72fb9b08b7aa1a0d10 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TablesTempDataStacked 87765fad2f57e82f2bba88e3251d818cdfe1cd03b13017ea1f60a08bf038cbf2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TempBuffer ee818ce785ffa91f023aa910c736defc06a73787c9ab82d9c080c3dc6d65ecd3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TempInputId 1ebfe7912a98a8f286402461de74b836a50bd83b6c8e17fb8312499cee25c92e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TempKeychordName 5b6744f6f21dc7d82ee8a68b83607a376eb46e300e487d339b60bca17ac929c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TestEngine 51b9b0f98f016e25c4b5e5fa4f45de83d3c8bb795bdcb09286c0ee0c705dda9d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TestEngineHookItems 9c3d0febcd8f1d2be92b4d0f6a67aace2005f6c894a61ac2326a648fdf2a1907 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Time a8341b30d2e700ad160f0b2e628a1e44935757f5646ad4b1fede23c49e984ee0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TooltipOverrideCount 87c108a39ff7d607d170ac706609e5bd8212bd02a52397d624e9a6381f3e5e22 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TooltipPreviousWindow 478ff2795070115d76f46140e9b015418f6c3f9ecf89f82e166e684df452fb87 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TreeNodeStack bf3b55488928744ad087e662d8ba5977fa3ccb69816ab52de1c0f5b857672108 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.TypingSelectState 4bd0cfaf8bbfdbb2975e0184dea711653a6ddf5120d64c5e55782296bb445d4a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.UserTextures 2dfe08a7b3c03ea260e85bed9827028e5bc592e6360bb58bfef52a9e571a587f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ViewportCreatedCount 3e3deaf71af09a7acc7f211c21507e730642d63386f3a426916b0d0abda14948 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.ViewportFocusedStampCount 9dc64d4d3a29f092d30799bed20f439c9bed50fa8d61b0aa3d4887aa07453dd9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Viewports 7e7cdd6d4f26ba97b4ee99d06279a72d6790fe989b828272b3c663a4b5df445d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WantCaptureKeyboardNextFrame 6f0fe4ca59308a805064e7fa227f5989460a4967c60d4022b84f3a0e8c116a89 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WantCaptureMouseNextFrame 43aca38c4d881b35c1971be23de300ceb1ee60dadc22be3ad78174d854bf0999 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WantTextInputNextFrame 0994018b01199aec97f41dd4e87855f2664ae2debf1331da52e33876520c43e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingAxisAvg ffcda9d3c61fff68726a1ee7f223b3d78365fea2e83316eaf94cdc73c333936b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindow c23464f8a6a60badc8e6aee2a2a2b440ad77de5311d2ef91cab9bca56face5ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowRefMousePos 690f870600ea88231ec90a751039a4959cce1a6a0eaa5434db481dded21343f3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowReleaseTimer 2bbeddc9ce4b79a14056348dbc5a4d843f73ce8066be2bc7e2e8ab4e93a77198 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowScrolledFrame 1692ab60d02e6de840a43677eb80aa9a1f5c2eeb5a7ba00ffa111ddf336dfd3e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowStartFrame b433d7869823f26959075fcca7a8c1315c02b22d9f37535f91cc9113ba0c72c8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowWheelRemainder a91e0ab2f99f497326d8ec6264699c76ff9aca6fd4354e45b98be93a30daff34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowResizeBorderExpectedRect 1c8f44ac9b0ecb7223ee02ffb2a94c5b8c1d8405e6a7081746f895d3ac5e29ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowResizeRelativeMode 54758753dfccb77d03d5f5f2ed7080c7a455104bf8f3bddc55a30e5f1299c455 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.Windows 8610ede342217f1d0e0525de7d17faf3510602b9defa7707f256ba8b26fe7970 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowsActiveCount b4e3d134aa480f04e5c54739f2a733da249a35e0461edbf19f7267eb6db05bf8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowsBorderHoverPadding 1331bad02c5f908ba3eadf60adccaea5c95662c90b38f7aa06c844593627c1d0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowsById d3b01efa217e06042dea2a52d453c86925c1428029ac4dc6c9d4c4dca9684a34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowsFocusOrder aecd51e7c6c07135d42b0e011b2811eb68315384bb3790bd49207a91af964bf8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WindowsTempSortBuffer 092b0f2f9f787ef7cda7d6171dbd06aab34fe6e077c0f91d4da3a4b4db21716b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WithinEndChildId 18432879baacf37e97df279357fe7deb98d90ab226ce93c6030e06d02835e938 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WithinFrameScope da32640527d58e4adebd8629314160fe4c0394a41a39ecc02866a7ed5984bbb9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContext.WithinFrameScopeWithImplicitWindow 82b3e9ec5c1ad0e189531e93b0274ab954065b3079c2c1e095dd139e6038c124 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHook.Callback a9d89f2ab61547ec290c5b8ef3aa5732efb889bac3f721b2dee06366af2c54a4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHook.HookId 14f596a531b714624d13825d863677a1d59ca90c2f4ff6561499e50010135004 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHook.Owner 17fce0a6d3b2dd7cdcab133c1e0bceac86da5adb7d57a3f220809088aa9fd945 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHook.Type 1a1925ff513d2ad59f510367da37111f45fa8bb618b5d04d1d84683633b00eb9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHook.UserData e860586356c58a62feb4ea2a4f0d1a17d27efdf96e2f7766c7751e6ca84e3c90 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeEndFramePost 9bf7e467f2d95149daf4398f3a0a33bae1afd668c272f54d0d5807a1b7427d4c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeEndFramePre ac130190f8e60b8746ad8b4129b240899266c5a672c78a63ca565926c8ae3692 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeNewFramePost bd247069b7ef5bb2f49e137f9a914d3ee911d6655a3d346e9ae1deaffd6bdd71 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeNewFramePre 558a9806d9966d5dccf7f199b832ff3fe231d22ce87c5ff9f8cf3e6e0a6ac719 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypePendingRemoval 25a8f7acb5ee1ffe60438141d774ed56b0ddaac4d2966360788505a171f04847 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeRenderPost a64442625642f8b2403c1dd0d89d391cebb9c2ddc072411602d44702838b981c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeRenderPre 5b4c01db7c85a88b139346ab300390bb8b6d71c86d7ae938afcec75a9dcdbeb0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeShutdown 5b7924a4dd2700dba1935397c4cda62ff40247e58bc19862b38c7d28d3197cef -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDataTypeStorage.Data b106b7a819d7426e4cc09d97aa99b1d67bf8bb926c0d319867749aff627c287f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.ElapseFrame 3c8d2290071d5050eb757047257477270ccfc88f38d5fdd2a6c3657a69f6f699 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.HasBeenEditedBefore 070eea8aba6860fb0e6307d04316c9099d408ff2ff381a9379a904de6474f34c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.Id 06ad669eef85205b84d8103d92ce8f1162d7a98b30acf1b1f33ec78b7677799f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.IsAlive 356a9bbee77872e491270a5e7080b546a2123ded0f54dceca0134baa56dd2b2a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.AllocCount e2b77b3100bdf14164589467611dd86ad758dd04cb44bdd9937858cedd331078 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.FrameCount d3e4130a8403965948deee1e0994c7e5546c5b4649137a00657ded1590a74d29 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.FreeCount 3c1e94686d91d814160d7f03497f9cbc722c0e511328000ec60a1724971e2c73 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.LastEntriesBuf b0454079c3d0170dd71fa007519930ed94b1b804c16b0e52c19eda33954c192f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.LastEntriesIdx ca86657f7d0edddf1fe51480e8f7974b6e06d57bc5c1aa01bda9ef97202aa6da -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.TotalAllocCount 5b5f00215485093985135a1318682ed282555d74121cce029e1384765b7d846a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.TotalFreeCount 2b1e401cc345aaf1ae6c15ac093036f03a1e1e6dddd663639793005dd93174d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Active b20930f5885cc2401042b8b0fd48172e0c69806e4c922a45ffb8721f77a280c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Complete 48aab7463db7eca87549bcf095e33a494b6df13a51fa453643f03a7cd49ec0b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.MainId 7bac5f2e8fdb722cbbdffd433a4b6ef999cdf489587d15b11d5d4bc236036f54 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.ResultPathBuf 49b8109cfb3ac42f227ba4500c6059aea3af7e255e3f410212b5caf60b560eb2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Results 07aace0665e2f32b22fd82331b2402abf472ec305ac4d19de74ada4086388bb4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.ResultsDescBuf 7f846732eb54ee7973095517ab1b031d7f832777919c93183cdbabbf276ddf02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Step ad6bdca84b82d745d9b7da51701cce6b76f88429229570379a7f2fbdd6cf3c8d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirCount 41c4007f28ea34923934953b0d62ee8afacd8a96d6ce4e943070763f085cbcb3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirDown 8d99a9af327e8866700adaebfbf8a91a306b6638f865b14fcfeefcd2708542d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirLeft 32f679bfa49e764c66dd49ae5db7c2ef972df41a23ad79f14bd84ca94e6160cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirNone 9362af7beeaf3eb292649d6e6296670723e1df5da312cbd80e9218436c480e6c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirRight a34ffd5901244c3f990a76d2a6dc8956da7fe44abd85fb25854fbfb5a8abc49a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirUp 1797880e564677e7f647d18a97ae667dd9166edbde888f7ee19a836c0764d4c3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockContext.Nodes f8b4db06db230e77bf251ed4bca7ff58354316c5a63510d14cda62a3d0822d05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockContext.NodesSettings f9fa4f70f45d01eafd966b84ae2f849d55731be6f3bc97bc3fa2d984e6d76768 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockContext.Requests 26e50e0532351020b0a0a58cfa6bc557b7e456975313185a4fb72cb33014448a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockContext.WantFullRebuild bdaf5320d2e62cded293a0bdac31e67ef21efa561e47d4cab707f6a781be4a55 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.CentralNode e0a4b2e357e8c3400b45fabf2f849d853f5d972b1393375740606b9ec94453f7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.ChildNodes 00b9722a0f831b65d76cccbe265031122e0ab3ac6568bc81ab922320a2ed2c58 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.CountNodeWithWindows 445bc5e12616a21ea6e207657a29509b3454b7a0b4095f348a0ee1fc85244be5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.HostWindow 03d9cac8005be69810194f631b306c2a0da604bf4c3fdf51c2e364d352bd0df1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.Id f286992f703e1e57c9b206fca8739f0da94d7e36023b7121afef867c243e4776 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LastBgColor 207c8ee17b510340c0319b982a8b02210c36594e8f7db5bb82b4bd8452a0492e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LastFocusedNodeId 75777efb0c3315fa76bdc4c35253d5c85de2555cecca2464432e850491fed75c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameActive 262c47fede6fb47c4490ab2de6884a3db0ba2f97cc21a57c6033c545be57bc53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameAlive a2c05c3f1fd807657190f545c7fb145ff42df0545a3d6ff5f61b200d305435ea -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameFocused 156046d45d4685d11b4a1cc0443806412a1d96cbfcf3e2d0c70d2d5ecf63d90d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LocalFlags 0d8822a4dfcfcc65e63091d224367bc55ac22ab86a4051c5e1220636054466f3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.LocalFlagsInWindows 33ef637ad6767fb39d5dde3e5de914403480563c4869af43e087a4e11de53a34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.MergedFlags 4d5d79238dcc265f00df9b972a7bbf34f0d025f9706918b6de12290ba3575080 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.OnlyNodeWithWindows ee19420fa8e887993e59d71af7263c70adb34f8ccc81f22bd805aea3f9b58750 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.ParentNode 983407fbc15838bdc8dbea5a035fe83ec830fb81572a033953cc8281671d168d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.Pos 4cb723d583df831bb0ce4327190053470dea4848163517f480d940aebe8edba6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.RefViewportId 480d98c0173619393f1be90a04557845c0321537fa7085667f24c9c34b24d358 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.SelectedTabId 757d8848f8b95a80a792e69e69f96fc0466af3eb9b098cef732a60b7d8ee2ac9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.SharedFlags df2d27a3027dc065ab9b302f8d2092f9a9d54e27dcee5a9bcd008a11923014cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.Size 0dd9826a457ad8af378a67310de7c538815650e18743d25e982bf6f78d5b1a2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.SizeRef f0931400bb968355b266be4be0da82de25bc9b78cae9cf2a9c828cf5ce9f05ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.SplitAxis 2603a91dfb62cda77e246c20d000b868cac98d9895bd2f817005898e7f43842f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.State cc0543d5b7e71df629ebe942032118277061cc8f7312a711d23e5ea98533bc37 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.TabBar d521d64c969a3c38154830f0bbdc70ed5f6a9a7723f907ad6e12adc75f26fa33 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.VisibleWindow 2f60747782193b01acc8b102db998c733a681d6fd7ddbfb2acebd50ca6e2e9ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.WantCloseTabId aa9c8af06032a677856bfda10d9ad7956cd76a5fcd69ea21957c2a4538a8d3cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.WindowClass 8c2e3c92ed36f4c29f8af00ae9b45c4f6ff7a034b1a8242ba871caf291a2f288 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNode.Windows 3797bb7b553e5c43ce0ac37c4763b31dcfd647503d16b4d0b9f247d98b2b55cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowHiddenBecauseSingleWindow f713fe2f657f600a5ff4caa915da6bb4fa971798b93e46c5c5fd11c82bd5d66c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowHiddenBecauseWindowsAreResizing 3c2048de5c133c65a03e4abcbf0676b63fe64bb81440432196d21dfe53d0a35c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowVisible 3eaf0e16347bcfa8aad706ab34c1e04e226ed2399a8afe14e518bb353aa96f9c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateUnknown 5667cc4ddc5aa6422360ed58251a068e6b65c01b203b97a646a38a3204712c0a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfBeginPopupStack a7a0f2a27752a9e2e43215fbc958e4349c75a7a65e0b6d8725f91daafda3e993 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfColorStack 23473701916801666b0ef50ce9b1f001d754bc2db524b3f0f76a87b8feb479b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfDisabledStack 5d2da422c9d461ce4c6afd77cd7be10bf12e72c76ab0951b3fdbe7425cd4e7d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfFocusScopeStack e897350bd35445d3541d3ca488bf9f9168e2bd64bafe77caffbb40362fa3da82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfFontStack dbbac0f3d2937c53001a0c47f656f7261d3d7e8464cef2fbfc68f09a517c8fc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfGroupStack a9532e90c7f32a4743c4c4085f7a087ad9d87f52fe89dc655fb61f5be07ab16e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfIdStack 4cdb5d04dbbf713e830dc8a2b70cfbc83194fbc8fb8a586a3733f69b1d744904 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfItemFlagsStack c18d80459fcf4ede633172ea95dc647477cb9efae414be7cb1b09565f01ab12d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfStyleVarStack 49e6cc303c391c61524b312a5a00777a015c80a4692431629628dce043e85a10 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfTreeStack c830baecb820a1cf59b0fe05433d9bb1b6a2904477fec77bb88c058136131aa2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfWindowStack ad33227dd1c4b018457cf665ab13282ffa84b76037b75372c4e7eaeae2aa8e49 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiFocusScopeData.Id f5dd1708360801c25b170b5bebf6d3650b3e15497436699e136da4a403635503 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiFocusScopeData.WindowId 57dbad4fc6d6815c117521ca82cfe96de384f02684803c8056d5659387712b5d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame cdf7cb3781661bb1771467293fc7bd1022e9484dd5e139b459ca391e1906f890 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupActiveIdIsAlive 30581a09d0f274bf906c176779ec3899911cadfa867c580e5100cd8f9190bd83 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupCurrLineSize d0a91ace7311f4c191366b7d9f80686fe4a7d9f70e7eebf8fa3877c4f0afd442 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupCurrLineTextBaseOffset d9cb781e655be66acef14ab1a61c2a4dbfd78844cf98cde44bf0fbe672b40290 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorMaxPos d79deb5e89df3e4f23fd4ae514911776cdd72c81150970265bc1f41f8e150bbf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorPos 9e8c299543f934e39551178f1786f58c96db3b748bd6e022b097312cdcfe0565 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorPosPrevLine 69d74d69d60a813062e02124e4b5e44ce9be1092819f2e79c5722a1f33f4b1d2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupDeactivatedIdIsAlive 0f8f934764be988bc80d52229206107178ab0db744ad7429cf26a2b9ddb08828 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupGroupOffset c8230b676fc563597253af4ea897af47c7b47da65678268033fc1c25f1b678c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupHoveredIdIsAlive 14e96428dadd0eda67ce1e976895c30bceb9655c4afc8ddb8f1b9735b1221e26 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupIndent dad61839e656e0b6097669a5a47ec9b0a73ae9af1e995910c6ba07f9b907d9b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.BackupIsSameLine a1f206f6894d3818e704d86245305beddb30c01c1d0f3e9f0a983d5562a7166e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.EmitItem 8bf3949107507eb429c6343e55d57b56267b90b5802d13c064450c7c92e89cf3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiGroupData.WindowId b45b0776a59d32b65abc5eae771aa00e7cc3937e28e70a170fc24f065089c408 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIDStackTool.CopyToClipboardLastTime 41dc9aa8710748f91a05e9d3bac853d3809889a33f8af253881ded85985614bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIDStackTool.LastActiveFrame d4f971cb919233bac7cff9561ce3bf763663c275de9b6475fa2bc458afa16420 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC d5e87250c51359988233e8806c7f7b75bb5ee5b48363d60ed8de856cc03c312b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIDStackTool.OptHexEncodeNonAsciiChars 98383d270d3caf1329e98905b3719617f41aa3fae5520add35ea9e6e94ca3f6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.AppAcceptingEvents bb1412aaf9d53321d6c0bdeffd6c23d1926bb4af69a043a867dc701f695d7e81 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.AppFocusLost a0386e01e6e49860b9b3d82aa8d968c3eb3bcf1919afe86b53f0b6352061c978 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendFlags 0fcfdd9c3be78109758f004a335f8bf424fa8628f1e817e165d0f23f9080b175 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendLanguageUserData a00fa03872dbec16072264930a6c2a07fc47eb6c607c9599e3a4f6693b6bf9e8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendPlatformName e7e95d359fd8ba2b9432de799aa3048e8f87f9473005d56efff97a1e31bdbcc4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendPlatformUserData b2e1e15bc237b2d840eb22e916a0c66234bf4ac00e233bf0959def61fa10343b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendRendererName edf64bde8d59cd9b8b6b305d94c52a81206a2b05c1c06c0ccf46a0957bb36d17 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.BackendRendererUserData 906af0b245b8fb662c8bd8fe2d85fb07291a49f2e2d2e779c1e9319efcc45d78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugBeginReturnValueLoop 40e0d52034a7e25b1e41e8c84fd10a8cd6226f3de913849b873c3ba9c005212f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugBeginReturnValueOnce 873b3d1ac832c018db220d3c6fc593e78e2c91624ef964301a9a2ed677ba82bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugHighlightIdConflicts 32a2703208d0fad15414ec28bc680c95990d748bdc4f52126993c24f205030cf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker aa5d15bef2503030d25be5c6cd5cea06b3ed9036fe099685da04f1873dad7f87 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIgnoreFocusLoss 93ac9b32b78b83f5afa84ca8eb5a60c703ca26a28d441fefb7ad63f27e660d67 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIniSettings a0c195a8e53f66bf4729e8c588778253869b41e7ae689e1181e9bc67a5981dda -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIsDebuggerPresent 4b20b909e36f0a64c5a83bdbd4f164b682cd8e74a0f96ee4f0cb8a99e72ea051 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingAlwaysTabBar 1c07649a7c61de1ebd117a02763aad8f06430254bdf1fb1dded5d10e6d0124c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingNoDockingOver c359875cd70c32ba8dc05ff7252f26f59fc2f29ece6aff53e82c2b325b9c309f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingNoSplit d54108b6d5714580dc23c88c37d6be70eadcc262fdf82ba6fc3d6e40630f9cf8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingTransparentPayload e32ce73ede27ae8b10d71280fc7e20d67af3e997f05005e398c796e17273b5b2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingWithShift a9d45bace92a4f7a47b92f53defa997e6ae81afcc3fc78a3f388dd29b50a4e47 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDpiScaleFonts 577328c2cc2d0e355c83271fa72cc6af7676adbd9c180df07b85f8a79fcb2429 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDpiScaleViewports 0a924a753ccdb79f4b73105b47ed21dbdf7e4d49885ef22f08ae19f7c3cbe68b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigDragClickToInputText 697052586816e6dce8e434ec7ca2de1e3f209edd5f7caff5403e2268dcc0d650 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecovery 637da2149eb7ef37d32058eb2b9fe2a80e0ec9d5b1a8ebeccdfc409182fcec0e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableAssert 4e0723a72380ebbbad27a97ce488104a87f91ccb854586371418995ae14414f3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableDebugLog ecb7962dc19cee63ea4bae67d478bc94ea050946d251d12d49b8f81fc57671c4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableTooltip 6aae8ff0fd2402f4f9a26bce87a7027aa34200dc269812535894ed0b8b727c52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigFlags b74f30d10a484a35cf42e1fe6dfc7c5031af7f7cd6a40ea8bef6ee770ba4161a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTextCursorBlink 65a056355ad88dee02fe123b8ab62e6b01f20af489e3b293e3e28620c0d0afe8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTextEnterKeepActive e06ee624e9de4d6d6da32fc2f0b7bff6c475b107b6490454b15e6c28e25454f4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTrickleEventQueue 86222ddf017cbe24aa338b38ab946b0f39111794f182995436a18be5bfb8f5d3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigMacOsxBehaviors 23d047459695752fca610cb3168e809fa25b1afd22f84e60e33d3e42e01aebb8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigMemoryCompactTimer 43ae8dc46d5a347c9dec9fb70135a692cab1083be4f18246775e543d9c23fd5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCaptureKeyboard ed63fced13bbd847d09f915f33a45cbc68fc199c85abd97c1a01a1b9ac8c1044 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCursorVisibleAlways 3097e787ec9593a1fa71cd6de25813aef4632727916b2c5c44762a72aad57e57 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCursorVisibleAuto 771e228fe4cb7e217f5e0b1cd10b0c53150a9c9245f88e01d0a6ff79b3d25223 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavEscapeClearFocusItem 5597eb53870d6aa5583f2cb50fce8cf549bf4acc1d7753f35932ec7af510c531 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavEscapeClearFocusWindow d5f79b1b498f8f6118b5e10d0c57c69c8693b116062bbffa08f083e28e10f238 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavMoveSetMousePos a62e3939dbed91b3510f60532c7dccd6ce81672541d9dc59f47112dd3fe4e858 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigNavSwapGamepadButtons de7d00e3ae5f8313a09ac55f573ad9a8e6a6f537d05a36d32e04980705dc9d04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigScrollbarScrollByPage eeb81f52690605b329baf18d703d7e18935c4350ace1cbaf6f2ebc98dab35147 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoAutoMerge 23b55ebe640356873ed89b9ae13472c32a9652e26d5bea84d53522655935fdf5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoDecoration 1fbb7205f1366ae2d913c7ef6e4e75bd5477a0b7c86c19ca9e456154773f8e82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoDefaultParent 7446e06a58bce70eb0be7c845878895cd8a859867b837c03616be34d83b70e78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoTaskBarIcon c65f0d648117b49bd2c1e4fa7550cae4c1872167718b2d57acd43fe5ec05a821 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus 68a0a901b1b0f1b7fbcb3619d54ab3560fd09e88df6fbded04f51cbed8618d2b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC 1269cfc5b07ee36235446a01426dbc342c5deacf406d2ac31ba9fe9f895876f1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly fd0280dd94418e347987f955e5b41bedfd6764946f668734779cb530a5dfce74 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsResizeFromEdges 6fcc9ac13fbe4faa7292a9e6acc437cc187b118474e218400208f014dedffa46 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.Ctx cc0a7ba2923d35de9375320709ba868a2e35beb030e0501a80d1678343489e61 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.DeltaTime 38611b69c184186e8556460bb1a219f9605b018e070ab6ee6b17613d78958861 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.DisplayFramebufferScale c0c7786580ae3967f92efad2c863b901cc033ad62f045e195cb3654ca79245c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.DisplaySize 73c89ea7cd71a42711700bbc7a25204458e7a49b4a7b3341229c8b707df0f964 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.FontAllowUserScaling 88cc42e93a50697dc1f4fb8ba186c093af8ec24e0e509017da2561d6bfa94ee3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.FontDefault 7b7776eb7855349fb8aea62613e27d2e32e081ea799c1d8f3e0073c8000c6018 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.Fonts aed6337c5720ed91f925d00db71a112182798e9adcf1f15ca941c8b4c46d32a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.Framerate bf04551fc6ffe3fad8df580bb0a59a057407392f901f1dc17712d2a31463917c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.IniFilename ab15821d29fd90a90073cb3cec582b0cfa765fc53422afded91cce00571bd262 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.IniSavingRate 2857824ec58ea0eab7993bd0f5e4b24c0dc68ff6573337d97a25267c12755726 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.InputQueueCharacters 1d47816802040b0027b5b8782349b992c0ce45c2dab2418c01d4980e15fe509b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.InputQueueSurrogate ec62af6c025cf0d249bbf0a8d87e731b99fdf32889a292aee36b5281f9fb67eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyAlt 10b3a1b2c8c26e4fcfba9e23c1267d0303cf10754f4b582e07729383d01efb2f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyCtrl 76b67c953a8134db8ec6367712cb6923498f286fe6a3e49402421de99f20f1bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyMods 688b5b26cf3c86da425da9b6d694d9bc1f518714016c1bc77b7204ffe8cbac12 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyRepeatDelay c26f69539dca88e6108d9fb4bf3f2fa972d8c1fb9823b0a5898c4d3e6858744b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyRepeatRate ac36ee534d4e13b733a2c9142a7fed5c9b7c71abb966f09e3aba8cd93c459940 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeyShift 97c3c10158b0c2b86806bbfbefb5e38e7001a0d380b5be8295cd45ad35b28e60 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeySuper beb5047aa56ee705e0b19c7ccdf04c97316b57c3c9ebcd378637d56b2cd3c88d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.KeysData 8d50f081c8def6b3fab02b913d2aeef142636b1a5d67398b130e8cfdbf2fe500 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.LogFilename ebfaf2457ac6454e6928f2608e5cb1fb46fe5d7036783aa48d57786488cfd61e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MetricsActiveWindows 73b3f862d1e86dfad04eb0afaac6530f66f0b57b4d16229b23874eb168779783 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderIndices 5c722a81a7d817906fa3bf04954a6232d5aee7b2386a267ad5b5e90ab373706c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderVertices fda64112aa09b64755ccf6b32208942e647a4b9d3fe368dc215f63695c2d950c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderWindows 16885bef6d11811a42d6c5a42d4ebaec639eee7bec493eae541ad7d541295c8c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseClicked 5088689eee11c78ca0647e3d7bcc62df805c57cfcf517cc82d2f2f44856c581b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseClickedCount 53e1e29f55a7db5a06cabe1810257ac183073cc78af44be7bddbba6d6cac5e6f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseClickedLastCount 55fa7c62b225993f00fcc645adf800df5f746f210d7f1a0a0d6eab1026113650 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseClickedPos 2615d86eb54141363e233255f1800cacd863e745f0520a9b8979a31e68d52b32 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseClickedTime f3d7675b84d6147541ee1025825d7497541cfbffc57525c4973f21c29f341411 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseCtrlLeftAsRightClick 60610a7b0b6e955ea47b0afec815455a3eba19fc12d092a1c5ca9fd5e69965d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDelta d7b9a0c3c7b660291993b4bd26d68e010a8e2c64e35100a9dcf66c2e86d303ef -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClickMaxDist bd595c883e8d2bfa6ec3f266127cfd5248bbe561e2621435f54c49d29769f1c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClickTime 3c82f95bfd858d7b964aac692c0e839da52f3567fc7915b3301216d243e2f40e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClicked 58a75b47b7dcb8fa698db49e6f23a1f071866ff1b8896eda2f09f29f7d07f685 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDown 2e92b59b4ec4ae358c7cf31191feb6b5f410172d9086b3dee38862e22a90ca33 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDownDuration 13588ae76775beea527664fa07ed04523fee743ba464610b8795e96354c5379f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDownDurationPrev 225d08d1e561dc0ebb2e386ec42eef8f809164e7fc8745c1cabfbc7a4921bfdc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDownOwned 1da804de31615c244f65cb6bae4eec1f64a995acef7617ad95b680ecf44f8902 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDownOwnedUnlessPopupClose 0004bf6d29659c5c23113773dd0248d06917d00e166397028447f857d8f575d2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDragMaxDistanceAbs 5494b16b0b774e43e4c0c5fb88b79b61bf17b195cb4adfbd3f05a3f466e27c8c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDragMaxDistanceSqr a70c3061f57ebb2734dda06cb7bb43d066cf98b32fbffbe2f99d20a138efaebe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDragThreshold 3f682f08fb1941a503e9f93b70da4f29dd61c8b051ba710b045171657ff23323 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseDrawCursor 9415422c58ae201c09923f4731d549a6157f74841a702ef3af407f0bb5712e9b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseHoveredViewport 93e83d2d5d8029bc72e6707870e4eee9ff713d25b64e0dde3c4e6a74a7316bc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MousePos 3d5a94ca2e1e9cf23b2d34c4f082b5684e16d48bfa6f09badf08c06c8ad152b0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MousePosPrev b37ce620bcc8ca09afa11a49bae98a2d37b10a8d5b6612f3af6592b464ab49a1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseReleased 86b7e62670c4883a36a63804577502da4459a0bca90a73ace1ec09ea2ce568a0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseReleasedTime a1a8905fb7af55856a08e3bc6372e28a0e3b4b4c0fba85da09be43166f2f665d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseSource b978867ce2a49b9e4fb74edd1dfe341be3e55eea34d1eee5146004cf9df7ef38 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseWheel 7ad6ba12c6bf533f27fc660eb8e1ec47e19b97ed5aedb592d6567dd4011147f8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseWheelH 4274b01fe1def911d76730c9212b85c92c16b05e0b005fbda20bdb788e5d96a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.MouseWheelRequestAxisSwap 1c70df4588431264ad3bb9d453034c2f594836f45780f660308c616dfba1574d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.NavActive 83a6728cc37afc7484d16fe3234b0855f4fd75b589d88e4004cd385fac3ad191 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.NavVisible bba9ca1542f0abd5c414f649443a3c70e5d01eb816ca976988c714639638dc4d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.PenPressure cf8dbd7e2294eab0673b8ac037feaf7b4df9b44d64791a7740e138365d4c2833 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.UserData a09e1a5641f9cff1538738aa29ecb2e39840e9a2b53ba134d2e37a3c0c4aa6e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantCaptureKeyboard 4bf5c0a49063d2ffd632e6860d2fdad6809219f6cc4fd0148e1319f5a60c1b61 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantCaptureMouse 44fca564c77b43c39eae87808ca941b4ebfabee82790557d5b4a1a67b04f41ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantCaptureMouseUnlessPopupClose c316c313af8122f203461e8c3d6c92b94094de854575444788aaebcd654b1902 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantSaveIniSettings f6f9a4795a5d07eea730e56491c00547aa01b6d1e7408dc0dea8af7f91ed3d94 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantSetMousePos d6f2e9ca8b49e7f0fea4412d025fd7caf54b92ee8b1fcc50bb99c257e3909f5e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiIO.WantTextInput b82a4fe6aa19893ea1515260223f1054fdb867a3d42de9fb7a777b79a634d69c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEvent.AddedByTestEngine 810de051b0f00d262b7873ee517c05b76397fa4787cd0c02ec944c38cafcba82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEvent.AnonymousField3 207aa353b222032cc8d64cd01906396338fccbce0d80593f5da75b4747437a8f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEvent.EventId bc679a2f9eef1c706275958c24d969663d1ff65b1fddc45aa97f3ea19eef1c68 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEvent.Source 16a84978504a249ad9fb0f0bb994d72c6ee44c54c51b633c230d8267944ccfa1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEvent.Type 5f1a0ac99e3202e00f69d72862ccebdc6033c341544a367a8b92bb9cbe2be023 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.AppFocused 06748f9b65b2a36fd6db6e2f836dc645e1f3b472746261fb1baae7bd46c38a83 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.Key 1d059bda59950c8c7891ddc273ddb7308cbdb5c4e3ee5c11247723f27ed49879 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseButton d7d18fdee4ca1f4cf9071f9e7fc4e520732e27f444682c634d6cd89328209b4e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MousePos 36a3df37e9aa49ef52cf493eed3b5af505c356f6831aad4461c9c63e3e0574bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseViewport 9563878306e47c1a835345d13685decd9fe39ca60d5d344102576099da0a19fd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseWheel 89315ddb8c253e19fa82bd19bee90eaebdeb5e210bd214a7e43e86f24f3bac02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.Text 9fa9c73be4e881d4392a62c054e78660216826eae6d4bdeb888f9ace994de647 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventAppFocused.Focused 6fa98df36eed8b8a6064f3cf23a5aac5efee798c9420caa94d0c9b46382725f0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventKey.AnalogValue 33c86d3ec5101ab62118a493e6ae043a4b9b8f6a95d54378f9312a3795d4aed3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventKey.Down d0da0d9fde8701b20ff1f949db92778cf10be3e8e712f72c501ca0d89cfe8f65 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventKey.Key 61c792559bc21a22ea22a8d5a6f0308cf8c27c9a71c7663dca4c89df4e0f65b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.Button 900a03970e8d9df16c2009090c1ad8aa1cd9916a09977579a088a8bd6110097d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.Down 46e1336afec0ad34fee1c901671658ab949945f879082179f61b5f98a1556a59 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.MouseSource e998579b5d7253612c0fac421558f0a3a41dfca59eea0f153eac3f3a49853849 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMousePos.MouseSource 8a061081171cd4df79aae3dd7ec0df2445f07dfb79316d1709e1804b0c622879 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMousePos.PosX 6940042dad6afdfeff65a713e0eddf107fde608566d4ddfda73ee4a8625cbb4a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMousePos.PosY b916a987b5037776a370a25c71080f3fd853dd0f5c6db8fe6108222a580b636b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseViewport.HoveredViewportId ef92f7a00749589836a5b7e23a5064baedf515eb620ee4a16a1cedf5c0e2f2cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.MouseSource 1794b7e09703e5afa4e0e6a6ce7c04e463f2c3eab78ae4808d6f530fd666441f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.WheelX 5c2f39225049668e2a267bd7c9a9941a1864e0646e93fef4a26ddffa5503ae07 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.WheelY f1f948ccc930939084b180dd2b2a50bf0fb8d68f9a205f21ebff407c6fc17c7b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventText.Char be6e39bdefa2e23612765eb960cdf40ba562c75c14d9233f537c409cb81d193b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeCount 31e1b9d3057cc0609a1be1cdc9e449f7f396d7bf0f4c8b0552b5a1e292565620 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeFocus c8669454b31661b5268051aa4d3d5e386c47c9a7fc86654cc852334b52b3b082 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeKey 7f85b2ae761ea120052eb4d44123282c562ee97952274e1714a685e0cc4b6f37 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseButton 0f3a9a961325b8939ca0cecab30c2bc4d748695780290515a7027956b6ba5f4b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMousePos 4e446a6c8efeb5a1d2dc342ecb6614215db383fb048256e41b2fef2543b652c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseViewport 39f6676907607770c420dc2c975e2a8f6dde2123a28e147f167dfada1814be7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseWheel 4cdf4702e057d0793104959719c9a77df8edc76fe02c74baa18e45cd4d242d9f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeNone 6641b43b8950767d76601608e1203463b08e174d4f11526ccc4b3460e0be8eb1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeText a488c34c3e1ae22681f059da25810231fbce68f7fe4388414ff58f96bdac7936 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceCount 0f8723ffbd31478fcdfc50459ceb39ed9a891f02539659812bddce492dcca79b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceGamepad 99b899af51a9ba535df2a9ca01a0a309a5a1bfd52629e2d38e056e4e79ba0866 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceKeyboard e11871efd078a9763f8308a7da21feb843a83ca8768c90177c2ed535c4dc32c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceMouse 431db6bfcb5d9677e66a6bb0c43b45b62fcb971280dc00ff12d88bd31793a05f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceNone 7e589a9b1621b9e16a07056084a3f9147a378efe1e6dafed742292e63cf536fb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState.Id aa1ff166164bb4fd8a9c5b1c0691beb60cfb0dcd939faf88705f58b583c76109 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState.TextA a1ff530767785438f54d2ae42904f8b11dfdbc46f38a639ac6ba833adac8fd7a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.BufCapacity db5dd3fe671ab2c815093591f21b24a9a970fd49e7b36dc534f90556b3511be8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.CallbackTextBackup 7bdc6b0c8096283c4f4580b1e79bcb4aa42327e21dd140a8eac8b9463f78f513 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Ctx a41c9f24d1f5170c17618d20303f665ccda06eeee0bf797b9fa32bc116b6ec89 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.CursorAnim 2b4844a95133a6f76ad9c4419ebfd4189778fe66deb9b52a3dbed5b048872343 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.CursorCenterY 1a72644d17a3e41601ccb53165a30da7781f904f434caab965c91e495b113ad4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.CursorFollow da6ab55d7859739cebd667921d4a6c96309c4c9aaf2dab5e8d6d067017b84583 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Edited 95ff996c5a7586f62d9090cb0fd0a3233fbe5c4c5d28909a61589a7a60922ce2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Flags 09e01d2869caa0f6294de36a76dadd52c77e5e93cbc1d0b71ee5416095d8d083 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Id b8daa899001e15310e69683abca99e9004aeefc8c6fbef9588350a48fe6391f0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.LastMoveDirectionLr 8f7200b312a1b4b2f126368318e1d28a2768977b77cbbe634ba8556f65d16385 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.LineCount 1608939c6aac0b8bb8b25f7f06d5ac2ecb2c2729cc4a02d0fa7e704a3f9b89f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.ReloadSelectionEnd 6340b34f41882178223892f533e781156ef66eb2e65d6e6a6e8f29767ac0dfe1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.ReloadSelectionStart c3636e2b9d08c59415b90f5d68e43ed6fb904226f67bfb15f599750f54328cc0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Scroll c0ab57aa6e82d3686ab3256882d536c507a402260f6bce80c5a4811db5e032f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.SelectedAllMouseLock 23fc4f5b35bfb1fa02b1b158eeaa889554eea229e44e798d3672acf3e5006257 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.Stb 647e978eba53398297f2d1799a95e1225d1e0bfd6172a5231bd9638e369742b9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.TextA c50c8488dfb1284466231420fa9f703e820e2e4f2d303094613bf56af52bc80d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.TextLen b8623b617561a584f1d5f1e9f452df20878d4d21ea1b55374c4e3273391dcee6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.TextSrc a747d6c2ab595e8ab9ceb73e93af0eae6293036c64cb30469b6c261851ab95c7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.TextToRevertTo 99ade1c22b52f7516e3590cd606871b94cd4a1e29eb1d7a9998a0b5d48498f21 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.WantReloadUserBuf e8377512dcfb49e6c216f4a30b09414562056759e064ad03c207c1e8467e7874 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiInputTextState.WrapWidth 9bbe704fd45c0a4dc809c8621752cd20d28ae7b9df68473c647679fd97b348f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey0 2642da29449d55f7772123832a5ccae7be10301ea0c7c9ae7c4d79705d9de8e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey1 2676d329e3d4240edf89798afccb652dbc5c37c084874d97af166356e5f97ee9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey2 def22781451256cc730d02d9bcc32fde7abc9220a81ba4b339299d986fd839d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey3 f9223335726b800e2759c79f45bf96ee10d1773655c49b5f7c361e3ab08d19e8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey4 84225d3b900f083ced78568def66651604f7ce7d044863531aa7d8c3aee44e53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey5 ac4372e9b9bb434d5cc89d3b8774fc6817db26c39bdfe6110e3142847b42993c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey6 5d1db8e2007b7bba4d7a032005147dba953fc61c2f623439332dd046523898dd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey7 3da5969009939a14a414674f338a13aec582d3fb0faafc8735e362bb0a2c7089 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey8 6227b66a92141f5a445383b582edc9a45679996bd7f76fb1b80fe5320d294e1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey9 8abd938d2e6237076cd88bd6e1efdd1113427af0635bba70fb786e76ad4f5745 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyA f94e30acab009011971d2c6331389ac7eecc997747783fb89c8946e30f793159 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyApostrophe e27ff6331cbc6eb866a5536bdd28542636a321b133da677cecbaac0a5138223d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyAppBack 672587e2c2f6ea0ca00fc8b65173f1aaf65dd8014ce65f94f9f211b75a9a8e7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyAppForward e6f46df7bbaa96133e5368d50ce69ea1ed847f5b404c3f20fc73beb044f3031a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyB 976721af0a8101838eb8ffed3819ebc1fe7c58512696d526fd4a54de05ef36df -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyBackslash 015c7fac3acdadc09dfa2bd5a88f0a66b71c48bd609d8cb5dd96fcce4a0f6e76 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyBackspace 831b80936de63c0feecb5fb37697aba965d75cd353e1f232f852a8d7ea77bb5e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyC 8bc121ad057ed85c7a4c0de6dd8c8876edea84dbf5ebbdf972ea0d6e4cd983c1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyCapsLock c525a7d0877b69fff696189520eadd855fc356705aa87b88c1266a22c349229d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyComma ae9cffb0c92a9808c91c18f002c9fdfae54ce53cfdee76286d2e4f27e89bbf8f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyD f9fece230e4a7888e80198c21c3c205ed8bd4e45dea6944a037a5aae1b33e35c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyDelete 22d0c7104d4ac11929f564ceae202251009afe61af2ec546a9ce1614a9d906cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyDownArrow 3a12351eaca4019d20dd9f691ab0c55c4e30eb639f11e990ce2f5f77e9b7a453 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyE eeb88c1d360dc9c29f8007ed63670c8b4e247dcd5b2aa92dcefc667d8bb26071 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEnd 66e3fe535d446a530e83c40d0a1542f9f26232d1560f964272935e7eb874892a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEnter 0795268041ab065e67e354e5e7b80f5d47fd4daf528b043422cf5e56090b1838 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEqual f48bf88665eb1a559613019a9a2d1d7f18ec54375a5e5c088f30acb3d3c788d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEscape bb27e9e82459a0270b6ce2ed321d3d4d656b0f5663954728de577ff08b392e5e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF c24205ef2afbfa4d40a030ba914c59724de8e49cc8591fa0471fff4531ade34d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF1 a8299f5845e681f8883b14371a362bb2c120f80e6087da0ee8c894bde304f4d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF10 65387f6cdaa952f196c98343d8cdd4565ad1258e9e511bc1e31b3e64187a9e24 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF11 1c1e9f317ccb80799e92f2439243c798e97211a9f0d78a8efe99bf29e1b491a1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF12 85c25b530303cb3d1f16485e189f31b95cc987c9ec43103098a722d33df2bf2f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF13 ddf87856be9033a6ad7e4c2ced38888cc8f7754b6ef2ba6da5d06d9680c4ed27 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF14 2ce68ad61c0e114c274f0cfcfe382472e82f423554b5231bdc83e8d6e00df9d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF15 6d21d855cbf17b245ed4b142fb4413a47ad7835422223ab179d0f095a618407d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF16 6104128b2fda0cb635a4c2aad3be42992194865df00d4dded1d9d34fb5100939 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF17 3f75d9a9a7ede2e4827ad2573e06a64f407a184bcf83c96dde4cab1ba58ce309 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF18 56893e8aede9aaf32da94c0e51c8b8a4413ce276ef84b5e11b4c8c9cd48f4967 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF19 3f288a228117dbb9d4ba38765adad07f52090537dece804ed2ab49361228dbb6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF2 7f55f10d23f86fbef4ec027b3cf2af346f7cc342730c1cc408273a16b72b26ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF20 e725b9b4d0f8dec35ddc0311a44b55a990c3d70e265e0b315c3dd20c480f3ace -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF21 3d1dbe1a96e73d253fd2254b6d0caebea1826c53ce56b3facf19db1d25838e50 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF22 2b3825f19cdafcadbcc346fc73e54a7a4cc930e9b8d38df6931970c0106ab6ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF23 4a13eed61765b7c8aae8972ea6197fe99789d4ddf15154d14a84dc1f8044687d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF24 3bd3c81a6e0d63a17caed5c57379d00ca59db5d074ea1c61cfbcdd6282b799ee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF3 2990b1e2a67aa848ea5460d9f0f4cdbe7b4ef1706f88519e0bfdfd9f980d957a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF4 38dc0498ded71a159d25eb9d6668a57824e4d4e76644d4933b09a62f5dddda2d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF5 954bab537d5fc0aaec4e2e63d2820c0f1aa6b1bcff1c8a70bd0542678d6f5890 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF6 4f3c78ab007b8328575ad2b10f08478f627b186391228bdb6120be0b83659b3b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF7 3e6263f96bc59bb25e2fa296182145b321df8b3f177848fe31174117c445bebc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF8 3700853c95b05daa55b6323e94a4264cd79d1de7b44a3ee963c4fd4f28d9411c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF9 8108e23e5814435dea7e89bc0b2092991bd31e748f4c5942edbc1702c1ca7029 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyG 197ae2a2fc790209053860ba0dafd480f4a8ac0f50b28aa42a1e3f26ad6c993a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadBack 2a22c106956b91b0025928f6e5b3273dd1fa0dc70a538269d1984b7c5a071515 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadDown 49c3475aa449c62f87da85af5ebdc6e6dbca9a8d3f5311742c5365b59d25fc48 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadLeft 086a754ec6a422e5bc334ac97494f878a959b024d2c97822b892856acfd16edc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadRight ad5448dedbaeb36fb78fe25dac178a3ca537ea9004fd34bef776b880eaf4b4f1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadUp 21d51d56f1d768edbbb0ce0a3a6c4e112011cebd966d99fad1c9ba5cac5a9c62 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceDown 0147c77221f73e163e5572e26b17a76a705043d54e68460b80d5713cc462da2a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceLeft 73b721379361a5d8bd118f8c03febf1236dc06afa156db2b1c4c7fc4e81a3711 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceRight 3f6d19706adb4fcfbffa66f5375bb5639186fdcb973537f6e05f75ec06617709 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceUp 9d9ef32a0b25450cd1c026bb2805c943861e1933ac07ff42219786e49e5a2bd2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL1 38892c0c11f09a3de0554cbaddd07520420deb87ca1603575da432b1c399fa8a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL2 f244cc13a30e7e622bf64a56e8be8e429602b8b159cbbfa23e23f6d21049f2d0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL3 2494f8c686ad1031633d131d2c579ff581c94c3291bb922ac676e5d2b42012ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickDown 73b675c35c17745ecca341816d2ae88a71d42297547b85cc37a86585cd9690b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickLeft d08d142cc6670cec7e8412b4a1c80789820d161da1caf78fb48a3303bdb10c9c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickRight 2bbe7815f36f635c730f88061b1036ab1200878e91a925f9b5e01c6e3f22ac31 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickUp 73659aebda06eb256ef0584c97c29e3894dd752651128eb4c14d766d4aa040cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR1 c5935af6e2d79e4a11ac14b1eaa57f25891ad3cee76e8a7bb65832403a33aab0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR2 3c1ec14ada75af5df2e9853b21b96e3ef7ba642b0c3910e15a68c2d4ddccaf3e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR3 2454437956e3f86d631f7019b55ba5e7b6338373b92b4e28810607041135323d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickDown 1e931b6d8b6b67fef881ab2c6a77ed77eaa90ea3536c70da8b3eb2dabff83943 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickLeft b57f3e70aba6c61489e6bb934f5212dfcb69ea4afbdcb9719aef36bf29154b34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickRight ce60fd60c7a99a6cf96f93d79219997ab0c55a848c7466976c153d3c4feab3cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickUp 514a0400bb6f1d06e09c4f1133aff3c149ab73d516d78e0a282eaa1f8d54cc3e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadStart 623c95dd074a9d1138e4209f5185f183ea5c939856f845171ebbd4a8e9ab0390 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGraveAccent af5cf98a666ff7a1007156dfd80d41e5171ae507b7e5d235b4b7ab916a716cf5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyH 0787665f2789e1742d3a79204729d9735badaff45137bdfa64cf377e453751be -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyHome fee867e04b4d626d9ae822e53e1999f4f379d5d9bdaf13bef525103add22daab -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyI 308def949896d01f102b03eee8d16f5439d9d92b0a1a8c154cf2bba063d2f4a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyInsert 90b743822cfe7b66763a6980c8219672cb10710cd4a06b2dbe47f7961f0df5db -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyJ c274743fc125c2ceae7b06f4334002aee7391563f679f02a99f2bf154172b696 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyK 74ba4dcde2d7051bf4836d989cf4fa8f41efc0c79c48183805f2ad656adbf9e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad0 036d94e1975af724cb9bcf8dbeb8a83e5e3786019c22c8f5615e134887708c64 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad1 e08ac9907334fe166795a8dcef2a0c19d2b2a08b7bba6ecb19e7b7603b39ac89 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad2 7bc9e4cb631015e5ecb49797d88f61bda91b9b6a614f7dd1d526ec102bd4933d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad3 1db66681770cd8c1862549bc6c7cbd941ae7b57fded3f5e705930ed539f78924 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad4 c6a727be9f3927d80a81ac4c4b7e2d5d90e4923f3449965092ba40da2f3bdcfc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad5 c973b38a07ffb226e7452eccd0e522a9bfe15c260830aafd1914d26a198cdaeb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad6 f0729f3a63a86258762198c965bac957968e8e1e8704b618145830be0078d855 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad7 93f0935298ac017bf3a2e91ead50a0efd48c459e0c46ff97284d488725c40b45 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad8 35735f7af3e747c654d2211ae9e967ee7167e3f67282805cc439d22cf2303572 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad9 1bb9eda798e91a9e36599b8a64bf4666c59eadcfee6a3c5de3b0b5a897cf5b7a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadAdd 6c3d3f81190ac26a1d8c8d0c395eb8f8fd20a721d4e9b114d16c3d0133797f4c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadDecimal 51c15e4cbf77f187019d48a016bd5287f72ed6f74f591f764eee854559de9323 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadDivide dc001ab6fa9f55f4f61c626fa673bce6f2b233cbb81098e71a5a1a8607667523 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadEnter ba83b9209285bec3913ec107267839f4c68f23b2985cfd6a4b9d88e3e71f5510 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadEqual 359d560ee1984e92a7b61e28028b48fc7824c114ace1549a99847f2d8f868f85 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadMultiply eb426ac9aa3affb9a0950142a3e5f81af2dc7d3d0a8345aa669edec6638ca19c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadSubtract 89a77aaba0dc7cf292160cd7259b4dca17a135936270b92149b051757a8156c7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyL 5b3f76b0816193df145ccac5d485136b1de4c1c3cd131c8b49073bfb77aff768 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftAlt 10879e340b722d86ff36bd0cdbfe0490821abf4d8adb702dc856869fe2e66ff1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftArrow 69642983d7014d8b1412eb32540b6e4767d6f403e86c95a9873b9be9b777d860 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftBracket 37fc88196223e50e63f3ac8fd36023f12f01a81f52f39cd712e3124567cec38b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftCtrl 5c48efe2b811125ca726ff2d0c0f5d58dcbfa3bd6dbdb3e5f5604d8faecdcfba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftShift 1e77d26015d21bbdebe9f13899f39367095fea287037a971958027d5686b1afb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftSuper c8f31714c995a701763dde331ee155f163ed3c2f1ce20b330cbc6e78a2f5c6f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyM 80c314af89b9d1b8ec4e9053deffe98ec6859f7f77f7cce10cbc1d0f268961b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMenu e611013d1b2a4bd0813b13c361219e0ca45d97d100b88bf261a6b9630ce16b22 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMinus fc18a8dcbadd5a276e0e29088d768007734ab4ebccc4d631fbb3b7deac4cca56 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseLeft a769d94556584461bee5eda27e0d2e6957eb984e29ed0d356efb8a2f588844d3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseMiddle d7256cb2fad4cca0732e28b675d8b43424b6570730b00035f39050cb8367a21d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseRight 8aeea170888cb4a9dae695846e6e88d1f05867867b763b219c0e9885beaf594d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseWheelX 8aef438f90fdb79465503b6b0608497aef355dfe18bcb7f8b7b4eced16b4ec0c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseWheelY 329db2bb1f77a3db66772e45f1254d5c5b862b59901a576fb63e3752f4a9bf6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseX1 62ec0bcc9bec0f00310c283b9aa89c2d712edcb428d724512e9bb299f683f327 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseX2 becbc4411e9f142e9d30b9944cd660a1d9ef8015e70cf312ffb588bb73b746f4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyN 494be6ff25ed601f5dfea30ae9af91b041b8f8d8292aa218d8dbe4dc5e309a86 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyBegin 5bfa100e3aa346544450805780d97095b94e33afb2ae984a8f3202e72de13711 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyCount e287dbae6bd6fcbcb2caa82ed7bf4b70f4c8b714f68a4308c0d5d2a8979a8c76 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyEnd fb454ffeeddc5df75373835ffc0c0df3ebb558e48eab2c5c1499498d64845edb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNone 08a19e1efde64d11d2e00607a59a5dfa378666d2f9b2332db54fb133c4b71926 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNumLock 141eb4b3627403ca393a777225e9d267c37042d6d0f10055eca990cb427f4901 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyO 285c4cb9ffa62682776e9f769001ff0eb0bc7d91e4fd638ae71838bc53d580e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyOem102 6401f9c30538c6c35810da9f924019f8a41b6a977bf6abe91d5de6c8bc386ac7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyP 297da4bae28785cc065ae49a0f53e6079b65c039413d77cc75cb253f87c7b3e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPageDown 834dcbfa3b238a4aa86b955b9e4f6cf580ed8890faa5b92d11eaba4a35edca16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPageUp 09a174efee94d1f09df457633e7cfcb7fc04abbe5dc30b03289767776833cf13 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPause 9341e3bddc4e6cc08da37ac41ae5f9c9ccb16fa6ba83b89cf3be7c9272375239 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPeriod d1137825bc0a2d437905b0a4e3d78dc0ea16865278a291c69b0a1ed8084c93ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPrintScreen 416d5929c555927140b21a832f978467d4ac92cfb5f8e83c6f926d10c7c336e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyQ 98a84d23046b412c672bd4231b24a59087c3ea435079a347d6dd181d5bb14734 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyR 9774fbbe11a02469f162d7a6e647c18bb8e68a06ed96548a2820a228f60d51d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModAlt 5f9e0226960e392f5076ee60ca4f7c05d93c619b3350142239023612e9a12b2d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModCtrl efc3af2a33cb243843b073c97ff71be01d8879aaa56e10aab54ac89b8763b59c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModShift 294ce6aaa78aee27a72071cf9ff5c09587a06eaf2235df9689534b531e1e29ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModSuper 4d7f94b71165b1d40690c7fa89ac145b8dbe67f2e4c6eefb4021162e5d59caac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightAlt 21ad52f70f01b65b3361ad68f7bcde7521bc875964fe5bd763edb1303ccb2646 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightArrow 64bca1d8fba4dba22cfbda50b46237554a26bba2478565af062f9e5fb290f9ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightBracket 2b9eb1d538182df1717fe1b1006043546f2d5aa4da81fbaa13abd95740c58e68 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightCtrl 58217c1095f7ba09a4f1abbc29d33318b941933f8a297aff986582d6560668ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightShift 7ef06990e628f35d238f8e08518ce7d2885e3af7fde57637d5396360e787b89a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightSuper 1c7d4e92e8e7aa82ab6866c60f7e03432902b93b2adb8000f27aa7e3d6e908af -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyS 1bfa970b105152a8d2f7d7e5a0afd224df5a82b2f86e9f23e855f2117fad8f78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyScrollLock d881f0a6064411ac14e7f2b855ac7e70c89e4c0bbe79a7e6218f2ace1fa9bdfa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySemicolon 12663dbaf8c9eee473b5a5bc158e25c9a2ad90710b7835d8dbab414a3f333038 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySlash 523104fec168a3f999818c97a73c176d725ed8a974c724d5fc806a7c95fbdcdb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySpace 766c1af5235619addf0cb54ea2f52a51291c27802aa8bc8e9005ac8a0701c46d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyT 9bdf35b7c3a1da5fcb9fec5374341003b551fc07fce7e10d62fb14169b4a4cd3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyTab 31c0e71d37ce71e6ef54d9d3eb934bb63495a16a2f2ad6c55f855bda6b58a5fb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyU ead056f2df7bb32c40d148955b2e6067c1038184e3994e6b32f41b7b48764781 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyUpArrow 600f95069b87a72d5b3dba9cfd987d07ac6fb9e8b56cec0ada861698767d0f37 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyV 808fa18f69728bb833ac9440236a4a4829e3e7d8cd51e62abb6888bc481ff9cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyW 467e47f7171a451a82df487286214a5009df86cad9a0aa8f61133622430be847 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyX 27264420e282988765a5f509734ef4624da319c150cfab81f2874994b42d6b9e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyY 8a9abbd645d5f58c9fdf8da7c98955376466ab1f132e58e642ea926e8ebd71a8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyZ 1e5f6be230136f4741d39bccf78dc030c754487aa9a7815d9b1538d1de62f99f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModAlt 83b59e824892302b08a58df56003f2a1c8fa457efe2210141d9c4f4e85a9e0a1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModCtrl 57589d58b5f20b860a37ca23938ecd8718ea28b069ed3723758a30b23bd75733 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModMask 790fc2095498d53516465c3c9b40f096ab04849884132dbcdecc900a588b3330 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModNone 2730051a3487b14ccd9d524671aca03008eb012e16b82d481e940ef20dd0aea9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModShift bf5f74b91605221053db8f7966a5527834d5e1ceeaef5a4b5656450bfbb7545d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKey.ImGuiModSuper 935b666323425b67136d2b241b58e332aaeaaeb9dacaec025873f0891f4833cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyData.AnalogValue 9721700c6026732eff580a683a30ac622bae24a72c0922c81ec0b8d1cc56cde5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyData.Down 4dc9fae5881a4e1b791d65f52e1cfff1bb81eb52806b160bbcdd1ba00cab4ece -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyData.DownDuration 665909264f8cedcade647a86945ca4976d8bdad96f1617e844c14bcdcb3fd171 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyData.DownDurationPrev c7ff32b9c218f566c4cec28f3c70cf1b13928da3d275f97732eb9ddd1577c504 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyOwnerData.LockThisFrame 665bb1c94ffd99f0a57c3447f56899bc46b7577d9840afe4584c8ecd9b4794e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyOwnerData.LockUntilRelease 0e42c271be9b6d9d42154315fbc85470f0c43aa99f6559bec6b705e41513424d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyOwnerData.OwnerCurr 68084281225d56c7d5aabd04701960b547ea00a5e643eba2b20168ec457cd81f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyOwnerData.OwnerNext 2845f63f458f665b72dc8fcb911c494af82ece3e898fa49ac00f0af1210a77c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.Mods 9c232b235db214b2fba55a10aefd9bfac73f87376b27e67e93fb538efe46d0e1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.NextEntryIndex be279d33622ccb21b10cf9a4e5c0c6ac954255691865a12c1a889b90360ba437 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingCurr fb1996b3d7fde9f4f5a1a0d27724aa537418af7c5c20bd03ad5ec9eda33af088 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingCurrScore c7bfd2fa03bbe1f4066fc07c5e53e4685a27fc061e2a4a715495831c111a5319 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingNext 06a1f73330d13daba6c890f17fd543776e6260dc6b685f9a577deb70c2afd686 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingNextScore 6ceef4782ab602a063d7b8d584a77466c6df9b7a30a1d3f0e1b90d7c82573278 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.Entries 529daf28d1ec7b3cb60b48bfb812d8281c53ea2c70cb1325e88c6730681406ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.EntriesNext ee2cd0d93102d69c9073af80826ec2df43a5dab570f6349ee104a5bd19da4e25 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.Index aee302dffae0bc4066ae340070a729a99228a65c600bf089bf881a08c4e89e5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.ClipRect 938cd3018f96c7795d4519b5ee758f9cbb742e17760c189f665fd7f659886ec5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.DisplayRect 94d1e642a298f76ee1c6122087c2903d99fbdfca3b0c936b3ffe19350af2fb82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.Id 26831c3f28a7d83b5c4853814e159e6aa4e26cd2416f3cb5e33ebf53b136dda6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.ItemFlags eff126c18f673112e34e29bc70e881541c3d5b2768020dfd7d5058b2d99c06f7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.NavRect b2b9e59005a2fb8924943971fc73f7bf77acb0ddfcfeabc62e172a78dded9083 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.Rect 8149befe4052794e02d0b20d00d57a7d7a376efde35a8192d027494b58e6d938 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.Shortcut 69860eec84db27fd1d6cdfb4a168b08707dad4510459c19ad34705c6edccda66 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiLastItemData.StatusFlags 41a6b9fdf38fd5adc0c516adb1cdb10c276ad327efc4804c38800bd402a6f233 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.Ctx 71d38d63fbb05d5140be4994f6cad3a317aa6373dec774f6dfd4b17062ecbafe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.DisplayEnd af90be237f8deb0194dbcba4c743b70608d6225953ea67a377daaff28d6eb197 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.DisplayStart a14da770403f53aaab085df24928fbe514b3bac1a486f594e5ce9b3d0e44579b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.Flags 2eeb6576ff1bbbd03555193e52a814d00402d6ea80698f6c62034b6d3c7d31af -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.ItemsCount 933baa0994c8634402061886dbf348f158ac87e9d10e5ea0b172a31c66a0bb57 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.ItemsHeight f2087e67c66eb48adb39723735ebbefa0b49f5dd9592f34cd471b49f9666640a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.StartPosY 1d86f97acfe06448bc99e8523bddc16a07a40d69405036a0d20f77b1aabed901 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.StartSeekOffsetY 85530b67620bf9a11a8b610a74fe523b259dd555c2adef9fcca7f857b7c59a82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipper.TempData dbb07fc9db658d780d443ff544028078f740bea908cd6906cc980f4701f558b4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperData.ItemsFrozen 2979adae88b4910bbb6b194b411a2ce5ce5d94c5b24f1bedbcaae2d997b1baa7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperData.ListClipper 4949fdcd67c2e8277e7b28eeda0514cf5dfcbd8bbcd2dcdcefd933e20dc2f963 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperData.LossynessOffset e4b05ff805fd5d79b3cd5ad3e485e29f6e574b789ca7758447d91b4cc472440a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperData.Ranges c9305cd239a436000dca256806d893193aea0605ad303dcc3cb7fcda73793593 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperData.StepNo 11463af8d61a775ba29407ed931c34b7e20661d6a90d4255a1190936b838f9bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperRange.Max f70bd525dde29c9e1e1a03f7728bb6e93359ea710e4334d89d59913b664758a5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperRange.Min d47e6411c7fdbe3fd1eb76429401c6c24f5dbc4cc89a51fdc79a8cc8184c98e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexConvert 2c79ac3e23b15994856b0bf817131884186a2efb8edd216a35603909c0bf1d2b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexOffsetMax c57cd49c08f1e0d6ad96862375849fdf91f85f4f680b2592ea2edbb3740db1fb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexOffsetMin 05c22ae03a7840d54aeb7c27d5e4a001761bddd02d4172e1bba77134ac46567d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.NextTotalWidth 4d1161c23f0194b4da696a83f30f5c5d3dc07f5a3140adcc06af23098fc19e0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetIcon ff54e3dda11d6502b27474cb070bf01f89a9b49278bea2eb896257267e8b2e4b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetLabel 586a545f562c45000a2b12ff9d61b5df1b5a12001c036532e80b4f9f21b30f82 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetMark 62bdd0b0faddd3a04fc8bbdd982c089768aed225b4f9fd8d93f409c5650f668e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetShortcut c2351bf751ce506ae27329759acd4fc27d61522f18e299a3fe834f4e2a17dd3c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.Spacing a8f45100ea330bf29e719d6590b77416c20d9b3cc1c6c6fb4fd26ef2a3f3dcb5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.TotalWidth 2b2d0b036db8c8ec7a4068df3de8abed5e1532ace3c5ba89fb1c83966cbbdb50 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMenuColumns.Widths ea5469293ca5b623c1dde7eda69a5570e3ca235f5a493b86220423052c5db47b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.HighlightMonitorIdx 70e67c5d177b8437baf3099c0c82f37bdf2fa5ba460dc332950f4dbe60e46364 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.HighlightViewportId 91ec75dc50fe4805ff9c8e61eb6e6e3ac46103aed067c2a2c8744bc18c4ab4aa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDebugLog 84b3d1771b76fcebc7bea6358ea702d4b9753b5b93a21e44b0a692d557b1fe7d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDockingNodes d9f99e8747793bf771a81b3d1a87075796b8efd7bbd840aa047bc7bdf367cc71 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes 59b865aca34633525c41ab324980cf821cf4a39c1e93d1fd370aae1defc2de27 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDrawCmdMesh ad082b2d799a32cf0b47721cc69ccad687f2dc86731eaff48f5befa925ff9e6c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowFontPreview 920fb20aa0cc2845535e5d9cb0e783c41b958cb0f56ab0be7b469b31ec18a697 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowIdStackTool 3efdb12e39ef5527bfe010314820e1ef9f5dc5034aeb5529c4d228a7c5986a97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTablesRects 8046ef44ee9ba02a2ebc0d84f5a0ebcab2e0014cc1e945447872ac930a36a296 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTablesRectsType 69f9d9fec2e3c5389c49c4242f94ea567e92a3b99cd08eafde2ac57031472bdc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTextEncodingViewer 44d652e3fca7a1e57f43310f22fb173264233c52ad81aab67d0320c4bd908563 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTextureUsedRect d88fa6860f7f5fe46d001798785f07d2a3edca1f24a5a70020bf2b3ba1049d16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsBeginOrder 57af7b8c69d1eae28d2ebe4dd42cf3cf816276d160c2eec1c3e0e67db5ef64f6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsRects 57105f4fdbcdf2802020d28dee2d1df6aecbdaadf63eef57fd064950e716e514 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsRectsType 7fed67c71d15c4fdb7e9e5d722b276077d7b0b94e6a37865e7fa4d0c93f2634a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceCount ae8e4ab43850ca5f97835cd334fa8727c4a5204b976f7b2cd4f14b0cc1ec5465 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceMouse d1635c39885c3c3b6c11b15725baab5669654b996216aba1be31f9e991b6cfbe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourcePen c65cc8b5d3780d36a18bc6f4b618ade9816a0fae98ab42b3c6bdba2e9a7e3047 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceTouchScreen 69763da7c1259d4a40a903da9b93b0e0fe0f511a6a208c592900dd8c9a92e6ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.ItemsCount 0f752a84f3cf56798b7a0a4fec6adc8d34cc20f0bf52463006a59756246a13f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.NavIdItem 811fe8ed40013903804cb8475f7fc781c9abd9824070378521b3bf8b71f62a40 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.NavIdSelected 42205b57fc2d0b31d218feb780cf9bb1c1e5ec9c3cab74d744c7c4b5cd65b6a9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.RangeSrcItem 76b7027c351918b375a4b8f4c751a7e5bc9675a1b0e232be1a4604e0af755a5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.RangeSrcReset fd76145b5f193c30c00f620721d0b23dd50991081323e59cbef916ed32bd656c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectIO.Requests ab77a8e18001f21c17f2e0ed97a5bab2135197b41791262b4ee0300cded0c1e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.Id fa76963869db4e7061899fa04ed2d1fafe4ba5171f9f9d88bbf55a351694f7af -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.LastFrameActive c5c6ba0da375fb918e497feef86d73490bf7768d129aeebb1ec42e464d262ddc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.LastSelectionSize 5396ecbe6ce905e4ee59c5832524cf16e5d23c1849b39d0d4fff86fa84376daa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.NavIdItem a6a4cda58ad435f63864307990d730addee372dffadf7bb67e78573070dea8df -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.NavIdSelected 30ec0fb8e0e219b29e82867ed335e0b0eeb011905d7eb87bd8b5214267567954 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.RangeSelected e8f7611d7d59636dc0620342a07a6495d0e0c335f04d12d99b9ee150b5cb66c0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.RangeSrcItem bd7e688bee02743b0504eb69d8bcc0659425a59b68d56c09c4a6f8a9815f0bdb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectState.Window f8edb735d269266e39d8fcd1002e78f7cbd1929a74a6b955eb7ad7dbe0717866 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.BackupCursorMaxPos fa18f6862ac66613a6424f6d17f1c3bdf51a7fe794afeedc588cc080631b0ed5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.BoxSelectId 11d65e2c55f19f97626297cc5ebfbd09c90f60ff60bb32b9f6d26cce8a3d638d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Flags 31653ddd0ffe70baaba6d8afdfd36260012617962c48daa85b2d2edbfeaecf17 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.FocusScopeId be386f5c1a63fb20b8d2951b3bf001bb76cec9e9bbb7571bed0f293c7691a3e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Io 20db9d248dca2260dd857589c562803dd00b6f32481114c392fb9975c8522672 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsEndIo 08e2dbe63656923c041e4c39218c8739b0e9a7deb4bc3adf80fd27df1f3fb6b8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsFocused 0d296cc0789df93b5ba622e323a166d17cbbea6acb9f087bd744b108a27db409 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsKeyboardSetRange cbea56de1563dff17090f95572cdac718d215a92c80b32635d825085a7e7c15c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.KeyMods f011bba28a69fe4695f49671110b6af4036656852ec9a69d160485cfd432330d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.LastSubmittedItem 9201f1b356177adf41a7238c9f0227a034749fc902fcc705d0b28f9fa1b88418 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.LoopRequestSetAll d5b3eecd38fa99552ee94397b6b34237ef9e4e7acbe9ced9a7840dfd229026fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.NavIdPassedBy 6d82d2323b899100e06086523e044bd6794e131bd9df7a35a76ef20e33a3b001 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.RangeDstPassedBy 143ad784ae3a2dc8b9df2b2a3692a354a3bd959ca51eb3c10ce70316dd4307f6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.RangeSrcPassedBy 96595426674b030c5833aa6749ff63848d663c152841e347187288541bbd7dc7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.ScopeRectMin 5e8c3be7ea387ee91bf02a67f92ee92b9516e5ec176e857b5d92bf7f55e40bb3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Storage f07990f3e21904671c9bcd890ee09366b0e01aeea2888dfb738322ec9e0210e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.DistAxial 9729aac1ab70119750b8266e022c7992d4b776b5d6936bdca25c92a7756c5e0a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.DistBox 9735f9936b367c1739c02b3bd4c5d76a274c38dd8d7b417c54f9bb3cd4aee771 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.DistCenter 2db88b067b2344fea9ccfd0970418d6ccc6aa43df69f59363a96a5f230723c34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.FocusScopeId a717fa8b93a43952b4f75bf3a0db34ac626bcb78db8bfa13436010235c1c9eb3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.Id ad09093c6903051ea5147f39cd767f9fb7e7aa7bbe0cf7d0270c86663482e7bd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.ItemFlags 6f11a4f5d0949e50f5351fdf18e435c6f8f5682fbe0ba1aba0e584a613fcfcdc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.RectRel f9c34d9766916d1537c335c88789ff01d43c2ebfdff7ab763979882c60514eb7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.SelectionUserData 7594bebdeda65c9eb2c4c9668d37e9c408a9cff310abb1c81696e99d369b971b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavItemData.Window bd7d0657309f87ee4c73b383ccf38fb98e401a83cab10e7fadcb6447fda4b48c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerCount 06342067285d2bcc58053c356d2f1d21b6022f0be14d3caaa1721fb9d3bbf3bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerMain 4dcc3b626960582bb31361375f22ea90e0402ec051e6cd1f1de38683a91a14f0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerMenu 99de329d9c6aced7f16b8f4ad13d1d3014dc1627208ab75f39cdadaf8725c2a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.ColorMarker 22c62bca60cbfba108b551f10b46fe3a042d1b3d157829ddc178ac2e9029d657 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.FocusScopeId d9a07a60eeba6f17d56c9803f0b21dd929b2ef49a024626501e46e303face4a9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.HasFlags 08407cab8ae619dc3bac865c5610faa4f691cdd575765a23123cf59160be25a8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.ItemFlags 3a2ed5448890a94dafa4278df845bfe72ee1d6fcefe247479936b4550e44977a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.OpenCond a79de14569a0cc9363914dc26576ef4a13b9bf4621e061a3690a0e8c82c50976 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.OpenVal ee57aa1269aae53565e7b2067e494a60d909d8f57737534b810bd198ef9e7f09 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.RefVal 95a29f11ed8d89f3cff31447f1dd28cc1a7eb767507b626bacb8e08c96eb73db -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.SelectionUserData 544fbe1d90b4deca45c2c3b6f3c90d7437bb04beec39b52ce766513617031ed9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.Shortcut 7aab44a54fad0843513ecbc42192c8f1f1660f3c6fbdf880912d71452b96946c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.ShortcutFlags 8c75cbea709990ffd4f1820b2fcc6bdffc1571b97e975a4cb7ccf8a410885368 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.StorageId 1a8b80eacc1709b7e8e3ec073e7992e642eb7d671420660c97af46f9e3c2b519 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextItemData.Width 978dba85a287eafa1b80239b21be11de4bd1ea6cb3ea6611532b13c1f8d342a8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.BgAlphaVal cced8d0bdeaa1a6da0a644eb357904b396377579e6e3f1143a4173319b0a7be6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.ChildFlags 83a5a3b301db3d1a632769239d9043cf97dd31a78942d54800e837a5c0874659 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.CollapsedCond 846322304572c5f5360dbf8d5f4177952c9be4c0dcf68e70b5445e93f1125278 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.CollapsedVal 17d6313e0c6a8d5e6f5352f2fe9d22686ee38ef9bc5ca65d5c571b07ed1ca157 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.ContentSizeVal e53fb5ca9ce8f56974b8f52b26f5583c2063f7090625c602cbe67ba639981ed1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.DockCond 37f3c027e0d3ca71ecd637c59f6484338acbfcf42bef13c5cdf1b54365a04fa9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.DockId ab9716eb9e58e2af38ce5e08f6c45bff7673f0025db2a8f7911be590e88985ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.HasFlags 2e4adb24d1db1855f6dd48518ce9a0a9b9120016bfc0a33781cc4fd9d33dc80a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.MenuBarOffsetMinVal 7f1f5c2bc93f99bf073f1189b256f9bc6c53c9f41602ec88ad08a26cf5bb23b3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.PosCond 90b1e860c1627e249779adf9dbb70ff896dd36c6654b05e546a13c7cae61b322 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.PosPivotVal 03a6abfb44bd58430080a42712bc7e419b0ea0f9e0713d002a43285ae9b816be -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.PosUndock 30a093c2401165b81a28dc14171da13154f956725a788c8740a5d4462b89de7d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.PosVal 72c6784fdb225251e0313e1ad5b3302bcc7fbaa0b375bea9503fc01d4595dabe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.RefreshFlagsVal 1829f910bba213d0127f3258196492a3d59fcdb4e644804b19541619b7acf30d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.ScrollVal 71657224766987745108ef0a96ce92e8d3bf8053de8aefa62fe4c7bf38a8f984 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCallback b533ecfbfdc502623cc0137e0d56dd60b39cdef4bc58ba4cb07627c525666f0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCallbackUserData 227fbd61581f9f36b9d77cb1019738e996e10b03c3e8699dc1045b23ff0a7377 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCond 452f588a2a50115938b7861c772805be8a297d756d887e772ad0783411c62290 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeConstraintRect f867e2423b04b5e33c648ee270ed4e6bdb3b0fb40aafd7eb1fa2fba48b8c265e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeVal e03836dda36c1d609c3dcdcf9ca5bb5f08c7be0465ed38bcf41b5ed91eac73d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.ViewportId ba12552588c875bb347c97d3061b5396535e99fec82ef4c466e4eeddcb99e2b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.WindowClass bdca19eae9e7fdba16ef0de2af3c079c4948773b4200f1e01a013c3f1a98a695 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiNextWindowData.WindowFlags c9d77e0e6bc83d668e72fb6e441cbf3fa1c769f77d3f68d62d5b1ad352e93928 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumnData.ClipRect 51bfdc8f6d31ac194cdf3a844128a8b0e788cd8528de019d88801f2adc93750f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumnData.Flags dbcb8891eca188b7a7721a32476680469fb4f12e68bc5bd4d1c4b2974f20b1b2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumnData.OffsetNorm 3991796dfbe9112092f0b3643631aff4f014071e6d64bee8120111892f4ef47f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumnData.OffsetNormBeforeResize 73e8fcbfab9250445295aa2857961cce1189943d5bb1c7d0f3c2bdb3ea229fbf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Columns ecd661b1d56f41d2feb33a06796e3ae20e648630a5bdda331cc83fdd808158c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Count 0ded4fe3672ff89ece959ea919775910f27e5380b3861dda7361c8132e9405ce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Current f216d122d77c508431f3bb6dfec3850a6fcb5501b5fca19c0e7d06ef26b39cd7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Flags 1ce6b58d7909537f376e3267a48e4b08c1643ec47d54ca00da222cf4488c1222 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.HostBackupClipRect ba5ec3dfa16b10b181a1e294276c617c77f25409d5d71cbf73db777ed230c063 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.HostBackupParentWorkRect 2c82ba951e3246cb0f3db2e70dd20dd50162d7a9d9265e7647616a7488748c05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.HostCursorMaxPosX 776f3deb81be9301b18d5c966fadaac1a7a611f14bbd2eb79212a3236dfc02ec -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.HostCursorPosY d9c8f6e153aa8d6ea5add2dc40a3a829e39034c2d9747ae2682a254272d9c7ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.HostInitialClipRect d9ff155f8de336adfdb8ba5b6322d095b8cd12bbfb31f2b7c16e929dc8d82f8e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Id 6eaadafab18df1e7a546dd2d97cc2be5c909d8505e9e8f334af09dc7f4de4dad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.IsBeingResized 294a7b2a1477a4c96a3e593ae9ea349a976d61291325ae6ba5a1acd666c79a95 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.IsFirstFrame 4f11a365bdfc0b461707ef0a27d9dd56390f74a31e82bb33b60e496235339f43 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.LineMaxY 93accc4a04326388d69f52322524d590a55a0de39de93ba753902038e32fd4e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.LineMinY da4dac4f5eaf42ab561d1efdf90d262bb7d3843c615af035ded7aadba6194cd4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.OffMaxX c3f7fe6d275edfaed2fa1110c091b554ec122cec99b4999b27c9ba52f3a5bbb4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.OffMinX a990af9a94a455a32eea6c062de90a9624ab78b6279e4e49df1f9092ace97970 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiOldColumns.Splitter 3ad02f9657561baefafbcfcf356d48a9645bea8b0ff234dfd75bc427313af04b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.Data 9a608a6ea6ec6a0b736fd9bf812798c730a71f69e9e34a1e542d936683eb28dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.DataFrameCount b635e28581a4229eb5754c3fd487291a9c723eaf7110126b7f08525c59cda282 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.DataSize ae7d5a578b1bdf4a819cd0fb21034828677c9f803bfdbd1b7192404825c59757 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.DataType 1ed0ed3be8f0569c913860c2ec1c4afaa41abf59f8a000b7ef6e9d423b9233ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.Delivery e044299da466ae15f559cc1666bece4e9bbe5a1b6bbe59de800fdb0d30e67462 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.Preview 65eac529cdc39adb783f688ad14eb3757e449012ad1458d7f24298e2ed40fda7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.SourceId 554a54864ac8409ba5a2f1c7ba7a56ed368601b7229159a2742a2eaf86b84ba4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPayload.SourceParentId 6605dece05152f86f94c139623ac94619cb0e2d71dc8ed502d0174367700e084 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.Monitors 8902ea5e08364f4906ea2def50cb2cefd814261af94b8744bf2fbd6e5e05e51b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformClipboardUserData 508067cb5f2536fa58acd7f0cb01b4f691ecef61f00f0c89f0607eb00cd8f6e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformCreateVkSurface 8ee7ef808c56d654f7f48a16285d90bb5c1e45a2d2b749c992c84fca5b292d16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformCreateWindow c807beadbab06704f0f74212ad0fa32e1b2c32fee711baf3d1c02bf46592e077 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformDestroyWindow 6883edc68cca10578de9141a9c0224dc356364c9c8b18ddcd2c08a0e3ef45928 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetClipboardTextFn 33e384eb27dc78ee56d07785c4c382a0d7e36803950d6bf6313ee4cfbcd30eec -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowDpiScale 74dda3a881691d77e84c7e413233a7bf778f21c7fbc707308200f81103deec72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowFocus 0933d1672a047f1ecc4a3b931fa06be82c4b1e8bf2803800be5bcfce888e3e06 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowFramebufferScale a754e8b4cc77ec6f15acbecd7439548c9bb0484abbd4b399e470293e56339ca9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowMinimized 58ff9119783c279e6a7a619e21379e1f67a90ffabb61f8b275a0746f69388554 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowPos 4632c4d8005d186a9b30b282df610159e977de79855a4fc40306287380dc0b6f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowSize ac0de67d605f07de2791c649684bbfc9b9c86d9f9b076def77b6fadf30d7ba21 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets 42ff1db888235077dea3ac4f04e777d96cd28e2fb9bf29482f5671cc7ec085b4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformImeUserData 1ced143c98adbffbc4799bd9d93bc405bfc1bc4df4ba16a23db85b81fd1a7b36 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformLocaleDecimalPoint da628c9b99b0ecab3d7df39009bdae4ea156a4007f2bfdd65ef6cffc9d5f9230 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOnChangedViewport 6b9d3c034bfcb61a373fbd7c7c82e5a8162c15595aff958336539fd49b116d64 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOpenInShellFn 7c141e1a07bb0a96530f00e67390511cb05e0c453cf359127b97050bab4b5825 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOpenInShellUserData a557de1f40df31d123bcc1502554a086b2becb4db5eab46870be08f3921af008 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformRenderWindow f5327d3d01b332ef1bc23fa1487239dc03067516f1c716d0cb4db35efac21009 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetClipboardTextFn 7d59d65c658d09077172ad3dd8dbe9f2ef1e7608e4ab813caba055b86668b51a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetImeDataFn 455b0c55adebdb41a0434c4e2a681ea724e9de7ff69608db67002ef2370b087c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowAlpha 1c73a94b7690a9a6137cd31c282485ee4a2d55bee34139c9c005b13e82be02e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowFocus bc269f56fea3c9eb26eb37a6d4afb7f1269cd21a7914cba3f5756c35072ffe52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowPos 8515a20dac34b1de09a63a8717de1b520c6da636876b9dcbafda61a988518ca4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowSize e3cb7708458dd2df513e6ade28ffcf3f93fc2820bdc3e4ec083b310e3018acae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowTitle 5c0de0327bba1a7378fff7bdd37efe86197347cd81ce3676b4cf0e0472bd71e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformShowWindow 91edaa9f806539c9e5729cda4cee1ec4bfeaff8d436d9a7c7a70e702988d1703 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSwapBuffers 83ee6ef5d16ae6b451d73aa8662ffd5412acb281191ccfffe7f3dfcac4815fa3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformUpdateWindow 1375a85f3a9e611911842dada1cddbd762a47edf7a37969b89ea8954c72e5e0e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererCreateWindow 99015cb3b713d2678573b59f5661e23573a7a4f818a78936f0653cffad7fd625 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererDestroyWindow 4ecb4f214ac143fb1d4b5da306c6483637687c6aed7c74b5370dd5dc5ec29728 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererRenderState dc8e0f865043d155aae6a86d8107764c38f0d896f9f7760e54de8a7d63a2af29 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererRenderWindow b08c3f0c2637e1f8bbd4b1fbdbd1bb92ac3f9feafb2c7191ed53cb722aaa7eaf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererSetWindowSize 78f1e382ad3bf29377136a3bd68fb2e1598d2562f322260d50098cc017117c3f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererSwapBuffers 269eb9f5a2faaac38624309d20c46dfea42e02ff4c374650cf6a0cc563e395ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererTextureMaxHeight ffc71be805edbd7d36199d506ade4cd7150f5352e6f7f56b587e43ac34a92c05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererTextureMaxWidth ed98ee6cbb50007534b29ed03c5ef6e3127e225727e06174e900fad869aca882 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.Textures 417624220293c265371d8e18920600d612d6e7f2d9a9a8baed7d35b7594a206b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformIO.Viewports 100097e1bf65e06cbe1a5fae498cd5b969af7b9edbc94e80d170c745ade2fda7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformImeData.InputLineHeight cbd9f1ab2bc21f68f69e04937fd4d7f3840806eabf14ffd265457c5d16d00a22 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformImeData.InputPos 91918063715af2625eb57b193eb331805bc754cf6d46908d2a0ae7e5a8021e1b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformImeData.ViewportId df7499e43c25a5dd354ff5fe3e8d0e2260d57cf5edd19c8fb2cc8b15cbc66dd8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformImeData.WantTextInput 440376af914c8e60b8d87fd7c47b74646ab1213276aed8621a8d4f98ad24b694 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformImeData.WantVisible 316ba7003bd1818ce6cace03adda742c614d13e10eb634e2bccfa4eeca064350 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.DpiScale 70a2586a5c05d2fee52dd6e6e18b1004bb59b757fb499f654d7b3ebe6508cabe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.MainPos 53afeecb1ceccd88943b154d2f019cda005f1bd64cc9844becd54c1a8f0563eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.MainSize 1e8cf0a58f8b69d9a8ca621a3488c8945fe51aa68b943ae0dd05703d8a122952 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.PlatformHandle e8fc0e11a7bee1a344631ec245ddea9d32c96ee725e5df16f19d6d121147a4a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.WorkPos b140eda4f911b6e0f67ada6d89662f326ba13743e12597888b77b6174b1ec5f8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPlatformMonitor.WorkSize 282222096450c7b1e72ae2c13e1705302c9a7fa7adfa15bf76e2df10321b459b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.OpenFrameCount 41478840194b6249eb73483da643b44f733a1b861560004d9c20b016761c3a63 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.OpenMousePos ae4543ea4ca7cf1e6cc5c6891eb5428ae955fa6bc9950cca66977371290ae7ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.OpenParentId a5dde1c67de5f32ec2a4acaf784c3cfc34805ee2a94b8d2f324812eeadb67da2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.OpenPopupPos 66bc1ea90ecd105f3111c04f8071d25d3870daccdc92daec0cef3b82ed638a4a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.ParentNavLayer bfe5d515fcfd4533f279d1954b6816d72510e34622f4d9742d5fa13beae8a813 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.PopupId 2b02430b646a5b148bb06493657193a3d9f042b55d6e27a39a3ea10f232fa0c1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.RestoreNavWindow cdb22f56fd73440ebbcf0448fe62b401ff325b47b14430b91e8d138069684c80 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPopupData.Window d7689352f3442747cb88c507be12dc736a8364c5721eacdbe89359f49fd6ce5f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPtrOrIndex.Index 9f64310f84940ea705a9ff1484555c014a3d2c9fee53767fb8639e066b4be7d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiPtrOrIndex.Ptr b6f80fac544f2404a4c959c20a113794ed27302ef3a6eb08d9b445ffaea20d38 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeDirection 05a5b51d9e0f4d0bd795f604408710bb2a2fe7777e1a4ff0ccefb2c70f61f793 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeFirstItem fc98c4a3be7835e1a16e20ab344f46af2eb49e76c1b169122d32294240848e41 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeLastItem 5b66229a0fb7cf4c0bcc5ad51d15eb5873605df31b5e25c09138acef6abeeab0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequest.Selected a2c9548e9286bf20c6c4cb9c089ce5dfb7d8abfc00a35aa6af80cef813bba6a1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequest.Type 83ebaff191a65e80bfccc99bbd7af3f753aaa40f7789fa708bd95a5f9f6ae1ce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeNone 3b74f8d87f146e52003f65a046ba7ab525ee6349fa0b7371adc5c994bce8fa02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeSetAll c5bdfbe1d306680c224749fb1017d400c5b00ffe293a1f3b0554305bc75d5c8a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeSetRange e614a14b58de043866c462dbe24c634e296ca02b8741012e24f2cd509230c660 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.ApplyAllFn f9ca24d78842176dd17d74de904d000dfcf5b6b97f3190514429d0d7105bc197 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.ClearAllFn 2b63f4aad321e48ccc13337fc878ca529f67873df5a6688341a499fd9e81f036 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadInitFn 4270aee5414615a4d92dcdf45e5a563df3e3c7208dd513f33721d158898c25e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadLineFn e720eb47679385f62e79aa09e81e2d11bedc08c9694dec07ca951a70a16f3bd6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadOpenFn cb1018eef54c61d30d3f3b1931b7e8b01971e5bb28d0e6a3ed3ac46ad11a070e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.TypeHash 06131e67a8c4e50ff1347ce12b3e353f37ddf695f3304a13ba9301e9142f9eb1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.TypeName de931b36d284ce674264337940de362493fa38696611642717c07fa474a913e5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.UserData cda720c31a9b250a0fc9ca31644114f97ba5bfacae8e6b308acbef60c34a7095 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSettingsHandler.WriteAllFn 41a2b0f4ec9df0ffa6ed7627a2d7b5c2d291ba035051f145ab0a83118e0302c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.Index c8e99b31cb666715ca364531b973b27475d3b9f184ce36ef9d02895dcf744d3d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.InitialWidth 8af360c78581692a16dac78677195d641a121bdc966b7d646f3792d99189c06c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.Width 1a687e480940beaebe8b06c58e984551bc78f0faf5fac0cce9ad7bba05bba22a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSizeCallbackData.CurrentSize 4683416becc8c5cd287181d8cc0e4dcff01f4a583ee06a451b60dbc61f9dfebc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSizeCallbackData.DesiredSize e8fb5b424bff1e869140b37280893bf68fcf4821f97f50dfe0b5416560469394 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSizeCallbackData.Pos acead9fc116d7460166b1d3be630741accdd75e8dcac1ec9fc72b93674344233 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSizeCallbackData.UserData b248cfb34fb06ad1db76f584eb6c8c749bc37e511772f7419e47a68ff6cde34f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionAscending 35b8050eb70b12b3b5f3d1a914b77dae1fd3a72ad216be9c98b5bc6e295d1b41 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionDescending deb3b37f0a30c92baedccfaa43a981aa2cce49262927cfb1136f4ab7e89d744a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionNone f9539a7879eb06d7c9904ae63d5675103cafe12fb0dbb3fdad9fa424fef54710 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStackLevelInfo.DataType 5667cb620559c0d872d11f29c2ffdbde5270316114baa6c465762fd3736ab361 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStackLevelInfo.DescOffset 9a1e4c9d94148c7262ee5777c3479b1770cc1bd0206757f8d583d061646c343c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStackLevelInfo.Id e8b33242e9120fd1aadacead4b7491c591566df1f76a2ae943835dcfafcf96e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStackLevelInfo.QueryFrameCount a9eef4ccaecf7e06c560e6a678793b6b571f02783c5701377bd5db2eea4ac4d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStackLevelInfo.QuerySuccess ea92d7d39291e4eab04a23f3604201632a3a7a8b276dca243aabc077fd304517 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStorage.Data 1b7f0a6d623cfa21d704cce129a7052a54512a839e47d11729f29e367ec349e7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStoragePair.AnonymousField1 d3a3cbcf8ee4f015c8c271efb7045773464b06377dae3217c9122caf8b9e4fcb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStoragePair.Key 214b892e98db083b2caa98f6de774333ac9f18f1f535744e7df83b9f305bdae4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValF e582b08f60af09ac6d7539ff52218fdf43dbed36bc914cf2ce8226c025d8f734 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValI 93d75a4beee3600c09c70302a7c4692a0d17cfa9ec447a3af2186f7f481d4427 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValP c00f482cc84fb6cb8e23759fd04163abaeba0bb006789842100336d4b720030b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.Alpha b917483cc65e280ecc32ce519129345814c9e334376e871434b462df1236bbeb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedFill b73c9272d513a5d744a8b92cdffd025eff6e085146f12dfa5d9fc44d47ed8e00 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedLines 1a6e7d3b27561e4a85b470d37b41c4128628d8f7ac36ef96618a6d427990155f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedLinesUseTex ad736542290957b8f23d5563ea2d91cc5e549d247027b4298474222691b6fc6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ButtonTextAlign 3f7d00e3302c601d2290a3f4b3031c1a555fdc98fbcc575d0a51e1432675b246 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.CellPadding 2331459e3228611838c7ebb25c4aab1ddce15d8b6303cfab0fe91885b658cba8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ChildBorderSize 8eab765843e75fdf2d03b8e839a339c6b05d282fff628791f87d4972e906d472 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ChildRounding 751f9c9573b452add8dda6d445dc17e83333b88a87558a3a681ea095a90f8689 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.CircleTessellationMaxError ef0c85e34892fd141f93e657c5dab0e804bfef717a57486cd0575f324e98e564 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ColorButtonPosition 820793aa23bb55562fac5a4dfbb98e3c59c279e399f89f5b6d3e144a5b3f7bbf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ColorMarkerSize e11c8d0d9ab4509a58af0047d1a6a8b4192e034c469d83d151c2df389cbe339c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.Colors d5a9a04ee81a998224289851aea7b8f6036b6c318a4eed88106e97a959b72228 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ColumnsMinSpacing 8be0b3cb3784be609c0b58c356a65a7584ac2ea83ac58d04a72037d8ef395bdc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.CurveTessellationTol 280e5f49876e84ea76579735f28f3b903e2d33ea011af91ad4f47c7749a40a04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DisabledAlpha 6e176dc9cc82812e90e3c4d5789cd8ed9eacda9c3426bbcb0d85697309501554 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DisplaySafeAreaPadding ddaf96308788fa9eaed867b2c2926c5a117a3899169b0e4f9a926718bb90d03e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DisplayWindowPadding 70ccab16d1fe50f1928222211e24dbeb4d6c20f398193359de214adf87cba009 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DockingNodeHasCloseButton bdcc2330c53e17bdc15bb59f403cac942167ab5c5423a88168a4eaeca34f76d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DockingSeparatorSize 033a58b8d17348e228de86be2900a1f3f54b6c3fbd1560478cbe86337a89351f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetBorderSize 0b0757414cd5ef9913c7a8d0a6cffe31fe1d227852aeac3c44487784eb15f41e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetPadding 6a454ec9443d4695258bceff8eef9c50c05e85adb1dcfca51a1e27dcdb7899ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetRounding 08144778839c4ab37c1e3ece5dd6a087b3e21234e5d6185827925dfc10070ed5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FontScaleDpi 2744bbcd0abee0d1e3c5d6f686a15a5093c73be8dd022f2f86c61fb70acacc64 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FontScaleMain 88754004caca9681d10ff6161ad1b9ed487c6f1e0b2b0d3ecae27d3c6b45ee45 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FontSizeBase dbb94313c9b6f8958851a75cc26cfc9e310670043b17a20d0ba907687ea65303 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FrameBorderSize 1397c007fbc01219d2a36f39ee505ddc1b14041096eedf3eb13ab6704464d000 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FramePadding 694e63151edaa74f1f360a253de63748ad154656bd6f8c21b100c92a64c8fe02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.FrameRounding b2721744eb3676df59f7c3fbdb9287bd65c83eb2fee9224cde6a3c62214c4832 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.GrabMinSize 1a64e0732027c4d6e9480a02d60a15f307a19e9cd0af75c0a1550ca539b0c830 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.GrabRounding c34c4f3dc6dcf494b6d2066738a1349338f291fe6a12caaf8e02083cc82790d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.HoverDelayNormal 767bde223a75b8ab13fc078c1bd84a5196dd99bb872c186e04d0f3f046f75938 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.HoverDelayShort e53dd66028e18772877450c12442d57ee5c27372b1c293df98f982c0f34a682f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.HoverFlagsForTooltipMouse 3cd708386b748b087df9b81127a466eb5551abd76d9c0ff5a590c0cc3e98db7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.HoverFlagsForTooltipNav 6cfd93068530b8915c000990110549aecf0ad1b59e5ae0b320ae03c8d841cd03 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.HoverStationaryDelay 465f9b0f6bc1d15a15f5da793cf39b06081f5b03465493d4465b770eaf65cf22 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ImageBorderSize a566614ba9ed7a203d2f577684d4170968a7c70e6e8484cbab1c5a04a995496d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ImageRounding ddedf066cf55caa638d73e8c3a276ea658c1a927164a5da43a6b409155157756 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.IndentSpacing dd09c4423a8599dac13004a9c5f945bac9bf004a0da69846d5cb643fe33e4ee2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ItemInnerSpacing 15d93a735cd2546607c6792d445b390bea582475ca50ebc1beef1ac603f1222a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ItemSpacing 82770d78bbc5201e6292f21ba0d284e2312cf87b8969c63ba2ea03e1c3d98817 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.LogSliderDeadzone 1b380f9663e2196e8ea9ad6575248862156963a8e29e22785d52b7c96a1286b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.MainScale 7f80c68e80b2537f8faea01c5cc40762c3309bda12b9af28939e44b980d32b93 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.MouseCursorScale 688d2c93502e7e1433885bca3d33446d03d94b47e2240546e7ac1d852731a942 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.NextFrameFontSizeBase 20ecaa8bba19c4ba8d14d4875b00daeee00f7981d09435e4a79ad8b2d3e0f572 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.PopupBorderSize 48028525bef6692d9d34f6cd01c637196461193bd2be1189fb5d2de4c68789fc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.PopupRounding 69c2de9ebea38d6e327b20da192b7baccb3c8dcd23d1c8e8476b7abaedc40f0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarPadding 386c7f9659afec812c3a8221f0ceb26841b7ede0039e37fd501f8c84ac0e540c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarRounding d118aff9c8b3488ddd362be28add5517f2e930e489235b935d70e08b97a1cdc4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarSize 1654855c8254e1b0e03f317163482d22359d1f53baf65000616579c68b662b73 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.SelectableTextAlign a5f9caee6dc6a9a7594a331b1382a11132d8807a23e41f50ba92a7529ed5169c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextAlign 9a5164210791a88c007ecaca8e1cdc76f160c3ce6e601de6c66e0fb2ae4a3fc5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextBorderSize bc6ec4556712774c264570d5395444caac77e47f2137999716e16f439b9f3d19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextPadding 5412a16ac597f2e968bdf170f13e31b5c819d1e5ac23f1c005c7afa64f95b953 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabBarBorderSize fbc91303450db115a2129afabc67abb3970164ec4effa08d1145d7d50cd2b893 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabBarOverlineSize f88fe7619c48d445ddece4abb8cf45b34e49dca0b15b4cce55ce00de9796511e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabBorderSize 6eecca142435d400fccd5bfbe55a49d266f192bcdca16f31b9e73f86311be3b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabCloseButtonMinWidthSelected 4c38ad7a7dc2bc96f73cbdb3a4ab655ac8d923bc1a12fa34e17a4c9bfa771aac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabCloseButtonMinWidthUnselected 4229ff393008af4ce58c823df8f0407a5f22454f498762043b0d5985a4eb7d7b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabMinWidthBase 786f2d8828c9f026848d46960e097f893b7cacc28686fa484c54d8c70eebccd9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabMinWidthShrink b0404615b89661fc4bdd7e902577f05bd890dfa0f9f8037d8451fe327ed62744 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TabRounding a2b3c37eaede8fe4b97f2cf754b30b4a406a19a31f30a07d5bef4fb2dc68e6be -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TableAngledHeadersAngle 5e12efd536a2697b2c3b67e6ab3f580f58131e4fe5ec5074bd1cc11e85cb9eac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TableAngledHeadersTextAlign 19cdf10b7f86b17babfd0b298ec2f4670a5b548da540b197ee38468e4bc60929 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TouchExtraPadding 7e65c8e864eeb0a9b51f3a50c0a40039a90e58843ec704e8c5d1ff54d2ceb3fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesFlags 051e93d0daa7117d9c8d8a0a5c14d69ca033c1ff91a5ea3abf20f0d4753fc9e3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesRounding 44094c560b5738fdd3db20bc7a12fecb5d0a38196990588efe9d9d38e7f8a5ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesSize 6e856f0c03fec4daef64b7c91f66822019c48939da40d381d35f17baf17dc764 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowBorderHoverPadding 361a187ea7d4edd8d610f11850984ab2c85f45687b44c4a2d3a71e0520a7c2e0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowBorderSize a7baa1bda324484b279725da0782c3a8f6a6293e067437fc6968f1ff799145d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowMenuButtonPosition 4984f7f149daa50862c9e475a1f1a62105bf77317f6f6c2e27c4e014cc996065 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowMinSize 84eccdb1d8a60047d0d7668cd57d6493de7b6358ac92e5dd2f8e46c30fa17f95 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowPadding ef962e775a52719d137d321d25622249ff259b59e1d4d9f59dc2741688999254 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowRounding a6963a4bc37702cdecbd08a1dcbe48c129d04d8f8408b57dc7aa9d043da275e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyle.WindowTitleAlign 1e8ae849e4b63afeb6813a9f03f2e70ebe1e7c938d67769bb78886567d560059 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyleMod.AnonymousField1 dba441237b3a41c93a73d345587e691238fe47ede8c7e09f514e93499c0e08a6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyleMod.VarIdx 7f3d79333a14eb9a025c1bda702f01c655c0f5319f916ed8c045fee0556479a2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupFloat 157fa80aa7859612b664064130099b68268baa0508a240447ca02256898cf96f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupInt 29125f1e76857687aac8b346934cfa1aacde84a10a7dcbfb1604f8463c8910c3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.BackupCursorPos 7cdad7b2c0f84f4b2e364c2dbe250886a3618358d20c7a6856fb8eea79ab54d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.BarRect 90cf0fabfcedeefc4f40271f151a59e7ec28461af0fda0675c0fa4f11e4b2ad6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.BarRectPrevWidth 2b66d5642ae57bb156d7a1cebd1363a2fb23875a6414e2a95f3ee9973b05c837 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.BeginCount 22e04a69e852317ce7d630727a3d8a9b54f410bb30e5361db9d10b1410bfeaea -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.CurrFrameVisible 948560b7d62eacaf27cbfd7b8b01622e0fd03ed82c6e5eb876a0e02645828eca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.CurrTabsContentsHeight 40cff52292869d038ec579440e0bc30a348ea0667a07e6b4e3fec94ab60bba19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.Flags 8af2bfc68bc5eabca6d5633371a61bc46ab5f22f1e253dca3dd915483f620be0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.FramePadding 7e96bc50cfb205411ba4ff6b17e76924f1d28006ddad9b636dcc6be790bd9a72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.Id 15ee9ec5afaf0f417c6da6e9b3ec9d66132469137090faa0ec4cda7010c7f457 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ItemSpacingY 858d203ee5e2a99631fd6b78a45cb39579f61a2722179e4b6b77766bc3bc3a45 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.LastTabItemIdx b7f2ebe38a1b624f918268711abf9ff97191154a0a488e144c006277401e9220 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.NextScrollToTabId bd8cd12d83ebf93976f28389e850e676802a19ee78d1794a1a74ff4842967fc7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.NextSelectedTabId 627a1524f188c343eb7218f2aaa4f7cd48b418da04eb9d6af9bfcd6c0fa05329 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.PrevFrameVisible c66d8db858d270bd1d3e563417577ea857254a1ab02e142e983e8f2214d46854 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.PrevTabsContentsHeight 04ae3f556bb783581c4261f22a7074b047982ad7f331b682a0baa0e96789cd89 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ReorderRequestOffset 747ee288287397ff7c44dfdb678b49759e7e4452b363611f1d09cb18ec31c763 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ReorderRequestTabId 0dd3f78e916034f4c654abc26badb62e66bb47560bd00d5d9e8f859bb3c67851 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollButtonEnabled 0f7eaca3d6a3fa3d089476d5cd8368dd9fa778db769dfcbb8042f52073b3de44 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingAnim c356b6c225a504b5b97f0b6845d604d3dcfaeb1afeb9df030ec105c79f750db3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingRectMaxX 815c76c0e9fb686676ab25c67f1384ae4f92b03cd9bf21b8a018a662ca06af00 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingRectMinX 6e879315f7fafcb6db096a472581d3d967b63156aa6946f845665c0751ce70b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingSpeed 7cb87eba75a2900d90a6680462579d6631eea590901034c37d51ec6f636f6fef -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingTarget 425dc1a4c2e76378989b2f08ae8a5f3d86a5d50969a6d4d0fa6e28f73dbbe31b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingTargetDistToVisibility 4a5bb2c255332ae5da9a1abfe7ec883547300aeb8f34219e89ad782ab1407628 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.SelectedTabId 91a2c522c6f88116eb58ef51dcd17e1162e71c22359d318fabc61e5e5ddcad74 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.SeparatorMaxX b1fbfd0594ac4243a8cd9b4517126422efc0d92c307cf1eb63649899dbdae90b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.SeparatorMinX 113a3484917f8cfd54f405a359164f7243c74885e6e728dc7f2209db0c3487fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.Tabs dd4d0ba8521f365c61df74fb26859759dc18d7e2384a67d9f7cdd13c03e19974 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.TabsActiveCount 32aec1c7392e7eb1d9a3b0d13a326c03dd52bb6f3fbe017385f47d4c7d4333cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.TabsAddedNew 5f6735f7bf10b68c9c801164c674f7a383a48a82885d8cfc5b970c580918888d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.TabsNames f074ff00f93fdee728ebe43a502b4d7abd8425a582a817af78475a46bff408a5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.VisibleTabId c054d8b124ce7d649283082478b6b628e1dc07354629ea1e9882cbdf816c34b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.VisibleTabWasSubmitted ece864a0cebaed6d0a30f74e0d9ede4df7d2435034cdf765b6a1f32f426e8132 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.WantLayout f74d32e3fe45c71267a26f84ff50000bceea4ca3702fd33640b30ae077eed233 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.WidthAllTabs 92de6d4afbd95b20b960dc53c96a625bdc4336997c745f6e69e3ec29e17d6ebe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.WidthAllTabsIdeal 2dc5aa105c99b92d022b740899bf33164ad48ae29758f92ae8d27d99a80250f7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabBar.Window eadcdf1556860a7ea9f1ada8f29e9e3f27f82f3f722481c95affaa790761470d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.BeginOrder c63e8e138e69c4bfa6605cde5929091ddd47d0020e9fac6cbd63c454528d3fc2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.ContentWidth 76c3f891b954dfd4a70b522ac5bd750475562731b4ad10397b4a558ec3210f85 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.Flags 8d85f77a74599e006eb08830d3898d387e6ff45a932697737f9b2e428baea846 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.Id d7d9c697ff4c3e9c9896fc8e6305208267cbcbfe976a2c3891a12bbc6924502e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.IndexDuringLayout 38f33a60aa1d31ffe30888a64c8068f051ed6da7f90074eba79fcbc7f36e61c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.LastFrameSelected e35856b882c78add535d257a4078226b3c43713df60af2423476aba448e6854e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.LastFrameVisible b10ce50008cf010dc43745ff8f68a9477c6ccd510e0429a58f5b3e212481583c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.NameOffset cbf971d532a83be81edb82cab11ccac56fc27a92255fffd1cfdb8b3469adbb17 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.Offset fb9ec04d7ccc39822703b17c94591330cfb1d32af729d6da498de6d7c4d9d0dc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.RequestedWidth c5a22420e69b62fade11cffb3bedb4d533286c9a668ac34a283ca028b9729569 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.WantClose 6942763e0cfb3b6653d2edaf8b02d4024ecf1db07be974707b924cb3d5268415 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.Width c53a6481f482589487255d07bc7a9271a04a05cb4acf3ddb624a12dc566b7b7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTabItem.Window 49e08e0ca127d941bd53df934104b5b9fa0e7b6dd6aacd77dab3d64e223749c1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersCount 8616fff943c047eb9fa67b08b6dac2992bd21414c62cd616852cd4f3b4dbd548 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersHeight 66c6dfa6ee15e2bf6106e580ac5ecdd13c476458ed914b107fdabf326f6f0cfd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersSlope 117ecfe47c4c0ccfa91791d4ba4d88c6b76d9187a18d3345fc0e8b9041fe2863 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.AutoFitSingleColumn 2faf066ab290530b8cab656abaa63baab94aa282f8e301470e91c0898242a580 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Bg0ClipRectForDrawCmd 93327893a44d6d898d5d493752235ec78f434192f469843c254d4619ff90e76c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Bg2ClipRectForDrawCmd 2ec90991da26bf7d3070e9e46ae0233d3e8d525d851266473ef61e3730b10595 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Bg2DrawChannelCurrent 092e33b1258e853e5483b9d1143b82cbe77911ccc0cc25206d0478857e2541b2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Bg2DrawChannelUnfrozen cd3a8e9854f75eec221ed1e6303849f6ce584730dd1913edb51e0cce283e8da2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.BgClipRect 93e73fece922690266b68ce322d8bee8a9cf27298b0423be6555a0891285445f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.BorderColorLight 86441e32d9f125af56c8582b437bb62ffbf7c1bdab442e52dd40280b57a1bb7a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.BorderColorStrong 2f39819a22b66d19098de2b4aab824d3cc50f7677933e32a03b4b6e948c4687d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.BorderX1 0d2a764e093614fdb4142454d999e36b07573d0fe8c8774da0134affabaa620c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.BorderX2 e59d296d8f045a06cb53b7a30d68a4f641548507151278aeb18360e8b1f660a0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.CellPaddingX 3c967341c81c3463cf057e6a1dc2ec8442ce99996c5572c50c8e37ab02e3f786 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.CellSpacingX1 2e06fe31d94a7fcbdf8a9d1818b1006e8c0b58db2441e557bab3aa08c9fcb166 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.CellSpacingX2 ca885e838c1497f1eba96546d41a48ac2ff1a46cecfbc1f5cdbc6f52f7b2b2c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Columns ac81374deb6920cfe04699bc511132ac40ea28b25caca0529f6525644d07ca92 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsAutoFitWidth 748dd853ae7c64b53d68153482905f0f7ab898c86888fd26471d6e4b315682d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsCount bfdf7e78156fb71198e4d6ccff0f658e499e073fa6f3bd96215f0ba0f55ba48f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsEnabledCount c85f52dd997b17b76454be034bf2d62e9fe9de2374d6db269e5e2a1647dc7139 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsEnabledFixedCount 32591bc4ea4913d891ee94edbf3fc599d303e4f7560ca6861a0fe8f927b7f2c5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsGivenWidth 410d9644fc07a3ade2236349ed88c5802a35e49da30e2a87d239246249593d12 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsNames 0868209cf8dc39f69462d45eaa157fdab434bb142a2f67e760b1827d6a4baad2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ColumnsStretchSumWeights 5b2a2f0189778c7005ac15f3ca27ddb9c35dbe3eab063892bcf9fb25496fa17e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ContextPopupColumn cc0f926ecd428e28303236f7c01fea9d7e35512c9987bf2a46308d28a78fc538 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.CurrentColumn a0ac078fabaaf13b660c9bc73054da1e2b396cdb3c15f8630771599dcea353ee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.CurrentRow 5e8b8f397131dd809d37d3e06bbefb5ed04603b5267642fb5330ce831ed96968 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.DeclColumnsCount 6299e378d1b6c0c9b5afebd9fdf46b2de6d76f5a8b734ce356106380525ce44b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.DisableDefaultContextMenu f9d0b1983cbc0fc9f0cbd18c4d4e1d3e10f9545b90bde335419e59fc4c21bb94 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.DisplayOrderToIndex d9e0864d6f512c942411479ffccc0011a5c0304b20f4be842c59bb8f0ebbc473 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.DrawSplitter 8d706530503e05e24abbdf388b72535170d2f5f24be5b96b2dc1ba64bd799209 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.DummyDrawChannel 4d9e0158b49fb2f4e4891a7cf4801d5366d5c1b627635305c7e1783054616cff -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.EnabledMaskByDisplayOrder dacb949844d52fecda0b27a4765f7435408caa10318b10f2372a02f0cf206223 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.EnabledMaskByIndex 35d1e683e2dc75932ae63548ff4f3f9fcbb20ec98837795a1841a3e1287cecd3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Flags 0bd9e71770dd909fe2f8f41dd96ec04a72d2ef44122ed8b7afeb9463a93f8b07 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.FreezeColumnsCount cb0c655a2d95d751135d63a7af9acf89041685f0f000ac919d4902f5161b84c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.FreezeColumnsRequest c29912a0f6c9161b48f39bd7b7a4e00326445c105416240e881c978164b9a2b9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.FreezeRowsCount dbc37d23112492ed7d9d51456be8704460fce9212f66297009f141916480b28a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.FreezeRowsRequest 97b9911a029084831315ba118e18c0860619e605d533607a6d4dff13bca3a68d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HasScrollbarYCurr 6a051b10911dc18def56787d15bb48c4a5dd1a63b361c23f06b0327a5e5d462a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HasScrollbarYPrev bc0d51ba9d4f9b92b9014c00e1f6f4f307e6974d060d32e863ea3837ec1c6548 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HeldHeaderColumn 3bd730a71d8a676c71facc48fd8ea9c37f0f6a7f86a67971730f4b5fe35ad3d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HighlightColumnHeader 4124763188a6b9a4878c5cf43b20554da158cfad93b6ae6b0ebc0174d4bd27f5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HostBackupInnerClipRect 7d66ae470207db98e408597e5e751df2ef700437c4685c8036072ece4c2531d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HostClipRect 3e70cd6683ed6288fe9d05310f93fc55bbfc3f4c327a30855f84fe893e48283b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HostIndentX b5bb0625ae76b082137e3617efa8226bc2d33764b199b0152ba28729c5bac0aa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HostSkipItems 50bbd07aa62b8e7bc4d6c5208798b1dc61bb5dbdf22ab7ca52bf99bcccaa94e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HoveredColumnBody c6818339099cab3ed575bf525102fc85f09dd66711e364979cd5ef81f5058f35 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.HoveredColumnBorder ab7eb21a68b25e4155a71265034cd65ab754e9ce3c1bd76ba447f98db19213bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.Id b669d6131514e9783f14eab48e856126863c74b75aa744eca613f07925bc9d9e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InnerClipRect 63c20fad9a9ef32c842794a1fa20a97f2e95e66659d8aac217b27cf85bae1ed8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InnerRect 9f1a3458500c0205aa696e17d77fab0f8e79d6946c4d879aab9bcfddd1465026 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InnerWidth cf15edc87bbb740459262588a5e25f0f2cd7478fe5da4b4cbcca3e7651820b77 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InnerWindow 535bc4accf870cad6de9bcab15093d5913aa524592dd642b0958e0aebcf5106e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InstanceCurrent 3fc9946f24336287dbe5e572a7e51ea41ad96b1550eb382f394d91f3e5beac54 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InstanceDataExtra 008a613df42451210f5d6afcce240ba4d1dc90a29407ed2b7eb57cc29ba2a54a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InstanceDataFirst 2906dea7905447eba62ede7654db160a829ec6b5c39e86321a8ea2ade5149cd1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.InstanceInteracted 107f655ea6f81cb3e5315f9d7a32368296489b7ce7db90a434fb73f42337296e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsActiveIdAliveBeforeTable eed4f464f68e3d1fb45b204cd2c3d860d6469bfa5933692ef0571d6b1d3fb4ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsActiveIdInTable 784ae06b732782c96383ab7bf937d0351a08485327099ed66fcdde89f10b29d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsContextPopupOpen 4ce8276a44ebcfab8c57bebdc873c9394bd638149a824160061f5b6aa38b0402 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsDefaultDisplayOrder 570d17ae5623061aa4a039ae1411bb4cd0dcb5379c2c2c71812e58c3cec60c78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsDefaultSizingPolicy 0fb7e4304bfa890ab5c95215fbf28a3c3bb083d82b027ae9e6d73f514cb0fbfe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsInitializing ae71f4d1575351f8e27fb96401b5b1c8ff892731ec0e8866e1b867d8f582995c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsInsideRow 8acca4ace0e364802290fcddd6aa8cac3e51a9fa69df702c03860558f8b78f5b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsLayoutLocked 7e9b02f920e1d55d02351b0c53213183def61a87d0c5637076dec77e5dd2cc97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsResetAllRequest 25c8e6208415a5833ae8cb735f9705efdcba171307f3cd9a095f51bafdeafef1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsResetDisplayOrderRequest bdaedb0cff0019f0313ebc92c471c976902e6a32ffe7b344ae633413e86d52a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsSettingsDirty 5e0617cdae345169cf681bd3b770a465d92eb33b9609e58a48b04175abc0660b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsSettingsRequestLoad 21e54ea14018da238ae8c2cd777894c7626190a1a7cdba7e42084f633dc35efe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsSortSpecsDirty 981ce28a590e9872a75b7c31f066b65981a307480ff253081a5af1407a67425c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsUnfrozenRows 89f96fce5f34d427a936d0b125e3451b050cbcb0ae17f5de13f920d3a86b3e50 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.IsUsingHeaders 64d75f18d1b1a79c292f41ee9aa0ccabe4637cdb9af40bc020c693ac2d9c4108 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.LastFrameActive f304473b4fe0141fd6bd056be0de458ea6d2440bb6786fcbd562d1c93ff389c0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.LastResizedColumn 3ff0240542ab1982aa2fbf46190776e814cc5d30390ecb17ab353b388c0817fa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.LeftMostEnabledColumn b51db2ddbe96ed214fd9a219732def25388c9da7ebed495331225ee155cb1f0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.LeftMostStretchedColumn 1feb545a9acd6255ebae2e329e535559a29ecfe241e572205aa911132f857d7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.MemoryCompacted 238d8194a0665d5f30018b42b3096146cd2690a086872e9d37805e7b8d4e531e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.MinColumnWidth 3f7288f07793977fef6a1499c61bfb235fedf6f7d6fc586ed4e92121fac08ace -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.NavLayer a95f699ced91df8293df4df36650bc39eae85f120dc449eefe7b0a1e81c4787c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.OuterPaddingX a53d517a96be82d52a511260c36f8ffb7666a52667cef925713fb60a3e209439 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.OuterRect 49590dd15c93a9f232aa471b11d847251edf7c31b1a20912447661dbddecdc4b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.OuterWindow cde6b251eb5d1b9e2afede85d224a59f43201047b3bba7eb995155194b1f5536 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RawData c73981de065055930d3b5e1870e9ab83d84d78cfc526f092a044342ebc71a146 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RefScale 8a9b77575036a89a6322589bfb0dbe73950a0cab3d9a0a587bac8aca0b851650 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ReorderColumn 7a0e9da4752ba8c4a13874a7b63cf203a362e286575341e8bb06234bf10742b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ReorderColumnDir a5e479c6044cf1091905b752dfd1848b965b7925ac679f9f18b78c72b1690e45 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ResizeLockMinContentsX2 000524ccc3f54c2b13dca906d1332e77117454c713031cffacb079115cc0af9c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ResizedColumn c0364ca44f9cf9dbec4967e58102c948f9b9f664461d01a4b2367a3a84507ec7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.ResizedColumnNextWidth 530bb7679e157a181c5608e09c761091f62e25d9a2aea0268335afa18b70461f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RightMostEnabledColumn 8b45b8363645e1307a7a26a358a5eebd7f8897ae3a0f2da5328fa2cd8dba665d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RightMostStretchedColumn 6b1e4bbeccff2141c9580eda0f300430df9075b9007911d1d7b82aee2c1fd244 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowBgColor 2126c577612cc9cd4f143e688f45eed969ce1a2e3305489b231adee935e7e610 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowBgColorCounter d675ebf21971d208113d7ea31899f5cc4b127045ecb6f7b747bd60f6dd03d7e5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowCellData ee3a12a3711b90127db72eeeb159794cd712721f062131a3eaff8652112b6a46 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowCellDataCurrent 70cabadabb1d4d74dc84522eacf672116c1cd24f9f073efe506d845c159b3cf1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowCellPaddingY 593cd95c8b65eadad6efce80356d565c485d2986de0fa53107d4f43ccee0aab8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowIndentOffsetX bb1ea1fa11d1402aa0490fa660e661aa8d1be1f383a713e9718ff607df34101d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowMinHeight bbb666504a16f1d123d29a5df306a594cae616e6ff7ded031de3218b8d179cb4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowPosY1 7bb50c35958f339b0b18612d01901190a6cb7dd47cc6915c53a81a25300999d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowPosY2 164436733f8d58289231d6acd830a1532a419518605e212e19dac6d159ccf80f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.RowTextBaseline e82fc5001c4d55504306da8c048b8e0f01f4323f210c0d5155fa64a2162e30ad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SettingsLoadedFlags 26b4e0d3db7201f3951b516daff790ee6302b042be7791d64a8936ede8f10029 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SettingsOffset e71b53d909916c031dafccaf2e000292a18fc0fef7dc019b35e4a0572acb38f0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SortSpecs 6281132f348046dafce89e01aa8b872b7667a03b20124f13a7f7c6bd4843e6fd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SortSpecsCount 7808ce677a8df31a2014d2c9f3cae1cda2834d7ab5a13e54b55b402744a3f617 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SortSpecsMulti 3519431eb00b73297dde776a2e1deec4eb5bae4fbd7c9a9d2ad167ff711b1c0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.SortSpecsSingle 08a99aa84bd552e41e16cc2893cd405d070ccc5286720b25d422108cae39111c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.TempData e8dbeba9308737fe25fe85754fa671cd056928e65a241f7108d86b83c56d1e15 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.VisibleMaskByIndex d8d084d97f9ede0602e8fabfcd08c774c16f33ba35354f26f1317d43eb107800 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTable.WorkRect eee3675ea68c0ce91039b091a30a61c8686ed4a1cad73654034c654d3f5a0d6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableCellData.BgColor 95b96f299e733a0515d275bd14992320b6a164afcacc201c8f1d050f8382d137 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableCellData.Column 435db529b259d9cfeb5ec5db204584b5a6cf403e833e2340caa05dd0157ab037 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.AutoFitQueue 08c29f6bee9b086879fa325150891144372b7512bb3089b05274562c350f088b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.CannotSkipItemsQueue eb9f6c1b2bd33926b2aba9819296df53c71391ae9df70615bd3e768b1037dc29 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ClipRect 6c6cdb250e93f89b530a47684466f108ce31f29841751cdb3268ab105334c45c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXFrozen 7f65ca254a827f9f4e566269c12416356b5752d4e2a7c36a768b12fed8c8e599 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXHeadersIdeal dec4c0265ac611834fedb5eeb2b3a0d1872fafada2d1b8bd69b8e81193567dba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXHeadersUsed 05b92756ec6b3ca80b5218984c6d45778a8cd640ab91f9d1d5aa8705bb792ea2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXUnfrozen 13fbfdd573035eb3613b0c8d6ddc9b4059eff7fd71e713d1c67f084a19efbb14 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.DisplayOrder da02fc02d304e733e48ca2dc21fbc3f5f0b025ef473a468603a8ad08b5c22dce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelCurrent 9ee56e3c24ea7bc38f7add3572adf90e3069d1ea88a1da0afec316f8d1bee597 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelFrozen 45e2ee1d7d2068e0f1477e3f7c76d3eb7b4a14ad46149d4d169cad0292f6efc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelUnfrozen 55ccc1e51087405eb30cb34672bba6fa2acc702f28553cb2aa9368b209c08c91 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.Flags a2e367a5196196158341a7df78d3313ff38b6ffa5e883e8682246a95655862e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IndexWithinEnabledSet 487c3f58216b50e53b5624183971e2b1cf3b83bb6d09d18c8a5910a7c1c4cfce -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.InitStretchWeightOrWidth 91b2223b32a7e12d22eff6367aaebcae30e43a1636e7c228ce5896b7c87272d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsEnabled dabfa06f1acd29d9de0be2a32d74ab5dd0cd01109618b16eeaea7d9f6da1dfc2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsPreserveWidthAuto 6f7f28c513352fadf69cc3508614a3069ca643fd700cba48726ec46ae225b638 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsRequestOutput 5c91140729a705b78fb8c1b9e6cf135b538aab5f240721a9de6c48c5f96dea17 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsSkipItems 9792ede3967ad5a0b8da37527bcda3fdb031eb39d9aa873cf7729e6371630307 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsUserEnabled 04732c9c52cf44c7dc93f77eaca5d4c463772b04c04307983526e67956305278 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsUserEnabledNextFrame 1681d39188815ea1ad6047bd5000ea92620b3260e31682b1b4682c98e44aaed9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsVisibleX 07a30200898d828bce4d32ef0376e1fba57e2ac95af14d77e4278f34489839ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.IsVisibleY 18a00e4c310711832d1abd2d7b64376881481ce5e8687713c87dac1a9f48f8e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.ItemWidth 93134c89efd50f0e95412f3ddb8b37ad87fff24c76378561828071b43ee2b5f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.MaxX 8d8f3c17371fb7c53da10f5ab5e96ebb8e4c2ad6d1b6668f8bd6832bec1581bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.MinX 853b8be3e32d88c2e6eafea43c0f0dd8910677d7d073443fd89ec1b77a07e33b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.NameOffset d0148dac7903995eb0d997a2aa549ac525a2ee8f998be901e40675b113467f16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.NavLayerCurrent aa921e111ca8b1aeee7cd79909cbf4eb9453d20b5d6cce4361afd119b1c3c455 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.NextEnabledColumn 90e818e8c2cb462e416002adf84899fc68b4ae0cbd81b8cff4b22686e92afda5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.PrevEnabledColumn 5bf690d3443f8c49b7a1682069d833cec4478dcd0dabded7773a17670076388f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailList add227029a68a69dfe96dd0ab78ec63e0d281e92c14d116952b98bac487fd616 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.SortOrder 879e56cd1614403a28e6a2270e62d9cc6d216ce9a180c511ccf3fcf15c7b216b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.StretchWeight de8c27ba849d15b8cad22cd1d0afcf051970dba4887ddfb0108956c13a0f32df -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.UserId 47bf5d7d594a345ef107d072e69bf785363dad9a4053b0c0bc7ee4e14513e976 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WidthAuto 1b16393685702944c811224a6e3020b9d4c874b29d6748a1b9bdb846ff52ccfd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WidthGiven b65be85c301485154125d7ae38cb86121b0cb73ba55b3508f4b36e013543f4de -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WidthMax bddd9d2dd7e8979fe172c89be3d1cce5d2ce484375892f91fd8f51ea63c86391 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WidthRequest 79a33cda65f4bc096d7cf8b9f367110a9be4df2186ee5f6919b0b38b80f1dbb4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WorkMaxX 14361f7c1e13b14e2774705eeceadc50d590d76a7496cc8ee0299134ced2b511 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumn.WorkMinX 986e43b3561b03557a531d1f7c3e0fed62dedcc242ef6d5cd817f665b6f5a166 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.ColumnIndex 88485ae4b8a6e937e5a879cd2d111fb02d3c28c2a62cc47cee32f957ffbf5633 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.ColumnUserId 788e3843475bc655c25dc0662ab2c41a1e0f9d4ab78b980d9e74b965417ef7d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.SortDirection d8c622e48986331aadf1f65aa89c0f878c0ec810b8c7b56021f4940ac97aa861 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.SortOrder d8df34c5c57b7d0a1e958e08dc935a90c5088b5e14805733c48c477264d631e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableHeaderData.BgColor0 2bee702c21136b5c571be5becc8adcfd9b4c0e82747b8d0c862577564fe02cfd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableHeaderData.BgColor1 340a37e02ef256c9cce4c351ec5017b867607fae2fc7c7ec14dae6bbfaf3b462 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableHeaderData.Index 9ebca3fee026624d7543d480f001c6bbc93e4149d5ed38670ecf23b65f630c4e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableHeaderData.TextColor 6f67a2d5456c3358c9f760ccd5d9915607d19ce7e0aa854bfc73965d1fe27af2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.HoveredRowLast 1437e08b024fd95964653697056cf935cae55a8da9f98b6bdd7e8ca9e8f98b69 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.HoveredRowNext d8d16c18dbc876563e0b2b0d2c7b24409da3e8b709b4c18ef36044be88e34149 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastFrozenHeight 7f782e1144e627dc5a2c159a5b70d8634689ae536aab5557fea5bed0c04294ea -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastOuterHeight a72eb6c2c281848241635395033607eb6ae9617a060308e7a1b739abbabbff8a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastTopHeadersRowHeight 8ea5a292b1443cf69e26af2ef48ee30c98449cfaeddd3349c09cbfc07a03f392 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableInstanceData.TableInstanceId 3c2407d63a644486feb9e233dc22a9fd628e4b3d5f112868ef801b0be60a84ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableSortSpecs.Specs 74e8cc4db8afeee557aad4e83fc6a263207afde017653490d9edae93928d2c7a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableSortSpecs.SpecsCount 86ce032c0d34add9aee7106b1edcc3b8e71bc04e6fe768995292e60658fc092c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableSortSpecs.SpecsDirty 33e6476d60c134484ea1121fddad11e8bf3e745b2e53fbc0ca98f47a6d96901b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.AngledHeadersExtraWidth 088eb275c4757fc785e10dd7baeb4803fb18d878fd8136a0e4e0bcae578542cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.AngledHeadersRequests 3100fe9537b9ac441194e06107569f3e44305b865f9cfc3159d1ad85713685b8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.DrawSplitter e7dda262030ecbe9d265a3b1c94b4bc9355612298ee1dfcf59ffa472f4775f91 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupColumnsOffset 0b7897239f7fbc91aca482f7e4305c0e0bf9f6cedbf5b260192a51efd360eb40 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupCurrLineSize 4fbcc2f9df07b8ed2ba6768b5606b48bed054fc1671259a76c9ff264d209962a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupCursorMaxPos f9db99e29c014aefaa5a7c7d3a942040bf43c178c9b374828da82428088b6cdb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupItemWidth 2b2045ce7a33fb6bb8ad50c5ba202286144082bfed5a4ec5ac385b86c24a881a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupItemWidthStackSize 40f22acb2a3816bd005d9a79c150c928416576b1348c40124aa2d6b68bdddee1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupParentWorkRect dbb7d84c8ce0730359ccd66cb7c76bab3e88842172c6df35d9da253957eb694b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupPrevLineSize 28c45b1377b1ddc9ac94c49df6175cefbf42d43e62d47ce5affac280a78ca24f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupWorkRect e665f1584b83ac4bdf2c1e82028b985123ee115052a62babe213925680fe6b33 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.LastTimeActive 71b8d2439724a257b49d2c7607f476ecddb07c27a7634665f8057deb6abbfc34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.TableIndex 80ac2bc02f8209c7aba588f0b661f9d628956b6b870f064083d79bd7fc2c98bd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.UserOuterSize e7294c1299a0238452466fe2ef1b9f06f9697996546d7fd1a00d242949b31d52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTableTempData.WindowId 677a3db2beae6fae5a53dc03d601ecfbb2f32ae08016fc176d6af1ffa8d45d47 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTextBuffer.Buf 7763894b1cf56b84c9afe04fbbf57f41a2d995166f0f4323f0790b79000cfd5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTextIndex.EndOffset 41114a23c49a57e19e3409b0e5f7ff202d63683e307b0534cd851641b2f41579 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTextIndex.Offsets eef86fe6304738e30ad737e48e80837728759806ceed2c2c903d7c71b520c5cf -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesTableColumn 70d0cc03b530a9592d3c472970f782b423a03c15728ed184b9f5542527e5b42b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesToNodesY2 59aa9744ac6d10104da4ba0fa02848161abe9e3cb0cf9a608db30f2c2a3516d8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesX1 fa238cfd71b54cb2733cd305954216743c96f0fc18f6fd3e07e60024e6ffa567 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.Id 2aa42c22dbcf5ca37820247b2db3cc8b86944f9be393622fd4c32a943105224f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.ItemFlags c9860f0f4e05b6881a2425af51baf1020d8018fc1467019e096d87b5d575516f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.NavRect 4f8956a3e2772280a5b4b340aec1976089d21c3cb0794b2b8b81447b5553cc16 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.TreeFlags 33fe3c36d0a2d4eac443407d716e6f81ca1f573c0965790b8097e10946ace57f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.Flags 3cf6e2a9d3c61868e7a416aaa1248c6fa9610bf3af4b4086191472288c51b7aa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SearchBuffer 6ee742bb6ea392b32b9461b96b94ae31778fe7e4edf65de535d74cf234515a53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SearchBufferLen e5143fb16715af2f555dc69b130cbf8ffdb5359f49fc74c594ee180cc3c9de9f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SelectRequest 3027d28595083738b541a2bf49a6e5f96517cc0e19ba75fd5469bb4770d3c35f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SingleCharMode b79cd690bc257ee04ab990026f45500bbb5f1babacc1bde2d2f321de5fa11364 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SingleCharSize 83d479a5e47160c84bb50b22831fb160e34d4eed312b2dbc52fd080bb076a467 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.FocusScope 9ecf3ad3a2eaa81ca6e816727db0b10d7862645175a1be650bf06f99df33480a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.LastRequestFrame 33935f41dc21959b5c0bb85b88034bd23253513a97da22c503aaf08a1d68cd8f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.LastRequestTime d8af510151279b197d911289e4a79207503fb67f120170fc3dd09e4f086be2ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.Request 18bb239094fff49502175d4b229a018a784aea5892f2f55bef74f09211f405de -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.SearchBuffer 698208e8186af0058893227b7057fbabddeabbe604f3996683af7f43b1009abe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiTypingSelectState.SingleCharModeLock 85cfa17337a0af4ca11b47a95bb3916e0b9de7de691c9f74bc40c93c71cc3413 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.DpiScale afce98192f6536bd63e2329d36f56d3d97b1d725216e21ff62d70efca9b20c5e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.DrawData 9095627e0d3ef78c2f98a38d189d7d5faee125adf7d9fb2e65516b0600cc9cc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.Flags ae2619f253eb9caf00426cf9feb954f89fe75eaf2facd25817484db7d7811ecd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.FramebufferScale 1aaf94adfaa874dd40b034258b993d7ac18a9c7f6e216b6ba5eee8c1b5fb1f36 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.Id 79ddeb3eb88928bcf61aec17b433c32330c160b30487d1639654c820ca6cd934 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.ParentViewport 3aba3d7a8dc6a5140806f9a8c69523e6dfcc5d74d0cd773343cd8b7f3f309180 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.ParentViewportId 609c589f892416694a1d85e394dc15a9a5cf69ce58af3a88e29415309a8c37ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformHandle d1594fe1ec5991cd574284d19fd323bb835251b6878444f72e94b0061c8ae374 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformHandleRaw 290c887171c964d1b6ae50a74c1982bf3f81fc8f20db6851afa93cac462f7369 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestClose 7c1a3370f39d9454fa6d4201f002792f90f8f586aa2ebb992be48ce24b1eb2ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestMove 913c8f2c60651a39eab3694d384654cb2227ba0d944bf053b1f015cd16d944d6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestResize d3101850afc52cf637f1470a9e87f13c29fc4339e23d046178d1ce52110aaf8d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformUserData 47878ea16a36979cfe360405d0c85c83a0b8df2e24856ee3cd4e71d0ca311d44 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.PlatformWindowCreated fa979daf79ffdfbba3624d22c164c31975d52154cc8cea0891833bae663beeb6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.Pos b4c0dd4bcef2ade527c8882b7ab72dc68c40d76f915f0b414c588d692288987b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.RendererUserData 89970686a46e012b999011ff4fc991f338e8c0385551e9f2ae849aadd38b757a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.Size 35a38edd2564bedc12d4b3864ab1aa0127c81a5993a3f213be5808b10e511a92 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.WorkPos 33d28649d9d330b6b8df0b0297fb3b8b13058ff81db48c5f793e3344d51f5ffa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewport.WorkSize 22c1b9e7a75e8859f6f67588452e25f6b936242323f3995f1234287d58a0eea8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.Alpha 46b2f4c1274d66b2c45683fd0bad79a76ed68b13bde9314a869fb732bee00272 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.BgFgDrawLists 07299e56c5d9a418c38afa216997ca1bfc7f84fe655f177f292f7ddf430843ee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.BgFgDrawListsLastFrame 06dcffb7228f2147a15cf688ed6ce0bdf7f70bc3ab127de8c596fb1500782336 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.BuildWorkInsetMax db5359115b1ad68622e64f95676eca15c5995e53e7e5d1679b75f0671d87bb7f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.BuildWorkInsetMin 8228b1ab173ef07cd5ff6d8984602a0eafbc2699701c8453440076bda8620590 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.DrawDataBuilder 8a887b661f25336f012ee34209f07027b2b0e89f552a94617b35aec4b9e25914 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.DrawDataP c3f8d4bc1bbe46862ab2dd55b966d5ba8e7b4056fc694b0bfc0cf0629d1dcd7b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.Idx 73f4256e1ffeb850e0db7654754f4a43a19ecceb1c196cefea959e2652d4d6b2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.ImGuiViewport f45642a926cb54a670eda7719e21bc5b8664615b2a77adb6c51ad8b6017ec276 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastAlpha 49267621b768e1299fedd1c4c63fdde75889aadc65cde10a71408cc612c4a0ea -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastFocusedHadNavWindow 4b106e19e837fc8cd43770fc2becf347f0c295eef404d6e5b625fcdd11e6707e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastFocusedStampCount aa1050db2e478ad7d2373c4ec83e5d30c8cd8d3a0d57ce6767c6e27ce90f2309 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastFrameActive a7231d8992de184fca9361dce8fbcf9110d7ad52fd6e8db1a79c92222f35a56c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastNameHash 67331ecd588e968f1cdd026ef4d4f809edc474090b91b79cfd0e264b58b0ed28 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastPlatformPos da6f8160dab0c7e3f3f980671ffdcc8c98de2a8f79f2bd62933d3f40ca1e3a97 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastPlatformSize 694825837d620e5987e9e144647907fa114c8540a4332c2c9ac9db569ba79522 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastPos 3297e57466773d0ae72535cb9ecc3f76b4d8a3f21881886990c8faa8700f8e62 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastRendererSize 288e3fed0b29da6e642ca874a483c84200b9ec3b6114c4fb3c48628b4aa90608 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.LastSize f8fdc1d48e9a6c47626387b2e63d137442fad2972cdebf17445b053a21471302 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.PlatformMonitor 935426fd4dae95341d253612ad7119c8e27b5e1036f4e7030466fdfa5ee2450a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.Window 59af80f8cbdd4212ff5dd7325acd58badb3ff0960c6ae25bd5b58bec80fa7b9a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.WorkInsetMax 754ab26283fad398eb99d9474e7344d063507e2d822c43067075b5b3bb8d2c3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiViewportP.WorkInsetMin 1486ce478ca8c97d6674721abdb22792eb3617d4448cd95b64225b022db0bea2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Active 72b81563635c4d530970a8314e09bdd9a560b665dd43a6dc5a2fac4d6337a766 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Appearing 05e73e05782ed5ba15724f600e4ed94ad70c01a8ed8982201fe693c0b647e5ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.AutoFitFramesX 624db8ccd0f7e277fff27474b947d460e5c9ef8a35df7ce1ac5b163128d896bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.AutoFitFramesY 5feec08274818165c35a6f14cc24c137ce7bf28f589f56504072c18bedef7b72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.AutoFitOnlyGrows 0c6923fae048a4e0541232f41782ada54fb9041c5209006aae49181da4d2b9d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.AutoPosLastDirection f00b72067a440437c8db411e2a5d39d679e6ede0dfe9c49cdca5f35a0574b865 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.BeginCount 2fbc5e428760511b62a6575b8d3f10d0323ad2f8ced07dd9d25054a2cc619c69 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.BeginCountPreviousFrame e2bb45915cce9b26a9061b82da35e5579eff7b893f33f090c4d5aad556d045ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.BeginOrderWithinContext 11cd615040b7e7773d44cb69d279e69717f6123f9442db0d1a5356304fa60394 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.BeginOrderWithinParent 56a8e068911ced1f87b3791aceb6f23d9fa546dfbaf3f27cd0779e560abcabae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ChildFlags 8e1e70e5465a25f8d4374068050d36401ab1d54b5b10dde595551fddb1f673cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ChildId 39ec774d124f8732ea6cc38953a64c3eb1d73254ab3a0ea23847d3d67cf05b86 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ClipRect 1931a86c01ff83a95fcc6a9d1e9152e4b9e0a0aed2504b5cbabc73ca5b0d432e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Collapsed 19884822c54c25a84d87f3c45db7502ad8156033626eca5a6b391611a485cc72 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ColumnsStorage ff00d6fe93cbc0d26ff96b11cde58c94b8e6215f1113da41a0d5cf9f75cd6930 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ContentRegionRect f12f62938b2fd2dcc189b5b9e6e49ed09a2a56a13039b45b3a6421704c50a80c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ContentSize 0ac6ff9f0660962bbf3592a8881c73c47495baae58c3bcd3672635d3a5cd1345 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ContentSizeExplicit 93f4f40d12d624c49142a5915e1d6e63771e38739af1dc22a3181fbd14823175 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ContentSizeIdeal 83a8e51494699c9321daacb5cbe2266693584e18eb2d37bb5e6ce65cc87152bd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Ctx f2b169a38649b745ef17ce8c5d2c2e06a51fe0a3fd3d2a6442902b89e89fcb47 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Dc 2077d041cb9de684dcd605b7a5f02ba6a5d26eb0770d89c83b84000049f81565 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoInnerSizeX1 e07799e53b256625e419e300271e35e43c3beefe298f5d5a33ede7a2fb5b70a3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoInnerSizeY1 77817c4590c17339733d3ef1e407e8fe94a51eb1c0f66c18b0ef3ab3446c48cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeX1 41744ace657f52f7b4f6708c4ae5b497ed69d710f302f9214e3901742fcf06f2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeX2 d656f429cbdff4925ffc9b7b73c9532081be098e2592870b33309bd8d7bfc78a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeY1 83406a07743937fb2bfb3243330885a8c5446dda8b7d3e1fc9a31c9d77c0fa3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeY2 a4635cb73c899441490aa9e514281e2bef992a085ec816eb84389cadcadcdb9f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DisableInputsFrames 82e990a5ea1dc84f2673eeec33488b14187a8149bd757a5bf7bf4bb4346c2147 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DockId fbd9643a4c998aa6d03b195aece828f262858cfe31dec45f13002d5ed6e6c51e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DockNode f936e82ff0213526140738a9c358fb56d220012cfdbf01d5e3f49e6c9a1c7c5f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DockNodeAsHost 27c85156e11204ce56a7635c007d66ad1d50640b793e2ad261bf8cb4205b8539 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DockOrder 10c8d8f41a21ccfc0279b89ca97c9425a1a6e5bb967e031cb9db1f6a6dcf6a83 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DockStyle 8da9b9da38569a8600bc507fce0c24e1aeb9c26d2949af8adcc812f45993b95b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DrawList 1d120a2b07b20d28e08311f434b316bed3f50e97eeeae98732c281312abfdbd8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.DrawListInst 705bfc0dc233509715fc5e3548f275962198cb1b3cd725f1337b2e19ea6e3e58 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Flags d02bcb9cd511e52d50dd017f0742a41023f6df0c5c96bad5709198ca75e3ac06 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.FlagsPreviousFrame 8133267da0bf9c4e5907bf36f823f11782b85f45826f3deb8a7cc1f129fdb7e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.FocusOrder 29789415e5dab9336d4fc02c7adca71403af1364f2d68b7a32bebeb4d9403853 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.FontRefSize 384affd6a64fdaac8afa1c144fb93b11f0a61e914755e14a9fda9ed3af8dbc53 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.FontWindowScale 90790d5b8eabc3501d2c7df926826e5f981a5b8576bdf0ed9c6299af6b921260 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.FontWindowScaleParents 1b39bf083a2ad58957716b0e2db08a44019f74a39907425d75096ff1024e2ca8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HasCloseButton 99c460c257414aabfc5fae892ff3521e78a9ae56bbf1ed22ec85e5b76cb251e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Hidden 60b6fca5543811376892ce4c64f316e93a4f783093ec0746b3719db924db03eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesCanSkipItems df0ae0c0c126816701253f00c3e3281bf7b5554c8230e072385e2fe07be4eaac -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesCannotSkipItems 96b3b2822042a41ff759276a99c8115f94237911e72636c1365ae608740706f7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesForRenderOnly 83ebb2a5efe5e910f345220b75612c981a580ac09d9c9845ba0680f7247fa36e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HitTestHoleOffset 3e2e06db3546a3502f15df9a649dadf005f52b5919dcf19e72ba0fb0c5fbbe1a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.HitTestHoleSize c9b5c916e0e87c09dc6a642e79b539a68ea5b9bef3684b7a9698a04793772f64 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Id 09063e4baf9bd0c059ae0258747bb2b49c98003497772eea69f1eeeeabf8cd80 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.IdStack 1b2e221ef16d129132c39426746f65e6569f4263b03f39ff3c8a6c21d35a0b34 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.InnerClipRect d5718f53327bc2787678965156252ca4fc22854702dd4b663186c64ef48a9a99 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.InnerRect 6458faf59aa8ecd0f50bbf58e73bbec8f4138a719a8bca55b3f9c8ba4a6d3557 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.IsExplicitChild 3a14334c850b93b68546663b5c53f3693bdecc86a85c979d95afffdef4cd5aee -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.IsFallbackWindow 92999e7bc2a48d5f10492b2c37625ba54b7c0dba3cad1b6b39f3722b663daffe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.LastFrameActive 9b6597091fdfac85ac5f3e34b6bf8cdee9a70a38c2b07d9af2bcc67c397c52c9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.LastFrameJustFocused 9077a72f46b54017d17b36733fc82531ed917125c1752f7dba706a33001621fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.LastTimeActive 7ae499af0951b47f0247c54354c690699ca153b2d5932a95333c2e810350098c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.MemoryCompacted a74fe99159204f3079926a19cdfe5dfd74d99fe243d841f6e97d33aaa50f891a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.MemoryDrawListIdxCapacity d8d78f7f008ea43c7e323fbb95590742eed74f59ad11a5b269a8d95e2f6eea0a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.MemoryDrawListVtxCapacity 1ffd1964722324844c800c808eb12c51860fddad8d536c650ab4af27e60b7750 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.MenuBarHeight 9af1715e165a9fdef18122ae46e5bb9439309842b986dd94958b4c605d8b4800 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.MoveId d93171bd5adfe4dcee467bfe72321b3e796d3f964719cd6eac1f5ea3167ac889 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Name 4f71bf577a831f4a63fffb0dad5ad48e76b5a97d9167f6ab57af506dc70a9d5b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NameBufLen c8d25e0ded733eba4694f7c0bcdea174ee8bab12ab59a4f81493d5d8e398fdde -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NavLastChildNavWindow 10381d6beb37beed4a2437a16a29a182dc45fe41c002ec9231b4540e1f9d229b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NavLastIds ff7bada4bdadce19015f21276da652f8317532c38dc3c26d79e48d4b53fd71a2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NavPreferredScoringPosRel 83d2e6d72c1ccac30f1e96f54ef529cf6a125a5ad6c2e451bfc487358cabc153 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NavRectRel b69cafe70fb601c49d90e71a62ff1c5f3e766a8028f12d80a14d4e70db438067 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.NavRootFocusScopeId c5b13836d52f6730ccfc48e96449cbcb4c88b6f788e12c116d40cb83481d0921 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.OuterRectClipped 175b24bd445616e98f402effd3e0490006af6b01986221c139b1b318fe4a6da1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ParentWindow 5ecca48944b4dc614fe665e8eba24f991c60574b5520a4743eae90296c85e081 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ParentWindowForFocusRoute 35241e23f94ece71cc744892fe913f4599be151f24924d590ba434600e0493e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ParentWindowInBeginStack 68b394f93eacbab9e10bdf7eea60d76d6d24a7ef63d79aca036f41b3d841a136 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ParentWorkRect 5e3f39c291ee1f6059e70b1ed60e9083968390e2eb2fb6086c21f3929cc70294 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.PopupId 5a1afd6cdafa24a19f045fe3da2582532e3fb17f0784b7b00e575762321db331 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Pos 10f9423ceb719ed7e69be108f598d2dadfdaf9568bc980a356b1879987ecd004 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ResizeBorderHeld 12bd7606a85e25b52686f961e8e9f55fd80013bc93940e2ccafcd2dccc385e6f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ResizeBorderHovered b784d89719cd86fec495007fccba26bd320bc1f0a968a5403ebbbe76ad2910bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.RootWindow 42954bbb7ec872003dc9a402832eaa20f6e83041a448ce0df05c222ba2cd143b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.RootWindowDockTree 1e4b75289ac99a95c5953e0737fe59d92786561164dced9a34ca5cec7adf7338 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.RootWindowForNav f63f9ba919a77f59028c3b39f375f473008b596844705c8bed0caa4473f524d7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.RootWindowForTitleBarHighlight 951618df5200b595a7db3428c58020340c725d9410586d3127492b696343a636 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.RootWindowPopupTree 419e446f14c742119d3003ee60b8640fdd4e670c63dcda64cf9da90977429ef5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Scroll 88ddd1ec454cf9b3a9013b8dcb97be6109527277e118f2f423587eaec6d0f8b1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollMax 6f4af392cbcc71450aacc338208e08e0c72a7e092ff0db813d91cdbc1c889003 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollTarget 2b655cf9f8a7dbca2bc57090dd41e5207930895fcec3367b79ca794d31724c6a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollTargetCenterRatio a75ec8abd4fe9efc815cdd68039928bf2576f1513904cb17577c9888e5b80c6c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollTargetEdgeSnapDist 0b9680bb1e2912f5596f85184702d2ef27cd53fd9cbb042a8bb61995ba997c2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarSizes 52ce5d6bb9a7c36556889536a9a571192d12b54492482a3bc90323ba441a61e6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarX 97d212674011edfeaae9ae31bed146d4448aef49634fac48dcb8b8074fe79770 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarXStabilizeEnabled a4b56a0d0d652d7617661a9dc1cf7be9694f8e7be7074e6f444d665e4efb98ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarXStabilizeToggledHistory d5094819c67967b2b8cc4291d27f31c433b49822379492801a59f2fcf222d734 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarY 6c9b70d3aaab0ef8aabfe256d3913a119280c531f1b7f7d45212140387b70409 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosPivot fb01725f8910e15f7d22d51744edaca39ccd638ba21f0e22e9a690d02393e950 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosVal 33049360b149c1f6d780089b2a6dc856c41bd6ff0154f11936b1d4833af0b992 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SettingsOffset e63799b366365e3126ca0cda48d12126a4cb1bd1800faa205753cc7bd8a85e3b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Size 6979100d02e146e7073fc9e8af0be8049af33b9fb6b0c33698317c775f729e1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SizeFull cf0bb93b9257d4993a5036b65108605022377189263fd88388306c5239aa1223 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SkipItems b09a879be428b5bcd7245c32c2990e483e2a23da61d4ec6691165c9d0e18af92 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.SkipRefresh ad8cdb2f02e97084269edc1014ecac7b9e8fbf276e2d1011bdfa72dde32e4785 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.StateStorage 46bdfde4963183c871190e84649d6fe358248db759d477e9e0b41cc8fae1915f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.TabId bcb46d9da34f5a0607d68891baecf24496703f1fdab2ecef0f6046abbd9c3ac7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.TitleBarHeight 0165a9cc7e774458e3fa46a0fdc713b554d1d2b56785a6827bda1b952f71640b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.Viewport 5b86ba508a252b892775aac5dbf62a24cbb6935a141e3f87ee776f0647091e6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ViewportAllowPlatformMonitorExtend ba6e9bc010ea23cfb0a96b7c360fc42ad45e14444b2402596fb04be0a98045e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ViewportId e6455f359ac186a69a4bc8ffbcbb3c4b00ed026e67f0bbd2082b06ddc8a30759 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ViewportOwned bb22c197b054c5a506de306fb4f5103259ec9f313e7e320d80964e945ffb334f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.ViewportPos d11608c81fa90dcf6c7d8242822e00f08b998485b9ecc7b865ac31cd23d7bf2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WantCollapseToggle 96b83f7b16aaf9cb481a3896dff3209fac1166485d40497c0b2ee5d13d0fcdc1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WasActive 7df7c170eedc78a703c2127f83d98f9d83414cd992e519983ac7fb2f12a2de1c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WindowBorderSize f75f3dce5b7d3829725bb028aa4570932eef98ce2fe9b18b8a57b948fd3efd73 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WindowClass 7e0b81f8bcc0e41a7ae3a4cc13fda233e10b4b072afea8b3d096ea0c19e47169 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WindowPadding 62a3af3f5b68853ad4bf961c5ea1780c3a19ce5488ab0e6126976e3852092819 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WindowRounding beecbff839cda656caca4fd7b523949b473026d97b7ef0477f1394e16f028d94 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WorkRect 386ae16aa3f3ac6145aa1c2dc32f3f08bb70e0660d576816d69bf63d53b46afe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindow.WriteAccessed c4549f47a3eed23abaf524712c4221fcbccbdea6ddccb0e154d7e3e4bbf6a993 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.ClassId 29588b19c210f0ffd16c9a71f866e62845ad065ed95f24c58532b3c56ddb7681 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.DockNodeFlagsOverrideSet 2537eaa015047adb7f7b183d21084ef5b9918698b79c6650c0531a6b904f0c4d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.DockingAllowUnclassed 8da1a88b703ec7ae5118a00ac2eb840573832df2c0107412dc26276383c89b3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.DockingAlwaysTabBar efbf0e7df1101d3c0008dcd74ba18c308941e473004ef15a64d9ffdb6409b07e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.FocusRouteParentWindowId fb5272c93f4295756f65bab2dece6f01d300ceb79580ca577827bbbc3ca19f7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.ParentViewportId 133f69a2fa626136218a83c63f861d1c64423e3566abb08f0e03b9c187d9b731 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.TabItemFlagsOverrideSet d1ff59e8c28e8baf7355dfd78e2e06eeaccb18b96f52ecd441addbc8d14fde7e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.ViewportFlagsOverrideClear 71f02d81d83710e015a3312f29d7df0f09a86f169f1e50ea46db28ce6e4c994d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowClass.ViewportFlagsOverrideSet 54c79d03cfba6fc2e34e77258eb7158c252e5dc667d473171f79255425d8771b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowDockStyle.Colors 9ac5a92b19473271ee101941a9a4103e1dab2b7ac9d03629540182d45a9fcecc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowStackData.DisabledOverrideReenable 54b9ef44478b3b64cd236c331a46dd0f4076aaaf743027d6f103b877a63aec24 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup 7ea838f00f3fb2f19159432f99608c6246a282b472c211cbcaf7419fa998a40f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowStackData.ParentLastItemDataBackup 964d0885693195489e9741e241a36b440b6f4f1f71bab8530bb00c219213ded6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowStackData.StackSizesInBegin db74787140c66b44a79b273b0fefb165554ffe032b6a393cea453eb0ff03bd23 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowStackData.Window 45b9a24052db3bbf19a1d7f152fb0f6a829f0ff32df47daafdca6ebbfec0b738 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ChildItemStatusFlags 1f5d579b55eae810975993b35c685c08ff0a5af6545987b164d078573770c745 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ChildWindows 7f86bd8b1e3b6461d0a474b254a98257f1bbe0d8b8b546cb2bd310d2f7bcf407 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ColumnsOffset 5f06f184635154e5e36ba3e695adb1c0675d81e79d2c2d302a5a1ae87a86ade3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrLineSize fb16ff7d5e98100ec4e834043c5f8dc51f14bc1c23e83693c94a659741b77dd2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrLineTextBaseOffset 5fac4d901880291f309af98400c6717505ab5a8685b4341160a0bd8aa96b9215 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrentColumns 7ed1e6617fe367612aaed265dc9c3bfa5e177cbbe9a4ac58e39b0042fb7064ba -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrentTableIdx 9229c13f0e9824eb330123d2df2cbe4d3e6e4c2f2162bbd10b60ff996a61a216 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorMaxPos cf3d746d274ff0e0d22096fd1d7267e3bbf909cca522b44ac43e338d86d9ef7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorPos 1bbbda346edca2ebc4bde449fd827661f4126d11408be711fef689312e662c04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorPosPrevLine c2a4e28e750051e3438298dad73b49ae47b53a9679354466c8b4de86bc3afea0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorStartPos 99d479d2e9b48f9ca2dd1bcd93617320b8c375c9032cb362e4355c68b74b4ed9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorStartPosLossyness 0625499741e50c54874bc7159fd8ca2fe5a2fff760fe066e63a7be4cd0ed8405 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.DockTabItemRect 41f30926c02aa40d313a67c222f9a7891c054c0fd3aa1afa88c234c58b793f4a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.DockTabItemStatusFlags f792ca6278d421f42e4fe7492815cd1c7184b26129a86836950d668ad2d16e6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.GroupOffset 06b378123c79fca33cd8cd9ccc431a90c8a707bedcb52585036c869c0e6f25aa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.IdealMaxPos 43a176d1f72304607e555ae6a73270803189701515a7931f5501c7344472bbad -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.Indent 9228d539742e1bdb190ddea370f214069a91abde29c008c7cf6adb4c73acfdbc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.IsSameLine 9c7d0813e3fa2fa66a401ed72480f1a1397a2d0fc7214371d6c4bc567b85dab9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.IsSetPos d52fd0662623ab3fb917dffa30faf3204295a7f4cd4604c42262063f9a224109 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidth d3537c3de4080ebdbaf8e684e278ac957e7823b8879cf13e8aeddb90cf81faa9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidthDefault 1fb419a84960c0da7f1ad3d9b67310afdf05a8a60139c14a13ac9ea1204fcefe -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidthStack c5b76417ab4d73cfb9c624e20dcaab8c3cf03c7a08e16b5443456b439ff02824 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.LayoutType 0216e63200bcef2818c7305165c26caea4e74de429dfcc706a970fd11729b2aa -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuBarAppending 74e31db3247619da84befe43493996669084a3090ae4d39b602fd744ea0b1c2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuBarOffset ed2eabe80aa4d404bab75068131498cd0702525985a0efb730bfb92d6c69b1d2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuColumns cf22419d29f901fdcaf7800440d5601a3a49036641bc88d803a14759b6aa7c05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ModalDimBgColor 653aea361a1b502556b305aaeca8dea438e9578f7a6bb6784d3003376f850934 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavHideHighlightOneFrame 6af443d1ef23b06211579a127b6dd799f19971669587dd934f395c7522042a73 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavIsScrollPushableX 922a057e2de71a4ded5a4dbcddead1196d449b96be921568b507d6c69934c8d2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayerCurrent 5f74affddb59e4d3bfdfa2bc4a4dcca5ccdc76b3216ffe7de706185ce23ca34e -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayersActiveMask 5802941345c4f20f03f5b5dbe3bd9687b3a3400e6b9b2385d0c7a572d4639524 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayersActiveMaskNext 2846e53d1890ba511e4ca80cf4522fb6706fe5b4e66c2f65a4694540ebaa39b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.NavWindowHasScrollY a4cc86724a2a4e56417fe965a5473be4c3da8d309c72f55e60c8ee8ba82279c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.ParentLayoutType 1dbb66f6b8ad5e6fa35112bc4b8f9babb197787eff6f2505d92f5859006ea52f -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.PrevLineSize f08bee1de77156f96195765f2508cb94ebfeab3b4f06020a92adae7ebd228bbb -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.PrevLineTextBaseOffset 4f610d8db8497ec6090bfb44de099e8f797546324426682f2c6758386056e684 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.StateStorage 1fd2c8309ba827938d361555fe3bd2cb1efcace5db37548604d3e3dc69848547 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.TextWrapPos 0e6411c490422517cbafd04e6f45d6b5df51b4092afd3cf8afe61d47527276da -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.TextWrapPosStack 55e64aa7c350593b22841a8306fcb13de7f91212af5ec1b82fec279039151263 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeDepth ab8086c2520e63fa25b3a8b9be50a56a5ec14075925c48b01e2ba820cabe2700 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeHasStackDataDepthMask 7f5af4f4f8c00a59134d6b03afe99d6b32be26f3d465c790d9d184dbec354d6d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask b8a5b70ccc6927a7c5e34ea1de42853c10cce109d7edceb03d5f2191bf105afd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuiWindowTempData.WindowItemStatusFlags 569d4cbe3e33516aaf72a577b0d0d3dab27b7ecac8fa5a28f08a6f93364f1035 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmo.NativeLibraryName 702bc27fc0dc1e9fdf2b5004ec2ec4b0923e7ee5d3bf2dac8c3dbac420928e60 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Count 9abb494aa7940947b7e9ff327ec6e3f253670df14780e7145d21bc72f3261a0c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionX e64cdb2e0003bc0425af8cb8c118baf28808e98a214f16a2c30ee159f3d23541 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionY 5f158eba9239a420d753d57123298ffa1444a35b0fabadb10ecdd2dd97b4b82b -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.DirectionZ c7b44ea709130c0002c0af133113189befeb6e942384a84e09de04c0db9475d9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.HatchedAxisLines 97692ca2d3dae36dddab5310cd9166b7d8aede45a7e4e70cc3ca91b280dce14d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Inactive 46c4a991b4981ed8b4b2850f60ad33ab44a3c13c1f5e1665201d0bb0e4b0412a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneX 835f215122c8045ee628bd7faafb10c7036a90a378423484d1a228ec6cdb2b19 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneY e13c5f8d8dd34334d9c5367f8d0835c022f52c4adf46a961cd37ed5a8e337f76 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.PlaneZ 9ae6277bc086c1bb207fabbb45da6020900811c331340ff3c4a3fa6c8a2d33c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingBorder 8fc4433588ddb4a38ff6efb8a4a0acc7e58f209b3b09d09f2774a3955c92e2cd -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingFill b75bacaa2bee9fbdef627c4cea77ed4ce1a5e62be769d7fc2311b90c5ab4f9a0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.ScaleLine 36e41fe706453c626a1858f4d9277fc99b2b98b46205945b158d929d31508903 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Selection ec31b7c6a820ffb51e1456e3f1a6e584d5a75c6ce00824a08c9ae3b036a4bf28 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.Text 4247aa9a505e0e5fa780f742dc31aafa772d9a7a275645d5268913244e8160f9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.TextShadow 6ad1ca4ec140a3341efba90aa19118a763bc819423179814da9b7a27ae391a5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoColor.TranslationLine bd7be90bfb8d2ee5432ad681480336168923e4c89c69aa1c89e51cf37c332cf6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoContext.NativeLibraryName f2f520a45946a30484040154dc54b1a1302a427af3ebfd7efc9176e68a4bdb7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoMode.Local f09125d741b5a910d873be53eb382e0ce9e2d484d3314753cb0655cd49823a78 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoMode.World 4987275973c43c985f156758508c3578fab4400b8b69b9b61ddf1bfbad15c4a9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Bounds 50186e8d0d84362042c68d00f07f8a6d43d81c69517c31f364904058a858c659 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Rotate 22dcca70138acb085947442bc9db4d6a091c3e3932ea2db98bc34fb3c8c79de3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateScreen b591eea376526c45e43a9d91d08b6a7a7a3bd26f5c9967bc34dbf6e1e035f9b4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateX 5b7ead8f7a885699c57c78f87f00d1cd492cafe2a8e6fce4d1bf4409824807c3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateY 9af9b3f01a2ed53f773d7acda06a2ac62cb5e7d11f9faf44b446137feb2192fc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.RotateZ 24ee78358d546151d8d7ee70986cb9be6f0da09b3c173e76d779d8f4a84915c4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Scale 6bb306c35cd7dcd4049f69f1686a1006dd559c82e51da6928a0dac814e464c52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleX 043fffc74bff282f7b3ccff15229a07367d82b92e067a474a060216d16d189cc -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleXu 25683bf27265544ec21e165c486c822fda1c658c117b81d705655808ff968d99 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleY dbf122acf45943b5721aa116423e2d196ca63e14d7589dfcb69235ea8b127679 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleYu 92839c18ece00d2fec2ba98f56504058bc90691ad2aa1df98f563facf3b1fff9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZ 23eb68c7874059aa2cc4423af67ef495bcd3fa51076e0f7b52317fff44ef74f6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZu 539f592fa319d21645af44b72dd6244c344dd3db224c5bc3f2ee125007f44d42 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Scaleu dd623bbdfab04a4c218378124e40d451cc9e0a7831568a2f12657c4e8cf90fe0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Translate 65f4dc54e414e64b33595cd67ee51782e48b54118f89bf3ab7d42e83cc05839d -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateX e3dd626a5241d835592c3b78573af191043a1345d3b2d400a7a7025303276d02 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateY f15d8b4af077b7b3a44b18fc4a94f262b6f376b6f5abbcc5645eec8c155e3444 -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateZ 234fb2c6251e3e03895a80cd959814236746e508d583ab949046d3070d8d6e3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImGuizmoOperation.Universal 65c245af8ff9c48d39567a6ca5cb02648edcfac9620ea22f31c384479971bcf7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.AliveCount bb61dd45fb20666ae8c02eeb8ca36d79ad8d5be2f0be272bb8d96543c7532ef8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.Buf 5d895d8e6638f3848b46f59cfce552a8ecb8ba31fd4805a4acf6f22162d138bb -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.FreeIdx 11bd2fef114836fd7ec1da0aba3a5253a6ea8033fe5e5f1059281541208554a3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.Map 192cee0daf0c5e12d067b775d5140e5e216599c10956dd5b63efd98e9bfdc351 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTabBar.AliveCount ce7ed833902e5948f3422c5c6563a131f4b5f75cd953710a0e76828e95cec0ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTabBar.Buf 41d29bb8851a00fac4df6bc4a4ea42f6cd946b834489d36ef10530c0ecc21ede -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTabBar.FreeIdx 27722f5be7fda4e73c1a70beeee4a38d567a18fd50521d95c4676692e72c0450 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTabBar.Map 56e190df497b42d06d2b7b3030e48ba71fe840f094c04c0cd9d913ee2e31dd8f -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTable.AliveCount 65481d53345acf00abc793561acf2519c28a3a81b4e88cd59cb479ca3b826091 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTable.Buf 9ffc070eab53f62ca4dd769243a12e321aeac8b024859ee8ca3770b6ce760990 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTable.FreeIdx 2616f02fefce3c5b98b08bbe292cacbcf5239644746bed478980ad5be2a069e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImPoolImGuiTable.Map 0ecc498a618274a455af97809c3e3b3d6e52f80621563d69caee6b3d7bc35fcc -->
<!-- api: F:Inno.Native.ImGuizmo.ImRectC.Max e6580f76a45cb789f7b70461d3c152ddcd38eac22e486d3ea92a31e7cb45953d -->
<!-- api: F:Inno.Native.ImGuizmo.ImRectC.Min f44d9892cba9af01377779cb944a033fea6e99c6c66a38b7cc7b6ac1a84a654f -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableCellData.Data 10d0ae41bf190348054a14dc8e3520b2616477ff1e0ef314bdcbe8bbdb300a05 -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableCellData.DataEnd 16c77a76a6b46d49f1c5323fa2b8dedad0891f62ff0b96bffdc0538cb04e9bd8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableColumn.Data 72646f2a16c3e39fba6f44d366303819ae933975ee99ff0bddbe30a2c8a7f110 -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableColumn.DataEnd 22d5e56fca99a5696e3170e47c2696138f9ef0301c13b3b0ed88cb7ffc0c4017 -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx.Data 2b28b3d5affb71ef0c5d562093635bed5f13da7228a3bd93ca9736233a085340 -->
<!-- api: F:Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx.DataEnd c7ef2d01d20404950f2bccd2d49663dd9340050f62136dd2f7863e7e7b16ce6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Blocks 6a5a30635b17751480375b367f71a23650f299d377e7ae3886ffa384e9de541c -->
<!-- api: F:Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Capacity 91bd5e44d05e04239b8d9fa2e73f932e917db3f9fcef61d4f2b5e900a7437d37 -->
<!-- api: F:Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Size e39cd53d228b21a32e3d4ce3a5a24724148c7f3801c071e7e8b72b6daa40fa5a -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.BackendUserData 8cdf7eb9346b164b4b333915e9ff9bb10cdf47df027d7b01208d747fa17c64a4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.BytesPerPixel 06c746e329c4412a191b264a4871c6c995fb35e043e99b07a318f1f1d7d345bf -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Format 95f0758479e9ed10a84cd7238f729ae1f5aea40beb35844cf1b7c8d2bad0e023 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Height 63237f93bf03bcc925b781791f9010109bfbe469b8e9329f88c915c9b232c18a -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Pixels 8d33fe43b6a214806c65da3dadd4507349e1abb647d9c9cc420d15fa54315d71 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.RefCount 3c1c253d2b9cf41631d15f8da59b353e057a3bd089dab33bfbe15ebce5579046 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Status 6e19d254152dc7d9615ed6bea2e2600d8f5b0cc8b8e056f27660c33a36bf38d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.TexId b0f17d8c04baaeaa7a73f9218803122bc2f42c3ce0f0c8254fa9563172d41f1d -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.UniqueId e334e7f64505c566d2195841211b8fd7392bfb6afe78e2d1a40d8ae887e7f6ed -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.UnusedFrames 1e5fc4c9a341838f4497ae448b7a2aaf4aea9bbecf651c3bbec3df55c4230ce9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.UpdateRect c94994cc933f6a7f18c9334d7c1bc0299fb42ead1aec3f22fed3f82018d31730 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Updates f1ca42d5475d8fc428c179b400758d76118c767da17bfef06c38157b0fe60d2c -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.UseColors 0bfaa66eeb8b097df07e2f1653bab9fbcf51d771210cc53b434db00c9915a324 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.UsedRect 3824db9d59f4d89e8567c4173f2a15b23424af15e45e3b2cb55a8ce262d9bcc9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.WantDestroyNextFrame c4556d0e626dbb140c96ef5b64cf0181a151027ebe4c16668c70db6a3c632b09 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureData.Width 4230d5a68265cc0e137d1d136dee137599a80d1cd2f1fd6024ca13d8f4b08390 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureFormat.ImTextureFormatAlpha8 5eb5f209e111d3441d793e80a67c601934412d25dacc5839a30b3dbb9c1d65c7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureFormat.ImTextureFormatRgba32 758753ab5b0976641a5097136e6a0c08a2c97459c240fe765f9736f9ef1411a7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRect.H f5a86c0dbb11d5a61c393740f12c93774243327e2e6bfddf9aa777a364ec6183 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRect.W ba8caa890e8c2553e8d2cb642823feb5db3819b22747281de0a4f81ce3113020 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRect.X d60d3e0d6de75b18953d6edafaee49c31edffd87c91d83f3af8e323a4a47627a -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRect.Y 56fc417451d551b34e68e3b7ea010b570c38d3d90730979c50ed28f66308fe4e -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRefC.TexData 3609282b94d3496f66a86e99e44ce376bfbcf743e43b08ac4f4c38a1db6eaa81 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureRefC.TexId 038e498d672c52fc64fb8533fc6e815a998684525091d1bab499cbce6feca64f -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusDestroyed d9d53cc355bf24b26ed7d3960b1a96dfb876cc7d3371bdbf8261be1e907561ef -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusOk 61fa11d362c120f3cf22cf9bcbb022a03fdbb957df75161fddefa956d6291569 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantCreate a548daf1dca37adc708651b129a548ea1e7f2f5c4ced11b73b3e0e1bcf36e9ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantDestroy 7a04ee1ff5913f84f49c00541a177f695edc760add3e74dfd2893c42791a1594 -->
<!-- api: F:Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantUpdates 4401c6f995a7740a1a79f7759e22297a870a441f4ce9c8c18eb2d6add4574ba4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec1.X 34ae2c50fb496d0b0ff9ac6504354dcb5d869d419fbafbb725ca739a4ca5ac04 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2C.X 293456792b6be3cc5781ba88a7f631aa6b407078745835149575f4b84d5ce07c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2C.Y 3f0e964bc931566a7de9587a61f32318c6b41695f88f8683b029f1a20c7decef -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2iC.X c80b8d8141fedcddbb7f3163338b0e6499b7db6d813f8934ef470a543a17069a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2iC.Y fd21112f2725c8ab914ba2ffee2ae47ecb0d82e1f00b5b6a2ddb44b9980c74ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2ih.X 2e709ed46812fc918563d3bc8944e0948d3a8d3cc9db33a361dcb7b3560c70bd -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec2ih.Y a1d8210dab644fec4460eaf2953f227d345e600499fadad9eedf79e698ca97c8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec4C.W 205e65860c7289eefba90cb998a14eb817a0923cfa219ec497c48f59854898e4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec4C.X 131febc9110fb60cc286d5de45adf4d1e68621d1bfd2b46ad640e8d2febfb9a6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec4C.Y 3f237bc790754598317608965b509e5ecbe67e3bb563ef04170af4abd14fb23e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVec4C.Z e346b11dce178034d8c876b09deb00fc6b124f216290edf97f66df6d70971608 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorChar.Capacity 703c6ee49b233c2df84d9c16b78b9ca19436befd54d5bd08b16c435422fdba71 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorChar.Data 91d86b43f929ab498f2d5bd9776a50618cc46caad2a93acbc33cf2e42bdf2eb3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorChar.Size 3ce645337105bfddc5c9af39c80feecf72e2d1579f3b5909d1d5f2d85322ac5f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorFloat.Capacity 82a1ab23461943a6e568b8e37f3a0ac6ad4c3ebc4cdc8cdd154479db5fc301de -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorFloat.Data 9a1be9ece18372e3ac261c693ac2aabe7eae75ae8321716ddc4a39a2f4af1ffb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorFloat.Size 48423b81dfe5c90f5244e336d10b8a43e6e33d97d5e89de74e4eb1a31233f07a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawChannel.Capacity 9a3cfd0c53ed994c96727dc73c1c974b602a244220dc949419bb55c849d1c6b6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawChannel.Data e24f8feb51a9906f425d9ec2ab2e63d328cfbef28cf344762ef983dd6844619a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawChannel.Size 99bd9c2eb1ee6d0c0a66288e852b359286e19aaced8a48494a45caadeef74e35 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawCmd.Capacity 938e314f6b89f5d3f6c4c3d2823ad20d1327e302ffe379ed28c1bb082ba8ec0b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawCmd.Data 18e16fae3fd57f7f8c00c774f0533d9293fe0b41310cc3f5aa6eeddd8938e68b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawCmd.Size d59d5e8bb5590e902d868112a37ef85cb34cf65057c0c9376585c80d70a6c31a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawIdx.Capacity 66f0da9276ad3f2058203f3777036c609c22fa2b70029d4a0a8652e68faf3b96 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawIdx.Data 98718ea6bf3600750b9992beeaa89b1991117a6f2dd303994c278c4c60a84ce6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawIdx.Size 9c32e2c6ac58d9a8dbc5b26f8c7e1c145ed790fb59a1fbc4a42bbe8ea77bd863 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Capacity 86e8a460374e6bae9a4c81c7ec2a97843f3ef8968591625d501a4241f9a18b2a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Data f88951d543e96d034a7f2105335801fadfb037d7bf9d93c713b2ea106eaae0c2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Size 367d85e5fbefc097a4af065fde60c0d8dd1eeef567163c4c3ed93939f1197b79 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Capacity 9ff3969b77d5b943a306c8395011923f6d58b84af365c760076f8898bb911ba2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Data 7fc2c509a9a766f4fec21b97614e15936e2866a43e9d3cb07aa0882f5b6901ff -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Size c6c3f0db801391dbd3536d75a5c58b95fea1aa988e5456137286d649302145ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawVert.Capacity 02f4eba215e94a1559e96a77046c2aa3ee0a130b4a2d96ba87678468dd9c8746 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawVert.Data 22a3e235f5387d651bebdfcf6e22f7f495a3ed3f0acd9ed7111ae261bae4918d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImDrawVert.Size 93f9079a925ebd4dfedcd55367488c4dc057769416dd8c78a2f54c33475c9d0f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Capacity 93c5168d89bcf22dda7317a75340f92d0f228b98349bed8148f5ddb693e72761 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Data f3790e59c561ad5a5d74460f2f9970596346bda0b7abc5898613ec004550aea6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Size b8057313327ed5e555b3911844d4850e3ce7dc4894619697b93d3c31f90aaa9c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Capacity 3f376e684739d506bc984314d3ca556c29920fb9a55f0c8fd231014ae2fdcd52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Data 65095a4657996c724993ec5ba0f0cac88abf366e7770ead5b7247ef566c8d9b5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Size 9c43eb97857aacbd7f5bdb729c4265ac3a8720fdd99cf1459f2ed0ad9666a415 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Capacity 60f3c6068c49c124d9b055e1415c09926c82095a0f366368c2bddb1850b63126 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Data 5a2ae77a225a1d4571c7afc4fe64d9f700f4cdf942fb2af4ab11b43f2d7aec3e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Size 6767b47877639e2dfbf419a441056af70ded9e63a66790b0da88754d07648c8b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfig.Capacity 5b10b1f87f4442a217d2bb29fb86b1f9a70cb6c5db16001814ff2dbd1f2c6b7f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfig.Data 02db8b9e2dfc08e28a33870601ef1663a6b28638123c596d52849e811eebe922 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfig.Size a195a1b4b8052eb4e9c5c3c46494034455be9f9478c4623896299315cbe3725d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Capacity 5bebbde1bdc43b42c2fcdea1d1c331ee8efa7d3359037cfb0e7a7ba7be0a2092 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Data 6285e401abb18705612fd64f89f6f71489bffbe6349a0f892ca73125d44ebbf4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Size e046fc340cda1d840d92b4d3e765690bcc8e5fd91f5464a45ee8975ffc632876 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontGlyph.Capacity 250073267319b342b2703d6bbedb220b8d43071883d1f6a5e013554029dc0430 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontGlyph.Data 8c31f6b886e00ac13a30da917ebe1cc44609ae349452b76f331259214ae70cd7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontGlyph.Size ef28beb649f9a76e9fee84526e928bff77a2cef5d3e0243e30f5fd1aef3951dd -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontPtr.Capacity 56bac74b9808702c8d6bc884003cef5383c81d6cc72c0269a4cc1ce154bad11a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontPtr.Data 4336079c22dc5f6b567d8cc6ed967297f1ef88053d60043d594dc1591f8e7f3f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontPtr.Size 0796a9da31cc3cb31f9fbedb477408e0eca622e45f4f4eb3af67604b44bde382 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontStackData.Capacity b1e939eb4eaba91a9f17a66692539e8efa1b86fe9d7cccbd945c9d38e47e5e94 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontStackData.Data f1ca7da2675e7426bd00fdc68e7caf232f07f472ecea34d572109215d7c64cfb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImFontStackData.Size c91eaeb14f8fe338d75594c8c57149db36e74a370269624934a6a8ca7ce69ceb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Capacity 7f58442222d26d05eb95e16bfc68e6c3f6f695d59db63ee8a91d4dbc0162f85d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Data 59ae2d3e57deda058cf0c34e0dee4b4963adcf3aea152a8f2a991e6172bdaf47 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Size ff4007f94de11bc036480ba820246112de92767923186f061d296e431c19516b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Capacity 96433ef4170af669e6b3af19860d56e7ad1151bfa1cdbb548f16646dea615cae -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Data 4bcd4f4a03ccd63496f67438ae441494c4d9769eacbc180aab60d27e50b2599e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Size 1246bf2364ae7f7da039c25dfac3800af122d4697e7c2215f7ca53ba71a08af4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Capacity 5dbe460338bf0ca106ae34d7fa8470da32526f9fa599c5a819cad1df7159ed1e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Data a6de1fe653d9efc4c801d9d3b559c0f09a6a9e4b54575cad05446afaa0463ef0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Size 760a7ee9456024816639530708e7e65552e51ba10ee2a5b7be2ea59b1f0f6ff3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Capacity da07765c96955509128f3d46df3cc569c1ad0a1ae63d9a0913b224517c5390ee -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Data ac396a3245ca119e4952229fd0e0c8ae8940dce49b60c3dc545834bb9f472452 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Size 367741e6bb0dedbebeb129e2bab55dcd43676cc43c21afefe401a567d22bd118 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Capacity e7d8e59c45ff32080c5499ec966303867144fbac7d00d84bb400d41c7cbcfd29 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Data ddb35757ed1960cc7d6fdc47ada739a789cff58365c8960655871a989fa1f752 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Size fdfd01463126ab8775f3557e8bbe56be9b22f51c8a328c5d130e09d025f6012b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Capacity 67795a7fddf5296ce08799835abd843cd4f817a285298ae638d84932d1ae329b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Data 5ff493b579a1f11fc9f90ad325396d845549a442229305d7bf976aca32fac6f8 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Size 9a3b753e6db632068ca31f656320983e5c183aa999b6159f246f34c462c0cc42 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiID.Capacity 42b76cdfa973d752cdec3499ddfa52f3c8962f52fc02d81d7747db33c53cfbc6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiID.Data 97a76c1e2fbb9c2c6266b6f1a6c0a63308f224628f48a924fb3be4cb8b0eff10 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiID.Size b2f7ded77365e4cbb06efb4373fbf604b1237bf5461e5d10c4c752ad1d055874 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Capacity 1577b6d7dac7819e199a3c5a8e4c916b19ac3c3526ace0a6d4a8169425233186 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Data 9b540d5c97551b92dc299eb4d1406a9520be3815e6a33a567e98079243f4be87 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Size 58d19d2c220fb67f0e4ec67e8e4eba3d3a94f5188f91b83f36c8b5920a5d7062 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Capacity 63ea2f63c5e541eafb875b6b5c0dbd69274427aa4f7de0fa95e70bc0f2db09d4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Data 5e9d1b7b6157073f1a8cbb7738b5df205f3a0cda9569f3a870a42605d7377b2e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Size 7caa23acfcc85019b704f96572e0cffc373e31bba91e369446738783520785a3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Capacity e4a3e9f7ca1b8c2401acd7019f4e1fc0d5f4d53ad33219e89dbbf8756a28f31a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Data fee4d3c799ed8ef748815d4717b951a0053f2feb6ed61bab99c9f0641a4fbba4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Size 74f7b32f754fa51994935d9d3c395fa19da77bd5d1b6bbcf4c8d7b2f38b2612f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Capacity 84f5f5fd092af12a45a71bfd8dbced90047f0bf72c584faf2a46888ca8af321f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Data a3f634fd552737e0d861d2b3f67351dca1f680cb768061b39ca3e7d8ba498095 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Size 9dcafb23d54c1cf8b83ff844fbf0c3cd507246947e13ffd46b8148cf1efa6f8a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Capacity c045109e9f57a0977bbe4b2f9c9704b5f8e818aaff584799f616483dc3b06524 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Data 37432f2ba2d933a890fc90115b3d9581724a9baa4d3c1ccc869a710a4793ab23 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Size f2b87d68b4cab151b6ed4e05123160cbbfa5f5eba0134b1105ae04db2bc5392c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Capacity a42893ab0ed80c9c6082d80f3c3ff0e349033ccd16df11f5eac510bbd4858562 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Data 9fb4e15d93407d4cd543e839fd854ed057c95c1788aec185fa07a58eb493c21f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Size 8d219225571d42ca21cc755f2b30cdb91428c225827ce63cbb282db025597e69 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Capacity 485ddd3940cfad40745771efadbf1ad4aab69a70df63f03d5978d98b31e0dc69 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Data 4aa30a99784917a1712221307b36731a860b5d664e0e769f1c1ee970f865d6cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Size 88e51570d86328aba4a27329dc7065c57426f23735fedf7dca82306f0b6f7d0d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Capacity 9b70eb206424eac4e942a06a32d9b23867c26f2b6c883912d850a8eb3794a45a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Data 54bcdbc0271d0bd161059e3273fa6abad870da406239eea095720d4ed19bf46e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Size 79b7c673c5a93da2b6c38d74d32e770536d441c79d76dbf988fd47c273f068eb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Capacity 0dce2de0839b14dd143cbc8bce7fca65142bcc600db25ad31ce5bb92e274c9e1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Data dbbf3029c7be99d3238a5f6666f5fb655ca9d7f8ddc916007773df69dfd24e56 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Size 99e4668ce89310aa411b2623ac5e034a4b46004b0ee98298ab6a60db07ad7387 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Capacity b902e530b0852bfe1580d1512d76288c2694b672a12e149dedf9c5626bc24f32 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Data 0978b200f20db0f44e95f0de267959d4a4f847ae3088baad747ec6e0065734fa -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Size 57b88f0f0095eb07d36f1fcf8b7ac261ccc1250ad66fdf78bb5f8eef147f4bea -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Capacity 422213dfe4c3c641ec44960de6f38951dbfc281121cd6da3870ca7e8b73695c6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Data b8850a45a5b86f7b03cf520a43f34d83430b204b0ea6422d3dea19e6a9ef34cb -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Size 8c1b28bafe0c0c47e37006709ef5a52c6ec219105094c675781813f081fef552 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Capacity adcd3b5c2b0524d525a9d49d21e92c29eb9ce28a1f4c95a12532cd1ec54b96ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Data cf086c6ebe004ec8d4c17ec6a17c534027833f9d7b05bbe9f035fb6826235a6a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Size 350e4b2f0901ffa7b1232037815064a4c883ba5a95c67de3d19cf1117d31b987 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Capacity 2c46cb9b9b740a92538145a0f93a9c5ccf5ab6bd8f3c40b875726f70a8ea8fba -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Data 53f24abcbdd39a14232194cb4d9d3869af91b1e6ac0b81fa9bb2e374f72caf5d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Size cb43cfe0e067d909ea73534833b06ac589ac1b7f1202e28c6b36825f59beeb43 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Capacity 534669a18bd403abe2604166e17a55baf2916e317f48306da401b1d84278eca6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Data 627c817f66ab1efb91a749e90e9fe0a4b74d2201395e7000ed9d7fa7396c4d52 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Size da23e5c7394964f838268ee7c2f2d80acdbcff723445dc649c719a7646917218 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Capacity 20184cc18a78303c7bb03f4b404f02916106e4fda6e0b34c1fcd60252cbffded -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Data c8cf7cd6fb59be5cab5ff1a7081d1647cd71f5d5c0f7c29b1566f07ad91d2f68 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Size e2e232ca62d4d9281c60e1473f3dd73d06bcdd02a955ffce2ccbfb450445ee7c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Capacity cd7db08b92ef95f20fc2fb995921b4a7934619636ea6498a90cd450d62ea9b6e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Data e5152969ecad454a3c0570b627b84857d13eb21eeab89d285223fbd684847391 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Size da66bd9f8a7877f9b72fec84ca66ea92b875f009f0d415c0ba3365bc70759ac1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Capacity b6f8b1ce10705278c93122d7c194f2befe61e2c5a17b373ef6a9480195c5b579 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Data 0b597d909267d7c0b7c371431c2269abd4e7db4275b66573756951ce3d8f5fb6 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Size d10a973481ff859af3ee85adcb99135290c2d6fce4dec1757995fecd93267407 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Capacity 6070d927d7ba4ada4d634599296f9cbd65e8c5ff6e50ce01bbcf4129aab98aa2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Data 0ae6f48f37a93be249a667b748c9e3b52daa7dae9e770ccff0b68af09764728c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Size ede6f403b8bfa91ca69e4f862316f96a72fa5fd0ea66e2b91725c20e00cddb77 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Capacity f4b0d8d8c340a330f80034ee91415803182e1a20b83974162b0e3020bc8b886f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Data 68ceb19c7f465a833d7809d9626c576b6b36078b0ac8585b39dff29805936f0b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Size b290d07a6cb43b67c63d2967e98be594254f9be61d32b30f559cf97c6774b870 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Capacity a2648bd06b649aaa50b5d1ea0576ad657f67cb665c6c2dc98eecc336695b3f42 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Data abd9ec066b62aa802237303451717e260c4634d8724b9116d9e64da9794e86ee -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Size 122be1e047d8e96d7d778b7222a3417b90c9b4d479c0bb8c55226ebf3f41e893 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTable.Capacity 22b1ebd480efbd4a5c40f9a9c2b65b9163fc50ba2c058a1fc5e0ad991dc0cebd -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTable.Data bb7ee8464b96731cfb44317805d8f9702a2923d184b73e141729217e29cb8487 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTable.Size aedf36434736f0b873bc5cff0f0dccf25e7db3862f9d4aac2860f69cb402a983 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Capacity 0d3b4d9280f0bbeb9e9a9c9408553f5b29f0fdc209ba92d4764945db6f1755d5 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Data 7bde669c223624aa6c5ee398e75a2c97ae2023869d3c80281df191d26327b466 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Size c8fa15fe911583501fb9d6111709fca2d178b2d169195b8bd49f084d878f7dbe -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Capacity 4012b049231390a47be0aac0cc15c38a4d2c8c0796a02cca21e9f157e86ef5e9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Data 3e65c68444c6700f906044861b81f48010ef6a85e6975603bf86e1ea57aba2bc -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Size f480a333e890c34d25d9dca3f71c663a88da0391f75742e55e14094c817cbd6c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Capacity 9c9bdabc74429ec3ca407bd73e2a34b116ffbd7c1f2dd2f6f762733296aa5071 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Data 278e837152a7e1edf865257267d0c962b6d5b0d2e6776f19f0865e5b88b2302c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Size 76c340e5a4a44e8834dec87bf1954a5804193ce642de097bb55051c9eba3bb0b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Capacity 6330f1e340ed0b8058bdef58620b7ffc98bb844574aa3ae52d274c5fe84ac6ac -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Data 300ddfd8f6a6d82c30bfb6478827058e8273d54a3a67062274a20e9dd80b4df3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Size 7264eb8d0d51751451324a4085f87c854ea0a7d3260724da0cc386ee1ca847e2 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Capacity cb7415313157cee5f96521cb17320e368a44adc9c044a04437cd995ebb841f3a -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Data 7ad682b29db9c46f90c2f074d1e1c270079cf0bc93dd97362d19e76a0f2470ec -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Size 7b791e46f5ddcdbfa62201b73f6cb94bd449fd0224be77405af09eae9c488faa -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Capacity ebaa2e1e8a52595ca5126e3e4dd7729241b5f0ceef4db4478114b1f726f1e0c3 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Data a9c1447de77def8bf0b270480913d9c67d1f68aa865f281a13dd8616de9bd44c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Size f428558bda6aa9d06cdfa9da943150c1a116b3f0907fb3698d8843006ae0d710 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Capacity ac7c2a7fb73ae4ccff0c8913b37c2b55d2efca637ceee2bd1d06bba8213557ca -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Data adf0257833c48883e35fb512828d561023d1471da81169b2f226cca3f3d6bf6b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Size f118bb3b21727b7afa03f98c09553e0c87f673207c68f8798d679ed8ead35480 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Capacity f0eb1f19afcb1b6df98aaedb2f1373a656367021a6114a9763f619fa9a6227ae -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Data 905b17046edf11b764578ec2f6db405fb4841f7a53a2105bf91516e057d6877e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Size 70ae95c704623eec81ee3176e74c4a7d2b951251836e653f90a319124b30b90d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Capacity 64279a30f6d123c153a15243a03f57f82d75e8312d0bff7654b32f686ea28423 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Data 71fd58fd7fd11ce2008bd8feee572d7c3ad192898abe1b39047263754985538f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Size 5b48117d703a700966f10775e18fdf0e108c3e781e079de5984da975eeb0236e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Capacity 6bf849c28d0e33c13f6fd7bab75c7b591748995f60b2648028d3d3cd4e08ecd9 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Data 5903c440d05a248b700d8021968dd0885415e188ca9240fc4fc61fa3752fe85d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Size ed37b11c805c9bd9322cbd96dff6456eec38e1ad66935e8ad50ec41e0a486449 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRect.Capacity 52e84dd66e2b1caa570f8874ffaa515aac438eeb9e4239020fe775d26a63715b -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRect.Data 8af8d07b2b4eae56576b9379b778d5e4b9f48d449135d675cd96ff13f239fd7d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRect.Size 3818cf887d15f1573f743e97cb390df9336f28b6c19495973ed1c779d8c2bbce -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRef.Capacity ef48040d81560bc1e0aacc825e6606603498c8afec344a8f7b08d8776889466e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRef.Data 49b05ed73b28b53909ae95943a043a731dbd311bb3c4301aeed2c707bd06a1fe -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImTextureRef.Size ada4ba6739ab7d084f61224eea6079ae65509e334d0bcdf3cd06f4333326d29e -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU16.Capacity a282e539e3e3194668623a6afbf7fc17fcda7e4a4c67a82714ad92666ca98789 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU16.Data bd475137b9eda0078598ea285a9d7f7bfb1cfa9b95de7bf66c8aed5f47ccc72c -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU16.Size 268a769d2c75de6d25e24e8d2d47e1d6f947d1c26704112c8e720a8bd88c7670 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU8.Capacity 0cdb9d4a76ec031a00f40ca0c669f0600417e8082d9f04e2206c400bed818e4f -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU8.Data 204ee7703b4b9bcb08f7c7cfffa673b9979e380aefb58395e6a816fb91635989 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImU8.Size b7a0d9b8e510557c97a4fe72d756016d183aa070a1ff6d374fb1599946fcc930 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec2.Capacity 1e9a2a17f918f4cc432f59155557ebf948a83a49de11534cda5156f92d8e1b42 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec2.Data 8e129cd84d616cb4ad43db418f3d1b2746193aa1755a95511431c18b6d53a060 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec2.Size a2168b44585df5fe23b74b3014c165a1eea9a8618025a8b16fbcdf41d54b5c7d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec4.Capacity 7be318e88efbb1598a3ef3ecceaa64038d8cd3a2f67928deb205fc561799c9d0 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec4.Data f12a09e11fa666703ab0d89b56ad6007b777f27874c3a93e223270aaa7ed62c4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImVec4.Size d57a3f6e1f66e31aebf306202876cb13c2b7a5a3b6794056c29c4c7a49d27cd4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImWchar.Capacity 91e19cf1eea12d17a7f8a6a527ec5fb98e338c0328ab99c72363b437daa56782 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImWchar.Data 36cc1951a1a72e3e602beb3bad3b49290764a742504d5de1dc990f887d4f6493 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorImWchar.Size e95aa557039f971aef18eabe704686cb9747c4547fe9264c7c0bc086c571e5b7 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorInt.Capacity 4638751e42c3c420bf9189d595710bd1e0cc5b19372584b7407a20d8c3064bf4 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorInt.Data 330a07bdd93d6c3e92ae78d8c2564722f2603740972edaeb063f0a074abb0918 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorInt.Size c4ef39e656559e389faf2dcda352211afdf2e5c2a98103e12958467ab4ede0ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Capacity 6ee280df2bd6bb7e00a29a50a5884d00c0d847bb292414d8f5091f5e75d21cea -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Data fb11e4dc72b4a2ad68855c1710a142228996413e2feba78946b9234a006ae022 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Size 30784f4b83bdd0b15d8bfdab2a5ef04a2527eb4b77ede9af1fb5033a51d7f0d1 -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorUnsignedChar.Capacity 4ec33aaaafe2c92996fd4e6881c534d5a8ed8c46a4b9e4e5e301e5c731d7b89d -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorUnsignedChar.Data 7ab260e67c12a0943bb9dbe2f7892fbd8ffba3d0a9beb5b6daf6b7996eef00ab -->
<!-- api: F:Inno.Native.ImGuizmo.ImVectorUnsignedChar.Size 6fe0ede3e86768d00a7ee6ac19c73ee7feabf70596f2dad59ab9e3163f8efd1e -->
<!-- api: F:Inno.Native.ImGuizmo.Iobuf.Placeholder b2e57036b37637a53bac7b7ccd670b7550f7d2626480dc2c09a20fbecf82f9b4 -->
<!-- api: F:Inno.Native.ImGuizmo.StbrpContextOpaque.Data 68b6911760a918c1cbd1965a106654434d0c39c87623dae84317441d1874756d -->
<!-- api: F:Inno.Native.ImGuizmo.Style.CenterCircleSize 22f136fba2057d620d43fa8e0deef09fbf2acc66bcf427d995e2874c17fbd845 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.Colors aa3fe19d2b0da4dc7b93444944faec9f4ab077ecf57c74d87c912a26a14cbc72 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.HatchedAxisLineThickness d2d005dacfe0dc1118ee44de8f13fc13305deb6da50c56c4cf5f9fa70284ece5 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.RotationLineThickness 394fe0f464f61f77737ac3e2aebfc6f6d276d4b8ad23677b0f555ba343017b6c -->
<!-- api: F:Inno.Native.ImGuizmo.Style.RotationOuterLineThickness 16f9a2fc37702615d4c06e377afa7b32c95becd20c1ded7a84c2fd599174b43b -->
<!-- api: F:Inno.Native.ImGuizmo.Style.ScaleLineCircleSize 79a00c46bc0264dff7914b6ed7365a03627dc384671e0a2b024bd949d75cb5ad -->
<!-- api: F:Inno.Native.ImGuizmo.Style.ScaleLineThickness bbca3b9b6c4ba17e884aa43aaa187a1017e93d3255e7d79c9ecccd1c7502f7c4 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.TranslationLineArrowSize de7ff1f57c8f860a8b6fe53fd7a29523e03be91a6004e259770d74986f80c028 -->
<!-- api: F:Inno.Native.ImGuizmo.Style.TranslationLineThickness ef7a9c34d0425b63d25d1a205b2ec8fe351a74346d258a1a2bcf748753212518 -->
<!-- api: M:Inno.Native.ImGuizmo.ImDrawCallback.#ctor(System.Object,System.IntPtr) f3b4dee3db9a24549cf5653e95176da3832e45ee0f0cef6d0037614afb09051e -->
<!-- api: M:Inno.Native.ImGuizmo.ImDrawCallback.BeginInvoke(Inno.Native.ImGuizmo.ImDrawList*,Inno.Native.ImGuizmo.ImDrawCmd*,System.AsyncCallback,System.Object) 9e0875dbeca67483e72e36640a0ddacfe497db200f8eea81b7401ab99e7ce6af -->
<!-- api: M:Inno.Native.ImGuizmo.ImDrawCallback.EndInvoke(System.IAsyncResult) 572c9ae3fd738a63dbcc2da327115f35753805b045dc6becca3d209d9fc072ce -->
<!-- api: M:Inno.Native.ImGuizmo.ImDrawCallback.Invoke(Inno.Native.ImGuizmo.ImDrawList*,Inno.Native.ImGuizmo.ImDrawCmd*) ce06f263f1d364402002767ff225c539a129aabca359c5c0c6f570dd62553b10 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiContextHookCallback.#ctor(System.Object,System.IntPtr) d46faffdbfa197ff53960d92e5834d9e0200dc157ad4829775bfd5df565a8960 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiContextHookCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiContext*,Inno.Native.ImGuizmo.ImGuiContextHook*,System.AsyncCallback,System.Object) 36dad710390ed94d8c990b48a02a627a05b27351245e01e284a2e4acc8ab35ff -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiContextHookCallback.EndInvoke(System.IAsyncResult) 2f5b02f7049969f056efa33551a574f8421eda3bb240023a5e55980c3e963e32 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiContextHookCallback.Invoke(Inno.Native.ImGuizmo.ImGuiContext*,Inno.Native.ImGuizmo.ImGuiContextHook*) 1ac7a72755fb439b65843e820b1fa531b7c49a0e1a7a0f4f3be0b5b5bdd142af -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiErrorCallback.#ctor(System.Object,System.IntPtr) 2691a47f10f8a71d61838c6583b46f95163188aea858b6bd4f363c3055bbf9fc -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiErrorCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiContext*,System.Void*,System.SByte*,System.AsyncCallback,System.Object) eec280a413e0451d3d7211fc5fea0a2e989e767747c7814927870404fca9a352 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiErrorCallback.EndInvoke(System.IAsyncResult) 55fc5cfcc2d521d8813585bec914b4a6dd06dcf04e3ba22ab98517d1fdb00b3e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiErrorCallback.Invoke(Inno.Native.ImGuizmo.ImGuiContext*,System.Void*,System.SByte*) 1c37be240a6edafad4bc7bad4aaf342a6b07202549aa4f411d2d7d7a7d0d919f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiSizeCallback.#ctor(System.Object,System.IntPtr) 937a6bf56f679670455b957ff1ec0beaf670851906a8cd557721396627caeea3 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiSizeCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiSizeCallbackData*,System.AsyncCallback,System.Object) 9b8e8d856b36e7c28673abee50974682098fe361b4d628366a45e4891d0e1c37 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiSizeCallback.EndInvoke(System.IAsyncResult) cd5df3c55eb415ed5aaee7da2cad13cf38c8fb8e8c7e4f9389a02f809c5df4cf -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuiSizeCallback.Invoke(Inno.Native.ImGuizmo.ImGuiSizeCallbackData*) 30044e7ac50d2392f5d5746d369651f7af288380b72a6ae5af0a72a4c5b04049 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(System.Boolean) b1e2ca41f94579b2eb908f680c606e995148682cfc56da22e8b581f4dd045239 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.BeginFrame e1f3fb8a92d4bf3a435194aec231c42df3818e68e4791181866f760853271177 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Dispose d47e771c05c1ea8d7ce0fb1ae3f64bd1a08dc0c764b61ff069562f109838ee60 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.DisposeAsync fa724508f281c9f2348e484b2e43128d663b08f2ff986f628017c75d47110459 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Enable(System.Boolean) cdb6430407ac3ee87a5ef208f071af940dc3c3642aef736024a4b569f7923b31 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.FromProvider(BGCS.Runtime.INativeModuleProvider) cfe8011ba2d4d30f356a1714814c38d519b23eb6e0e6316689315d289d70a177 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver 02aa69d6729d1f58e2d74c231ae6ba5cbf9000587ea90ba3aa0d09ac4c7a52bc -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation) d83f7aacc5e51fd6554dc770d9f624d7ea8914d28066676104dc78147adc4a65 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsing 83fb2a0c449ec0ceca14127ee91b00654f273d921fd4591c8ae8249adbefbc80 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsingAny d0cc7edcde1b47fe59c3188a7d6a979d10778fe38d5656e25ad9c14fc8495210 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsUsingViewManipulate ea38cd855ce3da20e0d95e665d3294789a212d0615d2221b7b7b721ae2723fe3 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.IsViewManipulateHovered ad29658464a10b311f22c6d6ba819045e7c75a23502531085feab08afa7c06d9 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.Load(System.String) 2a276fc9efb1447c61df0f8dcfc325cdff0ad54125c857476f1f29ef2c489e44 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PopId 3493518d38e6102e94e88f8dc9957637f408c7818fb218a42c23dc2a76e95645 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.PushID(System.Int32) 5b48172ff753cc30b938c52a916ef86520bfc1e4960b73c2498a54365b0794ab -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAxisLimit(System.Single) 5da062ac1593edf605124085d3a167e457c4341701237644fc72a42480910871 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(System.Boolean,System.Boolean,System.Boolean) 09fb6bfd4a376eaeb620b90c004e1444ade08d5517a7925759ea808602d34f89 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetGizmoSizeClipSpace(System.Single) bb25e589a83bb87b1b264b8c47ef5d9d1b94cdcd90a9201d98417cb6cce1d009 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(System.Boolean) 49c781f0353d24d4b7228d0931d65aaa10b6fc277ade80e844b822a9d62ea34e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetPlaneLimit(System.Single) 6cdb4648a928c797ad7befb7da1a34886adcd264621a2fc2536af4ce5a15901e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmo.SetRect(System.Single,System.Single,System.Single,System.Single) f26606fcd30d73a581ed72f2ee0a0188b960ff2a2ac264d91b86a6164d3fcb12 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.AllowAxisFlip(System.Boolean) 5a218b8d8feb57520d2dc93635ac6a000b12cd968e8363e5af4dd2922b1cf12f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.BeginFrame f265bbd13cdb7986c305fc0948fe636e624c0c518d00556b3e33ef8c48ce3b73 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.Dispose e2a895e13cd43de497db16a88e618c72e3ab46fc9bab204efbf518f2b299abec -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.DisposeAsync a96a735caf634a3866fecbca54d804e20597e4f37980505813e2d3cf9e440e15 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.Enable(System.Boolean) 5d6fcba401d5fb06e8d25ec93370e187a94fd490944c436955d1f80d079451f8 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.FromProvider(BGCS.Runtime.INativeModuleProvider) 5d09cb4d12b81f38ba79c0f9b1a5e5992100da9ec757d43527451bc1a6d87349 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsOver b9e1ba8960206aac790029eec9de9daa7711d3fd5f0e6d4cb5486102688fd571 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation) 11213471e99b95b26e5b6d81d1074e748a05ec690c84fc33f14e69b347b3a433 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsUsing 3efface8d0eef4568ee618d765aea794e6ca093b571efd1dfa624ae712ec284f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsUsingAny d289fb324ad3812988ef5788d33bb5ba025677d9d86db72acf5c038bf3cbad5d -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsUsingViewManipulate fa661993382d9f1d7a0d336201e5455bde31aec82d74ff68392b36ef09aab4bd -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.IsViewManipulateHovered 0301a5d760572a7ac566c6c33c5d49115934be061a750c94ed8abb507247ce72 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.Load(System.String) 68030ae6b5a42d2b5e6c379df2bbea57b4464ea5ab69709adf720af438ca07ee -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.PopId 10bb81e57b7fb556e68a63d2fbd012faf629e727791ecd085de010e4c66939ba -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.PushID(System.Int32) 6022f0944d5f60f93882fe40b5f3914e064de4eacbf81178c69a8ab64153aceb -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.AllowAxisFlip(System.Byte) 2e6bfee8529c184af57715f3cfbe1f7d60a5e61d144ba1c04a5977be76aca6c2 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.BeginFrame 2853a9ec8b59d7ceefbd9312d121c8c30bc2a8c328a421e1c409bd42dab901eb -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DecomposeMatrixToComponents(System.Single*,System.Single*,System.Single*,System.Single*) 6a53a1dbde555b4cf6b8414d279d8c97be85f720e9ca4d3bbd5570ef5d2f0f6a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DrawCubes(System.Single*,System.Single*,System.Single*,System.Int32) 5375c277924b05f4e89885c30ea7df3872dc1b9bde8e939bbdc7d346bd1404f2 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DrawGrid(System.Single*,System.Single*,System.Single*,System.Single) fe12f0eb38e2488a1d5942eb38baad63effd1a494b81ffc0c8999ad8bcbf7da5 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.Enable(System.Byte) b25414b07603cb9f9769f38b3ea5bdb40ef4c0731166b1b960b936ffdd60814b -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetID(System.SByte*) e65d6bbaa3e6a65b783dad37cb61652bed73df264d9255b663ad2b3ffaf4c8bc -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetID(System.Void*) f4efaca9a97d88036e11f9d85ff98d699c7b6d68009241bf4fb2e0fc04e57d07 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetStyle 0116b95c4fd0c49f5dd667a842675c1160054d1a6bdac28f8cae7567ccf1e064 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver ae96480b03ea0103a5b0fd3e554ccfab3e5ba946bed055f464d1cec2a423f499 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation) bb665ff9ae1206849059180e426945aa98b28b2f7b99c4ddd0dd573c65c52d0f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver(System.Single*,System.Single) b105e2162309ff0fe2ee7c2901d99e1f01bf1b157fab8f181e0b1b12b3f4e708 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsing c46457d06083445e20032ea63b540121dcb8aa2efb9819202634f2fbf56d808a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsingAny 6407fe4fd26cfa17eda826ad38bdb2bbf7db3bb13bab94e90102175015fa9a05 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsingViewManipulate 387d4f589802e51b5b761f4d2a0583d95c77171ab3638d67df89905569dc7c60 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsViewManipulateHovered d59be18cf7e3a3940fa3375aa0b9f774901c5631b9609e0838d1c1a385044b5b -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.Manipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single*,System.Single*,System.Single*,System.Single*,System.Single*) 016433a666e02b8a5684f7f2947c7bb352d72eed55d514c9c1785eb7ffbb090a -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PopId 7f3f44a9e786dcee1dad8eab4db3242fd034d74c7e36152a3faf9fe3f31ff952 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(System.Int32) 4bf6f2138e82af83c08dadf7f586d37d94514425d0711fe932f0b93300ff5e1c -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(System.SByte*) b86b240dbe49f569c3d4dafc44873372fc314c8cb0d3dc2948139c601a6ddb64 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(System.Void*) f9a2f44c5acc73e55b693329f71c8c3e19981319a8758d2aed0a1d25a24e224e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.RecomposeMatrixFromComponents(System.Single*,System.Single*,System.Single*,System.Single*) 53a7b7c04583ef8c7cb680c8eeb5fa9cbccabd143b07fa67b5b1f5a082b93a3e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAlternativeWindow(Inno.Native.ImGuizmo.ImGuiWindow*) 0b555a82e1ff3145bf6e23551f993b581192f81c57ca166c0731a756a6e597e6 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAxisLimit(System.Single) 7b9ba6adf66b268cbb76cfb9f1ac21abb5d97a751fc98e2c56fdd163e6eb31d0 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAxisMask(System.Byte,System.Byte,System.Byte) 9303e52fd7344644d5aa9ff7b5233f3ab3b686bd7fb7e5e7d00cfda3b6712a8e -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetDrawlist(Inno.Native.ImGuizmo.ImDrawList*) 962c4ff2ba74a6ce6ab6b00d0c4b49501400b2847f30b7fab27dcd083aa2091c -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetGizmoSizeClipSpace(System.Single) f9adcfeb3673bdf6cab73803f2d6709532cd08ea820075ac6d6d326fe485eee2 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetImGuiContext(Inno.Native.ImGuizmo.ImGuiContext*) cceee0a1f952b8e4ea49d0df6c0a1c5538b8a4d3e3d479f51c04872239cf44d1 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetOrthographic(System.Byte) 45c7d7c342e95a50d4f22d35b38084aaf37fa0c8d6303b727e5e674c9ef79e2f -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetPlaneLimit(System.Single) 3cb1ffc4d026337c643bffe99ead6f339fd187d910ca53e3a010e909e86925a6 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetRect(System.Single,System.Single,System.Single,System.Single) 3c0a105264750d77d6f4cb384ed9bfb06c6b9ab4d2baf62842d25f4b3d91c044 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.StyleDestroy(Inno.Native.ImGuizmo.Style*) 5dbb1014ef29243d80fd8c6d8dfeea7b1af9d6bcdf080f091f7675fb8e3134c7 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.StyleStyle 8363ff4d089d4750aba338288009ff0a9d71869fcaa2f33cfbbaa55f269288ef -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.ViewManipulate(System.Single*,System.Single*,Inno.Native.ImGuizmo.ImGuizmoOperation,Inno.Native.ImGuizmo.ImGuizmoMode,System.Single*,System.Single,Inno.Native.ImGuizmo.ImVec2C,Inno.Native.ImGuizmo.ImVec2C,System.UInt32) a9a65bcc220354cb2705cc0612f923a4de04697d4d7aa495e9d8e24f06c6dd86 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.ViewManipulate(System.Single*,System.Single,Inno.Native.ImGuizmo.ImVec2C,Inno.Native.ImGuizmo.ImVec2C,System.UInt32) 6d4fc93736a52e8cf302fcbf610cf665265bcd411a455cfc77454d1e5567aa33 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetAxisLimit(System.Single) 4d67568e7dc678d56e45991b8964dd24f28298ea04835d7076b4e1cb22f6fd06 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetAxisMask(System.Boolean,System.Boolean,System.Boolean) e8e92e33a7e9896c607ee2590d5eef7f53a0957b489c6e52c5cd4a35395a7cb7 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetGizmoSizeClipSpace(System.Single) 7aa4961d9d8e7a8557150ca8583031c169159637d39fbf4c21de71b90ddb0040 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetOrthographic(System.Boolean) 2073b1508714475e8b1b9b49363341329a3f8c43c36aa7d84c02c294252e8f3b -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetPlaneLimit(System.Single) a3dba06dc247f293222c6d1456eef2be1796091bfd3a03f56cc109fcc069db04 -->
<!-- api: M:Inno.Native.ImGuizmo.ImGuizmoContext.SetRect(System.Single,System.Single,System.Single,System.Single) 4df3b0c3198a4cc2875b6672c1453affbc9defa9a230c3141d4b19466eb29cb3 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontAtlasRectEntry.Generation 2a733f5f91be98535a27365162516fb2189005172d60b73287dd74a5f9ea715c -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontAtlasRectEntry.IsUsed 067f90b4156a72da54f84722a62d5fff78b1d4c6607f103bf3bbd222fa79f173 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontAtlasRectEntry.TargetIndex f6973e564fdc5ca383f2614ab134e0a77d08c9d4d71915696f2b98e34218285c -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontBaked.LoadNoFallback b8c4bb49ada6e5a3712f8bc883b79e195cea64f7031473d639f936addcaec546 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontBaked.LoadNoRenderOnLayout f4cec24fc3aab65a42447686fc08697e041a5a4ed2624b5bc7acae996f951f17 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontBaked.MetricsTotalSurface 7660306dded106eb6a666df2d7272fd7cbe04a135f4beda704da972bb10f5bab -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontBaked.WantDestroy ee1050b5b8fc09a39c70d7b68182591cae5796a7880082524cd093ec592c0482 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontGlyph.Codepoint c91394cf93238fad855af54c59692f3bf608d39da3dafa0d36ad06f8e1c31332 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontGlyph.Colored 514f2202e944413d356127d3f440ede1d5f0f2951434b45d76e5ba48bd7160d2 -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontGlyph.SourceIdx a9174a5aaf4f1533b92895975c3dd8a75b33fbaa73805a846e09463bc7ad433b -->
<!-- api: P:Inno.Native.ImGuizmo.ImFontGlyph.Visible b724dcba2991b26387a3d74e911a9f5946544af62bdd8b1e9478136922ebe541 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiBoxSelectState.KeyMods 2db120d5d8a4a910f2646be59e1baad2c6f843f80d5fd1dac71ad0c15408978c -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForPos afed23f10361bd26dfc5dbec812aff32e269462060a60099b1e092bf7a4a9fa3 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForSize 730a3271d33233371e5c720d24a234c6d158bbbd18dbbe3b7fb5379391421ec7 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForViewport e6c0efde6839bfa94517b21427c06ccda0ea0aad3007629b851a3cd105a34d52 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.HasCentralNodeChild 3ccfa2b9e06dcfa5f68b014d1a0bc7872dc5a6bf123cc9eb6b6a965247330412 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.HasCloseButton f14932add98ed12b180d0433a760f707a9124908d6524aadb48a124148a48bd8 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.HasWindowMenuButton d375e40355fca52ef97e53c46ea5d345ad7e9b6391ed9255243dafe2df11aa16 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.IsBgDrawnThisFrame b6caff9d8827421f4dd3d687c1355b50830eda83e47163bba6ecf8bb90ae6a11 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.IsFocused d64ad10cbc0d2d00d077c264b87f57f94630d3dd60f7e6f62b903ae2ccaf3cb7 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.IsVisible 6aae953f1ecd51f2a9f62f8fd13d4fe20dc9c112c2ec4a8c086513f8ab9dcc16 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.WantCloseAll 62ccf6438820d47b8db0b2f38313a720fb7626750fb92ba4c64afa13350e6572 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.WantHiddenTabBarToggle d5ad51ac725f0d2d503b9aed5043ff735678c815f90f4b8dc4dd8fb3463bdcd4 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.WantHiddenTabBarUpdate dfc97dd6b41f721c0425b4b08d3b479ae6203be4b71f81dfcdbb3124c821f3a0 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.WantLockSizeOnce 342f37f558bad47034072c60ee613e48aa8a7b7e2d65faa6f02881016d8e8332 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiDockNode.WantMouseMove 0e604fc0f68b44303a10c01058fcdfbf84c86ac2703651b84bc25e90ae1e72d9 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiTable.LastRowFlags 502e51cc653899ac59c6f83e8d2a1f11af0907c1cfd1cee749911d04914d73dd -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiTable.RowFlags bbd0c524b307f20154037c926633942c75ca267ce71fd8498a0a80b046e4bf49 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirection b9ba94ea1e9e87e2ec8499690431eee85a0a5538f31bda6312fc0af317fdedc3 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailCount 759e3089017a96d6949858351df92f822b1f8c0d31743729a7457d45e512a5b0 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailMask e8a2160d85df8a8de0d609a08313b3b672cfbdd40ff96bbe10ecbe9a4ddc00c0 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.BgClickFlags dbded763aa418e4630c91f6c42bd9bfd8bee065e7cfcd11a06f7ccfad956fb20 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.DockIsActive 2c293bf541f9ea86aa016e537adb99c8ed6929a66130e6bfff29551ea1b310ca -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.DockNodeIsVisible 0813f12c8ca5319a66ac202dc636a3c65bef888e02b8c49465db1a671e1b1041 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.DockTabIsVisible 1d5e5d143e8145728475f5f0859d8bf75f270caf3d4456059f621e391b0df48e -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.DockTabWantClose 65789994c2ecd430ef5985cae11e26677dfaa1c9eb8f06c4cef0c009a8cc6c4f -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowCollapsedAllowFlags 4e74b010056c11c73736af399b02c061441aae9c82b9e66ccf0d7b4c4803f8f8 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowDockAllowFlags b85f07c6a6c13665e2ac16a8800cce743597bd3b7d03996ff73910b861d8ea1d -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosAllowFlags 0e44b192e3e13d80571e43f2f6145e75136fef2e124df1df3437a14693bd0f0c -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuiWindow.SetWindowSizeAllowFlags 65a74123f2e2560abf9db518f714dcd457a2e1470408a2fda1702a97d22bb1dc -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuizmo.IsInitialized 7e67f76a93c213f42e868b928ff8b69b3d08572b26c142d26dc5105425e18560 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuizmo.Raw 521c1579072b3e5f4695a5b982f4eeafbf0c4e218395bb42a33f8b51f9918506 -->
<!-- api: P:Inno.Native.ImGuizmo.ImGuizmoContext.Raw 74671b6b57930f9b79837f6bdfd40d3a165c380131fd3052d1f59557f955a290 -->
<!-- api: P:Inno.Native.ImGuizmo.NativeArray_1eccc798bd000925.Item(System.Int32) 83c3b51108528c45eee3c897ac696226cee53298d4880525fcdd53a1d1d8d1ff -->
<!-- api: P:Inno.Native.ImGuizmo.NativeArray_68e25b300381a6fc.Item(System.Int32) 8fff928dce80a3871029503a30b900386275ca524f42b3f34f65dad8281faf2f -->
<!-- api: P:Inno.Native.ImGuizmo.NativeArray_7b9f5e5665cb58c5.Item(System.Int32) 28019c8282e8ce2a31f97c1d315b9728132b94bd470d19c6247aceb8f837413a -->
<!-- api: P:Inno.Native.ImGuizmo.NativeArray_df98efe8482c9a26.Item(System.Int32) ef533398d33137e9c6c9dc33372296bc4efdeb7882dfbdb796ff1253c26bfe42 -->
<!-- api: T:Inno.Native.ImGuizmo.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN 328ab3dce901d82d26c7ff2265a3df14ddadfef12baa4862125b5709ac0404ba -->
<!-- api: T:Inno.Native.ImGuizmo.ImChunkStreamImGuiTableSettings 50fbe34c3106b0f2ceb5dcbf08894f6b547bf5eb18de62b68c552396ed6523f1 -->
<!-- api: T:Inno.Native.ImGuizmo.ImChunkStreamImGuiWindowSettings c2db75a69448117ab7c870d27d0c7255710274f36de63ba81eb32cfc8cf79532 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawCallback c943e85b4ed5d440716574afbbced0a61b963904427cdbb3645c4d166717cfce -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawChannel 3e13edefd30beb6fa83ee7a4bf762383b67b439c52c3ca445136568377fce068 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawCmd b7f3343c68abcdcdf9aac0d15ecc43b8e46d6f48427a081d598c53e5f8bbf6a0 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawCmdHeader 5168aa4f448fa6f57168966872bd1d37861933ccb6b99d43c30b7dad07cfa658 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawData 553826799d1b5d9d3288ebdbc5b8d76daf8f7a5f074bf2640cd5c807e693ff7e -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawDataBuilder 1f0f23d58d524cfebb0636aa0db33bc3adeaf8481c3f7530d68b95a9ebf3f4d9 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawList d4e78f18699da1994ace65f1d022b55ea48f98bfd2285331a86e3730d2c758f5 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawListSharedData 78ffe0ad34b0faacd17cf29fca5807c8bc12b8fb3f98767a330adcb61fcdbe43 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawListSplitter 0b6afb697073b7b68b21307a966b75d3fd6f53de196a32369c2108d455345200 -->
<!-- api: T:Inno.Native.ImGuizmo.ImDrawVert fd79d58d628ff0cd7f2c96e2efac6075688518edff3734598b1c320cf881d8d5 -->
<!-- api: T:Inno.Native.ImGuizmo.ImFont f097a5d9017f1d4faabe9733a75f8a1ff2f61fa3bc579ca958ad25f429343b09 -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontAtlas d0eeb4364bdf2df23224a2aed6167c742ad88f69a79a4d37fe47da4c34c1d8ab -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontAtlasBuilder a88c312fb398510493daeb7287bfc01fead347523ad4a7004954270be860358e -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontAtlasRectEntry 4bc7306c11cf02081a528a7f73c7b7dd1e5ccd6e0ce7f2940a5c33aa90c099e3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontBaked 579af8a096d947faa4933bdb9ffd8aaf7e1a9898c10bb85deaf74c77cfe589c5 -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontConfig 6c8502c205d35ead89e2ca9e144e9bf045edfff161b279ecdfdf9fde02b53a9d -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontGlyph b07dae168714e88e2f1f05f73e805f3b18b3c2316407fba4348205d6c645947b -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontLoader 608ad4a584435ad9edb763f2b86c9996c00bf7677bc821077087d19147cce82d -->
<!-- api: T:Inno.Native.ImGuizmo.ImFontStackData be6ff8103729d829056ad5dd9b68b1f0a9f87dc3a56bd0c3e1c58c0eaa36caa7 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiAxis f88872e9d3861f28f7bdd7b8d08b6b47ef9ec5308abda9d8fca9ad54c24c69b4 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiBoxSelectState 961d88b41f737f90b48d89dfaef8953c4a65cd3b881e9ae5595cacf97c028c8c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiColorMod 3c1d8ae1ebf969e8b590608a36b1065cf7ad9b727159e12a3f31d57b42086428 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiComboPreviewData bcb78d57895c523a99f9bcf1dbae47a8117a515d023619332290c73611185d0e -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiContext 3766ebcc6d4ed67d02ea2551aae7c62a7edee8616c6cc2de13758dc7a7271c7e -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiContextHook 28171e319dd7e2cbf407ecccd037ce1948cf0d7033b14a3771e3615bbf95fd95 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiContextHookCallback 58d1d02ea5cfef77a9d8b698ffcfeb9342b2b4459fa57f47075d78768d5a3739 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiContextHookType b7922c20094f070876cbfff6b1a590acd03827074ac996b1661d391632b9064a -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDataTypeStorage d6edc9c1e13714119e5db0570dd2209a6a5578e4c480289c4af54c0b8c367025 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDeactivatedItemData 815f05d7d91dd42bd666d0c2ea79ec57e994dd86272be05c59aa462df129a5ca -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDebugAllocEntry c61dee32fa43a5c6ae2328d8f979b36ed3ebd56b255136075cc79ef4681f504c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDebugAllocInfo e60031acb470239aa30df4c8f301aaedb2a3559c3e8a13fb01d06b8d6317c55c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery c4a2d84f0aeba73fd5ba3b4530659b31862f7b5f6d0718d74bb1ab9f3aaf166c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDir 5606fcca2b5ba5135e2a5507c1586f801a34981fe9df5b4b034d743d7408b643 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDockContext 3aba12dfedd257756fb14a6f50eb4dec87cb166faf234e4557575e450626d650 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDockNode c4072041327570358b51c547350f825e1f4922dab25814de236f3847ce3b54e3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDockNodeSettings af9d5650fb13f0d1a60446ddbad6eeb7b2539b1dc9a2ad1fecc0b59263e08293 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDockNodeState 82ec12cc3269ac73ecd10afb669d971a53691a4d26340a66b65e541c6ecd027e -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiDockRequest abbebbfa43fd0f6ea73f0fe5836c13c138df716bbe40d5fbfa415dd96030e83a -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiErrorCallback 451a163cc3cbbf2c83bcc7e812daf9d3aa483a56222656c6b0e4e5874bb51f97 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiErrorRecoveryState 2dc6c4c3637f8baa3ae8b49c0d4cb7858473cd76db7d745a25d30a5544fbe887 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiFocusScopeData 6d08eababb346b80b3782170f0ac040ec21fc7f869bdee055a56c4fd8f8900c8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiGroupData 7d262d2209d37b61a0bd088266be671ccbe67a2a5b95d4f2ef3417ebe813fc49 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiIDStackTool dc634989b1119b66e3cbe53322ced6eaae41fcb61c1357c9671f5aa90806da65 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiIO 24e6582064753c481e1a905f96ae72e64e81eee0c95eeccda8985763e80bdb0e -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEvent 58555b3d0bad766284e118705605c9f742a37d87003383c5ad04666554fcf989 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440 c6a55d6e89cc9b4d68f2d5536449fbc76033972104aa49dc95e9723d3dc224b3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventAppFocused a54ce7a794a7c53c37bfa1bc51e23a44f3b63051f59b181d3225430446432196 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventKey 963d5a24ba6e16961a8f37b6eaff4aa64539fb1ef1fbee4afd43e57a92429ba3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventMouseButton dd62ce786fe48f4212f9b338ab1d0c2471b12f0bf1de04a9dd1d94eee0740c12 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventMousePos 3e3dda4019a0bb7388bf742404fd0a98fe199b12dbeef67af400fcc229f32b71 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventMouseViewport ba33bf659d2dd2f5f6a32c585da8eab4f558b6794930852eac290d65da292af8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel 4fa484b5208e95671d840d80a029c9dfc2d3085fb670d551b0638f1d8bdd6526 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventText 5a82885729a846af00e12a4543aad8545b9f140cd73a3fa598417e5f1298c673 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputEventType 0ac5ff31768aeaaeab9c547730096c40c21628d82e212266bfd01cfe0708503d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputSource 55ae576ca26108f82a4bc267b43e60749d330189bb719e4959c905e714fe6dad -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState 5735e209223fd751108f1a9f03eb30dcab47882ed21f48fe90120c2a5c80c88d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiInputTextState 28fdf41af018fa6bdc69a05c452541bfb20128f209eb1fa4dd62e451f4160b9b -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiKey 7a9b4aec0e4d63362128326f7f1c6bcc01081fed8648e032fd1bef67c9a4185d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiKeyData 62261693528fe55881f8ce52d04c8398357f6b2d4a4054ef3871a29ea8a8f7fc -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiKeyOwnerData 6d0b9410a884be995cbc2c3e4ce14a9822db970a92973c5cfb7bafc4c1b483c8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiKeyRoutingData c3d1d8e11bf4b20cdd2ff34be836445be3de3bdb442449f2369fd702666b71e2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiKeyRoutingTable 43184c10b069de6ae236c1243d77cf32bccfd705d6093e151178bcb627011435 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiLastItemData f08b66090a22862db6031de5c1bbad95eb9da1ded981266745e3a4e7de9353ab -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiListClipper 317a0450b082c4b32ff1913f63651c060315b2ba9989ba8c1c241ee597c3d325 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiListClipperData 06f6d79ecde3d14c18dba9043a26b0bc10dc127b80e8355be138aac2ac23cdb8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiListClipperRange 7e607d3b2f43e60416870dfa610683cfbc4a47e3072d0e73877e82562a7f197b -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMenuColumns c970a52a51ee11be80d4fae861ba712908183f139d633a61c53f758efe4efae2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMetricsConfig dac2e040c59adf7f87f33e958fe192b57de9bdfd497f7e1b849493c7ab71338f -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMouseSource 61bd28ab2b11acdb33e5b318d397ffbceb15492685ebe72744e2d91ba6368d1f -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMultiSelectIO bfe536a1c2f4752664f2a7a4722fc6d4f6cfc1278efd47597e0b5eee2d6e9445 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMultiSelectState 8381a0d82724cdbf35daec7d96b8f3c10b0524e452ed9979fca40c6117572203 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiMultiSelectTempData eac8281a62a16a8ead180c3c889a6a53fce60c75fc94cbb4ad119e86ccc89bc2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiNavItemData 59405726254e9597dc6c7ce3f5c097b0cf94c0875c7fe1e95d4a73e4f22106ac -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiNavLayer 2ef9b913a315906f8d0d49309af0e3e14a406029fabf31798bb275cd9b2468ec -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiNextItemData abe76ca3dfece33a61b0f2983353561089bf3f7fedc5ac3087ec09ba74b06f25 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiNextWindowData 52e22d9829b1f1209581a873975e35b5622670acc14aa17c6df0f6172e91c97a -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiOldColumnData 849e2bf47ae06d93a319b23b396fcc71a01a78d2d4f9d90bbf6bd88ad10ee8c1 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiOldColumns baf7cc117ed1bf05281696b335fc0c666046bdba0aba99e6dc39fdefbf9038b4 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPayload 08bf00f61cc75267995c39dbf3e16563e6e21c4d2a31829b2980c5a05745e265 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPlatformIO 6a1373159d11305492cca91828d62a2082c0935a113eed55221ab7d71eda1b02 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPlatformImeData 9594f57d67fd30e275c960abc91650664634c79f4bac89db79d57800972dd2a5 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPlatformMonitor bb04768d4b74d50eaa0eb0a26736afc98aef39146237212d18b9cf432ec70d29 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPopupData e42ab2736fb8a50e36c7d22ed8f3bcd5f60c7af6cfc7be3a40e7105793f0d409 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiPtrOrIndex 46e19fd029451f891e5a33dccb55b3916048e292a714f704bec9a7a9ce2bc1ff -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSelectionRequest 22adf1fd0001a04e66c4e7598773211adfd3706e26d32b42bafbba1398bffe94 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSelectionRequestType 483d97f69dfb5b08609c50c30fdca70facebe291dc68fd52cb4347105ca8d2b7 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSettingsHandler dd28c25793828014b82e65cb8c6e3c8c1789354f8e1b9442c9c492f597a90c51 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiShrinkWidthItem 617a264b9c0b51acefb06299c184868597416263ae63734924ffdce7e2baf387 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSizeCallback 40956d8bf8a86f5d33336377f10bfc29f968fd1d09700e301224eeae0a6cb280 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSizeCallbackData d4c3c3c65c11256952f8ed1b8eee707bd23c68b95c3fcb717fe398311d381368 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiSortDirection 4959735b1ebecb1941831fc174aef419526c0c0790e2b4c2da715729f6960e73 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStackLevelInfo 3550526628101ad658e1a642c96e6236224a67252e1c9d39172ecb454d95252d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStorage be5f1c6c52f9aac22ecee03b7f5f437d7b76ed4de5c9302f0b404e545f0b9366 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStoragePair cbc18ec40aa9f40ffd14243b85e3e1589b14c07eb169ba164a1a359589672ba2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0 a5195cc55a6e81849ed0f3d775b196ab0790b0965964aad994b54a46d010c138 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStyle 1aed5a9692acf255eff54ac0617904d585926cb6780e0bd416a84bec7af62c3c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStyleMod 85d85c010a2324e7503d8a661c08f2405b811535897b5fba428d7174f12275da -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2 85b4f4b0d3bbd15f837f3f0e28f05a441121169101b2a1549744e430f5e69d68 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTabBar e26c3747725af2fa41ae3e2f37e0bf8c8c240375a9832a6c0b964205e3625555 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTabItem d35fcf288d85fff75002c65e69c4c0bfad6fef2428f798154c17847c87073653 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTable 15d6a2f6aa0934616df2f166d729085473d510f3a36b0e0867224a673d330c04 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableCellData 3785440b1881f2f03742f95a79ed1ec692f4132ed070ab45e781c389c2dc28b8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableColumn 8977a5fad2f2c14b7cda0bb1ff50ac18da37704b783e65927a3156970e34454d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs f42acf803b847765ad91c4df7941449fbd08c24135421032326a62324c4ec413 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableHeaderData c81b6db36a3ddebd6a1b909949c8c42d6b6eb3268094cf2a1ccf2ebabedf8367 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableInstanceData 8c961e884e5281398ea3126bb3dac3c886ed427a430b9373770eb221aa284407 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableSortSpecs 62f95dd8f68b53552600751ffa985df64c69573a3ce6b1cf606da9eb94b7662c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTableTempData 80c2e5bbf20cc9d34adf7587f588fddc13694b37bbe50fd7084f5700c3fba7fb -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTextBuffer 92ef80bc60ad39ad3ed900e2c7840a407f0e06f2efc9e7fcc21ce7c077a1fb7c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTextIndex 6660f062b12380e12342169aba80157d89b5d8ff28dd60adaca47c325bfe8d10 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTreeNodeStackData 8c625be4f1decf5ed5d34fe574b6818fa4cc91901197d11618d99bde731bf94c -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTypingSelectRequest 1dd6a1d840277355e15367b6854170732541f3d33ac4d325fc10f77e314e98ef -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiTypingSelectState 3f899111ed4d3ba0eb3e104aa59532fb474a78ef444b37c619e25a6aaecadcaf -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiViewport c3d80da70c92c5148fce80d387a3cfee731f588adf6e91ec470b91dce9ec028d -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiViewportP 2f8a1a8962b38e106608fd14c6e2d0841a353650afaaf0a5acb1c087d422a1c8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindow 191384eb1b84f42b9324c78a4eb3c14b33b8b9f6d6d697a5fda50e88c5472921 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindowClass c3b38ab90a884aef1a02e065b10237cdb4b802c7edf85025b9f27b49369c7fb4 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindowDockStyle c4dab1494ea7a49f201d3ae50f1a161da8a156eb9596a0fcfc54ba93f3f8ae35 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindowStackData 485fc146c8b4990b9f53b92e2a4ec3efc5dcc672ba579ff5cb6f76d735b2b279 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuiWindowTempData 14f35772a11c24bf3d2597c3ce8ed351db5f5a880d6600a9cbaa1d7ede00a574 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmo ed65d8c2c0b06497666de94eb48d12abbb1875a851043ff95de573abf339f5c3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoColor cf4f9bb7a70a975769702c81dabe5c54d8b4c6974592bb9673f8b7db6fbf2516 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoContext 25f942bb66ee75b8acebc3c60ff60a3d8f0c331048231984d99b87ffaf4c27c8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoContext.RawApi 38b7531e89bd8f257246a4d4dd904267748dfada0581be5fc708383df05e44c2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoContext.RecipeConversions 9a69ebd0d4abe905aa014dfd63d0fef9747773393ad272021b58d03194918c56 -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoMode 65909b6a9c6e6ee448e29e02b8aec317eeabb952893a8a1d4247454b9884b94f -->
<!-- api: T:Inno.Native.ImGuizmo.ImGuizmoOperation 6bf7f9e6a258ec6ca246a9a537a5caa71e1ef4f1955c06e594d01e59e2322d3c -->
<!-- api: T:Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState 2e6bd46b8a6c46a327c1bd06e8a29abe8d99e1feef5da98ccfdee46a47248dfe -->
<!-- api: T:Inno.Native.ImGuizmo.ImPoolImGuiTabBar 32567290d774b8283ee3d24c9b4acf47eebeb5552e1fc25613834d9e7c51ea6b -->
<!-- api: T:Inno.Native.ImGuizmo.ImPoolImGuiTable 39a7a1b236fc16a041723bbd769ead45370d4165b82b9ef9460a247851511bb0 -->
<!-- api: T:Inno.Native.ImGuizmo.ImRectC 54e453a998e2d51aa7597debbba5d781225c7c4b05c29b3913a000ead5c3a046 -->
<!-- api: T:Inno.Native.ImGuizmo.ImSpanImGuiTableCellData eeb423acf6f98810395d29722bdf185cb4708ed28c0f0946c9363a990db15343 -->
<!-- api: T:Inno.Native.ImGuizmo.ImSpanImGuiTableColumn edb9e966620ed01ca507cddc0bfa3b8fde7d330d65181f99b2f6705670b7f804 -->
<!-- api: T:Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx 9e434b62db4c8bf995bc649275ec186010494e13e64d50e4969b5197655d3477 -->
<!-- api: T:Inno.Native.ImGuizmo.ImStableVectorImFontBaked32 3082ca12cfe2d19176438d02580bdbe285847f3255189932e547b266103da871 -->
<!-- api: T:Inno.Native.ImGuizmo.ImTextureData 139ef584394cbd6d806314983a25729aecf341653a65ab92cf9c07f22748d5e7 -->
<!-- api: T:Inno.Native.ImGuizmo.ImTextureFormat da4e93cc92b2f71e614cf7045e0d6c3e9ec82d053c789195c889d4e5bb6c9b11 -->
<!-- api: T:Inno.Native.ImGuizmo.ImTextureRect e97ccbe739f11d4fe67e3af4299047b97df7bdea11a8a71365898dbb04b1dca8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImTextureRefC 37a3e30c0ecf174bab9cc68df5b5d9e3dec6aeb8d0a7fb9aa43319553509760b -->
<!-- api: T:Inno.Native.ImGuizmo.ImTextureStatus 68792163e6090bc541d434f6e9dced0988ab2685d34cf5d2d06d311426e669ee -->
<!-- api: T:Inno.Native.ImGuizmo.ImVec1 e9d685cc8a74c9726b31ad1f47f68def7a876e12ffd6a2be6a6eed9e44cb5dec -->
<!-- api: T:Inno.Native.ImGuizmo.ImVec2C a3df74d865662cb1b0abbb070e2a7be2dd7726ae695b11f04c36ebce3d897616 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVec2iC 5c2c8af856a99dea3598a0f9884ee57e43b5662d9d48a8e8560a4c32f27840b8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVec2ih 6326b227c2b809fd5e23b1b54b837ac55f0bb27fb3975ab75638166130a414dd -->
<!-- api: T:Inno.Native.ImGuizmo.ImVec4C 8abb303168503ec8cef627bfcc439a1bb226c610ec5940b90597ed9a6bfc81af -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorChar a5e715ba786e0d37abfce93894b2f54a397a947105018dc0e82225a393d6c184 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorFloat 8ef06e741bcf03418f01aba2e1f1306fae897c614d665e7dfeca8f883f270f97 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawChannel aeb964832347225aecd178f5212e2c38728f9495f82aa6fc5801a83dc207e84e -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawCmd 280fd95286402d4f111cd197bab54cfb006efc3fe5bc953c4bf1f11bd3062651 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawIdx bf7636199dd20f57205723bc07c4b98ad911cb1e6ffb35c352442c640722eacf -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawListPtr c875f6d1482dcb78efd7c1e4f1cd1c6f87556fd2cab467f095fcb8d31887386e -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr b0ffdc7266515fbbb1672113e85551fde611b29fb6d36e3bbd38185ac2ccff25 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImDrawVert a228f7039d059ca83380db8ac5ea52beecd3d7bbf774aa13152819e676a7669f -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr 8e2ffcb70096a5d808779f72d05b8fc941fc7937d929235f8bb76331ade56b99 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry 2c7320796b63cd86563413274f0d310800c107c42a97d6e95dbfc4d7a886b084 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontBakedPtr 4fab0fdc32236b2b97efad0b8c2ae687b96455cd5b061ae447eeb2ba314fece0 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontConfig 4c7b1d1b278f9e1f8ea9a4cf24af56db1dee7f45ad8f627e88ea738c6990f010 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontConfigPtr 75f1e890d00aa7306bfa5778034bd19bc72c554148365d248c939a34fa914d47 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontGlyph 31ca98ce1949f206aa8bfb5f46a43ce817457d5cd0d764da551a78291f4ce5a0 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontPtr fe4b3df2ced9b39c13b6cf58eca639dd5821eff0b83c080276085ae57fb3ff33 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImFontStackData 8871e6ef0766b69a3b7147adec647a8cd7644736396fb96c7971efc83bd62ea9 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiColorMod 5cfc76c9d21a7ed1a2a4740a7783847f1a02b0aee79658c90778706f8dbc74aa -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiContextHook a932ea217138630853ae7a00b0bfec0f2a85aaa722b17e4717ee1c1c8bb82954 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings 143aa5c72a2d119c16d3880bbbcb007bcd716f90a5721e838b16a09fb98d7d98 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiDockRequest 47c74245f35ee7af59945af465b3eef9df114486c5c07fcdc131a11963c06bdc -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData 33c93979ebe26fd0225d5104725861993ed411ba7ab5bdc7df9ac75233150b77 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiGroupData ea4e4335247b683b54b8f2ca35882f1f7cdeb14983d945ebea06824d7bd6f037 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiID f28ba941fb3c3d32a54114484b01d7c2bb75dfa8987254faef60e1905196eccb -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiInputEvent 43fa0fa6334ebc491610cadfa7f0f805651e9c82e9f06ee17606b37ababfca60 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiItemFlags 3bccf80a30c9a71f13008728c8b0d28d4e85f573e1a9481251cb3e5191552512 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData 29910fcb12620c3da4e79c65f72224de8ca1300605b8c0f021abd70b9f9904fd -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiListClipperData 453564efb16d369645b01b3b34c4747259b73dd34c98b702b4b707f7beda777f -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange ae3707f8870a565862a0748a49ff1e8bdfdd4ecf6a37e2629299f05c7c629d93 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState c744745ac0b2151dd2940a636ae6a32b392c732e0fb529690300e4abcf3de6dc -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData e856ac6de8b441827d062e6824822514803faba369bcadfa8fd21885bdaec2f9 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData 0c6a2648d16b4f7cb77a97a56405dc676a3453e46cb634a829682028909ae123 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiOldColumns e6132acc94e33e5a2e9688f66f8153bf4708169ee3dbf143032bd62b5b9568d9 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor 1ae706831108fec08e885c71b8dd12b4188a2af9f1432485af114bea0d78edd7 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiPopupData a47f487febea31020a543ae12aafd7f9a3bad1a53e554086d9913fd3b98b85f1 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex 3dd8d333a1145d9ec144b828964218c71810c0840067a60ad5da9554f2f20dd2 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest a2e69302a76713af819d2521694218be60fae30dedacedaaaafb47b3c0d61027 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler 19a9974188b2e73887314f058e78829ef4a2e993d0a862fdb9cd53fa33750d7c -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem 365803dc9787e119b80b974ffe58c4030459cec9a37d9efa06c43ff170b6daae -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo ad30144d575fb481634575e7999dba432d2ef625da76600ff3836f8c3fe68341 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiStoragePair 56496509ddeaa4fbd3b8ca0e80f2e83a52c64703960c7780348b892dd40746db -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiStyleMod 9b0e0a40550081772d4cf30b3d3048b233046c76c94ae5ebd4202e1c5fbf129c -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTabBar 3ade0c3dbf161ad8cef9357fb102ae2a276be14f147517be18f4361fa03e99ca -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTabItem e59105d06fbe3ccf9fa5dce279b182c45ed73edb4133710ce63e8fa627234411 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTable 901d71b56d2e4ad2e2d5267bda788839c509973fe22255a1a59f68ca31b8aba8 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs 3c9909d61f0fb20a2b2e7aaf52cfb322f26c85ae343a4e7bd03761731ad1b732 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData 5019f13a6588d49bc86419fbe21ff31b7166b77e8b10387b00a071922c4b93a4 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData 35b465a8ef33767b61e923ce16e57d6a20938388156438401a09b15bf0b4ff8c -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTableTempData 1cabf9641ff4164ae6c2e25e7f8c7ef2b80253afe9e2b8c78387f4aabb9b7f33 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData c9a237dd1e7fb7c1da1a5317b05cef1866a06d93cbe0639c23a93c4dec698525 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr 4357a721c22223767431cf465ab116ece9e4aad087c2dd1145bd9044efccaaff -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr 46e1a52321f6c04035feb7eb3a90d41ec6ecdbf6201720e01a8eb0de690acd72 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr 4aecd472ae20b4bd410fc199e940fd56725c3b37efae5c1a91e99f36e96500b3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData a6ce4e8317e0b37950293e961956f454ff7b90bc9922833165649c578e13d694 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImTextureDataPtr 21ab86d542a625c754220228c879e1d37ea02a5d4b556bf49d944429969b9cd3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImTextureRect 9deb6c522fd602b641f47532e99d1d2bd56ba24a604c6d0d642198622fd287ea -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImTextureRef e336ace952b2892cc60c1bf245332f98df80250123ac7c44a354156ce96a5da3 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImU16 577efcd802ad5586885b1478269cc6a433241e1893aaef5ea284f3cba7cd9b60 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImU8 ece5404b1f65a2e42b143650b79ed4f2aa2092a0ff5bcc6d7663de7797e2ea94 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImVec2 27fa893057644066c2a6237c8cf28ef710345ac715ea3ed061eb29766bb6c50b -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImVec4 17edaf93c87a783c150efee0a25c22bdb1dfbcb4e4fb8d5be539b1fe1d35c964 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorImWchar 90e8e4839b5c09c577cc3c3683a299ffb689d8f69951199caec9553bea141eae -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorInt e8700714077b3d7fa71552d063d96768f44579bf0c3fea213148419321bae864 -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorStbrpNodeIm 09111d2ceabe1d408de8d30a755dbd99466294643075497d627b8f8d905bf95d -->
<!-- api: T:Inno.Native.ImGuizmo.ImVectorUnsignedChar f0c53284f3d8cf9658d816c3316a959d598aa4a79cdce3dbe561136ec696137a -->
<!-- api: T:Inno.Native.ImGuizmo.Iobuf 86ade60aacf96b678a52d8a0154455d2c11832e1ba2c5b6411ec0a9dc61e7568 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_16a683b69e90f4cd c9888baf27d64588b56f467aebaf5a33ccd26c104e011ccb0f949b1023f00c9c -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_1eccc798bd000925 38cd2754285e0018b808bbd87b973fa4047150e926653d31362a192fd725c70c -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_3325e8ed3a9a98ba 2d553349c53302106ee2705af841137c4a3a35c2a25f61625bd0e3d6770c5046 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_332d40ffc044d296 b824bc623a6f28e5473018b58498deb07d7e419d4b9d633987966f32c7ec0edb -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_3571fc580e8d9e55 151c9aa86a390c475f534a4b5693c876402f72e68926341b12b270dfc4856f4d -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_35aebb639e6eb330 8f2570b06024035683faf0f3e905e4c3ed62dc1e6df9b207b1dd11b2857660d7 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_3d337b926fc04643 c66f01a41ba16ae62e3835a2114df1def740403890926dd9e8c12fbcd419784a -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_41ea187b8e0849de b64d7d7c8158f4f43198467295778045719366bcf1b2c2d842f0b36e4d8dcc3b -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_42c049e3f15c43ec 05070a3ad81f696b16640ba2eec8aecb009645d18601a956e36ad9cac39e8ab0 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_43db0ce246f18b3f e31ae5a634bacf8d48248fe705593cdc4bf2b704161c184c9c39eee1738b8ad8 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_4a44fb190803b3be d413ae3eaa834f6e8839622cc77329690951e11800581d6865db256967d03773 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_5caf1d5dfc8a2e78 7b415350d1c95fa5a56273cdb5dc1c00b5ab6833b406c6d006c7c32d08cd7e97 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 60578726fb04c947e4373fe43f5f44609d4658307f9822ea8fa45638ec017456 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_605a5c71dc256950 6dc731df8e17e75b3e50256cb126b810f74dffd63c0f81ec2bd6c2b2e90f4bb5 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_68e25b300381a6fc 58d227044229ad994c2a44ae25fa710d7a949eec13d623d7fbc25edae50d7f49 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_704527fa6c941baa 4f460efbfd81cb1c6d68eeb764f714961075159c8ded71fd41ba7fbc828d8f2e -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_76771c8c9ab15473 e41a989e69881a055add0d3bd91796acc7f2323dbd33dbc892700aa524ce6d46 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_7b9f5e5665cb58c5 c24145e100c297a0d303b608ceb158133eb16167424ce54470c28ab730d1984f -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_87beacaa77c69f9d 2fe439eca6e552584bde1fd9daaebfa2cef52bc30c02c887a9ef3f61e648d9bc -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_8da6be2ffdb0de05 0b20a24655c7bc85af317e717da36e35e2284cbe4ed9f9b6a59e7292c75828ce -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_9be1bbaad6627fe8 92277e8d706cfd024ac2994005c3b433a65c1d4999e10f72ff89f3745847ac65 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_a4748c2e54d124a7 c343dd58a0aa6f7d081f223f1c57f861d81ee8050c251e12927a9c5c879939ce -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_afa9ec5b5680f876 382fc4386769150f481685ae707af956829ed7f8c29ba6d78dea87a507f61d64 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_b3004bc9c15942c6 ba347ccef016ab716e7fdac9d2478c0583baf24685bd19373070cfdf29fc3a90 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_b726fff0e8801db7 5c0de88f725ee48bff7a35f85878579e5eee6f16da0bc2d0e24969fa7f41ac77 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_bae28ecfe547f5d7 5d5bc967f21f00593c5319a8638438f9ca3da9e857e1edc20c24c1d77b1b3da3 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_c34fe7f9f3db1a4e 54ef713ed19a67c844727c6fdd4e529872ceca59b3599333a10166c060bae146 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_d36642d3104a6c6f e20afd6c60d80119af49e2cc0374c23c1ec2b28eefeb1fc1a73bb9262cb81d42 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_df98efe8482c9a26 2b6295fe5b19a28afc061f162d3b7904061dad90bb2fa9837f6b96f7d35889fd -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_e60e879954c840fa 890b14813d58cffe80c25f3cd38a29672cd3948cd63a9d23e8776b01d9e3f263 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_ec6e6e3c26b88eb8 f5981352fd86806a4639093ce5696e0218887175871f3a450b62593e8eca6919 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_f3ea9f13691638c5 f4f471d35e3e7345c086d3d9df1fbf6ddc2940a0119ac4ff0240c84bd50dbf78 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_f4284eefd115ca43 e5a915565308b7f3b03d27af484bb75cd5ccadcca4fa2bdba9abb34d127f8b7f -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_f9c2e11bc01cc26a 1c4b2b0be329d394b9b2d41162ab528751c90e59008fab5d7895368de2e0c3f5 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_fcecdbb15210e44b 6187f85426dc089eedad19b9a06af6058842383905778a3f8bbad6f3bf8bf796 -->
<!-- api: T:Inno.Native.ImGuizmo.NativeArray_ff485db19a16ca26 d75ee3df815e45eafd9d10ad354de7bfdacc48bc5680d23c2ee14e4b3ba9c04a -->
<!-- api: T:Inno.Native.ImGuizmo.STBTexteditState 6f04f75d0498b4e698e15963167ff1138f3dcf117437d298084ad4815981232c -->
<!-- api: T:Inno.Native.ImGuizmo.StbrpContextOpaque eb5e481b481166f211ac60474dba570bee941f4c232a1f85b97dd04811d7aac4 -->
<!-- api: T:Inno.Native.ImGuizmo.StbrpNode 563030353863ae22108d2729becdee6f1ec40c4afb25b524f38e86d4ce0766c8 -->
<!-- api: T:Inno.Native.ImGuizmo.Style 9f07e65b24c1ad8b3405ee5207f8642f01a94de562c9302422541ffc6c4bc86f -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-57328b0ecda94570"></a>[`public Inno.Native.ImGuizmo.NativeArray_3325e8ed3a9a98ba Inno.Native.ImGuizmo.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d708a4c8778a97ea"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImChunkStreamImGuiTableSettings.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5180) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c065cdddba7345fb"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImChunkStreamImGuiWindowSettings.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5198) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a0edbdd4a9c9642"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawCmd Inno.Native.ImGuizmo.ImDrawChannel.CmdBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46a1694fef08ad35"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawIdx Inno.Native.ImGuizmo.ImDrawChannel.IdxBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92f78417b19d50d2"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImDrawCmd.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5242) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61dd911723f6a85c"></a>[`public uint Inno.Native.ImGuizmo.ImDrawCmd.ElemCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59bde9745fd244de"></a>[`public uint Inno.Native.ImGuizmo.ImDrawCmd.IdxOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-44d79674bda9f877"></a>[`public Inno.Native.ImGuizmo.ImTextureRefC Inno.Native.ImGuizmo.ImDrawCmd.TexRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5244) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8a6cd2fc007bf62"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImDrawList*, Inno.Native.ImGuizmo.ImDrawCmd*, void> Inno.Native.ImGuizmo.ImDrawCmd.UserCallback`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6477da2aef15857"></a>[`public void* Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db732083b62106bf"></a>[`public int Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackDataOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42d6d6656f75368c"></a>[`public int Inno.Native.ImGuizmo.ImDrawCmd.UserCallbackDataSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-741d6f9740135d45"></a>[`public uint Inno.Native.ImGuizmo.ImDrawCmd.VtxOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43b90d4870989039"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImDrawCmdHeader.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5276) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7234cac12f3fb723"></a>[`public Inno.Native.ImGuizmo.ImTextureRefC Inno.Native.ImGuizmo.ImDrawCmdHeader.TexRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5278) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-554c2d558524559a"></a>[`public uint Inno.Native.ImGuizmo.ImDrawCmdHeader.VtxOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-52ad623afe3dc9ac"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawListPtr Inno.Native.ImGuizmo.ImDrawData.CmdLists`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5306) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c11de8fd1e83ca9b"></a>[`public int Inno.Native.ImGuizmo.ImDrawData.CmdListsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2f319ec68b57811"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawData.DisplayPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5308) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ba22dd1a1ef32b3"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawData.DisplaySize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9cea411524f0c9f7"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawData.FramebufferScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b6d459d943c7e7d"></a>[`public Inno.Native.ImGuizmo.ImGuiViewport* Inno.Native.ImGuizmo.ImDrawData.OwnerViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10edcd62c19d4ea4"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureDataPtr* Inno.Native.ImGuizmo.ImDrawData.Textures`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5316) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eeaab64efef62d9a"></a>[`public int Inno.Native.ImGuizmo.ImDrawData.TotalIdxCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e6c3c121ec1ce8a5"></a>[`public int Inno.Native.ImGuizmo.ImDrawData.TotalVtxCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5304) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0acfbcef53c988f"></a>[`public byte Inno.Native.ImGuizmo.ImDrawData.Valid`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5298) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95c33a0aab33a889"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawListPtr Inno.Native.ImGuizmo.ImDrawDataBuilder.LayerData1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5336) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e6071bd423f347f"></a>[`public Inno.Native.ImGuizmo.NativeArray_7b9f5e5665cb58c5 Inno.Native.ImGuizmo.ImDrawDataBuilder.Layers`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-73132b93fa88cfc8"></a>[`public Inno.Native.ImGuizmo.ImVectorImU8 Inno.Native.ImGuizmo.ImDrawList.CallbacksDataBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-23dfddfd33226853"></a>[`public Inno.Native.ImGuizmo.ImVectorImVec4 Inno.Native.ImGuizmo.ImDrawList.ClipRectStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48de7095fc2d68e9"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawCmd Inno.Native.ImGuizmo.ImDrawList.CmdBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-528b794fafb47d96"></a>[`public Inno.Native.ImGuizmo.ImDrawCmdHeader Inno.Native.ImGuizmo.ImDrawList.CmdHeader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fe05a45d74db4d4"></a>[`public Inno.Native.ImGuizmo.ImDrawListSharedData* Inno.Native.ImGuizmo.ImDrawList.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8dcb1ed78664eca"></a>[`public int Inno.Native.ImGuizmo.ImDrawList.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2e74683dd96f75d"></a>[`public float Inno.Native.ImGuizmo.ImDrawList.FringeScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c67de3ca9e6b4f44"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawIdx Inno.Native.ImGuizmo.ImDrawList.IdxBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-36b869b825c6dc59"></a>[`public ushort* Inno.Native.ImGuizmo.ImDrawList.IdxWritePtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aea1a4c7d53b4d78"></a>[`public sbyte* Inno.Native.ImGuizmo.ImDrawList.OwnerName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9025d266328773b"></a>[`public Inno.Native.ImGuizmo.ImVectorImVec2 Inno.Native.ImGuizmo.ImDrawList.Path`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8991fd85f2ede8dd"></a>[`public Inno.Native.ImGuizmo.ImDrawListSplitter Inno.Native.ImGuizmo.ImDrawList.Splitter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1deaa6e322958c19"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureRef Inno.Native.ImGuizmo.ImDrawList.TextureStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e569203b8bd5116"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawVert Inno.Native.ImGuizmo.ImDrawList.VtxBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d87e75110eac188c"></a>[`public uint Inno.Native.ImGuizmo.ImDrawList.VtxCurrentIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5362) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87d37c2a784cb2e1"></a>[`public Inno.Native.ImGuizmo.ImDrawVert* Inno.Native.ImGuizmo.ImDrawList.VtxWritePtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e2aba833a84cc1a1"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.ArcFastRadiusCutoff`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-685f3fcb4675d0b6"></a>[`public Inno.Native.ImGuizmo.NativeArray_f3ea9f13691638c5 Inno.Native.ImGuizmo.ImDrawListSharedData.ArcFastVtx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0af207de0ad7b94"></a>[`public Inno.Native.ImGuizmo.NativeArray_332d40ffc044d296 Inno.Native.ImGuizmo.ImDrawListSharedData.CircleSegmentCounts`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0f92744168fd1d9"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.CircleSegmentMaxError`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d22901fe0e2c913d"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImDrawListSharedData.ClipRectFullscreen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77da1f2719834ae9"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImDrawListSharedData.Context`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b74c90f6aa570ef"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.CurveTessellationTol`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f477ea7b95572247"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawListPtr Inno.Native.ImGuizmo.ImDrawListSharedData.DrawLists`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18fcca0dedeaccff"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImDrawListSharedData.Font`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c9a7657e4c1bc36"></a>[`public Inno.Native.ImGuizmo.ImFontAtlas* Inno.Native.ImGuizmo.ImDrawListSharedData.FontAtlas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25e754acabc1d906"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.FontScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1185473ef28d4ee"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.FontSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bbb1ff66e7ab311b"></a>[`public int Inno.Native.ImGuizmo.ImDrawListSharedData.InitialFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02bdc22d0605e5b6"></a>[`public float Inno.Native.ImGuizmo.ImDrawListSharedData.InitialFringeScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5418) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32f02e12e62caa58"></a>[`public Inno.Native.ImGuizmo.ImVectorImVec2 Inno.Native.ImGuizmo.ImDrawListSharedData.TempBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a2b2adf50e56d70"></a>[`public Inno.Native.ImGuizmo.ImVec4C* Inno.Native.ImGuizmo.ImDrawListSharedData.TexUvLines`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c22982304a41fa67"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawListSharedData.TexUvWhitePixel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90b9ef681f3a6cb1"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawChannel Inno.Native.ImGuizmo.ImDrawListSplitter.Channels`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fb249a02fb5c55b"></a>[`public int Inno.Native.ImGuizmo.ImDrawListSplitter.Count`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28a1dfcff8bb8029"></a>[`public int Inno.Native.ImGuizmo.ImDrawListSplitter.Current`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8139c6a59081ba29"></a>[`public uint Inno.Native.ImGuizmo.ImDrawVert.Col`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15a011f65a073c70"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawVert.Pos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08439e409498e12f"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImDrawVert.Uv`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3059ca11134c59bd"></a>[`public float Inno.Native.ImGuizmo.ImFont.CurrentRasterizerDensity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e951dd140170a31a"></a>[`public byte Inno.Native.ImGuizmo.ImFont.EllipsisAutoBake`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd38c50df23fcce2"></a>[`public ushort Inno.Native.ImGuizmo.ImFont.EllipsisChar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e32719d91580d83"></a>[`public ushort Inno.Native.ImGuizmo.ImFont.FallbackChar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84031da0971b913c"></a>[`public int Inno.Native.ImGuizmo.ImFont.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b533339f05c63297"></a>[`public uint Inno.Native.ImGuizmo.ImFont.FontId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-815928a652565be1"></a>[`public Inno.Native.ImGuizmo.ImFontBaked* Inno.Native.ImGuizmo.ImFont.LastBaked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79bba75e9c3bc7f9"></a>[`public float Inno.Native.ImGuizmo.ImFont.LegacySize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f96c6eac8f76cb32"></a>[`public Inno.Native.ImGuizmo.ImFontAtlas* Inno.Native.ImGuizmo.ImFont.OwnerAtlas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5b49f6b8effb240"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImFont.RemapPairs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5518) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-62c07b27e7da522b"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontConfigPtr Inno.Native.ImGuizmo.ImFont.Sources`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5508) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-50d0dd2b12a30dfc"></a>[`public Inno.Native.ImGuizmo.NativeArray_41ea187b8e0849de Inno.Native.ImGuizmo.ImFont.Used8kPagesMap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5738c0d7a3be90b4"></a>[`public Inno.Native.ImGuizmo.ImFontAtlasBuilder* Inno.Native.ImGuizmo.ImFontAtlas.Builder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6dfda9e09344fb0a"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr Inno.Native.ImGuizmo.ImFontAtlas.DrawListSharedDatas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a4e7149a70e1a01f"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5536) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f985e5e56c5ee59"></a>[`public Inno.Native.ImGuizmo.ImFontLoader* Inno.Native.ImGuizmo.ImFontAtlas.FontLoader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c656b3f2c66471e"></a>[`public void* Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5588) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d0b69beae9f67e9"></a>[`public uint Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-502ef8c474b04919"></a>[`public sbyte* Inno.Native.ImGuizmo.ImFontAtlas.FontLoaderName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8bb87513b1a91a8"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.FontNextUniqueId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5578) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ff35969b58136d9"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontPtr Inno.Native.ImGuizmo.ImFontAtlas.Fonts`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c03289b8ea6ee4e"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlas.Locked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afd956fb30e8b4c5"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImFontAtlas.OwnerContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5594) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccaf35277c6cfc40"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.RefCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3d8d8fc52ba2bfd"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlas.RendererHasTextures`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c982f44c47f9a6e"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontConfig Inno.Native.ImGuizmo.ImFontAtlas.Sources`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20b4b1ca51aa8d49"></a>[`public Inno.Native.ImGuizmo.ImTextureData* Inno.Native.ImGuizmo.ImFontAtlas.TexData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5554) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84c28c0388b08915"></a>[`public Inno.Native.ImGuizmo.ImTextureFormat Inno.Native.ImGuizmo.ImFontAtlas.TexDesiredFormat`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a4352058bb1dc122"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexGlyphPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5540) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0fc035e57c3767b9"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlas.TexIsBuilt`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-189544bea474830d"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureDataPtr Inno.Native.ImGuizmo.ImFontAtlas.TexList`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-997834b37da9c5b3"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexMaxHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9aa75ed2b6f8828"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexMaxWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67088565940cfd4e"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexMinHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbe8e9cac21737b6"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexMinWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10f061078c886dd7"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlas.TexNextUniqueId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5576) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4342e7d88bd883e4"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlas.TexPixelsUseColors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0b14ef64ce47940"></a>[`public Inno.Native.ImGuizmo.ImTextureRefC Inno.Native.ImGuizmo.ImFontAtlas.TexRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-106924dd931f1412"></a>[`public Inno.Native.ImGuizmo.NativeArray_43db0ce246f18b3f Inno.Native.ImGuizmo.ImFontAtlas.TexUvLines`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8767ab077f505cd"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImFontAtlas.TexUvScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee87ee62bef52607"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImFontAtlas.TexUvWhitePixel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e755a594d007f000"></a>[`public void* Inno.Native.ImGuizmo.ImFontAtlas.UserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8801c401d4fa39e1"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedDiscardedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fb1d0ee799d0ee9c"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedMap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3279991eac50e3ae"></a>[`public Inno.Native.ImGuizmo.ImStableVectorImFontBaked32 Inno.Native.ImGuizmo.ImFontAtlasBuilder.BakedPool`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5642) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b4fa6e7e2b19589"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.FrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79ec421df4345b8d"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlasBuilder.LockDisableResize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5638) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06852f5dadc88d0a"></a>[`public Inno.Native.ImGuizmo.ImVec2iC Inno.Native.ImGuizmo.ImFontAtlasBuilder.MaxRectBounds`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5636) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-faf15ea300b591bc"></a>[`public Inno.Native.ImGuizmo.ImVec2iC Inno.Native.ImGuizmo.ImFontAtlasBuilder.MaxRectSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28b8ce8ae932012d"></a>[`public Inno.Native.ImGuizmo.StbrpContextOpaque Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61dbe29a019772ab"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackIdLinesTexData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5650) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0e721e067806343"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackIdMouseCursors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b83820a9782b8048"></a>[`public Inno.Native.ImGuizmo.ImVectorStbrpNodeIm Inno.Native.ImGuizmo.ImFontAtlasBuilder.PackNodes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a4f3edd10c7d5ccc"></a>[`public byte Inno.Native.ImGuizmo.ImFontAtlasBuilder.PreloadedAllGlyphsRanges`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5640) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b47854854cd954cb"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureRect Inno.Native.ImGuizmo.ImFontAtlasBuilder.Rects`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6bc45e0a5cd0a7fc"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsDiscardedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc7a1b5847414d19"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsDiscardedSurface`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5630) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8862f32501b142b1"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ed80d7c95019271"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsIndexFreeListStart`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71a6e89f1b516d77"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsPackedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5624) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-972e477e7f90be92"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasBuilder.RectsPackedSurface`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5626) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da0ebfed21fc9bee"></a>[`public Inno.Native.ImGuizmo.ImVectorUnsignedChar Inno.Native.ImGuizmo.ImFontAtlasBuilder.TempBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5620) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db02fd0dac6dd361"></a>[`public float Inno.Native.ImGuizmo.ImFontBaked.Ascent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5790) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c65c7331ecb77d28"></a>[`public uint Inno.Native.ImGuizmo.ImFontBaked.BakedId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5796) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29bab080aa5454e0"></a>[`public float Inno.Native.ImGuizmo.ImFontBaked.Descent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5792) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3d775a2e7cb1df6"></a>[`public float Inno.Native.ImGuizmo.ImFontBaked.FallbackAdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5778) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77d91ad8c4724fb7"></a>[`public int Inno.Native.ImGuizmo.ImFontBaked.FallbackGlyphIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5788) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff12278f80cf20fa"></a>[`public void* Inno.Native.ImGuizmo.ImFontBaked.FontLoaderDatas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5800) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95640c674ae28abf"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontGlyph Inno.Native.ImGuizmo.ImFontBaked.Glyphs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5786) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25ab496dffd75ae2"></a>[`public Inno.Native.ImGuizmo.ImVectorFloat Inno.Native.ImGuizmo.ImFontBaked.IndexAdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5776) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afe225145bf5b047"></a>[`public Inno.Native.ImGuizmo.ImVectorImU16 Inno.Native.ImGuizmo.ImFontBaked.IndexLookup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5784) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-167dd42391f77b05"></a>[`public int Inno.Native.ImGuizmo.ImFontBaked.LastUsedFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5794) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f817a2775214db3"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImFontBaked.OwnerFont`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5798) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2d9acd785b92ed7"></a>[`public float Inno.Native.ImGuizmo.ImFontBaked.RasterizerDensity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb9355147cec790f"></a>[`public float Inno.Native.ImGuizmo.ImFontBaked.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d16ebd6ed5e0f0d7"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImFontConfig.DstFont`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cba5180d41bf2e8e"></a>[`public ushort Inno.Native.ImGuizmo.ImFontConfig.EllipsisChar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5954) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc65f13ce0bc8272"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.ExtraSizeScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5978) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae8c5bc127dff3e7"></a>[`public int Inno.Native.ImGuizmo.ImFontConfig.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5dfca80af69ae347"></a>[`public void* Inno.Native.ImGuizmo.ImFontConfig.FontData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5940) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86065eca7055711b"></a>[`public byte Inno.Native.ImGuizmo.ImFontConfig.FontDataOwnedByAtlas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c526cc66caa5c9f"></a>[`public int Inno.Native.ImGuizmo.ImFontConfig.FontDataSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1eadf439a8d437f6"></a>[`public Inno.Native.ImGuizmo.ImFontLoader* Inno.Native.ImGuizmo.ImFontConfig.FontLoader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5984) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58d2232e582aec2c"></a>[`public void* Inno.Native.ImGuizmo.ImFontConfig.FontLoaderData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5986) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0eab020f1e54fdf6"></a>[`public uint Inno.Native.ImGuizmo.ImFontConfig.FontLoaderFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3af338ac28c3264b"></a>[`public uint Inno.Native.ImGuizmo.ImFontConfig.FontNo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5970) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b03d68066d5f00f8"></a>[`public ushort* Inno.Native.ImGuizmo.ImFontConfig.GlyphExcludeRanges`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecf0a87bea3e31ed"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.GlyphExtraAdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5968) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b13bf9282ed58d4"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.GlyphMaxAdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5966) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91814f55081e95c2"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.GlyphMinAdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5964) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77ccb286b30a1adc"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImFontConfig.GlyphOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5962) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a524dfb5f4a27fd"></a>[`public ushort* Inno.Native.ImGuizmo.ImFontConfig.GlyphRanges`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5958) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ef6ee0c509476a6"></a>[`public byte Inno.Native.ImGuizmo.ImFontConfig.MergeMode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5946) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e45f5ee160131bb"></a>[`public Inno.Native.ImGuizmo.NativeArray_d36642d3104a6c6f Inno.Native.ImGuizmo.ImFontConfig.Name`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5938) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb98d26eb4ee6476"></a>[`public sbyte Inno.Native.ImGuizmo.ImFontConfig.OversampleH`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5950) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea1575438432cb44"></a>[`public sbyte Inno.Native.ImGuizmo.ImFontConfig.OversampleV`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5952) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c679e9bd56db9f3e"></a>[`public byte Inno.Native.ImGuizmo.ImFontConfig.PixelSnapH`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5948) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-34ad0f529b2e1687"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.RasterizerDensity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8982d92233ab7b06"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.RasterizerMultiply`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ed75e2e281581d2"></a>[`public float Inno.Native.ImGuizmo.ImFontConfig.SizePixels`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a23d7170d2cf1e8"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.AdvanceX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6006) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c58d59aabfb19ba"></a>[`public int Inno.Native.ImGuizmo.ImFontGlyph.PackId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6024) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75ffdbf24acf619b"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.U0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6016) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa0a8160c22da2e4"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.U1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6020) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-163d6c022d8eacb4"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.V0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6018) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6bc476c4173a5b73"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.V1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6022) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f19f1086147e8dd"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.X0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6008) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd847d7ff755d8a8"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.X1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6012) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbea145c0b017165"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.Y0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6010) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-47b82382f17cc23e"></a>[`public float Inno.Native.ImGuizmo.ImFontGlyph.Y1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6fad03bd781b5864"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, Inno.Native.ImGuizmo.ImFontBaked*, void*, void> Inno.Native.ImGuizmo.ImFontLoader.FontBakedDestroy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6176) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07eb01895056a9a4"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, Inno.Native.ImGuizmo.ImFontBaked*, void*, byte> Inno.Native.ImGuizmo.ImFontLoader.FontBakedInit`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6174) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7cdf6dd140ff1d3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, Inno.Native.ImGuizmo.ImFontBaked*, void*, ushort, Inno.Native.ImGuizmo.ImFontGlyph*, float*, byte> Inno.Native.ImGuizmo.ImFontLoader.FontBakedLoadGlyph`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6178) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aad83c8606d625a8"></a>[`public ulong Inno.Native.ImGuizmo.ImFontLoader.FontBakedSrcLoaderDataSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6180) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0526cd8bdfde11d3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, ushort, byte> Inno.Native.ImGuizmo.ImFontLoader.FontSrcContainsGlyph`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6172) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-731307f5889360fd"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, void> Inno.Native.ImGuizmo.ImFontLoader.FontSrcDestroy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6170) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afdaeb3190135f20"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, Inno.Native.ImGuizmo.ImFontConfig*, byte> Inno.Native.ImGuizmo.ImFontLoader.FontSrcInit`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d990375c82c84dd0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, byte> Inno.Native.ImGuizmo.ImFontLoader.LoaderInit`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6164) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0aa8b7668ce4b9d7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImFontAtlas*, void> Inno.Native.ImGuizmo.ImFontLoader.LoaderShutdown`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b231101c11995c4"></a>[`public sbyte* Inno.Native.ImGuizmo.ImFontLoader.Name`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecab1c61f141f5cb"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImFontStackData.Font`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6198) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dca2de0f8da036f"></a>[`public float Inno.Native.ImGuizmo.ImFontStackData.FontSizeAfterScaling`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6202) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5cf7d947d9d9e35b"></a>[`public float Inno.Native.ImGuizmo.ImFontStackData.FontSizeBeforeScaling`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6200) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3a73e1c9fcf9962"></a>[`Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisNone = -1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6217) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-f83cb85dc4ba2dc5"></a>[`Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisX = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6218) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-df172e8c7b5bb890"></a>[`Inno.Native.ImGuizmo.ImGuiAxis.ImGuiAxisY = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-5001c597c96acf00"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiBoxSelectState.BoxSelectRectCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fdfe553ab83c230"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiBoxSelectState.BoxSelectRectPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e31657628f7013f4"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiBoxSelectState.EndPosRel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46771201f9a88fd2"></a>[`public uint Inno.Native.ImGuizmo.ImGuiBoxSelectState.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6232) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b450c2fbcfcb6b5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6234) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa67b3dec8938207"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStartedFromVoid`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3d3fc6b6efe3715"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStartedSetNavIdOnce`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6240) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba35d36ba3c9fda3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.IsStarting`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6236) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fdd0f8a8398f3b7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.RequestClear`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6242) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2bb4f59e58bd7b11"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiBoxSelectState.ScrollAccum`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9279ea991cdc2832"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiBoxSelectState.StartPosRel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6244) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e806c55732590c3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiBoxSelectState.UnclipMode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-906a4da4a7b1bd86"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiBoxSelectState.UnclipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39442b50828fae00"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiBoxSelectState.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ae9c29d55fd649f"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImGuiColorMod.BackupValue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06938b3c04ee60b6"></a>[`public int Inno.Native.ImGuizmo.ImGuiColorMod.Col`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6a909088c9635cab"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6324) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3eb87cd3bb965e5b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-010ba0e8ef12764c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupCursorPosPrevLine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e99da7e3c64cd0e8"></a>[`public int Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupLayout`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-97eae6f9d9a9b1d8"></a>[`public float Inno.Native.ImGuizmo.ImGuiComboPreviewData.BackupPrevLineTextBaseOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-262cec406880a1f6"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiComboPreviewData.PreviewRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6320) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64a4fe6dd14cce76"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ActiveId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6468) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9bb923dec27ce10"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdAllowOverlap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a57695d633cea1ec"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.ActiveIdClickOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15cefce72f90dda3"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ActiveIdDisabledId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6490) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0016fc37fefcec06"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdFromShortcut`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d2b84975526d2f6"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenEditedBefore`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b0852eb769a91d13"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenEditedThisFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-545d4b4ee06b6221"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdHasBeenPressedBefore`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2559badc2b3c7951"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ActiveIdIsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41056d7ccb56bc92"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdIsJustActivated`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2999aaf28f4e8e0"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdMouseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ceeac853f927190"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdNoClearOnFocusLoss`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4723d4b22afcaf0"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ActiveIdPreviousFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-784ca16699c706d6"></a>[`public Inno.Native.ImGuizmo.ImGuiInputSource Inno.Native.ImGuizmo.ImGuiContext.ActiveIdSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6494) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d13bce166a2fc65"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.ActiveIdTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-954102c32fcb11bc"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ActiveIdUsingAllKeyboardKeys`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6522) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-112b003ab474bc51"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ActiveIdUsingNavDirMask`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-215ef3a8f97d1a55"></a>[`public Inno.Native.ImGuizmo.ImGuiDataTypeStorage Inno.Native.ImGuizmo.ImGuiContext.ActiveIdValueOnActivation`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a78595bc4156315"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.ActiveIdWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac584fb8bedb2f50"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.BeginComboDepth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cff7775d7b3dc3bb"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.BeginMenuDepth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0468242bcca0a0b3"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiPopupData Inno.Native.ImGuizmo.ImGuiContext.BeginPopupStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed1f57532fd6247c"></a>[`public Inno.Native.ImGuizmo.ImGuiBoxSelectState Inno.Native.ImGuizmo.ImGuiContext.BoxSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6774) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e600f59ca239431"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiContext.ClipboardHandlerData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6864) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10bd6d6cda32e32c"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiListClipperData Inno.Native.ImGuizmo.ImGuiContext.ClipperTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6750) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-239304bb65ac82c7"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ClipperTempDataStacked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6748) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e7ba2139664fd8b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ColorEditCurrentId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6822) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a454854e420637ab"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ColorEditOptions`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6820) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-928646541d478063"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54315c5d40eb8b8d"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedHue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6826) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ae9202061732677"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6824) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-26c73c00a3dc6cc3"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.ColorEditSavedSat`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6828) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cfebddec180f7380"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImGuiContext.ColorPickerRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6832) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91e4074c2992f1f4"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiColorMod Inno.Native.ImGuizmo.ImGuiContext.ColorStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b03176d8707eaff"></a>[`public Inno.Native.ImGuizmo.ImGuiComboPreviewData Inno.Native.ImGuizmo.ImGuiContext.ComboPreviewData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6834) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e3b6f010c8dad16"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ConfigFlagsCurrFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59304f926dbc46a0"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ConfigFlagsLastFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-922e7b0208622683"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ConfigNavEnableTabbing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6678) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdd54714feb70e15"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingKeyNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6682) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-514cf7122f65f1a2"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingKeyPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6684) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c48a4d8f9ec03fa0"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ConfigNavWindowingWithGamepad`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6680) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70b15a97211ce55d"></a>[`public Inno.Native.ImGuizmo.NativeArray_e60e879954c840fa Inno.Native.ImGuizmo.ImGuiContext.ContextName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8ef6a29b4e6f18b"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.CurrentDpiScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71552ea461178203"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.CurrentFocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0da4d2000e21191"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.CurrentItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6528) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b75e899966686663"></a>[`public Inno.Native.ImGuizmo.ImGuiMultiSelectTempData* Inno.Native.ImGuizmo.ImGuiContext.CurrentMultiSelect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6776) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3414b72758070489"></a>[`public Inno.Native.ImGuizmo.ImGuiTabBar* Inno.Native.ImGuizmo.ImGuiContext.CurrentTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c880426b9c9e080"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex Inno.Native.ImGuizmo.ImGuiContext.CurrentTabBarStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3fdca3684e0eb51d"></a>[`public Inno.Native.ImGuizmo.ImGuiTable* Inno.Native.ImGuizmo.ImGuiContext.CurrentTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6752) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-730510d2537e435b"></a>[`public Inno.Native.ImGuizmo.ImGuiViewportP* Inno.Native.ImGuizmo.ImGuiContext.CurrentViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5b35f59771f4f2b"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.CurrentWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a46184f1c69c39c"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData Inno.Native.ImGuizmo.ImGuiContext.CurrentWindowStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-acac1a348229e52e"></a>[`public Inno.Native.ImGuizmo.ImGuiDataTypeStorage Inno.Native.ImGuizmo.ImGuiContext.DataTypeZeroValue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6814) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a284fbe7df7632f"></a>[`public Inno.Native.ImGuizmo.ImGuiDeactivatedItemData Inno.Native.ImGuizmo.ImGuiContext.DeactivatedItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f94e4cd4d0fd785f"></a>[`public Inno.Native.ImGuizmo.ImGuiDebugAllocInfo Inno.Native.ImGuizmo.ImGuiContext.DebugAllocInfo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ddb093bc078ee67"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiContext.DebugBeginReturnValueCullDepth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-260519a643751ea4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInLocateId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6952) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1997f6af942d5a8a"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInShortcutRouting`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efd6984a1a39ddfa"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6754) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e1c053df4598477"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugBreakInWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-780052fb3d87937b"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugBreakKeyChord`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6954) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0cded861a893ec36"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugDrawIdConflictsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6936) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce48a7247061aff4"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugDrawIdConflictsId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ee53ab9a4104fb3"></a>[`public Inno.Native.ImGuizmo.ImVec4C Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorBackup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6966) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78781bf39c4c911e"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76e838e89c93412c"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DebugFlashStyleColorTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6964) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75686b74f2343298"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugHookIdInfoId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7dc7a8f73634bbc"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiContext.DebugHoveredDockNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a2df58efa783758"></a>[`public Inno.Native.ImGuizmo.ImGuiIDStackTool Inno.Native.ImGuizmo.ImGuiContext.DebugIdStackTool`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-be1e32cb1a761b2d"></a>[`public Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery Inno.Native.ImGuizmo.ImGuiContext.DebugItemPathQuery`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6970) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2563976fd422a73a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6958) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-373fbd34cc7a3eb2"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerBreakId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6962) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ffa2e63d21b91c43"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugItemPickerMouseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a79151c0e93cb81"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugLocateFrames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6950) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75de99c6cadf98df"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DebugLocateId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6530) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-033aa1b9bc176cc9"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugLogAutoDisableFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6946) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b937dd99cf0c5056"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugLogAutoDisableFrames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6948) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db653deb3ee9af4b"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiContext.DebugLogBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6940) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08146f6b0e33ea5d"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugLogFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6938) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7b1563dfae33e0e"></a>[`public Inno.Native.ImGuizmo.ImGuiTextIndex Inno.Native.ImGuizmo.ImGuiContext.DebugLogIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d896ef256f76b5d5"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DebugLogSkippedErrors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a26194c836ced48"></a>[`public Inno.Native.ImGuizmo.ImGuiMetricsConfig Inno.Native.ImGuizmo.ImGuiContext.DebugMetricsConfig`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6968) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcce9ac47a43e749"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DebugShowGroupRects`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77c6358de2fc5f9b"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DimBgRatio`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6706) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d772155ab20e6e6"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DisabledAlphaBackup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6856) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d64227028ca5ef9"></a>[`public short Inno.Native.ImGuizmo.ImGuiContext.DisabledStackSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff63dca3cc07ae00"></a>[`public Inno.Native.ImGuizmo.ImGuiDockContext Inno.Native.ImGuizmo.ImGuiContext.DockContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6876) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-489ff0c7f4a33bd3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiDockNode*, Inno.Native.ImGuizmo.ImGuiTabBar*, void> Inno.Native.ImGuizmo.ImGuiContext.DockNodeWindowMenuHandler`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6878) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-671b1c4c11b9e9a9"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DragCurrentAccum`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6852) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35b3fabf74f15a76"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DragCurrentAccumDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6850) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2806e35666d754fa"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFlagsCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7804133ee169059"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFlagsPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77813b65793d1732"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-825da2f325f4f55b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd099b9e4024f9d8"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdCurrRectSurface`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b1652861784eaf7"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DragDropAcceptIdPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9bd7291dbdd926ee"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DragDropActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33c697436be1ef6b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DragDropHoldJustPressedId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6742) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f8fd24f414613a18"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropMouseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6718) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b26769808e1161b"></a>[`public Inno.Native.ImGuizmo.ImGuiPayload Inno.Native.ImGuizmo.ImGuiContext.DragDropPayload`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6720) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aef9514a7fb8f5c6"></a>[`public Inno.Native.ImGuizmo.ImVectorUnsignedChar Inno.Native.ImGuizmo.ImGuiContext.DragDropPayloadBufHeap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3daf65c024ce082c"></a>[`public Inno.Native.ImGuizmo.NativeArray_a4748c2e54d124a7 Inno.Native.ImGuizmo.ImGuiContext.DragDropPayloadBufLocal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6746) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6770aaa2a3a018c6"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropSourceFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6714) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e627072638f3de9"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.DragDropSourceFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6716) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d1c30e7c8b3a10b"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6724) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a39c432bbad79f4d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetFullViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c12d188e5d292708"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32a8a5f3e66f1ad4"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.DragDropTargetRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6722) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ed8a1599424f356"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DragDropWithinSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6710) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d8e356d9f5cc575"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.DragDropWithinTarget`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6712) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4a2c955f5df1bce"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.DragSpeedDefaultRatio`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6854) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-785a1079a1322e8a"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawChannel Inno.Native.ImGuizmo.ImGuiContext.DrawChannelsTempMergeBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb58acc0737ace54"></a>[`public Inno.Native.ImGuizmo.ImDrawListSharedData Inno.Native.ImGuizmo.ImGuiContext.DrawListSharedData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8102ceb4cdcdbe09"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, void*, sbyte*, void> Inno.Native.ImGuizmo.ImGuiContext.ErrorCallback`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6922) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9ea45e517b76900"></a>[`public void* Inno.Native.ImGuizmo.ImGuiContext.ErrorCallbackUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6924) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17bcda85fc2649f4"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ErrorCountCurrentFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6930) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e000264bd46164c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ErrorFirst`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6928) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-487486b66e195357"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.ErrorTooltipLockedPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6926) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1656c55c51536a26"></a>[`public Inno.Native.ImGuizmo.ImGuiPlatformMonitor Inno.Native.ImGuizmo.ImGuiContext.FallbackMonitor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b0de75aac53fed8"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData Inno.Native.ImGuizmo.ImGuiContext.FocusScopeStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0fe2e5d0d0057898"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImGuiContext.Font`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-392a5c8c6ea406e8"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr Inno.Native.ImGuizmo.ImGuiContext.FontAtlases`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f3eae2f16b7b648"></a>[`public Inno.Native.ImGuizmo.ImFontBaked* Inno.Native.ImGuizmo.ImGuiContext.FontBaked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77ac0d377da7c815"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.FontBakedScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6388) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f478bbd401902e3"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.FontRasterizerDensity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff260ab2c9977c45"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.FontSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a5a98a06db9240a9"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.FontSizeBase`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6386) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-070847567e45ef06"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontStackData Inno.Native.ImGuizmo.ImGuiContext.FontStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-96708132876859a2"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdc1d781e32244f2"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FrameCountEnded`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-035fce7df04f9999"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FrameCountPlatformEnded`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-845b717f9c4c7e4c"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FrameCountRendered`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6362) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d798cef00cb75e2b"></a>[`public Inno.Native.ImGuizmo.NativeArray_605a5c71dc256950 Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6978) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7489a4ae7c801e24"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameAccum`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6984) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8dc3860e813a4c2"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3aed18e17b9f4614"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.FramerateSecPerFrameIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a63da3785857898c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.GcCompactAll`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6540) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d5eecbac5ded5cf"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiGroupData Inno.Native.ImGuizmo.ImGuiContext.GroupStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6554) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-916c3c3e1c5adec4"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HookIdNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6894) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78ed5fee4c3a8e60"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiContextHook Inno.Native.ImGuizmo.ImGuiContext.Hooks`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6892) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d27f01f600025365"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayClearTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6790) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2112188e1a015da4"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6784) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20c567aeac5ec150"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayIdPreviousFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6786) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-963e429ec266f7f4"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.HoverItemDelayTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6788) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51d39332c9befec6"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoverItemUnlockedStationaryId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6792) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4691115a36083ef"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoverWindowUnlockedStationaryId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6794) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fd8bb7af22944d1"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoveredId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c8d713c740f07cf"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.HoveredIdAllowOverlap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-47b9bcbb40fc633e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.HoveredIdIsDisabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6464) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7ccfa801d3cfbe1"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.HoveredIdNotActiveTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-63e5dad17df5bedd"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.HoveredIdPreviousFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a69f3317ae2fc0d"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.HoveredIdPreviousFrameItemCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7817d1c2b839aa0b"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.HoveredIdTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8dd94f70df68cd2"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.HoveredWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-997cb3e0cf4d6891"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.HoveredWindowBeforeClear`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72cac22706007314"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.HoveredWindowUnderMovingWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ed6b43f09793b70"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.Initialized`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c685a7102f59131"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.InputEventsNextEventId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76a45a6488bd2631"></a>[`public Inno.Native.ImGuizmo.ImGuiMouseSource Inno.Native.ImGuizmo.ImGuiContext.InputEventsNextMouseSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5417e8d46da2d2d"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiInputEvent Inno.Native.ImGuizmo.ImGuiContext.InputEventsQueue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eaa7ca9cba572eba"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiInputEvent Inno.Native.ImGuizmo.ImGuiContext.InputEventsTrail`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-751839ae66cb51bd"></a>[`public Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState Inno.Native.ImGuizmo.ImGuiContext.InputTextDeactivatedState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6806) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b364c2075d31203a"></a>[`public Inno.Native.ImGuizmo.ImGuiTextIndex Inno.Native.ImGuizmo.ImGuiContext.InputTextLineIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6804) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e2e356e42048e6f"></a>[`public Inno.Native.ImGuizmo.ImFontBaked Inno.Native.ImGuizmo.ImGuiContext.InputTextPasswordFontBackupBaked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6808) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-570bf3744c8b42f5"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.InputTextPasswordFontBackupFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6810) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e855fd1144ea7a74"></a>[`public Inno.Native.ImGuizmo.ImGuiInputTextState Inno.Native.ImGuizmo.ImGuiContext.InputTextState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6802) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-accdeae820ed6b32"></a>[`public Inno.Native.ImGuizmo.ImGuiIO Inno.Native.ImGuizmo.ImGuiContext.Io`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e726da0a47e7a9e7"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiItemFlags Inno.Native.ImGuizmo.ImGuiContext.ItemFlagsStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-62675a5ea7f91c9f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.ItemUnclipByLog`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6466) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d1fd3a8670a9170"></a>[`public Inno.Native.ImGuizmo.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN Inno.Native.ImGuizmo.ImGuiContext.KeysMayBeCharInput`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a25f56c3dff49e30"></a>[`public Inno.Native.ImGuizmo.NativeArray_ec6e6e3c26b88eb8 Inno.Native.ImGuizmo.ImGuiContext.KeysOwnerData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af72073692299d41"></a>[`public Inno.Native.ImGuizmo.ImGuiKeyRoutingTable Inno.Native.ImGuizmo.ImGuiContext.KeysRoutingTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6518) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ebb51d6e8c54696"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.LastActiveId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba72e7460e2b0056"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.LastActiveIdTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48c7e66cbaf03b80"></a>[`public Inno.Native.ImGuizmo.ImGuiLastItemData Inno.Native.ImGuizmo.ImGuiContext.LastItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6534) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9e50aee2054a66f"></a>[`public double Inno.Native.ImGuizmo.ImGuiContext.LastKeyModsChangeFromNoneTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2d1037b6fbf4e95"></a>[`public double Inno.Native.ImGuizmo.ImGuiContext.LastKeyModsChangeTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6508) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e40cba7c4414017f"></a>[`public double Inno.Native.ImGuizmo.ImGuiContext.LastKeyboardKeyPressTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e292e41e6ebba250"></a>[`public Inno.Native.ImGuizmo.NativeArray_df98efe8482c9a26 Inno.Native.ImGuizmo.ImGuiContext.LocalizationTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6896) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d793574e041a61b9"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiContext.LogBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6908) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-712cee031a414240"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.LogDepthRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6916) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-228bee42a1b2fbce"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.LogDepthToExpand`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a3d5452174ac16b"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.LogDepthToExpandDefault`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6920) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ab30f5ef8ef4d8a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.LogEnabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6898) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd2bced1090ede78"></a>[`public Inno.Native.ImGuizmo.Iobuf* Inno.Native.ImGuizmo.ImGuiContext.LogFile`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6906) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-180aaaca1acaec27"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.LogFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-56f40421fc6a23ed"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.LogLineFirstItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6900) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33847379103c398c"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.LogLinePosY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6914) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a25e3373b4613da1"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiContext.LogNextPrefix`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17d1891977227899"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiContext.LogNextSuffix`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6912) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-790d2f7cb82d8c28"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.LogWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6904) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d84feeb55a57f0b5"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiID Inno.Native.ImGuizmo.ImGuiContext.MenusIdSubmittedThisFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6866) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ab424c665677f23a"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.MouseCursor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6796) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fca0e27e60e90374"></a>[`public Inno.Native.ImGuizmo.ImGuiViewportP* Inno.Native.ImGuizmo.ImGuiContext.MouseLastHoveredViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2ea47ba36b1865e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.MouseLastValidPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6800) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eea0028811a6ff88"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.MouseStationaryTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6798) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4468f64a40e8f5a3"></a>[`public Inno.Native.ImGuizmo.ImGuiViewportP* Inno.Native.ImGuizmo.ImGuiContext.MouseViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b8ba1926e0636cd"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.MovingWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a36e2ab0025e650e"></a>[`public Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState Inno.Native.ImGuizmo.ImGuiContext.MultiSelectStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-509ffb777f1e93c9"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData Inno.Native.ImGuizmo.ImGuiContext.MultiSelectTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-710d6e01686655df"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.MultiSelectTempDataStacked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6778) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b0b80bc633590cdc"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavActivateDownId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-598d671d6a537783"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavActivateFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6604) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d305a40980350de"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavActivateId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13742571ae3e101b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavActivatePressedId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6602) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc013532405c6007"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavAnyRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c79e08a3778c9f5"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiContext.NavCursorHideFrames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6620) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff84ac5cd4e8f343"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavCursorVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53ad062f76b01d7b"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData Inno.Native.ImGuizmo.ImGuiContext.NavFocusRoute`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6606) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-905d600b64c1324d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavFocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6594) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c5951edc2a0a32e"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavHighlightActivatedId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-137fd3c97da5b71f"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.NavHighlightActivatedTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0896313365df6d35"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavHighlightItemUnderNav`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff6b92780f54c4a6"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b914d79bfefa71f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavIdIsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6588) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b15ddf5c84a6ddf"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavInitRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6624) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-56d7948149c9dc87"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavInitRequestFromMove`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6626) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb6e6ca55fa3b162"></a>[`public Inno.Native.ImGuizmo.ImGuiNavItemData Inno.Native.ImGuizmo.ImGuiContext.NavInitResult`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a64f5fc3d69a0e83"></a>[`public Inno.Native.ImGuizmo.ImGuiInputSource Inno.Native.ImGuizmo.ImGuiContext.NavInputSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8df1a071db38ce9a"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedFromFocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6666) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35b4a76480382c19"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToFocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6670) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cccad799aa4a5601"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToHasSelectionData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8840edf97b5f9e61"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6668) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02f691f6c0e2b35c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToIsTabbing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6674) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb9c55e51a402e3e"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavJustMovedToKeyMods`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6672) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d2ab965a7c0b690"></a>[`public long Inno.Native.ImGuizmo.ImGuiContext.NavLastValidSelectionUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eedb12e61fb7e1e7"></a>[`public Inno.Native.ImGuizmo.ImGuiNavLayer Inno.Native.ImGuizmo.ImGuiContext.NavLayer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6596) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf646b925d6f781b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavMousePosDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e84c7abe34cb26d1"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiContext.NavMoveClipDir`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-11ba9cb918cdc2a9"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiContext.NavMoveDir`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6642) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8306e0339b6a8422"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiContext.NavMoveDirForDebug`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-97fc73675502a34d"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavMoveFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6636) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-018c4f8e7ebb66d6"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavMoveForwardToNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8728c582456e927"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavMoveKeyMods`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6640) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f93ab1f3cd4cb287"></a>[`public Inno.Native.ImGuizmo.ImGuiNavItemData Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultLocal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6658) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e91359b7e92b63d"></a>[`public Inno.Native.ImGuizmo.ImGuiNavItemData Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultLocalVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6660) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c45134a1ae348140"></a>[`public Inno.Native.ImGuizmo.ImGuiNavItemData Inno.Native.ImGuizmo.ImGuiContext.NavMoveResultOther`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6662) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a8ac2a9e7a28ae2"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavMoveScoringItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0bf7ad7e4d842b2"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavMoveScrollFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6638) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd555e2253dc81ed"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavMoveSubmitted`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6630) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2a178fea0888ff9"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavNextActivateFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e54fb0321d5b0b7b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.NavNextActivateId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f40a662dae950881"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavScoringDebugCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6652) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-928d48871e4d41b3"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.NavScoringNoClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6650) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f82c554c3f4f5fca"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.NavScoringRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-067b38b91b6eaf73"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavTabbingCounter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df109b1e220983e5"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.NavTabbingDir`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9866a0a823154709"></a>[`public Inno.Native.ImGuizmo.ImGuiNavItemData Inno.Native.ImGuizmo.ImGuiContext.NavTabbingResultFirst`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6664) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba9a05c45a10b9d1"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.NavWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59ea3ffbd9aafd41"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.NavWindowingAccumDeltaPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6702) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-224c84b6db7f9f51"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.NavWindowingAccumDeltaSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6704) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c911c54ec9828e8"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.NavWindowingHighlightAlpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6694) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b827bb1243b6242"></a>[`public Inno.Native.ImGuizmo.ImGuiInputSource Inno.Native.ImGuizmo.ImGuiContext.NavWindowingInputSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5d2371a05f2c5692"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.NavWindowingListWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6690) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d4d0b0ab476ae4d"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTarget`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6686) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98a70fd79eb746f6"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTargetAnim`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6688) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9a77c85e2970354"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.NavWindowingTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6692) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6ec92c9173a7e0e"></a>[`public Inno.Native.ImGuizmo.ImGuiKey Inno.Native.ImGuizmo.ImGuiContext.NavWindowingToggleKey`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6700) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9eff997a22f3958b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.NavWindowingToggleLayer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6698) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f35860ad31c75467"></a>[`public Inno.Native.ImGuizmo.ImGuiNextItemData Inno.Native.ImGuizmo.ImGuiContext.NextItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6532) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4afc47e128219527"></a>[`public Inno.Native.ImGuizmo.ImGuiNextWindowData Inno.Native.ImGuizmo.ImGuiContext.NextWindowData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6536) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e155b06cc13cf0d2"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiPopupData Inno.Native.ImGuizmo.ImGuiContext.OpenPopupStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-981c057f2580b583"></a>[`public Inno.Native.ImGuizmo.ImGuiPlatformImeData Inno.Native.ImGuizmo.ImGuiContext.PlatformImeData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6870) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-905227a12db49dd4"></a>[`public Inno.Native.ImGuizmo.ImGuiPlatformImeData Inno.Native.ImGuizmo.ImGuiContext.PlatformImeDataPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6872) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fa801e6c6a0b971"></a>[`public Inno.Native.ImGuizmo.ImGuiPlatformIO Inno.Native.ImGuizmo.ImGuiContext.PlatformIo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2cca62ee05a30dfa"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.PlatformLastFocusedViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c035aafca401907"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.PlatformMonitorsFullWorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c15df0d63653cf6d"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.PlatformWindowsCreatedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6578) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c71ee657d6cb3e39"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.ScrollbarClickDeltaToGrabCenter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6842) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-88a05ceff93c3cab"></a>[`public short Inno.Native.ImGuizmo.ImGuiContext.ScrollbarSeekMode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6840) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-56ce2c93d3c04c78"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.SettingsDirtyTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6882) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bd34eccdb548af0"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler Inno.Native.ImGuizmo.ImGuiContext.SettingsHandlers`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6886) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fcdf94fb62f8cad"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiContext.SettingsIniData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6884) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e27d36c1abfa79e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.SettingsLoaded`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6880) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27c3ae0b2f8ff06b"></a>[`public Inno.Native.ImGuizmo.ImChunkStreamImGuiTableSettings Inno.Native.ImGuizmo.ImGuiContext.SettingsTables`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6890) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b2e7cd209f42939"></a>[`public Inno.Native.ImGuizmo.ImChunkStreamImGuiWindowSettings Inno.Native.ImGuizmo.ImGuiContext.SettingsWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6888) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6b0129a070b0a55"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem Inno.Native.ImGuizmo.ImGuiContext.ShrinkWidthBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6772) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5394c9a911585096"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.SliderCurrentAccum`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6846) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82ed7081b8a2ac88"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.SliderCurrentAccumDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6848) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b7fe64bf25b3d9f"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.SliderGrabClickOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6844) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61652828d5e48075"></a>[`public Inno.Native.ImGuizmo.ImGuiErrorRecoveryState* Inno.Native.ImGuizmo.ImGuiContext.StackSizesInBeginForCurrentWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6934) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f49d76b1644976d8"></a>[`public Inno.Native.ImGuizmo.ImGuiErrorRecoveryState Inno.Native.ImGuizmo.ImGuiContext.StackSizesInNewFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6932) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95a411f3fb487844"></a>[`public Inno.Native.ImGuizmo.ImGuiStyle Inno.Native.ImGuizmo.ImGuiContext.Style`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c57688423f00cbc5"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiStyleMod Inno.Native.ImGuizmo.ImGuiContext.StyleVarStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6893a791e274881"></a>[`public Inno.Native.ImGuizmo.ImPoolImGuiTabBar Inno.Native.ImGuizmo.ImGuiContext.TabBars`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4ff790af38dc702"></a>[`public Inno.Native.ImGuizmo.ImPoolImGuiTable Inno.Native.ImGuizmo.ImGuiContext.Tables`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6760) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6f9a474f4410878"></a>[`public Inno.Native.ImGuizmo.ImVectorFloat Inno.Native.ImGuizmo.ImGuiContext.TablesLastTimeActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46e6a56d85deb665"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTableTempData Inno.Native.ImGuizmo.ImGuiContext.TablesTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da6a95c4ab772edb"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.TablesTempDataStacked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6756) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78d7e58e075896ed"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiContext.TempBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6992) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4112152dde68e04b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.TempInputId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6812) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e8c28ad189ccd32"></a>[`public Inno.Native.ImGuizmo.NativeArray_f4284eefd115ca43 Inno.Native.ImGuizmo.ImGuiContext.TempKeychordName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6994) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ead0977ba049c567"></a>[`public void* Inno.Native.ImGuizmo.ImGuiContext.TestEngine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e1c7430c7ad7593d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.TestEngineHookItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-12bb82be50e09194"></a>[`public double Inno.Native.ImGuizmo.ImGuiContext.Time`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7354cd42cefe601b"></a>[`public short Inno.Native.ImGuizmo.ImGuiContext.TooltipOverrideCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6860) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c3e6bb7c521ffd6"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.TooltipPreviousWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe41874a4a4b9541"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData Inno.Native.ImGuizmo.ImGuiContext.TreeNodeStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c5e6e8549e1b018"></a>[`public Inno.Native.ImGuizmo.ImGuiTypingSelectState Inno.Native.ImGuizmo.ImGuiContext.TypingSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6868) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7d20e179d74c1ca"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureDataPtr Inno.Native.ImGuizmo.ImGuiContext.UserTextures`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8768d9ba6832a09"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ViewportCreatedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6576) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-918b6d61595b2516"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.ViewportFocusedStampCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e534b141f4a7517"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr Inno.Native.ImGuizmo.ImGuiContext.Viewports`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbcd6a34faba6e7b"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WantCaptureKeyboardNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6988) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f8e8b60f3c42414"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WantCaptureMouseNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6986) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-604dd467d8f0e7fb"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WantTextInputNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6990) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67d24d2dbbbb2aff"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.WheelingAxisAvg`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbcad9697990de60"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiContext.WheelingWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd6fb29a5e75ceff"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowRefMousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c122e6494e03fe2f"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowReleaseTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6442) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-93add1827a188f77"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowScrolledFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1db88f27463ae273"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowStartFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7927e9c07b68daf6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiContext.WheelingWindowWheelRemainder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6a757a55a67aace"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiContext.WindowResizeBorderExpectedRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6836) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-453784bd0704331a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.WindowResizeRelativeMode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6838) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b955e2b6994432dd"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr Inno.Native.ImGuizmo.ImGuiContext.Windows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-52bf38656ddacf32"></a>[`public int Inno.Native.ImGuizmo.ImGuiContext.WindowsActiveCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6418) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdfcf306193f94bb"></a>[`public float Inno.Native.ImGuizmo.ImGuiContext.WindowsBorderHoverPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fb736efbefb64b56"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImGuiContext.WindowsById`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-571f2695f4a9dc5c"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr Inno.Native.ImGuizmo.ImGuiContext.WindowsFocusOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e5158008e93a62a"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr Inno.Native.ImGuizmo.ImGuiContext.WindowsTempSortBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e21bc26a3675c07"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContext.WithinEndChildId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-acad8f1c6012b25e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.WithinFrameScope`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10a74eebfbfa8219"></a>[`public byte Inno.Native.ImGuizmo.ImGuiContext.WithinFrameScopeWithImplicitWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-198d297e54813a2a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiContextHook*, void> Inno.Native.ImGuizmo.ImGuiContextHook.Callback`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3b007446b403b6a"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContextHook.HookId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7005) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e20088e5beed1e3b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiContextHook.Owner`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7009) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ca694eb47d672fb"></a>[`public Inno.Native.ImGuizmo.ImGuiContextHookType Inno.Native.ImGuizmo.ImGuiContextHook.Type`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7007) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43a70264de6eb8f8"></a>[`public void* Inno.Native.ImGuizmo.ImGuiContextHook.UserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0313a6c3d3859bd"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeEndFramePost = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7037) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-90c4d5c23aa829bf"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeEndFramePre = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7036) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-e1d7facd2c346568"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeNewFramePost = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-3ab855bc2ed49044"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeNewFramePre = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7034) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-86d4a71bcf4e1297"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypePendingRemoval = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7041) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-ba4b02c17dafa0a7"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeRenderPost = 5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7039) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-bf5be759e1034e84"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeRenderPre = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-6b252bb41aabe746"></a>[`Inno.Native.ImGuizmo.ImGuiContextHookType.ImGuiContextHookTypeShutdown = 6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-94fc9f56bebae357"></a>[`public Inno.Native.ImGuizmo.NativeArray_bae28ecfe547f5d7 Inno.Native.ImGuizmo.ImGuiDataTypeStorage.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7052) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f3649fb50f10d62d"></a>[`public int Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.ElapseFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7072) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03cce15494be0b39"></a>[`public byte Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.HasBeenEditedBefore`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7074) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ae1ef0e4e043564"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7070) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7012bdcbad2c92c7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiDeactivatedItemData.IsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7076) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8a6cbfef4300b7a"></a>[`public short Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.AllocCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7096) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cabafb5098188b8f"></a>[`public int Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.FrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dfb0e805431f295a"></a>[`public short Inno.Native.ImGuizmo.ImGuiDebugAllocEntry.FreeCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7098) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e85c04e6f07aad0c"></a>[`public Inno.Native.ImGuizmo.NativeArray_f9c2e11bc01cc26a Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.LastEntriesBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7122) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b128a9facfb1a032"></a>[`public short Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.LastEntriesIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7120) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a21857523db11cef"></a>[`public int Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.TotalAllocCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7116) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3778f47bd88172e"></a>[`public int Inno.Native.ImGuizmo.ImGuiDebugAllocInfo.TotalFreeCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7118) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32af88a3f1727131"></a>[`public byte Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Active`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7142) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e22a24d606baf6e7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Complete`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7144) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43dcd17044c0b8a1"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.MainId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7140) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ac1bbbb2d90eb98"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.ResultPathBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7152) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9349dec648e7874f"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Results`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7148) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-169ee46c2518e719"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.ResultsDescBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7150) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4421f02f136cc925"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery.Step`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7146) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-30ae478e8e6f530a"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirCount = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7172) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-387d22120a68a984"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirDown = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7171) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-27db91d393a5a22a"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirLeft = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-ec25833f0c4f715b"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirNone = -1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7167) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-d37dd01aab9f24c1"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirRight = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7169) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-19735e0429484009"></a>[`Inno.Native.ImGuizmo.ImGuiDir.ImGuiDirUp = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7170) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-df6fee0d466da46d"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImGuiDockContext.Nodes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33bf41c887779945"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings Inno.Native.ImGuizmo.ImGuiDockContext.NodesSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d4139ccd72c9ba7"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiDockRequest Inno.Native.ImGuizmo.ImGuiDockContext.Requests`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7185) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3433fdabd320327"></a>[`public byte Inno.Native.ImGuizmo.ImGuiDockContext.WantFullRebuild`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b8811def24b8c38"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiDockNode.CentralNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7245) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d45a43d73579bdbe"></a>[`public Inno.Native.ImGuizmo.NativeArray_68e25b300381a6fc Inno.Native.ImGuizmo.ImGuiDockNode.ChildNodes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83034f2aacdf33c4"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.CountNodeWithWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7249) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-676b4b1e04609e52"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiDockNode.HostWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7241) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05499df06ed50f2b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7209) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20580c2cf3aaa22a"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.LastBgColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7239) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed362ab4f0ee3e7f"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.LastFocusedNodeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7257) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-773a23706ca85549"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-064f15b1873acb80"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7251) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83b2d19afe8707f3"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.LastFrameFocused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7255) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84f37e753493e68a"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.LocalFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bbd0700e3cb2854"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.LocalFlagsInWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7215) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aff1b7332a82aa49"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.MergedFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7217) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c42b209f5265339"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiDockNode.OnlyNodeWithWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7247) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe45487c17ada0af"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiDockNode.ParentNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-09eceaf355737eed"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiDockNode.Pos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7229) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd1275ce25f5be6c"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.RefViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7263) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e32dbfcb46e2ea5b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.SelectedTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7259) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-079a6471bc2fa8dc"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.SharedFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7211) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a01ef8bd767689e0"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiDockNode.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-259de3c06ca524cf"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiDockNode.SizeRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff780386c2e56423"></a>[`public Inno.Native.ImGuizmo.ImGuiAxis Inno.Native.ImGuizmo.ImGuiDockNode.SplitAxis`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b621256e411eaee4"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNodeState Inno.Native.ImGuizmo.ImGuiDockNode.State`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ae4cda849e99cf9"></a>[`public Inno.Native.ImGuizmo.ImGuiTabBar* Inno.Native.ImGuizmo.ImGuiDockNode.TabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7227) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27968bf98dec84ab"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiDockNode.VisibleWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7243) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6bd0c1888354324"></a>[`public uint Inno.Native.ImGuizmo.ImGuiDockNode.WantCloseTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7261) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-341cbcd63814f9f2"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowClass Inno.Native.ImGuizmo.ImGuiDockNode.WindowClass`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cfd0aa278560c58d"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr Inno.Native.ImGuizmo.ImGuiDockNode.Windows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7225) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef5b64c4b9214b41"></a>[`Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowHiddenBecauseSingleWindow = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7727) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-32798fde5826db02"></a>[`Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowHiddenBecauseWindowsAreResizing = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-854afbff24eb4e71"></a>[`Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateHostWindowVisible = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7729) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-cf580ae3fb9fe48d"></a>[`Inno.Native.ImGuizmo.ImGuiDockNodeState.ImGuiDockNodeStateUnknown = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-42c72da5ef39f8b4"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfBeginPopupStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7772) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-40bc78cdcdf1dcfc"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfColorStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7760) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fc736c3b55f05db"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfDisabledStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7774) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65d6634bed8d7a3e"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfFocusScopeStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8efdd57c0a9a542f"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfFontStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-224c1b597d37b03a"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfGroupStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdceba954f94bfb1"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfIdStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7756) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b694a2169fe10da0"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfItemFlagsStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-31bf36aa088d7f2f"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfStyleVarStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-453a93a995f57fdc"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfTreeStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27fe6ccc71ca275a"></a>[`public short Inno.Native.ImGuizmo.ImGuiErrorRecoveryState.SizeOfWindowStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7754) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f2d16512540af74"></a>[`public uint Inno.Native.ImGuizmo.ImGuiFocusScopeData.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7792) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d656f964966e915a"></a>[`public uint Inno.Native.ImGuizmo.ImGuiFocusScopeData.WindowId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7794) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-271dbc57e422d242"></a>[`public byte Inno.Native.ImGuizmo.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f798611e09783ef3"></a>[`public uint Inno.Native.ImGuizmo.ImGuiGroupData.BackupActiveIdIsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7828) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-080cd17fee97cc14"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiGroupData.BackupCurrLineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7824) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-199efc1b1da86d37"></a>[`public float Inno.Native.ImGuizmo.ImGuiGroupData.BackupCurrLineTextBaseOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7826) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-526566c20c3dd7f4"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2d4e481f6cd86c2"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7814) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcaeff69c54d8095"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiGroupData.BackupCursorPosPrevLine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-176f7049203f59a9"></a>[`public byte Inno.Native.ImGuizmo.ImGuiGroupData.BackupDeactivatedIdIsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7832) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c64259c4060eb93b"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiGroupData.BackupGroupOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7822) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9ba746df4c75d46"></a>[`public byte Inno.Native.ImGuizmo.ImGuiGroupData.BackupHoveredIdIsAlive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7834) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c4958593c1f5363"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiGroupData.BackupIndent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7820) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5decebeebba8298d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiGroupData.BackupIsSameLine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7836) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dac5ea1b2bba947d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiGroupData.EmitItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7838) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-571a1110bd257897"></a>[`public uint Inno.Native.ImGuizmo.ImGuiGroupData.WindowId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7812) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd13f0c672c5f73f"></a>[`public float Inno.Native.ImGuizmo.ImGuiIDStackTool.CopyToClipboardLastTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90d1a9001fb86651"></a>[`public int Inno.Native.ImGuizmo.ImGuiIDStackTool.LastActiveFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7860) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5379d552e7c332bd"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e6bd1996de09f0e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIDStackTool.OptHexEncodeNonAsciiChars`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7856) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81558c2c1b275075"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.AppAcceptingEvents`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6806cebf3175bf55"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.AppFocusLost`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-153edad2232a8f92"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.BackendFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8188) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e84b6d4ea0b1952f"></a>[`public void* Inno.Native.ImGuizmo.ImGuiIO.BackendLanguageUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7ef231dc3e99a86"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiIO.BackendPlatformName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aedd514974677cfe"></a>[`public void* Inno.Native.ImGuizmo.ImGuiIO.BackendPlatformUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8306) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7b00d7f798930cf"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiIO.BackendRendererName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8304) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33da4372a9a299b8"></a>[`public void* Inno.Native.ImGuizmo.ImGuiIO.BackendRendererUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8308) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a94fb1f9871c6ec7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugBeginReturnValueLoop`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8296) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86690dd13f591a67"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugBeginReturnValueOnce`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1247bac9d785be55"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugHighlightIdConflicts`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95fdbe9fc133b372"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-420f17010053cc3a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIgnoreFocusLoss`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8298) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fddcce83930b922"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIniSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-191222fdc8ba17e8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDebugIsDebuggerPresent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8288) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a00c6269929ebcc4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingAlwaysTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8230) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdc73a9f1e446eba"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingNoDockingOver`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8226) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a77f73985c0eb315"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingNoSplit`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49c2df7a4b92c832"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingTransparentPayload`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8232) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0bfbe4625ada3e69"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDockingWithShift`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8228) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f6462628817e1f1"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDpiScaleFonts`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8244) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20f2bf17337b28de"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDpiScaleViewports`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-34f22f167501a0a8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigDragClickToInputText`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f68e634fee3f4bf0"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecovery`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b201dbe3c1fb24ce"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableAssert`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1922da14e0b62423"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableDebugLog`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c74b3da6a299da86"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigErrorRecoveryEnableTooltip`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e65b69a707d9db1a"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.ConfigFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8186) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3fda5b61dc59b84f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTextCursorBlink`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b436ff5ef40b924"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTextEnterKeepActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95ba600a53b16e29"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigInputTrickleEventQueue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a886e1b583014e3e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigMacOsxBehaviors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da3aacf85a7a07cf"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.ConfigMemoryCompactTimer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fd1805e114b97efb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCaptureKeyboard`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8214) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-708547680609dd20"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCursorVisibleAlways`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9b5b43c620fa142"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavCursorVisibleAuto`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8220) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe15d201306a5675"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavEscapeClearFocusItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8216) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-816f6fb956549aec"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavEscapeClearFocusWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8218) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-588ac655f05b1683"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavMoveSetMousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-228b636722b24e5a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigNavSwapGamepadButtons`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8210) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7ac15b2eaab1101"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigScrollbarScrollByPage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8266) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4fa3f00a68bbdacc"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoAutoMerge`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8234) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2e9d2f8c8248c20"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoDecoration`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c801018ae0957bb7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoDefaultParent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8240) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2428af25ca318c21"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsNoTaskBarIcon`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8236) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-920892f65393cfe1"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8242) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9125a8160627bdeb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a523dffa12b751f4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8262) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-554bcc152b06d7c8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.ConfigWindowsResizeFromEdges`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ba0439c3f8b4359"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImGuiIO.Ctx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8338) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-601f20849306ad2c"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.DeltaTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8194) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-483eb19524704eee"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiIO.DisplayFramebufferScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8192) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a20a2c9a4f2573b2"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiIO.DisplaySize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8190) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a0e912f3b35f273"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.FontAllowUserScaling`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8208) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79ddfc3e08a803cb"></a>[`public Inno.Native.ImGuizmo.ImFont* Inno.Native.ImGuizmo.ImGuiIO.FontDefault`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8206) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f87f8c2c86fe1f7f"></a>[`public Inno.Native.ImGuizmo.ImFontAtlas* Inno.Native.ImGuizmo.ImGuiIO.Fonts`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ca980985ca2909b"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.Framerate`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c6c15c4f9f9d240"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiIO.IniFilename`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8198) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd3c8c91f87349a5"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.IniSavingRate`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8196) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b515333717af9bb"></a>[`public Inno.Native.ImGuizmo.ImVectorImWchar Inno.Native.ImGuizmo.ImGuiIO.InputQueueCharacters`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bb18a3fb038f234"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiIO.InputQueueSurrogate`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b524c952958a0ddf"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.KeyAlt`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a09c3008cd729995"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.KeyCtrl`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c470a1923f6758d3"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.KeyMods`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6a9f22b4f87717d4"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.KeyRepeatDelay`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8276) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-93371a7afc6d8a2a"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.KeyRepeatRate`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8278) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-008c71418e597ae5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.KeyShift`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01c344171f240567"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.KeySuper`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e0b83336906826a"></a>[`public Inno.Native.ImGuizmo.NativeArray_3d337b926fc04643 Inno.Native.ImGuizmo.ImGuiIO.KeysData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8362) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fa2ec56320eb515"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiIO.LogFilename`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8200) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28ba95ca2208a837"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.MetricsActiveWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4bbe1e9bac4a9622"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderIndices`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-043a1f40e80f58f7"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderVertices`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da78f164c75dd788"></a>[`public int Inno.Native.ImGuizmo.ImGuiIO.MetricsRenderWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8332) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ecebcbbfbe8e1be"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseClicked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e05fac73656f31b"></a>[`public Inno.Native.ImGuizmo.NativeArray_76771c8c9ab15473 Inno.Native.ImGuizmo.ImGuiIO.MouseClickedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdd3187041b4d63a"></a>[`public Inno.Native.ImGuizmo.NativeArray_76771c8c9ab15473 Inno.Native.ImGuizmo.ImGuiIO.MouseClickedLastCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bb98b274a97e10d"></a>[`public Inno.Native.ImGuizmo.NativeArray_8da6be2ffdb0de05 Inno.Native.ImGuizmo.ImGuiIO.MouseClickedPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f16eb3d7f86b21d"></a>[`public Inno.Native.ImGuizmo.NativeArray_ff485db19a16ca26 Inno.Native.ImGuizmo.ImGuiIO.MouseClickedTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79e4e2eb1df33d54"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.MouseCtrlLeftAsRightClick`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f460b1844828f6a"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiIO.MouseDelta`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8336) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc4d6d2e715db610"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClickMaxDist`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8272) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84baf0d081a057b2"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClickTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13d7d98fab8c3d64"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseDoubleClicked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-892a7b157be36a9c"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseDown`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8342) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed7857343918d746"></a>[`public Inno.Native.ImGuizmo.NativeArray_35aebb639e6eb330 Inno.Native.ImGuizmo.ImGuiIO.MouseDownDuration`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-057a8a9071afc328"></a>[`public Inno.Native.ImGuizmo.NativeArray_35aebb639e6eb330 Inno.Native.ImGuizmo.ImGuiIO.MouseDownDurationPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-072be1765df28f1f"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseDownOwned`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6fb257d04f5a037b"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseDownOwnedUnlessPopupClose`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8386) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ba8c51b499c97e9"></a>[`public Inno.Native.ImGuizmo.NativeArray_8da6be2ffdb0de05 Inno.Native.ImGuizmo.ImGuiIO.MouseDragMaxDistanceAbs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fb11123142cfdb3"></a>[`public Inno.Native.ImGuizmo.NativeArray_35aebb639e6eb330 Inno.Native.ImGuizmo.ImGuiIO.MouseDragMaxDistanceSqr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05628e65c157f5af"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.MouseDragThreshold`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8274) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c57cd39356108188"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.MouseDrawCursor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3d21636c565b69f"></a>[`public uint Inno.Native.ImGuizmo.ImGuiIO.MouseHoveredViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41fb1ec6d8749afd"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiIO.MousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8340) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e40da8f31febbf32"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiIO.MousePosPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79742b1fff436052"></a>[`public Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGuizmo.ImGuiIO.MouseReleased`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85fa5bd20b28ae3b"></a>[`public Inno.Native.ImGuizmo.NativeArray_ff485db19a16ca26 Inno.Native.ImGuizmo.ImGuiIO.MouseReleasedTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75278a4859ff1c8d"></a>[`public Inno.Native.ImGuizmo.ImGuiMouseSource Inno.Native.ImGuizmo.ImGuiIO.MouseSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27445a7f823365ef"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.MouseWheel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8344) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f044450311991af2"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.MouseWheelH`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8346) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-252d1cad043d13fb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.MouseWheelRequestAxisSwap`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8388) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f8b943ff1b6e0ef0"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.NavActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9464b4d4f01abced"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.NavVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8324) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2687ec390f235330"></a>[`public float Inno.Native.ImGuizmo.ImGuiIO.PenPressure`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e039479843a45915"></a>[`public void* Inno.Native.ImGuizmo.ImGuiIO.UserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8202) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-817bb60bfee706fb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantCaptureKeyboard`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f928bf95357f573"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantCaptureMouse`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-592bb5f5ae7333ce"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantCaptureMouseUnlessPopupClose`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b99efb7cb6c3f78"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantSaveIniSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8320) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1538b8a6eb018550"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantSetMousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8318) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71d962001c12bcb5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiIO.WantTextInput`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8316) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2563b5784b2d118"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputEvent.AddedByTestEngine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7888) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6b110dcf5bf9b08"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440 Inno.Native.ImGuizmo.ImGuiInputEvent.AnonymousField3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7886) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75ddbca80a23e60f"></a>[`public uint Inno.Native.ImGuizmo.ImGuiInputEvent.EventId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7884) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3cf2b8fe0d5d2196"></a>[`public Inno.Native.ImGuizmo.ImGuiInputSource Inno.Native.ImGuizmo.ImGuiInputEvent.Source`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7882) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1d108cc6bf3b28c"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventType Inno.Native.ImGuizmo.ImGuiInputEvent.Type`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7880) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c8b767e4f6b6ba0"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventAppFocused Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.AppFocused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5aa237e77cfd7ef"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventKey Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.Key`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7914) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21a3a1be8f59312e"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventMouseButton Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ffdec6c4a404d5a0"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventMousePos Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7906) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9adba94ce656aa65"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventMouseViewport Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7912) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b07c945955362a7"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseWheel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7908) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72abe6af4984d708"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEventText Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440.Text`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7916) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1df56cb906011e29"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputEventAppFocused.Focused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7936) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f9e761eba94d7d8"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputEventKey.AnalogValue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7958) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-305f86aa72f48ce8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputEventKey.Down`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-adbcb4bf394b3268"></a>[`public Inno.Native.ImGuizmo.ImGuiKey Inno.Native.ImGuizmo.ImGuiInputEventKey.Key`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7954) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d505a00778cae8ad"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.Button`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-093128ef50e9a709"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.Down`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7978) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5cc937f76b3f10d"></a>[`public Inno.Native.ImGuizmo.ImGuiMouseSource Inno.Native.ImGuizmo.ImGuiInputEventMouseButton.MouseSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-501be79a2f895c64"></a>[`public Inno.Native.ImGuizmo.ImGuiMouseSource Inno.Native.ImGuizmo.ImGuiInputEventMousePos.MouseSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8002) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ff06a8891f7825e"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputEventMousePos.PosX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7998) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7aa2d27ac3ef95ef"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputEventMousePos.PosY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8000) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e579a877f43ac99"></a>[`public uint Inno.Native.ImGuizmo.ImGuiInputEventMouseViewport.HoveredViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8020) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21f7b77bcf9bf06e"></a>[`public Inno.Native.ImGuizmo.ImGuiMouseSource Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.MouseSource`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8042) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42caf6f4e7c075d0"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.WheelX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92da596d28576513"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel.WheelY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ea181fba9364000"></a>[`public uint Inno.Native.ImGuizmo.ImGuiInputEventText.Char`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8060) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8242926caa0fd4a4"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeCount = 8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8083) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-a48bd27a3255f07c"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeFocus = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8082) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-026d79a24cb0e27c"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeKey = 5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8080) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-39f97af491b36d69"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseButton = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8078) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-96e828bc62bea8ad"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMousePos = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8076) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-22c27b0c3948fe96"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseViewport = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-7da4fdac11870762"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeMouseWheel = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-a20a0de9b841ec89"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-30e74e5dd0031b8b"></a>[`Inno.Native.ImGuizmo.ImGuiInputEventType.ImGuiInputEventTypeText = 6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-028170b33e6129c1"></a>[`Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceCount = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8095) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-e638211655c3b932"></a>[`Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceGamepad = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-82ad9cbfe80a4043"></a>[`Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceKeyboard = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8093) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-31a3ea4d86abea6f"></a>[`Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceMouse = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8092) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-7bd3c4775ca5b63d"></a>[`Inno.Native.ImGuizmo.ImGuiInputSource.ImGuiInputSourceNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-564fd3f876d446a7"></a>[`public uint Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8106) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2b660ffa80757ab"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState.TextA`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8108) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3088ecff9acb16af"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.BufCapacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8144) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a7a559f0c1e7768"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiInputTextState.CallbackTextBackup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8142) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a1db60419420460"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImGuiInputTextState.Ctx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8126) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0acf99a74ed5db3"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputTextState.CursorAnim`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8152) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-96533c63dde2a84d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputTextState.CursorCenterY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8156) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-030e21c6c8b10b92"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputTextState.CursorFollow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8154) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ffaaea30195f162b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputTextState.Edited`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8160) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eae398722b1cf414"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8130) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-420356a53b220f48"></a>[`public uint Inno.Native.ImGuizmo.ImGuiInputTextState.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8132) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5cf60bc0babf8b55"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiInputTextState.LastMoveDirectionLr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8164) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7471316738e26896"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.LineCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8148) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2001fd363b33d2f"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.ReloadSelectionEnd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-577b4f0097a01066"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.ReloadSelectionStart`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a68fdc8ac1118d4"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiInputTextState.Scroll`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8146) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c14bd0c201e2a994"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputTextState.SelectedAllMouseLock`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8158) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-763c1b2682873612"></a>[`public Inno.Native.ImGuizmo.STBTexteditState* Inno.Native.ImGuizmo.ImGuiInputTextState.Stb`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8128) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c661843e65f2e40"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiInputTextState.TextA`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8138) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92599dfbd7cdaede"></a>[`public int Inno.Native.ImGuizmo.ImGuiInputTextState.TextLen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8134) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ddfe1ba035e4c506"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiInputTextState.TextSrc`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8136) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1523acd27c49746"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiInputTextState.TextToRevertTo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8140) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bab0c5e33176ab21"></a>[`public byte Inno.Native.ImGuizmo.ImGuiInputTextState.WantReloadUserBuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fde9b1e58bc7be6"></a>[`public float Inno.Native.ImGuizmo.ImGuiInputTextState.WrapWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8150) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e6b0bf43f6750cea"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey0 = 536`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`536` |
| <a id="api-1816167f722786c2"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey1 = 537`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`537` |
| <a id="api-abb61576ca0d91d6"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey2 = 538`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`538` |
| <a id="api-3c2dcc3379080c32"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey3 = 539`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`539` |
| <a id="api-f2967c21ead8fd54"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey4 = 540`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`540` |
| <a id="api-a111682e68e83a0c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey5 = 541`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`541` |
| <a id="api-51916a6334ec71bb"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey6 = 542`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`542` |
| <a id="api-a41471920fdb50ab"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey7 = 543`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`543` |
| <a id="api-f4d11594b8c7975a"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey8 = 544`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`544` |
| <a id="api-ab742be613b87c37"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKey9 = 545`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`545` |
| <a id="api-162fad18fe3df4c0"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyA = 546`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8459) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`546` |
| <a id="api-c97180dc2e52f924"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyApostrophe = 596`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`596` |
| <a id="api-25e8fbadcc2fcc30"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyAppBack = 629`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`629` |
| <a id="api-4d6831255f9ce1f9"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyAppForward = 630`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8543) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`630` |
| <a id="api-cd70665dbdcbb2e3"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyB = 547`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`547` |
| <a id="api-f8a9e0dcee9cfe02"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyBackslash = 604`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`604` |
| <a id="api-bc3e4430ca2b70e1"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyBackspace = 523`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`523` |
| <a id="api-74520b5c8bb305b3"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyC = 548`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8461) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`548` |
| <a id="api-1a22adc4dee79c8a"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyCapsLock = 607`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`607` |
| <a id="api-b830d855b421776f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyComma = 597`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`597` |
| <a id="api-54f4413d72b486c3"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyD = 549`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`549` |
| <a id="api-5341664864d14556"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyDelete = 522`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`522` |
| <a id="api-998c87ef05776bfb"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyDownArrow = 516`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`516` |
| <a id="api-193e8054a8ed1083"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyE = 550`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`550` |
| <a id="api-584e954b50863e0f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEnd = 520`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8433) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`520` |
| <a id="api-bed595415581227f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEnter = 525`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`525` |
| <a id="api-1a4240124c346665"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEqual = 602`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`602` |
| <a id="api-076cca1fdd579f57"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyEscape = 526`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`526` |
| <a id="api-72200bd554e44f0f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF = 551`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8464) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`551` |
| <a id="api-8f79d56986bc5d7e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF1 = 572`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8485) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`572` |
| <a id="api-01539fae497dd938"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF10 = 581`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8494) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`581` |
| <a id="api-dfd99d9cdfdc27bd"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF11 = 582`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`582` |
| <a id="api-e40bd5841e8f07e5"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF12 = 583`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`583` |
| <a id="api-ed7de2d54a028be7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF13 = 584`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`584` |
| <a id="api-d6095ce12503e7ef"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF14 = 585`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`585` |
| <a id="api-eede2f25140c03f1"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF15 = 586`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`586` |
| <a id="api-553100bcf3b2d702"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF16 = 587`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`587` |
| <a id="api-2454c44a7db9a00f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF17 = 588`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`588` |
| <a id="api-3306b8860454931f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF18 = 589`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`589` |
| <a id="api-16cb8ba37ba66024"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF19 = 590`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`590` |
| <a id="api-73b02a4f88b8a475"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF2 = 573`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`573` |
| <a id="api-6fedf1287ec32d75"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF20 = 591`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`591` |
| <a id="api-4923a07f3cfda640"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF21 = 592`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8505) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`592` |
| <a id="api-d354de14b3644616"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF22 = 593`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`593` |
| <a id="api-4edb0f92f759c68f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF23 = 594`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8507) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`594` |
| <a id="api-b25b751af6c6daa6"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF24 = 595`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8508) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`595` |
| <a id="api-22679ba193eecf29"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF3 = 574`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`574` |
| <a id="api-085464a5afe38ee6"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF4 = 575`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`575` |
| <a id="api-7026e1b164d1fbab"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF5 = 576`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`576` |
| <a id="api-4d221458a6a1750c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF6 = 577`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8490) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`577` |
| <a id="api-460e87113e930ef9"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF7 = 578`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8491) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`578` |
| <a id="api-d5831e17596c81db"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF8 = 579`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`579` |
| <a id="api-5600457f468910e7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyF9 = 580`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`580` |
| <a id="api-9d71150df626ee28"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyG = 552`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`552` |
| <a id="api-ea09e0598c70da50"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadBack = 633`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`633` |
| <a id="api-6b750f439d457296"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadDown = 641`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8554) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`641` |
| <a id="api-f7942f37ac6828d4"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadLeft = 638`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`638` |
| <a id="api-18ef562228d76e61"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadRight = 639`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`639` |
| <a id="api-b08a06bb916f84fd"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadDpadUp = 640`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8553) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`640` |
| <a id="api-af475688222fec58"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceDown = 637`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`637` |
| <a id="api-07d1e2268c74148c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceLeft = 634`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`634` |
| <a id="api-8be23204c5b5c56e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceRight = 635`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`635` |
| <a id="api-2cd58399a08d0269"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadFaceUp = 636`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8549) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`636` |
| <a id="api-d471af4373f55da9"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL1 = 642`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8555) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`642` |
| <a id="api-c7c889db3e62125f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL2 = 644`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8557) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`644` |
| <a id="api-98ba95256782e809"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadL3 = 646`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8559) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`646` |
| <a id="api-a363e792a3c4c80b"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickDown = 651`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`651` |
| <a id="api-b4ba4a17a79053d7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickLeft = 648`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`648` |
| <a id="api-592e5ac8507e338b"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickRight = 649`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`649` |
| <a id="api-74bd1c0174b9bb5f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadLStickUp = 650`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`650` |
| <a id="api-da22f9794bc22cc7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR1 = 643`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`643` |
| <a id="api-bd093b3cbd370187"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR2 = 645`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`645` |
| <a id="api-56e7d24bce6a8a9c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadR3 = 647`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`647` |
| <a id="api-3922a5b791a90562"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickDown = 655`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`655` |
| <a id="api-3d26dffea4ea6e54"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickLeft = 652`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`652` |
| <a id="api-8eeb0de738d067eb"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickRight = 653`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`653` |
| <a id="api-2719dfbdecc1bc74"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadRStickUp = 654`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`654` |
| <a id="api-9faa28edeafd6d34"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGamepadStart = 632`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8545) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`632` |
| <a id="api-a40296f544bbb429"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyGraveAccent = 606`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`606` |
| <a id="api-e0c96f3d088ccfb2"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyH = 553`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8466) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`553` |
| <a id="api-71d9cc1bcfbf0526"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyHome = 519`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`519` |
| <a id="api-b471ecdc72a498e4"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyI = 554`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8467) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`554` |
| <a id="api-27a0531354c54c20"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyInsert = 521`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`521` |
| <a id="api-3bfb0f32287a2a79"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyJ = 555`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8468) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`555` |
| <a id="api-dc5ebe8db5df443b"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyK = 556`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`556` |
| <a id="api-e400ff806bd35fc7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad0 = 612`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`612` |
| <a id="api-a40d9eba691d998c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad1 = 613`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`613` |
| <a id="api-9b8aabc7d367973c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad2 = 614`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`614` |
| <a id="api-23d8dbcc5fdd51f2"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad3 = 615`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8528) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`615` |
| <a id="api-a1539cf6eecc885c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad4 = 616`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`616` |
| <a id="api-9bc0dcf0a6257f16"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad5 = 617`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8530) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`617` |
| <a id="api-35258da9b95ad419"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad6 = 618`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8531) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`618` |
| <a id="api-cf21d943de08f671"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad7 = 619`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8532) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`619` |
| <a id="api-58ea3acc5da2a7e7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad8 = 620`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`620` |
| <a id="api-6bf154b5f81ac084"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypad9 = 621`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8534) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`621` |
| <a id="api-25bf8f67506c7059"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadAdd = 626`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`626` |
| <a id="api-8550d1173f60ec0a"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadDecimal = 622`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8535) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`622` |
| <a id="api-2e6cbb248f0447bf"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadDivide = 623`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8536) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`623` |
| <a id="api-f74c31c5479efebf"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadEnter = 627`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8540) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`627` |
| <a id="api-f4aeb5c3e0170ab0"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadEqual = 628`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8541) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`628` |
| <a id="api-d0f6a30473062a0c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadMultiply = 624`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`624` |
| <a id="api-b07c17f4c858f546"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyKeypadSubtract = 625`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`625` |
| <a id="api-b36f961716988904"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyL = 557`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`557` |
| <a id="api-5b1d074b424683e0"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftAlt = 529`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8442) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`529` |
| <a id="api-f1a00119c715ee1c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftArrow = 513`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`513` |
| <a id="api-b80a07c843ec1b76"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftBracket = 603`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`603` |
| <a id="api-478cca0bf07ff430"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftCtrl = 527`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`527` |
| <a id="api-5d6ffa85ffc602e9"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftShift = 528`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`528` |
| <a id="api-b4087df04e159613"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyLeftSuper = 530`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`530` |
| <a id="api-a4e2df655445190c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyM = 558`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`558` |
| <a id="api-e9e9ae58bdaaf0a0"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMenu = 535`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`535` |
| <a id="api-4d1e882a518b1bf2"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMinus = 598`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`598` |
| <a id="api-6dd7260962b10eaa"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseLeft = 656`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`656` |
| <a id="api-5abeb9c6835ab11f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseMiddle = 658`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`658` |
| <a id="api-7b4a89c47a7a59e1"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseRight = 657`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`657` |
| <a id="api-648f220be5f1705e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseWheelX = 661`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`661` |
| <a id="api-1b69ddb1d6ef7ec7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseWheelY = 662`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8575) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`662` |
| <a id="api-b374edd1999948cd"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseX1 = 659`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`659` |
| <a id="api-42348063ff4ca136"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyMouseX2 = 660`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`660` |
| <a id="api-bd57930cb4c0695d"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyN = 559`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`559` |
| <a id="api-34836282eacb6715"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyBegin = 512`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-da93599c7764fd50"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyCount = 155`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8581) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`155` |
| <a id="api-6c4a42993bd94b2e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNamedKeyEnd = 667`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`667` |
| <a id="api-317e6bed2c094cad"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-9d7cf5611525e23e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyNumLock = 609`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8522) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`609` |
| <a id="api-e0842b1816f5970c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyO = 560`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8473) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`560` |
| <a id="api-840571458e0ec01f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyOem102 = 631`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`631` |
| <a id="api-564a73e6e54ef202"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyP = 561`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`561` |
| <a id="api-b2900d50e8da2ee7"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPageDown = 518`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8431) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`518` |
| <a id="api-1aaf7ec3d5ad3a27"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPageUp = 517`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`517` |
| <a id="api-e2ec8711d39cbfd8"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPause = 611`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`611` |
| <a id="api-ea238349edaca9d4"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPeriod = 599`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`599` |
| <a id="api-e0f2fbd32fdf1c9f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyPrintScreen = 610`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`610` |
| <a id="api-858b7c0002f1cd4b"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyQ = 562`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8475) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`562` |
| <a id="api-8a1e2b1defdb7895"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyR = 563`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`563` |
| <a id="api-2bd284ad87b641c4"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModAlt = 665`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8578) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`665` |
| <a id="api-da7e27ede3a79a25"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModCtrl = 663`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8576) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`663` |
| <a id="api-542d243a215ffe61"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModShift = 664`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8577) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`664` |
| <a id="api-142525ea30246222"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyReservedForModSuper = 666`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8579) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`666` |
| <a id="api-30a55c75d4f5ca0d"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightAlt = 533`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`533` |
| <a id="api-a893d21c5903d75a"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightArrow = 514`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8427) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`514` |
| <a id="api-8c7e3e43a667a20c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightBracket = 605`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8518) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`605` |
| <a id="api-05c4bcc8ab4b032f"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightCtrl = 531`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`531` |
| <a id="api-54e0560442fe61af"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightShift = 532`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`532` |
| <a id="api-7263fcf3cd666440"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyRightSuper = 534`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`534` |
| <a id="api-ccba1bf9b23587fd"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyS = 564`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8477) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`564` |
| <a id="api-613e0c24cdfe0c22"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyScrollLock = 608`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`608` |
| <a id="api-2d0d1ee63ca68be0"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySemicolon = 601`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`601` |
| <a id="api-08e027c30fbb546c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySlash = 600`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`600` |
| <a id="api-41a495d33a672630"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeySpace = 524`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524` |
| <a id="api-98f180e14e12f7cf"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyT = 565`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`565` |
| <a id="api-9ce14fd296899477"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyTab = 512`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-e3fc9e155585ca8d"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyU = 566`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8479) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`566` |
| <a id="api-be8eeba597e88fbd"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyUpArrow = 515`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`515` |
| <a id="api-ba6bdebc658697aa"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyV = 567`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`567` |
| <a id="api-4e7042f27aa709af"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyW = 568`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8481) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`568` |
| <a id="api-dd442b785506063d"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyX = 569`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`569` |
| <a id="api-800b244ea6762ebc"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyY = 570`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8483) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`570` |
| <a id="api-271acf56e9ea5c1c"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiKeyZ = 571`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`571` |
| <a id="api-5cec1a719525d1ed"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModAlt = 16384`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8585) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-2acb47217b3e7593"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModCtrl = 4096`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-31f9d2effba9a1a9"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModMask = 61440`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8587) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`61440` |
| <a id="api-a18b04d98d7ac8f4"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-4cf78645e4e0a908"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModShift = 8192`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-e5088988c68b252e"></a>[`Inno.Native.ImGuizmo.ImGuiKey.ImGuiModSuper = 32768`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-1aca3cd85f5c0449"></a>[`public float Inno.Native.ImGuizmo.ImGuiKeyData.AnalogValue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8604) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06b59490d37213d8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiKeyData.Down`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ac8630f3fb62835"></a>[`public float Inno.Native.ImGuizmo.ImGuiKeyData.DownDuration`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89ddabf2e423bef7"></a>[`public float Inno.Native.ImGuizmo.ImGuiKeyData.DownDurationPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8602) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e6751631ba65b75"></a>[`public byte Inno.Native.ImGuizmo.ImGuiKeyOwnerData.LockThisFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8626) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a9e780cd3243c69"></a>[`public byte Inno.Native.ImGuizmo.ImGuiKeyOwnerData.LockUntilRelease`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eab06d102666cf56"></a>[`public uint Inno.Native.ImGuizmo.ImGuiKeyOwnerData.OwnerCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8eed8096686c2481"></a>[`public uint Inno.Native.ImGuizmo.ImGuiKeyOwnerData.OwnerNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8624) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-983619488bbda6b8"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiKeyRoutingData.Mods`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc82a36bfdfc3124"></a>[`public short Inno.Native.ImGuizmo.ImGuiKeyRoutingData.NextEntryIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1a04e2cb3c7dfa8"></a>[`public uint Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17a90621041b2693"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingCurrScore`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8650) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e182977935c0619c"></a>[`public uint Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2a153d873b533af"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiKeyRoutingData.RoutingNextScore`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8652) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35ba30471bf51a37"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.Entries`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b4d73e639256773e"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.EntriesNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8678) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-73934608b41ee91d"></a>[`public Inno.Native.ImGuizmo.NativeArray_42c049e3f15c43ec Inno.Native.ImGuizmo.ImGuiKeyRoutingTable.Index`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8674) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbfcade731f9b22a"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiLastItemData.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8789638b63fda12a"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiLastItemData.DisplayRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8706) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c49decc5d7d27350"></a>[`public uint Inno.Native.ImGuizmo.ImGuiLastItemData.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e6665dcebfd9537"></a>[`public int Inno.Native.ImGuizmo.ImGuiLastItemData.ItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8698) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e75f72a8f106ff4"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiLastItemData.NavRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8704) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1655a600b4dcf755"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiLastItemData.Rect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8702) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7235d72b5fc1fc34"></a>[`public int Inno.Native.ImGuizmo.ImGuiLastItemData.Shortcut`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8710) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0257a989470ce3b4"></a>[`public int Inno.Native.ImGuizmo.ImGuiLastItemData.StatusFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8700) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58a652b3bae16599"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImGuiListClipper.Ctx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c34b5931f18b7bb"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipper.DisplayEnd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-411314c79db97566"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipper.DisplayStart`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f99bd60ef87854d"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipper.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d0bc96e86a96573"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipper.ItemsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe69a4c2c46cf430"></a>[`public float Inno.Native.ImGuizmo.ImGuiListClipper.ItemsHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3dc4e5849e8a01a4"></a>[`public double Inno.Native.ImGuizmo.ImGuiListClipper.StartPosY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b73ac9c175f018dd"></a>[`public double Inno.Native.ImGuizmo.ImGuiListClipper.StartSeekOffsetY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3b8cfc03eb41194"></a>[`public void* Inno.Native.ImGuizmo.ImGuiListClipper.TempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8742) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-708c806f5553bb80"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipperData.ItemsFrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c2ba4fa5f80a148"></a>[`public Inno.Native.ImGuizmo.ImGuiListClipper* Inno.Native.ImGuizmo.ImGuiListClipperData.ListClipper`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00b315fdda2a5703"></a>[`public float Inno.Native.ImGuizmo.ImGuiListClipperData.LossynessOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c88d5df3858de2b6"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange Inno.Native.ImGuizmo.ImGuiListClipperData.Ranges`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd88388c7bc039e0"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipperData.StepNo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bab4b17ffaab409"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipperRange.Max`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8790) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ae437cc9f68e55d"></a>[`public int Inno.Native.ImGuizmo.ImGuiListClipperRange.Min`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8788) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35c3eb92892184cb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexConvert`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8792) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a40f30a80e6dbe72"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexOffsetMax`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8796) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c14cc161982af8ad"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiListClipperRange.PosToIndexOffsetMin`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8794) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00105abe2ea6533d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMenuColumns.NextTotalWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c38db922708c5c9"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetIcon`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8820) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ffad78d13b038f5"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetLabel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8822) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbf647b44eee0ea1"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetMark`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8826) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c05d630ea421d8cf"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiMenuColumns.OffsetShortcut`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8824) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa29e9787c72678e"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiMenuColumns.Spacing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ab6b902da3d5f82"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMenuColumns.TotalWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8814) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1be8ab86c6adcd33"></a>[`public Inno.Native.ImGuizmo.NativeArray_c34fe7f9f3db1a4e Inno.Native.ImGuizmo.ImGuiMenuColumns.Widths`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8828) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa519c60758e746f"></a>[`public int Inno.Native.ImGuizmo.ImGuiMetricsConfig.HighlightMonitorIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8870) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f92de3f530043909"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMetricsConfig.HighlightViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8872) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6d6869358e42643c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDebugLog`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8846) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8d19a8ced5f24366"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDockingNodes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8864) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-04e2890b58cf7c06"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07579c34cce26799"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowDrawCmdMesh`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8856) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8d28386532cfefe5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowFontPreview`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccda39b6937f1a82"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowIdStackTool`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8848) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f39a9c48424175e5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTablesRects`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8854) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b4546f5833f8bf9f"></a>[`public int Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTablesRectsType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8868) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4063f55271a19586"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTextEncodingViewer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8860) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-26338fc43cb35317"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowTextureUsedRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bf0577127245a69"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsBeginOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8852) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-04efae6aa0fcd447"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsRects`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8850) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6cd50f92c2561bec"></a>[`public int Inno.Native.ImGuizmo.ImGuiMetricsConfig.ShowWindowsRectsType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8866) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a6cfc2d496174ee"></a>[`Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceCount = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8892) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-7a6a788d9488452c"></a>[`Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceMouse = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8889) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-aa4b21d7a47f0605"></a>[`Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourcePen = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8891) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-3199aed0cb911d67"></a>[`Inno.Native.ImGuizmo.ImGuiMouseSource.ImGuiMouseSourceTouchScreen = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8890) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-01cfaf58986cb852"></a>[`public int Inno.Native.ImGuizmo.ImGuiMultiSelectIO.ItemsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e960d48d672ed846"></a>[`public long Inno.Native.ImGuizmo.ImGuiMultiSelectIO.NavIdItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8907) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c10a8a793bc73f2"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectIO.NavIdSelected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8909) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-605493f6bdc19b90"></a>[`public long Inno.Native.ImGuizmo.ImGuiMultiSelectIO.RangeSrcItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d084375274d829fa"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectIO.RangeSrcReset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8911) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-16053eb9033599c1"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest Inno.Native.ImGuizmo.ImGuiMultiSelectIO.Requests`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5fafd41a70955ef"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMultiSelectState.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8933) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa137e39b39a8987"></a>[`public int Inno.Native.ImGuizmo.ImGuiMultiSelectState.LastFrameActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-181c4e74ee7eeb10"></a>[`public int Inno.Native.ImGuizmo.ImGuiMultiSelectState.LastSelectionSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8937) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc489b1394d61003"></a>[`public long Inno.Native.ImGuizmo.ImGuiMultiSelectState.NavIdItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8945) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0868870c6302b26c"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiMultiSelectState.NavIdSelected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8941) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8acd1f272499531"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiMultiSelectState.RangeSelected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8939) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-663baf76b649c3a6"></a>[`public long Inno.Native.ImGuizmo.ImGuiMultiSelectState.RangeSrcItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8943) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4011f12c24def825"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiMultiSelectState.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8931) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79c80924d8d26d87"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.BackupCursorMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8973) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c34343da27625d2"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.BoxSelectId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-23fd795a3ed4d00a"></a>[`public int Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8969) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e93fecc118f04136"></a>[`public uint Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.FocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8967) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c2e9180dd3c5083"></a>[`public Inno.Native.ImGuizmo.ImGuiMultiSelectIO Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Io`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8963) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-238ffd11fbc93ae6"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsEndIo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c97c2af50ad48e3f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsFocused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f16575efd2d6fe96"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.IsKeyboardSetRange`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8987) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-896360bc63ada332"></a>[`public int Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.KeyMods`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f626ef613deb4d0"></a>[`public long Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.LastSubmittedItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8975) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80d2a94cae4103be"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.LoopRequestSetAll`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1176eeba02d7acbe"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.NavIdPassedBy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8989) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0637fb263b71d39"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.RangeDstPassedBy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8993) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61fe5c7ba3e326d8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.RangeSrcPassedBy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7e5642b88e5ee9c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.ScopeRectMin`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8971) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae972d36a6855e16"></a>[`public Inno.Native.ImGuizmo.ImGuiMultiSelectState* Inno.Native.ImGuizmo.ImGuiMultiSelectTempData.Storage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8965) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-411cdc4dbd5c588e"></a>[`public float Inno.Native.ImGuizmo.ImGuiNavItemData.DistAxial`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9025) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33d9422fa8a4006e"></a>[`public float Inno.Native.ImGuizmo.ImGuiNavItemData.DistBox`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9021) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b20835fc9381b67e"></a>[`public float Inno.Native.ImGuizmo.ImGuiNavItemData.DistCenter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8886cc3392e9e05"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNavItemData.FocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9015) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5928acee0ad80bfc"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNavItemData.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ae3f06a74a376b1"></a>[`public int Inno.Native.ImGuizmo.ImGuiNavItemData.ItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9019) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35f2acd86478b4d5"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiNavItemData.RectRel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9017) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91f074a8cacad5da"></a>[`public long Inno.Native.ImGuizmo.ImGuiNavItemData.SelectionUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14f994e8ba614522"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiNavItemData.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8625698f5adc81e"></a>[`Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerCount = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9044) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-b0bf62111c0b15dc"></a>[`Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerMain = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9042) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-6ba7bfc726e025c3"></a>[`Inno.Native.ImGuizmo.ImGuiNavLayer.ImGuiNavLayerMenu = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9043) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-16b677a9c7e0c10c"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNextItemData.ColorMarker`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c5ae15e095857fc"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNextItemData.FocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9059) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18651f8daf58e58f"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextItemData.HasFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dd8a650098b56db"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextItemData.ItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-815db460754cee27"></a>[`public byte Inno.Native.ImGuizmo.ImGuiNextItemData.OpenCond`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9071) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-63c79be1ea5e1b3f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiNextItemData.OpenVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9069) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77966b042236973a"></a>[`public Inno.Native.ImGuizmo.ImGuiDataTypeStorage Inno.Native.ImGuizmo.ImGuiNextItemData.RefVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9073) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e409b464c1ffdefa"></a>[`public long Inno.Native.ImGuizmo.ImGuiNextItemData.SelectionUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9061) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6b00e9b89bc754b"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextItemData.Shortcut`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9065) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06213b1566dcfeee"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextItemData.ShortcutFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4088ffcaa9ff496f"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNextItemData.StorageId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a371d7e8d1e2ded9"></a>[`public float Inno.Native.ImGuizmo.ImGuiNextItemData.Width`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9063) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca90f5df244bb628"></a>[`public float Inno.Native.ImGuizmo.ImGuiNextWindowData.BgAlphaVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9129) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7349c58dd4581a5c"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.ChildFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9117) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07589364b40b2df7"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.CollapsedCond`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9101) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1895303d870eb14"></a>[`public byte Inno.Native.ImGuizmo.ImGuiNextWindowData.CollapsedVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25770e281cb3c966"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.ContentSizeVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9111) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db65daf4fb697d35"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.DockCond`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9103) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7d6e06e40cbc64d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNextWindowData.DockId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35150fe5dff1d71a"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.HasFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9095) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-248302e4a315717f"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.MenuBarOffsetMinVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9137) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa7afbad97aaa104"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.PosCond`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9097) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc764832dd02291d"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.PosPivotVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9107) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d23fc32edea4d29e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiNextWindowData.PosUndock`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9119) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15b84bfdda691993"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.PosVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9105) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-094f3366d1f7efdf"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.RefreshFlagsVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9139) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70bf5d6fbd94198c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.ScrollVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9113) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea969842f35d8724"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiSizeCallbackData*, void> Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCallback`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9125) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4336c17effe33016"></a>[`public void* Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCallbackUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9127) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc00534eaf67090d"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeCond`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33bf5d5be6b81009"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeConstraintRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9123) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7905295a1069159"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiNextWindowData.SizeVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9109) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-983c63730ace52da"></a>[`public uint Inno.Native.ImGuizmo.ImGuiNextWindowData.ViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a94f5bd37123443"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowClass Inno.Native.ImGuizmo.ImGuiNextWindowData.WindowClass`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9135) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4505ee11869aaa84"></a>[`public int Inno.Native.ImGuizmo.ImGuiNextWindowData.WindowFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9115) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcf022eee3be1d03"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiOldColumnData.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9163) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c4c63e9dfa64856"></a>[`public int Inno.Native.ImGuizmo.ImGuiOldColumnData.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9161) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85b411afe4452664"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumnData.OffsetNorm`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9157) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4373643f37649ee2"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumnData.OffsetNormBeforeResize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-010e78d325c6290d"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData Inno.Native.ImGuizmo.ImGuiOldColumns.Columns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9211) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-57c417a8485a894a"></a>[`public int Inno.Native.ImGuizmo.ImGuiOldColumns.Count`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9191) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29658a976f14fc83"></a>[`public int Inno.Native.ImGuizmo.ImGuiOldColumns.Current`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b39d790a290242e"></a>[`public int Inno.Native.ImGuizmo.ImGuiOldColumns.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7374af5efcb7fad"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiOldColumns.HostBackupClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9207) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db4e86b514d794d6"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiOldColumns.HostBackupParentWorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9209) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e13d229535493d2"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.HostCursorMaxPosX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9203) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bf27c9cf9fb3467"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.HostCursorPosY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9201) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbf59becb5f16ad6"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiOldColumns.HostInitialClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9205) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-286b5e5daa11672e"></a>[`public uint Inno.Native.ImGuizmo.ImGuiOldColumns.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9181) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2ec44f7f5154aad"></a>[`public byte Inno.Native.ImGuizmo.ImGuiOldColumns.IsBeingResized`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb593660c5a38400"></a>[`public byte Inno.Native.ImGuizmo.ImGuiOldColumns.IsFirstFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9185) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ab14cf2c015c417"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.LineMaxY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9199) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc5284ccc3304fdf"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.LineMinY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9197) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-108b1ed1cc905a09"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.OffMaxX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb4cc1b75cc90b7d"></a>[`public float Inno.Native.ImGuizmo.ImGuiOldColumns.OffMinX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9193) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d8584886d03fdea"></a>[`public Inno.Native.ImGuizmo.ImDrawListSplitter Inno.Native.ImGuizmo.ImGuiOldColumns.Splitter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08a60f6014d0292b"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPayload.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a688fbba7d3e296d"></a>[`public int Inno.Native.ImGuizmo.ImGuiPayload.DataFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9239) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f2a11b78564d84f"></a>[`public int Inno.Native.ImGuizmo.ImGuiPayload.DataSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b90c2d664062030"></a>[`public Inno.Native.ImGuizmo.NativeArray_704527fa6c941baa Inno.Native.ImGuizmo.ImGuiPayload.DataType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9241) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b5c0b7d0ff95e6aa"></a>[`public byte Inno.Native.ImGuizmo.ImGuiPayload.Delivery`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9245) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9ef136ffe8eb709"></a>[`public byte Inno.Native.ImGuizmo.ImGuiPayload.Preview`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9243) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efefd8d8aef89ef3"></a>[`public uint Inno.Native.ImGuizmo.ImGuiPayload.SourceId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e84556cb4b157ce9"></a>[`public uint Inno.Native.ImGuizmo.ImGuiPayload.SourceParentId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e4f3836ba605cf4"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor Inno.Native.ImGuizmo.ImGuiPlatformIO.Monitors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9361) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa9b33214a1c0fb2"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformClipboardUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9293) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65a5272e63eaa60e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, ulong, void*, ulong*, int> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformCreateVkSurface`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9e8cb3ad0e77d19"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformCreateWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9311) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-db8e21ffee45cfdf"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformDestroyWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9313) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6817d8eebc69eed1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, sbyte*> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetClipboardTextFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9289) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8eb5e9c1c124621e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, float> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowDpiScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9343) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-31e3a3e6a21fb0f0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, byte> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowFocus`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60174af1b0fd4a2c"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowFramebufferScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06fe73a53304e132"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, byte> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowMinimized`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f2326baeaf5dbb6"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9319) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e8c6ab5434de3bb"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c18ccb2255efa8e1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec4C> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9347) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dd41636dd9ca07f"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformImeUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9301) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5fd2f0f34315906"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformLocaleDecimalPoint`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9303) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ace1035a9a7fc831"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOnChangedViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9345) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42731c105c585ac5"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, sbyte*, byte> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOpenInShellFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9295) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a33bf87fdab3689"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformOpenInShellUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac97d021f72c15a1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformRenderWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b798a77ef4f0e92"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, sbyte*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetClipboardTextFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9291) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18051c6c301ce1e8"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImGuiPlatformImeData*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetImeDataFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9299) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f9b269b2117bb63"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, float, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowAlpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9335) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-847d8164065ed574"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowFocus`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3430a0ee0c0f9ecf"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9317) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0bab56a4612b01f9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9321) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5500290a41f73d7c"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, sbyte*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSetWindowTitle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9333) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4b6fb5fd5c17bd7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformShowWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9315) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aba258a2396d2340"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformSwapBuffers`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ad06caa02d423673"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.PlatformUpdateWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-878d1dbb93687c2e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererCreateWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95a0a42f2854bda8"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererDestroyWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9353) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-74e42b446ac83215"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererRenderState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9e3663c3f643297"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererRenderWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9357) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bcb9c7452826a27d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, Inno.Native.ImGuizmo.ImVec2C, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererSetWindowSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9355) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b12e91f14107136"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiViewport*, void*, void> Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererSwapBuffers`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9359) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d062df0137e160f7"></a>[`public int Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererTextureMaxHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9307) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1908fc637b2fb49"></a>[`public int Inno.Native.ImGuizmo.ImGuiPlatformIO.RendererTextureMaxWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9305) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e14c0a1c1e438be"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureDataPtr Inno.Native.ImGuizmo.ImGuiPlatformIO.Textures`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9363) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-876745afb277abb5"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr Inno.Native.ImGuizmo.ImGuiPlatformIO.Viewports`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f8b3fb9ef3be87f"></a>[`public float Inno.Native.ImGuizmo.ImGuiPlatformImeData.InputLineHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9269) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e91bc8bc88056f3"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPlatformImeData.InputPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9267) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ac140d86e701f34"></a>[`public uint Inno.Native.ImGuizmo.ImGuiPlatformImeData.ViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9271) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49ca200864d23d7b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiPlatformImeData.WantTextInput`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9265) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33a7abdf0fdf2fca"></a>[`public byte Inno.Native.ImGuizmo.ImGuiPlatformImeData.WantVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9263) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecc245a8b8409dc8"></a>[`public float Inno.Native.ImGuizmo.ImGuiPlatformMonitor.DpiScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d1d63040f19f0a2"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPlatformMonitor.MainPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee2043433afe177c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPlatformMonitor.MainSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d1e14896642809b"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPlatformMonitor.PlatformHandle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c31e2539cb3fd85e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPlatformMonitor.WorkPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9387) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e63580af6774e0b8"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPlatformMonitor.WorkSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5d632b7d0be679d6"></a>[`public int Inno.Native.ImGuizmo.ImGuiPopupData.OpenFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14de9c7e36267185"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPopupData.OpenMousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61f0e5004c653efd"></a>[`public uint Inno.Native.ImGuizmo.ImGuiPopupData.OpenParentId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5331490d10dd2c37"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiPopupData.OpenPopupPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91513d2c5128ceb0"></a>[`public int Inno.Native.ImGuizmo.ImGuiPopupData.ParentNavLayer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9417) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3e7505647a7ef63"></a>[`public uint Inno.Native.ImGuizmo.ImGuiPopupData.PopupId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9411) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a6a12d827998bf69"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiPopupData.RestoreNavWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9415) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a804f076b9510c6"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiPopupData.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9413) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-339af8dfa1bacaa0"></a>[`public int Inno.Native.ImGuizmo.ImGuiPtrOrIndex.Index`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-daf0c2eb6281d433"></a>[`public void* Inno.Native.ImGuizmo.ImGuiPtrOrIndex.Ptr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-385da98a3e9c4789"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeDirection`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9467) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-206e02f4390fdd22"></a>[`public long Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeFirstItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe78d2b4843addee"></a>[`public long Inno.Native.ImGuizmo.ImGuiSelectionRequest.RangeLastItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d506b8ce08f49f3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiSelectionRequest.Selected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b3268d988339dcb"></a>[`public Inno.Native.ImGuizmo.ImGuiSelectionRequestType Inno.Native.ImGuizmo.ImGuiSelectionRequest.Type`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58c5df1d17b34acb"></a>[`Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-e0262dbc0d0bd9c9"></a>[`Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeSetAll = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-45e0358b48f82d56"></a>[`Inno.Native.ImGuizmo.ImGuiSelectionRequestType.ImGuiSelectionRequestTypeSetRange = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-17d584ff01ea1ce9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, void> Inno.Native.ImGuizmo.ImGuiSettingsHandler.ApplyAllFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8493c2ac5294c73d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, void> Inno.Native.ImGuizmo.ImGuiSettingsHandler.ClearAllFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eea5a7d645f143ab"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, void> Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadInitFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9505) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4180d530f2f553bc"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, void*, sbyte*, void> Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadLineFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65e5f23642c41948"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, sbyte*, void*> Inno.Native.ImGuizmo.ImGuiSettingsHandler.ReadOpenFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9507) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5372c81784ff1d9b"></a>[`public uint Inno.Native.ImGuizmo.ImGuiSettingsHandler.TypeHash`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f93ad3ce1f877a91"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiSettingsHandler.TypeName`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1630e0610527973"></a>[`public void* Inno.Native.ImGuizmo.ImGuiSettingsHandler.UserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d4ad437bd4bd08a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGuizmo.ImGuiContext*, Inno.Native.ImGuizmo.ImGuiSettingsHandler*, Inno.Native.ImGuizmo.ImGuiTextBuffer*, void> Inno.Native.ImGuizmo.ImGuiSettingsHandler.WriteAllFn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-153e878cdae045f1"></a>[`public int Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.Index`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4c233919be2f40c"></a>[`public float Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.InitialWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-66e5a4a0d9cc6923"></a>[`public float Inno.Native.ImGuizmo.ImGuiShrinkWidthItem.Width`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9535) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80147454ff93b16b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiSizeCallbackData.CurrentSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f5eda1013475e3a"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiSizeCallbackData.DesiredSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b857a50c645a3e6b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiSizeCallbackData.Pos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5038f972d724166"></a>[`public void* Inno.Native.ImGuizmo.ImGuiSizeCallbackData.UserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-279e3198bbc0cce3"></a>[`Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionAscending = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-8bdda284bab361bc"></a>[`Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionDescending = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-6a7626f099c0ab48"></a>[`Inno.Native.ImGuizmo.ImGuiSortDirection.ImGuiSortDirectionNone = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-4308da481d65a8c7"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiStackLevelInfo.DataType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9601) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3030c37d4c892054"></a>[`public int Inno.Native.ImGuizmo.ImGuiStackLevelInfo.DescOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9603) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1c0a04ddde94eb2"></a>[`public uint Inno.Native.ImGuizmo.ImGuiStackLevelInfo.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9595) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64e29729d34d622d"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiStackLevelInfo.QueryFrameCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9597) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-135e40daab59aac9"></a>[`public byte Inno.Native.ImGuizmo.ImGuiStackLevelInfo.QuerySuccess`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9599) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb684c2b66d30ea6"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiStoragePair Inno.Native.ImGuizmo.ImGuiStorage.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-619705ca2e4d1e5d"></a>[`public Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0 Inno.Native.ImGuizmo.ImGuiStoragePair.AnonymousField1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-12ad47c94224a5ac"></a>[`public uint Inno.Native.ImGuizmo.ImGuiStoragePair.Key`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e15cb8b098382e2"></a>[`public float Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValF`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9661) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eccb005af7867a13"></a>[`public int Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValI`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9659) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e21f809dba646d3a"></a>[`public void* Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValP`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9663) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-765f1518fbffa5b4"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.Alpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9687) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-665b48d04db797f5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedFill`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9807) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-832123ab97bac7fc"></a>[`public byte Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedLines`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46e92786979fbab9"></a>[`public byte Inno.Native.ImGuizmo.ImGuiStyle.AntiAliasedLinesUseTex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9805) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a856226420a2421f"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.ButtonTextAlign`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9783) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4662acf7df9f064b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.CellPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9723) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f7926d7c0291b0e"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ChildBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9707) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3cc2bc3d9794de1b"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ChildRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9705) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9840fca06bcd9ced"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.CircleTessellationMaxError`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9811) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-882750eecf3c0385"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiStyle.ColorButtonPosition`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9781) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32201960d9f1382f"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ColorMarkerSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9779) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bbccf2c6193214f"></a>[`public Inno.Native.ImGuizmo.NativeArray_afa9ec5b5680f876 Inno.Native.ImGuizmo.ImGuiStyle.Colors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9813) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-685527e0701169a7"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ColumnsMinSpacing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9729) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc740071f2818d79"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.CurveTessellationTol`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9809) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-398cc9dc9530e747"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.DisabledAlpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9689) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14ee70e4995da8d0"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.DisplaySafeAreaPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9795) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f25611afbe460171"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.DisplayWindowPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9793) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ce04f2721470963"></a>[`public byte Inno.Native.ImGuizmo.ImGuiStyle.DockingNodeHasCloseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9797) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-288f2bc78a9ecb73"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.DockingSeparatorSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9799) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f292c19306709b1"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9775) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4dc9e21a0b6f1ce"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9777) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2c0151d168f8c64"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.DragDropTargetRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9773) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0122fff6d8077356"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.FontScaleDpi`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9685) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f3d52b4657dad22"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.FontScaleMain`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9683) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d113eedfc5c2812a"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.FontSizeBase`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9681) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9beaf48c23115826"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.FrameBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9717) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c58c5316252610b5"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.FramePadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9713) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa6ee0888afb18e8"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.FrameRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9715) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f562d6a8c4e1382"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.GrabMinSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6512f249daa17516"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.GrabRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9739) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c944688a0ee5a83"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.HoverDelayNormal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9819) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41a2012c455e59d9"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.HoverDelayShort`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9817) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27f9849228476869"></a>[`public int Inno.Native.ImGuizmo.ImGuiStyle.HoverFlagsForTooltipMouse`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a448f4c48afd3efa"></a>[`public int Inno.Native.ImGuizmo.ImGuiStyle.HoverFlagsForTooltipNav`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9823) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3dc035d5eaa1f95"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.HoverStationaryDelay`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9815) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8322961581d25d80"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ImageBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9745) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ddc05808451c5fea"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ImageRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9743) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7dadeac471e7cc96"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.IndentSpacing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9727) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76ba459bc7b5c236"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.ItemInnerSpacing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9721) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4db0f2f47feb8797"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.ItemSpacing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9719) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-621976cd4d528d58"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.LogSliderDeadzone`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9741) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25021a732d519df9"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.MainScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9825) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a25aaf4fe11ef375"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.MouseCursorScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21c7d32ce61cd0e0"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.NextFrameFontSizeBase`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9827) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac26001d5497ded3"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.PopupBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9711) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-609d163dad40a2fe"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.PopupRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9709) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0843645694d88a02"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24039f8a1dfeefaf"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9733) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89e94e467aefdd48"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.ScrollbarSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9731) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-135cd23767430bc5"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.SelectableTextAlign`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9785) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae216f016eab4781"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextAlign`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9789) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-979a44abf5dff1f6"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9787) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f15f927e7757bdea"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.SeparatorTextPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9791) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3f41f2acc1f7657"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabBarBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92a501ba9cac3973"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabBarOverlineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9761) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-615ac26e534ddd05"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9749) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a02fd33151331c20"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabCloseButtonMinWidthSelected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9755) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da1ecf328b87e08c"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabCloseButtonMinWidthUnselected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9757) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc37703adcd7327f"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabMinWidthBase`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9751) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-534a912d9912e0f8"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabMinWidthShrink`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9753) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e599edd470f2f92f"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TabRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9747) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d151fc84e7cf5b0c"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TableAngledHeadersAngle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9763) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6332607d7aa2eae"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.TableAngledHeadersTextAlign`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9765) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9a55ba133009138"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.TouchExtraPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9725) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5abec0bfc9e669e0"></a>[`public int Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9767) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a07d498861832466"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9771) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d96a6ecc24a72f88"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.TreeLinesSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9769) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2fe4b3d506487525"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.WindowBorderHoverPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9697) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5aed336aacb61ff6"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.WindowBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9695) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f868baee1d0b162"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiStyle.WindowMenuButtonPosition`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9703) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4eb0942bce358b3"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.WindowMinSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9699) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9922609b882b301e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.WindowPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5d95953a0aab5258"></a>[`public float Inno.Native.ImGuizmo.ImGuiStyle.WindowRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d0416f934281d44"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiStyle.WindowTitleAlign`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9701) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0dabf51f320cbb5"></a>[`public Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2 Inno.Native.ImGuizmo.ImGuiStyleMod.AnonymousField1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72a1e60fb669ded2"></a>[`public int Inno.Native.ImGuizmo.ImGuiStyleMod.VarIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed5ee063a7d758f4"></a>[`public Inno.Native.ImGuizmo.NativeArray_3571fc580e8d9e55 Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupFloat`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9867) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1bcde589a88371e"></a>[`public Inno.Native.ImGuizmo.NativeArray_b3004bc9c15942c6 Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupInt`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-77ec77b14bbe041b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTabBar.BackupCursorPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9955) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aaaee4e122f34522"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTabBar.BarRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a4b520396dc7007"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.BarRectPrevWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9907) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e157e7febec9a96"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiTabBar.BeginCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9937) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41aa9830fe48d320"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabBar.CurrFrameVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9901) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cf50ea82d828c234"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.CurrTabsContentsHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9909) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-565c4d9241f12f31"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabBar.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9889) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-458ee115b8e9d660"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTabBar.FramePadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-217785eddff224e0"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9891) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae1a0b89df024f33"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ItemSpacingY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9951) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-45d87b239faa720e"></a>[`public short Inno.Native.ImGuizmo.ImGuiTabBar.LastTabItemIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9949) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e140f57f55aa0930"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.NextScrollToTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe701b1a4c4c3230"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.NextSelectedTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9895) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43b24fb2d2f4d94f"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabBar.PrevFrameVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fce84a73ce1ca5c3"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.PrevTabsContentsHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9911) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e0c2bb59a31a7fc"></a>[`public short Inno.Native.ImGuizmo.ImGuiTabBar.ReorderRequestOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-096d1004d6cdf04c"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.ReorderRequestTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9933) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a78fb9a6f87ec99d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTabBar.ScrollButtonEnabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9945) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83f6c323124d8a62"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingAnim`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9917) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c828262b06d548a"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingRectMaxX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9927) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c147a007320320e2"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingRectMinX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9925) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b52abffca89005fa"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingSpeed`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9923) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21934df736cfe3d3"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingTarget`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9919) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1c46e1502e222df"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.ScrollingTargetDistToVisibility`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9921) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92c0108db859c935"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.SelectedTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9893) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a6cf9e8820fc8558"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.SeparatorMaxX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9931) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b701f70b03dff5ed"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.SeparatorMinX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9929) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-948061ec4c7ec15d"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTabItem Inno.Native.ImGuizmo.ImGuiTabBar.Tabs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9887) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75dd9e4581ed645b"></a>[`public short Inno.Native.ImGuizmo.ImGuiTabBar.TabsActiveCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9947) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a2e2efd2d88fa07"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTabBar.TabsAddedNew`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9943) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64bd6bad0fbd5899"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiTabBar.TabsNames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18ec7a0fe2a632a0"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabBar.VisibleTabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9899) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-182b50b5511cf203"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTabBar.VisibleTabWasSubmitted`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9941) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3234c80fa559f44c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTabBar.WantLayout`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9939) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39da6e746dcd2087"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.WidthAllTabs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17762492f7bca5c3"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabBar.WidthAllTabsIdeal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9915) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-982ab45634b25c83"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiTabBar.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9885) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6fcb22abb4fbfea"></a>[`public short Inno.Native.ImGuizmo.ImGuiTabItem.BeginOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9995) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-939237dba5949079"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabItem.ContentWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9989) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4751e538e7911517"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabItem.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-45a820aa8a93630f"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTabItem.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9975) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd0b646f8867e489"></a>[`public short Inno.Native.ImGuizmo.ImGuiTabItem.IndexDuringLayout`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9997) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e6da46caf84ff6c7"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabItem.LastFrameSelected`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-325721e04748c14a"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabItem.LastFrameVisible`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4da2d923d1e78acb"></a>[`public int Inno.Native.ImGuizmo.ImGuiTabItem.NameOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9993) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e65335202408dca"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabItem.Offset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8d0c56832e3a546"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabItem.RequestedWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f1d4a6b9ff4ccee4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTabItem.WantClose`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9999) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe93ee67caa8e0ce"></a>[`public float Inno.Native.ImGuizmo.ImGuiTabItem.Width`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9987) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-336228524013a4a4"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiTabItem.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b749bf7d5428b028"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10153) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37cfc32bb0ec5614"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10105) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-361d97baac404137"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.AngledHeadersSlope`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10107) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1edaca7dbb60d7e2"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.AutoFitSingleColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10161) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f0f0ae8a3ed7dd9"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.Bg0ClipRectForDrawCmd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10119) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-26f144c656db1a85"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.Bg2ClipRectForDrawCmd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c10a08553ff3164"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTable.Bg2DrawChannelCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2332f191a89c24ae"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTable.Bg2DrawChannelUnfrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10197) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-271c0d8691da0bb2"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.BgClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10117) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6532015ffc103b42"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTable.BorderColorLight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10073) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef93576726c838c8"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTable.BorderColorStrong`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10071) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9825837d5987de2b"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.BorderX1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-643597fc02d456a4"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.BorderX2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ae938562341ccf9"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.CellPaddingX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10085) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c751bb269020ff68"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.CellSpacingX1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10087) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65d2542ada1ebeef"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.CellSpacingX2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20e0e1d8554d9701"></a>[`public Inno.Native.ImGuizmo.ImSpanImGuiTableColumn Inno.Native.ImGuizmo.ImGuiTable.Columns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a527f0da99783f4d"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.ColumnsAutoFitWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10095) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24f0aac35941fb7b"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.ColumnsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10045) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-227b385afa668901"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ColumnsEnabledCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10147) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f607c70eb60579c"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ColumnsEnabledFixedCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10149) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-edb5e076e6be0112"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.ColumnsGivenWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10093) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84d479d6457145b1"></a>[`public Inno.Native.ImGuizmo.ImGuiTextBuffer Inno.Native.ImGuizmo.ImGuiTable.ColumnsNames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bcaa4e284d887d0d"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.ColumnsStretchSumWeights`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10097) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79e3b7170ecb0eeb"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ContextPopupColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10181) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-615768e926695b5c"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.CurrentColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10049) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a185638d4a64cbae"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.CurrentRow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10047) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bde4ea50128e5d3d"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.DeclColumnsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10151) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ec4c65bd80be1a5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.DisableDefaultContextMenu`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bdcd9ea7a591767"></a>[`public Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx Inno.Native.ImGuizmo.ImGuiTable.DisplayOrderToIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10029) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-09b890af7e022f20"></a>[`public Inno.Native.ImGuizmo.ImDrawListSplitter* Inno.Native.ImGuizmo.ImGuiTable.DrawSplitter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7079f2d3b227f6a0"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTable.DummyDrawChannel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10193) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1c780ede9444438"></a>[`public uint* Inno.Native.ImGuizmo.ImGuiTable.EnabledMaskByDisplayOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10033) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1809b88822ec0c9"></a>[`public uint* Inno.Native.ImGuizmo.ImGuiTable.EnabledMaskByIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bcf5a39558b5dd2c"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10021) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-427c6ec511644440"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.FreezeColumnsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f17c08f1e507c989"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.FreezeColumnsRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4c010343373bb3f"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.FreezeRowsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10185) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f016520c92501325"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.FreezeRowsRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa3df9450245236d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.HasScrollbarYCurr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f98f61b2293da0f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.HasScrollbarYPrev`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c50d6f196518a66f"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.HeldHeaderColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10167) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b364fbbdb609d04"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.HighlightColumnHeader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3372d78019b809ed"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.HostBackupInnerClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10125) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5460770fbabefbea"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.HostClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10123) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4762030b8385769"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.HostIndentX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b30b30b104e085c"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.HostSkipItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10239) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dafeaa8c026883d7"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.HoveredColumnBody`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10155) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90f3a761429079a2"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.HoveredColumnBorder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10157) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-818fa93d79dd9211"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTable.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10019) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8067c4251d0b560e"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.InnerClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10115) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa590f3a9f34619a"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.InnerRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10111) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-561704ee1b293533"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.InnerWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b5e201f2958e5694"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiTable.InnerWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10129) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ac431158e29f727"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.InstanceCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10051) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10c7b6f7844b95aa"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData Inno.Native.ImGuizmo.ImGuiTable.InstanceDataExtra`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10137) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-133665101481058c"></a>[`public Inno.Native.ImGuizmo.ImGuiTableInstanceData Inno.Native.ImGuizmo.ImGuiTable.InstanceDataFirst`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10135) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-57bf4a7cf0080fd0"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.InstanceInteracted`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10053) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-312bdd7d6e1c9e7f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsActiveIdAliveBeforeTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10229) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb900333c538c76b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsActiveIdInTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7d9030d628ebb27"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsContextPopupOpen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10211) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ad53956a1fc5a0e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsDefaultDisplayOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-36b36023b2ac671a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsDefaultSizingPolicy`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10227) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd3ed70796cac6c1"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsInitializing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10205) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-230f625af51e08e9"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsInsideRow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10203) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5040c6bb51fb189"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsLayoutLocked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10201) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f2e6df4396adc18"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsResetAllRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-baab3abfbe24c1a9"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsResetDisplayOrderRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81dc68f2a009b36d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsSettingsDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10217) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1595485b604e52ec"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsSettingsRequestLoad`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10215) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87f512bce73fca43"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsSortSpecsDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10207) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-edb72f5945ecd015"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsUnfrozenRows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10225) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9b192f70eb57518"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.IsUsingHeaders`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10209) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9a1bebd930ccaf5e"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.LastFrameActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10043) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f93cbb925b52cf7d"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.LastResizedColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10165) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7b7d6dfdc84742a"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.LeftMostEnabledColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10173) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f75a776c4c658315"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.LeftMostStretchedColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10177) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-033e5d49c62da2d5"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTable.MemoryCompacted`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c21e68f8e7eb234"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.MinColumnWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5081016f0b575e40"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiTable.NavLayer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10199) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35943760b949054d"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.OuterPaddingX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10083) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c649922be319f1f4"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.OuterRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10109) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b8e8cb9ece150af"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiTable.OuterWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10127) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07b12832cb99d237"></a>[`public void* Inno.Native.ImGuizmo.ImGuiTable.RawData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abdd17187300a9b8"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RefScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10103) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8de0c5529b855cc7"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ReorderColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10169) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9025e57d9a6c8a92"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ReorderColumnDir`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10171) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af89c4aa47ff7af9"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.ResizeLockMinContentsX2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10101) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4ee5c96ed0d7107"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.ResizedColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10163) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ae8453d18c09dc4"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.ResizedColumnNextWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0e47b9f4fbd90ba"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.RightMostEnabledColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10175) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4918656ec822515"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.RightMostStretchedColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10179) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0643635eb595cd9d"></a>[`public Inno.Native.ImGuizmo.NativeArray_9be1bbaad6627fe8 Inno.Native.ImGuizmo.ImGuiTable.RowBgColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10069) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d400edde125e348e"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.RowBgColorCounter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-931e6572cb0d8f22"></a>[`public Inno.Native.ImGuizmo.ImSpanImGuiTableCellData Inno.Native.ImGuizmo.ImGuiTable.RowCellData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf02505ca5a0f7f6"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.RowCellDataCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10191) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-385569afb9c0f626"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowCellPaddingY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10061) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9afceb0757d3b593"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowIndentOffsetX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10065) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-558296a23c814ae4"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowMinHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10059) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17a64b9c6ae57e4a"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowPosY1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcab39c04d2381de"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowPosY2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f29f1d5a3c4af51"></a>[`public float Inno.Native.ImGuizmo.ImGuiTable.RowTextBaseline`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10063) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86ebbf8334647a6e"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.SettingsLoadedFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10039) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-905c12df83db6640"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.SettingsOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10041) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-36fbf0d01649fe38"></a>[`public Inno.Native.ImGuizmo.ImGuiTableSortSpecs Inno.Native.ImGuizmo.ImGuiTable.SortSpecs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10143) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f37105fbdc06440"></a>[`public short Inno.Native.ImGuizmo.ImGuiTable.SortSpecsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10145) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d49bc03336de5499"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs Inno.Native.ImGuizmo.ImGuiTable.SortSpecsMulti`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10141) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03a3d28be44f102f"></a>[`public Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs Inno.Native.ImGuizmo.ImGuiTable.SortSpecsSingle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10139) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01379aed225b0d5e"></a>[`public Inno.Native.ImGuizmo.ImGuiTableTempData* Inno.Native.ImGuizmo.ImGuiTable.TempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10025) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06194cbb608bfc29"></a>[`public uint* Inno.Native.ImGuizmo.ImGuiTable.VisibleMaskByIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10037) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e6ff5bfe78986fb9"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTable.WorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10113) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4202d106bc47aa33"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableCellData.BgColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10313) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3db914659713cdbc"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableCellData.Column`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10315) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28a4c748e37d7603"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.AutoFitQueue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10407) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc2e77d20a833d67"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.CannotSkipItemsQueue`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10409) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-998a78379da74624"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTableColumn.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10353) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86d7e86882dc9466"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXFrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10363) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e93f757034c932a"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXHeadersIdeal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10369) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b2571e2d7c54e18"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXHeadersUsed`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10367) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3deaa5c110e7ceff"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.ContentMaxXUnfrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9330fc9d0f778bd4"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.DisplayOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe8ad88e24f8fdbb"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a51e37e2e053711"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelFrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-30950818514aa8dc"></a>[`public ushort Inno.Native.ImGuizmo.ImGuiTableColumn.DrawChannelUnfrozen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10387) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9389ca9f76a78b0d"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableColumn.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10335) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-336acdc7b8fa8995"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.IndexWithinEnabledSet`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8febfdc2475e2261"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.InitStretchWeightOrWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7cd9ce46cc1fc550"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsEnabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec3fde390ff6b36b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsPreserveWidthAuto`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10403) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-31608342f40c4b6e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsRequestOutput`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca553f709e14dfb4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsSkipItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10401) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b43042d0725c046"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsUserEnabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c5c9b0cd661d564"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsUserEnabledNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9db19dc42f6e35f"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsVisibleX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac0624f249ed3658"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.IsVisibleY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-527e0b0b73a49229"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.ItemWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10361) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-674ef6cba3d89242"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.MaxX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9661c42fafa2c737"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.MinX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a3757db82aa8a614"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.NameOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10371) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5160d93646dad2c1"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiTableColumn.NavLayerCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10405) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-545783d753325667"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.NextEnabledColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2063d6c282f94f32"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.PrevEnabledColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10377) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f3d135d1442e42e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailList`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10411) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39d9221d57e51837"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumn.SortOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecb0623354682ad5"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.StretchWeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d0a8d89fc862650"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableColumn.UserId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10355) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7937707df6e313df"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WidthAuto`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10345) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-50877fede08c6508"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WidthGiven`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ec2a43977ceae28"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WidthMax`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10347) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdbea6f98e3ed4f3"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WidthRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10343) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd3ff593b1452a2e"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WorkMaxX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10359) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75d3bf9c14a094dc"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableColumn.WorkMinX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10357) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13aaf933888f2bd5"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.ColumnIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25238486d75366a2"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.ColumnUserId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c042ddbe1e26e05"></a>[`public Inno.Native.ImGuizmo.ImGuiSortDirection Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.SortDirection`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e6a01b1f5cb087e"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs.SortOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c797b762b671b9d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableHeaderData.BgColor0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10545) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fb5734ed03edd434"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableHeaderData.BgColor1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2b994d09f4665aa"></a>[`public short Inno.Native.ImGuizmo.ImGuiTableHeaderData.Index`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10541) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ae9c8788d02000d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableHeaderData.TextColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10543) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f8b5d00ce6f52879"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableInstanceData.HoveredRowLast`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-946ba224172f4ced"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableInstanceData.HoveredRowNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10575) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d92ec253c83cdd49"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastFrozenHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fed32c585d0e455"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastOuterHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06ccef90131570d4"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableInstanceData.LastTopHeadersRowHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9170e967deebb636"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableInstanceData.TableInstanceId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c64f02afdd44bc72"></a>[`public Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs* Inno.Native.ImGuizmo.ImGuiTableSortSpecs.Specs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10593) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ff3d9bf815e5117"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableSortSpecs.SpecsCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10595) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b68423664cbcd1eb"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableSortSpecs.SpecsDirty`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10597) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fad20096c043191f"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableTempData.AngledHeadersExtraWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86c3192677f0bf0c"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData Inno.Native.ImGuizmo.ImGuiTableTempData.AngledHeadersRequests`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10623) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba039a17e974e31b"></a>[`public Inno.Native.ImGuizmo.ImDrawListSplitter Inno.Native.ImGuizmo.ImGuiTableTempData.DrawSplitter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10627) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-119cce461870d554"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupColumnsOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15fff7d89a6fa7b5"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupCurrLineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10635) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ed63381cc7186e7"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupCursorMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10637) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8a0beee52b638d0"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupItemWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e679a95be60a522a"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupItemWidthStackSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f8e31c26e7c74a4"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupParentWorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10631) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42323f04c9380909"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupPrevLineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10633) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83eb2bae2a7702b0"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTableTempData.HostBackupWorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10629) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce665b79bc8f48db"></a>[`public float Inno.Native.ImGuizmo.ImGuiTableTempData.LastTimeActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9e331557ef4b9a1"></a>[`public int Inno.Native.ImGuizmo.ImGuiTableTempData.TableIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10617) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6d1ad70a844df66a"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiTableTempData.UserOuterSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10625) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b17242b62a76c9f9"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTableTempData.WindowId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10615) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0ed1f01eca277d1"></a>[`public Inno.Native.ImGuizmo.ImVectorChar Inno.Native.ImGuizmo.ImGuiTextBuffer.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10661) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6514c078cbdd0a1c"></a>[`public int Inno.Native.ImGuizmo.ImGuiTextIndex.EndOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10681) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef45c335211656d2"></a>[`public Inno.Native.ImGuizmo.ImVectorInt Inno.Native.ImGuizmo.ImGuiTextIndex.Offsets`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10679) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d05328e68461297"></a>[`public short Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesTableColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10711) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c60f80c1c7a166b0"></a>[`public float Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesToNodesY2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10709) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-053d392af9e2342b"></a>[`public float Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.DrawLinesX1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10707) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-426e3ff74a5aec85"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10699) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a63fda363583913"></a>[`public int Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.ItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10703) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3dc4b096676fcb95"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.NavRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10705) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53d0557c15fa1f14"></a>[`public int Inno.Native.ImGuizmo.ImGuiTreeNodeStackData.TreeFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10701) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca07f7bc2526286b"></a>[`public int Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10729) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee1dbffa8369709c"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SearchBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10733) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bd564d2a6102fd1"></a>[`public int Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SearchBufferLen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10731) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13fe2af21892e70a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SelectRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-021cb2e02e29e90d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SingleCharMode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0210588c44e3d0da"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiTypingSelectRequest.SingleCharSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10739) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-760b106b3d5985a3"></a>[`public uint Inno.Native.ImGuizmo.ImGuiTypingSelectState.FocusScope`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10761) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-09e4c8cb087d430d"></a>[`public int Inno.Native.ImGuizmo.ImGuiTypingSelectState.LastRequestFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10763) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af7dfc914389d835"></a>[`public float Inno.Native.ImGuizmo.ImGuiTypingSelectState.LastRequestTime`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10765) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e65128da4f42e18"></a>[`public Inno.Native.ImGuizmo.ImGuiTypingSelectRequest Inno.Native.ImGuizmo.ImGuiTypingSelectState.Request`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10757) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-943f18c8f5389d7f"></a>[`public Inno.Native.ImGuizmo.NativeArray_f4284eefd115ca43 Inno.Native.ImGuizmo.ImGuiTypingSelectState.SearchBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6601c1375756d1c6"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTypingSelectState.SingleCharModeLock`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10767) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb61608397cab39e"></a>[`public float Inno.Native.ImGuizmo.ImGuiViewport.DpiScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10799) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e8d5e61ecbe1e450"></a>[`public Inno.Native.ImGuizmo.ImDrawData* Inno.Native.ImGuizmo.ImGuiViewport.DrawData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10805) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3709355569f290f"></a>[`public int Inno.Native.ImGuizmo.ImGuiViewport.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10787) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0724cd9a9d4a6e85"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewport.FramebufferScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10793) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b2f20c190bb24bd"></a>[`public uint Inno.Native.ImGuizmo.ImGuiViewport.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10785) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f88258e12dbd1d66"></a>[`public Inno.Native.ImGuizmo.ImGuiViewport* Inno.Native.ImGuizmo.ImGuiViewport.ParentViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e69fffb26797016"></a>[`public uint Inno.Native.ImGuizmo.ImGuiViewport.ParentViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4531474bae0d1d67"></a>[`public void* Inno.Native.ImGuizmo.ImGuiViewport.PlatformHandle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10811) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-301419036b72a175"></a>[`public void* Inno.Native.ImGuizmo.ImGuiViewport.PlatformHandleRaw`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10813) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33c2fe92d3af6503"></a>[`public byte Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestClose`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f844b5ac32b18e4a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestMove`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10817) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ab866eba2b73659"></a>[`public byte Inno.Native.ImGuizmo.ImGuiViewport.PlatformRequestResize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10819) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ea2174cd308b075"></a>[`public void* Inno.Native.ImGuizmo.ImGuiViewport.PlatformUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10809) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-348ea7b3a921e6a6"></a>[`public byte Inno.Native.ImGuizmo.ImGuiViewport.PlatformWindowCreated`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10815) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-893ce7826fa35992"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewport.Pos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10789) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-001ae8991200170d"></a>[`public void* Inno.Native.ImGuizmo.ImGuiViewport.RendererUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10807) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f36f30db92c67ef2"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewport.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10791) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d60a6234b11bd96"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewport.WorkPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10795) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35732f049c0494e6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewport.WorkSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10797) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8768caf5158462ca"></a>[`public float Inno.Native.ImGuizmo.ImGuiViewportP.Alpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10855) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b01b7b266d570545"></a>[`public Inno.Native.ImGuizmo.NativeArray_1eccc798bd000925 Inno.Native.ImGuizmo.ImGuiViewportP.BgFgDrawLists`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a32490bc12053226"></a>[`public Inno.Native.ImGuizmo.NativeArray_b3004bc9c15942c6 Inno.Native.ImGuizmo.ImGuiViewportP.BgFgDrawListsLastFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10863) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcd288846ada8e98"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.BuildWorkInsetMax`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10883) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-355f1255afce9bbe"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.BuildWorkInsetMin`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10881) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c687b1bbe8b1c76"></a>[`public Inno.Native.ImGuizmo.ImDrawDataBuilder Inno.Native.ImGuizmo.ImGuiViewportP.DrawDataBuilder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10869) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdb27b0bfbfa21e2"></a>[`public Inno.Native.ImGuizmo.ImDrawData Inno.Native.ImGuizmo.ImGuiViewportP.DrawDataP`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10867) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01a787b586f0a448"></a>[`public int Inno.Native.ImGuizmo.ImGuiViewportP.Idx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f0895f88c7ce656"></a>[`public Inno.Native.ImGuizmo.ImGuiViewport Inno.Native.ImGuizmo.ImGuiViewportP.ImGuiViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10839) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-455dd6f18202dbaf"></a>[`public float Inno.Native.ImGuizmo.ImGuiViewportP.LastAlpha`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10857) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d732146a0a2698e1"></a>[`public byte Inno.Native.ImGuizmo.ImGuiViewportP.LastFocusedHadNavWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10859) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efef58a1e959c814"></a>[`public int Inno.Native.ImGuizmo.ImGuiViewportP.LastFocusedStampCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-47a4a43bf5c202fe"></a>[`public int Inno.Native.ImGuizmo.ImGuiViewportP.LastFrameActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-840ea2977ea06ce7"></a>[`public uint Inno.Native.ImGuizmo.ImGuiViewportP.LastNameHash`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10849) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00194ec7c7593441"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.LastPlatformPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10871) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc851c58ee1b8a68"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.LastPlatformSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10873) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-263a21992ca1c06c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.LastPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10851) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d41fe76d36d75d6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.LastRendererSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10875) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-877f1bf975420a1d"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.LastSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10853) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-323167f9683e315e"></a>[`public short Inno.Native.ImGuizmo.ImGuiViewportP.PlatformMonitor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10861) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c12af35c11d623c7"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiViewportP.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10841) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b4f3bf72707ad1f"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.WorkInsetMax`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10879) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7cde5fe410718f5b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiViewportP.WorkInsetMin`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10877) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dea37a60e1582234"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.Active`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28c6ac1fd34f3bee"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.Appearing`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11005) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2160f5f3d96ab491"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.AutoFitFramesX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7cf53375c3ef9080"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.AutoFitFramesY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11033) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4d458458bc6b007"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.AutoFitOnlyGrows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-66f3f8ec80659e91"></a>[`public Inno.Native.ImGuizmo.ImGuiDir Inno.Native.ImGuizmo.ImGuiWindow.AutoPosLastDirection`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11029) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-752891702de468ae"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.BeginCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11019) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cade6770eeaf47ef"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.BeginCountPreviousFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11021) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ab959c99a798fb9d"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.BeginOrderWithinContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11025) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7406024df2579f0f"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.BeginOrderWithinParent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbc4cdc0cbb039fa"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.ChildFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6fd4976530472a54"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.ChildId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10965) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-999e6596d4f08d16"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.ClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11063) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccd043acc21a2b48"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.Collapsed`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10997) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea307ccba1002bc1"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiOldColumns Inno.Native.ImGuizmo.ImGuiWindow.ColumnsStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5189db047aa5b3e7"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.ContentRegionRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11065) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-606f4ae780b56f61"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ContentSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10931) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-45f34ff6cbed2b00"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ContentSizeExplicit`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b68f0a3b34396b61"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ContentSizeIdeal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10933) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3db0a8b4d1c294cc"></a>[`public Inno.Native.ImGuizmo.ImGuiContext* Inno.Native.ImGuizmo.ImGuiWindow.Ctx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8178024b890210d"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowTempData Inno.Native.ImGuizmo.ImGuiWindow.Dc`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11051) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0e72469c077272a"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoInnerSizeX1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10955) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fb772942c25a60d1"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoInnerSizeY1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c8f8339d4bdccf2"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeX1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10947) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9b200d153b1208d"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeX2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10951) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-017d1b8e44574f5e"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeY1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10949) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e55c5d906e286cda"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.DecoOuterSizeY2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd769c210fdd5c99"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.DisableInputsFrames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11043) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51a3ea392b8ed0ce"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.DockId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-12a5879a3c0cae45"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiWindow.DockNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11129) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d61eabc4698c0144"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.ImGuiWindow.DockNodeAsHost`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff7241ad6efccdbb"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.DockOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11125) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83f3a25907a068d2"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowDockStyle Inno.Native.ImGuizmo.ImGuiWindow.DockStyle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11127) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e6dc3fa141556c0"></a>[`public Inno.Native.ImGuizmo.ImDrawList* Inno.Native.ImGuizmo.ImGuiWindow.DrawList`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b7d9cf91ee8cee03"></a>[`public Inno.Native.ImGuizmo.ImDrawList Inno.Native.ImGuizmo.ImGuiWindow.DrawListInst`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e363f9b5817d3b7"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.Flags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10909) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5528674025d4c8d5"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.FlagsPreviousFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10911) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c5f786cbeb2bad3"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindow.FocusOrder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-802d52feb4866af2"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.FontRefSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11085) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a3cabf47d782f31"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.FontWindowScale`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b79e5ef8246922b"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.FontWindowScaleParents`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11083) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d75587ec1576297a"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.HasCloseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-412180ac6fa8dd93"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.Hidden`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11007) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd163236af6ec7d0"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesCanSkipItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11037) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67fdfd59d5c0d822"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesCannotSkipItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11039) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3865df83b98fe94"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.HiddenFramesForRenderOnly`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11041) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5cf371c3412eef8"></a>[`public Inno.Native.ImGuizmo.ImVec2ih Inno.Native.ImGuizmo.ImGuiWindow.HitTestHoleOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11069) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ebaf35f64d41afac"></a>[`public Inno.Native.ImGuizmo.ImVec2ih Inno.Native.ImGuizmo.ImGuiWindow.HitTestHoleSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-04ec31e3a5ea999d"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.Id`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10907) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b138a5228506584"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiID Inno.Native.ImGuizmo.ImGuiWindow.IdStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11049) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37e0a7b56d34e002"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.InnerClipRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60f828d98ff0245b"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.InnerRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-625f8f2c1d874795"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.IsExplicitChild`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f55fd691c7a2df3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.IsFallbackWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11009) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eacb2a1d5576d3b9"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.LastFrameActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11071) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cff845d2acdbf2e3"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.LastFrameJustFocused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11073) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afe84818c87e7080"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.LastTimeActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-449f65aee6788e85"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.MemoryCompacted`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11123) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a5cb596193abf163"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.MemoryDrawListIdxCapacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11119) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-daaa173df71dc1c5"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.MemoryDrawListVtxCapacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d2ec182d8f81636"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.MenuBarHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10945) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2700a44dc2bc9be7"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.MoveId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10961) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc833fe39b964cc4"></a>[`public sbyte* Inno.Native.ImGuizmo.ImGuiWindow.Name`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85368b9f5402d2fc"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.NameBufLen`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10959) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0734aaf5be90693c"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.NavLastChildNavWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11109) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3bf5a6e9b171a306"></a>[`public Inno.Native.ImGuizmo.NativeArray_16a683b69e90f4cd Inno.Native.ImGuizmo.ImGuiWindow.NavLastIds`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11111) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d794081abe603d66"></a>[`public Inno.Native.ImGuizmo.NativeArray_b726fff0e8801db7 Inno.Native.ImGuizmo.ImGuiWindow.NavPreferredScoringPosRel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11115) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24be440c0af5e614"></a>[`public Inno.Native.ImGuizmo.NativeArray_87beacaa77c69f9d Inno.Native.ImGuizmo.ImGuiWindow.NavRectRel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11113) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-38c8ee688c30d5d8"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.NavRootFocusScopeId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11117) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e387456df7e23b2"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.OuterRectClipped`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11053) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8125efcdb523e1cb"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.ParentWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11093) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-051666ffb4d002d2"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.ParentWindowForFocusRoute`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11107) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ff3ee215ef52b61"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.ParentWindowInBeginStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11095) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42a5b93fdfea9f5b"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.ParentWorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11061) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8464bed28492dfb8"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.PopupId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10967) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-674aebbeb97221ad"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.Pos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10925) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca56037defcaaa17"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.ResizeBorderHeld`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11017) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e782454cb8184dc"></a>[`public sbyte Inno.Native.ImGuizmo.ImGuiWindow.ResizeBorderHovered`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11015) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec3a8d68040dba83"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.RootWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11097) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-981c1ecff25e7325"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.RootWindowDockTree`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11101) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-40e60bfb0184f827"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.RootWindowForNav`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11105) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-594fcc18c185fecf"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.RootWindowForTitleBarHighlight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11103) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-337c9f65c1839b13"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindow.RootWindowPopupTree`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fde1d378c3fdc55e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.Scroll`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10969) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abc31765e0f40bb6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ScrollMax`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10971) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e2fc269d027530d"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ScrollTarget`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10973) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c4125b3ed4fd1bc"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ScrollTargetCenterRatio`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10975) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd7462cf6b443e0b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ScrollTargetEdgeSnapDist`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e849ae4d641a448a"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarSizes`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c7ca02c291cf640"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c2f57ea926d509d"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarXStabilizeEnabled`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-939ed7048ab32754"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarXStabilizeToggledHistory`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10987) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8bcdecf938a9663"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.ScrollbarY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59961b84590f32d5"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosPivot`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11047) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2023298e4ca186c"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosVal`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11045) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d593ec2c3518078"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.SettingsOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11087) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ada870fda8d2c8a8"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10927) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dab4a2e5765100e3"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.SizeFull`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10929) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ce04e70802bd718"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.SkipItems`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11001) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9243368a536cd107"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.SkipRefresh`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11003) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2be851fcf42a603a"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImGuiWindow.StateStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b4060762524c3344"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.TabId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10963) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5e786fa638b702e"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.TitleBarHeight`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10943) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-344636082f99baa6"></a>[`public Inno.Native.ImGuizmo.ImGuiViewportP* Inno.Native.ImGuizmo.ImGuiWindow.Viewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10917) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f82d5f18b33dfc17"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.ViewportAllowPlatformMonitorExtend`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10923) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-19db6e383afa7a6a"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindow.ViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10919) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6cd630cc4eac59c3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.ViewportOwned`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10989) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60903860a5d9609e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.ViewportPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10921) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b365d0b7843bbbe3"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.WantCollapseToggle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10999) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca0e02661ff6efa4"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.WasActive`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10993) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f73102f358e34a2"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.WindowBorderSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10941) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1375bc527dcb798b"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowClass Inno.Native.ImGuizmo.ImGuiWindow.WindowClass`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10915) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1b29b57c75dc8200"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindow.WindowPadding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10937) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a47c38c5eb66b2e1"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindow.WindowRounding`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10939) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e88bed25d9db8f7"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindow.WorkRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11059) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e32a76c53e9a9e34"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindow.WriteAccessed`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10995) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c3c0596b1f5eb83"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowClass.ClassId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11431) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41cf6815a306a511"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowClass.DockNodeFlagsOverrideSet`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-510cb32192b86083"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowClass.DockingAllowUnclassed`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cab54acf135671f8"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowClass.DockingAlwaysTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2bbce1f1ddae817"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowClass.FocusRouteParentWindowId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c01f8a23e1bca65"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowClass.ParentViewportId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11433) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ef848a1bd35c768"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowClass.TabItemFlagsOverrideSet`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39f838626c207099"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowClass.ViewportFlagsOverrideClear`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d02a413ab0739fcb"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowClass.ViewportFlagsOverrideSet`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76e512f1cd3234ac"></a>[`public Inno.Native.ImGuizmo.NativeArray_fcecdbb15210e44b Inno.Native.ImGuizmo.ImGuiWindowDockStyle.Colors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5f8643eb11c30cf"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowStackData.DisabledOverrideReenable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6193839ab82dd294"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11491) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27f0b4ef9ba215bd"></a>[`public Inno.Native.ImGuizmo.ImGuiLastItemData Inno.Native.ImGuizmo.ImGuiWindowStackData.ParentLastItemDataBackup`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11485) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4386d19a2842afbf"></a>[`public Inno.Native.ImGuizmo.ImGuiErrorRecoveryState Inno.Native.ImGuizmo.ImGuiWindowStackData.StackSizesInBegin`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-34413497a7a39704"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow* Inno.Native.ImGuizmo.ImGuiWindowStackData.Window`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11483) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2db5c6e34d27b978"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.ChildItemStatusFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11579) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f65cf086bb7228d"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr Inno.Native.ImGuizmo.ImGuiWindowTempData.ChildWindows`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7a4f16af801f437"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiWindowTempData.ColumnsOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1796bff91e66d731"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrLineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86ed6f6972294ec7"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrLineTextBaseOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c77d672ca2ae4ce"></a>[`public Inno.Native.ImGuizmo.ImGuiOldColumns* Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrentColumns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65c24ce6ffa0a899"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.CurrentTableIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-775eca0d328ca92a"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ed7cdd70f760660"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-413cb940b03a6430"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorPosPrevLine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53b151643d9391b3"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorStartPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d9e4d4c7c4efe2e"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.CursorStartPosLossyness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-345f8131a79e29e1"></a>[`public Inno.Native.ImGuizmo.ImRectC Inno.Native.ImGuizmo.ImGuiWindowTempData.DockTabItemRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e471b3813b2393d"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.DockTabItemStatusFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11581) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6e58794d998eeb2"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiWindowTempData.GroupOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11535) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a8a343d9c1a3ddc"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.IdealMaxPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0e96f3e59abbff6"></a>[`public Inno.Native.ImGuizmo.ImVec1 Inno.Native.ImGuizmo.ImGuiWindowTempData.Indent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11531) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78af84a3751e927b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.IsSameLine`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e0e8110ae823b7e"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.IsSetPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-293534773169136c"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11585) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4b7deee8101a77d"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidthDefault`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11587) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d47dd92d196498d4"></a>[`public Inno.Native.ImGuizmo.ImVectorFloat Inno.Native.ImGuizmo.ImGuiWindowTempData.ItemWidthStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11591) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-52d8305f7fdd0aed"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.LayoutType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c05ff023c606b36b"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuBarAppending`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0ecb84a90937a8b"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuBarOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11553) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8df0192c8e794c80"></a>[`public Inno.Native.ImGuizmo.ImGuiMenuColumns Inno.Native.ImGuizmo.ImGuiWindowTempData.MenuColumns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11555) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-722126e69e20f6f6"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowTempData.ModalDimBgColor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11575) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39936046c56695ba"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.NavHideHighlightOneFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-108ac0a1e713f4df"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.NavIsScrollPushableX`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11545) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8dcd96dacc704bc8"></a>[`public Inno.Native.ImGuizmo.ImGuiNavLayer Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayerCurrent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bb279fb2ede97c0"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayersActiveMask`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11541) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e3f62f3914fe7b1"></a>[`public short Inno.Native.ImGuizmo.ImGuiWindowTempData.NavLayersActiveMaskNext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11543) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb7135df43bbd0d1"></a>[`public byte Inno.Native.ImGuizmo.ImGuiWindowTempData.NavWindowHasScrollY`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11549) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d694da04a97e79a7"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.ParentLayoutType`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1341e71b8f29f0a6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImGuiWindowTempData.PrevLineSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27b4d5e1383d1741"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowTempData.PrevLineTextBaseOffset`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-272b26d79b6d3ed6"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage* Inno.Native.ImGuizmo.ImGuiWindowTempData.StateStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a1a0bc7b94b6b39"></a>[`public float Inno.Native.ImGuizmo.ImGuiWindowTempData.TextWrapPos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11589) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8d6579cbc28f5d41"></a>[`public Inno.Native.ImGuizmo.ImVectorFloat Inno.Native.ImGuizmo.ImGuiWindowTempData.TextWrapPosStack`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11593) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbf9596ea7ecadc4"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeDepth`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11557) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e2fcea4b8f047cff"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeHasStackDataDepthMask`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11559) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22ae28bc0ecdc289"></a>[`public uint Inno.Native.ImGuizmo.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6bc11b1b8cfc6afa"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindowTempData.WindowItemStatusFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11577) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2bd91dc1184534b"></a>[`public const string Inno.Native.ImGuizmo.ImGuizmo.NativeLibraryName = "cimguizmo"`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L18) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`cimguizmo` |
| <a id="api-4bdbb4e97292f174"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Count = 15`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11623) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`15` |
| <a id="api-11930f021248778c"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionX = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-90658d9db52f6b52"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionY = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-24555adb8be87afa"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.DirectionZ = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-169d30a361a0408f"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.HatchedAxisLines = 12`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11620) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12` |
| <a id="api-58ee63d7f72871e6"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Inactive = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11615) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-454141c8efa68f07"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneX = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11611) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-5437fa9441132719"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneY = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-6b9c5b0049fab31a"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.PlaneZ = 5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11613) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-85e4b0654a06bc44"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingBorder = 10`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`10` |
| <a id="api-656b3593fc58b9f4"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.RotationUsingFill = 11`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`11` |
| <a id="api-b53e5b2c885fbfb1"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.ScaleLine = 9`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11617) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`9` |
| <a id="api-360c19368c31ac85"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Selection = 6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-0d27bde1532afd2c"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.Text = 13`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`13` |
| <a id="api-c4abc66f41f9c275"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.TextShadow = 14`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`14` |
| <a id="api-adc7f7941421fa85"></a>[`Inno.Native.ImGuizmo.ImGuizmoColor.TranslationLine = 8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-506abf541987ffe1"></a>[`public const string Inno.Native.ImGuizmo.ImGuizmoContext.NativeLibraryName = "cimguizmo"`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L63) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`cimguizmo` |
| <a id="api-d58148eb0cd5eef8"></a>[`Inno.Native.ImGuizmo.ImGuizmoMode.Local = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11631) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5d860e611346635e"></a>[`Inno.Native.ImGuizmo.ImGuizmoMode.World = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-9dcbeb936afc06a5"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Bounds = 1024`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11650) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-6fea563686cc5345"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Rotate = 120`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11655) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`120` |
| <a id="api-0a0145e3a5fdfceb"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateScreen = 64`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-ce26d54c18fd8693"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateX = 8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-486ad0400c55fcf3"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateY = 16`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-f1b4ee15048cc595"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.RotateZ = 32`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11645) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-441e053e384d387f"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Scale = 896`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`896` |
| <a id="api-5dc424efea31ff2a"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleX = 128`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11647) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-b07fac0f79872f56"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleXu = 2048`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11651) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-46a076509af2c622"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleY = 256`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-a358dbf06a215778"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleYu = 4096`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11652) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-58cbb40ba85b1ff0"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZ = 512`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11649) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-af2a21975af86da2"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.ScaleZu = 8192`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11653) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-d3dbc8381a811efc"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Scaleu = 14336`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11657) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`14336` |
| <a id="api-98d814199c4c7d29"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Translate = 7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-43900ba55866a636"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateX = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11640) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-4d3e9ef0b284d7c4"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateY = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-34863e4810a9b516"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.TranslateZ = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11642) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-4e736d1ad5e76713"></a>[`Inno.Native.ImGuizmo.ImGuizmoOperation.Universal = 14463`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11658) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`14463` |
| <a id="api-023b0390dd148ccc"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.AliveCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11675) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7175dbc764f73156"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11669) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-355c931f728bab96"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.FreeIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11673) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8743785558b5466d"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState.Map`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11671) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4edc4742188e7ecb"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiTabBar.AliveCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11699) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ccd63248f209a95"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTabBar Inno.Native.ImGuizmo.ImPoolImGuiTabBar.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78bd531dea2f949c"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiTabBar.FreeIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11697) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e5dbf190a0cabc8"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImPoolImGuiTabBar.Map`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11695) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2cfdf4e347f4ee1c"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiTable.AliveCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11723) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce23e827ef91304c"></a>[`public Inno.Native.ImGuizmo.ImVectorImGuiTable Inno.Native.ImGuizmo.ImPoolImGuiTable.Buf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11717) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3743b62d3f9f6ddb"></a>[`public int Inno.Native.ImGuizmo.ImPoolImGuiTable.FreeIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11721) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b55513b1be1f777b"></a>[`public Inno.Native.ImGuizmo.ImGuiStorage Inno.Native.ImGuizmo.ImPoolImGuiTable.Map`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11719) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eeb066800097adae"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImRectC.Max`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11743) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02465f9d6babcbf6"></a>[`public Inno.Native.ImGuizmo.ImVec2C Inno.Native.ImGuizmo.ImRectC.Min`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11741) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a61c3c79d0b2eae8"></a>[`public Inno.Native.ImGuizmo.ImGuiTableCellData* Inno.Native.ImGuizmo.ImSpanImGuiTableCellData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11761) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e7626b99a347226"></a>[`public Inno.Native.ImGuizmo.ImGuiTableCellData* Inno.Native.ImGuizmo.ImSpanImGuiTableCellData.DataEnd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11763) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b997d829be04dfe"></a>[`public Inno.Native.ImGuizmo.ImGuiTableColumn* Inno.Native.ImGuizmo.ImSpanImGuiTableColumn.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11781) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ec200098bb29dd8"></a>[`public Inno.Native.ImGuizmo.ImGuiTableColumn* Inno.Native.ImGuizmo.ImSpanImGuiTableColumn.DataEnd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11783) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f515e32edb0b4247"></a>[`public short* Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ab13c24c0d14557"></a>[`public short* Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx.DataEnd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c52370074b3e9509"></a>[`public Inno.Native.ImGuizmo.ImVectorImFontBakedPtr Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Blocks`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11825) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3fcf9279e9c34d52"></a>[`public int Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11823) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e34d22d5ff227ea1"></a>[`public int Inno.Native.ImGuizmo.ImStableVectorImFontBaked32.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-719d455aeabf15e1"></a>[`public void* Inno.Native.ImGuizmo.ImTextureData.BackendUserData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6298b84c1c079a26"></a>[`public int Inno.Native.ImGuizmo.ImTextureData.BytesPerPixel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11857) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54fc0b17fb8de49e"></a>[`public Inno.Native.ImGuizmo.ImTextureFormat Inno.Native.ImGuizmo.ImTextureData.Format`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11851) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6382fbe3e30892ce"></a>[`public int Inno.Native.ImGuizmo.ImTextureData.Height`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11855) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb7762157169801b"></a>[`public byte* Inno.Native.ImGuizmo.ImTextureData.Pixels`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11859) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8aa57e2da82223e"></a>[`public ushort Inno.Native.ImGuizmo.ImTextureData.RefCount`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11869) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-26696db5c48cf24d"></a>[`public Inno.Native.ImGuizmo.ImTextureStatus Inno.Native.ImGuizmo.ImTextureData.Status`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b4867b93a731f303"></a>[`public ulong Inno.Native.ImGuizmo.ImTextureData.TexId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11849) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f3d76249c183584"></a>[`public int Inno.Native.ImGuizmo.ImTextureData.UniqueId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9101f6898a11a69"></a>[`public int Inno.Native.ImGuizmo.ImTextureData.UnusedFrames`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11867) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67c385e4b64db676"></a>[`public Inno.Native.ImGuizmo.ImTextureRect Inno.Native.ImGuizmo.ImTextureData.UpdateRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11863) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c21c90545ff21eb"></a>[`public Inno.Native.ImGuizmo.ImVectorImTextureRect Inno.Native.ImGuizmo.ImTextureData.Updates`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75e66cc3b47f1da5"></a>[`public byte Inno.Native.ImGuizmo.ImTextureData.UseColors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11871) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65e4a7d43412cbbf"></a>[`public Inno.Native.ImGuizmo.ImTextureRect Inno.Native.ImGuizmo.ImTextureData.UsedRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11861) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-319c5b07eee2d213"></a>[`public byte Inno.Native.ImGuizmo.ImTextureData.WantDestroyNextFrame`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11873) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c74455a4f133313"></a>[`public int Inno.Native.ImGuizmo.ImTextureData.Width`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11853) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e7d4301ec76ba01"></a>[`Inno.Native.ImGuizmo.ImTextureFormat.ImTextureFormatAlpha8 = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11889) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d3a4f151b786df54"></a>[`Inno.Native.ImGuizmo.ImTextureFormat.ImTextureFormatRgba32 = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11888) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-b3a8e93ba2ae38bf"></a>[`public ushort Inno.Native.ImGuizmo.ImTextureRect.H`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11906) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72a49f9c16d72f12"></a>[`public ushort Inno.Native.ImGuizmo.ImTextureRect.W`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11904) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69ade065cb6331a3"></a>[`public ushort Inno.Native.ImGuizmo.ImTextureRect.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11900) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-164bed78c61825cc"></a>[`public ushort Inno.Native.ImGuizmo.ImTextureRect.Y`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1fb7ad4dba175a5"></a>[`public Inno.Native.ImGuizmo.ImTextureData* Inno.Native.ImGuizmo.ImTextureRefC.TexData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11924) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08e96959bbd37f5c"></a>[`public ulong Inno.Native.ImGuizmo.ImTextureRefC.TexId`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11926) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-63b88fe072fd6c3e"></a>[`Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusDestroyed = 1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-e6cf38ee50487417"></a>[`Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusOk = 0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11941) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-9c68b82cd4c32e7a"></a>[`Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantCreate = 2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11943) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-76e5b890cbb61a00"></a>[`Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantDestroy = 4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11945) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-07dc46b6beaf2297"></a>[`Inno.Native.ImGuizmo.ImTextureStatus.ImTextureStatusWantUpdates = 3`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-8e97dc3134c87118"></a>[`public float Inno.Native.ImGuizmo.ImVec1.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24833dcfe5aa2b24"></a>[`public float Inno.Native.ImGuizmo.ImVec2C.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f4c04545f721794"></a>[`public float Inno.Native.ImGuizmo.ImVec2C.Y`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e927823e8b0f382"></a>[`public int Inno.Native.ImGuizmo.ImVec2iC.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11994) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3963d6b8ff60be41"></a>[`public int Inno.Native.ImGuizmo.ImVec2iC.Y`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11996) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ed24bb7554ca307"></a>[`public short Inno.Native.ImGuizmo.ImVec2ih.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec33fa081d1ff460"></a>[`public short Inno.Native.ImGuizmo.ImVec2ih.Y`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12016) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-157da4c5f894f504"></a>[`public float Inno.Native.ImGuizmo.ImVec4C.W`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c396c2be816a2c06"></a>[`public float Inno.Native.ImGuizmo.ImVec4C.X`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12034) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea5897dc003fee2f"></a>[`public float Inno.Native.ImGuizmo.ImVec4C.Y`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12036) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a557faafba0f8a08"></a>[`public float Inno.Native.ImGuizmo.ImVec4C.Z`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17d035fe37ac8858"></a>[`public int Inno.Native.ImGuizmo.ImVectorChar.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12060) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6db076c0a372a3b6"></a>[`public sbyte* Inno.Native.ImGuizmo.ImVectorChar.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12062) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7f0e8a27fae115bb"></a>[`public int Inno.Native.ImGuizmo.ImVectorChar.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12058) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-002e56966528ebba"></a>[`public int Inno.Native.ImGuizmo.ImVectorFloat.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12082) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71768f7ad82de991"></a>[`public float* Inno.Native.ImGuizmo.ImVectorFloat.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12084) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ffe1d739f8d06a49"></a>[`public int Inno.Native.ImGuizmo.ImVectorFloat.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12080) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15ecbbb2489e6858"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawChannel.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12104) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f48b78be32dd6b93"></a>[`public Inno.Native.ImGuizmo.ImDrawChannel* Inno.Native.ImGuizmo.ImVectorImDrawChannel.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12106) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c90bf1551b23374"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawChannel.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12102) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a72ef3bab145f0c8"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawCmd.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12126) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ac562b045ced6aa"></a>[`public Inno.Native.ImGuizmo.ImDrawCmd* Inno.Native.ImGuizmo.ImVectorImDrawCmd.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12128) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48f13f4a2c31bb11"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawCmd.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12124) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f859ac62b323705"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawIdx.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12148) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5b97c7ab4657085"></a>[`public ushort* Inno.Native.ImGuizmo.ImVectorImDrawIdx.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12150) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce10bef079506eab"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawIdx.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12146) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32ddb0a5642240a0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12170) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c232f56a8c3836ff"></a>[`public Inno.Native.ImGuizmo.ImDrawList** Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12172) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-74e93e77c97ab6b2"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawListPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6400eba7b05c3542"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12192) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd11954320179f74"></a>[`public Inno.Native.ImGuizmo.ImDrawListSharedData** Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12194) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8077f6a5736c9551"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12190) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af25c186c110c90b"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawVert.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12214) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea809f5e2cb87997"></a>[`public Inno.Native.ImGuizmo.ImDrawVert* Inno.Native.ImGuizmo.ImVectorImDrawVert.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12216) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f797916fbf1c375f"></a>[`public int Inno.Native.ImGuizmo.ImVectorImDrawVert.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9a549271b001759"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12236) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f34ad9230ac812a"></a>[`public Inno.Native.ImGuizmo.ImFontAtlas** Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-718f414300ba1e8a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12234) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00040dc4115e3e28"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c49ad833a145c55b"></a>[`public Inno.Native.ImGuizmo.ImFontAtlasRectEntry* Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbb422d6e73678da"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4996f66e874545b"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdaca1b2fb9788ff"></a>[`public Inno.Native.ImGuizmo.ImFontBaked** Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f65e3b09ccf0df0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontBakedPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12278) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fd2f8bc2a3731ac"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontConfig.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83471aaca50ed44e"></a>[`public Inno.Native.ImGuizmo.ImFontConfig* Inno.Native.ImGuizmo.ImVectorImFontConfig.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12304) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2cbefae5071d0f77"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontConfig.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-984e922ba332bf6a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12324) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af5d9ac52c7b07e6"></a>[`public Inno.Native.ImGuizmo.ImFontConfig** Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fa2dd82ee6b6b95"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontConfigPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e036d0fda132a9d2"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontGlyph.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12346) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a05e3d894f1e1ee"></a>[`public Inno.Native.ImGuizmo.ImFontGlyph* Inno.Native.ImGuizmo.ImVectorImFontGlyph.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e7a6600733b2ef4"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontGlyph.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12344) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b845d11d086acf1d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1492dd5bd28b4fab"></a>[`public Inno.Native.ImGuizmo.ImFont** Inno.Native.ImGuizmo.ImVectorImFontPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3d1f823867913d3"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ba79406b6cedd0f"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontStackData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-09b1872e829deaea"></a>[`public Inno.Native.ImGuizmo.ImFontStackData* Inno.Native.ImGuizmo.ImVectorImFontStackData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51c4fd703c3e86cb"></a>[`public int Inno.Native.ImGuizmo.ImVectorImFontStackData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12388) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-256207c69d1abf37"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d035ad3ae375158"></a>[`public Inno.Native.ImGuizmo.ImGuiColorMod* Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff1edca64df53123"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiColorMod.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4c401316f73f9bc"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14d8ca8b42ced255"></a>[`public Inno.Native.ImGuizmo.ImGuiContextHook* Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1d6f823924c612f"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiContextHook.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80671ea91c781e02"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1d61737e9f26e4d"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNodeSettings* Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-94b99e0ab8a7d423"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c112b51195df7ed7"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9e7bb67c044f1ed"></a>[`public Inno.Native.ImGuizmo.ImGuiDockRequest* Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cf3566cec685ece3"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiDockRequest.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f6cafa2e7c0bbae7"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6caa8b24af672b66"></a>[`public Inno.Native.ImGuizmo.ImGuiFocusScopeData* Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4723f2283c166133"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5032c35e8cc3600"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12522) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-161d563e1fc24f35"></a>[`public Inno.Native.ImGuizmo.ImGuiGroupData* Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec87d640f8c738df"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiGroupData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7eb06aeb83d3f4f5"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiID.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5a8cbab7c3ab6b0a"></a>[`public uint* Inno.Native.ImGuizmo.ImVectorImGuiID.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a052c9802e74621"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiID.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8b508717b397611"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70cb4d37e64bd4c7"></a>[`public Inno.Native.ImGuizmo.ImGuiInputEvent* Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70eaa7231819d207"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiInputEvent.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1160349d0072155e"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12588) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f591bb62297fc535"></a>[`public int* Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b47afe365a12951"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiItemFlags.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5d503981ec65342"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3dfb0035eb6956cf"></a>[`public Inno.Native.ImGuizmo.ImGuiKeyRoutingData* Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-23f6e3edbc093836"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92c5907ecbafb2fc"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df935667643f44c3"></a>[`public Inno.Native.ImGuizmo.ImGuiListClipperData* Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efc2621996617c68"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiListClipperData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12630) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-015c899508641ec8"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fe15285c1ed03db"></a>[`public Inno.Native.ImGuizmo.ImGuiListClipperRange* Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf320a9a22040dcf"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12652) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-944fa7f35198c0ec"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-587b11ca8810a792"></a>[`public Inno.Native.ImGuizmo.ImGuiMultiSelectState* Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12678) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b0ffcd25814ca4d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12674) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc04aca4a4b6846d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12698) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b404cb57d211a41"></a>[`public Inno.Native.ImGuizmo.ImGuiMultiSelectTempData* Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12700) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ceae093675090f0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f0cb7525ae14d27"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12720) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d94e693d1e764ee"></a>[`public Inno.Native.ImGuizmo.ImGuiOldColumnData* Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12722) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcc68393d6df23b3"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12718) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9dcb30b6b71ca42d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12742) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a74848294a7c8a0"></a>[`public Inno.Native.ImGuizmo.ImGuiOldColumns* Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dec35fb9f06d4020"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiOldColumns.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e302097299996e1"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f3fb391257e45df1"></a>[`public Inno.Native.ImGuizmo.ImGuiPlatformMonitor* Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1cc92ed4c432316c"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-830f78f0ab87c475"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12786) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dbe3cf6c94164cd"></a>[`public Inno.Native.ImGuizmo.ImGuiPopupData* Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12788) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3159bea8a21c95c0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPopupData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12784) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c51d14d7faf75a5"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12808) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8025f80c4d9cf69"></a>[`public Inno.Native.ImGuizmo.ImGuiPtrOrIndex* Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12810) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5d2f87092e1c1c0c"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12806) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d06c83bf0318a62a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18bc35db59361152"></a>[`public Inno.Native.ImGuizmo.ImGuiSelectionRequest* Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12832) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d65d4a7024c00029"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12828) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-468d840eba4a7e92"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12852) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-276bbd64f7118f1f"></a>[`public Inno.Native.ImGuizmo.ImGuiSettingsHandler* Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12854) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e168d1463e90bf6"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12850) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3561b639c99d7c3c"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b6532de61609cb6"></a>[`public Inno.Native.ImGuizmo.ImGuiShrinkWidthItem* Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12876) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbfbe59a39f15a73"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12872) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fde43103886cb6d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12896) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-895aa4ee8643ea92"></a>[`public Inno.Native.ImGuizmo.ImGuiStackLevelInfo* Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12898) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3063474654c1707c"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12894) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-570266cbccdd3975"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdbe960212304863"></a>[`public Inno.Native.ImGuizmo.ImGuiStoragePair* Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12920) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-243d8c12f05b9750"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStoragePair.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12916) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d077766daaae8fc"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12940) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3bfde0f02c3af40"></a>[`public Inno.Native.ImGuizmo.ImGuiStyleMod* Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03470a5dace36bd1"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiStyleMod.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12938) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-988e7bc5d9c43e4a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12962) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-be96a18c47bf9c0a"></a>[`public Inno.Native.ImGuizmo.ImGuiTabBar* Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12964) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42e5a44e10f07e3a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTabBar.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a87fbf2f34fe7cfc"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12984) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0099ba9ada1f3841"></a>[`public Inno.Native.ImGuizmo.ImGuiTabItem* Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12986) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c0a8fb9a11d7937"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTabItem.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac39b9d7fcd31c89"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTable.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13006) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-45d7e2ae4946012b"></a>[`public Inno.Native.ImGuizmo.ImGuiTable* Inno.Native.ImGuizmo.ImVectorImGuiTable.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13008) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7cc8fbed3752b617"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTable.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13004) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02aaa49529a7ca38"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13028) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ecc8f2df30716f0"></a>[`public Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs* Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13030) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fec8bca4fa144c9a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13026) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe75a1ddc2ec4359"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13050) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ded635ffbebb412"></a>[`public Inno.Native.ImGuizmo.ImGuiTableHeaderData* Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13052) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-125828592fd7d3b6"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13048) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15dc65c294110266"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13072) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd6f1f17ba7ad9bf"></a>[`public Inno.Native.ImGuizmo.ImGuiTableInstanceData* Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13074) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75258e24f0074f70"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13070) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27933d201b8be88d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-de3ebfd66027631a"></a>[`public Inno.Native.ImGuizmo.ImGuiTableTempData* Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13096) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bda843a675ac8dac"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTableTempData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13092) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-665f1dc79095f209"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13116) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8429ebd3885cd966"></a>[`public Inno.Native.ImGuizmo.ImGuiTreeNodeStackData* Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13118) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a62de2536f0513a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13114) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87b89f33dd63197c"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13138) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-320ac5843231ab99"></a>[`public Inno.Native.ImGuizmo.ImGuiViewportP** Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13140) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-840cdbf6c387aa19"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13136) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ea22a8d388f5195"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13160) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a4395d05d5d5011"></a>[`public Inno.Native.ImGuizmo.ImGuiViewport** Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9128c1fdd10d81cf"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13158) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7e3702b086652b6"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13182) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a06c0d5cc6413d2"></a>[`public Inno.Native.ImGuizmo.ImGuiWindow** Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13184) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06a68a18a1a795ef"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13180) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3241a4162bf81b3"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f05fb9b6cfe636da"></a>[`public Inno.Native.ImGuizmo.ImGuiWindowStackData* Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13206) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-773952ddbf855b91"></a>[`public int Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13202) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10b63d9c40e27b90"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13226) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-375ef2844a1f5ac4"></a>[`public Inno.Native.ImGuizmo.ImTextureData** Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13228) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9081b376a8d4b5b2"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureDataPtr.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29752733bc9cc20d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureRect.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-744685aba91a543c"></a>[`public Inno.Native.ImGuizmo.ImTextureRect* Inno.Native.ImGuizmo.ImVectorImTextureRect.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-96b2beda3958d453"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureRect.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cadc917f4319c652"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureRef.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a008f2c704e428b0"></a>[`public Inno.Native.ImGuizmo.ImTextureRefC* Inno.Native.ImGuizmo.ImVectorImTextureRef.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13272) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a86b775f4cce3b76"></a>[`public int Inno.Native.ImGuizmo.ImVectorImTextureRef.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4abd214dbcc5002"></a>[`public int Inno.Native.ImGuizmo.ImVectorImU16.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4cae4b5900aa5c81"></a>[`public ushort* Inno.Native.ImGuizmo.ImVectorImU16.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-44781739895e63ff"></a>[`public int Inno.Native.ImGuizmo.ImVectorImU16.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdaa581b85460cb0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImU8.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fd8e1d1a2e6bacfd"></a>[`public byte* Inno.Native.ImGuizmo.ImVectorImU8.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13316) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae436ce2ecf245a1"></a>[`public int Inno.Native.ImGuizmo.ImVectorImU8.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65456dceae302feb"></a>[`public int Inno.Native.ImGuizmo.ImVectorImVec2.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13336) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-034169cde0197551"></a>[`public Inno.Native.ImGuizmo.ImVec2C* Inno.Native.ImGuizmo.ImVectorImVec2.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13338) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2f2ecb5d41791d7"></a>[`public int Inno.Native.ImGuizmo.ImVectorImVec2.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-62272b8ac39f446e"></a>[`public int Inno.Native.ImGuizmo.ImVectorImVec4.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2539eb9ffec47eb6"></a>[`public Inno.Native.ImGuizmo.ImVec4C* Inno.Native.ImGuizmo.ImVectorImVec4.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ffb204bc0ecc5b0"></a>[`public int Inno.Native.ImGuizmo.ImVectorImVec4.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8186a1e5f5593e6a"></a>[`public int Inno.Native.ImGuizmo.ImVectorImWchar.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0b111d1746d7b6f"></a>[`public ushort* Inno.Native.ImGuizmo.ImVectorImWchar.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-149fb0c64267673d"></a>[`public int Inno.Native.ImGuizmo.ImVectorImWchar.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1f2595b2efae43d"></a>[`public int Inno.Native.ImGuizmo.ImVectorInt.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a24c0a44b19a9701"></a>[`public int* Inno.Native.ImGuizmo.ImVectorInt.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c909dce0f47bce37"></a>[`public int Inno.Native.ImGuizmo.ImVectorInt.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf3ac73e77c820b1"></a>[`public int Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0558408075fc6624"></a>[`public Inno.Native.ImGuizmo.StbrpNode* Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6a55551479674ec"></a>[`public int Inno.Native.ImGuizmo.ImVectorStbrpNodeIm.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ad7fd43d2fcd8786"></a>[`public int Inno.Native.ImGuizmo.ImVectorUnsignedChar.Capacity`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5722dc05ae0fa75"></a>[`public byte* Inno.Native.ImGuizmo.ImVectorUnsignedChar.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a37c7f997c4bc369"></a>[`public int Inno.Native.ImGuizmo.ImVectorUnsignedChar.Size`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e3f0cc6b6815754"></a>[`public void* Inno.Native.ImGuizmo.Iobuf.Placeholder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13466) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-52807f0247f679ea"></a>[`public Inno.Native.ImGuizmo.NativeArray_5caf1d5dfc8a2e78 Inno.Native.ImGuizmo.StbrpContextOpaque.Data`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13896) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef8aad1d4d42a69f"></a>[`public float Inno.Native.ImGuizmo.Style.CenterCircleSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dde1b5d40a09bb2e"></a>[`public Inno.Native.ImGuizmo.NativeArray_4a44fb190803b3be Inno.Native.ImGuizmo.Style.Colors`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13946) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-99745d956414e228"></a>[`public float Inno.Native.ImGuizmo.Style.HatchedAxisLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bbf30041f24587bc"></a>[`public float Inno.Native.ImGuizmo.Style.RotationLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13934) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3893ac9ca0ae8cb"></a>[`public float Inno.Native.ImGuizmo.Style.RotationOuterLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13936) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bba8fc1f070672fe"></a>[`public float Inno.Native.ImGuizmo.Style.ScaleLineCircleSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13940) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87e6a364720841a0"></a>[`public float Inno.Native.ImGuizmo.Style.ScaleLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13938) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5bc69672c400c0a"></a>[`public float Inno.Native.ImGuizmo.Style.TranslationLineArrowSize`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13932) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-16f8b317178a925b"></a>[`public float Inno.Native.ImGuizmo.Style.TranslationLineThickness`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13930) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-144dcaeda87e27d6"></a>[`public Inno.Native.ImGuizmo.ImDrawCallback.ImDrawCallback(object @object, nint method)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41d7b775c51391c6"></a>[`public virtual System.IAsyncResult Inno.Native.ImGuizmo.ImDrawCallback.BeginInvoke(Inno.Native.ImGuizmo.ImDrawList* argument0, Inno.Native.ImGuizmo.ImDrawCmd* argument1, System.AsyncCallback callback, object @object)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3f41f0b1399d4d3"></a>[`public virtual void Inno.Native.ImGuizmo.ImDrawCallback.EndInvoke(System.IAsyncResult result)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-36666df5ed331fbf"></a>[`public virtual void Inno.Native.ImGuizmo.ImDrawCallback.Invoke(Inno.Native.ImGuizmo.ImDrawList* argument0, Inno.Native.ImGuizmo.ImDrawCmd* argument1)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95d8dd16ba71dd30"></a>[`public Inno.Native.ImGuizmo.ImGuiContextHookCallback.ImGuiContextHookCallback(object @object, nint method)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa3c1b4930e22b7f"></a>[`public virtual System.IAsyncResult Inno.Native.ImGuizmo.ImGuiContextHookCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiContext* argument0, Inno.Native.ImGuizmo.ImGuiContextHook* argument1, System.AsyncCallback callback, object @object)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2bbe90c29d3cedb"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiContextHookCallback.EndInvoke(System.IAsyncResult result)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f7af65131afafd1"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiContextHookCallback.Invoke(Inno.Native.ImGuizmo.ImGuiContext* argument0, Inno.Native.ImGuizmo.ImGuiContextHook* argument1)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ff5ac2a02a1caa0"></a>[`public Inno.Native.ImGuizmo.ImGuiErrorCallback.ImGuiErrorCallback(object @object, nint method)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c20493193e1ad2b"></a>[`public virtual System.IAsyncResult Inno.Native.ImGuizmo.ImGuiErrorCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiContext* argument0, void* argument1, sbyte* argument2, System.AsyncCallback callback, object @object)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3199862a94af3e61"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiErrorCallback.EndInvoke(System.IAsyncResult result)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd667d49573eb82a"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiErrorCallback.Invoke(Inno.Native.ImGuizmo.ImGuiContext* argument0, void* argument1, sbyte* argument2)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6d9a36ae876d4c93"></a>[`public Inno.Native.ImGuizmo.ImGuiSizeCallback.ImGuiSizeCallback(object @object, nint method)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dfa00fd70e0cc9e1"></a>[`public virtual System.IAsyncResult Inno.Native.ImGuizmo.ImGuiSizeCallback.BeginInvoke(Inno.Native.ImGuizmo.ImGuiSizeCallbackData* argument0, System.AsyncCallback callback, object @object)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a3e959c43eee839"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiSizeCallback.EndInvoke(System.IAsyncResult result)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e25af853d877324"></a>[`public virtual void Inno.Native.ImGuizmo.ImGuiSizeCallback.Invoke(Inno.Native.ImGuizmo.ImGuiSizeCallbackData* argument0)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc812372612e5a1e"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.AllowAxisFlip(bool value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L54) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a13a7f4655de91ce"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.BeginFrame()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L55) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6942e308591bde46"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.Dispose()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L32) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48442ea1a0b652f8"></a>[`public static System.Threading.Tasks.ValueTask Inno.Native.ImGuizmo.ImGuizmo.DisposeAsync()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L33) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ddcddb3db0ec723"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.Enable(bool enable)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L50) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89a92f270a967f12"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L30) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1991e29ff85db09"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsOver()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L48) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f56d8889187d02a5"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation op)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L51) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0388ed704c16022f"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsing()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L40) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-282d0906cea647e2"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsingAny()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L39) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb2de5ca893f4dfd"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsUsingViewManipulate()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L41) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a7b3ef9363eb70ef"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsViewManipulateHovered()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L45) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b1388ae36ce9d2d"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.Load(string libraryPath = "cimguizmo")`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L31) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64c8b6617f150f58"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PopId()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L43) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-decc610a92d24710"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.PushID(int int_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L42) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25eb00fa4564ad4f"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAxisLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L46) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8fa3341b26a6b54"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetAxisMask(bool x, bool y, bool z)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L44) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b63dab49d291a1f4"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetGizmoSizeClipSpace(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L49) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9407557979bcd4bc"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetOrthographic(bool isOrthographic)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L52) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d757ddaa7c72cf32"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetPlaneLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L47) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28c25175fd2fb563"></a>[`public static void Inno.Native.ImGuizmo.ImGuizmo.SetRect(float x, float y, float width, float height)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L53) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b80489d235da9be7"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.AllowAxisFlip(bool value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4800) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c41f40aafd4c52c"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.BeginFrame()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4805) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33ea224766ba9809"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.Dispose()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4716) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| <a id="api-226939314feddd26"></a>[`public System.Threading.Tasks.ValueTask Inno.Native.ImGuizmo.ImGuizmoContext.DisposeAsync()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4717) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.<br>返回：A task that represents the asynchronous dispose operation. |
| <a id="api-c742c60263c9a443"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.Enable(bool enable)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d6d32c9fdb93827"></a>[`public static Inno.Native.ImGuizmo.ImGuizmoContext Inno.Native.ImGuizmo.ImGuizmoContext.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4714) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e3a0cd5463adcf7"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsOver()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b30ce72c4e354f2"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation op)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4785) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6246ba905d549067"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsUsing()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2aeda5c1df9f1cfc"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsUsingAny()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4725) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9be9536bd1147d15"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsUsingViewManipulate()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4df7c67b5df09a15"></a>[`public bool Inno.Native.ImGuizmo.ImGuizmoContext.IsViewManipulateHovered()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4755) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0733c57cf5c99ace"></a>[`public static Inno.Native.ImGuizmo.ImGuizmoContext Inno.Native.ImGuizmo.ImGuizmoContext.Load(string libraryPath = "cimguizmo")`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4715) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c75de54fc2986e1a"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.PopId()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4745) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a297a36d86dcd0c5"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.PushID(int int_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb3118dbe800133d"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.AllowAxisFlip(byte value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5078) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59272cfcb6a55d57"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.BeginFrame()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c33232c117cb6f06"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DecomposeMatrixToComponents(float* matrix, float* translation, float* rotation, float* scale)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4894) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05ddd9a836b29915"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DrawCubes(float* view, float* projection, float* matrices, int matrixCount)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5006) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b1ef3fcf913b22f"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.DrawGrid(float* view, float* projection, float* matrix, float gridSize)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-de920ff726e82f23"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.Enable(byte enable)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5030) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cffa481f1679da3e"></a>[`public uint Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetID(sbyte* str_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4870) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b0ae822386981e98"></a>[`public uint Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetID(void* ptr_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e50447c9424dec8"></a>[`public Inno.Native.ImGuizmo.Style* Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.GetStyle()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4942) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf44fe9569c2a8fa"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4990) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e27627c137971fbf"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver(Inno.Native.ImGuizmo.ImGuizmoOperation op)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-580d9fffe3728895"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsOver(float* position, float pixelRadius)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4926) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2081c29662f9f6b6"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsing()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4846) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e8b25964c6eba9a"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsingAny()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4822) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-edb3b70a2163dff6"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsUsingViewManipulate()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4854) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ed9f903ac2fa462"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.IsViewManipulateHovered()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4950) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d9c8adab66bf701"></a>[`public byte Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.Manipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, float* matrix, float* deltaMatrix, float* snap, float* localBounds, float* boundsSnap)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4998) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-30911263cd7a062b"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PopId()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df1da2f396ccbc9e"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(int int_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4878) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1eb63b1f640dc0a2"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(sbyte* str_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5022) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4cac9301ead889f4"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.PushID(void* ptr_id)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4886) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-224f91b092d010df"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.RecomposeMatrixFromComponents(float* translation, float* rotation, float* scale, float* matrix)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a118bd22acbe0145"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAlternativeWindow(Inno.Native.ImGuizmo.ImGuiWindow* window)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5070) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df1c134dd26d97b3"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAxisLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4958) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-762c609d513b607f"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetAxisMask(byte x, byte y, byte z)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ebfbf734772b117a"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetDrawlist(Inno.Native.ImGuizmo.ImDrawList* drawlist)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f5085e983cc4a32"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetGizmoSizeClipSpace(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2586d04110d44ecf"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetImGuiContext(Inno.Native.ImGuizmo.ImGuiContext* ctx)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5046) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08ca5d6ac7b6b2f8"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetOrthographic(byte isOrthographic)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5054) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e8dfd50df34a74a"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetPlaneLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4966) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22a15f2778853393"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.SetRect(float x, float y, float width, float height)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5062) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ee88bd9a5b7ddb6"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.StyleDestroy(Inno.Native.ImGuizmo.Style* self)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5086) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d00aa8d01a2a25d"></a>[`public Inno.Native.ImGuizmo.Style* Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.StyleStyle()`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4838) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8847b53474a63e75"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.ViewManipulate(float* view, float* projection, Inno.Native.ImGuizmo.ImGuizmoOperation operation, Inno.Native.ImGuizmo.ImGuizmoMode mode, float* matrix, float length, Inno.Native.ImGuizmo.ImVec2C position, Inno.Native.ImGuizmo.ImVec2C size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4934) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8baf91928e7756d6"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.RawApi.ViewManipulate(float* view, float length, Inno.Native.ImGuizmo.ImVec2C position, Inno.Native.ImGuizmo.ImVec2C size, uint backgroundColor)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1326950aca376ecb"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetAxisLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4760) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2a83c22a42096e9"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetAxisMask(bool x, bool y, bool z)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4750) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0473945365c51a9"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetGizmoSizeClipSpace(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4775) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b9aaa225822c569"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetOrthographic(bool isOrthographic)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4790) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-837bd35bd87083b6"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetPlaneLimit(float value)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4765) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b379b8f8703a7f2"></a>[`public void Inno.Native.ImGuizmo.ImGuizmoContext.SetRect(float x, float y, float width, float height)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4795) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b09395605afba87"></a>[`public uint Inno.Native.ImGuizmo.ImFontAtlasRectEntry.Generation { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5701) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e06793d3553f1e55"></a>[`public uint Inno.Native.ImGuizmo.ImFontAtlasRectEntry.IsUsed { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5733) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6a5840ed97867b7"></a>[`public int Inno.Native.ImGuizmo.ImFontAtlasRectEntry.TargetIndex { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5669) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc05ae2d7c8c10f3"></a>[`public uint Inno.Native.ImGuizmo.ImFontBaked.LoadNoFallback { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f84df799fcff9b27"></a>[`public uint Inno.Native.ImGuizmo.ImFontBaked.LoadNoRenderOnLayout { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1feeb496bf034502"></a>[`public uint Inno.Native.ImGuizmo.ImFontBaked.MetricsTotalSurface { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8851d7eaf7a1026"></a>[`public uint Inno.Native.ImGuizmo.ImFontBaked.WantDestroy { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5833) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b5a877073baee33a"></a>[`public uint Inno.Native.ImGuizmo.ImFontGlyph.Codepoint { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f1a26426a74fc0d9"></a>[`public uint Inno.Native.ImGuizmo.ImFontGlyph.Colored { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6025) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d38129ed1e0a252"></a>[`public uint Inno.Native.ImGuizmo.ImFontGlyph.SourceIdx { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a370ba4fde33e41a"></a>[`public uint Inno.Native.ImGuizmo.ImFontGlyph.Visible { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1666a883cd6779c"></a>[`public int Inno.Native.ImGuizmo.ImGuiBoxSelectState.KeyMods { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6259) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f9bd0023fa105ba"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForPos { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9abde6452b608b76"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForSize { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7296) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d061ad186375847"></a>[`public int Inno.Native.ImGuizmo.ImGuiDockNode.AuthorityForViewport { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2202808e86e6c0e4"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.HasCentralNodeChild { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bcf42c6cce6d81b0"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.HasCloseButton { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c3dfdc5419b053a"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.HasWindowMenuButton { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f1a0d6f35126cde"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.IsBgDrawnThisFrame { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdb0e7193c98bfa6"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.IsFocused { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35082a42e6f4c7db"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.IsVisible { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aeca0ea858dceadb"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.WantCloseAll { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1da63249a8f74bb1"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.WantHiddenTabBarToggle { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7680) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90de895bd6c975a9"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.WantHiddenTabBarUpdate { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6ae8a70906bdb0a"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.WantLockSizeOnce { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2bf50336b06e1497"></a>[`public bool Inno.Native.ImGuizmo.ImGuiDockNode.WantMouseMove { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c46d3f01a36b6d9"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.LastRowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10272) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9a9cee191af0c33"></a>[`public int Inno.Native.ImGuizmo.ImGuiTable.RowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10240) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10f3057472674b61"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirection { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8af3e11284be5f34"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailCount { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe1fe6d73f3b61b7"></a>[`public byte Inno.Native.ImGuizmo.ImGuiTableColumn.SortDirectionsAvailMask { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9cc9e904f7359590"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.BgClickFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11134) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd12d0b1eede54f9"></a>[`public bool Inno.Native.ImGuizmo.ImGuiWindow.DockIsActive { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b61deb533a6794a5"></a>[`public bool Inno.Native.ImGuizmo.ImGuiWindow.DockNodeIsVisible { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e16f2cd64b563342"></a>[`public bool Inno.Native.ImGuizmo.ImGuiWindow.DockTabIsVisible { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b09f738fdb3d19e"></a>[`public bool Inno.Native.ImGuizmo.ImGuiWindow.DockTabWantClose { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1274ff84c3cd1c63"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.SetWindowCollapsedAllowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11230) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ff5ba7d9337715a"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.SetWindowDockAllowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11262) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5000ffadee48d1cf"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.SetWindowPosAllowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7241982d7b1efab"></a>[`public int Inno.Native.ImGuizmo.ImGuiWindow.SetWindowSizeAllowFlags { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11198) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1e21df04bc1ace3"></a>[`public static bool Inno.Native.ImGuizmo.ImGuizmo.IsInitialized { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L21) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13d47b5dc93d5f8e"></a>[`public static Inno.Native.ImGuizmo.ImGuizmoContext.RawApi Inno.Native.ImGuizmo.ImGuizmo.Raw { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L22) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84ed898a447f60c2"></a>[`public Inno.Native.ImGuizmo.ImGuizmoContext.RawApi Inno.Native.ImGuizmo.ImGuizmoContext.Raw { get; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L100) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70c9b1ef5ed49357"></a>[`public Inno.Native.ImGuizmo.ImDrawList* Inno.Native.ImGuizmo.NativeArray_1eccc798bd000925.this[int index] { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8805d327d7376ac5"></a>[`public Inno.Native.ImGuizmo.ImGuiDockNode* Inno.Native.ImGuizmo.NativeArray_68e25b300381a6fc.this[int index] { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13631) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-508044ec4d68b462"></a>[`public Inno.Native.ImGuizmo.ImVectorImDrawListPtr* Inno.Native.ImGuizmo.NativeArray_7b9f5e5665cb58c5.this[int index] { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13680) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-99481739f9c2b492"></a>[`public sbyte* Inno.Native.ImGuizmo.NativeArray_df98efe8482c9a26.this[int index] { get; set; }`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5ec15746002f7b0"></a>[`public struct Inno.Native.ImGuizmo.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-637bf3c187e9338b"></a>[`public struct Inno.Native.ImGuizmo.ImChunkStreamImGuiTableSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5177) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c2721af6adf8776"></a>[`public struct Inno.Native.ImGuizmo.ImChunkStreamImGuiWindowSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-429b074bd9dbc70f"></a>[`public delegate void Inno.Native.ImGuizmo.ImDrawCallback(Inno.Native.ImGuizmo.ImDrawList* argument0, Inno.Native.ImGuizmo.ImDrawCmd* argument1)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3e110cb441d3de9"></a>[`public struct Inno.Native.ImGuizmo.ImDrawChannel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f14d7f505823536"></a>[`public struct Inno.Native.ImGuizmo.ImDrawCmd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5239) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49505b92a6aca334"></a>[`public struct Inno.Native.ImGuizmo.ImDrawCmdHeader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5273) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc7fa8092ffbe11f"></a>[`public struct Inno.Native.ImGuizmo.ImDrawData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5295) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dc1f25f4f99cd4d"></a>[`public struct Inno.Native.ImGuizmo.ImDrawDataBuilder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed59b4381c73dd06"></a>[`public struct Inno.Native.ImGuizmo.ImDrawList`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5df6c58c817da2f2"></a>[`public struct Inno.Native.ImGuizmo.ImDrawListSharedData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5b00a3fca80d1ae"></a>[`public struct Inno.Native.ImGuizmo.ImDrawListSplitter`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c68f2f5195eebcae"></a>[`public struct Inno.Native.ImGuizmo.ImDrawVert`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8846efe7c70c7d04"></a>[`public struct Inno.Native.ImGuizmo.ImFont`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-074726c8441affb0"></a>[`public struct Inno.Native.ImGuizmo.ImFontAtlas`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6eac39ad8315b8a"></a>[`public struct Inno.Native.ImGuizmo.ImFontAtlasBuilder`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f84d2316b6843654"></a>[`public struct Inno.Native.ImGuizmo.ImFontAtlasRectEntry`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5665) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1624260bc0d2e5af"></a>[`public struct Inno.Native.ImGuizmo.ImFontBaked`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5771) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa11e04f577a39c7"></a>[`public struct Inno.Native.ImGuizmo.ImFontConfig`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c57f8c8ed3a3c8da"></a>[`public struct Inno.Native.ImGuizmo.ImFontGlyph`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6001) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5af29423098823f7"></a>[`public struct Inno.Native.ImGuizmo.ImFontLoader`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01fdb0f9b22202a1"></a>[`public struct Inno.Native.ImGuizmo.ImFontStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f60f54b24a1f113"></a>[`public enum Inno.Native.ImGuizmo.ImGuiAxis : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6215) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccd591d48366e4c7"></a>[`public struct Inno.Native.ImGuizmo.ImGuiBoxSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6227) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec87cbb93ced7600"></a>[`public struct Inno.Native.ImGuizmo.ImGuiColorMod`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a3608284ab9f4a76"></a>[`public struct Inno.Native.ImGuizmo.ImGuiComboPreviewData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6317) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-467a80e5c02db33e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L6345) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e42ec765891ad71"></a>[`public struct Inno.Native.ImGuizmo.ImGuiContextHook`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7002) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1159a4ac0052999"></a>[`public delegate void Inno.Native.ImGuizmo.ImGuiContextHookCallback(Inno.Native.ImGuizmo.ImGuiContext* argument0, Inno.Native.ImGuizmo.ImGuiContextHook* argument1)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64d0146f0bbcc3b2"></a>[`public enum Inno.Native.ImGuizmo.ImGuiContextHookType : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7032) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5f0932f6f7f4d91"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDataTypeStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7049) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-acad305f1a85ffb7"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDeactivatedItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-429d3d1396b6576c"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDebugAllocEntry`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-467478074c9f5dd1"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDebugAllocInfo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7113) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b52bcc38d3c4501"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDebugItemPathQuery`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7137) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c2235481c17fe76"></a>[`public enum Inno.Native.ImGuizmo.ImGuiDir : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7165) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6aadd177f2a16930"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDockContext`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7180) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a02fa724597387a4"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDockNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42ca3a4042f5da82"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDockNodeSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7717) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-40c0355c759f0a1b"></a>[`public enum Inno.Native.ImGuizmo.ImGuiDockNodeState : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7724) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2ac536a9fad8878"></a>[`public struct Inno.Native.ImGuizmo.ImGuiDockRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d911a1f55ab8e5de"></a>[`public delegate void Inno.Native.ImGuizmo.ImGuiErrorCallback(Inno.Native.ImGuizmo.ImGuiContext* argument0, void* argument1, sbyte* argument2)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-23abcbf616a7d5e3"></a>[`public struct Inno.Native.ImGuizmo.ImGuiErrorRecoveryState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7751) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3719e4c6975a0906"></a>[`public struct Inno.Native.ImGuizmo.ImGuiFocusScopeData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7789) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a6b06029ff299127"></a>[`public struct Inno.Native.ImGuizmo.ImGuiGroupData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7809) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-901e3c922e846894"></a>[`public struct Inno.Native.ImGuizmo.ImGuiIDStackTool`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7853) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-563bec5ce60eaa29"></a>[`public struct Inno.Native.ImGuizmo.ImGuiIO`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7453c4b632a46a83"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEvent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7877) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a5dc9b9d191de83e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventAnonymousUnionC908181b7025c440`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b59126a9e72394b"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventAppFocused`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7933) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b070d364588fe21f"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventKey`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7951) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22b11821054390ab"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventMouseButton`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7973) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ca371ccff89fb9d"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventMousePos`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L7995) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8566fbeab2a1ab0b"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventMouseViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8017) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07e067e5476d1064"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventMouseWheel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43c503745f4b979e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputEventText`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-495c8e7ff86920bb"></a>[`public enum Inno.Native.ImGuizmo.ImGuiInputEventType : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8073) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a133a0a0bf4f8730"></a>[`public enum Inno.Native.ImGuizmo.ImGuiInputSource : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecc6befb7359c5ba"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputTextDeactivatedState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8103) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1103ebcf7fba62a2"></a>[`public struct Inno.Native.ImGuizmo.ImGuiInputTextState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8123) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76be9d072dd019e3"></a>[`public enum Inno.Native.ImGuizmo.ImGuiKey : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84963a9168f12156"></a>[`public struct Inno.Native.ImGuizmo.ImGuiKeyData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8595) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4b34f41644435e6"></a>[`public struct Inno.Native.ImGuizmo.ImGuiKeyOwnerData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f70b742247137219"></a>[`public struct Inno.Native.ImGuizmo.ImGuiKeyRoutingData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ebe20d632157654"></a>[`public struct Inno.Native.ImGuizmo.ImGuiKeyRoutingTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8671) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03479079196ad334"></a>[`public struct Inno.Native.ImGuizmo.ImGuiLastItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-532516ae0d2b2305"></a>[`public struct Inno.Native.ImGuizmo.ImGuiListClipper`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8725) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbfe8b88a3595928"></a>[`public struct Inno.Native.ImGuizmo.ImGuiListClipperData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-14a1f060b5204878"></a>[`public struct Inno.Native.ImGuizmo.ImGuiListClipperRange`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8785) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc4a8f92c0e74c59"></a>[`public struct Inno.Native.ImGuizmo.ImGuiMenuColumns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8811) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-792cdda4b0ab8c68"></a>[`public struct Inno.Native.ImGuizmo.ImGuiMetricsConfig`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d24a64df98ee4d72"></a>[`public enum Inno.Native.ImGuizmo.ImGuiMouseSource : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8887) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-455f02d941f9abff"></a>[`public struct Inno.Native.ImGuizmo.ImGuiMultiSelectIO`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8900) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a2e0512378dba53"></a>[`public struct Inno.Native.ImGuizmo.ImGuiMultiSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8928) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-247f952c7afe21b2"></a>[`public struct Inno.Native.ImGuizmo.ImGuiMultiSelectTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L8960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee3c38747892ff8c"></a>[`public struct Inno.Native.ImGuizmo.ImGuiNavItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9008) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51e5b7766833d6dc"></a>[`public enum Inno.Native.ImGuizmo.ImGuiNavLayer : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-99bb56f51e18ab3a"></a>[`public struct Inno.Native.ImGuizmo.ImGuiNextItemData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9052) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df1a4780136f92ef"></a>[`public struct Inno.Native.ImGuizmo.ImGuiNextWindowData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9092) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ad3531fbf86bd7f"></a>[`public struct Inno.Native.ImGuizmo.ImGuiOldColumnData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9154) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c288dce7d426cc5"></a>[`public struct Inno.Native.ImGuizmo.ImGuiOldColumns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9178) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3dd05841134ee773"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPayload`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9228) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aba956dd0d142ece"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPlatformIO`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7ca9121d0385110"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPlatformImeData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bd2ba8befba1ff0"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPlatformMonitor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-737a6d427d467104"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPopupData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-717bc25309a4ee15"></a>[`public struct Inno.Native.ImGuizmo.ImGuiPtrOrIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbf97c2a7c89bdbe"></a>[`public struct Inno.Native.ImGuizmo.ImGuiSelectionRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-754be423f5071c12"></a>[`public enum Inno.Native.ImGuizmo.ImGuiSelectionRequestType : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2735b432426cdd38"></a>[`public struct Inno.Native.ImGuizmo.ImGuiSettingsHandler`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7b441a27bdd054e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiShrinkWidthItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9530) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c5bb11cb95d544c0"></a>[`public delegate void Inno.Native.ImGuizmo.ImGuiSizeCallback(Inno.Native.ImGuizmo.ImGuiSizeCallbackData* argument0)`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3d6a4fcc70f69f8"></a>[`public struct Inno.Native.ImGuizmo.ImGuiSizeCallbackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d646aadab10f2b4e"></a>[`public enum Inno.Native.ImGuizmo.ImGuiSortDirection : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6437c6dec5e98d75"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStackLevelInfo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82344016ee1988c6"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStorage`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c59825ca181ccaf"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStoragePair`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9636) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b71522370fc0037"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9922db72a863c1e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStyle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9678) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6bd3feff27c3a76a"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStyleMod`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9842) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2bfa66d41f999a79"></a>[`public struct Inno.Native.ImGuizmo.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-88fa52e075a13663"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9882) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3fa821064a2f93b8"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTabItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L9972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c4452c3ba635895"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0fb7bf06fc3b0387"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableCellData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b42e0980dd2154b8"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69a626643f3a1d54"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableColumnSortSpecs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d91f58fec25eea61"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableHeaderData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b584fbaf83e99c4e"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableInstanceData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3cef797a4e5acf8d"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableSortSpecs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ac0216bb898c3ed"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTableTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-086f3493b4980088"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTextBuffer`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10658) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-868527addbdb369c"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTextIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9a11afc7a0b2435"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTreeNodeStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1af7a2b1569b0833"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTypingSelectRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98b73e4a949c9ff1"></a>[`public struct Inno.Native.ImGuizmo.ImGuiTypingSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10754) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e158ae0fc0d61ec8"></a>[`public struct Inno.Native.ImGuizmo.ImGuiViewport`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ed5132587200a65"></a>[`public struct Inno.Native.ImGuizmo.ImGuiViewportP`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10836) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d580248b5ef2e19"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindow`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L10898) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-450d2d54eb8d117b"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindowClass`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4b12ec2e63762d5"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindowDockStyle`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5fc7c5383a8209c"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindowStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3d2e8f01d886d84"></a>[`public struct Inno.Native.ImGuizmo.ImGuiWindowTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0dad34c884f5db58"></a>[`public static class Inno.Native.ImGuizmo.ImGuizmo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/ImGuizmo.cs#L9) | Initializes generated imports against the component's dynamic native library. |
| <a id="api-b5ddd4e5526053e0"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoColor : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11606) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18f28d2cf6f7c6be"></a>[`public sealed class Inno.Native.ImGuizmo.ImGuizmoContext : System.IDisposable, System.IAsyncDisposable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L61) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29bbd36af19db47c"></a>[`public sealed class Inno.Native.ImGuizmo.ImGuizmoContext.RawApi`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L4816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e9bea3fd570e80c"></a>[`public static class Inno.Native.ImGuizmo.ImGuizmoContext.RecipeConversions`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L5107) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d56bfb7db457de4"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoMode : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11629) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e947f2b321c138b1"></a>[`public enum Inno.Native.ImGuizmo.ImGuizmoOperation : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11638) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f527c3af5ea0f21e"></a>[`public struct Inno.Native.ImGuizmo.ImPoolImGuiMultiSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11666) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6fe8e3684217029"></a>[`public struct Inno.Native.ImGuizmo.ImPoolImGuiTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11690) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-94b8e8375638efc4"></a>[`public struct Inno.Native.ImGuizmo.ImPoolImGuiTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11714) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d2b2eefce80a6b7"></a>[`public struct Inno.Native.ImGuizmo.ImRectC`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cfb10b6fa2df24eb"></a>[`public struct Inno.Native.ImGuizmo.ImSpanImGuiTableCellData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b7fabd425ec4cc96"></a>[`public struct Inno.Native.ImGuizmo.ImSpanImGuiTableColumn`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11778) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb4df74903389ffa"></a>[`public struct Inno.Native.ImGuizmo.ImSpanImGuiTableColumnIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11798) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c87086ce82bc53f4"></a>[`public struct Inno.Native.ImGuizmo.ImStableVectorImFontBaked32`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9f826e216f3944c"></a>[`public struct Inno.Native.ImGuizmo.ImTextureData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11840) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60b646ed6d39794d"></a>[`public enum Inno.Native.ImGuizmo.ImTextureFormat : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11886) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f279ed6c233f6f35"></a>[`public struct Inno.Native.ImGuizmo.ImTextureRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-012ece51a8d3c5c8"></a>[`public struct Inno.Native.ImGuizmo.ImTextureRefC`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11921) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d9035fa56dc045b"></a>[`public enum Inno.Native.ImGuizmo.ImTextureStatus : int`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11939) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53c9fbc1c8330f18"></a>[`public struct Inno.Native.ImGuizmo.ImVec1`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1fcf5ff20d92855"></a>[`public struct Inno.Native.ImGuizmo.ImVec2C`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11971) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ccee8393bee1472"></a>[`public struct Inno.Native.ImGuizmo.ImVec2iC`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L11991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f20b2f424618508a"></a>[`public struct Inno.Native.ImGuizmo.ImVec2ih`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e35060ebc88dc7a"></a>[`public struct Inno.Native.ImGuizmo.ImVec4C`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86235f5d0c172df3"></a>[`public struct Inno.Native.ImGuizmo.ImVectorChar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6dbdf8ee3425fa39"></a>[`public struct Inno.Native.ImGuizmo.ImVectorFloat`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80f4b597dfa79e35"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawChannel`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e22fb0601c1b8cf9"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawCmd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f9505134e75a887"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawIdx`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12143) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4342ab3921e76773"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawListPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12165) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ab86cd1abd51e06"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawListSharedDataPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2fe33e8e8b12da13"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImDrawVert`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12209) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-04ac0f4fdb2b2ec6"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontAtlasPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-984f452007afff14"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontAtlasRectEntry`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca4e9015d689a145"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontBakedPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12275) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9ebbd44ce9257ae"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontConfig`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f5dab7ad808de81"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontConfigPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12319) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4e28ae826d55fce"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontGlyph`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6cfaa7070f232f99"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12363) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4994a655c6fa82ff"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImFontStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6619e4fb8755d19d"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiColorMod`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12407) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1725d26785e31edb"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiContextHook`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ec41e044282a743"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiDockNodeSettings`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7edcd4161bd55196"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiDockRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12473) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec8fa7a08bfd7a96"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiFocusScopeData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2eec6df6d891321d"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiGroupData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c07d922c11c36637"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiID`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-620c31e8468a42c7"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiInputEvent`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a8875564bf6e4bb"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiItemFlags`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efb3d132ff6b7c31"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiKeyRoutingData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12605) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7d5da9d8e118df4"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiListClipperData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12627) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e371df0e76a6a0f"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiListClipperRange`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12649) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd21dc5f60c67f38"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12671) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6b64a42273af210"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiMultiSelectTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f370831e8a02ec8f"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiOldColumnData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12715) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b65d1c34b8a70725"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiOldColumns`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c83e68a7619d6042"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiPlatformMonitor`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c025552795b127a8"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiPopupData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12781) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-347c7710209f625f"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiPtrOrIndex`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fb494fa8da93bc10"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiSelectionRequest`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12825) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b97fd27e960da3a"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiSettingsHandler`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06b4d4085d83bc7b"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiShrinkWidthItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12869) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b7dd9791c3b3b629"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiStackLevelInfo`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12891) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9784fd0ea2d84e31"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiStoragePair`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d2e244f2ae840c6"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiStyleMod`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-064cdc194707f333"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTabBar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d4528adce2d3e30b"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTabItem`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L12979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c33ed9222db2c05e"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTable`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13001) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-61c8e0d9e955763d"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTableColumnSortSpecs`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9a9c6a042e61874e"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTableHeaderData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13045) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b830547ea64e6b7"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTableInstanceData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-007c0ce26f4ac165"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTableTempData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6675cb12277ae7ab"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiTreeNodeStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13111) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb854b8eb6b007ee"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiViewportPPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7aa26621a2b38e1"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiViewportPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13155) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb72f1b27d549317"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiWindowPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13177) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-765e1c44a4711099"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImGuiWindowStackData`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13199) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a995668ff549142b"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImTextureDataPtr`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8e12d9a21741cbe"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImTextureRect`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13243) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5374b1055bf08ccb"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImTextureRef`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13265) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb8c8a639ee59ea1"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImU16`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13287) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7eeadd09e686c9e"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImU8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-40337af1a9df10af"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImVec2`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3cd6f1827ec1f59a"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImVec4`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13353) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84b5ca188570cf57"></a>[`public struct Inno.Native.ImGuizmo.ImVectorImWchar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-079c1746872c6dbe"></a>[`public struct Inno.Native.ImGuizmo.ImVectorInt`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85554aa42513afe8"></a>[`public struct Inno.Native.ImGuizmo.ImVectorStbrpNodeIm`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fae373d75e933df5"></a>[`public struct Inno.Native.ImGuizmo.ImVectorUnsignedChar`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f060a2a86fcd9c7e"></a>[`public struct Inno.Native.ImGuizmo.Iobuf`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd4eab332550a3a3"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_16a683b69e90f4cd`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dc765ebbf49e1a4"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_1eccc798bd000925`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa3eeb41bef9aeed"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_3325e8ed3a9a98ba`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c05e046d941516f"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_332d40ffc044d296`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0aebc3fe84ed31ff"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_3571fc580e8d9e55`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4965ed47be0def83"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_35aebb639e6eb330`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8154f704d6c9b1ef"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_3d337b926fc04643`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0209be7cff75b353"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_41ea187b8e0849de`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9f7a47dd60251c3c"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_42c049e3f15c43ec`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-437ac3bb1dd2ee79"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_43db0ce246f18b3f`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-324aca793f5355be"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_4a44fb190803b3be`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28342f394790fda3"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_5caf1d5dfc8a2e78`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13601) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ed32eb3533eb0e40"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_5dde4d9494a49aa0`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-665efd29be29ada3"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_605a5c71dc256950`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8685fe17011f6467"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_68e25b300381a6fc`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbb2b9ff3ad21492"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_704527fa6c941baa`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13659) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-986f3fc400e48bb4"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_76771c8c9ab15473`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13668) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9acc694e2c337fad"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_7b9f5e5665cb58c5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13677) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dae97b5594c9c7bd"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_87beacaa77c69f9d`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b816213729d35065"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_8da6be2ffdb0de05`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13717) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b9a89e4a199348d"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_9be1bbaad6627fe8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b18a3d13057e4da"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_a4748c2e54d124a7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b184ad2b72db6abb"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_afa9ec5b5680f876`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f98300623f5ea7a6"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_b3004bc9c15942c6`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13753) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e3df23d6fe41c74"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_b726fff0e8801db7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-15fdafe907d92d73"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_bae28ecfe547f5d7`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13771) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cedf7a4e4cc5b0b3"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_c34fe7f9f3db1a4e`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-118fd1d8a01677a4"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_d36642d3104a6c6f`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13789) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b97d33f3bfb9cf9"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_df98efe8482c9a26`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13798) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0210ba9784751fd1"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_e60e879954c840fa`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13829) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ceed76c2753e7e6"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_ec6e6e3c26b88eb8`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13838) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-73b31408877bd51a"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_f3ea9f13691638c5`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3abcfe0db7277f4"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_f4284eefd115ca43`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13856) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9c6d8c42611e70f"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_f9c2e11bc01cc26a`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03316d6db8c8a88b"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_fcecdbb15210e44b`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-acc0e259ef151860"></a>[`public struct Inno.Native.ImGuizmo.NativeArray_ff485db19a16ca26`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13883) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-187fee0e356964bf"></a>[`public struct Inno.Native.ImGuizmo.STBTexteditState`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-441cd03275d550ad"></a>[`public struct Inno.Native.ImGuizmo.StbrpContextOpaque`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13893) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e56d299ba279b515"></a>[`public struct Inno.Native.ImGuizmo.StbrpNode`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a087d023d7380fb"></a>[`public struct Inno.Native.ImGuizmo.Style`](../../../backends/ImGui/native/Inno.Native.ImGuizmo/Generated/Bindings.cs#L13927) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
