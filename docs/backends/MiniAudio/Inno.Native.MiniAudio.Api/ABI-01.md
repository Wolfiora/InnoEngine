# Inno.Native.MiniAudio ABI · 01

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_all 67945a3bcbd9684309020bed96fb2da39fc9e4d0f81c57ff40e6d494a91e73b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_none 1c69b93df20bc60319c4992e7b5e9a7b4dfca5372c697f2f86d178eecee66e42 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_system a0ec7ab945636a81493803d710a4b0286fbb74e1e279ec8aa5033ac7f6404ac5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_default d012e2cfd09a86a2b1252bd38790d8c40d2b8dfa80db36e814587a4ce4896189 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_default 83fcc133a137d7fcff4a6ecc7137ff6c5aa83290cdd20345ac41477bbd30ed9a -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_movie 0d8554596347527977c55a23b0efe0c071d9ddb4e7636aa1cf4086ee5d71ee9b -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_music 2249d6645c8badcc15e8e9942e61a7462c45e23020256858c0fd87d2894cbe51 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_sonification fa8f16d6241bd854ad18889bb3e255c0c9d40f4c3d2db74f30e6fa9abf9170bc -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_speech d3899ad3df57a87ccf55a9b70730d0fba88b5adbaba7b17cca37fb8c7f9a2340 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_camcorder 4cf6ef54eecbf5766308b847afc9d60ceb7a4483f1446d55987e89d487ebc76c -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_default bb9e1a32293c87b21cc3fc5124caaef3768536265f66fa71ed9b9dc5a35765d4 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_generic 9cc2c23356bc8101cc4958c52d825273dcfd260b53bd195aed8f14253df1c5d5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_unprocessed ca001b04043ed9959fb0cdaa6de24400457ab4efa3b42b97d829c8dd0d3b3819 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_communication 5a1cc057919019ad6be28535e2a844150ad62ce83b59a72e1fed89e96a591eb3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_performance 5f367baa355ed0345859e21ba9386a4c3656bab887d2935c2f47c772dc9aeebe -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_recognition d3144a131689ca467da66cad205941262653626ca6c3a298d0d397f793783ae3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_alarm c4542ef59bb66f7d7bba6d84b38b719e208a6bbaf892531ab3ee96e7d7322cfb -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_announcement 1d81347e30e61f98984a50b3ca7ae497938840ff750f5098238ac8953fc0ab74 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_accessibility 4a02eca2bb05f434643a0a0a3e33b7fb96d52b3df4f04fa92d86731067ffc14a -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_navigation_guidance 28670942f11890224cf005488a1fb4856f3dfc85d43b2dd5100692ce4bca8acd -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_sonification b2801fe2d2a69fa9a907e75f7421289fa8d1fa5e7b143c9e333c29b5efa7cf52 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assitant cafcb64d42cacd44a21ca84ccf8a29621afd3ff2e1d41237b9850becb5070a1f -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_default a0a2993bd7dbc00a1fd4c9f1c090de2511f8b47939351c87b415438ed02360e1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_emergency ae3563e5f77181168a9092992ca13ca6d3daf516081b6972399f885188c4d3ce -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_game fafdcb662c61fc8f7d441699c996a8302c2c6b1512b1c0e840452195d29eb64e -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_media 77e377da55d2a59a939efa5e1803012408fb1680451cd1c64f1ba251936dc679 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification 0d47801fe7e2970d469bcf5cbac65af53ee278e46fed2258ad2c49540262194c -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification_event 446ceb94b75c08cb7dad639944750eb03531978b0d92d69f931c95abb9fe13e1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification_ringtone fc9553f8b8825ddee1c291b053158aeb6995d2fcf2489414851752e4cbb7b72c -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_safety 069cb399f4bf247a374bb4f349503c04acb43bfda9848f757627c772ec732b6e -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_vehicle_status 6a1663267b2d5f33506347435b3353773b6671937a31e725e8b259b28ca544dc -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_voice_communication 51d2c4b0d82c132ffe8e79523776df7e2ed270b9398c34d5e86551d194459a11 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_voice_communication_signalling 3accd3fa402751090b2d317cc1d6a865b611ac107234d48137608848a519357f -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnFree 8e57f7a9911d9ce04feb94428262e4661f7171c49f254727c92a8a3edd2a4330 -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnMalloc 73b809eea6feb16769c97519dfc23f41f19bc1a4e8be216f958e37615b5c366a -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnRealloc 6f7b43e90c56607b0665ad59f0bcecbd95a676ef791a17e18ba70fcb27c212c1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.PUserData 40ccba2af34ce74aa8c2d713943abd7ad03798689453d153c0d66aded67d878c -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.OnSignal a991e6288cc03154dc4f193e17b44a2c1fb83ea6bcd74c7181b290efb7398b75 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationEvent.Cb 964b5492c2f19714f827a78d13d2ef657b4d30e6ef91b62b2ad856929746ae55 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationEvent.E 6fe25e6a61392466f64f3de39ff2855cf30a57e766c0dbf5f845e45e734d584f -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationPoll.Cb 9089e634b74d6050583bb03573433e7c9178adb94c3e0e3e89eefe1cb867bd2c -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationPoll.Signalled 561cab618cfb7527f1c05bf0116a30a4d08d7e7007ec3a9b2444b0031ae6b14b -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicBool32.Value dc720aef214c6dc55cbfa177303d520083fd3130639961e08ff1b286a6fb996b -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicDeviceState.Value 5c70151e8cbc8512f3e3a7f0c0ff21258642c6890626c94a35aff77b573a36f0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicFloat.Value 893012d54cbecbf22ae83fe807498782ebfc1bde69bf3767d9e459b8ce948439 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicInt32.Value b8b9ebd52251e7d62c3a07d150c7bc5f82d926371e8704b94b6c19138114edca -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicUint32.Value 8542759536b5f16a8b382dc6fb462ca7357cf4fede215075a5a5435a5f3a2ace -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicUint64.Value 8b1043aa919775537dd7b8d302bc4ca75e14b2c8fc270a9c2c9308643a2297e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicVec3f.Lock 3d5f9f4e897dbbb424d5fae7364a6101e4fa91c6fa2a3652c82ca617404a16a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicVec3f.V c66b33ca18a953a2126587d942bf3e5840174c75e204f7dfa77bbdb72ac4e624 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_exponential fbfb71f84c740252a0b5e50f3b9fb05b05a017c3dac4d79cc8236d0983f2db33 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_inverse e01c88e39f64776993af16cb105cd2844b7db52982bb54647cdcbdb04f122051 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_linear 4dbbf317279e05069e3890db3b920126c76d0a86d3f29e16d7ede95f4db8dd73 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_none 5b145db2d3866893f547cc32d3b1c1d6265958c0c40906222a84bd8bb85a1eb6 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.AllocationCallbacks a9e16c68f46ba27fc60ff26f01a07880f369dad6af9d64a457e88458728c9404 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.OwnsData 7c2fc8f85e9a096c73daa420b321157604540d06be1a76bf6323ffc1e2b87f28 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.PExtraData 71bbc8eeb4faf56ef05c0aa0451cbda1ba93a397c354c71d9eee098fdefc3871 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.Ref 66ac32dadd92355f549e24702eee235591691f4a560d5f81b7ec7cae010c235a -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.AllocationCallbacks 4fee3ef4135266cff9abc9a2978435c3fd2522ad11d87b45feefc8281a1f2fb0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.Channels fa872b5568832ed17c833c051d14c10fb5c6fa517569d827da80e0d62b3614b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.Format 84447d12cf61c921c1ffd30769d6b57338e7112215c15bfeb618482ad82ce098 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.PData 8e53b5eca8cf89223140f867517374e95eb305b0bf8f3128a94a36d5887bb456 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.SampleRate 15ffab6667b3b27cad25f0802bedad0db662fd707bf7af6bac8b0db9def516b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.SizeInFrames 07e4cff8403fbb51bf6ce5efb7aa76fd3c7244b170736fd89205f37aedae37be -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Channels c384bb7e9dfee4a84019a5a7c2931bc0a4a7612998cbf8efd141e7809c93055e -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Cursor 13979587330223bf3212d469c67b16414ceb824efc8e90caa7d38a137e807fb8 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Ds 0ed73fa4d7fdc223f9c68e12473e6a761c0a1ee1f4c127d44bc3f7c5290b24f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Format 193363daf7065dcdf73899211cf56190ae8fe2c0e9321c8f30ac783f738f2e10 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.PData afae3499f162142efc6bcfd8a377678a63573d7eb798a86def974fdff9b96454 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.SampleRate 2b1cf46bb2717257a64976670071c668fd99cc0582372d201b4d9551b3272369 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.SizeInFrames d8c5bc2c9809bcf608ec25f7dc2e83d29e8164ccac6a354dd8a3876dff29df7b -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_aaudio b9c976437de3c1b74f13b33d0487fbfd661ea0c62b56b2349b555b2efb4ec2fa -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_alsa 8c6a40b60415ccca4600136aa9d0daaeb7435fbd5ecc734e3882b2384f365e56 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_audio4 03280db6456ea4d89f0e6421e3f5fbd85c61f99da8108394b1eec963dd5f9da1 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_coreaudio ffd1f07db4c784a71599e0fe3c9ace3f706ed6963ae887d42d25ff6820cda01c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_custom d476930bff5fa9dfe41d9d3116f2251e1d367df35b30b9e80eee9b33c83c492a -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_dsound 8f01573bdbba597a9898bc0a7779c9c0f9b5e73ef6ce9d1bfe710104c8ad2745 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_jack 66387824b75d61239de381484183957121605d554cc0e6ef5e6fecbde5d819f6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_null 61386e0b0c53c41c2868ed2e99692bc06140434d09b4aaae564ee38d8dc64110 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_opensl 0d6cad80239835fd1723afbbe6efc381eca085127e1d825c2e6831cfea5200e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_oss ece082939d1f4927f5acdc96b71ae9c122f3f480ccf69a78a42170045f25cef4 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_pulseaudio 9762761360c71f82b0cdc957d294b0857d510cda9fd8f6833cde7a84d473aece -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_sndio e9f6d63a3077fd29a808351045e28a91c61200594397903e6c9b3510eae4aa7f -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_wasapi 094e1be1ff7f4f140cb63ffaf1d99e36d586b4ff2b035c5180a97d9a6d92aea6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_webaudio 5c37327451661fc376ab5040ce4f8040f414d7b36367ec25bd437d8f3295533c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.ma_backend_winmm 74f03f4ce41fc3ebb566e90a815b69da925945a6a1a316e783f6326066edb07a -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextEnumerateDevices 6c7b1138f3d2d6bc72cbb8e04b7e3172d1cad9a1ad3b03d7fbbc20be7e791100 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextGetDeviceInfo c28fb4aaab34b96b1ec5accd053fb6e4dcb5d2c1dae9a66da2e8ca45d3a11f26 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextInit 048a835369df2093e21429db1a92133f680256d1f6748c11c558f06592b7dbd6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextUninit d2cba83511fdadf6c86276efa96d6e70402f0f51c7525147a0204c9eb6d4c822 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoop 51db88c2c4f620b99f81b9d824d289c976cd43c0274f4ac2cd2937b11e52dc65 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoopWakeup b849d34833ba22dbcfdc8a40cf380452bc35018e9335db0eaf069744a160ef4f -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceGetInfo 876c2e2f0090aae590fedec7cd24724332b95b6b8b41158273c99fe452dcde9b -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceInit b542e794559c9b7d812d3bb3e8a0cce0f83cf39fa57b044bfef63419ea2ea03d -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceRead 86761f1901ed44b12633f2b26619bcbd27f22c835b035e1c652063b6a477bc67 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStart 02c5629b65cbd2d39cf0888328303ae93a5eb6344fdfd702b5b8f949a175c578 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStop fa4854c88a640404b0e3ecf66df56bfc0568d3eaa4c3f0e13290d7702e023845 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceUninit d36da64c4c7d7281dbdd7075d59f919c031cfc02e433dda446c16ad91fc7c0e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceWrite 6b86c7267eb5f3114d900f85c1bb775cdb95461ecdea65eaf59818e40e27ff75 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.A1 cb42b80555d205b3bf24a064f04f3b566185715f7f460ae905f1f1afad9c52c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.A2 af6ccb581333c293b10086ff649e5a83de733225997e91ec9608d93938e894fc -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B0 d6722a153e84e63b172bfe9f08c96a7e02d88ea8e5eb8ab1af1b30c371295da7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B1 fecc599048b841b93fceb3f1d9a5434475bd229982d0b71bbdfdd7b3762fe817 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B2 73e5ee32c7ab519a88d87d6e32197548b0f164394c1ee6452e37e346afdc8b1f -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Channels fa727f3dd4f0b3b75705e60fdf89322df7b51f57e141bccd721422ee613a1b30 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Format 617a5d57db485c60cb98f5024408c383bbac34d4808f9b77aad74834f696532f -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.OwnsHeap bb79f6af8813af093bd8fae657d2e19c8a3a1c637658552b6eeba4251f7c4368 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.PHeap 068d229b8e0232484209019055cd582ae603b1f42f4bf7fa0837acf3941f9dd8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.PR1 c90084dffdb480b81555672c47122a700619025d22e7d6cd68d11e3e55f7a647 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.PR2 82af9b550a8e7362c37bcd5e11452b6fe545d2f54370496661e610c324fb13b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadCoefficient.F32 0a7f04fe62dee1cfe2810ece51a223d490b168102eafa23ecce1b13a7f81fc1d -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadCoefficient.S32 97678c522ec58d5216952fcdd5616487dddef088f24c7a9e4c42d9d17358552d -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A0 4812dd6aa1aaf4b3fca25046732a7a2ce7ca5900d7e35e4a5d9bde730c40e5de -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A1 4b3a5c0c492bfcda93610d8f36d7467ab29ed5981947f6c3b6ff4377bdf2e4d6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A2 fa36fdbe3b113a4027a390047cb3040270e5dd1d29acd93507372f1b25b36b08 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B0 fd68adaa472133883676e7c626e0bdfcbc9d7341de9bad53a28c4cc16e938cef -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B1 f03a68fe8a5619ffb1bad1fc6fe55faaec37f926c2a7c65b6f7e79a7853bc121 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B2 aa1cb48273ab4207a94757bfe520408c883ee58959d219283d4d950cc1cbbe01 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.Channels 145c10ea89398d1b46671b800eb8964a9da222d1ba712c61e2ef663a7473e5f6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.Format 25d3e4470dc926779c2f7be682e251b1334688bf52c7c575e593049ae221f971 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNode.BaseNode 510396e24daaac4b7f226f4ca58635144b0d1013a65a80cb56f3bd22d950b9f9 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNode.Biquad 56444e1ecc2747b0930bc4d080d29cb4a550a14f0cac3823c6e81dc809d96603 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodeConfig.Biquad 89a7b219e052789f168af4837f623b3588f7e2e123890491cf3707d08ade7722 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodeConfig.NodeConfig 5c6cfc61913ed63463e051ad0bd3758d5280de3ea780a82a6a05d590c606ef93 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Bpf2Count 98ab6670264aaa23c1953f9780be9c2de1e9b3724af042a82d88d6b2af8f06d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Channels 24d7e74f82b8504fb011db6508c5930d2ce30d6d00093a307d8699108671e56f -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Format 6644b88fcbc72eea7d1901baa8456afb038bb9f26e64c7367f3ad21c043c62a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.OwnsHeap 5a65472db9345eef9fb56b4556c14020209b2276f165a19123840337676bbca0 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.PBpf2 5b66e52f042edb5b192ed8fca55d128d8abcf6313e585d635ca7072054ad83b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.PHeap d612bdfb4f691d5f4467b7bfe69d84cc1ef769638fddf3e5d36657d2e0644af3 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2.Bq 1a59b280aafee5a1b8bae663eebd3795f0a1ae260f6235d429194c2d3b7f77a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Channels 875a240f39a859ef529d0a73c2b8ac1cea17663608d59740d55222ac04253f00 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.CutoffFrequency 915cbfa81d1ea651d8bab49f82470d272f512fb89970aa9031656c479de60a6f -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Format 6708605beeb49a16fef0b55256c8cfdb3e89594456fe74ed77cdddd4a4ac5936 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Q 18d4a28860b583be7aa38ffcbd8f124d5dac07eaf5dc04144d8b25ea553a2182 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.SampleRate 5ceadcd3899baa2a9d865a136d414f2de8952bc3506dc976225f7f7cc01d8c11 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Channels 765efe5de199e1535901f1335977d99dd7e89a9e700c057d011a2eff32254bdc -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.CutoffFrequency 2283f9653e7e3dee6d4524db1d288e6c0a1f4b2b5e6aeb0cd7c09464b042c44d -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Format 08f71b47959d88299f5ce9e780e4df040bfabfd8b415d54b124de14064fd08d8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Order b74d904f6b06708b63a6fd10ef8f367961a1371e3ab5dc0231ef899cae224649 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.SampleRate f59cb259c394adcd3c77ad8e649dc033c6c76854954109421145f7947446604c -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNode.BaseNode 2320018cd6a6e083caaabcd202359b7a94e03ee1260e65d980b233a51baf9614 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNode.Bpf c0ddd71a223ee9cec7513a0ee529775f7f218cb86f7c1e143efe0dd5eab67658 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodeConfig.Bpf 9f208f3b903cddc0fff8e088f95f1c50bdec6bdf00bc9cc5a3a14609f2e691f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodeConfig.NodeConfig e46bf278bb58b7cb831c0cbac020c713d189e92a5810f0294626ea65604078ca -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_mono_in 3c32be1d1dd04edcb55267f76375108e750626cfa2ce32d28e3b2055ed723f23 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_mono_out 9f0d2dea69e7ac6f5c6b85a1decca0d3979604162d767ec2fe83ed03a06552b3 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-1ba0c268619e828a"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_all = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16619) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4ed5bb096f7bcc1"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_none = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16621) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d558c5d570320f52"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_by_system = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16620) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf6668ee14a1afbb"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.ma_aaudio_allow_capture_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16618) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5b17321e8584095"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16632) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c23a318115d0aace"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_movie = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa4c1d7a1e2cfd20"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_music = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16634) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a400b0e0a1f6c859"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_sonification = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16636) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7cc69c2536346d5f"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.ma_aaudio_content_type_speech = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16633) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62bd9c22ee51c4d8"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_camcorder = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16649) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6dc74b919e12e4f8"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16647) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac4bda063573c70c"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_generic = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16648) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fc41df8e927ae5e"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_unprocessed = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16652) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ca775b08b987824e"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_communication = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16651) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e87161c69b64eec0"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_performance = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16653) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33d580ddd0b1c265"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.ma_aaudio_input_preset_voice_recognition = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16650) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-632cb04f5d374dde"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_alarm = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16668) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7252125f6aef2d32"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_announcement = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16680) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0791764c36a756d0"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_accessibility = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16672) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3b44079fc87ac82"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_navigation_guidance = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16673) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8476891feb5a5c2"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assistance_sonification = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16674) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3fa75dd136904b50"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_assitant = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-462006515aed3d93"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16664) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f4563703d1c6a1fe"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_emergency = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16677) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-815baea5c7bd6bca"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_game = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16675) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-be185623b4a5b69c"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_media = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16665) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7420c290d381b741"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16669) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b07c5748c0a5c88"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification_event = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16671) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7eb8c84434ba1ec8"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_notification_ringtone = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16670) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7858d516ce05a81f"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_safety = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16678) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9bb5d9ed63d28992"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_vehicle_status = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16679) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-948a4a546c8c4ba9"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_voice_communication = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16666) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33df444a4b19c724"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.ma_aaudio_usage_voice_communication_signalling = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16667) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56ce109a3a8d3b2e"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, void> Inno.Native.MiniAudio.MaAllocationCallbacks.OnFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16700) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c8e2163351bace51"></a>[`public delegate* unmanaged[Cdecl]<ulong, void*, void*> Inno.Native.MiniAudio.MaAllocationCallbacks.OnMalloc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16696) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-637aa255449c682f"></a>[`public delegate* unmanaged[Cdecl]<void*, ulong, void*, void*> Inno.Native.MiniAudio.MaAllocationCallbacks.OnRealloc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16698) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e50a66b6d59c8bb3"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacks.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da270ab00185c146"></a>[`public delegate* unmanaged[Cdecl]<void*, void> Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.OnSignal`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16721) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bdd3863cc5009117"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationEvent.Cb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16742) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e731159d93d4ab74"></a>[`public void* Inno.Native.MiniAudio.MaAsyncNotificationEvent.E`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16744) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f10779b131ccc28c"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationPoll.Cb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16765) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-841adfe9d0c66e00"></a>[`public uint Inno.Native.MiniAudio.MaAsyncNotificationPoll.Signalled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16767) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af6a7e4c54322ab3"></a>[`public uint Inno.Native.MiniAudio.MaAtomicBool32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16788) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f8a6a08cbc0e6ad7"></a>[`public Inno.Native.MiniAudio.MaDeviceState Inno.Native.MiniAudio.MaAtomicDeviceState.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16809) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d27fd76fc672b6bb"></a>[`public float Inno.Native.MiniAudio.MaAtomicFloat.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16830) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3db8edac1c2555a"></a>[`public int Inno.Native.MiniAudio.MaAtomicInt32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16851) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a008dd9f5ab8cfe8"></a>[`public uint Inno.Native.MiniAudio.MaAtomicUint32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16872) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c57c8ef0e724f83"></a>[`public ulong Inno.Native.MiniAudio.MaAtomicUint64.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16893) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c83c404a2090d0f"></a>[`public uint Inno.Native.MiniAudio.MaAtomicVec3f.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16916) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39c3316f76b4eae8"></a>[`public Inno.Native.MiniAudio.MaVec3f Inno.Native.MiniAudio.MaAtomicVec3f.V`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16914) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d057fd0a16ccf427"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_exponential = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16937) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-defea749cd99307d"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_inverse = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16935) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed5a095c74a9bc0f"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_linear = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16936) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bde87bff2c333bb0"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.ma_attenuation_model_none = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16934) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5777ff0e91789cee"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBuffer.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16953) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-885321d5e8323c1d"></a>[`public uint Inno.Native.MiniAudio.MaAudioBuffer.OwnsData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16955) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c8ef720b136faf95"></a>[`public Inno.Native.MiniAudio.NativeArray_ded1899ba091cc15 Inno.Native.MiniAudio.MaAudioBuffer.PExtraData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16957) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c105df0cbbb308be"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRef Inno.Native.MiniAudio.MaAudioBuffer.Ref`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16951) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2a1c7db6d0a189d"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBufferConfig.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16988) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1edb8821d9a0e7d4"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16980) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c38678dcd2d6970a"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16978) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-471a7013de785799"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferConfig.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16986) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f7e904b564a95e3"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16982) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e255526c0fcc8659"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferConfig.SizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L16984) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-90b78726e7d97301"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferRef.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17013) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-046d3342978d613d"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferRef.Cursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17017) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1469f179600f2e17"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaAudioBufferRef.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17009) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7feca9adc0e4f676"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferRef.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17011) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-91ecaebc68a5deb5"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferRef.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17021) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4142dfb0d88aefc0"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferRef.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17015) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-03d4f77ba9b27955"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferRef.SizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17019) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-618d4c0480f1cc9b"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_aaudio = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17049) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a693fa9117a81901"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_alsa = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17047) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1254816483460e11"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_audio4 = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17044) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1f2de0b80155539"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_coreaudio = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17042) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dbae4c389d8b99f7"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_custom = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17052) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c5fc2a14639691ab"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_dsound = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17040) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-69993ae8d5b6415c"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_jack = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17048) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c992a65e98871b4"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_null = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17053) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-54f53876f113479a"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_opensl = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17050) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23152568fd04f3d9"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_oss = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17045) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bfb696b51b53a095"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_pulseaudio = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17046) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af026de47f660306"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_sndio = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17043) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d69bc1aa587f96c"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_wasapi = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17039) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8a29893cdc54f99"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_webaudio = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17051) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc273a40feafb622"></a>[`Inno.Native.MiniAudio.MaBackend.ma_backend_winmm = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17041) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0b722c6c357091f1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaContext*, delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaContext*, Inno.Native.MiniAudio.MaDeviceType, Inno.Native.MiniAudio.MaDeviceInfo*, void*, uint>, void*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnContextEnumerateDevices`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c59169761e8480c8"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaContext*, Inno.Native.MiniAudio.MaDeviceType, Inno.Native.MiniAudio.MaDeviceId*, Inno.Native.MiniAudio.MaDeviceInfo*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnContextGetDeviceInfo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17073) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9236703c4584431d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaContext*, Inno.Native.MiniAudio.MaContextConfig*, Inno.Native.MiniAudio.MaBackendCallbacks*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnContextInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17067) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d0ec56713da2b9e1"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaContext*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnContextUninit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c98ff7afcb8defc"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoop`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17087) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-904942369558e9fe"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoopWakeup`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17089) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c781e404bad636d0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaDeviceType, Inno.Native.MiniAudio.MaDeviceInfo*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceGetInfo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17091) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39b08a31960db357"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaDeviceConfig*, Inno.Native.MiniAudio.MaDeviceDescriptor*, Inno.Native.MiniAudio.MaDeviceDescriptor*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17075) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a57f4db463e2bab"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, void*, uint, uint*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceRead`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17083) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-265551867d74f1a4"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStart`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17079) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d68db46c0a105e00"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStop`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17081) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-994313f80a1c75d2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceUninit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17077) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c8e957bb087921a6"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaDevice*, void*, uint, uint*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceWrite`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17085) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5f3a3c9fd4ba169"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.A1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17122) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9701bc2d10b29c5c"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.A2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17124) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fb0ad7b36ec5b754"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17116) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-773939b4974550f3"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17118) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d7582951a407e6a"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17120) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63ee30585af73b89"></a>[`public uint Inno.Native.MiniAudio.MaBiquad.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17114) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-84a2c7a5875a1826"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquad.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17112) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-35465d7d1ad89063"></a>[`public uint Inno.Native.MiniAudio.MaBiquad.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17132) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d605fa7bbf39109"></a>[`public void* Inno.Native.MiniAudio.MaBiquad.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17130) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89fbe3f6d6f6fcfa"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaBiquad.PR1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17126) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c38df7bfa7f0275d"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaBiquad.PR2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17128) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b84aaa71646f6f1"></a>[`public float Inno.Native.MiniAudio.MaBiquadCoefficient.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17153) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d695e1c0bd9899a9"></a>[`public int Inno.Native.MiniAudio.MaBiquadCoefficient.S32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17155) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-81d4a2a9c16fc39b"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17186) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0044bfc720c8370"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17188) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b56bb8b570402ab2"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17190) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44f7bbde79d1e09d"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17180) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23860d5205109b4f"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17182) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e7cfc62cdd7e8f7"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17184) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4d3c5d6369406f6a"></a>[`public uint Inno.Native.MiniAudio.MaBiquadConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17178) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3548d64f987e1288"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquadConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17176) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a580b0165bc90a2c"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBiquadNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17211) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-259b664132479ea4"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBiquadNode.Biquad`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17213) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ddca3fd52bb40864"></a>[`public Inno.Native.MiniAudio.MaBiquadConfig Inno.Native.MiniAudio.MaBiquadNodeConfig.Biquad`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17236) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-869425287704ba7d"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBiquadNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17234) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1f06a4464206359c"></a>[`public uint Inno.Native.MiniAudio.MaBpf.Bpf2Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17261) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80d3d7d2e6f7a586"></a>[`public uint Inno.Native.MiniAudio.MaBpf.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17259) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad005ae8bc0f5814"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpf.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17257) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-90858e0eab3347fc"></a>[`public uint Inno.Native.MiniAudio.MaBpf.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17267) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1c3be2784fd2515"></a>[`public Inno.Native.MiniAudio.MaBpf2* Inno.Native.MiniAudio.MaBpf.PBpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17263) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b7f4bc965e2c2be"></a>[`public void* Inno.Native.MiniAudio.MaBpf.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17265) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3e11a1fc6a654d2"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBpf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17288) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a82f61099b22fcca"></a>[`public uint Inno.Native.MiniAudio.MaBpf2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17311) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5b7f389a0ed9169"></a>[`public double Inno.Native.MiniAudio.MaBpf2Config.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17315) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-357b36e168348de3"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpf2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f5cd72d6327cb54e"></a>[`public double Inno.Native.MiniAudio.MaBpf2Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17317) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e70d4986d2ec716"></a>[`public uint Inno.Native.MiniAudio.MaBpf2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17313) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db056d0bb30c8f21"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17340) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c1d77df3aaa18a45"></a>[`public double Inno.Native.MiniAudio.MaBpfConfig.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17344) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d057dae1f5043ef6"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpfConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17338) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb071e5d0cb9163a"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.Order`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17346) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39d168f3120759f2"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17342) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-de2656691c32de19"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBpfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fffce342e110bf70"></a>[`public Inno.Native.MiniAudio.MaBpf Inno.Native.MiniAudio.MaBpfNode.Bpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c9be482ceada1e4"></a>[`public Inno.Native.MiniAudio.MaBpfConfig Inno.Native.MiniAudio.MaBpfNodeConfig.Bpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17392) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2d1a59b8a55ee899"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBpfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17390) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3172c47bc604098"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_mono_in = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38611adae4c9650d"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.ma_channel_conversion_path_mono_out = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17412) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
