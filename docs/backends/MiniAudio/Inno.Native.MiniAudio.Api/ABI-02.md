# Inno.Native.MiniAudio ABI · 02

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_passthrough 0db4bc5e6852cedb4d2c6b6f640e668efede4decd5815ea868e8eb675dd8193f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_shuffle 941bc135d51e8d4b2501013b3456656f378b906cd04605fb05ce82c20ce44203 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_unknown d131cc6399713d078cc8f6c6592408bfc9a02a5d0640fdf0b67560e7d6eaa4b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_weights b9d398cffcba161103179636454f46970fa10ef64f84506992b8f8574f14488b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ChannelsIn 82872fb29be4de352199fd7082a86d708a5559affdb1e8ab44125e3a913436c7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ChannelsOut d7d2ffdc4a4de62748748d58e1ee9b1ea90c9a72168fd99a8e7b923154673619 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ConversionPath ae20a940d2f340ba71c54cfb6e75fff4f70d31da107f5070d52ea52d438e043d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Format 73021555e88b2fda7e96c34157813f1b332eda9198b4f694c3d6bd843d0addbc -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.MixingMode 2b3a8674eeaa83b17c34b937027d318b07c5ec8bba8b4e919cc37d2b4738c63c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.OwnsHeap 19b67635d3372387c1b64bafae7b60f8e4472d23c1239701d970b6e6124b2756 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PChannelMapIn 8c2993c2f82601804d88c24b2a959a011f842cfee160653b952eb172f9cd9d12 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PChannelMapOut f65a3ab0d9992da5412af307f3cd7286e308633b8b503d739d52bbb6a0335b36 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PHeap b74e34908494713cbde41b2ac586e1dde557abfdae7dfc4622d2d7639457e1ed -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PShuffleTable 8d9ea99c98d0e5caab5207086532b12226f2fccbc2c8fd3209534576f871b6db -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Weights 39353e82b003b89b55261f80d1ecdc6ea10ffef1bc3b2036d3fe3cf0e0e3b860 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterAnonymousUnion7025e92e25c444e2.F32 acff296eeec885f66f2858263eac87ff2777574a601a10d7e34e85f67bdba6f4 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterAnonymousUnion7025e92e25c444e2.S16 78bfbfbb81f37df42b6eb1ae30f49ff0484d975aa8206bd4ac3594cefb1c86b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.CalculateLfeFromSpatialChannels 9d27071351cc2b5038989f4d0fe21a81eba423c4589e043e1d339a8471ac011f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsIn 5b7a2ca6bf5c96adc3b8ac3a4a2c4c266aed638d63aeeb5da4dd50928fd8b698 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsOut 2e5f5806139f61e6fb91f9d7f85dfd5f6e53a3355b26231b5d48e8d692c3f0c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.Format b4a952498fa582c711fe8a11fd299aa6e47809815e098c14e4b3279f1f04b6d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.MixingMode 88482c2d09642602d158e6f5b63ccfb15d2f0a85ff2803ac7c5b50fca4bdcb68 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapIn 2191d1bf4315a97278f6f4b6b07b47f59e496f256684f4baa8ff8a5dbd5ecf0a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapOut 076ab55238f05d66a5d7fcb3817f9e692d7222a2f9cd00fbbbd1127e0671db2a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PpWeights db755ab9d6b4656d9d2eb46c37ff3fcd1528ea56226909a43d7914849bc9bbe7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_custom_weights af7a172c1085456b16cc30402ff1a07a1fe9db25651e8e98af917be18aab358c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_default 5b3b654fdbb80fdc6fcf8117d877497fa9ed074f7ad021d3115c19c19ad17eda -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_rectangular 2d14a0040c7309dd9b1c686776e69201161ba541c1f95c09de817ceb516388c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_simple 86fbed411ed65a641131fde0649c5b1692d1bd62c5f21d09efc573c8ca7b411a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_0 e8d3463f3d0010741379012b929f0d6f49aaf9e77f9860cb9e22ce26ef6d4821 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_1 0280aa4adf8cb8179d5ea2ddb71f860b4bd87e5b1267a16694f61be04ef9d191 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_10 d3ffb6e6f85e4d95534c8ca7abd88e5a674f4a4e77cd99ca1266e01b0e8a977b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_11 2bcbf10b094a9a16432fa944e4bdc785f67fad68bfd5bd5709199f61b9f34024 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_12 043e72e938e211ae97852278a24258600170aaf2ccf781369f25dd3949072fa9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_13 d46f3a97131a3f5c7d4de1bb958f9a96c4dcfce06480405a3e1ceec1a0a1bd6f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_14 31fc2970b95749d98c515361c0d12faca27cca055368181acf59540e4dcb70f7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_15 f86bfd5d82c33cc69dc3607bef550ff9308b929bec46b3b7b3b088d0ad70ba10 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_16 619cc8d1c44d64c4073404b894bc9afcfb0298b8118137677465e2501574dc80 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_17 a0d20b67b496bb66533295ca9e3f17ae46343ce2964f3188a351d8453ab5f69e -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_18 69d6ef9443d86ff9690ddcb917c8a24b81f947cbe5a8959fa390d86a3c4c664d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_19 622f8436139a8672e921f5636162933fa215d01e0829110d924d4c2e45872abb -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_2 125a4c08ede3e993ab1c937384e0eb25f418b5b47e1f4122d3174318e9f9216c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_20 f247f3d2cfdd4774c998ccd7aa1adccf81d0337279738eed3abc695d694b1e20 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_21 05acd2dcbeeb81bf6f272d3b00258cb06a53d73cf836a0255aa49ea4a4507684 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_22 a88433d0aa9bde3424930039636117b022f0d8d7bd8d2f65c9db4b5416325e7e -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_23 e7ff77257e7e0c0c4ffbe8d458481a5646d2e28adc9fd4c918fa8d2c30d6c2e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_24 0b28c878afeca88c601cbacd3c154b8ff9b55d97d70c8942f237eee77a6731fb -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_25 3e273fc9b1f50d9bae36ced07848d960f3aa4fe6a419cffba028e43b98391911 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_26 1799903c4cf4839b71b2c2b66f24f65d089e1fee1f5572f887109dbc32d014ca -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_27 2d4f1865c889d47fd9d75a05aaadc3f66e842254c9e52c4ff1acee639d24ad2f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_28 4357a7074442b2faa0f8eccd675db444d5879bde2638ae3c4ee4a65093e255ee -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_29 fb83c2e744d9801754a1840de4fab36b4e1e570aacefcfacc254247fc0e3ed95 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_3 b2a9b1f1496a722bae9630eaf3666b1b220490f2865dd4b3c2c960b100082aeb -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_30 dbca0352f04b50dd96652a3a8dddec7744a469bbe59d8f7d6883b15664235c57 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_31 8134388d29cc3b5e60887a9348de1867aacf24e29abeeb5fbbb5cd484fba0f54 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_4 3f405e60cd2cda633798da5297b804ffb8544595761520cdd576761361cb02a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_5 73f7eb3775e9c57d16e08800e20159f0f1a8cf50f3e84580d495e2d27b5c436e -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_6 4b3d22f88b8fdca67833fad9792f3b6d358d7b11b26ea17a5e8b0d36f0befae9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_7 a3c922d25d726d1460db55957c52607b8086b6cd7cc2ea3d7d957b806ae7766e -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_8 dc4e500c4277f6d45572ee55eedd84c48f6023ec18af04814c4882ffd9f291a5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_9 8f39c2578c955f70d95452f11a3e72415ef6331811bb8ab4f6a1c87b458b21dd -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_CENTER e14238493158448a08a88c66d6dbefa5ec93af60c07a243fbabd58945615f018 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_LEFT 6a286f03b40295a4ab11fdbdafbed1a2e06b1305ef8c4f6d7902029dfc897502 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_RIGHT 09fb075b32a0c3ae29f89e04acaa69a351c9bdfdad75afc0017128efb2b86490 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_CENTER 6c1bbd3414026da529dd9cacdedbee082228f7a15ad71cd5040ba2e83c484ae6 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_LEFT 8d5e983c501310d8ab9fcce8015a0e31d0e6381b0ea482b0f8ccc462e0fe573d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_LEFT_CENTER 102ee82fb31c2a5a98778238cacb01fe9276affdfb7c5017a98983b15bb5b20d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_RIGHT 2a4439b0995c6ae89d79ed97a54b09bb382f857490808ddd3ecf014984e48b64 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_RIGHT_CENTER 9a72e7560d4450aedbd3f54ffc1d9520a233bd0b8cc57652b83ad1738433bdd9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_LEFT c743e7a0e27cedbb70d01dbda9edeeec2c3093383b6e29411d759c6153f5efc5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_LFE 82d8fd43502806f723951fac158b8d1d36f21f453dc072e3a8aa881652d679e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_MONO ce5581472d1165edf42aa97262a8627b3d7a939428a5adece768c429d0196581 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_NONE c6fbe26bbbbe854a98e73b599ba16fe4f195fc39a69efd58747d5d169944106b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_POSITION_COUNT 31f2163432a7fcdb8d9cabf29f54a9fa1393f60c97e48a4da9791897d7dcdae9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_RIGHT bfb302264575629e5f49ec33c5f616c1d47e4450c93a99206cef32e5a9b9082c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_SIDE_LEFT 285b4d93b4ff22612c323be12b21218376b431882cd9268dc66fcc437b34fd61 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_SIDE_RIGHT 25442da4b3a01537d5083ced6333c2985ce3e548b45a1a071d6968c526f8b5d1 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_CENTER 2d8a2a7f99af093e90fa3d7a22e1a0184c8155ae768ebef47b3c2eb3c67aca63 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_LEFT d2368b27b8059ad2949fb5d901670033a10aba1843e5bc985b196824432ff00c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_RIGHT 3ed72c926240a320bb18df4fa1dbe5d73bb7e785cad09e1190068e2444dc2018 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_CENTER 2807e532fd408451e36a98c945ab1052fc4cd6abf23c8a23da6a5e3461039f07 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_CENTER fa9a8ad3ab12c3e101a5d56d67bb7954672df6e643df510b26e63de233f52c35 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_LEFT 187cbc3b672fde3ca5038cae8015e337e8e2b95c4785b5ff27cbfb973a51df97 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_RIGHT e45f9c2298fbab895755d8b8751fbe20cc251185ad4d21a357ab02ae59718231 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AllocationCallbacks 0c285d309480063849621f998273bfdc375a595e2ba23c166a7b3750d0ee2296 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AnonymousField14 a6607d31bb1dac8bf2d3482639b0d0d7da3d390919e9e899f2b69b4c7b636d08 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AnonymousField15 1a9fe880502851cd6fe2359742f11eeb29de05599388a4aa8ccd12100cae75ec -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Backend 18b540034cdd2268a54aff1288ad7152ae06f464f05c005f33a11f557fcb7fa5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Callbacks 28b509fcaab92616a9d6077466007adf3c0154e0288676bfe5e37d930202aeb3 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.CaptureDeviceInfoCount 4f8adcdc3fd692159788bf17b6edcd611441424aecc9341623790a3d729fc666 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceEnumLock 6feca4944ab16ea9639b189becc8be8ebdc03e027855675fd5d040697f5131dd -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceInfoCapacity 3e2586af0852863b9ecaeb732e4dc6771bf7fd6dfb281664a7a8db97dea47330 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceInfoLock 4b71c0e4136a52de6b1af1885d52c253987adf777bb619ff2579c64efd0129b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Log 6eae8db7fee112396418f07b8106308ca1a8915531b3d9818ed3be16908c049d -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PDeviceInfos 406b269548a05bda49b52a1c71e68c9d6fcae28969a48d73dd59b419619e90e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PLog 3875f128f5f53d12ed6c2b6b402a802bcc293646825944d772c6c9be13d50139 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PUserData 3bf7a5ed163d8e02efd477d7264651f969f3b6844392500c05e827211f6ceca4 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PlaybackDeviceInfoCount 63b2c34d046d82ce2d32ab6b46234a15dc9b70e4c221c979c2668c16ad3acb70 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.ThreadPriority be41aae1c7bd271fef8cbc14d3ac1fca57294be62546f252a8b58fd165a974c0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.ThreadStackSize e9c539661d71df0a3efd5bef8193f0f49ad9d00c25e76f566bb421aec51dc97d -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackActivate 797b5097e4947436cc39ae332b33e785fdbc633d6fa59a4b1f84859aaef400bc -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientClose d33fd8c884ff8aac63d01b765b147a97fc53295822bfd76457a374990f281a45 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientNameSize 55ed2c8c49e25af048d80bbe354b19eb6be2c8d0be061c91ad69f6b5e4e3e8f1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientOpen eb162d7d53f248caa313c4f4ab99bcbdb9529527c7000a6d9c7b5b3d89776049 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackConnect 884db5200cae3aa869ba429eeff67875784ace9ca1a79b1c7ad1c71b2a6dfa2a -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackDeactivate 8ab43313dba7edeacae6595216f61c6cab2a4a9d88983a7b03b49140d5667aa7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackFree 4f9a709b22fd1c3480755e29c3b7b057d1fe13057319e54fe86ea3863cad6c00 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetBufferSize c0c8cdb41a9e78f7daff97c64c376f50d3628d18fe2dd143067759a7ef477587 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetPorts 837923cfb0e5c3912dcdcea999fc0eb30effe8aaf956f247b24d36c860b7079f -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetSampleRate 858a1b48dd30a3466f73023abfd4e33777211efcd62a7a4072bc914b3fa91d2a -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackOnShutdown 74d64794faafc33259a6b84b6b4e67d8275b5b4a3b3f3c028a18011c3844e430 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortGetBuffer 33892010f671b272662654d13d257a4e322aa8bc8105d6f64f9eca5a1b8ea8df -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortName de2e51dd740dc202747b5f4e3189bd6fa51f9f625bbd3c870c05d82548d1a57e -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortRegister 750c5e4935ea5938c220f6da9387b8715e2436ea9fbe423acaf10c9601d63a4c -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSetBufferSizeCallback a54bfb10fa08684125524aa07c5699751141e2acbb52e83c5bd5695cf6248eb8 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSetProcessCallback 00be9a094363c761aba0021bb9349fcdfe3a6ec67c2778fdf5633cfda99c3c2d -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSo 7151dc453b47f406de2654066a82b0eca05aaeef33989a39fde41b16b79b3ea3 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.PClientName 980fc9edce04b383e02c872aae927c22e69a225c75c85515547f15ba90cae0e9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.TryStartServer 480c75aedb0319aedb7a219e44737477fbbab4fac0e612d7806152b769c7ef30 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoCreateInstance 2e96291849ddf188628b46480e6fc5e98f185df829127374fd6ef70083cda7de -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitialize ba1096a9db9b882901ad03b24aebdcf516ab9213dad0726b8fef44ec0be4fa39 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitializeEx 426302825946694c1af5682e5f9c00a630f210a22dc598f79cb1e6c37b2f1a6b -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitializeResult 42cf3bf5664689f39f210968858c6a3d036273b08fb03650047d627b657252ed -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoTaskMemFree baa6c8ebb7b3a56ecaee5fbedf5d38b3fb4b58978350d2a563d317551e11ecc1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoUninitialize 5d1dbd7ee35e7a7f6e1beeac2b8f75e7a602206e6147abfb1e5ddd2ad57f7ba5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.GetDesktopWindow aa245b5b65459197a523dd2f71b436f6bf8f4ce1c011509f00251ef3e32903c1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.GetForegroundWindow 33b8bd4f262a537b07477e7d104fe2eccb846d4802248ce3c589c57a48e0ea21 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HAdvapi32Dll 8e6f5fc55fe16014778e5d3fa789ec65b220f3fa14b7d5e0202f09d8cafc3f5b -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HOle32Dll 2fc380941abf99a6f8f090a17411b969a977e4496b98a43e3088ef2059557942 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HUser32Dll 871aa8d4be40f5a420ea4d4bac3f78650291231df1eed90a886198411513f148 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.PropVariantClear 02aa91ad7b6483fea9b38467b1d36a35679e64a45d9804e95d399b6422d05ab9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegCloseKey 290b40ef2e92bd202af156fd92b18b86b1bc2cef4f289e21d01518603f69311a -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegOpenKeyExA 01cef7151bb35d026a2da08d4539ac76a245e2719ebc5cd1997f321ad2c7511e -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegQueryValueExA ad8d2e2fd7c5c399c00f2c09186a054f7760bfc7623a0bce666f38ca1617bafc -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.StringFromGuid2 3f068d2d594b8c7b0d458be379a8d6f5d813f37227dea8d0f7b03477598d42d5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct5fae19458e2803d4.Unused 4e9c712b01e3ba8943e54e969ec9b67bea675b2d693353e264fae66be49da64d -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.ActivateAudioInterfaceAsync 28d1a92ab315a70f8d0359f4aa16a4c46ba98f85b6eef5e005fdd2f80dfcb568 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.AvRevertMmThreadcharacteristics ca0082389b90ced4c6facb97d1fe39f65ab5fb0ab63246ef2c9fdb80016aa486 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.AvSetMmThreadCharacteristicsA 49d8c07c9fc20201ac77e55a3e42ff1ade8812376ce82dbff36060e20887ab53 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandCount bf2a99a7da9af494831627fc02e00bf5a167d4670dd84a440474c3a024c80eb7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandIndex 0132540d34e40414ea49d27f6f996f61d4e925420b6f7457cb6a7ce8969921f1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandLock fb073c193a4f76a4422d7a9c5a959063d10cc5d6e94762cb014323879f7f4d2e -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandSem fd08a44227459e62f39d2883f55089558f2da6d8493f0d0ed7c504e03905da46 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandThread 70b4a29a526ced0370dd979e39a7a07f8cce9a24c667af24a5946e7db3dcd833 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.Commands 64f51ffd8e7e9886c27b6a21b798579f571f69295a1a378331e26b421c11ebfd -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.HAvrt 4eb7f15169aee1567c1889e3237508fdbe5e136d6d1eccdbbceac47c2d8588b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.HMmDevapi 3c84b82d8e3cae6360897af2c95326ac5bfa8f679f35c29b67c82c0c7ce0eb82 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-577b14d7b4ec880b"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_passthrough = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17411) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f68afef6237583da"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_shuffle = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17414) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1fa881513fa1f73"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_unknown = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17410) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-71665f5946190e65"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_weights = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2cba1118021b816"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17431) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-403167225e6c400a"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-053d9c87782fc877"></a>[`public Inno.Native.MiniAudio.MaChannelConversionPath Inno.Native.MiniAudio.MaChannelConverter.ConversionPath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17437) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3897f31ce2c91ac8"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverter.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-181b3fe145a33673"></a>[`public Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverter.MixingMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17435) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d32e0539f5eed32"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17449) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-304cc3054e1d1319"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17439) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e72b3dd4f47d27ec"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17441) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db6fc4931e6490cd"></a>[`public void* Inno.Native.MiniAudio.MaChannelConverter.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17447) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e72a5d133444a90"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PShuffleTable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e50acfa353b8964e"></a>[`public Inno.Native.MiniAudio.MaChannelConverterAnonymousUnion7025e92e25c444e2 Inno.Native.MiniAudio.MaChannelConverter.Weights`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-434f159111f70206"></a>[`public float** Inno.Native.MiniAudio.MaChannelConverterAnonymousUnion7025e92e25c444e2.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17470) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-07255e88df988e81"></a>[`public int** Inno.Native.MiniAudio.MaChannelConverterAnonymousUnion7025e92e25c444e2.S16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17472) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5349bfbedd340b7"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.CalculateLfeFromSpatialChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17505) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4fd9bac4d7dbb4c5"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17495) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dc6f7c20c17e32a6"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17497) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa83b1808418278a"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverterConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17493) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f8a527725bcfe910"></a>[`public Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverterConfig.MixingMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17503) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1433e65a57125d7"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17499) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b78a222875a06125"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17501) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d52831761392d25a"></a>[`public float** Inno.Native.MiniAudio.MaChannelConverterConfig.PpWeights`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17507) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d35a11c5b5f0a44b"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_custom_weights = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17527) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a6b7d4f44fbbcf7f"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17528) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5fdd621e2b93f897"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_rectangular = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17525) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9b671a502cdd679"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.ma_channel_mix_mode_simple = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17526) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e589ee8354071ad1"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_0 = 20`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17559) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c924fa74a81749ea"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_1 = 21`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c0e498deb824d12"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_10 = 30`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17569) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64457823e38258b2"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_11 = 31`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17570) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f9bdbe2f3df298e9"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_12 = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17571) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7f960ea445d35ef"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_13 = 33`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17572) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5eecd11714f7dc58"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_14 = 34`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17573) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62873d2cdbb263c2"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_15 = 35`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17574) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-889da12d9b97cbb9"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_16 = 36`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17575) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e058c3b5307e535e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_17 = 37`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17576) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-43cfc0209ba77b91"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_18 = 38`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17577) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c8380c20c581bd6"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_19 = 39`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17578) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd7b24dae446ecf9"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_2 = 22`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17561) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75caa6ba50a57aa7"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_20 = 40`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17579) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9622281fdce21afa"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_21 = 41`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c095f7ce49869311"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_22 = 42`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7058038a813e4959"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_23 = 43`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-58276a8ec411fda1"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_24 = 44`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17583) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a56d9b8d4c2286e7"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_25 = 45`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-131f299b7ef9af68"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_26 = 46`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fbe6e6069d44566c"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_27 = 47`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-224f7072fa6e5224"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_28 = 48`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17587) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4402177bd74eac7a"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_29 = 49`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17588) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-306177d04806ae55"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_3 = 23`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-26a036de8bf004c4"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_30 = 50`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17589) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-caa18fcf14606933"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_31 = 51`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17590) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-754e17a543cc82ea"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_4 = 24`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17563) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b96a6c92df27b55e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_5 = 25`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17564) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d48d99334db42c6"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_6 = 26`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17565) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e20e98e1d249e4c2"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_7 = 27`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17566) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff869e5b8d1c90f3"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_8 = 28`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17567) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-617da82ddad01177"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_AUX_9 = 29`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17568) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c2389d90495cab63"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_CENTER = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17549) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fecb0a0826a68ff5"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_LEFT = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17545) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75aea58d99214eca"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_BACK_RIGHT = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17546) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8d678f4c0b232b5d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_CENTER = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17543) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6a1be9a8d88e80fd"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_LEFT = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17541) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dac6cf40ca91d651"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_LEFT_CENTER = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17547) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf68110cd94f577d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_RIGHT = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17542) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8678d0f710ae51c6"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_FRONT_RIGHT_CENTER = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17548) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6629195e905a2e21"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_LEFT = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17592) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af5d805f0687e81d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_LFE = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17544) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99bf4a83d7d3d41e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_MONO = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17540) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3ae7439fdfc90be"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_NONE = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17539) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-600f35ee3864232e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_POSITION_COUNT = 52`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17591) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2fda13711c22a39"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_RIGHT = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17593) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-97fa4cf985a66b97"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_SIDE_LEFT = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-07d9722f1ffd7201"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_SIDE_RIGHT = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17551) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-552e32c508543d9e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_CENTER = 18`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17557) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2589db9da0cbe5c"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_LEFT = 17`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8881a15b53c99cdc"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_BACK_RIGHT = 19`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17558) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3ad93e429f58545"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_CENTER = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c85687a03d70706"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_CENTER = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17554) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eeff3118e148bcac"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_LEFT = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17553) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09b5582c139b3436"></a>[`Inno.Native.MiniAudio.MaChannelPosition.MA_CHANNEL_TOP_FRONT_RIGHT = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17555) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-153d7ae4a2fe9b53"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaContext.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17621) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ef189974e26a81f"></a>[`public Inno.Native.MiniAudio.MaContextAnonymousUnion3dfceaecfb81c0d8 Inno.Native.MiniAudio.MaContext.AnonymousField14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1aa7e3abc776daf"></a>[`public Inno.Native.MiniAudio.MaContextAnonymousUnionBedbc270a69d8c34 Inno.Native.MiniAudio.MaContext.AnonymousField15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17637) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a34e1c7f249fe3d0"></a>[`public Inno.Native.MiniAudio.MaBackend Inno.Native.MiniAudio.MaContext.Backend`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17609) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8add20e77b255fcc"></a>[`public Inno.Native.MiniAudio.MaBackendCallbacks Inno.Native.MiniAudio.MaContext.Callbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17607) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ebe31d572abc28c"></a>[`public uint Inno.Native.MiniAudio.MaContext.CaptureDeviceInfoCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17631) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2cadc9f9ca4f2efc"></a>[`public void* Inno.Native.MiniAudio.MaContext.DeviceEnumLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17623) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cdd827aa112763ce"></a>[`public uint Inno.Native.MiniAudio.MaContext.DeviceInfoCapacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17627) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-422b8b209a649872"></a>[`public void* Inno.Native.MiniAudio.MaContext.DeviceInfoLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17625) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5624a544f17921be"></a>[`public Inno.Native.MiniAudio.MaLog Inno.Native.MiniAudio.MaContext.Log`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17613) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a86f8f5a69f67a63"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo* Inno.Native.MiniAudio.MaContext.PDeviceInfos`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17633) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d5d303d279b0be10"></a>[`public Inno.Native.MiniAudio.MaLog* Inno.Native.MiniAudio.MaContext.PLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17611) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44f4a4e060cfb88c"></a>[`public void* Inno.Native.MiniAudio.MaContext.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17619) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e52180873f637195"></a>[`public uint Inno.Native.MiniAudio.MaContext.PlaybackDeviceInfoCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17629) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e818be270ae0a1c6"></a>[`public Inno.Native.MiniAudio.MaThreadPriority Inno.Native.MiniAudio.MaContext.ThreadPriority`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17615) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-233e9ce8167b1368"></a>[`public ulong Inno.Native.MiniAudio.MaContext.ThreadStackSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17617) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f8719e3e6fa80fd2"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackActivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17678) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a6aeb4be6ce2190f"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientClose`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-721f2dee4946580e"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientNameSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17664) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fce679f14bdafdbc"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackClientOpen`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17660) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d4679871aed9802d"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackConnect`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17682) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b22bb83cf32fb4e6"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackDeactivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17680) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1180f8ad1a9467a6"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17690) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6acc207861536e84"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetBufferSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17674) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-96e6b0b3c9f64c1c"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetPorts`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f18e36529952db2"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackGetSampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17672) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-620febdca4b712c2"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackOnShutdown`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17670) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e13448112f602b78"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortGetBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17688) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77b441226bd3da16"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17686) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9af48c6dd4ec79d0"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackPortRegister`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17684) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-870e96789b680fac"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSetBufferSizeCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17668) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d11a005392a1d08"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSetProcessCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17666) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-321430f7376b80c5"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.JackSo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17658) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-899157907be39b80"></a>[`public sbyte* Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.PClientName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17692) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3c18517a70466b4"></a>[`public uint Inno.Native.MiniAudio.MaContextAnonymousStruct37d23b63c3c13156.TryStartServer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e90b32d6e81bd606"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoCreateInstance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17723) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ba8323302caaae5"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitialize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17717) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72f74965791b86f9"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitializeEx`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17719) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac008c2f5927a68e"></a>[`public int Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoInitializeResult`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17745) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2c6bb4e92cf02fe"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoTaskMemFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17725) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-31d5e304f8629ab9"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.CoUninitialize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17721) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d2bac0eac173e3e"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.GetDesktopWindow`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17735) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-910250e4545b5e85"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.GetForegroundWindow`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17733) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a70ae72ea9908025"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HAdvapi32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17737) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-94fb9720939e3ad8"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HOle32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17715) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-754fb213adb080b8"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.HUser32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17731) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c82e2c114438b3b"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.PropVariantClear`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17727) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8fb08b7d31f188fa"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegCloseKey`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17741) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e1f5ca6ccd5cf7a"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegOpenKeyExA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17739) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b12abe7fe769d84"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.RegQueryValueExA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17743) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72dd7fd29c596c09"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct3f09d48453db1e24.StringFromGuid2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17729) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74cb7628c29a6d30"></a>[`public int Inno.Native.MiniAudio.MaContextAnonymousStruct5fae19458e2803d4.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17766) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c0732b2b9bcf19fb"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.ActivateAudioInterfaceAsync`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17807) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7a7834daa02f2f94"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.AvRevertMmThreadcharacteristics`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17803) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e75d90dccbda13dd"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.AvSetMmThreadCharacteristicsA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17801) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac32405527845263"></a>[`public uint Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17795) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f0907128d5c9393"></a>[`public uint Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17793) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce2149853e07783a"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17789) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-962ced47c74c8243"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandSem`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17791) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b56ad4d2cfa89ba9"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.CommandThread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17787) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88cfdef27dddb82e"></a>[`public Inno.Native.MiniAudio.NativeArray_bfed4867daabc092 Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.Commands`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17797) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c3d254a899acf2e"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.HAvrt`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17799) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7b29e39367d9de2"></a>[`public void* Inno.Native.MiniAudio.MaContextAnonymousStruct778a78bc23645b5d.HMmDevapi`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17805) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
