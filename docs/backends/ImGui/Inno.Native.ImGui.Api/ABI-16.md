# Inno.Native.ImGui ABI · 16

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: M:Inno.Native.ImGui.ImGui.PushTextWrapPos d6fcdea22c0e4af9aeffcc58f4c35e0124bfef9bd844fd85f96832fe01a5ef82 -->
<!-- api: M:Inno.Native.ImGui.ImGui.PushTextWrapPos(System.Single) a4818ba7c61b04fdd60ec21c0c895d8ef442db934e9a0b8d2740cceebb3176c2 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Render e125e6cefa5ac2e7bc97ca7db7f2399afa5dc8d9a54a9170b7d1e0fd6808fb49 -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderColorComponentMarker(Inno.Native.ImGui.ImRect,System.UInt32,System.Single) 6f8a85640566dda3b560531805598d3c91eea44a2201612db87a12c1578205df -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderDragDropTargetRectForItem(Inno.Native.ImGui.ImRect) bf4716c8cbd8c7b0add73fc62de2295fddd61f88f19efd0c795540903657409a -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderFrame(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Boolean,System.Single) b613c7f036c3be6f9866109d8c0c4b308fb5232589bc823b5660e24fedf52332 -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderFrameBorder(System.Numerics.Vector2,System.Numerics.Vector2) 9c2a235af9b0d0e285507df5432c8984d9ed6304c05c6410d05f4092f07aee1d -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderFrameBorder(System.Numerics.Vector2,System.Numerics.Vector2,System.Single) bbceeb5f93dbf536301e5106435eac9cf8c8ca932a45bf6ead1a6ee1fa76d21f -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderMouseCursor(System.Numerics.Vector2,System.Single,System.Int32,System.UInt32,System.UInt32,System.UInt32) 1c8cfa29696e45bd217f821333d3e09b4d596855bc3cc7f0e9a15a0833223e31 -->
<!-- api: M:Inno.Native.ImGui.ImGui.RenderNavCursor(Inno.Native.ImGui.ImRect,System.UInt32,System.Int32) 129d3a40e6433e960cf11bac1ffa7e1caca4ce4da12242edd398f0da4b559e49 -->
<!-- api: M:Inno.Native.ImGui.ImGui.ResetMouseDragDelta da3228483558efacb2592c46264df7725e13ba88bdc799bae07ad9b7f56d07a7 -->
<!-- api: M:Inno.Native.ImGui.ImGui.ResetMouseDragDelta(System.Int32) 0bf9a2efb2baa3467ded080638e5d80207dc3c14ad6224eeb63a49db61f172a2 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SameLine(System.Single) c3752bf08421cda8b39aa16a886f2b88b8661ec402424457775e38a6e59f20e3 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SameLine(System.Single,System.Single) a671039bd137cfb4ff531c1155aa6e8f1532e17231d13adde7ec33aff38d139d -->
<!-- api: M:Inno.Native.ImGui.ImGui.ScrollToItem(System.Int32) b3f46de104852170a5a65a8d55352139ca1e2752127024be79b8f712f615136b -->
<!-- api: M:Inno.Native.ImGui.ImGui.Scrollbar(Inno.Native.ImGui.ImGuiAxis) 80201bbe9f043f92977067e8596ea0510efdf5050d90b819dc469c9d61b244e1 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String) ce0b85be3bce4582b1761480e5335af616e902c04d9ac09f5ceba078fd6ef1a6 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Boolean) 468cb2991ddb469aa14f235ab71483706278d9e52fe86b97c48e04ade6d45538 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Boolean,System.Int32) c32052eff3e6b1c58fd0a29f71c3b9776acee50685b2ef9ae1337e59be97cb65 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Boolean,System.Int32,System.Numerics.Vector2) ee2128c5da192f337edeb29f9d74c42e3f9746c353926d69c31519804c5f8b4a -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Boolean,System.Numerics.Vector2) 296bf53db3829c50e009a298c90fcb58df89347e1a8964d733c9324c881ed6e8 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Int32) 9b13d8c15a89e27bb0bf4942349e68f51585b35e9006be4d150168d9067e1f81 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Int32,System.Numerics.Vector2) cc8360be659d94319627cf53bf6ea50ea763da103626a16355eb122588d72524 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Selectable(System.String,System.Numerics.Vector2) e27e02659e2c6a2ccf631a3307b54032fe082f608f235ba867b241b1a5902dc4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Separator ed6e46c2f9ff82fd81e84251d94a9fbf819913a1f256df8c1730d352b8b5e24d -->
<!-- api: M:Inno.Native.ImGui.ImGui.SeparatorEx(System.Int32,System.Single) 9bdbc47d1198fd2205aec23b84521f360866222d7a567cbcda40076cd1b3d3f7 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SeparatorText(System.String) 18c9e549e065c39cda2f5aaf75a75e76a9f9e1608231a6b96694ea19cfa83900 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetActiveIdUsingAllKeyboardKeys 6959561c342c7b63437ad045b4cc38d2375c2eea39b5e1674b4f3361ea3f7877 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetClipboardText(System.String) d38bd8f1306911a6cd32446e7cdfe5042359969a19e222044161d8a522eef0b8 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetColorEditOptions(System.Int32) 700930f2e25fc8c2dc0a641af422a46b821044b8e81d1f6168d5fdc4e1a5a375 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetColumnOffset(System.Int32,System.Single) 987098db395f880d60f3a76a9f7870560bb6408959d50ebb0073f00d583337cf -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetColumnWidth(System.Int32,System.Single) 50cbb9be85397bb33f1efd0f7b3ccf7eefab0cd1332daa06ef33d4ab80f6f3d8 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetCursorPos(System.Numerics.Vector2) 18a16ba2b28a87920c797c0ff2f49ef41285e6078765c3e05375c2152aa32fba -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetCursorPosX(System.Single) 22b245d0a112c22fd59fd5f070f8882e9f27a2bd03abb781c5abe0590f5809c0 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetCursorPosY(System.Single) cecbf6431a074e387b7d7a1a8316193e694ba9b724010141d4c7f063d6bd85fd -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetCursorScreenPos(System.Numerics.Vector2) f09a1c376ff6bb4216b567b3e0db5b553de8153fbe229aea348a19c8985c4307 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetFontRasterizerDensity(System.Single) 148df4246699c30a10340faee088725c542a83bfe6e352e9fe76b67333e85f59 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetHoveredID(System.UInt32) 0136ea39766e93599384b0875322fab3e6a2d3314c4e880b0a276130cd7cda6d -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetItemDefaultFocus daa7e8d518928f39096d890a0edeb37b446a6213810e1efb7367d49746626b3c -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetItemKeyOwner(Inno.Native.ImGui.ImGuiKey) 24244189922d5a6f29423391da5119fef074ce2153164cff70fed8e5e9404696 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetItemKeyOwner(Inno.Native.ImGui.ImGuiKey,System.Int32) 56f6f3f0c828da2e2e9df972cc0ae53f27231f8bedf0468bea54e8e64ea33dda -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetKeyOwner(Inno.Native.ImGui.ImGuiKey,System.UInt32,System.Int32) 16fb11cbbe64be9a5e5f69198e7b10f55c159535f794fc7bba6dbadcfd968db5 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetKeyOwnersForKeyChord(System.Int32,System.UInt32,System.Int32) 6160b5a8feefc16d343f6227be9fc563d551ef973d9bc381e9ab7fb3a2dc2d86 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetKeyboardFocusHere b8eb2640ed04bbd91594c1e0a10ad0b25714e09b1d868d473c605cc45b75d129 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetKeyboardFocusHere(System.Int32) a889bcdb0c8d5df5aa3323ede94647a2f8fccb945eae1a428dea7cb60e3a7f69 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetLastItemData(System.UInt32,System.Int32,System.Int32,Inno.Native.ImGui.ImRect) 225b14de608fe8ae3a1e163e1d3a4c56eca1443f0d7f0719d05453ee5512d9d9 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetMouseCursor(System.Int32) 510853b5fbdd90fd5892c5de8b12cec926f35dcd7aca89967074c8781e13fe7e -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNavCursorVisible(System.Boolean) d6b4fc071aa9d4fce7671cffa06cb88e2d876c7e66f0dfcd4d11cb8a6dff8aad -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNavCursorVisibleAfterMove 8fd00aebd0dde0d1d7315a84985ca60a23d0cb4db1a1e6889e8c762f6b476fc6 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNavFocusScope(System.UInt32) deef2859219176816f284ebec2a0d55a698dfbeffb803ea5314527feaab00f96 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNavID(System.UInt32,Inno.Native.ImGui.ImGuiNavLayer,System.UInt32,Inno.Native.ImGui.ImRect) 250bef602aac50e54866267bbb6e8bce31f012cb01db021d73d79739c74c41d8 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextFrameWantCaptureKeyboard(System.Boolean) 072edfc44797919791098d3a250eab6393a89c10d19e74885eba6c35a81ea5bb -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextFrameWantCaptureMouse(System.Boolean) bf1fb65ee770745a0208475ed7a7c3d0dbb1c3670b07c4b3e33e7f26e91c2799 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemAllowOverlap e22bf6f243cb34d46dce917d34d0553ad97f712b239f8b7f37072271edf776c4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemColorMarker(System.UInt32) 7ffbaa6be1589ff9bb53f958b35b21756d3b5596cd58b3210c6f5a4e2ce3cd76 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemOpen(System.Boolean) 8c2bc1f14e1fb6d784c56d5098057727ee77e2325ac3e15780b5cd032812c241 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemOpen(System.Boolean,System.Int32) 0aaff478be987aaffd5efaf5f75828f2d133c32032c239afb4ce0efd1f2fbd68 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemSelectionUserData(System.Int64) 2eaaae224b78967551e745785c6ac43b164de8684eef472095c41b63eb858bb7 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemShortcut(System.Int32,System.Int32) eeb1cdfb5cb3b1ea20acb692bec5bb019172af82a75941c32f225d76a9e62715 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemStorageID(System.UInt32) a13d037a5e67e9ee79d0432ff3fc10ddeb60fbe193380a90eab79c91cc936ae6 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextItemWidth(System.Single) 80298439d59091a76795d4c9eb1e0cc358fd9e7f5b69223b787e879f60dd80dd -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowBgAlpha(System.Single) b9e6334a7b1b9dda06b60eb69766e6b5bc4759a44313d664ca670544cc197ea8 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowCollapsed(System.Boolean,System.Int32) 6b2d9cf63d48436ad61d9187488183ce0f0e3964f85c94fc012a44b7eac8e3c6 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowContentSize(System.Numerics.Vector2) de16999c2ca74c120010a768d9f5fec956a0992bcfb67b854d459f5fdac7afad -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowDockID(System.UInt32,System.Int32) 29606456747fa82f71af87731372e6ecfc9fe2c44693eb9b8ae95f86a50486e4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowFocus 1428534ecde70c856f423d589e41a4f05c559d259c6e187f7cefc86e8d6cf657 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2) dae215b9fc5946d98fa72e4ae1e2686bb0c8dc858f9fe48f920c7077b6d78c07 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2,System.Int32) 433a502bb5fd42c3208f204cabed19463ee7be8ddada9a7656fefadebb33e6df -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2,System.Int32,System.Numerics.Vector2) cf2e938cdcd25759a067f1d77235c2d9e2909d6d315e59344856a373796cbab2 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2,System.Numerics.Vector2) 5230493882402e243d98b4c8570737b77ff9f384f774f25a4ced011d3f28e05c -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowRefreshPolicy(System.Int32) 2dc041726876e83afad3e82ef9af4f9a959e86486895465e7fff14f66dd48838 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowScroll(System.Numerics.Vector2) 9b58e4b3d5e2f58a448d1309970d4f6d518090c83a937bc0931ec7f77714d767 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowSize(System.Numerics.Vector2) ee33adcb5b2912112bf9f59898f3f348718b1768a75e9ff17970530cb88bac9f -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowSize(System.Numerics.Vector2,System.Int32) d47056e26cd74ac6cf0298318f27f6abe3ef84b6f31ec8cc2d793e5c9f86d8b9 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetNextWindowViewport(System.UInt32) 820ecda2735daddc8284be09c22af1dacdaa738c67787e3a7440773d22bd93fa -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollFromPosX(System.Single,System.Single) 20a71ae1d6a135873339a0dcf2266ad88059630b969754acada9bfa622a52d0f -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollFromPosY(System.Single,System.Single) 516a7e938a7ab99fd9cdc1008b3b1672768803d42535edff9f6ea4117714769b -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollHereX(System.Single) e3d46021e0fb054dd36c4560bdd1697c0a1d434e7654b043daedb7b0c3098c0b -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollHereY 842ae989ccc7a0265a4d456eb7e35edecf15639e769596e58cfd6d274de25f64 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollHereY(System.Single) e521a2bc252f56eeb1cb38ea8a2a068bc39c17e1d1864847fb5b92494b4dd6a3 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollX(System.Single) 66d1815ae988e3a3559bfe4513572feb6e549fb79a4be38c105d58ace73df5b5 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetScrollY(System.Single) 812dfe2c19ae3f97b65354df65532b7bd5da15d98f5ce97ee8980826dfb4f5de -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetShortcutRouting(System.Int32,System.Int32,System.UInt32) 4211adb07dcbbd7064a9747827f41985ead27357e3ce80aa226376ffcaa5c161 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetWindowCollapsed(System.Boolean,System.Int32) e67c9a2e800a5828dec499df8ae56e886e290dc485ebbbdc3ace6a2f6a045b88 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetWindowFocus 600fa1ebf739b3c7c9c6cad151b63bc2f7e118bfe188ae1f42549375807a5c18 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetWindowPos(System.Numerics.Vector2,System.Int32) cf44751539a3607b463036b949673d0f15e3639a4e3146a0d1b9fd5e4090dfd4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SetWindowSize(System.Numerics.Vector2,System.Int32) 11da9fa1981783e7bde4247eb7baece19dc45628ef20e4a067bc10f70d0a53fd -->
<!-- api: M:Inno.Native.ImGui.ImGui.Shortcut(System.Int32,System.Int32) 84093ecd5843a5fab35d3aee99895ef629fb524cd3b87da07e7817a84a731714 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Shortcut(System.Int32,System.Int32,System.UInt32) 191119ade2b559ed0591ad1c9b3f18080521b83088ba5d2d74b7766f2dc6fe9e -->
<!-- api: M:Inno.Native.ImGui.ImGui.ShowUserGuide 2781195ed31674d3b1e45869545eedabdb6634e708057a402a153c434ac95e10 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Shutdown 61680e1e23bd5f6516cba71ec38754d4b6e2b01c0caa886133e0e614051897ab -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderFloat(System.String,System.Single@,System.Single,System.Single) 900fdd1728d372f01d305c0989dd03595dcaa1573c27ecff12f813a98fed2d73 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderFloat(System.String,System.Single@,System.Single,System.Single,System.Int32) f52863b378a919be5c3cb4e14053e4a0829396a9048cf5b5a1b24e4b6712dd61 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderFloat(System.String,System.Single@,System.Single,System.Single,System.String) 81fdea819d9a46c97fa824203290e720d9a97aed8b3e49d443e57a716f6dcbcb -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderFloat(System.String,System.Single@,System.Single,System.Single,System.String,System.Int32) 1c81fd07d98236e1c1784657225d86b14db80f234471d44fb59a487cc8d130d4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderInt(System.String,System.Int32@,System.Int32,System.Int32) ca8e9b1c6939a30fc81668a7ba36a896dfdbec323e97cb3c763d2513556effa5 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderInt(System.String,System.Int32@,System.Int32,System.Int32,System.Int32) 265fd8e210840adf41b7d9c8c8581a39f68020421c5af74db4295650bb80da3e -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderInt(System.String,System.Int32@,System.Int32,System.Int32,System.String) 42e825ad9d4ae0300eb2c47a9ac015d9b3a307a25fdaedd5599d778959df0495 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SliderInt(System.String,System.Int32@,System.Int32,System.Int32,System.String,System.Int32) 814fb8bb2ca1b0d3f492eaf715c2e7bf263ed7e81b1eb201d4ab6a0bdf18c8f0 -->
<!-- api: M:Inno.Native.ImGui.ImGui.SmallButton(System.String) 0e3e700386f8da41b111131c1259d41944137224a02137f998d2382432bca503 -->
<!-- api: M:Inno.Native.ImGui.ImGui.Spacing 8fdb67ffb52f3d53ac315c7a0ec9d9f2d0bbdc4607001843434a6781051c729d -->
<!-- api: M:Inno.Native.ImGui.ImGui.StopMouseMovingWindow bca0e9e99580c6d1ba179fa3e97f2324b675ee5058da29f7eb3e25dc73fda60c -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableAngledHeadersRow 2b5d9c86ff9f77507df45a352bedec1bef79ae0a6dbf8b1430867689df47e9d3 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGcCompactSettings f31ba5f8369d18e1ab8beff0351be4b89fe202d433c837732ff354f85b16ff8d -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetColumnCount 556a2b9a0961b58d0dcfb55f1adfa7ed6d7eeebd342ca5772b6b2a9b9b360c3c -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetColumnFlags(System.Int32) 73ec2bb00082b9220756d6977809dd080e91d4efc09c1ecacb6e4ea110f8c75b -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetColumnIndex 979a9213c55794b382af6cbcfc101f31cb40b4c4a18b40ef1a4a4a0dc6b00585 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetHeaderAngledMaxLabelWidth d2744b0e6e1eec113192bf7aa31f22630704d8dafedf9fe66c5e8c607bfcac55 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetHeaderRowHeight ef66a483e0436da88ae3d2a0bf479ed4a07cc7413363a54fc50bb3e8d450dd51 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetHoveredColumn e317242eca475412d0e9bfca72a843b2e2b2d07b9ddc9abbdf33dbc3ad8447c5 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetHoveredRow 8b821736b2575ef8ec22018a0f99c647fe77696da135233fcc01955190be6f87 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableGetRowIndex 85aaa1f99d397e3a6cf79db046cb7be96528496b90a61d15bdcbad72ef3f9b18 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableHeadersRow b3006eee336c8ae043e418958a124589861c97e992885caef8fb005e08aadc11 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableNextColumn 760ba8af67bee8b286fc192e4bfaf094849e9ad577094bc50cc4190b24b02ace -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableNextRow be147e5072835fac2a9ee81e5c818192db508890c470a5dc276077c9b3d8b18f -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableNextRow(System.Int32) a6be141423c315a9d8c654645ac7965ac1c3bdbd6ea54e0f47a26ee1ac02847c -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableNextRow(System.Int32,System.Single) f18080ee220f74715973cecab4d86bb10c8c9ab3d6918aa8a7a105e06d569126 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableNextRow(System.Single) 575590938281266404ea9d50870ba77ad26b6e00f16726023b283709c634385a -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableOpenContextMenu(System.Int32) 2242dd31f4118435021769a6e0d48a6e88ca4f052b94664272437a21dd34cf71 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TablePopBackgroundChannel 9b14fa630b903297169f6308a998226fa249b95a4757e26012c63b74d3b71cbd -->
<!-- api: M:Inno.Native.ImGui.ImGui.TablePopColumnChannel 1451bb9037a1e6bc8689ea5b49e15b90821588ec2ff4dcb18934781986ceb6ed -->
<!-- api: M:Inno.Native.ImGui.ImGui.TablePushBackgroundChannel 82bcdbf5b2bb5d263a218418634295e552346ddbeb71dcd6501a5b6d36952119 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TablePushColumnChannel(System.Int32) 2d3668d62e7777e2f6905caca0c56c619ddad60bdfb64af56494bd85687e8b25 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetBgColor(System.Int32,System.UInt32) 91d3f125fcbe5c5feeab9c221725d78b0fc5e987af0768c588f149548a6d6416 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetBgColor(System.Int32,System.UInt32,System.Int32) e57ff87b3015e1b982f9ce50a437dd5f07fc3bb6faa01a220470bb06f9345560 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetColumnEnabled(System.Int32,System.Boolean) ea53bc1812484e8abc011ec6268606130274db1f5938a9b20df3f1535f17cc6b -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetColumnIndex(System.Int32) 994525306d32a1ed0cd6ac55455efb4f56a5660cbda33820c5879218fa815b53 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetColumnSortDirection(System.Int32,Inno.Native.ImGui.ImGuiSortDirection,System.Boolean) ddcb5d2c25613ead335df865b15e3ba6970797b82c19fca77984e05c92001594 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetColumnWidth(System.Int32,System.Single) 6ca99780d24ca981cc8a5ec83df11ed32b04cc7d7cbddde1b934881fbb4596b6 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSettingsAddSettingsHandler cc910a761bf5addc2def14004db5d0ae8c3aa9cb4e87debcf9e763690bf3e678 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetupColumn(System.String,System.Int32) 8322a14d06c98d2321fb5f05d528e29721886769e1e65fdf4335b403658fc680 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetupColumn(System.String,System.Int32,System.Single) 89ec3fdcfa6cb7e91bcab2e7fcfa8c6865c6f59046cbd27c91b3b73e69b5033c -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetupColumn(System.String,System.Int32,System.Single,System.UInt32) 41c98a42477243b640712c2870ec3710143e079de42a7d8afb4d02d1c794ff4f -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetupColumn(System.String,System.Int32,System.UInt32) 9fffe8a1e322cf558c0e8c3180b78d16d54880dcc2254d47dd08023e9fcfe93e -->
<!-- api: M:Inno.Native.ImGui.ImGui.TableSetupScrollFreeze(System.Int32,System.Int32) 03ad2fe496321d59f097779a3016ba62fe177dbf9fc3591eb9a37b249fd5e8da -->
<!-- api: M:Inno.Native.ImGui.ImGui.TeleportMousePos(System.Numerics.Vector2) 875f3827d35a11b5e50b6bf7e13797743a66e868454f9a64cb7993f927ff8c19 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TempInputIsActive(System.UInt32) 01322731ab7cc72455d69f9e6fa003d5a34058fa94ac641c08e0fa7af2f2e4ce -->
<!-- api: M:Inno.Native.ImGui.ImGui.TestKeyOwner(Inno.Native.ImGui.ImGuiKey,System.UInt32) 0d444222854711e6de0c20129eaa9cec16b6c83549a827ee1a9e0d4b5dfe1dab -->
<!-- api: M:Inno.Native.ImGui.ImGui.TestShortcutRouting(System.Int32,System.UInt32) 36181572e42ad1d78a1b24b88b1f1418323852eff171f27330e79a1d8fb0fcb3 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TextUnformatted(System.String) 6dd27b16bd51df9336b4d0ac1d19a0f0c3f566b84159b4bb9ed3bfd377c5b338 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeDrawLineToChildNode(System.Numerics.Vector2) 3e4a8e8f492ec2819225732216db8ee4db39a5f7fed07be29e8c4705f444fd08 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeEx(System.String) 06064246f20670641eb47432d9efe6f481e878ad85ce701fdb380e1b4dfee6de -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeEx(System.String,System.Int32) 5daa668a3b423c562e033ecc3b37c906fb6e584f706cf27373f59759ea28cc42 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeGetOpen(System.UInt32) 4ab0dae6d4e6b746d1950bdec33583f9e8837c5b85dcdd98f71fe1140d13c2c4 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeSetOpen(System.UInt32,System.Boolean) 6456ae4166082a9eb83c3da0ef705b66475d1ebc81b684ecde238708172aca7e -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreeNodeUpdateNextOpen(System.UInt32,System.Int32) 4e9523b78fcacf7d1bf7eca36bd592de4bcb4de44a4540fcf210651d6fb195d7 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreePop 04e51f6020b4a895edf58ae39f96b05df27ca18502e494c8e0df3a9ed6dc7cb9 -->
<!-- api: M:Inno.Native.ImGui.ImGui.TreePushOverrideID(System.UInt32) 059e1d27f2bdb20c879eac872a8fbeb312c026c946716df78041ed8874a2172c -->
<!-- api: M:Inno.Native.ImGui.ImGui.Unindent 6b558b160488ea09324cedae11378a00290d60b005ea1f399ace863cbb56cd3a -->
<!-- api: M:Inno.Native.ImGui.ImGui.Unindent(System.Single) 4be05ef34d9b1c00d1cef4c4fe2e62244615437f4eb616878a7599cf9f77d147 -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdateCurrentFontSize(System.Single) 5f07a4b63b5ace54b3535991282d291d7554486d34c969ba61559e9e97bf9b10 -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdateHoveredWindowAndCaptureFlags(System.Numerics.Vector2) b1f4023ac76672c49dca8aa2c04a7c12a916abf546f981339a0057b234d49eb2 -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdateInputEvents(System.Boolean) 1f8149aa51ac29971d3cb2f7d66c12e2b59c799b79c232694f5d6fbf693613bc -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdateMouseMovingWindowEndFrame 840dbe45b8678895e1450df86e8bae8ba2185112e706273b65d5c6e252a9f5f1 -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdateMouseMovingWindowNewFrame c1cd1106ae64ec56be8a680932b0a629f18d642e6b3dbe38d50b26d143fef84f -->
<!-- api: M:Inno.Native.ImGui.ImGui.UpdatePlatformWindows 9cd20c8e36b465effb027040e33109d4cea0a9eb371be8c8e97d740ce8921d1c -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ActivateItemByID(System.UInt32) 028477de39d23037dcdeeae9fbf48b7c18dc1de7b6f13874c4f36d2ab0980b57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.AlignTextToFramePadding 5678384626dd1bef42598dc9904db446674dd9a13d32ddd8956be431b7d0d4ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Begin(System.String,System.Boolean@) 5aebb22d2c1bac5e5fc9e38679a496c1793f8aea81c4ad1b31878d6e8d703275 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Begin(System.String,System.Boolean@,System.Int32) e20613d2f09065e4a1d886537bae44054a2b3355b24b67975caff3ac8a8d6f45 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.String,System.Int32) a5e9b03fc8ce0fd097f80bdf4ffd0ddf92a1e0628fc9e05a0dcbf5e81124a3e9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.String,System.Int32,System.Int32) 440d8686d51769358afe4b72ab0ee194e38fdd9af3a014535239a66b61071e3e -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.String,System.Numerics.Vector2,System.Int32) bc53aa460c202cbc90f1c388ad1e08b55afb927074b0e9489a485a3f63c25d96 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.String,System.Numerics.Vector2,System.Int32,System.Int32) d3ba4186c3f18f2f5d222a2710aab6210af6993153d7e66fe172f1ef48f9dafb -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.UInt32,System.Int32) 50295dfd20e852a33205802f6113ecdaebd4e89161455b8e937e8801eef65242 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.UInt32,System.Int32,System.Int32) 59efbd501c66388bb7a6aa607bcd5d11563b525a2cfbc0e5cc5b8d982cd1024a -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.UInt32,System.Numerics.Vector2,System.Int32) a24b8cc5b84788cb5311d44b0c234e38aa6f22f7bc2cdf734b082e54be682394 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginChild(System.UInt32,System.Numerics.Vector2,System.Int32,System.Int32) 2cd658c9d63260b6e4d0b603130f87a6e6bffafd1d2b5aff2af2381591381dbd -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginCombo(System.String,System.String) ea47eab12a9bb6073a62054370b6975ccb7abf7d08a6fa44ecf276b0f52b3ef0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginCombo(System.String,System.String,System.Int32) 4295a2237ceb58ad4726b4ca491fc160c68faa28f3cf1e64715939927ad14d82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginComboPopup(System.UInt32,Inno.Native.ImGui.ImRect,System.Int32) 47dc993070452f5aed7bb948f92bea37856992fd3fc69b65fefcff647d699093 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginComboPreview 9ab76e98247d51d2959a287b9390bdf2f195797385e2c15e983c97f65540de74 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDisabled 9d477166a327cb8ed0e7fca7a4c6f7b35818af45722268dee49ed4fc656d5ad7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDisabled(System.Boolean) 39b1a9bec624aee27cd2731f888f98bf5ac0e55808015d42a04e62c4968fd0dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDisabledOverrideReenable 910e572de8d5d35ea997bf4f36183db5964f2f6a08ed1d6f85426094c545100d -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDragDropSource ee37239228dc27acac2ba82c137e56e1513e23c5eab3c5bb43bf04e63a808561 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDragDropSource(System.Int32) 9bfa1d6660b6322fb431a9bc242579f78227e903752d5472cb49872083a753c0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDragDropTarget 0729ee025a3b1ab338a8e83ab830e8f04faf3dc1ef381d3efc9df3f6b5de7234 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginDragDropTargetCustom(Inno.Native.ImGui.ImRect,System.UInt32) 393a165bd43eb683d70ffcf76b4c267081270c25c884ca9f80da53e93ae746b3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginErrorTooltip 89975e27b60f92d7ec206868dccf996878d9521363ce6c610865f41998be1d16 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginGroup 7f5bfc77e1d7bc3cdad405e164efc1a267b87566b1b708ff595f51fe2717a49b -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginItemTooltip b34e00a6ef62216b38f781c878a6d6a7b58949c90d212b9f30bc698a261c2a27 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginMainMenuBar ccf408f70b9d39af59e7228e5c66cdc3df32af6321e12da1af486766ed0502b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginMenu(System.String) 51aa2d056d4d479aa4e12885c85af6f539bf7d67103bc205b483cfa79b9427ec -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginMenu(System.String,System.Boolean) 070634d6e3a49ee8a452f49f721f6c41593c19d6fbdb6151f694af7809ecc418 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginMenuBar 26853edf173697b490f4cba5f8512d7f580d150a336b658f481e50928cf93e82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopup(System.String) 9348a2a5f9c10080137c25c51477a8f92cc850bfb3763846680ec280a8b67b94 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopup(System.String,System.Int32) 09aaa081eb4fa36aa4d3e035455834808cd5c383c341aa317f2c12812945b515 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupContextItem(System.String) 7400f0d5b4b5567de443ca7139c1761949c69e186fbcca0ba5402a73b8dc0221 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupContextItem(System.String,System.Int32) d2df1730470bb544da6c5941f521810792f761979962d244c1f1ada1d3098380 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupContextWindow(System.String) a4093ae677e99ea53c2d2956abc3cd72d1c69d50636e8baa36ed97b092adee3a -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupContextWindow(System.String,System.Int32) 6fc90fd6a586ad22f8f4d2f819df46c76be381d1a58cef67ad5626d4f823f145 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupEx(System.UInt32,System.Int32) 2da73fee081e3f216a8347276672fd426ac6514ef6dca94f2f7d38a7358be533 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupModal(System.String,System.Boolean@) 8e24525d372b06d92d34d814aa13bb98dbaa172f39d8e508fee1346c6bda5270 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginPopupModal(System.String,System.Boolean@,System.Int32) 2ced21ce84111f64c0b6540a94133235a5ee5965165f0b5d19b6993a66483ebc -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTabBar(System.String) d818b664dbe12ae76a23fc43c3c330c5a1269c02eca3d41d6252b3f666637967 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTabBar(System.String,System.Int32) 24ac49604dce2a78f86a672661146ff600e7f236486984716be900a188f3d269 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTabItem(System.String,System.Boolean@) f0fbe1dc3c8bc56416cf8ccbf5feace103ca8fd349c86785d5bd7eac7122aad4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTabItem(System.String,System.Boolean@,System.Int32) 7ab7d019c4cd84902d29e8fd64fb9dae09ac18bb9ead017cd85481d119827ddb -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTable(System.String,System.Int32,System.Int32,System.Numerics.Vector2) 8142cbc50b56476e8fe6c03e0bbceeb2c58ec0f64cff6562e946e4fcf7bb68f3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTable(System.String,System.Int32,System.Int32,System.Numerics.Vector2,System.Single) 49625a0ebd358150ee77ffbfac0337c0fbff92ccaf6f3244b7b9f1288f3615af -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTable(System.String,System.Int32,System.Numerics.Vector2) 66023c64e90d87e5db4882af7f3d1946bceff92203de7380f2b1c21cffcae40f -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTable(System.String,System.Int32,System.Numerics.Vector2,System.Single) d80d0f2b592966b32e47211a230c3b26e53cf7abeb0041dab11c488521a6acf4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTooltip 31d18a5b81c812fec8adc42be1075f7ecbe7305b6c5ce919c3280b5a3425143d -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTooltipEx(System.Int32,System.Int32) b36272ccb8b49a466001e4d3b948e8e178ff20dfb2d834a6ddf9564315bfdeb3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.BeginTooltipHidden 3faaa7fc46a5dcf3e57c89dc1c2bb36301eadb2ffa6a6907b5e71bcdd99a49f0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Bullet 132c397524d51e43d8c4074ab72c798e13722cba873893ed02cab806024d83ec -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Button(System.String) bee29d1755626a25c0112da1354cae0c3e56dcc676e7924be22ef555a7de963d -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Button(System.String,System.Numerics.Vector2) 5685df835bbc3389fe96fee6b412b25776a7bf65ddb00a374213634fb41ac0ce -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ButtonBehavior(Inno.Native.ImGui.ImRect,System.UInt32,System.Boolean@,System.Boolean@) 18f382321d99e9858bc9c3a90823759793960c2fd635ae26b46f6da534b2db48 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ButtonBehavior(Inno.Native.ImGui.ImRect,System.UInt32,System.Boolean@,System.Boolean@,System.Int32) b85f1656a7df7611d14cf0227cd1eb3fe857732facb32badb0b2c23f71d02c4d -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcItemSize(System.Numerics.Vector2,System.Single,System.Single) b50658c5555928b290f5c462924be1395caaa39c877dea222b312a43e6845273 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcItemWidth 8ed7e879b45954b7afc9a7d7a226697e53a5ca1f5eaef9ba041b3059d9c37185 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcRoundingFlagsForRectInRect(Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,System.Single) 8902cf9911f77713481f2844dd86491ebd4c931492d84cd39940f9bc17e41321 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcTextSize(System.String) 873f5cc7a7c44534960d206f0dd3378583098e3d3ab8d0ede13cb4099ffbed23 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcTextSize(System.String,System.Boolean) 50246963c71ba87b57748ea6a611d53cb497c0124f872a23508ecc6c232315cb -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcTextSize(System.String,System.Boolean,System.Single) 5d92a0b37014ecedd0b3be707f27893d5081b74d2b63153f6dd3d278a2232d36 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcTextSize(System.String,System.Single) 5032a83c8398e9f84418e92e2814aac538cfcba15ad215324a3aa5b00238f47c -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcTypematicRepeatAmount(System.Single,System.Single,System.Single,System.Single) ad5483e520440fdde402ce47b0a04cf3abc9270a42e09569c7fd94169231c638 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CalcWrapWidthForPos(System.Numerics.Vector2,System.Single) 1fd44a269724ea0d1e6f759a4238e160b7c09ffeee4419a46bd0aa7e89f68438 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.Checkbox(System.String,System.Boolean@) 8ccd0bf272efd9152f85f03b7ee99a8c928a7ad854b7ab9ab24d24e20386aefd -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ClearActiveID b022ba4a3f964906c1c5bdb19d069fbb4b37181c8f689bcdf8f8488662df3b3f -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ClearDragDrop f2f07ad63abddb3e300bcab99e507c40c37bc2a2d6c416d27af5a12f43bbbb1c -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ClearIniSettings 7ccd55772ffb21f5d74c1a6f0d5b49e41c92b37905b7be9b0a87ddfa44dea587 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CloseButton(System.UInt32,System.Numerics.Vector2) 4ca56ed5535a488148f33357c3bc52462a0e5fe27846902f4a95691e1cfcecc1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.CloseCurrentPopup 6af1c5eccc929ceb14d2173c17cc10fe12dee318bbb8559b906fe2e2204935bf -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.ClosePopupToLevel(System.Int32,System.Boolean) 71f7570a3de1186e30eee124cb01becf153ddd6bba12a3931d84e425f5b3c09b -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-80f9d2f0937182ad"></a>[`public static void Inno.Native.ImGui.ImGui.PushTextWrapPos()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d55710c652a7d69"></a>[`public static void Inno.Native.ImGui.ImGui.PushTextWrapPos(float wrap_local_pos_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L260) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b41dfc796c6c174d"></a>[`public static void Inno.Native.ImGui.ImGui.Render()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L198) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e923f5fe723cfde4"></a>[`public static void Inno.Native.ImGui.ImGui.RenderColorComponentMarker(Inno.Native.ImGui.ImRect bb, uint col, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L238) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad29f520f7af56b5"></a>[`public static void Inno.Native.ImGui.ImGui.RenderDragDropTargetRectForItem(Inno.Native.ImGui.ImRect bb)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L467) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0eec8418616bddb6"></a>[`public static void Inno.Native.ImGui.ImGui.RenderFrame(System.Numerics.Vector2 p_min, System.Numerics.Vector2 p_max, uint fill_col, bool borders, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L426) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a4295096e261265"></a>[`public static void Inno.Native.ImGui.ImGui.RenderFrameBorder(System.Numerics.Vector2 p_min, System.Numerics.Vector2 p_max)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L515) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4a7c8092b90d03c0"></a>[`public static void Inno.Native.ImGui.ImGui.RenderFrameBorder(System.Numerics.Vector2 p_min, System.Numerics.Vector2 p_max, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L53) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73e8ed45ed478b75"></a>[`public static void Inno.Native.ImGui.ImGui.RenderMouseCursor(System.Numerics.Vector2 pos, float scale, int mouse_cursor, uint col_fill, uint col_border, uint col_shadow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L38) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-514e5d674073af46"></a>[`public static void Inno.Native.ImGui.ImGui.RenderNavCursor(Inno.Native.ImGui.ImRect bb, uint id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L337) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-600a6400800af323"></a>[`public static void Inno.Native.ImGui.ImGui.ResetMouseDragDelta()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L597) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0880ea822c18c7d2"></a>[`public static void Inno.Native.ImGui.ImGui.ResetMouseDragDelta(int button)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L440) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a9bc2b82c37144c"></a>[`public static void Inno.Native.ImGui.ImGui.SameLine(float offset_from_start_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L572) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5275dce3092fbc03"></a>[`public static void Inno.Native.ImGui.ImGui.SameLine(float offset_from_start_x, float spacing)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L377) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f04249a3fdeda1e"></a>[`public static void Inno.Native.ImGui.ImGui.ScrollToItem(int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L192) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bcfb12704ff75758"></a>[`public static void Inno.Native.ImGui.ImGui.Scrollbar(Inno.Native.ImGui.ImGuiAxis axis)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-916cff958fdc5cf4"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-196927ada2c19c42"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, bool selected)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L583) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b340a5df6f0f24a"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, bool selected, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b035f87fd3b730f3"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, bool selected, int flags, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L402) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c143d5c4de9ef312"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, bool selected, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L579) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-246ecef3ccbd7120"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eeca3d3936e5f664"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, int flags, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L578) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c1a1239d9f9f9a1"></a>[`public static bool Inno.Native.ImGui.ImGui.Selectable(string label, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fdbbc91a09b27646"></a>[`public static void Inno.Native.ImGui.ImGui.Separator()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L384) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6848d00e60ce63f4"></a>[`public static void Inno.Native.ImGui.ImGui.SeparatorEx(int flags, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L497) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f74a1bcfe6ac23b5"></a>[`public static void Inno.Native.ImGui.ImGui.SeparatorText(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L329) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6de7fbc5f2da99f7"></a>[`public static void Inno.Native.ImGui.ImGui.SetActiveIdUsingAllKeyboardKeys()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L110) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ac736c533bfd083"></a>[`public static void Inno.Native.ImGui.ImGui.SetClipboardText(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-339b7a5cb8c9e8c3"></a>[`public static void Inno.Native.ImGui.ImGui.SetColorEditOptions(int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L185) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b29a082b2d72697c"></a>[`public static void Inno.Native.ImGui.ImGui.SetColumnOffset(int column_index, float offset_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L75) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6b993853ff04398f"></a>[`public static void Inno.Native.ImGui.ImGui.SetColumnWidth(int column_index, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L391) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0a18bdb8e674375"></a>[`public static void Inno.Native.ImGui.ImGui.SetCursorPos(System.Numerics.Vector2 local_pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L315) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bfd62221940657c4"></a>[`public static void Inno.Native.ImGui.ImGui.SetCursorPosX(float local_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L209) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12f4c65524b2d416"></a>[`public static void Inno.Native.ImGui.ImGui.SetCursorPosY(float local_y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L492) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7b238e1c39200b1"></a>[`public static void Inno.Native.ImGui.ImGui.SetCursorScreenPos(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L387) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc9758b396209f88"></a>[`public static void Inno.Native.ImGui.ImGui.SetFontRasterizerDensity(float rasterizer_density)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L477) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-97d4267dd35458be"></a>[`public static void Inno.Native.ImGui.ImGui.SetHoveredID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L113) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e45aec60733d1145"></a>[`public static void Inno.Native.ImGui.ImGui.SetItemDefaultFocus()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L349) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8cf811755b1222d8"></a>[`public static void Inno.Native.ImGui.ImGui.SetItemKeyOwner(Inno.Native.ImGui.ImGuiKey key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L439) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e81993eb70bf9271"></a>[`public static void Inno.Native.ImGui.ImGui.SetItemKeyOwner(Inno.Native.ImGui.ImGuiKey key, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L509) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-30a553fc048a1d79"></a>[`public static void Inno.Native.ImGui.ImGui.SetKeyOwner(Inno.Native.ImGui.ImGuiKey key, uint owner_id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L306) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c046b40160f5b650"></a>[`public static void Inno.Native.ImGui.ImGui.SetKeyOwnersForKeyChord(int key, uint owner_id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L374) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3fbd67aefa332618"></a>[`public static void Inno.Native.ImGui.ImGui.SetKeyboardFocusHere()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L591) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13e95c453a9b8c98"></a>[`public static void Inno.Native.ImGui.ImGui.SetKeyboardFocusHere(int offset)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L417) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e05a9751f196fbe1"></a>[`public static void Inno.Native.ImGui.ImGui.SetLastItemData(uint item_id, int item_flags, int status_flags, Inno.Native.ImGui.ImRect item_rect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L422) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64456d6efc58106e"></a>[`public static void Inno.Native.ImGui.ImGui.SetMouseCursor(int cursor_type)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L388) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f493914b695a1f29"></a>[`public static void Inno.Native.ImGui.ImGui.SetNavCursorVisible(bool visible)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L348) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8d583b23e5f1495e"></a>[`public static void Inno.Native.ImGui.ImGui.SetNavCursorVisibleAfterMove()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-afbf9e29f4b83dfa"></a>[`public static void Inno.Native.ImGui.ImGui.SetNavFocusScope(uint focus_scope_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L195) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d56cafb51e03c35f"></a>[`public static void Inno.Native.ImGui.ImGui.SetNavID(uint id, Inno.Native.ImGui.ImGuiNavLayer nav_layer, uint focus_scope_id, Inno.Native.ImGui.ImRect rect_rel)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L106) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3726ebb6205c7149"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L404) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-800e830badc35005"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextFrameWantCaptureMouse(bool want_capture_mouse)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L80) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e40f7c23dcfdacdb"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemAllowOverlap()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L499) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ecb77d549216e671"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemColorMarker(uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L389) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f8a20ef2cbe3cff0"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemOpen(bool is_open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L595) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5f6deef4889d8b7b"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemOpen(bool is_open, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L432) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4344a5307370043f"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemSelectionUserData(long selection_user_data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L170) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec062d255cd2bdad"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemShortcut(int key_chord, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L134) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bebf54a5da3126d3"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemStorageID(uint storage_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L101) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed5063c308e29c82"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextItemWidth(float item_width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L184) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-914f2b630136d1e2"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowBgAlpha(float alpha)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L76) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cce4c02a09276ec6"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowCollapsed(bool collapsed, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L191) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47981e77be7f9573"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowContentSize(System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L291) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d79faedfd15b289"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowDockID(uint dock_id, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L451) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd650812789778b1"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowFocus()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L96) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-92a716ef8c0ff42c"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L587) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b284c95242652125"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2 pos, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d779a422ecc7c4c8"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2 pos, int cond, System.Numerics.Vector2 pivot)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L403) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-37f5a662c1ca32cc"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowPos(System.Numerics.Vector2 pos, System.Numerics.Vector2 pivot)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c3fa7dfc1c6e5b1"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowRefreshPolicy(int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L258) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c37611d2339c97b7"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowScroll(System.Numerics.Vector2 scroll)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L95) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2bda97cdefbfda5a"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowSize(System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L610) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a42a740594e66287"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowSize(System.Numerics.Vector2 size, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L490) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-11752a7a9dfcf0dc"></a>[`public static void Inno.Native.ImGui.ImGui.SetNextWindowViewport(uint viewport_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L503) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c712f365e2685185"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollFromPosX(float local_x, float center_x_ratio)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L480) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c530f6a607d1f36"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollFromPosY(float local_y, float center_y_ratio)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c690c5910a662f04"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollHereX(float center_x_ratio)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L145) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c6375d5a8604387e"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollHereY()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L611) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bddcf83e1ccdff98"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollHereY(float center_y_ratio)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L491) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63564c51c1a62f45"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollX(float scroll_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L364) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-832b8c5707199cd9"></a>[`public static void Inno.Native.ImGui.ImGui.SetScrollY(float scroll_y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L335) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-582c569f1070cf92"></a>[`public static bool Inno.Native.ImGui.ImGui.SetShortcutRouting(int key_chord, int flags, uint owner_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L358) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77de5b2763451da0"></a>[`public static void Inno.Native.ImGui.ImGui.SetWindowCollapsed(bool collapsed, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L275) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77632517c6a2bef9"></a>[`public static void Inno.Native.ImGui.ImGui.SetWindowFocus()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L316) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9be945f462125e28"></a>[`public static void Inno.Native.ImGui.ImGui.SetWindowPos(System.Numerics.Vector2 pos, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L116) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3fac47c0e6567f9a"></a>[`public static void Inno.Native.ImGui.ImGui.SetWindowSize(System.Numerics.Vector2 size, int cond)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L156) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c930d397d6bca736"></a>[`public static bool Inno.Native.ImGui.ImGui.Shortcut(int key_chord, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L456) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80f407b48afbed4e"></a>[`public static bool Inno.Native.ImGui.ImGui.Shortcut(int key_chord, int flags, uint owner_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L418) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-26b83af6f3c3795c"></a>[`public static void Inno.Native.ImGui.ImGui.ShowUserGuide()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L109) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b16f4b16a2165b0e"></a>[`public static void Inno.Native.ImGui.ImGui.Shutdown()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L353) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f605e5f74ceb730"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderFloat(string label, ref float v, float v_min, float v_max)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1aae10c5939822f1"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderFloat(string label, ref float v, float v_min, float v_max, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0ce6314c45983d9"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderFloat(string label, ref float v, float v_min, float v_max, string format)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L561) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b00013620c7f1cb"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderFloat(string label, ref float v, float v_min, float v_max, string format, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L318) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-46c8e5648d7ca917"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderInt(string label, ref int v, int v_min, int v_max)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L529) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4f5e0bf6c049e0f0"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderInt(string label, ref int v, int v_min, int v_max, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L527) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd2b4fb5e8b2e7f3"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderInt(string label, ref int v, int v_min, int v_max, string format)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L528) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b12d6a62fcd9a9da"></a>[`public static bool Inno.Native.ImGui.ImGui.SliderInt(string label, ref int v, int v_min, int v_max, string format, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L127) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d859014e283b52e1"></a>[`public static bool Inno.Native.ImGui.ImGui.SmallButton(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-25c596bdf4d36c3a"></a>[`public static void Inno.Native.ImGui.ImGui.Spacing()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a138d750af978553"></a>[`public static void Inno.Native.ImGui.ImGui.StopMouseMovingWindow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L289) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-02dcaa1ca95c67bb"></a>[`public static void Inno.Native.ImGui.ImGui.TableAngledHeadersRow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L224) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64782b4b6916479e"></a>[`public static void Inno.Native.ImGui.ImGui.TableGcCompactSettings()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L114) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-48aff6ab57a2eb4f"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetColumnCount()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L341) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-850a48d9b4a471fe"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetColumnFlags(int column_n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L105) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4cfd9e227fdce6c2"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetColumnIndex()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L307) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89246979fcd95378"></a>[`public static float Inno.Native.ImGui.ImGui.TableGetHeaderAngledMaxLabelWidth()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L168) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4fa22df04111e3ec"></a>[`public static float Inno.Native.ImGui.ImGui.TableGetHeaderRowHeight()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L203) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-27b655e4aadf177c"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetHoveredColumn()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L143) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2efdf3c98a825420"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetHoveredRow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L120) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1dce80e3dd1c82d"></a>[`public static int Inno.Native.ImGui.ImGui.TableGetRowIndex()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L158) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-920b5259259a163f"></a>[`public static void Inno.Native.ImGui.ImGui.TableHeadersRow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L489) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3a5b498dda1bbe33"></a>[`public static bool Inno.Native.ImGui.ImGui.TableNextColumn()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L217) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1ee3d25e1760fd6"></a>[`public static void Inno.Native.ImGui.ImGui.TableNextRow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L519) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b20970a29776add5"></a>[`public static void Inno.Native.ImGui.ImGui.TableNextRow(int row_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L518) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb476b024787fcfc"></a>[`public static void Inno.Native.ImGui.ImGui.TableNextRow(int row_flags, float min_row_height)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba2579a1eb8ed2fe"></a>[`public static void Inno.Native.ImGui.ImGui.TableNextRow(float min_row_height)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L517) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a6ada7fd7587a9b6"></a>[`public static void Inno.Native.ImGui.ImGui.TableOpenContextMenu(int column_n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L378) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e891e911c2531d2"></a>[`public static void Inno.Native.ImGui.ImGui.TablePopBackgroundChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L251) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38a65695a254f3fa"></a>[`public static void Inno.Native.ImGui.ImGui.TablePopColumnChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L255) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-524c0885a0f3f7c4"></a>[`public static void Inno.Native.ImGui.ImGui.TablePushBackgroundChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L478) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2106eea4aeb4568"></a>[`public static void Inno.Native.ImGui.ImGui.TablePushColumnChannel(int column_n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L72) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b925634550a8535"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetBgColor(int target, uint color)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L531) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d25055da8c8464a"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetBgColor(int target, uint color, int column_n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L149) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-088ac8e628f94023"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetColumnEnabled(int column_n, bool v)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L225) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b37d08775b74aee1"></a>[`public static bool Inno.Native.ImGui.ImGui.TableSetColumnIndex(int column_n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-43788197078b476b"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetColumnSortDirection(int column_n, Inno.Native.ImGui.ImGuiSortDirection sort_direction, bool append_to_sort_specs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L284) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-04cc573c1763c979"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetColumnWidth(int column_n, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L314) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b5273faa6b84c518"></a>[`public static void Inno.Native.ImGui.ImGui.TableSettingsAddSettingsHandler()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L412) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9ef295bbaeac0190"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetupColumn(string label, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L575) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-946235c3f3f7b52a"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetupColumn(string label, int flags, float init_width_or_weight)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L574) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c7e699e4a56f783"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetupColumn(string label, int flags, float init_width_or_weight, uint user_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L385) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-46b11a7110dd5074"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetupColumn(string label, int flags, uint user_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L573) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-79e664610c9aaf23"></a>[`public static void Inno.Native.ImGui.ImGui.TableSetupScrollFreeze(int cols, int rows)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L214) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa6ba254c2d45bee"></a>[`public static void Inno.Native.ImGui.ImGui.TeleportMousePos(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L462) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc640dc2416768d2"></a>[`public static bool Inno.Native.ImGui.ImGui.TempInputIsActive(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7a0524f924b9e0c3"></a>[`public static bool Inno.Native.ImGui.ImGui.TestKeyOwner(Inno.Native.ImGui.ImGuiKey key, uint owner_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L263) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8d44c8d506fd5b69"></a>[`public static bool Inno.Native.ImGui.ImGui.TestShortcutRouting(int key_chord, uint owner_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L304) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b5415f045468b5a3"></a>[`public static void Inno.Native.ImGui.ImGui.TextUnformatted(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L513) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ecb138edde5b4800"></a>[`public static void Inno.Native.ImGui.ImGui.TreeNodeDrawLineToChildNode(System.Numerics.Vector2 target_pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L414) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e101c21693f59a4"></a>[`public static bool Inno.Native.ImGui.ImGui.TreeNodeEx(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L551) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c52903eca515d07e"></a>[`public static bool Inno.Native.ImGui.ImGui.TreeNodeEx(string label, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L248) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b75d59425b0dcaa"></a>[`public static bool Inno.Native.ImGui.ImGui.TreeNodeGetOpen(uint storage_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L312) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-04e231ef753ae1a8"></a>[`public static void Inno.Native.ImGui.ImGui.TreeNodeSetOpen(uint storage_id, bool open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L468) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb9c8a9ca77c6692"></a>[`public static bool Inno.Native.ImGui.ImGui.TreeNodeUpdateNextOpen(uint storage_id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L317) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8cf4b682bd71ae37"></a>[`public static void Inno.Native.ImGui.ImGui.TreePop()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L295) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e7ad4f5388b6e824"></a>[`public static void Inno.Native.ImGui.ImGui.TreePushOverrideID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L274) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b89cbafd486e579"></a>[`public static void Inno.Native.ImGui.ImGui.Unindent()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d44afa8ade8fd29"></a>[`public static void Inno.Native.ImGui.ImGui.Unindent(float indent_w)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L219) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b9ec60f91f54edea"></a>[`public static void Inno.Native.ImGui.ImGui.UpdateCurrentFontSize(float restore_font_size_after_scaling)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L179) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2466bc4666a484b8"></a>[`public static void Inno.Native.ImGui.ImGui.UpdateHoveredWindowAndCaptureFlags(System.Numerics.Vector2 mouse_pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L177) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1a6d8221cf09bb9"></a>[`public static void Inno.Native.ImGui.ImGui.UpdateInputEvents(bool trickle_fast_inputs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L448) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-567f99bed39b3d99"></a>[`public static void Inno.Native.ImGui.ImGui.UpdateMouseMovingWindowEndFrame()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L131) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-60fac8c7b23d8385"></a>[`public static void Inno.Native.ImGui.ImGui.UpdateMouseMovingWindowNewFrame()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L510) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2540de0d8a44422"></a>[`public static void Inno.Native.ImGui.ImGui.UpdatePlatformWindows()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L234) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d8e9ee69c47cb6d"></a>[`public void Inno.Native.ImGui.ImGuiContext.ActivateItemByID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9151) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c0426cd970006a4"></a>[`public void Inno.Native.ImGui.ImGuiContext.AlignTextToFramePadding()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11801) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5b43cfb23c3ef332"></a>[`public bool Inno.Native.ImGui.ImGuiContext.Begin(string name, ref bool p_open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L705) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c72159ef7308479"></a>[`public bool Inno.Native.ImGui.ImGuiContext.Begin(string name, ref bool p_open, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b01396b2e47843f0"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(string str_id, int child_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L698) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe76a535e9922e2f"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(string str_id, int child_flags, int window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L696) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-643eff95a6d04022"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(string str_id, System.Numerics.Vector2 size, int child_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L697) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0b0b334a35b3adc"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(string str_id, System.Numerics.Vector2 size, int child_flags, int window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11337) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3d6e68127b2c29d"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(uint id, int child_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L688) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83c7988b1837aa0a"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(uint id, int child_flags, int window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L686) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e832c0f165c93a3d"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(uint id, System.Numerics.Vector2 size, int child_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L687) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c2de2e00291cc344"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginChild(uint id, System.Numerics.Vector2 size, int child_flags, int window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10917) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c255d1cd70964cb"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginCombo(string label, string? preview_value)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L653) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0583402ad7f48ffc"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginCombo(string label, string? preview_value, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9464) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-864f0795fff3d17a"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginComboPopup(uint popup_id, Inno.Native.ImGui.ImRect bb, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11777) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bc152d743aa6824"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginComboPreview()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10193) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e778cd4358e34c01"></a>[`public void Inno.Native.ImGui.ImGuiContext.BeginDisabled()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L643) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a0934a924c170a79"></a>[`public void Inno.Native.ImGui.ImGuiContext.BeginDisabled(bool disabled)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8935) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d293d3fbbcaab51e"></a>[`public void Inno.Native.ImGui.ImGuiContext.BeginDisabledOverrideReenable()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12034) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2f7e5d7c9409582"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginDragDropSource()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L729) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-97dfc5008658b12d"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginDragDropSource(int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-948e1e42ac4b9d27"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginDragDropTarget()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9291) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f71cb0091cd8bb0e"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginDragDropTargetCustom(Inno.Native.ImGui.ImRect bb, uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8916) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ea7848276bbf873e"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginErrorTooltip()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8866) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-663534748a499b61"></a>[`public void Inno.Native.ImGui.ImGuiContext.BeginGroup()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8930) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5720f64cd55b296b"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginItemTooltip()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10224) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3b9c94d4ef4c47f"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginMainMenuBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11268) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7e9b4530abd5b4b8"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginMenu(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L742) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f4f30474d469c67b"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginMenu(string label, bool enabled)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12474) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2de1e904b2baa98d"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginMenuBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11523) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d5cae515a38d68a1"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopup(string str_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L670) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0bbeba50ef1fd06"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopup(string str_id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10019) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-779c5d18c28ac4d6"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupContextItem(string? str_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L731) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba4ddffa0007f0de"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupContextItem(string? str_id, int popup_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12283) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a897f9be4bdfb7f5"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupContextWindow(string? str_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L700) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8b7358ad1c44f0a5"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupContextWindow(string? str_id, int popup_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11498) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-faae74bb9d0096d3"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupEx(uint id, int extra_window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8876) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6f6c5859eae414ed"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupModal(string name, ref bool p_open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L692) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-525a5dc69c686840"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginPopupModal(string name, ref bool p_open, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1984990c4b2067b"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTabBar(string str_id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L699) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff7e0101c8f5dcac"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTabBar(string str_id, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11352) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e823380b24dd1636"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTabItem(string label, ref bool p_open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L725) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d756837a1c803c2d"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTabItem(string label, ref bool p_open, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12049) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8109651937cee6ee"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTable(string str_id, int columns, int flags, System.Numerics.Vector2 outer_size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L683) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8b1c588fa25058f"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTable(string str_id, int columns, int flags, System.Numerics.Vector2 outer_size, float inner_width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10475) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ee94ef8a77d4d0ae"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTable(string str_id, int columns, System.Numerics.Vector2 outer_size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L684) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-094d4a5cb6e7ba28"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTable(string str_id, int columns, System.Numerics.Vector2 outer_size, float inner_width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L682) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1381df8f415bad8b"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTooltip()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10758) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-335780d0f566c4a8"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTooltipEx(int tooltip_flags, int extra_window_flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12064) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-635df63d57f08e0b"></a>[`public bool Inno.Native.ImGui.ImGuiContext.BeginTooltipHidden()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9950) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-119d6c390d888cde"></a>[`public void Inno.Native.ImGui.ImGuiContext.Bullet()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9296) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-400ba1f69a636add"></a>[`public bool Inno.Native.ImGui.ImGuiContext.Button(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-451aec9417c01f1e"></a>[`public bool Inno.Native.ImGui.ImGuiContext.Button(string label, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11240) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a3897e38cb0e187e"></a>[`public bool Inno.Native.ImGui.ImGuiContext.ButtonBehavior(Inno.Native.ImGui.ImRect bb, uint id, out bool out_hovered, out bool out_held)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L723) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4be42de180170ba4"></a>[`public bool Inno.Native.ImGui.ImGuiContext.ButtonBehavior(Inno.Native.ImGui.ImRect bb, uint id, out bool out_hovered, out bool out_held, int flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11980) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ddb82526546fda0"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.CalcItemSize(System.Numerics.Vector2 size, float default_w, float default_h)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12197) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b30a1961b87253b"></a>[`public float Inno.Native.ImGui.ImGuiContext.CalcItemWidth()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11619) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-990b0c9553d18fec"></a>[`public int Inno.Native.ImGui.ImGuiContext.CalcRoundingFlagsForRectInRect(Inno.Native.ImGui.ImRect r_in, Inno.Native.ImGui.ImRect r_outer, float threshold)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9399) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7b2a70371659be7"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.CalcTextSize(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L745) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44ee94ab97099bd6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.CalcTextSize(string text, bool hide_text_after_double_hash)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L744) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d6a601a71789b85f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.CalcTextSize(string text, bool hide_text_after_double_hash, float wrap_width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d1ce21eab69e25c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiContext.CalcTextSize(string text, float wrap_width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L743) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9ad8560ed1247a1a"></a>[`public int Inno.Native.ImGui.ImGuiContext.CalcTypematicRepeatAmount(float t0, float t1, float repeat_delay, float repeat_rate)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9965) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b1549be46050372"></a>[`public float Inno.Native.ImGui.ImGuiContext.CalcWrapWidthForPos(System.Numerics.Vector2 pos, float wrap_pos_x)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9796) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5975a72c265d3140"></a>[`public bool Inno.Native.ImGui.ImGuiContext.Checkbox(string label, ref bool v)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12353) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c935a8a790144bbd"></a>[`public void Inno.Native.ImGui.ImGuiContext.ClearActiveID()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11591) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-139205c544e4bf61"></a>[`public void Inno.Native.ImGui.ImGuiContext.ClearDragDrop()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11189) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4256377de01ffc7f"></a>[`public void Inno.Native.ImGui.ImGuiContext.ClearIniSettings()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9791) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3ea5733297a4f4ae"></a>[`public bool Inno.Native.ImGui.ImGuiContext.CloseButton(uint id, System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9844) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b0dd24a57fdf3c0"></a>[`public void Inno.Native.ImGui.ImGuiContext.CloseCurrentPopup()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-84d348d40f426f1d"></a>[`public void Inno.Native.ImGui.ImGuiContext.ClosePopupToLevel(int remaining, bool restore_focus_to_window_under_popup)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12173) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
