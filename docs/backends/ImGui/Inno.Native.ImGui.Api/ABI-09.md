# Inno.Native.ImGui ABI · 09

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.None fd2e1c8426a7fa9d5675480e2cea8fe356f50207656bec58878b5913bd250535 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.AllowDoubleClick 0c88c7c9e908aa906bf8aebf1ff4b23508094566993517b9551d5606a25f6ab1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.AllowOverlap 2eaace58dffea308a0c680c55d8ba9be5a988349ae44c87fdfff259279510cf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.Disabled a2408ce05ee8d935300d364dab99be6a502f1de1445818dd3d0c99ce85085536 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.Highlight c84f5cdf07b8e7230790f23372d9a2666a3c47d1850f00121f799fd1ea074e4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.NoAutoClosePopups c0bc8e332571ae7533321fce978d07854a9a6256ae7685c1b041c70084e36fdd -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.None 92d68b95b147f0abaac557b0f91f34ea90e11a9b32896a78ce424508597426da -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.SelectOnNav 69b09b5eca5cea89348b860dddfa26a07ee17c16923a64d7a00bd5f0a496f8ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.SpanAllColumns c9cafbd7f0f17806c5678db73896e4b80ac78520bd2afec62e0472d0e2764849 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoHoldingActiveId 8772aec504e71e705f3f61cd5fc7af5c2e3ebfd124cb4f8c94403e59c76dfc95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoPadWithHalfSpacing 58b19d3647ef2235dc221574751485030b8e44d919a81c86690d340a3efc8137 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoSetKeyOwner 730f491ac29170360d9cf6b0958189b4e743e1cd8238806d4cdfb982347303cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnClick 15df74107db4dbfbe2976a487cc4e366fec959478715ba1c99c231203b85bbc3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnRelease 61047701ab9502fef990d3f933a09d548a279393606a1adb92010cfa89420b45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SetNavIdOnHover 6393cc2db857ab8d6f9959aca5dcf72259a106ba126642ecfa2d01c61f47a581 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SpanAvailWidth 805f7a2a6256c1b499b04255b8f0cc66e00c0d3e705910a3c7ad171e8ad9806f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.AdapterIndexToStorageId 7b0e30e135f8295a67ccefc5852efaddf4d94233e5253022f832691cd2307f51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.PreserveOrder dd103c1cf59679162c2aeb5cf9c4e8c412c0e958fad99697f4b23a39d8123624 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.SelectionOrder a7a6e38212cc995c7833fd65980647fb09c17b70bbbcc3256deeeab92e5936c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Size 2fc9e317bfb0bab50865c1f491f3f6b5fdde622a3321d704f2efaa0c5b651817 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Storage 39cd1df731d42ca2fb2c6acd6255114bf8596201b0f480452347ce9fd0e2dd7f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.UserData 83835e6b344b89c8e8f98ae23fe66992c2aea03adf047248be6696ea84c036e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionExternalStorage.AdapterSetItemSelected 73f4886d84fcd7b8976eef8bed0fbe35ec7a887d787fb207c7d7a2cb7c05872a -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionExternalStorage.UserData 3b09bc56e19f88e927f28f81cdcb1caf42104d013cd4493147cb7b8826cc054e -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeDirection eefb2e9b281d3ecf727aea25fcef35dae84f75aeff6c3c4ddf2893ec119d66e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeFirstItem 82b01665162b52883cfc34dca310be2ded37b097f4ea390e3840f730b165f821 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeLastItem 1b473a0f9265aedc6475dd5269edcb1e6e44156c5694094296c9ecdd052b92b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.Selected 439e579aff3c026844aa25be5329381f80316fd5d477dff3afd3b9c6876a20f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.Type 912aaed9983d797dc3c8ed5af47ce783cb1e5e209baee6a923707e4d90ac1d0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.None 890642d17f89933cde0ae9f5f2d87aa69422571b5bc14a066b2a874bca64937a -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.SetAll 4fcef370dc4b5135b5245033f73b867d5fdb7c37f233fc2de3f37a9e0b51ba04 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.SetRange 488fd2a83f0dd00e0451eb7bb156397a5899de4d0ab6a40658fd25c67872f852 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.Horizontal 94062d97d692e139a296b66ab14317af98e9c90c795bdb2306368f85650444f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.None b23bd8aafd1757f4a386165acd26715946f2a3b22f53e22f4baf833e40169048 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.SpanAllColumns 0dcd0e6944441351e8237d723d33a84d2451674e2047877a8e4bd1cfcfc7fd7d -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.Vertical 9d2dbe0e16857cbb252667910ca6221615985924c9930b3f3a3a87b7d93892f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ApplyAllFn 1d3e807ad9161694b1367ac7adb0f0352c350fe0ef5c9fad66f44fd37372ab7f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ClearAllFn 965ac857931dc63f34d835f18ad5ae1af2d874245c15336d9d24406a3a62151c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadInitFn 05fe491ac336f2450b582a9249654333748b3e006b325e3a17ebd2d079472cf3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadLineFn 3a385fecb6a07badc4e52e7efa6b15721c5b0be993d3ed59c23f5bdce8f7c7f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadOpenFn 61025fb53105870ba319ada3b2ca2ac604ebdc3bdb7030ed309223eaf6c8f519 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.TypeHash 8d6bd0f30e17b244e349533c9839ad043c3d761864d92fecbef4b6c2fca18354 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.TypeName 7b6047c6dfc1ec61643fed725569e5f359ad570e9ef98711c18af1dcdce8a45a -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.UserData 03c4ac0c0c388ca330cb4a5bf2042cb034222b94bbcf7ca3539cdb42c725b875 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.WriteAllFn 50567cb1df60fe973ca610ae69ec8be47b60d71cc05159776b04814b7a052503 -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.Index 346aa0e2bde67fc3ed786fd8dd5a3cae0425ebc3499bd81d3c928a81780d6240 -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.InitialWidth 3374afd5325b34eb9701d23ececfdf8592cd4ae589beb3086190e33c9ff250f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.Width 0a25f0ea90677f5724b61bc233ddc575e5034d96bd96cfa5c7dd242701311d81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.CurrentSize 3a064c80ba0b8fce2eb33f118d814c3eaa155b8411129a78ac0f54e3b40ca58e -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.DesiredSize e849644fd1ba24b4324486574e6664a71773882a697e9ecaedb632d58f1dca92 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.Pos c65e67c9e9e8c1cb49d1918f85048da62d980047df3d2cb8353bd63ce12fd0b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.UserData be3a97ca264399fa41dee8c6daacf0a05010d84422edd662532c58caa9b51c86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.AlwaysClamp e20bb75ad3f5496a27545f4c98bddb5b53262e5f7a6174e64367ecb161b5a56b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ClampOnInput 48095b7e1ba7791347c5bc52cfe1d502525224c48feb095c4292a77bb315e26b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ClampZeroRange 7f73682be5d9d6083c16768d228198716e7d41bde886017474bc454fbd520253 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ColorMarkers 455ad6b332005c01a47bfaeb466d38badea8bb791c762e29103ef7aca936338f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.InvalidMask a91b94be547d055d0e068050dad99ea3a6921e15936262ca657201c3a3daf506 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.Logarithmic 249b38753fe4ef6c0146a76db58dd3305bbfabfb347b341bd6be109fe7630370 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoInput 84cbe87b325e4690e20006369bd2474327c8c541cad3e45eb6efb286c6783f11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoRoundToFormat 5585f87ce96dcab43110afde1ea8f1a806dabcfb2714ebc02b15b95f81828665 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoSpeedTweaks ad8639c9538dcf9ca9d913315aa639c3a319990590c652ff442cec1a52bef3be -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.None a34ebe2706b271589785fcf5fd99984138cff2bec9595a912cee571ef5074e9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.WrapAround e74ea226788b9a086db0607fb77ee7b814cca64d8e57263e87000132286dd5b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlagsPrivate.ReadOnly 1a8afefbd520105f50ed919104abda81687d42827970740f8beb5b9ec082b9eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlagsPrivate.Vertical 96b4e680a45a9db804e099d0b38ec629d52748e27f7390ba2a6dd86a54fe4996 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.Ascending 7064f3ea2115a7e817d01f9bd5d29bfa38a133846db55ce165e3137f348a6c6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.Descending 4876418496fb29f18b04263faef51ee740eda2b961c13ce643b880a41b87e98c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.None 9a1df5a6ad82f1819f56c2ca6947b64d475b7b4ff1941cfb61fa65f7d4ecc403 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.DataType b4309585a5049310d4dd83a2823b0d5464aca0de078bb6c528ee4b5ef75b3af2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.DescOffset 7dd8e2673505ffb9c515f3a914afb221cc877ae1d03a1564eb13c7e236063e9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.ID 322b5bb16a4a954f081c2bcafea7c89f6d4b6f69aaedaf3a76d7a854d214ebe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.QueryFrameCount 895ac1aedc9cc9434ebcd03151c11c4090874e49f2e821d36f107838232ccf53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.QuerySuccess a007222545b3abae22716bba89a9b77459e08fa1ad1863620710551faacab831 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStorage.Data 12c4c5fdece1579b18678a834af5a82efcbcf4e6856edad9ec220c21be106551 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.AnonymousField1 42548d10f765b1991c346704e0320ad4ae06b0d45a77c18e7be907e358a74f6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.Key b03a481fc71904ed9dfae24c5aaa327ad699ee1824eb4fb3443358c6329371ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValF 64486612184cda2ce5c1f5ac1b246ce1cbd39f9b5675a5144bdc9aa048512ea9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValI f4f8a8cf3fac85a5256c88376c23e7e77125c7be368ac3802e88ed98ec1ded5b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValP 93d5b15f4655a51b5e55bba7663d41c7a2aae2be8820ee96bba6ccb9f3e0c2d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Alpha df3fa12df592598ae1c50d4f83e4ad23497f04f857864f620ba945cb8fcb5d61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedFill d30d590226a71fb567b706f7dce948fa1b8339900ec3ffa332cc1fa2673feedc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedLines 0dea99e0493c894c93949671318da19ac8252839f7c5a5ff9af8d24ed59a62e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedLinesUseTex ee4752c7acd29ab47f291fe661ad17e6b6ace216a9e32764d263702fd35a360f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ButtonTextAlign 33e3cd6c3dcbeea6be3bba6a01cc2a114274128bd3d8c79c696f781c358421f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CellPadding da14351010a9801fdf3aa5442b4b1449a4f98a97b22af2b6f088e3c9189c5e09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ChildBorderSize 0b195ffdffcb0f1461bf39641c6b5770833241dfd681c77fc98102b5c30e940d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ChildRounding 48c123bb04490ce0239ff5323627555bd63c2407cca7bece5b3617ea8abd709c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CircleTessellationMaxError e833d4d1eb31582514ad8d52f5b77c19bf8986d5fd8ba7ca03112e4af8b7ce25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColorButtonPosition 96e8c25b33493c87c4466fbc6275e99b537fefd858cab257b34e1dc61268ccc6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColorMarkerSize d93b6e5f151664970cd71b1f15a379121a8e591b56d28e0a4d3617f50f4c67e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors dfd434c8697cdce15650371e6942e89f7ec12ec257f390022efda96df07a646c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColumnsMinSpacing 666b5db6cc7eb2ea79763392783f8a15723f45b457d8a8c8263e3de83cefc5f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CurveTessellationTol 154cf7977064be6012a0bdb92f60cf76fd7354cd79beeaa2bf2bcfebb3208c13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisabledAlpha 1bebc4cb818d2253779aa448d91bc16f543080bbf9c2e7a702769873f08074eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisplaySafeAreaPadding 6157db2750f585fa2cea2ec3726d8f99a6f567d9aa249434ece20c51fbe47ffe -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisplayWindowPadding a9fe96f2986ddfa23abf852355a0e0ec5036e2770702109a4f60ab884ba84253 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DockingNodeHasCloseButton 0b6f9f0cd29ffb162deb311328735b33fa010b7b521610ed39715637069a4cb8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DockingSeparatorSize 012378928ae55ef7d63a752845af06558a3a6b67a59e8b88ee103d32db6eb940 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetBorderSize ae7b78b4cbf4c57d495662741f5184a723ac8d335d2b681942b7e09d56c8abf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetPadding ac7a4215df20e760a1ad682924a364dcee96cb078876f735a68410f5919e6ddd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetRounding b97e28ececa9749df5f4ac0b55e075f603d6e59373040363f498bf54c9ff009f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontScaleDpi 1cb4ac465c5555707f76ccbcb015ba6db5d1f83f97792de13072f0280d08f4be -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontScaleMain 7b6fc1425ff1bf01ae41183fd6150d5d05ddf12a5cb38d13ba1da89959bbdf80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontSizeBase e3bba90f5e86991bf82756964499255c5ae8ac21bcc3cea9c16a1f7edd5780f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FrameBorderSize 3a76433a72a3b3efeb2d1cd11014d9e6fa995321aa83c38cb27c9db9b749acfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FramePadding 526515e352f5c94cabe7320149ef3089c4f67799987aae03467526ff05e3d2ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FrameRounding 9969c634946329355ba47bf59a3d47f46d668662e00945917c4fb58af4c06fbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.GrabMinSize f23859f15d8e73559d8cf25abd991e5969444dc691a2056dcf21d73f8b5cae20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.GrabRounding 7adcbb67f47da8cf436c8fec939b1cf0f8d436ec4b10db6c03b8409a248951dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverDelayNormal 8a880111ff95e288f369825cd378f670396d162fa476778530a4dc8dc9fb5e16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverDelayShort a7b0ee335ded9c3837bdad65f37db48007c5b5dda3c4a29ca01927b6bd777488 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipMouse 2c04469c0857cea169d7b19021bf439991fc13e8f4824c0cbec99c3d62566ccc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipNav 29d5d0877340e7f4780ed66517b9dfef8e12ea62b56b6a90b4abe6f3ba1f7d78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverStationaryDelay abafbfc93f083e8f672efda0bf2977882b9d0ad1752f56a2a2f1d3b426749b63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ImageBorderSize 494ba1fdf338e2a5d65821edb232809e3d24646247bbe12286d0985574202c18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ImageRounding f8fa0c424534e6268b6c366e0acfba437b1d432a7bd11ee446b7e610ad689a07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.IndentSpacing 69f1aac1ec365816db88622feb14c12444c2df3d0554ea0de9077305b1bf3960 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ItemInnerSpacing 1e473e5ee9b39ebb4ce1d69daeca1e4b46a181b1e7a6ddc7e6737ce8d127fd17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ItemSpacing b011c4b2b7f9af8ca8ac79d4424c9a4ffed36224c90a4a545d8d7de90ac3598e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.LogSliderDeadzone f611431807c74660ba5f163f240befd88a7f8b9b31d62fe480d04991fe094884 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.MainScale d38bca700c024ba7cefd6f37215c3b8e5ef77c5435963a69aad290dac2564829 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.MouseCursorScale cdfabe442cd9cc81ce676151e990563bbd0ddd86a6fe3ae728fc8e7418f508fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.NextFrameFontSizeBase 2048b9b57c8dfd97a146e3d0e6171dfb9abb824b642a8eacaba80e0fa41b742c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.PopupBorderSize 0b23f022257662466c1cae6e726e8cab213b5e46e50e7616b33b945e759ac083 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.PopupRounding f79f789fc3781c57d8ab6c1015f4506a9cd9d5d5efb45c591f7a1fd1a4e9360a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarPadding 49fca0161a6e9867d91b916de21549f7f0bc3d3847eb35a49092a891995fa1f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarRounding beb2265cfc77efdab799fa0cdeb7860825222703818727770f97a6823c670cae -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarSize 7b1b91446c51ef08a48ed73800e1c35d47d1f5e21f06693d852af92633b35e6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SelectableTextAlign 10f0240fda1ed910cd8795ef8e0cfb1244949bd6b73e0cc7ae8f2e2609a720a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextAlign ad5fb12ae3f9fa8e3200729bf1498fcf673ef5f53a9f8f2ab3b651aa3703086b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextBorderSize d13366e8c909c593c14b51f23bac6ae1478bb3a8b8f1f3b00cbb29a1d3b2ec45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextPadding dbce92036034a4b4db06909c065fdbf2ec8b37f966a5264d146f02179e67e86a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBarBorderSize ac9cc4fde4ccd7e2a5da3b2850292689fbce8fc616e05c5d3de508f104e62bad -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBarOverlineSize f345b6b2ecbac86950c4443591280151961f5630b60fb388e5dbadd04cc5dcd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBorderSize 43fe43cc7ed74b013ca4b8c355df46a9fe1adde96dd6dbb962f1756cc22f2743 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthSelected b69e5aae8795cce729443d84592a4379b2bd03140d8e371456952aa470913fe6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthUnselected f1e486ee85268b9f780e91998b019daaec08c14bd00820cbe7f738baf52f6ece -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabMinWidthBase d3c69f8b760ef1fac741d0d531701ad34cd4628df6c8bbc3d16333e9b7453e52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabMinWidthShrink cb43cf34a54371764dd4a668ed78c7069cd5e1040004b00a4d40a45c62d058ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabRounding b9e74be6b7118e591e3779872738faaeb8850f797b207f5e247ecf8bc1506577 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersAngle 603452fbaced36e5d772ba39f0b5599ba98a601d74d9fe10f775bc9bf2a1fc43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersTextAlign a409eeebe5ce601beb80dfbf330320bc7452137f1629fabce119a21c1329b185 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TouchExtraPadding b45c0955660dc744465f331e7d767ac667da38ff42c4e4f8d2c26f5b64f97c6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesFlags bd37306a45d67b695d3a40d098f6ea23423e5f8c876f09bf265c92640537cb42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesRounding 66436947becac494e90b0b19562d2f50c27f90af919293e5f743e0e7c1876635 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesSize e07bfa960b9ed536451a10e75269ac606c92be58b23f521dea99a7f39496efaf -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowBorderHoverPadding 0b3fca86ff9812bba76f99bdb064420af7b67889b6e71884d0d69adbff2dd1c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowBorderSize e4a0b750ec759e2027e0e84350ea5bcf29ea3e73a7f30a49fd2b2ca512eeb725 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowMenuButtonPosition fac317691348544c8d6abfa6e0adaae81588545145b6522aa1f41a99a63ef43e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowMinSize b491d3d07ac6e592c73778cb6611dd5a7631c1ee14e6e96a836c9bf69c21a99c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowPadding 206b347cc5e0dff3d00ba79aef72e6da4baf1f71533c06bd505cbda16e77a7af -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowRounding 2e59ddb8ddf997446391d808d72ed3fcc1ac7b7feec59538fec07e7536fd2e0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowTitleAlign 211f7e8bab3dd9c28a4c2ad9d44c01ac204a6d2525ce5ca5e274d69b14e0fca3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.AnonymousField1 f1086096f74dabd7cadeb11d27411f4cce0272573c62b06329273a33d045419b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.VarIdx 9ae2e179bb4c32d5140d4b4a72dbe0553a837968b3274fa1e886865e0917750e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupFloat 21516b3be8b953cbae12327770447aa5eae76249f2e16d3a57dd4e487deba20e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupInt 5d2cf21cb7bebd320c6f9f75b1731d4ef7acc52d425cb2d7f832d7eb817247dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.Alpha 505143a19231b608333a1244b2fa61639623710b42cccab51873f46947c790f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ButtonTextAlign aee08ceccb94ebbdfaa2322da4d9ae62c7b1613947da24831dad34c15cb07d52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.CellPadding c663828fa0e833250d8afe9451c45a7760f5a405cfc7c8bd82c036e01e1fbb56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ChildBorderSize 4fda34a77d2afea697ef321ae567bf2d6ba966adb8b3a4bc5b99142dc8947fe4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ChildRounding 531bac72fad0e0aee36b34a5115190af4a8cc40161dd6a7ee4ab80977950b337 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.Count b234cd1025ea132a66bbd51f21fccfbfc175392449bfafe7b1858a056c630870 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.DisabledAlpha a7a8e556559a46155df3a1b5bcf73a253da96ba0b0fcf958fbd61f009fe5ac60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.DockingSeparatorSize e2efb33680cf3ede5d8db52d70e5a2158e022d1a221821217273020c4c787f9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FrameBorderSize 40fab9c62d666271c77a4d17124a1b74cb211fa0b9f431bce874589c59c765a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FramePadding e4885399d90ef43285fbe93edeb234bb9057df249431ea24bd6ba7a71a0a20a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FrameRounding 14f62ddbb2526724f4af727d75b8e2629aca932b56b04458659587f7893cb1f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.GrabMinSize b9053ea3cc69de2481c26d82953cdb12076bf2d23c508f28316201445e0230d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.GrabRounding 6b6a51384257b07da1cf360ab6948f0888b54e1a8055906d7fe051dae2abc87c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ImageBorderSize 33b03ac7980a6586bc7ef6e8481327884ad36a07107a7d27ffcf8c67dbf72090 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ImageRounding b80b38ce095e0f48268986fc0f15709b4b383394b6a76fef4f17bb1c13ad9d43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.IndentSpacing 8e094af1603cfd80d66122cb8919b66bee0e4c09c819e3750da930d03fe77c8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ItemInnerSpacing bc6f3bc6181f11cf3dbffa36ab87bd142badd95410dad2b379a7c9ebe3ca0ad9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ItemSpacing 9feded6639ad590149802298230556b773d21c4368d9e1e7f8e75999b949fe6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.PopupBorderSize ae2e32786ede1e320e0754cdac506b11b6391e3d5222594bca0712f091422ed0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.PopupRounding a3d7ddf6a43bd64cae13df9f7627d718132797cbfbe4ac7a3b4b7743aaa1a7fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarPadding 1cf57d6d937e629c2262cfb9405c334c156de6b48404a437580f7876d0091350 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarRounding 18ab6150a04d0bbb852b6231e517db0e6163bf03534f9772ad60c89bcd7cf566 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarSize 414838963846bf4362bb73395c916f8ffd3e43ab6e9ee0ce0a6d3b1c768efd32 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SelectableTextAlign 4ae369d792d57c4e5693265480ac8474b3abe991efdc61e1d5117530d8f54534 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextAlign 2c9b61e51949299a8e390c5a674b1f2c96e5fb100b1a58dfe4e6b3eab7bee9f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextBorderSize 00c91d52c298d07e512ffe9023194311215d84c5ee832376052044b3eea81850 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextPadding c759093a8efcc6c97ae770367c622c46c4716438b1857f122e9169e884d161c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBarBorderSize f10762e3945939f713a5459f650475a613c7f4877610cdf633393c7a2ed8aecc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBarOverlineSize ee10862b0db69155515728b0082ac586e1cb0363284873815c0b95f1f5b0eb7d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBorderSize 1815ba7ccedf48158bb91d5ea494510b349b699a2209b35073e7d3f966aedd10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthBase a669d6098f6710ce0520b077fb430ab9adcf58a0608ec865d551466106f1ccfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthShrink b8c148700898ff9beb73b88a537828e6869d22219fe4f8da2fe9f4bb4b9021cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabRounding da3da2f88f137d3fc056318944c14dededb5d19653f27793e92db9131acbecbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersAngle 676adf7ed3fdf2c1c100fc40123622b422356b3a0fc0e7138ccab353a9990ba5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersTextAlign 5260ee4b80d7098b18df51b054be9154c81ba90e12f38f57c3d14992656f37de -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TreeLinesRounding df4db1182df980b67ddea878eaeb5360a83a03299233eb7f0f54bbe5f87d2b27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TreeLinesSize 6865d239b7e0f439dce0bd12a0e588118080dd139f45241b542ad21427b0f652 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowBorderSize a8044c3e984b6f76f02c7d4454c6d7c887a050ede2a6aa0aaa05503b9263257d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowMinSize d6ba1749d12047d8fd882d7134b2ce8e1c41d58842be18fd2cad57e27da2387b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowPadding b26f703855cffdeda52c34de761d74aaba8232492af4808b81ecd70414d4142c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowRounding f58de425c6800fb21d5d68e0c9333b21f40533350355725e842fe62dd7081a82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowTitleAlign be8580d6c6d457bc5a028d1ffa310733c4919a3275b0d5ec57f38a9c7758c403 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BackupCursorPos bf56942db19fccc07c7868140002684b9d015f2ff5083156eddb429f032b669c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BarRect 1c925581390a17f55c0b18f09da0279a7425e4b20a2928cf19f605cf42d024fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BarRectPrevWidth dfd50973ea3c5a20bb349d9d054f56021ede92f5d7ac00607bc7f9a19904ed86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BeginCount b6d99f6decae2f168d2f5a7d376b1c1b07324173cb8541fa1e6507bbaccfbd78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.CurrFrameVisible cc08c3258f59cb2423e22208d7c7d2a006be8e829d0d200ed370dc2d1c81eec7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.CurrTabsContentsHeight ed693ac1b0f9f27393eca97b76c193670bfe2b37763359d552be08456cc32ba4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.Flags 50f1ef9df32d17de8b1d0788d19e9aba83996e5768ba1335c223b296d98181be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.FramePadding dd04383d83a399167fba849bdf385fc50a7ae09063ba3ee07cb376fcd59fc839 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ID 7bcce30541e6120a2f1786f598d60f983953a7c6b52e7de1fdaa9929c077bbba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ItemSpacingY 18d62ae7a1651b9be5e6460159f9c6fadeb3b38dd7cc8f3a0bb4992004355810 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.LastTabItemIdx 3e33ea94053994405008cf1820fecd2cb0ddb025d7e5d876c67dfc2b5b3b37b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.NextScrollToTabId 98f7e80123c1c7a8d501d13da15ac7c11881299e000a4740904ea1fcc9197830 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.NextSelectedTabId 4e3a842481974832a61e8755585c9f8c406834fb09a80f82895ec2f00dcf6a9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.PrevFrameVisible 95da69035a2d071826b33ccd10b018d6c9cc6b949592a537caecba22b65f43e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.PrevTabsContentsHeight c7637cec6c40ae986895962b9edcdcac68109aad8c00894994871224efbb3c3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ReorderRequestOffset c18467facf18648d9abc6d7c1e84c6cdcc164d18c66355fc217052eaf407db03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ReorderRequestTabId fdc8f0d15c659d319e56481f04b4dc3ea600f6b5f7e0260a9b5c2f1406bd9418 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollButtonEnabled 806689d3c66f0bf8738cf41cce0890df9f781f9024cf38f356885f36b6baccff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingAnim d584af7d0673286b4bd7f852fe604900af243a5be54a17be4f0f85faa3a0e0b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMaxX 04503f407a66da3c29cc1740c64542910849994de0d5c5e7b24bdbe5d807c83b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMinX a9536fae96436de5f8486d764c91596365ed9dd0dc469f52ad79ca0ece38b594 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingSpeed 0f12b056e2baf2e03a70eb055c86406383620e9717592841247b9a37ef637ba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingTarget ec0db00b1dde5209e48f6ac206bf9217dfdc7afbd620ea142c5abe251bc5e780 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingTargetDistToVisibility 17a1422d1c417516f25ff6447c881ec22335521a9fb078551317795a7d11ed7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SelectedTabId 2cf70c281d3e26a95b61fce5df649ef270299a43e4a48b40e5b547c33be8dd93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SeparatorMaxX b37a0567bcdd239b5b584b5e11c072ef053b1552caae3935fc56ac366adb7750 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SeparatorMinX 2041f2c5abfeb9cbe03fa39ea6bd0be0d6dbd1ab8ee3629f8f8db4beb1123d9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.Tabs 05b1c07fe7f540f5f1a9788bd3ac8585b87b6e42349e48d97e4a25f4b2c96e32 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-3ee0b83ff3fe311d"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30629) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2ac927753f860d1f"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.AllowDoubleClick = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30649) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b3b3400cc855336"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.AllowOverlap = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30651) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b3c8236acc5c265"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.Disabled = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30650) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bad567b4234aef62"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.Highlight = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30652) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0bfd360dc615642"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.NoAutoClosePopups = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30647) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-635157aedba687c4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30646) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-61ce9490ca0cde3a"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.SelectOnNav = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30653) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08000ad116af3cd7"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.SpanAllColumns = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30648) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e85fb6e411fa0dd4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoHoldingActiveId = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30661) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd81ebc6f3851a85"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoPadWithHalfSpacing = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30666) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-deeea01cd6a33a59"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoSetKeyOwner = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30667) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-59746736764f59d8"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnClick = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e489f7a606d017fd"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnRelease = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30663) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-775a9accb64f99f4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SetNavIdOnHover = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30665) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-86b4d34bece39957"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SpanAvailWidth = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30664) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4868d0dcfb2f0427"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionBasicStorage*, int, uint> Inno.Native.ImGui.ImGuiSelectionBasicStorage.AdapterIndexToStorageId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30684) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a0c48a047497df1e"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionBasicStorage.PreserveOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30680) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c7bd8f1f3de9b5b"></a>[`public int Inno.Native.ImGui.ImGuiSelectionBasicStorage.SelectionOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30686) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff910581c064ff95"></a>[`public int Inno.Native.ImGui.ImGuiSelectionBasicStorage.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30678) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-27e7ff090443d8a9"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiSelectionBasicStorage.Storage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30688) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42a4915237174a46"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionBasicStorage.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30682) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-639dc2e40fe89d03"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionExternalStorage*, int, byte, void> Inno.Native.ImGui.ImGuiSelectionExternalStorage.AdapterSetItemSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30708) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3fb18a3742e5ef61"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionExternalStorage.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30706) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c732e00715c56833"></a>[`public sbyte Inno.Native.ImGui.ImGuiSelectionRequest.RangeDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30730) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5bc01d7067aedb5d"></a>[`public long Inno.Native.ImGui.ImGuiSelectionRequest.RangeFirstItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30732) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21a9cc66509d1654"></a>[`public long Inno.Native.ImGui.ImGuiSelectionRequest.RangeLastItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30734) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd76bcbd92be6db5"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionRequest.Selected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30728) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-43a259c81397745d"></a>[`public Inno.Native.ImGui.ImGuiSelectionRequestType Inno.Native.ImGui.ImGuiSelectionRequest.Type`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30726) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d4857f5b6939363"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30749) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-84eecf5aaef985b7"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.SetAll = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30750) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fbc7d82123b83360"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.SetRange = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30751) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9543a3e5f1328e2f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.Horizontal = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30760) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8b36144ce4cd07f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30759) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89345f9edadb165f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.SpanAllColumns = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30762) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-98c8bd48b433031e"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.Vertical = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30761) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-346e117589a8ffd2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ApplyAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30785) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d6cb4e600042bbe"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ClearAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30777) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dfdbc831f81321dc"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ReadInitFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30779) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4232ffd44543f674"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void*, sbyte*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ReadLineFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30783) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c4450f3054f49e9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, sbyte*, void*> Inno.Native.ImGui.ImGuiSettingsHandler.ReadOpenFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30781) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a0532820394aedd"></a>[`public uint Inno.Native.ImGui.ImGuiSettingsHandler.TypeHash`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30775) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9458ce627c515ac"></a>[`public sbyte* Inno.Native.ImGui.ImGuiSettingsHandler.TypeName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30773) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d04859f133f6027"></a>[`public void* Inno.Native.ImGui.ImGuiSettingsHandler.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30789) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c23c45fde6bde39"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, Inno.Native.ImGui.ImGuiTextBuffer*, void> Inno.Native.ImGui.ImGuiSettingsHandler.WriteAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30787) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bd1ef678472e8df7"></a>[`public int Inno.Native.ImGui.ImGuiShrinkWidthItem.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30807) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b864fc20f2d9ba32"></a>[`public float Inno.Native.ImGui.ImGuiShrinkWidthItem.InitialWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30811) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e944f6455fd44621"></a>[`public float Inno.Native.ImGui.ImGuiShrinkWidthItem.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30809) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d442d220ed35e07d"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiSizeCallbackData.CurrentSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30839) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd10c8cc259e2a7d"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiSizeCallbackData.DesiredSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30841) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f33a1facf69000ef"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiSizeCallbackData.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30837) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-20dd9de962a805fc"></a>[`public void* Inno.Native.ImGui.ImGuiSizeCallbackData.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30835) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b20c87c52c5b8e2a"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.AlwaysClamp = 1536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30865) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93e9795cdd25cd72"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ClampOnInput = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30861) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f4f24eb67af2604f"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ClampZeroRange = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30862) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88218c2539a4fcde"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ColorMarkers = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30864) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d81c235421dcdaf"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.InvalidMask = 1879048207`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30866) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-98509f542dc5e1a1"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.Logarithmic = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30857) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1b73c2eddbceba3"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoInput = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30859) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ebe87c4f0af59c04"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoRoundToFormat = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30858) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ab881b9f26ab1df7"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoSpeedTweaks = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30863) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a4ab044b219dd5c"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30856) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e6a6876847099d17"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.WrapAround = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30860) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f9d93215fa071f02"></a>[`Inno.Native.ImGui.ImGuiSliderFlagsPrivate.ReadOnly = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30875) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09655decd92fb245"></a>[`Inno.Native.ImGui.ImGuiSliderFlagsPrivate.Vertical = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30874) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6fd71dc4e707eac0"></a>[`Inno.Native.ImGui.ImGuiSortDirection.Ascending = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30884) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c38738a3fe33dae"></a>[`Inno.Native.ImGui.ImGuiSortDirection.Descending = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30885) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2885d34ac884f449"></a>[`Inno.Native.ImGui.ImGuiSortDirection.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30883) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e7ffa5040467b8c"></a>[`public sbyte Inno.Native.ImGui.ImGuiStackLevelInfo.DataType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30902) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-894b214a2b2bbca5"></a>[`public int Inno.Native.ImGui.ImGuiStackLevelInfo.DescOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30904) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-34d24e3ce2f1145d"></a>[`public uint Inno.Native.ImGui.ImGuiStackLevelInfo.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30896) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d844c334b7dcdd62"></a>[`public sbyte Inno.Native.ImGui.ImGuiStackLevelInfo.QueryFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30898) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bd51c14691f16c45"></a>[`public byte Inno.Native.ImGui.ImGuiStackLevelInfo.QuerySuccess`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30900) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-76a26838d1f3aece"></a>[`public Inno.Native.ImGui.ImVectorImGuiStoragePair Inno.Native.ImGui.ImGuiStorage.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30922) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08bd42b5a3895f8f"></a>[`public Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0 Inno.Native.ImGui.ImGuiStoragePair.AnonymousField1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30942) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0542418d188e0832"></a>[`public uint Inno.Native.ImGui.ImGuiStoragePair.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30940) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13d931c67ba20a1f"></a>[`public float Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValF`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30962) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7278fb3f0592c760"></a>[`public int Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValI`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30960) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-595623739eeab3d5"></a>[`public void* Inno.Native.ImGui.ImGuiStoragePairAnonymousUnionAd9ee15fe77a7ef0.ValP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30964) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6ef591bb0c0dce3e"></a>[`public float Inno.Native.ImGui.ImGuiStyle.Alpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30988) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b07068c43f0fb233"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedFill`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31108) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f599aa5818a59da"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedLines`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31104) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ea026ae9af1dda5"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedLinesUseTex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31106) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5722ffb5cb47d962"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.ButtonTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31084) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-41c90bbf0ee50b47"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.CellPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31024) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-994c912497ea8ad8"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ChildBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31008) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4179c5543f51fbc"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ChildRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31006) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c86e0b0e6084ff5"></a>[`public float Inno.Native.ImGui.ImGuiStyle.CircleTessellationMaxError`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31112) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64d570b6a983c24d"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStyle.ColorButtonPosition`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31082) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d17e68bab6396d47"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ColorMarkerSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31080) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb5e4bfd1364e08a"></a>[`public Inno.Native.ImGui.NativeArray_afa9ec5b5680f876 Inno.Native.ImGui.ImGuiStyle.Colors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31114) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b45c626999de213"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ColumnsMinSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31030) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6e118ce57cb59e3c"></a>[`public float Inno.Native.ImGui.ImGuiStyle.CurveTessellationTol`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31110) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fead72db9eb52e99"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DisabledAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30990) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f03d779c27451eee"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.DisplaySafeAreaPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31096) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e6f0ea87a47f7526"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.DisplayWindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31094) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89a57e5c0450b4dd"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.DockingNodeHasCloseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31098) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-95e02ec03b26164d"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DockingSeparatorSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31100) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-163a2ecf8e7e9493"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31076) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a3e2161d1adb92bd"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31078) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-240ef39e41989482"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31074) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e3370b1c6643a08"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontScaleDpi`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30986) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c04cc26d02191d7"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontScaleMain`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30984) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-71575ba54d1d5ebd"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30982) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-acc47f9fefcfe033"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FrameBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31018) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fff4fa87263022eb"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.FramePadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31014) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a640d64dfd1fac3"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FrameRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31016) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db3411846e904b96"></a>[`public float Inno.Native.ImGui.ImGuiStyle.GrabMinSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31038) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c175d31c4fca73fc"></a>[`public float Inno.Native.ImGui.ImGuiStyle.GrabRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31040) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fac00b8e160c3335"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverDelayNormal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31120) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0db3459e1786ee7e"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverDelayShort`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31118) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba9d6dd2f65a4052"></a>[`public int Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipMouse`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31122) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1cc88ee28439430"></a>[`public int Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31124) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff3fbe9e600498c3"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverStationaryDelay`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31116) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6990e2649707f5ec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ImageBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31046) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cdbeff14e4b2bd2f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ImageRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31044) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-046e488af5e97e09"></a>[`public float Inno.Native.ImGui.ImGuiStyle.IndentSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31028) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83e9202f67d39b55"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.ItemInnerSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31022) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-45ce9f00abc10b06"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.ItemSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31020) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21c28c24b92b2e96"></a>[`public float Inno.Native.ImGui.ImGuiStyle.LogSliderDeadzone`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31042) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-553d5ba6ce3ab326"></a>[`public float Inno.Native.ImGui.ImGuiStyle.MainScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31126) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-664c3314f63fc258"></a>[`public float Inno.Native.ImGui.ImGuiStyle.MouseCursorScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31102) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2cbfcce9020cb1f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.NextFrameFontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31128) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7607553eb09b91f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.PopupBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31012) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9aa1d3f6f683f5be"></a>[`public float Inno.Native.ImGui.ImGuiStyle.PopupRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31010) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e383f90281f2a8ae"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31036) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-52f1864181ab3103"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31034) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-075e79c6474e5b35"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31032) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b786b5e793fa9e1"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.SelectableTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31086) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b148f428863522a"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.SeparatorTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31090) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e670f22cf167c056"></a>[`public float Inno.Native.ImGui.ImGuiStyle.SeparatorTextBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31088) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-51973c57077fe667"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.SeparatorTextPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31092) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ebe5554e15a160db"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBarBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31060) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d65cf64fe9f2cec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBarOverlineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31062) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db333c541726c49b"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31050) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7e09a3979fd8e62"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31056) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4e40f7462410644"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthUnselected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31058) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9bde22232725965a"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabMinWidthBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31052) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f71d1380475d8546"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabMinWidthShrink`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31054) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89d9f0e5473a9b0b"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31048) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8de2ca5ee1e84ec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersAngle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31064) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13d39dabb5fcdd18"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31066) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e99c12bdc53eb46"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.TouchExtraPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31026) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42e20a72e3a6f369"></a>[`public int Inno.Native.ImGui.ImGuiStyle.TreeLinesFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31068) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c5d43e108046b37"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TreeLinesRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31072) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-600ba61660acf748"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TreeLinesSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31070) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7c8f3b399b71d10"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowBorderHoverPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30998) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4309b50615a5f17c"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30996) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0969369746e519d"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStyle.WindowMenuButtonPosition`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31004) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ed37898ed8ec126"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.WindowMinSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31000) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-038bf9667c158bcd"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.WindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30992) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c836bb5efa11e76"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30994) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-53b712cea320be03"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiStyle.WindowTitleAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31002) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9557dbd2824ff960"></a>[`public Inno.Native.ImGui.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2 Inno.Native.ImGui.ImGuiStyleMod.AnonymousField1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31148) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a75b9abf0d0ba9d"></a>[`public int Inno.Native.ImGui.ImGuiStyleMod.VarIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31146) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-427559b9db0340a1"></a>[`public Inno.Native.ImGui.NativeArray_3571fc580e8d9e55 Inno.Native.ImGui.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupFloat`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31168) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8a2b61e41aba5afe"></a>[`public Inno.Native.ImGui.NativeArray_b3004bc9c15942c6 Inno.Native.ImGui.ImGuiStyleModAnonymousUnionDf6ebb975d3ca6e2.BackupInt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31166) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4178bd418c1842b3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.Alpha = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31183) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f458270b0cbf4ce3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ButtonTextAlign = 35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31218) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-36b231643629a070"></a>[`Inno.Native.ImGui.ImGuiStyleVar.CellPadding = 17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31200) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-30161d591c9b5460"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ChildBorderSize = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31191) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c006d4cee3106a5"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ChildRounding = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31190) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d23b52a51829094"></a>[`Inno.Native.ImGui.ImGuiStyleVar.Count = 41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31224) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0b181b52014f2fa"></a>[`Inno.Native.ImGui.ImGuiStyleVar.DisabledAlpha = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31184) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-95113eae2a08ccd2"></a>[`Inno.Native.ImGui.ImGuiStyleVar.DockingSeparatorSize = 40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31223) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3a63ddfe53af5d0c"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FrameBorderSize = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31196) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a0195db57e16e84"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FramePadding = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31194) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f0dfe068c67810c0"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FrameRounding = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31195) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-49f2271008e0cacd"></a>[`Inno.Native.ImGui.ImGuiStyleVar.GrabMinSize = 21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31204) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e26e73019b19d0f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.GrabRounding = 22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31205) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4b8b0d7bb7e4294"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ImageBorderSize = 24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31207) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-105ef4c5c851f115"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ImageRounding = 23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31206) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-127ff08efd6c1316"></a>[`Inno.Native.ImGui.ImGuiStyleVar.IndentSpacing = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31199) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a33dd420e4d6165f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ItemInnerSpacing = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31198) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5f3ddaee458f6de7"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ItemSpacing = 14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31197) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f22fd2591fe8948d"></a>[`Inno.Native.ImGui.ImGuiStyleVar.PopupBorderSize = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31193) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f44119cef0700e94"></a>[`Inno.Native.ImGui.ImGuiStyleVar.PopupRounding = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31192) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-31265abf938800d9"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarPadding = 20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31203) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62d5adcfd404b473"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarRounding = 19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31202) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42760993743aa03b"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarSize = 18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31201) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f56f7265999278f8"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SelectableTextAlign = 36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31219) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-19883bbd25510880"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextAlign = 38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31221) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-500b80266597e861"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextBorderSize = 37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31220) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da6254d0c3deb9f1"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextPadding = 39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31222) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f660e8d97ad0015b"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBarBorderSize = 29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31212) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5717698e19c49113"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBarOverlineSize = 30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31213) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-17a13688cac42cde"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBorderSize = 26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31209) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d59dd0f0b7c9894c"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthBase = 27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2267e41608dab3c2"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthShrink = 28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31211) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83f3b34009692680"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabRounding = 25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31208) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0957e1a17f57d09a"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersAngle = 31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31214) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0bd417bc68fe7ec"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersTextAlign = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31215) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d44a5d3357a9013f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TreeLinesRounding = 34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31217) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-59a9ddff2c5424c8"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TreeLinesSize = 33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31216) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3516af8c00967a88"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowBorderSize = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31187) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf4848e9285f9ed7"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowMinSize = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31188) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1cb7c8795e873c3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowPadding = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31185) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-110cde35a826c3af"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowRounding = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31186) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c64462a281251aca"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowTitleAlign = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31189) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67b14375a67a1e91"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTabBar.BackupCursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31411) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a517d8d61a1baa6"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTabBar.BarRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31361) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce51e903f66d9817"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.BarRectPrevWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31363) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c49473fc23be1d1d"></a>[`public sbyte Inno.Native.ImGui.ImGuiTabBar.BeginCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31393) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c51ee8b385eabc5e"></a>[`public int Inno.Native.ImGui.ImGuiTabBar.CurrFrameVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31357) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-039ce7855bffd2c8"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.CurrTabsContentsHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31365) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2cbc61f7878b5c8f"></a>[`public int Inno.Native.ImGui.ImGuiTabBar.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31345) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bedc89ac523972a"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTabBar.FramePadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31409) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5220e152c01a7369"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31347) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c09fac665a2d1ec1"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ItemSpacingY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31407) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c11cac5c36eda80"></a>[`public short Inno.Native.ImGui.ImGuiTabBar.LastTabItemIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31405) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2385839a1804fe0c"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.NextScrollToTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31353) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb3ec40cdefa30cb"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.NextSelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31351) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e3c900c4c2e96d8"></a>[`public int Inno.Native.ImGui.ImGuiTabBar.PrevFrameVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31359) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a525bdaac392ba95"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.PrevTabsContentsHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-907cad1ab7055bb4"></a>[`public short Inno.Native.ImGui.ImGuiTabBar.ReorderRequestOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31391) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e19aba7011f3651"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.ReorderRequestTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31389) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83fa12d722e2b39e"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.ScrollButtonEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31401) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39cf5a6035ebf20b"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7c01f9ba2891df3"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31383) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db80af7daa2f8fe7"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31381) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aac19d4dee97d40f"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingSpeed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31379) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ddd18d777f53fad1"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31375) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-58d7ae4aa3cf9280"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingTargetDistToVisibility`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31377) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d127544ab5897478"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.SelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31349) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c33a08a2857bbfde"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.SeparatorMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31387) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f2e566a808c1ef8"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.SeparatorMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31385) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed1d33c0bd121eda"></a>[`public Inno.Native.ImGui.ImVectorImGuiTabItem Inno.Native.ImGui.ImGuiTabBar.Tabs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31343) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
