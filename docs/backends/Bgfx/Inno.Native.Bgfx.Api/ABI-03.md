# Inno.Native.Bgfx ABI · 03

[项目与生命周期](../Inno.Native.Bgfx.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvFactor 10c089020a3ee42a7cbf1c75bb9e5b9f77d96213a19768c383e640c802e4dc98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha 0127d7d285b7502adadc7c25595e8a17ecd2b1311b2f58888a063ebbe75c5c0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor 89b995f535bdbbca09886e5012a6002285a999718d1ce924391e6f9ffda0cf41 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendMask dc11506694303c38a7ba221bd451ae91c474c802c5c066611cb9be84acb9f5c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendOne 863a27f1cf65e8dbff01d805003c06d626e57a9e304260dfa014245127da5e0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendShift 3c80afe6a9855bac76acf453809ae10d355138e7c8669f0f1a9feb754ad3debf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlpha 828918700e97bbd274c335c95f0905114d3329097da73691a0d8dcbe1303b568 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlphaSat ac3278556ebe68e2c5e6c1a08fc45085eafce49fa9cd0d141c3a4a40b3458774 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcColor 3752e140b78ce9e3cd3a0c13f52176a54867462ce9a42c25b6fbe167e4a373eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendZero 7cdd2be0e145863f59e69fb3b7257a0b88e6b7a0e3a7d5f4739cf89d373d817f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ConservativeRaster e51f7506e8e2cd6142ad84fda6bd6ccb4666d9e4de37b36977a381aa412b3e2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullCcw e551c9486973c2cd94ac3f034190530b4fd990611e4da63d47b5bdf82783ddae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullCw f4e2e8684f0ef53fdd77a5cdf5580a078351b6a6ffc72efe08505233e821de4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullMask 0c6a87cc37e8366a395cd063f985f2fda45533caa7f962a03f52c8b73ddbdc76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullShift 8fe835e2aef7b4253a12fa40734322b275c8e2390a15923f1c90720fd5e4cf8e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Default 079a757c4fbde83f8b23673c57d9103d0b814f64f2fe9029a5d1184c670488ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways 4fbe7d219d56415222e164371dda4c904c6e90075af235d6cd864db14814d98c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual 91e350c21716d1b13cf02e897c240609ef249739bb550a1728d651f5a1c5d940 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal e0cb4f92448c65de89841198567f4942b126172e2c6aa6db54256b8e972b21d9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGreater 574e284d36ec9a81b5bfbef7c070858a2a2fd9f07f11d8836a0fd2108c255df8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal 4bc72141d54bcbf04883fe064b57864b901939bcfc429358a9958ad6e251f19c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess 7c1ad9b1a47aaa339a7099c04bb5598a3b8051021fa1876efb361dca0dba2b91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestMask a1832dbb39b61f6a0a455d739578bff58988f762e25ef30d26ebb2a7a5488d03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever 27ec0f3e6552f0747eece2454efdd1b91545960f28bc16241281a2a9309398c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal c16f79ddfef1ce67232c60235adad5215619e16b77ba398b027923e899605f5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift 3bbc59618e9d0b86d3bc422e30c466e61941a792f4e534a1ac5ffd5945666c19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.FrontCcw 19d3cf42fda8c109b72a7c8f7de35b88ace855d92f18800d1aab96901a135391 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Lineaa e93f9cd3eafaf4a333ea46291607a9c827d3bc37d3510f552e2bcbceb4194994 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Mask eb87da757e445a55866ed15c17d066e11efb9e5d6a824f738d3c54d9dddb5bd8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Msaa 73857ae5e7393d1f80994bff68ded3cc89f85de0ac33dd8e68f0de40ec1eaade -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.None 08c0808e038bb200666208e95c651c2cd0a5c1df06120098550becd2569475e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PointSizeMask e3ee99db75a07a837be6105f237d1e7a42e654f4a2925f98c0173803a2238b78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift 71057d5d2a770ea2619273aecc7b6c9863c319ae92b9d5062076c7bc4fb6af8d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtLines 1195c30df7ec757cf31952df6fbabf9eb7a593eb0c091946f12a52c7456146a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip b9944dfeff0bc11f8208e3b16aa53f03f0e10a9954e176c6ee45606075da911a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtMask 4452875cc205eca1ec8bc961ec4056a3fed4741c1fc179178e81be6695744412 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtPoints a9a077e88bd1d756131d00c54e61546a15bae3f0d022ed7d5a37e71ba8318dcc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtShift 0441077150279155a1906bb58144d5886640040597ad9ddce62441cee99e8a3a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip 9c79671d2eead2f45563637482f9a2c85c0b23bb6bcad35e5095364c6b33e8f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ReservedMask 23bd59ec6982e784e7ed0ba8088a509786871f6f5b6c53d676ca72edb03bbb2c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ReservedShift 5b994ab97c27fd9c75ea807a217d2e716cb9a356458cde7aa40560ffe5856ab1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteA a2e3842c5ae616f7817e0f3bdf43d8ae6de07a5ab764a7266a07d7f7a41ea0c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteB bbcd49c9db451b0a78204e1524fbf6c080b081970ef3084ad8cc5f32f037d954 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteG d369c91355f084832fdfe26922080aa6ea9f3d1b66eff635ac6d92b6b2577be3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteMask ff57585da60fb5b499f4ca58554bbed8f300b03f06f0b8f9e01f0a5cbe1700ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteR 5f1c4910766e97114984aaf1345337e7e09f705d2ee57e19e896f257f0ebd337 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteRgb 4b37ae4ad753767e86931858a416920e3c2cd540e85d398b5bc25c6e60a8f38f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteZ cc1e18e008013193e148d76cc8ca4adfff54a416c7198608e13ed4f783fff3d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeBegin 9419728bda5960691b14c2662e504627a974a065618cc3ec014c5f01b0c621e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeEnd ffabfb887c33f2b322396bb5b1916371b1ead108cc59cff2ef26818ca07779e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeFrame 6ee1fc2ea4dff1113181008fa7ba188b6dd43739227c9a8c1cb0ddb42c94d26d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimerFreq 54644f580f2657c078571db74cbbf908b850cb30b41dfe59d06dfa0cf78bf4ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.encoderStats 0c70477a85dd40a94d5b63fc59b9d5b8d3bacd0fa618df39e464bc978b5b4561 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuFrameNum 176ce586f947b1e5c82809aa0da40a7bafb5dbad13c2fdd4550486158bfa5ea0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuMemoryMax a6b534f797ecb0a2d3185e45996b70e786e250cf9978e4db98bff09438383bc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuMemoryUsed a5dcda14ded3728f7aa91625c6be0095d2aad02ca9e305d428ffc7367acbc175 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimeBegin 3c0b51e5383cec8e3aaec6053160adfd08142e4423348628cc4bc195ccdadfc2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimeEnd 840459a5c99552c4fbc7bc728d90bb72d4cd1ddf9e8a3b132e62ed31920d06dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimerFreq a928763f79848ad81d54b4c86d46f2a34ff7d525a5830bdda86edb468d40d54e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.height 42c93fd00c8ce91f5dbe2c1c4f7e8fe67f34a14f1c4684abda76bab88fce75d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.maxGpuLatency 566673abf94af68ad1d971476fbe503ebfa4894fba9951c8d248c40bf2b24805 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numBlit 615512f13c1afc0fba7ba88ccf916a6c228d90e29d177c56161a082539c03b97 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numCompute 68b1bc6d3998cc648d3ae826be64f1794f5799b6ffe353fe9448d9be833d1400 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDraw 0c42df5f18a94f815eaeacfe54b4e27559e20b0301ae609c0ddb5de7cad2681a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDynamicIndexBuffers 8b2aa6fbf4949288b70fea8140e7411502dbc233f0e6bef7762aafda8140c475 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDynamicVertexBuffers 4e89e696d04093b02cf37aef349ff37a327f921e6db7211618ec99535c9636a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numEncoders 60c1df37901f5a216f58a53e60010a305326886ae5009eb5eb0ad5073c1bd1d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numFrameBuffers c0e947a58a029945c9029e9d6d52eccbd9db88b31c05ca8b42e40f47cfeb28f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numIndexBuffers 622518ff073bf8e9b0b921a58acc986d5754194069225f72959b9224029bd649 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numOcclusionQueries 12ad12f24b0e9c15a5ac358f922a2d9e8a91cfa47a5e07a7e3a6417025a8eb55 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims 9cfdd027a98235495a3b6557196e54c59aa9d4ff1223895e3d267e96c967d02b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrograms b2506791b3f4d04d171ff9e644b92b99b4e8b6c638fd7a04cfce0ae77abb8095 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numShaders b506a89538356b2b1712b497c7226accaf95d4db1d40ca6861587590ebdb5063 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numTextures 913d381e8307d0149c455cd7e2a73508a8296bd2602f64a295ef5134cafd8a90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numUniforms 19a89945b54f5c2589a63982a5d1897d6d6b7aa0b9eb7799537d9378b2f9f5da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numVertexBuffers 299ba79e91f37a9bd8941a199c9debf0a60c7af3d723ca34349cb41a6295028b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numVertexLayouts 54b236d6bcb98f2960e0b4c423fc0bcfd67c5f494f4afcdcf15cb17e306cd057 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numViews 43598473298b21c58d973f1614f07abbf7bc9a39e7e925074f73375df7986307 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.rtMemoryUsed 6c4d8ab608c6ad8402a5146e5a247d58b0ccb03a0e5f3d11f723578970963b91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textHeight 0bb7a2e0b6f051636c1a3363e21495291414c42086217318ca8929a3573a8e85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textWidth 4f89e41f31fe7fe17d7805c8310ebb718d56f4774e545705f072e9851e2726d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textureMemoryUsed dba41c5ac02219c48172c38c254b7e1c1476d2e34c466b7ead9a73ce50c17de4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.transientIbUsed 4001af8426652c5cc8b754bde18a8ffb0a25adbc8df5dc0fd20d013c4f30c609 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.transientVbUsed bd3daa5ff0a46391646ba4b5d2bd8a420a4b39807ac16cd542edb3ce1fa4a118 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.viewStats 6a3c39226c6cee4612ec283f12c9eb5d15c720d54041a01018d917f894f953ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.waitRender b9fb3b1a118d63d63fefd9237233645848830b7647eec08be6cd7c71dae1687d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.waitSubmit 428266567c38f0267a8643b4636e0f7195f4331d1aba302f48187060fbeaa7d9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.width 2911764368b3f8efe9a2f432b06986ba2ff350e00fb9fc4f89fd88d0234c7d9a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.Default 877f25ca3ed70bc1db52ae518386210ea5ed0f22a16aa547fbf3358adfe24f63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask 2a46c717a08353db549320d44e5d0c74e793ae7ea98b385f6a9a080c7cbc06be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefShift c30b923d444f4647a4aeba77387d905a8f355150feaa529b8bf149c378326c66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask 5cab34795b39a5ef776f31b91544b043b8f824a934a0fc30fb963bd5ebb18fbc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift 618edc9be6f6635e525cb461c50819e22e31876f055576d56f4cb6013000a708 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.Mask dd4d52d0b67416483e2c7a1483f79da0ee520dc82a2f2618c39939be00d41290 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.None 6cc0305ba49150276a210b93ba0d9da859ee1508b1e6f437cb470852da2579ab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecr c3073345235e373133c33185a31a7d18a8917eb6e2a473abdf8dfa24f8d7c3db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat 87b10ff12acfea28f064d38089540813c0758b3f7625d4cb359dee8b39af5a23 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncr afe425a94f499b733a8bd47c79189cfa2c27444dd33a09717240506a2934dd10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat 207f2d9b1b53ba554057800982a084560e9604f3c45d4b85982c40c047a5ecf6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSInvert f47013483ceb8e0fa0de448b55e004f60785acf4ed57d310434f9350b6e4190e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep 261b82db536ac9f303f2ff78e9ec75bfa432c7c05239c811731dce46be859b11 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask 16a1fde196996c3aecf34b9039891bf663a556c64a8d99bef3be5ce40b169d31 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace 1cd8a4898eb19bd351e373b0d091363351755a8c25413d78730dbac6098ec400 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSShift a5e3d6c6f178fa7b8f0ec3b2f6e370447075429196c50c2d60b83eab7b12ccea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSZero df49178c12a5f8b04ce4d85c0a923519a1f3cfb81d8aef4014bdb29adc5a8383 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecr f628abb3dddb5b555676b90c5fb31e2d6a2b0edac77a3241be586779f3016b82 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat 8399da8afee1bcf3542e62e79266acc6be80b007fdda451ad9210ec3f6ebae89 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncr c4e6e02115b165994ae011cbc17bd44b73ce06314d1c4b610b00626d6782fc51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat 0812a4f589b08930e9b58f7102c58e6537fd8b11185359b0810ce1feb0d21ddd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZInvert 77990b8d4b55496d0356768708b90b3a102f81f48086e841aa2d74a922970c55 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep 3671b401a587069847d28434ee11a235f495b30425a483f8ef698b82977050bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask 7fce6ebdc0618521fae336c8491f6908fdeae91a4447fbbfa3475996241115e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace 591873d6ad4237358d181276918224c1e014e7dfd78f12d511f0ae6de0766e57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZShift e916d284c1e17925ab946ec7126a91fc8a3ddda85fde3974390f47d51324a51f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZZero 286e04506dd1d21467c4549e436c0996fd9d7cc622c162bff23d0094d1b4df08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecr 3c74b00609f9e17959406ad7712500f66e1fe1bd9cd224500f3426ebb6a733d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat 4048cc7e9b4895d550d6bc839991e9f843516b91ce6419431faea3df92829bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncr f06134d07ae90fa7df6e5770048c683f2c2be405d9c125ca066e9e9a4b98af7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat 0bb66b3e2280d3f9dbe521c90a8ebb81539d2cae832e021a6b8e6adfecfa5b4d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZInvert 4e55a065743ee2b60d198eb3bcf06d8cc042328b89be2635804c33a2dbf046e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep 67651f79ff29f7588bf4caa7d8dc1e94f78df068f4f5769e3406232590aebacc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask 9b6d290376b8935c50afa7cceae4acf722d4c9b3e873ae55d8216ac2afc1929f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace 60dcbaf5f847698d3145d8c98b9d7e69237025ad6c15d3b43f396832071c7fa7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZShift 7e4444651a575b4d5220049b2138234565feb8891236a0d80926686062834350 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZZero 04712a9b6c7b947b20fb84146516753a3bf7af7f3de96e98ddc1dfd359cb131e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways 4d9bf031a93c0d77445035620ad59386971fa3ecd5359f07346a4c9831035cb0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestEqual ddb24384d2a968a6fe77457d90a87652768b250db3241419dd05ea25861c4fa3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal 861f146817c9fb2b01bdb76ef57034210963d1d1105bf8ccfa4465d2ba52bde9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestGreater 70c56cfb3a6330704a07e808f9139fce9d567057e97c42c6683842da32120ad4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal acd0892745c4a99bdb7ec15260f4c957776078a12d1340cab6855327e706dbd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestLess f0fb6a34eb16e3982f1ac22717334f37980acf613f5fdebbe6924785c6b06945 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestMask 8b75570e059b96c99fbbf9212bc096df849c4caa6a3195a5258a3c8dfa995e3d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestNever f4565d33acb23d70b7fa3c4b9de693730e4fa91624be58ef42c18c9719f49515 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal 62dcb476848c08185322968200c37bb8da0390eac9a7fab97bdb16e2e5a178f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestShift d180d3e3fab5424fde6efc91c5a6c9dfcfb78c29947a435890d9a5d85843a508 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.BlitDst 63c5810733f5326259d95e014ac00527b05d8cb7ac67cee6c14c0553f80d34ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ComputeWrite 490f60bc1394a6ebb57fe9caa43d22272c25176d70321bf899b0e5b8b9036c9a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ExternalShared 0b2a4bf061f4de587890c1eaaf63c88b48fce352a8cad72bb4ddb61a62248522 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.MsaaSample dd103f1ef08809960486d311bebef2f20eb6176f4275f48bb4ac06d88b39a21a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.None 4107b87f05d0d46f65e581d65c2010b918b8dc0af02da401c10becffcae5fc61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ReadBack b4159c91c331d0407d9390548c07cba5eeefe23e8e24c50b18951a945e71858c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.Rt f08366f05dfb9877e106c26acde993d25e438cc7f174c40abd0c211e880d531c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMask a57290930910f3536c7c7ff34bd61268e50646cb6ed26b2a4b604c1005519ae8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask dfdb8c2a7c3604e6718bb0a38509901dcaa2b1949ea107ce058eb0d5da4bd47c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaShift 3c7b696486232d1ced5437814f8151046d1fc31a9b398dd36c18e148d70bee74 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16 dde3f009f8fb165a3365a16b5a9e48c56749365509fd287d2aee92dec70f2ab0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 4a005981b1c08067d4ca91e17cbf8bdde3688479e2d3d023739c7a2dd8eafbd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX4 7c7bfd994a0e8dd09e33bcae9c67098b0ab906aeec088acde3bdd7442ff5c5c4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8 304e2e67ad427f8a6665f93595c87b2cf6365f19118b55cf53bfa86d07b29d9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtShift 14ab0bd1cadbc663b4913188765873f57fa029278d7711f851162c365f7b0cc8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly 349a50e78bd173d850a4b0aed2b9da4fcecd44c59b5b1ca6dd6dda82b31fd027 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.Srgb 5bea9d5925480eefa65b4b45cc6717ca2ec5767086e1f5f0e7ae9536d845c03e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.A8 a0ef817c8026cf2c31176959d543faa90cada448ebddff9071c19b376b6bd917 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x10 20e8b592b09ef3f72d826c031dc94654e3c11afd9c407283ea97b07c2e62d63d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x5 2c2e26e9a4fec0e173b68812182837f3e0a2008c12061329241286670106d5b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x6 0bb20b2714ff8af63424b9e2179c6fbacd3f2ceeab6032e50f860a7d2b2ac687 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x8 61ac5f20b25a7f19f219b21fb1463b152df96dd14d7a77939dd3cd3d2cba903f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x10 c6c53e2ddb494c3e287d160fa56ca99b48bae80924923e583f1e220ec432aeb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x12 751d1ae15e24d8636f6084072c3f641dab2657be27e028c1805d2fc12e7435f7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC4x4 f54e35e13907099397aa7e5afc33d3c08f96d5fab21d25b0c58f3e7d68af7000 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x4 bdc3272b019f97b1ed04124e812ea8b719795f714e1de542a6506435465148a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x5 d62e48681d26c80710e8a479d9ca029d4f2805e19b4ac38cefefcf29d0b9cc25 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x5 6bc26c5182280ec429db798b49484eb652200bac439ae7f283d72b78bc056869 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x6 221fa7f8149870c1b1a71afbdf52dbdd2ee8f62c2e03db424c1fb91c3eb00471 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x5 55f3541294ccbbe9b51fb453fba0ac75033b0647b6172153ced789a216620cf9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x6 cd96c303e5137781dac3345d8054344491256e004e28ca36b99578bc79b7bdf1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x8 ceabe3adf09cad5db0c1dec7fd7d11213f1bba2d62c23fd0a96ca0eed5a9bac1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATC adeba935fa40dab5e66990263bb6f5b55759fc466a800cd0f6ae228ab268a4da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATCE aa2079c4ed6679d414d7f0e625b70b77a3148d4b4431469a769ffdd9ebd432ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATCI 4a5e17d58f848a896ffaf1c88618252c555cdf932132e2b5ab8330f505acceb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.B5g6r5 8647f42e5c844c14e743336f96ce2cdf3b9687a4b5580126a3104038f55ea311 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC1 8fe4def5320b12bce41ff21a83589683d9d43e161120f39de7918da5f050fbd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC2 66ce4f9f97df710eaaee3d052fa950d83ef6a804ab89f6b29d97457d4f054aa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC3 4258a4b819a47683f194f6e9e772be8dd5b56857a4ba1e667931c518cce665c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC4 da4748037f147659011f057f959e3e4acb6d7096bf885d13aadd8a4f2f10a2e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC5 1b751077b4b4b65c3ae4b3089c688b7f7b56d0392efb7db3c36c7cd917ab2614 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC6H af5ea98f9e349b3987089e635c69b650a5d90a3c97f3247cd09adb03e83e8922 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC7 844355fc08eb069678dfffe3385b55decf99425524c5d93f2235b44b3ef2f58d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGR5A1 3c7b9eea714dc3c8ca41d4d6558dc35879605e8bec0bcfef86336e5ef0a9afa5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGRA4 5c000822ece5d93944db183e8f77ae13688860b8eeda1f7ad0dec9aa1f9b4f73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGRA8 60ab9c0dd2b88d541d958e8a9819d83eeb3f154f993fa15075901679f890cce4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.Count deef704398cc1c036ae3447fc98c82b270038065e998cebab449686370c2233c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D0s8 0b2bd205c8f37902bfd665ec103c73a6860c131cbffad67ed05c09b1e00f738d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D16 ddd3f0678b9319afb23de0c4a35e8e6f161e5d9e91203bae28143c914ad8316e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D16f 043b30a85e9c5489f7cb1cd7f306e52dd5f6cf39e45dd6473626e7451dc3c22d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24 eff7318a794ca4dfeae0600de14946f019f24e8144f4f99f3c470c186c998791 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24f 910cd02bd892de57c5eae42565cd64ab30133956668a7f38c1dd4d9b24a5966e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24s8 b0c7af1e1c23958144c198b880dad082cc0938635c21bfcb5917202d59571560 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D32 24beae21e3d59fb0940df9b035966436ce475805dcecdff94b972bac728cae63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D32f 2703b7bbe59afd5adbc1da04adb59ae3a751fe47347149be766cd63a1607b99a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACR11 ddae45c11558eddfa41b1d3182d58eed063cc2b5aa5c2b21d493d5c677ecc9a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACR11S d90b14433a31b0f0204162e8daace0d683bf8b423e940106db60356953dae4bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11 efff49e97c1e28bfbb714cfd49d47f765c3fe1127a3a3ca6a3d1c3b0fc6ecfda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11S 30cf23433b94d5568439a2de2da0177468d42173a3683f473a7c3b61743b79df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC1 e37a2d15f9c2049b8ab5a43fbf66cec6d6e86a0323c4474d35263f4451de5f2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2 d04e58a4a4a6dd5d1d3e77ca92fab78f4d4b1528363e9b581337a27ad156dcf9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A 422b9c3732fe82362c45753b112ee2e6db4bfa3424471539337f14bfb92ca39d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A1 73d7e23953d9e735377831c96164f0e50f4e35c19723e29ccba0c55fa34dac5e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC12 7620079cb68763fb1e319159bcf20aa12db7f7cde38f0910a2841e50b31ffadb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC12A 02ed8dc90952adad05bb627c2f4d1737adfae8ab57b571a968c0a7fb1b5d93ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC14 cb36cbcf0b728f606f8e9238fd7b249697f42377f80263b69872a51c038ed0f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC14A 4352274da2c7de27ce9f5d3e3d9d41d66f18bb51e59a2319cb5113c758a228c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC22 a886ca86449a122e8a70c9aaf589d70dfe05cc9a8eca37273ef5ab5807d3179c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC24 8006be7c0dccfd240a9592ab6a01ce7972fe26a620e3e331d2e93c6b8cd3fafa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R1 6672201a2ab8199f93914ee6c505eb43816b21a0890ca87ce8bcaa03bdbfa535 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16 6754d15523cb8f3d1d499f2c2435bbdc18c1f59fe73f06cbe823b4f5a40252e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16f ee00152061a4528ded3fb6bbdc5ef3bed6572e7ce06d1af5d2658893556808b7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16i 12134ccc7848d4d5c612588e04d89b2f8df475fc2269bc734fcfab8b1816c73e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16s 96bf04ae5b1c122b09e0aaea893b5ee945dec3eb155090a1ccdafab85f0868e4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16u 45798200f6eab88a4ce325e48218888839ca5db4532a57ac151c09971ab7a6a4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32f e2a2e8c4043eef6eb703ea3cdadfa459837bd4499971122f444e0416d447a7f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32i d7d3a4a2522194ff5f222b0a3ef94d89f2b4e874b77cd2b9f78818710e6fe85c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32u 901f985ab60d51f8abe7ca31cb8a974566ac64b97eac897daef39caf92aa8fb9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R5g6b5 cac7e62fdc60daed4b806309501ab402e17e330eb47d056201d91098907d36f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8 caf8f31ca66b52194790add1f0b255f5118956a5af8fe2a9e4bdb68234208c82 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8i b0dd27da5554f0df8df8c6e2c48a00d3f1ab41a35ca72389fd4d61a286f4731d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8s fcb62284c054f65b81a2c59b81db478f9e4d4fc0e3c20f9dc37731a744784d58 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8u a189e96041c4c2fc3aa2ffefb70fab5392c618c07bd11d137612c6aa0994e689 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG11B10F 111c8de19a322a3d8315478a43d61720c6bae039e0d46fe94182b0f224e0b2c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16 9145c363699af6b64e1f703bcef067b8c3a565e611e84fc27314f56450fb25c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16F 4bcfa38c36c649b47bfdf0641e4b0ae366df8b96a25b296931628f1f371bcdad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16I 5519f3ed7bc256f90d8308826a3738ef4926f93cb0934a01fa486c5e60b3c836 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16S 9744a5dbdc363b5648c18232fe66fe321c54eff08ed2362ae11a7f31afcc0e51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16U 176d70076a54f73745b807929d755de3e5b44497dbd66a955d4d3e50729c08f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32F 954ba6d6a9a1c0bc614728f7661f151bfcb558d323cdf3e8007a6081ea9d4fbd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32I a97da419b8bd9cc74f9291b65414dd56bd824274520572da9c045eefe80dc2e2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32U 001b300902fba4726498ea36c82064885a5b4818848e174b358c2a93d4117aec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8 a358ac22038e3bc3cf387bd76ef00fa50a2a4c9a95a30ebd6d2c716e183e8484 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8I 219bcf5178b9114ac8151c60720a0961518612092fb5d0c5677b3002a37e2c10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8S 294498c477441a05c01081f303e14f5005270e0b71d276f2c9cc27a4c7d9d7d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8U 633775ea5d443c233a3948f003ad0898afa430595aa15f973a43c79c14c7eb4d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB10A2 9d6e29047a8bae09920438c0dc50840b18096f02634a659db6e364e0965f1c9e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB5A1 9861c443b16c49efd0f5356525439ea7f1448a223353802db63dd231fc81d503 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8 3c1e4f3999ac69325181b92d0fceb0cc1a891f1921aa19d09954344849586f90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8I 6f02d6b62b470a71177ac3d0e9e3d328cb65856d4bdaec81b4e8de46feb10643 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8S 77a4d1876529b2077dbbe3b0e864ab901d5ac1025997ca0cd571befc02f32f0a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8U 9c30be68958b219332518d714ff104c1807680e2d23eac3ef7481f5b6f26d846 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB9E5F 3fcb6270061bdf82d0969c8677e93889c25d968cab5cfaf04acfbed21641a955 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16 f6d1ce70c9fc9897c5b048d52a956cb51cfa49186c822a62c5aa39ba952d277c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16F 2d705086b57b0a3e5e425c042188849dce9be69b1a9234a7d2240d79d64446a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16I 96c485e3253542e63369b2f53b9b9d42e54fe006f1ca7a7a8495f4e8ac1ec478 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16S c9b0e07112e4d23dc012839075692e45f7a9c17fe92002b558dda1de4b323c9d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16U a810ff3baa3215625cd2790e5d67109a4ac3f1dfd0b424335414d7d3c1265e05 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32F 951acbe8ca8feaf879d3331a9bc524af3c806ce434324831e66e980ccb389ceb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32I 1d3d13e8e1151bd475dd502337b8407088c2d49ece00f0d4d829d4e0a9967b48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32U 3873c48382495c96b76bf8fa165bb08941966ee66d17dd8749ec1c5676795246 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA4 21a86132d281ca3cae3e85c74ee97bcbbac67742dc2cfe6fe47bb3cc7838a378 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8 a1a9ff7204586623956ffc6d5ef7ada9216822e5dc8e548050b2d6e4c13bd156 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8I 230110346b2c7d0aefb4d7b3b81bc5138c86ad92c1af734254dd1c75ca967db1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8S 8d617d5b5e3d2f15e654ebe2b3af03dbda792c94bd4a26e67ae4652fcb8e9ebd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8U 7c5b4a92ea7fd6a4bbc7c1a91066cc29d391ddbe642822ba4c9762a53e94c67a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.Unknown c5e9f57a695070d821262224d9f485379aa43698e4b07f3352f97c3384ec15ad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.UnknownDepth a09eba3f0e66c8bc1141dd98452a536c7b0ccd150e586f2f52a85bd01758560e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureHandle.idx 937a8eea10c68d76e882526160582e2fb784b8f4fb62e88f43f20a59dd355bd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.bitsPerPixel 3f6f6fa76254f1804d28efe9fb4078524c19543ac0af85a205f8ddb831008fa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.cubeMap 23d02f03f2e1a458d77f7884ee45321a735db3349aaf9336e73b769a212e7e97 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.depth da619003d4720c6311eb7fd0fae7f0f70fb3ef2ce75dea6047e8b7fa5fb33a94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.format 5e7f28c28b9b5112f6d1d2580ca0271d85d68e8d02c8568bd73a5d8e406b3dfd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.height 3ba96b85f8722fa122b18f7eadf23fbde0804abbe930be628dace39d07fe9df5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.numLayers 548012a6125fd3080453e16a8ccad5fbd4556ca902276ba263e3a353ed46f744 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.numMips 79cf391346bcedac70a62b36fb8ce2bfcbb8214ffac3c3a9205db6986201e443 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.storageSize 1c06cee4dd0eac2517b815d49a21fa5a6dc904e7c84201800af0c49c0c6fe4ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.width 30f68db008b42ebe526d39295291fbfef4b16e2b9245d47026d3bd007b660556 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.Count 7dc9ba43d05955519f765604b377f084fc074bc2df453ee4abcf02f535ceec65 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.LineList 8cf597971d0126b545a494080d27d302c680003b348d44e2f6f69ae9f4847884 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.LineStrip 8cd0e2708663f728863216369f691e76f549cd14175e0bef6e7a37138b9e138e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.PointList 679b633f9aa1aa286165bed4331e892987ca5540d1359750e371b055a02c8a15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.TriList 4e2735f6d6dfade9a5dfc66ad237258ff3ae118e4f827b7b59d380c260cec6b0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.TriStrip 8dd869ce426943543c0a5351ec874c7e1bddeeedc282e54a8aff4a114ec8fa2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.Count 9b718b881f0894339663d62f133b1ccf13b06dd303f266a3e5be149ee33b2d35 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.LineStripToLineList 5d889e600568be448aaf19f0baa7938663c3316bdf5f51b9eb4b802e449456c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriListFlipWinding 407af5fca2d68a4794211efeba339583c97f4bf1eefa1d3fdf884987143f3532 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriListToLineList f4b6619f31a0b0994bf5d8e666a690298190e19725ce286361ced74a6e138743 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripFlipWinding dd73b2c49ea3cb46e387df9c89f7de7560e644a514d0560d79d11f41ab039ca1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripToTriList f87790d49bebcc18c029a1a8464116741a738d408fb4457113b6b956e9d24f48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.Count bc15ad655c5bb25793c8e0182d3bcbe25f114660daa71de243163698aebe6a19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontAvg a07572c7be4b58061558c6d295539fe57759e5b3ff48b7bd0934fae3547cd03c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMax 3ade8b18fb6bd03ee8ddf05a8c0044a2c8f7c700c203fde95cc79d8bf5d14e6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMin 21d968613b62e356209756f755b6d6d1883252a6d764cd09a0bbca8ad5b495ef -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackAvg 9dbbf36154e5dede514cf178b52b764fd089089bf89467029fb14ebb4798c709 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMax 7ea35327f76933f89c73946d35cf44441c4beb5945942a1d2618c6b41188c7fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMin fa755080e850123314f2e5ae7751fb472e250d57c1f2d75bcb6eb67eb472b5eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontAvg 4572d942a9bf3cdd6527bda329e1399451a0f7446d424d4b77224c7d192637a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMax 92a2f22b4e9644c704de570821d89c8b91ed2be746bc89c9962314102fff594b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMin 2058af260d82c2d626cbf56588b947c07af3de916a0c575b912e8a5a65320fde -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackAvg 0ddaff404b7aaf8dd767b4f50f1d650f4be572134e5d30c95f0dee886b2a5bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMax 6398682b7e64c994552456f9f93cd9f4b87f70ef848ff0876b40de44b390cd75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMin dfd91e4acbabe6e2fc5b87bbec48447862a52ef8e0803da5315c5b6a9a8e561c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Transform.data c06a6226a27c4cf76802b790c233cf62c99d3ae8ec11e2b4c55eab1906c59dc4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Transform.num 18bec64d93169f71a8643ab76a6e716ca005ffbd852e7dbe9f964b7ff37427e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.data b5757e0b52182b3207867dca9498713d8df6df4d54458f4e5dbdaf65fe8b8fc1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.handle 705989c2ad12ea5f266b39e10c471c57abb7e9df461db5d8d10c71712adb5347 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.isIndex16 07549f62cdb4c7c3d837381d9ac4ed7d7a9f0fb45b29f748e7151fce1d07b309 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.size 83b022907135455d5c06fc01f428ca39a3d9d266aea5dc0075c75caea8ec9b9d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.startIndex b8d7a0c813782e8f0da1f9fdf3b3b8cd5a74108740548dc9b9629b90ad322efa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.data 8c9705f06756025e4bbdbc41fa6a9286b2e73c2af23b9546827292ffc59c689b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.handle 17c8c26c7f3a658ba7ef23d3286c4ecf89cb208493e1cef92a125ac6a350eac1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.layoutHandle 2cb8d23c46aaabdf8733d718c89faa58880f7ecfacbb04899034eeae279c0647 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.size 7dcb8245b540e2e0682beb772d9e330652e540317c0ca3dd329ae2eabf472791 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.startVertex 82331946ba9cf6db26ea1b23350fc2790c35d7234ac758458fe44eac3a64fb5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.stride 5788db19cdd0c53a1a10f4aca847aab0b0fd4aab48441146962f8cd9437078fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Count 03b9bfccafe686e2f2fb9ccd2d0e55c9b7d8286f6d35439ea88172e7a33b038c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Draw 6a7bbf0faf3dec1612b5db77ddc56f28885aac3c187b0c580f9072dd087a34a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Frame d32b29cc1eb45acd8b8b8ee042d0797ba92e96285f17e1464ca49a9d8a050635 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.View bbc4ec5ac3a55dc2a6917dd1793d16e8d115224a904cf60af8dd2337e9d3c365 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformHandle.idx 13bfbfa32beb43dbf3477dbcaeca3d1ecd2f7368df883a01da9a645799f511ea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name fd08ce7f02efbe697c75d6edef9ef54e839b65bf637d409e88dd973f8779f335 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.num faeb19aec0a1f642c3c62a778f86744a2363c029aa410416f30fc082a0573d13 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.type a84538e8bac70980f4720836b0abbe55ddab9394f38bc24ad5d270536d8b63da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Count f3afc1ced5e0ecfd74248d65b20878934edb1acdb1a1c663660fe85b8b3d35c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.End 7fdddc2f063427c1210b9a2b50ba69747bc892ad939f72f2353c4fc200829cd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Mat3 a0d0c5bf74e7c0dd6f025986f460576bf74d7fd3f8f37ded984b550b9be042e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Mat4 3d740d34720daf448d1d8d73bca7cccd4dc7d71e28205b6b6d75b799ba21195d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Sampler 2ba9a7abced5b6c21c6d49f4367f02d6404316959cb94910ae798732bf8054b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Vec4 c57e95ae6e312249865eda8a6cf85d6d13f86ee339f1b18665949326e81b105e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexBufferHandle.idx 37bd56093dfd2f25596b2a108365fdce4cfe2c862db2c34cd45d4dc37f60e5eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes 8c187b26057c29e17f52f1712cd8212cff130c474a91c06ebab4a8f3b13c8df3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.hash 018e90fd8889a7d3ab0eec921d262a9d69415ae5273f8ad8414a6b1cc94a3d25 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset 36fabc823415c8b0a0e91a42ef100bfaacab5a134a0a3aa1e777bcdb1f25bf61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.stride 552a65b00ebca9d0e3f048c5d0aadc8d410af75ba6fae97a6569dfd5692ef788 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayoutHandle.idx d83a913d08177c910577673303945de1948068c77fe5f9f3e77f4747b06bc748 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Count 826573109cf2c7ed45f5908a7338f22a2ff874bb7e5e1f22945cc0e0668c6b1a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Default e3b372a50ce840e1cf0d20abd44884c51954898dd51302cee393218e14753f3f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.DepthAscending 2ff3c1e7eb758216eb927edf1d08b6f8c98dda790b8bc9dd84fc7963c0034268 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.DepthDescending d9b4f29254a27c5ed18e055bd2e3fb7f31b02f46ee856277fe705749808417d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Sequential 7718a7b104aeaec0ee47b8abe4fda7fe4b6349b7590cf0c1f62ec13e20cee936 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeBegin 8f5f2be366e431b2b4eb3907123708ac87468a3cf2a61c5b350d26eb22376a02 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeEnd 7a50169b8e1992a8bb30b991ebffea3a9b4fba7028e82f9eb247429c0f999085 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuFrameNum 28c38d1c3d428cd0dd263116464926784a1a07b979b5b2914c6debce5bf6efff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeBegin 2dcb52cc484c0f20ffa2ceb2c38f4c68d0d6434708bc394dfc05b282379020dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeEnd 64fa70cfccb0e171843c5a2afe3ea3746abdf9d65680096db1d2a1144d5271cc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name 0d4c4e710226588020c6acdb243a9a26ec4f999ab05b1a55f39bf8a8ba455cee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.view cbbab5895bbc98a7d1b79d1ad5c1083e08e88b12ef231e29efa9e976c158b0b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERALLOWRESIZE 43871f2ae92e1511f88cdbe6cc9a913d665b7d8fefa77d3e96e085c66f01569e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X1 868f6675ae887786bb178178bef04c00f353d4213855e17d0289dbd2533e78d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X2 a90ff50eb0861ac4fd4a7ba77e58fb8b4eaa8d758ef47f7eff289bbd724fbff4 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X4 acc70ddb6c6127292f95fdb401d2db43a48c39be4e48f40cd1903465c23fa28d -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X1 840cf73de52a551424ad5863c664f4589a99a6f1e4dd6349519508ab8071b2df -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X2 658c2cddc7df6fee4b4a5aa5a66cfa2ddfee283890f7e63bc44b6d8d4cae283c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X4 26812bd7d6ec3bd97286b1387187e2f1b402420fb14a7b2610bd71dcfacc7783 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X1 960ea81e64f8aed36577cf9d4e23cf7f4fe6cd31dbabb38d156dbe0f5f50051c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X2 39a2d8713f95275e56bcdbbc9a8972564f9cf6a1da37e483a1271a471e8cf043 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X4 54247f0d86c05e8176a0b04039aa6e10d81df61c301f2544e50f72f2e01154a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMATMASK b852539b1891071ba745b1f9daed8a421d5f3830cf02201c535665a6fef7ba5e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMATSHIFT 18191b107f911c0af26e571360c841e4ecc0472e3f28d1525d3a7f42edc0472d -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEREAD 1941f5bb352be7fbf54d7c885b01dc8b4670d27a9df9daeb96e69070711cea94 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEREADWRITE e87cfa4ff7e1b664d196370afb20d0b26a36259e20fa3e8f5332007c4f770a8e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEFLOAT a20878b3965f5db6bbd47a48c67940ac86ae5ea2b09466415e21d7ae8d323979 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEINT 71db467c22e32fe564613efb455197b4e260b93b281d2f52f00911f550dbbb2a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEMASK 4b350d0f5cefb5c607f40293d8f9f0d4ce6f06d64603dbbffdacf9df6dcce260 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPESHIFT 7aa6d14ae524899753f8a9f5ae7607a62ada638e9f46054d454e10cdb4f9cff6 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEUINT 833a8d46c1902a648ab05e8ca233dae9532d0bb9946b7829d2a464dbe0da1ed1 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEWRITE 9672d89d74356263200edda0be7ba14142e0f9b045ed64e74cadbf6ff7b12117 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERDRAWINDIRECT 55de800cf4451ea9fb61aeb92a76e08242e854102d6128e8eea5da5ac933274c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERINDEX32 aeba825b33eb38c9098e17ef8249b5bd84e7762c9afd041dd2f1cddac5ac2bfb -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERNONE 7c2209540817a35cf79aa344942b80de98611655a7380d63e5272993064d907f -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARCOLOR 2232f5481ab3789609f2e52309cdf06faa001f62345084b441c8a9b3080986ad -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDEPTH 42f31b634fa60f4fd6907d9d7f365bceef97b863d9be50690cdf83105ab875fb -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR0 d4dd7df4565ed135e13d6f5178fb3bc6fe66999cc0ddd13b2d48b01fa1e0f1f7 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR1 7b98254dd51b393ddf7a7d90386298a2cc9d2a3c1fb5e496fa98a54adcfab03b -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR2 4dc17309ade932736b3e2fd07f53f6ed9599a78a0f3c2367244822589ec0eff4 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR3 df53576ed1c0c983ef557e129e2560dcfe2f3633dc60b66f5147a362c3d649a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR4 395d40f851025a9a807636f51aca0b4a2af01f81f7e91504afa0fcc212e5d907 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR5 5010adf2550de518c8828baceee3a93b6b641be91f929bcebde6eeace9f27c2a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR6 94fef367e25d8614c64cab7789b50839a61920e49d93b3f52414b9a5e147c7cd -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR7 397d4980fb5ce692d0df3b06b6ef148f3c77e8fd8b8fac5b3773147db5e17630 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLORMASK 9eeed85a5bce20b78facba1b58c57a35f79661b7e8afbd1069ab03f6cd8f48b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDDEPTH 9b5ce64f2a8beeb3cb3a7104ba7faee64696b1d03d751812985e01392bc26be6 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDMASK facf47709220600269db6562c08acb465ae97b6171eeaf3d38bc127f21ae319c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDSTENCIL 65110a0748e28416bae3730c41eaf325b533e5e6815c5fecb13f9991b75e2e18 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARNONE f3777a6c4a3366161e6b32664dac66a18a8ec7e6dad3898cc88e08cd23554615 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARSTENCIL 1b1fddea0d780db3504fa31f4fa41c5864cfb49774d44f481e03eac14aa7d90e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEX a6154e92258282760910255ed2e8c45b34fd0d5963658f5285485fef1954f20e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEY e3c12699be46065791db65b4072607f49cb194d179f13b38a2485cfe970e71c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEZ 1e46d7e092d489b5abcd488998139c9e84a4614c7da9174250029d0e0ea8e68c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEX 14f74aa8e62d562e2b0b33030bbb973529e67e77839b9cae6a3f3526de13dd05 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEY 3eb52406d19cc5b4e082f7bf93b90c8a6f8639c4e7e4316baa57fb264cf44d86 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEZ 6f7e4ddeb607defe9b6ad692a767a8e9cd3a0b9c1a87d343cab2f5b3ee044825 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDALL 36ed7ffa6caf78660f54ade7015ceaafd00bac64476a0f517c9f9c5ef735ab27 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDBINDINGS 799e43f66458b1c0c5e6f81f3f4c253d94d4d900ebbf2dbd86f6c00224498ba9 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDINDEXBUFFER 3234a6ad819ea39cf9d1acc0c38befabe4c19b04933b31a91ac84b7fa19710cd -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDINSTANCEDATA 2d7c02b1b855893a1621957e470abb26a04e64484bdc1def616b0467be6af383 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDNONE 37722593a171449a1dce51a813db0c824b880d02073220795fd0d80b17bcf08a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDSTATE 5ee154abc08574da78c82dfb71c476ea0860c4dda601a0adbd8032b71910bea8 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDTRANSFORM 78194553e8e7ed1aa36f3d3a7fdb9cf55fa6b3fccaee70a072e2aaa52059487b -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDVERTEXSTREAMS f93f543f54bc07b037975eda11fab1c6b56d59f060b5bbcd486ffd477b18f0cd -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEDEBUGCAPTURE 5244fcd0e7136a30d1137981fd65ccb93c28091abc522102e4e8c565b9ea3847 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEDISCARD e905e07ec06bd638a59c855b0e1a9746a533214a7ce8766cbd8bd298a98fb446 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEFLUSH 8058e11d3b46d39b0971072765b37b942f58a402cdcd0003f32e2fe1604b6a55 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMENONE 41a2a418dc79b34e913a5743747df3342adcd616405c060be296e5fcb20cb4d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDAMD d62960f1c9564750f947e8b8714244085a080806f1b7479ea2627eac742e7dc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDAPPLE 62f0842682c218cb8993387178942961608f84fcb042abd930b8f076603478fe -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDARM 8d188aae2cdc992a6e01b740766d35de9187ece353380dd024c3524feb1c7bf0 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDINTEL cdcf955f5efd5321ee1cfb58000db5de7c9189ef6974c418e48183d1d5569680 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDMICROSOFT 1aac3b4eaa937a4bd70c6fb4c8140816edba014b7f7d29a9457d4a976fe084b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDNONE 2394bbbf9e0b49813b242a119e89482ca544b9722ecc98bccfcc9a3419741c13 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDNVIDIA 8f69da65e4c929a9b8802654c3b48a6399a577ed35f696027644f3c6af27ab0f -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDSOFTWARERASTERIZER a89bf6dc260c00d3fb1df749f3cc8ba8fad8f9d3af6d663042b2ebd7fce4164b -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETFULLSCREENSHIFT 60aa480426afe88e02f7e0b1912c86e53ff46ad3250040ef17b30c12b7bd9fe4 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETMSAASHIFT 26808fdc6e44f065cf460e8587af62ac83614cf9bb7ed2e1900e38a3d91a24a2 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETRESERVEDSHIFT 312a3dbebeb88b516684ed06d0b750f92e96d53b8b125737be889a4d51d1d9d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESOLVEAUTOGENMIPS 1c9bafd983c52d29db134978ffdf9f21c958c417237d4f6e046f93f5564fd620 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESOLVENONE dc4618d357af71b746ec3fccfcc6d2ef23ce279ed2906d76a1592f7b5a60f73e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERBORDERCOLORSHIFT 95d0e58a7b3fbc370a3b2e7c2f9e11b70b36a6b2a611f833a59a88e365d8d6ea -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERCOMPARESHIFT 033a8d5bd4ed3152aa20154120f985cbfaa67cf99345dec0f76b8f62fcca366c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMAGSHIFT d7e6c5e0dd517652d4ba3c71238263081fe811517308de4aad276bcede6302d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMINSHIFT e9d83a51e7dbef89d6c5129d6ca230ddafffaec113ee26ffe7129dcfd0503402 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMIPSHIFT d9f45969aeba4ffd0a8d69640d8de4e2156fc8e25198f17e3ec09a7d3465eea0 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERRESERVEDSHIFT 100abd9e4d3ea64156767051ac95cd73307019bea45d1c990ea5482647ec3ef7 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERUSHIFT b7122b45fd536b00c25ca47c3013c02915f4041d345be91476c9e06566c8bed3 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERVSHIFT edc73accdf0c46c1bb7b6a8a27bf011525c9decd51a66a12578b70a01f57675c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERWSHIFT 6b3025f64495e6378fe81fba51d23a6f9316902eba20a466bdf6650e11047756 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSHAREDLIBBUILD e8aa38bb0fd45b95b27b8dc3b387abeb6752fbf2bc0214e34c45ae35e78e1c3f -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSHAREDLIBUSE 072b1baec6d9b73806d1c8f07c39944c97c05bba111cc8b57593f1707060741f -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEALPHAREFSHIFT fe3294c558297cfed89835c59b55b290c02cc10a4171e61e51cc979011ed8e86 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEBLENDEQUATIONSHIFT a1f21c1f5bab3a910d26fbe70f2f05d926837d8c0701a4245fb7001ead2fcfd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEBLENDSHIFT 71d309cea873b3f75fa58eb288a8639883dab395566d3dc3e108d006977ef372 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATECULLSHIFT 3574a08115bdef4f8a3bd88c80776735db687f60850222142fa0ac27ba97ce7a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEDEPTHTESTSHIFT 9427045126fd28dddecfbe5a2c030a5a0ba2a63f07987b606b0c59be109b2626 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEPOINTSIZESHIFT 6db19ecc315458cfb37c004d739cddea5391253a07662958e7b4272fdeba5d7a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEPTSHIFT e6488c97ce70168c55dca6744c67935755a48310f284a3d817229487acda61a4 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATERESERVEDSHIFT 90aebc959f9725d43b5a0a744edac58e20458877fae272b61ea5ffcde0588a3e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILFUNCREFSHIFT 14aca6e152327877c696724cc9b87fb78a2b2e757a0d5f0957818f1c62947661 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILFUNCRMASKSHIFT 57cdaa083fd2b574915f9ec44ed1e02268fa0b9c1893745bb77662ec588e42bb -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPFAILSSHIFT ac6578b5289dad5e27d35bea5d234c8998b5d677379aca26f949a678c7461b3c -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPFAILZSHIFT 440f14fdfad64942b4266636381646cb1e052a0995392aa6114044567e3cb16d -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPPASSZSHIFT 1a5f139367ef88fd4394662a1e46e884db734562f777963d9cb5238e23ace7a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILTESTSHIFT 86aeca7e1913684b380dabea895389111b8cd379be822a55d0ef28bd5ff3991b -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXTEXTURERTMSAASHIFT ccce701154883414bda14ded04cdf7c8f16999ead63ba6ce339f6d61c4b6709e -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.Constants.BGFXTEXTURERTSHIFT e56afbcf9ab28ba345a3a486f423d80afa7f3f9a21ecece5bf22ee7124e1300a -->
<!-- api: F:Inno.Native.Bgfx.bgfxContext.NativeLibraryName b7a7683cfcc656caa7bff61fdcc483b8ec67333d44279d147dc425f25ba2b1f2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Dispose 43b90af593ccf894dd9a748e2d87fb40ae51a7c15400adb0287ad1054786b8f3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DisposeAsync 7c901429390f01e5aacdb295ce4286cca3caa470604d37d856baf7501715c299 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.#ctor(System.UInt16) 3c56eda81af62dfbdd977eb2167ae063232263be0895a7921cd4922180292611 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.#ctor(System.UInt16) c8271c255289d68d9ef0e765eb1f7bb5eadfe08b9d2bf6fe2f2dafd17e0841fc -->
<!-- api: M:Inno.Native.Bgfx.bgfx.FrameBufferHandle.#ctor(System.UInt16) a45e72a3fc5773a39578b03234a597114f206ac1683e2d05aa808aca7bcde78c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.FromProvider(BGCS.Runtime.INativeModuleProvider) 3c5735e7064d423c37e424b062ad1c38a1304e990f710cf0c4c2e18557374622 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndexBufferHandle.#ctor(System.UInt16) 023f850d14d0fa4b2e4dd0ebe2f5df82e969f8ebcd90397f2ed746f608a51ea3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.#ctor(System.UInt16) 0184d1d6cd25b069a960f2750b59584934ba5105e16a4d97b006fdba0e111d3b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Load(System.String) 674892f8e4c344224d2a016f778c906f0da97f4f52c3b1901e500addd1c0675d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.#ctor(System.UInt16) 7f6b650fee4d565de979b5a712f401206ce88d1c0aa1785cc4b1f331c3d08b6d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ProgramHandle.#ctor(System.UInt16) 919d9c64f6653ed99e9cb26268ebe49511ad1e3eb06be4ab9cb8577247db9d97 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ShaderHandle.#ctor(System.UInt16) 89cfc889ff2157b9376b080bfe97e1ce4f23916820069d62c705575f1ca8a3ca -->
<!-- api: M:Inno.Native.Bgfx.bgfx.TextureHandle.#ctor(System.UInt16) 0b1faf79c6855f6c714f7738d0bd4e306efd270a0888364ac79ce7e2dfe8f014 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.UniformHandle.#ctor(System.UInt16) fabaf9ca6eff3b5bf67621123e6185dfd8d9177d3b1511f8ec47a124015b9d28 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.VertexBufferHandle.#ctor(System.UInt16) c13174309fb0d31021cca88872ce9e9c5a0496299b192d03edac540491e5c551 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.VertexLayoutHandle.#ctor(System.UInt16) e4b41effae826c6a32ce2a9cf5dbfcd80ba129acb5952afc4b627adb7c714518 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.blit(System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) a4d39ec63e559f6fa7e7dfa82de604058f19a85794b2e73518951864180e748e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle,System.Boolean) 3296e4a4396f9844c5d7995c96a40a829039fb98392d82927c9e7582e6f77395 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer(System.UInt32,System.UInt16) 47270b8f94b174b3fe5adec7e8edb6ce3574a84b74b165fd5bbd9f344b013950 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-56aef5508110e2bc"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvFactor = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendFactor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6390) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-234b1b04931f45d4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha = Inno.Native.Bgfx.bgfx.StateFlags.BlendOne &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6383) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c61a5298819224b7"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor = 16384`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6381) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b684427bf8e7f76"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendMask = 268431360`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6392) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bcc8d24afc3fc7b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendOne = 8192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6379) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8b00db65fb0fdf6"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6391) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56c3baa2b6d7e8b0"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlpha = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6382) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77cf3ddbfc6b7d6b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlphaSat = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstColor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6388) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cec26b3c0f344950"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcColor = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendOne`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6380) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-204792f493e2eaf3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6378) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df5d3d434a493523"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ConservativeRaster = 288230376151711744`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6416) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c24f1d378a5a62a3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullCcw = 137438953472`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6401) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c50985eae6077a7f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullCw = 68719476736`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6400) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-778e027c8d25b8fb"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullMask = Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCcw`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6403) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4105a10e1aca43d3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6402) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c3be8b39041cd86"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Default = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteMask &#124; Inno.Native.Bgfx.bgfx.StateFlags.Msaa`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6421) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-17ba048027a41408"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6375) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5b676318437fe06"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6370) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5649a4c4c8bfb1ba"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6371) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5d945fd06a9cd4b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGreater = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6372) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-802e4ebfd85f3ee9"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9ad6bf9fede4599d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6368) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9fa75edcb7b899ef"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestMask = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6377) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fdf2bca17c7bd753"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6374) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9851b42f8e395328"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c8de688f1e89340f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6376) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd55a03141ebd1b1"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.FrontCcw = 549755813888`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6418) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22b2eee58b334496"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Lineaa = 144115188075855872`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6415) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5748239768e7575"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Mask = 18446744073709551615`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6422) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-65fefcab49295ddf"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Msaa = 72057594037927936`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6414) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dee3bc3c4e708c19"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6417) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1cf5a76f0650a51d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PointSizeMask = 67553994410557440`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d07fce8da8a62579"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6412) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-62212de36a8dee7b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtLines = 562949953421312`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6407) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df22e9a9a82abfcd"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip = Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtLines`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6408) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d28bfd8d10302c3f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtMask = Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtPoints`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6411) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dded7568e697c7d6"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtPoints = 1125899906842624`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6409) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-07728a0b8bd92ce4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6410) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-861bd87f009fcd42"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip = 281474976710656`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6406) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d2ffaab99116fb8"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ReservedMask = 16140901064495857664`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6424) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d9cf56c6eff1d46e"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ReservedShift = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA &#124; Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6423) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fafc0b62e00d93c5"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteA = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6364) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8f5195dac13685fd"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteB = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6363) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1dd82952afbbe45f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteG = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6362) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-48a7d3b0dd2e48df"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteMask = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteZ`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c876eb49303b4a4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteR = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6361) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d18361b966edb4d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteRgb = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6366) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-074cfed13df902d2"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteZ = 274877906944`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6365) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5fcdd7d2c11538a8"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe353d14755f3ce1"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e7ba1e78542ccefe"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeFrame`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6441) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8a5fec1f6e0e93c7"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimerFreq`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6447) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-91fb4c7d625a4113"></a>[`public Inno.Native.Bgfx.bgfx.EncoderStats* Inno.Native.Bgfx.bgfx.Stats.encoderStats`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6519) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7614848d7bcb2b2c"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.gpuFrameNum`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6467) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b57b25fd5696e495"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuMemoryMax`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6501) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89aba356656c240c"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuMemoryUsed`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6503) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8779b033246b4c7d"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6449) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a4faf3e02ddf95c"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6451) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-45f90891c8ddff5b"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimerFreq`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6453) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fd41e623a514574d"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.height`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6507) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc6172bb484a5604"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.maxGpuLatency`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6465) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7363a5dd289fef12"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numBlit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6463) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fb69ed3f123a3867"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numCompute`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6461) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e14c7a287da9c7da"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numDraw`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6459) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-409464e0a4b6cf43"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numDynamicIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6469) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-116fc8144be5e9aa"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numDynamicVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6471) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-40300efee382e725"></a>[`public byte Inno.Native.Bgfx.bgfx.Stats.numEncoders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6517) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c46ac6e847d2b8f6"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numFrameBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6473) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9f4b2387d558ea89"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6475) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ec07f8bea273da9"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numOcclusionQueries`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6477) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bd9182079fdb5f79"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_29165abb2c909d90 Inno.Native.Bgfx.bgfx.Stats.numPrims`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6499) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a99733bbe2f13e5b"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numPrograms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6479) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50f4d7e6209f3705"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numShaders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6481) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-750d38e84ed6458d"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numTextures`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6483) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2d61bad017670508"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numUniforms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6485) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a599e8b762610dd"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6487) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c740d7409062c9e3"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numVertexLayouts`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6489) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7bd2b9f586951085"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numViews`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6513) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb0d01acb4407d9e"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.rtMemoryUsed`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6493) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-645a240cbd836fc8"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.textHeight`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6511) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8d54214b9916f70"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.textWidth`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6509) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c8c1f3f510ec591"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.textureMemoryUsed`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6491) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2a690896d439919"></a>[`public int Inno.Native.Bgfx.bgfx.Stats.transientIbUsed`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6497) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3116278f02b80cdf"></a>[`public int Inno.Native.Bgfx.bgfx.Stats.transientVbUsed`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6495) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fda1fade0373904"></a>[`public Inno.Native.Bgfx.bgfx.ViewStats* Inno.Native.Bgfx.bgfx.Stats.viewStats`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6515) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2ef17c97eabe8af"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.waitRender`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6455) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b109d8e0c2dc2e1a"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.waitSubmit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6457) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-53dca2c06ef86ee3"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.width`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6505) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8440918e4c65733"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.Default = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6547) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13642a6ef512e4ee"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask = 255`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6542) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b28230ebbb893ccc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefShift = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6541) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dd5ca704bea02827"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask = 65280`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6544) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2588a91b6451d9f9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6543) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c91d707f0dd0177e"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.Mask = Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6546) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74dc1f4a807841fb"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6545) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aaa98bacf6e6f0a2"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6563) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-468ccd7b4992175a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6564) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-81e0930aa900d2d1"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6561) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cdf26e804343f77b"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat = 4194304`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6562) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d6755cd45096c38"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6565) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-27dffd9f1c0e1e14"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep = 1048576`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6559) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c01b033ab18e154c"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask = 15728640`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6567) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5f79e0dfa70b538d"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace = 2097152`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6560) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-48c73c18e7df1a68"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSShift = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6566) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1a02bdad5b90eaba"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSZero = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6558) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-69d7b4503196909a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6573) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-533cc1e26a7994dc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6574) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-725d9c6365cb2a21"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6571) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e654d82f451aff12"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat = 67108864`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6572) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce75d3f635016e89"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6575) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3a9d22aa0740637"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep = 16777216`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6569) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7aec9acc9aaddea4"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask = 251658240`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6577) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1eb9e66fab4215a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace = 33554432`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6570) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-92dd83c4a81e256d"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZShift = Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestShift`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6576) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e591ae5c0e343b0a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZZero = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6568) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12ef45a26972a719"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6583) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-863efe1366ebb957"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93dd7b34d78f6f90"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-396bb4fdfb9002db"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat = 1073741824`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8c6c71861dc13ce9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dff5777e41d12475"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep = 268435456`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6579) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-57e9ae87de2ed853"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask = 4026531840`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6587) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9375700744fc8fcf"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace = 536870912`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bd91f583986e119a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZShift = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ebc28e94861361f8"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZZero = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6578) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77b1d5c40c1736ce"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways = 524288`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6555) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf0a8e9670dcbcf5"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestEqual = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-979d7c90bedf27fc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal = 262144`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6551) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ea1ebcccad501a34"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestGreater = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-82584b775422a4d8"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal = 131072`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6549) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fb192e7e9be88646"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestLess = 65536`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6548) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89b102d899dac77e"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestMask = Inno.Native.Bgfx.bgfx.StencilFlags.TestNever &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6557) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c5c43d16765dd59a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestNever = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6554) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db766617b0a6c1f9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal = Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6553) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5f6f9db1c92dcdce"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestShift = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6556) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a9bba16267d642c"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.BlitDst = 70368744177664`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6607) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6390c8f76ef36055"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ComputeWrite = 17592186044416`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6605) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5109bc630a203166"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ExternalShared = 281474976710656`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6609) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73996bf41fc1f20b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.MsaaSample = 34359738368`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6603) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c97e50d5ac0fdf30"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6602) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f6a45f532b94662"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ReadBack = 140737488355328`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6608) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b636f5e75662dd32"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.Rt = 68719476736`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6604) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8f5bd9b70ba2ac0b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMask = Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6618) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8ea9017ddb4d86c"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask = Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6615) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8038f0a0fdd05c32"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaShift = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6614) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-70c30a801186feee"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16 = Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6613) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3dcaa1ae4bc8320b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 = 137438953472`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6610) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f17095603b4f1214"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX4 = Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6611) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5dadf6d31502e705"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8 = 274877906944`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6612) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-59d71e39559d31de"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtShift = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6617) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-977d6877894c242e"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly = 549755813888`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6616) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33071d235f96c6b8"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.Srgb = 35184372088832`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6606) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23c0f04e2eb98716"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.A8 = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6672) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5705a14dc0dc7d33"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x10 = 35`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6667) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8875015509d61e93"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x5 = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6664) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-548de5cf143f71d0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x6 = 33`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6665) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-02f1d2f91889ecc0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x8 = 34`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6666) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c35ea18fe18cdc4d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x10 = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6668) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff521b55f359e497"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x12 = 37`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6669) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f42959c467b192c8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC4x4 = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6656) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d743ae641a22aab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x4 = 25`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6657) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-512caa8bd702532d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x5 = 26`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6658) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c6ebe57923126cd0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x5 = 27`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6659) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c8de7dfa46c76f2"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x6 = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6660) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aebfce2697a31e5d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x5 = 29`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6661) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f0eefceb1f45c82a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x6 = 30`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4c5907a89d8e76c9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x8 = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6663) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b38598a970a73120"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATC = 21`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6653) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-54b77ba4a5fa4df7"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATCE = 22`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6654) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1dbb6cff587c4ab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATCI = 23`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6655) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b40ec74bc68da34"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.B5g6r5 = 83`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6715) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c33604913c26fa6b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC1 = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6632) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7014f16361d37d78"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC2 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6633) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e7953e6561ae80d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC3 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6634) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-413b1bd095bbd798"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC4 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3526ad3ef151117c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC5 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6636) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-518558f9a730eb55"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC6H = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6637) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb9099333a6cf2d4"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC7 = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6638) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0954fb0f0f403804"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGR5A1 = 87`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6719) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1779c5b9a7f8256"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGRA4 = 85`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6717) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-148ec8528a2f33fd"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGRA8 = 70`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6702) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1bbc5155f48eec9a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.Count = 100`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6732) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5db32c76380fbe8b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D0s8 = 99`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6731) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4f3981d5417cc34b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D16 = 92`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6724) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2dd749d6719cf7a6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D16f = 96`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6728) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe78f2ac7a89e54d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24 = 93`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6725) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9eb685a3749123a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24f = 97`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6729) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-809c1d0dc5be78a0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24s8 = 94`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6726) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0afda02d5519ab64"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D32 = 95`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6727) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2fc038f75e7a3b92"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D32f = 98`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6730) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1daacf030029d2c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACR11 = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6643) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b98c2cc8ff8cfec6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACR11S = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6644) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-add7986230fdcc26"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11 = 13`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6645) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dda5edbff906b910"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11S = 14`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6646) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-96129d43519837a1"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC1 = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6639) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cade0ea92b1af805"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6640) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0147a1351d89f869"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6641) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e4d0069af20360d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A1 = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6642) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-18c33aaaeee2c510"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC12 = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6647) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77c30ee4c40567c3"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC12A = 17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6649) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-582eff537e3dd93e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC14 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6648) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cea8851350e380ae"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC14A = 18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6650) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b22f0ef5d6ebb633"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC22 = 19`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6651) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e9cc2b63c5a3e1c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC24 = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6652) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-280afce2f48c1f67"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R1 = 39`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6671) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6336b507066b597e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16 = 45`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6677) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-95e6a5e010b593a6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16f = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6680) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-43ba2b4a58ccbbbe"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16i = 46`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6678) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2a88711d9d3110e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16s = 49`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6681) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-211d16e14623e25f"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16u = 47`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6679) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a753611410d0c47"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32f = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6684) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5b475ec9ea24592e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32i = 50`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6682) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f59d5013235683ee"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32u = 51`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6683) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9d7be71cb5dda9e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R5g6b5 = 84`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6716) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-230fc6149ce10428"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8 = 41`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6673) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-24df062ee3f9bb14"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8i = 42`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6674) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-17ba5178cf650daf"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8s = 44`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6676) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6dfd23ddf2749df9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8u = 43`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6675) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d8afb7471596782"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG11B10F = 90`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6722) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc9e262efb44de9c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16 = 57`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6689) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12010e567c525ba6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16F = 60`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6692) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-612f74a3ee8f8fab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16I = 58`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6690) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-808e3622e87b9aa4"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16S = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6693) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb083dcdd7b020ed"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16U = 59`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6691) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ae10eceba4e85f1"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32F = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6696) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3e58bb36e10e06d0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32I = 62`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73eed92110705a40"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32U = 63`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6695) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f77a07bc69e5791"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8 = 53`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6685) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a10db0d14d1ca86"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8I = 54`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6686) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b55ceaf16bcc7b22"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8S = 56`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6688) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74f3887995eb21c8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8U = 55`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6687) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-17948d24fecea324"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB10A2 = 89`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6721) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e0755bf9d645659"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB5A1 = 88`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6720) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e4edff53731ca96"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8 = 65`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6697) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db00fdf8245e35e0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8I = 66`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6698) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b67ce1dc7bada87d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8S = 68`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6700) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-78eda65f908e39af"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8U = 67`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6699) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80baaa71b427b580"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB9E5F = 69`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6701) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e84416e650d14f55"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16 = 75`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6707) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e65a72da10ff0d68"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16F = 78`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6710) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64adc43131d6f265"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16I = 76`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6708) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a01556ae4098e8d9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16S = 79`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6711) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-21c98ee0e8b9864b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16U = 77`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6709) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-18e398824526999a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32F = 82`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6714) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3481aa9d017ec7a5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32I = 80`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6712) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a426d890d4e63b5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32U = 81`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6713) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d5942a366d02e029"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA4 = 86`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6718) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3ddd421494ecdeb5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8 = 71`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6703) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c42d6cc9d9631903"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8I = 72`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6704) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf5eb58e38baf92f"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8S = 74`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6706) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c816e91728db8333"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8U = 73`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6705) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ac6c89ebe6f72c6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.Unknown = 38`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6670) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ecab39411bdb0ac8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.UnknownDepth = 91`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6723) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a12af8e5fbaa248e"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6749) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d12f93a0845ce29f"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.bitsPerPixel`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6794) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-049d76765e5a4335"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.cubeMap`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6796) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47a5a60ca146aa86"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.depth`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6788) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-41c4a030d5e261d0"></a>[`public Inno.Native.Bgfx.bgfx.TextureFormat Inno.Native.Bgfx.bgfx.TextureInfo.format`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6780) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3481da1eaae05b08"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.height`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6786) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff645d0ce20d3b0f"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.numLayers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6790) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-655a88cd816f4431"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.numMips`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6792) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc4dd21b85338e23"></a>[`public uint Inno.Native.Bgfx.bgfx.TextureInfo.storageSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6782) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8172e2bf94f11563"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.width`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6784) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8b6910553ada3769"></a>[`Inno.Native.Bgfx.bgfx.Topology.Count = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6822) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db541e2be67dae9d"></a>[`Inno.Native.Bgfx.bgfx.Topology.LineList = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6819) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-14cd62e056e92d29"></a>[`Inno.Native.Bgfx.bgfx.Topology.LineStrip = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6820) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0cf43eeb8dbb7488"></a>[`Inno.Native.Bgfx.bgfx.Topology.PointList = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6821) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1c5e54184405351"></a>[`Inno.Native.Bgfx.bgfx.Topology.TriList = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6817) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8bd3bd86e8e5c95"></a>[`Inno.Native.Bgfx.bgfx.Topology.TriStrip = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6818) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-078a504d709ca7dc"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.Count = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6841) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f98bd84126ccbf84"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.LineStripToLineList = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6840) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-649541ffef6496af"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriListFlipWinding = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6836) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9be8fb3cdf253a7"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriListToLineList = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6838) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-35cfc4d2a15d09f2"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripFlipWinding = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6837) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6195a9a3e112a374"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripToTriList = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6839) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8efbddabd9c57232"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.Count = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6867) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2eeb987b42f57866"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontAvg = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6859) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-497f27adb1e28d74"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMax = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6860) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a4c37f17a618963d"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMin = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6858) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b89e9170242435d6"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackAvg = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6856) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ff53c9c42b9b8b7"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMax = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6857) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b4b10834784764a9"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMin = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6855) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c35f2625aa1d7c1"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontAvg = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6865) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3408d5e9a35f1014"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMax = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6866) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-33d29641e9b02924"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMin = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6864) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad07e5ba5b250f24"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackAvg = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6862) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4ba5af0aea45ba0e"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMax = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6863) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf4daca8ba2cf2ca"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMin = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6861) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d84e99e0a5b3dc59"></a>[`public float* Inno.Native.Bgfx.bgfx.Transform.data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6884) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-263a31c9949380f2"></a>[`public ushort Inno.Native.Bgfx.bgfx.Transform.num`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6886) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-18f1802cd4397c6f"></a>[`public byte* Inno.Native.Bgfx.bgfx.TransientIndexBuffer.data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6910) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6980ccbf4534b354"></a>[`public Inno.Native.Bgfx.bgfx.IndexBufferHandle Inno.Native.Bgfx.bgfx.TransientIndexBuffer.handle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6916) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d13cb18885a3e03b"></a>[`public byte Inno.Native.Bgfx.bgfx.TransientIndexBuffer.isIndex16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6918) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dc30f972af108b87"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientIndexBuffer.size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6912) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e764b950509f125"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientIndexBuffer.startIndex`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6914) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e120c0c6d8fc71dc"></a>[`public byte* Inno.Native.Bgfx.bgfx.TransientVertexBuffer.data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6942) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4146223f8a0b1ec6"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.TransientVertexBuffer.handle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6950) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef5a5e97ad61e13e"></a>[`public Inno.Native.Bgfx.bgfx.VertexLayoutHandle Inno.Native.Bgfx.bgfx.TransientVertexBuffer.layoutHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6952) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe83589e28da09b2"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientVertexBuffer.size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6944) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0581d218ae71def8"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientVertexBuffer.startVertex`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6946) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2aa415839743c25"></a>[`public ushort Inno.Native.Bgfx.bgfx.TransientVertexBuffer.stride`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6948) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9522fe206af8d3a9"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Count = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6976) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39d05e005bc7c1c0"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Draw = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6973) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6a4463d1c89bb01d"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Frame = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6975) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4edcc4f23d94d8bb"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.View = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6974) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c713a911e39ab8b4"></a>[`public ushort Inno.Native.Bgfx.bgfx.UniformHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6993) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d13f8d847325bd2"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_a4f71ca2b114fa9f Inno.Native.Bgfx.bgfx.UniformInfo.name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7024) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7549e017e6a1d7db"></a>[`public ushort Inno.Native.Bgfx.bgfx.UniformInfo.num`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7028) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-46b62d3bc8ef6d2e"></a>[`public Inno.Native.Bgfx.bgfx.UniformType Inno.Native.Bgfx.bgfx.UniformInfo.type`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7026) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-951586710cbf6546"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Count = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7054) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc13d102899845a9"></a>[`Inno.Native.Bgfx.bgfx.UniformType.End = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7050) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-959dbc3400f1e207"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Mat3 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7052) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc2f49de1672cbca"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Mat4 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7053) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-451b5b54562fb8b3"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Sampler = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7049) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c19759fb08ca8703"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Vec4 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7051) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-be865c84d8b881c7"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64e7d7d662ea73b9"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_06d4c704d220c9c8 Inno.Native.Bgfx.bgfx.VertexLayout.attributes`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7108) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c582349572c5d085"></a>[`public uint Inno.Native.Bgfx.bgfx.VertexLayout.hash`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7102) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b923372a3e8e700"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_06d4c704d220c9c8 Inno.Native.Bgfx.bgfx.VertexLayout.offset`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7106) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-89b688b0dd29a945"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.stride`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7104) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d14c26c111ac26ec"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayoutHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7132) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-666bfad0df5c5379"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Count = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7164) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f8ecd9b2b95bf17"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Default = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a56ff3552d2c5157"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.DepthAscending = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7162) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a04133c20bd372e"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.DepthDescending = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7163) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b50843a78e5c7772"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Sequential = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7161) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a49c2dfdf36902da"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7185) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a78a7b13c029eb9b"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7187) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d1c68858009d403"></a>[`public uint Inno.Native.Bgfx.bgfx.ViewStats.gpuFrameNum`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7193) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6113f2941369cd4e"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7189) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-626198fc6d5610aa"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7191) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-711d21e493d7afef"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_a4f71ca2b114fa9f Inno.Native.Bgfx.bgfx.ViewStats.name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7181) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce00f92e508930ec"></a>[`public ushort Inno.Native.Bgfx.bgfx.ViewStats.view`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7183) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-260581bbc642018a"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERALLOWRESIZE = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L260) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d3104f1d0b4326c3"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X1 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L273) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ad9c75afdd5ece8"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X2 = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L240) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-de3fa668c5b7c3c4"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT16X4 = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L306) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55e4354628910f18"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X1 = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L247) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d2fbe73c58c2d26"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X2 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L312) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3962ff1d510cd44b"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT32X4 = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L232) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ca67997d5f56a092"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X1 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L262) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75145cc42fcda426"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X2 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L290) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6313540026fed109"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMAT8X4 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L298) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eb63d648f149b6ce"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMATMASK = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L271) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2a8974dac944c73"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEFORMATSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L293) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1d79c5457ee7f17"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEREAD = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L238) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-efb683fa5bd597e2"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEREADWRITE = 768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L280) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f480cf3497a7c4a8"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEFLOAT = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L319) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-127072e62d65b02b"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEINT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L268) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-68307d353cd07033"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEMASK = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L243) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-aa3ca7731f236244"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPESHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L258) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c1e03d12d59482cb"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTETYPEUINT = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L282) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f00c391e4f094443"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERCOMPUTEWRITE = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L323) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-52e2b6e6191066f3"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERDRAWINDIRECT = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L257) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1d73c26b0b7f42e"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERINDEX32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L266) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c419cbf201a2e70"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXBUFFERNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L245) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-03abb564f82e22e2"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARCOLOR = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L299) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4c316b5b23d104dc"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDEPTH = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L259) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-35af375d5f68eacd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR0 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L285) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1d91e1a27133537d"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR1 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-319f7fd7882d7fcd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR2 = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L295) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-58102ef5b45ecb26"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR3 = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L281) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c56b57fea4be112a"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR4 = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L322) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff80893935a64b24"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR5 = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L305) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb1b806b19588fb0"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR6 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L316) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ac0b4cfc19abec9"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLOR7 = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67c444f9a447b460"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDCOLORMASK = 2040`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L278) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-265a34355f476c98"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDDEPTH = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L314) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-193b6625470ab7dd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDMASK = 8184`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L294) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f3bfa962cae18f6"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARDISCARDSTENCIL = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L255) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2613b8a212625d27"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L311) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-803961dd9a533d76"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCLEARSTENCIL = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L321) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-03d780b2e724d02b"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEX = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L234) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72ffd104fae2c11b"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEY = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L301) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c1328865821c2fc"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPNEGATIVEZ = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L286) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7b222650b3138f1"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEX = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L274) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8cc81d5208718fd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEY = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L265) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d3c9d920e1b128a"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXCUBEMAPPOSITIVEZ = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L291) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9ccc09467adff3f9"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDALL = 255`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L304) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6366b40587e13e9e"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDBINDINGS = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L239) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67ab1813366afcda"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDINDEXBUFFER = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L284) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d47d800a6b362a65"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDINSTANCEDATA = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L249) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-20518bec70e343c1"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L244) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5b3e5bb65ba682c8"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDSTATE = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L236) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-08c569441b9c0096"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDTRANSFORM = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L263) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3b93b4bfeaf2e5f"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXDISCARDVERTEXSTREAMS = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L230) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d2867b97beae3acc"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEDEBUGCAPTURE = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L287) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4ba8aa5135a03613"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEDISCARD = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L275) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-460aeb678cc8d98f"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMEFLUSH = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L302) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f42acf20e92ae8ae"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXFRAMENONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L283) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-61d903b7c853d403"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDAMD = 4098`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L288) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-803e83a1ffd81898"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDAPPLE = 4203`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L256) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63d7ee8a353544c0"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDARM = 5045`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L289) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7a5aca661eb2a04c"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDINTEL = 32902`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L279) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f86fd50f75df9f10"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDMICROSOFT = 5140`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L252) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-68dd82f446f8d85a"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L318) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9f85a318a18254a"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDNVIDIA = 4318`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L227) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3270485f8ee8c74"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXPCIIDSOFTWARERASTERIZER = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38907ae25b0b657d"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETFULLSCREENSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L231) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-86b379774365233e"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETMSAASHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L315) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-703fe241f54b27e3"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESETRESERVEDSHIFT = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L228) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b0557886afb1f352"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESOLVEAUTOGENMIPS = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L229) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4b5a46dc0204f58"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXRESOLVENONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L320) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-886263bc10bb9258"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERBORDERCOLORSHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L264) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4b97de6a71fe3396"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERCOMPARESHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L241) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-633a49203738477c"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMAGSHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L300) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce6ffe985368f88b"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMINSHIFT = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L233) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c839b68f1ac7672c"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERMIPSHIFT = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L308) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09a22f3f1d9f39c5"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERRESERVEDSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L250) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e206d1832441895d"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERUSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L267) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1efe2de1d890886"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERVSHIFT = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L310) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c4f09e3e9bf3c94d"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSAMPLERWSHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L248) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-106aa13fda7fbb13"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSHAREDLIBBUILD = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L303) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9efdda3ee510d73"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSHAREDLIBUSE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L307) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1bb20b6c3e32d0cf"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEALPHAREFSHIFT = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L297) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ff49ead82d9de8ce"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEBLENDEQUATIONSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L292) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7441fd726178743"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEBLENDSHIFT = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L254) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-48b71b5b41cdc3dd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATECULLSHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L277) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42f8cad1d33fb6f0"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEDEPTHTESTSHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L313) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf7a9fb49f01e9f7"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEPOINTSIZESHIFT = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L272) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b6ac8ceb954abe7e"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATEPTSHIFT = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L276) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-41813fe2727080b3"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTATERESERVEDSHIFT = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L270) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-397d2e11becd2157"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILFUNCREFSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L317) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-476a6cb0baaa83cd"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILFUNCRMASKSHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L246) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb9ae06837662fdb"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPFAILSSHIFT = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L269) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f021384acf0c9646"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPFAILZSHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L242) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-395a1f46f3abe990"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILOPPASSZSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L253) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1830e6d22ed053a0"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXSTENCILTESTSHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L261) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1822b75b9697195c"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXTEXTURERTMSAASHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L251) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd183003e799819e"></a>[`public const int Inno.Native.Bgfx.bgfxContext.Constants.BGFXTEXTURERTSHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L296) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a365a6bd7bb13a4b"></a>[`public const string Inno.Native.Bgfx.bgfxContext.NativeLibraryName = "bgfx-shared-lib"`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L335) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-252fbd1253216db1"></a>[`public static void Inno.Native.Bgfx.bgfx.Dispose()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L32) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8cf88ca4637ca77"></a>[`public static System.Threading.Tasks.ValueTask Inno.Native.Bgfx.bgfx.DisposeAsync()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L33) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2f15c3d7d0b6736"></a>[`public Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.DynamicIndexBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4851) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ca2621934475c0c"></a>[`public Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.DynamicVertexBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4882) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44e9aecaaf760a5c"></a>[`public Inno.Native.Bgfx.bgfx.FrameBufferHandle.FrameBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4972) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7576526bdac61c43"></a>[`public static void Inno.Native.Bgfx.bgfx.FromProvider(BGCS.Runtime.INativeModuleProvider provider)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L30) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-01f3e20f2406d2be"></a>[`public Inno.Native.Bgfx.bgfx.IndexBufferHandle.IndexBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67a729f8d3be6d2b"></a>[`public Inno.Native.Bgfx.bgfx.IndirectBufferHandle.IndirectBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5268) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c36e5680e7726aee"></a>[`public static void Inno.Native.Bgfx.bgfx.Load(string libraryPath = "bgfx-shared-lib")`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L31) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a8557a94ced2aff8"></a>[`public Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.OcclusionQueryHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5980) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf7ccaab95ba7740"></a>[`public Inno.Native.Bgfx.bgfx.ProgramHandle.ProgramHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6086) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6bc8254f501e0c3d"></a>[`public Inno.Native.Bgfx.bgfx.ShaderHandle.ShaderHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6312) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-340617ce25eecb8f"></a>[`public Inno.Native.Bgfx.bgfx.TextureHandle.TextureHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6750) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8cb37bd3e69f347b"></a>[`public Inno.Native.Bgfx.bgfx.UniformHandle.UniformHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6994) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e11094bd2440488c"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle.VertexBufferHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7072) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f0cbfe7770f30dcf"></a>[`public Inno.Native.Bgfx.bgfx.VertexLayoutHandle.VertexLayoutHandle(ushort idx)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L7133) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7138968811491c0e"></a>[`public static void Inno.Native.Bgfx.bgfx.blit(ushort _id, Inno.Native.Bgfx.bgfx.TextureHandle _dst, byte _dstMip, ushort _dstX, ushort _dstY, ushort _dstZ, Inno.Native.Bgfx.bgfx.TextureHandle _src, byte _srcMip, ushort _srcX, ushort _srcY, ushort _srcZ, ushort _width, ushort _height, ushort _depth)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L82) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fc7023daa6889073"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle _csh, bool _destroyShaders)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L74) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c0c010ce45bfe468"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer(uint _num, ushort _flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L101) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
