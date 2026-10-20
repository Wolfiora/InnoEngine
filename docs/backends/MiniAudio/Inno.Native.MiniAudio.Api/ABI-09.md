# Inno.Native.MiniAudio ABI · 09

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.PInputNode 849c4503541360e3c792e1d2a7c52f20deb130feccbbda50d1afde67ec4461e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.PNext 38aecaf7ab1ce4105ca2bcb2da622d514a1a5a25f6a7d78666461456d8be94f0 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.PNode 81335ed18c5a2e455549abc00fbab96b886090941d7bf010927cb94651077986 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.PPrev aa96aa9c9927494e64f863046f83f01684a7bae9cf713e7b20f7cc6a0d14cbd9 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.RefCount 71a28bd76653ab7ab32d576d607979ed6359895e4aea0b94ff192e76caabd152 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.Volume fb4a864f42904eb16bb9ce45e8ec6f9fd312f4fdbb00e2e6c14db3f834df7eca -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeState.ma_node_state_started b68d9153c12c2e947ddda2dca8ea8a90a9e6b3a185eb820af80855430d914385 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeState.ma_node_state_stopped 35738d0e58e6622bca6e331d287a642f56e2bea9e7584d7c2ad25a1f7cca6455 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeVtable.Flags 9e0948e8445c8730f106b0b99d883be0ad64f8776154ebe75b6592559e645889 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeVtable.InputBusCount 090d05880f4329f109a1eb77357a56a6f62b0b115d2a1df97e7811ae0bd55add -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeVtable.OnGetRequiredInputFrameCount 720402d766534d5e8c2ac3245dc7f79b64599d01d8ff4d6a57dda3e9af38edad -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeVtable.OnProcess 7bef4adbc2de47861a5dc1dd2b40d137eb2020d6221856503c28b998720c69d0 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeVtable.OutputBusCount 162364748078ddd2665200f85dd8b28e65ec672f82730c9a531b8d488f684e0d -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.Config d3191f43ef7894680fe96e3cbb0e12cc175fcf1ea310c3845b5eedc151ced465 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.Ds a678d41110a51c1197e6ea4571c63ddeeaa4f1e7124ce89dd65efe5c4da801ed -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.Lcg 4f47eacbde396b44ff6752e6b309b4e870dc6ae78e01edbe16183a7da1196b5b -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.OwnsHeap 630d46a3fd5200d575f0c3b6cbaae4fdbdcb2e3df5627356dc2abd46525e3bde -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.PHeap 84c2b142a30532d0823cda5a8bc7e1349e7bc089754e3e0d0c15eaf41c9fed22 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoise.State 55378e21ac542c6bbc5e8a7231b713bd100296b20ebb2e708b7e2c1b4674fe97 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousStruct150b5b8daffaf7dd.Accumulation 03a7d03d29f9d4e0e78d1043c9b7834cc9cec3a268ffdefb02398587634fa826 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Accumulation 404bce82ca896ac74c0404983c554f1170c638b0f5d41f8dccd7f7d6dc75dfb4 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Bin d27cc6b606bd8b5a7b92773ffbf72817a469b4ac331f8befa2e0491bca7f0774 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Counter e70a0b9c85d4848e7c22ee62a9cf672af4deecc33ccc4e06f909909d664f5321 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c.Brownian 3fa76b37d09ee14bd6a9fd3f65c2507452a22098cb95840731413a889d286b04 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c.Pink 7aebee7123f7c78bccff5ff17950e14d4e70ce63d8f94abd9c71c5eaa7e5b3c4 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.Amplitude 1b81d4ff58061454460817aa034f3bfd7873264e73cc39ec519783de3e157d2d -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.Channels 7358b76c9c8f205b9355e0a682fb3d54530e1d094e2a790514a16c4afe2ea859 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.DuplicateChannels dc086ecc22043b27f28faeb400ac6869c622e94033391f3a8ba467f89e399d7e -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.Format c3c163f90c4a66160ac15c313e48b7d73abb78c258c18a4974c03aa62dd8bdea -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.Seed 2e1eda191047f2820fc8e8966c4d564262d461b62731ac0596eb03ae59d034b7 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseConfig.Type 72a5673517e33293e877a694cb767207b66d9c4636791d77cf7ec2e6458bacaf -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_brownian 1059b318383f59b2544ff32bafefe27e0c5300a5d721425dcc53da44b525fef9 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_pink 4d7054f88790af6b262c8378656bd23c211aa5fd21d06d332d711ac830825ba7 -->
<!-- api: F:Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_white d854bd4302bbc34deec28075054582567f13f71f0f1d276f853942e89eae7ac8 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2.Bq da50a428fbb86ff2ec07dbe13ea33024712d0a7a8fdf8efd61e240c2a3c0e864 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2Config.Channels 6e918760b26a941828d38a1f15ea6e6228510d7d8b4357071f271beba143a14b -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2Config.Format 7535c82ee8e910f2c00dbe5224327585fb2138aad2758b42fad432a927dda1b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2Config.Frequency c761d7df57eb14388f003b4ba9b055dcaf92256f99c358b52c44802ffba70071 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2Config.Q 8e69170327a67b4d83778310310c69804ff2586e6e95a44b2382d7ee3d03d2b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotch2Config.SampleRate 948a136b78c96e62212a0d968a9c19c96d9528a5511867339aebc09d17afa30f -->
<!-- api: F:Inno.Native.MiniAudio.MaNotchNode.BaseNode 4604bfdbb3843ea7c8ec53f6dfd31c7fb35e6763456b4b659da41b0955c20f2a -->
<!-- api: F:Inno.Native.MiniAudio.MaNotchNode.Notch 8d9178815a67b7ea9dc16a9401ba924d6c622b14efc3548c3a86c63ba80f1162 -->
<!-- api: F:Inno.Native.MiniAudio.MaNotchNodeConfig.NodeConfig d5509c3c2fd39acb6ce8a4a9e7099ed126db5ad837d23a5f386c0d50789916fd -->
<!-- api: F:Inno.Native.MiniAudio.MaNotchNodeConfig.Notch d9bd2b441680d6b1eb2444f74deac1cd9e964379f75ab1a94a12c6ea77823048 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenModeFlags.MA_OPEN_MODE_READ fb60fae370bb81d4b703a3a10da10b61ecf2e45852a10529580541b71c47e125 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenModeFlags.MA_OPEN_MODE_WRITE 849927bf553037a323229d8f0807da30b725e2cde27ff2b4e4383e3b32dc5222 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_camcorder 94f4cbced777af39e9536b48829ede952ba83ec27dcbbb97f01b7795cb0efe7a -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_default e93402d3c6214caac728905dac0bf5296e58cc0ffa5204345c8edda47ab52dfb -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_generic e5ce6de5ff2b3d6c71b52fdcb0f057b69a0d7825f4c03374d0c9f34955bff79e -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_communication 4ba400cafbe0b35ca1aff82987d375cbc6e5ab53baa46012af9ddb69eb7ca97b -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_recognition 04ad341fef55eff46c91e1e44d418cf390c2172f104765106388f11e9a8b406f -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_unprocessed 52f05b31157f07ea2e5e76f060b9e8ff1f2f8bd773c5eb9c6958ea80495e8412 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_alarm 56563e7dbe8496f628c796af58f06fc233be9cd4c6eecd0d41ab092ca7a7a8e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_default 5cf298631089051a7aef54f31b258ef43cc177c21d1c71db2cda182ecd2b44a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_media 8164ce5ac03b95f38af5fe87d2bb52df9f94c57f63fb92cdb4accfe30ffb32c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_notification feb112f5a2da453a863c1afe669f18237e2616367241b6f902836bf472155e96 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_ring b0416e2a38f776fc7e950d55426650b3f2c18593502e44aff179dee10d902e0c -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_system 0facbea13ad59a95f9d35f1b72beb7e85647873faf429c793776b3b3539f5a59 -->
<!-- api: F:Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_voice c930cfda07b979ea20746cba1a5bd61a23168c8518b3cc74c02fc8a32112c5a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBuffer.AbsoluteCursor e45edb2f0271bde8e0d161b6d63a1a5b2902d71e2b057d8125b1a3e8ff72e385 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBuffer.Ds 5d6f8c370a89df15791cc781fc874a88f3a696525e320d060a9d523b545ba664 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBuffer.PCurrent 0ba2920f5b79906852f1c139f76c073d57495ab95dbdb06d222f791f8a9102ec -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBuffer.PData 1850d9363e250bd953400807a61f07c50f290ab10c76049f819f67a829dad862 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBuffer.RelativeCursor ff082ab7bd520d7c6d06e7f6d11761b5f22f1332d2ae2f0fca4ec87a1534559b -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferConfig.PData 292e60a6b408a372a6456e7ac783c518e1cd817387bda8e2310170f6654e4f40 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferData.Channels 5f7600d45900838612b67d603315ebc01dbb4c85f6851e902159d979c739e3a5 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferData.Format e8fb4fefa2492518e2b6e0a31c5b8e35fb3fd9a159404ac0d8c7b2ad0cff72ef -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferData.Head ffa21f058c6ccfc0f76ead3254f55defa0ded09cf83973c0f6897f760c16ede0 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferData.PTail 666e7e26e3483f0fbdaf927882d3a4af1831f5dc5fed2489da4403e3d62b9e13 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferPage.PAudioData 79738aa06c5fd84dd9d86add705c55dd5deb86c43d5385cb341c91e5bd9e1160 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferPage.PNext 612c628c70062aa9ae76d0f6c3994163be3a4e842d39001c7cf3bf3975f6b838 -->
<!-- api: F:Inno.Native.MiniAudio.MaPagedAudioBufferPage.SizeInFrames 6e75c783074e754762a92fcdec7bff04ba6e580f339cc24c02ed532e362bee24 -->
<!-- api: F:Inno.Native.MiniAudio.MaPanMode.ma_pan_mode_balance 0707f805707cd5195f7d8d721cda8aaf667498de4448db862752c469dad1d362 -->
<!-- api: F:Inno.Native.MiniAudio.MaPanMode.ma_pan_mode_pan d4170378578538b56e9e1e7ab320379dbf142e690314681cf3d6a20541dcf590 -->
<!-- api: F:Inno.Native.MiniAudio.MaPanner.Channels 2379f7c2eb957b7ea22bccdd6c704873c8aeb579826206a53996189b6b9999de -->
<!-- api: F:Inno.Native.MiniAudio.MaPanner.Format 0c3cbec39b0c5de25440f2d3aff7a419a480123011cb1d61defc0073f3e1f07f -->
<!-- api: F:Inno.Native.MiniAudio.MaPanner.Mode 29369aaa6ac100b3595a60bcca2b5209608c2d02dd77eac9e0f930aa288046d8 -->
<!-- api: F:Inno.Native.MiniAudio.MaPanner.Pan 7feb6e50016b4815385015e7609d3bf3a925a81cc6cc90fbc532cd2e2a40eaa5 -->
<!-- api: F:Inno.Native.MiniAudio.MaPannerConfig.Channels c55e464ac39a278040b3de4f30c2556f41b4e3da334dc1372542db6e193ce155 -->
<!-- api: F:Inno.Native.MiniAudio.MaPannerConfig.Format d62e66b74ef9a77525cc804d906595632fb57db78371f4f72ca2172da5748eaa -->
<!-- api: F:Inno.Native.MiniAudio.MaPannerConfig.Mode 688ca29f3959bee7a060ce9d26cd266e5f53b8be5260be590c48f25e52a80dee -->
<!-- api: F:Inno.Native.MiniAudio.MaPannerConfig.Pan 4231bfcd57fa88a5c922fc0540974bafca51e64a6249c0065559446b079cf625 -->
<!-- api: F:Inno.Native.MiniAudio.MaPcmRb.Channels 0fe8cc8b30214bf492811059e984d6b89bd53af41e8e05ac8355b0736d1fb0bc -->
<!-- api: F:Inno.Native.MiniAudio.MaPcmRb.Ds 4bcf85d8c639cd5d28503398864c87119cbda914b9356ba0b438e9dcbde1a754 -->
<!-- api: F:Inno.Native.MiniAudio.MaPcmRb.Format e8435db385153af66ed3aae7803532a6fdea5387916a85bbbb12a11383e85e32 -->
<!-- api: F:Inno.Native.MiniAudio.MaPcmRb.Rb 97553d19f8010ba859560537e62a0e7c4712fc201bd197570268aa44a1c0bb64 -->
<!-- api: F:Inno.Native.MiniAudio.MaPcmRb.SampleRate 39c4c65e720cec6139e1c4aa491e1c8d0367e25d7ae0f4e08470a7228970e3cd -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2.Bq ab32cdfbace163a057d8ba8fe7b125bda1787b2923167e6721fa3bf88c069916 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.Channels 056b097489fab3741af609151d827971e42e988173e757b8e04471ba685a20d9 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.Format 9d3b31f5f15014449de525985548d9ed249839fe244d78c1dac76692ccd3ddf4 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.Frequency 26cc9de89e7023f6ad06e6aa09f2d1dd2fa1c2d4190d61c8c560ad12b9aa826d -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.GainDb cee2485a74e2f37b8925fb00d6124171b046161a6e1bee48c9c4add6d3109ab6 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.Q 9020d9d1f3e52e91ea2b607b50b3e3c0733d265b45f88f8e3eca048e12c7cd05 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeak2Config.SampleRate d26fca1ba54395f466e4a052e1781b8080af8b2fecff3fb9abec75f0eed7ad9a -->
<!-- api: F:Inno.Native.MiniAudio.MaPeakNode.BaseNode d4306576bbc33a6bf28092bf429a9083f1b60bafb63c5b793224d89599d2880b -->
<!-- api: F:Inno.Native.MiniAudio.MaPeakNode.Peak fea7df9081727859a32b7b182d8101fd514ea8d51cc111f634b69f5b63828008 -->
<!-- api: F:Inno.Native.MiniAudio.MaPeakNodeConfig.NodeConfig 5d09b22d18a8a9ffa98af3226a5c8b091796b0e3d10e44fafadc8770abf365de -->
<!-- api: F:Inno.Native.MiniAudio.MaPeakNodeConfig.Peak 56b205159a37612a8060d7e6de17d4a567c09126eb36ddec35ca01062b3a4640 -->
<!-- api: F:Inno.Native.MiniAudio.MaPerformanceProfile.ma_performance_profile_conservative 66b9f777673313465b470583b8eb648d47f4b358b63d2a94b041339eeb4b390b -->
<!-- api: F:Inno.Native.MiniAudio.MaPerformanceProfile.ma_performance_profile_low_latency 1c78515cff92cfe917de293c6bbd9a2ebc7450618302cba5f2f01ce0d803a25c -->
<!-- api: F:Inno.Native.MiniAudio.MaPositioning.ma_positioning_absolute b25bb4d7129dfe5254e96acdaea1cb914929f3ec953ada9f2100445e78361ae4 -->
<!-- api: F:Inno.Native.MiniAudio.MaPositioning.ma_positioning_relative ae45466cb62e7f8105f5054f40bd0327120e85266ce5258aade253dc625b894d -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewave.Config f6f409870e4b146ca4e4c91c5be5c6ef0ca4114d283c11e1578310ca0ef91ac4 -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewave.Waveform d3b36ae455bb411230ea4194d0710995478d6dfbcf783200f78a16caeaf0efde -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.Amplitude d1aa1ab283215c5ad3f6ca6a3f10bc269ddb620f99a217ba5621952a942ed545 -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.Channels 090273be4a1d4da5c3aaeb5da09c28b23f4a3de074c8acf8773b3c371ba0badf -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.DutyCycle 67efe553f4bf27f34f33c5c612afe9a74b000db6f023f839ec5d9fde79de9fa3 -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.Format f13e8dd4e0ecb54e2dd2670c61e6c0b1f9294263b2c27466b92ce58aaebe3ac8 -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.Frequency 17ecf44ac5cda670663ed8884905a61297d65a7adc54e648a3c56fc1e6272aee -->
<!-- api: F:Inno.Native.MiniAudio.MaPulsewaveConfig.SampleRate 7f5d8c234d90b856a5b35dc8774a5086899a7f632332959e5fad5153f93ae55e -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.AllocationCallbacks 805ed7623c7f5165986900e14dea72dc2598f24890036c62b799ef30d04310d9 -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.ClearOnWriteAcquire 4b92c687e5a46fc33a30f2b309158981655871aced9b6f1324cd2dc9cff8f80b -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.EncodedReadOffset df02be9496c115fbdadc8538d1d5221e5ee568d9e3f6af9952ee656c262af92b -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.EncodedWriteOffset d8343f8def903749668da73fb2c3b4172cb9c4332f74c77d1046ca25b7c65898 -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.OwnsBuffer d1b9218d65c6bbe036a6fcb175c5926fc03bb0a8cbb9c4cabd7f36eedfe66828 -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.PBuffer 3397d3321a7da3e60d39bf009e09be00e0cec02d97013e1ae4d02d72c5e75cd5 -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.SubbufferCount cfe2d1a18131c3cb64346afa83127292321948930621f47b37f761d68a63bfe1 -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.SubbufferSizeInBytes 12c57691bbad951c43699cd43285704295aeb2e6b9f186b84c4340476f392f1f -->
<!-- api: F:Inno.Native.MiniAudio.MaRb.SubbufferStrideInBytes 03d5675f2070068a134f496df1a5e9c19cc87c5387da33956a7c69fd328d0ec7 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampleAlgorithm.ma_resample_algorithm_custom deebf55209859e3c43ce8a97a5d7ca8d41c7a34cc7302f3d4a57cb875411da47 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampleAlgorithm.ma_resample_algorithm_linear 2a88c8f0efc915ceb377412dbf0fe90e323a5d2815d397f177a6547a7a41e2ec -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.Channels 406000a88cf48d2d4ca12997a09639500a990ababc7768f5a78c6ed378fac908 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.Format ca528a1bac44d3b355c329f0ff0a16b86a153a84af9c4d51f7fc8b6538570c4a -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.OwnsHeap d003a01bce7d2f41192412ba3fe3806005c7c63f4a35820b484c2b3705e1d8d5 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.PBackend e0c96339cd5ac31e7fb1b276c202a05ab00e97548311d9ebca750265578628d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.PBackendUserData 001c87411d1abd0d08ac2bdb8727cb8063aa25bd1b89385d90ec285328548daa -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.PBackendVTable 025e339d75daf58b5328dd93a92e62dbe897d2bb8e15933b816061e577235228 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.PHeap 81db9b0ea8501d697d66820f074ce8f1667fa9cecacb6fd00f4dc250cd7919c3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.SampleRateIn 253bf87dcbd9c8bf0eec36d6b6759879c0bf1db98d445baa5b917c36eec0a6a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.SampleRateOut e44e398dcc5bea34b1dde2a028ec4dbdb2ddb8b2610319b75b7d09026c942cfe -->
<!-- api: F:Inno.Native.MiniAudio.MaResampler.State e3f679e902800c644e9bb5db4f7491219cb94d02455cdfc79e5f97bef21c2413 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerAnonymousUnion67c9af061924ab62.Linear 9448a1c4bac8bd34a4be36e48bd8c42a6f4850bacb485a2868028ffeff39e977 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.Algorithm ce94c5721c2752a88de86a1def6fc8578f8e8c609edcd05d96ee260bddf222cf -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.Channels 49974ed88de672ca9041144c9ddba79394fa39fb653061725738d2987825b873 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.Format b407fabe5585e7605d1b61ce1b6243abf39964a23eb05138663e63b89e1b1c96 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.Linear 9a9423c7575dd27b1b1b65c20c9cffd37f583d06d677339887fca336fb235f74 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.PBackendUserData 39100a24782255ae242531a5cf1811df31929097e62d63001878ffe2652c08a5 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.PBackendVTable a2544102800a92e8a611c85dd3abb7348faf3f25335dfa66bf483cf100a2eaa3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.SampleRateIn 9296fdc902dde213acd6edb3a70db8bb07bef6ade101b9e45b6e35f621aad592 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfig.SampleRateOut c23fe6ea9a9fde00b089ecd0388b935975a9b06d831072935123401690c25884 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplerConfigAnonymousStruct816361d296299c5c.LpfOrder 81953220bbbfe8d9bc5a21af5e8e0e93ce3435fcb9d00f468c3028c0c82b7295 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetExpectedOutputFrameCount aef0fae6450fc82fcfe25e649b7168e24e8ae214a3b4d37c9c8d5d4ba8fbb788 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetHeapSize c83a8f33b2addd2d3241a046704622312e0e425c2e25531d38c8ef934050d6a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetInputLatency b96154b35b6708724c70d5cbf334f69400ba0297fc525f371215cac689d7192f -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetOutputLatency f377414a28a8142fef48d6b57eadcfade154d0f38a8ea1ea03ba312684fd3d13 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetRequiredInputFrameCount 010bbb487787aaed817da1bcd67782e90a6861e238e3c29b30120d97fa629338 -->
<!-- api: F:Inno.Native.MiniAudio.MaResamplingBackendVtable.OnInit 747f9459d0483b7ca561001828807355dcb78c40dff904e0d26489d404850d35 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-d8eb29ee578d552c"></a>[`public void* Inno.Native.MiniAudio.MaNodeOutputBus.PInputNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23471) | The node that this output bus is attached to. Required for detaching. |
| <a id="api-8aebb25142946593"></a>[`public Inno.Native.MiniAudio.MaNodeOutputBus* Inno.Native.MiniAudio.MaNodeOutputBus.PNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23461) | If null, it's the tail node or detached. |
| <a id="api-34109d78c717fbe3"></a>[`public void* Inno.Native.MiniAudio.MaNodeOutputBus.PNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23416) | The node that owns this output bus. The input node. Will be null for dummy head and tail nodes. |
| <a id="api-b54522301a78fe30"></a>[`public Inno.Native.MiniAudio.MaNodeOutputBus* Inno.Native.MiniAudio.MaNodeOutputBus.PPrev`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23466) | If null, it's the head node or detached. |
| <a id="api-6dd2b6bc8414835f"></a>[`public uint Inno.Native.MiniAudio.MaNodeOutputBus.RefCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23441) | Reference count for some thread-safety when detaching. |
| <a id="api-0fbe5a189d219d58"></a>[`public float Inno.Native.MiniAudio.MaNodeOutputBus.Volume`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23456) | Linear. |
| <a id="api-824e3ef6190403bd"></a>[`Inno.Native.MiniAudio.MaNodeState.ma_node_state_started = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23492) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-19ca0973bbf41a50"></a>[`Inno.Native.MiniAudio.MaNodeState.ma_node_state_stopped = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23493) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-a0b594fc64e6e45d"></a>[`public uint Inno.Native.MiniAudio.MaNodeVtable.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23543) | Flags describing characteristics of the node. This is currently just a placeholder for some ideas for later on. |
| <a id="api-f8b3f4f8a1c3299f"></a>[`public byte Inno.Native.MiniAudio.MaNodeVtable.InputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23531) | The number of input buses. This is how many sub-buffers will be contained in the `ppFramesIn` parameters of the callbacks above. |
| <a id="api-52c9c7c728e623cf"></a>[`public delegate* unmanaged[Cdecl]<void*, uint, uint*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaNodeVtable.OnGetRequiredInputFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23525) | A callback for retrieving the number of input frames that are required to output the specified number of output frames. You would only want to implement this when the node performs resampling. This is optional, even for nodes that perform resampling, but it does offer a small reduction in latency as it allows miniaudio to calculate the exact number of input frames to read at a time instead of having to estimate. |
| <a id="api-632d0a79a7881412"></a>[`public delegate* unmanaged[Cdecl]<void*, float**, uint*, float**, uint*, void> Inno.Native.MiniAudio.MaNodeVtable.OnProcess`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23516) | Extended processing callback. This callback is used for effects that process input and output at different rates (i.e. they perform resampling). This is similar to the simple version, only they take two separate frame counts: one for input, and one for output. On input, `pFrameCountOut` is equal to the capacity of the output buffer for each bus, whereas `pFrameCountIn` will be equal to the number of PCM frames in each of the buffers in `ppFramesIn`. On output, set `pFrameCountOut` to the number of PCM frames that were actually output and set `pFrameCountIn` to the number of input frames that were consumed. |
| <a id="api-f7bad9af76491e23"></a>[`public byte Inno.Native.MiniAudio.MaNodeVtable.OutputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23537) | The number of output buses. This is how many sub-buffers will be contained in the `ppFramesOut` parameters of the callbacks above. |
| <a id="api-3272c1ff1e07d6e0"></a>[`public Inno.Native.MiniAudio.MaNoiseConfig Inno.Native.MiniAudio.MaNoise.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23566) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2f8c6fe35570650f"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaNoise.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23564) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cff73585c0b89e5d"></a>[`public Inno.Native.MiniAudio.MaLcg Inno.Native.MiniAudio.MaNoise.Lcg`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23568) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-441049842c8d418f"></a>[`public uint Inno.Native.MiniAudio.MaNoise.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23577) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-71930099b356cd96"></a>[`public void* Inno.Native.MiniAudio.MaNoise.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23575) | Memory management. |
| <a id="api-d499b948439bc0e9"></a>[`public Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c Inno.Native.MiniAudio.MaNoise.State`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23570) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-03c03a5b3ebd9a17"></a>[`public double* Inno.Native.MiniAudio.MaNoiseAnonymousStruct150b5b8daffaf7dd.Accumulation`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23598) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e9de377265f058f5"></a>[`public double* Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Accumulation`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23621) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-421e84f5100f9c25"></a>[`public double** Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Bin`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23619) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-075f5e5089ac15e4"></a>[`public uint* Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8.Counter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23623) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cf129a12f0414d1a"></a>[`public Inno.Native.MiniAudio.MaNoiseAnonymousStruct150b5b8daffaf7dd Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c.Brownian`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23646) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3efd38be1589dfad"></a>[`public Inno.Native.MiniAudio.MaNoiseAnonymousStruct829bbf4dea8396a8 Inno.Native.MiniAudio.MaNoiseAnonymousUnionDdcc25dc73447b3c.Pink`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23644) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-05bd5997ff3c49aa"></a>[`public double Inno.Native.MiniAudio.MaNoiseConfig.Amplitude`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23675) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb8bfa73b1200f0e"></a>[`public uint Inno.Native.MiniAudio.MaNoiseConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23669) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2ec38d2c73820d32"></a>[`public uint Inno.Native.MiniAudio.MaNoiseConfig.DuplicateChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23677) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3a291f47ee013ce8"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaNoiseConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23667) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-4810315712c28ee0"></a>[`public int Inno.Native.MiniAudio.MaNoiseConfig.Seed`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23673) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c7a6ad465085947e"></a>[`public Inno.Native.MiniAudio.MaNoiseType Inno.Native.MiniAudio.MaNoiseConfig.Type`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23671) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21356d632e662b55"></a>[`Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_brownian = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23697) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-2c22235d23c34abb"></a>[`Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_pink = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23696) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-1acf61410eb12b7a"></a>[`Inno.Native.MiniAudio.MaNoiseType.ma_noise_type_white = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23695) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-f037cb6290ff8e24"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaNotch2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23711) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-19b6549d7fe20601"></a>[`public uint Inno.Native.MiniAudio.MaNotch2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23739) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-68eab5aa338867f3"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaNotch2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23737) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd0d90c1961e391f"></a>[`public double Inno.Native.MiniAudio.MaNotch2Config.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23745) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eaf8e37539a97c12"></a>[`public double Inno.Native.MiniAudio.MaNotch2Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23743) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0688abd49383245d"></a>[`public uint Inno.Native.MiniAudio.MaNotch2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23741) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-91cfe2ee0c5f7871"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaNotchNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23766) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-f2d1f3d88ae3c6f3"></a>[`public Inno.Native.MiniAudio.MaNotch2 Inno.Native.MiniAudio.MaNotchNode.Notch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23768) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-eeb12d6ed1d51e0f"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaNotchNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23792) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-6d413735a1be786a"></a>[`public Inno.Native.MiniAudio.MaNotch2Config Inno.Native.MiniAudio.MaNotchNodeConfig.Notch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23794) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5c008fae7f30605c"></a>[`Inno.Native.MiniAudio.MaOpenModeFlags.MA_OPEN_MODE_READ = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23812) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-4abfa99d0f4b1c20"></a>[`Inno.Native.MiniAudio.MaOpenModeFlags.MA_OPEN_MODE_WRITE = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23813) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-7767388fc872dc1a"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_camcorder = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23838) | SL_ANDROID_RECORDING_PRESET_CAMCORDER<br>常量：`2` |
| <a id="api-b3f6c8d0412e3d33"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23830) | Leaves the input preset unset.<br>常量：`0` |
| <a id="api-43493a47d29177a0"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_generic = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23834) | SL_ANDROID_RECORDING_PRESET_GENERIC<br>常量：`1` |
| <a id="api-98d0838712d20d6b"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_communication = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23846) | SL_ANDROID_RECORDING_PRESET_VOICE_COMMUNICATION<br>常量：`4` |
| <a id="api-492fdbd09982f504"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_recognition = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23842) | SL_ANDROID_RECORDING_PRESET_VOICE_RECOGNITION<br>常量：`3` |
| <a id="api-645727550ffe1bbe"></a>[`Inno.Native.MiniAudio.MaOpenslRecordingPreset.ma_opensl_recording_preset_voice_unprocessed = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23850) | SL_ANDROID_RECORDING_PRESET_UNPROCESSED<br>常量：`5` |
| <a id="api-3d1ab90a05bcc168"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_alarm = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23887) | SL_ANDROID_STREAM_ALARM<br>常量：`5` |
| <a id="api-b50075bdc809897b"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23867) | Leaves the stream type unset.<br>常量：`0` |
| <a id="api-b7a84fbcebcf99e5"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_media = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23883) | SL_ANDROID_STREAM_MEDIA<br>常量：`4` |
| <a id="api-bc1eb3659e94b9e1"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_notification = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23891) | SL_ANDROID_STREAM_NOTIFICATION<br>常量：`6` |
| <a id="api-dd7ea5a472c91599"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_ring = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23879) | SL_ANDROID_STREAM_RING<br>常量：`3` |
| <a id="api-48f95e7e182b8403"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_system = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23875) | SL_ANDROID_STREAM_SYSTEM<br>常量：`2` |
| <a id="api-f34b65f1e0dcd3d6"></a>[`Inno.Native.MiniAudio.MaOpenslStreamType.ma_opensl_stream_type_voice = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23871) | SL_ANDROID_STREAM_VOICE<br>常量：`1` |
| <a id="api-8d62e0dd6c3f1eab"></a>[`public ulong Inno.Native.MiniAudio.MaPagedAudioBuffer.AbsoluteCursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23919) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-95b1a216f1612069"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaPagedAudioBuffer.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23905) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-42b703295fc27052"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferPage* Inno.Native.MiniAudio.MaPagedAudioBuffer.PCurrent`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23912) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0d4d1c84de21ccae"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferData* Inno.Native.MiniAudio.MaPagedAudioBuffer.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23910) | Audio data is read from here. Cannot be null. |
| <a id="api-7ba841d95e32b0d0"></a>[`public ulong Inno.Native.MiniAudio.MaPagedAudioBuffer.RelativeCursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23917) | Relative to the current page. |
| <a id="api-70808cace48d93d7"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferData* Inno.Native.MiniAudio.MaPagedAudioBufferConfig.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23943) | Must not be null. |
| <a id="api-0d783428db23005e"></a>[`public uint Inno.Native.MiniAudio.MaPagedAudioBufferData.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23966) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c20bdcfd2049e3fa"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPagedAudioBufferData.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23964) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e71674c1fcdffc9f"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferPage Inno.Native.MiniAudio.MaPagedAudioBufferData.Head`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23971) | Dummy head for the lock-free algorithm. Always has a size of 0. |
| <a id="api-02530d858ee01bfd"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferPage* Inno.Native.MiniAudio.MaPagedAudioBufferData.PTail`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23978) | Never null. Initially set to &head . |
| <a id="api-0cb9b0667dd09a09"></a>[`public Inno.Native.MiniAudio.NativeArray_ded1899ba091cc15 Inno.Native.MiniAudio.MaPagedAudioBufferPage.PAudioData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24003) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bcba86889c3f729c"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferPage* Inno.Native.MiniAudio.MaPagedAudioBufferPage.PNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L23999) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8ba09fb04ba70d73"></a>[`public ulong Inno.Native.MiniAudio.MaPagedAudioBufferPage.SizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24001) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-d3cccd10b4a8fe0e"></a>[`Inno.Native.MiniAudio.MaPanMode.ma_pan_mode_balance = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24027) | Does not blend one side with the other. Technically just a balance. Compatible with other popular audio engines and therefore the default.<br>常量：`0` |
| <a id="api-193d59e12d0a0469"></a>[`Inno.Native.MiniAudio.MaPanMode.ma_pan_mode_pan = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24031) | A true pan. The sound from one side will "move" to the other side and blend with it.<br>常量：`1` |
| <a id="api-81140bad5ec37cf5"></a>[`public uint Inno.Native.MiniAudio.MaPanner.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24047) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-81cd95c223c6fc8e"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPanner.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24045) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0376b7e99580d7ad"></a>[`public Inno.Native.MiniAudio.MaPanMode Inno.Native.MiniAudio.MaPanner.Mode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24049) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-552ecd706190187a"></a>[`public float Inno.Native.MiniAudio.MaPanner.Pan`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24054) | -1..1 where 0 is no pan, -1 is left side, +1 is right side. Defaults to 0. |
| <a id="api-32cb8eaa4bfc7522"></a>[`public uint Inno.Native.MiniAudio.MaPannerConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24077) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-0a718b2a420be923"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPannerConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24075) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1bbd6f2da4138b7b"></a>[`public Inno.Native.MiniAudio.MaPanMode Inno.Native.MiniAudio.MaPannerConfig.Mode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24079) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e4cd4dec0ff95b2e"></a>[`public float Inno.Native.MiniAudio.MaPannerConfig.Pan`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24081) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-198ce55e5ed6771b"></a>[`public uint Inno.Native.MiniAudio.MaPcmRb.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24108) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c4b2e94be849328d"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaPcmRb.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24102) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e41587721024e958"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPcmRb.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24106) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-63586ab16d1fa3c9"></a>[`public Inno.Native.MiniAudio.MaRb Inno.Native.MiniAudio.MaPcmRb.Rb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24104) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c0207f24e80f87ca"></a>[`public uint Inno.Native.MiniAudio.MaPcmRb.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24113) | Not required for the ring buffer itself, but useful for associating the data with some sample rate, particularly for data sources. |
| <a id="api-ab6ff0951ecad73d"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaPeak2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24134) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3c834c4eba2c5521"></a>[`public uint Inno.Native.MiniAudio.MaPeak2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24162) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83c510a03b36c666"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPeak2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24160) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-06256c7c810845e8"></a>[`public double Inno.Native.MiniAudio.MaPeak2Config.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24170) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-706d7bbf93b50011"></a>[`public double Inno.Native.MiniAudio.MaPeak2Config.GainDb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24166) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-21f6f62f7ed9ddb6"></a>[`public double Inno.Native.MiniAudio.MaPeak2Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24168) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3ed5390f749941c2"></a>[`public uint Inno.Native.MiniAudio.MaPeak2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24164) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7e7f59ab073d2a04"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaPeakNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24191) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-83b8df4109e05170"></a>[`public Inno.Native.MiniAudio.MaPeak2 Inno.Native.MiniAudio.MaPeakNode.Peak`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24193) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-198a9475897819bc"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaPeakNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24217) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-294a09b2c4cda6e3"></a>[`public Inno.Native.MiniAudio.MaPeak2Config Inno.Native.MiniAudio.MaPeakNodeConfig.Peak`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24219) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-52a7a1625a55786e"></a>[`Inno.Native.MiniAudio.MaPerformanceProfile.ma_performance_profile_conservative = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24238) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-91209a465ff539d1"></a>[`Inno.Native.MiniAudio.MaPerformanceProfile.ma_performance_profile_low_latency = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24237) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-8b65f32686be344c"></a>[`Inno.Native.MiniAudio.MaPositioning.ma_positioning_absolute = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24249) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-b72cec77d96b1086"></a>[`Inno.Native.MiniAudio.MaPositioning.ma_positioning_relative = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24250) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-7bf133869b78b648"></a>[`public Inno.Native.MiniAudio.MaPulsewaveConfig Inno.Native.MiniAudio.MaPulsewave.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24266) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-97b8caeea88bc4f0"></a>[`public Inno.Native.MiniAudio.MaWaveform Inno.Native.MiniAudio.MaPulsewave.Waveform`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24264) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1903c6e9dc82fab8"></a>[`public double Inno.Native.MiniAudio.MaPulsewaveConfig.Amplitude`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24295) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-48d202ad5386efa2"></a>[`public uint Inno.Native.MiniAudio.MaPulsewaveConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24289) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a19fcc049f86d4eb"></a>[`public double Inno.Native.MiniAudio.MaPulsewaveConfig.DutyCycle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24293) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cc415b8ac21c2fe6"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaPulsewaveConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24287) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a91a48555e6b30e0"></a>[`public double Inno.Native.MiniAudio.MaPulsewaveConfig.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24297) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-54b2b7a7cd94bfa8"></a>[`public uint Inno.Native.MiniAudio.MaPulsewaveConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24291) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c00e2446ea7570d5"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaRb.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-66259114ac98674a"></a>[`public byte Inno.Native.MiniAudio.MaRb.ClearOnWriteAcquire`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24349) | When set, clears the acquired write buffer before returning from ma_rb_acquire_write(). |
| <a id="api-707c5c2fcc1ecaea"></a>[`public uint Inno.Native.MiniAudio.MaRb.EncodedReadOffset`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24334) | Most significant bit is the loop flag. Lower 31 bits contains the actual offset in bytes. Must be used atomically. |
| <a id="api-f20ee8b805ac2c95"></a>[`public uint Inno.Native.MiniAudio.MaRb.EncodedWriteOffset`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24339) | Most significant bit is the loop flag. Lower 31 bits contains the actual offset in bytes. Must be used atomically. |
| <a id="api-a3839e9a3fed5d13"></a>[`public byte Inno.Native.MiniAudio.MaRb.OwnsBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24344) | Used to know whether or not miniaudio is responsible for free()-ing the buffer. |
| <a id="api-3bf8de3a975d46af"></a>[`public void* Inno.Native.MiniAudio.MaRb.PBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24323) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bb4cd8c32a20849b"></a>[`public uint Inno.Native.MiniAudio.MaRb.SubbufferCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a61904803c11b2d6"></a>[`public uint Inno.Native.MiniAudio.MaRb.SubbufferSizeInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ab8a940c3bceac24"></a>[`public uint Inno.Native.MiniAudio.MaRb.SubbufferStrideInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-93f722df9e9178ff"></a>[`Inno.Native.MiniAudio.MaResampleAlgorithm.ma_resample_algorithm_custom = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24382) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-d5922c056e07f683"></a>[`Inno.Native.MiniAudio.MaResampleAlgorithm.ma_resample_algorithm_linear = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24381) | Fastest, lowest quality. Optional low-pass filtering. Default.<br>常量：`0` |
| <a id="api-e0623dbc8ea9580d"></a>[`public uint Inno.Native.MiniAudio.MaResampler.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24404) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-27f7c7c3451ff1de"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaResampler.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24402) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8e347064ec41688e"></a>[`public uint Inno.Native.MiniAudio.MaResampler.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24420) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-86479ce2e312a385"></a>[`public void* Inno.Native.MiniAudio.MaResampler.PBackend`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24396) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c1c230c1ae1f2cc0"></a>[`public void* Inno.Native.MiniAudio.MaResampler.PBackendUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24400) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5627bb3ad1d59aaa"></a>[`public Inno.Native.MiniAudio.MaResamplingBackendVtable* Inno.Native.MiniAudio.MaResampler.PBackendVTable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24398) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43b747feccfa0d35"></a>[`public void* Inno.Native.MiniAudio.MaResampler.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24418) | Memory management. |
| <a id="api-86a8554afa1106f0"></a>[`public uint Inno.Native.MiniAudio.MaResampler.SampleRateIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24406) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-c69cc418eae049c6"></a>[`public uint Inno.Native.MiniAudio.MaResampler.SampleRateOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24408) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8d29520677184286"></a>[`public Inno.Native.MiniAudio.MaResamplerAnonymousUnion67c9af061924ab62 Inno.Native.MiniAudio.MaResampler.State`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24413) | State for stock resamplers so we can avoid a malloc. For stock resamplers, pBackend will point here. |
| <a id="api-9ef706def879a268"></a>[`public Inno.Native.MiniAudio.MaLinearResampler Inno.Native.MiniAudio.MaResamplerAnonymousUnion67c9af061924ab62.Linear`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24441) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-daece72cecb45e94"></a>[`public Inno.Native.MiniAudio.MaResampleAlgorithm Inno.Native.MiniAudio.MaResamplerConfig.Algorithm`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24476) | When set to ma_resample_algorithm_custom, pBackendVTable will be used. |
| <a id="api-7a77c2b8cee539d9"></a>[`public uint Inno.Native.MiniAudio.MaResamplerConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24467) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a4ef9b4d3e195967"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaResamplerConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24465) | Must be either ma_format_f32 or ma_format_s16. |
| <a id="api-e81b4837edd8ed0c"></a>[`public Inno.Native.MiniAudio.MaResamplerConfigAnonymousStruct816361d296299c5c Inno.Native.MiniAudio.MaResamplerConfig.Linear`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24482) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-7ca8a97aa600b141"></a>[`public void* Inno.Native.MiniAudio.MaResamplerConfig.PBackendUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-8bfb01999a7e7485"></a>[`public Inno.Native.MiniAudio.MaResamplingBackendVtable* Inno.Native.MiniAudio.MaResamplerConfig.PBackendVTable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24478) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1728a261fb3723d9"></a>[`public uint Inno.Native.MiniAudio.MaResamplerConfig.SampleRateIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24469) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-bd5bc023a13e320a"></a>[`public uint Inno.Native.MiniAudio.MaResamplerConfig.SampleRateOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24471) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-43dde1b08f721c9e"></a>[`public uint Inno.Native.MiniAudio.MaResamplerConfigAnonymousStruct816361d296299c5c.LpfOrder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24503) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1d1acae301a12715"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, ulong*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetExpectedOutputFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24555) | Optional. Latency mitigation will be disabled. |
| <a id="api-bc0fa6ada2a0d585"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.MiniAudio.MaResamplerConfig*, ulong*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetHeapSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24524) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5975c4b417cf3c00"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetInputLatency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24540) | Optional. Latency will be reported as 0. |
| <a id="api-c0000cc43bcd0541"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetOutputLatency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24545) | Optional. Latency will be reported as 0. |
| <a id="api-62ba0d4ae41c9dc5"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, ulong*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnGetRequiredInputFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24550) | Optional. Latency mitigation will be disabled. |
| <a id="api-5d331a947dc8a066"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.MiniAudio.MaResamplerConfig*, void*, void**, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaResamplingBackendVtable.OnInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24526) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
