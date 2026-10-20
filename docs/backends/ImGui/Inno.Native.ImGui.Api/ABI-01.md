# Inno.Native.ImGui ABI · 01

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN.Data b09659ed94a7c5d0c0d36482cc93febc00cff858d469cd41252ebde49a546319 -->
<!-- api: F:Inno.Native.ImGui.ImBitVector.Storage 4f5b65f9a2df668cab5a4f1036b82173b3539951c199cc5ac245f26acc585c4f -->
<!-- api: F:Inno.Native.ImGui.ImChunkStreamImGuiTableSettings.Buf 700bf5734a49cd64984beca2d8670cfface7f24edc431ebc47f8484b063c73e8 -->
<!-- api: F:Inno.Native.ImGui.ImChunkStreamImGuiWindowSettings.Buf fed1731072dc4f2687496f9b84c2a59c59f19a6f41984e5d2dae2646d0c549a6 -->
<!-- api: F:Inno.Native.ImGui.ImColor.Value 5b61dfe56dc5b64074672c5ebe6cb4f274d229df3afb91ae7391bc98f4d42603 -->
<!-- api: F:Inno.Native.ImGui.ImDrawChannel.CmdBuffer 3dda6b7136c878ff0cfe797500ef571b69afc54551560ed87862080ada094b11 -->
<!-- api: F:Inno.Native.ImGui.ImDrawChannel.IdxBuffer 5fa9e4707743ffb5411ff2a66428674158e12ad39662a954c82f06d31ebf191e -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.ClipRect 9159b5cc90ca4bc0263f3727dc69f8ebdf2681a85f4277271de79f2c9c9ff581 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.ElemCount 753d501342f6b1e723187d0b1e715cde57c880945c561d332ac9b71e526db096 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.IdxOffset 5ecd224ff7f72ca4daa2f744597ee025bf9469c0c8589fbbaf5b90c09f7759d3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.TexRef 1cb1d6810b1d4dff1a799545a5cdc86a3898014fc965fa2b33a11eeb047306c6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.UserCallback 1797cb5474c4c8a98b27f1d722ed0c21b9c6b427c454bc29c78cd89fdc499426 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.UserCallbackData 2707db84b47db6630e768d0a9ed918bee86cf4e97960a733dfc34f05b892a45a -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.UserCallbackDataOffset c2f83742fa7e7e0c04218ae1c034006249571399c9b72e51025e8172874fdf9d -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.UserCallbackDataSize 062803caa71f9f29b1c84fe827f7cc61ecec9017a0f0b69167f2c89d5601756b -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmd.VtxOffset cb037fd23d52969c61c2a7f8fe2f1da1b69bbd217870ff8bc308c4296b5b731b -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmdHeader.ClipRect 46003b3f0215d9d0c2de3f9a77785d9a7be10407024ed52ccfca63b1158a89ab -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmdHeader.TexRef 55cf3f15e8699ebd45363091104a9d4f92eef581e4893f93e6d612d9caec8704 -->
<!-- api: F:Inno.Native.ImGui.ImDrawCmdHeader.VtxOffset c01a4f0c3d978c4c188a48279724789daccabbcac0072b7dfcc0227ade67ece9 -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.CmdLists a6d5ac3e28b0baee7aa7d198bb9caa1bf3b6f35c360358bf2ba486083b2118bb -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.CmdListsCount 1266d4128fe9552f35e5b1885607bd6636ed48876fac52c41b61b298955ca35d -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.DisplayPos c18baeba9689359ab86b10e316e66305cf75c183958141e8eacd3495f65b663f -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.DisplaySize 2fae62f7d23aa716fef740f82c72f2498fefbca8c6b81627c5d7918816a1561f -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.FramebufferScale 85d88678080f03b311ab567b88a62a0b59d0edcdb16250fb9164d37eaac640f3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.OwnerViewport 5146d90371c6e5b75007694df86dc0821c196832cfbad61e6358d6cf2b95bfcb -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.Textures 460e62d15a004045c433146b1bd5ace4c995c906820cfab6c3a7300a32422a04 -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.TotalIdxCount 095b214ee19fd9130adb21458a25ad350d00a426e6a71dd86031992307f7234f -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.TotalVtxCount 2a652d8cc655be3ffa994d51bda0ff4c0501f0ca774b53df8be27bcba713d837 -->
<!-- api: F:Inno.Native.ImGui.ImDrawData.Valid bd7265826456fd68f14155e38b3ef62c2d5c9942002dce6d3604edb5e5782e0a -->
<!-- api: F:Inno.Native.ImGui.ImDrawDataBuilder.LayerData1 e193b06c0fec04cd9b69a3c8ca8a0f130de53d7e937ef1fabe0a4e658d4a6281 -->
<!-- api: F:Inno.Native.ImGui.ImDrawDataBuilder.Layers 0fdfce163ce2403deeb043345c43223150bab0d967dc1092a9c3955a2e70e2c5 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.Closed 52f41e43d21e0afe6347bc65f49f5f7f7c65af0ac0a8e0596f537521da816eca -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.None 38f2d547e5769e7b85b81e9240f852d588fe6ffdd03c25ab1b720d6f927050cb -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersAll 0fa10c93f1af623010caee24e02a7b8cdc9bc26d9780f8aa4748a8d4de8dc64c -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersBottom 77458f694cd2c2c42cb8ca86b98d2a9c6f286f3304ace5e0597100ceb4cc46a7 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersBottomLeft dbc625aec9d63e6b163a93f56df3f901d4c30522ca50fb881307b27f4dd281db -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersBottomRight 7043c8caf8e3454b91790c16da07e32a09e2d291a8ba34b7cd1a71811f7f753c -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersDefault 66ca41c125c16ec46bb2ece69ae74581260f8bfbf3f817e9d1d735cdeb8ea8f2 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersLeft f422f44bb9dfbcafa07a964fdf53d1b743fe8085ea0572663f1ed516a0f8fedf -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersMask 08e4c99f4b615660817013e2e1fedb16a49a71864ff137ae426097d07a806de8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersNone 99b9264a7d9b194895fed0c096fc8c75138e2aa71bb98568a1cd8312f6b7566b -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersRight 47581fe0b41c95eb2d9e8a9d9a19d4644670886f10059a2cf2309a0203a0d366 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersTop e1a80607c2354b36489b7855735400dfa151b3296ce77e915cb08aa82e9fe367 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersTopLeft 8545c4d4c7a6e61729b5a4a58f808ad16592108a54bec9d5c562ca22ab7de988 -->
<!-- api: F:Inno.Native.ImGui.ImDrawFlags.RoundCornersTopRight 761cf81bb26a0edbaacc898e7c70f3523a3773afbfb0d4fecceb1a112b67922d -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.CallbacksDataBuf f4bfeea5d6118636455b6396f6f657b3fe581e115b756cd2f33faeeb82e3b02c -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.ClipRectStack 96a9228d1186ad7f39948bb85fc1f29739ae5f4100207171215a38dfd3cb00fb -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.CmdBuffer e490207e8dff4523a3169299dd19b8064f141b2e6a888df1fe8edd4509c0c972 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.CmdHeader 4b5cae0c3ffb34ee377031879ea56904c0c045435a0774f0c14396d59446452b -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.Data 4fe48cc8ccbcc617fc7c6f91691b35b2fd1ab28ad970f889e854b0d700c4ee4e -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.Flags 5eeae3b9be5fec8138c6460b56c3c7a277a24ce8236798b49fbdbd6fe4a43bc4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.FringeScale 8f83a43779cf1acd94645c4f5d6581d870debefb8320670455b95ae1072156bc -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.IdxBuffer 1d89c61c36d6426d859383775f4ccea606a66fbba44b12d978568c7f394069af -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.IdxWritePtr 8ca1bd5a28b727cd4aebec3e3c02b98bc91c5dd7e0b74195ca9db417a64c7e73 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.OwnerName e2d9c2ae096a955c73f7c5e2082236a08a9b5c80cee21cd91a44194713da322c -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.Path 9091b01575fec414aa456a238faf5d3d6c6714ba595bc623106c7328e683c275 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.Splitter c52cf43f55dd366cd46dd1eff08ddd2c1e996d504b600a63536e6439e8826b4b -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.TextureStack 859de492db95ea2047e51b0460bcdc1d53fe47d21e120e4ededa792e61181236 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.VtxBuffer 708bb25afacd6235f0f87be207a066108bc140bacaf1f6460b8e9302a32f68c0 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.VtxCurrentIdx e65b7e2f2d4697db1bd4d079184136f98b89f5554c1b1a6153e6a6defcbf7471 -->
<!-- api: F:Inno.Native.ImGui.ImDrawList.VtxWritePtr 5314257cf286df74465f6f982bdb77d4f64c4e2595172d71e3f8c7167736ca35 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListFlags.AllowVtxOffset 74affe80abae9c88e868cf8584c6fee3e3b5667f3d1d8cc9fa0243257e98e65c -->
<!-- api: F:Inno.Native.ImGui.ImDrawListFlags.AntiAliasedFill 69f32345581eb33fb9a4d9b3e16740618f43afc4aad74072c2ff0507618d1395 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListFlags.AntiAliasedLines ec1c29d66e8810b8d6eee90db8918f986fc20ad6ffb6aa96758b754978532627 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListFlags.AntiAliasedLinesUseTex c71ea519a9e1a1f2ae7dd220e4ef19406c1c286ca80149593589c594cde46212 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListFlags.None 658362a496f8dd238759eab143ca7898577383dee8e4527154938f03afb6e9ef -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastRadiusCutoff 3e8bf9b90e3d36b16b0aed90390cb338b6c36cfce231c6c53c510e779e343534 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx e8f4d09b4747694a706164058108f23d6052f466fb9948d7e8857880636521c5 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts 7e3eaba8ad83b4ead4c23ab2896c4f4b80d8df5e82c284d6d01d22f9f5939015 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentMaxError 6d52d190eb24ab50dd9ba7560a6f901c53b851f8fa55ae12e3ff7690bd1341b1 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ClipRectFullscreen 173d0217aede4215e8ce1bf782f2bb63d39715492bc3f5952e8f96bc71008d07 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.Context a9ca546b24946c9b745e8a16da411db3900b9da67f47649a8e3b09660abde8d1 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CurveTessellationTol 4cfa2e512473438281943fe7dbf81498060992c38b628c603b23cd36d14d5b98 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.DrawLists d116c9e90b4b7a658ffa176b8069decda76ddc1e2d70c1315752a138f6dae5c1 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.Font 1aa61d1e10dd19276413c8a094c777447c55c49476581a07aa2cb4a52e106912 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontAtlas ef3be8108c30c3edf6aa32267023b40e16fdace71dd30720f2f75aca33eb9a42 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontScale 18e7df49b1bf40e478bd644040873af0aebe6e52a87f184e5852f9bd858e1683 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontSize 6988cb2315be316908084c58ca0f9d6deb83b4f5e157987254492d2cdc7b9ba4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.InitialFlags 29a961810ca9e23290ef070190cc163a6df99c7a31d75d691990f2eeae87de3a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.InitialFringeScale 9f755bbebafa5c19e06cc70101cabe514fcd6a42d9e286466be3d3caf1672c88 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TempBuffer f1826a6ec4f64f940ed4ace6247ca36e99add89158d814e3fb0ddcff6f241d0a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TexUvLines d62b88bcb39ea39f16ff4ecb20dd522ea0a6a8b4ba93372ed47de4db0fa00ad6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TexUvWhitePixel ef7bd3000da262b92b159800b30149e442016c0d1411cc831dc1b55e9f4c901f -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Channels 7a67965e2bab8f44cfc310d8e3aa751f0a329e495a4d8deea0f6e33ff6b3f799 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Count f70c6add03bb4804cc4d7494c164dccd2d02ef271a6ac55b659ea5b882680a63 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Current 87d8e83bdffd8bca61355e667caaa0a6ad73ff0dd6658742747dc308a66db424 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.CpuFineClip 364901fb497d84a240581626259dc962db4fc9cdd4ff4f91e40afab45f020004 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.None cfd46bc7d68f49cf776916f02fc9e4c392e61cea6747dc221bce7b7b2d498613 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.StopOnNewLine aa87926922aa05ee539e24539e4004002341fffcb9eaa81755475d4f575dfe1d -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.WrapKeepBlanks 3dd997bc9ed9e52299950e2301cf17a650c4bfceb0b6020125e779655a104bf3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Col 4b2f83bf9a3a011b27c432f1f8aa9642adaa14cfcd56a6cff249fd8f2e49634f -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Pos 81a9d9d03ea25c28d08c3a89a2733de60c1f9f3a1d96ed9bfd720f095ce2c24d -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Uv 338aaeed059984844fc7365ee2fe98caa4cbd34f1017772eba558220c215ecbc -->
<!-- api: F:Inno.Native.ImGui.ImFont.CurrentRasterizerDensity 29663c197b9c3aa5592bbb0c3d67c8255bafbedf5bb5f25bbd32420b49e59815 -->
<!-- api: F:Inno.Native.ImGui.ImFont.EllipsisAutoBake 2fbe23bbd4e2ca899a230de2e104f48facd2a67f578f72cee6943b85077e8f75 -->
<!-- api: F:Inno.Native.ImGui.ImFont.EllipsisChar 1ad14b88cf6778700ced4e1b79017a137f908029ac089178570415281769c967 -->
<!-- api: F:Inno.Native.ImGui.ImFont.FallbackChar f6780ca8afca07767060e12de70980144e7a950076e891f1ee929671c6090074 -->
<!-- api: F:Inno.Native.ImGui.ImFont.Flags b4e394a481f5d1d48952725303031010922501a9088b26a47515430c92845390 -->
<!-- api: F:Inno.Native.ImGui.ImFont.FontId eebfeaf827bda24d21d0437f4897a00806a5271bdc1fdc6ec872f459c175daf0 -->
<!-- api: F:Inno.Native.ImGui.ImFont.LastBaked 08343f617c8811be2134af8150d6983feb01628868825c3b9d5729392db05eb9 -->
<!-- api: F:Inno.Native.ImGui.ImFont.LegacySize f7c587380baadc9a4b66565a687f297794030dfca78030d7e4eea85502edf06d -->
<!-- api: F:Inno.Native.ImGui.ImFont.OwnerAtlas 53043cf662f318a13cb6e47cca4e05629ce33dd86d3b124d9b2dea76b2072f01 -->
<!-- api: F:Inno.Native.ImGui.ImFont.RemapPairs eed325e0a40354be33bdb3115202f960f47aea04e828430d1ab62b5afeea334e -->
<!-- api: F:Inno.Native.ImGui.ImFont.Sources aaccb2a3e0f1a95b0e27760e188e47f46a7619bb9f2147879f4ba97abb2ffe24 -->
<!-- api: F:Inno.Native.ImGui.ImFont.Used8kPagesMap 9f2fa59eed9f34e128024a458dc62434b4b938706e26316ab5d518da946c724b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Builder 03c6a1c758ae453bc97e34a5d203a7d14427c812d0d9b0bfc27708db7a8c4de1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.DrawListSharedDatas 646078c9142e8f4e6fba3d0e6e6cbaf66a8b2bd178826120f5c4e05f0afeacb0 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Flags 2dec374aba1f4f725a9bd762c54ea21bf9f8a750cfc47e7a79d3d934415fb1cd -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoader 63648859fafa3112ccc049a737ea947cbed6ebb38ad9390d0b0de5eee3050436 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderData 2631182e334994cb85b1190abdc660435876c990e034aa81fb1c33f6cce81b2e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderFlags b0114d2d37d696493738cc7f857f013c0946d9ce40c1035bcca08383fd988424 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderName 850cd52ef21d9965f4bad92859076d0dfc2efd23a302bbb869caa4bf0994f504 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontNextUniqueId 8c6cec8b356ae1408b7b034ea3c925aa3d2798f1ce625ae803c97ce735aeaf0a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Fonts 9a7e61be832938ae6723f378909af6f08771ac5b0e2068993112cf59dec1f090 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Locked 4c81206b272b07f14dbdde664fdaf974376b35fdc8618bc63db2ab817f7aed39 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.OwnerContext f7fa3192266641beb56a4aa7113e4a5d441bb9ed3bee7e7bf24c6f8c50e85580 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.RefCount be0e219b7e74a3d0b5ac83ec5248df11515da00aa99047f99540a0cc78e4fc07 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.RendererHasTextures 976ac194635735ce4883890ac3848fa71ec902bd92580f535f27dfd5acb6fd7a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Sources f72bfd8362bd930abfd6b3d52b53d93128e3c0a382e6077e0ab3377abb2a8933 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexData 7d2ebaf016fd440e22e779068d2d1b566e92a848ffeeb3d4656afba938e5339a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexDesiredFormat 68609f953fe8d84a3c450282f2d0899707d51f5c266e35dac10b49b071a6a711 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexGlyphPadding 31e81a4d21282de6f992be04538b22c7115e6ccd8644ebfe95b60bb7d203ae37 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexIsBuilt e48fce51db941a21141217fd7506f5fa40c806f2ab4335e99c2120bd20718359 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexList 989ab89059910fce8be60e4bacb8d5e735144e14f1527fb3b8a2f779865506cd -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMaxHeight f06ef3035e420ba7b687540c92663b3cf7550364f884a4c98ecfdc64cd00005a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMaxWidth e4736b760b875f9fa7364124c71c2dc31eef53dfff2b7fa831272acabcaaffe7 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMinHeight 4a1b6fb9d0ad4ad8c8b62bb8fb219874708d1d2bfa7515af3558794a9aa2c52d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMinWidth 118f140b621f3d91f54ce588a4cdb21f7e612cac8bfa67bd023aae4e2cae0af1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexNextUniqueId 856d1c8ff13dc04c2dcc5e49897bb7071e26917da9d92f111e5145bfb0e94f5c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexPixelsUseColors bdd1a2bdabcf0ed54a676dd9d1176b8f29eb8529ae0a28e7e4c0761a5de36598 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexRef 5433c5242cccce7b274ede23ae52d663ee39f0775d5e4008f58938d6f5347d1a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines 438510cf8ba9d6535ada8b81c3357f7e89e6fc45d05dabc5c32ba2978c76e79b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvScale 5ea94b3cd626cdfeaf5aac87d67966431fdc1cf1761654899e1296febf86f5cb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvWhitePixel e6d6bcdeae69a42572e94b4e4309715048fb3280ef4fe81caddba4ac8e12f81d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.UserData 1655f660941874ffa6a7eff345e1539bec0bc2b95717783087ba738214f9a185 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedDiscardedCount 54bdc8d1837d7bd74c796f8b9bb2fb7775d2472a3099e9049c8fdf4aac5c59ea -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedMap e40e2d38520900c7d4a2421c0ecaae67270a1ec83de0e63e38bfe68432766b57 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedPool fab1af88f7cf6744514b5ecd4e044639923dc6f9b3a1238ab994d4c6ec5cffcb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.FrameCount fc5716eecec27e1d9fcc76e6f314d4ff32ee7ffcc2305ff1a5dd524bad5b7d3f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.LockDisableResize 34b97c4736e1712266367b9d3bd760a696bfbc8a16a5cad92a3eb208cc5e9dbc -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectBounds 23395fbc01915b081dcbe6e7046441f7df1ff23dbf720a1dc504c0fdccaded0c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectSize 35217dc1413c09c02e920a3260cf463c42e4e9ba82dcaa48daf69480807661ba -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackContext 00699c02afffd6ea997ee906784854a8c46d28c327d41295fb23dafacec0d932 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackIdLinesTexData af03ec8b488f535bac71aa66bd47311897d24fc019152edfb6a1b498c363bad2 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackIdMouseCursors b2cd89fe606d850613808fa07ce684d9fea0d3bf50b4e533386a1a5bcdf867b9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackNodes a4c3bc16153ec2462c62c80d97e66a27c05d5661b7ae3b7eb0baab8c7f7943a4 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PreloadedAllGlyphsRanges 544b686626cc29f84a96463b529509a84b26665ca84f192300c2f1604e406e27 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.Rects 1c5d7eabb0df4f4712080825d8bc7e068f430f6accb6848e2fa28f53ac7d4ce3 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedCount 7dce6371ac69bf0b214ebeb35ee42cd51904cdae05cf4de7eac4bff4eb6bf15f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedSurface 1366686366f94123eaded6c5303423dfe4d0950aa3eb1c8dd4f72249bcaad97c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndex 444a542eed7e4e0e885ca6547b5e7e993229376b7ec2d3c7da0625b050191637 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndexFreeListStart 927eddeb609de0a03e70b43a3dd52ff297a8f007250274110476df8bc4dade17 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedCount 626bdbd24e25f43d8aba7d86e5c3aacb667ee23701a1a8694841e0ac019ced83 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedSurface 4165df6429c2172a97730d6642788b22701c1b566da0a5eca6cac96880b8ae8a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.TempBuffer 8e0b6504bc8e789dbff4c6326af64d473983282a25f9a6d70d5eab4a4ec82d16 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoBakedLines 197db3f8b9ada349175ae73eb8783dda4c43044a7631ca6f03b264775bc86348 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoMouseCursors 96e35b0a2d640a6aee0eca8c0393545c3a96711913a0a410d3edd99dd8297e84 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoPowerOfTwoHeight 25ae178c01c9a72b19ac606b9155b04b82afd7e9991d741291560a32cc0651aa -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.None dedfa9fd8f313d4fed97e2fe306cb1a25409b7558daf81f86f258889ffff23a6 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Font 728b9b37d2974dda005600828b81a6bff3b83648561a75c3bfbee93aeb9c447b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontAtlas ad9c8db07c92c840a4dbc1a3b0f6a2849ea8964bb9544b9d9b6702fa4872c2f1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontBaked 83b78ac7a020256faf2a8991032ca364fc6ac18699f29b18453164e71de465f9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontSrc fe5c44181c057294c4d0ea1c4bdbc6b4c6f9f1ff74a708d173328c9c82578779 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Format 27c1db2d618b6992dced2f49de9077518de2726ad1b551b7984d4627bdc25978 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Glyph 038e439d55efa2ec333466fbb1a2797298a6102c502c715c8df9d227e6c0a0a9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Height 1445ead71e4576203cc59627ac73f6f0b0e0d3399a377a14648f7702b38f1484 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Pitch 7ed87da8bca8803823b127d00358116aeebebaceae5c9300f6efff0408161789 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Pixels 638c5c9179278faf2f7ab2548a07c4a13f273a0ef39084fd2dbea86b66297c43 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Width b53d3ec3be57cd8fb74dd87c709ae19f397967ccaba98d0c76d057bd1ae947c1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.H 8f68f2babcb5cbf7680092eb896ffd753b55dc00312feb217743e741e7a2b300 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Uv0 acb3c509355342abbb6fe564290297a925551a619a2090c7bfffd58a2dcb0e34 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Uv1 f73ca86f72d948554c025b67ed70db3d1b80c977cb23c2d58ccc7a7e992e37c6 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.W 87e1a50610702c95be8dfdb5dcb37a5538ae58fddd8ff9b1b84fb23bde757ea2 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.X c3945ea410feb779a86402599b62004e558d2db2179975449d8fbfff795c82fc -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Y 2fd419020c7fd41daecaced157cdd4e5e8e900b23fedd0383274f5031d011038 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Ascent 8d5b516dae8ec7321b1606deb4c0e404c86c222f810cdf927fb79fcd358204ae -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.BakedId f45afa01747d2e4a48dbe823aff1826279a62a735af4f32ab5d33579a2e7cd3b -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Descent 1eac196988aeb7f4f49be9da36e2e844e149639504c6468eb8c49377ac2abd0b -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FallbackAdvanceX 5516f07415ebc4fb2ffd629593fb83ceef86e21ed3b068bbcbdab074ab78559f -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FallbackGlyphIndex e55e16abf07fdf2d53dc361de6e5ac5458bd1eb82a0f11ac65eedf693307aa87 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FontLoaderDatas c115110510dbd155b7eb6826e27ecf06238a23a2c2700b98c905f25b4805675e -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Glyphs d480a5a4d4b82fe3c74abec7889372330ebad63ba419bd9f03b49a6f3ee21060 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.IndexAdvanceX baa77497888ed1fc25672676bfc739ebbfb2c4fd28d697568976b7ec13e8ab0a -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.IndexLookup 70f62fde1d0577a7bc1e61ef9e3f1a304b1514001e87730ef9c57b8b3e0b331f -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.LastUsedFrame ef74237cf67a9f0bc99e52e841f6bd0e1e12a2aa0e0ae61010769e8a6b242bc6 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.OwnerFont cb1ea5a8cb17c75ebfb3eb2fc5a48d62df6f363b9ad3cb41203f02ae9cc04f02 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RasterizerDensity 1c721fec00f19a735a31dc41ebfa40d86978fa40a7f039ac4e62556373784e05 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Size 9a72ef1a0a7cf90d90166d2016503ed6e02f26df26210e91b2270a5087bd8d99 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.DstFont d268e4bb907a4450bc849236ca602dc91b497243f0d9b0f97eb4379b1b93c23d -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.EllipsisChar e6892a28797e83829ed0c81991a7b08dcc6c2a5ae0c9d5c3eb853a74e557baf5 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.ExtraSizeScale 437db4b9236b2b7277110b7d2a77467219fabd95f550e064688bce5a7d0d73fa -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Flags 089c22a55ef35b0a38c5e9c8d40c8622848e80a80e6c94a7a8e29069492ddda0 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontData 8bc14e762f59e225d11656df3ee54839b16401366d5ce510f4d54fbc596486ca -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontDataOwnedByAtlas ccba5b455a890ce595e02a5fa576b2cbb9685910c64ccf312c9524ac0dd7e41f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontDataSize c2685aa3aed4e25d1960b207ecd685145bdd441e5d3cef388cdad2a29677ad2f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoader 4315784b8dc0012e82178b622969c8a025143139cf46114c82b92177da33d8d1 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoaderData d54e08a889dfd441ab590fea00e30a168db9f09855ba45643806924095a3d5ee -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoaderFlags 80a60d892c058d0e25c01f8ab6a3d43d3bad36574e9744d2dfc8cb30bf7ab6ab -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontNo 304de142e7de9b412fb00d09c1fdf59a64badb99aededc1341ef656676f65f77 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphExcludeRanges 0b977aabb4fcd601f5479743871bb8d0cd6d4a18499d91d8f1ba747aa6e89cea -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphExtraAdvanceX 47af0f7333f08e9b2475c01449a3caf5689acc55aaf21d57b685d20c018b7339 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphMaxAdvanceX 63fec218032f20a94881b863a7cf61daef46f1e281745c1c21aaa1e4068afe3d -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphMinAdvanceX 60ac1b73702ff5b0cacd586b7eb4340d1becd5641832bad55db44a1b4a4522b7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphOffset 7f01cf4900cbbd834f58d40e7a9d90ba7da2028f5b9fd0091627e28b376caf25 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphRanges 252543dc0fd3519ce1beed9ba9fb8c3f737c67e47ea1eab247d5bc4fd9b08bbb -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.MergeMode 58035bee83a8b66a5903bad5facaff4f9f42219aa49ea409a96b1897f8fb9566 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name 1344d9385c4c0f5ef7167b04ae05141a29fc1c4feb372ed2288611cfd58ab899 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.OversampleH 3e8ac1084bbaa81013a1cd4ca752203be06747ecf4ce37a6426fb34137555944 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.OversampleV 4db4be8d51f8c3b6ddd334b7a9f9789a489e906798526a7599d0c449f8dbc114 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.PixelSnapH 61156fe7d73c2cead26c75f3b9215f94339272cd4bc047d589b80d149b0a16a7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.RasterizerDensity d5a19d17e18b572eae8ed46af135fc73960c4f647562cacefea4270df32b793f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.RasterizerMultiply 954c87aac86da7741712d5e89f3dbea548f51d3498de170597d9cd206f736afa -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.SizePixels 9f8d0be4885029f80be17892c07330c69bed4e1028fe14024ce66738bc8b1f71 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.LockBakedSizes 17fbef4eaee5ebe2b2b391e6d7ab6b6a08f1ba3ee1ba6bc1302f228a028c1573 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.NoLoadError 8d1c5cf5d7c959997af2ced45d0adc5ee265d66c184f5ebfb47bb1bbe783f8f8 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.NoLoadGlyphs 2cb8514cc87d36879db54c81e51d9e52e0399b280efb8de7e984a670a7f571c0 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.None aa2af6644aa8b4cd7a34ec9bd89a67108639b91bcbbbbb316e279be815274906 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.AdvanceX 3fe8ee80580152bd152938af09d51a8566d558363b2817a0aa3f7d740764dba9 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.PackId 36e79c69011dff8870a3eb2ad05b72d020e739b5a85764898370397b0aa05584 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.U0 cbe169fd778e0ce80216c92313d8d2c8e45b623ca534212014045a16d5aa67f6 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.U1 ceeca41e605d2a784346fb8f0286ef421322bf43a49dac1489028e8978d3bc6b -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.V0 6fa4d011a83fdaf4e1ca82f99808678c32938a8066b9d0dae5a33c069e235181 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.V1 bc576e043d3653ca9cea0a46be84ff7da055e1d27a5939070c70af08f3933d50 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.X0 83700d5c0fa4a70b219f3ed71c99111e7169922165126f80a76d3f90d2847e53 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.X1 31580d9c0c92da85099900e25f916095dbee8952258692f88de2418a1b25645c -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.Y0 c8b5e8d34a8bf5688a88c7f34d8cf24fbf271ec97bac54ca18bf762d417bc59a -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.Y1 ffa8854969811b4f421948a0deba5991f45cec8ec1e068338fe3a549ed18c777 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-61aed3054a9af2b5"></a>[`public Inno.Native.ImGui.NativeArray_3325e8ed3a9a98ba Inno.Native.ImGui.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L23959) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-311326315192ceba"></a>[`public Inno.Native.ImGui.ImVectorImU32 Inno.Native.ImGui.ImBitVector.Storage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L23977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3a1d5cf54a2ecbe"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImChunkStreamImGuiTableSettings.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L23995) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7a5e240503f972a"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImChunkStreamImGuiWindowSettings.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81ff39e1a32f9828"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImColor.Value`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ad18bc6ca25a3a6"></a>[`public Inno.Native.ImGui.ImVectorImDrawCmd Inno.Native.ImGui.ImDrawChannel.CmdBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c547171d7f2a30d8"></a>[`public Inno.Native.ImGui.ImVectorImDrawIdx Inno.Native.ImGui.ImDrawChannel.IdxBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee3f123e06f19265"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImDrawCmd.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2502e72463b0cb3"></a>[`public uint Inno.Native.ImGui.ImDrawCmd.ElemCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24083) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-875a4a0e93f7b8e4"></a>[`public uint Inno.Native.ImGui.ImDrawCmd.IdxOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91fa1fe2c715b7ef"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImDrawCmd.TexRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb7ff46c1c702276"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImDrawList*, Inno.Native.ImGui.ImDrawCmd*, void> Inno.Native.ImGui.ImDrawCmd.UserCallback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24085) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae1d96fc34fe7d12"></a>[`public void* Inno.Native.ImGui.ImDrawCmd.UserCallbackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24087) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d57f32def488f0b"></a>[`public int Inno.Native.ImGui.ImDrawCmd.UserCallbackDataOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f45af73d294c410d"></a>[`public int Inno.Native.ImGui.ImDrawCmd.UserCallbackDataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8133a9bb1c545cba"></a>[`public uint Inno.Native.ImGui.ImDrawCmd.VtxOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b2e5c4cc4cafe979"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImDrawCmdHeader.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24109) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92f6329e50db3d98"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImDrawCmdHeader.TexRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24111) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b50b6e84474f0eeb"></a>[`public uint Inno.Native.ImGui.ImDrawCmdHeader.VtxOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24113) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-deef9ef90218a9be"></a>[`public Inno.Native.ImGui.ImVectorImDrawListPtr Inno.Native.ImGui.ImDrawData.CmdLists`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24139) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81274aa4bbd85422"></a>[`public int Inno.Native.ImGui.ImDrawData.CmdListsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c255d6790f2d697"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawData.DisplayPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24141) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c64ffc98e1e2a5c"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawData.DisplaySize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24143) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2e154395f336481"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawData.FramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24145) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13963b782dcd25e1"></a>[`public Inno.Native.ImGui.ImGuiViewport* Inno.Native.ImGui.ImDrawData.OwnerViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24147) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da246874594d82f1"></a>[`public Inno.Native.ImGui.ImVectorImTextureDataPtr* Inno.Native.ImGui.ImDrawData.Textures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24149) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-074ebdf6c5646208"></a>[`public int Inno.Native.ImGui.ImDrawData.TotalIdxCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24135) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea047ef618e0215a"></a>[`public int Inno.Native.ImGui.ImDrawData.TotalVtxCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24137) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87d1773ce0a2b5e9"></a>[`public byte Inno.Native.ImGui.ImDrawData.Valid`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7d9d18f6872cf57"></a>[`public Inno.Native.ImGui.ImVectorImDrawListPtr Inno.Native.ImGui.ImDrawDataBuilder.LayerData1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24169) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6ca6aa05cf6b918"></a>[`public Inno.Native.ImGui.NativeArray_7b9f5e5665cb58c5 Inno.Native.ImGui.ImDrawDataBuilder.Layers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24167) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b4d38bd44da2d7b"></a>[`Inno.Native.ImGui.ImDrawFlags.Closed = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24185) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-17999aba55b9dad3"></a>[`Inno.Native.ImGui.ImDrawFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24184) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-bd439ba8ecf00e37"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersAll = 240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`240` |
| <a id="api-b67cf1cb3ef5c4d2"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersBottom = 192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24192) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`192` |
| <a id="api-fe1e8c81007a9a4c"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersBottomLeft = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24188) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-f067d1dbc5d8c87b"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersBottomRight = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-22bf968e263f4dca"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersDefault = 240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24196) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`240` |
| <a id="api-e1397bccbfc6cf99"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersLeft = 80`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24193) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`80` |
| <a id="api-ea5900aac09f8058"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersMask = 496`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24197) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`496` |
| <a id="api-0ec6495ffea4dda2"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersNone = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24190) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-a2effcb17df4c55a"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersRight = 160`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24194) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`160` |
| <a id="api-a64b9c6ba0432fbd"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersTop = 48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24191) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`48` |
| <a id="api-62c7f1d6c53e1688"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersTopLeft = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24186) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-ce6a7cc504b69031"></a>[`Inno.Native.ImGui.ImDrawFlags.RoundCornersTopRight = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-3629b614f5df9f30"></a>[`public Inno.Native.ImGui.ImVectorImU8 Inno.Native.ImGui.ImDrawList.CallbacksDataBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24234) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8a2a8ce2eff083f"></a>[`public Inno.Native.ImGui.ImVectorImVec4 Inno.Native.ImGui.ImDrawList.ClipRectStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24230) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-34925c68fb221d13"></a>[`public Inno.Native.ImGui.ImVectorImDrawCmd Inno.Native.ImGui.ImDrawList.CmdBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24208) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f3d33bb0f49226db"></a>[`public Inno.Native.ImGui.ImDrawCmdHeader Inno.Native.ImGui.ImDrawList.CmdHeader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24226) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d75a39dbaf7ba28"></a>[`public Inno.Native.ImGui.ImDrawListSharedData* Inno.Native.ImGui.ImDrawList.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24218) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-564ee8c0de84b434"></a>[`public int Inno.Native.ImGui.ImDrawList.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24214) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39866311abd9a37d"></a>[`public float Inno.Native.ImGui.ImDrawList.FringeScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24236) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1570466ae51a1d9"></a>[`public Inno.Native.ImGui.ImVectorImDrawIdx Inno.Native.ImGui.ImDrawList.IdxBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24210) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fc7b03a455abebd"></a>[`public ushort* Inno.Native.ImGui.ImDrawList.IdxWritePtr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ce9afae0de2fa14"></a>[`public sbyte* Inno.Native.ImGui.ImDrawList.OwnerName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ebe8b20e2f09561c"></a>[`public Inno.Native.ImGui.ImVectorImVec2 Inno.Native.ImGui.ImDrawList.Path`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b819d02296dfd691"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImDrawList.Splitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24228) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9613b327b335d9dd"></a>[`public Inno.Native.ImGui.ImVectorImTextureRef Inno.Native.ImGui.ImDrawList.TextureStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24232) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6647fe4698c2ebd3"></a>[`public Inno.Native.ImGui.ImVectorImDrawVert Inno.Native.ImGui.ImDrawList.VtxBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-38676720edb4211c"></a>[`public uint Inno.Native.ImGui.ImDrawList.VtxCurrentIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24216) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98d4fbe65ced9210"></a>[`public Inno.Native.ImGui.ImDrawVert* Inno.Native.ImGui.ImDrawList.VtxWritePtr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24220) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43af850c7fd833f9"></a>[`Inno.Native.ImGui.ImDrawListFlags.AllowVtxOffset = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24257) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-5d951c92df04e09d"></a>[`Inno.Native.ImGui.ImDrawListFlags.AntiAliasedFill = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-44fa07b40b4f50ad"></a>[`Inno.Native.ImGui.ImDrawListFlags.AntiAliasedLines = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-ff286c7f09c20286"></a>[`Inno.Native.ImGui.ImDrawListFlags.AntiAliasedLinesUseTex = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24255) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ff01a28ee8e24f18"></a>[`Inno.Native.ImGui.ImDrawListFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-70d959d094e448a1"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.ArcFastRadiusCutoff`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24298) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-525ae74d699737dc"></a>[`public Inno.Native.ImGui.NativeArray_f3ea9f13691638c5 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24296) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ca55286a0785d46"></a>[`public Inno.Native.ImGui.NativeArray_332d40ffc044d296 Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1ece1c7ed540868"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentMaxError`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-55f74dd87949fdf3"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImDrawListSharedData.ClipRectFullscreen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24288) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ef8d61824e051fa"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImDrawListSharedData.Context`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0bbfb3c415dc779"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.CurveTessellationTol`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85fc0832070db3d9"></a>[`public Inno.Native.ImGui.ImVectorImDrawListPtr Inno.Native.ImGui.ImDrawListSharedData.DrawLists`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e28f1de758901a49"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImDrawListSharedData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24274) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-990b844d157eae0f"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImDrawListSharedData.FontAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24272) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02afdfac2de93b9b"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.FontScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24278) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00682ca2aa4e2596"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.FontSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24276) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58033bec04e705ca"></a>[`public int Inno.Native.ImGui.ImDrawListSharedData.InitialFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bc704503688c95a"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.InitialFringeScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33e6e747fc42d009"></a>[`public Inno.Native.ImGui.ImVectorImVec2 Inno.Native.ImGui.ImDrawListSharedData.TempBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c032d7e5ed4aa9f8"></a>[`public Inno.Native.ImGui.ImVec4C* Inno.Native.ImGui.ImDrawListSharedData.TexUvLines`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c70b1b12948e17fb"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawListSharedData.TexUvWhitePixel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d679e8adcb67106a"></a>[`public Inno.Native.ImGui.ImVectorImDrawChannel Inno.Native.ImGui.ImDrawListSplitter.Channels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c692b8bafdbca77d"></a>[`public int Inno.Native.ImGui.ImDrawListSplitter.Count`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24320) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90e8e3244f6c66cd"></a>[`public int Inno.Native.ImGui.ImDrawListSplitter.Current`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24318) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f0a580f1d79bf67"></a>[`Inno.Native.ImGui.ImDrawTextFlags.CpuFineClip = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24338) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-a2cdcacdbb9f1acf"></a>[`Inno.Native.ImGui.ImDrawTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-84279497451aba60"></a>[`Inno.Native.ImGui.ImDrawTextFlags.StopOnNewLine = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24340) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-f47b677d2dae9d7c"></a>[`Inno.Native.ImGui.ImDrawTextFlags.WrapKeepBlanks = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ababaa11667571ab"></a>[`public uint Inno.Native.ImGui.ImDrawVert.Col`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24355) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ff55358637c6aad"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawVert.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abb5a110093083ea"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImDrawVert.Uv`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24353) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0e1467f3f3d2595"></a>[`public float Inno.Native.ImGui.ImFont.CurrentRasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c37acf863225b28"></a>[`public byte Inno.Native.ImGui.ImFont.EllipsisAutoBake`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8c439be98270dd5"></a>[`public ushort Inno.Native.ImGui.ImFont.EllipsisChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24387) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecd67340ae6c1e72"></a>[`public ushort Inno.Native.ImGui.ImFont.FallbackChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bca918b10c7ee1c1"></a>[`public int Inno.Native.ImGui.ImFont.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24377) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e5f1ddad2108110"></a>[`public uint Inno.Native.ImGui.ImFont.FontId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8315c6bbcedcf3e8"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFont.LastBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4241ac17fa103b32"></a>[`public float Inno.Native.ImGui.ImFont.LegacySize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b738799a9e758f7"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImFont.OwnerAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-848cdc7ef4d8f482"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFont.RemapPairs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dd6b35db6796b6f"></a>[`public Inno.Native.ImGui.ImVectorImFontConfigPtr Inno.Native.ImGui.ImFont.Sources`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a0122a3c43f0b90"></a>[`public Inno.Native.ImGui.NativeArray_41ea187b8e0849de Inno.Native.ImGui.ImFont.Used8kPagesMap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-de2901bcf77d614a"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilder* Inno.Native.ImGui.ImFontAtlas.Builder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24459) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e84339005d84a743"></a>[`public Inno.Native.ImGui.ImVectorImDrawListSharedDataPtr Inno.Native.ImGui.ImFontAtlas.DrawListSharedDatas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78fd5a8afaa3ee20"></a>[`public int Inno.Native.ImGui.ImFontAtlas.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24413) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b46e8ffd03f85a8"></a>[`public Inno.Native.ImGui.ImFontLoader* Inno.Native.ImGui.ImFontAtlas.FontLoader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24461) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89bd886b7c764b9f"></a>[`public void* Inno.Native.ImGui.ImFontAtlas.FontLoaderData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-249104438b88ac58"></a>[`public uint Inno.Native.ImGui.ImFontAtlas.FontLoaderFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24467) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bddb020f43f017f0"></a>[`public sbyte* Inno.Native.ImGui.ImFontAtlas.FontLoaderName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7947d17c9422a081"></a>[`public int Inno.Native.ImGui.ImFontAtlas.FontNextUniqueId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba157f7dbf01aed3"></a>[`public Inno.Native.ImGui.ImVectorImFontPtr Inno.Native.ImGui.ImFontAtlas.Fonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f55b1ce33d949351"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.Locked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8b52fdea076daa4"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImFontAtlas.OwnerContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42b323d7556da327"></a>[`public int Inno.Native.ImGui.ImFontAtlas.RefCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-687a405fc041a0c6"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.RendererHasTextures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f801dc9e34833b46"></a>[`public Inno.Native.ImGui.ImVectorImFontConfig Inno.Native.ImGui.ImFontAtlas.Sources`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-47b7081a1c8e7181"></a>[`public Inno.Native.ImGui.ImTextureData* Inno.Native.ImGui.ImFontAtlas.TexData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24431) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b167617c55317b7b"></a>[`public Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlas.TexDesiredFormat`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24415) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7ea411791d36958"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexGlyphPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24417) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dd1a7418202092e"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.TexIsBuilt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f42e0a7b5c55525"></a>[`public Inno.Native.ImGui.ImVectorImTextureDataPtr Inno.Native.ImGui.ImFontAtlas.TexList`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24433) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9a5e256cc8fd27eb"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMaxHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89e6a9368686ed43"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMaxWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67c4b03d78dd6ec9"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMinHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-395d4f3bae13de50"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMinWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07a3c08a7d254618"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexNextUniqueId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a00649b4874841bb"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.TexPixelsUseColors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3b3434f91d8a564"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImFontAtlas.TexRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9e2846321eb9609"></a>[`public Inno.Native.ImGui.NativeArray_43db0ce246f18b3f Inno.Native.ImGui.ImFontAtlas.TexUvLines`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65f5c22578222698"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImFontAtlas.TexUvScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8899bc72ce47d823"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImFontAtlas.TexUvWhitePixel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd767d91e5790458"></a>[`public void* Inno.Native.ImGui.ImFontAtlas.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24427) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07c827fa1df1d0e7"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.BakedDiscardedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3faf269dfd35bcec"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFontAtlasBuilder.BakedMap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf1881d7a18b8d2f"></a>[`public Inno.Native.ImGui.ImStableVectorImFontBaked32 Inno.Native.ImGui.ImFontAtlasBuilder.BakedPool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d05f5b778937bf38"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4055ca66eec31054"></a>[`public byte Inno.Native.ImGui.ImFontAtlasBuilder.LockDisableResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c39e7043d476c10c"></a>[`public Inno.Native.ImGui.ImVec2iC Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectBounds`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3238120006c20c81"></a>[`public Inno.Native.ImGui.ImVec2iC Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5756f3300af7c602"></a>[`public Inno.Native.ImGui.StbrpContextOpaque Inno.Native.ImGui.ImFontAtlasBuilder.PackContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01690bdb8728dd49"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.PackIdLinesTexData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-96490c8e8f3d9dac"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.PackIdMouseCursors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17d9fe15747236cd"></a>[`public Inno.Native.ImGui.ImVectorStbrpNodeIm Inno.Native.ImGui.ImFontAtlasBuilder.PackNodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24491) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5b0e00ddef13d681"></a>[`public byte Inno.Native.ImGui.ImFontAtlasBuilder.PreloadedAllGlyphsRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cab3df087ccd4cd8"></a>[`public Inno.Native.ImGui.ImVectorImTextureRect Inno.Native.ImGui.ImFontAtlasBuilder.Rects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e661214496fb7da"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24505) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b94b58ef6679ebc"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24507) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39be156881fa640b"></a>[`public Inno.Native.ImGui.ImVectorImFontAtlasRectEntry Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-867057279c285ced"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndexFreeListStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ab76ffcf0c470133"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-977c3854f60cd7ee"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-401799921c5db2ff"></a>[`public Inno.Native.ImGui.ImVectorUnsignedChar Inno.Native.ImGui.ImFontAtlasBuilder.TempBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-660e24a9a7b2356f"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoBakedLines = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24545) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-a58f023553ef7d1c"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoMouseCursors = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ca7436ae0a1085cc"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoPowerOfTwoHeight = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24543) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-34206012bf420d9e"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-37d2812778aa7b14"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlasPostProcessData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3118f15b10196051"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fe019e031713259"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f55a3b944cd6aec1"></a>[`public Inno.Native.ImGui.ImFontConfig* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontSrc`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abe4c82f8f0dc97b"></a>[`public Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlasPostProcessData.Format`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef7754b3608f3925"></a>[`public Inno.Native.ImGui.ImFontGlyph* Inno.Native.ImGui.ImFontAtlasPostProcessData.Glyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48146103cae9f161"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Height`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e63da786d675ef6"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Pitch`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c27accfebfb5da2"></a>[`public void* Inno.Native.ImGui.ImFontAtlasPostProcessData.Pixels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b35e74a5b3095c6"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5356290a16640ef2"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.H`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b7a084e068c0bf9e"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImFontAtlasRect.Uv0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-520270eaf53dee74"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImFontAtlasRect.Uv1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24602) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-399006c4063b7052"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.W`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24596) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72dfe8419eef5ca1"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49efae5970b2f601"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.Y`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24594) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7555e23a9d2c8947"></a>[`public float Inno.Native.ImGui.ImFontBaked.Ascent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24742) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76ddcfbd5cb2c275"></a>[`public uint Inno.Native.ImGui.ImFontBaked.BakedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24748) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f8b6beca2090bcc1"></a>[`public float Inno.Native.ImGui.ImFontBaked.Descent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-160255c6ea182c2d"></a>[`public float Inno.Native.ImGui.ImFontBaked.FallbackAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aba9ee0759107ff9"></a>[`public int Inno.Native.ImGui.ImFontBaked.FallbackGlyphIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-94ae5d2351abd3cd"></a>[`public void* Inno.Native.ImGui.ImFontBaked.FontLoaderDatas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24752) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2841290088bf018"></a>[`public Inno.Native.ImGui.ImVectorImFontGlyph Inno.Native.ImGui.ImFontBaked.Glyphs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-258446819a73c411"></a>[`public Inno.Native.ImGui.ImVectorFloat Inno.Native.ImGui.ImFontBaked.IndexAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-945db06e5cd7d929"></a>[`public Inno.Native.ImGui.ImVectorImU16 Inno.Native.ImGui.ImFontBaked.IndexLookup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6c5393effa41fc0"></a>[`public int Inno.Native.ImGui.ImFontBaked.LastUsedFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24746) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac0cf908b904f0ae"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontBaked.OwnerFont`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24750) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e11fd1e9c5e12d4"></a>[`public float Inno.Native.ImGui.ImFontBaked.RasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4e834592728a496"></a>[`public float Inno.Native.ImGui.ImFontBaked.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7a782a852fd8df2"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontConfig.DstFont`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24934) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e124191f544ed040"></a>[`public ushort Inno.Native.ImGui.ImFontConfig.EllipsisChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24906) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c821cd35c0297253"></a>[`public float Inno.Native.ImGui.ImFontConfig.ExtraSizeScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24930) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4b17a3fff70e9a4"></a>[`public int Inno.Native.ImGui.ImFontConfig.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24932) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a3adf85e179cfe2"></a>[`public void* Inno.Native.ImGui.ImFontConfig.FontData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24892) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ee48a63074d3d5ef"></a>[`public byte Inno.Native.ImGui.ImFontConfig.FontDataOwnedByAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24896) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b0ee7fca47abcd77"></a>[`public int Inno.Native.ImGui.ImFontConfig.FontDataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24894) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-573335bd0068963b"></a>[`public Inno.Native.ImGui.ImFontLoader* Inno.Native.ImGui.ImFontConfig.FontLoader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24936) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-16ac50676bd181ae"></a>[`public void* Inno.Native.ImGui.ImFontConfig.FontLoaderData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24938) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ce0cfa96d1f2e77"></a>[`public uint Inno.Native.ImGui.ImFontConfig.FontLoaderFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24924) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d12400559f1828aa"></a>[`public uint Inno.Native.ImGui.ImFontConfig.FontNo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24922) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-039e88813ecd0afb"></a>[`public ushort* Inno.Native.ImGui.ImFontConfig.GlyphExcludeRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24912) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90324cfee3b31e9c"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphExtraAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24920) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2bcbe73f40768c30"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphMaxAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24918) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-406a7164ad2ffa5f"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphMinAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24916) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c0251835e77feb0"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImFontConfig.GlyphOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24914) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10211709913ba67e"></a>[`public ushort* Inno.Native.ImGui.ImFontConfig.GlyphRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98fdc2f1ba2abfc2"></a>[`public byte Inno.Native.ImGui.ImFontConfig.MergeMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24898) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8514730edffeefb1"></a>[`public Inno.Native.ImGui.NativeArray_d36642d3104a6c6f Inno.Native.ImGui.ImFontConfig.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24890) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f9a5ee08aa733b6"></a>[`public sbyte Inno.Native.ImGui.ImFontConfig.OversampleH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-caae6c2d960239b5"></a>[`public sbyte Inno.Native.ImGui.ImFontConfig.OversampleV`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24904) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-73634d547402eee6"></a>[`public byte Inno.Native.ImGui.ImFontConfig.PixelSnapH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24900) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb205bf404ccd87e"></a>[`public float Inno.Native.ImGui.ImFontConfig.RasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24928) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b6d581e03caffac"></a>[`public float Inno.Native.ImGui.ImFontConfig.RasterizerMultiply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24926) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cab5c1423b74c83e"></a>[`public float Inno.Native.ImGui.ImFontConfig.SizePixels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24908) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b7c644d2816048e"></a>[`Inno.Native.ImGui.ImFontFlags.LockBakedSizes = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-2b0167e30546066c"></a>[`Inno.Native.ImGui.ImFontFlags.NoLoadError = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24954) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-b059ef7f97762d98"></a>[`Inno.Native.ImGui.ImFontFlags.NoLoadGlyphs = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24955) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-8272abf84b30e3ae"></a>[`Inno.Native.ImGui.ImFontFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-a9e6287fe720a977"></a>[`public float Inno.Native.ImGui.ImFontGlyph.AdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24969) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e76fe05f547912e2"></a>[`public int Inno.Native.ImGui.ImFontGlyph.PackId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24987) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7f48b3244b71961"></a>[`public float Inno.Native.ImGui.ImFontGlyph.U0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37b10ac68ec03c4b"></a>[`public float Inno.Native.ImGui.ImFontGlyph.U1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc741280795e74fd"></a>[`public float Inno.Native.ImGui.ImFontGlyph.V0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9c3a761bf886477"></a>[`public float Inno.Native.ImGui.ImFontGlyph.V1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e128d0e6f42edf4c"></a>[`public float Inno.Native.ImGui.ImFontGlyph.X0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24971) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35fb168aac94c4f7"></a>[`public float Inno.Native.ImGui.ImFontGlyph.X1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24975) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-38669eb8bff6325a"></a>[`public float Inno.Native.ImGui.ImFontGlyph.Y0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24973) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-adf9b0f054122c32"></a>[`public float Inno.Native.ImGui.ImFontGlyph.Y1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L24977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
