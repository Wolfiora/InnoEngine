# Inno.Native.ImGui ABI · 02

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImFontGlyphRangesBuilder.UsedChars 708980ced06720a2bf484999eca95b25bef6641a62ccbd0a167d6917f630049c -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedDestroy 90946ec5a02a66c7d68c676e0cf06539551e9969b5386ef37ce87b474dc7488c -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedInit af34fe0a926c4cd3eacf83b7c314d98803d7897abf21a064411b9259447bd26a -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedLoadGlyph a4ec00b46fbf3b8002fb7802d55ffd5f094512c7df5b09ae6d5592f335af81b0 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedSrcLoaderDataSize f6a4de28d1e30ce55b2a9f8deab0d0091020df87ecf674802165abc90f7d6642 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcContainsGlyph 9f2a8fc4292fa3fc90b762b6bde6af1eeaeeac9318d9fcb393525a007a547607 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcDestroy b024ceb8ae9625e26aaf5bccd49687b2f8189057736f88c31c5994e624863a36 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcInit 7689742be49c884b07fa6beec8da98205a6079606af028ad8c3baf2720a9c421 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.LoaderInit 890ea7e105a3e30de7c102158d5c122233435e4756d7b4cf91cef4d3544d53b8 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.LoaderShutdown e9f13dfc74f17741f67eef169889fe8fd663e8d7b11e904936dad1b0f5fdb12a -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.Name 40867bd679aa79c292247980b40a5f5c11a067e2c8381f6fb3343eff6fcd76a7 -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.Font 5eca0d0f8c67d0aae7fd80edc8832ca31ffb91ba490ac67da7b32dab3c1e9bba -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.FontSizeAfterScaling e88f4bba7006e2e51e0255475fe032c04328c45c6e70a927ea306c0fb5ca4ac9 -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.FontSizeBeforeScaling b398baf4b6e6c725ef4e73b781102cf59dcb3124103c1d5d79b3e02951f8b79c -->
<!-- api: F:Inno.Native.ImGui.ImGui.Constants.IMGUIHASDOCK bd75ed636ad18675ee36c5ea842f7802315c14d5e9b2d94a9c443b58b9b02f68 -->
<!-- api: F:Inno.Native.ImGui.ImGui.Constants.IMUNICODECODEPOINTMAX d2296c26f9425aa29d4147affa2b46571764a4974a1b66f7b398d13956e6668b -->
<!-- api: F:Inno.Native.ImGui.ImGui.Constants.ImTextureIDInvalid 918eeeec705f33b5fc81be70a96cc82410968c4033fbd274d17831fdd789ddce -->
<!-- api: F:Inno.Native.ImGui.ImGui.NativeLibraryName bb2869b6e082e153d919049474b12fcef221c5ab1204ce97c2f2ae1dc26ac314 -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.FromFocusApi 7c68443ee4dacee8721301462b9bcb49b03a1017d9f8bf4c33cc002cc3bd04a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.FromShortcut ac6f45a57430f749ae2cee254d17d5b3137da82ddc6f731da84b0c1f0bfeca12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.FromTabbing 360a7e9a9e0dbbe4a89ebce326db31da69cdd5436f9382dc1977710b49724136 -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.None 60c701692c073fd3fbed38756065accfa99bfebc1b0e7efdfdc5e18ae93f5caa -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.PreferInput 60a9499b1df18cb967c219dcf3bebc11d3fed6b2f7eae9de9ed5b9f4fcf90fd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.PreferTweak 51fcc0d0e3572d3a9f16ac1ae07a845daec46631965bcb32c64c27b057f9caed -->
<!-- api: F:Inno.Native.ImGui.ImGuiActivateFlags.TryToPreserveState cb1d8d304122b0b330e1f37184d971b6284f25006749003ed454ca83bf638bda -->
<!-- api: F:Inno.Native.ImGui.ImGuiAxis.None aef996c7f07c23d5a2a028c8b1d1d0b24c155089cc8f70b4828b38f63163a71f -->
<!-- api: F:Inno.Native.ImGui.ImGuiAxis.X d7b9d0a420f4c917888394dab2dd1540f3cf3ea403aa32e78fedcfa7bd500556 -->
<!-- api: F:Inno.Native.ImGui.ImGuiAxis.Y 9fe280f6c2582588c9a0fe5649f50fce41d270da958f86d1c736ed7cfceaa6d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.HasGamepad 1c209606d982a2eccd289cbd450085e2e108b4d6d16af9c3feb7d99ac59dc90a -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.HasMouseCursors ae99a5218f9b6f5432dd051a5f5dda4f2dc159bf17b756d49d4a7512e1a10bf5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.HasMouseHoveredViewport a56f8b761a14b6f3956b013f97c4761b05fe8ac3ceede16909d23274182489fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.HasParentViewport e91330082144d85e45de69736abbde8c7809d45eeb5169443a5adac0125d5869 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.HasSetMousePos 8349ce1a94072bb7443cdcfca3fa987d17c4cde7266ef4a5f6c6f81284d31cd6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.None 37c61bf49fcd844db4f2c7e5880a067a6790a3fe068e09c50bc5445e53f1e2e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.PlatformHasViewports d7f5f63b1adb2b93729605040e6b3270dcac6e8042e5b0fb9359d438bf4b2dbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.RendererHasTextures 16d905693fc202ddc1a6bfa96dda42f1019726957422e15a0db3590d5417e08c -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.RendererHasViewports 87c0c81ee7915cff38c5fa087a1ce7aff07f4a01413f3f34d2de37d686e61e21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBackendFlags.RendererHasVtxOffset 5e3fe8c343980aa5bf8f23b299f3aa2428585fc87c30ea93aed4cb7ed56867bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.BoxSelectRectCurr 1ffe62b8bb069d4e45d2282f03228c62df1cbda81692d48f1c08ff28df4fd38b -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.BoxSelectRectPrev 3ed55ae8676302e994eaea2076b95c4e145a7570ca810def82c6f4e6c12ec763 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.EndPosRel 9a08f33b6267d1ae087adacd6d6faf9a7078a78fdc8b02aa0c08ddaaaff6ffd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.ID 445afd1f633410177764b73ead5e8cc090c94d9117abe54356d3463664efbc26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.IsActive 48012cd6e729d409430846f8e6217883b7776fdaa4825dd6df3deeb54a4cc874 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.IsStartedFromVoid c882d1511dd4e584200a881a9b5b113937e62eba93fc18b799693399717f0418 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.IsStartedSetNavIdOnce 90bccf6c576c52a61daac88d773937cda2a4aefe478544717574733185c8f518 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.IsStarting 9de43ff4e2c25206ae0d1828e5b28d1665015d903cb3a5dd6f439cffd6c3e9f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.RequestClear eb4804de0e041fe9102ba05aea4fbd30d0fc5b9c3fd9adf3638510d46729f2c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.ScrollAccum cba03007127429db7e10e92678273cf7d88250abdf25b13d7d2dec371ff6c30a -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.StartPosRel 06a12c071c804e4598dbdcec92c02456bfcfdd8d55459150b5eb582239c3a182 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.UnclipMode 86e32f76bd26d143cfd58073ed1286cb82cfca2d18650173d30d69dd353e2576 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.UnclipRect a9286205aa47159e1f063de1026e4c3aec5c4819dd5c97cf882095b932dcb6e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiBoxSelectState.Window 0f842ece6711f58ca23f34171239e623e48534a325a984c2cdc08e54165e76ef -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.EnableNav 48f9c17494c2de50f6f8109c5c8b514b234323301ec590c28221c4dd2a2e8936 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonLeft 553da5f5331af89c2d6fc519b2f5579bd5a5283d3e0bc262f0d24dddcd7572fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMask bc6bfe604dfe8c1321a7586e6644cb9d2080fd26bbf094785aebae5bab11dbe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMiddle 1f805619db8ec6674e92e588642d51281f501b33307ad77fd25a01e1b6d0a70a -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonRight 4486f17dbdfc02c66c1dbf01e2777e9f44dc89853054bfd5cdfd4b0d0724c9a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlags.None b3d7b41c09ca47112bf98bd37872bd57309162d0fd190a0bb5986c5654cc0087 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.AlignTextBaseLine 184ccefb8bbdc12a3cfaec45310cdcc58ddb1c42274b03f26c66433a198775a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.AllowOverlap 957c4f86f8085b87c0219a483fb2a161f8ae1d64e0f8b887d7485c39bca2bd75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.FlattenChildren 6118f4671ce634545049e70d1d23e996998991c1a4eb72ad2c10abd5145d522a -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoFocus b234cba719c3c34662ea6f10a2a8995560d35f2f4d0b0aec6b6046f2a1d9e583 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoHoldingActiveId 78dd64f6cad5da7be47918b90233255b0ae2c28de0b1b97fc93aa3ab7cecb85b -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoHoveredOnFocus d273d01d9eb707960a75ff9fd6bf657582da135daf20fd394f3cb2abc68fe3e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoKeyModsAllowed 95f61d62a9e7f4251279c3cacc2122afa9fa703c92e82b190679c6714137f893 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoNavFocus 427130b653a91c23d836716987c3477e671cbdcd7bc7bb9497854ce033bab85a -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoSetKeyOwner 8d155637c3a5c5b2f5ff4dba563effc057ed5a92ccdadf1b5b15485083993c05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoTestKeyOwner 68d434221fa61872d0b2a9e8bbe52091eedaadcb50f7bb4aab60af26598dd8ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClick e9ae6702a5125b1a5b9e27c3937f54e70e7fddd4c88b082499f4cee8d75968dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClickRelease 7e7d21f340ff54014167b1eca44fa0d6ebd5becee2432072d0f4037e24397b5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClickReleaseAnywhere 796e3a2c51a4135367ce653d360995ff7c5149e76f1fa4fba36d2f7c912cb18e -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDefault 30e1a809335a29afe58dda052ac70149d3cff659d8275c0168d0f8986a3d020c -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDoubleClick 3d918771343c33ade37d9b14c138cd5d96c22a5557d914abf9f8cebaac123a9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDragDropHold 51897edc71227ff04c4c3a7cabda8036ab25116fb10ca0905ce0fef4702b4e13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnMask eb20b3c64213482d2b8fd7ad8ecece4dfab70212d557c043d9920ff7d46c887c -->
<!-- api: F:Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnRelease 41bb414b0f78a4e6ef6ea258a12c3b81262d04dcbc8dc2a3213adfb8f2c93ef5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.AlwaysAutoResize 6665a618a98223034f512c790392c26fc1f4b813e2a486617c3c070cb8828548 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.AlwaysUseWindowPadding d69c059a13f567e558871504d9657fc99311c797957a09e5646b0dfafbdd508c -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.AutoResizeX c80ac42acfb6bb7899f140eee40c8433b35c094140efadac67bddb9e0ee9614b -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.AutoResizeY 83c2fad5efb93dffbf2219c9ac9abbb99f4b3a0d3d6c60cad8916375b7b3c4d3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.Borders 8ce69aadd6490cbefb84d1f240a1a85b47ce643d596c3d915315b4f65d4ebcd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.FrameStyle 5e0100da040df5f6f3bc7139dc99a84b6365af45be20eab59eb4f65a65334be3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.NavFlattened db88fa9705aff254564fc0f6aa682b9450aa4ba4b572c67847cfb98584ba7e0e -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.None 89515a15f82699d36764400c847522bb3f524903ead07a125b551fc350735a23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.ResizeX 4d51cd6e2f93cc7295c5e93018b8cd0cf9516f5bd3ee04fd2abc233ff453e074 -->
<!-- api: F:Inno.Native.ImGui.ImGuiChildFlags.ResizeY 104353cc8df9cb975d72de6c9c216e14061955ac5ef11f0945b01528cbb117a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Border 8ebbab33a8df0ac86f473114cccbf985865812b0742c14d8ca507488e20cd726 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.BorderShadow 34ccb400b8f2e33262bf4dac200e0bbc277a6d1e95a9b2ace3a3a546da4a3a6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Button b08963c1321e9c2e2d32323579f9dcad0d37f337870e7adc5a66a13176eac2c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ButtonActive fbf7ffa14c208b3f5e3fc17fc740b2c0008cbe22f3b58de500a01ea142b190a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ButtonHovered 6c95e848f49c3d2eb8f35fd523c7be29b9c4781ad57fee868a4a475b214e5309 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.CheckMark b17a33cd141e42398c81faeeaea5a8ae4043ac364d5e4e657cfb452a30c04c77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ChildBg 719f02f7aa99a71e72a9931f806f3875703277c3fa861436c6437e9fa66af45d -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Count f123e2c1f70bf7ae77f607a489db156247de68299a63a52a082bff9bdde30fb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.DockingEmptyBg b88fa121139e45c443d7903133e873338a22d2258d7a70e0e2e946dca35d33d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.DockingPreview 8ceb73e704f059b85649c806cf22d0468a70903013279b36b98533b394c7842f -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.DragDropTarget 5a5bcd4db4a35ba744d2774ab9a813e00fc315a801193c54f796f7cc00449189 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.DragDropTargetBg 6d107cbbb170552879b2d6792a29e66729a1cd6ea8f892441cceca12a95aca86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.FrameBg ac24fe644574cf10a322366a01cddc233a62cfe69dd98299a72614db874139ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.FrameBgActive 5887d2708609b4701ec5542a88ac22d1f0aa09b56471b3e271614fecf2c9ba94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.FrameBgHovered d2c19470da5c5f317766ba8a644f980ec65baade00b0ac71cfc5df91da093c20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Header 8d3039afe38f4638423ae6a8c73f3038bc2777ebae253861d10a45fa89c66a38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.HeaderActive d640d5952533483db830fce812c28b7925a6af96c80de9dd571a27dafe730aad -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.HeaderHovered 3614c2b8c37d5cdf0d3ca27e8aee14b24a7bff5486b5ff14f2162506d3bf16d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.InputTextCursor f1e61d2025479bc299f1cb0aae49b65f85954f0b4624f70c1ae7fa2561ab4c76 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.MenuBarBg 7b19b8dbd5326cf6947b6584fa84f884a15ada90c6dea5887f4ecf62420d952c -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ModalWindowDimBg 1c7e4dabdee5d7a2042cbad2d5668f6d6896023c33a3db8236b34f767ad04c18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.NavCursor 3aa2b9cb6327ad8ce49524b4f2c1709cfd405c03049a8eab7a44cf1f43024c95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.NavWindowingDimBg 33fe134fcf249bbcb1089195a7b5626b79efe5fd0b2e28345c6d535ff86a7e02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.NavWindowingHighlight c3c9f132c3a8bffd7dc1a54fb67871b3be3fa8bd08ee9593ffb353456d3d67d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.PlotHistogram ad30cdc8eebbf49238757df53a0d16572a33984e565407342ca60183fdd4ad6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.PlotHistogramHovered 7a6a0fb46cafcfb129566e7209f156087fd33de6063cc31f77f7fe5146d3769a -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.PlotLines ec673aa910d6230cae20a8e9bc2827db5b77bcc83cdd7e4cac58a30c9b831df8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.PlotLinesHovered e747d5be058cf014879233e208691ff9bf09a2b56ad98fac0561e64b8774fd7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.PopupBg 2ade368145005b4e0e2138063fbf1890650ce99cd6a8f1fb11132118512814eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ResizeGrip bd642b735675821090a36ca04ed3a54d4ae690105fadd1e03c76899ab3766d55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ResizeGripActive d5b8320fd89c81c78e5734d167c554933ad619a4fe8bc160b452d85c9f4c4610 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ResizeGripHovered 44edb6192ed7ef8596c5ece10f7686a2db45322abf57e72a2f8a27c245c40744 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ScrollbarBg 9905d78a00b5cd762c879469e7a4e738dd7eadd48b3b35a604623a3ee8ee866f -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ScrollbarGrab e51cfa1ae5d16d67bc061a33e892c1d5f53e9202eaaa29a0fcd4d13e559480e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ScrollbarGrabActive f085b1b173a0a161a22fbcdb8510e17c12aef985e8999d030ca21e46de5b7fda -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.ScrollbarGrabHovered 69251ccc9fa018ecdbd45b9261cfcb4bc01579446d0c2fb96bce76bcd5e7c05c -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Separator 6158dde798acedfdb2cd458988c4868ca19f2807de9e7bef19ebac9cc0bc1728 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.SeparatorActive 8bcc5d1ad146eae02d88e076b13586e2d851358dd5027807ba56bc1e13e8ef6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.SeparatorHovered 429f08bc5d2a42a0343a2083e5166a738c851ba10389f676aa590f531af9dd2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.SliderGrab 29e384be3774335332fac44781bddae7a96205e861de7855d0840c191a3aee13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.SliderGrabActive 4425c551fdc8c1a6f7f6e2e4215d0e6f06778db13a3def93ea99657b39877d6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Tab 2e26782a7381e2afa150c217693cd95558fa4f719b19fc68605f7def1d3e8698 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabDimmed 67254259ae8db111512efc897c4b8c22ff343869599b735b7c0f7486b0148fe2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabDimmedSelected 5a4eddae60c1cf11572b5ecc680d0978902cb3677fe874e4bf47cab98ffb8336 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabDimmedSelectedOverline 7e2081d02cc0c0d9259d2bb53bf7bd78ae6643d5d0dee56c65eac8ce7faa6133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabHovered daef91bc2660bceb809bea3201e4288fc39939a352cdcc8462cf83c0321b7851 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabSelected 06e2b747d97ad39c865b6232b37588b5cc789e5d07f4789c4345d9a9adbee3a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TabSelectedOverline b4bbc3c8a62c4356b684b98cfd28b5c790e0a2063f4b4fc6f8d26aa2027595f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TableBorderLight 646607b6edb646478f4dbf0964b6334f3f6dc7d7a22f3f1f1033cb959c37f94f -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TableBorderStrong 8705d7feb3adc891dc79d2d4699ac9ec6fbf3d5e1e81f75b4a8338fd0bc59b36 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TableHeaderBg 2f6a98fd5211a81d140605c2b038d6f6b50615426ef687ccaed07de38cb0719c -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TableRowBg 2eb79075bd5a079a9f87e0476fa138cbdc5c44aa5c3bf7c40f29bf693ba85176 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TableRowBgAlt b25a749084f4d366fae07322987cc7914d4cfdc5295cc548895cfa5215789275 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.Text 5f00b64e656332f25f5ee7f59c925178a645bb061a7be8a21a988ed2d9f7ad15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TextDisabled c04d002debab1f5c5799081c797f28a71c3eced2bfd98ebac031bfc24cdf5d61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TextLink abc4ffd772f097eb759e7af5a590cacdb96eb5021a389a6634b8e902b3b22daf -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TextSelectedBg ffd9df062cca1ee0a9686b7cca8e9a8064dc47e7a1161b0bab6b34c22c007d9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TitleBg 7358c38112ccc9af08cee9743c2730866ecae37d2b6d838c3ca2b7f655538687 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TitleBgActive 0d74b3c3fedd4d9c6aafbcd0fb051e4e77f745399387e4fa2f34d40d6c7327c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TitleBgCollapsed 799b960fad1afdbf6f81eeaae5a1d849fac222cdbd705345757f9c98b2564cd6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.TreeLines 68cae298f2856a5f105649e9dea445d66b2b912f75c0e73ca269375033b77d56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.UnsavedMarker 12a173cd393ddf4dcc166c0d737bf29a8fb32992326090891cd0edea6c884bfe -->
<!-- api: F:Inno.Native.ImGui.ImGuiCol.WindowBg 1988309fca84c622c055735ea110d1fb5073db28e9559cdf46e0e7b6303c973e -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.AlphaBar 7df6d0021d9ace2b3e2c14cd3ccb288df620edfd4487e4669148e8df4f4dbc28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.AlphaMask 7a90af5f8b87ae0a91963b5ebc6d8142f5bd2be7ae43339be6dde6a00d106b21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.AlphaNoBg 90e9298b9d1891839d0f2973061f3191d1562a53ad80db0e1899444b7c687c8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.AlphaOpaque 9351c0b707fa6cc8a57c00b507e96ec0191dcbf5d67115328b32756d89b78397 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.AlphaPreviewHalf b19506005c49e77f030468335bd144bac003ef56461845a77488d0e47529140d -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DataTypeMask e466568888e2387d8d17774f2cc44811ad8489d6e01c6e27af3c91e6a97ae1af -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DefaultOptions 7e126722e839df81809b3044df7c6957081fddc5737f845e1d716a808dc0b525 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHex 67a6895a44af44791d01f8088f8d09ad19e8be661e4830a79ee6198bdd377b48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHsv 43f004d110a86bda33e59b124d75c5f99e1ae6a5a52c543b770fb97f8ef09edf -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DisplayMask 9468549b0d9e0c2017aed53cee7243319575b427b6d26d6cce797b87b6835fff -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.DisplayRgb 191ddb01dc1941fe83c96b9665fd1918564630a61358f6234ed73d4c25703d79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.Float fd5176032da5478e3fc1064f742af8ad20a968a92211040a33ae8a6fab398dbb -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.Hdr ba34bcbe42bf64e74294da8c985971cde0befff4de49089a93225030315fed23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.InputHsv 59340e2f46e78b3e62a86955d370d350890b897338c8d9aefd7194f6937b24fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.InputMask aee53f79590197e6922d491b0840076a70f27ae71cb91b43150680b7e8540108 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.InputRgb c3ffe86716448d7300a00465c17efe919298a43d466b3ffe209f20e8c91fd5d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoAlpha edbb757b62daf35655340fc9ec6024c24597912231e0e55e193ee988de27c7fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoBorder 5ffaadd51e4b3721d4d66b37d4a9aa37394757390ec2a496ec78079ec181971f -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoColorMarkers 62942c6e54276c9bd6055db2bad03c47180a39fbb6cc83cb47af3c7c522a7a22 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoDragDrop 0e2de94edf3902afc4995dfce5fd0519c3b2949be0dcc801997996261210f474 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoInputs b923036fe26fb332d3236eeb3e4f0cc39574c27719009347ff6a3f46df946e7d -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoLabel a57691539a1c8ba4818f12c8766b5ab506455699726013f1576d3a56b7f8bcde -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoOptions 3fb3d8b67b8e4400bd713c7af58076fc67c4739459c560f7135e2f6e96b57a8c -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoPicker c9268755d4b709dff70fd3cd22dac3bf3df69144d53bbd1db79526aac97f2731 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoSidePreview 3be5b6a102263368d3765c7ed742cacd59c79071fb753e97258f3e33e5017068 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoSmallPreview d45e35e81a627325b98ad00ce86566ecd1e017f1e9226fc2ac6ab99a25650a4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.NoTooltip 95026fb5e84745f9d9d7c9ace072420b3247a0d35586597e1fa3668cc75b02c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.None 02aad14b31f5f45221c362e982ef4ae59e1e96ed433c1f70fc27a576e559874a -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueBar 8360c6aa7dd9d70a6be99145be0c150cd71b5d31f6bbcf0b0dfc47294cff513d -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueWheel c6e2a8cc45365e17037c181703392c89c463bbb24a0d3777fdd6fdc4775791f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.PickerMask 764fc7b5eeb2825d2ea1504606b548912ba3dd511f99b35bb6bbe8f82fd30f4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorEditFlags.Uint8 aa2d5fe79091b3ee15721647fee0ed80b51eb478b7a984eb6bab7b40558c53b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorMod.BackupValue 66c07bf16ed1b09b3fd8480b4a0ea0f87d2c69287ce0f795a8883fe121236835 -->
<!-- api: F:Inno.Native.ImGui.ImGuiColorMod.Col 28cbb7e05cbc965687c43ab8af498126cd03fed118e361652afe23f3c5c7e276 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.HeightLarge 0a6e114203675c85df9ee61d959e18a33a0d779dade778a4d5fdd516f03d585d -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.HeightLargest 1f79d23a972bf16e0b72f3056d8446056df305c9e7ad6b420c16c3b9a7d135ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.HeightMask ae64d07963e6450e9fdb6c1790a943bfc70e1f2f3acdc8ac47ebe098e3dd81b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.HeightRegular efb1a90efa0e61e7b3dde9e1f4b10a5b59783f6ad21c87ae92a05c0b1646d99e -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.HeightSmall 522f4af33cc6b7aaeea8c85a4dd9d5552c59e843333281452cca3a976b6b1e50 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.NoArrowButton 06f3df97fb879d7fcc683cb23f81ccf9ade5656022302627debce0abd3543ca8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.NoPreview 34a70a104cf9d063c05ee62cba0a244a65ccf89a34349ede0e1a4c7c498f9935 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.None 77e02d104e988d19af1af24c3f21e4180e02fb34514f9cf73fde8ba65dc91b9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.PopupAlignLeft 0b325194bbf0396f7726f7acf1dcb56b48655f300352cec2a77fbdb96bbc88fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlags.WidthFitPreview 8eeaafe39a7b11a04c8f5c9c03a322f78afdaa3f10caac784c1df461a1a1aa60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboFlagsPrivate.CustomPreview 9525c9ef91518ad453d9079dc0e2c9b80eddac733ab57ee1b0b0d580bf8ee4d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorMaxPos 5bbc5e72bca95c3423a706ee15374230d4e4befb308c1caac0f482916e1d140d -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorPos 4e98d8d6897096c84fc9d9eb7e452387d270004ebf23133501df42fb4c387eec -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorPosPrevLine 98cbd9bd3276f1bd372bf2be00ba4c2e4d16364f70605c89d1e30cff12d5ff91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.BackupLayout 49fedebdccbc213552be3e76d2fc8a7fd68d43a2f0175d7f22e696ac7dbc51cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.BackupPrevLineTextBaseOffset 4d455f8af47984683c34b840eee3e610ad9517a278eca8bbcdd3991976c04f8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiComboPreviewData.PreviewRect 92dbf1f70f01f409aa0bf35da4f04a58bf1bb5521405b965ab1c7116f31de1a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiCond.Always ece1417f5d991eb1f489a7e1593f814eeb87193a6490b4e78539ddf8abf5417e -->
<!-- api: F:Inno.Native.ImGui.ImGuiCond.Appearing 9ec889711e6e426550ff14f5b62b9077c074c3a4c734385a708a68a59a47288b -->
<!-- api: F:Inno.Native.ImGui.ImGuiCond.FirstUseEver ca2d97efbc4bf5e2e849992ee3cdcc0eb2d2e73c3df25929f61fe03a069bb82c -->
<!-- api: F:Inno.Native.ImGui.ImGuiCond.None cedb1b260491e1ce81b067513190d80d7c69fe5946a1bd991e246e2869d65c2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiCond.Once 9d7e153df85ff086ce7fa067a23b3c6976d902ac5a271fdf9676eb6c2022dcf7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.DockingEnable 6b974c8e4e43ee1ea3935caace0bcfba5f7b27fe6108c6744702cf8428d0575d -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.IsSrgb 12b30324316275db2d50711e1133e6d1b2beb2cb4e4785f9a031a0f91334af89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.IsTouchScreen 205538bf1ccbed004de319fad743f0c1078ea5313e65c0a8bb08b8f0bd2b8904 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.NavEnableGamepad f20ff9af37e8dbde5462cc6103db2327018154a064fd03ef7ae15b44ec028d23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.NavEnableKeyboard 7619f949cca57d0c123234d13e30da0475071306779ab0cab807952c4d08458b -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.NoKeyboard df64fc502984f75f2ecc23fbab2036262aa8c26d934a32db97eb5fea6dcf85c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.NoMouse 6ca537f0460f7dcc56de1f1af81cfc9e5c0fbe88c8c6d8ec7a69cd56fd084b2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.NoMouseCursorChange 173b13f44f525eb9fc1e530c9d20586737da5dbb8543df65b838d159977de596 -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.None 9be662c8797af5bf178be67d66b03c0a30b15d7e6c5c876ec8924c33b086ea3c -->
<!-- api: F:Inno.Native.ImGui.ImGuiConfigFlags.ViewportsEnable 1915f231bdf26012a2c90d5dd439530753f4db4aa3dfc69a913620ad779269b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Constants.IMGUIHASDOCK 858498373cd05411596d97ee5e6c36ebb4ceefca997c4e1411b497a06298663b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Constants.IMUNICODECODEPOINTMAX 1543d39194f128f7a94302160e2ca127636fcaebd88f327b96ce4912e2e44fe5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Constants.ImTextureIDInvalid c84dbd594280e77f6ab04cbedc8774016f778d2eb3c8455956f2996d118f54a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NativeLibraryName 86e431ed1b7b7dded0ecd131b26cb186ea76cbf936461086642f87debb7ccb46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHook.Callback f109abd14aca311cdc81c2de7e2bf7e758687f202ab980f8e949d0902fc174da -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHook.HookId 28cb7d58a84893575b39bd7a39fda3eb7aed96458608aaa3d0c54fa6398e124b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHook.Owner dfc7aa53c74f765851c638f99988bb94a968d3fabaf47377c886406f0cb80dad -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHook.Type 027be7e31d956a62a997a4ce243c849ac0e92f1deffc3607cda341874683c2e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHook.UserData af3f9f176d21660e9ed920d12adebee379d9cf08946266d165b3fc97aafef768 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.EndFramePost 44aa9aa561aee2a825e3e626f88814f9f0c52ae7dbe81b246c14bbf7941cc617 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.EndFramePre 69e9445a762baead5f7a69a20369fa8c7fb4cfca3aaca70e523b37bca257e05c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.NewFramePost bce10c09e5c5c3ca8a45cf8261742dbd5a9f1aaaa3393ecb3f3b2295f339436a -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-fe3b427f212bb41a"></a>[`public Inno.Native.ImGui.ImVectorImU32 Inno.Native.ImGui.ImFontGlyphRangesBuilder.UsedChars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25125) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24d33c9c6725f980"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, void> Inno.Native.ImGui.ImFontLoader.FontBakedDestroy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25157) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02c1d17cd328971b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, byte> Inno.Native.ImGui.ImFontLoader.FontBakedInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25155) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3e6ec6883ed7aa2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, ushort, Inno.Native.ImGui.ImFontGlyph*, float*, byte> Inno.Native.ImGui.ImFontLoader.FontBakedLoadGlyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd31edbbd16150c8"></a>[`public ulong Inno.Native.ImGui.ImFontLoader.FontBakedSrcLoaderDataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25161) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-365f0bdea6e1882e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, ushort, byte> Inno.Native.ImGui.ImFontLoader.FontSrcContainsGlyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25153) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4003547bc06cdea3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, void> Inno.Native.ImGui.ImFontLoader.FontSrcDestroy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25151) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29c5da3f1f3f6081"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, byte> Inno.Native.ImGui.ImFontLoader.FontSrcInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25149) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b737a24b92e15ed7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, byte> Inno.Native.ImGui.ImFontLoader.LoaderInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25145) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03895e95c9cc2f99"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, void> Inno.Native.ImGui.ImFontLoader.LoaderShutdown`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25147) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a3830146dfe4390a"></a>[`public sbyte* Inno.Native.ImGui.ImFontLoader.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25143) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0f43cb17ac2c087"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontStackData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25179) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aeec05dc0e63f10d"></a>[`public float Inno.Native.ImGui.ImFontStackData.FontSizeAfterScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0da61bca47fb0ba1"></a>[`public float Inno.Native.ImGui.ImFontStackData.FontSizeBeforeScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25181) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e2e8b3e04c932db8"></a>[`public const int Inno.Native.ImGui.ImGui.Constants.IMGUIHASDOCK = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L620) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-18eae4a3ba3acbbb"></a>[`public const int Inno.Native.ImGui.ImGui.Constants.IMUNICODECODEPOINTMAX = 65535`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65535` |
| <a id="api-927413aff535fbfe"></a>[`public const ulong Inno.Native.ImGui.ImGui.Constants.ImTextureIDInvalid = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-88987fe4206e3344"></a>[`public const string Inno.Native.ImGui.ImGui.NativeLibraryName = "cimgui"`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`cimgui` |
| <a id="api-014aaca2f81304a3"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.FromFocusApi = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-6d81d92c6c636501"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.FromShortcut = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25203) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-b24efaf47ab21653"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.FromTabbing = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25202) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-5a170893f40eaacd"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25198) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-08ec4b7a0d5b632e"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.PreferInput = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25199) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-407188098bbccca8"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.PreferTweak = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25200) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-d84043fa418eb6b4"></a>[`Inno.Native.ImGui.ImGuiActivateFlags.TryToPreserveState = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25201) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-521841f699b19aad"></a>[`Inno.Native.ImGui.ImGuiAxis.None = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-758b8999f62aaf28"></a>[`Inno.Native.ImGui.ImGuiAxis.X = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5444e0fcf4d1088e"></a>[`Inno.Native.ImGui.ImGuiAxis.Y = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25214) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-448d1f4ec4f69b90"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.HasGamepad = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-e93b66b406b8b9f6"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.HasMouseCursors = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-b5c5048bb573271f"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.HasMouseHoveredViewport = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25230) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-c7b4d1610ee562c2"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.HasParentViewport = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-104166880795dcb3"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.HasSetMousePos = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25225) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-00ad34eadb845391"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-c2a16f50a48ad7cf"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.PlatformHasViewports = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25229) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-9c90ec7e58e42181"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.RendererHasTextures = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25227) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-9d932b2c7ea884e5"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.RendererHasViewports = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25228) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-3fab37fdf223474c"></a>[`Inno.Native.ImGui.ImGuiBackendFlags.RendererHasVtxOffset = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25226) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-52a1021dab48a2b1"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiBoxSelectState.BoxSelectRectCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2c314fe7e834f81"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiBoxSelectState.BoxSelectRectPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8ec6631c21a60b4"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiBoxSelectState.EndPosRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d2da98b95e60dde"></a>[`public uint Inno.Native.ImGui.ImGuiBoxSelectState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25244) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c52889d71b3d13e"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.IsActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb0ad4e22a999331"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.IsStartedFromVoid`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b602a2d7386e563"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.IsStartedSetNavIdOnce`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c26190f55a3b59a"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.IsStarting`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13cb1e850eabea11"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.RequestClear`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5978262211f0a7a6"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiBoxSelectState.ScrollAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c5a62b43f91a05a"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiBoxSelectState.StartPosRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e0c07803ad53891"></a>[`public byte Inno.Native.ImGui.ImGuiBoxSelectState.UnclipMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18c12fc974267d5d"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiBoxSelectState.UnclipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25266) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9159e624b512e4ac"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiBoxSelectState.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25262) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0371f85900001c11"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.EnableNav = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-4bfcc04b8497d277"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonLeft = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d93b4bafd5e24595"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMask = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25313) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-28a0b4baad95d766"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMiddle = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-c6913a74a9f1ef52"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonRight = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25311) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-0138a763a29afe63"></a>[`Inno.Native.ImGui.ImGuiButtonFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-ac44df6d03713da7"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.AlignTextBaseLine = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-d5ab87a1d9ea9a7a"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.AllowOverlap = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-f5c85675b46af037"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.FlattenChildren = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-cdb03ff10ffa1a87"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoFocus = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-01ae2dc229256f6d"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoHoldingActiveId = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25332) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-7f89b4d476415734"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoHoveredOnFocus = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-30ee8053fc4b891c"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoKeyModsAllowed = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-d63cffcd159dbc22"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoNavFocus = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25333) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-73c7903e35b180d5"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoSetKeyOwner = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25335) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-241f91d461f9c272"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.NoTestKeyOwner = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25336) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-de1c7d6cdfa1fecc"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClick = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-83a1319e6afea825"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClickRelease = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-92a3e9aa2c4c73fe"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnClickReleaseAnywhere = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25324) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-d03502304c0f6c92"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDefault = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-545dc8173009a3ef"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDoubleClick = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-6185eeee5610c6a2"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnDragDropHold = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-0c98bdafa188f407"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnMask = 1008`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25338) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1008` |
| <a id="api-60cbebd1fd9dbba9"></a>[`Inno.Native.ImGui.ImGuiButtonFlagsPrivate.PressedOnRelease = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-1af76fbcaf264bc3"></a>[`Inno.Native.ImGui.ImGuiChildFlags.AlwaysAutoResize = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-443fad09c6453238"></a>[`Inno.Native.ImGui.ImGuiChildFlags.AlwaysUseWindowPadding = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-dc9b12a0cfc66c71"></a>[`Inno.Native.ImGui.ImGuiChildFlags.AutoResizeX = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-3df355d4cdb7d7c8"></a>[`Inno.Native.ImGui.ImGuiChildFlags.AutoResizeY = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25353) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-cceaa0eb97c11c41"></a>[`Inno.Native.ImGui.ImGuiChildFlags.Borders = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-45950592e66e0a29"></a>[`Inno.Native.ImGui.ImGuiChildFlags.FrameStyle = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25355) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-4ac54c5cf4b5eb77"></a>[`Inno.Native.ImGui.ImGuiChildFlags.NavFlattened = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-3146d8c80bee7ca2"></a>[`Inno.Native.ImGui.ImGuiChildFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25347) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-4516b10de04c5929"></a>[`Inno.Native.ImGui.ImGuiChildFlags.ResizeX = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-a64c6e1554721ef5"></a>[`Inno.Native.ImGui.ImGuiChildFlags.ResizeY = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-5b57e8d5cd7f52c7"></a>[`Inno.Native.ImGui.ImGuiCol.Border = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25369) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-40ff62910d0f2331"></a>[`Inno.Native.ImGui.ImGuiCol.BorderShadow = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-963fbda7ac0c16ff"></a>[`Inno.Native.ImGui.ImGuiCol.Button = 21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`21` |
| <a id="api-6c9fbe56539ae15e"></a>[`Inno.Native.ImGui.ImGuiCol.ButtonActive = 23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25387) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`23` |
| <a id="api-5a410cf47b81cec8"></a>[`Inno.Native.ImGui.ImGuiCol.ButtonHovered = 22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25386) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`22` |
| <a id="api-2f4a4fc8e4a50bc2"></a>[`Inno.Native.ImGui.ImGuiCol.CheckMark = 18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`18` |
| <a id="api-45b46820c8e7efb9"></a>[`Inno.Native.ImGui.ImGuiCol.ChildBg = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25367) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-a3b89c2e44c0ca82"></a>[`Inno.Native.ImGui.ImGuiCol.Count = 62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`62` |
| <a id="api-4a5537852867b24a"></a>[`Inno.Native.ImGui.ImGuiCol.DockingEmptyBg = 42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`42` |
| <a id="api-077570bce7da74aa"></a>[`Inno.Native.ImGui.ImGuiCol.DockingPreview = 41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25405) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`41` |
| <a id="api-be511e91a88b40e5"></a>[`Inno.Native.ImGui.ImGuiCol.DragDropTarget = 55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`55` |
| <a id="api-aeed32f87f7627d0"></a>[`Inno.Native.ImGui.ImGuiCol.DragDropTargetBg = 56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`56` |
| <a id="api-e96d4700f84aa8a6"></a>[`Inno.Native.ImGui.ImGuiCol.FrameBg = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25371) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-95d22042ebb9ac72"></a>[`Inno.Native.ImGui.ImGuiCol.FrameBgActive = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`9` |
| <a id="api-436197d8a526e53d"></a>[`Inno.Native.ImGui.ImGuiCol.FrameBgHovered = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-6ec08847757f8a99"></a>[`Inno.Native.ImGui.ImGuiCol.Header = 24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25388) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`24` |
| <a id="api-1768de7716cdb9ef"></a>[`Inno.Native.ImGui.ImGuiCol.HeaderActive = 26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`26` |
| <a id="api-9a1c51b10b230b27"></a>[`Inno.Native.ImGui.ImGuiCol.HeaderHovered = 25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`25` |
| <a id="api-b989168d52d8b170"></a>[`Inno.Native.ImGui.ImGuiCol.InputTextCursor = 33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`33` |
| <a id="api-b086a728ae0a7b5e"></a>[`Inno.Native.ImGui.ImGuiCol.MenuBarBg = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25377) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`13` |
| <a id="api-6026ceca37d67e09"></a>[`Inno.Native.ImGui.ImGuiCol.ModalWindowDimBg = 61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`61` |
| <a id="api-6884b22e2bc98024"></a>[`Inno.Native.ImGui.ImGuiCol.NavCursor = 58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`58` |
| <a id="api-01dec4291dee745a"></a>[`Inno.Native.ImGui.ImGuiCol.NavWindowingDimBg = 60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`60` |
| <a id="api-ffe24388a9cba93e"></a>[`Inno.Native.ImGui.ImGuiCol.NavWindowingHighlight = 59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`59` |
| <a id="api-ddcccd84ab23521b"></a>[`Inno.Native.ImGui.ImGuiCol.PlotHistogram = 45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25409) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`45` |
| <a id="api-072e705b4c2212d9"></a>[`Inno.Native.ImGui.ImGuiCol.PlotHistogramHovered = 46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`46` |
| <a id="api-244edf31f03914b6"></a>[`Inno.Native.ImGui.ImGuiCol.PlotLines = 43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25407) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`43` |
| <a id="api-2b0b2a7da074c794"></a>[`Inno.Native.ImGui.ImGuiCol.PlotLinesHovered = 44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`44` |
| <a id="api-f9735364a6822bf9"></a>[`Inno.Native.ImGui.ImGuiCol.PopupBg = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-74bb5e5c180d4a26"></a>[`Inno.Native.ImGui.ImGuiCol.ResizeGrip = 30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`30` |
| <a id="api-81868c47edf8411d"></a>[`Inno.Native.ImGui.ImGuiCol.ResizeGripActive = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-ef033db78a7a890e"></a>[`Inno.Native.ImGui.ImGuiCol.ResizeGripHovered = 31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`31` |
| <a id="api-fc4f513a4ac59225"></a>[`Inno.Native.ImGui.ImGuiCol.ScrollbarBg = 14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`14` |
| <a id="api-ca6e79e9c12b63df"></a>[`Inno.Native.ImGui.ImGuiCol.ScrollbarGrab = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`15` |
| <a id="api-15c030f96b3c18f3"></a>[`Inno.Native.ImGui.ImGuiCol.ScrollbarGrabActive = 17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`17` |
| <a id="api-986dd5f88fc30d7e"></a>[`Inno.Native.ImGui.ImGuiCol.ScrollbarGrabHovered = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-a1b62edcc16a0675"></a>[`Inno.Native.ImGui.ImGuiCol.Separator = 27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`27` |
| <a id="api-1987c0cb22362b1d"></a>[`Inno.Native.ImGui.ImGuiCol.SeparatorActive = 29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`29` |
| <a id="api-1e059691eef05887"></a>[`Inno.Native.ImGui.ImGuiCol.SeparatorHovered = 28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`28` |
| <a id="api-1650759aca5c86a0"></a>[`Inno.Native.ImGui.ImGuiCol.SliderGrab = 19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`19` |
| <a id="api-f7efbc08cea21919"></a>[`Inno.Native.ImGui.ImGuiCol.SliderGrabActive = 20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`20` |
| <a id="api-9e4d4ffb9e7ae679"></a>[`Inno.Native.ImGui.ImGuiCol.Tab = 35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`35` |
| <a id="api-54129b963e626a2b"></a>[`Inno.Native.ImGui.ImGuiCol.TabDimmed = 38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`38` |
| <a id="api-30b4abec2dc844b9"></a>[`Inno.Native.ImGui.ImGuiCol.TabDimmedSelected = 39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25403) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`39` |
| <a id="api-d6b56355a42ea895"></a>[`Inno.Native.ImGui.ImGuiCol.TabDimmedSelectedOverline = 40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`40` |
| <a id="api-cc8bd33b707bc82b"></a>[`Inno.Native.ImGui.ImGuiCol.TabHovered = 34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`34` |
| <a id="api-c006f98ea31bcb38"></a>[`Inno.Native.ImGui.ImGuiCol.TabSelected = 36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`36` |
| <a id="api-68c3f0a9d9090ec2"></a>[`Inno.Native.ImGui.ImGuiCol.TabSelectedOverline = 37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25401) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`37` |
| <a id="api-adeb951217454aec"></a>[`Inno.Native.ImGui.ImGuiCol.TableBorderLight = 49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25413) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`49` |
| <a id="api-2cb1628820797742"></a>[`Inno.Native.ImGui.ImGuiCol.TableBorderStrong = 48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`48` |
| <a id="api-e0cd6c669f580c4c"></a>[`Inno.Native.ImGui.ImGuiCol.TableHeaderBg = 47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25411) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`47` |
| <a id="api-dbdbf2186899e419"></a>[`Inno.Native.ImGui.ImGuiCol.TableRowBg = 50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`50` |
| <a id="api-85fa98b5508eccba"></a>[`Inno.Native.ImGui.ImGuiCol.TableRowBgAlt = 51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25415) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`51` |
| <a id="api-2d3b8839c2cfb42f"></a>[`Inno.Native.ImGui.ImGuiCol.Text = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-2b71da1c59c3f04f"></a>[`Inno.Native.ImGui.ImGuiCol.TextDisabled = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-70fb7bdea46917c5"></a>[`Inno.Native.ImGui.ImGuiCol.TextLink = 52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`52` |
| <a id="api-2f7c789b62f25985"></a>[`Inno.Native.ImGui.ImGuiCol.TextSelectedBg = 53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25417) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`53` |
| <a id="api-e2be82852028991f"></a>[`Inno.Native.ImGui.ImGuiCol.TitleBg = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`10` |
| <a id="api-aa40d06730dcb633"></a>[`Inno.Native.ImGui.ImGuiCol.TitleBgActive = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`11` |
| <a id="api-71eae61cf770cfa7"></a>[`Inno.Native.ImGui.ImGuiCol.TitleBgCollapsed = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12` |
| <a id="api-62f4373a7cef90c3"></a>[`Inno.Native.ImGui.ImGuiCol.TreeLines = 54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25418) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`54` |
| <a id="api-43b4e5ee26c0ed90"></a>[`Inno.Native.ImGui.ImGuiCol.UnsavedMarker = 57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`57` |
| <a id="api-c14f50eb0d4aad2e"></a>[`Inno.Native.ImGui.ImGuiCol.WindowBg = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-a16513deb7908730"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.AlphaBar = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-09cfb3f6b55903d4"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.AlphaMask = 28674`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25461) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`28674` |
| <a id="api-88fa33c72ac00d68"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.AlphaNoBg = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-6dd8a2eccc5b6dc4"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.AlphaOpaque = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-4c7f871d957437ad"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.AlphaPreviewHalf = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-339d8fa77dd41af6"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DataTypeMask = 25165824`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`25165824` |
| <a id="api-ac545b550342bddb"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DefaultOptions = 177209344`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`177209344` |
| <a id="api-7082d5943b19199e"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHex = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-19062ca9a7c30a7f"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DisplayHsv = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-6762802669df243c"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DisplayMask = 7340032`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7340032` |
| <a id="api-023c60da5e88bb86"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.DisplayRgb = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-b5dcb77620082932"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.Float = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16777216` |
| <a id="api-e63b903efddc7bdb"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.Hdr = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-436e0e25723db9e1"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.InputHsv = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25459) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`268435456` |
| <a id="api-501e52c4e0854424"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.InputMask = 402653184`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`402653184` |
| <a id="api-db190ccb29ddb2ce"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.InputRgb = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`134217728` |
| <a id="api-d5f3879c51c67b15"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoAlpha = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-87940bd50398c449"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoBorder = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-2d80b83686a2a0ab"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoColorMarkers = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-59dc55dbc6e1cf2e"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoDragDrop = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-d63d53d62a1e5687"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoInputs = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-571bcdf45b25cf69"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoLabel = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-5760a4ede93e3ea6"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoOptions = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-e31e94e87fcee3f9"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoPicker = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-6bea92b4bcd079e0"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoSidePreview = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25442) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-e6a658f65165588e"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoSmallPreview = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-189004fd87405122"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.NoTooltip = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-ce6c78469d97e31d"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-d39d0bfaed7a213f"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueBar = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`33554432` |
| <a id="api-5a1c7e71ca281917"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.PickerHueWheel = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`67108864` |
| <a id="api-4fc1d3ed893c2da8"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.PickerMask = 100663296`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25464) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`100663296` |
| <a id="api-e69f20fe63401254"></a>[`Inno.Native.ImGui.ImGuiColorEditFlags.Uint8 = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8388608` |
| <a id="api-f8b1ce19f9000274"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImGuiColorMod.BackupValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ce186eacb24c2c5"></a>[`public int Inno.Native.ImGui.ImGuiColorMod.Col`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb0d634b8d0f6788"></a>[`Inno.Native.ImGui.ImGuiComboFlags.HeightLarge = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-69715dd01c0fe422"></a>[`Inno.Native.ImGui.ImGuiComboFlags.HeightLargest = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-21bbd01ef0f1df63"></a>[`Inno.Native.ImGui.ImGuiComboFlags.HeightMask = 30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`30` |
| <a id="api-f2bf22d0b6c7fc3b"></a>[`Inno.Native.ImGui.ImGuiComboFlags.HeightRegular = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-3cd7b244ae964d77"></a>[`Inno.Native.ImGui.ImGuiComboFlags.HeightSmall = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-cdcf7941402bd7fb"></a>[`Inno.Native.ImGui.ImGuiComboFlags.NoArrowButton = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-9873ca1710fc92bd"></a>[`Inno.Native.ImGui.ImGuiComboFlags.NoPreview = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-cba984b32b2b0321"></a>[`Inno.Native.ImGui.ImGuiComboFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-1b6f95342a45993f"></a>[`Inno.Native.ImGui.ImGuiComboFlags.PopupAlignLeft = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25494) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-264d8fc4ad1fa9ed"></a>[`Inno.Native.ImGui.ImGuiComboFlags.WidthFitPreview = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-1fdfacd0b539167f"></a>[`Inno.Native.ImGui.ImGuiComboFlagsPrivate.CustomPreview = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-8fb14bbc39105d1d"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e70a549e6d9b2e4"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5d5329fa79a2055"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiComboPreviewData.BackupCursorPosPrevLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc4f8dc2e3eaba7c"></a>[`public int Inno.Native.ImGui.ImGuiComboPreviewData.BackupLayout`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25531) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b944ee28f9184064"></a>[`public float Inno.Native.ImGui.ImGuiComboPreviewData.BackupPrevLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58dbc9481aecfaa8"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiComboPreviewData.PreviewRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea98db2fccc176ea"></a>[`Inno.Native.ImGui.ImGuiCond.Always = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-fee039ce0a13e298"></a>[`Inno.Native.ImGui.ImGuiCond.Appearing = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-59a26297fc191307"></a>[`Inno.Native.ImGui.ImGuiCond.FirstUseEver = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25549) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-1b356f2c4ab7a291"></a>[`Inno.Native.ImGui.ImGuiCond.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-dc934d07cbafd74f"></a>[`Inno.Native.ImGui.ImGuiCond.Once = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-5e19cbce6029083e"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.DockingEnable = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-a245c78814ee2df1"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.IsSrgb = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-3f05b59538326a22"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.IsTouchScreen = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-17c75c873afab9a3"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.NavEnableGamepad = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-dcfed2f435019d7f"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.NavEnableKeyboard = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25559) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d7f026c70598bb08"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.NoKeyboard = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-1c546aea6b84632f"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.NoMouse = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-5854cc87ad3df263"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.NoMouseCursorChange = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-13616cb6dd32452c"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-09b237963037a165"></a>[`Inno.Native.ImGui.ImGuiConfigFlags.ViewportsEnable = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-eb75e4e4023ad446"></a>[`public const int Inno.Native.ImGui.ImGuiContext.Constants.IMGUIHASDOCK = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L633) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-fdf4c748f9c56e60"></a>[`public const int Inno.Native.ImGui.ImGuiContext.Constants.IMUNICODECODEPOINTMAX = 65535`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65535` |
| <a id="api-15faf49f4e85014c"></a>[`public const ulong Inno.Native.ImGui.ImGuiContext.Constants.ImTextureIDInvalid = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-4b8883aec95d30c8"></a>[`public const string Inno.Native.ImGui.ImGuiContext.NativeLibraryName = "cimgui"`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L753) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`cimgui` |
| <a id="api-9c5502bfadecd919"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiContextHook*, void> Inno.Native.ImGui.ImGuiContextHook.Callback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24ac489f622f1b56"></a>[`public uint Inno.Native.ImGui.ImGuiContextHook.HookId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25578) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-640d1a6a81e93e72"></a>[`public uint Inno.Native.ImGui.ImGuiContextHook.Owner`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c859a73969128d3"></a>[`public Inno.Native.ImGui.ImGuiContextHookType Inno.Native.ImGui.ImGuiContextHook.Type`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f6f3f6e17fa9da9"></a>[`public void* Inno.Native.ImGui.ImGuiContextHook.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01c9ab6d0c0805b0"></a>[`Inno.Native.ImGui.ImGuiContextHookType.EndFramePost = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-3ee73c0102285acb"></a>[`Inno.Native.ImGui.ImGuiContextHookType.EndFramePre = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-175f56f54a29e8d8"></a>[`Inno.Native.ImGui.ImGuiContextHookType.NewFramePost = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
