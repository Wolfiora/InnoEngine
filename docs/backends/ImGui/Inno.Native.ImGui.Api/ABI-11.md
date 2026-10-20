# Inno.Native.ImGui ABI · 11

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSort 0ef4f5a9c429b204332e5e561c3f0491c4ea20250d346f66940da500f26fa14e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortAscending c873872107005343513ce8c5e770961b7f614bc9f60fadbd926649090910ce36 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortDescending 11c834642e6b86c387fdda4bb0fbac19005176e5f6613f4eb139ff8a732c9464 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.None 4cd871053a4f61ac0d7f2a3169897b626faeb73f9266d473bc949c6978d34e28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortAscending 420f941fc4f9140f701858b191ab4cb66009e0b3d71e75eec6e72bf1a77ca0ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortDescending 2224088596f84896d3929794325b26b368e98bb4e716997b33f3dea429f10b06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.StatusMask 76325e3b813821f4ffb1f82734f27ae87b8f188604cc1ce496195940117906c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthFixed fd78ec583f9c7294cf21f5eb43036f2ff6d84221f3913759a5c8d416ad924293 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthMask 8d1c301af53bab8d4d08b8b4faed2e73a29b86837ee34bb341eba1165eb5cc18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthStretch 0d358350e76cb111b41023bf38818c8ed8afde0ed8f763b9f7e2da31b06c6356 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.DisplayOrder 07409fc95a0dda1741c3511a1d6ca49ad2ba25ab9ef9cf365a01427ca3249a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.Index 3b6f196a62660100f1ad199bfe1a59d15245582d06bd266772b69d2a8ac0dfe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.SortOrder d50e60ae441dc84080ad72ce2037989866d5f0746391d2f07975b79e30648304 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.UserId 16ca3bca6c15bf3e6ce4120e7307607a8a0d09f12499c8d6c20755df2e335dea -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.WidthOrWeight b930f97f15dda069a59fccbc06ace2c40ea167e7027695f757d9a4961e8053fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnIndex 452ff7abd35a19e200a5759f37861b37bbe626d63d8519e8d5869a60554a6e5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnUserId cb681d49ba987e4cc82050989c2fb9d19b3403330608615c982b8d4d00f49d46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortDirection e8a0df8c88bc45d62f329547c0bc7fead7d58747996e86ecb32ad2713e91fe08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortOrder 0cbe8cba84ff0bdbfc8930b773a5b2cd1d83b04f73f23fbc77913331a36e51c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Borders f08ef0cb0e8256af986ea853776353f0511dcdcba7c6bfcc561bf85dc2396f5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersH 172064a263535cd60251aad80578f440f55891751531086d7331b33132b2e867 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInner 0d4019036e10b835bbe8630209dd1d36cb72c06b62133c911efd07cd52010628 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInnerH 1c0e73864abfc2e85f1b2cdb9a15ea764a8a1ec377f663f7229052f71d4887c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInnerV 5a6c6d22a4cb75079b101c7c3b8494943de8cea08c7a63d24dc3129fe3a4db2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuter 0f6131e3f50b6afaa94dac6c76b535f632cd68e821532eccf881e6c3d7e17ffc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuterH 7f2b13a475c453a8ad65d184de4b29d70a5bd2ba1014e3e7b0b9e997884dadc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuterV cd535e6eda6b955d24a50aa4c713b715fd1b2106e6e9e239de4b25e48bb1968a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersV 0cd3f47820294e15671c7e04eb5b43545a7bb629a4309285ece5cbac37e9cf56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ContextMenuInBody 3768ed4b3b02342c6c71e9e2ad2f6ee779b1083cf31928226decd5875dbc9b63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Hideable 19f3f0b352813423ab15e494b82bbb4e4085224628b2a1ceb983d1ede9e549bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.HighlightHoveredColumn 2c67873a977e342a09c70f552a2fa5e48a6540b312944e68deb3159cec876f48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBody c26e77bd0a57aee5c7269259c781c3cf995643dc6f4461ad81a17a349e7c22b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBodyUntilResize 68af7e0fa32b3f3b267bd9c8e027f04d43d3b64d7821c43054d3a818acd6d9fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoClip ae1869315656c4dd9211c64ec27c7183581b91692719761d85d2f78780b307b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendX f0062597c890c70cd10642c3c4de8b6bff7ed890435f6b040f49e9a7ea09c17e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendY c5f7268810423b613e380433486eeba70f79a7bd1d174a4bfcc6392411c22eb7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoKeepColumnsVisible c7aaba62ca55fbdffc7ef9f0cdec02aed5454487891596bdef0ab060192ad78c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoPadInnerX 91e9b56ad64c21b3b88f6a145e8e0568534adfd648a3796625543027aee26e60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoPadOuterX 80aff3649ebb9368ef5466e693515edc9c1891200e1b3825d689c2e8891e373f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoSavedSettings e2904cec19423405b16892280cfedf496e38f0a7fe6f0b98730cb45d200ae947 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.None 7b8bd4df1c37f63c1a5c6d27cebd3c67c64ad8e4cbf62b75e6c2e9ade73e8c74 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.PadOuterX be5daa224279763cf5093f3e014366e2820c054f063b2ebb0c75375455dc485a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.PreciseWidths fdd700c5b5670cfaeeebefc537396054d6936dc2c95dccf3f245f441cf825a8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Reorderable 2ea0993f3e1fac04336b6adab896d01dce0c65c93b7286f729914f146dc36cf4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Resizable dd353a22eff3b77c1d19ea78c65e8750c0cf9251984e62939bb56b25cda7dd4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.RowBg d35d23dbf33fcc140d067d2127c106c46974c957c77719e4504d972cbed8db29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ScrollX 622ffc74daddda718d810725784787b196be427917e837516b26acbae0212a63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ScrollY 118c582da38e1072ee5fbdbbecc8c4f064bcfbc4b1e6bff2f50c4fb678c40b72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingFixedFit 5df469577f4f5e438e8e652669be65c0247abee9ca675d9e4bd84a07b008d0aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingFixedSame 432a7a3a131fb4e4a1922be044c133bb8e525aa286bd8915b46b363d72300aea -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingMask 4ae7c0347a308b4b918215c452c0c6a602433c7d3d0ddc008a0da587166117b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingStretchProp 5e04f10fed8b5ed602030eff37838f6db99c3a175880b97452ce888bd50341e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingStretchSame 1507b2edcc85e3c0a63b74b1713a76a7bd1eb4c2c5e746e461dec6648a3b7501 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SortMulti 827df129ad13c6866d7a27b24f46ad4776130041421c9bf647c1a90b29a967fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SortTristate ea5af6a4a3bbf27fa31539e85471809edcd0d65ddb25b25cdf0ecaa3b30a208a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Sortable f2bf29817e56de8fe517e30f27441cd19b53e13da0e77da9ed377a4844392faf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.BgColor0 9f0dddf645ee9abb71fb8c6d08431d60140ada236fb639c7c8f19fd71fac162a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.BgColor1 dc39fc70b33803a6a5fdf5a7c192cab5cf1e511d8a20c0dbc7987c220026ba6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.Index fe230c3f92c757b299cea821f0a6288011e7a9f80745fd4c1378a0102bd6487d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.TextColor f9b74676e7684339e6a27793082322616a9612fc43189d4ba9717adb3ee342ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowLast 0f5d61c99c8edc149503c522bc46574c991b1a0563b0a23b40be0ca37e24a3d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowNext 7a2b6c4b22d18963527c68c86075e006fa2556615f03797bd167fa5d5de8e15a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastFrozenHeight 94d31d7b6885fc05e822026c35276aac65696fd856ac112699d8902546e7f2bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastOuterHeight 0f88b19afb2753f07e28cb46a0ba5e8cfac0176504b79c4d9e605fc7d6a2d393 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastTopHeadersRowHeight b246847cb401fd81f1b19d7e1d1a9667e4dbe5e39f1255d9994a7ed93340b93d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.TableInstanceId b86f290c9b34c3a8418959b235977e6a9d2c662a9c0294309777537f0ea84c15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableRowFlags.Headers 3659dd34302842288137e0c18cb6ff00a1ee850bf27a3ed71214d221c4c53be8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableRowFlags.None 63227798f2d4f49821e27d1a9cf419ad20e1767dc95fe7ea3cfa5f129a20ac41 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ColumnsCount 9212b5eb37a97dfa294c93545088e159a9a1e6edae62423de3b14f2b94610c2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ColumnsCountMax ee0ad85ca046889551bc6be0b4ecf55785a8161ff229b1d2df465f535c222fca -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ID 32fa19346b05650550df121fca8df3ba4be81ea387654ca7422e92d8cbb617f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.RefScale 78e4d074874313d80b3a68836c2b1ff98fe0eadcf8bd4c0d7a1b043311d653f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.SaveFlags 8f3e288004d1be9ab7cbac5bcfe01d3526ca88a6e6d121e798ffe7334c2acad2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.WantApply 213b88510c44c1d63f5b005fe7ef0df876c8ffabb154f6ff9544d34e8666acd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.Specs 4f0083b2afb17c7a5cc2dbcaa6638453b1a6d20ff63f9cf8d5c387f23b2c746e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsCount 7903cdebda5352999dfd7c2c3413a2e707fd0b79a7eb4287f3e8f9b7e38f4e2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsDirty 9732936ab691eb8cc1ffe0ee5020c3e11ce33b6d65a0bff7de23362db487356e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersExtraWidth e4651b2d4ad1e18bc88bb5239d1cb5e4c334916cfe5b3bada7a040cc6df5842a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersRequests 482e24f8a100684baebae150430a4cf25bbf33f7c6dd7ad976db8d980565a7f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.DrawSplitter a36024977a10a640aa4dc9e94a8c3725740025c2264c0a3fec0393af171e0068 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupColumnsOffset a85e2428cefa424ad2c4129e68c2e53a9a7ca33d9a6a3079d2039a818e4729d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupCurrLineSize c87ce3159d214896ce345d3bd77363c708348e4649173a7814f466adf793a745 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupCursorMaxPos 6772a83f572ddaea33092b26c274fcd739ad75f9de5075516e8ed5a9971d626a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidth 082650763a20ac215808b5d699acc0d930c475240b8707d6d66c94623bcf6cef -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidthStackSize b844b86d030c31d6b11963dad6dee07700ca89426a1ad23a36de32e439fd8da4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupParentWorkRect 952eb8266e8623b56346c4e7472cb8e32254c2949091635169f321755942ace6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupPrevLineSize a3ede6600a41ac34836a345029535743eb45e56de427d30a83cc5cbd9bc0e888 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupWorkRect 27f89e6e816bacbac4c5c3e6cc786a0ea9d0d73b3f49efc2eb600c8c60de09a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.LastTimeActive 0afeeca4be881506632b011d4aa8593b79d1de6d733723fee09a11cfec6ec14d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.TableIndex 1e63d33c8ef8acc950c5be35bad0e518b61cb2853d05ed4e2746d8dd23f24f25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.UserOuterSize 85f8d00c96b5e0e567ec69027c60b1a162ddf215853985ac47a9a7ac51b7d518 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.WindowId 5cac5cbec7db5d6a09a2df88f3accea861d9e863d8fbe85cd0c922b4c6f1c27c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextBuffer.Buf 5a004f56414d3c31ac1ddfe410dbc65bbb223c95056ca88170a155395294b86f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.CountGrep 5156ec524b3048ebb151b0c535f8c6bc8fd11257f5f79241978286cd8b4b0754 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.Filters df967a1bb7e0e763394fc9c98d65256042aba0808072454b77f4bd2fe86fa17f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf 15dfbd9ea595777a3e2a886c7aa00c0079d3d2b4c96a98ce26341b91d2b53ddd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFlags.NoWidthForLargeClippedText 71c4c4733b4d53bb57620cbdeb7472791ab473b42dbec4cd721c72ff4f0d5b8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFlags.None 537453d7f24da5562d6c75a3996885d318856e75070f60747945b50755fdd344 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextIndex.EndOffset ae1e632d6cb1ca091f5e5dd07aaad960369d731efa338f03907e2e51d178f1bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextIndex.Offsets 180530d2b1f801121ab499f8cc6ec7ffb266791820002d9104028e91549c7407 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextRange.B 12ef445e128e1ce5c4b7af142a149bf30bea64d8c5bca5334ca0ba9b0be7d91f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextRange.E 7adda8fde1ed06c17c2ed157fb70be4d63087296a17d8e95c6c9d0154339cdcc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTooltipFlags.None 607c7c7e31a23708d81ec6efcb662e1ec93bb240eef3c39714bec6ec824fc5fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTooltipFlags.OverridePrevious 3d2dcb842e5067e5a924020830db4c0ee5abd266f10f0044274d6b0e5051b735 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.AllowOverlap 5cca712641611e4dbde3f34b65bcc769c75b1cfa4012ac8333811fa028d02ee8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Bullet 894e6f889a5de292448d72fcfc14635ab025909d4953d97f315bd1c9dcef5bb6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.CollapsingHeader ce00073df6e736ce3676d277d65579cfe193de1dd5c1387eeb64fab73eb92597 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DefaultOpen 9cf0e0c3757b07fe3aa75bc25f3dbfcb9fd34bee6dcba313237e9ea992043570 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesFull 568304f531af95eddcea0d0c9a3d1d60fc24be7cc2a9cb842c940f0b87a3fd07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesNone 23f1d0b975e3d7b4942d2c0728e76ad057f277b0ae6412ffe0eba6e42eadc261 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesToNodes 37bb907a5ae57e1e1d1bab5e1b115cb522da5e542215c0111ccaabf686cf371c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.FramePadding 81a3f8ef580c6b63fc9584b53b1d2147f27b396bff27ef33d820c0d754bc028d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Framed 818cf9f19bf59718535b9a0bdd6def4938cdb2f664d0c844bf17fafd43099a5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.LabelSpanAllColumns a8b53b64b25663189335cfade14b5966272bb4858d72014051b453e476a808dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Leaf 014b6bb8b9499fb9a99a29d7663b6f51668875a19903ba71bb9f9170567b3128 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NavLeftJumpsToParent 136be8472e80abc90a3fc122f3cb6070c4f28306c4ef36710f0711d4f12102c6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NoAutoOpenOnLog 4afb7e187aab1529cc5291667a6b0380031151694ebd11b5a9ec573ed3b6a715 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NoTreePushOnOpen aaf384e3de636e93b45fcfdcd67d7d717198e8f29eac131a1f28c542d6701a40 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.None ccb4f06b960b4da95bf21b63107f7e4a8d95ec5ec592be096ac11d02dfdb16f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnArrow 2b508dff8303a14bd399a7ee5ca2d8a55e538965a344b34d024eaa9a5f2c9e98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnDoubleClick 8449f16496a630ec2d816499f75d9125fdeaa3782d1fb901f90a6d19099664ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Selected 07d887285ecee664390d78d5023b9b480ac2fa4a1e44633c482a9dd750ed49bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAllColumns 5bb145552533a629ad5dc67ad17a4129cbff35d5549f9a1fe2d83d0e8756126a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAvailWidth 40a578ca28ebb19cd7e08e08323c412c7964b58c1b30886df087ace18512c450 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanFullWidth 3382adc0b96ae30b4bc286932459d6d5b3bc04f3fe4211c8b004fe682cadaec1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanLabelWidth 18daac04611a50e4eedfefac0490de7b2a37d01af1674b79164ef37af3b68eb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.ClipLabelForTrailingButton abfc44796c23c0b14a6a817ed7a6a51966628304862206ad013117911977c31d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.DrawLinesMask 337240cc5c727d01559c505ed0f2f2f91fd2e9e6805fe5890f9d1cf2f4e69ac1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.NoNavFocus e9a17b9b43b7e5c7579dd48372895a65333f1888d35a659489ba62fccf1d54d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.OpenOnMask e5bcfbe269839ff4735a56213cdf164b9f5cb616b01b44199bfa4f7694979fb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.UpsideDownArrow 06c2b93412e412a6a927b7cba8b35fce9f0f870efec98f0ed478db6e91dcea1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesTableColumn a4935ee94c17801abc338900da747d7b58f94949671bb7c2e51584215dc8cf3b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesToNodesY2 4880fe11a1dd00b98aac46c07295d761ac042cac69dea18439c174d8213158ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesX1 00d8115440c7b11402ab178fea65100daffc03658a12c3d4926d0cef38d82c23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.ID 8e81f00390357b5a405b1587caa11469a171fe82e0b54a62333aaeb2dfc440bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.ItemFlags e51abc0a68a9889dc6042087bf17558406d0c6441152f6e78226cb137d9af780 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.NavRect f38b1f10df51f266b8fcabe104197ab3c7251f15122699a9b6348b2bb5c9f2e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.TreeFlags cb070533caf8147d76aa6dce4bf36216c6412ce53e8f92b5e6848398a5e92a59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowBackspace c139073fd39370a39b7448dbb71a5326a94d6f7e89df5747ae7563da4cf2de56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowSingleCharMode b989ce02ffbaaca6648dca4e0d62ed651d4c00111b670ee1bf241191326a2456 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.None 5eed297f5340753a2b5f7eb259c214ef3d9b33f1f82ecea8421f5218749a1f6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.Flags 963b4acd762e70c4366f8c5914397ded021740952741e26b966e868d17462750 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBuffer 7eb380d805de0077e33b500f814ad69425f086a18fc8779aaec758fa2003a239 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBufferLen cf5b1a881009744ab35d6bf168f93170874398d9873c2cddc7587b1721285322 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SelectRequest 1f8adf2d7817e0587de0e3ee3f27bd09c56108ada9a6f54d5c5d27fff55bed26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharMode 203be941a256b16d623ea295d04d4b2476f98718688f9221f06e2697b6273d89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharSize c83e01585fb23d92db1ea584f84cc3f0123c7e44d5b81bce14b5fa1bc5ff9635 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.FocusScope a25f82b252a16dcec810ca61cb7d17e9f8b36eebb914ac7272a03f47c64505db -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestFrame 6fd797d0d2a1957def86331be0ad530bfbe98a54ffa12dc3bbb2db4a7648b1b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestTime da5cb0c547a5b140b2cb8527ab93c0ab4f9c66427b95d4ed36499c7d2d01c4bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.Request 7478a76bb393bfe159d4ff6cacd39d3da1c5643ee717d05033f1a94df4533a4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer 0e98b29d4e9214ff45809090a96191954baa5f6d3a42befaf7189ea78870e8be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SingleCharModeLock 0d45ebea8707ed02c05798d5584203d2aa8519cc969f820a7df1b7d73570241c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.DpiScale 6e0ed563018cf4c827304544986bda59a07179673f3006916b1cb39187ac5fbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.DrawData b8653d1569dfe1c3d5d21275c918988c435854007dcae808bf2cc02f542acf25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Flags c2f621fa839cda3f8ee604f74d9b1ae1e3a30d8beeb24ecce259116878da6d2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.FramebufferScale 58a4b0d2beed8556b261891e2833025ba7de56f7987f64fc137f8497d712cd72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ID f3b2b2a43c33b311903ced9aec17518711459d9b10c363e71a5af35c4163f93c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ParentViewport b99198055742b827a65bfbc94eb9b6b013b3baf7cb4d547c79aeac896f51e54e -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ParentViewportId 8e725bd57e3f571454aaea1c0f5264b94035c67295a3d618c482b965ad04f396 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformHandle 42b0dd83f867f5e1b76ecc092b633d87dedb8e3ceea5c0ec7585b8fab2fad4f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformHandleRaw fbda7a3a8ff9071b458a8eba021d45b6c04f2ae7d2a61a03914b1219c2d89ace -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestClose a0e0c3ff20c930d5c3b10626a590177c42f396bf10a5ec2b2d3e832f4b18f45c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestMove b3c2ee715445169c3a78b197575a82808ca0867c56019f8ecfc35827beeb000a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestResize 472f7ea0a296d72844206b670ec8e4a9fa2d2a37452d0439c27d147258b459dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformUserData 49b39b58b7bab38759338493266204beca545a786720711855b36a7afccddbb3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformWindowCreated 0df67cb518ef290d7a290566667ba57f2a02dac3cc33273dd059e8ca00631e4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Pos a3fcb267b524502397452ae37b2a65df6eefc4c88bc5b72273907f65cec82779 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.RendererUserData f6663043b0b3f9274bcc295bb20c54c2d21df1c227cc6325a1fa8bbe9cf67a24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Size 9651a729f9ec2b14e2729449f78ebb6849068e5184eb353c634bd5e30fc54c5b -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.WorkPos 83f516df301f5c62688784679d15b93c92c96c4ecee109f1b24bdfd5fce46571 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.WorkSize 9408b88de7b6f0db36e92eb98c426167690104acfe06a768c01b2f1a755b4c87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.CanHostOtherWindows c7628b97b9eb758293b7f507e52c407f4cce1fa051750615e2dabbe228224bcf -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsFocused 4fb5af53319007bf2f563bd6dc13c4cab197309c7d50d996ddd249cfa3da5294 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsMinimized b65c8583b8cc9b569e39396b29f4b4f6098152976d043aacf26e70fb8f25f6d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformMonitor c5bd25006bdcff7a4b28f8e410ea51f5e4d5384d1f3f2798d4419b2b64f0f2ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformWindow 519f9f9d13779780f88abc457ae7030056bc1402bd85e21b00bba2ab35cf9a03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoAutoMerge b4b4818b2db35c77a079d785947b580ccb831654d38784f8316f6b347035466f -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoDecoration dc8d0c366d0fed102fe55b15b6be3858bd3f0c89254867a80731961b7d81f8e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnAppearing a8016c7718ec383442783541525d9a269c95911e3317cda21549bee1e11a9f81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnClick 687d4550dd496152dedc4f0e5487c14a700ca9fb2ba6cbe2da631bc3fc3694e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoInputs 423e2d50da0e5cb5ea57eb37e8dc25c99a6b62ae73baf38b3670b8d59001f963 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoRendererClear 4f70204cc719cecf01b3159c948b9ebef7de429f7e0f4280e27368444d7f0d80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoTaskBarIcon 40b3a30fde8b36c1ca551c56ec09c17e03dc8e97c464ee37113757e1d61afced -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.None 43c7b1dca5e52f29280540c180bc474efc6ded15d633c0100e019f93437f3aef -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.OwnedByApp 7beb3dc317766871fa7681ff5ea8cef5707080bb14ed2a277bcbd1418f99cb29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.TopMost 13184c0b2705eb78094df0fd5f0492de6169b42d2315c13b1412b66745760091 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Alpha 4caadab41b4ceb60e19bcf7c3975b0e633e7ca580417ebec2c7887060408bab0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists 20709054f834cdb990d427ef56598b8b0a12e6ba8c86614a49e41e961d731a1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame 58749bf40413560566210e3bcb3dc54586bb106074ed42eabae4232743d0f648 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMax 1282d2fe61632ebb709946b20f6e90681e309368d21e27d3bd0d2db499be09de -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMin 45f0a8f8bee4451b7fd5b0cfb690db6fd1ad4af889f8bf5ce32c84ff7ea8426c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.DrawDataBuilder 9987b786ba97772e31d4168d4e34b8c28196b576c7ab369d81c6619931b32416 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.DrawDataP 4dd2d63e81c32afb2a8ad5a53b637fbfe254811c9ed01bd3e611036e88c800fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Idx 4801e158eb41a24438a4c63ca448bb005f935b557c2100d78e3dd900e2c53279 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.ImGuiViewport 3a6ffea8fad0c5438533f76224a75a37b07a48f0c8ae4b5b23f80d43b36cfa29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastAlpha d0f8a6b8037f650f54ffd77327d14be28acc372ecea78af090ddf469990fb12a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFocusedHadNavWindow 141df790b0667e6eaf178c82575e3821388d356c68b3737564ecf36980e1ce35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFocusedStampCount e71cb38f1ec2b51571c71d5ebafeae09ba4a7a7f4e63772daf3842e8cb64b046 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFrameActive e0b4fd21294cca6f67a6623e9d09c6212ac6dfb49d14dab13671999f72349ae9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastNameHash 69f7ff2038b01b28a45072ace51cd8e8871705a0c9ae68e112de432d4bb86f6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPlatformPos 85766e3f538b5f2bf5ab2a5a9a8df29a278c135051fcbd636b5869cf757e2ce9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPlatformSize 4e4a10769057bd729c8946b1c8b02bd43b07bf04d64e929e227b0f0fbc6623fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPos b7f3d4ead6c70cd2d4b39086ef4e8bbfe0f72918b58db5aa52b1436ce459e9b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastRendererSize 484ad5c6451e4e4f24f0df9a0b6df2bebc0b817affed3f8de4083b5e0a3b0e35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastSize f6ac3db2b8cb7731395db0f2ab28fd333206a3d684a7bdab71d52c78c2b03f79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.PlatformMonitor c0deeef766fe55df126328a21f0d3886abe44be82bd1014a18cb0da8908e5366 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Window 2f61462f8253c0b09ca534be92aa74462d574e19289bbaa7918e037d9f1d2929 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.WorkInsetMax fc7d35ba4afa6f87dbdc870aab22ac7c5d7db667df2fdf6c2fea5c8a3021ea34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.WorkInsetMin 74bb9bab33b9f88cf5315409b60770a5beca6f140ea7bce159a8b30942d30486 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Active 919d0ea6238382d1dfbacae08dc56d53f0d66cd7f47d78fe44da3d4512da3e98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Appearing 50ca0f55682e9e9bcd22ea5cfcce8b1307ea7da78a04a345d88cbdd40ed3f4a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitFramesX 79f753394c6b1d376da5435516ca9376cb825a55b6ac05e5f0a10b284f2a0916 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitFramesY 6ca37e6b6cc9dafbd8f896b9515f6a7c1ae89649fb8c91bc1e725d07b556b5a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitOnlyGrows 2c7e9226bbe4fece36ec975d964112357733f736c4f04a8254e6a65390e9526e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoPosLastDirection 367a3c28b751122e63394f0148518fda67ed37904b4daf347ef4f2d86788c58a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginCount 065ca40d6e5e7e9473adbb7c71e528906adc6c5a5b0bb68448046dd151d18f68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginCountPreviousFrame 9d01652671e3ab44eb5407178bdaf36fcb8e359a48ee2836d423e7aca00fd1fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinContext 377e73c612c0e949f2a0b31d43a85ecca4ce7932bad341a2740b30b83660a4cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinParent 381396cdf6275de281ac2889843191f3e9b984266e34c8459ae71a7ed735908d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ChildFlags a6d71a799b0682a6fd3bad34b52ab6b41392903bc582cf10c5034d88ab44cf54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ChildId 786004ba7c05f66e039f570ec86f90023fcf61ac904354fe101ecae1891ce812 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ClipRect 1a30b1dc19f5d01e2a14db010721803fbb9363a71c354e61a4afcf32c67812eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Collapsed 232a09e8e28abc84d16cafd17f6f28aed57193be2eb08aa3aa56181cde101a7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ColumnsStorage 62f4db58a82127eb8cb38bc5319cf97f104c62fda96a9e98263bb553da544543 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentRegionRect 499494324cb5253cdbaf645b0de961f74bd0154b197fcfb65018a2a97460d45f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSize a35d4ecb3d8b21e7e9b7369593f84b6c9814dace40ee284cbd8cfc086a933742 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-60f2d5ff19464178"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSort = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30807) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-708b13dc6c6f0334"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortAscending = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30808) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-2b3507f45c7c27ab"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortDescending = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30809) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-f9e7411558471717"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30797) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-f9914e369a5ca77d"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortAscending = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30812) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-4c12aa792805a85b"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortDescending = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30813) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-8431a7ad3327764f"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.StatusMask = 251658240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30823) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`251658240` |
| <a id="api-29c9ae3e43d32303"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthFixed = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30802) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-d3a38e54a0527f3e"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthMask = 24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`24` |
| <a id="api-d952c06c2f46e09e"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthStretch = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-db83a78bcbfa64f4"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.DisplayOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-472c62a4557a3efb"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30841) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d059053ebc6646b"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.SortOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e91a3979d823156f"></a>[`public uint Inno.Native.ImGui.ImGuiTableColumnSettings.UserId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30839) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cabdc2d5ecfcfb3e"></a>[`public float Inno.Native.ImGui.ImGuiTableColumnSettings.WidthOrWeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30837) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e85dcde0b3165a16"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5f4723fe03ba1b5c"></a>[`public uint Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnUserId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30951) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-085df0ec171ff685"></a>[`public Inno.Native.ImGui.ImGuiSortDirection Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c249022bfb40f8e0"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30955) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f6b3d6087f47d1f4"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Borders = 1920`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30996) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1920` |
| <a id="api-9c5132d1b29f4443"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersH = 384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30992) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`384` |
| <a id="api-844a150a2499be33"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInner = 640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30994) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`640` |
| <a id="api-a79e21fadfaeb37c"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInnerH = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30988) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-f950d9f2bdf0965a"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInnerV = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30990) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-79118132ff48700d"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuter = 1280`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30995) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1280` |
| <a id="api-e378c6c936712259"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuterH = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30989) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-6b61b2dc8fa27243"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuterV = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-ca71a8a71309ef10"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersV = 1536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30993) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1536` |
| <a id="api-59e9bce3a562cda3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ContextMenuInBody = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30986) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-54d493e7d0d7bd1f"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Hideable = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-e5648f466c9e5237"></a>[`Inno.Native.ImGui.ImGuiTableFlags.HighlightHoveredColumn = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31015) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`268435456` |
| <a id="api-b14792932639e9f3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBody = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30997) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-cf4f0293d4f40dbc"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBodyUntilResize = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30998) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-96cf9fbdbd5110d8"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoClip = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31007) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-f356741da1017c27"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31003) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-154b06c11643a7b3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendY = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31004) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-5a52457abe6542d0"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoKeepColumnsVisible = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31005) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-d1ce5f62f285c94e"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoPadInnerX = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31010) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8388608` |
| <a id="api-cf4dd114a4086a71"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoPadOuterX = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31009) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-d3dc1816f6a37977"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoSavedSettings = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-78045e6e89a79067"></a>[`Inno.Native.ImGui.ImGuiTableFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-585c1f118b9795b2"></a>[`Inno.Native.ImGui.ImGuiTableFlags.PadOuterX = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31008) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-9a0fcd5c749bb465"></a>[`Inno.Native.ImGui.ImGuiTableFlags.PreciseWidths = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31006) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-275ec52bcf0c9eb2"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Reorderable = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-0cf1cdf2acbda102"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Resizable = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-0887b5ab04c72ff3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.RowBg = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30987) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-692a398acc464700"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ScrollX = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16777216` |
| <a id="api-ca99295fd91cf0ae"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ScrollY = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31012) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`33554432` |
| <a id="api-435bc4294b942807"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingFixedFit = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30999) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-478ac1cf53c0c6b1"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingFixedSame = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31000) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-c33c46f037468cdd"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingMask = 57344`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31016) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`57344` |
| <a id="api-6730dd9feecd8a06"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingStretchProp = 24576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31001) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`24576` |
| <a id="api-6fa16952294db7ed"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingStretchSame = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31002) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-2b020aafac5e8a47"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SortMulti = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`67108864` |
| <a id="api-fcc32383389b862f"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SortTristate = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`134217728` |
| <a id="api-4a2b0a53ecaa4b92"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Sortable = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L30984) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-67d900d489fdbb43"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.BgColor0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c12a84ef3d7924b"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.BgColor1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31033) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c69d4dd6d6e7136f"></a>[`public short Inno.Native.ImGui.ImGuiTableHeaderData.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39c6f861499d5a98"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.TextColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31029) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4581be651344dbf"></a>[`public int Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowLast`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31059) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e52522c49020c22"></a>[`public int Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31061) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d53b154cdc07dfe"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastFrozenHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31057) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f9a737440f5961f"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastOuterHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31053) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e61eab8364e27e86"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastTopHeadersRowHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31055) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1684750bcbe8fb2c"></a>[`public uint Inno.Native.ImGui.ImGuiTableInstanceData.TableInstanceId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31051) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-209b51ae48a7a728"></a>[`Inno.Native.ImGui.ImGuiTableRowFlags.Headers = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-21a5b41c3890513b"></a>[`Inno.Native.ImGui.ImGuiTableRowFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31076) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-532e0e458d30c59f"></a>[`public short Inno.Native.ImGui.ImGuiTableSettings.ColumnsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91c2a3a3c6274212"></a>[`public short Inno.Native.ImGui.ImGuiTableSettings.ColumnsCountMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31096) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9e6ad75d20981b7c"></a>[`public uint Inno.Native.ImGui.ImGuiTableSettings.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31088) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-283b506092e40c19"></a>[`public float Inno.Native.ImGui.ImGuiTableSettings.RefScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31092) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c34c36d1535b21e0"></a>[`public int Inno.Native.ImGui.ImGuiTableSettings.SaveFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31090) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-255e97a3e36ccff7"></a>[`public byte Inno.Native.ImGui.ImGuiTableSettings.WantApply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31098) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa51e488c27090b4"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs* Inno.Native.ImGui.ImGuiTableSortSpecs.Specs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31116) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9066addadd4ba39e"></a>[`public int Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31118) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea4677916ce82a71"></a>[`public byte Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31120) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b60a03b5fb6d87c"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersExtraWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31144) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-596eeab6cb1138ba"></a>[`public Inno.Native.ImGui.ImVectorImGuiTableHeaderData Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersRequests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31146) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f77d03c174b1acc3"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiTableTempData.DrawSplitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31150) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-161eb7a478a8224f"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiTableTempData.HostBackupColumnsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f28721a2f1b95854"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTableTempData.HostBackupCurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31158) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39cb41710c82a75c"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTableTempData.HostBackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31160) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bdb44b74d758235b"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31164) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e48200d7c8d9070"></a>[`public int Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidthStackSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3829a5a53ba77b3f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempData.HostBackupParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31154) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ddc9e0513e991511"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTableTempData.HostBackupPrevLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31156) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5605b44d32221a4d"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempData.HostBackupWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31152) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f53c7d76fc47760d"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.LastTimeActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31142) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7333ee044a440ab2"></a>[`public int Inno.Native.ImGui.ImGuiTableTempData.TableIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31140) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d9457cbad88016d"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiTableTempData.UserOuterSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31148) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d2e059615404bb6"></a>[`public uint Inno.Native.ImGui.ImGuiTableTempData.WindowId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31138) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c6a3dcdb0b9dc72"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiTextBuffer.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31184) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7bf43086f84a6342"></a>[`public int Inno.Native.ImGui.ImGuiTextFilter.CountGrep`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31206) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d619f698ce1b9fdc"></a>[`public Inno.Native.ImGui.ImVectorImGuiTextRange Inno.Native.ImGui.ImGuiTextFilter.Filters`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4eaaa57db4382d73"></a>[`public Inno.Native.ImGui.NativeArray_a4f71ca2b114fa9f Inno.Native.ImGui.ImGuiTextFilter.InputBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31202) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d6b1f41af55d91a"></a>[`Inno.Native.ImGui.ImGuiTextFlags.NoWidthForLargeClippedText = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31222) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d97a653199b4969a"></a>[`Inno.Native.ImGui.ImGuiTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-4ae3d87f4969ca18"></a>[`public int Inno.Native.ImGui.ImGuiTextIndex.EndOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac8e58baada4ede4"></a>[`public Inno.Native.ImGui.ImVectorInt Inno.Native.ImGui.ImGuiTextIndex.Offsets`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-081754acb7096079"></a>[`public sbyte* Inno.Native.ImGui.ImGuiTextRange.B`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2eee60232ed2806"></a>[`public sbyte* Inno.Native.ImGui.ImGuiTextRange.E`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31255) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f3f787d41c7faf75"></a>[`Inno.Native.ImGui.ImGuiTooltipFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-aab728b0f73db94c"></a>[`Inno.Native.ImGui.ImGuiTooltipFlags.OverridePrevious = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31271) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-3a97a6ecb92ac5af"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.AllowOverlap = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-d7e29ae0df79f3a0"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Bullet = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31289) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-b4e38e56463d9d99"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.CollapsingHeader = 26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`26` |
| <a id="api-62a4456c38f5e2e3"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DefaultOpen = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31285) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-e30ac06bcc779164"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesFull = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31299) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-c994280b21aafab9"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesNone = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31298) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-000bd7c70ed58c43"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesToNodes = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31300) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-11b3a3b4e70b7f01"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.FramePadding = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-66d28e9d10efc12c"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Framed = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31281) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-7f478abb7103345e"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.LabelSpanAllColumns = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31295) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-74e8f224bd4f5d05"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Leaf = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31288) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-8fd685f784075163"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NavLeftJumpsToParent = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31296) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-79989ec16385b45d"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NoAutoOpenOnLog = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-7d10dd2737a0bed8"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NoTreePushOnOpen = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31283) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-9f2af96b7ef7dab4"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31279) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-50a3c083c98747ed"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnArrow = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31287) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-50f0d517688f871c"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnDoubleClick = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-6d2156bdc18b8e92"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Selected = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-f95d5b9f02df749f"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAllColumns = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31294) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-68763c1d7c077abe"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAvailWidth = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31291) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-841cb3de82ad1097"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanFullWidth = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-6058cc1b4dfdea54"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanLabelWidth = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31293) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-0a6a939cbc7824b1"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.ClipLabelForTrailingButton = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`268435456` |
| <a id="api-9fd7d920d8127c69"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.DrawLinesMask = 1835008`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1835008` |
| <a id="api-f52f0d1096c078c5"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.NoNavFocus = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31308) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`134217728` |
| <a id="api-648701d6809c3795"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.OpenOnMask = 192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31311) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`192` |
| <a id="api-62a360fb26ec7f33"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.UpsideDownArrow = 536870912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`536870912` |
| <a id="api-834b288b2a376cab"></a>[`public short Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesTableColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31335) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-837db83193266e71"></a>[`public float Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesToNodesY2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31333) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7fb2f7442c84c297"></a>[`public float Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d366ace816252860"></a>[`public uint Inno.Native.ImGui.ImGuiTreeNodeStackData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7cb32bae92244eeb"></a>[`public int Inno.Native.ImGui.ImGuiTreeNodeStackData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a01e8a23d5c06881"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTreeNodeStackData.NavRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-922defd594b230b1"></a>[`public int Inno.Native.ImGui.ImGuiTreeNodeStackData.TreeFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1b5a6d2aeba0f507"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowBackspace = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-1111da51f1f631f0"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowSingleCharMode = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-5d4ee99a1bd8472a"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-609b285aadb2fce0"></a>[`public int Inno.Native.ImGui.ImGuiTypingSelectRequest.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31363) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac7a37d4c408a659"></a>[`public sbyte* Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31367) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-601926cd3136ce8d"></a>[`public int Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBufferLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5214dfcc4dce6480"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectRequest.SelectRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31369) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f22275f7b09a7929"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31371) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4cc046953890b0f8"></a>[`public sbyte Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8bf0527a5fb3ea6"></a>[`public uint Inno.Native.ImGui.ImGuiTypingSelectState.FocusScope`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-412e92c258094694"></a>[`public int Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6ead8c8d96b46328"></a>[`public float Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a60a9640813ca2c5"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequest Inno.Native.ImGui.ImGuiTypingSelectState.Request`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72ed0d9ad3140624"></a>[`public Inno.Native.ImGui.NativeArray_f4284eefd115ca43 Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce2dfe39cd0608cc"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SingleCharModeLock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31401) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d5c7ce8628b96e2"></a>[`public float Inno.Native.ImGui.ImGuiViewport.DpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31433) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-30572c957fd1bd06"></a>[`public Inno.Native.ImGui.ImDrawData* Inno.Native.ImGui.ImGuiViewport.DrawData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9fd27e7bc986e84"></a>[`public int Inno.Native.ImGui.ImGuiViewport.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13d1a436cce51d13"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewport.FramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31427) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc04399bd9111850"></a>[`public uint Inno.Native.ImGui.ImGuiViewport.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8cfd7dcdce7dbd08"></a>[`public Inno.Native.ImGui.ImGuiViewport* Inno.Native.ImGui.ImGuiViewport.ParentViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53527ecfbe3c4d63"></a>[`public uint Inno.Native.ImGui.ImGuiViewport.ParentViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-766bd6bbb2a4d996"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformHandle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-819a03cddd8b1c1b"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformHandleRaw`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a10ec2dd373c1d0"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03072fbd0f40aebe"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestMove`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-458b6e3686a03c31"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-237df79cf8e32673"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-782e17cc91cfab54"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformWindowCreated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59a1476c05b9da42"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewport.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f33ea75fe018b2f0"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.RendererUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-454bef48e9174fd5"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewport.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6f4508b49d05e3e9"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewport.WorkPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef4c635caef6e527"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewport.WorkSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31431) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75676e92db69eeb1"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.CanHostOtherWindows = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-d7042caa28bb04c3"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsFocused = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-e91364d4eca17739"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsMinimized = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31483) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-5657e1896cfeccf1"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformMonitor = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-5d111ef1f9016f69"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformWindow = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-dbe2709e4ff3322d"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoAutoMerge = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-c80714a094cb2368"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoDecoration = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-979d655dbd45021d"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnAppearing = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-17ffee14cb639cb5"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnClick = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31477) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-2aabac1082cfa0b2"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoInputs = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-ec7811248978bf2b"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoRendererClear = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31479) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-5f64e5040cab6b52"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoTaskBarIcon = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31475) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-4f2cae143d94f35c"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-1c15c1c6dfd00954"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.OwnedByApp = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31473) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-a0bbf5d84162b6cc"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.TopMost = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31481) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-b0fbdb9863f65ba6"></a>[`public float Inno.Native.ImGui.ImGuiViewportP.Alpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03edbe59b219f92d"></a>[`public Inno.Native.ImGui.NativeArray_1eccc798bd000925 Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4faf3ca747ba4c6"></a>[`public Inno.Native.ImGui.NativeArray_b3004bc9c15942c6 Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07a313d41949adc6"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a81ae790d5088240"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd035b114d2ca616"></a>[`public Inno.Native.ImGui.ImDrawDataBuilder Inno.Native.ImGui.ImGuiViewportP.DrawDataBuilder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70a641c318103463"></a>[`public Inno.Native.ImGui.ImDrawData Inno.Native.ImGui.ImGuiViewportP.DrawDataP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e82498f053e0d3c1"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.Idx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d950429b194008c"></a>[`public Inno.Native.ImGui.ImGuiViewport Inno.Native.ImGui.ImGuiViewportP.ImGuiViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f18cbedf75a6ccb5"></a>[`public float Inno.Native.ImGui.ImGuiViewportP.LastAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1c43eb66526ab0b"></a>[`public byte Inno.Native.ImGui.ImGuiViewportP.LastFocusedHadNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-da0e73c47047e677"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.LastFocusedStampCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0989d46f1b37a08"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01426bb0313be199"></a>[`public uint Inno.Native.ImGui.ImGuiViewportP.LastNameHash`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31505) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d5a01608ab94cabc"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.LastPlatformPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46691feaa92973ee"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.LastPlatformSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8177a9c75e1a6d0"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.LastPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31507) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd79a7181956946d"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.LastRendererSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31531) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2e7d7d998ae985f"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.LastSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd8b0cad0163684e"></a>[`public short Inno.Native.ImGui.ImGuiViewportP.PlatformMonitor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aba34f852e003ad2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiViewportP.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8221fe5bd4915bd"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.WorkInsetMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31535) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b03a0e8f5bfd8bb"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiViewportP.WorkInsetMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-80dca4416e80591e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Active`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31647) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69467560368323cd"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Appearing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31661) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a8012777649cb8c"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.AutoFitFramesX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31687) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d9a6e3cc272bb167"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.AutoFitFramesY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31689) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e7e2c3b0d9621fdf"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.AutoFitOnlyGrows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a558e19a7dea30dc"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiWindow.AutoPosLastDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31685) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-420b3bc302f20b17"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31675) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e1f46585d9db7117"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginCountPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31677) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f795c56ea1ca5003"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31681) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-47ca08f9349c921b"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinParent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31679) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ca55ad39d77092c6"></a>[`public int Inno.Native.ImGui.ImGuiWindow.ChildFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-79d795b0eb4215a6"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ChildId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-731af9d100384c9c"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31719) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8922cfb7e0b10024"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Collapsed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31653) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-368287765ac98e04"></a>[`public Inno.Native.ImGui.ImVectorImGuiOldColumns Inno.Native.ImGui.ImGuiWindow.ColumnsStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31735) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-59d8cd54ebbaa1e4"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ContentRegionRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31721) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa7a98c6f5c41624"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ContentSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L31587) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
