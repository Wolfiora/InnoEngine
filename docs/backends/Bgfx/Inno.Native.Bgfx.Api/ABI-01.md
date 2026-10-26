# Inno.Native.Bgfx ABI · 01

[项目与生命周期](../Inno.Native.Bgfx.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Count 0bfc35a495ba091b364a5ab3d3a4a13a3f80a0cc11c31567ea1ff35577ea49dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Read 337d20f939da6df8e10cf34dd1f6e8b261833cf46307601cd6cfb9eae1c32147 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.ReadWrite 71e986dd343a1126b27b6497fa0a2c35aed45eacb7b76a92d0bfebf9b8cfff87 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Write 8d38c84cf027cfee8099e9aa368c70934d0828be10d44d880ec81d536b8aeb41 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl ca2734dbbc1049d2b0a1a8d267b82ac1368740abc74c8a03144ac5b4b5806f36 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc 18f329924f2846fae9969c067e452d7ffd36373571eb6d074b8474193e6608e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.access 82cb154ca0320f82921e54939cc71945b2211720287f66555ec81bbfea639f99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.handle 7df1d393da4351432c4e939b99eaeb20b282ebe3a8f4b6bb38142c42c299078e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.layer 3ff98afce8f96b3baf3ff94be10347645133457afc8b483bf48492cb9882a363 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.mip 709f05611fb955372e0d3a04d7c8e710b3f88322fe4782e76607e15c78e8231f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.numLayers f06e1c6a10592e1ea868d4e3387cd641abc7dc1d943f47cf0c570b33f75cbed6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.resolve 29d113751008fe7b0e2b1f656774ffee4cc3fcb41aeed1040fa525646e19154a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Bitangent 326e286e8bdba3ed131f0aca4ff08738eb4fa5ac384e4889efd96b0ef95e9fd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color0 803241a00e9c6e97c2723cf892c2dbcedebabf0c54a74b82f89178f29abf09bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color1 7564c1931a6058afd3b98cf292021318b48d235629d9aabb6a8317f1ffadbd0a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color2 35ff6e122e43108a5bec50e94f67e4c17e90a82248125c3266536e757460be03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color3 283e8ef819ec8820512db7ce17690baea882f6e3878d2151e250331eb09907ae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Count 803e1d8db641c042fb7137f70c53fc9d8a4ccba0dcee97cd7411886c5b3b8d8d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Indices 25687869149130405c60072a191c93902ea9df82b6e97318fdd8b3023fc04c06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Normal 53e22d735a37da1d39e4f058190d628e7a7c08e7ee2c9f34cbd5cfcc699d89fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Position 13aed34c18d87eb76d480a65948bd3abd7a25511e374d34c670fdfc85ac9ddff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Tangent a31a302c0c55dea2ac58eee84a981704c6d01468f2882398c6fd8b5abc9471a4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord0 b8a301d17ef985b8d92d0cfc78ced4f13a965a221b4200af249d72ec917115b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord1 44158fa789152cb1355377a2646f9af961b4d9aab638732b9ce85aa80943f1a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord2 349373569d7b1e48505b155712bd9682b9054b8c051c688f8c68ef3cf1a0ebe4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord3 088148090f94489ebab3b96c71f9cda0491cc3710c5fcea94a6e7abd9fef8197 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord4 4595cc98f39d28fe28b549ec52f846c72d93fd4e58b80061324105eb19f77111 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord5 697fa601ff2f58c2435ac7ae827834e1e43cd3edab7604f089914cba8259b1b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord6 dbd0e499be59bfbb1fb021463354955a48ff2679c803ee7f6d3c2b03c09430bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord7 b386256587d02f4c575ae18c8739e5b000047557b0542fd2528d738a84d247b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Weight 1bf74d17915ce8d1735cdcc2e5ada2a5ed58524747cf5f8c51470ef963915832 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Count b2b9a57b915278b1fbb3c9636382a7944f84d30c996dab2c1e9e8be74b3d0acc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Float bf67345863359d77c53a534f734171532bdecbb830392b03f9327c175e7ae577 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Half 442aeaf0f9bec4e09367a730ada38aeaee317cf1a442518d758ce115ee42fd6f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Int16 8c80f6b16bdb4e4732fcfc906411ef41801e24cdd2b73ceecc28e37282e45ef9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Int8 57c42b036b8e829cd24fd9d4ddc1447bfa575b1101710cd88c426528dc3e2e59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint10 d1e9debc525d61da9be88917aaf1415f14f893aa0c303ed83f9c5cb9fcedc96e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint16 c3d11dfec20fed06283c622ec53dc0d6ab0ec284f565d441493909efe929d261 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint8 784f8de6bcc3daa22e3fbc6773bda10fafe466931f59f049599eff59924a214c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Count cf908eb5ab9b943c0936b70ff1b3f863e0a0e093b2f6205f248bd8cb8f993190 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Double f1f3151a43f787f4bdcb7d701ae1f2e091cb367d0d146320cd755ba9374cc78b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Eighth c2e6fabc09bdd1dab31259c38dd7f828500209ec25ede2ac7fb9872c3b2555cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Equal de9ffbccbf5147f6dbeff769e9ea40ead98e44553590bdb55333b35d46679722 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Half 718e3a2e9123af7727372fac2289466701f7d00312a512c504f79ced08fe2907 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Quarter 6a799e2908b8d62bee6e08e5ea101ffcf99af2be4d91a4a814c31ca02dc909f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Sixteenth 86f041d159545ccc774e84212aabd34b4110e0cbc81f7790b1d4f81c032996e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.AllowResize 12a6f4c6c1eca467e5c3fbd39f27624ad474ff8f8e4e9ece97779f29d1bbbcd4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1 10af29ffe62df99a9cf39500883df255d5c66b62332fef3be8a8c70753fa5db2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x2 a23d97af8d6fa62e66aa24b6b387256ec83c56a66ca22a5eac0c16205e78a4db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 5cdd50acad9855b3f93ac0ddb1ca31b2f71bfea95f2946b7cbb6d33fea5a1275 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x1 fb5e31f528475ab44138f27923a1596ca4926e891fbc260033dafd1079b93b2e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2 cc5bd0156b9bb923d781e674108fff7ac114dd93f697d0424ddab4176d4775a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4 5f954c7bfbd822c60484349865aca939956f4d9def3dda66b0a784232b2d618d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 1c056744b4c71413eab71f6fb0e58b6f802965f637a17e3c3a0ab9a60ed6599a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 0f2304c16ff37196242139ba1ed5d291a225c7642a66d7e0a07cb07de57c57fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x4 65712cfd95cad6d79df85dffb1eee1d74d693006fa6e9aa27c1c7498c1a5e38d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatMask 0f28aa8770d7c9084d64efa3e99ddd838836aa47b69d825a1f6875b009522a69 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatShift 4a27d32c200da8444803266effffa7806bc08d93260b2780c642e9faa51cbf8f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead 1cd8cf0763094dbfad315728fdba6a87b1cb9586e77ebf4d6e79344f98506bae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeReadWrite 6ae4d0f28b35d349672dbe8a9505c037f96c1f3207a0fc5168e5adebc898e427 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeFloat e2de7b43b8bce0d9ba40a9c70f2ac7308229b699d8879ae894165ba8094a0586 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt 5d1666f582662ef559fe4e1db8367e9b7c5008235a7ed5d00b382d68a113ee85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeMask f0f24151180788c96a27c60c3b95be9de59683cf16db9c1b68115d6d265e1396 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeShift 110d1f477bf3ff1271ba907dc9518d6499fdda98e876f024f79bf0014f55b327 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint ef227e2aef4bd88cf9d586577f6fb561954f834e096cab8cf4b760951e30ff21 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite 10175c3cf3b2f73ff39debcf49a4ee3e2cc1b8a85ec2e27279ba09f09d8751a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.DrawIndirect e3ce8f9353140b2646f29d7cb906bc9539c67fda8d678473adc769bc8d31bded -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.Index32 88fe42b594dc1dda55a74a4dd072d909bd7b819953b921c035c5f17e9a348e88 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.None 9981691488de1c28f8d65873013673cc11a757b9049b6d98af9eab81b48a8d4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl fa98a95c9ccedd70a7753910f9f1b33aeca9e3bafd64a7156fbc9ba3ede00fcf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read 2466a0f08c25107a21ae2f6c739cfc7864d9e6e84c282c8d3b8d5cf62b13b9e0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read_size 1445d46c5fc5e0a562507aca626446b905d4651b2098a32712d71f98d1754951 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_write b66d496fcf3bee87b1fe9cdb6a0d5068336ac82eb42661d0d211dea73d0ca552 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_begin fdef783ce3e9dc1db3b160a78527ccdf06a66f3c53cb8262e483a7f6c694d985 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_end 74ad919f517943c8b59fc353325d58cd39f147e756d984b677f45713faa66060 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_frame b284fbc1d05a04e8e629ae7a4641d37c038be36e3a5f8329bbec523f4b6b1437 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.fatal ba2cb4e0e35a443734c07549d15d6ffea642c1f5377e0181708d8df1e2526e6c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin 66271abf211570bd0d43fbdfda355f0f080a2234f8a94ae90d7a7e90b26e534d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin_literal 5558019aa6e598e3aa63154ec2c9ab993fd037b81733d658f2b01db1e051db64 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_end aafcbe676070763b9b82a8d5e0363e21676c9cf6d7067d6e8c44752fdbb63522 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.screen_shot a4e8808c05e3c3741de9ef20129ef56159bf8901f6b7537af4f53c695ae9395a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.trace_vargs a39ba22f0eacac52af375f232988a386322d79054d9628275b69374ecdc6c5fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.deviceId ce44e23adf9221d4dd819547427468c31ab30026b6b8d799a6b319530fef9f17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats e5a91bb34263003eb361bdd4ba5bd70b9816f4aee6b5899a2a54c3a77fb2b118 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.gpu e2ed71a5dc0c5033c21f0cec99be2451569fa5fadcd1dc74cbacf1e4dff5caba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.homogeneousDepth 969593f4db2ac52b1c5b1b2a4308551ae61b74558e9e9ffe9032914515e0f5f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.limits 70670eb0f8e93fdde28a3b7fb9acb0884281d80605203f8cf2e215c6ad7ad34d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.numGPUs d4628f2ea688ebb87b898caae22e78f8909fd174cf2399125d7aebd7bd86b662 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.originBottomLeft eebfc475d12d398c14ec4c305cafff9b41f009da5dc749e6541ecae645db1c16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.rendererType 9fb71805ae9b143422307d5c3966ec61522d09cfecf353d15503641b367abd81 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.supported 8876033cd63d07f53d88bc19cfdd949c7e126efc70efdbf5bff79e7ba009829a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.vendorId 7fab8c53a53a0018b9f8c95062e9e6f7f2c8343123439aaa2111d2cbd29c3d58 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.AlphaToCoverage 439abc5e60eb733d325b8ee01019f661577c4263facd0bc979df2769a9fc269b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.BlendIndependent 620a8a1b087d551a685a9c8dc413322d64316e537ebb813dec2d71a265a632e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Compute c1bf0854c47444dd948f860b9fe6fb9f3b8ad1062d6d9439e263958c1bdb56b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.ConservativeRaster 4aabe61fd057794e3d8228761ce0e676269f217f79e73ffe9e2f14c9766399bb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirect 6ea6b64e0c8aedd0b08e64a8d0f36c4aa5461bb59ca6d243e0d32206145cdf3c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirectCount 5ffab2fb3554128a6c1dd9da8785ee0befc48ef654c8086c58cd2182827186a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.FragmentDepth 42083cd0842658e0d7146e8b96d3d963afe51fc61ea4a01d179508e04ea89734 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.FragmentOrdering f6eb9d43981873a808d5276f7a111b6d4323bbbd11c25d1f49b1e1ceea255737 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.GraphicsDebugger 9f8082191d726732a9128340c6ef8966f7a3e1b4c5a96e6428630835ed302ca7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Hdr10 5695954e19e2a282a117a69878b8ff3b9074e2bb845c244264a32906aa9cb0a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Hidpi 8dcf0bbad02172565c4abdfa185522c5db3743e6b56505ff41da5451e6447c4a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.ImageRw 32c1d31d6298cb1ff355869e759d305d3619287834ce3b7d95ceaa1a226fc4a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Index32 83e320749a9cde06e690ea7afa3d8e034dc2b1d34734317985d3d6dbd93f6269 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Instancing 0effb33683de926a5619701905d7e927eff56106469751468626b63613cff5f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.OcclusionQuery 32a9b45bb2a11c4ae971bb4ba2bec5b4a6007be9212dd355c248fc90975fcac5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.PrimitiveId cb18a4747c1953955f717648dc93dd527fdc27b21acbd70af01cfe9ec974b8a3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.RendererMultithreaded 4305ccb9e50528d56a5135f5f32f2f559d789f5557eb9c1091ad66fbe8f43db0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.SwapChain da5b89ed0ee60a5bf452009e0f8e3f1b9daee9dd7fa70d390caa87264626b301 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Texture2dArray abbc45b89f7fe8dbdcca50c0070e4b406286d2c3cb5500e4ae697c67b8923f76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Texture3d b582f52ab1fbf53a04b6a4349f2e467e5369ca9c8e1a03701f8280be1714cbb1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureBlit f32fbad176ba23bdddc75beb79ee5e493b8cc1c2ba6c67515ed78bfe2e72298f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareAll c42b32af74f15d85e18eaa48e0ddd13051e50e65faa9bd3b7d9cbdac48dc14fd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal 1690de3f7ae12ad25f2d6e02cafa691a23addcf9fd7e12ea14e83b16bb481d26 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved d164bb2c802f4252c0b5396b1689e4d6f9bec3608cf13d917a7443646eaf5d53 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCubeArray 987a549efb963929b3eb7d8e3ce6953e0de35b00c057de13f559ea582ac6c466 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureDirectAccess 861e58eb89e209200923df2590a40ee32b1e1feea166d2910e59ee4792c9b6f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternal 7d860b15d2e995de01a541c67095a635dba708f598905f9dcd7f114e8cbc1304 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternalShared d5d2b7ac7ce9940fdad51b075a09585d1bea16fcc59e239584ee8f3439df4683 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureReadBack 0fd70a805445315a748ccd4e82b5b2e80293f82dba75c70334837c15605bd67c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TransparentBackbuffer 6e52b40379d0c290542e96549b275dec0422ee5cbf889c30ee016e8a5a21502a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VariableRateShading 775bc728ebca9878586fcb54229f32010f7ea5d6ef9c25a2f0192144433ff15b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribHalf e804b9498411c964f39d163d6900cb4a6e4f85b366686c7153e80cf6ee400519 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribUint10 6b9b66d22872edfb396ec5b1e8c6b15242b33883ed322381cf8289db980181a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexId f9b2b8115c314c6be4b9a247f4dd97ed11f3ce2a9087fd2be782cc135ede8c3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.ViewportLayerArray 1abf26fee3105cddec3bdd410b85877bbcfa49771146daaee728c0cc822c92f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2d cffd9d761387f05d614e6acb1ebe324bb5c8ebab1b3ae537b8020ba4e489c977 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dEmulated 8df86ad28b20f9213e33a2f57baeafb1465755e96cfb4928cb1a680c1ed43751 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dSrgb 9fe91314d60e3777683f75e18236033364ef33c2d7b30fe0d5d3b47e5d2ad325 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3d 730ca36bac2a11df5528c807c76a436d354a54297e4b286d01f9a40d076c5204 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dEmulated 1df207759c46821ce54a7515369096f45840a2461592f578dc641af00c8ab5f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dSrgb 0e68ccbaf5d7d5e3fa24efc3e59e764306294fea7fc2633a6863f5d37acdff90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureBackbuffer 0f9c6c3c2a2bf8d63072640e71df3f0789913f34b09c5d7679382351b14e6f92 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCube 5cda4d661598c624a7af77ba0160bd7cb8790affdfca01ede7bc0af64c186c27 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeEmulated 7b0bab205b7714af07b836f2a2bde9c119d251c8520c0b182bbd04fc91d4d613 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeSrgb 2f5a7fb5f194c7dfe7c10d32e6924b70c68b315167e6774e547f8c08a8659c52 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebuffer eb264d5339eba152a40fbbb2885da1d09ef89cdd2f0325a4302eecf204ce535d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebufferMsaa e99f7e9b275948c1540aa348cc67faf253a4acc86fe33a3d0d687a07bb391d14 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageRead 499f596113e81b340bc8a0ba4a8ee11ff79acceacd377db2c79f5afb46f5471f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageWrite 7640fee5b478ce82d640fcd696080c3c4bf5bba335defa6b7f4ab6987ef41f15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMipAutogen 5990eced6850ec4ad709037e7aff731278a4a0c1ed5c4a709e7e0ac1bac99f07 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMsaa 6de39e129f266a00c8a120b39bcbc943867e636318af9a853e9ad1f3336bfcdc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureNone 943de3cfd95e6776716ac500920e62541594d57f06aa87233471cb6eb2a9f101 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureVertex f113ef0b63e6d7472639bf9919dd5652e0ce935583619a732bef9b763562814c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.deviceId 210383e7159d5179187639ce8f105bf03f02937a684507925f33cfcd2e25bedd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.vendorId ea3d2264ea5a649a38b287fc3cbaa2f2fdff873e4915e717b23a9f156b8d0a4e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxBlits 4bb03a9deb5a008f839fef657040c174ba60d6dec4ca61dece06b6b6cebcc8e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxComputeBindings ff095532263ed255c1a735d960a091ce1b48cefd2c7cf1d4be60fe40b645ede7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDrawCalls 5404e8f540caf9a6895ce07101d5899d6d2a795d95890e9bc44a09cbd9609ec6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicIndexBuffers 7bca156c069de95ac05003fd51084b790ac28c9f7a032feb8a455a6c03526f17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicVertexBuffers 0424321ec2aff930754888e85f1b421d2830764298cc4e5393a4fc9ac4019009 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxEncoders 82d0460e19755e2f47affdcd9e89a2e50240a205c2b6d81f50a49034928e81e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxFBAttachments d82222e2628148d1b87f6e4ea00a24639e4d64533d367d4ff84f788ff27868ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxFrameBuffers b6135b745caaa1e0e5845f74d9759f99f47d0776aa54097ed8e8174531d3dd14 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxIndexBuffers 0c2aae4b18275ce796761b3ac888a8002ae2a5ae515c49a57a98b8d75220bd78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxOcclusionQueries 07af44b66e47aeb757de39ddaf4b303fb5cca01fc629a764e652bfa377d52bce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxPrograms 1f5b13fd9fef992a11c49064c04fc118be39be6a075eb953cc7beb9c5e143372 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxShaders 237a6e7dfe6bb8bcef0f5f966be1230667892189ebcc48489c40a4f94ef7353e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureLayers 6850f9e0d4b8f112ec543296e4140b4cc6fe17274014013c56301cb1f71cd0ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSamplers 0b72a03a9d74b240d433e8af46306685be4c40cda7dda712feadc15d0501a17f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSize a86e33659d1f78b9da0ebce4570989eff8dcd8b72e882c3ea0b40b55cda73337 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextures c5fecbdb145e60a007136d06ef429a9a5f6784a10cd08fc00d12a5e85a4cbe19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientIbSize 0655d7410f1ba44ae5588777e3f2a959be9bc5182f58c2f479b32ac975551e2a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientVbSize b8c86da82f8e759710064242a75060ac64233c7b912a206524714700c96cd8d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxUniforms 21c1ac1fa486528fcbf3058ce1795abc1eb4586cc02d251a2c5235e65d09e744 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexBuffers be25de997e1534693de0a65366cdcdcf66a5fe68c5fad0c51e620b0232dcf34e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexLayouts 9f6275caf8d2aad3cdde495b8de59a1ab2f268bf3e09ebb9f5338cfc75616d24 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexStreams 37a2aa9e94beeb868646a8f5fc851d574dc5a83a66e6bffb5df5ea6e4b592b74 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxViews 7e0a273160b38a192ed598137df585833c4a2180a805484a1cc161d31d4a0f50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.minResourceCbSize 1dd1eec804eba223c0772ae208a19334e459dee24c9c139ca832598a5109bed5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.minUniformBufferSize 706591535f20fa88dd227a63efc289bb84666adc251403d514eecb0dc804cc28 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Color 46b30310b7235e42ff549b7ee3c480c6ca6544c8bb4bca3eacf884161b21388c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Depth 8d345b15eeb112a70d64d07561b42baf4fcb9400bab991071fe5f59bb1b9ff10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 77b62c2635b3e6dbc423804b47097e74326a9cd53f5841a714df13ed86996d3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 fffa5d57a2d0ceb4c8ba8ca07223abd52b059cdc5f6aa45eab11d5286e8132a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 010e0647c7d10affaf52729bbf5b31e3e47fe1ce522718d219b9a97fc5258dc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 92779379ed46af0b20a1e2d56965ce4e47206f6cad0a290fe5248c9488ea50c2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 4d22cf8af73c26a0249973a56759e919567b9320f6231b061fc49e0197d90026 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 ba0e01161f6af9c58c98177831f1a6d8195ce7cf034e27333c22ed8bcce28f40 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 137f4c2e90d991941209f179e6db549ddd08bd719e1c0c3689502ca9624a24de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7 aa064e164262fbc22e2034b70a857d78325bfb1ce01435db1b5ce298ba4f2154 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask 5b2481f45c1544629e81e6d8f400af7cb66573fe9277b757d916ef38bbb9330a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth 7d8f3e31fa112e524f1afa820c7525d8f3ff2c412bd0c87b02e7ac5e4518ea61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardMask 3e2c50a8634e383a5acd52bf67b65c655a6e60b9b1932d046b2851596e179501 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil 56f6423dd0d8746c797fefc9df0c148720c93dfdbbcaccb8c7442ef9fbf7c2f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.None 242bb572e3915a1adf262e866c6ba2fb7172c792c0037fc323f1d1ab5ff9dcb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Stencil d835d99998e725e9b1066f2736501b69c8075920df99cc3ae8cfa5b08a0e6d73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERALLOWRESIZE 7d41f9a2d39a20ca3bec65098226d125f115304f65a320364ed00e4288a9ba92 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X1 c6bb67475f8297339bd668c116221f92af75df2046fad4d7e7b1d5465e3152d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X2 36101339c8a0ca4a423ddbbe6a6205f7ddb218c45823a3790366b3b6184d1d8d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X4 513c7abd0e4789f31729666c127e9c2c8b7eea6b941a7e7c76d57392c778edda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X1 7ed3a2bdbf5acc839a8f29b170a470682433b614a22a13ce6b998203f76fdaae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X2 20b85cbc07cf983a135a6424ef2d32edf464924b0bb0f38ea07a98f64d3d0af7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X4 c9e27a8343cda437031058d068c7f0320b2a68bd03d4ea01ab208bd3e93579cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X1 93997ea1895799c433f7469afa86abfedcefe14d25f2ed8df7457643e5847d95 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X2 64b70b150e57b700087bacdff03d715d66598105ae065363b9a8e5508cb47a93 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X4 27e18a020670161497f9536650ed36d6ce36dc0bed26130d64c36238d07ec9d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMATMASK dd8963bc297d4c49b35404f65d50d4c7039100ac2af954a733ef638b4bd80e0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMATSHIFT 08c30edb479c50f77a7b1406536e135feccd35cb75cf82c47eceb16135cf8cb0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEREAD 35a838ddb06b0076656b9cfcb082f8de42db68f10ec9d3adf3bf8a02ada1aa02 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEREADWRITE 1c043cda853fad9591467ea85b0e5eb6ddbe2795098982bd53bd72dacfab5625 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEFLOAT eb39154f232fde9a6fc60f7d064a67dbf15471e243daea5a25cd66ded5fa196a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEINT 6597a6d0c0e1d3c53f0493bf2ed870316aa2c32946aa31736c2ab611545166bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEMASK b0b2443ef0d9e203be54d1667dea618e6f8f65ea967d12881a3f30196a92f209 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPESHIFT 573872b7cd51149647c3cb213d78f57862e0f5237c6b46c194a1286754d424fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEUINT 36084f47ce5694688d006d5b37eca3a81e071e43ed3379585d4bd0663a7fb3bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEWRITE 02ad68aca6e3874b86266932bda070d60539675f95825806f138a78de2d9cf26 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERDRAWINDIRECT fdaa09b53663c957e1214c5ac09d5ee3792df3c550e89ad74e58481d1475205f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERINDEX32 c5bd1fac1185faa8234fa5e86e987cafc6f7dc796482d3f10acf0d8a114d0d9f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERNONE 9a28e976ba449736b7d0f153b872d0926d92417b9eccc0480e7d15ffdc8f0a63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARCOLOR 1739f3084056cb8bc665ac71e8a1a8473abfb7d81efc5e15d9922aa28b7ee94b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDEPTH 11c618c08d0595896a220a225bf5e7ab3611d4ded9fd5d3d31e65c23f1f234df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR0 130c7228fc17ee84833c08da30e64e95ae8d0ea2c6c27556db2c871afc5d5ee4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR1 ca9555217c5c4bee786fb22d62596f5f752e27bfc72f8155016c7e8879b764c0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR2 bf58c58893d63b882b62c57dcaf8cdbbf44e25aeff1aa70c9ae7646e95eb49ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR3 147a4f726efe05d18108ff11dd889883aded4e3097c93f1e58d5a7516bb33c13 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR4 4aa4bf954431c85a336a798cd5795af93371d3599b58d294033508cdf59dc17c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR5 ead153075026443813e99d111cee668af35e938016430465f53e293fd5035164 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR6 ed81653ae4598f9e1d1d18a8174e49eea41b8abf7a9959e45f2f710a7c4196d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR7 186e651f0fed51f33f42cb80f3aa3ef0983ecf031cc5018d59de7f841ceaa9be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLORMASK 173339ba17665196325bd89fd5a9bc9724cf8e084b492121fdfbe63d05497093 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDDEPTH 4e0f5db86972b81977b523e2f131c89170e0cd37a24ccf62b66df036cd6bed30 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDMASK c6512459b1449119980c4c9dfd332bbe2b05067e2e90dc661a657811d8c681c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDSTENCIL a4e8d57411ca76c0d5f7aec093a31c01114bed44197c33f87301a271236e6584 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARNONE 12ffd7046f1e5225de931065073d62bf3971f99b013162bf44aaca7e7a38cbb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARSTENCIL 3cb2c7665d07122420c0aef3204de80427ea36744455413ea8c0be5dbae0ccaf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEX 6e3f20b1fc6ecd8ee9bd6f9773b33fa7a5161c7eff715dcbdc8d18b29205d6d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEY b6331fe71b5490a806237e880e3b3b14d6e53a572a367f3906aea882db029190 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEZ 6afcea1738edd8a321f726a5bff93618dcfb45275e45753e007df9cde0058115 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEX 6f613b8504114083f92c5acbf7477edd2ef5d2dfea18ec42db034301fbc397e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEY a6a7226b20d60d4a2257c761d313cce90ff370204a9d69f89ca479d1e0e7a6b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEZ 82c2dca7ca5ef66cbe5be5b1a6498a666620da6e524171a1b8ec8799cc4acc51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDALL d8b374cca31f589c5d3ae8444d7286f7cb896c96d192fba1713997ca72e1a861 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDBINDINGS 189c2b9b58def7dec09eaf63f961ef159468d510d5bdc7c065fa6e9237a6d91e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDINDEXBUFFER c6e9707b4a02d2ee6fd1afa1bf4d8c628cdb0786c7cb4c668a7fc2ed8f810020 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDINSTANCEDATA 85e76dc0a0d9e0f00bc011d9ca007857d51124e649f6be4667608da9f24e7f81 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDNONE e3fc5b747d36dec3c3376db3ccb0546505ac50cb2e82384e5eaa5cc67e201ccc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDSTATE f8f8e67d0160a1115def5b1c33bce70f3162453b465aaaf8db51e2e004612aea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDTRANSFORM 91db442ff9ac69ef9c4bf351577b3a423a224ece57e9d1fffc111ccd970a7afc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDVERTEXSTREAMS ffc2ea2edddcc4bc8458aba38468bf44ca63f47c82de72d341637ab973605caa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEDEBUGCAPTURE 6e2b733742d041ddaa5dccc8a03939f21ab3358880e93d9b0a703cbb0400307f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEDISCARD 4796aae5d9534290efc2561b6cb477342feec02bcfd4e817e8557217f144506a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEFLUSH 79655d5a05a53d3166b8a0a241e423ea791b8c8cc09eec85211ec886591dfe9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMENONE 67ad080a339ae82f21cc3376e635b02ef01c94a295bf28df7a07074e7c247827 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDAMD 99b7b99bbcd34c7d057591351d298fee9515adff639a0b4c8e73eed780cc8297 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDAPPLE 2a527c8752e9c25e4464af191f79b62ba6a41f7b583e880cd14aa269cf11429a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDARM 13c616d7d1e578ac6eefbcb05ea924bfca611e9dd23b7736c6452ac4097d03f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDINTEL db6db6cc9f95b3734a7cff4cdab9e1586e0a7f0ad698e25465b3efc153c14e2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDMICROSOFT 21cfb2350c1b2957b8c9b0dde1ace3071f4be61a6ddeedbd596a442d7348fcfe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDNONE 138846a7d71aff5a448f42a40fef26fabffbaffb971a7b1504705a943085b937 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDNVIDIA a074565b3ed78e2befbc6041d4f139b87c51af4d4d76c0f95d710a58d4225df6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDSOFTWARERASTERIZER 07599046bead4add2617fece15ddbe9e7d5e8b5d6bcd36803823e4c99c6395e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXRESETFULLSCREENSHIFT 7a406e976449d9e6fa37d8f4c99c40765289a9fa4efbb4cc7e43d4dc10d750f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXRESETMSAASHIFT 6ecd5f5b8c938c2d4bf7ee786a993ed9668ccd91a0cf7337d3429b6f0b6d7996 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXRESETRESERVEDSHIFT 0fc4b626ccee64906e21bd09402f5954365fabb0fe5efbfa01adba42fbae2a3b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXRESOLVEAUTOGENMIPS 80d09551523a16c57c2b2d59313368357e859601c2a6b16d33cbce0ba952e4f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXRESOLVENONE 304b40d172c64ed7ef0899dfadaff3e2e05f42f5336271a7de3e08b72d05e7df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERBORDERCOLORSHIFT cee774cbff51b81d534b7341c3c7389d477b4bc8509c7843ea11477c7e50b3cd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERCOMPARESHIFT 7929d4b1677b0dfd817b8d77e3635bbc95e053a6f83a23ec25bec889c66d468a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMAGSHIFT 8a3cfc72cf1437a0854a2b0246043e32c9cff046ace43b86fc84794531311844 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMINSHIFT 9b204689512e363e03f3cb89172ba2548cfbf6678de76524a0ea175e8755f290 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMIPSHIFT bd4123104341198710bc85772bb9a8c6c5f94a64c19ce247d5db94b0c799ba4a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERRESERVEDSHIFT 40078471003034140d2e89ffa0c2a65bc27db75fef13c1610060d7e40e7c744f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERUSHIFT 272234fec7f25b99f3eb4bf5c5325a6d2518072b426cb683ff0bb25eb876acac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERVSHIFT de9c18fe92649b46e2ad3d0c0b9464c3148d6b12f2aeb81bb723308c160bf87d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERWSHIFT b6963ecc0367b1645fa50293d979be62ad9ae9b5be7cd8054d2e85cba8880515 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSHAREDLIBBUILD dab6957e88a423be38c94ccc059f996f308512930f27ee84764157e78a4c7996 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSHAREDLIBUSE f879b535e36aa9b019e132812836aecf80eec455d975c24be1cc67a39b5d4246 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEALPHAREFSHIFT 028fb13cea9bf3534834b266e987eacae015d3f28684f3b974b0f07bfbc84c59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEBLENDEQUATIONSHIFT 19869540a71f3c3defcdeb582922f72437a9fad6ae29ef8f665ff9b6de96166b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEBLENDSHIFT 1a048124cfeee8a4a4cc7386b81db9076bab3ed0192e6629f92fce8fe861ed5b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATECULLSHIFT 0ade37aa3a610653f39cd8668be28304e09a23c00594dfb100c812c04b3c3377 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEDEPTHTESTSHIFT e4e694f8549375a674ddc8d4b7a31ad0f5e5612b09f7cbeaa7cd5df12113b49b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEPOINTSIZESHIFT a1fee1d132e4a025ea24829fca7b3b45b1a9776d13b1a7055ce18ca056c233b0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEPTSHIFT 1696a5835ea818bb1a9eb2d2cd9bc12e1a7494b815d6972b9e155109c5f4de1d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTATERESERVEDSHIFT a22b01ce23d9a80bcb44b1cedcfb6b572ea8fbd731071efb5ab0898115bb5430 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILFUNCREFSHIFT 24727e316df806a9734913012aa915bd39df37c86b7bc5fe19f0a7b07d3c069d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILFUNCRMASKSHIFT 166058e08c8ab3608d6aa368ef211c827b8b01d461c30e674561100b92b3b8b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPFAILSSHIFT 6c6144558ade7e70b641a6045b679de4deeedabe2e06c22c76bd996adc4e3c43 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPFAILZSHIFT 3fc9ceda20757ded68a4cab73d95183c1cbfc197690c24433a0f2c1380df4cb9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPPASSZSHIFT 2c601239c6f95486027581c6c10c5119c56d799c0d43650b47a05294f662e4b7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILTESTSHIFT 45f8acebd7d65fb8be8861a39f5c481d937c1158f3f4c2ef810cbe6c89ee6140 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXTEXTURERTMSAASHIFT fbe4515b65b6a68d88e4e07d9e1ef8fc17435aadfae9cac435e7de7a839371b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Constants.BGFXTEXTURERTSHIFT 99b215852fd4a28e120bfdf435557cfdba43d46691b43c5c7a86d69ab2299a28 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX 781661cb869a5a35b953fa1006d3fca9c05f3e94a496e0856598c27cd55eb567 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeY 8b9242403aaae05efab35f30fc7162dc32ceb9fdf175f02ddecba08f40879ae8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeZ d621a5e5ab6152a00ac2ab9bdc0193149223383e4e1fde6734cd50b6d131c4ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveX ef91aa7b46656652da50ffa948c351f5ea17451232f857b05d69e50850170bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY 303f26f23186972768972da5fb7b7479533cd73c1f8d963fdead6a4a0e53f730 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ fd231bb359d3dc4e0fe9b90fc87fe84a5eb2543a4a6d3d2d6806dc2ba07f82d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Ifh b0fdcc46cbf0d18f163248bdd5959197f2e6fb7d8e768e340a84bbb8505187d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.None 6bd3227881fb6a7e2d3e08a306852d0543a5b98cf9818beba1da4adab7ab898c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Profiler ab1f415cb698fe95d32685db79888e8641b93caf02cb6bf06ef319ba6549510d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Stats 80f1a6c5e652871d09d7df6c916d62bb685cc1e451daebba6107dcbed94e2571 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Text 303775f867c32e5791cffdcd212f23fb654fd8034d1d174a5cb087347192d186 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Wireframe 0b8da2aa4d32de5b6b5edccaa47e0b7dbee092960f6f40067e915b2c289df20f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.All af305deb2c916b737272d518ba52357b0bcf0647cae193eaf6f094d44022fa7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.Bindings 316df6c50949739e3f3d985f8a534ffe2788f703c62ee6d6beeae942cc0cba59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.IndexBuffer 8bdadcae635fbdcced973362b6536c73463b5966ed0039805807efbbc21ad456 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.InstanceData e33cc55604fb60c6248c309bce5b4a3725666a4bb4648799be0c849703ef4cac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.None 16f60a85e9ae8b3dce3478f46f898f493d177a170d3c3ea7ff4e16cf5684937f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.State b6befc34ed9f16b2ef5e5501d2679f7ff311e25c133b3674ed220072be619ee4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.Transform 3f68b21afd7fd99ac6ac9ca418c4d5729b085f5e36d8b81306437ff644f15166 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.VertexStreams 47a8118f123f57db5d199e8c3dc1f3051763acb649050c024fc11ccc5251a7d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx 96546596c287b1ace30c93454b55f9ef7b339270da9c1984d461c8b4647392e0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx 530fe00296e11bb9cff2e355c64c806812c8879cdfa2598611725b239f7330d6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin 51e2efaa9afb68e2872a57268a6273982d65c6fb86fe5c6d4c72e2332959de45 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd 772bdf9572237b6536f8ec05d8f139d1093293acd41861fd44a66121e8240686 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.Count 9333bec29cb3cfdcb30b236eaa7be070682d7ffd52185823f73c5bbc80aef1de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.DebugCheck 40f56a28fad72595e91d81a2dff0ec30b80da83743b2c9035f80e7571a8d9121 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.DeviceLost 907ca1a9b2e9379ad5134a6086dc9cfaa3a20a897e541040b20ff81198c8eac8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.InvalidShader 73b8d24d442d26c61e0f19afd8bab0d7bc860eafe46b814332497da59d7c9a50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.UnableToCreateTexture c72d0df5df1afb0586e49aee5bf75aa5767abac05a43cddd6fda06a36d9c4ef8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.UnableToInitialize eb71be931fdb4998748e09ccd69552e73e6c1bea30f2cf25bd0421d747f867ca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx 5bae03d21976dd700e1ecab1b2da33b72dc4b9260178dc360eafc6aadb8a5fd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.DebugCapture 8909f6cec9ec64430b11feaf2a01689d8fd471d7121d8d8fc4ed5b206f6b96d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.Discard 78a1e3e57040ff0fc29cb0e9283a3c28fd02c7f74ccf224bf30638aa094c2f6c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.Flush 6bead72177dc7a457dd687e8b9db95e8b2580dba67b1796000c55565b031e50e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.None e19600a4a626c216a5bec920e0fbc0868b1de9bf5a54b5de0719c8d1c8883ae5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Alloc 1b98defaabae4cb48a8170657cd99fe3f05ce6477450f4fa2f25aae85def01ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocInstanceDataBuffer 0d80f9662e2e57620ee7ea0e1cd9fbe3a0e82e1e2be1c06a145f7ef7a3a3d2dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransform d4e99b631fda8409a9e1236a17355882fadd82b1aa3a59f1c563525a0cb8e75b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientBuffers 49d654f19b55a8a7b0da32de5627d7e0bee42a650c1990f664c77313dd9632ef -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientIndexBuffer 63f4b65bfe2c9f9308797e812c66777a7133ccb2e27680dd3d31546770d39287 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientVertexBuffer bbec0776f74323d2b2d0fe30be97917686abda90eb38558d6867021f24dc72b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AttachmentInit 048c4975dc2fc7337e48322e9a82c28e8da8303fe46815df45270fdfe463c3a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Blit f076b24c97eedd5edda262013b1888d468c57def867b55e56f50ad7047fd5bac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CalcTextureSize 4da46723055ae9b4c2b270ed7c185411d7454b0f969cc1ab734428a7fba72978 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Copy ae6c351bb78763238a17d8046b2774bfccdefed6d0e0d003317c9609aeb4ef49 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Count 86fc5502bf30f6bbbdf8ceb1c339e8e8701715498a0a54710fb156b2faf2c46a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateComputeProgram f7e65cb98bb010fcb4ad43a517548aba92fa68cbf2be68038b66e9190654d2a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBuffer fd2f52dd2db68c29b22e2e56f15b1eac32a47dd6f7260e6d9e28b39778b33420 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBufferMem f815149972c2a9ac3cdaf3184960beae8ce6c35cda493691886320f6fcfdd3a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBuffer 73610e15d3ddf6938ab7a12cda8ce4f57ceb5ef32334c7fb322b1bbbba94ce0d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBufferMem 6bae9fc4fd116a3ebb0d8830858b24a2559974e82e3b94d287a1cc4cc6116a67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBuffer 6380e82944bf2b99a2aabc1b53d8c52f32371904932033ad88d935d6301ba9df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromAttachment c811b8abda7c89a8f5546de2c803b5b279041a1df884bac9ee9c2bfa56cd91f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromHandles 99b2d27b4ec79fa5d190f431be3f3ae13e30d70e8c284b0c59406f5af7e93f5a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromNwh c29bb93c37ef3235a859ce5cdfd17ba5c5f2fd0349b2ba154b27ed7aa435fe98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferScaled b3995adb7d797dc91d43e71e4bf149d6ae06abe2df6acf409e605b3d9dc2a657 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateIndexBuffer 16119b5f64396aa43f639eb3c6dfee9e57e45e3354f4560aed0e74a4cc4a7d94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateIndirectBuffer 109df236c503bff4d6f83d6aef9442d3fc4fdee0f25705ee8076c4e80eeae424 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateOcclusionQuery 295d18b89ece23e3de16658d5e6dacbcea89207c724c3221af4bf8e84a538e16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateProgram 302306beac5e94263bc1a9a586e87272d9cbb6960d31543e9d10053f2b81433e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateShader 09184394f3e6cc381f9e724269137a5b4f139177534caf7f094e08361c38865b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture 423685972e236e7b179ae4c4106995df4bcb010c4c94cdd4769b59b297404ae7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2d dca33dc757f74121f00be9c650c1fcd103787ac9c80745fadc8c7499fd048466 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2dScaled 19a0cadf73e943f7f549bbcb4d2f70b2445497f749efb5bc04ade5b8814968fd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture3d 27ee5bf470b7fc875d5d7142a19b94acf02c13d1d7c06f518685c3cb388bab56 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTextureCube 9abfa76a4b5171653cab76ab49bdd601f3d7c79f319b2a8283742ed921689424 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateUniform bc43bd050044fda3ff6a00514b9c8c8255fdd8f90c914eef8edfe39598c272e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateUniformWithFreq 175dce3315553ccee7f21093e57712f805e67f1283609fb5c644173369586b84 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexBuffer fa156f943b3b20acf222e9e61ebcedcff8031e0c0911caca74eb77529e99b19c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexLayout 2eb348c8a3326407a87017fc8ca43bf49ff50108236481ef2c41e6ed42349ce2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextClear 1f1d6bf88b04d32781e52e7a4dc38425458e8e5f6796ca28b284c3b3722a1c73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextImage a5f9209de7b12b445bd032e79f6e7dd184d42b53ce72394f3cf84c511a8f6eca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextPrintf 8ab3d1cd851895c7a416b108771490d93c6aec43ea2cf3bee9f6fd7bb35098a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextVprintf 7dc3743b17201dbdd8315383af2c7c876e825b9a4c52b761b52f5d802400c486 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicIndexBuffer 40187af551a8843b9c4640dbc77dfb00f13e07d50cf405af20b0067691188f28 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicVertexBuffer 7348961ac032ebd317507afab66f0ab761eee042961fa9e4276ca96e4b91ec7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyFrameBuffer 7e9dc273f245d5057e0c2f00d53b686527838d2f153238efe4674f6c64b0e716 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndexBuffer 4fc19fa4976003ef416ddd4aa069f30319e132d9914121f42f4e37cf8b168087 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndirectBuffer 6ee6b123b29f6fa398da44056968e8ba750a3dab9f5a9bb1fc8bee08e5b10dbd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyOcclusionQuery 36a696d2327aa8c98297ca26b3a1fb90142b7ec4844e6623b20946bf33242bb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyProgram d1f2a896d2eb55c5399f7d5f4f14af83a58fa3e704b7e152dd220d471143a10a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyShader bbfb69a1a8539ad150faae6e922c19fb5e86bfe0bbc3a1f60666e2b002a2111b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyTexture 79c78aa41445086b509dd83a8972f872f01159b3204af9ac38c889a97a271b72 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyUniform 0ec9febfdb38202482790088dd14fef7135a0c316508893df3edfb0e2ee4160f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexBuffer eb1e7a391dd10b1ff35666e9628e3818ad7412ec7cea36ebe4b2df5cb4c1637c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexLayout 593d4d79f80d3d98ebe3d7af53f2e3f89fa9e36519981e38bd395570135a9201 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Discard 561f7f70eaee90d6bfe11a29b5ae5b8f8d757dea5825061035f3d5828c93b6bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Dispatch dd113c3c56aea26cc6583d456db9dda1f65967369f1d5a9e4c6adc82104fcf2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DispatchIndirect c99ed018f8976dfdc2661d6494655802f3a45ceabc9967a7c9cf7751d5090317 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderAllocTransform 838279d53039732f580be9b8069a3216b367832f8ea40612eb4be5e14cd18696 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderBegin 7ced6d64890bfe94d00f3fd69e8afd69f4773736cf337536909f7c4b3f81a716 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderBlit 5d465fa76fe896e9f352e57307f2dafbca11862ffeaddcc8b58417d20cae4320 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDiscard 3157b8749e754900113b35b1dc8a9d0817136bef6fa6b3dc54bdfb7893e3af9c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatch 913b4f25250a9cfc13bb5ade3c5ea67c1c8ffed96a8872cbbf3140e332f4cbd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatchIndirect fcd0a147785d9a13033e0269eb7a24b2f3a95f5324eaebbf847a1f420e1770ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderEnd 35b551d7ddb3c24f334a13f462fa95b26250ba82edd5d793935d0bf890a05a98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicIndexBuffer a373e2cbdfc7e5b390dfcfb39e4aee086835ecdc29398a8033822509ad41ecb7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicVertexBuffer 0563f12ea6d28bb0859dae05480c77b03be59c3806044f5eec1a707b36d51350 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndexBuffer 711ff52050139876971192dbdc6b03cb81d0ba6b0100a05258a517fd0f12459c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndirectBuffer 4d664e112a778d389d63db642d5c17c456b8b11d2aa29ffe7fef0a9f3689454c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeVertexBuffer deeb50ca76ca411c3d8d153226c49ed603c72f1283f2299b6517f18d1434dc15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetCondition f808818835532e385e97ee64f6b2673652fd49bdae90a86cbe2aa5c5db579409 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicIndexBuffer 418710afbda728083cdc6fea5c948fb276ed71d071dd60fab3ac569d529d9f7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBuffer 5d763d8818464a603aabd7ca259af3e270d85c04d769f0eaaacb242fafcb1779 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBufferWithLayout 576d85cabd7f61dd5966b0787aab75b7b1beba6d3a2d9e30e069812041b68c06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetImage 5bbb33c7946e24b39d9a6459e3e62f628ff842591eff098d29e168fca5fd645e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetIndexBuffer 5ae6666aafea2844fd80a44a34f1789cf480768c59a0b10d7e0b13419e999f7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceCount a470105e22a7919bdd8633a5ec75f9c2f93e4e23eacc46bc857287be2d67c709 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataBuffer 03215ae6721304a99bca300bad0aff5c9d44ef0daa5f3aef19ace5daecffdd56 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromDynamicVertexBuffer 991883543ba1408de2f311a30768bd7638363716a276047027bd0b77709acf1a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromVertexBuffer 626783fbaf2e8b2eba987ba8d4cbe61f447c0e0d7524f5222db96c7d4fdb92e0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetMarker ac856f0b6c69436bfa9ccfa31e86f65ecca6b67beb04bbee8054b76e15947a31 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissor 5f3c3b0554b5ad4f3a2abf7d5609a80c21e77ec1381ae62ca18de9e5007494ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissorCached c7b39b4fc1267329a0c4b6d9e34892621a462ac816bcaa1c1f8feebf13691dc3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetState 98836579c8ff44cc7ed773793e3e86d218dda085acb657faaf45081d6afc77c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetStencil e17da68488c73b45cd17ba79f4a6cdc769bda31544c397a34bb59f2f6788e165 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTexture 759374fc99df5f15efed192ff21ed6c31d5c7bf13bf29b39160988c35615fe52 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransform cc591bafbdc6a0235bc09385dd8d2dd45b63575df67834c9a53d0dfc39bbf076 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransformCached 2d8abe865332c47dc2c8ae50497ee355e273d9640f32d5ff0398e14081be2f66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientIndexBuffer 337ac10d65601d14379a77ed48cf2825cd346e725eee485e26d07eb5f7e9fbec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBuffer e546888f09dd17bc17d1ef9e072142adf0306326e355a2ffd10caf03eb7154a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBufferWithLayout 5b533b5b57cf46012508485b87b58cffbd2c51d6d79c7957ca4164d75b35048f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetUniform 2eb5cef0809f5f943f7bace9822464cdbeefb436ffd877e8fa36b33b53fa1728 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBuffer 80fdbf61ffa83ffc632906ecc2b705d55df6b5582009c0dda6a0a915408fe91e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBufferWithLayout a5152f8e4351ddc86a2fc31bd74ae485e6960495fecd557787c0d14fd6f95a67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexCount 9ce855ad84219817f85f7cc9d6148fd2fcf13d678d0a9086cacbed10eb4e3444 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmit c39c5cdaa0963284e627ae0c20c92f420c5e5ed6dccc1a5a6c83a89f3b76bc63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirect 89fe6af1577337751ebffb53f7d8dc2c4b3e803b95c82a8193a54ce7fc2bede6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirectCount 9625a47bcb590bb685c3d560ed0018eee6e0f6058b5a5dbc3a26a9a0d8d3a2c3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitOcclusionQuery 9b225e89ec1d2d0b74e640f3d3b7d32c07bda41ef656ce07b65eeeeb2cfcd51c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderTouch ade34a7f3e714bc4f02b2d9041e0f423dd58892c2d49f656121a84c268fe3d78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Frame d4a6524fbfbf33174aa0802ed9aaf63647e127244fdac4831af3adda787120cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailInstanceDataBuffer 9ead60a9830d0ba1799c545573556c624c4f09ad8eb6b05816174c6bb6647485 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientIndexBuffer 9958c32665e3993ca8d7ef2d6430ddc6c9bd6a5327c6095127c8cffbdc6f88c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientVertexBuffer 5e462dfa2fc1f0bd36f437c299c242001154d63e5f5a3b9e2d35e03c69e0e3c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetCaps 26a7b591a35f067db3666f3ec2e63c250d0ba827a9ee78c128b398bee54d6df9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetDirectAccessPtr 1039fdbec557d90c0d91ddec098eb1880881b9231a216c6a9274d21aa181d107 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetInternalData c8a790cd219b73c009707e2fbdcf97e9b993a71fe94075ab445b471a37fa95e4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetRendererName 7a4ad97ad0d9996d55d4fd5975bfc858efaaf5f081d8c8c80a36fa1cab3e19cb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetRendererType caf617c5a597083a65d6d5832caf45587859fcbff0edfd586b948c8e1e62e863 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetResult fff9dcb2c33e16ff52085d994231dc677e919a5ba15f95c90383483fcd30bd5c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetShaderUniforms 2a98738722ff461c5895377b49f850c29debbe9f4946aafe76256543c9c68223 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetStats 740c2dd559d39adb22853c96f7af3f6e88d8e028399f59e50e054edbdf2f31ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetSupportedRenderers 250c000d66ae978bdb030c82b010a2b24e9582f67f409b9f15b0f815f89db971 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetTexture 50da90495c674449b1780ca5f0aa37bfdec5b6b10afffb4a5f1bbba72b639ca0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetUniformInfo 60fdb5f0a1fc03218f41656598bceb5d7dd533b1dadfe1f133436b2f9aa05431 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Init ba1e186ed307bca5d77a593bcaf758057e80eb04c2b7c658ee5b2ebba6ccea67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.InitCtor 848a3e3a575303824f9242eba014a07319cc6e421627843a6cd88e99cb38f4dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.IsFrameBufferValid 111a23fade94e0eb807b065d8fa61f5fd3685bff77dcf2781452f9c0786d92e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.IsTextureValid 80b8a620847bccf07ee6666d1224a37dc1d3ef49ade985b7e0b7aa7e534fd80c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.MakeRef c787b008c06a5a1285dfe816f2c4f39d23cee42819f7fed92ea2cd76c72e1081 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.MakeRefRelease 9f1f04f00f153fa33a54f75505dc75bbe0351396ddb3fb475233f4b4dcf41616 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexture aaed258dd8ab340215bafaec006079a165f1cadd9c954fe6ca54cfeb7d6a11c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexturePtr 926bd87fcfd397bc1aeca1fc8bdd547b862fbfbca9ce0e301cd8e5c92bf19831 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.ReadTexture 7b16ac715ed651afd7202d56ce9e383c8cc92c2164fe6043809dc8fd902b1c54 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.RenderFrame 1733e6bc7994f2d584a65fc37970432d89d7a04842a43a7cc461df6ab5b47739 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.RequestScreenShot 75c9f797b0de0407f2d948635d5fe04f12b7e5fdcfc0202c791d664797d87595 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Reset ef1c520c29279a7e708c9d3caff920ba29248d1a2e529ebd187ff2d5d478ef45 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.ResetView ba5cbdf2bfbc78b082f4f6e038e521df752ba26370003a8584a48e6fe04e0b10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicIndexBuffer bb0a5af9f7b81ba00e7a1ebd4533d8e423816547ca0ddaff5b2d388b607eafa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicVertexBuffer 562791e1ba62cb9b3c4bbf50722e96da7baf8a94f755731e9f012dfbf43af807 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndexBuffer 63dece3bd700a00eeb7e590b25215182718b69acfb9f8ddda73501f2151c9f9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndirectBuffer dcb7d120aec62d93705b480b19bc4f1ab360119a8ed02aa1da4b566df276e53f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeVertexBuffer 50293cea470eab1cf362dcd0816a5ef7c1ff6f45b0fa73cdd9d0a1d5acd3b845 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetCondition 14422c640b37688664c33d30fb9775b514a1fa35543e01ccad5f95bfef564a6c -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-b62c66249e25b0dd"></a>[`Inno.Native.Bgfx.bgfx.Access.Count = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4258) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-978c23ff4e03f669"></a>[`Inno.Native.Bgfx.bgfx.Access.Read = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4255) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93970281ade43b49"></a>[`Inno.Native.Bgfx.bgfx.Access.ReadWrite = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4257) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e654b97e9f88a0d"></a>[`Inno.Native.Bgfx.bgfx.Access.Write = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4256) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9af84d37db2c4edd"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorVtbl* Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4275) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b9c20ec37d6c5041"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.AllocatorInterface*, void*, ulong, ulong, sbyte*, uint, void*> Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4299) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ca56734cc6dcac55"></a>[`public Inno.Native.Bgfx.bgfx.Access Inno.Native.Bgfx.bgfx.Attachment.access`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4323) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1148b21562ec97aa"></a>[`public Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.Attachment.handle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4325) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad14c64cd6fac96c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.layer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4329) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a11dbfc3fcbcb23"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.mip`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4327) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d873aeec6fa9a569"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.numLayers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4331) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f175347be0a50ec2"></a>[`public byte Inno.Native.Bgfx.bgfx.Attachment.resolve`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4333) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f3b58444579011b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Bitangent = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4357) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83cc69b64191f061"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color0 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4358) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef1b9b473978fc66"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color1 = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4359) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-71914f44c802e31d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color2 = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4360) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c19f9c0d38668589"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color3 = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4361) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f78ad44d0d995c5f"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Count = 18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4372) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dedda80f71a7c53c"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Indices = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4362) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3050f8cea02ab541"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Normal = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4355) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-25edc5225dd95950"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Position = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4354) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b95ff9cd2601e91"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Tangent = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4356) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9be37662c648364b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord0 = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4364) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b5248d0062dd4fda"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord1 = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4365) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2e0dd75b746e2bf"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord2 = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4366) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80c6003126e86973"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord3 = 13`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6404af589ea696cd"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord4 = 14`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4368) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-234ad6f63cbe637d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord5 = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-231e702c881a9a3b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord6 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4370) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b45dd98d310d6fdc"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord7 = 17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4371) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-152e091f7bde2453"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Weight = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4363) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3c271624c59b3c0"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Count = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4393) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3955ddb4efb4fd83"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Float = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4392) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3999bfa16c8207e6"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Half = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4391) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-180b8c2651880ab5"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int16 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4389) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-585bd9a6f2b531a2"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int8 = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4386) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c80a9880f92726f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint10 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4388) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7efed06fad7f93f9"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint16 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4390) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-322e43a871605c2f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint8 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4387) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c8f4fcfa05a9c76"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Count = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4413) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a948e60552e1491e"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Double = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4412) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-70efe07325dfaba4"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Eighth = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4410) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ecef3ce8de1fb1b"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Equal = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4407) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf74ebfa416732e9"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Half = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4408) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e181eef723c57c64"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Quarter = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4409) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fad8b69008226e79"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Sixteenth = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4411) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d16ff06e47e6bfb0"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.AllowResize = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4448) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-35524ceb14ef725c"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4431) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f100aa4a0a2cf2a"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x2 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4432) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1f02876e33b59889"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4433) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0dee05c3761245cc"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x1 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4434) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6f89f2d378e029c1"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4435) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a558dcea60f402b5"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4436) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e531f64700675d48"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4428) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a84a0c7acddfd5d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4429) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fab09fc8a824a9c9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4430) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad55712fc49fd9a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4438) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-72d65506ec1bf622"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatShift = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4437) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8644fe2002d3bb60"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4445) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13b20fa6c0a26d52"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeReadWrite = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4450) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88ca145caba0ef2d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeFloat = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4441) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc887928149ca9a9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4439) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f414028f82735d2f"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4443) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad82f9f91b4f00c4"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeShift = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4442) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3cda6dc3c68cb37b"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4440) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-95324ba48e71a2a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4446) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef5ee455c7d332f9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.DrawIndirect = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4447) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e4675979e6df7cf"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.Index32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4449) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d743ad3cdd347b86"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4444) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ff34dd77421c952"></a>[`public Inno.Native.Bgfx.bgfx.CallbackVtbl* Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4467) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d73e6fc6d2063631"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, ulong, void*, uint, byte> Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4503) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fe3bca2f9e213d5"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, ulong, uint> Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read_size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4501) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4f330c2e702cfe82"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, ulong, void*, uint, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_write`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4505) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-758c86e5a291947e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, uint, uint, uint, Inno.Native.Bgfx.bgfx.TextureFormat, byte, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4509) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ec5437498d66d56"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4511) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-94d832cfdc5c7b31"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, void*, uint, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_frame`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4513) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-052aa9071b6842ec"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, sbyte*, ushort, Inno.Native.Bgfx.bgfx.Fatal, sbyte*, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.fatal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4491) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cc4608b49c2581d2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, sbyte*, uint, sbyte*, ushort, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4495) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2217aab42a703df4"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, sbyte*, uint, sbyte*, ushort, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin_literal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4497) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9c3c692326d4c98"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4499) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63e6d7ec64a626b7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, sbyte*, uint, uint, uint, Inno.Native.Bgfx.bgfx.TextureFormat, void*, uint, byte, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.screen_shot`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4507) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9780e0e42697f23f"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.Bgfx.bgfx.CallbackInterface*, sbyte*, ushort, sbyte*, sbyte*, void> Inno.Native.Bgfx.bgfx.CallbackVtbl.trace_vargs`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4493) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c296b29cbd5cc2ac"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.deviceId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4543) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1165739e44962b9"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_538958e7fdcac864 Inno.Native.Bgfx.bgfx.Caps.formats`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4555) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3fcc00f7047a504"></a>[`public Inno.Native.Bgfx.bgfx.NativeArray_60f03ada28b9ab74 Inno.Native.Bgfx.bgfx.Caps.gpu`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4551) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-27ddcc19c30a412e"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.homogeneousDepth`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4545) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-01ba6930ccaec984"></a>[`public Inno.Native.Bgfx.bgfx.CapsLimits Inno.Native.Bgfx.bgfx.Caps.limits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4553) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e82b89f0a6c5f5c"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.numGPUs`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4549) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-07c40e3168520a17"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.originBottomLeft`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4547) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38984fcead5c385f"></a>[`public Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.Caps.rendererType`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4537) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da3f042458bd7b2d"></a>[`public ulong Inno.Native.Bgfx.bgfx.Caps.supported`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4539) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8bd06ce1833cfa0c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.vendorId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4541) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e63ea4ef16117617"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.AlphaToCoverage = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4577) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-401a8755f10cbe93"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.BlendIndependent = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4578) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2de09d22b20d1f98"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Compute = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4579) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a0a336438a36f6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ConservativeRaster = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4580) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b4055a1d16076214"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirect = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4581) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e57030e7f118eb21"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirectCount = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4582) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-312245001b7e1e69"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentDepth = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4583) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef3ede883dd70a50"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentOrdering = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4584) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-538e864d124ff618"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.GraphicsDebugger = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4585) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c92174cf69172cf"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hdr10 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4586) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b5801bf1936d4ac"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hidpi = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4587) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-35516eaa769e8cce"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ImageRw = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4588) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb16ca8edd5052a1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Index32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4589) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e38858d52d69835a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Instancing = 8192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4590) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9367d7bd208d7b1e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.OcclusionQuery = 16384`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4591) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5247949a958e3906"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.PrimitiveId = 32768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4592) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2f7bfd3aebf66c43"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.RendererMultithreaded = 65536`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4593) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf73b1f637ab3738"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.SwapChain = 131072`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4594) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-706d2f8ec1db53d1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture2dArray = 67108864`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4603) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8f62f47327b0354a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture3d = 134217728`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4604) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-196fa754a77a7caa"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureBlit = 262144`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4595) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-294ebbd947b1b25d"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareAll = Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal &#124; Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4611) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1e82e7b3fb41cc4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal = 524288`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4596) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e2160773f01ecae6"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved = 1048576`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4597) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a82e77236bb3e5d0"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCubeArray = 2097152`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4598) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb72b041fd9e2557"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureDirectAccess = 4194304`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4599) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73b506e4f66be42b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternal = 8388608`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4600) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b7136d5e848189b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternalShared = 16777216`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4601) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9446f753d8219dc7"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureReadBack = 33554432`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4602) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2f3aea4aec88d6d9"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TransparentBackbuffer = 268435456`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4605) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5de1c36a13800854"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VariableRateShading = 536870912`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4606) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bcf276f753113ded"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribHalf = 1073741824`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4607) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e8e49bb12536d63e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribUint10 = 2147483648`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4608) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf2e01443711a55b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexId = 4294967296`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4609) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c2c500bda81fff4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ViewportLayerArray = 8589934592`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4610) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3da49be5c5a306b2"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2d = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4627) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f772621033f14a12"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dEmulated = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4629) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e6165e90c161f2b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dSrgb = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4628) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ed19df705f4336e"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3d = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4630) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-15194169bbdfc439"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dEmulated = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4632) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9e3e338aa0f2ae46"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dSrgb = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4631) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-931a471c01b6dc6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureBackbuffer = 65536`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4643) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-75eb501f1d6d17be"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCube = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4633) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0ebbbb76dfced243"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeEmulated = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4635) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-827599bfc1d2077f"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeSrgb = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4634) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c2b24998e3199ba"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebuffer = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4639) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1bf8e8ce118c408a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebufferMsaa = 8192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4640) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2f30408ed6c8167"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageRead = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4637) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e4f05f9771829f0c"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageWrite = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4638) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-68b0c661a4685a34"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMipAutogen = 32768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4642) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-05cbfd2ae7bd783b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMsaa = 16384`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4641) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ba57c1f03f49bedd"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureNone = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4626) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0551948e143f5905"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureVertex = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4636) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5837532d8759bdaa"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.deviceId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4662) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-969d9f770cf15076"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.vendorId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4660) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-65e3f1bf15576517"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxBlits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4688) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-39eb0aaa9466a700"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxComputeBindings`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4708) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-70cc05a83cc666ea"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDrawCalls`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4686) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-933783d3bfe9bfb5"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4718) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-96297dad7347d9a4"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4720) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa950218380585c9"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxEncoders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4726) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5db62de07f47d7ad"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFBAttachments`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4698) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-93aa81e57e218929"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFrameBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4696) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cacce2bcfaa1ed36"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4714) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-51ef725397f6caa3"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxOcclusionQueries`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4724) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af03219246ee4a5c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxPrograms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4700) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3a86d75eeafa0711"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxShaders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4702) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-922e67fb588c721c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureLayers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4692) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-82239e7760c0e4ba"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSamplers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4706) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc102356bb523bab"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4690) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-44b8bfc0aad4819b"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextures`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4704) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a960d6261e8a09be"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientIbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4732) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-52b394fe124f3846"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientVbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4730) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-973474f577357a8d"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxUniforms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4722) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-736621a2335a3f40"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4716) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9cbee65c09c6e4d8"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexLayouts`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4710) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1c048618e05b00c1"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexStreams`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4712) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-194931c571755652"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxViews`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4694) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-90dacdb5313092ff"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minResourceCbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4728) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c77e99054396b1a0"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minUniformBufferSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4734) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55683955805566e2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Color = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4757) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-81fa711b2c7eda24"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Depth = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4758) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c0a3651e5629d25"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4760) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5cea43178a812f5c"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4761) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5bd465ec83a0df99"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4762) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2c78911dccf14ef2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4763) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-74bd2f698cca607f"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4764) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77d52fc8b49e1853"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4765) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23c202c7cc85e35a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4766) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7df2160d2166599a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7 = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4767) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-71a273235ece6fd0"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4770) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fda9a32fe3f8ccb4"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4768) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-858ce7d5b3105b6d"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4771) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7b4b210e6c342bf"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4769) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f489cdc9c03d2a86"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4756) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df13a9926895b371"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Stencil = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4759) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1337eb2c8c2fad40"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERALLOWRESIZE = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L150) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4f30c73c0fc6b978"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X1 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L163) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1ed4c5e6d58ea4d"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X2 = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L130) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63137ee13e3c91e1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT16X4 = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L196) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bdf38459cce8ed15"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X1 = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L137) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce728e078d5d1eb1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X2 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L202) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8da1c23e21664412"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT32X4 = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L122) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e8b42749f3f37d5"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X1 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L152) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1b836450266675f"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X2 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L180) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9bf12edf6542db7c"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMAT8X4 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L188) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-932e40edbe25a305"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMATMASK = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L161) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1f69650a9e242319"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEFORMATSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L183) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2781f50e902a3cdd"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEREAD = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L128) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3632a94ee256f8cb"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEREADWRITE = 768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L170) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-52913ab22d8f2b91"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEFLOAT = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L209) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-63785b3a597818d1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEINT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L158) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7612303637930093"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEMASK = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L133) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a6a7e6cba14e583"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPESHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L148) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56af63b0dcd25dfb"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTETYPEUINT = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L172) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d02ceffae18548d1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERCOMPUTEWRITE = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L213) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e3cdcd78e52e3a28"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERDRAWINDIRECT = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L147) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bc7c2a4b98b79da1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERINDEX32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L156) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9cb173459ae229df"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXBUFFERNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L135) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-96272e5ca8c16816"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARCOLOR = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L189) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d47d5a327ed0dce3"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDEPTH = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L149) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b095e021d376d44"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR0 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L175) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a5e54c9e7791f461"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR1 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L127) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bcb78cb56eee89a5"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR2 = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L185) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8038c27ccf662db8"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR3 = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L171) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-27b8930e3517f17c"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR4 = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L212) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50064801a22410c7"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR5 = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L195) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a1aac530f3d29a92"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR6 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L206) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2477a1c6355453c8"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLOR7 = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L199) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf676cda21423130"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDCOLORMASK = 2040`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L168) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d163ab4ac762a31b"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDDEPTH = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L204) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c076b752294688ab"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDMASK = 8184`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L184) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f78321d8b6b0b6b"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARDISCARDSTENCIL = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L145) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-695b8ac9e514d2a7"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L201) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3209b994ace039fe"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCLEARSTENCIL = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L211) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-82e54fe2f86ba01d"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEX = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L124) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b913a3e348529b46"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEY = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L191) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c6063a2b2252ed48"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPNEGATIVEZ = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L176) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e6ce65fbb53bb2c"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEX = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L164) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3042a53216e13ea4"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEY = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L155) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ae378bb8864ea8c1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXCUBEMAPPOSITIVEZ = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L181) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-114696bf7d88ec26"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDALL = 255`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L194) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a93ccacc466ad415"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDBINDINGS = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L129) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5343b712648f6c1a"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDINDEXBUFFER = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L174) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1dd90b735ce255d"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDINSTANCEDATA = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L139) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f080657f11eba634"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L134) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d4b786065f16e474"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDSTATE = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L126) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7110f1ba60482e19"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDTRANSFORM = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L153) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1ca062d9b1848ad4"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXDISCARDVERTEXSTREAMS = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L120) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e6b4c3371537886d"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEDEBUGCAPTURE = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L177) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ab756ce89d77440"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEDISCARD = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L165) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df57fba5466ba17c"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMEFLUSH = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L192) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7db4dc69fec4dabe"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXFRAMENONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L173) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-54daaa071477e75e"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDAMD = 4098`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L178) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1f94dce4329eb59"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDAPPLE = 4203`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L146) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d584e7ae81838c71"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDARM = 5045`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L179) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a668ead7d352b789"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDINTEL = 32902`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L169) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fe5f63bf1bed1ef0"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDMICROSOFT = 5140`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L142) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d7eb5573da2abfe"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDNONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L208) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f8246dcfcfc2d34"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDNVIDIA = 4318`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L117) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64f530f9cc0715ec"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXPCIIDSOFTWARERASTERIZER = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L125) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-359f0c54abbbd375"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXRESETFULLSCREENSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L121) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38d083fc1f29e5c1"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXRESETMSAASHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L205) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3d0b32a84c7c1e50"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXRESETRESERVEDSHIFT = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L118) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6fc8591649b6e4a4"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXRESOLVEAUTOGENMIPS = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L119) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ca00b0487b33b4e"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXRESOLVENONE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2b8051b40322b28e"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERBORDERCOLORSHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L154) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-208f2cbf3278a513"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERCOMPARESHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L131) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8991625e62bd2f6"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMAGSHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L190) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ef5ad2d8236739c7"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMINSHIFT = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L123) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af2e0b6b67ca58ea"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERMIPSHIFT = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L198) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fdc326fedd398348"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERRESERVEDSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L140) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2288a9485028defa"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERUSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L157) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4653185da6c7ebab"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERVSHIFT = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L200) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-973ad1e62381f028"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSAMPLERWSHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L138) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7574495e22d06179"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSHAREDLIBBUILD = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L193) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-038a15ca4eb53b44"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSHAREDLIBUSE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L197) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9361535ccd78fe4a"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEALPHAREFSHIFT = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L187) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b8640c0fd4eb6b39"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEBLENDEQUATIONSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L182) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55d28acb2711681e"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEBLENDSHIFT = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L144) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b1ec8b04b13e5ffe"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATECULLSHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L167) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38d4b5584d698559"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEDEPTHTESTSHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L203) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-249c4821eeec8cfe"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEPOINTSIZESHIFT = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L162) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-753dd4001ea8075f"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATEPTSHIFT = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L166) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13ab3784f3d165dd"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTATERESERVEDSHIFT = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-acfdc468daa80df8"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILFUNCREFSHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L207) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d7f332d681d79fe9"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILFUNCRMASKSHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L136) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e76c1d9d44507c5f"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPFAILSSHIFT = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L159) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dea628cd85f97932"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPFAILZSHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L132) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa93ff62fabf164c"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILOPPASSZSHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L143) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9de5be7e83acc359"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXSTENCILTESTSHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L151) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1abef6b27ce75ef"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXTEXTURERTMSAASHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L141) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-413202577a9afb2e"></a>[`public const int Inno.Native.Bgfx.bgfx.Constants.BGFXTEXTURERTSHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L186) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0bc5ae37abaadda2"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4787) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d348511dce75e85f"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeY = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4789) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bec211413355f27b"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeZ = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4791) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8f74dc88979c48c4"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveX = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4786) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d23c2f90aa612228"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4788) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5cefec28499bd4ff"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4790) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-685f04e0445b1dd6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Ifh = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4808) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d8ae087a15dfcb99"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4806) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50916d8add82e4d0"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Profiler = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4811) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d432868431fc190"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Stats = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4809) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-88d1da0621d16dfe"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Text = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4810) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cd9178b52165a1b6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Wireframe = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4807) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a62d451b83c8860a"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.All = 255`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4833) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8841f49063f259c3"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Bindings = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4827) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f918abeac53be327"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.IndexBuffer = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4828) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f26d559644d72567"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.InstanceData = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4829) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-10f2e3ab39580016"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4826) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-091a1a75aa26214b"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.State = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4830) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4e8db6bdaf7b3511"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Transform = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4831) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ab5bf5c555e4d16c"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.VertexStreams = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4832) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c89292187534ca3"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4850) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9ff345f95247caf5"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4881) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0cc480539714c685"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4926) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c290f76248752c9a"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4928) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e1c7be5f766534e2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.Count = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4954) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d86286188a268fa5"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DebugCheck = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4949) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c8d5e4f834222239"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DeviceLost = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4953) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c3cf46b00327cda2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.InvalidShader = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4950) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1d19e1f870ff975"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToCreateTexture = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4952) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec62e4affe72ad73"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToInitialize = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4951) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b152bc971c6863b"></a>[`public ushort Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L4971) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c43e6f1ea597cbe0"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.DebugCapture = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5001) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5a6922758581a816"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Discard = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5002) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ce5dd3173913b278"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Flush = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5003) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7791472875fccd60"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5000) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-85a895ffa6a5093f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Alloc = 26`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5043) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb969015e6940eb4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocInstanceDataBuffer = 57`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5074) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d3f590f8a4c4b49"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransform = 170`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5187) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-507c30efc94f5dec"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientBuffers = 56`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5073) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a709bc8bbbd7544c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientIndexBuffer = 54`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f05db85ee0df0cb3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientVertexBuffer = 55`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5072) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67750b9743fc7927"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AttachmentInit = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5017) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5883e5d81ca902b6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Blit = 201`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5218) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-033a8e815148ef7a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CalcTextureSize = 69`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5086) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9d9dac5946fa719"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Copy = 27`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5044) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a59a35dafff487ae"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Count = 202`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5219) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-deb35b30de1ffb76"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateComputeProgram = 65`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5082) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e38ef9ea4ac56565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBuffer = 43`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5060) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2cf9e85c14fa7395"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBufferMem = 44`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5061) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7378243eb84a1ca6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBuffer = 47`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5064) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4037f3a0e69ef8ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBufferMem = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5065) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9141babfb0a48504"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBuffer = 82`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5099) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ed772833357b885"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromAttachment = 85`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5102) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-55736e389a8543de"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromHandles = 84`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5101) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-047b35b246803fe0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromNwh = 86`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5103) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f7a24a9c48cc3e0b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferScaled = 83`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5100) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b5e2e51d8993426f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndexBuffer = 35`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5052) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4a0d5070e2aef459"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndirectBuffer = 58`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5075) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e5353f5a182bd08d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateOcclusionQuery = 94`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5111) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-67432dbb454409e4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateProgram = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5081) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-642ef1fd65fc4ff5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateShader = 60`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5077) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-815b5dfcf30d5172"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture = 70`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5087) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7885fba42e26ae14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2d = 71`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5088) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8dc5e587266c541f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2dScaled = 72`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5089) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bde6ddcd48a4c90b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture3d = 73`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5090) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-14420387b9f7fa09"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTextureCube = 74`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5091) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b9f95c9d8a52e13"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniform = 90`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5107) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-99cef14808b7a8d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniformWithFreq = 91`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5108) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6f2e1baf2f4b01ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexBuffer = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5057) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0be9d301ccf88065"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexLayout = 38`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5055) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7280f13dff5a97aa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextClear = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5048) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-36258c40c9c1a806"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextImage = 34`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5051) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-65d3b625fd3ea013"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextPrintf = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5049) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c6971919bf077fa2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextVprintf = 33`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5050) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-749f24806ff8d30a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicIndexBuffer = 46`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5063) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-13df7fb7a87febdc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicVertexBuffer = 50`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5067) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-03f738743c0c5cfe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyFrameBuffer = 89`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5106) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-85e9ce86ac819890"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndexBuffer = 37`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5054) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6685182d0c0ee5e8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndirectBuffer = 59`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5076) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9879c983dbc1a970"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyOcclusionQuery = 96`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5113) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e50f4b643561b3da"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyProgram = 66`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5083) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f24fe09a08d57ad"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyShader = 63`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5080) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e368b60ab5e08171"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyTexture = 81`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5098) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-09ecd371004e5f47"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyUniform = 93`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5110) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2a2c96c292a3259a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexBuffer = 42`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5059) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-34a899dd698472ca"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexLayout = 39`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5056) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eb05801310aaae45"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Discard = 200`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5217) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ee9e8cd8e18bb14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Dispatch = 198`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5215) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-780eb4f1274a8e18"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DispatchIndirect = 199`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5216) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-71c0cf0ddf10e8c7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderAllocTransform = 122`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5139) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c7371c9a38b0a63"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBegin = 112`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5129) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a300cd97efd22d2a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBlit = 155`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5172) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7cf672d21f42f32e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDiscard = 154`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5171) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-966e173e32ca68be"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatch = 152`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5169) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-25888a212ca46f53"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatchIndirect = 153`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5170) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5750c5f3cd4657fc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderEnd = 113`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5130) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b61fd7dda94a022"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicIndexBuffer = 148`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5165) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0d6fac852d8101a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicVertexBuffer = 149`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5166) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-db68b460687f7c6e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndexBuffer = 146`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5163) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4bf12d88c3532b6c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndirectBuffer = 150`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5167) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1acbd608d5435008"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeVertexBuffer = 147`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5164) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4c07b7fc8bb871e6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetCondition = 116`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5133) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bfda36cdd2d2b3b8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicIndexBuffer = 127`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5144) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1b5ff9bd1f77f286"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBuffer = 131`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5148) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-713367ef8a81ce14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBufferWithLayout = 132`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5149) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1458c391029bffcf"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetImage = 151`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5168) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2360fdadbfeda9d3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetIndexBuffer = 126`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5143) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1571c750bcefcf24"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceCount = 139`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5156) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d1c77e6ad2829c20"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataBuffer = 136`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5153) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec6c827052145565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromDynamicVertexBuffer = 138`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5155) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bf337dd0e43b7807"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromVertexBuffer = 137`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5154) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d57f7926986d7b7e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetMarker = 114`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5131) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b151ea647630f9e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissor = 118`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5135) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7eb6dc68ac2e5e61"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissorCached = 119`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5136) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-83949a8392834e56"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetState = 115`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5132) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a824258758a052d1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetStencil = 117`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5134) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ea79ddb2b509922"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTexture = 140`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5157) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df711de2e60a1d3e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransform = 120`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5137) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b230614d454ef471"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransformCached = 121`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5138) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77300260d257b46c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientIndexBuffer = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5145) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-af5fa62a951ea327"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBuffer = 133`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5150) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-744ecb59bf36dc6f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBufferWithLayout = 134`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5151) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-dbdeb64aeae63635"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetUniform = 123`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5140) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4490644ef94ca750"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBuffer = 129`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5146) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da79919fe8e389a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBufferWithLayout = 130`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5147) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6cbf73d7c97381ef"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexCount = 135`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5152) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5e2967fbb80ec47b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmit = 142`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5159) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3f4673865046f4fe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirect = 144`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5161) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a442c6f19f628d82"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirectCount = 145`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5162) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bd699dffd96d210b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitOcclusionQuery = 143`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f8d0e137480340a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderTouch = 141`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5158) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-219c90dd8a9e665b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Frame = 22`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5039) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0fa144662ec87291"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailInstanceDataBuffer = 53`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5070) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f1b2347f877369d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientIndexBuffer = 51`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5068) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-37596f0bccef83cd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientVertexBuffer = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5ed8d7007cb040b4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetCaps = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5041) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0331963a4cc501c2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetDirectAccessPtr = 80`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5097) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e50eff5f98f100a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetInternalData = 159`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5176) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e9d8644167d62655"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererName = 17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5034) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a334b4fd24fadd3b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererType = 23`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5040) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56d3524161975d83"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetResult = 95`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5112) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cb5f068742771e62"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetShaderUniforms = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5078) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6c426f93ba8a59b1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetStats = 25`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5042) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5d6091c2743280f1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetSupportedRenderers = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5033) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-80f97ddd94b88472"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetTexture = 88`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5105) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2420b5b51305f3fa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetUniformInfo = 92`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5109) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-81209b1c071d2aa7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Init = 19`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5036) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1f0d4ea55c3e7716"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.InitCtor = 18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5035) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e93f8a21c7547a4b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsFrameBufferValid = 68`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5085) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2e1d7ed7a7450cda"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsTextureValid = 67`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5084) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7d84423f94602183"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRef = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5045) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-262371678afd4d84"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRefRelease = 29`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5046) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa8afd6f88d7c6bc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexture = 161`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5178) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-012f8234d7ad82d2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexturePtr = 160`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5177) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c1f92897042ea64e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ReadTexture = 78`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5095) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1556e8a28c320faa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RenderFrame = 157`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5174) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0831b474e73a2601"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RequestScreenShot = 156`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5173) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3792a6514dd32b86"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Reset = 21`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5038) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0a4190fda702185a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ResetView = 111`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5128) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1118fd9eef9d9c57"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicIndexBuffer = 194`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5211) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-53169b881589ae6a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicVertexBuffer = 195`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5212) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed38c5b76e5563b5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndexBuffer = 192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5209) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7c52b4218158594b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndirectBuffer = 196`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5213) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-658d89676ba9d858"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeVertexBuffer = 193`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-eef3c277c746764f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetCondition = 164`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5181) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
