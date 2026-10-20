# Inno.Native.MiniAudio ABI · 11

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INTERRUPT 4f0c0e13a4aecfae0121222863a21fc3b2ddb1f49b6870cb4e4667c9783f5493 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INVALID_ARGS 1f9e5d256c1338cea9009f26ee8a98a1a34b316c11b7551d8552b55347c55079 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INVALID_DATA 4fef915206cd14c676f93259dc7bd81f51651171f661fd8a4ba0d05697570062 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INVALID_DEVICE_CONFIG 7b199c86cbbcf7fb581e41dd65f6aa9d2288e9cad4b018fe16486370b0d80f67 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INVALID_FILE 201b271d8041cfb2b1812763a9691f8a8f0aa106f8837c93825fb92a3b1ef7fd -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_INVALID_OPERATION f69e52d33c20934440e1a98eb22cf3d8ca0df8f3f37231685da0e97661380f5a -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_IN_PROGRESS 040be10eb724cbfc1e534852d9e78954b9dc347258931af3ef580101ea9eaebf -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_IO_ERROR 957e9e1e2f9fea40efb68a1874befd55d9c9949a910edb6778abe369835e43a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_IS_DIRECTORY b0c765b7225396cd6a30db583a91fc57c4759f719a2b70836dd45ee67c266445 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_LOOP b24497c93f65713506245b3ca04e5e1c9510290e8b78cccfcd40c70173b98360 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_MEMORY_ALREADY_MAPPED a6970413c6a8eca790c6d3de91a0b6f6ba792e98c99d25d456dc6c4fd37766af -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NAME_TOO_LONG c9af28805365f4eb1bf0bffccc9b4955730daca7ce2283744a0877a750917666 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NOT_CONNECTED c0cb45672395e72030f43ed6d6c7d65da344decf9fc880d9ece3389934032988 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NOT_DIRECTORY b36a211a02f62e4f4ccd7597208cc014e2b5ff45d7bb9111d7c5e4ed5c17d7e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NOT_IMPLEMENTED 4fb69083b821898b898633ccd296630ead54234262a7a6cbb81b876aae29f92e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NOT_SOCKET 6fbaf1ed9fee764e25577c7307e5c3c9a9bd2f6ebc788ecc2ae1e1b772d5b321 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NOT_UNIQUE ec2f9da07265a277eb857c8a0739cbb79cf77235e6a990eb1d3b07c9116c12a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_ADDRESS 302768bad25f853b2fa326c8d19cda26323c9aee8e103cf9293f2822a89a0e04 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_BACKEND f107932a7e91dd475e9072f12bc135e9fca366e2710f0f4cacb3acda8fe85a20 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_DATA_AVAILABLE 515f88e5741b1362b84bc5692ad63d77a605fd74ef1668e34189971bc3ac16ba -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_DEVICE e48f999dcd2a53fb914b4efe2db7d9095826f0e2f46b5c9fd3c5cab5c36f200a -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_HOST 941f6da1e55e84bd04d13925492def6fee27c369059a328c7e57046b9846e9db -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_MESSAGE 032e7e6c887e0cc7c4c36f3162bd3497c561201f2d9d53b3626e09a5aab8a41b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_NETWORK de6695d83d1225e2d57abdb731f619e2a1733853ccf5d44596e522893a3d082b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_NO_SPACE 23356defd547965d7d3e0ee27ba4469efd8c44cc29b3290d16410ddbcffdf7a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_OUT_OF_MEMORY ef61e454afce2f2b669903858258dcbe7f60b2712716dca431c85c55b749ab73 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_OUT_OF_RANGE 237077d14b2dd04daf77ce92e4228a94c139e0bffe0c451f3495d0034be681d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_PATH_TOO_LONG f1a1fcb1a1d685c40b47333837c99a0b6ed6f3fa5b3089a821ba2069a7c52424 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_FAMILY_NOT_SUPPORTED ddf2084205a6eb16aec026be154ff8130a6e02346f23c4a040f904fc3ea98b26 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_NOT_SUPPORTED 5ed83267adf873c914bfe1abae3fad1e5ee3c03e36304c2a0fa2e17c5356aa00 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_UNAVAILABLE 3c6c32c8899b5c6a239750922e498aa8a519775e68c92a25c25162ebf57335ed -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_SHARE_MODE_NOT_SUPPORTED 4ab993941736049b3650867e6af17a692c2c8c175f889c34e0d5bccb6dccd5b4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_SOCKET_NOT_SUPPORTED a1aecdc0e318ec249fb46c2517d5c0bc8d5967f6abc1d614fbc69e98bd13c486 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_SUCCESS 6f0fa6b483972d957a43048c30fe4fae44bbbf813ee8a0d22061247b5d8ab3a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_TIMEOUT b7b598a134d027069a25b14d9415737e4e790a8d758d46a87800cfa986d1bfb2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_TOO_BIG 8f3d795669643ecc9ec5091eb2d54f10437310c8246a13609d9b41dc9f2acadf -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_TOO_MANY_LINKS f1d8882d2ed1cbd401ce65ae9d49822db58044cf156cecb0089558a6e427def1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_TOO_MANY_OPEN_FILES cc2a77988b8b061479647958c6a23a770ef10521273054682182090c875bac6f -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MA_UNAVAILABLE ee34c653b90e09342207e7b037118d253dac3cad06231bb3ae58508275c63ee5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_current 57733e37463b6d92305518c8ccc17e9e86e50607a790591b0a0b01c36a3fbd16 -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_end 8ccd68f2a67c3816604cdada29225521ca94d696418344dcc8e131213a1d9168 -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_start 706b6b48eceef2cc78692d06e8b182ef85b47e7c9e2e6091dba7f6f52cdf09b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaShareMode.ma_share_mode_exclusive 841842ef0e1b6f3332450b8f1303b36def4cf7bd3ed5e61c36f0303fe33ed857 -->
<!-- api: F:Inno.Native.MiniAudio.MaShareMode.ma_share_mode_shared 82500c0b22c477365263a510713efd58a3f681cf36e4fa588bf4d525b381d373 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Capacity 3c9a1df30431d566bca600bcc3375d6c21ad28121ad588138d64232f723b755a -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Count afe311c497ad5fb004ec54359eedda411b62947e1ae428671a4a9391bd01b715 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.OwnsHeap d593eca2c54342cb209d59eef358e4f91280055c3929909c24d32797a340fb3c -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.PGroups 1dbb57d6ee0cc1dbad63d43ac858b97d29a4aecc79cd9be1be14699b58f90f08 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.PHeap 0d2d0b6605dd1654e55a88441ea221ebd8a19168dfb0f18d0b08b42a0e8d050a -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.PSlots 24b6d1a8631cbe76c111a606f983eeaa63c8620bb65890d60af986f94b1be482 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorConfig.Capacity d40d3efd8bc5698aafa34e59c4ae921fc15bd3a0213d661c7f20506b868b3613 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorGroup.Bitfield 01644108b93c99e4ec6bfd028523108c87bff34e6d52e5830cd66c8ca5db3864 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.AtEnd 6bb4480261c05cb596a931a5a17a3d7ad9f4d9c4c58c7d8206eb1998cccbce7c -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.EndCallback beb0edeb2dbe4b3a2322b70f1add6a71b600c58410d43bdc5ecdbed35c1737de -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.EngineNode 2b3d95297876acbb077455ec4f38e0865eef949c44e6c84499a4c45ebceba0df -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.OwnsDataSource ca39ac4bec562b3dd45791271f126677efe253e4f6a90684e452574549b4b3e6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PDataSource 5a472f3173a665f97b3a865691ffaf0dbacd23f93b7686be06fff482453f3263 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PEndCallbackUserData dfbeeada94f260f89ec9e3c398730080bd921374cd0945966a5ca286890437a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PProcessingCache 392388d21fb49622d30ed29d9fd0c9de93cbdff5873901f18e41af3a38339193 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PResourceManagerDataSource 4441aba8109c461c137e1056cd4f9b439ec53bd3d065485b4a5dc47263a43461 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.ProcessingCacheCap fc201c22b91178f520b8f27e21198145dc1e80f45c4e1c397cf076b0429f00d8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.ProcessingCacheFramesRemaining 8b8f3bf8431318065d3c1df22700d7da1ff590d0760812a2030fc978363bce3c -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.SeekTarget d3bfaf40fdd8df7a2d7258beef117d0171fa1ec8aaa329564a8d4735a912bc5c -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.ChannelsIn 4da5e6088fec25fbfd0260dfa31bf2bab561907efc68b3ba8e39a1229c7eef3f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.ChannelsOut 88358e176767ba1114cee7668f75d403781a94fa87658f9c1c1ed3386aa1bbc4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.EndCallback 0d06f95b528a4356187073b7d3489571ddfe9df9506a7607f1ededc8019b1976 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.Flags 8a5bedb58fdafbe58b84cc17322ab07b5737995b6e917a58489edf9ada8dd35d -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitNotifications 82d9f070e99c77baf4cf152767de5b86e6c10fa201528886a9dfeeb3d398dbe0 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitialAttachmentInputBusIndex 51e00693da7f2e7ea7b358fa554f72f7660c1c1079b2ac1f5485bdfb53ebc00d -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitialSeekPointInPcmFrames 38ddc60b309b92347bc8e35cea62514bf677b6e11a5a4e7b2acc32f72ec65b38 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.IsLooping b5331bad83393d934fe5ac1823f7372a4f0beacfeda6bc5428d0276316c0e444 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.LoopPointBegInPcmFrames badd30f43c6da396e4a0ec657a2116fec34e09c5249f8fe1da9b17c83ea39f11 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.LoopPointEndInPcmFrames 122eb0abba34c30b7d056e4c2f2528b8554afdd2d2de0e7cd16c023923efb95f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.MonoExpansionMode 8b6ca813f2a8278994da17f6cbd99e50ff5f5f975b60935d215b1e7bf75156b6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PDataSource 8a3de6f1d55ba957661548c04b9a0aef4efffd18b6361bce586f89bed0e568d9 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PDoneFence f075851e9db60f2bded2f97662bd6346e51517df5055f1fec70b0112b42a93c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PEndCallbackUserData ecca5bf999e0400b9c9f5e1973b615023381967d8d1b8c56228e4eb6cd5a2306 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PFilePath 37d67b857381b855debf03dd953a5f6199926cfb7ce666e081d1252625d457c9 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PFilePathW 69b050ef30bebf9517717995e048b09da174fc1a02175508972b2580340e4e3f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PInitialAttachment a4672ed3dde31deb1fa22af704ff5bafbf8a1769380d8ea4dac078c00dd4aefb -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PitchResampling d573019e98df8cb5f38ffb1c80ae857c51eaeed486f57852d723eada23ddf265 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.RangeBegInPcmFrames e520e22d59cedc0eb4c75addc7ebd1243a0738f19dfb97995254f21490e3b232 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.RangeEndInPcmFrames 17be88fae2f39dd0425211360716e3734b0be89368d4626c20d6d6f2d9056275 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.VolumeSmoothTimeInPcmFrames db29eee5d830ff4fa34690907282ac384eec85b1ab091f55ffae96d5ba8a8e35 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_ASYNC 098b8df8bcb1908b3fa5277496d9930c1e3b3ba3275a9e67fb0e1a711b2301d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_DECODE 347c6492b2f890ded49c2ffd86947b2e136f16ef45b9f62a41c6aafd8d41ea77 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_LOOPING c42c8739e14bf81dd35d2310e5bd5d1abc10133ca8c0aa6bb1a53c226932f832 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_DEFAULT_ATTACHMENT 31d1542e71844350b64ab07cde34b9d7e3e75d0b15ac1c54fc7a976e82d20a34 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_PITCH 6064b85201d4ebb3ae9b7ff7fc30f291d69eb6886ac1dd2bc18b88085993e1b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_SPATIALIZATION a4ad0d524677a8da5a7a1c8f68cc54e13315c3a0d724efded4f9a4a3ebd59152 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_STREAM 1672074ccb140ed452f58dc5571f6300b768bf7efb04d3534789404f64d0f80a -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_UNKNOWN_LENGTH 8fcd3ab6c4d76473c151ad07fcc4d16d09642d9dc3ceb7de2e6eafbedba6cf1c -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_WAIT_INIT 60f323b941a321731acc94c3239805179764f9964a3157cb8a3b52f1c44cce37 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.PNext 21b924a96c28fff11c5704e9a87b50f5051c01ced040e0cad15f0c9a38855263 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.PPrev d0d4828b3c4c58e4b628ac45b311a7a85616dea7adfac735b3fdcb45cedcb1c2 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.Sound 7b0c3df2aaaa6880aa040d258cd58195bf06d08ad5e6d503e598fc59f63731a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.AttenuationModel 41bfa9fbdbc76aa7565b7931d7bb9a6258c169593170e5435bba297245ce40f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ChannelsIn bb4fdb2f8c102706cfbbe36eb8b83c999b8085b392fd57168c422ec3558f12ae -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ChannelsOut 6e7badde1bec7e72134036ab523e7b479d1dd56e5174fd3582b2591ef5db520f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeInnerAngleInRadians 7f8ec8b83288924d2a8e87705d7168a6f5e9c7ab80684b8694fbcaffce95d476 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeOuterAngleInRadians ee9c26fbaa5e40024b6367fc45015142d24897901e7deaed67967dfa97a41295 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeOuterGain f6ca22a6cc192fa1df9dfb20afdbb7f037c5880dd1504bf0a64b229fa59bdb90 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Direction c42b58e6c7f5ac72cb909efacd5e382a6db72ff59335501090e8dc02a6878685 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DirectionalAttenuationFactor c59a674c085770e57811e24cdeaeb702681920692b6c58e0c810e09c4777d81c -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DopplerFactor 523283393d8e040837e35de4ff7206652a146c2a947d29b832482282a965b395 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DopplerPitch 260246735f5f4240b8f2175c92978a53779356c57ef24932309f07214584af5a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.GainSmoothTimeInFrames 940b3759e22060a843a12b7dbb8e1dee8aaef088f77d3ac753d5a1bf49ee26ea -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Gainer e15d509af13b1f02a94eb6208153dd44d848cf7621b7cee95726eb7a25514e24 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Handedness 19ff37e80a4e3ec0304a75c92475e21e0f75649d732cd7c6d93e2f1780e7a6be -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MaxDistance 994865c6cb47057a3f212b7e297e74f38fbb62c6ea2714af15b5cf59e418391a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MaxGain ffed079054d7ad00943c0bd3fa6dffa9d89ff6546e39f02f38d3a6e72b68dccc -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinDistance 41c5a2c4b3234461f4a833047b74fd7470f0a30ed6cb0a8c5141634bb2c9d698 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinGain 39591723530d1245da6fd58906c30c92ebf89707cfe097479275d61a245d97da -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinSpatializationChannelGain 0cef4bf613b55ce73d877d170f313895630073b63ffc742fc32187dbd5286c13 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.OwnsHeap 56d177a6be29f044bb9acd5b862a3ad2ef9d814a57918ba5970c5086c796b29b -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.PChannelMapIn 82e76768d32246b5914041277c5dd205761be687cec8fcb5f42b61e6311c2c04 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.PHeap ec08146123a3acc7a9e0844c4ad115a79e8faf8bdf0f17f702bf9fefe1b9f258 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.PNewChannelGainsOut b7f2fad3a36b073f6bd1eb4054463965ec0139d8728a9e1c74013fdf1880459f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Position 1ad841ee03c28d409f74cbf28f9597e772e7e2670695010184010c7ab8411e82 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Positioning 313c42a04956e9880e895ced3ab7e733d8bfea280fec4fe542fcee5368759457 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Rolloff b96765360ace5403d617c76ad8f26069011b1769a59c5304608ff612c8fa8ec4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Velocity 12d54d57c1f81f853e68ea2b967a714e940cbf0c7026430d5bc6f5231c381eaf -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.AttenuationModel 7b8dae6595b407c2807a6142989f4478bf0b7e07a5e35142ca5f5be52650820a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsIn 3d368a2b89a8a6cdf5d2f72a1a4c6f5d6f9d47ecf044ea0ccccbd00076f359f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsOut 4db469abeb909ea5733dc2781e25c44bb2c58328bc7df9b25843c406e107b1c4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeInnerAngleInRadians e125b999f348a029d14b1005ab2de879c2699bdb89e76e804cd8891cddc48bc8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterAngleInRadians 8fa46327101240b0be8c5ef38c91190e3989f57cf11afe319612a442bec2bc08 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterGain c4eab956933dcaf8338daf32153a93037ca9f49f149330f1f7a5ede2bbc6bb8e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.DirectionalAttenuationFactor 6736770a2ad698799677feaccba912d1af3b0fb76eceba823374887ca90b2b14 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.DopplerFactor d768a5201656492fa2680a8afec61b34b3ea0bdead876b2b32fa9f89df5df99e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.GainSmoothTimeInFrames ee102b9bfa4ce607bc76c788fe1de86a6ac3d521f0d6472a4d6ebf333dd49c83 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Handedness 7672a49555482acd4a1d5acce14bfe4bbbe97d400443b8652f11ab95d3c96487 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MaxDistance 8561ee6127c832d65e6a3c428bfc8ab8fd0dc0b7ce4a8ab04f09686ee69ee68b -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MaxGain 69ff069ba819501d7d5cb174ad0a68c393d61ddd5cdd8f68998aa61f2c3d9e93 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinDistance ac99ed1814c09004b4fe20a3839a5e133229bcf76142641fb18e6eba9cf673fa -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinGain 1dc6c773ab07bb2b4038557d4398e5c9d2c0637cfa5b74261b815fe7918a5835 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinSpatializationChannelGain d37ae1231a04131e48553f3b8f1c7b196c972025183ef7f7de2bbc5a2a260d0e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.PChannelMapIn 540d5d4d3a8b34c133eac1355e2cf27b414e0b2e9aabcd6a470bba9c3caacb9f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Positioning 4e5a366e9da547f801c02337d04467ed9260941533b372319285c173b124e9a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Rolloff f91759d6c59aa8202d80f3a883e16d8a70c8031f660a925a8b6f9baa1dc0e3ba -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Config 06d9fe80517b1ddc0d60fa4c4dc582cc3ec85e063d47b816c4a4598c4f14e4eb -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Direction 793c50da05c88fd4349a3b989ad5b10a2bc2013c622cc6eb3a128f2b9acc2596 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.IsEnabled 50fb682c0183d142c874836110e92d1e0b64af07f46ae0ac3c3cb9e71ffb618d -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.OwnsHeap 83c5e9981c58330dd18f80ee4e5d68d9e043c35b08c254294e24770eddc9c72f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.PHeap 59d8fd229612f64ab0bdf5f8107f794414d4b59c3633b716533322ae0a22f545 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Position 8f958a663f654253d50c7ecc12602ea10a368fe83e4e060055cf4294733b8f38 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Velocity 20e724ed1a67a30d58376ca2566942131cedecc6339488a2ac2a275252647cfa -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-4e8822cafdad3811"></a>[`Inno.Native.MiniAudio.MaResult.MA_INTERRUPT = -21`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25358) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-21` |
| <a id="api-27c81dbb0b01350f"></a>[`Inno.Native.MiniAudio.MaResult.MA_INVALID_ARGS = -2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25339) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-2` |
| <a id="api-6f88f6d2010d77c8"></a>[`Inno.Native.MiniAudio.MaResult.MA_INVALID_DATA = -33`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25370) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-33` |
| <a id="api-c73ebfb68d71a0e6"></a>[`Inno.Native.MiniAudio.MaResult.MA_INVALID_DEVICE_CONFIG = -206`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25421) | General miniaudio-specific errors.<br>常量：`-206` |
| <a id="api-89a1d3d518094c28"></a>[`Inno.Native.MiniAudio.MaResult.MA_INVALID_FILE = -10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25347) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-10` |
| <a id="api-f268117667950fb2"></a>[`Inno.Native.MiniAudio.MaResult.MA_INVALID_OPERATION = -3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25340) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-3` |
| <a id="api-1e5d6e6c109d48aa"></a>[`Inno.Native.MiniAudio.MaResult.MA_IN_PROGRESS = -50`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25387) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-50` |
| <a id="api-67b09d5bb75fc62b"></a>[`Inno.Native.MiniAudio.MaResult.MA_IO_ERROR = -20`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25357) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-20` |
| <a id="api-c2bd6145a8754215"></a>[`Inno.Native.MiniAudio.MaResult.MA_IS_DIRECTORY = -15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25352) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-15` |
| <a id="api-f0ad5cd96e84aafe"></a>[`Inno.Native.MiniAudio.MaResult.MA_LOOP = -207`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25425) | General miniaudio-specific errors.<br>常量：`-207` |
| <a id="api-9639b2ee94710189"></a>[`Inno.Native.MiniAudio.MaResult.MA_MEMORY_ALREADY_MAPPED = -52`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25389) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-52` |
| <a id="api-7c34ea0aa9ef1562"></a>[`Inno.Native.MiniAudio.MaResult.MA_NAME_TOO_LONG = -13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-13` |
| <a id="api-37966a4741065cb7"></a>[`Inno.Native.MiniAudio.MaResult.MA_NOT_CONNECTED = -47`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25384) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-47` |
| <a id="api-d521687426204e0b"></a>[`Inno.Native.MiniAudio.MaResult.MA_NOT_DIRECTORY = -14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-14` |
| <a id="api-be2b7f1a3c0731bf"></a>[`Inno.Native.MiniAudio.MaResult.MA_NOT_IMPLEMENTED = -29`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25366) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-29` |
| <a id="api-df825d4e1832bd29"></a>[`Inno.Native.MiniAudio.MaResult.MA_NOT_SOCKET = -37`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25374) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-37` |
| <a id="api-b9e8115b5db2fbbc"></a>[`Inno.Native.MiniAudio.MaResult.MA_NOT_UNIQUE = -36`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25373) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-36` |
| <a id="api-8b248d2bfcd69ad7"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_ADDRESS = -38`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25375) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-38` |
| <a id="api-79b201d804745ea7"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_BACKEND = -203`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25409) | General miniaudio-specific errors.<br>常量：`-203` |
| <a id="api-458da668fff9d89d"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_DATA_AVAILABLE = -32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25369) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-32` |
| <a id="api-031e66b877975c23"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_DEVICE = -204`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25413) | General miniaudio-specific errors.<br>常量：`-204` |
| <a id="api-698739379adbdbe6"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_HOST = -49`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25386) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-49` |
| <a id="api-051cba91e1a60b24"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_MESSAGE = -30`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25367) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-30` |
| <a id="api-7d363e48306b66f0"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_NETWORK = -35`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25372) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-35` |
| <a id="api-749e808041932099"></a>[`Inno.Native.MiniAudio.MaResult.MA_NO_SPACE = -18`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25355) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-18` |
| <a id="api-3a224b6abf51d568"></a>[`Inno.Native.MiniAudio.MaResult.MA_OUT_OF_MEMORY = -4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25341) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-4` |
| <a id="api-daead66a914ffde1"></a>[`Inno.Native.MiniAudio.MaResult.MA_OUT_OF_RANGE = -5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25342) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-5` |
| <a id="api-22fea694dba079ba"></a>[`Inno.Native.MiniAudio.MaResult.MA_PATH_TOO_LONG = -12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-12` |
| <a id="api-72c98d054ecb081d"></a>[`Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_FAMILY_NOT_SUPPORTED = -42`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25379) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-42` |
| <a id="api-c3de395b963025db"></a>[`Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_NOT_SUPPORTED = -41`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25378) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-41` |
| <a id="api-95e9fe848302d55d"></a>[`Inno.Native.MiniAudio.MaResult.MA_PROTOCOL_UNAVAILABLE = -40`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25377) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-40` |
| <a id="api-10f3a1cdbaace330"></a>[`Inno.Native.MiniAudio.MaResult.MA_SHARE_MODE_NOT_SUPPORTED = -202`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25405) | General miniaudio-specific errors.<br>常量：`-202` |
| <a id="api-f337eb99aba5bfd8"></a>[`Inno.Native.MiniAudio.MaResult.MA_SOCKET_NOT_SUPPORTED = -44`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25381) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-44` |
| <a id="api-3caad6821702fc1b"></a>[`Inno.Native.MiniAudio.MaResult.MA_SUCCESS = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25334) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-73bba3bdc14cae25"></a>[`Inno.Native.MiniAudio.MaResult.MA_TIMEOUT = -34`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25371) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-34` |
| <a id="api-a91b09ae30e08c99"></a>[`Inno.Native.MiniAudio.MaResult.MA_TOO_BIG = -11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25348) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-11` |
| <a id="api-98e854326e0357bc"></a>[`Inno.Native.MiniAudio.MaResult.MA_TOO_MANY_LINKS = -28`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25365) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-28` |
| <a id="api-0e8802a8ad4826a2"></a>[`Inno.Native.MiniAudio.MaResult.MA_TOO_MANY_OPEN_FILES = -9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25346) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-9` |
| <a id="api-0b8fea98021d7244"></a>[`Inno.Native.MiniAudio.MaResult.MA_UNAVAILABLE = -22`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25359) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`-22` |
| <a id="api-ec857312b706489a"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_current = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25473) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-8999a64734bf8ceb"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_end = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25477) | Not used by decoders.<br>常量：`2` |
| <a id="api-985c8e5f323b1a05"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.ma_seek_origin_start = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25472) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-3e15ae0674d05e5a"></a>[`Inno.Native.MiniAudio.MaShareMode.ma_share_mode_exclusive = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25498) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-b9a3b9f1bae993d2"></a>[`Inno.Native.MiniAudio.MaShareMode.ma_share_mode_shared = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25497) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-bc548c2258adb49a"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25527) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c6283ceb33fd832a"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25525) | Allocation count. |
| <a id="api-3228fc4711208965"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25532) | Memory management. |
| <a id="api-ed0ab0f32cbd54c6"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroup* Inno.Native.MiniAudio.MaSlotAllocator.PGroups`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25515) | Slots are grouped in chunks of 32. |
| <a id="api-06d53109000aa0b4"></a>[`public void* Inno.Native.MiniAudio.MaSlotAllocator.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25534) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d0b3915562d799bd"></a>[`public uint* Inno.Native.MiniAudio.MaSlotAllocator.PSlots`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25520) | 32 bits for reference counting for ABA mitigation. |
| <a id="api-635bb79a779e7a43"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocatorConfig.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25571) | The number of slots to make available. |
| <a id="api-1280e7ebdca0138e"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocatorGroup.Bitfield`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25595) | Must be used atomically because the allocation and freeing routines need to make copies of this which must never be optimized away by the compiler. |
| <a id="api-118332f5e0575021"></a>[`public uint Inno.Native.MiniAudio.MaSound.AtEnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25628) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05ae06c6c9af7229"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.MiniAudio.MaSound*, void> Inno.Native.MiniAudio.MaSound.EndCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25630) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-535b5b2b0dddbfa8"></a>[`public Inno.Native.MiniAudio.MaEngineNode Inno.Native.MiniAudio.MaSound.EngineNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25619) | Must be the first member for compatibility with the ma_node API. |
| <a id="api-0751cb39207623fa"></a>[`public byte Inno.Native.MiniAudio.MaSound.OwnsDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25643) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f62ae2c23253fe8b"></a>[`public void* Inno.Native.MiniAudio.MaSound.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a30d07942bda1d46"></a>[`public void* Inno.Native.MiniAudio.MaSound.PEndCallbackUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25632) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-056a721044f20c0e"></a>[`public float* Inno.Native.MiniAudio.MaSound.PProcessingCache`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25637) | Will be null if pDataSource is null. |
| <a id="api-5a961e6f04c3b4ec"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSource* Inno.Native.MiniAudio.MaSound.PResourceManagerDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25645) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1bce2347f7ec385"></a>[`public uint Inno.Native.MiniAudio.MaSound.ProcessingCacheCap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25641) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-627aa8bad44b07bb"></a>[`public uint Inno.Native.MiniAudio.MaSound.ProcessingCacheFramesRemaining`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25639) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-22ea76993677e163"></a>[`public ulong Inno.Native.MiniAudio.MaSound.SeekTarget`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25626) | The PCM frame index to seek to in the mixing thread. Set to (~(ma_uint64)0) to not perform any seeking. |
| <a id="api-0d4513afc4b6d766"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25694) | Ignored if using a data source as input (the data source's channel count will be used always). Otherwise, setting to 0 will cause the engine's channel count to be used. |
| <a id="api-8ecd5c2a47015efb"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25699) | Set this to 0 (default) to use the engine's channel count. Set to MA_SOUND_SOURCE_CHANNEL_COUNT to use the data source's channel count (only used if using a data source as input). |
| <a id="api-7e415a42c4fddecd"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.MiniAudio.MaSound*, void> Inno.Native.MiniAudio.MaSoundConfig.EndCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25732) | Fired when the sound reaches the end. Will be fired from the audio thread. Do not restart, uninitialize or otherwise change the state of the sound from here. Instead fire an event or set a variable to indicate to a different thread to change the start of the sound. Will not be fired in response to a scheduled stop with ma_sound_set_stop_time_*(). |
| <a id="api-967ea5e5865cd5ce"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25709) | A combination of MA_SOUND_FLAG_* flags. |
| <a id="api-a737fe62308fe3b6"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications Inno.Native.MiniAudio.MaSoundConfig.InitNotifications`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25738) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b6ffc8f4e6e4ae7a"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.InitialAttachmentInputBusIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25689) | The index of the input bus of pInitialAttachment to attach the sound to. |
| <a id="api-0232ff91ccfe5230"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.InitialSeekPointInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25719) | Initializes the sound such that it's seeked to this location by default. |
| <a id="api-728bb873b9a5b071"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.IsLooping`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25748) | Deprecated. Use the MA_SOUND_FLAG_LOOPING flag in `flags` instead. |
| <a id="api-58c6eb93d24f3798"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.LoopPointBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25725) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-be25fb476825ce2f"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.LoopPointEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25727) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5aa71b910d7dbf0a"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaSoundConfig.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25704) | Controls how the mono channel should be expanded to other channels when spatialization is disabled on a sound. |
| <a id="api-91e0c560f14f087e"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25679) | Set this to load from an existing data source. |
| <a id="api-85865f40cf81473c"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaSoundConfig.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25743) | Deprecated. Use initNotifications instead. Released when the resource manager has finished decoding the entire sound. Not used with streams. |
| <a id="api-1671c65897be2efc"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.PEndCallbackUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25734) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-9ebabb33e000ddf1"></a>[`public sbyte* Inno.Native.MiniAudio.MaSoundConfig.PFilePath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25669) | Set this to load from the resource manager. |
| <a id="api-6f9d147730c10f96"></a>[`public ushort* Inno.Native.MiniAudio.MaSoundConfig.PFilePathW`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25674) | Set this to load from the resource manager. |
| <a id="api-95ee0bfb3b87c1da"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.PInitialAttachment`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25684) | If set, the sound will be attached to an input of this node. This can be set to a ma_sound. If set to NULL, the sound will be attached directly to the endpoint unless MA_SOUND_FLAG_NO_DEFAULT_ATTACHMENT is set in `flags`. |
| <a id="api-39f51c64ea8914d2"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaSoundConfig.PitchResampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25736) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d953433f1c420e69"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.RangeBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25721) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fbdd4c6626cc5b07"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.RangeEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25723) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ea1ea58bfc72b2e"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.VolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25714) | The number of frames to smooth over volume changes. Defaults to 0 in which case no smoothing is used. |
| <a id="api-ff647b2a27fc8285"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_ASYNC = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25792) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_ASYNC<br>常量：`4` |
| <a id="api-c9ad773ad49fe437"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_DECODE = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25788) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_DECODE<br>常量：`2` |
| <a id="api-e4d451f4826ce3a7"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_LOOPING = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25804) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_LOOPING<br>常量：`32` |
| <a id="api-be0095f192070fde"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_DEFAULT_ATTACHMENT = 4096`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25808) | Do not attach to the endpoint by default. Useful for when setting up nodes in a complex graph system.<br>常量：`4096` |
| <a id="api-e49eb0e67ca6f745"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_PITCH = 8192`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25812) | Disable pitch shifting with ma_sound_set_pitch() and ma_sound_group_set_pitch(). This is an optimization.<br>常量：`8192` |
| <a id="api-8a8b40dd7861c346"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_NO_SPATIALIZATION = 16384`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25816) | Disable spatialization.<br>常量：`16384` |
| <a id="api-b4f5616b37a053c7"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_STREAM = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25784) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_STREAM<br>常量：`1` |
| <a id="api-19e39ef2a21cc7de"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_UNKNOWN_LENGTH = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25800) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_UNKNOWN_LENGTH<br>常量：`16` |
| <a id="api-cee24160869137b4"></a>[`Inno.Native.MiniAudio.MaSoundFlags.MA_SOUND_FLAG_WAIT_INIT = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25796) | MA_RESOURCE_MANAGER_DATA_SOURCE_FLAG_WAIT_INIT<br>常量：`8` |
| <a id="api-5dc8cfb5854878db"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaSoundInlined.PNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25832) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df5b480286c98190"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaSoundInlined.PPrev`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25834) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c21a86d03c3073d5"></a>[`public Inno.Native.MiniAudio.MaSound Inno.Native.MiniAudio.MaSoundInlined.Sound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25830) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d74718f7b8d4a19"></a>[`public Inno.Native.MiniAudio.MaAttenuationModel Inno.Native.MiniAudio.MaSpatializer.AttenuationModel`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25861) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c9e0b4d8e036b70d"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25855) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eb46a4dbf3689d4b"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25857) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c618d42947a4077"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25880) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-02b02a210b4321f6"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25882) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-08a34eaaba8ead5b"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25884) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-76a21b6b3ac22ed1"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializer.Direction`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25903) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-38ff734e67c5dd13"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DirectionalAttenuationFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25894) | Set to 0 to disable directional attenuation. |
| <a id="api-653fb6193cabf131"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DopplerFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25889) | Set to 0 to disable doppler effect. |
| <a id="api-cc6d280aaee13191"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DopplerPitch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25913) | Will be updated by ma_spatializer_process_pcm_frames() and can be used by higher level functions to apply a pitch shift for doppler effect. |
| <a id="api-9d4e52cb91c692cb"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25899) | When the gain of a channel changes during spatialization, the transition will be linearly interpolated over this number of frames. |
| <a id="api-53d82e6452e16183"></a>[`public Inno.Native.MiniAudio.MaGainer Inno.Native.MiniAudio.MaSpatializer.Gainer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25920) | For smooth gain transitions. |
| <a id="api-e524ccceb71ad05e"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializer.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25868) | Defaults to right. Forward is -1 on the Z axis. In a left handed system, forward is +1 on the Z axis. |
| <a id="api-8ad4f34909216ebb"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MaxDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25876) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5fc86c720ffc25b7"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MaxGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25872) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0b2586e12e812e07"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25874) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d053ac2a8106f3c"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25870) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4f995192538e13e6"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinSpatializationChannelGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25915) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7534e57f293d68f1"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25932) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9c3df018ed68785"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializer.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25859) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5e94ee03b45dc940"></a>[`public void* Inno.Native.MiniAudio.MaSpatializer.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25930) | Memory management. |
| <a id="api-da47297377f2924b"></a>[`public float* Inno.Native.MiniAudio.MaSpatializer.PNewChannelGainsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25925) | An offset of _pHeap. Used by ma_spatializer_process_pcm_frames() to store new channel gains. The number of elements in this array is equal to config.channelsOut. |
| <a id="api-bec7fb9db3dfeedd"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializer.Position`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25901) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ba95edeeea8de436"></a>[`public Inno.Native.MiniAudio.MaPositioning Inno.Native.MiniAudio.MaSpatializer.Positioning`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25863) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9e07e253a615479"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.Rolloff`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25878) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-13555afe3d758ea0"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializer.Velocity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25908) | For doppler effect. |
| <a id="api-c737a162ca4119d6"></a>[`public Inno.Native.MiniAudio.MaAttenuationModel Inno.Native.MiniAudio.MaSpatializerConfig.AttenuationModel`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25959) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ae05b1b2ad9f026a"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25953) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-559ac3ed16fb4494"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25955) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-082a8b9e2ecbc70b"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25978) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-07fd88c28431d01c"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25980) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bed9d2c02da7426b"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25982) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-82fa6d998250622f"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.DirectionalAttenuationFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25992) | Set to 0 to disable directional attenuation. |
| <a id="api-6bb27c99e1679bae"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.DopplerFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25987) | Set to 0 to disable doppler effect. |
| <a id="api-ecc626f535ccbf58"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26002) | When the gain of a channel changes during spatialization, the transition will be linearly interpolated over this number of frames. |
| <a id="api-1df46fe2a4c2abf9"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerConfig.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25966) | Defaults to right. Forward is -1 on the Z axis. In a left handed system, forward is +1 on the Z axis. |
| <a id="api-b83625040edd3efd"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MaxDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25974) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-850f3d0b587a2e55"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MaxGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25970) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bea6f2884b1a20ac"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25972) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-402713492925a7d1"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25968) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-232b31974267e29a"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinSpatializationChannelGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25997) | The minimal scaling factor to apply to channel gains when accounting for the direction of the sound relative to the listener. Must be in the range of 0..1. Smaller values means more aggressive directional panning, larger values means more subtle directional panning. |
| <a id="api-c243919e0f71dd7f"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerConfig.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-78ae6f10da6f2bac"></a>[`public Inno.Native.MiniAudio.MaPositioning Inno.Native.MiniAudio.MaSpatializerConfig.Positioning`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25961) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aa929108b2a53dc4"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.Rolloff`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L25976) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6dcb3bd57d9edc2f"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfig Inno.Native.MiniAudio.MaSpatializerListener.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26023) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-783a161c467354ae"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializerListener.Direction`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26033) | The direction the listener is facing. The world up vector is config.worldUp. |
| <a id="api-443e23fb1ae21611"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListener.IsEnabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26037) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-b9bf6f69d399c426"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListener.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26042) | Memory management. |
| <a id="api-9b9b98dc8255e0fb"></a>[`public void* Inno.Native.MiniAudio.MaSpatializerListener.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26044) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-94c03972c143be1d"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializerListener.Position`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26028) | The absolute position of the listener. |
| <a id="api-97c755ef67bdc92c"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3f Inno.Native.MiniAudio.MaSpatializerListener.Velocity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L26035) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
