# Inno.Native.ImGui ABI · 12

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSizeExplicit 177310f94c6699bbddd85cb516f4e5f9b341f1398416a415bf71488d365a5e1f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSizeIdeal 60e2422f5d19621825adae23640e3575418f5fbe922e46b4590136166b93ac08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Ctx 49c78f6d4660f2cdf31c92d71eb47ca46b76570d839b29763642dfa5d350923f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DC e05d46aa35f1c561b5e5f7580a4937452514c5a16a921c8d1c0c5ec9ebe61264 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeX1 6a4769216bb4ebfd74f5183de87e4bf419bb638dd393d64e09e42fb23d885209 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeY1 483f81fea96c236207e846774beb7260eed3c68841b1222c81b9544b72fc4b5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX1 84f3fb84e5255a37df679f3efbbd0df914ccbdf811a7242150c67c870f6a700e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX2 2d061576f4b769a105fd1e6544c97746c1ccf1658a84c90c423d4869b5d8869e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY1 f19bad36efd3c04e7ade7c12dd2b99740ca4982fbe6326cf14d2e2ed48f6f3d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY2 94efb7478319cd712fc6dcfa6e29a3920d8d3228926856fa63eaa27e0fdf779d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DisableInputsFrames 6a98ecf6505520f7df1625cedc304ea1fa8a969cbbe3af2660dac59d5bbb800a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockId 23052e5f1586f19d40c14fd5edd385a80bc5d1a99a5840bc94f52e756de8e473 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockNode 41ed8d8522f79b37322155850327389dc75e356ee884a17952cd5b296841ab69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockNodeAsHost 1e1f43a7d01c42538de30c2f6b58a097f198332a16ce380c57a440380608f441 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockOrder 4232db82a18272c7b310e1eaa92a1b4df0e9de8e49f93eeeaf26681bd624353a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockStyle 0a6e2eebb97673e65ea245d96fb1d2ebe74cb95c655b566e589379705e5d8546 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DrawList 2f0b1d42a37c52b8e423f1c42e0ec638049bd5450ed276cc5f874b4b1b0c4b81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DrawListInst 8e5fb113a4a620508ab0f906ae178f8745ebf8bc711a4ad53f6d10594d05baba -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Flags 2776bd559395ab9ab257d294d9d490c2d832bda83e33db161c628118fa1c7260 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FlagsPreviousFrame 5de9c50d106b4e5980c8d66a26c1b548d8d0d7e70dbb58de12a346d1fd97b1cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FocusOrder d6db49de6b89c35ccb7c463f795d1d9e909019d5e8fac38bd0b75eb7649fb3d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontRefSize 6c02155efbf336bdec901840e739f3a379aeb85c41b3465020a3d8f6e5731e08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontWindowScale 438f23f5fef8de08e27f13c87988315417bc70c43c1efb84e95df402c0034a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontWindowScaleParents 666bad46ca2f243f2564fa6d93d430514df254047b8c088cecf4e51227804942 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HasCloseButton a3ec5c3eaf083003f2bf217a2fae18f4f4a04ed2c6f40040644a2677cb9bbab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Hidden 04edbcd24a0cc21bc4408d8653463302233c8424c8768a11469702154d0eb557 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesCanSkipItems dec0baeae60c04339553e255c5477909b056da7dd7193b5f50cab8383b2e6563 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesCannotSkipItems d25de5f14a353dbe665021230395edb8aedeb71ce9efbd1bc59849fb1c5961cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesForRenderOnly 1fce7d2690b9d575b9efb2c2a26a122a28ddd4e6cc6ee7bb9fc70f45ce8c7247 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HitTestHoleOffset b462e26d215a5731cf02aa7e148c282c4cfaa02ebe63bfcf97e4034b803d0f10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HitTestHoleSize 7ffecaf8335b68917fb8fb05a00d2a042cfe471dd438273612b48d6fd590a879 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ID 1cf64230b2fdbe5ba812e21789c586d2fce6e6eea4bbd914d5f5ba98951f152e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IdStack 230aa2c122209e1be0303710e9836212c6720a1233c7b797b73ef0962392a8b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.InnerClipRect 80ee33dedc658b8183f2b9bc0ae86195917441e6c105caae5d94df7ea52a2310 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.InnerRect 3614047cb7fc66e53ecaecbc7d7eb8b3bbf17f08585f6c3601b324d9e7bedcd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IsExplicitChild 7fdaef5ff07b46e1aec582a89aae2a3ba8f8e48dcc5c6136eb2ff4f8f9556bcb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IsFallbackWindow 143c1827c81a868e378609165a1cd54d7ee1e543e2e898126a55c577429554d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastFrameActive 79bfb127fff75545cec12ad7f18f32993a1a9258f194e3d3f92c7138e987608c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastFrameJustFocused cae2dbb5e09d45096018ac4a8ff6bf33ea1a4b5d7931c73a522bc1b6dbf93ccb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastTimeActive 98f1989ca4d196543eef7f389f32e0311709213ea211b8d684fcf54ac35bb1b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryCompacted 4fdffa6df563f028503a12901451fda0bab699cfc09ebceb5963c28a039b2b9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryDrawListIdxCapacity 8793502507ac2401a4b5b56db25057be8e526f8e93b50fb3405381deb41c741b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryDrawListVtxCapacity ae7e0b2731675684f66427480c485882d82080befeb31557a0ae7b40113a3645 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MenuBarHeight 588f3a750c3a393f058e7479c91374a0a33d2ee175f3619ecc8d4806e2e99272 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MoveId c97223bfa0349b861dd7feb2a6d3782354edef87f6a06fb001e9336d286a7a51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Name f422c81da0d5303cc9df5b817f839af3ec8cd5a028e115964aad0fca4e7ad90c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NameBufLen 6a35fc6e51d85bf7ab2eb77661c3eeb8286dc5193796cde3a8db9adc8d607577 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavLastChildNavWindow 4840fc253e3141f7ab8b95a5c5b453c020aeb85ed8a986ce0e3c2ec9dbf1c615 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavLastIds eb6366ae932792af524642f27c5c2fe0590fecb972e197362b820ff098b4f22c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel 316c387e944aaf542ac121f0c817cb74d6e15deb5a39ac6bf3da8af2907e2339 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavRectRel 4565aa876516d3a60018da93fcdb6edf0022844d3d5d55fa64acf701f49d0389 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavRootFocusScopeId 21f220836078f6e44e9620af718db0b03e7fcbb8e2d0277ea4bce9f4000b7804 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.OuterRectClipped 5bc85ba68be511a78ae90d5d4d12ac5f49c4a15b6a4167de10eb84e944771b03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindow c4f48ff1359ae798addeaaf2711cab190c8ed4dce0d5bdd82eed318b0c216e42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindowForFocusRoute e6a3c5b9ba5e9e6f66170eea4bd0f0ced3bfd8df91c0616aaa65b583ff0fea9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindowInBeginStack 721bcf4ce6545c7416f1cafa51407fc64156ea8eaf0ec1bac0589fe2e4240443 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWorkRect 0541bf05df99ab20e18c27b18f589166e8327f6264080e5d6316a749ee4615b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.PopupId accd526eec6aa04dc5e8fc624bf3862dd696bfb7443be24bad3b2e2827b8283b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Pos f00e03e1041924488dac1867d03e75a8d21011254b8b63498d2bdb701ab86925 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ResizeBorderHeld 53ceee9c214d66327146862eefefbaaff66e266246740959a1f4f8164cbaa8b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ResizeBorderHovered 39d5f99041640ce1239d352e652562360c809f20ed1bfe6965243b7f0e36b03c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindow 5ad622d3274971ad859b66efc8612d1f759dc7f9a0de8370958c09093b20ee3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowDockTree 6ee3896fc2705d62c891108547db4923596165e7bb2c15c17ab1985b4191a162 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowForNav dd10250ed4c058bd316ea2705c9c1846c29abba84af69a48980dce0204c0df65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowForTitleBarHighlight c563b89c4acab4b9dc73877daf56cb21e961d91c6906cd79937c84aa1fab416f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowPopupTree cb6b3fa3a33cfd605ed502d3ab8ea520340dfdb983459e7043a045793bdaac42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Scroll 57119ced0ffc12dd6e39b894a91e3d85aed2ca4d20042cbc31941d3d15a25535 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollMax 492df71d2c9ced6fbfa3a5ba147eb8c5528206549615ba06a719624a785261e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTarget 60836e467bfc55b972b0450b1b49866a782cc4cade0e3230f1ec6af7bee03bea -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTargetCenterRatio 1657f5d71fa20c19acdf21152e410e658d31d1c05e06966af3beefcf1bcc300f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTargetEdgeSnapDist bed266a2e78cbf9ff82d3bb9d964596508ad36ae952404e9688480af8b22a9bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarSizes d3c45d96691e17b9651766a07fee09a5f8d55464922c172483640ba3a8a6d77f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarX f2194d7c7eb1f654187122a1fdb5860192bab396a6e83a39aad9c38e1baf97d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeEnabled 95f3896ea6f35678f9575f11eddb2ae504a1de67c10188eb1aa044754e2035df -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeToggledHistory b7d904be74ab9e467b229483821f90510a915466e3e904ab60ce8a8b7a43ad9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarY aa82bd9ba94537de9c7be2dab0cf2862a42057c0fc35b3f9dac979b012457ad7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SetWindowPosPivot dd6477f53b731f117f5dbd3ea323b3da13b54aa0e691899b6cdc20848bb2886f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SetWindowPosVal 88227368fffefac7c5cf23a475161ae6734c0a1fc2c8ee1877b6c4a6b46f801f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SettingsOffset 6b929d719a6e7022b04196f00fcbc7db8596bbe409d9c849439d9a80de2c8bc0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Size b2c6a3058c7e4cfceae1d5ef63a1b84594658d6d2d256df01bf6ace0df757768 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SizeFull 790ff935a24f4203cc1cb92ed2b00bdc3e464b7a2849d13c0b6e1d18c2fc0a55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SkipItems 4a235e302c78116ee1af379b155fbdaac0a4ff0ba16b54858ce3379134acecaa -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SkipRefresh 132d12c60e003ea1fd83c37f4e559815b45186b9575d4c105198e50a8cd60ab7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.StateStorage eb601cbd745b89b7cdc2e17cd394ee9a4ce8f21f9ed674f49c326fa9f8bf37eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.TabId 6df9eacde57f4840f3ad8990632daf4e942b0f377813e7b7496a74cd2c047ee9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.TitleBarHeight b2976c4353652e4b76bcd6b62acafee41fcb0ba004cc1d99501d8d04b2d6e8a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Viewport 1566afac1c1a42b8c360b427e489c56fe9dc0382007562a38610ec33b7f52e12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportAllowPlatformMonitorExtend c727abf1be03c57c3346c88b1862ee78d8fbc6b33d7a1f99ef6af5816f64ff90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportId f957c9a72408a709bcbf89b1f93f89922169bfd49d616038d85653193e7c1937 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportOwned 05523704b73b4118294a90cb507cdb365f0f56651ec6bd1aa66e54388ad58319 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportPos b5c9a45a8b060b71a114a4b118c84ebcec06326386afeb4b25d5d9c065051a07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WantCollapseToggle 3c82e549926f39966381e2a2d5e9ccf3dbaf83513ab1ce008324ee17971d9812 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WasActive e6b6d02de3dd21330e89092616335707b1e15718d3cd8ab52530c3748b7be19b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowBorderSize 8cf86e70872a566394b2fd9cb9553624c1e99c849cab8c63d4b858cede20139f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowClass 3c8a76503b0a75a31bebfabe24143a7325b80fb0d31cf2cbc0405f0719685ed9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowPadding 68d738c6077f636f716297bc3ed9875fe2e35f39727f4673bba6cd91c45ed2c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowRounding 8461cb94e205ee5c109eaa7d10e3795aef02e3ab0405ba56b51c4b6a463945b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WorkRect b9ca97bf64be12a41d7e8f717fd7c4eb0cb039f53b97cdd34336abeb7936b803 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WriteAccessed ad6db20800a75711c1a64295b4b88888a857206fb88aa5cfcf83d009affeda0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowBgClickFlags.Move b58f2f4a7de18048950436b68838df3c9eeee24ce1892590fe6ba2b7e6b967d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowBgClickFlags.None 0b94df2082d7e1164ad7b5d5cb853f06d8fac1e73c143c3b81eb2a0bf205253f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ClassId 2705421d90d5f19f10f4da4460e94d5ce60533d02a3ffb0081c32cf5f79b7f13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockNodeFlagsOverrideSet df143fbf85cc2770635fd260ad841f590f33d164d9b161915790ec89b1a4994b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockingAllowUnclassed b47b61fd1f5b6d96185d8ecef1fec832ef167a8e043af70ea04ef6671600c567 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockingAlwaysTabBar 75e17b864dff77a261ea9f13a7f219b4659d0fa1f2964de6558416fa74fa4e89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.FocusRouteParentWindowId d8c870c44668af0ea034b1e6ef79f313f13abd266ca4f4798049bb0d421ef133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ParentViewportId 8463b45b1b6ce1d78df198d863745c12b04f8ee643dab06fe6ebbffa66c544a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.TabItemFlagsOverrideSet 5f69e628c2fc63a50a8b824c36e0287ce83afe9e94507c5c547621a87124268d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideClear 3d7626431531e02ce7b45346ffd8a3d0cde6b4af01c3957393d7bf4eb8525568 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideSet 7cd57554cb2c21737176e83464884df588a80452a46b6ccba9965de5a2329ea3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors 9443b0f5450c3bf382283eb09ca6ac8caeae75c5c3315a79772e22591badf996 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.Count fd673b0175f15666f2f6f88415baa2089d2f22a246c28605fe5502d6c408efad -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmed 2068abba28caae5d047418b697bfc1dba46653a4f0d190845a2c90a5388c65f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelected adce32789152b389cdfc416d36ff2f14d749908091ba74a1916cf4697980f6f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelectedOverline b5455062bce4c46c9866a97c9455e8a5ef5a4b5b545fdc97b49b58842c3b3969 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabFocused bea1a433abff591f8d00b009c37840f0a7c4e9edb2170319aefb1fb555a9b5ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabHovered 948aca87e57dcbb22e730b0ede4e516e94dcfdc72cb9b372fef322f77a835107 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelected 43b1d7c8210ce38aa4ce5c3c98e16805ac4797b59141a62975da2159a539a954 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelectedOverline 3eb0ad848d20fd65799e31e291ff6cb35c98daaeb141fd9556547e56dc908a91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.Text 606ae666cc28ad0485e4fca59d3cbbb266ce9e40f400c6ae5427c8e2b48db5cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.UnsavedMarker e6769b50a798e16592ce2a1e258369cfb1171fb4c5606b20d6dc1a15a90590eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysAutoResize a3907901a65b84fe6bdce21dcb44adb5f5e976943a410ecd4da617d3ebefbac3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysHorizontalScrollbar 1ce26edebb4d8204c0630c62a0142383c43b62cf4fc04b6fe6f26ee6dd2e9a09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysVerticalScrollbar 240c0374fc0b4d9e1b8b3e576386954b78d555f051213f59ea6d0216524ec8b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.ChildMenu 16735d6c6519b8c7741e7780a280fd28622a7d04ed0583ecf70ee81c3db50bed -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.ChildWindow 4dbef0793a95eca4be246e6abbdaf74e4a9c316cb7f9e2a5cff50cc5381e1660 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.DockNodeHost d69e26321c06c86a21fefaf978855fb4483b62d85a976a32856e9fad83e24599 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.HorizontalScrollbar 5ece295352288becb9e870d024b779eb5b3beb3c99052a766c51e1d82a88d940 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.MenuBar e787c64d0e2ed51ff1aa7828ea01c6e2c1c8bf2f15923a5856c3df8353a3fe94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Modal 817154d4cb0f94db44b6cc8d4f0ca1cc28c81c0c7ba610de4b31afd73fab263d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoBackground b93d1d1ff69f441f4cbb82d7d6461a6361e28cf6cd2cc2e11e41a300c58b217e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoBringToFrontOnFocus decfaff1ce1d9d5ff94f64fdfec1ee7537a069ff9d13d866c90957a96a47be88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoCollapse ff42a7c3368a7a8432a08ed563cfd567452eb68d87367de1625cc472998bae49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoDecoration 8de2384363287e8f9caf950758516c5b76e4124a84f4778a1af9a60bd79dfabe -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoDocking d216a660ee432da378f016ba8a8c97c3b3ce942f62fc4de23b2885d47c2899b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoFocusOnAppearing dc728c3fee7d327becd62cd07f6dabd1dd75bb34bd2a47dd9f760a7920087d53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoInputs 362684f79e03eb5f6c8e5562557332b152da38e53236ec96cc0c36fb1012ef5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoMouseInputs 6ccaf9ce641d06410919a5d7d11adca4897fb375b00ee83d1c2e663f15584981 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoMove 816b6fa50a307965c431c2dc014e7d6e42a2a9b00ae4a06fb63100dedc3cd7b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNav b7e53a431adb05ecbc3dfe2992174c52a2d327987e3b2577fa71cdaf8adc277a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNavFocus 98e13b35afaea0ddcbff957859b64330ffe5a8d86e8242d2b35fb36bb3df6275 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNavInputs a73c4b075815c56809e337319ca38675e1b8c8dfdc97b9370198c56bfc38158c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoResize 875640a55f410a23a7dd4bc63bb17ff5f7237775b5a078c8a83a84e565b8cd90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoSavedSettings c7899d39d5faf926416fe95018cda71782456a164d6eedd4bc64c468b07197fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoScrollWithMouse 2310e2d408022f45b4953170c622d177457be5d1a143046ba759cb059d7776b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoScrollbar 10c250fc04b011341bbb6416adc0706cc760633e976dfb01619dc5ff7515d6cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoTitleBar e0dc9d459797a0be3364d2747afd84c362fa604d0d7d7f55e47eaf0d638f644f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.None f045c2c0e19f0e3b4e71fa297bcff1789f51ffb2dff263fd76ae439b62c13b59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Popup 07224abf6a0dbd4a1328cf306a1a33eeefa67a9b212708446c354d7bc4c26c08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Tooltip 565fd5e4f4b5cb44a281707e4c18f379c59b2c45e71fcd9bc6c7455f226c48ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.UnsavedDocument 8f533f8a810571e0e084c8fab755c1423964b4b611c90ca2a8db0f3e0180e8c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.None 054a6bd7cd3838ad5cbf9e803d53dfdbb16df5d31af51edd5436c3cde3be615f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.RefreshOnFocus cdac2f62f0edd4eb68ba2ced93cea444a887057fda96bf8a98aea45b5da7c031 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.RefreshOnHover 1ae82e90ddd39b7b1048e54ecfe921260325bba59cb923177df37a853162c528 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.TryToAvoidRefresh 1be565597f401ae0a45c31256a77d9206992272afbdf706eccc87aad66792482 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ClassId e53aacca933ef54a88ba8906a72c563c22e3b78552f3c1861ab8bd69627ee721 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Collapsed 4a1a3831b8461e25270ed33eeca31199a0f54cf874d02688cedcea79129db586 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.DockId b4b6e133e63cc087150c6abd0a12e4a0880ddc8268cc5089777e16409316d5f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.DockOrder 2cb54d9c023d0a3af29c4a46165d23c013ce9e28927ed1ecf8a4fa34d0c8a65a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ID 6af8853414788dc884cd22aeab1a6fe6e27edcf738fc4614e32efbd77fb5c7e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.IsChild 782b2dccbe5ab8c376c08b78e4c455b0d9dc9b6f60bd201bb503bff95fa9c288 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Pos 2d59b49bb41bc179377d57f512b2e9b8a6b772d0c8de3653891ba5e5d3c1645c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Size b61aee0e948861ecd7bfbd9fc5a3403d12fb233c045471bddafa74ed939dfee5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ViewportId 2967e60558ec524cbbf14d0439559fa2e59b7380824d13b837a2efdf448d5e43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ViewportPos 5c05c837b84718a25e4de57abccb2851a537ef6a442ed1db7157135cd036ff89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.WantApply 35dd3326d35a72baa89ad7ab30eba3d8dd1b8bad8195b4cef7745c9ffc2c342c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.WantDelete 123218a88cc64b95ad83adaea163ec9f1f29d6c40f04df8ddc50cd05cfe49532 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenable 932a6370b5dfcd784e01ceb9b65687e8a275050f090e1e6d7cda2131ea7d597f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup 80cd099eec3cbbdbb73a60e6769839ca107b7ff0dd3303f171f6a68784b1263b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.ParentLastItemDataBackup 0933fec14c6668c94a39da687d9060e2b56ff13b48520d1b6e56a4bbf76c3dd0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.StackSizesInBegin ab1ef4590675cc21d314f678e4a9a6b640055eb42c661846fb3ecb85bb078320 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.Window dea37fa6b1c249065c7fb13bfb136525ba9045f0c3fe1d9bb6ad362b44346623 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ChildItemStatusFlags 74da8464e2b23e6f5b0221ca67cbaaa7cdcaa3b0d17aaff627d999a6a52ef23f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ChildWindows 20c630aff1d0337dcc5bcd9a5c0e5b6639a9795f828215cda1c9c6827cd6ebe4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ColumnsOffset 01b07e1e9dcbb6c1b5bbc020f77717e8797a8cc25c55d3cccc24be6e488a470b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrLineSize f9f3548564f2fcd26da83a4f23398d64c8698eb12e77b4d74343b9ddcb791dad -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrLineTextBaseOffset 8e65d5eb9f422b0f0eb64121f6d1805da728fa367e2bd9d9a7a8f6e25b661c47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrentColumns 56488fe3e6e040bdae31c7bbf783f02b148d1dd64637e493849f99665bc48294 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrentTableIdx 84849585b74f46ea17e23eed7c981a9310c7fd428394ad745d8f711ba5269da9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorMaxPos 37fd8c6dc08cb406e8e7c1d2fdb5b9bef0106a1992b158690cfca56ebc8933e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorPos 97821637549577e67c42f8687cf475dbe0339def04c72e1f8f0da9745bfeb0ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorPosPrevLine e656e061f81ca1653ad38c0b525763e6952584eeae743372964e53f84a332839 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPos 6ed2a9a75dcbe05ddff744a1af111ffdee76aa754d2ca00f89052af2ebe66456 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPosLossyness 4ccceab7c505a4d7e08096b757a7152669a0d80488d53e5d0bbde99b37a5aee2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemRect 5d6982d561132a8eca601236e9d394505e96391160289b0e1934a0e2c27cc733 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemStatusFlags 761087028c613c7aeaa241b65a59a176bee55cebe0551158338814995a59b2b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.GroupOffset cc47f6adf5bdfea69be2ad46215fb4a0e7eea6c8ddb7807a33aaedf4b82137e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IdealMaxPos 9260b118e06d387dc943240069d072c9a75766fa9885bc34cf5560ae1489ce70 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.Indent 64698146d54c033fc3bcc15a3085ef1501a713c4c0ad5a08a0e76a8d9fe064e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IsSameLine 315e01a8f2324369733e0c31c6b7344b2f51102f8a320801c3f42c1691b89515 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IsSetPos 635f5af6f477f4bef4d9acb62446e1ca48bb2ce147bff06e66a1c601cb2b65a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidth 9eed310cab6962832d0826afd9c09bf0bd98b0e39fe58133ebc309d26f0dca06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthDefault e16e7f0ad6633c1b56da5cbf4aa64929c4aa097bd04a325410532a70bd73c0cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthStack e0da56934b4fdc5ceef4a91dfd9fa9930d8ececeda3ad820da4ddae288154aa2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.LayoutType af9f3af0aae820fdc364887b842e216f186454d754c27d0fc1b7d25b04238ab2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuBarAppending 58cff1a3e2bf807bd57588780fceca61d7ba1baab275afa16c1805429afa62a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuBarOffset 1765dd68af3a5d7a145af1fbc126902fb35df590f000ab0f8d6848ba9f6045c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuColumns 96a86883f03a2521d34431c5dc88da2b87aa92ef9798a70f494b963d120ea175 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ModalDimBgColor 4f630576ca1d9692453ee7a71c0389e11c82185f0afe768c28cc416af4a46376 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavHideHighlightOneFrame 8c71afeae95bcf98adcff1e0f6afb25d67566f28de5e4f97fbd99b746912f334 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavIsScrollPushableX 075e84ec6fe3d5ee2b9417366b6355d8f40314b6fe6577f0890d20451b98c4ef -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayerCurrent bb40e928ec4f39e69fe6f34013231765529d0d8d981983c7458376ed6b6b08b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMask e078d667a29d787f6f6ed163ca6ff7fa190a44b7f5ca9ddee883522e0807e3de -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMaskNext 76e273d994c473919812704309a40f3662b6dd6fbbcff7f4482910cc317825a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavWindowHasScrollY 29038cd56609fa19dec5f6fbd7e477a3a0540a8e0e313a3f451c00c12378a16f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ParentLayoutType dbec56f6562c013269beec2df7693f0d8af6ace7f2575b3715d629e23f0fb23c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.PrevLineSize 53d5e1065137e1ea4269fb074f8be4971267c6792a82703034bc3a022aba45f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.PrevLineTextBaseOffset 14746b8aeafa585ff8ee9b823c6f07ad2f10c0b97ab2eb226140b68fdf2c0d35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.StateStorage 33f9217982bf1a863d34eabcd4071785741829bbdc6d78c74d66befc32ce2c48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPos 5dbc5a265122481543a22769134310b87d52a2fb133e8df14b126785820df939 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPosStack e00245c889d6d4347ee64710a90b5227c674ec2a1e604f891fd35b442f0da7c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeDepth 7b48c9dc1a3a002543d6f69442ada3ed2d133ab40ceebe3da3473f238dd5240e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeHasStackDataDepthMask 00605da12741f901d7fd4ed3a7b377fc0e50cb0567f9f299c22856291cfa61a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask d3ffbba6ba1bbbbe06ebc8559f63f0551ac2bb152f86461eb510b0f90bde2f84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.WindowItemStatusFlags dfe87c92ea9a087d2a2c8dbb40ac78df269723658963c5be9b8454f796dc6b4e -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.AliveCount f3a2a2c84f40162766c5c7d435003b66bf68d4144c2926220a2572ca202e08e0 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Buf 5a1024fd5c15763b8e94637017c0f4f058330bd01c3a3951b28fb41cd70ca13d -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.FreeIdx 3f3216e8b05e4089cda67f501325d6ab90e92085e69d8458281b72f8c92e8018 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Map 25aeebfb3d2b693af3a9a6c20293968623d60f5b3bbb47739e930fcec5493b6f -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.AliveCount ad7722f4eaa3038538fcef822f3060617e27d499333c13aea53e6826417a5cd4 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.Buf 436cc082e5f679d0cdb10eec3ac3c5937b67a876f876cbbd8b27b1bb74218d7e -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.FreeIdx 0c09bdaacf9ca81f4cd0f60936c755db4ae4c7006efde1f5858eb15b1bc8fa1c -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.Map 444ac11551105859d8fd7b954a18847139b3b03c85eaadc1757ba23022b363b7 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.AliveCount 2ba64543d1b9bceb0fbe1e6595f1df0b6cec742f01db6a5b9bb6af6b0a4b014e -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.Buf 97b1346c52d73a690adc3752439af3286c3d0a238494b1e9945d867b30965afd -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.FreeIdx 566af8c4049121566219884d034f1712912d111285efeb6eebb23aa2ecf8c4ed -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.Map a3f9d6ea369687dd3f76d65d90d893b2e6f28d24796a19425e1cd5db35f8905d -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-2c99d3f000efd663"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ContentSizeExplicit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32834) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-335b7f98d7ecd17b"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ContentSizeIdeal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32832) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-92d3a281f3fdb2ad"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImGuiWindow.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32802) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa2cebf9f495ccbb"></a>[`public Inno.Native.ImGui.ImGuiWindowTempData Inno.Native.ImGui.ImGuiWindow.DC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32950) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-694b8444de5377ef"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32854) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c2301d2d2383bbe8"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeY1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32856) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f888b8cae35b0bcc"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32846) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-34aa076504db78e1"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32850) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-489d5e6871980308"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32848) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3bcedf37081928b3"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32852) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9dcac30eceb910c9"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.DisableInputsFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32942) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-49cadf083fdeefdc"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33032) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b86dc1deb1e4269c"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiWindow.DockNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33028) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec0ee9516e9cffdc"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiWindow.DockNodeAsHost`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33030) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3ba1e6d244acc2b8"></a>[`public short Inno.Native.ImGui.ImGuiWindow.DockOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33024) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e0ab33a745f637d1"></a>[`public Inno.Native.ImGui.ImGuiWindowDockStyle Inno.Native.ImGui.ImGuiWindow.DockStyle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33026) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8bb73f245ab27416"></a>[`public Inno.Native.ImGui.ImDrawList* Inno.Native.ImGui.ImGuiWindow.DrawList`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32988) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f4a7c2d58c16c45"></a>[`public Inno.Native.ImGui.ImDrawList Inno.Native.ImGui.ImGuiWindow.DrawListInst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32990) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-31639eae3625393d"></a>[`public int Inno.Native.ImGui.ImGuiWindow.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32808) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da278c7738fbb30f"></a>[`public int Inno.Native.ImGui.ImGuiWindow.FlagsPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32810) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62a3c655ca286844"></a>[`public short Inno.Native.ImGui.ImGuiWindow.FocusOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32926) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3a364ab39b523f94"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontRefSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32984) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd630d985bf35286"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontWindowScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32980) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-faa40eb756c95580"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontWindowScaleParents`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32982) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dedb9f78c164eec2"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.HasCloseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32912) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2302e9889c31187"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Hidden`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32906) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d3ed9d47a0c93e79"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.HiddenFramesCanSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32936) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6de1229a0383e12d"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.HiddenFramesCannotSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32938) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b3c0c62959c98fe"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.HiddenFramesForRenderOnly`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32940) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a765bd74e93f226c"></a>[`public Inno.Native.ImGui.ImVec2ih Inno.Native.ImGui.ImGuiWindow.HitTestHoleOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32968) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38145a8f32d1b839"></a>[`public Inno.Native.ImGui.ImVec2ih Inno.Native.ImGui.ImGuiWindow.HitTestHoleSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32966) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77260794b6a18588"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32806) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-542bf39df8841304"></a>[`public Inno.Native.ImGui.ImVectorImGuiID Inno.Native.ImGui.ImGuiWindow.IdStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32948) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ccff29a8e615617"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.InnerClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32956) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-636f470e6d7929ae"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.InnerRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32954) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9abd45c8964a842b"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.IsExplicitChild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32910) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0d68798066d9718"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.IsFallbackWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32908) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-591feeaba7c13970"></a>[`public int Inno.Native.ImGui.ImGuiWindow.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32970) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf0faa9fbce5b869"></a>[`public int Inno.Native.ImGui.ImGuiWindow.LastFrameJustFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32972) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4224061eb8ad84e4"></a>[`public float Inno.Native.ImGui.ImGuiWindow.LastTimeActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32974) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-864d3354071f40a7"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.MemoryCompacted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33022) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4a3624e577065095"></a>[`public int Inno.Native.ImGui.ImGuiWindow.MemoryDrawListIdxCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33018) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74c41a44c7722677"></a>[`public int Inno.Native.ImGui.ImGuiWindow.MemoryDrawListVtxCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33020) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d84a7af9cf848cda"></a>[`public float Inno.Native.ImGui.ImGuiWindow.MenuBarHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32844) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7c90b97c5a43e77"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.MoveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32860) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba4bfe0112400fd0"></a>[`public sbyte* Inno.Native.ImGui.ImGuiWindow.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32804) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-554dfe5fe9a3b941"></a>[`public int Inno.Native.ImGui.ImGuiWindow.NameBufLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32858) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0168fbc69518a8ba"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.NavLastChildNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33008) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ab9bd37d51c04e25"></a>[`public Inno.Native.ImGui.NativeArray_16a683b69e90f4cd Inno.Native.ImGui.ImGuiWindow.NavLastIds`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33010) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2d3b322b9b4e14de"></a>[`public Inno.Native.ImGui.NativeArray_b726fff0e8801db7 Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33014) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc938c12cde08e2a"></a>[`public Inno.Native.ImGui.NativeArray_87beacaa77c69f9d Inno.Native.ImGui.ImGuiWindow.NavRectRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33012) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a6033e5631feb0b2"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.NavRootFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33016) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2feb9db7078541f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.OuterRectClipped`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32952) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8593154dcd4b38c"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32992) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-077758854e3d9bf2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindowForFocusRoute`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33006) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-40ddbda550e4b89b"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindowInBeginStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32994) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a9957bf814bd582"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32960) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22dfeadcebb789db"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.PopupId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32866) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99f04f7d751abdc8"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32824) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a7283922fafd711"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.ResizeBorderHeld`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32916) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a3b9e934bae749fb"></a>[`public sbyte Inno.Native.ImGui.ImGuiWindow.ResizeBorderHovered`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32914) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9fbba09a41a715b3"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32996) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e03966956e3ad821"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowDockTree`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33000) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-37934f85547dcfeb"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowForNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33004) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce7966d01ef249ae"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowForTitleBarHighlight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33002) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ee6f068c35cdf381"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowPopupTree`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32998) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bbdfdb9c289da5b3"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.Scroll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32868) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dfce0e2bd3409247"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ScrollMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32870) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4889f5c8a7c6d243"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ScrollTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32872) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21c51818097c87e1"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ScrollTargetCenterRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32874) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ae33a242dc4b8b2"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ScrollTargetEdgeSnapDist`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32876) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b14a782b4decf598"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ScrollbarSizes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32878) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8b3ccb8e4a533ea9"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32880) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-41d93596d173cc9f"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32884) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c64c32e49af23c85"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeToggledHistory`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32886) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7617ff020df61f53"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32882) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b6f31ed66d9a980"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.SetWindowPosPivot`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32946) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-15a9dd191586a787"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.SetWindowPosVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32944) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-18066a3077e6c2a9"></a>[`public int Inno.Native.ImGui.ImGuiWindow.SettingsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32986) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7affa54bed984ff"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32826) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80c0a125e670f8fe"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.SizeFull`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32828) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e47daa5e5118fb2e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.SkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32900) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-01eea428f43bc1a1"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.SkipRefresh`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32902) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4ef93030f84ab0a5"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiWindow.StateStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32976) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-262440c2aaca759e"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.TabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32862) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2fa364cd07c07f3d"></a>[`public float Inno.Native.ImGui.ImGuiWindow.TitleBarHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32842) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-341fc7fa1e5b59bc"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiWindow.Viewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32816) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-31676bb86d1a2341"></a>[`public int Inno.Native.ImGui.ImGuiWindow.ViewportAllowPlatformMonitorExtend`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32822) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-79e2cf1c05a182bc"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32818) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d4f11c3774e5666"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ViewportOwned`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32888) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8c3ab847a2e9383"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.ViewportPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32820) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d9a7e48953ef3d1b"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WantCollapseToggle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32898) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-badb43cf9f04a94e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WasActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32892) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a57342312b30b3fc"></a>[`public float Inno.Native.ImGui.ImGuiWindow.WindowBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32840) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c90be11f5b8e04bf"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiWindow.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32814) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ab7d1f30abb6514"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindow.WindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32836) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa21ee1d01997f54"></a>[`public float Inno.Native.ImGui.ImGuiWindow.WindowRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32838) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af3a8cd33c6cd6a4"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.WorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32958) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ecab06ebf2c36b64"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WriteAccessed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L32894) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63388454d905add2"></a>[`Inno.Native.ImGui.ImGuiWindowBgClickFlags.Move = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33328) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5f27b533be215db"></a>[`Inno.Native.ImGui.ImGuiWindowBgClickFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33327) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e00a3775db996eb4"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.ClassId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33339) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c2a5d15e5ab451bf"></a>[`public int Inno.Native.ImGui.ImGuiWindowClass.DockNodeFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33351) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a9635a8709fba30f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowClass.DockingAllowUnclassed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33355) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dc6512d01b5dbf7b"></a>[`public byte Inno.Native.ImGui.ImGuiWindowClass.DockingAlwaysTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33353) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ab0f4b9db11e33b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.FocusRouteParentWindowId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33343) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-456b86daa9dde79b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.ParentViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33341) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c66d8a9d5d1177e5"></a>[`public int Inno.Native.ImGui.ImGuiWindowClass.TabItemFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33349) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc07e819aa715dc9"></a>[`public int Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideClear`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33347) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12f2d544a46558d2"></a>[`public int Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33345) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f70baf1595949d38"></a>[`public Inno.Native.ImGui.NativeArray_fcecdbb15210e44b Inno.Native.ImGui.ImGuiWindowDockStyle.Colors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe41bd80698972fc"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.Count = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33397) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3da4f316631940a"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmed = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33393) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-65d8489a8c7d5b2a"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelected = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33394) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e085d208f4b4776"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelectedOverline = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33395) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-87a2cd64758d7221"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabFocused = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33390) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1994b402d7351cc8"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabHovered = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33389) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fae55d2f8efc0fb4"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelected = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33391) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb1e4b5381d88f13"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelectedOverline = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33392) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9ef0b64d675a226"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.Text = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33388) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dd95cc935822797b"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.UnsavedMarker = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33396) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3107eb2de586bf92"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysAutoResize = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33412) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c485c8d464a47a60"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysHorizontalScrollbar = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33421) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3e1277527edebc97"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysVerticalScrollbar = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33420) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-216a8ac02eb0a56b"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.ChildMenu = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33434) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-391cd2018009ac96"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.ChildWindow = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33430) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-be65c06811b8053e"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.DockNodeHost = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ca3d967357bf2d21"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.HorizontalScrollbar = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33417) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0b9a068669520889"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.MenuBar = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33416) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a536eb923c8e4b5"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Modal = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c564f146f9b22f6d"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoBackground = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39935cd83cadb4d5"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoBringToFrontOnFocus = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33419) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8eac89e2da3cc2a"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoCollapse = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33411) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-46d7f3310f721cf2"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoDecoration = 43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33427) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2c1a8a3216fb433"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoDocking = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33425) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1a242f418472cff"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoFocusOnAppearing = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33418) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ae25992af2430683"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoInputs = 197120`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33428) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-85248a96449eae75"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoMouseInputs = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22d852c3cd0e6952"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoMove = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33408) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5edb13ac6dcb743c"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNav = 196608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33426) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c64ab4f9b4856df"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNavFocus = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33423) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d95cfeba0469d6e6"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNavInputs = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33422) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-959cb4fad057fcd6"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoResize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33407) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df39fed2562f2b6e"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoSavedSettings = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33414) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5fde1189e545a0f1"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoScrollWithMouse = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33410) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3480dbbf9bab926a"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoScrollbar = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33409) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-572d7124fa48f034"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoTitleBar = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33406) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa798aa47da16614"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33405) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9591427aea941770"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Popup = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33432) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1db323a27a405042"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Tooltip = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33431) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0cdb780bfc563630"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.UnsavedDocument = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33424) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4acf3904b93abec9"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33442) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7e05748ec161410"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.RefreshOnFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2fbc197117ec3609"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.RefreshOnHover = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33444) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38075743be0523aa"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.TryToAvoidRefresh = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b9424d4f4d63bb3"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ClassId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33468) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d93cbaf8263b50d"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.Collapsed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33472) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa98c2da220d194a"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33466) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-543c5549e3e8ef66"></a>[`public short Inno.Native.ImGui.ImGuiWindowSettings.DockOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33470) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-19b9f71a9549ef3d"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33456) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f6ac0826f5a1bb6e"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.IsChild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33474) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2f9978ed631eacb"></a>[`public Inno.Native.ImGui.ImVec2ih Inno.Native.ImGui.ImGuiWindowSettings.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33458) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c37aee57e1bac8cc"></a>[`public Inno.Native.ImGui.ImVec2ih Inno.Native.ImGui.ImGuiWindowSettings.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33460) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-555cdd09107b2323"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33464) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d05e1212175ecbe8"></a>[`public Inno.Native.ImGui.ImVec2ih Inno.Native.ImGui.ImGuiWindowSettings.ViewportPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33462) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44eb9583854cf14f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.WantApply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33476) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ea6b9f78cc45aa3d"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.WantDelete`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33478) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c7d50cd6a243687"></a>[`public byte Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33502) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c2c265a92ccae49"></a>[`public float Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33504) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d4a9dcc128e41598"></a>[`public Inno.Native.ImGui.ImGuiLastItemData Inno.Native.ImGui.ImGuiWindowStackData.ParentLastItemDataBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33498) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1814e20a9521d803"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState Inno.Native.ImGui.ImGuiWindowStackData.StackSizesInBegin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33500) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-adfb19bae3dd4e3a"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindowStackData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33496) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50b57f9411d64a29"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.ChildItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33592) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e85fd61b471fd63"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowTempData.ChildWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33576) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-24d1a6c27918ca28"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.ColumnsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33546) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6b08af5c2b2d8809"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33532) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d966df174614d9d"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.CurrLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33536) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0cb6f6ad0d52a7f8"></a>[`public Inno.Native.ImGui.ImGuiOldColumns* Inno.Native.ImGui.ImGuiWindowTempData.CurrentColumns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b98a72054a0f3c1"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.CurrentTableIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-30fc3a60a5b96a31"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33528) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c616b91559e25b13"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33522) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f5cbf8b73296f614"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CursorPosPrevLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33524) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e807b50448744c93"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33526) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7ab958cd463b434"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPosLossyness`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83733f98367ed604"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33596) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6f0ce38c8e05fae2"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33594) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ecf60549addd7f4"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.GroupOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33548) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a2bd34fd7632b488"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.IdealMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33530) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b9aac6ab787cc53f"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.Indent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33544) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e58408ab9fa30c6f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.IsSameLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33540) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9456e9d524c61bcf"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.IsSetPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33542) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aaa2dc6262d8cf5b"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.ItemWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33598) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9045e7079f19859b"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33600) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c43bf5bb54a8886"></a>[`public Inno.Native.ImGui.ImVectorFloat Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33604) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7eecadbbaa264746"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.LayoutType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-85ba52893bbabca7"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.MenuBarAppending`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33564) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f9dd89c226398d99"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.MenuBarOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33566) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-674994db3c661125"></a>[`public Inno.Native.ImGui.ImGuiMenuColumns Inno.Native.ImGui.ImGuiWindowTempData.MenuColumns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33568) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb7353119d136582"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.ModalDimBgColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33588) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e507bf3d9fd3de52"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavHideHighlightOneFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a2b96fe58de0a53f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavIsScrollPushableX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33558) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2275e05700fece88"></a>[`public Inno.Native.ImGui.ImGuiNavLayer Inno.Native.ImGui.ImGuiWindowTempData.NavLayerCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7e580b66042d6528"></a>[`public short Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33554) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bacec926eee71805"></a>[`public short Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMaskNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d663f7240eb0892"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavWindowHasScrollY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0a46eb3ac9a03ec"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.ParentLayoutType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-be755de938a5b21c"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiWindowTempData.PrevLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33534) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7de7d1c666a04d6"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.PrevLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33538) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d006153a1af7d1f"></a>[`public Inno.Native.ImGui.ImGuiStorage* Inno.Native.ImGui.ImGuiWindowTempData.StateStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33578) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-170ef4966aa9a5da"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33602) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5140323d1c72a903"></a>[`public Inno.Native.ImGui.ImVectorFloat Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPosStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33606) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f46a1fc3a1017130"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.TreeDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33570) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f75784c86340f72e"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.TreeHasStackDataDepthMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33572) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e68d3a606819dd19"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33574) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47f6796ae5514b35"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.WindowItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33590) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75a910388662f108"></a>[`public int Inno.Native.ImGui.ImPoolImGuiMultiSelectState.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33630) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dd935003e8c54304"></a>[`public Inno.Native.ImGui.ImVectorImGuiMultiSelectState Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33624) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eab3c4c35185559a"></a>[`public int Inno.Native.ImGui.ImPoolImGuiMultiSelectState.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33628) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da339cd85e65788e"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33626) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e6e3791d951fc0e"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTabBar.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33654) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e3e9437461d3bb2"></a>[`public Inno.Native.ImGui.ImVectorImGuiTabBar Inno.Native.ImGui.ImPoolImGuiTabBar.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33648) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-daada1943dfa133f"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTabBar.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33652) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd91269887376225"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiTabBar.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33650) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4fbc71ebe517f26e"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTable.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33678) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad9b88272f488a20"></a>[`public Inno.Native.ImGui.ImVectorImGuiTable Inno.Native.ImGui.ImPoolImGuiTable.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33672) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-395debc92e31c640"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTable.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-78149f1481ab6ae2"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiTable.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L33674) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
