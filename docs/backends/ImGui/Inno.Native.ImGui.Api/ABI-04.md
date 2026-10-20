# Inno.Native.ImGui ABI · 04

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingNoSplit d3452a0bed8daa9472f17b5ea1d4b08312d06e1a6809b9b78de31e30afdec6d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingTransparentPayload b8e65859d4c445a0f01ee18cf0ba46165cf8a90aa88eb8a01ee0468204022fec -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingWithShift 0c6a17cc201f4ad596508631b6451aaa05410f02657959a4fff7812a8e663a76 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleFonts 87af1311c79400e50939f1b1eb23dcfb9953130d9a62170c6bace7478b9f0745 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleViewports 6ec9adb4fed088f648bc47a0c009c40699ae1c30c940199e5534232023e73a7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDragClickToInputText d62ad9139d471b7a4592235e332b58a384618f2ef4321a7c7c6922058e6fd8db -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecovery b87c5bb5e5aa0c0870673ac68f05b5c3a29f01cdc3a9fba7fdda9d39e2ff24f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableAssert 389d400f94abb485a6f3f7add7d7d1cc8d57bc6876246fb27ff65c8b648ef0f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableDebugLog d34bba52841cc03a654120ecd8346d4719228ef70090ca641a9fa5ab69bb759d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableTooltip 4858825f8574463cd2b13c1df507daf71f8d5bacac8a740923b6d8860648ca54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigFlags 660c7ec40b833131cb86e071f9754f4eedaec923d57fc0fd08ea63313b2d56a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTextCursorBlink 8a772f33730dd4331e3aa243937fbb6deef27d3d56ff9c70433b12a025623218 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTextEnterKeepActive 331739b419c3a1fa06b821254d748397aa6719dc6c1bae02855da55c49cc8dbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTrickleEventQueue c3bd1ab562829ef3ba20991673ad9a3e523e6ce89149c8f3460fbc0464a566b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigMacOsxBehaviors b3e5b35a025a26e00c941ed67992a85bfad18c7070273aa2775e9f1e549ceab1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigMemoryCompactTimer f19a9204c3628db3ceeaec705838fec4408294d09107c311d8667ee7d394c668 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCaptureKeyboard 38a6310d94f4f911233b2e4b199c7714a009be9a0a0d473ae6997ae4e3943e01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAlways 2e7dcb8113fbc940016375e7ded8cf8c86c16f36b5d8ecf947c46818d69c67d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAuto b1e6740398cc68b8d96af1536dbc88afd8162525b1d3c94ddb9e4196eb2538f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusItem e1cccf25c3561376a83a56d19835d183b0d847d810a987c8449f22307c7fe60b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusWindow 6df335933a7da1933723117bdd6a6bbe82d3f71f37fce154daa793ea0c5f1cfa -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavMoveSetMousePos 1cfd2720a7d3764eab0f0e47d03c1b381c50b1dab2902dfe21287831f231ee2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavSwapGamepadButtons 7307392dc1d31b8341e8bf238ba65ba11a524ab60828125b63a02307d15ca572 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigScrollbarScrollByPage 717978ea4b68c6f5a94f721e183e3656f3a0d42483e80bc80702ab4c393b2d23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoAutoMerge ac5b5c3c7eaf0c91d301051b55f34a0c92802ea606bf49ff82ede5e3c7c41be6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDecoration 4a00a80eb4e73b6b7bd39e384b6cc8620c569aadb305f76722d2f6dfa56a2031 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDefaultParent 3c94e412619ab4cbe0716274449ac3aa7cb96790e46804665e1ea2c3bf91c1cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoTaskBarIcon c61fe1b6292383be04c97a62f476744dd2bbf0e0af337faa93a4023c6aa4caf2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus c966c16f3164a4020a4418b0776a9030fda14a4a8347453bf5049ed61cb7196f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC a32fada124b2dc6e7c8a890223473c38610036b2d88963a4dc4d44a8831caf49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly 04fb34211b8bdee34d421785f5b8898868b6dc97f01e2bfa4659bf3008e23518 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsResizeFromEdges dcae45f62d82c6c47c7c10c3a5c15dc8adb0473f672e0739ee08c40178d1a2b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Ctx ab520b8524aa7a20578a6dbf5e44298d19ca61bc86f0d9d9d3ade52633aa912b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DeltaTime 9a375d34756db231c46a45304c7bd74bc7c8339c8886913d39ed22ae02d36dbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DisplayFramebufferScale 3270b3afd4303c666b25e323d90d1e4933554e9eea10522c20f0a1e8459079e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DisplaySize 8bd8c5d8b5f8ce83b22055bf582a3adfc146e3c5a521711359946fb7f9daba95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.FontAllowUserScaling fcdcf066cbefba10e1fd7a1a49a4a608293db0f44461b135b3880a7219bdc544 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.FontDefault f8b6ebb95486c722e738ecad22d147631679a5bb6dabf20756463943c2c4b1b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Fonts 8dd60013fb13a01cf0deed8019da875b74dcf67b5af0f95f476488286abd4949 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Framerate d3a56bea7d3aef91a1d093eae96cd6dfcf0be01fd21521536661fc2a656edc47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.IniFilename a56784953ff3d035349e593776b94bf5528f84436f279848806cc5094b45ec6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.IniSavingRate cbb50304148425fed02f24831ebf6f5ba8d20431983edc7ef351568ec0c8feab -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.InputQueueCharacters 32413512a2bb6fa50d058b682fdfc435d0b688e4300190589c6b16d8137bb1af -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.InputQueueSurrogate 5bda403c0aee2ad8020158ec9a82d8d2f1788066730d248e9923f474d7f5b865 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyAlt de47e9cf073109924eeeb0ea47558d3669c582de6bbf6e500edce735fd1e6c25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyCtrl 5ac0267b623914eaa0c12576d363fd441a1235eb1ab4fe647eb169037f313a81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyMods c09cbe3668ec2aafb4d647f85a1c2672391ee1fb5f1de0d0e64d645525041072 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyRepeatDelay 618a5ed35533c0b8176dca7affffab2b240ba957344802be6bc134a767e30442 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyRepeatRate 9a52829a04971bd841d7bb7e1ad1194232bb5b671c87077eebfeb8351ab3115b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyShift 3bdb69a679a6759fcb6cb47a38a100709058398f0aecab8947b8505b557c3ec6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeySuper 4153148f0765db8a852f5e4e45d1a08db6da37fbc0a73dfa3621cd53c213e9c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData 1759bfbe6ae6128e77124c557d1626409f21431c3982ed8b7ffe1413eabfcdfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.LogFilename 8ba09634eafcf2deb1c6997863ca7174471edf8c08fc49cad4c7d47048a2bd34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsActiveWindows 98259dbb5ef21b4e0d3abcff7e12237c99810cd0dedaf57de90fbc743ec0ffac -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderIndices 526880adc3807a437b30a7550f7a744dd894bc52f606b0ac16d875e8cb2ebb45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderVertices efb03484335f36728fb61de595409712612924364a550fac8f515b625b5cfd2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderWindows 75fa5dfa58263002dd866027e470dfbda6540c08627091d97937d99c6f16dda7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked 97e1e4aacb45521c23856b5fb17d3a551689aa08a29803f364ad05eb5a8dbde9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount dca70904b267f8a62dcaa8f20d32c18a1e671da3cc8d34581a7840c65bdd11c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount 4148cfc265f7bbfcb97bb923329a67ca73cff1f9dfc4e8b5c8df1cd0f266d921 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos d58bd407388f02e63ffbb3dadc361313059ba31e914ae8ed8dc122570e6778ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime c2f68e360504da8dc6f28816480706837517c9d9996c5a445392114ae607086f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseCtrlLeftAsRightClick b403c30b5980c07200c0d8237432599a5b6895bda889f6f307bd2e7a132ea954 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDelta f535aacb79bfc4a1ab8ee7add7b58bd090f0b0b7270d3dda9ee886831a73b377 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClickMaxDist 7e141a34ef8120a2933608b629bf2d182fadfe0c63148dd7e26844bfa44a73f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClickTime 671f9f3729c60e3cf05377dbf8015eb3b1b036ba9792db58173edc1c5ca7d2aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked 41fca0a33843204f59b99a2d54982a671a648fb243f3c8cd3b7b9d254a501d7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown b8bb2d250d6d983e97ff8d10eb7eea932c8f8193b7f570d0060e003f63613359 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration 53881db69a0d593bc518727dd3af877643c97bab3e68d47d62533c4b5fcf28e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev ebb2988db63dc4b206648245130f81f78142fd2829b91a84913f1a1384286877 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned 5b01c39488f6605d50467a49f35eb76d566762b15fa5ba8f9cc7a69ba508bb2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose 83a23581130ee1cd7f962b55070ccfed697f7c5645761059544938d50fd3fc85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs 441ed6d6483bc2207d480cf8d8b8d129f94ed957d35951e9113f4407e1e85d26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr d79e7506e2e6dd756a7051781ea7270779f3d4848f7a4e17774e1778641a8f5e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragThreshold 932078cec167f7db26240c5f31028f40e8eac3b1a6082247e952072020202c68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDrawCursor 4417009cd3a4afcd12708ce455e30787cb5d3f3dc727ae7b22221534bfd890e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseHoveredViewport c87ebf9000c195e934d2c93881cf97ac5b0d02e1fad304a1356ad1db3cb27757 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MousePos 3a134d1ae836a9310bc75834198be31a420d54758d68f936aa2b7feeb62c6b13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MousePosPrev c25b6586f96c62d0d8ff0f8b4791f27219adb923e2b040563d701dea3a37f7de -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased 6cb99b1ac9cdcec98b325bf7c79540b380026444d0321197a23dd2b9c4121459 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime 61357c6343fee1a5442fbfc1cfca43cea6f3e7eb0878fc38a6d1016bdfb2987a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseSource 5a7be2fec55e63603abe722b4e6f121211516e9ef044e971a53006678539c8e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheel a0fdc5260971b42e8d477583ad7b6fbc82c41ae980e285b36857fb5af15e8435 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheelH c034607a18778de9720337e671c33f75c8a26e9607ae40ee0eaa6181f2cf30a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheelRequestAxisSwap 30c2ca7c4e80fcbd7fdfab4061f7029c27598b06cc4af7660343bbadc8190603 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.NavActive 57ffed426d0253acd224ceeb4c65a98d6d014ed4180bb8de092d94c4815f2db4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.NavVisible 824a4ed991e32faa8c7596f690da39f8c42d5d67dc025b3662f66100a8db5e17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.PenPressure 052c118310747cc773eb3a87b483683cf507de06054d9925ac8f0ed131a8a3c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.UserData 9048cd1e3679b6d80733a5dd2aef6c742d28be164c8491c06b87a1cd24793d67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureKeyboard c010c50860e709190545f7237f8a7921d2c44d869cecf445d6ddf85e34b0f5e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureMouse d745ae6832dd98f731cb7d5889e988ef3e53151e4d96daf2c2c5e66f51c9ba07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureMouseUnlessPopupClose b393e8a827b1c693eccb27b0bd9c326aaa33eb2a5738da2b76cb54342485567c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantSaveIniSettings 24c344ba28b2295790d8d104cb0a9a7117b79f7e0283cf5786ce83e7266ca0f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantSetMousePos 32a4706d66f13c9881ac65526c73ce287eb63e03a694a88ad619b36a04c5d0ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantTextInput 8e951a6539658219ebd253753074e2ca3e08a0348e1abe82bdce34ccf2ed71fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.AddedByTestEngine 036fd63f37ed9502808d1e39bff9e5f1682680df43c33e5fd25c1771a644cc77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.AnonymousField3 ffd04f99a4935df6ecc941fc09ca69a15c5cb6b706243dc65cc1f1cafc5f3e4e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.EventId 937485f180b87fabc6711d09949144e4e8d58908e17d5b9de0787789979750a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.Source 748690d893db36a7c2bc4b51ec218756dcd7a4a3f1fca55f3cec037e862bbcd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.Type a9b587c050f3f68ca961f5ba8b44afdd51a40eda5e5896d1d2dc33c1021f5992 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.AppFocused 79c017029f98007434e405223742c9ef8a432d27cfe9e20017c12fd150225d16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.Key 4480290435393449a1bfca29962e68bae08839cf47d2bd42b0edea216624e9e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseButton 6bca9384546f25d0599ca091ce82ea5ebf3d223dc2a8fd2562ac3f7b266715f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MousePos c9a28d791952238e172ae10a87cf08a835969778b89483c984d1b108f367b484 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseViewport dbdfcfbf17f6f0c8e683b1e077e15265b8040f54be67c4ae784eed996490ff65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseWheel 6040549096df40878e9143569a1279ceba42954b0df300c1b9c744ffbfff5d76 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.Text 1c0769972334fa367336600a6ea56c9b792bb09f6ef70a2d2312cd3133836e11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAppFocused.Focused 6b4ecb380f49d28a9f5802bd9466675c9902e24cfd76b9b2d970443056bf2d0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.AnalogValue ae3c5cea90839c92d9202b4402cc2f9c1c9865748eac6f7676f8e988305ff598 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.Down fcccbec40a9fed88bc4cfa06725fd76598037fa45c0dc0af2d35cfc164be0b3b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.Key e2cf8d92d5fc5c0a940ee2ea3d37ec78b55421bc38423c910a26e7d327fc145b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.Button 6a7b26be94a7ff930ed4189078cbb77ea39150d68d9fded2f7037f24466f4039 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.Down 9f466675459d775b4332af35819951e3a0aa1e92f4ede105ab689dd6c5aa974f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.MouseSource 94a440c80296b8973c9a413072c3fecc374c65d07905b978fb4e7e6952e3e96b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.MouseSource cf4d1d79dd36cc91659bcf7b7ee7b5564b8b2ca967fdfa286ed7bc83152ae7d3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.PosX 627d975847c3da37233d3f28f310e18040ca1b6eba7e84e199546b0209c5346a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.PosY a15fd1e000eee1e3b0b7f1c9d0a5c3e3ea13865383434388bf494d3c0344217a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseViewport.HoveredViewportId c6d0844ee88491b772f30c5881fd354a3dccfd4cd2a8e7c3569a0665ac68e8c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.MouseSource 733d4b55fa997408126463884af1c0c4c0ccf9f88824d17c2ee550ebd72339fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelX 13ba434353d88e760aaa14ae11168c6148c6fc9528ed217d2de037356ca5703e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelY 57acac0bfd2ea498e5d563ebddc076c634a0b36d6d3f4430f18b88a42bf3629a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventText.Char 20d91b6b24f6a64711eae3389924c6e8c94a2b4b5f2583167940f95d0233b8eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Count 6d117ac86c73e12a58b874f8a905f932e4e1780fce204a7b77cc1df6f08d9a79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Focus 84eb12eae92ea23d0ba58f98e5d391ad90c64b1e460c9ca8f22d8d1d25aedcfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Key dc6cd0ac8d970c8117576344ef1c8ed5f10a6a5b47115da8da5560673eca2a55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseButton 8418b32d9069431bba8900a9b6d6d845c607d0826f9f154a90bdc03b6fca2c19 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MousePos 81f11d5d68d04e39d34aec7643dd49ba7492e131da5a66d32d3deeaca53281dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseViewport 66e368c22b87efc4eaef86ead2599a4623c2bf0c09cef7e9cf31dcf0a212683b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseWheel 211d54a42343d7f6361075b7f478ea3b698bce86f402867372d46bb642b73bba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.None c090510d9c0661dc0cfb1c96cc30e94006a718a26315a2bffe502971379b6048 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Text ecb8a5d58834e0e23c810a12d54f6f54fc62da39df762cd08ce649d015182311 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.None 1460de4a15c2d7a9c5f81dd2b13627ca3ca9b141a2226fbcb4ec0d8c205aa55f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.Repeat 1f1f37216c8d2cc2e2ab0df9e5e418a121b833c97e1bb1c4d09ef3ee55f83c09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteActive 969d400a39f92ec7bffa5ffe4d5628cdce3e24f29090a7261fcbf363e750f5fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteAlways b79dc0fba74364281ef4505d0e744cf41c1f7ccc98111e8446e803d9508b5c75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteFocused 975e85609926f92a6e06836586fd9aea634324f2b8e792c1cc423fb7e6123cff -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteFromRootWindow d06aeaac7454dfd18dd4755ed8bd6030cf5958a8e6e5a6f36e640d21640b0328 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteGlobal 4b52efb500802a1f0e15ec03049a62c05e34301932c416a305b93483a032cb1a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteOverActive e6ffefeb08807c148da8869d573c8b0a5833d4d0c39c38d7ed8eadbeac609df3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteOverFocused 67784bd3bd142265d3cc713de68dfc4819d29789e7a7289df12de448cd9f927c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteUnlessBgFocused b73f3d5603413d122bb4d94baccdae6ed99592482bc5ab0ada045d894fc5da4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.Tooltip cf948211a0d5cc0410de0e3b658d629582efba2022b07b456bd87069ca055ad4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondActive 06c2aaa40c0ab0181800190a9d117cf78584cb480a0559e82111d9fa027b91f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondDefault 7c373ada6d99d435bdf656c09df679b3e84150463e62fb01cffe24d1c1b7e05d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondHovered d5d51ae360cf8a80ff3c4e307213d3e396ecd3411aa1187edefa9a5f5b7e90c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondMask 6723a1233fcf7259e469ab4d11ef81d8d159b8f994264aae63c01c31c2bf53dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockThisFrame bb2f2eab324dec6610502d47a37b3f4de2c213587e16e6c3b248616cfbe25e24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockUntilRelease 7299963ddee860f58fa4ff3d4e74cd3ad2fbbfec914a15915cd3c4627784f542 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatMask dc9938a0f7a06739fd038831ddb838c152858f83321426d8a21e1b31407d5198 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateDefault 7ad2d4f024bf5ce5602a28c2404c9124a438269be9c9e6ae7aac2b6add17c9c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateMask e82f5b2195ab76728927525f0ac02d7f3e8cb56b39168c01068470b71c4c4e8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavMove 5f73dcbc2072165e36a6fb541199b88e72683d4e430f0827f91efb4abe9cb523 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavTweak 57f540508d185fb347c2fdc2afa5f1a55beaa92c50688f4aa58526645bf6e0bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChange 2d1b2a2ecfc53b58a655335dcb7ae2d25426d11701ddab7eec0a86050990c4cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChangeFromNone 3aeef51ae1b991a05741049d5fc900701e9a936ec3ab97e0bdaf29bdd311947d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilMask e42acab981a012324358350708513a32ed27893c94a42e041898eddc6d1c7ce3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilOtherKeyPress 12484234d827946793e8a48e93a3ae26d92359753a5e1600ce56bdfd70d6a522 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilRelease dadfeb7d76b1a41872f5c84f95de7cd737ae367229313e6f2518b9567aff1689 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteOptionsMask ff28bc14e1345b0fd4dbe3acaa56f288c871e283a2a80df9e912509a41d5443b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteTypeMask 972e9e134b51248be8cb72d064ff948a1a21d2c4ffe8b2f9e0b3463cc4737990 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsKeyPressed fa9ebfdc9f8532cb784009b354539053682998f6e843fe39832c704e0f8c1ab8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsMouseClicked e2021b8b876373e1400ce3f730b7b607464c5c456862a8a3b361d9cd3f084e95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetItemKeyOwner 0a9c45af7e1269dd0ba757d316264b5787d8c93346c19661ad27a64c979d647c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetKeyOwner dc56031ee6f8d91881e2250150b5f4675e5f210b27f72d3fa187d03eb70f763b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetNextItemShortcut ba3d552c96b50b2b0ac093442bbf23b32cd1de866db0743a7504305ee1ec4e14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByShortcut caf50f2abd23eed8c914a1384ec11b05f8ec70caffc5018c6fa233c0f0504e77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Count 71ca3af0c02cc19394f73d77a4843d0d7012e32873b30e4e782426a2a97f0ab1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Gamepad 29c9fa0a2a5d206da2b652766fa6c1010018433182e2e115b42a066bc1f37b8a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Keyboard 344310fcb47497c4a9d26085cd68b6b694402a4816ad5ab4f60e4a3b8f99eea5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Mouse 4be8bbc3c2b0fa7ccf7ed03e2de7d9630ee490c8cf0b638e4228117cee044e56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.None 2437bf87fe39c936cffcafa5415e0e2c60b6e72aa67aa1cd3abaa39ea93c3fe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Buf cab94a09689dba8ae2e5dc871506001d7690aaf99eccced8c09cd0a3ea642a54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufDirty 7dd4583ed7b94a14f71690f33bb92a1289206286fcab25ac9644f8927022df1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufSize b67ab7fbd5eec114e8159c8f6fd25f337423e84cf8140ce0810d5edce896a2c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufTextLen 8cedb7f2fcd337d0a5d73dc72885982b80a76d83b5cb29d12836c4c4ef0a36cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Ctx fdd1267475d363061325506dc415bc6ed0100d20ccaf51a12ca1ea73f39e27bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.CursorPos 40a9ffad725c5ee63769c17b70be14fe59dd7e9704402c5ab2df16394202e3c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventActivated 7d84fb39727b98c0db0012bfe89b4aac13d944ad0b41638be0d7be3724af8563 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventChar bbcd5b29e595be67ab0a5733eb06f5de74943a597c7c8282977adb3f83d58fde -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventFlag 01cc18b097ab5acf18d9f0a6964a15bbf552d3919ff681d00b31b389c411c84b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventKey d78970fd8619ac743b25719f52a9a0ac43acb91fab68c766801d07c94caef6ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Flags bbe2cfaf0fd99df5cea8f697fe018c10ecc430f4937d91dd2c6bbb0ac5281331 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Id afcc03e16e1b4fa9d621337c940522dcc6b7f8fee88e67a675f533be30148b65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionEnd 594c28c1ac35eb08be80389e363aa1a64a454bf45f50bba243c7e306af08dcab -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionStart 9e273ab99b2b5770ff20929e8b1bbb348309664ae13a3705a9db4acd465bf7ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.UserData 293316177cb871ff028fcfbba4ea6a788de82a1776613e8454e020d15f7494f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ID d235deafb51c231ab0f8c5399dcff4567ddb6dd5ecd9305cf4d11ee0be2ba146 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.TextA 5890115faeb16db330ee16c77cf3bcf5e84f464a9f29810e0c88251b7a0bead6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AllowTabInput 9fef9e0aded44b517435b278cca59f62c8c3b1296495232c7c3ac4f043c48b09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AlwaysOverwrite cb903cc8fe2d9c293c2aac5027a8bec05d76536d88231d4301deb2987b67692f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AutoSelectAll 889e6440b182e30955d60ec2d5360caca65309da6816900403bcaafaf3fc4979 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackAlways 2d92c8b8336390ea3c7b768f16b8d7d67b3604286eb872e02b5765ca7dfb8138 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCharFilter dbf47a7ae6ceab6d19e12ad4278d0dcec3c8e8162bd1b85d8e728a24b69ef5e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCompletion 86cc14614cafea01054ec6f647bba74ca27379d999a54232b0ed82d63354b20a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackEdit 1e62e2fb8d5825bcadc07597b3c67cbf400a97fcf64edbb274f10917d8dccd8a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackHistory 749cd2186cd98e41ebe2b6407dae1cde158322503629c19be92b04f82d36dd2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackResize d0b3a3bf04e4832f61f170eab8e79b5b42a839bc907d62719877d9ef8456a5ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsDecimal 798b6963a5f1e28cc57b425ca48d74c0449523736200d7b811b22c2ea7f6a974 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsHexadecimal ac946a86a41128358b8631d909d0d6091423ef170da45c8edaf579e2418cb648 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsNoBlank b4b2180fb0f246f2c5eefc223c0cfd28cf965fa121f015907f418e28e968869e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsScientific e2207a27131afec39f2c870c18967c838d5288f93f968c3f2ce4e4e263fea2b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsUppercase 031b05334c4b7a9f31b106d28b088f23a1d6cd5c62b6c8b5a98f4f96e759c02e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CtrlEnterForNewLine cf3697796c20a3c68627a4cba19f2d8b9cf7139031d0bdc64b93f7f8b42470ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.DisplayEmptyRefVal 589eb61751bba330bb440f62a6b3833476f773391f9cc7b7401fb42eba80f023 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ElideLeft 079304c36132d7c6b3254df83d20f87bd597858451e600b7820c44f8f14b6b46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.EnterReturnsTrue 8acebc393c89e1eef9717e3da09a0bcb59acd810d97c787e0d253bb56f83aba1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.EscapeClearsAll 9a26935275c21120215a7b8bff5d168f84f882a70f60e3663a3939c0d59f0e57 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.NoHorizontalScroll bfe66162a036f5324dae21a16bb71b4cb512eea977785c55ece592e081b66cab -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.NoUndoRedo 9b6d2320e7daf5477de943a2e520c0b568a42082e6c5b99c8c73fa8d11ec7711 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.None 618cbe1bce830e5d3a3179b9a0b1a519aa796c856e6a7312c9fc6ca5c89f5309 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ParseEmptyRefVal dcbd061432f9895791661d0e1e0129af4c4e2dad2b7555c6e8e1d10a927623a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.Password dffd546fae3cbcda955dcc5320293f6c33126896f1643e22616acadb6a3a778a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ReadOnly 449365763f014160473986958bbb52cab88a70b43b70e7626d981c6ad131e0a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.WordWrap 94770230d2f3cb407326760309d8661d1b1493721a875450acfac4dd22c0d75d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.LocalizeDecimalPoint 00ae5b44da4de8fbfb671c49f5b7b6831714783fd8375aa123ae6f11d4b2faa0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.MergedItem 0200a627b273cc00b8f3ac587bb298da95c6d5bed349828a0a23e5a7bf53868c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.Multiline eda54ac23da738c14f0ce7578e71e61b19d95367d80772c7c4e87bf0be215f5b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.BufCapacity 1b11d96dc4665c3f5b4f919e9caf7e7a25446eedc6af4d72c6dbfdd351a6ff87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CallbackTextBackup e75c6a9820b346808c7b813427c0dda2b7689bfb5d110736d6c46b579f1d7c36 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Ctx 1b65833da403f0c4a4100e0bdf595ce946f72b727f9c2f5aac62851fb71ccd4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorAnim 0d3322710dff28330eb6420b651887f8443656cae1865d54a91e9f49eab695c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorCenterY 88cddca66251bb5514a75e9ebfd913248500b61dd234d24fc1bab72411e6e85a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorFollow 7fe801325961be8ed5617e1ad283ba4c5a8a608554a2e7403cc6c0b7d7894aba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Edited 60be2cab6987ade82b0e9a335e44c38e528bceb818f4c395b0f99135dc35d722 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Flags f0175c1b9be7898e2fe2c1cb6c33ca2c00677ac226ff08ddcfa803209c65c056 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ID 0e7527150d279777c93d18bdadcdfeabfd5759880ac96566ada272f72f8895ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.LastMoveDirectionLr 7bdae21fa8edcfde81078432dbf1f1fa12798711428bf66c8e58dc05e3197012 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-3449dc3107a16581"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingNoSplit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27159) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dcd381ba6e1761cf"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingTransparentPayload`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27167) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b64ccd01b25e54c4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingWithShift`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27163) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e09180a93539d7a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleFonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27179) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5663cd5260bfa304"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleViewports`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27181) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-087ff95be3c765bb"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDragClickToInputText`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27193) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eae48247ef845f3e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecovery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27215) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03cca90436af44a4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableAssert`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27217) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e960c39ef259843b"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableDebugLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cee4a06ccf209803"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableTooltip`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27221) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-184f99e237853d35"></a>[`public int Inno.Native.ImGui.ImGuiIO.ConfigFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27121) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42f4018231cfa095"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTextCursorBlink`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27189) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-567866c2b785d7b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTextEnterKeepActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27191) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-10d6f7d19f68bb2a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTrickleEventQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27187) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bee390a7d9664519"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigMacOsxBehaviors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27185) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-56b6fbb530c90d46"></a>[`public float Inno.Native.ImGui.ImGuiIO.ConfigMemoryCompactTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27203) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a751e2b60bf27059"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCaptureKeyboard`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27149) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-310d3afaa8461378"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAlways`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27157) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ab133ca362e703fa"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAuto`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27155) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3591b2d4c924b2f2"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27151) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-778715af2466ad31"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27153) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-819edf88ba409835"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavMoveSetMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27147) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cb5cc9395f33d651"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavSwapGamepadButtons`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27145) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a440538199988f1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigScrollbarScrollByPage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27201) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9a771aa0f35cf7fb"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoAutoMerge`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27169) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0dc972a8fe2ad04"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDecoration`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27173) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1482833b9dbcd8da"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDefaultParent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27175) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dd6bf771b639f67f"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoTaskBarIcon`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27171) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-190e57848928db1c"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27177) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2aa82f26b60e6963"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27199) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69477f2b6692aa5f"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27197) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae18ee57769f3d3d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsResizeFromEdges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27195) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2156246a1f875b93"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImGuiIO.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27273) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6d15625fd707e209"></a>[`public float Inno.Native.ImGui.ImGuiIO.DeltaTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27129) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d38f83350f233b1"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiIO.DisplayFramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27127) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cde9bc29d29a5459"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiIO.DisplaySize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27125) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-41d4bd1035613d39"></a>[`public byte Inno.Native.ImGui.ImGuiIO.FontAllowUserScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27143) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01fe95432c4a55ed"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImGuiIO.FontDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27141) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-85490f60262b5a9a"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImGuiIO.Fonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27139) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08db12e804a506db"></a>[`public float Inno.Native.ImGui.ImGuiIO.Framerate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27261) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ac62ed66d28fd67"></a>[`public sbyte* Inno.Native.ImGui.ImGuiIO.IniFilename`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27133) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e6867590e5013ad4"></a>[`public float Inno.Native.ImGui.ImGuiIO.IniSavingRate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0060caf8432fd452"></a>[`public Inno.Native.ImGui.ImVectorImWchar Inno.Native.ImGui.ImGuiIO.InputQueueCharacters`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27343) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-68fa36a4f1a60c03"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.InputQueueSurrogate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbd95554dc06eb93"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyAlt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27291) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7a52490065216b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyCtrl`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27287) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f7e79edc06a06d3a"></a>[`public int Inno.Native.ImGui.ImGuiIO.KeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27295) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0a4f8ddd849027c"></a>[`public float Inno.Native.ImGui.ImGuiIO.KeyRepeatDelay`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27211) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-002f1136816a1f6c"></a>[`public float Inno.Native.ImGui.ImGuiIO.KeyRepeatRate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27213) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43c30de04dba7c4a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyShift`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27289) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e3a3071e63ee384"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeySuper`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27293) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1953965a6fab991c"></a>[`public Inno.Native.ImGui.NativeArray_3d337b926fc04643 Inno.Native.ImGui.ImGuiIO.KeysData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9b83e446834c067"></a>[`public sbyte* Inno.Native.ImGui.ImGuiIO.LogFilename`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27135) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7e2eae2d6ffa0f4"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsActiveWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27269) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f1864c0eb06bfa3c"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderIndices`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27265) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67e9188ba3022d1a"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderVertices`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27263) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05aab965896541c6"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27267) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcca1024ab096b46"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseClicked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27307) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cffc28084b6e2f6a"></a>[`public Inno.Native.ImGui.NativeArray_76771c8c9ab15473 Inno.Native.ImGui.ImGuiIO.MouseClickedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27311) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d82ec5495c3a0f51"></a>[`public Inno.Native.ImGui.NativeArray_76771c8c9ab15473 Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27313) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7c8bcd2fa69a03ee"></a>[`public Inno.Native.ImGui.NativeArray_8da6be2ffdb0de05 Inno.Native.ImGui.ImGuiIO.MouseClickedPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27303) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc0a2809a447b2e4"></a>[`public Inno.Native.ImGui.NativeArray_ff485db19a16ca26 Inno.Native.ImGui.ImGuiIO.MouseClickedTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27305) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05018607e1fcf145"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseCtrlLeftAsRightClick`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-566b1ca5c686c6ed"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiIO.MouseDelta`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27271) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92e5523943017cbc"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDoubleClickMaxDist`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27207) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-29ca33ecc0716ee0"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDoubleClickTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27205) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17f69e2075a51292"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27309) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-45dd8b27c409dc02"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseDown`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27277) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a12abcedcb6dd4c5"></a>[`public Inno.Native.ImGui.NativeArray_35aebb639e6eb330 Inno.Native.ImGui.ImGuiIO.MouseDownDuration`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af862d37a7a36d49"></a>[`public Inno.Native.ImGui.NativeArray_35aebb639e6eb330 Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-679d144680fa7911"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseDownOwned`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27319) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b304f08a39446b29"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27321) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32626a6ee52ea866"></a>[`public Inno.Native.ImGui.NativeArray_8da6be2ffdb0de05 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27331) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92aa45c61859ab86"></a>[`public Inno.Native.ImGui.NativeArray_35aebb639e6eb330 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27333) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d17add21bf837763"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragThreshold`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27209) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a26b8229d50128e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDrawCursor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27183) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ca28a9053713af3"></a>[`public uint Inno.Native.ImGui.ImGuiIO.MouseHoveredViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27285) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8fb045377c429e00"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiIO.MousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27275) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37236031b9707a24"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiIO.MousePosPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27301) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f4060353f2a6acfd"></a>[`public Inno.Native.ImGui.NativeArray_5dde4d9494a49aa0 Inno.Native.ImGui.ImGuiIO.MouseReleased`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27315) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a7baaac218ea7fd"></a>[`public Inno.Native.ImGui.NativeArray_ff485db19a16ca26 Inno.Native.ImGui.ImGuiIO.MouseReleasedTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27317) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-062ec2b8e7fdb03f"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiIO.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27283) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6958a8642556a3b"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseWheel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27279) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8e168819a5bf1eb"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseWheelH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27281) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b77a71ab15c37ee"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseWheelRequestAxisSwap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-37b212a3306586c5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.NavActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27257) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f6a0c1fcf41ca5b5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.NavVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27259) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a41e72d3d0c8f858"></a>[`public float Inno.Native.ImGui.ImGuiIO.PenPressure`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27335) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f02740ab587353c5"></a>[`public void* Inno.Native.ImGui.ImGuiIO.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27137) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d50426b4c2ceab5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureKeyboard`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27249) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82187aa0a4f931d1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureMouse`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27247) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc3d1ff57c89c01c"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureMouseUnlessPopupClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27299) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72a20cd55108fdc3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantSaveIniSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27255) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-56b3ca1836b1d0b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantSetMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27253) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dafedeb6bf8a660d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantTextInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27251) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3d367b63b7286a7"></a>[`public byte Inno.Native.ImGui.ImGuiInputEvent.AddedByTestEngine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26671) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8d73971af9da73eb"></a>[`public Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440 Inno.Native.ImGui.ImGuiInputEvent.AnonymousField3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26669) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0a73a481e3218b3"></a>[`public uint Inno.Native.ImGui.ImGuiInputEvent.EventId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26667) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b0c5ac815dad30c3"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiInputEvent.Source`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26665) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c293f8a09786f8e"></a>[`public Inno.Native.ImGui.ImGuiInputEventType Inno.Native.ImGui.ImGuiInputEvent.Type`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26663) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b49886095d34c937"></a>[`public Inno.Native.ImGui.ImGuiInputEventAppFocused Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.AppFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26701) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-26c1488335452019"></a>[`public Inno.Native.ImGui.ImGuiInputEventKey Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26697) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-472354ded419c181"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseButton Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bbfb59f27b9de038"></a>[`public Inno.Native.ImGui.ImGuiInputEventMousePos Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26689) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb2ae64937fc1c6a"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseViewport Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26695) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-46c53d756e088ad1"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseWheel Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.MouseWheel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6c01d1ff20619ca"></a>[`public Inno.Native.ImGui.ImGuiInputEventText Inno.Native.ImGui.ImGuiInputEventAnonymousUnionC908181b7025c440.Text`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26699) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49c271e9b7abfd47"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventAppFocused.Focused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26719) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-232e57af6c0f63cc"></a>[`public float Inno.Native.ImGui.ImGuiInputEventKey.AnalogValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26741) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aecddddbc63ce495"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventKey.Down`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26739) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4b9c78dc11bb7ab1"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiInputEventKey.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ac4fde28c41f119a"></a>[`public int Inno.Native.ImGui.ImGuiInputEventMouseButton.Button`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26759) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7efc5f1d3a1e6293"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventMouseButton.Down`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26761) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3df9c69dac0ecc5e"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMouseButton.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26763) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-97fb78e518686f83"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMousePos.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26785) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc7b7402c06f5d2f"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMousePos.PosX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26781) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5cd97ea4cefb3422"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMousePos.PosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26783) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-076f449406da3eb2"></a>[`public uint Inno.Native.ImGui.ImGuiInputEventMouseViewport.HoveredViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bd2abab6ee8fd9b"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMouseWheel.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26825) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e990e8ed0ecd954"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ad236e17e1bc8f73"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26823) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6cbac024d599088b"></a>[`public uint Inno.Native.ImGui.ImGuiInputEventText.Char`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-abb1cdcb3da05bee"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Count = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26866) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-b9ced90929a735c2"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Focus = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26865) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-70e39ff855567f56"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Key = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26863) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-839916ef576d2680"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseButton = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26861) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-7c89f0e73d5422dd"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MousePos = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26859) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d501f762e3a35e92"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseViewport = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-3ecf117f190db062"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseWheel = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26860) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-66949ffa0fd8cc4f"></a>[`Inno.Native.ImGui.ImGuiInputEventType.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5db03e42e9f40ca8"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Text = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26864) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-3eab52058f6101f0"></a>[`Inno.Native.ImGui.ImGuiInputFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-e59a42d1316a7a0f"></a>[`Inno.Native.ImGui.ImGuiInputFlags.Repeat = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26875) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-23950f50c3c21f03"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteActive = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26876) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-ce2a86c1de287a27"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteAlways = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26879) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-38e634e2293a1701"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteFocused = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26877) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-fb26673e4f57f05a"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteFromRootWindow = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26883) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-8c54b89b106ca86a"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteGlobal = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26878) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-45f243a2f814b183"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteOverActive = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26881) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-f901ad3f70db8917"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteOverFocused = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26880) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-f7076d32306b2d55"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteUnlessBgFocused = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26882) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-6b3432ce32970977"></a>[`Inno.Native.ImGui.ImGuiInputFlags.Tooltip = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26884) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-146a64e4204f98e7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondActive = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8388608` |
| <a id="api-31661f429ae898a5"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondDefault = 12582912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12582912` |
| <a id="api-64d42f308bc246e2"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondHovered = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26901) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-81e76d27642f5993"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondMask = 12582912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26907) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12582912` |
| <a id="api-f002fc5752e1ae28"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockThisFrame = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26899) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-1ef053660f8f79db"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockUntilRelease = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26900) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-a456a39a8c810ced"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatMask = 255`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26906) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`255` |
| <a id="api-f13b0484f21f645f"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateDefault = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26892) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-84aeae692b0fd2cb"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateMask = 14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26904) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`14` |
| <a id="api-32f64fe59e8d6c08"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavMove = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26893) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-7b52357b75da66e7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavTweak = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26894) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-fa475bb169cf230d"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChange = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26896) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-7ac505b4484f39d7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChangeFromNone = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-7f92220556527bc9"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilMask = 240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`240` |
| <a id="api-4fdc892eb07e6ac5"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilOtherKeyPress = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26898) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-5b7dbc4285889dc4"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilRelease = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26895) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-5356231d0adf2cc4"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteOptionsMask = 245760`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26909) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`245760` |
| <a id="api-1b6a12456a5c6191"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteTypeMask = 15360`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26908) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`15360` |
| <a id="api-20066c00619c5859"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsKeyPressed = 255`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26910) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`255` |
| <a id="api-75855ef117cf4a7b"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsMouseClicked = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26911) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-75952a09dc7ed737"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetItemKeyOwner = 15728640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26915) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`15728640` |
| <a id="api-844917e97c0dac94"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetKeyOwner = 3145728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26914) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3145728` |
| <a id="api-c4d11bc015e6cba0"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetNextItemShortcut = 523519`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`523519` |
| <a id="api-a4fb999e5ad24088"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByShortcut = 261375`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26912) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`261375` |
| <a id="api-c5e065e19ba3cfad"></a>[`Inno.Native.ImGui.ImGuiInputSource.Count = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26927) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-6ba31005ac26d385"></a>[`Inno.Native.ImGui.ImGuiInputSource.Gamepad = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26926) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-cad50b0e8c962e69"></a>[`Inno.Native.ImGui.ImGuiInputSource.Keyboard = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26925) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-676a3d166e3ab44f"></a>[`Inno.Native.ImGui.ImGuiInputSource.Mouse = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26924) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-4fe4be45e79f4328"></a>[`Inno.Native.ImGui.ImGuiInputSource.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26923) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5199ade47dabd476"></a>[`public sbyte* Inno.Native.ImGui.ImGuiInputTextCallbackData.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26962) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b5b065523663484c"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextCallbackData.BufDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81029f8674dc084f"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.BufSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26966) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-520a672dc9c5ee5d"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.BufTextLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26964) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22196866a9fb958d"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImGuiInputTextCallbackData.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2def5190b9257987"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.CursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26968) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2c5701c08995132f"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextCallbackData.EventActivated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26958) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a3ed9e7c16ae135"></a>[`public ushort Inno.Native.ImGui.ImGuiInputTextCallbackData.EventChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26956) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2936eec12840863"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.EventFlag`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26946) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9db71fe5b85ae32"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiInputTextCallbackData.EventKey`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26954) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22cd0c2105a24b9b"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26948) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4c8967b49e23a1b5"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextCallbackData.Id`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26952) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4762f6fb5a44da04"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f1c6ab3169ad9e1"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26970) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-967de5a05135b3ff"></a>[`public void* Inno.Native.ImGui.ImGuiInputTextCallbackData.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26950) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccb9488687586598"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26998) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5adee9adfbfa07d1"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiInputTextDeactivatedState.TextA`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27000) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d0d5279b2962bed"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AllowTabInput = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27021) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-84bae4c474c690ae"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AlwaysOverwrite = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-d74da270609d1dff"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AutoSelectAll = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27028) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-f07a7fb97151529e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackAlways = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27036) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-d95d6db30c2db7b0"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCharFilter = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27037) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-e7fe67572e7822d5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCompletion = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27034) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-986c72ecab998a19"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackEdit = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27039) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8388608` |
| <a id="api-3fe064e026de0ebe"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackHistory = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-0172b7feccb629fd"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackResize = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-09d8961b0c572784"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsDecimal = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27016) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-f2d4f5ed9e86b97e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsHexadecimal = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27017) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-310e5f13a0183724"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsNoBlank = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27020) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-a05dc9a3277ca2a5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsScientific = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27018) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-bbaf35c301fedfc3"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsUppercase = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27019) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-87b737161ab68ce9"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CtrlEnterForNewLine = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27024) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-e3ba3b41cbf08065"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.DisplayEmptyRefVal = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27030) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-423b914fdd45d540"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ElideLeft = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27033) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-71cf6018391460d2"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.EnterReturnsTrue = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27022) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-d47f159fc6d1a485"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.EscapeClearsAll = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-763f6fcc2544b0ca"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.NoHorizontalScroll = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27031) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-3e5d5f93dd2818c9"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.NoUndoRedo = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27032) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-11fac55123ca452a"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27015) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-befa4078f096ae1b"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ParseEmptyRefVal = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27029) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-be94303d04adab1a"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.Password = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27026) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-f5c062ca03485ea5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ReadOnly = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27025) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-a57196c33bf25e2e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.WordWrap = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16777216` |
| <a id="api-f5601ac60a81dbc7"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.LocalizeDecimalPoint = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27050) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`268435456` |
| <a id="api-14150ef76488cb68"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.MergedItem = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27049) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`134217728` |
| <a id="api-3246f263b32f5118"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.Multiline = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27048) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`67108864` |
| <a id="api-6b16806af3af357e"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.BufCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-92acd58866af8f37"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiInputTextState.CallbackTextBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9beb5db65f7497a"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImGuiInputTextState.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27061) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9b43d415bf5155a"></a>[`public float Inno.Native.ImGui.ImGuiInputTextState.CursorAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27087) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76256e4b345e1850"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.CursorCenterY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27091) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b34abc175270c73"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.CursorFollow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27089) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba72441b697548e2"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.Edited`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27095) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa44a53ee7824747"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27065) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ecafe0127d489de2"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27067) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1cca7ec3e67b8f9c"></a>[`public sbyte Inno.Native.ImGui.ImGuiInputTextState.LastMoveDirectionLr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
