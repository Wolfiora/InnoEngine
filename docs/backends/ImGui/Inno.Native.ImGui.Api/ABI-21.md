# Inno.Native.ImGui ABI · ImGuiP → ImGuiSelectionBasicStorage

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiP</code> · 1081–1247</summary>

<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemCalcSize(Inno.Native.ImGui.ImGuiWindow@) 820f6f58b26332e8f7f0d244151d64dc175376ebb9612ac2408c14bb0286ae54 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemCalcSize(Inno.Native.ImGui.ImGuiWindowPtr) c721205b6a2df9bb802b63bbf860a0f60634cf2c6ac4331da655c5e63406c64b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemCalcSize(System.Byte*,System.Byte) 110de9db5d9cb0f4ca03b72c113092ecc7feaf19ed8df6cacd804d9211faf500 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemCalcSize(System.String,System.Boolean) 5a31ce6dde55cd589e8401148267e5953fd64776d479475a980d880d20de0e56 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemEx(Inno.Native.ImGui.ImGuiTabBar@,System.Byte*,System.Byte*,Inno.Native.ImGui.ImGuiTabItemFlags,Inno.Native.ImGui.ImGuiWindow@) 557324c2aabc0a47309aaa3d912cfa09afcc59fb866659823c6408e5b417b5e2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemEx(Inno.Native.ImGui.ImGuiTabBarPtr,System.Byte*,System.Byte*,Inno.Native.ImGui.ImGuiTabItemFlags,Inno.Native.ImGui.ImGuiWindowPtr) bfc07bc0af251d79ff710cce44f40cf65496014046dbd68be688f44898114af6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemEx(Inno.Native.ImGui.ImGuiTabBarPtr,System.String,System.Boolean@,Inno.Native.ImGui.ImGuiTabItemFlags,Inno.Native.ImGui.ImGuiWindowPtr) c8ee8e2d162fee2d9167a94cde062abc0e2a352aa475d82edc1359bd6141011b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(Inno.Native.ImGui.ImDrawList@,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImGuiTabItemFlags,System.Numerics.Vector2,System.Byte*,System.UInt32,System.UInt32,System.Byte,System.Byte*,System.Byte*) 7f002930beea345140c3526bd599875e6e3e8b95e65c630d2d528183f96348eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(Inno.Native.ImGui.ImDrawListPtr,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImGuiTabItemFlags,System.Numerics.Vector2,System.Byte*,System.UInt32,System.UInt32,System.Byte,System.Byte*,System.Byte*) 373bcdb904cb777268170527d0f593f60aac03d4fc5830fb1999976304d056cd -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(Inno.Native.ImGui.ImDrawListPtr,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImGuiTabItemFlags,System.Numerics.Vector2,System.String,System.UInt32,System.UInt32,System.Boolean,System.Boolean@,System.Boolean@) 600e45f8b5748316c3eb810c88f12da35be0a32da43ff959cfd9e167efc3ab4d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemSpacing(System.Byte*,Inno.Native.ImGui.ImGuiTabItemFlags,System.Single) 66511a157cc9c2851b4188a644ada24ca21a9548403573aabd587ee14c5308a9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TabItemSpacing(System.String,Inno.Native.ImGui.ImGuiTabItemFlags,System.Single) c7329067e254ce5e623358cd6b76bc55c5629f889e12522e7f6ed5eca2b512c8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(System.UInt32,System.Single,System.Single,Inno.Native.ImGui.ImGuiTableHeaderData@,System.Int32) 08c7b0ed24453e25c07b6be5a3a54e24f7b57a3145ba92d04ff2e48c866c5b34 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(System.UInt32,System.Single,System.Single,Inno.Native.ImGui.ImGuiTableHeaderDataPtr,System.Int32) cf71243493a63110164eaac9e937430d3a0d06611fd47a6fb3969aa5860956d7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(System.UInt32,System.Single,System.Single,System.ReadOnlySpan{Inno.Native.ImGui.ImGuiTableHeaderData}) c66df18cb58214b5134cc2365493fdc778024e7651b5fcb7c0dee94cc401b3b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginApplyRequests(Inno.Native.ImGui.ImGuiTable@) 82aa6b0bbeaca5edc560942dbd9c953c23daa8b42d6fc657fa80e87812705203 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginApplyRequests(Inno.Native.ImGui.ImGuiTablePtr) 6813a530960e28629d02997cc169c98fc996231b325f81dfe5f0ac5268df0515 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginCell(Inno.Native.ImGui.ImGuiTable@,System.Int32) b1e7ccbff4dbcc84682538d599e7f5ec9466a6e8cee8b7e605bbf161c248cfdc -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginCell(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) e6763d02ec5e8b423bb3f9f5b0822cea90ecdcda72c8162ba74e1460323cc9b6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginContextMenuPopup(Inno.Native.ImGui.ImGuiTable@) 904fc40d761c28378f0681b641ab8fc826a4ea5940a5d732218ecf130251ff1d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginContextMenuPopup(Inno.Native.ImGui.ImGuiTablePtr) 469af9917ff1639dffac41afcca7fb67c7bad4cf2a7d162575ce60ef3f19e91b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(Inno.Native.ImGui.ImGuiTable@,System.Int32) 647e43315ef83c4aa934913df8eb35ad88355c5cb639544029f131ba1d7ce638 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 37c652d32fa26f332c4755acf5e02eacd4d0772609bda67f60de41fbd4c92504 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(System.Span{Inno.Native.ImGui.ImGuiTable}) 82c4dde7f7de9d4d14a78b167086376fb9819384b5b461e8a7117b483aa36927 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginRow(Inno.Native.ImGui.ImGuiTable@) 75081a144123d70e7a866a7a620efcd0315279d1c5d2b0ddd58c2985595bf749 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableBeginRow(Inno.Native.ImGui.ImGuiTablePtr) 97464985016bcdf594d21a26f36046e92234389166c106153935948ac195e62a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableCalcMaxColumnWidth(Inno.Native.ImGui.ImGuiTable@,System.Int32) 96d6226319495a174f7793351febb6a9b57b26e54a833bfd6e81270df943ef61 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableCalcMaxColumnWidth(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 3132c0847ba01ec2e41fe5bc3705049d3c5943dc1a7f11b03c3e6d243816af7b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableDrawBorders(Inno.Native.ImGui.ImGuiTable@) ad25d8eb878297f385fa76b171c9ddbf5d920b188704462c0817fe5d65dceb37 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableDrawBorders(Inno.Native.ImGui.ImGuiTablePtr) d89d4751351f02a987d21e206979fc0b8f366fd89bbc169ebad64dd83aed5b07 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableDrawDefaultContextMenu(Inno.Native.ImGui.ImGuiTable@,Inno.Native.ImGui.ImGuiTableFlags) 1abbd9c81a6c3707e14bb96fba6207271d3ca647a0202cd07cb362bf982c556d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableDrawDefaultContextMenu(Inno.Native.ImGui.ImGuiTablePtr,Inno.Native.ImGui.ImGuiTableFlags) b110b96b08963fdc3c9a1609e5dc377629d40ef5acb26a3850b13de9d3751804 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableEndCell(Inno.Native.ImGui.ImGuiTable@) d6dfce674a4a5d27650319a1a2ef213286f96545972a7648c45ffa50f78cef1a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableEndCell(Inno.Native.ImGui.ImGuiTablePtr) 8d13b428dd23e480d3779fbac24554d04f515839f7a40ac6a75d96f22fbf745f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableEndRow(Inno.Native.ImGui.ImGuiTable@) 1eeb1282ee0564c6a2ccfe749f43e50e1589932891611da083a4eace70931cea -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableEndRow(Inno.Native.ImGui.ImGuiTablePtr) d76c4a5056b7eb4d0a7c8d343967ec54c8fa965fffa83e21469d85e4a00e6b47 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableFindByID(System.UInt32) a5d26907184ba5b7c3ac3a9ada5a599956626467ed37b20072e2aed0bf83ac28 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableFixColumnSortDirection(Inno.Native.ImGui.ImGuiTable@,Inno.Native.ImGui.ImGuiTableColumn@) 2bd741b6d6b3c5f33811b289efef85956b9f06f5a5b867adb9afed923eb33c32 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableFixColumnSortDirection(Inno.Native.ImGui.ImGuiTablePtr,Inno.Native.ImGui.ImGuiTableColumnPtr) a8b6a2c0d89156dc202ca09bdc7cea3a73669a310fab4592e5765121997a97a6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableFixDisplayOrder(Inno.Native.ImGui.ImGuiTable@) c47b0723a21469771d4d51d15eb56df191239cb5d82772e41350db5b1a57307e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableFixDisplayOrder(Inno.Native.ImGui.ImGuiTablePtr) ea4ddb125dcdad6c4998dd169a9eac14553e6e1d845a689ecdbff42fdba85fa5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGcCompactSettings 3684e3d80547b4e3d16f44728d6e3c3e4720cdb46294414c4285b1244bd6c68e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTable@) 7feb1abd073be17f51c887e5f07a944d4b7fa35536af16465917df794cf3e3a6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTablePtr) 597175692a452f50d34152cdc206e21ab130f3718640ebb684a2c32ee5519ea2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTableTempData@) e4798255c5f59cf936b82703420facbf1bdc3aa664e794cbe577869cc037dc98 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTableTempDataPtr) 902fdb9a23b44c25134dcb2bef276e487a90228ee5ce0bd8b21fd4dae0ba54c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetBoundSettings(Inno.Native.ImGui.ImGuiTable@) 060769e3db59a7af7bcfe7a56add174234ec99782da228a5f371d73f80c35503 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetBoundSettings(Inno.Native.ImGui.ImGuiTablePtr) 79aebc7f0201d3c46faf3e7da0dd6cec165689e99c9b6bdb60ece6e29e32fe9c -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetCellBgRect(Inno.Native.ImGui.ImGuiTable@,System.Int32) 261345d88c8e0b54c8c8880ed6fa91607ce31f4d161e561ce2ec02efcc7d248e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetCellBgRect(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 7be4e13dc1670e0ef847546f4e7519e0ec0f24f845b18706f9d563d68e763b2b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnName(Inno.Native.ImGui.ImGuiTable@,System.Int32) 56b5ec82b778683fb559681995a6624b7999c247f4c3b939dbd6d490643471de -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnName(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) a8d60866310e45a986ff00907706576a62fe70f8b9234a9563b23eb81e837032 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnNextSortDirection(Inno.Native.ImGui.ImGuiTableColumn@) 412807f847f7fd21dd65b9f7a2c41034b0325b4c59716e353fcca4e6e7615eff -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnNextSortDirection(Inno.Native.ImGui.ImGuiTableColumnPtr) 26e7920448f4becce18f8f69c484a5c0af39b6fabe9a05aeac8f6402d4f2e474 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(Inno.Native.ImGui.ImGuiTable@,System.Int32,System.Int32) b5a0dc0087a6c5c481ca7e7f96a150104953b501e04e2bc44a11032d0916f6fd -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(Inno.Native.ImGui.ImGuiTablePtr,System.Int32,System.Int32) 85218f052f1f673cb321383f2aa023acbd41c16dc4cb3a1953be276738335b90 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 7b8efaae89f354e8c759b750a53d7f14464e352fc0334e236f352850e6d60a49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnWidthAuto(Inno.Native.ImGui.ImGuiTable@,Inno.Native.ImGui.ImGuiTableColumn@) b5843a38d42381f80533136d47bc5165995dc0efa4c6cf92dd7b7207afa27ef0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetColumnWidthAuto(Inno.Native.ImGui.ImGuiTablePtr,Inno.Native.ImGui.ImGuiTableColumnPtr) cbf34368a8f0a82ca65a5ebc85e9e761b4780c3981dceb43851ad7a62fff54ca -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetHeaderAngledMaxLabelWidth 7958b88cf9c692dfdcc798e8bdc8c98835e1040cd4598129a01c2c85a98ef164 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetHeaderRowHeight 8989d49346b98abd60ec9c791d6fc553da6bc7d9df7ae037b509d62df22183c0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetHoveredRow 532de6a420abae591c7910de959be5f48dde9fdf1cc2eff519c5cc16c461799c -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetInstanceData(Inno.Native.ImGui.ImGuiTable@,System.Int32) 3be5138b17f9a1898f145fef6d9d1e256e4b8d5f6b358eefe89dcbd1af46d472 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetInstanceData(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 9512cbdab791ad3b496aac1c623107f84ecae7665095405bbc2c0cfb0f0ad6c1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetInstanceID(Inno.Native.ImGui.ImGuiTable@,System.Int32) 801a51437dfabefb0284bcd29cd8884068ca75f0f56a48d9f4a980ab0cc6afa3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableGetInstanceID(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 7f489ac8efda30cebc3168b450e1269336d752e0fe4e6ff280ff0ec997523fe2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableLoadSettings(Inno.Native.ImGui.ImGuiTable@) 7478b743f5283325e4d1419e5e5e8409fe8be4457bcdeb0606b0df33d4dec49c -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableLoadSettings(Inno.Native.ImGui.ImGuiTablePtr) 5c6098d6de70e859cdf209c36865d52eb3ad19d68ee621a7022bd79f47539d8a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableMergeDrawChannels(Inno.Native.ImGui.ImGuiTable@) d01ef4f0135f9c0db02e1d2cf34e5ae2530c0a877adc9f078f0a175732f36e8a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableMergeDrawChannels(Inno.Native.ImGui.ImGuiTablePtr) 4b6c39b7d8f9c9e19e3db9ad343402ce68f5ae711ae17138f35aab96a1e16e9e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableOpenContextMenu 351ca7e360ae7d76cdc50e71cd8a63415913c032c9a46b4d043db018e33dfd4b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableOpenContextMenu(System.Int32) 65af31082b975410f4b7017af20a720efc5b811d94bbf86e1f796d063d194c38 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TablePopBackgroundChannel 0f61c94013819008217610e430d18752d38345211d441a3228588b3eacf672cc -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TablePopColumnChannel 597029a15df836fe94d1142cea3271b440f1fc011e5395d0926e7aad3820e258 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TablePushBackgroundChannel 02af7afbce27ef2850b79e6bcb17dbeb6163ea52377554097e33acf78e4958a9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TablePushColumnChannel(System.Int32) 021147ce8dac0a3e012f377fe7cc095aa690d4d337ddb0f92e20a771ea9d2537 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableRemove(Inno.Native.ImGui.ImGuiTable@) 3f53e5635fcce3d3fd6907b7e6e31d4aa9238b2f03629be3b481afcacc2a45ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableRemove(Inno.Native.ImGui.ImGuiTablePtr) 8b93a9808dd7c387c5a633c4f2b04942354b686faae162dff22c79ba83a532c5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableResetSettings(Inno.Native.ImGui.ImGuiTable@) 2a45961c794cf1e2bc83278c81f004670ed57be8115466fee265e28c758c467f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableResetSettings(Inno.Native.ImGui.ImGuiTablePtr) 01c76792046a43d1186262e0954d459cc9b800a41da3f72b6eb44a11dda45621 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSaveSettings(Inno.Native.ImGui.ImGuiTable@) 19e3bd206e6c25591d06ffad6bc16787a6782c741a31b42d59b49589330099de -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSaveSettings(Inno.Native.ImGui.ImGuiTablePtr) 1f1149ad9732be5cd79f1725187c2ca8ff35cd4e98aa654d6b65abb5a1b02401 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnDisplayOrder(Inno.Native.ImGui.ImGuiTable@,System.Int32,System.Int32) e7dd3cecf73e74c843ebafd1bc3b415d9ec1a8fe6c0e4dad403881ed2016456f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnDisplayOrder(Inno.Native.ImGui.ImGuiTablePtr,System.Int32,System.Int32) 48d8070b550fee3dcc7eda88fa45251c7168315492b4f4f96277c544127abd1b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnSortDirection(System.Int32,Inno.Native.ImGui.ImGuiSortDirection,System.Boolean) 97737adb586def7e869d1581a33cdf9f3f8237531a325f6954cdb87d62831e68 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnSortDirection(System.Int32,Inno.Native.ImGui.ImGuiSortDirection,System.Byte) fdf988e9421f5f69242c58f5425a65d76c8685c8c9a6944af88c7ffcbbdabe0f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnWidth(System.Int32,System.Single) 2c97af5793c23c15f77ce36c57544002e8578884c55a3292a02f96d3727fa853 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoAll(Inno.Native.ImGui.ImGuiTable@) 930efae3c02b21115244ac01ccbb7f6c4ebed166f231ccaa7add4d62a1f3d5be -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoAll(Inno.Native.ImGui.ImGuiTablePtr) 30f1f79bf6a1201cbbada0a5ccfc37ed9dedb19b4923a5a0bee7dec9d693f878 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoSingle(Inno.Native.ImGui.ImGuiTable@,System.Int32) 05b61f770559dd6d66fc8cbf6533128f0c4f6f092fa793d45579231c7b3dc25b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoSingle(Inno.Native.ImGui.ImGuiTablePtr,System.Int32) 0424fec8ba20998a6a379d1a869924232f6c26dbba29406e87333461dd8e8a05 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSettingsAddSettingsHandler c23671665ab9128b13dde19d0b069c9c956014659472b1922d2fb433b655173b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSettingsCreate(System.UInt32,System.Int32) 4f54062e3ad664213e7dde2dcb71465fc5381cb6eea2f869f35bb5ecf97a22fb -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSettingsFindByID(System.UInt32) c1056d97de5aa49d363b18adac4266c83ba141e5d5e2363e81d8bc7d91f27c27 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetupDrawChannels(Inno.Native.ImGui.ImGuiTable@) 1b40413832f196806a79ccae47e1d5387666d5e2ba0e313b48569adc3db21abc -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSetupDrawChannels(Inno.Native.ImGui.ImGuiTablePtr) addf0e402ae30cf3ed3a2748ff7e9ad12dc103b635df7af3b2e99124f7dc6a49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSortSpecsBuild(Inno.Native.ImGui.ImGuiTable@) bddbbf2646d164f2e282843c9c30097c24040476ec0ca2300f41dcb8f09adad9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSortSpecsBuild(Inno.Native.ImGui.ImGuiTablePtr) dc0f2a46ffc7bfe1fccb2712eda35ea4e95aad9252e5c15c315b321149ef9d20 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSortSpecsSanitize(Inno.Native.ImGui.ImGuiTable@) bc308bb67470c18ab65fc13c94ad15d3ac3e5f87b4d14d9bb3652ffe1ddf461a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableSortSpecsSanitize(Inno.Native.ImGui.ImGuiTablePtr) b740bc1ac0fc3b76e0f23f2a585a9298409bc5aec29ee4400f7bcf380c348202 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateBorders(Inno.Native.ImGui.ImGuiTable@) 195ccd5879cc03b5cadd312d46c0b1dd2ea8f7cdb2bbba817717ea07bdd0c954 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateBorders(Inno.Native.ImGui.ImGuiTablePtr) b36d13a92139b4121f933f12f70ef944584b81c298ba482b4aa8d0e4f6ce333b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateColumnsWeightFromWidth(Inno.Native.ImGui.ImGuiTable@) 802342c38295d6f95dc59b93eed58a128643ac300ed0e9d15839ec10d269dae5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateColumnsWeightFromWidth(Inno.Native.ImGui.ImGuiTablePtr) 5d7a4853fe9e9efdff4e68db4aa875700173080ab08a6ca80f6ed7eeabd67c29 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateLayout(Inno.Native.ImGui.ImGuiTable@) 7d91fb4124bf869be46d7314935bc97680a644ddbafd4579bd45143e317fb684 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TableUpdateLayout(Inno.Native.ImGui.ImGuiTablePtr) 74d23d6a8f5a33dbe8dd41f6cc8b74700c9c9add0c044e1c5aa77a325d2233d6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TeleportMousePos(System.Numerics.Vector2) e7818085677b6b23246af8bf2292dd47b02f61972e8c0c4e5f201d104e5bf9da -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputIsActive(System.UInt32) feda2908b9673765ef34313f01fe9e1f6aa02a2b564b370ec6799eb501c50112 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect,System.UInt32,System.Byte*,Inno.Native.ImGui.ImGuiDataType,System.Void*,System.Byte*,System.Void*,System.Void*) 801076984e5388722775b817b701fdf688b7506f03ab2fe1c36ed1985ba56802 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect,System.UInt32,System.String,Inno.Native.ImGui.ImGuiDataType,System.Void*,System.String,System.Void*,System.Void*) e78827e40d8a89cbd07f2bdd853fcd322f1e18228f79795491229690188da673 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect,System.UInt32,System.String,Inno.Native.ImGui.ImGuiDataType,System.Void*,System.String,System.Void*) f71136c091104ca50e5460efc2ab82e20f8ac5cda7f0215d712b5d9b2b810cb1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect,System.UInt32,System.String,Inno.Native.ImGui.ImGuiDataType,System.Void*,System.String) 30c2fc38c9f4f8f434c4b969d23fdb73411fd6a4546af0e15c4b0b8960ff5c9d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputText(Inno.Native.ImGui.ImRect,System.UInt32,System.Byte*,System.Byte*,System.Int32,Inno.Native.ImGui.ImGuiInputTextFlags) 6fedf9e212f6534c8e308d22037d36420fb9b76a692af5c8991cbf709cf9aae7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TempInputText(Inno.Native.ImGui.ImRect,System.UInt32,System.String,System.Byte*,System.Int32,Inno.Native.ImGui.ImGuiInputTextFlags) 41be400bf814e149bdcf551fca0e5f34596a87d369a5bcba8042465d5d39630b -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TestKeyOwner(Inno.Native.ImGui.ImGuiKey,System.UInt32) f5cafb648d3f23890a888e7bc90e584422ace0bbdc999586b8b2220d92ed808a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TestShortcutRouting(System.Int32,System.UInt32) 2100c3b38b28748ab92245cc1f2fcbf581d77eacb7bea808f1fdbdcb7859ce97 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextAligned(System.Single,System.Single,System.Byte*) ad188a40e109ef858388fed3dcc0b15ae755f4879069f2e491356045d240a004 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextAligned(System.Single,System.Single,System.String) c7864935ae6b2b4cf2cd69766284dab990b74b84c8aceddd2d7fd5a279bc5fa8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextAlignedV(System.Single,System.Single,System.Byte*,System.Byte*) 967e6a89fac138abbcc935b210ac9c664d57d1498df5a70aec5b2e6633fdcbe2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextAlignedV(System.Single,System.Single,System.String,System.Byte*) 6429e023a9d0ea43d8086a30a173de4aed65e16df3d2c9226a7ee563fda4ca59 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextEx(System.Byte*,System.Byte*,Inno.Native.ImGui.ImGuiTextFlags) 3b753d1fc7172a91e7c7603aa0d116a1785bcec0f719692aa76f0215a1eef1ce -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextEx(System.String,Inno.Native.ImGui.ImGuiTextFlags) 41549688e4111f2cb054450fa150e53970bc75da662849db91b74d310c83f2b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextEx(System.String,System.String,Inno.Native.ImGui.ImGuiTextFlags) 1ac19c4901cd0f441fbb734c419a8e8de0367937875f269e90d7147830b6737e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextEx(System.String,System.String) 393bdf0fbbb77997a5ffb78d4458b525375d7972265eeb3777fb087ae48c7b77 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TextEx(System.String) f668ce0fffb2adff95cdd54d224246f93c666e2741c859845e695f33e1b4347d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TranslateWindowsInViewport(Inno.Native.ImGui.ImGuiViewportP@,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) d1e9ff378ebe13b89622433d50c039cc96e6291951179df34c0b40beca412f89 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TranslateWindowsInViewport(Inno.Native.ImGui.ImGuiViewportPPtr,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 91f638512c4ab881a5d189fa704d47fb971d5bd2017bf72a81cbd3e9d5031d16 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(System.UInt32,Inno.Native.ImGui.ImGuiTreeNodeFlags,System.Byte*,System.Byte*) 89350bdc6b6efcc332ab8180363eec5a9a94f75eca1df3227b16107a6065f247 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(System.UInt32,Inno.Native.ImGui.ImGuiTreeNodeFlags,System.String,System.String) 54ef2a080a97e7039327f26ed215bb9f6eb4db6503d3991afaaab306e84eac5a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(System.UInt32,Inno.Native.ImGui.ImGuiTreeNodeFlags,System.String) 1d042cb8cad5480d2834c34baa103b18c13a5d11d738b46df28ca9de2deec2bb -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToChildNode(System.Numerics.Vector2) 379c20247c0f7cfdb1b9bcd404a1f461bcca195c54fd203096ede73b6a24b76e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToTreePop(Inno.Native.ImGui.ImGuiTreeNodeStackData@) 74503a23f5a1590b9e4e2f5b1f12e4794c803389cb268722acd298dd9fb7793e -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToTreePop(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr) 70d92e601357299d9c9c4b8437775a2bbccfac2456de5bef375ae271896d156d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeGetOpen(System.UInt32) abf07db6a0e785fc653b9d95cfff938141cf27eb707e54b8b75f07aea2a4ca92 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeSetOpen(System.UInt32,System.Boolean) 41838474dbe1948801e4cb63e975140e37fd4e896cfebeac03cbd5c28a9499ae -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeSetOpen(System.UInt32,System.Byte) 7adf43ef049dc555d9f5d3ca7523a7429547fb3597095b0ceb3789f9188280e3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreeNodeUpdateNextOpen(System.UInt32,Inno.Native.ImGui.ImGuiTreeNodeFlags) 2d16c5e5ec968a355f7a6b05a8a2f967c17d86c6883b9fa736e4a88c218f308c -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TreePushOverrideID(System.UInt32) 618ba0b56b7c74070fd78f46b5c91dc569cfbd78d6b3bfd9f923d30600033a57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindBestLeadingMatch(Inno.Native.ImGui.ImGuiTypingSelectRequest@,System.Int32,,System.Void*) c8f5476d5854b6f4ef5c1038824b9fc09e9d5edd60c2ca6a9c9b747a9bf24b03 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindBestLeadingMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr,System.Int32,,System.Void*) 40301e9d85df585f36c9845bece3b68469f17c3cff1627baf5d8d2e3fa10412a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindMatch(Inno.Native.ImGui.ImGuiTypingSelectRequest@,System.Int32,,System.Void*,System.Int32) bbd6e41cca6788273d2a634bf3d77ecedbb902639b86d893fd8cc04024ed4329 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr,System.Int32,,System.Void*,System.Int32) 0eaa68c5b342b0cf8252c58fcde39f7b1d0cd2352e5233102b3416a31227aa9a -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindNextSingleCharMatch(Inno.Native.ImGui.ImGuiTypingSelectRequest@,System.Int32,,System.Void*,System.Int32) 9763f5081d511b75b335d4a87f8061c94abf3856c89934cf71ff7ceab372b8f5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.TypingSelectFindNextSingleCharMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr,System.Int32,,System.Void*,System.Int32) 98bdc471669cb44906da1b29ff38a27d70de4bfe6f926b2e77bcc6d678345668 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UnregisterFontAtlas(Inno.Native.ImGui.ImFontAtlas@) 7660b5e6f7e20d7a169375241f34903181db9bda5290dda48cee053983a8380d -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UnregisterFontAtlas(Inno.Native.ImGui.ImFontAtlasPtr) 2ca69c261e50998a7532df54bfaadf0054279b156ce96536296cdd9f57333ea7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UnregisterUserTexture(Inno.Native.ImGui.ImTextureData@) 13bd3977332540225f5f9848e24762b4c8ae2b46ed2be784386467d89ce38224 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UnregisterUserTexture(Inno.Native.ImGui.ImTextureDataPtr) 13afde772bc28f050fb4454eab633c0044c63779f74a8cd3410b52ad618fe9f2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateCurrentFontSize(System.Single) ec64ddfeb9744166430f29a3aab01e0f7381e45345a5a7c4d41bcc9cc6b1e15f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateHoveredWindowAndCaptureFlags(System.Numerics.Vector2) c062e5abfdc66ff3850b391a2435e56a768b1b1f420a1300cefa9cec96c95b82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateInputEvents(System.Boolean) 118425c6697f85b7f24a588635b916628f6271990a0e167ad6cb0c75f563c367 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateInputEvents(System.Byte) 00c65d3f567cfc791862e3d4018c390eb55aa26288981c0677c936a658038277 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateMouseMovingWindowEndFrame 992b5c99afeadae23bca00e8f0c00be634b475f8d3614cc98b24a905d903ee92 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateMouseMovingWindowNewFrame 023c20590b2d63f957b35fcb602fb80695edb25cabebb7503dd2571c94deb47f -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateWindowParentAndRootLinks(Inno.Native.ImGui.ImGuiWindow@,Inno.Native.ImGui.ImGuiWindowFlags,Inno.Native.ImGui.ImGuiWindow@) 94abfc3f6ee2569f151fe279fd22a1546367581630131169903b40e5f38bea16 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateWindowParentAndRootLinks(Inno.Native.ImGui.ImGuiWindowPtr,Inno.Native.ImGui.ImGuiWindowFlags,Inno.Native.ImGui.ImGuiWindowPtr) abc291e6891738e70d2105b6a3041c3fb4720211a09206f9465023b3b76f2609 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateWindowSkipRefresh(Inno.Native.ImGui.ImGuiWindow@) 159a79c09bb1332be7a38a3941e3b81e64e74b6c7ffffcc69ee8aa4af4fa7ec6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.UpdateWindowSkipRefresh(Inno.Native.ImGui.ImGuiWindowPtr) b7c1872b93f17f97aa50decf5008f1fd55bd662531bf3ba403e9af37a86c4498 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowPosAbsToRel(Inno.Native.ImGui.ImGuiWindow@,System.Numerics.Vector2) 8c005add347b712d2225f09f30cc319a8a4dfd0d20dad3f1a3060da6981e3804 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowPosAbsToRel(Inno.Native.ImGui.ImGuiWindowPtr,System.Numerics.Vector2) c00cbe8bb07ad52df5de719b4526a0a3ce0ab4db92e3c1d843281cfb5b167097 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowPosRelToAbs(Inno.Native.ImGui.ImGuiWindow@,System.Numerics.Vector2) dd9b4f725bfdf01ee212f8549dc62f887a310248b834ec7156f5c705e4eec653 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowPosRelToAbs(Inno.Native.ImGui.ImGuiWindowPtr,System.Numerics.Vector2) a8aa6021858ef94e27cba9e556047405d9935867a24ed859014595995a170a87 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowRectAbsToRel(Inno.Native.ImGui.ImGuiWindow@,Inno.Native.ImGui.ImRect) 3a0fa3497b5e01b468c0e69c0a9ad26cc721c3d9203d2ae116b386a9b76752ba -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowRectAbsToRel(Inno.Native.ImGui.ImGuiWindowPtr,Inno.Native.ImGui.ImRect) c289675ca1fd7bd7a5fd478cd2091c7046b4a89125e9dba77acea4471c8abaea -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowRectRelToAbs(Inno.Native.ImGui.ImGuiWindow@,Inno.Native.ImGui.ImRect) 0a21872a2179358314c082a908152e80886e143ef12d5ab278eac879ebf22045 -->
<!-- api: M:Inno.Native.ImGui.ImGuiP.WindowRectRelToAbs(Inno.Native.ImGui.ImGuiWindowPtr,Inno.Native.ImGui.ImRect) e61eac0dde3a9bcde9561705b33f26353ad89020b47505917c737ece0e657217 -->
<!-- api: T:Inno.Native.ImGui.ImGuiP 1aa1eefdc5a4d38a37a1ff4ef0687f4bfb66f5a16c41c2f625b3baa4e8c7abd8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-320772f51f0b3a07"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.TabItemCalcSize(ref Inno.Native.ImGui.ImGuiWindow window)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59676) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bec496f7256afa27"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.TabItemCalcSize(Inno.Native.ImGui.ImGuiWindowPtr window)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59669) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f16475ce2b712077"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.TabItemCalcSize(byte* label, byte hasCloseButtonOrUnsavedMarker)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59646) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6b15312a1e3888d"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.TabItemCalcSize(string label, bool hasCloseButtonOrUnsavedMarker)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59653) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a208cadcbb2acf1"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TabItemEx(ref Inno.Native.ImGui.ImGuiTabBar tabBar, byte* label, byte* pOpen, Inno.Native.ImGui.ImGuiTabItemFlags flags, ref Inno.Native.ImGui.ImGuiWindow dockedWindow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59591) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9edf42be66a2cc2f"></a>[`public static byte Inno.Native.ImGui.ImGuiP.TabItemEx(Inno.Native.ImGui.ImGuiTabBarPtr tabBar, byte* label, byte* pOpen, Inno.Native.ImGui.ImGuiTabItemFlags flags, Inno.Native.ImGui.ImGuiWindowPtr dockedWindow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1dd9a56ae0b963ab"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TabItemEx(Inno.Native.ImGui.ImGuiTabBarPtr tabBar, string label, ref bool pOpen, Inno.Native.ImGui.ImGuiTabItemFlags flags, Inno.Native.ImGui.ImGuiWindowPtr dockedWindow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f3534676f963fc59"></a>[`public static void Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(ref Inno.Native.ImGui.ImDrawList drawList, Inno.Native.ImGui.ImRect bb, Inno.Native.ImGui.ImGuiTabItemFlags flags, System.Numerics.Vector2 framePadding, byte* label, uint tabId, uint closeButtonId, byte isContentsVisible, byte* outJustClosed, byte* outTextClipped)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59707) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6c39e6973863e92"></a>[`public static void Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(Inno.Native.ImGui.ImDrawListPtr drawList, Inno.Native.ImGui.ImRect bb, Inno.Native.ImGui.ImGuiTabItemFlags flags, System.Numerics.Vector2 framePadding, byte* label, uint tabId, uint closeButtonId, byte isContentsVisible, byte* outJustClosed, byte* outTextClipped)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59701) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8c328865dcaceb7"></a>[`public static void Inno.Native.ImGui.ImGuiP.TabItemLabelAndCloseButton(Inno.Native.ImGui.ImDrawListPtr drawList, Inno.Native.ImGui.ImRect bb, Inno.Native.ImGui.ImGuiTabItemFlags flags, System.Numerics.Vector2 framePadding, string label, uint tabId, uint closeButtonId, bool isContentsVisible, ref bool outJustClosed, ref bool outTextClipped)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59716) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e0279c72dcb48f0a"></a>[`public static void Inno.Native.ImGui.ImGuiP.TabItemSpacing(byte* strId, Inno.Native.ImGui.ImGuiTabItemFlags flags, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59624) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-076b231425603d37"></a>[`public static void Inno.Native.ImGui.ImGuiP.TabItemSpacing(string strId, Inno.Native.ImGui.ImGuiTabItemFlags flags, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49c290cfaefd2aa4"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(uint rowId, float angle, float maxLabelWidth, ref Inno.Native.ImGui.ImGuiTableHeaderData data, int dataCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58556) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fda9e09bc258792b"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(uint rowId, float angle, float maxLabelWidth, Inno.Native.ImGui.ImGuiTableHeaderDataPtr data, int dataCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58550) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d91a9e1c3542a5e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableAngledHeadersRowEx(uint rowId, float angle, float maxLabelWidth, System.ReadOnlySpan<Inno.Native.ImGui.ImGuiTableHeaderData> data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58565) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8568e56baf93f062"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginApplyRequests(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58698) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1caed5f10ce380b7"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginApplyRequests(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58692) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-104c261be1c4c8eb"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginCell(ref Inno.Native.ImGui.ImGuiTable table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58999) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca70ce8a671937da"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginCell(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58993) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f63318f124d80afd"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TableBeginContextMenuPopup(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58797) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8911f462bc9bb33"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TableBeginContextMenuPopup(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58807) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e0bbdf58c805b915"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(ref Inno.Native.ImGui.ImGuiTable table, int columnsCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a91608655ca0010d"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(Inno.Native.ImGui.ImGuiTablePtr table, int columnsCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58668) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95584010fae8f77f"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginInitMemory(System.Span<Inno.Native.ImGui.ImGuiTable> table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58683) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74be14df8ac89f08"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginRow(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58969) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09d4a26e06f97e1e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableBeginRow(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58963) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9d13ed45e1cd704"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableCalcMaxColumnWidth(ref Inno.Native.ImGui.ImGuiTable table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59089) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4c2cef207d90b65"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableCalcMaxColumnWidth(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59082) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1960eca6c896f85"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableDrawBorders(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58773) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4bcbcb932ca2ba0b"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableDrawBorders(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58767) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-132667d58830ff41"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableDrawDefaultContextMenu(ref Inno.Native.ImGui.ImGuiTable table, Inno.Native.ImGui.ImGuiTableFlags flagsForSectionToDisplay)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58788) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce95982416a8a898"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableDrawDefaultContextMenu(Inno.Native.ImGui.ImGuiTablePtr table, Inno.Native.ImGui.ImGuiTableFlags flagsForSectionToDisplay)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58782) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a07f03821ab5039"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableEndCell(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59014) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-155fa3e901f17c35"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableEndCell(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59008) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dbc8cc02d12e006"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableEndRow(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58984) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6532b29df555f53"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableEndRow(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58978) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a1ae127f45ce872"></a>[`public static Inno.Native.ImGui.ImGuiTablePtr Inno.Native.ImGui.ImGuiP.TableFindByID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58581) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49de9a91a50dcb44"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableFixColumnSortDirection(ref Inno.Native.ImGui.ImGuiTable table, ref Inno.Native.ImGui.ImGuiTableColumn column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06a0de37d015068a"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableFixColumnSortDirection(Inno.Native.ImGui.ImGuiTablePtr table, Inno.Native.ImGui.ImGuiTableColumnPtr column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e37fe7b7070c32d"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableFixDisplayOrder(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58869) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d60e7484dfaab63"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableFixDisplayOrder(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58863) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e723686d8b4ceb4e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableGcCompactSettings()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59189) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97e32a8ae99d3e73"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59165) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9fc5157dd377aa49"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59159) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82186870cc74eadc"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(ref Inno.Native.ImGui.ImGuiTableTempData table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59180) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0cd60a2a0e6d20d"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableGcCompactTransientBuffers(Inno.Native.ImGui.ImGuiTableTempDataPtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59174) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76fbcb7242b7f3df"></a>[`public static Inno.Native.ImGui.ImGuiTableSettings* Inno.Native.ImGui.ImGuiP.TableGetBoundSettings(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c81c840cac3b73a"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr Inno.Native.ImGui.ImGuiP.TableGetBoundSettings(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59240) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb8167de692b5d09"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.TableGetCellBgRect(ref Inno.Native.ImGui.ImGuiTable table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59030) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5fe8bc8672bea556"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.TableGetCellBgRect(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59023) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-802d06e18ea04b2d"></a>[`public static byte* Inno.Native.ImGui.ImGuiP.TableGetColumnName(ref Inno.Native.ImGui.ImGuiTable table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59040) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-028e8f137ca6604e"></a>[`public static string? Inno.Native.ImGui.ImGuiP.TableGetColumnName(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59050) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c44d811e5520b6c"></a>[`public static Inno.Native.ImGui.ImGuiSortDirection Inno.Native.ImGui.ImGuiP.TableGetColumnNextSortDirection(ref Inno.Native.ImGui.ImGuiTableColumn column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b29e88ccb9a287a"></a>[`public static Inno.Native.ImGui.ImGuiSortDirection Inno.Native.ImGui.ImGuiP.TableGetColumnNextSortDirection(Inno.Native.ImGui.ImGuiTableColumnPtr column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58908) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d327c61a98866d12"></a>[`public static uint Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(ref Inno.Native.ImGui.ImGuiTable table, int columnN, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59064) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9dd5294fb010b5cc"></a>[`public static uint Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(Inno.Native.ImGui.ImGuiTablePtr table, int columnN, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a18025ea3e7fed4e"></a>[`public static uint Inno.Native.ImGui.ImGuiP.TableGetColumnResizeID(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7bde4480deda60ea"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableGetColumnWidthAuto(ref Inno.Native.ImGui.ImGuiTable table, ref Inno.Native.ImGui.ImGuiTableColumn column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58950) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4a62e919eedaa34"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableGetColumnWidthAuto(Inno.Native.ImGui.ImGuiTablePtr table, Inno.Native.ImGui.ImGuiTableColumnPtr column)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58943) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2f25458617efe29"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableGetHeaderAngledMaxLabelWidth()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58519) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1fd52c62bdbc4553"></a>[`public static float Inno.Native.ImGui.ImGuiP.TableGetHeaderRowHeight()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58512) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4059b8912e493814"></a>[`public static int Inno.Native.ImGui.ImGuiP.TableGetHoveredRow()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c054f5222ea8e3f3"></a>[`public static Inno.Native.ImGui.ImGuiTableInstanceData* Inno.Native.ImGui.ImGuiP.TableGetInstanceData(ref Inno.Native.ImGui.ImGuiTable table, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58836) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ec397bb01fba0cb1"></a>[`public static Inno.Native.ImGui.ImGuiTableInstanceDataPtr Inno.Native.ImGui.ImGuiP.TableGetInstanceData(Inno.Native.ImGui.ImGuiTablePtr table, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58829) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-771cb31e683a9af3"></a>[`public static uint Inno.Native.ImGui.ImGuiP.TableGetInstanceID(ref Inno.Native.ImGui.ImGuiTable table, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c386644395b663b6"></a>[`public static uint Inno.Native.ImGui.ImGuiP.TableGetInstanceID(Inno.Native.ImGui.ImGuiTablePtr table, int instanceNo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58846) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-037ecc4c68f52e7a"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableLoadSettings(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59201) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5d262f60009bfab"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableLoadSettings(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59195) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ade684eae91c7209"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableMergeDrawChannels(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0765b841832ee315"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableMergeDrawChannels(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58814) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7035db40cc74c179"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableOpenContextMenu()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4bbaa70009d8e32f"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableOpenContextMenu(int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e2825d50bd3f48ed"></a>[`public static void Inno.Native.ImGui.ImGuiP.TablePopBackgroundChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58532) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fba84ddb2687a248"></a>[`public static void Inno.Native.ImGui.ImGuiP.TablePopColumnChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58544) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ee63b6608a1a1dd"></a>[`public static void Inno.Native.ImGui.ImGuiP.TablePushBackgroundChannel()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58526) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0cb5f0960b09fe3"></a>[`public static void Inno.Native.ImGui.ImGuiP.TablePushColumnChannel(int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0599a8964c1dc7e8"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableRemove(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59150) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e575b06d1e58325"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableRemove(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59144) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d40e0da4d9af7991"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableResetSettings(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8bf1884879f4a58e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableResetSettings(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59225) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77268024148a7569"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSaveSettings(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59216) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-517a7d81661f81b6"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSaveSettings(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59210) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eac7b53cf4c7c39d"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnDisplayOrder(ref Inno.Native.ImGui.ImGuiTable table, int columnN, int dstOrder)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59135) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b84ef7a5feb82d9"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnDisplayOrder(Inno.Native.ImGui.ImGuiTablePtr table, int columnN, int dstOrder)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59129) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-07eff53397cd1ed8"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnSortDirection(int columnN, Inno.Native.ImGui.ImGuiSortDirection sortDirection, bool appendToSortSpecs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3844d9a6cc17fc0"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnSortDirection(int columnN, Inno.Native.ImGui.ImGuiSortDirection sortDirection, byte appendToSortSpecs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a14424229157133"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnWidth(int columnN, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58487) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71718febdf227fba"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoAll(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59120) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-66c6c9bdee1f891f"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoAll(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59114) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24b095a08ed167f2"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoSingle(ref Inno.Native.ImGui.ImGuiTable table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59105) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d1971f96320de6b6"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetColumnWidthAutoSingle(Inno.Native.ImGui.ImGuiTablePtr table, int columnN)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59099) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79d3e54eaa5220f5"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSettingsAddSettingsHandler()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59257) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1403610003dee1ca"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr Inno.Native.ImGui.ImGuiP.TableSettingsCreate(uint id, int columnsCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59263) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b331eb4ddc88418"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr Inno.Native.ImGui.ImGuiP.TableSettingsFindByID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59270) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bad05b2c9aa09639"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetupDrawChannels(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95338d3a1f310fb6"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSetupDrawChannels(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58707) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d47db6548ca9b5cb"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSortSpecsBuild(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58899) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87fa55e462e2157b"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSortSpecsBuild(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58893) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eebf3f40e49d81a2"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSortSpecsSanitize(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58884) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bdfd664cbd88dc89"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableSortSpecsSanitize(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58878) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0a0145686bc0187"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateBorders(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58743) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0a87c670240251b"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateBorders(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58737) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c74c7ef7c467ac9f"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateColumnsWeightFromWidth(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58758) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-499bb3dd19665522"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateColumnsWeightFromWidth(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58752) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee8bdd6967ec37a7"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateLayout(ref Inno.Native.ImGui.ImGuiTable table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58728) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e47381acca622c24"></a>[`public static void Inno.Native.ImGui.ImGuiP.TableUpdateLayout(Inno.Native.ImGui.ImGuiTablePtr table)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58722) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89403abc6a90e41e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TeleportMousePos(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L57165) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31bffe5982df4c6b"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TempInputIsActive(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61108) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e625ef8e4645b627"></a>[`public static byte Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect bb, uint id, byte* label, Inno.Native.ImGui.ImGuiDataType dataType, void* pData, byte* format, void* pClampMin, void* pClampMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61064) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11a90f2e1bd9e9d1"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect bb, uint id, string label, Inno.Native.ImGui.ImGuiDataType dataType, void* pData, string format, void* pClampMin, void* pClampMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f0e1be9edf63f6f"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect bb, uint id, string label, Inno.Native.ImGui.ImGuiDataType dataType, void* pData, string format, void* pClampMin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61102) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-162c26a36668995a"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TempInputScalar(Inno.Native.ImGui.ImRect bb, uint id, string label, Inno.Native.ImGui.ImGuiDataType dataType, void* pData, string format)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61094) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5be5f0500ec798e0"></a>[`public static byte Inno.Native.ImGui.ImGuiP.TempInputText(Inno.Native.ImGui.ImRect bb, uint id, byte* label, byte* buf, int bufSize, Inno.Native.ImGui.ImGuiInputTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61040) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96e99761c686b778"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TempInputText(Inno.Native.ImGui.ImRect bb, uint id, string label, byte* buf, int bufSize, Inno.Native.ImGui.ImGuiInputTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61047) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f3eb9bf53a366b69"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TestKeyOwner(Inno.Native.ImGui.ImGuiKey key, uint ownerId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L57225) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87f2290eff1ad7b6"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TestShortcutRouting(int keyChord, uint ownerId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L57343) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea105876869c48a4"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextAligned(float alignX, float sizeX, byte* fmt)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a2d2e0ef4a20372c"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextAligned(float alignX, float sizeX, string fmt)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60297) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e8e0b348c802d1e"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextAlignedV(float alignX, float sizeX, byte* fmt, byte* args)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2a860dfb38108df"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextAlignedV(float alignX, float sizeX, string fmt, byte* args)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60319) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d18ac96bbf7bd21"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextEx(byte* text, byte* textEnd, Inno.Native.ImGui.ImGuiTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60241) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ebdc49d88ca94f0"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextEx(string text, Inno.Native.ImGui.ImGuiTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60285) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1996b8c46850e7aa"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextEx(string text, string? textEnd, Inno.Native.ImGui.ImGuiTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c3646c394075409"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextEx(string text, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60277) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86705800a1215b93"></a>[`public static void Inno.Native.ImGui.ImGuiP.TextEx(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60269) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-66ca016d1b794d7a"></a>[`public static void Inno.Native.ImGui.ImGuiP.TranslateWindowsInViewport(ref Inno.Native.ImGui.ImGuiViewportP viewport, System.Numerics.Vector2 oldPos, System.Numerics.Vector2 newPos, System.Numerics.Vector2 oldSize, System.Numerics.Vector2 newSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55633) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-94aab5d3227cd478"></a>[`public static void Inno.Native.ImGui.ImGuiP.TranslateWindowsInViewport(Inno.Native.ImGui.ImGuiViewportPPtr viewport, System.Numerics.Vector2 oldPos, System.Numerics.Vector2 newPos, System.Numerics.Vector2 oldSize, System.Numerics.Vector2 newSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55627) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6f64168104b3b14"></a>[`public static byte Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(uint id, Inno.Native.ImGui.ImGuiTreeNodeFlags flags, byte* label, byte* labelEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d4c05eb0a4e392e"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(uint id, Inno.Native.ImGui.ImGuiTreeNodeFlags flags, string label, string? labelEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d045cde4bdf15284"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TreeNodeBehavior(uint id, Inno.Native.ImGui.ImGuiTreeNodeFlags flags, string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6e87ec8dbbdef7c"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToChildNode(System.Numerics.Vector2 targetPos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60829) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-22225cf1bc74fc05"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToTreePop(ref Inno.Native.ImGui.ImGuiTreeNodeStackData data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60841) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db6eb318fcacfc63"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreeNodeDrawLineToTreePop(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff48c7aded91b62f"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TreeNodeGetOpen(uint storageId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60856) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e89a4d7818e8c37"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreeNodeSetOpen(uint storageId, bool open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60869) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c79a4db6d698ecc"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreeNodeSetOpen(uint storageId, byte open)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60863) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2181f63497eea800"></a>[`public static bool Inno.Native.ImGui.ImGuiP.TreeNodeUpdateNextOpen(uint storageId, Inno.Native.ImGui.ImGuiTreeNodeFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60875) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2063dc373b72084b"></a>[`public static void Inno.Native.ImGui.ImGuiP.TreePushOverrideID(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L60850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a3ac28810d1132a"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindBestLeadingMatch(ref Inno.Native.ImGui.ImGuiTypingSelectRequest req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58205) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e2a2984c08ce5de"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindBestLeadingMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58198) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e7516ba4012c900e"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindMatch(ref Inno.Native.ImGui.ImGuiTypingSelectRequest req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData, int navItemIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9df8a27b00d61d80"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData, int navItemIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58164) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5fe27b06b634fb8"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindNextSingleCharMatch(ref Inno.Native.ImGui.ImGuiTypingSelectRequest req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData, int navItemIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58188) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-142f4b1ac01698ba"></a>[`public static int Inno.Native.ImGui.ImGuiP.TypingSelectFindNextSingleCharMatch(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr req, int itemsCount, delegate* unmanaged[Cdecl]<void*, int, byte*> getItemNameFunc, void* userData, int navItemIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L58181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d90e2803c43cb767"></a>[`public static void Inno.Native.ImGui.ImGuiP.UnregisterFontAtlas(ref Inno.Native.ImGui.ImFontAtlas atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-94e461ea78e92fc7"></a>[`public static void Inno.Native.ImGui.ImGuiP.UnregisterFontAtlas(Inno.Native.ImGui.ImFontAtlasPtr atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b04538d54aeb0557"></a>[`public static void Inno.Native.ImGui.ImGuiP.UnregisterUserTexture(ref Inno.Native.ImGui.ImTextureData tex)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55304) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f3a89b7e16bc6f7e"></a>[`public static void Inno.Native.ImGui.ImGuiP.UnregisterUserTexture(Inno.Native.ImGui.ImTextureDataPtr tex)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55298) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-281a14dd9146798e"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateCurrentFontSize(float restoreFontSizeAfterScaling)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55358) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9041b92bce183e28"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateHoveredWindowAndCaptureFlags(System.Numerics.Vector2 mousePos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55552) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a72ca8ec89f436cb"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateInputEvents(bool trickleFastInputs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55546) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-923f4cb6f348ee14"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateInputEvents(byte trickleFastInputs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55540) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e4fda927c75bb9e"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateMouseMovingWindowEndFrame()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55621) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8324132ab3a5e712"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateMouseMovingWindowNewFrame()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55615) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6de609a710de95c2"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateWindowParentAndRootLinks(ref Inno.Native.ImGui.ImGuiWindow window, Inno.Native.ImGui.ImGuiWindowFlags flags, ref Inno.Native.ImGui.ImGuiWindow parentWindow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L54800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c37b430225c35de"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateWindowParentAndRootLinks(Inno.Native.ImGui.ImGuiWindowPtr window, Inno.Native.ImGui.ImGuiWindowFlags flags, Inno.Native.ImGui.ImGuiWindowPtr parentWindow)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L54794) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-587bc48664676fc6"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateWindowSkipRefresh(ref Inno.Native.ImGui.ImGuiWindow window)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L54818) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-737bdf2e7d01e8a3"></a>[`public static void Inno.Native.ImGui.ImGuiP.UpdateWindowSkipRefresh(Inno.Native.ImGui.ImGuiWindowPtr window)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L54812) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-073188c225f36ca3"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.WindowPosAbsToRel(ref Inno.Native.ImGui.ImGuiWindow window, System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55109) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-169a631519581763"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.WindowPosAbsToRel(Inno.Native.ImGui.ImGuiWindowPtr window, System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55102) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7dd8ec8586e669f7"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.WindowPosRelToAbs(ref Inno.Native.ImGui.ImGuiWindow window, System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-195e3783791c51fe"></a>[`public static System.Numerics.Vector2 Inno.Native.ImGui.ImGuiP.WindowPosRelToAbs(Inno.Native.ImGui.ImGuiWindowPtr window, System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55119) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-112fc03ce051ac61"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.WindowRectAbsToRel(ref Inno.Native.ImGui.ImGuiWindow window, Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55075) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c98be5e643e36e96"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.WindowRectAbsToRel(Inno.Native.ImGui.ImGuiWindowPtr window, Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55068) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8ee359b05d36efe"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.WindowRectRelToAbs(ref Inno.Native.ImGui.ImGuiWindow window, Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55092) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca090480533a4c69"></a>[`public static Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiP.WindowRectRelToAbs(Inno.Native.ImGui.ImGuiWindowPtr window, Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L55085) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce389aa11e37b2fc"></a>[`public class Inno.Native.ImGui.ImGuiP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L52581) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPayload</code> · 48 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPayload 8cfcbabe3c7c5ee70ecc14b421b997646e43872f0ef26dacd2f880a9d38708cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Data 562a00350ee26ea15a51f78273aed9638150a5316f18f9754190ee4dc9065440 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataSize 8dcd980ac92db691ef377c19571f802640453761becbd25a84f70e5325277931 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.SourceId ea5d94e28eb4a99627f9dfb013e534b1dcb50b55ffac6b3bd7ba6d150b5c6271 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.SourceParentId 5fca869d9fe35af22ec3652431b7a6f2915668681ea9f3e61909d38b060bc4b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataFrameCount e2266b09d99807bacb9d4473923aa13b77c931c3bc8a2eeb655e9362df9f9238 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_0 8eefece3b6458d8ae52cac36a7fd4ba034e2ed9093fd53102041c64c768378bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_1 ce340437ffd265525b48837c0256254cd61e487c19ad4956bf4060a711954077 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_2 b43503f9aa0282951a85695bda7a9b19890f3ad5f60990487cb6ebee723d592b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_3 6e4a4f4318950b9a57d005ece59f0662f0d22e082432e59e2f9c4424ec2f41fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_4 636ff71cd39776454ca204d8d1d6271e5d8c48d080a79102e233b327d32ec90c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_5 ba655d7adc17209476cec511f045fb0c4b6e5580f3a5a51880a94f0d74614bc5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_6 614615b994d7e12d1b536666f2275fc1becdd4c81e3efdfb00f604750d4b2514 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_7 92297170467162b4023d403c80968e76c111f18303eab60320a0bec56d5a74c3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_8 513607a937710a58fd874508f839f8ee9a6f47e92186c29d8683c6f1a6dc9ecb -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_9 a50314f632e96a699714c93b78f5db8454d4beb74eff3b2493d02fe859b1cf1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_10 ebcca426a7241ac4a4b3b63d568ec839d3710b40ecd368347c32b43e3973f558 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_11 24ca8facb5c239aa65178553d63e7c96bde6f39b447f91f17d940280ac26d273 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_12 9b603c622b2e787f08a654a031134b78180642a980b526e8535e3cf370dec5f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_13 5053a858bc533ee314114faf8a728fa883bde6f58fbceeeb2cb84c93f771ae2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_14 d2ad204773f87ca28f6e1a146619a106996a9a80e5f324f29f3f138eb0ee97d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_15 014b7e1bf22503751ca7dd402a2c78093f2709fe4d45e6f75ced82846e4e56af -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_16 020d83839c5a9bd6e468c3d6d5a319faf999fb9e0bcbdcd40fb8846e9884b14e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_17 50c9109ed5314c623e039baac3e5321cbfa72e710609422a98f516fd52f611ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_18 1da2e38cba6ce1c00f77fdcf14ba42818564a6dd23fa4463f5066ccaf319b0a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_19 7a7b8aa6141f702bf47a1a79669974c6eb7624029929d89053544c1247c6a740 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_20 3cae9cf1b1611818e69b3ec0d4f631b609fee302c9cdeb90f88063ba4341b105 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_21 efc085967f7cd2528bb98352b3be72ee7dfb9ae30d92011d44cfa4d4ce8428c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_22 b108fc24499b2c9b5f0917cc4f4fefca6d52fb049587310a0a12915bbf7e09d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_23 5b983b782835e7bca5e1a0430828e65537d95b4e3958d81dcd9117d77da3f1f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_24 434c5064b14f7dbe6cc993371097a76844259d0f05c234b23851017e9065dcf8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_25 5f01e96cb26d54a6bbbb01a06383c1b04d8e5ba92896080d5c5d72134843d68e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_26 8323c4f41de99c4e7b7f03bce2a08b66ba165b806cfb8bd48903a76878752557 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_27 aeb55956632bcf7565935d4166565af50f09f3153683bc4c5c2de71547ff3b56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_28 c5fcb92ed8e800a46f656fd34f7cb45a71ae38a8a781834f876421888456b1e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_29 25f03b77a6100300f8f4a1d2ce2317a113f9eb470e3afcd0aff1f1e139ef2af1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_30 c332ef981d9909b62198ec2409551acc13389d8551c722d4a3fcfb7a32a90bbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_31 839115c9b197ed337b4529a28ae76a0257ef4d764c20fca01902aca433b28b2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.DataType_32 dee5754d02c5a79b9a37e92f96fec6e881489d61d2d1b1b8b4a9407cbd365b0f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayload.DataType 749aa94d053e8cfe620525b5ae2d52a2dcd5d22f5a2b2b14d6beecf811797e04 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Preview 2defe3bb44aa2c09e4330f60d7b9c2385406e7f9dc7961c19e0ddf6cd76a9de2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayload.Delivery 24cc9088a8882878dc04363e750557c22f8f40c747635d5c532ad60ca64fb5b6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.#ctor(System.Void*,System.Int32,System.UInt32,System.UInt32,System.Int32,System.Byte*,System.Byte,System.Byte) dcb676114ce3f3b2fb07b2aa23abb128417e63b135ed2438eee8969da8da891f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.Destroy c9a2f4992476d5f7bb745569f7576bdf5553904a6d4c72357e0ec7bf85198c26 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.Clear 6bbca5e1f50e6b0d62456841e9cb032c286315dd16ff17729fc64ad1ad8c46aa -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.IsDataType(System.Byte*) e00071d94bf0bf0507e49dc98372eb1a605e0edb7503863fa87319bda265829d -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.IsPreview 807a43f4ced622bcd80f71b860345ed944b95127d4f4d48e18ceb4b70156e9d6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayload.IsDelivery 2053e68603d00447cb4942590525346b121b9b0f12c711bb1ff7659a32a1ec94 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-39f9e851d55ad8fc"></a>[`public void* Inno.Native.ImGui.ImGuiPayload.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5876) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c9f71e771b3bbb1"></a>[`public int Inno.Native.ImGui.ImGuiPayload.DataFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5880) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c3abfa58133b190"></a>[`public int Inno.Native.ImGui.ImGuiPayload.DataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5aecbf7d1041f8a9"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad681440dc780d70"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5882) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c09468a5e91c31cf"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-713ae0e3a61edaed"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c1e7e3254ab9f0f"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0482bdd14146f6fc"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37fcdf0431bbbbc6"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c022b44bfd075e2"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b118589f9714e85"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5acb498b90d1ad9d"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5898) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de2ef956bb6a5f1f"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5899) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fc1729d9409a0ef"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d7ef87f313a9e50"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5883) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-344c464b360647f7"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5901) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1c8a5cf9482a4f7"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45a5c1f1353c2af2"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22922af75003e629"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-368445b4163f095a"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6094bd4e31dcf53"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b645e9afbe7cac64"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7dba050ce1346f4"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acc3c5a8c16d758e"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2de6cebd83a96c9"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-945ae0fe5ec98794"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b95ac9d40342a913"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4331846709e3e38f"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58dba2cb1954f3af"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5913) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91322bb4ce9f7247"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73c415867c0c5312"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-745c7d14ccc05191"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36a445e96810bb71"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61214c0d937a79d0"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91817dad8897f3ca"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.DataType_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3eb3d01446d342da"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.Delivery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed202d46716097ca"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.Preview`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5916) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efdc0d7ccad82a8b"></a>[`public uint Inno.Native.ImGui.ImGuiPayload.SourceId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5878) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11e30ff0f51c6411"></a>[`public uint Inno.Native.ImGui.ImGuiPayload.SourceParentId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5879) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3c20a0925dafe97"></a>[`public Inno.Native.ImGui.ImGuiPayload.ImGuiPayload(void* data = null, int dataSize = 0, uint sourceId = 0, uint sourceParentId = 0, int dataFrameCount = 0, byte* dataType = null, byte preview = 0, byte delivery = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5918) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9390cbf451d0a7e9"></a>[`public void Inno.Native.ImGui.ImGuiPayload.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63542) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9b0fa16194ca8ba"></a>[`public void Inno.Native.ImGui.ImGuiPayload.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c886617e64cf27d"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.IsDataType(byte* type)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a4ef10ead855c7b"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.IsDelivery()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63575) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e3442853d53af1b3"></a>[`public byte Inno.Native.ImGui.ImGuiPayload.IsPreview()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63564) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a104336414777fca"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiPayload.DataType { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5914) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-586ea73697d8b23b"></a>[`public struct Inno.Native.ImGui.ImGuiPayload`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5874) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPayloadPtr</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPayloadPtr 5dc8cffbfe0580284e31ffdc88d93691f4fe72869426e771c3ba06663fe30df7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.#ctor(Inno.Native.ImGui.ImGuiPayload*) 73bb5b92938e1cb5ac3e5dd1c07e5d44a459fae3839f78fa432d5613d7a3a2d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPayloadPtr.Handle 9fde39291bd04d0e35aa0892c547571d3c7c7a6c7471a0136350922141004974 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.IsNull 62a58712d72cf4b85bded5bdbaee43fff2952a584caffa0e5155a5b16dba9ef6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.Null 8119d8fabb9cb7a9608fc2d6ba5277a35e3c6e076c9de0f7b64ba34add9ec102 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.Item(System.Int32) a744783b3e266b90bc77102071bfa3f87dbf615940ef8dd08580bf13734f1000 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.op_Implicit(Inno.Native.ImGui.ImGuiPayload*)~Inno.Native.ImGui.ImGuiPayloadPtr e3a7efd4b7aa6b27975e09c749466c9c8e3af2101efd3d36dc8fb0d5658777b5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.op_Implicit(Inno.Native.ImGui.ImGuiPayloadPtr)~Inno.Native.ImGui.ImGuiPayload* 8b04e296a8281ddfec7a51aa028a443db76cbf554806a40032d8e5ede2a1d394 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.op_Equality(Inno.Native.ImGui.ImGuiPayloadPtr,Inno.Native.ImGui.ImGuiPayloadPtr) 5eb1228ef3e2dce1ef49e4f5a1cfb1fc7b03f49a8a1dc1012fed270ba06116ce -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.op_Inequality(Inno.Native.ImGui.ImGuiPayloadPtr,Inno.Native.ImGui.ImGuiPayloadPtr) e93277311a6bb76e1932776f0db388bcdfae36d9e558761276007b059ed98b2b -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.Equals(Inno.Native.ImGui.ImGuiPayloadPtr) 09c6e5b0e130a52e94e732fc110ca85cc598878501d7c755b8c4a23d9e1e8a00 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.Equals(System.Object) e099ec9c9e6447f92d985329212b97ca23467f59694343f3183b2e4e3f04e0cd -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.GetHashCode 4dab52fede74407f313c2f6a509651751fcc9debbae889892cf06bd41d9e7de1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.Data bd9eba5d367adf9ff3b08b32e6599c01b6fbfca8f6fc639d39db37386ef2f404 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.DataSize 644ee0d77f79ebea8ca17de79abc19b5a5a2b799c6a0f3bd6f75fa0d16393548 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.SourceId ad4dc84e64e95312231438d2badac24e8b9837cf03249a5e45fe428294221b83 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.SourceParentId 0aacaba45e43916a87389007ab14df9bc6c117e7d1429c8e3f3cc5c9a95a5f36 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.DataFrameCount b5b145b284f9ab143bddd4859fcb3a7122081a6de4d3e77643298e21df5a171d -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.DataType d74b4fb094966f305c4e01db49eee9fe254faabcdfbb05e2eb630505bb2b1525 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.Preview 77ddd93b500996a817f967b2844090f589742142cbc0caeff218d07a12e2c65a -->
<!-- api: P:Inno.Native.ImGui.ImGuiPayloadPtr.Delivery 9cd6fab3e17c83bfc630f9753d52861066092e43f8eb98592853d33b1527972a -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.Destroy cecf4412501ac3448afe7da87fa6e28162f95e3cb026d8b5115b2a5ebea47367 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.Clear 7c0eff3ab9e5745ace7eb3d3879515b8cfcea3b4963172a596ea0b4750b8242e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.IsDataType(System.String) b7f59260c10ed2b4029ef768c92498bcaf1be71a6d38dec75fd892a14016051c -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.IsPreview 74398b1cc8cdf9fef2ff3333cc5408bd877bff60c508165c5f228476c1749e57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPayloadPtr.IsDelivery 85ad8497ca959c918c3fa403429ab03acc6cad039329c1dd7d28ea2756f043fb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6432cf2dae4413af"></a>[`public Inno.Native.ImGui.ImGuiPayload* Inno.Native.ImGui.ImGuiPayloadPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15657) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4dfd9ecec6776da6"></a>[`public Inno.Native.ImGui.ImGuiPayloadPtr.ImGuiPayloadPtr(Inno.Native.ImGui.ImGuiPayload* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15652) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6303a23b0af74d3f"></a>[`public void Inno.Native.ImGui.ImGuiPayloadPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15691) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36d62ef38e2aa2e9"></a>[`public void Inno.Native.ImGui.ImGuiPayloadPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15683) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9a42a0b29760416"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.Equals(Inno.Native.ImGui.ImGuiPayloadPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15667) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-807b3419b0267745"></a>[`public override bool Inno.Native.ImGui.ImGuiPayloadPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15668) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-92d5acf4a1ca0404"></a>[`public override int Inno.Native.ImGui.ImGuiPayloadPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15669) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-bfb53a95ac1d823e"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.IsDataType(string type)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15699) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e1b2d016227111a"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.IsDelivery()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15715) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5418ed1dc412c4a7"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.IsPreview()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15707) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d81eb14ed10945d"></a>[`public static bool Inno.Native.ImGui.ImGuiPayloadPtr.operator ==(Inno.Native.ImGui.ImGuiPayloadPtr left, Inno.Native.ImGui.ImGuiPayloadPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db5d081535f48f23"></a>[`public static Inno.Native.ImGui.ImGuiPayloadPtr.implicit operator Inno.Native.ImGui.ImGuiPayloadPtr(Inno.Native.ImGui.ImGuiPayload* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15663) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-91afc5931ae48bdd"></a>[`public static Inno.Native.ImGui.ImGuiPayloadPtr.implicit operator Inno.Native.ImGui.ImGuiPayload*(Inno.Native.ImGui.ImGuiPayloadPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15664) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d0a599fe537151f"></a>[`public static bool Inno.Native.ImGui.ImGuiPayloadPtr.operator !=(Inno.Native.ImGui.ImGuiPayloadPtr left, Inno.Native.ImGui.ImGuiPayloadPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15666) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0568113e8e61617"></a>[`public void* Inno.Native.ImGui.ImGuiPayloadPtr.Data { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15671) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79d9ac9753413839"></a>[`public ref int Inno.Native.ImGui.ImGuiPayloadPtr.DataFrameCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15675) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1dfefb42ba110e20"></a>[`public ref int Inno.Native.ImGui.ImGuiPayloadPtr.DataSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15672) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2865f5939ce2476"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiPayloadPtr.DataType { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15676) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aa56a6200b0be7b0"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.Delivery { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15678) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef1522cb5c2f0bed"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15658) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9bec1069c21b153d"></a>[`public Inno.Native.ImGui.ImGuiPayload Inno.Native.ImGui.ImGuiPayloadPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15661) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74c548f582d1b627"></a>[`public static Inno.Native.ImGui.ImGuiPayloadPtr Inno.Native.ImGui.ImGuiPayloadPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ed2570a28bef0c2"></a>[`public bool Inno.Native.ImGui.ImGuiPayloadPtr.Preview { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15677) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-598423d9ec0b27f1"></a>[`public ref uint Inno.Native.ImGui.ImGuiPayloadPtr.SourceId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15673) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aea22f4ac466c67d"></a>[`public ref uint Inno.Native.ImGui.ImGuiPayloadPtr.SourceParentId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15674) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-86e403c251e61fa0"></a>[`public struct Inno.Native.ImGui.ImGuiPayloadPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPayloadPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15650) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformImeData</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformImeData 0def7b47c016615a678eaea8f5cd6ff720c7e38f32fdfdacd671e0e1253a3cbb -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.WantVisible e3fcfe65dcec2952e37260ae67f2ea906877fa7133b5af789fe0203f2fa9158a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.WantTextInput 282b951b0b7a7b666efa30a2d9e3a3c41fb03e7c4c086f3c79a4abaecc1a2475 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.InputPos 2fffcb1707bcc17c7b6e0e941b7b0af49035853aeaa618455eaadd6ae83ede49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.InputLineHeight 961e015ca9dc11dd1aa141e5023183b7db5afa12f782fef42af7b0286eb0abc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeData.ViewportId 0f8b2ba715fd92eb2b97c71d569a23b03e8d57a221c496520762cbbb6cde802d -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeData.#ctor(System.Byte,System.Byte,System.Numerics.Vector2,System.Single,System.UInt32) 1d91fd7fab1e594bde65adeed461a4e073a8fd57982aa81cca9d7fc1628b235b -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeData.Destroy 3cb54ed73feff9e2335b949147fcd9240797020c3e1e0923ffa2c194271e3a60 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0c8fb911f40ee0b3"></a>[`public float Inno.Native.ImGui.ImGuiPlatformImeData.InputLineHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-609335ca817b4e7a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformImeData.InputPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac258622471ef495"></a>[`public uint Inno.Native.ImGui.ImGuiPlatformImeData.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3351) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efdd65f4592d25db"></a>[`public byte Inno.Native.ImGui.ImGuiPlatformImeData.WantTextInput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64724d9eacd0fbee"></a>[`public byte Inno.Native.ImGui.ImGuiPlatformImeData.WantVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ded857203e66fa86"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData.ImGuiPlatformImeData(byte wantVisible = 0, byte wantTextInput = 0, System.Numerics.Vector2 inputPos = default(System.Numerics.Vector2), float inputLineHeight = 0, uint viewportId = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3352) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e1b0ca23d084e930"></a>[`public void Inno.Native.ImGui.ImGuiPlatformImeData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65930) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e0da01fc8028290"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformImeData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3345) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformImeDataPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformImeDataPtr 5130549b489603ba24b3fd9f35b2957aa978dfa313841ea7460f4f4dee45b987 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.#ctor(Inno.Native.ImGui.ImGuiPlatformImeData*) 851a9a9c14757523bedd055c642903d9c54f6f0706d4001dea22b45048259d60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Handle 539c7df45b37708577a0c1fe924ba9ea4df0d6adae8e102595b043fd73e43caf -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.IsNull 14fc77f2011d6bd64ee10987e33ed6840c277a4a2e186928e07b4e9addca2644 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Null b80512b67dd8ff913d68d7b1aab70f4052b97ccc173b2d5b3af0f8b57824b80f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Item(System.Int32) e61a36074552c31f405700d5378478accccb05dbbc285d60d0810c5ca63a3d6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformImeData*)~Inno.Native.ImGui.ImGuiPlatformImeDataPtr 55a76711df13b4eb6b1d320b8cc2fd875dc30c0885126e8212359967e190448f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformImeDataPtr)~Inno.Native.ImGui.ImGuiPlatformImeData* 32b22322c9ec2259e7a37ebf5f7cb7eb962d585c4aaed542ae582788f6612b5d -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.op_Equality(Inno.Native.ImGui.ImGuiPlatformImeDataPtr,Inno.Native.ImGui.ImGuiPlatformImeDataPtr) da2d085434bbbfd5465b04e39db95dc6cbb8d3b1243a7844048d0aabda60254b -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiPlatformImeDataPtr,Inno.Native.ImGui.ImGuiPlatformImeDataPtr) 3c6e3ebc0b93dc6f790bc34a5a3d8cf08467ae9d4c0d60929b855d04b40b2725 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Equals(Inno.Native.ImGui.ImGuiPlatformImeDataPtr) 9a95dece177c345f7efe917e7aff569398d62ecdc82768532a3b090e80b0c8f9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Equals(System.Object) e777bacae49e578528aca22b935bff170ab33f802132fb06fef4f114a16788a3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.GetHashCode f90b6510a8b56e50199f45d876ffe4f66f845418c2b2cbd5e44feb9d41504e7f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.WantVisible 1947acb8f677914d4724e0bb841fc89affe600e617434b97588bc068539d71cb -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.WantTextInput ed709f94c217f1a9c14b6079d3d356ca090b90a5c8b675a195331f20ce7193d8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.InputPos b4965c7687e0dd205af0956c0380c23113f928a330fa15f5ef28482f7f34e96d -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.InputLineHeight 23c4ff15a3413c560696c5543ccb1d25764ef7e8a515fac24c976e8bffaf943d -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.ViewportId 18954f7df46f8a80476705c679119ba69883eaba45a1688be63c20ca3e4df739 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Destroy fd2965f007f4fe692744443790cada34899f41491529414cbc977fe45f752f43 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-248109fd5927f736"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData* Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbd7ba119ac48253"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeDataPtr.ImGuiPlatformImeDataPtr(Inno.Native.ImGui.ImGuiPlatformImeData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15811) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-33a941a1fcdee4a8"></a>[`public void Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15839) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c564304c28260e1d"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Equals(Inno.Native.ImGui.ImGuiPlatformImeDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15826) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-168223994381d265"></a>[`public override bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15827) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2b9354778d5d2bcd"></a>[`public override int Inno.Native.ImGui.ImGuiPlatformImeDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15828) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-92dd93322bccc7fe"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.operator ==(Inno.Native.ImGui.ImGuiPlatformImeDataPtr left, Inno.Native.ImGui.ImGuiPlatformImeDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15824) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d59f014d97c36a4a"></a>[`public static Inno.Native.ImGui.ImGuiPlatformImeDataPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformImeDataPtr(Inno.Native.ImGui.ImGuiPlatformImeData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15822) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46564eea3cda771a"></a>[`public static Inno.Native.ImGui.ImGuiPlatformImeDataPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformImeData*(Inno.Native.ImGui.ImGuiPlatformImeDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1942e85f1633e00d"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.operator !=(Inno.Native.ImGui.ImGuiPlatformImeDataPtr left, Inno.Native.ImGui.ImGuiPlatformImeDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15825) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5069e2e96601d1aa"></a>[`public ref float Inno.Native.ImGui.ImGuiPlatformImeDataPtr.InputLineHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15833) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-77e1bfa3ddc32292"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformImeDataPtr.InputPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15832) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d164e6e34fa9156"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15817) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ded5c348d0c59a3"></a>[`public Inno.Native.ImGui.ImGuiPlatformImeData Inno.Native.ImGui.ImGuiPlatformImeDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15820) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7bc21e093959a25"></a>[`public static Inno.Native.ImGui.ImGuiPlatformImeDataPtr Inno.Native.ImGui.ImGuiPlatformImeDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15818) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-07513683b753925c"></a>[`public ref uint Inno.Native.ImGui.ImGuiPlatformImeDataPtr.ViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15834) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e2e7669a64e7154"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.WantTextInput { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15831) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2b493106d421f07"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformImeDataPtr.WantVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15830) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6f4f3c31e7f7023e"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformImeDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPlatformImeDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15809) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformIO</code> · 44 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformIO 3bf4798f07d19fe557f1ff6a7892103a62c8f354ace61995ba7e64cdb7205181 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetClipboardTextFn a44b5581230440000f2ab09754ef46e75b0829b0edbd529247347f2e9ebecf33 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetClipboardTextFn bc2c21d24f4d51275530c101a28f6ac986ec7cf6e915b217555de77b131ca6a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformClipboardUserData 5cc782d47005ec2eb28f2e12f10fdd6c5b4955c315ab9e78d141876dfbf593a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellFn d950ddae8fc3893ee04a85673f606cd7bd122493ff29018a60822cab89241862 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellUserData fc76bbd60c3b0e48bcfca407888b7187c81278c1f1834429e90b077fc1fcf47c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetImeDataFn 1c8425c4cec1f9922d73ec6bdf4ab9536db8a9a209e92b151a7671cb765d5244 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformImeUserData 1b3ebd583f296f6d4c034cdc302c6b3ff6a6ee6a3cc2ec9452d29b4f4fbf79a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformLocaleDecimalPoint aee27e1515543da7d030cfb83e39a7bdd6dace6c8fe85dbfbe589fdaa17b1133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxWidth 101e548771c63df11f0aeb680ec40e1d14375d34029eac1aa610a3c070918324 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxHeight 9ac7a6c2f0f8f9cf4bca3396820628412566933fd3ebcb195cc8a96d16e82686 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderState c01126fa7be28c5244e2cb29abfe92118383a7adcbe3d4ef6213e2468198a5b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateWindow 11363fd859fade4a610ba620790e3a97eb7502ea2590a28a81963a4075e16b3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformDestroyWindow aa1a9a2867327d8db5777a6a832fc6ab11cb59673e05c74ce7cb229ba7e74532 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformShowWindow 9537a405b45dfdae540d79e300e758f0de92acd5f5eeaf5a49c03eba748fd010 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowPos b22abcc284db8152573b6c9fdfcdfe674151ce837a3b594d0872502f8af59948 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowPos 17e3e583098f9eca205c61cb7c7b95b096d6ce439aa4db612622030142f5257a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowSize 878a7a0db2d3f091d76789049e0af661a1bf6c72f8669ccf069a4d385478419b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowSize 70fa0e35b25189160c5350c2b7ffe1a0fc906c32280e530b7616b4953395af8a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFramebufferScale ddbaf15f423433deb99ba5c2c49155671a9e8364e846fc23c0f67423d05666c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowFocus 1f7bd01662e782646e593a5be7df84d34a18c9d2b3a963743fa3ed67776d82a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFocus 093ef803c690f5bd495c655f1702a56bd79fa7b35efb4f7eef7f3eb45f1d9630 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowMinimized d4fc99cd214dbc41b5e182a882c0d01fb9e9b602e95fbac164f39ce4e8282add -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowTitle ae4a72c7e8cb021d4e1993fbc25ed1e79a20c119bd4e79033e93032db4eb9841 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowAlpha 4a8c97f9c6d0c09dc3430aec3ad99aa7ad705c8dd79716a6b7e72eecb6325e8c -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformUpdateWindow d1ee85e9bf075f5e1f10cdab07d26466ad52810c58e7d25abbd552aa4000440b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformRenderWindow 6116861cab394c1fc4e7ca19ca342212d19f135169da30b360d7a195744957f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformSwapBuffers 6c5aac3510f185995277e455a4db50758fdcb974d387bcb0da6693ccb9f03074 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowDpiScale 6615e8c45088f526363152c7eee9ebbd75d63555589e8a2324f0d9c9a5784788 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformOnChangedViewport a8758f3732df3d412b1058d63c2a2d3b8e86fa4416dd1f11f945207b86ac4f07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets d441bfc4ddda1d6a44c416bdcd6b50eb25832a5c287af9bc975149590c7438cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateVkSurface dfb3b240d01f393e962f590944a61d473c41c444e592243ecb19d84aec99dba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererCreateWindow dc2a91be9c6e14bcd3e74d1e48bb0004aa14d747734091050149f42b5f1b14ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererDestroyWindow 52fd684c0afd5e52c0e52de8f630a42c6943df70d95e589d02777aefa6dc45e2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererSetWindowSize eac105133a58320918c397de1faf0c741267329dadda32cde3954e1435c22d2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderWindow 230fb0a7c701f9aaaf96c5b72e40e15ff9f2f75b3d49954630713d1934123cfe -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.RendererSwapBuffers f3473cb2123cb9e1ebfe9e592c3812bdab1f4974e2e3dbdfafcd98ea9cf7d401 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Monitors 8bc938b758b5d4df1723d88853d824bf8d0ff954e5261452ff25f6028251ff46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Textures 67466682c0a2402199799308b921e6a82e8d9eaac2f77f191d4cf2b6d2773f64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIO.Viewports 9b4222d4f26d028c90d487c12df9e782a763a9b31c6d35d293d941dd42791655 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIO.#ctor(,,System.Void*,,System.Void*,,System.Void*,System.UInt16,System.Int32,System.Int32,System.Void*,,,,,,,,,,,,,,,,,,,,,,,,,,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiPlatformMonitor},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImTextureDataPtr},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiViewportPtr}) 1416c4b58bad2f3f7897a74824ab78010e99c702f707eaebaba692287327152c -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIO.Destroy 8b7498ea39c07296954dcc5524fc16a4557475e858897c7d4fb88e26e2204fce -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIO.ClearPlatformHandlers fc618f671c60b7839acc19b59a8141b5de8ba3757ad4b30a58df3097bb72a132 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIO.ClearRendererHandlers 4457a2e8ac9bda3e431f4614a15fd4ca7fac9bbfedae910dfe917b80d6a82316 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-96955d15f0a272d8"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPlatformMonitor> Inno.Native.ImGui.ImGuiPlatformIO.Monitors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3427) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee71c790d45cd8ff"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformClipboardUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-805091d7636148eb"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, ulong, void*, ulong*, int> Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateVkSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61f75f16e34fcf86"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformCreateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3402) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-325f51eea722551a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformDestroyWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3403) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20ccd86778e124b0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetClipboardTextFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2d39ed2c0670eb0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowDpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93b5e3f3f0af14a2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba72b4869220874d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowFramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5caae9eb3e259d2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowMinimized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3412) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8782b2548a1e494e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2b31ec036a7c675"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db3df6339467505a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector4> Inno.Native.ImGui.ImGuiPlatformIO.PlatformGetWindowWorkAreaInsets`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e135013da3ea036"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformImeUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-336c6cc4912d34e9"></a>[`public ushort Inno.Native.ImGui.ImGuiPlatformIO.PlatformLocaleDecimalPoint`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3073fc2205130e7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformOnChangedViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-584d4b06ac242817"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, byte> Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e8fd44be7c33e00"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.PlatformOpenInShellUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70ee0f4ec5ea1d2e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformRenderWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6412b2a72dfc248c"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetClipboardTextFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c481186421965641"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImGuiPlatformImeData*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetImeDataFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f945ce519f96a4a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bbe6892538fa1b9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowFocus`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3410) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab36016f5fb61b1d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3405) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d71b84d87806f77"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a7f727d21185e59"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSetWindowTitle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-877daabf88067fb0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformShowWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9192013451fe934f"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformSwapBuffers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f06c53f5cfab3a1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.PlatformUpdateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88986afbdb5c6b33"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererCreateWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3d6a7ab3cef76da"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererDestroyWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2adc0f574437d72"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3401) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4aefc9a7411a829"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererRenderWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3425) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d8e7e9cfc991d5d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererSetWindowSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6dc566227d66416b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIO.RendererSwapBuffers`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3426) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0efdc7d7a9acd66"></a>[`public int Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b372b13c355ec33"></a>[`public int Inno.Native.ImGui.ImGuiPlatformIO.RendererTextureMaxWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb81ef75a20848b0"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> Inno.Native.ImGui.ImGuiPlatformIO.Textures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3428) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21fbb3d268130f1a"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiViewportPtr> Inno.Native.ImGui.ImGuiPlatformIO.Viewports`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3429) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-788f1bb2677d8ead"></a>[`public Inno.Native.ImGui.ImGuiPlatformIO.ImGuiPlatformIO(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*> platform_GetClipboardTextFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, void> platform_SetClipboardTextFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, void>), void* platform_ClipboardUserData = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, byte> platform_OpenInShellFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, byte>), void* platform_OpenInShellUserData = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImGuiPlatformImeData*, void> platform_SetImeDataFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImGuiPlatformImeData*, void>), void* platform_ImeUserData = null, ushort platform_LocaleDecimalPoint = 0, int renderer_TextureMaxWidth = 0, int renderer_TextureMaxHeight = 0, void* renderer_RenderState = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_CreateWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_DestroyWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_ShowWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> platform_SetWindowPos = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> platform_GetWindowPos = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> platform_SetWindowSize = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> platform_GetWindowSize = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> platform_GetWindowFramebufferScale = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_SetWindowFocus = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> platform_GetWindowFocus = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> platform_GetWindowMinimized = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte*, void> platform_SetWindowTitle = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, void> platform_SetWindowAlpha = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_UpdateWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> platform_RenderWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> platform_SwapBuffers = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float> platform_GetWindowDpiScale = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> platform_OnChangedViewport = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector4> platform_GetWindowWorkAreaInsets = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector4>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, ulong, void*, ulong*, int> platform_CreateVkSurface = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, ulong, void*, ulong*, int>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> renderer_CreateWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> renderer_DestroyWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> renderer_SetWindowSize = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> renderer_RenderWindow = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> renderer_SwapBuffers = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPlatformMonitor> monitors = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPlatformMonitor>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> textures = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiViewportPtr> viewports = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiViewportPtr>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3430) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8d1d1c5747edf76e"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIO.ClearPlatformHandlers()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65891) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7dadce26c3b873fa"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIO.ClearRendererHandlers()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65902) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-625a5e2816b5984b"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIO.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4656b47761cd8e91"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformIO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3389) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformIOPtr</code> · 55 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformIOPtr 8dd1c23aaef69368c696c9cd9cd781a645f557c91725a7c95da4465c336fbf98 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.#ctor(Inno.Native.ImGui.ImGuiPlatformIO*) 07044b90d5141618f5bb21899f5b0f29c511f29838c318bb7d1b490fb928085e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformIOPtr.Handle bb25ee3e065404b14335a50187434c652a022c370f4dd41d8c0fdcf32695b21e -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.IsNull 1622d737082e50524f72ca6d42682924c026c91b1ca008a877381814dc857781 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.Null 3cd3eb58907c2c33e8a00e9ae704c7a7856c19eb5ad39dd2bf13252c4437411e -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.Item(System.Int32) 1ca48e23464a8cafc8e1b3a41326a88cb83a2334d035efdc39fe34b1d440eb3e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformIO*)~Inno.Native.ImGui.ImGuiPlatformIOPtr 810d4a622cc68cc7487827e0d933dc52a3c533e116cc01503d17e9d3ac40d91f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformIOPtr)~Inno.Native.ImGui.ImGuiPlatformIO* 6ba874d8d802dc1c19d2a18a3684430be7176a36563c85ca60619b820073d4ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.op_Equality(Inno.Native.ImGui.ImGuiPlatformIOPtr,Inno.Native.ImGui.ImGuiPlatformIOPtr) 9a4cec8c104bafae8f7a3a532f10552e98e93e5ef9390cd7b412803487a7560a -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.op_Inequality(Inno.Native.ImGui.ImGuiPlatformIOPtr,Inno.Native.ImGui.ImGuiPlatformIOPtr) 4a299b4a84a45cfc8d689d13c70a19979f82e0872d427ff3a73ea1c33afbfb8c -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.Equals(Inno.Native.ImGui.ImGuiPlatformIOPtr) a424ab8261c26e43a49d54cf0d37db1833cad2b8cd9299dfdf4ad2ba33b78d2e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.Equals(System.Object) 5aca1c36beafea2eae5d6cde63bfa3af3e286e7a4fecaf4d44deb5503af187bf -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.GetHashCode 319e4f268d6685c1f98cff0b28ada1183ff4430cc64e63554d2688d71016b8ac -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetClipboardTextFn 858d24e147e69404e96b4101e4caa67dec64e4934364477aeaee2ddf3ec8d70c -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetClipboardTextFn bcd883b6b3b44d1f36eed71e87a068b4e91dd5c0985f2fac9fea2b47dbab44f1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformClipboardUserData 92a6b0a320a7b464aabd4fb81df69b21eb41e5f412354d14bbfba93c4969b35e -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOpenInShellFn 3741144de358e9a01dc2564c141be76e1a242dc8991c2b2c310b74b7a66251f3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOpenInShellUserData b458bfd17a6d544dd3fd1a8735e8378534780d5089205898cd669929a9f9257c -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetImeDataFn bd7f8e73082ac7e6f6896b29b2e82921a3ecfa90dcc7ffb0ad8604c7cc748d4f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformImeUserData 50a4986e43cf3a8b41457d016d27c83eb920791dcf247733b524b2e92e85d203 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformLocaleDecimalPoint 10bc3e2d9470245a3bb381d2abde391f1a29a9de0d34e96890084290f61bdb75 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererTextureMaxWidth 9725d3984c42760ef71748e25eae02cd1b427769e269a162bf071dceb3535d8a -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererTextureMaxHeight 654f41deac60db90ffc83d09f82847e2d3136ccb49ba2078abc9de07cb3e55ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererRenderState 9a7d167b87069b33bec1808aed11b25964961cf35a9c6b58df3881a3b69f6fb4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformCreateWindow 9c7dbd35a56b24e8a81614713e85cff1f5ad3034a9ccde8a580fa9014e304bf3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformDestroyWindow d9701672c0a8568211e6bb57139160ae7c9dd38f59a1b6c5450c27907fa3456e -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformShowWindow 8ca576ebcdc3384d49f6e6a71016582f0e53cfa16d60cbf78b76332bb030ce23 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowPos be4d55160acf754f3921cd8bebdfec049a22b3935e6f8c0a9bb0d5469118993f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowPos 52e02dd19ebbe72459639b3af30144c22aaaf11ca0cec84f0b099cd02ab4b3fd -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowSize f1c5aef63ddab5b8e7e5d2e07507808b81ab5cda5ace83c24573432d0c19c379 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowSize c94530fec3fb3cdbd50e11fd4ae34824c8e804e16e6d4c04b1b28938841fe3c0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowFramebufferScale 70387d132ab29008b38716c9bdb0b5720b437ccb9d0efa83fc848093e93cf8f4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowFocus c37f7e6e3a5a10af9306bcabf452fd983a03ae3fcbab4bd435498bff53d5fe87 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowFocus 44c1a424a4059d8785a3790b97b0355c2f06b298a76c185963d448235205c166 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowMinimized dbe517fa15a11df120dc6b0e1996b2bd07aae84d6c98e47a990364c1405fb549 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowTitle 6f89c5c3b5f094848626afda92f77f68ea1ef9e3b0e9e0d194d7d81ed58326f3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowAlpha e16cb82b0f8cda75dc0ccccd7bb7e4e5680475a8e45580d3ee0e7360c4fa7a6a -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformUpdateWindow bc420ad7d82014d13dd37a579f146fede2cc3cdef2591e8d45cecdde05994512 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformRenderWindow 1c528ca873d3602aea23e16c93300febbbb53fa4e096d642dfceae5cb4866691 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSwapBuffers 5f14906dac227455a89e62a9ed8d1cf7429c5f1f6f69d78b25895e4d12ff5bbd -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowDpiScale 6ac6bace637d37c08e0c5c524a6d95a0b32be8c7d0699c1163859082ca4a9dc4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOnChangedViewport f053bc43a06f68815c19dc6347866bcadaab5ff7579fd28b9f5c764d508f670f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowWorkAreaInsets faa1045bf81b63d1f4dc9e552103d6c7150b620563d5be2993d547b4bc288216 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformCreateVkSurface 24774392e8921cfb18976d387d917252e093b697ea7659b561c0b39857238ddd -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererCreateWindow 6a5677c5f823f9e1db2ad043782c571d770f1e9793b1bab4516dca9c73bab533 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererDestroyWindow d0016e258389b5ee28e9d8736d43494ca41c9f7887402720284242166be81b8a -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererSetWindowSize 4704c3d739d2d9e7cab79ee48b647e4b44e4e7037dff3ef0e7b4686706a94eef -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererRenderWindow 1e4a0b175418a5718696774690a1a4531cc785805cfbf7408c9e20add011e90c -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererSwapBuffers d5571f3ff96affa0a8dec62b25178ac0cb1b3b8771341ea0bf4374b408e79836 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.Monitors a770d7d29528fb0ea6abe4cec9fbf2dd257523c442ebe8b0e63d686f55c40d68 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.Textures 6afd17fd26d98a327f526a4ff03a637df40613b7842af9dec803cdfb617c8246 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformIOPtr.Viewports 583e4885a7685362a39ad243e9ab784c7de401924c98ceb0157f232af93118dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.Destroy 29d1cc19bad5cb682c1631575b3b51b4c28570f8a762ce39cc993ac8ebfd08b6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.ClearPlatformHandlers 1d900d3552855ba5114382a30bb480267eaff599e0a88f9c59f789186de54c53 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformIOPtr.ClearRendererHandlers c8f3bbf000a8e0ee7125997247059d2e021930872e4eec72a2ea8a113515c6f6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3974821f4c742997"></a>[`public Inno.Native.ImGui.ImGuiPlatformIO* Inno.Native.ImGui.ImGuiPlatformIOPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0dbd61c1965c276"></a>[`public Inno.Native.ImGui.ImGuiPlatformIOPtr.ImGuiPlatformIOPtr(Inno.Native.ImGui.ImGuiPlatformIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15724) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-32353b8747d8c6bb"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIOPtr.ClearPlatformHandlers()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15794) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-40b44afb35e5141e"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIOPtr.ClearRendererHandlers()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15802) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8eda78b1ce7a1ffe"></a>[`public void Inno.Native.ImGui.ImGuiPlatformIOPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15786) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e6a80cd7163a864"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformIOPtr.Equals(Inno.Native.ImGui.ImGuiPlatformIOPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15739) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b0973460ea4fd2db"></a>[`public override bool Inno.Native.ImGui.ImGuiPlatformIOPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15740) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a990cdc570bb39c0"></a>[`public override int Inno.Native.ImGui.ImGuiPlatformIOPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15741) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-921161f9aa438d7e"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformIOPtr.operator ==(Inno.Native.ImGui.ImGuiPlatformIOPtr left, Inno.Native.ImGui.ImGuiPlatformIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15737) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b89d19d0da766228"></a>[`public static Inno.Native.ImGui.ImGuiPlatformIOPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformIOPtr(Inno.Native.ImGui.ImGuiPlatformIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15735) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6525679a8f563f9a"></a>[`public static Inno.Native.ImGui.ImGuiPlatformIOPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformIO*(Inno.Native.ImGui.ImGuiPlatformIOPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15736) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81474ce9174607da"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformIOPtr.operator !=(Inno.Native.ImGui.ImGuiPlatformIOPtr left, Inno.Native.ImGui.ImGuiPlatformIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15738) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1f239cf4ea0d6426"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformIOPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15730) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c6c460593f4d7d3c"></a>[`public Inno.Native.ImGui.ImGuiPlatformIO Inno.Native.ImGui.ImGuiPlatformIOPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15733) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a4caf8c19bc9355"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiPlatformMonitor> Inno.Native.ImGui.ImGuiPlatformIOPtr.Monitors { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15779) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c9d9b237d155f4c"></a>[`public static Inno.Native.ImGui.ImGuiPlatformIOPtr Inno.Native.ImGui.ImGuiPlatformIOPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15731) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-058cf4a18d7606e9"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformClipboardUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15745) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-627311e28db71fe5"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, ulong, void*, ulong*, int> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformCreateVkSurface { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15773) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-85b4e489160ff795"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformCreateWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15754) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3ea57e901e8285d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformDestroyWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15755) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b0ff773be6b884f"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetClipboardTextFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15743) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-455e276386ba0cb7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowDpiScale { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15770) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d78dfbc206005233"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowFocus { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15763) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-00dcd1a9b37b446d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowFramebufferScale { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15761) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c69f20c9c2198e22"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowMinimized { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15764) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-adf0a8a7a501aadd"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowPos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15758) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b2dac45593f1a22"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowSize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15760) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a83247bb1961ba43"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector4> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformGetWindowWorkAreaInsets { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15772) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5abef616e8a9c02"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformImeUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15749) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48cd1218a8c533b2"></a>[`public ref ushort Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformLocaleDecimalPoint { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15750) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4f0cca7479dec94d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOnChangedViewport { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15771) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aa09231b5a5e41f7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, byte> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOpenInShellFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15746) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-53fb29825042235d"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformOpenInShellUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15747) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-caeefc504ed38758"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformRenderWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15768) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7325e865efb1f61"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, byte*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetClipboardTextFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15744) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e3fd682702cfbcd"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiViewport*, Inno.Native.ImGui.ImGuiPlatformImeData*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetImeDataFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15748) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4923075d9e3bf436"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowAlpha { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-21ca0406174544d8"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowFocus { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15762) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2687bc08e4c4dbd7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowPos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15757) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80d25006aeea9d01"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowSize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15759) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9dfa11ce94c6830e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, byte*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSetWindowTitle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15765) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-778b9b03a9fa23df"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformShowWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15756) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f4ad2623abb7779b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformSwapBuffers { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15769) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-821645eb59a86f97"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.PlatformUpdateWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15767) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6334591a8419a795"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererCreateWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15774) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-339525a1cb05851e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererDestroyWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15775) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d04fbf3242b67ec7"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererRenderState { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15753) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8996f612ef5367b4"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererRenderWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15777) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4690cfd3fdade4e4"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, System.Numerics.Vector2, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererSetWindowSize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15776) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-17f01529f773121d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, void*, void> Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererSwapBuffers { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15778) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65ac71abc2f16ab5"></a>[`public ref int Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererTextureMaxHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15752) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad94f88d6501b0ab"></a>[`public ref int Inno.Native.ImGui.ImGuiPlatformIOPtr.RendererTextureMaxWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15751) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b6d4feccc2a4b11"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> Inno.Native.ImGui.ImGuiPlatformIOPtr.Textures { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1bc58a3a4d81b9e6"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiViewportPtr> Inno.Native.ImGui.ImGuiPlatformIOPtr.Viewports { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15781) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a59585d7737fa73"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformIOPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPlatformIOPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15722) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformMonitor</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformMonitor d0f14efc369bf3da53db52144a951100027df483b489bbce354b024080544b6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.MainPos f91576c81e91877bccbb60cadb97949288935774f2565b59b62d0d46e46e7be7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.MainSize cd724a5b2d3645daf66157b31deaecd228509a10ed6f9b008b8bf6bd05754f83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.WorkPos 3e8512eea7cda634e3f0b27362e2087dc052f6b25111243efed4a6294333bc12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.WorkSize 271425e29ea847019da4f50eb4868f0d200bf32aff72a402ca26df1357f6ace3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.DpiScale c07c5bcebca138068c9e9b8ddc54f0f571f4d4d20ddee991e6a704caa70a784b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitor.PlatformHandle 9e5a71b7c4771020851383e2192d6d53aaa00b3fd09adafda04707fa9057f34f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitor.#ctor(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Void*) 95dfbcb848ac47d1e43f62afc5f47f8168c34ff07c145e4064cd5ba929312490 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitor.Destroy 420b614ba68264fb3c6155ea8450ab60a99fffc94daf3ff7843c812833c49f0c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-af7b5292380d2faa"></a>[`public float Inno.Native.ImGui.ImGuiPlatformMonitor.DpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d5bf1084fa7a13c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitor.MainPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3368) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-320f7b1c88bec785"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitor.MainSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3369) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d5f9acff29a4058"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformMonitor.PlatformHandle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3373) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19e4702887672070"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitor.WorkPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3370) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6389336ede07c4fc"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitor.WorkSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3371) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86822c15eb49e97b"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitor.ImGuiPlatformMonitor(System.Numerics.Vector2 mainPos = default(System.Numerics.Vector2), System.Numerics.Vector2 mainSize = default(System.Numerics.Vector2), System.Numerics.Vector2 workPos = default(System.Numerics.Vector2), System.Numerics.Vector2 workSize = default(System.Numerics.Vector2), float dpiScale = 0, void* platformHandle = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3374) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-faa535948c4170b4"></a>[`public void Inno.Native.ImGui.ImGuiPlatformMonitor.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65916) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e290fb602b7c0b7b"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformMonitor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3366) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlatformMonitorPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlatformMonitorPtr 45c7396adca34dce5f9b67d3ef70e640ac19e56b28ca7c53b34b80027151a652 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.#ctor(Inno.Native.ImGui.ImGuiPlatformMonitor*) 47efae6dcbf234c450840f1977cb2d33d016ade85a0df4af670b8c0bb31d67b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Handle 85e94cd3f6d1f97d494dc69816fb01b218d31dd8b83ff39b604fb17f897e0724 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.IsNull ca56cb845b0ed424338b09bc83eb3018cdeb29770c221b4187455641a8b0a5d6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Null 064475a24747a4f62a5e684ea39518cd54b857f5d844295ed0c7d5874f125457 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Item(System.Int32) e5d0e20a033c4723e1b8b00e4929ad2851d4d41acfdd5c0adcd259ff43c0eed0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformMonitor*)~Inno.Native.ImGui.ImGuiPlatformMonitorPtr 9b1ef74e00c13c683b83743a217356af15381e82b7f3454b086a784079dbed33 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.op_Implicit(Inno.Native.ImGui.ImGuiPlatformMonitorPtr)~Inno.Native.ImGui.ImGuiPlatformMonitor* c0179cf2b14e8f130bc23a4bad11e963e5e9bed7294acf5439bbfb10117cd82a -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.op_Equality(Inno.Native.ImGui.ImGuiPlatformMonitorPtr,Inno.Native.ImGui.ImGuiPlatformMonitorPtr) 9afb78bfb445157ef14aaa1fc6bb09306319c001267f7386d1d11aa455b51ed9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.op_Inequality(Inno.Native.ImGui.ImGuiPlatformMonitorPtr,Inno.Native.ImGui.ImGuiPlatformMonitorPtr) 441936cd82405fc79645b24214765484e2f6249d497dc8e2df2c1c89a956fe6f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Equals(Inno.Native.ImGui.ImGuiPlatformMonitorPtr) 6b4bc739f241147e2886789c499cc2d8b9a1f1aacde6857f46eddc80f8056ec1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Equals(System.Object) 487d288bf1262e840fb42048da911ab5ec32ee008c4aa7e88e9bf3daf00d8d27 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.GetHashCode b490ec76284783963228a31a1ebddf933b104e770309922a7ac5e3897e338d92 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.MainPos e92ba9ec1d93d3899c7834579c2d623abf8126b8acd68443c60b96c3c6d448a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.MainSize f3c4ccbb77d5487083912c088558814cb8010b1f17031bc75dff6531663eccb6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.WorkPos 04c11eb31ac0f3024be0a66d6d1afb524e16d09b5cf0b090fd547a597655495c -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.WorkSize 90a9b191f0236e6c987ff38661b9c06f0b6356e77eef3db277ea24c70fdfcdce -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.DpiScale 3594d93c6a64ce6a0b7e41ec5971944bf662316334b51a0dba6635c47d79ba19 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.PlatformHandle ef7319e314925c77f331bfd533c0db0fdde1dd313ee40b7c0fc5ec8dea7afc2c -->
<!-- api: M:Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Destroy 99d4b26fb391207bb8b48b274fba70bff50bdd4fe57aa56a762cc0c4e727afe7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-772424f0e3fc5c43"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitor* Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-058b9c601648b087"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitorPtr.ImGuiPlatformMonitorPtr(Inno.Native.ImGui.ImGuiPlatformMonitor* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15848) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-820f5a823b99dfd6"></a>[`public void Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15877) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-738807f71a16633d"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Equals(Inno.Native.ImGui.ImGuiPlatformMonitorPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15863) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-789730d8fd1be63d"></a>[`public override bool Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15864) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9d6d01cde0f19a31"></a>[`public override int Inno.Native.ImGui.ImGuiPlatformMonitorPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15865) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c5cc6ff96e1d8ae8"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformMonitorPtr.operator ==(Inno.Native.ImGui.ImGuiPlatformMonitorPtr left, Inno.Native.ImGui.ImGuiPlatformMonitorPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15861) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f96d4909ea686b8d"></a>[`public static Inno.Native.ImGui.ImGuiPlatformMonitorPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformMonitorPtr(Inno.Native.ImGui.ImGuiPlatformMonitor* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15859) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9dba299f0aae3fd1"></a>[`public static Inno.Native.ImGui.ImGuiPlatformMonitorPtr.implicit operator Inno.Native.ImGui.ImGuiPlatformMonitor*(Inno.Native.ImGui.ImGuiPlatformMonitorPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-458c8b2ccd7dc8d1"></a>[`public static bool Inno.Native.ImGui.ImGuiPlatformMonitorPtr.operator !=(Inno.Native.ImGui.ImGuiPlatformMonitorPtr left, Inno.Native.ImGui.ImGuiPlatformMonitorPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15862) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6543fa5d3090211b"></a>[`public ref float Inno.Native.ImGui.ImGuiPlatformMonitorPtr.DpiScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15871) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e85ab0eb8e523414"></a>[`public bool Inno.Native.ImGui.ImGuiPlatformMonitorPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15854) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-70e31e187c4a5849"></a>[`public Inno.Native.ImGui.ImGuiPlatformMonitor Inno.Native.ImGui.ImGuiPlatformMonitorPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15857) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc3d1656e974a1e8"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitorPtr.MainPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15867) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-381617a64cc1e670"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitorPtr.MainSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15868) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28fd058dc97da013"></a>[`public static Inno.Native.ImGui.ImGuiPlatformMonitorPtr Inno.Native.ImGui.ImGuiPlatformMonitorPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15855) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9dbfe6bf3e11179"></a>[`public void* Inno.Native.ImGui.ImGuiPlatformMonitorPtr.PlatformHandle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15872) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7fa3047b3eb06259"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitorPtr.WorkPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15869) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9c5dac6e2a8692e"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPlatformMonitorPtr.WorkSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15870) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-37e77df980b3fded"></a>[`public struct Inno.Native.ImGui.ImGuiPlatformMonitorPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPlatformMonitorPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15846) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPlotType</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPlotType 74f08a7a458f7c030954ac33518c8958027b8cda3f2e9f1c7ae4d8fd3ed9c2a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlotType.Lines b5cc34fb790d731cd01fe0c7776729818dd21be216a106f1dad8460858e66410 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPlotType.Histogram 11befc89dcbc5b936ab604d6951f0ce33bf9685b8718083d914403cf88f4bc53 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e1cba763aa75ebe0"></a>[`Inno.Native.ImGui.ImGuiPlotType.Histogram = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1410) | 当前常量或枚举值：`1`。 | — |
| <a id="api-efd05cc1c4a1e66c"></a>[`Inno.Native.ImGui.ImGuiPlotType.Lines = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1409) | 当前常量或枚举值：`0`。 | — |
| <a id="api-2ad272c09cd6f01b"></a>[`public enum Inno.Native.ImGui.ImGuiPlotType : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1407) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPopupData</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPopupData 11b751be54f7cd74582d20560d10cff629b6bdd958f6a0b9314e4652f31d9f26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.PopupId f5393e613842c400a6506f2436fa38e43c5e97b24d5871157a0ac0b9d5e9ec2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.Window 839a5593cb090d32198805d237dfc940ec64e573efdcd86c7b57053bb4822779 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.RestoreNavWindow fcfe0860c568f1a54e29c29a42841111cfecbac732f647ae6f9b9d5eabffd6a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.ParentNavLayer 249f403ff96c459512016708988faad1c0926a98d80ec2cc42155c20550dbed7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenFrameCount 6e1855ad2b8b9cabb335cc42dcd72db30305ee739e70049256a7df21649e160f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenParentId a34d99d416bf6eae1e0b7e5cbec087da4de7cc4baa882180e2e43d9260e5d6f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenPopupPos 945ac2c127cdef0867f59f18d1ae86b75ca6267c0a8c80a930d5d6ee1e53850d -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupData.OpenMousePos b2349688539d9b48d9b03fb3dc0619a972904e876fd89e3551f36f961125264e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupData.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,System.Int32,System.Int32,System.UInt32,System.Numerics.Vector2,System.Numerics.Vector2) 196fa3b8b2717ba68b463c53f3427b8e2d9665a25e1fc96f40eea3c259301fc3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupData.Destroy 743923ae141f1f68c08a3857bfa06e15596d288749778f7bceaf0330c4bc9ea3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4e93b9539dfafb88"></a>[`public int Inno.Native.ImGui.ImGuiPopupData.OpenFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5799) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5fd2348ce8bf23b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPopupData.OpenMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05c4f4dd138f10a3"></a>[`public uint Inno.Native.ImGui.ImGuiPopupData.OpenParentId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5800) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0429df8ec6952760"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPopupData.OpenPopupPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5801) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9727ded753062dc2"></a>[`public int Inno.Native.ImGui.ImGuiPopupData.ParentNavLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5798) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6d053aa556e6a03"></a>[`public uint Inno.Native.ImGui.ImGuiPopupData.PopupId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5795) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac6c79c4cedff243"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiPopupData.RestoreNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5797) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-652237859d91c0c5"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiPopupData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5796) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e2af5df4a9f4202"></a>[`public Inno.Native.ImGui.ImGuiPopupData.ImGuiPopupData(uint popupId = 0, Inno.Native.ImGui.ImGuiWindow* window = null, Inno.Native.ImGui.ImGuiWindow* restoreNavWindow = null, int parentNavLayer = 0, int openFrameCount = 0, uint openParentId = 0, System.Numerics.Vector2 openPopupPos = default(System.Numerics.Vector2), System.Numerics.Vector2 openMousePos = default(System.Numerics.Vector2))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5803) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ebed626143420cd8"></a>[`public void Inno.Native.ImGui.ImGuiPopupData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38416e7579c58988"></a>[`public struct Inno.Native.ImGui.ImGuiPopupData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5793) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPopupDataPtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPopupDataPtr 9491edade808b9d4fd174a7ffd42ba235fd7c2de1176f5ba2140659c7c849f0e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.#ctor(Inno.Native.ImGui.ImGuiPopupData*) a24db34eee04242fe56ce02441b227547f8cc859239f2b4db61908b43346e878 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupDataPtr.Handle bb9239aaedc365c7268b1ce01c590b861c0832d2aa2ba60c08ac45abf00e05c8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.IsNull 3609a7e47efb68b4038a69ac94e74515cb8fbe3d572bd788943d21ec20781be2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.Null 1446ddee507599fe2bffa6e5fcc2a47dd06094877e76f5657ff69a672a46fe69 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.Item(System.Int32) eff5914e2e3849901f1ca4299b45255152b7eedf2af7bb651f3b6ed9e5372ae3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiPopupData*)~Inno.Native.ImGui.ImGuiPopupDataPtr f15e18aabdfc1a4329e67ae1f5d6aba8e051bd7f7c1cc37a0869396715b50e12 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiPopupDataPtr)~Inno.Native.ImGui.ImGuiPopupData* 8265f904c2f00c6957b15c017e4e6a99ce7cdb953343a874841953bcfcaffd16 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.op_Equality(Inno.Native.ImGui.ImGuiPopupDataPtr,Inno.Native.ImGui.ImGuiPopupDataPtr) c44351b79026576d0240df3d56c073a581cfc4f134de79b9612cbf9ce26e8e6e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiPopupDataPtr,Inno.Native.ImGui.ImGuiPopupDataPtr) afdc5f01af9175e05eaab0bee07413da852cd078b0422efa0db3457aacfe2db6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.Equals(Inno.Native.ImGui.ImGuiPopupDataPtr) d6b6911b056faba9c241477707eb0e9d2fbed0d30cf79d14ceb99218589360ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.Equals(System.Object) 42e9176b1e2299f24c08a8644874adf341111aa5a059865fd4f99fd35ca1234d -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.GetHashCode 16b0d749755d8a4827d1555bfa4227d34eeb798e54464e12bdfec19b70e70dc1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.PopupId e5b2b2c0e790d8b3d59490f2304241accb247049b069570959b0fd40c4674c32 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.Window 75652e792695a0e3974bd1ce422570c7ca9af7e0cd9ed5c7dbaeaa283979b1ea -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.RestoreNavWindow f758cb581c4900f8cdc3448ae8b996df1be32ce016bf6eef69c449fde648b314 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.ParentNavLayer 51a659eb1cc3176d65e923293dec932b523082db42226e1476579ff474b8502d -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.OpenFrameCount 13c7c5731fa1e19985128d454a62b436bec900e874c116e6658bacdc376381ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.OpenParentId a5876375ca2dab4c2a65fbe45b863c1fde6e6795c194bbf8744518b780e8fed0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.OpenPopupPos ac119fb11a013c8adf583e382295dd12fb0b5a8e1a063810e9f6bdffac4a4ee5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPopupDataPtr.OpenMousePos 2512771e80080a2cc5b1596cf4d80f1737633504b1e55ed192422a1ff4697614 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPopupDataPtr.Destroy 0c7afe96c9da680cdad22b671a04f603143984ba064d367a29a8ac2001313af8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-26cd7ee171d2b9d0"></a>[`public Inno.Native.ImGui.ImGuiPopupData* Inno.Native.ImGui.ImGuiPopupDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80a36f172c9efc8f"></a>[`public Inno.Native.ImGui.ImGuiPopupDataPtr.ImGuiPopupDataPtr(Inno.Native.ImGui.ImGuiPopupData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15886) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ea06d4096573559a"></a>[`public void Inno.Native.ImGui.ImGuiPopupDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15917) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1ad90bff54710bb1"></a>[`public bool Inno.Native.ImGui.ImGuiPopupDataPtr.Equals(Inno.Native.ImGui.ImGuiPopupDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15901) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8f2fd85a7d21a884"></a>[`public override bool Inno.Native.ImGui.ImGuiPopupDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15902) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bd0093cdcc3f9533"></a>[`public override int Inno.Native.ImGui.ImGuiPopupDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15903) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6ca078d098447530"></a>[`public static bool Inno.Native.ImGui.ImGuiPopupDataPtr.operator ==(Inno.Native.ImGui.ImGuiPopupDataPtr left, Inno.Native.ImGui.ImGuiPopupDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15899) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c2f929042db8182e"></a>[`public static Inno.Native.ImGui.ImGuiPopupDataPtr.implicit operator Inno.Native.ImGui.ImGuiPopupDataPtr(Inno.Native.ImGui.ImGuiPopupData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15897) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dced81e5d564067c"></a>[`public static Inno.Native.ImGui.ImGuiPopupDataPtr.implicit operator Inno.Native.ImGui.ImGuiPopupData*(Inno.Native.ImGui.ImGuiPopupDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15898) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-99e8b176de532492"></a>[`public static bool Inno.Native.ImGui.ImGuiPopupDataPtr.operator !=(Inno.Native.ImGui.ImGuiPopupDataPtr left, Inno.Native.ImGui.ImGuiPopupDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15900) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-982cf6ebcf189b86"></a>[`public bool Inno.Native.ImGui.ImGuiPopupDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15892) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39cbcbcdf643c6b9"></a>[`public Inno.Native.ImGui.ImGuiPopupData Inno.Native.ImGui.ImGuiPopupDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15895) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04b0275ff86e3b2a"></a>[`public static Inno.Native.ImGui.ImGuiPopupDataPtr Inno.Native.ImGui.ImGuiPopupDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15893) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-59030e42c07cf713"></a>[`public ref int Inno.Native.ImGui.ImGuiPopupDataPtr.OpenFrameCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15909) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-897fb180da7f5bc9"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPopupDataPtr.OpenMousePos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15912) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-85078fb9eb2523d3"></a>[`public ref uint Inno.Native.ImGui.ImGuiPopupDataPtr.OpenParentId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15910) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-82dc33cc6928102a"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiPopupDataPtr.OpenPopupPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15911) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-555a8c7b21701dea"></a>[`public ref int Inno.Native.ImGui.ImGuiPopupDataPtr.ParentNavLayer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15908) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a69c907e4cd213e9"></a>[`public ref uint Inno.Native.ImGui.ImGuiPopupDataPtr.PopupId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15905) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6026773ac6046595"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiPopupDataPtr.RestoreNavWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15907) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f152e79380e7a02"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiPopupDataPtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15906) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b188742e11b421cf"></a>[`public struct Inno.Native.ImGui.ImGuiPopupDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPopupDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15884) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPopupFlags</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPopupFlags 7d1d517d037eb5aced5dcf35e836782b4a34c994020905f1adf612c5b9897d2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.None b7822f6fd7d2817eebb4f6fe1536d017c96a2519f1c5687f307a9f4c475bb434 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonLeft 11af9334baa6990a7f2531bc8d0f29b54db72428c319592682d9a11c8a21d849 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonRight 3a383118b2d4c03b3d6f5f6032c1cdabccef8392dd0e1ae484c7ff014795d13b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMiddle 0826c27d3b744b1b96463a12e94a6463821ff065da716cda8b75ebdefb696909 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoReopen 97e80c8016f461ca0d9a8a6322776f46d17716c4fd70dcec98dcf9f48324399e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverExistingPopup 3b4791627e5081911b4654699a2255f95620f0d66444692a9cd5412f75a0b1cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverItems e0f568879ee191dfe5cc4820fa31fdcf47140fa94731e29873de247e9473e328 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupId 339615e303e7c431d6e649ad483b32c5adce76ec0c87aa5c5af9671a71f0dd38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupLevel 220454cb706e49d5f02464407aa9dfea5e6c18e7257106f526077e81f2ed223e -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.AnyPopup ba7f0731b32d0256605c6418f71ba474dd5582ff3b44aa7ef2b27aaed3389672 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonShift b6a84b629a0cd27e5a64159d814e808a5316d690f8b699f4c146ecb79560123b -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMask 5c09cbb6fc522810f5f27d71bbb9fbd3ad635e35cf4253041c0646e12430d968 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupFlags.InvalidMask 9b476c11f46ed22e6ff3b4cd9516aabdb0b755c40a4b407ed7fb794211df56e3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5367af1064a8b661"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopup = 3072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L521) | 当前常量或枚举值：`3072`。 | — |
| <a id="api-eeeedc96254379db"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupId = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L519) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-73bfce64a27940ee"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupLevel = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L520) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-8081ad3075c128e1"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.InvalidMask = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L524) | 当前常量或枚举值：`3`。 | — |
| <a id="api-a86545df09d14ddd"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonLeft = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L513) | 当前常量或枚举值：`4`。 | — |
| <a id="api-9f2a6b1d5bda1514"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMask = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L523) | 当前常量或枚举值：`12`。 | — |
| <a id="api-f48090d52543c1a5"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMiddle = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L515) | 当前常量或枚举值：`12`。 | — |
| <a id="api-47ed8ad03a8e4935"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonRight = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L514) | 当前常量或枚举值：`8`。 | — |
| <a id="api-9d5f2a29806dae07"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonShift = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L522) | 当前常量或枚举值：`2`。 | — |
| <a id="api-c0ead90b54319a95"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L512) | 当前常量或枚举值：`0`。 | — |
| <a id="api-281b22106c2b2c31"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverExistingPopup = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L517) | 当前常量或枚举值：`128`。 | — |
| <a id="api-762d933922afe410"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverItems = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L518) | 当前常量或枚举值：`256`。 | — |
| <a id="api-a88e558e786deea4"></a>[`Inno.Native.ImGui.ImGuiPopupFlags.NoReopen = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L516) | 当前常量或枚举值：`32`。 | — |
| <a id="api-4d7658eb5f2010c2"></a>[`public enum Inno.Native.ImGui.ImGuiPopupFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L510) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPopupPositionPolicy</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPopupPositionPolicy 65ad0e47870d204ca576b7dc0a1aa842af5ea4268dd9619145d80ae5fd453ff3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.Default 36017846af42975c5dfdae76824bb9072358d94860cc4b9da726bb88433363af -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.ComboBox 4728980d186fd63a697ac482f105290f81cd4eed1057b24cc9c248ab17510452 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPopupPositionPolicy.Tooltip 0dc35f30b85355f468d6a822ba6cc09b317b589448705a097e385cb76c858e02 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-01de0ef6f53f6ba0"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.ComboBox = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1475) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d0b46f47ba0e61d8"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.Default = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1474) | 当前常量或枚举值：`0`。 | — |
| <a id="api-16bc1bc2378ef2d9"></a>[`Inno.Native.ImGui.ImGuiPopupPositionPolicy.Tooltip = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1476) | 当前常量或枚举值：`2`。 | — |
| <a id="api-942b642cf33a01ab"></a>[`public enum Inno.Native.ImGui.ImGuiPopupPositionPolicy : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1472) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPtrOrIndex</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPtrOrIndex 526dc22533fa8d211a7680e2f318e522748f21b5f4bccf499cad4b2c710a466a -->
<!-- api: F:Inno.Native.ImGui.ImGuiPtrOrIndex.Ptr 2430dfd3205444c3895b3aa4af233adbe83f8d81260a1d9b7d0e7df2846c75b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiPtrOrIndex.Index a6e44e8aef7d5d76a940219d76aa91b668e28e0055cd0b24c88e5fa87656e7cf -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndex.#ctor(System.Void*,System.Int32) 634ef4ac9c367d3640168c4450c428cecf25cc94008cb8dec178d59c5338772f -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndex.Destroy 44f63e9e994c12902bc3844f50d327a68e5f2fe308d768484645d45e054b87d8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5fc4750a47ae3d79"></a>[`public int Inno.Native.ImGui.ImGuiPtrOrIndex.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bf3212153f4685f"></a>[`public void* Inno.Native.ImGui.ImGuiPtrOrIndex.Ptr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b4504276193d665"></a>[`public Inno.Native.ImGui.ImGuiPtrOrIndex.ImGuiPtrOrIndex(void* ptr = null, int index = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6766) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a31d36fbb2341822"></a>[`public void Inno.Native.ImGui.ImGuiPtrOrIndex.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66809) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6e66a762e70aca3"></a>[`public struct Inno.Native.ImGui.ImGuiPtrOrIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6762) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiPtrOrIndexPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiPtrOrIndexPtr cc4837eacdd15a0f1c8dc34a5d10412cedb3432f83b37df75039d45bb6fcdd7e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.#ctor(Inno.Native.ImGui.ImGuiPtrOrIndex*) c145a80ebb0af6c3cb5ed5c5db5d3d825e973c3c2864891c8ab0de4750800cdc -->
<!-- api: F:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Handle f2ebb57f9620fa6633c0c562a4bc18d44601da723660ad7328521afb457c7a14 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.IsNull dd0c0e296947740d9f1e9263e2a097d624b0fa747438ab19915a436a369cca51 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Null a277fb2ef14dcc0399761f53222ad01c0ca0f43ba3748bed6b69ee11c60a1f6f -->
<!-- api: P:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Item(System.Int32) 5a3c3f97cb238f133f7808ffef08e19c1e283044d7858c200448400163230be6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.op_Implicit(Inno.Native.ImGui.ImGuiPtrOrIndex*)~Inno.Native.ImGui.ImGuiPtrOrIndexPtr 31952f4da36f3189a47e5747ccadcc680bdb3f2c2db3e54b3c57d811c7a1cc9c -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.op_Implicit(Inno.Native.ImGui.ImGuiPtrOrIndexPtr)~Inno.Native.ImGui.ImGuiPtrOrIndex* 7bbcad0c6323c600af76860fda1fea20c350b28521abacc04be06f873ceb35a4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.op_Equality(Inno.Native.ImGui.ImGuiPtrOrIndexPtr,Inno.Native.ImGui.ImGuiPtrOrIndexPtr) 7d1d7e760df50fec0effe0b39ca41bf47d305440e990689b2a37df333c43e0e6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.op_Inequality(Inno.Native.ImGui.ImGuiPtrOrIndexPtr,Inno.Native.ImGui.ImGuiPtrOrIndexPtr) f7a386dbe8051c7bd3a76c2cc890997838a899a0fc0888ea4a311b802428e0ab -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Equals(Inno.Native.ImGui.ImGuiPtrOrIndexPtr) 0557801238f7ad6d3ccc93076f8255e0bbbc01f3e80b4241b6a439f12163800e -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Equals(System.Object) 928b4fbe9d26a1603e9bbf73939f711b232a00f00b3516de9ff6144310084987 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.GetHashCode e1d8a3f90536a811f5fbcd20cac235e727c943e986fa152e86048f4aa3443a1d -->
<!-- api: P:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Ptr 56ed65411f005c13249fc0ba0b76b2a14acd8e7e7d8df5309315af1215604f69 -->
<!-- api: P:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Index b4a23d5d4f022f8c5132c73346da14cc0ca709167d54d2518c18183fd8b109b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Destroy 12cc846dc298214ec4fa84052f4246fd17bdd0d08776405814e092cde7d00ab5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eb1a20ec15bd2880"></a>[`public Inno.Native.ImGui.ImGuiPtrOrIndex* Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2ae215c7272374c"></a>[`public Inno.Native.ImGui.ImGuiPtrOrIndexPtr.ImGuiPtrOrIndexPtr(Inno.Native.ImGui.ImGuiPtrOrIndex* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15926) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e94b12f8e17fd00c"></a>[`public void Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15951) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2016c1ac1392fa59"></a>[`public bool Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Equals(Inno.Native.ImGui.ImGuiPtrOrIndexPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15941) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-61ab4464f0fa9243"></a>[`public override bool Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15942) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1b6cec03a9879d94"></a>[`public override int Inno.Native.ImGui.ImGuiPtrOrIndexPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15943) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-701ca04b15c2a3bf"></a>[`public static bool Inno.Native.ImGui.ImGuiPtrOrIndexPtr.operator ==(Inno.Native.ImGui.ImGuiPtrOrIndexPtr left, Inno.Native.ImGui.ImGuiPtrOrIndexPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15939) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b949fe805d903fa"></a>[`public static Inno.Native.ImGui.ImGuiPtrOrIndexPtr.implicit operator Inno.Native.ImGui.ImGuiPtrOrIndexPtr(Inno.Native.ImGui.ImGuiPtrOrIndex* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dfd444bd1d7b3c1e"></a>[`public static Inno.Native.ImGui.ImGuiPtrOrIndexPtr.implicit operator Inno.Native.ImGui.ImGuiPtrOrIndex*(Inno.Native.ImGui.ImGuiPtrOrIndexPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15938) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-52447639ed3ffe7b"></a>[`public static bool Inno.Native.ImGui.ImGuiPtrOrIndexPtr.operator !=(Inno.Native.ImGui.ImGuiPtrOrIndexPtr left, Inno.Native.ImGui.ImGuiPtrOrIndexPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67efa4887caad789"></a>[`public ref int Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Index { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8cb49a02efefc7ec"></a>[`public bool Inno.Native.ImGui.ImGuiPtrOrIndexPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15932) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c341446adb32cfe4"></a>[`public Inno.Native.ImGui.ImGuiPtrOrIndex Inno.Native.ImGui.ImGuiPtrOrIndexPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15935) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e6f783df42b6ed5e"></a>[`public static Inno.Native.ImGui.ImGuiPtrOrIndexPtr Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15933) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7cff3672e3f60b9b"></a>[`public void* Inno.Native.ImGui.ImGuiPtrOrIndexPtr.Ptr { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c1c60f766e8adfc8"></a>[`public struct Inno.Native.ImGui.ImGuiPtrOrIndexPtr : System.IEquatable<Inno.Native.ImGui.ImGuiPtrOrIndexPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15924) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiScrollFlags</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiScrollFlags 83b91a120928067602561b1c3b158690b33b23e2920647e9cb2bb308f176b0b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.None fd2e1c8426a7fa9d5675480e2cea8fe356f50207656bec58878b5913bd250535 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeX 07ce3cbacc79551a6f9a03286128ad622ce60def3fc701e676c7f12f4127cd79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeY ef06e1329fda07bb8efb65d1a44822c94cb450497c01588a220951f7c49a8dc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterX a62e31f5e34f819854b35e31235a02ba3404981f0cf9d217295ed7f3fee6a7ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterY 76c4371c15a157692817fcaab7e9b2a1190d446f036a0ae2506a08595a8c398a -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterX eaad362c4f2f6fabbcc74f345033da5f9ba65bc05f6c1d43fac2272594a0d1d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterY 3e1e7b962f84b83bd8478be265ffd61700d9d517e3adb68a703751e705389f34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.NoScrollParent 4f2b1b006b72e0258502532441c62c7cee5d53fbf981d8d2c8eb14cb8e8d3497 -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.MaskX c6a856f42097e72458ba57827e2931a254980fee211066719c2e60453031bf6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiScrollFlags.MaskY 99891f4eef96364dc779ffe115e12c25ba4a9f6a9dc802b37d84b3ac4b462c11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2122107d3c221b39"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterX = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1534) | 当前常量或枚举值：`16`。 | — |
| <a id="api-cd23c367229cb58b"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.AlwaysCenterY = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1535) | 当前常量或枚举值：`32`。 | — |
| <a id="api-241c412fd3970e7b"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterX = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1532) | 当前常量或枚举值：`4`。 | — |
| <a id="api-b267bdc523e2ec7d"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleCenterY = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1533) | 当前常量或枚举值：`8`。 | — |
| <a id="api-98f797e5258ead1e"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeX = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1530) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c9c7ebf01380b744"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.KeepVisibleEdgeY = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1531) | 当前常量或枚举值：`2`。 | — |
| <a id="api-23adc1222b8fa226"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.MaskX = 21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1537) | 当前常量或枚举值：`21`。 | — |
| <a id="api-1b1f49fb598c472e"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.MaskY = 42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1538) | 当前常量或枚举值：`42`。 | — |
| <a id="api-3ee0b83ff3fe311d"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1529) | 当前常量或枚举值：`0`。 | — |
| <a id="api-6942fa1304b94c26"></a>[`Inno.Native.ImGui.ImGuiScrollFlags.NoScrollParent = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1536) | 当前常量或枚举值：`64`。 | — |
| <a id="api-2729164b0dbacb40"></a>[`public enum Inno.Native.ImGui.ImGuiScrollFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1527) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectableFlags</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectableFlags e79b69047edd458ef269fd9627fad4e75f76b84751ca7bb1075e03a778a1cea0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.None 92d68b95b147f0abaac557b0f91f34ea90e11a9b32896a78ce424508597426da -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.NoAutoClosePopups c0bc8e332571ae7533321fce978d07854a9a6256ae7685c1b041c70084e36fdd -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.SpanAllColumns c9cafbd7f0f17806c5678db73896e4b80ac78520bd2afec62e0472d0e2764849 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.AllowDoubleClick 0c88c7c9e908aa906bf8aebf1ff4b23508094566993517b9551d5606a25f6ab1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.Disabled a2408ce05ee8d935300d364dab99be6a502f1de1445818dd3d0c99ce85085536 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.AllowOverlap 2eaace58dffea308a0c680c55d8ba9be5a988349ae44c87fdfff259279510cf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.Highlight c84f5cdf07b8e7230790f23372d9a2666a3c47d1850f00121f799fd1ea074e4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlags.SelectOnNav 69b09b5eca5cea89348b860dddfa26a07ee17c16923a64d7a00bd5f0a496f8ca -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2ac927753f860d1f"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.AllowDoubleClick = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L535) | 当前常量或枚举值：`4`。 | — |
| <a id="api-9b3b3400cc855336"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.AllowOverlap = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L537) | 当前常量或枚举值：`16`。 | — |
| <a id="api-1b3c8236acc5c265"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.Disabled = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L536) | 当前常量或枚举值：`8`。 | — |
| <a id="api-bad567b4234aef62"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.Highlight = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L538) | 当前常量或枚举值：`32`。 | — |
| <a id="api-d0bfd360dc615642"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.NoAutoClosePopups = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L533) | 当前常量或枚举值：`1`。 | — |
| <a id="api-635157aedba687c4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L532) | 当前常量或枚举值：`0`。 | — |
| <a id="api-61ce9490ca0cde3a"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.SelectOnNav = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L539) | 当前常量或枚举值：`64`。 | — |
| <a id="api-08000ad116af3cd7"></a>[`Inno.Native.ImGui.ImGuiSelectableFlags.SpanAllColumns = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L534) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a9ae011677891708"></a>[`public enum Inno.Native.ImGui.ImGuiSelectableFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L530) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectableFlagsPrivate</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate 483dc62be4d413b4f76c1a4465a764177e3eb6a878c4605832d93ecac1e01f16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoHoldingActiveId 8772aec504e71e705f3f61cd5fc7af5c2e3ebfd124cb4f8c94403e59c76dfc95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnClick 15df74107db4dbfbe2976a487cc4e366fec959478715ba1c99c231203b85bbc3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnRelease 61047701ab9502fef990d3f933a09d548a279393606a1adb92010cfa89420b45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SpanAvailWidth 805f7a2a6256c1b499b04255b8f0cc66e00c0d3e705910a3c7ad171e8ad9806f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SetNavIdOnHover 6393cc2db857ab8d6f9959aca5dcf72259a106ba126642ecfa2d01c61f47a581 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoPadWithHalfSpacing 58b19d3647ef2235dc221574751485030b8e44d919a81c86690d340a3efc8137 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoSetKeyOwner 730f491ac29170360d9cf6b0958189b4e743e1cd8238806d4cdfb982347303cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e85fb6e411fa0dd4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoHoldingActiveId = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1322) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-cd81ebc6f3851a85"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoPadWithHalfSpacing = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1327) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-deeea01cd6a33a59"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.NoSetKeyOwner = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1328) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-59746736764f59d8"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnClick = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1323) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-e489f7a606d017fd"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SelectOnRelease = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1324) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-775a9accb64f99f4"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SetNavIdOnHover = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1326) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-86b4d34bece39957"></a>[`Inno.Native.ImGui.ImGuiSelectableFlagsPrivate.SpanAvailWidth = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1325) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-3aff27f96cad158e"></a>[`public enum Inno.Native.ImGui.ImGuiSelectableFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1320) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionBasicStorage</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionBasicStorage f305c097dd05abefdaca772cad86bb7fee0947843687eb8456effa4c2c844a61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Size 2fc9e317bfb0bab50865c1f491f3f6b5fdde622a3321d704f2efaa0c5b651817 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.PreserveOrder dd103c1cf59679162c2aeb5cf9c4e8c412c0e958fad99697f4b23a39d8123624 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.UserData 83835e6b344b89c8e8f98ae23fe66992c2aea03adf047248be6696ea84c036e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.AdapterIndexToStorageId 7b0e30e135f8295a67ccefc5852efaddf4d94233e5253022f832691cd2307f51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.SelectionOrder a7a6e38212cc995c7833fd65980647fb09c17b70bbbcc3256deeeab92e5936c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Storage 39cd1df731d42ca2fb2c6acd6255114bf8596201b0f480452347ce9fd0e2dd7f -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.#ctor(System.Int32,System.Byte,System.Void*,,System.Int32,Inno.Native.ImGui.ImGuiStorage) 7a1f2ec1a24e1493e0ddd9af1645f3a0a908086e36a3772200d19d797dad05a4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Destroy db6e2549ac7f5b3616006f0f6aa0b6e1de83181cfe54717350331e99a8aa0094 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIO*) c9a4f914ffef3bbd1077cfffddb6b34a8267016815ed6adc17e8240a43830ead -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Contains(System.UInt32) ffa63993bf006efdd637082cfea327c5e6a07569010812058e17559bf0050966 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Clear 11975051616569703ac5d4f018991a56b67c47f7bf74276d5b5cad9a1a0fc0f5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.Swap(Inno.Native.ImGui.ImGuiSelectionBasicStorage*) 5bb416a0db1694fd3593ae106114f25e7e554322c42a61d8f1a3c407936c79e7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.SetItemSelected(System.UInt32,System.Byte) 013e6d837df4106322751fe88409cbd153a63cc9608595f9f42c518b858ab401 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.GetNextSelectedItem(System.Void**,System.UInt32*) 0f8b0ab949ade89d6eb3370a4a81d0e0d7a5a598af21996dd56b3c4b44e8d82a -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStorage.GetStorageIdFromIndex(System.Int32) 34bda9ea4bfac9bc8dca04f72c0f18c4555a1fba9542c307e7b36e038c55ffc7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4868d0dcfb2f0427"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionBasicStorage*, int, uint> Inno.Native.ImGui.ImGuiSelectionBasicStorage.AdapterIndexToStorageId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0c48a047497df1e"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionBasicStorage.PreserveOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c7bd8f1f3de9b5b"></a>[`public int Inno.Native.ImGui.ImGuiSelectionBasicStorage.SelectionOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9557) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff910581c064ff95"></a>[`public int Inno.Native.ImGui.ImGuiSelectionBasicStorage.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27e7ff090443d8a9"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiSelectionBasicStorage.Storage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42a4915237174a46"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionBasicStorage.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9555) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9dc2e4902c14109"></a>[`public Inno.Native.ImGui.ImGuiSelectionBasicStorage.ImGuiSelectionBasicStorage(int size = 0, byte preserveOrder = 0, void* userData = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionBasicStorage*, int, uint> adapterIndexToStorageId = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionBasicStorage*, int, uint>), int selectionOrder = 0, Inno.Native.ImGui.ImGuiStorage storage = default(Inno.Native.ImGui.ImGuiStorage))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9559) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5fc73ff01ac9f512"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStorage.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIO* msIo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64130) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aef36a92ba39fb00"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStorage.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64152) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08d4a2c298da1e69"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionBasicStorage.Contains(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-114b219c19cde81c"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStorage.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64119) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cda0d2daea841d6"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionBasicStorage.GetNextSelectedItem(void** opaqueIt, uint* outId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74f9b6560bb16cdf"></a>[`public uint Inno.Native.ImGui.ImGuiSelectionBasicStorage.GetStorageIdFromIndex(int idx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64196) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-750745dc41484d88"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStorage.SetItemSelected(uint id, byte selected)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64174) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d9563cdec5d14e7"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStorage.Swap(Inno.Native.ImGui.ImGuiSelectionBasicStorage* r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64163) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ecbeeba48a29d11c"></a>[`public struct Inno.Native.ImGui.ImGuiSelectionBasicStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9551) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
