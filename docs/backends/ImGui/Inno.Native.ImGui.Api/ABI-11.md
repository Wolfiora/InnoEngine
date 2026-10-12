# Inno.Native.ImGui ABI · ImGuiContext → ImGuiContext

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiContext</code> · 1–180</summary>

<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveId 1ffe6f35f855957303f8ef48c81d3560c937878238a4e05c6ce41880f89fe50f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdAllowOverlap b80e0a792e84f46492ae3d0d949dec20c62c9593bf03af4fc8aea1e4ec3a2bc2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdClickOffset 8cd2786f2053503485e30bd0c8d167853ffefb8f5c938ed9692a46e578f4cb8c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdDisabledId 0cb0992f2e47c9c752d3356446373df7980b0e4c9365f714a43d39c585fdd951 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdFromShortcut 7faad8030d99618fb5ca7946a2594af8d200f0df0af2ea196197434765a9b600 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenEditedBefore ddc3fafed65638b1b45e7c728a820b04ca1446909afa18c16be3c67235e60364 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenEditedThisFrame bfb167e07210e021622a250ab0a5985e78a302c2870663e1d05a5f9d045df319 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenPressedBefore 78ae6113dc652083e1560bd9e7c7ce3cbbb835b24ab319584c52dfc5d3ddd209 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdIsAlive 0a2521b2389835434f60c221999c5c947df703df410f3c54f120c6e42d806cf7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdIsJustActivated 0349b09eb042df0ad6d3e32eeca34da673d7b6d656b1accdfb0e330d42d370fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdMouseButton 18e937bb0518c42e704726ef65b3f7ca22e8010c05c73d298d5a80d7b4f61429 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdNoClearOnFocusLoss 65a5200f0a67c77473d1148c2b2ae836832ec780276c02ae37811c9ca8b5ac8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdPreviousFrame 34ce6beb923c3b840b4ac617a8a3c8f08049274b6def44d14826afcbb55da6ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdSource dd1a4fde614a0430b766cf21c88c0bd8530d3d2109068ea2e9e79911a5303378 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdTimer 451d115e9e6df1db1deb41fc9081a1a15a2e0cf4737a8d05bf1dc759907bb494 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdUsingAllKeyboardKeys 6be573d49f5b9c85f9237fb5b5f9f9bd3e83a0c4465e6165dbfd17b13885fbc9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdUsingNavDirMask 34bae4669b31b54b77beff3c2b7f3eb6bf28f1fd46ef1932a0b7b6ca4d0830e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdValueOnActivation 941d72c3df08b3f27bf1f6899ca4f734aa12ab45aa834c14339b4643e58c6a8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ActiveIdWindow c56d625c99288fb44b8567461ef03466b828e65a040ebb6067af93d158d386ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.BeginComboDepth 851d7137081345dfe7dcee7ca62636788655eac549fc61a938cf62a690826e25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.BeginMenuDepth 4a49fd7f4452626dec4abe6ad336b75c6c7b969624ab0b37470caece772d13e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.BeginPopupStack 69f897859f5767589fcaf57be6768f2d064abca25170d2efa8e6f6827e2cd617 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.BoxSelectState ba4e50d194bb40c0882dfa0e4fbed6f4ce18301a53bd6c3786ef1a6274f07f93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ClipboardHandlerData 64c1cc914288bf238dffceb142842d067daf05d493e61f860ee86fab730c30c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ClipperTempData b010434f38abc3c32b728934ac0c9563393c1223172e29603a87fd6724895ee6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ClipperTempDataStacked 3334f374d3b37386f8067b099d9a0242ea647ee6056ce46108bf112198c4e642 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditCurrentID 7803a41737126bdfb70ff336c0f5dbb7ea5dac234f32e7312e83efe93f33e421 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditOptions 566066b26143366a5be5e5a42f620a918fe6af8735a8c38bfc2f3a893eb73f8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditSavedColor 3de2e1a9ac9f12434123fa897cff7ed8917206af525b189ab52b4ce6ba98451f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditSavedHue e89bd65ae2e0b4b888fb3c1cf8721e73a4badbffc096e30f225fb7e6d8770b77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditSavedID c7a74a7af02e096d3682f008c300515e8f67dceef317652caf54d399e6ae2134 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorEditSavedSat 76a2a9869f774ecb838e2e0ff8042816db9fbaee0c35661ea71c2f8522e3a031 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorPickerRef e8a93a80996e58e28304951b4b8532b85568f26f62b008991f945a916e6adfd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ColorStack bb15578c1f3d35d1ebfa877ab4fc907e1a4702c0939ab9f807f9e20f584b4190 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ComboPreviewData 6593fb47103b63ffa6e1a7b497b113518bf992d5c55e83c14c7764ec89fdc0c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigFlagsCurrFrame eee4c26ac0971bd39d20dfdc3f8879ca0c8d2062c4106c3c051a01501ab2bbd5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigFlagsLastFrame 95456aeeea89cd5cfba30f59cb7de9cf22185809f39189818ef2815ea48420b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigNavEnableTabbing ed7bef382a24fe8c61a659e18bdd250f489b2c4d8a6975647967b842c354bbaf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingKeyNext 0173afc4cec7abfce8ff7f93f2d1e7ac6255824c16039380a691515e3ba55422 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingKeyPrev 33d8f08df0fd61d9c36b4615e8ce28f07edffa5470524c3c277304acdc58abca -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingWithGamepad 0bcee41aaf8f4882eb213ac4b6d3bd2fe3e458ac93208953377f4982bb69e858 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_0 70621a064ae272af7ee99c27e9228f4a9e9191bfe884db68f5341a0b614d9b02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_1 10ad9da2fd5fca59f7be4d860a645c79f027ad178727bb99e0fd3448a48d328e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_10 2e27bed0792d27bb10562525d4490ac818de1ec06a1646b15b6c4c38579f3e01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_11 421e7d5e19bf2ae21d30aabf54d7c319886f79277c64baaa3e54688803389b31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_12 06f8a9d8fe3cf4c28f19460e6e8be82c8406e578670ff24c39fe38804152fd34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_13 d690d1e75129270d3d79c1c5f1255d520b4d79980c3575828ab5e9fb8e17b619 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_14 4893b4633b9c07683b4584700077c09db3660386c7b532ae2a4d826ab3c1518d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_15 c96a7d0da2a83a68704041359a7331f959739f7a369bcddbf8180a3eef5be9b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_2 0c1333d1f236706682305f0e034f99cdfc57c94068250e4299f1af31e3470afc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_3 835d031d397fa71a91c8b7ea311f5bb5e95283447ab36be6d903f85f172b9d20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_4 01f7ac8da9143ad30329a772d5f39aba30642a4759a3a5bfa3f1982d4e79b3a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_5 c1e781f1254a3019dcb12ec4481fe80457c3a8afe99cbe5c9f5f5b2bffd3b30e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_6 bf91c4d4290f62948a129309564efe382cc4eb3d9114356ace2aa02ef408d24d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_7 7fb48a05c9fc0fe11419e84a52684b07c837f9801f6443327f413775817f27c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_8 d6b670f9bfdc1975c862b3d76ed312cdf0c3556c116155346aaa06337f846b61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ContextName_9 eeeaff03c88745a9edc76acb09f1535044b4b8fef9f03bf7214785f7c890257a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentDpiScale 08641b02d6f73fd11abe991a07a74fe7a1830bd9cabdbf695ed72e69070151ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentFocusScopeId de38a8521e6aac1cb76611274794bd176e40e18317ad08d704c901b7d2072556 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentItemFlags ef90db29de2a3b8ac402b9cd9e81c70ff14ef7aebf0f12744f42d239c859f9b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentMultiSelect 6bc158d038c98d308c6e45813a8bdef4aca137afa7be6df19b9007179cda242d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentTabBar f64150db02cf60d3bb2c7bb3363f8e3951718e5b45f31c99675790faf17916e2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentTabBarStack 7c1f9f61c9a387f86d13beb540d5eb501ec567d36c194f1b9a0b9e7bcda38272 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentTable bd1bb31b4abf272341f73cc011f36332704a9601b76399258b0fde968765cafa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentViewport 38dab5573c28d5172e6c87e95436b363b178b55df2804b109379f10a2eefd542 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentWindow 1e5097ec024b449b9edbc713d4e3c45a0c376d79c868fe5b0e17d53d96f9344c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.CurrentWindowStack 596ede3e40e5dcd71cb22f55d36716d697699d77ea26d875e1784f9757e3cb6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DataTypeZeroValue 297694ba7fb87fc330dedf93de757acb0666fd91a625aa05b2b5c920f58e495c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DeactivatedItemData d5756bfdee1882c542810aabd3655be68b1fb8acc86af1d5a952a87672290231 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugAllocInfo 3ce3a3f08ca05370a25765c309f81299104d71cb702dce4513e135fb52913859 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBeginReturnValueCullDepth 48fdfe001e6186a56265fb2c6691636efb9df85de51adb32b2aaab26578e8b73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBreakInLocateId 0e2979f83bad8fdfce1b9e2912add4f0e0a10e6264e0f04943b0fa882281dd50 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBreakInShortcutRouting 20101343b710ffcc720b08d674127612505f5d721a4c5e95eefd79daac718bcd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBreakInTable 39cc0a25c058ceaf3af8207fa22ee6ac23814828ceca1937d3be7c997286e4a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBreakInWindow 4e87ab46ea55cec97eae85e5bae29f55ef96dc6dc30de2d51d06d38e0a392f99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugBreakKeyChord 3f82ab753d4911cbd543b32b6a9365a1d3debbf1c9d37b3cc972a43aafcf47ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugDrawIdConflictsCount d0cefd8817c8058456c67ccde8bc9271402ee173f9c074abc33719d4332fd30c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugDrawIdConflictsId 71107e29cd864ce3ced91ec0e033a8abd6e441076dec6619315979c4837f23f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorBackup b285f4712420c814b4f39e66b05f3e399c9b2110c01d686e650f16069d43bbdb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorIdx 4dd6409b1ebdc4ebef53aa797236102a555a282ca7fd10e8594d39efa0f8f71e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorTime 25e38e5c8797a0bb30e90bffcc2be4663231edbc27512228ad27a3cd65f9a536 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugHookIdInfoId 3cd9011a166fac40f36b5b1bc363d136995c9f816049e7c0221329ef337aa749 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugHoveredDockNode beb00f5cc0291fd06d0e49a68bf20249f47e35db3bea168602e47b71aa9696a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugIDStackTool cea84394ab0eb98bbe318f0b02387254b732fe2e4050b603541f49c104a99007 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugItemPathQuery a5b6e573beb5b442cf94b93666d437413eec8ae2015a49f3f23ea3e292223f27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugItemPickerActive 3980e89ec9f31f19990a3bc6e2a3ec61cedc30c79b19996ad0e4b1ace0a22753 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugItemPickerBreakId b8ba558e5b1346b00a09ac2f9c90f488b0937e8564ca0e7950450357c4b818db -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugItemPickerMouseButton fedb2deff7e3d8a9deb1ed92f0e82b862238640fa414c1480ebf94570450a6c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLocateFrames 29398c4f5cbb0a39ae733539e4c3f5530f0292e66047d599a212cb0fcbe1e56b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLocateId 451bc445d1e220d067e65212b81a8fd5ff337a6a7cb1c9431f8cbe1f5ee506a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogAutoDisableFlags 8f13abcbba72561145570e5ab717af771e8e27dd45a13b9c98ac553c9bece26e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogAutoDisableFrames f957a479b0e2dcf0b10be710398a38f90c91e74b3e14d8859df7f73e56296d2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogBuf 9193be6f58e73dbc8d31ac694fa5d8a6b47113892bed88e2f099565d6400eede -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogFlags 209737f17ca4ae3e425319e10d874aa15c7193b8486da033903dcb26224dc7c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogIndex 4e97e5510f028b0a31cf23b901faae146da097dbe0e8d1aa357dfa8f1070c55b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugLogSkippedErrors 130ed0101a8cc12e0c951cd257a54c4b2028adea600c3f5c125f29e7376b7919 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugMetricsConfig 0bcab775bd23454c4710b787753173710416d8ad87c6c20a84782611fcab214a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DebugShowGroupRects f3e81215b64e972bb0b956359d4fde92983c4008d980fa65c8e464c7b9946636 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DimBgRatio 98de8b423da0bf8baf71b4e1d47dad5bb5d09b1c2a6bde6c287539682c8b50d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DisabledAlphaBackup cb198f6e08ec368575d1d22e139d3e6d6fb0c374040c39f5425ea7f812dd0285 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DisabledStackSize 5b78fbd55975f8bf2f8639fd72cd52558557569b4009c9f851e97f4b2eadb932 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DockContext 83fd52e65b42f9f92afa0a19032e740e6cc9dc78611bf6cbf88d4e3e305d7bf3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DockNodeWindowMenuHandler b16fb3a4d926bc015e00ecb2c5bc6fe1eed59462a0bfe8f713d86f09431341a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragCurrentAccum ceb7a534295a81f34041593fff8c5abbbe7fcf344be4ab25f0891d8af638dd73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragCurrentAccumDirty 756bcc9826795087fcefcccb4cb6265b571dae2ac158ce5eeb8e36f2b49c8a20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptFlagsCurr 5de0218fc15af966b28ec4be32be667cb284d872a6afa024086702f9ed759466 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptFlagsPrev 7978164bf7e8e70fe3daaaa1f088a9d532d83bd9ccb50ab0974c66956fad7829 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptFrameCount dbf41ce1b3effa50d7d84627ac3e956059175fa09f9a63305e9b4a98565f0775 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdCurr 3322eb7c3cb2d408a0261ce640c7b11d8fa9238adf0b6dd88a3094bf3833c430 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdCurrRectSurface 94afebd51be04f17156f5ba862e38a921fe3dc214d31247db45e2b085c327a81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdPrev 7f01317b163be649e480cc063c82db297540dd76653eb118e0c424332de4e336 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropActive b0b3aed0bcf286de2f4021be3f68886c21ecaab62e595bd39a8aa6a166c5c5da -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropHoldJustPressedId 4dd3fa0c8853aa5eb24bef17f0360cbe2366c6c6030bf67aea75e5d73a8057b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropMouseButton a5dd0ffb63dee4145f98ba78330c9f66399af4dca2aa53cb24ad5197e80ae7f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayload 0436373b3a0820b3df2a40b68083fda409387db2b802b5a305a5092855fb2334 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufHeap e8cd42dec021682703453385cfc580af599fb920af1698942cd702a632449340 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_0 7c86863ac506734e5a5adc41097cdff6916c393b1cc3c7e32d35547f3e0b5f7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_1 3a3a89df0c613a4b8684a2b4cd1bda84ef0ace8cb60cd5c37b8bdac9d2eb7675 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_10 9834d81f0f9a03e91e5c532117282aca95de4f7c7b14caa6c59b43d28af3eecb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_11 8bb310148752c48aa1fa9d250a8a21c3542bcb4d26b23e5468acebde585389da -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_12 b430dfdfd5dcc764e3108dc50f06596dd0795cfcca8e360a9f96c44d5bb4d410 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_13 28ac88363389ba4ac89001d49a61d32a0cc9c95a4e290526d27a5dd55bd0a55f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_14 a1fcd13fe8ef512b05550ab60b73d7eba5e82a4fd27e985f48c65ba3f2bf7767 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_15 19ab134c16010b243abb69cee8edf08e6192128f760c3b47f3a8f059800b27d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_2 1e16689d746254aad60a91e5039e7031797eb19dbdc1e0ca54e01b361bb49122 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_3 2b73ad70b887cf462e03a0b534ca5454c93c4f9edc969c09896320503b0037c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_4 a8af03f6198222805c1b8e0e3a4686da0b4fa93434da0ce1703fb87388225614 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_5 0f0807b8cfb219f44dfdd36ba389fa65d66f455d7fe93c4093ad51bbf4d42091 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_6 b9d8c52a002340e6d6238f0731b437ca2534f4c1858286eacc0a4427f82897b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_7 558b89a752af782f434471cadc46512ca9ba5e967b8e37e4394639dca2df906b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_8 f98af6c2d44f1a0b1bc07b0e56811d1a8f2e56aa8b14fd639fa812278dd3ce55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_9 e9cd2f0ef6bd7e556694a62c935bbbf56ceb6c5dbd4d8135dd9b4247067963cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropSourceFlags 4c0a89ae9f80522ab6e85b9176659b0b96f458324b874ebce399a5c41ffd6790 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropSourceFrameCount 5c086e381955241b3509f2103b117f085c05d12016962ae09d17c996cfc3d4c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropTargetClipRect b7dcb1c5643746cf1817c8935d406d168dc0ac98eb69ad9832372a4d09a87b5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropTargetFullViewport acf90fd113d6dedbaf4aba651ffdbf7bff339716d43b398d0aa69d6e0679856b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropTargetId 2dd55484fe664ccbb50cfd374daaa0e7e4a4776d3f9f45c3a701205bff56dbd0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropTargetRect c035aaf73b2950fdb356a1245f4686d3ffdde19e06d1ebf44a80042bf208b5cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropWithinSource c282d1542e3faa91d4528600899edba2dff7eb4fbcf09b03a3ae0a45c78f5907 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragDropWithinTarget 30301a551b829eb8f6d6e8707d1c40ea7751d161c2dafecd05904bea53d7e72c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DragSpeedDefaultRatio 70f56618c920427f24c6b8893c96de1c003ba44879ac616b7f81d0ae2267700a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DrawChannelsTempMergeBuffer 48731215c7f33ba3a05025bfe4deeb313d74ac1c4af3c2c6e8db37642303ca53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.DrawListSharedData 43cb36a8c5abcaa203ccb027ecb512e1a3e8ea2b3b3e0e9d48f6686525e50753 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ErrorCallback 76856f2d2739635b615bb1f9f2d7995948a7874da3956743c38c7166d4cef089 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ErrorCallbackUserData f6c40d0bf777dff226c0a4225587f4683791a3331e2e30e7341597e531f81eec -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ErrorCountCurrentFrame 00756a16f117bf707ea7b875ac433be1cc7be59ee5d87043c9c42911319bfadb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ErrorFirst fae346aa5f14b884da9fecda2bb5d9ea773e271f7807a05f34c1d0cf536781f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ErrorTooltipLockedPos 2910faf6fbe2ab673288edc5156e06304507dfb5d65c1fcd001f090cb7b6a936 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FallbackMonitor c0ca1839c8b93d881214d3b5214a52795c0674a73ad9fd8630594d66effaba05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FocusScopeStack e9a14e1e6b1c727a5cd62d2143e945c7c7722a2d3789e421f268e5f3946cec1f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Font a2542034d4054edb1800ca2e1f56aa58800adc2a8420c50f604d449c31dcd64c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontAtlases f217acf62d77fcae58c00a2374bec08e753b944a6e774fc7d6103ac2a7d7a47c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontBaked bbfc0fbae0e5d02e654d03069d4289606a54de9bcd65f8901b4413e40cedc12f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontBakedScale a4d104c3e008d900aa7d4d3ffb304fdc2fdd5579708947dbbb8a0257bcb7eac3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontRasterizerDensity 4cbe3d25af69359123abdcbe10b01ad8ffdc34853996561b55f4c5089e3920ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontSize 1e1910fd4197a9d6dcd6002c38814d43fd2b46442a4c52bbb95a6036ca067208 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontSizeBase 8a16abf8a41b7a02106ebf58ab147f52581bfedab4a03a21fa20d3455c5e720d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FontStack 451b714ce091e4877e20c5248a5f176ba2b37d7c0c04e3b5499c981a1ff44c30 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FrameCount a6ddf3c7458549c6d12e33521b4bb08e365047cde8f77d8f92aa57190b995dcd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FrameCountEnded cfa2e5c39cd434d9db8bcb1862faa72944680df658162e9bf605315038943bbf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FrameCountPlatformEnded 21ef40200bd9f3aec3bf3a2c8fd5b70ced23afeeccaa43ad299cd5c027397b01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FrameCountRendered 252794a064e64e08e05a5bc70607271a823d660e44c2874ad6fea23e16added7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_0 19281305b46fb0b87e32caec7c83685211e23c984bf1d60e551d10bd6b5c347a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_1 cbea54aae07904d33c547575df57c7c625740f24b93d3d30081afc9c1a870c94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_10 3e0b13fcfa331a8549ad452cffef56642c0573dbfa2298561790101ec7d3f428 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_11 9e18e7ba509794de47b8bf948c2e35ebbe1f02723e80cfd9d530be1336885f42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_12 624bf3f010ec224ebc15c7006fe74b390ee8226c5e75261bc62e463fc090853d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_13 32b071955cca9ff85b9ae6833a72dff3a1bfa2c6c329c5ce9b260938eaa5578c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_14 f915a9799b5b874393fe66f9d508229d19100e3677cf127e542143a757922a0f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_15 dad48603c6c5bb05df69365d3f0ad3fb0fdb33207adb3b3a163a403d7afb351f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_16 55841e8e2e63112de1f06c3af3f6021916c9a26bf2018fde544a1e17a8ca2438 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_17 b99e49a61d304a448bc35b833371e5d77eee53a3ef1fdc758662f178bc8e0d8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_18 9656624aa50d7a0e52607ad9f313ffc04d45257989882c70381536159e664808 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_19 4353a513ecc5c9e0bceae73729a7b3baca3390d3e2596c57162cb1172e2fca46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_2 fb22d8461e26839d92b6813b1cc591284e7157b33c588a4f5ac1600142dfec73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_20 0b076cd2b2e8e16780d89d3eebceafda9e1ff90fe1f57f98ecd6fb7dfe26924e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_21 935bd6cc7ffb894dfdee2ce890a4dd855649214a38e2e96ed6a595a40d2930b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_22 3ab1c34677d2ab4975c6bac86c06adcadd14ed26a01cc7cfc27d5be1f21fd117 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_23 24ae00113cb9ac28075c33e526cd9aeffcbf26e6b11980dd6ce5d0e4b66338df -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_24 3ac603fd4e8fb9906c93745720a0912aacc31b4369d7723322252ae12bef0047 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-722e9e0ca5538ec4"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ActiveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b84c9a6ad7df74d"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdAllowOverlap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a239db7fe297e83"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.ActiveIdClickOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db9780f2423329b9"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ActiveIdDisabledId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7687) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b6362f4b8ab285e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdFromShortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7685) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57d58b4ebe126b10"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenEditedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7683) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58a235c133081c92"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenEditedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d6178cc8fa08527"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdHasBeenPressedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00ba0c2586b47635"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ActiveIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-082ddfacb2aa167a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdIsJustActivated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6d3546e6b39b9a8"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-829d02ae982feccb"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdNoClearOnFocusLoss`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-179bbe43210486f8"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ActiveIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ab5a0ca70b182c8"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiContext.ActiveIdSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7689) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d561cd3ffc306df"></a>[`public float Inno.Native.ImGui.ImGuiContext.ActiveIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f325ab1116f7985"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ActiveIdUsingAllKeyboardKeys`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7859) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f806c10f41a6932b"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ActiveIdUsingNavDirMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45da1c2b6a5efa27"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiContext.ActiveIdValueOnActivation`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74eaf83957f06d8c"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.ActiveIdWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11b649d9ce5f39f3"></a>[`public int Inno.Native.ImGui.ImGuiContext.BeginComboDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9438ed293f4a4738"></a>[`public int Inno.Native.ImGui.ImGuiContext.BeginMenuDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eca744232015733c"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPopupData> Inno.Native.ImGui.ImGuiContext.BeginPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d997ab392b19608"></a>[`public Inno.Native.ImGui.ImGuiBoxSelectState Inno.Native.ImGui.ImGuiContext.BoxSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef46edc5c9abfde2"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiContext.ClipboardHandlerData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8047) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7afcd1ccced8ba27"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiListClipperData> Inno.Native.ImGui.ImGuiContext.ClipperTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8d69018174246e2"></a>[`public int Inno.Native.ImGui.ImGuiContext.ClipperTempDataStacked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98ee17e8b5bd2d38"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ColorEditCurrentID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04165c110839199d"></a>[`public Inno.Native.ImGui.ImGuiColorEditFlags Inno.Native.ImGui.ImGuiContext.ColorEditOptions`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdd023ae78be6305"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ColorEditSavedColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8030) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ec7577295bdaac6"></a>[`public float Inno.Native.ImGui.ImGuiContext.ColorEditSavedHue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b32f69bb5fb2e65"></a>[`public uint Inno.Native.ImGui.ImGuiContext.ColorEditSavedID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07a9990c278f4fa0"></a>[`public float Inno.Native.ImGui.ImGuiContext.ColorEditSavedSat`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8029) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abe76ce2750b7650"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiContext.ColorPickerRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8031) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9d421cbdc7a2d9f"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiColorMod> Inno.Native.ImGui.ImGuiContext.ColorStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7870) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c60b8e6b2cdfeb2"></a>[`public Inno.Native.ImGui.ImGuiComboPreviewData Inno.Native.ImGui.ImGuiContext.ComboPreviewData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8032) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e7fe1dacb422d04"></a>[`public Inno.Native.ImGui.ImGuiConfigFlags Inno.Native.ImGui.ImGuiContext.ConfigFlagsCurrFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7629) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15f1ddfdd8105ce6"></a>[`public Inno.Native.ImGui.ImGuiConfigFlags Inno.Native.ImGui.ImGuiContext.ConfigFlagsLastFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7630) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df2b741149224c8e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ConfigNavEnableTabbing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e50473b34ccb887f"></a>[`public int Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingKeyNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea50de08a5661a41"></a>[`public int Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingKeyPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4ce356a56c08e9a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ConfigNavWindowingWithGamepad`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8fc0bf3b8bab68e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7608) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fab41b26d9e7f3d9"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7609) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6472acbf469d028"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-829a1968fe76a970"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7619) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25d5b759a2ddafdd"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-032f8709ebfdc498"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7621) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6469d21360498cdb"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13c53109f3b1b1a0"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7623) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-413bbf883b8a020d"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3890b44c0b4a2280"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e01d6caad865a899"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7612) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5577681cdf5f6f89"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7613) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d6277e1095e4f1e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7614) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3dac31f2adfe30c"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7615) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c7c4116cc56cf39"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-868530dfa68b9a0a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ContextName_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7617) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2947f62a285c4bc"></a>[`public float Inno.Native.ImGui.ImGuiContext.CurrentDpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7638) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-153a345fa6a9ef36"></a>[`public uint Inno.Native.ImGui.ImGuiContext.CurrentFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b573b8d2d97a0b29"></a>[`public Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiContext.CurrentItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5cd2273497c6d33c"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempData* Inno.Native.ImGui.ImGuiContext.CurrentMultiSelect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60c99232482451d4"></a>[`public Inno.Native.ImGui.ImGuiTabBar* Inno.Native.ImGui.ImGuiContext.CurrentTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-636c047308967f83"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPtrOrIndex> Inno.Native.ImGui.ImGuiContext.CurrentTabBarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59b4de280bd2b4a1"></a>[`public Inno.Native.ImGui.ImGuiTable* Inno.Native.ImGui.ImGuiContext.CurrentTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d23401d3e69a8ba9"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiContext.CurrentViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7880) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b948e7d4ee5e424e"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.CurrentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7654) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8defb98431b36794"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowStackData> Inno.Native.ImGui.ImGuiContext.CurrentWindowStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7649) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94487f3ff7951864"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiContext.DataTypeZeroValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71dbeb689d27b76b"></a>[`public Inno.Native.ImGui.ImGuiDeactivatedItemData Inno.Native.ImGui.ImGuiContext.DeactivatedItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c81cf8bbd327595"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfo Inno.Native.ImGui.ImGuiContext.DebugAllocInfo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a77b4032c6463580"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugBeginReturnValueCullDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8107) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4355c83b4a2ee711"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugBreakInLocateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8105) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64d17948fa04a20c"></a>[`public int Inno.Native.ImGui.ImGuiContext.DebugBreakInShortcutRouting`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7860) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0644986a2b1e8735"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugBreakInTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da83e032dd6ba399"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugBreakInWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7653) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd73ed3088c4b15e"></a>[`public int Inno.Native.ImGui.ImGuiContext.DebugBreakKeyChord`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bf8efdb9b7e5375"></a>[`public int Inno.Native.ImGui.ImGuiContext.DebugDrawIdConflictsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fec7e3ba4508cef7"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugDrawIdConflictsId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ad30764d009657a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47b9267b2014ba76"></a>[`public Inno.Native.ImGui.ImGuiCol Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7869) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc352507989b56ad"></a>[`public float Inno.Native.ImGui.ImGuiContext.DebugFlashStyleColorTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f7a06584209b55f"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugHookIdInfoId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7667) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c41e5d11b7ce049"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiContext.DebugHoveredDockNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1dfdfbfb9aab4382"></a>[`public Inno.Native.ImGui.ImGuiIDStackTool Inno.Native.ImGui.ImGuiContext.DebugIDStackTool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94c24baa72e9df03"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQuery Inno.Native.ImGui.ImGuiContext.DebugItemPathQuery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdbc344ec57c4e01"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugItemPickerActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68fdd2669d83cafd"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugItemPickerBreakId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6443031043edf066"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugItemPickerMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38db7248b68399df"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugLocateFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b31b7005aab28957"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DebugLocateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-643345fd64b619ca"></a>[`public Inno.Native.ImGui.ImGuiDebugLogFlags Inno.Native.ImGui.ImGuiContext.DebugLogAutoDisableFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7774c8d09f76b1c3"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugLogAutoDisableFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d4fb087d729eac9"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiContext.DebugLogBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-411443db64499fd4"></a>[`public Inno.Native.ImGui.ImGuiDebugLogFlags Inno.Native.ImGui.ImGuiContext.DebugLogFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7952d53cc869b13"></a>[`public Inno.Native.ImGui.ImGuiTextIndex Inno.Native.ImGui.ImGuiContext.DebugLogIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f5531ba2a14c642"></a>[`public int Inno.Native.ImGui.ImGuiContext.DebugLogSkippedErrors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e70dbafcd80490a3"></a>[`public Inno.Native.ImGui.ImGuiMetricsConfig Inno.Native.ImGui.ImGuiContext.DebugMetricsConfig`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aea50faf3369ae64"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DebugShowGroupRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7867) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe593eef1217f06c"></a>[`public float Inno.Native.ImGui.ImGuiContext.DimBgRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d446e331b6cb19a8"></a>[`public float Inno.Native.ImGui.ImGuiContext.DisabledAlphaBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8043) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46a00b4a63f1cd60"></a>[`public short Inno.Native.ImGui.ImGuiContext.DisabledStackSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8044) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5977c4dd51a0a90f"></a>[`public Inno.Native.ImGui.ImGuiDockContext Inno.Native.ImGui.ImGuiContext.DockContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8053) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5716eb248dbecb2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiDockNode*, Inno.Native.ImGui.ImGuiTabBar*, void> Inno.Native.ImGui.ImGuiContext.DockNodeWindowMenuHandler`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce458c17d0fb5851"></a>[`public float Inno.Native.ImGui.ImGuiContext.DragCurrentAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8041) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d5e722ef6efcf29"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragCurrentAccumDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8040) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13243670d53947c7"></a>[`public Inno.Native.ImGui.ImGuiDragDropFlags Inno.Native.ImGui.ImGuiContext.DragDropAcceptFlagsCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7963) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3afa1fdeef161d5"></a>[`public Inno.Native.ImGui.ImGuiDragDropFlags Inno.Native.ImGui.ImGuiContext.DragDropAcceptFlagsPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8332183e6844bce3"></a>[`public int Inno.Native.ImGui.ImGuiContext.DragDropAcceptFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d23ec4c7f2a305c2"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7966) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5801a06d6f3a5203"></a>[`public float Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdCurrRectSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed2d11dcd54060c5"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DragDropAcceptIdPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7967) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ae4cad4d63d0999"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7952) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2045d6284d3301e"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DragDropHoldJustPressedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2dbc1b35d27fc64a"></a>[`public int Inno.Native.ImGui.ImGuiContext.DragDropMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8c8b2c101990da2"></a>[`public Inno.Native.ImGui.ImGuiPayload Inno.Native.ImGui.ImGuiContext.DragDropPayload`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbb6c14506f33696"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufHeap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ceefb0b8625676cb"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6774dac438d8ad39"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8e79e759d5838a56"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69149ba72e1130a5"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62e4078337cef3e3"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7983) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e903378cdf8acd9b"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7984) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-536c61e932c33d03"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be3432b26864431b"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd5a0d3d3ecc444e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50551cd1e2e314d5"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a1e1bb8487628a7"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4369fbddac503987"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dda3605a4f871c0b"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5215212d6414a64a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2516ffd8a8d4eb3c"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d20f631436013102"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropPayloadBufLocal_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e91cca7a6b462c45"></a>[`public Inno.Native.ImGui.ImGuiDragDropFlags Inno.Native.ImGui.ImGuiContext.DragDropSourceFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7955) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-756214af27f4cf54"></a>[`public int Inno.Native.ImGui.ImGuiContext.DragDropSourceFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7956) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04cd71dae859bcad"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiContext.DragDropTargetClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3aabe6b916cd7207"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DragDropTargetFullViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd0c74701044542d"></a>[`public uint Inno.Native.ImGui.ImGuiContext.DragDropTargetId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb9f9d98bdeab5d6"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiContext.DragDropTargetRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d1689591dbbff5e"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropWithinSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7953) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4584955780d76fbd"></a>[`public byte Inno.Native.ImGui.ImGuiContext.DragDropWithinTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7954) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-befc3085b966e018"></a>[`public float Inno.Native.ImGui.ImGuiContext.DragSpeedDefaultRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8042) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e652e0d9aaa0609"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawChannel> Inno.Native.ImGui.ImGuiContext.DrawChannelsTempMergeBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b970a948456306f9"></a>[`public Inno.Native.ImGui.ImDrawListSharedData Inno.Native.ImGui.ImGuiContext.DrawListSharedData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7639) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd14bf252c5cde6b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, void*, byte*, void> Inno.Native.ImGui.ImGuiContext.ErrorCallback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8699f99b5817975a"></a>[`public void* Inno.Native.ImGui.ImGuiContext.ErrorCallbackUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9070696481318eae"></a>[`public int Inno.Native.ImGui.ImGuiContext.ErrorCountCurrentFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdfd73a67aa76950"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ErrorFirst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05f66824754c1510"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.ErrorTooltipLockedPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-945983149d50662f"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitor Inno.Native.ImGui.ImGuiContext.FallbackMonitor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3bebed64cddec4fa"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiFocusScopeData> Inno.Native.ImGui.ImGuiContext.FocusScopeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7873) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35a9416a6b46fb3a"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImGuiContext.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7632) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dad011a72f36952d"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontAtlasPtr> Inno.Native.ImGui.ImGuiContext.FontAtlases`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7631) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7cbd7d4dbff7cd2"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImGuiContext.FontBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7633) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ae174ce643993a9"></a>[`public float Inno.Native.ImGui.ImGuiContext.FontBakedScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7636) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7f9c4818f7d284c"></a>[`public float Inno.Native.ImGui.ImGuiContext.FontRasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7637) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db213776ba69abee"></a>[`public float Inno.Native.ImGui.ImGuiContext.FontSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be0b9d9aedbc38bc"></a>[`public float Inno.Native.ImGui.ImGuiContext.FontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7635) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5faf71574fcb14e1"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontStackData> Inno.Native.ImGui.ImGuiContext.FontStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7872) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3af7b935759a151b"></a>[`public int Inno.Native.ImGui.ImGuiContext.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7603) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b01374f32517e930"></a>[`public int Inno.Native.ImGui.ImGuiContext.FrameCountEnded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7604) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a844849acdb01bb3"></a>[`public int Inno.Native.ImGui.ImGuiContext.FrameCountPlatformEnded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7605) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8395f5f7dde2e25"></a>[`public int Inno.Native.ImGui.ImGuiContext.FrameCountRendered`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7606) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a108dfa028f7a38"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebd9154028de5b85"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc199a0194b5b563"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19e3d8aa998aaabb"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f525150e9765f93"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-962477f4ed605430"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58b5c8a33f922d06"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8132) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb1630181dfe511b"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4967ba4a500b8e58"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4aba8f3e0bd8a5d0"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-009ef872d5092494"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e630f57da124c94"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdbb0b02d47fbc34"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17d410c87e45dded"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5419dd6ade00987"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b955b1e315b88b36"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa7361ea1868d540"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd45168f02a7b071"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiContext</code> · 181–360</summary>

<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_25 8692d8b91798cfa11837a0ac17bf93e18176743c790f56082407a577f84edf4e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_26 8f1a1c019a431894e11edfc301d56a8deed39e2dbff93dc5b9b245ac6a11936a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_27 11ce2bc3400da51998769d9c0348af7f6c6b80219ecc91e8fee435dc5d2671cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_28 c283ba535beb6bba11ba3ae1ba92390b5c4cb6904aecc8c5bf106a423b5f9007 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_29 756e0a7a23ff560c5cfba267278ab91998843d662fc40f1c8725386d44990f7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_3 81e626cd512f1887dd7a0cca1895b4bae0677ac9bd81204dd73484e378d3ab90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_30 77b6a97e7cb394de3eedadbf55dda5854156bf83b15986fcae1cde707ef983d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_31 22b1a0ee379abcd9bd8b5ec4cf0c2029dba9867de2d247d96d25c31b85e7df58 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_32 b7c67de5294ec15d28806cfd13a4aed4f501f1b02dfbb6c9fd7311e91df95671 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_33 f7150779e2764356ed601314818c7251227f998c6a23cecc45d8017b0ccabbfa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_34 35bde2d1e4b49f5fbec417f3cc725dac38aa6182de38a5a9f04972568059b3a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_35 14ec195115eb9cac7a5e266514a23fce6191487c506ef5ccffb261e06165b78e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_36 90ff5fd75f2190bee3a9bcc568ed3caa0c5c6939a7ff8d284a261ae022464bdf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_37 5b346203da0ccf0827cf9dca192d3d93b0fac744cffc65c3d83b5bc14e5ef7bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_38 0e5fc1b6196b85854e5be8ccc3d1552e3f4684cb744e343e0bd5d0a23071aa45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_39 e3dc7964a9b95b3abdedfb44258a6b3b24c803cf82d7ad2e17c64faa21b876d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_4 a5fdbccbc361ac0fc56aa1c0258e5a7527d3a0d8c00ffb274ed5cb4b1d5cb399 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_40 5569a446918f590d2f5d0d6d3ba192e1b08dbf4e4a80b4bd6817e90943a8f9d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_41 500923883709e5bef91a4e3d36eaa346323f455f750530098fd581008e7f0f78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_42 d871bd876cb0aca372cfe0748073365cd46a4d47b3c526a20b1847cf5da3b36a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_43 61dcf7700c7c1faf054c46798557a3c7492fc48ba0ff48bbca399c94c993a67c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_44 2f041308f3717b9a13962454f48315bc3181c36370275023ec637a4fd4f653e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_45 a5aaa39667095a3b73306f20f686c64b41905ab58e47875579aa18ebdcb1f20c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_46 440059cca4f9a28e762a4f59ece4b39f49f9683375d67a1995e78797595b3628 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_47 c55b5acab37da4404895ccb2768ac9969c3b5310b6c895ab3c3ca47be69e9da8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_48 408b0bae32ba582d90a77bca26192365e42f81c32bbe0817cb6943b54d7d1a63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_49 7e0f28067db902d7ac1cdf9bb0858a02abdd81fcd934ead1577e4a4c3dfe535f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_5 7237f94fb71636cd72d23a70318c54c0ac295e1b67b6c7326854ddda30641f98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_50 230b7ca2e8dc5d78262c3404e0d963fdc8b13e6aad5f8180642a6a9c31683320 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_51 fc602aa6381f4d2a8261cc0115d7e6d968818452cffa3c13a0dbe0b6743a2184 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_52 47b2fae931e31e4308975b70b6834965a7263b17f10f8d257a80a963f8a431e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_53 76af2275d411280a1cfd1791f3c89f8ad58765731c9c6bbe96f4952007f9b640 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_54 e691e58a6bf1e1400490dc43907233cd88632fa7229ffa69218bea4baf84112a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_55 56afe4f7f25f9ed79f497bc6c5658acbe5ffa5ccef19e8068acd12782b663b21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_56 8cc29cfd95fe56adabb56dec309a32f211ebf7dd41cbb8e8e1f684ad118d4b12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_57 9ccc1a7664a9054b27b85ab059a92d6d61c7f076782634ed8f5d83f76be6d1b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_58 39bc4935c09ecbf1a8186eaa696bf41622a192ea8f4262da9716740464ddecdb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_59 e9b93140285e7f590ca1757138e2ade83c8acb9c1e4551a743ab388159bdb4aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_6 e437564e3dbe0d0f073b0d2c6ff3d626fd2de5aa474688884cf429c5e35edef8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_7 d615a44e4bdcb4671803e841a930ed40db38ec4871cb051338bab1c712d4daca -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_8 c26af37905c44bd48670313e4920119496632a5494634b1f95406f9cfa7af513 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_9 af7e698ab0c80a8c67035f298c6bef71d3ee0eaa7d4bc7008572fc2be5061f64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameAccum f4469034b791d2a47edc7a48c12e3a83b21b18bf3c4e0377dc994368003d1a68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameCount f7a0ba32b19751ad6fd62b1836d688482795f9b85f67fb728da0f08bc3df09bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameIdx 055facdba977ec9d04e4556dc278c4de67d9a49ff4f3e59433ba51d8f4934337 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.GcCompactAll 0060eb4da169e8dfaeb6c6d63dcae4f1116e3d750e62baa4bb6c5cc39e764ee5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.GroupStack e1fbeffd48b01a30421c8a279107c1d0f03344af4e70697ca4565eddc3dfd473 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HookIdNext 5b7a33cdfb140db7ad2da1b6aea366b933b999d611b6a57881390627a58c4aa4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Hooks a405633d7d82b8b2e00184966c1659e29571dd1e789590eb8f29a3fbb7aaf5af -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredId 3de54530ad65363ef33dc7d09dcbc5072baa9718de280889f4874d393b93ba65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdAllowOverlap 27e1cb4b39d159f23c0f56281460cdabd39a0263ebdfc178065fae667e68e9dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdIsDisabled c2d9f8aca7974053a2ad4be07e1df1d0e40c5f47f44390b2bdf2a013a3626ee6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdNotActiveTimer cfd803f604df059d5d27baebea513dc960767cef0c7966ecda766aa07ae917d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdPreviousFrame dfb86d43477afc91e6bdcec05efecdad07d4c3cf8c8c7bd9d6580d0811721ed2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdPreviousFrameItemCount 770a4794496bfa8b3caefa1955b591311231996a0eedefeba0ca40fa9cdd9beb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredIdTimer ddbb29d322b30a10bd9ca2c8d673b17b398858b672a898ce84946597109aa799 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredWindow 5017e5869685c715de9db92609f80d8109e476a4b07707eec1fcde6973f9c887 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredWindowBeforeClear 8fa01618b8c17eabf17851ba901380068158bd4b5595e60a10c9f151e4d167e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoveredWindowUnderMovingWindow 161be2f0065003ea9c22e2c26891ed70de1a5a8fd4e0b62cb81120a820d1cb83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverItemDelayClearTimer 98f871d8d9f9287b3c388a8a3f17a40bdd256f054597b01e51063dafefc414eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverItemDelayId 697181025152ab891bb14a7a8773c2342111a231615051ad06d8a3d389306953 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverItemDelayIdPreviousFrame 60c90eb2a536debc06f1a6478e9cc1fdc67f7bfd6ba5990caf5569ade173379b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverItemDelayTimer ff7ecf57fc2670936aaea1cc4b0775fdcfe6b3310de53b910993b19210ec4816 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverItemUnlockedStationaryId 6b3d0c8682e89c34387da630b5c235ceada977585a849c0fa3536c53b0a452ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.HoverWindowUnlockedStationaryId c165a675f347623b09685d974ac916f3bd39d229bb40faeaea62b584f90b02a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Initialized 48901c39410f1ef121e6c037701992095668df006f29ce7c23911779c9754ef5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputEventsNextEventId 450e00ab79ea8f2a6e13d7ce8da5ec68ff29de3b5b92198e02b1ab73eb3d3700 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputEventsNextMouseSource 90a1f321dc5bfa705304ecd8307c22cf4d8f7082c436e2490bae7efd32affcf8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputEventsQueue 7626594f62a235779c1badbc187d580246d322622d76c33e5a6dbcf70c7f9ea2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputEventsTrail 274f83c13bfabc5d602bde6c6a3f1db936ceb79731d225055281a375d884147f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputTextDeactivatedState 2de466f5f09df7096dbb77694fb4b4390a3973cfcf1edd013bf17244fe4a48bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputTextLineIndex 647b315a245193703cde36c8ec7abfcdb10fbf57ace410585e503c1a4606c6d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputTextPasswordFontBackupBaked 00b1784ef220c0dc3b3d5aeef497c8587f02bf64d3cd3fe4875f3c49eb4f46a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputTextPasswordFontBackupFlags d92555998535710465725fba6a2f3f46061f3de1f9a89c4ac7e4aa2f96c4b906 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.InputTextState e6af7a4765e86411892d8cf3cda6b2527d59a97b7081f0d37af07c5f2df24e29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.IO 451ad0be3315ef957691a9e72c00e40bfc8a001fa7e4261bcf3e79b4d6838d0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ItemFlagsStack e29196630da85bc5af6d331cc1235e71138b5f030797313bafc610e8b0b94ba2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ItemUnclipByLog 5f8630cb09ab1e761253bc095491c4067580832b814bbd91d250adbc5091d7cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysMayBeCharInput 6ccbe866d67c5686e9d95bdc72473341ef9867c4e2cb6853bc4a72a0eb3fbd47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_0 182a4d5abec9dd48f41dfa8457f0eb4f15f41ae5b9b0484e0484ed26177a4553 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_1 f1ad69b7d1a30dcd211a49b4b82f6362364d4ff52403fcff1cc6f772bb3b5514 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_10 542081d695082d41cf84947c7d4d0059b7b9394dda6423e3d4b99303764be7ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_100 5150bd63b1be756dd07061ebaa7840f891eba4823f3753d8ec503abe85b45d00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_101 4376aa7e0b109e386c2073205553b369ee75582a324b433888e83f5f2f504366 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_102 fe8f4145504368241bbca7b193ecd2bc402c899d352973d3cedfee289002739f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_103 2f323e1f9055f0891d54a0fbf515ce648cd604fbe3b8c628421b5e5c0729830d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_104 ed076aa14554bd888ef5151be051275fd46b8aff21ec1f30f80347ea8232b9b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_105 0aad89a729a031bd2d2b31a95e63297e298a6ca5cfad00cf1a94abfe17fa872f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_106 815ce9afe2f9268e7c759a8b684b03bcd9835e647a1ed7e1068ade359439ab74 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_107 e46f7a20d93741810d30061f9029f116dcfbf2f20420e6cf58cbd99311b526c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_108 b2577ee0ad2d4a5ddd73b55ea21fd806a10afdb895cb1cc182feffe9094c8a6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_109 af918cd35ea8bd84a30610c923ee340b6a59538581030f918844d9d280286d4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_11 6048f33d0ed4ca5e8580bf09b71fc38d563610f35a6b5f910d1ff8d62f29d8db -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_110 afae9c3c328b8c0a99ab43545f1b67ca5fff06fa252636851cfa35ed50bb168d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_111 0d1aace3ba4fd8936d9d92c64c543727796ebb2e3926c4f60488275c93d955f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_112 33ae4df89b66aa9dd325eb0435481be47f88b0cfaa68c41345a32b56115e007c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_113 98feaf257114450aabfee81ba2d1279b55422a8d78af2b961c3f20f2946ab27c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_114 1e68d809edd697d4cc97280f88c91c7c6c7e70a99ca03a0c5e7e410ca9520b18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_115 9b36be97d8de4969d420977ddcd4793f336379c29b0b820f74f3829dc0109951 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_116 cc202efdf768c88fa95b6602115e5578667402e01049f886842976e4f9181e2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_117 a196a5d43a5b494f50da520ea95db0afdff849b5117ac4a08fc397eed2f291a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_118 6cd4504e42fb65fa6a5cf50c5188705f78e04f4d45162f5a7d60f8e91abf135c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_119 20694f412d5069187989cd71f8622d531b77de43197822a7c2cb51bfa8081a52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_12 7450b82c36ac61c2ea6d37d570849083fd7752190fe1c5ee448dae8df00291aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_120 f710f842d824123010008e3f280900e3d2c1e70bcbfaafbd60569f189a99a8cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_121 4e85c2f169ae7e8b125ebe7e6a194e6c0c920e6927cdf58e10b4412158161f04 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_122 68ed991394b8e8d3468f7f1c817da98b2f106c129da2b62d01a944b0e4e69305 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_123 c9895ee7c1d898ca1ee4186fd752ed6648fb3ec2989a56fd729623d48694b23a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_124 5ceb05f7061fd286460c7c462ef6e8d988a33e3cca6eba5f77e75d65dfe3ccff -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_125 b5be125e157ab0f99d06d7bdfbe733a67988a22b84a8412d5f4377827d26561f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_126 83808d467c65ab7854926cea05ccb6a396fe92f2dd1fd0ed6a5fea87bdb9c0cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_127 ef83c1eb4691d101b31aa73db24b269ce698bdec875cb683008a11bd4f5a1401 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_128 0aec11df7deaefb457e342f54f116fe999c34234969990c3eda58d5a3bf121d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_129 47aae489fcc46b79e55c66bac39021c3c6acdd31f3254c71fcd553b62d80bf4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_13 167ab6b5eed2626b293a46719c29ada93654dfa29fc03960ec61969c4ce5ff04 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_130 5739d891d6af192de92a3015b8934c51a4a3200b5df6639744b0653724509f10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_131 efa3afb42e8f9362c14c85e7254c24764fbd88feb0acd3c610de07b0d4a0247f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_132 10e32b0c7e205b887f18f263a2b6db9c5cd0b819135e667e7e1c72c4730f7fc1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_133 752e8ba2dc864f02f79ea9bb500050d7de4099b63e6bd938ff847ac7417c2ccd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_134 7d4d31f4ddad739e296aea754156d49d61cc31b37945a5831f956a0b5ff8b901 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_135 1f625a1e80541527357abebe48482035dd31f88776e4c012fbe46bcca223a22b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_136 a873e30332cb0fffcf80518f5709cf4881996cf52378cdbb462e8c2bba284373 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_137 57c2ecaabd5a19346c28cad7f573ae9855a32189b61190e374da4e2dd01f744f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_138 03d62a6a3338d55bbfd1aba1ffc450dd39f1767c66de91c3e69a1e029613e836 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_139 cf226cd8580e372d9e4b38b180815fe5ecc88c04843d17d36bba634167cbc7d3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_14 f04890f255bc79b2f292ae46c1dc17765a8a81b633b929af5f36267c22ab64a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_140 4e83e7974de9d27997c68a3793072beba6565b113e67507f5a652ce3ddeae049 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_141 5d5720d25a85fb02aa2b2879b85b2a7e0d84b3a3d02050d937f9dc12f6604f58 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_142 7a33d9ad7f8545ea6e96722eb82c12fc0e177b4c0087827b8558bb21725c3da1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_143 94212d1a9fba76dc1d3703862a0004d76cf6548420d89d5cc157adf9c2d5ba42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_144 1679860cde23909af6c3d121856761d687ad212b38bd36329b5255bf758a24cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_145 11adb10052b4f7ccfe81e7f5ed2fb85214cf067c420b3e213de1b28fa5d8f267 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_146 3d52306885f00838da75a38ddbdaca06f3c7c2ee2d1bc6fa475a96f4a57cc681 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_147 e904fffffb650c6eddf48957cd7c70ba862042094300c62b00eb57826db74d73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_148 192456416979f7d3499cfc942eaad83d4f508dfd31419634b402304eb0e6f003 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_149 1eafd25df3456677a89ce78e9e2a791ad59b66d469003e9597a9517168ef4c9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_15 f95ba858fb67e9478ade1bad91f44518190181b3cc76bcef7dd70fe3c2b5cd22 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_150 944886764fcd95d770312dc6ddf1fcf6f519efeb22a80a5f3568b58b4b3f5bfa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_151 e742f332942f8ae6283de438ffd1033ebd86a811553932107233f01139738216 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_152 ec78835d950533cf21232d77a32534294225da9677886b03faa936d2880fa407 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_153 771a47930303845eb6021f3322490e289e10082fa9686090646a1f69ed57db97 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_154 742318bff8bc4ce81ebdccb56b1bdac5933bc77df3a04fe0f24a91ba46216d03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_16 6cadd1b670965879364a21f846e3c95cc3c4c41459f392fd853cf1e515fd1e73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_17 2cba6c4b90949d018565e21c078a3f491ef87979a7eee20df499786f54db337c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_18 3e6d00fe92832f23f456c07d078b47441470115cdad869b31f9f792e38045c83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_19 865bd373f6f0ff31a9c8642a1f7adb473369ac9b7263e1c79c743cd8f49aa355 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_2 26b63883d3d295dd8e900dd59272310e670425f4e4096b6d96a5d673a5cd7356 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_20 4e00596d28ca7039a539c993f0e9385dab68286180325943211445333b9a1768 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_21 d943febb5f5b0bcb9173f7ce2015047f555fb6b9dc8754c62b65060572b9407f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_22 425e9379c3fe660980424e0a26ce4fe0818e40292819f58e50f8d2b418544b42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_23 4c7c10ddcb211b6de49f3ad2f14e18d5afba2068a6347dac8afe018fec209b17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_24 f6b557b08f8f74ba7bb34d91dc43d53a502ba6c26bd51e497395f5d50e151281 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_25 f8b8c5158094b25f6db197c95a486305bd048e3cb24276045fb05f322de7d85b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_26 cd55f45135b4b25b4444a2487acb5a750337989923f6a8594720ad2a649d0705 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_27 9db63550749d91cd7a350fe5784abc09d523fa63a2a1359603912717da6dfdc6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_28 df3bfb5b4873661c0b088ce23facaf85d49fc438c6ef534da25616052e593b43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_29 e48d153a8234b90ea594c4ba16dde78e8bf713d993c77362666f58df5e1b3104 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_3 2d8a1950dddff11d63fdd49ae54d7df71e86fa2099192fe6c648aa4af227b05e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_30 bf31fd48c7633cd429a3f485574d14e48e59b7e8654e789b787c6378a255c1f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_31 6b58368feadcd8a957fde27a6bc55dddd8d60be4022ee18e08372090c8e7182e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_32 2fe8e659ce26c7800f3da4c92448580a3720887b1a65acf699aff12f6f616e07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_33 233f1ce4bbb53928c90f374122da0f4775a65ffb14a346d9ea88b3fb56e9b804 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_34 2021b38daca3c481b339ab10be997c730c79927821905a8a97ac39ebb2d329d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_35 65616f60c3f29f8374677f29f1037c7c1f22b50ec73ff2cc72f85419400651a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_36 ba26bff5c9e19819e59915acf7a8d4f94fd2c8f0acaf2c9d60dde6392dbeee66 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_37 f6c40e27fec59e918a4cc8ac055c7b081e5f94999e4a6315adea264c8e2acd51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_38 888e2713bd35ed62a3d923363eeed6c6e2f7aa684aa0b46b0bcc4ffc42567321 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_39 585a391687e3b948b967ab999a3466ccfc68ae7a5efd1d49dca67be414d90eec -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_4 5277a6ead582bb130710c2475fa43706ce2b045fcc2a43273cab78c03811fa17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_40 3495b8041ea1f9a97d1cda71c80e228416d1c0b71eee4999fd8c515a0062b1ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_41 61495c753d2b9836984f3500e4d1fb0dcdc1ec739e5bad89314f9452254dda10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_42 666f4261164caa3d78c1b56a8cf10c63d680cf0a5022526b5749ca6ffdc5c4f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_43 942ee0772eadde94dca041075c28aed847ec78ee073062e327a2317601011779 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_44 5756c2e301cf79499cb66e33c0947ddc54d96702a11f0f764fb91f78662a20b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_45 7f2056d339c5fd33a53e2fc6bfb05132e307c482f14f2bb26ae648862813dc05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_46 874165972d80c88112ffa00faa69596da13902ada94b5d6635bc8f2bb364caad -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_47 c672c41154d4828460853f58041560baac55df86d26be2b43673dd749f9f595a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_48 7eb966251f2290645c6df6e3369973cf7fcfabbb017e24ceefe818d7dfc6b272 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_49 0bbca43225b56e4d6adb266d64abb25d53b64a9072b68c7e3a1b33c5b8813eba -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_5 1fae5a83218f96f67859719ab6629811f30db169fb57a54502602d80e7902951 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6822119a9d8b5128"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1aa5cdc396db63ac"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c8257fc3d56c39"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6c83da252abd2ec"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7459f010714bd4d"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c898a413f2c3153"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b609950e9b3ed35"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d39e51fac0f95d9"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-973cd048c6eed70c"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60d7cf24b4483cf8"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3868db18f0287811"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14bb6b7dd2f4e563"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84178a0c289b55c4"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46fe8a5b4839d681"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27d1429100cd5bb0"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6855d12ce228a96"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4b7f0df5cc66435"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16c12acce3139116"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c884c16ba317fd4f"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e247456e95df0687"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14ec3f5f409e969e"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6515ec0622fa6325"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f84043d62274b99"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e59c30b1f0e32c5"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea8f96873fa4828c"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af63e8552076276c"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6db255b8b10e6f7f"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a5b43d89e5290cd"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fca33d222ed4a4a9"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-441b8ffa785a71fd"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19b930db7a7b9404"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8285133bfdeb1ce0"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fda1913216baefe8"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56142a78fb654ee0"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7cf220c7c86ca91"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85f30bacf24b4ac6"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8175) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b51b50904818f0bb"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c57f455c3c862d69"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38e74aecdc14ca7d"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a82c37e69c4893e3"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54ff41671dae5c5a"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5e196ab4d1fac56"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrame_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8035f97d1b1e28f1"></a>[`public float Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d954de1ce85d93db"></a>[`public int Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85931c74aa342557"></a>[`public int Inno.Native.ImGui.ImGuiContext.FramerateSecPerFrameIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12415b4c78c24845"></a>[`public byte Inno.Native.ImGui.ImGuiContext.GcCompactAll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7868) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf3ae6a34731bfa6"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiGroupData> Inno.Native.ImGui.ImGuiContext.GroupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7875) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0aab501168b8e81e"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HookIdNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41574b9abfba27cd"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiContextHook> Inno.Native.ImGui.ImGuiContext.Hooks`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56073b7d65a1b16e"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoveredId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a73ea48c0e8e27ac"></a>[`public byte Inno.Native.ImGui.ImGuiContext.HoveredIdAllowOverlap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eae0954f658d3ae1"></a>[`public byte Inno.Native.ImGui.ImGuiContext.HoveredIdIsDisabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd58da1bb7bf907b"></a>[`public float Inno.Native.ImGui.ImGuiContext.HoveredIdNotActiveTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f6d4e49d667ab22"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoveredIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7669) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69b2c9f91810cff5"></a>[`public int Inno.Native.ImGui.ImGuiContext.HoveredIdPreviousFrameItemCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7670) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e4c6ad3eb6b209e"></a>[`public float Inno.Native.ImGui.ImGuiContext.HoveredIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46be321b883b26bb"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.HoveredWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7655) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5477a0af4f86f013"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.HoveredWindowBeforeClear`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7657) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53ef7fbbb4ee8488"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.HoveredWindowUnderMovingWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1542c3512a257268"></a>[`public float Inno.Native.ImGui.ImGuiContext.HoverItemDelayClearTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd71f2127d0d259d"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoverItemDelayId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0658e93850edfcf3"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoverItemDelayIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e93104f4d258a64"></a>[`public float Inno.Native.ImGui.ImGuiContext.HoverItemDelayTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90aa9f266ffa9507"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoverItemUnlockedStationaryId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36e0dbb585691207"></a>[`public uint Inno.Native.ImGui.ImGuiContext.HoverWindowUnlockedStationaryId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a537792030dc279"></a>[`public byte Inno.Native.ImGui.ImGuiContext.Initialized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7599) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1dcc6896738f710"></a>[`public uint Inno.Native.ImGui.ImGuiContext.InputEventsNextEventId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7645) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f59353c18bda10f1"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiContext.InputEventsNextMouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7644) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-962f1d1e44dd461e"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiInputEvent> Inno.Native.ImGui.ImGuiContext.InputEventsQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccebd238642b7060"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiInputEvent> Inno.Native.ImGui.ImGuiContext.InputEventsTrail`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a202570acf1c4090"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedState Inno.Native.ImGui.ImGuiContext.InputTextDeactivatedState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6846b848542b600a"></a>[`public Inno.Native.ImGui.ImGuiTextIndex Inno.Native.ImGui.ImGuiContext.InputTextLineIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b5305c66d5c6668"></a>[`public Inno.Native.ImGui.ImFontBaked Inno.Native.ImGui.ImGuiContext.InputTextPasswordFontBackupBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-956c10f14e11275e"></a>[`public Inno.Native.ImGui.ImFontFlags Inno.Native.ImGui.ImGuiContext.InputTextPasswordFontBackupFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f44ff9425f8723c9"></a>[`public Inno.Native.ImGui.ImGuiInputTextState Inno.Native.ImGui.ImGuiContext.InputTextState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a4e4f08f56a14e0"></a>[`public Inno.Native.ImGui.ImGuiIO Inno.Native.ImGui.ImGuiContext.IO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bfcc250701e33c9"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiItemFlags> Inno.Native.ImGui.ImGuiContext.ItemFlagsStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7874) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d9f78b6e1999aa0"></a>[`public byte Inno.Native.ImGui.ImGuiContext.ItemUnclipByLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7675) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c024a00c2a19851a"></a>[`public Inno.Native.ImGui.ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN Inno.Native.ImGui.ImGuiContext.KeysMayBeCharInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c7593b6bbd167f0"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86f87cdacdd4ca7a"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7701) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25de809e7c372ae8"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fc0dd5c14834292"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_100`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7800) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d1196c6a2157014"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_101`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7801) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73b920ef6c828f90"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_102`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1a5e816c2d4d4c9"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_103`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7803) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1827df621aaea497"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_104`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14b727cc148706bd"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_105`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-889b468647d6d3dc"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_106`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75cf52f795da1e69"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_107`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-957090f56ac76762"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_108`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f87c79193bf2947"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_109`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7809) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64b7e63a2a35eb23"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1cb51170205f40c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_110`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f167fc47f5099d5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_111`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7811) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-184a7c8fc6d6af00"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_112`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b80304028a2f3442"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_113`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7813) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bea5e802d102bd5d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_114`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b7bf386c3cf84f9"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_115`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7815) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4fd7e31987ca0e0"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_116`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c387a5a1c2442bf"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_117`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7817) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9113c09013877761"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_118`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-265a68b5acdc0dda"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_119`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7819) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d160bd633db8af02"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81165576fb9cbcb7"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_120`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c98e51b60d2742f7"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_121`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c2939f7cd5c9e3d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_122`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9281e7d0a82d8e86"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_123`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7823) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f1d8cb72fbb0676"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_124`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6894a1dcd63b771"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_125`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d2a1646e1a1404a"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_126`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1277199e6e495ff"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_127`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5a3e6c8a2d01fe8"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51071c61d2a08767"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_129`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5175925f89349a8"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59ea3ec2d83c36a6"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_130`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3b419a9b73ff491"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_131`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1bf72dbf12213cf"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_132`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6d02d9f65263eab"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_133`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99a42eeafacea081"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_134`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e940d18a6c014ff"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_135`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a336a20376d71946"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_136`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-789ea340d5fd2b1d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_137`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28cb765b1e0f5d7e"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_138`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc24a5834dda9393"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_139`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7839) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcb6ccfcfefeb0c4"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91e9cb111e4b7104"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_140`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0788da8d1bff6fd"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_141`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7841) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2616e026581ab1f7"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_142`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a25300bfc7655416"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_143`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7843) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba3b60e790e8b42c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7844) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fbb81763e260c2d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_145`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7845) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59af24bcfe88a2c5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_146`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7846) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0c45889fbb485a8"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_147`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7847) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9e75dd2cae06c00"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_148`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19fa99c1cea10c38"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_149`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7849) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09d748adb713ef55"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7715) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43d0c4c6cdd8ff91"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_150`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7850) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ad66aaa09712189"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_151`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7851) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b422a762f5e8495e"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7852) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eda07dff18e87e06"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_153`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6af5791f1b4d282f"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_154`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34d625b6d189ac47"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ecef3e422ce61eb"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7717) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97b10ebf4843a9d4"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7718) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5744dc60af10ccc"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7719) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4d893d1b0e1118c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-629265127d36fa4e"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7fe9e0178f031b0c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7721) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f961bf670443193d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-353da555a23b7aa2"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7723) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10bc16c55cae81dc"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1431fcb4baa3bdfe"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f087f25109a4892c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f531c36115c25306"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b118c03db5c75fc"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1cba95e0f1163bf0"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eee0a8265dfcb97b"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f3499473d3b9d674"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd2c522bcdc62148"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7731) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-865b7c8e81d2fbbe"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fca59cf1b27ed9f7"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-751cbf6bcc34c50b"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6555ea9a9f7c61f"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e08ef3f00019749b"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-097c109eccf86870"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38373cf2e50a8c27"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-382f58306c5dc49c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7739) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e367e38967c47154"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c6ecc661720c4d4"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33ce6cff43aee2f1"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61f1af2f6aa04142"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f4f2c0787be24de"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7743) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f3253953b67a9d5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7744) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c075ab2473400913"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7745) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afd8bc1237cccbcc"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-704f11cf486d920b"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7747) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e5bb4737ef0cbd2"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc4cb05b5dbd538c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7749) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-798115844e08bfc1"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiContext</code> · 361–540</summary>

<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_50 e7bf44f4a59984a3a0818d4da09d9f41f5338a6de8f31b5f34415f54100af3db -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_51 0650d1b64050dc694b15e309c56e2f8b4090aa99509300c5f03fd4f5789f1a2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_52 1319fa3ff5140e62fa954aa1562fca826d54bcc0c63e867168bc3a134936bbe9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_53 66aa258aefc504fcd90cfa57a39fd9c9fd7fb9f1cdfe65045ec5d5cd7dba549c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_54 8947770c3a9638db58b354882620134f88ab814ed5473e70f1b37191b4e1036a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_55 ba2a43370ea7a3e9520bc445b8ca3d296394a0d431c08d3ab34017e905b33689 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_56 d12ccdf88c34122c0723edf47f36aee1429e1e48e758af466adab8208e5499d1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_57 92d57b01da75e17730ce89c51f2a7effc12679016955593f17b457a0d644f4dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_58 cc680241240dde14206f84f0f1b3168f8f03ddc6c3c8731f85511d0f4f640400 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_59 6aa47974126cdc4edbd4ea1ebb8e6727f822262532b670388df7b058d6471929 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_6 ee5b5a131a8e827b0416433dcd5c77ebc38ce298b2202ae0777c8a085d6bb310 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_60 603d7478a7f3f96cf343c5b8a944e2e405eedb4b5f30b3d4f03fc245c3c2fda9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_61 8884d9d9f58d2c41beec77de1e6b556f026a869a4e95ee836e64095ded17988b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_62 6dfbd16d4f5e229dcf7358b971be26459beb30b4a413502961ad6ce7d9897640 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_63 98f3ddb4981e08ca00975a5528a0dc000ae0e39140f76b70c2e300bf2de0ab6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_64 20c2f0c2eb939530802ecacd57555f99abd70eaa2436b7c51e504c6c5504d612 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_65 b128d7a468c6793e97eeab1dd548e4e095bd58dcd162f24ad42aa7940f6d582a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_66 2d2c4d9269dbf2d9319f478e02a171ff43ea9b1e3d7e2dbcfc147e417b7fc8d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_67 7b9aa6eddbe427c14e42638f60e70f81d4554661f0fc4e305145212a7765399f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_68 f9ad0ae599d922f895e3be3a6074e3d5410f91470892ff67c8083c620be25d2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_69 5fb0cb85c0c766ab42185bd41bbd03bafcb08bb9aaa6acaf1a2f0163e5f6d307 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_7 5f674e89e8bcf340aed6df667d27a6fdf17e86e96aed938ac4cd5605e071c32e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_70 966b09bb2b5c1c038c73947ca624e8031bf4c220a17eed55fbf1c85535fee848 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_71 507f9dd490a06e06338dda7118dda6d1baebb6ab87613afa2baadcbe87866871 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_72 5605dfa78089384a783141f4997d714ac5dfe35ffe8d338849b9483cf71379de -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_73 ecdffc369197a44bbeee41b2153a99a4caed8019fee20dab0d9c9fd4c8e0fac7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_74 b5a3974f13eb02b80afe7bf76bfa2c9c7d7816f57f987ceae32347e12eb725ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_75 aedec7a65b19f39007a0289c87d972bfe78c0b9e33e283c6c0793e95a35786f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_76 f6055508a84cd61873399c087868d0da08895e8c2a43eed128d49cc8cb071b7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_77 de03acdfbe538ac9cdd9ca094595c34ac28c6d4332c0dda1c380faab6650f218 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_78 b7225381a1f22ae8a390d2da8c82c017f616aa9c0db8b36e01f23546243f0ae6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_79 6aee2092ae8af4a0bc15a8cc452933a5cce8f28d6adfc610232988ae74716312 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_8 788698065b1483f7384803ddee095ebf18bdc53241a7c3e138a3188d88bb8bc1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_80 63bc5fc060796d53e46070ae264549d7c7ae27620c092c1ff010a8130b89926a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_81 37ee94671880288cc0a633f9db4024bc6e55c843fffa959271a86fa8eb52675c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_82 f6e5bd1b3756e762a71daf3bf526f9785adbc2b13a7cc0c46abc5f1bb20d564c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_83 a35160d02cd0ad6057d735c3da907481f9c04dd28d19861da179a3e35da04f4e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_84 9f70d51544bd085d11eab1b1eed49e8c5ee068d364e184fa9156667fb2158d94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_85 11dd8ef41268edf10340434be28ff43b99cfc38d61ed7aad5a974ebace108847 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_86 07c5476a63eaeb2f54966939757f204a69adedf50e63d24faeb1c9580fd6759b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_87 b659120f0662086411ab622c1509fb5d7f448658106e16df0d319b87571c4ae1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_88 1a29ccf5cc9354efb70913dcaa2ca5625ad52c32c3712c38dd930da9556a4129 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_89 085acac838f629ab241455115cb1554dd8578bd20cd8f0812c1c475d35202b26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_9 952c75ec734998261e8cb542b6df750f1f984d7b3c79a7da51b01ec78936fe5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_90 d1db2f18541a63b6692a9a1b86a40059f7254e4d14c7e5268deb06b374b7c5b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_91 e640fbcf222ec873c7293dab0f084b12196fc86f59e3c516179bcfce6629b336 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_92 c51f88b2ba834e09dd8c2105826d1003c504994798deea9341f42a454633d7e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_93 d88257f7ac27f04a56d4534fae683603027d6203dbbaca19994ebccb743f50fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_94 0bcdb5dbef6a16e0cf5b9bc9c7f231401ab4857768528f41f4d64751468b1fa6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_95 e2be834719e0646cf77d6d55c4d48bb1ae21f1629beb9a2efae624934a40fbcc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_96 d414c1145a6b2570fe40a114b585ac3b25e89c65d7adde1d1d13e15ad43ad689 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_97 34ad08e80523a1b2d5db7299520ed8cc5d06bb53e68e83d6b35cf5bad5454b75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_98 8ba0ba7e1b1110d7781e7600225aec038d769d9c01d6e5a6f3188d96b7e348ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysOwnerData_99 0f8e27235d54627c003428c791f5112c945c5ab049f3e1a34e8715da5d02f2ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.KeysRoutingTable 4f2661bc365fa7eb4ffd8a5397eba71216e0dee94604bc9cb085ae35196a7ce4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastActiveId 818ab18f2fe0db3eb17c7035fdd7610d631bf9ca741ce2018ef940eb4aba7fac -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastActiveIdTimer 3a50dd94d9f2441abc9a70f6e1ed298c48ac773c87403a3ec81f073796d7ffa9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastItemData 4f6c9c8f9a2d1e0f9ab3dad18e6e947103e7b79ed9795eb71a282a6d4cdcf2b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastKeyboardKeyPressTime 5a6b7fbea07b7c83d2fb25ac98d01df1cdb51c658f274decd00acb96bedeae5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastKeyModsChangeFromNoneTime fc4366972a7f04eb5ddc07f475dff6730370a4ddfddb681bbdaf0b3a3bc384c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LastKeyModsChangeTime e13e251605ce2377a8d90449841b10164bc5c3a5048dc8688ccd9280506a0795 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_0 56b9182d85dfff390cb44aaf327e0b5bf3919a4797adb6b411e9ada8bd92254f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_1 611ebfede869cbd4556e1258efe6b5049e66bb1bc32c94874729b145453fc512 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_10 97111ceacac05c6c786d83ab815b3c86ce2ca41c8c41ef842e4773f7a74c1514 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_11 bc0471fa18ffb00d0b01c93f0981ca6513435ba2a45c3729b473b3a190cbe75e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_12 655efdd76cf42e237f503c03fb49ae514e27f4d00f687389c25943c27218c29c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_2 89f8935d20934112f7b0c1d3f34cd90a44fe8a7cad09256405d48d58d6658de1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_3 419716fc658d9b7cb6160b45a2a746de8970d4ec6a18b28a1153dd026ea15c0e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_4 cad04efea14d1c0a1f7b4a3d38ca14d31928b8e670d05d1201d30fdf2e6fa0b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_5 6af76228f1da8fe542669337badf95aa46ba61c3cb7a002e87d5dc33a51b9ea2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_6 a1e6d6218925f53bbae35265c9e1ca2b676e7c58b5ac077b2ade0d8e2b426600 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_7 e6515abfdf1fe3f6d64cf9758e16aa70741f2579a2230fd3845c0dbe73ae9a5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_8 18102accd09cc997e4a7751ead08182b058c5a72152d158c98297b4ea3a8ec02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LocalizationTable_9 71d391a9e791a961f8927048eb642537fd40e68da575fe8eb1a97253e05e2a7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogBuffer dc12756f022fdcea8f4dfff3393074e562544e02b8426d91477eea4cd3746265 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogDepthRef 8b95caf220e09e046cc43f7516d5f24371efbd02c0df7712e77539c76802ed19 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogDepthToExpand 62d893c93d5167edcd9895a7d6f47c6d5e040c22984c54dc15b3cb15ec38cb49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogDepthToExpandDefault 8d1646aa8bcd03967fe819335bcb73d40d04e70cf3340e495301dbe3fd3fca61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogEnabled dec0b6a9e67c074d26c41b3245e504bf846f2ccc631e92f57c765dd8fbfd9065 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogFile e838a93bc4829cb8c33a043520d21c394eafb743358c74ef7aaf03ec6ff3fd11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogFlags b766e55c9dc51025a9fe13a68af32b591be690463a21116f505205ba33059fd9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogLineFirstItem 0be31a6758e4cdf5e0c7c0b6c46c21fc11444510376ede15598fedd12fd0cb85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogLinePosY 6826624c555e6288a2bf609c36892bc2cea07797bf93a1f7638b017f69059b9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogNextPrefix 50404a70a8d5fe8ea93cafdba86d9c9da356483d951c5486fa7dc33f3c6313ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogNextSuffix 8b5547d1938c1cf55c8fc15480af7e19fff289adbb482afff3fa428f370581cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.LogWindow 36ebcd862edc355b4cf1e95bcd7e590739310a7e87fb00263eff6bd5a01f62b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MenusIdSubmittedThisFrame 90111bf516a48c244613df14ad3ec32b2d4efa3a1dbdb108c16666349ffbcc0f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MouseCursor 9cbb18eafe21b7a9d04273081a1d8aee1b8197aaafedb7700df0f6bb77331ff8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MouseLastHoveredViewport 357f6502e6e388b7f053538f89949c9179d9c0e371ed46c2f091e73020365a0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MouseLastValidPos f685fdcad3f918ce58b0eb1d8a6d587da70a8b8889f480aecb39a480404ef1e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MouseStationaryTimer a7e6b4000aa8e68093d900482a2df134760ac27efa717029bdfebafe9e193325 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MouseViewport 2f8e030320b7819e4850910b198746f01771a44050340ab97cd35cd38b1a3a7d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MovingWindow 32ff4d757e2803d77563ebcb0861741b498ba91bb4f083b19245d2c1c7939311 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MultiSelectStorage fd3584db64aee3003ce431cac842bede13b8806bcdf67329b1a87c8e8f81a9d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MultiSelectTempData 3c2d996ff1d51fd88c55d7d44b64b2d0bf534c07a6679813ad83f57d6571885c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.MultiSelectTempDataStacked eab559aea9b11de9f1b2d84264358de70fd86f3cb21cba931edbaecd745044dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavActivateDownId 0477b27bfa6ee05ab40da26ba06dc07f67360c37bde7ab15af5659ca29493aeb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavActivateFlags 8a49cedd888c59860b1a650aadba054bbc77d48ed6a280c3c8168bd1d1fa783d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavActivateId eaa5ad628fefaf4e725244df73710247bc7de84e7e5a198080d3ff64481c3fa8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavActivatePressedId 265df26c08aed7728fcf4d04ded985a663d272a3535b9df17a75c705e1493745 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavAnyRequest 793dbad314d2809617eabeb4739feaae3dd35a2a398309b46ea1b5298008a0aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavCursorHideFrames a194e4908054da8f09dcad9a309034f4a33c4ddd1e93a802dfbd76875b95c7bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavCursorVisible 16482945b7415001bcd7bac4ffd4e25f4ad4aed51745836802ceeddbad5029f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavFocusRoute 4016e03d6bf15e649c888f075cf39bc5e45447f0f8f27ca326710a80b2801d84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavFocusScopeId 1fb8af206ac5b6b4148a7ab47e2011201884304d4538797068d8073953cdf5de -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavHighlightActivatedId f0de61859a12ff036844747d0e1f7659a3d2af7bce516cb920123390d8df0214 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavHighlightActivatedTimer 19476fdcc7f4439e9bacc45b7f0c0473fc2ce3ca0c42c3b99fe703b0494317b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavHighlightItemUnderNav 5db9b40842499a4f7d37c9a4cf236df608fc0bb3b12e3c3b21b5189291a643ef -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavId c8ce8e07371378ac289dcc1e5adfdebeb6b7b7af467ab50d11d9c8fdaced212b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavIdIsAlive 79fa49350e4cb6b00f8f0356f1ce9474ce3b30ffb7e760fd48b1d11651a55921 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavInitRequest 7c81730d045e1075a019715d26fb48cdb00def96ef693d60436da5eacb588d12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavInitRequestFromMove 9fcf89f1f79b4c45bfd9dcd05057d5a0aba57ef41c216c25007ff9b8c4f59630 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavInitResult 04410fa0d6dddd8e7b4cf37cdb318368abc86d8ccf5aaeb0dbabf883ab669aa0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavInputSource 0e63c5b13e04353d0ae8827f1d67a253dbe7d58207a5a684a23854515da3c8c6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedFromFocusScopeId 50513627a882b576a4eb19dc51ee4837a942a7662440b9716fa8b8cde051d8c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedToFocusScopeId 62fe307159d76ba161d069b7f99862e5bae38b2f8a16d57d0bfd4e40d8ea09df -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedToHasSelectionData 5bd3865cdbec078694e6b877c16b54237a5890b06e98d438b0524e5e4d8f02a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedToId 09771cddf62b2c7c80e1861cbd8ff4da4f7a48f477b279657a5a8e3d6fa554b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedToIsTabbing cdaef68766ac6221afdd1fc5f2a858192e79a0599ca10ec3d5a672d2d95e60c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavJustMovedToKeyMods 5c6b605fff4fa145065e534aad8e5ab9bd236ca0b6f74b5604b6b0bca325bb6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavLastValidSelectionUserData 10aae5e3a2b7d7e0a6b8cca3ceb0f6646f7564af1aca0eb37e9132b068cc60ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavLayer dfaab68dd0ea86baf421d1b251c38973f03aa203b386f7a191a4be38b55c739e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMousePosDirty 6d2e21c75795e19fe837700777f926f3c09ff868028dddc753d89cf5d142ce14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveClipDir e36ab93aee7707867ffc602b88982ab492533ce3148ef03bb92ed1624b3b9b6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveDir dfbcaa736487d38c74d27a07c39ee416d229003f325e0bcb6f9a21861cf8ce37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveDirForDebug 47216701e89dc70b10bb3e924bcc609da5bae4cde15a1c4c231a3ff77d0259cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveFlags 17a888a18630ccf611b089c2142c071fb728a35a716abbb59acf5ad2500b5a3b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveForwardToNextFrame 0423767cd5022d53a0c72895a45c72c0b2414df246090635c47b4fdf8f894168 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveKeyMods a12492ab9daa35073a83f4d6be39f0b42ef8401e0d1598eb0536d0ea1ab64913 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveResultLocal a0135c0688af4078da84f7cfeea538f8a4f72c2fca0aacb560178e6485fa3430 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveResultLocalVisible 5a9badce19fbda5377bc1e21b6eacd4a3cc32cfb3263919aca6b79ce9d184433 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveResultOther 956048ef5022c3b89e5c16eff206375f0d82ba248146160992b882a926404e45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveScoringItems bbaa227124458ad33c8060808e2cfdc3084c5cba9521bde88676430664e10e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveScrollFlags fbc2640dc7435e7a53fd5e83fe80f1cb921d502477f8fed05eb0c08e75ee8ae8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavMoveSubmitted ac7dc1c9a9898a5eaaed95101e22f1b2273693814a8db8afcf176ea6c137f8b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavNextActivateFlags 68964326a9331170cc182fb61b498e0653a27beb55d1cb58aa9b51ee5e284a56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavNextActivateId 558331b2333a56c8e293960978f522048d38a35b38decf9086cccc652b277feb -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavScoringDebugCount 7f698487ae75e1ebee2c2025534d3c278e1e267ce4cb6acd0666d2d2b1f0ac98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavScoringNoClipRect 510637d6f9a8412e8a87c9d92a519f81d78edf341f94a3ab5485936d6b590fd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavScoringRect b28847a0c64a643f7966fa5211ba2ce301e707fc668a4e2438c53d307efdedc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavTabbingCounter 6cf602b903ca193fe8b1af41e61be69a662c7a05d5162305b0902694952c35f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavTabbingDir 03341e7c8ac8a1252766344c1832bdbdc35a8fe96b3bdff28e87ab349a86a0c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavTabbingResultFirst b22ffc302b6ce244b57e4b651b15526bf4e7219b864db50072dd7a4f190a6c3a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindow 19536328462afc1db962aa57f1d702b69aa13d44a6f3dfdd7c466b9fa306e6bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingAccumDeltaPos f095e22e21e7599d21f68a3f513ff9b61c2fc9d8624d7c6e3926c5dd151f7e07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingAccumDeltaSize 8387249c38eb9df74863d3470d2fb8ae6aebe2bc891d6623b667b13df5fc42bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingHighlightAlpha 3a9a358862a9442b4fc02dd9656ea16e6b8dadfbcf7901e0af1208346e856cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingInputSource 7b59f59115a48a7148c816de2e04b0ba5a999db07b60ff9ffed52930d986900e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingListWindow e899b687292e21554b11b612a3869273ec5eb8dff599a81329735b1f32a91aa8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingTarget dc405afca8c79abed63cce50ef89ea73a276aaf3397e7c2bf56539a03491a1a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingTargetAnim 8f006bab5f040064a30981a29a947f07499b09fba979d8625f85fc067b72a155 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingTimer b6c6eec430676e439bb18215b19859fbfc23dbd671a24a632d8f4e9f086e1d94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingToggleKey bf285d8c1a39c7657a4ba2372d408d85c45da9d19ee8acb7ab5c5d7ad3b7ca17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NavWindowingToggleLayer d723d5ca2bb61a123f5f16a1c8a7e89635fac9150e8380b88c5d061305ac5c14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NextItemData 818b3f5cb581af0a4d0d2b9745c095d6d3e29ade5a30c7a492d55283b58ba8dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.NextWindowData d0b2a27e439b876675c4ff4b6a18d0811db25e4c93813b0934594edfa08840df -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.OpenPopupStack e1863dba55986d881a84e8b822be282b689f64fa36b09fe62d51ade499003876 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformImeData 3ba80e5ffca0ca3e52f5d38ebc94fbb34f389d75f2fe71cfd06a7eeefd0b2b28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformImeDataPrev c3c012179095f60015f3699ef6eec649bb160420c3fae05b16504794f359cfbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformIO 9e9e616cd9e3dcb38d96efc35ba79a6372b3efd21a77f7fa8ee2e6a24c814b80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformLastFocusedViewportId f56df23667636940d1cfc99cd8cb4e58ff8a9245c066385e9a97d977bf453d20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformMonitorsFullWorkRect e5b4250e26f09db8e48660ed652e9a6723225bd2b3aca89de4d5bfa4eead3966 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.PlatformWindowsCreatedCount e67ec17afb3c3b531127f73b199de82fbd41dd0aaeb692c686fe49703aeed88d -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ScrollbarClickDeltaToGrabCenter 208e28bf795f1391bd63b02f5abf6423b5738c91f6ad2b83735c4c5568f01bd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ScrollbarSeekMode 61e4b8fdfc14de893ac3e057180c2b43be1e3944c7ca37b407fa4769f3f30afe -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsDirtyTimer e5d57b1801ba6d2acd9db542dac5461a2625eb4a196a02598b03fdf4f6069f78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsHandlers 51c78614c720e6dde07b084ce8d9617c305736fac5567a521f438114ccbc2ef6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsIniData 3e70d5b77c7d38b3d83ff238cb5348d0c64b1ef8b3e49d1576fb9751dc221e59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsLoaded 5ff63203e15692d8ec45dd9f74502ffa0cd79cb9730b70a1ea515e4a976e1c72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsTables d4ae7d62a6b629c2ceaddb4f6d2e0470b278f2892fed8b840169e314a778dc7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SettingsWindows d6c9e525d4ad205fd372008b9cb0f73cddcf5e806e5a0949f1c9f7dca36d745b -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.ShrinkWidthBuffer 1b6956158e47661c39e807d5107f118785797ec9128151f7484ad0bf835e7234 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SliderCurrentAccum a125091f39a652df74eaef53bc6ce96e27bcfeb89ff57c4e7eab676c9b744bd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SliderCurrentAccumDirty 32f5caa598f355600a53eb5e16fd5b45f6dc46595040fda56abed8a49cbb866a -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.SliderGrabClickOffset 698d05d0b06e3c3cd168188d5f870b967678efeaade7c989bcb14d11c431206f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.StackSizesInBeginForCurrentWindow 68cf42a0694e5ec9d0d6ed1a04127e4d84c439b0822ac22d91424fdf28aff497 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.StackSizesInNewFrame 973f6ff70a599a965d75adaa5bffedd6275da0cc5052864ce3ab58e571e69a69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.Style 438d1d53b6b0188e22899cc4dfc7f4ec0f1fa785e806577a83deff08a7c47ec1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.StyleVarStack 7c456bc773f209518136d223227615930d596f0a9dffb045112fce427ac54abc -->
<!-- api: F:Inno.Native.ImGui.ImGuiContext.TabBars 94e86ecb120c53dda2ddf8b547360c595b5394666c7733beb88251e6779553c1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3028118d3059c63d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7750) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70fe208f8f9cdbd0"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-131e8566df385758"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07d2f06e01de05e6"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-caa359ac9d9e9a91"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30ecf6e66b265faf"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4737d8eb0da8de12"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a2af89245c90f59"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de8b93c6734a03d7"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74728ac8b6e6ddb9"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23650ff4d43c6ae0"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5b3e30c544de7be"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3f86d837881a41d"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-335b6e6cd2264f66"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fd366a8bfd7bfe8"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bb2d5fbe8bd1d54"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae483fd6b1b6d683"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_65`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f87bc5474e4f735a"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_66`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c64381bd7c0d96eb"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_67`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ac9fcd3fc38eece"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_68`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f622bf9f26b84892"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_69`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c866402b5059dbd5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe2e9b2b1917e091"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_70`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7770) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba6df33829f0147b"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_71`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7771) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e942cae312185e48"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_72`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7772) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5594c7aaedf077f"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_73`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7773) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e542b9365eef22ec"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_74`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7774) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8f8804662a242e1"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_75`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7775) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5be9f81aacce24a5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_76`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7776) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69551e68fad480f6"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_77`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7777) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1f2e4f793a0bf54"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_78`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7778) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-567006b0f3602bc1"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_79`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7779) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b9fe26b4987ea20"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5bf95bff028ff70"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_80`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7780) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07f6c16afbda36c5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_81`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7781) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a5a5db92f64a509"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_82`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7782) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ff2a27cf48e7475"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_83`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7783) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcc3392d85f718bd"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_84`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdc89cb8d10169cd"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_85`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7785) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11b67c88b3528362"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_86`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7786) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4bbaf6d7a6b26d6"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_87`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7787) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-142db0fe9834a808"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_88`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7788) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62a10d7677f1517a"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_89`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7789) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d570bb880191c8c1"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bc89e9b1557e43c"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_90`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7790) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05b03e1b4fb78eff"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_91`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7791) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a1fdd80b662e7ab"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_92`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7792) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92416875ac7ab611"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_93`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7793) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14da0d09c8279757"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_94`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7794) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19e692dc0fbc09f5"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_95`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7795) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ba946105dfb39bb"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_96`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7796) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c0d3c44015c5890"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_97`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7797) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c44e970e35c4cd94"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_98`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7798) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-542affe95c548fda"></a>[`public Inno.Native.ImGui.ImGuiKeyOwnerData Inno.Native.ImGui.ImGuiContext.KeysOwnerData_99`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7799) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9504768034092e71"></a>[`public Inno.Native.ImGui.ImGuiKeyRoutingTable Inno.Native.ImGui.ImGuiContext.KeysRoutingTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4731520b4fa4a1ff"></a>[`public uint Inno.Native.ImGui.ImGuiContext.LastActiveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6db4dd6d2daaaa7"></a>[`public float Inno.Native.ImGui.ImGuiContext.LastActiveIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7695) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccd1d77ebe446791"></a>[`public Inno.Native.ImGui.ImGuiLastItemData Inno.Native.ImGui.ImGuiContext.LastItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ab720635f6844ce"></a>[`public double Inno.Native.ImGui.ImGuiContext.LastKeyboardKeyPressTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7e94a8e9620dfcb"></a>[`public double Inno.Native.ImGui.ImGuiContext.LastKeyModsChangeFromNoneTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b8ab1216046c890"></a>[`public double Inno.Native.ImGui.ImGuiContext.LastKeyModsChangeTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27e25da4298f4a9d"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4bc2c26e7505a4ba"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-589439b6b2c56a63"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ef8aba8dc378d3c"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e323db91755bc30c"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b88bddd0b6ed8789"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b10a5d7dab80768"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f03935fb2eddd3f5"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb4509d0f694035b"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c88e6f6713268287"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6190467f1cdc7304"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c777b35691b34bf4"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bff1fcd9be40781"></a>[`public BGCS.Runtime.Pointer<byte> Inno.Native.ImGui.ImGuiContext.LocalizationTable_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-743c8b72c10c2c42"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiContext.LogBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a309f7d2290c9386"></a>[`public int Inno.Native.ImGui.ImGuiContext.LogDepthRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20071bd405205a87"></a>[`public int Inno.Native.ImGui.ImGuiContext.LogDepthToExpand`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67e5796ae2c54eab"></a>[`public int Inno.Native.ImGui.ImGuiContext.LogDepthToExpandDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03d15ac68f7726dd"></a>[`public byte Inno.Native.ImGui.ImGuiContext.LogEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f895d9806813544"></a>[`public Inno.Native.ImGui.Iobuf* Inno.Native.ImGui.ImGuiContext.LogFile`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7101a3c4fe88dbb8"></a>[`public Inno.Native.ImGui.ImGuiLogFlags Inno.Native.ImGui.ImGuiContext.LogFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddce2feb253aa1ae"></a>[`public byte Inno.Native.ImGui.ImGuiContext.LogLineFirstItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f749c6dca2f33c7"></a>[`public float Inno.Native.ImGui.ImGuiContext.LogLinePosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6398ddae8863957"></a>[`public byte* Inno.Native.ImGui.ImGuiContext.LogNextPrefix`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8b0bd627d6038ef"></a>[`public byte* Inno.Native.ImGui.ImGuiContext.LogNextSuffix`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-593323fe60bce744"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.LogWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f61bb99db50bac0d"></a>[`public Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImGuiContext.MenusIdSubmittedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8048) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f46ede4027a7e6e"></a>[`public Inno.Native.ImGui.ImGuiMouseCursor Inno.Native.ImGui.ImGuiContext.MouseCursor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bfd52e9eb84c0204"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiContext.MouseLastHoveredViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7882) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22e726c0b28e2ba9"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.MouseLastValidPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08d9b66a65e372fe"></a>[`public float Inno.Native.ImGui.ImGuiContext.MouseStationaryTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2558850b86208ef2"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiContext.MouseViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88fa98adf0999d98"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.MovingWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa2728cc478c1760"></a>[`public Inno.Native.ImGui.ImPoolImGuiMultiSelectState Inno.Native.ImGui.ImGuiContext.MultiSelectStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c34f39d6013a2ad6"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiMultiSelectTempData> Inno.Native.ImGui.ImGuiContext.MultiSelectTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8620b873a2f5dfc7"></a>[`public int Inno.Native.ImGui.ImGuiContext.MultiSelectTempDataStacked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-996f82e61a249804"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavActivateDownId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7898) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4a0633f0dc03ae9"></a>[`public Inno.Native.ImGui.ImGuiActivateFlags Inno.Native.ImGui.ImGuiContext.NavActivateFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bde171a3c27266a"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavActivateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b445b7802adacf98"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavActivatePressedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7899) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee977d7eb9fe947f"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavAnyRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4779a8caa15a79d7"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavCursorHideFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bda70f94a0dd8532"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavCursorVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67ad1f98db37c759"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiFocusScopeData> Inno.Native.ImGui.ImGuiContext.NavFocusRoute`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7901) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db490bad24d9b04a"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93226ea47fa23eb4"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavHighlightActivatedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da09207e855b27b6"></a>[`public float Inno.Native.ImGui.ImGuiContext.NavHighlightActivatedTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1ad49e937652525"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavHighlightItemUnderNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be21c35dab42261a"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccfceee92645c7d7"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-852cfe9158b0725f"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavInitRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ece3dbf08d1b0263"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavInitRequestFromMove`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd4981519173a37e"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiContext.NavInitResult`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5314a83c80c7a41"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiContext.NavInputSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8453741dabebbad5"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavJustMovedFromFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0464b80a85677c3c"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavJustMovedToFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5dae6298944a7c5f"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavJustMovedToHasSelectionData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44b38bc503f2d9a3"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavJustMovedToId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3510173644db50d1"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavJustMovedToIsTabbing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7c11c5d0d6f3644"></a>[`public int Inno.Native.ImGui.ImGuiContext.NavJustMovedToKeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-999a7fb0e0f8c4dd"></a>[`public long Inno.Native.ImGui.ImGuiContext.NavLastValidSelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa7ac8da9838fff2"></a>[`public Inno.Native.ImGui.ImGuiNavLayer Inno.Native.ImGui.ImGuiContext.NavLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7d61d02531af95b"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavMousePosDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c05d587b76bd3ab"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiContext.NavMoveClipDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7921) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6213158484b23472"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiContext.NavMoveDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7919) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fc514a263822d67"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiContext.NavMoveDirForDebug`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7920) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98d8e47624d1e002"></a>[`public Inno.Native.ImGui.ImGuiNavMoveFlags Inno.Native.ImGui.ImGuiContext.NavMoveFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7916) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-914cb8ca718d302a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavMoveForwardToNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7915) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-850004e1936c6c04"></a>[`public int Inno.Native.ImGui.ImGuiContext.NavMoveKeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7918) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1116ba17d1396ae8"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiContext.NavMoveResultLocal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db3e86c374a2e555"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiContext.NavMoveResultLocalVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7928) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eac8ac9e810244a5"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiContext.NavMoveResultOther`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5e4f711fa6ed3f6"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavMoveScoringItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7914) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-103e6d832ebac54c"></a>[`public Inno.Native.ImGui.ImGuiScrollFlags Inno.Native.ImGui.ImGuiContext.NavMoveScrollFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbf724b599358855"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavMoveSubmitted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7913) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62937b4eec9e7877"></a>[`public Inno.Native.ImGui.ImGuiActivateFlags Inno.Native.ImGui.ImGuiContext.NavNextActivateFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b18c0d1ead84000e"></a>[`public uint Inno.Native.ImGui.ImGuiContext.NavNextActivateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-410a0171b3dd8e87"></a>[`public int Inno.Native.ImGui.ImGuiContext.NavScoringDebugCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-559eac440bc44956"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiContext.NavScoringNoClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-729cf06d6bf56cb6"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiContext.NavScoringRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7922) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-456d0ab7d898ac2e"></a>[`public int Inno.Native.ImGui.ImGuiContext.NavTabbingCounter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7926) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-253e0ca4c6956e20"></a>[`public int Inno.Native.ImGui.ImGuiContext.NavTabbingDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b11706e4d5714e4"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiContext.NavTabbingResultFirst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60d65417c5a395da"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.NavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5be61ddfb0e9483e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.NavWindowingAccumDeltaPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd00fd32e3a253da"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.NavWindowingAccumDeltaSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f83b4b75a6d1d13"></a>[`public float Inno.Native.ImGui.ImGuiContext.NavWindowingHighlightAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d6a68edf627f56f"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiContext.NavWindowingInputSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f15fd2df0a51e11"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.NavWindowingListWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54a3d5946a6df8eb"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.NavWindowingTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e82cb8fd4880db21"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiContext.NavWindowingTargetAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4196a0ac4db9e275"></a>[`public float Inno.Native.ImGui.ImGuiContext.NavWindowingTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7a4cd2aff9d064c"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiContext.NavWindowingToggleKey`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64794b502284f28c"></a>[`public byte Inno.Native.ImGui.ImGuiContext.NavWindowingToggleLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d7f97d994977d30"></a>[`public Inno.Native.ImGui.ImGuiNextItemData Inno.Native.ImGui.ImGuiContext.NextItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9ac40ffe42c3732"></a>[`public Inno.Native.ImGui.ImGuiNextWindowData Inno.Native.ImGui.ImGuiContext.NextWindowData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33dce81e8f7d3a62"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPopupData> Inno.Native.ImGui.ImGuiContext.OpenPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7876) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9042b9bda9ae5142"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData Inno.Native.ImGui.ImGuiContext.PlatformImeData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8050) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-088899a2c241786c"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData Inno.Native.ImGui.ImGuiContext.PlatformImeDataPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64a8caf77c722de4"></a>[`public Inno.Native.ImGui.ImGuiPlatformIO Inno.Native.ImGui.ImGuiContext.PlatformIO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f603f300683e1a6"></a>[`public uint Inno.Native.ImGui.ImGuiContext.PlatformLastFocusedViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7883) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bcfd5a31aad6aec"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiContext.PlatformMonitorsFullWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a08d37cddc4c643"></a>[`public int Inno.Native.ImGui.ImGuiContext.PlatformWindowsCreatedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6938d00adffe139e"></a>[`public float Inno.Native.ImGui.ImGuiContext.ScrollbarClickDeltaToGrabCenter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8036) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9356b87a66ca5cc2"></a>[`public short Inno.Native.ImGui.ImGuiContext.ScrollbarSeekMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8035) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68bce1d58cdeadcd"></a>[`public float Inno.Native.ImGui.ImGuiContext.SettingsDirtyTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-26750d23843f2fe6"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiSettingsHandler> Inno.Native.ImGui.ImGuiContext.SettingsHandlers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8058) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac5dc8016224d348"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiContext.SettingsIniData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8057) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37fc08dd414cb406"></a>[`public byte Inno.Native.ImGui.ImGuiContext.SettingsLoaded`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-306ac2b6c2d806f5"></a>[`public Inno.Native.ImGui.ImChunkStreamImGuiTableSettings Inno.Native.ImGui.ImGuiContext.SettingsTables`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58ecbdf33bb50f4a"></a>[`public Inno.Native.ImGui.ImChunkStreamImGuiWindowSettings Inno.Native.ImGui.ImGuiContext.SettingsWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8059) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95b164c700c75b7d"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiShrinkWidthItem> Inno.Native.ImGui.ImGuiContext.ShrinkWidthBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-608ed33c8236b9f6"></a>[`public float Inno.Native.ImGui.ImGuiContext.SliderCurrentAccum`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8038) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-24c3e62b4f06bd7a"></a>[`public byte Inno.Native.ImGui.ImGuiContext.SliderCurrentAccumDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8039) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bb7f2babb885c75"></a>[`public float Inno.Native.ImGui.ImGuiContext.SliderGrabClickOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8037) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b698ca97b321440"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState* Inno.Native.ImGui.ImGuiContext.StackSizesInBeginForCurrentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dba8051d29d4a24c"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState Inno.Native.ImGui.ImGuiContext.StackSizesInNewFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58e97da4a498ea33"></a>[`public Inno.Native.ImGui.ImGuiStyle Inno.Native.ImGui.ImGuiContext.Style`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf5ab6cdd82341be"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStyleMod> Inno.Native.ImGui.ImGuiContext.StyleVarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7871) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62cdcdb57f9170ef"></a>[`public Inno.Native.ImGui.ImPoolImGuiTabBar Inno.Native.ImGui.ImGuiContext.TabBars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |

</details>
