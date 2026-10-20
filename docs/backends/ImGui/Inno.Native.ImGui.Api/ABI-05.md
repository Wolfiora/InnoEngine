# Inno.Native.ImGui ABI · 05

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.LineCount 31b0d68c24ba3d918515f43cb4a6dce23753334441086696d4ebaca8a7dc535c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionEnd ed1d25967ab4e09443b1ce05969c8a7a151771ff5c6552b068d21f02e3f5628f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionStart ae1c6b449f35a20f0216aef9656a69fb4002f5e09c5f84f86b88201d3fb052a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Scroll 7131a9884f32b83e19e34246754fd78a638d68460cd5e5e39ada9253bf00f8f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.SelectedAllMouseLock b569ed9c899a4c5ac17b4e8ed50a6c30e22ac0b46529bddd1893b9b65916e152 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Stb 0fe2c0991e1a321afb0fa0161aa654545d6e5237c6cd47ee51b3a03fa7b2e34b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextA 8dc6f082ceab169645c3b83e572f0285a4ed7346e51f0b43a0266da6b6dc25da -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextLen a24db05f880e63e956d660294baaa3d01533b0d4c0db9b5f411d6676aa327e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextSrc 78e147d3d72554a8ea8db2c0ffe3eba05d98fb5bb4376a3f602fb5df2c7456ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextToRevertTo 378198f3dbf26459d6bb8b655f199512842af4b5f0e6296f545baedbf1d0dc3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.WantReloadUserBuf 4db46609c314bfc9529b386bbb7f3605bb4fdaee0eba5432dd543defce715bb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.WrapWidth da0e32f9149449c1b67d033336bc1b6d2865d26fe8aed8b161dfab4455d5fcc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.AllowDuplicateId f7cf4498873d673845f58e791b5536bcc80b2684bd3b072e90107e1683085fca -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.AutoClosePopups 3545cb7b93ac704f3120dd1a5e0097b08fdabfb81162d3fc8d039ac3a763795c -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.ButtonRepeat c8f47729215ab1688c4aaa353eb31abe2da2d1b1a016ccdd00eca310d8f5e3a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.Disabled fe216da5787ba8f8c35db5fd89f52169d550db019769746cea063e69b676bcbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoNav 6d291fb200e3c7f5918cf9e2557da21c5aaf35ada653a9610f90088c1a4b7a5e -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoNavDefaultFocus e4bcbb27118bfdb349eb9ef96c4a308581bbaad677060f5c55e0fba5ef53adf9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.NoTabStop 46cbd523d6e3c8dff843ed8a5137e442789afa74d2507430e6d6df9f6d7b861f -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlags.None c28fdf200a21a53a91afcc3b595353de63fe1a337247a4d8fbd7c44693ace665 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.AllowOverlap 41a1abcd7a2092aac0d70a3cf0957055a023580f4acfacccd1fbf9e6298f263b -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.Default c307b0abf7210d71c14d90bd351079e1dbd87537bd6df7ce28a72715d77242d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.HasSelectionUserData 4ce711ad3956932c5404f5bb9782775a1542ed14ed485d94c71bf8de6409b783 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.Inputable b11737fcfba3702e4d8057f610e14d22efa498f4205059e889d4bab3cfe9cbf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.IsMultiSelect 08bd515d9ad14d37aef8d421bfcc8dcf3d4681588172bdafa48457f72fc02635 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.MixedValue a1c07b6bace148be2eec5330bb176ca356f42f3aa793be8d6b3f44ec207eee47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoFocus 93b4b36294c1e5ac8f52db5d22098b4b8ea03989e49a680d17190afea2afbc88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoMarkEdited f8371bffabdc35faf5b35f027554c3a6e9e1f1851352f6144934b8894df42130 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoNavDisableMouseHover d01529a621b28b6420a6233ad8c5239ed5aa8b6b0f93d6ed077d9263e61af8a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoWindowHoverableCheck 06d689b7f79093addc13d888b10400c82eabdce9ad56c4415b989c7e5a229be7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemFlagsPrivate.ReadOnly a662a934b36d956f654cac4a53dca4a4e93f9464883d080319a7c0d1c5bd6d31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Deactivated 6f55bdcaeac5cafc0149acd3b357318486ba1a664aece960c761bad1b1c58152 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Edited ac7fef635a7e4c5e0713a3e61fae8b84c76553d7b70b331086c11db5ae802f6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasClipRect c7e698c3f7b51229c0fefe79c90051455d75b4ccedbd17e2fea1dd3b93b013cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasDeactivated fb654fe396e2c50cebb96a0d18458695cfb48d2f7ac35529e7453b0ccb3d064b -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasDisplayRect b353826eeca527d33ddc03b55038c3298e573621b69993ee5db597307d988844 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HasShortcut b2995824d3448932dd744e0a391610e0cd9962172c814b1461253ddcaf663cb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredRect 546545768099a88f4ab87cf146eac433ab2289a99796239cf755636eb3c15eb9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredWindow c6581d2eceb5c1cb3b5d31c8b9d840201bc16eed5cacfe3d31ae23ed4e878696 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.None a68ce50f8fe5d2a477d01d15e11effb06878f2d05253478aa172973394e534f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledOpen 07df7f227bbfe27557cbe1ac096c21a8bd0553da752880ab5dd5c04375142f0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledSelection 5dfb81b5e56c21399501abf60cd97af1c66236bfb46fccf52a3353c9fdb0e04f -->
<!-- api: F:Inno.Native.ImGui.ImGuiItemStatusFlags.Visible f1b12d964625a4140dfc1bf80e6087fbf835a78cafd7020ca327185de7bc3427 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.A 10f5c065d46fb83d8467a52bf275ec6e58771516f551167653da430cfeb6ccad -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Apostrophe 9ef879e5e950135ef54c4e73155e47e225c5cfc425df052a75f0fa532ea5062d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.AppBack 980befd95f9bbacf79313f16adb5985809dd486300ca2c81b86c966567224fb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.AppForward 97e240227a3a18c1d9317f49fbf231a0a0a940e0bdc2cae88ae75272688a6540 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.B 86c415ba393c87114e686d658b8a9ccc03e3427d3f278a18effa141c06eb803d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Backslash 60e0566b4142b73aa3b5267624d997b0a28cb8d9c86d8902d8d0f87a0406c124 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Backspace 97c6a5516f50787e6bdcfd1b65c34c4e7dacd98783b82ca2fb6c07e8a962f4f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.C 98f28547d109f779b9859525a86d62f4797a76a1f95b437b92c51bb7f02769a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.CapsLock 3c6dece1a10d5c27e3cd88107164d9045d2169158f4789327a4aca89a875defd -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Comma cb0603451eaf79f65b9d310194b4ce42a81291033c5c7931215e34fa76066133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.D 8eed7023d425a113741381193bc3f66c677bc223efae1f2baf330094f56f97b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Delete 43347a87deeb8347d09af8e0705f65d4ab85b259ff921562bbe42d1cd70eb3fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.DownArrow 747d2ca475b03c5d857ca5b4859dee793936d036958a2e4361596be7726d000e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.E 64e97c1f9ed82865f6f39e44132db582baea13fa43be7f375d4fae59c0febb71 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.End c267d555da614aceb54bb02232067fb9d51507be33926307eac5723e00454bbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Enter 7f053126380551e2266bf9dc11aba33d2f8fd53aacdf77d9575e71546b991ab7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Equal ffc1b693445471c910c14d13a76adfdf539680a3aeb2bc03b5b860bab3614352 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Escape b19a1f857cf221538ae1233c90efa82562211555aecf763660adf8823aec8832 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F 6928c19f9bcc86b8f7bd4430798209347fc04fa190c83b6a771220f2949ea43e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F1 49b4bacf1123a55823298edcfd14986ce38f1dbe3d96f74b435274eb41bf37f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F10 3d4e6ed5fb0e61b581b5246dd44b9587ade669f842c36d83c594a3d4e82ec458 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F11 abeb6161ccc9310eda77c90f613e93c49096aa9fdecc28bd59d86d2010d3e36f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F12 7f4e911c6cdda0facebf9b22d0f2577e563c44fdaf9d51102a4fbbae7e3a9c53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F13 972cc1acf02c7aaa099036fb4c79d632f65a3df738fd93d925a79bad4d675dd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F14 922f48024f2f0d6753892c93c39af94001d559fb596be306613857adfced31c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F15 3c763e8d2b2da0ba3972525657592113282ab83c716046a58ab72d88f447ee37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F16 cae0f0f5ba92664b1e288f5fcee0bba9e0750a292df88a84a5c5b203fb94a943 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F17 5d18a097793de3c0b58bef31072617b3e8d46e06cf9acc586900b2070fc9f563 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F18 2f787244ed8fbcd6d574984d78a50a76f0fb96b792a32fc0372d7273870178c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F19 774dcde4879df17921da57c7425cdaebae452f9f952c53107ee0d0cb9ae74718 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F2 59e1c5b439e793f8dc45109b5ee08fc3130eca2d2bac8f760782ae8bee8127ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F20 3a2202117bd305275629a91eca713f247f28e314c353fe4a11c0b24419da04ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F21 1032c0788dd37baf2ce33e59456f47a3ed359be59774bd1d691bcc895eb75382 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F22 03977743c5ad9522209e504afeb61b1b9718b62bbd84edb1b9e1bfe6ecf481b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F23 24832932e6975f73c7f7bae4e4d7f2b4c642d71f9bf994f24ba91f325061c46a -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F24 1b8d817778203bd4d5cc5d2c36110d5c5376a7772741081e5c338a17b53f5696 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F3 feefbb5dadbd57a68b8d02c88c3796b59f082957ce3f05c2db49b111ed93856e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F4 33ee741264946f2bc2b566945345266b1747ba0d3a4259f566b6a321e1d657b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F5 795e8233351d922cc57328441c1bfec98dbfaa195ff6de087e22fe841aa3e71e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F6 da1b0bf0e6f160863f36eca66a54c52435afe1b203da5c93214f24294010f77f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F7 cfb07775e6914cb63b49e6083cdb078310c7322034c7062d05fda0a08b2e532f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F8 17bcc3ee7c9d598e63a1855ab2243fc9c3f4a11ec4f0bce685dbd7f4cb8f6736 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.F9 d0fc026786bd32317767a7810a11f555081fde75637d39345967f85cdff627b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.G b9dbf827d4045385523599efc34c69c397e0a51c715206d9e0e1201c407f4549 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadBack 026441b01553b093e80e7e64ba0a4bf9994373f5e276c3dfe3f671f2d8eccba9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadDpadDown 69a8f9e866eff8836b18c28e533aaf75a9f2341be05fae7b066b6a13988445ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadDpadLeft cfe41534e89a2d52e2f50c24ac1a86265a88579b466201a9412ac7bf7da802ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadDpadRight e9fe0e3f0e9ceef5d56297997b3dcb88fb45248b1967e0fc4214a86f1e99e40a -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadDpadUp 95833ae47f20da362fb96fa7b1a9223af84a1f60aabb1db3bc26a09295c381f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadFaceDown 84a5f4dcce6b65f813a7fa5e5e30cdf858b89aa19e62731a6d8d4d32c969eef4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadFaceLeft 713bb20b6375c69ae19715d83aade49b2a10e87c2e90dce98efc07f1f1ab0060 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadFaceRight 804178d9206e3362aa48a2233121eef962f9d82ca9217854f5b8789a93253ed3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadFaceUp 9ca9cba1ce4f164da5e7a037ac4cc6660e110c39efdc9c432cf7910e1b9ed548 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadL1 8b954d21e4fb13ee704a75d96585a9f9c14b761837ec84a540f334178f22defb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadL2 b66330f818b01b56e3b1690963b47fea150fedbd1e6c0f3cf5a5487ed2185c21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadL3 fec2f312125aca163cc0cd54fdf6f8f9ae3961d07fd1224ff7cd41c0b97d1d8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadLStickDown 61db765f8e4df22be65a7427598ce40f39600e6eda1b2c1afe6f2becda830c17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadLStickLeft b172f920ea66783ae83252521460a41757288e1ff9b9ac7411c580b75bd3a618 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadLStickRight 6ff7220d71c9fe54bbc8dd1332f2faf2d753a6205ff0aea637add192a48cab6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadLStickUp 1c35024e94316026a8c3f35557dc6fe9bd1d2309db3918c3324103137c3df471 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadR1 2ee6fc601465158470bec7a51f49c1862e550c73f77a000c3583750c7db98e85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadR2 ebdb00248233c995758cb9dc3cb8805026ebf4aff0b540447f06323b5672a036 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadR3 71ef011d571a1de89b45490e06402451ecdf49182fce3d07ced4b2c59493c80a -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadRStickDown 6a875f9277865dc62ccb57d8b589e849e82f5401135eec60a94597dd19c7324e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadRStickLeft ac04dfd5fa3e200b27d290ea352d00b183b3cdc9338e85ebb921b5efd3136962 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadRStickRight 5e3b892f26303af36daf7110f714e870edb3a9be64239eb14d9a9fefd6487c9d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadRStickUp 57a37fa865cad6a449749063853b7db7a8d904abe252f3ec9fa45eb2427fac34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GamepadStart 9465945a78e9df4af80bf08e1e47d100d7b53b765605a4b20a4c572e72a29be3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.GraveAccent 5abb54698656bdcff45214a0d9e82c4b608951c06989f5e93e7e2a493bf74312 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.H 36536cfcf9e09b4cbc61b7a716bdda9c9be469d1c1d5eeb177a1db93011d3007 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Home 50785efab30e0007caa20269ca25153efa68f8ec0c750c6b4204a6c6ab5a2c67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.I 070e08666db16f2382ef5055fbc41fa4617091ded591ef2cb2f4d80865624a25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModAlt 6ae8b3f3a6926fd7a8d76f5ef642b00c78b5ab18ebc685c66103bf356d02e7d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModCtrl f2268595d7d5d124352b0683b5170a6ed80d2ccb47fb150fdc55728029f1a49f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModMask 4e6876b322c623a04bd95fdd4f39daeb1f68d6e5a55085676ae60ed0b3116329 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModNone 212feeaa16ed4c7542d6bd96301ddb3728ed344bc63728242aca42a809ddd24c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModShift 208bff2d5c23f904da74299404b616643c5afe17f0ee8f3be3cc3333bac9d955 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ImGuiModSuper 011b197a5ef979e06158321db334669d9989a8fd2876dd3dd35ee797302ce5d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Insert 7e4992889c2a33ec322fa9c055d338a1e7dfddb7b4671e8127ee274a0cc069d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.J ed4f828c9e52ea075abb52edac9e665968ef06c25a86b91117678d04decd9fd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.K 3598198f86a38cf9ba9cc5dc50f2b517d826e57f6ca223066fe88c8799cfc16d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad0 009996990285b77facbf8a6d602bb010c17b8c6af1451ed15d09da3e63a4dff1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad1 9b7d60db4ce5f51f58cf7018f074208827860c40ba7ad45b7a8d5ab1726691a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad2 92ab26f64ca906b19030e4355278df32d8372d4285c758f13a869f62d0af0b5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad3 55f3e165132321b5a758646bce86a906e005fcf83786bdef152888f9caeb27df -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad4 da4e7f081650509af8c0508ada42c824010936fc541ca51f2c9219c50bc35356 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad5 a68f78a33f2ed0a2225a213f6aa3a827b3e9310e9c2e617e44041904b1d60c94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad6 02d0aa0898321aa47e3389a7199ab53e853279e407453c06fb16767302c12d89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad7 06fdbf63e7c4d5000d138ecec4aedb51c4df31c4112141262072c41358da1961 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad8 28aed8e63316b8ee0b4486b529f98f14009ab16a8f14a451ebebc03616301e8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Keypad9 e3fedb99f64c32f5cf97b3ebee7b9725cab4fbf07be80bb7130e0c91021b6da1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadAdd 8ff4a134f4550940f0ff8c1952d5b0cfcda46d511da361ab621e141a8998bc14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadDecimal 646d009bf3d979648cfedf7344552288872943ef6612fe7c0062b512148629b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadDivide 2bf476c8795ff9363c4a92f806056f2442f274d2a1055228f5cf8c701c4b63c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadEnter a0e3652778235cdfea09e241fa3f4fc7c77469d34d4f40ae5dda355353e95dbb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadEqual 4bfc2c48b5c664ac9ef11205d746d33d3d8c3124c4127c1be32e12602354a30f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadMultiply df0d25717059650b7efc71e8af90ba5548acd95833fb21231c851d1b47bcc1e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.KeypadSubtract cb5ca2b823b7d019be29327e4f6d674287794f918ee20180a0c22174aee2217c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.L dd61c4cf6fb05fa6fda418699d5a5a49342ace8e8d15495ca211cee2c04329ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftAlt 5747641b5c8f2483e039dfc181fa5db6c5b08da7ebbbfb8cb445948736b93129 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftArrow ae4989ea9ae1e76c8f779f315090e9a25c02cfe77e3039a57b77afc7f58b8322 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftBracket 533e3397e0089e76877b9cb01f3645d4c0283c81c8b8318b20ac652002b63328 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftCtrl f6bfe51b18a162e2b141f881f5af00d87e63fc31a5b350684beaf9f0515adfb4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftShift 2eee1d8aa123fbf46acb09e92768dc86feb808407129b24a57fdd46730c2f3f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.LeftSuper 14cf1ab211aaecb40676e6de46c18304976416248197e31dc27cdc70de6fd061 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.M d2b42f2950fc39162d5a7ad33509478f2e8a0bdea39ee04824e5018782477fda -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Menu dfe5f8a491d3b99b0fd8bc50627efdedaadf98a9220b554f2b04d4b94a233261 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Minus 318d4d2c3696918fe9f0a8af4c335c8d6d96b7c09ca121b5eca200b3d23bc004 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseLeft 4f5e87d28c66b289b70dd5ca114e33e5356508fc7587b62af6e1fd5c59d4281f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseMiddle 2ab5a4a25c1c6bd540a29f74cd03e534d61be5e742137da6546612c48eb3a35b -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseRight a4c0b15789c7be6bed7092cfb3dc47ca178e8529ed39d48be8a253c7d7c850a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseWheelX dae8185b2dad92d4c3ede8ad0c7369cee8612d3eda27f38b7d80e805bfa5bbbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseWheelY 383598c1b3f6d561f6fb59f73657edfa152ac090e0a12b64611b9bf880eff3a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseX1 ba39df49a8081d1605bfa904b694420209af8613043452d5964c22b14cd37b88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.MouseX2 86ab71e165e56d9067963bbb48e90e59c1da5fee7bcb3ae2ffc56db519de8713 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.N f0d2d51944729d02941d934da0a482ce1fcff07d0299a764c5c26c8260eb3949 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.NamedKeyBegin f5f3c7d07ba1d24d89c0b5d98c918c0e9fca584ec21b886ae5474aa313dec6a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.NamedKeyCount a6af752d84926dffafc1d86bff133e0d55901ddc54a0a29c4f7512adc2bf4944 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.NamedKeyEnd 3a0c14c357d346e7d9d52dc8657c7ae82260f201ccf88b7eff5af35851456f74 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.None 646048518d13c52f80ea1062cb736fc8c5f8d3057f71088a9d41e80530954e52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.NumLock 18d527a399f4249cf05118172677ab39af6ced8177e7fa74e57320ec488caea5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.O bcdef98e7b4d63aa683317c37d0a7c24bfdcceda1228339144b1e9260f66ccc9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Oem102 91498ed3871aa079b591f5582de190a663f1033035a76c80a0723205ac92291c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.P 264cef9d8fb109be9171398ad3d2a01ecb93043e952534d42f36bb6c110ab663 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.PageDown 0fceb6ba954cab7bdcb9e5e09aba6e98863e4397634d1ebdf21576394f31b5b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.PageUp 64ff22e391c7ee6d402548f96ed36a5c8e04e06dded87f339b699a5e3b9e3db4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Pause ae166548d92a5f7ff07d55c7fc1920b861e35cd85fbce7eb2b645539bc09229f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Period 3f1935ea9442c2118c4c805536c9ec647ff67e702a30693581787b38a8fc325d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.PrintScreen ac9b9d36f29f200fe3052ccd4dc4758595cead920b6e4d1201e2a364e836fdd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Q a98c7cb776bae9f23459c1d7603652c144681ec7fee32bc216f83002cfbb9fb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.R 0336f319a9e3c6ec315315c6a91217254e3a7f2c762b1c8859059ca2d8b44e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ReservedForModAlt ffc6082724ac1d96b03a05ec9dbfc2f8b43f9cecb32dd9ad8ef8414b57f01c4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ReservedForModCtrl 3f0acd73fdd1128366c9f63017a2c47b0e94489852e5daa32d4bec76dd9be7c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ReservedForModShift 42a67fc0fc77d5dc6c4dc3143fc94479d731f75ae97abddacdf98cd3e8bb5f06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ReservedForModSuper 3f3da7d52b267d328ac699253a8761c72a701ff3d4bdb8666a6ca635e3b0e314 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightAlt 213e63d60802590f2d26b8324ec9b75d38cab32a31ea491380efcc0fb4cd6b8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightArrow 597ad22dfdefd6ce245da9a926a39c64caf81c4bae77efc5378ed477b06e13cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightBracket e9968b841a5d25751a1566a7f8e3d893813e10667c21e3f94e671ed1aef16ec6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightCtrl f449e18175ab957816ed522a97bf061b6d5bd79bdf50141794d14dfa813bd6da -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightShift 0a90a307707b9aebc2320c353321edc8b9d78f89a1b77669aa3d50040855e945 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.RightSuper 81eff2e63f7e163226ad64de90389bb82eb32566a3e6bf13ad35395964be0d2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.S 6220a2216362e8ff4496aed31c586601acf521cfd48eeba8ca0b9198cf39498d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.ScrollLock bec7cbb3e9bd27a489563c6322cf4291a2688b41ab83061e36b262b2fa925425 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Semicolon c745c41f5d04ddbdaa3fc1ddef4ba0e91e7a4cfca5eaa63e0f2b7754e34cfa6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Slash 24ab1e5cf69dd70347dacdce6630a2b4139d8918b47cc9d0df58bf732b0d53cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Space d950a2b0961226bfddb1ca624eb8547199304a4050bc43ab3eef0c2be391c7c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.T b41ebcc47be58edaa5d19e3b311672319bc0967d2bb2b1945859ebb1f8e4a69e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Tab f5872b80cd8df90b4673395cc60e50044c10820fa43fd69ad15515cd3b751c0f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.U 84db1854e78bab8ec3c2fe0db782fafb9b1134643928ecd78490e042b570077a -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.UpArrow 84a3ad7eec2169fa1fd6dcf5d3b7ec5c3306eccca11d5a5d27884ada1c5dbb1f -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.V a138551d2f8ce19ca069e48075ec6c8a5a102d2d750dd033b34363787375a077 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.W 3e39aba5c1f5e958ee061821c426be8b83b150d312b1720015fc4446468afdca -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.X e7039a738db1793f2caadab804bfb40cc4a733bd223a14aeb1738c32a186505c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Y b9db3be40ed24eb4784fc78e8521bc345d82f86d624c615f01a4ed2a864e0515 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey.Z 0aded0546b89bac44f85a962fe0ce9102884fd41702eb1f99cc04234905d2885 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._0 6ce2a7b0125c5a51cd9fea579209b99f0e720c7da02904c36492128c5ced326e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._1 d97897250a0923e7cb58ebaaf269e0c6e4ad0a6045b0f7fafad5ff2ffa9e66df -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._2 4abb8103484ff819e8d153c0c8616decea6935232be83eca4b6475ddf49050ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._3 3c8c5ee3ac9778fa21b8cee4d2945b63fce7519d16fa11b687e00cdca89b63e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._4 b3722a23997d821237568152deddbb521a134859905995f757dcfb8a665182dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._5 066c06750ddbc7509bd001d07567650386942019026abe4e6cb4fabb8ea8a886 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._6 67592dfde285daa61650683258307ddfd6baec08a164985b4d7a8e7e9dd4501c -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._7 dd2259345190e1ac9e0d818609834df54d14cb07700230e82fbaed686c187a20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._8 a1607e33cf34b4f7448b90fcf675619a0aaa1eccb4c01b8c2d4a7092f5ed05ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiKey._9 3a08ebeee0ca94b80517cdfb2bf66ea21a056e18a0c12094c236e524e41e6f0e -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyData.AnalogValue 1b380438a80944c39490ca1b7525b7e99399704b81a617bac482f577b403f6fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyData.Down 0c1b515638a2d394a90c4ba945f2022f16fe21490b35d111e15e278e4dde2781 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyData.DownDuration 27ec4fe5488fabc0ea5474816b58a49fe710872fd324b85658a538afcbeccf16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyData.DownDurationPrev ec69b806e7c6fae6dde08f2dea1a9da4fb17192ba767aee873ca3c85f4dbadbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyOwnerData.LockThisFrame f9e876640385c4bd223bd5d98ff7aef557f59bf2def558a38fe392c2582b0fe0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyOwnerData.LockUntilRelease c2b86ea6ddc8980fca8744bb3606265f75118e694422ac731f4fe4cb8adbea71 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyOwnerData.OwnerCurr 4fa49ae72357f4f89d109ccf0ee8504d4b28fade88a43adb80bf5b8821dc0521 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyOwnerData.OwnerNext 0c9bb8c9b835da4645c37ecc60a0dc2b7ab25ef146061d99017921e484b06ba0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.Mods fdf5893be9e92df5a3cd6e5972f86928d051f821f6f48a0e3a0736bf54f6b878 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.NextEntryIndex c3084574e32b431795658d95b8c33452291cbaa2f58e85c1eab35ccda4f5abbb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingCurr 473c0abe771c66a3f74ef12dbdc2e2e8fea282669ff5f0cddd988232d5bb5c21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingCurrScore 3bb450b95e25e56e299d3b783c6cd63cf8466b6b1b1fff0a5d5700d14a419177 -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingNext 8420af391f03ce6b7b88467c9eb1310b5597af4c93434b98cb2aae7fcdf389dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingNextScore d5c4edef2ce40e0fa8a86badf84fef5be9b2ccc8fabdff0b8ad724e4af81df2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingTable.Entries 7f6d9b58c824e94df036fc2589202574f28b7294fc627cdbc12bf0ebfc4b18fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingTable.EntriesNext b44f42a0ec4daad902f6ca9c183b51c6cce5fb8a2762a68e33cb92e04e1a3fcc -->
<!-- api: F:Inno.Native.ImGui.ImGuiKeyRoutingTable.Index 787c92a13d74a354af8735342455b8b05e063156ac9009172e7be0348d0baac5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.ClipRect f759e793d939fc558418df732a7ea90f90aaca52a5b5bd26c4fc744dddde022b -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.DisplayRect 068f2365e1e815a7343972aeb8da734df46090fa43cc3950e07ee814ce9bb901 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-193a2cd8184b798f"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.LineCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27083) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-302eddccff819aa0"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27103) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc83a2be39e0c88b"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27101) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b80957c4a18f3f5"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiInputTextState.Scroll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3f4a91c2858cd636"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.SelectedAllMouseLock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27093) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42227ebc9b7372c9"></a>[`public Inno.Native.ImGui.STBTexteditState* Inno.Native.ImGui.ImGuiInputTextState.Stb`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27063) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-55cece1f443c7f0b"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiInputTextState.TextA`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27073) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-273c57ebeaa61659"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.TextLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27069) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b151c84a5ca37ac"></a>[`public sbyte* Inno.Native.ImGui.ImGuiInputTextState.TextSrc`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27071) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2431b54e173248ea"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiInputTextState.TextToRevertTo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5bc66a7a04be61b0"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.WantReloadUserBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27097) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9d3b40f31ae7229"></a>[`public float Inno.Native.ImGui.ImGuiInputTextState.WrapWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27085) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fd8f596a6217c5cb"></a>[`Inno.Native.ImGui.ImGuiItemFlags.AllowDuplicateId = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27364) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-6f9e7f588f320d50"></a>[`Inno.Native.ImGui.ImGuiItemFlags.AutoClosePopups = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27363) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-426adf5d44534e32"></a>[`Inno.Native.ImGui.ImGuiItemFlags.ButtonRepeat = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27362) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-5d03c92896ddcc55"></a>[`Inno.Native.ImGui.ImGuiItemFlags.Disabled = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-59bda85d2cbb73f3"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoNav = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27360) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-9e20e19ff412ee61"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoNavDefaultFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27361) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-dde2588c917ae1ca"></a>[`Inno.Native.ImGui.ImGuiItemFlags.NoTabStop = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27359) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-cf78b6930cd62118"></a>[`Inno.Native.ImGui.ImGuiItemFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-1bef239ee3de4c73"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.AllowOverlap = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-ce4248f7f11eb108"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.Default = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-3782ad711838e1c7"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.HasSelectionUserData = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-e50282328a6b8ac1"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.Inputable = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-38648899c15bb3d5"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.IsMultiSelect = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-3f038318182624c9"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.MixedValue = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-5900c743dd53f96b"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoFocus = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-08f9a44837a6c13d"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoMarkEdited = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-0872ba6344aa0ed4"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoNavDisableMouseHover = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27377) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-01703648e58d58bc"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.NoWindowHoverableCheck = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-9930d7a1359e2656"></a>[`Inno.Native.ImGui.ImGuiItemFlagsPrivate.ReadOnly = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-e25e947a1fceadf4"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Deactivated = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-12cbb2baf4b2580a"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Edited = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-dad0c1570c0495a8"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasClipRect = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27401) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-d53323e2d841e0de"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasDeactivated = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-e944b45274a2af0c"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasDisplayRect = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ee6dc035e8a80289"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HasShortcut = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-9e35ba5187d18fe8"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredRect = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27392) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-1d54a3ff2a76c432"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.HoveredWindow = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-5c9211fc2c2e96e5"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27391) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-0eec2aa794300dcd"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledOpen = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-9a7e18378f5235d1"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.ToggledSelection = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-c046f864b71e8392"></a>[`Inno.Native.ImGui.ImGuiItemStatusFlags.Visible = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-f33e26dcdb2ff38d"></a>[`Inno.Native.ImGui.ImGuiKey.A = 546`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`546` |
| <a id="api-953390fdaaabade2"></a>[`Inno.Native.ImGui.ImGuiKey.Apostrophe = 596`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`596` |
| <a id="api-a03dcad1cf94a183"></a>[`Inno.Native.ImGui.ImGuiKey.AppBack = 629`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27529) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`629` |
| <a id="api-da7a17c49579e710"></a>[`Inno.Native.ImGui.ImGuiKey.AppForward = 630`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27530) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`630` |
| <a id="api-601b95f6aab0a76b"></a>[`Inno.Native.ImGui.ImGuiKey.B = 547`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`547` |
| <a id="api-33882870f3198960"></a>[`Inno.Native.ImGui.ImGuiKey.Backslash = 604`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`604` |
| <a id="api-edc2349c6580a4f9"></a>[`Inno.Native.ImGui.ImGuiKey.Backspace = 523`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`523` |
| <a id="api-91c08a838255c319"></a>[`Inno.Native.ImGui.ImGuiKey.C = 548`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`548` |
| <a id="api-5f9d7fe9f3edd0da"></a>[`Inno.Native.ImGui.ImGuiKey.CapsLock = 607`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27507) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`607` |
| <a id="api-821d68593a8fdae1"></a>[`Inno.Native.ImGui.ImGuiKey.Comma = 597`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`597` |
| <a id="api-90bec75ea870b66e"></a>[`Inno.Native.ImGui.ImGuiKey.D = 549`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`549` |
| <a id="api-02dd6c15634c760d"></a>[`Inno.Native.ImGui.ImGuiKey.Delete = 522`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27422) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`522` |
| <a id="api-c3f03cb97144c736"></a>[`Inno.Native.ImGui.ImGuiKey.DownArrow = 516`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`516` |
| <a id="api-e526a88892f3c85d"></a>[`Inno.Native.ImGui.ImGuiKey.E = 550`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`550` |
| <a id="api-2dcec79de3a16d62"></a>[`Inno.Native.ImGui.ImGuiKey.End = 520`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`520` |
| <a id="api-fa16776f5a649fc6"></a>[`Inno.Native.ImGui.ImGuiKey.Enter = 525`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`525` |
| <a id="api-234bcce47e17473b"></a>[`Inno.Native.ImGui.ImGuiKey.Equal = 602`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27502) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`602` |
| <a id="api-8918f48f4dd2cb7a"></a>[`Inno.Native.ImGui.ImGuiKey.Escape = 526`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`526` |
| <a id="api-d4ab540f9fe3d7e2"></a>[`Inno.Native.ImGui.ImGuiKey.F = 551`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`551` |
| <a id="api-c319e9f49f5091e9"></a>[`Inno.Native.ImGui.ImGuiKey.F1 = 572`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`572` |
| <a id="api-91351c91c7d73859"></a>[`Inno.Native.ImGui.ImGuiKey.F10 = 581`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27481) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`581` |
| <a id="api-010c9b4ba74f0b68"></a>[`Inno.Native.ImGui.ImGuiKey.F11 = 582`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`582` |
| <a id="api-6b21cbd455e6039f"></a>[`Inno.Native.ImGui.ImGuiKey.F12 = 583`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27483) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`583` |
| <a id="api-a2edd4be0e18c5e6"></a>[`Inno.Native.ImGui.ImGuiKey.F13 = 584`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27484) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`584` |
| <a id="api-b32d408aaa94a7c9"></a>[`Inno.Native.ImGui.ImGuiKey.F14 = 585`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27485) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`585` |
| <a id="api-4756cf2b651b889b"></a>[`Inno.Native.ImGui.ImGuiKey.F15 = 586`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`586` |
| <a id="api-3e5c228bede88621"></a>[`Inno.Native.ImGui.ImGuiKey.F16 = 587`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`587` |
| <a id="api-e986056b33c42efb"></a>[`Inno.Native.ImGui.ImGuiKey.F17 = 588`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27488) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`588` |
| <a id="api-3bcb308169268c9f"></a>[`Inno.Native.ImGui.ImGuiKey.F18 = 589`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`589` |
| <a id="api-435321c816042e2c"></a>[`Inno.Native.ImGui.ImGuiKey.F19 = 590`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27490) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`590` |
| <a id="api-bc22bf92dd2e8280"></a>[`Inno.Native.ImGui.ImGuiKey.F2 = 573`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27473) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`573` |
| <a id="api-2bb8e5977b08bd9f"></a>[`Inno.Native.ImGui.ImGuiKey.F20 = 591`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27491) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`591` |
| <a id="api-af24024b0126d3fa"></a>[`Inno.Native.ImGui.ImGuiKey.F21 = 592`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`592` |
| <a id="api-016e6c434ccfc5ef"></a>[`Inno.Native.ImGui.ImGuiKey.F22 = 593`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`593` |
| <a id="api-4904ecc26f510429"></a>[`Inno.Native.ImGui.ImGuiKey.F23 = 594`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27494) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`594` |
| <a id="api-54277127fd285a0a"></a>[`Inno.Native.ImGui.ImGuiKey.F24 = 595`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`595` |
| <a id="api-ac144672c74214c5"></a>[`Inno.Native.ImGui.ImGuiKey.F3 = 574`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`574` |
| <a id="api-da4d98f51a7bc6a8"></a>[`Inno.Native.ImGui.ImGuiKey.F4 = 575`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27475) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`575` |
| <a id="api-33335e4018248548"></a>[`Inno.Native.ImGui.ImGuiKey.F5 = 576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27476) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`576` |
| <a id="api-329c49325b1ac3ea"></a>[`Inno.Native.ImGui.ImGuiKey.F6 = 577`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27477) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`577` |
| <a id="api-c6a84beee795911b"></a>[`Inno.Native.ImGui.ImGuiKey.F7 = 578`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`578` |
| <a id="api-74e6cada269394d1"></a>[`Inno.Native.ImGui.ImGuiKey.F8 = 579`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27479) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`579` |
| <a id="api-bda5e995b926388a"></a>[`Inno.Native.ImGui.ImGuiKey.F9 = 580`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`580` |
| <a id="api-6eb530291a655a03"></a>[`Inno.Native.ImGui.ImGuiKey.G = 552`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`552` |
| <a id="api-078db2a385533c86"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadBack = 633`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27533) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`633` |
| <a id="api-5d7ffc5a36bbd242"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadDpadDown = 641`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27541) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`641` |
| <a id="api-d140b7e5096f1689"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadDpadLeft = 638`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`638` |
| <a id="api-6962971a73f7a170"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadDpadRight = 639`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`639` |
| <a id="api-589ec6955f3729fc"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadDpadUp = 640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27540) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`640` |
| <a id="api-7e99782ec383fd81"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadFaceDown = 637`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`637` |
| <a id="api-c07f2691e39002f6"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadFaceLeft = 634`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27534) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`634` |
| <a id="api-03cd2e241efd5e5f"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadFaceRight = 635`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27535) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`635` |
| <a id="api-e9582358620cf87a"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadFaceUp = 636`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27536) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`636` |
| <a id="api-0a2e6e2ba6ce4732"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadL1 = 642`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`642` |
| <a id="api-7ff079556fe559de"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadL2 = 644`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`644` |
| <a id="api-cabf54d0cef0ee56"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadL3 = 646`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27546) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`646` |
| <a id="api-7c293a2a14d5991d"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadLStickDown = 651`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27551) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`651` |
| <a id="api-9351524ad818932b"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadLStickLeft = 648`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27548) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`648` |
| <a id="api-0e8bd7bcecd135a6"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadLStickRight = 649`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27549) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`649` |
| <a id="api-c5df24f4d65dac4f"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadLStickUp = 650`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27550) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`650` |
| <a id="api-0966f016b4631afb"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadR1 = 643`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27543) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`643` |
| <a id="api-8edf966c8f37acc2"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadR2 = 645`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27545) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`645` |
| <a id="api-6cade7ce592dd5f6"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadR3 = 647`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27547) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`647` |
| <a id="api-2b1f3afd25e80aa3"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadRStickDown = 655`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27555) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`655` |
| <a id="api-eba019bbd7beec8e"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadRStickLeft = 652`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27552) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`652` |
| <a id="api-613b0ea3a414aaf5"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadRStickRight = 653`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27553) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`653` |
| <a id="api-6313c0463ab8fccd"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadRStickUp = 654`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27554) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`654` |
| <a id="api-514c89abaec9425b"></a>[`Inno.Native.ImGui.ImGuiKey.GamepadStart = 632`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27532) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`632` |
| <a id="api-7d4ba782031331ff"></a>[`Inno.Native.ImGui.ImGuiKey.GraveAccent = 606`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27506) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`606` |
| <a id="api-34a4deeaa5252efb"></a>[`Inno.Native.ImGui.ImGuiKey.H = 553`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`553` |
| <a id="api-380651ee1cb7043a"></a>[`Inno.Native.ImGui.ImGuiKey.Home = 519`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27419) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`519` |
| <a id="api-cceb52fab7114124"></a>[`Inno.Native.ImGui.ImGuiKey.I = 554`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`554` |
| <a id="api-6097751af7a9b27e"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModAlt = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27572) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-e031cfa40d9e4710"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModCtrl = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-de3e7b75840e09b6"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModMask = 61440`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27574) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`61440` |
| <a id="api-741e239694d95edd"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModNone = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-9be99bfe0e2ccd37"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModShift = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-f1af5b73582d0466"></a>[`Inno.Native.ImGui.ImGuiKey.ImGuiModSuper = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-eac2f7f475dd7247"></a>[`Inno.Native.ImGui.ImGuiKey.Insert = 521`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`521` |
| <a id="api-9693328fb0b0571e"></a>[`Inno.Native.ImGui.ImGuiKey.J = 555`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`555` |
| <a id="api-cb98fc5d7e001835"></a>[`Inno.Native.ImGui.ImGuiKey.K = 556`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`556` |
| <a id="api-d604d0f74cb9555f"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad0 = 612`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`612` |
| <a id="api-ddad94b221d624c6"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad1 = 613`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`613` |
| <a id="api-3c3e065c90250f9e"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad2 = 614`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`614` |
| <a id="api-04b1ea75ec599e2e"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad3 = 615`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`615` |
| <a id="api-41ca994bbde8cae9"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad4 = 616`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`616` |
| <a id="api-001ea2fb53803550"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad5 = 617`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27517) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`617` |
| <a id="api-2bac945c103cf082"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad6 = 618`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27518) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`618` |
| <a id="api-e6adda0c73778644"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad7 = 619`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27519) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`619` |
| <a id="api-636d3c982f6f1499"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad8 = 620`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27520) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`620` |
| <a id="api-e552a17e62987ea6"></a>[`Inno.Native.ImGui.ImGuiKey.Keypad9 = 621`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`621` |
| <a id="api-2552a0717c911896"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadAdd = 626`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`626` |
| <a id="api-2a185c3d12b390f1"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadDecimal = 622`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27522) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`622` |
| <a id="api-afa4415803f323fd"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadDivide = 623`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`623` |
| <a id="api-40809a1af7b171c3"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadEnter = 627`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`627` |
| <a id="api-bb790da2ce970976"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadEqual = 628`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27528) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`628` |
| <a id="api-9a6a7468367f8f51"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadMultiply = 624`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`624` |
| <a id="api-0d1c9684b57acb06"></a>[`Inno.Native.ImGui.ImGuiKey.KeypadSubtract = 625`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`625` |
| <a id="api-230b4082a899e4b4"></a>[`Inno.Native.ImGui.ImGuiKey.L = 557`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`557` |
| <a id="api-6a25cbaab3e99789"></a>[`Inno.Native.ImGui.ImGuiKey.LeftAlt = 529`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`529` |
| <a id="api-974389a493a1594b"></a>[`Inno.Native.ImGui.ImGuiKey.LeftArrow = 513`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27413) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`513` |
| <a id="api-6e7a69ba39f20ac3"></a>[`Inno.Native.ImGui.ImGuiKey.LeftBracket = 603`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`603` |
| <a id="api-2ea333a9f57a5678"></a>[`Inno.Native.ImGui.ImGuiKey.LeftCtrl = 527`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27427) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`527` |
| <a id="api-b6717226617756b4"></a>[`Inno.Native.ImGui.ImGuiKey.LeftShift = 528`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`528` |
| <a id="api-cbc84ebdea33825e"></a>[`Inno.Native.ImGui.ImGuiKey.LeftSuper = 530`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`530` |
| <a id="api-eeb5babb15479dda"></a>[`Inno.Native.ImGui.ImGuiKey.M = 558`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`558` |
| <a id="api-0f9bbdc6635d4224"></a>[`Inno.Native.ImGui.ImGuiKey.Menu = 535`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27435) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`535` |
| <a id="api-166df92385234c78"></a>[`Inno.Native.ImGui.ImGuiKey.Minus = 598`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`598` |
| <a id="api-117557a628321909"></a>[`Inno.Native.ImGui.ImGuiKey.MouseLeft = 656`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`656` |
| <a id="api-c8f29ed3e55d68b7"></a>[`Inno.Native.ImGui.ImGuiKey.MouseMiddle = 658`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27558) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`658` |
| <a id="api-7fbf7b3b704b8b3b"></a>[`Inno.Native.ImGui.ImGuiKey.MouseRight = 657`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27557) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`657` |
| <a id="api-648b59b246406502"></a>[`Inno.Native.ImGui.ImGuiKey.MouseWheelX = 661`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`661` |
| <a id="api-33f74ac31d90e82e"></a>[`Inno.Native.ImGui.ImGuiKey.MouseWheelY = 662`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27562) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`662` |
| <a id="api-7c7883f239b9ca12"></a>[`Inno.Native.ImGui.ImGuiKey.MouseX1 = 659`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27559) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`659` |
| <a id="api-10ac453a6a349933"></a>[`Inno.Native.ImGui.ImGuiKey.MouseX2 = 660`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`660` |
| <a id="api-bef36525a3b859dd"></a>[`Inno.Native.ImGui.ImGuiKey.N = 559`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27459) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`559` |
| <a id="api-c3ed264810ddf6c6"></a>[`Inno.Native.ImGui.ImGuiKey.NamedKeyBegin = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27411) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-8b3e6e05b3a815e6"></a>[`Inno.Native.ImGui.ImGuiKey.NamedKeyCount = 155`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`155` |
| <a id="api-628cd2453e77094b"></a>[`Inno.Native.ImGui.ImGuiKey.NamedKeyEnd = 667`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`667` |
| <a id="api-ddfcb5ab7cbd187c"></a>[`Inno.Native.ImGui.ImGuiKey.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-27d4ff83e9683675"></a>[`Inno.Native.ImGui.ImGuiKey.NumLock = 609`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`609` |
| <a id="api-b44b25db4cd7c197"></a>[`Inno.Native.ImGui.ImGuiKey.O = 560`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27460) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`560` |
| <a id="api-39fdc4b1aad4dd44"></a>[`Inno.Native.ImGui.ImGuiKey.Oem102 = 631`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27531) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`631` |
| <a id="api-fc8e0194e884b3a8"></a>[`Inno.Native.ImGui.ImGuiKey.P = 561`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27461) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`561` |
| <a id="api-53cd00b778e50ce5"></a>[`Inno.Native.ImGui.ImGuiKey.PageDown = 518`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27418) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`518` |
| <a id="api-d4ae52c303dad70e"></a>[`Inno.Native.ImGui.ImGuiKey.PageUp = 517`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27417) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`517` |
| <a id="api-3def06b2c39d4c8a"></a>[`Inno.Native.ImGui.ImGuiKey.Pause = 611`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`611` |
| <a id="api-635e24f6e71d24a6"></a>[`Inno.Native.ImGui.ImGuiKey.Period = 599`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27499) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`599` |
| <a id="api-2259c456abbfd360"></a>[`Inno.Native.ImGui.ImGuiKey.PrintScreen = 610`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`610` |
| <a id="api-b30d0a30bc6bbe57"></a>[`Inno.Native.ImGui.ImGuiKey.Q = 562`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27462) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`562` |
| <a id="api-13f2fb4d2cd3b8ad"></a>[`Inno.Native.ImGui.ImGuiKey.R = 563`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27463) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`563` |
| <a id="api-dc0b3a93a732d675"></a>[`Inno.Native.ImGui.ImGuiKey.ReservedForModAlt = 665`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`665` |
| <a id="api-34aeb55235c17c05"></a>[`Inno.Native.ImGui.ImGuiKey.ReservedForModCtrl = 663`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`663` |
| <a id="api-459da5436fc599eb"></a>[`Inno.Native.ImGui.ImGuiKey.ReservedForModShift = 664`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`664` |
| <a id="api-49a143c1a6c17dd0"></a>[`Inno.Native.ImGui.ImGuiKey.ReservedForModSuper = 666`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`666` |
| <a id="api-8b3efcc8a28be273"></a>[`Inno.Native.ImGui.ImGuiKey.RightAlt = 533`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27433) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`533` |
| <a id="api-d5106268c48693ee"></a>[`Inno.Native.ImGui.ImGuiKey.RightArrow = 514`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27414) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`514` |
| <a id="api-57f560fa0807b864"></a>[`Inno.Native.ImGui.ImGuiKey.RightBracket = 605`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27505) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`605` |
| <a id="api-bd26af4f5c67df64"></a>[`Inno.Native.ImGui.ImGuiKey.RightCtrl = 531`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27431) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`531` |
| <a id="api-80e20d8034f7a170"></a>[`Inno.Native.ImGui.ImGuiKey.RightShift = 532`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27432) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`532` |
| <a id="api-955af3db326cd0f4"></a>[`Inno.Native.ImGui.ImGuiKey.RightSuper = 534`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27434) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`534` |
| <a id="api-102d2cc82522a29b"></a>[`Inno.Native.ImGui.ImGuiKey.S = 564`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27464) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`564` |
| <a id="api-8448665c32292018"></a>[`Inno.Native.ImGui.ImGuiKey.ScrollLock = 608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27508) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`608` |
| <a id="api-de258b0e4e0dcf7d"></a>[`Inno.Native.ImGui.ImGuiKey.Semicolon = 601`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`601` |
| <a id="api-ce8475deb3566aac"></a>[`Inno.Native.ImGui.ImGuiKey.Slash = 600`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`600` |
| <a id="api-fee33764f2d7a62b"></a>[`Inno.Native.ImGui.ImGuiKey.Space = 524`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27424) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524` |
| <a id="api-0b54a4f9dcb2fbef"></a>[`Inno.Native.ImGui.ImGuiKey.T = 565`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27465) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`565` |
| <a id="api-7141378a24babaf4"></a>[`Inno.Native.ImGui.ImGuiKey.Tab = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27412) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-88bcb3854a67949a"></a>[`Inno.Native.ImGui.ImGuiKey.U = 566`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27466) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`566` |
| <a id="api-fda62abb6c0f21c3"></a>[`Inno.Native.ImGui.ImGuiKey.UpArrow = 515`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27415) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`515` |
| <a id="api-4392201280bc74a6"></a>[`Inno.Native.ImGui.ImGuiKey.V = 567`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27467) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`567` |
| <a id="api-fab894f472852624"></a>[`Inno.Native.ImGui.ImGuiKey.W = 568`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27468) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`568` |
| <a id="api-6a84ccf9cbb5845d"></a>[`Inno.Native.ImGui.ImGuiKey.X = 569`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`569` |
| <a id="api-74df71cf74c4805c"></a>[`Inno.Native.ImGui.ImGuiKey.Y = 570`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`570` |
| <a id="api-6d74ca05f17acffd"></a>[`Inno.Native.ImGui.ImGuiKey.Z = 571`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`571` |
| <a id="api-91876c3aac792b1f"></a>[`Inno.Native.ImGui.ImGuiKey._0 = 536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27436) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`536` |
| <a id="api-2e04a9b078440fee"></a>[`Inno.Native.ImGui.ImGuiKey._1 = 537`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27437) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`537` |
| <a id="api-afbc1b6c5856c1cf"></a>[`Inno.Native.ImGui.ImGuiKey._2 = 538`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`538` |
| <a id="api-d01f033814e77ed9"></a>[`Inno.Native.ImGui.ImGuiKey._3 = 539`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27439) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`539` |
| <a id="api-ff6c229dcc12e730"></a>[`Inno.Native.ImGui.ImGuiKey._4 = 540`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27440) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`540` |
| <a id="api-7ef90d635c45deac"></a>[`Inno.Native.ImGui.ImGuiKey._5 = 541`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`541` |
| <a id="api-65f53d0a630dd050"></a>[`Inno.Native.ImGui.ImGuiKey._6 = 542`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27442) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`542` |
| <a id="api-12283c665a729f59"></a>[`Inno.Native.ImGui.ImGuiKey._7 = 543`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27443) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`543` |
| <a id="api-a4d3a61651100fe1"></a>[`Inno.Native.ImGui.ImGuiKey._8 = 544`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27444) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`544` |
| <a id="api-69df2b61405ffc24"></a>[`Inno.Native.ImGui.ImGuiKey._9 = 545`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`545` |
| <a id="api-83cd73503dc6a62e"></a>[`public float Inno.Native.ImGui.ImGuiKeyData.AnalogValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27591) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b1c51abb0a99dde"></a>[`public byte Inno.Native.ImGui.ImGuiKeyData.Down`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27585) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a377bd2b37d1b29e"></a>[`public float Inno.Native.ImGui.ImGuiKeyData.DownDuration`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27587) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-512d4fe251d7fb30"></a>[`public float Inno.Native.ImGui.ImGuiKeyData.DownDurationPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27589) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5ff239fe333a1523"></a>[`public byte Inno.Native.ImGui.ImGuiKeyOwnerData.LockThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27613) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fb4e67bdbfb20e6"></a>[`public byte Inno.Native.ImGui.ImGuiKeyOwnerData.LockUntilRelease`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27615) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba4bdf2906e56941"></a>[`public uint Inno.Native.ImGui.ImGuiKeyOwnerData.OwnerCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e75cffb33a1ce644"></a>[`public uint Inno.Native.ImGui.ImGuiKeyOwnerData.OwnerNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27611) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eefeab4fc8d5d63d"></a>[`public ushort Inno.Native.ImGui.ImGuiKeyRoutingData.Mods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27635) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9571e065ba9b1de7"></a>[`public short Inno.Native.ImGui.ImGuiKeyRoutingData.NextEntryIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27633) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-39ec5d81b295e33c"></a>[`public uint Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0ef40112ff53d662"></a>[`public ushort Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingCurrScore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27637) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21fd9cd021790be9"></a>[`public uint Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b11113e43fee66fc"></a>[`public ushort Inno.Native.ImGui.ImGuiKeyRoutingData.RoutingNextScore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1877f2e45a2d6a35"></a>[`public Inno.Native.ImGui.ImVectorImGuiKeyRoutingData Inno.Native.ImGui.ImGuiKeyRoutingTable.Entries`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27663) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3277e93f675e0630"></a>[`public Inno.Native.ImGui.ImVectorImGuiKeyRoutingData Inno.Native.ImGui.ImGuiKeyRoutingTable.EntriesNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27665) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-954ef8a613a9138c"></a>[`public Inno.Native.ImGui.NativeArray_42c049e3f15c43ec Inno.Native.ImGui.ImGuiKeyRoutingTable.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27661) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c10b2a037c5849d2"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiLastItemData.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27695) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c69faa5a347b0ad1"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiLastItemData.DisplayRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
