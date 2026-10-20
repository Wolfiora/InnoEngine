# Inno.Native.MiniAudio ABI · 22

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: T:Inno.Native.MiniAudio.MaJobAnonymousUnion73ca8060d9820b4c 21d85afb7e34772ff187bd7e6f016bba3248479e90cbcfc341d8bfd55708cd20 -->
<!-- api: T:Inno.Native.MiniAudio.MaJobAnonymousUnion85cb70704396c9bd 35c568d5389df1e35712633c651ac6e7c1ca087ea9a9a332f9a6f29f3ad1f861 -->
<!-- api: T:Inno.Native.MiniAudio.MaJobProc c836d26b7c992f74c5275aee0c691a9692b00d5ed423f9dbcdfd7c32b54ec315 -->
<!-- api: T:Inno.Native.MiniAudio.MaJobQueue 2274079910d7ed5c226de5d7a6e7410c2bfd582abd6bb8819bb1727fe24def6a -->
<!-- api: T:Inno.Native.MiniAudio.MaJobQueueConfig 126c4c0932a85e23b504d148d1afab533309b134ad24a293870a093e38402b3d -->
<!-- api: T:Inno.Native.MiniAudio.MaJobQueueFlags ffb3fcc92961427defd7241dbe918bbdb7380d2836e50f0fbf24f1e5df3f7534 -->
<!-- api: T:Inno.Native.MiniAudio.MaJobType a039b311acfcace86e4ca64e0b49b9b4a0404047379f327d9b7997162cdfcf41 -->
<!-- api: T:Inno.Native.MiniAudio.MaLcg 33b57750804abf34cf0b2f2cfdc835acab3628e72a8a179d31311769434208e5 -->
<!-- api: T:Inno.Native.MiniAudio.MaLinearResampler dcf211fe3d8e5fb9d6cc532dc02b8abbcb4774e8000f253272d7f0669bdc8d52 -->
<!-- api: T:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be a7a2df001ce14228826d6a6b14a418e2dcd7c6a2033ad83855061c3111635d2b -->
<!-- api: T:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588 e338a7fdafdea9fb3e1c2d006b416df8e151724677f9a06bc0b05ed2c3500014 -->
<!-- api: T:Inno.Native.MiniAudio.MaLinearResamplerConfig 8a20d46476c517ff90eab0ccdcd05cec4bee0d0c3cea3ab09099c5601e5f9625 -->
<!-- api: T:Inno.Native.MiniAudio.MaLog de3c63f455a6ec130738603a1b6609188538d52aa254c2379d98dff01a3a9363 -->
<!-- api: T:Inno.Native.MiniAudio.MaLogCallback fbd352d4898ff82f4932dd1c53847641d97e03d2053c3044439be7b9f5bf0463 -->
<!-- api: T:Inno.Native.MiniAudio.MaLogCallbackProc 18420f0ac8dab38627a78de734b1c164aa528b4cf77b7f029f11f61ba19a320f -->
<!-- api: T:Inno.Native.MiniAudio.MaLogLevel d7e36f498208d34f920278618e0ba112ce6184782f000e04451342545674a3a4 -->
<!-- api: T:Inno.Native.MiniAudio.MaLoshelf2 5e94b30b0433a985680ffad0b93632134b59fa9adb05b43f5ae679b531052f86 -->
<!-- api: T:Inno.Native.MiniAudio.MaLoshelf2Config aa3d1ad1cf44a2c68be70e0b50fcce4511ceec40db8c421f1cab908f3332de9f -->
<!-- api: T:Inno.Native.MiniAudio.MaLoshelfNode 8163ce86ca7c4a32698a075e8bf7457f4cb121da3ece27ddc0a91e08bb046a1e -->
<!-- api: T:Inno.Native.MiniAudio.MaLoshelfNodeConfig 0a697eb7e520a5bde919f045712fe55fb316ece363404ebcf9bffc90bc0f751f -->
<!-- api: T:Inno.Native.MiniAudio.MaLpf dca0cc124b4795ff56b2a10e39d2cbb9d69dffd9d7dcace14c0fe059bbbf3f8a -->
<!-- api: T:Inno.Native.MiniAudio.MaLpf1 0e2e2d408c831287fdfe41571747419c777f9ce411c55fcdd91d16ca9aa1dc83 -->
<!-- api: T:Inno.Native.MiniAudio.MaLpf1Config dac8b7e74f581b6ba68e29495e829e37538b18cb27c933d43caaace4a8c16635 -->
<!-- api: T:Inno.Native.MiniAudio.MaLpf2 df6df2caada18c7976a7888889d2f2b895d0ba3dab6e6e103039c88d5d9a04d6 -->
<!-- api: T:Inno.Native.MiniAudio.MaLpfConfig c2652a5a8fba362896fd3ac65d33284e9085429a25a5ae9150a27a1a0ce4e27a -->
<!-- api: T:Inno.Native.MiniAudio.MaLpfNode 0c8e71a3f5dcc37014ee8e089f2d942429c3dff41f53507cf347ee807bfe24d3 -->
<!-- api: T:Inno.Native.MiniAudio.MaLpfNodeConfig 50459298eab4de9e33b664aa3428f47224cddea8856f85993798570199e0e909 -->
<!-- api: T:Inno.Native.MiniAudio.MaMonoExpansionMode 4153b7291f5d1d89e36ef74ca8dd61882e0b0b681f45e666828f4613568c7ae5 -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeBase 2bda87174fe935741958249b31ed204f28a912436a8819684c660520c89536df -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeConfig d5297d1c2468ebc07ecd8a79ba4738f0582747fc73577e5a5d02ce5b6b622d8e -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeFlags 1113dfd4ee9ffa90a0bfb38448e8d4a7d8d1f2d9a263ed6c61fb29b1509da059 -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeGraph 6eec49ae326946d5391167ab77665af933b862767e1a94b5e65f22ef137c46a8 -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeGraphConfig b20dce2f820b5c18c23a832e5925657236824363c4d74b3d66477667e8bd5bb6 -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeInputBus f7981571f2df9a4823c325120d2ba76d4e99389b455d21f46cebb891baa6a21e -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeOutputBus 52e3c9b55482cc091eb925167a4807428ba9bdd7aa6fa5ae89f347519654268e -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeState 6f7d4d733dedeba3ed24291d3a4bef991b5438355c4d98d77171e92d77b8cfc1 -->
<!-- api: T:Inno.Native.MiniAudio.MaNodeVtable cdb8b84d8c652019df3b9fe72616b37496b404fd19a8466740b68c8ea0ef20a9 -->
<!-- api: T:Inno.Native.MiniAudio.MaNoise 6b8af565beac9941d46cb2b37d9adc3f6ce8ebefdbd05119fdd8b6c30146725f -->
<!-- api: T:Inno.Native.MiniAudio.MaNoiseAnonymousStruct150b5b8daffaf7dd 024ae46f67ffd715c6f54a8a3e11889e274fa649b5f1a521d22cdef6c9a56c6b -->
<!-- api: T:Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8 84fcca5adaf69f83bd517efbd3b181cce74308e5262fa4788f803005d858bd53 -->
<!-- api: T:Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c 7e31f86d7b8d9a97e80be8c9d20a7ae11dc71e340d92a052507b3f3b9e37c303 -->
<!-- api: T:Inno.Native.MiniAudio.MaNoiseConfig 2b3af4960f8f79baedd84beeee35547c8a2740dc601cdebe08b5be3fa68dcfef -->
<!-- api: T:Inno.Native.MiniAudio.MaNoiseType 64f0a5b08934b4ab43d99fb16d88ad9c6128aa7b98bd0fa981cba51e5ecc2fbb -->
<!-- api: T:Inno.Native.MiniAudio.MaNotch2 052f48108a45f131b7edfd7b0560772b80c2731efdec41c9551db2b02ea4fe2a -->
<!-- api: T:Inno.Native.MiniAudio.MaNotch2Config f8ae116bb1dac8f89c2aba8067d63e337c74f38ff08ea31b3a183ed03f0e44fb -->
<!-- api: T:Inno.Native.MiniAudio.MaNotchNode d49514b542b86120a753aead7a3c66ccb96a74764a7600a0558b16fe0e714d7a -->
<!-- api: T:Inno.Native.MiniAudio.MaNotchNodeConfig 3e1e21e5f2d90fe441296014604aa263768ce5a389102e10e8ac45c535140268 -->
<!-- api: T:Inno.Native.MiniAudio.MaOpenModeFlags 112b2210570f83532209af5e8a8a77801d4c8e57eec438cbb8734783c068ba66 -->
<!-- api: T:Inno.Native.MiniAudio.MaOpenslRecordingPreset 186e3c600f949cef721225828760c4b9127942d8303643dcb155efe1afc5fd7d -->
<!-- api: T:Inno.Native.MiniAudio.MaOpenslStreamType 5b29fdf468fdb29ebb951bf2fb1da8e3e8183c6a62d2e877d987ebb69bf71263 -->
<!-- api: T:Inno.Native.MiniAudio.MaPagedAudioBuffer 74ee603312ffe507f645a38fba0b48401a076284471d0207608b2a8b83eb3b19 -->
<!-- api: T:Inno.Native.MiniAudio.MaPagedAudioBufferConfig dc26a71eda5e6834f70b1865d5c185c71d54dc4a800d30e3f19ed6acb75cf931 -->
<!-- api: T:Inno.Native.MiniAudio.MaPagedAudioBufferData 1b5d94eab060008f61dd9b8b00e81fb038f63a04b797dcc04ae0fd67c995e2f7 -->
<!-- api: T:Inno.Native.MiniAudio.MaPagedAudioBufferPage d2bed49dcd300fe0698e52276b21162e158b502f57e89ba1ccebb1c6a2070a32 -->
<!-- api: T:Inno.Native.MiniAudio.MaPanMode c6fafda174a55c67db7b7c83cacd9ec57b59b686267008cd2514d7a70c500c8f -->
<!-- api: T:Inno.Native.MiniAudio.MaPanner 671c0e95fcad3d5f51de2aace74ed9099f97782342637180f52ea7f39975243c -->
<!-- api: T:Inno.Native.MiniAudio.MaPannerConfig 583a5a478f22bc8eeb9db6f00f6f5657347758d9c2baa17c25dec1773e3ccab2 -->
<!-- api: T:Inno.Native.MiniAudio.MaPcmRb f7a7232af8257fc7e73909c468836193dd81ca9c123e847b4268b8ec66086da8 -->
<!-- api: T:Inno.Native.MiniAudio.MaPeak2 19136e8151fee98e3f4756dbf2009bed5089fb152f336e3e41443f8bce2afd2b -->
<!-- api: T:Inno.Native.MiniAudio.MaPeak2Config 1828f016adc97134fb0fc6ee8b87193ed0deba209d0c4b99842abc6ac9c400d9 -->
<!-- api: T:Inno.Native.MiniAudio.MaPeakNode 1409f9d15ec953de54732364cdc61e2f47286d631b36b9438e2988c4b3f9e210 -->
<!-- api: T:Inno.Native.MiniAudio.MaPeakNodeConfig e7bf91cd055d4ec12adef56a375bb3d088e4d63d46572f8ab5a14c417a2dfbe8 -->
<!-- api: T:Inno.Native.MiniAudio.MaPerformanceProfile 48be9eefb77ba380c10e9230f854b9a95c21f9c643c9e49731b01e765f1b482c -->
<!-- api: T:Inno.Native.MiniAudio.MaPositioning 7c0b71c5c32b31c97482f541a0894385ee654cfe5b96aa019d42a39b74939ff2 -->
<!-- api: T:Inno.Native.MiniAudio.MaPulsewave 10628029fe0280916514cf230f6935b7cf50662076d918dc404e66254a13b8ca -->
<!-- api: T:Inno.Native.MiniAudio.MaPulsewaveConfig 702c68c9c37dbd546182b406082e8ebfb9d90cc8cb5a385c379033f0ec5d8356 -->
<!-- api: T:Inno.Native.MiniAudio.MaRb e75f38e9974397707a2a472b57328ceb247b75ff0010061f7b8dd939482ad5e4 -->
<!-- api: T:Inno.Native.MiniAudio.MaReadProc 818ddc2717e2ac9615a7779253d3c579817272496bba528b5f23b35050522bf9 -->
<!-- api: T:Inno.Native.MiniAudio.MaResampleAlgorithm ff0b5905fd931c31d36bab7ffb097f533f0929f192c85426689431025affa295 -->
<!-- api: T:Inno.Native.MiniAudio.MaResampler 1cb7a9f5394e7ca91de615e386e0e98444e0d7e9f68d0148b19cd3db0a93d30b -->
<!-- api: T:Inno.Native.MiniAudio.MaResamplerAnonymousUnion67c9af061924ab62 96966e90bfb0d5cbd9da75d84f19b69ec07fcc376b33da4a85c986ac94d20436 -->
<!-- api: T:Inno.Native.MiniAudio.MaResamplerConfig 87d96d62e1638baf4992cb432f52919a14a146582f69e4546b3b336690fff2ac -->
<!-- api: T:Inno.Native.MiniAudio.MaResamplerConfigAnonymousStruct816361d296299c5c cf054b74269e50e5a72becf1636b91f0e3740ca29359529afeb6f2511d5986f9 -->
<!-- api: T:Inno.Native.MiniAudio.MaResamplingBackendVtable 503842a9c71141de8e8145ea932e06d28ef207e3184a87996fbf26cefba28a4d -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManager 1c42c067ab37f7e1f9b8a5dc0cc2b63491c0c5cd7ee4a64df53be4a73984da5b -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerConfig f318d49bd3315cdccb3ef1d17ed991fb252fee7b595db606057ccdc2f531750d -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataBuffer ce14fb5f05b95cb00b168f2669087319dfe136642c7e4576f20cf8e04f440d8a -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataBufferAnonymousUnion9072cdd7c6d42b32 66eae29994b019098700ed0b0af7147fd35ff86907da726484f697b2a42b2ce0 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataBufferNode 8cb6a70e7091936b0275a2c3770e8faef8680c1e2ba1327e35ba85d969119dd2 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSource 6dec3b4fdc9a6d994b9990d201e9104da3b9f6df6f7d902d9e5cfe5260d78d17 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSourceAnonymousUnion88c267d2dde03905 e1b94c223a8adccb35c5a1e552811ea5b917076aa9725474bbaff4d0eafdc2ee -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSourceConfig 3f8755892f13f2ac640c24785fed098b7ca8c783e450ec696c75b5cc9797d1f1 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSourceFlags ccac5c30be24361d5a7e53dbbe7567466734b26597d6f436f511499be342aea4 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataStream 48369b97cb83704aa69fc96358ff0fee2a3e2f6d4132b91c0fd7e5466f14ee93 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply 1f54fe7d831a473f13d7e8980986b580f0a9df0d705820f36ed2f94ddbffd09d -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStruct86905fc328b96f21 528c3f4322370b4879fe597074e343d8ae5e2b6b94db6ff646fcc5bc890925db -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStructB49f272bd36a3013 7cabb26de7e1e4cfb9f8098c64d1f1d22a0b3a48a5074932526d3d0beac73abf -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStructE4f5f54148be4e4d 286f76c1a05f82e6aa7034e6383e45def5fc5fae422d5d239400f7a684f2ac37 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousUnion7b7c4bd0caacafb5 02eb825ce42de7366f5afd11d600fd70ff987d5e27e146c8c906901907df4ffd -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType 5dbc677d88ab4fd2890971453b5a45142e711c18a6c07491bcc0d96fb0f4e16b -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerFlags 6afb3db39fd64cc99250d90d7440f9324640ef3f266fe089f8d5671b7fc28527 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications 1736821d7f8c06f7fc7f844fbfc1fcc54965ece6df13a83b8f0bf194242d2ec1 -->
<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification 8620c921a9b4be9b49850684d778ad0d502eca95f56170bca4d258799979b290 -->
<!-- api: T:Inno.Native.MiniAudio.MaResult e6a336898190005bc691cb82bcc705caec40111be67aba472d711cd13b300b71 -->
<!-- api: T:Inno.Native.MiniAudio.MaSeekOrigin 5301b9d072afc6ed94c304386e6beb3944cfd7d15d597fe145211b24d4290589 -->
<!-- api: T:Inno.Native.MiniAudio.MaSeekProc 357aafe7661ce2d2efdae8a4d28a92ee8e96422d49d25544e9f8658641ef942b -->
<!-- api: T:Inno.Native.MiniAudio.MaShareMode c1fb0e17d082ca6c97ed866d54c5e5142f28f1d257b42f44faf15739d91b161d -->
<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocator ef7f60dfc2064cfdcd6765b97fb15b5de127c42f21dd9b933874912eb01d1883 -->
<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorConfig 38a4e32e6ee5c9f6d891f7c6046dd87c3a6797c86888a8bc239b3fc1664e02b3 -->
<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorGroup a7a267d788de15ceb3402b858810802c618c709053891a35f4ac114b350e93b1 -->
<!-- api: T:Inno.Native.MiniAudio.MaSound 1b902aca94b5fa85b887e68a1531a307d11d621aef8013dc4145adfe9c112a94 -->
<!-- api: T:Inno.Native.MiniAudio.MaSoundConfig 8efbc3d0ed4c4254ceb5d90cfb9669029f5c91405db18ee46eaf5a5b3679914b -->
<!-- api: T:Inno.Native.MiniAudio.MaSoundEndProc f1450dff51ff08f0ff79e76f60926cbb34f1882d88620941eae16ca31e868323 -->
<!-- api: T:Inno.Native.MiniAudio.MaSoundFlags 6f1377640a9dc74afeef258e7d76cba337cc72886e6cadcc73c87666bbcfc305 -->
<!-- api: T:Inno.Native.MiniAudio.MaSoundInlined a59cc404cbea88211afb4845b438abc3f511e3e6b2320536cbdea1cba70f5b8c -->
<!-- api: T:Inno.Native.MiniAudio.MaSpatializer 30ea53be26f0a8b269853e28d3810647c32d9c4c636aa6d5e38674d4814d3caa -->
<!-- api: T:Inno.Native.MiniAudio.MaSpatializerConfig 223af95a01db556658034b1ce5bd785640eb944339700f5a348ce6bc5fe4acdb -->
<!-- api: T:Inno.Native.MiniAudio.MaSpatializerListener 7c8e78c77cc919855f3a4c756faefe199026526d70218a0bf1cbf8da28dfd6d3 -->
<!-- api: T:Inno.Native.MiniAudio.MaSpatializerListenerConfig b1d09d488873c9866ec446a8bb99841612a8dbb84a24c84d71e8dae4004689da -->
<!-- api: T:Inno.Native.MiniAudio.MaSplitterNode 6b8d35ab7f04abaa9af7fa8ee6e34b609b1260df075f1ae7735c6d7c3b1a91d8 -->
<!-- api: T:Inno.Native.MiniAudio.MaSplitterNodeConfig 659efc802a17a1dcedb1457774b7dc2ae7293f18e1d63219e6a09fc2a50aa4e7 -->
<!-- api: T:Inno.Native.MiniAudio.MaStack c67fec7693ed0e95c932078fe0f40a972d260476f7d425d55fbb4fa453dfb563 -->
<!-- api: T:Inno.Native.MiniAudio.MaStandardChannelMap b71678d981a622276c2738de9a4f5b0581eaea3a6d5cef6e75362fe81389c87b -->
<!-- api: T:Inno.Native.MiniAudio.MaStandardSampleRate fa299136162468910dbf68a05dd89b31719246dddd25e0dfa127a22992e3cc2c -->
<!-- api: T:Inno.Native.MiniAudio.MaStopProc 1f94c714802fc68eeff41dd31b25442fe01f221484c5f2079f4cf22d3bccda40 -->
<!-- api: T:Inno.Native.MiniAudio.MaStreamFormat feb523017b5703e10466f15c4f8f5a873cdfb8a08349e2d9fb40d93c08732746 -->
<!-- api: T:Inno.Native.MiniAudio.MaStreamLayout beef273d2c66d961b5eb5b37eaf96ff942e33e72c6d4156757f22c1c6bc21b41 -->
<!-- api: T:Inno.Native.MiniAudio.MaTellProc 9685dd1dacf4a3a601250963165f40f29787c1ef807d438d065c19295fa1f235 -->
<!-- api: T:Inno.Native.MiniAudio.MaThreadPriority 8e50a72fe03cbc2e31ee51c184f996a8662f7a579c73901689d3a441e0a204be -->
<!-- api: T:Inno.Native.MiniAudio.MaTimer 40f7ef60dbe016eb693b63629430dc6a99eb852ee1204c2080f415f6220b4c94 -->
<!-- api: T:Inno.Native.MiniAudio.MaVec3f feca604a5c99d50f9d2a63f8368d0b239638842af56e5741a6d6e513e797e575 -->
<!-- api: T:Inno.Native.MiniAudio.MaVfsCallbacks 3a523966aa4840d4d8446101143a2d99c44600acdb12b50b5099b68811df7bf1 -->
<!-- api: T:Inno.Native.MiniAudio.MaWasapiUsage f071a5d53e0cce35b2f4f9f93765263977439548cae3104abf0304e2d86de15b -->
<!-- api: T:Inno.Native.MiniAudio.MaWaveform 91ca422b0c221438447a7c03473c8e0fee29c22d04a1dbda6f483d3b50c9f2af -->
<!-- api: T:Inno.Native.MiniAudio.MaWaveformConfig 3d34d4046c3b7dc6c58d50d98315ea9e1953b39c542a6a740d43ff7ee82863a8 -->
<!-- api: T:Inno.Native.MiniAudio.MaWaveformType 21398b60e4307077fdba0e34080ce9fe79579f33c71528a1b0d365a08c5e27b5 -->
<!-- api: T:Inno.Native.MiniAudio.MiniAudio.Constants 76a684f2b36b8943544ada684d20364940ab81ce0b642b7e927cbe2fb5795d27 -->
<!-- api: T:Inno.Native.MiniAudio.MiniAudioContext ccfb60f2c6595dd895b594398efcb0d663ad8734b19beef974aa910d27b97846 -->
<!-- api: T:Inno.Native.MiniAudio.MiniAudioContext.Constants 32e265123d8fbcd0b24aab4dd256cef746e0b80adb4d9e43dcc319aba829a2c2 -->
<!-- api: T:Inno.Native.MiniAudio.MiniAudioContext.RawApi 2351f280b0baf41ebe943163fe435e618cb2d01f5acdedc5e148b2ff7ad7dc76 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_282e8fc7b0cbe4ce 80ac98c9c15b65211064c3ec29b34ae4b7bc0499d51ce916e5997328650cdfaf -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_3561217d0d8daa30 073f1d4cbb99e7b111cd554b5637b25e8a51c1014374d9742f90a3a42fd2c5ff -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_50ae7efedbdc618e dd6b059ffd56a1b2b759e5336fa242e3d44aec08bdfb93a3afc1ebcf5b378404 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_513b8c6685130cf6 4a1ea7b864888445659e33f19eec7b999ccb80ef9fe7c140d829a7c7a2734952 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_5631104b5a38a871 8bfa1337157a8de8f270c0fdcc965e0cd72489b5c52b91fa1b1829de9d0ebdc8 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_5cecd9fb8864fb43 aaf3d211d712ebea25f9919bbdb9235721a674d88352bef4636adf620e0724d1 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_5f1329750a7569d7 1f0bd452c6648024a0fddc937b94c7994b001342449227318fd14f2899a107ba -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_84d2a1a3ae107f27 1c8ac6c7041328f633153c175d67912ac5113706ba9ada4b52e7fdebbf403cab -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_8666bb8f69608bb0 06b450aa62f1a4ca0ce97ab2f97690edad09998d50c3e275cc2abeaa328d6a67 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_89fe77cdd9158c23 56d60e80611db8bd504f9a20b7cf78a4558442bf49882e8d99d3d6b6878fb29a -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_95a1f650075f892e e374cf40d8dac1f4d4e19516aea2bab601c8b4b9adf03c406e50e891f1e908de -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_a4f71ca2b114fa9f 42cd27296003c3128273b449c4fe0cf80e028f9d5e1c52d50c24e7b3d4c69eae -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_bfed4867daabc092 532e09afd39d0c848544af05810d84c346ba0bd9877905bd78c21970f295c949 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_d14c56c076b259b5 e0f1b60b86f4f0fecd6dad5f118aafedc6eae42324ce7aaa669e05c1bd7428f9 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_d8db4ca1d386799a 4a6035b77e94e1af2f4bdd9b21f4bbc2ec2d46f8682d11ccb233a4e69f8e356c -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_dba04c537bb532b7 d7be85baf3fff9844f6483426972b2be2e11c52644e18983be2e2e0639032174 -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_ded1899ba091cc15 0cb24a4bdc604601663ce28af52010218aa3305d104f30fb96891fce7d36fc3f -->
<!-- api: T:Inno.Native.MiniAudio.NativeArray_f4284eefd115ca43 1b95219b618f608f94f8a6609d035da20c238ae17f5934cea7a3e24a864c9254 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-e234b5d145985c27"></a>[`public struct Inno.Native.MiniAudio.MaJobAnonymousUnion73ca8060d9820b4c`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22304) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c2e12b5479e216b1"></a>[`public struct Inno.Native.MiniAudio.MaJobAnonymousUnion85cb70704396c9bd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a3251dcb2325294c"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaJobProc(Inno.Native.MiniAudio.MaJob* argument0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22349) | Callback for processing a job. Each job type will have their own processing callback which will be called by ma_job_process(). |
| <a id="api-610d57cb597254b0"></a>[`public struct Inno.Native.MiniAudio.MaJobQueue`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22359) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84d9c9c409de2e78"></a>[`public struct Inno.Native.MiniAudio.MaJobQueueConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22416) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6822d402a568bbcc"></a>[`public enum Inno.Native.MiniAudio.MaJobQueueFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22445) | When set, ma_job_queue_next() will not wait and no semaphore will be signaled in ma_job_queue_post(). ma_job_queue_next() will return MA_NO_DATA_AVAILABLE if nothing is available. This flag should always be used for platforms that do not support multithreading. |
| <a id="api-be6005a51b094615"></a>[`public enum Inno.Native.MiniAudio.MaJobType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22459) | When a job type is added here an callback needs to be added go "g_jobVTable" in the implementation section. |
| <a id="api-555d21e3d880edce"></a>[`public struct Inno.Native.MiniAudio.MaLcg`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22523) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a8fbc363d7cf4291"></a>[`public struct Inno.Native.MiniAudio.MaLinearResampler`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22544) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6eb66894186399e1"></a>[`public struct Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22592) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ad4c516ef43cb34"></a>[`public struct Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22615) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c97abc986c2ecd1b"></a>[`public struct Inno.Native.MiniAudio.MaLinearResamplerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22643) | ************************************************************************************************************************************************************ Resampling ************************************************************************************************************************************************************ |
| <a id="api-a027870e706f7c7f"></a>[`public struct Inno.Native.MiniAudio.MaLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22680) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ca8d1dd9c302320"></a>[`public struct Inno.Native.MiniAudio.MaLogCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22713) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ad4d4b111e4a0e31"></a>[`public delegate void Inno.Native.MiniAudio.MaLogCallbackProc(void* argument0, uint argument1, sbyte* argument2)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22754) | The callback for handling log messages. Parameters ---------- pUserData (in) The user data pointer that was passed into ma_log_register_callback(). logLevel (in) The log level. This can be one of the following: +----------------------+ &#124; Log Level &#124; +----------------------+ &#124; MA_LOG_LEVEL_DEBUG &#124; &#124; MA_LOG_LEVEL_INFO &#124; &#124; MA_LOG_LEVEL_WARNING &#124; &#124; MA_LOG_LEVEL_ERROR &#124; +----------------------+ pMessage (in) The log message. |
| <a id="api-bed0c8105ea017b6"></a>[`public enum Inno.Native.MiniAudio.MaLogLevel : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22782) | Logging Levels ============== Log levels are only used to give logging callbacks some context as to the severity of a log message so they can do filtering. All log levels will be posted to registered logging callbacks. If you don't want to output a certain log level you can discriminate against the log level in the callback. MA_LOG_LEVEL_DEBUG Used for debugging. Useful for debug and test builds, but should be disabled in release builds. MA_LOG_LEVEL_INFO Informational logging. Useful for debugging. This will never be called from within the data callback. MA_LOG_LEVEL_WARNING Warnings. You should enable this in you development builds and action them when encountered. These logs usually indicate a potential problem or misconfiguration, but still allow you to keep running. This will never be called from within the data callback. MA_LOG_LEVEL_ERROR Error logging. This will be fired when an operation fails and is subsequently aborted. This can be fired from within the data callback, in which case the device will be stopped. You should always have this log level enabled. |
| <a id="api-1aa073c06579231c"></a>[`public struct Inno.Native.MiniAudio.MaLoshelf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22798) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-32a977199cbdae4c"></a>[`public struct Inno.Native.MiniAudio.MaLoshelf2Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22824) | ************************************************************************************************************************************************************ Low Shelf Filter ************************************************************************************************************************************************************ |
| <a id="api-d24cb0b048366691"></a>[`public struct Inno.Native.MiniAudio.MaLoshelfNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22855) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-84c8aaad6de8b78b"></a>[`public struct Inno.Native.MiniAudio.MaLoshelfNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22881) | Low Shelf Filter Node |
| <a id="api-efe9da960aec6ee9"></a>[`public struct Inno.Native.MiniAudio.MaLpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22904) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-114a199ab48934ac"></a>[`public struct Inno.Native.MiniAudio.MaLpf1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22944) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ff6e27a29bb8fce8"></a>[`public struct Inno.Native.MiniAudio.MaLpf1Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22983) | ************************************************************************************************************************************************************ Low-Pass Filtering ************************************************************************************************************************************************************ |
| <a id="api-8f76ee0a002b49dc"></a>[`public struct Inno.Native.MiniAudio.MaLpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23012) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bfd4487050989909"></a>[`public struct Inno.Native.MiniAudio.MaLpfConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23036) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-50adb94c0e06a4b8"></a>[`public struct Inno.Native.MiniAudio.MaLpfNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23068) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9462c9e208d8c73"></a>[`public struct Inno.Native.MiniAudio.MaLpfNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23094) | Low Pass Filter Node |
| <a id="api-2e4294edccef0d6f"></a>[`public enum Inno.Native.MiniAudio.MaMonoExpansionMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23115) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c48bb6c0af61e7e"></a>[`public struct Inno.Native.MiniAudio.MaNodeBase`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23140) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-083a1d8272cf6f26"></a>[`public struct Inno.Native.MiniAudio.MaNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23225) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4d89321a5aeea51"></a>[`public enum Inno.Native.MiniAudio.MaNodeFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23275) | Node flags. |
| <a id="api-b4bca9dcb1e5524e"></a>[`public struct Inno.Native.MiniAudio.MaNodeGraph`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23292) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67a5edb8dab7cba9"></a>[`public struct Inno.Native.MiniAudio.MaNodeGraphConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23340) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21514daac0b9dc53"></a>[`public struct Inno.Native.MiniAudio.MaNodeInputBus`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23371) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b3797c85269413ae"></a>[`public struct Inno.Native.MiniAudio.MaNodeOutputBus`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23410) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9bc944ae71909a5e"></a>[`public enum Inno.Native.MiniAudio.MaNodeState : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23490) | The playback state of a node. Either started or stopped. |
| <a id="api-ff68ba0ba26cf3a0"></a>[`public struct Inno.Native.MiniAudio.MaNodeVtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23504) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2b705b71db82dde"></a>[`public struct Inno.Native.MiniAudio.MaNoise`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b34d2c2f2a08f7d3"></a>[`public struct Inno.Native.MiniAudio.MaNoiseAnonymousStruct150b5b8daffaf7dd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23595) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1dfb2e491c0d1d55"></a>[`public struct Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23616) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cdef80ada4cab6f9"></a>[`public struct Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67ca793df9efaf40"></a>[`public struct Inno.Native.MiniAudio.MaNoiseConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23664) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8282a4a80614552f"></a>[`public enum Inno.Native.MiniAudio.MaNoiseType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23693) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ebb1c128a58b240f"></a>[`public struct Inno.Native.MiniAudio.MaNotch2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23708) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fefb2aad767cf35"></a>[`public struct Inno.Native.MiniAudio.MaNotch2Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23734) | ************************************************************************************************************************************************************ Notching Filter ************************************************************************************************************************************************************ |
| <a id="api-d89cd30163a81533"></a>[`public struct Inno.Native.MiniAudio.MaNotchNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23763) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4edc42b28fbb09dc"></a>[`public struct Inno.Native.MiniAudio.MaNotchNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23789) | Notching Filter Node |
| <a id="api-625075ae268835df"></a>[`public enum Inno.Native.MiniAudio.MaOpenModeFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23810) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9aceb610367bc85a"></a>[`public enum Inno.Native.MiniAudio.MaOpenslRecordingPreset : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23825) | OpenSL recording presets. |
| <a id="api-48944c398e5971ea"></a>[`public enum Inno.Native.MiniAudio.MaOpenslStreamType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23862) | OpenSL stream types. |
| <a id="api-713d428a3ed08322"></a>[`public struct Inno.Native.MiniAudio.MaPagedAudioBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23902) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f94fc92f5516ebf3"></a>[`public struct Inno.Native.MiniAudio.MaPagedAudioBufferConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23937) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ee4a3d0a95accb8"></a>[`public struct Inno.Native.MiniAudio.MaPagedAudioBufferData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23961) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2eeb47b5051168ed"></a>[`public struct Inno.Native.MiniAudio.MaPagedAudioBufferPage`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23996) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7b99d3dcf822746"></a>[`public enum Inno.Native.MiniAudio.MaPanMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24022) | Stereo panner. |
| <a id="api-3d3f689e6dabafb8"></a>[`public struct Inno.Native.MiniAudio.MaPanner`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24042) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6893d9575e35b768"></a>[`public struct Inno.Native.MiniAudio.MaPannerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24072) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9fdc33e489f80b0"></a>[`public struct Inno.Native.MiniAudio.MaPcmRb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24099) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c3afc423acbcd938"></a>[`public struct Inno.Native.MiniAudio.MaPeak2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6545fe91bd484b3a"></a>[`public struct Inno.Native.MiniAudio.MaPeak2Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24157) | ************************************************************************************************************************************************************ Peaking EQ Filter ************************************************************************************************************************************************************ |
| <a id="api-29fae89b1de0edb9"></a>[`public struct Inno.Native.MiniAudio.MaPeakNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24188) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7f7b8dd3e213f0d"></a>[`public struct Inno.Native.MiniAudio.MaPeakNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24214) | Peaking Filter Node |
| <a id="api-b1d74a932f03be98"></a>[`public enum Inno.Native.MiniAudio.MaPerformanceProfile : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24235) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fdbe67d72f3af2ec"></a>[`public enum Inno.Native.MiniAudio.MaPositioning : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24247) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ddfd884bf30b4381"></a>[`public struct Inno.Native.MiniAudio.MaPulsewave`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24261) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-dc00a8d4231a3832"></a>[`public struct Inno.Native.MiniAudio.MaPulsewaveConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd956b4bda5fbc09"></a>[`public struct Inno.Native.MiniAudio.MaRb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24320) | ********************************************************************************************************************************************************** Ring Buffer ********************************************************************************************************************************************************** |
| <a id="api-9e7878e898a757c6"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaReadProc(void* argument0, void* argument1, ulong argument2, ulong* argument3)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24368) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-295d4bbb99e6dbd9"></a>[`public enum Inno.Native.MiniAudio.MaResampleAlgorithm : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24376) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ce90cda065a3f5bb"></a>[`public struct Inno.Native.MiniAudio.MaResampler`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24393) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ddb0fb0c2d01ad95"></a>[`public struct Inno.Native.MiniAudio.MaResamplerAnonymousUnion67c9af061924ab62`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24438) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6d7d405d974d54a"></a>[`public struct Inno.Native.MiniAudio.MaResamplerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24459) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-328006bf20954a63"></a>[`public struct Inno.Native.MiniAudio.MaResamplerConfigAnonymousStruct816361d296299c5c`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24500) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-963a610d1911d85c"></a>[`public struct Inno.Native.MiniAudio.MaResamplingBackendVtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24521) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-392400befc01b8d8"></a>[`public struct Inno.Native.MiniAudio.MaResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24575) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6847bc6fc09527fc"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24626) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7d150ce72af470c0"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9fead97186ef2a14"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataBufferAnonymousUnion9072cdd7c6d42b32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24770) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9efea4825302e8e"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24804) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-67265064080fa5d8"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24858) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7a256475d25141ca"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSourceAnonymousUnion88c267d2dde03905`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24897) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4558adc613a34ff"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSourceConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24920) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e3f09dabe6eec67d"></a>[`public enum Inno.Native.MiniAudio.MaResourceManagerDataSourceFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24960) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af2236e33b3db092"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24996) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fc60f249f07b5e52"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25105) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e5e01964041bc7d"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStruct86905fc328b96f21`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25131) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1deff9ef911026a9"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStructB49f272bd36a3013`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25154) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-670aa4114048a25e"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousStructE4f5f54148be4e4d`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25179) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a382da69dc0411a"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupplyAnonymousUnion7b7c4bd0caacafb5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25210) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-09db401c062951c2"></a>[`public enum Inno.Native.MiniAudio.MaResourceManagerDataSupplyType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25233) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cada1ada996212c6"></a>[`public enum Inno.Native.MiniAudio.MaResourceManagerFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25259) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6e585ae676f32eba"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25279) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2396bf7c553596ca"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25311) | Pipeline notifications used by the resource manager. Made up of both an async notification and a fence, both of which are optional. |
| <a id="api-f650295e33f003aa"></a>[`public enum Inno.Native.MiniAudio.MaResult : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25332) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8b4e70bec707ef0d"></a>[`public enum Inno.Native.MiniAudio.MaSeekOrigin : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25470) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f1cd8abbb7f327d2"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaSeekProc(void* argument0, long argument1, Inno.Native.MiniAudio.MaSeekOrigin argument2)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25487) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1a6fefee69bc1634"></a>[`public enum Inno.Native.MiniAudio.MaShareMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25495) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f9ee0b16ac433536"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocator`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25509) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7173093e8eff7a7f"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25565) | Slot Allocator -------------- The idea of the slot allocator is for it to be used in conjunction with a fixed sized buffer. You use the slot allocator to allocate an index that can be used as the insertion point for an object. Slots are reference counted to help mitigate the ABA problem in the lock-free queue we use for tracking jobs. The slot index is stored in the low 32 bits. The reference counter is stored in the high 32 bits: +-----------------+-----------------+ &#124; 32 Bits &#124; 32 Bits &#124; +-----------------+-----------------+ &#124; Reference Count &#124; Slot Index &#124; +-----------------+-----------------+ |
| <a id="api-d328bf7b31c41dfe"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorGroup`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25589) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ffde3f0787e42290"></a>[`public struct Inno.Native.MiniAudio.MaSound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25613) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-69e44e1ca454f6f0"></a>[`public struct Inno.Native.MiniAudio.MaSoundConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25663) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3fee673fd3a1769a"></a>[`public delegate void Inno.Native.MiniAudio.MaSoundEndProc(void* argument0, Inno.Native.MiniAudio.MaSound* argument1)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25768) | Callback for when a sound reaches the end. |
| <a id="api-91c7940a5da3831c"></a>[`public enum Inno.Native.MiniAudio.MaSoundFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25779) | Sound flags. |
| <a id="api-b78f298f9787b5bc"></a>[`public struct Inno.Native.MiniAudio.MaSoundInlined`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25827) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2dcc070e580b2b00"></a>[`public struct Inno.Native.MiniAudio.MaSpatializer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25852) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cf5b7d073799650a"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25950) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-98042f57a705f4a4"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerListener`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26020) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ee37cb3454b1f68"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerListenerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26062) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6200d70d76291538"></a>[`public struct Inno.Native.MiniAudio.MaSplitterNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26100) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ef7d15eb6e354489"></a>[`public struct Inno.Native.MiniAudio.MaSplitterNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26124) | Splitter Node. 1 input, many outputs. Used for splitting/copying a stream so it can be as input into two separate output nodes. |
| <a id="api-92de37ad0b6713ae"></a>[`public struct Inno.Native.MiniAudio.MaStack`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26152) | For some internal memory management of ma_node_graph. |
| <a id="api-98c1d0470f8cb60a"></a>[`public enum Inno.Native.MiniAudio.MaStandardChannelMap : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26175) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-18ad72c653bc799c"></a>[`public enum Inno.Native.MiniAudio.MaStandardSampleRate : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26206) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6d20467d437781f"></a>[`public delegate void Inno.Native.MiniAudio.MaStopProc(Inno.Native.MiniAudio.MaDevice* argument0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26265) | DEPRECATED. Use ma_device_notification_proc instead. The callback for when the device has been stopped. This will be called when the device is stopped explicitly with `ma_device_stop()` and also called implicitly when the device is stopped through external forces such as being unplugged or an internal error occurring. Parameters ---------- pDevice (in) A pointer to the device that has just stopped. Remarks ------- Do not restart or uninitialize the device from the callback. |
| <a id="api-8358168ef1fe01c6"></a>[`public enum Inno.Native.MiniAudio.MaStreamFormat : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26273) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f44eb49c8025ef86"></a>[`public enum Inno.Native.MiniAudio.MaStreamLayout : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26284) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c03509976afc66ec"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaTellProc(void* argument0, long* argument1)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ccb429d9aecb0bd1"></a>[`public enum Inno.Native.MiniAudio.MaThreadPriority : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26308) | Thread priorities should be ordered such that the default priority of the worker thread is 0. |
| <a id="api-9a3f14f6ded11085"></a>[`public struct Inno.Native.MiniAudio.MaTimer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6aa94e0fd57c5e87"></a>[`public struct Inno.Native.MiniAudio.MaVec3f`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26354) | Spatializer. |
| <a id="api-305998c7433c4665"></a>[`public struct Inno.Native.MiniAudio.MaVfsCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-930b0295f5f52b41"></a>[`public enum Inno.Native.MiniAudio.MaWasapiUsage : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26415) | WASAPI audio thread priority characteristics. |
| <a id="api-4624010da3ae3343"></a>[`public struct Inno.Native.MiniAudio.MaWaveform`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26430) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b761db1ae39529d"></a>[`public struct Inno.Native.MiniAudio.MaWaveformConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26457) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9d3ccba593165947"></a>[`public enum Inno.Native.MiniAudio.MaWaveformType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26486) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b5ad6d41739d3111"></a>[`public static class Inno.Native.MiniAudio.MiniAudio.Constants`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L223) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f90eae4c1a703608"></a>[`public sealed class Inno.Native.MiniAudio.MiniAudioContext : System.IDisposable, System.IAsyncDisposable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L260) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9e3c171ef9b29cd"></a>[`public static class Inno.Native.MiniAudio.MiniAudioContext.Constants`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L285) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-303e581c9f4a4376"></a>[`public sealed class Inno.Native.MiniAudio.MiniAudioContext.RawApi`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-419fe7946c59b00f"></a>[`public struct Inno.Native.MiniAudio.NativeArray_282e8fc7b0cbe4ce`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26501) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9003c41ed0744475"></a>[`public struct Inno.Native.MiniAudio.NativeArray_3561217d0d8daa30`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26513) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6255e492cb683b4"></a>[`public struct Inno.Native.MiniAudio.NativeArray_50ae7efedbdc618e`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26525) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d8f42f1a6d424f47"></a>[`public struct Inno.Native.MiniAudio.NativeArray_513b8c6685130cf6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26537) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-addd5fea92a5dc73"></a>[`public struct Inno.Native.MiniAudio.NativeArray_5631104b5a38a871`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26549) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-11497e828b7b253d"></a>[`public struct Inno.Native.MiniAudio.NativeArray_5cecd9fb8864fb43`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26561) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2bee9a62c93b4c16"></a>[`public struct Inno.Native.MiniAudio.NativeArray_5f1329750a7569d7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26573) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a0a8dd9e4631af59"></a>[`public struct Inno.Native.MiniAudio.NativeArray_84d2a1a3ae107f27`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26585) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3dbf9dd49ed140c8"></a>[`public struct Inno.Native.MiniAudio.NativeArray_8666bb8f69608bb0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26597) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af5ddf8825c30539"></a>[`public struct Inno.Native.MiniAudio.NativeArray_89fe77cdd9158c23`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26609) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f82819889a0524ba"></a>[`public struct Inno.Native.MiniAudio.NativeArray_95a1f650075f892e`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c8f16721967c2855"></a>[`public struct Inno.Native.MiniAudio.NativeArray_a4f71ca2b114fa9f`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26655) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-74c6e37af0789364"></a>[`public struct Inno.Native.MiniAudio.NativeArray_bfed4867daabc092`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26667) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c152dc2ee4f88b56"></a>[`public struct Inno.Native.MiniAudio.NativeArray_d14c56c076b259b5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26679) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df514bc4aabaf4a1"></a>[`public struct Inno.Native.MiniAudio.NativeArray_d8db4ca1d386799a`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26691) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-253b5207527846e4"></a>[`public struct Inno.Native.MiniAudio.NativeArray_dba04c537bb532b7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26703) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-471a2a3a747c24f5"></a>[`public struct Inno.Native.MiniAudio.NativeArray_ded1899ba091cc15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26715) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-01ccd00559685c97"></a>[`public struct Inno.Native.MiniAudio.NativeArray_f4284eefd115ca43`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26727) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
