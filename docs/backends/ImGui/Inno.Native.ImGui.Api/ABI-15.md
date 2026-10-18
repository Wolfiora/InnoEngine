# Inno.Native.ImGui ABI · ImGuiIO → ImGuiItemStatusFlags

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiIO</code> · 361 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiIO b0bbb002e37a3fb7fd023e514c5a5b69d9edd5efdbe3f618062ee3975cb11fcd -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigFlags da78f2867f6a9512a3a4522c612b983a9bd643782acdb4b52bcec7900fb2ea6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendFlags b9b0619f67fcb5771000cdae2c337e7aea4220a1a28090fc6b266a386e4c97c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DisplaySize 28132605d9e763a810fa7716bd50b861bb45fdbffafdf90ef8c6896febf3a3b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DisplayFramebufferScale 3013e995bebd544c22ac979816bd1f5dddeb4e1468d3dbae7553c4533fc4e4ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.DeltaTime 9a375d34756db231c46a45304c7bd74bc7c8339c8886913d39ed22ae02d36dbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.IniSavingRate cbb50304148425fed02f24831ebf6f5ba8d20431983edc7ef351568ec0c8feab -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.IniFilename 7e68b4aa856a880d2ed6aa50b2d2121b4fe732862e6a6394b441436ffce77017 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.LogFilename 7232f648317a010d9ac16e8035d6fb721cc31166baf30589d3d148bb3efe1d9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.UserData 9048cd1e3679b6d80733a5dd2aef6c742d28be164c8491c06b87a1cd24793d67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Fonts 8dd60013fb13a01cf0deed8019da875b74dcf67b5af0f95f476488286abd4949 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.FontDefault f8b6ebb95486c722e738ecad22d147631679a5bb6dabf20756463943c2c4b1b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.FontAllowUserScaling fcdcf066cbefba10e1fd7a1a49a4a608293db0f44461b135b3880a7219bdc544 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavSwapGamepadButtons 7307392dc1d31b8341e8bf238ba65ba11a524ab60828125b63a02307d15ca572 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavMoveSetMousePos 1cfd2720a7d3764eab0f0e47d03c1b381c50b1dab2902dfe21287831f231ee2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCaptureKeyboard 38a6310d94f4f911233b2e4b199c7714a009be9a0a0d473ae6997ae4e3943e01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusItem e1cccf25c3561376a83a56d19835d183b0d847d810a987c8449f22307c7fe60b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusWindow 6df335933a7da1933723117bdd6a6bbe82d3f71f37fce154daa793ea0c5f1cfa -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAuto b1e6740398cc68b8d96af1536dbc88afd8162525b1d3c94ddb9e4196eb2538f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAlways 2e7dcb8113fbc940016375e7ded8cf8c86c16f36b5d8ecf947c46818d69c67d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingNoSplit d3452a0bed8daa9472f17b5ea1d4b08312d06e1a6809b9b78de31e30afdec6d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingNoDockingOver 735ce5afa0fa7881b7470b77a00e80844d1538184716950231893e9624edf97b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingWithShift 0c6a17cc201f4ad596508631b6451aaa05410f02657959a4fff7812a8e663a76 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingAlwaysTabBar c56d9d9281ddaf5f3e2b4b2b289f513b10dd61333d98f38c34c43d68deaf596a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingTransparentPayload b8e65859d4c445a0f01ee18cf0ba46165cf8a90aa88eb8a01ee0468204022fec -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoAutoMerge ac5b5c3c7eaf0c91d301051b55f34a0c92802ea606bf49ff82ede5e3c7c41be6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoTaskBarIcon c61fe1b6292383be04c97a62f476744dd2bbf0e0af337faa93a4023c6aa4caf2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDecoration 4a00a80eb4e73b6b7bd39e384b6cc8620c569aadb305f76722d2f6dfa56a2031 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDefaultParent 3c94e412619ab4cbe0716274449ac3aa7cb96790e46804665e1ea2c3bf91c1cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus c966c16f3164a4020a4418b0776a9030fda14a4a8347453bf5049ed61cb7196f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleFonts 87af1311c79400e50939f1b1eb23dcfb9953130d9a62170c6bace7478b9f0745 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleViewports 6ec9adb4fed088f648bc47a0c009c40699ae1c30c940199e5534232023e73a7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDrawCursor 4417009cd3a4afcd12708ce455e30787cb5d3f3dc727ae7b22221534bfd890e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigMacOSXBehaviors 6f276798fde366ebadc147eb64db5e37fc2e56b6a729570669e93c63d9186b9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTrickleEventQueue c3bd1ab562829ef3ba20991673ad9a3e523e6ce89149c8f3460fbc0464a566b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTextCursorBlink 8a772f33730dd4331e3aa243937fbb6deef27d3d56ff9c70433b12a025623218 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigInputTextEnterKeepActive 331739b419c3a1fa06b821254d748397aa6719dc6c1bae02855da55c49cc8dbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDragClickToInputText d62ad9139d471b7a4592235e332b58a384618f2ef4321a7c7c6922058e6fd8db -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsResizeFromEdges dcae45f62d82c6c47c7c10c3a5c15dc8adb0473f672e0739ee08c40178d1a2b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly 04fb34211b8bdee34d421785f5b8898868b6dc97f01e2bfa4659bf3008e23518 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC a32fada124b2dc6e7c8a890223473c38610036b2d88963a4dc4d44a8831caf49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigScrollbarScrollByPage 717978ea4b68c6f5a94f721e183e3656f3a0d42483e80bc80702ab4c393b2d23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigMemoryCompactTimer f19a9204c3628db3ceeaec705838fec4408294d09107c311d8667ee7d394c668 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClickTime 671f9f3729c60e3cf05377dbf8015eb3b1b036ba9792db58173edc1c5ca7d2aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClickMaxDist 7e141a34ef8120a2933608b629bf2d182fadfe0c63148dd7e26844bfa44a73f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragThreshold 932078cec167f7db26240c5f31028f40e8eac3b1a6082247e952072020202c68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyRepeatDelay 618a5ed35533c0b8176dca7affffab2b240ba957344802be6bc134a767e30442 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyRepeatRate 9a52829a04971bd841d7bb7e1ad1194232bb5b671c87077eebfeb8351ab3115b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecovery b87c5bb5e5aa0c0870673ac68f05b5c3a29f01cdc3a9fba7fdda9d39e2ff24f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableAssert 389d400f94abb485a6f3f7add7d7d1cc8d57bc6876246fb27ff65c8b648ef0f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableDebugLog d34bba52841cc03a654120ecd8346d4719228ef70090ca641a9fa5ab69bb759d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableTooltip 4858825f8574463cd2b13c1df507daf71f8d5bacac8a740923b6d8860648ca54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIsDebuggerPresent 2337694ab8e7b978488d8e8dff88d6e76fda2e33b5bb0ed95c188c969bc69cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflicts ddcb5b317d5bb0a9f8bb967a725c70e15978de27ae41474c08c0739230ea3a99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker c1442e651719d683faa2111681bf94156cfcd386d66892ae39ffd4921f65d0c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueOnce 3712a68fb7764a58615718c71764dad7e7d7fefc858f7faa3f59cc1805b135e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueLoop 09b2b85de04a02599d3f35a8b6df3d8fdc3902f9fc2819bb20b2aefe132d8aa2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIgnoreFocusLoss 8e6e57e4087148d1bcf9ba137021e5abf14c223072fac6d1777f606cce7028fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIniSettings 2c1aaecb37760932877bc8b8fa700504bc9f4f6ec651a238df408bdcba2bd560 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendPlatformName 6eefdc9093a3bd010118c0fe636974f68f20869e75dbdc22ea1aacd9d82836e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendRendererName d51880c165f517687112cfa82aebd59737b1eb00e5ecc9e490a752be5472f45f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendPlatformUserData 07f21fc53fd75956058e3795c3cb2be24485c9721b837397b6f680fdf9b659b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendRendererUserData d327b25e3eb78ff4ff7fa78ccbd3583799107f4e270bd3a49bc6c172f07cbbfb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendLanguageUserData 9d01ff998636dcdb31974caac57b1a13f0748032f814abc98d79f8a950467b57 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureMouse d745ae6832dd98f731cb7d5889e988ef3e53151e4d96daf2c2c5e66f51c9ba07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureKeyboard c010c50860e709190545f7237f8a7921d2c44d869cecf445d6ddf85e34b0f5e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantTextInput 8e951a6539658219ebd253753074e2ca3e08a0348e1abe82bdce34ccf2ed71fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantSetMousePos 32a4706d66f13c9881ac65526c73ce287eb63e03a694a88ad619b36a04c5d0ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantSaveIniSettings 24c344ba28b2295790d8d104cb0a9a7117b79f7e0283cf5786ce83e7266ca0f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.NavActive 57ffed426d0253acd224ceeb4c65a98d6d014ed4180bb8de092d94c4815f2db4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.NavVisible 824a4ed991e32faa8c7596f690da39f8c42d5d67dc025b3662f66100a8db5e17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Framerate d3a56bea7d3aef91a1d093eae96cd6dfcf0be01fd21521536661fc2a656edc47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderVertices efb03484335f36728fb61de595409712612924364a550fac8f515b625b5cfd2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderIndices 526880adc3807a437b30a7550f7a744dd894bc52f606b0ac16d875e8cb2ebb45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsRenderWindows 75fa5dfa58263002dd866027e470dfbda6540c08627091d97937d99c6f16dda7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MetricsActiveWindows 98259dbb5ef21b4e0d3abcff7e12237c99810cd0dedaf57de90fbc743ec0ffac -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDelta 0cadeb9c97cb4231ddab08e94ccbd9615c6e8b50371c60eacdbd875dce9fef79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.Ctx 03a949d66f559dbfd759e12b92f9ef764c305a94902f24d4674c15c37f77e351 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MousePos 5593fc9620485e008c1ba974b3ae69ab534b8ab5dd0a35d479652f83cbefd169 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown_0 a90bf8a39676e12c880a4fc7cefef0960829e1d5d2e668e59e1d43244cb1ca8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown_1 22bfbf39bb049de1548c5303618ee207d5d571974b5d5dcde570ef8d3a92fb55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown_2 1b7ff4ac2397566659eaaa2a9f3eb04e1da318a61dcf1f82420c93feaea42fbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown_3 c51919602a63d76e62ac3cd47043539e90bfba0437de490357a94c824e8f5e09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDown_4 9ab62cbeffbf8038ce7402b6ef25ece9eedc8b6e94601f0b58b024ee99d8d427 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDown 53b192954ef034f32b2a0bbaa504b4d9fae43bb0c1f1645cf2fa45b9cdc687ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheel a0fdc5260971b42e8d477583ad7b6fbc82c41ae980e285b36857fb5af15e8435 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheelH c034607a18778de9720337e671c33f75c8a26e9607ae40ee0eaa6181f2cf30a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseSource 5a7be2fec55e63603abe722b4e6f121211516e9ef044e971a53006678539c8e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseHoveredViewport c87ebf9000c195e934d2c93881cf97ac5b0d02e1fad304a1356ad1db3cb27757 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyCtrl 5ac0267b623914eaa0c12576d363fd441a1235eb1ab4fe647eb169037f313a81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyShift 3bdb69a679a6759fcb6cb47a38a100709058398f0aecab8947b8505b557c3ec6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyAlt de47e9cf073109924eeeb0ea47558d3669c582de6bbf6e500edce735fd1e6c25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeySuper 4153148f0765db8a852f5e4e45d1a08db6da37fbc0a73dfa3621cd53c213e9c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeyMods c09cbe3668ec2aafb4d647f85a1c2672391ee1fb5f1de0d0e64d645525041072 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_0 c1c0d8c057d7c097761d8be0d8f14b79c225efca96b8fd21c4d898cb24f9d8f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_1 d932a4f1b3fedd3cc4ce024f4aa2a8cd569675d28202542643a5fe33441ee220 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_2 d15782abafe9e72834a5626e218adac3f84da76bbbeddede5ce03fc48f6c997c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_3 f5141c01874a9226a2b9098ebf0a8b334977b98df3b0b4eb847e35eec04b1e93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_4 a7b50daeb75e62cbdeb7e4b6d3121186acc44ee7bb56c11ede6c225229bfd7b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_5 f188a8d06a86e8454e8702dcb8d65026eb0edb161abd4cbee689de9432fa19be -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_6 2e71d43844e1a1b442ce3134e9d9a67dfd9ecca96527fa9616f9929a64217f9d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_7 74c363df692227391f913274cca564c4030f298c00854b1686614d8667c9caa1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_8 85e6b9cc24b742ec37f200213243b15ab9b4f36f5ebdff56d1530fab37b54999 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_9 5d16b3a38073e6756ce28529a900fdad24ae152b8d5df9c42698589ed257a7a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_10 eb3a13031b5ea0768d75d91293a7dc226d63f0fcdc540c8e73551a61940c3295 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_11 ed088c934725fb88553adb4e0af7884204fcfa30b6e83139c70951641b25797a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_12 e9638dc74bcba3d2914d2aace0a47e7596ba62b061a6a0cb9df210daa0196327 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_13 e61582a814ddec057a96f49eb56fed7cd8f1025aa87d2b598390b2612a8e95b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_14 14eebe81d1cb959ae42723e708d114b14cc7ac65ed99eaea4743bdb31f3902a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_15 cdb93c6806470ccd2eef47e9721e6c07fb98634f926a5b4b660c82893c1e5a9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_16 e5d8388ec1545c155dc839ad8ac9bb1f001c000675c334ced43bbb6ba2f931f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_17 04d9764b3908a85b778ae76eb08600fe3cf7e1837e2518747f7eaaae8743bf2c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_18 02c38d347fac9b9cfda19ad382da05c22f39d8e1304432bbc420aa52f5c914a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_19 3781f6e2aa3cc165af363835c2fdea27a01237531e5427df9ddba0ee399fab28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_20 490d45058d953f06d8e85901bd4f7a8187d0cb33496bf7b10178d8160cfe483f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_21 89088e193c4f20ab5bbf10fa26914b7e6429b76312c06485a0af53ec81ae1203 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_22 9d8ccd1f74c10f50641fe08986d33996d7ffc0b537cf6d7153f936f92811fef3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_23 8995f25031efbfd3a97cbf81e334556c7f2322cb654b8a42415924e40bdb220b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_24 4988798f2a47707d99b893fd521613ce770530279d1a261ef4ab72aec5c83e43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_25 7a6c5e9dca94ccbf1f8503f2bfe0656117b7dd537e972684f0a9ec032ad76bad -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_26 bcfe0cb0c8aea2ceaa6c1812ba2dbc903c19cbd82ffa491eeaba44a2ed815f90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_27 fa09d2bc5458e8b835accdd55187e63ea680c5a4776e81facb023e074284a6be -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_28 f9b4d8a478eb3bdff56f975888f03ad2fd2ebc161ddd2f48e639e619d3a6e8d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_29 eca70cd0f02cd85075d35ec418ab15ea0d30c2b613b51b3c9dda595855419c3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_30 16539c246dbe49b7b4207225b6eeceadfc4cf7d396ff525af4a0c1f328446012 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_31 09fbc296b4952cb868f4484575a149abea90fdbe2613f5a94a8958cc4615c8c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_32 99879a6d32ef5eb7171687743de889791800c600c5e29695e02dec5f4ccfc837 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_33 bff3be3d00b6fb6ed458e81ba8e81ae203cb3b2c59d802cf4157d632cfc8189c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_34 11256c282f505ce2384469de6008121dcf25ede2877459881264fe5d7231c650 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_35 c6fb1ca3544116df686f1426d94cde0c3221886d194cc2df5e75e90bcf942bd6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_36 3de7bd9ebf3d850b444d3176ba882cab8f3d22732ceed6653d2266352b945d31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_37 992dbf66900be66b124f0420af7577c22dac002a3122aa7a8a2bb8ba94f2df39 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_38 e69234c937dc033a21e8d02f8ec943632d6b3d747ce9fd4545bf451dd59586bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_39 3376f694516c091279d933cc16f75b1b5281c1eeceb6bf317f19d24692bc6857 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_40 f63b854c8b4cd6f7588d847da2c73bb8d59bda146f74b0cfded0613153adbb31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_41 94fecb229aabe83d23f4db25d64480f5fdcabb46a16bbf8e6e8998c6ecf608b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_42 eca2185723248a98f1118aa96dc6078ab0f9c096826b826575b1225a85e316db -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_43 62449964c2cd2a42a529684427e2367204f51450f8470b5f873510c752ee62f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_44 2ad053e3540303822b43634f84957392ae91c262c64d8d3226d42b4c998d91f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_45 31c8ad8414d36f9f271b94861394d7012b54dddc53ff8466761c52e4f1df83fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_46 32a5ee6595827ec30a0de5c2ce205aa5895bd507193e7a2ff4ed3788b6a191fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_47 d99cdb1610cab82bf628d357b70ca4270f670eceeb40796df94687bc54ab5ab3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_48 fef714b065b3886a344e899821e15b407556cc64b6c4efad18594ac0f69e8883 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_49 bee3b258914c05cff5cf85ff8c8c658785a8cf7c77a8e131a5c25bed9380e38e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_50 6384bcddd83b681ab51e348d6c4082879e56c422e8db36ae53d82a9ab686e093 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_51 544b95b74abd296bec25f95f08765886e052de723f5f314fcb2b0691d97aa0ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_52 944f2f3c3bbc8e80a7d382c17af91084e3a357fd727daa9d8598763c606c495b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_53 03e5e9dcd19a4515b5c293997c3e3d4ca5d9d95864bedb0a5dc8accbe3631b9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_54 c94253ad1125b02dce54c9bf2982efededc1fa46026e500f03a3574b1915292e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_55 ecba788544bff4f71062cf41595d5023cdae41578808009cfe13e13c8dafffbf -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_56 014bbdb7abbf5ec370ab44122426dd6c4dee67691dd6208f01b229feb9f57118 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_57 1fad687e538c21a93269bedd73674ed5691e695fc60def59e790e4cbf72bf0dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_58 6a7375682b5612ff8b678007ae3be6b6e4a7b5484ea0f1d0ba70f61df350883d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_59 5dd276be641fc600e1ede008cbce6a73781f8b8e4b6542ba15005781644452c6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_60 f33b6bd612fb647bb3177bb2d13fbe1243710f8558bd33704ba275a2dbb02562 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_61 822eff2b2d0e53e9810865aae42028e2954e004cc191cd80abfa6dd33c8f37e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_62 534c137e51d9c55eb9867c5619c449f1797791483a83b6288a6599e9b6e124f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_63 0795f179997aec01c61c72ca3136041bb1efaf96796167d52df31691b556ba99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_64 e73c17ceb68c4a1d1111db1c5483e50f5111261db59755a9364f4e3965f8c175 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_65 f66d7bf884dc934e1cac353c53402e94e57b28fa4d3d1024c8083d2fa8c53799 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_66 5cc80201a5c5654d49d2fbe23a04e4470ba062957c14ac58600f745c6552dc10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_67 39db9bcb7acfd0fc84d974c05d89872f6d5d9f9ce0091594081f8059b98f96f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_68 8475e21d6c391c8c6680344208b14bc3858dba56afdf312417dd0c2a070c12a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_69 f5736f711d29537fd9600f5064176fc9871e92874174250665f3cb9746013c6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_70 a33f6c64230e825a488e207854222cdf8dac3995da309cb1b412f51d5558214d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_71 3d0bcca4b9b06ce410103628f28e01e027f0ae944a628f6e5ac8ae88bebe2d9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_72 4594655238ddb587d6b18169637f758a37eb9f26ac4911a83fe90e265d4c7461 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_73 0db3a765e8a8f5fed5bc273553b8fb47e8962173d039f2942b32a7a7d897cf26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_74 088f2f3d92e15c15d7a418026ce3e79ad6d5e38b69df846a869fa9cf20701833 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_75 702df24aa1c5ef7016efcc1e06404773aa64067ac2cc05d6b6e97071a70d72c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_76 4654000a6986ca76999e5caebbb76e245bcea54ec7060b265a460bcc21c2e3ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_77 9db64213579b43550b1d8170156eaf7d9f017e37ce8c355b52b3187907ea9f9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_78 d3c89d4e1c415abc04884e750b0b15ea1fc7daadbd01921eaf53e1c57da73742 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_79 5ece4ace05989ea550c3007baf4faf99fda138c8f7f7521dd66dc6c83e66ebaf -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_80 8fcea125147154164977dfb4b2884fed3b70fe952783ae8f0735edd7f47d845b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_81 ffa8e36a9a58eca470b40afb82c3286e6ba68c8d238d95a5d4e03dc7c274b853 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_82 244b3a3f81c20062d7186fcf6f63d01fa33b265b5433586e37772d2fa7332b72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_83 6e8fa63cd313210fa5c8048e0a8d5ca7edc76837627a4dd6e59c6a247acdd3ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_84 9fb62dfa5840abc7455d46badf2b37bff84c94e2eca3934a7bc06686f3d73ba9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_85 0755c1d5338445fe88feb2e42d012feed2f4150b88a8f8f0c60bcb5fe7a2bd44 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_86 f119845ebcbd7025679c91b22947529c8f1e947ade5dcca41851d82c2c7c89e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_87 4f9dcd10ce48b112b9998a17e98e4fc1e43d7a376fb8dcc0e63c0bf13709bf68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_88 48a3cc5a2dc3bd72ea565c1409c4225ddea489e3eb0e607e72887a2eda100cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_89 20f71d7213f8bcdeec3016a68885000588642ca4dfd6e62b0231c47483426322 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_90 dc8e7fee8684f28596c664367d6caec1772fa4be0055f56d7fcc4487e29835ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_91 c36530df2d2ace28c455511f56ebb42c6ddb0d363e24b6f684238e6cfc55afda -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_92 ea2b17aecfa1afa3a326e89d3ab51228aff5788b85a6ac068001fde82cb55904 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_93 f0270c692e9cebbc9c6cd1fb0a0ea9e97d5b0d0fb08267bc0c9fa543e75a7aa5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_94 bfa8c8455b4a4dfe315572cdab5b0c886deb45d621aa001185997724bbd5a2ef -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_95 10709b8d222bd3601616bfdc540ee1959f821c79d9cdf5cc7d920fbad5ef5dae -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_96 75274c834be17ccec2021516fa68bb7bdfd5f51481ae922f7fd22f22b72e26d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_97 b8b0e5b952adfe9acf79bf4f9cafa3929409f30174e3cf00d48d521b92a0d4b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_98 0995bdcfb0e29a5dde0e20b3561654dad6ca998383b7d1ffaa19b04bbbe7eeb4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_99 fa0109d6f09f1b25926dd6a0d9510d2f40f90aea9f952e996dbb4b4ce19492fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_100 79c348bf404cb6d1d9730bc0fe69c2b7afbb378a5be23ce1ec12abf0f1930f6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_101 674a2b684e887e037245a5a53f81abf1daa0240852216fb7bcc4aa7be2492a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_102 aa68c0099ae50858dae6b725f65e6523e87436cd15b7a554725dc84f1e13579c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_103 0ae97e666bb1fc61a47cf54d18d5ce90b6968e85323a00d9cddc954e971c0ed3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_104 4191fc64bef3fb4fc62b45c9606981185ecb2c680b3a033bf9b859a32e9d1cac -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_105 3a6985ea98fd3fff67108431da7c6baa69d626eb023401eea7c28c2d8a15e677 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_106 6d09a1035ef9282d46cdcff952c41b1e9d9dca3e99d17c968438fd58c2ce5f6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_107 a34e06def616d4c8455d9afc0fa37df795bb41289ddb6aaa5454d5549b726580 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_108 ed64eebe647ce9fe8937c4aae9f576b85f4c23b154029207ce5359e16fc7af5b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_109 2e46c8cc60ea312c73cf323981efc0129e538d6b99eb765920c6a92067b52e11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_110 0bba435989a4d0d5fd03405a455c123808a4f63feab5e478596a6a9eed8e4ff8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_111 2fcc7eeb8295be347ae505235bfe4eb97fbc3cb11ca525a4765eaa4a5e2a381a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_112 9dcc8b20179d751d5027580e9ae311a269cbb4fbba5b205eb6c009bac326a35e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_113 5f82c63bb4d635fe002b0a8a102416e73b96de73bd60b8fec0676515aafbe210 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_114 068bac88abdfd674312fd5f9c39c6bd120aa4d0acc00adebf6e4dfa4ba62442a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_115 c544309df08a2ed3c3154ac0c164b5cf465b1bfba7a92e60f217cc64158f2f49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_116 63b0b3bd560956810c6d7c752c58257753a266bc11bbf947c9a2d8a28940d970 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_117 3ff84ec898bf3ab64c855bcdfa533d4c68b9fb57de2548e2f90c1f5cf10268fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_118 578214217a9f74b018ab5afe279543b9941f825dfeb78670ac88ba06dae4fe19 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_119 3b52232b95986beecc27715b67fb03c70b48f9d94afd69f143334c6744937765 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_120 b050c555095a722611748327d8df7326e3a73f21c821e987e62bcc93d30142db -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_121 ffb88111c39b248e84694cf164bd03b852c965a576607aad3f41fe23c58465c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_122 02f884299e5ddf2220908c222d6354ff91ff74604b4aaa6051d426eee1af61d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_123 860239f6025949f7fe8c6449d39b9263aed0e55fd848682f93ce926d39d37a61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_124 c87bed01fe4125b5279b2a5685fccde5f7dd1dd9fd97f27667a40858e62bde5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_125 ba6d43bd88968938364ee5d1e4250075a39a4593b783c31a463bc138bc88fb9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_126 be7901098ca3dd77ca6f7bff9bb1fdaba28b56fabe574f812bfbcf0f29cda86c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_127 0464cc8b6d95c7c4998d227fc4432df12e24a1573fd328c77dc981559143381d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_128 6560816f4ae0d8c3a2d14a2c242ca2c8b932fe776fd0f8aae76799389890d753 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_129 e6afad00f15b10b0fae400ae951fca7e7ae7cba36dabafab0aeecf162cd0c842 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_130 7ca64fcbfba26426f16daf70b38a8cb835bc487cb07b59963f8018fba3da9177 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_131 250b7edfda9ba19f74d85e141726309c7e3a824ad66b8af5dc8b8d6ae88a00c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_132 58f6d2baae3d4ccff3c55099adc59a9d6598b25785f2777243ba553584580e82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_133 e2540c7b77fda6229b1e07c6c3e23a4be9d2a79e8b92ecf6bfc3da2ea0870284 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_134 700784bc53f4d861640f9b1775caeca75dd8816ccddd9229ad08990c3fd2de28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_135 f2e43844433b3419623e5a236456e368c1d6922d4dc58591464ee9733cd2d42e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_136 869d81a966374dff855a1887833834345acefd279b19a6c2518f2b5b76ab8363 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_137 46398ef52f18167c13b0bd3bf76faa8a4a2acbfa4a9ee42689388205c5845da0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_138 5a891599a96c67f766135fe069464378bc3650528c377d2fe5ee8ca1c1f5d03f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_139 16c8bc6d9cc2112fb5b79a856301197571836614a05769628bb5e3e63b7d7794 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_140 884e6c52d1690b26d5e2772f751fc869606b4701c9778ffcc52bce5b0e1044c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_141 db390703b20b8668d6103534dddd5f608512966f2c52c300aef80af6a6b45a84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_142 20e6d5d2ee8648f83ac13a92e9ccf60592fc7e5e153b9b9579a69dccf9fd423c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_143 323e61b8abdd3ad6817b0523a22ab7e8ece60da380282b7bcee1f71320693312 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_144 c1ce6386c4254d2aa99bbf26c6cc6dcf2667e0f95d68edab252ce3067fb98d66 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_145 8e89c927a595dd2ad8f54d59592a73160ff6f43fdb5c9cbf6a18304143d6d5ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_146 9f84312748e69bc595eb9031f4ae308293bc003efca1fd461051a534697040d3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_147 2246e34da8782b1c89e620f5f52276f295452a32b79395aaabdad15059bde5e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_148 868602759b943c60c91766af951dd7218dfede3452ab821f545c22c85b631b7c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_149 71ca647c1c2bfa5eb82de4a2809f0ee448311cb55960448d18f2c1b29045dd4e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_150 4bcfb72e38b12776a61862e955cd0298cc5605c1b6dfe183d37099ab97412af6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_151 0bf1feaa7f12e8c99945912fe592fbccefd9e388647713076e4a000effa5a0c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_152 d4b9c4cc2fc0758a1d8e856ad1ca063ca9b37e7940f8bf79ee945b30681e26f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_153 a1b992caecf4edec162f3eea0095f1b187cfcbd2cce4cd4a6be792a89255ea21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.KeysData_154 baa61f77798ff79a6172a822031be16596db6780cc9956ebc6613f51bfd462a9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.KeysData 5233615b41886db83acaa2fc5632c50219c21d9c3c899ddc2f051bd5422505ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.WantCaptureMouseUnlessPopupClose b393e8a827b1c693eccb27b0bd9c326aaa33eb2a5738da2b76cb54342485567c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MousePosPrev a072468dec4c0a4d1871a2906c945e3be5d7d5fc697313c01f2113ae36cff158 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos_0 bf1c2a349e106a53e219b55e97bfaf592f2e1079fce5a0fd967af2549cd3654f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos_1 baa7d444a00c0b0e5c706087cc8ccf37945886c36aea6dd3eae56cbf7c39bafb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos_2 4b404e52f296a5ae12640175c009488c04ecfd7efe6f16bef7bb27224fbf7a0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos_3 400e28dddf66b44bf931f0a57dd7120161906c40a49a2f9328050d5cc96e27a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedPos_4 d1e1997dd9217e1593f2e8593cf592478d80b23e99f2775abd3557a9cb131002 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseClickedPos 566013071004e7687e57f4dcdd3edb5bca0d6aacc1657dd7d717804e607b915e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime_0 f036da92cea36899e1519999f2190d30916508475cb3ddab15298e33fac5070f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime_1 f497a2eb59ccc1f7ccf00e4cae8bc5d6905a7a015d277d2f44833057fc63c406 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime_2 fe911317ca82d9a5a150513851fc3b0cef00102d17b7904d3b1e7769d362a88c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime_3 0d218263f2b7b32c3abfe340101ee58fd1e59da9f581fbbf268d9dc043f4e41b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedTime_4 77a2711717d9e9ca947a9ea6661e28a5b2eefcf3219053c56176d5018c803bd7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseClickedTime 49b9dfc4ed576f33b8cea04e41c2dba6108d5743db1fa814fac7e53c54943aa3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked_0 9ce9cb07da49718d7dba26d5c2116dd51bd4ff18ad4bec09338ab697c073d8be -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked_1 f6d5c74cec3e9584cc47be79eac3c4a375a643ecb99268d1b9a7f24b7ec4829d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked_2 7a75591a35a50088b4c47f3e94791e85d62ec0b038f038d3d183894588786864 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked_3 4075dcaa9d733f6ac4b174683440503143c9c59acb5dbdf2382ec90a7835d363 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClicked_4 6102c5b98c00d14e2f0d6eec808da937fa38193e6c88cdab89ee816db64a21dd -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseClicked 1b02bb7191ecea83661e15bc3505100ab1506743cea4220436357307793ecdcd -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_0 91d19ab0c15f61c8f4652e4957421a88ba6abd5637831a4ed362583c60396a64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_1 69984ff7f8a1a8615a57feba6a61be1a6dca434b3ce8769768de47e41cf4802a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_2 858dc58d2e9517ffd3d38e17de0a107f385fe9d64028fc6667973f6263f542c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_3 30eb814bcbd6961bc361e93c551340e5c10c17906a5f6e2b44312c3adc1d9f72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_4 82e468ec21cfe99b0e7b7a1f4b2aab726372a3ca02a3259939eae12472b2a5bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked b5d53373087f9fd04c311de3dd2e8545407d69f451f8ef9f5f7111828394494f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount_0 8b888aaeb32d2707705f16c502c885e0c2d57c11d2bbfc80883a53d37989525a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount_1 0c9c49ae9bbd836ea4e8234e7e9d4cc0aff4f84ecac7e86e9db9b5c46b3586fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount_2 df897f2e2d9b71e839163a83d8a917a9f2717a0290b70edea70a665dfa4822c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount_3 3a3cbe93cdcfd79d64bc6f0efc271423ea2e3c6c4144ec5c418c950102c88117 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedCount_4 87ec445d482a3302c97ff2f4ecfe9b2d124b75248de293e2874a87a5115360ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseClickedCount 6a20c51dec46c16ff6a6e7388ff3ce9d871c94bce15718f7945f99e6cd83321f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_0 beac2aadf0bbeaca5e5142b52b285cc2f3b84265389b7ce6e1cfbad0d6fc947a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_1 dbb23c5782f8a98b2266b28b50c8b7e614961f07fbf8db70b61d736486a5e6e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_2 9bf6869f93bb0af0c7d1e88cdc36db20705bd261a98ea4a33b98a74943b30cd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_3 b1a8bbb0a62352bd4d3cd97fe3557f28c786fe6f0dfce8534fc0df3743a83fad -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_4 30f10e48f943fca838f89da5e678fe2cacbfae94272997f60a91fd13767e5da2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount d6fcc7b17524cb316ce201e681580cfef1d3dae677e015d6aac812511104a1e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased_0 a14c6fc133aa02f84730b0c6f96056f9889a31ef7b986f02c188a9b16e58d008 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased_1 1bec14a32c697f00738bc575074730b8dff72bd692a19899373faa4b8b60d1a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased_2 438b9be280d4ec74f46cebedaee881e0a3e14e873f55855f51dacc27320e9126 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased_3 2551015fef8d3005ef40120ecda59de6111d23726f113c162684ad5f11df332f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleased_4 b4b574181c65279133610dea487d6dc6f49ad80228a6f5e913037379ec1ac8fc -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseReleased e20cd3e5ada74bf5d0a39131bba97a25c082cf0ff60cf2adecf543e4e54b3c09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_0 ec588919b2ebf90bee7a879836c13c56050e76d9117074b882a1b2eecf53ee8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_1 5dd1680b7fc14853e7d7e472ccf4f6236f187a5d18102a08a3f0c2982302e045 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_2 7a0bdfd221d75882e9d1f5a0a12ecfdc43bd05ecd5c2c522dbe64c084f2a069d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_3 755eb7f8ae27192bca6b48b4093fc2a92076201323588dff3bf34188978500eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_4 92a251a99e07ff45c6a5e9a7d5921cb83a25d50e27e33f410846964a71defd5c -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseReleasedTime fd0561acf33b4c6f0b40f6c4a8e2c13854f5eff70213841682853f549a8d9f1e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned_0 b0b246762dd3302cafcc8e50c85354431f492eab8daf96400499ebc41532d4e2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned_1 edcb53c054c7d13a340985f453d568f359f54f0f6badd6e6deb50ec9f015e765 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned_2 02b931776348dd59fc95f1d0626fde2334f89ad069d0faa987215d7bba3eba14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned_3 02e611efd9a3909e89da3366c2da8c5046ddd83eb66085c69e54858d825fdcc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwned_4 a0904493447a05a28b9fc6298ce45ff723e31f2e82f48efe2863224f56addcdc -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDownOwned 31f73eb6fdce4ce966d3bc87d273a8fb7a0f7337192dc4ab9c0d229c9fe76494 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_0 9713cfd2e500e363b9c6766c6e8b14ef70edcf32ddb096c887fdfaff9f5a81b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_1 0806fae4e105c8a71c8789f04820eaa3d04d74b6405f7824dfbda7fe8564887f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_2 531031a0fe6650adf0f667075c970f63e0ec0f997088ce6a95256fd2bc8c618f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_3 9fabfc8fb4e4ed16aa201e1c5f85720ffed24d424d65e654bbfa4fd65ac1f80c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_4 9ca645d93188d59e136f8c1848ae8a520eadb124f35e86ed913fedd30d5945a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose 4b634f3b21498833f1f1e0f52caa65fef598baf8636f49e84f6033abbc107c0e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseWheelRequestAxisSwap 30c2ca7c4e80fcbd7fdfab4061f7029c27598b06cc4af7660343bbadc8190603 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseCtrlLeftAsRightClick b403c30b5980c07200c0d8237432599a5b6895bda889f6f307bd2e7a132ea954 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration_0 48656de689940aca9da372b61dc0bd025fe22a305d2417b18cd54597e959520b -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration_1 bbbbd9f359991c63642f54552992bb076a6ef1161af736a493fc4eb7504ef608 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration_2 bd076571ab502d71d2e5483ca0b35c7afc461df87f7ca827ea0c34acd0cd245c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration_3 0427eecae9bc018af97dea13a49f797ead7e9ac773bcaac3f3286c3e97b7521c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDuration_4 f3309626dbf2c4e52a06811a19d64309dc71a8bfa6bc72de30fc2100c80e0c2a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDownDuration 384376e81844d6c127e6b129044ab30db91067dac9e53debcc4501f53b76f68d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_0 bcbedf59bc421bfdc6aa98f1b2cc1ef774a31c8f84d42075c34589918af2f6e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_1 6853050f5dee886e071e30d85d3d56d8eb9676fa75578eb3ec29585206e22513 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_2 d77fe33fa902d461f43544a48bf8d198886f1daa9400823ffa3285304305af34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_3 8195e6d091f5da25c81da9234fd0c4e1c7f023c3ffaa36d154c62a960d6e9d5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_4 8341ac093bdf03a7623798d51dbc8c4417f28a5d817485c47bf4590b3c5554d6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev c4cab275e14bb0f90d2fa1daaffcee2f43678d7bc93a7b292dcff28b781e2da8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_0 fc2e996c4e593345e8bf8f08203f54767a52ee85ddd15e42efff02b27b245c72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_1 d5fdeacfc0be90471a6f452c9289e294c1bafa2576511823c11029572542336d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_2 63f44ea01a7e122be24ac584cccd63e7eeb8370ca26c42be6384374d7e0a1c0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_3 ceb518d436aae082018b3e28ae9e72c8d0324f077411efc3ecebd8f03de2800d -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_4 f74b0e7cc8b4aab3fbf2fe38a62678e0f531cb0183beb408f060adbbbf8e0773 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs d2073cf370ebaa5950403ff0d5358d7ff6785a2e4ffa66d2ecb79a934b1af179 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_0 5352143cfbf38a4fc63c7d8df0217f7a15f3ff2eb08c6c9ae11e9211a7cb5b7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_1 17e1553c5ea40c69e99c1a18c92703b081074c3bbae499f173533fa03b43e409 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_2 d136b2ee84ba02a3f15247d973eb0a1d46db586d3b4fd7e390ba622f1a2900df -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_3 5fa5a71371c29983e02c1725b8acd09b48a9b1d5f2b152130b0d3fe76355ab13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_4 1220f67297dea50c7121101984316d04f2b605b8793c195cbac788a1760e5578 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr 00a872685407cbed1d9270492f5b0b5377e4e9ec14c42b1f35435e2c68cfa88e -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.PenPressure 052c118310747cc773eb3a87b483683cf507de06054d9925ac8f0ed131a8a3c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.AppFocusLost 6cf42bf78b72e07c4165cf94d9b97a92fca4bcfc34d54d2cdc77695fe1122510 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.AppAcceptingEvents 6a16c680b1d6019ef470ca5fd7d7057ca21b73625d39ba30a0b9ec85aa0518cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.InputQueueSurrogate 5bda403c0aee2ad8020158ec9a82d8d2f1788066730d248e9923f474d7f5b865 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.InputQueueCharacters 87a971452b0ae8c997704a98733922187d7b70c85ca84aeb1c0604fe2a872544 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.#ctor(Inno.Native.ImGui.ImGuiConfigFlags,Inno.Native.ImGui.ImGuiBackendFlags,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Byte*,System.Byte*,System.Void*,Inno.Native.ImGui.ImFontAtlas*,Inno.Native.ImGui.ImFont*,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte*,System.Byte*,System.Void*,System.Void*,System.Void*,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Single,System.Int32,System.Int32,System.Int32,System.Int32,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiContext*,System.Numerics.Vector2,System.Byte*,System.Single,System.Single,Inno.Native.ImGui.ImGuiMouseSource,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Int32,Inno.Native.ImGui.ImGuiKeyData*,System.Byte,System.Numerics.Vector2,System.Numerics.Vector2*,System.Double*,System.Byte*,System.Byte*,System.UInt16*,System.UInt16*,System.Byte*,System.Double*,System.Byte*,System.Byte*,System.Byte,System.Byte,System.Single*,System.Single*,System.Numerics.Vector2*,System.Single*,System.Single,System.Byte,System.Byte,System.UInt16,Inno.Native.ImGui.ImVector{System.UInt32}) ff6cd740067e8c2cac4fa75cf7992442385a708bf7f8e8fda651fd60756dce98 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddKeyEvent(Inno.Native.ImGui.ImGuiKey,System.Byte) 2ae21a45dad4dc968343be82055146b3bc3927aa55dbd5193646098b54bd2e50 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddKeyAnalogEvent(Inno.Native.ImGui.ImGuiKey,System.Byte,System.Single) 8fe697ba285890e0735003b2592d3c5f16628ec5fe5173da448205ac947d6458 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddMousePosEvent(System.Single,System.Single) f6c80a25ec37dda6ec2996e9be80436daff71dd2db1a955b81dd35488708fb17 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddMouseButtonEvent(System.Int32,System.Byte) b716bf556a0db40897c0694ee557cff113463ffe64b87c6c1054da2e439562c5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddMouseWheelEvent(System.Single,System.Single) 01ef006af60a1c0c6a5fe522e6679a121cd6174d74b422cb4b308cd4d35f5416 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddMouseSourceEvent(Inno.Native.ImGui.ImGuiMouseSource) d20cd50077ab39f32d1dbce15f90d223369f55c915a1d8f596ab374963046065 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddMouseViewportEvent(System.UInt32) c8201b957f6d39688f6ccfead6c0f61e9753824d91c69344de7f8bd1403c4990 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddFocusEvent(System.Byte) 671304cf87bc54dd8eec70ae5bb61923cbb63a82921cd5e1de6bdbc19892009d -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddInputCharacter(System.UInt32) fd7a69a3d8241c2edd92a043c4daa00ed47c1c1befedbc304ca2fd1398e0ce86 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddInputCharacterUTF16(System.UInt16) f55d98b605074f515eb977a5ff16fba17d7feb542e1c46f38776743449ce035f -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.AddInputCharactersUTF8(System.Byte*) 75734ff542b5226a0cb05792650cb29849f27ed0d727252464c7ff3341b749ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey,System.Int32,System.Int32,System.Int32) d86a8530e15584927c4b0e695aaf92129cfbbd191f26754ede2c08999cbe6f0b -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.SetAppAcceptingEvents(System.Byte) dcf462430c271f52650e63a35da4e588159975872576f81b5fa5e85e7100800e -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.ClearEventsQueue 5731a6bbec364542544e857a3951bea2463d4f8e393812f1306cc02acd917778 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.ClearInputKeys 64d6abbbf8e411759aabdd96136db037d5924a939ecd8794aac8bd1e50ed2e95 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.ClearInputMouse 3e09b93540a9ce77f8f689c4a3a468f1aa29aef4ba741859138954c7497c9cf0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIO.Destroy d70155c2a41765ecfb8afc19d787507c12fbf28fd79fbea897943b921482edbf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ece5f3d6a509425d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.AppAcceptingEvents`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d0f81dccc9ebb8e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.AppFocusLost`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c097e8c0096fd87d"></a>[`public Inno.Native.ImGui.ImGuiBackendFlags Inno.Native.ImGui.ImGuiIO.BackendFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2c28ffa429dd274"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendLanguageUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a16ad28563e6188"></a>[`public byte* Inno.Native.ImGui.ImGuiIO.BackendPlatformName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a9051cade246ae8"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendPlatformUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-537a2022fbf1b21f"></a>[`public byte* Inno.Native.ImGui.ImGuiIO.BackendRendererName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e41c5dee4bcef5bf"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendRendererUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c3fb0f856f6df8d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueLoop`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2563) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d94d6cbe007e5395"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueOnce`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2562) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a370c5ffe4ec2890"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflicts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2560) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4acea1642217f2c4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2561) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d56bdf52e163966"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIgnoreFocusLoss`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9789cd96e7e6fdc0"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIniSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2565) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efc872d50a8462c5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIsDebuggerPresent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2559) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07c51a5d384b1ab3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingAlwaysTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02c286d2cfb612bd"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingNoDockingOver`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2528) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3449dc3107a16581"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingNoSplit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2527) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dcd381ba6e1761cf"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingTransparentPayload`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b64ccd01b25e54c4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingWithShift`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e09180a93539d7a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleFonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2537) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5663cd5260bfa304"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDpiScaleViewports`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2538) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-087ff95be3c765bb"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDragClickToInputText`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eae48247ef845f3e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecovery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2555) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03cca90436af44a4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableAssert`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e960c39ef259843b"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableDebugLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2557) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cee4a06ccf209803"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigErrorRecoveryEnableTooltip`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-184f99e237853d35"></a>[`public Inno.Native.ImGui.ImGuiConfigFlags Inno.Native.ImGui.ImGuiIO.ConfigFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42f4018231cfa095"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTextCursorBlink`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2542) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-567866c2b785d7b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTextEnterKeepActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2543) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10d6f7d19f68bb2a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigInputTrickleEventQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2541) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae9509680f32d318"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigMacOSXBehaviors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56b6fbb530c90d46"></a>[`public float Inno.Native.ImGui.ImGuiIO.ConfigMemoryCompactTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a751e2b60bf27059"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCaptureKeyboard`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-310d3afaa8461378"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAlways`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2526) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab133ca362e703fa"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavCursorVisibleAuto`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3591b2d4c924b2f2"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-778715af2466ad31"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavEscapeClearFocusWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-819edf88ba409835"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavMoveSetMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb5cc9395f33d651"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigNavSwapGamepadButtons`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a440538199988f1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigScrollbarScrollByPage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a771aa0f35cf7fb"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoAutoMerge`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0dc972a8fe2ad04"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDecoration`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1482833b9dbcd8da"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoDefaultParent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2535) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd6bf771b639f67f"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsNoTaskBarIcon`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-190e57848928db1c"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigViewportsPlatformFocusSetsImGuiFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2536) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2aa82f26b60e6963"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsCopyContentsWithCtrlC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69477f2b6692aa5f"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsMoveFromTitleBarOnly`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae18ee57769f3d3d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigWindowsResizeFromEdges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2156246a1f875b93"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImGuiIO.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2584) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d15625fd707e209"></a>[`public float Inno.Native.ImGui.ImGuiIO.DeltaTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d38f83350f233b1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.DisplayFramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cde9bc29d29a5459"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.DisplaySize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41d4bd1035613d39"></a>[`public byte Inno.Native.ImGui.ImGuiIO.FontAllowUserScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01fe95432c4a55ed"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImGuiIO.FontDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2518) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85490f60262b5a9a"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImGuiIO.Fonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2517) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08db12e804a506db"></a>[`public float Inno.Native.ImGui.ImGuiIO.Framerate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2578) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ac62ed66d28fd67"></a>[`public byte* Inno.Native.ImGui.ImGuiIO.IniFilename`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6867590e5013ad4"></a>[`public float Inno.Native.ImGui.ImGuiIO.IniSavingRate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0060caf8432fd452"></a>[`public Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImGuiIO.InputQueueCharacters`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68fa36a4f1a60c03"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.InputQueueSurrogate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbd95554dc06eb93"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyAlt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2599) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7a52490065216b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyCtrl`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2597) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7e79edc06a06d3a"></a>[`public int Inno.Native.ImGui.ImGuiIO.KeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2601) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0a4f8ddd849027c"></a>[`public float Inno.Native.ImGui.ImGuiIO.KeyRepeatDelay`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-002f1136816a1f6c"></a>[`public float Inno.Native.ImGui.ImGuiIO.KeyRepeatRate`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4999a7c5a913c301"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2602) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d950145d4fc8a49"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2603) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bfd124071f592b81"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2612) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4407fdced75a73d7"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_100`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d24abf5e5078a3b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_101`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90805c9919d1f6ef"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_102`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae65892354965e62"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_103`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b306f690c7efb09b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_104`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be6202be62ba35b2"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_105`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3275a6eda99d3076"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_106`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52be75e863904bd9"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_107`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4609deee7160257b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_108`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b8ae3cb0e8bce1d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_109`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d895e5b20960f07e"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2613) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3cdda9f9dc62a753"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_110`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d3e9bcf716ec7d7"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_111`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16ae74dfba6c924f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_112`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a83e2e442154da0a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_113`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2715) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fd16d8eb52f66e3"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_114`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-303d265b8ceff4b2"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_115`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2717) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db375d62d32b0f36"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_116`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2718) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-852f9c0a708f9bca"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_117`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2719) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54a6efe9187b7c5d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_118`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6777bc3e94001b36"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_119`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2721) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7138b2691516e97a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2614) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce72cc53824708b9"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_120`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e727dd80f2bab840"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_121`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2723) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ba9af7444f1351c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_122`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77ee48cdb7d802bf"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_123`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f03b731615ada34"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_124`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-472d21476e08bacc"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_125`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-119150cb2ab45813"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_126`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf3d30e7776cc04c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_127`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35f3069b08611443"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-405832ed7098fde0"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_129`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2731) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f64862665dcebf7"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2615) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d97320c87f3c224"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_130`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58dd552d77275e5f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_131`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2c68ee9d8d1d9be"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_132`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4a4698031dcfd33"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_133`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6dbcdcf30571fbbe"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_134`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-912bf98d5a5d9289"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_135`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fbd4bdf25275c21"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_136`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2235f671423d12d3"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_137`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2739) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96e9c8b716972311"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_138`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e8c4560c41625ea"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_139`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abb24e13e135c428"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f2b8839e5a7b3ab"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_140`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c080ccbb7250ebd9"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_141`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2743) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e4ae720aa0de10a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_142`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2744) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61f8da80d1cd15ab"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_143`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2745) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f6ae6ca5849d999"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49db531b76845bcc"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_145`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2747) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-954d06e3f7fc7689"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_146`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b2c95873ce10ffd"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_147`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2749) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af53d8594f8ca6cc"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_148`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2750) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0406748320240a11"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_149`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea2c42e381ede2cf"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2617) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d65d782f320348b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_150`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97bb9951fbb8760a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_151`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d107515bbce96be6"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b4757c27c16921c9"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_153`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ec6e4adeb9b1881"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_154`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f3032840b2f19f8e"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33b6f8213e6dc0c1"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2619) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54fb15a957df33aa"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb455d2839ce3348"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2621) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf2d46f6d03e768e"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2604) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b47e89f1826b17ef"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d99d90af4c2e42b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2623) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0408e510159f8f8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec3ca283fbcc828b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2625) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6be4534ee7278583"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e72c6efd06708700"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-168391b00a57982f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bdd8a411778d091"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2629) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7149176fd890f9f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2630) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-963b86bf7d7e40a7"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2631) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-818b242cffad0f0c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2605) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54664a9265cb298e"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2632) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc1cae669da030a5"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2633) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34784e02a7e9b5ef"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2727f90dcd941dbd"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2635) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-607f0f555710531d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2636) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48e497280fc4de7c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2637) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-936f3c7567da54fd"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2638) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d3a56ae81d710e8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2639) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce237c6a4bb4dd48"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2640) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-332c6673d6509992"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2641) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b272ff8eb28baaf8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2606) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-323eb20c0bb3671c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf2e3c274c72887d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58398424cdbb425f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2644) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6bc874a91255c76"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2645) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b606883837241c51"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2646) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5df99551c38f8c8e"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2647) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a31a3b5a1b508f0f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2648) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b64dbdfa100b0148"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2649) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b90ee438b522c6d1"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2650) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3cca24ca7452216d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2651) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95eaf0cba682d9d1"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2607) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbe07d11dfe60fe4"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2652) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a70603e5578210c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2653) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53297da7818983e1"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2654) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-036c4b4c24a34a58"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2655) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80a36d50d8b66ee8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be16b4bb78e97b50"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2657) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-533a24c1aa26ce14"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d77f072b40a24b2"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-090d7513cd21684d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5162b17f7f4f6131"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2661) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e31baf98d5b57ce5"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2608) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c73baad2c2c02d0"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2662) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5d610b72947d8c1"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2663) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f22428fe3acb6d4f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2664) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9db851c2237ddcfa"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2665) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9138038acc4f5d5"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4477f08f63ee7f8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_65`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2667) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60ca8139836dce57"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_66`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33acb3384ef79b1d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_67`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2669) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f74095603248f95b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_68`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2670) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bd3b6089a8590e2"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_69`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d852ffa20a98b083"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2609) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f7f993cdf309fee"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_70`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e7e496ec9ec1355"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_71`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14ad31747d387f0d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_72`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd34066244a4eb6a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_73`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2675) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acbedf10464bb416"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_74`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13da1ce83c4f3201"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_75`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54b999f1e55fa0a8"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_76`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-660c2850e00ea55d"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_77`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9493e4ad3d613d49"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_78`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cca1421be39683bd"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_79`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec792076b12376a0"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14d3f705eaa7e30a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_80`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4813e57fcd38e966"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_81`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2683) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32e268aef65b9a06"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_82`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c616b18b7a0a9bab"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_83`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2685) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89ebae0fe84462b9"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_84`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6c2fde360a6a78c"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_85`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2687) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d47ed16becffd446"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_86`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5232012cccd9673"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_87`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2689) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6545e17683015123"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_88`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83433d44bd9b3518"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_89`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ef4d8df9a61e4e0"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63c36c820a4ca92b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_90`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62cfaceeccc6f825"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_91`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-195d2802ced001cb"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_92`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6480e7bc3e2c927"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_93`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2695) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da6e1cdaa751fd48"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_94`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a268bd36122ff9b"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_95`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59c7184469591c9f"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_96`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b01c88192661ca48"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_97`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05e7b2e4b1128234"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_98`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-226504613ceb024a"></a>[`public Inno.Native.ImGui.ImGuiKeyData Inno.Native.ImGui.ImGuiIO.KeysData_99`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2701) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43c30de04dba7c4a"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeyShift`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2598) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e3a3071e63ee384"></a>[`public byte Inno.Native.ImGui.ImGuiIO.KeySuper`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2600) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9b83e446834c067"></a>[`public byte* Inno.Native.ImGui.ImGuiIO.LogFilename`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7e2eae2d6ffa0f4"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsActiveWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2582) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1864c0eb06bfa3c"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderIndices`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2580) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67e9188ba3022d1a"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderVertices`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2579) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05aab965896541c6"></a>[`public int Inno.Native.ImGui.ImGuiIO.MetricsRenderWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2581) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19614d4f36b5dafc"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseClicked_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2775) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6227b7044de7ec2b"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseClicked_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2776) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c02c124f3978cf68"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseClicked_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2777) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c9bfee3eb4894d8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseClicked_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2778) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07a341c77bd3556f"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseClicked_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2779) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc63a9b77be11527"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedCount_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2789) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c8d4beb74a7b41d"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedCount_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2790) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a566d02bc60141f"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedCount_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2791) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-506269e14c2294e9"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedCount_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2792) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aaac395ffdc8fd16"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedCount_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2793) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b00a79da15c9330"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2796) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8f1148ebbd8a117"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2797) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b6d0b51884e985c"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2798) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da03ed6fff94d98e"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2799) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b9033445ca22ead"></a>[`public ushort Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2800) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cc72e5a5a31f6f0"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseClickedPos_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04da88a29e95c41a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseClickedPos_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-24f18f4427961840"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseClickedPos_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62f0ec0c205f3d57"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseClickedPos_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c851750d33af923"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseClickedPos_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e5738e5e3177c8e"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseClickedTime_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cee7699c37650a20"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseClickedTime_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79d9f60d2c666622"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseClickedTime_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2770) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77be13340ffac490"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseClickedTime_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2771) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02147b784a82a28b"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseClickedTime_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2772) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05018607e1fcf145"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseCtrlLeftAsRightClick`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-566b1ca5c686c6ed"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDelta`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2583) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b51489e8760507bd"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2782) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98eeff973f4d2204"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2783) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e915e760f977fec"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a91d696372491707"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2785) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ad79c8c176accc0"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2786) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92e5523943017cbc"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDoubleClickMaxDist`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2551) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29ca33ecc0716ee0"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDoubleClickTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b4dec9e4c15a993"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDown_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2586) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d35747c75845217d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDown_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2587) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d52ca647a581b7c"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDown_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2588) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fb8a39e15debda1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDown_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2589) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e84cdf0232df7e92"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDown_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2590) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4135fcc6033ae17e"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDuration_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d80320d5205f0ae"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDuration_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ecf6bb335d77596"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDuration_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b02bbef1e813c361"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDuration_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c46d6ebce4639567"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDuration_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-012acaad071147fa"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2703a6e2a5061ef"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2841) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96980c35893f0e7d"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-835886e5223f9a72"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2843) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf1df805bb70b47c"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2844) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78d5df09f37705f5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwned_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2817) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6dab8dd78f64705"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwned_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b893038027d8c00d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwned_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2819) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23e1cc2cc8aaa0aa"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwned_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0253a964c0fb9fb3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwned_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a15f813707cb30b"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a06e448e7497247"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-562f90a0b7f5fc85"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3989a6f4bec18910"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e458484dac7ac840"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d77e1fac993ab1d7"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2847) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-754c2295ba0e41b1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ba4eb6611da4dc2"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2849) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c46d5139d1e2414"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2850) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-771075f72302a0dd"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2851) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35b3b0ba24ac0ec8"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb74cc99748c862b"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2855) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92ad69c6480a4dc1"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2856) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92b4071202682240"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a24c16c3b8180af"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d17add21bf837763"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseDragThreshold`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a26b8229d50128e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseDrawCursor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ca28a9053713af3"></a>[`public uint Inno.Native.ImGui.ImGuiIO.MouseHoveredViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2596) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8fb045377c429e00"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2585) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37236031b9707a24"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIO.MousePosPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8136a53a82daf8e7"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseReleased_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2803) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c0ded7556d75ad1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseReleased_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55ff68d28c1b98f3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseReleased_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e18c0820d199f13"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseReleased_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec06adf4d9a60e19"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseReleased_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41c153da5b1b6ad7"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93764f73d328570f"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2811) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b3599e5a56b415b"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-295eb494f241b758"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2813) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7448c46db3addb4b"></a>[`public double Inno.Native.ImGui.ImGuiIO.MouseReleasedTime_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-062ec2b8e7fdb03f"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiIO.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2595) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6958a8642556a3b"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseWheel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2593) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8e168819a5bf1eb"></a>[`public float Inno.Native.ImGui.ImGuiIO.MouseWheelH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2594) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b77a71ab15c37ee"></a>[`public byte Inno.Native.ImGui.ImGuiIO.MouseWheelRequestAxisSwap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37b212a3306586c5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.NavActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6a0c1fcf41ca5b5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.NavVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2577) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a41e72d3d0c8f858"></a>[`public float Inno.Native.ImGui.ImGuiIO.PenPressure`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f02740ab587353c5"></a>[`public void* Inno.Native.ImGui.ImGuiIO.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2516) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d50426b4c2ceab5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureKeyboard`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82187aa0a4f931d1"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureMouse`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc3d1ff57c89c01c"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantCaptureMouseUnlessPopupClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72a20cd55108fdc3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantSaveIniSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56b3ca1836b1d0b8"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantSetMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dafedeb6bf8a660d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.WantTextInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2573) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7746f7117fe18c6e"></a>[`public Inno.Native.ImGui.ImGuiIO.ImGuiIO(Inno.Native.ImGui.ImGuiConfigFlags configFlags = Inno.Native.ImGui.ImGuiConfigFlags.None, Inno.Native.ImGui.ImGuiBackendFlags backendFlags = Inno.Native.ImGui.ImGuiBackendFlags.None, System.Numerics.Vector2 displaySize = default(System.Numerics.Vector2), System.Numerics.Vector2 displayFramebufferScale = default(System.Numerics.Vector2), float deltaTime = 0, float iniSavingRate = 0, byte* iniFilename = null, byte* logFilename = null, void* userData = null, Inno.Native.ImGui.ImFontAtlas* fonts = null, Inno.Native.ImGui.ImFont* fontDefault = null, byte fontAllowUserScaling = 0, byte configNavSwapGamepadButtons = 0, byte configNavMoveSetMousePos = 0, byte configNavCaptureKeyboard = 0, byte configNavEscapeClearFocusItem = 0, byte configNavEscapeClearFocusWindow = 0, byte configNavCursorVisibleAuto = 0, byte configNavCursorVisibleAlways = 0, byte configDockingNoSplit = 0, byte configDockingNoDockingOver = 0, byte configDockingWithShift = 0, byte configDockingAlwaysTabBar = 0, byte configDockingTransparentPayload = 0, byte configViewportsNoAutoMerge = 0, byte configViewportsNoTaskBarIcon = 0, byte configViewportsNoDecoration = 0, byte configViewportsNoDefaultParent = 0, byte configViewportsPlatformFocusSetsImGuiFocus = 0, byte configDpiScaleFonts = 0, byte configDpiScaleViewports = 0, byte mouseDrawCursor = 0, byte configMacOSXBehaviors = 0, byte configInputTrickleEventQueue = 0, byte configInputTextCursorBlink = 0, byte configInputTextEnterKeepActive = 0, byte configDragClickToInputText = 0, byte configWindowsResizeFromEdges = 0, byte configWindowsMoveFromTitleBarOnly = 0, byte configWindowsCopyContentsWithCtrlC = 0, byte configScrollbarScrollByPage = 0, float configMemoryCompactTimer = 0, float mouseDoubleClickTime = 0, float mouseDoubleClickMaxDist = 0, float mouseDragThreshold = 0, float keyRepeatDelay = 0, float keyRepeatRate = 0, byte configErrorRecovery = 0, byte configErrorRecoveryEnableAssert = 0, byte configErrorRecoveryEnableDebugLog = 0, byte configErrorRecoveryEnableTooltip = 0, byte configDebugIsDebuggerPresent = 0, byte configDebugHighlightIdConflicts = 0, byte configDebugHighlightIdConflictsShowItemPicker = 0, byte configDebugBeginReturnValueOnce = 0, byte configDebugBeginReturnValueLoop = 0, byte configDebugIgnoreFocusLoss = 0, byte configDebugIniSettings = 0, byte* backendPlatformName = null, byte* backendRendererName = null, void* backendPlatformUserData = null, void* backendRendererUserData = null, void* backendLanguageUserData = null, byte wantCaptureMouse = 0, byte wantCaptureKeyboard = 0, byte wantTextInput = 0, byte wantSetMousePos = 0, byte wantSaveIniSettings = 0, byte navActive = 0, byte navVisible = 0, float framerate = 0, int metricsRenderVertices = 0, int metricsRenderIndices = 0, int metricsRenderWindows = 0, int metricsActiveWindows = 0, System.Numerics.Vector2 mouseDelta = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiContext* ctx = null, System.Numerics.Vector2 mousePos = default(System.Numerics.Vector2), byte* mouseDown = null, float mouseWheel = 0, float mouseWheelH = 0, Inno.Native.ImGui.ImGuiMouseSource mouseSource = Inno.Native.ImGui.ImGuiMouseSource.Mouse, uint mouseHoveredViewport = 0, byte keyCtrl = 0, byte keyShift = 0, byte keyAlt = 0, byte keySuper = 0, int keyMods = 0, Inno.Native.ImGui.ImGuiKeyData* keysData = null, byte wantCaptureMouseUnlessPopupClose = 0, System.Numerics.Vector2 mousePosPrev = default(System.Numerics.Vector2), System.Numerics.Vector2* mouseClickedPos = null, double* mouseClickedTime = null, byte* mouseClicked = null, byte* mouseDoubleClicked = null, ushort* mouseClickedCount = null, ushort* mouseClickedLastCount = null, byte* mouseReleased = null, double* mouseReleasedTime = null, byte* mouseDownOwned = null, byte* mouseDownOwnedUnlessPopupClose = null, byte mouseWheelRequestAxisSwap = 0, byte mouseCtrlLeftAsRightClick = 0, float* mouseDownDuration = null, float* mouseDownDurationPrev = null, System.Numerics.Vector2* mouseDragMaxDistanceAbs = null, float* mouseDragMaxDistanceSqr = null, float penPressure = 0, byte appFocusLost = 0, byte appAcceptingEvents = 0, ushort inputQueueSurrogate = 0, Inno.Native.ImGui.ImVector<uint> inputQueueCharacters = default(Inno.Native.ImGui.ImVector<uint>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2866) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ab4220768ca5803c"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddFocusEvent(byte focused)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63324) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d881b687aa50df29"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddInputCharacter(uint c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63335) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5c11363ca4d2bcd5"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddInputCharactersUTF8(byte* str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63357) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c21a4baaa84f1a4a"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddInputCharacterUTF16(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63346) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c534c3dfe74b3e76"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddKeyAnalogEvent(Inno.Native.ImGui.ImGuiKey key, byte down, float v)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63258) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c426f72e0e504d0f"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddKeyEvent(Inno.Native.ImGui.ImGuiKey key, byte down)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de3862e628a6c610"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddMouseButtonEvent(int button, byte down)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63280) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59284dded1671a27"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddMousePosEvent(float x, float y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63269) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69e9f40a10655500"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddMouseSourceEvent(Inno.Native.ImGui.ImGuiMouseSource source)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63302) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05424bbcbf9a07b3"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddMouseViewportEvent(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cd626fd953e99dfc"></a>[`public void Inno.Native.ImGui.ImGuiIO.AddMouseWheelEvent(float wheelX, float wheelY)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ae3b14d6c81ee9a"></a>[`public void Inno.Native.ImGui.ImGuiIO.ClearEventsQueue()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63390) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7d3f87304b2bc33"></a>[`public void Inno.Native.ImGui.ImGuiIO.ClearInputKeys()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63401) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3611dbb89cf2bae5"></a>[`public void Inno.Native.ImGui.ImGuiIO.ClearInputMouse()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63412) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ba58e662bb45dea"></a>[`public void Inno.Native.ImGui.ImGuiIO.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63423) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5fb8b2411edf65de"></a>[`public void Inno.Native.ImGui.ImGuiIO.SetAppAcceptingEvents(byte acceptingEvents)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63379) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f22259628e2692f"></a>[`public void Inno.Native.ImGui.ImGuiIO.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey key, int nativeKeycode, int nativeScancode, int nativeLegacyIndex)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09239f7f509004eb"></a>[`public System.Span<Inno.Native.ImGui.ImGuiKeyData> Inno.Native.ImGui.ImGuiIO.KeysData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2757) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c8da9698e4cd96c8"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseClicked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5b4216bdc49549d4"></a>[`public System.Span<ushort> Inno.Native.ImGui.ImGuiIO.MouseClickedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2794) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5d0721b18dd63bd8"></a>[`public System.Span<ushort> Inno.Native.ImGui.ImGuiIO.MouseClickedLastCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2801) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f3a70a1c7056e60"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiIO.MouseClickedPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e3e94ad6ad6b1a3"></a>[`public System.Span<double> Inno.Native.ImGui.ImGuiIO.MouseClickedTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2773) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a5ee3a85085a8f82"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseDoubleClicked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2787) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-caf6a55f5fcc77d9"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseDown { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2591) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f2290110b74447bd"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIO.MouseDownDuration { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2838) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ddaa23d1bda1dfa"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIO.MouseDownDurationPrev { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2845) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2a18c8a8f67441a"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseDownOwned { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2822) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4cb6afdbd7f17a9c"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseDownOwnedUnlessPopupClose { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2829) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f259f548827f9ff"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceAbs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2852) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54beb02bbb48093a"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIO.MouseDragMaxDistanceSqr { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2859) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20dd237ad9c160bb"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIO.MouseReleased { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d988b6eeae9a1074"></a>[`public System.Span<double> Inno.Native.ImGui.ImGuiIO.MouseReleasedTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2815) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-efbc6af74c9c086c"></a>[`public struct Inno.Native.ImGui.ImGuiIO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2506) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiIOPtr</code> · 143 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiIOPtr f3245cefb16ed04d920eedb0a02bd789d11c57f77d98463d834f59c662b5ab8c -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.#ctor(Inno.Native.ImGui.ImGuiIO*) 0391ad647a2be7000f45a99c73cde0fff85475d1a5eb32bd62f2f8394771ffce -->
<!-- api: F:Inno.Native.ImGui.ImGuiIOPtr.Handle e34a792306bac03e5ca9b03206beda9f50c8ee2f6557ecb96a6f87cb31e29ea4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.IsNull af39e16cbe28f5a60c251481d5d21a8edce9fe71b76085098ea4ba5aa5f0ef27 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.Null e1f33dd89dd84c19e50ed31a4cf6c82d2dcd90ad80c450507039fa6f40cd8ab9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.Item(System.Int32) e5357a7bf6ac007288ae38cb7cd3cb3ee8aa9950f62adfc3e4a014dfdc184866 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiIO*)~Inno.Native.ImGui.ImGuiIOPtr c595db51f7e872a4b096d0a474837a2e6d16715fcfed41fda864ff34216f4892 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiIOPtr)~Inno.Native.ImGui.ImGuiIO* 0acd06b4a3d6308dd343a0d49b346c486ba4bbf6588f3622d761cc2da28c09dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.op_Equality(Inno.Native.ImGui.ImGuiIOPtr,Inno.Native.ImGui.ImGuiIOPtr) 688ce5872d0407cc7855912ab08fa1bc2927488defb66f4f4c529869a65bdb03 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.op_Inequality(Inno.Native.ImGui.ImGuiIOPtr,Inno.Native.ImGui.ImGuiIOPtr) ab94adfbbf3a5b82baec1c76b0aa7c2b0854e15b4152eddd636f26c06b8ccbde -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.Equals(Inno.Native.ImGui.ImGuiIOPtr) 3e924ed72ce6429a76a9ea5370b2810c86479cf7d70760a80a31f64cb307b751 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.Equals(System.Object) bf5d4b6a709a4866c336591f2cf56367eb9630d7084086a276b1935b72efb737 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.GetHashCode e6ee83562348db44c9b0feceafb50d3b4b9155867c2556edc4d3c47bf9943072 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigFlags 5b18fdd853e1c7bd1c2270dc546b313f2a6403ef77720cb31201012ed6b9c67a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendFlags f3e575f21f8d59000915dc999d6b6969ee3343bc5a953188be33057a108ec34b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.DisplaySize d04b2852b299e68cb1f082b1922396ba6bb55f1d9c40b10cbc09e482d71c93be -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.DisplayFramebufferScale f28ee68e25a49ab6fa35c3a189117c4142aa2533c696330d24681ae15e109c23 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.DeltaTime 65a1d6d9061906026016ee953a3e072e55b161d0cf75766c1ee10222d1f5646a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.IniSavingRate 1d4faadf8fe0f4dc727e0ceb4098d741f9c59d5742b3e329e84d399b62bc3234 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.IniFilename ef243aa87c184270b74c86d4b902f4dad32e2364180c8b8aa7dfa16cb54de834 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.LogFilename f22562c61d3960115e60092b822daf3abd85d675f1bf94107ce98cefcfddec2f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.UserData 5669a07844b9eea5338e2bbbcdc92fbc391c7dd132a6688b9dfb2550be3605c2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.Fonts 48ccb9276e3a7be813d43f969a1800d6045c0162ff3926de769866ade6c74ce1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.FontDefault 6241b70983f390c4056e544284b68c383f1e5c8eeb13b65e584debd2df5d2813 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.FontAllowUserScaling c8e21842ab50ea37e4d24a04f98e1b819a59ca1cb7459a4be6acfc1a1a063e1b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavSwapGamepadButtons ab8f2938463794b1a37cfef06196b070dbe13c644af8383de3a720e6789e9ca4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavMoveSetMousePos 9cc3ec44982b938127aae6176205d312417c5da36d01798166f2b1db39abc5b6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCaptureKeyboard 59832f8e22e2256452049a0fae412d71d50c1fbb5e3ae4dc6a57cfaacc88e15d -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavEscapeClearFocusItem c3621534d7ce95e4578a2c63a5c33972aa04a98f46381504e5836ff0e473989d -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavEscapeClearFocusWindow 8b69dd65015483ee7503c9c21cee1eb29a5d4ed94c8bdffd4b80a37e64a5569e -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCursorVisibleAuto fd77e25a58f76be0d2f48a38d84ca17806c2c744d40903f3665c859c67cd2796 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCursorVisibleAlways a0d9a4c8da11a78d27e35bc38d16d01013fe1930309cbe2decf4a1fec8be72d7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingNoSplit 85715228a408307ebf48b63fe439c9d7e82e73d5024358be988c788f8592a2de -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingNoDockingOver f318c68f2ad9957bc0f3ec48cb4400a745cbfa2ef249eff4f3d0eff6793643ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingWithShift 8e59c3fecc4ec4542c279f3879a3feeb1f570ea8279ee3c2fc79c094ce0c6edd -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingAlwaysTabBar 5703e85f9c4f4eaa6141e39069d1a1209a5bdd500c6d8fe4450750d06c2ff419 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingTransparentPayload f74cd1d854f409c9a95d187dc2c923d88cef697387e37e6aee75030360cb4094 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoAutoMerge 9aa8d1c45e290c454b48346a7a3553e7370e602364f136fd09388da75e5d455f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoTaskBarIcon 9fe297987b87ef6cdc77bad0dcfff42d8b8e2be9578f08d4245a7cdd10cc891d -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoDecoration 28d1e27868a2ca78423cf1385ce95c67254cd2e7c96a1a93db9b23333ca5dd36 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoDefaultParent c0fd83931e50088bfc1b0fbae812d3438bd7a6ac606151a2cb655f347caefec4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsPlatformFocusSetsImGuiFocus c1d0fbdc87ad3915271c04584b8bc9adaba2fcba6ecb997c6dafe77ff60ed631 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDpiScaleFonts 9457bb733ef78418f485c4ebd2b1d02b8eff619e153ca25a2cff8d8479b2a23c -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDpiScaleViewports 70391b52c9f4b195571b7b259d96e3ca01f6ad57d97b3fbe99103592ababd076 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDrawCursor 66151e8038d5569d73e1cbf2167b2916491deacc74f85af5f6d7b1cc2802453c -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigMacOSXBehaviors 79337839a4736f86bc64126106ace89a70031f054349dd3579c872e5c05a293b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTrickleEventQueue 6de3adfa2df3f9fdf0410ef33e31446d11afcab0697121a79ad364048698e7a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTextCursorBlink 539d323e3a215dfb208d56aceb50e2220e40627b628d67e272bb35668d71b8f9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTextEnterKeepActive b8fe3d7da906306aadde88be3e6463522075816a882b2bbf151aaab4cae53c7e -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDragClickToInputText 986d722a4f2a64215f1445ebb3961e82ee3047b353bd429521a77b7846d781bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsResizeFromEdges ecd63759887d65003d0f0cc4d83eaf0531dac3e60bfc124979c911bdc9eadbab -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsMoveFromTitleBarOnly e403bc47734b7631caa4dc967dd00bda40c6f9f78f064a70dfa18b297676e096 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsCopyContentsWithCtrlC 8dcb59978119f4f87f5fdfc06d356ef233d89d607795944f00ae03f19ed14e77 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigScrollbarScrollByPage b098f66362dab21d2531e1fae9c15f36d6864add3de37156c55b7e8d22a2ad6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigMemoryCompactTimer 767f9b9d2ce97d6ab779b10c8f0ae2bfc786ed1ef819ad786f02faaf7508ab94 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClickTime cefda3a09bd66e9520afd7a304c011b510ebde956ec7e1d276752fa5c0fa4f41 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClickMaxDist 8ca53e8f59cc271afea294cab26d3e1a62e0db42f7d5f6507798d816310d3973 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDragThreshold af5fd8fa710f8b4a18c2f0b69cb80f6abf13c62cd2b041fc4fd211b724a896cc -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyRepeatDelay 87b0af59783dd64df62b145fb61843c9d8d454cf52453cb4c072aa9c77dc7cf6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyRepeatRate bd84338b821771ca1e8a5249f2d2bd4008d271db70d031a7a1fc0e152f053af1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecovery 806149cc4ea172c8b0e7488f8920284163cd82b8ded83982c3dffb9cca7dfab3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableAssert 81f93bd6d7aabd3988000249ec56942f62253d7197b0670f927c53be44a19b09 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableDebugLog 8208450f8fd9ba9fc7f1f31c7865c0b9fb8bd96fc8f0b0ff529ba658f8f4df7f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableTooltip fbf3a57925610a3934cffe507a49ea52db04f117e92d4d6be2472cd89bf3c3a9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIsDebuggerPresent da0d389da22bbe4bff81f20c55becdc511bd93834c091a3a0e00671098c818a1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugHighlightIdConflicts ab26a1e2588bb34a288cc938efdd15e24ce585d7be91c7656d695a62f810390f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugHighlightIdConflictsShowItemPicker 47b62d1768179313eccd406bbdd86e735ab9336a7fa2597fa8def021969e894f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugBeginReturnValueOnce b45830d0aa2139e063d3a100181db0dfb6ec8c6e10b9c1022a4285954ba06300 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugBeginReturnValueLoop 141012d1eef6eea45dc35265cd46107281ab047ebc21bac0ce3299bc0d898fd8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIgnoreFocusLoss 9959d323915605076c7ced92507ab1864656ad40bc85e4c60bc8c1defc78ee32 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIniSettings 7c16a8ac84df51a5ebd267f784432abb58f7b592a0d0a418cde95bc3fdd4e66a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendPlatformName e75656e76dbb8d4448505e2dc9fb8c761569c492f28a8410fc28083b22244684 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendRendererName db67957e1c8d7c15526741ddeae6e4e5d1c854878307362358d806dc9a2a13c8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendPlatformUserData 3627a6089bba2d9ccd275e15ddad04c6634c717764ed47c57cdb569013848391 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendRendererUserData 3708144a2cf366b1fc107411a2daa3ff20b8d366fbccc2e494f4f5b8b088bc0f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.BackendLanguageUserData f9f44ce711e68d4a426bed82a933f785eaa40177e9be2c09281626c29c37b508 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantCaptureMouse a7a32f07d034899576abb6ff146f304f023070e9994ba0a41b5204249ad9da2b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantCaptureKeyboard 01d1a284c67c5538f30bf5d20fa5d9cc1883e2263ac212933ab04d009d0bad76 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantTextInput 3e78ee009049c9032110200c992302fa61dfd9e2a31baa2cbf80c86889aa3785 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantSetMousePos 85f6d08936b3ccf761913758c5e9fa2b3cead2961be4debbaef6b31e1ac13b4f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantSaveIniSettings c16354a3dc61c52bad935f3e5065020651a35bcab458f200425bc8968c2d9cce -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.NavActive 6250a834e9de21fc152181c59d3b57cd4d7b0ead8fd8120276a78fa44e13ea52 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.NavVisible 8929a80c43efc6ec60382ceacc730b1773c768c5fbabc9a744814eb97012342e -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.Framerate bac59db8a732678b2ed2392cd80cd1d784781f0ab141bc678f03b32ae3be22d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderVertices 95e71d106ca5f8c433b32fd804e70d49d80f0b6feec2aad1d7ca4cc6306c51a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderIndices f52d84b983e812bc80d782a959cd12b95f87775c2e14cbbb43ce50365b9b18ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderWindows 52be3d9fb8b0856d52c8b651ba6357f0bbceb5897de1a215ca9164404fd3348b -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MetricsActiveWindows 39e1a224e1544836771cf6ddeec9d6dd76231297dc4f9abb9f99b7f4d82fc583 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDelta 75f56c1f69dec965b2938aed5c1a0c3053095958143fef2dec2f1a5bd83940b2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.Ctx bcab2ea4c5590a786d7f56e795a18d39860446efea8e8889be2fba7116f4643f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MousePos 4753262a5f27b978e9e67bf34f1cf3457a0d790d6b391ceb7088c6f599973a23 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDown b9018cae9d7f8202b316eb26e5135aad226c24b90ef03ccd43baaa3fcb6b3653 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseWheel 7f69cd5e878552b57ea1cf2168bbc7ebaa8d2dd1c41620878e41850a31b5de90 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseWheelH 345bed9b51cc58abb51e5aede4826d0f750b694a97f1c41a9c67654ad28fbce5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseSource d5ee2ca76f14c0cc957e241cb630532577be622321ecd45f7215d5bfb6f2d8f9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseHoveredViewport 6e243561e40c1cff0b886d1100e99afba89b5f0119be5d7bc854b9c0716161e3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyCtrl eef5d022a49da579471f446b934b94989ec615dcf65d24733b7fdff4d7f7a950 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyShift b083b65c75747ec064075b60b19178cf01e9c2c7da9cf5cefc4040947f918abe -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyAlt 10fd1b74825d915c943c6bae1b2d11c2121aa05088e89802dc50ee7ab70d80bd -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeySuper 10da129afab7b65e36a3ca9da29c948232ca96853993b7c8ee150997b1cab5d2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeyMods 78f115b6e50058f5975f1dd0c9866e8b62677ed50786993a199be709535f2f34 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.KeysData 01756a158eb68901e5a9808762cc820c043523207294c23ac5ac82333221c8d8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.WantCaptureMouseUnlessPopupClose 258fa49b23eaba2137b52d9b98b8522206bca4593afc1f28de4c793e4d6fd88f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MousePosPrev 602429ba82c8a863376ff9cb34531f87b4ff67cdd868a456183319300a52332c -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseClickedPos dad7ef1a34bba65ee8d0ef2a7cf5dd3964ed709371d7dd1395d0082bb553b6f1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseClickedTime 0c792f22ab56b4c94e78ffe0544d2f36e9bbe5031b79f3267f879b6b4ad64d77 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseClicked 828ccd2f7de687f2bbf88528b6c89498cb613dc1e53b1aec5c3fd254277bd3db -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClicked 32c2b235331fa5088a948944d2ed99179232b94e881da86bc45eeef572549e44 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseClickedCount 774f6b591829a0a5182f947dfd820269c9c137fd8661609e872d41c2305ee87e -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseClickedLastCount 799ef6d2b5200c9956b2e3a2b78605b8b2c239bd2914514821b477e6ab10bf2d -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseReleased 90dda245abcfabf3bfa97b24c86d9ccc8dd8010b70ba069fac350d10eb919eaa -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseReleasedTime 57d069a0333282b57d05a4899a69dbbbbc4c2a62aa9d55bb87404233217c2c3f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDownOwned e91cc5c3fa4fe267ddc3ff21415ce8e8e607831bde26380e57deb8e0a3b99bd7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDownOwnedUnlessPopupClose 896aa5867f9518890ed0f81322927a47cb32160afddf0c2bf1c5cf1b08397236 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseWheelRequestAxisSwap 845316b993d6aaae64e4dda5deb9e60bacaa7c8fda13d75f59aa85c50dceb2ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseCtrlLeftAsRightClick bf220ba4a0fe97aa42868bb7ffeda61c23668eea83ae6e9927a4054b5b879525 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDownDuration 9fa403034197b42837e7e6ad0fb51edcf1126862a5cc737ef8fcb6ec2fe114d6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDownDurationPrev 6985c7971eb3a4b27747388ac8f53ca4cd5aa16677578b57bd01ebbb4a413c6a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDragMaxDistanceAbs 51a62e129b0c2dc6cc30cdcea7cf388687494e99483533f4c7701e09d2d6d2a1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.MouseDragMaxDistanceSqr e889ad0f2c0d9ac5df20c9b9b245e1a04c26221becaac070501bb30959ec1277 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.PenPressure c964e4399257ec04a102a7eb982c143255053480c04a465228451bd924bea623 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.AppFocusLost 3a25152ef4cc0b87b47a7c72da07d99d9256bfb06e10eb6b5e729e758572306a -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.AppAcceptingEvents c0dc987a88b669b431ab944a14dc14a268ee5796dba19d2d284ae9bea0917024 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.InputQueueSurrogate ea5564d2d6db8f577ed0e98cb34e29fe922565ec165af406828b73517176f74e -->
<!-- api: P:Inno.Native.ImGui.ImGuiIOPtr.InputQueueCharacters 5f8dcec95f87d74709e7b5635739aa3f201ceae75e4d375eb1032e117b26a13a -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddKeyEvent(Inno.Native.ImGui.ImGuiKey,System.Boolean) 7a218b6bbf93f12ffa6e542c226273299531d4edc170c907217584ff64fa70a3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddKeyAnalogEvent(Inno.Native.ImGui.ImGuiKey,System.Boolean,System.Single) 605ef46346e3202f4b3f8c55b1327edd46d5bffd8922f4ef7419d3bcd0693e21 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddMousePosEvent(System.Single,System.Single) 1c6fecf19966baf0e3955ba24dda5b991c0716f55bf1d15040ecda55cd26a704 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddMouseButtonEvent(System.Int32,System.Boolean) d569d716c71eb6bbac2bddd8179317bc6cc0d5df1954ecadf82dc8e935f1b53f -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddMouseWheelEvent(System.Single,System.Single) b9a1e9ed95f3445c019ee970b65bc9dcaf846c45fd64888769ab4cad15249a04 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddMouseSourceEvent(Inno.Native.ImGui.ImGuiMouseSource) 1d74de21b1ecd34ddede89a06d670ddfacdf10da732f25bed122e5477f0bedc0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddMouseViewportEvent(System.UInt32) a95c820ae6394a35378062ad30576a75e9bc6c5393d84726328f1aae0062b8e7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddFocusEvent(System.Boolean) eb67e9062efc9f293a783fa9e6ede15571f13e88e8000d72b2246bf3b2b8e447 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddInputCharacter(System.UInt32) 30ab69a9e3333b9461ed8e158a6198c58e5d2ebf3d609ee02edbf8c042edcf27 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddInputCharacterUTF16(System.UInt16) 69671a04a4f93ce4f0ad1573dad5d691306b96cff441aa6e168d0046718288d5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.AddInputCharactersUTF8(System.String) 17977e16d000f4a714f184a202f68185ef466122dede86d428edba7097b27484 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey,System.Int32,System.Int32,System.Int32) f3f671a1e975f2a7ed1d06e027256edcc0a123ffb9294738a6bbfb065030a1e7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey,System.Int32,System.Int32) 846bb75cb5107c568a01dbc4e811bcd823a8459b86db7afd7ace7f4ac12dea67 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.SetAppAcceptingEvents(System.Boolean) 21dca1577459989c6241984e3feb39431fc162170f1a49f552525a81f7bf0f46 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.ClearEventsQueue be7d1372c0ffe63e0f99c71ef222156a3297a4b486b262e58af482e6e329e317 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.ClearInputKeys 805e116571cbd9c859531fd37c2be0a468ca76eb7fa2ec527e06600b3e2da038 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.ClearInputMouse 26ab84145cc008384d13093e25831476eaf5c4d3279a918d752389ceee068126 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIOPtr.Destroy 5e6c3b45b3f98dd708b18ef44f8d30f5a3a472f8807daa9a47bb3c32eff76e9f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-943696bc9f49dad8"></a>[`public Inno.Native.ImGui.ImGuiIO* Inno.Native.ImGui.ImGuiIOPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-770c9b24d56eb60d"></a>[`public Inno.Native.ImGui.ImGuiIOPtr.ImGuiIOPtr(Inno.Native.ImGui.ImGuiIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14167) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6f44742ef557f8b9"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddFocusEvent(bool focused)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14358) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f40b0526122808b"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddInputCharacter(uint c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14366) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe10a81d4aeb5437"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddInputCharactersUTF8(string str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14382) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2abf48f36d2eedf8"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddInputCharacterUTF16(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14374) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b53f689725de7c14"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddKeyAnalogEvent(Inno.Native.ImGui.ImGuiKey key, bool down, float v)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14310) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00b59964c13a355c"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddKeyEvent(Inno.Native.ImGui.ImGuiKey key, bool down)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14302) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6be49f79407ce82"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddMouseButtonEvent(int button, bool down)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f4de3200cbda7b7"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddMousePosEvent(float x, float y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14318) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ebb24531807a7dd"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddMouseSourceEvent(Inno.Native.ImGui.ImGuiMouseSource source)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14342) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e2e6e0c8c55df89"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddMouseViewportEvent(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14350) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-828607939805395b"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.AddMouseWheelEvent(float wheelX, float wheelY)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7deff9b82eaf80ae"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.ClearEventsQueue()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14414) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-936487481f404294"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.ClearInputKeys()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14422) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-94c2246f8232b0d7"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.ClearInputMouse()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14430) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-340b763199f1a18c"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef9490f3338486b6"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.Equals(Inno.Native.ImGui.ImGuiIOPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14182) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1478bf877f455628"></a>[`public override bool Inno.Native.ImGui.ImGuiIOPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14183) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-dadaa280b6658de3"></a>[`public override int Inno.Native.ImGui.ImGuiIOPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14184) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-92bc2e713becefd6"></a>[`public static bool Inno.Native.ImGui.ImGuiIOPtr.operator ==(Inno.Native.ImGui.ImGuiIOPtr left, Inno.Native.ImGui.ImGuiIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14180) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d7963ac5501c05d"></a>[`public static Inno.Native.ImGui.ImGuiIOPtr.implicit operator Inno.Native.ImGui.ImGuiIOPtr(Inno.Native.ImGui.ImGuiIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14178) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9547451ffef5278"></a>[`public static Inno.Native.ImGui.ImGuiIOPtr.implicit operator Inno.Native.ImGui.ImGuiIO*(Inno.Native.ImGui.ImGuiIOPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14179) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e176db022a514d41"></a>[`public static bool Inno.Native.ImGui.ImGuiIOPtr.operator !=(Inno.Native.ImGui.ImGuiIOPtr left, Inno.Native.ImGui.ImGuiIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a8c8c57eb18a12e"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.SetAppAcceptingEvents(bool acceptingEvents)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14406) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a77c56afc83dd9ff"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey key, int nativeKeycode, int nativeScancode, int nativeLegacyIndex)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14390) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5617d0a31b5ba31"></a>[`public void Inno.Native.ImGui.ImGuiIOPtr.SetKeyEventNativeData(Inno.Native.ImGui.ImGuiKey key, int nativeKeycode, int nativeScancode)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e6489403d28fecc"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.AppAcceptingEvents { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14295) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ddc37cd55d222fd"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.AppFocusLost { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14294) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f8fd70404f1f11b3"></a>[`public ref Inno.Native.ImGui.ImGuiBackendFlags Inno.Native.ImGui.ImGuiIOPtr.BackendFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14187) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-161974e94a1dd3ae"></a>[`public void* Inno.Native.ImGui.ImGuiIOPtr.BackendLanguageUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14248) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-43404f94502e3869"></a>[`public byte* Inno.Native.ImGui.ImGuiIOPtr.BackendPlatformName { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14244) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ccc1317b1bfc90ea"></a>[`public void* Inno.Native.ImGui.ImGuiIOPtr.BackendPlatformUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14246) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba429674cd8d6ce9"></a>[`public byte* Inno.Native.ImGui.ImGuiIOPtr.BackendRendererName { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14245) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-847b6381ed37ea58"></a>[`public void* Inno.Native.ImGui.ImGuiIOPtr.BackendRendererUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14247) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-322b65a13c6a9e57"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugBeginReturnValueLoop { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14241) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7e221214906ec841"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugBeginReturnValueOnce { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14240) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1987994e511cab9"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugHighlightIdConflicts { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14238) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7e8f3f0fafa6d0cf"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugHighlightIdConflictsShowItemPicker { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14239) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a242bb6c066e1b92"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIgnoreFocusLoss { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e7663107bae3bc6"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIniSettings { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14243) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5636d604a5b4578"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDebugIsDebuggerPresent { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14237) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef5b4bdc69c38ae0"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingAlwaysTabBar { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14208) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4a1b6c3a4ad7e3e7"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingNoDockingOver { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14206) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e45f53ab5182a88c"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingNoSplit { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14205) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08e6843faad6bfa0"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingTransparentPayload { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14209) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ec65d5428866be3"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDockingWithShift { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14207) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fdbb2493c9d2ba34"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDpiScaleFonts { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14215) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28d8a1ad959b1151"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDpiScaleViewports { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14216) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1cf7d755dd2192b"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigDragClickToInputText { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14222) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3be9f6a10bae8505"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecovery { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14233) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93bd971147a92c85"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableAssert { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14234) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cff5743a5cb4745f"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableDebugLog { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14235) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9e649af2127ca688"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigErrorRecoveryEnableTooltip { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14236) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-927317860a2f99f7"></a>[`public ref Inno.Native.ImGui.ImGuiConfigFlags Inno.Native.ImGui.ImGuiIOPtr.ConfigFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14186) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-83e6d13e5960cf7f"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTextCursorBlink { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d80bcc4cde926f10"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTextEnterKeepActive { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14221) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d934e618e4549490"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigInputTrickleEventQueue { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14219) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c1e2b245ae532588"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigMacOSXBehaviors { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14218) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1061ea45aef760e9"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.ConfigMemoryCompactTimer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14227) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b22c4deac95abea"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCaptureKeyboard { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14200) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a10ac96d1139d05"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCursorVisibleAlways { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14204) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38a9ed43d244384a"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavCursorVisibleAuto { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14203) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e273b82b3f7c903"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavEscapeClearFocusItem { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14201) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7fd4cdd755e3d22e"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavEscapeClearFocusWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14202) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f11eea672cf350db"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavMoveSetMousePos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5eec3582317aab3f"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigNavSwapGamepadButtons { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14198) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bfb8f1722221e803"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigScrollbarScrollByPage { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14226) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3772261f74644939"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoAutoMerge { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14210) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d8777803ef788b9"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoDecoration { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14212) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb6be34e5bc5e58b"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoDefaultParent { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14213) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1085e3a14862e573"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsNoTaskBarIcon { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14211) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0acac0f3c3d325bb"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigViewportsPlatformFocusSetsImGuiFocus { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14214) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1a2af6b5360bf2f2"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsCopyContentsWithCtrlC { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14225) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2945acce37894873"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsMoveFromTitleBarOnly { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14224) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-099ccbd6efeb55de"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.ConfigWindowsResizeFromEdges { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14223) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b0c9a291ff0f934"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImGuiIOPtr.Ctx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14262) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-02b6a83b2f3b52de"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.DeltaTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14190) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-75892730496641c2"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIOPtr.DisplayFramebufferScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14189) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d857e23848a8d1f2"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIOPtr.DisplaySize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14188) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d76e813d803e637a"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.FontAllowUserScaling { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14197) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-065bfc54facfb7f7"></a>[`public ref Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImGuiIOPtr.FontDefault { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14196) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aab1540a77900291"></a>[`public ref Inno.Native.ImGui.ImFontAtlasPtr Inno.Native.ImGui.ImGuiIOPtr.Fonts { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14195) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-61e83b6ffc6b906b"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.Framerate { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14256) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34bb265576705b1c"></a>[`public byte* Inno.Native.ImGui.ImGuiIOPtr.IniFilename { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14192) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6478fb91a2ca42c6"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.IniSavingRate { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14191) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-73e8714eae5b4bdc"></a>[`public ref Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImGuiIOPtr.InputQueueCharacters { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14297) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d96adb9a8480514"></a>[`public ref ushort Inno.Native.ImGui.ImGuiIOPtr.InputQueueSurrogate { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14296) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-524e773570a8195c"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14173) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-430c9307ce04535b"></a>[`public Inno.Native.ImGui.ImGuiIO Inno.Native.ImGui.ImGuiIOPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14176) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5d36f165faafb703"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.KeyAlt { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14271) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-96b104ce127230d1"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.KeyCtrl { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14269) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0173936ef79b95e7"></a>[`public ref int Inno.Native.ImGui.ImGuiIOPtr.KeyMods { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14273) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fc4bfebcc9015f1"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.KeyRepeatDelay { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14231) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-539192776f0adb42"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.KeyRepeatRate { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14232) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a48460c0e7e710a2"></a>[`public System.Span<Inno.Native.ImGui.ImGuiKeyData> Inno.Native.ImGui.ImGuiIOPtr.KeysData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14274) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0708cede6ec195da"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.KeyShift { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14270) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ce530d4acc63ffc"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.KeySuper { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14272) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7ee7f1c064f1002"></a>[`public byte* Inno.Native.ImGui.ImGuiIOPtr.LogFilename { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14193) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-53f278ede4653bdf"></a>[`public ref int Inno.Native.ImGui.ImGuiIOPtr.MetricsActiveWindows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14260) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da1c2cfa4aa2c406"></a>[`public ref int Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderIndices { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14258) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f72ddf300c4d8ac6"></a>[`public ref int Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderVertices { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14257) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-89eae331403d0319"></a>[`public ref int Inno.Native.ImGui.ImGuiIOPtr.MetricsRenderWindows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14259) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90ed4505e178c209"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseClicked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14279) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19358babc8b4d842"></a>[`public System.Span<ushort> Inno.Native.ImGui.ImGuiIOPtr.MouseClickedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14281) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc0c37c52bb02143"></a>[`public System.Span<ushort> Inno.Native.ImGui.ImGuiIOPtr.MouseClickedLastCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14282) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ae916068a6a3130"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiIOPtr.MouseClickedPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14277) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-001881fb5fc9274e"></a>[`public System.Span<double> Inno.Native.ImGui.ImGuiIOPtr.MouseClickedTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14278) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-adb112f23b83a07a"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.MouseCtrlLeftAsRightClick { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14288) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5abdee13271c7422"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIOPtr.MouseDelta { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14261) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9151984f6639db77"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClicked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14280) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3371758f3475511a"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClickMaxDist { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14229) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8965916e919c077"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.MouseDoubleClickTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14228) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6554cf828394ff8b"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseDown { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14264) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-142131dfd7262a8a"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIOPtr.MouseDownDuration { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14289) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-318cf0c1c2335b7f"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIOPtr.MouseDownDurationPrev { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e36b0cf8220cc53"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseDownOwned { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14285) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42ba00b5e19a4810"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseDownOwnedUnlessPopupClose { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14286) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a839aac58090f04d"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiIOPtr.MouseDragMaxDistanceAbs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fba27016abf301c6"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiIOPtr.MouseDragMaxDistanceSqr { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14292) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9117cb5b9e276cfb"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.MouseDragThreshold { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14230) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8c28c4c8cd97cad0"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.MouseDrawCursor { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14217) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-761b3b9fa192b2c1"></a>[`public ref uint Inno.Native.ImGui.ImGuiIOPtr.MouseHoveredViewport { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14268) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e4e9d9a1ffb2350c"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIOPtr.MousePos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14263) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5aeb95480677319"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiIOPtr.MousePosPrev { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14276) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-33c5659b3def910a"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiIOPtr.MouseReleased { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14283) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a6f750fbd3949b0"></a>[`public System.Span<double> Inno.Native.ImGui.ImGuiIOPtr.MouseReleasedTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14284) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-50e8f6681eb80464"></a>[`public ref Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiIOPtr.MouseSource { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14267) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d45ada912e26f3a6"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.MouseWheel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14265) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9427b781ec463217"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.MouseWheelH { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14266) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7be26a74bc35200"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.MouseWheelRequestAxisSwap { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14287) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5bfc8d2942fa9ec"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.NavActive { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fa3b1e6c6fc6778"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.NavVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14255) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ebaec920badc8fe0"></a>[`public static Inno.Native.ImGui.ImGuiIOPtr Inno.Native.ImGui.ImGuiIOPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14174) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5326939f5687717"></a>[`public ref float Inno.Native.ImGui.ImGuiIOPtr.PenPressure { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14293) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45f82554c7adfe79"></a>[`public void* Inno.Native.ImGui.ImGuiIOPtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14194) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1218e946233de944"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantCaptureKeyboard { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14250) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6f1c77ad09dbab5"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantCaptureMouse { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14249) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fbf941914deabdb9"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantCaptureMouseUnlessPopupClose { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14275) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ce8f639bf1315e8f"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantSaveIniSettings { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14253) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc1197f838c224ff"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantSetMousePos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14252) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b332e7c309522c2"></a>[`public bool Inno.Native.ImGui.ImGuiIOPtr.WantTextInput { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14251) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d12942df998c5036"></a>[`public struct Inno.Native.ImGui.ImGuiIOPtr : System.IEquatable<Inno.Native.ImGui.ImGuiIOPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14165) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiItemFlags</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiItemFlags dce2c5d5d9e7c2f4c321e9f7e9e7e5d28551b82554ef3450a3721d25c02aaf29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.None c28fdf200a21a53a91afcc3b595353de63fe1a337247a4d8fbd7c44693ace665 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoTabStop 46cbd523d6e3c8dff843ed8a5137e442789afa74d2507430e6d6df9f6d7b861f -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoNav 6d291fb200e3c7f5918cf9e2557da21c5aaf35ada653a9610f90088c1a4b7a5e -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoNavDefaultFocus e4bcbb27118bfdb349eb9ef96c4a308581bbaad677060f5c55e0fba5ef53adf9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.ButtonRepeat c8f47729215ab1688c4aaa353eb31abe2da2d1b1a016ccdd00eca310d8f5e3a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.AutoClosePopups 3545cb7b93ac704f3120dd1a5e0097b08fdabfb81162d3fc8d039ac3a763795c -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.AllowDuplicateId f7cf4498873d673845f58e791b5536bcc80b2684bd3b072e90107e1683085fca -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.Disabled fe216da5787ba8f8c35db5fd89f52169d550db019769746cea063e69b676bcbd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd8f596a6217c5cb"></a>[`Inno.Native.ImGui.ImGuiItemFlags.AllowDuplicateId = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L441) | 当前常量或枚举值：`32`。 | — |
| <a id="api-6f9e7f588f320d50"></a>[`Inno.Native.ImGui.ImGuiItemFlags.AutoClosePopups = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L440) | 当前常量或枚举值：`16`。 | — |
| <a id="api-426adf5d44534e32"></a>[`Inno.Native.ImGui.ImGuiItemFlags.ButtonRepeat = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L439) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5d03c92896ddcc55"></a>[`Inno.Native.ImGui.ImGuiItemFlags.Disabled = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L442) | 当前常量或枚举值：`64`。 | — |
| <a id="api-59bda85d2cbb73f3"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoNav = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L437) | 当前常量或枚举值：`2`。 | — |
| <a id="api-9e20e19ff412ee61"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoNavDefaultFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L438) | 当前常量或枚举值：`4`。 | — |
| <a id="api-cf78b6930cd62118"></a>[`Inno.Native.ImGui.ImGuiItemFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L435) | 当前常量或枚举值：`0`。 | — |
| <a id="api-dde2588c917ae1ca"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoTabStop = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L436) | 当前常量或枚举值：`1`。 | — |
| <a id="api-31123de16428e53d"></a>[`public enum Inno.Native.ImGui.ImGuiItemFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L433) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiItemFlagsPrivate</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiItemFlagsPrivate ee23725d1bd2ae39a22dc3f0bf967aaa9be249f447857d1725475b281f0da567 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.ReadOnly a662a934b36d956f654cac4a53dca4a4e93f9464883d080319a7c0d1c5bd6d31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.MixedValue a1c07b6bace148be2eec5330bb176ca356f42f3aa793be8d6b3f44ec207eee47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoWindowHoverableCheck 06d689b7f79093addc13d888b10400c82eabdce9ad56c4415b989c7e5a229be7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.AllowOverlap 41a1abcd7a2092aac0d70a3cf0957055a023580f4acfacccd1fbf9e6298f263b -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoNavDisableMouseHover d01529a621b28b6420a6233ad8c5239ed5aa8b6b0f93d6ed077d9263e61af8a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoMarkEdited f8371bffabdc35faf5b35f027554c3a6e9e1f1851352f6144934b8894df42130 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoFocus 93b4b36294c1e5ac8f52db5d22098b4b8ea03989e49a680d17190afea2afbc88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.Inputable b11737fcfba3702e4d8057f610e14d22efa498f4205059e889d4bab3cfe9cbf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.HasSelectionUserData 4ce711ad3956932c5404f5bb9782775a1542ed14ed485d94c71bf8de6409b783 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.IsMultiSelect 08bd515d9ad14d37aef8d421bfcc8dcf3d4681588172bdafa48457f72fc02635 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.Default c307b0abf7210d71c14d90bd351079e1dbd87537bd6df7ce28a72715d77242d7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1bef239ee3de4c73"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.AllowOverlap = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1226) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-ce4248f7f11eb108"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.Default = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1233) | 当前常量或枚举值：`16`。 | — |
| <a id="api-3782ad711838e1c7"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.HasSelectionUserData = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1231) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-e50282328a6b8ac1"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.Inputable = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1230) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-38648899c15bb3d5"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.IsMultiSelect = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1232) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-3f038318182624c9"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.MixedValue = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1224) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-5900c743dd53f96b"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoFocus = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1229) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-08f9a44837a6c13d"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoMarkEdited = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1228) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-0872ba6344aa0ed4"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoNavDisableMouseHover = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1227) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-01703648e58d58bc"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoWindowHoverableCheck = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1225) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-9930d7a1359e2656"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.ReadOnly = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1223) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-3c96bb1d2c75647a"></a>[`public enum Inno.Native.ImGui.ImGuiItemFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1221) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiItemStatusFlags</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiItemStatusFlags 4773059f0b3c5b5170f27b4a8821e3068a29b3bf5bdb368065bedb3c307ab76f -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.None a68ce50f8fe5d2a477d01d15e11effb06878f2d05253478aa172973394e534f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredRect 546545768099a88f4ab87cf146eac433ab2289a99796239cf755636eb3c15eb9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasDisplayRect b353826eeca527d33ddc03b55038c3298e573621b69993ee5db597307d988844 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Edited ac7fef635a7e4c5e0713a3e61fae8b84c76553d7b70b331086c11db5ae802f6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledSelection 5dfb81b5e56c21399501abf60cd97af1c66236bfb46fccf52a3353c9fdb0e04f -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledOpen 07df7f227bbfe27557cbe1ac096c21a8bd0553da752880ab5dd5c04375142f0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasDeactivated fb654fe396e2c50cebb96a0d18458695cfb48d2f7ac35529e7453b0ccb3d064b -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Deactivated 6f55bdcaeac5cafc0149acd3b357318486ba1a664aece960c761bad1b1c58152 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredWindow c6581d2eceb5c1cb3b5d31c8b9d840201bc16eed5cacfe3d31ae23ed4e878696 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Visible f1b12d964625a4140dfc1bf80e6087fbf835a78cafd7020ca327185de7bc3427 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasClipRect c7e698c3f7b51229c0fefe79c90051455d75b4ccedbd17e2fea1dd3b93b013cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasShortcut b2995824d3448932dd744e0a391610e0cd9962172c814b1461253ddcaf663cb0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e25e947a1fceadf4"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Deactivated = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1248) | 当前常量或枚举值：`64`。 | — |
| <a id="api-12cbb2baf4b2580a"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Edited = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1244) | 当前常量或枚举值：`4`。 | — |
| <a id="api-dad0c1570c0495a8"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasClipRect = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1251) | 当前常量或枚举值：`512`。 | — |
| <a id="api-d53323e2d841e0de"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasDeactivated = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1247) | 当前常量或枚举值：`32`。 | — |
| <a id="api-e944b45274a2af0c"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasDisplayRect = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1243) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ee6dc035e8a80289"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasShortcut = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1252) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-9e35ba5187d18fe8"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredRect = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1242) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1d54a3ff2a76c432"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredWindow = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1249) | 当前常量或枚举值：`128`。 | — |
| <a id="api-5c9211fc2c2e96e5"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1241) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0eec2aa794300dcd"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledOpen = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1246) | 当前常量或枚举值：`16`。 | — |
| <a id="api-9a7e18378f5235d1"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledSelection = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1245) | 当前常量或枚举值：`8`。 | — |
| <a id="api-c046f864b71e8392"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Visible = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1250) | 当前常量或枚举值：`256`。 | — |
| <a id="api-01e42b21190505ac"></a>[`public enum Inno.Native.ImGui.ImGuiItemStatusFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1239) | Native 取值集合；当前取值见本类型成员。 | — |

</details>
