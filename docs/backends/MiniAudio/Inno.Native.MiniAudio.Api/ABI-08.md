# Inno.Native.MiniAudio ABI · 08

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Head 4eca980496bf8dfbdc4da3faaf2e209ac8e4ed7c02d0940b4bfa6aa0ec7c0036 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Lock e024344e4a07260b1d5675d55e91fb43f581a991d3d3cc962f200b3ca49ac3c7 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.OwnsHeap fbbf0371fe9f51ff4583a5aea67bc6891dbfc571ffda37e9b9eba8e25fe6f409 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.PHeap 240ceaf161d0fa86bfc6bc6f54fc369a7251fc5507bf643ce407ae08db5ad6a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.PJobs 93d4d4a3b0029a6d95312df61ebb7419f37c34de1d0aedc2b855b91a09e28c00 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Sem 04ea41901a8e987052dba9fa1a6b405e8e77cc0493e097d8b108e49db4f6b8cc -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Tail ecb44ee71013742c0c42d5e892c844a71d80b56b4e7fe8f4242e57bef5d0f9ad -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueueConfig.Capacity 17ce244fd2e2ca8a7d2bc2e03b533171e233b5533db7ed9f13feaee2a76b7515 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueueConfig.Flags b4d943c53b3e78fc5e8bd74686aae2a281cdccd94ff73b058507eced221b8a37 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueueFlags.MA_JOB_QUEUE_FLAG_NON_BLOCKING fdd812fb729c1258b89d8c80da4b79b2ed6fde1bb4e56880a421702c284fbe0b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_COUNT 57c6b0a623b648c41a3867de68d49884ce3411ac0574b856c69c1270f3c6200e -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_CUSTOM a9518a13d3c8cabaede80620fe10a2ccdf2d9bf2512f4c1a5079e4be82a9e1cb -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_DEVICE_AAUDIO_REROUTE 954c60dcc9afbce9f7d07b2a463632c7f83c00f7ec4566edb95f90d51d2ea32f -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_QUIT 7f390ec959070c147ed40303f80df6a36cb15141d1988b237f7231d9f6c7fe5d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_BUFFER 96e7193b099501b87a163711c7e73243a99644b2e68514f25c92ce4ac15f15eb -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_BUFFER_NODE be86fbd557b994a8e175a5f1add4f3dd3de54d83aa517b0d8228bbdb7ec20bfe -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_STREAM df48e9a4aba3436c967734ce745f9310aa20fa9ad15e7487243efbee3ede3ccb -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_BUFFER c5e29b28dd00c242774437d24980efaf3a28c5ce7854aa50946c4013fc138dc9 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_BUFFER_NODE 08cec4df23f9766e6607c993941a605309e6d142a44578e8b76d86d3b4b600f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_STREAM d801ec9e3705ae81d90fb106744ad2862a517567fc7dba6d5382d6f9db05d823 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_PAGE_DATA_BUFFER_NODE c7fd3071dc1280acfaed7c144530fe582e484c6bf72b74d61b0e51ac1e938386 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_PAGE_DATA_STREAM 0e7bb93b34baeb88756c82f7aa10b7b10edfb6521c5682953c5e3f13bbd913f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_SEEK_DATA_STREAM 074e7d4a55121e729a42f8d0588fef012aab1e272ac4d804957bd93c594ec361 -->
<!-- api: F:Inno.Native.MiniAudio.MaLcg.State eb937c162f0303fdf94a605c117ffc11beb5bbd91022699f5033a738d0c0da0f -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.Config 3c544c0a26367d287da5cec4a4f5e374dff5e4b64e97e0fd7635b7755dc236a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.InAdvanceFrac f09dcabee53c695ffbfa6738df086a5e080532a3c9d03a525e1da1837c4f676e -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.InAdvanceInt a4b12977240b381e62666eb4131bc5a5ef5903f81d7c5dd2f4783ffd10161e06 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.InTimeFrac bb65a4552b2432d8900ddd74b6919b0c13ba7bac1e437cef27793de8e7e53965 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.InTimeInt c0a0ed96c111cf1df886bfa8d498fd9dc3e03d595270def9bec799af89fd44f1 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.Lpf ae3a0567a20822b2f14539e71002e0a8e4f7751010c3c27a6ca20108c54bd6a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.OwnsHeap ef4b30ae8d5b14eecbe5175e9215fa13dd50d3d9e3aeb8b43ac6c427242d092b -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.PHeap 1d193d5f2a644faa33180f43626931de8a461fb785266311483e161ac83022df -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.X0 dc7853e9e2fc01133b2136d76ff241882b39a94c0efea0f8834379f5005ec82c -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResampler.X1 61a6f59a3982be049992ddf75fb778846213bfe99b27f5b62e2b0c7d3ef7e14c -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be.F32 c117365028dfbf662d5821bba8121aef0f0ba1f8f94d61bb600bf8c4c0565f56 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be.S16 f5207a5e43691a7dcb645009626579fe4acda38009cdd9d0e4afc97ed05f968e -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588.F32 239dd9d4d94739ca4c1c01810013a54025267b8a809c0db4a68e5bddc9cba9a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588.S16 73f94b92f6b05cf92266824ccc3c4ba7564335590367c25e9b5d88d6ea29daf7 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.Channels 51899919c50bff11ba3f165951cc2bcd9dd15106d43f5f49522bac672234d954 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.Format e257b70368f21e935dc27cc702b823605580773f0b324cf5a356a64f86512e35 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.LpfNyquistFactor f012903d82a7f5d4cbb14480bbaf4ddce84d9bf89555f82ffa0685f78ec6437f -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.LpfOrder dd1f45c4237c9374d7f60ba9e7d7e95de0c89d9d5d7f7b24d7feadbbd3fbdabd -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.SampleRateIn 483cf420b9edb1b30cb251cf2744313a892a9712975b0c36243d47b35f409760 -->
<!-- api: F:Inno.Native.MiniAudio.MaLinearResamplerConfig.SampleRateOut f370f671e8dbde5a50ddab41f9520d22577099cc03ccd3020c4b6bbc1e840d99 -->
<!-- api: F:Inno.Native.MiniAudio.MaLog.AllocationCallbacks db5f471b5c5721c36596365f7e07e137538505fcbd9383bb2508ad0a94505446 -->
<!-- api: F:Inno.Native.MiniAudio.MaLog.CallbackCount c0b367a86b091039814329cd4cd69bd892a94de5aca0b3572610d161fb75c6b4 -->
<!-- api: F:Inno.Native.MiniAudio.MaLog.Callbacks a9a1419638500322fab622e7705df33ddf5c2c9e140287f6b3283dbb528012e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaLog.Lock c5f1c72374b0cc7345e30d8cf10e0aa5fb3bb89c46400c238d0559ece77e5f14 -->
<!-- api: F:Inno.Native.MiniAudio.MaLogCallback.OnLog a60376592fca2a77698ad06ff4980e49fc0e9050b4102be9020d3a87b0cbdfc3 -->
<!-- api: F:Inno.Native.MiniAudio.MaLogCallback.PUserData f9f6c16cc237960d49cdbe7a61a05bc7235963f4ad4d18226eb39fcb09fd6139 -->
<!-- api: F:Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_DEBUG 7c4a366ed57e50f64082af105d95dbf356139e8072da0c0a59282385af3d497a -->
<!-- api: F:Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_ERROR 02a23e064eb3c936f69a5fdcde9f74f54ddd88d168a10609ad1ede4e527c321f -->
<!-- api: F:Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_INFO e015b64898cde2cee3f69aa43992d3545f2e5e42ad52185a96907a7643abc4b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_WARNING 011ef68080a2b57b74f2c5ef102616b4711d310a537c811ad3ec4ee6c6c6af42 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2.Bq ed898890ff648a8e87bb020ec07368c20e6a4b5bd56c0301ea36629cd53ff3b4 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.Channels 2b9cf923064c1b07f05812a96a0193aa9d62b2edcf515625baa4ec7a111140ee -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.Format b9aa00c9aa6ff4f5f11669502585008fb988b48f1033974deabbfd1c53c9aedc -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.Frequency 6db5ae6333b0e9d54813b99a270953401757cf9e020a1268f0fa47a837f962fa -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.GainDb b312fbcb8b2ed520d6b20af20cc06192cbd4012fcd664d598d0e795b9194aa41 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.SampleRate d8d0e94d2fac845cc22ef44e810b544844cb0edc41d6e6923bf01a26613e4e96 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelf2Config.ShelfSlope 4601fd44128a66e145feba05f28da8835f83a0a9857c7bc93ae7ce69dfc6def9 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelfNode.BaseNode 2ec858cb0f9b3fcb001bc304a1d7267cffa00302ddf464d4e28d9d7e3f9bd3bc -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelfNode.Loshelf c70e025689105a31345add3a6a882d92641383f1eb095fc8aa4d5c8e54782186 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelfNodeConfig.Loshelf 26bb512165717c57e0e7c1653e939ed08681b8a3e92277ffa5ab856f89b6bb03 -->
<!-- api: F:Inno.Native.MiniAudio.MaLoshelfNodeConfig.NodeConfig b003c3e28fe1cd0cc165ee5319977621012e3b74d4c72d7730518195f8b1d2f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.Channels e3f712278061268df555ad1d1e257ed400ae7ee580076b22e08df840378df669 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.Format cd347adb51498b1bb3e9b7447484b84ebf27992d6e94b5074393618e929449be -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.Lpf1Count 79379c70e4238eea7a3ff025d6a5b814a3d40d21697981471655415f1687e02b -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.Lpf2Count ed090d1065a4942a7c20cb2b24002915529d9812219eabaa746f49960066fb54 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.OwnsHeap 41b6a88d29c0db27628fb49d2cb9c7b9626c460f63ccc65c7f1d65c46d63fe8a -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.PHeap 12c9fbff1a8b67a694f969464f02da54786d6dd76963b4deaebd0529f9555bee -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.PLpf1 4007580701c814825ab50def9fca00fc46eeb7ffef73729b8ae8450376f769ff -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.PLpf2 6f6d0de78ebe187aca58ab3c9dfcdd554fa68053ec42879961debadbbbb9c099 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf.SampleRate 4e64cb0441b6741224c7f9218f29e619ff4b8a00ddb497c431329340dc2b9aaa -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.A 209226cc5d60c0518d80900bb36cbf61bb3eb30027117c0e4855968122abbeb9 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.Channels 9b99cc4f41d6e2bdc0caa92eabcc75e917169aae7697c67c7cfb7390f4d41559 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.Format 911ff9376ca91a7fbd096e4567d806c18ad061b8108aa940965ba57c0357e19b -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.OwnsHeap c14caa7adac52d6ced967385cbaa2beef2cdf609dfb12ad22bdf386272f4413d -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.PHeap e27f1d392c3046a4d3ab2e1f967ebdd49023765f4e88d6a06f6e6d66e70febec -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1.PR1 d0c7bf74f68ed2e9f6b476647ec1b7aa1ce2fbdee6fc6d34f0645cda33703946 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1Config.Channels d5ef5ceb6ad293604181f8689feb6c0403e3478601d435dc05445d6657067406 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1Config.CutoffFrequency 0ea8d5df4678509b3394d37a0ebfa82f4c3f4d5ca82b7338c034441dde835fd9 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1Config.Format 48a4cfe98eab750beb07709fc620fc8f8262c1fc4fbdfa2273420c8747cb9682 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1Config.Q 51e5ce9771f602a5b6930ad23e29bdd705fa039c99b04911f4f7de5593074812 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf1Config.SampleRate ddbd1b2f4ee41e2cc9620ed2881f2fe92bbf7246b189cc56061d9fe3a158dd3b -->
<!-- api: F:Inno.Native.MiniAudio.MaLpf2.Bq b559b45e06cbe442a3e935e1bba2d351e1d287cec91033760558f4aca607901e -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfConfig.Channels c47bbef3d3276ed1fa2faab671cbc9b48149669623a464a7eebe91ed2b94de47 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfConfig.CutoffFrequency e255fe8092ffbb5496bf7b978f38f74315dd65d7a50b8bec1d9964defb269cbc -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfConfig.Format 4a726f2c016d7adc01bfbb1143fc1e9225d54524ec8bd996be55781a093eb9e7 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfConfig.Order 589ab6d8b9ce0180c0154a7e2dfd4ba8ea526631153bec4d2aefe2bbe0454557 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfConfig.SampleRate 24f64c51ffe1d8ec8cf725749cadb323523532c54e54569fbf1bf0e7eeeea4d4 -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfNode.BaseNode afe87153ece24c82bd446692bca746d5af873c639b0f315afb5494598ad7331b -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfNode.Lpf 8dec52d0cf8a6ca43e68049a88937d0b11cebc42800de047ecec13c55898a59a -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfNodeConfig.Lpf e6793b843630cc2dfa5c18d39a729366b615013aae7709bd4c2b70dd771eca1e -->
<!-- api: F:Inno.Native.MiniAudio.MaLpfNodeConfig.NodeConfig b5574b53f9702fb2ebde3284e4bad836648df4201bb8665de84d014fa4f93db5 -->
<!-- api: F:Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_average be729cc2557f8cee0cee224352d5c5963750bcadc1e2ee8f22a38ea0b377efa9 -->
<!-- api: F:Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_default 5d718e68e19cc824b8b1e958d411dcc6b371c9441749236334494c037770b499 -->
<!-- api: F:Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_duplicate 7cdc8ad9b21294b932ab7a3e6c2e96d30061b431a5d781310b7703eea2ef2224 -->
<!-- api: F:Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_stereo_only 94e83ec29bbcd04e8d39c459dad3ab80476de2752de8486a951770ffef40c0d6 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.CachedDataCapInFramesPerBus f00bdf8ba977a90eff58bf253ab559f6389ffa0500e7feced9a4035dc4752e34 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.CachedFrameCountIn c29ccd0542ce62c9a6902fb28c259f74299ecf9293d9e68a4730216c52caa858 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.CachedFrameCountOut d6b931658c1ea6af10f76d68d1ec65c4853f060ce4cb3337be61f83860c86710 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.ConsumedFrameCountIn 4c0225359aba52265a964be013b2caad4568d17c3a6f1083188c9d7ca3b75ec5 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.InputBusCount e38a08bcaa6d24435f9ebb99421129bec1cc2cf6dff2a048dbd1dbc626f9fe8d -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.InputBuses e7eb5dbfcea9f3b2ba30180d5232a69d11148e537c27ee2fa59daa06b2b087b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.LocalTime e3019c1e92bfdc91a188abfc902295be5d074ac8e16600fdd0cd59713f3c8617 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.OutputBusCount d3f4667501b3b418923d84d46b1bcd1ef8feb68f923fae78fb32a8c6859e73ff -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.OutputBuses 44939af1f961e980aad45a8d55f55087d603963ed98dbad4bb7ed30c380965e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.OwnsHeap 9275333e7ad03a84330281fbfb6e1680df09af146fec46988272b21a89fbee68 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.PCachedData 5a140d551f6bd458a3af154f1533e0732016d90ab41c7e1bd7532cfe8c16f4e7 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.PHeap 110d2a2d654f1d77f642c1453aa6f84787ec48bb2f92225786f55e35e20d6434 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.PInputBuses 00695b80cd6ac51c1bb6fb5e280a64fdb91ceb66aa8788b43b60658fef62c927 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.PNodeGraph 26c7ceaebf8b6587504c56a462422cf60fdc29d5983800af2096c703609b2f90 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.POutputBuses fda4958e02ca13d9273e759b858a116bd0f5603b14a860f044d4e1c75b49ee5a -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.State 51e3dbf1e28a104c7568ff7535e28d971baa15e5afb546fe56cf008f890ec82b -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.StateTimes 9461906831c795708613c6e60ecd222a37559fd69801c3bb04308e61e321c964 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeBase.Vtable dc2bfa57612c7d0b5b2f26c181e2cd5baa41bbfce6684097b15a96192c995637 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.InitialState c3610ccf26d30e382ef9a174067e8dd2281acb4c7a3dc86e825a40ae15029488 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.InputBusCount d01e6fd7c32fe8543caea8eedcea939e2ec6e68681ce821322cf15f2550e706f -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.OutputBusCount b604658131182a746a742eaa43412f467e7bbf8b2262e2290c249b53e2af7193 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.PInputChannels 6ee014f397018829de5fd3c5d3166ea1c70da85c1fb72c1dc681c9771dfc179f -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.POutputChannels 0b8ea15fb0b617e6e1471b3a75c15815947900828da3d9902cad827cfe27e507 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeConfig.Vtable 7d50c37eb49ac016ecb2b28975757896aed2e236ab91049e561855b94010703b -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_ALLOW_NULL_INPUT 7ab0a256b5bcd2abc6f2c5e62ea644eb663d824265d8ebe5314d10ff1ed8c0e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_CONTINUOUS_PROCESSING 7b2425302d292372c8da5d8deef6146b07f0ce2a2ddc99aa083a3f2216ac3156 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_DIFFERENT_PROCESSING_RATES 920087b525d24ab17970e179d2bb95d3573bb9c6fa118a8b759bce6e8338759b -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_PASSTHROUGH b8d9ce41a395841653aafb91224279c3db1c9819530f72f93e94e41585758533 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_SILENT_OUTPUT f70977b37de99d08cd2ad3d06978d45dd78299de28cf758e1f341dfa69e729c2 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.Base 7ebd343484a4b98af8b6a394a3a2427d888d85ea5c5d9e339ea9fa9f87cef8b4 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.Endpoint 4f0a0c40fcce552ecfc4193b3e91c6358b67f5da8128f33bf7e7f7a8aee01d0b -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.IsReading d7c4929eee29c1c7b8f9f79afbc9c8159a9e5cf032de55e69b0ebbbfed4f9d45 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.PPreMixStack e6bd49757782557a0a771a08fabcae90fbe6c822c194add05fe12438152ff16f -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.PProcessingCache 41ed2f55e2219b3f354dac4f7a6d699f12568e326be46683600137d43a9e1db4 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.ProcessingCacheFramesRemaining 88d2f3ca83d8c29c213ee5123054f28638170b98c2b69005b5b0acfe712fc0e7 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraph.ProcessingSizeInFrames df0d63bcc307be734afbef23571fe39d223a96c30cb408138baa097cb1e8c77e -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraphConfig.Channels 078414aaceb57766885074f4f22832538f3ef80b1dd4f3c1f53f4a249a965593 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraphConfig.PreMixStackSizeInBytes b41d1c20af6768f62dd66c338d981f220bc765f9dd30612c77b06ed4c6f21bbb -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeGraphConfig.ProcessingSizeInFrames ce6f6ec84c7c190d83e48b2b407ffcb6824a6f893dbe23c195e069dc13537729 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeInputBus.Channels fef25fd253422e9963da23039f4bce550f2b61ab5d9ee179f8c70d526bd5764d -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeInputBus.Head d8ac1a3287d2228a9c02665015550e91196ea89788bf521d3ece9b887d392c94 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeInputBus.Lock d992da79c83f2550a1961a6eac9708959370548012e64cd5bf3fb13f4f166183 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeInputBus.NextCounter b641ab8998b499f44312911f17e3dc56a30aecec6609d456bcdb3932319b7c29 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.Channels 59a1a38b248827d15b8d5b9f26e8eb4129f1cdf1e9b420da470df6eacc3fa010 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.Flags eefb62b84444b0c05575da5b935ebb9914a33acaae1d76fa88cd5f3d7954338e -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.InputNodeInputBusIndex 37b68bf4403e111020400e892094d1defcf10e59ce45c8caa32a9bb4196a3a3c -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.IsAttached 4ccefb639059eb9a7c52c619219457a7fce5da906f5349038a4e1cf8a59a0613 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.Lock 77bbc9c60bfafbcb1fd3de65f04ddc41098002786421f5e8dfd0e31f7b71d4a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaNodeOutputBus.OutputBusIndex 900ce770650fba75b805b7dd0ef65e09cebba436d3012da27a5565f7a13e26e6 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-8e0b3eec67f5e92c"></a>[`public ulong Inno.Native.MiniAudio.MaJobQueue.Head`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21628) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5b3da4130e33128"></a>[`public uint Inno.Native.MiniAudio.MaJobQueue.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21638) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-20ca7982ce70a678"></a>[`public uint Inno.Native.MiniAudio.MaJobQueue.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21642) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-651916576752a7a9"></a>[`public void* Inno.Native.MiniAudio.MaJobQueue.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21640) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c8f5ad03aed80e5"></a>[`public Inno.Native.MiniAudio.MaJob* Inno.Native.MiniAudio.MaJobQueue.PJobs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21636) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c0833e7254a8f556"></a>[`public void* Inno.Native.MiniAudio.MaJobQueue.Sem`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21632) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b3becc6e62c650d"></a>[`public ulong Inno.Native.MiniAudio.MaJobQueue.Tail`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21630) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1747b0ae296722e"></a>[`public uint Inno.Native.MiniAudio.MaJobQueueConfig.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21665) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3b979cd1cf0bf2da"></a>[`public uint Inno.Native.MiniAudio.MaJobQueueConfig.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21663) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8320f28322d635d3"></a>[`Inno.Native.MiniAudio.MaJobQueueFlags.MA_JOB_QUEUE_FLAG_NON_BLOCKING = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21683) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e24cdb2cab85a6f9"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_COUNT = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21706) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d4fef969dc1976ba"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_CUSTOM = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21695) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3187a00f458013a"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_DEVICE_AAUDIO_REROUTE = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21705) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d4b9f90071edf94"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_QUIT = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1da0337252f9806"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_BUFFER = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21700) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8a44890bb2130d83"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_BUFFER_NODE = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21697) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93aaa5d9bd309bca"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_FREE_DATA_STREAM = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21702) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75fe0b5e10dcea3a"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_BUFFER = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21699) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8be064bbdd9f3ed5"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_BUFFER_NODE = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21696) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d072a3cb9c547c8a"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_LOAD_DATA_STREAM = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21701) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22521337660342a0"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_PAGE_DATA_BUFFER_NODE = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21698) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e4d5b109a8f827c"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_PAGE_DATA_STREAM = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21703) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-76aa8a96f9b89655"></a>[`Inno.Native.MiniAudio.MaJobType.MA_JOB_TYPE_RESOURCE_MANAGER_SEEK_DATA_STREAM = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21704) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-efad26fa5ba79881"></a>[`public uint Inno.Native.MiniAudio.MaLcg.State`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21720) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-891bd657a8a44dfc"></a>[`public Inno.Native.MiniAudio.MaLinearResamplerConfig Inno.Native.MiniAudio.MaLinearResampler.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21741) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d3e0ca52f210523"></a>[`public uint Inno.Native.MiniAudio.MaLinearResampler.InAdvanceFrac`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21745) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-920be2499f1d738a"></a>[`public uint Inno.Native.MiniAudio.MaLinearResampler.InAdvanceInt`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21743) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55390fde043f3ffa"></a>[`public uint Inno.Native.MiniAudio.MaLinearResampler.InTimeFrac`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21749) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-635aa5370f3f9610"></a>[`public uint Inno.Native.MiniAudio.MaLinearResampler.InTimeInt`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21747) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7385884ff2698341"></a>[`public Inno.Native.MiniAudio.MaLpf Inno.Native.MiniAudio.MaLinearResampler.Lpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21755) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f071f6360e99eaa0"></a>[`public uint Inno.Native.MiniAudio.MaLinearResampler.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21759) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-117167d8263a263d"></a>[`public void* Inno.Native.MiniAudio.MaLinearResampler.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21757) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8dac40cedca41ad"></a>[`public Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588 Inno.Native.MiniAudio.MaLinearResampler.X0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21751) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-741794ffd49daf53"></a>[`public Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be Inno.Native.MiniAudio.MaLinearResampler.X1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21753) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8a45637297226f17"></a>[`public float* Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21780) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7cb8f4f2663250a2"></a>[`public short* Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion04e50dfd327cc4be.S16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21782) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4fee3c377eff7119"></a>[`public float* Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21803) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec134808eec8a8e4"></a>[`public short* Inno.Native.MiniAudio.MaLinearResamplerAnonymousUnion6e85c913901ac588.S16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21805) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-873fb9cc9e87a549"></a>[`public uint Inno.Native.MiniAudio.MaLinearResamplerConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21828) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9105d799c28d6669"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLinearResamplerConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21826) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5385be4a3639b88d"></a>[`public double Inno.Native.MiniAudio.MaLinearResamplerConfig.LpfNyquistFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21836) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c8edfff86bb1b95"></a>[`public uint Inno.Native.MiniAudio.MaLinearResamplerConfig.LpfOrder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21834) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d66581f167423f8"></a>[`public uint Inno.Native.MiniAudio.MaLinearResamplerConfig.SampleRateIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21830) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ac591edce3eb6d8"></a>[`public uint Inno.Native.MiniAudio.MaLinearResamplerConfig.SampleRateOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21832) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00cbb8a2417d3419"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaLog.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21861) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-154c76379a085541"></a>[`public uint Inno.Native.MiniAudio.MaLog.CallbackCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21859) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b3cdb7a3582c2d4f"></a>[`public Inno.Native.MiniAudio.NativeArray_d8db4ca1d386799a Inno.Native.MiniAudio.MaLog.Callbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21857) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7a277e0842ca9446"></a>[`public void* Inno.Native.MiniAudio.MaLog.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21863) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6f16e523d3772db0"></a>[`public delegate* unmanaged[Cdecl]<void*, uint, sbyte*, void> Inno.Native.MiniAudio.MaLogCallback.OnLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21884) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f379de7fcdbbab7c"></a>[`public void* Inno.Native.MiniAudio.MaLogCallback.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21886) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9cfcbbb91d197d02"></a>[`Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_DEBUG = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21913) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-beb594de6bb2f834"></a>[`Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_ERROR = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21916) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-add4a6a5c811b5f0"></a>[`Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_INFO = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21914) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b17611cc761ada50"></a>[`Inno.Native.MiniAudio.MaLogLevel.MA_LOG_LEVEL_WARNING = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21915) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ef9aab2d52227ee"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaLoshelf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21930) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e3d26c06ec64760"></a>[`public uint Inno.Native.MiniAudio.MaLoshelf2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21953) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec1cc2c74045ea83"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLoshelf2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21951) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-beca0690b0faf325"></a>[`public double Inno.Native.MiniAudio.MaLoshelf2Config.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21961) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8d3f2bb39d3e0e5d"></a>[`public double Inno.Native.MiniAudio.MaLoshelf2Config.GainDb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21957) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-983215645677987d"></a>[`public uint Inno.Native.MiniAudio.MaLoshelf2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21955) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c5764bdbcca18de"></a>[`public double Inno.Native.MiniAudio.MaLoshelf2Config.ShelfSlope`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21959) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99fd388415e53cd3"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaLoshelfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21982) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e3ed6348afe7073"></a>[`public Inno.Native.MiniAudio.MaLoshelf2 Inno.Native.MiniAudio.MaLoshelfNode.Loshelf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21984) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b9bf3dcaa4f5cd7"></a>[`public Inno.Native.MiniAudio.MaLoshelf2Config Inno.Native.MiniAudio.MaLoshelfNodeConfig.Loshelf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22007) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-937013e8aefd6981"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaLoshelfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22005) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba628ca32d934b8d"></a>[`public uint Inno.Native.MiniAudio.MaLpf.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22030) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-36aa8f512a9ee127"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLpf.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22028) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7cde658fe52cc845"></a>[`public uint Inno.Native.MiniAudio.MaLpf.Lpf1Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22034) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-60639d98c58fe977"></a>[`public uint Inno.Native.MiniAudio.MaLpf.Lpf2Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22036) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ada469a756b68c4"></a>[`public uint Inno.Native.MiniAudio.MaLpf.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22044) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-98e7c6ae54589dfe"></a>[`public void* Inno.Native.MiniAudio.MaLpf.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22042) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33ba8b9e9552f0b7"></a>[`public Inno.Native.MiniAudio.MaLpf1* Inno.Native.MiniAudio.MaLpf.PLpf1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22038) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b949a615077b69c0"></a>[`public Inno.Native.MiniAudio.MaLpf2* Inno.Native.MiniAudio.MaLpf.PLpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22040) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c6297e05c6872e4"></a>[`public uint Inno.Native.MiniAudio.MaLpf.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22032) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1cc37a1f2b08dd5a"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaLpf1.A`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d186327f7272b89"></a>[`public uint Inno.Native.MiniAudio.MaLpf1.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22067) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c26d1aac340a1941"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLpf1.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22065) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d239031e6204713"></a>[`public uint Inno.Native.MiniAudio.MaLpf1.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22075) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc8998f782f3fa4f"></a>[`public void* Inno.Native.MiniAudio.MaLpf1.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22073) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72c27feeb273ac42"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaLpf1.PR1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bbfff1eb2b32dfe2"></a>[`public uint Inno.Native.MiniAudio.MaLpf1Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22098) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8e0b6a4a8af5836"></a>[`public double Inno.Native.MiniAudio.MaLpf1Config.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22102) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-477025bbeca24970"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLpf1Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22096) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8cdfe1e094e195fe"></a>[`public double Inno.Native.MiniAudio.MaLpf1Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22104) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-49e29fc34f52d1b9"></a>[`public uint Inno.Native.MiniAudio.MaLpf1Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22100) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3c9173b75fe5adcd"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaLpf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22125) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b72a937a35b6f12e"></a>[`public uint Inno.Native.MiniAudio.MaLpfConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22148) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-05f908726d11b00f"></a>[`public double Inno.Native.MiniAudio.MaLpfConfig.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22152) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-391e560feb1c1aeb"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaLpfConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22146) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a2a76be85c053442"></a>[`public uint Inno.Native.MiniAudio.MaLpfConfig.Order`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22154) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f17b8d782166a52"></a>[`public uint Inno.Native.MiniAudio.MaLpfConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22150) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-03ddb9f8de257f20"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaLpfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22175) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72cbcf2493585c9d"></a>[`public Inno.Native.MiniAudio.MaLpf Inno.Native.MiniAudio.MaLpfNode.Lpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22177) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c438aac7d57080d"></a>[`public Inno.Native.MiniAudio.MaLpfConfig Inno.Native.MiniAudio.MaLpfNodeConfig.Lpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22200) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8ac8de7e573c5b08"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaLpfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22198) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-199b9faaf409dc39"></a>[`Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_average = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22219) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21ee2d758b6d442b"></a>[`Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22221) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2977897a6ca90652"></a>[`Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_duplicate = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22218) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-87a6525cd4526b07"></a>[`Inno.Native.MiniAudio.MaMonoExpansionMode.ma_mono_expansion_mode_stereo_only = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22220) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f5c3f0afc85548e0"></a>[`public ushort Inno.Native.MiniAudio.MaNodeBase.CachedDataCapInFramesPerBus`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22249) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5b71d71f17ef8857"></a>[`public ushort Inno.Native.MiniAudio.MaNodeBase.CachedFrameCountIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22253) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89765ea64fa90c71"></a>[`public ushort Inno.Native.MiniAudio.MaNodeBase.CachedFrameCountOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22251) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f20631690a33e626"></a>[`public ushort Inno.Native.MiniAudio.MaNodeBase.ConsumedFrameCountIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22255) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6ac01784770f7e7d"></a>[`public uint Inno.Native.MiniAudio.MaNodeBase.InputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22239) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-907343bc0391050f"></a>[`public Inno.Native.MiniAudio.NativeArray_5cecd9fb8864fb43 Inno.Native.MiniAudio.MaNodeBase.InputBuses`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22263) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b40751ca5e97ce46"></a>[`public ulong Inno.Native.MiniAudio.MaNodeBase.LocalTime`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22261) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7baf516bb45c8b2d"></a>[`public uint Inno.Native.MiniAudio.MaNodeBase.OutputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22241) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f6a1f3f3d45a6fb"></a>[`public Inno.Native.MiniAudio.NativeArray_d14c56c076b259b5 Inno.Native.MiniAudio.MaNodeBase.OutputBuses`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22265) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a2fcb1905622fd8d"></a>[`public uint Inno.Native.MiniAudio.MaNodeBase.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22269) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42a4609cbc567515"></a>[`public float* Inno.Native.MiniAudio.MaNodeBase.PCachedData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22247) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-512c1da4e98837a1"></a>[`public void* Inno.Native.MiniAudio.MaNodeBase.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22267) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1d1efc131f66010"></a>[`public Inno.Native.MiniAudio.MaNodeInputBus* Inno.Native.MiniAudio.MaNodeBase.PInputBuses`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22243) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13cdfeee9b70b32d"></a>[`public Inno.Native.MiniAudio.MaNodeGraph* Inno.Native.MiniAudio.MaNodeBase.PNodeGraph`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13bbf8c3d76b6ba5"></a>[`public Inno.Native.MiniAudio.MaNodeOutputBus* Inno.Native.MiniAudio.MaNodeBase.POutputBuses`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22245) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88ced94fb4a50c71"></a>[`public Inno.Native.MiniAudio.MaNodeState Inno.Native.MiniAudio.MaNodeBase.State`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22257) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0940190a2a01cf07"></a>[`public Inno.Native.MiniAudio.NativeArray_3561217d0d8daa30 Inno.Native.MiniAudio.MaNodeBase.StateTimes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22259) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a25b9cc698f23ed9"></a>[`public Inno.Native.MiniAudio.MaNodeVtable* Inno.Native.MiniAudio.MaNodeBase.Vtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-51d72f7c145e4e2a"></a>[`public Inno.Native.MiniAudio.MaNodeState Inno.Native.MiniAudio.MaNodeConfig.InitialState`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22292) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f67ef7197a689e45"></a>[`public uint Inno.Native.MiniAudio.MaNodeConfig.InputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22294) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-86ccb3b186ce7c82"></a>[`public uint Inno.Native.MiniAudio.MaNodeConfig.OutputBusCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22296) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-122d1978ca541a60"></a>[`public uint* Inno.Native.MiniAudio.MaNodeConfig.PInputChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22298) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-126c78e2acd0d8de"></a>[`public uint* Inno.Native.MiniAudio.MaNodeConfig.POutputChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22300) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0df9c2eb67620cf1"></a>[`public Inno.Native.MiniAudio.MaNodeVtable* Inno.Native.MiniAudio.MaNodeConfig.Vtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22290) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4a9cb2e5ac528b3c"></a>[`Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_ALLOW_NULL_INPUT = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22320) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d3de024a3901424"></a>[`Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_CONTINUOUS_PROCESSING = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22319) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-52596098a899dd04"></a>[`Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_DIFFERENT_PROCESSING_RATES = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22321) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ea53552736bea859"></a>[`Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_PASSTHROUGH = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22318) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d46e03076133133d"></a>[`Inno.Native.MiniAudio.MaNodeFlags.MA_NODE_FLAG_SILENT_OUTPUT = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22322) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e66f70316b472c3c"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaNodeGraph.Base`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22336) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-00ca680724a7d9a4"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaNodeGraph.Endpoint`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22338) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ee91ff79834525c7"></a>[`public uint Inno.Native.MiniAudio.MaNodeGraph.IsReading`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22346) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7702da1cc4c44ac9"></a>[`public Inno.Native.MiniAudio.MaStack* Inno.Native.MiniAudio.MaNodeGraph.PPreMixStack`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22348) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9948e8028a0f8d05"></a>[`public float* Inno.Native.MiniAudio.MaNodeGraph.PProcessingCache`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22340) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8607abdf0872ec00"></a>[`public uint Inno.Native.MiniAudio.MaNodeGraph.ProcessingCacheFramesRemaining`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22342) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b8b3caeb4acc0f5"></a>[`public uint Inno.Native.MiniAudio.MaNodeGraph.ProcessingSizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22344) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-36f871e888359367"></a>[`public uint Inno.Native.MiniAudio.MaNodeGraphConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b18800ecbf0303e7"></a>[`public ulong Inno.Native.MiniAudio.MaNodeGraphConfig.PreMixStackSizeInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33d4202033bd08a2"></a>[`public uint Inno.Native.MiniAudio.MaNodeGraphConfig.ProcessingSizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22371) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e7d5968aa60dba3f"></a>[`public byte Inno.Native.MiniAudio.MaNodeInputBus.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22400) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d840fe68f8265aa2"></a>[`public Inno.Native.MiniAudio.MaNodeOutputBus Inno.Native.MiniAudio.MaNodeInputBus.Head`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22394) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e307e7586adaaba"></a>[`public uint Inno.Native.MiniAudio.MaNodeInputBus.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22398) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7df6b781e4473d9"></a>[`public uint Inno.Native.MiniAudio.MaNodeInputBus.NextCounter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22396) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-822eb7178ce3a866"></a>[`public byte Inno.Native.MiniAudio.MaNodeOutputBus.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22425) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-089d73edb1f1e492"></a>[`public uint Inno.Native.MiniAudio.MaNodeOutputBus.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f2ad6b32a8156f0"></a>[`public byte Inno.Native.MiniAudio.MaNodeOutputBus.InputNodeInputBusIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22427) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f5ae1a632dd54bf3"></a>[`public uint Inno.Native.MiniAudio.MaNodeOutputBus.IsAttached`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-489c513d519698fa"></a>[`public uint Inno.Native.MiniAudio.MaNodeOutputBus.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22435) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-368c71025d5e6294"></a>[`public byte Inno.Native.MiniAudio.MaNodeOutputBus.OutputBusIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L22423) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
