# Inno.Native.MiniAudio ABI · 12

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ChannelsOut d3c2b221a6a87d428cf5eb8ccca930621a43a4f6f226e861f3ce559223594beb -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeInnerAngleInRadians b64d8dd34ca54886dd33275b5f36b3b7704fd3fef1755493ca595582817dbfed -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterAngleInRadians 15bfe2c2ad68cf29b6085db7c36be0a45e62de4e2d11d971d6bea41b735b5c1a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterGain 6080d8b1d74db971eccb68110e699217573158f7485772b5f9b6ae782538ade3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.Handedness 2a6ccc5c4db25800d468c42ab9aa418af0d4556e0d0f48a73b6bed0bae680f51 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.PChannelMapOut 6b95f8b7913bdeb8aa8deb33b161e86361d01c68346a7164cb54c24468731ab6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.SpeedOfSound d38e018eec959c039aab791ad9a99d3bb6706c5d7e57a744de289fb2f6a10a53 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.WorldUp a44afc4a9e38c2a4c08e47feba8f626491a751168c2930333d0c7b839b23ab7d -->
<!-- api: F:Inno.Native.MiniAudio.MaSplitterNode.Base 0d73f1fe013b6005f7805b7612d929ed37dcfb8292a87ccf81d0a7a757b22d5c -->
<!-- api: F:Inno.Native.MiniAudio.MaSplitterNodeConfig.Channels dba4e3ca8fdb509e7fac793cf2c05b7891657c3dfdac60dc540e5f96abc66455 -->
<!-- api: F:Inno.Native.MiniAudio.MaSplitterNodeConfig.NodeConfig 2c569590a102d062e4a884cdb3b7340b753d012c8aa6f27185958a2271df4b52 -->
<!-- api: F:Inno.Native.MiniAudio.MaSplitterNodeConfig.OutputBusCount e88245f59393b30f6d45a0ea24eb1b288f4f7f047893eb119240e5cabf58b50f -->
<!-- api: F:Inno.Native.MiniAudio.MaStack.Data 32426e85ee556f1901852c16134580142d1b6c2151884bcdd9d1499872423915 -->
<!-- api: F:Inno.Native.MiniAudio.MaStack.Offset 4366506599f72ca97d1db277b2ae73fbf0c8b0b0d51a4d6d81eb8ed5eeaedef5 -->
<!-- api: F:Inno.Native.MiniAudio.MaStack.SizeInBytes 7f0f4dadcff133f407fce79ab9e9190ada1f424b628d7e03ac8803342dc88cd8 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_alsa 02eca2dd1026556e714632e35444042382e93f4c00d66d5e649f1060b2f40ee1 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_default 39829a0887a401db390152e1e83d0dbb3640cffad8a0c56ee5a1d6e62262cb8e -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_flac 890c1a3e024e0424e82864b1de8431ac8c8fb1d62d7f4bbdfab2e74d6ea69b61 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_microsoft a769df79b2574243f1e1020ba0b86c882397402b72dc4bb8b0b33a003a8b82f1 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_rfc3551 515a0e280077bbf4aa5998fa9c9880fefe042a84d5567c023b1d97f8606fc2bd -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_sndio bad4f9108b66e6eb9379e716499e701dade0d195df52d2b5f1a558cd8a624837 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_sound4 67f5a7703aa243de5ddcac93301545868cb23bf8e9cc6bb2a8e49b95c6a097e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_vorbis 4373dbbe15c39a4f34af475d19675a3bfb5a5edb512e6b3da9a2681f407c1ec1 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_webaudio 2dd5b88b85c72f3a7462c409208e044ddaec406b9293a0eae561bca9e1ca6d5b -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_11025 72c31e0ebafb9183817e443eb19e51173b69b70a91965c08c00c95c5aa68efab -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_16000 6cbec2f268d59d72e94e51ff6a893eef6663483f36d5e5482600af4f01b60779 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_176400 3b5125972b7971a5c3e5b223b4f19e26937c81f138b6632bd61dd7f1570a0f88 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_192000 f1c225dce8c6860966ff5ef3e679612cb925a4c79971d97777eb178479483e9e -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_22050 e4f815ed91668a748c444501460f2f046cd410a51d68d896aacbacded8f8e780 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_24000 4db4c14feb32a62c3e6fd8bfb2c257a1f312e154ed1b958b69df2207e997eedf -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_32000 ff1ac0e8bd09650a6f852ffd76b0bf85e978b7ca571818fbdf8fb1240a6a3ee2 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_352800 1d270a9d628b65db6cc73bbb6881b863a0286aab94fe3638485a5ebbf4721d88 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_384000 25c06146f600dff14bb45d91b87164b85656e18e12fb2ceb231a15bbc8259c35 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_44100 46114a08574ba126150f196ab6b13c3dce2a51310be0acd6558872f926c3afef -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_48000 d362b9e920b97eac6e1d1e2dddbba36ed85d5576955e6cc97a3747e1757f2940 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_8000 7a9b7fd68f9a15f8609274cb4364e9b02d47749dad9d80a61e14356a92891ecc -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_88200 fccdafc4604a936c08c6b6ffd695214e6946f2cf9e856226d0a978a37ac1ee8f -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_96000 b42500db336b393e8979608856732a12461dd905ab9fa241c7158d3b77496724 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_count d02e7b0ff7a238c34c8eae4935aa6313195543d077f6bd75c3881581307b0286 -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_max 6dff27eb1e1c0734a6e2f98bbdca5a0ce3a80b71ffec65c20f6f2baeed3d65dd -->
<!-- api: F:Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_min d7d91f9be47217fe0c417ec58973d478d4e75026365a1fe74d6c0a1c11bdde1e -->
<!-- api: F:Inno.Native.MiniAudio.MaStreamFormat.ma_stream_format_pcm e7eb65b86283ac258b722798b38a7c3b2d0988b4c6a4d684b448a19a7cc3e98e -->
<!-- api: F:Inno.Native.MiniAudio.MaStreamLayout.ma_stream_layout_deinterleaved 09c38ac890a9595008e7086870e01e6fc8f7bb857345cc00c44a0fbe628ae582 -->
<!-- api: F:Inno.Native.MiniAudio.MaStreamLayout.ma_stream_layout_interleaved de65443d7de6e13b849c5218ed0bb6e59da269952ccb44d9327c6cf476924d56 -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_default a48ca2b082e9503137fbddc45c124697503b506c8c8ffb2eb2ed7a1cc98f3ecf -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_high 84a4ada8f58b58a857ffc745393e5dfa5cabc91f63dfa4431e0bd55667148e15 -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_highest cf561b0d6e8aa32c6e7241d0737b26fbc472f6e7bfbd6716f227fbc7eee61ed0 -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_idle 9801e8f7e2d72a6a2f439b1663ebccd744908d9d85bd1b469e732cc6c3b91ba0 -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_low bc92b51b7d0c62acfc4dd621f9947f8df9174ef495284b61f091e934777a2935 -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_lowest b9123f03c562c9b04df0e353777471fde33b68a2cf8e121f1ef5e5588b95df1e -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_normal 716017c3d8f8760ffa79b4be6d50cad9d557c0bf52909cd6a35953a0a869530a -->
<!-- api: F:Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_realtime c8ad637efd9e0d0c3aa8133a45b04964d78b787a9e075c59c0b42f6ca4973ca4 -->
<!-- api: F:Inno.Native.MiniAudio.MaTimer.Counter 1fde743f8ca3014669b54053564f00fbe925cf2e1050eecc370fdd1af65426bb -->
<!-- api: F:Inno.Native.MiniAudio.MaTimer.CounterD 69ee9958b9e2eae5e7e0e05df3aa2eadf567420f430be5b97333f843480709f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaVec3f.X 937f35a8d786f8e029fdb83d6c448f1152f3c5b842b93c9394f0b8dee0571fdb -->
<!-- api: F:Inno.Native.MiniAudio.MaVec3f.Y e82844d2c16803bc4637cab1198d9e199686bae530ca1cd9af56a677ef52b6eb -->
<!-- api: F:Inno.Native.MiniAudio.MaVec3f.Z 0b247407073cc087d4c94d6514e6ad4d724179dfa743c18df3615d5f8e5bdf91 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnClose e9c0e037e825c051569c3a4bfd29922dd49f310046aa4417feb73025c7f6e29b -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnInfo 86923a5292dfb10cdd4cacbe532d09fd5dab8e526a34a8b3c528be2b5b85c0f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnOpen 34e24394a2817e1bb657ad61f1fdf55cb03c943dafa4a958b1809e21b9c2e475 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnOpenW 59880167b55b4115ee2624d742eec688be663a1f41900fbc081a53026eb875d9 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnRead 538447ec4d478d9e221081bbd50175f7b1eb4ce425084712c1767f63297ef6a6 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnSeek 48dec55b44afa4536afa02d28295bbda060ae06474b105bbc8fb15b07f6f9073 -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnTell b69fd0fdf1da6f76d64cf73d70768aefe8c9f69b08ce64301298f6830c50bd7e -->
<!-- api: F:Inno.Native.MiniAudio.MaVfsCallbacks.OnWrite c9d8e85f37f0386e353ec4d7ce26f842e195fc53db6a267e4fd9b818344c4dfa -->
<!-- api: F:Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_default f29aff13d83119b23cd98aec85b02e7473bfcf583d5d80ddf2a6c80ba897c342 -->
<!-- api: F:Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_games e8ecb99865c9ea542d9129ce28e92b987437b340aa1c2814abd98e5a8c199d37 -->
<!-- api: F:Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_pro_audio 80100e4ca953ec453eb98d36cbce1b8e34d213ddbb82bfbeee0811971c84d22a -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveform.Advance 978c10e1bf64bcda61019618fbef3bda24e132f73b5d2b303128da69a18109e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveform.Config 4cb8f71ba0ee3f899b56415b16a050a2099d4fbfaf9e7ad710851f40ae2ffba2 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveform.Ds ae4f31e514fba8f4eaa52b67b822c388cd5a42c96e3ba9680cf372b4a572cafe -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveform.Time 736e0ede738a77135e575c10977c0c937a9989d645d4624226a3dd4a52e5de1a -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.Amplitude 69449e3864414eb00d2b742cc0e6674a87a2606feded1b8f0f6e410634d902bb -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.Channels 087398d5805fab809989216a65dd9a95016f7dc23dc82bf55696799e5f4229dc -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.Format ee2d068c2ff71adccea9dba768b63a8e4b861ba8a64feb069193620207203cd2 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.Frequency 2948b3cb8b3746ccbd996c1e73070ee75e3623d413ba19ca046d3c1bbdcdec26 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.SampleRate 48295a8b2c717d19d668a54c55fbd69731bac522f50156690a69fb036dfae04f -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformConfig.Type 463ed59027e3305736a4338c19382379b46d05bb18ab2cc335df4f5f7e4b3ed9 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_sawtooth 3d04809cae85194b57119195f2d27cc71d86ac6a7d25317288099ddafc8c20d4 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_sine f0dd0fbeb37b8778d32e483dc40fcba2a3800a54c565035b5a3e7d28b9fe1e5b -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_square e47bca282c53fd73da33f832a27ab1ec2d95c95c8dc4b495e0abe28142f8dd14 -->
<!-- api: F:Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_triangle 8069d906c4de27f646c81c3f64a9e68e968312708817535a6f2f7fb41ece46a8 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MABACKENDCOUNT f13a9551903ed62282a8050ed5d72dc6ee7ca06edec8908762d3aa460573309d -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MACHANNELINDEXNULL ade9b655b507993364cebce0ac09981a2b5394deb62f16c0a3208c41debd748d -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MADATAFORMATFLAGEXCLUSIVEMODE 7b076a33d88fe5ab8cb6cf19ff2b2e4d359cf410a12055860268eb8cd61f9314 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MADATASOURCESELFMANAGEDRANGEANDLOOPPOINT a7128f2612b3708d45903ec4f4732783f2410d057e9c8227ea097d987f3691d2 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAENGINEMAXLISTENERS a70ad9f8611b9dbdafe5c106a272abfae475a18f4595089b9ae0026559c50bab -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAFALSE 04af8abdfa66952bf9048864ab51fb250a160f861a60aa7a85dd761917c09b98 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAJOBTYPERESOURCEMANAGERQUEUEFLAGNONBLOCKING 2ef2d9672348f39a278ed94f081b6a69d8787532e9bd41506d0295d3dac8f552 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MALISTENERINDEXCLOSEST 9652b55fa86c2b6341af309aac84bbb15c63eb78d31a62a1d97e07976c6402af -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXCHANNELS e2565116593d95c00ae33b66d228db6cd0beec46c0a3f01e28c9f05eaafef601 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXDEVICENAMELENGTH 3a8eca1ab0a42d42e7938af568faf98ec6724c63c52cb6f1e7fcf1ccd39aa49d -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXFILTERORDER 1137db869829f9811b568f375af177602aa2dd5226d013cace9d3825dad7368d -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXLOGCALLBACKS ad5a7bc50751ba47bfea3778d1cef1f6cd27df34404177c06d034781367bf90f -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXNODEBUSCOUNT fff64e1655fe2a08abf3856a7ed1dcc2940e61efbba9208d65410f4b80b690d4 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXNODELOCALBUSCOUNT 34e1530ddd169ca2a70031aa9edbef0ae453df0ad741523c8963590697105ba4 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAMINCHANNELS 7afcfecc77abd1136831818249b2c1dd74c78d0ae44939e4fa76bc6a1afb3565 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MANODEBUSCOUNTUNKNOWN f5572940bef1d71fcd3a095a9a9dc3c68dd8b64bbcc04a27d6e9d75ce072f827 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MARESOURCEMANAGERMAXJOBTHREADCOUNT 08015bf3e789b190f35a7c8ec5732a218aa9bd3c159a395684ed3edf59f1c733 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MASIMDALIGNMENT 31cbeb978504570adf16bc95f21d4a844f53ceb4d617508c82b57d42d842418c -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MASIZEMAX fdfdffbe1264800408abdd2454f5e24c5ab6bf80c9dc254b381b9971c4c67460 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MASIZEOFPTR 310260d4be49bc72a265b45fb92989da78654d13f12c14e0d7794ce827a3e7bf -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MASOUNDSOURCECHANNELCOUNT aa081a1920cda205bd3aa7ac589bee746691d85d13e100330e9c8a24ab82729e -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MATRUE 8b82cfccd314be8bd2210baf112781700c7fc2cbe26d9cdf4a463af8b739f889 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAUINT64MAX 8788075e93942deb1cd834377506d797504eef8723a6802a3c6a9521c5039048 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONMAJOR 2081d9b2708222c864a00bf4545b76fd2bffb4e462c17be526c8fee76afd05d7 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONMINOR 4e2a33e716d0c6c902cc3bad907df1ed8d351d988eec73d7f3af098fd90ed406 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONREVISION edc816a0a533e274253145229caea94d6900570a9f85db344250c033a4dc0574 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudio.NativeLibraryName 80e41bad2a910099f6d636ff75b606a5740faee2bdee0dde0e1556367746a15f -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MABACKENDCOUNT 66face4773362583aab469e863b54c341ae5cc6e78239ef6c97db0d45e359bdc -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MACHANNELINDEXNULL 2ac8ab6b049e1a217d1208419ced71b4b6c93c5a77a9e9704588334cfe390238 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MADATAFORMATFLAGEXCLUSIVEMODE ea93d6a0abe366a314c48cf1a7818b93a378df6a2e518fa1e80430da2b5f217a -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MADATASOURCESELFMANAGEDRANGEANDLOOPPOINT 99e3d224f7231efa6c3be00ec42eebef9b01c0017318c5e7eb835a8cb3674010 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAENGINEMAXLISTENERS 257ba26dbd59d181cf635d5b4c927217f7a446a74c122021a46824594da3ca2b -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAFALSE 5cfe14d256a8fbb7c369cb4e2bceee430e5d0e14ea8c159062b312591507c5a2 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAJOBTYPERESOURCEMANAGERQUEUEFLAGNONBLOCKING 8d25d32d9b281f195dfe11c716cc44c52aa5cd069da6c06219dc3b8627132cb5 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MALISTENERINDEXCLOSEST 48229b7db97f06586341582cdd339334ad228de043fded08a7cd77e21e7cbf96 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXCHANNELS d9a0e1707fecd85a3957ee7e510b4dbc28941639995a84a3c3043a7b2b5cdf42 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXDEVICENAMELENGTH 88253d1f8add739e15d481053915c860a359f2d2226687a7b21f13fd99d88f0b -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXFILTERORDER 4ebfbbafde2a390aa8e7017c38f5addaa61d5d5adeca1e27e05cf549e9524648 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXLOGCALLBACKS e94cb34383ffd5e4b8947f45e35588479b6a374fef82fc2cc7f92cc115b30a4b -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXNODEBUSCOUNT 329af9c706395e444dbdda7e3b26eaaaf37e2e762e373e5a40464b9c44b1823b -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXNODELOCALBUSCOUNT cf476d25797937d3847517eb3ea2d5aff2010932991d446c1fb5660161e110a0 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMINCHANNELS 4c41fdd3591cf56c6d3c86d6d584319037a1133ce5ba0f56f90dab9d6ba8cd4f -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MANODEBUSCOUNTUNKNOWN 832f3b84f6a615e194640706ee5288d718a7d2eb4b205901539971e5a2263e93 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MARESOURCEMANAGERMAXJOBTHREADCOUNT 40b28b4b086e49afdfda37e62d91cec6e256b9c7f68341826008cfbe29e3e7e1 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIMDALIGNMENT dc0871e85ba4e645caa1d61136d06adfbaebe21c01b25eb897ac2b84f9455349 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIZEMAX 290777803549f6d4ba74cfac76e0489eda242c9cdfd36b0a99acd4909f9d0470 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIZEOFPTR ba007ca1199b2fd42dda32902dd5f704fa9a7af8243c730edb937e027f1828f5 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MASOUNDSOURCECHANNELCOUNT e1a20be51c62efc71685727270c0504bc8bc3587f56158b483d9b62db3e8c83e -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MATRUE 049efb8d239029c2aef6bf11fb5131ee055df0db010ea4c10c875a6a5f5ea2b9 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAUINT64MAX f1d28008881b962269c03c54ab8a901e20b0bc7e2f69bc2fc16fd0ca3d1eaf12 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONMAJOR fa9954e1ea77ea9fe0218a62abb3ab2f862b3cceb9b366308c371541308000b2 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONMINOR 7dc389b36ffdd8b791474f1491ca8c6585393da12d0cc4477f943ad08643a3a9 -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONREVISION 5c08210750de350a539079bb0306160702e7578bde23f0e5102a4ffea64d2efa -->
<!-- api: F:Inno.Native.MiniAudio.MiniAudioContext.NativeLibraryName 3eee0ed54fad1bbcbebb55d3461de26a59a2948fdb746876cdf23bdd760ad24a -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.#ctor(System.Object,System.IntPtr) 9f15362024cc2a81f63c4a7152b2a17fee27f87dfc2e5d151ece6be13b765ca2 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 50f2ae193483f453cd29e60fd5544717ec36d45f355f8e16ee76ddcf35305edc -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.EndInvoke(System.IAsyncResult) 1d1183c8983fa33fa80eaf98db80ee5ef5ea81bff846f8b8d02fb2bc112e839d -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.Invoke(System.Void*) f3f0caf65d1c6bd9f52686e0c654d6766043f968fd548d203c8ce83d7dba54a4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderReadProc.#ctor(System.Object,System.IntPtr) 73891942d13d1a36e5521052dd38c8cbc78c4ef56fa32c3f9ff860c800211aa1 -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderReadProc.BeginInvoke(Inno.Native.MiniAudio.MaDecoder*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) 9db354464e787b7ea11842d71551dc0bcf471c99cab11221ead076ecc38e2c29 -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderReadProc.EndInvoke(System.IAsyncResult) 5b6d57f701bf6590935b561f498a9afb295d90be1a53e11c9dbd081864eef11e -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderReadProc.Invoke(Inno.Native.MiniAudio.MaDecoder*,System.Void*,System.UInt64,System.UInt64*) d048654b8cc5d4ad9eac6aa68147462625763844c52ad654fc6a3c1171b0397f -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderSeekProc.#ctor(System.Object,System.IntPtr) 8bacdb045cbda8d2981e9e1b91e3f732386d0887b35ae89e7acfdd76634fc25d -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderSeekProc.BeginInvoke(Inno.Native.MiniAudio.MaDecoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) 897103da53c606a2f0e585af4ca6bc4b4bbe305fdaf2476831d441be24055f86 -->
<!-- api: M:Inno.Native.MiniAudio.MaDecoderSeekProc.EndInvoke(System.IAsyncResult) b986382e7208c3a918357873c9da28521e8e85a891a1508eabe913e6e38b1cae -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-72911cac7baaa9fa"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListenerConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24424) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8f48319dfd7b6555"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24430) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb68ae19a7bc1517"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24432) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4d51d40f6930d3f2"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24434) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a076f8748f805621"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerListenerConfig.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24428) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e3f753c60cdabf4"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerListenerConfig.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24426) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99b0c9b72d817e68"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.SpeedOfSound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24436) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d754bc1144989a2"></a>[`public Inno.Native.MiniAudio.MaVec3f Inno.Native.MiniAudio.MaSpatializerListenerConfig.WorldUp`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24438) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d9f9b304a3fde426"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaSplitterNode.Base`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24459) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56e3fde1398735ad"></a>[`public uint Inno.Native.MiniAudio.MaSplitterNodeConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24482) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9461e85dc2fd61c0"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaSplitterNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24480) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b5e2eec03e77aad9"></a>[`public uint Inno.Native.MiniAudio.MaSplitterNodeConfig.OutputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24484) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cbe3c7db5c53d2cd"></a>[`public Inno.Native.MiniAudio.NativeArray_5631104b5a38a871 Inno.Native.MiniAudio.MaStack.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24509) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c355e77c9f3f8153"></a>[`public ulong Inno.Native.MiniAudio.MaStack.Offset`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24505) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-adf844e8c8f4e080"></a>[`public ulong Inno.Native.MiniAudio.MaStack.SizeInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24507) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-907e3d293182839f"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_alsa = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24528) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00fef0bc8c3e6cce"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24535) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b016fb4587506a3"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_flac = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24530) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2505d6c9ded40e7a"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_microsoft = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24527) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22222bec7a9c8ea5"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_rfc3551 = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24529) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb4761ea978f4b38"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_sndio = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24533) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-337a883cfe2eebe5"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_sound4 = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24532) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93198c8dd375cc8c"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_vorbis = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24531) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5886f52d9adcaa18"></a>[`Inno.Native.MiniAudio.MaStandardChannelMap.ma_standard_channel_map_webaudio = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24534) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-78ef21d86805aaf8"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_11025 = 11025`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00320dd5a2a4bfaf"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_16000 = 16000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24555) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-43024020fbc63987"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_176400 = 176400`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24553) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed4a46ae7c2a85a4"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_192000 = 192000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24554) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21a8aaba1aabb597"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_22050 = 22050`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-174d9d4bdbfbbb42"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_24000 = 24000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24549) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c386b6a026d28066"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_32000 = 32000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24548) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b07081e14e0d54c"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_352800 = 352800`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24558) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f9e2ef1af2c7480c"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_384000 = 384000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24559) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33c1fa198abbaa12"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_44100 = 44100`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24547) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e817950e07ff9070"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_48000 = 48000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24546) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9306c5dbb62cb75a"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_8000 = 8000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24557) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55f1e89273a0c24e"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_88200 = 88200`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24551) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e14e11d0082735d"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_96000 = 96000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e9092e0d26f4355"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_count = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-838a1f5243236cbe"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_max = 384000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24561) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ae2b8a297b461a1"></a>[`Inno.Native.MiniAudio.MaStandardSampleRate.ma_standard_sample_rate_min = 8000`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1a505397d87a14c0"></a>[`Inno.Native.MiniAudio.MaStreamFormat.ma_stream_format_pcm = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-585947a94c4d379f"></a>[`Inno.Native.MiniAudio.MaStreamLayout.ma_stream_layout_deinterleaved = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24594) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ff50c8fec502803"></a>[`Inno.Native.MiniAudio.MaStreamLayout.ma_stream_layout_interleaved = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24593) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72ffe6e69651ea6b"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24621) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50588473ddcbda15"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_high = -1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24618) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-31a065b3ac9de0f1"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_highest = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24619) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ee5dddab7389f4d"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_idle = -5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24614) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f871c6fa8c6a5678"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_low = -3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24616) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c626055bd346e078"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_lowest = -4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24615) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5dcd5f166ab55e4"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_normal = -2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24617) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-372f5b42cd0d9313"></a>[`Inno.Native.MiniAudio.MaThreadPriority.ma_thread_priority_realtime = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24620) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6db9b7e6120ce588"></a>[`public long Inno.Native.MiniAudio.MaTimer.Counter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d6354d888973303"></a>[`public double Inno.Native.MiniAudio.MaTimer.CounterD`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24637) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08a5d77988bb7159"></a>[`public float Inno.Native.MiniAudio.MaVec3f.X`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24658) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8db7b949ca219dbb"></a>[`public float Inno.Native.MiniAudio.MaVec3f.Y`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24660) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-32144675d89e7dee"></a>[`public float Inno.Native.MiniAudio.MaVec3f.Z`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da7b46a2779b8770"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnClose`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24687) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b24eebf63a37812c"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, Inno.Native.MiniAudio.MaFileInfo*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnInfo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24697) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3de829686cc3f2a4"></a>[`public delegate* unmanaged[Cdecl]<void*, sbyte*, uint, void**, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnOpen`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24683) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-06a250e80780ce31"></a>[`public delegate* unmanaged[Cdecl]<void*, ushort*, uint, void**, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnOpenW`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24685) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d5e930ceda2a4f6"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, void*, ulong, ulong*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnRead`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24689) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-268e32bb57df987a"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, long, Inno.Native.MiniAudio.MaSeekOrigin, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnSeek`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24693) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-912c0c93878d5733"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, long*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnTell`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24695) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f50437a2f16f1a20"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, void*, ulong, ulong*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaVfsCallbacks.OnWrite`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24691) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d5e28dbb09947d6"></a>[`Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24715) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fe1b13091f9ea5b"></a>[`Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_games = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24716) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7790294473e31b47"></a>[`Inno.Native.MiniAudio.MaWasapiUsage.ma_wasapi_usage_pro_audio = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24717) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec19c20eafb37e53"></a>[`public double Inno.Native.MiniAudio.MaWaveform.Advance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24735) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-419f3f9a4f029d8a"></a>[`public Inno.Native.MiniAudio.MaWaveformConfig Inno.Native.MiniAudio.MaWaveform.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24733) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75ef1efd292ef5b8"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaWaveform.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24731) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d1d7975dcd2b70f"></a>[`public double Inno.Native.MiniAudio.MaWaveform.Time`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24737) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0943705fb7846325"></a>[`public double Inno.Native.MiniAudio.MaWaveformConfig.Amplitude`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24766) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f0c7e5f41b306c6f"></a>[`public uint Inno.Native.MiniAudio.MaWaveformConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24760) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a9391918492b9c69"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaWaveformConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24758) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d9eadbb4e4d75416"></a>[`public double Inno.Native.MiniAudio.MaWaveformConfig.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24768) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38120bd0dca963a3"></a>[`public uint Inno.Native.MiniAudio.MaWaveformConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24762) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe5c0ee513f2f1dd"></a>[`public Inno.Native.MiniAudio.MaWaveformType Inno.Native.MiniAudio.MaWaveformConfig.Type`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24764) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dece1b70169967ef"></a>[`Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_sawtooth = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24789) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3ef0ca19c62460e7"></a>[`Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_sine = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24786) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-87737eec4def98e4"></a>[`Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_square = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24787) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-721e765c99f33901"></a>[`Inno.Native.MiniAudio.MaWaveformType.ma_waveform_type_triangle = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L24788) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a0cd4a9c3d745b58"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MABACKENDCOUNT = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56c45c79b75e82aa"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MACHANNELINDEXNULL = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L231) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-34eca1a6e0165d7e"></a>[`public const uint Inno.Native.MiniAudio.MiniAudio.Constants.MADATAFORMATFLAGEXCLUSIVEMODE = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L241) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-771330dcbce67533"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MADATASOURCESELFMANAGEDRANGEANDLOOPPOINT = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L240) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c7059d40f712db7f"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAENGINEMAXLISTENERS = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L236) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d7d414cc4f39124"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAFALSE = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L243) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c07bbe4885134c3"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAJOBTYPERESOURCEMANAGERQUEUEFLAGNONBLOCKING = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L225) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc0b05a438baeb49"></a>[`public const byte Inno.Native.MiniAudio.MiniAudio.Constants.MALISTENERINDEXCLOSEST = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L248) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1a9e1f107b9f0e43"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXCHANNELS = 254`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L250) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a772e52715188cd8"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXDEVICENAMELENGTH = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L242) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89977dbfa2663c34"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXFILTERORDER = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L227) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6b778af7d2481ce6"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXLOGCALLBACKS = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L238) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38acfbb587c21a54"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXNODEBUSCOUNT = 254`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L245) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c209cb2b88947e1"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMAXNODELOCALBUSCOUNT = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L249) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc5063057bea8173"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAMINCHANNELS = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8dc91ddc5007bdbd"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MANODEBUSCOUNTUNKNOWN = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L247) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-912b6db221bacaf5"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MARESOURCEMANAGERMAXJOBTHREADCOUNT = 64`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L246) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d2518db032365f0"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MASIMDALIGNMENT = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L233) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21e408dbd428e475"></a>[`public const uint Inno.Native.MiniAudio.MiniAudio.Constants.MASIZEMAX = 4294967295`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L229) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cabdfbe2a469ccac"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MASIZEOFPTR = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L228) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-be409794773c8151"></a>[`public const uint Inno.Native.MiniAudio.MiniAudio.Constants.MASOUNDSOURCECHANNELCOUNT = 4294967295`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L239) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d533d42f0847e700"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MATRUE = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L230) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-414ff848238b05f4"></a>[`public const ulong Inno.Native.MiniAudio.MiniAudio.Constants.MAUINT64MAX = 18446744073709551615`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L232) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c1107667a92d243"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONMAJOR = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L226) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c8f473901d05fa2"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONMINOR = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L234) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c22c303f768b2ec9"></a>[`public const int Inno.Native.MiniAudio.MiniAudio.Constants.MAVERSIONREVISION = 25`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L244) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b11aec77d44b3977"></a>[`public const string Inno.Native.MiniAudio.MiniAudio.NativeLibraryName = "miniaudio"`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33f8f24e501c726b"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MABACKENDCOUNT = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L297) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9602221a0d24080a"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MACHANNELINDEXNULL = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L293) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3ef1d005834ab4a1"></a>[`public const uint Inno.Native.MiniAudio.MiniAudioContext.Constants.MADATAFORMATFLAGEXCLUSIVEMODE = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L303) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3682ee6cf62b610d"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MADATASOURCESELFMANAGEDRANGEANDLOOPPOINT = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L302) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ca8c7f1391143682"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAENGINEMAXLISTENERS = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L298) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-17fa6ba9d2ee78f8"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAFALSE = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L305) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec00847d058fed8c"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAJOBTYPERESOURCEMANAGERQUEUEFLAGNONBLOCKING = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L287) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ae7694a73998adb8"></a>[`public const byte Inno.Native.MiniAudio.MiniAudioContext.Constants.MALISTENERINDEXCLOSEST = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L310) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7e0a7415d5f8579"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXCHANNELS = 254`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L312) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-882cf9209eb139bf"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXDEVICENAMELENGTH = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L304) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3f2716c14809ee8"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXFILTERORDER = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L289) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b9159300c59c02dc"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXLOGCALLBACKS = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L300) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bbcde47daf33ee4d"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXNODEBUSCOUNT = 254`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L307) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88bad155bd9a8510"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMAXNODELOCALBUSCOUNT = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L311) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-abe0e012a2f9a26e"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAMINCHANNELS = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L299) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d5c7c4516f6c34d7"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MANODEBUSCOUNTUNKNOWN = 255`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f884ecedf009682a"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MARESOURCEMANAGERMAXJOBTHREADCOUNT = 64`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L308) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1cc46b2fe5457fe2"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIMDALIGNMENT = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L295) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ec9f2cbff4cb493"></a>[`public const uint Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIZEMAX = 4294967295`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L291) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ea94c8670245a391"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MASIZEOFPTR = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L290) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2deaeef3d4dcdfdc"></a>[`public const uint Inno.Native.MiniAudio.MiniAudioContext.Constants.MASOUNDSOURCECHANNELCOUNT = 4294967295`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L301) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4809f8ac973d5435"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MATRUE = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L292) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09a2f5cc2f0e6d0a"></a>[`public const ulong Inno.Native.MiniAudio.MiniAudioContext.Constants.MAUINT64MAX = 18446744073709551615`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L294) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d74cb6701272af4f"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONMAJOR = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L288) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a9c3ff6d7f8535c2"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONMINOR = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L296) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c4a939407b7b11c"></a>[`public const int Inno.Native.MiniAudio.MiniAudioContext.Constants.MAVERSIONREVISION = 25`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L306) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-840b3b6599f2541a"></a>[`public const string Inno.Native.MiniAudio.MiniAudioContext.NativeLibraryName = "miniaudio"`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L324) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-320e7aecda002499"></a>[`public Inno.Native.MiniAudio.MaDataSourceGetNextProc.MaDataSourceGetNextProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-711df65b82a37f16"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaDataSourceGetNextProc.BeginInvoke(void* argument0, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f05b2a1932ec6c14"></a>[`public virtual void* Inno.Native.MiniAudio.MaDataSourceGetNextProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c30143cb200a0e37"></a>[`public virtual void* Inno.Native.MiniAudio.MaDataSourceGetNextProc.Invoke(void* argument0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-15acc3516954f5a1"></a>[`public Inno.Native.MiniAudio.MaDecoderReadProc.MaDecoderReadProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23501dd307c50f9d"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaDecoderReadProc.BeginInvoke(Inno.Native.MiniAudio.MaDecoder* argument0, void* argument1, ulong argument2, ulong* argument3, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b45beadad298e11"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaDecoderReadProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bff795b69c315551"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaDecoderReadProc.Invoke(Inno.Native.MiniAudio.MaDecoder* argument0, void* argument1, ulong argument2, ulong* argument3)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1fced1be7caefe7"></a>[`public Inno.Native.MiniAudio.MaDecoderSeekProc.MaDecoderSeekProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18685) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-128f00c527ae5266"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaDecoderSeekProc.BeginInvoke(Inno.Native.MiniAudio.MaDecoder* argument0, long argument1, Inno.Native.MiniAudio.MaSeekOrigin argument2, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18685) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ffeb47e5e308556"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaDecoderSeekProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18685) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
