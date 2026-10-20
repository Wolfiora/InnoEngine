# Inno.Native.ImGui ABI · 06

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.ID 31af3e1ad5788dc498cfb54db53bbfc601b57a83cb427c33a319fcd92cda8b43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.ItemFlags d1f8f1bd760d97932809ffa70c8d69b9223faf981b391a25a2e3086ac304f15b -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.NavRect aa70ee04109021922fdcdd53233826f003681924352a4fe9da261e7852ec85d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.Rect 2d0d133098515f33ea39694b581f6fd121ab64429db1d1b14a161f2ef18a0f52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.Shortcut fc724405b522c4ec4d15073b5f054cb937103d18bff8f15bc771aa4b1b46e74c -->
<!-- api: F:Inno.Native.ImGui.ImGuiLastItemData.StatusFlags 68bd5fcc7310398c45dcf714d365829ad9bc619773929c922b8528e6331fdd34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLayoutType.Horizontal dc425247c3a974bb2e00951bc42d3f9a026c21e70547dffdb33bdaad4cec0e8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiLayoutType.Vertical be7319cab6d0b81b48d78d8216896611fd311642212a673728bf2cc2e8df7351 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.Ctx cc0b9a07a1114b583b7227f6a987426448704edce9faf23c7a73e502abaa70c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.DisplayEnd 010f6e0c0df17ed77733300f2c395f7d428a9364c447053b7c51bbfe1e2c7e32 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.DisplayStart 92b376ea546f0ea1ca52e3c7e980576b579467e5c62dea68b683610b70af7bb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.Flags 77fdc4a336e15f07bd28fa53bbf9d1c0bf5811f6a215b64de171691fcf37e0cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.ItemsCount 72a2f612df56848268aaff95fcec29c7913232d5d7eeed1e07d8f2269a77b7d1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.ItemsHeight d5a1b3ec5174967ee76f5b9b1278644c578a58ea07a505f46920dce355eebf08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.StartPosY 46fed085b69347d0006b9a5d7af65ebe521c68b40d3f4a892dd07a321b03527d -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.StartSeekOffsetY 2adee374eccae7ad94132419c29d898c532966ac4fb62109a2cfab58defddaf2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipper.TempData 0316f94f0f0f313b1883b007cc4a2bd6d13d5fd51742fcb0b5f337e1fd2292b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperData.ItemsFrozen 8977353da3ee483489bae2d599e93a9df048a1437ca0f9d4f8f8c85cf113123c -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperData.ListClipper 34cfb5684b1d5a91ee02704582ea64354fbd9687f9d0ac9bfc02f2c19f9d5ac8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperData.LossynessOffset 9dbff7a0456f69f382253d5f053001785c0af73555054fe5e0b233e5e4f2863b -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperData.Ranges e832e3665dce74fa033d2b0e876eaa1f71b9e055edbbe20eb2190cb960bba5b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperData.StepNo dbff3f9a1085d5d734cdd45e5375bd86b2f6269663364c0500eca022863e6f49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperFlags.NoSetTableRowCounters 94c4614f8a1ed2b0590645129562bd24366e3a7788661c2bb457e9f7959519cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperFlags.None e5dd29c2acae9cb44658aba6823adcf5011132f577ac7fc8f3f493d14ca92e0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperRange.Max 3ccf10e76006ba811b065bd8d715296497985fe8d2cc315cf25679bb31257866 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperRange.Min e5e3f8b6f69d5a08b25b9f19b16f3fd96a6df233e7ca682947a875415ba73b83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexConvert 7eec8e707c8dd741796e3149c6b30f44a908bf8abf0a9ba1a343277b474c4659 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexOffsetMax 280f785c07219d15eec624c380986e69c61225eb88a6cea8f93187053d0b22b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexOffsetMin d8d1fed236736f1dc22f96cc9672d73fc8c22fd92ccb4b08f235ae3e3225234f -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocEntry.Key 54511adea2d9bbfb516d3892eac857b6e8452aca8f1418b9528c80464e48124c -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocEntry.Text dd1a14d8b8e0e47f86e63bf889a406e37588f4aa1295f51cf6200a568636a72f -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.CopyLink 89b2155c8f3a3fa9f6c562ae1c5dd82ae36f59764661b4810c405f4da16e9683 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.Count 7bd9f1192cf2bcb66f9246a9ec46a841e75354a2ded6d79634ec1db410188ec9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.DockingDragToUndockOrMoveNode 9ca950ced58be37bf2b18ebad834d95f0d99fd71632ebdd73f9d06c705968c3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.DockingHideTabBar 3c6f8d90252de2c47ac5d1c9075a45fb00db2fdbf76cf97f341d2bbea2f3d95e -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.DockingHoldShiftToDock 078f3036c298dcda6b5ac4de0ea3edfb8a057f9743016fe6f4be4e40cf9a5bcd -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.OpenLinkS 57a278b4295cdc24ffb00b0433e6c106934f2e702593533270b484028c13078a -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.TableResetOrder 343dbdc9bbabfdc738b7a064dfdcb85bf9cff466840f772853efd81248991889 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.TableSizeAllDefault 5fd07ebb0b67b6f98dc38527c34297934f2852ab3a5f39d92ec2e47202c74464 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.TableSizeAllFit 634f9bada726d42f54924ca32370015fe54eda5e369f177446c5da4edb589d4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.TableSizeOne b13d090cfbaac26b0eb9c5540bf0083bc3ffc06d56dd5d8b144e9784df5dcc6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.VersionStr baa97017d2aee21db098c3ea7750e5a6001fadd939e7016bb10e50448405368c -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.WindowingMainMenuBar 584a2ce5883e4d4a03df0b97eaceb45803584590c3ce2945c3479c4a9f35c319 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.WindowingPopup bfcbf95653578a6b96639bacbe129c3163712d09361cd04a4e48bdddf9d77a62 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLocKey.WindowingUntitled e280fef3e618ed7d7c0e301eff4c74716cbce622fcc3d0d43aa1a44bb5c26c82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.None 0cb4a04d2e29a63fd069925288934543e5f7cfec94a7ab569cf8bad64601cc34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.OutputBuffer 5ecfc8e7fac72cd66a454700fff875fb0107c503492705813b52b6610f6bde66 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.OutputClipboard 2f4e20db37a722b195347caf1c71342378d0a1450ab3b60c6a9be3956f7931b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.OutputFile 3ecd1db0a3af81078c8143f7f0318ab80b961786f80342cc00d4fd709b467930 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.OutputMask 3e90f2630d1b3b41ac1925a2917767cd927ff7d08f856f2dcb00e672e6dd2a80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiLogFlags.OutputTty a7b14fabc07771ddf6e0f01c81384f327a18d98e94a70e397ed4061333c95329 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.NextTotalWidth aa92cf50ed700797f4066d898498b34791990372c8b02f187991b6418db1f67c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.OffsetIcon d24bb932fa6511f0d4878e2aeeec7a215c241e38127500e4496939bc2a152407 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.OffsetLabel 251c2dcd94d7362c48fcfdcf10d5e75dca5ca697337c792a9333706fe9cb1dda -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.OffsetMark 7dd9f745276b3d364d55cea35a80e45c371071d2ac2bd9eab47212fe35159a83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.OffsetShortcut 93624cbffc52b08ec3c760b3f9e512998df30790783ea10364636832407aef21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.Spacing 29abdd264fb73b6688631d2d36b70acf96cdcbad69d7ed4ffbb79077f7995c4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.TotalWidth 84ef1502ebd3eb281bb7fc32e71d1daba92863b8ea6881c90c1c8fcb32b8e92a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumns.Widths 718dfd78c38fdeae072275c5705b6bf6d1a5d30c8a83298690bc9672d96f2292 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.HighlightMonitorIdx a757c4b8494852460a7f6f65583d2f06b2bcff2e2a4e358739c44ad2f8089977 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.HighlightViewportId 42b9635c19813ff30e13e388a2a66f8f441050ebb3bccd588dd5759b035cd93b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDebugLog 9daf376cf91f3b2c84b2fb06d8d6089b85e3679732a117cf35d96e26cf531716 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDockingNodes fb71b097d1fcb51223c38c0f0cbf884e80bd0bbc7c298292ad7f3e48774fc52a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes 59f3b07f3216e1958dd1cf06bcb453be038042273a3cc93873bcf7f4c30e95a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdMesh aa8bfb6aa36ba7f7ad66c880c3a04d7479a2798ea8a37afae526f8a92b30f6e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowFontPreview 48a7abc4630d86ed5b29bbdeae96c2ff2cb7ef9a21207b9c533e928d4e2c2c2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowIdStackTool ce1f465948d4c4e1cde9eb45d7088d70d94e8b075e75e5bd4c445965892a9ef8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRects 779557f697da954133f3d93e68d74af1b2a466b58a63dfaba2ef272c29e7985d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRectsType 25ce956b73e904a18b151c0228b6de3d584cc17a10d9488e08eec53eb0ac4fe5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextEncodingViewer 99bbfea36151148e9c042e24f0530b085f40a995e200acc80c5df921d9142abe -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextureUsedRect 1c1ca2b714ba6de39150bb71f4183277e43ce58bf6fb705b78f9c2160360ec14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsBeginOrder 71950b681ef04d183cd3cb82388784809806ffff0a581cb2db2d2d9f4f87d22c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRects 607fa9073657cd3992f7bfa8360c526b42741be9a690c26bc8a6e7b0459ba7bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRectsType 63c1acee7356841e328c6fda5e0d97a82134d6bab9d77014847e381c3359e86d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Count 334d262d3d79bead188d2de21360e669cee0a68df8470be1038bdb3f2b9d0066 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Left e7145fd4085c2462dff441b45d4121a6b62d22dc243f66ebf3a588b12218868d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Middle 47041045a7ed4a9291fcb552d7c769014a175ae58cd98a08ae5e6082dac55101 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Right 0aaabee2694d6913620129458eb5514c4fa1d0e5b6650d6a4ad373e8480769a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Arrow 487a020c8a5dce36302640a59b04e386d9e087927e3e23c8065ab2883883a2db -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Count 413fe82064f84ed9b7eb4f3974d5c1748cddc61eafd7a91270c686bccee9a3f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Hand fe2636d660f2dc87c03e5b101e0f0501377ad0a0b2b041b046b45a150d9580e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.None 5a67abb7a545ef26a5c8570b601ce362af73c1af7dc549c2a033ee496853ee00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.NotAllowed c0fd5e82fc7b404a00ffa0afed584519152d4d0cb1815e1d3d2a2819e615607a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Progress 33efb98ccdf07d18c76919a31f3e4440a413301190bb74546e4012af6af3b05e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeAll ff0dce0352afa515ce58ef9ea6ee3ae90701ba3dc3b3d07383683327c96f9b05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeEw 69d949633184a89b98200863f98210b39867ec01d3fed940aae7aba96d428105 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNesw 1874c76bedc454acdb3982fda4a52d07cbee84e05f28cbaddbee0aad31862afc -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNs b39712f1191571cc507d0e891b66704c27d65f88547cfeef390924c1604aa2e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNwse 8c4d882e445c6d2984a862ccd0e0219f107ef24e5e46e64cf4b87a5da2d3758b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.TextInput a6972dd102617a954725a4b2d3a1a6c78d3f4ca12562162cf49d6e79f6d1524d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Wait da83f7dc43ba4f9b18ccfeba8467a6302c75258e6b4b5574102a0c85969650ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Count 580d44c508e26e556cc52461f4f02539071bbe5ed1e4e5d98f5b605352f0d5b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Mouse 6b277eca9b3b9056c74618b3a57b228cfbe956a5a0633f999696dc33dcc5b205 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Pen 59971b676438767ccd57a68d0ce6eca98c0fb9876746f66e81ba9b9d834aa4c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.TouchScreen d6f0e1f7df07a57b8803fa351c69710b75a00008ca2ba5bd35f3e3b7c385a573 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect1d c81cbeffdc314d387bd5d733edcf4411d9d71c7859a2231d0448d9ee4a935ec4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect2d 91ac7c1cc8631fa46fd4bd07b674ff2d706e3560f9c8551fc0f8af9770c45e72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelectNoScroll d1e4879919f7548e59b26ba33c6c8544af6c1a9d0e83ef1e5685f8939f77364a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnClickVoid cae82052dc96373492446e8269bc563e4b8aa832c28000c8764e6ebdccca0107 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnEscape b9c0a9fd289c668dca31b39937d01608d834cd36a09c65be9fb1519287dad0f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NavWrapX 8e8b269df467923641818ffbf602e742e44ba38a8256d3a21a831d3e2c660e69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClear 8333586e4fce18753ece9ca966ed990f91897049fc57fc6bc4ac76aec7d83c73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClearOnReselect e012491363ca67eca1fb6b4dafe7694da6497742c413414212860481f57f429d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoSelect 8402e75fa39f21c4622d68a1250fbc78f4f4ea603cb35e81d66824190e5cb979 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoRangeSelect 737ac369d8ccb78fac3ea6d9ba82a582e7254588d84a798a4ff2f9613b01f938 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectAll 1f56d6e8577c15424297fe0fe3c7730184e354a45d844cbd0c6198e706da1d8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectOnRightClick 1697112e2cec5e347aa89c3ab27f4989488fa52de48b7d0afd429d12c9efccb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.None 7bb419e19e16d32ac960bd1eb8b72c6389f40f7aa5f9ae74f1e3cfcf01ab17f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeRect 04d377208b3356b9deae45b0c234cfca8f24952627af654fcf4d7893e3e01d7f -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeWindow ed4cd90f68be8347a33db41c8a3b3e83c224b852b2d28ae7364bad6db5c2236d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.SelectOnClick 8212ecd0755067a351d069030326c7754792bb8ddf9ea3a4224c351e55172666 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.SelectOnClickRelease fa2260f64ac722bb8f08ff14750000b8775164c8203f699823a51d3c42dc268c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.SingleSelect 04bfb905e1062d84bb47e4f16b87a289e4dc8ee22f701bf50ed28b7cfed569d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.ItemsCount 107876833ba6a0d177fe379ca355125e88970e95c0db46a62a83a833c3b8b2bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdItem 37906babb126bd1b1f3012558cb6853b3873f30cb9b9b8a87752a3444f3b642e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdSelected e27bc07a262c587c0faba6b91481dbd33bf4cb8a3b6d80e8622170abaa2b9d79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcItem bb75232b8bdcc6843c086db7ea3479145a1d3f6f7c44c8cddf7ca1ffa333131c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcReset 8832fc936071bf374d9a3b916e989d272a17d3f32e14822d619cd03971b94ce0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.Requests 7358a3ccfcc7ced70be4c980b7988076838e9403912e246c2b79c825a6170dc5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.ID 451413ad7d020ceb29baca8205e061fc38fc80c09d08201c95da69dce25f4ef6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.LastFrameActive 98ee4298ccbd5989627e14207918c9c03aa5e185dc3ea8c95d81e9eddc203296 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.LastSelectionSize 6dcdea3e80d45ebdf4c4b163c86ab8259bc2a2412389d8cd1e4e57eac8e7998a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.NavIdItem 186bc1367b6088295867894a58281478bf3041d46131cab96deb2ac0716b4de5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.NavIdSelected e022e922801c5d05e603f775c33cc7ae3d01abe7f2eced82c81694b71a9ce3b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.RangeSelected c9ce3aec7f594a45a1739cc933313a3b0135ccafaa57b489557126dd9e080a15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.RangeSrcItem 5ef5916e720ebb3ab4dd69c40eb07c93e1388de2fc7a5fff374dc10fa68414f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.Window 5359132edb2a5d8ff2ba169b5497b417835a2c7dd680a6a7aa8d2737a38274f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.BackupCursorMaxPos dba118dff1f50eb33d7bcf51ef00decaf7fee1ea052cabd56a983744dae0d142 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.BoxSelectId 04ba5d4bd75ff795c7c8a1f0f772e7a442a9dc3d96661f89e519ed261e4db7df -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.Flags ed27384db11cbf72e320520ea33475dedf81ecd35128d58f130fa5810cf43715 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.FocusScopeId fd4833f1e1e872fa631653c52338b67fb574616e115eb40eafa6cb1edb8cc776 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IO f8da4e856b44d95c1e8b0b958bba1642996205743ca8bb3c3a3fab5a132b4b7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsEndIo 3efc453ad708e5b51e88aaa2fe5106b0743e61c631dd920eafe6498b91fd3267 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsFocused 5f58388923323dccc3f645a08dd5b06bf814b971353ae18e510e2bc9ffda18c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsKeyboardSetRange dc910b83dfcb5a907835bf1ed39c0c2030b4a417b820752dcb53948919abd00c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.KeyMods 4daaa61673c993c49b9aeb3f82cfa8fdb9e54f824e923eac4b1ddb5074321b08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.LastSubmittedItem 2c903c26892c3004a4b6e76cc072a5420a71e1e65e89350438da26ed2110287c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.LoopRequestSetAll 51bd4df146d256b46496c3e6e5ee613a7dbd10de608df0a64b69d5ca333127cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.NavIdPassedBy 91b88d02f31ece400bfdb3f838a5e4215259708115508a9a0d5ba1c35b91b288 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeDstPassedBy 35e4495abccd76fd50a1cf15a7d644d66679165cf234b075d64f2b635ce8c911 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeSrcPassedBy b24d8cdee7688bc20d40c634a17196bd7b8d4972ad013c70365087230ce4d2d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.ScopeRectMin 54461c61703a4924698a74a88b9206c0db63e3c563398019143f830ee5b22caa -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.Storage 5c73198a8ece93bf4166c9bae6c5af8be8ac958f36c9dafb5ecfe5522ead9a18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveId fdf74cac49dcaab358656b1a58081376871c6d7862759f9c47df0b8f55f67926 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdAllowOverlap d9cc52198db1fc4a6aef1d5725dddb0ca5d07b23df7cea2440986e3fd97bb56a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdClickOffset 45f61d9880450ba9ea18b08816cb31e3d72f03f2414d1c28c9fbbb707a8cf1ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdDisabledId ade51ffd1df81a1f7e610f7fad15fd9df4fe8ef8fb180aacd8bd537777fd9738 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdFromShortcut 53232dfd0048f44a1d259298931315c18372392e583164c534b74d0a3cdceb94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenEditedBefore 6b199220f9a0b38b3d650d9e93a1ab164939051499938f782068518a5be470a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenEditedThisFrame 48f954cfa08188e35b19ab79ddb3cb52c03e334c84abf045cafc4f7f5b784236 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenPressedBefore a0247a9c5974e9990fadd5f085fd11a5894f799f63c4ee75ad64425ed26a0db1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdIsAlive e94ad7b09256bb617a1c5de38dc815ef57d85cb0c8789aa5110e3f59708eb8c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdIsJustActivated 204421bef055d8391f4f5deddce76b33bf60fd4606c40a97f7446d728fc39935 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdMouseButton cfc8ce207324f02d984f68b08e3570356c8fad26676eefbd5f4846f906a462b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdNoClearOnFocusLoss 8a1a69e6c1ea8e7d8f8b893e197658bd1a78bc4784b6b3a31a611cd35ea059ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdPreviousFrame c6091fd09b50085860f5e48b458207f19a44e01b38ef2111bd035cd4e87b9ee6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdSource f1f247cdbe0ac13f1d6a645d905e27201f8273a0192e061be55163b03d7df6b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdTimer 56e8d73ac30caa997ef28d90c63408bdcdc8b7c053c4b78a902c9f9e38f83e1f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdUsingAllKeyboardKeys 895d98cb41cf4a34481352f6e15a7d228831a4fc8d122fb7803b1772c14820a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdUsingNavDirMask edd2694bf589e09d4e118b7cc8686be4e9e143104eb2c551504d85c653a3b1ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdValueOnActivation 84b2930b30d7256123fa905eba5df9a42c4cd239560b4873dc8398753e72e3d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ActiveIdWindow 809c5da05da4ac4e7a3d99d873721e3286b46ff7a999bb4de8d9e88e484bd25f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.BeginComboDepth ada126dc3d1e6ef24a6610a20c373da9e41b11ad792b8486c092dcd2378146c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.BeginMenuDepth 42ac5758ff596a9fa106f0016a5eb1ce4a6a9559f0af77e70bf6b78875897b6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.BeginPopupStack 55ef74c23015494b7012e6b70ccf3a78aea143afbf0599f76b21ba198e8dbcf5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.BoxSelectState 47fd85183088ede5b292dc8257ac0789b8f5138e722178ebcfa0c5fdc3f2e6a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ClipboardHandlerData 32551bbb2d0ed73f16231695af2c14b251d7de969c02d2b3796d090dc6332c2c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ClipperTempData 749da65f5187b4eb19dda680d6fcf6b5b65709c3809fc3d03ff91998c0e118bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ClipperTempDataStacked 51e305695102fec4589f5d4e2d9a7847f0a3bd83d9d37cfd39ba1ce96571aba6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditCurrentId f3e0be677c132592abe8c4405e461019d1e11cc7c70d4575cad2610a53aee758 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditOptions 837e01acf6d13b67dc4a638676e888c91c8ea1f3199731f6f10418a66f014efc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedColor ce1d5d7c72536bc84095f45f9427f4a07773f6b670740a63223e698b4c39cdce -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedHue c4852aafcd4b8765e4c981590103860c8ad054d91aea9b7cd3d37a19e9c1fa0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedId 5a6246e66abab6e456e49ade8a604d7fc8f84711eeafd291b614331b3c670252 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedSat dc9844c526b32c75295cd420ecb7fb03a773ec79003bd4035ff31ce84d26e67d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorPickerRef 6d2540befd1d9fb66ec752fe4622e9bbfc03d635c8e156baab7dd99e8567fac6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ColorStack ffbfbb375c17c77ad01157b73f7e7f4121505fba5aed75e169c3416be85a86bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ComboPreviewData 5cf4f133a669be9e0c33853654569b25479a6387951fd3daa94d4ba0f70ca238 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigFlagsCurrFrame 39e7d91c91b1e525a37aeedf8fb5421470014b7ae7425e1ffe76e04346b2c901 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigFlagsLastFrame 29b007ecee0277a5fd7ef38c096cbb9c81bdc66d7ee1acdb00b858e71cfd4391 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigNavEnableTabbing c1f99f7fef92fa616710909a44bca745b32cafc8375177f06296730d506f831a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingKeyNext a1a38a96eda89048e407a383a825f8be276f9d5981143f0b12c082d8936f3437 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingKeyPrev d3ef927ee13c73237ba7a2a947f66b1d24b3528161a59061104620fc1b31667b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingWithGamepad 90360828a49bc556d54fd97c9c9e594c66cd859a009651e18eeb2b4d5ace5149 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.ContextName 3a9405f0c2edd0f35ba00912a1be2b4958556711f1b172e949b27acc54dd44f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentDpiScale 3deb8f949a59a13c2303976080ab609ef72fa7ab3aea201f6eca857ff306e795 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentFocusScopeId 281fc644e3a1a1790f4167b149696cbacbdddeb7b920dc22c71e84373f38c551 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentItemFlags c11899407884a367561749bf4d55045f511f33e398268ac98d333ed3287375ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentMultiSelect 3067631309adc6838473284e443745fcb1bae16d87f1a2699a2964a6172f3c80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentTabBar 58d3f5536c634e673aed09a62ea41009baeab4d6d41066fb92583889782ee5ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentTabBarStack f8b08f1678b25f0704c13f85dd8d7132ab7b28cf4121d5fa6884dc6b922c1326 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentTable fe52695cb3eca9ab5125c63806c93675940c1f51f197ac7e8f2571a8fdb86933 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentViewport 49fbc59d615267c59d8ec52cf604b56623c86957648fc1dab105ca6f93cca50e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentWindow ab89cd4fba07f78dabd0bd8a8227fec8968ccee41946e3feedf0c124ad46f367 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.CurrentWindowStack aa8306ab82174738d03e47efd9d6c7dcdad86360172b7a0bdf6ef03511a1e55e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DataTypeZeroValue 14c07f9e2ae064801894a056eb7aa799962ab0a35bffb03e45aba0e9d42420ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DeactivatedItemData f20f89a32f7031b5103349f6a174885e54c4195ce75ee8362c1643cbac4c8f07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugAllocInfo eabbd3ff04e73650302b1e24dc6f1be7184eb1e44c95705b71efb357d7219d26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBeginReturnValueCullDepth 5ddc3abef8a380bbb9123d55f5491e0dd71bb2d60dec68a3cd04401b23abf7d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInLocateId 85ddf344bff40e6356788199eae04b299332d45e548a0ca403c0dfa64373309e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInShortcutRouting f81121eeed209503ba0f713d6226310b4fc59b130b07fbfe6bb8d08d75f3be78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInTable 8076144d667307f052db653293e72d7f3c29e9dccd7468c4de91d58f4bb10458 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInWindow 9af0a889bc3511bf32022ed657302ba39961136886516ad152fce9ab86bec227 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugBreakKeyChord baefc2cd6c698e0529c3e5afa9921d34385b2b7d0e4b4bce1e8c6ccd4fdb5921 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugDrawIdConflictsCount 8282e491091fbe356eb94ee13b65334ffc33a0ab10b2c60bc51dc230e1909b27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugDrawIdConflictsId ff9ae3364be79cdf38b885de405bc4e4ef838df1cc36cfaa76efb790de4edf97 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorBackup f245b769b46cafd70cb2cfc560beb4ddf577e08790b07bb875642eb5268f3455 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorIdx d88b0e1a184f2212f1baa014784f011867666135ecc0d435d46e9d0f27c3a51a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorTime 269744fc8f247dba89292ec8bce26e3c3388b5edb3232c8f04e174416a0b4373 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugHookIdInfoId 548d1ee2916a0667087ebf05b7f0c9d1801812a1961e104d92e1b9cb5a3a50af -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugHoveredDockNode 6251a2ed62c718da5ce68cb84980ce3650975bfdeaa106ec75af4b42b020ed99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugIdStackTool af2bdc8c8fa3be90cee86549599a0f035df5afc92fad4bf1d9f87629f2806903 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugItemPathQuery 5694868ceedd6f3bce5d5210fb8886fb1295031a3a5cf39aca42015a44a8ab99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerActive 644fe08db7b316eab83b2425bc8db70b92d9519cabdb68b7837ba20a8976097a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerBreakId e6c934382fb31c33df7541689b7d56701cac7df7bb16623e95d3ee6b6a94607c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerMouseButton ec04ade199a25866e51296daa282245f9c3850e44f95b3606ae2dbcd93e2af00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLocateFrames 07a2ad85a0908a7f1b3623682517ee35d3117dcd18c0ab19bda3927ff2e9c297 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLocateId eb9babad668cf6149811fc8acb8bef0e473d7e8711fc03b7397939febc6af166 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogAutoDisableFlags cd25e2a648625ac937f5305657b190d9847536a17c90689b119dc302c5db9297 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogAutoDisableFrames 27af10f29a4159cb3fd3df3e1377ff626ac9ef6c5dc4f04123db533a3b8355fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogBuf cdb22cbb16df4fd15b3212ec1486d35f6b08e9aa3a3d9fd3fe611c3f702e25ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogFlags 14c5e0bcdf52d94dbca90a8da17e5a37a7d90eab8f4c7e7454d0adaf44a83416 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogIndex 29d5668cd99c5ceef83a3891ccc03f32b41aaafce3df901c97ea842a43fcf7b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugLogSkippedErrors 887c3b0fa8ffbd59a75ee33d579340ba0b4d1fad9c306fa404ef66071459d181 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugMetricsConfig 3b10f4169e66a09b9616c077dc7f2365b569956d96c5c3b7655978e6fc664813 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DebugShowGroupRects 2ddbb2c0ad45219a4405a393ef2e23e88f7c41b52ad7f306bace3abfda302430 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNativeContext.DimBgRatio 7404f106f233b7b5fd670c8579dffa82562ee688385193978462efad2ea14a49 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-ae821d922743c0f1"></a>[`public uint Inno.Native.ImGui.ImGuiLastItemData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27683) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-623505034ba458cb"></a>[`public int Inno.Native.ImGui.ImGuiLastItemData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27685) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-51e200b07638e20e"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiLastItemData.NavRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2a1c844278e97157"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiLastItemData.Rect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27689) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-286849c9073d490c"></a>[`public int Inno.Native.ImGui.ImGuiLastItemData.Shortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27697) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-53a72c95fd9ced97"></a>[`public int Inno.Native.ImGui.ImGuiLastItemData.StatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27687) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-75e86667599b1215"></a>[`Inno.Native.ImGui.ImGuiLayoutType.Horizontal = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27712) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-082583e4b23bdbf6"></a>[`Inno.Native.ImGui.ImGuiLayoutType.Vertical = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27713) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-7a24a49c575b7bf5"></a>[`public Inno.Native.ImGui.ImGuiNativeContext* Inno.Native.ImGui.ImGuiListClipper.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27724) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4e52de819712e1a4"></a>[`public int Inno.Native.ImGui.ImGuiListClipper.DisplayEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2e00b310cb20124"></a>[`public int Inno.Native.ImGui.ImGuiListClipper.DisplayStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-97205c4e067ef829"></a>[`public int Inno.Native.ImGui.ImGuiListClipper.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f38407f2a8843cd"></a>[`public int Inno.Native.ImGui.ImGuiListClipper.ItemsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-777389dcd793a5c5"></a>[`public float Inno.Native.ImGui.ImGuiListClipper.ItemsHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c417ee36fc3e2e5"></a>[`public double Inno.Native.ImGui.ImGuiListClipper.StartPosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fedd3a6555be262f"></a>[`public double Inno.Native.ImGui.ImGuiListClipper.StartSeekOffsetY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02f770832d756d79"></a>[`public void* Inno.Native.ImGui.ImGuiListClipper.TempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-663b4e3c8e89bb4d"></a>[`public int Inno.Native.ImGui.ImGuiListClipperData.ItemsFrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1dc5e0448b3502b"></a>[`public Inno.Native.ImGui.ImGuiListClipper* Inno.Native.ImGui.ImGuiListClipperData.ListClipper`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9a6fb907ff27c992"></a>[`public float Inno.Native.ImGui.ImGuiListClipperData.LossynessOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27760) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3141c264352446d"></a>[`public Inno.Native.ImGui.ImVectorImGuiListClipperRange Inno.Native.ImGui.ImGuiListClipperData.Ranges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e65abb25880aecd"></a>[`public int Inno.Native.ImGui.ImGuiListClipperData.StepNo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2466e292f2c0fe0"></a>[`Inno.Native.ImGui.ImGuiListClipperFlags.NoSetTableRowCounters = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27782) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-c05da89e03f76a0f"></a>[`Inno.Native.ImGui.ImGuiListClipperFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27781) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-dfb468c965b3dc10"></a>[`public int Inno.Native.ImGui.ImGuiListClipperRange.Max`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27795) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f5e46c04f4b79e6a"></a>[`public int Inno.Native.ImGui.ImGuiListClipperRange.Min`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27793) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-127cf9569b093570"></a>[`public byte Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexConvert`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27797) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d347420dda507af0"></a>[`public sbyte Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexOffsetMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27801) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-50836ee6f40dde55"></a>[`public sbyte Inno.Native.ImGui.ImGuiListClipperRange.PosToIndexOffsetMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27799) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f15f615320e3f37"></a>[`public Inno.Native.ImGui.ImGuiLocKey Inno.Native.ImGui.ImGuiLocEntry.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27819) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-adc68fbe07cde5f7"></a>[`public sbyte* Inno.Native.ImGui.ImGuiLocEntry.Text`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27821) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0e686d7271cfb66d"></a>[`Inno.Native.ImGui.ImGuiLocKey.CopyLink = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27845) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`9` |
| <a id="api-6a39e50b2603cad9"></a>[`Inno.Native.ImGui.ImGuiLocKey.Count = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27849) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`13` |
| <a id="api-e9d5a400c6ffe65d"></a>[`Inno.Native.ImGui.ImGuiLocKey.DockingDragToUndockOrMoveNode = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27848) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`12` |
| <a id="api-e9f90bc8eca516b6"></a>[`Inno.Native.ImGui.ImGuiLocKey.DockingHideTabBar = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27846) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`10` |
| <a id="api-def2565bab121bb8"></a>[`Inno.Native.ImGui.ImGuiLocKey.DockingHoldShiftToDock = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27847) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`11` |
| <a id="api-45228edd52ec8997"></a>[`Inno.Native.ImGui.ImGuiLocKey.OpenLinkS = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27844) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-ac395e7de33ad9d7"></a>[`Inno.Native.ImGui.ImGuiLocKey.TableResetOrder = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27840) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-793a0d6986f96e6d"></a>[`Inno.Native.ImGui.ImGuiLocKey.TableSizeAllDefault = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27839) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-d6117ac27f738804"></a>[`Inno.Native.ImGui.ImGuiLocKey.TableSizeAllFit = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27838) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-7cb56379036975b1"></a>[`Inno.Native.ImGui.ImGuiLocKey.TableSizeOne = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27837) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-afa7a17cf231da75"></a>[`Inno.Native.ImGui.ImGuiLocKey.VersionStr = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27836) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-3472d1173696be0f"></a>[`Inno.Native.ImGui.ImGuiLocKey.WindowingMainMenuBar = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27841) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-837187000c17daf8"></a>[`Inno.Native.ImGui.ImGuiLocKey.WindowingPopup = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27842) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-750a3b93db72c87c"></a>[`Inno.Native.ImGui.ImGuiLocKey.WindowingUntitled = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27843) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-bbb19cf0c496cd0b"></a>[`Inno.Native.ImGui.ImGuiLogFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27857) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-a9daf23efaf4b4a8"></a>[`Inno.Native.ImGui.ImGuiLogFlags.OutputBuffer = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27860) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-2b018f61acd0a285"></a>[`Inno.Native.ImGui.ImGuiLogFlags.OutputClipboard = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27861) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-9026d2c39ada19f2"></a>[`Inno.Native.ImGui.ImGuiLogFlags.OutputFile = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27859) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-89fdc6545b4ee5d2"></a>[`Inno.Native.ImGui.ImGuiLogFlags.OutputMask = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27862) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`15` |
| <a id="api-54b2ba4831f98b8d"></a>[`Inno.Native.ImGui.ImGuiLogFlags.OutputTty = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-2199421596d9c06f"></a>[`public uint Inno.Native.ImGui.ImGuiMenuColumns.NextTotalWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27887) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1e7c11f64737c47d"></a>[`public ushort Inno.Native.ImGui.ImGuiMenuColumns.OffsetIcon`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27891) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-19c50676a9e25782"></a>[`public ushort Inno.Native.ImGui.ImGuiMenuColumns.OffsetLabel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27893) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a1ca417e20870f6"></a>[`public ushort Inno.Native.ImGui.ImGuiMenuColumns.OffsetMark`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f29244aa80d385d6"></a>[`public ushort Inno.Native.ImGui.ImGuiMenuColumns.OffsetShortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27895) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-beb568bd208c2c05"></a>[`public ushort Inno.Native.ImGui.ImGuiMenuColumns.Spacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27889) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa9478086e412893"></a>[`public uint Inno.Native.ImGui.ImGuiMenuColumns.TotalWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27885) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b01eb4116954dc35"></a>[`public Inno.Native.ImGui.NativeArray_c34fe7f9f3db1a4e Inno.Native.ImGui.ImGuiMenuColumns.Widths`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27899) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f848cae886d6a972"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.HighlightMonitorIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27941) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67759fa9ffdc13db"></a>[`public uint Inno.Native.ImGui.ImGuiMetricsConfig.HighlightViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27943) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bc317db5dffccde8"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDebugLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27917) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f0e326f99b40b6d4"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDockingNodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27935) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-96d59e096dccb0bc"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27929) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9c699d9c14f89dcf"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdMesh`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27927) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69ebd3e7cd4c9b52"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowFontPreview`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27945) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cac914dac8010052"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowIdStackTool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27919) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3b667a21fcc397e0"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27925) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb5d2799cf4f604a"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRectsType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27939) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7383752e796fa5e8"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextEncodingViewer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27931) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-775aafeaab9b5bb5"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextureUsedRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27933) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0c55f18ecc2bada1"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsBeginOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27923) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8807475d77221c7f"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27921) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03fc20285be5765b"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRectsType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27937) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a6fcfda0bb4df2c0"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Count = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27963) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-d59f238c717deb34"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Left = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-311b2bccee63e80f"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Middle = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27962) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-318fde4336096fab"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Right = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27961) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d77e9652c31f9eb7"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Arrow = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-79d62e2a04a6d62e"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Count = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27983) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`11` |
| <a id="api-ec310ddc0bd503c3"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Hand = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27979) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`7` |
| <a id="api-66f308d07ca94cf6"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.None = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27971) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-1` |
| <a id="api-56233c865606d534"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.NotAllowed = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`10` |
| <a id="api-72c7fdbea9c9e631"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Progress = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27981) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`9` |
| <a id="api-a50483949ad23016"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeAll = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-3fda170757875622"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeEw = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-6f771666cd035f69"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNesw = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27977) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-7d16ea01f5501b4c"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNs = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27975) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-2a2d09dbaa5137df"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNwse = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27978) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`6` |
| <a id="api-842b96d2a419b7b7"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.TextInput = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27973) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-a4f89f0dff919d0e"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Wait = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-eea81617c663ff3f"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Count = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27994) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-12a85c922e6d29f7"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Mouse = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27991) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-de0c1d25789e6199"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Pen = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27993) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-657a5135de0971ab"></a>[`Inno.Native.ImGui.ImGuiMouseSource.TouchScreen = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L27992) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-86730d1acfbdfe80"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect1d = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28009) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`64` |
| <a id="api-0183c70a4370a358"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect2d = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28010) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`128` |
| <a id="api-de6d851a2362e683"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelectNoScroll = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28011) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`256` |
| <a id="api-c55de8c772f6932c"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnClickVoid = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28013) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1024` |
| <a id="api-6e06cd554e1ab32e"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnEscape = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28012) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`512` |
| <a id="api-ed893505f5526d24"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NavWrapX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28018) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`65536` |
| <a id="api-5923897871255ba5"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClear = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28007) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16` |
| <a id="api-2973100e14d43fdf"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClearOnReselect = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28008) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`32` |
| <a id="api-033d4e00ed672469"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoSelect = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28006) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-94b313a4e4a603ed"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoRangeSelect = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28005) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-66e4f5f6909538e4"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectAll = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28004) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-799ef1b498d5e371"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectOnRightClick = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28019) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`131072` |
| <a id="api-81c213885c8d8758"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28002) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-a8941a6147084e3f"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeRect = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28015) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4096` |
| <a id="api-0f26c8441d9b24d0"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeWindow = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28014) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2048` |
| <a id="api-8d65b1383c594b2e"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.SelectOnClick = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28016) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8192` |
| <a id="api-9bea5fca23bc26d1"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.SelectOnClickRelease = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28017) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`16384` |
| <a id="api-45508217565d21c1"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.SingleSelect = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28003) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-c3088fe023f7bd75"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectIO.ItemsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28040) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-475d987b106085e4"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28034) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3ec9f40ce3b42b0"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28036) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8616b6c60c52a29"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28032) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb20130ca55cc1ad"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcReset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28038) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ea8a2fb04f9cf645"></a>[`public Inno.Native.ImGui.ImVectorImGuiSelectionRequest Inno.Native.ImGui.ImGuiMultiSelectIO.Requests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28030) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4890ed19d0130f88"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28060) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32d752a03fc7525a"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectState.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28062) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-65b47993faa656c4"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectState.LastSelectionSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28064) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec5aee6e7546395b"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectState.NavIdItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28072) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8a0a0b04c4024493"></a>[`public sbyte Inno.Native.ImGui.ImGuiMultiSelectState.NavIdSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28068) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9be9fa4d50e13c85"></a>[`public sbyte Inno.Native.ImGui.ImGuiMultiSelectState.RangeSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28066) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8cf191b13f087024"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectState.RangeSrcItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28070) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3daf8bc437ca192"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiMultiSelectState.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28058) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2de59547d03817a7"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiMultiSelectTempData.BackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28100) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0dedee8e029ffd4"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectTempData.BoxSelectId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28104) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8eced293ef8d0d3c"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectTempData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28096) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-70f4b0a4c576ae18"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectTempData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28094) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f5389a11744303c"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIO Inno.Native.ImGui.ImGuiMultiSelectTempData.IO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28090) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-089294645cfa7920"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsEndIo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28110) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c34374ac4ff3602c"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28112) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3db8867b3c06588e"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsKeyboardSetRange`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28114) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6afe04a3f737912f"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectTempData.KeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28106) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2e28d578ced6ca32"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectTempData.LastSubmittedItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28102) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-49eabb21a2b456c7"></a>[`public sbyte Inno.Native.ImGui.ImGuiMultiSelectTempData.LoopRequestSetAll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28108) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54f10bb97aacdb48"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.NavIdPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28116) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-897ddff5514e03d8"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeDstPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28120) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2633832377592dd0"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeSrcPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28118) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-64a7650d18092265"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiMultiSelectTempData.ScopeRectMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28098) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-87a0e63101f7dfd2"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectState* Inno.Native.ImGui.ImGuiMultiSelectTempData.Storage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28092) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7b8bcefdc18ddf56"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ActiveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28258) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-17b92f2e36c80f8a"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdAllowOverlap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28266) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcf589bfc28d62d8"></a>[`public Inno.Native.ImGui.ImVec2C Inno.Native.ImGui.ImGuiNativeContext.ActiveIdClickOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d10b925e46059f51"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ActiveIdDisabledId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28280) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3661daedef726bbd"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdFromShortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28276) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-327ce847885b24e2"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenEditedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28272) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec6398ee27bb5407"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenEditedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28274) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e97ab1b59c56d11e"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdHasBeenPressedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28270) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1b0eb27ab7798515"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ActiveIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bbb6f3b0db235c55"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdIsJustActivated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-62211180ca39ffa5"></a>[`public sbyte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28278) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b1b610c4f8dd3126"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdNoClearOnFocusLoss`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28268) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-31a86ea64fb474c7"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ActiveIdPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28288) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-72be07d802542359"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiNativeContext.ActiveIdSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b8e602bba5fa5d69"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.ActiveIdTimer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28262) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-68fa0126c9be3b4b"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ActiveIdUsingAllKeyboardKeys`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28312) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9b3fb8acd94456c"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ActiveIdUsingNavDirMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28310) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-587194987d592ad5"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiNativeContext.ActiveIdValueOnActivation`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0431d4b72af9cdcf"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.ActiveIdWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28286) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22fbf309e0b95e6d"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.BeginComboDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5052d4e65a9fcb28"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.BeginMenuDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28606) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dec1f30bc622d08"></a>[`public Inno.Native.ImGui.ImVectorImGuiPopupData Inno.Native.ImGui.ImGuiNativeContext.BeginPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af9711358b0fd9f5"></a>[`public Inno.Native.ImGui.ImGuiBoxSelectState Inno.Native.ImGui.ImGuiNativeContext.BoxSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3799a0ad284ccc8"></a>[`public Inno.Native.ImGui.ImVectorChar Inno.Native.ImGui.ImGuiNativeContext.ClipboardHandlerData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28654) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-593a2bacedd189e2"></a>[`public Inno.Native.ImGui.ImVectorImGuiListClipperData Inno.Native.ImGui.ImGuiNativeContext.ClipperTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28540) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-565209a13c8c058b"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ClipperTempDataStacked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28538) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0152c89e8fb1648e"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ColorEditCurrentId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28612) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fe0ebc01aec7fe68"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ColorEditOptions`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c2162092cea90cd"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28620) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bf8601333e0be4ef"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedHue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ba2c87e282bc237"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28614) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4914d6a109fbb18b"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.ColorEditSavedSat`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28618) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-90a6d80e6ba9a32f"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImGuiNativeContext.ColorPickerRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28622) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d46733814adeccf"></a>[`public Inno.Native.ImGui.ImVectorImGuiColorMod Inno.Native.ImGui.ImGuiNativeContext.ColorStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7b86707c5a00abd"></a>[`public Inno.Native.ImGui.ImGuiComboPreviewData Inno.Native.ImGui.ImGuiNativeContext.ComboPreviewData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28624) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-275c34107040bc03"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ConfigFlagsCurrFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28164) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0cdfbf879c6441be"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ConfigFlagsLastFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4fd6fba73590e566"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ConfigNavEnableTabbing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28468) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1f8eba8962139ddd"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingKeyNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-decc93135face358"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingKeyPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28474) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8adfb51142a75f8e"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.ConfigNavWindowingWithGamepad`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1058ff5a31d72a6"></a>[`public Inno.Native.ImGui.NativeArray_e60e879954c840fa Inno.Native.ImGui.ImGuiNativeContext.ContextName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28156) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-daad21e6dc473ffd"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.CurrentDpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28182) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1825c00e11bfa6a0"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.CurrentFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28316) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1ba8c6ad7f9663b9"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.CurrentItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28318) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff2b00f94f010184"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempData* Inno.Native.ImGui.ImGuiNativeContext.CurrentMultiSelect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48ad04e19078e0ae"></a>[`public Inno.Native.ImGui.ImGuiTabBar* Inno.Native.ImGui.ImGuiNativeContext.CurrentTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28556) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-89e650b76db1f023"></a>[`public Inno.Native.ImGui.ImVectorImGuiPtrOrIndex Inno.Native.ImGui.ImGuiNativeContext.CurrentTabBarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28560) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7faea184dbbe4e1d"></a>[`public Inno.Native.ImGui.ImGuiTable* Inno.Native.ImGui.ImGuiNativeContext.CurrentTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28542) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6d6def44c2f0cbe"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiNativeContext.CurrentViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28354) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc02d18bd98ae43e"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNativeContext.CurrentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28214) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3e6b6f29d3e29c46"></a>[`public Inno.Native.ImGui.ImVectorImGuiWindowStackData Inno.Native.ImGui.ImGuiNativeContext.CurrentWindowStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28204) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bf840ddab026c46"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiNativeContext.DataTypeZeroValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28604) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8fb5ef80d9d3e67"></a>[`public Inno.Native.ImGui.ImGuiDeactivatedItemData Inno.Native.ImGui.ImGuiNativeContext.DeactivatedItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28290) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fa13d492e9ae3dfc"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfo Inno.Native.ImGui.ImGuiNativeContext.DebugAllocInfo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28764) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c793ddd79cf5af9"></a>[`public sbyte Inno.Native.ImGui.ImGuiNativeContext.DebugBeginReturnValueCullDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28746) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54c74378cbaa9bdd"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInLocateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28742) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b51d1e7f2aa6e712"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInShortcutRouting`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28314) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cd8569bbc4b74aa8"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-004eb6ff6d25f7f1"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugBreakInWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28212) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eef89fad0323b8ea"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugBreakKeyChord`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28744) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-444f99b9403a366f"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugDrawIdConflictsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28726) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-55131ae5cedc8dc5"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugDrawIdConflictsId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-781fa7e3d03fd24a"></a>[`public Inno.Native.ImGui.ImVec4C Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28756) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec6d9a0af6c84057"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28332) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60f9160e99d43f81"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DebugFlashStyleColorTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28754) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-de464932b1ce6948"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugHookIdInfoId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28240) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c13ef3582d811295"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiNativeContext.DebugHoveredDockNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f80c4a15f3d48214"></a>[`public Inno.Native.ImGui.ImGuiIDStackTool Inno.Native.ImGui.ImGuiNativeContext.DebugIdStackTool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28762) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eed49179974498e4"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQuery Inno.Native.ImGui.ImGuiNativeContext.DebugItemPathQuery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28760) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-013f0e87907970b9"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28748) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff2e3f86bc32e987"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerBreakId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28752) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7884906ffe4445f0"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugItemPickerMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28750) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5030b630d1d3dd04"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugLocateFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28740) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f655fc2892fc70f3"></a>[`public uint Inno.Native.ImGui.ImGuiNativeContext.DebugLocateId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28320) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d2209a5fcd0e3513"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugLogAutoDisableFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-00b267f762572759"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugLogAutoDisableFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0acce15fdf57ed85"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiNativeContext.DebugLogBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28730) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c43ffff3aa15f764"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugLogFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28728) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a2c9146330c57a81"></a>[`public Inno.Native.ImGui.ImGuiTextIndex Inno.Native.ImGui.ImGuiNativeContext.DebugLogIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28732) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d7c84fe03945fa7"></a>[`public int Inno.Native.ImGui.ImGuiNativeContext.DebugLogSkippedErrors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4c352a58ad1cb0d"></a>[`public Inno.Native.ImGui.ImGuiMetricsConfig Inno.Native.ImGui.ImGuiNativeContext.DebugMetricsConfig`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28758) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3cfddaaa4ef9bfd7"></a>[`public byte Inno.Native.ImGui.ImGuiNativeContext.DebugShowGroupRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03d49c7fd844799c"></a>[`public float Inno.Native.ImGui.ImGuiNativeContext.DimBgRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L28496) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
