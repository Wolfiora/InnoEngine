# Inno.Native.ImGui ABI · 03

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.NewFramePre 9e94bf1c638c7bc088d871ef7ffd907496340fae32e31ffe17a438ce07eb9faf -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.PendingRemoval 9835a0e71bcdaf94dccaf6eaa9777304cb692d4c08be22cb8ab3240ca113b22c -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.RenderPost 0a329448b5c072079a1a595930ebe031ce08f5400fcc38bbd61b13ee35113d9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.RenderPre f4294807739a52361a9abdd98fad0e3a29ad6b842371db37c4bad4327c018c22 -->
<!-- api: F:Inno.Native.ImGui.ImGuiContextHookType.Shutdown 5994cd9cd28eca4107ccff9c1d3d3e9e3b4a25c33e4da0ae03d4c7af263371e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataAuthority.Auto 699353692652acc8819f4b34f413bf848aa0ca18cceb3f942005e75e84c04dd9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataAuthority.DockNode ee97e3dfa67a4143797e92790bb78534b69c3756d8e4e4f220025b804e1fb65f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataAuthority.Window 62d5631d18c0cefe987426ad67ed132c1ea8f2cd6c734ffe29496d39cdca16a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.Bool 17ce6308ca4661286430548ae93878a57610cf50232d98e8155535b0fd375183 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.Count 99d6d4a418a20b3ff1e82a50effaea4356ba86df635bfa1bcdedb359a0aabe5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.Double 4d9ba0267b6f4b3cf8513bce4cc80181af3204f44cdc677f06656ec81715490a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.Float a890a0eecb2bfc5d5a01aec18b69900984402b9135d0bca5419bb830c53a01d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.S16 37c1a4582f20f32cc3be651d8f2f186fa284f4623619f5d56c2191105ec0163b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.S32 1c2196b5aa203ede3a929c07a47707f4be43099707410238d622f1fd8fca00b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.S64 15a228fcd2572cad5f486d203b70a1ee13f34c33c68a253e46e6d2f68fe0d40d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.S8 baae0624f7e6c1501a944a2781ac7114cfeb2d447b2431994f85a9859072b1a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.String 84b5728c19dae9c55956235c4e47eb47fb085c3650f41509080e79709246c996 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.U16 009317f5b3eabe8d318eff3a138787dd2b6d23841a7fa07ddf519801fc53ba82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.U32 0d142fbb4524995839a04dbab1149bdad62050168b25646782b783658655e8db -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.U64 fb4b4e147c9b9f37a19b355b61ebfaef62671b6607beea690a38d50633873909 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataType.U8 4c51380dd3ded2885b985fb846a9c06c7e6ec6fdbf096a3bed2b6693698b7682 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeInfo.Name be36d4cc9f37343e1bc013f0fb5835c3b1de69eaf5c9687889379cb45d2f3a35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeInfo.PrintFmt 2d5043d8a0939ab502b69d744deccc7cd0abfc5f27af993c8f4076e9e06cf6ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeInfo.ScanFmt 375141f9749ad066476e2d114c98ce32bef80b81be4e88a85ed330f3ca58e802 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeInfo.Size adb1cd431031b7a54184a44b0f94236ae277a7497609af232e7f637373fe0902 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypePrivate.Id c1761f22fc273bc8c4427aacd79b6b4255fe86d2ca98d64e801b507133331113 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypePrivate.Pointer 557494ca6601b79d842caff840af11fa5b2ee753783eb5e89540a7c944f71e4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data 5d1877d6ca651b993ea3be3dc6814fd33f4b45212322422a4de73042a37d1895 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.ElapseFrame baca15f780fae1d22d04e051b06840ca5a1b0b35f8ed355be5950cf217c36c4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.HasBeenEditedBefore dc5a2647105d339e09f64bead84275a8caf898bb35e1d3cdea17aaf38097430e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.ID 866d9ca91d424ecd7660e1e2b18f2e31641dd572aac3f8a4f897635df530c1b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.IsAlive 6ce2faee6f3e589494ca734fd6125b1294b2a701e0bc75e4f20815dd6404be11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.AllocCount c9cbdbd461332f623bb508c792838a4a437fadf071a174e8b306b09f9612202c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.FrameCount 2e1015f6122f8150784c65a856bfbcbe7a9fd715ff480875ef0b20edd184963f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.FreeCount 2b2ea4782944e01bcf19006a48cfb4276f73847b6e2eaee6511f0b2be7acae6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf ed9c97cdf2f993b03e489ac2727e9cb6b227a9be0916896373bbf895a4adf83a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesIdx 02cfcc2fb30c260a5b3e3950c1a097a5f2c0682b488f469bac9bc0ed990b6c88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalAllocCount 5310079a116dbb6f660d672aacf7613ec085bab0f6d891d174b3212ca4764d0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalFreeCount 6cc94815a570feced0c989ad4b442409e248af38e0b83117a863c74c7ce983f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Active 51de303f0d0ba8dac5a298b0f604a0386cd70e6d5dc6cedaecef57a6805f0ddd -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Complete 03db705e16a1ffe65865a60136203973b07b91985c8c04eb08f153a08ca23161 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.MainId 6d30c3f28f0f48617d7fa3669306716c0ac7968fb8c2ca53fe81b299356ec31d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultPathBuf d621a87ed3b39d3138e30dc3742cb3b98cf327b66cbfe031517a6e1b469d6445 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Results 112fa07a37e331aa302cb34a812f2c634223f119d77f20b5b6f5e4e2a825fe59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultsDescBuf aac3a978cc869648a69f05cfbac7cc6cf0f9b5b7ab0ca10e29606a8f7a611851 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Step 0af6d80963fddca00eb72abdfa6341cb77aa1e1556599fa024b7ef0d0194a5d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventActiveId 5a7f66cae6796cdf9e5b260ce940904c32f3f0f28e6f99e6f7df51403c10aaed -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventClipper f08e5ff31f5f1415126dc832f4191c4e6b17729fb3e93b216f63d875b23ade1c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventDocking 8ca7583cab0eb8789730aeaf26937d7cb2136d405cb9c50d051f5c6a44c7498a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventError 3f7988e732ff6ec766731d1a1e003fb56e62e33cb585ff8c98c1f8afc1413ebb -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventFocus c28af45a27e2c81e95ff1db8dd4a5d5f14516a17bb12b862edda3c20d5f0dfa4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventFont 9d399dba738ec76cf391ff985040eac2c809ed6d34500cc1183d8ebe8c818d21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventInputRouting 9c6e0743775b3e2871b754831f3b6b7277a57926953bcf65dcd2a8a34c585d85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventIo cb9687b49654f0fb70f15a082fb1f331f7adabc2def0443acced7402badee42e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventMask 77629ff2b8486624af89b9e27e41851df465dbd948ef465cedeb655059fd10f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventNav a3cfe7d9c916356913616cd2160ede6f2fefa0d43831c7c246fcc2f352d98b15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventPopup 45c2af9abfc5bfbb107c99d1e7d679f859d354108f5de8dbb6eb163d5e982411 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventSelection 555428b1768c339006bd6109bbb9299e3ff2cf90a5713bafa9c816389338fbab -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventViewport 728163581207d6495c09f676defb0e1262e2b2fb930372c76894789220b26084 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.None 0c1cd8890d6f7d48459fe3db82af858f7bad1f9ec0253632f826c1978483d3a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToDebugger 082eaf8ea663acfb466e2528cf5b084c972a6c6056c4ca216937fcbd7a9ce323 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTestEngine d2984fdc18f091d6bd391413afb73b7b3c14ee95790ae4f3d360b8e8d2a44528 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTty 8643639a7a8fb00a8c52f7bd81a7e665827b7f41b0551d82db951ed6c4d42a90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Count dd046e12a8c6e27e00b10a7e36898d0038dbfdb7cde70bd990cb39cd038ac251 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Down 0638e7645cd4a4a3cc39b17445528ac780a93bd4cc2f100ae12bb5ff491c8429 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Left 50658553ec6d5020271cb72a871d6c5ce4607fa193c220b8b5dfbf93314fe66b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.None 00e88cceda45629797e247a3ca62c8a0e484157521ecf6a0474b86c53e14abd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Right fa53e20c630d52f792338df6a59b7fd9275e086ee025f3e4bf22132d8e79e5f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Up 11d462f3b308a7bfdcc5a7b9ec998ff3ce88e195808c1f94a0c942f78d5fd283 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.Nodes 1578e93beb29d8f8c4da1f03f23bcbc641420b7d8a1731412752c8cee00d9a4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.NodesSettings b3ce549cd1e8657174351e599f5a70fa70dbd8aa94711b7a9c1354d5422a9063 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.Requests e1c192510ae6f1c4097d1d23d07324d27a728bb8192a9f10cfca1e4a0023249f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.WantFullRebuild 8191e143784209eac5df172ae668599f2ede932c13a7853ac8fb88a8d4fe5632 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.CentralNode bff53ea71985265ecf013c72d2b33138361a8b691f9c16b5334030b033fd59c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ChildNodes aecba7000560861611583ca5b45976b0663b4faaea94d4bdf7251b6b28a8f746 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.CountNodeWithWindows 9d3a7263fc279b997508fa2d106be36dbbb97391b8c4f4f33b046a814af81de9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.HostWindow 4ee164f7052af22493c8c6d74a5ca61a16ce1f92c727b5534619d4ea60c6b6d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ID 83cf6dc9cb7c0b2a8068ef0e9c2099b76f59a0af6a4253e8b05c77cdc27d1116 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastBgColor 413a7d83229e6bbd1a093ba75f38447b4ec2552cb80df8b819bf48ec01e09dde -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFocusedNodeId 591ace644a8bdc5cd4da1360c35df1944a63c797dc18d584699e0514d64075a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameActive fa99399f2ef65d4a6c63a7e885efc0efd1469bb030c67599739f10124b2491df -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameAlive a8053871af02ccb409a20533eba0de54d4d657908cc79829385d16a1002eb32a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameFocused 8938a8ca5e8b109d02efe115fdbbecb1a70a984c1df287192296408dac97418c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LocalFlags 5d11eee11605be8a23962fbc14f0d81f96ac91cba9108c62343d77d40ff67ec4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LocalFlagsInWindows 4f543d66059e7ad4ae45dafc676efbf33e36746875f7737bef6fafd6ecb27bbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.MergedFlags 95b5cedac891c23a7eb0075d56151d83981bfa0621b7b37b39e42d48da1c968c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.OnlyNodeWithWindows 3bd5a29153e12559f20a8f30ff18f9f3062a9f06f28edf7d5a349b40cf55007a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ParentNode 43371c690fa3deb111bfd1658a3e00012a7b7fdcf5a82303464f2314bb157063 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Pos 84c5093c467248b321895a55d040b524d41d1a554bfec3bfdd0124d7ba0ff6ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RefViewportId 4ffe38488f649e20ba6dc907c4050fe7eed8c9dd96425a2fcb771e4174bcf4ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SelectedTabId d0dc2dd4290c2918b58d6a1fc2c9adb50b2d77e0b6db7ed36c3eeaf4a094e631 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SharedFlags f2e909954f12ac5b8e5f4c569ae7a3e30413ba4e16b678d1aade64e4d98eed35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Size f9d89b89e4645392c2a327ec95c2a1a797e4648ad84dd72a8d9e7d79d5637f73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SizeRef e9b08721316c3b41418727e0985c5b9cec185823e458eb24f703c67238499e14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SplitAxis f29df46cc65e3e2ea6658f1c15ae5d8520fc7e38eeac322cf7e8edc346d18d8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.State c71c4f27bc8164d8251ad1e9ee4679bc8a292e2a69a6ebf441fb8d69aedafcd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.TabBar 5162767d665919c53a3e132282cbecb664011b35454ce480451eec2ecaef1f09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.VisibleWindow 1bb3c69dbf98d413dfec5cd2f8b62aae0f0aaa5d47d2df85c2516d3877eed0f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.WantCloseTabId 8ac7f36775900f625a9849427a44590370c218b0816a45d8c0c9ac15eabc99d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.WindowClass 9034c18dd73de69780bb396eb9cbd8267d9a982f436b2c745c8087ed54d1f9ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Windows 0ad6baf32dc49213a2c4a842151adb9275cd73c0a19491c1b7e6d7c50b9cd436 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.AutoHideTabBar 2f88ee1e5e8ac7d0ec921a661d58ee95c720c60ca94cc558b6061d8333b4f43f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.KeepAliveOnly dbf7af3742cfb1966c6b1f42139ff5ecd461f061d6718fee652dcfc8adb96da3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingOverCentralNode 9127305d46c4655e46bd4950ce973d0020d2f733ae510bba537c1c96b547ba6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingSplit 9b9670f5cd933bddd5f55acb3461fdcebd0843ea93bbae95e6c44f0b5e0a80b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoResize fe2af67ead24cd624870dbc0c6e15b0ace0b703b9d8e61567c62cbe5860ed70c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoUndocking 8d602906ec7061fc42b8b6cf9c4eb5abc65e95ea6e4f50bc7a41c0493ef53b11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.None 178c50d7deb255ac58b994dabe749a930500120038a312900627a22d99dcd421 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.PassthruCentralNode 33cb04a405cfa14e5fbcbfba8e5fc5a6e6bd5169346523eac40c98ccbc51e650 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.CentralNode 051e3ee7d115d89414ad4bb60ece189813df109518c2e2e0159c774eb03b3999 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockSpace cc64074e8f4dc693250fda7eff49704505a1748a68c160e0c1445f275f279ed1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockedWindowsInFocusRoute 7484140a6673dc4950c08863934efa82536155efe784eda3f4e08ab99f96d45b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.HiddenTabBar 270ee4a90eff712046a43e59f57174e167f1164e769252f21519a0ba87c8b9b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.LocalFlagsTransferMask 30bb5349f13195a706edd246b9a599437041e79c836fc3574066d1d3df9ba56f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoCloseButton 5179adf36fbde6250b1f57449719b5a87bd2cdfd3c233992cdd429dfad0f1ab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDocking 58f9cc5ab73b4b4ba69e4038588c6ecf64445859277dfc5db16a2a396a3e035f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverEmpty 74e7c45b175d3faec0e259d4cf989c28deb8135fdcfb2a58fe239ad28d2a5606 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverMe 9a1c9d9896f29e1f82d6718174240e0c9a934c962d10610e8d3b16fe230351de -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverOther 69ce8030034e60fea87504fae98e6ae4151ed2b61f0c0fd7db79cb89cae0bb53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingSplitOther ab1366cb21afedcc7cd98a5672daf774eeaf363985ebdb0ca4ad7f159838db4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeFlagsMask f75d5afd92f292522df74720216b724cb9479f1e8cc1058a2eca4d09dbda9636 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeX 46eeb3f8609f38b168196c590991935a9af458b742150de0ab21d0232c3bf460 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeY 59a4ac1c4d5404ec95f01dd1380444e2cc5c0971d56da1e8f452a9c1b455c0ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoTabBar 8868c588820999686fa2168547f1ae9a1851c76d8e5b7f04c46f24e21cc84333 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoWindowMenuButton f426df40de2883d6b7367fbdd5de9e1c6fe9c453db82e40c2edbfdbb4e93aeb3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SavedFlagsMask 38a6d7cc1c4f0d2aeb7b10a62c9e1a1c500a13a1f54febb5f7164e5595d3c848 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SharedFlagsInheritMask b1601cd309daff2eb7a4b061ff34c160063f7eabb91c48797ba50e2e19d26f78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseSingleWindow 3587b05420f89296c80b9ee61d161c7c1b4704c9574538452260e545b77dd7d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseWindowsAreResizing bc4cca0095703b1b1e1a11992c96424591d58741ec1e094c6856c60dd87cf06c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowVisible 7e8e9ae6febd17e3d242a405b5d745e5218238c4cea927604acb14b9212f71cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.Unknown 71d42f4227d3e5f6ebdbcd97cd4861b9860d366cfb22f46635d40c3b2c553ba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptBeforeDelivery 7afcd94396ded7a7da6f9b09d325e159f7a63593b4c00c6b1bfcd9723620c14f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptDrawAsHovered e461d1650dad05ab82c0069845a8f84f409c544cb88f2a3cdacc474aff68b1af -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoDrawDefaultRect 6bbae35b30677839430cc75aeb014fba25b44d2676041a4f018f5fd15f1263bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoPreviewTooltip 6b282ec13c4eef0bd0d8be0f8dbdaaf60c26e09ec5cb3e8fcfcb2adc7b95d692 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptPeekOnly 8e00261b512ae1cea0227770757c57b3381ee9d7e22fcdbcbf5936c22e75b074 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.None d4fe042828fae51a4987425580d303b61d6c173a1c6e8ef0c33e22f190a41ad9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadAutoExpire 509ec95567dcb87e056c3a5bc8de211300cf0032130a28cb112712ade1193f88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossContext e41d3b47a41758df64004a31f0e38bb182f89268fd7c45259f7ec003e5bc1e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossProcess 313eff03e12348146dbbe83a1c95a89dc353985a8d76c4a24b832b624f430e0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceAllowNullId 1d9ed25097edc818e386a6dcb9f5bb979ed9e3c43bcd240faa20106e994735ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceExtern bd2f6bc5820a8d1f318bd24cdf514d10b59f473d1620640e489d47d770330b4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoDisableHover 0c730bbc8f30596b315e93167409248ed57eb128be4d8c069389e3ff7ad8ee03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoHoldToOpenOthers 4d544d4686ebf9d915a905500d6b350afd8cffe219da4660320552231e62b0c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoPreviewTooltip bb16204be45d1664683e035512d668d5f01fede8fa1c74dca5bb8442971299c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfBeginPopupStack c8eca3a1843e99f919a2d665abae9c39c101e8e6d74a6a811fb27e70738dfa2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfColorStack e5cca104a5b5e6671d3988ca6d82028ea8934fcc94c6fe1c964c94ec8b1a0a2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfDisabledStack 94e9cb8891adb3df232a673ec916ca26d9ca88a8da807fd98968ddfc5d7b9a81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFocusScopeStack 85fadf2c952d006abfa5b72be5c46899ab775a4e575e11193e54db323f76e0f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFontStack 378088e7cb65918b536c06d4e3896a38ae9de7f782fd7ed8c8a20ef47691caf9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfGroupStack 88ccb2abd6e61cd628cc31fbac6d69340136946dd2222ff14f0b2ea690255a01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfIdStack 83cff222307189ad6efbe771eb9e375c46d676ba5167bcf0aa12d4786fb8e900 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfItemFlagsStack 6595bcb6788acecb4e01167f3145370d764c4e1c461b52af50073a5a77837355 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfStyleVarStack d5c0129a5a3adf2a2df019a13f6bd88467bafc3ac71d3cf0defc904c7aae3144 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfTreeStack 4bcf12c9750324e4814424c81817cfa90d308e7c597869bd78d2426cfeb89eb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfWindowStack 5e64af8c6f5a9e0f315cb2644a6c95e2f2fbe12532dc69bb2a1a96b5727d273c -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.None 319f3410fc1cbb2b4b8a5780302d026d182636e964b8e0ac16b668f2df3d62f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.RestoreFocusedChild afad0a0425aaecbf09be616f8986ef7453de828d49b110bb900351a6b70fea3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.UnlessBelowModal 6b66194762e235c5d1c9d53d9acdd19f7f5814047c31cb3030376f91c0006030 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusScopeData.ID d096c9d5d403b73e55a3b38a2c58024369861db18b5d20fccf0a077986440ef2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusScopeData.WindowId f51d1643d79f6c4bbc887d5b720ddf3710b6737ebd5f29b4742ee1855bec8905 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.AnyWindow 5a5e4184da532c70d0236287b5944e0cca955a9d471918278cf1af059f9ba718 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows 531294df59105bfa6f8a656766a023101850dc917ffc2f58070f1c82bf5e3379 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.DockHierarchy b9195f314ed30b9bdc2005d86df52a087236a966c4c37877bdaf0dbbedbecc26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.NoPopupHierarchy 980ebdb5fa8c39d9456b762dbda59f14f160aa0e47d92e2e3d38347790409c49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.None c3abb5dfd70e022fec5958a80cb89467de94108ef23959bf816b2e3d2c5dea4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows e2d32f9ff08ce2bae0830e63f55ebb3c853934ed3ed89d192afe09d1508d504a -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.RootWindow d2a56705b0e4a13294e257ff61523da1324bfde99421d06be4791dd95b923cc2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame 2ca8716829703bc7822950c5d2ab69b5f2ea6e3ca3c9761ce0ebf39b3f001af4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdIsAlive 5e21b0bde4cc95b797be1616beffc6686b00f0672c31ee64a5a4e099eacd71e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineSize 0dac212fc0d834b8fe1119c78d57b2f8c96484cd010b0859a6075af620acafc3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineTextBaseOffset ba1b53b8f358e3d75bf27d852df2c8703092a6b885f2ab1d502c377d92c8b027 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorMaxPos 79c0d544efde1ce67c34bd606ea8e1a51955f523ff9139ad6977cb849105302e -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorPos 1bff0557579679ff97317be57e193db284d704aa965a5f4e3127ec0d32e24f9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorPosPrevLine cdb3bef8cde294ccf8e4ba2670262623b6d868e4cb187b8825aeb8c4c4170f7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupDeactivatedIdIsAlive 4d9791f36e58510516a172b4ed142943cb96dc90734fc89272e7dd7b980370d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupGroupOffset 7d4bf8ac889145243e61d110dc7a7a2c6fa00f0b6a7244c0c79175bcb1837d9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupHoveredIdIsAlive daff51bc1c888ee21f1d4e6fc0a2b2b60b72807219689682043995f8d195be31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupIndent 11a1885e377c5a1fdcb8becd32e5e29c4286ab3c8f7bebc98378724e70f13182 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupIsSameLine b3d01556b0e8ed404903845509351a88d889fe957d4ddc405680ba585e0c9b1d -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.EmitItem 8ea3bf3ba86c8ac6e571fe625adb3d92bdc977563bea7982c670082356d98cc8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.WindowId 30bf6fc1ca39b03082e45fddbecd8277fdf1846c49b0c7f9ae11e50bb83df30f -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByActiveItem 43aa9a06a8dec4a28b0ff820f2aad6f7456ce9e6401b57a21190daef75db7281 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByPopup 02e45b1aade27e380726ae85777eb76c0ca4e33e4b7bb0733c0b1efce2f0d11b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenDisabled e9ea227df2815f612c9b36f4c0a06bedfabb9ed6de201a7c40b88a4940451551 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlapped a7a54669386d05ccaa9fd799617638af5f169205b71c45a37628caa44c155d9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByItem dd3db1e1b023dfd27c0e45c3e4dd45e8bf0a7ad39a73aa27b42bf6a5a65940c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByWindow c1be38ba67bf51d36aad19b2c85b0c29debbe52463e768b59b0ec2dbfb6b7afe -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AnyWindow 3eace326716589447b2f4a3213932bb57117a740b78af27b07527054a4e1bcbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.ChildWindows 5595d0b918e3e9f7cd65be528245bbaeeb832abefb81fa333f4e308503069438 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayNone e624fb40f1f5e3d54cfb23bd44b938adb13777fe9e5748433fba620eab5c24db -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayNormal f6831006ed7a5dcf4e7f230b3152212fcc8f1b6bb162254488654de2cbee00d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayShort 4b984b9dbc2fc74695f40db013bbd5a52ef2dc7839edf052843c03669efa40fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DockHierarchy 0ba70c2f2485420cb81adae4a5265e2d6addebb1c9fc728b3291e5086709fcc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.ForTooltip 36a095518c30f0ab3e7944b0de40ce37f9588ef65202267ae1b24ebcb07098f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoNavOverride 5300470c9cb9aa53552ec2b18c3437128e3c200a45fbaed1035b7fd7d6c0176b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoPopupHierarchy 7f7b2ec42146c84314aa61d156b54dfa0042896f197a0507a5fd5d1f364f5165 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoSharedDelay 8643e7cbbef7f7f55b9a50896956d5e61ea26b00a1d9969585a1b5b5cd806f07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.None 38b0283bef9785ae661d33bc557617099eb8056555015b9f0182697bb4d47e93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RectOnly 0941a97eb163ac016ded328b8591dc1438944eadcb2da85684d2eddda6cbf89e -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RootAndChildWindows 5dcc376351a92625fdfe28935e50d52c13052d5c0e25680fc3cc4993cc3e45f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RootWindow 8ef160a26447f05a3055355c5e13a535b2c7511ecd7401b69ed1b340f0dfd8de -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.Stationary 14299ce4c48681d60da5127c098a510a12b5997d7db734a740eddd5677bc74db -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsItemHovered 0a652a304672b35ca64fea7a6b80cf35621d49fdd60467597a760138685c44f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsWindowHovered 25bc20b9fce157a3bb97ea75c1ebd3c6160ce0c7621a06d7fe5b305d93945593 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.DelayMask 46c4092453feacb5e9f7168b33dc9f4cb2b333bdd51ac6da3bfe9fc3035bd7b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.CopyToClipboardLastTime 99c432fbc1c97f5dee7459d5431531f7de3a0958c05396b619ebf729115f9399 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.LastActiveFrame f8524c04af1f16d3da6de78d10e58cd08760921cc8eb6eb10324a5126397be9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC bafa88af02563baab3b05a0856af94f5330f56e502989fa26350b65fc80f4b5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.OptHexEncodeNonAsciiChars ee3fba6141481ac64b2c8701794dee9670d91630ed485a6cc2e4e3fc2aad9a2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.AppAcceptingEvents 6a16c680b1d6019ef470ca5fd7d7057ca21b73625d39ba30a0b9ec85aa0518cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.AppFocusLost 6cf42bf78b72e07c4165cf94d9b97a92fca4bcfc34d54d2cdc77695fe1122510 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendFlags 8db64ea311650c4dd2391ca1a4e488a92e2a5840df5fd7ce3edc87bb9acc804f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendLanguageUserData 9d01ff998636dcdb31974caac57b1a13f0748032f814abc98d79f8a950467b57 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendPlatformName d14569127ce5e3f52f139e37e8f8e9eba5ccd8142793bbbfe2c75fa9df8a7e56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendPlatformUserData 07f21fc53fd75956058e3795c3cb2be24485c9721b837397b6f680fdf9b659b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendRendererName 21797d1c9abf4ee1f0d08bed16d982f0214076e0d4d43ae3bbf1fbfe92205b24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.BackendRendererUserData d327b25e3eb78ff4ff7fa78ccbd3583799107f4e270bd3a49bc6c172f07cbbfb -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueLoop 09b2b85de04a02599d3f35a8b6df3d8fdc3902f9fc2819bb20b2aefe132d8aa2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueOnce 3712a68fb7764a58615718c71764dad7e7d7fefc858f7faa3f59cc1805b135e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflicts ddcb5b317d5bb0a9f8bb967a725c70e15978de27ae41474c08c0739230ea3a99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker c1442e651719d683faa2111681bf94156cfcd386d66892ae39ffd4921f65d0c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIgnoreFocusLoss 8e6e57e4087148d1bcf9ba137021e5abf14c223072fac6d1777f606cce7028fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIniSettings 2c1aaecb37760932877bc8b8fa700504bc9f4f6ec651a238df408bdcba2bd560 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDebugIsDebuggerPresent 2337694ab8e7b978488d8e8dff88d6e76fda2e33b5bb0ed95c188c969bc69cb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingAlwaysTabBar c56d9d9281ddaf5f3e2b4b2b289f513b10dd61333d98f38c34c43d68deaf596a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIO.ConfigDockingNoDockingOver 735ce5afa0fa7881b7470b77a00e80844d1538184716950231893e9624edf97b -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-7f330913a285b972"></a>[`Inno.Native.ImGui.ImGuiContextHookType.NewFramePre = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25607) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-524adef15304d797"></a>[`Inno.Native.ImGui.ImGuiContextHookType.PendingRemoval = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-ac2d4d79088c6eaf"></a>[`Inno.Native.ImGui.ImGuiContextHookType.RenderPost = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-97508dbca4acbe96"></a>[`Inno.Native.ImGui.ImGuiContextHookType.RenderPre = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25611) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-3fbaafd26b084e2a"></a>[`Inno.Native.ImGui.ImGuiContextHookType.Shutdown = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25613) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-13580c4cb946fef7"></a>[`Inno.Native.ImGui.ImGuiDataAuthority.Auto = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-24bbba182dcf0e6f"></a>[`Inno.Native.ImGui.ImGuiDataAuthority.DockNode = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25623) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-454a26d5feb88b05"></a>[`Inno.Native.ImGui.ImGuiDataAuthority.Window = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25624) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-355ab866f9d837d3"></a>[`Inno.Native.ImGui.ImGuiDataType.Bool = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25642) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`10` |
| <a id="api-4b1432fa5eec77b6"></a>[`Inno.Native.ImGui.ImGuiDataType.Count = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12` |
| <a id="api-c18adfb099a789dd"></a>[`Inno.Native.ImGui.ImGuiDataType.Double = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`9` |
| <a id="api-84c672380a76b136"></a>[`Inno.Native.ImGui.ImGuiDataType.Float = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25640) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-5e6b781904170657"></a>[`Inno.Native.ImGui.ImGuiDataType.S16 = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25634) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ceb1a6340ad15ad3"></a>[`Inno.Native.ImGui.ImGuiDataType.S32 = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25636) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-18bb8087da0b879e"></a>[`Inno.Native.ImGui.ImGuiDataType.S64 = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25638) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-7cf321e7ed28fe65"></a>[`Inno.Native.ImGui.ImGuiDataType.S8 = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-79bdcf2ec52d1d30"></a>[`Inno.Native.ImGui.ImGuiDataType.String = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`11` |
| <a id="api-4ad3fbc3331205cc"></a>[`Inno.Native.ImGui.ImGuiDataType.U16 = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25635) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-26bcdd605f0d927d"></a>[`Inno.Native.ImGui.ImGuiDataType.U32 = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25637) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-7a9dccf15f2c8980"></a>[`Inno.Native.ImGui.ImGuiDataType.U64 = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-b11d413e884ed45b"></a>[`Inno.Native.ImGui.ImGuiDataType.U8 = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25633) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-a2d125f76fb61f4f"></a>[`public sbyte* Inno.Native.ImGui.ImGuiDataTypeInfo.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25657) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c249d1533d5ca9e4"></a>[`public sbyte* Inno.Native.ImGui.ImGuiDataTypeInfo.PrintFmt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25659) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35db8f224d57eef6"></a>[`public sbyte* Inno.Native.ImGui.ImGuiDataTypeInfo.ScanFmt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25661) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ddaa3c5f4c3a8b4"></a>[`public ulong Inno.Native.ImGui.ImGuiDataTypeInfo.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25655) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a1a5ff567abbcc0"></a>[`Inno.Native.ImGui.ImGuiDataTypePrivate.Id = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25677) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`13` |
| <a id="api-eeb98e7079de8f1c"></a>[`Inno.Native.ImGui.ImGuiDataTypePrivate.Pointer = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25676) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12` |
| <a id="api-022d17f986b2f8f7"></a>[`public Inno.Native.ImGui.NativeArray_bae28ecfe547f5d7 Inno.Native.ImGui.ImGuiDataTypeStorage.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25688) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff8ecf23bc6eae5f"></a>[`public int Inno.Native.ImGui.ImGuiDeactivatedItemData.ElapseFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e496fb8b3afc70b1"></a>[`public byte Inno.Native.ImGui.ImGuiDeactivatedItemData.HasBeenEditedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25710) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6c18da6c20b968ee"></a>[`public uint Inno.Native.ImGui.ImGuiDeactivatedItemData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25706) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35d0375be9e657a4"></a>[`public byte Inno.Native.ImGui.ImGuiDeactivatedItemData.IsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25712) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0f8bdd72efee5c5d"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocEntry.AllocCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb7709c7bf465916"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocEntry.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a71acc2efb2a559f"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocEntry.FreeCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ba6a8d7a8bcffed"></a>[`public Inno.Native.ImGui.NativeArray_f9c2e11bc01cc26a Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d069bc55598ddec8"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25756) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a0578971080ce47"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalAllocCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25752) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1880d7370271332"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalFreeCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25754) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cae7a729d314030e"></a>[`public byte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Active`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25778) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-877efcf3af99abda"></a>[`public byte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Complete`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25780) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-862d15c7689e4782"></a>[`public uint Inno.Native.ImGui.ImGuiDebugItemPathQuery.MainId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25776) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3861750ecf11c95e"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultPathBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25788) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3af90225c9ebe3e"></a>[`public Inno.Native.ImGui.ImVectorImGuiStackLevelInfo Inno.Native.ImGui.ImGuiDebugItemPathQuery.Results`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25784) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd11eafae4e50ccc"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultsDescBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25786) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af0b6c07b42ac899"></a>[`public sbyte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Step`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-24782b9fea8d0c9b"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventActiveId = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25805) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-a7262b7b42aa6d38"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventClipper = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25809) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-ce31126d2a527df0"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventDocking = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25814) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-4e2d0abd6a1d8671"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventError = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25804) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d7f7886c6903ea06"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25806) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-3d9b573266145b3d"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventFont = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25812) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-3de9448b4926a3bd"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventInputRouting = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25813) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-8131fc55b32b6597"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventIo = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25811) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-76105d237e11ec5a"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventMask = 4095`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25816) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4095` |
| <a id="api-bc8df9eef99c87e4"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventNav = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25808) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-4205f4f42034d340"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventPopup = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25807) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-6570ff0a41ecfd7f"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventSelection = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25810) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-40e271d9e610901c"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventViewport = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25815) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-71b35ebf3682d109"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25803) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5752c7395f24123a"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToDebugger = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25818) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-2b06a45f02381ac9"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTestEngine = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25819) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-5ed9130f7e4d2887"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTty = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25817) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-76057f7e3e8ae460"></a>[`Inno.Native.ImGui.ImGuiDir.Count = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25832) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-ddada31e995ed4cc"></a>[`Inno.Native.ImGui.ImGuiDir.Down = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25831) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-c5ddf8721645aa0c"></a>[`Inno.Native.ImGui.ImGuiDir.Left = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25828) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-54ef62041121f742"></a>[`Inno.Native.ImGui.ImGuiDir.None = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25827) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-67059de5645ec266"></a>[`Inno.Native.ImGui.ImGuiDir.Right = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25829) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-360919daa1f4e053"></a>[`Inno.Native.ImGui.ImGuiDir.Up = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ffa5783685f7745f"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiDockContext.Nodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a52c021c16c5416"></a>[`public Inno.Native.ImGui.ImVectorImGuiDockNodeSettings Inno.Native.ImGui.ImGuiDockContext.NodesSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6787ebdedac72b3e"></a>[`public Inno.Native.ImGui.ImVectorImGuiDockRequest Inno.Native.ImGui.ImGuiDockContext.Requests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f0032db012694e9"></a>[`public byte Inno.Native.ImGui.ImGuiDockContext.WantFullRebuild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25849) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-28763f272e1d6cd6"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.CentralNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-212287a250a5ee9f"></a>[`public Inno.Native.ImGui.NativeArray_68e25b300381a6fc Inno.Native.ImGui.ImGuiDockNode.ChildNodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25883) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-378591bc97506952"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.CountNodeWithWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25909) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af0558960ca01a21"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiDockNode.HostWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25901) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9cfbc36419a3e9d7"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25869) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a214656dc3e3c20"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.LastBgColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25899) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cbffb8609369541b"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.LastFocusedNodeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25917) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc780a82e9aa4548"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25913) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-522d71c858004949"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25911) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a055b5e879fb2075"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25915) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3052565cf232bedd"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LocalFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25873) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c3ff7b7aebcfb96"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LocalFlagsInWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25875) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-788e196f6de5643e"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.MergedFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25877) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5ba23708724aa92"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.OnlyNodeWithWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25907) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71e80f00248151b6"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.ParentNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25881) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3477c90c8e407e79"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiDockNode.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25889) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c3666060ea429c2"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.RefViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25923) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1de62fec720a946"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.SelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25919) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a878c1c9a9b64d8"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.SharedFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25871) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-50069db99f31c7f5"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiDockNode.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25891) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2dbe0f5a0a0ad70"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiDockNode.SizeRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25893) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81d91c40828df37c"></a>[`public Inno.Native.ImGui.ImGuiAxis Inno.Native.ImGui.ImGuiDockNode.SplitAxis`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25895) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-16b0ea67cb59a23c"></a>[`public Inno.Native.ImGui.ImGuiDockNodeState Inno.Native.ImGui.ImGuiDockNode.State`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25879) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-273b2b6aa06f9262"></a>[`public Inno.Native.ImGui.ImGuiTabBar* Inno.Native.ImGui.ImGuiDockNode.TabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25887) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-afe4842d12545f40"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiDockNode.VisibleWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e532ce708a8cd468"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.WantCloseTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25921) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df9fbd3a16e83d53"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiDockNode.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d6e5a7f91bba7b09"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowPtr Inno.Native.ImGui.ImGuiDockNode.Windows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L25885) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a999bb07e13357d"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.AutoHideTabBar = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-3919153543b48450"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.KeepAliveOnly = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-f9b10e923aaf38b6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingOverCentralNode = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26380) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-c356494214e38f83"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingSplit = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-4723e3f8a62c1532"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoResize = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26383) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-14fd3ea0a9d530c6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoUndocking = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26385) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-fc8a70341509cd22"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-541a6e8aef2d6e32"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.PassthruCentralNode = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-aa08bb3811330f67"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.CentralNode = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26394) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-837e6e806cb51107"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockSpace = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-410624b248e03810"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockedWindowsInFocusRoute = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26401) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262144` |
| <a id="api-31467c4fbd4c0636"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.HiddenTabBar = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-1350f8fb3f915a37"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.LocalFlagsTransferMask = 260208`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26409) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`260208` |
| <a id="api-2062a72ccaf91c6f"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoCloseButton = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-0570c016111719f6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDocking = 7864336`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7864336` |
| <a id="api-31bf3e293fe1cac8"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverEmpty = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26405) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4194304` |
| <a id="api-e2a369798e731db6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverMe = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26403) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1048576` |
| <a id="api-2d7b185044398799"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverOther = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2097152` |
| <a id="api-589c6ef7f2c9406e"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingSplitOther = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`524288` |
| <a id="api-1e4dec7efd09c6c8"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeFlagsMask = 196640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`196640` |
| <a id="api-5596bfbdc43ab9c5"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26399) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-522d2e7c00b01acd"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeY = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-19395a46f6301502"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoTabBar = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26395) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-f525167f49f8984f"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoWindowMenuButton = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26397) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-614c00740e165ef5"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SavedFlagsMask = 261152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`261152` |
| <a id="api-ce421bbe97f8b6d1"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SharedFlagsInheritMask = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26407) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-ea8a3c54424b2084"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseSingleWindow = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26427) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-68d83e6823e1e0e5"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseWindowsAreResizing = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26428) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-6f4d5cfe622b753d"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowVisible = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26429) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-ea41da876a87f544"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.Unknown = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26426) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-f898b77e19d4eb63"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptBeforeDelivery = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26454) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-f5e7e48915eb9799"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptDrawAsHovered = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-101f0599cfd9a748"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoDrawDefaultRect = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26455) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-acde484a0972af2e"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoPreviewTooltip = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26456) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-10ef5dd79d0a1321"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptPeekOnly = 3072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26458) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3072` |
| <a id="api-d15145c419f10c2a"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26445) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-3f355eae0cb27ad2"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadAutoExpire = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26451) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-ca707c1e554cfa1e"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossContext = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26452) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-0198b836614f22cd"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossProcess = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26453) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-1e71d0b09cb05ded"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceAllowNullId = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26449) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-1374667047c4a7d7"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceExtern = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26450) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-3bf079b0d2834924"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoDisableHover = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-5b2b11e02c68dce0"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoHoldToOpenOthers = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-b8a5d66595f80be1"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoPreviewTooltip = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-f02cf74adfa050c5"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfBeginPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c90d803f289c9924"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfColorStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26481) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9b11d46f800d9ccb"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfDisabledStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcadac409cb2a22f"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFocusScopeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-914619ca46481864"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFontStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26485) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e276f1ed5189f152"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfGroupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26489) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e3117d7f5459d4b"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfIdStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26477) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9bd83aa72cb80646"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfItemFlagsStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26491) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-511412a9b16c4f07"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfStyleVarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26483) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf68557ca5e847be"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfTreeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26479) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f1e48a8c8b24b48"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfWindowStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26475) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4196a7b2c7a4b73"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-5f05a812496bdf64"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.RestoreFocusedChild = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-e6d7f7740dd52d76"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.UnlessBelowModal = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-12d06390c0a14585"></a>[`public uint Inno.Native.ImGui.ImGuiFocusScopeData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-173afb55898502aa"></a>[`public uint Inno.Native.ImGui.ImGuiFocusScopeData.WindowId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26539) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-beafb7259b651458"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.AnyWindow = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-2fd951a0e3cdd132"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26511) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-ece639883333e74c"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.DockHierarchy = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26515) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-2cf1e4d8ad21d9e0"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.NoPopupHierarchy = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26514) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-271a75417115f0f2"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26510) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-bb3961076ef734eb"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26516) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-02c277c7ba788224"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.RootWindow = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26512) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ba5c605940367179"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26575) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d1f0f8fc6912ac1a"></a>[`public uint Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-096fa740eff0d33b"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26569) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-094a4d0f13917e47"></a>[`public float Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26571) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9808f65b38937f3"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiGroupData.BackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9d4eed84d9d1089"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiGroupData.BackupCursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26559) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dfcc8ac08b25b26"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiGroupData.BackupCursorPosPrevLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8622e68c1ee85272"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupDeactivatedIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26577) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8f05173f5959611b"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiGroupData.BackupGroupOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26567) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-be7edcd25369440f"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupHoveredIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26579) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b975d270e74562d7"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiGroupData.BackupIndent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0672762cb3858339"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupIsSameLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26581) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-74480013c7dcec70"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.EmitItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26583) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-20fa8b09b95dd80a"></a>[`public uint Inno.Native.ImGui.ImGuiGroupData.WindowId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26557) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a7c6e84c08dbc9a1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByActiveItem = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26605) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-bddc03d9d79bc331"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByPopup = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26604) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-44c29cb6c6e45262"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenDisabled = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-9aea98909bb3d768"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlapped = 768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`768` |
| <a id="api-84dfa71f0425bb7c"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByItem = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26606) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-f0d172182aa750b1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByWindow = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26607) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-c1c3c4cdd62b122d"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AnyWindow = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26601) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-068571d0414937b1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.ChildWindows = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26599) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d2f93567ee8e5703"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayNone = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26615) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-4047f8d865052d33"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayNormal = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26617) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-0a933930e7436547"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayShort = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32768` |
| <a id="api-3c919fc43ec2b09a"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DockHierarchy = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26603) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-73dc70081922a8c0"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.ForTooltip = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26613) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-53927798dcbaa74e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoNavOverride = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-569d1fed0bd148d6"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoPopupHierarchy = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26602) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-ff89039f3136289f"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoSharedDelay = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-3830371d23b30d84"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-16c3a9e304c7250e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RectOnly = 928`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26611) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`928` |
| <a id="api-9dd9742f4826b3e9"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RootAndChildWindows = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-14c8092c5b85651a"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RootWindow = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26600) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-3f5a2f2b7e28464b"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.Stationary = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-6cf83c9ede1ed36e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsItemHovered = 262048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`262048` |
| <a id="api-654d00efdb2533f9"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsWindowHovered = 12479`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26627) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12479` |
| <a id="api-3d444fcd46ad5422"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.DelayMask = 245760`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26626) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`245760` |
| <a id="api-a67572abdcb56c5e"></a>[`public float Inno.Native.ImGui.ImGuiIDStackTool.CopyToClipboardLastTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26645) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-795cb5f135cb95ae"></a>[`public int Inno.Native.ImGui.ImGuiIDStackTool.LastActiveFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccc5e195aa6c2493"></a>[`public byte Inno.Native.ImGui.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce15ac9012098860"></a>[`public byte Inno.Native.ImGui.ImGuiIDStackTool.OptHexEncodeNonAsciiChars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L26639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ece5f3d6a509425d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.AppAcceptingEvents`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4d0f81dccc9ebb8e"></a>[`public byte Inno.Native.ImGui.ImGuiIO.AppFocusLost`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27337) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c097e8c0096fd87d"></a>[`public int Inno.Native.ImGui.ImGuiIO.BackendFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27123) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2c28ffa429dd274"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendLanguageUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27245) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4a16ad28563e6188"></a>[`public sbyte* Inno.Native.ImGui.ImGuiIO.BackendPlatformName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6a9051cade246ae8"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendPlatformUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27241) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-537a2022fbf1b21f"></a>[`public sbyte* Inno.Native.ImGui.ImGuiIO.BackendRendererName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27239) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e41c5dee4bcef5bf"></a>[`public void* Inno.Native.ImGui.ImGuiIO.BackendRendererUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27243) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8c3fb0f856f6df8d"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueLoop`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27231) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d94d6cbe007e5395"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugBeginReturnValueOnce`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27229) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a370c5ffe4ec2890"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflicts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27225) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4acea1642217f2c4"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugHighlightIdConflictsShowItemPicker`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27227) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d56bdf52e163966"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIgnoreFocusLoss`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9789cd96e7e6fdc0"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIniSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efc872d50a8462c5"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDebugIsDebuggerPresent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07c51a5d384b1ab3"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingAlwaysTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27165) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02c286d2cfb612bd"></a>[`public byte Inno.Native.ImGui.ImGuiIO.ConfigDockingNoDockingOver`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27161) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
