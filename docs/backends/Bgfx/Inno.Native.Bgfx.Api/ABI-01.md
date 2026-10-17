# Inno.Native.Bgfx ABI · bgfx → CapsGpu

[项目与生命周期](../Inno.Native.Bgfx.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Bgfx.bgfx</code> · 278 个声明</summary>

<!-- api: F:Inno.Native.Bgfx.bgfx._SHARED_LIB_BUILD ed97e778d8b735a944a990534695366d1c8d036d8d211b594a3568b78f18460a -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SHARED_LIB_USE fb1822640f4db55b176cb516656261cb1500436ba8504da5c73449d956d89cf7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_DEPTH_TEST_SHIFT 47ef119ac995131143cd7eb0dc5bb7dcc3b880e3c05101babbbb3ea946fec7c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_BLEND_SHIFT 6b3fcca61c28e87e3689624aff8b3b269fc7f31d9466c08f319018b653d46f8a -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_BLEND_EQUATION_SHIFT 9981c5cfeedf62de8a7def74a48f788509f9dcc8a9a1fda9406257ae378ff8dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_CULL_SHIFT ad4676b94cb838ef2be3f1ee352149c151eb70f6dd322bf61d127e0c2e279666 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_ALPHA_REF_SHIFT 6645ad34faf8f0d5d4957d23a7481db7c22e1b407910b3073feea48fbe9734d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_PT_SHIFT ee2e68e001a9a4d7c8e14bd204181b5ca74db2c21a38fb83fe78c5f86a901979 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_POINT_SIZE_SHIFT 4431b38b576dabada7a347d636eec90d5186a653c1802030f08f1cd2d28b955f -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STATE_RESERVED_SHIFT 6cfb48c73be3ab853bd3e5f07f6fc5b0fc3eb77c3e74e09905aeebbe1702285e -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_FUNC_REF_SHIFT 3a666e1d9b4dbd49a82e675e806b0c8820b043ef97a37609fde783560ac99bc7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_FUNC_RMASK_SHIFT bf9c4255237087faa05f8d331a47d3e8f33253fb91bb221c925ed8dea6874958 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_TEST_SHIFT b24a79bd483c57db8ef4e152c6fe67b9a4a5bbef563ac4cebf4812270e5296fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_S_SHIFT 3cc3f38a51e30ee565dfa9d06a69c2e218ff17e1dd47babcb3b08f6083dd90f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_Z_SHIFT 39e5592b966694a31138ac60306f01ac57104f7553dad60f3c860a90edcb44ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx._STENCIL_OP_PASS_Z_SHIFT 789bdb186d755f0c5d12d71ba8a12cddd4e667d0a3d6db0f25689eab9d529c7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_FORMAT_SHIFT 38be6e4ca341e2b40cbcba4e01f7753cbf8e48265e470f32642083b63a1cd554 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_TYPE_SHIFT 136e897995b94e7662965404194e4143b358fcebab339f35fd2da708c983f053 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._TEXTURE_RT_MSAA_SHIFT 96eab9a0405d54487a5775205db6b92e5afaf28f7c70b39f230e622225c3e6cc -->
<!-- api: F:Inno.Native.Bgfx.bgfx._TEXTURE_RT_SHIFT bbe1c3dd5eeee61b209af29a3a836028f36fe1f6b2d448829ef1f9f9399282c4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_U_SHIFT e4e03dc13998378e5e00348c8c73c4120aff797b3622fdad5c3edb5e8663d9b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_V_SHIFT 3b5ddcff09b5d093efcbcaa8f9aa98ac62fed6fd21d107585d9d9df26dca639f -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_W_SHIFT 7d25312f791f1dc164fbc75d27f16b087431fe465068585feebd349f115832cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_MIN_SHIFT 0e9b52c19072fa74b00aea8df11c751678076b958dc4932e9aa0c14dfd0d87fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_MAG_SHIFT f3b586c311176ecfc1026e403ecb9d6a87dc38a6026acc69bc79fd5a11da6b96 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_MIP_SHIFT d8a9fd58789d7baf101e1f068c3155f55be1b14acfe35c6ead553f148e00a684 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_COMPARE_SHIFT f28c591c1041c2e9549e3844e2bafb19b11b64e7c2a31a320e06666c5ee1614f -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_BORDER_COLOR_SHIFT 27a54c6a9c905d12d92d29a5fb44ddb2125391e95061248408f097f4727d2b43 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._SAMPLER_RESERVED_SHIFT e4b49567fdcd857b2c8d214d3849be744c391d7e005065cd6a6f32e0daf83b54 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._RESET_MSAA_SHIFT 784a63edf37afc108d00f2dfcb797c6f76d39bdaabce2142b63b5a25de21b966 -->
<!-- api: F:Inno.Native.Bgfx.bgfx._RESET_FULLSCREEN_SHIFT 77688e86da64dc4f045633035a1975d5d01856ec268e9f1531c71cd04ca27efc -->
<!-- api: F:Inno.Native.Bgfx.bgfx._RESET_RESERVED_SHIFT 430fe0a3962e5c1a305d95e4bf3a79e02629b4758af028dc4e01c3a7d3fd7781 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.attachment_init(Inno.Native.Bgfx.bgfx.Attachment*,Inno.Native.Bgfx.bgfx.TextureHandle,Inno.Native.Bgfx.bgfx.Access,System.UInt16,System.UInt16,System.UInt16,System.Byte) 0499fadbbb3115b71385487784271e885d2f8eda59bdc348734e9998385ff3f0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.attachment_init(Inno.Native.Bgfx.bgfx.Attachment@,Inno.Native.Bgfx.bgfx.TextureHandle,Inno.Native.Bgfx.bgfx.Access,System.UInt16,System.UInt16,System.UInt16,System.Byte) ef08639befa59b7adf490065b73aa7ef8c95553d928db60aa61d55071bda952c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_begin(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.RendererType) c74f76f9ae401b7e5c6b1b8f9c999ae496dd84130dbb7104fd37185347f799b6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_add(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.Attrib,System.Byte,Inno.Native.Bgfx.bgfx.AttribType,System.Byte,System.Byte) 1ab734e203e5a817d613ab6aa2eba7fc30d5d9838daec94c9416534bcb26d645 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_add(Inno.Native.Bgfx.bgfx.VertexLayout@,Inno.Native.Bgfx.bgfx.Attrib,System.Byte,Inno.Native.Bgfx.bgfx.AttribType,System.Boolean,System.Boolean) 4548ade04aa02f4680fd85bbc32bddf5ce1a9e426c08e762dd89d8594db05636 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.Attrib,System.Byte*,Inno.Native.Bgfx.bgfx.AttribType*,System.Byte*,System.Byte*) 05c67d59f23c0cd180d6fdad72cd0f85a0f41969737c8024faa359126cf1713b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.Attrib,System.Byte@,Inno.Native.Bgfx.bgfx.AttribType@,System.Boolean@,System.Boolean@) 6bff682c4e9b331a706d491f4f8ff311b922ce8119cc940c54a2939c17aececd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_has(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.Attrib) 4b0fdc95360fb5a070bd1f1e6f1bf27d143a52eded7f50df413af7d57bdca806 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_skip(Inno.Native.Bgfx.bgfx.VertexLayout*,System.Byte) fe18e6efe80be2ab63453b9419254479e9b73554eeef5021fd7dd5b3df89bf74 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_end(Inno.Native.Bgfx.bgfx.VertexLayout*) fab354e400585d5906b13b65a64bc3fa5d027d208aa7cee553a4abaea505d451 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_end(Inno.Native.Bgfx.bgfx.VertexLayout@) e63f753ea309dbe84eb1f989825ca03978d60ae28677ebcbb7f40a9744a628f3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_get_offset(Inno.Native.Bgfx.bgfx.VertexLayout*,Inno.Native.Bgfx.bgfx.Attrib) 61ce369cb415075086de9d391b421bc95a64bd7584ead67dc309810809d05ec2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_get_stride(Inno.Native.Bgfx.bgfx.VertexLayout*) 182dea99f8cbf31d135242dce617872097dd0e656da5ca5e12040ae55b3de9b6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_layout_get_size(Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt32) b5e222275486f508fc3df4504536bfebe6c919d8b0202a218ac0e10fbb7c03db -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_pack(System.Single*,System.Byte,Inno.Native.Bgfx.bgfx.Attrib,Inno.Native.Bgfx.bgfx.VertexLayout*,System.Void*,System.UInt32) 9b7781672d121086c6d20ff145f1b8d774950538623bac4c230b980762a73c9d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_unpack(System.Single*,Inno.Native.Bgfx.bgfx.Attrib,Inno.Native.Bgfx.bgfx.VertexLayout*,System.Void*,System.UInt32) d28ae9d5b5b0b2437c512ef444e17ab84466c9ce145ccd3caf906aee12eaa745 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.vertex_convert(Inno.Native.Bgfx.bgfx.VertexLayout*,System.Void*,Inno.Native.Bgfx.bgfx.VertexLayout*,System.Void*,System.UInt32) a02f28b766f9785edc541134a78dfe8e1cd1744f8a474a8cb6282325a512a6ef -->
<!-- api: M:Inno.Native.Bgfx.bgfx.weld_vertices(System.Void*,Inno.Native.Bgfx.bgfx.VertexLayout*,System.Void*,System.UInt32,System.Byte,System.Single) d286f68d80316ee07b96ea25e8aedcff6d4a84440a8f6f6bf198fa258e97f890 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert,System.Void*,System.UInt32,System.Void*,System.UInt32,System.Byte) b0948380f635e15b61e73553b9bab2c65b235918fe3c5c08dc6c64ad5af8e3b9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert,System.Void*,System.UInt32,System.Void*,System.UInt32,System.Boolean) cd44f30968d54782646d5cd79d4ed7eb181cc073e5f1d628f303d4c8ecb6b043 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort,System.Void*,System.UInt32,System.Single*,System.Single*,System.Void*,System.UInt32,System.Void*,System.UInt32,System.Byte) 3739737378ce4863430b1e87ab586cb9d0a4aec89ed0a8cbd81cc9df8d9143dd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort,System.Void*,System.UInt32,System.Single*,System.Single*,System.Void*,System.UInt32,System.Void*,System.UInt32,System.Boolean) cfd34be93be78daaa7a92cb2c028fcbb149e9dbc5e27bb9182539e84ed84e353 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_supported_renderers(System.Byte,Inno.Native.Bgfx.bgfx.RendererType*) af0eedae6805ba4fe74cc2da99d7483c88bace584dcc7033eead3ad22521ef4e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_supported_renderers(System.Byte,Inno.Native.Bgfx.bgfx.RendererType@) 9b49d14cba8c6623dc81782587398964f72c82d17254fdc2d78aa2ceba4c1782 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_renderer_name(Inno.Native.Bgfx.bgfx.RendererType) c8f1c2d0a69ac0d21001779442c7a4ba6189ba880ab95bf8f791b23533cba866 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.init_ctor(Inno.Native.Bgfx.bgfx.Init*) 7f1091650f4c0131296cfec55f72b90aa3215fd8a33948bf4aeeace4601a180f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.init_ctor(Inno.Native.Bgfx.bgfx.Init@) 4465b8ec466655ca4f5c62026376181a80da86eb183933b351e8ba66d300d2e1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.init(Inno.Native.Bgfx.bgfx.Init*) 1bb3a41977b121b258f34e2d8f3a95569774edd63d671985b74dded8f0b545ff -->
<!-- api: M:Inno.Native.Bgfx.bgfx.shutdown b62ea4d06b1e550fa16a3b0926bc6b237b4ea47e5aec4ae79100dda5720bfb58 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.reset(System.UInt32,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.TextureFormat) 123ef4e0ba70ff38fba1ba89e56d9a3314c64cc8f82adc6ee1754b277a07eea7 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.frame(System.Byte) 9e99f76a0b0633eee198222b87608f9bcd3f9c614d60b2716e6cffce6545d0e1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_renderer_type 74cea2c909601a6476e3e115d1724ff5c850485a951407d2a13f6d4f495ad425 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_caps ac6ee5f43c3774aef5bba8137a2f7f7c1ea13ba44fd4e5359d043c30c50af40d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_stats a2cbaf09e334ef13b54f46d3bb66fec319602cd6c06433f3d5aba5d565412266 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc(System.UInt32) 8754814a3785832a53046ed95642ad61ea088dd1cb0420f85412fb71cd966d7b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.copy(System.Void*,System.UInt32) 54c6d9ea782f8a3e0ccacb62fc605f974df4a8346f70a872e38edb6cdceaa01c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.make_ref(System.Void*,System.UInt32) 8b9dec172c6fd7e829b095b595214ed6347ee8fa4a2b1215c54c7562ff87c4f4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.make_ref_release(System.Void*,System.UInt32,System.Void*,System.Void*) 1cfb5a92c6c764716be6c408e5be121a1d820ee4c82c25df6f1b0f2eabb2f7f0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_debug(System.UInt32) fca8e4796b1bc8af8903dc00b8245a28cf6ed0fbb03dfeee25751b0cc16b9f75 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dbg_text_clear(System.Byte,System.Byte) 8c267af5b673486bfd06fb9501e71fabb3f3ca9aed46de36b44aef3f380da148 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dbg_text_clear(System.Byte,System.Boolean) ba1ce20d44ffc51e3e3e782f3df5ab12c46704f3d310a7943abf698753a22902 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dbg_text_vprintf(System.UInt16,System.UInt16,System.Byte,System.Byte*,System.Byte*) 8ab5bde5913ed55534084bd171d15f81f646318ee7811a03b71ebb4ec4c2f833 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dbg_text_image(System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.Void*,System.UInt16) 47012b6de5cf22e0456c263cd06d812cd51412ac1864d1395ec0cdfe55ca3b27 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_index_buffer(Inno.Native.Bgfx.bgfx.Memory*,System.UInt16) 6f1db2729118ee50a1365947a09393980a247ef0870fec5f726678fec536f51b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.Byte*,System.Int32) c2158f57c8517feb827dd79beba459b4a5861852fc6b0079cf0521500a1c5a8a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.String,System.Int32) 038e2699326d928f78c1d37c32a73a42a1ba2973aae4298aa70461ba24c0dcdf -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle) 06bd235721413c9bcf5b4bb8197b039ec59b4b60ad54b0c339e274e939747b07 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayout*) 9b11f34de68075cc91ed753cd6d6e5c6fa850dbc3ed9e38c0f3f5753ce111b86 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 6cf32fe58691d911d8867beb60b324f0f8b0e1d57140b7ebf3b88add87b03f42 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_vertex_buffer(Inno.Native.Bgfx.bgfx.Memory*,Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt16) 55fb299daa8f42fb8b33c05b554c041716a7e2737b9ed86730806cf4d86a3c21 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.Byte*,System.Int32) c37ac7e31c6d03b0618ea6d0bfb76c0f55224a7190a836354fc3999ed4e9a581 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.String,System.Int32) 8821aacaad59a57f7b57fca7f82a279e70f5dbc776f343c7d1e7153b8953625b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle) e4e53cb9931089438b06fcb86bd2f6ed3e3981a46d372835e87b62fe7a8dd7d8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer(System.UInt32,System.UInt16) e916673bb5ff0de02b57343cb39ed09980b5858f6e97f417948f25d8463a8388 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer_mem(Inno.Native.Bgfx.bgfx.Memory*,System.UInt16) 88d0dbfa2864719b4ca92191fedaf4ca49a7c5cbadda6c05ac8816b32916577d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.update_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle,System.UInt32,Inno.Native.Bgfx.bgfx.Memory*) 4f33d4cae70bab242dc1bd0d20a02c487d389fef3f7769421fb30ad868676c40 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle) 1ac8f4e4ff8e43b9e111062cf4e48a697bf1eecbe1b5ad07fca99fe969610d9f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer(System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt16) 38229da420ae49c6ea962f96fe81eeb797b67db60fa333ad582d8b576d6440da -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer_mem(Inno.Native.Bgfx.bgfx.Memory*,Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt16) 31dea1268d3b9a420fdea1674e41fd8c6ca7081e1310f98a171806be8f0867cc -->
<!-- api: M:Inno.Native.Bgfx.bgfx.update_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,Inno.Native.Bgfx.bgfx.Memory*) 0b0ae497d095a77ae0894b4d672368ae0ee36d64885452a4d3cffc5e45de4db9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle) 2fc893cb10920cbb6f273e620930c74e3a73cdaec3d0774f34747728ea3198f1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(System.UInt32,System.Byte) a81d3d4c06b49b14da889d14305d21d6dc18489b86924772f43914ebeb545796 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(System.UInt32,System.Boolean) ba504988b78bd68d46bf618b4bf8013fd8ed50bdaf8f1067abfe8f18e3338d3e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_avail_transient_vertex_buffer(System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayout*) 9523790dc1034c0d688ac9b5be78eda5127db6d219c1221381143b8c76aada0f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_avail_instance_data_buffer(System.UInt32,System.UInt16) 23e475b722ce35118a1fa0ce7999f082e2d45524aed815d5811fca1be340def6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer*,System.UInt32,System.Byte) 7a509af9c2b671929eb6785fcc4561b94f8d5c278b6d82a29120e146f70df4d9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer@,System.UInt32,System.Boolean) 656e68c2bcb6f6cb73673ce97b54110ff5b0c7a91b41e35b09fd031b1e03d3df -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayout*) 67c3cc51912e46addff4791fe3c34ab721cc2983a72b8bd1eae8b7e21320e5f4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.TransientVertexBuffer@,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayout*) b78ba890e95bc961f6f516f9d2b79925bdca89b94d9a020708961ff1bfa4f8e0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_buffers(Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt32,Inno.Native.Bgfx.bgfx.TransientIndexBuffer*,System.UInt32,System.Byte) 94d93ffa7a09c63d3be43e3e46c953148fbf2af1a2e0ddb21cd7d0c2daecf37e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transient_buffers(Inno.Native.Bgfx.bgfx.TransientVertexBuffer@,Inno.Native.Bgfx.bgfx.VertexLayout*,System.UInt32,Inno.Native.Bgfx.bgfx.TransientIndexBuffer@,System.UInt32,System.Boolean) 3575eadd90a901fd12af0c9e8c0f16ad03ed54583654f245ce32919d90566f30 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer*,System.UInt32,System.UInt16) 1912f13f6ac8054a3765c0c4bc03b4eb257b4b895d2c45c8bb6486158d804e34 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer@,System.UInt32,System.UInt16) bb6d274cc7fb14a2f81ab157fa60d75ce1ac41db0e2ec22fdafd5db2b178c47a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_indirect_buffer(System.UInt32) 0682eab569dae6915f7ecd3d896e35507f44d98788d7b5e0af860bd28c6b1998 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_indirect_buffer(Inno.Native.Bgfx.bgfx.IndirectBufferHandle) ac71559a67eeb730a80ad22959662710738b4cd929367ac6cad9120ab2364ea2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_shader(Inno.Native.Bgfx.bgfx.Memory*) b00ca8d3d93a5e4bf66618f2a75d693e4e3087de945f06ccf53e09de5f6f7411 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle,Inno.Native.Bgfx.bgfx.UniformHandle*,System.UInt16) c864c30dbbebb3aa70e7bc292744a69cc0711f8d09b95ae87c9ede2c51724b68 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle,Inno.Native.Bgfx.bgfx.UniformHandle@,System.UInt16) 59b4a710772e62dea10238ef88749c7ef36e78088a15ee11412fbb200da9f041 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle,System.Byte*,System.Int32) 4991ab8d4032d69892a3d66d2fc1755974d5ef3f3bd6e81beb778ba015bbc6b9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle,System.String,System.Int32) f80113d126c3f539e56d1a00a9471465231232294f0076dcb23b7de7963f5cf4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_shader(Inno.Native.Bgfx.bgfx.ShaderHandle) 24b5b83ec98ebfde82373743a3cf421e46f2dcb32ea4ef02255209cba77223b6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle,Inno.Native.Bgfx.bgfx.ShaderHandle,System.Byte) 662372880b6e8502768c3ad438e6dc6435adc2439e68e140495ab91a91f348d7 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle,Inno.Native.Bgfx.bgfx.ShaderHandle,System.Boolean) 2ef9c806d4b4584ec1548bd99489e924fb6d67d90cd43faa732bfb54cf880789 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle,System.Byte) b3ee23f02194ea4e2d02f866172af69e364f76c531d385d7576f07487d5f722b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle,System.Boolean) c895a2fac47eb81f48a49f4daf5e31c1d79f80571f0ad220eef4be565d36483a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_program(Inno.Native.Bgfx.bgfx.ProgramHandle) 54a81e4aca6601bff2b7fe65ba5eb4cd4b464291e639291287e845c94e498745 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.is_texture_valid(System.UInt16,System.Byte,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) d2b073fd1aaa40787a83f2b70dec49a89a4ad98aeddf0402458dddcb919942f2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.is_texture_valid(System.UInt16,System.Boolean,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) 0f7932bb9d6241b70d1221989d4bee434054bf39aa96a1f6d5765b48d76fb41c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.is_frame_buffer_valid(System.Byte,Inno.Native.Bgfx.bgfx.Attachment*) 3469a95f721a21ef05d10bf4dfb0a8e58fe815203b6119934ac27f7c238931f8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.calc_texture_size(Inno.Native.Bgfx.bgfx.TextureInfo*,System.UInt16,System.UInt16,System.UInt16,System.Byte,System.Byte,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat) d814de964001714f04b5adff0c6875aef0c2e65e5301123cd886cd9a9c348da3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.calc_texture_size(Inno.Native.Bgfx.bgfx.TextureInfo@,System.UInt16,System.UInt16,System.UInt16,System.Boolean,System.Boolean,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat) a7234ab008d63bd4a2f3042e8d8421318cd321784bf5ec47221ef1ad2f7d817c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory*,System.UInt64,System.Byte,Inno.Native.Bgfx.bgfx.TextureInfo*) 4267f1e405e35ec091735f758cb0e42df8647df55ae55b5412ced7b48c62fe79 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory*,System.UInt64,System.Byte,Inno.Native.Bgfx.bgfx.TextureInfo@) 32e284c610dbfb4d52ac2b94fbd451fe94ba0241003e4c55d7a8351d77c4c745 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_2d(System.UInt16,System.UInt16,System.Byte,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) a895b6ee0c9b7beac0bd7c9f2298e93f577b551fb19a06eb26d0aabf715a732b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_2d(System.UInt16,System.UInt16,System.Boolean,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) 7e9bb6de776d2ca960799b51b650e2b0dbdf038e3e4d06d2dc224a665455c89f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio,System.Byte,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) fdc2f9145c3ad168beb6a9357b9bef315068b2f1f5af4c0cfdd1c5bf62343b3b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio,System.Boolean,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) 94cfc8ba78115ac014239865418d58f5e818af7950d165210cb1b8ba877ec096 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_3d(System.UInt16,System.UInt16,System.UInt16,System.Byte,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) 4e558f769ea7f2ba1271646a05cd4799f5697a2ea1c18383f4c61df8ec92d1c8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_3d(System.UInt16,System.UInt16,System.UInt16,System.Boolean,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) 9b127ff86c46a9f075ebd841f6fae8d0e1fd5212b2360b66c1eb4941e97f3f16 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_cube(System.UInt16,System.Byte,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) 6d1024a0fa871f6b80de34fc9640cf92273769f6926fceb0bcebeb234dff3c9f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_texture_cube(System.UInt16,System.Boolean,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64,Inno.Native.Bgfx.bgfx.Memory*,System.UInt64) ce5bede5fd5bd3f0d197f0ed835bf230ff1da891adbb267b42b2f7577e998496 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.update_texture_2d(Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt16,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.Memory*,System.UInt16) 3b145ae28ea9b1ebdb61483d4613b65a858ef19b2eb8a1bca4781dccadb7b882 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.update_texture_3d(Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.Memory*) 8556cc773305a0dca0002cd8a0c73c387f2bcb5cd9069c25308598108f85e199 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.update_texture_cube(Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt16,System.Byte,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.Memory*,System.UInt16) ddce89404e44f6ecc2c4151f637173b05f9401c298a5bfb8465088a64dd5f09e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.read_texture(Inno.Native.Bgfx.bgfx.TextureHandle,System.Void*,System.Byte) f035c3333c9c4cda48d18401c36a9d0a4331a1201ac278fff4b002b5b5a10c73 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte*,System.Int32) c2b02b023fef41f567b82120c364a69def4106fbeb853725b0c0f64379d1c4af -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle,System.String,System.Int32) b750df606c0e3a83463ce3a92de90eb783a05c060d02a9c25b2353de2a9411b8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_direct_access_ptr(Inno.Native.Bgfx.bgfx.TextureHandle) e273e75ee0afc1581166405b6f6ccc7072605926e7c54c1bd0f9515c66c85eef -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_texture(Inno.Native.Bgfx.bgfx.TextureHandle) aaec3639ce5820dbaf331ea20c29a1367b7b86b48910b0fa34787244f6776f30 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer(System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) 9deb73c78c64603617cc9f1d01d711ea4cd74d628180aa24548d75e5c4e3162c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) f3292050861d278a5e72f334d2fe5afc2f835d6bb003a0c9f1a6b9bd8c5952fa -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(System.Byte,Inno.Native.Bgfx.bgfx.TextureHandle*,System.Byte) 01cf216ed3f2078ca864d2b5f9c81700e3f2def5cd91b28a0827b70599efbeb0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(System.Byte,Inno.Native.Bgfx.bgfx.TextureHandle*,System.Boolean) e38d05f0188102d3b69c06f24527d63229b472eff0cb23ca9ca4ebff80e68a60 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(System.Byte,Inno.Native.Bgfx.bgfx.Attachment*,System.Byte) e12f4531b945ac7ac36c68bb2b5be7c127396b85c8f21c30bc35e35bb3562d36 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(System.Byte,Inno.Native.Bgfx.bgfx.Attachment*,System.Boolean) 149e4b33e48e3e2e637dfab6400a924510fdb5e1d2a6ce27b5bd97a2eae013cd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_frame_buffer_from_nwh(System.Void*,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.TextureFormat,Inno.Native.Bgfx.bgfx.TextureFormat) c6574390dec6be76c6cc1f9161a3a9774d330755b372d3c98769710a7ac06db2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle,System.Byte*,System.Int32) 67ca17dea4319fb4463ea6ddb777e2fbf7b67333a840e95bddc669dc837863f1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle,System.String,System.Int32) fd3b0b130269501aa39b698071c9fbf34c07a5fd89238af7c4926455ec5821c0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_texture(Inno.Native.Bgfx.bgfx.FrameBufferHandle,System.Byte) 7a866aad5f8d82f7cb4bcbfe826e5c24f04933f93f7db60b3318bbc01be1a8f4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_frame_buffer(Inno.Native.Bgfx.bgfx.FrameBufferHandle) a33c4c71ce5a433659ec9fad697cc32615a3ddcb5415ea52f8c122f2fe880ad7 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_uniform(System.Byte*,Inno.Native.Bgfx.bgfx.UniformType,System.UInt16) a59621199d19e4577aff4d2c43cb3f7c84e5e4dbab48319bcb3001f849254e11 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_uniform(System.String,Inno.Native.Bgfx.bgfx.UniformType,System.UInt16) 02b76b3668e174326a281bcfbabe36b2b6ee3eb0308567336735700d754ced5a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_uniform_with_freq(System.Byte*,Inno.Native.Bgfx.bgfx.UniformFreq,Inno.Native.Bgfx.bgfx.UniformType,System.UInt16) 310a9e90c5e9e85c64fcd27cc511268a6be0c46305e5514ef83029b698c772ae -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_uniform_with_freq(System.String,Inno.Native.Bgfx.bgfx.UniformFreq,Inno.Native.Bgfx.bgfx.UniformType,System.UInt16) 9e388578de0bb38aff0d9ed849769205c88ba365433a683001e607655574a6f0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle,Inno.Native.Bgfx.bgfx.UniformInfo*) b0b12f04a7804b9ebeadf201c8fef74173ce79c430b86e7121e7fdfd91016300 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle,Inno.Native.Bgfx.bgfx.UniformInfo@) e527a8e3557804da23b54c5c0021a3a586d1a0e1b565a5641557dc39e6891f9a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_uniform(Inno.Native.Bgfx.bgfx.UniformHandle) acc1df88fc3c988ce3f085e89b3ab11e04fb6d21fde82a0182a83447099de005 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.create_occlusion_query bf4a8793ba8d1558e173104348c30151e23b95ede380ce31c28c1ffafaffd493 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Int32*) a51a6c1961f55afd7114a81a023a641a7ce1275424f8662bdbcee2b5af445abb -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Int32@) e70f2059cc1bc821b3bbdabe9116e988edb228e100aa68162137058d359d03de -->
<!-- api: M:Inno.Native.Bgfx.bgfx.destroy_occlusion_query(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle) 2b6da665cc63673b9a55de1b45005f571b7a3ac674a1b4d07112cfe79e52a34d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_palette_color(System.Byte,System.Single*) b966ca917e12da2de9eee4bdc2523f33a9bc295269f842ea6d24401c8363f870 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_palette_color_rgba32f(System.Byte,System.Single,System.Single,System.Single,System.Single) 43a78568d84dc2c4fb28595c118c55d285a7dcc26dfc85ad32120c1e07a3d56e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_palette_color_rgba8(System.Byte,System.UInt32) dd9e30e341eae7e2985ee179d36cc03c8d672caab5c61adda1407123ae55b83e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_name(System.UInt16,System.Byte*,System.Int32) 54554190b1168a2b3e4fffa15017ccae33c0ee57f7bdf1bbf24b7a1b00790eee -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_name(System.UInt16,System.String,System.Int32) ef4297cb20f112488673f04aa531c5325dd725f01f8a04a567474755077734cc -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_rect(System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) ef49e8dce15083d50fc0c0463fbb8decee2e68a6f5535731b66f34a3c6857e73 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_rect_ratio(System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.BackbufferRatio) 8ff4487a371b5df9768ae7db6899d670ad323741547563003532e369913e043b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_scissor(System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 819e20dfb8731d710f1456fb821a341bbd44005ab2fd594b8e04208ef2189a0b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_clear(System.UInt16,System.UInt16,System.UInt32,System.Single,System.Byte) 0a13fc3118778bd1ddce9c4c3e95ddde17adcbb42f496ebe594362da5e2cd0fe -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_clear_mrt(System.UInt16,System.UInt16,System.Single,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 9260654bb26258bc1a92a27f6a171b724ea30286f9c837234035a9bbe62a85b0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_mode(System.UInt16,Inno.Native.Bgfx.bgfx.ViewMode) f665008b8897430a1c22055c80494c2c8e1d0e554fe746c6447077f102399935 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_frame_buffer(System.UInt16,Inno.Native.Bgfx.bgfx.FrameBufferHandle) 83178ddbb2720c488c53b57e610ecbf9550c1afa2d789fb7dc06edc6cfdeafb3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_transform(System.UInt16,System.Void*,System.Void*) c7dde80312932093ff2c47443e0e625946e09f809e1e68192434a359bda92e41 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_order(System.UInt16,System.UInt16,System.UInt16*) 4c50cba2d5c7e5654a1df39e7199029700e117d7ca3d7c48ddc373c20cd19d32 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_shading_rate(System.UInt16,Inno.Native.Bgfx.bgfx.ShadingRate) 8389de1ee192bc0ccefedcd72b4325756bd5419cc14ccc1d647c91b782ff446c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.reset_view(System.UInt16) cb536a98a18b0015428f8bb46efc717e7bffe2270c9d9bd07bd3643c2c71b531 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_begin(System.Byte) b6c99e2594cd7cb19ad31ae07cabd852bb833259aa9ad7b94391c66b73965fca -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_begin(System.Boolean) 3cf85742a3050a1a24e8f0b2f75db9b00d2dea702281f4b2e959d1b44694619f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_end(Inno.Native.Bgfx.bgfx.Encoder) 5229f4f4d7860128f9a468877b3e786d2df9c084cafa61d8f5b4a8bea2c3b05c -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder,System.Byte*,System.Int32) 71f88f6ed595a70d0d315e4649f8f4a1fdbd0c2d1dc5d351239d6427bb4627d6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder,System.String,System.Int32) 57a4232d35897a5c90cbdf9a8550c8bfd38ae0128a51f426fc85ece69b20063b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_state(Inno.Native.Bgfx.bgfx.Encoder,System.UInt64,System.UInt32) f7ad63d0165727cb23a602633bdaa00dbdeac47f73a8d6a07a3017699772da3e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Byte) 56438cdebbcdcc5022562a5d1659110d4297870aae64c79327361938112f75f3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Boolean) 7c4ebe0a0ae6149f2fcfbe0f7076c9970183dac86ee36ffb0c6760f1aaedfec6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_stencil(Inno.Native.Bgfx.bgfx.Encoder,System.UInt32,System.UInt32) a6574bb09737d78afa36ddcea4bb9de476914c9b32fb22d059f8926597d809c7 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_scissor(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 85f6e0ebad0c7654f735d8218ebbee247010e2988c456de6d81bdae3e9e96c74 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_scissor_cached(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16) c067e984a16a7432551e0e2be857d0bf9c92acbd6ed0b90ff69b303034947201 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_transform(Inno.Native.Bgfx.bgfx.Encoder,System.Void*,System.UInt16) 07fbc9448b50b412aa9c33dbf18fcbb217c71545d2e99cff29e3ae244df6afdd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_transform_cached(Inno.Native.Bgfx.bgfx.Encoder,System.UInt32,System.UInt16) 14233ca35e61d81692d7a03c056aa39cd7adff3ccd857ff8c982b1e3840183ba -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.Transform*,System.UInt16) 3528bd45e66f9d67f195772ce69c6949879964b43d6b0726ea91d61acc8efa7f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.Transform@,System.UInt16) 28925eb6a14c1de13711273d271661f7b429bb9599190453d4b026435ca9a997 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_uniform(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.UniformHandle,System.Void*,System.UInt16) f43d9b088a71370ccc757473d9fe313f70603f60ebd97f9e52f09726455b6544 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_view_uniform(System.UInt16,Inno.Native.Bgfx.bgfx.UniformHandle,System.Void*,System.UInt16) 8e43ef4180c5eead76d3212ca24b943b4d1c837bb56e5337e3a30331fad7d4f2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_frame_uniform(Inno.Native.Bgfx.bgfx.UniformHandle,System.Void*,System.UInt16) 70796ff910709b582e50f9f54a6558afed4437d8652e04692e04299b85bf8194 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_index_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.UInt32,System.UInt32) 8edf9f709af2288b0b415a506756946ad84699a100495089d79e63e2b02eb85e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle,System.UInt32,System.UInt32) ccbed41f2f17bff48dec8c089d19cf92db076ff588317e5caada9f6bf34582d4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_transient_index_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.TransientIndexBuffer*,System.UInt32,System.UInt32) 6922020c57b441250e0f80b5e527d2fa96d6a9872b8c07d3ef02ceda705bb914 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32) d0fa20479a666e8a3afc59fea8ceaf4088ec3aca45ba68739f9cad0828ec87a8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) b1cc568572bf66a9acf705a53ec6fd5bc95043b9f28ee64ae4283103f9f7f734 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32) 3cac560a1e2e5a342cfcafa2ce6d572fdd491b0fea80b7cb1a330d2e33081afc -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 2b3ba4c66c94e2275e4252a81c129e1ab9235a34fc10d7db826c9ec8bed4005a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,System.UInt32,System.UInt32) 8426c1d6b85baeb34349d6b940b1b03459db7abc442581f8a7fb3fcab4204798 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) ed4923160282134f6a386fe748883c46e92defea3e9a77918b12597d9b51ea76 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_vertex_count(Inno.Native.Bgfx.bgfx.Encoder,System.UInt32) c9bc8e39e7ee1a5310e69e15ce6e9df1d06e8391f896fd6b8364f31c26860815 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_instance_data_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.InstanceDataBuffer*,System.UInt32,System.UInt32) 9e342f4dd7fd03caf2ed4f72cf1935ca11156e1bec612d26432b472bcc5e9684 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32) d59d1282b409c1928c967d6f5a8296505c2825baa51c6cfb8b3e929553fb80cd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32) 4d7e94c55a0c82a504dff248a6d746bcf573e626705b8ed213baa73ae6f42272 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_instance_count(Inno.Native.Bgfx.bgfx.Encoder,System.UInt32) 9e710184cad02c4479b4fd9ff4f54efab2de1f0b015ce2a803475e4750b4cc92 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_texture(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.UniformHandle,Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt32) 2b436558e6ddf3cff229505e364d67b6659311114cb2684455062bf1d8a1866b -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_touch(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16) a3ce94af7a77faf0d661caff391d61ae73461a0dc3e08b0fd502cb06049b63e8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_submit(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,System.UInt32,System.Byte) 8713ab8c1d15305fc5379158d25bc0d3458b0e392e7caee9419ffed4a3e28457 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_submit_occlusion_query(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.UInt32,System.Byte) 71f82609e61e709ed092c37baedb4c620ca0996ad4af1e848ee8fc74ab90c7a0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_submit_indirect(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) e653725edc32dc96213ca614b800d2a29d90e045d035b7b131abc628c06dacda -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_submit_indirect_count(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) 9bc81cd3aca9f6b9793c8832147cc294334c989d8d8c593ed24a8bb64598aa75 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_compute_index_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.IndexBufferHandle,Inno.Native.Bgfx.bgfx.Access) bf7ad291a224a25bdda61387afde37f2a6e29ab18a6a9db9e2b0ab656630e6ab -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_compute_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,Inno.Native.Bgfx.bgfx.Access) d42ca0f4fbf24dcc544306a1124a0b50ebad05b05ceded7695184b462e2c3dd3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle,Inno.Native.Bgfx.bgfx.Access) 801bc228e6f28e9d3a1cae5e9371da6104d79e92f651f5c8cf9957130da7809f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,Inno.Native.Bgfx.bgfx.Access) 069c8adf1011f7df65f52745572112bb942eacaf15f9abc7af743cc0129d66cb -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_compute_indirect_buffer(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,Inno.Native.Bgfx.bgfx.Access) 881ac7e888d6a2408dfaf7db244a88645076e7f4a5e8b11f9e5973e998a0071e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_set_image(Inno.Native.Bgfx.bgfx.Encoder,System.Byte,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,Inno.Native.Bgfx.bgfx.Access,Inno.Native.Bgfx.bgfx.TextureFormat) 62d59977569fa26ba98f6cd5f3f295294aaba38472c7fa669ac0fdb0bbd17aca -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_dispatch(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) e2ee4959caff64b8fa12dc7bfcc8416146568bff7755f9602c9b0d6214f7c5c6 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_dispatch_indirect(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,System.UInt32,System.Byte) cc353c75a2e65048f5e4a09a63ce22d9ecb661a97136856f719cca581434f7c0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_discard(Inno.Native.Bgfx.bgfx.Encoder,System.Byte) 737b1390bc1899635a6cdd2e2b603d98255a401c78dbbdec7ed248f571097963 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.encoder_blit(Inno.Native.Bgfx.bgfx.Encoder,System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 843d188cae8c93c04bbc5147d256c6c4b6a881cac631dfe6db89f9fd53479e1d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle,System.Byte*) b930dae07a13401e7ff219128977ad366cc6e0408502b743885bc09beaf1bc97 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle,System.String) 47cbd6ea62cfd6b1463325c2d8052e0b9a5ee7e052be45b4f24cf58c2c0481e2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.render_frame(System.Int32) eb200a255c1d43c887f1a8670aba52002206f92dded957ef2ce21eea144fcca3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_platform_data(Inno.Native.Bgfx.bgfx.PlatformData*) 0f1cf05b7e9802aaf88772ff31e303c49aed44642246ad66bc15560c4ed5f9e3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_internal_data 07b4063dc3d161684ec04c495caea85f63c9afdd218861daf47fd9546bb574b1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.override_internal_texture_ptr(Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt64,System.UInt16) e7ff9ba3978cee49fe0ac3af09f8615dfacf8845a5470ca433039eb916bb2d1e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.override_internal_texture(Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt16,System.UInt16,System.Byte,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt64) e9a8fae202c83ae4b318efb11334acc572a43d1aad5b10e2965cde88b5f077c2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_marker(System.Byte*,System.Int32) 8ec6f436a2270a087fdf5b433f50f8a71391494202a7230d35ee224664987be5 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_marker(System.String,System.Int32) 9174697c738f6206de25c381d1bfe9fa73a16be2315bcbefe418e7c2f5544f99 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_state(System.UInt64,System.UInt32) bcf7fbd50830ca0d03c935b4119b8487c1ae8f4571f5e473a079ea9648a1dca2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Byte) 4363d61860853a7b3d3021a0f9173f186b94482f4ca86fb49c647630ae0839ec -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.Boolean) d4a46894cc1dd6f331d8694aad52c88c67999aff4ec819d5a487f69091e33fa7 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_stencil(System.UInt32,System.UInt32) 27f123690a5986507708b64dd7d36f542712855e8179c6aeca83fbaabb08482e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_scissor(System.UInt16,System.UInt16,System.UInt16,System.UInt16) 2b8edbf7378abeb72195700932ba921f372fe543f2c7d17e865b4ae3e3d24ffb -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_scissor_cached(System.UInt16) ab3732da25e59b07c4e8373b44255346a2152a2ce6043d8078872495f0d5a160 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_transform(System.Void*,System.UInt16) fc344fa185eefec302b066887c6873d34157769278c548502bf0b8b527a1965f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_transform_cached(System.UInt32,System.UInt16) ea4c8f982dd23f3acb458db7e6be0920c69cf1f8cd510cf097bdd0286453438d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transform(Inno.Native.Bgfx.bgfx.Transform*,System.UInt16) 9fc52c80f995baf5ea0a781cb8f63cbc9e548a86fa9412bac4f5d326a06c8800 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.alloc_transform(Inno.Native.Bgfx.bgfx.Transform@,System.UInt16) 74bbb9cf2c3e779b5101a885f6c4f4f47489e0a5c6c10715ea60515f10d9844f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_uniform(Inno.Native.Bgfx.bgfx.UniformHandle,System.Void*,System.UInt16) 95b4b0d58bb4c669f703db8307a1b872be0db66f861e98ecbf367e2018d1c610 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.UInt32,System.UInt32) 896f338b7a77282c320af03d8e96ef8af754ba02b9d58e6216e195bdde011b02 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle,System.UInt32,System.UInt32) a7198ee28a1028d48f9ccaed525fd9ca53c547e6587534d3b094661d22b9fad5 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer*,System.UInt32,System.UInt32) 6f2fe3f46bda31e41441ec63c7d5c4f0714fca55a4a5dd6f7819d36cf895da6f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_vertex_buffer(System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32) 1d34e8a321bb1d512ef3b7701c8ed928ee9962a4c218df5da3c2061965b1f258 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_vertex_buffer_with_layout(System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 5c6b40c1b867c7ef75c2965ee1a5aa62fb52cb59deef1337a6d1e6f3644df369 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer(System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32) 8d895fda329ce051452dcf58052a40a4057901bd1712217001fa06b4aa9ffc88 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer_with_layout(System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 3df6dcd220834f53c52c886244a8d2e9c6311b154e6882c0ec255f431643f55f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer(System.Byte,Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,System.UInt32,System.UInt32) 28451b4c0d5d6d9891688c634b0187457f4084679de58ca90399c4ac2c3f9bcb -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer_with_layout(System.Byte,Inno.Native.Bgfx.bgfx.TransientVertexBuffer*,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 8a6f0fd910e46ef0af596cae4adc4156eb5514f7b65c25da526c959e7c32796a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_vertex_count(System.UInt32) da8a6655c16486591807260bac8ad1fa4151ac267faf08868d5bd37f0cecff28 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer*,System.UInt32,System.UInt32) c349ed3e9a000037348a544433ae947a60aafaa080d6ea062e94203edcab976f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle,System.UInt32,System.UInt32) 66af034aab1b87549e692d05688a8c80b5838acb68cae1e5572aad8b860f3698 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,System.UInt32,System.UInt32) f3c27f4e9e606a86217ee801b8d52194688a6eeaf5ce58be65e33d70daf06298 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_instance_count(System.UInt32) 119e871e9698c54bc7ca8ff2c94e1bd6b9c86fa659f6d2c5e754292b07284f7a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_texture(System.Byte,Inno.Native.Bgfx.bgfx.UniformHandle,Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt32) 8ba147ed9c479caadc29e637964127719d3a6c98001f1cb9f41038150d1eaa30 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.touch(System.UInt16) a50cdcf1b4de6897388b8a3924ca6e002571af72cb49b1ba26515be4be8ec3e2 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.submit(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,System.UInt32,System.Byte) af6c1f08bcf81adb26517a149774fa6aaad60a3dd0f925dd2c18bab15e2e1368 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.submit_occlusion_query(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.OcclusionQueryHandle,System.UInt32,System.Byte) 43c94f0d5da70c7b1c9fd36d84d8c0a45355690bf0195d2b882c41a504fcfbcb -->
<!-- api: M:Inno.Native.Bgfx.bgfx.submit_indirect(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) 5edffb17529eae2ec5bc367370b8136abeb5b942fc630998f7d41dbbd2b44cfc -->
<!-- api: M:Inno.Native.Bgfx.bgfx.submit_indirect_count(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) 507bf88cc2ac7dee0eeb6663f7cbace2d75e3f1536eb82eeb18863e98ceb5837 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_compute_index_buffer(System.Byte,Inno.Native.Bgfx.bgfx.IndexBufferHandle,Inno.Native.Bgfx.bgfx.Access) 89ef2961eb30387288cc66bb5a9a7bd219f7e51dfe7f18be4ced338c2b9149ec -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_compute_vertex_buffer(System.Byte,Inno.Native.Bgfx.bgfx.VertexBufferHandle,Inno.Native.Bgfx.bgfx.Access) 3a4b5479dc04077276dfc26c619fa390902bad77981a92da8284f18b0e9ad668 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_compute_dynamic_index_buffer(System.Byte,Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle,Inno.Native.Bgfx.bgfx.Access) 08ce1133831b84dd393524ca70eb9fb0f53163af631a5d447975d932034522a9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_compute_dynamic_vertex_buffer(System.Byte,Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle,Inno.Native.Bgfx.bgfx.Access) b07a551439ebc5a28a1dc33b01039f85ccaadc1c9afbe1dae1a0602a33d68c78 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_compute_indirect_buffer(System.Byte,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,Inno.Native.Bgfx.bgfx.Access) 9947ff45fa2c1e341d65e6de928a3db834322a14828af132761fdd906f3e0a07 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.set_image(System.Byte,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,Inno.Native.Bgfx.bgfx.Access,Inno.Native.Bgfx.bgfx.TextureFormat) 6c772837bb15965e4a921cf476556781e152343dfe862251e790c572d9999678 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dispatch(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,System.UInt32,System.UInt32,System.UInt32,System.Byte) 9d2aaa7d844a6a1eae3cbda9731a4b84ba6603db8c48cf5bace01a2c09290cbd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.dispatch_indirect(System.UInt16,Inno.Native.Bgfx.bgfx.ProgramHandle,Inno.Native.Bgfx.bgfx.IndirectBufferHandle,System.UInt32,System.UInt32,System.Byte) d5440338a7408910c1e00d4b1b6f9ef10149967b5b15550107533dbaeb013233 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.discard(System.Byte) be712aee8029e61431509a794e2efa308b4f5bbb8ef64ec35cc983f517a6e496 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.blit(System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.TextureHandle,System.Byte,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 570b7f7ca80cf755c12ba559ea46f02d470efbce313a4258109a5450022429ec -->
<!-- api: M:Inno.Native.Bgfx.bgfx.get_interface(System.UInt32) c66bb96cf9b7a65ec20b7f97a7e4bb61bbf90ecbc0f92fc42ec6276e3e6361f1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-23db435917e7bdf2"></a>[`public const int Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_FORMAT_SHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3819) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f9dcf9cf4331573b"></a>[`public const int Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_TYPE_SHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3820) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6d22ecae24b8f3bb"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_FULLSCREEN_SHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3833) | 当前常量或枚举值：`0`。 | — |
| <a id="api-49c5fa3575807782"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_MSAA_SHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3832) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5dce4a5b092ad3cd"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_RESERVED_SHIFT = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3834) | 当前常量或枚举值：`31`。 | — |
| <a id="api-5d2aec46e79ebeb2"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_BORDER_COLOR_SHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3830) | 当前常量或枚举值：`24`。 | — |
| <a id="api-4de3da5a238f075e"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_COMPARE_SHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3829) | 当前常量或枚举值：`16`。 | — |
| <a id="api-47db570615b4bc8e"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MAG_SHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3827) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3326c3b119e08b7f"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MIN_SHIFT = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3826) | 当前常量或枚举值：`6`。 | — |
| <a id="api-b4066c407f53f77a"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MIP_SHIFT = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3828) | 当前常量或枚举值：`10`。 | — |
| <a id="api-0006a57781a3581d"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_RESERVED_SHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3831) | 当前常量或枚举值：`28`。 | — |
| <a id="api-250d4bf7a47bc8ea"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_U_SHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3823) | 当前常量或枚举值：`0`。 | — |
| <a id="api-46ffe328edff07be"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_V_SHIFT = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3824) | 当前常量或枚举值：`2`。 | — |
| <a id="api-4cdd144afac3c084"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_W_SHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3825) | 当前常量或枚举值：`4`。 | — |
| <a id="api-203f36d3179c05ed"></a>[`public const int Inno.Native.Bgfx.bgfx._SHARED_LIB_BUILD = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3803) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9286f8ed7f9f0c2f"></a>[`public const int Inno.Native.Bgfx.bgfx._SHARED_LIB_USE = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3804) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9be693aab02c2f72"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_ALPHA_REF_SHIFT = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3809) | 当前常量或枚举值：`40`。 | — |
| <a id="api-a1e2edd38318c9c0"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_BLEND_EQUATION_SHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3807) | 当前常量或枚举值：`28`。 | — |
| <a id="api-4baadc4c0f5b68b3"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_BLEND_SHIFT = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3806) | 当前常量或枚举值：`12`。 | — |
| <a id="api-e67779b101fb0079"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_CULL_SHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3808) | 当前常量或枚举值：`36`。 | — |
| <a id="api-1dfb97525b6af8c7"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_DEPTH_TEST_SHIFT = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3805) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6956a8ad4f36c2ad"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_POINT_SIZE_SHIFT = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3811) | 当前常量或枚举值：`52`。 | — |
| <a id="api-1bffaa48fa262852"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_PT_SHIFT = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3810) | 当前常量或枚举值：`48`。 | — |
| <a id="api-cbbb10afa3ed84c1"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_RESERVED_SHIFT = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3812) | 当前常量或枚举值：`61`。 | — |
| <a id="api-d96634bff573db39"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_FUNC_REF_SHIFT = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3813) | 当前常量或枚举值：`0`。 | — |
| <a id="api-aa13261ba9ce80dd"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_FUNC_RMASK_SHIFT = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3814) | 当前常量或枚举值：`8`。 | — |
| <a id="api-a564ce60e14b8097"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_S_SHIFT = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3816) | 当前常量或枚举值：`20`。 | — |
| <a id="api-5bc7a542615677cc"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_Z_SHIFT = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3817) | 当前常量或枚举值：`24`。 | — |
| <a id="api-0f3180878aa6a088"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_PASS_Z_SHIFT = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3818) | 当前常量或枚举值：`28`。 | — |
| <a id="api-844dec506124d7a3"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_TEST_SHIFT = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3815) | 当前常量或枚举值：`16`。 | — |
| <a id="api-25e7390e0e22c9d0"></a>[`public const int Inno.Native.Bgfx.bgfx._TEXTURE_RT_MSAA_SHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3821) | 当前常量或枚举值：`36`。 | — |
| <a id="api-2bd84794b613cabe"></a>[`public const int Inno.Native.Bgfx.bgfx._TEXTURE_RT_SHIFT = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3822) | 当前常量或枚举值：`36`。 | — |
| <a id="api-e4c50d79e34a1a75"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(ref Inno.Native.Bgfx.bgfx.InstanceDataBuffer idb, uint num, ushort stride)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5767) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa189a5a078a2f87"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint num, ushort stride)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe6b9285ed125cd9"></a>[`public static uint Inno.Native.Bgfx.bgfx.alloc_transform(ref Inno.Native.Bgfx.bgfx.Transform transform, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1108a3763e9ff12e"></a>[`public static uint Inno.Native.Bgfx.bgfx.alloc_transform(Inno.Native.Bgfx.bgfx.Transform* transform, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6753) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7fde83b5910217b8"></a>[`public static bool Inno.Native.Bgfx.bgfx.alloc_transient_buffers(ref Inno.Native.Bgfx.bgfx.TransientVertexBuffer tvb, Inno.Native.Bgfx.bgfx.VertexLayout* layout, uint numVertices, ref Inno.Native.Bgfx.bgfx.TransientIndexBuffer tib, uint numIndices, bool index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5748) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d7780fd7f3bb7d34"></a>[`public static byte Inno.Native.Bgfx.bgfx.alloc_transient_buffers(Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, Inno.Native.Bgfx.bgfx.VertexLayout* layout, uint numVertices, Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint numIndices, byte index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5741) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4936c6f68c0950f8"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(ref Inno.Native.Bgfx.bgfx.TransientIndexBuffer tib, uint num, bool index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-961ff2be02230e7c"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint num, byte index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5711) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-489b1e2b4ec7b180"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(ref Inno.Native.Bgfx.bgfx.TransientVertexBuffer tvb, uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5732) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63ad27f8fac9ddfc"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5726) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8868171a9c4f5db9"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.alloc(uint size)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5491) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17db06a0bb24830a"></a>[`public static void Inno.Native.Bgfx.bgfx.attachment_init(ref Inno.Native.Bgfx.bgfx.Attachment @this, Inno.Native.Bgfx.bgfx.TextureHandle handle, Inno.Native.Bgfx.bgfx.Access access, ushort layer, ushort numLayers, ushort mip, byte resolve)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5250) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-092d3dc9fe1325fc"></a>[`public static void Inno.Native.Bgfx.bgfx.attachment_init(Inno.Native.Bgfx.bgfx.Attachment* @this, Inno.Native.Bgfx.bgfx.TextureHandle handle, Inno.Native.Bgfx.bgfx.Access access, ushort layer, ushort numLayers, ushort mip, byte resolve)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5244) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7138968811491c0e"></a>[`public static void Inno.Native.Bgfx.bgfx.blit(ushort id, Inno.Native.Bgfx.bgfx.TextureHandle dst, byte dstMip, ushort dstX, ushort dstY, ushort dstZ, Inno.Native.Bgfx.bgfx.TextureHandle src, byte srcMip, ushort srcX, ushort srcY, ushort srcZ, ushort width, ushort height, ushort depth)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6949) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-106c1a0fe1da2358"></a>[`public static void Inno.Native.Bgfx.bgfx.calc_texture_size(ref Inno.Native.Bgfx.bgfx.TextureInfo info, ushort width, ushort height, ushort depth, bool cubeMap, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5899) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cbc4c10b3193e5b3"></a>[`public static void Inno.Native.Bgfx.bgfx.calc_texture_size(Inno.Native.Bgfx.bgfx.TextureInfo* info, ushort width, ushort height, ushort depth, byte cubeMap, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5893) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5911a5a9c118f1c"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.copy(void* data, uint size)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc7023daa6889073"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle csh, bool destroyShaders)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39fbdd32e6cd876d"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle csh, byte destroyShaders)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af76ed7b6f0f2cf0"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer_mem(Inno.Native.Bgfx.bgfx.Memory* mem, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5639) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c0c010ce45bfe468"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer(uint num, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5632) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-236d519af3820295"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer_mem(Inno.Native.Bgfx.bgfx.Memory* mem, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0978bdb2520508c"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer(uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5658) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d0ba29bf53b5d09"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment, bool destroyTexture)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6070) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4dbf067cc16296ff"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment, byte destroyTexture)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5599e8fa4dd5d8bf"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(byte num, Inno.Native.Bgfx.bgfx.TextureHandle* handles, bool destroyTexture)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d5bf53d472b353b"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(byte num, Inno.Native.Bgfx.bgfx.TextureHandle* handles, byte destroyTexture)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6050) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82b8771d0dc14760"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_nwh(void* nwh, ushort width, ushort height, Inno.Native.Bgfx.bgfx.TextureFormat format, Inno.Native.Bgfx.bgfx.TextureFormat depthFormat)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1bf830a53c2931c"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong textureFlags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6043) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0efccb3b8c51ac80"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer(ushort width, ushort height, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong textureFlags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6036) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed04e2bbe9c40763"></a>[`public static Inno.Native.Bgfx.bgfx.IndexBufferHandle Inno.Native.Bgfx.bgfx.create_index_buffer(Inno.Native.Bgfx.bgfx.Memory* mem, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5549) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebd3d4c69f10bba4"></a>[`public static Inno.Native.Bgfx.bgfx.IndirectBufferHandle Inno.Native.Bgfx.bgfx.create_indirect_buffer(uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5776) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6411b05decde4172"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryHandle Inno.Native.Bgfx.bgfx.create_occlusion_query()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b02360fb955b6f6"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle vsh, Inno.Native.Bgfx.bgfx.ShaderHandle fsh, bool destroyShaders)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b922495f96e29193"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle vsh, Inno.Native.Bgfx.bgfx.ShaderHandle fsh, byte destroyShaders)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-436e7e79c40a1426"></a>[`public static Inno.Native.Bgfx.bgfx.ShaderHandle Inno.Native.Bgfx.bgfx.create_shader(Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5789) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f19f760375d719f"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5944) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd52dfb9c94263fc"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b3d5c5aea0ab41df"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d(ushort width, ushort height, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1a34cdc007e078e"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d(ushort width, ushort height, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5924) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54bc803918460306"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_3d(ushort width, ushort height, ushort depth, bool hasMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5957) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33e37475e0cb0f4c"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_3d(ushort width, ushort height, ushort depth, byte hasMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5950) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-607a96a3890b7b50"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_cube(ushort size, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5970) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83a1303f61bcdefb"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_cube(ushort size, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5963) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-679d3226c2a35a09"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory* mem, ulong flags, byte skip, ref Inno.Native.Bgfx.bgfx.TextureInfo info)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea0db0281cf0dbea"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory* mem, ulong flags, byte skip, Inno.Native.Bgfx.bgfx.TextureInfo* info)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5908) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f759bb83f6219b1"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform_with_freq(byte* name, Inno.Native.Bgfx.bgfx.UniformFreq freq, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d36725a96e924aa8"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform_with_freq(string name, Inno.Native.Bgfx.bgfx.UniformFreq freq, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e0ead37f9c917de"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform(byte* name, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6118) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02a6730563f1e5c2"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform(string name, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6125) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-986048fd87e6d6a9"></a>[`public static Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.create_vertex_buffer(Inno.Native.Bgfx.bgfx.Memory* mem, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5597) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b64de9eb17299de"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayoutHandle Inno.Native.Bgfx.bgfx.create_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4fbbcbe3ff6f7002"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_clear(byte attr, bool small)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc6c59370639f89f"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_clear(byte attr, byte small)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5525) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c269d6141643691f"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_image(ushort x, ushort y, ushort width, ushort height, void* data, ushort pitch)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5543) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c330cb276c3eb14"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_vprintf(ushort x, ushort y, byte attr, byte* format, byte* argList)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5537) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-caa5c6c345c932a5"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5652) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e74b16731ea348ed"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5678) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49cd3a162167659c"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_frame_buffer(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6112) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f87790dd91f7ec23"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a15fbfa842197a9"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_indirect_buffer(Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5783) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-773517a02bd9b550"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_occlusion_query(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6208) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75797b2be91f6dae"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_program(Inno.Native.Bgfx.bgfx.ProgramHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5866) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d632036991dbb012"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_shader(Inno.Native.Bgfx.bgfx.ShaderHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5834) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d090ab333494f9fe"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6030) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39bb8218db68567a"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6179) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e66e8d93bc62df56"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5626) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4dc9f7af8f800318"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5591) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-25d2eba666696fc8"></a>[`public static void Inno.Native.Bgfx.bgfx.discard(byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6943) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8663e0a11bdb2b6"></a>[`public static void Inno.Native.Bgfx.bgfx.dispatch_indirect(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33f6314e69393327"></a>[`public static void Inno.Native.Bgfx.bgfx.dispatch(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint numX, uint numY, uint numZ, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-010166f084f040d5"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder @this, ref Inno.Native.Bgfx.bgfx.Transform transform, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6418) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e3ec687bc065300"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.Transform* transform, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6411) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4969ee82fcbd511d"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.encoder_begin(bool forceNewEncoder)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-499d76343dba5fc1"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.encoder_begin(byte forceNewEncoder)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13349a233d96f685"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_blit(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.TextureHandle dst, byte dstMip, ushort dstX, ushort dstY, ushort dstZ, Inno.Native.Bgfx.bgfx.TextureHandle src, byte srcMip, ushort srcX, ushort srcY, ushort srcZ, ushort width, ushort height, ushort depth)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6619) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c07fb6571af5639"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_discard(Inno.Native.Bgfx.bgfx.Encoder @this, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6613) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76500d8e39a173da"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_dispatch_indirect(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6607) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9450d699b6eb623b"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_dispatch(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint numX, uint numY, uint numZ, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d18eb4d580d70db8"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_end(Inno.Native.Bgfx.bgfx.Encoder encoder)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-271f96912d94e703"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6577) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-463505e0fbe32da2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-745f2a55fa71ed53"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6565) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e30e91f95e8e9e2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_indirect_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5dd088176dc20fa"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6571) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89177791febfc42a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, bool visible)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6373) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76ef72340ec69077"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, byte visible)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6367) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc084b644481c619"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6451) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-368f9646f655023c"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bad8ca1eca175265"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6475) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bb88528405f2d8ac"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_image(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, Inno.Native.Bgfx.bgfx.Access access, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6595) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-133154351cc50d7f"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6445) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-500a796291b281ec"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_count(Inno.Native.Bgfx.bgfx.Encoder @this, uint numInstances)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6523) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8ace63d23de5fb7"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint start, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-493ef198fd096a42"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ac8dcf05b05617c"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6511) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8721835a30804b7"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder @this, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6339) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36d1025ed4e67886"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder @this, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cbcb1dcbaa6085d6"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_scissor_cached(Inno.Native.Bgfx.bgfx.Encoder @this, ushort cache)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6392) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b9e8cab1a32cf07e"></a>[`public static ushort Inno.Native.Bgfx.bgfx.encoder_set_scissor(Inno.Native.Bgfx.bgfx.Encoder @this, ushort x, ushort y, ushort width, ushort height)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6385) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4cf8a2f1116a60a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_state(Inno.Native.Bgfx.bgfx.Encoder @this, ulong state, uint rgba)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-962d655aa954859a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_stencil(Inno.Native.Bgfx.bgfx.Encoder @this, uint fstencil, uint bstencil)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6379) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33bdeaccb55319d9"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_texture(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.UniformHandle sampler, Inno.Native.Bgfx.bgfx.TextureHandle handle, uint flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6529) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e86092ebbd81ddd3"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transform_cached(Inno.Native.Bgfx.bgfx.Encoder @this, uint cache, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6405) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-336f76db9e11bb93"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_set_transform(Inno.Native.Bgfx.bgfx.Encoder @this, void* mtx, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74697adec1f59bae"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6457) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a56449ef249e45d"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e18da5585963bd3"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6487) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c93123f051e73d2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_uniform(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6427) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef4b4d0a9425c856"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6469) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2f0f23f7d9d2b294"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6cd226cadc92d6f"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_count(Inno.Native.Bgfx.bgfx.Encoder @this, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ef0d1fea0f5c455"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_indirect_count(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, Inno.Native.Bgfx.bgfx.IndexBufferHandle numHandle, uint numIndex, uint numMax, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6559) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b63559dfb9a0699"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_indirect(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c61a002fdf29823"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_occlusion_query(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle occlusionQuery, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6547) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ff5f55912860b4b"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6541) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a28377be1323788d"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_touch(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6535) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ac10fbd4f920963"></a>[`public static uint Inno.Native.Bgfx.bgfx.frame(byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-223bec0ded89112c"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_instance_data_buffer(uint num, ushort stride)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5704) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ffa7fa3247b466f"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(uint num, bool index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5691) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bed4a004003ebe0e"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(uint num, byte index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13a7ceca503281b5"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_vertex_buffer(uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5697) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c4bc26a5f3c1fc7"></a>[`public static Inno.Native.Bgfx.bgfx.Caps* Inno.Native.Bgfx.bgfx.get_caps()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5477) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-64800a88c23328f1"></a>[`public static void* Inno.Native.Bgfx.bgfx.get_direct_access_ptr(Inno.Native.Bgfx.bgfx.TextureHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6023) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cb8cbf0649bf7de"></a>[`public static Inno.Native.Bgfx.bgfx.InterfaceVtbl* Inno.Native.Bgfx.bgfx.get_interface(uint version)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6955) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-afd273a36592e54c"></a>[`public static Inno.Native.Bgfx.bgfx.InternalData* Inno.Native.Bgfx.bgfx.get_internal_data()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6660) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30a599b1f2d9a37d"></a>[`public static byte* Inno.Native.Bgfx.bgfx.get_renderer_name(Inno.Native.Bgfx.bgfx.RendererType type)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5422) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b276d01ffb294f97"></a>[`public static Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.get_renderer_type()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5470) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92f375261670e3f6"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryResult Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, ref int result)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6199) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e84a1f51854d412a"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryResult Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, int* result)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8bb31878a9dcc228"></a>[`public static ushort Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle handle, ref Inno.Native.Bgfx.bgfx.UniformHandle uniforms, ushort max)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5803) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcfd39e2dd527a5d"></a>[`public static ushort Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle handle, Inno.Native.Bgfx.bgfx.UniformHandle* uniforms, ushort max)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5796) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9dcf2ffe09fcade"></a>[`public static Inno.Native.Bgfx.bgfx.Stats* Inno.Native.Bgfx.bgfx.get_stats()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5484) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a13dda870c8775b"></a>[`public static byte Inno.Native.Bgfx.bgfx.get_supported_renderers(byte max, ref Inno.Native.Bgfx.bgfx.RendererType @enum)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5413) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6c6a8e4716e16bf"></a>[`public static byte Inno.Native.Bgfx.bgfx.get_supported_renderers(byte max, Inno.Native.Bgfx.bgfx.RendererType* @enum)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5406) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63580e06cdda7fd5"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.get_texture(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte attachment)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6105) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca1bc73c3f34e38d"></a>[`public static void Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle handle, ref Inno.Native.Bgfx.bgfx.UniformInfo info)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6170) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e2f8160fef18336"></a>[`public static void Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle handle, Inno.Native.Bgfx.bgfx.UniformInfo* info)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6164) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8606ba3efde35831"></a>[`public static void Inno.Native.Bgfx.bgfx.init_ctor(ref Inno.Native.Bgfx.bgfx.Init init)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5435) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-982d6938f60dae60"></a>[`public static void Inno.Native.Bgfx.bgfx.init_ctor(Inno.Native.Bgfx.bgfx.Init* init)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5429) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c73b6ecbd456086"></a>[`public static bool Inno.Native.Bgfx.bgfx.init(Inno.Native.Bgfx.bgfx.Init* init)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5444) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dbbb40570568e998"></a>[`public static bool Inno.Native.Bgfx.bgfx.is_frame_buffer_valid(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e714c0c65a4eb1c2"></a>[`public static bool Inno.Native.Bgfx.bgfx.is_texture_valid(ushort depth, bool cubeMap, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5879) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de4f9af2a0f96a36"></a>[`public static byte Inno.Native.Bgfx.bgfx.is_texture_valid(ushort depth, byte cubeMap, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5872) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2daf849959e1dd21"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.make_ref_release(void* data, uint size, void* releaseFn, void* userData)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5512) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3fcd801d438711fd"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.make_ref(void* data, uint size)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-491db3c91d92d1f6"></a>[`public static ulong Inno.Native.Bgfx.bgfx.override_internal_texture_ptr(Inno.Native.Bgfx.bgfx.TextureHandle handle, ulong ptr, ushort layerIndex)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53aac9599cd7d0d6"></a>[`public static ulong Inno.Native.Bgfx.bgfx.override_internal_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort width, ushort height, byte numMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e9735dec716185d"></a>[`public static uint Inno.Native.Bgfx.bgfx.read_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle, void* data, byte mip)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5994) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fdd3bce9915a7af"></a>[`public static Inno.Native.Bgfx.bgfx.RenderFrame Inno.Native.Bgfx.bgfx.render_frame(int msecs)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6647) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e2eb0b0a46862a2"></a>[`public static void Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte* filePath)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e2872f8bd5381cb"></a>[`public static void Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, string filePath)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f350c90c5907aefe"></a>[`public static void Inno.Native.Bgfx.bgfx.reset_view(ushort id)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8581dc7fd036db18"></a>[`public static void Inno.Native.Bgfx.bgfx.reset(uint width, uint height, uint flags, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5457) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-646ed76981f18a6b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_dynamic_index_buffer(byte stage, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6907) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e74df60e6a48716b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_dynamic_vertex_buffer(byte stage, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6913) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-753a07448c149bcc"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_index_buffer(byte stage, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-daa16e2b12448b71"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_indirect_buffer(byte stage, Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6919) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e1920c1dbda625d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_vertex_buffer(byte stage, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6901) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51ff163428f08c26"></a>[`public static void Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, bool visible)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6715) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96bab17bce8b6e08"></a>[`public static void Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, byte visible)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6709) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c0f204f1bd3b3b6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_debug(uint debug)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5519) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca2aefd308b07964"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6781) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9622812d37952e94"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6811) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b01cdd54730470c"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6805) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e84f294a31f7f7f"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6083) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f2ddc9a3486d92c"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6089) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95bfd79789c5b51a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89949b891bba47fc"></a>[`public static void Inno.Native.Bgfx.bgfx.set_image(byte stage, Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, Inno.Native.Bgfx.bgfx.Access access, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1accec77f97d79b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5556) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-597f2f70d8dd4265"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5562) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c55fcc8ca0c06a5a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6775) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd1f21c1a61913dd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_count(uint numInstances)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e09a7be538fa71ad"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint start, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a4ae6d3e06730f9"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1435bc3f813ad994"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6841) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59a96dbdccc61080"></a>[`public static void Inno.Native.Bgfx.bgfx.set_marker(byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6681) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c4d3ef39b5f09f47"></a>[`public static void Inno.Native.Bgfx.bgfx.set_marker(string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6687) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddb575840923ccb7"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color_rgba32f(byte index, float r, float g, float b, float a)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6220) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b6eaa98915748952"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color_rgba8(byte index, uint rgba)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-abc577c13884a5dd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color(byte index, float* rgba)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c2b964e3fd9cc7ee"></a>[`public static void Inno.Native.Bgfx.bgfx.set_platform_data(Inno.Native.Bgfx.bgfx.PlatformData* data)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6654) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b23a412fbf0d0a8"></a>[`public static void Inno.Native.Bgfx.bgfx.set_scissor_cached(ushort cache)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ee75e85b27be4b3"></a>[`public static ushort Inno.Native.Bgfx.bgfx.set_scissor(ushort x, ushort y, ushort width, ushort height)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6727) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81d50f55ea1cfc83"></a>[`public static void Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle handle, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5812) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d1e3ab90c44528b9"></a>[`public static void Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle handle, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5818) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87b10d1dcaa305fb"></a>[`public static void Inno.Native.Bgfx.bgfx.set_state(ulong state, uint rgba)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6703) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b03d1fa939d42a4"></a>[`public static void Inno.Native.Bgfx.bgfx.set_stencil(uint fstencil, uint bstencil)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6721) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ece3f9f8a5531ba8"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle handle, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e4e3a0394f9215a6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle handle, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6007) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f6307965c7263fd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture(byte stage, Inno.Native.Bgfx.bgfx.UniformHandle sampler, Inno.Native.Bgfx.bgfx.TextureHandle handle, uint flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6859) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db074d2f292cdf10"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transform_cached(uint cache, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6747) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ec7c9af703b01e07"></a>[`public static uint Inno.Native.Bgfx.bgfx.set_transform(void* mtx, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6740) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d0530019946876d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint firstIndex, uint numIndices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6787) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e22459a4209f6ec7"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e99ead3e822e593"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6817) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b63390901df5d69e"></a>[`public static void Inno.Native.Bgfx.bgfx.set_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f105ed9190aea811"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5dda862139013950"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5610) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-054b90c51db42b41"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6799) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df9ff2bc9ec227d3"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10727debc3d2e21d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_count(uint numVertices)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6829) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c27c6f3945d8f03"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_clear_mrt(ushort id, ushort flags, float depth, byte stencil, byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6278) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6393b51ff5851146"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_clear(ushort id, ushort flags, uint rgba, float depth, byte stencil)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6272) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f1d3ac33def335d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_frame_buffer(ushort id, Inno.Native.Bgfx.bgfx.FrameBufferHandle handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6290) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b288e0f55166dd94"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_mode(ushort id, Inno.Native.Bgfx.bgfx.ViewMode mode)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6284) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-198c9ed3e49c5806"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_name(ushort id, byte* name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6232) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6d3cc80f7013d4a6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_name(ushort id, string name, int len)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6238) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82c3fe64cbcf6dca"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_order(ushort id, ushort num, ushort* order)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6302) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7868ede7d7ddd518"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_rect_ratio(ushort id, ushort x, ushort y, Inno.Native.Bgfx.bgfx.BackbufferRatio ratio)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e12fb6f91ec2477a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_rect(ushort id, ushort x, ushort y, ushort width, ushort height)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6254) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f1fd6c953ddcfb5"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_scissor(ushort id, ushort x, ushort y, ushort width, ushort height)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f95a8a5f01658298"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_shading_rate(ushort id, Inno.Native.Bgfx.bgfx.ShadingRate shadingRate)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6308) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8aa4bf32657d9084"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_transform(ushort id, void* view, void* proj)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e1da1ccf7636137"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_uniform(ushort id, Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57c7ec2cf8cb7dd5"></a>[`public static void Inno.Native.Bgfx.bgfx.shutdown()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5451) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ae82a91e40e0daa7"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_indirect_count(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, Inno.Native.Bgfx.bgfx.IndexBufferHandle numHandle, uint numIndex, uint numMax, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6889) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9bd3e7a1e766f8ed"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_indirect(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6883) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f41f1626f8380cd"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_occlusion_query(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle occlusionQuery, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6877) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8fb2f93d0daec9b"></a>[`public static void Inno.Native.Bgfx.bgfx.submit(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint depth, byte flags)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6871) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5dc2d411ff512a29"></a>[`public static uint Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert conversion, void* dst, uint dstSize, void* indices, uint numIndices, bool index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29dc825998a36386"></a>[`public static uint Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert conversion, void* dst, uint dstSize, void* indices, uint numIndices, byte index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5381) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a43450fa60b810a9"></a>[`public static void Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort sort, void* dst, uint dstSize, float* dir, float* pos, void* vertices, uint stride, void* indices, uint numIndices, bool index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5400) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e13b17307c4810ed"></a>[`public static void Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort sort, void* dst, uint dstSize, float* dir, float* pos, void* vertices, uint stride, void* indices, uint numIndices, byte index32)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5394) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa8e4fcb10ea14f4"></a>[`public static void Inno.Native.Bgfx.bgfx.touch(ushort id)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77b8f7ad043000d6"></a>[`public static void Inno.Native.Bgfx.bgfx.update_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint startIndex, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5646) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-559b9ccfb5760e98"></a>[`public static void Inno.Native.Bgfx.bgfx.update_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5672) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b30b41af41ae4f26"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_2d(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort layer, byte mip, ushort x, ushort y, ushort width, ushort height, Inno.Native.Bgfx.bgfx.Memory* mem, ushort pitch)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5976) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a25d1d2a08994d9d"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_3d(Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, ushort x, ushort y, ushort z, ushort width, ushort height, ushort depth, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5982) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8dffebd73f20cb7"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_cube(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort layer, byte side, byte mip, ushort x, ushort y, ushort width, ushort height, Inno.Native.Bgfx.bgfx.Memory* mem, ushort pitch)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5988) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa578c50cb6e3d74"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_convert(Inno.Native.Bgfx.bgfx.VertexLayout* dstLayout, void* dstData, Inno.Native.Bgfx.bgfx.VertexLayout* srcLayout, void* srcData, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3291bb14a9395f47"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_add(ref Inno.Native.Bgfx.bgfx.VertexLayout @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte num, Inno.Native.Bgfx.bgfx.AttribType type, bool normalized, bool asInt)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5273) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0dfc5bdb6f86709f"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_add(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte num, Inno.Native.Bgfx.bgfx.AttribType type, byte normalized, byte asInt)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3be12eb7901957c1"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_begin(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.RendererType rendererType)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-296db424e5446a72"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, ref byte num, ref Inno.Native.Bgfx.bgfx.AttribType type, ref bool normalized, ref bool asInt)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-23d30dd54644177a"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte* num, Inno.Native.Bgfx.bgfx.AttribType* type, byte* normalized, byte* asInt)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5282) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e4033b34404745a"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_end(ref Inno.Native.Bgfx.bgfx.VertexLayout @this)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c552a69ddfce242e"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_end(Inno.Native.Bgfx.bgfx.VertexLayout* @this)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd3607802f1d07ec"></a>[`public static ushort Inno.Native.Bgfx.bgfx.vertex_layout_get_offset(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5335) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9578cc1678ad4d69"></a>[`public static uint Inno.Native.Bgfx.bgfx.vertex_layout_get_size(Inno.Native.Bgfx.bgfx.VertexLayout* @this, uint num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5349) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddd18700d08b624a"></a>[`public static ushort Inno.Native.Bgfx.bgfx.vertex_layout_get_stride(Inno.Native.Bgfx.bgfx.VertexLayout* @this)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5342) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba0857acdaa95727"></a>[`public static bool Inno.Native.Bgfx.bgfx.vertex_layout_has(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1977b9ee0a34a63"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_skip(Inno.Native.Bgfx.bgfx.VertexLayout* @this, byte num)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d8aa907653b20db"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_pack(float* input, byte inputNormalized, Inno.Native.Bgfx.bgfx.Attrib attr, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint index)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5356) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aaafb8602c478b9e"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_unpack(float* output, Inno.Native.Bgfx.bgfx.Attrib attr, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint index)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5362) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea090d86c0e9b5f0"></a>[`public static uint Inno.Native.Bgfx.bgfx.weld_vertices(void* output, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint num, byte index32, float epsilon)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5374) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Access</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Access d4f57c02368eaeabb4fbf1c0194d827ab1941a085c59ddb97f7a605509a5e20e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Read 337d20f939da6df8e10cf34dd1f6e8b261833cf46307601cd6cfb9eae1c32147 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Write 8d38c84cf027cfee8099e9aa368c70934d0828be10d44d880ec81d536b8aeb41 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.ReadWrite 71e986dd343a1126b27b6497fa0a2c35aed45eacb7b76a92d0bfebf9b8cfff87 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Access.Count 0bfc35a495ba091b364a5ab3d3a4a13a3f80a0cc11c31567ea1ff35577ea49dc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b62c66249e25b0dd"></a>[`Inno.Native.Bgfx.bgfx.Access.Count = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L67) | 当前常量或枚举值：`3`。 | — |
| <a id="api-978c23ff4e03f669"></a>[`Inno.Native.Bgfx.bgfx.Access.Read = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L64) | 当前常量或枚举值：`0`。 | — |
| <a id="api-93970281ade43b49"></a>[`Inno.Native.Bgfx.bgfx.Access.ReadWrite = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L66) | 当前常量或枚举值：`2`。 | — |
| <a id="api-0e654b97e9f88a0d"></a>[`Inno.Native.Bgfx.bgfx.Access.Write = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L65) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3ea31ac55ae50695"></a>[`public enum Inno.Native.Bgfx.bgfx.Access : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L62) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.AllocatorInterface</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.AllocatorInterface 2f6990b229fac9c223cc41e52e7b90b0fe5e0d4cb4a03995f92d1583f8dd0ab1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl ca2734dbbc1049d2b0a1a8d267b82ac1368740abc74c8a03144ac5b4b5806f36 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.AllocatorInterface.#ctor(Inno.Native.Bgfx.bgfx.AllocatorVtbl*) c1f6573176fd7fffefedc5dc475196fb42b6d2b6f5726eec73c890ffcc839918 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9af84d37db2c4edd"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorVtbl* Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f21e94d43a468a4"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorInterface.AllocatorInterface(Inno.Native.Bgfx.bgfx.AllocatorVtbl* vtbl = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L644) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c860e42f4f8fd2cf"></a>[`public struct Inno.Native.Bgfx.bgfx.AllocatorInterface`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L641) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.AllocatorVtbl</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.AllocatorVtbl bb1c39160c1ae3af1b44e3156ebab8888021966056b07dfb60a7bd2db7e11b3c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc e3417ea53e7daf30ad9ad53738b8ba0d570a5eac9895d31452fede2831fab9ce -->
<!-- api: M:Inno.Native.Bgfx.bgfx.AllocatorVtbl.#ctor(System.Void*) 0c30dcf9ff5b2df3a94593d7d3e9e07f5fb155b46bd6996c525fba2dfd5a523a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b9c20ec37d6c5041"></a>[`public void* Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7aeb407d0d95c0ca"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorVtbl.AllocatorVtbl(void* realloc = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L628) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91c24f218ac3d147"></a>[`public struct Inno.Native.Bgfx.bgfx.AllocatorVtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L625) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Attachment</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Attachment 1beafaf64bfc6cbb3ec37cb3c80b241705b421fce70a039e654e872060a56249 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.access 82cb154ca0320f82921e54939cc71945b2211720287f66555ec81bbfea639f99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.handle 7df1d393da4351432c4e939b99eaeb20b282ebe3a8f4b6bb38142c42c299078e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.mip 709f05611fb955372e0d3a04d7c8e710b3f88322fe4782e76607e15c78e8231f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.layer 3ff98afce8f96b3baf3ff94be10347645133457afc8b483bf48492cb9882a363 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.numLayers f06e1c6a10592e1ea868d4e3387cd641abc7dc1d943f47cf0c570b33f75cbed6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attachment.resolve 29d113751008fe7b0e2b1f656774ffee4cc3fcb41aeed1040fa525646e19154a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Attachment.#ctor(Inno.Native.Bgfx.bgfx.Access,Inno.Native.Bgfx.bgfx.TextureHandle,System.UInt16,System.UInt16,System.UInt16,System.Byte) d8c08d26134a93c46d3c6a0a3571cedba3447d79e62d388ad8905811dea5a208 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ca56734cc6dcac55"></a>[`public Inno.Native.Bgfx.bgfx.Access Inno.Native.Bgfx.bgfx.Attachment.access`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1148b21562ec97aa"></a>[`public Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.Attachment.handle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad14c64cd6fac96c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.layer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a11dbfc3fcbcb23"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.mip`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d873aeec6fa9a569"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.numLayers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f175347be0a50ec2"></a>[`public byte Inno.Native.Bgfx.bgfx.Attachment.resolve`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dcd3b7d31a0695d"></a>[`public Inno.Native.Bgfx.bgfx.Attachment.Attachment(Inno.Native.Bgfx.bgfx.Access access = Inno.Native.Bgfx.bgfx.Access.Read, Inno.Native.Bgfx.bgfx.TextureHandle handle = default(Inno.Native.Bgfx.bgfx.TextureHandle), ushort mip = 0, ushort layer = 0, ushort numLayers = 0, byte resolve = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L683) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48cb5b66777519a0"></a>[`public struct Inno.Native.Bgfx.bgfx.Attachment`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L675) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Attrib</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Attrib d17830a934214e0871c35132988914e65eb1772b663faade8bd2c8397850adc1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Position 13aed34c18d87eb76d480a65948bd3abd7a25511e374d34c670fdfc85ac9ddff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Normal 53e22d735a37da1d39e4f058190d628e7a7c08e7ee2c9f34cbd5cfcc699d89fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Tangent a31a302c0c55dea2ac58eee84a981704c6d01468f2882398c6fd8b5abc9471a4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Bitangent 326e286e8bdba3ed131f0aca4ff08738eb4fa5ac384e4889efd96b0ef95e9fd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color0 803241a00e9c6e97c2723cf892c2dbcedebabf0c54a74b82f89178f29abf09bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color1 7564c1931a6058afd3b98cf292021318b48d235629d9aabb6a8317f1ffadbd0a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color2 35ff6e122e43108a5bec50e94f67e4c17e90a82248125c3266536e757460be03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Color3 283e8ef819ec8820512db7ce17690baea882f6e3878d2151e250331eb09907ae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Indices 25687869149130405c60072a191c93902ea9df82b6e97318fdd8b3023fc04c06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Weight 1bf74d17915ce8d1735cdcc2e5ada2a5ed58524747cf5f8c51470ef963915832 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord0 b8a301d17ef985b8d92d0cfc78ced4f13a965a221b4200af249d72ec917115b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord1 44158fa789152cb1355377a2646f9af961b4d9aab638732b9ce85aa80943f1a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord2 349373569d7b1e48505b155712bd9682b9054b8c051c688f8c68ef3cf1a0ebe4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord3 088148090f94489ebab3b96c71f9cda0491cc3710c5fcea94a6e7abd9fef8197 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord4 4595cc98f39d28fe28b549ec52f846c72d93fd4e58b80061324105eb19f77111 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord5 697fa601ff2f58c2435ac7ae827834e1e43cd3edab7604f089914cba8259b1b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord6 dbd0e499be59bfbb1fb021463354955a48ff2679c803ee7f6d3c2b03c09430bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.TexCoord7 b386256587d02f4c575ae18c8739e5b000047557b0542fd2528d738a84d247b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Attrib.Count 803e1d8db641c042fb7137f70c53fc9d8a4ccba0dcee97cd7411886c5b3b8d8d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7f3b58444579011b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Bitangent = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L81) | 当前常量或枚举值：`3`。 | — |
| <a id="api-83cc69b64191f061"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color0 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L82) | 当前常量或枚举值：`4`。 | — |
| <a id="api-ef1b9b473978fc66"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color1 = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L83) | 当前常量或枚举值：`5`。 | — |
| <a id="api-71914f44c802e31d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color2 = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L84) | 当前常量或枚举值：`6`。 | — |
| <a id="api-c19f9c0d38668589"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color3 = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L85) | 当前常量或枚举值：`7`。 | — |
| <a id="api-f78ad44d0d995c5f"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Count = 18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L96) | 当前常量或枚举值：`18`。 | — |
| <a id="api-dedda80f71a7c53c"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Indices = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L86) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3050f8cea02ab541"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Normal = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L79) | 当前常量或枚举值：`1`。 | — |
| <a id="api-25edc5225dd95950"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Position = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L78) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1b95ff9cd2601e91"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Tangent = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L80) | 当前常量或枚举值：`2`。 | — |
| <a id="api-9be37662c648364b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord0 = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L88) | 当前常量或枚举值：`10`。 | — |
| <a id="api-b5248d0062dd4fda"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord1 = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L89) | 当前常量或枚举值：`11`。 | — |
| <a id="api-f2e0dd75b746e2bf"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord2 = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L90) | 当前常量或枚举值：`12`。 | — |
| <a id="api-80c6003126e86973"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord3 = 13`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L91) | 当前常量或枚举值：`13`。 | — |
| <a id="api-6404af589ea696cd"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord4 = 14`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L92) | 当前常量或枚举值：`14`。 | — |
| <a id="api-234ad6f63cbe637d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord5 = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L93) | 当前常量或枚举值：`15`。 | — |
| <a id="api-231e702c881a9a3b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord6 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L94) | 当前常量或枚举值：`16`。 | — |
| <a id="api-b45dd98d310d6fdc"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord7 = 17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L95) | 当前常量或枚举值：`17`。 | — |
| <a id="api-152e091f7bde2453"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Weight = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L87) | 当前常量或枚举值：`9`。 | — |
| <a id="api-f6b86a9ed3b8ddfa"></a>[`public enum Inno.Native.Bgfx.bgfx.Attrib : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L76) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.AttribType</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.AttribType be1174853651ea5c15afd37f9cf0a64e1a2753eb3a294793cc2d7eb3197f0237 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Int8 57c42b036b8e829cd24fd9d4ddc1447bfa575b1101710cd88c426528dc3e2e59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint8 784f8de6bcc3daa22e3fbc6773bda10fafe466931f59f049599eff59924a214c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint10 d1e9debc525d61da9be88917aaf1415f14f893aa0c303ed83f9c5cb9fcedc96e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Int16 8c80f6b16bdb4e4732fcfc906411ef41801e24cdd2b73ceecc28e37282e45ef9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Uint16 c3d11dfec20fed06283c622ec53dc0d6ab0ec284f565d441493909efe929d261 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Half 442aeaf0f9bec4e09367a730ada38aeaee317cf1a442518d758ce115ee42fd6f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Float bf67345863359d77c53a534f734171532bdecbb830392b03f9327c175e7ae577 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AttribType.Count b2b9a57b915278b1fbb3c9636382a7944f84d30c996dab2c1e9e8be74b3d0acc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f3c271624c59b3c0"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Count = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L114) | 当前常量或枚举值：`7`。 | — |
| <a id="api-3955ddb4efb4fd83"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Float = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L113) | 当前常量或枚举值：`6`。 | — |
| <a id="api-3999bfa16c8207e6"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Half = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L112) | 当前常量或枚举值：`5`。 | — |
| <a id="api-180b8c2651880ab5"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int16 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L110) | 当前常量或枚举值：`3`。 | — |
| <a id="api-585bd9a6f2b531a2"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int8 = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L107) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5c80a9880f92726f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint10 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L109) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7efed06fad7f93f9"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint16 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L111) | 当前常量或枚举值：`4`。 | — |
| <a id="api-322e43a871605c2f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint8 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L108) | 当前常量或枚举值：`1`。 | — |
| <a id="api-efe291254e8ede9c"></a>[`public enum Inno.Native.Bgfx.bgfx.AttribType : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L105) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.BackbufferRatio</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.BackbufferRatio 721dc3f40c5aeff0580ce06c52f4d90d42c9e037c6127a180b5eddf72d294c5a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Equal de9ffbccbf5147f6dbeff769e9ea40ead98e44553590bdb55333b35d46679722 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Half 718e3a2e9123af7727372fac2289466701f7d00312a512c504f79ced08fe2907 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Quarter 6a799e2908b8d62bee6e08e5ea101ffcf99af2be4d91a4a814c31ca02dc909f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Eighth c2e6fabc09bdd1dab31259c38dd7f828500209ec25ede2ac7fb9872c3b2555cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Sixteenth 86f041d159545ccc774e84212aabd34b4110e0cbc81f7790b1d4f81c032996e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Double f1f3151a43f787f4bdcb7d701ae1f2e091cb367d0d146320cd755ba9374cc78b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BackbufferRatio.Count cf908eb5ab9b943c0936b70ff1b3f863e0a0e093b2f6205f248bd8cb8f993190 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5c8f4fcfa05a9c76"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Count = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L272) | 当前常量或枚举值：`6`。 | — |
| <a id="api-a948e60552e1491e"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Double = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L271) | 当前常量或枚举值：`5`。 | — |
| <a id="api-70efe07325dfaba4"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Eighth = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L269) | 当前常量或枚举值：`3`。 | — |
| <a id="api-0ecef3ce8de1fb1b"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Equal = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L266) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cf74ebfa416732e9"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Half = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L267) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e181eef723c57c64"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Quarter = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L268) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fad8b69008226e79"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Sixteenth = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L270) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d554ee7a6af87716"></a>[`public enum Inno.Native.Bgfx.bgfx.BackbufferRatio : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L264) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.BufferFlags</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.BufferFlags e6dc700749c2785bafbb2cf30f31a25b9065074b1fbee91a336f2fb9da5208b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 1c056744b4c71413eab71f6fb0e58b6f802965f637a17e3c3a0ab9a60ed6599a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 0f2304c16ff37196242139ba1ed5d291a225c7642a66d7e0a07cb07de57c57fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x4 65712cfd95cad6d79df85dffb1eee1d74d693006fa6e9aa27c1c7498c1a5e38d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1 10af29ffe62df99a9cf39500883df255d5c66b62332fef3be8a8c70753fa5db2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x2 a23d97af8d6fa62e66aa24b6b387256ec83c56a66ca22a5eac0c16205e78a4db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 5cdd50acad9855b3f93ac0ddb1ca31b2f71bfea95f2946b7cbb6d33fea5a1275 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x1 fb5e31f528475ab44138f27923a1596ca4926e891fbc260033dafd1079b93b2e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2 cc5bd0156b9bb923d781e674108fff7ac114dd93f697d0424ddab4176d4775a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4 5f954c7bfbd822c60484349865aca939956f4d9def3dda66b0a784232b2d618d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatShift 4a27d32c200da8444803266effffa7806bc08d93260b2780c642e9faa51cbf8f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatMask 0f28aa8770d7c9084d64efa3e99ddd838836aa47b69d825a1f6875b009522a69 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt 5d1666f582662ef559fe4e1db8367e9b7c5008235a7ed5d00b382d68a113ee85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint ef227e2aef4bd88cf9d586577f6fb561954f834e096cab8cf4b760951e30ff21 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeFloat e2de7b43b8bce0d9ba40a9c70f2ac7308229b699d8879ae894165ba8094a0586 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeShift 110d1f477bf3ff1271ba907dc9518d6499fdda98e876f024f79bf0014f55b327 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeMask f0f24151180788c96a27c60c3b95be9de59683cf16db9c1b68115d6d265e1396 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.None 9981691488de1c28f8d65873013673cc11a757b9049b6d98af9eab81b48a8d4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead 1cd8cf0763094dbfad315728fdba6a87b1cb9586e77ebf4d6e79344f98506bae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite 10175c3cf3b2f73ff39debcf49a4ee3e2cc1b8a85ec2e27279ba09f09d8751a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.DrawIndirect e3ce8f9353140b2646f29d7cb906bc9539c67fda8d678473adc769bc8d31bded -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.AllowResize 12a6f4c6c1eca467e5c3fbd39f27624ad474ff8f8e4e9ece97779f29d1bbbcd4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.Index32 88fe42b594dc1dda55a74a4dd072d909bd7b819953b921c035c5f17e9a348e88 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.BufferFlags.ComputeReadWrite 6ae4d0f28b35d349672dbe8a9505c037f96c1f3207a0fc5168e5adebc898e427 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d16ff06e47e6bfb0"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.AllowResize = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3340) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-35524ceb14ef725c"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1 = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3323) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3f100aa4a0a2cf2a"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x2 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3324) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`。 | — |
| <a id="api-1f02876e33b59889"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3325) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`。 | — |
| <a id="api-0dee05c3761245cc"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x1 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3326) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4`。 | — |
| <a id="api-6f89f2d378e029c1"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3327) | 当前常量或枚举值：`8`。 | — |
| <a id="api-a558dcea60f402b5"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3328) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2`。 | — |
| <a id="api-e531f64700675d48"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3320) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9a84a0c7acddfd5d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3321) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fab09fc8a824a9c9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3322) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2`。 | — |
| <a id="api-ad55712fc49fd9a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3330) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4`。 | — |
| <a id="api-72d65506ec1bf622"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatShift = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3329) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8644fe2002d3bb60"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3337) | 当前常量或枚举值：`256`。 | — |
| <a id="api-13b20fa6c0a26d52"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeReadWrite = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3342) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite`。 | — |
| <a id="api-88ca145caba0ef2d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeFloat = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3333) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`。 | — |
| <a id="api-cc887928149ca9a9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3331) | 当前常量或枚举值：`16`。 | — |
| <a id="api-f414028f82735d2f"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3335) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`。 | — |
| <a id="api-ad82f9f91b4f00c4"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeShift = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3334) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3cda6dc3c68cb37b"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3332) | 当前常量或枚举值：`32`。 | — |
| <a id="api-95324ba48e71a2a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3338) | 当前常量或枚举值：`512`。 | — |
| <a id="api-ef5ee455c7d332f9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.DrawIndirect = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3339) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-4e4675979e6df7cf"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.Index32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3341) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-d743ad3cdd347b86"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3336) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ad7d0c1ee74cff78"></a>[`public enum Inno.Native.Bgfx.bgfx.BufferFlags : ushort`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3318) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CallbackInterface</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CallbackInterface 3e5ee07ecd943a967b1052357fbbba992e0254201f248479e6c1c0b498753e95 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl fa98a95c9ccedd70a7753910f9f1b33aeca9e3bafd64a7156fbc9ba3ede00fcf -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CallbackInterface.#ctor(Inno.Native.Bgfx.bgfx.CallbackVtbl*) 1be9a79dc12f23e745351afe3b9da298e585e9d2166770acd442e51e57c05d11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5ff34dd77421c952"></a>[`public Inno.Native.Bgfx.bgfx.CallbackVtbl* Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f99b560205f0e99"></a>[`public Inno.Native.Bgfx.bgfx.CallbackInterface.CallbackInterface(Inno.Native.Bgfx.bgfx.CallbackVtbl* vtbl = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L925) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d99b0c4ef9192227"></a>[`public struct Inno.Native.Bgfx.bgfx.CallbackInterface`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L922) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CallbackVtbl</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CallbackVtbl 880a3adf4646a3b395a2fa317a1a2585c77edd5d767371820ad3c539c200cd53 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.fatal 291de638979de866f337f8c8938c9735fdcf1cce54fbc6424c878bfb59771a46 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.trace_vargs d8d4c090cc2e12df6e4999520058b6e3e73777f861617491abcb5143692a068f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin 13089c72a7529d23c6d2b90e71a523ea3f8203791162bdff9663eaea6b65768c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin_literal d2bb37eb4a0cd9432bebde390e31dfe0ec0c1a33aa530890dc5bf85b139f5c2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_end c96944a327b0fb487c3c96b4d2d5569c92cdfa0b193269abb6dfa940c21148e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read_size 5e87059658a91f687ee8e4a43184f029e97e2bdcfa9f7aac1320a665a174ed2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read bd20a314de6d165a3385c72f62750c3c8cb26de418490db19e5a9682bf48755d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_write f6e8c40cd363913affd1b838f814ec088b50a31cac7d36e1197eaada237afc4d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.screen_shot c1edd14354ea041f0d36200e06a6bc7b8cd97c01df4cecb9971a0216428a7cc6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_begin bf51395450f72ef0eb607f8d403cc1ec813b5015a8e3f6d1e2ec5020d1ecafd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_end cab910983d34f3d9490fe06d1c18560776cbc67be044cde3c3a7fcaf523bac01 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_frame 1a12454f2a167c44aee9ef5e167185e6bca071094c0940c8e492aaac276f87f8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CallbackVtbl.#ctor(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*) 701594f034ccd311fa12f758ddf503696b79bb29c06a54ea60dc4a1cae4490ab -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d73e6fc6d2063631"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fe3bca2f9e213d5"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read_size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f330c2e702cfe82"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_write`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-758c86e5a291947e"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ec5437498d66d56"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94d832cfdc5c7b31"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_frame`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-052aa9071b6842ec"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.fatal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc4608b49c2581d2"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2217aab42a703df4"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin_literal`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9c3c692326d4c98"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63e6d7ec64a626b7"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.screen_shot`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9780e0e42697f23f"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.trace_vargs`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-752446baa2cc4539"></a>[`public Inno.Native.Bgfx.bgfx.CallbackVtbl.CallbackVtbl(void* fatal = null, void* trace_vargs = null, void* profiler_begin = null, void* profiler_begin_literal = null, void* profiler_end = null, void* cache_read_size = null, void* cache_read = null, void* cache_write = null, void* screen_shot = null, void* capture_begin = null, void* capture_end = null, void* capture_frame = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L898) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1bbdee6bf23a07b5"></a>[`public struct Inno.Native.Bgfx.bgfx.CallbackVtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L884) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Caps</code> · 116 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Caps 7ecf0eba5650f2bae8185de4ac933ead8d400a8813b8abb4a8b1693de4083936 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.rendererType 9fb71805ae9b143422307d5c3966ec61522d09cfecf353d15503641b367abd81 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.supported 8876033cd63d07f53d88bc19cfdd949c7e126efc70efdbf5bff79e7ba009829a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.vendorId 7fab8c53a53a0018b9f8c95062e9e6f7f2c8343123439aaa2111d2cbd29c3d58 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.deviceId ce44e23adf9221d4dd819547427468c31ab30026b6b8d799a6b319530fef9f17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.homogeneousDepth 969593f4db2ac52b1c5b1b2a4308551ae61b74558e9e9ffe9032914515e0f5f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.originBottomLeft eebfc475d12d398c14ec4c305cafff9b41f009da5dc749e6541ecae645db1c16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.numGPUs d4628f2ea688ebb87b898caae22e78f8909fd174cf2399125d7aebd7bd86b662 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.gpu_0 c167999ecd0b3712e09b2456b09b5d2644c34e5ea05023c18e14689b8bcb09d2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.gpu_1 99d7e2eee52e0c6a83d5e8b37c6086456908a9cd90a0d083206ee57aa8e88ed6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.gpu_2 62f55753abc8dde8030c1117ff4e178bb3ae28c3e40f0d5de49e2da6ca19e3d6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.gpu_3 68dd40f11fb63bfa67257ab217750538f8de0b4ea936f08bad77b5bdd5705dd5 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Caps.gpu b5d8780a66c724b954cccba4ccca448871f2dc44b55b67f9df205abbbb6c5f99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.limits 70670eb0f8e93fdde28a3b7fb9acb0884281d80605203f8cf2e215c6ad7ad34d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_0 621f443be5467e43048ec81549e7e4d01bef0a9157fc7d22ed7ece94f89f1ad3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_1 58ee3186911a4f00a23206139c0b426004312bc3980df1f48b4cbfffc59c44d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_2 6aa01f3ea5698b26633c9610481a6f3e5740c7f5344edade7bb52aad8013cf60 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_3 b5f79d320d6ee6b58533035f176aa8f34124ce3d05d9f2d8bf0cabf698d49931 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_4 cdfdd0d925f266fdef10a80391fc08c08d26a2d7856bd8cbf059222f481e3302 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_5 b758d9394d0425a7d334e1df24dda7d89d6234c39b45b6692e1cbf1bf872d88b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_6 67131a5e355b284880c0eaff45f90871311d3856b880c53bfe9915b6bc682432 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_7 78997a5f17901a5676494d0680f6cc881978e6355b66b6704ffbc93ad01e7a01 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_8 642119cbab8326eba5fb181dd94c5ca05b1a9eeb05d6d316f05d7c28c5fa4a54 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_9 3bd04b5604d7a01c4b842b1935991c6a25cd73d07a3e91d251faefab009315e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_10 93ffd4c1729ce38ecaefffe1e384ebea0e4c33b9c27244bbd89583590054d353 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_11 540f9d1b94ada40b1e7482832177a64683c613aee0e7e3577409c732bda0c14c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_12 8ade7afe04cd7e5c9b0b1eef42e54fd67860546a882557152f4a2c001cb5b1c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_13 1b751ac16503457770f8de7c89652f13f1be1f592dc28e904043417964ec56b7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_14 1a98a4fbe28fcf4707ebe4156bf46ae09e846d6dc172485908170cd96a06395f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_15 509902d9d2d5d47a73ea78e5fb8f577d225bf7a34b389623fd63f5ef4da5df2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_16 0a60993b0bcafd24392bfe376874ddc681a6a92de6abf28b98deb436cd230421 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_17 74219d7f827a219645bc8c8380a505c0de274e2a11f37ed544a23b70505835ad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_18 0bd1fd31b4679afac30ec4b0b99e408ad2320b6aab677bc38f077117095f49af -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_19 840b2f06055a9de16786dd8e180a5daaa01b2962a79b8384f910beffe881b4b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_20 a63cd60a82946fdfb7650a94e27c3ff3dfc54e791f97b28eca5d18c0c90cdb53 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_21 a18144a8f8ec66216facecdf54ca844a9a2a0197f1ecd30aec0d92fb2a0f0f3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_22 ce2bbca806bca27c20e462d78fed466bb53fd4c83d999a52104b371a0b03edd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_23 ea4137559bc75f45d6fc55ee94a72351e6fec596958cbcf046e64e135648c3ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_24 f5a022991564346dc93bdada0d4344bf10c9085d5f2e8e41e00d6ef9dce4e7f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_25 e59d433c9f349115fb0dc35b3a62f0635629b99f8b2172ff446354ac0073d6b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_26 7e845fbf926490a158b8cfbb0c7d5b7c882c1343e13f08b94f345e9eb7db934f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_27 ce99a3e9d7e690cc4c666cb1eadc12ae413c735f4524f8a3e10a74cca9b52cbf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_28 5d2eadefbbcd3e6a9dd5dc9f74f79831cb7f30b660861b3750b94a6da8a4c7a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_29 0394b0f44593ac5aadffd04efe64e033f4a7aa4b2fba75063612cbadc0c75cd6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_30 b237dc6346ce6a711d8cb0ee92656096500e207b1f0f987024dfe16173608ac2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_31 9cc73bc8910287db332803f4f8fb09d262cb19bad66f83091143db315532abd7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_32 2380a162448214ebb4bab035f67beaa05586dc45ad46143c67b9db3244ca22de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_33 a3cd872a62d03166f7c94b353e17478b6dccb46ed609e3d1ed91c1fca6856bd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_34 a5c1a15a77a0a19f86f4ec6bc097f547923304a22d123c2513190782f7352794 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_35 fe1b4f669ae3eb8f342febb6be266f8073829a15ec59bcb73a1552c2101763dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_36 1661352a268e7eed0771d113430dec5d3c2d3d0ef1739e9a78ffab2419726582 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_37 ef64d6a18a2f55c430d89c5ec86a59e3b82b59470d3662e7b711c8959132c180 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_38 72924161694517346f8cc1722d7e7ade94adf2249a03e477783fd5a2db8e07c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_39 85980beef41c8632adee3e77371be84fece5f94b36598902580209901dc48bd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_40 80e364245bac006c9c4eedaccf6f7bc674ece1b8062681519f697c91266e0802 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_41 605f1661bc5a99b86105c172f74f6836ce59667b3427bfa8ab1d6b2bc882bc62 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_42 72dc99eb8ac2ea3142521a2d20188568b314f9911546f6d6a4821e080a799cfa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_43 f20a19d3d37ef7aed633ff81668144dc7a72b3d724038e9f07943ae0efef4457 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_44 4179d5eee9520c631a08669a1f43a029c4b67a095bb4ba9d21642535f32dbce3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_45 73e5f10ae39099e6804ccce459222fad3ef15a1c99c61459d7bad9892acfc41c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_46 6a0b4becbcd0af675dbf15853e2f6999e0e655b41c83756c02ea81e1f6d59923 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_47 f73804b2757a9652dcbac2fcc667fc13c25130daeb2bf6cf7fde54b82d28839c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_48 eb3779d938c846ea809ff748b6b62952cf5e2595062409abc77d680bb58429f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_49 0514833065a934e26b6c9857d16dd7cc1988570e485bce66b11fbd8783f26c1e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_50 a142db5323c545f7cd5c83f32b457b018d79635ed426350e30a3e0b820344a43 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_51 660846ada931d02ebc3061b0645987f0525f9a3878a0a0898204a8ea03ad2849 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_52 7ca668bb2707f22b3f282a8a4617fe18c52229329ad910b613d854246fe777b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_53 276e95c9b84128d1d9d0b537ef0ba5279fbc495d306556bbea93e3df7abd71b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_54 287232bdab175d2056658e2e1b62858a6023fc8abfcfe22632bc07b800d79e4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_55 c5f3d56f49db5dca84bf4de4f2033e696919b1cec5c8c4b6cf2dac7cf273f78d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_56 22a545ca5157e46ba0d0afa25a65a6c24f633b14e31893efe6a9921d9255f275 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_57 567bc4ac6c087c5737e68277a1fb56c1d4c332ea185a37091095ec71f924f6d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_58 8014c5cc7c1aecb6201ecc26500b38f2a3191543a08edb25c7c74f91d22575a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_59 ec1f4e298d0ae322a38f8ae58abb1e874e573dc347321ff540ecfb9257e23982 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_60 9bf1559fc331752724af4307edfacc4eadb36a448dd213eef832288656d54cd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_61 c603e520c07641698142b91d5391b54245f09c781b4e33fe808dd3bc850ac46e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_62 3d85cc2818e701b80438c8de95b37e5762d238b712d20b6524f2401104b7c2f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_63 8b6a450d4328e9563538959178d4757a36f5888977953dd3ba07a80bc6f69b17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_64 e918555585a582e6dedcd42074bc0c5ce65affd579af44816445c69d3cd62979 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_65 db6ba9fc72bdac81383cd9a41430fe0f8e5141759b20ba1b2b6160b0801f7406 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_66 e277fbf6fbb6557dd4bb6984dd9978634f5c027229b0585d4ad2536f6ed517cc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_67 97c7580fc0a84878b177fb22b3e72d502e2cbc70fe04dd777664d849fea47a7a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_68 c560a1f61f7d69a3e3f346dd54ccfffb0cbc8d87183a8f0a769e14f61330458c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_69 198efd861686487cee265c22630ee6eb9238661024a2db9487769b4188a865ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_70 86f17144152280e6f8752fc1849eb39798d9e7a395091cf2aca30ebe73617334 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_71 b956607af30372c96d8febff155a2d10d73ddf2b2124d607a9c4e2d96e0766b0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_72 d73224383ff95caf895e5ec4830f0040bb417b570349dea538c212bd329b995a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_73 4277c069dfbb18e5b343e94fe8c4cfa0b6f9c2266177eea80df5d8788e7bf94d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_74 3ce3c8caa2e449b00334a3a0f6576e11a7f47f9c1e7dc6c2b23b777f7549b9bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_75 3467eff887ff7f6df21b48be0f537278a5767248d9dd9b7eef828a4f6ec52b80 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_76 fdd5cacc693090048d9bcc1193149f8a797efda362c39d5a7b7e5f7ea9629b56 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_77 e17d65c98ef3f8eb99b0b3de432b1404fc86f62ab745a67dba2f01a0b817fdc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_78 c5b7a6091a3ba00cf8d2e6e330fea560ee1d4b353f9be47ddfc777a4aa9df98c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_79 b1167e6b313f5f44930014c9040b84453b99da6c48739d61f45f59194cf24a11 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_80 bf3da3c01b162decd5cb12e26c797ad6d39949fd871cd5d91f062693fb98a344 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_81 0a7a225e2652ce6ddaad6ca08ca44d5f96eb8e6104c803875427aab1d4f1c355 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_82 66ac1fffbdfbfb44ddccf0a7e1a969921c2adbb8c3828043c8587aae182d9659 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_83 a5df7a315a270e13cdef276212e0562a600edb946bdf49bcd54318a2e0d43d15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_84 ba87023e20308ca6d1e5f57d51d435a144941cbf5c4067aa599889100f7abb0b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_85 afa1b0579a9e26be02e75014891decc6001232371c7b8e4c5a7b40e5a71848ae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_86 b21ac50f031248678bd7ed16119f91b5da139729b6676eab3459edb0315658e2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_87 244084ecc2f42bd1a9b98fc4dd649c22a9a5778f904aae65dc5ab738b5e6aa10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_88 f89de75e691b410d76a3bfc5691fa07827034d15fea89e1cbe683258ad8ba532 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_89 9b8df3922479c0bebd6b4a7103ec9744fd4e8e399c2ecf654a4b84ea18de3b92 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_90 22c2815e6a447c4db04879afae81d611c67dc23a708ff541cc3661f26f73cca4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_91 67bcf72779cd45b3dcfa96c311a54d278752b0d4bb388773e9f75be25b8a7158 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_92 99e558363b999cbd3936268c0ae0ec83a693f13b115da684d08b93a23b040346 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_93 e9f56e1286393da8cdad8d5c5baf6120ba1430f550cb5bc315b766f68aa2e802 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_94 5c7cabdeeb7c137e9a4366b33acc08ab86540fa5d5b99749f8069d8e4cc85156 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_95 b8b47bdf7a10dcdb656d662408d36ab35bdee9e7bd148436c53dc07b9e381b16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_96 15e1ca2306f1eb70496c3e847901c7bca64e277fdd1bb5f6e82509ba8ed51b50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_97 71b4ab3d04d07f8cb3cb31f5ee7372f40bd62a619c1e6fb8245a235bc3b43aeb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_98 e704dd47e0b3b8e599236317f73a893a1f2593fbd0055338ed41db7a27182290 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Caps.formats_99 074ef79193da0ea1700120c1f9ef5b8b2be1926ab354e5199d51e1d144063dc2 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Caps.formats 50eb110a57b8df5a8f2f3d95b5376a0282639dc6d8be0df624f9a011ab79115f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Caps.#ctor(Inno.Native.Bgfx.bgfx.RendererType,System.UInt64,System.UInt16,System.UInt16,System.Byte,System.Byte,System.Byte,Inno.Native.Bgfx.bgfx.CapsGpu*,Inno.Native.Bgfx.bgfx.CapsLimits,System.UInt32*) 8bfd4b0ed49c226ddf1329f91ab8fa76b2ba224856327ce5dd20bf9c3c72975c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c296b29cbd5cc2ac"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.deviceId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2eeab95923de628a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1efe3550af516b95"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74b0acfb8f0690d5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ef2008b8a96ee3b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99722a32e02f0542"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39f6dcbb9a9d649b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_13`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf067affe7341152"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_14`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5dafd73f90ed15ea"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77980cd72ac45bd0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e3598064e3a97eb5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3751b7aac59d094e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abb9f8596d8ee67a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_19`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49af88ce9c70041f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ddb5bc958e2d2e0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6308e658ce7b1023"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_21`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25937e866997b711"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_22`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1e42acd657e4db7"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_23`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f75a130102855728"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1539e7d1d8570ef5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_25`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a58e46d567074a9e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_26`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-356a82f20a0bc166"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_27`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e9037cd677f05f5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a56e4fd87fb786a7"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_29`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e698b55c944841a6"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f3174868e1134274"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_30`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9421b720700a1abe"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1105) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79e4ff38365491b2"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c8665f5b70bc893"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_33`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1107) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-160d298ae22b00b3"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_34`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fd549a6a4b7f10d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_35`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ee45fac30105876"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa1a1d4c2f4e5411"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_37`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-deecb5972ffbeb3f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_38`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd5351ee27642958"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_39`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-479bee5285805258"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90ba574055eabd2a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c543b307541c6e8"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_41`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-decbcce91ae5b7cc"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_42`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b12fda68e9084700"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_43`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7acc08e58178d0b4"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_44`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-691111d301f32f77"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_45`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8677b1d4786f413d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_46`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4be756d9f97f29f9"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_47`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ab7cf0a952b1290"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c15f55f4d53f75ff"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_49`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f5b0eb9042d02e3"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62c8d649dee52e46"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_50`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c36c198e2e957af"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_51`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed9176c7dec10c23"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e509b4239435f593"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_53`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88631bd486c756b9"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_54`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be5639e5af1a61b0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_55`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ab60cc337ae6f03"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_56`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-306e631082dab874"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_57`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52dea80d554fd363"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_58`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1132) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52bb436affb51088"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_59`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ef99a88fc390c75"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e67764276e0fc963"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_60`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78363d2c7469fa0c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-368e368f65954b38"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_62`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c2a0c64d507f463"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_63`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0b2f529d479e40f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-991e8ae26c0df666"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_65`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3c4dc7ccbd34d06"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_66`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a756ab8cafaec08"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_67`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46a8cc28b08d5899"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_68`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a474da508716913"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_69`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6699ab98ca884eb"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f27061ed2e9b96f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_70`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eaf93e654f8f9a3f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_71`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6e8bcaf2454a465"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_72`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d0587bbcd99596a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_73`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8888fca2cefad658"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_74`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0359cd04893b87c2"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_75`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2eb583e070dae3b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_76`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0849e50af22757d0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_77`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab034a9ef82a5cc5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_78`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c6eb9176a69e821"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_79`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2342dffe76fdea79"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5fb5034940a8390"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_80`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42d286cf11caf5d8"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_81`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b7d35a66bc5baca"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_82`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e9350927fa4358b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_83`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85dcff78dcc55e73"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_84`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22654da813917783"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_85`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3def6419cb4d6f6c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_86`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-591e6287866da913"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_87`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbdd5c2a0767775c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_88`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c18f8d8c5b97b59f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_89`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20123a88c6741b69"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bac3688446edd81"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_90`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad478cf46d79e05e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_91`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e69e09b025d651fd"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_92`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c589da2acc14971d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_93`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-87e6005e795a21d6"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_94`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0da3eba203712b6b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_95`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5be01da7da403d5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_96`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a95d2d8a4e19dc06"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_97`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3440c684eabd5bed"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_98`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f4c8029ef22a17"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_99`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-668c5b0695756329"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61d1b951236b1506"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8920ed901e32fed1"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3f55534107beb5f"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27ddcc19c30a412e"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.homogeneousDepth`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01ba6930ccaec984"></a>[`public Inno.Native.Bgfx.bgfx.CapsLimits Inno.Native.Bgfx.bgfx.Caps.limits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e82b89f0a6c5f5c"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.numGPUs`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07c40e3168520a17"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.originBottomLeft`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38984fcead5c385f"></a>[`public Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.Caps.rendererType`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da3f042458bd7b2d"></a>[`public ulong Inno.Native.Bgfx.bgfx.Caps.supported`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bd06ce1833cfa0c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.vendorId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2527c3aaf1b62559"></a>[`public Inno.Native.Bgfx.bgfx.Caps.Caps(Inno.Native.Bgfx.bgfx.RendererType rendererType = Inno.Native.Bgfx.bgfx.RendererType.Noop, ulong supported = 0, ushort vendorId = 0, ushort deviceId = 0, byte homogeneousDepth = 0, byte originBottomLeft = 0, byte numGPUs = 0, Inno.Native.Bgfx.bgfx.CapsGpu* gpu = null, Inno.Native.Bgfx.bgfx.CapsLimits limits = default(Inno.Native.Bgfx.bgfx.CapsLimits), uint* formats = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1176) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8c2dbabaca5dd55a"></a>[`public System.Span<uint> Inno.Native.Bgfx.bgfx.Caps.formats { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1174) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf1d2a4953a6f851"></a>[`public System.Span<Inno.Native.Bgfx.bgfx.CapsGpu> Inno.Native.Bgfx.bgfx.Caps.gpu { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1071) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6306e998434feb48"></a>[`public struct Inno.Native.Bgfx.bgfx.Caps`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1058) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CapsFlags</code> · 36 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CapsFlags f5c47950cf7bb5348eb9ed344a7c5f6867b163d0ee05c30a524c8506ec271545 -->
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
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureBlit f32fbad176ba23bdddc75beb79ee5e493b8cc1c2ba6c67515ed78bfe2e72298f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal 1690de3f7ae12ad25f2d6e02cafa691a23addcf9fd7e12ea14e83b16bb481d26 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved d164bb2c802f4252c0b5396b1689e4d6f9bec3608cf13d917a7443646eaf5d53 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCubeArray 987a549efb963929b3eb7d8e3ce6953e0de35b00c057de13f559ea582ac6c466 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureDirectAccess 861e58eb89e209200923df2590a40ee32b1e1feea166d2910e59ee4792c9b6f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternal 7d860b15d2e995de01a541c67095a635dba708f598905f9dcd7f114e8cbc1304 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternalShared d5d2b7ac7ce9940fdad51b075a09585d1bea16fcc59e239584ee8f3439df4683 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureReadBack 0fd70a805445315a748ccd4e82b5b2e80293f82dba75c70334837c15605bd67c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Texture2dArray abbc45b89f7fe8dbdcca50c0070e4b406286d2c3cb5500e4ae697c67b8923f76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.Texture3d b582f52ab1fbf53a04b6a4349f2e467e5369ca9c8e1a03701f8280be1714cbb1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TransparentBackbuffer 6e52b40379d0c290542e96549b275dec0422ee5cbf889c30ee016e8a5a21502a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VariableRateShading 775bc728ebca9878586fcb54229f32010f7ea5d6ef9c25a2f0192144433ff15b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribHalf e804b9498411c964f39d163d6900cb4a6e4f85b366686c7153e80cf6ee400519 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribUint10 6b9b66d22872edfb396ec5b1e8c6b15242b33883ed322381cf8289db980181a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.VertexId f9b2b8115c314c6be4b9a247f4dd97ed11f3ce2a9087fd2be782cc135ede8c3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.ViewportLayerArray 1abf26fee3105cddec3bdd410b85877bbcfa49771146daaee728c0cc822c92f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareAll c42b32af74f15d85e18eaa48e0ddd13051e50e65faa9bd3b7d9cbdac48dc14fd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e63ea4ef16117617"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.AlphaToCoverage = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3354) | 当前常量或枚举值：`1`。 | — |
| <a id="api-401a8755f10cbe93"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.BlendIndependent = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3355) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2de09d22b20d1f98"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Compute = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3356) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5a0a336438a36f6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ConservativeRaster = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3357) | 当前常量或枚举值：`8`。 | — |
| <a id="api-b4055a1d16076214"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirect = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3358) | 当前常量或枚举值：`16`。 | — |
| <a id="api-e57030e7f118eb21"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirectCount = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3359) | 当前常量或枚举值：`32`。 | — |
| <a id="api-312245001b7e1e69"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentDepth = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3360) | 当前常量或枚举值：`64`。 | — |
| <a id="api-ef3ede883dd70a50"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentOrdering = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3361) | 当前常量或枚举值：`128`。 | — |
| <a id="api-538e864d124ff618"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.GraphicsDebugger = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3362) | 当前常量或枚举值：`256`。 | — |
| <a id="api-9c92174cf69172cf"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hdr10 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3363) | 当前常量或枚举值：`512`。 | — |
| <a id="api-2b5801bf1936d4ac"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hidpi = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3364) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-35516eaa769e8cce"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ImageRw = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3365) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-bb16ca8edd5052a1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Index32 = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3366) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-e38858d52d69835a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Instancing = 8192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3367) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-9367d7bd208d7b1e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.OcclusionQuery = 16384`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3368) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-5247949a958e3906"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.PrimitiveId = 32768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3369) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-2f7bfd3aebf66c43"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.RendererMultithreaded = 65536`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3370) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-bf73b1f637ab3738"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.SwapChain = 131072`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3371) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-706d2f8ec1db53d1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture2dArray = 67108864`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3380) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-8f62f47327b0354a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture3d = 134217728`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3381) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-196fa754a77a7caa"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureBlit = 262144`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3372) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-294ebbd947b1b25d"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareAll = Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal &#124; Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3388) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal &#124; Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved`。 | — |
| <a id="api-e1e82e7b3fb41cc4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal = 524288`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3373) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-e2160773f01ecae6"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved = 1048576`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3374) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-a82e77236bb3e5d0"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCubeArray = 2097152`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3375) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-bb72b041fd9e2557"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureDirectAccess = 4194304`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3376) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-73b506e4f66be42b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternal = 8388608`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3377) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-9b7136d5e848189b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternalShared = 16777216`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3378) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-9446f753d8219dc7"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureReadBack = 33554432`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3379) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-2f3aea4aec88d6d9"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TransparentBackbuffer = 268435456`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3382) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-5de1c36a13800854"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VariableRateShading = 536870912`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3383) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-bcf276f753113ded"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribHalf = 1073741824`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3384) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-e8e49bb12536d63e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribUint10 = 2147483648`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3385) | 当前常量或枚举值：`2147483648`。 | — |
| <a id="api-cf2e01443711a55b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexId = 4294967296`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3386) | 当前常量或枚举值：`4294967296`。 | — |
| <a id="api-1c2c500bda81fff4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ViewportLayerArray = 8589934592`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3387) | 当前常量或枚举值：`8589934592`。 | — |
| <a id="api-baf9c42ad60713b6"></a>[`public enum Inno.Native.Bgfx.bgfx.CapsFlags : ulong`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3352) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CapsFormatFlags</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CapsFormatFlags 6917b6bc37f1acfa26b4ac98dc0a8f970ee0c8798cb9665fadedf2fab7b338e6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureNone 943de3cfd95e6776716ac500920e62541594d57f06aa87233471cb6eb2a9f101 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2d cffd9d761387f05d614e6acb1ebe324bb5c8ebab1b3ae537b8020ba4e489c977 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dSrgb 9fe91314d60e3777683f75e18236033364ef33c2d7b30fe0d5d3b47e5d2ad325 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dEmulated 8df86ad28b20f9213e33a2f57baeafb1465755e96cfb4928cb1a680c1ed43751 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3d 730ca36bac2a11df5528c807c76a436d354a54297e4b286d01f9a40d076c5204 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dSrgb 0e68ccbaf5d7d5e3fa24efc3e59e764306294fea7fc2633a6863f5d37acdff90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dEmulated 1df207759c46821ce54a7515369096f45840a2461592f578dc641af00c8ab5f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCube 5cda4d661598c624a7af77ba0160bd7cb8790affdfca01ede7bc0af64c186c27 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeSrgb 2f5a7fb5f194c7dfe7c10d32e6924b70c68b315167e6774e547f8c08a8659c52 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeEmulated 7b0bab205b7714af07b836f2a2bde9c119d251c8520c0b182bbd04fc91d4d613 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureVertex f113ef0b63e6d7472639bf9919dd5652e0ce935583619a732bef9b763562814c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageRead 499f596113e81b340bc8a0ba4a8ee11ff79acceacd377db2c79f5afb46f5471f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageWrite 7640fee5b478ce82d640fcd696080c3c4bf5bba335defa6b7f4ab6987ef41f15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebuffer eb264d5339eba152a40fbbb2885da1d09ef89cdd2f0325a4302eecf204ce535d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebufferMsaa e99f7e9b275948c1540aa348cc67faf253a4acc86fe33a3d0d687a07bb391d14 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMsaa 6de39e129f266a00c8a120b39bcbc943867e636318af9a853e9ad1f3336bfcdc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMipAutogen 5990eced6850ec4ad709037e7aff731278a4a0c1ed5c4a709e7e0ac1bac99f07 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureBackbuffer 0f9c6c3c2a2bf8d63072640e71df3f0789913f34b09c5d7679382351b14e6f92 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3da49be5c5a306b2"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2d = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3401) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f772621033f14a12"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dEmulated = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3403) | 当前常量或枚举值：`4`。 | — |
| <a id="api-4e6165e90c161f2b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dSrgb = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3402) | 当前常量或枚举值：`2`。 | — |
| <a id="api-1ed19df705f4336e"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3d = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3404) | 当前常量或枚举值：`8`。 | — |
| <a id="api-15194169bbdfc439"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dEmulated = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3406) | 当前常量或枚举值：`32`。 | — |
| <a id="api-9e3e338aa0f2ae46"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dSrgb = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3405) | 当前常量或枚举值：`16`。 | — |
| <a id="api-931a471c01b6dc6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureBackbuffer = 65536`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3417) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-75eb501f1d6d17be"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCube = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3407) | 当前常量或枚举值：`64`。 | — |
| <a id="api-0ebbbb76dfced243"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeEmulated = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3409) | 当前常量或枚举值：`256`。 | — |
| <a id="api-827599bfc1d2077f"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeSrgb = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3408) | 当前常量或枚举值：`128`。 | — |
| <a id="api-6c2b24998e3199ba"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebuffer = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3413) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-1bf8e8ce118c408a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebufferMsaa = 8192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3414) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-b2f30408ed6c8167"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageRead = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3411) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-e4f05f9771829f0c"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageWrite = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3412) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-68b0c661a4685a34"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMipAutogen = 32768`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3416) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-05cbfd2ae7bd783b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMsaa = 16384`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3415) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-ba57c1f03f49bedd"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureNone = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3400) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0551948e143f5905"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureVertex = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3410) | 当前常量或枚举值：`512`。 | — |
| <a id="api-55d759994eddb6a1"></a>[`public enum Inno.Native.Bgfx.bgfx.CapsFormatFlags : uint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3398) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CapsGpu</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CapsGpu 9d9aec56d8519ee8a7195c76a30ded3c99730b612d87b49c1c0ff69946a46199 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.vendorId ea3d2264ea5a649a38b287fc3cbaa2f2fdff873e4915e717b23a9f156b8d0a4e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.deviceId 210383e7159d5179187639ce8f105bf03f02937a684507925f33cfcd2e25bedd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CapsGpu.#ctor(System.UInt16,System.UInt16) c6b776360fd09fe9801dce3f29a910bd123a47d356be5445f321dbb3b1f95e67 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5837532d8759bdaa"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.deviceId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-969d9f770cf15076"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.vendorId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4eaf49f0cac58f3c"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu.CapsGpu(ushort vendorId = 0, ushort deviceId = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L980) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a6aa1f9f1582c7df"></a>[`public struct Inno.Native.Bgfx.bgfx.CapsGpu`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L976) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
