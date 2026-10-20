# Inno.Native.ImGui ABI · 07

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DisabledAlphaBackup eefe0fb9256e2902c442e5e4f35bf0da9ace382d7dd8ddb3264961bee9640678 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DisabledStackSize 13f2b73c4c5f91c86aa9853129abc8e27ded4bd228dd7425300b9a51c9c6275f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DockContext c58b6c1c1827a5d609208cba3596410227ad2a6f3498f6a02eda8673e9a74cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DockNodeWindowMenuHandler b61c1fd70776ae564195707b24455270e31f7e20b4ceba43ec4373ea27382b4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragCurrentAccum 357fc8fa071373182d68197b328a132c77a065d8bf3b8f81c000f5fd0217119b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragCurrentAccumDirty f9f972a3f4c93785492ef3a5bd851c8c5bf815607eae14912a2a71851e5e9a75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFlagsCurr 048b6c36a054d80bd38e5acbe5d3c9661a2b9beca56f1fb45eb2d3b9ec0b68cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFlagsPrev 84af3768e9ee40366eb7728872f0c7d72a93839fd4e6925b632059c36d21a435 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFrameCount 92e977acd6b7cfe72a93adedbce857204017f8e704444669d81986cfc9156833 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdCurr f68b2a24a01db95e62a374a59208f494cc31e266dcc4f7f99702e9c118c1b904 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdCurrRectSurface dfca3b2ea7d5de04169ef18a0057fd7e71e48e3a23cc74a069af645215388d3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdPrev 17c23992172b0d3800c98e7fbadcb8dd751aab124eece9e4bb3b6817769cd5bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropActive 7fe4adc3abac54ed4312b1e5c9ca63353eb3985c62c6fb37019f90b65b00662b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropHoldJustPressedId e4ba59a0dc5b4332bd0b02aa91ff18e12c8fd309c61b3ee32bc99dcee6c9be56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropMouseButton 3bf7241eddbb117af3c283742bdaaefc11a87f877e08236e851390961b85e55e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropPayload 0ec45981d0d9da0aaacaf359ee0ac1c42eff0898f2f483d37c3ab3afd45091bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropPayloadBufHeap 0af86e3e2f52d0648e8c177b0f0ded29ec723bd078b5c22b2b60af84379cf038 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropPayloadBufLocal 0b29cd1bfae0e91b94f74b29d03ae14b6baea4bdaf5761e6db9a1ea4b0858600 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropSourceFlags d923e0e75cdeff58b451a83bd5bba98cbdcc760d0121d1b85faf768868926d6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropSourceFrameCount 23d1f48fb5b6fec3f028255174ba188f42b78b170813565972890038e1ee2bd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetClipRect 7845401d53cdb00e680bc01004b5deb612b4d2642a64e2b2007d49ea16f43082 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetFullViewport f4817d7ea30b8683052a39604dcd6df5ab6cf6b9d42b3525d63e42988b1bae13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetId b209fa24a7df4ffa701ac92dc974766df642f7fa54ce9c955a85f0384038f318 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetRect 13d378bc6695120bd6c5277e04c6aada97c5f1aea5c0ab4148e23048ccb382e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropWithinSource 75807c0ff93161ee3a8813edfa27f9828ae97a0abb3c1aa5e0bd7ef2c3405e6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragDropWithinTarget 06ed870ab1eb4a26ea9f50dca31d9e1927afb2a653a0982e2d222ee4164f6f5e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DragSpeedDefaultRatio 6583505313add97378719b85f3e214d7aaba64a50176dc71f6ed4481a5898946 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DrawChannelsTempMergeBuffer f23331f21eedee41a76060d07038282e6e685a0b8fb07d66a29077f5e9e1f701 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DrawListSharedData 13f55d8e924dd877eed8659730f146b03d7848891e39097c92e852b8ed0ef737 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ErrorCallback c848654c868afa111048e812c8248990300e53f5da4e085745e7e80b8bc5bec4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ErrorCallbackUserData b24f53788ac4d9aa7c7730386ff1227565b2fd22b227019b6d083de49bee9e62 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ErrorCountCurrentFrame 2fcbfc2bf84dfd19934f28a78e54abc019f2159fe76e5b0ba8f380e9f6f17f51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ErrorFirst 310ea929fa9b75e6244227e2b85c706b10ab6cbe3b29cd1c893c95e08ee65ec0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ErrorTooltipLockedPos 0d17a631fdd251e74cae5ad56da5da161f9325648ef5aef75d876027a82f120f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FallbackMonitor 94a7189568ac848fdb9deff32deb6b51aa1a6621c690316e037626d2ffad9bbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FocusScopeStack 89b62cb6861b57e608baee65342a4b76023d546f2399a6c08bc5dcdc30a070dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Font cff911bd7ab8c94909ce58645c82d411a2aa5155950a8e550a71143f7383a8d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontAtlases 30e2aa1b23a2a070a239bc6bdc15174f978fe76fe8ce48bf99cd0938e24ba920 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontBaked 1763529dc96e27a87a28ca30c038f97168e4c16227607c4265dacd8a8e028451 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontBakedScale 8fdefb146f390676f7a1601a02ecf419cb88fb5aecb6451434a68e8a52b68342 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontRasterizerDensity 39c5811f1dbbef4b3871da0f5b1788c149f7715f14744b7b0b98c9c3e82380c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontSize efd2991558a79619071535188202c69b509ef59d12608decc356ceb072a06a44 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontSizeBase de25494c3f09ff7373cba41738958625cc347fc8064c5f61aef1f33840214b19 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FontStack bde0ed55df75fdd3e26913d73d0456c0945a0a02d6735ff77d2e2ff41fbefbd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FrameCount 95b5ea2f109f99502e8429ad2bcd174923aff20569135da9a230b26c041cfb13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FrameCountEnded c909de2b1562b58472c65497abc8682c0c4f19fe010aee2e41ef0628f578a1f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FrameCountPlatformEnded 70aa10534f8cd9b6820e22f5a587a6f65ff6adc590c7b7c016941b6a0a1b87cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FrameCountRendered c1a967afe062873ba7f7d89ea8cf2a721fe933b97e0e87720da5b50812f7c420 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrame 246ac8d31d631731e47d7b1a220550efd070fa5f9ed7bc72238d1b1c8e7251e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameAccum 11d42e1ec5b72425e7f94ec2751439dc8673392e0a44e6f1bab0e35c1f872bd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameCount fc1903e317fd26dc371a9f5faadee6cc6762fd484df1ee45c28f0000e0613b05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameIdx 54ca6c6d57e204dd3c812162c1887245864df09511e22feee2c07e5e75a7c864 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.GcCompactAll 7cf39d5bd029a555b6aa62de1e97b42eddb7448e2320a6829c59b6a9538ab197 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.GroupStack 04806775bef7f880945d026a8aeda9e1b512f38b51d5977fdc5df16d4f2984fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HookIdNext 7d2d943c611f678a18d72ff252432ce30cee2d5659cc8f0540daddad007bda1a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Hooks e6ffbe714e9d97082e4edf4c1ae124fde669d0a31da51da997e5b2f9da398e2c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayClearTimer 1a85131172f5d29f00c5c6e65408f26caf79599d22a0ccb699b90e86342b87a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayId 02dc3497b006a9fa565efb362a43ba8c01d47b753f3a8b3ef47e623a34ba65f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayIdPreviousFrame 6762ba7a1a4fc9a5b966125d67a157d360b5c68723eb8fe1162953f6e3a6b608 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayTimer 46e6fe80452d45b9473628ee5be55a3cc568a616ffd906530097722d885aa3d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverItemUnlockedStationaryId 1ea60afdd8ac2189034b94bc1b7bad3c71b4a9a084a1972fb36f7931a607b165 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoverWindowUnlockedStationaryId c2b3df55d5d296c4cefc7989c6259c211436cf49db9522e5af35c31f291444bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredId 09854af699b9f35ce2aa959bccaa67dcdae7a493d6f1651f54d64314ad061aa1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdAllowOverlap 9f26fdc86ed96abd3f4d7a66d58a755530003b6aa1f66ecd928fca163ece7a6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdIsDisabled 406d5a437e0327230e9c81133b303b6371185ab8d99678d3f7d105f5c9ae9d30 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdNotActiveTimer 92b9ee21a294494a4e6191d2d491f702209eed464aa28054d93e5b1c4319632d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdPreviousFrame 9648d401ac2d671ec0f8c604fa00e13e0dffcb9e2c188fd000644b2db7dd8e0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdPreviousFrameItemCount ee76fe92b081b80dfd6425794468054ebcb282a2ce2042be2af9e5c5ead32e3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredIdTimer 81ef0d69e7927232fc92f3c26a349b95b5905e8e9cc185c3845732ecfb0a8f38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredWindow f899205108d9aefb80fefe5518a9ea65e5a46922b7284e9b8629980646909683 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredWindowBeforeClear b602f52cad11cd1cc7918096894564a713269eaccaf5493d973ff621a06b8898 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.HoveredWindowUnderMovingWindow 411de693beb28bf8008041250548d32b493ac247aa222381c458ef1ab07db6fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.IO 39935f4bd0775863133b5e8ae25d842d3bc475ddc5ea48d80c373fb9066ca30a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Initialized 607f537cce204ce0fabc3fed7c38c4234b139071d16e8ed81a7d7a080a6e3ed6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputEventsNextEventId 68fed1c3b2dd168968ec283ae7f397b6c1bdd8721f09df4c03760e4ff89f72d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputEventsNextMouseSource 6fdf1d587e414317a6f642b2efe551b9fc7fc8ba5e00944d7fc2555605132cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputEventsQueue 96f5b547a490af236bf50609ffa317eff319d4a142d22f1d86be90033800a6ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputEventsTrail 5d02f1a1c4eca16cc988822ac40af1e466d850c4843d1a6f4d2db77a9821f96b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputTextDeactivatedState 52cce5233b7c7fcd84edf019557e814d00c439fa8797dae05750ecd5f733dd3c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputTextLineIndex 4a9f7bdfdce597f4def7d2ea7967a1dc1ac02cab4af57cc31a39c8ac7d8f3314 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputTextPasswordFontBackupBaked 46732dbc5a28524d3786ae0d42a9c8fbdea9d1c235ed3f5095b9b0315f7d9351 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputTextPasswordFontBackupFlags fa76a8b5cc9a5065a87a5ff02f2908fb8d0b4e72202e01ab793d46df6845033e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.InputTextState 4b162c1ad796f7e2605737e3a9bbadb03902f5d00ee6d8170a5848bf31dee2dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ItemFlagsStack 7813f1fc027f5f9c16b9cb269edb478bca9ef704d247d7b5903d1aa40ffcbc06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ItemUnclipByLog b9db74c2c54aba2750a477d4fea61f9148e425af455f7b428538dcb323a0a0b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.KeysMayBeCharInput c688abd5d51131484f875e62a13dc37879668450b59557009764afc8798b314a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.KeysOwnerData 37f11b8e161a67284bf8cd4fe9d011e902a82dae9a5dccf31b0fa09a623fceeb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.KeysRoutingTable ed67cd8d1102b01d44f599885f421d456b4c51ef25d8d3439a1bd6553dd4a2d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastActiveId c3bdb45e2c549513e863f788d189db850a37983ff02e85ac8ddbf33d3f91d107 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastActiveIdTimer b2fb4bf58230d65cf0b6c241a93acbff5d688dd434dc9b753d3826117a378a0a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastItemData 75e4b40d623f0841094600f8c23a3deb89455e30f40c0b2bac246080d92ae016 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastKeyModsChangeFromNoneTime bd3ff0f008111b360d51545d082bada0843a389a9ba7868c6818d1fa510cf9cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastKeyModsChangeTime b92b33c52c486ad1ed342c7a312089498ff09bfab73b1d8f269b6c23a6a3f55c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LastKeyboardKeyPressTime 047553e44c6297730684142c8b446dd20a9a45620a615d4f6336173f18ce2fd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LocalizationTable b14ff10988df76e4d98a9adcab808250b659e026926c99659690adb38c2c9d0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogBuffer fce3cefb8db1cce4e38beb2afe8755e1ca505bb6a4f9de70b985925d7e5d7a85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogDepthRef 167f6e62246f9ecb9ebd92d4c3349e5b3434a4b0afecd995c8bc51e8f13753d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogDepthToExpand 45f8d929283f435add32dcba38036eef1cd370cb10d93835de8f898ae5e7809c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogDepthToExpandDefault b3b8ddb8a9a741c32b24ba2f6d1e9e318dd4ce701ae42644b7f325ad70c425af -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogEnabled 782693bf705870aa6aacf8dded583980392152de40f19daaa6a97776ddb893b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogFile bb17a6fa941b4f435437eeb7eddf4ff6247cf504dab2dcebfa9a97212cf8e6cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogFlags 0510e4f795391630ef29b036757b60e892bf9408b291fbff632d81797e0906f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogLineFirstItem f6061130061f28ff433107a5e8dc2138601bb2bbef3780b214e875e1e638e5a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogLinePosY 8ea0302d2c0fc8f7e4c34d89f2df8d32b4d44be83794ade37f9f111c4dc8279e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogNextPrefix 996df4217e937a9eee3fcb86d63124e336cb5fe212f0cd78f2fb5b2543326f22 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogNextSuffix bdbb633a638edcb67934d9ed99ddee764555f5b9e1ad706b285bcf404e9c6e5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.LogWindow aa08a7a94aba73bf067e1eda146280affd369d48e56cd111d5b20c68fbee3793 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MenusIdSubmittedThisFrame 74a849f896ff146a327523c1523c37fb76a2f02753bb91534403dab536d8405d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MouseCursor 86f62825eca267fb5987d594c3fdebb87b06630e240596a4ad5475fc66f27dc6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MouseLastHoveredViewport 1ee28fbdda3baaf016b2ec5adee8f257b8648c3e12f6cf96b6157c91a93dc264 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MouseLastValidPos 8eefdf2300d7cc54f5b244697f82e826208c5d06bf32365d51e829d989a06820 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MouseStationaryTimer 82bccedd17789c731f22a29afda9d015d162f969c800e0da90318d00e88f5a48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MouseViewport 6e441a89f8ba93f6ce0ee26ca94e5b8873f68a3d7333d90ee8121898fe1f5472 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MovingWindow b173f9b647c1a470bb843fb2496d9a74c2ab3dd88c8a0b51d7377cc7ccb765d1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MultiSelectStorage d5645a1f12bab223c7ab7eec92cc7a34683f70907172c65f8d84676e6e9b4336 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MultiSelectTempData f4b2ae0edbf0d0aeb43d49ed3559ddc74b0706fa36cd12df1e3c49464766a1e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.MultiSelectTempDataStacked b66180a4cd2bcd117071154391974aefd85bd809f82b48899fd66480d7425b01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavActivateDownId ee6b364dc93dc4638bbdf3aca0a981f43fbf78e4a0be9bfdd79faa0226ee5a1f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavActivateFlags eb51a6bca5a955abcbd22072934b66321971ac081cf305445a274cdb2af5e013 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavActivateId 61952242aff9130692bb4cdef95773424e88a92f4c5a6366e01fb28b032d7552 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavActivatePressedId 5c52c1afa6e1935367ada78b93dab8cd65ea5091ac5f6598fa3d1608183d5c93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavAnyRequest fedeb41e3e67a3523819bd17ce255e995e449470f31e85a1df2577c4dd656461 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavCursorHideFrames 8b3c392c4069e5058b9e77579ba5aa35ff829875dee063702572aaf3bc5bb3a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavCursorVisible f299c3bb13769dc94b41c4b4a12843b7a01709b318b8c5870078e42f543b2de7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavFocusRoute c0943f932bf8c091275417b65e89fafd74db437a205225b6c8333ea4cb81d0be -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavFocusScopeId ea4cec3a2c76fb9c1b4b1a60008da4885499eb11fdc529ca14281818e54fa8dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavHighlightActivatedId 62ca25a86bc549a1c5b16d2792f5a2ed8ad202040309c7990f299ba94a9b7d5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavHighlightActivatedTimer d34ac256bb19c7eb4eb5384dff86f4925c685d358d92f2436feb1bed958b10e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavHighlightItemUnderNav 81bd19d542f5695f93d4be30fd42c19af68eeb7632bf4439b72a88cd50c6eb85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavId 47b0ebefe776b825d1a91685b6b297f592eb545433e19b90769f70b5ea1df2a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavIdIsAlive c4c5403e5e8aecc58c1e0bfe02a0be7b1a1575d89f850879c5e3619777b2ae64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavInitRequest ee23075aef397762837ab39d0f89e5091c142b3766f7e2db4ec2d4cd0279c092 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavInitRequestFromMove f18d0a9ed014bc1df493a3b3fe001c193f7e841bb03540c78ac3286d471772c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavInitResult 78ea54e95d682bd38bbcf90564ed9a700ac9897b9fe2268e69d4eafeaa3efc2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavInputSource b328fde45739387010533ff6eeb1cc4d437c9cd923b4cf6aaac6706b6bae478e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedFromFocusScopeId 2d7f7620073f040b607325e6cafe15fc6b2df50b8cde941d3219a15f0f9992da -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToFocusScopeId 88e35b01d0a1290eaaea49e78804211074ccc7b46567466405e3e3031657ab17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToHasSelectionData 8568374238f8bbef99d0ac0056de9ac6a31d75d5de785f66a691073693d88911 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToId 17ea54626936f12cd290004a8db7461cd4a6bc7048c1e1f70fd4dd644c53c784 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToIsTabbing 3a128ff6a26ca114da732cd7501286a93c8a2e9eeb31e71a2e8e95dafaacd1ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToKeyMods d4aa252f86f1123924dd709f3b5204503dd53512b8a9ad8d81f2061b12cfa76f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavLastValidSelectionUserData 75784dcd06b3258866c5012a3464d6c8ec9163f2f46b9c51ec292b24733520ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavLayer 2808c1b274a071a4a19f41f90e4faf61ea15ac25b8597d0f02478fe32f34f486 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMousePosDirty 761f4d9d1625178a31af1ce4b310c96b26b1b0eb54b8333d0603ee83fc1699d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveClipDir 7df46dd883bb49b6f0741e0d2b53f8ab1afce88683f3731d794c687822be023c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveDir 1a774c51c8f368df86f281a73226e07cb07c20e2af3e02820c1fe366e72231ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveDirForDebug a6df6b221b2e5600cb84d6cf4cb42f5a263a533664432189812273d4ae33a820 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveFlags 59b72d05f79860219827af2d7057aba13a107961155919cddb6d58e82190b413 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveForwardToNextFrame 2dbeb8a5a9e67a119a31bbec1243f400b56c1fa441bba1b10c751d6abac889a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveKeyMods d347b802a11cb695dd2a6b1ef4368accdc01fa205577858b71e6c66e50d7b0e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultLocal 8dfbf9224e070b606508c514c4e90f4ecd773fb00dd279d754b665387069ce16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultLocalVisible 85777de596883e0765f22b417c0f24ab66784a74bbe456f786e8e3aa07c1672a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultOther fb480fd149f0ec3b8ef2fefc7faf33e6c87aea9fdb778f09fb1f85d9a45b80ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveScoringItems 03d0c251fd23cd0680e96963f851fec8b3a58ca3e42ea72fb8ebf5dcd638b2ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveScrollFlags 08b41aeb49e2e4419e2eb5d89d1a2ec3b979883d4aa6a187524da3f97c7d0e48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavMoveSubmitted e2beb71fd409a7ea059df7ee98c907898c79893664ab0e6b01852985e8755a8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavNextActivateFlags 1cc249f853113ee23e018923a8da72eba87580a949718179ca185e9695931c38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavNextActivateId a9f186f4e90a738e5fc6cbbe008526dcdc448d7d1f80f64f7be649c88c4bbeea -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavScoringDebugCount a3211d4b7212685b5f657a3160358d15bb07884966e8c433b10b1362e26c21f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavScoringNoClipRect d4619fa142f39879aedcd4cfac86632a848a85f8b1b2a8b7b4a6dd8e5333c715 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavScoringRect 6c6ed6baada19eb868debb2f8de5aa7105df10395aa0849d4c99e183a64b9fe4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavTabbingCounter d126da2ec239e22ecec2b09fcb6b78e959f19a39e145d851c15ce94c62067f1e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavTabbingDir c9131190382b16f24c0dcdf270ab763a702d0f4093f0313c72e9cfc0e40304e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavTabbingResultFirst d4c07440532d05f7eef217e98840e6bea6004b460b0b4b73685979510f777530 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindow b18d3f68b2cbc3ac9211946cba8b8e6bb00603f0ef071ce0d6a26268a10cb620 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingAccumDeltaPos eeaf4377fb96fadafb25c9d44e6242f435dda4b90e5a63f6163facf823a8e367 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingAccumDeltaSize 0b152069e94d1c104fe74f9135fc09ab865858da0ce6a120aec8a3d6e964fbeb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingHighlightAlpha 5d944da85702f17ce39ecb56efa3bbb5cfca94253afca80c5c2e397f5bde65a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingInputSource e63cc07103cf8e0cdd26bfe8c0df714261b3d4802a18e8f42a2ed29ac3896d4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingListWindow bb34ecc5cdb3b4db2b895311e0e844c1514815faa6532cd1fce6544e2b77bf6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTarget 5185223904ee9410b266f068c276894055d1012957e91d3c3f6373ec3e0ca5a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTargetAnim 7685d85b42183b8f9385b42d4ee7933d8f02bd5a86db1d03a4ae2c3b149b8380 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTimer 1d30ab1dafb6f0b9c2c9a53c3dbb5c79a7e94cc0bc4969e8bb335f5d8598024f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingToggleKey f7c37a52a4b9ea8b729c1dde4cf55adda7ef5258ae901ce21cc923d2726294af -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NavWindowingToggleLayer 82edfd43de676fde1e5e5a2bffb3e3d8433173963eff36fa6e9a14a3893dbe65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NextItemData b63d1d0e4cbd3f006397f3ed4caa78c01372ec17d1ead6e8f3bc9465c8d27388 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.NextWindowData 2730151f175e742cce9b30b4b72092d9d5023a860cb2fd4541e8c6c36b137cd1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.OpenPopupStack 5fee7ee3b7aba613f45c70a25315934002a9fda9b8c4856b3bc84bf760dac9ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformImeData dddfa677a81aa66c747c8316b8a69ab986a8c514e823bed1ba6f483f20fe9fd2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformImeDataPrev 0d767030b7161c582b200a2f40c8854020c2063b4b7b421f2ef44361c65f478e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformIo 0857be4dca8dcddc6e44e9bc05a08a83e434116742bfad3201c5c371a0b78688 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformLastFocusedViewportId b6081701b6919563b75af6bb6a6cf0a29f910b93be1a814b5b85a212fa4b578d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformMonitorsFullWorkRect 6365062c23a2c2f8217cebf6f976c9b3a570043d03a0183c7cb058c4dc2af061 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.PlatformWindowsCreatedCount affd3bd909ea41b31c300afe33c61f000e4eacd30280ade48c57f424d77b2467 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ScrollbarClickDeltaToGrabCenter f8c34c9df9b5c2bdbbedea27fe564f9bb981a5ade5ba5946b453067217c7e20b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ScrollbarSeekMode e3204661e43fda41b78c3db158cb7550808099d8b8701a1308f3991fc7882b8c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsDirtyTimer de00467573bbcbf815f3e566a681dabc12750abbcb385d55430998c5f9fd8bc5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsHandlers f892cdc2b18db01c04836f74caa633b24b3653aed6054427e5d61fd9b1743b0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsIniData 4cb52ea60d6819f9d3764f32eea481080b95ac91957ac607a1588329a43f110d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsLoaded 2bc9ec55165be1a69655907ad0b72292a2d93f6c686e22ddeaeab3f7336e37a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsTables 17b1594d307aa2788f7280c0d89c69185c6601e6fd1f4d19572f88acf7661725 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SettingsWindows 5f8974d6b18f7e325ec00243b2a24f9030ca4529b07d9f01a537c4e191ed3624 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ShrinkWidthBuffer 0bf7457a0b7c8126e4ea12903ab72ffbca1eb4da3e0200cec544dbfa9401e222 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SliderCurrentAccum c428f3270af16789e526709db679bdd86d8a6a1901d7f2324bc8f5e9a9e7adda -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SliderCurrentAccumDirty 5c79ca06685dc7bc837426db193971a2acf8154c9128b593f2ede01d1d8c3c7c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.SliderGrabClickOffset 7f22e6a7c60a74a6b40d4768dc660af1ccd4aa3ccff31eb455e27eff80178e83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.StackSizesInBeginForCurrentWindow bcd4d77df49ea165fd4e71c6456dfa7117d5dac8c84747414d866df02cce22e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.StackSizesInNewFrame 65f1dd5ea92f20d74fc626078970e3b678c93a3aa6ba82dadbbe6804e0389f6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Style 218ef558e4fefcf692375b5d23177963b0251f05160a4c657e1967ec3cbf7a13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.StyleVarStack aa89b0f1f5b02fe866b79f9f3aefa7bd347ce7c5ed2f7d8fa5d98248e8dcfb6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TabBars f61b47f112e6db6b71db8670914b3d543b0b387bffd187cbb972333cc8fe9713 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Tables 1a226fccf2dde20c3421a27114f0b965dc9c66b31df0432777ae44eba77ab351 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TablesLastTimeActive 89ed8c7b602f89b4318ed8f9c2a7cda0479c02f313aa27b794c171341f955d4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TablesTempData d5d90e669277aea730ac82e317098f421913f41fed9de167170ec484b3cf4ad7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TablesTempDataStacked dedfef640421823a62d14276d823e62b9a39b7c3698bb057e9aa2ed287da5cb4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TempBuffer 19b53c715bc15e8a76a61c50f464fbfb06f5697bf9986f3fd1ad72521d5ef8d1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TempInputId 1daabbdbfdb3def1f5b490da23c254e132f96d4a0d16406f38c54c502525189b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TempKeychordName 89b0282dc7937bad162d869976c8d362757496a178d8cf926738e3ac301c87d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TestEngine 67bdd38f3d4c459235d6f572602290c2b9904c70a6166b0753657a670bc16e61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TestEngineHookItems a29b0deae0890584897cc65312881b578cbc9559b88d7cce7db50f694af733f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Time d8309c2b63384f3a6e477314347afd87600db85549c0ae4ab3346495512ef83a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TooltipOverrideCount 4faea8c98d4c1c1521461f4d641143e853d016dfd0222ab88e66f6d98ced22ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TooltipPreviousWindow 86e574560079cccf56b4c0dbe6273779ea6c6f51fc9ecc8b77e9371ebca697ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TreeNodeStack 71ffc4a384e8537fd6710df55c376f3390818d8554bb4faa37d9e17018ab39e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.TypingSelectState 7a0c1b839e49857d52dc9b9182f6fb1929a5387d76110b7ef3af44cb38e336f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.UserTextures 7f69178ae4f4b2bb228d18b896c0706428432356ad1a7df5b98dd01a68ff52f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ViewportCreatedCount 659fc8f5b0fe92df82c394808ac237f501da7ebbb4df3433cbe930e41f411992 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ViewportFocusedStampCount 1812afaedbb1f06df94d4ec06075d9a81fd97cd0a85004d41963797e763c0be4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Viewports 479a7106489bca2f5583c0a3dfdd3ab3fcefd80dc6338c7ef8a6a1ae008628a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WantCaptureKeyboardNextFrame fe94754c21ee94f3d25998b1ee69ed18898ddd775934ddcef6c2ff6e9fc66ffd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WantCaptureMouseNextFrame 9d01f8b8494c8d7ad9bd65ba0dee0235ddf269f6391d83e1ac95385217477019 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WantTextInputNextFrame b95fef14ad66140599af0a0d0dd6471d586419a14a80333d5d8c5ec77a6f317f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingAxisAvg 551e12381127d813793a3f172682f70a54d21dd197e3079e8284cbacc7eebbae -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindow 50d252e87fc787f08b75d6fb48c98f8e21a3bb93030692256fb6d9651a8e48b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowRefMousePos fd3d41b056f23bb82fe4c945d7907d74f70770cd4daf4ae0c11ec1407456fa47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowReleaseTimer 1b0e99374aa47252c1d4997cb83b2625924242be0ab757c0c5d59dfaadd1aa0a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowScrolledFrame 89e3a4c86c955db36b348e791d7a349522cb02dffbbba669356c5283778bd312 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-4ed551b3a9547392"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DisabledAlphaBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-36350ec5936ff3c1"></a>[`public short Inno.Native.ImGui.ImGuiNativeContext.DisabledStackSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28648) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-962b489b7920fb7f"></a>[`public Inno.Native.ImGui.ImGuiDockContext Inno.Native.ImGui.ImGuiNativeContext.DockContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28666) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-33b371652dd3c48a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiDockNode*, Inno.Native.ImGui.ImGuiTabBar*, void> Inno.Native.ImGui.ImGuiNativeContext.DockNodeWindowMenuHandler`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28668) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-043eb4d9a201a6e0"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DragCurrentAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28642) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb5a764c375a78a9"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DragCurrentAccumDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28640) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89d2a1e709380cce"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFlagsCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-104ce2b9df7c518b"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFlagsPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28522) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-489884f66134c995"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28530) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d8465b18e3c8929"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58f8a894a6a67e9e"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdCurrRectSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fffc36fa18983050"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DragDropAcceptIdPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28528) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10f2eed602e735bc"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DragDropActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ace45b9b47b8c02"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DragDropHoldJustPressedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28532) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7772abf1d85a89ea"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28508) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-023e7493c6dc7719"></a>[`public Inno.Native.ImGui.ImGuiPayload Inno.Native.ImGui.ImGuiNativeContext.DragDropPayload`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f64539d7eabd13d2"></a>[`public Inno.Native.ImGui.ImVectorUnsignedChar Inno.Native.ImGui.ImGuiNativeContext.DragDropPayloadBufHeap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28534) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-041b5ae81ff1387e"></a>[`public Inno.Native.ImGui.NativeArray_a4748c2e54d124a7 Inno.Native.ImGui.ImGuiNativeContext.DragDropPayloadBufLocal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28536) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a945386cbdb4fb1b"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropSourceFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ea57584b628527d"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DragDropSourceFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85d7c832e5c93dd5"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ad20b6acaa897d9"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetFullViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28518) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2e12daa11861e49"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ef8975b9770b975"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.DragDropTargetRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a370774e6d28b01"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DragDropWithinSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-25bc1fe9b105a8e7"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DragDropWithinTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7fd209b1dacb069"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DragSpeedDefaultRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35f7f561e16e3041"></a>[`public Inno.Native.ImGui.ImVectorImDrawChannel Inno.Native.ImGui.ImGuiNativeContext.DrawChannelsTempMergeBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28554) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7995d8520cb5ac14"></a>[`public Inno.Native.ImGui.ImDrawListSharedData Inno.Native.ImGui.ImGuiNativeContext.DrawListSharedData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28184) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e671fb55f9f3df3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, void*, sbyte*, void> Inno.Native.ImGui.ImGuiNativeContext.ErrorCallback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28712) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80f857dde0d326ce"></a>[`public void* Inno.Native.ImGui.ImGuiNativeContext.ErrorCallbackUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28714) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5cb9707040d00c1d"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ErrorCountCurrentFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28720) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6b8c919a2128d6be"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ErrorFirst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28718) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fd82d1e4c1fe59f7"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.ErrorTooltipLockedPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28716) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8f48b7113934782"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitor Inno.Native.ImGui.ImGuiNativeContext.FallbackMonitor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28362) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7051f0631451bb89"></a>[`public Inno.Native.ImGui.ImVectorImGuiFocusScopeData Inno.Native.ImGui.ImGuiNativeContext.FocusScopeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28340) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da0b6b48abb1c49e"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImGuiNativeContext.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28170) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c6a2971c2cdadc4"></a>[`public Inno.Native.ImGui.ImVectorImFontAtlasPtr Inno.Native.ImGui.ImGuiNativeContext.FontAtlases`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b6a3b82ad7b3ad6"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImGuiNativeContext.FontBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28172) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43ab65ee5fbe1649"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.FontBakedScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28178) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-249802c2ad6d9e30"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.FontRasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28180) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4020dbb8d19688f9"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.FontSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28174) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cee684ad08cf38a0"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.FontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28176) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f514a532ef7c0f82"></a>[`public Inno.Native.ImGui.ImVectorImFontStackData Inno.Native.ImGui.ImGuiNativeContext.FontStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28338) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10a2eabca5105638"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28146) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b804ec0a10054b3e"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FrameCountEnded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28148) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60ee1b50634f8148"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FrameCountPlatformEnded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28150) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a5ae41efcc0c70fc"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FrameCountRendered`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28152) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-859e97e121741823"></a>[`public Inno.Native.ImGui.NativeArray_605a5c71dc256950 Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c597b46e71a3372a"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28774) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2715f78007ea2ad3"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28772) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e747f7b4194778f"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.FramerateSecPerFrameIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f9c7b6c23d71f2e"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.GcCompactAll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-619acdf3c156878f"></a>[`public Inno.Native.ImGui.ImVectorImGuiGroupData Inno.Native.ImGui.ImGuiNativeContext.GroupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28344) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc8575865c061950"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HookIdNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28684) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89b98f85ddf9f2cc"></a>[`public Inno.Native.ImGui.ImVectorImGuiContextHook Inno.Native.ImGui.ImGuiNativeContext.Hooks`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28682) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-658231df5e3be5d5"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayClearTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28580) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2aa28aa804387a21"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86ce92780599259b"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28576) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c7fa863dc0d45ee"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.HoverItemDelayTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28578) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08ec4783f906e82a"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoverItemUnlockedStationaryId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28582) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4ad2bc3f65a460ea"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoverWindowUnlockedStationaryId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28584) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f35d003f5ed1c57"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoveredId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28242) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afb909ffc966b34c"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.HoveredIdAllowOverlap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28252) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c85a0c2a7bff716"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.HoveredIdIsDisabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28254) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-11259d3979ffec77"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.HoveredIdNotActiveTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7326dc48d3c04b7"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.HoveredIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28244) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22782b16e4e86f47"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.HoveredIdPreviousFrameItemCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28246) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7188af216b8c5b07"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.HoveredIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28248) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-872d542dd6206852"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.HoveredWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28216) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27f89352a9670a77"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.HoveredWindowBeforeClear`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28220) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f29a0126854708c"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.HoveredWindowUnderMovingWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28218) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82ade9dc4616507f"></a>[`public Inno.Native.ImGui.ImGuiIO Inno.Native.ImGui.ImGuiNativeContext.IO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28158) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-697fb36a86f0d075"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.Initialized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28138) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d3af569ffc9a394"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.InputEventsNextEventId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28196) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8da640e1540c8412"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiNativeContext.InputEventsNextMouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28194) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0833e5f962ffc73"></a>[`public Inno.Native.ImGui.ImVectorImGuiInputEvent Inno.Native.ImGui.ImGuiNativeContext.InputEventsQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28190) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c18bd23b52dfbea"></a>[`public Inno.Native.ImGui.ImVectorImGuiInputEvent Inno.Native.ImGui.ImGuiNativeContext.InputEventsTrail`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28192) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7724f0475fb0ce46"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedState Inno.Native.ImGui.ImGuiNativeContext.InputTextDeactivatedState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28596) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67f2253b36915f4b"></a>[`public Inno.Native.ImGui.ImGuiTextIndex Inno.Native.ImGui.ImGuiNativeContext.InputTextLineIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28594) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46ce7268a243e6ef"></a>[`public Inno.Native.ImGui.ImFontBaked Inno.Native.ImGui.ImGuiNativeContext.InputTextPasswordFontBackupBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d485e016ee65879"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.InputTextPasswordFontBackupFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1766cd9436dd13f5"></a>[`public Inno.Native.ImGui.ImGuiInputTextState Inno.Native.ImGui.ImGuiNativeContext.InputTextState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bfc20e8d29593333"></a>[`public Inno.Native.ImGui.ImVectorImGuiItemFlags Inno.Native.ImGui.ImGuiNativeContext.ItemFlagsStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28342) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b361095dd30daa1"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ItemUnclipByLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28256) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6cef2e5d9692305"></a>[`public Inno.Native.ImGui.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN Inno.Native.ImGui.ImGuiNativeContext.KeysMayBeCharInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28304) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-770f03cb41a02384"></a>[`public Inno.Native.ImGui.NativeArray_ec6e6e3c26b88eb8 Inno.Native.ImGui.ImGuiNativeContext.KeysOwnerData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28306) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ad667532b4c05d7"></a>[`public Inno.Native.ImGui.ImGuiKeyRoutingTable Inno.Native.ImGui.ImGuiNativeContext.KeysRoutingTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28308) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-814f497da26d08c5"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.LastActiveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6adeea396574f645"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.LastActiveIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28296) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ccf12b68ac353d9"></a>[`public Inno.Native.ImGui.ImGuiLastItemData Inno.Native.ImGui.ImGuiNativeContext.LastItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28324) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef738394f51e7b49"></a>[`public double Inno.Native.ImGui.ImGuiNativeContext.LastKeyModsChangeFromNoneTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90845b00c0cd63a2"></a>[`public double Inno.Native.ImGui.ImGuiNativeContext.LastKeyModsChangeTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28298) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69fec6bbd00c95f7"></a>[`public double Inno.Native.ImGui.ImGuiNativeContext.LastKeyboardKeyPressTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28302) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff37c3d4448c6e0a"></a>[`public Inno.Native.ImGui.NativeArray_df98efe8482c9a26 Inno.Native.ImGui.ImGuiNativeContext.LocalizationTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28686) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-577e7aa7df3b69f1"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiNativeContext.LogBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28698) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4a2284a6e39a221"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.LogDepthRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28706) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01a88d33d12e6c8b"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.LogDepthToExpand`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fad4aef0dd2fbed0"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.LogDepthToExpandDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28710) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2454d1f92d9bbea3"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.LogEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28688) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c0b00ae5941ba26"></a>[`public Inno.Native.ImGui.Iobuf* Inno.Native.ImGui.ImGuiNativeContext.LogFile`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46743b6c01cd6900"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.LogFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28692) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f89b5b344e81eb51"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.LogLineFirstItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28690) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-919ec02bacad6ab6"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.LogLinePosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28704) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dbf98fea5a5390ce"></a>[`public sbyte* Inno.Native.ImGui.ImGuiNativeContext.LogNextPrefix`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28700) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a4d4bab1bef75847"></a>[`public sbyte* Inno.Native.ImGui.ImGuiNativeContext.LogNextSuffix`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28702) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a13ae7df5b380c2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.LogWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28694) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d023508ccf9a0403"></a>[`public Inno.Native.ImGui.ImVectorImGuiID Inno.Native.ImGui.ImGuiNativeContext.MenusIdSubmittedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28656) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fff582bd0e77db64"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.MouseCursor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28586) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dfe5befafdf2fca9"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiNativeContext.MouseLastHoveredViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc1641b79e83c810"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.MouseLastValidPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28590) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f6b09961b540d6e"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.MouseStationaryTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28588) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a1aeae791f865575"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiNativeContext.MouseViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28356) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a321c6be9365c8a"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.MovingWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e5819e9bd4ffe879"></a>[`public Inno.Native.ImGui.ImPoolImGuiMultiSelectState Inno.Native.ImGui.ImGuiNativeContext.MultiSelectStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a500bfcf998ea700"></a>[`public Inno.Native.ImGui.ImVectorImGuiMultiSelectTempData Inno.Native.ImGui.ImGuiNativeContext.MultiSelectTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-99e484fc926128d9"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.MultiSelectTempDataStacked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-73ad35e03dd481c7"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavActivateDownId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28390) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e728edab9ad1eef"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavActivateFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d06307a2b0211262"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavActivateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28388) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3230660a0ce9e984"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavActivatePressedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f58ad7c92c078878"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavAnyRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afaddef0bfec504d"></a>[`public sbyte Inno.Native.ImGui.ImGuiNativeContext.NavCursorHideFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a7ea9f508fcc6e6"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavCursorVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27d50f29b9899fe9"></a>[`public Inno.Native.ImGui.ImVectorImGuiFocusScopeData Inno.Native.ImGui.ImGuiNativeContext.NavFocusRoute`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87f2c75cdafb7c36"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-008bfb04219c788c"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavHighlightActivatedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fb6e17c61cf413e"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.NavHighlightActivatedTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2cfdc7f020e5010"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavHighlightItemUnderNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-356f37c101e4dc13"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb0d3af55123e04d"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43c633ba6579469b"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavInitRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbb9d3675bfa1e83"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavInitRequestFromMove`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-658aaccd6d2603f6"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNativeContext.NavInitResult`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28418) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-604e58f8592e5b39"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiNativeContext.NavInputSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c505bd841f364c2"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedFromFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5547e2419ab4413"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2b285144eead380a"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToHasSelectionData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28466) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7345c82437299074"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbca86eaf99de935"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToIsTabbing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28464) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-de415b20fdda44f8"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavJustMovedToKeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c488da18e92ef01"></a>[`public long Inno.Native.ImGui.ImGuiNativeContext.NavLastValidSelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b399f022f957ab5e"></a>[`public Inno.Native.ImGui.ImGuiNavLayer Inno.Native.ImGui.ImGuiNativeContext.NavLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28386) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b20ee3a9cdb6e159"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavMousePosDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81d7fbcee469894d"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiNativeContext.NavMoveClipDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6e444a91a93a3b4"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiNativeContext.NavMoveDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae92773ba667cc72"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiNativeContext.NavMoveDirForDebug`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-033912eea8b358b2"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavMoveFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcfebb1a2ca1b677"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavMoveForwardToNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80ad0e23c98161d6"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavMoveKeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ebd133fcd5b0b13"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultLocal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ae16274420d1246"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultLocalVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c880485c0a332f6b"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNativeContext.NavMoveResultOther`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a680e1f7c39058db"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavMoveScoringItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efca53561b916350"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavMoveScrollFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3536c22550dde32"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavMoveSubmitted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0e21c83a691655c"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavNextActivateFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f063d549642c2b7"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.NavNextActivateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf271dc34d54502c"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavScoringDebugCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28442) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fdd98f11737460c4"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.NavScoringNoClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a5863450135b021"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.NavScoringRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3566c22f0a360189"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavTabbingCounter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c88f276a90749fe"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.NavTabbingDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72faddcf1b5c201e"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNativeContext.NavTabbingResultFirst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecd1211a5a1862f1"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.NavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4219d46d3daee66"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.NavWindowingAccumDeltaPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e6393740bedafb2"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.NavWindowingAccumDeltaSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28494) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6f4d438f22bcdf5"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.NavWindowingHighlightAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-861d36db3a9d37e9"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiNativeContext.NavWindowingInputSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f43d1a1ab120a569"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.NavWindowingListWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7e5fa25a4bd4311"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98e4db2a71b01ec6"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTargetAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3d65af25da5eb64a"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.NavWindowingTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-035718a7329a9739"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiNativeContext.NavWindowingToggleKey`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28490) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bac4b3505d6164fa"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.NavWindowingToggleLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21fc2f46eda4887a"></a>[`public Inno.Native.ImGui.ImGuiNextItemData Inno.Native.ImGui.ImGuiNativeContext.NextItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bb6f7a874885456"></a>[`public Inno.Native.ImGui.ImGuiNextWindowData Inno.Native.ImGui.ImGuiNativeContext.NextWindowData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7335750a9475f113"></a>[`public Inno.Native.ImGui.ImVectorImGuiPopupData Inno.Native.ImGui.ImGuiNativeContext.OpenPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28346) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4018a0a4f6b6bf71"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData Inno.Native.ImGui.ImGuiNativeContext.PlatformImeData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28660) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9749cb56d8841660"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData Inno.Native.ImGui.ImGuiNativeContext.PlatformImeDataPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28662) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07bc42f614b1a4ca"></a>[`public Inno.Native.ImGui.ImGuiPlatformIO Inno.Native.ImGui.ImGuiNativeContext.PlatformIo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28160) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b45760c0c8010edb"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.PlatformLastFocusedViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb9f105def4b7f11"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.PlatformMonitorsFullWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b159cc4183896793"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.PlatformWindowsCreatedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b4edc94d18547e2"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.ScrollbarClickDeltaToGrabCenter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95152cd655d81f18"></a>[`public short Inno.Native.ImGui.ImGuiNativeContext.ScrollbarSeekMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28630) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a19f1df8c63cdc0a"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.SettingsDirtyTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28672) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9334e6152b905523"></a>[`public Inno.Native.ImGui.ImVectorImGuiSettingsHandler Inno.Native.ImGui.ImGuiNativeContext.SettingsHandlers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a7401cd3a91c30db"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiNativeContext.SettingsIniData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28674) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-38eba4f19ef79e5a"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.SettingsLoaded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28670) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37b3f16d5c52d435"></a>[`public Inno.Native.ImGui.ImChunkStreamImGuiTableSettings Inno.Native.ImGui.ImGuiNativeContext.SettingsTables`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28680) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-046a965debc2dc0e"></a>[`public Inno.Native.ImGui.ImChunkStreamImGuiWindowSettings Inno.Native.ImGui.ImGuiNativeContext.SettingsWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28678) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba6b92a837e54c67"></a>[`public Inno.Native.ImGui.ImVectorImGuiShrinkWidthItem Inno.Native.ImGui.ImGuiNativeContext.ShrinkWidthBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eec6bd57d0cb756f"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.SliderCurrentAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28636) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cf02913ab407ee3d"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.SliderCurrentAccumDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28638) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-685de25f3bd57d4b"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.SliderGrabClickOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-714e041d65438809"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState* Inno.Native.ImGui.ImGuiNativeContext.StackSizesInBeginForCurrentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28724) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f8adc94d02429a4"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState Inno.Native.ImGui.ImGuiNativeContext.StackSizesInNewFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28722) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07cd2deadc3be415"></a>[`public Inno.Native.ImGui.ImGuiStyle Inno.Native.ImGui.ImGuiNativeContext.Style`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03421018f44967a5"></a>[`public Inno.Native.ImGui.ImVectorImGuiStyleMod Inno.Native.ImGui.ImGuiNativeContext.StyleVarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28336) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d7bad21b2df9ecd0"></a>[`public Inno.Native.ImGui.ImPoolImGuiTabBar Inno.Native.ImGui.ImGuiNativeContext.TabBars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43d4f841f25a88f5"></a>[`public Inno.Native.ImGui.ImPoolImGuiTable Inno.Native.ImGui.ImGuiNativeContext.Tables`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dfacaddc2d5ebaa8"></a>[`public Inno.Native.ImGui.ImVectorFloat Inno.Native.ImGui.ImGuiNativeContext.TablesLastTimeActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84b0fe02d3c47ee9"></a>[`public Inno.Native.ImGui.ImVectorImGuiTableTempData Inno.Native.ImGui.ImGuiNativeContext.TablesTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fca8fa3d3c98b6a"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.TablesTempDataStacked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4772102f8cba8ab5"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiNativeContext.TempBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1b0d3375d8eed85c"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.TempInputId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28602) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85064acf4289e94d"></a>[`public Inno.Native.ImGui.NativeArray_f4284eefd115ca43 Inno.Native.ImGui.ImGuiNativeContext.TempKeychordName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28784) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b51d198699af402"></a>[`public void* Inno.Native.ImGui.ImGuiNativeContext.TestEngine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28188) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ad993dbc30913a21"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.TestEngineHookItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28144) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29e74ee1f00ef09e"></a>[`public double Inno.Native.ImGui.ImGuiNativeContext.Time`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28154) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-57eb133518cee39b"></a>[`public short Inno.Native.ImGui.ImGuiNativeContext.TooltipOverrideCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28650) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e005ecf6a2d330b2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.TooltipPreviousWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28652) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b72f944d7459900d"></a>[`public Inno.Native.ImGui.ImVectorImGuiTreeNodeStackData Inno.Native.ImGui.ImGuiNativeContext.TreeNodeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6bdb6e4019fad07f"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectState Inno.Native.ImGui.ImGuiNativeContext.TypingSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28658) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a752bfcccea63b0"></a>[`public Inno.Native.ImGui.ImVectorImTextureDataPtr Inno.Native.ImGui.ImGuiNativeContext.UserTextures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28664) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0899839626668679"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ViewportCreatedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a28191a1f7f47e05"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ViewportFocusedStampCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-58c9d8552443aa98"></a>[`public Inno.Native.ImGui.ImVectorImGuiViewportPPtr Inno.Native.ImGui.ImGuiNativeContext.Viewports`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b125bb16bff82393"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WantCaptureKeyboardNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28778) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54dd33d286060523"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WantCaptureMouseNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28776) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b52f17f5395b966c"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WantTextInputNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27805290908a4c85"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.WheelingAxisAvg`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28236) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e05fa93159da3a9"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.WheelingWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28224) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f43402f4898a23cf"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowRefMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28226) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4731ceec180ffeda"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowReleaseTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28232) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a02a187dca5da8e7"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowScrolledFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28230) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
