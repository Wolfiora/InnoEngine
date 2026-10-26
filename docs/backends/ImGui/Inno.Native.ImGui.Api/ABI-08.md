# Inno.Native.ImGui ABI · 08

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowStartFrame 47eee57e53b9b993eb9802c2e8ceed4b296a696d6396fd0f0dedb17f09ace805 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowWheelRemainder 8dd930d186367a7eb6b741233b62c8ca92307950447b3fbfc871ccb5a3e230a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowResizeBorderExpectedRect 1ef62df11ed475813328683ea09e740d0ba4df6e567ce3756abf58a4d45e56f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowResizeRelativeMode 413f16df111b4af09247254c8bcfe20fda8b302852f7a91d9af8f232fdf6cb2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.Windows 249c9c2e175a488bf5afcdab363df460b3647e1ab9bc992cb2e53983eb8096cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowsActiveCount 140f4f51274ea1eaf4f21745bea21067e4c50e6b9d2cb2e5ab4e410e5a32c04c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowsBorderHoverPadding e0b88b46f2bc65b0f25e6f7ab1a236ef638f283860014b5e47e4c75dbab8d9f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowsById e67ded47c6bb39a2c6490aed0468e2042cade5cdeedd419aa2763c25e382a834 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowsFocusOrder 85d6134feb9da193a01a47efd76b026ed2c7804948ec9b397faf87e450008f77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WindowsTempSortBuffer d18367506ca20f6d0006fc980b536635786926ab62b446c28602b97f42da5f23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WithinEndChildId 98d1ae144b1d2c54eb51759abbf2c1a85cc6a6effa03e95a128ab001bf347836 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WithinFrameScope 9164a9ef310ef699519f9575810c5ec894a4da60a468deed94b1e4c269e0a9d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.WithinFrameScopeWithImplicitWindow 4b1825ccaccd36b4a7d1e57675820ec2bb55675753af204a2c8cc8aa15f70a53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistAxial 3a60b0936e924028d5bf89c15ecedf3b87f997758c9c88d8e06942b433e81f66 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistBox 9c6a70fb9334867804d0926d07a7ecf332245d7f3b58ba3746f7e724b1c8c3b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistCenter 85be80201fdda080d39cf564b27fd9899a45306480ded14307456cddca6fd0f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.FocusScopeId 6dfd2c29dced5b8eabe6ed9b2564d73cdefcde5e8bc2b124003731e7424347de -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.ID f8c6687a4ca42fd5bde17c492b0ae5f54340c44b07464668b38e68ebfa210d48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.ItemFlags 697d762402f1cfb5b7d8b844431964d4984edf6921439e472655899cb24ca32f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.RectRel 16e848eb119e9e9ec631fdf4c0e2d0c5ab37cbb69731210d831378136347a3c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.SelectionUserData a53e06895e9916c29c6dc1462981f6b45a217c19be5738d4c16d77fed67a7ef2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.Window 6ed7420b009ef6db21097b18be205c2ab0bfb4f59002b89c24e09237d880e141 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Count 6111c017a0e55d12548e98855365027d16824607a7cf61b8bf6d0ad0f4ec7e12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Main 76d8459515dd895c0ebb881b5a52a62bcbf0ada531aa9e3666944281f4425ec1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Menu 61f544dfb3478216cca3dbeda1bc3e514f1dba8fd5091f6f102ad38a0224cb4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.Activate a7f7084fc836ed73e69905bb7fad735b347739ddf9f6e74e53d94c3530adbfba -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.AllowCurrentNavId e6622681e2b94a8f0eba2890ec2eb35fc7f1062400499ec279a27eff627d4438 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.AlsoScoreVisibleSet 61ea041aee33cd743d27447bfb41c0fdd8b4a3ab2c93892cff6fcbbe808fddbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.DebugNoResult ad08fe0ac93944b96766e79c9e99477ef9a0f8f36acb70558f968e73f1457faa -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.FocusApi 89d5f4a131bc5f26af5f8f01531e78b3f5a4cd02de9b4825dcf51bebe5ccfcc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.Forwarded 4b983a4cb36d25bb1d24c7dc660dcaf73b90089d9ff0d2c249d36595531c2e7c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.IsPageMove 75d5366e18f998d15af5ae018a0f52e44fd13684fa03e29ea8cacc9060724856 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.IsTabbing ee69fe9621bf38f200e7087a49bab98f174dc3e4959ea22550170436f9a81e4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.LoopX 1c2f01ccab08ddc4ab228b876b4074639531d6aace6d8f159574a02148d2b4f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.LoopY ad873d85e0142207c7c8b49836fa7dbe87644889ed4537eb942ffb683e57003d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoClearActiveId db30896343c0d28904d9b98e0b7fb5d27e55813448e86c16be3f0106598764da -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoSelect caef67a2d6da648a305fceb61ef8e4503e7d78a7d0a50e5f3fad568fdad2e199 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoSetNavCursorVisible e24c1254da86b7fd28b27e27161e084bc6a347aa4490b2665d697caae3b33003 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.None 329cc1707162925860092c9bdc9e83d7010a899958586eb1c1903ffaec0eb497 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.ScrollToEdgeY 4796a966da1a7a8d822a0089dadbad3d7fc5b72ab239939202c96ae97b981493 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapMask 8e5b5e558bd36d85431199ce8a834d0f8d6b531922e05c2ac9600c58c3a4db55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapX 90dc4d316f43df6ecd1e71e96f533537b0e40cb2bbe140db118950ca743f7108 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapY 7a22d18fd31b041294e561269665db8b625750d9213ba20278fa96d07c5577cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.AlwaysDraw 6d3ed4b73c089bedcb509c0eb840f4af64c63c61f93a0027455d09376413cfe9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.Compact 0201831572db08c20d9013c5d7a1f4f0bf77f6c134be3e53bf4313901884c264 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.NoRounding 513a0bb2ccacd3fc449fadbdbb9c9b26193daa97fe85f6b56689796fceef5df5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.None f904211ef9396a34af978a912163c455a5b3c7bf10ddf9b5cf739ccede6a1990 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ColorMarker c921e559d5c3373f4b08d165339e28a73ec9058d38e64be62355ea19b765ead4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.FocusScopeId 3bbc25b999b5fe20436e202e8e7ccc58f29f6a543fab1c4d314ca8af6e3a56f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.HasFlags 6557ac62855743d7260d68a28179cb8f3591f2c1c04b41aa4287c24fe33134f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ItemFlags 10cd41d6345436b2e7188c069980df297d00aa38552c55f714e12ab01c5003d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.OpenCond 68b27ac107da2f002d6917751413629817185854358fc0508c55b401290a0c88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.OpenVal 8fa21e7f09b30177ff787ccdfde301dc8cbdc2e420a676a98e9f5e71d7a4d72a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.RefVal e724cc731638321927f444c7761ab72c77e84911570bf0d10ee65420dbaf4fac -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.SelectionUserData 102887142e116387733e0ab9eb738c28b707ee2a0963a0e9bb488f5b76ab1be6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.Shortcut 5d154e5da63b5829bb287d969cc0866c7e1f3d1b7a5c1a18fad1cc6308d9d0cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ShortcutFlags c9b8a5d7b3b873f40cd6fdab525ed2b4342669dabda68fc90e67974cd392ef91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.StorageId 95525cfe6b7c26b69df59c034758338a624cc4f621b85cd069e5368078f68a9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.Width 8a59fbd9a81ecd3c592b70749db5ceb5c0e7c869028e03a9e0c195b71ad0f2b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasColorMarker 0fb2bbbbcacaf94890863f5876834e18b86ca255a6459d6c22dd3f3d4c375067 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasOpen 9b4488041734bde5c07920e22a5a47b44d14db491deb47bd79e087d8671b3958 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasRefVal b1e2b3d39cb3aac9ba6e1b69e79c8be03d0bc8a618f65af2960767be68499ed5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasShortcut 88fd8018f54b2d1f54be5914b2bb3a35355ea86c24b7485bda21eded64ae5cf5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasStorageId fb2a494e0093bbd852078e3ecdd127e9e958d8eab68022b6a7e80e8188c511cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasWidth ccef2d657540610ea3ee9b7d482ab39ceacc4a062c76be5716a5c050a21d67b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.None a35cb6681aa33de2b3b97f88d3979a0bb620f922a5d7c89019d1142fd3519cae -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.BgAlphaVal 61a8e877468692b8755b702ecba0874382b8da209ab1d55494df404f6d79a51c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ChildFlags f9afe486c6f7db44db51c0c439377a06458df22b11830b7159df8be3570eeb38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.CollapsedCond d6a21ab3a8cdb943aefde76f250234bb6c58ad5201b0901657033f23ee59ed0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.CollapsedVal 6c89d022d814a93f3e70e631cfb0009899ef46c74ef2ec4617a78a3c66473880 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ContentSizeVal 6eec42fec34efd760e10f85fff29d63d25554b3bfaa5da16f06673064f895780 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.DockCond a543cb811990f0b0fda653fd8b59ee48a2dd9485195136f5932e2c2bbffb8de0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.DockId 26b82e00f340ae0b41f083f370dbbbc21db76dea7d4de90039ae246fa4092553 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.HasFlags c760f4d69461c44056899c4a90337e3c8d627ce629600b25cd02e63642cfde87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.MenuBarOffsetMinVal f882bbfa68a93486ef88027cab1d5d67f68febc91452a188d9d589fc5ea16041 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosCond c725141ba78642d16ba700b0db34348e8e8ac80c78fbdca574ec95fb2fde8909 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosPivotVal b1d7ba03be827c9bef62b43708f4bf2ce82dd3022cbbfb5f091f3b1be6831cab -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosUndock dc5def497fc22e9a8758c62115080945e3be831996150c0b5548427aa36e89ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosVal 04de86021abd0d3ef708eb12afd33a3121feac01c8f0a73db2fab5b933e793a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.RefreshFlagsVal 5618877243e7deb615c7294ea279811c4e74346ce3ec574f890e2196bcc15715 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ScrollVal 3adc04d135a32b17b16de658c6bfd6c96bbd3d11b7412c196e3d583663e9e382 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCallback 307377675d8e0dfbaa024c462356847c98854181e82d4b4c22786728dd59ed3c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCallbackUserData a4335d3de151b91722e2885979dbd4ece2191ac317734aa2ab61ab3a7da7d9a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCond 9cc3ca0904f9938ba2c4f95b20e59330b99d0adc22184750dafb14cc75bafd09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeConstraintRect b5cf571851d0c14dfa813e68ee5f7a65b6cdd5ea0f74b2fc77adcfc204b5253b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeVal 7b9fae37e6b1612a836c6a64282fcaa37b6a4c63e6786984dd83ab673aaa922e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ViewportId aa91b4a368aaf76d6a21b396c97716c7ce4a2f26f9c868b658361ceca306308f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.WindowClass 808da3c9ef05c0f9f29b17ed215aab5ec75c475d86711e7eb263d172591e22ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.WindowFlags 3aa1b81cb37fb16314ffce4dbec2b062d23fbb72b3cabc4074a065aa6c133de8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasBgAlpha 444cdb211fcf550173269833490f1fe91e801f11471ec60fd25f47e2f76968e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasChildFlags 997e4aa2e36e45f260488af67cb9c295f03e743855506040bcb34d69c8040538 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasCollapsed 413582f02a1c3effdef144609d2307de889f48cde66272a4531b10452369182e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasContentSize bfc1c67cdb016dec93c77b6c99dae90a02406af7e67b601b7e18629f46a3c2ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasDock 3f628a3af9b85e7098a918ac423cba637910c2b698f2ba8d94622d9f7ac47185 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasFocus 77369f098e2d5bf0c8a0c4fdc4ce846c4295d48be52fc3e10c46b552bf662fe5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasPos 9d879e7dba9413f1c0a7460304c61ce1258500be5f92476ded18a5dbfde3a370 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasRefreshPolicy 0b84136afba323d84149e487f026d1576ad954b5c7514db2b6f7e363e0592de7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasScroll 310fff112b9c0f0f42f2604f85e61c29be3e3f2675025da92e5ab09bff3b4cbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSize 573fcf85da4cb5e1947dcaa764d4ac07d0e469b19438c2b6d3ca80dfb4fd56e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSizeConstraint 9b2d87a30c211b6e1acad3b62c06497d395c45fa59cac8669042317e78c5e43f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasViewport b295d2f703068fd3109e7af324c9b9e52ec412749d2d395b26ffb97b99e2845b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowClass d3d8824c229d749c7ed50d81bb5342689f008f93c94d50058bd91edfe206c6b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowFlags 846b7227a6a7c0291fa0cf59b0402acd767bb0fb9e1b51a9a88d45b6c7d8ffbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.None 73e048ef8d7a1337b6064529571aa084b21e01d4f1304768bf7d34fbc3142a87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.ClipRect b897ce0d413e02aaafd9ec5b3b4ddd55e519bb31ec44d40f9be0385841380686 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.Flags df6db7f900de3efa167d93c49568993dc2396799e824cc552fc0f797989b2906 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.OffsetNorm 9375bf111997171b8dd9dc00a3d2f775102c037d3df0d251560c0985d4c7dd17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.OffsetNormBeforeResize 03c1a1a433a299028b0908d876869faa3d4246699f2747715187e93c0fe7ecd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.GrowParentContentsSize 2856b7f6c7081232a5e85c398ad3c45624fba4f8599a26a162c47861e2b8c806 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoBorder 3b3338852e3db2aeacb8e160dcd0229afa239efe1dadc2892aa95bb249484be2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoForceWithinWindow 70cda2b9c15ef52188ce82ae854bd429c946bc71267e7dc31f43d0d9b5431e63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoPreserveWidths 488ed5d84b8d87ac42e8dee5e2e40d792ed8dc2546c6d98b4eece60f8059e94b -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoResize 267864d7b88b130a0983f2d781ec9917bcc97128551b1e7be1da238791fc9ab9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.None 02126e7a33654b2b46380fb67427ebc303a2380871cedd6b619e761caa280c59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Columns eb0be2c24fe57da16c6f4687cff40e2bcf052c5eb41f5104ef83dc29d6f3601c -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Count 29b51c036c10a4c316c242e60dd1e40b597a3e82aeb6168475b4144002c4ddf0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Current b7e7d1033a208ab4c6b7050b8d3e8f5b4bcce38f068ebb7fee8d0a6d520bb133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Flags 12a41c7c367f8b0e20a24e38b9134b0430c15bde0cbed8be48a0525b4a79942d -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostBackupClipRect 8d557116b7a6ab678bb798b1b495c2a30c0629380b09658bd8c89175aa606eb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostBackupParentWorkRect 5c8c7b2d7b208beaf44390b41f0b8762a3f8e0a1dafaffbf37d6f0b937d29ff8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostCursorMaxPosX 8790a2f9e518161b8b5fcbd9bbb1de093b09aed930612dfe67849e676ee2872f -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostCursorPosY e2ffe76649ac5507e4b144bc13cae8d6fea19c0f05a471930fc92750cd41d8c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostInitialClipRect 6c792997167573473f7326ca8b778581d3eae54d1efd5f2e1e7f5c4a493d6e0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.ID 413c81d4310e2ca2f7f9d3623c1f2b7464097a3ec41ded4b9f49fcd48e4ad393 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.IsBeingResized 81b777a13ec737792b5d28e48b1637da2e1ba2b3723108f39752320699664d2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.IsFirstFrame a9e768440dd20fbc556fb5f3ea88cbfdc3ed43b9720c4b188ca6651283fb3dd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.LineMaxY b2f3fa6504a02a2e57bda7b1cc1174b3b58d689a180e413398d6ecf34bd7bbec -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.LineMinY 20f9875695d1b9f6987b0a056ebae8530542f5fb3eb0099d08dd3d2232f936b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.OffMaxX 1c9a0f692d00e6bb07c5640f8a885928ca1aedad3871c2e08cd9e195e12b790a -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.OffMinX 0d0b56ee016b593c1766ce248dc490af016d161d345383226fe282dbb4b87c63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Splitter a8b1ec2465cd31ffec2405913c1e9eded1a6bc2d6e38c564cb85f55b9e2140b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOnceUponAFrame.RefFrame 483453aa19fdcd75f59733ae5b6beb090f7e813cc9c16914ec99cf62d94cc206 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Data 562a00350ee26ea15a51f78273aed9638150a5316f18f9754190ee4dc9065440 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataFrameCount e2266b09d99807bacb9d4473923aa13b77c931c3bc8a2eeb655e9362df9f9238 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataSize 8dcd980ac92db691ef377c19571f802640453761becbd25a84f70e5325277931 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType 1b603a8f0749c4dc3462b08ca776db4026fd10faa9a8c6bbd22c7d5a4be8e4ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Delivery 24cc9088a8882878dc04363e750557c22f8f40c747635d5c532ad60ca64fb5b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Preview 2defe3bb44aa2c09e4330f60d7b9c2385406e7f9dc7961c19e0ddf6cd76a9de2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.SourceId ea5d94e28eb4a99627f9dfb013e534b1dcb50b55ffac6b3bd7ba6d150b5c6271 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.SourceParentId 5fca869d9fe35af22ec3652431b7a6f2915668681ea9f3e61909d38b060bc4b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Monitors 33e449f1b5528ce81bf875f5b975596cf388dce52018421cb50049ad683aeb69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformClipboardUserData 5cc782d47005ec2eb28f2e12f10fdd6c5b4955c315ab9e78d141876dfbf593a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateVkSurface dfb3b240d01f393e962f590944a61d473c41c444e592243ecb19d84aec99dba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateWindow 11363fd859fade4a610ba620790e3a97eb7502ea2590a28a81963a4075e16b3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformDestroyWindow aa1a9a2867327d8db5777a6a832fc6ab11cb59673e05c74ce7cb229ba7e74532 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetClipboardTextFn 6837a67589cf802f4724cfb64f77b4b8b6ba6c6a8273fe707bc23b05753ad483 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowDpiScale 6615e8c45088f526363152c7eee9ebbd75d63555589e8a2324f0d9c9a5784788 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFocus 093ef803c690f5bd495c655f1702a56bd79fa7b35efb4f7eef7f3eb45f1d9630 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFramebufferScale 5cfbb51e32f76a06edb2551738da7f61796f14507bb6573d140822deded36316 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowMinimized d4fc99cd214dbc41b5e182a882c0d01fb9e9b602e95fbac164f39ce4e8282add -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowPos 86589c36dfda146a31b261cce069577fd2426cb8e7211d0afb4da31f03cf1be4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowSize 4dc429f00a0b942a3f85282a60a13fe0e6cae2143affbd350278e76eca2782b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets c881f8a80dd2634ffe4e0bd70cb0c2c6e2635b9b1f0faf5c9996889def51cffb -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformImeUserData 1b3ebd583f296f6d4c034cdc302c6b3ff6a6ee6a3cc2ec9452d29b4f4fbf79a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformLocaleDecimalPoint aee27e1515543da7d030cfb83e39a7bdd6dace6c8fe85dbfbe589fdaa17b1133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOnChangedViewport a8758f3732df3d412b1058d63c2a2d3b8e86fa4416dd1f11f945207b86ac4f07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellFn 27367c089b0e840641f191dd45ca774ef098e0aa7f13d4f6c09bedf226978424 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellUserData fc76bbd60c3b0e48bcfca407888b7187c81278c1f1834429e90b077fc1fcf47c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformRenderWindow 6116861cab394c1fc4e7ca19ca342212d19f135169da30b360d7a195744957f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetClipboardTextFn 60c1181dcc6329da149f1635d2a8973c7ec0bb6008c14a00c2ef58b3e7ad2597 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetImeDataFn 775b2993c854993e87cda46447737a87c071eebf9b78236241855ee750e89041 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowAlpha 4a8c97f9c6d0c09dc3430aec3ad99aa7ad705c8dd79716a6b7e72eecb6325e8c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowFocus 1f7bd01662e782646e593a5be7df84d34a18c9d2b3a963743fa3ed67776d82a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowPos d14bfa247a881f3aa28a245d7f65bc98fa62954adc6ca9d0f5dbc837aad4a46e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowSize a19fd3b57120f578193ae502037135940752f75cb4dd1d28219787f726bdf76c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowTitle dd45b74d204ad1ceb7fe893a557c5d051ec16fcec37e0714e90d45176ea5dbfa -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformShowWindow 9537a405b45dfdae540d79e300e758f0de92acd5f5eeaf5a49c03eba748fd010 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSwapBuffers 6c5aac3510f185995277e455a4db50758fdcb974d387bcb0da6693ccb9f03074 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformUpdateWindow d1ee85e9bf075f5e1f10cdab07d26466ad52810c58e7d25abbd552aa4000440b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererCreateWindow dc2a91be9c6e14bcd3e74d1e48bb0004aa14d747734091050149f42b5f1b14ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererDestroyWindow 52fd684c0afd5e52c0e52de8f630a42c6943df70d95e589d02777aefa6dc45e2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderState c01126fa7be28c5244e2cb29abfe92118383a7adcbe3d4ef6213e2468198a5b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderWindow 230fb0a7c701f9aaaf96c5b72e40e15ff9f2f75b3d49954630713d1934123cfe -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererSetWindowSize 8eafaf2c54f98c9cec0be7e34b11cc1296f1db57edfb4f5d9d8ab66b88c05689 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererSwapBuffers f3473cb2123cb9e1ebfe9e592c3812bdab1f4974e2e3dbdfafcd98ea9cf7d401 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxHeight 9ac7a6c2f0f8f9cf4bca3396820628412566933fd3ebcb195cc8a96d16e82686 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxWidth 101e548771c63df11f0aeb680ec40e1d14375d34029eac1aa610a3c070918324 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Textures 850aabbc3de8a77167d3370d675ea5c7f21c2fb5eeb8a95b83a53929da956fef -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Viewports 40d3cb1e8cdac208005699c5e065b6cc12a99f1b24f8c4875d29b5d5200e1009 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.InputLineHeight 961e015ca9dc11dd1aa141e5023183b7db5afa12f782fef42af7b0286eb0abc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.InputPos 9988cf3523d78a3b5ddc6023760dbfb8d9e3519f609026cb7725ac32bb59ae5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.ViewportId 0f8b2ba715fd92eb2b97c71d569a23b03e8d57a221c496520762cbbb6cde802d -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.WantTextInput 282b951b0b7a7b666efa30a2d9e3a3c41fb03e7c4c086f3c79a4abaecc1a2475 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.WantVisible e3fcfe65dcec2952e37260ae67f2ea906877fa7133b5af789fe0203f2fa9158a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.DpiScale c07c5bcebca138068c9e9b8ddc54f0f571f4d4d20ddee991e6a704caa70a784b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.MainPos b7a0ea662c6a195abc6c7020225848fa3443b36b19722552c2fdc3aed400207c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.MainSize d307ede2748d55197839a78505a8a9710f422bc5a7594ca47e821f8d79371ffb -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.PlatformHandle 9e5a71b7c4771020851383e2192d6d53aaa00b3fd09adafda04707fa9057f34f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.WorkPos e19b809e061d25fb1f1235385df4c2c4f47b7a7f3043c9419b7843b96f64e4f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.WorkSize 42d5bb4f2ec2ce18a888b85e87c56e1411c247165cd15e62beed83aedd7df4fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlotType.Histogram 11befc89dcbc5b936ab604d6951f0ce33bf9685b8718083d914403cf88f4bc53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlotType.Lines b5cc34fb790d731cd01fe0c7776729818dd21be216a106f1dad8460858e66410 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenFrameCount 6e1855ad2b8b9cabb335cc42dcd72db30305ee739e70049256a7df21649e160f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenMousePos d9776857b50ec25de9986f54f0c0be34202bab4308a49e79a3b0b15d3823994f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenParentId a34d99d416bf6eae1e0b7e5cbec087da4de7cc4baa882180e2e43d9260e5d6f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenPopupPos 52c0cc697218680f7d8dd701cf060a1b1eca8e180084aacba9335f2be35ec028 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.ParentNavLayer 249f403ff96c459512016708988faad1c0926a98d80ec2cc42155c20550dbed7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.PopupId f5393e613842c400a6506f2436fa38e43c5e97b24d5871157a0ac0b9d5e9ec2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.RestoreNavWindow fcfe0860c568f1a54e29c29a42841111cfecbac732f647ae6f9b9d5eabffd6a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.Window 839a5593cb090d32198805d237dfc940ec64e573efdcd86c7b57053bb4822779 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopup ba7f0731b32d0256605c6418f71ba474dd5582ff3b44aa7ef2b27aaed3389672 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupId 339615e303e7c431d6e649ad483b32c5adce76ec0c87aa5c5af9671a71f0dd38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupLevel 220454cb706e49d5f02464407aa9dfea5e6c18e7257106f526077e81f2ed223e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.InvalidMask 9b476c11f46ed22e6ff3b4cd9516aabdb0b755c40a4b407ed7fb794211df56e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonLeft 11af9334baa6990a7f2531bc8d0f29b54db72428c319592682d9a11c8a21d849 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMask 5c09cbb6fc522810f5f27d71bbb9fbd3ad635e35cf4253041c0646e12430d968 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMiddle 0826c27d3b744b1b96463a12e94a6463821ff065da716cda8b75ebdefb696909 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonRight 3a383118b2d4c03b3d6f5f6032c1cdabccef8392dd0e1ae484c7ff014795d13b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonShift b6a84b629a0cd27e5a64159d814e808a5316d690f8b699f4c146ecb79560123b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverExistingPopup 3b4791627e5081911b4654699a2255f95620f0d66444692a9cd5412f75a0b1cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverItems e0f568879ee191dfe5cc4820fa31fdcf47140fa94731e29873de247e9473e328 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoReopen 97e80c8016f461ca0d9a8a6322776f46d17716c4fd70dcec98dcf9f48324399e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.None b7822f6fd7d2817eebb4f6fe1536d017c96a2519f1c5687f307a9f4c475bb434 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.ComboBox 4728980d186fd63a697ac482f105290f81cd4eed1057b24cc9c248ab17510452 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.Default 36017846af42975c5dfdae76824bb9072358d94860cc4b9da726bb88433363af -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.Tooltip 0dc35f30b85355f468d6a822ba6cc09b317b589448705a097e385cb76c858e02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPtrOrIndex.Index a6e44e8aef7d5d76a940219d76aa91b668e28e0055cd0b24c88e5fa87656e7cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiPtrOrIndex.Ptr 2430dfd3205444c3895b3aa4af233adbe83f8d81260a1d9b7d0e7df2846c75b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterX eaad362c4f2f6fabbcc74f345033da5f9ba65bc05f6c1d43fac2272594a0d1d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterY 3e1e7b962f84b83bd8478be265ffd61700d9d517e3adb68a703751e705389f34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterX a62e31f5e34f819854b35e31235a02ba3404981f0cf9d217295ed7f3fee6a7ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterY 76c4371c15a157692817fcaab7e9b2a1190d446f036a0ae2506a08595a8c398a -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeX 07ce3cbacc79551a6f9a03286128ad622ce60def3fc701e676c7f12f4127cd79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeY ef06e1329fda07bb8efb65d1a44822c94cb450497c01588a220951f7c49a8dc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.MaskX c6a856f42097e72458ba57827e2931a254980fee211066719c2e60453031bf6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.MaskY 99891f4eef96364dc779ffe115e12c25ba4a9f6a9dc802b37d84b3ac4b462c11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.NoScrollParent 4f2b1b006b72e0258502532441c62c7cee5d53fbf981d8d2c8eb14cb8e8d3497 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-fef565da092391c8"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowStartFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29471) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa3e0b62a8745ea1"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.WheelingWindowWheelRemainder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29477) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-494e03aa57874413"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNativeContext.WindowResizeBorderExpectedRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29869) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a85091661dc45ac"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.WindowResizeRelativeMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29871) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fb19d26705ff1ca3"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowPtr Inno.Native.ImGui.ImGuiNativeContext.Windows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29441) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f09499dc6b6ea5be"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.WindowsActiveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29451) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7dde1b0dfd3048b4"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.WindowsBorderHoverPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29453) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dcfc1a9a853ffb38"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiNativeContext.WindowsById`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29449) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff16c129086fc64b"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowPtr Inno.Native.ImGui.ImGuiNativeContext.WindowsFocusOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-95b2e1de4146dc81"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowPtr Inno.Native.ImGui.ImGuiNativeContext.WindowsTempSortBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e5155f39bc41a53"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.WithinEndChildId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7daadbe2518fdea2"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.WithinFrameScope`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29383) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e509d46942e1fb7c"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.WithinFrameScopeWithImplicitWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L29385) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-53b78d87497cb8b2"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistAxial`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30052) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a4c6ef8eb2f7d43"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistBox`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30048) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c634f552d86b806"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistCenter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30050) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1534403ec635eec"></a>[`public uint Inno.Native.ImGui.ImGuiNavItemData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30042) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-06d735254c72d11d"></a>[`public uint Inno.Native.ImGui.ImGuiNavItemData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30040) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f0be9f19764ae174"></a>[`public int Inno.Native.ImGui.ImGuiNavItemData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30046) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7c8ff14c77b618e"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNavItemData.RectRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30044) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0636faa75804cdf"></a>[`public long Inno.Native.ImGui.ImGuiNavItemData.SelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30054) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-36fa467fe5046024"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNavItemData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30038) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b87cc314141bd7cd"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Count = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8aa50785334ce37d"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Main = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bab7dbeb34f9076"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Menu = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30070) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c690579cc6dc617"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.Activate = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30093) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b4cb8cff2ae706f"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.AllowCurrentNavId = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30085) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1353f3d0cfe56c75"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.AlsoScoreVisibleSet = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30086) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f61f67bf8a01b819"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.DebugNoResult = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30089) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b411af17ef17725"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.FocusApi = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30090) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b228e9666470c0c3"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.Forwarded = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30088) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-513d21788bf7fd0d"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.IsPageMove = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30092) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f20d2babd3394113"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.IsTabbing = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30091) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b9ae580865ca332"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.LoopX = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30080) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c77478b0672c479"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.LoopY = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30081) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6b9d6b6a38d62a77"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoClearActiveId = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30096) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d2eaaaad4ddf388"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoSelect = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30094) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c43069ef61058a3"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoSetNavCursorVisible = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30095) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-34e19d3683cef603"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30079) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-abbd985c145a8b4a"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.ScrollToEdgeY = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30087) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09370d3795ae50cf"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapMask = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30084) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-24d1f509a5d61960"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapX = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30082) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-48c1ee04b948c13a"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapY = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30083) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-16d88d64ea25a5f2"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.AlwaysDraw = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30106) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a40da3a1cbe06c18"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.Compact = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30105) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-41a4011dd6008b53"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.NoRounding = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30107) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc4f9ac7d13f5a8a"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30104) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d491bf67bcef932e"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.ColorMarker`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30140) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc57b839fe5c7c91"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30122) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33471d71c1ef43ef"></a>[`public int Inno.Native.ImGui.ImGuiNextItemData.HasFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30118) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1175cc7595b45a57"></a>[`public int Inno.Native.ImGui.ImGuiNextItemData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30120) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d7c09f455703345"></a>[`public byte Inno.Native.ImGui.ImGuiNextItemData.OpenCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30134) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff785e33e7fdc232"></a>[`public byte Inno.Native.ImGui.ImGuiNextItemData.OpenVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30132) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b20e517f6aca3d07"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiNextItemData.RefVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30136) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f6fc9eeb650c5352"></a>[`public long Inno.Native.ImGui.ImGuiNextItemData.SelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30124) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb304c1176025c44"></a>[`public int Inno.Native.ImGui.ImGuiNextItemData.Shortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30128) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1bde502810227a8b"></a>[`public int Inno.Native.ImGui.ImGuiNextItemData.ShortcutFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30130) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-40f80813c47c83f0"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.StorageId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30138) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00c383a21c1cecbe"></a>[`public float Inno.Native.ImGui.ImGuiNextItemData.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30126) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd03df99c64b1312"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasColorMarker = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30161) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6acf87a92133940e"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasOpen = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30157) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f55e0942bcba1bbb"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasRefVal = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30159) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9292c49b71a9068b"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasShortcut = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30158) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f0483168eae586b"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasStorageId = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00e6883ff1b956d9"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasWidth = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30156) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4735e874c3e4c2c3"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30155) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-86d29d402f4e5f60"></a>[`public float Inno.Native.ImGui.ImGuiNextWindowData.BgAlphaVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30206) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fbdd803c1ba8087"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.ChildFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30194) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c79505681c34b29"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.CollapsedCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30178) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-20dd4d8f1f2bcff7"></a>[`public byte Inno.Native.ImGui.ImGuiNextWindowData.CollapsedVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30198) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8eaa2a72fa57f436"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.ContentSizeVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30188) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2ef69a4e19ec6f8"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.DockCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30180) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7aadfda3b2aac1f3"></a>[`public uint Inno.Native.ImGui.ImGuiNextWindowData.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-29a625e3b066b3da"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.HasFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30172) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4553a823ed5d9010"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.MenuBarOffsetMinVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30214) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a3703461e6a11d51"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.PosCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30174) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-979020b50da0714b"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.PosPivotVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30184) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21c2a7d25d49d195"></a>[`public byte Inno.Native.ImGui.ImGuiNextWindowData.PosUndock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30196) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d08f153db4466a5c"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.PosVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30182) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8df1bd51096b796a"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.RefreshFlagsVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30216) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ade3c9f8b87e5222"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.ScrollVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30190) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fdde0aeda027e16b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSizeCallbackData*, void> Inno.Native.ImGui.ImGuiNextWindowData.SizeCallback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30202) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e4bacb1ba9cc426"></a>[`public void* Inno.Native.ImGui.ImGuiNextWindowData.SizeCallbackUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30204) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7dc342259498e60b"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.SizeCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30176) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ef9dbbb24ebab29"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNextWindowData.SizeConstraintRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30200) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1880d9064d79e547"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNextWindowData.SizeVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30186) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df94efd4a397f0cd"></a>[`public uint Inno.Native.ImGui.ImGuiNextWindowData.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30208) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b794191bc86cf6f0"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiNextWindowData.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30212) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-90a53b200c7488a1"></a>[`public int Inno.Native.ImGui.ImGuiNextWindowData.WindowFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30192) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b4e8214aa6c4e50d"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasBgAlpha = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30238) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3bc68d7755d69b2"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasChildFlags = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30241) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db48cd40ee3faae7"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasCollapsed = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c216fc568a09e5e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasContentSize = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30234) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-54e1d79f007edd2e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasDock = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30244) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3f19b666803679e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasFocus = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2056be70590de589"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasPos = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30232) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d50bf76c02a575a5"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasRefreshPolicy = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30242) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f4885673bbb9a161"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasScroll = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30239) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a3338413d6b93105"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30233) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aec4b4a5be689c54"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSizeConstraint = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30236) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-295a7987927b854c"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasViewport = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30243) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5851856dfd6e804a"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowClass = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30245) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0dd70be3e78252c0"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowFlags = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30240) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e45cf9c0b471ac47"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30231) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d36647322e6452a3"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnData.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30262) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99e68e8c6b122274"></a>[`public int Inno.Native.ImGui.ImGuiOldColumnData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30260) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1b28976a85209cd"></a>[`public float Inno.Native.ImGui.ImGuiOldColumnData.OffsetNorm`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30256) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-32fbd4dd030d8c17"></a>[`public float Inno.Native.ImGui.ImGuiOldColumnData.OffsetNormBeforeResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30258) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08eda66ba26cfc3b"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.GrowParentContentsSize = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30282) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d7707738a2c7c5a"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoBorder = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30278) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2af499f1a29e6773"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoForceWithinWindow = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30281) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-30763187d0ba791e"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoPreserveWidths = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30280) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-966e707832f0e70a"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoResize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30279) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aed5e11716fb7ae8"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30277) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cbe281f349601174"></a>[`public Inno.Native.ImGui.ImVectorImGuiOldColumnData Inno.Native.ImGui.ImGuiOldColumns.Columns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30323) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-28bc4b4312140ac8"></a>[`public int Inno.Native.ImGui.ImGuiOldColumns.Count`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30303) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b0a0c6f94296121"></a>[`public int Inno.Native.ImGui.ImGuiOldColumns.Current`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30301) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-170104eb4d65ba58"></a>[`public int Inno.Native.ImGui.ImGuiOldColumns.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30295) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ebe47f94b5445e7a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostBackupClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30319) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e42faf806ed2ae6a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostBackupParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30321) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-caeaca75902dfd21"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.HostCursorMaxPosX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30315) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-348f1bd981c2af11"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.HostCursorPosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30313) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4eef989dd15ea579"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostInitialClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30317) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf7dc4294ec18818"></a>[`public uint Inno.Native.ImGui.ImGuiOldColumns.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30293) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6309a2f21835e8bc"></a>[`public byte Inno.Native.ImGui.ImGuiOldColumns.IsBeingResized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30299) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-939a1a21dd9eb8af"></a>[`public byte Inno.Native.ImGui.ImGuiOldColumns.IsFirstFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30297) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-91f81c03bf697294"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.LineMaxY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30311) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-489ea63b3292070e"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.LineMinY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6bcf2a2225f6db85"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.OffMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30307) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9331a80d9ba5844d"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.OffMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30305) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a3df4e7fa250a34"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiOldColumns.Splitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30325) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6160ce3a78b2836c"></a>[`public int Inno.Native.ImGui.ImGuiOnceUponAFrame.RefFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30343) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39f9e851d55ad8fc"></a>[`public void* Inno.Native.ImGui.ImGuiPayload.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30361) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c9f71e771b3bbb1"></a>[`public int Inno.Native.ImGui.ImGuiPayload.DataFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c3abfa58133b190"></a>[`public int Inno.Native.ImGui.ImGuiPayload.DataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30363) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef78a9aa16e4128d"></a>[`public Inno.Native.ImGui.NativeArray_704527fa6c941baa Inno.Native.ImGui.ImGuiPayload.DataType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30371) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3eb3d01446d342da"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.Delivery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30375) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed202d46716097ca"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.Preview`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-efdc0d7ccad82a8b"></a>[`public uint Inno.Native.ImGui.ImGuiPayload.SourceId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30365) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-11e30ff0f51c6411"></a>[`public uint Inno.Native.ImGui.ImGuiPayload.SourceParentId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-96955d15f0a272d8"></a>[`public Inno.Native.ImGui.ImVectorImGuiPlatformMonitor Inno.Native.ImGui.ImGuiPlatformIO.Monitors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30491) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ee71c790d45cd8ff"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformClipboardUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30423) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-805091d7636148eb"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, ulong, void*, ulong*, int> Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateVkSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30479) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-61f75f16e34fcf86"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30441) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-325f51eea722551a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformDestroyWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-20ccd86778e124b0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, sbyte*> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetClipboardTextFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30419) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2d39ed2c0670eb0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowDpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30473) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93b5e3f3f0af14a2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30459) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba72b4869220874d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30455) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5caae9eb3e259d2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowMinimized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30461) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8782b2548a1e494e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30449) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2b31ec036a7c675"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30453) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db3df6339467505a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec4C> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30477) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e135013da3ea036"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformImeUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30431) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-336c6cc4912d34e9"></a>[`public ushort Inno.Native.ImGui.ImGuiPlatformIO.PlatformLocaleDecimalPoint`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3073fc2205130e7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformOnChangedViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30475) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-584d4b06ac242817"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, sbyte*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30425) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e8fd44be7c33e00"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30427) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-70ee0f4ec5ea1d2e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformRenderWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30469) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6412b2a72dfc248c"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, sbyte*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetClipboardTextFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30421) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c481186421965641"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiNativeContext*, Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImGuiPlatformImeData*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetImeDataFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2f945ce519f96a4a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30465) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5bbe6892538fa1b9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30457) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ab36016f5fb61b1d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30447) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d71b84d87806f77"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30451) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a7f727d21185e59"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, sbyte*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowTitle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30463) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-877daabf88067fb0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformShowWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9192013451fe934f"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSwapBuffers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30471) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2f06c53f5cfab3a1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformUpdateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30467) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88986afbdb5c6b33"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererCreateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30481) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3d6a7ab3cef76da"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererDestroyWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30483) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2adc0f574437d72"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30439) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d4aefc9a7411a829"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30487) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d8e7e9cfc991d5d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImVec2C, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererSetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30485) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6dc566227d66416b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererSwapBuffers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30489) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a0efdc7d7a9acd66"></a>[`public int Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30437) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8b372b13c355ec33"></a>[`public int Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30435) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fb81ef75a20848b0"></a>[`public Inno.Native.ImGui.ImVectorImTextureDataPtr Inno.Native.ImGui.ImGuiPlatformIO.Textures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30493) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21fbb3d268130f1a"></a>[`public Inno.Native.ImGui.ImVectorImGuiViewportPtr Inno.Native.ImGui.ImGuiPlatformIO.Viewports`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30495) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c8fb911f40ee0b3"></a>[`public float Inno.Native.ImGui.ImGuiPlatformImeData.InputLineHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30399) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-609335ca817b4e7a"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPlatformImeData.InputPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30397) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac258622471ef495"></a>[`public uint Inno.Native.ImGui.ImGuiPlatformImeData.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30401) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-efdd65f4592d25db"></a>[`public byte Inno.Native.ImGui.ImGuiPlatformImeData.WantTextInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30395) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64724d9eacd0fbee"></a>[`public byte Inno.Native.ImGui.ImGuiPlatformImeData.WantVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30393) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af7b5292380d2faa"></a>[`public float Inno.Native.ImGui.ImGuiPlatformMonitor.DpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30521) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d5bf1084fa7a13c"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPlatformMonitor.MainPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30513) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-320f7b1c88bec785"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPlatformMonitor.MainSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30515) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d5f9acff29a4058"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformMonitor.PlatformHandle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30523) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-19e4702887672070"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPlatformMonitor.WorkPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30517) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6389336ede07c4fc"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPlatformMonitor.WorkSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30519) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1cba763aa75ebe0"></a>[`Inno.Native.ImGui.ImGuiPlotType.Histogram = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30539) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-efd05cc1c4a1e66c"></a>[`Inno.Native.ImGui.ImGuiPlotType.Lines = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30538) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e93b9539dfafb88"></a>[`public int Inno.Native.ImGui.ImGuiPopupData.OpenFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30558) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f5fd2348ce8bf23b"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPopupData.OpenMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30564) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-05c4f4dd138f10a3"></a>[`public uint Inno.Native.ImGui.ImGuiPopupData.OpenParentId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0429df8ec6952760"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiPopupData.OpenPopupPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9727ded753062dc2"></a>[`public int Inno.Native.ImGui.ImGuiPopupData.ParentNavLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e6d053aa556e6a03"></a>[`public uint Inno.Native.ImGui.ImGuiPopupData.PopupId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac6c79c4cedff243"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiPopupData.RestoreNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30554) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-652237859d91c0c5"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiPopupData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5367af1064a8b661"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopup = 3072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30588) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eeeedc96254379db"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupId = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73bfce64a27940ee"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupLevel = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30587) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8081ad3075c128e1"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.InvalidMask = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30591) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a86545df09d14ddd"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonLeft = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f2a6b1d5bda1514"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMask = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30590) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f48090d52543c1a5"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMiddle = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47ed8ad03a8e4935"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonRight = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d5f2a29806dae07"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonShift = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30589) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-281b22106c2b2c31"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverExistingPopup = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-762d933922afe410"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverItems = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a88e558e786deea4"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoReopen = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30583) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c0ead90b54319a95"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30579) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-01de0ef6f53f6ba0"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.ComboBox = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30600) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0b46f47ba0e61d8"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.Default = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30599) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-16bc1bc2378ef2d9"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.Tooltip = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30601) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5fc4750a47ae3d79"></a>[`public int Inno.Native.ImGui.ImGuiPtrOrIndex.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30614) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1bf3212153f4685f"></a>[`public void* Inno.Native.ImGui.ImGuiPtrOrIndex.Ptr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30612) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2122107d3c221b39"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterX = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30634) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd23c367229cb58b"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterY = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-241c412fd3970e7b"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterX = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30632) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b267bdc523e2ec7d"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterY = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30633) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-98f797e5258ead1e"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeX = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30630) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9c7ebf01380b744"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeY = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30631) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23adc1222b8fa226"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.MaskX = 21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30637) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b1f49fb598c472e"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.MaskY = 42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30638) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6942fa1304b94c26"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.NoScrollParent = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30636) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
